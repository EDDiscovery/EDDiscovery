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
    public class ActionPackEditVoice : ActionPackEditBase
    {
        public System.Func<Form, System.Drawing.Icon, string, string> onEditKeys;   // edit the key string..  must provide
        public System.Func<Form, string, ActionCoreController, string> onEditSay;   // edit the say string..  must provide

        private ExtendedControls.TextBoxBorder textBoxInput;
        private ActionPackEditProgram ucprog;

        private const int panelxmargin = 3;
        private const int panelymargin = 1;

        public override void Init(Condition cond, List<string> events, ActionCoreController cp, string appfolder, ActionFile actionfile,
                        System.Func<string, List<string>> func, Icon ic , ToolTip toolTip)
        {
            cd = cond;      // on creation, the cond with be set to onVoice with one condition, checked in ActionController.cs:SetPackEditor..

            textBoxInput = new ExtendedControls.TextBoxBorder();
            textBoxInput.Location = new Point(panelxmargin, panelymargin);
            textBoxInput.Size = new Size(356, 24);      // manually matched to size of eventprogramcondition bits
            textBoxInput.Text = cd.fields[0].matchstring;
            textBoxInput.TextChanged += TextBoxInput_TextChanged;
            textBoxInput.SetTipDynamically(toolTip,"Enter the voice input to recognise.  Multiple phrases seperate with semicolons");
            
            Controls.Add(textBoxInput);

            ActionProgram p = cond.action.HasChars() ? actionfile.actionprogramlist.Get(cond.action) : null;
            ActionProgram.ProgramConditionClass classifier = p != null ? p.progclass : ActionProgram.ProgramConditionClass.KeySay;

            ucprog = new ActionPackEditProgram();
            ucprog.Location = new Point(textBoxInput.Right+16, 0);
            ucprog.Size = new Size(400, this.Height);       // init all the panels to 0/this height, select widths
            ucprog.Init(actionfile, cond, cp, appfolder, ic, toolTip, classifier);
            ucprog.onEditKeys = onEditKeys;
            ucprog.onEditSay = onEditSay;
            ucprog.onAdditionalNames += () => { return func(null); };
            ucprog.SuggestedName += () => 
            {
                string textparse = (textBoxInput.Text.Length > 0 && !textBoxInput.Text.Equals("?")) ? ("_" + textBoxInput.Text.Split(';')[0].SafeVariableString()) : "";
                return "VoiceInput" + textparse;
            };
            ucprog.RefreshEvent += () => { RefreshIt(); };
            Controls.Add(ucprog);

        }

        private void TextBoxInput_TextChanged(object sender, System.EventArgs e)
        {
            cd.fields[0].matchstring = ((ExtendedControls.TextBoxBorder)sender).Text;
        }

        public override void UpdateProgramList(string[] proglist)
        {
            ucprog.UpdateProgramList(proglist);
        }

        public override void Dispose()
        {
            base.Dispose();
            textBoxInput.Dispose();
            ucprog.Dispose();
        }

        public override string ID() { return "Voice Input"; }
    }
}