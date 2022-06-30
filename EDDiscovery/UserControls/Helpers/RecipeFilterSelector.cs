﻿/*
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

    public class RecipeFilterSelector : ExtendedControls.CheckedIconListBoxFormGroup
    {
        public RecipeFilterSelector(List<string> opts)
        {
            CloseOnDeactivate = false;          // this one, we hide it on deactivate, to make it pop up quicker next time
            HideOnDeactivate = true;
            ScreenMargin = new Size(0, 0);

            AddAllNone();
            foreach (var s in opts)
                AddStandardOption(s, s);
        }

        public void Open(string settings, Control ctr, Form parent)
        {
            CloseBoundaryRegion = new Size(32, ctr.Height);
            if (this.Visible == true)
            {
                Hide();
            }
            else
            {
                Show(settings, ctr, parent);     // use the quick helper. 
            }
        }
    }
}