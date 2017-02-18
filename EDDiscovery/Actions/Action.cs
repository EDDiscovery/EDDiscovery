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
        public enum ActionType { Cmd, If, Else, ElseIf, Do, While, Loop , Call , Return };

        private string actionname;
        private ActionType actiontype;
        protected string userdata;
        protected int levelup;              // indicates for control structures that this entry is N levels up (ie. to the left).
        protected int whitespace;           // optional whitespace.. lines

        public string Name { get { return actionname; } }
        public ActionType Type { get { return actiontype; } }
        public string UserData { get { return userdata; } }
        public int LevelUp { get { return levelup; } set { levelup = value; } }
        public int Whitespace { get { return whitespace; } set { whitespace = value; } }

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

       private struct Commands
        {
            public Commands(string s, Type t , ActionType a) { name = s;  type = t; at = a; }
            public string name;
            public Type type;
            public ActionType at;
        }

        static private Commands[] cmdlist = new Commands[]
        {
            new Commands("Call", typeof(ActionCall) , ActionType.Call),
            new Commands("Commodities", typeof(ActionCommodities) , ActionType.Cmd),
            new Commands("Do", typeof(ActionDo) , ActionType.Do),
            new Commands("DeleteVariable", typeof(ActionDeleteVariable) , ActionType.Cmd),
            new Commands("Else", typeof(ActionElse), ActionType.Else),
            new Commands("ElseIf", typeof(ActionElseIf) , ActionType.ElseIf),
            new Commands("End", typeof(ActionEnd) , ActionType.Cmd),
            new Commands("ErrorIf", typeof(ActionErrorIf) , ActionType.Cmd),
            new Commands("Event", typeof(ActionEvent) , ActionType.Cmd),
            new Commands("FileDialog", typeof(ActionFileDialog) , ActionType.Cmd),
            new Commands("Global", typeof(ActionGlobal) , ActionType.Cmd),
            new Commands("Historytab", typeof(ActionHistoryTab) , ActionType.Cmd),
            new Commands("If", typeof(ActionIf) , ActionType.If),
            new Commands("InputBox", typeof(ActionInputBox) , ActionType.Cmd),
            new Commands("Ledger", typeof(ActionLedger) , ActionType.Cmd),
            new Commands("Let", typeof(ActionLet) , ActionType.Cmd),
            new Commands("Loop", typeof(ActionLoop) , ActionType.Loop),
            new Commands("Materials", typeof(ActionMaterials) , ActionType.Cmd),
            new Commands("MessageBox", typeof(ActionMessageBox) , ActionType.Cmd),
            new Commands("MenuItem", typeof(ActionMenuItem) , ActionType.Cmd),
            new Commands("Rem", typeof(ActionRem) , ActionType.Cmd),
            new Commands("Return", typeof(ActionReturn) , ActionType.Return),
            new Commands("Scan", typeof(ActionScan) , ActionType.Cmd),
            new Commands("Perform", typeof(ActionPerform) , ActionType.Cmd),
            new Commands("PersistentGlobal", typeof(ActionPersistentGlobal) , ActionType.Cmd),
            new Commands("Play", typeof(ActionPlay) , ActionType.Cmd),
            new Commands("Popout", typeof(ActionPopout) , ActionType.Cmd),
            new Commands("Pragma", typeof(ActionPragma) , ActionType.Cmd),
            new Commands("Print", typeof(ActionPrint) , ActionType.Cmd),
            new Commands("ProgramWindow", typeof(ActionProgramwindow) , ActionType.Cmd),
            new Commands("Say", typeof(ActionSay), ActionType.Cmd ),
            new Commands("Set", typeof(ActionSet) , ActionType.Cmd),
            new Commands("Timer", typeof(ActionTimer) , ActionType.Cmd),
            new Commands("Sleep", typeof(ActionSleep) , ActionType.Cmd),
            new Commands("While", typeof(ActionWhile) , ActionType.While),
        };

        public static string[] GetActionNameList()
        {
            string[] list = new string[cmdlist.Length];
            for (int i = 0; i < cmdlist.Length; i++)
                list[i] = cmdlist[i].name;
            return list;
        }

        // FACTORY make the correct class from name.

        public static Action CreateAction( string name, string user = null , int lu = 0 , int ws = 0 )       
        {
            for (int i = 0; i < cmdlist.Length; i++)
            {
                if ( name.Equals(cmdlist[i].name, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (user == null)
                        user = "";

                    Action a = (Action)Activator.CreateInstance(cmdlist[i].type, new Object[] { });
                    a.actionname = name;
                    a.userdata = user;
                    a.levelup = lu;
                    a.whitespace = ws;
                    a.actiontype = cmdlist[i].at;
                    return a;
                }
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
            a.levelup = r.levelup;
            a.whitespace = r.whitespace;
            a.actiontype = r.actiontype;
            return a;
        }
        
    }
}
