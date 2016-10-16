using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EDDiscovery.Export
{
    public class ExportFSDJump : ExportBase
    {
        private List<JournalEntry> scans;
        List<HistoryEntry> historyJumps;

        override public bool GetData(EDDiscoveryForm _discoveryForm)
        {
            var filter = (TravelHistoryFilter)_discoveryForm.TravelControl.comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;

            historyJumps = filter.Filter(_discoveryForm.history);

            return true;
        }


        override public bool ToCSV(string filename )
        {

            using (StreamWriter writer = new StreamWriter(filename))
            {
                // Write header
                writer.Write("Time" + delimiter);
                writer.Write("Name" + delimiter);
                writer.Write("Distance" + delimiter);
                writer.Write("Note" + delimiter);

                writer.WriteLine();

                foreach (HistoryEntry je in historyJumps)
                {
                    if (je.EntryType == JournalTypeEnum.FSDJump)
                    {
                        

                        writer.Write(MakeValueCsvFriendly(je.EventTimeLocal));
                        writer.Write(MakeValueCsvFriendly(je.System.name));
                        writer.Write(MakeValueCsvFriendly(0));
                        writer.Write(MakeValueCsvFriendly(je.System.SystemNote));

                        writer.WriteLine();
                    }
                }
            }

            return true;
        }


    }
}
