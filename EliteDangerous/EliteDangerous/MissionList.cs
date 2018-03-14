/*
 * Copyright © 2016 EDDiscovery development team
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

using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EliteDangerousCore
{
    [System.Diagnostics.DebuggerDisplay("Mission {Mission.Name} {State} {DestinationSystemStation()}")]
    public class MissionState
    {
        public JournalMissionAccepted Mission { get; private set; }                  // never null
        public JournalMissionCompleted Completed { get; private set; }               // null until complete
        public JournalMissionRedirected Redirected { get; private set; }             // null unless redirected
        public enum StateTypes { InProgress, Completed, Abandoned, Failed };
        public StateTypes State { get; private set; }
        public DateTime MissionEndTime;  // on Accepted, Expiry time, then actual finish time on Completed/Abandoned/Failed

        public bool InProgress { get { return (State == StateTypes.InProgress); } }
        public bool InProgressDateTime(DateTime compare) { return InProgress && DateTime.Compare(compare, Mission.Expiry)<0; }

        public string OriginatingSystem { get { return sys.Name; } }
        public string OriginatingStation { get { return body; } }

        public string DestinationSystemStation()        // allowing for redirection
        {
            return (Redirected != null) ? ("->" + Redirected.NewDestinationSystem.AppendPrePad(Redirected.NewDestinationStation, ":")) : Mission.DestinationSystem.AppendPrePad(Mission.DestinationStation, ":");
        }

        public ISystem sys;                                         // where it was found
        public string body;                                                         // and body

        public string Info()            // looking at state
        {
            return Mission.MissionInformation() + ((Completed != null) ? (Environment.NewLine + Completed.MissionInformation()) : "");
        }

        public string StateText
        {
            get
            {
                if (State != MissionState.StateTypes.Completed)
                    return State.ToString();
                else
                    return Completed.Value.ToString("N0");
            }
        }

        public long Value
        {
            get
            {
                if (State != MissionState.StateTypes.Completed)
                    return 0;
                else
                    return Completed.Value;
            }
        }

        public MissionState(JournalMissionAccepted m, ISystem s, string b)      // Start!
        {
            Mission = m;
            State = StateTypes.InProgress;
            MissionEndTime = m.Expiry;
            sys = s;
            body = b;
        }

        public MissionState(MissionState other, JournalMissionRedirected m)      // completed mission
        {
            Mission = other.Mission;
            Redirected = m;
            State = other.State;
            MissionEndTime = other.MissionEndTime;
            sys = other.sys;
            body = other.body;
        }

        public MissionState(MissionState other, JournalMissionCompleted m)      // completed mission
        {
            Mission = other.Mission;
            Completed = m;
            State = StateTypes.Completed;
            MissionEndTime = m.EventTimeUTC;
            sys = other.sys;
            body = other.body;
        }

        public MissionState(MissionState other, StateTypes type , DateTime? endtime )           // changed to another state..
        {
            Mission = other.Mission;
            State = type;
            MissionEndTime = (endtime != null) ? endtime.Value : other.MissionEndTime;
            sys = other.sys;
            body = other.body;
        }
    }

    public class MissionList
    {
        public Dictionary<string, MissionState> Missions { get; private set; }      // indiced by MissionUniqueID

        public MissionList()
        {
            Missions = new Dictionary<string, MissionState>();
        }

        public MissionList( MissionList other)
        {
            Missions = new Dictionary<string, MissionState>(other.Missions);
        }

        public void Add(JournalMissionAccepted m, ISystem sys, string body)
        {
            Missions[Key(m)] = new MissionState(m, sys, body); // add a new one..
        }

        public void Completed(JournalMissionCompleted c)
        {
            Missions[Key(c)] = new MissionState(Missions[Key(c)], c); // copy previous mission state, add completed
        }

        public void Abandoned(JournalMissionAbandoned a)
        {
            Missions[Key(a)] = new MissionState(Missions[Key(a)], MissionState.StateTypes.Abandoned, a.EventTimeUTC); // copy previous mission state, add abandonded
        }

        public void Failed(JournalMissionFailed f)
        {
            Missions[Key(f)] = new MissionState(Missions[Key(f)], MissionState.StateTypes.Failed , f.EventTimeUTC); // copy previous mission state, add failed
        }

        public void Redirected(JournalMissionRedirected r)
        {
            Missions[Key(r)] = new MissionState(Missions[Key(r)], r); // copy previous, add redirected
        }

        public List<MissionState> GetAllCombatMissionsLatestFirst() { return (from x in Missions.Values where x.Mission.TargetType.Length > 0 && x.Mission.ExpiryValid orderby x.Mission.EventTimeUTC descending select x).ToList(); }

        // can't think of a better way, don't want to put it in the actual entries since it should all be here.. can't be bothered to refactor so they have a common ancestor.
        public static string Key(JournalMissionFailed m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }
        public static string Key(JournalMissionCompleted m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }
        public static string Key(JournalMissionAccepted m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }
        public static string Key(JournalMissionRedirected m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }  
        public static string Key(JournalMissionAbandoned m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }
    }

    [System.Diagnostics.DebuggerDisplay("Total {current.Missions.Count}")]
    public class MissionListAccumulator
    {
        private MissionList current;

        public MissionListAccumulator()
        {
            current = new MissionList();
        }

        public void Accepted(JournalMissionAccepted m, ISystem sys, string body)
        {
            if (!current.Missions.ContainsKey(MissionList.Key(m)))        // make sure not repeating, ignore if so
            {
                current = new MissionList(current);     // shallow copy
                current.Add(m, sys, body);
            }
            else
                System.Diagnostics.Debug.WriteLine("Missions: Duplicate " + MissionList.Key(m));
        }

        public void Completed(JournalMissionCompleted m)
        {
            if (current.Missions.ContainsKey(MissionList.Key(m)))        // make sure not repeating, ignore if so
            {
                current = new MissionList(current);     // shallow copy
                current.Completed(m);
            }
            else
                System.Diagnostics.Debug.WriteLine("Missions: Unknown " + MissionList.Key(m));
        }

        public void Abandoned(JournalMissionAbandoned m)
        {
            if (current.Missions.ContainsKey(MissionList.Key(m)))        // make sure not repeating, ignore if so
            {
                current = new MissionList(current);     // shallow copy
                current.Abandoned(m);
            }
            else
                System.Diagnostics.Debug.WriteLine("Missions: Unknown " + MissionList.Key(m));
        }

        public void Failed(JournalMissionFailed m)
        {
            if (current.Missions.ContainsKey(MissionList.Key(m)))        // make sure not repeating, ignore if so
            {
                current = new MissionList(current);     // shallow copy
                current.Failed(m);
            }
            else
                System.Diagnostics.Debug.WriteLine("Missions: Unknown " + MissionList.Key(m));
        }

        public void Redirected(JournalMissionRedirected m)
        {
            if (current.Missions.ContainsKey(MissionList.Key(m)))        // make sure not repeating, ignore if so
            {
                current = new MissionList(current);     // shallow copy
                current.Redirected(m);
            }
            else
                System.Diagnostics.Debug.WriteLine("Missions: Unknown " + MissionList.Key(m));
        }

        #region process

        public MissionList Process(JournalEntry je, ISystem sys, string body , DB.SQLiteConnectionUser conn)
        {
            if (je is IMissions)
            {
                IMissions e = je as IMissions;
                e.UpdateMissions(this, sys , body, conn);                                   // not cloned.. up to callers to see if they need to
            }

            return current;
        }

        #endregion

    }
}

