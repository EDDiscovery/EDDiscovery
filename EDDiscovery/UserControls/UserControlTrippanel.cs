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
using EDDiscovery.DB;
using EMK.LightGeometry;
using System.Collections.ObjectModel;
using EDDiscovery.Forms;
using EDDiscovery.EliteDangerous;

namespace EDDiscovery.UserControls
{
    public partial class UserControlTrippanel : UserControlCommonBase
    {
        private EDDiscoveryForm discoveryform;
        private TravelHistoryControl travelhistorycontrol;

        //static String TITLE = "Trip panel";

        private int displaynumber = 0;
        private string DbSave { get { return "TripPanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        HistoryEntry lastHE;

        private Font displayfont;

        public UserControlTrippanel()
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

            displayfont = discoveryform.theme.GetFont;

            jumpRange = SQLiteDBClass.GetSettingDouble(DbSave + "JumpRange", -1.0);
            tankSize = SQLiteDBClass.GetSettingDouble(DbSave + "TankSize", -1);
            currentCargo = SQLiteDBClass.GetSettingDouble(DbSave + "currentCargo", 0);
            unladenMass = SQLiteDBClass.GetSettingDouble(DbSave + "unladenMass", -1);

            linearConstant = SQLiteDBClass.GetSettingDouble(DbSave + "linearConstant", -1);
            powerConstant = SQLiteDBClass.GetSettingDouble(DbSave + "powerConstant", -1);
            optimalMass = SQLiteDBClass.GetSettingDouble(DbSave + "optimalMass", -1);
            maxFuelPerJump = SQLiteDBClass.GetSettingDouble(DbSave + "maxFuelPerJump", -1);
            fsdDrive = SQLiteDBClass.GetSettingString(DbSave + "fsdDrive", null);
            tankWarning = SQLiteDBClass.GetSettingDouble(DbSave + "TankWarning", -1);


        }

        private double jumpRange = -1;
        private double tankSize = -1;
        private double currentCargo = -1;
        private double linearConstant = -1;
        private double unladenMass = -1;
        private double optimalMass = -1;
        private double powerConstant = -1;
        private double maxFuelPerJump = -1;
        private string fsdDrive = null;
        private double tankWarning = -1;

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

        public override void Display(HistoryEntry current, HistoryList history)
        {
            Display(history);
        }

        public void Display(HistoryList hl)            // when user clicks around..  HE may be null here
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

        public void NewTarget()
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
                edsm.SetAlternateImage(ExtendedControls.ControlHelpers.DrawTextIntoFixedSizeBitmapC("EDSM", edsm.img.Size, displayfont, backcolour, textcolour.Multiply(1.2F), 0.5F, true), edsm.pos, true);

                ExtendedControls.PictureBoxHotspot.ImageElement start = pictureBox.AddTextFixedSizeC(new Point(5, 35), new Size(80, 20), "Start", displayfont, backcolour, textcolour, 0.5F, true, "Start", "Set a journey start point");
                start.SetAlternateImage(ExtendedControls.ControlHelpers.DrawTextIntoFixedSizeBitmapC("Start", edsm.img.Size, displayfont, backcolour, textcolour.Multiply(1.2F), 0.5F, true), start.pos, true);

                backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                string name;
                Point3D tpos;
                bool targetpresent = TargetClass.GetTargetPosition(out name, out tpos);

                String topline = "  Route Not Set";

                if (targetpresent)
                {
                    double dist = SystemClass.Distance(he.System, tpos.X, tpos.Y, tpos.Z);

                    string mesg = "Left";
                    if (jumpRange > 0)
                    {
                        int jumps = (int)Math.Ceiling(dist / jumpRange);
                        if (jumps > 0)
                            mesg = "@ " + jumps.ToString() + ((jumps == 1) ? " jump" : " jumps");
                    }
                    topline = String.Format("{0} | {1:N2}ly {2}", name, dist, mesg);
                }

                topline = String.Format("{0} [{2}] | {1}", he.System.name, topline, discoveryform.history.GetVisitsCount(he.System.name));

                pictureBox.AddTextAutoSize(new Point(100, 5), new Size(1000, 40), topline, displayfont, textcolour, backcolour, 1.0F);

                string botline = "";

                botline += String.Format("{0:n}{1} @ {2} | {3} | ", he.TravelledDistance, ((he.TravelledMissingjump > 0) ? "ly (*)" : "ly"),
                                        he.Travelledjumps,
                                        he.TravelledSeconds);

                ExtendedControls.PictureBoxHotspot.ImageElement botlineleft = pictureBox.AddTextAutoSize(new Point(100, 35), new Size(1000, 40), botline, displayfont, textcolour, backcolour, 1.0F);


                double fuel = 0.0;
                HistoryEntry fuelhe;


                switch (he.journalEntry.EventTypeID)
                {
                    case EliteDangerous.JournalTypeEnum.FuelScoop:
                        fuel = (he.journalEntry as EliteDangerous.JournalEvents.JournalFuelScoop).Total;
                        break;
                    case EliteDangerous.JournalTypeEnum.FSDJump:
                        fuel = (he.journalEntry as EliteDangerous.JournalEvents.JournalFSDJump).FuelLevel;
                        break;
                    case EliteDangerous.JournalTypeEnum.RefuelAll:
                        fuelhe = discoveryform.history.GetLastHistoryEntry(x => x.journalEntry.EventTypeID == JournalTypeEnum.FSDJump
                    || x.journalEntry.EventTypeID == JournalTypeEnum.FuelScoop);
                        if (fuelhe.journalEntry.EventTypeID == EliteDangerous.JournalTypeEnum.FSDJump)
                            fuel = (fuelhe.journalEntry as EliteDangerous.JournalEvents.JournalFSDJump).FuelLevel;
                        else
                            fuel = (fuelhe.journalEntry as EliteDangerous.JournalEvents.JournalFuelScoop).Total;
                        fuel += (he.journalEntry as EliteDangerous.JournalEvents.JournalRefuelAll).Amount;
                        break;
                    case EliteDangerous.JournalTypeEnum.RefuelPartial:
                        fuelhe = discoveryform.history.GetLastHistoryEntry(x => x.journalEntry.EventTypeID == JournalTypeEnum.FSDJump
                    || x.journalEntry.EventTypeID == JournalTypeEnum.FuelScoop);
                        if (fuelhe.journalEntry.EventTypeID == EliteDangerous.JournalTypeEnum.FSDJump)
                            fuel = (fuelhe.journalEntry as EliteDangerous.JournalEvents.JournalFSDJump).FuelLevel;
                        else
                            fuel = (fuelhe.journalEntry as EliteDangerous.JournalEvents.JournalFuelScoop).Total;
                        fuel += (he.journalEntry as EliteDangerous.JournalEvents.JournalRefuelPartial).Amount;
                        break;
                    //fuel += (he.journalEntry as EliteDangerous.JournalEvents.JournalRefuelAll).Amount;
                    //case EliteDangerous.JournalTypeEnum.RefuelPartial:
                    ////fuel += (he.journalEntry as EliteDangerous.JournalEvents.JournalRefuelPartial).Amount;
                    default:
                        break;
                }
                fuel = Math.Floor(fuel * 100.0) / 100.0;
                if (tankSize == -1||tankWarning==-1)
                {
                    botline = "Please set ships details";
                }
                else
                {
                    if (fuel > tankSize)
                        fuel = tankSize;
                    botline = String.Format("{0}t / {1}t", fuel.ToString("N1"), tankSize.ToString("N1"));

                    if ((fuel / tankSize) < (tankWarning / 100.0))
                    {
                        textcolour = discoveryform.theme.TextBlockHighlightColor;
                        botline += String.Format(" < {0}%", tankWarning.ToString("N1"));
                    }
                }

                if (currentCargo >= 0 && linearConstant > 0 && unladenMass > 0 && optimalMass > 0
                    && powerConstant > 0 && maxFuelPerJump > 0)
                {
                    double maxJumps = 0;
                    double maxJumpDistance = RoutingUtils.maxJumpDistance(fuel,
                        currentCargo, linearConstant, unladenMass,
                        optimalMass, powerConstant,
                        maxFuelPerJump, out maxJumps);
                    double JumpRange = Math.Pow(maxFuelPerJump / (linearConstant * 0.001), 1 / powerConstant) * optimalMass / (currentCargo + unladenMass + fuel);

                    HistoryEntry lastJet= discoveryform.history.GetLastHistoryEntry(x => x.journalEntry.EventTypeID == JournalTypeEnum.JetConeBoost);
                    if (lastJet != null && lastJet.EventTimeLocal > lastHE.EventTimeLocal)
                    {
                        JumpRange *= (lastJet.journalEntry as EliteDangerous.JournalEvents.JournalJetConeBoost).BoostValue;
                        botline += String.Format(" [{0:N2}ly @ BOOST]", Math.Floor(JumpRange * 100) / 100);
                    }
                    else
                    {
                        botline += String.Format(" [{0:N2}ly @ {1:N2}ly / {2:N0}]",
                         Math.Floor(JumpRange * 100) / 100,
                         Math.Floor(maxJumpDistance * 100) / 100,
                         Math.Floor(maxJumps * 100) / 100);
                    }
                } 
                pictureBox.AddTextAutoSize(new Point(botlineleft.pos.Right, 35), new Size(1000, 40), botline, displayfont, textcolour, backcolour, 1.0F);
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
                    EDDiscovery2.EDSM.EDSMClass edsm = new EDDiscovery2.EDSM.EDSMClass();

                    string url = edsm.GetUrlToEDSMSystem(he.System.name);

                    if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                        System.Diagnostics.Process.Start(url);
                    else
                        EDDiscovery.Forms.MessageBoxTheme.Show("System " + he.System.name + " unknown to EDSM");
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
                    EliteDangerous.JournalEntry.UpdateSyncFlagBit(he.Journalid, EliteDangerous.SyncFlags.StartMarker, he.StartMarker);
                    if (list.Count() > 1 && he.isTravelling)
                    {
                        he = list.ToArray()[1];
                        he.StopMarker = true;
                        EliteDangerous.JournalEntry.UpdateSyncFlagBit(he.Journalid, EliteDangerous.SyncFlags.StopMarker, he.StopMarker);
                    }
                    discoveryform.RefreshHistoryAsync();
                }
            }
        }

        #endregion

        private void setShipDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShipDetails form =    new ShipDetails();
            form.JumpRange = jumpRange;
            form.TankSize = tankSize;
            form.CurrentCargo = currentCargo;
            form.UnladenMass = unladenMass;
            form.LinearConstant = linearConstant;
            form.PowerConstant = powerConstant;
            form.OptimalMass = optimalMass;
            form.maxFuelPerJump = maxFuelPerJump;
            form.FSDDrive = "5A";
            form.tankWarning = tankWarning;

            if (form.ShowDialog(this.FindForm() )== DialogResult.OK){

                jumpRange = form.JumpRange;
                tankSize = form.TankSize;
                currentCargo = form.CurrentCargo;
                unladenMass = form.UnladenMass;
                powerConstant = form.PowerConstant;
                optimalMass = form.OptimalMass;
                maxFuelPerJump = form.maxFuelPerJump;
                fsdDrive = form.FSDDrive;
                linearConstant = form.LinearConstant;
                tankWarning = form.tankWarning;

                SQLiteDBClass.PutSettingDouble(DbSave + "JumpRange", jumpRange);
                SQLiteDBClass.PutSettingDouble(DbSave + "TankSize", tankSize);
                SQLiteDBClass.PutSettingDouble(DbSave + "currentCargo", currentCargo);
                SQLiteDBClass.PutSettingDouble(DbSave + "unladenMass", unladenMass);
                SQLiteDBClass.PutSettingDouble(DbSave + "linearConstant", linearConstant);
                SQLiteDBClass.PutSettingDouble(DbSave + "powerConstant", powerConstant);
                SQLiteDBClass.PutSettingDouble(DbSave + "optimalMass", optimalMass);
                SQLiteDBClass.PutSettingDouble(DbSave + "maxFuelPerJump", maxFuelPerJump);
                SQLiteDBClass.PutSettingDouble(DbSave + "TankWarning", tankWarning);
                SQLiteDBClass.PutSettingString(DbSave + "fsdDrive", fsdDrive);

                displayLastFSDOrScoop(lastHE);
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            displayLastFSDOrScoop(lastHE);
        }
    }
}

