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
    public class xSystemPosition
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
        

     

    }
}
