/*
 * Copyright © 2016 - 2022 EDDiscovery development team
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
        private string dbSelection = "Sel";
        private string dbOSave = "Order";

        const int BitSelSystem = 0;
        const int BitSelEDSM = 1;
        const int BitSelVisits = 2;
        const int BitSelBody = 3;
        const int BitSelPosition = 4;
        const int BitSelDistanceFrom = 5;
        const int BitSelSystemState = 6;
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
        const int BitSelStationFaction = 21;
        const int BitSelMR = 22;
        const int BitSelSecurity = 23;
        const int BitSelNextDestination = 24;
        const int BitSelTotal = 25;

        const int BitSelEDSMButtonsNextLine = 28;       // other options
        const int BitSelSkinny = 29;

        const int BitSelDefault = ((1 << BitSelTotal) - 1) + (1 << BitSelEDSMButtonsNextLine);

        private int[] ItemSize = new int[]      // size of items in HorzPosititons
        {
            2,2,1,2,        // 0    - small: Visits
            2,2,2,2,        // 4
            2,2,1,1,        // 8    - small: Fuel,Cargo
            1,1,1,2,        // 12   - small: Mats, Data, Credits
            2,2,1,2,        // 16   - small: jumprange
            2,2,2,1,        // 20   - small: security
            2,0,0,0,        // 24   - large: Next target
        };

        private int[,] resetorder = new int[,]          // default reset order
        {
            {BitSelSystem,-1},
            {BitSelPosition,-1},
            {BitSelNextDestination,-1},
            {BitSelEDSM,-1},
            {BitSelVisits,BitSelCredits},
            {BitSelBody,-1},
            {BitSelStationFaction,-1},
            {BitSelStationButtons,-1},
            {BitSelShipInfo,-1},
            {BitSelShipyardButtons,-1},
            {BitSelDistanceFrom,-1},
            {BitSelSystemState,-1},
            {BitSelSecurity,-1},
            {BitSelTarget,-1},
            {BitSelFuel,BitSelCargo},
            {BitSelMats,BitSelData},
            {BitSelMR, -1 },
            {BitSelGameMode,-1},
            {BitSelTravel,-1},
            {BitSelMissions,-1},
            {BitSelJumpRange,-1},
        };

        public const int HorzPositions = 8;
        const int hspacing = 2;

        private string shiptexttranslation;         // text of ship: entry

        private ToolStripMenuItem[] toolstriplist;          // ref to toolstrip items for each bit above. in same order as bits BitSel..

        private int Selection;          // selection bits
        private List<BaseUtils.LineStore> Lines;            // stores settings on each line, values are BitN+1, 0 means position not used.

        private EliteDangerousCore.JournalEvents.JournalFSDTarget lasttarget = null;        // set when UI FSDTarget passes thru and not in jump sequence on top of history
        private EliteDangerousCore.JournalEvents.JournalFSDTarget pendingtarget = null;     // if we are, its stored here, and transfered back to lasttarget on the next FSD
        private EliteDangerousCore.UIEvents.UIDestination lastdestination = null;           // When UI Destination comes thru

        private bool travelhistoryisattop = true;

        bool neverdisplayed = true;
        HistoryEntry last_he = null;

        private ControlHelpersStaticFunc.ControlDragger drag = new ControlHelpersStaticFunc.ControlDragger();

        #region Init

        public UserControlSysInfo()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "SystemInformationPanel";

            // seem to need to do it here, first, before anything else gets into it

            var enumlist = new Enum[] { EDTx.UserControlSysInfo_labelStationFaction, EDTx.UserControlSysInfo_extButtonEDSMTarget, EDTx.UserControlSysInfo_labelSecurity, 
                                        EDTx.UserControlSysInfo_labelJumpRange, EDTx.UserControlSysInfo_labelTarget, EDTx.UserControlSysInfo_labelSysName, 
                                        EDTx.UserControlSysInfo_labelGamemode, EDTx.UserControlSysInfo_labelTravel, EDTx.UserControlSysInfo_labelOpenShip, 
                                        EDTx.UserControlSysInfo_labelOpenStation, EDTx.UserControlSysInfo_labelOpen, EDTx.UserControlSysInfo_labelCargo, 
                                        EDTx.UserControlSysInfo_labelCredits, EDTx.UserControlSysInfo_labelShip, EDTx.UserControlSysInfo_labelMaterials, 
                                        EDTx.UserControlSysInfo_labelVisits, EDTx.UserControlSysInfo_labelMR, EDTx.UserControlSysInfo_labelData, 
                                        EDTx.UserControlSysInfo_labelFuel, EDTx.UserControlSysInfo_labelBodyName, EDTx.UserControlSysInfo_labelPosition, 
                                        EDTx.UserControlSysInfo_labelMissions, EDTx.UserControlSysInfo_labelEconomy, 
                                        EDTx.UserControlSysInfo_labelGov, EDTx.UserControlSysInfo_labelAllegiance, EDTx.UserControlSysInfo_labelState, EDTx.UserControlSysInfo_labelSolDist, 
                                        EDTx.UserControlSysInfo_labelHomeDist, EDTx.UserControlSysInfo_labelNextDestination };
            var enumlistcms = new Enum[] { EDTx.UserControlSysInfo_toolStripSystem, EDTx.UserControlSysInfo_toolStripEDSM, EDTx.UserControlSysInfo_toolStripEDSMDownLine, 
                                        EDTx.UserControlSysInfo_toolStripVisits, EDTx.UserControlSysInfo_toolStripBody, EDTx.UserControlSysInfo_displayStationButtonsToolStripMenuItem, 
                                        EDTx.UserControlSysInfo_displayStationFactionToolStripMenuItem, EDTx.UserControlSysInfo_toolStripPosition, EDTx.UserControlSysInfo_toolStripDistanceFrom, 
                                        EDTx.UserControlSysInfo_toolStripSystemState, EDTx.UserControlSysInfo_displaySecurityToolStripMenuItem,  
                                        EDTx.UserControlSysInfo_toolStripTarget, EDTx.UserControlSysInfo_toolStripShip, EDTx.UserControlSysInfo_displayShipButtonsToolStripMenuItem, 
                                        EDTx.UserControlSysInfo_toolStripFuel, EDTx.UserControlSysInfo_toolStripCargo, EDTx.UserControlSysInfo_toolStripDataCount, 
                                        EDTx.UserControlSysInfo_toolStripMaterialCounts, EDTx.UserControlSysInfo_displayMicroresourcesCountToolStripMenuItem, 
                                        EDTx.UserControlSysInfo_toolStripCredits, EDTx.UserControlSysInfo_toolStripGameMode, EDTx.UserControlSysInfo_toolStripTravel, 
                                        EDTx.UserControlSysInfo_toolStripMissionList, EDTx.UserControlSysInfo_toolStripJumpRange, EDTx.UserControlSysInfo_toolStripSkinny, 
                                        EDTx.UserControlSysInfo_toolStripReset, EDTx.UserControlSysInfo_toolStripRemoveAll , EDTx.UserControlSysInfo_displayNextDestinationToolStripMenuItem};
            var enumlisttt = new Enum[] { EDTx.UserControlSysInfo_ToolTip, EDTx.UserControlSysInfo_textBoxTargetDist_ToolTip, EDTx.UserControlSysInfo_textBoxTarget_ToolTip };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            textBoxTarget.SetAutoCompletor(SystemCache.ReturnSystemAdditionalListForAutoComplete);
            textBoxTarget.AutoCompleteTimeout = 500;

            // same order as Sel bits are defined in, one bit per selection item.
            toolstriplist = new ToolStripMenuItem[]
            {   toolStripSystem , toolStripEDSM , toolStripVisits, toolStripBody,
                toolStripPosition, toolStripDistanceFrom,
                toolStripSystemState, null, toolStripTarget,        // removed notes
                toolStripShip, toolStripFuel , toolStripCargo, toolStripMaterialCounts,  toolStripDataCount,
                toolStripCredits,
                toolStripGameMode,toolStripTravel, toolStripMissionList,
                toolStripJumpRange, displayStationButtonsToolStripMenuItem,
                displayShipButtonsToolStripMenuItem,
                displayStationFactionToolStripMenuItem,         // BitSelStationFaction 21
                displayMicroresourcesCountToolStripMenuItem,    // BitSelMR 22
                displaySecurityToolStripMenuItem,               // 23
                displayNextDestinationToolStripMenuItem,  // 24
            };

            Debug.Assert(toolstriplist.Length == BitSelTotal);

            Selection = GetSetting(dbSelection, BitSelDefault);

            string rs = GetSetting(dbOSave, "-");
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
                    else if (bit == BitSelStationFaction)
                    {
                        var p = BaseUtils.LineStore.FindValue(Lines, BitSelBody + 1);
                        if (p != null)
                            insertat = Lines.IndexOf(p) + 1;

                        Selection |= (1 << bit);
                    }
                    else if (bit == BitSelMR)
                    {
                        var p = BaseUtils.LineStore.FindValue(Lines, BitSelData + 1);
                        if (p != null)
                            insertat = Lines.IndexOf(p) + 1;

                        Selection |= (1 << bit);
                    }
                    else if (bit == BitSelSecurity)
                    {
                        var p = BaseUtils.LineStore.FindValue(Lines, BitSelSystemState + 1);
                        if (p != null)
                            insertat = Lines.IndexOf(p) + 1;

                        Selection |= (1 << bit);
                    }
                    else if (bit == BitSelNextDestination)
                    {
                        var p = BaseUtils.LineStore.FindValue(Lines, BitSelPosition + 1);
                        if (p != null)
                            insertat = Lines.IndexOf(p) + 1;

                        Selection |= (1 << bit);
                    }

                    Lines.Insert(insertat, new BaseUtils.LineStore() { Items = new int[HorzPositions] { bit + 1, 0, 0, 0, 0, 0, 0, 0 } });
                }
            }

            discoveryform.OnNewTarget += RefreshTargetDisplay;
            discoveryform.OnEDSMSyncComplete += Discoveryform_OnEDSMSyncComplete;
            discoveryform.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            discoveryform.OnThemeChanged += Discoveryform_OnThemeChanged;

            panelFD.BackgroundImage = EDDiscovery.Icons.Controls.notfirstdiscover;      // just to hide it during boot up

            shiptexttranslation = labelShip.Text;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += TravelSelChanged;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= TravelSelChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += TravelSelChanged;
            //System.Diagnostics.Debug.WriteLine("UCTG changed in sysinfo to " + uctg.GetHashCode());
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= TravelSelChanged;
            discoveryform.OnNewTarget -= RefreshTargetDisplay;
            discoveryform.OnEDSMSyncComplete -= Discoveryform_OnEDSMSyncComplete;
            discoveryform.OnNewUIEvent -= Discoveryform_OnNewUIEvent;

            PutSetting(dbOSave, BaseUtils.LineStore.ToString(Lines));
            PutSetting(dbSelection, Selection);
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry);
        }

        private void Discoveryform_OnEDSMSyncComplete(int count, string syslist)     // EDSM ~MAY~ have updated the last discovery flag, so redisplay
        {
            //System.Diagnostics.Debug.WriteLine("EDSM SYNC COMPLETED with " + count + " '" + syslist + "'");
            Display(last_he);
        }

        private void Discoveryform_OnNewUIEvent(UIEvent obj)
        {
            if (obj is EliteDangerousCore.UIEvents.UIFuel && travelhistoryisattop) // fuel UI update the SI information globally.  if tracking at the top..
            {
                var tophe = discoveryform.history.GetLast;      // we feed in the top, which is being updated by EDDiscoveryControllerNewEntry with the latest fuel
                if (tophe != null)   // paranoia
                {
                    System.Diagnostics.Debug.WriteLine($"UI Top he Fuel {last_he.EventTimeUTC} {last_he.ShipInformation.FuelLevel} {last_he.ShipInformation.ReserveFuelCapacity}");
                    Display(tophe);
                }
            }

            if (obj is EliteDangerousCore.UIEvents.UIFSDTarget)
            {
                var j = ((EliteDangerousCore.UIEvents.UIFSDTarget)obj).FSDTarget;
                if (lasttarget == null || j.StarSystem != lasttarget.StarSystem)       // a little bit of debouncing, see if the target info has changed
                {
                    if ( (discoveryform.history.GetLast?.FSDJumpSequence??false)  == true) 
                    {
                        System.Diagnostics.Debug.WriteLine($"Sysinfo - FSD target got, but in fsd sequence, pend it");
                        pendingtarget = j;
                    }
                    else
                    {
                        lasttarget = j;
                        System.Diagnostics.Debug.WriteLine($"Sysinfo - FSD target got");
                        Display(last_he);
                    }
                }
            }
            if (obj is EliteDangerousCore.UIEvents.UIDestination)
            {
                var j = (EliteDangerousCore.UIEvents.UIDestination)obj;
                if (lastdestination == null || j.Name != lastdestination.Name || j.BodyID != lastdestination.BodyID)        // if name or bodyid has changed
                {
                    System.Diagnostics.Debug.WriteLine($"Sysinfo - Destination got");

                    lastdestination = j;
                    Display(last_he);
                }
            }
        }

        private void TravelSelChanged(HistoryEntry he, HistoryList hl, bool sel)
        {
            travelhistoryisattop = he == hl.GetLast;      // see if tracking at top

            bool duetosystem = last_he == null;
            bool duetostatus = false;
            bool duetocomms = false;
            bool duetoship = false;
            bool duetoother = false;
            bool duetomissions = false;

            if (he.FSDJumpSequence == false && pendingtarget != null )      // if we reached the end of the fsd sequence, but we have a pend, free
            {
                System.Diagnostics.Debug.WriteLine($"Sysinfo - FSDJump and pending target set, so end of jump sequence");
                lasttarget = pendingtarget;
                pendingtarget = null;
                duetoother = true;          // force update
            }

            if (last_he!=null)       // last_he must be non null for these tests
            {
                duetosystem |= last_he.System.Name != he.System.Name;       // add on sys name changes to this
                duetostatus = last_he.Status != he.Status;
                duetocomms = last_he.MaterialCommodity != he.MaterialCommodity;
                duetoship = last_he.ShipInformation != he.ShipInformation;
                duetoother = last_he.Credits != he.Credits || last_he.Suits != he.Suits || last_he.Loadouts != he.Loadouts;
                duetomissions = last_he.MissionList != he.MissionList;
            }

            if (duetosystem || duetostatus || duetocomms || duetoship || duetoother || duetomissions)
            {
                System.Diagnostics.Debug.WriteLine($"SysInfo - {he.journalEntry.EventTypeStr} got: sys {duetosystem} st {duetostatus} comds {duetocomms} ship {duetoship} missions {duetomissions} other {duetoother}");
                Display(he);
            }
        }

        private async void Display(HistoryEntry he) 
        {
            if (neverdisplayed)
            {
                UpdateViewOnSelection();  // then turn the right ones on
                neverdisplayed = false;
            }

            last_he = he;
            var hl = discoveryform.history;

            if (last_he != null)
            {
                SetControlText(he.System.Name);

                HistoryEntry lastfsd = hl.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.FSDJump, he);

                textBoxSystem.Text = he.System.Name;
                panelFD.BackgroundImage = (lastfsd != null && (lastfsd.journalEntry as EliteDangerousCore.JournalEvents.JournalFSDJump).EDSMFirstDiscover) ? EDDiscovery.Icons.Controls.firstdiscover : EDDiscovery.Icons.Controls.notfirstdiscover;

                textBoxBody.Text = he.WhereAmI + " (" + he.Status.BodyType + ")";

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

                int count = hl.GetVisitsCount(he.System.Name);
                textBoxVisits.Text = count.ToString();

                //                System.Diagnostics.Debug.WriteLine("UserControlSysInfo sys info {0} {1} {2}", he.System.Name, he.System.EDSMID, he.System.EDDBID);


                extButtonEDSMSystem.Enabled = extButtonEDDBSystem.Enabled = extButtonInaraSystem.Enabled = extButtonSpanshSystem.Enabled = true;

                string allegiance, economy, gov, faction, factionstate, security;
                hl.ReturnSystemInfo(he, out allegiance, out economy, out gov, out faction, out factionstate, out security);

                textBoxAllegiance.Text = allegiance;
                textBoxEconomy.Text = economy;
                textBoxGovernment.Text = gov;
                textBoxState.Text = factionstate;
                extTextBoxSecurity.Text = security;

                extTextBoxStationFaction.Text = he.StationFaction ?? "";

                List<MissionState> mcurrent = (from MissionState ms in hl.MissionListAccumulator.GetMissionList(he.MissionList) where ms.InProgressDateTime(last_he.EventTimeUTC) orderby ms.Mission.EventTimeUTC descending select ms).ToList();

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
                            + " @ " + ms.DestinationSystemStationSettlement(),
                            Environment.NewLine);
                    }

                    richTextBoxScrollMissions.Text = t;
                }

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

                var mcl = hl.MaterialCommoditiesMicroResources.Get(he.MaterialCommodity);
                var counts = MaterialCommoditiesMicroResourceList.Count(mcl,0);
                int cargocount = counts[(int)MaterialCommodityMicroResourceType.CatType.Commodity];

                int cc = he.ShipInformation?.CargoCapacity() ?? 0;// so if we don't have a ShipInformation, use 0
                if (cc > 0)
                    textBoxCargo.Text = cargocount.ToString() + "/" + cc.ToString();
                else
                    textBoxCargo.Text = cargocount.ToString();

                textBoxMaterials.Text = (counts[(int)MaterialCommodityMicroResourceType.CatType.Raw]+ counts[(int)MaterialCommodityMicroResourceType.CatType.Manufactured]).ToString();
                textBoxData.Text = counts[(int)MaterialCommodityMicroResourceType.CatType.Encoded].ToString();
                textBoxCredits.Text = he.Credits.ToString("N0");
                extTextBoxMR.Text = "C:" + counts[(int)MaterialCommodityMicroResourceType.CatType.Consumable].ToString() +
                                    ", A:" + counts[(int)MaterialCommodityMicroResourceType.CatType.Component].ToString() +
                                    ", G:" + counts[(int)MaterialCommodityMicroResourceType.CatType.Item].ToString() +
                                    ", D:" + counts[(int)MaterialCommodityMicroResourceType.CatType.Data].ToString();


                //= lasttarget != null ? $"{lasttarget.StarSystem} {lasttarget.StarClass}" : "";


                EliteDangerousCalculations.FSDSpec fsd = !he.Status.OnFoot && he.ShipInformation != null ? he.ShipInformation.GetFSDSpec() : null;
                EliteDangerousCalculations.FSDSpec.JumpInfo ji = fsd != null ? fsd.GetJumpInfo(cargocount, he.ShipInformation.HullModuleMass(),
                                he.ShipInformation.FuelLevel, he.ShipInformation.FuelCapacity / 2, he.Status.CurrentBoost) : null;

                textBoxJumpRange.Text = "";

                if (he.Status.OnFoot)
                {
                    labelShip.Text = "On Foot".T(EDTx.UserControlSysInfo_OnFoot);   

                    var cursuit = hl.SuitList.CurrentID(he.Suits);                     // get current suit ID, or 0 if none
                    if (cursuit != 0)
                    {
                        var suit = hl.SuitList.Suit(cursuit,he.Suits);                // get suit
                        textBoxShip.Text = suit?.FriendlyName ?? "???";
                        var curloadout = discoveryform.history.SuitLoadoutList.CurrentID(he.Loadouts);         // get current loadout ID, or 0 if none

                        if ( curloadout != 0 )
                        {
                            var loadout = hl.SuitLoadoutList.Loadout(curloadout, he.Loadouts);
                            textBoxShip.Text += ":" + (loadout?.Name ?? "???");
                        }
                    }
                    else
                        textBoxShip.Text = "";

                    textBoxFuel.Text = "";
                    extButtonCoriolis.Enabled = extButtonEDSY.Enabled = false;
                }
                else
                {
                    labelShip.Text = shiptexttranslation;       // back to ship

                    if (he.ShipInformation != null)
                    {
                        ShipInformation si = he.ShipInformation;

                        textBoxShip.Text = si.ShipFullInfo(cargo: false, fuel: false);

                        if (si.FuelCapacity > 0 && si.FuelLevel > 0)            // Fuel info 
                            textBoxFuel.Text = si.FuelLevel.ToString("0.#") + "/" + si.FuelCapacity.ToString("0.#");
                        else if (si.FuelCapacity > 0)
                            textBoxFuel.Text = si.FuelCapacity.ToString("0.#");
                        else
                            textBoxFuel.Text = "N/A".T(EDTx.UserControlSysInfo_NA);

                        if ( ji != null)
                            textBoxJumpRange.Text = ji.cursinglejump.ToString("N2") + "ly";

                        extButtonCoriolis.Enabled = extButtonEDSY.Enabled = true;
                    }
                    else
                    {
                        textBoxShip.Text = textBoxFuel.Text = "";
                        extButtonCoriolis.Enabled = extButtonEDSY.Enabled = false;
                    }
                }

                // if we have a destination, or we have a last target but its not on our own star system (because we don't track FSD Jump to clearlast target)
                // sequence of lastdestination vs lasttarget is indeterminate

                if (travelhistoryisattop && ( lastdestination != null || (lasttarget != null && lasttarget.StarSystem != he.System.Name)))
                {
                    string starname = "";
                    string bodyname = "";
                    string starclass = "";
                    string distance = "";
                    string pos = "";
                    Color textdistcolor = ExtendedControls.Theme.Current.TextBlockColor;

                    //bool sysinfoincurrentsystem = he.System.Name == (discoveryform.history.LastOrDefault?.System.Name ?? "xx");

                    // decide on time which one to present..  if lastdestination is newer..
                    if (lastdestination != null && (lasttarget == null || lastdestination.EventTimeUTC >= lasttarget.EventTimeUTC))
                    {
                        if (lastdestination.BodyID == 0)    // if its a star..
                        {
                            starname = lastdestination.Name;
                            starclass = lasttarget != null && lasttarget.StarSystem == lastdestination.Name ? ": " + lasttarget.StarClass : "";
                          //  System.Diagnostics.Debug.WriteLine($"Destination select star {lastdestination.Name} {lastdestination.BodyID}");
                        }
                        else
                        {
                            bodyname = lastdestination.Name;    // else body
                          //  System.Diagnostics.Debug.WriteLine($"Destination select body {lastdestination.BodyID}");

                            var ss = await discoveryform.history.StarScan.FindSystemAsync(discoveryform.history.GetLast.System, false);
                            if (IsClosed)       //ASYNC! warning! may have closed.
                                return;

                            // with a found system, see if we can get the body name
                            if (ss != null && ss.NodesByID.TryGetValue(lastdestination.BodyID, out StarScan.ScanNode body))
                            {
                                pos = body.FullName.ReplaceIfStartsWith(he.System.Name).Trim();

                                if (body.ScanData != null)
                                    distance = body.ScanData.DistanceFromArrivalLS.ToString("N0") + "ls";
                            }
                        }
                    }
                    else if (lasttarget != null)  // paranoia
                    {
                        starname = lasttarget.StarSystem;
                        starclass = ": " + lasttarget.StarClass;
                       // System.Diagnostics.Debug.WriteLine($"Target select star");
                    }

                    if (starname != null)
                    {
                        ISystem sys = await SystemCache.FindSystemAsync(starname, null);         // no EDSM, no glist, find star pos

                        if (IsClosed)       //ASYNC! warning! may have closed.
                            return;

                        if (sys != null && sys.HasCoordinate)       // Bingo!
                        {
                            double dist = sys.Distance(discoveryform.history.GetLast.System);       // we must have a last to be here
                            distance = $"{dist:N2}ly";

                            if (ji != null) // and therefore fsd is non null
                            {
                                double fuel = fsd.FuelUse(cargocount, he.ShipInformation.HullModuleMass(), he.ShipInformation.FuelLevel, dist, he.Status.CurrentBoost);
                                distance += $" {fuel:N2}t";

                                if (ji.cursinglejump < dist)
                                    textdistcolor = ExtendedControls.Theme.Current.TextBlockHighlightColor;
                            }

                            pos = $"{sys.X:N1}, {sys.Y:N1}, {sys.Z:N1}";
                        }
                    }

                    extTextBoxNextDestination.Text = $"{starname}{bodyname}{starclass}";
                    extTextBoxNextDestinationDistance.Text = distance;
                    extTextBoxNextDestinationPosition.Text = pos;
                    extTextBoxNextDestinationDistance.ForeColor = textdistcolor;
                }
                else
                    extTextBoxNextDestinationDistance.Text = extTextBoxNextDestinationPosition.Text = extTextBoxNextDestination.Text = "";


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
                                extTextBoxStationFaction.Text = extTextBoxNextDestination.Text = 
                                "";

                extButtonEDSMSystem.Enabled = extButtonEDDBSystem.Enabled = extButtonInaraSystem.Enabled = extButtonSpanshSystem.Enabled = false;
                extButtonEDDBStation.Enabled = extButtonInaraStation.Enabled = extButtonSpanshStation.Enabled = false;
                extButtonCoriolis.Enabled = extButtonEDSY.Enabled = false;
            }
        }

        #endregion

        #region Clicks

        private void buttonEDDBSystem_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                BaseUtils.BrowserInfo.LaunchBrowser(Properties.Resources.URLEDDBSystemName + HttpUtility.UrlEncode(last_he.System.Name));
            }
        }

        private void buttonEDSMSystem_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                EDSMClass edsm = new EDSMClass();

                if (!edsm.ShowSystemInEDSM(last_he.System.Name))        
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "System unknown to EDSM".T(EDTx.UserControlSysInfo_SysUnk));
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
                BaseUtils.BrowserInfo.LaunchBrowser(Properties.Resources.URLSpanshSystemSystemId + last_he.System.SystemAddress.Value.ToStringInvariant());
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
                BaseUtils.BrowserInfo.LaunchBrowser(Properties.Resources.URLEDDBStationMarketId + last_he.MarketID.ToStringInvariant());
        }

        private void extButtonSpanshStation_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                if (last_he.MarketID != null)
                    BaseUtils.BrowserInfo.LaunchBrowser(Properties.Resources.URLSpanshStationMarketId + last_he.MarketID.ToStringInvariant());
                else if (last_he.FullBodyID.HasValue)
                    BaseUtils.BrowserInfo.LaunchBrowser(Properties.Resources.URLSpanshBodyId + last_he.FullBodyID.ToStringInvariant());
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
                if (textBoxTarget.Text.HasChars())          // if chars, means to set it
                {
                    ISystem sc = SystemCache.FindSystem(textBoxTarget.Text);        // find system
                    if ( sc != null)
                    {
                        TargetClass.SetTargetOnSystem(sc.Name, sc.X, sc.Y, sc.Z);
                        discoveryform.NewTargetSet(this);
                    }
                    else
                    {
                        GalacticMapObject gmo = discoveryform.galacticMapping.Find(textBoxTarget.Text, true);       // find gmo, any part
                        if (gmo != null)
                        {
                            TargetClass.SetTargetOnGMO(gmo.Name,gmo.ID, gmo.Points[0].X, gmo.Points[0].Y, gmo.Points[0].Z);
                            textBoxTarget.Text = gmo.Name;
                            discoveryform.NewTargetSet(this);
                        }
                        else
                            Console.Beep(256, 200); // beep boop!
                    }
                }
                else
                {
                    TargetClass.ClearTarget();          // empty means clear
                    discoveryform.NewTargetSet(this);
                }
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
                textBoxTarget.Select(0, 0);
                textBoxTargetDist.Text = "No Pos".T(EDTx.NoPos);

                HistoryEntry cs = discoveryform.history.GetLastWithPosition();
                if (cs != null)
                    textBoxTargetDist.Text = cs.System.Distance(x, y, z).ToString("0.0");

              //  textBoxTarget.SetTipDynamically(toolTip, string.Format("Position is {0:0.00},{1:0.00},{2:0.00}".T(EDTx.UserControlSysInfo_Pos), x, y, z));
            }
            else
            {
                textBoxTarget.Text = "?";
                textBoxTargetDist.Text = "";
          //      textBoxTarget.SetTipDynamically(toolTip, "On 3D Map right click to make a bookmark, region mark or click on a notemark and then tick on Set Target, or type it here and hit enter".T(EDTx.UserControlSysInfo_Target));
            }
        }

        private void buttonEDSMTarget_Click(object sender, EventArgs e)
        {
            TargetClass.GetTargetPosition(out string name, out double x, out double y, out double z);
            if (name.HasChars())
            {
                EDSMClass edsm = new EDSMClass();
                if (!edsm.ShowSystemInEDSM(name))         // may pass back empty string if not known, this solves another exception
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "System unknown to EDSM".T(EDTx.UserControlSysInfo_SysUnk));
            }
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

        private void displayStationFactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelStationFaction);
        }

        private void displayMicroresourcesCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelMR);
        }
        private void displaySecurityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelSecurity);
        }

        private void displayNextDestinationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelNextDestination);
        }

        private void toolStripRemoveAll_Click(object sender, EventArgs e)
        {
            Selection = (Selection | ((1 << BitSelTotal) - 1)) ^ ((1 << BitSelTotal) - 1);
            UpdateViewOnSelection();
        }

        private void whenTransparentUseSkinnyLookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelSkinny);
            UpdateTransparency();
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

                    if (bitno >= 0 && bitno < toolstriplist.Length && toolstriplist[bitno] != null )     // ensure within range, ignore any out of range, in case going backwards in versions
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
                                    panelFD.Tag = si;
                                    panelFD.Visible = true;

                                    if (!selEDSMonNextLine && (Selection & (1 << BitSelEDSM)) != 0)
                                    {
                                        extButtonEDSMSystem.Location = new Point(panelFD.Right + hspacing, datapos.Y);
                                        extButtonEDDBSystem.Location = new Point(extButtonEDSMSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonInaraSystem.Location = new Point(extButtonEDDBSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonSpanshSystem.Location = new Point(extButtonInaraSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonEDSMSystem.Visible = extButtonEDDBSystem.Visible = extButtonInaraSystem.Visible = extButtonSpanshSystem.Visible = true;
                                        extButtonEDSMSystem.Tag = extButtonEDDBSystem.Tag = extButtonInaraSystem.Tag = extButtonSpanshSystem.Tag = si;
                                        itembottom = Math.Max(extButtonEDSMSystem.Bottom, itembottom);
                                    }

                                    break;

                                case BitSelNextDestination:
                                    itembottom = this.SetPos(labpos, labelNextDestination, datapos, extTextBoxNextDestination, si);
                                    itembottom++;
                                    extTextBoxNextDestinationDistance.Location = new Point(extTextBoxNextDestination.Left, itembottom);
                                    extTextBoxNextDestinationPosition.Bounds = new Rectangle(extTextBoxNextDestinationDistance.Right + hspacing, extTextBoxNextDestinationDistance.Top,
                                                                                             extTextBoxNextDestination.Width - hspacing - extTextBoxNextDestinationDistance.Width, extTextBoxNextDestinationPosition.Height);
                                    extTextBoxNextDestinationDistance.Visible = extTextBoxNextDestinationPosition.Visible = true;
                                    itembottom = Math.Max(extTextBoxNextDestinationDistance.Bottom, itembottom);
                                    break;


                                case BitSelEDSM:
                                    if (selEDSMonNextLine)
                                    {
                                        labelOpen.Location = labpos;
                                        extButtonEDSMSystem.Location = new Point(datapos.X, datapos.Y);
                                        extButtonEDDBSystem.Location = new Point(extButtonEDSMSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonInaraSystem.Location = new Point(extButtonEDDBSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonSpanshSystem.Location = new Point(extButtonInaraSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        labelOpen.Tag = extButtonEDSMSystem.Tag = extButtonEDDBSystem.Tag = extButtonInaraSystem.Tag = extButtonSpanshSystem.Tag = si;
                                        extButtonEDSMSystem.Visible = extButtonEDDBSystem.Visible = extButtonInaraSystem.Visible = extButtonSpanshSystem.Visible = true;
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

                                case BitSelStationFaction:
                                    itembottom = this.SetPos(labpos, labelStationFaction, datapos, extTextBoxStationFaction, si);
                                    break;

                                case BitSelMR:
                                    itembottom = this.SetPos(labpos, labelMR, datapos, extTextBoxMR, si);
                                    break;
                                case BitSelSecurity:
                                    itembottom = this.SetPos(labpos, labelSecurity, datapos, extTextBoxSecurity, si);
                                    break;

                                default:
                                    System.Diagnostics.Debug.WriteLine($"SysInfo Ignoring unknown type {bitno}");
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

        private void Reset()
        {
            Selection = BitSelDefault;
            Lines = new List<BaseUtils.LineStore>();
            for(int i = 0 ; i < resetorder.Length/2; i++ )
                Lines.Add(new BaseUtils.LineStore() { Items = new int[HorzPositions] { resetorder[i,0]+1, resetorder[i,1]+1, 0, 0, 0, 0, 0, 0 } });
            //BaseUtils.LineStore.DumpOrder(Lines, "Reset");
        }

        private int SetPos(Point lp, Label lab, Point tp, ExtendedControls.ExtTextBox box, int i )
        {
            lab.Location = lp;
            box.Location = tp;
            box.Tag = lab.Tag = i;
            lab.Visible = box.Visible = true;
            return Math.Max(lab.Bottom, box.Bottom);
        }

        private int SetPos(Point lp, Label lab, Point tp, ExtendedControls.ExtRichTextBox box, int i)
        {
            lab.Location = lp;
            box.Location = tp;
            box.Tag = lab.Tag = i;
            lab.Visible = box.Visible = true;
            return Math.Max(lab.Bottom, box.Bottom);
        }

        private int OffsetPos(Point lp, Label lab, Point tp, ExtendedControls.ExtTextBox box , int i)
        {
            lab.Location = lp;
            box.Location = tp;
            box.Tag = lab.Tag = i;
            lab.Visible = box.Visible = true;
            return Math.Max(lab.Bottom, box.Bottom);
        }

        #endregion


        #region Move around

        private void controlMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Control.ModifierKeys.HasFlag(Keys.Control))
            {
                Control c = sender as Control;
                int si = (int)c.Tag;
                var clist = c.Parent.Controls.OfType<Control>();

                // add controls tagged with SI to the dragger..  convert position to abs screen pos, we give the parent screen rectangle as a clip area
                drag.Start(clist.Where(x => x.Tag != null && (int)x.Tag == si), true, 
                                                        c.PointToScreen(e.Location), 
                                                        c.Parent.RectangleToScreen(new Rectangle(0, 0, c.Parent.Width, c.Parent.Height)));
                Cursor.Current = Cursors.Hand;
            }
        }

        private void controlMouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            // convert position to screen, as thats the reference pos, and if change pos, use the panel scroll change child location to stop flicker.
            drag.MouseMoved(c.PointToScreen(e.Location), (ch,p) => { extPanelScroll.ChangeChildLocation(ch,new Point(p.X, p.Y - extPanelScroll.ScrollOffset)); });
        }

        private void controlMouseUp(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;

            if ( drag.DragStarted)
            {
                if ( drag.InMoveDrag)
                {
                    // work out from position
                    int fromorder = (int)drag.DragControls[0].Tag;
                    int fromrow = fromorder / HorzPositions;
                    int fromcol = fromorder % HorzPositions;

                    // get position
                    int xpos = c.Location.X;
                    int movetoy = c.Location.Y - extPanelScroll.ScrollOffset;
                    //System.Diagnostics.Debug.WriteLine($"Control {c.Name} dropped at {c.Location} = {xpos} {movetoy}");

                    // work out to position
                    int torow = BaseUtils.LineStore.FindRow(Lines, movetoy);       // may be -1 if can't find
                    if (torow == -1)    // if at end
                    {
                        torow = Lines.Count;        // fresh row here
                        Lines.Add(new BaseUtils.LineStore() { Items = new int[HorzPositions] });
                    }

                    int colwidth = textBoxCredits.Right + 4 - labelCredits.Left;
                    int tocol = Math.Min(xpos / colwidth, HorzPositions - 1);

                    int itemno = Lines[fromrow].Items[fromcol];

                    //System.Diagnostics.Debug.WriteLine($"Move item {itemno} from {fromrow}:{fromcol} to {torow}:{tocol} Column width {colwidth}");

                    // if something is happening, we need to rework the lines array
                    if (fromrow != torow || fromcol != tocol)
                    {
                        //BaseUtils.LineStore.DumpOrder(Lines, "Move");

                        // lets fill in the line, with markers indicating in all slots if its occupied.  Use the item code to fill it in
                        int[] filledline = new int[HorzPositions];
                        {
                            int p = 0;
                            int rep = 0;
                            int repno = 0;
                            foreach (var x in Lines[torow].Items)
                            {
                                filledline[p++] = rep-- > 0 ? repno : x;
                                if (x > 0)
                                {
                                    rep = ItemSize[x - 1] - 1;
                                    repno = x;
                                }
                            }
                        }

                       // for (int i = 0; i < HorzPositions; i++)  System.Diagnostics.Debug.WriteLine($"{i} = {Lines[torow].Items[i]} {filledline[i]}");

                        bool isfilled = false;
                        for (int i = tocol; i < tocol + ItemSize[itemno - 1]; i++)       // all cols must be empty or set to the itemno to allow it to stay on this line
                        {
                            isfilled |= filledline[i] != itemno && filledline[i] != 0;
                        }

                        if (isfilled)      // one or more cells occupied  - push content down and make an empty line
                        {
                            Lines.Insert(torow, new BaseUtils.LineStore() { Items = new int[HorzPositions] });    // fresh row in here

                            if (fromrow >= torow)            // adjust from down if torow is in front of it
                                fromrow++;

                            //BaseUtils.LineStore.DumpOrder(Lines, "Inserted");
                        }

                        System.Diagnostics.Debug.WriteLine($"Move item {itemno} from {fromrow}:{fromcol} to {torow}:{tocol} and zero");

                        Lines[torow].Items[tocol] = Lines[fromrow].Items[fromcol];      // set slot
                        Lines[fromrow].Items[fromcol] = 0;      // clear from slot.

                        BaseUtils.LineStore.CompressOrder(Lines);
                        BaseUtils.LineStore.DumpOrder(Lines, "State");
                    }

                    UpdateViewOnSelection();

                }

                Cursor.Current = Cursors.Default;
                drag.End();
                extPanelScroll.EnsureScrollBarZ();
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
            ExtendedControls.Theme.Current.ApplyStd(extPanelScroll);     // reset themeing

            BackColor = curcol;
            extPanelScroll.BackColor = curcol;

            bool skinny = (Selection & (1 << BitSelSkinny)) != 0;

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
                else if ( c is ExtendedControls.ExtButton )
                {
                    var bt = c as ExtendedControls.ExtButton;
                    if (on)
                        bt.ButtonColorScaling = bt.ButtonDisabledScaling = 1.0f;
                    bt.BackColor = curcol;
                }
                else
                    c.BackColor = curcol;
            }
        }

        #endregion


    }
}
