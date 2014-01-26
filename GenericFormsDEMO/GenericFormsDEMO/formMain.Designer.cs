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
            this.SuspendLayout();
            // 
            // buttPrefs
            // 
            this.buttPrefs.Location = new System.Drawing.Point(47, 45);
            this.buttPrefs.Name = "buttPrefs";
            this.buttPrefs.Size = new System.Drawing.Size(176, 47);
            this.buttPrefs.TabIndex = 0;
            this.buttPrefs.Text = "Show Preferences\r\n(Example: Rectangle)";
            this.buttPrefs.UseVisualStyleBackColor = true;
            this.buttPrefs.Click += new System.EventHandler(this.buttPrefs_Click);
            // 
            // lblCurVer
            // 
            this.lblCurVer.AutoSize = true;
            this.lblCurVer.Location = new System.Drawing.Point(86, 152);
            this.lblCurVer.Name = "lblCurVer";
            this.lblCurVer.Size = new System.Drawing.Size(99, 13);
            this.lblCurVer.TabIndex = 1;
            this.lblCurVer.Text = "Current version: 1.0";
            // 
            // buttUpdate
            // 
            this.buttUpdate.Location = new System.Drawing.Point(47, 168);
            this.buttUpdate.Name = "buttUpdate";
            this.buttUpdate.Size = new System.Drawing.Size(176, 47);
            this.buttUpdate.TabIndex = 2;
            this.buttUpdate.Text = "Update";
            this.buttUpdate.UseVisualStyleBackColor = true;
            this.buttUpdate.Click += new System.EventHandler(this.buttUpdate_Click);
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 285);
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
    }
}

