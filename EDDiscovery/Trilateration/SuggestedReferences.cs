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
        int sections = 12;

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
            FindCandidates();
            sw.Stop();
            System.Diagnostics.Trace.WriteLine("FindCandidates time " + sw.Elapsed.TotalSeconds.ToString("0.000s"));

            for (int i = 0; i < sections; i++)
            {
                for (int j = 0; j < sections/2; j++)
                {
                    System.Diagnostics.Trace.WriteLine(i.ToString() + ":" + j.ToString()+ "  " + sectors[i, j].Name + "  " + sectors[i, j].candidateReferences.Count.ToString());
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


        private void FindCandidates()
        {
            int aznr, altnr;


            foreach (SystemClass sys in SQLiteDBClass.globalSystems)
            {
                if (sys.HasCoordinate)
                {
                    ReferenceSystem refSys = new ReferenceSystem(sys, EstimatedPosition);

                    if (refSys.Distance > 0.0) // Exlude own position
                    {
                        aznr = (int)Math.Floor((refSys.Azimuth * 180 / Math.PI + 180) / (360.0 / sections));
                        altnr = (int)Math.Floor((refSys.Altitude * 180 / Math.PI) / (360.0 / sections));

                        sectors[aznr%sections, altnr%(sections/2)].AddCandidate(refSys);
                    }
                }
            }

        }
  

    }
}
