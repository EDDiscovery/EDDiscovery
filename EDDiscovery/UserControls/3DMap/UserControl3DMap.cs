/*
 * Copyright 2019-2021 Robbyxp1 @ github.com
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

using EDDiscovery.UserControls.Map3D;
using EliteDangerousCore;
using GLOFC.WinForm;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControl3DMap : UserControlCommonBase
    {
        private GLWinFormControl glwfc;
        private Timer systemtimer = new Timer();
        private Map map;
        private MapSaverImpl mapsave;

        public UserControl3DMap() 
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "3dMapPanel_";

            BaseUtils.Translator.Instance.Translate(this);      // translate before we add anything else to the panel

            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;

            glwfc = new GLOFC.WinForm.GLWinFormControl(panelOuter);
            glwfc.EnsureCurrentPaintResize = true;      // set, ensures context is set up for internal code on paint and any Paints chained to it

            mapsave = new MapSaverImpl(this);
        }

        public override void LoadLayout()
        {
            base.LoadLayout();

            glwfc.EnsureCurrentContext();

            // load setup restore settings of map
            map = new Map();
            map.Start(glwfc, discoveryform.galacticMapping, discoveryform.eliteRegions, this, 
                Map.Parts.Map3D);
            map.LoadState(mapsave,true);

            // start clock
            systemtimer.Interval = 50;
            systemtimer.Tick += new EventHandler(SystemTick);
            systemtimer.Start();
        }

        public override void Closing()
        {
            System.Diagnostics.Debug.WriteLine($"3dmap {displaynumber} stop");
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            systemtimer.Stop();

            glwfc.EnsureCurrentContext();           // must make sure current context before we call all the dispose functions
            map.SaveState(mapsave);
            map.Dispose();

            glwfc.Dispose();
            glwfc = null;
        }

        public void SetRoute(List<ISystem> syslist)
        {
            map.SetRoute(syslist);
        }
        public void GotoSystem(ISystem sys, float distancely = 50)
        {
            map.GoToSystem(sys, distancely);
        }

        private void SystemTick(object sender, EventArgs e)
        {
            glwfc.EnsureCurrentContext();           // ensure the context, work may be done in the timers to the GL.
            //System.Diagnostics.Debug.WriteLine($"3dmap {displaynumber} tick");
            GLOFC.Timers.Timer.ProcessTimers();
            map.Systick();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            if (he.IsFSDCarrierJump)
            {
                map.UpdateTravelPath(discoveryform.history);
            }
            else if ( he.journalEntry.EventTypeID == JournalTypeEnum.NavRoute)
            {
                map.UpdateNavRoute(discoveryform.history);
            }
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            map.UpdateHistory(discoveryform.history);
        }

        public class MapSaverImpl : MapSaver
        {
            public MapSaverImpl(UserControlCommonBase n)
            {
                uc3d = n;
            }

            UserControlCommonBase uc3d;
            public T GetSetting<T>(string id, T defaultvalue)
            {
                return uc3d.GetSetting(id,defaultvalue);
            }

            public void PutSetting<T>(string id, T value)
            {
                uc3d.PutSetting(id, value);
            }
        }

    }
}
