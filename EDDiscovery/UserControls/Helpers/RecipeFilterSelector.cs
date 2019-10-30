/*
 * Copyright © 2016 EDDiscovery development team
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

using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    // UI to help select recipes

    public class RecipeFilterSelector
    {
        public Action Changed;

        private List<string> options;

        public RecipeFilterSelector(List<string> opts)
        {
            options = opts;
        }

        public void FilterButton(string db, Control ctr, Color back, Color fore, Form parent)
        {
            FilterButton(db, ctr, back, fore, parent, options);
        }

        ExtendedControls.CheckedIconListBoxFormGroup cc;

        public void FilterButton(string db, Control ctr, Color back, Color fore, Form parent, List<string> list)
        {
            cc = new ExtendedControls.CheckedIconListBoxFormGroup();
            cc.AddAllNone();

            foreach (var s in list)
                cc.AddStandardOption(s, s);

            cc.SaveSettings += (s, o) =>
            {
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(db, s);
                Changed?.Invoke();
            };

            cc.Show(EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(db, "All"), ctr, parent);
        }

    }


}