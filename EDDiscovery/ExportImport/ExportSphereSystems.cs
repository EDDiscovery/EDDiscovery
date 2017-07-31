/*
 * Copyright © 2017 EDDiscovery development team
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
using EDDiscovery.Export;
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Export
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
               // ExtendedControls.MessageBoxTheme.Show(String.Format("Export complete {0}",
               //    filename), TITLE);
            }
            catch (IOException)
            {
              //  ExtendedControls.MessageBoxTheme.Show(String.Format("Is file {0} open?", filename), TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


    }
}
