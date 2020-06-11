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

            showEDSMStartButtonsToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showedsmbut", true);
            showEDSMStartButtonsToolStripMenuItem.Click += Optionchanged_Click;

            showFuelLevelToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "fuel", true);
            showFuelLevelToolStripMenuItem.Click += Optionchanged_Click;

            showCurrentFSDRangeToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "Dcur", true);
            showCurrentFSDRangeToolStripMenuItem.Click += Optionchanged_Click;

            showAvgFSDRangeToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "Dmax", true);
            showAvgFSDRangeToolStripMenuItem.Click += Optionchanged_Click;

            showMaxFSDRangeToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "Dfume", true);
            showMaxFSDRangeToolStripMenuItem.Click += Optionchanged_Click;

            showFSDRangeToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "Drange", true);
            showFSDRangeToolStripMenuItem.Click += Optionchanged_Click;

            showTargetToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "target", true);
            showTargetToolStripMenuItem.Click += Optionchanged_Click;

            showTravelledDistanceToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "travel", true);
            showTravelledDistanceToolStripMenuItem.Click += Optionchanged_Click;

            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
        }


        public override void Closing()
        {
            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= NewEntry;
            discoveryform.OnNewTarget -= NewTarget;
            discoveryform.OnEDSMSyncComplete -= Discoveryform_OnEDSMSyncComplete;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showedsmbut", showEDSMStartButtonsToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "fuel", showFuelLevelToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "Dcur", showCurrentFSDRangeToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "Dmax", showAvgFSDRangeToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "Dfume", showMaxFSDRangeToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "Drange", showFSDRangeToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "target", showTargetToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "travel", showTravelledDistanceToolStripMenuItem.Checked);
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
            HistoryEntry fsd = hl.GetLastHistoryEntry(x => x.journalEntry.EventTypeID == JournalTypeEnum.FSDJump);      // fsd is presumed to be JournalFSDJump below
            HistoryEntry fuel = hl.GetLastHistoryEntry(x => x.journalEntry.EventTypeID == JournalTypeEnum.RefuelAll
                    || x.journalEntry.EventTypeID == JournalTypeEnum.RefuelPartial);
            HistoryEntry hex = fsd;
            if (lfs != null && (hex == null || lfs.EventTimeUTC >= hex.EventTimeUTC))
                hex = lfs;
            if (fuel != null && (hex == null || fuel.EventTimeUTC >= hex.EventTimeUTC))
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

                int leftstart = 5;
                int topstart = 5;
                int coltext = leftstart;

                ExtendedControls.ExtPictureBox.ImageElement iedsm = null;

                if (showEDSMStartButtonsToolStripMenuItem.Checked)
                {
                    iedsm = pictureBox.AddTextAutoSize(new Point(leftstart, topstart), new Size(1000, 1000), "EDSM", displayfont, backcolour, textcolour, 0.5F, he, "View system on EDSM");
                    iedsm.SetAlternateImage(BaseUtils.BitMapHelpers.DrawTextIntoAutoSizedBitmap("EDSM", iedsm.Image.Size, displayfont, backcolour, textcolour.Multiply(1.2F), 0.5F), iedsm.Location, true);
                    coltext = iedsm.Location.Right + displayfont.ScalePixels(8);
                }

                backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                EliteDangerousCalculations.FSDSpec.JumpInfo ji = he.GetJumpInfo();          // may be null

                string line = String.Format("{0} [{1}]", he.System.Name, discoveryform.history.GetVisitsCount(he.System.Name));

                if (showTargetToolStripMenuItem.Checked)
                {
                    if (TargetClass.GetTargetPosition(out string name, out Point3D tpos))
                    {
                        double dist = he.System.Distance(tpos.X, tpos.Y, tpos.Z);

                        string mesg = "Left".T(EDTx.UserControlTrippanel_Left);

                        if (ji != null)
                        {
                            int jumps = (int)Math.Ceiling(dist / ji.avgsinglejump);
                            if (jumps > 0)
                                mesg = jumps.ToString() + " " + ((jumps == 1) ? "jump".T(EDTx.UserControlTrippanel_jump) : "jumps".T(EDTx.UserControlTrippanel_jumps));
                        }

                        line += String.Format("-> {0} {1:N1}ly {2}", name, dist, mesg);
                    }
                    else
                        line += " -> Target not set".T(EDTx.UserControlTrippanel_NoT);
                }

                bool firstdiscovery = (lastfsd != null && (lastfsd.journalEntry as EliteDangerousCore.JournalEvents.JournalFSDJump).EDSMFirstDiscover);

                int line1hpos = coltext;
                if ( firstdiscovery )
                {
                    var i1 = pictureBox.AddImage(new Rectangle(line1hpos, 5, 24, 24), Icons.Controls.firstdiscover, null, "Shows if EDSM indicates your it's first discoverer".T(EDTx.UserControlTrippanel_FDEDSM), false);
                    line1hpos = i1.Location.Right + Font.ScalePixels(8);
                }

                var eline1 = pictureBox.AddTextAutoSize(new Point(line1hpos, topstart), new Size(1000, 1000), line, displayfont, textcolour, backcolour, 1.0F);
                int line2vpos = eline1.Location.Bottom + displayfont.ScalePixels(8);

                line = "";

                if (showTravelledDistanceToolStripMenuItem.Checked)
                {
                    line = String.Format("{0:N1}{1},{2} " + "jumps".T(EDTx.UserControlTrippanel_jumps) + ", {3}", he.TravelledDistance, ((he.TravelledMissingjump > 0) ? "ly*" : "ly"),
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

                        if (lastJet != null && lastfsd != null && lastJet.EventTimeUTC > lastfsd.EventTimeUTC)
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

                if (showEDSMStartButtonsToolStripMenuItem.Checked)
                {
                    ExtendedControls.ExtPictureBox.ImageElement start = pictureBox.AddTextFixedSizeC(new Point(leftstart, line2vpos), iedsm.Image.Size, "Start", displayfont, backcolour, textcolour, 0.5F, true, "Start", "Set a journey start point");
                    start.SetAlternateImage(BaseUtils.BitMapHelpers.DrawTextIntoFixedSizeBitmapC("Start", start.Image.Size, displayfont, backcolour, textcolour.Multiply(1.2F), 0.5F, true), start.Location, true);
                }

                if (line.HasChars())
                    pictureBox.AddTextAutoSize(new Point(coltext, line2vpos), new Size(1000, 40), line, displayfont, textcolour, backcolour, 1.0F);

                pictureBox.Render();
            }
        }


        #endregion

        #region Clicks
        private void pictureBox_ClickElement(object sender, MouseEventArgs eventargs, ExtendedControls.ExtPictureBox.ImageElement i, object tag)
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
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), string.Format("System {0} unknown to EDSM".T(EDTx.UserControlTrippanel_UKN), he.System.Name));
                }
                else
                {
                    var list = discoveryform.history.Where(p => p.IsFSDCarrierJump).OrderByDescending(p => p.EventTimeUTC).Take(2);
                    if (list.Count() == 0)
                        return;
                    he = list.ToArray()[0];
                    if (he.StartMarker)
                        return;

                    he.journalEntry.SetStartFlag(); 

                    if (list.Count() > 1 && he.isTravelling)
                    {
                        he = list.ToArray()[1];
                        he.journalEntry.SetEndFlag();
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

