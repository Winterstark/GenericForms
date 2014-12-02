using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GenericForms
{
    public partial class UpdateConfig : Form
    {
        DateTime lastCheck;
        string updateURL, configPath;
        int permissionLevel, daysSinceLastSuccess;
        bool showChangelog;


        void refreshUpdateNotifLabel()
        {
            switch (trackUpdate.Value)
            {
                case 0:
                    lblUpdateNotifications.Text = "Always ask";
                    break;
                case 1:
                    lblUpdateNotifications.Text = "Check for update automatically";
                    break;
                case 2:
                    lblUpdateNotifications.Text = "Download update automatically";
                    break;
                case 3:
                    lblUpdateNotifications.Text = "Install update automatically";
                    break;
            }
        }

        void checkForChanges()
        {
            buttSave.Enabled =
                txtUpdateURL.Text != updateURL ||
                trackUpdate.Value != permissionLevel ||
                chkShowChangelog.Checked != showChangelog;
        }


        public UpdateConfig()
        {
            InitializeComponent();
        }

        private void UpdateConfig_Load(object sender, EventArgs e)
        {
            configPath = Application.StartupPath + "\\updateConfig.txt";

            if (File.Exists(configPath))
            {
                StreamReader fRdr = new StreamReader(Application.StartupPath + "\\updateConfig.txt");
                updateURL = fRdr.ReadLine();
                lastCheck = DateTime.Parse(fRdr.ReadLine());
                daysSinceLastSuccess = int.Parse(fRdr.ReadLine());
                permissionLevel = int.Parse(fRdr.ReadLine());
                showChangelog = bool.Parse(fRdr.ReadLine());
                fRdr.Close();
            }

            txtUpdateURL.Text = updateURL;
            trackUpdate.Value = permissionLevel;
            refreshUpdateNotifLabel();
            chkShowChangelog.Checked = showChangelog;
        }

        private void txtUpdateURL_TextChanged(object sender, EventArgs e)
        {
            refreshUpdateNotifLabel();
            checkForChanges();
        }

        private void trackUpdate_Scroll(object sender, EventArgs e)
        {
            checkForChanges();
        }

        private void chkShowChangelog_CheckedChanged(object sender, EventArgs e)
        {
            checkForChanges();
        }

        private void buttSave_Click(object sender, EventArgs e)
        {
            StreamWriter fWrtr = new StreamWriter(Application.StartupPath + "\\updateConfig.txt");
            fWrtr.WriteLine(txtUpdateURL.Text);
            fWrtr.WriteLine(lastCheck.ToString("yyyy-MM-dd HH:mm"));
            fWrtr.WriteLine(daysSinceLastSuccess);
            fWrtr.WriteLine(trackUpdate.Value);
            fWrtr.WriteLine(chkShowChangelog.Checked);
            fWrtr.Close();

            this.Close();
        }

        private void buttCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
