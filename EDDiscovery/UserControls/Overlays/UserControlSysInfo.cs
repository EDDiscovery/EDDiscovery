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
using EliteDangerousCore.GMO;
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

        [Flags]
        enum ControlBits
        {
            BitSelSystem = 0,       // main items
            BitSelEDSM = 1,
            BitSelVisits = 2,
            BitSelBody = 3,
            BitSelPosition = 4,
            BitSelDistanceFrom = 5,
            BitSelSystemState = 6,
            BitSelTarget = 8,
            BitSelShipInfo = 9,
            BitSelFuel = 10,
            BitSelCargo = 11,
            BitSelMats = 12,
            BitSelData = 13,
            BitSelCredits = 14,
            BitSelGameMode = 15,
            BitSelTravel = 16,
            BitSelMissions = 17,
            BitSelJumpRange = 18,
            BitSelStationButtons = 19,
            BitSelShipyardButtons = 20,
            BitSelStationFaction = 21,
            BitSelMR = 22,
            BitSelSecurity = 23,
            BitSelNextDestination = 24,
            BitSelEDSMButtonsNextLine = 28,       // other options
            BitSelSkinny = 29,

            BitSelDisableCopyToClipboardWhenClickingOnTextBoxes = 40,
            BitSelNotValid = -1,
        };

        const int BitSelTotal = 25; // total number of main items
        const long BitSelDefault = ((1L << BitSelTotal) - 1) + (1L << (int)ControlBits.BitSelEDSMButtonsNextLine);

        private int[] ItemSize = new int[]      // size of items in HorzPosititons (1/2/3/4 etc)
        {
            2,2,1,2,        // 0    - small: Visits
            2,2,2,0,        // 4    - pos, distancefrom systemstate, Note(removed)
            2,2,1,1,        // 8    - small: Fuel,Cargo
            1,1,1,2,        // 12   - small: Mats, Data, Credits
            2,2,1,2,        // 16   - small: jumprange
            2,2,2,1,        // 20   - small: security
            2,0,0,0,        // 24   - large: Next target
        };

        private ControlBits[,] resetorder = new ControlBits[,]          // default reset order
        {
            {ControlBits.BitSelSystem,ControlBits.BitSelNotValid},
            {ControlBits.BitSelPosition,ControlBits.BitSelNotValid},
            {ControlBits.BitSelNextDestination,ControlBits.BitSelNotValid},
            {ControlBits.BitSelJumpRange,ControlBits.BitSelFuel },
            {ControlBits.BitSelEDSM,ControlBits.BitSelNotValid},
            {ControlBits.BitSelVisits,ControlBits.BitSelCredits},
            {ControlBits.BitSelBody,ControlBits.BitSelNotValid},
            {ControlBits.BitSelStationFaction,ControlBits.BitSelNotValid},
            {ControlBits.BitSelStationButtons,ControlBits.BitSelNotValid},
            {ControlBits.BitSelShipInfo,ControlBits.BitSelNotValid},
            {ControlBits.BitSelShipyardButtons,ControlBits.BitSelNotValid},
            {ControlBits.BitSelDistanceFrom,ControlBits.BitSelNotValid},
            {ControlBits.BitSelSystemState,ControlBits.BitSelNotValid},
            {ControlBits.BitSelSecurity,ControlBits.BitSelNotValid},
            {ControlBits.BitSelTarget,ControlBits.BitSelNotValid},
            {ControlBits.BitSelCargo,ControlBits.BitSelNotValid},
            {ControlBits.BitSelMats,ControlBits.BitSelData},
            {ControlBits.BitSelMR, ControlBits.BitSelNotValid },
            {ControlBits.BitSelGameMode,ControlBits.BitSelNotValid},
            {ControlBits.BitSelTravel,ControlBits.BitSelNotValid},
            {ControlBits.BitSelMissions,ControlBits.BitSelNotValid},
        };

        public const int HorzPositions = 8;
        const int hspacing = 2;

        private string shiptexttranslation;         // text of ship: entry

        private ToolStripMenuItem[] toolstriplist;          // ref to toolstrip items for each bit above. in same order as bits BitSel..

        private long Selection;          // selection bits
        private List<BaseUtils.LineStore> Lines;            // stores settings on each line, values are BitN+1, 0 means position not used.

        private EliteDangerousCore.JournalEvents.JournalFSDTarget lasttarget = null;        // set when UI FSDTarget passes thru and not in jump sequence on top of history
        private EliteDangerousCore.JournalEvents.JournalFSDTarget pendingtarget = null;     // if we are, its stored here, and transfered back to lasttarget on the next FSD
        private EliteDangerousCore.UIEvents.UIDestination lastdestination = null;           // When UI Destination comes thru

        private bool travelhistoryisattop = true;

        private HistoryEntry last_he = null;
        private uint lastprocessedidgen = uint.MaxValue;

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
                                        EDTx.UserControlSysInfo_toolStripReset, EDTx.UserControlSysInfo_toolStripRemoveAll , EDTx.UserControlSysInfo_displayNextDestinationToolStripMenuItem,
                                        EDTx.UserControlSysInfo_disableCopyToClipboardWhenClickingOnTextBoxesToolStripMenuItem};
            var enumlisttt = new Enum[] { EDTx.UserControlSysInfo_ToolTip, EDTx.UserControlSysInfo_textBoxTargetDist_ToolTip, EDTx.UserControlSysInfo_textBoxTarget_ToolTip };
            
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            textBoxTarget.SetAutoCompletor(SystemCache.ReturnSystemAdditionalListForAutoComplete);
            textBoxTarget.AutoCompleteTimeout = 500;

            // same order as Sel bits are defined in, one bit per selection item. Null if bit is no longer used
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

            //foreach (var l in Lines) { for (int i = 0; i < l.Items.Length; i++) {if ( l.Items[i]>0) System.Diagnostics.Debug.Write(((ControlBits)(l.Items[i]-1)).ToString() + " "); } System.Diagnostics.Debug.WriteLine(""); }

            for (int bitn = 0; bitn < BitSelTotal; bitn++)     // new bits added will not be in older lists, need to add on in!
            {
                if (BaseUtils.LineStore.FindValue(Lines, bitn + 1) == null)   // if can't find
                {
                    int insertat = Lines.Count;
                    ControlBits cb = (ControlBits)bitn;

                    if (cb == ControlBits.BitSelStationButtons)
                    {
                        var p = BaseUtils.LineStore.FindValue(Lines, (int)ControlBits.BitSelBody + 1);     // stored +1
                        if (p != null)
                            insertat = Lines.IndexOf(p) + 1;

                        Selection |= (1L << bitn);
                    }
                    else if (cb == ControlBits.BitSelShipyardButtons)
                    {
                        var p = BaseUtils.LineStore.FindValue(Lines, (int)ControlBits.BitSelShipInfo + 1);
                        if (p != null)
                            insertat = Lines.IndexOf(p) + 1;

                        Selection |= (1L << bitn);
                    }
                    else if (cb == ControlBits.BitSelStationFaction)
                    {
                        var p = BaseUtils.LineStore.FindValue(Lines, (int)ControlBits.BitSelBody + 1);
                        if (p != null)
                            insertat = Lines.IndexOf(p) + 1;

                        Selection |= (1L << bitn);
                    }
                    else if (cb == ControlBits.BitSelMR)
                    {
                        var p = BaseUtils.LineStore.FindValue(Lines, (int)ControlBits.BitSelData + 1);
                        if (p != null)
                            insertat = Lines.IndexOf(p) + 1;

                        Selection |= (1L << bitn);
                    }
                    else if (cb == ControlBits.BitSelSecurity)
                    {
                        var p = BaseUtils.LineStore.FindValue(Lines, (int)ControlBits.BitSelSystemState + 1);
                        if (p != null)
                            insertat = Lines.IndexOf(p) + 1;

                        Selection |= (1L << bitn);
                    }
                    else if (cb == ControlBits.BitSelNextDestination)
                    {
                        var p = BaseUtils.LineStore.FindValue(Lines, (int)ControlBits.BitSelPosition + 1);
                        if (p != null)
                            insertat = Lines.IndexOf(p) + 1;

                        Selection |= (1L << bitn);
                    }

                    Lines.Insert(insertat, new BaseUtils.LineStore() { Items = new int[HorzPositions] { bitn + 1, 0, 0, 0, 0, 0, 0, 0 } });
                }
            }

            DiscoveryForm.OnNewTarget += RefreshTargetDisplay;
            DiscoveryForm.OnEDSMSyncComplete += Discoveryform_OnEDSMSyncComplete;
            DiscoveryForm.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnThemeChanged += Discoveryform_OnThemeChanged;

            panelFD.BackgroundImage = EDDiscovery.Icons.Controls.notfirstdiscover;      // just to hide it during boot up

            shiptexttranslation = labelShip.Text;
        }

        public override void Closing()
        {
            DiscoveryForm.OnNewTarget -= RefreshTargetDisplay;
            DiscoveryForm.OnEDSMSyncComplete -= Discoveryform_OnEDSMSyncComplete;
            DiscoveryForm.OnNewUIEvent -= Discoveryform_OnNewUIEvent;

            PutSetting(dbOSave, BaseUtils.LineStore.ToString(Lines));
            PutSetting(dbSelection, Selection);
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            UpdateViewOnSelection();  // then turn the right ones on
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
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
                var tophe = DiscoveryForm.History.GetLast;      // we feed in the top, which is being updated by EDDiscoveryControllerNewEntry with the latest fuel
                if (tophe != null)   // paranoia
                {
                    //System.Diagnostics.Debug.WriteLine($"UI Top he Fuel {tophe.EventTimeUTC} {tophe.ShipInformation.FuelLevel} {tophe.ShipInformation.ReserveFuelCapacity}");
                    Display(tophe);
                }
            }

            if (obj is EliteDangerousCore.UIEvents.UIFSDTarget)
            {
                var j = ((EliteDangerousCore.UIEvents.UIFSDTarget)obj).FSDTarget;
                if (lasttarget == null || j.StarSystem != lasttarget.StarSystem)       // a little bit of debouncing, see if the target info has changed
                {
                    if ( (DiscoveryForm.History.GetLast?.Status.FSDJumpSequence??false)  == true) 
                    {
                        //System.Diagnostics.Debug.WriteLine($"Sysinfo - FSD target got, but in fsd sequence, pend it");
                        pendingtarget = j;
                    }
                    else
                    {
                        lasttarget = j;
                        //System.Diagnostics.Debug.WriteLine($"Sysinfo - FSD target got");
                        Display(last_he);
                    }
                }
            }
            if (obj is EliteDangerousCore.UIEvents.UIDestination)
            {
                var j = (EliteDangerousCore.UIEvents.UIDestination)obj;
                if (lastdestination == null || j.Name != lastdestination.Name || j.BodyID != lastdestination.BodyID)        // if name or bodyid has changed
                {
                    //System.Diagnostics.Debug.WriteLine($"Sysinfo - Destination got");
                    lastdestination = j;
                    Display(last_he);
                }
            }
        }

        // override and intercept events
        public override PanelActionState PerformPanelOperation(UserControlCommonBase sender, object actionobj)
        {
            if (actionobj is EliteDangerousCore.HistoryEntry)       // he from travel grid
            {
                //System.Diagnostics.Debug.WriteLine($"systeminfo perform panel operation history");
                NewHistoryEntry((EliteDangerousCore.HistoryEntry)actionobj);
                return PanelActionState.HandledContinue;
            }
            else if (actionobj is UserControlCommonBase.TravelHistoryStartStopChanged)  // travel change
            {
                Display(last_he);
                return PanelActionState.HandledContinue;
            }

            return PanelActionState.NotHandled;
        }


        public void NewHistoryEntry(HistoryEntry he)
        {
            //System.Diagnostics.Debug.WriteLine($"Sysinfo {DisplayNumber} : Cursor {he.Index} {he.EventSummary}");
            travelhistoryisattop = he == DiscoveryForm.History.GetLast;      // see if tracking at top

            bool duetosystem = last_he == null;
            bool duetostatus = false;
            bool duetocomms = false;
            bool duetoship = false;
            bool duetoother = false;
            bool duetomissions = false;
            bool duetoidentifiers = Identifiers.Generation != lastprocessedidgen;  // global history kick if Ids changed, not on he, but if its changed, a new he would be coming in

            if (he.Status.FSDJumpSequence == false && pendingtarget != null )      // if we reached the end of the fsd sequence, but we have a pend, free
            {
                //System.Diagnostics.Debug.WriteLine($"Sysinfo - FSDJump and pending target set, so end of jump sequence");
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

            if (duetosystem || duetostatus || duetocomms || duetoship || duetoother || duetomissions || duetoidentifiers)
            {
                //System.Diagnostics.Debug.WriteLine($"SysInfo - {he.journalEntry.EventTypeStr} got: sys {duetosystem} st {duetostatus} comds {duetocomms} ship {duetoship} missions {duetomissions} other {duetoother}");
                Display(he);
                lastprocessedidgen = Identifiers.Generation;   // stop identifiers kicking again
            }
        }

        private async void Display(HistoryEntry he) 
        {
            last_he = he;
            var hl = DiscoveryForm.History;

            if (last_he != null)
            {
                SetControlText(he.System.Name);

                HistoryEntry lastfsd = hl.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.FSDJump, he);

                textBoxSystem.Text = he.System.Name;
#if DEBUG
                textBoxSystem.Text += $" {he.System.SystemAddress}";
#endif
                panelFD.BackgroundImage = (lastfsd != null && (lastfsd.journalEntry as EliteDangerousCore.JournalEvents.JournalFSDJump).EDSMFirstDiscover) ? EDDiscovery.Icons.Controls.firstdiscover : EDDiscovery.Icons.Controls.notfirstdiscover;

                textBoxBody.Text = he.WhereAmI + " (" + he.Status.BodyType + ")";

                bool hasmarketid = he?.Status.MarketID.HasValue ?? false;
                bool hasbodyormarketid = hasmarketid || he.FullBodyID.HasValue;

                extButtonInaraStation.Enabled = hasmarketid;
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


                extButtonEDSMSystem.Enabled = extButtonInaraSystem.Enabled = extButtonSpanshSystem.Enabled = true;

                string allegiance, economy, gov, faction, factionstate, security;
                hl.ReturnSystemInfo(he, out allegiance, out economy, out gov, out faction, out factionstate, out security);

                textBoxAllegiance.Text = allegiance;
                textBoxEconomy.Text = economy;
                textBoxGovernment.Text = gov;
                textBoxState.Text = factionstate;
                extTextBoxSecurity.Text = security;

                extTextBoxStationFaction.Text = he.Status.StationFaction ?? "";

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

                textBoxGameMode.Text = he.Status.GameModeGroupMulticrew;

                if (he.isTravelling)
                {
                    textBoxTravelDist.Text = he.TravelledDistance.ToString("0.0") + "ly";
                    textBoxTravelTime.Text = he.TravelledTimeSpan().ToString();
                    textBoxTravelJumps.Text = he.TravelledJumps.ToString();
                }
                else
                {
                    textBoxTravelDist.Text = textBoxTravelTime.Text = textBoxTravelJumps.Text = "";
                }

                var mcl = hl.MaterialCommoditiesMicroResources.Get(he.MaterialCommodity);
                var counts = MaterialCommoditiesMicroResourceList.Count(mcl,0);
                int cargocount = counts[(int)MaterialCommodityMicroResourceType.CatType.Commodity];

                int cc = he.ShipInformation?.CalculateCargoCapacity() ?? 0;// so if we don't have a ShipInformation, use 0
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

                double? currentjumprange = fsd?.JumpRange(cargocount, he.ShipInformation.HullModuleMass(), he.ShipInformation.FuelLevel, he.Status.CurrentBoost);


                textBoxJumpRange.Text = "";

                if (he.Status.OnFoot)
                {
                    labelShip.Text = "On Foot".T(EDTx.UserControlSysInfo_OnFoot);   

                    var cursuit = hl.SuitList.CurrentID(he.Suits);                     // get current suit ID, or 0 if none
                    if (cursuit != 0)
                    {
                        var suit = hl.SuitList.Suit(cursuit,he.Suits);                // get suit
                        textBoxShip.Text = suit?.FriendlyName ?? "???";
                        var curloadout = DiscoveryForm.History.SuitLoadoutList.CurrentID(he.Loadouts);         // get current loadout ID, or 0 if none

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
                        Ship si = he.ShipInformation;

                        textBoxShip.Text = si.ShipFullInfo(cargo: false, fuel: false);

                        if (si.FuelCapacity > 0 && si.FuelLevel > 0)            // Fuel info 
                            textBoxFuel.Text = si.FuelLevel.ToString("0.#") + "/" + si.FuelCapacity.ToString("0.#");
                        else if (si.FuelCapacity > 0)
                            textBoxFuel.Text = si.FuelCapacity.ToString("0.#");
                        else
                            textBoxFuel.Text = "N/A".T(EDTx.UserControlSysInfo_NA);

                        if ( currentjumprange != null)
                            textBoxJumpRange.Text = currentjumprange.Value.ToString("N2") + "ly";

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
                    string destname = "";
                    string starclass = "";
                    string distance = "";
                    string onbody = "";
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
                            // else body name destination or $POI $MULTIPLAYER etc
                            // Its not localised, so attempt a rename for those $xxx forms ($Multiplayer.. $POI)

                            destname = JournalFieldNaming.SignalBodyName(lastdestination.Name);   

                            //System.Diagnostics.Debug.WriteLine($"Sysinfo destination {lastdestination.Name} -> {destname}");

                            var ss = await DiscoveryForm.History.StarScan.FindSystemAsync(DiscoveryForm.History.GetLast.System);
                            if (IsClosed)       //ASYNC! warning! may have closed.
                                return;

                            // with a found system, see if we can get the body name so we know what body its on
                            if (ss != null && ss.NodesByID.TryGetValue(lastdestination.BodyID, out StarScan.ScanNode body))
                            {
                                onbody = body.BodyDesignator.ReplaceIfStartsWith(he.System.Name).Trim();

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

                        if (sys != null && sys.HasCoordinate && DiscoveryForm.History.GetLast != null)       // check we have data - defense against #3373, if the async call was long the history could have changed
                        {
                            double dist = sys.Distance(DiscoveryForm.History.GetLast.System);       // we must have a last to be here
                            distance = $"{dist:N2}ly";

                            if (currentjumprange != null) // and therefore fsd is non null
                            {
                                double fuel = fsd.FuelUse(cargocount, he.ShipInformation.HullModuleMass(), he.ShipInformation.FuelLevel, dist, he.Status.CurrentBoost);
                                distance += $" {fuel:N2}t";

                                if (currentjumprange.Value < dist)
                                    textdistcolor = ExtendedControls.Theme.Current.TextBlockHighlightColor;
                            }

                            onbody = $"{sys.X:N1}, {sys.Y:N1}, {sys.Z:N1}";
                        }
                    }

                    extTextBoxNextDestination.Text = $"{starname}{destname}{starclass}";
                    extTextBoxNextDestinationDistance.Text = distance;
                    extTextBoxNextDestinationPosition.Text = onbody;
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

                extButtonEDSMSystem.Enabled = extButtonInaraSystem.Enabled = extButtonSpanshSystem.Enabled = false;
                extButtonInaraStation.Enabled = extButtonSpanshStation.Enabled = false;
                extButtonCoriolis.Enabled = extButtonEDSY.Enabled = false;
            }
        }

        #endregion

        #region Clicks

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
                var ir = new EliteDangerousCore.Inara.InaraClass();
                ir.LaunchBrowserForSystem(last_he.System.Name);
            }
        }

        private void extButtonSpanshSystem_Click(object sender, EventArgs e)
        {
            if (last_he != null && last_he.System.SystemAddress.HasValue)
                EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForSystem(last_he.System.SystemAddress.Value);
        }

        private void extButtonInaraStation_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                var ir = new EliteDangerousCore.Inara.InaraClass();
                ir.LaunchBrowserForStation(last_he.System.Name,last_he.WhereAmI);
            }
        }

        private void extButtonSpanshStation_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                if (last_he.Status.MarketID != null)
                    EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForStationByMarketID(last_he.Status.MarketID.Value);
                else if (last_he.FullBodyID.HasValue)
                    EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForStationByFullBodyID(last_he.FullBodyID.Value);
            }
        }

        private void extButtonEDSY_Click(object sender, EventArgs e)
        {
            if (last_he?.ShipInformation != null)
            {
                var si = last_he.ShipInformation;

                string loadoutjournalline = si.ToJSONLoadout();

                //     File.WriteAllText(@"c:\code\loadoutout.txt", loadoutjournalline);

                string uri = EDDConfig.Instance.EDDShipyardURL + "#/I=" + loadoutjournalline.URIGZipBase64Escape();

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

                string uri = EDDConfig.Instance.CoriolisURL + "data=" + s.URIGZipBase64Escape() + "&bn=" + Uri.EscapeDataString(si.Name);

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
                        DiscoveryForm.NewTargetSet(this);
                    }
                    else
                    {
                        GalacticMapObject gmo = DiscoveryForm.GalacticMapping.FindDescriptiveNameOrSystem(textBoxTarget.Text, true);       // find gmo, any part
                        if (gmo != null)
                        {
                            TargetClass.SetTargetOnGMO(gmo.NameList,gmo.ID, gmo.Points[0].X, gmo.Points[0].Y, gmo.Points[0].Z);
                            textBoxTarget.Text = gmo.NameList;
                            DiscoveryForm.NewTargetSet(this);
                        }
                        else
                            Console.Beep(256, 200); // beep boop!
                    }
                }
                else
                {
                    TargetClass.ClearTarget();          // empty means clear
                    DiscoveryForm.NewTargetSet(this);
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

                HistoryEntry cs = DiscoveryForm.History.GetLastWithPosition();
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
            if ( (Selection & (1L<<(int)(ControlBits.BitSelDisableCopyToClipboardWhenClickingOnTextBoxes))) == 0)
                SetClipboardText(((Control)sender).Text);
        }

        private void toolStripSystem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelSystem);
        }
        private void toolStripBody_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelBody);
        }
        private void toolStripTarget_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelTarget);
        }
        private void toolStripEDSMButtons_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelEDSMButtonsNextLine);
        }
        private void toolStripEDSM_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelEDSM);
        }
        private void toolStripVisits_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelVisits);
        }
        private void toolStripPosition_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelPosition);
        }
        private void enableDistanceFromToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelDistanceFrom);
        }
        private void toolStripSystemState_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelSystemState);
        }
        private void toolStripGameMode_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelGameMode);
        }
        private void toolStripTravel_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelTravel);
        }
        private void toolStripCargo_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelCargo);
        }
        private void toolStripMaterialCount_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelMats);
        }
        private void toolStripDataCount_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelData);
        }
        private void toolStripCredits_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelCredits);
        }
        private void toolStripShip_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelShipInfo);
        }
        private void toolStripFuel_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelFuel);
        }

        private void displayJumpRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelJumpRange);
        }

        private void toolStripMissionsList_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelMissions);
        }
        private void displayStationButtonsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelStationButtons);
        }

        private void displayShipButtonsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelShipyardButtons);
        }

        private void displayStationFactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelStationFaction);
        }

        private void displayMicroresourcesCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelMR);
        }
        private void displaySecurityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelSecurity);
        }

        private void displayNextDestinationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelNextDestination);
        }

        private void toolStripRemoveAll_Click(object sender, EventArgs e)
        {
            Selection = (Selection | ((1L << BitSelTotal) - 1)) ^ ((1L << BitSelTotal) - 1);
            UpdateViewOnSelection();
        }

        private void whenTransparentUseSkinnyLookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, ControlBits.BitSelSkinny);
            UpdateTransparency();
        }

        private void disableCopyToClipboardWhenClickingOnTextBoxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Selection = Selection ^ (1L << (int)ControlBits.BitSelDisableCopyToClipboardWhenClickingOnTextBoxes);
            UpdateViewOnSelection();
        }

        void ToggleSelection(Object sender, ControlBits bit)
        {
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            if (mi.Enabled)
            {
                Selection ^= (1L << (int)bit);
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

            bool selEDSMonNextLine = (Selection & (1L << (int)ControlBits.BitSelEDSMButtonsNextLine)) != 0;
            toolStripEDSMDownLine.Checked = selEDSMonNextLine;
            toolStripSkinny.Checked = (Selection & (1L << (int)ControlBits.BitSelSkinny)) != 0;
            disableCopyToClipboardWhenClickingOnTextBoxesToolStripMenuItem.Checked = (Selection & (1L << (int)ControlBits.BitSelDisableCopyToClipboardWhenClickingOnTextBoxes)) != 0;

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
                        bool ison = (Selection & (1L << bitno)) != 0;

                        toolstriplist[bitno].Enabled = false;
                        toolstriplist[bitno].Checked = ison;
                        toolstriplist[bitno].Enabled = true;

                        int itembottom = rowbottom;

                        if (ison)
                        {
                            var cb = (ControlBits)bitno;

                            Lines[r].YStart = ypos;
                            int si = r * HorzPositions + c;

                            Point labpos = new Point(3 + c * coloffset, ypos);
                            Point datapos = new Point(labpos.X + data1offset, labpos.Y);
                            Point labpos2 = new Point(labpos.X + lab2offset, labpos.Y);
                            Point datapos2 = new Point(labpos.X + data2offset, labpos.Y);

                            //System.Diagnostics.Debug.WriteLine($"Position {cb.ToString()} at {labpos} {datapos} {labpos2} {datapos2}");

                            switch (cb)
                            {
                                case ControlBits.BitSelSystem:
                                    itembottom = this.SetPos(labpos, labelSysName, datapos, textBoxSystem, si);
                                    panelFD.Location = new Point(textBoxSystem.Right, textBoxSystem.Top);
                                    panelFD.Tag = si;
                                    panelFD.Visible = true;

                                    if (!selEDSMonNextLine && (Selection & (1L << (int)ControlBits.BitSelEDSM)) != 0)
                                    {
                                        extButtonEDSMSystem.Location = new Point(panelFD.Right + hspacing, datapos.Y);
                                        extButtonInaraSystem.Location = new Point(extButtonEDSMSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonSpanshSystem.Location = new Point(extButtonInaraSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonEDSMSystem.Visible = extButtonInaraSystem.Visible = extButtonSpanshSystem.Visible = true;
                                        extButtonEDSMSystem.Tag = extButtonInaraSystem.Tag = extButtonSpanshSystem.Tag = si;
                                        itembottom = Math.Max(extButtonEDSMSystem.Bottom, itembottom);
                                    }

                                    break;
                                case ControlBits.BitSelEDSM:
                                    if (selEDSMonNextLine)
                                    {
                                        labelOpen.Location = labpos;
                                        extButtonEDSMSystem.Location = new Point(datapos.X, datapos.Y);
                                        extButtonInaraSystem.Location = new Point(extButtonEDSMSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        extButtonSpanshSystem.Location = new Point(extButtonInaraSystem.Right + hspacing, extButtonEDSMSystem.Top);
                                        labelOpen.Tag = extButtonEDSMSystem.Tag = extButtonInaraSystem.Tag = extButtonSpanshSystem.Tag = si;
                                        extButtonEDSMSystem.Visible = extButtonInaraSystem.Visible = extButtonSpanshSystem.Visible = true;
                                        labelOpen.Visible = true;
                                        itembottom = extButtonEDSMSystem.Bottom;
                                    }
                                    break;

                                case ControlBits.BitSelNextDestination:
                                    itembottom = this.SetPos(labpos, labelNextDestination, datapos, extTextBoxNextDestination, si);
                                    itembottom++;
                                    extTextBoxNextDestinationDistance.Location = new Point(extTextBoxNextDestination.Left, itembottom);
                                    extTextBoxNextDestinationPosition.Bounds = new Rectangle(extTextBoxNextDestinationDistance.Right + hspacing, extTextBoxNextDestinationDistance.Top,
                                                                                             extTextBoxNextDestination.Width - hspacing - extTextBoxNextDestinationDistance.Width, extTextBoxNextDestinationPosition.Height);
                                    extTextBoxNextDestinationDistance.Visible = extTextBoxNextDestinationPosition.Visible = true;
                                    extTextBoxNextDestinationDistance.Tag = extTextBoxNextDestinationPosition.Tag = si;
                                    itembottom = Math.Max(extTextBoxNextDestinationDistance.Bottom, itembottom);
                                    break;


                                case ControlBits.BitSelStationButtons:
                                    labelOpenStation.Location = labpos;
                                    extButtonInaraStation.Location = new Point(datapos.X, datapos.Y); 
                                    extButtonSpanshStation.Location = new Point(extButtonInaraStation.Right + hspacing, extButtonInaraStation.Top);
                                    labelOpenStation.Tag = extButtonInaraStation.Tag = extButtonSpanshStation.Tag = si;
                                    extButtonInaraStation.Visible = extButtonSpanshStation.Visible = true;
                                    labelOpenStation.Visible = true;
                                    itembottom = extButtonInaraStation.Bottom;

                                    break;

                                case ControlBits.BitSelShipyardButtons:
                                    labelOpenShip.Location = labpos;
                                    extButtonCoriolis.Location = new Point(datapos.X, datapos.Y);
                                    extButtonEDSY.Location = new Point(extButtonCoriolis.Right + hspacing, extButtonCoriolis.Top);
                                    labelOpenShip.Tag = extButtonEDSY.Tag = extButtonCoriolis.Tag = si;
                                    extButtonEDSY.Visible = extButtonCoriolis.Visible = true;
                                    labelOpenShip.Visible = true;
                                    itembottom = extButtonCoriolis.Bottom;
                                    break;


                                case ControlBits.BitSelVisits:
                                    itembottom = this.SetPos(labpos, labelVisits, datapos, textBoxVisits, si);
                                    break;

                                case ControlBits.BitSelBody:
                                    itembottom = this.SetPos(labpos, labelBodyName, datapos, textBoxBody, si);
                                    break;

                                case ControlBits.BitSelPosition:
                                    itembottom = this.SetPos(labpos, labelPosition, datapos, textBoxPosition, si);
                                    break;

                                case ControlBits.BitSelDistanceFrom:
                                    itembottom = this.SetPos(labpos, labelHomeDist, datapos, textBoxHomeDist, si);
                                    OffsetPos(labpos2, labelSolDist, datapos2, textBoxSolDist, si);
                                    break;

                                case ControlBits.BitSelSystemState:
                                    itembottom = this.SetPos(labpos, labelState, datapos, textBoxState, si);
                                    OffsetPos(labpos2, labelAllegiance, datapos2, textBoxAllegiance, si);
                                    labpos.Y = datapos.Y = labpos2.Y = datapos2.Y = itembottom + 1;
                                    itembottom = this.SetPos(labpos, labelGov, datapos, textBoxGovernment, si);
                                    OffsetPos(labpos2, labelEconomy, datapos2, textBoxEconomy, si);
                                    break;

                                case ControlBits.BitSelTarget:
                                    itembottom = this.SetPos(labpos, labelTarget, datapos, textBoxTarget, si);
                                    textBoxTargetDist.Location = new Point(textBoxTarget.Right + hspacing, datapos.Y);
                                    extButtonEDSMTarget.Location = new Point(textBoxTargetDist.Right + hspacing, datapos.Y);
                                    textBoxTargetDist.Tag = extButtonEDSMTarget.Tag = si;
                                    textBoxTargetDist.Visible = extButtonEDSMTarget.Visible = true;
                                    break;

                                case ControlBits.BitSelGameMode:
                                    itembottom = this.SetPos(labpos, labelGamemode, datapos, textBoxGameMode, si);
                                    break;

                                case ControlBits.BitSelTravel:
                                    itembottom = this.SetPos(labpos, labelTravel, datapos, textBoxTravelDist, si);
                                    textBoxTravelTime.Location = new Point(textBoxTravelDist.Right + hspacing, datapos.Y);
                                    textBoxTravelJumps.Location = new Point(textBoxTravelTime.Right + hspacing, datapos.Y);
                                    textBoxTravelTime.Tag = textBoxTravelJumps.Tag = si;
                                    textBoxTravelTime.Visible = textBoxTravelJumps.Visible = true;
                                    // don't set visible for the last two, may not be if not travelling. Display will deal with it
                                    break;

                                case ControlBits.BitSelCargo:
                                    itembottom = this.SetPos(labpos, labelCargo, datapos, textBoxCargo, si);
                                    break;

                                case ControlBits.BitSelMats:
                                    itembottom = this.SetPos(labpos, labelMaterials, datapos, textBoxMaterials, si);
                                    break;

                                case ControlBits.BitSelData:
                                    itembottom = this.SetPos(labpos, labelData, datapos, textBoxData, si);
                                    break;

                                case ControlBits.BitSelShipInfo:
                                    itembottom = this.SetPos(labpos, labelShip, datapos, textBoxShip, si);
                                    break;

                                case ControlBits.BitSelFuel:
                                    itembottom = this.SetPos(labpos, labelFuel, datapos, textBoxFuel, si);
                                    break;

                                case ControlBits.BitSelCredits:
                                    itembottom = this.SetPos(labpos, labelCredits, datapos, textBoxCredits, si);
                                    break;

                                case ControlBits.BitSelMissions:
                                    itembottom = this.SetPos(labpos, labelMissions, datapos, richTextBoxScrollMissions, si);
                                    break;

                                case ControlBits.BitSelJumpRange:
                                    itembottom = this.SetPos(labpos, labelJumpRange, datapos, textBoxJumpRange, si);
                                    break;

                                case ControlBits.BitSelStationFaction:
                                    itembottom = this.SetPos(labpos, labelStationFaction, datapos, extTextBoxStationFaction, si);
                                    break;

                                case ControlBits.BitSelMR:
                                    itembottom = this.SetPos(labpos, labelMR, datapos, extTextBoxMR, si);
                                    break;
                                case ControlBits.BitSelSecurity:
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
                Lines.Add(new BaseUtils.LineStore() { Items = new int[HorzPositions] { (int)resetorder[i,0]+1, (int)resetorder[i,1]+1, 0, 0, 0, 0, 0, 0 } });
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

            bool skinny = (Selection & (1L << (int)ControlBits.BitSelSkinny)) != 0;

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
