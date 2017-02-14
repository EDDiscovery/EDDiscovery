﻿/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace EDDiscovery.DB
{
    public class SavedRouteClass : IEquatable<SavedRouteClass>
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
            return (Name == null) ? 0 : Name.GetHashCode();
        }

        public bool Add()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                bool ret = Add(cn);     // pass it an open connection since it does multiple SQLs
                return ret;
            }
        }

        private bool Add(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into routes_expeditions (name, start, end) values (@name, @start, @end)"))
            {
                cmd.AddParameterWithValue("@name", Name);
                cmd.AddParameterWithValue("@start", StartDate);
                cmd.AddParameterWithValue("@end", EndDate);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from routes_expeditions"))
                {
                    Id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }

                using (DbCommand cmd2 = cn.CreateCommand("INSERT INTO route_systems (routeid, systemname) VALUES (@routeid, @name)"))
                {
                    cmd2.AddParameter("@routeid", DbType.String);
                    cmd2.AddParameter("@name", DbType.String);

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
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                bool ret = Update(cn);
                return ret;
            }
        }

        private bool Update(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("UPDATE routes_expeditions SET name=@name, start=@start, end=@end WHERE id=@id"))
            {
                cmd.AddParameterWithValue("@id", Id);
                cmd.AddParameterWithValue("@name", Name);
                cmd.AddParameterWithValue("@start", StartDate);
                cmd.AddParameterWithValue("@end", EndDate);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand("DELETE FROM route_systems WHERE routeid=@routeid"))
                {
                    cmd2.AddParameterWithValue("@routeid", Id);
                    SQLiteDBClass.SQLNonQueryText(cn, cmd2);
                }

                using (DbCommand cmd2 = cn.CreateCommand("INSERT INTO route_systems (routeid, systemname) VALUES (@routeid, @name)"))
                {
                    cmd2.AddParameter("@routeid", DbType.String);
                    cmd2.AddParameter("@name", DbType.String);

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

        public bool Delete()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return Delete(cn);
            }
        }

        public bool Delete(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("DELETE FROM routes_expeditions WHERE id=@id"))
            {
                cmd.AddParameterWithValue("@id", Id);
                cmd.ExecuteNonQuery();
            }

            using (DbCommand cmd = cn.CreateCommand("DELETE FROM route_systems WHERE routeid=@routeid"))
            {
                cmd.AddParameterWithValue("@routeid", Id);
                cmd.ExecuteNonQuery();
            }

            return true;
        }


        public static List<SavedRouteClass> GetAllSavedRoutes()
        {
            List<SavedRouteClass> retVal = new List<SavedRouteClass>();

            try
            {
                using (SQLiteConnectionUser cn = new SQLiteConnectionUser(mode: EDDbAccessMode.Reader))
                {
                    using (DbCommand cmd1 = cn.CreateCommand("select * from routes_expeditions"))
                    {
                        DataSet ds1 = SQLiteDBClass.SQLQueryText(cn, cmd1);

                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            using (DbCommand cmd2 = cn.CreateCommand("select * from route_systems"))
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
