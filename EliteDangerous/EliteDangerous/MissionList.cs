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

namespace EliteDangerousCore
{
    [System.Diagnostics.DebuggerDisplay("Mission {Mission.Name} {InProgress}")]
    public class MissionState
    {
        public JournalMissionAccepted Mission { get; private set; }                  // never null
        public JournalMissionCompleted Completed { get; private set; }               // null until complete
        public enum StateTypes { InProgress, Completed, Abandoned, Failed, Redirected };
        public StateTypes State { get; private set; }     

        public bool InProgress { get { return (State == StateTypes.InProgress || State == StateTypes.Redirected); } }
        public bool InProgressDateTime(DateTime compare) { return InProgress && DateTime.Compare(compare, Mission.Expiry)<0; }

        public string OriginatingSystem { get { return sys.Name; } }
        public string OriginatingStation { get { return body; } }

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

        public MissionState(JournalMissionAccepted m, ISystem s, string b)
        {
            Mission = m;
            sys = s;
            body = b;
            State = StateTypes.InProgress;
        }

        public MissionState(MissionState other, JournalMissionCompleted m)
        {
            Mission = other.Mission;
            sys = other.sys;
            body = other.body;
            Completed = m;
            State = StateTypes.Completed;
        }

        public MissionState(MissionState other, StateTypes type )
        {
            Mission = other.Mission;
            sys = other.sys;
            body = other.body;
            State = type;
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

        public void Redirected(JournalMissionRedirected m, ISystem sys, string body)
        {
            // Update State with new info...     TODO
            //Missions[Key(m)] = new MissionState(m, sys, body); // add a new one..
        }


        public void Completed(JournalMissionCompleted m)
        {
            Missions[Key(m)] = new MissionState(Missions[Key(m)], m); // copy previous mission state, add completed
        }

        public void Abandoned(JournalMissionAbandoned m)
        {
            Missions[Key(m)] = new MissionState(Missions[Key(m)], MissionState.StateTypes.Abandoned); // copy previous mission state, add abandonded
        }

        public void Failed(JournalMissionFailed m)
        {
            Missions[Key(m)] = new MissionState(Missions[Key(m)], MissionState.StateTypes.Failed); // copy previous mission state, add failed
        }
        public void Redirected(JournalMissionRedirected m)
        {
            Missions[Key(m)] = new MissionState(Missions[Key(m)], MissionState.StateTypes.Redirected); // copy previous mission state, add failed
            // Todo  update destination....
            //Missions[Key(m)] = new MissionState(Missions[Key(m)], MissionState.StateTypes.Failed); // copy previous mission state, add failed
        }


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

