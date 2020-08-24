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
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSysInfo : UserControlCommonBase
    {
        public bool IsNotesShowing { get { return richTextBoxNote.Visible; } }

        private string DbSelection { get { return DBName("SystemInformationPanel", "Sel"); } }
        private string DbOSave { get { return DBName("SystemInformationPanel", "Order"); } }

        const int BitSelSystem = 0;
        const int BitSelEDSM = 1;
        const int BitSelVisits = 2;
        const int BitSelBody = 3;
        const int BitSelPosition = 4;
        const int BitSelDistanceFrom = 5;
        const int BitSelSystemState = 6;
        const int BitSelNotes = 7;
        const int BitSelTarget = 8;
        const int BitSelShipInfo = 9;
        const int BitSelFuel = 10;
        const int BitSelCargo = 11;
        const int BitSelMats = 12;
        const int BitSelData = 13;
        const int BitSelCredits = 14;
        const int BitSelGameMode = 15;
        const int BitSelTravel = 16;
        const int BitSelMissions = 17;
        const int BitSelJumpRange = 18;
        const int BitSelStationButtons = 19;
        const int BitSelShipyardButtons = 20;
        const int BitSelTotal = 21;
        const int BitSelDefault = ((1 << BitSelTotal) - 1) + (1 << BitSelEDSMButtonsNextLine);

        int[,] resetorder = new int[,]
        {
            {BitSelSystem,-1},
            {BitSelPosition,-1},
            {BitSelEDSM,-1},
            {BitSelVisits,BitSelCredits},
            {BitSelBody,-1},
            {BitSelStationButtons,-1},
            {BitSelShipInfo,-1},
            {BitSelShipyardButtons,-1},
            {BitSelDistanceFrom,-1},
            {BitSelSystemState,-1},
            {BitSelNotes,-1},
            {BitSelTarget,-1},
            {BitSelFuel,BitSelCargo},
            {BitSelMats,BitSelData},
            {BitSelGameMode,-1},
            {BitSelTravel,-1},
            {BitSelMissions,-1},
            {BitSelJumpRange,-1},
        };

        int[] SmallItems = new int[] { BitSelFuel, BitSelCargo, BitSelVisits, BitSelMats, BitSelData, BitSelCredits, BitSelJumpRange };

        const int BitSelEDSMButtonsNextLine = 28;       // other options
        const int BitSelSkinny = 29;

        public const int HorzPositions = 8;
        const int hspacing = 2;

        ToolStripMenuItem[] toolstriplist;          // ref to toolstrip items for each bit above. in same order as bits BitSel..

        int Selection;          // selection bits
        List<BaseUtils.LineStore> Lines;            // stores settings on each line, values are BitN+1, 0 means position not used.

        #region Init

        public UserControlSysInfo()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            textBoxTarget.SetAutoCompletor(SystemCache.ReturnSystemAutoCompleteList);

            // same order as Sel bits are defined in, one bit per selection item.
            toolstriplist = new ToolStripMenuItem[]
            {   toolStripSystem , toolStripEDSM , toolStripVisits, toolStripBody,
                toolStripPosition, toolStripDistanceFrom,
                toolStripSystemState, toolStripNotes, toolStripTarget,
                toolStripShip, toolStripFuel , toolStripCargo, toolStripMaterialCounts,  toolStripDataCount,
                toolStripCredits,
                toolStripGameMode,toolStripTravel, toolStripMissionList,
                toolStripJumpRange, displayStationButtonsToolStripMenuItem,
                displayShipButtonsToolStripMenuItem,
            };

            Debug.Assert(toolstriplist.Length == BitSelTotal);

            Selection = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbSelection, BitSelDefault);

            string rs = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbOSave, "-");
            if (rs == "-")
                Reset();
            else
                Lines = BaseUtils.LineStore.Restore(rs, HorzPositions);

            for (int bit = 0; bit < BitSelTotal; bit++)     // new bits added will not be in older lists, need to add on in!
            {
                if (BaseUtils.LineStore.FindValue(Lines, bit + 1) == null)   // if can't find
                {
                    int insertat = Lines.Count;

                    if (bit == BitSelStationButtons)
                    {
                        var p = BaseUtils.LineStore.FindValue(Lines, BitSelBody + 1);     // stored +1
                        if (p != null)
                            insertat = Lines.IndexOf(p) + 1;

                        Selection |= (1 << bit);
                    }
                    else if (bit == BitSelShipyardButtons)
                    {
                        var p = BaseUtils.LineStore.FindValue(Lines, BitSelShipInfo + 1);
                        if (p != null)
                            insertat = Lines.IndexOf(p) + 1;

                        Selection |= (1 << bit);
                    }

                    Lines.Insert(insertat, new BaseUtils.LineStore() { Items = new int[HorzPositions] { bit + 1, 0, 0, 0, 0, 0, 0, 0 } });
                }
            }

            discoveryform.OnNewTarget += RefreshTargetDisplay;
            discoveryform.OnNoteChanged += OnNoteChanged;
            discoveryform.OnEDSMSyncComplete += Discoveryform_OnEDSMSyncComplete;
            discoveryform.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            discoveryform.OnThemeChanged += Discoveryform_OnThemeChanged;

            panelFD.BackgroundImage = EDDiscovery.Icons.Controls.notfirstdiscover;      // just to hide it during boot up

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip1, this);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
            //System.Diagnostics.Debug.WriteLine("UCTG changed in sysinfo to " + uctg.GetHashCode());
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;    // get this whenever current selection or refreshed..
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewTarget -= RefreshTargetDisplay;
            discoveryform.OnNoteChanged -= OnNoteChanged;
            discoveryform.OnEDSMSyncComplete -= Discoveryform_OnEDSMSyncComplete;
            discoveryform.OnNewUIEvent -= Discoveryform_OnNewUIEvent;

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbOSave, BaseUtils.LineStore.ToString(Lines));
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DbSelection, Selection);
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        private void Discoveryform_OnEDSMSyncComplete(int count, string syslist)     // EDSM ~MAY~ have updated the last discovery flag, so redisplay
        {
            //System.Diagnostics.Debug.WriteLine("EDSM SYNC COMPLETED with " + count + " '" + syslist + "'");
            Display(last_he, discoveryform.history);
        }

        private void Discoveryform_OnNewUIEvent(UIEvent obj)
        {
            if (obj is EliteDangerousCore.UIEvents.UIFuel) // fuel UI update the SI information globally.
                Display(last_he, discoveryform.history);
        }

        bool neverdisplayed = true;
        HistoryEntry last_he = null;

        private void Display(HistoryEntry he, HistoryList hl) =>
            Display(he, hl, true);

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            //System.Diagnostics.Debug.WriteLine("SI:Display ");

            if (neverdisplayed)
            {
                UpdateViewOnSelection();  // then turn the right ones on
                neverdisplayed = false;
            }

            last_he = he;

            if (last_he != null)
            {
                SetControlText(he.System.Name);

                HistoryEntry lastfsd = hl.GetLastHistoryEntry(x => x.journalEntry is EliteDangerousCore.JournalEvents.JournalFSDJump, he);

                textBoxSystem.Text = he.System.Name;
                panelFD.BackgroundImage = (lastfsd != null && (lastfsd.journalEntry as EliteDangerousCore.JournalEvents.JournalFSDJump).EDSMFirstDiscover) ? EDDiscovery.Icons.Controls.firstdiscover : EDDiscovery.Icons.Controls.notfirstdiscover;

                if (!he.System.HasEDDBInformation || !he.System.HasCoordinate)
                    discoveryform.history.FillEDSM(he); // Fill in any EDSM info we have

                //textBoxBody.Text = he.WhereAmI + ((he.IsInHyperSpace) ? " (HS)": "");
                textBoxBody.Text = he.WhereAmI + " (" + he.BodyType + ")";

                bool hasmarketid = he?.MarketID.HasValue ?? false;
                bool hasbodyormarketid = hasmarketid || he.FullBodyID.HasValue;

                extButtonEDDBStation.Enabled = extButtonInaraStation.Enabled = hasmarketid;
                extButtonSpanshStation.Enabled = hasbodyormarketid;

                if (he.System.HasCoordinate)         // cursystem has them?
                {
                    string SingleCoordinateFormat = "0.##";

                    string separ = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";

                    textBoxPosition.Text = he.System.X.ToString(SingleCoordinateFormat) + separ + he.System.Y.ToString(SingleCoordinateFormat) + separ + he.System.Z.ToString(SingleCoordinateFormat);

                    ISystem homesys = EDCommander.Current.HomeSystemI;
                    textBoxHomeDist.Text = homesys != null ? he.System.Distance(homesys).ToString(SingleCoordinateFormat) : "---";
                    textBoxSolDist.Text = he.System.Distance(0, 0, 0).ToString(SingleCoordinateFormat);
                }
                else
                {
                    textBoxPosition.Text = "?";
                    textBoxHomeDist.Text = "";
                    textBoxSolDist.Text = "";
                }

                int count = discoveryform.history.GetVisitsCount(he.System.Name);
                textBoxVisits.Text = count.ToString();

                System.Diagnostics.Debug.WriteLine("UserControlSysInfo sys info {0} {1} {2}", he.System.Name, he.System.EDSMID, he.System.EDDBID);

                extButtonEDSMSystem.Enabled = extButtonRossSystem.Enabled = extButtonEDDBSystem.Enabled = extButtonInaraSystem.Enabled = extButtonSpanshSystem.Enabled = true;

                string allegiance, economy, gov, faction, factionstate, security;
                hl.ReturnSystemInfo(he, out allegiance, out economy, out gov, out faction, out factionstate, out security);

                textBoxAllegiance.Text = allegiance;
                textBoxEconomy.Text = economy;
                textBoxGovernment.Text = gov;
                textBoxState.Text = factionstate;

                List<MissionState> mcurrent = (from MissionState ms in he.MissionList.Missions.Values where ms.InProgressDateTime(last_he.EventTimeUTC) orderby ms.Mission.EventTimeUTC descending select ms).ToList();

                if (mcurrent == null || mcurrent.Count == 0)
                    richTextBoxScrollMissions.Text = "No Missions".T(EDTx.UserControlSysInfo_NoMissions);
                else
                {
                    string t = "";
                    foreach (MissionState ms in mcurrent)
                    {
                        DateTime exp = EliteConfigInstance.InstanceConfig.ConvertTimeToSelectedFromUTC(ms.Mission.Expiry);

                        t = ObjectExtensionsStrings.AppendPrePad(t,
                            JournalFieldNaming.ShortenMissionName(ms.Mission.Name)
                            + " Exp:" + exp
                            + " @ " + ms.DestinationSystemStation(),
                            Environment.NewLine);
                    }

                    richTextBoxScrollMissions.Text = t;
                }

                SetNote(he.snc != null ? he.snc.Note : "");
                textBoxGameMode.Text = he.GameModeGroup;
                if (he.isTravelling)
                {
                    textBoxTravelDist.Text = he.TravelledDistance.ToString("0.0") + "ly";
                    textBoxTravelTime.Text = he.TravelledSeconds.ToString();
                    textBoxTravelJumps.Text = he.TravelledJumpsAndMisses;
                }
                else
                {
                    textBoxTravelDist.Text = textBoxTravelTime.Text = textBoxTravelJumps.Text = "";
                }

                int cc = (he.ShipInformation) != null ? he.ShipInformation.CargoCapacity() : 0;
                if (cc > 0)
                    textBoxCargo.Text = he.MaterialCommodity.CargoCount.ToString() + "/" + cc.ToString();
                else
                    textBoxCargo.Text = he.MaterialCommodity.CargoCount.ToString();

                textBoxMaterials.Text = he.MaterialCommodity.MaterialsCount.ToString();
                textBoxData.Text = he.MaterialCommodity.DataCount.ToString();
                textBoxCredits.Text = he.Credits.ToString("N0");

                textBoxJumpRange.Text = "";

                if (he.ShipInformation != null)
                {
                    ShipInformation si = he.ShipInformation;

                    textBoxShip.Text = si.ShipFullInfo(cargo: false, fuel: false);
                    if (si.FuelCapacity > 0 && si.FuelLevel > 0)
                        textBoxFuel.Text = si.FuelLevel.ToString("0.#") + "/" + si.FuelCapacity.ToString("0.#");
                    else if (si.FuelCapacity > 0)
                        textBoxFuel.Text = si.FuelCapacity.ToString("0.#");
                    else
                        textBoxFuel.Text = "N/A".T(EDTx.UserControlSysInfo_NA);

                    EliteDangerousCalculations.FSDSpec fsd = si.GetFSDSpec();
                    if (fsd != null)
                    {
                        EliteDangerousCalculations.FSDSpec.JumpInfo ji = fsd.GetJumpInfo(he.MaterialCommodity.CargoCount,
                                                                    si.ModuleMass() + si.HullMass(), si.FuelLevel, si.FuelCapacity / 2);

                        //System.Diagnostics.Debug.WriteLine("Jump range " + si.FuelLevel + " " + si.FuelCapacity + " " + ji.cursinglejump);
                        textBoxJumpRange.Text = ji.cursinglejump.ToString("N2") + "ly";
                    }

                    extButtonCoriolis.Enabled = extButtonEDSY.Enabled = true;
                }
                else
                {
                    textBoxShip.Text = textBoxFuel.Text = "";
                    extButtonCoriolis.Enabled = extButtonEDSY.Enabled = false;
                }

                RefreshTargetDisplay(this);
            }
            else
            {
                SetControlText("");
                textBoxSystem.Text = textBoxBody.Text = textBoxPosition.Text =
                                textBoxAllegiance.Text = textBoxEconomy.Text = textBoxGovernment.Text =
                                textBoxVisits.Text = textBoxState.Text = textBoxHomeDist.Text = textBoxSolDist.Text =
                                textBoxGameMode.Text = textBoxTravelDist.Text = textBoxTravelTime.Text = textBoxTravelJumps.Text =
                                textBoxCargo.Text = textBoxMaterials.Text = textBoxData.Text = textBoxShip.Text = textBoxFuel.Text =
                                "";

                extButtonEDSMSystem.Enabled = extButtonRossSystem.Enabled = extButtonEDDBSystem.Enabled = extButtonInaraSystem.Enabled = extButtonSpanshSystem.Enabled = false;
                extButtonEDDBStation.Enabled = extButtonInaraStation.Enabled = extButtonSpanshStation.Enabled = false;
                extButtonCoriolis.Enabled = extButtonEDSY.Enabled = false;
                SetNote("");
            }
        }

        private void SetNote(string text)
        {
            noteenabled = false;
            //System.Diagnostics.Debug.WriteLine("SI:Note text " + text);
            richTextBoxNote.Text = text;
            noteenabled = true;
        }

        #endregion

        #region Clicks

        private void buttonEDDBSystem_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                System.Diagnostics.Process.Start(Properties.Resources.URLEDDBSystemName + HttpUtility.UrlEncode(last_he.System.Name));
            }
        }

        private void buttonRossSystem_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                discoveryform.history.FillEDSM(last_he);

                if (last_he.System.EDDBID > 0)
                    Process.Start(Properties.Resources.URLRossSystem + last_he.System.EDDBID.ToString());
                else
                    extButtonRossSystem.Enabled = extButtonEDDBSystem.Enabled = false;
            }
        }

        private void buttonEDSMSystem_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                discoveryform.history.FillEDSM(last_he);

                if (last_he.System != null) // solve a possible exception
                {
                    if (!String.IsNullOrEmpty(last_he.System.Name))
                    {
                        long? id_edsm = last_he.System.EDSMID;
                        if (id_edsm <= 0)
                        {
                            id_edsm = null;
                        }

                        EDSMClass edsm = new EDSMClass();
                        string url = edsm.GetUrlToEDSMSystem(last_he.System.Name, id_edsm);

                        if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                            Process.Start(url);
                        else
                            ExtendedControls.MessageBoxTheme.Show(FindForm(), "System unknown to EDSM".T(EDTx.UserControlSysInfo_SysUnk));
                    }
                }
            }
        }

        private void extButtonInaraSystem_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                string uri = Properties.Resources.URLInaraStarSystem + HttpUtility.UrlEncode(last_he.System.Name);
                BaseUtils.BrowserInfo.LaunchBrowser(uri);
            }
        }

        private void extButtonSpanshSystem_Click(object sender, EventArgs e)
        {
            if (last_he != null && last_he.System.SystemAddress.HasValue)
                System.Diagnostics.Process.Start(Properties.Resources.URLSpanshSystemSystemId + last_he.System.SystemAddress.Value.ToStringInvariant());
        }

        private void extButtonInaraStation_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                string uri = Properties.Resources.URLInaraStation + HttpUtility.UrlEncode(last_he.System.Name + "[" + last_he.WhereAmI + "]");
                BaseUtils.BrowserInfo.LaunchBrowser(uri);
            }
        }

        private void extButtonEDDBStation_Click(object sender, EventArgs e)
        {
            if (last_he != null && last_he.MarketID != null)
                System.Diagnostics.Process.Start(Properties.Resources.URLEDDBStationMarketId + last_he.MarketID.ToStringInvariant());
        }

        private void extButtonSpanshStation_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                if (last_he.MarketID != null)
                    System.Diagnostics.Process.Start(Properties.Resources.URLSpanshStationMarketId + last_he.MarketID.ToStringInvariant());
                else if (last_he.FullBodyID.HasValue)
                    System.Diagnostics.Process.Start(Properties.Resources.URLSpanshBodyId + last_he.FullBodyID.ToStringInvariant());
            }
        }

        private void extButtonEDSY_Click(object sender, EventArgs e)
        {
            if (last_he?.ShipInformation != null)
            {
                var si = last_he.ShipInformation;

                string loadoutjournalline = si.ToJSONLoadout();

                //     File.WriteAllText(@"c:\code\loadoutout.txt", loadoutjournalline);

                string uri = EDDConfig.Instance.EDDShipyardURL + "#/I=" + BaseUtils.HttpUriEncode.URIGZipBase64Escape(loadoutjournalline);

                if (!BaseUtils.BrowserInfo.LaunchBrowser(uri))
                {
                    ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                    info.Info("Cannot launch browser, use this JSON for manual ED Shipyard import", FindForm().Icon, loadoutjournalline);
                    info.ShowDialog(FindForm());
                }
            }
        }

        private void extButtonCoriolis_Click(object sender, EventArgs e)
        {
            if (last_he?.ShipInformation != null)
            {
                var si = last_he.ShipInformation;

                string errstr;
                string s = si.ToJSONCoriolis(out errstr);

                if (errstr.Length > 0)
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), errstr + Environment.NewLine + "This is probably a new or powerplay module" + Environment.NewLine + "Report to EDD Team by Github giving the full text above", "Unknown Module Type");

                string uri = EDDConfig.Instance.CoriolisURL + "data=" + BaseUtils.HttpUriEncode.URIGZipBase64Escape(s) + "&bn=" + Uri.EscapeDataString(si.Name);

                if (!BaseUtils.BrowserInfo.LaunchBrowser(uri))
                {
                    ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                    info.Info("Cannot launch browser, use this JSON for manual Coriolis import", FindForm().Icon, s);
                    info.ShowDialog(FindForm());
                }
            }
        }

        private void textBoxTarget_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TargetHelpers.SetTargetSystem(this, discoveryform, textBoxTarget.Text);
            }
        }

        private void RefreshTargetDisplay(Object sender)              // called when a target has been changed.. via EDDiscoveryform
        {
            string name;
            double x, y, z;

            //System.Diagnostics.Debug.WriteLine("Refresh target display");

            if (TargetClass.GetTargetPosition(out name, out x, out y, out z))
            {
                textBoxTarget.Text = name;
                textBoxTarget.Select(textBoxTarget.Text.Length, textBoxTarget.Text.Length);
                textBoxTargetDist.Text = "No Pos".T(EDTx.NoPos);

                HistoryEntry cs = discoveryform.history.GetLastWithPosition;
                if (cs != null)
                    textBoxTargetDist.Text = cs.System.Distance(x, y, z).ToString("0.0");

                textBoxTarget.SetTipDynamically(toolTip1, string.Format("Position is {0:0.00},{1:0.00},{2:0.00}".T(EDTx.UserControlSysInfo_Pos), x, y, z));
            }
            else
            {
                textBoxTarget.Text = "?";
                textBoxTargetDist.Text = "";
                textBoxTarget.SetTipDynamically(toolTip1, "On 3D Map right click to make a bookmark, region mark or click on a notemark and then tick on Set Target, or type it here and hit enter".T(EDTx.UserControlSysInfo_Target));
            }
        }

        private void buttonEDSMTarget_Click(object sender, EventArgs e)
        {
            string name;
            long? edsmid = null;
            double x, y, z;

            if (TargetClass.GetTargetPosition(out name, out x, out y, out z))
            {
                ISystem sc = this.discoveryform.history.FindSystem(TargetClass.GetNameWithoutPrefix(name), discoveryform.galacticMapping);

                if (sc != null)
                {
                    name = sc.Name;
                    edsmid = sc.EDSMID;
                }
            }

            EDSMClass edsm = new EDSMClass();
            string url = edsm.GetUrlToEDSMSystem(name, edsmid);

            if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                Process.Start(url);
            else
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System unknown to EDSM".T(EDTx.UserControlSysInfo_SysUnk));

        }

        private void clickTextBox(object sender, EventArgs e)
        {
            SetClipboardText(((Control)sender).Text);
        }

        private void toolStripSystem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelSystem);
        }
        private void toolStripBody_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelBody);
        }
        private void toolStripNotes_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelNotes);
        }
        private void toolStripTarget_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelTarget);
        }
        private void toolStripEDSMButtons_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelEDSMButtonsNextLine);
        }
        private void toolStripEDSM_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelEDSM);
        }
        private void toolStripVisits_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelVisits);
        }
        private void toolStripPosition_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelPosition);
        }
        private void enableDistanceFromToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelDistanceFrom);
        }
        private void toolStripSystemState_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelSystemState);
        }
        private void toolStripGameMode_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelGameMode);
        }
        private void toolStripTravel_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelTravel);
        }
        private void toolStripCargo_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelCargo);
        }
        private void toolStripMaterialCount_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelMats);
        }
        private void toolStripDataCount_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelData);
        }
        private void toolStripCredits_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelCredits);
        }
        private void toolStripShip_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelShipInfo);
        }
        private void toolStripFuel_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelFuel);
        }

        private void displayJumpRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelJumpRange);
        }

        private void toolStripMissionsList_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelMissions);
        }
        private void displayStationButtonsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelStationButtons);
        }

        private void displayShipButtonsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelShipyardButtons);
        }

        private void toolStripRemoveAll_Click(object sender, EventArgs e)
        {
            Selection = (Selection | ((1 << BitSelTotal) - 1)) ^ ((1 << BitSelTotal) - 1);
            UpdateViewOnSelection();
        }

        private void whenTransparentUseSkinnyLookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelSkinny);
        }

        void ToggleSelection(Object sender, int bit)
        {
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            if (mi.Enabled)
            {
                Selection ^= (1 << bit);
                UpdateViewOnSelection();
            }
        }

        void UpdateViewOnSelection()
        {
            SuspendLayout();

            foreach (Control c in extPanelScroll.Controls)
            {
                if (!(c is ExtendedControls.ExtScrollBar))
                    c.Visible = false;
            }

            //System.Diagnostics.Debug.WriteLine("Selection is " + sel);

            bool selEDSMonNextLine = (Selection & (1 << BitSelEDSMButtonsNextLine)) != 0;
            toolStripEDSMDownLine.Checked = selEDSMonNextLine;
            toolStripSkinny.Checked = (Selection & (1 << BitSelSkinny)) != 0;

            int data1offset = textBoxCredits.Left - labelCredits.Left;      // offset between first item to initial label - basing it on actual pos allow the auto font scale to work
            int lab2offset = textBoxCredits.Right + 4 - labelCredits.Left;  // offset between second label and initial label
            int data2offset = lab2offset + data1offset;                     // offset between data 2 pos and initial label
            int coloffset = lab2offset;                                     // offset between each column

            //System.Diagnostics.Debug.WriteLine("Sys Info first data {0} second lab {1} second data {2} col offset {3}", data1offset, lab2offset, data2offset, coloffset);

            int ypos = 3;

            for (int r = 0; r < Lines.Count; r++)
            {
                int rowbottom = ypos;
                Lines[r].YStart = -1;

                for (int c = 0; c < HorzPositions; c++)
                {
                    int bitno = Lines[r].Items[c] - 1;    // stored +1

                    if (bitno >= 0 && bitno < toolstriplist.Length)     // ensure within range, ignore any out of range, in case going backwards in versions
                    {
                        bool ison = (Selection & (1 << bitno)) != 0;

                        toolstriplist[bitno].Enabled = false;
                        toolstriplist[bitno].Checked = ison;
                        toolstriplist[bitno].Enabled = true;

                        int itembottom = rowbottom;

                        if (ison)
                        {
                            Lines[r].YStart = ypos;
                            int si = r * HorzPositions + c;

                            Point labpos = new Point(3 + c * coloffset, ypos);
                            Point datapos = new Point(labpos.X + data1offset, labpos.Y);
                            Point labpos2 = new Point(labpos.X + lab2offset, labpos.Y);
                            Point datapos2 = new Point(labpos.X + data2offset, labpos.Y);

                            switch (bitno)
                            {
                                case BitSelSystem:
                                    itembottom = this.SetPos(labpos, labelSysName, datapos, textBoxSystem, si);
                                    panelFD.Location = new Point(textBoxSystem.Right, textBoxSystem.Top);
                                    panelFD.Visible = true;

                                    if (!selEDSMonNextLine && (Selection & (1 << BitSelEDSM)) != 0)
                                    {
                                        extButtonEDSMSystem.Location = new Point(panelFD.Right + hspacing, datapos.Y);
                                        extButtonEDDBSystem.Location = new Point(extButtonEDSMSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonInaraSystem.Location = new Point(extButtonEDDBSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonSpanshSystem.Location = new Point(extButtonInaraSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonRossSystem.Location = new Point(extButtonSpanshSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonEDSMSystem.Visible = extButtonEDDBSystem.Visible = extButtonRossSystem.Visible = extButtonInaraSystem.Visible = extButtonSpanshSystem.Visible = true;
                                        extButtonEDSMSystem.Tag = extButtonEDDBSystem.Tag = extButtonRossSystem.Tag = extButtonInaraSystem.Tag = extButtonSpanshSystem.Tag = si;
                                        itembottom = Math.Max(extButtonEDSMSystem.Bottom, itembottom);
                                    }

                                    break;

                                case BitSelEDSM:
                                    if (selEDSMonNextLine)
                                    {
                                        labelOpen.Location = labpos;
                                        extButtonEDSMSystem.Location = new Point(datapos.X, datapos.Y);
                                        extButtonEDDBSystem.Location = new Point(extButtonEDSMSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonInaraSystem.Location = new Point(extButtonEDDBSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonSpanshSystem.Location = new Point(extButtonInaraSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonRossSystem.Location = new Point(extButtonSpanshSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        labelOpen.Tag = extButtonEDSMSystem.Tag = extButtonEDDBSystem.Tag = extButtonRossSystem.Tag = extButtonInaraSystem.Tag = extButtonSpanshSystem.Tag = si;
                                        extButtonEDSMSystem.Visible = extButtonEDDBSystem.Visible = extButtonRossSystem.Visible = extButtonInaraSystem.Visible = extButtonSpanshSystem.Visible = true;
                                        labelOpen.Visible = true;
                                        itembottom = extButtonEDSMSystem.Bottom;
                                    }
                                    break;

                                case BitSelStationButtons:
                                    labelOpenStation.Location = labpos;
                                    extButtonEDDBStation.Location = new Point(datapos.X, datapos.Y);
                                    extButtonInaraStation.Location = new Point(extButtonEDDBStation.Right + hspacing, extButtonEDDBStation.Top);
                                    extButtonSpanshStation.Location = new Point(extButtonInaraStation.Right + hspacing, extButtonEDDBStation.Top);
                                    labelOpenStation.Tag = extButtonEDDBStation.Tag = extButtonInaraStation.Tag = extButtonSpanshStation.Tag = si;
                                    extButtonEDDBStation.Visible = extButtonInaraStation.Visible = extButtonSpanshStation.Visible = true;
                                    labelOpenStation.Visible = true;
                                    itembottom = extButtonEDDBStation.Bottom;
                                    break;

                                case BitSelShipyardButtons:
                                    labelOpenShip.Location = labpos;
                                    extButtonCoriolis.Location = new Point(datapos.X, datapos.Y);
                                    extButtonEDSY.Location = new Point(extButtonCoriolis.Right + hspacing, extButtonCoriolis.Top);
                                    labelOpenShip.Tag = extButtonEDSY.Tag = extButtonCoriolis.Tag = si;
                                    extButtonEDSY.Visible = extButtonCoriolis.Visible = true;
                                    labelOpenShip.Visible = true;
                                    itembottom = extButtonCoriolis.Bottom;
                                    break;


                                case BitSelVisits:
                                    itembottom = this.SetPos(labpos, labelVisits, datapos, textBoxVisits, si);
                                    break;

                                case BitSelBody:
                                    itembottom = this.SetPos(labpos, labelBodyName, datapos, textBoxBody, si);
                                    break;

                                case BitSelPosition:
                                    itembottom = this.SetPos(labpos, labelPosition, datapos, textBoxPosition, si);
                                    break;

                                case BitSelDistanceFrom:
                                    itembottom = this.SetPos(labpos, labelHomeDist, datapos, textBoxHomeDist, si);
                                    OffsetPos(labpos2, labelSolDist, datapos2, textBoxSolDist, si);
                                    break;

                                case BitSelSystemState:
                                    itembottom = this.SetPos(labpos, labelState, datapos, textBoxState, si);
                                    OffsetPos(labpos2, labelAllegiance, datapos2, textBoxAllegiance, si);
                                    labpos.Y = datapos.Y = labpos2.Y = datapos2.Y = itembottom + 1;
                                    itembottom = this.SetPos(labpos, labelGov, datapos, textBoxGovernment, si);
                                    OffsetPos(labpos2, labelEconomy, datapos2, textBoxEconomy, si);
                                    break;

                                case BitSelNotes:
                                    itembottom = SetPos(labpos, labelNote, datapos, richTextBoxNote, si);
                                    break;

                                case BitSelTarget:
                                    itembottom = this.SetPos(labpos, labelTarget, datapos, textBoxTarget, si);
                                    textBoxTargetDist.Location = new Point(textBoxTarget.Right + hspacing, datapos.Y);
                                    extButtonEDSMTarget.Location = new Point(textBoxTargetDist.Right + hspacing, datapos.Y);
                                    textBoxTargetDist.Tag = extButtonEDSMTarget.Tag = si;
                                    textBoxTargetDist.Visible = extButtonEDSMTarget.Visible = true;
                                    break;

                                case BitSelGameMode:
                                    itembottom = this.SetPos(labpos, labelGamemode, datapos, textBoxGameMode, si);
                                    break;

                                case BitSelTravel:
                                    itembottom = this.SetPos(labpos, labelTravel, datapos, textBoxTravelDist, si);
                                    textBoxTravelTime.Location = new Point(textBoxTravelDist.Right + hspacing, datapos.Y);
                                    textBoxTravelJumps.Location = new Point(textBoxTravelTime.Right + hspacing, datapos.Y);
                                    textBoxTravelTime.Tag = textBoxTravelJumps.Tag = si;
                                    textBoxTravelTime.Visible = textBoxTravelJumps.Visible = true;
                                    // don't set visible for the last two, may not be if not travelling. Display will deal with it
                                    break;

                                case BitSelCargo:
                                    itembottom = this.SetPos(labpos, labelCargo, datapos, textBoxCargo, si);
                                    break;

                                case BitSelMats:
                                    itembottom = this.SetPos(labpos, labelMaterials, datapos, textBoxMaterials, si);
                                    break;

                                case BitSelData:
                                    itembottom = this.SetPos(labpos, labelData, datapos, textBoxData, si);
                                    break;

                                case BitSelShipInfo:
                                    itembottom = this.SetPos(labpos, labelShip, datapos, textBoxShip, si);
                                    break;

                                case BitSelFuel:
                                    itembottom = this.SetPos(labpos, labelFuel, datapos, textBoxFuel, si);
                                    break;

                                case BitSelCredits:
                                    itembottom = this.SetPos(labpos, labelCredits, datapos, textBoxCredits, si);
                                    break;

                                case BitSelMissions:
                                    itembottom = this.SetPos(labpos, labelMissions, datapos, richTextBoxScrollMissions, si);
                                    break;

                                case BitSelJumpRange:
                                    itembottom = this.SetPos(labpos, labelJumpRange, datapos, textBoxJumpRange, si);
                                    break;

                                default:
                                    System.Diagnostics.Debug.WriteLine("Ignoring unknown type");
                                    break;
                            }

                            rowbottom = Math.Max(itembottom + 4, rowbottom);        // update vertical, 
                            Lines[r].YEnd = rowbottom - 1;
                        }
                    }
                }

                //System.Diagnostics.Debug.WriteLine("{0} : Ypos {1}-{2}", r, Lines[r].YStart, Lines[r].YEnd);
                ypos = rowbottom;
            }

            ResumeLayout();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
            UpdateViewOnSelection();
        }

        public void Reset()
        {
            Selection = BitSelDefault;
            Lines = new List<BaseUtils.LineStore>();
            for(int i = 0 ; i < resetorder.Length/2; i++ )
                Lines.Add(new BaseUtils.LineStore() { Items = new int[HorzPositions] { resetorder[i,0]+1, resetorder[i,1]+1, 0, 0, 0, 0, 0, 0 } });
            //BaseUtils.LineStore.DumpOrder(Lines, "Reset");
        }

        int SetPos(Point lp, Label lab, Point tp, ExtendedControls.ExtTextBox box, int i )
        {
            lab.Location = lp;
            box.Location = tp;
            box.Tag = lab.Tag = i;
            lab.Visible = box.Visible = true;
            return Math.Max(lab.Bottom, box.Bottom);
        }

        int SetPos(Point lp, Label lab, Point tp, ExtendedControls.ExtRichTextBox box, int i)
        {
            lab.Location = lp;
            box.Location = tp;
            box.Tag = lab.Tag = i;
            lab.Visible = box.Visible = true;
            return Math.Max(lab.Bottom, box.Bottom);
        }

        int OffsetPos(Point lp, Label lab, Point tp, ExtendedControls.ExtTextBox box , int i)
        {
            lab.Location = lp;
            box.Location = tp;
            box.Tag = lab.Tag = i;
            lab.Visible = box.Visible = true;
            return Math.Max(lab.Bottom, box.Bottom);
        }

        #endregion

        #region Notes

        bool noteenabled = true;
        private void richTextBoxNote_Leave(object sender, EventArgs e)
        {
            if (last_he != null && noteenabled)
            {
                //System.Diagnostics.Debug.WriteLine("SI:leave note changed: " + richTextBoxNote.Text);
                last_he.SetJournalSystemNoteText(richTextBoxNote.Text.Trim(), true , EDCommander.Current.SyncToEdsm);   // commit, maybe send to edsm
                discoveryform.NoteChanged(this, last_he, true);
            }
        }

        private void richTextBoxNote_TextBoxChanged(object sender, EventArgs e)
        {
            if (last_he != null && noteenabled)
            {
                //System.Diagnostics.Debug.WriteLine("SI:type note changed: " + richTextBoxNote.Text);
                last_he.SetJournalSystemNoteText(richTextBoxNote.Text.Trim(), false, false);        // no commit, no send to edsm..
                discoveryform.NoteChanged(this, last_he, false);
            }
        }

        private void OnNoteChanged(Object sender, HistoryEntry he, bool arg)  // BEWARE we do this as well..
        {
            if ( !Object.ReferenceEquals(this,sender) )     // so, make sure this sys info is not sending it
            {
                //System.Diagnostics.Debug.WriteLine("SI:On note changed: " + he.snc.Note);
                SetNote(he.snc != null ? he.snc.Note : "");
            }
        }

        public void FocusOnNote( int asciikeycode )     // called if a focus is wanted
        {
            if (IsNotesShowing)
            {
                //System.Diagnostics.Debug.WriteLine("SI:Focus on Note due to key");

                richTextBoxNote.Select(richTextBoxNote.Text.Length, 0);     // move caret to end and focus.
                richTextBoxNote.ScrollToCaret();
                richTextBoxNote.Focus();

                string s = null;
                if (asciikeycode == 8)      // strange old sendkeys
                    s = "{BACKSPACE}";
                else if (asciikeycode == '+' || asciikeycode == '^' || asciikeycode == '%' || asciikeycode == '(' || asciikeycode == ')' || asciikeycode == '~')
                    s = "{" + (new string((char)asciikeycode, 1)) + "}";
                else if ( asciikeycode >= 32 && asciikeycode <= 126 )
                    s = new string((char)asciikeycode, 1);

                //System.Diagnostics.Debug.WriteLine("Send " + s);
                if (s != null)
                    SendKeys.Send(s);
            }
        }

        #endregion

        #region Move around

        int fromorder = -1;
        int fromy = -1;
        bool inmovedrag = false;

        private void controlMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Control.ModifierKeys.HasFlag(Keys.Control))
            {
                Control c = sender as Control;
                fromorder = (int)c.Tag;
                fromy = c.Top;
                inmovedrag = false;
            }
           // System.Diagnostics.Debug.WriteLine("Control " + inmove.Name + " grabbed");
        }

        private void controlMouseUp(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;

            if (fromorder != -1 )
            {
                if (inmovedrag)
                {
                    int xpos = this.PointToClient(Cursor.Position).X;

                    int fromrow = fromorder / HorzPositions;
                    int fromcol = fromorder % HorzPositions;

                    int col2pos = textBoxCredits.Right + 4 - labelCredits.Left;

                    int movetoy = fromy + e.Y;
                    int torow = BaseUtils.LineStore.FindRow(Lines,movetoy);       // may be -1 if can't find

                    if (torow == -1)    // if at end
                    {
                        torow = Lines.Count;        // fresh row here
                        Lines.Add(new BaseUtils.LineStore() { Items = new int[HorzPositions] });
                    }

                    int tocol = Math.Min(xpos / col2pos, HorzPositions - 1);

                    System.Diagnostics.Debug.WriteLine("Move " + fromrow +":"+ fromcol+" -> " + torow +":" + tocol + " Y est " + movetoy);

                    bool oneleftisnotshort = tocol > 0 && Lines[torow].Items[tocol - 1] > 0 && 
                                            Lines[torow].Items[tocol - 1] != Lines[fromrow].Items[fromcol] &&
                                            Array.IndexOf(SmallItems, Lines[torow].Items[tocol - 1] - 1) == -1;

                    bool onerightisnotblank = tocol < HorzPositions - 1 &&    // not last
                                            Array.IndexOf(SmallItems, Lines[fromrow].Items[fromcol]) == -1 && // one moving in is two columns
                                            Lines[torow].Items[tocol + 1] > 0 &&        // one to the right is occupied
                                            Lines[torow].Items[tocol + 1] != Lines[fromrow].Items[fromcol];      // and is not us..

                    if (Lines[torow].Items[tocol] > 0 || oneleftisnotshort || onerightisnotblank )      // occupied
                    {
                        Lines.Insert(torow, new BaseUtils.LineStore() { Items = new int[HorzPositions] });    // fresh row in here

                        if (fromrow > torow)            // adjust from down if torow is in front of it
                            fromrow++;
                    }

                    Lines[torow].Items[tocol] = Lines[fromrow].Items[fromcol];
                    Lines[fromrow].Items[fromcol] = -1;

                    BaseUtils.LineStore.CompressOrder(Lines);
                   // BaseUtils.LineStore.DumpOrder(Lines, "Move");

                    UpdateViewOnSelection();
                    Cursor.Current = Cursors.Default;
                }

                fromorder = -1;
            }
        }

        private void controlMouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;

            if (fromorder != -1 )
            {
                if (e.Y < -4 || e.Y > c.Height + 4 || e.X < -100 || e.X > 100)
                {
                    if (!inmovedrag)
                    {
                        inmovedrag = true;
                        Cursor.Current = Cursors.Hand;
                    }
                }
                else if ( inmovedrag )
                {
                    inmovedrag = false;
                    Cursor.Current = Cursors.Default;
                }

                //System.Diagnostics.Debug.WriteLine("Control " + inmove.Name + " Drag " + e.X + "," + e.Y);
            }
        }

        #endregion

        #region Display control

        private void Discoveryform_OnThemeChanged()
        {
            UpdateViewOnSelection();
        }

        private void UserControlSysInfo_Resize(object sender, EventArgs e)
        {
            //  later we may resize to width if other column is not used, but not now
        }

        public override bool SupportTransparency { get { return true; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            BackColor = curcol;
            extPanelScroll.BackColor = curcol;
            bool skinny = (Selection & (1 << BitSelSkinny)) != 0;

            if ( !skinny )
                EDDTheme.Instance.ApplyStd(extPanelScroll);

            foreach (Control c in extPanelScroll.Controls)
            {
                if (c is ExtendedControls.ExtTextBox)
                {
                    var tb = c as ExtendedControls.ExtTextBox;
                    tb.BackColor = curcol;
                    tb.ControlBackground = curcol;
                    if (skinny)
                    {
                        tb.BorderStyle = BorderStyle.None;
                        tb.BorderColor = Color.Transparent;
                    }
                }
                else if (c is ExtendedControls.ExtRichTextBox)
                {
                    var tb = c as ExtendedControls.ExtRichTextBox;
                    tb.TextBoxBackColor = curcol;
                    if (skinny)
                    {
                        tb.BorderStyle = BorderStyle.None;
                        tb.BorderColor = Color.Transparent;
                    }
                }
                else
                    c.BackColor = curcol;
            }
        }

        #endregion

    }
}
