using EDDiscovery.EliteDangerous.JournalEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
{
    [System.Diagnostics.DebuggerDisplay("Mission {Mission.Name} {InProgress}")]
    public class MissionState
    {
        public JournalMissionAccepted Mission { get; private set; }                  // never null
        public JournalMissionCompleted Completed { get; private set; }               // null until complete
        public enum StateTypes { InProgress, Completed, Abandoned, Failed };
        public StateTypes State { get; private set; }     

        public bool InProgress { get { return State == StateTypes.InProgress; } }
        public bool InProgressDateTime(DateTime compare) { return State == StateTypes.InProgress && DateTime.Compare(compare, Mission.Expiry)<0; }

        public string OriginatingSystem { get { return sys.name; } }
        public string OriginatingStation { get { return body; } }

        public EDDiscovery2.DB.ISystem sys;                                         // where it was found
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
                    return Completed.RewardOrDonation + "cr";
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

        public MissionState(JournalMissionAccepted m, EDDiscovery2.DB.ISystem s, string b)
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

        public void Add(JournalMissionAccepted m, EDDiscovery2.DB.ISystem sys, string body)
        {
            Missions[Key(m)] = new MissionState(m, sys, body); // add a new one..
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

        // can't think of a better way, don't want to put it in the actual entries since it should all be here.. can't be bothered to refactor so they have a common ancestor.
        public static string Key(JournalMissionFailed m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }
        public static string Key(JournalMissionCompleted m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }
        public static string Key(JournalMissionAccepted m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }
        public static string Key(JournalMissionAbandoned m) { return m.MissionId.ToStringInvariant() + ":" + m.Name; }
    }

    [System.Diagnostics.DebuggerDisplay("Total {Missions.Count}")]
    public class MissionListAccumulator
    {
        private MissionList current;

        public MissionListAccumulator()
        {
            current = new MissionList();
        }

        public void Accepted(JournalMissionAccepted m, EDDiscovery2.DB.ISystem sys, string body)
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

        #region process

        public MissionList Process(JournalEntry je, EDDiscovery2.DB.ISystem sys, string body , DB.SQLiteConnectionUser conn)
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

