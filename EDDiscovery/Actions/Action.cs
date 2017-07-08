/*
 * Copyright © 2017 EDDiscovery development team
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class Action
    {
        public enum ActionType { Cmd, Call, Return,         // NOT auto
                                If, Else, ElseIf, Do, While, Loop // Force execute
                                };

        public struct Commands
        {
            public Commands(string s, Type t, ActionType a) { name = s; type = t; at = a; }
            public string name;
            public Type type;
            public ActionType at;
        }

        static public Commands[] cmdlist;           // must be set.

        private string actionname;
        private ActionType actiontype;
        protected string userdata;
        protected int levelup;              // indicates for control structures that this entry is N levels up (ie. to the left).
        protected int whitespace;           // optional whitespace.. lines
        protected string comment;

        public string Name { get { return actionname; } }
        public string Comment { get { return comment; } set { comment = value; } }
        public ActionType Type { get { return actiontype; } }
        public string UserData { get { return userdata; } }
        public int LevelUp { get { return levelup; } set { levelup = value; } }
        public int Whitespace { get { return whitespace; } set { whitespace = value; } }

        public int LineNumber { get; set; }             // NOT stored, calculated line number.. calculated on read and editing

        public int calcDisplayLevel { get; set; }       // NOT stored, for editing purposes, what is the display level?
        public int calcStructLevel { get; set; }        // NOT stored, for editing purposes, what is the structure level?
        public bool calcAllowRight { get; set; }        // NOT stored
        public bool calcAllowLeft { get; set; }         // NOT stored

        public bool IsStructStart { get { return Type == ActionType.If || Type == ActionType.Do || Type != ActionType.While || Type != ActionType.Loop; } }

        public virtual bool AllowDirectEditingOfUserData { get { return false; } }    // and allow editing?

        public virtual string DisplayedUserData { get { return userdata; } }        // null if you dont' want to display
        public virtual void UpdateUserData(string s) { userdata = s; }              // update user data, if allow direct editing

        public virtual bool ExecuteAction(ActionProgramRun ap)     // execute action in the action program context.. AP has data on current state, variables etc.
        {
            return false;
        }

        public virtual string VerifyActionCorrect() { return null; } // on load, is the action correct?

        public virtual bool ConfigurationMenuInUse { get { return true; } }
        public virtual bool ConfigurationMenu(System.Windows.Forms.Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            return false;
        }

        public static string[] GetActionNameList()
        {
            string[] list = new string[cmdlist.Length];
            for (int i = 0; i < cmdlist.Length; i++)
                list[i] = cmdlist[i].name;
            return list;
        }

        // FACTORY make the correct class from name.
        static Dictionary<string, Commands> cmdlookup = null;

        public static Action CreateAction( string name, string user = null , string comment = null, int lu = 0 , int ws = 0 )       
        {
            if ( cmdlookup == null )                                // first go, create the mapping dictionary.. 
            {
                cmdlookup = new Dictionary<string, Commands>();
                for (int i = 0; i < cmdlist.Length; i++)
                    cmdlookup[cmdlist[i].name.ToLower()] = cmdlist[i];
            }

            string nname = name.ToLower();

            if ( cmdlookup.ContainsKey(nname))
            {
                Commands c = cmdlookup[nname];

                Action a = (Action)Activator.CreateInstance(c.type, new Object[] { });
                a.actionname = c.name;
                a.userdata = user ?? "";
                a.comment = comment ?? "";
                a.levelup = lu;
                a.whitespace = ws;
                a.actiontype = c.at;
                return a;
            }

            return null;
        }

        // Make a copy of this action
        public static Action CreateCopy( Action r )     
        {
            Type ty = r.GetType();                      // get its actual type, not the base type..

            Action a = (Action)Activator.CreateInstance(ty, new Object[] { });
            a.actionname = r.actionname;
            a.userdata = r.userdata;
            a.comment = r.comment;
            a.levelup = r.levelup;
            a.whitespace = r.whitespace;
            a.actiontype = r.actiontype;
            a.LineNumber = r.LineNumber;
            return a;
        }
        
    }
}
