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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery.EliteDangerous;
using EDDiscovery.DB;
using EDDiscovery.Controls;
using EDDiscovery.EDSM;
using EDDiscovery.UserControls;

namespace EDDiscovery
{
    public partial class JournalViewControl : UserControl
    {
        EDDiscoveryForm discoveryform;

        public JournalViewControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm ed, int displaynumber)
        {
            discoveryform = ed;
            userControlJournalGrid.Init(ed,displaynumber);
            userControlJournalGrid.ShowRefresh();
            userControlJournalGrid.OnPopOut += PopOut;
        }

        #region Layout

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            userControlJournalGrid.LoadLayout();
        }

        public void SaveSettings()     // called by form when closing
        {
            userControlJournalGrid.Closing();
        }

        public void RefreshButton(bool state)
        {
            userControlJournalGrid.RefreshButton(state);
        }

        public void PopOut()
        {
            discoveryform.PopOuts.PopOut(Forms.PopOutControl.PopOuts.Journal);
        }

        #endregion

    }

}
