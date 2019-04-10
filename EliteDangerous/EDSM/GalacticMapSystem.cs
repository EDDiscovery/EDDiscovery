using EliteDangerousCore;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore.EDSM
{
    public class GalacticMapSystem : SystemClass
    {
        public GalacticMapObject GalMapObject { get; set; }

        public GalacticMapSystem(ISystem sys, GalacticMapObject gmo) : base(sys)
        {
            this.GalMapObject = gmo;
        }

        public GalacticMapSystem(GalacticMapObject gmo) : base()
        {
            this.Name = gmo.galMapSearch;
            this.X = gmo.points[0].X;
            this.Y = gmo.points[0].Y;
            this.Z = gmo.points[0].Z;
            this.GalMapObject = gmo;
        }
    }
}
