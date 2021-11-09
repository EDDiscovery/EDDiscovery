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

        public bool Import { get; private set; }

        private string[] outputextension;

        public ExportForm()
        {
            InitializeComponent();
        }

        public void Init(string[] selectionlist, bool disablestartendtime = false, string[] outputext = null, bool disableopeninclude = false, bool import = false)
        {
            outputextension = outputext;

            if ( selectionlist != null )
                comboBoxCustomExportType.Items.AddRange(selectionlist);

            customDateTimePickerFrom.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(new DateTime(2014, 11, 22, 0, 0, 0, DateTimeKind.Utc)); //Gamma start
            customDateTimePickerTo.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 23, 59, 59));

            if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Equals("."))
                radioButtonComma.Checked = true;
            else
                radioButtonSemiColon.Checked = true;

            checkBoxIncludeHeader.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("FormIncludeHeader", true);
            checkBoxCustomAutoOpen.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("ExportFormOpenExcel", true);

            comboBoxCustomExportType.SelectedIndex = 0;


            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            bool winborder = theme.ApplyDialog(this);
            panelTop.Visible = !winborder;

            if (comboBoxCustomExportType.Items.Count < 2)       // disable if no selection
                panelCombo.Visible = false;

            if (disablestartendtime)                // disable if required
                panelDate.Visible = false;

            if (disableopeninclude)
                panelIncludeOpen.Visible = false;

            BaseUtils.Translator.Instance.Translate(this);

            Import = import;
            if (import)
            {
                this.Text = "Import data".T(EDTx.ExportForm_ImportTitle);
                buttonExport.Text = "Import".T(EDTx.ExportForm_ImportButton);
            }

            label_index.Text = this.Text;
        }

        protected override void OnLoad(EventArgs e)
        {
            PerformLayout();
            // calculate height of window from panel sizes after layout
            int flowbot = flowLayoutPanel.Top + panelBottom.Bottom + panelBottom.Margin.Bottom + flowLayoutPanel.Margin.Bottom;
            Height = Bounds.Height - ClientRectangle.Height + flowbot;

            base.OnLoad(e);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            SelectedIndex = comboBoxCustomExportType.SelectedIndex;

            if (Import)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = outputextension != null ? outputextension[SelectedIndex] : "CSV import| *.csv";
                dlg.Title = string.Format("Import data {0}".T(EDTx.ExportForm_ImportData), dlg.Filter);

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
            else
            {
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("ExportFormIncludeHeader", checkBoxIncludeHeader.Checked);
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("ExportFormOpenExcel", checkBoxCustomAutoOpen.Checked);

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

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void panelTop_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }
    }
}
