/*
 * Copyright 2019-2023 Robbyxp1 @ github.com
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

using System;

namespace EDDiscovery.UserControls.Map3D
{
    public partial class Map
    {
        #region Enables

        public bool GalaxyDisplay { get { return galaxyshader?.Enable ?? true; } set { if (galaxyshader != null) galaxyshader.Enable = value; glwfc.Invalidate(); } }
        public bool StarDotsSpritesDisplay { get { return stardots?.Enable ?? true; } set { if (stardots != null) stardots.Enable = starsprites.Enable = value; glwfc.Invalidate(); } }
        public int GalaxyStars { get { return galaxystars?.EnableMode ?? 0; } set { if (galaxystars != null) galaxystars.EnableMode = value; glwfc.Invalidate(); } }
        public int GalaxyStarsMaxObjects { get { return galaxystars?.MaxObjectsAllowed ?? 100000; } set { if (galaxystars != null) galaxystars.MaxObjectsAllowed = value; } }
        public bool Grid { get { return gridshader?.Enable ?? true; } set { if (gridshader != null) gridshader.Enable = gridtextshader.Enable = value; glwfc.Invalidate(); } }

        public bool NavRouteDisplay { get { return navroute?.EnableTape ?? true; } set { if (navroute != null) navroute.EnableTape = navroute.EnableStars = navroute.EnableText = value; glwfc.Invalidate(); } }
        public bool TravelPathTapeDisplay { get { return travelpath?.EnableTape ?? true; } set { if (travelpath != null) travelpath.EnableTape = value; glwfc.Invalidate(); } }
        public bool TravelPathTapeStars { get { return travelpath?.EnableStars ?? true; } set { if (travelpath != null) travelpath.EnableStars = value; glwfc.Invalidate(); } }
        public bool TravelPathTextDisplay { get { return travelpath?.EnableText ?? true; } set { if (travelpath != null) travelpath.EnableText = value; glwfc.Invalidate(); } }
        public void TravelPathRefresh() { if (travelpath != null) UpdateTravelPath(); }   // travelpath.Refresh() manually after these have changed
        public DateTime TravelPathStartDateUTC { get { return travelpath?.TravelPathStartDateUTC ?? new DateTime(2014, 12, 14, 1, 1, 1, DateTimeKind.Utc); } set { if (travelpath != null && travelpath.TravelPathStartDateUTC != value) { travelpath.TravelPathStartDateUTC = value; } } }
        public bool TravelPathStartDateEnable { get { return travelpath?.TravelPathStartDateEnable ?? true; } set { if (travelpath != null && travelpath.TravelPathStartDateEnable != value) { travelpath.TravelPathStartDateEnable = value; } } }
        public DateTime TravelPathEndDateUTC { get { return travelpath?.TravelPathEndDateUTC ?? new DateTime(2040, 1, 1, 1, 1, 1, DateTimeKind.Utc); } set { if (travelpath != null && travelpath.TravelPathEndDateUTC != value) { travelpath.TravelPathEndDateUTC = value; } } }
        public bool TravelPathEndDateEnable { get { return travelpath?.TravelPathEndDateEnable ?? true; } set { if (travelpath != null && travelpath.TravelPathEndDateEnable != value) { travelpath.TravelPathEndDateEnable = value; } } }

        public bool UserImagesEnable { get { return usertexturebitmaps?.Enable ?? false; } set { if (usertexturebitmaps != null) { usertexturebitmaps.Enable = value; glwfc.Invalidate(); } } }

        public bool GalObjectDisplay
        {
            get { return galmapobjects?.Enable ?? true; }
            set
            {
                if (galmapobjects != null)
                {
                    galmapobjects.SetShaderEnable(value);
                    UpdateDueToGMOEnableChanging();
                    glwfc.Invalidate();
                }
            }
        }
        public void SetGalObjectTypeEnable(string id, bool state)
        {
            if (galmapobjects != null)
            {
                galmapobjects.SetGalObjectTypeEnable(id, state);
                UpdateDueToGMOEnableChanging();
                glwfc.Invalidate();
            }
        }
        public void SetAllGalObjectTypeEnables(string set)
        {
            if (galmapobjects != null)
            {
                galmapobjects.SetAllEnables(set);
                UpdateDueToGMOEnableChanging();
                glwfc.Invalidate();
            }
        }
        public bool GetGalObjectTypeEnable(string id) { return galmapobjects?.GetGalObjectTypeEnable(id) ?? true; }
        public string GetAllGalObjectTypeEnables() { return galmapobjects?.GetAllEnables() ?? ""; }
        public bool EDSMRegionsEnable { get { return edsmgalmapregions?.Enable ?? false; } set { if (edsmgalmapregions != null) edsmgalmapregions.Enable = value; glwfc.Invalidate(); } }
        public bool EDSMRegionsOutlineEnable { get { return edsmgalmapregions?.Outlines ?? true; } set { if (edsmgalmapregions != null) edsmgalmapregions.Outlines = value; glwfc.Invalidate(); } }
        public bool EDSMRegionsShadingEnable { get { return edsmgalmapregions?.Regions ?? false; } set { if (edsmgalmapregions != null) edsmgalmapregions.Regions = value; glwfc.Invalidate(); } }
        public bool EDSMRegionsTextEnable { get { return edsmgalmapregions?.Text ?? true; } set { if (edsmgalmapregions != null) edsmgalmapregions.Text = value; glwfc.Invalidate(); } }
        public bool EliteRegionsEnable { get { return elitemapregions?.Enable ?? true; } set { if (elitemapregions != null) elitemapregions.Enable = value; glwfc.Invalidate(); } }
        public bool EliteRegionsOutlineEnable { get { return elitemapregions?.Outlines ?? true; } set { if (elitemapregions != null) elitemapregions.Outlines = value; glwfc.Invalidate(); } }
        public bool EliteRegionsShadingEnable { get { return elitemapregions?.Regions ?? false; } set { if (elitemapregions != null) elitemapregions.Regions = value; glwfc.Invalidate(); } }
        public bool EliteRegionsTextEnable { get { return elitemapregions?.Text ?? true; } set { if (elitemapregions != null) elitemapregions.Text = value; glwfc.Invalidate(); } }
        public bool ShowBookmarks { get { return bookmarks?.Enable ?? true; } set { if (bookmarks != null) bookmarks.Enable = value; glwfc.Invalidate(); } }
        public int LocalAreaSize { get { return localareasize; } set { if (value != localareasize) { localareasize = value; UpdateEDSMStarsLocalArea(); } } }

        public int AutoScaleGMOs
        {
            get { return autoscalegmo; }
            set
            {
                autoscalegmo = value;
                if (galmapobjects != null)
                    galmapobjects.SetAutoScale(autoscalegmo);
            }
        }
        public int AutoScaleBookmarks
        {
            get { return autoscalebookmarks; }
            set
            {
                autoscalebookmarks = value;
                if (bookmarks != null)
                    bookmarks.SetAutoScale(autoscalebookmarks);
            }
        }
        public int AutoScaleGalaxyStars
        {
            get { return autoscalegalstars; }
            set
            {
                autoscalegalstars = value;
                if (galaxystars != null)
                    galaxystars.SetAutoScale(autoscalegalstars);
            }
        }
        #endregion

        #region State load

        public void LoadState(MapSaver defaults, bool restorepos, int loadlimit)
        {
            System.Diagnostics.Debug.WriteLine($"Load state");

            gl3dcontroller.ChangePerspectiveMode(defaults.GetSetting("GAL3DMode", true));

            GalaxyDisplay = defaults.GetSetting("GD", true);
            StarDotsSpritesDisplay = defaults.GetSetting("SDD", true);
            NavRouteDisplay = defaults.GetSetting("NRD", true);
            TravelPathTapeDisplay = defaults.GetSetting("TPD", true);
            TravelPathTapeStars = defaults.GetSetting("TPStars", true);
            TravelPathTextDisplay = defaults.GetSetting("TPText", true);
            TravelPathStartDateUTC = defaults.GetSetting("TPSD", EliteDangerousCore.EliteReleaseDates.GameRelease);
            TravelPathStartDateEnable = defaults.GetSetting("TPSDE", false);
            TravelPathEndDateUTC = defaults.GetSetting("TPED", DateTime.UtcNow.AddMonths(1));
            TravelPathEndDateEnable = defaults.GetSetting("TPEDE", false);
            if ((TravelPathStartDateEnable || TravelPathEndDateEnable) && travelpath != null)
                UpdateTravelPath();

            GalObjectDisplay = defaults.GetSetting("GALOD", true);
            SetAllGalObjectTypeEnables(defaults.GetSetting("GALOBJLIST", ""));

            UserImagesEnable = defaults.GetSetting("ImagesEnable", true);
            userimages?.LoadFromString(defaults.GetSetting("ImagesList", ""));

            EDSMRegionsEnable = defaults.GetSetting("ERe", false);
            EDSMRegionsOutlineEnable = defaults.GetSetting("ERoe", false);
            EDSMRegionsShadingEnable = defaults.GetSetting("ERse", false);
            EDSMRegionsTextEnable = defaults.GetSetting("ERte", false);

            EliteRegionsEnable = defaults.GetSetting("ELe", true);
            EliteRegionsOutlineEnable = defaults.GetSetting("ELoe", true);
            EliteRegionsShadingEnable = defaults.GetSetting("ELse", false);
            EliteRegionsTextEnable = defaults.GetSetting("ELte", true);

            Grid = defaults.GetSetting("GRIDS", true);

            GalaxyStars = defaults.GetSetting("GALSTARS", 3);
            GalaxyStarsMaxObjects = (loadlimit == 0) ? defaults.GetSetting("GALSTARSOBJ", 500000) : loadlimit;
            LocalAreaSize = defaults.GetSetting("LOCALAREALY", 50);

            ShowBookmarks = defaults.GetSetting("BKMK", true);

            AutoScaleBookmarks = defaults.GetSetting("AUTOSCALEBookmarks", 30);
            AutoScaleGMOs = defaults.GetSetting("AUTOSCALEGMO", 30);
            AutoScaleGalaxyStars = defaults.GetSetting("AUTOSCALEGS", 4);

            if (restorepos)
            {
                var pos = defaults.GetSetting("POSCAMERA", "");
                if (pos.HasChars() && !gl3dcontroller.SetPositionCamera(pos))     // go thru gl3dcontroller to set default position, so we reset the model matrix
                    System.Diagnostics.Trace.WriteLine($"*** NOTE 3DMAPS {pos} did not decode");
            }
            System.Diagnostics.Debug.WriteLine($"Load state finished");
        }

        public void SaveState(MapSaver defaults)
        {
            if (!mapcreatedokay)
                return;

            defaults.PutSetting("GAL3DMode", gl3dcontroller.MatrixCalc.InPerspectiveMode);
            defaults.PutSetting("GD", GalaxyDisplay);
            defaults.PutSetting("SDD", StarDotsSpritesDisplay);
            defaults.PutSetting("TPD", TravelPathTapeDisplay);
            defaults.PutSetting("TPStars", TravelPathTapeStars);
            defaults.PutSetting("TPText", TravelPathTextDisplay);
            defaults.PutSetting("NRD", NavRouteDisplay);
            defaults.PutSetting("TPSD", TravelPathStartDateUTC);
            defaults.PutSetting("TPSDE", TravelPathStartDateEnable);
            defaults.PutSetting("TPED", TravelPathEndDateUTC);
            defaults.PutSetting("TPEDE", TravelPathEndDateEnable);
            defaults.PutSetting("GALOD", GalObjectDisplay);
            defaults.PutSetting("GALOBJLIST", GetAllGalObjectTypeEnables());
            defaults.PutSetting("ERe", EDSMRegionsEnable);
            defaults.PutSetting("ERoe", EDSMRegionsOutlineEnable);
            defaults.PutSetting("ERse", EDSMRegionsShadingEnable);
            defaults.PutSetting("ERte", EDSMRegionsTextEnable);
            defaults.PutSetting("ELe", EliteRegionsEnable);
            defaults.PutSetting("ELoe", EliteRegionsOutlineEnable);
            defaults.PutSetting("ELse", EliteRegionsShadingEnable);
            defaults.PutSetting("ELte", EliteRegionsTextEnable);
            defaults.PutSetting("GRIDS", Grid);
            defaults.PutSetting("GALSTARS", GalaxyStars);
            defaults.PutSetting("GALSTARSOBJ", GalaxyStarsMaxObjects);
            defaults.PutSetting("LOCALAREALY", LocalAreaSize);
            var pos = gl3dcontroller.PosCamera.StringPositionCamera;
            defaults.PutSetting("POSCAMERA", pos);
            defaults.PutSetting("BKMK", bookmarks?.Enable ?? true);
            defaults.PutSetting("AUTOSCALEBookmarks", AutoScaleBookmarks);
            defaults.PutSetting("AUTOSCALEGMO", AutoScaleGMOs);
            defaults.PutSetting("AUTOSCALEGS", AutoScaleGalaxyStars);

            defaults.PutSetting("ImagesEnable", UserImagesEnable);
            if (userimages != null)
                defaults.PutSetting("ImagesList", userimages.ImageStringList());
        }

        #endregion

    }
}
