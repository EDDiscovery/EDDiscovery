using EliteDangerousCore.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore.EDDB
{
    class SystemClassEDDB
    {
        static public long ParseEDDBUpdateSystems(string filename, Action<string> logline)
        {
            StreamReader sr = new StreamReader(filename);         // read directly from file..

            if (sr == null)
                return 0;

            string line;

            int updated = 0;
            int inserted = 0;

            while (!sr.EndOfStream)
            {
                using (SQLiteTxnLockED<SQLiteConnectionSystem> tl = new SQLiteTxnLockED<SQLiteConnectionSystem>())
                {
                    tl.OpenWriter();
                    using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: EDDbAccessMode.Writer))  // open the db
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

                                while (!SQLiteConnectionSystem.IsReadWaiting)
                                {
                                    line = sr.ReadLine();
                                    if (line == null)  // End of stream
                                    {
                                        break;
                                    }

                                    {
                                        JObject jo = JObject.Parse(line);

                                        ISystem system = SystemClassDB.FromJson(jo, SystemInfoSource.EDDB);

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
                                ExtendedControls.MessageBoxTheme.Show("There is a problem using the EDDB systems file." + Environment.NewLine +
                                                "Please perform a manual EDDB sync (see Admin menu) next time you run the program ", "EDDB Sync Error");
                                break;
                            }
                            finally
                            {
                                if (selectCmd != null) selectCmd.Dispose();
                                if (updateCmd != null) updateCmd.Dispose();
                                if (insertCmd != null) insertCmd.Dispose();
                            }
                        }
                    }
                }
            }

            return updated + inserted;
        }

        public static void PerformEDDBFullSync(Func<bool> cancelRequested, Action<int, string> reportProgress, Action<string> logLine, Action<string> logError)
        {
            logLine("Get systems from EDDB.");

            string eddbdir = Path.Combine(EliteConfigInstance.InstanceOptions.AppDataDirectory, "eddb");
            if (!Directory.Exists(eddbdir))
                Directory.CreateDirectory(eddbdir);

            string systemFileName = Path.Combine(eddbdir, "systems_populated.jsonl");

            bool success = BaseUtils.DownloadFileHandler.DownloadFile("http://robert.astronet.se/Elite/eddb/v5/systems_populated.jsonl", systemFileName);

            if (success)
            {
                if (cancelRequested())
                    return;

                logLine("Resyncing all downloaded EDDB data with local database." + Environment.NewLine + "This will take a while.");

                long number = ParseEDDBUpdateSystems(systemFileName, logError);

                logLine("Local database updated with EDDB data, " + number + " systems updated");
                SQLiteConnectionSystem.PutSettingString("EDDBSystemsTime", DateTime.UtcNow.Ticks.ToString());
            }
            else
                logError("Failed to download EDDB Systems. Will try again next run.");

            GC.Collect();
        }


    }
}
