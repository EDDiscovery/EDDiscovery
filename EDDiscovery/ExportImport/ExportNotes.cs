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
