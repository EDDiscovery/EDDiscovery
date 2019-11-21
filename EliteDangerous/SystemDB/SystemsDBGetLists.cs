/*
 * Copyright © 2015 - 2019 EDDiscovery development team
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
using System.Data.Common;

namespace EliteDangerousCore.DB
{
    public partial class SystemsDB
    {
        public static long GetTotalSystems()            // this is SLOW try not to use
        {
            if (SystemsDatabase.Instance.RebuildRunning)
                return 0;

            return SystemsDatabase.Instance.ExecuteWithDatabase(db =>
            {
                var cn = db.Connection;
                using (DbCommand cmd = cn.CreateCommand("select Count(1) from Systems"))
                {
                    return (long)cmd.ExecuteScalar();
                }
            });
        }

        // Beware with no extra conditions, you get them all..  Mostly used for debugging
        // use starreport to avoid storing the entries instead pass back one by one
        public static List<ISystem> ListStars(string where = null, string orderby = null, string limit = null, bool eddbinfo = false, 
                                                Action<ISystem> starreport = null)
        {
            List<ISystem> ret = new List<ISystem>();

            if (SystemsDatabase.Instance.RebuildRunning)
                return ret;

            return SystemsDatabase.Instance.ExecuteWithDatabase(db =>
            {

                //BaseUtils.AppTicks.TickCountLap("Star");

                var cn = db.Connection;

                using (DbCommand selectSysCmd = cn.CreateSelect("Systems s", eddbinfo ? MakeSystemQueryEDDB : MakeSystemQueryNoEDDB, where, orderby, limit: limit,
                    joinlist: (eddbinfo ? MakeSystemQueryEDDBJoinList : MakeSystemQueryJoinList)))
                {
                    using (DbDataReader reader = selectSysCmd.ExecuteReader())
                    {
                        while (reader.Read())      // if there..
                        {
                            SystemClass s = MakeSystem(reader, eddbinfo);
                            if (starreport != null)
                                starreport(s);
                            else
                                ret.Add(s);
                        }
                    }
                }

                //System.Diagnostics.Debug.WriteLine("Find stars " + BaseUtils.AppTicks.TickCountLap("Star"));
                return ret;
            });
        }

        // randimised id % 100 < sercentage
        public static List<V> GetStarPositions<V>(int percentage, Func<int, int, int, V> tovect)  // return all star positions..
        {
            List<V> ret = new List<V>();

            if (SystemsDatabase.Instance.RebuildRunning)
                return ret;

            return SystemsDatabase.Instance.ExecuteWithDatabase(db =>
            {

                var cn = db.Connection;

                using (DbCommand cmd = cn.CreateSelect("Systems s",
                                                       outparas: "s.x,s.y,s.z",
                                                       where: "((s.edsmid*2333)%100) <" + percentage.ToStringInvariant()
                                                       ))
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ret.Add(tovect(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2)));
                        }
                    }
                }

                return ret;
            });
        }
    }
}


