using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.Trilateration
{
    public class ReferencesSector
    {
        double WidthAngle;
        public double AzimuthStartRad;
        public double LatitudeStartRad;

        private List<ReferenceSystem> usedReferences;
        private List<ReferenceSystem> candidateReferences;
        private List<ReferenceSystem> optcandidateReferences;


        private double minWeight;



        public double Azimuth
        {
            get { return AzimuthStartRad * 180 / Math.PI; }
        }

        public double Latitude
        {
            get { return LatitudeStartRad * 180 / Math.PI; }
        }

        public double AzimuthCenter
        {
            get { return AzimuthStartRad * 180 / Math.PI + WidthAngle/2; }
        }

        public double LatitudeCenter
        {
            get { return LatitudeStartRad * 180 / Math.PI + WidthAngle/2; }
        }

        public double AzimuthCenterRad
        {
            get { return AzimuthCenter *Math.PI / 180; }
        }

        public double LatitudeCenterRad
        {
            get { return LatitudeCenter *  Math.PI / 180; }
        }



        public int ReferencesCount
        {
            get { return usedReferences.Count; }
        }
        public int CandidatesCount
        {
            get { return candidateReferences.Count - usedReferences.Count; }
        }


        public double Width
        {
            get
            {
                return WidthAngle * Math.PI / 180;
            }
        }

        public ReferencesSector(double azimuth, double altitude, int width)
        {
            AzimuthStartRad = azimuth * Math.PI / 180;
            LatitudeStartRad = altitude * Math.PI / 180;
            WidthAngle = width;

            usedReferences = new List<ReferenceSystem>();
            candidateReferences = new List<ReferenceSystem>();
            optcandidateReferences = new List<ReferenceSystem>();
            minWeight = double.MaxValue;
        }

        public ReferenceSystem GetBestCandidate()
        {
            var candidate = from p in optcandidateReferences  orderby p.Weight ascending select p;

            return candidate.First<ReferenceSystem>();
        }

        public void AddCandidate(ReferenceSystem refSys)
        {
            candidateReferences.Add(refSys);
            double weight = refSys.Weight;

            if (weight < minWeight)
            {
                minWeight = weight;
                optcandidateReferences.Add(refSys);
            }
            else if (optcandidateReferences.Count < 10)
                optcandidateReferences.Add(refSys);
            else if (optcandidateReferences.Count < 100 && refSys.Distance < 1000 && refSys.Distance > 100)
                optcandidateReferences.Add(refSys);
           
        }

        public void AddReference(ReferenceSystem refSys)
        {
            usedReferences.Add(refSys);
        }

        public string Name
        {
            get
            {
                return "Sector:" + (AzimuthStartRad * 180 / Math.PI).ToString("0") + ":" + (LatitudeStartRad * 180 / Math.PI).ToString("0");
            }
        }

    }
}
