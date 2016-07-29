using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using EDDiscovery2.DB;
using System.IO;

namespace EDDiscovery
{
    public class NetLogFileReader : LogReaderBase
    {
        // Header line regular expression
        private static Regex netlogHeaderRe = new Regex(@"^(?<Localtime>\d\d-\d\d-\d\d-\d\d:\d\d) (?<Timezone>.*) [(](?<GMT>\d\d:\d\d) GMT[)]");

        // Close Quarters Combat
        public bool CQC { get; set; }

        // Time and timezone
        public DateTime LastLogTime { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public TimeSpan TimeZoneOffset { get; set; }

        public NetLogFileReader(string filename) : base(filename) { }
        public NetLogFileReader(TravelLogUnit tlu) : base(tlu) { }

        protected void ParseTime(string time)
        {
            TimeSpan logtime;
            TimeSpan lasttime = LastLogTime.TimeOfDay;
            if (TimeSpan.TryParseExact(time, "h\\:mm\\:ss", CultureInfo.InvariantCulture, out logtime))
            {
                if (logtime.TotalHours >= 0 && logtime.TotalHours < 24)
                {
                    TimeSpan timedelta = logtime - lasttime;
                    if (timedelta < TimeSpan.FromHours(-4))
                    {
                        // Midnight crossing - add 1 day
                        timedelta += TimeSpan.FromHours(24);
                    }
                    else if (timedelta < TimeSpan.Zero)
                    {
                        // Computer time jumped backwards - decrease timezone offset
                        TimeZoneOffset += timedelta;
                    }
                    else if (timedelta > TimeSpan.FromHours(20))
                    {
                        // Computer time jumped backwards - decrease timezone offset
                        timedelta -= TimeSpan.FromHours(24);
                        TimeZoneOffset += timedelta;
                    }

                    LastLogTime += timedelta;
                }
            }
        }

        protected void ParseLineTime(string line)
        {
            if (line[0] == '{' && line[3] == ':' && line[6] == ':' && line[9] == '}' && line[10] == ' ')
            {
                ParseTime(line.Substring(1, 8));
            }
        }

        public bool ReadNetLogSystem(out VisitedSystemsClass vsc)
        {
            string line;
            while (this.ReadLine(out line))
            {
                ParseLineTime(line);

                if (line.Contains("[PG] [Notification] Left a playlist lobby"))
                    this.CQC = false;

                if (line.Contains("[PG] Destroying playlist lobby."))
                    this.CQC = false;

                if (line.Contains("[PG] [Notification] Joined a playlist lobby"))
                    this.CQC = true;
                if (line.Contains("[PG] Created playlist lobby"))
                    this.CQC = true;
                if (line.Contains("[PG] Found matchmaking lobby object"))
                    this.CQC = true;

                if (line.Contains(" System:") && this.CQC == false)
                {
                    //Console.WriteLine(" RD:" + line );
                    if (line.Contains("ProvingGround"))
                        continue;

                    VisitedSystemsClass ps = VisitedSystemsClass.Parse(this.LastLogTime, line);
                    if (ps != null)
                    {   // Remove some training systems
                        if (ps.Name.Equals("Training"))
                            continue;
                        if (ps.Name.Equals("Destination"))
                            continue;
                        if (ps.Name.Equals("Altiris"))
                            continue;
                        this.LastLogTime = ps.Time;
                        ps.Source = TravelLogUnit.id;
                        ps.Unit = TravelLogUnit.Name;
                        vsc = ps;
                        return true;
                    }
                }
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
            Match match = netlogHeaderRe.Match(line);
            if (match != null && match.Success)
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
                    TravelLogUnit.type = 1;
                }

                return true;
            }

            return false;
        }
    }
}
