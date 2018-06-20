using BaseUtils;
using Conditions;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            public StandardTrigger(string n, string c ) { Name = n; Condition = c; }
            public string Name;
            public string Condition;
        }

        static public StandardTrigger[] StandardTriggers = new StandardTrigger[] 
        {
            new StandardTrigger("No Trigger","Condition AlwaysFalse"),      // always the first.
            new StandardTrigger("Docked","TriggerName $== Docked"),
        };

        static public int FindTriggerIndex(ConditionLists cond )
        {
            return Array.FindIndex(StandardTriggers, x => x.Condition.Equals(cond.ToString()));
        }

        static public int NoTriggerIndex = 0;

        public class Profile
        {
            public int Id;
            public string Name;
            public ConditionLists Condition;

            public Profile(int internalnumber, string name, string condition)
            {
                Id = internalnumber;
                Name = name;
                Condition = new Conditions.ConditionLists();
                Condition.Read(condition);
                if (Condition.Count == 0)
                {
                    Condition.Read("Condition AlwaysFalse");
                }
            }
        }

        private static EDDProfiles instance;

        public List<Profile> ProfileList { get; private set; }
        public int Count { get { return ProfileList.Count; } }
        public Profile Current { get; private set; }
        public List<string> Names() { return ProfileList.Select(x => x.Name).ToList(); }
        public int IndexOfCurrent() { return ProfileList.FindIndex(x => x.Id == Current.Id); }

        public string UserControlsPrefix { get { return Current.Id == 0 ? "" : ("Profile_" + Current.Id + "_"); } }

        static public string ProfilePrefix(int id) { return "Profile_" + id + "_"; }

        public void LoadProfiles()
        {
            string profiles = SQLiteConnectionUser.GetSettingString("ProfileIDs", "0");
            List<int> profileints = profiles.RestoreIntListFromString(1,0); // default is length 1, value 0

            foreach (int profileid in profileints)
            {
                StringParser sp = new StringParser(SQLiteConnectionUser.GetSettingString(ProfilePrefix(profileid) + "Settings", "Default,Condition AlwaysFalse"));

                string name = sp.NextQuotedWordComma();
                string condition = sp.NextQuotedWord();

                if (name != null && condition != null)
                {
                    Profile p = new Profile(profileid, name, condition);
                    ProfileList.Add(p);
                }
                else
                    break;
            }

            int curid = SQLiteConnectionUser.GetSettingInt("ProfileCurrentID", 0);

            Current = ProfileList.Find(x => x.Id == curid) ?? ProfileList[0];
        }

        public void SaveProfiles()
        {
            string ids = "";
            foreach (Profile p in ProfileList)
            {
                string idstr = p.Id.ToStringInvariant();
                SQLiteConnectionUser.PutSettingString(ProfilePrefix(p.Id) + "Settings",
                            p.Name.QuoteString(comma: true) + "," +
                            p.Condition.ToString().QuoteString(comma: true));
                ids = ids.AppendPrePad(idstr, ",");

            }
            SQLiteConnectionUser.PutSettingString("ProfileIDs", ids);
            SQLiteConnectionUser.PutSettingInt("ProfileCurrentID", Current.Id);
        }

        public bool UpdateProfiles(List<Profile> newset )        // true reload
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
                    p.Condition = c.Condition;
               }
            }

            bool removedcurrent = false;

            foreach (Profile p in toberemoved)
            {
                if (Object.ReferenceEquals(Current, p))
                    removedcurrent = true;

                System.Diagnostics.Debug.WriteLine("Delete ID " + p.Id);
                SQLiteConnectionUser.DeleteKey(ProfilePrefix(p.Id) + "%");       // all profiles string
                ProfileList.Remove(p);

            }

            int[] curids = (from x in ProfileList select x.Id).ToArray();

            foreach ( Profile p in newset.Where((p)=>p.Id==-1))
            {
                int id = 1;
                for(; id <10000; id++ )
                {
                    if (Array.IndexOf(curids, id) == -1)
                        break;
                }

                System.Diagnostics.Debug.WriteLine("Make ID " + id);
                p.Id = id;
                ProfileList.Add(p);
            }

            if ( removedcurrent )
                Current = ProfileList[0];

            SaveProfiles();

            return removedcurrent;
        }

        public bool ChangeCurrent(int indexof)
        {
            if (indexof >= 0 && indexof < ProfileList.Count && ProfileList.IndexOf(Current) != indexof)
            {
                Current = ProfileList[indexof];
                return true;
            }
            else
                return false;
        }

    }
}
