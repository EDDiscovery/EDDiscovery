/*
 * Copyright © 2017 EDDiscovery development team
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Collections;
using System.IO.Compression;
using ExtendedControls;
using System.Windows.Forms;

namespace EDDiscovery.Icons
{
    public static class IconSet
    {
        private static Dictionary<string, Image> defaultIcons;
        private static Dictionary<string, Image> icons;

        static IconSet()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resnames = asm.GetManifestResourceNames();
            defaultIcons = new Dictionary<string, Image>(StringComparer.InvariantCultureIgnoreCase);
            string basename = typeof(IconSet).Namespace + ".";

            foreach (string resname in resnames)
            {
                if (resname.StartsWith(basename) && resname.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                {
                    string name = resname.Substring(basename.Length, resname.Length - basename.Length - 4);
                    Image img = Image.FromStream(asm.GetManifestResourceStream(resname));
                    img.Tag = name;
                    defaultIcons[name] = img;
                }
            }
        }

        private static void InitLegacyIcons()
        {
            icons["Legacy.settings"] = IconSet.GetIcon("Controls.Main.Tools.Settings");
            icons["Legacy.missioncompleted"] = IconSet.GetIcon("Journal.MissionCompleted");

#if false
            icons["Legacy.A9III_White"] = IconSet.GetIcon("Stars.A");
            icons["Legacy.Ammonia_Brown"] = IconSet.GetIcon("Planets.Ammonia_world");
            icons["Legacy.B6V_Blueish"] = IconSet.GetIcon("Stars.B");
            icons["Legacy.Barycentre"] = IconSet.GetIcon("Controls.Scan.Bodies.Barycentre");
            icons["Legacy.Belt"] = IconSet.GetIcon("Controls.Scan.Bodies.Belt");
            icons["Legacy.Black_Hole"] = IconSet.GetIcon("Stars.H");
            icons["Legacy.Blackhole"] = IconSet.GetIcon("GalMap.blackHole");
            icons["Legacy.C7III"] = IconSet.GetIcon("Stars.C");
            icons["Legacy.Calendar"] = IconSet.GetIcon("Controls.DateTimePicker.Calendar");
            icons["Legacy.Class_III_Gas_Giant_Blue3"] = IconSet.GetIcon("Planets.Sudarsky_class_III_gas_giant");
            icons["Legacy.Class_II_Gas_Giant_Sand1"] = IconSet.GetIcon("Planets.Sudarsky_class_II_gas_giant");
            icons["Legacy.Class_I_Gas_Giant_Brown2"] = IconSet.GetIcon("Planets.Sudarsky_class_I_gas_giant");
            icons["Legacy.Coffinicon"] = IconSet.GetIcon("Journal.Died");
            icons["Legacy.Compass"] = IconSet.GetIcon("Panels.Compass");
            icons["Legacy.DA6VII_White"] = IconSet.GetIcon("Stars.DA");
            icons["Legacy.DefaultStar"] = IconSet.GetIcon("GalMap.stellarRemnant");
            icons["Legacy.EDDB"] = IconSet.GetIcon("Controls.Main.Admin.EDDBSystemsSync");
            icons["Legacy.EDDN"] = IconSet.GetIcon("Controls.Main.Admin.SendUnsyncedEDDN");
            icons["Legacy.Earth_Like_Standard"] = IconSet.GetIcon("Planets.Earthlike_body");
            icons["Legacy.F5VAB"] = IconSet.GetIcon("Stars.F");
            icons["Legacy.G1IV"] = IconSet.GetIcon("Stars.G");
            icons["Legacy.Gas_giant_ammonia_based_life1"] = IconSet.GetIcon("Planets.Gas_giant_with_ammonia_based_life");
            icons["Legacy.Gas_giant_water_based_life_Brown3"] = IconSet.GetIcon("Planets.Gas_giant_with_water_based_life");
            icons["Legacy.Globe"] = IconSet.GetIcon("Planets.Unknown");
            icons["Legacy.Helium_Rich_Gas_Giant1"] = IconSet.GetIcon("Planets.Helium_rich_gas_giant");
            icons["Legacy.High_metal_content_world_Lava1"] = IconSet.GetIcon("Planets.High_metal_content_body_700");
            icons["Legacy.High_metal_content_world_Mix3"] = IconSet.GetIcon("Planets.High_metal_content_body_250");
            icons["Legacy.High_metal_content_world_Orange8"] = IconSet.GetIcon("Planets.High_metal_content_body");
            icons["Legacy.High_metal_content_world_White3"] = IconSet.GetIcon("Planets.High_metal_content_body_hot_thick");
            icons["Legacy.Homeicon"] = IconSet.GetIcon("Controls.Map3D.Navigation.GoToHomeSystem");
            icons["Legacy.Icy_Body_Greenish1"] = IconSet.GetIcon("Planets.Icy_body");
            icons["Legacy.ImageStarDisc"] = IconSet.GetIcon("Controls.Map3D.Stars.ShowDiscs");
            icons["Legacy.ImageStarDiscWhite"] = IconSet.GetIcon("Controls.Map3D.Travel.WhiteStars");
            icons["Legacy.ImageTravel"] = IconSet.GetIcon("Controls.Map3D.Travel.Menu");
            icons["Legacy.K0V"] = IconSet.GetIcon("Stars.K_OrangeGiant");
            icons["Legacy.K1IV"] = IconSet.GetIcon("Stars.AeBe");
            icons["Legacy.L3V"] = IconSet.GetIcon("Stars.L");
            icons["Legacy.Log"] = IconSet.GetIcon("Panels.Log");
            icons["Legacy.M5V"] = IconSet.GetIcon("Stars.M");
            icons["Legacy.Moon24"] = IconSet.GetIcon("Controls.Scan.ShowMoons");
            icons["Legacy.Neutron_Star"] = IconSet.GetIcon("Stars.N");
            icons["Legacy.O"] = IconSet.GetIcon("Stars.O");
            icons["Legacy.OrangeDot"] = IconSet.GetIcon("Controls.Map3D.OrangeDot");
            icons["Legacy.PauseNormalRed"] = IconSet.GetIcon("Controls.Map3D.Recorder.Pause");
            icons["Legacy.PlanetaryNebula"] = IconSet.GetIcon("GalMap.planetaryNebula");
            icons["Legacy.PlayNormal"] = IconSet.GetIcon("Controls.Map3D.Recorder.Play");
            icons["Legacy.RecordPressed"] = IconSet.GetIcon("Controls.Map3D.Recorder.Record");
            icons["Legacy.Ring Only 512"] = IconSet.GetIcon("Controls.Scan.Bodies.RingOnly");
            icons["Legacy.RingGap512"] = IconSet.GetIcon("Controls.Scan.Bodies.RingGap");
            icons["Legacy.Rocky_Body_Sand2"] = IconSet.GetIcon("Planets.Rocky_body");
            icons["Legacy.Rocky_Ice_World_Sol_Titan"] = IconSet.GetIcon("Planets.Rocky_ice_body");
            icons["Legacy.SizeSelectorsLarge"] = IconSet.GetIcon("Controls.Scan.SizeLarge");
            icons["Legacy.SizeSelectorsMedium"] = IconSet.GetIcon("Controls.Scan.SizeMedium");
            icons["Legacy.SizeSelectorsSmall"] = IconSet.GetIcon("Controls.Scan.SizeSmall");
            icons["Legacy.SizeSelectorsTiny"] = IconSet.GetIcon("Controls.Scan.SizeTiny");
            icons["Legacy.Star_K1IV"] = IconSet.GetIcon("Stars.K");
            icons["Legacy.Stationenter"] = IconSet.GetIcon("Journal.Docked");
            icons["Legacy.Stationexit"] = IconSet.GetIcon("Journal.Undocked");
            icons["Legacy.StopNormalBlue"] = IconSet.GetIcon("Controls.Map3D.Recorder.StopPlay");
            icons["Legacy.StopNormalRed"] = IconSet.GetIcon("Controls.Map3D.Recorder.StopRecord");
            icons["Legacy.T4V"] = IconSet.GetIcon("Stars.T");
            icons["Legacy.terraform"] = IconSet.GetIcon("Controls.Scan.Bodies.Terraformable");
            icons["Legacy.Travelicon"] = IconSet.GetIcon("Controls.Map3D.Navigation.GoToHistorySelection");
            icons["Legacy.VideoRecorder"] = IconSet.GetIcon("Controls.Map3D.Recorder.Menu");
            icons["Legacy.Volcano"] = IconSet.GetIcon("Controls.Scan.Bodies.Volcanism");
            icons["Legacy.Water_Giant1"] = IconSet.GetIcon("Planets.Water_giant");
            icons["Legacy.Water_World_Poles_Cloudless4"] = IconSet.GetIcon("Planets.Water_world");
            icons["Legacy.WolfRayet"] = IconSet.GetIcon("Stars.W");
            icons["Legacy.Y2"] = IconSet.GetIcon("Stars.Y");
            icons["Legacy.YellowDot"] = IconSet.GetIcon("Controls.Map3D.YellowDot");
            icons["Legacy.amfurepair"] = IconSet.GetIcon("Journal.AfmuRepair");
            icons["Legacy.ammunition"] = IconSet.GetIcon("Journal.BuyAmmo");
            icons["Legacy.approachsettlement"] = IconSet.GetIcon("Journal.ApproachSettlement");
            icons["Legacy.bookmarkbrightred"] = IconSet.GetIcon("Controls.Map3D.Bookmarks.Noted");
            icons["Legacy.bookmarkgreen"] = IconSet.GetIcon("Controls.Map3D.Bookmarks.Star");
            icons["Legacy.bookmarktarget"] = IconSet.GetIcon("Controls.Map3D.Bookmarks.Target");
            icons["Legacy.bookmarkyellow"] = IconSet.GetIcon("Controls.Map3D.Bookmarks.Region");
            icons["Legacy.bounty"] = IconSet.GetIcon("Journal.Bounty");
            icons["Legacy.buttonCenter"] = IconSet.GetIcon("Controls.Map3D.Navigation.CenterOnSystem");
            icons["Legacy.buttonExt0"] = IconSet.GetIcon("Controls.SPanel.ResizeColumn");
            icons["Legacy.buttonExt1"] = IconSet.GetIcon("Controls.SPanel.ResizeColumn");
            icons["Legacy.buttonExt10"] = IconSet.GetIcon("Controls.SPanel.ResizeColumn");
            icons["Legacy.buttonExt11"] = IconSet.GetIcon("Controls.SPanel.ResizeColumn");
            icons["Legacy.buttonExt12"] = IconSet.GetIcon("Controls.SPanel.ResizeColumn");
            icons["Legacy.buttonExt2"] = IconSet.GetIcon("Controls.SPanel.ResizeColumn");
            icons["Legacy.buttonExt3"] = IconSet.GetIcon("Controls.SPanel.ResizeColumn");
            icons["Legacy.buttonExt4"] = IconSet.GetIcon("Controls.SPanel.ResizeColumn");
            icons["Legacy.buttonExt5"] = IconSet.GetIcon("Controls.SPanel.ResizeColumn");
            icons["Legacy.buttonExt6"] = IconSet.GetIcon("Controls.SPanel.ResizeColumn");
            icons["Legacy.buttonExt7"] = IconSet.GetIcon("Controls.SPanel.ResizeColumn");
            icons["Legacy.buttonExt8"] = IconSet.GetIcon("Controls.SPanel.ResizeColumn");
            icons["Legacy.buttonExt9"] = IconSet.GetIcon("Controls.SPanel.ResizeColumn");
            icons["Legacy.buydrones"] = IconSet.GetIcon("Journal.BuyDrones");
            icons["Legacy.buyexplorationdata"] = IconSet.GetIcon("Journal.BuyExplorationData");
            icons["Legacy.buytradedata"] = IconSet.GetIcon("Journal.BuyTradeData");
            icons["Legacy.capshipbond"] = IconSet.GetIcon("Journal.CapShipBond");
            icons["Legacy.cargomanifest"] = IconSet.GetIcon("Journal.Cargo");
            icons["Legacy.changecrewrole"] = IconSet.GetIcon("Journal.CrewMemberRoleChange");
            icons["Legacy.changecrewrols"] = IconSet.GetIcon("Journal.ChangeCrewRole");
            icons["Legacy.checkBoxCustomGraph"] = IconSet.GetIcon("Controls.StatsTime.Graph");
            icons["Legacy.checkBoxCustomPlanets"] = IconSet.GetIcon("Controls.StatsTime.Planets");
            icons["Legacy.checkBoxCustomStars"] = IconSet.GetIcon("Controls.StatsTime.Stars");
            icons["Legacy.checkBoxCustomText"] = IconSet.GetIcon("Controls.StatsTime.Text");
            icons["Legacy.checkBoxMaterials"] = IconSet.GetIcon("Controls.Scan.ShowAllMaterials");
            icons["Legacy.checkForNewReleaseToolStripMenuItem"] = IconSet.GetIcon("Controls.Main.Help.CheckForNewRelease");
            icons["Legacy.clearsavedgame"] = IconSet.GetIcon("Journal.ClearSavedGame");
            icons["Legacy.cockpitbreached"] = IconSet.GetIcon("Journal.CockpitBreached");
            icons["Legacy.collectcargo"] = IconSet.GetIcon("Journal.CollectCargo");
            icons["Legacy.comet"] = IconSet.GetIcon("GalMap.cometaryBody");
            icons["Legacy.commitcrime"] = IconSet.GetIcon("Journal.CommitCrime");
            icons["Legacy.commodities"] = IconSet.GetIcon("Journal.EDDCommodityPrices");
            icons["Legacy.communitygoal"] = IconSet.GetIcon("Journal.CommunityGoal");
            icons["Legacy.communitygoaldiscard"] = IconSet.GetIcon("Journal.CommunityGoalDiscard");
            icons["Legacy.communitygoaljoin"] = IconSet.GetIcon("Journal.CommunityGoalJoin");
            icons["Legacy.communitygoalreward"] = IconSet.GetIcon("Journal.CommunityGoalReward");
            icons["Legacy.coriolis"] = IconSet.GetIcon("Controls.Modules.ShowOnCoriolis");
            icons["Legacy.crewassign"] = IconSet.GetIcon("Journal.CrewAssign");
            icons["Legacy.crewfire"] = IconSet.GetIcon("Journal.CrewFire");
            icons["Legacy.crewhire"] = IconSet.GetIcon("Journal.CrewHire");
            icons["Legacy.crewmemberjoins"] = IconSet.GetIcon("Journal.CrewMemberJoins");
            icons["Legacy.crewmemberquits"] = IconSet.GetIcon("Journal.CrewMemberQuits");
            icons["Legacy.damage"] = IconSet.GetIcon("Journal.HullDamage");
            icons["Legacy.datalinkscan"] = IconSet.GetIcon("Journal.DatalinkScan");
            icons["Legacy.datalinkvoucher"] = IconSet.GetIcon("Journal.DatalinkVoucher");
            icons["Legacy.datascanned"] = IconSet.GetIcon("Journal.DataScanned");
            icons["Legacy.deleteDuplicateFSDJumpEntriesToolStripMenuItem"] = IconSet.GetIcon("Controls.Main.Admin.DeleteDupFSDJumps");
            icons["Legacy.dockfighter"] = IconSet.GetIcon("Journal.DockFighter");
            icons["Legacy.dockingcancelled"] = IconSet.GetIcon("Journal.DockingCancelled");
            icons["Legacy.dockingdenied"] = IconSet.GetIcon("Journal.DockingDenied");
            icons["Legacy.dockinggranted"] = IconSet.GetIcon("Journal.DockingGranted");
            icons["Legacy.dockingrequest"] = IconSet.GetIcon("Journal.DockingRequested");
            icons["Legacy.dockingtimeout"] = IconSet.GetIcon("Journal.DockingTimeout");
            icons["Legacy.docksrv"] = IconSet.GetIcon("Journal.DockSRV");
            icons["Legacy.dropdownFilterDate"] = IconSet.GetIcon("Controls.Map3D.FilterDate");
            icons["Legacy.dropdownMapNames"] = IconSet.GetIcon("Controls.Map3D.MapNames");
            icons["Legacy.dustbinshorter"] = IconSet.GetIcon("Controls.UCContainer.Remove");
            icons["Legacy.eDDiscoveryChatDiscordToolStripMenuItem"] = IconSet.GetIcon("Controls.Main.Help.DiscordChat");
            icons["Legacy.edsm16"] = IconSet.GetIcon("Controls.Main.Toolbar.SyncEDSM");
            icons["Legacy.edsm24"] = IconSet.GetIcon("Panels.EDSM");
            icons["Legacy.ego"] = IconSet.GetIcon("Controls.Main.Admin.SendUnsyncedEGO");
            icons["Legacy.ejectcargo"] = IconSet.GetIcon("Journal.EjectCargo");
            icons["Legacy.enableColoursToolStripMenuItem"] = IconSet.GetIcon("Controls.Map3D.Stars.DisplayColours");
            icons["Legacy.engineerapply"] = IconSet.GetIcon("Journal.EngineerApply");
            icons["Legacy.engineercraft"] = IconSet.GetIcon("Journal.EngineerCraft");
            icons["Legacy.engineerprogress"] = IconSet.GetIcon("Journal.EngineerProgress");
            icons["Legacy.escapeinterdiction"] = IconSet.GetIcon("Journal.EscapeInterdiction");
            icons["Legacy.estval"] = IconSet.GetIcon("Panels.EstimatedValues");
            icons["Legacy.event"] = IconSet.GetIcon("Journal.Unknown");
            icons["Legacy.excel"] = IconSet.GetIcon("Controls.JournalGrid.ExportToExcel");
            icons["Legacy.exitToolStripMenuItem"] = IconSet.GetIcon("Controls.Main.Tools.Exit");
            icons["Legacy.expedition"] = IconSet.GetIcon("Panels.Expedition");
            icons["Legacy.eye"] = IconSet.GetIcon("Controls.Map3D.Navigation.LookAtSystem");
            icons["Legacy.factionkillbond"] = IconSet.GetIcon("Journal.FactionKillBond");
            icons["Legacy.fetchremotemodule"] = IconSet.GetIcon("Journal.FetchRemoteModule");
            icons["Legacy.fighter"] = IconSet.GetIcon("Journal.VehicleSwitch_Fighter");
            icons["Legacy.fileheader"] = IconSet.GetIcon("Journal.Fileheader");
            icons["Legacy.floppy"] = IconSet.GetIcon("Controls.Map3D.Recorder.Save");
            icons["Legacy.friends"] = IconSet.GetIcon("Journal.Friends");
            icons["Legacy.frontierForumThreadToolStripMenuItem"] = IconSet.GetIcon("Controls.Main.Help.FrontierForumThread");
            icons["Legacy.fuelscoop"] = IconSet.GetIcon("Journal.FuelScoop");
            icons["Legacy.galaxy_gray"] = IconSet.GetIcon("Controls.Main.Toolbar.Open2DMap");
            icons["Legacy.genericevent"] = IconSet.GetIcon("Journal.Unknown");
            icons["Legacy.grid"] = IconSet.GetIcon("Panels.Grid");
            icons["Legacy.heatdamage"] = IconSet.GetIcon("Journal.HeatDamage");
            icons["Legacy.howToRunInSafeModeToResetVariousParametersToolStripMenuItem"] = IconSet.GetIcon("Controls.Main.Help.SafeModeHelp");
            icons["Legacy.hyperspace"] = IconSet.GetIcon("Journal.FSDJump");
            icons["Legacy.interdicted"] = IconSet.GetIcon("Journal.Interdicted");
            icons["Legacy.interdiction"] = IconSet.GetIcon("Journal.Interdiction");
            icons["Legacy.jetconeboost"] = IconSet.GetIcon("Journal.JetConeBoost");
            icons["Legacy.jetconedamage"] = IconSet.GetIcon("Journal.JetConeDamage");
            icons["Legacy.joinacrew"] = IconSet.GetIcon("Journal.JoinACrew");
            icons["Legacy.journal"] = IconSet.GetIcon("Panels.Journal");
            icons["Legacy.kickcrewmember"] = IconSet.GetIcon("Journal.KickCrewMember");
            icons["Legacy.launchfighter"] = IconSet.GetIcon("Journal.LaunchFighter");
            icons["Legacy.launchsrv"] = IconSet.GetIcon("Journal.LaunchSRV");
            icons["Legacy.ledger"] = IconSet.GetIcon("Panels.Ledger");
            icons["Legacy.liftoff"] = IconSet.GetIcon("Journal.Liftoff");
            icons["Legacy.loadgame"] = IconSet.GetIcon("Journal.LoadGame");
            icons["Legacy.loadout"] = IconSet.GetIcon("Journal.Loadout");
            icons["Legacy.location"] = IconSet.GetIcon("Journal.Location");
            icons["Legacy.map"] = IconSet.GetIcon("Panels.Map");
            icons["Legacy.manageaddons"] = IconSet.GetIcon("Controls.Main.Addons.ManageAddOns");
            icons["Legacy.marketbuy"] = IconSet.GetIcon("Journal.MarketBuy");
            icons["Legacy.marketdata"] = IconSet.GetIcon("Panels.MarketData");
            icons["Legacy.marketsell"] = IconSet.GetIcon("Journal.MarketSell");
            icons["Legacy.material"] = IconSet.GetIcon("Panels.Materials");
            icons["Legacy.materialcollected"] = IconSet.GetIcon("Journal.MaterialCollected");
            icons["Legacy.materialdiscarded"] = IconSet.GetIcon("Journal.MaterialDiscarded");
            icons["Legacy.materialdiscovered"] = IconSet.GetIcon("Journal.MaterialDiscovered");
            icons["Legacy.materialmarkerorangefilled"] = IconSet.GetIcon("Controls.Scan.Bodies.Material");
            icons["Legacy.materialrare"] = IconSet.GetIcon("Controls.Scan.ShowRareMaterials");
            icons["Legacy.materials"] = IconSet.GetIcon("Journal.Materials");
            icons["Legacy.materiamoreindicator"] = IconSet.GetIcon("Controls.Scan.Bodies.MaterialMore");
            icons["Legacy.metal_rich"] = IconSet.GetIcon("Planets.Metal_rich_body");
            icons["Legacy.miningrefined"] = IconSet.GetIcon("Journal.MiningRefined");
            icons["Legacy.missionabandoned"] = IconSet.GetIcon("Journal.MissionAbandoned");
            icons["Legacy.missionaccepted"] = IconSet.GetIcon("Journal.MissionAccepted");
            icons["Legacy.missioncompleted"] = IconSet.GetIcon("Journal.MissionCompleted");
            icons["Legacy.missionfailed"] = IconSet.GetIcon("Journal.MissionFailed");
            icons["Legacy.missionredirected"] = IconSet.GetIcon("Journal.MissionRedirected");
            icons["Legacy.modulebuy"] = IconSet.GetIcon("Journal.ModuleBuy");
            icons["Legacy.moduleretrieve"] = IconSet.GetIcon("Journal.ModuleRetrieve");
            icons["Legacy.modulesell"] = IconSet.GetIcon("Journal.ModuleSell");
            icons["Legacy.modulestore"] = IconSet.GetIcon("Journal.ModuleStore");
            icons["Legacy.moduleswap"] = IconSet.GetIcon("Journal.ModuleSwap");
            icons["Legacy.money"] = IconSet.GetIcon("Controls.Scan.Bodies.HighValue");
            icons["Legacy.mothership"] = IconSet.GetIcon("Journal.VehicleSwitch_Mothership");
            icons["Legacy.music"] = IconSet.GetIcon("Journal.Music");
            icons["Legacy.navbeacon"] = IconSet.GetIcon("Journal.NavBeaconScan");
            icons["Legacy.nebula"] = IconSet.GetIcon("GalMap.nebula");
            icons["Legacy.newRegionBookmarkToolStripMenuItem"] = IconSet.GetIcon("Controls.Map3D.Bookmarks.AddRegionBookmark");
            icons["Legacy.newcommander"] = IconSet.GetIcon("Journal.NewCommander");
            icons["Legacy.no_entry"] = IconSet.GetIcon("GalMap.restrictedSectors");
            icons["Legacy.notes"] = IconSet.GetIcon("Panels.NotePanel");
            icons["Legacy.panels"] = IconSet.GetIcon("Controls.Main.Toolbar.Popouts");
            icons["Legacy.passengers"] = IconSet.GetIcon("Journal.Passengers");
            icons["Legacy.pauseblue"] = IconSet.GetIcon("Controls.Map3D.Recorder.PauseRecord");
            icons["Legacy.payfines"] = IconSet.GetIcon("Journal.PayFines");
            icons["Legacy.planet landing"] = IconSet.GetIcon("Controls.Scan.Bodies.Landable");
            icons["Legacy.plot"] = IconSet.GetIcon("Panels.Plot");
            icons["Legacy.pointofinterest"] = IconSet.GetIcon("GalMap.beacon");
            icons["Legacy.popout1"] = IconSet.GetIcon("Controls.Main.Tools.Popouts.Menu");
            icons["Legacy.powerplaycollect"] = IconSet.GetIcon("Journal.PowerplayCollect");
            icons["Legacy.powerplaydefect"] = IconSet.GetIcon("Journal.PowerplayDefect");
            icons["Legacy.powerplaydeliver"] = IconSet.GetIcon("Journal.PowerplayDeliver");
            icons["Legacy.powerplayfasttrack"] = IconSet.GetIcon("Journal.PowerplayFastTrack");
            icons["Legacy.powerplayjoin"] = IconSet.GetIcon("Journal.PowerplayJoin");
            icons["Legacy.powerplayleave"] = IconSet.GetIcon("Journal.PowerplayLeave");
            icons["Legacy.powerplaysalary"] = IconSet.GetIcon("Journal.PowerplaySalary");
            icons["Legacy.powerplayvote"] = IconSet.GetIcon("Journal.PowerplayVote");
            icons["Legacy.powerplayvoucher"] = IconSet.GetIcon("Journal.PowerplayVoucher");
            icons["Legacy.progress"] = IconSet.GetIcon("Journal.Progress");
            icons["Legacy.promotion"] = IconSet.GetIcon("Journal.Promotion");
            icons["Legacy.pulsar"] = IconSet.GetIcon("GalMap.pulsar");
            icons["Legacy.pvpkill"] = IconSet.GetIcon("Journal.PVPKill");
            icons["Legacy.quitacrew"] = IconSet.GetIcon("Journal.QuitACrew");
            icons["Legacy.rank"] = IconSet.GetIcon("Journal.Rank");
            icons["Legacy.rebootrepair"] = IconSet.GetIcon("Journal.RebootRepair");
            icons["Legacy.receivetext"] = IconSet.GetIcon("Journal.ReceiveText");
            icons["Legacy.refresh_blue18"] = IconSet.GetIcon("Controls.Main.Toolbar.Refresh");
            icons["Legacy.refuel"] = IconSet.GetIcon("Journal.RefuelPartial");
            icons["Legacy.refuelall"] = IconSet.GetIcon("Journal.RefuelAll");
            icons["Legacy.repair"] = IconSet.GetIcon("Journal.Repair");
            icons["Legacy.repairall"] = IconSet.GetIcon("Journal.RepairAll");
            icons["Legacy.repairdrones"] = IconSet.GetIcon("Journal.RepairDrone");
            icons["Legacy.reportIssueIdeasToolStripMenuItem"] = IconSet.GetIcon("Controls.Main.Help.ReportIssue");
            icons["Legacy.ressurect"] = IconSet.GetIcon("Journal.Resurrect");
            icons["Legacy.restockfighter"] = IconSet.GetIcon("Journal.RestockVehicle_Fighter");
            icons["Legacy.restocksrv"] = IconSet.GetIcon("Journal.RestockVehicle_SRV");
            icons["Legacy.route"] = IconSet.GetIcon("Panels.Route");
            icons["Legacy.routetracker"] = IconSet.GetIcon("Panels.RouteTracker");
            icons["Legacy.save"] = IconSet.GetIcon("Controls.Map2D.Save");
            icons["Legacy.scan"] = IconSet.GetIcon("Journal.Scan");
            icons["Legacy.scangrid"] = IconSet.GetIcon("Panels.ScanGrid");
            icons["Legacy.scanned"] = IconSet.GetIcon("Journal.Scanned");
            icons["Legacy.scientificresearch"] = IconSet.GetIcon("Journal.ScientificResearch");
            icons["Legacy.screenshot"] = IconSet.GetIcon("Journal.Screenshot");
            icons["Legacy.searchrescue"] = IconSet.GetIcon("Journal.SearchAndRescue");
            icons["Legacy.selectedmarker"] = IconSet.GetIcon("Controls.Map3D.Markers.Selected");
            icons["Legacy.selfdestruct"] = IconSet.GetIcon("Journal.SelfDestruct");
            icons["Legacy.selldrones"] = IconSet.GetIcon("Journal.SellDrones");
            icons["Legacy.sellexplorationdata"] = IconSet.GetIcon("Journal.SellExplorationData");
            icons["Legacy.sellshiponrebuy"] = IconSet.GetIcon("Journal.SellShipOnRebuy");
            icons["Legacy.sendtext"] = IconSet.GetIcon("Journal.SendText");
            icons["Legacy.settings"] = IconSet.GetIcon("Controls.Main.Tools.Settings");
            icons["Legacy.setusershipname"] = IconSet.GetIcon("Journal.SetUserShipName");
            icons["Legacy.shields"] = IconSet.GetIcon("Journal.ShieldState");
            icons["Legacy.shieldsdown"] = IconSet.GetIcon("Journal.ShieldState_ShieldsDown");
            icons["Legacy.shieldsup"] = IconSet.GetIcon("Journal.ShieldState_ShieldsUp");
            icons["Legacy.shipyardnew"] = IconSet.GetIcon("Journal.ShipyardNew");
            icons["Legacy.shipyardsell"] = IconSet.GetIcon("Journal.ShipyardSell");
            icons["Legacy.shipyardswap"] = IconSet.GetIcon("Journal.ShipyardSwap");
            icons["Legacy.shipyardtransfer"] = IconSet.GetIcon("Journal.ShipyardTransfer");
            icons["Legacy.shoppinglist"] = IconSet.GetIcon("Panels.ShoppingList");
            icons["Legacy.show2DMapsToolStripMenuItem"] = IconSet.GetIcon("Controls.Main.Tools.Open2DMap");
            icons["Legacy.show3DMapsToolStripMenuItem"] = IconSet.GetIcon("Controls.Main.Tools.Open3DMap");
            icons["Legacy.showAllInTaskBarToolStripMenuItem"] = IconSet.GetIcon("Controls.Main.Tools.Popouts.ShowAllInTaskbar");
            icons["Legacy.showBookmarksToolStripMenuItem"] = IconSet.GetIcon("Controls.Map3D.Bookmarks.ShowBookmarks");
            icons["Legacy.showNamesToolStripMenuItem"] = IconSet.GetIcon("Controls.Map3D.Stars.ShowNames");
            icons["Legacy.showNoteMarksToolStripMenuItem"] = IconSet.GetIcon("Controls.Map3D.Bookmarks.ShowNotemarks");
            icons["Legacy.showStarstoolStripMenuItem"] = IconSet.GetIcon("Controls.Map3D.Filter.ShowAllStars");
            icons["Legacy.showStationsToolStripMenuItem"] = IconSet.GetIcon("Controls.Map3D.Filter.ShowPopSystems");
            icons["Legacy.spanel"] = IconSet.GetIcon("Panels.Spanel");
            icons["Legacy.starcluster"] = IconSet.GetIcon("GalMap.starCluster");
            icons["Legacy.starlist"] = IconSet.GetIcon("Controls.StarList.History");
            icons["Legacy.starsystem"] = IconSet.GetIcon("Panels.SystemInformation");
            icons["Legacy.startflag"] = IconSet.GetIcon("Controls.TravelGrid.FlagStart");
            icons["Legacy.startjump"] = IconSet.GetIcon("Journal.StartJump");
            icons["Legacy.stats"] = IconSet.GetIcon("Panels.Statistics");
            icons["Legacy.stopflag"] = IconSet.GetIcon("Controls.TravelGrid.FlagStop");
            icons["Legacy.supercruiseenter"] = IconSet.GetIcon("Journal.SupercruiseEntry");
            icons["Legacy.supercruiseexit"] = IconSet.GetIcon("Journal.SupercruiseExit");
            icons["Legacy.synthesis"] = IconSet.GetIcon("Journal.Synthesis");
            icons["Legacy.tilegrid"] = IconSet.GetIcon("Controls.UCContainer.Tile");
            icons["Legacy.toolStripButtonAutoForward"] = IconSet.GetIcon("Controls.Map3D.Navigation.GoForwardOnJump");
            icons["Legacy.toolStripButtonCoords"] = IconSet.GetIcon("Controls.Map3D.Grid.Coords");
            icons["Legacy.toolStripButtonDelete"] = IconSet.GetIcon("Controls.Expedition.Delete");
            icons["Legacy.toolStripButtonEliteMovement"] = IconSet.GetIcon("Controls.Map3D.EliteMovement");
            icons["Legacy.toolStripButtonExport"] = IconSet.GetIcon("Controls.Expedition.Export");
            icons["Legacy.toolStripButtonFineGrid"] = IconSet.GetIcon("Controls.Map3D.Grid.FineGrid");
            icons["Legacy.toolStripButtonGoBackward"] = IconSet.GetIcon("Controls.Map3D.Navigation.GoBackward");
            icons["Legacy.toolStripButtonGoForward"] = IconSet.GetIcon("Controls.Map3D.Navigation.GoForward");
            icons["Legacy.toolStripButtonGrid"] = IconSet.GetIcon("Controls.Map3D.Grid.Grid");
            icons["Legacy.toolStripButtonHelp"] = IconSet.GetIcon("Controls.Map3D.Help");
            icons["Legacy.toolStripButtonImportFile"] = IconSet.GetIcon("Controls.Expedition.ImportFile");
            icons["Legacy.toolStripButtonLastKnownPosition"] = IconSet.GetIcon("Controls.Map3D.Navigation.LastKnownPosition");
            icons["Legacy.toolStripButtonLoad"] = IconSet.GetIcon("Controls.Exploration.Load");
            icons["Legacy.toolStripButtonMap.Image"] = IconSet.GetIcon("Controls.Trilateration.ShowOnMap");
            icons["Legacy.toolStripButtonNew"] = IconSet.GetIcon("Controls.Exploration.ImportFile");
            icons["Legacy.toolStripButtonNew.Image"] = IconSet.GetIcon("Controls.Trilateration.StartNew");
            icons["Legacy.toolStripButtonPerspective"] = IconSet.GetIcon("Controls.Map3D.Perspective");
            icons["Legacy.toolStripButtonRemoveAll.Image"] = IconSet.GetIcon("Controls.Trilateration.RemoveAll");
            icons["Legacy.toolStripButtonRemoveUnused.Image"] = IconSet.GetIcon("Controls.Trilateration.RemoveUnused");
            icons["Legacy.toolStripButtonSave"] = IconSet.GetIcon("Controls.Exploration.Save");
            icons["Legacy.toolStripButtonSubmitDistances"] = IconSet.GetIcon("Controls.Expedition.Save");
            icons["Legacy.toolStripButtonSubmitDistances.Image"] = IconSet.GetIcon("Controls.Trilateration.SubmitDistances");
            icons["Legacy.toolStripButtonTarget"] = IconSet.GetIcon("Controls.Map3D.Navigation.GoToTarget");
            icons["Legacy.toolStripButtonZoomIn"] = IconSet.GetIcon("Controls.Map2D.ZoomIn");
            icons["Legacy.toolStripButtonZoomOut"] = IconSet.GetIcon("Controls.Map2D.ZoomOut");
            icons["Legacy.toolStripButtonZoomtoFit"] = IconSet.GetIcon("Controls.Map2D.ZoomToFit");
            icons["Legacy.toolStripDropDownButtonBookmarks"] = IconSet.GetIcon("Controls.Map3D.Bookmarks.Menu");
            icons["Legacy.toolStripDropDownButtonFilterStars"] = IconSet.GetIcon("Controls.Map3D.Filter.Menu");
            icons["Legacy.toolStripDropDownButtonGalObjects"] = IconSet.GetIcon("Controls.Map3D.GalObjects");
            icons["Legacy.toolStripDropDownButtonNameStars"] = IconSet.GetIcon("Controls.Map3D.Stars.Menu");
            icons["Legacy.touchdown"] = IconSet.GetIcon("Journal.Touchdown");
            icons["Legacy.travelgrid"] = IconSet.GetIcon("Controls.TravelGrid.History");
            icons["Legacy.triangulation"] = IconSet.GetIcon("Panels.Trilateration");
            icons["Legacy.trippanel"] = IconSet.GetIcon("Panels.Trippanel");
            icons["Legacy.tsbImportSphere"] = IconSet.GetIcon("Controls.Exploration.ImportSphere");
            icons["Legacy.turnOffAllTransparencyToolStripMenuItem"] = IconSet.GetIcon("Controls.Main.Tools.Popouts.DisableTransparency");
            icons["Legacy.uss"] = IconSet.GetIcon("Journal.USSDrop");
            icons["Legacy.warning"] = IconSet.GetIcon("Controls.Main.Admin.ResetHistory");
            icons["Legacy.wingadd"] = IconSet.GetIcon("Journal.WingAdd");
            icons["Legacy.wingjoin"] = IconSet.GetIcon("Journal.WingJoin");
            icons["Legacy.wingleave"] = IconSet.GetIcon("Journal.WingLeave");
            icons["Legacy.panelLogo"] = IconSet.GetIcon("Legacy.eddiscovery_logo");
            icons["Legacy.panel_eddiscovery"] = IconSet.GetIcon("Legacy.eddiscovery_logo");
#endif        
        }

        public static void ResetIcons()
        {
            icons = defaultIcons.ToArray().ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.InvariantCultureIgnoreCase);
            InitLegacyIcons();
        }

        public static void LoadIconsFromDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.EnumerateFiles(path, "*.png", SearchOption.AllDirectories))
                {
                    string name = file.Substring(path.Length + 1).Replace('/', '.').Replace('\\', '.');
                    Image img = null;

                    try
                    {
                        img = Image.FromFile(file);
                        img.Tag = name;
                    }
                    catch
                    {
                        // Ignore any bad images
                        continue;
                    }

                    icons[name] = img;
                }
            }
        }

        public static void LoadIconsFromZipFile(string path)
        {
            if (File.Exists(path))
            {
                using (var zipfile = ZipFile.Open(path, ZipArchiveMode.Read))
                {
                    foreach (var entry in zipfile.Entries)
                    {
                        if (entry.FullName.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string name = entry.FullName.Substring(0, entry.FullName.Length - 4).Replace('/', '.').Replace('\\', '.');
                            Image img = null;

                            try
                            {
                                using (var zipstrm = entry.Open())
                                {
                                    var memstrm = new MemoryStream(); // Image will own this
                                    zipstrm.CopyTo(memstrm);
                                    img = Image.FromStream(memstrm);
                                    img.Tag = name;
                                }
                            }
                            catch
                            {
                                // Ignore any bad images
                                continue;
                            }

                            icons[name] = img;
                        }
                    }
                }
            }
        }

        public static Image GetIcon(string name)
        {
            if (icons == null)      // seen designer barfing over this
                return null;

            if (!name.Contains("."))
            {
                name = "Legacy." + name;
            }

            System.Diagnostics.Debug.WriteLine("ICON " + name);

            if (icons.ContainsKey(name))            // written this way so you can debug step it.
                return icons[name];
            else if (defaultIcons.ContainsKey(name))
                return defaultIcons[name];
            else
            {
                System.Diagnostics.Debug.WriteLine("**************************** ************************" + Environment.NewLine + " Missing Icon " + name);
                return defaultIcons["Legacy.star"];
            }
        }
    }
}
