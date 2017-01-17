using EDDiscovery.Export;
using EDDiscovery2.EDSM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.ExportImport
{
    class ExportSphereSystems
    {
        private EDDiscoveryForm _discoveryForm;

        public ExportSphereSystems(EDDiscoveryForm _discoveryForm)
        {
            this._discoveryForm = _discoveryForm;
        }

        public void Execute(String systemName, double radius)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "SphereSystems export| *.txt";
            dlg.Title = TITLE;
            dlg.FileName = "SphereSystems.txt";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            String fileName = dlg.FileName;
            EDSMClass edsm = new EDSMClass();
            Task taskEDSM = Task<List<String>>.Factory.StartNew(() =>
            {
                return edsm.GetSphereSystems(systemName, radius);

            }).ContinueWith(task => ExportFile(task, fileName));
        }

        private String TITLE = "Export Sphere Systems";

        private void ExportFile(Task<List<String>> task, String filename)
        {

            try
            {
                using (StreamWriter writer = new StreamWriter(filename, false))
                {

                    foreach (String system in task.Result)
                    {
                        writer.WriteLine(system);
                    }
                }
               // MessageBox.Show(String.Format("Export complete {0}",
               //    filename), TITLE);
            }
            catch (IOException)
            {
              //  MessageBox.Show(String.Format("Is file {0} open?", filename), TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


    }
}
