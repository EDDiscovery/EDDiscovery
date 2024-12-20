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

using BaseUtils;
using EliteDangerousCore;
using System;
using System.Collections.Generic;

namespace EDDiscovery.UserControls
{
    // Control providing edsm/spansh settings, saving loading

    public class ScanDisplayBodyFiltersButton : ExtendedControls.ExtButtonWithNewCheckedListBox
    {
        // use ValueChanged to pick up the change 

        public string Setting { get; private set; }
        public string[] BodyFilters { get { return Setting.Split(";"); } }

        public void Init(EliteDangerousCore.DB.IUserDatabaseSettingsSaver ucb, string settingname)
        {
            Setting = ucb.GetSetting(settingname, "All");

            var options = new List<ExtendedControls.CheckedIconUserControl.Item>();
            foreach (var x in Enum.GetValues(typeof(EDPlanet)))
                options.Add(new ExtendedControls.CheckedIconUserControl.Item(x.ToString(), Planets.PlanetNameTranslated((EDPlanet)x)));

            foreach (var x in Enum.GetNames(typeof(EDStar)))
                options.Add(new ExtendedControls.CheckedIconUserControl.Item(x.ToString(), Stars.StarName(x.ParseEnum<EDStar>())));

            // these are filter types for items which are either do not have scandata or are not stars/bodies.  Only Belts/Barycentre are displayed.. scans of rings/beltculsters are not displayed
            options.Add(new ExtendedControls.CheckedIconUserControl.Item("star", "Star".T(EDTx.UserControlScan_Star)));
            options.Add(new ExtendedControls.CheckedIconUserControl.Item("body", "Body".T(EDTx.UserControlScan_Body)));
            options.Add(new ExtendedControls.CheckedIconUserControl.Item("barycentre", "Barycentre".T(EDTx.UserControlScan_Barycentre)));
            options.Add(new ExtendedControls.CheckedIconUserControl.Item("belt", "Belt".T(EDTx.UserControlScan_Belt)));

            Init(options,
                Setting,
                (newsetting,ch) => {
                    Setting = newsetting;
                    ucb.PutSetting(settingname, newsetting);
                },
                allornoneshown:true,
                closeboundaryregion:new System.Drawing.Size(64,64),
                multicolumns:true);
        }
    }
}
