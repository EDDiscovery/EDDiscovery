using EDDiscovery.DB;
using EDDiscovery2.DB;
using EDDiscovery2.EDSM;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2
{
    public partial class SummaryPopOut : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_NCL_RESIZE = 0x112;
        public const int HT_RESIZE = 61448;

        private IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(this.Handle, msg, wparam, lparam);
            this.WndProc(ref message);
            return message.Result;
        }

        private Color transparentkey = Color.Red;

        private Timer autofade = new Timer();
        private LabelTable lt;

        public SummaryPopOut()
        {
            InitializeComponent();
            panel_grip.Visible = false;
            autofade.Interval = 500;
            autofade.Tick += FadeOut;
            TopMost = true;
            lt = new LabelTable(this,30);
            UpdateControls(this);
            this.BackColor = transparentkey;
            this.TransparencyKey = transparentkey;
        }

        public void SetLabelFormat(Font f, Color textc)
        {
            if (textc.GetBrightness() < 0.15)       // override if its too dark..
                textc = Color.White;

            transparentkey = (textc == Color.Red) ? Color.Green : Color.Red;
            this.BackColor = transparentkey;
            this.TransparencyKey = transparentkey;
            lt.SetLabelFormat(f, textc);
            panel_grip.ForeColor = textc;
            panel_grip.MouseOverColor = ButtonExt.Multiply(textc, 1.3F);
            panel_grip.MouseSelectedColor = ButtonExt.Multiply(textc, 1.5F);
        }

        public void Update(DataGridView vsc )
        {
            if (vsc != null )
            {
                lt.Dispose();

                for( int i = 0; i < vsc.Rows.Count; i++ )
                {
                    lt.Add(false, ((DateTime)vsc.Rows[i].Cells[0].Value).ToString("hh:mm.ss"), (string)vsc.Rows[i].Cells[1].Value, (string)vsc.Rows[i].Cells[2].Value, (string)vsc.Rows[i].Cells[3].Value);

                    if (lt.Full)
                        break;
                }
            }

            UpdateControls(this);
        }

        private void SummaryPopOut_Load(object sender, EventArgs e)
        {
            var top = SQLiteDBClass.GetSettingInt("PopOutFormTop", -1);
            if (top >= 0 )
            {
                var left = SQLiteDBClass.GetSettingInt("PopOutFormLeft", 0);
                var height = SQLiteDBClass.GetSettingInt("PopOutFormHeight", 800);
                var width = SQLiteDBClass.GetSettingInt("PopOutFormWidth", 800);

                // Adjust so window fits on screen; just in case user unplugged a monitor or something

                var screen = SystemInformation.VirtualScreen;
                if (height > screen.Height) height = screen.Height;
                if (top + height > screen.Height + screen.Top) top = screen.Height + screen.Top - height;
                if (width > screen.Width) width = screen.Width;
                if (left + width > screen.Width + screen.Left) left = screen.Width + screen.Left - width;
                if (top < screen.Top) top = screen.Top;
                if (left < screen.Left) left = screen.Left;

                this.Top = top;
                this.Left = left;
                this.Height = height;
                this.Width = width;

                this.CreateParams.X = this.Left;
                this.CreateParams.Y = this.Top;
                this.StartPosition = FormStartPosition.Manual;
            }
        }

        private void SummaryPopOut_FormClosing(object sender, FormClosingEventArgs e)
        {
            SQLiteDBClass.PutSettingInt("PopOutFormWidth", this.Width);
            SQLiteDBClass.PutSettingInt("PopOutFormHeight", this.Height);
            SQLiteDBClass.PutSettingInt("PopOutFormTop", this.Top);
            SQLiteDBClass.PutSettingInt("PopOutFormLeft", this.Left);
        }

        private void UpdateControls(Control ctl)
        {
           // Console.WriteLine("Hook " + ctl.ToString() + " " + ctl.Name);
            ctl.MouseEnter -= MouseEnterControl;
            ctl.MouseLeave -= MouseLeaveControl;
            ctl.MouseEnter += MouseEnterControl;
            ctl.MouseLeave += MouseLeaveControl;

            if (ctl is DrawnPanel)
            {
                if (((DrawnPanel)ctl).ImageText != null )   
                {
                    ctl.Click += EDSM_Click;
                }
            }
            else
            { 
                ctl.MouseDown -= MouseDownOnForm;
                ctl.MouseDown += MouseDownOnForm;
                //Console.WriteLine("Hook Mouse down " + ctl.ToString() + " " + ctl.Name);
            }

            foreach (Control ctll in ctl.Controls)
            {
                UpdateControls(ctll);
            }
        }


        private void SummaryPopOut_Layout(object sender, LayoutEventArgs e)
        {
            panel_grip.Location = new Point(this.ClientSize.Width - panel_grip.Size.Width, this.ClientSize.Height - panel_grip.Size.Height);
        }

        void FadeOut(object sender, EventArgs e)            // hiding
        {
            panel_grip.Visible = false;
            autofade.Stop();
            this.BackColor = transparentkey;
            this.TransparencyKey = transparentkey;
            this.Opacity = 1;
        }

        private void panel_grip_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                panel_grip.Captured();      // tell drawn panel in a capture
                panel_grip.Capture = false;
                SendMessage(WM_NCL_RESIZE, (IntPtr)HT_RESIZE, IntPtr.Zero);
            }
        }

        private void MouseDownOnForm(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ((Control)sender).Capture = false;
                SendMessage(WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
            }
        }

        private void MouseEnterControl(object sender, EventArgs e)
        {
            autofade.Stop();
            panel_grip.Visible = true;
            this.BackColor = Color.FromArgb(255, 10, 10, 10);
            this.TransparencyKey = Color.Transparent;
            this.Opacity = 0.75;

            //Console.WriteLine(Environment.TickCount + " enter" + sender.ToString());
        }

        private void MouseLeaveControl(object sender, EventArgs e)
        {
            if (!ClientRectangle.Contains(this.PointToClient(MousePosition)) && !panel_grip.IsCaptured)
            {
                //Console.WriteLine(Environment.TickCount + " leave " + sender.ToString());
                autofade.Start();
            }
            else
            {
             //   Console.WriteLine(Environment.TickCount + " rejected leave " + sender.ToString());
            }

        }

        public void EDSM_Click(object sender, EventArgs e)
        {
            DrawnPanel dp = sender as DrawnPanel;
            Console.WriteLine("EDSM click on " + dp.Name);

            EDSMClass edsm = new EDSMClass();
            string url = edsm.GetUrlToEDSMSystem(dp.Name);

            if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                System.Diagnostics.Process.Start(url);
            else
                MessageBox.Show("System unknown to EDSM");
        }

    }

    public class LabelTable : IDisposable
    {
        Font fnt = new Font("Microsoft Sans Serif", 8.25F);
        Color textcol = Color.White;

        int maxvertical;
        const int toppos = 8;
        const int vspacing = 20;

        List<LabelRowEntry> entries = new List<LabelRowEntry>();
        Control parent;

        public LabelTable(Control p, int m)
        {
            parent = p;
            maxvertical = m;
        }

        public void SetLabelFormat(Font f, Color textc)
        {
            fnt = f;
            textcol = textc;
            foreach (LabelRowEntry lre in entries)
                lre.SetFormat(fnt, textcol);
        }

        public void Add(bool insertattop , string datetime, string systemname, string dist , string note)
        {
            string[] text = { datetime, systemname, dist , note };

            if (insertattop)
            {
                if (Full)
                {
                    entries[entries.Count-1].Dispose();
                    entries.RemoveAt(entries.Count - 1);
                }

                foreach (LabelRowEntry lre in entries)
                    lre.ShiftVert(vspacing);

                entries.Insert(0, new LabelRowEntry(text, toppos, vspacing, parent));
                entries[0].SetFormat(fnt, textcol);
            }
            else
            {
                if (Full)
                {
                    entries[0].Dispose();
                    entries.RemoveAt(0);
                }

                entries.Add(new LabelRowEntry(text, toppos+vspacing*entries.Count, vspacing, parent));
                entries[entries.Count-1].SetFormat(fnt, textcol);
            }
        }

        public bool Full { get { return entries.Count >= maxvertical;  } }
        public int Maximum {  get { return maxvertical;  } }

        public void Dispose()
        {
            foreach (LabelRowEntry lre in entries)
                lre.Dispose();

            entries.Clear();
        }
    }

    public class LabelRowEntry : IDisposable
    {
        private ExtendedControls.LabelExt[] labels = new ExtendedControls.LabelExt[4];
        private ExtendedControls.DrawnPanel edsm;
        Control parent;

        public LabelRowEntry(string[] text, int vpos, int vsize , Control p )
        {
            parent = p;
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = new ExtendedControls.LabelExt();
                labels[i].Text = text[i];
                labels[i].Location = new Point(0, vpos);
                labels[i].AutoSize = false;
                labels[i].Size = new Size(100, vsize);
                parent.Controls.Add(labels[i]);
            }

            edsm = new ExtendedControls.DrawnPanel();
            edsm.Name = text[1];
            edsm.Image = DrawnPanel.ImageType.Text;
            edsm.ImageText = "EDSM";
            edsm.Size = new Size(100, vsize);
            edsm.Location = new Point(0, vpos);
            parent.Controls.Add(edsm);
        }

        public void SetFormat(Font f, Color t)
        {
            int butoff = 40;
            int[] defh = { 4, 60, 200, 250,500 };       // for a 8.25 font..

            for (int i = 0; i < labels.Length+1; i++)
                defh[i] = butoff + (int)(defh[i] * (double)f.SizeInPoints / 8.25);

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Font = f;
                labels[i].ForeColor = t;
                labels[i].Location = new Point(defh[i], labels[i].Location.Y);
                labels[i].Size = new Size(defh[i+1] - defh[i] - 4, labels[i].Size.Height);
                labels[i].Show();
            }

            edsm.Font = f;
            edsm.ForeColor = t;
            edsm.Location = new Point(4,edsm.Location.Y);
            edsm.Size = new Size(defh[0]-8, edsm.Size.Height);
            edsm.Show();
        }

        public void ShiftVert(int vert)
        {
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Location = new Point(labels[i].Location.X, labels[i].Location.Y + vert);
            }

            edsm.Location = new Point(edsm.Location.X, edsm.Location.Y + vert);
        }

        public void Dispose()
        {
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Hide();
                parent.Controls.Remove(labels[i]);
                labels[i] = null;
            }
        }

        ~LabelRowEntry()
        {
        }
    }
}
