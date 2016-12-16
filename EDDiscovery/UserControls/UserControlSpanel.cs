#define TH

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.DB;
using EMK.LightGeometry;
using EDDiscovery2.DB;
using ExtendedControls;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSpanel : UserControlCommonBase
    {
        private EDDiscoveryForm discoveryform;
        private TravelHistoryControl travelhistorycontrol;

        private int displaynumber = 0;
        private string DbSave { get { return "SPanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbFilterSave { get { return "SPanelEventFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbFieldFilter { get { return "SPanelFieldFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        EventFilterSelector cfs = new EventFilterSelector();
        private JSONFilter fieldfilter = new JSONFilter();

        private Timer scanhide = new Timer();
        string scantext = null;             // if set, display this text at the right place.
        Point scanpostextoffset = new Point(0, 0); // left/ top used by scan display

        private List<int> columnpos;

        private Color textcolour;
        private Font displayfont;
        private int rowheight = 20;

        HistoryList current_historylist;

        private Timer dividercheck = new Timer();
        private ButtonExt[] dividers;
        private int dividercapture = -2;        //-2 not shown, -1 shown, >=0 captured
        private int divideroriginalxpos = -1;

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

            showScan15s = 32768,
            showScan30s = 65536,
            showScan60s = 131072,
            showScanIndefinite = 262144,
            showScanOff = 0,                    // this is not saved, but is a flag for the function to turn it off

            showScanRight = 524288,
            showScanLeft = 1048576,
            showScanOnTop = 2097152,
            showScanBelow = 4194304,
            showScanAbove = 8388608
        };

        Configuration config = (Configuration)(Configuration.showTargetLine | Configuration.showEDSMButton | Configuration.showTime | Configuration.showDescription |
                                               Configuration.showInformation | Configuration.showNotes | Configuration.showXYZ | Configuration.showDistancePerStar |
                                               Configuration.showScan15s |
                                               Configuration.showScanRight );

        bool Config(Configuration c) { return (config & c) != 0; }
        bool IsSurfaceScanOn { get { return Config(Configuration.showScan15s) || Config(Configuration.showScan30s) || Config(Configuration.showScan60s) || Config(Configuration.showScanIndefinite); } }

        int layoutorder = 0;



        public UserControlSpanel()
        {
            InitializeComponent();
        }

        public override void Init(EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            travelhistorycontrol = ed.TravelControl;
            displaynumber = vn;
            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += NewEntry;
            discoveryform.OnNewTarget += NewTarget;

            config = (Configuration)SQLiteDBClass.GetSettingInt(DbSave + "Config", (int)config);
            toolStripMenuItemTargetLine.Checked = Config(Configuration.showTargetLine);
            toolStripMenuItemTime.Checked = Config(Configuration.showTime);
            EDSMButtonToolStripMenuItem.Checked = Config(Configuration.showEDSMButton);
            showTargetToolStripMenuItem.Checked = Config(Configuration.showDistancePerStar);
            showNotesToolStripMenuItem.Checked = Config(Configuration.showNotes);
            showXYZToolStripMenuItem.Checked = Config(Configuration.showXYZ);
            showDescriptionToolStripMenuItem.Checked = Config(Configuration.showDescription);
            showInformationToolStripMenuItem.Checked = Config(Configuration.showInformation);
            blackBoxAroundTextToolStripMenuItem.Checked = Config(Configuration.showBlackBoxAroundText);
            showDistancesOnFSDJumpsOnlyToolStripMenuItem.Checked = Config(Configuration.showDistancesOnFSDJumpsOnly);
            expandTextOverEmptyColumnsToolStripMenuItem.Checked = Config(Configuration.showExpandOverColumns);
            showNothingWhenDockedtoolStripMenuItem.Checked = Config(Configuration.showNothingWhenDocked);

            SetSurfaceScanBehaviour(null);
            SetScanPosition(null);
            SetLayoutOrder(SQLiteDBClass.GetSettingInt(DbSave + "Layout", layoutorder),false);  // also resets the tab order

            scanhide.Tick += HideScanData;

            dividercheck.Tick += DividerCheck;
            dividercheck.Interval = 500;

            string tabs = SQLiteDBClass.GetSettingString(DbSave + "PanelTabs", "");
            try
            {
                List<int> tablist = tabs.Split(',').Select(int.Parse).ToList();

                for (int i = 0; i < tablist.Count && i < columnpos.Count; i++)      // for what we have, and not more than pre-populated, fill
                    columnpos[i] = tablist[i];
            }
            catch { }

            textcolour = discoveryform.theme.SPanelColor;
            displayfont = discoveryform.theme.GetFont;

            pictureBox.ContextMenuStrip = contextMenuStripConfig;

            string filter = SQLiteDBClass.GetSettingString(DbFieldFilter, "");
            if (filter.Length > 0)
                fieldfilter.FromJSON(filter);        // load filter

            cfs.ConfigureThirdOption("Travel", "Docked;FSD Jump;Undocked;");
            cfs.Changed += EventFilterChanged;

            dividers = new ButtonExt[] { buttonExt0, buttonExt1, buttonExt2, buttonExt3, buttonExt4, buttonExt5, buttonExt6, buttonExt7, buttonExt8, buttonExt9, buttonExt10 };

#if TH
            travelhistorycontrol.OnTravelSelectionChanged += Travelhistorycontrol_OnTravelSelectionChanged;
#endif
        }

#if TH
        private void Travelhistorycontrol_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl)     // DEBUGGING ONLY.. click on a scan and it will trigger it
        {
            if (he.journalEntry.EventTypeID == EliteDangerous.JournalTypeEnum.Scan)       // if scan, see if it needs to be displayed
            {
                ShowScanData(he.journalEntry as EliteDangerous.JournalEvents.JournalScan);
            }
            else
                HideScanData(null,null);
        }
#endif

        public override void Closing()
        {
            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= NewEntry;
            discoveryform.OnNewTarget -= NewTarget;

#if TH
            travelhistorycontrol.OnTravelSelectionChanged -= Travelhistorycontrol_OnTravelSelectionChanged;
#endif

            SQLiteDBClass.PutSettingInt(DbSave+"Config", (int)config);
            SQLiteDBClass.PutSettingInt(DbSave + "Layout", layoutorder);
            string s = string.Join<int>(",", columnpos);
            SQLiteDBClass.PutSettingString("SummaryPanelTabs", s);
        }

        Color transparencycolor = Color.Green;
        public override Color ColorTransparency { get { return transparencycolor; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
        }

        private void UserControlSpanel_Resize(object sender, EventArgs e)
        {
            if ( !inresizeduetoexpand && !IsInTemporaryResize && this.Width>0)
                Display(current_historylist);
        }


#region Display
        public void Display(HistoryList hl)            // when user clicks around..  HE may be null here
        {
            if (hl == null)     // just for safety
                return;

            current_historylist = hl;

            int ftotal;         // event filter
            List<HistoryEntry> result = current_historylist.LastFirst;      // Standard filtering

            result = HistoryList.FilterByJournalEvent(result, SQLiteDBClass.GetSettingString(DbFilterSave, "All"), out ftotal);
            
            result = fieldfilter.FilterHistory(result, out ftotal); // and the field filter..

            pictureBox.ClearImageList();

            RevertToNormalSize();                                           // ensure size is back to normal..
            scanpostextoffset = new Point(0, 0);                            // left/ top used by scan display

            bool drawnnootherstuff = DrawScanText(true);                    // go 1 for some of the scan positions

            if (!drawnnootherstuff)                                         // and it may indicate its overwriting all stuff, which is fine
            {
                int rowpos = scanpostextoffset.Y;

                if (Config(Configuration.showNothingWhenDocked) && (hl.GetLast.IsDocked || hl.GetLast.IsLanded))
                {
                    AddColText(0, 1, rowpos, (hl.GetLast.IsDocked) ? "Docked" : "Landed", null);
                }
                else
                {
                    string name;
                    Point3D tpos;
                    bool targetpresent = TargetClass.GetTargetPosition(out name, out tpos);

                    if (targetpresent && Config(Configuration.showTargetLine))
                    {
                        AddColText(0, 1, rowpos, "Target", null);
                        AddColText(1, 2, rowpos, name, null);
                        string dist = (hl.GetLast.System.HasCoordinate) ? SystemClass.Distance(hl.GetLast.System, tpos.X, tpos.Y, tpos.Z).ToString("0.00") : "Unknown";
                        AddColText(2, 3, rowpos, dist, null);
                        rowpos += rowheight;
                    }

                    foreach (HistoryEntry rhe in result)
                    {
                        DrawHistoryEntry(rhe, rowpos, tpos);
                        rowpos += rowheight;

                        if (rowpos > ClientRectangle.Height)                // stop when off of screen
                            break;
                    }
                }
            }

            DrawScanText(false);     // go 2
            
            pictureBox.Render();
        }

        void DrawHistoryEntry(HistoryEntry he, int rowpos, Point3D tpos)
        {
            List<string> coldata = new List<string>();                      // First we accumulate the strings
            List<int> tooltipattach = new List<int>();

            if (Config(Configuration.showTime))
                coldata.Add((EDDiscoveryForm.EDDConfig.DisplayUTC ? he.EventTimeUTC : he.EventTimeLocal).ToString("HH:mm.ss"));

            if (Config(Configuration.showDescription))
            {
                tooltipattach.Add(coldata.Count);
                coldata.Add(he.EventSummary.Replace("\r\n", " "));
            }

            if (Config(Configuration.showInformation))
            {
                tooltipattach.Add(coldata.Count);
                coldata.Add(he.EventDescription.Replace("\r\n", " "));
            }

            if (layoutorder == 0 && Config(Configuration.showNotes))
            {
                SystemNoteClass snc = SystemNoteClass.GetNoteOnJournalEntry(he.Journalid);
                if (snc == null && he.IsFSDJump)
                    snc = SystemNoteClass.GetNoteOnSystem(he.System.name, he.System.id_edsm);

                coldata.Add((snc != null) ? snc.Note.Replace("\r\n", " ") : "");
            }

            bool showdistance = !Config(Configuration.showDistancesOnFSDJumpsOnly) || he.IsFSDJump;

            if (layoutorder == 2 && Config(Configuration.showDistancePerStar))
                coldata.Add(showdistance ? DistToStar(he, tpos) : "");

            if (Config(Configuration.showXYZ))
            {
                coldata.Add((he.System.HasCoordinate && showdistance) ? he.System.x.ToString("0.00") : "");
                coldata.Add((he.System.HasCoordinate && showdistance) ? he.System.y.ToString("0.00") : "");
                coldata.Add((he.System.HasCoordinate && showdistance) ? he.System.z.ToString("0.00") : "");
            }

            if (layoutorder > 0 && Config(Configuration.showNotes))
            {
                SystemNoteClass snc = SystemNoteClass.GetNoteOnJournalEntry(he.Journalid);
                if (snc == null && he.IsFSDJump)
                    snc = SystemNoteClass.GetNoteOnSystem(he.System.name, he.System.id_edsm);

                coldata.Add((snc != null) ? snc.Note.Replace("\r\n", " ") : "");
            }

            if (layoutorder < 2 && Config(Configuration.showDistancePerStar))
                coldata.Add(showdistance ? DistToStar(he, tpos) : "");

            int colnum = 0;

            if (Config(Configuration.showEDSMButton))
            {
                Image edsm = EDDiscovery.Properties.Resources.edsm;
                pictureBox.AddImage(new Rectangle(scanpostextoffset.X+columnpos[colnum++], rowpos, edsm.Width, edsm.Height), edsm, he, "Click to view information on EDSM");
            }

            string tooltip = he.EventSummary + Environment.NewLine + he.EventDescription + Environment.NewLine + he.EventDetailedInfo;

            for (int i = 0; i < coldata.Count; i++)             // then we draw them, allowing them to overfill columns if required
            {
                int nextfull = i+1;
                for (; nextfull < coldata.Count && Config(Configuration.showExpandOverColumns) && coldata[nextfull].Length == 0; nextfull++)
                { }

                AddColText(colnum + i, colnum + nextfull , rowpos, coldata[i], tooltipattach.Contains(i) ? tooltip : null);
            }
        }

        public bool DrawScanText(bool attop)
        {
            Size maxscansize = new Size(1920, 1080);            // set arbitary large.. not important for this.

            if (scantext != null)
            {
                if (attop)
                {
                    if (Config(Configuration.showScanLeft))
                    {
                        PictureBoxHotspot.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(4, 0), maxscansize, scantext, displayfont, textcolour, Config(Configuration.showBlackBoxAroundText) ? Color.Black : Color.Transparent, 1.0F, "SCAN");
                        scanpostextoffset = new Point(4 + scanimg.img.Width + 4, 0);
                        RequestTemporaryMinimumSize(new Size(scanimg.img.Width + 8, scanimg.img.Height + 4));
                    }
                    else if (Config(Configuration.showScanAbove))
                    {
                        PictureBoxHotspot.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(4, 0), maxscansize, scantext, displayfont, textcolour, Config(Configuration.showBlackBoxAroundText) ? Color.Black : Color.Transparent, 1.0F, "SCAN");
                        scanpostextoffset = new Point(0, scanimg.img.Height + 4);
                        RequestTemporaryResizeExpand(new Size(0, scanimg.img.Height + 4));
                    }
                    else if (Config(Configuration.showScanOnTop))
                    {
                        PictureBoxHotspot.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(4, 0), maxscansize, scantext, displayfont, textcolour, Config(Configuration.showBlackBoxAroundText) ? Color.Black : Color.Transparent, 1.0F, "SCAN");
                        RequestTemporaryResize(new Size(scanimg.img.Width + 8, scanimg.img.Height + 4));        // match exactly to use minimum space
                        return true;
                    }
                }
                else // bottom chance
                {
                    if (Config(Configuration.showScanRight))
                    {
                        Size s = pictureBox.DisplaySize();
                        PictureBoxHotspot.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(s.Width + 4, 0), maxscansize, scantext, displayfont, textcolour, Config(Configuration.showBlackBoxAroundText) ? Color.Black : Color.Transparent, 1.0F, "SCAN");
                        RequestTemporaryMinimumSize(new Size(s.Width+4+scanimg.img.Width + 8, scanimg.img.Height + 4));
                    }
                    else if (Config(Configuration.showScanBelow))
                    {
                        Size s = pictureBox.DisplaySize();
                        PictureBoxHotspot.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(4, s.Height + 4), maxscansize, scantext, displayfont, textcolour, Config(Configuration.showBlackBoxAroundText) ? Color.Black : Color.Transparent, 1.0F, "SCAN");
                        RequestTemporaryResizeExpand(new Size(0, scanimg.img.Height + 4));
                    }
                }
            }

            return false;
        }

        void AddColText(int coli, int nextcol , int rowpos, string text, string tooltip)
        {
            if (text.Length > 0)            // don't place empty text, do not want image handling to work on blank screen
            {
                pictureBox.AddTextAutoSize(new Point(scanpostextoffset.X + columnpos[coli], rowpos),
                                new Size(columnpos[nextcol] - columnpos[coli] - 4, rowheight),
                                text, displayfont, textcolour,
                                Config(Configuration.showBlackBoxAroundText) ? Color.Black : Color.Transparent, 1.0F,
                                null, tooltip);
            }
        }

        private string DistToStar(HistoryEntry he, Point3D tpos)
        {
            string res = "";
            if (!double.IsNaN(tpos.X))
            {
                double dist = SystemClass.Distance(he.System, tpos.X, tpos.Y, tpos.Z);
                if (dist >= 0)
                    res = dist.ToString("0.00");
            }

            return res;
        }

        public void NewTarget()
        {
            System.Diagnostics.Debug.WriteLine("spanel Refresh target display");
            Display(current_historylist);
        }

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made..
        {
            bool add = WouldAddEntry(he);

            if (add)
                Display(hl);

            if (he.journalEntry.EventTypeID == EliteDangerous.JournalTypeEnum.Scan)       // if scan, see if it needs to be displayed
            {
                ShowScanData(he.journalEntry as EliteDangerous.JournalEvents.JournalScan);
            }
        }

        public bool WouldAddEntry(HistoryEntry he)                  // do we filter? if its not in the journal event filter, or it is in the field filter
        {
            return he.IsJournalEventInEventFilter(SQLiteDBClass.GetSettingString(DbFilterSave, "All")) && fieldfilter.FilterHistory(he);
        }

        #endregion

        #region Clicks

        private void pictureBox_ClickElement(object sender, MouseEventArgs e, PictureBoxHotspot.ImageElement i, object tag)
        {
            if (i != null)
            {
                string stag = tag as string;
                HistoryEntry he = tag as HistoryEntry;

                if (stag != null)      // its SCAN for now
                {
                    HideScanData(null, null);
                }
                else if (he != null)
                {
                    EDDiscovery2.EDSM.EDSMClass edsm = new EDDiscovery2.EDSM.EDSMClass();

                    string url = edsm.GetUrlToEDSMSystem(he.System.name);

                    if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                        System.Diagnostics.Process.Start(url);
                    else
                        MessageBox.Show("System " + he.System.name + " unknown to EDSM");
                }
            }
            else
                System.Diagnostics.Debug.WriteLine("Clicked at " + e.X + " " + e.Y );
        }

