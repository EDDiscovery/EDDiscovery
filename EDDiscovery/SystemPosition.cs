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
                if (line.Length < 15)
                    return null;

                if (line.StartsWith("<data>"))
                    return null;

                string str = line.Substring(1, 2);

                int hour = int.Parse(str);
                int min = int.Parse(line.Substring(4, 2));
                int sec = int.Parse(line.Substring(7, 2));

                if (hour >= lasttime.Hour)
                { sp.time = new DateTime(lasttime.Year, lasttime.Month, lasttime.Day, hour, min, sec); }
                else
                {
                    DateTime tomorrow = lasttime.AddDays(1);
                    sp.time = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, hour, min, sec);
                }

                if (sp.time.Subtract(lasttime).TotalHours < -4)
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
