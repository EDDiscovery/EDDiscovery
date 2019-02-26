/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EMK.LightGeometry;
using ExtendedControls;
using BaseUtils;
using EliteDangerousCore.DB;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSpanel : UserControlCommonBase
    {
        private string DbSave { get { return DBName("SPanel" ); } }
        private string DbFilterSave { get { return DBName("SPanelEventFilter2" ); } }
        private string DbFieldFilter { get { return DBName("SPanelFieldFilter" ); } }

        FilterSelector cfs;
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

        enum UIState { Normal, SystemMap, GalMap };
        UIState uistate = UIState.Normal;
		
        class Configuration         // SO many now we need to prepare for a Long
        {
            public const long showInformation = 1;
            public const long showEDSMButton = 2;
            public const long showTime = 4;
            public const long showDescription = 8;
            public const long showNotes = 16;
            public const long showXYZ = 32;
            public const long showDistancePerStar = 64;
            public const long showIcon = 128;

            public const long showaffectsTabs = showInformation | showEDSMButton | showTime | showDescription | showNotes | showXYZ | showDistancePerStar | showIcon;

            public const long showDistancesOnFSDJumpsOnly = 1024;
            public const long showTargetLine = 2048;
            public const long showBlackBoxAroundText = 4096;
            public const long showExpandOverColumns = 8192;
            public const long showNothingWhenDocked = 16384;

            public const long showScan15s = 32768;
            public const long showScan30s = 65536;
            public const long showScan60s = 131072;
            public const long showScanIndefinite = 262144;
            public const long showScanOff = 0;                    // this is not saved; but is a flag for the function to turn it off

            public const long showScanRight = 524288;
            public const long showScanLeft = 1048576;
            public const long showScanOnTop = 2097152;
            public const long showScanBelow = 4194304;
            public const long showScanAbove = 8388608;

            public const long showSystemInformation = 1L << 32;
            public const long showHabInformation = 1L << 33;
            public const long showNothingWhenSysmap = 1L << 34;
            public const long showNothingWhenGalmap = 1L << 35;
			public const long showNoTitleWhenHidden = 1L << 36;
			public const long showMetalRichZone = 1L << 40;
			public const long showWaterWrldZone = 1L << 41;
			public const long showEarthLikeZone = 1L << 42;
			public const long showAmmonWrldZone = 1L << 43;
			public const long showIcyPlanetZone = 1L << 44;
		}

        long config = Configuration.showTargetLine | Configuration.showEDSMButton | Configuration.showIcon | 
                                                Configuration.showTime | Configuration.showDescription |
                                               Configuration.showInformation | Configuration.showNotes | 
                                                Configuration.showXYZ | Configuration.showDistancePerStar |
                                               Configuration.showScan15s | Configuration.showSystemInformation |
                                               Configuration.showScanRight;

        bool Config(long c) { return (config & c) != 0; }
        bool IsSurfaceScanOn { get { return Config(Configuration.showScan15s) || Config(Configuration.showScan30s) || Config(Configuration.showScan60s) || Config(Configuration.showScanIndefinite); } }

        int layoutorder = 0;

        public UserControlSpanel()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            config = (long)(SQLiteDBClass.GetSettingInt(DbSave + "Config", (int)config)) | ((long)(SQLiteDBClass.GetSettingInt(DbSave + "ConfigH", (int)(config >> 32))) << 32);
            toolStripMenuItemTargetLine.Checked = Config(Configuration.showTargetLine);
            toolStripMenuItemTime.Checked = Config(Configuration.showTime);
            EDSMButtonToolStripMenuItem.Checked = Config(Configuration.showEDSMButton);
            iconToolStripMenuItem.Checked = Config(Configuration.showIcon);
            showTargetToolStripMenuItem.Checked = Config(Configuration.showDistancePerStar);
            showNotesToolStripMenuItem.Checked = Config(Configuration.showNotes);
            showXYZToolStripMenuItem.Checked = Config(Configuration.showXYZ);
            showDescriptionToolStripMenuItem.Checked = Config(Configuration.showDescription);
            showInformationToolStripMenuItem.Checked = Config(Configuration.showInformation);
            blackBoxAroundTextToolStripMenuItem.Checked = Config(Configuration.showBlackBoxAroundText);
            showDistancesOnFSDJumpsOnlyToolStripMenuItem.Checked = Config(Configuration.showDistancesOnFSDJumpsOnly);
            expandTextOverEmptyColumnsToolStripMenuItem.Checked = Config(Configuration.showExpandOverColumns);
            showNothingWhenDockedtoolStripMenuItem.Checked = Config(Configuration.showNothingWhenDocked);
            showSystemInformationToolStripMenuItem.Checked = Config(Configuration.showSystemInformation);
            showCircumstellarZonesToolStripMenuItem.Checked = Config(Configuration.showHabInformation);
			showMetalRichPlanetsToolStripMenuItem.Checked = Config(Configuration.showMetalRichZone);
			showWaterWorldsToolStripMenuItem.Checked = Config(Configuration.showWaterWrldZone);
			showEarthLikeToolStripMenuItem.Checked = Config(Configuration.showEarthLikeZone);
			showAmmoniaWorldsToolStripMenuItem.Checked = Config(Configuration.showAmmonWrldZone);
			showIcyPlanetsToolStripMenuItem.Checked = Config(Configuration.showIcyPlanetZone);
            dontshowwhenInGalaxyPanelToolStripMenuItem.Checked = Config(Configuration.showNothingWhenGalmap);
            dontshowwhenInSystemMapPanelToolStripMenuItem.Checked = Config(Configuration.showNothingWhenSysmap);
			completelyHideThePanelToolStripMenuItem.Checked = Config(Configuration.showNoTitleWhenHidden);

            SetSurfaceScanBehaviour(null);
            SetScanPosition(null);
            SetLayoutOrder(SQLiteDBClass.GetSettingInt(DbSave + "Layout", layoutorder),false);  // also resets the tab order

            scanhide.Tick += HideScanData;

            dividercheck.Tick += DividerCheck;
            dividercheck.Interval = 500;

            string tabs = SQLiteDBClass.GetSettingString(DbSave + "PanelTabs", "");

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

            displayfont = discoveryform.theme.GetFont;

            pictureBox.ContextMenuStrip = contextMenuStrip;

            string filter = SQLiteDBClass.GetSettingString(DbFieldFilter, "");
            if (filter.Length > 0)
                fieldfilter.FromJSON(filter);        // load filter

            cfs = new FilterSelector(DbFilterSave);
            cfs.AddAllNone();
            cfs.AddJournalExtraOptions();
            cfs.AddJournalEntries();
            cfs.Closing += EventFilterChanged;

            dividers = new ExtButton[] { buttonExt0, buttonExt1, buttonExt2, buttonExt3, buttonExt4, buttonExt5, buttonExt6, buttonExt7, buttonExt8, buttonExt9, buttonExt10, buttonExt11, buttonExt12 };

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += NewEntry;
            discoveryform.OnNewTarget += NewTarget;
            discoveryform.OnNewUIEvent += OnNewUIEvent;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
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

            SQLiteDBClass.PutSettingInt(DbSave + "Config", (int)config);
            SQLiteDBClass.PutSettingInt(DbSave + "ConfigH", (int)(config>>32));
            SQLiteDBClass.PutSettingInt(DbSave + "Layout", layoutorder);
            string s = string.Join<int>(",", columnpos);
            SQLiteDBClass.PutSettingString(DbSave + "PanelTabs", s);
        }

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            Display(current_historylist);
        }

        private void UserControlSpanel_Resize(object sender, EventArgs e)
        {
            if ( !ResizingNow && !IsInTemporaryResize && this.Width>0)
                Display(current_historylist);
        }


        #region Display

        public override void InitialDisplay()
        {
            Display(discoveryform.history);
        }

        private void Display(HistoryList hl)            
        {
            pictureBox.ClearImageList();

            current_historylist = hl;

            if (hl != null && hl.Count > 0)     // just for safety
            {
                List<HistoryEntry> result = current_historylist.LastFirst;      // Standard filtering

                int ftotal;         // event filter
                result = HistoryList.FilterByJournalEvent(result, SQLiteDBClass.GetSettingString(DbFilterSave, "All"), out ftotal);
                result = FilterHelpers.FilterHistory(result, fieldfilter , discoveryform.Globals, out ftotal); // and the field filter..

                RevertToNormalSize();                                           // ensure size is back to normal..
                scanpostextoffset = new Point(0, 0);                            // left/ top used by scan display

                Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                Color backcolour = IsTransparent ? (Config(Configuration.showBlackBoxAroundText) ? Color.Black : Color.Transparent) : this.BackColor;

                bool drawnnootherstuff = DrawScanText(true, textcolour, backcolour);                    // go 1 for some of the scan positions

                if (!drawnnootherstuff)                                         // and it may indicate its overwriting all stuff, which is fine
                {
                    int rowpos = scanpostextoffset.Y;
                    int rowheight = Config(Configuration.showIcon) ? 26 : 20;
					int habrowheight = Config(Configuration.showIcon) ? 26 : 20;

					// Check if need to hide the UI
                    if (Config(Configuration.showNothingWhenDocked) && 
						(hl.IsCurrentlyDocked || hl.IsCurrentlyLanded))
                    {
						if (!Config(Configuration.showNoTitleWhenHidden))
						{
							AddColText(0, 0, rowpos, rowheight, (hl.IsCurrentlyDocked) ? "Docked" : "Landed",
									   textcolour, backcolour, null);
						}
					}
                    else if ( ( uistate == UIState.GalMap && Config(Configuration.showNothingWhenGalmap)) 
							  || ( uistate == UIState.SystemMap && Config(Configuration.showNothingWhenSysmap)))
                    {
						if (!Config(Configuration.showNoTitleWhenHidden))
						{
							AddColText(0, 0, rowpos, rowheight,
									   (uistate == UIState.GalMap) ? "Galaxy Map" : "System Map", 
									   textcolour, backcolour, null);
						}
					}
                    else
                    {
                        string name;
                        Point3D tpos;
                        bool targetpresent = TargetClass.GetTargetPosition(out name, out tpos);

                        ISystem currentsystem = hl.CurrentSystem; // may be null

                        HistoryEntry last = hl.GetLast;

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

                            AddColText(0, 0, rowpos, rowheight, str, textcolour, backcolour, null , firstdiscovery ? EDDiscovery.Icons.Controls.firstdiscover : null, "Shows if EDSM indicates your it's first discoverer");

                            rowpos += rowheight;
                        }

                        if (Config(Configuration.showHabInformation) && last != null)
                        {
                            StarScan scan = hl.starscan;

                            StarScan.SystemNode sn = scan.FindSystem(last.System, true);    // EDSM look up here..

                            StringBuilder res = new StringBuilder();

							void expandRowHeight()
							{
								habrowheight += 20;
							}

                            if ( sn != null && sn.starnodes.Count>0 && sn.starnodes.Values[0].ScanData != null )
                            {
                                JournalScan js = sn.starnodes.Values[0].ScanData;

                                //res.AppendFormat(js.CircumstellarZonesString().Replace("\r\n", " "));

								if (showCircumstellarZonesToolStripMenuItem.Checked)
								{
									res.AppendFormat("Goldilocks, {0} ({1}-{2} AU),\n".Tx(this),
													 js.GetHabZoneStringLs(),
													 (js.HabitableZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"),
													 (js.HabitableZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"));
								}

								if (showMetalRichPlanetsToolStripMenuItem.Checked)
								{
									res.AppendFormat(js.MetalRichZoneString());
									expandRowHeight();
								}

								if (showWaterWorldsToolStripMenuItem.Checked)
								{
									res.AppendFormat((js.WaterWorldZoneString()));
									expandRowHeight();
								}

								if (showEarthLikeToolStripMenuItem.Checked)
								{
									res.AppendFormat(js.EarthLikeZoneString());
									expandRowHeight();
								}

								if (showAmmoniaWorldsToolStripMenuItem.Checked)
								{
									res.AppendFormat(js.AmmoniaWorldZoneString());
									expandRowHeight();
								}

								if (showIcyPlanetsToolStripMenuItem.Checked)
								{
									res.AppendFormat(js.IcyPlanetsZoneString());
									expandRowHeight();
								}
                            }

                            if (res != null)
                            {
                                AddColText(0, 0, rowpos, habrowheight, res.ToString(), textcolour, backcolour, null);
                                rowpos += habrowheight;
                            }
                        }

                        if (targetpresent && Config(Configuration.showTargetLine) && currentsystem != null)
                        {
                            string dist = (currentsystem.HasCoordinate) ? currentsystem.Distance(tpos.X, tpos.Y, tpos.Z).ToString("0.00") : "Unknown".Tx();
                            AddColText(0, 0, rowpos, rowheight, "Target".Tx(this) + ": " + name + " @ " + dist +" ly", textcolour, backcolour, null);
                            rowpos += rowheight;
                        }

                        foreach (HistoryEntry rhe in result)
                        {
                            DrawHistoryEntry(rhe, rowpos, rowheight, tpos, textcolour, backcolour);
                            rowpos += rowheight;

                            if (rowpos > ClientRectangle.Height)                // stop when off of screen
                                break;
                        }
                    }
                }

                DrawScanText(false, textcolour, backcolour);     // go 2
            }

            pictureBox.Render();
        }

        void DrawHistoryEntry(HistoryEntry he, int rowpos, int rowheight, Point3D tpos , Color textcolour , Color backcolour )
        {
            List<string> coldata = new List<string>();                      // First we accumulate the strings
            List<int> tooltipattach = new List<int>();

            if (Config(Configuration.showTime))
                coldata.Add((EDDiscoveryForm.EDDConfig.DisplayUTC ? he.EventTimeUTC : he.EventTimeLocal).ToString("HH:mm.ss"));

            if (Config(Configuration.showIcon))
                coldata.Add("`!!ICON!!");                // dummy place holder..

            he.journalEntry.FillInformation(out string EventDescription, out string EventDetailedInfo);

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
                coldata.Add((he.snc != null) ? he.snc.Note.Replace("\r\n", " ") : "");
            }

            bool showdistance = !Config(Configuration.showDistancesOnFSDJumpsOnly) || he.IsFSDJump;

            if (layoutorder == 2 && Config(Configuration.showDistancePerStar))
                coldata.Add(showdistance ? DistToStar(he, tpos) : "");

            if (Config(Configuration.showXYZ))
            {
                coldata.Add((he.System.HasCoordinate && showdistance) ? he.System.X.ToString("0.00") : "");
                coldata.Add((he.System.HasCoordinate && showdistance) ? he.System.Y.ToString("0.00") : "");
                coldata.Add((he.System.HasCoordinate && showdistance) ? he.System.Z.ToString("0.00") : "");
            }

            if (layoutorder > 0 && Config(Configuration.showNotes))
            {
                coldata.Add((he.snc != null) ? he.snc.Note.Replace("\r\n", " ") : "");
            }

            if (layoutorder < 2 && Config(Configuration.showDistancePerStar))
                coldata.Add(showdistance ? DistToStar(he, tpos) : "");

            int colnum = 0;

            if (Config(Configuration.showEDSMButton))
            {
                Color backtext = (backcolour.IsFullyTransparent()) ? Color.Black : backcolour;
                ExtendedControls.ExtPictureBox.ImageElement edsm = pictureBox.AddTextFixedSizeC(new Point(scanpostextoffset.X + columnpos[colnum++], rowpos), new Size(45, 14), 
                                            "EDSM", displayfont, backtext, textcolour, 0.5F, true, he, "View system on EDSM".Tx(this,"TVE"));
                edsm.Translate(0, (rowheight - edsm.img.Height) / 2);          // align to centre of rowh..
                edsm.SetAlternateImage(BaseUtils.BitMapHelpers.DrawTextIntoFixedSizeBitmapC("EDSM", edsm.img.Size, displayfont, backtext, textcolour.Multiply(1.2F), 0.5F, true), edsm.pos, true);
            }

            string tooltip = he.EventSummary + Environment.NewLine + EventDescription + Environment.NewLine + EventDetailedInfo;

            for (int i = 0; i < coldata.Count; i++)             // then we draw them, allowing them to overfill columns if required
            {
                int nextfull = i+1;
                for (; nextfull < coldata.Count && Config(Configuration.showExpandOverColumns) && coldata[nextfull].Length == 0; nextfull++)
                { }

                if ( coldata[i].Equals("`!!ICON!!") )            // marker for ICON..
                {
                    Image img = he.journalEntry.Icon;
                    ExtendedControls.ExtPictureBox.ImageElement e = pictureBox.AddImage(new Rectangle(scanpostextoffset.X + columnpos[colnum+i], rowpos, img.Width, img.Height), img, null, null, false);
                    e.Translate(0, (rowheight - e.img.Height) / 2);          // align to centre of rowh..
                }
                else
                    AddColText(colnum + i, colnum + nextfull, rowpos, rowheight, coldata[i], textcolour, backcolour, tooltipattach.Contains(i) ? tooltip : null);
            }
        }

        public bool DrawScanText(bool attop, Color textcolour , Color backcolour)
        {
            Size maxscansize = new Size(1920, 1080);            // set arbitary large.. not important for this.

            if (scantext != null)
            {
                if (attop)
                {
                    if (Config(Configuration.showScanLeft))
                    {
                        ExtPictureBox.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(4, 0), maxscansize, scantext, displayfont, textcolour, backcolour, 1.0F, "SCAN");
                        scanpostextoffset = new Point(4 + scanimg.img.Width + 4, 0);
                        RequestTemporaryMinimumSize(new Size(scanimg.img.Width + 8, scanimg.img.Height + 4));
                    }
                    else if (Config(Configuration.showScanAbove))
                    {
                        ExtPictureBox.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(4, 0), maxscansize, scantext, displayfont, textcolour, backcolour, 1.0F, "SCAN");
                        scanpostextoffset = new Point(0, scanimg.img.Height + 4);
                        RequestTemporaryResizeExpand(new Size(0, scanimg.img.Height + 4));
                    }
                    else if (Config(Configuration.showScanOnTop))
                    {
                        ExtPictureBox.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(4, 0), maxscansize, scantext, displayfont, textcolour, backcolour, 1.0F, "SCAN");
#if false

                        using (Graphics gr = Graphics.FromImage(scanimg.img))
                        {
                            using (Pen p1 = new Pen(Color.Red, 1.0F))
                            {
                                for (int i = 0; i < 1000; i += 50)
                                    gr.DrawLine(p1, new Point(0, i), new Point(100, i));
                            }
                        }
#endif
                        RequestTemporaryResize(new Size(scanimg.img.Width + 8, scanimg.img.Height + 4 ));        // match exactly to use minimum space
                        return true;
                    }
                }
                else // bottom chance
                {
                    if (Config(Configuration.showScanRight))
                    {
                        Size s = pictureBox.DisplaySize();
                        ExtPictureBox.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(s.Width + 4, 0), maxscansize, scantext, displayfont, textcolour, backcolour, 1.0F, "SCAN");
                        RequestTemporaryMinimumSize(new Size(s.Width+4+scanimg.img.Width + 8, scanimg.img.Height + 4));
                    }
                    else if (Config(Configuration.showScanBelow))
                    {
                        Size s = pictureBox.DisplaySize();
                        ExtPictureBox.ImageElement scanimg = pictureBox.AddTextAutoSize(new Point(4, s.Height + 4), maxscansize, scantext, displayfont, textcolour, backcolour, 1.0F, "SCAN");
                        RequestTemporaryResizeExpand(new Size(0, scanimg.img.Height + 4));
                    }
                }
            }

            return false;
        }

        void AddColText(int coli, int nextcol , int rowpos, int rowh, string text, Color textcolour, Color backcolour, string tooltip, Image opt = null , string imagetooltip = null)
        {
            if (text.Length > 0)            // don't place empty text, do not want image handling to work on blank screen
            {
                int endpos = (nextcol == 0) ? 1920 : (columnpos[nextcol] - columnpos[coli] - 4);

                int colpos = scanpostextoffset.X + columnpos[coli];

                if ( opt != null )
                {
                    pictureBox.AddImage(new Rectangle(colpos, rowpos, 24, 24), Icons.Controls.firstdiscover, null, imagetooltip, false);
                    colpos += 24;
                }

                ExtendedControls.ExtPictureBox.ImageElement e =
                                pictureBox.AddTextAutoSize(new Point(colpos, rowpos),
                                new Size(endpos, rowh),
                                text, displayfont, textcolour, backcolour, 1.0F, null, tooltip);

                e.Translate(0, (rowh - e.img.Height) / 2);          // align to centre of rowh..
            }
        }

        private void OnNewUIEvent(UIEvent uievent)       // UI event in, see if we want to hide.  UI events come before any onNew
        {
            EliteDangerousCore.UIEvents.UIJournalMusic jm = uievent as EliteDangerousCore.UIEvents.UIJournalMusic;

            if (jm != null)
            {
                string ev = jm.Track;

                bool refresh = false;
                if (ev.Contains("GalaxyMap"))
                {
                    refresh = (uistate != UIState.GalMap);
                    uistate = UIState.GalMap;
                }
                else if (ev.Contains("SystemMap"))
                {
                    refresh = (uistate != UIState.SystemMap);
                    uistate = UIState.SystemMap;
                }
                else
                {
                    refresh = (uistate != UIState.Normal);
                    uistate = UIState.Normal;
                }

                //System.Diagnostics.Debug.WriteLine("UI event " + obj + " " + uistate + " shown " + shown);
                if (refresh && !jm.Shown)      // if we materially changed, and we are not showing ui events, need to update here
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
            return he.IsJournalEventInEventFilter(SQLiteDBClass.GetSettingString(DbFilterSave, "All")) && FilterHelpers.FilterHistory(he, fieldfilter , discoveryform.Globals);
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

                    string url = edsm.GetUrlToEDSMSystem(he.System.Name, he.System.EDSMID);

                    if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                        System.Diagnostics.Process.Start(url);
                    else
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
            if (dividercapture == -2 && e.Y < 24)
                ShowDividers(true);
            else if (dividercapture == -1 && e.Y >= 24)
                ShowDividers(false);
        }

        private void ShowDividers(bool show)
        {
            foreach (ExtButton p in dividers)
                p.Visible = false;

            //System.Diagnostics.Debug.WriteLine("Dividers " + show);
            if (show)
            {
                dividercapture = -1;

                for (int i = 1; i < columnpos.Count; i++)              // bring up the number of dividers needed
                {
                    ExtButton b = dividers[i - 1];
                    b.Location = new Point(scanpostextoffset.X + columnpos[i] - b.Width/2, 0);
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
                scantext = scan.DisplayString(historicmatlist: discoveryform.history.GetLast?.MaterialCommodity);
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

        private void showSystemInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showSystemInformation, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void showHabitationMinimumAndMaximumDistanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showHabInformation, ((ToolStripMenuItem)sender).Checked, true);
        }

		private void showMetalRichPlanetsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FlipConfig(Configuration.showMetalRichZone, ((ToolStripMenuItem)sender).Checked, true);
		}

		private void showWaterWorldsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FlipConfig(Configuration.showWaterWrldZone, ((ToolStripMenuItem)sender).Checked, true);
		}

		private void showEarthLikeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FlipConfig(Configuration.showEarthLikeZone, ((ToolStripMenuItem)sender).Checked, true);
		}

		private void showAmmoniaWorldsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FlipConfig(Configuration.showAmmonWrldZone, ((ToolStripMenuItem)sender).Checked, true);
		}

		private void showIcyPlanetsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FlipConfig(Configuration.showIcyPlanetZone, ((ToolStripMenuItem)sender).Checked, true);
		}

        private void toolStripMenuItemTargetLine_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showTargetLine, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void EDSMButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showEDSMButton, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void iconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showIcon, ((ToolStripMenuItem)sender).Checked, true);
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
        private void dontshowwhenInGalaxyPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showNothingWhenGalmap, ((ToolStripMenuItem)sender).Checked, true);
        }

        private void dontshowwhenInSystemPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showNothingWhenSysmap, ((ToolStripMenuItem)sender).Checked, true);
        }

		private void completelyHideThePanelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FlipConfig(Configuration.showNoTitleWhenHidden, ((ToolStripMenuItem)sender).Checked, true);
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
            cfs.Filter(contextMenuStrip.PointToScreen(new Point(0, 0)), new Size(300,800), this.FindForm());
        }

        private void EventFilterChanged(object sender, bool same, Object e)
        {
            if (!same)
                Display(current_historylist);
        }

        private void configureFieldFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtendedConditionsForms.ConditionFilterForm frm = new ExtendedConditionsForms.ConditionFilterForm();
            List<string> namelist = new List<string>() { "Note" };
            namelist.AddRange(discoveryform.Globals.NameList);
            frm.InitFilter("Summary Panel: Filter out fields".Tx(this,"SPF"),
                            Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location),
                            JournalEntry.GetNameOfEvents() ,
                            (s) => { return BaseUtils.TypeHelpers.GetPropertyFieldNames(JournalEntry.TypeOfJournalEntry(s)); },
                            namelist, fieldfilter);
            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                fieldfilter = frm.Result;
                SQLiteDBClass.PutSettingString(DbFieldFilter, fieldfilter.GetJSON());
                Display(current_historylist);
            }
        }

        void FlipConfig(long item, bool ch , bool redisplay = false)
        {
            if (ch)
                config |= item;
            else
                config &= ~item;

            if ((item & Configuration.showaffectsTabs)!=0)
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

        private void SetSurfaceScanBehaviour(long? itemClicked)    // pass in a 
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

        private void SetScanPosition(long? position)
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
