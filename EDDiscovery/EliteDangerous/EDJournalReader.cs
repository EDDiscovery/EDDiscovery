using EDDiscovery2.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous
{
    public class EDJournalReader : LogReaderBase
    {


        public EDJournalReader(string filename) : base(filename) { }
        public EDJournalReader(TravelLogUnit tlu) : base(tlu) { }


        public DateTime LastLogTime { get; set; }

        public TimeZoneInfo TimeZone { get; set; }
        public TimeSpan TimeZoneOffset { get; set; }

        public bool ReadJournalLog(out JournalEntry vsc)
        {
            string line;
            while (this.ReadLine(out line))
            {
                // Todo  create JournalEvent
                vsc = null;
                return true;
            }

            vsc = null;
            return false;
        }

        public bool ReadHeader()
        {
            string line = null;

            // Try to read the first line of the log file
            try
            {
                using (Stream stream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (TextReader reader = new StreamReader(stream))
                    {
                        line = reader.ReadLine();
                    }
                }
            }
            catch
            {
            }

            // File may not have been written yet.
            if (line == null)
            {
                return false;
            }

            // Extract the start time from the first line
            //Match match = netlogHeaderRe.Match(line);
            //if (match != null && match.Success)
            /*
            {
                string localtimestr = match.Groups["Localtime"].Value;
                string timezonename = match.Groups["Timezone"].Value.Trim();
                string gmtimestr = match.Groups["GMT"].Value;

                DateTime localtime = DateTime.MinValue;
                TimeSpan gmtime = TimeSpan.MinValue;
                TimeZoneInfo tzi = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(t => t.DaylightName.Trim() == timezonename || t.StandardName.Trim() == timezonename);

                if (DateTime.TryParseExact(localtimestr, "yy-MM-dd-HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out localtime) &&
                    TimeSpan.TryParseExact(gmtimestr, "h\\:mm", CultureInfo.InvariantCulture, out gmtime))
                {
                    // Grab the timezone offset
                    TimeSpan tzoffset = localtime.TimeOfDay - gmtime;

                    if (tzi != null)
                    {
                        // Correct for wildly inaccurate values
                        if (tzoffset > tzi.BaseUtcOffset + TimeSpan.FromHours(18))
                        {
                            tzoffset -= TimeSpan.FromHours(24);
                        }
                        else if (tzoffset < tzi.BaseUtcOffset - TimeSpan.FromHours(18))
                        {
                            tzoffset += TimeSpan.FromHours(24);
                        }
                    }

                    // Set the start time, timezone info and timezone offset
                    LastLogTime = localtime;
                    TimeZone = tzi;
                    TimeZoneOffset = tzoffset;
                    TravelLogUnit.type = 3;
                }
                
                return true;
            }
            */
            return false;
        }

    }


}
