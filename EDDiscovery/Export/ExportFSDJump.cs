using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery.Export
{
    public class ExportFSDJump : ExportBase
    {
        private List<JournalEntry> scans;


        override public bool GetData(EDDiscoveryForm _discoveryForm)
        {
            var filter = _discoveryForm.TravelControl.GetPrimaryFilter;

            List<HistoryEntry> result = filter.Filter(_discoveryForm.history);

            scans = new List<JournalEntry>();

            scans = JournalEntry.GetByEventType(JournalTypeEnum.FSDJump, EDDiscoveryForm.EDDConfig.CurrentCmdrID, _discoveryForm.history.GetMinDate, _discoveryForm.history.GetMaxDate);

            return true;
        }


        override public bool ToCSV(string filename )
        {
            try { 
            using (StreamWriter writer = new StreamWriter(filename))
            {
                // Write header
                writer.Write("Time" + delimiter);
                writer.Write("Name" + delimiter);
                writer.Write("X" + delimiter);
                writer.Write("Y" + delimiter);
                writer.Write("Z" + delimiter);
                writer.Write("Distance" + delimiter);
                writer.Write("Fuel used" + delimiter);
                writer.Write("Fuel left" + delimiter);
                writer.Write("Boost" + delimiter);
                writer.Write("Note" + delimiter);

                writer.WriteLine();

                foreach (var je in scans)
                {
                    if (je.EventTypeID == JournalTypeEnum.FSDJump)
                    {
                        JournalFSDJump ev = je as JournalFSDJump;

                        writer.Write(MakeValueCsvFriendly(ev.EventTimeLocal));
                        writer.Write(MakeValueCsvFriendly(ev.StarSystem));
                        writer.Write(MakeValueCsvFriendly(ev.StarPos.X));
                        writer.Write(MakeValueCsvFriendly(ev.StarPos.Y));
                        writer.Write(MakeValueCsvFriendly(ev.StarPos.Z));
                        writer.Write(MakeValueCsvFriendly(ev.JumpDist));
                        writer.Write(MakeValueCsvFriendly(ev.FuelUsed));
                        writer.Write(MakeValueCsvFriendly(ev.FuelLevel));
                        writer.Write(MakeValueCsvFriendly(ev.BoostUsed));
//                        writer.Write(MakeValueCsvFriendly(je.System.SystemNote));

                        writer.WriteLine();
                    }
                }
            }

                return true;
            }
            catch (IOException)
            {
                MessageBox.Show(String.Format("Is file {0} open?", filename), "Export Scan",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


    }
}