#endregion

#region Positioning

        void ResetTabList()                             // work out optimum tab spacing by what is selected
        {
            columnpos = new List<int>();
            int visiblecolwidth = 4;

            int pos = 4;
            columnpos.Add(pos);

            if (Config(Configuration.showEDSMButton))       // mirrors UpdateRow
            {
                columnpos.Add(pos += 60);
                visiblecolwidth += 60;
            }

            if (Config(Configuration.showTime))
            {
                columnpos.Add(pos += 80);
                visiblecolwidth += 80;
            }

            if (Config(Configuration.showDescription))
            {
                columnpos.Add(pos += 200);
                visiblecolwidth += 200;
            }

            if (Config(Configuration.showInformation))
            {
                columnpos.Add(pos += 200);
                visiblecolwidth += 200;
            }

            if (layoutorder == 0 && Config(Configuration.showNotes))
            {
                columnpos.Add(pos += 200);
                visiblecolwidth += 200;
            }

            if (layoutorder == 2 && Config(Configuration.showDistancePerStar))
            {
                columnpos.Add(pos += 60);
                visiblecolwidth += 60;
            }

            if (Config(Configuration.showXYZ))
            {
                columnpos.Add(pos += 60);
                columnpos.Add(pos += 50);
                columnpos.Add(pos += 60);
                visiblecolwidth += 170;
            }

            if (layoutorder > 0 && Config(Configuration.showNotes))
            {
                columnpos.Add(pos += 200);
                visiblecolwidth += 200;
            }

            if (layoutorder < 2 && Config(Configuration.showDistancePerStar))
            {
                columnpos.Add(pos += 60);
                visiblecolwidth += 60;
            }

            while (columnpos.Count < 4)                                         // need a minimum of 4 columns for target info
                columnpos.Add(columnpos[columnpos.Count - 1] + 100);

        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (dividercapture == -2 && e.Y < 24)
                ShowDividers(true);
            else if (dividercapture == -1 && e.Y >= 24)
                ShowDividers(false);
        }

        private void ShowDividers(bool show)
        {
            foreach (ButtonExt p in dividers)
                p.Visible = false;

            //System.Diagnostics.Debug.WriteLine("Dividers " + show);
            if (show)
            {
                dividercapture = -1;

                for (int i = 1; i < columnpos.Count; i++)              // bring up the number of dividers needed
                {
                    ButtonExt b = dividers[i - 1];
                    b.Location = new Point(scanpostextoffset.X + columnpos[i] - b.Width/2, 0);
                    b.ButtonColorScaling = 1.0F;
                    b.FlatAppearance.BorderColor = dividers[i - 1].BackColor;
                    b.Visible = true;
                }

                dividercheck.Start();
            }
            else
            {
                dividercapture = -2;
                dividercheck.Stop();
            }
        }

        private void DividerCheck(object sender, EventArgs e)       // run at intervals to see if mouse beyond bounds.. can't catch all the leave events in all cirumstances
        {
            if (dividercapture == -1 && !ClientRectangle.Contains(this.PointToClient(MousePosition)))
            {
                ShowDividers(false);
            }
        }

        private void divider_MouseDown(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            dividercapture = int.Parse((string)b.Tag);
            b.Capture = true;
            divideroriginalxpos = e.X;
            //System.Diagnostics.Debug.WriteLine("Button " + dividercapture +" mouse down");
        }

        private void divider_MouseUp(object sender, MouseEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Button " + dividercapture + " mouse up");
            Button b = sender as Button;
            dividercapture = -1;
            b.Capture = false;
            Display(current_historylist);
        }

        private void divider_MouseMove(object sender, MouseEventArgs e)
        {
            if (dividercapture >= 0)
            {
                int colpos = dividercapture + 1;        // because divider 0 is at col pos 1
                Button b = sender as Button;
                int off = e.X - divideroriginalxpos;

                if (columnpos[colpos] + off - columnpos[colpos - 1] >= 20)         // ensure can't get too close to previous one
                {
                    for (int i = colpos; i < columnpos.Count; i++)          // shift this and ones to right..
                    {
                        dividers[i - 1].Location = new Point(dividers[i - 1].Location.X + off, dividers[i - 1].Location.Y);
                        columnpos[i] += off;
                        dividers[i - 1].Invalidate();
                    }

                }
                //System.Diagnostics.Debug.WriteLine("Capture " + dividercapture + " at " + e.X + "," + off + "," + off + " to " + b.Location);
            }
        }

