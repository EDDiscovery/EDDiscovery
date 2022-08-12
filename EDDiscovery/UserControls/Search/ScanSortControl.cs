/*
 * Copyright © 2022-2022 EDDiscovery development team
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

using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls.Search
{
    public partial class ScanSortControl : UserControl
    {
        public string Condition { get { return extTextBoxSortCondition.Text; } set { extTextBoxSortCondition.Text = value; } }
        public bool Ascending { get { return extCheckBoxAscending.Checked; } set { extCheckBoxAscending.Checked = value; } }

        public List<string> AutoCompletes;

        public Action<bool> SortDirectionClicked;

        public void Set(string condition, bool ascending)
        {
            ignoreset = true;
            extCheckBoxAscending.Checked = ascending;
            extTextBoxSortCondition.Text = condition;
            extTextBoxSortCondition.SetAutoCompletor(AutoList);
            extCheckBoxAscending.CheckedChanged += ExtCheckBoxAscending_CheckedChanged;
            ignoreset = false;
        }

        public ScanSortControl()
        {
            InitializeComponent();
        }

        public void AutoList(string input, ExtTextBoxAutoComplete t, SortedSet<string> set)
        {
            if (AutoCompletes != null)
            {
                var res = (from x in AutoCompletes where x.StartsWith(input, StringComparison.InvariantCultureIgnoreCase) select x).ToList();
                foreach (var x in res)
                    set.Add(x);
            }
        }
        private void ExtCheckBoxAscending_CheckedChanged(object sender, EventArgs e)
        {
            if ( !ignoreset )
                SortDirectionClicked?.Invoke(extCheckBoxAscending.Checked);
        }

        private bool ignoreset = false;
    }
}
