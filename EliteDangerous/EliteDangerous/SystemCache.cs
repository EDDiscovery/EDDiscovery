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

        public static ISystem FindEDSM(ISystem sys, bool usedb = false, bool useedsm = false, DB.SQLiteConnectionSystem conn = null)
        {
            List<ISystem> systems = systemsByName.ContainsKey(sys.name) ? systemsByName[sys.name] : new List<ISystem>();
            ISystem sysbyid = sys.id_edsm != 0 && systemsByEdsmId.ContainsKey(sys.id_edsm) ? systemsByEdsmId[sys.id_edsm] : null;
            ISystem sysbynamepos = null;
            ISystem dbsys = null;

            if (systems.Count == 1 && !sys.HasCoordinate && sys.id_edsm < 1)
            {
                sysbynamepos = systems[0];
            }

            if (systems.Count != 0 && sys.HasCoordinate)
            {
                double minsqdist = 0.25;

                foreach (ISystem csys in systems)
                {
                    if (csys.HasCoordinate)
                    {
                        double sqdist = SqDist(sys, csys);
                        if (sqdist < minsqdist)
                        {
                            sysbynamepos = csys;
                            minsqdist = sqdist;
                        }
                    }
                }
            }

            if (sysbyid != null && sysbyid.HasCoordinate && sysbyid.UpdateDate > sys.UpdateDate)
            {
                return sysbyid;
            }
            else if (sysbynamepos != null && sysbynamepos.id_edsm > 0 && sysbynamepos.HasCoordinate && sysbynamepos.UpdateDate > sys.UpdateDate)
            {
                return sysbynamepos;
            }

            if (usedb)
            {
                dbsys = DB.SystemClassDB.FindEDSM(sys, conn: conn, useedsm: false);

                if (useedsm && dbsys == null && (sys.id_edsm <= 0 || !sys.HasCoordinate || DateTime.UtcNow.Subtract(sys.UpdateDate).TotalDays > 7))
                {
                    dbsys = DB.SystemClassDB.FindEDSM(sys, conn: conn, useedsm: true);
                }

                if (dbsys != null && dbsys.UpdateDate > sys.UpdateDate)
                {
                    sys = dbsys;
                }
            }

            if (sys.id_edsm > 0 && (sysbyid == null || sys.UpdateDate > sysbyid.UpdateDate))
            {
                systemsByEdsmId[sys.id_edsm] = sys;
            }

            if (sysbynamepos == null || sys.UpdateDate > sysbynamepos.UpdateDate)
            {
                if (systems != null)
                {
                    if (sysbynamepos != null)
                    {
                        systems.Remove(sysbynamepos);
                    }

                    systems.Add(sys);
                }
                else
                {
                    systemsByName[sys.name] = new List<ISystem> { sys };
                }
            }

            return dbsys;
        }
    }
}
