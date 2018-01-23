using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
{
    public static class SystemCache
    {
        private static Dictionary<long, ISystem> systemsByEdsmId = new Dictionary<long, ISystem>();
        private static Dictionary<string, List<ISystem>> systemsByName = new Dictionary<string, List<ISystem>>(StringComparer.InvariantCultureIgnoreCase);

        // may return null if not found
        // by design, it keeps on trying.  Rob thought about caching the misses but the problem is, this is done at start up
        // the system db may not be full at that point.  So a restart would be required to clear the misses..
        // difficult

        public static ISystem FindSystem(long edsmid, DB.SQLiteConnectionSystem conn = null)
        {
            return FindSystem(new SystemClass(edsmid),conn);
        }
        
        public static ISystem FindSystem(ISystem find, DB.SQLiteConnectionSystem conn = null)
        {
            ISystem orgsys = find;

            List<ISystem> foundlist = new List<ISystem>();

            if (find.id_edsm > 0 && systemsByEdsmId.ContainsKey(find.id_edsm))        // add to list
            {
                ISystem s = systemsByEdsmId[find.id_edsm];
                foundlist.Add(s);
            }

            if (systemsByName.ContainsKey(find.name))            // and all names cached
            {
                List<ISystem> s = systemsByName[find.name];
                foundlist.AddRange(s);
            }

            ISystem found = null;

            if (find.HasCoordinate && foundlist.Count > 0)           // if sys has a co-ord, find the best match within 0.5 ly
                found = NearestTo(foundlist, find , 0.5);

            if (found == null && foundlist.Count == 1)              // if we did not find one, but we have only 1 candidate, use it.
                found = foundlist[0];

            if ( found == null )                                    // nope, no cache, so use the db
            {
                //System.Diagnostics.Debug.WriteLine("Look up from DB " + sys.name + " " + sys.id_edsm);

                bool closeit = false;

                if (conn == null)       // we may touch the db multiple times, so if we don't have it open, do it.
                {
                    closeit = true;
                    conn = new DB.SQLiteConnectionSystem();
                }

                if (find.id_edsm > 0)        // if we have an ID, look it up
                {
                    found = DB.SystemClassDB.GetSystem(find.id_edsm, conn, DB.SystemClassDB.SystemIDType.EdsmId, name: find.name);
                }
                 
                // if not found, or no co-ord (unlikely), or its old as the hills, AND has a name

                if ((found == null || !found.HasCoordinate || DateTime.UtcNow.Subtract(found.UpdateDate).TotalDays > 7) && find.name.Length >= 2)
                {
                    List<ISystem> _systems = DB.SystemClassDB.GetSystemsByName(find.name, cn: conn);     // try DB via names..

                    if ( found != null )
                        _systems.Add(found);        // add on the EDSM ID candidate.. if present

                    if (_systems.Count == 1 && !find.HasCoordinate)  // 1 name, no co-ord, go with it as a back stop
                        found = _systems[0];
                    else if (_systems.Count > 0 && find.HasCoordinate)  // entries, and we have a co-ord to distinguish
                    {
                        found = NearestTo(_systems, find, 0.5);      // find it..
                    }                                               // else, found will be edsmid lookup if set..
                }

                if (found == null && find.HasCoordinate)           // finally, not found, but we have a co-ord, find it from the db  by distance
                    found = DB.SystemClassDB.GetSystemNearestTo(find.x, find.y, find.z, conn);

                if (closeit && conn != null)                // finished with db, close
                {
                    conn.Dispose();
                }

                if (found != null)                              // if we have a good db, go for it
                {
                    if ((find.HasCoordinate && !found.HasCoordinate) || find.UpdateDate > found.UpdateDate) // if found does not have co-ord, or sys is more up to date..
                    {
                        found.x = find.x; found.y = find.y; found.z = find.z;
                    }

                    if (find.UpdateDate > found.UpdateDate)      // Bravada, check that we are not overwriting newer info..
                    {                                           // if the journal info is newer than the db system name, lets presume the journal data is better
                        found.name = find.name;
                        found.UpdateDate = find.UpdateDate;
                    }

                    if (found.id_edsm > 0)
                    {
                        systemsByEdsmId[found.id_edsm] = found;         // must be definition the best ID found.. and if the update date of sys is better, its now been updated
                    }

                    if (systemsByName.ContainsKey(orgsys.name))    // use the original name we looked it up in, if we found one.. remove it
                    {
                        systemsByName[orgsys.name].Remove(orgsys);  // and remove 
                    }

                    if (systemsByName.ContainsKey(found.name))
                        systemsByName[found.name].Add(found);   // add to list..
                    else
                        systemsByName[found.name] = new List<ISystem> { found }; // or make list

                    //System.Diagnostics.Trace.WriteLine($"DB found {found.name} {found.id_edsm} sysid {found.id_edsm}");

                    return found;
                }
                else
                {
                    //System.Diagnostics.Trace.WriteLine($"DB NOT found {find.name} {find.id_edsm} ");
                    return null;
                }
            }
            else
            {                                               // FROM CACHE
                //System.Diagnostics.Trace.WriteLine($"Cached reference to {found.name} {found.id_edsm}");
                return found;       // no need for extra work.
            }

        }

        static private ISystem NearestTo(List<ISystem> list, ISystem comparesystem ,double mindist)
        {
            ISystem dbsys = null;

            foreach (ISystem isys in list)
            {
                if (isys.HasCoordinate)
                {
                    double dist = isys.Distance(comparesystem);

                    if (dist < mindist)
                    {
                        mindist = dist;
                        dbsys = isys;
                    }
                }
            }

            return dbsys;
        }
    }
}


