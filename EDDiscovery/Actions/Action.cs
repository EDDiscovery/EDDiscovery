using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    public class Action
    {
        private string actionname;
        protected List<string> actionflags;
        protected string userdata;
        protected int levelup;

        public Action(string n, List<string> f , string ud , int lu)
        {
            actionname = n;
            actionflags = f;
            userdata = ud;
            levelup = lu;
        }

        public string ActionName { get { return actionname; } }
        public List<string> ActionFlags {  get { return actionflags; } }
        public string UserData { get { return userdata; } }
        public int LevelUp { get { return levelup; } set { levelup = value; } }

        public bool IsFlag(string flag) { return actionflags.Contains(flag); }

        public void SetFlag(string marker, bool state)
        {
            if (state)
            {
                if (!actionflags.Contains(marker))
                    actionflags.Add(marker);
            }
            else
            {
                if (actionflags.Contains(marker))
                    actionflags.Remove(marker);
            }
        }

        public string GetFlagList()
        {
            return string.Join("-",actionflags);
        }

        public virtual string DisplayedUserData { get { return userdata; } }        // what to display, null if you don't want to.
        public virtual bool AllowDirectEditingOfUserData { get { return false; } }    // and allow editing?
        public virtual void UpdateUserData(string s) { userdata = s; }              // update user data, if allow direct editing

        public virtual bool IsControlStructureStart { get { return false; } }
        public virtual bool IsControlElse { get { return false; } }

        public virtual bool ExecuteAction()
        {
            return false;
        }

        public virtual bool ConfigurationMenu(System.Windows.Forms.Form parent, EDDiscovery2.EDDTheme theme)
        {
            return false;
        }

        public struct Commands
        {
            public Commands(string s, Type t ) { name = s;  type = t; }
            public string name;
            public Type type;
        }

        static public Commands[] cmdlist = new Commands[] 
        {
            new Commands("Say", typeof(ActionSay)),
            new Commands("Play", typeof(ActionPlay)),
            new Commands("If", typeof(ActionIf)),
            new Commands("Else", typeof(ActionElse)),
            new Commands("Else If", typeof(ActionElseIf)),
            new Commands("Log", typeof(ActionLog)),
        };

        public static string[] GetActionNameList()
        {
            string[] list = new string[cmdlist.Length];
            for (int i = 0; i < cmdlist.Length; i++)
                list[i] = cmdlist[i].name;
            return list;
        }

        public static Action CreateAction( string name, List<string> control = null ,string user = null , int lu = 0)       // FACTORY make the correct class..
        {
            for (int i = 0; i < cmdlist.Length; i++)
            {
                if ( name.Equals(cmdlist[i].name))
                {
                    if (control == null)
                        control = new List<string>();

                    if (user == null)
                        user = "";

                    return (Action)Activator.CreateInstance(cmdlist[i].type, new Object[] { name,control,user, lu });
                }
            }

            return null;
        }
    }
}
