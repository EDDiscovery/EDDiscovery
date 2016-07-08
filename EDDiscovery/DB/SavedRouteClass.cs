using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace EDDiscovery.DB
{
    public class SavedRouteClass
    {
        public SavedRouteClass()
        {
            this.Id = -1;
            this.Systems = new List<string>();
        }

        public SavedRouteClass(string name, params string[] systems)
        {
            this.Id = -1;
            this.Name = name;
            this.Systems = systems.ToList();
        }

        public SavedRouteClass(DataRow dr, DataRow[] syslist)
        {
            this.Id = (int)(long)dr["id"];
            this.Name = (string)dr["name"];
            if (dr["start"] != DBNull.Value)
                this.StartDate = (DateTime?)dr["start"];
            if (dr["end"] != DBNull.Value)
                this.EndDate = (DateTime?)dr["end"];
            this.Systems = syslist.Select(s => (string)s["systemname"]).ToList();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string> Systems { get; private set; }

        public bool Equals(SavedRouteClass other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.Name == other.Name || (String.IsNullOrEmpty(this.Name) && String.IsNullOrEmpty(other.Name))) &&
                   this.StartDate == other.StartDate &&
                   this.EndDate == other.EndDate &&
                   this.Systems.Count == other.Systems.Count &&
                   this.Systems.Zip(other.Systems, (t, o) => t == o).All(v => v);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is SavedRouteClass)
            {
                return this.Equals((SavedRouteClass)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
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
                cmd.CommandText = "Insert into routes_expeditions (name, start, end) values (@name, @start, @end)";
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@start", StartDate);
                cmd.Parameters.AddWithValue("@end", EndDate);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                using (SQLiteCommand cmd2 = new SQLiteCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "Select Max(id) as id from routes_expeditions";

                    Id = (int)(long)SQLiteDBClass.SqlScalar(cn, cmd2);
                }

                using (SQLiteCommand cmd2 = new SQLiteCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "INSERT INTO route_systems (routeid, systemname) VALUES (@routeid, @name)";
                    cmd2.Parameters.Add("@routeid", DbType.String);
                    cmd2.Parameters.Add("@name", DbType.String);

                    foreach (var sysname in Systems)
                    {
                        cmd2.Parameters["@routeid"].Value = Id;
                        cmd2.Parameters["@name"].Value = sysname;
                        SQLiteDBClass.SqlNonQueryText(cn, cmd2);
                    }
                }

                return true;
            }
        }

        public bool Update()
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
                cmd.CommandText = "UPDATE routes_expeditions SET name=@name, start=@start, end=@end WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@start", StartDate);
                cmd.Parameters.AddWithValue("@end", EndDate);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                using (SQLiteCommand cmd2 = new SQLiteCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "DELETE FROM route_systems WHERE routeid=@routeid";
                    cmd2.Parameters.AddWithValue("@routeid", Id);
                    SQLiteDBClass.SqlNonQueryText(cn, cmd2);
                }

                using (SQLiteCommand cmd2 = new SQLiteCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "INSERT INTO route_systems (routeid, systemname) VALUES (@routeid, @name)";
                    cmd2.Parameters.Add("@routeid", DbType.String);
                    cmd2.Parameters.Add("@name", DbType.String);

                    foreach (var sysname in Systems)
                    {
                        cmd2.Parameters["@routeid"].Value = Id;
                        cmd2.Parameters["@name"].Value = sysname;
                        SQLiteDBClass.SqlNonQueryText(cn, cmd2);
                    }
                }

                return true;
            }
        }


        public static List<SavedRouteClass> GetAllSavedRoutes()
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    using (SQLiteCommand cmd1 = new SQLiteCommand())
                    {
                        DataSet ds1 = null;
                        cmd1.Connection = cn;
                        cmd1.CommandType = CommandType.Text;
                        cmd1.CommandTimeout = 30;
                        cmd1.CommandText = "select * from routes_expeditions";

                        ds1 = SQLiteDBClass.SqlQueryText(cn, cmd1);
                        if (ds1.Tables.Count == 0)
                        {
                            return null;
                        }
                        //
                        if (ds1.Tables[0].Rows.Count == 0)
                        {
                            return new List<SavedRouteClass>();
                        }

                        using (SQLiteCommand cmd2 = new SQLiteCommand())
                        {
                            DataSet ds2 = null;
                            cmd2.Connection = cn;
                            cmd2.CommandType = CommandType.Text;
                            cmd2.CommandTimeout = 30;
                            cmd2.CommandText = "select * from route_systems";

                            ds2 = SQLiteDBClass.SqlQueryText(cn, cmd2);

                            List<SavedRouteClass> retVal = new List<SavedRouteClass>();

                            foreach (DataRow dr in ds1.Tables[0].Rows)
                            {
                                DataRow[] syslist = new DataRow[0];
                                if (ds2.Tables.Count != 0)
                                {
                                    syslist = ds2.Tables[0].Select(String.Format("routeid = {0}", dr["id"]), "id ASC");
                                }
                                SavedRouteClass sys = new SavedRouteClass(dr, syslist);
                                retVal.Add(sys);
                            }

                            return retVal;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
