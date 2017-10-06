/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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
using System.Threading;
using System.Windows.Forms;
using EliteDangerousCore.EDSM;
using ExtendedControls;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;

namespace EDDiscovery
{
    public partial class TrilaterationControl : UserControl
    {
        public TrilaterationControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm, UserControls.UserControlCursorType uctg, int displaynumber)
        {
            userControlTrilateration.Init(discoveryForm, uctg, displaynumber);
        }

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            userControlTrilateration.LoadLayout();
            userControlTrilateration.InitialDisplay();
        }

        public void SaveSettings()     // called by form when closing
        {
            userControlTrilateration.Closing();
        }

    }
}
