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
        public static Image ConfigureAddOnActions { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ConfigureAddOnActions"); } }
        public static Image EditLastActionPack { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.EditLastActionPack"); } }
        public static Image ManageAddOns { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ManageAddOns"); } }
        public static Image ManageAddOns48 { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ManageAddOns48"); } }
        public static Image Pause { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Pause"); } }
        public static Image FSDJumps { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.FSDJumps"); } }
        public static Image ReadNetLogs { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ReadNetLogs"); } }
        public static Image RescanJournals { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.RescanJournals"); } }
        public static Image Warning { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Warning"); } }
        public static Image EDDN { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.EDDN"); } }
        public static Image InaraDark { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.InaraDark"); } }
        public static Image ShowLogFiles { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ShowLogFiles"); } }
        public static Image About { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.About"); } }
        public static Image CheckForNewRelease { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.CheckForNewRelease"); } }
        public static Image DiscordChat { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.DiscordChat"); } }
        public static Image FrontierForumThread { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.FrontierForumThread"); } }
        public static Image Github { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Github"); } }
        public static Image ReportIssue { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ReportIssue"); } }
        public static Image SafeModeHelp { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.SafeModeHelp"); } }
        public static Image EditAddons { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.EditAddons"); } }
        public static Image Popouts { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Popouts"); } }
        public static Image Refresh { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Refresh"); } }
        public static Image Exit { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Exit"); } }
        public static Image Map3D { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Map3D"); } }
        public static Image DisableTransparency { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.DisableTransparency"); } }
        public static Image Menu { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Menu"); } }
        public static Image ShowAllInTaskbar { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ShowAllInTaskbar"); } }
        public static Image Settings { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Settings"); } }
        public static Image ManyStars { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ManyStars"); } }
        #endregion
        public static Image Star { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Star"); } }
        public static Image ZoomIn { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ZoomIn"); } }
        public static Image ZoomOut { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ZoomOut"); } }
        public static Image ZoomToFit { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ZoomToFit"); } }
        public static Image ExportFile { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ExportFile"); } }
        public static Image ImportFile { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ImportFile"); } }
        public static Image ImportSphere { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ImportSphere"); } }
        public static Image ImportRoute { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ImportRoute"); } }
        public static Image ImportNavRoute { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ImportNavRoute"); } }
        public static Image ImportNavRouteLatest { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ImportNavRouteLatest"); } }
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
        public static Image Scan_NotDiscoveredButMapped { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Scan.NotDiscoveredButMapped"); } }
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
        public static Image ResizeColumn { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ResizeColumn"); } }
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
        public static Image Copy { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Copy"); } }
        public static Image Globe { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Globe"); } }
        public static Image SubmitDistances { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.SubmitDistances"); } }
        public static Image Panels { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Panels"); } }
        public static Image Tile { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Tile"); } }
        public static Image firstdiscover { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.firstdiscover"); } }
        public static Image notfirstdiscover { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.notfirstdiscover"); } }
        public static Image spansh { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.spansh"); } }
        public static Image EDDB { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.EDDB"); } }
        public static Image EDSM { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.EDSM"); } }
        public static Image Inara { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Inara"); } }
        public static Image Popout { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Popout"); } }
        public static Image Selector { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Selector"); } }
        public static Image Selector2 { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Selector2"); } }
        public static Image AddTab { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Addtab"); } }
        public static Image Addtab48 { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Addtab48"); } }
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

        public static Image AlignCentre { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.AlignCentre"); } }
        public static Image AlignLeft { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.AlignLeft"); } }
        public static Image AlignRight { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.AlignRight"); } }
        public static Image Route { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Route"); } }
        public static Image RouteConfig { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.RouteConfig"); } }
        public static Image Eccentric { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Eccentric"); } }
        public static Image Font { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Font"); } }
        public static Image Columns { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Columns"); } }
        public static Image ColumnOrder { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ColumnOrder"); } }
        public static Image ScanPosition { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ScanPosition"); } }
        public static Image HighValueTick { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.HighValueTick"); } }
        public static Image Level { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Level"); } }
        public static Image EngineerRecipe { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.EngineerRecipe"); } }
        public static Image EngineerCraft { get { return BaseUtils.Icons.IconSet.GetIcon("Journal.EngineerCraft"); } }
        public static Image Materials { get { return BaseUtils.Icons.IconSet.GetIcon("Journal.Materials"); } }
        public static Image FSSBodySignals { get { return BaseUtils.Icons.IconSet.GetIcon("Journal.FSSBodySignals"); } }
        public static Image ModuleInfo { get { return BaseUtils.Icons.IconSet.GetIcon("Journal.ModuleInfo"); } }
        public static Image EngineerApply { get { return BaseUtils.Icons.IconSet.GetIcon("Journal.EngineerApply"); } }
        public static Image Synthesis { get { return BaseUtils.Icons.IconSet.GetIcon("Journal.Synthesis"); } }
        public static Image OrganicIncomplete { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.OrganicIncomplete"); } }
        public static Image lockimage { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.lockimage"); } }
        public static Image RoundPlus { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.RoundPlus"); } }
        public static Image RoundMinus { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.RoundMinus"); } }
        public static Image RoundExpand { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.RoundExpand"); } }
        public static Image RoundExpanded { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.RoundExpanded"); } }
        public static Image RoundUp { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.RoundUp"); } }
        public static Image RoundDown { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.RoundDown"); } }
        public static Image Find { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Find"); } }
        public static Image Reverse { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Reverse"); } }
        public static Image Stop { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Stop"); } }
        public static Image Ledger { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Ledger"); } }

        public static Image Bookmarks { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.Bookmarks"); } }
        public static Image FleetCarrier { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.FleetCarrier"); } }
        public static Image ArrowsRight { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.ArrowsRight"); } }
        public static Image AddJournals { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.AddJournals"); } }
        public static Image NewRegion { get { return BaseUtils.Icons.IconSet.GetIcon("Controls.NewRegion"); } }
    }
}
