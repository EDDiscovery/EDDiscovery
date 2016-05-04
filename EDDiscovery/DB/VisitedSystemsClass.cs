using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EDDiscovery2.DB
{
    public class VisitedSystemsClass : InMemory.VisitedSystemsClass
    {
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
        }


        public bool Add()
        {
            var db = new SQLiteDBClass();
            using (DbConnection cn = db.CreateConnection())
            {
                return Add(cn);
            }
        }

        private bool Add(DbConnection cn)
        {
            using (DbCommand cmd = cn.CreateCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Insert into VisitedSystems (Name, Time, Unit, Commander, Source, edsm_sync, map_colour) values (@name, @time, @unit, @commander, @source, @edsm_sync, @map_colour)";
                cmd.AddParameterWithValue("@name", Name);
                cmd.AddParameterWithValue("@time", Time);
                cmd.AddParameterWithValue("@unit", Unit);
                cmd.AddParameterWithValue("@commander", Commander);
                cmd.AddParameterWithValue("@source", Source);
                cmd.AddParameterWithValue("@edsm_sync", EDSM_sync);
                cmd.AddParameterWithValue("@map_colour", MapColour);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand())
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
            var db = new SQLiteDBClass();
            using (DbConnection cn = db.CreateConnection())
            {
                return Update(cn);
            }
        }

        private bool Update(DbConnection cn)
        {
            using (DbCommand cmd = cn.CreateCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Update VisitedSystems set Name=@Name, Time=@Time, Unit=@Unit, Commander=@commander, Source=@Source, edsm_sync=@edsm_sync, map_colour=@map_colour  where ID=@id";
                cmd.AddParameterWithValue("@ID", id);
                cmd.AddParameterWithValue("@Name", Name);
                cmd.AddParameterWithValue("@Time", Time);
                cmd.AddParameterWithValue("@unit", Unit);
                cmd.AddParameterWithValue("@commander", Commander);
                cmd.AddParameterWithValue("@source", Source);
                cmd.AddParameterWithValue("@edsm_sync", EDSM_sync);
                cmd.AddParameterWithValue("@map_colour", MapColour);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                return true;
            }
        }



        static public List<VisitedSystemsClass> GetAll()
        {
            List<VisitedSystemsClass> list = new List<VisitedSystemsClass>();

            var db = new SQLiteDBClass();
            using (DbConnection cn = db.CreateConnection())
            {
                using (DbCommand cmd = cn.CreateCommand())
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

            var db = new SQLiteDBClass();
            using (DbConnection cn = db.CreateConnection())
            {
                using (DbCommand cmd = cn.CreateCommand())
                {
                    DataSet ds = null;
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.CommandText = "select * from VisitedSystems where commander=@commander Order by Time ";
                    cmd.AddParameterWithValue("@commander", commander);

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

            var db = new SQLiteDBClass();
            using (DbConnection cn = db.CreateConnection())
            {
                using (DbCommand cmd = cn.CreateCommand())
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

            var db = new SQLiteDBClass();
            using (DbConnection cn = db.CreateConnection())
            {
                using (DbCommand cmd = cn.CreateCommand())
                {
                    DataSet ds = null;
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.CommandText = "select * from VisitedSystems where name=@name and Time=@time  Order by Time DESC Limit 1";
                    cmd.AddParameterWithValue("@name", name);
                    cmd.AddParameterWithValue("@time", time);
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

