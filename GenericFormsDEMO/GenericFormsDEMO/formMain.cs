using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GenericForms;

namespace GenericFormsDEMO
{
    public partial class formMain : Form
    {
        const double VERSION = 1.0;
        const string UPDATE_URL = "https://raw.githubusercontent.com/Winterstark/GenericForms/master/GenericFormsDEMO/Update/update.txt";

        UpdateConfig updateConfig;


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
            string path = Application.StartupPath + "\\updateConfig.txt";

            if (File.Exists(path))
            {
                StreamReader file = new StreamReader(path);
                string line = file.ReadLine(); //skip url
                line = file.ReadLine(); //read datetime of last check

                if (DateTime.Now.Subtract(DateTime.Parse(line)).TotalDays < 1)
                    MessageBox.Show("Update will not succeed because another update took place less than a day ago." + Environment.NewLine + "If you don't want to wait that long delete \"updateConfig.txt\".");
                file.Close();
            }
            
            Updater.Update(VERSION, UPDATE_URL);
        }

        private void buttUpdateConfig_Click(object sender, EventArgs e)
        {
            if (updateConfig == null || updateConfig.IsDisposed)
            {
                updateConfig = new UpdateConfig();
                updateConfig.DefaultUpdateURL = UPDATE_URL;
                updateConfig.Show();
            }
        }
    }
}
