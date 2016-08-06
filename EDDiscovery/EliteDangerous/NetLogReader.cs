using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using EDDiscovery2.DB;
using System.IO;
using EDDiscovery2;
using System.Threading;

namespace EDDiscovery
{
    public class NetLogFileReader : LogReaderBase
    {
        // Header line regular expression
        private static Regex netlogHeaderRe = new Regex(@"^(?<Localtime>\d\d-\d\d-\d\d-\d\d:\d\d) (?<Timezone>.*) [(](?<GMT>\d\d:\d\d) GMT[)]");

        // Public release date of Elite: Dangerous
        DateTime gammastart = new DateTime(2014, 11, 22, 13, 00, 00);

        // Cached list of previous travel log entries
        protected List<VisitedSystemsClass> systems;

        // Close Quarters Combat
        public bool CQC { get; set; }

        // Time and timezone
        public DateTime LastLogTime { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public TimeSpan TimeZoneOffset { get; set; }

        public NetLogFileReader(string filename) : base(filename)
        {
            systems = new List<VisitedSystemsClass>();
        }

        public NetLogFileReader(TravelLogUnit tlu) : base(tlu)
        {
            systems = VisitedSystemsClass.GetAll(tlu);
        }

        protected bool ParseTime(string time)
        {
            TimeSpan logtime;
            TimeSpan lasttime = (LastLogTime + TimeZoneOffset).TimeOfDay;
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

                    return true;
                }
            }

            return false;
        }

        protected bool ParseLineTime(string line)
        {
            if (line.Length > 11 && line[0] == '{' && line[3] == ':' && line[6] == ':' && line[9] == '}' && line[10] == ' ')
            {
                return ParseTime(line.Substring(1, 8));
            }

            return false;
        }

        protected bool ParseVisitedSystem(DateTime time, TimeSpan tzoffset, string line, out VisitedSystemsClass sp)
        {
            sp = new VisitedSystemsClass();

            try
            {
                Regex pattern;

                /* MKW: Use regular expressions to parse the log; much more readable and robust.
                 * Example log entry:

                From  ED  2.1 /1.6
                   {19:21:15} System:"Ooscs Fraae JR-L d8-112" StarPos:(-11609.469,639.594,20141.875)ly  NormalFlight
                string rgexpstr = "{(?<Hour>\\d+):(?<Minute>\\d+):(?<Second>\\d+)} System:\"(?<SystemName>[^\"]+)\" StarPos:\\((?<Pos>.*?)\\)ly( +(?<TravelMode>\\w+))?";

                new from beta3?
                {18:15:14} System:"Pleiades Sector HR-W d1-41" StarPos:(-83.969,-146.156,-334.219)ly Body:0 RelPos:(-1.19887e+07,-9.95573e+06,2.55124e+06)km Supercruise
                string rgexpstr = "{(?<Hour>\\d+):(?<Minute>\\d+):(?<Second>\\d+)} System:\"(?<SystemName>[^\"]+)\" StarPos:\\((?<Pos>.*?)\\)ly Body:(?<Body>\d+) StarPos:\\((?<Pos>.*?)\\)ly( +(?<TravelMode>\\w+))?";


                Pre ED 2.1/1.6
                    {09:36:16} System:0(Thuechea JE-O b11-0) Body:1 Pos:(-6.67432e+009,7.3151e+009,-1.19125e+010) Supercruise

                 * Also, please note that due to E:D bugs, these entries can be at the end of a line as well, not just on a line of their own.
                 * The RegExp below actually just finds the pattern somewhere in the line, so it caters for rubbish at the end too.
                 */

                if (line.Contains("StarPos:")) // new  ED 2.1 format
                {

                    //{(?<Hour>\d+):(?<Minute>\d+):(?<Second>\d+)} System:"(?<SystemName>[^"]+)" StarPos:\((?<Pos>.*?)\)ly( +(?<TravelMode>\w+))?
                    //{(?<Hour>\d+):(?<Minute>\d+):(?<Second>\d+)} System:"(?<SystemName>[^"]+)" StarPos:\((?<Pos>.*?)\)ly( +(?<TravelMode>\w+))?
                    //string rgexpstr = "{(?<Hour>\\d+):(?<Minute>\\d+):(?<Second>\\d+)} System:\"(?<SystemName>[^\"]+)\" StarPos:\\((?<Pos>.*?)\\)ly( +(?<TravelMode>\\w+))?";
                    string rgexpstr;

                    if (line.Contains("Body:"))
                        rgexpstr = "System:\"(?<SystemName>[^\"]+)\" StarPos:\\((?<Pos>.*?)\\)ly Body:(?<Body>\\d+) RelPos:\\((?<RelPos>.*?)\\)km( +(?<TravelMode>\\w+))?";
                    else
                        rgexpstr = "System:\"(?<SystemName>[^\"]+)\" StarPos:\\((?<Pos>.*?)\\)ly( +(?<TravelMode>\\w+))?";

                    pattern = new Regex(rgexpstr);

                    Match match = pattern.Match(line);

                    if (match != null && match.Success)
                    {
                        //sp.Nr = int.Parse(match.Groups["Body"].Value);
                        sp.Name = match.Groups["SystemName"].Value;
                        string pos = match.Groups["Pos"].Value;
                        try
                        {
                            string[] xyzpos = pos.Split(',');
                            var culture = new System.Globalization.CultureInfo("en-US");
                            sp.X = double.Parse(xyzpos[0], culture);
                            sp.Y = double.Parse(xyzpos[1], culture);
                            sp.Z = double.Parse(xyzpos[2], culture);
                        }
                        catch
                        {
                            sp.X = double.NaN;
                            sp.Y = double.NaN;
                            sp.Z = double.NaN;
                        }

                    }
                    else
                    {
                        System.Diagnostics.Trace.WriteLine("System parse error 1:" + line);
                        return false;
                    }

                }
                else
                {
                    pattern = new Regex(@"System:\d+\((?<SystemName>.*?)\) Body:(?<Body>\d+) Pos:\(.*?\)( (?<TravelMode>\w+))?");
                    Match match = pattern.Match(line);

                    if (match != null && match.Success)
                    {
                        //sp.Nr = int.Parse(match.Groups["Body"].Value);
                        sp.Name = match.Groups["SystemName"].Value;
                        sp.X = Double.NaN;
                        sp.Y = Double.NaN;
                        sp.Z = Double.NaN;
                    }
                    else
                    {
                        System.Diagnostics.Trace.WriteLine("System parse error 2:" + line);
                        return false;
                    }
                }

                sp.Time = time + tzoffset;

                return true;
            }
            catch
            {
                // MKW TODO: should we log bad lines?
                return false;
            }
        }

