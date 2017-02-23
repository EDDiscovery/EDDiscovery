/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery;
using EDDiscovery2.DB;
using EDDiscovery.DB;
using System.IO;
using System.Diagnostics;

namespace EDDiscovery2
{
    public partial class Settings : UserControl
    {
        private EDDiscoveryForm _discoveryForm;
        private ThemeEditor themeeditor = null;
        private bool initialized = false;
        private EDDConfig.ThreadedBindingList _tbl = new EDDConfig.ThreadedBindingList();

        public float MapZoom { get { return float.Parse(textBoxDefaultZoom.Text); } }
        public bool MapCentreOnSelection { get { return radioButtonHistorySelection.Checked; } }

        public Settings()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            ResetThemeList();
            SetEntryThemeComboBox();
            textBoxHomeSystem.SetAutoCompletor(EDDiscovery.DB.SystemClass.ReturnOnlySystemsListForAutoComplete);

            comboBoxTheme.ItemHeight = 20;
        }

        void SetEntryThemeComboBox()
        {
            int i = _discoveryForm.theme.GetIndexOfCurrentTheme();
            if (i == -1)
                comboBoxTheme.SelectedItem = "Custom";
            else
                comboBoxTheme.SelectedIndex = i;
        }

        private void ResetThemeList()
        {
            comboBoxTheme.Items = _discoveryForm.theme.GetThemeList();
            comboBoxTheme.Items.Add("Custom");
        }

        public void InitSettingsTab()
        {
            DataBind();
#if DEBUG
            checkBoxSkipSlowUpdates.Visible = true;
#endif

            textBoxDefaultZoom.Text = SQLiteDBClass.GetSettingDouble("DefaultMapZoom", 1.0).ToString();

            bool selectionCentre = SQLiteDBClass.GetSettingBool("CentreMapOnSelection", true);
            if (selectionCentre)
            {
                radioButtonHistorySelection.Checked = true;
            }
            else
            {
                radioButtonCentreHome.Checked = true;
            }

            dataGridViewCommanders.DataSource = EDCommander.GetList();
            dataGridViewCommanders.AutoGenerateColumns = false;

            panel_defaultmapcolor.BackColor = EDDConfig.Instance.DefaultMapColour;

            this.comboBoxTheme.SelectedIndexChanged += this.comboBoxTheme_SelectedIndexChanged;    // now turn on the handler..
            initialized = true;
        }

        public void SaveSettings()
        {
            double zoom = 1;
            SQLiteDBClass.PutSettingDouble("DefaultMapZoom", Double.TryParse(textBoxDefaultZoom.Text, out zoom) ? zoom : 1.0);
            SQLiteDBClass.PutSettingBool("CentreMapOnSelection", radioButtonHistorySelection.Checked);
        }

        private bool _Bound = false;
        private void DataBind()
        {
            Debug.Assert(!_Bound);

            _tbl.Bind(checkBoxAutoLoad, "Checked", "AutoLoadPopouts");
            _tbl.Bind(checkBoxAutoSave, "Checked", "AutoSavePopouts");
            _tbl.Bind(checkBoxEDSMLog, "Checked", "EDSMLog");
            _tbl.Bind(checkBoxFocusNewSystem, "Checked", "FocusOnNewSystem");
            _tbl.Bind(checkBoxKeepOnTop, "Checked", "KeepOnTop");
            _tbl.Bind(checkBoxMinimizeToNotifyIcon, "Checked", "MinimizeToNotifyIcon");
            _tbl.Bind(checkBoxMinimizeToNotifyIcon, "Enabled", "UseNotifyIcon");
            _tbl.Bind(checkBoxOrderRowsInverted, "Checked", "OrderRowsInverted");
            _tbl.Bind(checkBoxSkipSlowUpdates, "Checked", "CanSkipSlowUpdates");
            _tbl.Bind(checkBoxUseNotifyIcon, "Checked", "UseNotifyIcon");
            _tbl.Bind(checkBoxUTC, "Checked", "DisplayUTC");
            _tbl.BindValidated(textBoxHomeSystem, "Text", "HomeSystem");

            _Bound = true;
        }

