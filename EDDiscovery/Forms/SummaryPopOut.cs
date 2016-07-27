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

        private Color transparentkey = Color.Red;
        private Timer autofade = new Timer();
        private ControlTable lt;
        private Font butfont = new Font("Microsoft Sans Serif", 8.25F);
        private int statictoplines = 0;
        private double tabscalar = 1.0;

        public event EventHandler RequiresRefresh;

        [Flags]
        enum Configuration
        {
            showTargetLine = 1,
            showEDSMButton = 2,
            showTime = 4,
            showDistance = 8,
            showNotes = 16,
            showXYZ = 32,
            showDistancePerStar = 64,
        };

        Configuration config = (Configuration)( Configuration.showTargetLine | Configuration.showEDSMButton | Configuration.showTime | Configuration.showDistance | Configuration.showNotes | Configuration.showXYZ | Configuration.showDistancePerStar);

        public SummaryPopOut()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            panel_grip.Visible = false;
            autofade.Interval = 500;
            autofade.Tick += FadeOut;
            TopMost = true;
            UpdateEventsOnControls(this);
            this.BackColor = transparentkey;
            this.TransparencyKey = transparentkey;
            this.ContextMenuStrip = contextMenuStripConfig;

            config = (Configuration)SQLiteDBClass.GetSettingInt("SummaryPanelOptions", (int)config);
            toolStripMenuItemTargetLine.Checked = (config & Configuration.showTargetLine) != 0;
            EDSMButtonToolStripMenuItem.Checked = (config & Configuration.showEDSMButton) != 0;
            showTargetToolStripMenuItem.Checked = (config & Configuration.showDistancePerStar) != 0;
            showNotesToolStripMenuItem.Checked = (config & Configuration.showNotes) != 0;
            showXYZToolStripMenuItem.Checked = (config & Configuration.showXYZ) != 0;
            showDistanceToolStripMenuItem.Checked = (config & Configuration.showDistance) != 0;
            toolStripComboBoxOrder.Enabled = false; // indicate its a program change
            toolStripComboBoxOrder.SelectedIndex = SQLiteDBClass.GetSettingInt("SummaryPanelLayout", 0);
            toolStripComboBoxOrder.Enabled = true;
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

            lt = new ControlTable(this, 30, 8, 20);
            statictoplines = 0;

            tabscalar = (double)FontSel(vsc.Columns[2].DefaultCellStyle.Font, vsc.Font).SizeInPoints / 8.25;

            if (vsc != null)
            {
                Point3D tpos;
                string name;
                TargetClass.GetTargetPosition(out name, out tpos);  // tpos.? is NAN if no target

                for (int i = 0; i < vsc.Rows.Count; i++)
                {
                    UpdateRow(vsc, vsc.Rows[i], true, 200000 , tpos);     // insert at end, give it a large number
                    if (lt.Full)
                        break;
                }
            }

            labelExt_NoSystems.Visible = (lt.Count == statictoplines);

            UpdateEventsOnControls(this);

            if (ltold != null)
            {
                ltold.Dispose();
                ltold = null;
            }

            ResumeLayout();
        }


        public void RefreshTarget(DataGridView vsc, List<VisitedSystemsClass> vscl)
        {
            SuspendLayout();

            string name;
            double x, y, z;
            bool targetpresent = TargetClass.GetTargetPosition(out name, out x, out y, out z);

            if (vsc != null && targetpresent && ( config & Configuration.showTargetLine) != 0 )
            {
                SystemClass cs = VisitedSystemsClass.GetSystemClassFirstPosition(vscl);
                string dist = (cs != null) ? SystemClass.Distance(cs, x, y, z).ToString("0.00") : "Unknown";

                List<ControlEntryProperties> cep = new List<ControlEntryProperties>();
                int pos = 4 + (((config & Configuration.showEDSMButton)!=0) ? (int)(40*tabscalar) : 0);
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[1].DefaultCellStyle.Font, vsc.Font), ref pos, 60 * tabscalar, panel_grip.ForeColor, "Target:"));
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[2].DefaultCellStyle.Font, vsc.Font), ref pos, 150 * tabscalar, panel_grip.ForeColor, name));
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[3].DefaultCellStyle.Font, vsc.Font), ref pos, 60 * tabscalar, panel_grip.ForeColor, dist));

                if (statictoplines == 0)
                {
                    int row = lt.Add(-1,cep, 0);       // insert with rowid -1, at 0
                    lt.SetFormat(row, cep);
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

            labelExt_NoSystems.Visible = (lt.Count == statictoplines);
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

            int pos = 4;

            List<ControlEntryProperties> cep = new List<ControlEntryProperties>();

            if ((config & Configuration.showEDSMButton) != 0)
                cep.Add(new ControlEntryProperties(butfont, ref pos, 40 * tabscalar, panel_grip.ForeColor, "!!<EDSMBUT:" + (string)vscrow.Cells[1].Value));

            if ((config & Configuration.showTime) != 0)
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[0].DefaultCellStyle.Font, vsc.Font), ref pos, 60 * tabscalar, rowc, ((DateTime)vscrow.Cells[0].Value).ToString("HH:mm.ss")));

            cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[1].DefaultCellStyle.Font, vsc.Font), ref pos, 150 * tabscalar, rowc, (string)vscrow.Cells[1].Value));

            if ((config & Configuration.showDistance) != 0)
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[2].DefaultCellStyle.Font, vsc.Font), ref pos, 50 * tabscalar, rowc, (string)vscrow.Cells[2].Value));

            if (toolStripComboBoxOrder.SelectedIndex == 0 && (config & Configuration.showNotes) != 0)
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[3].DefaultCellStyle.Font, vsc.Font), ref pos, 150 * tabscalar, rowc, (string)vscrow.Cells[3].Value));

            VisitedSystemsClass vscentry = (VisitedSystemsClass)vscrow.Cells[EDDiscovery.TravelHistoryControl.TravelHistoryColumns.SystemName].Tag;

            if (toolStripComboBoxOrder.SelectedIndex == 2 && (config & Configuration.showDistancePerStar) != 0)
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[2].DefaultCellStyle.Font, vsc.Font), ref pos, 60 * tabscalar, rowc, DistToStar(vscentry, tpos)));

            if ((config & Configuration.showXYZ) != 0 && vscentry != null)
            {
                string xv = (vscentry.curSystem.HasCoordinate) ? vscentry.curSystem.x.ToString("0.00") : "-";
                string yv = (vscentry.curSystem.HasCoordinate) ? vscentry.curSystem.y.ToString("0.00") : "-";
                string zv = (vscentry.curSystem.HasCoordinate) ? vscentry.curSystem.z.ToString("0.00") : "-";

                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[0].DefaultCellStyle.Font, vsc.Font), ref pos, 60 * tabscalar, rowc, xv));
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[0].DefaultCellStyle.Font, vsc.Font), ref pos, 50 * tabscalar, rowc, yv));
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[0].DefaultCellStyle.Font, vsc.Font), ref pos, 60 * tabscalar, rowc, zv));
            }

            if (toolStripComboBoxOrder.SelectedIndex > 0 && (config & Configuration.showNotes) != 0)
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[3].DefaultCellStyle.Font, vsc.Font), ref pos, 150 * tabscalar, rowc, (string)vscrow.Cells[3].Value));

            if (toolStripComboBoxOrder.SelectedIndex < 2 && (config & Configuration.showDistancePerStar) != 0)
                cep.Add(new ControlEntryProperties(FontSel(vsc.Columns[2].DefaultCellStyle.Font, vsc.Font), ref pos, 60 * tabscalar, rowc, DistToStar(vscentry, tpos)));

            if (addit)
            {
                int row = lt.Add(vscrow.Index,cep, insertat);        // row on the summary screen..  remember which row it came from
                lt.SetFormat(row, cep);
            }
            else
            {
                lt.RefreshTextAtID(vscrow.Index,cep);      // refresh this one with rowid= index
            }
        }

        private string DistToStar(VisitedSystemsClass vscentry, Point3D tpos)
        {
            string res = "";
            if (!double.IsNaN(tpos.X))
            {
                double dist = VisitedSystemsClass.Distance(vscentry, tpos);
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
            SQLiteDBClass.PutSettingInt("PopOutFormWidth", this.Width);
            SQLiteDBClass.PutSettingInt("PopOutFormHeight", this.Height);
            SQLiteDBClass.PutSettingInt("PopOutFormTop", this.Top);
            SQLiteDBClass.PutSettingInt("PopOutFormLeft", this.Left);
            SQLiteDBClass.PutSettingInt("SummaryPanelOptions", (int)config);
            SQLiteDBClass.PutSettingInt("SummaryPanelLayout", toolStripComboBoxOrder.SelectedIndex);
        }

        private void UpdateEventsOnControls(Control ctl)
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

            ctl.Show();

            foreach (Control ctll in ctl.Controls)
            {
                UpdateEventsOnControls(ctll);
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
            this.BackColor = Color.FromArgb(255, 40, 40, 40);
            //Console.WriteLine(Environment.TickCount + " enter" + sender.ToString());
        }

        private void MouseLeaveControl(object sender, EventArgs e)
        {
            if (!ClientRectangle.Contains(this.PointToClient(MousePosition)) && !panel_grip.IsCaptured)
            {
                //Console.WriteLine(Environment.TickCount + " leave " + sender.ToString());
                autofade.Start();
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

        private void showDistanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showDistance, ((ToolStripMenuItem)sender).Checked);
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

        private void toolStripComboBoxOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox cbx = (ToolStripComboBox)sender;

            if ( cbx.Enabled )
                RequiresRefresh(this, null);

            contextMenuStripConfig.Close();
        }

        void FlipConfig( Configuration item , bool ch)
        {
            if (ch)
                config = (Configuration)((int)config | (int)item);
            else
                config = (Configuration)((int)config & ~(int)item);

            RequiresRefresh(this, null);
        }

    }

    public class ControlTable : IDisposable
    {
        List<ControlRowEntry> entries = new List<ControlRowEntry>();
        Control parent;

        private int maxvertical;
        private int toppos;
        private int vspacing;

        public int Count { get { return entries.Count; } }

        public ControlTable(Control p, int m , int topp , int vspace)
        {
            parent = p;
            toppos = topp;
            maxvertical = m;
            vspacing = vspace;
        }

        public void SetFormat(int row, List<ControlEntryProperties> cep)
        {
            if ( row < entries.Count )
                entries[row].SetFormat(cep);
        }

        public int Add(int rowid, List<ControlEntryProperties> cep, int insertpos )      // return row that was added..
        {
            if (Full)
            {
                entries[entries.Count-1].Dispose();
                entries.RemoveAt(entries.Count - 1);
            }

            for (int i = insertpos; i < entries.Count; i++)
                entries[i].ShiftVert(vspacing);

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
            for (int i = pos+1; i < entries.Count; i++)
                entries[i].ShiftVert(-vspacing);
            entries[pos].Dispose();
            entries.RemoveAt(pos);
        }

        public void RefreshTextAtID(int rowid, List<ControlEntryProperties> cep)
        {
            foreach( ControlRowEntry cre in entries )
            {
                if ( cre.RowId == rowid )
                {
                    cre.RefreshText(cep);
                    break;
                }
            }
        }

        public bool Full { get { return entries.Count >= maxvertical;  } }
        public int Maximum {  get { return maxvertical;  } }

        public void Dispose()
        {
            foreach (ControlRowEntry lre in entries)
                lre.Dispose();

            entries.Clear();
        }
    }

    public class ControlRowEntry : IDisposable
    {
        private List<Control> items;
        private Control parent;
        public int RowId { get; }      // row id of creator..

        public ControlRowEntry(int id , List<ControlEntryProperties> cep, int vpos, int vsize , Control p )
        {
            RowId = id;
            parent = p;

            items = new List<Control>();
            for (int i = 0; i < cep.Count; i++)
            {
                if (cep[i].text.Length > 11 && cep[i].text.Substring(0,11).Equals("!!<EDSMBUT:"))
                {
                    ExtendedControls.DrawnPanel edsm = new ExtendedControls.DrawnPanel();
                    edsm.Name = cep[i].text.Substring(11);
                    edsm.Image = DrawnPanel.ImageType.Text;
                    edsm.ImageText = "EDSM";
                    edsm.Size = new Size(100, vsize);
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
                    lab.Size = new Size(100, vsize);
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

        public void SetFormat(List<ControlEntryProperties> cep)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Font = cep[i].font;
                items[i].ForeColor = cep[i].colour;

                int nexttabpos = (i < items.Count - 1) ? cep[i + 1].tabstop : 2000;

                if (items[i] is ExtendedControls.DrawnPanel)
                {
                    ExtendedControls.DrawnPanel dp = items[i] as ExtendedControls.DrawnPanel;
                    dp.MouseOverColor = ButtonExt.Multiply(cep[i].colour, 1.3F);
                    dp.MouseSelectedColor = ButtonExt.Multiply(cep[i].colour, 1.5F);
                    dp.BackColor = Color.Black;
                    items[i].Location = new Point(cep[i].tabstop, items[i].Location.Y+3);
                    items[i].Size = new Size(nexttabpos - cep[i].tabstop - 4, items[i].Size.Height - 6);
                }
                else
                {
                    items[i].BackColor = Color.Transparent;
                    items[i].Location = new Point(cep[i].tabstop, items[i].Location.Y);
                    items[i].Size = new Size(nexttabpos - cep[i].tabstop - 4, items[i].Size.Height - 4);
                }
            }
        }

        public void ShiftVert(int vert)
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
        public ControlEntryProperties(Font f, ref int pos , double width, Color c, string tx)
        {
            font = f; tabstop = pos; colour = c; text = tx;
            pos += (int)width;
        }

        public Font font;
        public int tabstop;
        public Color colour;
        public string text;
    }

}


