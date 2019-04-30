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

        public static ISystem FindSystem(long edsmid, SQLiteConnectionSystem cn = null)
        {
            return FindSystem(new SystemClass(edsmid),cn);
        }

        public static ISystem FindSystem(string name, SQLiteConnectionSystem cn = null)
        {
            return FindSystem(new SystemClass(name),cn);
        }

        public static ISystem FindSystem(ISystem find, SQLiteConnectionSystem cn = null)
        {
            ISystem orgsys = find;

            List<ISystem> foundlist = new List<ISystem>();

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

            ISystem found = null;

            if (find.HasCoordinate && foundlist.Count > 0)           // if sys has a co-ord, find the best match within 0.5 ly
                found = NearestTo(foundlist, find, 0.5);

            if (found == null && foundlist.Count == 1 && !find.HasCoordinate) // if we did not find one, but we have only 1 candidate, use it.
                found = foundlist[0];

            if (found == null)                                    // nope, no cache, so use the db
            {
                //System.Diagnostics.Debug.WriteLine("Look up from DB " + sys.name + " " + sys.id_edsm);

                bool owncn = cn == null;
                if (owncn)
                    cn = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Reader);

                if (find.EDSMID > 0)        // if we have an ID, look it up
                {
                    found = DB.SystemsDB.FindStar(find.EDSMID,cn);

                    if (found != null && find.Name.HasChars())      // if we find it, use the find name in the return as the EDSM name may be out of date..
                        found.Name = find.Name;
                }

                if (found == null && find.Name.HasChars())      // if not found by has a name
                    found = DB.SystemsDB.FindStar(find.Name,cn);   // find by name, no wildcards

                if (found == null && find.HasCoordinate)        // finally, not found, but we have a co-ord, find it from the db  by distance
                    found = DB.SystemsDB.GetSystemByPosition(find.X, find.Y, find.Z, cn);

                if (found == null)
                {
                    long newid = DB.SystemsDB.FindAlias(find.EDSMID, find.Name , cn);   // is there a named alias in there due to a system being renamed..
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

                if (owncn)
                    cn.Dispose();

                return found;
            }
            else
            {                                               // FROM CACHE
                //System.Diagnostics.Trace.WriteLine($"Cached reference to {found.name} {found.id_edsm}");
                return found;       // no need for extra work.
            }
        }

        //
        // Generally, cache is not used below, but systems are added to the cache to speed up above searches
        //

        static public List<ISystem> FindSystemWildcard(string name, int limit = int.MaxValue, SQLiteConnectionSystem cn = null)
        {
            bool owncn = cn == null;
            if (owncn)
                cn = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Reader);

            var list = DB.SystemsDB.FindStarWildcard(name, cn, limit);
            if (list != null)
            {
                foreach (var x in list)
                    AddToCache(x);
            }

            if (owncn)
                cn.Dispose();

            return list;
        }

        public static void GetSystemListBySqDistancesFrom(BaseUtils.SortedListDoubleDuplicate<ISystem> distlist, double x, double y, double z,
                                                    int maxitems,
                                                    double mindist, double maxdist, bool spherical, SQLiteConnectionSystem cn = null)
        {
            bool owncn = cn == null;
            if (owncn)
                cn = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Reader);

            DB.SystemsDB.GetSystemListBySqDistancesFrom(distlist, x, y, z, maxitems, mindist, maxdist, spherical, cn, (s) => AddToCache(s));
            if (distlist.Count > 0)
            {
                foreach (var s in distlist)
                {
                    AddToCache(s.Value);
                }
            }

            if (owncn)
                cn.Dispose();
        }

        public static ISystem GetSystemByPosition(double x, double y, double z, SQLiteConnectionSystem cn = null)
        {
            return FindNearestSystemTo(x, y, z, 0.125, cn);
        }

        public static ISystem FindNearestSystemTo(double x, double y, double z, double maxdistance, SQLiteConnectionSystem cn = null)
        {
            bool owncn = cn == null;
            if (owncn)
                cn = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Reader);

            ISystem s = DB.SystemsDB.GetSystemByPosition(x, y, z, cn, maxdistance);
            if (s != null)
                AddToCache(s);

            if (owncn)
                cn.Dispose();
            return s;
        }

        public static ISystem GetSystemNearestTo(Point3D currentpos,
                                                 Point3D wantedpos,
                                                 double maxfromcurpos,
                                                 double maxfromwanted,
                                                 int routemethod , 
                                                 int limitto, 
                                                 SQLiteConnectionSystem cn = null)
        {
            bool owncn = cn == null;
            if (owncn)
                cn = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Reader);

            ISystem sys = DB.SystemsDB.GetSystemNearestTo(currentpos, wantedpos, maxfromcurpos, maxfromwanted, routemethod, cn, (s) => AddToCache(s), limitto);

            if (owncn)
                cn.Dispose();

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

            return ret;
        }

        #endregion

        #region Helpers

        static private void AddToCache(ISystem found, ISystem orgsys = null)
        {
            if (found.EDSMID > 0)
                systemsByEdsmId[found.EDSMID] = found;  // must be definition the best ID found.. and if the update date of sys is better, its now been updated

            if (systemsByName.ContainsKey(found.Name))
            {
                if ( !systemsByName[found.Name].Contains(found))
                    systemsByName[found.Name].Add(found);   // add to list..
            }
            else
                systemsByName[found.Name] = new List<ISystem> { found }; // or make list
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

