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

using Conditions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ActionLanguage
{
    // this class adds on an event field, and program fields

    public class ActionPackEditEventProgram : ActionPackEditBase
    {
        public ExtendedControls.ComboBoxCustom eventtype;
        public ExtendedControls.ComboBoxCustom proglist;
        public ExtendedControls.ButtonExt progedit;
        public ExtendedControls.TextBoxBorder paras;

        public override void InitSub(List<string> events)
        {
            eventtype = new ExtendedControls.ComboBoxCustom();
            eventtype.Items.AddRange(events);
            eventtype.Location = new Point(panelxmargin, panelymargin);
            eventtype.Size = new Size(140, 24);
            eventtype.DropDownHeight = 400;
            eventtype.DropDownWidth = eventtype.Width * 3 / 2;
            if (cd.eventname != null)
                eventtype.SelectedItem = cd.eventname;
            eventtype.SelectedIndexChanged += Actiontype_SelectedIndexChanged;

            proglist = new ExtendedControls.ComboBoxCustom();
            proglist.Items.Add("New");
            proglist.Items.AddRange(actionfile.actionprogramlist.GetActionProgramList());
            proglist.Location = new Point(eventtype.Right + 16, panelymargin);
            proglist.Size = new Size(140, 24);
            proglist.DropDownHeight = 400;
            proglist.DropDownWidth = proglist.Width * 3 / 2;
            if (cd.action != null)
                proglist.Text = cd.action;
            else
                proglist.SelectedIndex = 0;

            proglist.SelectedIndexChanged += Proglist_SelectedIndexChanged;

            progedit = new ExtendedControls.ButtonExt();
            progedit.Text = "P";
            progedit.Location = new Point(proglist.Right + 8, panelymargin);
            progedit.Size = new Size(24, 24);
            progedit.Click += Progedit_Click;

            paras = new ExtendedControls.TextBoxBorder();
            paras.Text = (cd.actiondata != null) ? cd.actiondata : "";
            paras.Location = new Point(progedit.Right + 8, panelymargin + 2);
            paras.Size = new Size(96, 24);
            paras.ReadOnly = true;
            paras.Click += Paras_Click;

            SuspendLayout();
            Controls.Add(proglist);
            Controls.Add(eventtype);
            Controls.Add(progedit);
            Controls.Add(paras);
            ResumeLayout();
        }

        public override void UpdateProgramList(string[] progl)
        {
            proglist.Enabled = false;

            string text = proglist.Text;

            proglist.Items.Clear();
            proglist.Items.Add("New");
            proglist.Items.AddRange(progl);

            if (proglist.Items.Contains(text))
                proglist.SelectedItem = text;

            proglist.Enabled = true;
        }

        private void Paras_Click(object sender, EventArgs e)
        {
            ConditionVariables cond = new ConditionVariables();
            string flag = "";

            if (cd.actiondata != null)
            {
                cond.FromActionDataString(cd.actiondata, out flag);
            }

            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Input parameters and flags to pass to program on run", this.Icon, cond, showone: true, showrefresh: true, showrefreshstate: flag.Equals(ConditionVariables.flagRunAtRefresh));

            if (avf.ShowDialog(this) == DialogResult.OK)
            {
                cd.actiondata = paras.Text = avf.result.ToActionDataString(avf.result_refresh ? ConditionVariables.flagRunAtRefresh : "");
            }
        }

        private void Progedit_Click(object sender, EventArgs e)
        {
            bool shift = ModifierKeys.HasFlag(Keys.Shift);

            ActionProgram p = null;

            if (proglist.SelectedIndex > 0)     // exclude NEW from checking for program
                p = actionfile.actionprogramlist.Get(proglist.Text);

            if (p != null && p.StoredInSubFile != null && shift)        // if we have a stored in sub file, but we shift hit, cancel it
            {
                if (ExtendedControls.MessageBoxTheme.Show(this, "Do you want to bring the file back into the main file", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    p.CancelSubFileStorage();
                    shift = false;
                }
                else
                    return; // cancel, abort.
            }

            if (p != null && p.StoredInSubFile != null)
            {
                p.EditInEditor(p.StoredInSubFile);         // Edit in the editor.. this also updated the program steps held internally
            }
            else
            {
                string suggestedname = null;

                if (p == null)        // if no program, create a new suggested name and clear any action data
                {
                    suggestedname = eventtype.Text;
                    int n = 2;
                    while (actionfile.actionprogramlist.GetActionProgramList().Contains(suggestedname))
                    {
                        suggestedname = eventtype.Text + "_" + n.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        n++;
                    }

                    paras.Text = "";
                }

                ActionProgramEditForm apf = new ActionProgramEditForm();
                apf.EditProgram += EditProgram;

                // we init with a variable list based on the field names of the group (normally the event field names got by SetFieldNames)
                // pass in the program if found, and its action data.

                apf.Init("Action program ", this.Icon, actioncorecontroller, applicationfolder, onAdditionalNames(eventtype.Text), actionfile.name, p, actionfile.actionprogramlist.GetActionProgramList(), suggestedname, ModifierKeys.HasFlag(Keys.Shift));

                DialogResult res = apf.ShowDialog();

                if (res == DialogResult.OK)
                {
                    ActionProgram np = apf.GetProgram();
                    actionfile.actionprogramlist.AddOrChange(np);                // replaces or adds (if its a new name) same as rename
                    cd.action = np.Name;
                    RefreshIt();
                    proglist.Enabled = false;
                    proglist.SelectedItem = np.Name;
                    proglist.Enabled = true;
                }
                else if (res == DialogResult.Abort)   // delete
                {
                    ActionProgram np2 = apf.GetProgram();
                    actionfile.actionprogramlist.Delete(np2.Name);
                    cd.action = "";
                    RefreshIt();
                }
            }
        }

        private void EditProgram(string s)  // Callback by APF to ask to edit another program..
        {
            if (!actionfile.actionprogramlist.EditProgram(s, actionfile.name, actioncorecontroller, applicationfolder))
                ExtendedControls.MessageBoxTheme.Show(this, "Unknown program or not in this file " + s);
        }

        private void Actiontype_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd.eventname = eventtype.Text;
            EventChanged();
        }

        private void Proglist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (proglist.Enabled && proglist.SelectedIndex == 0)   // if selected NEW.
            {
                Progedit_Click(null, null);
            }
            else
                cd.action = proglist.Text;      // set program selected
        }

        public new void Dispose()
        {
            Controls.Clear();
            proglist.Dispose();
            eventtype.Dispose();
            progedit.Dispose();
            paras.Dispose();
            base.Dispose();
        }
    }

    // this class adds on an condition and delete options to the event and program fields

    public class ActionPackEditEventProgramCondition : ActionPackEditEventProgram
    {
        public ExtendedControls.TextBoxBorder condition;
        public ExtendedControls.ButtonExt delete;

        public override string ID() { return eventtype.Text.Length > 0 ? eventtype.Text : "Action not set"; }

        public override void InitSub(List<string> events)
        {
            base.InitSub(events);

            condition = new ExtendedControls.TextBoxBorder();
            condition.Text = cd.ToString();
            condition.Location = new Point(paras.Right + 16, panelymargin + 2);
            condition.Size = new Size(200, 24);
            condition.ReadOnly = true;
            condition.Click += Condition_Click;
            condition.Tag = this;

            delete = new ExtendedControls.ButtonExt();
            delete.Text = "X";
            delete.Location = new Point(condition.Right + 8, panelymargin);
            delete.Size = new Size(24, 24);
            delete.Click += Delete_Click;

            SuspendLayout();
            Controls.Add(condition);
            Controls.Add(delete);
            ResumeLayout();
        }

        private void Condition_Click(object sender, EventArgs e)
        {
            ConditionFilterForm frm = new ConditionFilterForm();

            frm.InitCondition("Action condition", this.Icon, onAdditionalNames(eventtype.Text), cd);
            frm.TopMost = this.FindForm().TopMost;
            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                Condition res = frm.result.Get(0);
                if (res != null)
                {
                    cd.fields = res.fields;
                    cd.innercondition = res.innercondition;
                }
                else
                    cd.fields = null;

                condition.Text = cd.ToString();
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            RemoveIt();
        }

        public new void Dispose()
        {
            condition.Dispose();
            delete.Dispose();
            base.Dispose();
        }

        public override void EventChanged()     // helpers to set initial conditions
        {
            if (cd.AlwaysTrue())
            {
                if (cd.eventname == "onKeyPress")
                    cd.Set(new ConditionEntry("KeyPress", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onTimer")
                    cd.Set(new ConditionEntry("TimerName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onPopUp" || cd.eventname == "onPopDown")
                    cd.Set(new ConditionEntry("PopOutName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onTabChange")
                    cd.Set(new ConditionEntry("TabName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onPanelChange")
                    cd.Set(new ConditionEntry("PopOutName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onEliteInput" || cd.eventname == "onEliteInputOff")
                    cd.Set(new ConditionEntry("Binding", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onMenuItem")
                    cd.Set(new ConditionEntry("MenuName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onSayStarted" || cd.eventname == "onSayFinished" || cd.eventname == "onPlayStarted" || cd.eventname == "onPlayFinished")
                    cd.Set(new ConditionEntry("EventName", ConditionEntry.MatchType.Equals, "?"));

                condition.Text = cd.ToString();
            }
        }
    }

}
