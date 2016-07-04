using EDDiscovery;
using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EDDiscovery2.Trilateration
{
    public class SuggestedReferences
    {
        public SystemClass EstimatedPosition;
        private ReferencesSector[,] sectors;
        const int sections = 12;
        const int MaxPerSection = 1000;         // so we don't keep millions of stars in memory, limit the number of stars in each section

        public SuggestedReferences(double x, double y, double z)
        {
            EstimatedPosition = new SystemClass();

            EstimatedPosition.x = x;
            EstimatedPosition.y = y;
            EstimatedPosition.z = z;
            EstimatedPosition.name = "Estimated position";

            Stopwatch sw = new Stopwatch();
            sw.Start();


            CreateSectors();
            AddSystemsToSectors();
            sw.Stop();
            System.Diagnostics.Trace.WriteLine("FindCandidates time " + sw.Elapsed.TotalSeconds.ToString("0.000s"));

            for (int i = 0; i < sections; i++)
            {
                for (int j = 0; j < sections/2; j++)
                {
                    System.Diagnostics.Trace.WriteLine(i.ToString() + ":" + j.ToString() + "  " + sectors[i, j].Name + "  " + sectors[i, j].CandidatesCount.ToString());
                }
            }
        }


        private void CreateSectors()
        {
            sectors = new ReferencesSector[sections, sections];

            for (int az = -180, aznr =0; az<180; az += 360/sections, aznr++)
            {
                for (int alt = -90, altnr =0 ; alt < 90; alt += 360 / sections, altnr++)
                {
                    ReferencesSector sector = new ReferencesSector(az, alt, 360 / sections);
                    sectors[aznr, altnr] = sector;
                 }
            }
        }

        private void AddStarToSector(SystemClass sys)       // call back from AddSystemToSectors
        {
            if (sys.HasCoordinate)
            {
                ReferenceSystem refSys = new ReferenceSystem(sys, EstimatedPosition);

                if (refSys.Distance > 0.0) // Exlude own position
                {
                    int aznr = (int)Math.Floor((refSys.Azimuth * 180 / Math.PI + 180) / (360.0 / sections));
                    int altnr = (int)Math.Floor((refSys.Altitude * 180 / Math.PI) / (360.0 / sections));
                    if ( sectors[aznr % sections, altnr % (sections / 2)].CandidatesCount < MaxPerSection)
                        sectors[aznr % sections, altnr % (sections / 2)].AddCandidate(refSys);
                }
            }
        }

        private void AddSystemsToSectors()
        {
            SystemClass.ProcessStars(AddStarToSector);
        }

        public ReferenceSystem GetCandidate()
        {


            double maxdistance = 0;
            ReferencesSector sectorcandidate = null;

            // Get Sector with maximum distance for all others...
            for (int i = 0; i < sections; i++)
                for (int j = 0; j < sections / 2; j++)
                {
                    if (sectors[i, j].ReferencesCount == 0 && sectors[i, j].CandidatesCount > 0)  // An unused sector with candidates left?
                    {
                        double dist;
                        double mindist = 10;

                        for (int ii = 0; ii < sections; ii++)
                            for (int jj = 0; jj < sections / 2; jj++)
                            {
                                if (sectors[ii, jj].CandidatesCount > 0)
                                {
                                    dist = CalculateAngularDistance(sectors[i, j].AzimuthCenterRad, sectors[i, j].LatitudeCenterRad, sectors[ii, jj].AzimuthCenterRad, sectors[ii, jj].LatitudeCenterRad);

                                    if (dist > 0.001)
                                    {
                                        if (dist < mindist)
                                            mindist = dist;
                                    }
                                }
                            }


                        if (mindist > maxdistance)  // New candidate
                        {
                            maxdistance = mindist;
                            sectorcandidate = sectors[i, j];
                        }

                    }
                }

            if (sectorcandidate == null)
            {
                if (NrOfRefenreceSystems == 0)
                {
                    SystemClass sys = SystemClass.GetSystem("Sol");

                    if (EstimatedPosition.x == 0 && EstimatedPosition.y == 0 && EstimatedPosition.z == 0)
                        sys = SystemClass.GetSystem("Sirius");

                    if (sys == null)
                        return null;   // Should not happend

                    ReferenceSystem refSys = new ReferenceSystem(sys, EstimatedPosition);

                    return refSys;
                }

                return null;
            }

            return sectorcandidate.GetBestCandidate(); 
        }

        public void AddReferenceStar(SystemClass sys)
        {
            ReferenceSystem refSys = new ReferenceSystem(sys, EstimatedPosition);

            if (NrOfRefenreceSystems== 0 || refSys.Distance > 0.0) // Exlude own position
            {
                int aznr = (int)Math.Floor((refSys.Azimuth * 180 / Math.PI + 180) / (360.0 / sections));
                int altnr = (int)Math.Floor((refSys.Altitude * 180 / Math.PI) / (360.0 / sections));

                sectors[aznr % sections, altnr % (sections / 2)].AddReference(refSys);
            }

        }

        public int NrOfRefenreceSystems
        {
            get
            {
                int nr = 0;
                for (int i = 0; i < sections; i++)
                    for (int j = 0; j < sections / 2; j++)
                        nr += sectors[i, j].ReferencesCount;

                return nr;
            }
        }



        /// <summary>
        /// Calculate angular distance between 2 sperical points
        /// </summary>
        /// <param name="longitude1Rad">Point 1 longitude in radians</param>
        /// <param name="latitude1Rad">Point 1 latitude in radians</param>
        /// <param name="longitude2Rad">Point 2 longitude in radians</param>
        /// <param name="latitude2Rad">Point 2 latitude in radians</param>
        /// <returns></returns>
        public double CalculateAngularDistance(double longitude1Rad, double latitude1Rad, double longitude2Rad, double latitude2Rad)
        {
            double logitudeDiff = Math.Abs(longitude1Rad - longitude2Rad);

            if (logitudeDiff > Math.PI)
                logitudeDiff = 2.0 * Math.PI - logitudeDiff;

            double angleCalculation =  Math.Acos( Math.Sin(latitude1Rad) * Math.Sin(latitude2Rad) +  Math.Cos(latitude1Rad) * Math.Cos(latitude2Rad) * Math.Cos(logitudeDiff));
            return angleCalculation;
        }
    }
}
