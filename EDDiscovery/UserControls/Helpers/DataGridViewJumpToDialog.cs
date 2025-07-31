/*
 * Copyright © 2017-2019 EDDiscovery development team
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

using EDDiscovery;
using System;
using System.Drawing;
using System.Windows.Forms;

public static class DataGridViewDialogs
{
    public static int JumpToDialog(this DataGridView grid, Form parent, int initialvalue , Func<DataGridViewRow, int> getindex)
    {
        ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

        int width = 430;
        int ctrlleft = 150;

        Type t = typeof(DataGridViewDialogs);

        f.Add(new ExtendedControls.ConfigurableEntryList.Entry("L", typeof(Label), "Jump to:".Tx(), new Point(10, 40), new Size(140, 24), ""));
        f.Add(new ExtendedControls.ConfigurableEntryList.Entry("Entry", typeof(ExtendedControls.NumberBoxLong), initialvalue.ToString(), new Point(ctrlleft, 40), new Size(width - ctrlleft - 20, 24), "Enter number to jump to or near to".Tx()) { NumberBoxDoubleMinimum = 0, NumberBoxFormat = "0" });

        f.AddOK(new Point(width - 100, 70), "Press to Accept".Tx());
        f.AddCancel(new Point(width - 200, 70), "Press to Cancel".Tx());

        f.Trigger += (dialogname, controlname, tag) =>
        {
            if (controlname == "OK" || controlname == "Entry:Return")
            {
                long? v3 = f.GetLong("Entry");
                if (v3.HasValue)
                {
                    f.ReturnResult(DialogResult.OK);
                }
                else
                    ExtendedControls.MessageBoxTheme.Show(parent, "Value is not valid".Tx(), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (controlname == "Cancel" || controlname == "Close" )
            {
                f.ReturnResult(DialogResult.Cancel);
            }
        };

        DialogResult res = f.ShowDialogCentred(parent, parent.Icon,  "Jump to Entry".Tx(), closeicon:true);

        if (res == DialogResult.OK)
        {
            int target = (int)f.GetLong("Entry").Value;

            int rowclosest = 0;
            int rowdist = int.MaxValue;

            foreach (DataGridViewRow r in grid.Rows)
            {
                if (r.Visible)
                {
                    int index = getindex(r);
                    int delta = Math.Abs(target - index);
                    if (delta < rowdist)
                    {
                        rowdist = delta;
                        rowclosest = r.Index;
                    }
                }
            }

            grid.DisplayRow(rowclosest, true);
                
            return rowclosest;
        }
        else
            return -1;
    }
}
