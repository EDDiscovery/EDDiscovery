﻿/*
 * Copyright © 2016 EDDiscovery development team
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
using EDDiscovery.Export;
using System.Diagnostics;
using System.IO;
using System.Globalization;

using EDDiscovery.Import;
using EliteDangerousCore.DB;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

namespace EDDiscovery.Export
{
    public partial class ExportControl : UserControl
    {
        private EDDiscoveryForm _discoveryForm;

        private List<ExportTypeClass> exportTypeList;
        private string importFile = string.Empty;

        public ExportControl()
        {
            InitializeComponent();

            this.textBoxArrivalDate.SetToolTip(toolTip, "Column containing the arrival date");
            this.textBoxSysName.SetToolTip(toolTip, "Column containing the system name");
            this.textBoxSysNotes.SetToolTip(toolTip, "Column containing any system notes");
            this.textBoxArrivalTime.SetToolTip(toolTip, "Column containing the arrival time");
            this.textBoxDelimiter.SetToolTip(toolTip, "Delimiting character");

            exportTypeList = new List<ExportTypeClass>();

            exportTypeList.Add(new ExportTypeClass("Exploration scans (all)", new ExportScan()));
            exportTypeList.Add(new ExportTypeClass("Exploration scans (Stars)", new ExportScan(true, false)));
            exportTypeList.Add(new ExportTypeClass("Exploration scans (Planets)", new ExportScan(false, true)));
            exportTypeList.Add(new ExportTypeClass("Travel history", new ExportFSDJump()));
            exportTypeList.Add(new ExportTypeClass("Sold exploration data (all)", new ExportExplorationData(false)));
            exportTypeList.Add(new ExportTypeClass("Sold exploration data (By date)", new ExportExplorationData(true)));
            exportTypeList.Add(new ExportTypeClass("Route plan", new ExportRoute()));
            exportTypeList.Add(new ExportTypeClass("All Notes from All Commanders", new ExportNotes()));
            exportTypeList.Add(new ExportTypeClass("Exploration list (star data)", new ExportScan(true, false, true)));
            exportTypeList.Add(new ExportTypeClass("Exploration list (planet data)", new ExportScan(false, true, true)));


            txtExportVisited.SetAutoCompletor(SystemClassDB.ReturnSystemListForAutoComplete);

            comboBoxCustomExportType.ItemHeight = 20;

            double ly = 10.0;
            txtsphereRadius.Text = ly.ToString("0.00");
            
        }

        public void PopulateCommanders()
        {
            comboBoxCommander.Enabled = false;      // in case its got a on selection item hook
            comboBoxCommander.Items.Clear();            // comboBox is nicer with items
            comboBoxCommander.Items.AddRange((from EDCommander c in EDCommander.GetList() select c.Name).ToList());
            comboBoxCommander.SelectedItem = EDCommander.Current.Name;
            comboBoxCommander.Enabled = true;
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
        }


        private void ExportControl_Load(object sender, EventArgs e)
        {
            comboBoxCustomExportType.Items.Clear();

            if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Equals("."))
                radioButtonCustomUSAUK.Checked = true;
            else
                radioButtonCustomEU.Checked = true;


            foreach (ExportTypeClass exp in exportTypeList)
                comboBoxCustomExportType.Items.Add(exp.Name);

            comboBoxCustomExportType.SelectedIndex = 0;

        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            //ExportBase export = new ExportScan();

            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "CSV export| *.csv";
            dlg.Title = "Export scan data to Excel (csv)";

            ExportTypeClass exptype = exportTypeList[comboBoxCustomExportType.SelectedIndex];

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (radioButtonCustomEU.Checked)
                    exptype.export.Csvformat = BaseUtils.CVSWrite.CSVFormat.EU;
                else
                    exptype.export.Csvformat = BaseUtils.CVSWrite.CSVFormat.USA_UK;

                exptype.export.IncludeHeader = checkBoxCustomIncludeHeader.Checked;

                //Check for failed getdata or failed CSV
                if (!exptype.export.GetData(_discoveryForm))
                    return;
                if (!exptype.export.ToCSV(dlg.FileName))
                    return;

                if (checkBoxCustomAutoOpen.Checked)
                        Process.Start(dlg.FileName);
                
            }

        }


        private class ExportTypeClass
        {
            public string Name;
            public BaseUtils.CVSWrite export;

            public ExportTypeClass(string name, BaseUtils.CVSWrite exportclass)
            {
                Name = name;
                export = exportclass;
            }

        }

        private void buttonExportToGalmap_Click(object sender, EventArgs e)
        {
            List<JournalEntry> scans = new List<JournalEntry>();
            String folder = findVisitedStarsCacheDirectory();
            String exportfilename;
            Boolean found = false;
            if (folder != null)
            {
                found = true;
                exportfilename = Path.Combine(folder, "ImportStars.txt");
            }
            else
            {
                SaveFileDialog dlg = new SaveFileDialog();

                dlg.Filter = "ImportedStars export| *.txt";
                dlg.Title = "Could not find VisitedStarsCache.dat file";
                dlg.FileName = "ImportStars.txt";

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                exportfilename = dlg.FileName;
            }

            scans = JournalEntry.GetByEventType(JournalTypeEnum.FSDJump, EDCommander.CurrentCmdrID, new DateTime (2014, 1,1), DateTime.UtcNow) ;

            var tscans = scans.ConvertAll<JournalFSDJump>(x=>(JournalFSDJump)x);
            try
            {
                using (StreamWriter writer = new StreamWriter(exportfilename, false))
                {

                    foreach (var system in tscans.Select(o => o.StarSystem).Distinct())
                    {
                        writer.WriteLine(system);
                    }
                }
                ExtendedControls.MessageBoxTheme.Show(this, "ImportStars.txt has been created in " + exportfilename + Environment.NewLine
                    + (found ? "Restart Elite Dangerous to have this file read into the galaxy map" : ""), "Export visited stars");
            }
            catch (IOException)
            {
                ExtendedControls.MessageBoxTheme.Show(String.Format("Is file {0} open?", exportfilename), "Export visited stars", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void buttonExportToFilteredSystems_Click(object sender, EventArgs e)
        {
            new ExportFilteredSystems().Execute(txtExportVisited.Text);

        }

        String findVisitedStarsCacheDirectory()
        {
            string[] allFiles;
            try
            {
                string EDimportstarsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Frontier Developments", "Elite Dangerous");
                allFiles = Directory.GetFiles(EDimportstarsDir, "VisitedStarsCache.dat", SearchOption.AllDirectories);
            }
            catch (IOException)
            {
                return null;
            }

            if (allFiles.Count<string>() == 0)
            {
                return null;
            }

            String folder = null;
            if (allFiles.Count<string>() > 1)  // signle account  just export
            {
                ExtendedControls.MessageBoxTheme.Show("Multiple commanders found. Will export to latest played in Elite Dangerous");
                DirectoryInfo newesetDi = null;
                for (int ii = 0; ii < allFiles.Count<string>(); ii++)
                {
                    DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(allFiles[ii]));

                    if (newesetDi == null)
                        newesetDi = di;

                    if (di.LastWriteTimeUtc > newesetDi.LastWriteTimeUtc)
                        newesetDi = di;
                }
                folder = newesetDi.FullName;
            }
            else
            {
                folder = Path.GetDirectoryName(allFiles[0]);
            }

            if (folder == null || folder.Trim().Length == 0)
                return null;

            return folder;
        }

        private void buttonExportOpenFolder_Click(object sender, EventArgs e)
        {
            String folder = findVisitedStarsCacheDirectory();
            if (folder == null)
            {
                ExtendedControls.MessageBoxTheme.Show("Could not find VisitedStarsCache.dat file, in commander folder","Open folder");
                return;
            }
            Process.Start(folder);
        }

        string lastsys = null;

        public void UpdateHistorySystem(string str)
        {
            lastsys = str;
        }

        private void btnExportTravel_Click(object sender, EventArgs e)
        {
                txtExportVisited.Text = lastsys;
        }

        private void buttonImportHelp_Click(object sender, EventArgs e)
        {
            ExtendedControls.InfoForm dl = new ExtendedControls.InfoForm();
            string text = EDDiscovery.Properties.Resources.ImportHelp;
            dl.Info("Import Help", Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location),
                        text, new Font("Microsoft Sans Serif", 10), new int[] { 50, 200, 400 });
            dl.Show();
        }
        
        private void buttonImport_Click(object sender, EventArgs e)
        {
            var itm = (from EDCommander c in EDCommander.GetList() where c.Name.Equals(comboBoxCommander.Text) select c).ToList();

            if (itm == null || itm.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show("Code failure - cannot find selected commander", "EDD Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long cmdrID = itm[0].Nr;

            if (string.IsNullOrEmpty(importFile) || ! File.Exists(importFile))
            {
                ExtendedControls.MessageBoxTheme.Show("An import file must be specified.", "EDD Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string delim = radioButtonTab.Checked ? "\t" : textBoxDelimiter.Text;
            if (string.IsNullOrEmpty(delim))
            {
                ExtendedControls.MessageBoxTheme.Show("A delimiter must be defined.", "EDD Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int? datecol = string.IsNullOrEmpty(textBoxArrivalDate.Text) ? null : (int?)int.Parse(textBoxArrivalDate.Text);
            int? timecol = string.IsNullOrEmpty(textBoxArrivalTime.Text) ? null : (int?)int.Parse(textBoxArrivalTime.Text);
            int? namecol = string.IsNullOrEmpty(textBoxSysName.Text) ? null : (int?)int.Parse(textBoxSysName.Text);
            int? notecol = string.IsNullOrEmpty(textBoxSysNotes.Text) ? null : (int?)int.Parse(textBoxSysNotes.Text);
            if (!namecol.HasValue)
            {
                ExtendedControls.MessageBoxTheme.Show("System Name column must be defined.", "EDD Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!datecol.HasValue && !notecol.HasValue)
            {
                ExtendedControls.MessageBoxTheme.Show("At least one of arrival date and system note columns must be defined.", "EDD Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ImportHistory ih = new ImportHistory(importFile, delim, datecol, timecol, namecol, notecol, checkBoxImpHeader.Checked, cmdrID);
            string result;
            if (ih.Import(out result))
            {
                ExtendedControls.MessageBoxTheme.Show("Import successful.", "EDD Import", MessageBoxButtons.OK, MessageBoxIcon.None);
                _discoveryForm.RefreshHistoryAsync();
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show("Import failed: " + result, "EDD Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ih = null;
        }

        private void buttonImportFile_Click_1(object sender, EventArgs e)
        {
            DialogResult dr = selectImportFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                importFile = selectImportFileDialog.FileName;
                labelExt2.Text = "File: " + importFile.Substring(importFile.LastIndexOf("\\") + 1);
            }
        }

        private void ValidateColumnInput(ExtendedControls.TextBoxBorder txtBox)
        {
            int dummy;
            if (!(string.IsNullOrEmpty(txtBox.Text) || int.TryParse(txtBox.Text, out dummy)))
            { txtBox.Text = ""; }
        }

        private void textBoxArrivalDate_Validating(object sender, CancelEventArgs e)
        {
            ValidateColumnInput(textBoxArrivalDate);
        }

        private void textBoxSysName_Validating(object sender, CancelEventArgs e)
        {
            ValidateColumnInput(textBoxSysName);
        }

        private void textBoxArrivalTime_TextChanged(object sender, EventArgs e)
        {
            ValidateColumnInput(textBoxArrivalTime);
        }

        private void textBoxSysNotes_TextChanged(object sender, EventArgs e)
        {
            ValidateColumnInput(textBoxSysNotes);
        }

        private void btnSphereSystems_Click(object sender, EventArgs e)
        {
            //txtExportVisited.Text
            double r = 0;
            if (Double.TryParse(txtsphereRadius.Text, out r))
                new ExportSphereSystems(_discoveryForm).Execute(txtExportVisited.Text, r); 

        }
    }
}
