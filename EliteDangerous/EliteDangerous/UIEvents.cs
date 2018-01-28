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
        InMainShip , 
        InFighter , 
        InSRV ,
    }

    public enum StatusFlags
    {
        Docked = 0, // (on a landing pad)
        Landed = 1, // (on planet surface)
        LandingGear = 2,
        ShieldsUp = 3,
        Supercruise = 4,
        FlightAssist = 5,
        HardpointsDeployed = 6,
        InWing = 7,
        Lights = 8,
        CargoScoopDeployed = 9,
        SilentRunning = 10,
        ScoopingFuel = 11,
        SrvHandbrake = 12,
        SrvTurret = 13,
        SrvUnderShip = 14,
        SrvDriveAssist = 15,
        FsdMassLocked = 16,
        FsdCharging = 17,
        FsdCooldown = 18,
        LowFuel = 19,
        OverHeating = 20,
        HasLatLong = 21,
        IsInDanger = 22,
        BeingInterdicted = 23,
        InMainShip = 24,
        InFighter = 25,
        InSRV = 26,
    }

    public abstract class UIEvent
    {
        public UIEvent(UITypeEnum t, DateTime time, long totalflags = 0)
        {
            EventTypeID = t;
            EventTimeUTC = time;
            FlagState = totalflags;
        }

        public DateTime EventTimeUTC { get; set; }
        public UITypeEnum EventTypeID { get; set; }             // name of event. 
        public string EventTypeStr { get { return EventTypeID.ToString(); } }
        public long FlagState { get; set; }                   // Flag state total at this event time, if you want to use it

        static string JournalRootClassname = typeof(UIEvents.UIDocked).Namespace;        // pick one at random to find out root classname

        // Flag Factory (others are created individually)

        static public UIEvent CreateFlagEvent(string name, bool value, DateTime time, long totalflags)
        {
            string evname = "UI" + name;
            Type t = Type.GetType(JournalRootClassname + "." + evname, false, true);
            if (t != null)
            {
                UIEvent e = (UIEvent)Activator.CreateInstance(t, new Object[] { value, time });
                e.FlagState = totalflags;
                return e;
            }
            else
                System.Diagnostics.Debug.Assert(true);

            return null;
        }
    }
}