#endregion

#region Scan Data

        public void ShowScanData(EDDiscovery.EliteDangerous.JournalEvents.JournalScan scan)
        {
            if ( IsSurfaceScanOn )
            {
                scantext = scan.DisplayString();
                Display(current_historylist);
                SetSurfaceScanBehaviour(null);  // set up timers etc.
            }
        }

        private void HideScanData(object sender, EventArgs e)
        {
            if (scantext != null)
            {
                scanhide.Stop();
                scantext = null;
                Display(current_historylist);
            }
        }

#endregion

#region Config

        private void toolStripMenuItemTargetLine_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showTargetLine, ((ToolStripMenuItem)sender).Checked, true);
        }
    
        private void EDSMButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showEDSMButton, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void toolStripMenuItemTime_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showTime, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void showDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showDescription, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void showInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showInformation, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void showNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showNotes, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void showXYZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showXYZ, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void showTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showDistancePerStar, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void blackBoxAroundTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showBlackBoxAroundText, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void showDistancesOnFSDJumpsOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showDistancesOnFSDJumpsOnly, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void showNothingWhenDockedtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showNothingWhenDocked, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void expandTextOverEmptyColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showExpandOverColumns, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLayoutOrder(0, true);
        }

        private void notesAfterXYZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLayoutOrder(1, true);
        }

        private void targetDistanceXYZNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLayoutOrder(2, true);
        }


        private void scanNoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSurfaceScanBehaviour(Configuration.showScanOff);
        }

        private void scan15sToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSurfaceScanBehaviour(Configuration.showScan15s);
        }

        private void scan30sToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSurfaceScanBehaviour(Configuration.showScan30s);
        }

        private void scan60sToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSurfaceScanBehaviour(Configuration.showScan60s);
        }

        private void scanUntilNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSurfaceScanBehaviour(Configuration.showScanIndefinite);
        }


        private void scanRightMenuItem_Click(object sender, EventArgs e)
        {
            SetScanPosition(Configuration.showScanRight);
        }

        private void scanLeftMenuItem_Click(object sender, EventArgs e)
        {
            SetScanPosition(Configuration.showScanLeft);
        }

        private void scanAboveMenuItem_Click(object sender, EventArgs e)
        {
            SetScanPosition(Configuration.showScanAbove);
        }

        private void scanBelowMenuItem_Click(object sender, EventArgs e)
        {
            SetScanPosition(Configuration.showScanBelow);
        }

        private void scanOnTopMenuItem_Click(object sender, EventArgs e)
        {
            SetScanPosition(Configuration.showScanOnTop);
        }

        private void configureEventFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point p = MousePosition;
            cfs.FilterButton(DbFilterSave, contextMenuStripConfig.PointToScreen(new Point(0, 0)), new Size(180,400), 
                             discoveryform.theme.TextBackColor, discoveryform.theme.TextBlockColor, this.FindForm());
        }

        private void EventFilterChanged(object sender, EventArgs e)
        {
            Display(current_historylist);
        }

        private void configureFieldFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EDDiscovery2.JSONFiltersForm frm = new EDDiscovery2.JSONFiltersForm();
            frm.Init("Summary Panel: Filter out fields", "Filter Out", true, discoveryform.theme, fieldfilter);
            frm.TopMost = this.FindForm().TopMost;
            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                fieldfilter = frm.result;
                SQLiteDBClass.PutSettingString(DbFieldFilter, fieldfilter.GetJSON());
                Display(current_historylist);
            }
        }

        void FlipConfig(Configuration item, bool ch , bool redisplay = false)
        {
            if (ch)
                config = (Configuration)((int)config | (int)item);
            else
                config = (Configuration)((int)config & ~(int)item);

            if (item < Configuration.showDoesNotAffectTabs)
            {
                ResetTabList();
                ShowDividers(false);
            }

            if (redisplay)
                Display(current_historylist);
        }

        void SetLayoutOrder(int n , bool refresh = false)
        {
            layoutorder = n;
            orderDefaultToolStripMenuItem.Checked = layoutorder == 0;
            orderNotesAfterXYZToolStripMenuItem.Checked = layoutorder == 1;
            orderTargetDistanceXYZNotesToolStripMenuItem.Checked = layoutorder == 2;

            ResetTabList();

            if (refresh)
            {
                ShowDividers(false);
                Display(current_historylist);
            }
        }

        private void SetSurfaceScanBehaviour(Configuration? itemClicked)    // pass in a 
        {
            if (itemClicked.HasValue)
            {
                FlipConfig(Configuration.showScan15s, itemClicked == Configuration.showScan15s);
                FlipConfig(Configuration.showScan30s, itemClicked == Configuration.showScan30s);
                FlipConfig(Configuration.showScan60s, itemClicked == Configuration.showScan60s);
                FlipConfig(Configuration.showScanIndefinite, itemClicked == Configuration.showScanIndefinite);
            }

            scanNoToolStripMenuItem.Checked = !IsSurfaceScanOn;
            scan15sToolStripMenuItem.Checked = Config(Configuration.showScan15s);
            scan30sToolStripMenuItem.Checked = Config(Configuration.showScan30s);
            scan60sToolStripMenuItem.Checked = Config(Configuration.showScan60s);
            scanUntilNextToolStripMenuItem.Checked = Config(Configuration.showScanIndefinite);

            scanhide.Stop();

            if (Config(Configuration.showScan15s)) scanhide.Interval = 15000;
            else if (Config(Configuration.showScan30s)) scanhide.Interval = 30000;
            else if (Config(Configuration.showScan60s)) scanhide.Interval = 60000;
            else scanhide.Interval = int.MaxValue;  // i know its not infinite, but are we going to be having it open this long, and it saves code !

            if (!IsSurfaceScanOn)
                HideScanData(null, null);
            else if ( scantext != null )
                scanhide.Start();
        }

        private void SetScanPosition(Configuration? position)
        {
            if (position.HasValue)
            {
                FlipConfig(Configuration.showScanLeft,  position.Value == Configuration.showScanLeft);
                FlipConfig(Configuration.showScanRight, position.Value == Configuration.showScanRight);
                FlipConfig(Configuration.showScanOnTop, position.Value == Configuration.showScanOnTop);
                FlipConfig(Configuration.showScanBelow, position.Value == Configuration.showScanBelow);
                FlipConfig(Configuration.showScanAbove, position.Value == Configuration.showScanAbove);
            }

            scanRightMenuItem.Checked = Config(Configuration.showScanRight);
            scanLeftMenuItem.Checked = Config(Configuration.showScanLeft);
            scanOnTopMenuItem.Checked = Config(Configuration.showScanOnTop);
            scanBelowMenuItem.Checked = Config(Configuration.showScanBelow);
            scanAboveMenuItem.Checked = Config(Configuration.showScanAbove);

            if ( scantext!=null)
            {
                Display(current_historylist);
            }
        }


#endregion

    }
}
