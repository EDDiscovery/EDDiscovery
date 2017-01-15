/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;

namespace EDDiscovery2._3DMap
{
    public class MapManager
    {
        private FormMap _formMap;

        public MapManager(bool nowindowreposition,EDDiscoveryForm frm)
        {
            _formMap = new FormMap();
            _formMap.discoveryForm = frm;
            _formMap.noWindowReposition = nowindowreposition;
        }

        public bool Is3DMapsRunning { get { return _formMap.Is3DMapsRunning; } }

        public void Prepare(ISystem historysel, string homesys, ISystem centersys, float zoom, List<HistoryEntry> visited)
        {
            _formMap.Prepare(historysel, homesys, centersys, zoom, visited);
        }

        public void SetPlanned(List<SystemClass> plannedr)
        {
            _formMap.SetPlannedRoute(plannedr);
        }

        public void UpdateHistorySystem(ISystem historysel)
        {
            _formMap.UpdateHistorySystem(historysel);
        }

        public bool MoveToSystem(ISystem system)
        {
            return _formMap.SetCenterSystemTo(system);
        }

        public void UpdateNote()
        {
            _formMap.UpdateNote();
        }

        public void Show()
        {
            _formMap.TopMost = EDDiscoveryForm.EDDConfig.KeepOnTop;
            // TODO: set Opacity to match EDDiscoveryForm
            _formMap.Show();
            _formMap.Focus();
        }
    }
}
