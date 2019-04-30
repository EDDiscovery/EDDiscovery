using EMK.LightGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
{
    public class RoutePlotter
    {
        public float maxrange;
        public bool usingcoordsfrom;
        public Point3D coordsfrom;
        public bool usingcoordsto;
        public Point3D coordsto;
        public string fromsys;
        public string tosys;
        public int routemethod;
        public int possiblejumps;

        // METRICs defined by systemclass GetSystemNearestTo function
        public static string[] metric_options = {
            "Nearest to Waypoint",
            "Minimum Deviation from Path",
            "Nearest to Waypoint with dev<=100ly",
            "Nearest to Waypoint with dev<=250ly",
            "Nearest to Waypoint with dev<=500ly",
            "Nearest to Waypoint + Deviation / 2"
        };

        public class ReturnInfo
        {
            public string name;             // always
            public double dist;             // always
            public Point3D pos;             // 3dpos can be null
            public double waypointdist;     // can be Nan
            public double deviation;        // can be Nan
            public ISystem system;          // only if its a real system

            public ReturnInfo(string s, double d, Point3D p = null, double way = double.NaN, double dev = double.NaN, ISystem sys = null)
            {
                name = s;dist = d;pos = p;waypointdist = way;deviation = dev; system = sys;
            }
        }

        public List<ISystem> RouteIterative(Action<ReturnInfo> info)
        {
            double traveldistance = Point3D.DistanceBetween(coordsfrom, coordsto);      // its based on a percentage of the traveldistance
            List<ISystem> routeSystems = new List<ISystem>();
            System.Diagnostics.Debug.WriteLine("From " + fromsys + " to  " + tosys);

            routeSystems.Add(new SystemClass(fromsys, coordsfrom.X, coordsfrom.Y, coordsfrom.Z));

            info(new ReturnInfo(fromsys, double.NaN, coordsfrom,double.NaN,double.NaN,routeSystems[0]));

            Point3D curpos = coordsfrom;
            int jump = 1;
            double actualdistance = 0;

            do
            {
                double distancetogo = Point3D.DistanceBetween(coordsto, curpos);      // to go

                if (distancetogo <= maxrange)                                         // within distance, we can go directly
                    break;

                Point3D travelvector = new Point3D(coordsto.X - curpos.X, coordsto.Y - curpos.Y, coordsto.Z - curpos.Z); // vector to destination
                Point3D travelvectorperly = new Point3D(travelvector.X / distancetogo, travelvector.Y / distancetogo, travelvector.Z / distancetogo); // per ly travel vector

                Point3D nextpos = new Point3D(curpos.X + maxrange * travelvectorperly.X,
                                              curpos.Y + maxrange * travelvectorperly.Y,
                                              curpos.Z + maxrange * travelvectorperly.Z);   // where we would like to be..

                ISystem bestsystem = DB.SystemCache.GetSystemNearestTo(curpos, nextpos, maxrange, Math.Min(maxrange*1/2,250), routemethod, 1000);     // at least get 1/4 way there, otherwise waypoint.  Best 1000 from waypoint checked

                string sysname = "WAYPOINT";
                double deltafromwaypoint = 0;
                double deviation = 0;

                if (bestsystem != null)
                {
                    Point3D bestposition = new Point3D(bestsystem.X, bestsystem.Y, bestsystem.Z);
                    deltafromwaypoint = Point3D.DistanceBetween(bestposition, nextpos);     // how much in error
                    deviation = Point3D.DistanceBetween(curpos.InterceptPoint(nextpos, bestposition), bestposition);
                    nextpos = bestposition;
                    sysname = bestsystem.Name;
                    routeSystems.Add(bestsystem);
                }

                info(new ReturnInfo(sysname, Point3D.DistanceBetween(curpos, nextpos), nextpos, deltafromwaypoint, deviation , bestsystem));

                actualdistance += Point3D.DistanceBetween(curpos, nextpos);
                curpos = nextpos;
                jump++;

            } while (true);

            routeSystems.Add(new SystemClass(tosys, coordsto.X, coordsto.Y, coordsto.Z));

            actualdistance += Point3D.DistanceBetween(curpos, coordsto);

            info(new ReturnInfo(tosys, Point3D.DistanceBetween(curpos, coordsto), coordsto, double.NaN, double.NaN, routeSystems.Last()));

            info(new ReturnInfo("Straight Line Distance", traveldistance));
            info(new ReturnInfo("Travelled Distance", actualdistance));

            return routeSystems;
        }
    }

}
