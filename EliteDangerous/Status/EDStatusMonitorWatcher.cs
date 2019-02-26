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

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Threading;

namespace EliteDangerousCore
{
    public class StatusMonitorWatcher
    {
        public Action<ConcurrentQueue<UIEvent>,string> UIEventCallBack;           // action passing event list.. in thread, not in UI
        public string WatcherFolder { get; set; }

        private string watchfile;
        private Thread ScanThread;
        private ManualResetEvent StopRequested;
        private int ScanRate;
        private ConcurrentQueue<UIEvent> Events = new ConcurrentQueue<UIEvent>();

        public StatusMonitorWatcher(string datapath, int srate)
        {
            WatcherFolder = datapath;
            ScanRate = srate;
            watchfile = Path.Combine(WatcherFolder, "status.json");
        }

        public void StartMonitor()
        {
            System.Diagnostics.Debug.WriteLine("Start Status Monitor on " + WatcherFolder);

            StopRequested = new ManualResetEvent(false);
            ScanThread = new Thread(ScanThreadProc) { Name = "Status.json Monitor Thread", IsBackground = true };
            ScanThread.Start();

        }
        public void StopMonitor()
        {
            if (ScanThread != null)
            {
                System.Diagnostics.Debug.WriteLine("Stop Status Monitor on " + WatcherFolder);
                StopRequested.Set();
                ScanThread.Join();
                StopRequested.Dispose();
                ScanThread = null;
            }
        }

        long? prev_flags = null;        // force at least one out here by invalid values
        int prev_guifocus = -1;                 
        int prev_firegroup = -1;
        double prev_curfuel = -1;
        double prev_curres = -1;
        int prev_cargo = -1;
        UIEvents.UIPips.Pips prev_pips = new UIEvents.UIPips.Pips();
        UIEvents.UIPosition.Position prev_pos = new UIEvents.UIPosition.Position();     // default is MinValue
        double prev_heading = double.MaxValue;    // this forces a pos report

        private enum StatusFlagsShip                        // PURPOSELY PRIVATE - don't want users to get into low level detail of BITS
        {
            Docked = 0, // (on a landing pad)
            Landed = 1, // (on planet surface)
            LandingGear = 2,
            Supercruise = 4,
            FlightAssist = 5,
            HardpointsDeployed = 6,
            InWing = 7,
            CargoScoopDeployed = 9,
            SilentRunning = 10,
            ScoopingFuel = 11,
            FsdMassLocked = 16,
            FsdCharging = 17,
            FsdCooldown = 18,
            OverHeating = 20,
            BeingInterdicted = 23,
            HUDInAnalysisMode = 27,     // 3.3
        }

        private enum StatusFlagsSRV
        {
            SrvHandbrake = 12,
            SrvTurret = 13,
            SrvUnderShip = 14,
            SrvDriveAssist = 15,
        }

        private enum StatusFlagsAll
        {
            ShieldsUp = 3,
            Lights = 8,
            LowFuel = 19,
            HasLatLong = 21,
            IsInDanger = 22,
            NightVision = 28,             // 3.3
        }

        private enum StatusFlagsShipType
        {
            InMainShip = 24,        // -> Degenerates to UIShipType
            InFighter = 25,
            InSRV = 26,
            ShipMask = (1<< InMainShip) | (1<< InFighter) | (1<< InSRV),
        }

