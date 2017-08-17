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

    public class ActionPackEditProgram : UserControl
    {
        public Func<List<string>> onAdditionalNames;        // give me more names
        public Func<string> SuggestedName;
        public System.Action RefreshEvent;

        private Condition cd;
        private ActionFile actionfile;
        private ActionCoreController actioncorecontroller;
        private string applicationfolder;
        private Icon Icon;

        private const int panelxmargin = 3;
        private const int panelymargin = 2;

        private ExtendedControls.PanelSelectionList progmajortype;
        private ExtendedControls.ComboBoxCustom proglist;
        private ExtendedControls.ButtonExt progedit;
        private ExtendedControls.TextBoxBorder paras;

        public void Init(ActionFile af, Condition c, ActionCoreController ac, string apf , Icon i)
        {
            cd = c;
            actionfile = af;
            actioncorecontroller = ac;
            applicationfolder = apf;
            Icon = i;

            progmajortype = new ExtendedControls.PanelSelectionList();
            progmajortype.Items.AddRange(new string[] { "Key", "Key+Speech", "Speech", "Program" });
            progmajortype.Location = new Point(0, 0);
            progmajortype.Size = new Size(this.Width, this.Height); // outer panel aligns with this UC 

            proglist = new ExtendedControls.ComboBoxCustom();
            proglist.Items.Add("New");
            proglist.Items.AddRange(actionfile.actionprogramlist.GetActionProgramList());
            proglist.Location = new Point(panelxmargin, panelymargin);
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
            progmajortype.Controls.Add(proglist);
            progmajortype.Controls.Add(progedit);
            progmajortype.Controls.Add(paras);
            Controls.Add(progmajortype);
            ResumeLayout();
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

        private void Paras_Click(object sender, EventArgs e)
        {
            ConditionVariables cond = new ConditionVariables();
            string flag = "";

            if (paras.Text.Length>0)
            {
                cond.FromActionDataString(paras.Text, out flag);
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
                    string sroot = SuggestedName();
                    suggestedname = sroot;
                    int n = 2;
                    while (actionfile.actionprogramlist.GetActionProgramList().Contains(suggestedname))
                    {
                        suggestedname = sroot + "_" + n.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        n++;
                    }

                    paras.Text = "";
                }

                ActionProgramEditForm apf = new ActionProgramEditForm();
                apf.EditProgram += EditProgram;

                // we init with a variable list based on the field names of the group (normally the event field names got by SetFieldNames)
                // pass in the program if found, and its action data.

                apf.Init("Action program ", this.Icon, actioncorecontroller, applicationfolder, onAdditionalNames(), actionfile.name, p, actionfile.actionprogramlist.GetActionProgramList(), suggestedname, ModifierKeys.HasFlag(Keys.Shift));

                DialogResult res = apf.ShowDialog();

                if (res == DialogResult.OK)
                {
                    ActionProgram np = apf.GetProgram();
                    actionfile.actionprogramlist.AddOrChange(np);                // replaces or adds (if its a new name) same as rename
                    cd.action = np.Name;
                    RefreshEvent();
                    proglist.Enabled = false;
                    proglist.SelectedItem = np.Name;
                    proglist.Enabled = true;
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

        private void EditProgram(string s)  // Callback by APF to ask to edit another program..
        {
            if (!actionfile.actionprogramlist.EditProgram(s, actionfile.name, actioncorecontroller, applicationfolder))
                ExtendedControls.MessageBoxTheme.Show(this, "Unknown program or not in this file " + s);
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
            progedit.Dispose();
            paras.Dispose();
            base.Dispose();
        }
    }
}
