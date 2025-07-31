/*
 * Copyright © 2023-2023 EDDiscovery development team
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

using System.Linq;

namespace EDDiscovery.UserControls
{
    // Control providing scan display settings

    public class ScanDisplayConfigureButton : ExtendedControls.ExtButtonWithNewCheckedListBox
    {
        // use ValueChanged to pick up the change 

        public string Setting { get; private set; }
        public string[] DisplayFilters { get { return Setting.Split(";"); } }

        public void Init(EliteDangerousCore.DB.IUserDatabaseSettingsSaver ucb, string settingname)
        {
            Setting = ucb.GetSetting(settingname, "moons;icons;mats;allg;habzone;starclass;planetclass;dist;starage;starmass");

            Init(new ExtendedControls.CheckedIconUserControl.Item[]
            {
                new ExtendedControls.CheckedIconUserControl.Item("icons", "Show body status icons".Tx(), global::EDDiscovery.Icons.Controls.Scan_ShowOverlays),
                new ExtendedControls.CheckedIconUserControl.Item("mats", "Show Materials".Tx(), global::EDDiscovery.Icons.Controls.Scan_ShowAllMaterials),
                new ExtendedControls.CheckedIconUserControl.Item("rares", "Show rare materials only".Tx(), global::EDDiscovery.Icons.Controls.Scan_ShowRareMaterials),
                new ExtendedControls.CheckedIconUserControl.Item("matfull", "Hide materials which have reached their storage limit".Tx(), global::EDDiscovery.Icons.Controls.Scan_HideFullMaterials),
                new ExtendedControls.CheckedIconUserControl.Item("moons", "Show Moons".Tx(), global::EDDiscovery.Icons.Controls.Scan_ShowMoons),
                new ExtendedControls.CheckedIconUserControl.Item("allg", "Show G on all planets".Tx(), global::EDDiscovery.Icons.Controls.ShowAllG),
                new ExtendedControls.CheckedIconUserControl.Item("planetmass", "Show mass of planets".Tx(), global::EDDiscovery.Icons.Controls.ShowAllG),
                new ExtendedControls.CheckedIconUserControl.Item("starmass", "Show mass of stars".Tx(), global::EDDiscovery.Icons.Controls.ShowAllG),
                new ExtendedControls.CheckedIconUserControl.Item("starage", "Show age of stars".Tx(), global::EDDiscovery.Icons.Controls.ShowAllG),
                new ExtendedControls.CheckedIconUserControl.Item("habzone", "Show Habitation Zone".Tx(), global::EDDiscovery.Icons.Controls.ShowHabZone),
                new ExtendedControls.CheckedIconUserControl.Item("starclass", "Show Classes of Stars".Tx(), global::EDDiscovery.Icons.Controls.ShowStarClasses),
                new ExtendedControls.CheckedIconUserControl.Item("planetclass", "Show Classes of Planets".Tx(), global::EDDiscovery.Icons.Controls.ShowPlanetClasses),
                new ExtendedControls.CheckedIconUserControl.Item("dist", "Show distance of bodies".Tx(), global::EDDiscovery.Icons.Controls.ShowDistances),
                // no longer used new ExtendedControls.CheckedIconUserControl.Item("sys", "Show system and value in main display".Tx(), global::EDDiscovery.Icons.Controls.Scan_DisplaySystemAlways),
                new ExtendedControls.CheckedIconUserControl.Item("starsondiffline", "Show bodyless stars on separate lines".Tx(), global::EDDiscovery.Icons.Controls.ShowStarClasses),
                }, 
                Setting,
                (newsetting,ch) => {
                    Setting = newsetting;
                    ucb.PutSetting(settingname, newsetting);
                },
                allornoneshown:true,
                closeboundaryregion:new System.Drawing.Size(64,64),
                multicolumns:true);
        }

        public void ApplyDisplayFilters(ScanDisplayUserControl sduc)
        {
            var displayfilters = DisplayFilters;
            bool all = displayfilters.Contains("All");
            sduc.SystemDisplay.ShowMoons = displayfilters.Contains("moons") || all;
            sduc.SystemDisplay.ShowOverlays = displayfilters.Contains("icons") || all;
            sduc.SystemDisplay.ShowMaterials = displayfilters.Contains("mats") || all;
            sduc.SystemDisplay.ShowOnlyMaterialsRare = displayfilters.Contains("rares") || all;
            sduc.SystemDisplay.HideFullMaterials = displayfilters.Contains("matfull") || all;
            sduc.SystemDisplay.ShowAllG = displayfilters.Contains("allg") || all;
            sduc.SystemDisplay.ShowPlanetMass = displayfilters.Contains("planetmass") || all;
            sduc.SystemDisplay.ShowStarMass = displayfilters.Contains("starmass") || all;
            sduc.SystemDisplay.ShowStarAge = displayfilters.Contains("starage") || all;
            sduc.SystemDisplay.ShowHabZone = displayfilters.Contains("habzone") || all;
            sduc.SystemDisplay.ShowStarClasses = displayfilters.Contains("starclass") || all;
            sduc.SystemDisplay.ShowPlanetClasses = displayfilters.Contains("planetclass") || all;
            sduc.SystemDisplay.ShowDist = displayfilters.Contains("dist") || all;
            sduc.SystemDisplay.NoPlanetStarsOnSameLine = displayfilters.Contains("starsondiffline") || all;
        }

    }
}
