/*
 * Copyright © 2015 - 2019 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using System;
using System.Linq;
using System.Data.Common;
using EMK.LightGeometry;
using System.Collections.Generic;

namespace EliteDangerousCore.DB
{
    public partial class SystemsDB
    {
        ///////////////////////////////////////// List of systems near xyz between mindist and maxdist

        internal static void GetSystemListBySqDistancesFrom(BaseUtils.SortedListDoubleDuplicate<ISystem> distlist, // MUST use duplicate double list to protect against EDSM having two at the same point
                                                            double x, double y, double z,
                                                            int maxitems,
                                                            double mindist,         // 0 = no min dist, always spherical
                                                            double maxdist,
                                                            bool spherical,     // enforces sphere on maxdist, else its a cube for maxdist
                                                            SQLiteConnectionSystem cn,
                                                            Action<ISystem> LookedUp = null
                                                        )

        {
            // for comparision, using the grid screener is slower than the xy index. keep code for record
            // grid screener..  "s.sectorid IN (Select id FROM Sectors sx where sx.gridid IN (" + strinlist + ")) " +
            //var gridids = GridId.Ids(x - maxdist, x + maxdist, z - maxdist, z + maxdist);       // find applicable grid ids across this range..
            //var strinlist = string.Join(",", (from x1 in gridids select x1.ToStringInvariant()));     // here we convert using invariant for paranoia sake.

           // System.Diagnostics.Debug.WriteLine("Time1 " + BaseUtils.AppTicks.TickCountLap("SDC"));

            int mindistint = mindist > 0 ? SystemClass.DoubleToInt(mindist) * SystemClass.DoubleToInt(mindist) : 0;

            // needs a xz index for speed

            using (DbCommand cmd = cn.CreateSelect("Systems s",
                MakeSystemQueryEDDB,
                where:
                    "s.x >= @xv - @maxdist " +
                    "AND s.x <= @xv + @maxdist " +
                    "AND s.z >= @zv - @maxdist " +
                    "AND s.z <= @zv + @maxdist " +
                    "AND s.y >= @yv - @maxdist " +
                    "AND s.y <= @yv + @maxdist " +
                    (mindist > 0 ? ("AND (s.x-@xv)*(s.x-@xv)+(s.y-@yv)*(s.y-@yv)+(s.z-@zv)*(s.z-@zv)>=" + (mindistint).ToStringInvariant()) : ""),
                orderby: "(s.x-@xv)*(s.x-@xv)+(s.y-@yv)*(s.y-@yv)+(s.z-@zv)*(s.z-@zv)",         // just use squares to order
                joinlist: MakeSystemQueryEDDBJoinList,
                limit: "@max"
                ))
            {
                cmd.AddParameterWithValue("@xv", SystemClass.DoubleToInt(x));
                cmd.AddParameterWithValue("@yv", SystemClass.DoubleToInt(y));
                cmd.AddParameterWithValue("@zv", SystemClass.DoubleToInt(z));
                cmd.AddParameterWithValue("@max", maxitems + 1);     // 1 more, because if we are on a System, that will be returned
                cmd.AddParameterWithValue("@maxdist", SystemClass.DoubleToInt(maxdist));

               // System.Diagnostics.Debug.WriteLine(cn.ExplainQueryPlanString(cmd));

                int xi = SystemClass.DoubleToInt(x);
                int yi = SystemClass.DoubleToInt(y);
                int zi = SystemClass.DoubleToInt(z);
                long maxdistsqi = (long)SystemClass.DoubleToInt(maxdist) * (long)SystemClass.DoubleToInt(maxdist);

                long count = 0;
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                  //  System.Diagnostics.Debug.WriteLine("Time1.5 " + BaseUtils.AppTicks.TickCountLap("SDC"));

                    while (reader.Read())      // already sorted, and already limited to max items
                    {
                        int sxi = reader.GetInt32(0);
                        int syi = reader.GetInt32(1);
                        int szi = reader.GetInt32(2);

                        long distsqi = (long)(xi - sxi) * (long)(xi - sxi) + (long)(yi - syi) * (long)(yi - syi) + (long)(zi - szi) * (long)(zi - szi);

                        if (!spherical || distsqi <= maxdistsqi)
                        {
                            SystemClass s = MakeSystem(reader);
                            double distnorm = ((double)distsqi) / SystemClass.XYZScalar / SystemClass.XYZScalar;
                            //System.Diagnostics.Debug.WriteLine("System " + s.Name + " " + Math.Sqrt(distnorm).ToString("0.0"));
                            LookedUp?.Invoke(s);                            // callback to say looked up
                            distlist.Add(distnorm, s);                  // which Rob has seen crashing the program! Bad EDSM!
                        }

                        count++;
                    }

                  //  System.Diagnostics.Debug.WriteLine("Time2 " + BaseUtils.AppTicks.TickCountLap("SDC") + "  count " + count);
                }
            }
        }

        internal static ISystem GetSystemByPosition(double x, double y, double z, SQLiteConnectionSystem cn, double maxdist = 0.125)
        {
            BaseUtils.SortedListDoubleDuplicate<ISystem> distlist = new BaseUtils.SortedListDoubleDuplicate<ISystem>();
            GetSystemListBySqDistancesFrom(distlist, x, y, z, 1, 0, maxdist, true, cn); // return 1 item, min dist 0, maxdist
            return (distlist.Count > 0) ? distlist.First().Value : null;
        }

        /////////////////////////////////////////////// Nearest to a point determined by a metric

        public enum SystemsNearestMetric
        {
            IterativeNearestWaypoint,
            IterativeMinDevFromPath,
            IterativeMaximumDev100Ly,
            IterativeMaximumDev250Ly,
            IterativeMaximumDev500Ly,
            IterativeWaypointDevHalf,
        }

        internal static ISystem GetSystemNearestTo(Point3D currentpos,
                                                  Point3D wantedpos,
                                                  double maxfromcurpos,
                                                  double maxfromwanted,
                                                  SystemsNearestMetric routemethod,
                                                  SQLiteConnectionSystem cn,
                                                  Action<ISystem> LookedUp = null,
                                                  int limitto = 1000)
        {
            using (DbCommand cmd = cn.CreateSelect("Systems s",
                        MakeSystemQueryEDDB,
                        where:
                                "x >= @xc - @maxfromcurpos " +
                                "AND x <= @xc + @maxfromcurpos " +
                                "AND z >= @zc - @maxfromcurpos " +
                                "AND z <= @zc + @maxfromcurpos " +
                                "AND x >= @xw - @maxfromwanted " +
                                "AND x <= @xw + @maxfromwanted " +
                                "AND z >= @zw - @maxfromwanted " +
                                "AND z <= @zw + @maxfromwanted " +
                                "AND y >= @yc - @maxfromcurpos " +
                                "AND y <= @yc + @maxfromcurpos " +
                                "AND y >= @yw - @maxfromwanted " +
                                "AND y <= @yw + @maxfromwanted ",
                        orderby: "(s.x-@xw)*(s.x-@xw)+(s.y-@yw)*(s.y-@yw)+(s.z-@zw)*(s.z-@zw)",         // orderby distance from wanted
                        limit: limitto,
                        joinlist: MakeSystemQueryEDDBJoinList))
            {
                cmd.AddParameterWithValue("@xw", SystemClass.DoubleToInt(wantedpos.X));         // easier to manage with named paras
                cmd.AddParameterWithValue("@yw", SystemClass.DoubleToInt(wantedpos.Y));
                cmd.AddParameterWithValue("@zw", SystemClass.DoubleToInt(wantedpos.Z));
                cmd.AddParameterWithValue("@maxfromwanted", SystemClass.DoubleToInt(maxfromwanted));
                cmd.AddParameterWithValue("@xc", SystemClass.DoubleToInt(currentpos.X));
                cmd.AddParameterWithValue("@yc", SystemClass.DoubleToInt(currentpos.Y));
                cmd.AddParameterWithValue("@zc", SystemClass.DoubleToInt(currentpos.Z));
                cmd.AddParameterWithValue("@maxfromcurpos", SystemClass.DoubleToInt(maxfromcurpos));

                //System.Diagnostics.Debug.WriteLine(cn.ExplainQueryPlanString(cmd));

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    var systems = MakeSystemEnumerable(reader, callback: LookedUp);

                    return GetSystemNearestTo(systems, currentpos, wantedpos, maxfromcurpos, maxfromwanted, routemethod);
                }
            }
        }

        internal static ISystem GetSystemNearestTo(IEnumerable<ISystem> systems,            // non database helper function
                                                   Point3D currentpos,
                                                   Point3D wantedpos,
                                                   double maxfromcurpos,
                                                   double maxfromwanted,
                                                   SystemsNearestMetric routemethod)
        {
            double bestmindistance = double.MaxValue;
            ISystem nearestsystem = null;

            foreach (var s in systems)
            {
                Point3D syspos = new Point3D(s.X, s.Y, s.Z);
                double distancefromwantedx2 = Point3D.DistanceBetweenX2(wantedpos, syspos); // range between the wanted point and this, ^2
                double distancefromcurposx2 = Point3D.DistanceBetweenX2(currentpos, syspos);    // range between the wanted point and this, ^2

                // ENSURE its withing the circles now
                if (distancefromcurposx2 <= (maxfromcurpos * maxfromcurpos) && distancefromwantedx2 <= (maxfromwanted * maxfromwanted))
                {
                    if (routemethod == SystemsNearestMetric.IterativeNearestWaypoint)
                    {
                        if (distancefromwantedx2 < bestmindistance)
                        {
                            nearestsystem = s;
                            bestmindistance = distancefromwantedx2;
                        }
                    }
                    else
                    {
                        Point3D interceptpoint = currentpos.InterceptPoint(wantedpos, syspos);      // work out where the perp. intercept point is..
                        double deviation = Point3D.DistanceBetween(interceptpoint, syspos);
                        double metric = 1E39;

                        if (routemethod == SystemsNearestMetric.IterativeMinDevFromPath)
                            metric = deviation;
                        else if (routemethod == SystemsNearestMetric.IterativeMaximumDev100Ly)
                            metric = (deviation <= 100) ? distancefromwantedx2 : metric;        // no need to sqrt it..
                        else if (routemethod == SystemsNearestMetric.IterativeMaximumDev250Ly)
                            metric = (deviation <= 250) ? distancefromwantedx2 : metric;
                        else if (routemethod == SystemsNearestMetric.IterativeMaximumDev500Ly)
                            metric = (deviation <= 500) ? distancefromwantedx2 : metric;
                        else if (routemethod == SystemsNearestMetric.IterativeWaypointDevHalf)
                            metric = Math.Sqrt(distancefromwantedx2) + deviation / 2;
                        else
                            throw new ArgumentOutOfRangeException(nameof(routemethod));

                        if (metric < bestmindistance)
                        {
                            nearestsystem = s;
                            bestmindistance = metric;
                        }
                    }
                }
            }

            return nearestsystem;
        }
    }
}


