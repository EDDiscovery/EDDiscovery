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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
{
    public enum UITypeEnum
    {
        Unknown = 0,
        GUIFocus,
        JournalMusic,
        Pips,
        Position,
        FireGroup,

        Docked, 
        Landed , 
        LandingGear , 
        ShieldsUp , 
        Supercruise , 
        FlightAssist ,
        HardpointsDeployed , 
        InWing ,
        Lights , 
        CargoScoopDeployed , 
        SilentRunning , 
        ScoopingFuel , 
        SrvHandbrake , 
        SrvTurret , 
        SrvUnderShip , 
        SrvDriveAssist , 
        FsdMassLocked , 
        FsdCharging , 
        FsdCooldown , 
        LowFuel , 
        OverHeating , 
        HasLatLong , 
        IsInDanger ,
        BeingInterdicted ,

        HUDInAnalysisMode, //3.3
        NightVision,             // 3.3
        Fuel,   // 3.3
        Cargo,  // 3.3
    
        ShipType ,     

        OverallStatus,
    }

    public abstract class UIEvent
    {
        public UIEvent(UITypeEnum t, DateTime time, bool refresh)
        {
            EventTypeID = t;
            EventTimeUTC = time;
            EventRefresh = refresh;
        }

        public DateTime EventTimeUTC { get; set; }
        public UITypeEnum EventTypeID { get; set; }             // name of event. 
        public string EventTypeStr { get { return EventTypeID.ToString(); } }
        public bool EventRefresh { get; set; }                  // either at the start or a forced refresh

        static string JournalRootClassname = typeof(UIEvents.UIDocked).Namespace;        // pick one at random to find out root classname

        // Flag Factory (others are created individually)

        static public UIEvent CreateFlagEvent(string name, bool value, DateTime time, bool refresh)
        {
            string evname = "UI" + name;
            Type t = Type.GetType(JournalRootClassname + "." + evname, false, true);
            if (t != null)
            {
                UIEvent e = (UIEvent)Activator.CreateInstance(t, new Object[] { value, time , refresh });
                return e;
            }
            else
                System.Diagnostics.Debug.Assert(true);

            return null;
        }
    }
}
