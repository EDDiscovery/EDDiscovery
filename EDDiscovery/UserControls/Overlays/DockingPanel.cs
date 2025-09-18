/*
 * Copyright 2025 - 2025 EDDiscovery development team
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
using EliteDangerousCore.UIEvents;
using System.Drawing;

namespace EDDiscovery.UserControls
{
    public partial class DockingPanel : UserControlCommonBase
    {
        public DockingPanel()
        {
            InitializeComponent();
            DBBaseName = "DockingPanel";
        }

        private int lastdockingpad = 0;

        UIOverallStatus uistatus;
        HistoryEntry lasthe;

        protected override void Init()
        {
            DiscoveryForm.OnNewUIEvent += DiscoveryForm_OnNewUIEvent;
            DiscoveryForm.OnNewEntry += DiscoveryForm_OnNewEntry;
            DiscoveryForm.OnHistoryChange += DiscoveryForm_OnHistoryChange;

            // if opened after start, we need to pick up state from discoveryform

            uistatus = DiscoveryForm.UIOverallStatus;
            lasthe = DiscoveryForm.History.GetLast;
        }

        protected override void InitialDisplay()
        {
            UpdateDisplay();
        }

        protected override void Closing()
        {
            DiscoveryForm.OnNewUIEvent -= DiscoveryForm_OnNewUIEvent;
            DiscoveryForm.OnHistoryChange -= DiscoveryForm_OnHistoryChange;
            DiscoveryForm.OnNewEntry -= DiscoveryForm_OnNewEntry;
        }

        public override bool SupportTransparency => true;
        protected override void SetTransparency(bool on, Color curcol)
        {
            System.Diagnostics.Debug.WriteLine($"Transparent mode {on}");
            this.BackColor = curcol;

            if ( IsInPanelShow )
            {
                dockingPads.Visible = true;
                dockingPads.SelectedIndex = 0;
            }
            else
            {
                UpdateDisplay();
            }
        }

        private void DiscoveryForm_OnHistoryChange()
        {
            lasthe = DiscoveryForm.History.GetLast;
            UpdateDisplay();
        }

        private void DiscoveryForm_OnNewEntry(HistoryEntry he)
        {
            System.Diagnostics.Debug.WriteLine($"Autopanel NewHistory {he.EventTimeUTC}");
            lasthe = DiscoveryForm.History.GetLast;
            lastdockingpad = lasthe?.Status.DockingPad ?? 0;
            UpdateDisplay();
        }

        private void DiscoveryForm_OnNewUIEvent(EliteDangerousCore.UIEvent ui)
        {
            if (ui is UIOverallStatus os)      // if opened at start we get this, and we get it every time flags changes
            {
                System.Diagnostics.Debug.WriteLine($"AutoPanel UI {ui.EventTypeID} : {ui.ToString()}");
                uistatus = os;
                UpdateDisplay();
            }
        }

        public override void ReceiveHistoryEntry(EliteDangerousCore.HistoryEntry he)
        {
            // use for debug purposes, follows history cursor
           // lasthe = he; UpdateDisplay();
        }

        void UpdateDisplay()
        {
            int currentpad = lasthe?.Status.DockingPad ?? 0;

            if (lasthe?.Status.IsDockingStationTypeCoriolisEtc == false)        // if its not coriolis, ignore
                currentpad = 0;

            if ( uistatus.Focus != 0 || currentpad == 0)
            {
                dockingPads.Visible = false;
            }
            else
            {
                dockingPads.Visible = true;
            }

            dockingPads.SelectedIndex = currentpad;
        }
    }
}
