using EDDiscovery.Icons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using System.Windows.Forms;
using ExtendedControls;

namespace EDDiscovery
{
    public class EDDIconSet : IEliteIconSet
    {
        public class IconGroup<T> : IReadOnlyDictionary<T, Image>
        {
            protected Dictionary<T, Image> icons;

            public IconGroup(string basedir, Func<string, Image> geticon)
            {
                Init(basedir, Enum.GetValues(typeof(T)).OfType<T>(), geticon);
            }

            protected void Init(string basedir, IEnumerable<T> keys, Func<string, Image> geticon)
            {
                icons = keys.ToDictionary(e => e, e => geticon(basedir + "." + e.ToString()));
            }

            public Image this[T key] => icons[key];
            public IEnumerable<T> Keys => icons.Keys;
            public IEnumerable<Image> Values => icons.Values;
            public int Count => icons.Count;
            public bool ContainsKey(T key) => icons.ContainsKey(key);
            public IEnumerator<KeyValuePair<T, Image>> GetEnumerator() => icons.GetEnumerator();
            public bool TryGetValue(T key, out Image value) => icons.TryGetValue(key, out value);
            IEnumerator IEnumerable.GetEnumerator() => icons.GetEnumerator();
        }

        public class IconReplacer
        {
            public string BaseName { get; private set; }
            protected Func<string, Image> GetImage { get; set; }

            public IconReplacer(IIconPackControl control, Func<string, Image> getimage)
            {
                BaseName = control.BaseName;
                GetImage = getimage;
            }

            public void ReplaceImage(Action<Image> setter, string name)
            {
                Image newimg = GetImage("Controls." + BaseName + "." + name);
                if (newimg != null)
                {
                    setter(newimg);
                }
            }
        }

        private static EDDIconSet _instance;
        private static Func<string, Image> getIcon;

        private EDDIconSet()
        {
            StarTypeIcons = new IconGroup<EDStar>("Stars", this.GetIcon);
            PlanetTypeIcons = new IconGroup<EDPlanet>("Planets", this.GetIcon);
            JournalTypeIcons = new IconGroup<JournalTypeEnum>("Journal", this.GetIcon);
            GalMapTypeIcons = new IconGroup<GalMapTypeEnum>("GalMap", this.GetIcon);
            PanelTypeIcons = new IconGroup<PanelInformation.PanelIDs>("Panels", this.GetIcon);
        }

        public static EDDIconSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EDDIconSet();
                    EliteConfigInstance.InstanceIconSet = _instance;
                }

