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
            this.Id = (long)dr["id"];
            this.Name = (string)dr["name"];
            if (dr["start"] != DBNull.Value)
                this.StartDate = (DateTime?)dr["start"];
            if (dr["end"] != DBNull.Value)
                this.EndDate = (DateTime?)dr["end"];
            this.Systems = syslist.Select(s => (string)s["systemname"]).ToList();
        }

        public long Id { get; set; }
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
            using (SQLiteConnection cn = SQLiteDBClass.CreateConnection(true))
            {
                bool ret = Add(cn);     // pass it an open connection since it does multiple SQLs
                cn.Close();
                return ret;
            }
        }

        private bool Add(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("Insert into routes_expeditions (name, start, end) values (@name, @start, @end)",cn))
            {
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@start", StartDate);
                cmd.Parameters.AddWithValue("@end", EndDate);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (SQLiteCommand cmd2 = SQLiteDBClass.CreateCommand("Select Max(id) as id from routes_expeditions",cn))
                {
                    Id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }

                using (SQLiteCommand cmd2 = SQLiteDBClass.CreateCommand("INSERT INTO route_systems (routeid, systemname) VALUES (@routeid, @name)",cn))
                {
                    cmd2.Parameters.Add("@routeid", DbType.String);
                    cmd2.Parameters.Add("@name", DbType.String);

                    foreach (var sysname in Systems)
                    {
                        cmd2.Parameters["@routeid"].Value = Id;
                        cmd2.Parameters["@name"].Value = sysname;
                        SQLiteDBClass.SQLNonQueryText(cn, cmd2);
                    }
                }

                return true;
            }
        }

        public bool Update()
        {
            using (SQLiteConnection cn = SQLiteDBClass.CreateConnection(true))
            {
                bool ret = Update(cn);
                cn.Close();
                return ret;
            }
        }

        private bool Update(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("UPDATE routes_expeditions SET name=@name, start=@start, end=@end WHERE id=@id",cn))
            {
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@start", StartDate);
                cmd.Parameters.AddWithValue("@end", EndDate);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (SQLiteCommand cmd2 = SQLiteDBClass.CreateCommand("DELETE FROM route_systems WHERE routeid=@routeid",cn))
                {
                    cmd2.Parameters.AddWithValue("@routeid", Id);
                    SQLiteDBClass.SQLNonQueryText(cn, cmd2);
                }

                using (SQLiteCommand cmd2 = SQLiteDBClass.CreateCommand("INSERT INTO route_systems (routeid, systemname) VALUES (@routeid, @name)",cn))
                {
                    cmd2.Parameters.Add("@routeid", DbType.String);
                    cmd2.Parameters.Add("@name", DbType.String);

                    foreach (var sysname in Systems)
                    {
                        cmd2.Parameters["@routeid"].Value = Id;
                        cmd2.Parameters["@name"].Value = sysname;
                        SQLiteDBClass.SQLNonQueryText(cn, cmd2);
                    }
                }

                return true;
            }
        }


        public static List<SavedRouteClass> GetAllSavedRoutes()
        {
            List<SavedRouteClass> retVal = new List<SavedRouteClass>();

            try
            {
                using (SQLiteConnection cn = SQLiteDBClass.CreateConnection(true))
                {
                    using (SQLiteCommand cmd1 = SQLiteDBClass.CreateCommand("select * from routes_expeditions",cn))
                    {
                        DataSet ds1 = SQLiteDBClass.SQLQueryText(cn, cmd1);

                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            using (SQLiteCommand cmd2 = SQLiteDBClass.CreateCommand("select * from route_systems", cn))
                            {
                                DataSet ds2 = SQLiteDBClass.SQLQueryText(cn, cmd2);

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

                            }
                        }

                        cn.Close();
                    }
                }
            }
            catch
            {
            }

            return retVal;
        }
    }
}
