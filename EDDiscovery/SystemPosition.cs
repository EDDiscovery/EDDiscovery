using EDDiscovery.DB;
using EDDiscovery2.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public class SystemPosition
    {
        public DateTime time;
        public string Name;
        public int Nr;
        public int BodyNr;

        public SystemClass curSystem;
        public SystemClass prevSystem;
        public string strDistance;
        public VisitedSystemsClass vs;

        public SystemPosition()
        {
        }

        public SystemPosition(VisitedSystemsClass vs)
        {
            Name = vs.Name;
            time = vs.Time;
            this.vs = vs;
        }

        static public SystemPosition Parse(DateTime lasttime, string line)
        {
            SystemPosition sp = new SystemPosition();

            try
            {
                if (line.Length < 15)
                    return null;

                if (line.StartsWith("<data>"))
                    return null;

                string str = line.Substring(1, 2);

                int hour = int.Parse(str);
                int min = int.Parse(line.Substring(4, 2));
                int sec = int.Parse(line.Substring(7, 2));

                sp.time = new DateTime(lasttime.Year, lasttime.Month, lasttime.Day, hour, min, sec);

                if (sp.time.Subtract(lasttime).TotalHours < -12)
                    sp.time = sp.time.AddDays(1);

                str = line.Substring(18, line.IndexOf(" Body:")-19);

                sp.Nr = int.Parse(str.Substring(0, str.IndexOf("(")));
                sp.Name = str.Substring(str.IndexOf("(")+1);
                return sp;
            }
            catch
            {
                return null;
            }
        }
    }
}
