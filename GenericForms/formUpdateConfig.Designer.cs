namespace GenericForms
{
    partial class UpdateConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtUpdateURL = new System.Windows.Forms.TextBox();
            this.chkShowChangelog = new System.Windows.Forms.CheckBox();
            this.lblUpdateNotifications = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.trackUpdate = new System.Windows.Forms.TrackBar();
            this.buttSave = new System.Windows.Forms.Button();
            this.buttClose = new System.Windows.Forms.Button();
            this.lblLastCheck = new System.Windows.Forms.Label();
            this.buttCheckForUpdates = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackUpdate)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Check this URL for updates:";
            // 
            // txtUpdateURL
            // 
            this.txtUpdateURL.Location = new System.Drawing.Point(168, 36);
            this.txtUpdateURL.Name = "txtUpdateURL";
            this.txtUpdateURL.Size = new System.Drawing.Size(399, 20);
            this.txtUpdateURL.TabIndex = 1;
            this.txtUpdateURL.TextChanged += new System.EventHandler(this.txtUpdateURL_TextChanged);
            // 
            // chkShowChangelog
            // 
            this.chkShowChangelog.AutoSize = true;
            this.chkShowChangelog.Location = new System.Drawing.Point(168, 126);
            this.chkShowChangelog.Name = "chkShowChangelog";
            this.chkShowChangelog.Size = new System.Drawing.Size(166, 17);
            this.chkShowChangelog.TabIndex = 13;
            this.chkShowChangelog.Text = "Show changelog after update";
            this.chkShowChangelog.UseVisualStyleBackColor = true;
            this.chkShowChangelog.CheckedChanged += new System.EventHandler(this.chkShowChangelog_CheckedChanged);
            // 
            // lblUpdateNotifications
            // 
            this.lblUpdateNotifications.AutoSize = true;
            this.lblUpdateNotifications.Location = new System.Drawing.Point(362, 75);
            this.lblUpdateNotifications.Name = "lblUpdateNotifications";
            this.lblUpdateNotifications.Size = new System.Drawing.Size(60, 13);
            this.lblUpdateNotifications.TabIndex = 15;
            this.lblUpdateNotifications.Text = "Always ask";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(58, 75);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Update notifications:";
            // 
            // trackUpdate
            // 
            this.trackUpdate.BackColor = System.Drawing.SystemColors.Control;
            this.trackUpdate.Location = new System.Drawing.Point(168, 75);
            this.trackUpdate.Maximum = 3;
            this.trackUpdate.Name = "trackUpdate";
            this.trackUpdate.Size = new System.Drawing.Size(188, 45);
            this.trackUpdate.TabIndex = 12;
            this.trackUpdate.Scroll += new System.EventHandler(this.trackUpdate_Scroll);
            // 
            // buttSave
            // 
            this.buttSave.Enabled = false;
            this.buttSave.Location = new System.Drawing.Point(168, 250);
            this.buttSave.Name = "buttSave";
            this.buttSave.Size = new System.Drawing.Size(180, 36);
            this.buttSave.TabIndex = 16;
            this.buttSave.Text = "Save and Close";
            this.buttSave.UseVisualStyleBackColor = true;
            this.buttSave.Click += new System.EventHandler(this.buttSave_Click);
            // 
            // buttClose
            // 
            this.buttClose.Location = new System.Drawing.Point(365, 250);
            this.buttClose.Name = "buttClose";
            this.buttClose.Size = new System.Drawing.Size(180, 36);
            this.buttClose.TabIndex = 17;
            this.buttClose.Text = "Close";
            this.buttClose.UseVisualStyleBackColor = true;
            this.buttClose.Click += new System.EventHandler(this.buttClose_Click);
            // 
            // lblLastCheck
            // 
            this.lblLastCheck.AutoSize = true;
            this.lblLastCheck.Location = new System.Drawing.Point(165, 180);
            this.lblLastCheck.Name = "lblLastCheck";
            this.lblLastCheck.Size = new System.Drawing.Size(63, 13);
            this.lblLastCheck.TabIndex = 18;
            this.lblLastCheck.Text = "Last check:";
            // 
            // buttCheckForUpdates
            // 
            this.buttCheckForUpdates.Location = new System.Drawing.Point(168, 196);
            this.buttCheckForUpdates.Name = "buttCheckForUpdates";
            this.buttCheckForUpdates.Size = new System.Drawing.Size(180, 36);
            this.buttCheckForUpdates.TabIndex = 19;
            this.buttCheckForUpdates.Text = "Save and Check for Updates";
            this.buttCheckForUpdates.UseVisualStyleBackColor = true;
            this.buttCheckForUpdates.Click += new System.EventHandler(this.buttCheckForUpdates_Click);
            // 
            // UpdateConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 324);
            this.Controls.Add(this.buttCheckForUpdates);
            this.Controls.Add(this.lblLastCheck);
            this.Controls.Add(this.buttClose);
            this.Controls.Add(this.buttSave);
            this.Controls.Add(this.chkShowChangelog);
            this.Controls.Add(this.lblUpdateNotifications);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.trackUpdate);
            this.Controls.Add(this.txtUpdateURL);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "UpdateConfig";
            this.Text = "Update Configuration";
            this.Load += new System.EventHandler(this.UpdateConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackUpdate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUpdateURL;
        private System.Windows.Forms.CheckBox chkShowChangelog;
        private System.Windows.Forms.Label lblUpdateNotifications;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar trackUpdate;
        private System.Windows.Forms.Button buttSave;
        private System.Windows.Forms.Button buttClose;
        private System.Windows.Forms.Label lblLastCheck;
        private System.Windows.Forms.Button buttCheckForUpdates;
    }
}