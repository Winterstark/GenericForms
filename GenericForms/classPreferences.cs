using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace GenericForms
{
    public class Preferences
    {
        private enum PrefType { String, Int, Bool };

        private Form window = new Form();
        private TabControl tabs = null;

        private Dictionary<string, PrefType> types = new Dictionary<string, PrefType>();
        private string savePath;


        public Preferences(string configPath, string savePath)
        {
            this.savePath = savePath;

            window.Text = "Preferences";
            window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            window.MaximizeBox = false;

            Control.ControlCollection controls = window.Controls;
            
            //read file and construct window
            string[] lines = Misc.ReadLines(configPath);
            string option, type, args;
            int y = 1;
            Label lbl;

            for (int i = 0; i < lines.Length; i++)
                if (lines[i][0] == '[')
                {
                    //add tab
                    if (tabs == null)
                    {
                        tabs = new TabControl();
                        tabs.Left = 24;
                        tabs.Top = 28;

                        window.Controls.Add(tabs);
                    }

                    tabs.TabPages.Add(lines[i].Substring(1, lines[i].Length - 2));
                    controls = tabs.TabPages[tabs.TabPages.Count - 1].Controls;
                    y = 1;
                }
                else if (lines[i].Contains('='))
                {
                    //add control to window
                    option = Misc.takePrefix(ref lines[i]);
                    type = Misc.takePrefix(ref lines[i]);

                    //check for arguments
                    if (type.Contains('('))
                    {
                        args = type.Substring(type.IndexOf('(') + 1, type.Length - type.IndexOf('(') - 2);
                        type = type.Substring(0, type.IndexOf('('));
                    }
                    else
                        args = "";
                    
                    switch (type)
                    {
                        case "textbox":
                            lbl = createLabel(Misc.takePrefix(ref lines[i]), 24, 28 * y++ + 3);
                            TextBox txtbox = new TextBox();

                            //MessageBox.Show(lines[i]);
                            controls.Add(txtbox);
                            controls.Add(lbl);

                            txtbox.Left = lbl.Left + lbl.Width + 6;
                            txtbox.Top = lbl.Top - 3;
                            txtbox.Tag = option;
                            txtbox.Text = lines[i];

                            types.Add(option, PrefType.String);
                            break;
                        case "numbox":
                            lbl = createLabel(Misc.takePrefix(ref lines[i]), 24, 28 * y++ + 3);
                            NumericUpDown numbox = new NumericUpDown();

                            controls.Add(numbox);
                            controls.Add(lbl);

                            numbox.Left = lbl.Left + lbl.Width + 6;
                            numbox.Top = lbl.Top - 3;
                            numbox.Tag = option;

                            if (args != "")
                            {
                                string[] bounds = args.Split(new string[] { "--" }, StringSplitOptions.None);

                                numbox.Minimum = int.Parse(bounds[0]);
                                numbox.Maximum = int.Parse(bounds[1]);
                            }

                            if (lines[i] != "")
                                numbox.Value = int.Parse(lines[i]);

                            types.Add(option, PrefType.Int);
                            break;
                        case "checkbox":
                            CheckBox chkbox = new CheckBox();

                            controls.Add(chkbox);   

                            chkbox.Left = 24;
                            chkbox.Top = 28 * y++ + 3;
                            chkbox.AutoSize = true;
                            chkbox.Tag = option;
                            chkbox.Text = Misc.takePrefix(ref lines[i]);

                            if (lines[i] != "")
                                chkbox.Checked = bool.Parse(lines[i]);

                            types.Add(option, PrefType.Bool);
                            break;
                        case "combobox":
                            lbl = createLabel(Misc.takePrefix(ref lines[i]), 24, 28 * y++ + 3);
                            ComboBox cmbbox = new ComboBox();

                            controls.Add(cmbbox);
                            controls.Add(lbl);

                            cmbbox.Left = lbl.Left + lbl.Width + 6;
                            cmbbox.Top = lbl.Top - 3;
                            cmbbox.AutoSize = true;
                            cmbbox.Tag = option;

                            if (lines[i] != "")
                            {
                                cmbbox.Items.AddRange(Misc.takePrefix(ref lines[i]).Split('/'));

                                if (lines[i] != "")
                                    cmbbox.Text = lines[i];
                            }

                            if (args == "list")
                                cmbbox.DropDownStyle = ComboBoxStyle.DropDownList;

                            types.Add(option, PrefType.String);
                            break;
                        case "radiobuttons":
                            controls.Add(createLabel(Misc.takePrefix(ref lines[i]), 24, 28 * y++ + 3));

                            Panel panel = new Panel();
                            controls.Add(panel);

                            panel.Left = 24;
                            panel.Top = 28 * y++ + 3;
                            panel.Tag = option;

                            if (lines[i] != "")
                            {
                                string chkedItem = "";
                                if (lines[i].Contains('=') || lines[i].Contains(':'))
                                {
                                    string temp = Misc.takePrefix(ref lines[i]);
                                    chkedItem = lines[i];
                                    lines[i] = temp;
                                }

                                RadioButton rdbutt;
                                int panW = 0, panH = 0;

                                if (args == "vert")
                                    foreach (string item in Misc.takePrefix(ref lines[i]).Split('/'))
                                    {
                                        rdbutt = createRadioButton(item, 0, panH, item == chkedItem);
                                        panel.Controls.Add(rdbutt);

                                        if (rdbutt.Width > panW)
                                            panW = rdbutt.Width;
                                        panH = rdbutt.Top + rdbutt.Height;
                                    }
                                else
                                    foreach (string item in Misc.takePrefix(ref lines[i]).Split('/'))
                                    {
                                        rdbutt = createRadioButton(item, panW, 0, item == chkedItem);
                                        panel.Controls.Add(rdbutt);

                                        panW = rdbutt.Left + rdbutt.Width;
                                        if (rdbutt.Height > panH)
                                            panH = rdbutt.Height;
                                    }

                                panel.Width = panW;
                                panel.Height = panH;
                            }

                            types.Add(option, PrefType.String);
                            break;
                    }
                }

            //autosize tabcontrol
            int w, h;
            
            if (tabs != null)
            {
                int maxW = 0, maxH = 0;

                foreach (TabPage tab in tabs.TabPages)
                {
                    getSize(tab.Controls, out w, out h);

                    if (w > maxW)
                        maxW = w;
                    if (h > maxH)
                        maxH = h;
                }

                tabs.Width = maxW + 16;
                tabs.Height = 38 + maxH;
            }

            //autosize window
            getSize(window.Controls, out w, out h);

            window.Width = w + 16;
            window.Height = 38 + h;
            
            //add save button
            Button buttSave = new Button();

            buttSave.Left = window.Width - 129;
            buttSave.Top = h - 16;
            buttSave.Width = 100;
            buttSave.Text = "Save";
            buttSave.Click += new EventHandler(buttSave_Click);
            
            window.Controls.Add(buttSave);
            window.Height += buttSave.Height - 16;
        }

        public void Show(FormClosedEventHandler endReport)
        {
            window.FormClosed += endReport;

            Show();
        }

        public void Show()
        {
            if (File.Exists(savePath))
            {
                Dictionary<string, object> prefs = Load();

                if (tabs == null)
                    assignValues(window.Controls, prefs);
                else
                    foreach (TabPage tab in tabs.TabPages)
                        assignValues(tab.Controls, prefs);
            }

            window.Show();
        }

        public Dictionary<string, object> Load()
        {
            Dictionary<string, object> prefs = new Dictionary<string, object>();
            StreamReader file = new StreamReader(savePath);

            foreach (string line in file.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] setting = line.Split('=');

                switch (types[setting[0]])
                {
                    case PrefType.String:
                        prefs.Add(setting[0], setting[1]);
                        break;
                    case PrefType.Int:
                        prefs.Add(setting[0], int.Parse(setting[1]));
                        break;
                    case PrefType.Bool:
                        prefs.Add(setting[0], bool.Parse(setting[1]));
                        break;
                }
            }

            file.Close();
            return prefs;
        }

        private void assignValues(Control.ControlCollection controls, Dictionary<string,object> prefs)
        {
            foreach (Control c in controls)
                if (c is Panel)
                    foreach (RadioButton rdbutt in c.Controls.OfType<RadioButton>())
                    {
                        if (rdbutt.Text == prefs[c.Tag.ToString()].ToString())
                        {
                            rdbutt.Checked = true;
                            break;
                        }
                    }
                else if (c is TextBox)
                    c.Text = prefs[c.Tag.ToString()].ToString();
                else if (c is ComboBox)
                    c.Text = prefs[c.Tag.ToString()].ToString();
                else if (c is NumericUpDown)
                    ((NumericUpDown)c).Value = (int)prefs[c.Tag.ToString()];
                else if (c is CheckBox)
                    ((CheckBox)c).Checked = (bool)prefs[c.Tag.ToString()];
        }

        private void buttSave_Click(object sender, EventArgs e)
        {
            StreamWriter file = new StreamWriter(savePath);

            if (tabs == null)
                foreach (Control c in tabs.Controls)
                    saveSetting(file, c);
            else
                foreach (TabPage tab in tabs.TabPages)
                    foreach (Control c in tab.Controls)
                        saveSetting(file, c);

            file.Close();
            window.Close();
        }

        private void saveSetting(StreamWriter file, Control c)
        {
            if (c is Label)
                return;

            string value = "";

            if (c is TextBox)
                value = c.Text;
            else if (c is NumericUpDown)
                value = c.Text;
            else if (c is CheckBox)
                value = ((CheckBox)c).Checked.ToString();
            else if (c is ComboBox)
                value = c.Text;
            else if (c is Panel)
                foreach (RadioButton rdbutt in c.Controls.OfType<RadioButton>())
                    if (rdbutt.Checked)
                    {
                        value = rdbutt.Text;
                        break;
                    }

            file.WriteLine(c.Tag + "=" + value);
        }

        private Label createLabel(string text, int x, int y)
        {
            Label lbl = new Label();

            lbl.Left = x;
            lbl.Top = y;
            lbl.AutoSize = true;
            lbl.Text = text;

            return lbl;
        }

        private RadioButton createRadioButton(string text, int x, int y, bool chk)
        {
            RadioButton rdbutt = new RadioButton();

            rdbutt.Left = x;
            rdbutt.Top = y;
            rdbutt.AutoSize = true;
            rdbutt.Text = text;
            rdbutt.Checked = chk;

            return rdbutt;
        }

        private void getSize(Control.ControlCollection controls, out int w, out int h)
        {
            w = 0;
            h = 0;

            foreach (Control c in controls)
            {
                if (c.Left + c.Width + 12 > w)
                    w = c.Left + c.Width + 12;
                if (c.Top + c.Height + 28 > h)
                    h = c.Top + c.Height + 28;
            }
        }
    }
}
