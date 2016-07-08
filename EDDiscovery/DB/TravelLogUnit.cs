
using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace EDDiscovery2.DB
{
    public class TravelLogUnit
    {
        public long id;
        public string Name;
        public int type;
        public int Size;
        public string Path;


        public TravelLogUnit()
        {
        }

        public TravelLogUnit(DataRow dr)
        {
            id = (long)dr["id"];
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
            using (SQLiteConnection cn = SQLiteDBClass.CreateConnection(true))
            {
                bool ret = Add(cn);
                cn.Close();
                return ret;
            }
        }

        private bool Add(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("Insert into TravelLogUnit (Name, type, size, Path) values (@name, @type, @size, @Path)",cn))
            {
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@size", Size);
                cmd.Parameters.AddWithValue("@Path", Path);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (SQLiteCommand cmd2 = SQLiteDBClass.CreateCommand("Select Max(id) as id from TravelLogUnit",cn))
                {
                    id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }

                return true;
            }
        }

        public bool Update()
        {
            using (SQLiteConnection cn = SQLiteDBClass.CreateConnection())
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("Update TravelLogUnit set Name=@Name, Type=@type, size=@size, Path=@Path  where ID=@id",cn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@size", Size);
                cmd.Parameters.AddWithValue("@Path", Path);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                return true;
            }
        }
        
        static public List<TravelLogUnit> GetAll()
        {
            List<TravelLogUnit> list = new List<TravelLogUnit>();

            using (SQLiteConnection cn = SQLiteDBClass.CreateConnection())
            {
                using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("select * from TravelLogUnit",cn))
                {
                    DataSet ds = SQLiteDBClass.SQLQueryText(cn, cmd);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return list;

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

