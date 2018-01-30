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
using System.Globalization;
using System.IO;
using System.Threading;

namespace EliteDangerousCore
{
    public class StatusMonitorWatcher
    {
        public Action<List<UIEvent>,string> UIEventCallBack;           // action passing event list.. in thread, not in UI

        public string watcherfolder;
        private string watchfile;
        private Thread ScanThread;
        private ManualResetEvent StopRequested;

        public StatusMonitorWatcher(string datapath)
        {
            watcherfolder = datapath;
            watchfile = Path.Combine(watcherfolder, "status.json");
        }

        public void StartMonitor()
        {
            System.Diagnostics.Debug.WriteLine("Start Status Monitor on " + watcherfolder);

            StopRequested = new ManualResetEvent(false);
            ScanThread = new Thread(ScanThreadProc) { Name = "Status.json Monitor Thread", IsBackground = true };
            ScanThread.Start();

        }
        public void StopMonitor()
        {
            System.Diagnostics.Debug.WriteLine("Stop Status Monitor on " + watcherfolder);
            StopRequested.Set();
            ScanThread.Join();
            StopRequested.Dispose();
            ScanThread = null;
        }

        long? prev_flags = null;        // force at least one out here by invalid values
        int prev_guifocus = -1;                 
        int prev_firegroup = -1;                
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
            int nextpolltime = 250;

            while (!StopRequested.WaitOne(nextpolltime))
            {
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount % 100000 + "Check " + watchfile);

                if (File.Exists(watchfile))
                {
                    nextpolltime = 250;

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

                        if (prev_flags == null || curflags != prev_flags.Value)
                        {
                            if (prev_flags == null)
                                prev_flags = (long)StatusFlagsShipType.ShipMask;      // set an impossible ship type to start the ball rolling

                            long shiptype = curflags & (long)StatusFlagsShipType.ShipMask;
                            long prevshiptype = prev_flags.Value & (long)StatusFlagsShipType.ShipMask;

                            if (shiptype != prevshiptype)
                            {
                                UIEvents.UIShipType.Shiptype t = shiptype == 1L<<(int)StatusFlagsShipType.InMainShip ? UIEvents.UIShipType.Shiptype.MainShip :
                                                                 shiptype == 1L<<(int)StatusFlagsShipType.InSRV ? UIEvents.UIShipType.Shiptype.SRV :
                                                                 shiptype == 1L<<(int)StatusFlagsShipType.InFighter ? UIEvents.UIShipType.Shiptype.Fighter :
                                                                 UIEvents.UIShipType.Shiptype.None;

                                events.Add(new UIEvents.UIShipType(t, EventTimeUTC));        // CHANGE of ship
                                prev_flags = ~curflags;       // force re-reporting
                            }

                            if (shiptype == (long)StatusFlagsShipType.InMainShip)
                                events.AddRange(ReportFlagState(typeof(StatusFlagsShip), curflags, prev_flags.Value, EventTimeUTC));
                            else if (shiptype == (long)StatusFlagsShipType.InSRV)
                                events.AddRange(ReportFlagState(typeof(StatusFlagsSRV), curflags, prev_flags.Value, EventTimeUTC));

                            if ( shiptype != 0 )    // not none
                                events.AddRange(ReportFlagState(typeof(StatusFlagsAll), curflags, prev_flags.Value, EventTimeUTC));

                            prev_flags = curflags;
                        }

                        int curguifocus = (int)jo["GuiFocus"].Int();
                        if (curguifocus != prev_guifocus)
                        {
                            events.Add(new UIEvents.UIGUIFocus(curguifocus, EventTimeUTC));
                            prev_guifocus = curguifocus;
                        }

                        int[] pips = jo["Pips"]?.ToObject<int[]>();

                        if (pips != null)
                        {
                            double sys = pips[0] / 2.0;     // convert to normal, instead of half pips
                            double eng = pips[1] / 2.0;
                            double wep = pips[2] / 2.0;
                            if (sys != prev_pips.Systems || wep != prev_pips.Weapons || eng != prev_pips.Engines)
                            {
                                UIEvents.UIPips.Pips newpips = new UIEvents.UIPips.Pips() { Systems = sys, Engines = eng, Weapons = wep };
                                events.Add(new UIEvents.UIPips(newpips, EventTimeUTC));
                                prev_pips = newpips;
                            }
                        }

                        int? curfiregroup = jo["FireGroup"].IntNull();      // may appear/disappear.

                        if (curfiregroup != null && curfiregroup != prev_firegroup)
                        {
                            events.Add(new UIEvents.UIFireGroup(curfiregroup.Value + 1, EventTimeUTC));
                            prev_firegroup = curfiregroup.Value;
                        }

                        double jlat = jo["Latitude"].Double(double.MinValue);       // if not there, min value
                        double jlon = jo["Longitude"].Double(double.MinValue);
                        double jalt = jo["Altitude"].Double(double.MinValue);
                        double jheading = jo["Heading"].Double(double.MinValue);

                        if (jlat != prev_pos.Latitude || jlon != prev_pos.Longitude || jalt != prev_pos.Altitude || jheading != prev_heading)
                        {
                            UIEvents.UIPosition.Position newpos = new UIEvents.UIPosition.Position() { Latitude = jlat, Longitude = jlon, Altitude = jalt };
                            events.Add(new UIEvents.UIPosition(newpos, jheading, EventTimeUTC));
                            prev_pos = newpos;
                            prev_heading = jheading;
                        }

                        if (events.Count > 0)
                            UIEventCallBack?.Invoke(events, watcherfolder);        // and fire..
                    }
                }
                else
                {
                    nextpolltime = 10000;           // if its not there, we are probably watching a non journal location.. so just do it occasionally
                }
            }
        }

        List<UIEvent> ReportFlagState(Type enumtype, long curflags, long prev_flags, DateTime EventTimeUTC)
        {
            List<UIEvent> events = new List<UIEvent>();
            long delta = curflags ^ prev_flags;

            //System.Diagnostics.Debug.WriteLine("Flags changed to {0:x} from {1:x} delta {2:x}", curflags, prev_flags , delta);

            foreach (string n in Enum.GetNames(enumtype))
            {
                int v = (int)Enum.Parse(enumtype, n);

                if (((delta >> v) & 1) != 0)
                {
                    bool flag = ((curflags >> v) & 1) != 0;
                    //System.Diagnostics.Debug.WriteLine("..Flag " + n + " changed to " + flag);
                    events.Add(UIEvent.CreateFlagEvent(n, flag, EventTimeUTC));
                }
            }

            return events;
        }
    }
}
