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

    public class ActionPackEditCondition : UserControl
    {
        public Func<List<string>> onAdditionalNames;        // give me more names

        private const int panelxmargin = 3;
        private const int panelymargin = 1;
        private Icon Icon;
        private ExtendedControls.TextBoxBorder condition;
        private Condition cd;

        public void Init(Condition c, Icon ic)
        {
            cd = c;
            Icon = ic;

            condition = new ExtendedControls.TextBoxBorder();
            condition.Text = cd.ToString();
            condition.Location = new Point(panelxmargin, panelymargin + 2);
            condition.Size = new Size(200, 24);
            condition.ReadOnly = true;
            condition.Click += Condition_Click;
            condition.Tag = this;

            SuspendLayout();
            Controls.Add(condition);
            ResumeLayout();
        }

        public void SetCondition(Condition c)
        {
            cd = c;
            condition.Text = cd.ToString();
        }

        private void Condition_Click(object sender, EventArgs e)
        {
            ConditionFilterForm frm = new ConditionFilterForm();

            frm.InitCondition("Action condition", this.Icon, onAdditionalNames(), cd);
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

        public new void Dispose()
        {
            Controls.Clear();
            condition.Dispose();
            base.Dispose();
        }
    }
}
