/*
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
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace EliteDangerousCore.DB
{
    [DebuggerDisplay("{DebugDisplay,nq}")]
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

        public string LastSystem { get { return Systems.Count > 0 ? Systems[Systems.Count - 1] : null; } }

        public bool Equals(SavedRouteClass other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.Name == other.Name || (String.IsNullOrEmpty(this.Name) && String.IsNullOrEmpty(other.Name))) &&
                   this.StartDate == other.StartDate &&
                   this.EndDate == other.EndDate &&
                   this.Systems.SequenceEqual(other.Systems);
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual string DebugDisplay
        {
            get
            {
                return $"\"{(Name ?? "(null)")}\" [{Id:n0}]: " + ((Systems == null || Systems.Count == 0) ? "no systems" : ((Systems?.Count == 1) ? "1 system" : (Systems.Count.ToString("n0") + " systems")));
            }
        }

        public double CumulativeDistance()
        {
            ISystem last = null;
            double distance = 0;

            for (int i = 0; i < Systems.Count; i++)
            {
                ISystem s = SystemClassDB.GetSystem(Systems[i]);
                if (s != null)
                {
                    if (last != null)
                        distance += s.Distance(last);

                    last = s;
                }
            }

            return distance;
        }

        public ISystem PosAlongRoute(double percentage)             // go along route and give me a co-ord along it..
        {
            double dist = CumulativeDistance() * percentage / 100.0;

            ISystem last = null;

            for (int i = 0; i < Systems.Count; i++)
            {
                ISystem s = SystemClassDB.GetSystem(Systems[i]);
                if (s != null)
                {
                    if (last != null)
                    {
                        double d = s.Distance(last);

                        if (dist < d)
                        {
                            d = dist / d;

                            return new SystemClass("WP" + (i).ToString() + "-" + "WP" + (i + 1).ToString() + "-" + d.ToString("#.00"), last.X + (s.X - last.X) * d, last.Y + (s.Y - last.Y) * d, last.Z + (s.Z - last.Z) * d);
                        }

                        dist -= d;
                    }

                    last = s;
                }
            }

            return last;
        }

        // given the system list, which is the next waypoint to go to.  return the system (or null if not available or past end) and the waypoint.. (0 based)

        public Tuple<ISystem, int> ClosestTo(ISystem sys)
        {
            double dist = Double.MaxValue;
            ISystem found = null;
            int closest = -1;

            List<ISystem> list = new List<ISystem>();

            for (int i = 0; i < Systems.Count; i++)
            {
                ISystem s = SystemClassDB.GetSystem(Systems[i]);

                if (s != null)
                {
                    double sd = s.Distance(sys);
                    if (sd < dist)
                    {
                        dist = sd;
                        closest = i;
                        found = s;
                    }
                }
                list.Add(s);
            }


            if (found != null)
            {
                //System.Diagnostics.Debug.WriteLine("Found at " + closest + " System " + found.name + " " + found.x + " " + found.y + " " + found.z + " dist " + dist);

                if (closest > 0)
                {
                    int lastentry = closest - 1;
                    while (lastentry >= 0 && list[lastentry] == null)       // go and find the last one which had a position..
                        lastentry--;

                    if (lastentry >= 0 && list[lastentry] != null)      // found it, so work out using distance if we are closest to last or past it
                    {
                        double distlasttoclosest = list[closest].Distance(list[lastentry]);     // last->closest vs
                        double distlasttocur = sys.Distance(list[lastentry]);                   // last->cur position

                        if (distlasttocur > distlasttoclosest - 0.1)   // past current because the distance last->cur > last->closest waypoint
                        {
                            return new Tuple<ISystem, int>(closest < Systems.Count - 1 ? list[closest + 1] : null, closest + 1); // en-route to this. may be null
                        }
                        else
                            return new Tuple<ISystem, int>(found, closest); // en-route to this
                    }
                }

                return new Tuple<ISystem, int>(found, closest);
            }
            else
                return null;
        }

        public void TestHarness()       // fly the route and debug the closestto.. keep this for testing
        {
            for (double percent = 0; percent < 110; percent += 1)
            {
                ISystem cursys = PosAlongRoute(percent);
                System.Diagnostics.Debug.WriteLine(Environment.NewLine + "Sys {0} {1} {2} {3}", cursys.X, cursys.Y, cursys.Z, cursys.Name);

                Tuple<ISystem, int> closest = ClosestTo(cursys);

                if (closest != null)
                {
                    System.Diagnostics.Debug.WriteLine("Next {0} {1} {2} {3}, index {4}", closest.Item1?.X, closest.Item1?.Y, closest.Item1?.Z, closest.Item1?.Name, closest.Item2);
                }
            }
        }
    }
}
