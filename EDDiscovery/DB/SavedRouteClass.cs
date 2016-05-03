using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

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
            using (IDbConnection cn = SQLiteDBClass.CreateConnection())
            {
                return Add(cn);
            }
        }

        private bool Add(IDbConnection cn)
        {
            using (IDbCommand cmd = cn.CreateCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Insert into routes_expeditions (name, start, end) values (@name, @start, @end)";
                SQLiteDBClass.AddParameter(cmd, "@name", Name);
                SQLiteDBClass.AddParameter(cmd, "@start", StartDate);
                SQLiteDBClass.AddParameter(cmd, "@end", EndDate);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                using (IDbCommand cmd2 = cn.CreateCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "Select Max(id) as id from routes_expeditions";

                    Id = (int)(long)SQLiteDBClass.SqlScalar(cn, cmd2);
                }

                using (IDbCommand cmd2 = cn.CreateCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "INSERT INTO route_systems (routeid, systemname) VALUES (@routeid, @name)";
                    SQLiteDBClass.AddParameter(cmd2, "@routeid", DbType.String);
                    SQLiteDBClass.AddParameter(cmd2, "@name", DbType.String);

                    foreach (var sysname in Systems)
                    {
                        SQLiteDBClass.SetParameter(cmd2, "@routeid", Id);
                        SQLiteDBClass.SetParameter(cmd2, "@name", sysname);
                        SQLiteDBClass.SqlNonQueryText(cn, cmd2);
                    }
                }

                return true;
            }
        }

        public bool Update()
        {
            using (IDbConnection cn = SQLiteDBClass.CreateConnection())
            {
                return Update(cn);
            }
        }

	private bool Update(IDbConnection cn)
        {
            using (IDbCommand cmd = cn.CreateCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "UPDATE routes_expeditions SET name=@name, start=@start, end=@end WHERE id=@id";
                SQLiteDBClass.AddParameter(cmd, "@id", Id);
                SQLiteDBClass.AddParameter(cmd, "@name", Name);
                SQLiteDBClass.AddParameter(cmd, "@start", StartDate);
                SQLiteDBClass.AddParameter(cmd, "@end", EndDate);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                using (IDbCommand cmd2 = cn.CreateCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "DELETE FROM route_systems WHERE routeid=@routeid";
                    SQLiteDBClass.AddParameter(cmd2, "@routeid", Id);
                    SQLiteDBClass.SqlNonQueryText(cn, cmd2);
                }

                using (IDbCommand cmd2 = cn.CreateCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "INSERT INTO route_systems (routeid, systemname) VALUES (@routeid, @name)";
                    SQLiteDBClass.AddParameter(cmd2, "@routeid", DbType.String);
                    SQLiteDBClass.AddParameter(cmd2, "@name", DbType.String);

                    foreach (var sysname in Systems)
                    {
                        SQLiteDBClass.SetParameter(cmd2, "@routeid", Id);
                        SQLiteDBClass.SetParameter(cmd2, "@name", sysname);
                        SQLiteDBClass.SqlNonQueryText(cn, cmd2);
                    }
                }

                return true;
            }
        }
    }
}
