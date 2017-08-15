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
        private ExtendedControls.ComboBoxCustom eventtype;

        private ActionPackEditProgram ucprog;
        private ActionPackEditCondition uccond;

        private ActionPackEditorForm.AdditionalNames anfunc;

        private const int panelxmargin = 3;
        private const int panelymargin = 1;

        public override void InitSub(Condition cond, List<string> events, ActionCoreController cp, string appfolder, ActionFile file,
                        ActionPackEditorForm.AdditionalNames func, Icon ic)
        {
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

            ucprog = new ActionPackEditProgram();
            ucprog.Location = new Point(160, 0);
            ucprog.Size = new Size(400, this.Height);       // init all the panels to 0/this height, select widths
            ucprog.Init(file, cond, cp, appfolder, ic);
            ucprog.onAdditionalNames += additionalnamelist;
            ucprog.SuggestedName += suggestedname;
            ucprog.RefreshEvent += refresh;
            Controls.Add(ucprog);

            uccond = new ActionPackEditCondition();
            uccond.Location = new Point(ucprog.Right + 8, 0);
            uccond.Size = new Size(200, this.Height);       // init all the panels to 0/this height, select widths
            uccond.Init(cond, ic);
            uccond.onAdditionalNames += additionalnamelist;

            Controls.Add(uccond);
        }

        private void Eventtype_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cd.eventname = eventtype.Text;

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

                uccond.SetCondition(cd);
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

        // tell the sub controls stuff

        public List<string> additionalnamelist()
        {
            return anfunc(eventtype.Text);
        }

        public string suggestedname()
        {
            return eventtype.Text;
        }

        public void refresh()
        {
            RefreshIt();
        }

    }
}