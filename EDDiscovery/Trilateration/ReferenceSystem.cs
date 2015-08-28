using EDDiscovery;
using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                if (refSys.name.EndsWith("0"))  // Elite has a bug with selecting systems ending with 0.  Prefere others first.
                    modifier += 50;

                return refSys.name.Length*2 + Math.Sqrt(Distance) / 3.5 + modifier;
                //return refSys.name.Length + Distance/100 + modifier;
            }
        }

        public ReferenceSystem(SystemClass refsys, SystemClass EstimatedPosition)
        {
            this.refSys = refsys;
            Azimuth = Math.Atan2(refSys.y - EstimatedPosition.y, refSys.x - EstimatedPosition.x);
            Distance = SystemData.Distance(refSys, EstimatedPosition);
            Altitude= Math.Acos((refSys.z-EstimatedPosition.z)/Distance);
        }




    }
}
