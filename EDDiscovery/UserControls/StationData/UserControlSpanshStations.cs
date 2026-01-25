/*
 * Copyright © 2023 - 2023 EDDiscovery development team
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

using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSpanshStations : UserControlCommonBase
    {
        public UserControlSpanshStations()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);

            DBBaseName = "SpanshStations";
        }

        protected override void LoadLayout()
        {
            spanshStationsUserControl.Init(this,()=>IsClosed);
        }

        protected override void Closing()
        {
            spanshStationsUserControl.Close();
        }

        protected override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestHistoryGridPos());     //request an update 
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            spanshStationsUserControl.UpdateDefaultSystem(he.System);
        }
        
   
    }
}
