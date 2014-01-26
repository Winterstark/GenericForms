using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenericForms;
using System.IO;

namespace GenericFormsDEMO
{
    public partial class formMain : Form
    {
        public formMain()
        {
            InitializeComponent();
        }

        private void buttPrefs_Click(object sender, EventArgs e)
        {
            Preferences prefs = new Preferences(Application.StartupPath + "\\prefs_config.txt", Application.StartupPath + "\\prefs.txt");
            prefs.Show();
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            Tutorial tutorial = new Tutorial(Application.StartupPath + "\\tutorial.txt", this);
        }

        private void buttUpdate_Click(object sender, EventArgs e)
        {
            //check if previous update check was too recent
            string path = Application.StartupPath + "\\lastUpdateCheck.txt";

            if (File.Exists(path))
            {
                StreamReader file = new StreamReader(path);
                if (DateTime.Now.Subtract(DateTime.Parse(file.ReadLine())).TotalDays < 1)
                    MessageBox.Show("Update will not succeed because another update took place less than a day ago." + Environment.NewLine + "If you don't want to wait that long delete \"lastUpdateCheck.txt\".");
                file.Close();
            }

            Updater.Update(1.0, "https://raw2.github.com/Winterstark/GenericForms/master/GenericFormsDEMO/Update/update.txt", new bool[3] { false, !chkAutoDownload.Checked, !chkAutoInstall.Checked }, true);
        }
    }
}
