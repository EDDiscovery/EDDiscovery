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

        private string DbSave { get { return "TripPanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        private HistoryEntry lastHE;

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
        }

        public override void Closing()
        {
            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= NewEntry;
            discoveryform.OnNewTarget -= NewTarget;
        }

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            displayLastFSDOrScoop(lastHE);
        }

        #region Display

        public override void InitialDisplay()
        {
            Display(discoveryform.history);
        }

        private void Display(HistoryList hl)            // when user clicks around..  HE may be null here
        {
            HistoryEntry lfs = hl.GetLastHistoryEntry(x => x.IsFuelScoop);
            HistoryEntry hex = hl.GetLastHistoryEntry(x => x.IsFSDJump);
            HistoryEntry fuel = hl.GetLastHistoryEntry(x => x.journalEntry.EventTypeID == JournalTypeEnum.RefuelAll
                    || x.journalEntry.EventTypeID == JournalTypeEnum.RefuelPartial);
            if (lfs != null && lfs.EventTimeUTC >= hex.EventTimeUTC)
                hex = lfs;
            if (fuel != null && fuel.EventTimeUTC >= hex.EventTimeUTC)
                hex = fuel;
            displayLastFSDOrScoop(hex);
        }

        public void NewTarget(Object sender)
        {
            displayLastFSDOrScoop(lastHE);
        }

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made..
        {
            Display(hl);
        }


        void displayLastFSDOrScoop(HistoryEntry he)
        {
            pictureBox.ClearImageList();

            lastHE = he;

            if (he != null)
            {
                Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                Color backcolour = IsTransparent ? Color.Black : this.BackColor;

                ExtendedControls.PictureBoxHotspot.ImageElement edsm = pictureBox.AddTextFixedSizeC(new Point(5, 5), new Size(80, 20), "EDSM", displayfont, backcolour, textcolour, 0.5F, true, he, "View system on EDSM");
                edsm.SetAlternateImage(BaseUtils.BitMapHelpers.DrawTextIntoFixedSizeBitmapC("EDSM", edsm.img.Size, displayfont, backcolour, textcolour.Multiply(1.2F), 0.5F, true), edsm.pos, true);

                ExtendedControls.PictureBoxHotspot.ImageElement start = pictureBox.AddTextFixedSizeC(new Point(5, 35), new Size(80, 20), "Start", displayfont, backcolour, textcolour, 0.5F, true, "Start", "Set a journey start point");
                start.SetAlternateImage(BaseUtils.BitMapHelpers.DrawTextIntoFixedSizeBitmapC("Start", edsm.img.Size, displayfont, backcolour, textcolour.Multiply(1.2F), 0.5F, true), start.pos, true);

                backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                EliteDangerousCalculations.FSDSpec.JumpInfo ji = he.GetJumpInfo();          // may be null

                String line = "  Target Not Set";

                if (TargetClass.GetTargetPosition(out string name, out Point3D tpos))
                {
                    double dist = he.System.Distance(tpos.X, tpos.Y, tpos.Z);

                    string mesg = "Left";

                    if (ji!=null)
                    {
                        int jumps = (int)Math.Ceiling(dist / ji.avgsinglejump);
                        if (jumps > 0)
                            mesg = "@ " + jumps.ToString() + ((jumps == 1) ? " jump" : " jumps");
                    }

                    line = String.Format("{0} | {1:N2}ly {2}", name, dist, mesg);
                }

                line = String.Format("{0} [{1}] | {2}", he.System.Name, discoveryform.history.GetVisitsCount(he.System.Name) , line);

                pictureBox.AddTextAutoSize(new Point(100, 5), new Size(1000, 40), line, displayfont, textcolour, backcolour, 1.0F);

                line = String.Format("{0:n}{1} @ {2} | {3} | ", he.TravelledDistance, ((he.TravelledMissingjump > 0) ? "ly (*)" : "ly"),
                                        he.Travelledjumps,
                                        he.TravelledSeconds);

                ExtendedControls.PictureBoxHotspot.ImageElement botlineleft = pictureBox.AddTextAutoSize(new Point(100, 35), new Size(1000, 40), line, displayfont, textcolour, backcolour, 1.0F);

                ShipInformation si = he.ShipInformation;
                if (si!=null)
                {
                    double fuel = si.FuelLevel;
                    double tanksize = si.FuelCapacity;
                    double warninglevelpercent = si.FuelWarningPercent;

                    line = String.Format("{0}/{1}t", fuel.ToString("N1"), tanksize.ToString("N1"));

                    if ( warninglevelpercent > 0 && fuel < tanksize * warninglevelpercent / 100.0 )
                    {
                        textcolour = discoveryform.theme.TextBlockHighlightColor;
                        line += String.Format(" < {0}%", warninglevelpercent.ToString("N1"));
                    }

                    if ( ji != null )
                    { 
                        HistoryEntry lastJet = discoveryform.history.GetLastHistoryEntry(x => x.journalEntry.EventTypeID == JournalTypeEnum.JetConeBoost);
                        if (lastJet != null && lastJet.EventTimeLocal > lastHE.EventTimeLocal)
                        {
                            double jumpdistance = ji.avgsinglejump * (lastJet.journalEntry as EliteDangerousCore.JournalEvents.JournalJetConeBoost).BoostValue;
                            line += String.Format(" [{0:N1}ly @ BOOST]", jumpdistance);
                        }
                        else
                        {
                            line += String.Format(" [{0:N1}ly, {1:N1}ly / {2:N0}]", ji.avgsinglejump, ji.maxjumprange, ji.maxjumps);
                        }
                    }

                    pictureBox.AddTextAutoSize(new Point(botlineleft.pos.Right, 35), new Size(1000, 40), line, displayfont, textcolour, backcolour, 1.0F);
                }


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

                    string url = edsm.GetUrlToEDSMSystem(he.System.Name);

                    if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                        System.Diagnostics.Process.Start(url);
                    else
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), "System " + he.System.Name + " unknown to EDSM");
                }
                else
                {
                    var list = discoveryform.history.Where(p => p.IsFSDJump).OrderByDescending(p => p.EventTimeUTC).Take(2);
                    if (list.Count() == 0)
                        return;
                    he = list.ToArray()[0];
                    if (he.StartMarker)
                        return;

                    he.StartMarker = true;
                    JournalEntry.UpdateSyncFlagBit(he.Journalid, SyncFlags.StartMarker, he.StartMarker);
                    if (list.Count() > 1 && he.isTravelling)
                    {
                        he = list.ToArray()[1];
                        he.StopMarker = true;
                        JournalEntry.UpdateSyncFlagBit(he.Journalid, SyncFlags.StopMarker, he.StopMarker);
                    }
                    discoveryform.RefreshHistoryAsync();
                }
            }
        }

        #endregion

    }
}

