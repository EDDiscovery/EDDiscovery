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

namespace EDDiscovery.UserControls
{
    // Control providing edsm/spansh settings, saving loading

    public class EDSMSpanshButton : ExtendedControls.ExtButtonWithNewCheckedListBox
    {
        // use ValueChanged to pick up the change 

        public void Init(EliteDangerousCore.DB.IUserDatabaseSettingsSaver ucb, string settingname, string defaultvalue)
        {
            string startsetting = ucb.GetSetting(settingname, defaultvalue);
            Init(new ExtendedControls.CheckedIconUserControl.Item[]
            {
                new ExtendedControls.CheckedIconUserControl.Item("EDSM","EDSM",EDDiscovery.Icons.Controls.EDSM,"SPANSHEDSM"),
                new ExtendedControls.CheckedIconUserControl.Item("SPANSH","Spansh",EDDiscovery.Icons.Controls.spansh,"SPANSHEDSM"),
                new ExtendedControls.CheckedIconUserControl.Item("SPANSHEDSM","Spansh -> EDSM",EDDiscovery.Icons.Controls.spansh,"EDSM;SPANSH"),
            }, 
            startsetting,
            (newsetting,ch) => { 
                ucb.PutSetting(settingname, newsetting);
                Image = newsetting.HasChars() ? EDDiscovery.Icons.Controls.EDSMSpanshOn : EDDiscovery.Icons.Controls.EDSMSpansh;
            },
            allornoneshown:false,
            closeboundaryregion:new System.Drawing.Size(64,64));
            
            Image = startsetting.HasChars() ? EDDiscovery.Icons.Controls.EDSMSpanshOn : EDDiscovery.Icons.Controls.EDSMSpansh;
        }

        public EliteDangerousCore.WebExternalDataLookup WebLookup { get
            {
                if (IsSet("SPANSHEDSM"))
                {
                    return EliteDangerousCore.WebExternalDataLookup.SpanshThenEDSM;
                }
                else if (IsSet("SPANSH"))
                {
                    if (IsSet("EDSM"))
                        return EliteDangerousCore.WebExternalDataLookup.All;
                    else
                        return EliteDangerousCore.WebExternalDataLookup.Spansh;
                }
                else if (IsSet("EDSM"))
                    return EliteDangerousCore.WebExternalDataLookup.EDSM;
                else
                    return EliteDangerousCore.WebExternalDataLookup.None;
            }
        }
    }
}
