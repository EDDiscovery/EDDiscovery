/*
 * Copyright 2025 - 2025 EDDiscovery development team
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
 */

using EliteDangerousCore;
using ExtendedControls;
using QuickJSON;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls.Helpers
{
    internal class EventColours
    {

        public EventColours(string load)
        {
            Reload(load);
        }

        public bool Reload(string load) 
        {
            JToken setting = JToken.Parse(load);
            Object jconv = setting.ToObjectProtected(this.GetType(), false, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public, this,
                            customconverter: (tt, o) => { if (o is string) return System.Drawing.ColorTranslator.FromHtml((string)o); else return Color.Orange; });

            return jconv is EventColours;
        }

        public override string ToString()
        {
            JToken outp = JToken.FromObjectWithError(this, false,
                membersearchflags: System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                customconverter: (mi, obj) => { return JToken.CreateToken(System.Drawing.ColorTranslator.ToHtml((Color)obj)); });

            return outp.ToString(true); 
        }

        public void Edit(Form f, Control below, Action<bool> closed)
        {
            CheckedIconNewListBoxForm frm = new CheckedIconNewListBoxForm();

            var jevents = JournalEntry.GetNameImageOfEvents();
            foreach (var x in jevents)
            {
                var enumv = BaseUtils.TypeHelpers.ParseEnum<JournalTypeEnum>(x.Item1);
                Color? tc = Colors.TryGetValue(enumv, out var color) ? color : default(Color?);
                frm.UC.AddButton(x.Item1, x.Item2, x.Item3, textcolor:tc, usertag:enumv, tooltiptext:"Click left to edit, click right to set to selected colour");
            }

            frm.UC.Sort();  // sorted by text

            string resettext = "Reset Colour";

            frm.UC.AddButton($"Load","Load".Tx(), attop: true, tooltiptext: "Load settings from file");
            frm.UC.AddButton($"Save","Save".Tx(), attop: true, tooltiptext: "Save settings to file");
            frm.UC.AddButton($"Selected", resettext, attop: true, tooltiptext: "Currently selected colour for right clicks. Click left to set right click colour to default");
            frm.UC.AddButton($"Elite", "Elite Theme", BaseUtils.Icons.IconSet.GetImage("Controls.elite24"), attop: true);
            frm.UC.AddButton($"None", "None".Tx(), CheckedIconGroupUserControl.ImageNone, attop: true);

            frm.CloseBoundaryRegion = new Size(64, 64);
            frm.UC.ShowClose = true;
            frm.UC.MultipleColumns = true;
            frm.UC.SlideLeft = true;
            frm.PositionBelow(below);

            var newcolours = new Dictionary<JournalTypeEnum, Color>(Colors);       // clone
            bool changed = false;
            bool closeclicked = false;
            Color? lastcolour = null;

            frm.RemoveCloseButtonHandler();         // need to turn off default behaviour as it installs a closehide action on the close icon. and that means the dialog closes before

            frm.UC.BackColor = Color.AliceBlue;

            frm.UC.CloseClicked += (xx) =>
            {
                if (changed)                        // changed, need to ask
                {
                    // opening a dialog will close the form (close on deactivate), and fire the form closed first, so we need to ignore it below and do all the handling here
                    
                    frm.CloseOnDeactivate = false;          // stop it deactivating due to dialog

                    closeclicked = true;            // don't run close code

                    if (MessageBoxTheme.Show(f, "Keep changes (Yes) or abandon (No)", "Warning".Tx(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        changed = false;
                    }
                    else
                        Colors = newcolours;        // we want then changed, apply
                }

                frm.Close();                        // now close the form

                closed(changed);                    // tell the caller
            };

            frm.FormClosed += (sender, o) =>        // form down,
            {
                if (!closeclicked)                  // if not ignoring
                {
                    if (changed)
                    {
                        Colors = newcolours;        // if changing
                    }
                    
                    closed(changed);                // tell the caller

                    //foreach (var x in Colors)       // debug output for use in making fixed sets below
                    //{
                    //    string cname = x.Value.IsKnownColor ? $"Color.{x.Value.Name}" : $"Color.FromArgb({x.Value.A.ToStringInvariant()},{x.Value.R.ToStringInvariant()},{x.Value.G.ToStringInvariant()},{x.Value.B.ToStringInvariant()})";
                    //    System.Diagnostics.Debug.WriteLine($"        [JournalTypeEnum.{x.Key.ToString()}] = {cname},");
                    //}
                }
            };
            
            frm.UC.ButtonPressed += (index, tag, text, usertag, butarg) => 
            {
                if (usertag != null)
                {
                    var en = (JournalTypeEnum)usertag;
                    Color? tc = newcolours.TryGetValue(en, out var color) ? color : default(Color?);

                    if (butarg.Button == MouseButtons.Left)
                    {
                        ColorDialog colourdialog = new ColorDialog();
                        colourdialog.AllowFullOpen = true;
                        colourdialog.FullOpen = true;
                        colourdialog.Color = tc ?? Theme.Current.GridCellText;

                        frm.CloseOnDeactivate = false;          // stop it deactivating due to dialog

                        if (colourdialog.ShowDialog(f) == DialogResult.OK)
                        {
                            lastcolour = colourdialog.Color;
                            frm.UC.ItemList[2].TextColor = lastcolour;
                            frm.UC.ItemList[2].Text = "Current Colour";

                            if (!tc.HasValue || tc.Value != colourdialog.Color)
                            {
                                newcolours[en] = colourdialog.Color;
                                changed = true;
                            }
                        }

                        frm.CloseOnDeactivate = true;
                    }
                    else if (butarg.Button == MouseButtons.Right)
                    {
                        if (lastcolour == null)
                            newcolours.Remove(en);
                        else
                            newcolours[en] = lastcolour.Value;
                        changed = true;
                    }
                }
                else if (tag == "Elite")
                {
                    if (butarg.Button == MouseButtons.Left)
                    {
                        newcolours = new Dictionary<JournalTypeEnum, Color>(elitecolours);       // clone
                        changed = true;
                    }
                }
                else if (tag == "None")
                {
                    if (butarg.Button == MouseButtons.Left)
                    {
                        newcolours.Clear();
                        changed = true;
                    }
                }
                else if (tag == "Selected")
                {
                    if (butarg.Button == MouseButtons.Left)
                    {
                        frm.UC.ItemList[2].TextColor = lastcolour = null;
                        frm.UC.ItemList[2].Text = resettext;
                        changed = true;
                    }
                }
                else if (tag == "Save")
                {
                    frm.CloseOnDeactivate = false;

                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.Filter = "Query| *.eventtheme";
                    dlg.Title = "Save Journal Colours";

                    // this will close this dialog
                    if (dlg.ShowDialog(f) == DialogResult.OK)
                    {
                        string path = dlg.FileName;
                        if (! BaseUtils.FileHelpers.TryWriteToFile(path,ToString()) )
                        {
                            MessageBoxTheme.Show("Failed to write to ".Tx() + path, "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    frm.CloseOnDeactivate = true;
                }
                else if (tag == "Load")
                {
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.Filter = "Query| *.eventtheme";
                    dlg.Title = "Import Journal Colours";

                    // this will close this dialog
                    if (dlg.ShowDialog(f) == DialogResult.OK)
                    {
                        string path = dlg.FileName;
                        string textin = BaseUtils.FileHelpers.TryReadAllTextFromFile(path);

                        if (textin != null)
                        {
                            if (Reload(textin))
                            {
                                changed = true;
                            }
                        }

                        if (!changed)
                            MessageBoxTheme.Show("Failed to open ".Tx() + path, "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    closed(changed);                    // tell the caller
                    frm.Close();
                    return;
                }

                foreach ( var x in frm.UC.ItemList)          // reset the colour set, if usertag is set (its not a group item).. just do it for all
                {
                    if (x.UserTag != null)
                    {
                        var en = (JournalTypeEnum)x.UserTag;
                        Color? tc = newcolours.TryGetValue(en, out var color) ? color : default(Color?);
                        x.TextColor = tc;
                    }
                }

                frm.ReDraw();
            };

            frm.Show(f);
        }

        static Dictionary<JournalTypeEnum, Color> elitecolours = new Dictionary<JournalTypeEnum, Color>()
        {
            [JournalTypeEnum.FSDJump] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.FSDTarget] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.Location] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.Scan] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.ScanBaryCentre] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.ScanOrganic] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.CommitCrime] = Color.Red,
            [JournalTypeEnum.PayBounties] = Color.Red,
            [JournalTypeEnum.PayFines] = Color.Red,
            [JournalTypeEnum.PayLegacyFines] = Color.Red,
            [JournalTypeEnum.Resurrect] = Color.Red,
            [JournalTypeEnum.SelfDestruct] = Color.Red,
            [JournalTypeEnum.ShipTargeted] = Color.Red,
            [JournalTypeEnum.Scanned] = Color.Red,
            [JournalTypeEnum.SRVDestroyed] = Color.Red,
            [JournalTypeEnum.SupercruiseEntry] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.SupercruiseExit] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.SupercruiseDestinationDrop] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.Touchdown] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.Undocked] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.VehicleSwitch] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.CarrierJumpRequest] = Color.Olive,
            [JournalTypeEnum.CarrierJumpCancelled] = Color.Olive,
            [JournalTypeEnum.CarrierJump] = Color.Olive,
            [JournalTypeEnum.ApproachSettlement] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.LaunchSRV] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.LeaveBody] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.ApproachBody] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.Died] = Color.Red,
            [JournalTypeEnum.DiscoveryScan] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.DockingRequested] = Color.Yellow,
            [JournalTypeEnum.DockingTimeout] = Color.Yellow,
            [JournalTypeEnum.EscapeInterdiction] = Color.Red,
            [JournalTypeEnum.FactionKillBond] = Color.Red,
            [JournalTypeEnum.HeatDamage] = Color.Red,
            [JournalTypeEnum.HeatWarning] = Color.Red,
            [JournalTypeEnum.HullDamage] = Color.Red,
            [JournalTypeEnum.Interdicted] = Color.Red,
            [JournalTypeEnum.Interdiction] = Color.Red,
            [JournalTypeEnum.LaunchFighter] = Color.Red,
            [JournalTypeEnum.Market] = Color.Aqua,
            [JournalTypeEnum.MarketBuy] = Color.Aqua,
            [JournalTypeEnum.MarketSell] = Color.Aqua,
            [JournalTypeEnum.ModuleBuy] = Color.Fuchsia,
            [JournalTypeEnum.ModuleBuyAndStore] = Color.Fuchsia,
            [JournalTypeEnum.ModuleSwap] = Color.Fuchsia,
            [JournalTypeEnum.ModuleInfo] = Color.Fuchsia,
            [JournalTypeEnum.ModuleRetrieve] = Color.Fuchsia,
            [JournalTypeEnum.ModuleSell] = Color.Fuchsia,
            [JournalTypeEnum.ModuleSellRemote] = Color.Fuchsia,
            [JournalTypeEnum.ModuleStore] = Color.Fuchsia,
            [JournalTypeEnum.NavBeaconScan] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.NavRoute] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.NavRouteClear] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.ShipyardBuy] = Color.FromArgb(255, 255, 128, 64),
            [JournalTypeEnum.ShipyardNew] = Color.FromArgb(255, 255, 128, 64),
            [JournalTypeEnum.ShipyardRedeem] = Color.FromArgb(255, 255, 128, 64),
            [JournalTypeEnum.ShipyardSell] = Color.FromArgb(255, 255, 128, 64),
            [JournalTypeEnum.ShipyardSwap] = Color.FromArgb(255, 255, 128, 64),
            [JournalTypeEnum.ShipyardTransfer] = Color.FromArgb(255, 255, 128, 64),
            [JournalTypeEnum.ShipyardBankDeposit] = Color.FromArgb(255, 255, 128, 64),
            [JournalTypeEnum.Shipyard] = Color.FromArgb(255, 255, 128, 64),
            [JournalTypeEnum.CockpitBreached] = Color.Red,
            [JournalTypeEnum.CodexEntry] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.CrewLaunchFighter] = Color.Red,
            [JournalTypeEnum.CrimeVictim] = Color.Red,
            [JournalTypeEnum.Embark] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.DropshipDeploy] = Color.Red,
            [JournalTypeEnum.DockFighter] = Color.Red,
            [JournalTypeEnum.SellExplorationData] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.SellMicroResources] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.SellOrganicData] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.Docked] = Color.Orange,
            [JournalTypeEnum.JoinedSquadron] = Color.Purple,
            [JournalTypeEnum.SquadronCreated] = Color.Purple,
            [JournalTypeEnum.SquadronDemotion] = Color.Purple,
            [JournalTypeEnum.SquadronPromotion] = Color.Purple,
            [JournalTypeEnum.SquadronStartup] = Color.Purple,
            [JournalTypeEnum.KickedFromSquadron] = Color.Purple,
            [JournalTypeEnum.LeftSquadron] = Color.Purple,
            [JournalTypeEnum.InvitedToSquadron] = Color.Purple,
            [JournalTypeEnum.BuyExplorationData] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.BuyTradeData] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.CarrierBankTransfer] = Color.Olive,
            [JournalTypeEnum.CarrierBuy] = Color.Olive,
            [JournalTypeEnum.CarrierCancelDecommission] = Color.Olive,
            [JournalTypeEnum.CarrierCrewServices] = Color.Olive,
            [JournalTypeEnum.CarrierDecommission] = Color.Olive,
            [JournalTypeEnum.CarrierDepositFuel] = Color.Olive,
            [JournalTypeEnum.CarrierDockingPermission] = Color.Olive,
            [JournalTypeEnum.CarrierFinance] = Color.Olive,
            [JournalTypeEnum.CarrierLocation] = Color.Olive,
            [JournalTypeEnum.CarrierModulePack] = Color.Olive,
            [JournalTypeEnum.CarrierNameChange] = Color.Olive,
            [JournalTypeEnum.CarrierShipPack] = Color.Olive,
            [JournalTypeEnum.CarrierStats] = Color.Olive,
            [JournalTypeEnum.CarrierTradeOrder] = Color.Olive,
            [JournalTypeEnum.FighterRebuilt] = Color.Red,
            [JournalTypeEnum.FighterDestroyed] = Color.Red,
            [JournalTypeEnum.FSSAllBodiesFound] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.FSSBodySignals] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.FSSDiscoveryScan] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.FSSSignalDiscovered] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.SAAScanComplete] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.SAASignalsFound] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.MultiSellExplorationData] = Color.FromArgb(255, 118, 118, 239),
            [JournalTypeEnum.BookDropship] = Color.Red,
            [JournalTypeEnum.BookTaxi] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.BuyAmmo] = Color.Red,
            [JournalTypeEnum.BuyWeapon] = Color.Red,
            [JournalTypeEnum.ShieldState] = Color.Red,
            [JournalTypeEnum.UnderAttack] = Color.Red,
            [JournalTypeEnum.Powerplay] = Color.FromArgb(255, 145, 43, 63),
            [JournalTypeEnum.PowerplayCollect] = Color.FromArgb(255, 145, 43, 63),
            [JournalTypeEnum.PowerplayDefect] = Color.FromArgb(255, 145, 43, 63),
            [JournalTypeEnum.PowerplayDeliver] = Color.FromArgb(255, 145, 43, 63),
            [JournalTypeEnum.PowerplayFastTrack] = Color.FromArgb(255, 145, 43, 63),
            [JournalTypeEnum.PowerplayJoin] = Color.FromArgb(255, 145, 43, 63),
            [JournalTypeEnum.PowerplayLeave] = Color.FromArgb(255, 145, 43, 63),
            [JournalTypeEnum.PowerplayMerits] = Color.FromArgb(255, 145, 43, 63),
            [JournalTypeEnum.PowerplayRank] = Color.FromArgb(255, 145, 43, 63),
            [JournalTypeEnum.PowerplaySalary] = Color.FromArgb(255, 145, 43, 63),
            [JournalTypeEnum.PowerplayVote] = Color.FromArgb(255, 145, 43, 63),
            [JournalTypeEnum.PowerplayVoucher] = Color.FromArgb(255, 145, 43, 63),
            [JournalTypeEnum.RedeemVoucher] = Color.Red,
            [JournalTypeEnum.RefuelAll] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.RefuelPartial] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.Liftoff] = Color.FromArgb(255, 68, 213, 43),
            [JournalTypeEnum.DisbandedSquadron] = Color.Purple,
            [JournalTypeEnum.CancelDropship] = Color.Red,
            [JournalTypeEnum.CapShipBond] = Color.Red,
        };


        [JsonCustomFormatArray]
        public Dictionary<JournalTypeEnum, Color> Colors { get; set; } = new Dictionary<JournalTypeEnum, Color>();

    }



}
