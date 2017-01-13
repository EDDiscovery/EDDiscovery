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
        protected List<string> actionflags; // store aux data in here..
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

        public bool IsFlag(string flag) { return actionflags.FindIndex(x => x.StartsWith(flag)) >= 0; }
        public void SetFlag(string flag, bool state, string auxdata = "")
        {
            int pos = actionflags.FindIndex(x => x.StartsWith(flag));
            if (state)
            {
                if (pos < 0)
                    actionflags.Add(flag + ":" + auxdata);
                else
                    actionflags[pos] = flag + ":" + auxdata;
            }
            else
            {
                if (pos >= 0)
                    actionflags.RemoveAt(pos);
            }
        }
        public string GetFlagAuxData( string flag , string def = null )
        {
            int pos = actionflags.FindIndex(x => x.StartsWith(flag));
            if (pos >= 0)
            {
                int colon = actionflags[pos].IndexOf(":");

                if ( colon >= 0)
                    return actionflags[pos].Substring(colon+1);
            }
            return def;
        }

        public string GetFlagList()
        {
            return string.Join("-", actionflags);
        }


        public virtual string DisplayedUserData { get { return userdata; } }        // what to display, null if you don't want to.
        public virtual bool AllowDirectEditingOfUserData { get { return false; } }    // and allow editing?
        public virtual void UpdateUserData(string s) { userdata = s; }              // update user data, if allow direct editing

        public virtual bool ExecuteAction(ActionProgramRun ap)     // execute action in the action program context.. AP has data on current state, variables etc.
        {
            return false;
        }

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
            new Commands("Event", typeof(ActionEvent) , ActionType.Cmd),
            new Commands("If", typeof(ActionIf) , ActionType.If),
            new Commands("Else", typeof(ActionElse), ActionType.Else),
            new Commands("Else If", typeof(ActionElseIf) , ActionType.ElseIf),
            new Commands("While", typeof(ActionWhile) , ActionType.While),
            new Commands("Do", typeof(ActionDo) , ActionType.Do),
            new Commands("Loop", typeof(ActionLoop) , ActionType.Loop),
            new Commands("Call", typeof(ActionCall) , ActionType.Call),
            new Commands("Return", typeof(ActionReturn) , ActionType.Return),
            new Commands("Pragma", typeof(ActionPragma) , ActionType.Cmd),
            new Commands("Sleep", typeof(ActionSleep) , ActionType.Cmd),
            new Commands("Rem", typeof(ActionRem) , ActionType.Cmd)
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

        // Make a copy of this action
        public static Action CreateCopy( Action r )     
        {
            Type ty = r.GetType();                      // get its actual type, not the base type..

            List<string> newaf = new List<string>(r.actionflags);   // make a copy of the flags, not a direct reference.

            return (Action)Activator.CreateInstance(ty, new Object[] { r.actionname, r.actiontype, newaf, r.userdata, r.levelup });
        }

        #region Helper Dialogs

        public static class PromptSingleLine
        {
            public static string ShowDialog(Form p, string text, String defaultValue, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 440,
                    Height = 160,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen,
                };

                Label textLabel = new Label() { Left = 10, Top = 20, Width = 400, Text = text };
                TextBox textBox = new TextBox() { Left = 10, Top = 50, Width = 400 };
                textBox.Text = defaultValue;
                Button confirmation = new Button() { Text = "Ok", Left = 330, Width = 80, Top = 90, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "Cancel", Left = 245, Width = 80, Top = 90, DialogResult = DialogResult.Cancel };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                cancel.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(cancel);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;
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
