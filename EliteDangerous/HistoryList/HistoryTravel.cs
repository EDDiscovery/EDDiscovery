/*
 * Copyright © 2016 - 2019 EDDiscovery development team
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
using EliteDangerousCore.JournalEvents;

namespace EliteDangerousCore
{
    public class HistoryTravelStatus
    {
        public double TravelledDistance { get; private set; }
        public TimeSpan TravelledSeconds { get; private set; }
        public bool IsTravelling { get; private set; }
        public bool IsShutDown { get; private set; }
        public int TravelledMissingjump { get; private set; }
        public int Travelledjumps { get; private set; }

        public string TravelledJumpsAndMisses { get { return Travelledjumps.ToStringInvariant() + ((TravelledMissingjump > 0) ? (" (" + TravelledMissingjump.ToStringInvariant() + ")") : ""); } }

        public string ToString(string prefix)
        {
            if (IsTravelling)
            {
                return prefix + " " + TravelledDistance.ToStringInvariant("0.0") + " LY"
                                 + ", " + Travelledjumps + " jumps"
                                 + ((TravelledMissingjump > 0) ? ", " + TravelledMissingjump + " unknown distance jumps" : "") +
                                  ", time " + TravelledSeconds;
            }
            else
                return "";
        }

        public HistoryTravelStatus()
        {
        }

        public HistoryTravelStatus(HistoryTravelStatus prevstatus)
        {
            TravelledDistance = prevstatus.TravelledDistance;
            TravelledSeconds = prevstatus.TravelledSeconds;
            TravelledMissingjump = prevstatus.TravelledMissingjump;
            Travelledjumps = prevstatus.Travelledjumps;

            IsShutDown = false; // can't be shutdown if copied..
            IsTravelling = true; // must be travelling
        }

        // previous and heprev can be null, hecur is always set

        public static HistoryTravelStatus Update(HistoryTravelStatus previous, HistoryEntry heprev, HistoryEntry hecur)
        {
            if (previous == null || (heprev?.StopMarker??false))       // if prev is null (start of list) OR previous ordered a stop, new list
            {
                previous = new HistoryTravelStatus();
            }

            if (previous.IsTravelling == false && hecur.StartMarker == false)    // if not travelling, and not ordered a start, we are in inert, return previous
                return previous;

            if (previous.IsShutDown)                
            {
                if (hecur.EntryType != JournalTypeEnum.LoadGame)     // in shutdown, and not loadgame, no counting, return previous
                    return previous;
            }

            TimeSpan diff = hecur.EventTimeUTC.Subtract(heprev?.EventTimeUTC ?? hecur.EventTimeUTC);        // heprev can be null, cope.

            if (hecur.EntryType == JournalTypeEnum.Fileheader && diff >= new TimeSpan(0, 30, 0))       // if we have a load game, and previous one was > X, its a game gap
                return previous;            // older logs did not have the shutdown event.

            HistoryTravelStatus cur = new HistoryTravelStatus(previous);    // fresh entry, not shutdown, travelling

            if ( !hecur.MultiPlayer )           // multiplayer bits don't count, but we need a fresh entry because we are now monitoring for shutdown.
            { 
                if (hecur.IsFSDJump )   // if jump, and not multiplayer..
                {
                    double dist = ((JournalFSDJump)hecur.journalEntry).JumpDist;
                    if (dist <= 0)
                        cur.TravelledMissingjump++;
                    else
                    {
                        cur.TravelledDistance += dist;
                        cur.Travelledjumps++;
                    }
                }

                cur.TravelledSeconds += diff;
            }

            if (hecur.EntryType == JournalTypeEnum.Shutdown)            // if into a shutdown, note
                cur.IsShutDown = true;

            return cur;
        }
    }
}
