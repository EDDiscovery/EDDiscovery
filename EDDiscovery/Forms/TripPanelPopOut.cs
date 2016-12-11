using EDDiscovery.DB;
using EDDiscovery2.EDSM;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class TripPanelPopOut : Form
    {
        static String TITLE = "Trip panel";

        public bool IsFormClosed { get; set; } = false;
        private Timer autofade = new Timer();
        private Color themeColour;
        private Color transparentkey = Color.Red;
        private static EDDiscoveryForm _discoveryform;
        private Color defaultColour;
        private Color warningColour = Color.Orange;

        public void SetGripperColour(Color grip)
        {
            if (grip.GetBrightness() < 0.15)       // override if its too dark..
                grip = Color.Orange;


            panel_grip.ForeColor = grip;
            panel_grip.MouseOverColor = ButtonExt.Multiply(grip, 1.3F);
            panel_grip.MouseSelectedColor = ButtonExt.Multiply(grip, 1.5F);

            transparentkey = (grip == Color.Red) ? Color.Green : Color.Red;
            this.BackColor = transparentkey;
            this.TransparencyKey = transparentkey;
        }

        public void SetTextColour(Color fromTheme)
        {
            themeColour = fromTheme;
        }



        void FadeOut(object sender, EventArgs e)            // hiding
        {
            autofade.Stop();
            panel_grip.Visible = false;
            this.BackColor = transparentkey;
            Invalidate();
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
            if (panel_grip.Visible == false)
            {
                panel_grip.Visible = true;
                this.BackColor = Color.FromArgb(255, 40, 40, 40);
            }
        }

        private void MouseLeaveControl(object sender, EventArgs e)
        {
            if (!ClientRectangle.Contains(this.PointToClient(MousePosition)) && !panel_grip.IsCaptured)
            {
                autofade.Start();
            }
        }



        private void UpdateEventsOnControls(Control ctl)
        {

            ctl.MouseEnter -= MouseEnterControl;
            ctl.MouseLeave -= MouseLeaveControl;
            ctl.MouseLeave += MouseLeaveControl;
            ctl.MouseEnter += MouseEnterControl;

            if (ctl == dpEDSM|| ctl==dpReset) {
             //   ctl is DrawnPanel && ((DrawnPanel)ctl).ImageText != null)
               // ctl.Click += EDSM_Click;
            }
            else
            {
                ctl.MouseDown -= MouseDownOnForm;
                ctl.MouseDown += MouseDownOnForm;
            }

            foreach (Control ctll in ctl.Controls)
            {
                UpdateEventsOnControls(ctll);
            }
        }


        public TripPanelPopOut(EDDiscoveryForm discoveryform)
        {
            _discoveryform = discoveryform;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            autofade.Interval = 500;
            autofade.Tick += FadeOut;
            UpdateEventsOnControls(this);
            this.BackColor = transparentkey;
            this.TransparencyKey = transparentkey;
            this.ContextMenuStrip = contextMenuStrip1;
            this.ShowInTaskbar = false;
            TopMost = true;


            //dpEDSM.Name = cep[i].text.Substring(11);
            dpEDSM.ImageSelected = DrawnPanel.ImageType.InverseText;
            dpEDSM.ImageText = "EDSM";
            dpEDSM.MarginSize = -1;       // 0 is auto calc, -1 is zero
            dpEDSM.Click += EDSM_Click;

            dpReset.ImageSelected = DrawnPanel.ImageType.InverseText;
            dpReset.ImageText = "Reset";
            dpReset.MarginSize = -1;       // 0 is auto calc, -1 is zero
            dpReset.Click += Reset_Click;


            //   dpEDSM.Size = new Size(100, vsize - 6);

            //..  FontSel(vsc.Columns[2].DefaultCellStyle.Font, vsc.Font), panel_grip.ForeColor
            defaultColour =  lblOutput.ForeColor;

            panel_grip.Visible = false;
        }

        private void TripPanelPopOut_Load(object sender, EventArgs e)
        {
            var top = SQLiteDBClass.GetSettingInt("TripPopOutFormTop", -1);
            if (top >= 0)
            {
                var left = SQLiteDBClass.GetSettingInt("TripPopOutFormLeft", 0);
                var height = SQLiteDBClass.GetSettingInt("TripPopOutFormHeight", 800);
                var width = SQLiteDBClass.GetSettingInt("TripPopOutFormWidth", 800);

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
            dpEDSM.MouseOverColor = ButtonExt.Multiply(panel_grip.ForeColor, 1.3F);
            dpEDSM.MouseSelectedColor = ButtonExt.Multiply(panel_grip.ForeColor, 1.5F);
            dpEDSM.BackColor = themeColour;//Color.Black;

            dpReset.MouseOverColor = ButtonExt.Multiply(panel_grip.ForeColor, 1.3F);
            dpReset.MouseSelectedColor = ButtonExt.Multiply(panel_grip.ForeColor, 1.5F);
            dpReset.BackColor = themeColour;//Color.Black;

        }

        private void TripPanelPopOut_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsFormClosed = true;

            SQLiteDBClass.PutSettingInt("TripPopOutFormWidth", this.Width);
            SQLiteDBClass.PutSettingInt("TripPopOutFormHeight", this.Height);
            SQLiteDBClass.PutSettingInt("TripPopOutFormTop", this.Top);
            SQLiteDBClass.PutSettingInt("TripPopOutFormLeft", this.Left);
        }

        private void TripPanelPopOut_Layout(object sender, LayoutEventArgs e)
        {
            panel_grip.Location = new Point(this.ClientSize.Width - panel_grip.Size.Width, this.ClientSize.Height - panel_grip.Size.Height);
        }

        private void TripPanelPopOut_Resize(object sender, EventArgs e)
        {
        }


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_NCL_RESIZE = 0x112;
        public const int HT_RESIZE = 61448;
        private HistoryEntry lastHE;

        private IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(this.Handle, msg, wparam, lparam);
            this.WndProc(ref message);
            return message.Result;
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
        
        internal void displayLastFSD(HistoryEntry he)
        {
            if (he == null)
                return;
            lastHE = he;
            String output = "";
            output += " distance " + he.TravelledDistance.ToString("0.0") + ((he.TravelledMissingjump > 0) ? " LY (*)" : " LY");
            output += " time " + he.TravelledSeconds;
            HistoryEntry lastFuelScoop = _discoveryform.history.GetLastFuelScoop;
            if (lastFuelScoop!=null && lastFuelScoop.FuelTotal > 0)
            {
                double tankSize = SQLiteDBClass.GetSettingDouble("TripPopOutTankSize", 32);
                double tankWarning = SQLiteDBClass.GetSettingDouble("TripPopOutTankWarning", 25);
                if ((he.FuelLevel / tankSize) < (tankWarning / 100.0))
                {
                    output += String.Format(" fuel < {0}%", tankWarning.ToString("0.0"));
                    lblOutput.ForeColor = warningColour;
                }
                else
                {
                    output += " fuel " + String.Format("{0}/{1}", he.FuelLevel.ToString("0.0"), tankSize.ToString("0.0"));
                    lblOutput.ForeColor = defaultColour;
                }
            }
            lblOutput.Text = output;
            dpEDSM.Name = he.System.name;

            lblSystemName.Text = he.System.name;

            EDSMClass edsm = new EDSMClass();
            if (edsm.GetUrlToEDSMSystem(he.System.name).Length > 0)
            {
                lblSystemName.Text += ", system known to edsm";
            }
            else
            {
                lblSystemName.Text += ", system unknown to edsm";
            }
        }

        public void EDSM_Click(object sender, EventArgs e)
        {
            DrawnPanel dp = sender as DrawnPanel;

            EDSMClass edsm = new EDSMClass();
            string url = edsm.GetUrlToEDSMSystem(dp.Name);

            if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                System.Diagnostics.Process.Start(url);
            else
                MessageBox.Show("System " + dp.Name + " unknown to EDSM");
        }

        public void Reset_Click(object sender, EventArgs e)
        {
            HistoryEntry lastFSD = _discoveryform.history.GetLastFSD;
            foreach (HistoryEntry he in _discoveryform.history)
            {
                if (he.StartMarker)
                {
                    he.StartMarker = false;
                    EliteDangerous.JournalEntry.UpdateSyncFlagBit(he.Journalid, EliteDangerous.SyncFlags.StartMarker, false);
                }
                if (he.StopMarker)
                {
                    he.StopMarker = false;
                    EliteDangerous.JournalEntry.UpdateSyncFlagBit(he.Journalid, EliteDangerous.SyncFlags.StopMarker, false);
                }
            }
            if (lastFSD != null)
            {
                lastFSD.StartMarker = true;
                EliteDangerous.JournalEntry.UpdateSyncFlagBit(lastFSD.Journalid, EliteDangerous.SyncFlags.StartMarker, true);
            }
            _discoveryform.RefreshHistoryAsync();
        }

         static class Prompt
        {
            public static string ShowDialog(string text,String defaultValue, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 500,
                    Height = 200,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
                textBox.Text = defaultValue;
                Button confirmation = new Button() { Text = "Ok", Left = 245, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "Cancel", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.Cancel };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                cancel.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(cancel);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
            }
        }

        private void setFuelTankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tankSize = SQLiteDBClass.GetSettingDouble("TripPopOutTankSize", 32.0);
            string promptValue = Prompt.ShowDialog("Set fuel tank size", "" + tankSize, TITLE);
            double value = 0;
            if (promptValue != null && double.TryParse(promptValue, out value))
            {
                SQLiteDBClass.PutSettingDouble("TripPopOutTankSize", value);
                displayLastFSD(lastHE);
            }
            else
            {
                MessageBox.Show("Please enter a numeric value", TITLE);
            }
        }

        private void setFuelWarningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tankWarning = SQLiteDBClass.GetSettingDouble("TripPopOutTankWarning", 25.0);
            string promptValue = Prompt.ShowDialog("Set fuel tank warning percentage","" + tankWarning, TITLE);
            double value = 0;
            if (promptValue != null && double.TryParse(promptValue, out value)
                && value>=0 && value<=100)
            {
                SQLiteDBClass.PutSettingDouble("TripPopOutTankWarning", value);
                displayLastFSD(lastHE);
            }
            else
            {
                MessageBox.Show("Please enter a numeric value between 1-100", TITLE);
            }
        }
    }
}
