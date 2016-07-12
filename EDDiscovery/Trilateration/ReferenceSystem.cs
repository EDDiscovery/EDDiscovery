using EDDiscovery;
using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EDDiscovery2.Trilateration
{
    public class ReferenceSystem
    {
        public double Distance;
        public double Azimuth, Altitude;
        private SystemClass refSys;

        public SystemClass System
        {
            get { return refSys; }
        }


        public double Weight
        {
            get
            {
                int modifier = 0;
                //search bug seems to be fixed now, don't avoid these any more.
                //if (refSys.name.EndsWith("0"))  // Elite has a bug with selecting systems ending with 0.  Prefere others first.
                //    modifier += 50;

                if (Regex.IsMatch(refSys.name, "\\s[A-Z][A-Z].[A-Z]\\s"))
                    modifier += 20;


                if (Distance > 20000)
                    modifier += 10;

                if (Distance > 30000)
                    modifier += 20;


                return refSys.name.Length*2 + Math.Sqrt(Distance) / 3.5 + modifier;
                //return refSys.name.Length + Distance/100 + modifier;
            }
        }

        public ReferenceSystem(SystemClass refsys, SystemClass EstimatedPosition)
        {
            this.refSys = refsys;
            Azimuth = Math.Atan2(refSys.y - EstimatedPosition.y, refSys.x - EstimatedPosition.x);
            Distance = SystemClass.Distance(refSys, EstimatedPosition);
            Altitude= Math.Acos((refSys.z-EstimatedPosition.z)/Distance);
        }




    }
}
