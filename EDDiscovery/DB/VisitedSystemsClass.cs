using EDDiscovery.DB;
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
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
            {
                bool ret = Add(cn);
                return ret;
            }
        }

        private bool Add(SQLiteConnectionED cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into VisitedSystems (Name, Time, Unit, Commander, Source, edsm_sync, map_colour, X, Y, Z) values (@name, @time, @unit, @commander, @source, @edsm_sync, @map_colour, @x, @y, @z)"))
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
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnectionED cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Update VisitedSystems set Name=@Name, Time=@Time, Unit=@Unit, Commander=@commander, Source=@Source, edsm_sync=@edsm_sync, map_colour=@map_colour, X=@x, Y=@y, Z=@z  where ID=@id"))
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
                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                return true;
            }
        }



        static public VisitedSystemsClass Parse(DateTime lasttime, string line)
        {
            VisitedSystemsClass sp = new VisitedSystemsClass();

            try
            {
                Regex pattern;
                int hour=0, min=0, sec=0;

                /* MKW: Use regular expressions to parse the log; much more readable and robust.
                 * Example log entry:
                
                From  ED  2.1 /1.6
                   {19:21:15} System:"Ooscs Fraae JR-L d8-112" StarPos:(-11609.469,639.594,20141.875)ly  NormalFlight
                string rgexpstr = "{(?<Hour>\\d+):(?<Minute>\\d+):(?<Second>\\d+)} System:\"(?<SystemName>[^\"]+)\" StarPos:\\((?<Pos>.*?)\\)ly( +(?<TravelMode>\\w+))?";

                new from beta3?
                {18:15:14} System:"Pleiades Sector HR-W d1-41" StarPos:(-83.969,-146.156,-334.219)ly Body:0 RelPos:(-1.19887e+07,-9.95573e+06,2.55124e+06)km Supercruise
                string rgexpstr = "{(?<Hour>\\d+):(?<Minute>\\d+):(?<Second>\\d+)} System:\"(?<SystemName>[^\"]+)\" StarPos:\\((?<Pos>.*?)\\)ly Body:(?<Body>\d+) StarPos:\\((?<Pos>.*?)\\)ly( +(?<TravelMode>\\w+))?";


                Pre ED 2.1/1.6
                    {09:36:16} System:0(Thuechea JE-O b11-0) Body:1 Pos:(-6.67432e+009,7.3151e+009,-1.19125e+010) Supercruise
                 
                 * Also, please note that due to E:D bugs, these entries can be at the end of a line as well, not just on a line of their own.
                 * The RegExp below actually just finds the pattern somewhere in the line, so it caters for rubbish at the end too.
                 */

                if (line.Contains("StarPos:")) // new  ED 2.1 format
                {

                    //{(?<Hour>\d+):(?<Minute>\d+):(?<Second>\d+)} System:"(?<SystemName>[^"]+)" StarPos:\((?<Pos>.*?)\)ly( +(?<TravelMode>\w+))?
                    //{(?<Hour>\d+):(?<Minute>\d+):(?<Second>\d+)} System:"(?<SystemName>[^"]+)" StarPos:\((?<Pos>.*?)\)ly( +(?<TravelMode>\w+))?
                    //string rgexpstr = "{(?<Hour>\\d+):(?<Minute>\\d+):(?<Second>\\d+)} System:\"(?<SystemName>[^\"]+)\" StarPos:\\((?<Pos>.*?)\\)ly( +(?<TravelMode>\\w+))?";
                    string rgexpstr;

                    if (line.Contains("Body:"))
                        rgexpstr = "{(?<Hour>\\d+):(?<Minute>\\d+):(?<Second>\\d+)} System:\"(?<SystemName>[^\"]+)\" StarPos:\\((?<Pos>.*?)\\)ly Body:(?<Body>\\d+) RelPos:\\((?<RelPos>.*?)\\)km( +(?<TravelMode>\\w+))?";
                    else
                        rgexpstr = "{(?<Hour>\\d+):(?<Minute>\\d+):(?<Second>\\d+)} System:\"(?<SystemName>[^\"]+)\" StarPos:\\((?<Pos>.*?)\\)ly( +(?<TravelMode>\\w+))?";

                    pattern = new Regex(rgexpstr);


                    Match match = pattern.Match(line);

                    if (match != null && match.Success)
                    {
                        hour = int.Parse(match.Groups["Hour"].Value);
                        min = int.Parse(match.Groups["Minute"].Value);
                        sec = int.Parse(match.Groups["Second"].Value);

                        //sp.Nr = int.Parse(match.Groups["Body"].Value);
                        sp.Name = match.Groups["SystemName"].Value;
                        string pos = match.Groups["Pos"].Value;
                        try
                        {
                            string[] xyzpos = pos.Split(',');
                            var culture = new System.Globalization.CultureInfo("en-US");
                            sp.X = double.Parse(xyzpos[0], culture);
                            sp.Y = double.Parse(xyzpos[1], culture);
                            sp.Z = double.Parse(xyzpos[2], culture);
                        }
                        catch
                        {
                            sp.X = double.NaN;
                            sp.Y = double.NaN;
                            sp.Z = double.NaN;
                        }

                    }
                    else
                    {
                        System.Diagnostics.Trace.WriteLine("System parse error 1:" + line);
                    }
                    
                }
                else
                {
                    pattern = new Regex(@"{(?<Hour>\d+):(?<Minute>\d+):(?<Second>\d+)} System:\d+\((?<SystemName>.*?)\) Body:(?<Body>\d+) Pos:\(.*?\)( (?<TravelMode>\w+))?");
                    Match match = pattern.Match(line);

                    if (match != null && match.Success)
                    {
                        hour = int.Parse(match.Groups["Hour"].Value);
                        min = int.Parse(match.Groups["Minute"].Value);
                        sec = int.Parse(match.Groups["Second"].Value);

                        //sp.Nr = int.Parse(match.Groups["Body"].Value);
                        sp.Name = match.Groups["SystemName"].Value;
                        sp.X = Double.NaN;
                        sp.Y = Double.NaN;
                        sp.Z = Double.NaN;
                    }
                    else
                    {
                        System.Diagnostics.Trace.WriteLine("System parse error 2:" + line);
                    }
                }
                if (hour >= lasttime.Hour)
                {
                    sp.Time = new DateTime(lasttime.Year, lasttime.Month, lasttime.Day, hour, min, sec);
                }
                else
                {
                    DateTime tomorrow = lasttime.AddDays(1);
                    sp.Time = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, hour, min, sec);
                }

                if (sp.Time.Subtract(lasttime).TotalHours < -4)
                {
                    sp.Time = sp.Time.AddDays(1);
                }
                return sp;
            }
            catch
            {
                // MKW TODO: should we log bad lines?
                return null;
            }
        }



        static public List<VisitedSystemsClass> GetAll()
        {
            List<VisitedSystemsClass> list = new List<VisitedSystemsClass>();

            using (SQLiteConnectionED cn = new SQLiteConnectionED())
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

            using (SQLiteConnectionED cn = new SQLiteConnectionED())
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

        static public VisitedSystemsClass GetLast()
        {
            List<VisitedSystemsClass> list = new List<VisitedSystemsClass>();

            using (SQLiteConnectionED cn = new SQLiteConnectionED())
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

        internal static bool Exist(string name, DateTime time)
        {
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
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

        public static void CalculateSqDistances(List<VisitedSystemsClass> vs, SortedList<double,string> distlist , double x, double y, double z, int maxitems , bool removezerodiststar )
        {
            double dist;
            double dx, dy, dz;
            foreach (VisitedSystemsClass pos in vs)
            {
                if (pos.HasTravelCoordinates && distlist.IndexOfValue(pos.Name) == -1)   // if co-ords, and not in list already..
                {
                    dx = (pos.X - x);
                    dy = (pos.Y - y);
                    dz = (pos.Z - z);
                    dist = dx * dx + dy * dy + dz * dz;

                    if (dist > 0.001 || !removezerodiststar)
                    {
                        if (distlist.Count < maxitems)          // if less than max, add..
                            distlist.Add(dist, pos.Name);
                        else if (dist < distlist.Last().Key)   // if last entry (which must be the biggest) is greater than dist..
                        {
                            distlist.Add(dist, pos.Name);           // add in
                            distlist.RemoveAt(maxitems);        // remove last..
                        }
                    }
                }
            }
        }
        
        // centresysname is a default one

        public static string FindNextVisitedSystem(List<VisitedSystemsClass> _visitedSystems, string sysname, int dir , string centresysname )
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

        public static void UpdateSys(List<VisitedSystemsClass> visitedSystems, bool usedistancedb)          // oldest system is lowest index
        {
            SystemClass.FillVisitedSystems(visitedSystems);                 // first try and populate with SystemClass info
            
            foreach (VisitedSystemsClass vsc in visitedSystems)
            {
                if (vsc.curSystem == null)                                  // if no systemclass info, make a dummy
                {
                    vsc.curSystem = new SystemClass(vsc.Name);
//TBD vsc.HasTravelCoordinates
                    if (vsc.HasTravelCoordinates)
                    {
                        vsc.curSystem.x = vsc.X;
                        vsc.curSystem.y = vsc.Y;
                        vsc.curSystem.z = vsc.Z;
                    }
                }

                vsc.strDistance = "";                                       // set empty, must have a string in there.
            }

            DistanceClass.FillVisitedSystems(visitedSystems, usedistancedb);    // finally fill in the distances, indicating if can use db or not
        }

        public static void UpdateVisitedSystemsEntries(VisitedSystemsClass item, VisitedSystemsClass item2 , bool usedistancedb)           // this is a split in two version with the same code of AddHistoryRow..
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
                double dist = usedistancedb ? SystemClass.DistanceIncludeDB(sys1,sys2) : SystemClass.Distance(sys1, sys2);
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
    }
}

