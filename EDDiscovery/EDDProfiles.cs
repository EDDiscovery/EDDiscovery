using BaseUtils;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDDiscovery
{
    public class EDDProfiles
    {
        public static EDDProfiles Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EDDProfiles();
                }
                return instance;
            }
        }

        private EDDProfiles()
        {
            ProfileList = new List<Profile>();
        }

        public class StandardTrigger
        {
            public StandardTrigger(string n, string c, string b ) { Name = n; TripCondition = c; BackCondition = b; }
            public string Name;
            public string TripCondition;
            public string BackCondition;
        }

        static public StandardTrigger[] StandardTriggers = new StandardTrigger[] 
        {
            new StandardTrigger("No Trigger","Condition AlwaysFalse","Condition AlwaysFalse"),      // always the first.
            new StandardTrigger("Normal Space","TriggerName $== UnDocked Or TriggerName $== LeaveBody Or TriggerName $== StartJump","Condition AlwaysFalse"),
            new StandardTrigger("Planet","TriggerName $== ApproachBody","Condition AlwaysFalse"),
            new StandardTrigger("Docked","TriggerName $== Docked","Condition AlwaysFalse"),
            new StandardTrigger("Undocked","TriggerName $== Undocked","Condition AlwaysFalse"),
            new StandardTrigger("Hardpoints","TriggerName $== UIHardpointsDeployed And EventClass_Deployed IsTrue","TriggerName $== UIHardpointsDeployed And EventClass_Deployed IsFalse"),
            new StandardTrigger("SRV","TriggerName $== LaunchSRV","TriggerName $== SRVDestroyed Or TriggerName $== DockSRV"),
            new StandardTrigger("Fighter","TriggerName $== LaunchFighter And PlayerControlled IsTrue","TriggerName $== DockFighter"),
        };

        static public int FindTriggerIndex(ConditionLists trigger, ConditionLists back )
        {
            return Array.FindIndex(StandardTriggers, x => x.TripCondition.Equals(trigger.ToString()) && x.BackCondition.Equals(back.ToString()));
        }

        static public int NoTriggerIndex = 0;

        static public int DefaultId = 100;          // 100, so you can tell index from IDs during debugging

        public class Profile
        {
            public int Id;
            public string Name;
            public ConditionLists TripCondition;
            public ConditionLists BackCondition;

            public Profile(int internalnumber, string name, string tripcondition, string backcondition)
            {
                Id = internalnumber;
                Name = name;
                TripCondition = new BaseUtils.ConditionLists();
                TripCondition.Read(tripcondition);
                if (TripCondition.Count == 0)
                {
                    TripCondition.Read("Condition AlwaysFalse");
                }
                BackCondition = new BaseUtils.ConditionLists();
                BackCondition.Read(backcondition);
                if (BackCondition.Count == 0)
                {
                    BackCondition.Read("Condition AlwaysFalse");
                }
            }
        }

        private static EDDProfiles instance;

        public List<Profile> ProfileList { get; private set; }
        public int Count { get { return ProfileList.Count; } }
        public Profile PowerOn { get; private set; }
        public Profile Current { get; private set; }
        public List<string> Names() { return ProfileList.Select(x => x.Name).ToList(); }

        public int IndexOf(int id) { return ProfileList.FindIndex(x => x.Id == id); }
        public int IdOfIndex(int index) { return ProfileList[index].Id; }

        public Stack<int> History { get; private set; } = new Stack<int>();

        public string UserControlsPrefix { get { return Current.Id == DefaultId ? "" : ("Profile_" + Current.Id + "_"); } }

        static public string ProfilePrefix(int id) { return "Profile_" + id + "_"; }

        public void LoadProfiles(string selectprofile)
        {
            string profiles = UserDatabase.Instance.GetSettingString("ProfileIDs", "0");
            List<int> profileints = profiles.RestoreIntListFromString(1,0); // default is length 1, value 0

            foreach (int profileid in profileints)
            {
                StringParser sp = new StringParser(UserDatabase.Instance.GetSettingString(ProfilePrefix(profileid) + "Settings", ""));

                string name = sp.NextQuotedWordComma();
                string tripcondition = sp.NextQuotedWordComma();
                string backcondition = sp.NextQuotedWord();

                if (name != null && tripcondition != null && backcondition != null)
                {
                    Profile p = new Profile(profileid, name, tripcondition, backcondition);
                    System.Diagnostics.Debug.WriteLine("Profile {0} {1} {2}", name, tripcondition, backcondition);
                    ProfileList.Add(p);
                }
            }

            if ( ProfileList.Count == 0 )
            {
                ProfileList.Add(new Profile(DefaultId, "Default", "Condition AlwaysFalse", "Condition AlwaysFalse"));
            }

            int curid = UserDatabase.Instance.GetSettingInt("ProfilePowerOnID", DefaultId);

            if ( selectprofile != null )
            {
                int found = ProfileList.FindIndex(x => x.Name.Equals(selectprofile, StringComparison.InvariantCultureIgnoreCase));
                if (found >= 0)
                    curid = ProfileList[found].Id;
            }

            PowerOn = Current = ProfileList.Find(x => x.Id == curid) ?? ProfileList[0];
            History.Push(Current.Id);
        }

        private void SaveProfiles()
        {
            string ids = "";
            foreach (Profile p in ProfileList)
            {
                string idstr = p.Id.ToStringInvariant();
                UserDatabase.Instance.PutSettingString(ProfilePrefix(p.Id) + "Settings",
                            p.Name.QuoteString(comma: true) + "," +
                            p.TripCondition.ToString().QuoteString(comma: true) + "," + p.BackCondition.ToString().QuoteString(comma: true)
                            );
                ids = ids.AppendPrePad(idstr, ",");
            }

            UserDatabase.Instance.PutSettingString("ProfileIDs", ids);
        }

        public bool UpdateProfiles(List<Profile> newset, int poweronindex )        // true reload - Current is invalid if true, must reload to new profile
        {
            List<Profile> toberemoved = new List<Profile>();

            foreach (Profile p in ProfileList)
            {
                Profile c = newset.Find(x => x.Id == p.Id);

                if ( c == null )       // if newset does not have this profile ID
                {
                    toberemoved.Add(p);
                }
                else
                {                           // existing in both, update
                    System.Diagnostics.Debug.WriteLine("Update ID " + p.Id);
                    p.Name = c.Name;        // update name and condition
                    p.TripCondition = c.TripCondition;
                    p.BackCondition = c.BackCondition;
               }
            }

            bool removedcurrent = false;

            foreach (Profile p in toberemoved)
            {
                if (Object.ReferenceEquals(Current, p))
                    removedcurrent = true;

                System.Diagnostics.Debug.WriteLine("Delete ID " + p.Id);
                UserDatabase.Instance.DeleteKey(ProfilePrefix(p.Id) + "%");       // all profiles string
                ProfileList.Remove(p);
            }

            foreach ( Profile p in newset.Where((p)=>p.Id==-1))
            {
                int[] curids = (from x in ProfileList select x.Id).ToArray();

                int id = DefaultId+1;
                for(; id <10000; id++ )
                {
                    if (Array.IndexOf(curids, id) == -1)
                        break;
                }

                System.Diagnostics.Debug.WriteLine("Make ID " + id);
                p.Id = id;
                ProfileList.Add(p);
            }

            poweronindex = poweronindex >= 0 ? poweronindex : 0;
            UserDatabase.Instance.PutSettingInt("ProfilePowerOnID", ProfileList[poweronindex].Id);
            PowerOn = ProfileList[poweronindex];

            SaveProfiles();
            History.Clear();        // because an ID may have gone awol

            return removedcurrent;
        }

        public bool ChangeToId(int id)
        {
            int indexof = ProfileList.FindIndex(x => x.Id == id);

            if (indexof >= 0 && id != Current.Id )
            {
                Current = ProfileList[indexof];
                History.Push(Current.Id);
                return true;
            }
            else
                return false;
        }

        public int ActionOn(Variables vars, out string errlist)        // -1 no change, else id of new profile
        {
            errlist = string.Empty;

            //System.Diagnostics.Debug.WriteLine("Profile check on " + vars.ToString(separ: Environment.NewLine));

            Functions functions = new Functions(vars, null);

            foreach (Profile p in ProfileList)
            {
                bool? condres = p.TripCondition.CheckAll(vars, out string err, null, functions);     // may return null.. and will return errlist

                if (err == null)
                {
                    bool res = condres.HasValue && condres.Value;

                    if (res)
                    {
                        System.Diagnostics.Debug.WriteLine("Profile " + p.Name + " Tripped due to " + p.TripCondition.ToString());
                        return p.Id;
                    }
                }
                else
                    errlist = errlist.AppendPrePad(err, ",");
            }

            bool? backres = Current.BackCondition.CheckAll(vars, out string err2, null, functions);     // check the back condition on the current profile..

            if (err2 == null)
            {
                bool res = backres.HasValue && backres.Value;

                if (res)
                {
                    System.Diagnostics.Debug.WriteLine("Profile " + Current.Name + " Back Tripped due to " + Current.BackCondition.ToString());

                    if ( History.Count>=2)       // we may have an empty history (because its been erased due to editing) or only a single entry (ours).. so just double check
                    {
                        History.Pop();  // pop us
                        return History.Pop();       // and return ID of 1 before to use
                    }
                    else
                    {       // not an error, just a state of being ;-)
                        System.Diagnostics.Debug.WriteLine("Profile " + Current.Name + " Back Tripped but no history to go back to!");
                    }
                }
            }
            else
                errlist = errlist.AppendPrePad(err2, ",");

            return -1;
        }

    }
}
