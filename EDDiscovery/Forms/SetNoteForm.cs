/*
 * Copyright © 2019 EDDiscovery development team
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

using EliteDangerousCore;
using System;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class SetNoteForm : ExtendedControls.DraggableForm
    {
        public string NoteText { get { return this.textBoxNote.Text; } }

        public SetNoteForm(HistoryEntry he)
        {
            Init(he.GetNoteText, he.EventTimeUTC, he.System.Name, he.GetInfo(), he.GetDetailed() ?? "");
        }
        public SetNoteForm()
        {
        }

        public void Init(string notetext, DateTime utc, string systemname, string summary , string details)
        { 
            InitializeComponent();

            var enumlist = new Enum[] { EDTx.SetNoteForm, EDTx.SetNoteForm_labelTTimestamp, EDTx.SetNoteForm_buttonSave, 
                                    EDTx.SetNoteForm_labelTSystem, EDTx.SetNoteForm_labelTSummary, EDTx.SetNoteForm_labelTDetails };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);

            label_index.Text = this.Text;

            this.textBoxNote.Text = notetext;
            if ( utc == DateTime.MinValue)
            {
                labelTimestamp.Visible = labelTTimestamp.Visible = false;
                int offset = labelTSystem.Top - labelTTimestamp.Top;
                panelMain.Controls.ShiftControls(labelTTimestamp, new System.Drawing.Point(0,-offset));
                labelDetails.Height += offset;
            }
            else
                this.labelTimestamp.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(utc).ToString();

            this.labelSystem.Text = systemname;
            this.labelSummary.Text = summary;
            this.labelDetails.Text = details;

            var theme = ExtendedControls.Theme.Current;
            bool winborder = theme.ApplyDialog(this);
            panelTop.Visible = panelTop.Enabled = !winborder;
        }

        private void SaveNote()
        {
            textBoxNote.Text = textBoxNote.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Enter))
            {
                SaveNote();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveNote();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            labelDetails.Width = panelMain.Width - 10 - labelDetails.Left;
        }
    }
}
