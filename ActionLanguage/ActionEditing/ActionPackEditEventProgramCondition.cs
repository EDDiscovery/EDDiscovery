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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ActionLanguage
{
    public class ActionPackEditEventProgramCondition : ActionPackEditBase
    {
        public System.Func<Form, System.Drawing.Icon, string, string> onEditKeys;   // edit the key string.. must provide
        public System.Func<Form, string, ActionCoreController, string> onEditSay;   // edit the say string..

        private ExtendedControls.ComboBoxCustom eventtype;

        private ActionPackEditProgram ucprog;
        private ActionPackEditCondition uccond;

        private System.Func<string, List<string>> anfunc;
        public System.Func<Condition, ActionProgram.ProgramConditionClass> autosetcondition;      // set the condition from the event type..

        private const int panelxmargin = 3;
        private const int panelymargin = 1;

        public override void Init(Condition cond, List<string> events, ActionCoreController cp, string appfolder, ActionFile actionfile,
                        System.Func<string, List<string>> func, Icon ic, ToolTip toolTip)
        {
            cd = cond;
            anfunc = func;

            eventtype = new ExtendedControls.ComboBoxCustom();
            eventtype.Items.AddRange(events);
            eventtype.Location = new Point(panelxmargin, panelymargin);
            eventtype.Size = new Size(140, 24);
            eventtype.DropDownHeight = 400;
            eventtype.DropDownWidth = eventtype.Width * 3 / 2;
            if (cd.eventname != null)
                eventtype.SelectedItem = cd.eventname;
            eventtype.SelectedIndexChanged += Eventtype_SelectedIndexChanged;

            Controls.Add(eventtype);

            uccond = new ActionPackEditCondition();
            uccond.Location = new Point(eventtype.Right+16, 0);
            uccond.Size = new Size(200, this.Height);       // init all the panels to 0/this height, select widths
            uccond.Init(cond, ic ,toolTip);
            uccond.onAdditionalNames += () => { return anfunc(eventtype.Text); };

            Controls.Add(uccond);

            ActionProgram p = cond.action.HasChars() ? actionfile.actionprogramlist.Get(cond.action) : null;
            ActionProgram.ProgramConditionClass classifier = p != null ? p.progclass : ActionProgram.ProgramConditionClass.Full;
            ucprog = new ActionPackEditProgram();
            ucprog.Location = new Point(uccond.Right+16, 0);
            ucprog.Size = new Size(400, this.Height);       // init all the panels to 0/this height, select widths
            ucprog.Init(actionfile, cond, cp, appfolder, ic, toolTip, classifier);
            ucprog.onEditKeys = onEditKeys;
            ucprog.onEditSay = onEditSay;
            ucprog.onAdditionalNames += () => { return anfunc(eventtype.Text); };
            ucprog.SuggestedName += () => { return eventtype.Text; };
            ucprog.RefreshEvent += () => { RefreshIt(); };
            Controls.Add(ucprog);

        }

        private void Eventtype_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cd.eventname = eventtype.Text;

            if (autosetcondition != null)
            {
                ActionProgram.ProgramConditionClass cls = autosetcondition(cd);     // ask how to set up the event..
                uccond.ChangedCondition();      // cd is updated
                ucprog.ChangedCondition(cls);   // program class is updated
            }
        }

        public override void UpdateProgramList(string[] proglist)
        {
            ucprog.UpdateProgramList(proglist);
        }

        public override void Dispose()
        {
            base.Dispose();
            uccond.Dispose();
            ucprog.Dispose();
            eventtype.Dispose();
        }

        public override string ID() { return eventtype.Text.Length > 0 ? eventtype.Text : "Action not set"; }
    }
}