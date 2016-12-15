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

namespace EDDiscovery
{
    public partial class ExportControl : UserControl
    {
        private EDDiscoveryForm _discoveryForm;

        private List<ExportTypeClass> exportTypeList;

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

            txtExportVisited.SetAutoCompletor(EDDiscovery.DB.SystemClass.ReturnSystemListForAutoComplete);

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
            if (folder == null)
            {
                MessageBox.Show("Could not find VisitedStarsCache.dat file");
                return;
            }
           String exportfilename = Path.Combine(folder, "ImportStars.txt");


            scans = JournalEntry.GetByEventType(JournalTypeEnum.FSDJump, EDDiscoveryForm.EDDConfig.CurrentCmdrID, new DateTime (2014, 1,1), DateTime.UtcNow) ;

            var tscans = scans.ConvertAll<JournalFSDJump>(x=>(JournalFSDJump)x);

            using (StreamWriter writer = new StreamWriter(exportfilename, false))
            {

                foreach (var system in tscans.Select(o => o.StarSystem).Distinct())
                {
                    writer.WriteLine(system);
                }
            }

            MessageBox.Show(this,"ImportStars.txt has been created in " + exportfilename + Environment.NewLine + "Restart Elite Dangerous to have this file read into the galaxy map");
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonExportToFilteredSystems_Click(object sender, EventArgs e)
        {
            new ExportFilteredSystems().Execute(txtExportVisited.Text);

        }

        String findVisitedStarsCacheDirectory()
        {
            string EDimportstarsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Frontier Developments", "Elite Dangerous");
            var allFiles = Directory.GetFiles(EDimportstarsDir, "VisitedStarsCache.dat", SearchOption.AllDirectories);

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
                MessageBox.Show("Could not find VisitedStarsCache.dat file");
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

    }
}
