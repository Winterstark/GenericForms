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


        public static void Update(double currVersion, string updateURL, bool[] askPermissions, bool showChangelog)
        {
            //ensure enough time passed since last check
            if (File.Exists(Application.StartupPath + "\\lastUpdateCheck.txt"))
            {
                StreamReader fRdr = new StreamReader(Application.StartupPath + "\\lastUpdateCheck.txt");
                DateTime lastCheck = DateTime.Parse(fRdr.ReadLine());
                fRdr.Close();

                if (DateTime.Now.Subtract(lastCheck).TotalDays < 1)
                    return;
            }

            StreamWriter fWrtr = new StreamWriter(Application.StartupPath + "\\lastUpdateCheck.txt");
            fWrtr.WriteLine(DateTime.Now);
            fWrtr.Close();

            //check for update
            if (askPermissions[0] && MessageBox.Show("Check for updates?", "Updater", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            string pg = dlPage(updateURL);
            if (pg == "not_found")
            {
                MessageBox.Show("Update check failed.");
                return;
            }

            //parse downloaded page
            pg = getTaggedData(ref pg, "UPDATE").Replace("&gt;", ">").Replace("<p class=\"c0\">", "").Replace("<span>", "").Replace("</span>", "").Replace("</p>", Environment.NewLine).Replace("</div><div>", Environment.NewLine).Replace("<br />", "");
            double newVersion = double.Parse(getTaggedData(ref pg, "VERSION").Replace('.', ','));
            string changelog = getTaggedData(ref pg, "CHANGELOG");

            List<string> links = new List<string>();
            string link = getTaggedData(ref pg, "FILE");

            while (link != "")
            {
                links.Add(link);
                link = getTaggedData(ref pg, "FILE");
            }
            
            if (newVersion <= currVersion)
                return;
            
            //download update
            if (askPermissions[1] && MessageBox.Show("Download version v_" + newVersion.ToString().Replace(',', '.') + "?" + Environment.NewLine + changelog, "New Update Available", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            string tempDir = Application.StartupPath + "\\new_update_temp";
            while (Directory.Exists(tempDir))
                tempDir += "_1";
            Directory.CreateDirectory(tempDir);
            
            string dlURL, dest;
            foreach (string l in links)
            {
                if (l.Contains("->"))
                {
                    dlURL = l.Substring(0, l.IndexOf("->"));
                    dest = l.Substring(l.IndexOf("->") + 2).Replace("root", tempDir);
                }
                else
                {
                    MessageBox.Show("Update download failed.");
                    return;
                }

                //ensure dest dir exists
                string destDir = dest.Substring(0, dest.LastIndexOf('\\'));
                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);

                //dl file
                if (!dlFile(dlURL, dest))
                {
                    MessageBox.Show("Update download failed.");
                    MessageBox.Show(dlURL);
                    MessageBox.Show(dest);
                    return;
                }
            }

            //install update
            if (askPermissions[2] && MessageBox.Show("Install update?" + Environment.NewLine + changelog, "Downloaded New Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

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
                    MessageBox.Show("Update download failed. Missing UpdInst.exe.");
                    return;
                }

            ProcessStartInfo instInfo = new ProcessStartInfo();
            instInfo.FileName = "UpdInst.exe";
            instInfo.Arguments = '"' + tempDir + "\" \"" + Application.ExecutablePath + '"';
            instInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(instInfo);

            Application.Exit();
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