                return _instance;
            }
            set
            {
                _instance = value;
                EliteConfigInstance.InstanceIconSet = value;
            }
        }

        public static void Init(Func<string, Image> geticon)
        {
            getIcon = geticon;
            EliteConfigInstance.InstanceIconSet = Instance;
        }

        public IReadOnlyDictionary<PanelInformation.PanelIDs, Image> PanelTypeIcons { get; private set; }
        public IReadOnlyDictionary<EDStar, Image> StarTypeIcons { get; private set; }
        public IReadOnlyDictionary<EDPlanet, Image> PlanetTypeIcons { get; private set; }
        public IReadOnlyDictionary<JournalTypeEnum, Image> JournalTypeIcons { get; private set; }
        public IReadOnlyDictionary<GalMapTypeEnum, Image> GalMapTypeIcons { get; private set; }

        public Image GetIcon(string name)
        {
            if (!name.Contains("."))
            {
                name = "Legacy." + name;
            }

            return getIcon(name);
        }

        public void ReplaceIcons(Control control)
        {
            if (control is IIconPackControl)
            {
                IIconPackControl ctrl = ((IIconPackControl)control);
                IconReplacer replacer = new IconReplacer(ctrl, GetIcon);
                ctrl.ReplaceImages(replacer.ReplaceImage);
            }

            if (control is TabStrip)
            {
                TabStrip ts = (TabStrip)control;
                ts.ImageList = PanelInformation.GetPanelImages();
            }

            if (control.HasChildren)
            {
                foreach (Control child in control.Controls)
                {
                    ReplaceIcons(child);
                }
            }
        }

        #region Icon References
        public static class Controls
        {
            public static class Expedition
            {
                public static Image Delete => Instance.GetIcon("Controls.Expedition.Delete");
                public static Image Export => Instance.GetIcon("Controls.Expedition.Export");
                public static Image ImportFile => Instance.GetIcon("Controls.Expedition.ImportFile");
                public static Image ImportRoute => Instance.GetIcon("Controls.Expedition.ImportRoute");
                public static Image New => Instance.GetIcon("Controls.Expedition.New");
                public static Image Save => Instance.GetIcon("Controls.Expedition.Save");
                public static Image ShowOnMap => Instance.GetIcon("Controls.Expedition.ShowOnMap");
            }
            public static class Exploration
            {
                public static Image Delete => Instance.GetIcon("Controls.Exploration.Delete");
                public static Image Export => Instance.GetIcon("Controls.Exploration.Export");
                public static Image ImportFile => Instance.GetIcon("Controls.Exploration.ImportFile");
                public static Image ImportSphere => Instance.GetIcon("Controls.Exploration.ImportSphere");
                public static Image Load => Instance.GetIcon("Controls.Exploration.Load");
                public static Image New => Instance.GetIcon("Controls.Exploration.New");
                public static Image Save => Instance.GetIcon("Controls.Exploration.Save");
            }
            public static class JournalGrid
            {
                public static Image ExportToExcel => Instance.GetIcon("Controls.JournalGrid.ExportToExcel");
                public static Image Journal => Instance.GetIcon("Controls.JournalGrid.Journal");
                public static Image Popout => Instance.GetIcon("Controls.JournalGrid.Popout");
            }
            public static class Main
            {
                public static Image Addons_ConfigureAddOnActions => Instance.GetIcon("Controls.Main.Addons.ConfigureAddOnActions");
                public static Image Addons_EditLastActionPack => Instance.GetIcon("Controls.Main.Addons.EditLastActionPack");
                public static Image Addons_ManageAddOns => Instance.GetIcon("Controls.Main.Addons.ManageAddOns");
                public static Image Addons_StopCurrentAction => Instance.GetIcon("Controls.Main.Addons.StopCurrentAction");
                public static Image Admin_ClearEDSMIDs => Instance.GetIcon("Controls.Main.Admin.ClearEDSMIDs");
                public static Image Admin_DeleteDupFSDJumps => Instance.GetIcon("Controls.Main.Admin.DeleteDupFSDJumps");
                public static Image Admin_EDDBSystemsSync => Instance.GetIcon("Controls.Main.Admin.EDDBSystemsSync");
                public static Image Admin_EDSMSystemsSync => Instance.GetIcon("Controls.Main.Admin.EDSMSystemsSync");
                public static Image Admin_ExportVisitedStars => Instance.GetIcon("Controls.Main.Admin.ExportVisitedStars");
                public static Image Admin_ReadNetLogs => Instance.GetIcon("Controls.Main.Admin.ReadNetLogs");
                public static Image Admin_RescanJournals => Instance.GetIcon("Controls.Main.Admin.RescanJournals");
                public static Image Admin_ResetHistory => Instance.GetIcon("Controls.Main.Admin.ResetHistory");
                public static Image Admin_SendUnsyncedEDDN => Instance.GetIcon("Controls.Main.Admin.SendUnsyncedEDDN");
                public static Image Admin_SendUnsynedEGO => Instance.GetIcon("Controls.Main.Admin.SendUnsynedEGO");
                public static Image Admin_ShowLogFiles => Instance.GetIcon("Controls.Main.Admin.ShowLogFiles");
                public static Image Help_About => Instance.GetIcon("Controls.Main.Help.About");
                public static Image Help_CheckForNewRelease => Instance.GetIcon("Controls.Main.Help.CheckForNewRelease");
                public static Image Help_DiscordChat => Instance.GetIcon("Controls.Main.Help.DiscordChat");
                public static Image Help_FrontierForumThread => Instance.GetIcon("Controls.Main.Help.FrontierForumThread");
                public static Image Help_Github => Instance.GetIcon("Controls.Main.Help.Github");
                public static Image Help_Help => Instance.GetIcon("Controls.Main.Help.Help");
                public static Image Help_ReportIssue => Instance.GetIcon("Controls.Main.Help.ReportIssue");
                public static Image Help_SafeModeHelp => Instance.GetIcon("Controls.Main.Help.SafeModeHelp");
                public static Image Toolbar_EditAddons => Instance.GetIcon("Controls.Main.Toolbar.EditAddons");
                public static Image Toolbar_ManageAddOns => Instance.GetIcon("Controls.Main.Toolbar.ManageAddOns");
                public static Image Toolbar_Open2DMap => Instance.GetIcon("Controls.Main.Toolbar.Open2DMap");
                public static Image Toolbar_Open3DMap => Instance.GetIcon("Controls.Main.Toolbar.Open3DMap");
                public static Image Toolbar_Popouts => Instance.GetIcon("Controls.Main.Toolbar.Popouts");
                public static Image Toolbar_Refresh => Instance.GetIcon("Controls.Main.Toolbar.Refresh");
                public static Image Toolbar_SyncEDSM => Instance.GetIcon("Controls.Main.Toolbar.SyncEDSM");
                public static Image Tools_Exit => Instance.GetIcon("Controls.Main.Tools.Exit");
                public static Image Tools_Open2DMap => Instance.GetIcon("Controls.Main.Tools.Open2DMap");
                public static Image Tools_Open3DMap => Instance.GetIcon("Controls.Main.Tools.Open3DMap");
                public static Image Tools_Popouts_DisableTransparency => Instance.GetIcon("Controls.Main.Tools.Popouts.DisableTransparency");
                public static Image Tools_Popouts_Menu => Instance.GetIcon("Controls.Main.Tools.Popouts.Menu");
                public static Image Tools_Popouts_ShowAllInTaskbar => Instance.GetIcon("Controls.Main.Tools.Popouts.ShowAllInTaskbar");
                public static Image Tools_Settings => Instance.GetIcon("Controls.Main.Tools.Settings");
            }
            public static class Map2D
            {
                public static Image Save => Instance.GetIcon("Controls.Map2D.Save");
                public static Image ShowAllStars => Instance.GetIcon("Controls.Map2D.ShowAllStars");
                public static Image ZoomIn => Instance.GetIcon("Controls.Map2D.ZoomIn");
                public static Image ZoomOut => Instance.GetIcon("Controls.Map2D.ZoomOut");
                public static Image ZoomToFit => Instance.GetIcon("Controls.Map2D.ZoomToFit");
            }
            public static class Map3D
            {
                public static Image Bookmarks_AddRegionBookmark => Instance.GetIcon("Controls.Map3D.Bookmarks.AddRegionBookmark");
                public static Image Bookmarks_Menu => Instance.GetIcon("Controls.Map3D.Bookmarks.Menu");
                public static Image Bookmarks_ShowBookmarks => Instance.GetIcon("Controls.Map3D.Bookmarks.ShowBookmarks");
                public static Image Bookmarks_ShowNotemarks => Instance.GetIcon("Controls.Map3D.Bookmarks.ShowNotemarks");
                public static Image Bookmarks_Noted => Instance.GetIcon("Controls.Map3D.Bookmarks.Noted");
                public static Image Bookmarks_Region => Instance.GetIcon("Controls.Map3D.Bookmarks.Region");
                public static Image Bookmarks_Star => Instance.GetIcon("Controls.Map3D.Bookmarks.Star");
                public static Image Bookmarks_Target => Instance.GetIcon("Controls.Map3D.Bookmarks.Target");
                public static Image EliteMovement => Instance.GetIcon("Controls.Map3D.EliteMovement");
                public static Image Filter_DisplayColours => Instance.GetIcon("Controls.Map3D.Filter.DisplayColours");
                public static Image Filter_Menu => Instance.GetIcon("Controls.Map3D.Filter.Menu");
                public static Image Filter_ShowAllStars => Instance.GetIcon("Controls.Map3D.Filter.ShowAllStars");
                public static Image Filter_ShowPopSystems => Instance.GetIcon("Controls.Map3D.Filter.ShowPopSystems");
                public static Image FilterDate => Instance.GetIcon("Controls.Map3D.FilterDate");
                public static Image GalObjects => Instance.GetIcon("Controls.Map3D.GalObjects");
                public static Image Grid_Coords => Instance.GetIcon("Controls.Map3D.Grid.Coords");
                public static Image Grid_FineGrid => Instance.GetIcon("Controls.Map3D.Grid.FineGrid");
                public static Image Grid_Grid => Instance.GetIcon("Controls.Map3D.Grid.Grid");
                public static Image Help => Instance.GetIcon("Controls.Map3D.Help");
                public static Image MapNames => Instance.GetIcon("Controls.Map3D.MapNames");
                public static Image Markers_Selected => Instance.GetIcon("Controls.Map3D.Markers.Selected");
                public static Image Navigation_CenterOnSystem => Instance.GetIcon("Controls.Map3D.Navigation.CenterOnSystem");
                public static Image Navigation_GoBackward => Instance.GetIcon("Controls.Map3D.Navigation.GoBackward");
                public static Image Navigation_GoForward => Instance.GetIcon("Controls.Map3D.Navigation.GoForward");
                public static Image Navigation_GoForwardOnJump => Instance.GetIcon("Controls.Map3D.Navigation.GoForwardOnJump");
                public static Image Navigation_GoToHistorySelection => Instance.GetIcon("Controls.Map3D.Navigation.GoToHistorySelection");
                public static Image Navigation_GoToHomeSystem => Instance.GetIcon("Controls.Map3D.Navigation.GoToHomeSystem");
                public static Image Navigation_GoToTarget => Instance.GetIcon("Controls.Map3D.Navigation.GoToTarget");
                public static Image Navigation_LastKnownPosition => Instance.GetIcon("Controls.Map3D.Navigation.LastKnownPosition");
                public static Image Navigation_LookAtSystem => Instance.GetIcon("Controls.Map3D.Navigation.LookAtSystem");
                public static Image OrangeDot => Instance.GetIcon("Controls.Map3D.OrangeDot");
                public static Image Perspective => Instance.GetIcon("Controls.Map3D.Perspective");
                public static Image Recorder_Clear => Instance.GetIcon("Controls.Map3D.Recorder.Clear");
                public static Image Recorder_Load => Instance.GetIcon("Controls.Map3D.Recorder.Load");
                public static Image Recorder_Menu => Instance.GetIcon("Controls.Map3D.Recorder.Menu");
                public static Image Recorder_NewRecordStep => Instance.GetIcon("Controls.Map3D.Recorder.NewRecordStep");
                public static Image Recorder_PausePlay => Instance.GetIcon("Controls.Map3D.Recorder.PausePlay");
                public static Image Recorder_PauseRecord => Instance.GetIcon("Controls.Map3D.Recorder.PauseRecord");
                public static Image Recorder_Pause => Instance.GetIcon("Controls.Map3D.Recorder.Pause");
                public static Image Recorder_Play => Instance.GetIcon("Controls.Map3D.Recorder.Play");
                public static Image Recorder_Record => Instance.GetIcon("Controls.Map3D.Recorder.Record");
                public static Image Recorder_RecordStep => Instance.GetIcon("Controls.Map3D.Recorder.RecordStep");
                public static Image Recorder_Save => Instance.GetIcon("Controls.Map3D.Recorder.Save");
                public static Image Recorder_StopRecord => Instance.GetIcon("Controls.Map3D.Recorder.StopRecord");
                public static Image Recorder_StopPlay => Instance.GetIcon("Controls.Map3D.Recorder.StopPlay");
                public static Image Stars_Menu => Instance.GetIcon("Controls.Map3D.Stars.Menu");
                public static Image Stars_ShowDiscs => Instance.GetIcon("Controls.Map3D.Stars.ShowDiscs");
                public static Image Stars_ShowNames => Instance.GetIcon("Controls.Map3D.Stars.ShowNames");
                public static Image StopNormalBlue => Instance.GetIcon("Controls.Map3D.StopNormalBlue");
                public static Image Travel_DrawLines => Instance.GetIcon("Controls.Map3D.Travel.DrawLines");
                public static Image Travel_DrawStars => Instance.GetIcon("Controls.Map3D.Travel.DrawStars");
                public static Image Travel_Menu => Instance.GetIcon("Controls.Map3D.Travel.Menu");
                public static Image Travel_WhiteStars => Instance.GetIcon("Controls.Map3D.Travel.WhiteStars");
                public static Image YellowDot => Instance.GetIcon("Controls.Map3D.YellowDot");
            }
            public static class Modules
            {
                public static Image ShowOnCoriolis => Instance.GetIcon("Controls.Modules.ShowOnCoriolis");
            }
            public static class Route
            {
                public static Image ExportToExcel => Instance.GetIcon("Controls.Route.ExportToExcel");
            }
            public static class Scan
            {
                public static Image Bodies_Barycentre => Instance.GetIcon("Controls.Scan.Bodies.Barycentre");
                public static Image Bodies_Belt => Instance.GetIcon("Controls.Scan.Bodies.Belt");
                public static Image Bodies_Landable => Instance.GetIcon("Controls.Scan.Bodies.Landable");
                public static Image Bodies_HighValue => Instance.GetIcon("Controls.Scan.Bodies.HighValue");
                public static Image Bodies_Terraformable => Instance.GetIcon("Controls.Scan.Bodies.Terraformable");
                public static Image Bodies_Volcanism => Instance.GetIcon("Controls.Scan.Bodies.Volcanism");
                public static Image Bodies_RingOnly => Instance.GetIcon("Controls.Scan.Bodies.RingOnly");
                public static Image Bodies_RingGap => Instance.GetIcon("Controls.Scan.Bodies.RingGap");
                public static Image Bodies_MaterialMore => Instance.GetIcon("Controls.Scan.Bodies.MaterialMore");
                public static Image Bodies_Material => Instance.GetIcon("Controls.Scan.Bodies.Material");
                public static Image ExportToExcel => Instance.GetIcon("Controls.Scan.ExportToExcel");
                public static Image FetchEDSMBodies => Instance.GetIcon("Controls.Scan.FetchEDSMBodies");
                public static Image ShowAllMaterials => Instance.GetIcon("Controls.Scan.ShowAllMaterials");
                public static Image ShowMoons => Instance.GetIcon("Controls.Scan.ShowMoons");
                public static Image ShowOverlays => Instance.GetIcon("Controls.Scan.ShowOverlays");
                public static Image ShowRareMaterials => Instance.GetIcon("Controls.Scan.ShowRareMaterials");
                public static Image SizeLarge => Instance.GetIcon("Controls.Scan.SizeLarge");
                public static Image SizeMedium => Instance.GetIcon("Controls.Scan.SizeMedium");
                public static Image SizeSmall => Instance.GetIcon("Controls.Scan.SizeSmall");
                public static Image SizeTiny => Instance.GetIcon("Controls.Scan.SizeTiny");
            }
            public static class StarList
            {
                public static Image EDSM => Instance.GetIcon("Controls.StarList.EDSM");
                public static Image ExportToExcel => Instance.GetIcon("Controls.StarList.ExportToExcel");
                public static Image History => Instance.GetIcon("Controls.StarList.History");
            }
            public static class StatsTime
            {
                public static Image Graph => Instance.GetIcon("Controls.StatsTime.Graph");
                public static Image Planets => Instance.GetIcon("Controls.StatsTime.Planets");
                public static Image Stars => Instance.GetIcon("Controls.StatsTime.Stars");
                public static Image Text => Instance.GetIcon("Controls.StatsTime.Text");
            }
            public static class TravelGrid
            {
                public static Image ExportToExcel => Instance.GetIcon("Controls.TravelGrid.ExportToExcel");
                public static Image History => Instance.GetIcon("Controls.TravelGrid.History");
                public static Image Popout => Instance.GetIcon("Controls.TravelGrid.Popout");
                public static Image FlagStart => Instance.GetIcon("Controls.TravelGrid.FlagStart");
                public static Image FlagStop => Instance.GetIcon("Controls.TravelGrid.FlagStop");
            }
            public static class Trilateration
            {
                public static Image RemoveAll => Instance.GetIcon("Controls.Trilateration.RemoveAll");
                public static Image RemoveUnused => Instance.GetIcon("Controls.Trilateration.RemoveUnused");
                public static Image ShowOnMap => Instance.GetIcon("Controls.Trilateration.ShowOnMap");
                public static Image StartNew => Instance.GetIcon("Controls.Trilateration.StartNew");
                public static Image SubmitDistances => Instance.GetIcon("Controls.Trilateration.SubmitDistances");
            }
            public static class UCContainer
            {
                public static Image Panels => Instance.GetIcon("Controls.UCContainer.Panels");
                public static Image Remove => Instance.GetIcon("Controls.UCContainer.Remove");
                public static Image Tile => Instance.GetIcon("Controls.UCContainer.Tile");
            }
        }
        public static class GalMap
        {
            public static Image beacon => Instance.GetIcon("GalMap.beacon");
            public static Image blackHole => Instance.GetIcon("GalMap.blackHole");
            public static Image cometaryBody => Instance.GetIcon("GalMap.cometaryBody");
            public static Image deepSpaceOutpost => Instance.GetIcon("GalMap.deepSpaceOutpost");
            public static Image EDSMUnknown => Instance.GetIcon("GalMap.EDSMUnknown");
            public static Image historicalLocation => Instance.GetIcon("GalMap.historicalLocation");
            public static Image jumponiumRichSystem => Instance.GetIcon("GalMap.jumponiumRichSystem");
            public static Image minorPOI => Instance.GetIcon("GalMap.minorPOI");
            public static Image mysteryPOI => Instance.GetIcon("GalMap.mysteryPOI");
            public static Image nebula => Instance.GetIcon("GalMap.nebula");
            public static Image planetaryNebula => Instance.GetIcon("GalMap.planetaryNebula");
            public static Image planetFeatures => Instance.GetIcon("GalMap.planetFeatures");
            public static Image pulsar => Instance.GetIcon("GalMap.pulsar");
            public static Image restrictedSectors => Instance.GetIcon("GalMap.restrictedSectors");
            public static Image starCluster => Instance.GetIcon("GalMap.starCluster");
            public static Image stellarRemnant => Instance.GetIcon("GalMap.stellarRemnant");
            public static Image surfacePOI => Instance.GetIcon("GalMap.surfacePOI");
        }
        public static class Journal
        {
            public static Image AfmuRepair => Instance.GetIcon("Journal.AfmuRepair");
            public static Image AfmuRepairs => Instance.GetIcon("Journal.AfmuRepairs");
            public static Image ApproachSettlement => Instance.GetIcon("Journal.ApproachSettlement");
            public static Image Bounty => Instance.GetIcon("Journal.Bounty");
            public static Image BuyAmmo => Instance.GetIcon("Journal.BuyAmmo");
            public static Image BuyDrones => Instance.GetIcon("Journal.BuyDrones");
            public static Image BuyExplorationData => Instance.GetIcon("Journal.BuyExplorationData");
            public static Image BuyTradeData => Instance.GetIcon("Journal.BuyTradeData");
            public static Image CapShipBond => Instance.GetIcon("Journal.CapShipBond");
            public static Image Cargo => Instance.GetIcon("Journal.Cargo");
            public static Image ChangeCrewRole => Instance.GetIcon("Journal.ChangeCrewRole");
            public static Image ClearSavedGame => Instance.GetIcon("Journal.ClearSavedGame");
            public static Image CockpitBreached => Instance.GetIcon("Journal.CockpitBreached");
            public static Image CollectCargo => Instance.GetIcon("Journal.CollectCargo");
            public static Image CommitCrime => Instance.GetIcon("Journal.CommitCrime");
            public static Image CommunityGoal => Instance.GetIcon("Journal.CommunityGoal");
            public static Image CommunityGoalDiscard => Instance.GetIcon("Journal.CommunityGoalDiscard");
            public static Image CommunityGoalJoin => Instance.GetIcon("Journal.CommunityGoalJoin");
            public static Image CommunityGoalReward => Instance.GetIcon("Journal.CommunityGoalReward");
            public static Image Continued => Instance.GetIcon("Journal.Continued");
            public static Image CrewAssign => Instance.GetIcon("Journal.CrewAssign");
            public static Image CrewFire => Instance.GetIcon("Journal.CrewFire");
            public static Image CrewHire => Instance.GetIcon("Journal.CrewHire");
            public static Image CrewLaunchFighter => Instance.GetIcon("Journal.CrewLaunchFighter");
            public static Image CrewMemberJoins => Instance.GetIcon("Journal.CrewMemberJoins");
            public static Image CrewMemberQuits => Instance.GetIcon("Journal.CrewMemberQuits");
            public static Image CrewMemberRoleChange => Instance.GetIcon("Journal.CrewMemberRoleChange");
            public static Image DatalinkScan => Instance.GetIcon("Journal.DatalinkScan");
            public static Image DatalinkVoucher => Instance.GetIcon("Journal.DatalinkVoucher");
            public static Image DataScanned => Instance.GetIcon("Journal.DataScanned");
            public static Image Died => Instance.GetIcon("Journal.Died");
            public static Image Docked => Instance.GetIcon("Journal.Docked");
            public static Image DockFighter => Instance.GetIcon("Journal.DockFighter");
            public static Image DockingCancelled => Instance.GetIcon("Journal.DockingCancelled");
            public static Image DockingDenied => Instance.GetIcon("Journal.DockingDenied");
            public static Image DockingGranted => Instance.GetIcon("Journal.DockingGranted");
            public static Image DockingRequested => Instance.GetIcon("Journal.DockingRequested");
            public static Image DockingTimeout => Instance.GetIcon("Journal.DockingTimeout");
            public static Image DockSRV => Instance.GetIcon("Journal.DockSRV");
            public static Image EDDCommodityPrices => Instance.GetIcon("Journal.EDDCommodityPrices");
            public static Image EDDItemSet => Instance.GetIcon("Journal.EDDItemSet");
            public static Image EjectCargo => Instance.GetIcon("Journal.EjectCargo");
            public static Image EndCrewSession => Instance.GetIcon("Journal.EndCrewSession");
            public static Image EngineerApply => Instance.GetIcon("Journal.EngineerApply");
            public static Image EngineerContribution => Instance.GetIcon("Journal.EngineerContribution");
            public static Image EngineerContribution_MatCommod => Instance.GetIcon("Journal.EngineerContribution_MatCommod");
            public static Image EngineerContribution_Unknown => Instance.GetIcon("Journal.EngineerContribution_Unknown");
            public static Image EngineerCraft => Instance.GetIcon("Journal.EngineerCraft");
            public static Image EngineerProgress => Instance.GetIcon("Journal.EngineerProgress");
            public static Image EscapeInterdiction => Instance.GetIcon("Journal.EscapeInterdiction");
            public static Image FactionKillBond => Instance.GetIcon("Journal.FactionKillBond");
            public static Image FetchRemoteModule => Instance.GetIcon("Journal.FetchRemoteModule");
            public static Image Fileheader => Instance.GetIcon("Journal.Fileheader");
            public static Image Friends => Instance.GetIcon("Journal.Friends");
            public static Image FSDJump => Instance.GetIcon("Journal.FSDJump");
            public static Image FuelScoop => Instance.GetIcon("Journal.FuelScoop");
            public static Image FuelScope => Instance.GetIcon("Journal.FuelScope");
            public static Image HeatDamage => Instance.GetIcon("Journal.HeatDamage");
            public static Image HeatWarning => Instance.GetIcon("Journal.HeatWarning");
            public static Image HullDamage => Instance.GetIcon("Journal.HullDamage");
            public static Image Interdicted => Instance.GetIcon("Journal.Interdicted");
            public static Image Interdiction => Instance.GetIcon("Journal.Interdiction");
            public static Image JetConeBoost => Instance.GetIcon("Journal.JetConeBoost");
            public static Image JetConeDamage => Instance.GetIcon("Journal.JetConeDamage");
            public static Image JoinACrew => Instance.GetIcon("Journal.JoinACrew");
            public static Image KickCrewMember => Instance.GetIcon("Journal.KickCrewMember");
            public static Image LaunchFighter => Instance.GetIcon("Journal.LaunchFighter");
            public static Image LaunchSRV => Instance.GetIcon("Journal.LaunchSRV");
            public static Image Liftoff => Instance.GetIcon("Journal.Liftoff");
            public static Image LoadGame => Instance.GetIcon("Journal.LoadGame");
            public static Image Loadout => Instance.GetIcon("Journal.Loadout");
            public static Image Location => Instance.GetIcon("Journal.Location");
            public static Image MarketBuy => Instance.GetIcon("Journal.MarketBuy");
            public static Image MarketSell => Instance.GetIcon("Journal.MarketSell");
            public static Image MassModuleStore => Instance.GetIcon("Journal.MassModuleStore");
            public static Image MaterialCollected => Instance.GetIcon("Journal.MaterialCollected");
            public static Image MaterialDiscarded => Instance.GetIcon("Journal.MaterialDiscarded");
            public static Image MaterialDiscovered => Instance.GetIcon("Journal.MaterialDiscovered");
            public static Image Materials => Instance.GetIcon("Journal.Materials");
            public static Image MiningRefined => Instance.GetIcon("Journal.MiningRefined");
            public static Image MissionAbandoned => Instance.GetIcon("Journal.MissionAbandoned");
            public static Image MissionAccepted => Instance.GetIcon("Journal.MissionAccepted");
            public static Image MissionCompleted => Instance.GetIcon("Journal.MissionCompleted");
            public static Image MissionFailed => Instance.GetIcon("Journal.MissionFailed");
            public static Image MissionRedirected => Instance.GetIcon("Journal.MissionRedirected");
            public static Image ModuleBuy => Instance.GetIcon("Journal.ModuleBuy");
            public static Image ModuleRetrieve => Instance.GetIcon("Journal.ModuleRetrieve");
            public static Image ModuleSell => Instance.GetIcon("Journal.ModuleSell");
            public static Image ModuleSellRemote => Instance.GetIcon("Journal.ModuleSellRemote");
            public static Image ModuleStore => Instance.GetIcon("Journal.ModuleStore");
            public static Image ModuleSwap => Instance.GetIcon("Journal.ModuleSwap");
            public static Image Music => Instance.GetIcon("Journal.Music");
            public static Image NavBeaconScan => Instance.GetIcon("Journal.NavBeaconScan");
            public static Image NewCommander => Instance.GetIcon("Journal.NewCommander");
            public static Image Passengers => Instance.GetIcon("Journal.Passengers");
            public static Image PayFines => Instance.GetIcon("Journal.PayFines");
            public static Image PayLegacyFines => Instance.GetIcon("Journal.PayLegacyFines");
            public static Image PowerplayCollect => Instance.GetIcon("Journal.PowerplayCollect");
            public static Image PowerplayDefect => Instance.GetIcon("Journal.PowerplayDefect");
            public static Image PowerplayDeliver => Instance.GetIcon("Journal.PowerplayDeliver");
            public static Image PowerplayFastTrack => Instance.GetIcon("Journal.PowerplayFastTrack");
            public static Image PowerplayJoin => Instance.GetIcon("Journal.PowerplayJoin");
            public static Image PowerplayLeave => Instance.GetIcon("Journal.PowerplayLeave");
            public static Image PowerplaySalary => Instance.GetIcon("Journal.PowerplaySalary");
            public static Image PowerplayVote => Instance.GetIcon("Journal.PowerplayVote");
            public static Image PowerplayVoucher => Instance.GetIcon("Journal.PowerplayVoucher");
            public static Image Progress => Instance.GetIcon("Journal.Progress");
            public static Image Promotion => Instance.GetIcon("Journal.Promotion");
            public static Image PVPKill => Instance.GetIcon("Journal.PVPKill");
            public static Image QuitACrew => Instance.GetIcon("Journal.QuitACrew");
            public static Image Rank => Instance.GetIcon("Journal.Rank");
            public static Image RebootRepair => Instance.GetIcon("Journal.RebootRepair");
            public static Image ReceiveText => Instance.GetIcon("Journal.ReceiveText");
            public static Image RedeemVoucher => Instance.GetIcon("Journal.RedeemVoucher");
            public static Image RefuelAll => Instance.GetIcon("Journal.RefuelAll");
            public static Image RefuelPartial => Instance.GetIcon("Journal.RefuelPartial");
            public static Image Repair => Instance.GetIcon("Journal.Repair");
            public static Image RepairAll => Instance.GetIcon("Journal.RepairAll");
            public static Image RepairDrone => Instance.GetIcon("Journal.RepairDrone");
            public static Image RestockVehicle => Instance.GetIcon("Journal.RestockVehicle");
            public static Image RestockVehicle_Fighter => Instance.GetIcon("Journal.RestockVehicle_Fighter");
            public static Image RestockVehicle_SRV => Instance.GetIcon("Journal.RestockVehicle_SRV");
            public static Image Resurrect => Instance.GetIcon("Journal.Resurrect");
            public static Image Scan => Instance.GetIcon("Journal.Scan");
            public static Image Scanned => Instance.GetIcon("Journal.Scanned");
            public static Image ScientificResearch => Instance.GetIcon("Journal.ScientificResearch");
            public static Image Screenshot => Instance.GetIcon("Journal.Screenshot");
            public static Image SearchAndRescue => Instance.GetIcon("Journal.SearchAndRescue");
            public static Image SelfDestruct => Instance.GetIcon("Journal.SelfDestruct");
            public static Image SellDrones => Instance.GetIcon("Journal.SellDrones");
            public static Image SellExplorationData => Instance.GetIcon("Journal.SellExplorationData");
            public static Image SellShipOnRebuy => Instance.GetIcon("Journal.SellShipOnRebuy");
            public static Image SendText => Instance.GetIcon("Journal.SendText");
            public static Image SetUserShipName => Instance.GetIcon("Journal.SetUserShipName");
            public static Image ShieldState => Instance.GetIcon("Journal.ShieldState");
            public static Image ShieldState_ShieldsDown => Instance.GetIcon("Journal.ShieldState_ShieldsDown");
            public static Image ShieldState_ShieldsUp => Instance.GetIcon("Journal.ShieldState_ShieldsUp");
            public static Image ShipyardBuy => Instance.GetIcon("Journal.ShipyardBuy");
            public static Image ShipyardNew => Instance.GetIcon("Journal.ShipyardNew");
            public static Image ShipyardSell => Instance.GetIcon("Journal.ShipyardSell");
            public static Image ShipyardSwap => Instance.GetIcon("Journal.ShipyardSwap");
            public static Image ShipyardTransfer => Instance.GetIcon("Journal.ShipyardTransfer");
            public static Image StartJump => Instance.GetIcon("Journal.StartJump");
            public static Image SupercruiseEntry => Instance.GetIcon("Journal.SupercruiseEntry");
            public static Image SupercruiseExit => Instance.GetIcon("Journal.SupercruiseExit");
            public static Image Synthesis => Instance.GetIcon("Journal.Synthesis");
            public static Image SystemsShutdown => Instance.GetIcon("Journal.SystemsShutdown");
            public static Image Touchdown => Instance.GetIcon("Journal.Touchdown");
            public static Image Undocked => Instance.GetIcon("Journal.Undocked");
            public static Image Unknown => Instance.GetIcon("Journal.Unknown");
            public static Image USSDrop => Instance.GetIcon("Journal.USSDrop");
            public static Image VehicleSwitch => Instance.GetIcon("Journal.VehicleSwitch");
            public static Image VehicleSwitch_Fighter => Instance.GetIcon("Journal.VehicleSwitch_Fighter");
            public static Image VehicleSwitch_Mothership => Instance.GetIcon("Journal.VehicleSwitch_Mothership");
            public static Image WingAdd => Instance.GetIcon("Journal.WingAdd");
            public static Image WingInvite => Instance.GetIcon("Journal.WingInvite");
            public static Image WingJoin => Instance.GetIcon("Journal.WingJoin");
            public static Image WingLeave => Instance.GetIcon("Journal.WingLeave");
        }
        public static class Panels
        {
            public static Image Commodities => Instance.GetIcon("Panels.Commodities");
            public static Image EDSM => Instance.GetIcon("Panels.EDSM");
            public static Image Engineering => Instance.GetIcon("Panels.Engineering");
            public static Image EstimatedValues => Instance.GetIcon("Panels.EstimatedValues");
            public static Image Expedition => Instance.GetIcon("Panels.Expedition");
            public static Image Exploration => Instance.GetIcon("Panels.Exploration");
            public static Image Grid => Instance.GetIcon("Panels.Grid");
            public static Image Journal => Instance.GetIcon("Panels.Journal");
            public static Image Ledger => Instance.GetIcon("Panels.Ledger");
            public static Image Log => Instance.GetIcon("Panels.Log");
            public static Image MarketData => Instance.GetIcon("Panels.MarketData");
            public static Image Materials => Instance.GetIcon("Panels.Materials");
            public static Image Missions => Instance.GetIcon("Panels.Missions");
            public static Image Modules => Instance.GetIcon("Panels.Modules");
            public static Image NotePanel => Instance.GetIcon("Panels.NotePanel");
            public static Image Route => Instance.GetIcon("Panels.Route");
            public static Image RouteTracker => Instance.GetIcon("Panels.RouteTracker");
            public static Image Scan => Instance.GetIcon("Panels.Scan");
            public static Image ScreenShot => Instance.GetIcon("Panels.ScreenShot");
            public static Image Settings => Instance.GetIcon("Panels.Settings");
            public static Image ShoppingList => Instance.GetIcon("Panels.ShoppingList");
            public static Image Spanel => Instance.GetIcon("Panels.Spanel");
            public static Image StarDistance => Instance.GetIcon("Panels.StarDistance");
            public static Image StarList => Instance.GetIcon("Panels.StarList");
            public static Image Statistics => Instance.GetIcon("Panels.Statistics");
            public static Image Synthesis => Instance.GetIcon("Panels.Synthesis");
            public static Image SystemInformation => Instance.GetIcon("Panels.SystemInformation");
            public static Image TravelGrid => Instance.GetIcon("Panels.TravelGrid");
            public static Image Trilateration => Instance.GetIcon("Panels.Trilateration");
            public static Image Trippanel => Instance.GetIcon("Panels.Trippanel");
        }
        public static class Planets
        {
            public static Image Ammonia_world => Instance.GetIcon("Planets.Ammonia_world");
            public static Image Earthlike_body => Instance.GetIcon("Planets.Earthlike_body");
            public static Image Gas_giant_with_ammonia_based_life => Instance.GetIcon("Planets.Gas_giant_with_ammonia_based_life");
            public static Image Gas_giant_with_water_based_life => Instance.GetIcon("Planets.Gas_giant_with_water_based_life");
            public static Image Helium_gas_giant => Instance.GetIcon("Planets.Helium_gas_giant");
            public static Image Helium_rich_gas_giant => Instance.GetIcon("Planets.Helium_rich_gas_giant");
            public static Image High_metal_content_body => Instance.GetIcon("Planets.High_metal_content_body");
            public static Image High_metal_content_body_250 => Instance.GetIcon("Planets.High_metal_content_body_250");
            public static Image High_metal_content_body_700 => Instance.GetIcon("Planets.High_metal_content_body_700");
            public static Image High_metal_content_body_hot_thick => Instance.GetIcon("Planets.High_metal_content_body_hot_thick");
            public static Image Icy_body => Instance.GetIcon("Planets.Icy_body");
            public static Image Metal_rich_body => Instance.GetIcon("Planets.Metal_rich_body");
            public static Image Rocky_body => Instance.GetIcon("Planets.Rocky_body");
            public static Image Rocky_ice_body => Instance.GetIcon("Planets.Rocky_ice_body");
            public static Image Sudarsky_class_III_gas_giant => Instance.GetIcon("Planets.Sudarsky_class_III_gas_giant");
            public static Image Sudarsky_class_II_gas_giant => Instance.GetIcon("Planets.Sudarsky_class_II_gas_giant");
            public static Image Sudarsky_class_IV_gas_giant => Instance.GetIcon("Planets.Sudarsky_class_IV_gas_giant");
            public static Image Sudarsky_class_I_gas_giant => Instance.GetIcon("Planets.Sudarsky_class_I_gas_giant");
            public static Image Sudarsky_class_V_gas_giant => Instance.GetIcon("Planets.Sudarsky_class_V_gas_giant");
            public static Image Unknown => Instance.GetIcon("Planets.Unknown");
            public static Image Water_giant => Instance.GetIcon("Planets.Water_giant");
            public static Image Water_giant_with_life => Instance.GetIcon("Planets.Water_giant_with_life");
            public static Image Water_world => Instance.GetIcon("Planets.Water_world");
        }
        public static class Stars
        {
            public static Image A => Instance.GetIcon("Stars.A");
            public static Image AeBe => Instance.GetIcon("Stars.AeBe");
            public static Image A_BlueWhiteSuperGiant => Instance.GetIcon("Stars.A_BlueWhiteSuperGiant");
            public static Image B => Instance.GetIcon("Stars.B");
            public static Image C => Instance.GetIcon("Stars.C");
            public static Image CHd => Instance.GetIcon("Stars.CHd");
            public static Image CJ => Instance.GetIcon("Stars.CJ");
            public static Image CN => Instance.GetIcon("Stars.CN");
            public static Image CS => Instance.GetIcon("Stars.CS");
            public static Image D => Instance.GetIcon("Stars.D");
            public static Image DA => Instance.GetIcon("Stars.DA");
            public static Image DAB => Instance.GetIcon("Stars.DAB");
            public static Image DAO => Instance.GetIcon("Stars.DAO");
            public static Image DAV => Instance.GetIcon("Stars.DAV");
            public static Image DAZ => Instance.GetIcon("Stars.DAZ");
            public static Image DB => Instance.GetIcon("Stars.DB");
            public static Image DBV => Instance.GetIcon("Stars.DBV");
            public static Image DBZ => Instance.GetIcon("Stars.DBZ");
            public static Image DC => Instance.GetIcon("Stars.DC");
            public static Image DCV => Instance.GetIcon("Stars.DCV");
            public static Image DO => Instance.GetIcon("Stars.DO");
            public static Image DOV => Instance.GetIcon("Stars.DOV");
            public static Image DQ => Instance.GetIcon("Stars.DQ");
            public static Image DX => Instance.GetIcon("Stars.DX");
            public static Image F => Instance.GetIcon("Stars.F");
            public static Image F_WhiteSuperGiant => Instance.GetIcon("Stars.F_WhiteSuperGiant");
            public static Image G => Instance.GetIcon("Stars.G");
            public static Image H => Instance.GetIcon("Stars.H");
            public static Image K => Instance.GetIcon("Stars.K");
            public static Image K_OrangeGiant => Instance.GetIcon("Stars.K_OrangeGiant");
            public static Image L => Instance.GetIcon("Stars.L");
            public static Image M => Instance.GetIcon("Stars.M");
            public static Image MS => Instance.GetIcon("Stars.MS");
            public static Image M_RedGiant => Instance.GetIcon("Stars.M_RedGiant");
            public static Image M_RedSuperGiant => Instance.GetIcon("Stars.M_RedSuperGiant");
            public static Image N => Instance.GetIcon("Stars.N");
            public static Image Nebula => Instance.GetIcon("Stars.Nebula");
            public static Image O => Instance.GetIcon("Stars.O");
            public static Image RoguePlanet => Instance.GetIcon("Stars.RoguePlanet");
            public static Image S => Instance.GetIcon("Stars.S");
            public static Image StellarRemnantNebula => Instance.GetIcon("Stars.StellarRemnantNebula");
            public static Image SuperMassiveBlackHole => Instance.GetIcon("Stars.SuperMassiveBlackHole");
            public static Image T => Instance.GetIcon("Stars.T");
            public static Image TTS => Instance.GetIcon("Stars.TTS");
            public static Image Unknown => Instance.GetIcon("Stars.Unknown");
            public static Image W => Instance.GetIcon("Stars.W");
            public static Image WC => Instance.GetIcon("Stars.WC");
            public static Image WN => Instance.GetIcon("Stars.WN");
            public static Image WNC => Instance.GetIcon("Stars.WNC");
            public static Image WO => Instance.GetIcon("Stars.WO");
            public static Image X => Instance.GetIcon("Stars.X");
            public static Image Y => Instance.GetIcon("Stars.Y");
        }
        #endregion
    }
}
