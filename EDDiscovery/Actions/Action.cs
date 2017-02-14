﻿using System;
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
        public bool calcAllowRight { get; set; }        // NOT stored
        public bool calcAllowLeft { get; set; }         // NOT stored

        public virtual bool AllowDirectEditingOfUserData { get { return false; } }    // and allow editing?

        public virtual string DisplayedUserData { get { return userdata; } }        // null if you dont' want to display
        public virtual void UpdateUserData(string s) { userdata = s; }              // update user data, if allow direct editing

        public virtual bool ExecuteAction(ActionProgramRun ap)     // execute action in the action program context.. AP has data on current state, variables etc.
        {
            return false;
        }

        public virtual string VerifyActionCorrect() { return null; } // on load, is the action correct?

        public virtual bool ConfigurationMenuInUse { get { return true; } }
        public virtual bool ConfigurationMenu(System.Windows.Forms.Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
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
            new Commands("Say", typeof(ActionSay), ActionType.Cmd ),
            new Commands("Play", typeof(ActionPlay) , ActionType.Cmd),
            new Commands("Print", typeof(ActionPrint) , ActionType.Cmd),
            new Commands("ErrorIf", typeof(ActionErrorIf) , ActionType.Cmd),
            new Commands("Set", typeof(ActionSet) , ActionType.Cmd),
            new Commands("Let", typeof(ActionLet) , ActionType.Cmd),
            new Commands("Global", typeof(ActionGlobal) , ActionType.Cmd),
            new Commands("Event", typeof(ActionEvent) , ActionType.Cmd),
            new Commands("Materials", typeof(ActionMaterials) , ActionType.Cmd),
            new Commands("Commodities", typeof(ActionCommodities) , ActionType.Cmd),
            new Commands("Ledger", typeof(ActionLedger) , ActionType.Cmd),
            new Commands("Scan", typeof(ActionScan) , ActionType.Cmd),
            new Commands("Popout", typeof(ActionPopout) , ActionType.Cmd),
            new Commands("Historytab", typeof(ActionHistoryTab) , ActionType.Cmd),
            new Commands("ProgramWindow", typeof(ActionProgramwindow) , ActionType.Cmd),
            new Commands("Perform", typeof(ActionPerform) , ActionType.Cmd),
            new Commands("If", typeof(ActionIf) , ActionType.If),
            new Commands("Else", typeof(ActionElse), ActionType.Else),
            new Commands("ElseIf", typeof(ActionElseIf) , ActionType.ElseIf),
            new Commands("While", typeof(ActionWhile) , ActionType.While),
            new Commands("Do", typeof(ActionDo) , ActionType.Do),
            new Commands("Loop", typeof(ActionLoop) , ActionType.Loop),
            new Commands("Call", typeof(ActionCall) , ActionType.Call),
            new Commands("Return", typeof(ActionReturn) , ActionType.Return),
            new Commands("Pragma", typeof(ActionPragma) , ActionType.Cmd),
            new Commands("Sleep", typeof(ActionSleep) , ActionType.Cmd),
            new Commands("Rem", typeof(ActionRem) , ActionType.Cmd),
            new Commands("End", typeof(ActionEnd) , ActionType.Cmd)
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

        #region Helper Dialogs

        public static class PromptSingleLine
        {
            public static string ShowDialog(Form p, string text, String defaultValue, string caption , bool multiline = false)
            {
                Form prompt = new Form()
                {
                    Width = 440,
                    Height = 160 + (multiline?40:0),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen,
                };

                Label textLabel = new Label() { Left = 10, Top = 20, Width = 400, Text = text };
                TextBox textBox = new TextBox() { Left = 10, Top = 50, Width = 400 , Height = 20 + (multiline?40:0) };
                textBox.Text = defaultValue;
                int okline = 90;
                if (multiline )
                {
                    textBox.Multiline = true;
                    textBox.ScrollBars = ScrollBars.Vertical;
                    textBox.WordWrap = true;
                    okline = 130;
                }
                Button confirmation = new Button() { Text = "Ok", Left = 330, Width = 80, Top = okline, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "Cancel", Left = 245, Width = 80, Top = okline, DialogResult = DialogResult.Cancel };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                cancel.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(cancel);
                prompt.Controls.Add(textLabel);
                prompt.CancelButton = cancel;
                prompt.ShowInTaskbar = false;

                return prompt.ShowDialog(p) == DialogResult.OK ? textBox.Text : null;
            }
        }

        public static class PromptDoubleLine
        {
            public static Tuple<string,string> ShowDialog(Form p, string lab1, string lab2, string defaultValue1 , string defaultValue2, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 600,
                    Height = 160,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen,
                };

                int lw = 80;
                int lx = 10;
                int tx = 10 + lw + 8;

                Label textLabel1 = new Label() { Left = lx, Top = 20, Width = lw, Text = lab1 };
                Label textLabel2 = new Label() { Left = lx, Top = 60, Width = lw, Text = lab2};
                TextBox textBox1 = new TextBox() { Left = tx, Top = 20, Width = prompt.Width - 50 - tx, Text = defaultValue1 };
                TextBox textBox2 = new TextBox() { Left = tx, Top = 60, Width = prompt.Width - 50 - tx, Text = defaultValue2 };
                Button confirmation = new Button() { Text = "Ok", Left = textBox1.Location.X+textBox1.Width-80, Width = 80, Top = prompt.Height - 70, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "Cancel", Left = confirmation.Location.X-90, Width = 80, Top = prompt.Height - 70, DialogResult = DialogResult.Cancel };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                cancel.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textLabel1);
                prompt.Controls.Add(textLabel2);
                prompt.Controls.Add(textBox1);
                prompt.Controls.Add(textBox2);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(cancel);
                prompt.AcceptButton = confirmation;
                prompt.CancelButton = cancel;
                prompt.ShowInTaskbar = false;

                return prompt.ShowDialog(p) == DialogResult.OK ? new Tuple<string,string>(textBox1.Text,textBox2.Text) : null;
            }
        }




        #endregion
    }
}
