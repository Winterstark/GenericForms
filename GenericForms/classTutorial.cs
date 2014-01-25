using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace GenericForms
{
    public class Tutorial
    {
        private class Step
        {
            private enum Selection { None, Circle, Rectangle };
            private enum Position { Left, Right, Top, Bottom };

            private Form targetWnd;
            private Control target;
            private string msg, adv;
            private Selection sel;
            private bool line, arrow;


            public Step(Form targetWnd, string desc)
            {
                this.targetWnd = targetWnd;

                desc = desc.Replace("\\n", Environment.NewLine);

                int delimit = desc.IndexOf('"', desc.IndexOf('"') + 1);
                msg = desc.Substring(1, delimit - 1);
                desc = desc.Substring(delimit + 1);

                if (desc[0] == '-')
                {
                    line = true;
                    desc = desc.Substring(1);
                }
                else
                    line = false;

                if (desc[0] == '>')
                {
                    arrow = true;
                    desc = desc.Substring(1);
                }
                else
                    arrow = false;

                switch (desc[0])
                {
                    case '(':
                        sel = Selection.Circle;
                        desc = desc.Substring(1);
                        break;
                    case '[':
                        sel = Selection.Rectangle;
                        desc = desc.Substring(1);
                        break;
                    default:
                        sel = Selection.None;
                        break;
                }

                delimit = desc.IndexOf('*');
                if (delimit != -1)
                {
                    adv = desc.Substring(delimit + 1);
                    desc = desc.Substring(0, delimit);
                }

                if (desc[desc.Length - 1] == ')' || desc[desc.Length - 1] == ']')
                    desc = desc.Substring(0, desc.Length - 1);

                if (desc == "this")
                    target = targetWnd;
                else
                {
                    target = targetWnd.Controls[desc];

                    if (target == null)
                        foreach (TabControl tabs in targetWnd.Controls.OfType<TabControl>())
                        {
                            foreach (TabPage tab in tabs.TabPages)
                                if (tab.Controls.ContainsKey(desc))
                                {
                                    target = tab.Controls[desc];
                                    break;
                                }

                            if (target != null)
                                break;
                        }
                }
            }

            public void Activate(Form popup)
            {
                Label lbl = (Label)popup.Controls["lbl"];
                lbl.Text = msg;
                
                //advanced button?
                Button buttAdv = (Button)popup.Controls["buttAdv"];

                if (adv != null)
                {
                    buttAdv.Visible = true;
                    buttAdv.Tag = adv;
                }
                else
                    buttAdv.Visible = false;
            }

            public void Draw(Form popup)
            {
                //determine popup position
                int max = targetWnd.Top + target.Top;
                
                Position pos = Position.Top;
                int resX = Screen.PrimaryScreen.Bounds.Width, resY = Screen.PrimaryScreen.Bounds.Height;

                if (resY - (targetWnd.Top + target.Top) > max)
                {
                    max = resY - (targetWnd.Top + target.Top);
                    pos = Position.Bottom;
                }

                if (targetWnd.Left + target.Left > max)
                {
                    max = targetWnd.Left + target.Left;
                    pos = Position.Left;
                }

                if (resX - (targetWnd.Left + target.Left) > max)
                {
                    max = resX - (targetWnd.Left + target.Left);
                    pos = Position.Right;
                }

                //position popup
                Label lbl = (Label)popup.Controls["lbl"];
                Button buttAdv = (Button)popup.Controls["buttAdv"];
                Panel butts = (Panel)popup.Controls["butts"];

                Rectangle back = new Rectangle();
                Point a = new Point(), coords = new Point();
                Size pointSize = getPointerSize(pos, lbl, buttAdv, butts);

                switch (pos)
                {
                    case Position.Top:
                        lbl.Left = 7;
                        lbl.Top = 5;
                        positionButtons(lbl, buttAdv, butts);

                        back = new Rectangle(new Point(2, 2), getBackSize(lbl, buttAdv, butts));
                        a = new Point(back.Left + back.Width / 2, back.Top + back.Height);

                        popup.Width = Math.Max(5 + lbl.Width + 5, 12 + target.Width + 12);
                        popup.Height = 7 + lbl.Height + 5 + (buttAdv.Visible ? buttAdv.Height + 5 : 0) + butts.Height + 7 + 150 + 7 + target.Height + 7;

                        coords = target.PointToScreen(new Point(target.Width / 2, 0));
                        popup.Location = checkBounds(popup, coords.X + target.Width / 2 + 12 - popup.Width, coords.Y + target.Height + 7 - popup.Height);
                        break;
                    case Position.Bottom:
                        lbl.Left = 7;
                        lbl.Top = pointSize.Height + 5;
                        positionButtons(lbl, buttAdv, butts);

                        back = new Rectangle(new Point(2, pointSize.Height - 2), getBackSize(lbl, buttAdv, butts));
                        a = new Point(back.Left + back.Width / 2, back.Top);

                        popup.Width = back.Width;
                        popup.Height = pointSize.Height + back.Height;
                        
                        coords = target.PointToScreen(new Point(target.Width / 2, target.Height));
                        popup.Location = checkBounds(popup, coords.X - popup.Width / 2, coords.Y - target.Height - 5);
                        break;
                    case Position.Left:
                        lbl.Left = 7;
                        lbl.Top = 5;
                        positionButtons(lbl, buttAdv, butts);

                        back = new Rectangle(new Point(0, 2), getBackSize(lbl, buttAdv, butts));
                        a = new Point(back.Left + back.Width, back.Top + back.Height / 2);

                        popup.Width = back.Width + pointSize.Width;
                        popup.Height = pointSize.Height;
                        
                        coords = target.PointToScreen(new Point(0, target.Height / 2));
                        popup.Location = checkBounds(popup, coords.X + target.Width + 12 - popup.Width, coords.Y - target.Height / 2 - 5);
                        break;
                    case Position.Right:
                        lbl.Left = pointSize.Width + 5;
                        lbl.Top = 5;
                        positionButtons(lbl, buttAdv, butts);

                        back = new Rectangle(new Point(pointSize.Width - 2, 2), getBackSize(lbl, buttAdv, butts));
                        a = new Point(back.Left, back.Top + back.Height / 2);

                        popup.Width = pointSize.Width + back.Width;
                        popup.Height = pointSize.Height;
                        
                        coords = target.PointToScreen(new Point(target.Width, target.Height / 2));
                        popup.Location = checkBounds(popup, coords.X - target.Width - 12, coords.Y - target.Height / 2 - 5);
                        break;
                }
                
                Point b = popup.PointToClient(coords);
                
                //draw
                Graphics gfx = popup.CreateGraphics();
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.Clear(Color.White);
                
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.Gray)), back);

                if (pos == Position.Top || pos == Position.Bottom)
                    back.Width -= 4;

                gfx.DrawRectangle(new Pen(Color.Black, 2), back);

                switch (sel)
                {
                    case Selection.Rectangle:
                        coords = popup.PointToClient(target.PointToScreen(Point.Empty));

                        if (target != targetWnd)
                            gfx.DrawRectangle(new Pen(Color.Black, 2), coords.X, coords.Y, target.Width, target.Height);
                        else
                            gfx.DrawRectangle(new Pen(Color.Black, 2), coords.X - 8, coords.Y - 30, target.Width, target.Height);
                        break;
                    case Selection.Circle:
                        coords = popup.PointToClient(target.PointToScreen(new Point(-10, -5)));

                        if (target != targetWnd)
                            gfx.DrawEllipse(new Pen(Color.Black, 2), coords.X, coords.Y, target.Width + 20, target.Height + 10);
                        else
                            gfx.DrawEllipse(new Pen(Color.Black, 2), coords.X, coords.Y - 30, target.Width, target.Height);

                        //adjust point b
                        switch (pos)
                        {
                            case Position.Top:
                                b.Y -= 5;
                                break;
                            case Position.Bottom:
                                b.Y += 5;
                                break;
                            case Position.Left:
                                b.X -= 10;
                                break;
                            case Position.Right:
                                b.X += 10;
                                break;
                        }
                        break;
                }

                if (arrow)
                    drawArrow(gfx, a, b);
                else if (line)
                    gfx.DrawLine(new Pen(Color.Black, 4), a, b);
            }

            private void positionButtons(Label lbl, Button buttAdv, Panel butts)
            {
                if (buttAdv.Visible == true)
                {
                    buttAdv.Left = lbl.Left;
                    buttAdv.Top = lbl.Top + lbl.Height + 5;
                    butts.Left = lbl.Left;
                    butts.Top = buttAdv.Top + 5 + buttAdv.Height;
                }
                else
                {
                    butts.Left = lbl.Left;
                    butts.Top = lbl.Top + lbl.Height + 5;
                }
            }

            private Point checkBounds(Form popup, int left, int top)
            {
                return new Point(Math.Max(0, Math.Min(Screen.PrimaryScreen.Bounds.Width - popup.Width, left)), Math.Max(0, Math.Min(Screen.PrimaryScreen.Bounds.Height - popup.Height, top)));
            }

            private Size getPointerSize(Position pos, Label lbl, Button buttAdv, Panel butts)
            {
                if (pos == Position.Left || pos == Position.Right)
                    return new Size(12 + target.Width + 12 + 150, Math.Max(7 + target.Height + 7, 7 + lbl.Height + 5 + (buttAdv.Visible ? buttAdv.Height + 5 : 0) + butts.Height + 7));
                else
                    return new Size(Math.Max(5 + lbl.Width + 5, 12 + target.Width + 12), 7 + target.Height + 7 + 150);
            }

            private Size getBackSize(Label lbl, Button buttAdv, Panel butts)
            {
                return new Size(Math.Max(240, 5 + lbl.Width + 5), 5 + lbl.Height + 5 + (buttAdv.Visible ? buttAdv.Height + 5 : 0) + butts.Height + 5);
            }

            private void drawArrow(Graphics gfx, Point a, Point b)
            {
                double dx = b.X - a.X, dy = b.Y - a.Y;
                double len = Math.Sqrt(dx * dx + dy * dy);
                double cos = dx / len;
                double sin = dy / len * (cos < 0 ? -1 : 1);
                double angle = Math.Asin(sin) + (cos < 0 ? Math.PI : 0);
                sin = Math.Sin(angle);

                Point c =           new Point(b.X - (int)(cos * ARROW_SIZE), b.Y - (int)(sin * ARROW_SIZE));
                Point[] arrow = {   new Point((int)(c.X + Math.Cos(angle) * ARROW_SIZE), (int)(c.Y + Math.Sin(angle) * ARROW_SIZE)),
                                    new Point((int)(c.X + Math.Cos(angle + 2 * Math.PI / 3) * ARROW_SIZE), (int)(c.Y + Math.Sin(angle + 2 * Math.PI / 3) * ARROW_SIZE)),
                                    new Point((int)(c.X + Math.Cos(angle + 4 * Math.PI / 3) * ARROW_SIZE), (int)(c.Y + Math.Sin(angle + 4 * Math.PI / 3) * ARROW_SIZE))};

                gfx.DrawLine(new Pen(Color.Black, 4), a, c);
                gfx.FillPolygon(new SolidBrush(Color.Black), arrow);
            }
        }

        private const int ARROW_SIZE = 6;

        private Form popup = new Form(), targetWnd;
        private Panel butts = new Panel();
        private Label lbl = new Label();
        private Button buttAdv = new Button(), buttPrev = new Button(), buttNext = new Button(), buttClose = new Button();

        private Step[] steps;
        private int currStep = -1;


        public Tutorial(string path, Form targetWnd)
        {
            //show tutorial?
            string[] lines = Misc.ReadLines(path);

            if (lines[0] == "SKIP")
            {
                popup.Close();
                return;
            }

            DialogResult choice = MessageBox.Show("Would you like to see the tutorial?" + Environment.NewLine + "Click 'No' to skip tutorial this time." + Environment.NewLine + "Click 'Cancel' to disable this tutorial permanently.", "It seems this is the first time this window is running.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

            if (choice == DialogResult.Cancel || choice == DialogResult.Yes)
            {
                StreamReader fRdr = new StreamReader(path);
                string contents = fRdr.ReadToEnd();
                fRdr.Close();

                StreamWriter fWrtr = new StreamWriter(path);
                fWrtr.Write("SKIP" + Environment.NewLine + contents);
                fWrtr.Close();
            }

            if (choice == DialogResult.Cancel || choice == DialogResult.No)
            {
                popup.Close();
                return;
            }
            
            //init popup window
            popup.FormBorderStyle = FormBorderStyle.None;
            popup.BackColor = Color.White;
            popup.TransparencyKey = Color.White;
            popup.TopMost = true;

            lbl.Name = "lbl";
            lbl.MinimumSize = new Size(240, 0);
            lbl.MaximumSize = new Size(300, 0);
            lbl.AutoSize = true;
            lbl.BackColor = Color.FromArgb(50, Color.Gray);
            popup.Controls.Add(lbl);

            buttAdv.Name = "buttAdv";
            buttAdv.Text = "Advanced...";
            buttAdv.Visible = false;
            buttAdv.FlatStyle = FlatStyle.Popup;
            buttAdv.BackColor = Color.LightSteelBlue;
            popup.Controls.Add(buttAdv);

            buttPrev.Text = "Previous";
            buttPrev.FlatStyle = FlatStyle.Popup;
            buttPrev.BackColor = Color.LightSteelBlue;
            buttNext.Text = "Next";
            buttNext.FlatStyle = FlatStyle.Popup;
            buttNext.BackColor = Color.LightSteelBlue;
            buttClose.Text = "Close";
            buttClose.FlatStyle = FlatStyle.Popup;
            buttClose.BackColor = Color.LightSteelBlue;

            butts.Name = "butts";
            butts.Width = 240;
            butts.Height = 23;
            butts.BackColor = lbl.BackColor;

            butts.Controls.Add(buttPrev);
            butts.Controls.Add(buttNext);
            butts.Controls.Add(buttClose);
            popup.Controls.Add(butts);

            buttPrev.Visible = false;
            buttPrev.Width = 75;
            buttNext.Width = 75;
            buttClose.Width = 75;
            buttNext.Left = 80;
            buttClose.Left = 160;

            popup.Paint += new PaintEventHandler(popup_Paint);
            targetWnd.Move += new EventHandler(target_Move);
            buttAdv.Click += new EventHandler(buttAdv_Click);
            buttPrev.Click += new EventHandler(buttPrev_Click);
            buttNext.Click += new EventHandler(buttNext_Click);
            buttClose.Click += new EventHandler(buttClose_Click);

            popup.Show();
            
            this.targetWnd = targetWnd;

            //prepare tutorial steps
            steps = new Step[lines.Length];

            for (int i = 0; i < lines.Length; i++)
                steps[i] = new Step(targetWnd, lines[i]);

            steps[++currStep].Activate(popup);
        }

        private void popup_Paint(object sender, PaintEventArgs e)
        {
            if (currStep == -1)
                return;

            steps[currStep].Draw(popup);
        }

        private void target_Move(object sender, EventArgs e)
        {
            popup.Invalidate();
        }

        private void buttAdv_Click(object sender, EventArgs e)
        {
            lbl.Text += Environment.NewLine + buttAdv.Tag;
            buttAdv.Visible = false;

            popup.Invalidate(); //resize popup
        }

        private void buttPrev_Click(object sender, EventArgs e)
        {
            steps[--currStep].Activate(popup);

            //prev/next buttons?
            if (currStep == 0)
                buttPrev.Visible = false;
            buttNext.Visible = true;
        }

        private void buttNext_Click(object sender, EventArgs e)
        {
            steps[++currStep].Activate(popup);

            //prev/next buttons?
            buttPrev.Visible = true;
            if (currStep == steps.Length - 1)
                buttNext.Visible = false;
        }

        private void buttClose_Click(object sender, EventArgs e)
        {
            popup.Close();
        }
    }
}
