using EDDiscovery.DB;
using EDDiscovery2.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public class SystemPosition
    {
        public DateTime time;
        public string Name;
        public int Nr;
        public int BodyNr;
        public float x, y, z;  // New in ED 2.1

        public ISystem curSystem;
        public ISystem prevSystem;
        public ISystem lastKnownSystem;
        public string strDistance;
        public IVisitedSystems vs;
        

        public SystemPosition()
        {
        }

        public SystemPosition(IVisitedSystems vs)
        {
            Name = vs.Name;
            time = vs.Time;
            this.vs = vs;
        }

        public void Update()
        {
            if (this.vs is VisitedSystemsClass)
            {
                VisitedSystemsClass vsc = (VisitedSystemsClass)vs;
                vsc.Update();
            }
        }

        static public SystemPosition Parse(DateTime lasttime, string line)
        {
            SystemPosition sp = new SystemPosition();

            try
            {
                Regex pattern;
                int hour, min, sec;

                /* MKW: Use regular expressions to parse the log; much more readable and robust.
                 * Example log entry:
                
                From  ED  2.1 /1.6
                   {19:21:15} System:"Ooscs Fraae JR-L d8-112" StarPos:(-11609.469,639.594,20141.875)ly  NormalFlight
                
                Pre ED 2.1/1.6
                    {09:36:16} System:0(Thuechea JE-O b11-0) Body:1 Pos:(-6.67432e+009,7.3151e+009,-1.19125e+010) Supercruise
                 
                 * Also, please note that due to E:D bugs, these entries can be at the end of a line as well, not just on a line of their own.
                 * The RegExp below actually just finds the pattern somewhere in the line, so it caters for rubbish at the end too.
                 */

                if (line.Contains("StarPos:")) // new  ED 2.1 format
                {

                    //{(?<Hour>\d+):(?<Minute>\d+):(?<Second>\d+)} System:"(?<SystemName>[^"]+)" StarPos:\((?<Pos>.*?)\)ly( +(?<TravelMode>\w+))?
                    //{(?<Hour>\d+):(?<Minute>\d+):(?<Second>\d+)} System:"(?<SystemName>[^"]+)" StarPos:\((?<Pos>.*?)\)ly( +(?<TravelMode>\w+))?
                    string rgexpstr = "{(?<Hour>\\d+):(?<Minute>\\d+):(?<Second>\\d+)} System:\"(?<SystemName>[^\"]+)\" StarPos:\\((?<Pos>.*?)\\)ly( +(?<TravelMode>\\w+))?";
                    pattern = new Regex(rgexpstr);

                    Match match = pattern.Match(line);

                    hour = int.Parse(match.Groups["Hour"].Value);
                    min = int.Parse(match.Groups["Minute"].Value);
                    sec = int.Parse(match.Groups["Second"].Value);

                    //sp.Nr = int.Parse(match.Groups["Body"].Value);
                    sp.Name = match.Groups["SystemName"].Value;
                    string pos = match.Groups["Pos"].Value;
                    try
                    {
                        string[] xyzpos = pos.Split(',');
                        var culture = new System.Globalization.CultureInfo("en-US");
                        sp.x = float.Parse(xyzpos[0], culture);
                        sp.y = float.Parse(xyzpos[1], culture);
                        sp.z = float.Parse(xyzpos[2], culture);
                    }
                    catch
                    {
                        sp.x = 0;
                        sp.y = 0;
                        sp.z = 0;
                    }
                }
                else
                {
                    pattern = new Regex(@"{(?<Hour>\d+):(?<Minute>\d+):(?<Second>\d+)} System:\d+\((?<SystemName>.*?)\) Body:(?<Body>\d+) Pos:\(.*?\)( (?<TravelMode>\w+))?");
                    Match match = pattern.Match(line);

                    hour = int.Parse(match.Groups["Hour"].Value);
                    min = int.Parse(match.Groups["Minute"].Value);
                    sec = int.Parse(match.Groups["Second"].Value);

                    //sp.Nr = int.Parse(match.Groups["Body"].Value);
                    sp.Name = match.Groups["SystemName"].Value;
                    

                }
                if (hour >= lasttime.Hour)
                {
                    sp.time = new DateTime(lasttime.Year, lasttime.Month, lasttime.Day, hour, min, sec);
                }
                else
                {
                    DateTime tomorrow = lasttime.AddDays(1);
                    sp.time = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, hour, min, sec);
                }

                if (sp.time.Subtract(lasttime).TotalHours < -4)
                {
                    sp.time = sp.time.AddDays(1);
                }
                return sp;
            }
            catch
            {
                // MKW TODO: should we log bad lines?
                return null;
            }
        }
    }
}
