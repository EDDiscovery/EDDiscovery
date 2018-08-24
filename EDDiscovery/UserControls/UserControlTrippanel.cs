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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using EDDiscovery.Forms;
using EliteDangerousCore.DB;
using EliteDangerousCore;
using EMK.LightGeometry;

namespace EDDiscovery.UserControls
{
    public partial class UserControlTrippanel : UserControlCommonBase
    {
        //static String TITLE = "Trip panel";

        private string DbSave { get { return DBName("TripPanel" ); } }

        private HistoryEntry lastHE;
        private HistoryEntry lastFSD;

        private Font displayfont;

        public UserControlTrippanel()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            displayfont = discoveryform.theme.GetFont;

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += NewEntry;
            discoveryform.OnNewTarget += NewTarget;
            discoveryform.OnEDSMSyncComplete += Discoveryform_OnEDSMSyncComplete;

            showEDSMStartButtonsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showedsmbut", true);
            showEDSMStartButtonsToolStripMenuItem.Click += Optionchanged_Click;

            showFuelLevelToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "fuel", true);
            showFuelLevelToolStripMenuItem.Click += Optionchanged_Click;

            showCurrentFSDRangeToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "Dcur", true);
            showCurrentFSDRangeToolStripMenuItem.Click += Optionchanged_Click;

            showAvgFSDRangeToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "Dmax", true);
            showAvgFSDRangeToolStripMenuItem.Click += Optionchanged_Click;

            showMaxFSDRangeToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "Dfume", true);
            showMaxFSDRangeToolStripMenuItem.Click += Optionchanged_Click;

            showFSDRangeToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "Drange", true);
            showFSDRangeToolStripMenuItem.Click += Optionchanged_Click;

            showTargetToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "target", true);
            showTargetToolStripMenuItem.Click += Optionchanged_Click;

            showTravelledDistanceToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "travel", true);
            showTravelledDistanceToolStripMenuItem.Click += Optionchanged_Click;

            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
        }


        public override void Closing()
        {
            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= NewEntry;
            discoveryform.OnNewTarget -= NewTarget;
            discoveryform.OnEDSMSyncComplete -= Discoveryform_OnEDSMSyncComplete;
            SQLiteDBClass.PutSettingBool(DbSave + "showedsmbut", showEDSMStartButtonsToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool(DbSave + "fuel", showFuelLevelToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool(DbSave + "Dcur", showCurrentFSDRangeToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool(DbSave + "Dmax", showAvgFSDRangeToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool(DbSave + "Dfume", showMaxFSDRangeToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool(DbSave + "Drange", showFSDRangeToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool(DbSave + "target", showTargetToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool(DbSave + "travel", showTravelledDistanceToolStripMenuItem.Checked);
        }

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            DisplayState(lastHE, lastFSD);
        }

        #region Display

        public override void InitialDisplay()
        {
            Display(discoveryform.history);
        }

        private void Display(HistoryList hl)            // when user clicks around..  HE may be null here
        {
            HistoryEntry lfs = hl.GetLastHistoryEntry(x => x.IsFuelScoop);
            HistoryEntry fsd = hl.GetLastHistoryEntry(x => x.IsFSDJump);
            HistoryEntry fuel = hl.GetLastHistoryEntry(x => x.journalEntry.EventTypeID == JournalTypeEnum.RefuelAll
                    || x.journalEntry.EventTypeID == JournalTypeEnum.RefuelPartial);
            HistoryEntry hex = fsd;
            if (lfs != null && lfs.EventTimeUTC >= hex.EventTimeUTC)
                hex = lfs;
            if (fuel != null && fuel.EventTimeUTC >= hex.EventTimeUTC)
                hex = fuel;
            DisplayState(hex, fsd);
        }

        public void NewTarget(Object sender)
        {
            DisplayState(lastHE, lastFSD);
        }

        private void Discoveryform_OnEDSMSyncComplete(int arg1, string arg2)
        {
            //System.Diagnostics.Debug.WriteLine("EDSM SYNC COMPLETED with " + count + " '" + syslist + "'");
            DisplayState(lastHE, lastFSD);
        }

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made..
        {
            Display(hl);
        }

        void DisplayState(HistoryEntry he, HistoryEntry lastfsd)
        {
            pictureBox.ClearImageList();

            lastHE = he;
            lastFSD = lastfsd;

            if (he != null)
            {
                Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                Color backcolour = IsTransparent ? Color.Black : this.BackColor;

                int coltext = 5;

                if (showEDSMStartButtonsToolStripMenuItem.Checked)
                {
                    ExtendedControls.PictureBoxHotspot.ImageElement edsm = pictureBox.AddTextFixedSizeC(new Point(5, 5), new Size(80, 20), "EDSM", displayfont, backcolour, textcolour, 0.5F, true, he, "View system on EDSM");
                    edsm.SetAlternateImage(BaseUtils.BitMapHelpers.DrawTextIntoFixedSizeBitmapC("EDSM", edsm.img.Size, displayfont, backcolour, textcolour.Multiply(1.2F), 0.5F, true), edsm.pos, true);

                    ExtendedControls.PictureBoxHotspot.ImageElement start = pictureBox.AddTextFixedSizeC(new Point(5, 35), new Size(80, 20), "Start", displayfont, backcolour, textcolour, 0.5F, true, "Start", "Set a journey start point");
                    start.SetAlternateImage(BaseUtils.BitMapHelpers.DrawTextIntoFixedSizeBitmapC("Start", edsm.img.Size, displayfont, backcolour, textcolour.Multiply(1.2F), 0.5F, true), start.pos, true);

                    coltext = 100;
                }

                backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                EliteDangerousCalculations.FSDSpec.JumpInfo ji = he.GetJumpInfo();          // may be null

                string line = String.Format("{0} [{1}]", he.System.Name, discoveryform.history.GetVisitsCount(he.System.Name));

                if (showTargetToolStripMenuItem.Checked)
                {
                    if (TargetClass.GetTargetPosition(out string name, out Point3D tpos))
                    {
                        double dist = he.System.Distance(tpos.X, tpos.Y, tpos.Z);

                        string mesg = "Left";

                        if (ji != null)
                        {
                            int jumps = (int)Math.Ceiling(dist / ji.avgsinglejump);
                            if (jumps > 0)
                                mesg = jumps.ToString() + " " + ((jumps == 1) ? "jump".Tx(this) : "jumps".Tx(this));
                        }

                        line += String.Format("-> {0} {1:N1}ly {2}", name, dist, mesg);
                    }
                    else
                        line += " -> Target not set".Tx(this,"NoT");
                }

                bool firstdiscovery = (lastfsd != null && (lastfsd.journalEntry as EliteDangerousCore.JournalEvents.JournalFSDJump).EDSMFirstDiscover);

                int line1hpos = coltext;
                if ( firstdiscovery )
                {
                    pictureBox.AddImage(new Rectangle(line1hpos, 5, 24, 24), Icons.Controls.firstdiscover, null, "Shows if EDSM indicates your it's first discoverer".Tx(this, "FDEDSM"), false);
                    line1hpos += 24;
                }

                pictureBox.AddTextAutoSize(new Point(line1hpos, 5), new Size(1000, 40), line, displayfont, textcolour, backcolour, 1.0F);

                line = "";

                if (showTravelledDistanceToolStripMenuItem.Checked)
                {
                    line = String.Format("{0:N1}{1},{2} " + "jumps".Tx(this) + ", {3}", he.TravelledDistance, ((he.TravelledMissingjump > 0) ? "ly*" : "ly"),
                                        he.Travelledjumps,
                                        he.TravelledSeconds);
                }

                ShipInformation si = he.ShipInformation;

                if (si!=null)
                {
                    string addtext = "";

                    if (showFuelLevelToolStripMenuItem.Checked)
                    {
                        double fuel = si.FuelLevel;
                        double tanksize = si.FuelCapacity;
                        double warninglevelpercent = si.FuelWarningPercent;

                        addtext = String.Format("{0}/{1}t", fuel.ToString("N1"), tanksize.ToString("N1"));

                        if (warninglevelpercent > 0 && fuel < tanksize * warninglevelpercent / 100.0)
                        {
                            textcolour = discoveryform.theme.TextBlockHighlightColor;
                            addtext += String.Format(" < {0}%", warninglevelpercent.ToString("N1"));
                        }
                    }

                    if ( ji != null )
                    { 
                        HistoryEntry lastJet = discoveryform.history.GetLastHistoryEntry(x => x.journalEntry.EventTypeID == JournalTypeEnum.JetConeBoost);

                        double boostval = 1;

                        if (lastJet != null && lastfsd != null && lastJet.EventTimeLocal > lastfsd.EventTimeLocal)
                            boostval = (lastJet.journalEntry as EliteDangerousCore.JournalEvents.JournalJetConeBoost).BoostValue;

                        string range = "";
                        if (showCurrentFSDRangeToolStripMenuItem.Checked)
                            range += String.Format("cur {0:N1}ly{1}", ji.cursinglejump * boostval, boostval > 1 ? " Boost" : "");
                        if (showAvgFSDRangeToolStripMenuItem.Checked)
                            range = range.AppendPrePad(String.Format("avg {0:N1}ly{1}", ji.avgsinglejump * boostval , boostval > 1 ? " Boost" : ""), ", ");
                        if (showMaxFSDRangeToolStripMenuItem.Checked)
                            range = range.AppendPrePad(String.Format("max {0:N1}ly{1}", ji.curfumessinglejump * boostval,boostval > 1 ? " Boost" : ""), ", ");
                        if ( showFSDRangeToolStripMenuItem.Checked)
                            range = range.AppendPrePad(String.Format("{0:N1}ly/{1:N0}", ji.maxjumprange, ji.maxjumps), ", ");

                        if (range.HasChars())
                            addtext = addtext.AppendPrePad(range, " | ");
                    }

                    if (addtext.HasChars())
                        line = line.AppendPrePad(addtext, " | ");
                }

                if (line.HasChars())
                    pictureBox.AddTextAutoSize(new Point(coltext, 35), new Size(1000, 40), line, displayfont, textcolour, backcolour, 1.0F);

                pictureBox.Render();
            }
        }


        #endregion

        #region Clicks
        private void pictureBox_ClickElement(object sender, MouseEventArgs eventargs, ExtendedControls.PictureBoxHotspot.ImageElement i, object tag)
        {
            if (i != null)
            {
                HistoryEntry he = tag as HistoryEntry;

                if (he != null)
                {
                    EliteDangerousCore.EDSM.EDSMClass edsm = new EliteDangerousCore.EDSM.EDSMClass();

                    string url = edsm.GetUrlToEDSMSystem(he.System.Name, he.System.EDSMID);

                    if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                        System.Diagnostics.Process.Start(url);
                    else
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), string.Format("System {0} unknown to EDSM".Tx(this,"UKN"), he.System.Name));
                }
                else
                {
                    var list = discoveryform.history.Where(p => p.IsFSDJump).OrderByDescending(p => p.EventTimeUTC).Take(2);
                    if (list.Count() == 0)
                        return;
                    he = list.ToArray()[0];
                    if (he.StartMarker)
                        return;

                    he.journalEntry.UpdateSyncFlagBit(SyncFlags.StartMarker, true, SyncFlags.StopMarker, false);

                    if (list.Count() > 1 && he.isTravelling)
                    {
                        he = list.ToArray()[1];
                        he.journalEntry.UpdateSyncFlagBit(SyncFlags.StopMarker, true , SyncFlags.StartMarker, false);
                    }

                    discoveryform.RefreshHistoryAsync();
                }
            }
        }

        private void Optionchanged_Click(object sender, EventArgs e)
        {
            DisplayState(lastHE, lastFSD);
        }

        #endregion
    }
}

