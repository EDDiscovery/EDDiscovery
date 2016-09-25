using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EDDiscovery.DB
{
    public static partial class SQLiteDBClass
    {

        #region Database Schemas
        private static readonly dynamic Schema = new
        {
            EDDUser = new
            {
                #region Tables
                Tables = new
                {
                    #region Bookmarks
                    Bookmarks = new
                    {
                        id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE",
                        StarName = "TEXT",
                        x = "DOUBLE NOT NULL",
                        y = "DOUBLE NOT NULL",
                        z = "DOUBLE NOT NULL",
                        Time = "DATETIME NOT NULL",
                        Heading = "TEXT",
                        Note = "TEXT NOT NULL"
                    },
                    #endregion
                    #region Commanders
                    Commanders = new
                    {
                        Id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT",
                        Name = "TEXT NOT NULL",
                        EdsmApiKey = "TEXT NOT NULL",
                        NetLogDir = "TEXT",
                        Deleted = "INTEGER NOT NULL"
                    },
                    #endregion
                    #region Objects
                    Objects = new
                    {
                        id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE",
                        SystemName = "TEXT NOT NULL",
                        ObjectName = "TEXT NOT NULL",
                        ObjectType = "INTEGER NOT NULL",
                        ArrivalPoint = "FLOAT",
                        Gravity = "FLOAT",
                        Atmosphere = "INTEGER",
                        Vulcanism = "INTEGER",
                        Terrain = "INTEGER",
                        Carbon = "BOOL",
                        Iron = "BOOL",
                        Nickel = "BOOL",
                        Phosphorus = "BOOL",
                        Sulphur = "BOOL",
                        Arsenic = "BOOL",
                        Chromium = "BOOL",
                        Germanium = "BOOL",
                        Manganese = "BOOL",
                        Selenium = "BOOL",
                        Vanadium = "BOOL",
                        Zinc = "BOOL",
                        Zirconium = "BOOL",
                        Cadmium = "BOOL",
                        Mercury = "BOOL",
                        Molybdenum = "BOOL",
                        Niobium = "BOOL",
                        Tin = "BOOL",
                        Tungsten = "BOOL",
                        Antimony = "BOOL",
                        Polonium = "BOOL",
                        Ruthenium = "BOOL",
                        Technetium = "BOOL",
                        Tellurium = "BOOL",
                        Yttrium = "BOOL",
                        Commander = "TEXT",
                        UpdateTime = "DATETIME",
                        Status = "INTEGER",
                        Landed = "BOOL",
                        Terraform = "INTEGER"
                    },
                    #endregion
                    #region Register
                    Register = new // Added in Schema v1
                    {
                        ID = "TEXT NOT NULL PRIMARY KEY UNIQUE",
                        ValueInt = "INTEGER",
                        ValueDouble = "DOUBLE",
                        ValueString = "TEXT",
                        ValueBlob = "BLOB"
                    },
                    #endregion
                    #region Stations
                    Stations = new
                    {
                        id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT",
                        system_id = "INTEGER",
                        name = "TEXT NOT NULL",
                        max_landing_pad_size = "INTEGER",
                        distance_to_star = "INTEGER",
                        faction = "TEXT",
                        government_id = "INTEGER",
                        allegiance_id = "INTEGER",
                        state_id = "INTEGER",
                        type_id = "INTEGER",
                        has_commodities = "BOOL",
                        has_refuel = "BOOL",
                        has_repair = "BOOL",
                        has_rearm = "BOOL",
                        has_outfitting = "BOOL",
                        has_shipyard = "BOOL",
                        has_blackmarket = "BOOL",
                        eddb_updated_at = "INTEGER"
                    },
                    #endregion
                    #region station_commodities
                    station_commodities = new
                    {
                        station_id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT",
                        commodity_id = "INTEGER",
                        type = "INTEGER"
                    },
                    #endregion
                    #region SystemNote
                    SystemNote = new // v2
                    {
                        id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE",
                        Name = "TEXT NOT NULL",
                        Time = "DATETIME NOT NULL",
                        Note = "TEXT" // v4
                    },
                    #endregion
                    #region TravelLogUnit
                    TravelLogUnit = new
                    {
                        id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT",
                        type = "INTEGER NOT NULL",
                        name = "TEXT NOT NULL",
                        size = "INTEGER",
                        path = "TEXT"
                    },
                    #endregion
                    #region VisitedSystems
                    VisitedSystems = new
                    {
                        id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT",
                        Name = "TEXT NOT NULL",
                        Time = "DATETIME NOT NULL",
                        Unit = "TEXT",
                        Commander = "INTEGER",
                        Source = "INTEGER",
                        edsm_sync = "BOOL",
                        Map_colour = "INTEGER DEFAULT (-65536)",
                        Status = "BOOL",
                        X = "DOUBLE",
                        Y = "DOUBLE",
                        Z = "DOUBLE",
                        id_edsm_assigned = "INTEGER"
                    },
                    #endregion
                    #region route_systems
                    route_systems = new
                    {
                        id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT",
                        routeid = "INTEGER NOT NULL",
                        systemname = "TEXT NOT NULL"
                    },
                    #endregion
                    #region routes_expeditions
                    routes_expeditions = new
                    {
                        id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT",
                        name = "TEXT NOT NULL UNIQUE",
                        start = "DATETIME",
                        end = "DATETIME"
                    },
                    #endregion
                    #region wanted_systems
                    wanted_systems = new
                    {
                        id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT",
                        systemname = "TEXT NOT NULL UNIQUE"
                    },
                    #endregion
                },
                #endregion
                #region Unique Indexes
                UniqueIndexes = new
                {
                },
                #endregion
                #region Indexes
                Indexes = new
                {
                    TravelLogUnit_Name = "TravelLogUnit (Name)",
                    VisitedSystems_Commander = "VisitedSystems (Commander)",
                    VisitedSystems_Name = "VisitedSystems (Name)",
                    VisitedSystems_Source = "VisitedSystems (Source)",
                    VisitedSystems_Time = "VisitedSystems (Time)",
                    VisitedSystems_id_edsm_assigned = "VisitedSystems (id_edsm_assigned)",
                    VisitedSystems_position = "VisitedSystems (X, Y, Z)",
                },
                #endregion
            },
            EDDSystem = new
            {
                #region Tables
                Tables = new
                {
                    #region Distances
                    Distances = new
                    {
                        id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE",
                        NameA = "TEXT NOT NULL",
                        NameB = "TEXT NOT NULL",
                        Dist = "FLOAT NOT NULL",
                        CommanderCreate = "TEXT NOT NULL",
                        CreateTime = "DATETIME NOT NULL",
                        Status = "INTEGER NOT NULL",
                        id_edsm = "INTEGER NOT NULL"
                    },
                    #endregion
                    #region Register
                    Register = new // Added in Schema v1
                    {
                        ID = "TEXT NOT NULL PRIMARY KEY UNIQUE",
                        ValueInt = "INTEGER",
                        ValueDouble = "DOUBLE",
                        ValueString = "TEXT",
                        ValueBlob = "BLOB"
                    },
                    #endregion
                    #region SystemAliases
                    SystemAliases = new
                    {
                        id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT",
                        name = "TEXT",
                        id_edsm = "INTEGER",
                        id_edsm_mergedto = "INTEGER"
                    },
                    #endregion
                    #region SystemNames
                    SystemNames = new
                    {
                        Id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT",
                        Name = "TEXT NOT NULL",
                        EdsmId = "INTEGER NOT NULL"
                    },
                    #endregion
                    #region EdsmSystems
                    EdsmSystems = new
                    {
                        Id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT",
                        EdsmId = "INTEGER NOT NULL",
                        EddbId = "INTEGER",
                        X = "INTEGER NOT NULL",
                        Y = "INTEGER NOT NULL",
                        Z = "INTEGER NOT NULL",
                        CreateTimestamp = "INTEGER NOT NULL", // Seconds since 2015-01-01 00:00:00 UTC
                        UpdateTimestamp = "INTEGER NOT NULL", // Seconds since 2015-01-01 00:00:00 UTC
                        VersionTimestamp = "INTEGER NOT NULL", // Seconds since 2015-01-01 00:00:00 UTC
                        GridId = "INTEGER NOT NULL DEFAULT -1",
                        RandomId = "INTEGER NOT NULL DEFAULT -1"
                    },
                    #endregion
                    #region EddbSystems
                    EddbSystems = new
                    {
                        Id = "INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT",
                        Name = "TEXT NOT NULL",
                        EdsmId = "INTEGER NOT NULL",
                        EddbId = "INTEGER NOT NULL",
                        Population = "INTEGER NOT NULL",
                        Faction = "TEXT NOT NULL",
                        GovernmentId = "INTEGER NOT NULL",
                        AllegianceId = "INTEGER NOT NULL",
                        PrimaryEconomyId = "INTEGER NOT NULL",
                        Security = "INTEGER NOT NULL",
                        EddbUpdatedAt = "INTEGER NOT NULL", // Seconds since 1970-01-01 00:00:00 UTC
                        State = "INTEGER NOT NULL",
                        NeedsPermit = "INTEGER NOT NULL"
                    },
                    #endregion
                },
                #endregion
                #region Unique Indexes
                UniqueIndexes = new
                {
                    SystemAliases_id_edsm = "SystemAliases (id_edsm)"
                },
                #endregion
                #region Indexes
                Indexes = new
                {
                    DistanceName = "Distances (NameA ASC, NameB ASC)",
                    Distances_EDSM_ID_Index = "Distances (id_edsm ASC)",
                    IDX_Systems_versiondate = "Systems (versiondate ASC)",
                    StationsIndex_ID = "Stations (id ASC)",
                    StationsIndex_system_ID = "Stations (system_id ASC)",
                    StationsIndex_system_Name = "Stations (Name ASC)",
                    SystemAliases_id_edsm_mergedto = "SystemAliases (id_edsm_mergedto)",
                    SystemAliases_name = "SystemAliases (name)",
                    station_commodities_index = "station_commodities (station_id ASC, commodity_id ASC, type ASC)"
                },
                #endregion
            }
        };
        #endregion


        private static void EnumerateSchema(object schema, Action<string, object> processor)
        {
            foreach (var prop in schema.GetType().GetProperties())
            {
                processor(prop.Name, prop.GetValue(schema, new object[] { }));
            }
        }

        private static void UpdateTableSchema(SQLiteConnectionED cn, string name, object schema)
        {
            HashSet<string> columns = new HashSet<string>();

            using (DbCommand cmd = cn.CreateCommand("PRAGMA table_info(@tablename)"))
            {
                cmd.AddParameterWithValue("@tablename", name);
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(((string)reader["name"]).ToLower());
                    }
                }
            }

            EnumerateSchema(schema, (colname, coldef) =>
            {
                if (!columns.Contains(colname.ToLower()))
                {
                    string altercmd = $"ALTER TABLE {name} ADD COLUMN {colname} {coldef}";
                    System.Diagnostics.Trace.WriteLine(altercmd);
                    using (DbCommand cmd = cn.CreateCommand(altercmd))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            });
        }

        private static void CreateTable(SQLiteConnectionED cn, string name, object schema)
        {
            List<string> columndefs = new List<string>();

            EnumerateSchema(schema, (colname, colschema) =>
            {
                columndefs.Add($"{colname} {colschema}");
            });

            string createstmt = $"CREATE TABLE {name} ({String.Join(",", columndefs)})";

            System.Diagnostics.Trace.WriteLine(createstmt);

            using (DbCommand cmd = cn.CreateCommand(createstmt))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void UpdateDbSchema(SQLiteConnectionED cn, dynamic schema)
        {
            HashSet<string> tables = new HashSet<string>();
            HashSet<string> indexes = new HashSet<string>();

            using (DbCommand cmd = cn.CreateCommand("SELECT name, type FROM sqlite_master"))
            {
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if ((string)reader["type"] == "table")
                        {
                            tables.Add((string)reader["name"]);
                        }
                        else if ((string)reader["type"] == "index")
                        {
                            indexes.Add((string)reader["name"]);
                        }
                    }
                }
            }

            EnumerateSchema((object)schema.Tables, (name, tblschema) =>
            {
                if (!tables.Contains(name))
                {
                    CreateTable(cn, name, tblschema);
                }
                else
                {
                    UpdateTableSchema(cn, name, tblschema);
                }
            });
            EnumerateSchema((object)schema.UniqueIndexes, (name, idxschema) =>
            {
                if (!indexes.Contains(name))
                {
                    string idxcmd = $"CREATE UNIQUE INDEX {name} ON {idxschema}";
                    System.Diagnostics.Trace.WriteLine(idxcmd);
                    using (DbCommand cmd = cn.CreateCommand(idxcmd))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            });
            EnumerateSchema((object)schema.Indexes, (name, idxschema) =>
            {
                if (!indexes.Contains(name))
                {
                    string idxcmd = $"CREATE INDEX {name} ON {idxschema}";
                    System.Diagnostics.Trace.WriteLine(idxcmd);
                    using (DbCommand cmd = cn.CreateCommand(idxcmd))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            });
        }

        private static void UpdateSchema()
        {
            bool id_edsm_isset = false;
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
            {
                UpdateDbSchema(cn, Schema.EDDSystem);

                // Reset EDSM / EDDB update time if no systems have their id_edsm set
                using (DbCommand cmd = cn.CreateCommand("SELECT COUNT(EdsmId) FROM EdsmSystems"))
                {
                    id_edsm_isset = (long)cmd.ExecuteScalar() != 0;
                }

                if (!id_edsm_isset)
                {
                    System.Diagnostics.Trace.WriteLine("Resetting EDSM and EDDB last update time");
                    cn.PutSettingStringCN("EDSMLastSystems", "2010-01-01 00:00:00");        // force EDSM sync..
                    cn.PutSettingStringCN("EDDBSystemsTime", "0");                               // force EDDB
                    cn.PutSettingStringCN("EDSCLastDist", "2010-01-01 00:00:00");                // force distances
                }
            }

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                UpdateDbSchema(cn, Schema.EDDUser);

                // Null out any coordinates where (x,y,z) = (0,0,0) and the system is not Sol
                /*
                using (DbCommand cmd = cn.CreateCommand("Update VisitedSystems set x=null, y=null, z=null where x=0 and y=0 and z=0 and name!=\"Sol\""))
                {
                    cmd.ExecuteNonQuery();
                }
                 */
            }
        }
    }
}
