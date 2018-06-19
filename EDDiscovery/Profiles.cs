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
    public class Profiles
    {
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
            public string Name;
            public ConditionLists Condition;

            public Profile(string name, string condition)
            {
                Name = name;
                Condition = new Conditions.ConditionLists();
                Condition.Read(condition);
                if (Condition.Count == 0)
                {
                    Condition.Read("Condition AlwaysFalse");
                }
            }
        }

        public List<Profile> ProfileList { get; private set; }

        public int Count { get { return ProfileList.Count; } }

        public List<string> Names() { return ProfileList.Select(x => x.Name).ToList();  }

        public Profiles()
        {
            ProfileList = new List<Profile>();
        }

        public void LoadProfiles()
        {
            ProfileList = new List<Profile>();

            for (int i = 0; ; i++)
            {
                StringParser sp = new StringParser(SQLiteConnectionUser.GetSettingString("ProfileList" + i.ToStringInvariant(), ""));

                string name = sp.NextQuotedWordComma();
                string condition = sp.NextQuotedWord();

                if (name != null && condition != null)
                {
                    Profile p = new Profile(name, condition);
                    ProfileList.Add(p);
                }
                else
                    break;
            }

            if (ProfileList.Count == 0)
            {
                ProfileList.Add(new Profile("Default", "Condition AlwaysFalse"));
            }
        }

        public void SaveProfiles()
        {
            for (int i = 0; i < ProfileList.Count; i++)
            {
                SQLiteConnectionUser.PutSettingString("ProfileList" + i.ToStringInvariant(),
                            ProfileList[i].Name.QuoteString(comma: true) + "," +
                            ProfileList[i].Condition.ToString().QuoteString(comma: true));
            }

            SQLiteConnectionUser.PutSettingString("ProfileList" + ProfileList.Count, "<END>"); // this will stop reader reading in any beyond this
        }

    }
}
