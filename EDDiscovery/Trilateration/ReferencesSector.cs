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

        public List<ReferenceSystem> usedReferences;
        public List<ReferenceSystem> candidateReferences;


        public double Width
        {
            get
            {
                return WidthAngle * Math.PI / 180;
            }
        }

        public ReferencesSector(double azimuth, double altitude, int width)
        {
            AzimuthStart = azimuth * Math.PI / 180;
            AltitudeStart = altitude * Math.PI / 180;
            WidthAngle = width;

            usedReferences = new List<ReferenceSystem>();
            candidateReferences = new List<ReferenceSystem>();

        }


        public void AddCandidate(ReferenceSystem refSys)
        {
            candidateReferences.Add(refSys);
        }

        public string Name
        {
            get
            {
                return "Sector:" + (AzimuthStart * 180 / Math.PI).ToString("0") + ":" + (AltitudeStart * 180 / Math.PI).ToString("0");
            }
        }

    }
}
