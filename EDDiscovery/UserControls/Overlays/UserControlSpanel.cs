/*
 * Copyright © 2016 - 2021 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using BaseUtils;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using EMK.LightGeometry;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSpanel : UserControlCommonBase
    {
        private string dbFilter = "EventFilter2";
        private string dbFieldFilter = "FieldFilter";

        private JournalFilterSelector cfs;
        private ConditionLists fieldfilter = new ConditionLists();

        private Timer scanhide = new Timer();
        string scantext = null;             // if set, display this text at the right place.
        Point scanpostextoffset = new Point(0, 0); // left/ top used by scan display

        private List<int> columnpos;

        private Font displayfont;
		
        HistoryList current_historylist;

        private Timer dividercheck = new Timer();
        private ExtButton[] dividers;
        private int dividercapture = -2;        //-2 not shown, -1 shown, >=0 captured
        private int divideroriginalxpos = -1;

        private int layoutorder = 0;

        private int startingtextrowpos = 0;

        private EliteDangerousCore.UIEvents.UIGUIFocus.Focus uistate = EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus;

        class Configuration         
        {
            // general controls (note numbers are historical in the order they were first added)

            public const long showSystemInformation = 1L << 32;
            public const long showTargetLine = 2048;
            public const long showBlackBoxAroundText = 4096;
            public const long showExpandOverColumns = 8192;
            public const long showNothingWhenDocked = 16384;
            public const long showNothingWhenPanel = 1L << 34;
            public const long showNoTitleWhenHidden = 1L << 36;

            // column control

            public const long showEDSMButton = 2;
            public const long showTime = 4;
            public const long showIcon = 128;
            public const long showDescription = 8;
            public const long showInformation = 1;
            public const long showNotes = 16;
            public const long showXYZ = 32;
            public const long showDistancePerStar = 64;
            public const long showDistancesOnFSDJumpsOnly = 1024;

            // zone info

            public const long showHabInformation = 1L << 33;
            public const long showMetalRichZone = 1L << 40;
            public const long showWaterWrldZone = 1L << 41;
            public const long showEarthLikeZone = 1L << 42;
            public const long showAmmonWrldZone = 1L << 43;
            public const long showIcyPlanetZone = 1L << 44;

            // scan show control

            public const long showScanOff = 0;                    
            public const long showScan15s = 32768;
            public const long showScan30s = 65536;
            public const long showScan60s = 131072;
            public const long showScanIndefinite = 262144;

            // scan position control
            public const long showScanRight = 524288;
            public const long showScanLeft = 1048576;
            public const long showScanOnTop = 2097152;
            public const long showScanBelow = 4194304;
            public const long showScanAbove = 8388608;
		}

        private long config = Configuration.showTargetLine | Configuration.showEDSMButton | Configuration.showIcon | 
                                                Configuration.showTime | Configuration.showDescription |
                                               Configuration.showInformation | Configuration.showNotes | 
                                                Configuration.showXYZ | Configuration.showDistancePerStar |
                                               Configuration.showScan15s | Configuration.showSystemInformation |
                                               Configuration.showScanRight;
        private bool Config(long c) { return (config & c) != 0; }
        private bool IsSurfaceScanOn { get { return Config(Configuration.showScan15s) || Config(Configuration.showScan30s) || Config(Configuration.showScan60s) || Config(Configuration.showScanIndefinite); } }


        #region Initialisation

        public UserControlSpanel()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "SPanel";

            config = (long)(GetSetting("Config", (int)config)) | ((long)(GetSetting("ConfigH", (int)(config >> 32))) << 32);
            System.Diagnostics.Debug.WriteLine($"spanel config start {config}");

            SetSurfaceScanBehaviour();
            SetLayoutOrder(GetSetting("Layout", layoutorder),false);  // also resets the tab order

            scanhide.Tick += HideScanData;

            dividercheck.Tick += DividerCheck;
            dividercheck.Interval = 500;

            string tabs = GetSetting("PanelTabs", "");

            if (tabs.HasChars())
            {
                try
                {
                    List<int> tablist = tabs.Split(',').Select(int.Parse).ToList();

                    for (int i = 0; i < tablist.Count && i < columnpos.Count; i++)      // for what we have, and not more than pre-populated, fill
                        columnpos[i] = tablist[i];
                }
                catch { }
            }

            displayfont = FontHelpers.GetFont(GetSetting("font", ""), null);

            string filter = GetSetting(dbFieldFilter, "");
            if (filter.Length > 0)
                fieldfilter.FromJSON(filter);        // load filter

            cfs = new JournalFilterSelector();
            cfs.AddAllNone();
            cfs.AddJournalExtraOptions();
            cfs.AddJournalEntries();
            cfs.SaveSettings += EventFilterChanged;

            extCheckBoxWordWrap.Checked = GetSetting("wordwrap", false);
            extCheckBoxWordWrap.Click += wordWrapToolStripMenuItem_Click;

            dividers = new ExtButton[] { buttonExt0, buttonExt1, buttonExt2, buttonExt3, buttonExt4, buttonExt5, buttonExt6, buttonExt7, buttonExt8, buttonExt9, buttonExt10, buttonExt11, buttonExt12 };

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += NewEntry;
            discoveryform.OnNewTarget += NewTarget;
            discoveryform.OnNewUIEvent += OnNewUIEvent;

            BaseUtils.Translator.Instance.Translate(toolTip, this);

            rollUpPanelTop.PinState = GetSetting("PinState", true);
            
             //scantext = "kwkwkwkw qwkqwkqw qw qwkqwjjqw qwjkqwjqwj wjwhghe wwjsjsjsjw.\r\nwkwkwkwk\r\nwjqjqjw ro"; // for debug
        }

        public override void Closing()
        {
            dividercheck.Stop();
            scanhide.Stop();

            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= NewEntry;
            discoveryform.OnNewTarget -= NewTarget;
            discoveryform.OnNewUIEvent -= OnNewUIEvent;
            scanhide.Tick -= HideScanData;
            dividercheck.Tick -= DividerCheck;
            scanhide.Dispose();
            dividercheck.Dispose();

            PutSetting("PinState", rollUpPanelTop.PinState);
            PutSetting("Config", (int)config);
            PutSetting("ConfigH", (int)(config>>32));
            PutSetting("Layout", layoutorder);
            string s = string.Join<int>(",", columnpos);
            PutSetting("PanelTabs", s);
        }

        public override bool SupportTransparency { get { return true; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            rollUpPanelTop.Visible = !on;
            Display(current_historylist);
        }

        private void UserControlSpanel_Resize(object sender, EventArgs e)
        {
            if ( !ResizingNow && !IsInTemporaryResize && this.Width>0)
                Display(current_historylist);
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            Display(discoveryform.history);
        }

        private async void Display(HistoryList hl)            
        {
            pictureBox.ClearImageList();

            current_historylist = hl;

            if (hl != null && hl.Count > 0)     // just for safety
            {
                List<HistoryEntry> result = current_historylist.LatestFirst();      // Standard filtering

                int ftotal;         // event filter
                result = HistoryList.FilterByJournalEvent(result, GetSetting(dbFilter, "All"), out ftotal);
                result = HistoryFilterHelpers.FilterHistory(result, fieldfilter , discoveryform.Globals, out ftotal); // and the field filter..

                RevertToNormalSize();                                           // ensure size is back to normal..
                scanpostextoffset = new Point(0, 0);                            // left/ top used by scan display

                Font dfont = displayfont ?? this.Font;
                Color textcolour = IsTransparent ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
                Color backcolour = IsTransparent ? (Config(Configuration.showBlackBoxAroundText) ? Color.Black : Color.Transparent) : this.BackColor;

                bool drawnnootherstuff = DrawScanText(true, textcolour, backcolour, dfont);                    // go 1 for some of the scan positions

                if (!drawnnootherstuff)                                         // and it may indicate its overwriting all stuff, which is fine
                {
                    int rowpos = scanpostextoffset.Y;
                    int rowmargin = dfont.ScalePixels(4);

                    // Check if need to hide the UI
                    var ts = hl.CurrentTravelState();

                    if (Config(Configuration.showNothingWhenDocked) && (ts == HistoryEntryStatus.TravelStateType.Docked))
                    {
						if (!Config(Configuration.showNoTitleWhenHidden))
						{
							AddColText(0, 0, rowpos, "<>",textcolour, backcolour, dfont, null);        // just show a marker
						}
					}
                    else if ( uistate != EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus && Config(Configuration.showNothingWhenPanel))
                    {
						if (!Config(Configuration.showNoTitleWhenHidden))
						{
							AddColText(0, 0, rowpos, uistate.ToString().SplitCapsWord(),
									   textcolour, backcolour, dfont, null);
						}
					}
                    else
                    {
                        string name;
                        Point3D tpos;
                        bool targetpresent = TargetClass.GetTargetPosition(out name, out tpos);

                        ISystem currentsystem = hl.CurrentSystem(); // may be null

                        HistoryEntry last = hl.GetLast;

                      // last = hl.FindByName("Myeia Thaa QY-H c23-0");

                        if (Config(Configuration.showSystemInformation) && last != null)
                        {
                            string allegiance, economy, gov, faction, factionstate, security;
                            hl.ReturnSystemInfo(last, out allegiance, out economy, out gov, out faction, out factionstate, out security);

                            string str = last.System.Name + " : " + BaseUtils.FieldBuilder.Build(
                                "", faction,
                                "", factionstate,
                                "", security,
                                "", allegiance,
                                "", economy,
                                "", gov
                                );


                            HistoryEntry lastfsd = hl.GetLastHistoryEntry(x => x.journalEntry is EliteDangerousCore.JournalEvents.JournalFSDJump, last);
                            bool firstdiscovery = (lastfsd != null && (lastfsd.journalEntry as EliteDangerousCore.JournalEvents.JournalFSDJump).EDSMFirstDiscover);

                            rowpos = rowmargin + AddColText(0, 0, rowpos, str, textcolour, backcolour, dfont, null, firstdiscovery ? EDDiscovery.Icons.Controls.firstdiscover : null, "Shows if EDSM indicates your it's first discoverer").Location.Bottom;
                        }

                        var zoneson = Config(Configuration.showHabInformation | Configuration.showMetalRichZone | Configuration.showWaterWrldZone |
                                              Configuration.showEarthLikeZone | Configuration.showAmmonWrldZone | Configuration.showIcyPlanetZone);

                        if (zoneson && last != null)
                        {
                            StarScan scan = hl.StarScan;

                            StarScan.SystemNode sn = await scan.FindSystemAsync(last.System, true);    // EDSM look up here..

                            StringBuilder res = new StringBuilder();

                            if (sn != null && sn.StarNodes.Count > 0 && sn.StarNodes.Values[0].ScanData != null)
                            {
                                JournalScan js = sn.StarNodes.Values[0].ScanData;

                                if (Config(Configuration.showHabInformation))
                                {
                                    string hz = js.CircumstellarZonesString(false, JournalScan.CZPrint.CZHab);
                                    res.AppendFormat(hz + Environment.NewLine);
                                }

                                if (Config(Configuration.showMetalRichZone))
                                {
                                    string hz = js.CircumstellarZonesString(false, JournalScan.CZPrint.CZMR);
                                    res.AppendFormat(hz + Environment.NewLine);
                                }

                                if (Config(Configuration.showWaterWrldZone))
                                {
                                    string hz = js.CircumstellarZonesString(false, JournalScan.CZPrint.CZWW);
                                    res.AppendFormat(hz + Environment.NewLine);
                                }

                                if (Config(Configuration.showEarthLikeZone))
                                {
                                    string hz = js.CircumstellarZonesString(false, JournalScan.CZPrint.CZEL);
                                    res.AppendFormat(hz + Environment.NewLine);
                                }

                                if (Config(Configuration.showAmmonWrldZone))
                                {
                                    string hz = js.CircumstellarZonesString(false, JournalScan.CZPrint.CZAW);
                                    res.AppendFormat(hz + Environment.NewLine);
                                }

                                if (Config(Configuration.showIcyPlanetZone))
                                {
                                    string hz = js.CircumstellarZonesString(false, JournalScan.CZPrint.CZIP);
                                    res.AppendFormat(hz + Environment.NewLine);
                                }
                            }

                            if (res.ToString().HasChars())
                            {
                                rowpos = rowmargin + AddColText(0, 0, rowpos, res.ToString(), textcolour, backcolour, dfont, null).Location.Bottom;
                            }
                        }

                        if (targetpresent && Config(Configuration.showTargetLine) && currentsystem != null)
                        {
                            string dist = (currentsystem.HasCoordinate) ? currentsystem.Distance(tpos.X, tpos.Y, tpos.Z).ToString("0.00") : "Unknown".T(EDTx.Unknown);
                            rowpos = rowmargin + AddColText(0, 0, rowpos, "Target".T(EDTx.UserControlSpanel_Target) + ": " + name + " @ " + dist +" ly", textcolour, backcolour, dfont, null).Location.Bottom;
                        }

                        startingtextrowpos = rowpos;

                        foreach (HistoryEntry rhe in result)
                        {
                            rowpos = rowmargin + DrawHistoryEntry(rhe, rowpos, tpos, textcolour, backcolour, dfont );

                            if (rowpos > ClientRectangle.Height)                // stop when off of screen
                                break;
                        }
                    }
                }

                DrawScanText(false, textcolour, backcolour, dfont);     // go 2
            }

            pictureBox.Render();
        }

        int DrawHistoryEntry(HistoryEntry he, int rowpos, Point3D tpos , Color textcolour , Color backcolour, Font dfont )
        {
            List<string> coldata = new List<string>();                      // First we accumulate the strings
            List<int> tooltipattach = new List<int>();

            int initialrowpos = rowpos;
            int maxrowpos = rowpos;

            bool showiffsdorconfigall = !Config(Configuration.showDistancesOnFSDJumpsOnly) || he.IsFSDCarrierJump;  // if this is off, or its a carrier jump

            if (Config(Configuration.showTime))
                coldata.Add(EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC).ToString("HH:mm.ss"));

            if (Config(Configuration.showIcon))
                coldata.Add("`!!ICON!!");                // dummy place holder..

            he.FillInformation(out string EventDescription, out string EventDetailedInfo);

            if (Config(Configuration.showDescription))
            {
                tooltipattach.Add(coldata.Count);
                coldata.Add(he.EventSummary.Replace("\r\n", " "));
            }

            if (Config(Configuration.showInformation))
            {
                tooltipattach.Add(coldata.Count);
                coldata.Add(EventDescription.Replace("\r\n", " "));
            }

            if (layoutorder == 0 && Config(Configuration.showNotes))
            {
                coldata.Add((he.SNC != null) ? he.SNC.Note.Replace("\r\n", " ") : "");
            }


            if (layoutorder == 2 && Config(Configuration.showDistancePerStar))
                coldata.Add(showiffsdorconfigall ? DistToStar(he, tpos) : "");

            if (Config(Configuration.showXYZ))
            {
                coldata.Add((he.System.HasCoordinate && showiffsdorconfigall) ? he.System.X.ToString("0.00") : "");
                coldata.Add((he.System.HasCoordinate && showiffsdorconfigall) ? he.System.Y.ToString("0.00") : "");
                coldata.Add((he.System.HasCoordinate && showiffsdorconfigall) ? he.System.Z.ToString("0.00") : "");
            }

            if (layoutorder > 0 && Config(Configuration.showNotes))
            {
                coldata.Add((he.SNC != null) ? he.SNC.Note.Replace("\r\n", " ") : "");
            }

            if (layoutorder < 2 && Config(Configuration.showDistancePerStar))
                coldata.Add(showiffsdorconfigall ? DistToStar(he, tpos) : "");

            int colnum = 0;

            ExtendedControls.ExtPictureBox.ImageElement edsm = null;

            if (Config(Configuration.showEDSMButton) )
            {
                if (showiffsdorconfigall)
                {
                    Color backtext = (backcolour.IsFullyTransparent()) ? Color.Black : backcolour;

                    edsm = pictureBox.AddTextAutoSize(new Point(scanpostextoffset.X + columnpos[colnum], rowpos), new Size(200, 200),
                                            "EDSM", dfont, backtext, textcolour, 0.5F, he, "View system on EDSM".T(EDTx.UserControlSpanel_TVE));
                    edsm.SetAlternateImage(BaseUtils.BitMapHelpers.DrawTextIntoAutoSizedBitmap("EDSM", new Size(200, 200), dfont, backtext, textcolour.Multiply(1.2F), 0.5F), edsm.Location, true);
                }

                colnum++;
            }

            string tooltip = he.EventSummary + Environment.NewLine + EventDescription + Environment.NewLine + EventDetailedInfo;

            List<ExtendedControls.ExtPictureBox.ImageElement> items = new List<ExtPictureBox.ImageElement>();

            for (int i = 0; i < coldata.Count; i++)             // then we draw them, allowing them to overfill columns if required
            {
                int nextfull = i+1;
                for (; nextfull < coldata.Count && Config(Configuration.showExpandOverColumns) && coldata[nextfull].Length == 0; nextfull++)
                { }

                if (coldata[i].Equals("`!!ICON!!"))            // marker for ICON..
                {
                    Image img = he.journalEntry.Icon;
                    var e = pictureBox.AddImage(new Rectangle(scanpostextoffset.X + columnpos[colnum + i], rowpos, img.Width, img.Height), img, null, null, false); 
                    maxrowpos = Math.Max(maxrowpos, e.Location.Bottom);
                    items.Add(e);
                }
                else
                {
                    var e = AddColText(colnum + i, colnum + nextfull, rowpos, coldata[i], textcolour, backcolour, dfont, tooltipattach.Contains(i) ? tooltip : null);
                    if (e != null)
                    {
                        maxrowpos = Math.Max(maxrowpos, e.Location.Bottom);
                        items.Add(e);
                    }
                }
            }

            if ( edsm != null )
                edsm.Translate(0, (maxrowpos-initialrowpos - edsm.Image.Height) / 2);    // align to centre of rowh..

            foreach( var e in items )
                e.Translate(0, (maxrowpos-initialrowpos - e.Image.Height) / 2);          // align to centre of rowh..

            return maxrowpos;
        }

        public bool DrawScanText(bool attop, Color textcolour , Color backcolour, Font dfont)
        {
            Size maxscansize = new Size(10000, 10000);            // set arbitary large.. not important for this.

            if (scantext != null)
            {
                using (StringFormat frmt = new StringFormat(extCheckBoxWordWrap.Checked ? 0 : StringFormatFlags.NoWrap))
                {
                    if (attop)
                    {
                        if (Config(Configuration.showScanLeft))
                        {
                            ExtPictureBox.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(4, 0), maxscansize, scantext, dfont, textcolour, backcolour, 1.0F, "SCAN", null, frmt);
                            scanpostextoffset = new Point(4 + scanimg.Image.Width + 4, 0);
                            RequestTemporaryMinimumSize(new Size(scanimg.Image.Width + 8, scanimg.Image.Height + 4));
                        }
                        else if (Config(Configuration.showScanAbove))     // if above, NOT transparent (can't do on top)
                        {
                            ExtPictureBox.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(4, 0), maxscansize, scantext, dfont, textcolour, backcolour, 1.0F, "SCAN", null, frmt);
                            scanpostextoffset = new Point(0, scanimg.Image.Height + 4);
                            RequestTemporaryResizeExpand(new Size(0, scanimg.Image.Height + 4));
                        }
                        else if (Config(Configuration.showScanOnTop))
                        {
                            ExtPictureBox.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(4, 0), maxscansize, scantext, dfont, textcolour, backcolour, 1.0F, "SCAN", null, frmt);

                            if (IsTransparent)        // if transparent, the roll up panel is not visible, we can set the whole size to the text
                                RequestTemporaryResize(new Size(scanimg.Image.Width + 8, scanimg.Image.Height + 4));        // match exactly to use minimum space
                            return true;
                        }
                    }
                    else // bottom chance
                    {
                        if (Config(Configuration.showScanRight))
                        {
                            Size s = pictureBox.DisplaySize();
                            ExtPictureBox.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(s.Width + 4, 0), maxscansize, scantext, dfont, textcolour, backcolour, 1.0F, "SCAN",null, frmt);
                            RequestTemporaryMinimumSize(new Size(s.Width + 4 + scanimg.Image.Width + 8, scanimg.Image.Height + 4));
                        }
                        else if (Config(Configuration.showScanBelow))
                        {
                            Size s = pictureBox.DisplaySize();
                            ExtPictureBox.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(4, s.Height + 4), maxscansize, scantext, dfont, textcolour, backcolour, 1.0F, "SCAN", null, frmt);
                            RequestTemporaryResizeExpand(new Size(0, scanimg.Image.Height + 4));
                        }
                    }
                }
            }

            return false;
        }

        ExtendedControls.ExtPictureBox.ImageElement AddColText(int coli, int nextcol , int rowpos, string text, Color textcolour, Color backcolour, 
                                Font dfont, string tooltip, Image opt = null , string imagetooltip = null)
        {
            if (text.Length > 0)            // don't place empty text, do not want image handling to work on blank screen
            {
                int endpos = (nextcol == 0) ? 1920 : (columnpos[nextcol] - columnpos[coli] - 4);

                int colpos = scanpostextoffset.X + columnpos[coli];

                if (opt != null)
                {
                    pictureBox.AddImage(new Rectangle(colpos, rowpos, 24, 24), Icons.Controls.firstdiscover, null, imagetooltip, false);
                    colpos += 24;
                }

                using (StringFormat frmt = new StringFormat(extCheckBoxWordWrap.Checked ? 0 : StringFormatFlags.NoWrap))
                {
                    ExtendedControls.ExtPictureBox.ImageElement e =
                                    pictureBox.AddTextAutoSize(new Point(colpos, rowpos),
                                    new Size(endpos, 200),
                                    text, dfont, textcolour, backcolour, 1.0F, null, tooltip, frmt);
                    return e;
                }

            }
            else
                return null;
        }

        private void OnNewUIEvent(UIEvent uievent)       // UI event in, see if we want to hide.  UI events come before any onNew
        {
            EliteDangerousCore.UIEvents.UIGUIFocus gui = uievent as EliteDangerousCore.UIEvents.UIGUIFocus;

            if (gui != null)
            {
                bool refresh = gui.GUIFocus != uistate;
                uistate = gui.GUIFocus;

                //System.Diagnostics.Debug.WriteLine("UI event " + obj + " " + uistate + " shown " + shown);
                if (refresh )      
                    Display(current_historylist);
            }
        }

        private string DistToStar(HistoryEntry he, Point3D tpos)
        {
            string res = "";
            if (!double.IsNaN(tpos.X))
            {
                double dist = he.System.Distance(tpos.X, tpos.Y, tpos.Z);
                if (dist >= 0)
                    res = dist.ToString("0.00");
            }

            return res;
        }

        public void NewTarget(Object sender)
        {
            System.Diagnostics.Debug.WriteLine("spanel Refresh target display");
            Display(current_historylist);
        }

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made..
        {
            bool add = WouldAddEntry(he);

            if (add)
                Display(hl);

            if (he.journalEntry.EventTypeID == JournalTypeEnum.Scan)       // if scan, see if it needs to be displayed
            {
                ShowScanData(he.journalEntry as JournalScan);
            }
        }

        public bool WouldAddEntry(HistoryEntry he)                  // do we filter? if its not in the journal event filter, or it is in the field filter
        {
            return he.IsJournalEventInEventFilter(GetSetting(dbFilter, "All")) && HistoryFilterHelpers.FilterHistory(he, fieldfilter , discoveryform.Globals);
        }

#endregion

#region Clicks

        private void pictureBox_ClickElement(object sender, MouseEventArgs e, ExtendedControls.ExtPictureBox.ImageElement i, object tag)
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
                    EliteDangerousCore.EDSM.EDSMClass edsm = new EliteDangerousCore.EDSM.EDSMClass();

                    if ( !edsm.ShowSystemInEDSM(he.System.Name))
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), "System " + he.System.Name + " unknown to EDSM");
                }
            }
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

            if (Config(Configuration.showIcon))
            {
                columnpos.Add(pos += 40);
                visiblecolwidth += 40;
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
            bool inarea = e.Y > startingtextrowpos && e.Y < startingtextrowpos + 24;
            if (dividercapture == -2 && inarea)
                ShowDividers(true);
            else if (dividercapture == -1 && !inarea)
                ShowDividers(false);
        }

        private void panelControls_MouseEnter(object sender, EventArgs e)
        {
            if (dividercapture == -1)
                ShowDividers(false);
        }

        private void ShowDividers(bool show)
        {
            foreach (ExtButton p in dividers)
                p.Visible = false;

           // System.Diagnostics.Debug.WriteLine($"Dividers {show}");
            if (show)
            {
                dividercapture = -1;

                for (int i = 1; i < columnpos.Count; i++)              // bring up the number of dividers needed
                {
                    ExtButton b = dividers[i - 1];
                    b.Location = new Point(scanpostextoffset.X + columnpos[i] - b.Width/2, pictureBox.Top + startingtextrowpos);
                    b.ButtonColorScaling = 1.0F;
                    if (b.FlatStyle == FlatStyle.System)            // System can't do bitmaps.. we need standard.
                        b.FlatStyle = FlatStyle.Standard;
                    else if ( b.FlatStyle == FlatStyle.Popup )      // if in Popup (ours) we can adjust the look further.
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

        public void ShowScanData(JournalScan scan)
        {
            if ( IsSurfaceScanOn )
            {
                scantext = scan.DisplayString(0,historicmatlist: discoveryform.history.MaterialCommoditiesMicroResources.GetLast());
                Display(current_historylist);
                SetSurfaceScanBehaviour();  // set up timers etc.
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

        void SetLayoutOrder(int n , bool refresh = false)
        {
            layoutorder = n;
            ResetTabList();

            if (refresh)
            {
                ShowDividers(false);
                Display(current_historylist);
            }
        }

        private void SetSurfaceScanBehaviour()    
        {
            scanhide.Stop();

            if (Config(Configuration.showScan15s)) 
                scanhide.Interval = 15000;
            else if (Config(Configuration.showScan30s)) 
                scanhide.Interval = 30000;
            else if (Config(Configuration.showScan60s)) 
                scanhide.Interval = 60000;
            else 
                scanhide.Interval = int.MaxValue;  // i know its not infinite, but are we going to be having it open this long, and it saves code !

            if (!IsSurfaceScanOn)
                HideScanData(null, null);
            else if ( scantext != null )
                scanhide.Start();
        }

        #endregion

        #region Toolbar UI

        private void extButtonShowControl_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(Configuration.showSystemInformation, "Show System Information".TxID("UserControlSpanel.showSystemInformationToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showTargetLine, "Show Target Line".TxID("UserControlSpanel.toolStripMenuItemTargetLine"));
            displayfilter.AddStandardOption(Configuration.showBlackBoxAroundText, "Show black box around text".TxID("UserControlSpanel.blackBoxAroundTextToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showExpandOverColumns, "Expand text over empty columns".TxID("UserControlSpanel.expandTextOverEmptyColumnsToolStripMenuItem"));

            string dontshow = "Don't show information when".TxID("UserControlSpanel.dontShowInformationWhenToolStripMenuItem") + " ";
            displayfilter.AddStandardOption(Configuration.showNothingWhenDocked, dontshow + "docked or landed".TxID("UserControlSpanel.dontShowInformationWhenToolStripMenuItem.showNothingWhenDockedtoolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showNothingWhenPanel, dontshow + "when in a panel".TxID("UserControlSpanel.dontShowInformationWhenToolStripMenuItem.dontshowwhenInGalaxyPanelToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showNoTitleWhenHidden, "Hide the title when hidden".TxID("UserControlSpanel.dontShowInformationWhenToolStripMenuItem.hideTitleToolStripMenuItem"));

            CommonCtrl(displayfilter,extButtonShowControl);
        }

        private void extButtonColumns_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();
            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(Configuration.showEDSMButton, "Show EDSM Button".TxID("UserControlSpanel.EDSMButtonToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showTime, "Show Time".TxID("UserControlSpanel.toolStripMenuItemTime"));
            displayfilter.AddStandardOption(Configuration.showIcon, "Show Event Icon".TxID("UserControlSpanel.iconToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showDescription, "Show Description".TxID("UserControlSpanel.showDescriptionToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showInformation, "Show Information".TxID("UserControlSpanel.showInformationToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showNotes, "Show Notes".TxID("UserControlSpanel.showNotesToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showXYZ, "Show XYZ".TxID("UserControlSpanel.showXYZToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showDistancePerStar, "Show Target Distance per Star".TxID("UserControlSpanel.showTargetToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showDistancesOnFSDJumpsOnly, "Show Distances/Coords on FSD Jumps Only".TxID("UserControlSpanel.showDistancesOnFSDJumpsOnlyToolStripMenuItem"));
            CommonCtrl(displayfilter,extButtonColumns);
        }

        private void extButtonHabZones_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();
            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(Configuration.showHabInformation, "Show Habitation Zone".TxID("UserControlSpanel.showCircumstellarZonesToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showMetalRichZone, "Show Metal Rich Planet Zone".TxID("UserControlSpanel.showCircumstellarZonesToolStripMenuItem.showMetalRichPlanetsToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showWaterWrldZone, "Show Water World Zone".TxID("UserControlSpanel.showCircumstellarZonesToolStripMenuItem.showWaterWorldsToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showEarthLikeZone, "Show Earth Like Zone".TxID("UserControlSpanel.showCircumstellarZonesToolStripMenuItem.showEarthLikeToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showAmmonWrldZone, "Show Ammonia Worlds Zone".TxID("UserControlSpanel.showCircumstellarZonesToolStripMenuItem.showAmmoniaWorldsToolStripMenuItem"));
            displayfilter.AddStandardOption(Configuration.showIcyPlanetZone, "Show Icy Planets Zone".TxID("UserControlSpanel.showCircumstellarZonesToolStripMenuItem.showIcyPlanetsToolStripMenuItem"));
            CommonCtrl(displayfilter,extButtonHabZones);
        }

        private void CommonCtrl(CheckedIconListBoxFormGroup displayfilter, Control button)
        {
            displayfilter.AllOrNoneBack = false;
            displayfilter.ScreenMargin = new Size(0, 0);

            displayfilter.SaveSettings = (s, o) =>
            {
                long v = CheckedIconListBoxFormGroup.SettingsStringToLong(s);
                config = (config & ~displayfilter.LongConfigurationValue) | v;
                System.Diagnostics.Debug.WriteLine($"Spanel config back is {v:X}, result {config:X}");
                ResetTabList();
                ShowDividers(false);
                Display(current_historylist);
            };

            System.Diagnostics.Debug.WriteLine($"Spanel config in {config:X}");
            displayfilter.Show(config & displayfilter.LongConfigurationValue, button, this.FindForm());

        }

        private void extButtonColumnOrder_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();
            displayfilter.AddStandardOption(0, "Default".TxID("UserControlSpanel.OrdertoolStripMenuItem.orderDefaultToolStripMenuItem"), null, "All", true);
            displayfilter.AddStandardOption(1, "Notes after XYZ".TxID("UserControlSpanel.OrdertoolStripMenuItem.orderNotesAfterXYZToolStripMenuItem"), null, "All", true);
            displayfilter.AddStandardOption(2, "Target Distance, XYZ, Notes".TxID("UserControlSpanel.OrdertoolStripMenuItem.orderTargetDistanceXYZNotesToolStripMenuItem"), null, "All", true);
            displayfilter.AllOrNoneBack = false;
            displayfilter.CloseOnChange = true;
            displayfilter.ScreenMargin = new Size(0, 0);
            displayfilter.SaveSettings = (s, o) =>
            {
                var lr = s.Replace(";", "").InvariantParseInt(0);
                SetLayoutOrder(lr,true);
            };

            displayfilter.Show(layoutorder.ToStringInvariant(), extButtonColumnOrder, this.FindForm());
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            cfs.Open(GetSetting(dbFilter, "All"), buttonFilter, this.FindForm());
        }

        private void EventFilterChanged(string newset, Object e)
        {
            string filters = GetSetting(dbFilter, "All");
            if (filters != newset)
            {
                PutSetting(dbFilter, newset);
                Display(current_historylist);
            }
        }

        private void buttonField_Click(object sender, EventArgs e)
        {
            BaseUtils.ConditionLists res = HistoryFilterHelpers.ShowDialog(FindForm(), fieldfilter, discoveryform, "Summary Panel: Filter out fields".T(EDTx.UserControlSpanel_SPF));
            if (res != null)
            {
                fieldfilter = res;
                PutSetting(dbFieldFilter, fieldfilter.GetJSON());
                Display(current_historylist);
            }
        }

        private void extButtonScanShow_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();
            displayfilter.AddStandardOption(Configuration.showScanOff, "Do not show".TxID("UserControlSpanel.surfaceScanDetailsToolStripMenuItem.scanNoToolStripMenuItem"), null, "All", true);
            displayfilter.AddStandardOption(Configuration.showScan15s, "Show for 15s".TxID("UserControlSpanel.surfaceScanDetailsToolStripMenuItem.scan15sToolStripMenuItem"), null, "All", true);
            displayfilter.AddStandardOption(Configuration.showScan30s, "Show for 30s".TxID("UserControlSpanel.surfaceScanDetailsToolStripMenuItem.scan30sToolStripMenuItem"), null, "All", true);
            displayfilter.AddStandardOption(Configuration.showScan60s, "Show for 60s".TxID("UserControlSpanel.surfaceScanDetailsToolStripMenuItem.scan60sToolStripMenuItem"), null, "All", true);
            displayfilter.AddStandardOption(Configuration.showScanIndefinite, "Show until next scan".TxID("UserControlSpanel.surfaceScanDetailsToolStripMenuItem.scanUntilNextToolStripMenuItem"), null, "All", true);
            displayfilter.AllOrNoneBack = false;
            displayfilter.CloseOnChange = true;
            displayfilter.ScreenMargin = new Size(0, 0);
            displayfilter.SaveSettings = (s, o) =>
            {
                long v = CheckedIconListBoxFormGroup.SettingsStringToLong(s);
                config = (config & ~displayfilter.LongConfigurationValue) | v;
                SetSurfaceScanBehaviour();
            };

            displayfilter.Show(config & displayfilter.LongConfigurationValue , extButtonScanShow, this.FindForm(), null, 0);

        }

        private void extButtoScanPos_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();
            displayfilter.AddStandardOption(Configuration.showScanRight, "Scan right".TxID("UserControlSpanel.showInPositionToolStripMenuItem.scanRightMenuItem"), null, "All", true);
            displayfilter.AddStandardOption(Configuration.showScanLeft, "Scan left".TxID("UserControlSpanel.showInPositionToolStripMenuItem.scanLeftMenuItem"), null, "All", true);
            displayfilter.AddStandardOption(Configuration.showScanAbove, "Scan above".TxID("UserControlSpanel.showInPositionToolStripMenuItem.scanAboveMenuItem"), null, "All", true);
            displayfilter.AddStandardOption(Configuration.showScanBelow, "Scan below".TxID("UserControlSpanel.showInPositionToolStripMenuItem.scanBelowMenuItem"), null, "All", true);
            displayfilter.AddStandardOption(Configuration.showScanOnTop, "Scan on top".TxID("UserControlSpanel.showInPositionToolStripMenuItem.scanOnTopMenuItem"), null, "All", true);
            displayfilter.AllOrNoneBack = false;
            displayfilter.CloseOnChange = true;
            displayfilter.ScreenMargin = new Size(0, 0);
            displayfilter.SaveSettings = (s, o) =>
            {
                long v = CheckedIconListBoxFormGroup.SettingsStringToLong(s);
                config = (config & ~displayfilter.LongConfigurationValue) | v;
                if (scantext != null)
                {
                    Display(current_historylist);
                }
            };

            displayfilter.Show(config & displayfilter.LongConfigurationValue, extButtoScanPos, this.FindForm(), null, 0);
        }

        private void extButtonFont_Click(object sender, EventArgs e)
        {
            Font f = FontHelpers.FontSelection(this.FindForm(), displayfont ?? this.Font);
            string setting = FontHelpers.GetFontSettingString(f);
            System.Diagnostics.Debug.WriteLine($"Spanel Font selected {setting}");
            PutSetting("font", setting);
            displayfont = f;
            Display(current_historylist);
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("wordwrap", extCheckBoxWordWrap.Checked);
            Display(current_historylist);
        }


        #endregion

    }
}
