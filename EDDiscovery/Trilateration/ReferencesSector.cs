using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.Trilateration
{
    public class ReferencesSector
    {
        double WidthAngle;
        double AzimuthStart;
        double AltitudeStart;

        public string Name
        {
            get
            {
                return "Sector:" + (AzimuthStart * 180 / Math.PI).ToString("0") + ":" + (AltitudeStart * 180 / Math.PI).ToString("0");
            }
        }

    }
}
