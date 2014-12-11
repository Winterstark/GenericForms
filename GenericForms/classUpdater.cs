using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace GenericForms
{
    public class Updater
    {
        private const string UPD_INST = "https://docs.google.com/uc?export=download&id=0B1jvFQ35nZDBZm5FSktmbEM4UTg";

        private static DateTime lastCheck;
        private static string updateURL;
        private static int permissionLevel, daysSinceLastSuccess;
        private static bool showChangelog;


        public static void Update(double currVersion, string defaultUpdateURL)
        {
            Update(currVersion, defaultUpdateURL, false);
        }

        public static void Update(double currVersion, string defaultUpdateURL, bool forceCheck)
        {
            //load update options
            if (File.Exists(Application.StartupPath + "\\updateConfig.txt"))
            {
                StreamReader fRdr = new StreamReader(Application.StartupPath + "\\updateConfig.txt");
                updateURL = fRdr.ReadLine();
                lastCheck = DateTime.Parse(fRdr.ReadLine());
                daysSinceLastSuccess = int.Parse(fRdr.ReadLine());
                permissionLevel = int.Parse(fRdr.ReadLine());
                showChangelog = bool.Parse(fRdr.ReadLine());
                fRdr.Close();
            }
            else
            {
                updateURL = defaultUpdateURL;
                lastCheck = DateTime.Now.AddDays(-2);
                daysSinceLastSuccess = 0;
                permissionLevel = 0;
                showChangelog = true;
            }

            //skip if already checked for update today
            if (!forceCheck && DateTime.Now.Subtract(lastCheck).TotalDays < 1)
            {
                SaveConfig(true, true, "");
                return;
            }

            //set askPermission flags
            bool[] askPermissions = {true, true, true};
            for (int i = 0; i < Math.Min(3, permissionLevel); i++)
                askPermissions[i] = false;

            //check for update
            if (!forceCheck && askPermissions[0] && MessageBox.Show("Check for updates?", "Updater", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                SaveConfig(true, true, "");
                return;
            }

            string pg = dlPage(updateURL);
            if (pg == "not_found")
            {
                SaveConfig(false, false, "Can't download update file.");
                return;
            }
            else if (pg == "no update")
            {
                SaveConfig(false, true, "");
                return;
            }

            //parse downloaded page
            string update = getUpdateInformation(ref pg);
            
            Dictionary<string, string> updatedFiles = new Dictionary<string, string>();
            string changelog = "";
            double newVersion = -1;

            if (update == "")
            {
                SaveConfig(false, false, "Error while processing update file.");
                return;
            }

            while (update != "")
            {
                newVersion = double.Parse(getTaggedData(ref update, "VERSION").Replace('.', ','));

                if (newVersion > currVersion) //only get information from newer versions than current
                {
                    //add changelog information
                    changelog += (changelog != "" ? Environment.NewLine : "") + Environment.NewLine + "v" + newVersion.ToString().Replace(',', '.') + " changelog::" + Environment.NewLine + getTaggedData(ref update, "CHANGELOG");

                    //parse links to updated files
                    string link = getTaggedData(ref update, "FILE");

                    while (link != "")
                    {
                        if (link.Contains("->"))
                        {
                            string dlURL = link.Substring(0, link.IndexOf("->"));
                            string dest = link.Substring(link.IndexOf("->") + 2);

                            if (updatedFiles.ContainsValue(dest)) //remove existing updatedFiles (from previous versions)
                                for (int i = 0; i < updatedFiles.Count; i++)
                                    if (updatedFiles.ElementAt(i).Value == dest)
                                    {
                                        updatedFiles.Remove(updatedFiles.ElementAt(i).Key);
                                        break;
                                    }

                            updatedFiles.Add(dlURL, dest);
                        }
                        else
                        {
                            SaveConfig(false, false, "Corrupted update file.");
                            return;
                        }

                        link = getTaggedData(ref update, "FILE");
                    }
                }

                update = getUpdateInformation(ref pg);
            }

            if (newVersion == -1 || updatedFiles.Count == 0)
            {
                SaveConfig(false, false, "Error while processing update information.");
                return;
            }

            //download update
            if (askPermissions[1] && MessageBox.Show("Download version v_" + newVersion.ToString().Replace(',', '.') + "?" + Environment.NewLine + changelog, "New Update Available", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                SaveConfig(false, true, "");
                return;
            }

            string tempDir = Application.StartupPath + "\\new_update_temp";
            while (Directory.Exists(tempDir))
                tempDir += "_1";
            Directory.CreateDirectory(tempDir);

            foreach (var updFile in updatedFiles)
            {
                string dlURL = updFile.Key;
                string dest = updFile.Value.Replace("root", tempDir); ;

                //ensure dest dir exists
                string destDir = dest.Substring(0, dest.LastIndexOf('\\'));
                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);

                //dl file
                if (!dlFile(dlURL, dest))
                {
                    SaveConfig(false, false, "Error while downloading updated files.");
                    return;
                }
            }

            //install update
            if (askPermissions[2] && MessageBox.Show("Install update?" + Environment.NewLine + changelog, "Downloaded New Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                SaveConfig(false, true, "");
                return;
            }

            if (File.Exists("updatesuccess.txt"))
                File.Delete("updatesuccess.txt");
            if (showChangelog)
            {
                StreamWriter file = new StreamWriter("updatesuccess.txt");
                file.WriteLine("Version " + newVersion.ToString().Replace(',', '.') + " installed" + Environment.NewLine + changelog);
                file.Close();
            }

            if (!File.Exists("UpdInst.exe"))
                if (!dlFile(UPD_INST, "UpdInst.exe"))
                {
                    SaveConfig(false, false, "Can't download UpdInst.exe.");
                    return;
                }

            ProcessStartInfo instInfo = new ProcessStartInfo();
            instInfo.FileName = "UpdInst.exe";
            instInfo.Arguments = '"' + tempDir + "\" \"" + Application.ExecutablePath + '"';
            instInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(instInfo);

            SaveConfig(false, true, "");
        }

        private static void SaveConfig(bool skippedBeforeChecking, bool success, string error)
        {
            if (!skippedBeforeChecking)
            {
                if (!success)
                {
                    daysSinceLastSuccess++;
                    if (daysSinceLastSuccess == 7)
                    {
                        string progName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
                        MessageBox.Show(progName + " hasn't been able to check for updates this week." + Environment.NewLine + Environment.NewLine + "This usually means that the update infrastructure is broken and you need to download the next update manually by visiting this program's homepage." + Environment.NewLine + Environment.NewLine + "Error message: " + error, "Update check failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        daysSinceLastSuccess = 0;
                    }
                }
                else
                    daysSinceLastSuccess = 0;
            }

            StreamWriter fWrtr = new StreamWriter(Application.StartupPath + "\\updateConfig.txt");
            fWrtr.WriteLine(updateURL);
            fWrtr.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            fWrtr.WriteLine(daysSinceLastSuccess);
            fWrtr.WriteLine(permissionLevel);
            fWrtr.WriteLine(showChangelog);
            fWrtr.Close();
        }

        private static string dlPage(string url)
        {
            string page;

            try
            {
                WebClient web = new WebClient();
                web.Headers.Add("user-agent", "c#");
                web.Headers[HttpRequestHeader.AcceptLanguage] = "en";

                page = web.DownloadString(url);
            }
            catch
            {
                page = "not_found";
            }

            return page;
        }

        private static bool dlFile(string fileURL, string savePath)
        {
            try
            {
                WebClient web = new WebClient();
                web.Headers.Add("user-agent", "c#");
                web.Headers[HttpRequestHeader.AcceptLanguage] = "en";

                web.DownloadFile(fileURL, savePath);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static string getUpdateInformation(ref string pg)
        {
            return getTaggedData(ref pg, "UPDATE").Replace("&gt;", ">").Replace("<p class=\"c0\">", "").Replace("<span>", "").Replace("</span>", "").Replace("</p>", Environment.NewLine).Replace("</div><div>", Environment.NewLine).Replace("<br />", "");
        }

        private static string getTaggedData(ref string page, string tag)
        {
            int lb = page.IndexOf("[" + tag + "]");
            if (lb == -1)
                return "";

            int ub = page.IndexOf("[/" + tag + "]", lb);
            if (ub == -1)
                return "";
            else
                ub += tag.Length + 3;

            string data = page.Substring(lb, ub - lb).Replace("[" + tag + "]", "").Replace("[/" + tag + "]", "");
            page = page.Remove(lb, ub - lb);

            return data;
        }
    }
}
