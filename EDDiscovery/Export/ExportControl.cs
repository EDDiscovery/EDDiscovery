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

        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
        }


        private void ExportControl_Load(object sender, EventArgs e)
        {
            comboBoxCustomExportType.Items.Clear();

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

                exptype.export.GetData(_discoveryForm);
                exptype.export.ToCSV(dlg.FileName);
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
    }


  

}
