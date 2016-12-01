using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using EDDiscovery2.HTTP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDDiscovery2.EDDB
{
    public class EDDBClass
    {
        private string stationFileName;
        private string systemFileName;
        private string commoditiesFileName;

        private string stationTempFileName;
        private string systemTempFileName;
        private string commoditiesTempFileName;

        public static Dictionary<int, Commodity> commodities;

        public string SystemFileName { get { return systemFileName; } }

        public EDDBClass()
        {
            stationFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbstations.json");
            systemFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbsystems.json");
            commoditiesFileName = Path.Combine(Tools.GetAppDataDirectory(), "commodities.json");

            stationTempFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbstationslite_temp.json");
            systemTempFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbsystems_temp.json");
            commoditiesTempFileName = Path.Combine(Tools.GetAppDataDirectory(), "commodities_temp.json");

            if (commodities == null)
                ReadCommodities();
        }

        public bool GetSystems()
        {
            if (File.Exists(stationTempFileName)) File.Delete(stationTempFileName); // migration - remove obsolete file
            return DownloadFileHandler.DownloadFile("http://robert.astronet.se/Elite/eddb/v4/systems_populated.json", systemFileName);
        }


        public bool GetCommodities()
        {
            if (File.Exists(systemTempFileName)) File.Delete(systemTempFileName); // migration - remove obsolete file
            return DownloadFileHandler.DownloadFile("http://robert.astronet.se/Elite/eddb/v4/commodities.json", commoditiesFileName);
        }


        public bool GetStationsLite()
        {
            if (File.Exists(commoditiesTempFileName)) File.Delete(commoditiesTempFileName); // migration - remove obsolete file
            return DownloadFileHandler.DownloadFile("http://robert.astronet.se/Elite/eddb/v4/stations.json", stationFileName);
        }


        private string ReadJson(string filename)
        {
            string json = null;

            try
            {
                if (!File.Exists(filename))
                    return null;

                StreamReader reader = new StreamReader(filename);
                json = reader.ReadToEnd();
                reader.Close();

                return json;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception:" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public void  ReadCommodities()
        {
            Dictionary<int, Commodity> eddbcommodities = new Dictionary<int, Commodity>();
            string json;

            try
            {
                json = ReadJson(commoditiesFileName);

                if (json == null)
                    return;

                JArray jcommodities = (JArray)JArray.Parse(json);

                if (jcommodities != null)
                {
                    foreach (JObject jo in jcommodities)
                    {
                        Commodity com = new Commodity(jo);

                        if (com != null)
                            eddbcommodities[com.id] = com;
                    }
                }

                commodities = eddbcommodities;

            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.WriteLine("ReadCommodities error: {0}" + ex.Message);
            }
        }



        public List<StationClass> ReadStations()
        {
            List<StationClass> eddbstations = new List<StationClass>();
            string json;

            json = ReadJson(stationFileName);

            if (json == null)
                return eddbstations;

            JArray systems = (JArray)JArray.Parse(json);

            if (systems != null)
            {
                foreach (JObject jo in systems)
                {
                    StationClass sys = new StationClass(jo, EDDiscovery2.DB.SystemInfoSource.EDDB);

                    if (sys != null)
                        eddbstations.Add(sys);
                }
            }

            return eddbstations;
        }
    }


    static public Commodity String2Commodity(string str)
    {

        var v = EDDBClass.commodities.FirstOrDefault(m => m.Value.name == str).Value;

        return v;
    }

    static public List<Commodity> EDCommodities2ID(JArray ja)
    {
        List<Commodity> commodity = new List<Commodity>();

        if (ja == null)
            return null;

        for (int ii = 0; ii < ja.Count; ii++)
        {
            string ecstr = ja[ii].Value<string>();

            commodity.Add(String2Commodity(ecstr));

        }
        return commodity;
    }
       static public long ParseEDDBUpdateSystems(string filename, Action<string> logline)
        {
            StreamReader sr = new StreamReader(filename);         // read directly from file..

            if (sr == null)
                return 0;

            JsonTextReader jr = new JsonTextReader(sr);

            if (jr == null)
                return 0;

            int updated = 0;
            int inserted = 0;

            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())  // open the db
            {
                DbCommand selectCmd = null;
                DbCommand insertCmd = null;
                DbCommand updateCmd = null;
                DbCommand updateSysCmd = null;

                using (DbTransaction txn = cn.BeginTransaction())
                {
                    try
                    {
                        selectCmd = cn.CreateCommand("SELECT EddbId, Population, EddbUpdatedAt FROM EddbSystems WHERE EdsmId = @EdsmId LIMIT 1", txn);   // 1 return matching ID
                        selectCmd.AddParameter("@Edsmid", DbType.Int64);

                        insertCmd = cn.CreateCommand("INSERT INTO EddbSystems (EdsmId, EddbId, Name, Faction, Population, GovernmentId, AllegianceId, State, Security, PrimaryEconomyId, NeedsPermit, EddbUpdatedAt) " +
                                                                      "VALUES (@EdsmId, @EddbId, @Name, @Faction, @Population, @GovernmentId, @AllegianceId, @State, @Security, @PrimaryEconomyid, @NeedsPermit, @EddbUpdatedAt)", txn);
                        insertCmd.AddParameter("@EdsmId", DbType.Int64);
                        insertCmd.AddParameter("@EddbId", DbType.Int64);
                        insertCmd.AddParameter("@Name", DbType.String);
                        insertCmd.AddParameter("@Faction", DbType.String);
                        insertCmd.AddParameter("@Population", DbType.Int64);
                        insertCmd.AddParameter("@GovernmentId", DbType.Int64);
                        insertCmd.AddParameter("@AllegianceId", DbType.Int64);
                        insertCmd.AddParameter("@State", DbType.Int64);
                        insertCmd.AddParameter("@Security", DbType.Int64);
                        insertCmd.AddParameter("@PrimaryEconomyId", DbType.Int64);
                        insertCmd.AddParameter("@NeedsPermit", DbType.Int64);
                        insertCmd.AddParameter("@EddbUpdatedAt", DbType.Int64);

                        updateCmd = cn.CreateCommand("UPDATE EddbSystems SET EddbId=@EddbId, Name=@Name, Faction=@Faction, Population=@Population, GovernmentId=@GovernmentId, AllegianceId=@AllegianceId, State=@State, Security=@Security, PrimaryEconomyId=@PrimaryEconomyId, NeedsPermit=@NeedsPermit, EddbUpdatedAt=@EddbUpdatedAt WHERE EdsmId=@EdsmId", txn);
                        updateCmd.AddParameter("@EdsmId", DbType.Int64);
                        updateCmd.AddParameter("@EddbId", DbType.Int64);
                        updateCmd.AddParameter("@Name", DbType.String);
                        updateCmd.AddParameter("@Faction", DbType.String);
                        updateCmd.AddParameter("@Population", DbType.Int64);
                        updateCmd.AddParameter("@GovernmentId", DbType.Int64);
                        updateCmd.AddParameter("@AllegianceId", DbType.Int64);
                        updateCmd.AddParameter("@State", DbType.Int64);
                        updateCmd.AddParameter("@Security", DbType.Int64);
                        updateCmd.AddParameter("@PrimaryEconomyId", DbType.Int64);
                        updateCmd.AddParameter("@NeedsPermit", DbType.Int64);
                        updateCmd.AddParameter("@EddbUpdatedAt", DbType.Int64);

                        updateSysCmd = cn.CreateCommand("UPDATE EdsmSystems SET EddbId=@EddbId WHERE EdsmId=@EdsmId");
                        updateSysCmd.AddParameter("@EdsmId", DbType.Int64);
                        updateSysCmd.AddParameter("@EddbId", DbType.Int64);

                        int c = 0;
                        int hasinfo = 0;
                        int lasttc = Environment.TickCount;

                        while (jr.Read())
                        {
                            if (jr.TokenType == JsonToken.StartObject)
                            {
                                JObject jo = JObject.Load(jr);

                                SystemClass system = new SystemClass(jo, SystemInfoSource.EDDB);

                                if (system.HasEDDBInformation)                                  // screen out for speed any EDDB data with empty interesting fields
                                {
                                    hasinfo++;

                                    selectCmd.Parameters["@EdsmId"].Value = system.id_edsm;     // EDDB carries EDSM ID, so find entry in dB

                                    //DEBUGif ( c > 30000 )  Console.WriteLine("EDDB ID " + system.id_eddb + " EDSM ID " + system.id_edsm + " " + system.name + " Late info system");

                                    long updated_at = 0;
                                    long population = 0;
                                    long eddbid = 0;

                                    using (DbDataReader reader1 = selectCmd.ExecuteReader())         // if found (if not, we ignore EDDB system)
                                    {
                                        if (reader1.Read())                                     // its there.. check its got the right stuff in it.
                                        {
                                            eddbid = (long)reader1["EddbId"];
                                            updated_at = (long)reader1["EddbUpdatedAt"];
                                            population = (long)reader1["Population"];
                                        }
                                    }

                                    updateSysCmd.Parameters["@EdsmId"].Value = system.id_edsm;
                                    updateSysCmd.Parameters["@EddbId"].Value = system.id_eddb;
                                    updateSysCmd.ExecuteNonQuery();

                                    if (eddbid != 0)
                                    {
                                        if (updated_at != system.eddb_updated_at || population != system.population)
                                        {
                                            updateCmd.Parameters["@EddbId"].Value = system.id_eddb;
                                            updateCmd.Parameters["@Name"].Value = system.name;
                                            updateCmd.Parameters["@Faction"].Value = system.faction;
                                            updateCmd.Parameters["@Population"].Value = system.population;
                                            updateCmd.Parameters["@GovernmentId"].Value = system.government;
                                            updateCmd.Parameters["@AllegianceId"].Value = system.allegiance;
                                            updateCmd.Parameters["@State"].Value = system.state;
                                            updateCmd.Parameters["@Security"].Value = system.security;
                                            updateCmd.Parameters["@PrimaryEconomyId"].Value = system.primary_economy;
                                            updateCmd.Parameters["@NeedsPermit"].Value = system.needs_permit;
                                            updateCmd.Parameters["@EddbUpdatedAt"].Value = system.eddb_updated_at;
                                            updateCmd.Parameters["@EdsmId"].Value = system.id_edsm;
                                            updateCmd.ExecuteNonQuery();
                                            updated++;
                                        }
                                    }
                                    else
                                    {
                                        insertCmd.Parameters["@EdsmId"].Value = system.id_edsm;
                                        insertCmd.Parameters["@EddbId"].Value = system.id_eddb;
                                        insertCmd.Parameters["@Name"].Value = system.name;
                                        insertCmd.Parameters["@Faction"].Value = system.faction;
                                        insertCmd.Parameters["@Population"].Value = system.population;
                                        insertCmd.Parameters["@GovernmentId"].Value = system.government;
                                        insertCmd.Parameters["@AllegianceId"].Value = system.allegiance;
                                        insertCmd.Parameters["@State"].Value = system.state;
                                        insertCmd.Parameters["@Security"].Value = system.security;
                                        insertCmd.Parameters["@PrimaryEconomyId"].Value = system.primary_economy;
                                        insertCmd.Parameters["@NeedsPermit"].Value = system.needs_permit;
                                        insertCmd.Parameters["@EddbUpdatedAt"].Value = system.eddb_updated_at;
                                        insertCmd.ExecuteNonQuery();
                                        inserted++;
                                    }
                                }
                                else
                                {
                                    //Console.WriteLine("EDDB ID " + system.id_eddb + " EDSM ID " + system.id_edsm + " " + system.name + " No info reject");
                                }

                                if (++c % 10000 == 0)
                                {
                                    Console.WriteLine("EDDB Count " + c + " Delta " + (Environment.TickCount - lasttc) + " info " + hasinfo + " update " + updated + " new " + inserted);
                                    lasttc = Environment.TickCount;
                                }
                            }
                        }

                        txn.Commit();
                    }
                    catch
                    {
                        MessageBox.Show("There is a problem using the EDDB systems file." + Environment.NewLine +
                                        "Please perform a manual EDDB sync (see Admin menu) next time you run the program ", "EDDB Sync Error");
                    }
                    finally
                    {
                        if (selectCmd != null) selectCmd.Dispose();
                        if (updateCmd != null) updateCmd.Dispose();
                        if (insertCmd != null) insertCmd.Dispose();
                    }
                }
            }

            return updated + inserted;
        }
    }



    static public Commodity String2Commodity(string str)
    {

        var v = EDDBClass.commodities.FirstOrDefault(m => m.Value.name == str).Value;

        return v;
    }

    static public List<Commodity> EDCommodities2ID(JArray ja)
    {
        List<Commodity> commodity = new List<Commodity>();

        if (ja == null)
            return null;

        for (int ii = 0; ii < ja.Count; ii++)
        {
            string ecstr = ja[ii].Value<string>();

            commodity.Add(String2Commodity(ecstr));

        }
        return commodity;
    }

static public long ParseEDDBUpdateSystems(string filename, Action<string> logline)
{
    StreamReader sr = new StreamReader(filename);         // read directly from file..

    if (sr == null)
        return 0;

    JsonTextReader jr = new JsonTextReader(sr);

    if (jr == null)
        return 0;

    int updated = 0;
    int inserted = 0;

    using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())  // open the db
    {
        DbCommand selectCmd = null;
        DbCommand insertCmd = null;
        DbCommand updateCmd = null;
        DbCommand updateSysCmd = null;

        using (DbTransaction txn = cn.BeginTransaction())
        {
            try
            {
                selectCmd = cn.CreateCommand("SELECT EddbId, Population, EddbUpdatedAt FROM EddbSystems WHERE EdsmId = @EdsmId LIMIT 1", txn);   // 1 return matching ID
                selectCmd.AddParameter("@Edsmid", DbType.Int64);

                insertCmd = cn.CreateCommand("INSERT INTO EddbSystems (EdsmId, EddbId, Name, Faction, Population, GovernmentId, AllegianceId, State, Security, PrimaryEconomyId, NeedsPermit, EddbUpdatedAt) " +
                                                              "VALUES (@EdsmId, @EddbId, @Name, @Faction, @Population, @GovernmentId, @AllegianceId, @State, @Security, @PrimaryEconomyid, @NeedsPermit, @EddbUpdatedAt)", txn);
                insertCmd.AddParameter("@EdsmId", DbType.Int64);
                insertCmd.AddParameter("@EddbId", DbType.Int64);
                insertCmd.AddParameter("@Name", DbType.String);
                insertCmd.AddParameter("@Faction", DbType.String);
                insertCmd.AddParameter("@Population", DbType.Int64);
                insertCmd.AddParameter("@GovernmentId", DbType.Int64);
                insertCmd.AddParameter("@AllegianceId", DbType.Int64);
                insertCmd.AddParameter("@State", DbType.Int64);
                insertCmd.AddParameter("@Security", DbType.Int64);
                insertCmd.AddParameter("@PrimaryEconomyId", DbType.Int64);
                insertCmd.AddParameter("@NeedsPermit", DbType.Int64);
                insertCmd.AddParameter("@EddbUpdatedAt", DbType.Int64);

                updateCmd = cn.CreateCommand("UPDATE EddbSystems SET EddbId=@EddbId, Name=@Name, Faction=@Faction, Population=@Population, GovernmentId=@GovernmentId, AllegianceId=@AllegianceId, State=@State, Security=@Security, PrimaryEconomyId=@PrimaryEconomyId, NeedsPermit=@NeedsPermit, EddbUpdatedAt=@EddbUpdatedAt WHERE EdsmId=@EdsmId", txn);
                updateCmd.AddParameter("@EdsmId", DbType.Int64);
                updateCmd.AddParameter("@EddbId", DbType.Int64);
                updateCmd.AddParameter("@Name", DbType.String);
                updateCmd.AddParameter("@Faction", DbType.String);
                updateCmd.AddParameter("@Population", DbType.Int64);
                updateCmd.AddParameter("@GovernmentId", DbType.Int64);
                updateCmd.AddParameter("@AllegianceId", DbType.Int64);
                updateCmd.AddParameter("@State", DbType.Int64);
                updateCmd.AddParameter("@Security", DbType.Int64);
                updateCmd.AddParameter("@PrimaryEconomyId", DbType.Int64);
                updateCmd.AddParameter("@NeedsPermit", DbType.Int64);
                updateCmd.AddParameter("@EddbUpdatedAt", DbType.Int64);

                updateSysCmd = cn.CreateCommand("UPDATE EdsmSystems SET EddbId=@EddbId WHERE EdsmId=@EdsmId");
                updateSysCmd.AddParameter("@EdsmId", DbType.Int64);
                updateSysCmd.AddParameter("@EddbId", DbType.Int64);

                int c = 0;
                int hasinfo = 0;
                int lasttc = Environment.TickCount;

                while (jr.Read())
                {
                    if (jr.TokenType == JsonToken.StartObject)
                    {
                        JObject jo = JObject.Load(jr);

                        SystemClass system = new SystemClass(jo, SystemInfoSource.EDDB);

                        if (system.HasEDDBInformation)                                  // screen out for speed any EDDB data with empty interesting fields
                        {
                            hasinfo++;

                            selectCmd.Parameters["@EdsmId"].Value = system.id_edsm;     // EDDB carries EDSM ID, so find entry in dB

                            //DEBUGif ( c > 30000 )  Console.WriteLine("EDDB ID " + system.id_eddb + " EDSM ID " + system.id_edsm + " " + system.name + " Late info system");

                            long updated_at = 0;
                            long population = 0;
                            long eddbid = 0;

                            using (DbDataReader reader1 = selectCmd.ExecuteReader())         // if found (if not, we ignore EDDB system)
                            {
                                if (reader1.Read())                                     // its there.. check its got the right stuff in it.
                                {
                                    eddbid = (long)reader1["EddbId"];
                                    updated_at = (long)reader1["EddbUpdatedAt"];
                                    population = (long)reader1["Population"];
                                }
                            }

                            updateSysCmd.Parameters["@EdsmId"].Value = system.id_edsm;
                            updateSysCmd.Parameters["@EddbId"].Value = system.id_eddb;
                            updateSysCmd.ExecuteNonQuery();

                            if (eddbid != 0)
                            {
                                if (updated_at != system.eddb_updated_at || population != system.population)
                                {
                                    updateCmd.Parameters["@EddbId"].Value = system.id_eddb;
                                    updateCmd.Parameters["@Name"].Value = system.name;
                                    updateCmd.Parameters["@Faction"].Value = system.faction;
                                    updateCmd.Parameters["@Population"].Value = system.population;
                                    updateCmd.Parameters["@GovernmentId"].Value = system.government;
                                    updateCmd.Parameters["@AllegianceId"].Value = system.allegiance;
                                    updateCmd.Parameters["@State"].Value = system.state;
                                    updateCmd.Parameters["@Security"].Value = system.security;
                                    updateCmd.Parameters["@PrimaryEconomyId"].Value = system.primary_economy;
                                    updateCmd.Parameters["@NeedsPermit"].Value = system.needs_permit;
                                    updateCmd.Parameters["@EddbUpdatedAt"].Value = system.eddb_updated_at;
                                    updateCmd.Parameters["@EdsmId"].Value = system.id_edsm;
                                    updateCmd.ExecuteNonQuery();
                                    updated++;
                                }
                            }
                            else
                            {
                                insertCmd.Parameters["@EdsmId"].Value = system.id_edsm;
                                insertCmd.Parameters["@EddbId"].Value = system.id_eddb;
                                insertCmd.Parameters["@Name"].Value = system.name;
                                insertCmd.Parameters["@Faction"].Value = system.faction;
                                insertCmd.Parameters["@Population"].Value = system.population;
                                insertCmd.Parameters["@GovernmentId"].Value = system.government;
                                insertCmd.Parameters["@AllegianceId"].Value = system.allegiance;
                                insertCmd.Parameters["@State"].Value = system.state;
                                insertCmd.Parameters["@Security"].Value = system.security;
                                insertCmd.Parameters["@PrimaryEconomyId"].Value = system.primary_economy;
                                insertCmd.Parameters["@NeedsPermit"].Value = system.needs_permit;
                                insertCmd.Parameters["@EddbUpdatedAt"].Value = system.eddb_updated_at;
                                insertCmd.ExecuteNonQuery();
                                inserted++;
                            }
                        }
                        else
                        {
                            //Console.WriteLine("EDDB ID " + system.id_eddb + " EDSM ID " + system.id_edsm + " " + system.name + " No info reject");
                        }

                        if (++c % 10000 == 0)
                        {
                            Console.WriteLine("EDDB Count " + c + " Delta " + (Environment.TickCount - lasttc) + " info " + hasinfo + " update " + updated + " new " + inserted);
                            lasttc = Environment.TickCount;
                        }
                    }
                }

                txn.Commit();
            }
            catch
            {
                MessageBox.Show("There is a problem using the EDDB systems file." + Environment.NewLine +
                                "Please perform a manual EDDB sync (see Admin menu) next time you run the program ", "EDDB Sync Error");
            }
            finally
            {
                if (selectCmd != null) selectCmd.Dispose();
                if (updateCmd != null) updateCmd.Dispose();
                if (insertCmd != null) insertCmd.Dispose();
            }
        }
    }

    return updated + inserted;
}



