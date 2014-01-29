using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace UpdInst
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
                return;
            
            //wait until application closes
            FileInfo fInfo = new FileInfo(args[1]);

            while (fileLocked(fInfo))
                Thread.Sleep(2000);
            
            //move downloaded files to root dir
            moveDir(args[0], args[0].Substring(0, args[0].LastIndexOf('\\', args[0].Length - 2) + 1));

            //run updated program
            Process.Start(args[1]);
            
            //show success msg (if any)
            if (File.Exists("updatesuccess.txt"))
                Process.Start("updatesuccess.txt");

            //cleanup
            Directory.Delete(args[0], true);
        }

        static void moveDir(string srcDir, string rootDir)
        {
            //move files
            foreach (string file in Directory.GetFiles(srcDir))
            {
                string dest = rootDir + file.Substring(file.IndexOf('\\', rootDir.Length) + 1);

                //ensure dest dir exists
                string destDir = dest.Substring(0, dest.LastIndexOf('\\'));
                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);

                if (File.Exists(dest))
                    File.Delete(dest);

                File.Move(file, dest);
            }

            //move subdirs
            foreach (string subdir in Directory.GetDirectories(srcDir))
                moveDir(subdir, rootDir);
        }

        static bool fileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }
    }
}