        private void textBoxDefaultZoom_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var value = textBoxDefaultZoom.Text.Trim();
            double parseout = 0;
            if (!Double.TryParse(value, out parseout) || parseout < 0.01 || parseout > 50.0)
            {
                textBoxDefaultZoom.Text = "1";
            }
        }

        public void UpdateCommandersListBox()
        {
            dataGridViewCommanders.DataSource = null;
            dataGridViewCommanders.DataSource = EDCommander.GetList();
            dataGridViewCommanders.Update();
        }

        private void dataGridViewCommanders_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            List<EDCommander> edcommanders = (List<EDCommander>)dataGridViewCommanders.DataSource;

            EDCommander.Update(edcommanders,false);     // DONT update the data source.. that fucks it right up.

            if ( e.ColumnIndex == 5 )                           // if changed journal location
                _discoveryForm.RefreshHistoryAsync();           // will do a new parse on commander list adding/removing scanners
        }

        private void buttonAddCommander_Click(object sender, EventArgs e)
        {
            EDCommander.Create();
            UpdateCommandersListBox();
            _discoveryForm.TravelControl.LoadCommandersListBox();
            _discoveryForm.ExportControl.PopulateCommanders();
            _discoveryForm.RefreshHistoryAsync();           // will do a new parse on commander list adding/removing scanners
        }

        private void btnDeleteCommander_Click(object sender, EventArgs e)
        {
            var cells = dataGridViewCommanders.SelectedCells;

            HashSet<int> rowindexes = new HashSet<int>();

            foreach (var cell in cells.OfType<DataGridViewCell>())
            {
                if (!rowindexes.Contains(cell.RowIndex))
                {
                    rowindexes.Add(cell.RowIndex);
                }
            }

            if (rowindexes.Count == 1)
            {
                var row = dataGridViewCommanders.Rows[rowindexes.Single()].DataBoundItem as EDCommander;
                var result = MessageBox.Show("Do you wish to delete commander " + row.Name + "?", "Delete commander", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    EDCommander.Delete(row);
                    _discoveryForm.TravelControl.LoadCommandersListBox();
                    _discoveryForm.ExportControl.PopulateCommanders();
                    UpdateCommandersListBox();
                    _discoveryForm.RefreshHistoryAsync();           // will do a new parse on commander list adding/removing scanners
                }
            }
        }

        public void panel_defaultmapcolor_Click(object sender, EventArgs e)
        {
            ColorDialog mapColorDialog = new ColorDialog();
            mapColorDialog.AllowFullOpen = true;
            mapColorDialog.FullOpen = true;
            mapColorDialog.Color = EDDConfig.Instance.DefaultMapColour;
            if (mapColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                EDDConfig.Instance.DefaultMapColour = mapColorDialog.Color;
                panel_defaultmapcolor.BackColor = EDDConfig.Instance.DefaultMapColour;
            }
        }

        private void comboBoxTheme_SelectedIndexChanged(object sender, EventArgs e) // theme selected..
        {
            if (!initialized) return;
            string themename = comboBoxTheme.Items[comboBoxTheme.SelectedIndex].ToString();

            string fontwanted = null;                                               // don't check custom, only a stored theme..
            if (!themename.Equals("Custom") && !_discoveryForm.theme.IsFontAvailableInTheme(themename, out fontwanted))
            {
                DialogResult res = MessageBox.Show("The font used by this theme is not available on your system" + Environment.NewLine +
                      "The font needed is \"" + fontwanted + "\"" + Environment.NewLine +
                      "Install this font and you can use this scheme." + Environment.NewLine +
                      "EuroCaps font is available www.edassets.org.",
                      "Warning", MessageBoxButtons.OK);

                _discoveryForm.theme.SetCustom();                              // go to custom theme whatever
                SetEntryThemeComboBox();
                return;
            }

            if (!_discoveryForm.theme.SetThemeByName(themename))
                _discoveryForm.theme.SetCustom();                                   // go to custom theme..

            SetEntryThemeComboBox();
            _discoveryForm.ApplyTheme();
        }

        private void buttonSaveTheme_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.InitialDirectory = Path.Combine(Tools.GetAppDataDirectory(), "Theme");
            dlg.DefaultExt = "eddtheme";
            dlg.AddExtension = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _discoveryForm.theme.SaveSettings(dlg.FileName);        // should create a new theme files
                _discoveryForm.theme.LoadThemes();          // make sure up to data - we added a theme, reload them all
                _discoveryForm.theme.Name = Path.GetFileNameWithoutExtension(dlg.FileName); // go to the theme name

                ResetThemeList();

                int curindex = _discoveryForm.theme.GetIndexOfCurrentTheme();       // get theme index.. may be -1 if theme not loaded back

                if (curindex == -1)                                   // if not loaded, back to custom
                    _discoveryForm.theme.SetCustom();   // custom

                SetEntryThemeComboBox();
            }
        }

        public void button_edittheme_Click(object sender, EventArgs e)
        {
            if (themeeditor == null)                    // no theme editor, make one..
            {
                themeeditor = new ThemeEditor();
                themeeditor.InitForm(_discoveryForm);
                themeeditor.FormClosing += close_edit;  // lets see when it closes

                comboBoxTheme.Enabled = false;          // no doing this while theme editor is open
                buttonSaveTheme.Enabled = false;

                themeeditor.Show();                     // run form
            }
            else
                themeeditor.BringToFront();             // its up, make it at front to show it
        }

        private void close_edit(object sender, FormClosingEventArgs e)
        {
            themeeditor = null;                         // called when editor closes
            SetEntryThemeComboBox();
            comboBoxTheme.Enabled = true;          // no doing this while theme editor is open
            buttonSaveTheme.Enabled = true;
        }

        private void buttonSaveSetup_Click(object sender, EventArgs e)
        {
            _discoveryForm.SaveCurrentPopOuts();
        }

        private void buttonReloadSaved_Click(object sender, EventArgs e)
        {
            _discoveryForm.LoadSavedPopouts();
        }
    }
}

