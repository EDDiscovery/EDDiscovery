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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExtendedControls
{
    public static class PromptSingleLine
    {
        public static string ShowDialog(Form p,
                            string lab1, string defaultValue1, string caption, Icon ic, bool multiline = false, string tooltip = null)
        {
            List<string> r = PromptMultiLine.ShowDialog(p, caption, ic, new string[] { lab1 }, 
                    new string[] { defaultValue1 }, multiline, tooltip != null ? new string[] { tooltip } : null);

            return r?[0];
        }
    }

    public static class PromptMultiLine
    {
        // lab sets the items, def can be less or null
        public static List<string> ShowDialog(Form p, string caption, Icon ic, string[] lab, string[] def, bool multiline = false, string[] tooltips = null)
        {
            ITheme theme = ThemeableFormsInstance.Instance;

            int vstart = theme.WindowsFrame ? 20 : 40;
            int vspacing = multiline ? 80 : 40;
            int lw = 100;
            int lx = 10;
            int tx = 10 + lw + 8;

            Form prompt = new Form()
            {
                Width = 600,
                Height = 90 + vspacing * lab.Length + (theme.WindowsFrame ? 20 : 0),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                Icon = ic
            };

            Panel outer = new Panel() { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle };
            prompt.Controls.Add(outer);

            Label textLabel = new Label() { Left = lx, Top = 8, Width = prompt.Width - 50, Text = caption };

            if (!theme.WindowsFrame)
                outer.Controls.Add(textLabel);

            Label[] lbs = new Label[lab.Length];
            ExtendedControls.TextBoxBorder[] tbs = new ExtendedControls.TextBoxBorder[lab.Length];

            ToolTip tt = new ToolTip();
            tt.ShowAlways = true;

            int y = vstart;

            for (int i = 0; i < lab.Length; i++)
            {
                lbs[i] = new Label() { Left = lx, Top = y, Width = lw, Text = lab[i] };
                tbs[i] = new ExtendedControls.TextBoxBorder()
                {
                    Left = tx,
                    Top = y,
                    Width = prompt.Width - 50 - tx,
                    Text = (def != null && i < def.Length) ? def[i] : "",
                    Multiline = multiline,      // set before height!
                    Height = vspacing - 20,
                    ScrollBars = (multiline) ? ScrollBars.Vertical : ScrollBars.None,
                    WordWrap = multiline
                };
                outer.Controls.Add(lbs[i]);
                outer.Controls.Add(tbs[i]);

                if (tooltips != null && i < tooltips.Length)
                {
                    tt.SetToolTip(lbs[i], tooltips[i]);
                    tbs[i].SetTipDynamically(tt, tooltips[i]);      // no container here, set tool tip on text boxes using this
                }

                y += vspacing;
            }

            ExtendedControls.ButtonExt confirmation = new ExtendedControls.ButtonExt() { Text = "Ok".Tx(), Left = tbs[0].Right - 80, Width = 80, Top = y, DialogResult = DialogResult.OK };
            outer.Controls.Add(confirmation);
            confirmation.Click += (sender, e) => { prompt.Close(); };

            ExtendedControls.ButtonExt cancel = new ExtendedControls.ButtonExt() { Text = "Cancel".Tx(), Left = confirmation.Location.X - 90, Width = 80, Top = confirmation.Top, DialogResult = DialogResult.Cancel };
            outer.Controls.Add(cancel);
            cancel.Click += (sender, e) => { prompt.Close(); };

            if (!multiline)
                prompt.AcceptButton = confirmation;

            prompt.CancelButton = cancel;
            prompt.ShowInTaskbar = false;

            theme.ApplyToFormStandardFontSize(prompt);

            if (prompt.ShowDialog(p) == DialogResult.OK)
            {
                var r = (from ExtendedControls.TextBoxBorder t in tbs select t.Text).ToList();
                return r;
            }
            else
                return null;
        }
    }

}
