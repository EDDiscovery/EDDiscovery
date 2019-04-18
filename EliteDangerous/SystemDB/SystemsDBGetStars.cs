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
        ///////////////////////////////////////// By Name

        public static ISystem FindStar(string name)
        {
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Reader))
            {
                return FindStar(name, cn);
            }
        }

        public static ISystem FindStar(string name, SQLiteConnectionSystem cn)
        {
            EliteNameClassifier ec = new EliteNameClassifier(name);

            if (ec.IsNamed)
            {
                // needs index on sectorid [nameid]. Relies on Names.id being the edsmid.   

                using (DbCommand selectSysCmd = cn.CreateSelect("Systems s", MakeSystemQueryEDDB,
                                                    "s.edsmid IN (Select id FROM Names WHERE name=@p1) AND s.sectorid IN (Select id FROM Sectors c WHERE c.name=@p2)",
                                                    new Object[] { ec.StarName, ec.SectorName },
                                                    joinlist: MakeSystemQueryEDDBJoinList))
                {
                    // System.Diagnostics.Debug.WriteLine( cn.ExplainQueryPlanString(selectSysCmd));

                    using (DbDataReader reader = selectSysCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MakeSystem(reader);        // read back and make name from db info due to case problems.
                        }
                    }
                }

            }
            else
            {
                // Numeric or Standard - all data in ID
                // needs index on Systems(sectorid, Nameid)

                using (DbCommand selectSysCmd = cn.CreateSelect("Systems s", MakeSysStdNumericQueryEDDB,
                                                    "s.nameid = @p1 AND s.sectorid IN (Select id FROM Sectors c WHERE c.name=@p2)",
                                                    new Object[] { ec.ID, ec.SectorName },
                                                    joinlist: MakeSysStdNumericQueryEDDBJoinList))
                {
                  //  System.Diagnostics.Debug.WriteLine( cn.ExplainQueryPlanString(selectSysCmd));

                    using (DbDataReader reader = selectSysCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MakeSystem(reader, ec.ID); // read back .. sector name is taken from DB for case reasons
                        }
                    }

                }
            }

            return null;
        }

        ///////////////////////////////////////// By EDSMID

        public static ISystem FindStar(long edsmid)
        {
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Reader))
            {
                return FindStar(edsmid, cn);
            }
        }

        public static ISystem FindStar(long edsmid, SQLiteConnectionSystem cn)
        {
            // No indexes needed- edsmid is primary key

            using (DbCommand selectSysCmd = cn.CreateSelect("Systems s", MakeSystemQueryEDDB,
                                                "s.edsmid=@p1",
                                                new Object[] { edsmid },
                                                joinlist: MakeSystemQueryEDDBJoinList))
            {
                //System.Diagnostics.Debug.WriteLine( cn.ExplainQueryPlanString(selectSysCmd));

                using (DbDataReader reader = selectSysCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MakeSystem(reader); 
                    }
                }
            }
            return null;
        }

        ///////////////////////////////////////// By Wildcard

        public static List<ISystem> FindStarWildcard(string name, int limit = int.MaxValue)
        {
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Reader))
            {
                return FindStarWildcard(name, cn, limit);
            }
        }

        public static List<ISystem> FindStarWildcard(string name, SQLiteConnectionSystem cn, int limit = int.MaxValue)
        {
            EliteNameClassifier ec = new EliteNameClassifier(name);

            List<ISystem> ret = new List<ISystem>();

            if (ec.IsStandardParts)     // normal Euk PRoc qc-l d2-3
            {
                // needs index on Systems(sectorid, Nameid)

                using (DbCommand selectSysCmd = cn.CreateSelect("Systems s", MakeSystemQueryEDDB,
                                                    "s.nameid >= @p1 AND s.nameid <= @p2 AND s.sectorid IN (Select id FROM Sectors c WHERE c.name=@p3)",
                                                    new Object[] { ec.ID, ec.IDHigh, ec.SectorName },
                                                    limit:limit,
                                                    joinlist: MakeSystemQueryEDDBJoinList))
                {
                    //System.Diagnostics.Debug.WriteLine( cn.ExplainQueryPlanString(selectSysCmd));

                    using (DbDataReader reader = selectSysCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SystemClass sc = MakeSystem(reader);
                            ret.Add(sc);
                        }
                    }
                }
            }
            else if (ec.IsNumeric)        // HIP 29282
            {
                // checked select *,s.nameid & 0x3fffffffff , cast((s.nameid & 0x3fffffffff) as text) From Systems  s where (s.nameid & (1<<46)!=0) and s.sectorid=15568 USNO entries
                // beware, 1<<46 works, 0x40 0000 0000 does not.. 
                // needs index on Systems(sectorid, Nameid)

                using (DbCommand selectSysCmd = cn.CreateSelect("Systems s", MakeSystemQueryEDDB,
                                                    "(s.nameid & (1<<46) != 0) AND cast((s.nameid & 0x3fffffffff) as text) LIKE @p1 AND s.sectorid IN (Select id FROM Sectors c WHERE c.name=@p2)",
                                                    new Object[] { ec.NameIdNumeric.ToStringInvariant() + "%", ec.SectorName },
                                                    limit:limit,
                                                    joinlist: MakeSystemQueryEDDBJoinList))  
                {

                    //System.Diagnostics.Debug.WriteLine( cn.ExplainQueryPlanString(selectSysCmd));

                    using (DbDataReader reader = selectSysCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SystemClass sc = MakeSystem(reader);
                            ret.Add(sc);
                        }
                    }
                }
            }
            else
            {                             // named
                if (ec.StarName.Length > 0)      // if we have a starname component and a sector name, look up sectorname + starname%
                {
                    // needs index on Systems(sectorid, Nameid)

                    using (DbCommand selectSysCmd = cn.CreateSelect("Systems s", MakeSystemQueryEDDB,
                                                        "s.nameid IN (Select id FROM Names WHERE name LIKE @p1) AND s.sectorid IN (Select id FROM Sectors c WHERE c.name=@p2)",
                                                        new Object[] { ec.StarName + "%", ec.SectorName },
                                                        limit: limit,
                                                        joinlist: MakeSystemQueryEDDBJoinList))
                    {
                        //System.Diagnostics.Debug.WriteLine(cn.ExplainQueryPlanString(selectSysCmd));

                        using (DbDataReader reader = selectSysCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SystemClass sc = MakeSystem(reader);
                                ret.Add(sc);
                            }

                            limit -= ret.Count;
                        }
                    }
                }

                // look up Sector. Use sectorname, unless it NoSectorName in which case use the starname as a presumed sector name

                // needs index on Systems(sectorid, [Nameid])

                if (limit > 0)
                {
                    using (DbCommand selectSysCmd = cn.CreateSelect("Systems s", MakeSystemQueryEDDB,
                                                        "s.sectorid IN (Select id FROM Sectors c WHERE c.name LIKE @p1)",
                                                        new Object[] { (ec.SectorName != EliteNameClassifier.NoSectorName ? ec.SectorName : ec.StarName) + "%" },
                                                        limit: limit,
                                                        joinlist: MakeSystemQueryEDDBJoinList))
                    {
                       // System.Diagnostics.Debug.WriteLine(cn.ExplainQueryPlanString(selectSysCmd));

                        using (DbDataReader reader = selectSysCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SystemClass sc = MakeSystem(reader);
                                ret.Add(sc);
                            }
                        }
                    }
                }
            }

            return ret;
        }

        #region Helpers for getting stars

        //                                         0   1   2   3        4      5        6 
        const string MakeSysStdNumericQuery = "s.x,s.y,s.z,s.edsmid,c.name,c.gridid";
        const string MakeSysStdNumericQueryEDDB = "s.x,s.y,s.z,s.edsmid,c.name,c.gridid,e.eddbid,e.eddbupdatedat,e.population,e.faction,e.government,e.allegiance,e.state,e.security,e.primaryeconomy,e.power,e.powerstate,e.needspermit";
        static string[] MakeSysStdNumericQueryJoinList = new string[] { "JOIN Sectors c on s.sectorid=c.id" };
        static string[] MakeSysStdNumericQueryEDDBJoinList = new string[] { "JOIN Sectors c on s.sectorid=c.id", "LEFT OUTER JOIN EDDB e on e.edsmid=s.edsmid" };

        static SystemClass MakeSystem(DbDataReader reader, ulong nid, bool eddb = true)
        {
            const int offset = 6;

            EliteNameClassifier ec = new EliteNameClassifier(nid);
            ec.SectorName = reader.GetString(4);

            if (!eddb || reader[offset] is System.DBNull)
            {
                return new SystemClass(ec.ToString(), reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt64(3), reader.GetInt32(5));
            }
            else
            {
                return new SystemClass(ec.ToString(), reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt64(3),
                                reader.GetInt64(offset), reader.GetInt32(offset + 1), reader.GetInt64(offset + 2), reader.GetString(offset + 3),
                                (EDGovernment)reader.GetInt64(offset + 4), (EDAllegiance)reader.GetInt64(offset + 5), (EDState)reader.GetInt64(offset + 6), (EDSecurity)reader.GetInt64(offset + 7),
                                (EDEconomy)reader.GetInt64(offset + offset), reader.GetString(offset + 9), reader.GetString(offset + 10), reader.GetInt32(offset + 11),
                                reader.GetInt32(5), SystemStatusEnum.EDSM);
            }
        }

        //                                     0   1   2   3        4      5        6        7      8            
        const string MakeSystemQueryEDDB = "s.x,s.y,s.z,s.edsmid,c.name,c.gridid,s.nameid,n.Name,e.eddbid,e.eddbupdatedat,e.population,e.faction,e.government,e.allegiance,e.state,e.security,e.primaryeconomy,e.power,e.powerstate,e.needspermit";
        const string MakeSystemQueryNoEDDB = "s.x,s.y,s.z,s.edsmid,c.name,c.gridid,s.nameid,n.Name";
        static string[] MakeSystemQueryJoinList = new string[] { "LEFT OUTER JOIN Names n On s.nameid=n.id", "JOIN Sectors c on s.sectorid=c.id" };
        static string[] MakeSystemQueryEDDBJoinList = new string[] { "LEFT OUTER JOIN Names n On s.nameid=n.id", "JOIN Sectors c on s.sectorid=c.id", "LEFT OUTER JOIN EDDB e on e.edsmid=s.edsmid" };

        static SystemClass MakeSystem(DbDataReader reader, bool eddbinfo = true)
        {
            EliteNameClassifier ec = new EliteNameClassifier((ulong)reader.GetInt64(6));
            ec.SectorName = reader.GetString(4);

            if (ec.IsNamed)
                ec.StarName = reader.GetString(7);

            const int offset = 8;
            if (!eddbinfo || reader[offset] is System.DBNull)
            {
                return new SystemClass(ec.ToString(), reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt64(3), reader.GetInt32(5));
            }
            else
            {
                return new SystemClass(ec.ToString(), reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt64(3),
                                reader.GetInt64(offset), reader.GetInt32(offset + 1), reader.GetInt64(offset + 2), reader.GetString(offset + 3),
                                (EDGovernment)reader.GetInt64(offset + 4), (EDAllegiance)reader.GetInt64(offset + 5), (EDState)reader.GetInt64(offset + 6), (EDSecurity)reader.GetInt64(offset + 7),
                                (EDEconomy)reader.GetInt64(offset + offset), reader.GetString(offset + 9), reader.GetString(offset + 10), reader.GetInt32(offset + 11),
                                reader.GetInt32(5), SystemStatusEnum.EDSM);
            }
        }

        #endregion

    }
}


