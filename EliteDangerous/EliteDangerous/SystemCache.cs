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

        public static List<ISystem> GetSystemsByName(string name)
        {
            if (systemsByName.ContainsKey(name))
            {
                return systemsByName[name];
            }
            else
            {
                List<ISystem> systems = DB.SystemClassDB.GetSystemsByName(name);

                if (systems.Count != 0)
                {
                    systemsByName[name] = systems;

                    foreach (ISystem sys in systems)
                    {
                        if (sys.id_edsm > 0)
                        {
                            systemsByEdsmId[sys.id_edsm] = sys;
                        }
                    }
                }

                return systems;
            }
        }

        public static ISystem GetSystemByEdsmId(long id)
        {
            if (systemsByEdsmId.ContainsKey(id))
            {
                return systemsByEdsmId[id];
            }
            else
            {
                ISystem sys = DB.SystemClassDB.GetSystem(id, idtype: DB.SystemClassDB.SystemIDType.EdsmId);
                if (sys != null)
                {
                    systemsByEdsmId[id] = sys;
                }
                return sys;
            }
        }

        private static double SqDist(ISystem a, ISystem b)
        {
            double dx = a.x - b.x;
            double dy = a.y - b.y;
            double dz = a.z - b.z;
            return dx * dx + dy * dy + dz * dz;
        }

        public static ISystem FindSystemConditional(ISystem sys, bool reload, DB.SQLiteConnectionSystem conn = null)
        {
            if (sys.status == SystemStatusEnum.EDSM || (!reload && sys.id_edsm == -1))  // if set already, or we tried and failed..
                return null;

            return FindSystem(sys, conn);
        }

        public static ISystem FindSystem(ISystem sys, DB.SQLiteConnectionSystem conn = null)
        {
            ISystem orgsys = sys;

            List<ISystem> foundlist = new List<ISystem>();

            if (sys.id_edsm > 0 && systemsByEdsmId.ContainsKey(sys.id_edsm))        // add to list
                foundlist.Add(systemsByEdsmId[sys.id_edsm]);

            if (systemsByName.ContainsKey(sys.name))            // and all names cached
                foundlist.AddRange(systemsByName[sys.name]);

            ISystem found = null;

            if (sys.HasCoordinate && foundlist.Count > 0)           // if sys has a co-ord, find the best match within 0.5 ly
                found = NearestTo(foundlist, sys , 0.5);

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

                if (sys.id_edsm > 0)        // if we have an ID, look it up
                {
                    found = DB.SystemClassDB.GetSystem(sys.id_edsm, conn, DB.SystemClassDB.SystemIDType.EdsmId);
                }
                 
                // if not found, or no co-ord (unlikely), or its old as the hills, AND has a name

                if ((found == null || !found.HasCoordinate || DateTime.UtcNow.Subtract(found.UpdateDate).TotalDays > 7) && sys.name.Length >= 2)
                {
                    List<ISystem> _systems = DB.SystemClassDB.GetSystemsByName(sys.name, cn: conn);     // try DB via names..

                    if ( found != null )
                        _systems.Add(found);        // add on the EDSM ID candidate.. if present

                    if (_systems.Count == 1 && !sys.HasCoordinate)  // 1 name, no co-ord, go with it as a back stop
                        found = _systems[0];
                    else if (_systems.Count > 0 && sys.HasCoordinate)  // entries, and we have a co-ord to distinguish
                    {
                        found = NearestTo(_systems, sys, 0.5);      // find it..
                    }                                               // else, found will be edsmid lookup if set..
                }

                if (found == null && sys.HasCoordinate)           // finally, not found, but we have a co-ord, find it from the db  by distance
                    found = DB.SystemClassDB.GetSystemNearestTo(sys.x, sys.y, sys.z, conn);

                if (closeit && conn != null)                // finished with db, close
                {
                    conn.Dispose();
                }

                if (found != null)                              // if we have a good db, go for it
                {
                    if ((sys.HasCoordinate && !found.HasCoordinate) || sys.UpdateDate > found.UpdateDate) // if found does not have co-ord, or sys is more up to date..
                    {
                        found.x = sys.x; found.y = sys.y; found.z = sys.z;
                    }

                    if (sys.UpdateDate > found.UpdateDate)      // Bravada, check that we are not overwriting newer info..
                    {                                           // if the journal info is newer than the db system name, lets presume the journal data is better
                        found.name = sys.name;
                        found.UpdateDate = sys.UpdateDate;
                    }

                    //System.Diagnostics.Trace.WriteLine($"DB found {found.name} {found.id_edsm} sysid {sys.id_edsm}");
                    sys = found;
                }
                else
                {
                    //System.Diagnostics.Trace.WriteLine($"DB NOT found {sys.name} {sys.id_edsm} ");
                }
            }
            else
            {                                               // FROM CACHE
                sys = found;
                //System.Diagnostics.Trace.WriteLine($"Cached reference to {sys.name} {sys.id_edsm}");
            }

            if (sys.id_edsm > 0)
            {
                //if (!systemsByEdsmId.ContainsKey(sys.id_edsm)) System.Diagnostics.Trace.WriteLine($"Added {sys.name} to id cache");
                systemsByEdsmId[sys.id_edsm] = sys;         // must be definition the best ID found.. and if the update date of sys is better, its now been updated
            }

            if (found != null && systemsByName.ContainsKey(orgsys.name))    // use the original name we looked it up in, if we found one.. remove it
            {
                systemsByName[orgsys.name].Remove(found);  // if found did not come from this, no worries
            }

            if (systemsByName.ContainsKey(sys.name))
                systemsByName[sys.name].Add(sys);   // add to list..
            else
                systemsByName[sys.name] = new List<ISystem> { sys }; // or make list

            return sys;
        }

        static private ISystem NearestTo(List<ISystem> list, ISystem comparesystem ,double mindist)
        {
            ISystem dbsys = null;

            foreach (ISystem isys in list)
            {
                if (isys.HasCoordinate)
                {
                    double dist = DB.SystemClassDB.Distance(isys, comparesystem);

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


