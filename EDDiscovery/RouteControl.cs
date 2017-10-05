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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class RouteControl : UserControl
    {
        private EDDiscoveryForm discoveryform;

        public RouteControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm, UserControls.UserControlCursorType uctg, int displaynumber )
        {
            discoveryform = discoveryForm;
            userControlRoute.Init(discoveryform, uctg, displaynumber);
        }

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            userControlRoute.LoadLayout();
            userControlRoute.InitialDisplay();
        }

        public void SaveSettings()     // called by form when closing
        {
            userControlRoute.Closing();
        }
    }
}
