/*
 * Copyright © 2016 EDDiscovery development team
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
using EMK.LightGeometry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EliteDangerousCore.DB
{
    [DebuggerDisplay("{DebugDisplay,nq}")]
    public class SavedRouteClass : IEquatable<SavedRouteClass>
    {
        public SavedRouteClass()
        {
            this.Id = -1;
            this.Systems = new List<string>();
        }

        public SavedRouteClass(string name, params string[] systems)
        {
            this.Id = -1;
            this.Name = name;
            this.Systems = systems.ToList();
        }

        public SavedRouteClass(DbDataReader dr, List<string> syslist)
        {
            this.Id = (long)dr["id"];
            this.Name = (string)dr["name"];
            if (dr["start"] != DBNull.Value)
                this.StartDate = ((DateTime)dr["start"]);
            if (dr["end"] != DBNull.Value)
                this.EndDate = ((DateTime)dr["end"]);
               
            this.Systems = syslist;
            int statusbits = (int)dr["Status"];
            this.Deleted = (statusbits & 1) != 0;
            this.EDSM = (statusbits & 2) != 0;

            if ( this.Name.StartsWith("\x7F"))  // marker 7F meant pre 9.1 a EDSM system which was deleted.
            {
                this.Name = this.Name.Substring(1);
                Deleted = true;
            }
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }            // these are LOCAL TIMES since the expedition editor is using DateTime.Now
        public DateTime? EndDate { get; set; }
        public List<string> Systems { get; private set; }
        public bool EDSM { get; set; }          // supplied by EDSM
        public bool Deleted { get; set; }       // Deleted by us
        public string LastSystem { get { return Systems.Count > 0 ? Systems[Systems.Count - 1] : null; } }

        public bool Equals(SavedRouteClass other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.Name == other.Name || (String.IsNullOrEmpty(this.Name) && String.IsNullOrEmpty(other.Name))) &&
                   this.StartDate == other.StartDate &&
                   this.EndDate == other.EndDate &&
                   this.Systems.SequenceEqual(other.Systems);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is SavedRouteClass)
            {
                return this.Equals((SavedRouteClass)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (Name == null) ? 0 : Name.GetHashCode();
        }

        public bool Add()
        {
            Id = UserDatabase.Instance.Add<long>("routes_expeditions", "id", new Dictionary<string, object>
            {
                ["name"] = Name,
                ["start"] = StartDate,
                ["end"] = EndDate,
                ["Status"] = (Deleted ? 1 : 0) + (EDSM ? 2 : 0)
            });

            var sysrows = Systems.Select(s => new Dictionary<string, object>
            {
                ["routeid"] = Id,
                ["systemname"] = s
            }).ToList();

            UserDatabase.Instance.Add("route_systems", sysrows);

            return true;
        }

        public bool Update()
        {
            UserDatabase.Instance.Update("routes_expeditions", "id", Id, new Dictionary<string, object>
            {
                ["name"] = Name,
                ["start"] = StartDate,
                ["end"] = EndDate,
                ["Status"] = (Deleted ? 1 : 0) + (EDSM ? 2 : 0)
            });

            UserDatabase.Instance.Delete("route_systems", "routeid", Id);

            var sysrows = Systems.Select(s => new Dictionary<string, object>
            {
                ["routeid"] = Id,
                ["systemname"] = s
            }).ToList();

            UserDatabase.Instance.Add("route_systems", sysrows);

            return true;
        }

        public bool Delete()
        {
            UserDatabase.Instance.Delete("route_systems", "routeid", Id);
            UserDatabase.Instance.Delete("routes_expeditions", "id", Id);
            return true;
        }

        public static List<SavedRouteClass> GetAllSavedRoutes()
        {
            List<SavedRouteClass> retVal = new List<SavedRouteClass>();

            try
            {
                Dictionary<int, List<string>> routesystems = new Dictionary<int, List<string>>();

                foreach (var row in UserDatabase.Instance.Retrieve("route_systems", "routeid", "systemname"))
                {
                    int routeid = (int)(long)row[0];
                    string sysname = (string)row[1];
                    if (!routesystems.ContainsKey(routeid))
                    {
                        routesystems[routeid] = new List<string>();
                    }
                    routesystems[routeid].Add(sysname);
                }

                retVal = UserDatabase.Instance.Retrieve("routes_expeditions", rdr =>
                {
                    int routeid = (int)(long)rdr[0];
                    List<string> syslist = routesystems.ContainsKey(routeid) ? routesystems[routeid] : new List<string>();
                    SavedRouteClass sys = new SavedRouteClass(rdr, syslist);
                    return sys;
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.ToString());
            }

            return retVal;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual string DebugDisplay
        {
            get
            {
                return $"\"{(Name ?? "(null)")}\" [{Id:n0}]: " + ((Systems == null || Systems.Count == 0) ? "no systems" : ((Systems?.Count == 1) ? "1 system" : (Systems.Count.ToString("n0") + " systems")));
            }
        }

        public List<Tuple<ISystem,int>> KnownSystemList()          // list of known system only.  ID holds original index of entry from Systems
        {
            List<Tuple<ISystem, int>> list = new List<Tuple<ISystem, int>>();
            for (int i = 0; i < Systems.Count; i++)
            {
                ISystem s = SystemCache.FindSystem(Systems[i]);
                if (s != null)
                {
                    list.Add(new Tuple<ISystem,int>(s,i));
                }
            }

            return list;
        }


        public double CumulativeDistance(ISystem start = null, List<Tuple<ISystem, int>> knownsystems = null)   // optional first system to measure from
        {
            if ( knownsystems == null )
                knownsystems = KnownSystemList();

            double distance = 0;
            int i = 0;

            if ( start != null)
            {
                i = knownsystems.FindIndex(x => x.Item1.Name.Equals(start.Name, StringComparison.InvariantCultureIgnoreCase));
                if (i == -1)
                    return -1;
            }

            for (i++; i < knownsystems.Count; i++)                          // from 1, or 1 past found, to end, accumulate distance
                distance += knownsystems[i].Item1.Distance(knownsystems[i-1].Item1);

            return distance;
        }


        public ISystem PosAlongRoute(double percentage, int error = 0)             // go along route and give me a co-ord along it..
        {
            List<Tuple<ISystem, int>> knownsystems = KnownSystemList();

            double totaldist = CumulativeDistance(null,knownsystems);
            double distleft = totaldist * percentage / 100.0;

            if (knownsystems.Count < 2)     // need a path
                return null;

            Point3D syspos = null;
            string name = "";

            if (percentage < 0 || percentage > 100)                         // not on route, interpolate to/from
            {
                int i = (percentage < 0) ? 0 : knownsystems.Count - 2;      // take first two, or last two.

                Point3D pos1 = P3D(knownsystems[i].Item1);
                Point3D pos2 = P3D(knownsystems[i + 1].Item1);
                double p12dist = pos1.Distance(pos2);
                double pospath = (percentage > 100) ? (1.0 + (percentage - 100) * totaldist / p12dist / 100.0) : (percentage * totaldist / p12dist / 100.0);
                syspos = pos1.PointAlongPath(pos2, pospath );       // amplify percentage by totaldist/this path dist
                name = "System at " + percentage.ToString("N1");
            }
            else
            {
                for (int i = 1; i < knownsystems.Count; i++)
                {
                    double d = knownsystems[i].Item1.Distance(knownsystems[i - 1].Item1);

                    if (distleft<d || (i==knownsystems.Count-1))        // if left, OR last system (allows for some rounding errors on floats)
                    {
                        d = distleft / d;
                        //System.Diagnostics.Debug.WriteLine(percentage + " " + d + " last:" + last.X + " " + last.Y + " " + last.Z + " s:" + s.X + " " + s.Y + " " + s.Z);
                        name = "WP" + knownsystems[i - 1].Item2.ToString() + "-" + "WP" + knownsystems[i].Item2.ToString() + "-" + d.ToString("#.00");
                        syspos = new Point3D(knownsystems[i - 1].Item1.X + (knownsystems[i].Item1.X - knownsystems[i - 1].Item1.X) * d, knownsystems[i - 1].Item1.Y + (knownsystems[i].Item1.Y - knownsystems[i - 1].Item1.Y) * d, knownsystems[i - 1].Item1.Z + (knownsystems[i].Item1.Z - knownsystems[i - 1].Item1.Z) * d);
                        break;
                    }

                    distleft -= d;
                }
            }

            if (error > 0)
                return new SystemClass(name, syspos.X + rnd.Next(error), syspos.Y + rnd.Next(error), syspos.Z + rnd.Next(error));
            else
                return new SystemClass(name, syspos.X, syspos.Y, syspos.Z);
        }

        Random rnd = new Random();

        // given the system list, which is the next waypoint to go to.  return the system (or null if not available or past end) and the waypoint.. (0 based) and the distance on the path left..

        public class ClosestInfo
        {
            public ISystem system;
            public int waypoint;                    // index of Systems
            public double deviation;                // -1 if not on path
            public double cumulativewpdist;         // distance after the WP, 0 means no more WPs after this
            public double disttowaypoint;           // distance to WP
            public ClosestInfo( ISystem s, int w, double dv, double wdl, double dtwp) { system = s; waypoint = w; deviation = dv; cumulativewpdist = wdl; disttowaypoint = dtwp; }
        }

        static Point3D P3D(ISystem s)
        {
            return new Point3D(s.X, s.Y, s.Z);
        }

        public ClosestInfo ClosestTo(ISystem currentsystem)
        {
            Point3D currentsystemp3d = P3D(currentsystem);

            List<Tuple<ISystem,int>> knownsystems = KnownSystemList();

            if (knownsystems.Count < 1)     // need at least one
                return null;

            double mininterceptdist = Double.MaxValue;
            int interceptendpoint = -1;

            double closesttodist = Double.MaxValue;
            int closestto = -1;

            for (int i = 0; i < knownsystems.Count; i++)
            {
                if (i > 0)
                {
                    Point3D lastp3d = P3D(knownsystems[i - 1].Item1);
                    Point3D top3d = P3D(knownsystems[i].Item1);

                    double distbetween = lastp3d.Distance(top3d);

                    double interceptpercent = lastp3d.InterceptPercentageDistance(top3d, currentsystemp3d, out double dist);       //dist to intercept point on line note.
                    //System.Diagnostics.Debug.WriteLine("From " + knownsystems[i - 1].ToString() + " to " + knownsystems[i].ToString() + " Percent " + interceptpercent + " Distance " + dist);

                    // allow a little margin in the intercept point for randomness, must be min dist, and not stupidly far.
                    if (interceptpercent >= -0.01 && interceptpercent < 1.01 && dist <= mininterceptdist && dist < distbetween)
                    {
                        interceptendpoint = i;
                        mininterceptdist = dist;
                    }
                }

                double disttofirstpoint = currentsystemp3d.Distance(P3D(knownsystems[i].Item1));

                if (disttofirstpoint < closesttodist)
                {
                    closesttodist = disttofirstpoint;
                    closestto = i;
                }
            }

            int topos = interceptendpoint;     // default value

            if (topos == -1)        // if not on path
            {
                topos = closestto;
                mininterceptdist = -1;
                //System.Diagnostics.Debug.WriteLine("Not on path, closest to" + knownsystems[closestto].ToString());
            }
            else
            { 
                //System.Diagnostics.Debug.WriteLine("Lies on line to WP" + interceptendpoint + " " + knownsystems[interceptendpoint].ToString());
            }

            double distto = currentsystemp3d.Distance(P3D(knownsystems[topos].Item1));
            double cumldist = CumulativeDistance(knownsystems[topos].Item1, knownsystems);

            return new ClosestInfo(knownsystems[topos].Item1, (int)knownsystems[topos].Item2, mininterceptdist, cumldist, distto);
        }

        // Given a set of expedition files, update the DB.  Add any new ones, and make sure the EDSM marker is on.
        // use a blank file to retire older ones

        public static bool UpdateDBFromExpeditionFiles(string expeditiondir )
        {
            bool changed = false;

            FileInfo[] allfiles = Directory.EnumerateFiles(expeditiondir, "*.json", SearchOption.TopDirectoryOnly).Select(f => new System.IO.FileInfo(f)).OrderByDescending(p => p.LastWriteTime).ToArray();

            foreach (FileInfo f in allfiles)        // iterate thru all files found
            {
                try
                {
                    string text = File.ReadAllText(f.FullName); // may except

                    if (text != null && text.Length>0)      // use a blank file to overwrite an old one you want to get rid of
                    {
                        var array = Newtonsoft.Json.Linq.JArray.Parse(text).ToObject<SavedRouteClass[]>();

                        List<SavedRouteClass> stored = SavedRouteClass.GetAllSavedRoutes(); // incl deleted

                        foreach (SavedRouteClass newentry in array)
                        {
                            if (newentry.StartDate.HasValue)
                                newentry.StartDate = newentry.StartDate.Value.ToLocalTime();      // supplied, and respected by JSON, as zulu time. the stupid database holds local times. Convert.
                            if (newentry.EndDate.HasValue)
                                newentry.EndDate = newentry.EndDate.Value.ToLocalTime();

                            SavedRouteClass storedentry = stored.Find(x => x.Name.Equals(newentry.Name));

                            if (newentry.Systems.Count == 0)              // no systems, means delete the database entry.. use with caution
                            {
                                if (storedentry != null)                // if we have it in the DB, delete it
                                {
                                    storedentry.Delete();
                                    changed = true;
                                }
                            }
                            else
                            {
                                newentry.EDSM = true;       // if we need to use newentry, then it must be marked as an EDSM one..

                                if (storedentry != null)  // if stored already..
                                {
                                    if (!storedentry.Systems.SequenceEqual(newentry.Systems)) // systems changed, we need to reset..
                                    {
                                        bool wasDel = storedentry.Deleted;
                                        storedentry.Delete();   // delete the old one.. systems may be in a different order, and there is no ordering except by ID in the DB
                                        newentry.Deleted = wasDel;
                                        newentry.Add();        // add to db..
                                        changed = changed || !wasDel;   // If it was marked deleted, don't report it as being changed.
                                    }
                                    else if (storedentry.EndDate != newentry.EndDate || storedentry.StartDate != newentry.StartDate)    // times change, just update
                                    {
                                        storedentry.StartDate = newentry.StartDate;             // update time and date but keep the expedition ID
                                        storedentry.EndDate = newentry.EndDate;
                                        storedentry.EDSM = true;
                                        storedentry.Update();
                                        changed = true;
                                    }
                                    else if (!storedentry.EDSM)    // backwards with previous system, set EDSM flag on these, prevents overwrite
                                    {
                                        storedentry.EDSM = true;    // ensure EDSM flag is set..
                                        storedentry.Update();
                                        changed = true;
                                    }
                                }
                                else
                                {                   // not there, add it..
                                    newentry.Add();        // add to db..
                                    changed = true;
                                }
                            }
                        }

                    }
                }
                catch { }       // bad file, try again
            }

            return changed;
        }

        public void TestHarness()       // fly the route and debug the closestto.. keep this for testing
        {
            for (double percent = -10; percent < 110; percent += 0.1)
            {
                ISystem cursys = PosAlongRoute(percent,100);
                System.Diagnostics.Debug.WriteLine(Environment.NewLine + "Sys {0} {1} {2} {3}", cursys.X, cursys.Y, cursys.Z, cursys.Name);

                if (cursys != null)
                {
                    ClosestInfo closest = ClosestTo(cursys);

                    if (closest != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Next {0} {1} {2} {3}, index {4} dev {5} dist to wp {6} cumul left {7}", closest.system?.X, closest.system?.Y, closest.system?.Z, closest.system?.Name, closest.waypoint, closest.deviation, closest.disttowaypoint, closest.cumulativewpdist);
                    }
                }
            }
        }
    }
}
