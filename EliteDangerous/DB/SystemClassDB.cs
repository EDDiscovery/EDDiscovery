﻿/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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

using EMK.LightGeometry;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace EliteDangerousCore.DB
{
    public static class SystemClassDB
    {
        public const float XYZScalar = 128.0F;     // scaling between DB stored values and floats

        public static ISystem FromEDDB(JObject jo)
        {
            ISystem sys = new SystemClass();

            try
            {
                sys.Name = jo["name"].Value<string>();

                sys.X = jo["x"].Value<double>();
                sys.Y = jo["y"].Value<double>();
                sys.Z = jo["z"].Value<double>();

                sys.EDDBID = jo["id"].Value<int>();

                sys.Faction = jo["controlling_minor_faction"].Value<string>();

                if (jo["population"].Type == JTokenType.Integer)
                    sys.Population = jo["population"].Value<long>();

                sys.Government = EliteDangerousTypesFromJSON.Government2ID(jo["government"]);
                sys.Allegiance = EliteDangerousTypesFromJSON.Allegiance2ID(jo["allegiance"]);

                sys.State = EliteDangerousTypesFromJSON.EDState2ID(jo["state"]);
                sys.Security = EliteDangerousTypesFromJSON.EDSecurity2ID(jo["security"]);

                sys.PrimaryEconomy = EliteDangerousTypesFromJSON.EDEconomy2ID(jo["primary_economy"]);

                if (jo["needs_permit"].Type == JTokenType.Integer)
                    sys.NeedsPermit = jo["needs_permit"].Value<int>();

                sys.EDDBUpdatedAt = jo["updated_at"].Value<int>();

                sys.EDSMID = jo["edsm_id"].Long();                         // pick up its edsm ID

                sys.status = SystemStatusEnum.EDDB;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("SystemClass exception: " + ex.Message);
            }           // since we don't have control of outside formats, we fail quietly.

            return sys;
        }

        public static void RemoveGridSystems(List<int> gridids, Action<string> Report)
        {
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
            {
                Report("Delete System Information from sector:");
                for (int i = 0; i < gridids.Count; i += 32)
                {
                    int left = Math.Min(gridids.Count - i, 32);     // could do it all at once, but this way, some visual feedback
                    List<int> todo = gridids.GetRange(i, left);

                    Report(" " + string.Join(" ", todo));

                    using (DbCommand cmd1 = cn.CreateCommand("DELETE FROM EdsmSystems Where gridid IN (" + string.Join(",", todo) + ")"))
                    {
                        cmd1.ExecuteNonQuery();
                    }
                }

                Report(Environment.NewLine);
            }
        }

        public static void RemoveGridNames(List<int> gridids, Action<string> Report)
        {
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
            {
                Report("Delete System Names from sector:");

                foreach (int grid in gridids)
                {
                    Report(" " + grid);
                    using (DbCommand cmd2 = cn.CreateCommand("DELETE FROM SystemNames Where EdsmId IN (Select EdsmId From EDSMSystems where GridId=" + grid.ToStringInvariant() + ")"))
                    {
                        cmd2.ExecuteNonQuery();
                    }
                }
                Report(Environment.NewLine);
            }
        }


        public static void Vacuum()
        {
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
            {
                using (DbCommand cmd = cn.CreateCommand("VACUUM"))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public enum SystemAskType { AnyStars, PopulatedStars, UnPopulatedStars };

        // Return vector of stars, for gridid
        public static int GetSystemVector<V>(int gridid, ref V[] vertices, ref uint[] colours,
                                               SystemAskType ask, int percentage,
                                               Func<float, float, float, V> tovect)
            where V : struct
        {
            int numvertices = 0;

            vertices = null;
            colours = null;

            Color[] fixedc = new Color[4];
            fixedc[0] = Color.Red;
            fixedc[1] = Color.Orange;
            fixedc[2] = Color.Yellow;
            fixedc[3] = Color.White;

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("SELECT id,x,y,z,randomid from EdsmSystems where gridid=@gridid"))
                    {
                        cmd.AddParameterWithValue("gridid", gridid);

                        if (ask == SystemAskType.PopulatedStars)
                            cmd.CommandText += " AND (EddbId IS NOT NULL AND EddbId <> 0)";
                        else if (ask == SystemAskType.UnPopulatedStars)
                            cmd.CommandText += " AND (EddbId IS NULL OR EddbId = 0)";

                        if (percentage < 100)
                            cmd.CommandText += " and randomid<" + percentage;

                        //Stopwatch ws = new Stopwatch();  ws.Start();

                        Object[] array = new Object[5];     // to the number of items above queried

                        vertices = new V[250000];
                        colours = new uint[250000];

                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reader.GetValues(array);

                                long id = (long)array[0];
                                long x = (long)array[1];
                                long y = (long)array[2];
                                long z = (long)array[3];
                                int rand = (int)(long)array[4];

                                if (numvertices == vertices.Length)
                                {
                                    Array.Resize(ref vertices, vertices.Length + 32768);
                                    Array.Resize(ref colours, colours.Length + 32768);
                                }

                                V pos = tovect((float)(x / XYZScalar), (float)(y / XYZScalar), (float)(z / XYZScalar));

                                Color basec = fixedc[rand & 3];
                                int fade = 100 - ((rand >> 2) & 7) * 8;
                                byte red = (byte)(basec.R * fade / 100);
                                byte green = (byte)(basec.G * fade / 100);
                                byte blue = (byte)(basec.B * fade / 100);
                                colours[numvertices] = BitConverter.ToUInt32(new byte[] { red, green, blue, 255 }, 0);
                                vertices[numvertices++] = pos;
                            }
                        }

                        Array.Resize(ref vertices, numvertices);
                        Array.Resize(ref colours, numvertices);

                        //Console.WriteLine("Query {0} grid {1} ret {2} took {3}", cmd.CommandText, gridid, numvertices, ws.ElapsedMilliseconds);

                        if (gridid == GridId.SolGrid && vertices != null)    // BODGE do here, better once on here than every star for every grid..
                        {                       // replace when we have a better naming system
                            int solindex = Array.IndexOf(vertices, new Vector3(0, 0, 0));

                            if (solindex >= 0)
                                colours[solindex] = 0x00ffff;   //yellow
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return numvertices;
        }

        public static List<Point3D> GetStarPositions(int percentage)  // return star positions..
        {
            List<Point3D> list = new List<Point3D>();

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("select x,y,z from EdsmSystems where RandomId<@percentage"))
                    {
                        cmd.AddParameterWithValue("@percentage", percentage);

                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (System.DBNull.Value != reader["x"])
                                    list.Add(new Point3D(((double)(long)reader["x"]) / XYZScalar, ((double)(long)reader["y"]) / XYZScalar, ((double)(long)reader["z"]) / XYZScalar));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
            return list;
        }


        public static List<long> GetEdsmIdsFromName(string name, SQLiteConnectionSystem cn = null, bool uselike = false)
        {
            List<long> ret = new List<long>();

            if (name != null && name.Length > 0)
            {
                bool ownconn = false;
                try
                {
                    if (cn == null)
                    {
                        ownconn = true;
                        cn = new SQLiteConnectionSystem();
                    }

                    using (DbCommand cmd = cn.CreateCommand("SELECT Name,EdsmId FROM SystemNames WHERE " + (uselike ? "Name like @first" : "Name==@first")))
                    {
                        cmd.AddParameterWithValue("first", name + (uselike ? "%" : ""));

                        using (DbDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                ret.Add((long)rdr[1]);
                            }
                        }
                    }
                }
                finally
                {
                    if (ownconn)
                    {
                        cn.Dispose();
                    }
                }
            }

            return ret;
        }

        public static List<ISystem> GetSystemsByName(string name, SQLiteConnectionSystem cn = null, bool uselike = false)
        {
            List<ISystem> systems = new List<ISystem>();

            List<long> edsmidlist = GetEdsmIdsFromName(name, cn, uselike);

            if (edsmidlist.Count != 0)
            {
                foreach (long edsmid in edsmidlist)
                {
                    ISystem sys = GetSystem(edsmid, cn, SystemIDType.EdsmId, name: name);
                    if (sys != null)
                    {
                        systems.Add(sys);
                    }
                }
            }

            return systems;
        }

        public enum SystemIDType { id, EdsmId, EddbId };       // which ID to match?
        public static ISystem GetSystem(long id, SQLiteConnectionSystem cn = null, SystemIDType idtype = SystemIDType.id, string name = null)      // using an id
        {
            // Outer JOIN - name is optional in lookup, but then enforced later in code
            string cmdtext = "Select s.*,n.Name " +
                            "From EdsmSystems s left Outer Join SystemNames n On n.EdsmId=s.EdsmId " +
                            "where s." + idtype.ToString() + " = " + id.ToStringInvariant() + " LIMIT 1";

            return GetSystemCmd(cmdtext, null, cn, name);
        }

        public static ISystem GetSystem(string name, SQLiteConnectionSystem cn = null)      // get a name.. 
        {
            if (string.IsNullOrEmpty(name))   // some bad behaving code is feed thru empty strings.. route planner!
                return null;

            // JOIN - edsmsystems is mandatory.. 
            string cmdtext = "Select s.*,n.Name From SystemNames n Join EdsmSystems s On n.EdsmId=s.EdsmId Where n.Name = @string LIMIT 1";

            return GetSystemCmd(cmdtext, name, cn, name);
        }

        private static ISystem GetSystemCmd(string cmdtext, string strpara, SQLiteConnectionSystem cn = null, string name = null)
        {
            ISystem sys = null;
            bool closeit = false;

            try
            {
                if (cn == null)
                {
                    closeit = true;
                    cn = new SQLiteConnectionSystem();
                }

                // combined lookup of EdsmSystems and Systemnames but allowing Names not to be present..
                // for rare circumstances, you can name it, but will only be used if for some reason the system name table is missing the entry
                // which should never occur, unless something nasty happened during table updates.
                // Tests performed show the best scenario is a combined lookup, followed by a EDDB lookup on its own.
                // Even in edsmid's where there is lots of EDDB data (<50000)
                // Tested on 23/1/2018 
                // Code for EDDB lookup but its slower "Select s.*,n.Name,e.Population,e.Faction,e.GovernmentId,e.AllegianceId,e.PrimaryEconomyId,e.Security,e.EddbUpdatedAt,e.State,e.NeedsPermit " | Left Outer Join EddbSystems e on e.Edsmid=s.EdsmId 

                using (DbCommand cmd = cn.CreateCommand(cmdtext))
                {
                    if (strpara != null)
                    {
                        cmd.AddParameterWithValue("@string", strpara);
                        //System.Diagnostics.Debug.WriteLine("Lookup " + strpara);
                    }

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sys = new SystemClass
                            {
                                ID = (long)reader["id"],
                                EDSMID = (long)reader["EdsmId"],
                                EDDBID = reader["EddbId"] == System.DBNull.Value ? 0 : (long)reader["EddbId"],
                                CreateDate = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds((long)reader["CreateTimestamp"]),
                                UpdateDate = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds((long)reader["UpdateTimestamp"]),
                                status = SystemStatusEnum.EDSM,
                                GridID = (int)(long)reader["GridId"],
                                RandomID = (int)(long)reader["RandomId"]
                            };

                            if (reader["Name"] == System.DBNull.Value) // if no name found, we have a db error, but lets see if we can recover
                            {
                                if (name == null)   // no name, fail (Tested 23/1/2018 with a manual delete of an entry)
                                    return null; //Finally of course gets executed

                                sys.Name = name;
                            }
                            else
                            {
                                sys.Name = (string)reader["Name"];
                            }

                            if (System.DBNull.Value != reader["x"])
                            {
                                sys.X = ((double)(long)reader["x"]) / XYZScalar;
                                sys.Y = ((double)(long)reader["y"]) / XYZScalar;
                                sys.Z = ((double)(long)reader["z"]) / XYZScalar;
                            }

                            if (sys.EDDBID != 0)
                            {
                                using (DbCommand cmd2 = cn.CreateCommand("SELECT * FROM EddbSystems WHERE EddbId = @EddbId LIMIT 1"))
                                {
                                    cmd2.AddParameterWithValue("EddbId", sys.EDDBID);
                                    using (DbDataReader reader2 = cmd2.ExecuteReader())
                                    {
                                        if (reader2.Read())
                                        {
                                            object o;

                                            o = reader2["Population"];
                                            sys.Population = o == DBNull.Value ? 0 : (long)o;

                                            o = reader2["Faction"];
                                            sys.Faction = o == DBNull.Value ? null : (string)o;

                                            o = reader2["GovernmentId"];
                                            sys.Government = o == DBNull.Value ? EDGovernment.Unknown : (EDGovernment)((long)o);

                                            o = reader2["AllegianceId"];
                                            sys.Allegiance = o == DBNull.Value ? EDAllegiance.Unknown : (EDAllegiance)((long)o);

                                            o = reader2["PrimaryEconomyId"];
                                            sys.PrimaryEconomy = o == DBNull.Value ? EDEconomy.Unknown : (EDEconomy)((long)o);

                                            o = reader2["Security"];
                                            sys.Security = o == DBNull.Value ? EDSecurity.Unknown : (EDSecurity)((long)o);

                                            o = reader2["EddbUpdatedAt"];
                                            sys.EDDBUpdatedAt = o == DBNull.Value ? 0 : (int)((long)o);

                                            o = reader2["State"];
                                            sys.State = o == DBNull.Value ? EDState.Unknown : (EDState)((long)o);

                                            o = reader2["NeedsPermit"];
                                            sys.NeedsPermit = o == DBNull.Value ? 0 : (int)((long)o);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (closeit && cn != null)
                {
                    cn.Dispose();
                }
            }

            return sys;
        }

        // Only hidden systems are deleted, so should be close.

        public static long GetTotalSystemsFast()        // it is indeed much faster
        {
            long value = 0;

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("select MAX(Id) from EdsmSystems"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                value = (long)reader["MAX(Id)"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return value;
        }

        public static long GetTotalSystems()
        {
            long value = 0;

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("select Count(EdsmId) from EdsmSystems"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                value = (long)reader["Count(EdsmId)"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return value;
        }

        // give nearest star to maxdistance limit
        public static ISystem FindNearestSystemTo(double x, double y, double z, double maxdistance = 1000, SQLiteConnectionSystem cn = null)
        {
            BaseUtils.SortedListDoubleDuplicate<ISystem> distlist = new BaseUtils.SortedListDoubleDuplicate<ISystem>();
            GetSystemListBySqDistancesFrom(distlist, x, y, z, 1, 0.0, maxdistance, false, cn);
            return distlist.Select(v => v.Value).FirstOrDefault();
        }

        // List stars, in dist order, from x/y/z, with min/max dist, limited to list, by spherical or cube.
        // Must use the duplicate class. Previously you could just use a sorted list, but FindNearestSystemTo was using the non
        // duplicate key compararer and faulting over two systems with the same distance! Doh

        public static void GetSystemListBySqDistancesFrom(BaseUtils.SortedListDoubleDuplicate<ISystem> distlist, double x, double y, double z,
                                                    int maxitems,
                                                    double mindist, double maxdist, bool spherical,
                                                    SQLiteConnectionSystem cn = null)
        {
            bool closeit = false;

            try
            {
                if (cn == null)
                {
                    closeit = true;
                    cn = new SQLiteConnectionSystem();
                }

                using (DbCommand cmd = cn.CreateCommand(
                    "SELECT EdsmId, x, y, z " +
                    "FROM EdsmSystems " +
                    "WHERE x >= @xv - @maxdist " +
                    "AND x <= @xv + @maxdist " +
                    "AND y >= @yv - @maxdist " +
                    "AND y <= @yv + @maxdist " +
                    "AND z >= @zv - @maxdist " +
                    "AND z <= @zv + @maxdist " +
                    "AND (x-@xv)*(x-@xv)+(y-@yv)*(y-@yv)+(z-@zv)*(z-@zv)>=@mindist " +     // tried a direct spherical lookup using <=maxdist, too slow
                    "ORDER BY (x-@xv)*(x-@xv)+(y-@yv)*(y-@yv)+(z-@zv)*(z-@zv) " +
                    "LIMIT @max"
                    ))
                {
                    //System.Diagnostics.Debug.WriteLine("DB query " + maxitems + " " + mindist + " " + maxdist);
                    cmd.AddParameterWithValue("xv", (long)(x * XYZScalar));
                    cmd.AddParameterWithValue("yv", (long)(y * XYZScalar));
                    cmd.AddParameterWithValue("zv", (long)(z * XYZScalar));
                    cmd.AddParameterWithValue("max", maxitems + 1);     // 1 more, because if we are on a star, that will be returned
                    cmd.AddParameterWithValue("maxdist", (long)(maxdist * XYZScalar));
                    cmd.AddParameterWithValue("mindist", (long)(mindist * mindist * XYZScalar * XYZScalar));  // note the double XYZScalar! needed

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())// && distlist.Count < maxitems)           // already sorted, and already limited to max items
                        {
                            long edsmid = (long)reader[0];

                            if (System.DBNull.Value != reader[1])                 // paranoid check for null
                            {
                                ISystem sys = SystemCache.FindSystem(edsmid, cn); // pass it thru the cache..
                               // System.Diagnostics.Debug.WriteLine("Return " + sys.name);
                                if (sys != null && sys.Name != null)
                                {
                                    double dx = ((double)(long)reader[1]) / XYZScalar - x;
                                    double dy = ((double)(long)reader[2]) / XYZScalar - y;
                                    double dz = ((double)(long)reader[3]) / XYZScalar - z;

                                    double distsq = dx * dx + dy * dy + dz * dz;

                                    if ((!spherical || distsq <= maxdist * maxdist))// MUST use duplicate double list to protect against EDSM having two at the same point
                                        distlist.Add(distsq, sys);                  // which Rob has seen crashing the program! Bad EDSM!
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (closeit && cn != null)
                {
                    cn.Dispose();
                }
            }
        }

        // System must be here, within reason, else its a null

        public static ISystem GetSystemByPosition(double x, double y, double z, SQLiteConnectionSystem cn)
        {
            bool closeit = false;

            if (cn == null)
            {
                closeit = true;
                cn = new SQLiteConnectionSystem();
            }

            ISystem sys = null;

            using (DbCommand selectByPosCmd = cn.CreateCommand(
                "SELECT s.EdsmId FROM EdsmSystems s " +         // 16 is 0.125 of 1/128, so pick system near this one
                "WHERE s.X >= @X - 16 " +
                "AND s.X <= @X + 16 " +
                "AND s.Y >= @Y - 16 " +
                "AND s.Y <= @Y + 16 " +
                "AND s.Z >= @Z - 16 " +
                "AND s.Z <= @Z + 16 LIMIT 1"))
            {
                selectByPosCmd.AddParameterWithValue("@X", (long)(x * XYZScalar));
                selectByPosCmd.AddParameterWithValue("@Y", (long)(y * XYZScalar));
                selectByPosCmd.AddParameterWithValue("@Z", (long)(z * XYZScalar));

                using (DbDataReader reader = selectByPosCmd.ExecuteReader())        // MEASURED very fast, <1ms
                {
                    while (reader.Read())
                    {
                        long pos_edsmid = (long)reader["EdsmId"];
                        sys = SystemCache.FindSystem(pos_edsmid, cn);
                        if (sys != null)
                            break;
                    }
                }
            }

            if (closeit && cn != null)
            {
                cn.Dispose();
            }

            return sys;
        }



        public const int metric_nearestwaypoint = 0;     // easiest way to synchronise metric selection..
        public const int metric_mindevfrompath = 1;
        public const int metric_maximum100ly = 2;
        public const int metric_maximum250ly = 3;
        public const int metric_maximum500ly = 4;
        public const int metric_waypointdev2 = 5;

        public static ISystem GetSystemNearestTo(Point3D curpos, Point3D wantedpos, double maxfromcurpos, double maxfromwanted,
                                    int routemethod)
        {
            ISystem nearestsystem = null;

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    string sqlquery = "SELECT EdsmId, x, y, z " +                   // DO a square test for speed, then double check its within the circle later..
                                      "FROM EdsmSystems " +
                                      "WHERE x >= @xc - @maxfromcurpos " +
                                      "AND x <= @xc + @maxfromcurpos " +
                                      "AND y >= @yc - @maxfromcurpos " +
                                      "AND y <= @yc + @maxfromcurpos " +
                                      "AND z >= @zc - @maxfromcurpos " +
                                      "AND z <= @zc + @maxfromcurpos " +
                                      "AND x >= @xw - @maxfromwanted " +
                                      "AND x <= @xw + @maxfromwanted " +
                                      "AND y >= @yw - @maxfromwanted " +
                                      "AND y <= @yw + @maxfromwanted " +
                                      "AND z >= @zw - @maxfromwanted " +
                                      "AND z <= @zw + @maxfromwanted ";

                    using (DbCommand cmd = cn.CreateCommand(sqlquery))
                    {
                        cmd.AddParameterWithValue("xw", (long)(wantedpos.X * XYZScalar));
                        cmd.AddParameterWithValue("yw", (long)(wantedpos.Y * XYZScalar));
                        cmd.AddParameterWithValue("zw", (long)(wantedpos.Z * XYZScalar));
                        cmd.AddParameterWithValue("maxfromwanted", (long)(maxfromwanted * XYZScalar));     //squared

                        cmd.AddParameterWithValue("xc", (long)(curpos.X * XYZScalar));
                        cmd.AddParameterWithValue("yc", (long)(curpos.Y * XYZScalar));
                        cmd.AddParameterWithValue("zc", (long)(curpos.Z * XYZScalar));
                        cmd.AddParameterWithValue("maxfromcurpos", (long)(maxfromcurpos * XYZScalar));     //squared

                        double bestmindistance = double.MaxValue;
                        long nearestedsmid = -1;

                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                long edsmid = (long)reader[0];

                                //SystemClass sys = GetSystem(edsmid, null, SystemIDType.EdsmId);  Console.WriteLine("FOund {0} at {1} {2} {3}", sys.name, sys.x, sys.y, sys.z);

                                if (System.DBNull.Value != reader["x"]) // paranoid check, it could be null in db
                                {
                                    Point3D syspos = new Point3D(((double)(long)reader[1]) / XYZScalar, ((double)(long)reader[2]) / XYZScalar, ((double)(long)reader[3]) / XYZScalar);

                                    double distancefromwantedx2 = Point3D.DistanceBetweenX2(wantedpos, syspos); // range between the wanted point and this, ^2
                                    double distancefromcurposx2 = Point3D.DistanceBetweenX2(curpos, syspos);    // range between the wanted point and this, ^2

                                    // ENSURE its withing the circles now
                                    if (distancefromcurposx2 <= (maxfromcurpos * maxfromcurpos) && distancefromwantedx2 <= (maxfromwanted * maxfromwanted))
                                    {
                                        if (routemethod == metric_nearestwaypoint)
                                        {
                                            if (distancefromwantedx2 < bestmindistance)
                                            {
                                                nearestedsmid = edsmid;
                                                bestmindistance = distancefromwantedx2;
                                            }
                                        }
                                        else
                                        {
                                            Point3D interceptpoint = curpos.InterceptPoint(wantedpos, syspos);      // work out where the perp. intercept point is..
                                            double deviation = Point3D.DistanceBetween(interceptpoint, syspos);
                                            double metric = 1E39;

                                            if (routemethod == metric_mindevfrompath)
                                                metric = deviation;
                                            else if (routemethod == metric_maximum100ly)
                                                metric = (deviation <= 100) ? distancefromwantedx2 : metric;        // no need to sqrt it..
                                            else if (routemethod == metric_maximum250ly)
                                                metric = (deviation <= 250) ? distancefromwantedx2 : metric;
                                            else if (routemethod == metric_maximum500ly)
                                                metric = (deviation <= 500) ? distancefromwantedx2 : metric;
                                            else if (routemethod == metric_waypointdev2)
                                                metric = Math.Sqrt(distancefromwantedx2) + deviation / 2;

                                            if (metric < bestmindistance)
                                            {
                                                nearestedsmid = edsmid;
                                                bestmindistance = metric;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (nearestedsmid != -1)
                            nearestsystem = GetSystem(nearestedsmid, cn, SystemIDType.EdsmId);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return nearestsystem;
        }



        public static bool GetSystemAndAlternatives(ISystem refsys, out ISystem system, out List<ISystem> alternatives, out string namestatus)
        {
            system = new EliteDangerousCore.SystemClass
            {
                Name = refsys.Name,
                X = refsys.HasCoordinate ? refsys.X : Double.NaN,
                Y = refsys.HasCoordinate ? refsys.Y : Double.NaN,
                Z = refsys.HasCoordinate ? refsys.Z : Double.NaN,
                EDSMID = refsys.EDSMID
            };

            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
            {
                Dictionary<string, List<long>> aliasesByName = new Dictionary<string, List<long>>(StringComparer.InvariantCultureIgnoreCase);
                Dictionary<long, long> aliasesById = new Dictionary<long, long>();

                using (DbCommand cmd = cn.CreateCommand("SELECT name, id_edsm, id_edsm_mergedto FROM SystemAliases"))
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = (string)reader["name"];
                            long edsmid = (long)reader["id_edsm"];
                            long mergedto = (long)reader["id_edsm_mergedto"];
                            if (!aliasesByName.ContainsKey(name))
                            {
                                aliasesByName[name] = new List<long>();
                            }
                            aliasesByName[name].Add(mergedto);
                            aliasesById[edsmid] = mergedto;
                        }
                    }
                }


                Dictionary<long, ISystem> altmatches = new Dictionary<long, ISystem>();
                Dictionary<long, ISystem> matches = new Dictionary<long, ISystem>();
                ISystem edsmidmatch = null;
                long sel_edsmid = refsys.EDSMID;
                bool hastravcoords = refsys.HasCoordinate && (refsys.Name.ToLowerInvariant() == "sol" || refsys.X != 0 || refsys.Y != 0 || refsys.Z != 0);
                bool multimatch = false;

                if (sel_edsmid != 0)
                {
                    edsmidmatch = GetSystem(sel_edsmid, cn, SystemIDType.EdsmId);
                    if (edsmidmatch != null)
                    {
                        matches.Add(edsmidmatch.ID, edsmidmatch);

                        while (aliasesById.ContainsKey(sel_edsmid))
                        {
                            sel_edsmid = aliasesById[sel_edsmid];
                            ISystem sys = GetSystem(sel_edsmid, cn, SystemIDType.EdsmId);
                            if (sys != null)
                                altmatches.Add(sys.ID, sys);
                            edsmidmatch = null;
                        }
                    }
                }

                //Stopwatch sw2 = new Stopwatch(); sw2.Start(); //long t2 = sw2.ElapsedMilliseconds; Tools.LogToFile(string.Format("Query names in {0}", t2));

                Dictionary<long, ISystem> namematches = GetSystemsByName(refsys.Name).Where(s => s != null).ToDictionary(s => s.ID, s => s);
                Dictionary<long, ISystem> posmatches = new Dictionary<long, ISystem>();
                Dictionary<long, ISystem> nameposmatches = new Dictionary<long, ISystem>();

                if (hastravcoords)
                {
                    using (DbCommand selectByPosCmd = cn.CreateCommand(
                        "SELECT s.EdsmId FROM EdsmSystems s " +         // 16 is 0.125 of 1/128, so pick system near this one
                        "WHERE s.X >= @X - 16 " +
                        "AND s.X <= @X + 16 " +
                        "AND s.Y >= @Y - 16 " +
                        "AND s.Y <= @Y + 16 " +
                        "AND s.Z >= @Z - 16 " +
                        "AND s.Z <= @Z + 16"))
                    {
                        selectByPosCmd.AddParameterWithValue("@X", (long)(refsys.X * XYZScalar));
                        selectByPosCmd.AddParameterWithValue("@Y", (long)(refsys.Y * XYZScalar));
                        selectByPosCmd.AddParameterWithValue("@Z", (long)(refsys.Z * XYZScalar));

                        //Stopwatch sw = new Stopwatch(); sw.Start(); long t1 = sw.ElapsedMilliseconds; Tools.LogToFile(string.Format("Query pos in {0}", t1));

                        using (DbDataReader reader = selectByPosCmd.ExecuteReader())        // MEASURED very fast, <1ms
                        {


                            while (reader.Read())
                            {
                                long pos_edsmid = (long)reader["EdsmId"];
                                ISystem sys = GetSystem(pos_edsmid, cn, SystemIDType.EdsmId);
                                if (sys != null)
                                {
                                    matches[sys.ID] = sys;
                                    posmatches[sys.ID] = sys;

                                    if (sys.Name.Equals(refsys.Name, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        nameposmatches[sys.ID] = sys;
                                    }
                                }
                            }
                        }
                    }
                }

                if (aliasesByName.ContainsKey(refsys.Name))
                {
                    foreach (long alt_edsmid in aliasesByName[refsys.Name])
                    {
                        ISystem sys = GetSystem(alt_edsmid, cn, SystemIDType.EdsmId);
                        if (sys != null)
                        {
                            altmatches[sys.ID] = sys;
                        }
                    }
                }

                foreach (var sys in namematches.Values)
                {
                    matches[sys.ID] = sys;
                }

                if (altmatches.Count != 0)
                {
                    foreach (var alt in altmatches.Values)
                    {
                        matches[alt.ID] = alt;
                    }
                }

                alternatives = matches.Values.Select(s => (ISystem)s).ToList();

                if (edsmidmatch != null)
                {
                    system = edsmidmatch;

                    if (nameposmatches.ContainsKey(system.ID)) // name and position matches
                    {
                        namestatus = "Exact match";
                        return true; // Continue to next system
                    }
                    else if (posmatches.ContainsKey(system.ID)) // position matches
                    {
                        namestatus = "Name differs";
                        return true; // Continue to next system
                    }
                    else if (!hastravcoords || !system.HasCoordinate) // no coordinates available
                    {
                        if (namematches.ContainsKey(system.ID)) // name matches
                        {
                            if (!system.HasCoordinate)
                            {
                                namestatus = "System has no known coordinates";
                            }
                            else
                            {
                                namestatus = "Travel log entry has no coordinates";
                            }

                            return true; // Continue to next system
                        }
                        else if (!refsys.HasCoordinate)
                        {
                            namestatus = "Name differs";
                        }
                    }
                }

                if (nameposmatches != null && nameposmatches.Count != 0)
                {
                    if (nameposmatches.Count == 1)
                    {
                        // Both name and position matches
                        system = nameposmatches.Values.Single();
                        namestatus = "Exact match";
                        return true; // Continue to next system
                    }
                    else if (posmatches.Count == 1)
                    {
                        // Position matches
                        system = posmatches.Values.Single();
                        namestatus = $"System {system.Name} found at location";
                        return true; // Continue to next system
                    }
                    else
                    {
                        multimatch = true;
                    }
                }

                if (namematches != null && namematches.Count != 0)
                {
                    if (namematches.Count == 1)
                    {
                        // One system name matched
                        system = namematches.Values.Single();
                        namestatus = "Name matched";
                        return true;
                    }
                    else if (namematches.Count > 1)
                    {
                        multimatch = true;
                    }
                }

                if (multimatch)
                {
                    namestatus = "Multiple system matches found";
                }
                else
                {
                    namestatus = "System not found";
                }
            }

            return false;
        }

        #region Autocomplete

        public static List<string> AutoCompleteAdditionalList = new List<string>();

        public static void AddToAutoComplete(List<string> t)
        {
            lock (AutoCompleteAdditionalList)
            {
                AutoCompleteAdditionalList.AddRange(t);
            }
        }

        public static List<string> ReturnSystemListForAutoComplete(string input, Object ctrl)
        {
            List<string> ret = new List<string>();
            ret.AddRange(ReturnOnlyGalMapListForAutoComplete(input, ctrl));
            ret.AddRange(ReturnOnlySystemsListForAutoComplete(input, ctrl));
            return ret;
        }

        public static List<string> ReturnOnlyGalMapListForAutoComplete(string input, Object ctrl)
        {
            List<string> ret = new List<string>();

            if (input != null && input.Length > 0)
            {
                lock (AutoCompleteAdditionalList)
                {
                    foreach (string other in AutoCompleteAdditionalList)
                    {
                        if (other.StartsWith(input, StringComparison.InvariantCultureIgnoreCase))
                            ret.Add(other);
                    }
                }
            }
            return ret;
        }

        public static List<string> ReturnOnlySystemsListForAutoComplete(string input, Object ctrl)
        {
            List<string> ret = new List<string>();
            if (input != null && input.Length > 0)
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("SELECT Name,EdsmId FROM SystemNames WHERE Name>=@first AND Name<=@second LIMIT 1000"))
                    {
                        cmd.AddParameterWithValue("first", input);
                        cmd.AddParameterWithValue("second", input + "~");

                        using (DbDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                ret.Add((string)rdr[0]);
                            }
                        }
                    }
                }
            }
            return ret;
        }

        #endregion

        /// <summary>
        /// Get an <see cref="ISystem"/> from <paramref name="systemName"/> optionally checking for merged systems if no exact match is found. Returns true if the system was found.
        /// </summary>
        /// <param name="systemName">The human-readable name for the system to be checked.</param>
        /// <param name="result">Will be <c>null</c> if the return value is <c>false</c>. Otherwise, will be the system known as the supplied <paramref name="systemName"/>.</param>
        /// <param name="checkMergers">If <c>true</c>, and no system exactly matches <paramref name="systemName"/>, check to see if it has was merged to another system.</param>
        /// <param name="cn">The database connection to use.</param>
        /// <returns><c>true</c> if the system is known (with the system in <paramref name="result"/>), <c>false</c> otherwise.</returns>
        public static bool TryGetSystem(string systemName, out ISystem result, bool checkMergers = false, SQLiteConnectionSystem cn = null)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(systemName))  // No way José.
                return false;

            result = GetSystem(systemName, cn);
            ISystem s;
            if (result == null && checkMergers && privTryGetMergedSystem(systemName, out s, cn))
                result = s;

            return (result != null);
        }

        /// <summary>
        /// Test if <paramref name="systemName"/> has been merged to another system. The return value indicates if a merged system was found.
        /// </summary>
        /// <param name="systemName">The name of the system to be checked.</param>
        /// <param name="result">Will be <c>null</c> if the return value is <c>false</c>. Otherwise, will be the system that was once named <paramref name="systemName"/>.</param>
        /// <param name="cn">The database connection to use.</param>
        /// <returns><c>true</c> if the system is known by a different name (with the system in <paramref name="result"/>), <c>false</c> otherwise.</returns>
        private static bool privTryGetMergedSystem(string systemName, out ISystem result, SQLiteConnectionSystem cn = null)
        {
            result = null;
            bool createdCn = false;
            long edsmMergedId = long.MinValue;

            try
            {
                if (cn == null)
                {
                    createdCn = true;
                    cn = new SQLiteConnectionSystem();
                }
                using (DbCommand cmd = cn.CreateCommand("SELECT id_edsm_mergedto FROM SystemAliases WHERE name = @param1 LIMIT 1"))
                {
                    cmd.AddParameterWithValue("param1", systemName);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            edsmMergedId = (long)reader["id_edsm_mergedto"];
                    }
                }
                if (edsmMergedId != long.MinValue)
                    result = GetSystem(edsmMergedId, cn, SystemIDType.EdsmId);
            }
            finally
            {
                if (createdCn && cn != null)
                    cn.Dispose();
            }

            return (result != null);
        }

        #region Star Database Debugging-  not for main line code.

        // give me list of duplicate systems by position

        static public void DuplicateSystemsByPosition()       
        {
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
            {
                List<Tuple<long, long, long>> pos = new List<Tuple<long, long, long>>();

                using (DbCommand cmd = cn.CreateCommand("SELECT X,y,Z,count(1) as CNT From EdsmSystems s Group By X,y,Z having CNT>1"))
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long xs = (long)reader["x"];
                            long ys = (long)reader["y"];
                            long zs = (long)reader["z"];

                            System.Diagnostics.Debug.WriteLine("Duplicate at {0} {1} {2}", xs/128.0, ys/128.0, zs/128.0);
                            pos.Add(new Tuple<long, long, long>(xs, ys, zs));
                        }
                    }
                }

                foreach (var p in pos)
                {
                    using (DbCommand cmd2 = cn.CreateCommand("SELECT s.*,n.Name From EdsmSystems s join SystemNames n on n.EdsmId=s.Edsmid where x=@x And y=@y And z=@z"))
                    {
                        cmd2.AddParameterWithValue("@x", p.Item1);
                        cmd2.AddParameterWithValue("@y", p.Item2);
                        cmd2.AddParameterWithValue("@z", p.Item3);

                        using (DbDataReader reader = cmd2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = (string)reader["Name"];
                                long edsmid = (long)reader["EdsmId"];
                                long xs = (long)reader["x"];
                                long ys = (long)reader["y"];
                                long zs = (long)reader["z"];
                                System.Diagnostics.Debug.WriteLine("Dup {0} {1} at {2},{3},{4}", name, edsmid, xs / 128.0, ys / 128.0, zs / 128.0);
                            }

                            System.Diagnostics.Debug.WriteLine("");
                        }

                    }
                }
            }

            #endregion
        }
    }

    public class GridId
    {
        public const int GridXRange = 20;
        static private int[] compresstablex = {
                                                0,1,1,1,1, 2,2,2,2,2,                   // 0   -20
                                                3,3,4,4,5, 5,6,7,8,9,                   // 10   -10,-8,-6,..
                                                10,11,12,13,14, 14,15,15,16,16,         // 20 centre
                                                17,17,17,17,17, 18,18,18,18,18,         // 30   +10
                                                19,19                                   // 40   +20
                                            };
        public const int GridZRange = 26;
        static private int[] compresstablez = {
                                                0,1,1,2,2,      3,4,5,6,7,              // 0  -10
                                                8,9,10,11,12,   12,13,13,14,14,         // 10 Sol 0
                                                15,15,15,15,15, 16,16,16,16,16,         // 20   +10
                                                17,17,17,17,17, 18,18,18,18,18,         // 30 centre +20
                                                19,19,19,19,19, 20,20,20,20,20,         // 40 +30
                                                21,21,21,21,21, 22,22,22,22,22,         // 50 +40    
                                                23,23,23,23,23, 24,24,24,24,24,         // 60 +50
                                                25,25                                   // 70 +60
                                            };
        public const int xleft = -20500;
        public const int xright = 20000;
        public const int zbot = -10500;
        public const int ztop = 60000;

        public const int MinGridID = 0;
        public const int MaxGridID = GridZRange * ZMult + GridXRange;

        public const int SolGrid = 810;

        private const int ZMult = 100;

        public static int Id(double x, double z)
        {
            x = Math.Min(Math.Max(x - xleft, 0), xright - xleft);       // 40500
            z = Math.Min(Math.Max(z - zbot, 0), ztop - zbot);           // 70500
            x /= 1000;                                                  // 0-40.5 inc
            z /= 1000;                                                  // 0-70.5 inc
            return compresstablex[(int)x] + ZMult * compresstablez[(int)z];
        }

        public static int IdFromComponents(int x, int z)                // given x grid/ y grid give ID
        {
            return x + ZMult * z;
        }

        public static bool XZ(int id, out float x, out float z, bool mid = true)         // given id, return x/z pos of left bottom
        {
            x = 0; z = 0;
            if (id >= 0)
            {
                int xid = (id % ZMult);
                int zid = (id / ZMult);

                if (xid < GridXRange && zid < GridZRange)
                {
                    for (int i = 0; i < compresstablex.Length; i++)
                    {
                        if (compresstablex[i] == xid)
                        {
                            double startx = i * 1000 + xleft;

                            while (i < compresstablex.Length && compresstablex[i] == xid)
                                i++;

                            x = (mid) ? (float)((((i * 1000) + xleft) + startx) / 2.0) : (float)startx;
                            break;
                        }
                    }

                    for (int i = 0; i < compresstablez.Length; i++)
                    {
                        if (compresstablez[i] == zid)
                        {
                            double startz = i * 1000 + zbot;

                            while (i < compresstablez.Length && compresstablez[i] == zid)
                                i++;

                            z = (mid) ? (float)((((i * 1000) + zbot) + startz) / 2.0) : (float)startz;
                            break;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public static List<int> AllId()
        {
            List<int> list = new List<int>();

            for (int z = 0; z < GridZRange; z++)
            {
                for (int x = 0; x < GridXRange; x++)
                    list.Add(IdFromComponents(x, z));
            }
            return list;
        }

        public static int[] XLines(int endentry)            // fill in the LY values, plus an end stop one
        {
            int[] xlines = new int[GridXRange + 1];

            for (int x = 0; x < GridXRange; x++)
            {
                float xp, zp;
                int id = GridId.IdFromComponents(x, 0);
                GridId.XZ(id, out xp, out zp, false);
                xlines[x] = (int)xp;
            }

            xlines[GridXRange] = endentry;

            return xlines;
        }

        public static int[] ZLines(int endentry)
        {
            int[] zlines = new int[GridZRange + 1];

            for (int z = 0; z < GridZRange; z++)
            {
                float xp, zp;
                int id = GridId.IdFromComponents(0, z);
                GridId.XZ(id, out xp, out zp, false);
                zlines[z] = (int)zp;
            }

            zlines[GridZRange] = endentry;

            return zlines;
        }

        public static List<int> FromString(string s)
        {
            if (s == "All")
                return GridId.AllId();
            else
                return s.RestoreIntListFromString();
        }

        public static string ToString(List<int> ids)
        {
            List<int> allid = GridId.AllId();
            if (ids.Count == allid.Count)
                return "All";
            else
                return string.Join(",", ids);
        }

    }
}

