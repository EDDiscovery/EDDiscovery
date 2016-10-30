using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using EDDiscovery2.EDSM;
using EMK.LightGeometry;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        public bool IsFormClosed { get; set; } = false;
        private Color transparentkey = Color.Red;
        private Timer autofade = new Timer();
        private ControlTable lt;
        private Font butfont = new Font("Microsoft Sans Serif", 8.25F);
        private int statictoplines = 0;

        private bool hideall = false;

        private List<int> columnpos;
        List<Button> dividers;

        public event EventHandler RequiresRefresh;

        double tabscalar= 1.0;

        [Flags]
        enum Configuration
        {
            showInformation = 1,
            showEDSMButton = 2,
            showTime = 4,
            showDescription = 8,
            showNotes = 16,
            showXYZ = 32,
            showDistancePerStar = 64,

            showDoesNotAffectTabs = 1024,        // above this, tab positions are not changed by changes in these values

            showDistancesOnFSDJumpsOnly = 1024,
            showTargetLine = 2048,
            showBlackBoxAroundText = 4096,
            showExpandOverColumns = 8192,
            showNothingWhenDocked = 16384,
        };

        Configuration config = (Configuration)(Configuration.showTargetLine | Configuration.showEDSMButton | Configuration.showTime | Configuration.showDescription | Configuration.showInformation | Configuration.showNotes | Configuration.showXYZ | Configuration.showDistancePerStar);

        bool Config(Configuration c) { return ( config & c ) != 0; }

        public SummaryPopOut()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            autofade.Interval = 500;
            autofade.Tick += FadeOut;
            TopMost = true;
            UpdateEventsOnControls(this);
            this.BackColor = transparentkey;
            this.TransparencyKey = transparentkey;
            this.ContextMenuStrip = contextMenuStripConfig;
            this.ShowInTaskbar = false;

            config = (Configuration)SQLiteDBClass.GetSettingInt("SummaryPanelOptions", (int)config);
            toolStripMenuItemTargetLine.Checked = Config( Configuration.showTargetLine);
            EDSMButtonToolStripMenuItem.Checked = Config( Configuration.showEDSMButton);
            showTargetToolStripMenuItem.Checked = Config( Configuration.showDistancePerStar);
            showNotesToolStripMenuItem.Checked = Config( Configuration.showNotes);
            showXYZToolStripMenuItem.Checked = Config( Configuration.showXYZ);
            showDescriptionToolStripMenuItem.Checked = Config( Configuration.showDescription);
            showInformationToolStripMenuItem.Checked = Config( Configuration.showInformation);
            blackBoxAroundTextToolStripMenuItem.Checked = Config( Configuration.showBlackBoxAroundText);
            showDistancesOnFSDJumpsOnlyToolStripMenuItem.Checked = Config(Configuration.showDistancesOnFSDJumpsOnly);
            expandTextOverEmptyColumnsToolStripMenuItem.Checked = Config(Configuration.showExpandOverColumns);
            showNothingWhenDockedtoolStripMenuItem.Checked = Config(Configuration.showNothingWhenDocked);

            toolStripComboBoxOrder.Enabled = false; // indicate its a program change
            toolStripComboBoxOrder.SelectedIndex = SQLiteDBClass.GetSettingInt("SummaryPanelLayout", 0);
            toolStripComboBoxOrder.Enabled = true;
            panel_grip.Visible = false;

            ResetTabList();     // pre-populate..

            string tabs = SQLiteDBClass.GetSettingString("SummaryPanelTabs", "");
            try
            {
                List<int> tablist = tabs.Split(',').Select(int.Parse).ToList();

                for (int i = 0; i < tablist.Count && i < columnpos.Count; i++)      // for what we have, and not more than pre-populated, fill
                    columnpos[i] = tablist[i];
            }
            catch { }

            dividers = new List<Button>();

            for (int i =0; i < 10; i++)        // generous number of dividers
            {
                Button p = new Button();
                p.BackgroundImage = EDDiscovery.Properties.Resources.divider;
                p.Size = new Size(20, 24);
                p.Name = "Divider";         // Name important
                p.Tag = i;                //1...N index corresponding to column pos it is associated with
                p.Visible = false;
                p.MouseDown += divider_MouseDown;
                p.MouseUp += divider_MouseUp;
                p.MouseMove += divider_MouseMove;
                this.Controls.Add(p);
                dividers.Add(p);
            }
        }

        void ResetTabList()                             // work out optimum tab spacing by what is selected
        {
            columnpos = new List<int>();

            int pos = 4;
            columnpos.Add(pos);

            if (Config( Configuration.showEDSMButton))       // mirrors UpdateRow
                columnpos.Add(pos=pos+60);

            if (Config( Configuration.showTime))
                columnpos.Add(pos = pos + 80);

            if (Config( Configuration.showDescription))
                columnpos.Add(pos = pos + 200);

            if (Config( Configuration.showInformation))
                columnpos.Add(pos = pos + 200);

            if (toolStripComboBoxOrder.SelectedIndex == 0 && Config( Configuration.showNotes))
                columnpos.Add(pos = pos + 200);

            if (toolStripComboBoxOrder.SelectedIndex == 2 && Config( Configuration.showDistancePerStar))
                columnpos.Add(pos = pos + 60);

            if (Config( Configuration.showXYZ))
            {
                columnpos.Add(pos = pos + 60);
                columnpos.Add(pos = pos + 50);
                columnpos.Add(pos = pos + 60);
            }

            if (toolStripComboBoxOrder.SelectedIndex > 0 && Config( Configuration.showNotes))
                columnpos.Add(pos = pos + 200);

            if (toolStripComboBoxOrder.SelectedIndex < 2 && Config( Configuration.showDistancePerStar))
                columnpos.Add(pos = pos + 60);

            while (columnpos.Count < 4)                                         // need a minimum of 4 columns for target info
                columnpos.Add(columnpos[columnpos.Count - 1] + 100);
            
        }

        public void SetGripperColour(Color grip)
        {
            if (grip.GetBrightness() < 0.15)       // override if its too dark..
                grip = Color.Orange;

            labelExt_NoSystems.ForeColor = grip;

            panel_grip.ForeColor = grip;
            panel_grip.MouseOverColor = ButtonExt.Multiply(grip, 1.3F);
            panel_grip.MouseSelectedColor = ButtonExt.Multiply(grip, 1.5F);

            transparentkey = (grip == Color.Red) ? Color.Green : Color.Red;
            this.BackColor = transparentkey;
            this.TransparencyKey = transparentkey;
        }
        
        public void ResetForm(DataGridView vsc)
        {
            SuspendLayout();

            ControlTable ltold = lt;

            lt = new ControlTable(this, 30, 8, 20 , ClientRectangle.Height);
            statictoplines = 0;

            tabscalar = (double)FontSel(vsc.Columns[2].DefaultCellStyle.Font, vsc.Font).SizeInPoints / 8.25;

            labelExt_NoSystems.Visible = false;
            labelExtDockedLanded.Visible = false;

            if (vsc != null)
            {
                Point3D tpos;
                string name;
                TargetClass.GetTargetPosition(out name, out tpos);  // tpos.? is NAN if no target

                for (int i = 0; i < vsc.Rows.Count; i++)
                {
                    UpdateRow(vsc, vsc.Rows[i], true, 200000, tpos);     // insert at end, give it a large number
                    if (lt.Full)
                        break;
                }

                CheckDocked(vsc.Rows.Count>0 ? vsc.Rows[0] : null);                          // if we are docked.. 

                labelExt_NoSystems.Visible = vsc.Rows.Count == 0;
            }
            else
                labelExt_NoSystems.Visible = true;

            UpdateEventsOnControls(this);

            if (ltold != null)
            {
                ltold.Dispose();
                ltold = null;
            }

            ResumeLayout();
        }

        public void CheckDocked(DataGridViewRow rw)
        {
            labelExtDockedLanded.Visible = false;
            hideall = false;

            if (rw != null)
            {
                HistoryEntry he = (HistoryEntry)rw.Cells[EDDiscovery.TravelHistoryControl.TravelHistoryColumns.HistoryTag].Tag;

                if (Config(Configuration.showNothingWhenDocked) && (he.IsLanded || he.IsDocked))
                {
                    labelExtDockedLanded.Text = (he.IsDocked) ? "Docked" : "Landed";
                    labelExtDockedLanded.Visible = true;
                    hideall = true;                 // forces the vertical extent in display size to 0, meaning no controls get shown
                }
            }

            lt.SetDisplaySize(hideall ? 0 : this.ClientRectangle.Height);
        }

        public void RefreshTarget(DataGridView vsc, HistoryEntry lastknownpos )
        {
            SuspendLayout();

            string name;
            double x, y, z;
            bool targetpresent = TargetClass.GetTargetPosition(out name, out x, out y, out z);

            if (vsc != null && targetpresent && Config(Configuration.showTargetLine) )
            {
                string dist = (lastknownpos != null) ? SystemClass.Distance(lastknownpos.System, x, y, z).ToString("0.00") : "Unknown";

                List<ControlEntryProperties> cep = new List<ControlEntryProperties>();
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[2].DefaultCellStyle.Font, vsc.Font), panel_grip.ForeColor, "Target:"));
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[2].DefaultCellStyle.Font, vsc.Font), panel_grip.ForeColor, name));
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[2].DefaultCellStyle.Font, vsc.Font), panel_grip.ForeColor, dist));

                if (statictoplines == 0)
                {
                    int row = lt.Add(-1,cep, 0);       // insert with rowid -1, at 0
                    lt.FormatRow(row, cep, columnpos, Config(Configuration.showBlackBoxAroundText), Config(Configuration.showExpandOverColumns));
                    statictoplines = 1;
                    UpdateEventsOnControls(this);
                }
                else
                    lt.RefreshTextAtID(-1,cep);     // just refresh data, with a row ID of -1
            }
            else
            {
                if (statictoplines == 1)
                {
                    lt.RemoveAt(0);
                    statictoplines = 0;
                }
            }

            ResumeLayout();
        }
        
        private static Font FontSel(Font f, Font g) { return (f != null) ? f : g; }
        private static Color CSel(Color f, Color g) { return (f.A != 0) ? f : g; }

        public void RefreshRow(DataGridView vsc, DataGridViewRow vscrow, bool addattop = false )
        {
            SuspendLayout();

            Point3D tpos;
            string name;
            TargetClass.GetTargetPosition(out name, out tpos);  // tpos.? is NAN if no target

            UpdateRow(vsc, vscrow, addattop , statictoplines , tpos );      // add= 0 , we do a refesh.  If all=true, we do an insert at top
            UpdateEventsOnControls(this);

            labelExt_NoSystems.Visible = vsc.Rows.Count == 0;

            if (addattop)
                CheckDocked(vscrow);

            ResumeLayout();
        }

        private void UpdateRow(DataGridView vsc, DataGridViewRow vscrow, bool addit, int insertat , Point3D tpos)
        {
            Debug.Assert(vscrow != null && vsc != null);

            if (!vscrow.Visible)            // may not be visible due to being turned off.. if so, reject.
                return;

            Color rowc = CSel(vsc.Rows[vscrow.Index].DefaultCellStyle.ForeColor, vsc.ForeColor);
            if (rowc.GetBrightness() < 0.15)       // override if its too dark..
                rowc = Color.White;

            List<ControlEntryProperties> cep = new List<ControlEntryProperties>();

            HistoryEntry he = (HistoryEntry)vscrow.Cells[EDDiscovery.TravelHistoryControl.TravelHistoryColumns.HistoryTag].Tag;

            if (Config( Configuration.showEDSMButton))
                cep.Add(new ControlEntryProperties(butfont, panel_grip.ForeColor, "!!<EDSMBUT:" + (string)he.System.name));

            if (Config( Configuration.showTime))
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[0].DefaultCellStyle.Font, vsc.Font), rowc, ((DateTime)vscrow.Cells[0].Value).ToString("HH:mm.ss")));

            if (Config( Configuration.showDescription))
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[2].DefaultCellStyle.Font, vsc.Font),  rowc, (string)vscrow.Cells[2].Value));

            if (Config( Configuration.showInformation))
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[3].DefaultCellStyle.Font, vsc.Font), rowc, (string)vscrow.Cells[3].Value));

            if (toolStripComboBoxOrder.SelectedIndex == 0 && Config( Configuration.showNotes))
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[4].DefaultCellStyle.Font, vsc.Font), rowc, (string)vscrow.Cells[4].Value));

            bool showdistance = !Config( Configuration.showDistancesOnFSDJumpsOnly) || he.IsFSDJump;

            if (toolStripComboBoxOrder.SelectedIndex == 2 && Config( Configuration.showDistancePerStar))
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[2].DefaultCellStyle.Font, vsc.Font), rowc, showdistance ? DistToStar(he, tpos) : "" ));

            if (Config( Configuration.showXYZ) )
            {
                string xv = (he.System.HasCoordinate && showdistance) ? he.System.x.ToString("0.00") : "";
                string yv = (he.System.HasCoordinate && showdistance) ? he.System.y.ToString("0.00") : "";
                string zv = (he.System.HasCoordinate && showdistance) ? he.System.z.ToString("0.00") : "";

                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[0].DefaultCellStyle.Font, vsc.Font), rowc, xv));
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[0].DefaultCellStyle.Font, vsc.Font), rowc, yv));
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[0].DefaultCellStyle.Font, vsc.Font), rowc, zv));
            }

            if (toolStripComboBoxOrder.SelectedIndex > 0 && Config( Configuration.showNotes))
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[4].DefaultCellStyle.Font, vsc.Font), rowc, (string)vscrow.Cells[4].Value));

            if (toolStripComboBoxOrder.SelectedIndex < 2 && Config( Configuration.showDistancePerStar))
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[2].DefaultCellStyle.Font, vsc.Font), rowc, showdistance ? DistToStar(he, tpos) : ""));

            if (addit)
            {
                int row = lt.Add(vscrow.Index,cep, insertat);        // row on the summary screen..  remember which row it came from
                lt.FormatRow(row, cep , columnpos, Config(Configuration.showBlackBoxAroundText), Config(Configuration.showExpandOverColumns));
            }
            else
            {
                lt.RefreshTextAtID(vscrow.Index,cep);      // refresh this one with rowid= index
            }
        }

        private string DistToStar(HistoryEntry he, Point3D tpos)
        {
            string res = "";
            if (!double.IsNaN(tpos.X))
            {
                double dist = SystemClass.Distance(he.System, tpos.X,tpos.Y,tpos.Z);
                if (dist >= 0)
                    res = dist.ToString("0.00");
            }

            return res;
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
            IsFormClosed = true;

            SQLiteDBClass.PutSettingInt("PopOutFormWidth", this.Width);
            SQLiteDBClass.PutSettingInt("PopOutFormHeight", this.Height);
            SQLiteDBClass.PutSettingInt("PopOutFormTop", this.Top);
            SQLiteDBClass.PutSettingInt("PopOutFormLeft", this.Left);
            SQLiteDBClass.PutSettingInt("SummaryPanelOptions", (int)config);
            SQLiteDBClass.PutSettingInt("SummaryPanelLayout", toolStripComboBoxOrder.SelectedIndex);
            string s = string.Join<int>(",", columnpos);
            SQLiteDBClass.PutSettingString("SummaryPanelTabs", s);
        }

        private void UpdateEventsOnControls(Control ctl)
        {
            if (!ctl.Name.Equals("Divider"))
            {
                ctl.MouseEnter -= MouseEnterControl;
                ctl.MouseLeave -= MouseLeaveControl;
                ctl.MouseLeave += MouseLeaveControl;
                ctl.MouseEnter += MouseEnterControl;

                if (ctl is DrawnPanel && ((DrawnPanel)ctl).ImageText != null)
                    ctl.Click += EDSM_Click;
                else
                {
                    ctl.MouseMove -= MouseMoveControl;      
                    ctl.MouseMove += MouseMoveControl;
                    ctl.MouseDown -= MouseDownOnForm;           
                    ctl.MouseDown += MouseDownOnForm;
                }
            }

            foreach (Control ctll in ctl.Controls)
            {
                UpdateEventsOnControls(ctll);
            }
        }


        private void SummaryPopOut_Layout(object sender, LayoutEventArgs e)
        {
            panel_grip.Location = new Point(this.ClientSize.Width - panel_grip.Size.Width, this.ClientSize.Height - panel_grip.Size.Height);
        }

        private void SummaryPopOut_Resize(object sender, EventArgs e)
        {
            lt.SetDisplaySize(hideall ? 0 : this.ClientRectangle.Height);
        }

        void FadeOut(object sender, EventArgs e)            // hiding
        {
            autofade.Stop();
            ShowDividers(false);
            panel_grip.Visible = false;
            this.BackColor = transparentkey;
            Invalidate();
        }

        private void panel_grip_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left )
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

        private void MouseMoveControl(object sender, MouseEventArgs e)
        {
            Point pt = this.PointToClient(MousePosition);
            //System.Diagnostics.Debug.WriteLine("Mouse " + pt + " over " + sender.ToString());
            if (dividercapture == -2 && pt.Y < 24)
                ShowDividers(true);
            else if (dividercapture ==-1 && pt.Y >= 24)
                ShowDividers(false);
        }

        int dividercapture = -2;        //-2 not shown, -1 shown, >=0 captured
        int originalxpos = -1;

        private void ShowDividers( bool show)
        {
            foreach (Button p in dividers)
                p.Visible = false;

            //System.Diagnostics.Debug.WriteLine("Dividers " + show);
            if (show)
            {
                dividercapture = -1;

                for (int i = 1; i < columnpos.Count; i++)              // bring up the number of dividers needed
                {
                    dividers[i - 1].Location = new Point(columnpos[i], 0);
                    dividers[i - 1].Visible = true;
                }
            }
            else
                dividercapture = -2;
        }

        private void divider_MouseDown(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            dividercapture = (int)b.Tag;
            b.Capture = true;
            originalxpos = e.X;
            //System.Diagnostics.Debug.WriteLine("Button " + dividercapture +" mouse down");
        }

        private void divider_MouseUp(object sender, MouseEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Button " + dividercapture + " mouse up");
            Button b = sender as Button;
            dividercapture = -1;
            b.Capture = false;
        }

        private void divider_MouseMove(object sender, MouseEventArgs e)
        {
            if ( dividercapture>=0 )
            {
                int colpos = dividercapture + 1;        // because divider 0 is at col pos 1
                Button b = sender as Button;
                int off = e.X - originalxpos;
                int soff = (int)(off / tabscalar);

                if (columnpos[colpos] + soff - columnpos[colpos - 1] >= 20)         // ensure can't get too close to previous one
                {
                    for (int i = colpos; i < columnpos.Count; i++)          // shift this and ones to right..
                    {
                        dividers[i-1].Location = new Point(dividers[i-1].Location.X + soff, dividers[i-1].Location.Y);
                        columnpos[i] += soff;
                        dividers[i-1].Invalidate();
                    }

                    lt.SetTabStops(columnpos, Config(Configuration.showExpandOverColumns));      // move the text, invalidate ones changed
                }
                //System.Diagnostics.Debug.WriteLine("Capture " + dividercapture + " at " + e.X + "," + off + "," + soff + " to " + b.Location);
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

        private void targetLinetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showTargetLine, ((ToolStripMenuItem)sender).Checked);
        }

        private void EDSMButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showEDSMButton, ((ToolStripMenuItem)sender).Checked);
        }

        private void toolStripMenuItemTime_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showTime, ((ToolStripMenuItem)sender).Checked);
        }

        private void showDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showDescription, ((ToolStripMenuItem)sender).Checked);
        }

        private void showInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showInformation, ((ToolStripMenuItem)sender).Checked);
        }

        private void showNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showNotes, ((ToolStripMenuItem)sender).Checked);
        }

        private void showXYZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showXYZ, ((ToolStripMenuItem)sender).Checked);
        }

        private void showTargetPerStarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showDistancePerStar, ((ToolStripMenuItem)sender).Checked);
        }

        private void blackBoxAroundTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showBlackBoxAroundText, ((ToolStripMenuItem)sender).Checked);
        }

        private void showDistancesOnFSDJumpsOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showDistancesOnFSDJumpsOnly, ((ToolStripMenuItem)sender).Checked);
        }

        private void expandTextOverEmptyColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showExpandOverColumns, ((ToolStripMenuItem)sender).Checked);
        }

        private void showNothingWhenDockedtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showNothingWhenDocked, ((ToolStripMenuItem)sender).Checked);
        }

        private void toolStripComboBoxOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox cbx = (ToolStripComboBox)sender;

            if (cbx.Enabled)
            {
                ResetTabList();
                ShowDividers(false);
                RequiresRefresh(this, null);
            }

            contextMenuStripConfig.Close();
        }

        void FlipConfig( Configuration item , bool ch)
        {
            if (ch)
                config = (Configuration)((int)config | (int)item);
            else
                config = (Configuration)((int)config & ~(int)item);

            if (item < Configuration.showDoesNotAffectTabs)
                ResetTabList();

            ShowDividers(false);

            RequiresRefresh(this, null);
        }

        private void contextMenuStripConfig_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {   // because we may move the cursor out of the client during the dialog, MouseLeave does not get called.. so check here
            if (!ClientRectangle.Contains(this.PointToClient(MousePosition)))
                autofade.Start(); 
        }
    }

    public class ControlTable : IDisposable
    {
        List<ControlRowEntry> entries = new List<ControlRowEntry>();
        Control parent;

        private int maxvertical;
        private int toppos;
        private int vspacing;
        private int vheight;

        public int Rows { get { return entries.Count; } }
        public int Columns { get { return entries.Count>0 ? entries[0].Count : 0; } }

        public ControlTable(Control p, int m , int topp , int vspace , int vh)
        {
            parent = p;
            toppos = topp;
            maxvertical = m;
            vspacing = vspace;
            vheight = vh;
        }

        public void FormatRow(int row, List<ControlEntryProperties> cep, List<int> tabstops, bool blackbox , bool expandover)
        {
            if (row < entries.Count)
            {
                entries[row].FormatRow(cep, tabstops, blackbox, expandover);
                entries[row].DisplayRow(vheight);
            }
        }

        public int Add(int rowid, List<ControlEntryProperties> cep, int insertpos )      // return row that was added..
        {
            if (Full)
            {
                entries[entries.Count-1].Dispose();
                entries.RemoveAt(entries.Count - 1);
            }

            for (int i = insertpos; i < entries.Count; i++)
            {
                entries[i].ShiftDownRow(vspacing);
                entries[i].DisplayRow(vheight);
            }

            if (insertpos >= entries.Count)
            {
                insertpos = entries.Count;
                entries.Add(new ControlRowEntry(rowid, cep, toppos + insertpos*vspacing, vspacing, parent));
                return insertpos;
            }
            else
            {
                entries.Insert(insertpos, new ControlRowEntry(rowid, cep, toppos + insertpos*vspacing, vspacing, parent));
                return insertpos;
            }
        }

        public void RemoveAt( int pos )
        {
            for (int i = pos + 1; i < entries.Count; i++)
            {
                entries[i].ShiftDownRow(-vspacing);
                entries[i].DisplayRow(vheight);
            }

            entries[pos].Dispose();
            entries.RemoveAt(pos);
        }

        public void RefreshTextAtID(int rowid, List<ControlEntryProperties> cep)
        {
            foreach (ControlRowEntry cre in entries)
            {
                if ( cre.RowId == rowid )
                {
                    cre.RefreshText(cep);
                    break;
                }
            }
        }

        public void SetDisplaySize(int vh)
        {
            vheight = vh;
            //System.Diagnostics.Debug.WriteLine("Display H " + vheight);
            foreach (ControlRowEntry cre in entries)
                cre.DisplayRow(vheight);
        }

        public bool Full { get { return entries.Count >= maxvertical;  } }
        public int Maximum {  get { return maxvertical;  } }

        public void Dispose()
        {
            foreach (ControlRowEntry lre in entries)
                lre.Dispose();

            entries.Clear();
        }

        public void SetTabStops(List<int> tabstops, bool expandover)
        {
            foreach (ControlRowEntry cre in entries)
                cre.SetTabStops(tabstops, expandover);
        }
    }

    public class ControlRowEntry : IDisposable
    {
        private List<Control> items;
        private Control parent;
        public int RowId { get; }      // row id of creator..
        public int Count { get { return items.Count; } }

        public ControlRowEntry(int id , List<ControlEntryProperties> cep, int vpos, int vsize , Control p )
        {
            RowId = id;
            parent = p;

            items = new List<Control>();
            for (int i = 0; i < cep.Count; i++)
            {
                if (cep[i].text.Length >= 11 && cep[i].text.Substring(0,11).Equals("!!<EDSMBUT:"))
                {
                    ExtendedControls.DrawnPanel edsm = new ExtendedControls.DrawnPanel();
                    edsm.Name = cep[i].text.Substring(11);
                    edsm.Image = DrawnPanel.ImageType.Text;
                    edsm.ImageText = "EDSM";
                    edsm.Size = new Size(100, vsize-6);
                    edsm.MarginSize = -1;       // 0 is auto calc, -1 is zero
                    edsm.Location = new Point(0, vpos);
                    parent.Controls.Add(edsm);
                    items.Add(edsm);
                }
                else
                {
                    LabelExt lab = new ExtendedControls.LabelExt();
                    lab.Text = cep[i].text;
                    lab.Location = new Point(0, vpos);
                    lab.AutoSize = false;
                    lab.Size = new Size(100, vsize-2);
                    parent.Controls.Add(lab);
                    items.Add(lab);
                }
            }
        }

        public void RefreshText(List<ControlEntryProperties> cep)
        {
            for (int i = 0; i < items.Count; i++)
                items[i].Text = cep[i].text;
        }

        public void FormatRow(List<ControlEntryProperties> cep , List<int> tabstops, bool blackbox , bool expandover)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Font = cep[i].font;
                items[i].ForeColor = cep[i].colour;

                if (items[i] is ExtendedControls.DrawnPanel)
                {
                    ExtendedControls.DrawnPanel dp = items[i] as ExtendedControls.DrawnPanel;
                    dp.MouseOverColor = ButtonExt.Multiply(cep[i].colour, 1.3F);
                    dp.MouseSelectedColor = ButtonExt.Multiply(cep[i].colour, 1.5F);
                    dp.BackColor = Color.Black;
                    items[i].Location = new Point(tabstops[i], items[i].Location.Y+3);
                }
                else
                {
                    LabelExt le = items[i] as ExtendedControls.LabelExt;
                    le.BackColor = Color.Transparent;
                    le.TextBackColor = (blackbox) ? Color.Black : Color.Transparent;
                    le.Location = new Point(tabstops[i], items[i].Location.Y);
                }

                SetSize(i, tabstops,expandover);
            }
        }

        public void SetTabStops(List<int> tabstops, bool expandover)
        {
            for (int i = 0; i < items.Count; i++)
            {
                int curx = items[i].Location.X;
                int sizex = items[i].Size.Width;

                items[i].Location = new Point(tabstops[i], items[i].Location.Y);
                SetSize(i, tabstops , expandover);

                if (items[i].Location.X != curx || items[i].Size.Width != sizex && items[i].Visible)
                    items[i].Invalidate();
            }
        }

        public void SetSize(int i, List<int> tabstops , bool expandover)
        {
            int j = i + 1;

            for (; j < items.Count && expandover; j++)                    // find next non empty column
            {
                if (items[i] is ExtendedControls.DrawnPanel || items[j].Text.Length > 0)
                    break;
            }

            int width = tabstops[j] - tabstops[i] - 4;

            if (items[i] is ExtendedControls.DrawnPanel)
                items[i].Size = new Size(Math.Min(width, 64), items[i].Size.Height);
            else
                items[i].Size = new Size(width, items[i].Size.Height);
        }

        public void DisplayRow( int vheight )
        {
            for (int i = 0; i < items.Count; i++)
            {
                bool vis = items[i].Location.Y < vheight;
                //if ( i == 0) System.Diagnostics.Debug.WriteLine("At " + items[i].Location + " vis " + vis);
                items[i].Visible = vis;
            }
        }

        public void ShiftDownRow(int vert)
        {
            for (int i = 0; i < items.Count; i++)
                items[i].Location = new Point(items[i].Location.X, items[i].Location.Y + vert);
        }

        public void Dispose()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Hide();
                parent.Controls.Remove(items[i]);
                items[i].Dispose();
            }

            items = null;
        }
    }

    public class ControlEntryProperties
    {
        public ControlEntryProperties(Font f, Color c, string tx)
        {
            font = f; colour = c; text = tx;
        }

        public Font font;
        public Color colour;
        public string text;
    }

}


