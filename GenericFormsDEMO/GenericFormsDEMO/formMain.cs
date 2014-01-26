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
            Updater.Update(1.0, "https://docs.google.com/document/pub?id=12XxGDFe_dEF8XawWLhQPIeMwOKBf-uI6C1dnW8MO9vA", new bool[3] { true, true, true }, true);
        }
    }
}
