/*
 * Copyright 2016 - 2024 EDDiscovery development team
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
 */

using System;
using System.Drawing;

namespace EDDiscovery.UserControls
{
    public partial class UserControlLog : UserControlCommonBase
    {
        public UserControlLog()
        {
            InitializeComponent();
            richTextBox_History.ReadOnly = true;
            richTextBox_History.ContextMenuStrip = contextMenuStrip;
        }

        public override void Init()
        {
            DiscoveryForm.OnNewLogEntry += AppendText;
            AppendText(DiscoveryForm.LogText, ExtendedControls.Theme.Current.TextBlockForeColor);
            richTextBox_History.ScrollToCaret();

            var enumlistcms = new Enum[] { EDTx.UserControlLog_toolStripMenuItemCopy, EDTx.UserControlLog_clearLogToolStripMenuItem };
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);
        }

        public override void InitialDisplay()
        {
        }

        public override void Closing()
        {
            DiscoveryForm.OnNewLogEntry -= AppendText;
        }

        public void AppendText(string s, Color c)
        {
            richTextBox_History.AppendText(s, c);
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox_History.Clear();
        }

        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            string s = richTextBox_History.SelectedText;
            if (s.Length == 0)
                s = richTextBox_History.Text;
            //System.Diagnostics.Debug.WriteLine("Sel " + s);
            SetClipboardText(s);
        }
    }
}
