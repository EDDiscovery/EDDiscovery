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
using System.IO;
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
                this.StartDate = ((DateTime)dr["start"]);
            if (dr["end"] != DBNull.Value)
                this.EndDate = ((DateTime)dr["end"]);
               
            this.Systems = syslist.Select(s => (string)s["systemname"]).ToList();
            int statusbits = (int)dr["Status"];
            this.Deleted = (statusbits & 1) != 0;
            this.EDSM = (statusbits & 2) != 0;

            if ( this.Name.StartsWith("\x7F"))  // marker 7F meant pre 9.1 a EDSM system which was deleted.
            {
                this.Name = this.Name.Substring(1);
                Deleted = true;
            }
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }            // these are LOCAL TIMES since the expedition editor is using DateTime.Now
        public DateTime? EndDate { get; set; }
        public List<string> Systems { get; private set; }
        public bool EDSM { get; set; }          // supplied by EDSM
        public bool Deleted { get; set; }       // Deleted by us
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
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                bool ret = Add(cn);     // pass it an open connection since it does multiple SQLs
                return ret;
            }
        }

        private bool Add(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into routes_expeditions (name, start, end, Status) values (@name, @start, @end, @stat)"))
            {
                cmd.AddParameterWithValue("@name", Name);
                cmd.AddParameterWithValue("@start", StartDate);
                cmd.AddParameterWithValue("@end", EndDate);
                cmd.AddParameterWithValue("@stat", (Deleted ? 1 : 0) + (EDSM ? 2 : 0));

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
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                bool ret = Update(cn);
                return ret;
            }
        }

        private bool Update(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("UPDATE routes_expeditions SET name=@name, start=@start, end=@end, Status=@stat WHERE id=@id"))
            {
                cmd.AddParameterWithValue("@id", Id);
                cmd.AddParameterWithValue("@name", Name);
                cmd.AddParameterWithValue("@start", StartDate);
                cmd.AddParameterWithValue("@end", EndDate);
                cmd.AddParameterWithValue("@stat", (Deleted ? 1 : 0) + (EDSM ? 2 : 0));
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
                using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc:true, mode: EDDbAccessMode.Reader))
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

        // Given a set of expedition files, update the DB.  Add any new ones, and make sure the EDSM marker is on.
        // use a blank file to retire older ones

        public static bool UpdateDBFromExpeditionFiles(string expeditiondir )
        {
            bool changed = false;

            FileInfo[] allfiles = Directory.EnumerateFiles(expeditiondir, "*.json", SearchOption.TopDirectoryOnly).Select(f => new System.IO.FileInfo(f)).OrderByDescending(p => p.LastWriteTime).ToArray();

            foreach (FileInfo f in allfiles)        // iterate thru all files found
            {
                try
                {
                    string text = File.ReadAllText(f.FullName); // may except

                    if (text != null && text.Length>0)      // use a blank file to overwrite an old one you want to get rid of
                    {
                        var array = Newtonsoft.Json.Linq.JArray.Parse(text).ToObject<SavedRouteClass[]>();

                        List<SavedRouteClass> stored = SavedRouteClass.GetAllSavedRoutes(); // incl deleted

                        foreach (SavedRouteClass newentry in array)
                        {
                            if (newentry.StartDate.HasValue)
                                newentry.StartDate = newentry.StartDate.Value.ToLocalTime();      // supplied, and respected by JSON, as zulu time. the stupid database holds local times. Convert.
                            if (newentry.EndDate.HasValue)
                                newentry.EndDate = newentry.EndDate.Value.ToLocalTime();

                            SavedRouteClass storedentry = stored.Find(x => x.Name.Equals(newentry.Name));

                            if (newentry.Systems.Count == 0)              // no systems, means delete the database entry.. use with caution
                            {
                                if (storedentry != null)                // if we have it in the DB, delete it
                                {
                                    storedentry.Delete();
                                    changed = true;
                                }
                            }
                            else
                            {
                                newentry.EDSM = true;       // if we need to use newentry, then it must be marked as an EDSM one..

                                if (storedentry != null)  // if stored already..
                                {
                                    if (!storedentry.Systems.SequenceEqual(newentry.Systems)) // systems changed, we need to reset..
                                    {
                                        bool wasDel = storedentry.Deleted;
                                        storedentry.Delete();   // delete the old one.. systems may be in a different order, and there is no ordering except by ID in the DB
                                        newentry.Deleted = wasDel;
                                        newentry.Add();        // add to db..
                                        changed = changed || !wasDel;   // If it was marked deleted, don't report it as being changed.
                                    }
                                    else if (storedentry.EndDate != newentry.EndDate || storedentry.StartDate != newentry.StartDate)    // times change, just update
                                    {
                                        storedentry.StartDate = newentry.StartDate;             // update time and date but keep the expedition ID
                                        storedentry.EndDate = newentry.EndDate;
                                        storedentry.EDSM = true;
                                        storedentry.Update();
                                        changed = true;
                                    }
                                    else if (!storedentry.EDSM)    // backwards with previous system, set EDSM flag on these, prevents overwrite
                                    {
                                        storedentry.EDSM = true;    // ensure EDSM flag is set..
                                        storedentry.Update();
                                        changed = true;
                                    }
                                }
                                else
                                {                   // not there, add it..
                                    newentry.Add();        // add to db..
                                    changed = true;
                                }
                            }
                        }

                    }
                }
                catch { }       // bad file, try again
            }

            return changed;
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
