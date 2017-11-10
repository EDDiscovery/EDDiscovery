﻿/*
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
using System.Drawing;
using System.Windows.Forms;

namespace ExtendedControls
{
    public interface ITheme             // Extended controls use this if they want to be themed
    {
        bool ApplyToForm(Form form, Font fnt = null);   // null means use standard one
        void ApplyToControls(Control parent, Font fnt = null, bool applytothis = false);
        Color TextBlockColor { get; set; }
        string FontName { get; set; }
        bool WindowsFrame { get; set; }
        Icon MessageBoxWindowIcon { get; set; }
    }
}
