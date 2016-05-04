﻿
using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EDDiscovery2.DB
{
    public class TravelLogUnit
    {
        public int id;
        public string Name;
        public int type;
        public int Size;
        public string Path;


        public TravelLogUnit()
        {
        }

        public TravelLogUnit(DataRow dr)
        {
            id = (int)(long)dr["id"];
            Name = (string)dr["Name"];
            type = (int)(long)dr["type"];
            Size = (int)(long)dr["size"];
            Path = (string)dr["Path"];

        }

        public bool Beta
        {
            get
            {
                if (Path == null)
                    return false;

                if (Path.Contains("PUBLIC_TEST_SERVER"))
                    return true;
                else
                    return false;
            }
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
                cmd.CommandText = "Insert into TravelLogUnit (Name, type, size, Path) values (@name, @type, @size, @Path)";
                cmd.AddParameterWithValue("@name", Name);
                cmd.AddParameterWithValue("@type", type);
                cmd.AddParameterWithValue("@size", Size);
                cmd.AddParameterWithValue("@Path", Path);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "Select Max(id) as id from TravelLogUnit";

                    id = (int)(long)SQLiteDBClass.SqlScalar(cn, cmd2);
                }
                return true;
            }
        }

        public bool Update()
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
                cmd.CommandText = "Update TravelLogUnit set Name=@Name, Type=@type, size=@size, Path=@Path  where ID=@id";
                cmd.AddParameterWithValue("@ID", id);
                cmd.AddParameterWithValue("@Name", Name);
                cmd.AddParameterWithValue("@Type", type);
                cmd.AddParameterWithValue("@size", Size);
                cmd.AddParameterWithValue("@Path", Path);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                return true;
            }
        }


        static public List<TravelLogUnit> GetAll()
        {
            List<TravelLogUnit> list = new List<TravelLogUnit>();

            var db = new SQLiteDBClass();
            using (DbConnection cn = db.CreateConnection())
            {
                using (DbCommand cmd = cn.CreateCommand())
                {
                    DataSet ds = null;
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.CommandText = "select * from TravelLogUnit";

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
                        TravelLogUnit sys = new TravelLogUnit(dr);

                        list.Add(sys);
                    }

                    return list;
                }
            }
        }

    }

}

