﻿/*
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
        public enum StateTypes { InProgress, Completed, Abandoned, Failed, Died };    

        public JournalMissionAccepted Mission { get; private set; }                  // never null
        public JournalMissionCompleted Completed { get; private set; }               // null until completed properly, may be complete but with this missing
        public JournalMissionRedirected Redirected { get; private set; }             // null unless redirected
        public JournalCargoDepot CargoDepot { get; private set; }                    // null unless we received a CD on this mission

        public StateTypes State { get; private set; }
        public DateTime MissionEndTime { get; private set; }  // on Accepted, Expiry time, then actual finish time on Completed/Abandoned/Failed
        private ISystem sys;                                      // where it was found
        private string body;                                        // and body

        public bool InProgress { get { return (State == StateTypes.InProgress); } }
        public bool InProgressDateTime(DateTime compare) { return InProgress && DateTime.Compare(compare, Mission.Expiry)<0; }
        public int Id { get { return Mission.MissionId; } }                         // id of entry
        public string OriginatingSystem { get { return sys.Name; } }
        public string OriginatingStation { get { return body; } }

        public string DestinationSystemStation()        // allowing for redirection
        {
            return (Redirected != null) ? ("->" + Redirected.NewDestinationSystem.AppendPrePad(Redirected.NewDestinationStation, ":")) : Mission.DestinationSystem.AppendPrePad(Mission.DestinationStation, ":");
        }

        public string MissionInfoColumn()            // Missions panel uses this for info column
        {
            string info = Mission.MissionInfoColumn();

            if (CargoDepot != null)
            {
                info += Environment.NewLine + BaseUtils.FieldBuilder.Build("To Go:".T(EDTx.MissionState_ToGo), CargoDepot.ItemsToGo, "Progress:;%;N1".T(EDTx.MissionState_Progress), CargoDepot.ProgressPercent);
            }
            if (Completed != null)
            {
                info += Environment.NewLine + Completed.RewardInformation(true);
            }

            info = info.ReplaceIfEndsWith(Environment.NewLine, "");

            return info;
        }

        public string DLLInfo()    // DLL Uses this for mission info
        {
            string s = Mission.MissionBasicInfo(false) + "," + Mission.MissionDetailedInfo(false) + ", State: " + State.ToString();
            if (Completed != null)
                s += Environment.NewLine + Completed.RewardInformation(false);
            return s;
        }

        public string StateText
        {
            get
            {
                if (State != MissionState.StateTypes.Completed || Completed == null)
                    return State.ToString();
                else
                    return Completed.Value.ToString("N0");
            }
        }

        public long Value
        {
            get
            {
                if (State != MissionState.StateTypes.Completed || Completed == null)
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

        public MissionState(MissionState other, JournalMissionRedirected m)      // redirected mission
        {
            Mission = other.Mission;
            Redirected = m;                                                     // no completed, since we can't be
            CargoDepot = other.CargoDepot;

            State = other.State;
            MissionEndTime = other.MissionEndTime;
            sys = other.sys;
            body = other.body;
        }

        public MissionState(MissionState other, JournalMissionCompleted m)      // completed mission
        {
            Mission = other.Mission;
            Completed = m;                                                      // full set..
            Redirected = other.Redirected;
            CargoDepot = other.CargoDepot;

            State = StateTypes.Completed;
            MissionEndTime = m.EventTimeUTC;
            sys = other.sys;
            body = other.body;
        }

        public MissionState(MissionState other, JournalCargoDepot cd)           // cargo depot
        {
            Mission = other.Mission;
            Redirected = other.Redirected;                                      // no completed, since we can't be
            CargoDepot = cd;

            State = other.State;
            MissionEndTime = other.MissionEndTime;
            sys = other.sys;
            body = other.body;
        }


        public MissionState(MissionState other, StateTypes type, DateTime? endtime)           // changed to another state..
        {
            Mission = other.Mission;
            Redirected = other.Redirected;                                      // no completed, since we can't be - abandoned, failed, died, resurrected
            CargoDepot = other.CargoDepot;

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

        public void Accepted(JournalMissionAccepted m, ISystem sys, string body)
        {
            Missions[Key(m)] = new MissionState(m, sys, body); // add a new one..
        }

        public void Completed(JournalMissionCompleted c)
        {
            Missions[Key(c)] = new MissionState(Missions[Key(c)], c); // copy previous mission state, add completed
        }

        public void CargoDepot(string key, JournalCargoDepot cd)
        {
            Missions[key] = new MissionState(Missions[key], cd); // copy previous mission state, add completed
        }

        public void Abandoned(JournalMissionAbandoned a)
        {
            Missions[Key(a)] = new MissionState(Missions[Key(a)], MissionState.StateTypes.Abandoned, a.EventTimeUTC); // copy previous mission state, add abandonded
        }

        public void Failed(JournalMissionFailed f)
        {
            Missions[Key(f)] = new MissionState(Missions[Key(f)], MissionState.StateTypes.Failed, f.EventTimeUTC); // copy previous mission state, add failed
        }

        public void Redirected(JournalMissionRedirected r)
        {
            Missions[Key(r)] = new MissionState(Missions[Key(r)], r); // copy previous, add redirected
        }

        public void Died(DateTime diedtimeutc)
        {
            List<MissionState> affected = new List<MissionState>();
            foreach (var m in Missions)
            {
                if (m.Value.InProgressDateTime(diedtimeutc))
                    affected.Add(m.Value);
            }

            foreach (var m in affected)
                Missions[Key(m.Mission)] = new MissionState(Missions[Key(m.Mission)], MissionState.StateTypes.Died, diedtimeutc); // copy previous mission info, set died, now!
        }

        public void Resurrect(List<string> keys)
        {
            foreach( string k in keys)
            {
                if (Missions.ContainsKey(k))
                {
                    Missions[k] = new MissionState(Missions[k], MissionState.StateTypes.InProgress, null); // copy previous mission info, resurrected, now!
                }
            }
        }

        public void ChangeStateIfInProgress(List<string> keys, MissionState.StateTypes state, DateTime missingtime)
        {
            foreach (string k in keys)
            {
                if (Missions.ContainsKey(k) && Missions[k].State == MissionState.StateTypes.InProgress)
                {
                    Missions[k] = new MissionState(Missions[k], state, missingtime);
                }
            }
        }

        public void Disappeared(List<string> keys, DateTime missingtime)
        {
            foreach (string k in keys)
            {
                if (Missions.ContainsKey(k) && Missions[k].State == MissionState.StateTypes.InProgress)
                {
                    // permits seem to be only 1 journal entry.. so its completed.
                    MissionState.StateTypes st = Missions[k].Mission.Name.Contains("permit", StringComparison.InvariantCultureIgnoreCase) ? MissionState.StateTypes.Completed : MissionState.StateTypes.Abandoned;
                    Missions[k] = new MissionState(Missions[k], st, missingtime);
                }
            }
        }


        public List<MissionState> GetAllCombatMissionsLatestFirst() { return (from x in Missions.Values where x.Mission.TargetType.Length > 0 && x.Mission.ExpiryValid orderby x.Mission.EventTimeUTC descending select x).ToList(); }

        public List<MissionState> GetAllCurrentMissions(DateTime curtime) { return (from MissionState ms in Missions.Values where ms.InProgressDateTime(curtime) orderby ms.Mission.EventTimeUTC descending select ms).ToList(); }
        public List<MissionState> GetAllExpiredMissions(DateTime curtime) { return (from MissionState ms in Missions.Values where !ms.InProgressDateTime(curtime) orderby ms.Mission.EventTimeUTC descending select ms).ToList(); }

        // can't think of a better way, don't want to put it in the actual entries since it should all be here.. can't be bothered to refactor so they have a common ancestor.
        public static string Key(JournalMissionFailed m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }
        public static string Key(JournalMissionCompleted m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }
        public static string Key(JournalMissionAccepted m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }
        public static string Key(JournalMissionRedirected m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }
        public static string Key(JournalMissionAbandoned m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }
        public static string Key(int id, string name) { return id.ToStringInvariant() + ":" + name; }

        public string GetExistingKeyFromID(int id)       // some only have mission ID, generated after accept. Find on key
        {
            string frontpart = id.ToStringInvariant() + ":";
            foreach( var x in Missions )
            {
                if (x.Key.StartsWith(frontpart))
                    return x.Key;
            }

            return null;
        }
    }

    [System.Diagnostics.DebuggerDisplay("Total {current.Missions.Count}")]
    public class MissionListAccumulator
    {
        private MissionList missionlist;

        public MissionListAccumulator()
        {
            missionlist = new MissionList();
        }

        public void Accepted(JournalMissionAccepted m, ISystem sys, string body)
        {
            if (!missionlist.Missions.ContainsKey(MissionList.Key(m)))        // make sure not repeating, ignore if so
            {
                missionlist = new MissionList(missionlist);     // shallow copy
                missionlist.Accepted(m, sys, body);
              //  System.Diagnostics.Debug.WriteLine(m.EventTimeUTC.ToStringZulu() + " Missions Accepted " + MissionList.Key(m));
            }
            else
                System.Diagnostics.Debug.WriteLine("Missions: Duplicate " + MissionList.Key(m));
        }

        public void Completed(JournalMissionCompleted m)
        {
            if (missionlist.Missions.ContainsKey(MissionList.Key(m)))        // make sure not repeating, ignore if so
            {
                missionlist = new MissionList(missionlist);     // shallow copy
                missionlist.Completed(m);
               // System.Diagnostics.Debug.WriteLine(m.EventTimeUTC.ToStringZulu() + " Missions Completed " + MissionList.Key(m));
            }
            else
                System.Diagnostics.Debug.WriteLine("Missions: Unknown " + MissionList.Key(m));
        }

        public void Abandoned(JournalMissionAbandoned m)
        {
            if (missionlist.Missions.ContainsKey(MissionList.Key(m)))        // make sure not repeating, ignore if so
            {
                missionlist = new MissionList(missionlist);     // shallow copy
                missionlist.Abandoned(m);
              //  System.Diagnostics.Debug.WriteLine(m.EventTimeUTC.ToStringZulu() + " Missions Abandoned " + MissionList.Key(m));
            }
            else
                System.Diagnostics.Debug.WriteLine("Missions: Unknown " + MissionList.Key(m));
        }

        public void Failed(JournalMissionFailed m)
        {
            if (missionlist.Missions.ContainsKey(MissionList.Key(m)))        // make sure not repeating, ignore if so
            {
                missionlist = new MissionList(missionlist);     // shallow copy
                missionlist.Failed(m);
               // System.Diagnostics.Debug.WriteLine(m.EventTimeUTC.ToStringZulu() + " Missions Failed " + MissionList.Key(m));
            }
            else
                System.Diagnostics.Debug.WriteLine("Missions: Unknown " + MissionList.Key(m));
        }

        public void Redirected(JournalMissionRedirected m)
        {
            if (missionlist.Missions.ContainsKey(MissionList.Key(m)))        // make sure not repeating, ignore if so
            {
                missionlist = new MissionList(missionlist);     // shallow copy
                missionlist.Redirected(m);
               // System.Diagnostics.Debug.WriteLine(m.EventTimeUTC.ToStringZulu() + " Missions Redirected " + MissionList.Key(m));
            }
            else
                System.Diagnostics.Debug.WriteLine("Missions: Unknown " + MissionList.Key(m));
        }

        public void CargoDepot(JournalCargoDepot m)
        {
            string key = missionlist.GetExistingKeyFromID(m.MissionId);
            if ( key != null )
            { 
                missionlist = new MissionList(missionlist);     // shallow copy
                missionlist.CargoDepot(key,m);
              //  System.Diagnostics.Debug.WriteLine(m.EventTimeUTC.ToStringZulu() + " Cargo depot " + key);
            }
            else
                System.Diagnostics.Debug.WriteLine("Missions: Unknown " + m.MissionId);
        }

        public void Missions( JournalMissions m )
        {
            List<string> toresurrect = new List<string>();
            HashSet<string> active = new HashSet<string>();
            List<string> failed = new List<string>();
            List<string> completed = new List<string>();
            List<string> disappeared = new List<string>();

            foreach ( var mi in m.ActiveMissions )
            {
                string kn = MissionList.Key(mi.MissionID, mi.Name);
                active.Add(kn);
                //System.Diagnostics.Debug.WriteLine(m.EventTimeUTC.ToStringZulu() + " Mission " + kn + " is active");

                if (missionlist.Missions.ContainsKey(kn))
                {
                    MissionState ms = missionlist.Missions[kn];

                    if ( ms.State == MissionState.StateTypes.Died)  // if marked died... 
                    {
                        System.Diagnostics.Debug.WriteLine("Missions in active list but marked died" + kn);
                        toresurrect.Add(kn);
                    }
                }
            }

            foreach (var mi in m.FailedMissions)
            {
                string kn = MissionList.Key(mi.MissionID, mi.Name);
                failed.Add(kn);
            }

            foreach (var mi in m.CompletedMissions)
            {
                string kn = MissionList.Key(mi.MissionID, mi.Name);
                completed.Add(kn);
            }

            foreach (var kvp in missionlist.Missions)
            {
                if (!active.Contains(kvp.Key) && !failed.Contains(kvp.Key) && !completed.Contains(kvp.Key) && kvp.Value.State == MissionState.StateTypes.InProgress)
                {
                    System.Diagnostics.Debug.WriteLine(m.EventTimeUTC.ToStringZulu() + " Mission " + kvp.Value.Mission.EventTimeUTC.ToStringZulu() + " " + kvp.Key + " disappeared ****");
                    disappeared.Add(kvp.Key);
                }
            }

            if ( toresurrect.Count>0)       // if any..
            {
                missionlist = new MissionList(missionlist);     // shallow copy
                missionlist.Resurrect(toresurrect);
            }

            if (disappeared.Count > 0)
            {
                missionlist = new MissionList(missionlist);
                missionlist.Disappeared(disappeared, m.EventTimeUTC);
            }

            if (failed.Count > 0)
            {
                missionlist = new MissionList(missionlist);
                missionlist.ChangeStateIfInProgress(failed, MissionState.StateTypes.Failed, m.EventTimeUTC);
            }

            if (completed.Count > 0)
            {
                missionlist = new MissionList(missionlist);
                missionlist.ChangeStateIfInProgress(failed, MissionState.StateTypes.Completed, m.EventTimeUTC);
            }
        }

        public void Died(DateTime diedtime)
        {
            missionlist = new MissionList(missionlist);     // shallow copy
            missionlist.Died(diedtime);
        }

        #region process

        public MissionList Process(JournalEntry je, ISystem sys, string body )
        {
            if (je is IMissions)
            {
                IMissions e = je as IMissions;
                e.UpdateMissions(this, sys , body);                                   // not cloned.. up to callers to see if they need to
            }

            return missionlist;
        }

        #endregion

    }
}

