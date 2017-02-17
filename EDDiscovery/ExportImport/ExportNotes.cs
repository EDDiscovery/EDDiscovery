/*
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
using EDDiscovery.DB;
using EMK.LightGeometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Export
{
    class ExportNotes : ExportBase
    {
        private const string TITLE = "Export Notes";

        public override bool GetData(EDDiscoveryForm _discoveryForm)
        {
            return true;
        }

        public override bool ToCSV(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    if (IncludeHeader)
                    {
                        writer.Write("StarName" + delimiter);
                        writer.Write("JournalEntry" + delimiter);
                        writer.Write("EDSMID" + delimiter);
                        writer.Write("Time" + delimiter);
                        writer.Write("Note");
                        writer.WriteLine();
                    }

                    EDDiscovery2.DB.SystemNoteClass.GetAllSystemNotes();

                    foreach (EDDiscovery2.DB.SystemNoteClass snc in EDDiscovery2.DB.SystemNoteClass.globalSystemNotes)
                    {
                        writer.Write(MakeValueCsvFriendly(snc.Name.Length > 0 ? snc.Name : "N/A"));
                        writer.Write(MakeValueCsvFriendly(snc.Journalid > 0 ? snc.Journalid.ToString() : "N/A"));
                        writer.Write(MakeValueCsvFriendly(snc.EdsmId > 0 ? snc.EdsmId.ToString() : "N/A"));
                        writer.Write(MakeValueCsvFriendly(snc.Time));
                        writer.Write(MakeValueCsvFriendly(snc.Note,false));
                        writer.WriteLine();
                    }
                }

                return true;
            }
            catch (IOException )
            {
                MessageBox.Show(String.Format("Is file {0} open?", filename), TITLE,
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
    }
}
