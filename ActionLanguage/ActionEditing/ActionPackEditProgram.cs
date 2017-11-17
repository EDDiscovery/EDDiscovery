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

using BaseUtils;
using Conditions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ActionLanguage
{
    // this class adds on an event field, and program fields

    public class ActionPackEditProgram : UserControl
    {
        public Func<List<string>> onAdditionalNames;        // give me more names must provide
        public Func<Form, System.Drawing.Icon, string, string> onEditKeys;   // edit the key string.. must provide
        public Func<Form, string, ActionCoreController,string> onEditSay;   // edit the say string.. must provide
        public Func<string> SuggestedName;      // give me a suggested program name must provide
        public System.Action RefreshEvent;     

        private Condition cd;
        private ActionFile actionfile;
        private ActionCoreController actioncorecontroller;
        private string applicationfolder;
        private Icon Icon;

        private const int panelxmargin = 3;
        private const int panelymargin = 2;

        private ExtendedControls.PanelSelectionList progmajortype;

        private ExtendedControls.ComboBoxCustom proglist;   //Full
        private ExtendedControls.ButtonExt progedit;
        private ExtendedControls.TextBoxBorder paras;

        private ExtendedControls.ButtonExt buttonSay;
        private ExtendedControls.ButtonExt buttonKeys;

        ActionProgram.ProgramConditionClass classifier;
        ActionProgram.ProgramConditionClass[] indextoclassifier;

        public void Init(ActionFile af, Condition c, ActionCoreController ac, string apf , Icon i, ToolTip toolTip,
                    ActionProgram.ProgramConditionClass cls)
        {
            cd = c;     // point at common condition, we never new it, just update action data/action
            actionfile = af;
            actioncorecontroller = ac;
            applicationfolder = apf;
            Icon = i;
            classifier = cls;

            progmajortype = new ExtendedControls.PanelSelectionList();
            progmajortype.Items.AddRange(new string[] { "Key" , "Say", "Key+Say" , "Full Program" });
            indextoclassifier = new ActionProgram.ProgramConditionClass[] { ActionProgram.ProgramConditionClass.Key , ActionProgram.ProgramConditionClass.Say ,
                                                                            ActionProgram.ProgramConditionClass.KeySay , ActionProgram.ProgramConditionClass.Full };
            progmajortype.Location = new Point(0, 0);
            progmajortype.Size = new Size(this.Width, this.Height); // outer panel aligns with this UC 
            progmajortype.SelectedIndexChanged += PanelType_SelectedIndexChanged;
            toolTip.SetToolTip(progmajortype, "Use the selector (click on bottom right arrow) to select program class type");

            proglist = new ExtendedControls.ComboBoxCustom();
            proglist.Items.Add("New");
            proglist.Items.AddRange(actionfile.actionprogramlist.GetActionProgramList());
            proglist.Location = new Point(panelxmargin, panelymargin);
            proglist.Size = new Size((this.Width-24-8-8-8 -panelxmargin*2)/2, 24);      // 24 button, 8+8 gaps, 8 for selector
            proglist.DropDownHeight = 400;
            proglist.DropDownWidth = proglist.Width * 3 / 2;
            proglist.SelectedIndexChanged += Proglist_SelectedIndexChanged;
            proglist.SetTipDynamically(toolTip, "Select program to associate with this event");

            progedit = new ExtendedControls.ButtonExt();
            progedit.Text = "P";
            progedit.Location = new Point(proglist.Right + 8, panelymargin);
            progedit.Size = new Size(24, 24);
            progedit.Click += Progedit_Click;
            toolTip.SetToolTip(progedit, "Edit associated program");

            paras = new ExtendedControls.TextBoxBorder();
            paras.Text = (cd.actiondata != null) ? cd.actiondata : "";
            paras.Location = new Point(progedit.Right + 8, panelymargin + 2);
            paras.Size = proglist.Size;
            paras.ReadOnly = true;
            paras.Click += Paras_Click;
            paras.SetTipDynamically(toolTip, "Click to enter parameters to pass to program");

            buttonKeys = new ExtendedControls.ButtonExt();
            buttonKeys.Location = proglist.Location;
            buttonKeys.Size = new Size((this.Width - 8 - 8 - panelxmargin * 2) / 2, 24);
            buttonKeys.Click += Keypress_Click;
            toolTip.SetToolTip(buttonKeys, "Click to define keystrokes to send");

            buttonSay = new ExtendedControls.ButtonExt();
            buttonSay.Location = new Point(buttonKeys.Right + 8, buttonKeys.Top);
            buttonSay.Size = buttonKeys.Size;
            buttonSay.Click += Saypress_Click;
            toolTip.SetToolTip(buttonSay, "Click to set speech to say");

            SuspendLayout();
            progmajortype.Controls.Add(proglist);
            progmajortype.Controls.Add(progedit);
            progmajortype.Controls.Add(paras);
            progmajortype.Controls.Add(buttonKeys);
            progmajortype.Controls.Add(buttonSay);
            Controls.Add(progmajortype);

            UpdateControls();

            ResumeLayout();
        }

        public void ChangedCondition(ActionProgram.ProgramConditionClass cls)  // upper swapped out condition.. brand new clean one
        {
            classifier = cls;
            cd.action = cd.actiondata = "";
            UpdateControls();
        }

        private void UpdateControls()
        {
            proglist.Visible = progedit.Visible = paras.Visible = (classifier == ActionProgram.ProgramConditionClass.Full);
            buttonKeys.Visible = ( classifier == ActionProgram.ProgramConditionClass.KeySay || classifier == ActionProgram.ProgramConditionClass.Key);
            buttonSay.Visible = (classifier == ActionProgram.ProgramConditionClass.KeySay || classifier == ActionProgram.ProgramConditionClass.Say);

            proglist.Enabled = false;
            if (cd.action.HasChars())
                proglist.SelectedItem = cd.action;
            else
                proglist.SelectedIndex = 0;
            proglist.Enabled = true;

            paras.Text = cd.actiondata.Alt("");

            buttonKeys.Text = "Enter Key";
            buttonSay.Text = "Enter Speech";
            ActionProgram p = cd.action.HasChars() ? actionfile.actionprogramlist.Get(cd.action) : null;
            if ( p != null )
            {
                buttonKeys.Text = StringParser.FirstQuotedWord(p.keyuserdata, " ,", buttonKeys.Text, prefix: "Key:").Left(25);
                buttonSay.Text = StringParser.FirstQuotedWord(p.sayuserdata, " ,", buttonSay.Text, prefix:"Say:").Left(25);
            }
        }

        public void UpdateProgramList(string[] progl)
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

        private void Proglist_SelectedIndexChanged(object sender, EventArgs e)      //FULL
        {
            if (proglist.Enabled && proglist.SelectedIndex == 0)   // if selected NEW.
            {
                Progedit_Click(null, null);
            }
            else
                cd.action = proglist.Text;      // set program selected
        }

        private void Progedit_Click(object sender, EventArgs e)     //FULL
        {
            bool shift = ModifierKeys.HasFlag(Keys.Shift);

            ActionProgram p = null;

            if (proglist.SelectedIndex > 0)     // exclude NEW from checking for program
                p = actionfile.actionprogramlist.Get(proglist.Text);

            if (p != null && p.StoredInSubFile != null && shift)        // if we have a stored in sub file, but we shift hit, cancel it
            {
                if (ExtendedControls.MessageBoxTheme.Show(FindForm(), "Do you want to bring the file back into the main file", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
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
                    suggestedname = GetSuggestedName();
                    paras.Text = "";
                }

                ActionProgramEditForm apf = new ActionProgramEditForm();
                apf.EditProgram += (s) =>
                {
                    if (!actionfile.actionprogramlist.EditProgram(s, actionfile.name, actioncorecontroller, applicationfolder))
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), "Unknown program or not in this file " + s);
                };

                // we init with a variable list based on the field names of the group (normally the event field names got by SetFieldNames)
                // pass in the program if found, and its action data.

                apf.Init("Action program ", this.Icon, actioncorecontroller, applicationfolder, onAdditionalNames(), actionfile.name, p, 
                                actionfile.actionprogramlist.GetActionProgramList(), suggestedname, ModifierKeys.HasFlag(Keys.Shift));

                DialogResult res = apf.ShowDialog(FindForm());

                if (res == DialogResult.OK)
                {
                    ActionProgram np = apf.GetProgram();
                    UpdateProgram(np);
                }
                else if (res == DialogResult.Abort)   // delete
                {
                    ActionProgram np2 = apf.GetProgram();
                    actionfile.actionprogramlist.Delete(np2.Name);
                    cd.action = "";
                    RefreshEvent();
                }
            }
        }

        private void Paras_Click(object sender, EventArgs e)        // FULL
        {
            ConditionVariables cond = new ConditionVariables();
            string flag = "";

            if (paras.Text.Length > 0)
            {
                cond.FromActionDataString(paras.Text, out flag);
            }

            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Input parameters and flags to pass to program on run", this.Icon, cond, showone: true, showrefresh: true, showrefreshstate: flag.Equals(ConditionVariables.flagRunAtRefresh));

            if (avf.ShowDialog(FindForm()) == DialogResult.OK)
            {
                cd.actiondata = paras.Text = avf.result.ToActionDataString(avf.result_refresh ? ConditionVariables.flagRunAtRefresh : "");
            }
        }

        private void PanelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActionProgram.ProgramConditionClass newcls = indextoclassifier[progmajortype.SelectedIndex];
            if (newcls != classifier)
            {
                ActionProgram p = cd.action.HasChars() ? actionfile.actionprogramlist.Get(cd.action) : null;

                if (p != null) // if we have an existing program, may need to alter it
                {
                    // to not full with prog class being full, need to clear, not in correct form
                    if (newcls != ActionProgram.ProgramConditionClass.Full && p.progclass == ActionProgram.ProgramConditionClass.Full)
                        cd.action = cd.actiondata = "";     // only change these
                    else if (newcls == ActionProgram.ProgramConditionClass.Key) // key, make sure no say
                        p.SetKeySayProgram(p.keyuserdata, null);
                    else if (newcls == ActionProgram.ProgramConditionClass.Say) //say, make sure no key
                        p.SetKeySayProgram(null, p.sayuserdata);
                }

                classifier = newcls;
                UpdateControls();
            }
        }

        private void Keypress_Click(object sender, EventArgs e)
        {
            ActionProgram p = cd.action.HasChars() ? actionfile.actionprogramlist.Get(cd.action) : null;

            string ud = onEditKeys(this.FindForm(), this.Icon, p != null ? p.keyuserdata.Alt("") : "");

            if ( ud != null )
            {
                if (p == null)
                    p = new ActionProgram(GetSuggestedName());

                paras.Text = cd.actiondata = "";
                p.SetKeySayProgram(ud, p.sayuserdata);
                UpdateProgram(p);
            }
        }

        private void Saypress_Click(object sender, EventArgs e)
        {
            ActionProgram p = cd.action.HasChars() ? actionfile.actionprogramlist.Get(cd.action) : null;

            string ud = onEditSay(this.FindForm(), p != null ? p.sayuserdata.Alt("") : "" , actioncorecontroller );

            if ( ud != null )
            {
                if (p == null)
                    p = new ActionProgram(GetSuggestedName());

                paras.Text = cd.actiondata = "";
                p.SetKeySayProgram(p.keyuserdata,ud);
                UpdateProgram(p);
            }
        }

        void UpdateProgram(ActionProgram np)
        {
            actionfile.actionprogramlist.AddOrChange(np);                // replaces or adds (if its a new name) same as rename
            cd.action = np.Name;
            RefreshEvent();
            UpdateControls();
        }

        public string GetSuggestedName()
        {
            string sroot = SuggestedName();
            string suggestedname = sroot;
            int n = 2;
            while (actionfile.actionprogramlist.GetActionProgramList().Contains(suggestedname))
            {
                suggestedname = sroot + "_" + n.ToStringInvariant();
                n++;
            }

            return suggestedname;
        }

        public new void Dispose()
        {
            Controls.Clear();
            proglist.Dispose();
            progedit.Dispose();
            paras.Dispose();
            base.Dispose();
        }
    }
}