private void PerformEDDBFullSync(Func<bool> cancelRequested, Action<int, string> reportProgress)
{
    try
    {
        travelHistoryControl1.LogLine("Get systems from EDDB.");

        string systemFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbsystems.json");
        bool success = EDDiscovery2.HTTP.DownloadFileHandler.DownloadFile("http://robert.astronet.se/Elite/eddb/v4/systems_populated.json", systemFileName);

        if (success)
        {
            if (cancelRequested())
                return;

            travelHistoryControl1.LogLine("Resyncing all downloaded EDDB data with local database." + Environment.NewLine + "This will take a while.");

            long number = SystemClass.ParseEDDBUpdateSystems(systemFileName, travelHistoryControl1.LogLineHighlight);

            travelHistoryControl1.LogLine("Local database updated with EDDB data, " + number + " systems updated");
            SQLiteConnectionSystem.PutSettingString("EDDBSystemsTime", DateTime.UtcNow.Ticks.ToString());
        }
        else
            travelHistoryControl1.LogLineHighlight("Failed to download EDDB Systems. Will try again next run.");

        GC.Collect();
        performeddbsync = false;
    }
    catch (Exception ex)
    {
        travelHistoryControl1.LogLineHighlight("GetEDDBUpdate exception: " + ex.Message);
    }
}



}
