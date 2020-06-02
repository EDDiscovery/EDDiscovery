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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Icons
{
    public static class Controls
    {
        #region EDDiscovery.EDDiscoveryForm
        public static Image Main_Addons_ConfigureAddOnActions { get { return IconSet.GetIcon("Controls.Main.Addons.ConfigureAddOnActions"); } }
        public static Image Main_Addons_EditLastActionPack { get { return IconSet.GetIcon("Controls.Main.Addons.EditLastActionPack"); } }
        public static Image Main_Addons_ManageAddOns { get { return IconSet.GetIcon("Controls.Main.Addons.ManageAddOns"); } }
        public static Image Main_Addons_StopCurrentAction { get { return IconSet.GetIcon("Controls.Main.Addons.StopCurrentAction"); } }
        public static Image Main_Admin_ClearEDSMIDs { get { return IconSet.GetIcon("Controls.Main.Admin.ClearEDSMIDs"); } }
        public static Image Main_Admin_DeleteDupFSDJumps { get { return IconSet.GetIcon("Controls.Main.Admin.DeleteDupFSDJumps"); } }
        public static Image Main_Admin_EDDBSystemsSync { get { return IconSet.GetIcon("Controls.Main.Admin.EDDBSystemsSync"); } }
        public static Image Main_Admin_EDSMSystemsSync { get { return IconSet.GetIcon("Controls.Main.Admin.EDSMSystemsSync"); } }
        public static Image Main_Admin_ExportVisitedStars { get { return IconSet.GetIcon("Controls.Main.Admin.ExportVisitedStars"); } }
        public static Image Main_Admin_ReadNetLogs { get { return IconSet.GetIcon("Controls.Main.Admin.ReadNetLogs"); } }
        public static Image Main_Admin_RescanJournals { get { return IconSet.GetIcon("Controls.Main.Admin.RescanJournals"); } }
        public static Image Main_Admin_ResetHistory { get { return IconSet.GetIcon("Controls.Main.Admin.ResetHistory"); } }
        public static Image Main_Admin_SendUnsyncedEDDN { get { return IconSet.GetIcon("Controls.Main.Admin.SendUnsyncedEDDN"); } }
        public static Image Main_Admin_SendUnsyncedEGO { get { return IconSet.GetIcon("Controls.Main.Admin.SendUnsyncedEGO"); } }
        public static Image SendInara { get { return IconSet.GetIcon("Controls.Main.Admin.SendInara"); } }
        public static Image Main_Admin_ShowLogFiles { get { return IconSet.GetIcon("Controls.Main.Admin.ShowLogFiles"); } }
        public static Image Main_Help_About { get { return IconSet.GetIcon("Controls.Main.Help.About"); } }
        public static Image Main_Help_CheckForNewRelease { get { return IconSet.GetIcon("Controls.Main.Help.CheckForNewRelease"); } }
        public static Image Main_Help_DiscordChat { get { return IconSet.GetIcon("Controls.Main.Help.DiscordChat"); } }
        public static Image Main_Help_FrontierForumThread { get { return IconSet.GetIcon("Controls.Main.Help.FrontierForumThread"); } }
        public static Image Main_Help_Github { get { return IconSet.GetIcon("Controls.Main.Help.Github"); } }
        public static Image Main_Help_Help { get { return IconSet.GetIcon("Controls.Main.Help.Help"); } }
        public static Image Main_Help_ReportIssue { get { return IconSet.GetIcon("Controls.Main.Help.ReportIssue"); } }
        public static Image Main_Help_SafeModeHelp { get { return IconSet.GetIcon("Controls.Main.Help.SafeModeHelp"); } }
        public static Image Main_Toolbar_EditAddons { get { return IconSet.GetIcon("Controls.Main.Toolbar.EditAddons"); } }
        public static Image Main_Toolbar_ManageAddOns { get { return IconSet.GetIcon("Controls.Main.Toolbar.ManageAddOns"); } }
        public static Image Main_Toolbar_Open2DMap { get { return IconSet.GetIcon("Controls.Main.Toolbar.Open2DMap"); } }
        public static Image Main_Toolbar_Open3DMap { get { return IconSet.GetIcon("Controls.Main.Toolbar.Open3DMap"); } }
        public static Image Main_Toolbar_Popouts { get { return IconSet.GetIcon("Controls.Main.Toolbar.Popouts"); } }
        public static Image Main_Toolbar_Refresh { get { return IconSet.GetIcon("Controls.Main.Toolbar.Refresh"); } }
        public static Image Main_Toolbar_SyncEDSM { get { return IconSet.GetIcon("Controls.Main.Toolbar.SyncEDSM"); } }
        public static Image Main_Tools_Exit { get { return IconSet.GetIcon("Controls.Main.Tools.Exit"); } }
        public static Image Main_Tools_Open2DMap { get { return IconSet.GetIcon("Controls.Main.Tools.Open2DMap"); } }
        public static Image Main_Tools_Open3DMap { get { return IconSet.GetIcon("Controls.Main.Tools.Open3DMap"); } }
        public static Image Main_Tools_Popouts_DisableTransparency { get { return IconSet.GetIcon("Controls.Main.Tools.Popouts.DisableTransparency"); } }
        public static Image Main_Tools_Popouts_Menu { get { return IconSet.GetIcon("Controls.Main.Tools.Popouts.Menu"); } }
        public static Image Main_Tools_Popouts_ShowAllInTaskbar { get { return IconSet.GetIcon("Controls.Main.Tools.Popouts.ShowAllInTaskbar"); } }
        public static Image Main_Tools_Settings { get { return IconSet.GetIcon("Controls.Main.Tools.Settings"); } }
        #endregion
        #region EDDiscovery.Form2DMap
        public static Image Map2D_Save { get { return IconSet.GetIcon("Controls.Map2D.Save"); } }
        public static Image Map2D_ShowAllStars { get { return IconSet.GetIcon("Controls.Map2D.ShowAllStars"); } }
        public static Image Map2D_ZoomIn { get { return IconSet.GetIcon("Controls.Map2D.ZoomIn"); } }
        public static Image Map2D_ZoomOut { get { return IconSet.GetIcon("Controls.Map2D.ZoomOut"); } }
        public static Image Map2D_ZoomToFit { get { return IconSet.GetIcon("Controls.Map2D.ZoomToFit"); } }
        #endregion
        #region EDDiscovery.FormMap
        public static Image Map3D_Bookmarks_AddRegionBookmark { get { return IconSet.GetIcon("Controls.Map3D.Bookmarks.AddRegionBookmark"); } }
        public static Image Map3D_Bookmarks_Menu { get { return IconSet.GetIcon("Controls.Map3D.Bookmarks.Menu"); } }
        public static Image Map3D_Bookmarks_ShowBookmarks { get { return IconSet.GetIcon("Controls.Map3D.Bookmarks.ShowBookmarks"); } }
        public static Image Map3D_Bookmarks_ShowNotemarks { get { return IconSet.GetIcon("Controls.Map3D.Bookmarks.ShowNotemarks"); } }
        public static Image Map3D_Bookmarks_Noted { get { return IconSet.GetIcon("Controls.Map3D.Bookmarks.Noted"); } }
        public static Image Map3D_Bookmarks_Region { get { return IconSet.GetIcon("Controls.Map3D.Bookmarks.Region"); } }
        public static Image Map3D_Bookmarks_Star { get { return IconSet.GetIcon("Controls.Map3D.Bookmarks.Star"); } }
        public static Image Map3d_Bookmarks_StarWithPlanets { get { return IconSet.GetIcon("Controls.Map3D.Bookmarks.StarWithPlanets"); } }
        public static Image Map3D_Bookmarks_Target { get { return IconSet.GetIcon("Controls.Map3D.Bookmarks.Target"); } }
        public static Image Map3D_EliteMovement { get { return IconSet.GetIcon("Controls.Map3D.EliteMovement"); } }
        public static Image Map3D_Filter_Menu { get { return IconSet.GetIcon("Controls.Map3D.Filter.Menu"); } }
        public static Image Map3D_Filter_ShowAllStars { get { return IconSet.GetIcon("Controls.Map3D.Filter.ShowAllStars"); } }
        public static Image Map3D_Filter_ShowPopSystems { get { return IconSet.GetIcon("Controls.Map3D.Filter.ShowPopSystems"); } }
        public static Image Map3D_FilterDate { get { return IconSet.GetIcon("Controls.Map3D.FilterDate"); } }
        public static Image Map3D_GalObjects { get { return IconSet.GetIcon("Controls.Map3D.GalObjects"); } }
        public static Image Map3D_Grid_Coords { get { return IconSet.GetIcon("Controls.Map3D.Grid.Coords"); } }
        public static Image Map3D_Grid_FineGrid { get { return IconSet.GetIcon("Controls.Map3D.Grid.FineGrid"); } }
        public static Image Map3D_Grid_Grid { get { return IconSet.GetIcon("Controls.Map3D.Grid.Grid"); } }
        public static Image Map3D_Help { get { return IconSet.GetIcon("Controls.Map3D.Help"); } }
        public static Image Map3D_MapNames { get { return IconSet.GetIcon("Controls.Map3D.MapNames"); } }
        public static Image Map3D_Markers_Selected { get { return IconSet.GetIcon("Controls.Map3D.Markers.Selected"); } }
        public static Image Map3D_Navigation_CenterOnSystem { get { return IconSet.GetIcon("Controls.Map3D.Navigation.CenterOnSystem"); } }
        public static Image Map3D_Navigation_GoBackward { get { return IconSet.GetIcon("Controls.Map3D.Navigation.GoBackward"); } }
        public static Image Map3D_Navigation_GoForward { get { return IconSet.GetIcon("Controls.Map3D.Navigation.GoForward"); } }
        public static Image Map3D_Navigation_GoForwardOnJump { get { return IconSet.GetIcon("Controls.Map3D.Navigation.GoForwardOnJump"); } }
        public static Image Map3D_Navigation_GoToHistorySelection { get { return IconSet.GetIcon("Controls.Map3D.Navigation.GoToHistorySelection"); } }
        public static Image Map3D_Navigation_GoToHomeSystem { get { return IconSet.GetIcon("Controls.Map3D.Navigation.GoToHomeSystem"); } }
        public static Image Map3D_Navigation_GoToTarget { get { return IconSet.GetIcon("Controls.Map3D.Navigation.GoToTarget"); } }
        public static Image Map3D_Navigation_LastKnownPosition { get { return IconSet.GetIcon("Controls.Map3D.Navigation.LastKnownPosition"); } }
        public static Image Map3D_Navigation_LookAtSystem { get { return IconSet.GetIcon("Controls.Map3D.Navigation.LookAtSystem"); } }
        public static Image Map3D_OrangeDot { get { return IconSet.GetIcon("Controls.Map3D.OrangeDot"); } }
        public static Image Map3D_Perspective { get { return IconSet.GetIcon("Controls.Map3D.Perspective"); } }
        public static Image Map3D_Recorder_Clear { get { return IconSet.GetIcon("Controls.Map3D.Recorder.Clear"); } }
        public static Image Map3D_Recorder_Load { get { return IconSet.GetIcon("Controls.Map3D.Recorder.Load"); } }
        public static Image Map3D_Recorder_Menu { get { return IconSet.GetIcon("Controls.Map3D.Recorder.Menu"); } }
        public static Image Map3D_Recorder_NewRecordStep { get { return IconSet.GetIcon("Controls.Map3D.Recorder.NewRecordStep"); } }
        public static Image Map3D_Recorder_PausePlay { get { return IconSet.GetIcon("Controls.Map3D.Recorder.PausePlay"); } }
        public static Image Map3D_Recorder_PauseRecord { get { return IconSet.GetIcon("Controls.Map3D.Recorder.PauseRecord"); } }
        public static Image Map3D_Recorder_Pause { get { return IconSet.GetIcon("Controls.Map3D.Recorder.Pause"); } }
        public static Image Map3D_Recorder_Play { get { return IconSet.GetIcon("Controls.Map3D.Recorder.Play"); } }
        public static Image Map3D_Recorder_Record { get { return IconSet.GetIcon("Controls.Map3D.Recorder.Record"); } }
        public static Image Map3D_Recorder_RecordStep { get { return IconSet.GetIcon("Controls.Map3D.Recorder.RecordStep"); } }
        public static Image Map3D_Recorder_Save { get { return IconSet.GetIcon("Controls.Map3D.Recorder.Save"); } }
        public static Image Map3D_Recorder_StopRecord { get { return IconSet.GetIcon("Controls.Map3D.Recorder.StopRecord"); } }
        public static Image Map3D_Recorder_StopPlay { get { return IconSet.GetIcon("Controls.Map3D.Recorder.StopPlay"); } }
        public static Image Map3D_Stars_Menu { get { return IconSet.GetIcon("Controls.Map3D.Stars.Menu"); } }
        public static Image Map3D_Stars_ShowDiscs { get { return IconSet.GetIcon("Controls.Map3D.Stars.ShowDiscs"); } }
        public static Image Map3D_Stars_ShowNames { get { return IconSet.GetIcon("Controls.Map3D.Stars.ShowNames"); } }
        public static Image Map3D_Stars_DisplayColours { get { return IconSet.GetIcon("Controls.Map3D.Stars.DisplayColours"); } }
        public static Image Map3D_Travel_DrawLines { get { return IconSet.GetIcon("Controls.Map3D.Travel.DrawLines"); } }
        public static Image Map3D_Travel_DrawStars { get { return IconSet.GetIcon("Controls.Map3D.Travel.DrawStars"); } }
        public static Image Map3D_Travel_Menu { get { return IconSet.GetIcon("Controls.Map3D.Travel.Menu"); } }
        public static Image Map3D_Travel_WhiteStars { get { return IconSet.GetIcon("Controls.Map3D.Travel.WhiteStars"); } }
        public static Image Map3D_YellowDot { get { return IconSet.GetIcon("Controls.Map3D.YellowDot"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlExpedition
        public static Image Expedition_Delete { get { return IconSet.GetIcon("Controls.Expedition.Delete"); } }
        public static Image Expedition_Export { get { return IconSet.GetIcon("Controls.Expedition.Export"); } }
        public static Image Expedition_ImportFile { get { return IconSet.GetIcon("Controls.Expedition.ImportFile"); } }
        public static Image Expedition_ImportRoute { get { return IconSet.GetIcon("Controls.Expedition.ImportRoute"); } }
        public static Image Expedition_New { get { return IconSet.GetIcon("Controls.Expedition.New"); } }
        public static Image Expedition_Save { get { return IconSet.GetIcon("Controls.Expedition.Save"); } }
        public static Image Expedition_ShowOnMap { get { return IconSet.GetIcon("Controls.Expedition.ShowOnMap"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlExploration
        public static Image Exploration_Delete { get { return IconSet.GetIcon("Controls.Exploration.Delete"); } }
        public static Image Exploration_Export { get { return IconSet.GetIcon("Controls.Exploration.Export"); } }
        public static Image Exploration_ImportFile { get { return IconSet.GetIcon("Controls.Exploration.ImportFile"); } }
        public static Image Exploration_ImportSphere { get { return IconSet.GetIcon("Controls.Exploration.ImportSphere"); } }
        public static Image Exploration_Load { get { return IconSet.GetIcon("Controls.Exploration.Load"); } }
        public static Image Exploration_New { get { return IconSet.GetIcon("Controls.Exploration.New"); } }
        public static Image Exploration_Save { get { return IconSet.GetIcon("Controls.Exploration.Save"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlJournalGrid
        public static Image JournalGrid_ExportToExcel { get { return IconSet.GetIcon("Controls.JournalGrid.ExportToExcel"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlModules
        public static Image Modules_ShowOnCoriolis { get { return IconSet.GetIcon("Controls.Modules.ShowOnCoriolis"); } }
        public static Image Modules_EDShipYard { get { return IconSet.GetIcon("Controls.Modules.EDShipYard"); } }
        public static Image Modules_Configure { get { return IconSet.GetIcon("Controls.Modules.Configure"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlRoute
        public static Image Route_ExportToExcel { get { return IconSet.GetIcon("Controls.Route.ExportToExcel"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlScan
        public static Image Scan_Bodies_Barycentre { get { return IconSet.GetIcon("Controls.Scan.Bodies.Barycentre"); } }
        public static Image Scan_Bodies_Belt { get { return IconSet.GetIcon("Controls.Scan.Bodies.Belt"); } }
        public static Image Scan_Bodies_Landable { get { return IconSet.GetIcon("Controls.Scan.Bodies.Landable"); } }
        public static Image Scan_Bodies_HighValue { get { return IconSet.GetIcon("Controls.Scan.Bodies.HighValue"); } }
        public static Image Scan_Bodies_Terraformable { get { return IconSet.GetIcon("Controls.Scan.Bodies.Terraformable"); } }
        public static Image Scan_Bodies_Volcanism { get { return IconSet.GetIcon("Controls.Scan.Bodies.Volcanism"); } }
        public static Image Scan_Bodies_Signals { get { return IconSet.GetIcon("Controls.Scan.Bodies.Signals"); } }
        public static Image Scan_Bodies_Mapped { get { return IconSet.GetIcon("Controls.Scan.Bodies.Mapped"); } }
        public static Image Scan_Bodies_RingOnly { get { return IconSet.GetIcon("Controls.Scan.Bodies.RingOnly"); } }
        public static Image Scan_Bodies_RingGap { get { return IconSet.GetIcon("Controls.Scan.Bodies.RingGap"); } }
        public static Image Scan_Bodies_MaterialMore { get { return IconSet.GetIcon("Controls.Scan.Bodies.MaterialMore"); } }
        public static Image Scan_Bodies_Material { get { return IconSet.GetIcon("Controls.Scan.Bodies.Material"); } }
        public static Image Scan_DisplaySystemAlways { get { return IconSet.GetIcon("Controls.Scan.DisplaySystemAlways"); } }
        public static Image Scan_ExportToExcel { get { return IconSet.GetIcon("Controls.Scan.ExportToExcel"); } }
        public static Image Scan_FetchEDSMBodies { get { return IconSet.GetIcon("Controls.Scan.FetchEDSMBodies"); } }
        public static Image Scan_ShowAllMaterials { get { return IconSet.GetIcon("Controls.Scan.ShowAllMaterials"); } }
        public static Image Scan_HideFullMaterials { get { return IconSet.GetIcon("Controls.Scan.HideFullMaterials"); } }
        public static Image Scan_ShowMoons { get { return IconSet.GetIcon("Controls.Scan.ShowMoons"); } }
        public static Image Scan_ShowOverlays { get { return IconSet.GetIcon("Controls.Scan.ShowOverlays"); } }
        public static Image Scan_ShowRareMaterials { get { return IconSet.GetIcon("Controls.Scan.ShowRareMaterials"); } }
        public static Image Scan_SizeHuge { get { return IconSet.GetIcon("Controls.Scan.SizeHuge"); } }
        public static Image Scan_SizeVeryLarge { get { return IconSet.GetIcon("Controls.Scan.SizeVeryLarge"); } }
        public static Image Scan_SizeLarge { get { return IconSet.GetIcon("Controls.Scan.SizeLarge"); } }
        public static Image Scan_SizeMedium { get { return IconSet.GetIcon("Controls.Scan.SizeMedium"); } }
        public static Image Scan_SizeSmall { get { return IconSet.GetIcon("Controls.Scan.SizeSmall"); } }
        public static Image Scan_SizeTiny { get { return IconSet.GetIcon("Controls.Scan.SizeTiny"); } }
        public static Image Scan_SizeTinyTiny { get { return IconSet.GetIcon("Controls.Scan.SizeTinyTiny"); } }
        public static Image Scan_SizeMinuscule { get { return IconSet.GetIcon("Controls.Scan.SizeMinuscule"); } }
        public static Image Scan_Star { get { return IconSet.GetIcon("Controls.Scan.Star"); } }
        public static Image backbutton { get { return IconSet.GetIcon("Controls.Scan.backbutton"); } }
        public static Image DisplayFilters { get { return IconSet.GetIcon("Controls.Scan.DisplayFilters"); } }
        public static Image ShowAllG { get { return IconSet.GetIcon("Controls.Scan.ShowAllG"); } }
        public static Image ShowDistances { get { return IconSet.GetIcon("Controls.Scan.ShowDistances"); } }
        public static Image ShowHabZone { get { return IconSet.GetIcon("Controls.Scan.ShowHabZone"); } }
        public static Image ShowPlanetClasses { get { return IconSet.GetIcon("Controls.Scan.ShowPlanetClasses"); } }
        public static Image ShowStarClasses { get { return IconSet.GetIcon("Controls.Scan.ShowStarClasses"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlScanGrid
        public static Image ScanGrid_Belt { get { return IconSet.GetIcon("Controls.ScanGrid.Belt"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlSPanel
        public static Image SPanel_ResizeColumn { get { return IconSet.GetIcon("Controls.SPanel.ResizeColumn"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlStarList
        public static Image StarList_EDSM { get { return IconSet.GetIcon("Controls.StarList.EDSM"); } }
        public static Image StarList_BodyClass { get { return IconSet.GetIcon("Controls.StarList.BodyClasses"); } }
        public static Image StarList_Jumponium { get { return IconSet.GetIcon("Controls.StarList.Jumponium"); } }
        public static Image StarList_ExportToExcel { get { return IconSet.GetIcon("Controls.StarList.ExportToExcel"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlStatsTime
        public static Image StatsTime_Graph { get { return IconSet.GetIcon("Controls.StatsTime.Graph"); } }
        public static Image StatsTime_Planets { get { return IconSet.GetIcon("Controls.StatsTime.Planets"); } }
        public static Image StatsTime_Stars { get { return IconSet.GetIcon("Controls.StatsTime.Stars"); } }
        public static Image StatsTime_Text { get { return IconSet.GetIcon("Controls.StatsTime.Text"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlTravelGrid
        public static Image TravelGrid_ExportToExcel { get { return IconSet.GetIcon("Controls.TravelGrid.ExportToExcel"); } }
        public static Image TravelGrid_FlagStart { get { return IconSet.GetIcon("Controls.TravelGrid.FlagStart"); } }
        public static Image TravelGrid_FlagStop { get { return IconSet.GetIcon("Controls.TravelGrid.FlagStop"); } }
        public static Image TravelGrid_CursorToTop { get { return IconSet.GetIcon("Controls.TravelGrid.CursorToTop"); } }
        public static Image TravelGrid_CursorStill { get { return IconSet.GetIcon("Controls.TravelGrid.CursorStill"); } }
        public static Image TravelGrid_Outlines { get { return IconSet.GetIcon("Controls.TravelGrid.Outlines"); } }
        public static Image TravelGrid_FieldFilter { get { return IconSet.GetIcon("Controls.TravelGrid.FieldFilter"); } }
        public static Image TravelGrid_EventFilter { get { return IconSet.GetIcon("Controls.TravelGrid.EventFilter"); } }
        public static Image WordWrapOff { get { return IconSet.GetIcon("Controls.TravelGrid.WordWrapOff"); } }
        public static Image WordWrapOn { get { return IconSet.GetIcon("Controls.TravelGrid.WordWrapOn"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlTrilateration
        public static Image Trilateration_RemoveAll { get { return IconSet.GetIcon("Controls.Trilateration.RemoveAll"); } }
        public static Image Trilateration_RemoveUnused { get { return IconSet.GetIcon("Controls.Trilateration.RemoveUnused"); } }
        public static Image Trilateration_ShowOnMap { get { return IconSet.GetIcon("Controls.Trilateration.ShowOnMap"); } }
        public static Image Trilateration_StartNew { get { return IconSet.GetIcon("Controls.Trilateration.StartNew"); } }
        public static Image Trilateration_SubmitDistances { get { return IconSet.GetIcon("Controls.Trilateration.SubmitDistances"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlContainerGrid
        public static Image UCContainer_Panels { get { return IconSet.GetIcon("Controls.UCContainer.Panels"); } }
        public static Image UCContainer_Remove { get { return IconSet.GetIcon("Controls.UCContainer.Remove"); } }
        public static Image UCContainer_Tile { get { return IconSet.GetIcon("Controls.UCContainer.Tile"); } }
        #endregion
        #region EDDiscovery.UserControls.UserControlSysInfo
        public static Image firstdiscover { get { return IconSet.GetIcon("Controls.SysInfo.firstdiscover"); } }
        public static Image notfirstdiscover { get { return IconSet.GetIcon("Controls.SysInfo.notfirstdiscover"); } }
        #endregion
        #region TabStrip
        public static Image TabStrip_Popout { get { return IconSet.GetIcon("Controls.TabStrip.Popout"); } }
        #endregion
        #region Selector
        public static Image Selector_Background { get { return IconSet.GetIcon("Controls.Selector.Selector"); } }
        public static Image Selector_Background2 { get { return IconSet.GetIcon("Controls.Selector.Selector2"); } }
        public static Image Selector_AddTab { get { return IconSet.GetIcon("Controls.Selector.Addtab"); } }
        #endregion
        #region Bookmarks
        public static Image Bookmarks_Edit { get { return IconSet.GetIcon("Controls.Bookmarks.Edit"); } }
        public static Image Bookmarks_New { get { return IconSet.GetIcon("Controls.Bookmarks.New"); } }
        public static Image Bookmarks_Delete { get { return IconSet.GetIcon("Controls.Bookmarks.Delete"); } }
        public static Image ImportExcel { get { return IconSet.GetIcon("Controls.Bookmarks.ImportExcel"); } }
        #endregion
        #region EDDiscovery.UserControls.Search
        public static Image SearchStars { get { return IconSet.GetIcon("Controls.Search.SearchStars"); } }
        public static Image SearchMaterials { get { return IconSet.GetIcon("Controls.Search.SearchMaterials"); } }
        public static Image SearchScan { get { return IconSet.GetIcon("Controls.Search.Scan"); } }
        #endregion
        #region EDDiscovery.UserControls.CaptainsLog
        public static Image CaptainsLog_Delete { get { return IconSet.GetIcon("Controls.CaptainsLog.Delete"); } }
        public static Image CaptainsLog_New { get { return IconSet.GetIcon("Controls.CaptainsLog.New"); } }
        public static Image CaptainsLog_Tags { get { return IconSet.GetIcon("Controls.CaptainsLog.Tags"); } }
        public static Image CaptainsLog_Entries { get { return IconSet.GetIcon("Controls.CaptainsLog.Entries"); } }
        public static Image CaptainsLog_Diary { get { return IconSet.GetIcon("Controls.CaptainsLog.Diary"); } }
        #endregion
        #region Mat Commds
        public static Image matnozeros { get { return IconSet.GetIcon("Controls.MatCommds.matnozeros"); } }
        public static Image matshowzeros { get { return IconSet.GetIcon("Controls.MatCommds.matshowzeros"); } }
        #endregion
        #region General
        public static Image All { get { return IconSet.GetIcon("Controls.General.All"); } }
        public static Image None { get { return IconSet.GetIcon("Controls.General.None"); } }
        #endregion

    }
}
