using EDDiscovery;
using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.Trilateration
{
    public class SuggestedReferences
    {
        public SystemClass EstimatedPosition;
        private Dictionary<string, ReferencesSector> sectors;

        public SuggestedReferences(double x, double y, double z)
        {
            EstimatedPosition = new SystemClass();

            EstimatedPosition.x = x;
            EstimatedPosition.y = y;
            EstimatedPosition.z = z;
            EstimatedPosition.name = "Estimated position";
            
            

            CreateSectors();

        }


        private void CreateSectors()
        {
            int nr = 6;
            sectors = new Dictionary<string, ReferencesSector>();


        }
  

    }
}
