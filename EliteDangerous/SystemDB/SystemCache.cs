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

using EMK.LightGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore.DB
{
    public static class SystemCache
    {
        // may return null if not found
        // by design, it keeps on trying.  Rob thought about caching the misses but the problem is, this is done at start up
        // the system db may not be full at that point.  So a restart would be required to clear the misses..
        // difficult

        #region Public Interface for Find System

        public static ISystem FindSystem(long edsmid)
        {
            return FindSystem(new SystemClass(edsmid));
        }

        public static ISystem FindSystem(string name, long edsmid)
        {
            return FindSystem(new SystemClass(name, edsmid));
        }

        public static ISystem FindSystem(string name)
        {
            return FindSystem(new SystemClass(name));
        }

        public static ISystem FindSystem(ISystem find)
        {
            if (SystemsDatabase.Instance.RebuildRunning) // Find the system in the cache if a rebuild is running
            {
                return FindSystem(find, null);
            }
            else 
            {
                return SystemsDatabase.Instance.ExecuteWithDatabase(conn => FindSystem(find, conn));
            }
        }

        internal static ISystem FindSystem(string name, long edsmid, SystemsDatabaseConnection cn)
        {
            return FindSystem(new SystemClass(name, edsmid), cn);
        }

        internal static ISystem FindSystem(ISystem find, SystemsDatabaseConnection cn)
        {
            ISystem orgsys = find;

            ISystem found = FindCachedSystem(find);

            if ((found == null || found.source != SystemSource.FromEDSM) && cn != null && !SystemsDatabase.Instance.RebuildRunning)   // not cached, connection and not rebuilding, try db
            {
                //System.Diagnostics.Debug.WriteLine("Look up from DB " + sys.name + " " + sys.id_edsm);

                if (find.EDSMID > 0)        // if we have an ID, look it up
                {
                    found = DB.SystemsDB.FindStar(find.EDSMID,cn.Connection);

                    if (found != null && find.Name.HasChars() && find.Name != "UnKnown")      // if we find it, use the find name in the return as the EDSM name may be out of date..
                        found.Name = find.Name;
                }

                if (found == null && find.Name.HasChars() && find.Name != "UnKnown")      // if not found by has a name
                    found = DB.SystemsDB.FindStar(find.Name,cn.Connection);   // find by name, no wildcards

                if (found == null && find.HasCoordinate)        // finally, not found, but we have a co-ord, find it from the db  by distance
                    found = DB.SystemsDB.GetSystemByPosition(find.X, find.Y, find.Z, cn.Connection);

                if (found == null)
                {
                    long newid = DB.SystemsDB.FindAlias(find.EDSMID, find.Name , cn.Connection);   // is there a named alias in there due to a system being renamed..
                    if (newid >= 0)
                        found = DB.SystemsDB.FindStar(newid);  // find it using the new id
                }

                if (found != null)                              // if we have a good db, go for it
                {
                    if (find.HasCoordinate)                     // if find has co-ordinate, it may be more up to date than the DB, so use it
                    {
                        found.X = find.X; found.Y = find.Y; found.Z = find.Z;
                    }

                    lock (systemsByEdsmId)          // lock to prevent multi change over these classes
                    {
                        if (systemsByName.ContainsKey(orgsys.Name))   // so, if name database already has name
                            systemsByName[orgsys.Name].Remove(orgsys);  // and remove the ISystem if present on that orgsys

                        AddToCache(found);
                    }

                    //System.Diagnostics.Trace.WriteLine($"DB found {found.name} {found.id_edsm} sysid {found.id_edsm}");
                }

                return found;
            }
            else
            {                                               // FROM CACHE
                if (found != null)
                {
                    return found;
                }

                if (find.source == SystemSource.FromJournal && find.Name != null && find.HasCoordinate == true)
                {
                    AddToCache(find);
                    return find;
                }

                //System.Diagnostics.Trace.WriteLine($"Cached reference to {found.name} {found.id_edsm}");
                return found;       // no need for extra work.
            }
        }

        private static ISystem FindCachedSystem(ISystem find)
        {
            List<ISystem> foundlist = new List<ISystem>();
            ISystem found = null;

            lock (systemsByEdsmId)          // Rob seen instances of it being locked together in multiple star distance threads, we need to serialise access to these two dictionaries
            {                               // Concurrent dictionary no good, they could both be about to add the same thing at the same time and pass the contains test.

                if (find.EDSMID > 0 && systemsByEdsmId.ContainsKey(find.EDSMID))        // add to list
                {
                    ISystem s = systemsByEdsmId[find.EDSMID];
                    foundlist.Add(s);
                }

                if (systemsByName.ContainsKey(find.Name))            // and all names cached
                {
                    List<ISystem> s = systemsByName[find.Name];
                    foundlist.AddRange(s);
                }
            }

            if (find.HasCoordinate && foundlist.Count > 0)           // if sys has a co-ord, find the best match within 0.5 ly
                found = NearestTo(foundlist, find, 0.5);

            if (found == null && foundlist.Count == 1 && !find.HasCoordinate) // if we did not find one, but we have only 1 candidate, use it.
                found = foundlist[0];

            return found;
        }

        public static ISystem FindCachedJournalSystem(ISystem system)
        {
            var found = FindCachedSystem(system);

            if ((found == null || (found.source != SystemSource.FromJournal && found.source != SystemSource.FromEDSM)) && system.HasCoordinate == true && system.Name != "")
            {
                AddToCache(system, found);
                found = system;
            }

            return found;
        }

        public static List<ISystem> FindSystemWildcard(string name, int limit = int.MaxValue)
        {
            if (SystemsDatabase.Instance.RebuildRunning) // use the cache is db is updating
            {
                lock (systemsByEdsmId)
                {
                    name = name.ToLowerInvariant();

                    return systemsByName.Where(kvp => kvp.Key.ToLowerInvariant().StartsWith(name))
                                        .SelectMany(s => s.Value)
                                        .Take(limit)
                                        .ToList();
                }
            }
            else
            {
                return SystemsDatabase.Instance.ExecuteWithDatabase(conn => FindSystemWildcard(name, conn, limit));
            }
        }

        static private List<ISystem> FindSystemWildcard(string name, SystemsDatabaseConnection cn, int limit = int.MaxValue)
        {
            var list = DB.SystemsDB.FindStarWildcard(name, cn.Connection, limit);
            if (list != null)
            {
                foreach (var x in list)
                    AddToCache(x);
            }

            return list;
        }

        public static void GetSystemListBySqDistancesFrom(BaseUtils.SortedListDoubleDuplicate<ISystem> distlist, double x, double y, double z,
                                                    int maxitems,
                                                    double mindist, double maxdist, bool spherical)
        {
            if (SystemsDatabase.Instance.RebuildRunning) // Return from cache if rebuild is running
            {
                lock (systemsByEdsmId)
                {
                    var sysdist = systemsByName.Values
                                               .SelectMany(s => s)
                                               .Select(s => new { distsq = s.DistanceSq(x, y, z), sys = s })
                                               .OrderBy(s => s.distsq)
                                               .ToList();
                    var minsq = mindist * mindist;
                    var maxsq = maxdist * maxdist;

                    foreach (var sd in sysdist)
                    {
                        if (sd.distsq <= minsq && sd.distsq >= maxsq)
                        {
                            distlist.Add(sd.distsq, sd.sys);
                        }

                        if (distlist.Count >= maxitems)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                SystemsDatabase.Instance.ExecuteWithDatabase(conn => GetSystemListBySqDistancesFrom(distlist, x, y, z, maxitems, mindist, maxdist, spherical, conn));
            }
        }

        private static void GetSystemListBySqDistancesFrom(BaseUtils.SortedListDoubleDuplicate<ISystem> distlist, double x, double y, double z,
                                                    int maxitems,
                                                    double mindist, double maxdist, bool spherical, SystemsDatabaseConnection cn)
        {
            DB.SystemsDB.GetSystemListBySqDistancesFrom(distlist, x, y, z, maxitems, mindist, maxdist, spherical, cn.Connection, (s) => AddToCache(s));
        }

        public static ISystem GetSystemByPosition(double x, double y, double z, int warnthreshold = 500)
        {
            return FindNearestSystemTo(x, y, z, 0.125, warnthreshold);
        }

        public static ISystem FindNearestSystemTo(double x, double y, double z, double maxdistance, int warnthreshold = 500)
        {
            if (SystemsDatabase.Instance.RebuildRunning) // Return from cache if rebuild is running
            {
                lock (systemsByEdsmId)
                {
                    return systemsByName.Values
                                        .SelectMany(s => s)
                                        .OrderBy(s => s.Distance(x, y, z))
                                        .FirstOrDefault(e => e.Distance(x, y, z) < maxdistance);
                }
            }
            else
            {
                return SystemsDatabase.Instance.ExecuteWithDatabase(conn => FindNearestSystemTo(x, y, z, maxdistance, conn), warnthreshold: warnthreshold);
            }
        }

        private static ISystem FindNearestSystemTo(double x, double y, double z, double maxdistance, SystemsDatabaseConnection cn)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch(); sw.Start();  System.Diagnostics.Debug.WriteLine("Look up " + x + "," + y + "," + z + " : " + maxdistance);

            ISystem s = DB.SystemsDB.GetSystemByPosition(x, y, z, cn.Connection, maxdistance);
            if (s != null)
                AddToCache(s);

            //System.Diagnostics.Debug.WriteLine(".. lookup done " + sw.ElapsedMilliseconds);
            return s;
        }

         public static ISystem GetSystemNearestTo(Point3D currentpos,
                                                 Point3D wantedpos,
                                                 double maxfromcurpos,
                                                 double maxfromwanted,
                                                 SystemsDB.SystemsNearestMetric routemethod,
                                                 int limitto)
        {
            if (SystemsDatabase.Instance.RebuildRunning)    // return from cache is rebuild is running
            {
                lock (systemsByEdsmId)
                {
                    var candidates =
                        systemsByName.Values
                                     .SelectMany(s => s)
                                     .Select(s => new
                                     {
                                         dc = s.Distance(currentpos.X, currentpos.Y, currentpos.Z),
                                         dw = s.Distance(wantedpos.X, wantedpos.Y, wantedpos.Z),
                                         sys = s
                                     })
                                     .Where(s => s.dw < maxfromwanted && s.dc < maxfromcurpos)
                                     .OrderBy(s => s.dw)
                                     .Select(s => s.sys)
                                     .ToList();

                    return DB.SystemsDB.GetSystemNearestTo(candidates, currentpos, wantedpos, maxfromcurpos, maxfromwanted, routemethod);
                }
            }
            else
            {
                return SystemsDatabase.Instance.ExecuteWithDatabase(conn => GetSystemNearestTo(currentpos, wantedpos, maxfromcurpos, maxfromwanted, routemethod, limitto, conn));
            }
        }

        private static ISystem GetSystemNearestTo(Point3D currentpos,
                                                 Point3D wantedpos,
                                                 double maxfromcurpos,
                                                 double maxfromwanted,
                                                 SystemsDB.SystemsNearestMetric routemethod,
                                                 int limitto, 
                                                 SystemsDatabaseConnection cn)
        {
            ISystem sys = DB.SystemsDB.GetSystemNearestTo(currentpos, wantedpos, maxfromcurpos, maxfromwanted, routemethod, cn.Connection, (s) => AddToCache(s), limitto);

            return sys;
        }

        #endregion

        #region Autocomplete

        // use this for additional autocompletes outside of the normal stars
        public static void AddToAutoCompleteList(List<string> t)
        {
            lock (AutoCompleteAdditionalList)
            {
                AutoCompleteAdditionalList.AddRange(t);
            }
        }

        public static List<string> ReturnSystemAdditionalListForAutoComplete(string input, Object ctrl)
        {
            List<string> ret = new List<string>();
            ret.AddRange(ReturnAdditionalAutoCompleteList(input, ctrl));
            ret.AddRange(ReturnSystemAutoCompleteList(input, ctrl));
            return ret;
        }

        public static List<string> ReturnAdditionalAutoCompleteList(string input, Object ctrl)
        {
            List<string> ret = new List<string>();

            if (input != null && input.Length > 0)
            {
                lock (AutoCompleteAdditionalList)
                {
                    foreach (string other in AutoCompleteAdditionalList)
                    {
                        if (other.StartsWith(input, StringComparison.InvariantCultureIgnoreCase))
                            ret.Add(other);
                    }
                }
            }
            return ret;
        }

        public static int MaximumStars { get; set; } = 1000;

        public static List<string> ReturnSystemAutoCompleteList(string input, Object ctrl)
        {
            List<string> ret = new List<string>();

            if (input.HasChars())
            {
                if (SystemsDatabase.Instance.RebuildRunning)
                {
                    lock (systemsByEdsmId)
                    {
                        input = input.ToLowerInvariant();

                        foreach (var kvp in systemsByName)
                        {
                            if (kvp.Key.ToLowerInvariant().StartsWith(input))
                            {
                                foreach (var s in kvp.Value)
                                {
                                    ret.Add(s.Name);
                                }
                            }
                        }
                    }
                }
                else
                {
                    List<ISystem> systems = DB.SystemsDB.FindStarWildcard(input, MaximumStars);
                    foreach (var i in systems)
                    {
                        AddToCache(i);
                        ret.Add(i.Name);
                    }

                    List<ISystem> aliases = DB.SystemsDB.FindAliasWildcard(input);
                    foreach (var i in aliases)
                    {
                        AddToCache(i);
                        ret.Add(i.Name);
                    }
                }
            }

            return ret;
        }

        #endregion

        #region Helpers

        static private void AddToCache(ISystem found, ISystem orgsys = null)
        {
            lock (systemsByEdsmId)
            {
                if (found.EDSMID > 0)
                    systemsByEdsmId[found.EDSMID] = found;  // must be definition the best ID found.. and if the update date of sys is better, its now been updated

                List<ISystem> byname;

                if (!systemsByName.TryGetValue(found.Name, out byname))
                {
                    systemsByName[found.Name] = byname = new List<ISystem>();
                }

                int idx = -1;

                if (found.EDSMID > 0)
                {
                    idx = byname.FindIndex(e => e.EDSMID == found.EDSMID);
                }

                if (idx < 0)
                {
                    idx = byname.FindIndex(e => e.Xi == found.Xi && e.Yi == found.Yi && e.Zi == found.Zi);
                }

                if (idx < 0 && orgsys != null)
                {
                    idx = byname.FindIndex(e => e.Xi == orgsys.Xi && e.Yi == orgsys.Yi && e.Zi == orgsys.Zi);
                }

                if (idx >= 0)
                {
                    byname[idx] = found;
                }
                else
                {
                    byname.Add(found);
                }
            }
        }

        static private ISystem NearestTo(List<ISystem> list, ISystem comparesystem, double mindist)
        {
            ISystem nearest = null;

            foreach (ISystem isys in list)
            {
                if (isys.HasCoordinate)
                {
                    double dist = isys.Distance(comparesystem);

                    if (dist < mindist)
                    {
                        mindist = dist;
                        nearest = isys;
                    }
                }
            }

            return nearest;
        }

        private static Dictionary<long, ISystem> systemsByEdsmId = new Dictionary<long, ISystem>();
        private static Dictionary<string, List<ISystem>> systemsByName = new Dictionary<string, List<ISystem>>(StringComparer.InvariantCultureIgnoreCase);

        private static List<string> AutoCompleteAdditionalList = new List<string>();

        #endregion
    }

}

