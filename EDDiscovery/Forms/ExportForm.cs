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

using System;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class ExportForm : ExtendedControls.DraggableForm
    {
        public int SelectedIndex { get; private set; }
        public DateTime StartTimeUTC { get { return EDDConfig.Instance.ConvertTimeToUTCFromSelected(customDateTimePickerFrom.Value); } }
        public DateTime EndTimeUTC { get { return EDDConfig.Instance.ConvertTimeToUTCFromSelected(customDateTimePickerTo.Value); } }
        public bool Comma { get { return radioButtonComma.Checked; } }
        public bool AutoOpen { get { return checkBoxCustomAutoOpen.Checked; } }
        public bool IncludeHeader { get { return checkBoxIncludeHeader.Checked; } }
        public string Path { get; private set; }

        private string[] outputextension;

        public ExportForm()
        {
            InitializeComponent();
        }

        public void Init(string[] exportlist, bool disablestartendtime = false, string[] outputext = null)
        {
            outputextension = outputext;

            comboBoxCustomExportType.Items.AddRange(exportlist);
            customDateTimePickerFrom.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(new DateTime(2014, 11, 22, 0, 0, 0, DateTimeKind.Utc)); //Gamma start
            customDateTimePickerTo.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(new DateTime(DateTime.UtcNow.Year,DateTime.UtcNow.Month,DateTime.UtcNow.Day,23,59,59));

            if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Equals("."))
                radioButtonComma.Checked = true;
            else
                radioButtonSemiColon.Checked = true;

            checkBoxIncludeHeader.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("ExportFormIncludeHeader", true);
            checkBoxCustomAutoOpen.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("ExportFormOpenExcel", true);

            comboBoxCustomExportType.SelectedIndex = 0;

            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            bool winborder = theme.ApplyDialog(this);
            label_index.Visible = panel_minimize.Visible = panel_close.Visible = !winborder;

            if (disablestartendtime)
            {
                customDateTimePickerFrom.Visible = customDateTimePickerTo.Visible = false;
                int d = panelBottom.Top - customDateTimePickerFrom.Top;
                panelBottom.Top -= d;
                panelOuter.Height -= d;
                Height -= d;
            }

            BaseUtils.Translator.Instance.Translate(this);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ExportForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("ExportFormIncludeHeader", checkBoxIncludeHeader.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("ExportFormOpenExcel", checkBoxCustomAutoOpen.Checked);

            SelectedIndex = comboBoxCustomExportType.SelectedIndex;

            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = outputextension != null ? outputextension[SelectedIndex] : "CSV export| *.csv";
            dlg.Title = string.Format("Export current data {0}".T(EDTx.ExportForm_ECH), dlg.Filter);

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Path = dlg.FileName;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void label_index_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void label_index_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void checkBoxRawJournal_CheckedChanged(object sender, EventArgs e)
        {
            ExtendedControls.ExtCheckBox control = (ExtendedControls.ExtCheckBox)sender;
            if (control.Checked && control.Visible)
            {
                checkBoxIncludeHeader.Checked = false;
                checkBoxIncludeHeader.Enabled = false;
            }
            else
                checkBoxIncludeHeader.Enabled = true;
        }
    }
}
