namespace GenericFormsDEMO
{
    partial class formMain
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
            this.buttPrefs = new System.Windows.Forms.Button();
            this.lblCurVer = new System.Windows.Forms.Label();
            this.buttUpdate = new System.Windows.Forms.Button();
            this.chkAutoDownload = new System.Windows.Forms.CheckBox();
            this.chkAutoInstall = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttPrefs
            // 
            this.buttPrefs.Location = new System.Drawing.Point(32, 35);
            this.buttPrefs.Name = "buttPrefs";
            this.buttPrefs.Size = new System.Drawing.Size(174, 47);
            this.buttPrefs.TabIndex = 0;
            this.buttPrefs.Text = "Show Preferences\r\n(Example: Rectangle)";
            this.buttPrefs.UseVisualStyleBackColor = true;
            this.buttPrefs.Click += new System.EventHandler(this.buttPrefs_Click);
            // 
            // lblCurVer
            // 
            this.lblCurVer.AutoSize = true;
            this.lblCurVer.Location = new System.Drawing.Point(70, 277);
            this.lblCurVer.Name = "lblCurVer";
            this.lblCurVer.Size = new System.Drawing.Size(99, 13);
            this.lblCurVer.TabIndex = 1;
            this.lblCurVer.Text = "Current version: 1.0";
            // 
            // buttUpdate
            // 
            this.buttUpdate.Location = new System.Drawing.Point(32, 227);
            this.buttUpdate.Name = "buttUpdate";
            this.buttUpdate.Size = new System.Drawing.Size(174, 47);
            this.buttUpdate.TabIndex = 2;
            this.buttUpdate.Text = "Update";
            this.buttUpdate.UseVisualStyleBackColor = true;
            this.buttUpdate.Click += new System.EventHandler(this.buttUpdate_Click);
            // 
            // chkAutoDownload
            // 
            this.chkAutoDownload.AutoSize = true;
            this.chkAutoDownload.Location = new System.Drawing.Point(48, 181);
            this.chkAutoDownload.Name = "chkAutoDownload";
            this.chkAutoDownload.Size = new System.Drawing.Size(142, 17);
            this.chkAutoDownload.TabIndex = 3;
            this.chkAutoDownload.Text = "Download wihout asking";
            this.chkAutoDownload.UseVisualStyleBackColor = true;
            // 
            // chkAutoInstall
            // 
            this.chkAutoInstall.AutoSize = true;
            this.chkAutoInstall.Location = new System.Drawing.Point(57, 204);
            this.chkAutoInstall.Name = "chkAutoInstall";
            this.chkAutoInstall.Size = new System.Drawing.Size(124, 17);
            this.chkAutoInstall.TabIndex = 3;
            this.chkAutoInstall.Text = "Install without asking";
            this.chkAutoInstall.UseVisualStyleBackColor = true;
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 326);
            this.Controls.Add(this.chkAutoInstall);
            this.Controls.Add(this.chkAutoDownload);
            this.Controls.Add(this.buttUpdate);
            this.Controls.Add(this.lblCurVer);
            this.Controls.Add(this.buttPrefs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "formMain";
            this.Text = "GenericForms DEMO";
            this.Load += new System.EventHandler(this.formMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttPrefs;
        private System.Windows.Forms.Label lblCurVer;
        private System.Windows.Forms.Button buttUpdate;
        private System.Windows.Forms.CheckBox chkAutoDownload;
        private System.Windows.Forms.CheckBox chkAutoInstall;
    }
}

