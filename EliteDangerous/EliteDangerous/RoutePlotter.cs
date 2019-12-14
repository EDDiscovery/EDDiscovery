/*
 * Copyright © 2019 EDDiscovery development team
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

using EliteDangerousCore.DB;
using EMK.LightGeometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EliteDangerousCore
{
    public class RoutePlotter
    {
        public bool StopPlotter { get; set; } = false;
        public float MaxRange;
        public Point3D Coordsfrom;
        public Point3D Coordsto;
        public string FromSystem;
        public string ToSystem;
        public SystemsDB.SystemsNearestMetric RouteMethod;
        public bool UseFsdBoost;

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
            double traveldistance = Point3D.DistanceBetween(Coordsfrom, Coordsto);      // its based on a percentage of the traveldistance
            List<ISystem> routeSystems = new List<ISystem>();
            System.Diagnostics.Debug.WriteLine("From " + FromSystem + " to  " + ToSystem + ", using metric " + RouteMethod.ToString());

            routeSystems.Add(new SystemClass(FromSystem, Coordsfrom.X, Coordsfrom.Y, Coordsfrom.Z));

            info(new ReturnInfo(FromSystem, double.NaN, Coordsfrom,double.NaN,double.NaN,routeSystems[0]));

            Point3D curpos = Coordsfrom;
            int jump = 1;
            double actualdistance = 0;

            float maxfromwanted = (MaxRange<100) ? (MaxRange-1) : (100+MaxRange * 1 / 5);       // if <100, then just make sure we jump off by 1 yr, else its a 100+1/5
            maxfromwanted = Math.Min(maxfromwanted, MaxRange - 1);

            do
            {
                double distancetogo = Point3D.DistanceBetween(Coordsto, curpos);      // to go

                if (distancetogo <= MaxRange)                                         // within distance, we can go directly
                    break;

                Point3D travelvector = new Point3D(Coordsto.X - curpos.X, Coordsto.Y - curpos.Y, Coordsto.Z - curpos.Z); // vector to destination
                Point3D travelvectorperly = new Point3D(travelvector.X / distancetogo, travelvector.Y / distancetogo, travelvector.Z / distancetogo); // per ly travel vector

                Point3D expectedNextPosition = GetNextPosition(curpos, travelvectorperly, MaxRange);    // where we would like to be..
                ISystem bestsystem = GetBestJumpSystem(curpos, travelvectorperly, maxfromwanted, MaxRange);    // see if we can find a system near  our target

                // if we haven't found a system in range, let's try boosting
                int boostStrength = 0;
                while (UseFsdBoost && bestsystem == null && boostStrength < 4)
                {
                    boostStrength = 1 << boostStrength;
                    float maxRangeWithBoost = MaxRange * (1.0f + BoostPercentage(boostStrength));
                    ISystem bestSystemWithBoost = GetBestJumpSystem(curpos, travelvectorperly, maxfromwanted, maxRangeWithBoost);

                    if (bestSystemWithBoost != null)
                        bestsystem = bestSystemWithBoost;
                }

                Point3D nextpos = expectedNextPosition;    // where we really are going to be
                string sysname = "WAYPOINT";
                double deltafromwaypoint = 0;
                double deviation = 0;

                if (bestsystem != null)
                {
                    nextpos = new Point3D(bestsystem.X, bestsystem.Y, bestsystem.Z);
                    deltafromwaypoint = Point3D.DistanceBetween(nextpos, expectedNextPosition);     // how much in error
                    deviation = Point3D.DistanceBetween(curpos.InterceptPoint(expectedNextPosition, nextpos), nextpos);
                    sysname = bestsystem.Name;
                    if (boostStrength > 0)
                        sysname += " (+" + BoostPercentage(boostStrength) * 100 + "% Boost)";
                    routeSystems.Add(bestsystem);
                }

                info(new ReturnInfo(sysname, Point3D.DistanceBetween(curpos, nextpos), nextpos, deltafromwaypoint, deviation , bestsystem));

                actualdistance += Point3D.DistanceBetween(curpos, nextpos);
                curpos = nextpos;
                jump++;

            } while ( !StopPlotter);

            routeSystems.Add(new SystemClass(ToSystem, Coordsto.X, Coordsto.Y, Coordsto.Z));

            actualdistance += Point3D.DistanceBetween(curpos, Coordsto);

            info(new ReturnInfo(ToSystem, Point3D.DistanceBetween(curpos, Coordsto), Coordsto, double.NaN, double.NaN, routeSystems.Last()));

            info(new ReturnInfo("Straight Line Distance", traveldistance));
            info(new ReturnInfo("Travelled Distance", actualdistance));

            return routeSystems;
        }

        private ISystem GetBestJumpSystem(Point3D currentPosition, Point3D travelVectorPerLy, float maxDistanceFromWanted, float maxRange)
        {
            Point3D nextPosition = GetNextPosition(currentPosition, travelVectorPerLy, maxRange);
            ISystem bestSystem = DB.SystemCache.GetSystemNearestTo(currentPosition, nextPosition, maxRange, maxDistanceFromWanted, RouteMethod, 1000);  // at least get 1/4 way there, otherwise waypoint.  Best 1000 from waypoint checked
            return bestSystem;
        }

        private static Point3D GetNextPosition(Point3D currentPosition, Point3D travelVectorPerLy, float maxRange)
        {
            return new Point3D(currentPosition.X + maxRange * travelVectorPerLy.X,
                currentPosition.Y + maxRange * travelVectorPerLy.Y,
                currentPosition.Z + maxRange * travelVectorPerLy.Z); // where we would like to be..
        }

        private static float BoostPercentage(int boostStrength)
        {
            return boostStrength / 4.0f;
        }
    }

}
