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
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlLocal3DMap : UserControlCommonBase
    {
        private GLWinFormControl glwfc;
        private Timer systemtimer = new Timer();
        private Map map;
        private UserControl3DMap.MapSaverImpl mapsave;

        public UserControlLocal3DMap() 
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "Local3DMapPanel_";

            BaseUtils.Translator.Instance.Translate(this);      // translate before we add anything else to the panel

            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnSyncComplete += Discoveryform_OnSyncComplete;

            glwfc = new GLOFC.WinForm.GLWinFormControl(panelOuter);
            glwfc.EnsureCurrentPaintResize = true;      // set, ensures context is set up for internal code on paint and any Paints chained to it

            mapsave = new UserControl3DMap.MapSaverImpl(this);
        }

        public override void LoadLayout()
        {
            base.LoadLayout();

            glwfc.EnsureCurrentContext();

            // load setup restore settings of map
            map = new Map();
            map.Start(glwfc, discoveryform.galacticMapping, discoveryform.eliteRegions, this, 
                  Map.Parts.None
                | Map.Parts.Galaxy
                | Map.Parts.Grid | Map.Parts.TravelPath | Map.Parts.NavRoute  
                | Map.Parts.EDSMStars
                | Map.Parts.PrepopulateEDSMLocalArea            
                | Map.Parts.Menu        
                | Map.Parts.SearchBox | Map.Parts.RightClick 
                | Map.Parts.YHoldButton | Map.Parts.GalaxyResetPos 
                );

            map.LoadState(mapsave,true,200000);

            map.UpdateEDSMStarsLocalArea();    // now try and ask for a populated update after loading the settings

            // start clock
            systemtimer.Interval = 50;
            systemtimer.Tick += new EventHandler(SystemTick);
            systemtimer.Start();
        }

        public override void Closing()
        {
            System.Diagnostics.Debug.WriteLine($"local 3dmap {displaynumber} stop");
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnSyncComplete -= Discoveryform_OnSyncComplete;
            systemtimer.Stop();

            glwfc.EnsureCurrentContext();           // must make sure current context before we call all the dispose functions
            map.SaveState(mapsave);
            map.Dispose();

            glwfc.Dispose();
            glwfc = null;
        }

        private void SystemTick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Assert(systemtimer.Enabled);

            glwfc.EnsureCurrentContext();           // ensure the context, work may be done in the timers to the GL.
            GLOFC.Timers.Timer.ProcessTimers();
            map.Systick();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            if (he.IsFSDCarrierJump)
            {
                map.UpdateTravelPath();
            }
            else if (he.journalEntry.EventTypeID == JournalTypeEnum.NavRoute)
            {
                map.UpdateNavRoute();
            }

        }

        private void Discoveryform_OnSyncComplete(long full, long update)
        {
            if (full + update > 0)      // only if something changes do we refresh
            {
                map.UpdateEDSMStarsLocalArea();
            }
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            map.UpdateEDSMStarsLocalArea();
            map.UpdateTravelPath();
            map.UpdateNavRoute();
        }
    }
}
