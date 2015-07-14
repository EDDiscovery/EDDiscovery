using EDDiscovery;
using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.Trilateration
{
    public class ReferenceSystem : SystemClass
    {
        public double Distance;
        public double Azimuth, Altitude;
        private SystemClass refSys;

        

        public ReferenceSystem(SystemClass refSys, SystemClass EstimatedPosition)
        {
            Azimuth = Math.Atan2(refSys.y - EstimatedPosition.y, refSys.x - EstimatedPosition.x);
            Distance = SystemData.Distance(refSys, EstimatedPosition);
            Altitude= Math.Acos(refSys.z-EstimatedPosition.z/Distance);
        }

    }
}
