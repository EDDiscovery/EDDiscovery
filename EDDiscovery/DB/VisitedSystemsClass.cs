using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
        }

        public VisitedSystemsClass(DataRow dr)
        {
            id = (int)(long)dr["id"];
            Name = (string)dr["Name"];
            Time = (DateTime)dr["Time"];
            Commander = (int)(long)dr["Commander"];
            Source = (int)(long)dr["Source"];
            Unit = (string)dr["Unit"];
            EDSM_sync = (bool)dr["edsm_sync"];
            MapColour = (int)(long)dr["Map_colour"];

            if (System.DBNull.Value == dr["X"])
            {
                X = 0;
                Y = 0;
                Z = 0;
            }
            else
            {
                X = (double)dr["X"];
                Y = (double)dr["Y"];
                Z = (double)dr["Z"];
            }
        }

        public bool Add()
        {
            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                return Add(cn);
            }
        }

        private bool Add(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Insert into VisitedSystems (Name, Time, Unit, Commander, Source, edsm_sync, map_colour, X, Y, Z) values (@name, @time, @unit, @commander, @source, @edsm_sync, @map_colour, @x, @y, @z)";
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@time", Time);
                cmd.Parameters.AddWithValue("@unit", Unit);
                cmd.Parameters.AddWithValue("@commander", Commander);
                cmd.Parameters.AddWithValue("@source", Source);
                cmd.Parameters.AddWithValue("@edsm_sync", EDSM_sync);
                cmd.Parameters.AddWithValue("@map_colour", MapColour);
                cmd.Parameters.AddWithValue("@x", X);
                cmd.Parameters.AddWithValue("@y", Y);
                cmd.Parameters.AddWithValue("@z", Z);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                using (SQLiteCommand cmd2 = new SQLiteCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "Select Max(id) as id from VisitedSystems";

                    id = (int)(long)SQLiteDBClass.SqlScalar(cn, cmd2);
                }
                return true;
            }
        }

        public new bool Update()
        {
            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Update VisitedSystems set Name=@Name, Time=@Time, Unit=@Unit, Commander=@commander, Source=@Source, edsm_sync=@edsm_sync, map_colour=@map_colour, X=@x, Y=@y, Z=@z  where ID=@id";
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Time", Time);
                cmd.Parameters.AddWithValue("@unit", Unit);
                cmd.Parameters.AddWithValue("@commander", Commander);
                cmd.Parameters.AddWithValue("@source", Source);
                cmd.Parameters.AddWithValue("@edsm_sync", EDSM_sync);
                cmd.Parameters.AddWithValue("@map_colour", MapColour);
                cmd.Parameters.AddWithValue("@x", X);
                cmd.Parameters.AddWithValue("@y", Y);
                cmd.Parameters.AddWithValue("@z", Z);
                SQLiteDBClass.SqlNonQueryText(cn, cmd);

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
                            sp.X = 0;
                            sp.Y = 0;
                            sp.Z = 0;
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


            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    DataSet ds = null;
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.CommandText = "select * from VisitedSystems Order by Time ";

                    ds = SQLiteDBClass.QueryText(cn, cmd);
                    if (ds.Tables.Count == 0)
                    {
                        return null;
                    }
                    //
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        return list;
                    }

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


            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    DataSet ds = null;
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.CommandText = "select * from VisitedSystems where commander=@commander Order by Time ";
                    cmd.Parameters.AddWithValue("@commander", commander);

                    ds = SQLiteDBClass.QueryText(cn, cmd);
                    if (ds.Tables.Count == 0)
                    {
                        return null;
                    }
                    //
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        return list;
                    }

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


            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    DataSet ds = null;
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.CommandText = "select * from VisitedSystems Order by Time DESC Limit 1";


                    ds = SQLiteDBClass.QueryText(cn, cmd);
                    if (ds.Tables.Count == 0)
                    {
                        return null;
                    }
                    //
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        return null;
                    }

                    VisitedSystemsClass sys = new VisitedSystemsClass(ds.Tables[0].Rows[0]);

                    return sys;
                }
            }
        }

        internal static bool  Exist(string name, DateTime time)
        {
            List<VisitedSystemsClass> list = new List<VisitedSystemsClass>();


            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    DataSet ds = null;
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.CommandText = "select * from VisitedSystems where name=@name and Time=@time  Order by Time DESC Limit 1";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@time", time);
                    ds = SQLiteDBClass.QueryText(cn, cmd);
                    if (ds.Tables.Count == 0)
                    {
                        return false;
                    }
                    //
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        return false;
                    }

                    //VisitedSystemsClass sys = new VisitedSystemsClass(ds.Tables[0].Rows[0]);

                    return true;
                }
            }
        }


    }

}

