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
        public static Image Main_Addons_ConfigureAddOnActions { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Addons.ConfigureAddOnActions"); } }
        public static Image Main_Addons_EditLastActionPack { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Addons.EditLastActionPack"); } }
        public static Image Main_Addons_ManageAddOns { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Addons.ManageAddOns"); } }
        public static Image Main_Addons_StopCurrentAction { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Addons.StopCurrentAction"); } }
        public static Image Main_Admin_ClearEDSMIDs { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Admin.ClearEDSMIDs"); } }
        public static Image Main_Admin_DeleteDupFSDJumps { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Admin.DeleteDupFSDJumps"); } }
        public static Image Main_Admin_EDSMSystemsSync { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Admin.EDSMSystemsSync"); } }
        public static Image Main_Admin_ReadNetLogs { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Admin.ReadNetLogs"); } }
        public static Image Main_Admin_RescanJournals { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Admin.RescanJournals"); } }
        public static Image Main_Admin_ResetHistory { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Admin.ResetHistory"); } }
        public static Image Main_Admin_SendUnsyncedEDDN { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Admin.SendUnsyncedEDDN"); } }
        public static Image SendInara { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Admin.SendInara"); } }
        public static Image Main_Admin_ShowLogFiles { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Admin.ShowLogFiles"); } }
        public static Image Main_Help_About { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Help.About"); } }
        public static Image Main_Help_CheckForNewRelease { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Help.CheckForNewRelease"); } }
        public static Image Main_Help_DiscordChat { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Help.DiscordChat"); } }
        public static Image Main_Help_FrontierForumThread { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Help.FrontierForumThread"); } }
        public static Image Main_Help_Github { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Help.Github"); } }
        public static Image Main_Help_ReportIssue { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Help.ReportIssue"); } }
        public static Image Main_Help_SafeModeHelp { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Help.SafeModeHelp"); } }
        public static Image Main_Toolbar_EditAddons { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Toolbar.EditAddons"); } }
        public static Image Main_Toolbar_ManageAddOns { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Toolbar.ManageAddOns"); } }
        public static Image Main_Toolbar_Popouts { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Toolbar.Popouts"); } }
        public static Image Main_Toolbar_Refresh { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Toolbar.Refresh"); } }
        public static Image Main_Tools_Exit { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Tools.Exit"); } }
        public static Image Main_Tools_Open3DMap { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Tools.Open3DMap"); } }
        public static Image Main_Tools_Popouts_DisableTransparency { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Tools.Popouts.DisableTransparency"); } }
        public static Image Main_Tools_Popouts_Menu { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Tools.Popouts.Menu"); } }
        public static Image Main_Tools_Popouts_ShowAllInTaskbar { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Tools.Popouts.ShowAllInTaskbar"); } }
        public static Image Main_Tools_Settings { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Main.Tools.Settings"); } }
        #endregion
        public static Image Star { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Star"); } }
        public static Image ZoomIn { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ZoomIn"); } }
        public static Image ZoomOut { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ZoomOut"); } }
        public static Image ZoomToFit { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ZoomToFit"); } }
        #region EDDiscovery.FormMap
        public static Image Map3D_Bookmarks_AddRegionBookmark { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Bookmarks.AddRegionBookmark"); } }
        public static Image Map3D_Bookmarks_Menu { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Bookmarks.Menu"); } }
        public static Image Map3D_Bookmarks_ShowBookmarks { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Bookmarks.ShowBookmarks"); } }
        public static Image Map3D_Bookmarks_ShowNotemarks { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Bookmarks.ShowNotemarks"); } }
        public static Image Map3D_Bookmarks_Noted { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Bookmarks.Noted"); } }
        public static Image Map3D_Bookmarks_Region { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Bookmarks.Region"); } }
        public static Image Map3D_Bookmarks_Star { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Bookmarks.Star"); } }
        public static Image Map3d_Bookmarks_StarWithPlanets { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Bookmarks.StarWithPlanets"); } }
        public static Image Map3D_Bookmarks_Target { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Bookmarks.Target"); } }
        public static Image Map3D_EliteMovement { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.EliteMovement"); } }
        public static Image Map3D_Filter_Menu { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Filter.Menu"); } }
        public static Image Map3D_Filter_ShowAllStars { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Filter.ShowAllStars"); } }
        public static Image Map3D_FilterDate { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.FilterDate"); } }
        public static Image Map3D_GalObjects { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.GalObjects"); } }
        public static Image Map3D_Grid_Coords { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Grid.Coords"); } }
        public static Image Map3D_Grid_FineGrid { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Grid.FineGrid"); } }
        public static Image Map3D_Grid_Grid { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Grid.Grid"); } }
        public static Image Map3D_Help { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Help"); } }
        public static Image Map3D_MapNames { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.MapNames"); } }
        public static Image Map3D_Markers_Selected { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Markers.Selected"); } }
        public static Image Map3D_Navigation_CenterOnSystem { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Navigation.CenterOnSystem"); } }
        public static Image Map3D_Navigation_GoBackward { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Navigation.GoBackward"); } }
        public static Image Map3D_Navigation_GoForward { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Navigation.GoForward"); } }
        public static Image Map3D_Navigation_GoForwardOnJump { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Navigation.GoForwardOnJump"); } }
        public static Image Map3D_Navigation_GoToHistorySelection { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Navigation.GoToHistorySelection"); } }
        public static Image Map3D_Navigation_GoToHomeSystem { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Navigation.GoToHomeSystem"); } }
        public static Image Map3D_Navigation_GoToTarget { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Navigation.GoToTarget"); } }
        public static Image Map3D_Navigation_LastKnownPosition { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Navigation.LastKnownPosition"); } }
        public static Image Map3D_Navigation_LookAtSystem { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Navigation.LookAtSystem"); } }
        public static Image Map3D_OrangeDot { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.OrangeDot"); } }
        public static Image Map3D_Perspective { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Perspective"); } }
        public static Image Map3D_Recorder_Clear { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Recorder.Clear"); } }
        public static Image Map3D_Recorder_Load { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Recorder.Load"); } }
        public static Image Map3D_Recorder_Menu { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Recorder.Menu"); } }
        public static Image Map3D_Recorder_NewRecordStep { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Recorder.NewRecordStep"); } }
        public static Image Map3D_Recorder_PausePlay { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Recorder.PausePlay"); } }
        public static Image Map3D_Recorder_PauseRecord { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Recorder.PauseRecord"); } }
        public static Image Map3D_Recorder_Pause { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Recorder.Pause"); } }
        public static Image Map3D_Recorder_Play { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Recorder.Play"); } }
        public static Image Map3D_Recorder_Record { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Recorder.Record"); } }
        public static Image Map3D_Recorder_RecordStep { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Recorder.RecordStep"); } }
        public static Image Map3D_Recorder_Save { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Recorder.Save"); } }
        public static Image Map3D_Recorder_StopRecord { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Recorder.StopRecord"); } }
        public static Image Map3D_Recorder_StopPlay { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Recorder.StopPlay"); } }
        public static Image Map3D_Stars_Menu { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Stars.Menu"); } }
        public static Image Map3D_Stars_ShowDiscs { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Stars.ShowDiscs"); } }
        public static Image Map3D_Stars_ShowNames { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Stars.ShowNames"); } }
        public static Image Map3D_Stars_DisplayColours { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Stars.DisplayColours"); } }
        public static Image Map3D_Travel_DrawLines { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Travel.DrawLines"); } }
        public static Image Map3D_Travel_DrawStars { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Travel.DrawStars"); } }
        public static Image Map3D_Travel_Menu { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Travel.Menu"); } }
        public static Image Map3D_Travel_WhiteStars { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.Travel.WhiteStars"); } }
        public static Image Map3D_YellowDot { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D.YellowDot"); } }
        #endregion
        public static Image Export { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Export"); } }
        public static Image ImportFile { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ImportFile"); } }
        public static Image ImportSphere { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ImportSphere"); } }
        public static Image ImportRoute { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ImportRoute"); } }
        public static Image ImportNavRoute { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ImportNavRoute"); } }
        public static Image ClearRoute { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ClearRoute"); } }
        public static Image Save { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Save"); } }
        public static Image ShowOnMap { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ShowOnMap"); } }
        public static Image Coriolis { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Coriolis"); } }
        public static Image EDShipYard { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.EDShipYard"); } }
        public static Image Spaceship { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Spaceship"); } }
        #region EDDiscovery.UserControls.UserControlScan
        public static Image Scan_Bodies_Landable { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.Bodies.Landable"); } }
        public static Image Scan_Bodies_LandablePlanet { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.Bodies.LandablePlanet"); } }
        public static Image Scan_Bodies_HighValue { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.Bodies.HighValue"); } }
        public static Image Scan_Bodies_Volcanism { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.Bodies.Volcanism"); } }
        public static Image Scan_Bodies_Signals { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.Bodies.Signals"); } }
        public static Image Scan_Bodies_Mapped { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.Bodies.Mapped"); } }
        public static Image Scan_Bodies_RingOnly { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.Bodies.RingOnly"); } }
        public static Image Scan_DisplaySystemAlways { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.DisplaySystemAlways"); } }
        public static Image Scan_ExportToExcel { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.ExportToExcel"); } }
        public static Image Scan_FetchEDSMBodies { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.FetchEDSMBodies"); } }
        public static Image Scan_ShowAllMaterials { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.ShowAllMaterials"); } }
        public static Image Scan_FSD { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.FSD"); } }
        public static Image Scan_HideFullMaterials { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.HideFullMaterials"); } }
        public static Image Scan_Bodies_NSP { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.Bodies.NSP"); } }
        public static Image Scan_ShowMoons { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.ShowMoons"); } }
        public static Image Scan_ShowOverlays { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.ShowOverlays"); } }
        public static Image Scan_ShowRareMaterials { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.ShowRareMaterials"); } }
        public static Image Scan_SizeHuge { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.SizeHuge"); } }
        public static Image Scan_SizeVeryLarge { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.SizeVeryLarge"); } }
        public static Image Scan_SizeLarge { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.SizeLarge"); } }
        public static Image Scan_SizeMedium { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.SizeMedium"); } }
        public static Image Scan_SizeSmall { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.SizeSmall"); } }
        public static Image Scan_SizeTiny { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.SizeTiny"); } }
        public static Image Scan_SizeTinyTiny { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.SizeTinyTiny"); } }
        public static Image Scan_SizeMinuscule { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.SizeMinuscule"); } }
        public static Image Scan_Star { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.Star"); } }
        public static Image backbutton { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.backbutton"); } }
        public static Image DisplayFilters { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.DisplayFilters"); } }
        public static Image ShowAllG { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.ShowAllG"); } }
        public static Image ShowDistances { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.ShowDistances"); } }
        public static Image ShowHabZone { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.ShowHabZone"); } }
        public static Image ShowPlanetClasses { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.ShowPlanetClasses"); } }
        public static Image ShowStarClasses { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.ShowStarClasses"); } }
        #endregion
        public static Image Belt { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Belt"); } }
        #region EDDiscovery.UserControls.UserControlSPanel
        public static Image SPanel_ResizeColumn { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.SPanel.ResizeColumn"); } }
        #endregion
        public static Image Graph { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Graph"); } }
        public static Image Planets { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Planets"); } }
        public static Image Stars { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Stars"); } }
        public static Image Text { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Text"); } }
        public static Image ExportToExcel { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ExportToExcel"); } }
        public static Image FlagStart { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.FlagStart"); } }
        public static Image FlagStop { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.FlagStop"); } }
        public static Image CursorToTop { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.CursorToTop"); } }
        public static Image CursorStill { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.CursorStill"); } }
        public static Image Outlines { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Outlines"); } }
        public static Image FieldFilter { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.FieldFilter"); } }
        public static Image EventFilter { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.EventFilter"); } }
        public static Image WordWrapOff { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.WordWrapOff"); } }
        public static Image WordWrapOn { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.WordWrapOn"); } }
        public static Image Cross { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Cross"); } }
        public static Image Globe { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Globe"); } }
        public static Image SubmitDistances { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.SubmitDistances"); } }
        #region EDDiscovery.UserControls.UserControlContainerGrid
        public static Image UCContainer_Panels { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.UCContainer.Panels"); } }
        public static Image UCContainer_Remove { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.UCContainer.Remove"); } }
        public static Image UCContainer_Tile { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.UCContainer.Tile"); } }
        #endregion
        public static Image firstdiscover { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.firstdiscover"); } }
        public static Image notfirstdiscover { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.notfirstdiscover"); } }
        public static Image spansh { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.spansh"); } }
        public static Image EDDB { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.EDDB"); } }
        public static Image EDSM { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.EDSM"); } }
        public static Image Inara { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Inara"); } }
        #region TabStrip
        public static Image Popout { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Popout"); } }
        #endregion
        #region Selector
        public static Image Selector_Background { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Selector.Selector"); } }
        public static Image Selector_Background2 { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Selector.Selector2"); } }
        public static Image Selector_AddTab { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Selector.Addtab"); } }
        #endregion
        public static Image Edit { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Edit"); } }
        public static Image EditSystem { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.EditSystem"); } }
        public static Image New { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.New"); } }
        public static Image Delete { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Delete"); } }
        public static Image ImportExcel { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ImportExcel"); } }
        public static Image SearchStars { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.SearchStars"); } }
        public static Image SearchMaterials { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.SearchMaterials"); } }
        public static Image Scan { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan"); } }
        public static Image NewPage { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.NewPage"); } }
        public static Image Tags { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Tags"); } }
        public static Image Entries { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Entries"); } }
        public static Image Diary { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Diary"); } }
        public static Image redzero { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.redzero"); } }
        public static Image greenzero { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.greenzero"); } }
        public static Image Wiki { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Wiki"); } }
        public static Image Video { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Video"); } }

        #region Journal
        public static Image FSSBodySignals { get { return BaseUtils.Icons.IconSet.GetIcon("Journal.FSSBodySignals"); } }

        #endregion

        public static Image AlignCentre { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.AlignCentre"); } }
        public static Image AlignLeft { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.AlignLeft"); } }
        public static Image AlignRight { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.AlignRight"); } }
        public static Image Route { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Route"); } }
        public static Image RouteConfig { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.RouteConfig"); } }
        public static Image Eccentric { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Eccentric"); } }
        public static Image Font { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Font"); } }

    }
}