        private void ScanThreadProc()
        {
            string prev_text = null;
            int nextpolltime = ScanRate;

            while (!StopRequested.WaitOne(nextpolltime))
            {
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount % 100000 + "Check " + watchfile);

                if (File.Exists(watchfile))
                {
                    nextpolltime = ScanRate;

                    JObject jo = null;

                    Stream stream = null;
                    try
                    {
                        stream = File.Open(watchfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        StreamReader reader = new StreamReader(stream);

                        string text = reader.ReadLine();

                        stream.Close();

                        if (text == null || (prev_text != null && text.Equals(prev_text)))        // if text the same, ignore
                            continue;

                        //System.Diagnostics.Debug.WriteLine("New status text " + text);

                        jo = (JObject)JObject.Parse(text);  // and of course the json could be crap

                        prev_text = text;       // set after successful parse
                    }
                    catch
                    { }
                    finally
                    {
                        if (stream != null)
                            stream.Dispose();
                    }

                    if (jo != null)
                    {
                        DateTime EventTimeUTC = DateTime.Parse(jo.Value<string>("timestamp"), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

                        List<UIEvent> events = new List<UIEvent>();

                        long curflags = (long)jo["Flags"].Long();

                        bool fireoverall = false;
                        bool fireoverallrefresh = prev_guifocus == -1;     //meaning its a refresh

                        if (prev_flags == null || curflags != prev_flags.Value)
                        {
                            if (prev_flags == null)
                                prev_flags = (long)StatusFlagsShipType.ShipMask;      // set an impossible ship type to start the ball rolling

                            UIEvents.UIShipType.Shiptype prevshiptype = ShipType(prev_flags.Value);
                            UIEvents.UIShipType.Shiptype curtype = ShipType(curflags);

                            bool refresh = prevshiptype == UIEvents.UIShipType.Shiptype.None;   // refresh if prev ship was none..

                            if (prevshiptype != curtype)
                            {
                                events.Add(new UIEvents.UIShipType(curtype, EventTimeUTC, refresh));        // CHANGE of ship
                                prev_flags = ~curflags;       // force re-reporting
                                refresh = true;
                            }

                            if (curtype == UIEvents.UIShipType.Shiptype.MainShip)
                                events.AddRange(ReportFlagState(typeof(StatusFlagsShip), curflags, prev_flags.Value, EventTimeUTC, refresh));
                            else if (curtype == UIEvents.UIShipType.Shiptype.SRV)
                                events.AddRange(ReportFlagState(typeof(StatusFlagsSRV), curflags, prev_flags.Value, EventTimeUTC, refresh));

                            if (curtype != UIEvents.UIShipType.Shiptype.None)
                                events.AddRange(ReportFlagState(typeof(StatusFlagsAll), curflags, prev_flags.Value, EventTimeUTC, refresh));

                            prev_flags = curflags;
                            fireoverall = true;
                        }

                        int curguifocus = (int)jo["GuiFocus"].Int();
                        if (curguifocus != prev_guifocus)
                        {
                            events.Add(new UIEvents.UIGUIFocus(curguifocus, EventTimeUTC, prev_guifocus == -1));
                            prev_guifocus = curguifocus;
                            fireoverall = true;
                        }

                        int[] pips = jo["Pips"]?.ToObjectProtected<int[]>();

                        if (pips != null)
                        {
                            double sys = pips[0] / 2.0;     // convert to normal, instead of half pips
                            double eng = pips[1] / 2.0;
                            double wep = pips[2] / 2.0;
                            if (sys != prev_pips.Systems || wep != prev_pips.Weapons || eng != prev_pips.Engines)
                            {
                                UIEvents.UIPips.Pips newpips = new UIEvents.UIPips.Pips() { Systems = sys, Engines = eng, Weapons = wep };
                                events.Add(new UIEvents.UIPips(newpips, EventTimeUTC, prev_pips.Engines<0));
                                prev_pips = newpips;
                                fireoverall = true;
                            }
                        }

                        int? curfiregroup = jo["FireGroup"].IntNull();      // may appear/disappear.

                        if (curfiregroup != null && curfiregroup != prev_firegroup)
                        {
                            events.Add(new UIEvents.UIFireGroup(curfiregroup.Value + 1, EventTimeUTC, prev_firegroup == -1));
                            prev_firegroup = curfiregroup.Value;
                            fireoverall = true;
                        }

                        JToken jfuel = (JToken)jo["Fuel"];

                        if (jfuel != null && jfuel.Type == JTokenType.Object)        // because they changed its type in 3.3.2
                        {
                            double? curfuel = jfuel["FuelMain"].DoubleNull();
                            double? curres = jfuel["FuelReservoir"].DoubleNull();
                            if (curfuel != null && curres != null && ( curfuel.Value != prev_curfuel || curres.Value != prev_curres) )
                            {
                                events.Add(new UIEvents.UIFuel(curfuel.Value,  curres.Value, ShipType(prev_flags.Value), EventTimeUTC, prev_firegroup == -1));
                                prev_curfuel = curfuel.Value;
                                prev_curres = curres.Value;
                                fireoverall = true;
                            }
                        }

                        int? curcargo = jo["Cargo"].IntNull();      // may appear/disappear and only introduced for 3.3
                        if (curcargo != null && curcargo.Value != prev_cargo)
                        {
                            events.Add(new UIEvents.UICargo(curcargo.Value, ShipType(prev_flags.Value), EventTimeUTC, prev_firegroup == -1));
                            prev_cargo = curcargo.Value;
                            fireoverall = true;
                        }

                        double jlat = jo["Latitude"].Double(double.MinValue);       // if not there, min value
                        double jlon = jo["Longitude"].Double(double.MinValue);
                        double jalt = jo["Altitude"].Double(double.MinValue);
                        double jheading = jo["Heading"].Double(double.MinValue);

                        if (jlat != prev_pos.Latitude || jlon != prev_pos.Longitude || jalt != prev_pos.Altitude || jheading != prev_heading)
                        {
                            UIEvents.UIPosition.Position newpos = new UIEvents.UIPosition.Position() { Latitude = jlat, Longitude = jlon, Altitude = jalt };
                            events.Add(new UIEvents.UIPosition(newpos, jheading, EventTimeUTC, jlat == double.MinValue));
                            prev_pos = newpos;
                            prev_heading = jheading;
                            fireoverall = true;
                        }

                        if ( fireoverall )
                        {
                            List<UITypeEnum> flagsset = ReportFlagState(typeof(StatusFlagsShip), curflags);
                            flagsset.AddRange(ReportFlagState(typeof(StatusFlagsSRV), curflags));
                            flagsset.AddRange(ReportFlagState(typeof(StatusFlagsAll), curflags));

                            events.Add(new UIEvents.UIOverallStatus(ShipType(curflags), flagsset, prev_guifocus, prev_pips, prev_firegroup, 
                                                                    prev_curfuel,prev_curres, prev_cargo, prev_pos, prev_heading,
                                                                    EventTimeUTC, fireoverallrefresh));        // overall list of flags set
                        }

                        if (events.Count > 0)
                        {
                            foreach (UIEvent e in events)
                            {
                                Events.Enqueue(e);
                            }

                            UIEventCallBack?.Invoke(Events, WatcherFolder);        // and fire..
                        }
                    }
                }
                else
                {
                    nextpolltime = ScanRate*40;           // if its not there, we are probably watching a non journal location.. so just do it occasionally
                }
            }
        }

        List<UITypeEnum> ReportFlagState(Type enumtype, long curflags)
        {
            List<UITypeEnum> flags = new List<UITypeEnum>();
            foreach (string n in Enum.GetNames(enumtype))
            {
                int v = (int)Enum.Parse(enumtype, n);

                bool flag = ((curflags >> v) & 1) != 0;
                if (flag)
                    flags.Add((UITypeEnum)Enum.Parse(typeof(UITypeEnum), n));
            }

            return flags;
        }

        List<UIEvent> ReportFlagState(Type enumtype, long curflags, long prev_flags, DateTime EventTimeUTC, bool refresh)
        {
            List<UIEvent> events = new List<UIEvent>();
            long delta = curflags ^ prev_flags;

            //System.Diagnostics.Debug.WriteLine("Flags changed to {0:x} from {1:x} delta {2:x}", curflags, prev_flags , delta);

            foreach (string n in Enum.GetNames(enumtype))
            {
                int v = (int)Enum.Parse(enumtype, n);

                bool flag = ((curflags >> v) & 1) != 0;

                if (((delta >> v) & 1) != 0)
                {
                    //  System.Diagnostics.Debug.WriteLine("..Flag " + n + " changed to " + flag);
                    events.Add(UIEvent.CreateFlagEvent(n, flag, EventTimeUTC, refresh));
                }
            }

            return events;
        }


        static public UIEvents.UIShipType.Shiptype ShipType(long shiptype)
        {
            shiptype &= (long)StatusFlagsShipType.ShipMask; // isolate flags

            var x = shiptype == 1L << (int)StatusFlagsShipType.InMainShip ? UIEvents.UIShipType.Shiptype.MainShip :
                                shiptype == 1L << (int)StatusFlagsShipType.InSRV ? UIEvents.UIShipType.Shiptype.SRV :
                                shiptype == 1L << (int)StatusFlagsShipType.InFighter ? UIEvents.UIShipType.Shiptype.Fighter :
                                UIEvents.UIShipType.Shiptype.None;
            return x;
        }

    }
}
