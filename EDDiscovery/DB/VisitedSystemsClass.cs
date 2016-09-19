using EDDiscovery.DB;
using EDDiscovery.EDSM;
using EMK.LightGeometry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EDDiscovery2.DB
{
    public class VisitedSystemsClass : InMemory.VisitedSystemsClass
    {
        public ISystem curSystem;
        public ISystem prevSystem;
        public ISystem lastKnownSystem;
        public string strDistance;
        public string NameStatus;
        public List<ISystem> alternatives;

        public bool IsAmbiguous
        {
            get
            {
                return alternatives != null && (alternatives.Count > 1 || (curSystem == null && alternatives.Count >= 1));
            }
        }

        public VisitedSystemsClass()
        {
            X = double.NaN;             // construct class with nulls/0's except for these, which use NaN as their no content marker.
            Y = double.NaN;
            Z = double.NaN;
        }

        public VisitedSystemsClass(DataRow dr)
        {
            id = (long)dr["id"];
            Name = (string)dr["Name"];
            Time = (DateTime)dr["Time"];
            Commander = (int)(long)dr["Commander"];
            Source = (long)dr["Source"];
            Unit = (string)dr["Unit"];
            EDSM_sync = (bool)dr["edsm_sync"];
            MapColour = (int)(long)dr["Map_colour"];

            if (System.DBNull.Value == dr["X"])
            {
                X = double.NaN;
                Y = double.NaN;
                Z = double.NaN;
            }
            else
            {
                X = (double)dr["X"];
                Y = (double)dr["Y"];
                Z = (double)dr["Z"];
            }

            if (dr["id_edsm_assigned"] != System.DBNull.Value)
            {
                id_edsm_assigned = (long)dr["id_edsm_assigned"];
            }
        }

        public VisitedSystemsClass(DbDataReader reader)
        {
            id = (long)reader["id"];
            Name = (string)reader["Name"];
            Time = (DateTime)reader["Time"];
            Commander = (int)(long)reader["Commander"];
            Source = (long)reader["Source"];
            Unit = (string)reader["Unit"];
            EDSM_sync = (bool)reader["edsm_sync"];
            MapColour = (int)(long)reader["Map_colour"];

            if (reader["X"] == DBNull.Value)
            {
                X = double.NaN;
                Y = double.NaN;
                Z = double.NaN;
            }
            else
            {
                X = (double)reader["X"];
                Y = (double)reader["Y"];
                Z = (double)reader["Z"];
            }

            if (reader["id_edsm_assigned"] != DBNull.Value)
            {
                id_edsm_assigned = (long)reader["id_edsm_assigned"];
            }
        }

        public bool HasTravelCoordinates
        {
            get
            {
                return !double.IsNaN(X);
            }
        }

        public bool Add()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                bool ret = Add(cn);
                return ret;
            }
        }

        public bool Add(SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into VisitedSystems (Name, Time, Unit, Commander, Source, edsm_sync, map_colour, X, Y, Z, id_edsm_assigned) values (@name, @time, @unit, @commander, @source, @edsm_sync, @map_colour, @x, @y, @z, @id_edsm_assigned)", tn))
            {
                cmd.AddParameterWithValue("@name", Name);
                cmd.AddParameterWithValue("@time", Time);
                cmd.AddParameterWithValue("@unit", Unit);
                cmd.AddParameterWithValue("@commander", Commander);
                cmd.AddParameterWithValue("@source", Source);
                cmd.AddParameterWithValue("@edsm_sync", EDSM_sync);
                cmd.AddParameterWithValue("@map_colour", MapColour);
                cmd.AddParameterWithValue("@x", X);
                cmd.AddParameterWithValue("@y", Y);
                cmd.AddParameterWithValue("@z", Z);
                cmd.AddParameterWithValue("@id_edsm_assigned", id_edsm_assigned);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from VisitedSystems"))
                {
                    id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }
                return true;
            }
        }

        public new bool Update()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Update VisitedSystems set Name=@Name, Time=@Time, Unit=@Unit, Commander=@commander, Source=@Source, edsm_sync=@edsm_sync, map_colour=@map_colour, X=@x, Y=@y, Z=@z, id_edsm_assigned=@id_edsm_assigned where ID=@id"))
            {
                cmd.AddParameterWithValue("@ID", id);
                cmd.AddParameterWithValue("@Name", Name);
                cmd.AddParameterWithValue("@Time", Time);
                cmd.AddParameterWithValue("@unit", Unit);
                cmd.AddParameterWithValue("@commander", Commander);
                cmd.AddParameterWithValue("@source", Source);
                cmd.AddParameterWithValue("@edsm_sync", EDSM_sync);
                cmd.AddParameterWithValue("@map_colour", MapColour);
                cmd.AddParameterWithValue("@x", X);
                cmd.AddParameterWithValue("@y", Y);
                cmd.AddParameterWithValue("@z", Z);
                cmd.AddParameterWithValue("@id_edsm_assigned", id_edsm_assigned);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                return true;
            }
        }






        static public List<VisitedSystemsClass> GetAll()
        {
            List<VisitedSystemsClass> list = new List<VisitedSystemsClass>();

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = cn.CreateCommand("select * from VisitedSystems Order by Time "))
                {
                    DataSet ds = SQLiteDBClass.SQLQueryText(cn, cmd);

                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return list;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        VisitedSystemsClass sys = new VisitedSystemsClass(dr);
                        list.Add(sys);
                    }

                    return list;
                }
            }
        }


        static public List<VisitedSystemsClass> GetAll(int commander)
        {
            List<VisitedSystemsClass> list = new List<VisitedSystemsClass>();

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = cn.CreateCommand("select * from VisitedSystems where commander=@commander Order by Time "))
                {
                    cmd.AddParameterWithValue("@commander", commander);

                    DataSet ds = SQLiteDBClass.SQLQueryText(cn, cmd);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return list;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        VisitedSystemsClass sys = new VisitedSystemsClass(dr);
                        list.Add(sys);
                    }

                    return list;
                }
            }
        }

        public static List<VisitedSystemsClass> GetAll(TravelLogUnit tlu)
        {
            List<VisitedSystemsClass> vsc = new List<VisitedSystemsClass>();
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT * FROM VisitedSystems WHERE Source = @source ORDER BY Time ASC"))
                {
                    cmd.AddParameterWithValue("@source", tlu.id);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vsc.Add(new VisitedSystemsClass(reader));
                        }
                    }
                }
            }
            return vsc;
        }

        static public VisitedSystemsClass GetLast()
        {
            List<VisitedSystemsClass> list = new List<VisitedSystemsClass>();

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = cn.CreateCommand("select * from VisitedSystems Order by Time DESC Limit 1"))
                {
                    DataSet ds = SQLiteDBClass.SQLQueryText(cn, cmd);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        return null;
                    }

                    VisitedSystemsClass sys = new VisitedSystemsClass(ds.Tables[0].Rows[0]);
                    return sys;
                }
            }
        }

        public static VisitedSystemsClass GetLast(int cmdrid, DateTime before)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT * FROM VisitedSystems WHERE Commander = @commander AND Time < @before ORDER BY Time DESC LIMIT 1"))
                {
                    cmd.AddParameterWithValue("@commander", cmdrid);
                    cmd.AddParameterWithValue("@before", before);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new VisitedSystemsClass(reader);
                        }
                    }
                }
            }

            return null;
        }

        internal static bool Exist(string name, DateTime time)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = cn.CreateCommand("select * from VisitedSystems where name=@name and Time=@time  Order by Time DESC Limit 1"))
                {
                    cmd.AddParameterWithValue("@name", name);
                    cmd.AddParameterWithValue("@time", time);
                    DataSet ds = SQLiteDBClass.SQLQueryText(cn, cmd);
                    return !(ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0);
                }
            }
        }

        public static void CalculateSqDistances(List<VisitedSystemsClass> vs, SortedList<double, ISystem> distlist, double x, double y, double z, int maxitems, bool removezerodiststar)
        {
            double dist;
            double dx, dy, dz;
            Dictionary<long, ISystem> systems = distlist.Values.GroupBy(s => s.id).ToDictionary(g => g.Key, g => g.First());

            foreach (VisitedSystemsClass pos in vs)
            {
                if (pos.HasTravelCoordinates && pos.curSystem != null && !systems.ContainsKey(pos.curSystem.id))   // if co-ords, and not in list already..
                {
                    dx = (pos.X - x);
                    dy = (pos.Y - y);
                    dz = (pos.Z - z);
                    dist = dx * dx + dy * dy + dz * dz;

                    if (dist > 0.001 || !removezerodiststar)
                    {
                        if (distlist.Count < maxitems)          // if less than max, add..
                            distlist.Add(dist, pos.curSystem);
                        else if (dist < distlist.Last().Key)   // if last entry (which must be the biggest) is greater than dist..
                        {
                            distlist.Add(dist, pos.curSystem);           // add in
                            distlist.RemoveAt(maxitems);        // remove last..
                        }
                    }
                }
            }
        }

        // centresysname is a default one

        public static string FindNextVisitedSystem(List<VisitedSystemsClass> _visitedSystems, string sysname, int dir, string centresysname)
        {
            int index = _visitedSystems.FindIndex(x => x.Name.Equals(sysname));

            if (index != -1)
            {
                if (dir == -1)
                {
                    if (index < 1)                                  //0, we go to the end and work from back..                      
                        index = _visitedSystems.Count;

                    int indexn = _visitedSystems.FindLastIndex(index - 1, x => x.HasTravelCoordinates || (x.curSystem != null && x.curSystem.HasCoordinate));

                    if (indexn == -1)                             // from where we were, did not find one, try from back..
                        indexn = _visitedSystems.FindLastIndex(x => x.HasTravelCoordinates || (x.curSystem != null && x.curSystem.HasCoordinate));

                    return (indexn != -1) ? _visitedSystems[indexn].Name : centresysname;
                }
                else
                {
                    index++;

                    if (index == _visitedSystems.Count)             // if at end, go to beginning
                        index = 0;

                    int indexn = _visitedSystems.FindIndex(index, x => x.HasTravelCoordinates || (x.curSystem != null && x.curSystem.HasCoordinate));

                    if (indexn == -1)                             // if not found, go to beginning
                        indexn = _visitedSystems.FindIndex(x => x.HasTravelCoordinates || (x.curSystem != null && x.curSystem.HasCoordinate));

                    return (indexn != -1) ? _visitedSystems[indexn].Name : centresysname;
                }
            }
            else
            {
                index = _visitedSystems.FindLastIndex(x => x.HasTravelCoordinates || (x.curSystem != null && x.curSystem.HasCoordinate));
                return (index != -1) ? _visitedSystems[index].Name : centresysname;
            }
        }

        public static void UpdateSys(List<VisitedSystemsClass> visitedSystems, bool usedistancedb, bool matchsystems)          // oldest system is lowest index
        {
            if (matchsystems)
                SystemClass.FillVisitedSystems(visitedSystems);                 // first try and populate with SystemClass info

            foreach (VisitedSystemsClass vsc in visitedSystems)
            {
                if (vsc.curSystem == null)                                  // if no systemclass info, make a dummy
                {
                    vsc.curSystem = new SystemClass(vsc.Name);

                    if (vsc.HasTravelCoordinates)
                    {
                        vsc.curSystem.x = vsc.X;
                        vsc.curSystem.y = vsc.Y;
                        vsc.curSystem.z = vsc.Z;
                    }
                }

                if (vsc.strDistance == null)
                    vsc.strDistance = "";                                       // set empty, must have a string in there.
            }

            DistanceClass.FillVisitedSystems(visitedSystems, usedistancedb);    // finally fill in the distances, indicating if can use db or not
        }

        public static void UpdateVisitedSystemsEntries(VisitedSystemsClass item, VisitedSystemsClass item2, bool usedistancedb)           // this is a split in two version with the same code of AddHistoryRow..
        {
            SystemClass sys1 = SystemClass.GetSystem(item.Name);
            if (sys1 == null)
            {
                sys1 = new SystemClass(item.Name);

                if (item.HasTravelCoordinates)
                {
                    sys1.x = item.X;
                    sys1.y = item.Y;
                    sys1.z = item.Z;
                }
            }

            SystemClass sys2 = null;

            if (item2 != null)
            {
                sys2 = SystemClass.GetSystem(item2.Name);
                if (sys2 == null)
                {
                    sys2 = new SystemClass(item2.Name);
                    if (item2.HasTravelCoordinates)
                    {
                        sys2.x = item2.X;
                        sys2.y = item2.Y;
                        sys2.z = item2.Z;
                    }
                }
            }
            else
                sys2 = null;

            item.curSystem = sys1;
            item.prevSystem = sys2;

            string diststr = "";
            if (sys2 != null)
            {
                double dist = usedistancedb ? SystemClass.DistanceIncludeDB(sys1, sys2) : SystemClass.Distance(sys1, sys2);
                if (dist > 0)
                    diststr = dist.ToString("0.00");
            }

            item.strDistance = diststr;
        }

        public static void SetLastKnownSystemPosition(List<VisitedSystemsClass> visitedSystems)     // go thru setting the lastknowsystem
        {
            ISystem lastknownps = null;
            foreach (VisitedSystemsClass ps in visitedSystems)
            {
                if (ps.curSystem != null && ps.curSystem.HasCoordinate)     // cursystem is always set.. see above
                {
                    ps.lastKnownSystem = lastknownps;
                    lastknownps = ps.curSystem;
                }
            }
        }

        public static SystemClass GetSystemClassFirstPosition(List<VisitedSystemsClass> visitedSystems)     // last pos system
        {
            if (visitedSystems != null)
            {
                IEnumerable<ISystem> slist = (from systems in visitedSystems orderby systems.Time descending select systems.curSystem);

                if (slist != null && slist.Any())
                {
                    ISystem sel = slist.First(s => s.HasCoordinate);

                    if (sel != null)
                        return (SystemClass)sel;
                }
            }

            return null;
        }

        public static double Distance(VisitedSystemsClass s1, double x, double y, double z)
        {
            if (s1 != null && s1.HasTravelCoordinates)
                return Math.Sqrt((s1.X - x) * (s1.X - x) + (s1.Y - y) * (s1.Y - y) + (s1.Z - z) * (s1.Z - z));
            else if (s1.curSystem.HasCoordinate)
                return Math.Sqrt((s1.curSystem.x - x) * (s1.curSystem.x - x) + (s1.curSystem.y - y) * (s1.curSystem.y - y) + (s1.curSystem.z - z) * (s1.curSystem.z - z));
            else
                return -1;
        }

        public static double Distance(VisitedSystemsClass s1, Point3D p)
        {
            return Distance(s1, p.X, p.Y, p.Z);
        }

        public static VisitedSystemsClass FindByPos(List<VisitedSystemsClass> visitedSystems, Point3D p, double limit)     // go thru setting the lastknowsystem
        {
            if (visitedSystems != null)
            {
                VisitedSystemsClass vs = visitedSystems.FindLast(x => x.curSystem.HasCoordinate &&
                                                Math.Abs(x.curSystem.x - p.X) < limit &&
                                                Math.Abs(x.curSystem.y - p.Y) < limit &&
                                                Math.Abs(x.curSystem.z - p.Z) < limit);
                return vs;
            }
            else
                return null;
        }

        public static VisitedSystemsClass FindByName(List<VisitedSystemsClass> visitedSystems, string name )    
        {
            if (visitedSystems != null)
            {
                VisitedSystemsClass vs = visitedSystems.FindLast(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                return vs;
            }
            else
                return null;
        }

        public static ISystem GetLatestSystem(List<VisitedSystemsClass> visitedSystems)
        {
            if (visitedSystems != null && visitedSystems.Count > 0)
                return (from systems in visitedSystems orderby systems.Time descending select systems.curSystem).First();
            else
                return null;
        }

        public static int GetVisitsCount(List<VisitedSystemsClass> visitedSystems, string name)
        {
            if (visitedSystems != null && visitedSystems.Count > 0)
                return (from row in visitedSystems where row.Name == name select row).Count();
            else
                return 0;
        }


        public  static ISystem FindSystem(List<VisitedSystemsClass> visitedSystems, GalacticMapping glist , string name)        // in system or name
        {
            EDDiscovery2.DB.ISystem ds1 = SystemClass.GetSystem(name);

            if (ds1 == null)
            {
                VisitedSystemsClass vs = VisitedSystemsClass.FindByName(visitedSystems, name);

                if (vs != null && vs.HasTravelCoordinates)
                    ds1 = vs.curSystem;
                else
                {
                    GalacticMapObject gmo = glist.Find(name, true, true);

                    if (gmo != null && gmo.points.Count > 0)
                    {
                        return new SystemClass(gmo.name, gmo.points[0].X, gmo.points[0].Y, gmo.points[0].Z);        // fudge it into a system
                    }
                }
            }

            return ds1;
        }


    }
}

