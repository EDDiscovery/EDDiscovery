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
using EDDiscovery.Controls;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlShoppingList : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;
        private UserControlCursorType uctg;

        #region Init

        public UserControlShoppingList()
        {
            InitializeComponent();
        }

        public override void Init(EDDiscoveryForm ed, UserControlCursorType thc, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            uctg = thc;
            displaynumber = vn;
            //need to do something for vn in child controls so it won't conflict with standalone ones.  +20 will work for now, but could be cleaner
            userControlEngineering.Init(ed, thc, vn+20);
            userControlSynthesis.Init(ed, thc, vn+20);
            
        }

        public override void ChangeCursorType(UserControlCursorType thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            last_he = uctg.GetCurrentHistoryEntry;
            Display();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            Display();
        }

        HistoryEntry last_he = null;
        private void Display(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            Display();
        }

        private void Display()
        {

            if (last_he != null)
            {
            }
        }

        #endregion

        #region Layout

        public override void LoadLayout()
        {
        }

        public override void Closing()
        {
        }

        #endregion

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            Display();
        }

        private Rectangle moveMoveDragBox;

    }
}
