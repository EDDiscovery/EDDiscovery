using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    public class Action
    {
        public enum ActionType { Cmd, If, ElseIf, Do, While, Loop };

        private string actionname;
        private ActionType actiontype;
        protected List<string> actionflags;
        protected string userdata;
        protected int levelup;              // indicates for control structures that this entry is N levels up (ie. to the left).

        public Action(string n, ActionType type, List<string> f, string ud, int lu)
        {
            actionname = n;
            actiontype = type;
            actionflags = f;
            userdata = ud;
            levelup = lu;
        }

        public string Name { get { return actionname; } }
        public ActionType Type { get { return actiontype; } }
        public List<string> Flags { get { return actionflags; } }
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
            return string.Join("-", actionflags);
        }

        public virtual string DisplayedUserData { get { return userdata; } }        // what to display, null if you don't want to.
        public virtual bool AllowDirectEditingOfUserData { get { return false; } }    // and allow editing?
        public virtual void UpdateUserData(string s) { userdata = s; }              // update user data, if allow direct editing

        public virtual bool ExecuteAction(ActionProgram ap)     // execute action in the action program context.. AP has data on current state, variables etc.
        {
            return false;
        }

        public virtual bool ConfigurationMenuInUse { get { return true; } }
        public virtual bool ConfigurationMenu(System.Windows.Forms.Form parent, EDDiscovery2.EDDTheme theme)
        {
            return false;
        }

        public struct Commands
        {
            public Commands(string s, Type t , ActionType a) { name = s;  type = t; at = a; }
            public string name;
            public Type type;
            public ActionType at;
        }

        static public Commands[] cmdlist = new Commands[] 
        {
            new Commands("Say", typeof(ActionSay), ActionType.Cmd ),
            new Commands("Play", typeof(ActionPlay) , ActionType.Cmd),
            new Commands("If", typeof(ActionIf) , ActionType.If),
            new Commands("Else", typeof(ActionElse), ActionType.ElseIf),
            new Commands("Else If", typeof(ActionElseIf) , ActionType.ElseIf),
            new Commands("Log", typeof(ActionLog) , ActionType.Cmd)
        };

        public static string[] GetActionNameList()
        {
            string[] list = new string[cmdlist.Length];
            for (int i = 0; i < cmdlist.Length; i++)
                list[i] = cmdlist[i].name;
            return list;
        }

        // FACTORY make the correct class from name.

        public static Action CreateAction( string name, List<string> control = null ,string user = null , int lu = 0)       
        {
            for (int i = 0; i < cmdlist.Length; i++)
            {
                if ( name.Equals(cmdlist[i].name))
                {
                    if (control == null)
                        control = new List<string>();

                    if (user == null)
                        user = "";

                    return (Action)Activator.CreateInstance(cmdlist[i].type, new Object[] { name, cmdlist[i].at, control, user, lu });
                }
            }

            return null;
        }

        public static Action CreateCopy( Action r )     
        {
            Type ty = r.GetType();                      // get its actual type, not the base type..

            List<string> newaf = new List<string>(r.actionflags);   // make a copy of the flags, not a direct reference.

            return (Action)Activator.CreateInstance(ty, new Object[] { r.actionname, r.actiontype, newaf, r.userdata, r.levelup });
        }
    }
}
