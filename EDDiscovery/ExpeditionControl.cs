/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class ExpeditionControl : UserControl
    {
        public ExpeditionControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm, UserControls.UserControlCursorType uctg, int displaynumber)
        {
            userControlExpedition.Init(discoveryForm, uctg, displaynumber);
        }

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            userControlExpedition.LoadLayout();
            userControlExpedition.InitialDisplay();
        }

        public void SaveSettings()     // called by form when closing
        {
            userControlExpedition.Closing();
        }

    }
}