        public bool ReadNetLogSystem(out VisitedSystemsClass vsc, Func<bool> cancelRequested = null)
        {
            if (cancelRequested == null)
                cancelRequested = () => false;

            string line;
            while (!cancelRequested() && this.ReadLine(out line))
            {
                ParseLineTime(line);

                if (line.Contains("[PG]"))
                {
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
                }

                int offset = line.IndexOf("} System:") - 8;
                if (offset >= 1 && ParseTime(line.Substring(offset, 8)) && this.CQC == false)
                {
                    //Console.WriteLine(" RD:" + line );
                    if (line.Contains("ProvingGround"))
                        continue;

                    VisitedSystemsClass ps;
                    if (ParseVisitedSystem(this.LastLogTime, this.TimeZoneOffset, line.Substring(offset + 10), out ps))
                    {   // Remove some training systems
                        if (ps.Name.Equals("Training"))
                            continue;
                        if (ps.Name.Equals("Destination"))
                            continue;
                        if (ps.Name.Equals("Altiris"))
                            continue;
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
                    LastLogTime = localtime - tzoffset;
                    TimeZone = tzi;
                    TimeZoneOffset = tzoffset;
                    TravelLogUnit.type = 1;
                }

                return true;
            }

            return false;
        }

        public IEnumerable<VisitedSystemsClass> ReadSystems(Func<bool> cancelRequested = null)
        {
            if (cancelRequested == null)
                cancelRequested = () => false;

            VisitedSystemsClass last = null;
            long startpos = filePos;

            if (TimeZone == null)
            {
                if (!ReadHeader())  // may be empty if we read it too fast.. don't worry, monitor will pick it up
                {
                    System.Diagnostics.Trace.WriteLine("File was empty (for now) " + FileName);
                    yield break;
                }
            }

            VisitedSystemsClass ps;
            while (!cancelRequested() && ReadNetLogSystem(out ps, cancelRequested))
            {
                if (last == null)
                {
                    if (systems.Count == 0)
                    {
                        last = VisitedSystemsClass.GetLast(EDDConfig.Instance.CurrentCmdrID, ps.Time);
                    }
                    else
                    {
                        last = systems[systems.Count - 1];
                    }
                }

                if (last != null && ps.Name.Equals(last.Name, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                if (ps.Time.Subtract(gammastart).TotalMinutes > 0)  // Ta bara med efter gamma.
                {
                    systems.Add(ps);
                    yield return ps;
                    last = ps;
                }
            }

            if ( startpos != filePos )
                Console.WriteLine("Parse ReadData " + FileName + " from " + startpos + " to " + filePos);
        }
    }
}
