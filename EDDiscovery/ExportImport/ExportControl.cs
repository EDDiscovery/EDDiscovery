using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery.Export;
using EDDiscovery.EliteDangerous;
using System.Diagnostics;
using EDDiscovery.EliteDangerous.JournalEvents;
using System.IO;
using System.Globalization;
using EDDiscovery2;
using EDDiscovery.Import;

namespace EDDiscovery
{
    public partial class ExportControl : UserControl
    {
        private EDDiscoveryForm _discoveryForm;

        private List<ExportTypeClass> exportTypeList;
        private List<EDCommander> commanders;
        private string importFile = string.Empty;

        public ExportControl()
        {
            InitializeComponent();

            exportTypeList = new List<ExportTypeClass>();

            exportTypeList.Add(new ExportTypeClass("Exploration scans (all)", new ExportScan()));
            exportTypeList.Add(new ExportTypeClass("Exploration scans (Stars)", new ExportScan(true, false)));
            exportTypeList.Add(new ExportTypeClass("Exploration scans (Planets)", new ExportScan(false, true)));
            exportTypeList.Add(new ExportTypeClass("Travel history", new ExportFSDJump()));
            exportTypeList.Add(new ExportTypeClass("Sold exploration data (all)", new ExportExplorationData(false)));
            exportTypeList.Add(new ExportTypeClass("Sold exploration data (By date)", new ExportExplorationData(true)));
            exportTypeList.Add(new ExportTypeClass("Route plan", new ExportRoute()));
            exportTypeList.Add(new ExportTypeClass("All Notes from All Commanders", new ExportNotes()));

            txtExportVisited.SetAutoCompletor(EDDiscovery.DB.SystemClass.ReturnSystemListForAutoComplete);

            comboBoxCustomExportType.ItemHeight = 20;
            
        }

        public void PopulateCommanders()
        {
            comboBoxCommander.Enabled = false;
            commanders = new List<EDCommander>();
            
            commanders.AddRange(EDDConfig.Instance.ListOfCommanders);

            comboBoxCommander.DataSource = null;
            comboBoxCommander.DataSource = commanders;
            comboBoxCommander.ValueMember = "Nr";
            comboBoxCommander.DisplayMember = "Name";

            Application.DoEvents();

            if (_discoveryForm.DisplayedCommander == -1)
                comboBoxCommander.SelectedIndex = 0;
            else
                comboBoxCommander.SelectedItem = EDDiscoveryForm.EDDConfig.CurrentCommander;

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
                    exptype.export.Csvformat = CSVFormat.EU;
                else
                    exptype.export.Csvformat = CSVFormat.USA_UK;

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
            public ExportBase export;

            public ExportTypeClass(string name, ExportBase exportclass)
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

            scans = JournalEntry.GetByEventType(JournalTypeEnum.FSDJump, EDDiscoveryForm.EDDConfig.CurrentCmdrID, new DateTime (2014, 1,1), DateTime.UtcNow) ;

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
                MessageBox.Show(this, "ImportStars.txt has been created in " + exportfilename + Environment.NewLine
                    + (found ? "Restart Elite Dangerous to have this file read into the galaxy map" : ""), "Export visited stars");
            }
            catch (IOException)
            {
                MessageBox.Show(String.Format("Is file {0} open?", exportfilename), "Export visited stars", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Multiple commanders found. Will export to latest played in Elite Dangerous");
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
                MessageBox.Show("Could not find VisitedStarsCache.dat file, in commander folder","Open folder");
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
            InfoForm dl = new InfoForm();
            string text = EDDiscovery.Properties.Resources.ImportHelp;
            dl.Info("Import Help", text, new Font("Microsoft Sans Serif", 10), new int[] { 50, 200, 400 });
            dl.Show();
        }
        
        private void buttonImport_Click(object sender, EventArgs e)
        {
            long cmdrID = long.Parse(comboBoxCommander.SelectedValue.ToString());
            if (string.IsNullOrEmpty(importFile) || ! File.Exists(importFile))
            {
                MessageBox.Show("An import file must be specified.", "EDD Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string delim = radioButtonTab.Checked ? "\t" : textBoxDelimiter.Text;
            if (string.IsNullOrEmpty(delim))
            {
                MessageBox.Show("A delimiter must be defined.", "EDD Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int? datecol = string.IsNullOrEmpty(textBoxArrivalDate.Text) ? null : (int?)int.Parse(textBoxArrivalDate.Text);
            int? timecol = string.IsNullOrEmpty(textBoxArrivalTime.Text) ? null : (int?)int.Parse(textBoxArrivalTime.Text);
            int? namecol = string.IsNullOrEmpty(textBoxSysName.Text) ? null : (int?)int.Parse(textBoxSysName.Text);
            int? notecol = string.IsNullOrEmpty(textBoxSysNotes.Text) ? null : (int?)int.Parse(textBoxSysNotes.Text);
            if (!namecol.HasValue)
            {
                MessageBox.Show("System Name column must be defined.", "EDD Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!datecol.HasValue && !notecol.HasValue)
            {
                MessageBox.Show("At least one of arrival date and system note columns must be defined.", "EDD Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ImportHistory ih = new ImportHistory(importFile, delim, datecol, timecol, namecol, notecol, checkBoxImpHeader.Checked, cmdrID);
            _discoveryForm.ShowInfoPanel("Importing, please wait...", true, Color.Gold);
            string result;
            if (ih.Import(out result))
            {
                MessageBox.Show("Import successful.", "EDD Import", MessageBoxButtons.OK, MessageBoxIcon.None);
                _discoveryForm.RefreshHistoryAsync();
            }
            else
            {
                MessageBox.Show("Import failed: " + result, "EDD Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            _discoveryForm.ShowInfoPanel("", false);
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
    }
}
