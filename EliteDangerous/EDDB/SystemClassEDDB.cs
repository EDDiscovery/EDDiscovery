using EliteDangerousCore.DB;
using Newtonsoft.Json.Linq;
using SQLLiteExtensions;
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
    public class SystemClassEDDB
    {
        static public long ParseEDDBUpdateSystems(string filename, Action<string> logline)
        {
            int updated = 0;
            int inserted = 0;

            try
            { 
                using (StreamReader sr = new StreamReader(filename))        // read directly from file..
                {
                    while (!sr.EndOfStream)
                    {
                        using (SQLExtTransactionLock<SQLiteConnectionSystem> tl = new SQLExtTransactionLock<SQLiteConnectionSystem>())
                        {
                            tl.OpenWriter();
                            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Writer))  // open the db
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
                                            string line = sr.ReadLine();
                                            if (line == null)  // End of stream
                                            {
                                                break;
                                            }

                                            JObject jo = JObject.Parse(line);

                                            ISystem system = SystemClassDB.FromEDDB(jo);

                                            if (system.HasEDDBInformation)                                  // screen out for speed any EDDB data with empty interesting fields
                                            {
                                                hasinfo++;

                                                selectCmd.Parameters["@EdsmId"].Value = system.EDSMID;     // EDDB carries EDSM ID, so find entry in dB

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

                                                updateSysCmd.Parameters["@EdsmId"].Value = system.EDSMID;
                                                updateSysCmd.Parameters["@EddbId"].Value = system.EDDBID;
                                                updateSysCmd.ExecuteNonQuery();

                                                if (eddbid != 0)
                                                {
                                                    if (updated_at != system.EDDBUpdatedAt || population != system.Population)
                                                    {
                                                        updateCmd.Parameters["@EddbId"].Value = system.EDDBID;
                                                        updateCmd.Parameters["@Name"].Value = system.Name;
                                                        updateCmd.Parameters["@Faction"].Value = system.Faction;
                                                        updateCmd.Parameters["@Population"].Value = system.Population;
                                                        updateCmd.Parameters["@GovernmentId"].Value = system.Government;
                                                        updateCmd.Parameters["@AllegianceId"].Value = system.Allegiance;
                                                        updateCmd.Parameters["@State"].Value = system.State;
                                                        updateCmd.Parameters["@Security"].Value = system.Security;
                                                        updateCmd.Parameters["@PrimaryEconomyId"].Value = system.PrimaryEconomy;
                                                        updateCmd.Parameters["@NeedsPermit"].Value = system.NeedsPermit;
                                                        updateCmd.Parameters["@EddbUpdatedAt"].Value = system.EDDBUpdatedAt;
                                                        updateCmd.Parameters["@EdsmId"].Value = system.EDSMID;
                                                        updateCmd.ExecuteNonQuery();
                                                        updated++;
                                                    }
                                                }
                                                else
                                                {
                                                    insertCmd.Parameters["@EdsmId"].Value = system.EDSMID;
                                                    insertCmd.Parameters["@EddbId"].Value = system.EDDBID;
                                                    insertCmd.Parameters["@Name"].Value = system.Name;
                                                    insertCmd.Parameters["@Faction"].Value = system.Faction;
                                                    insertCmd.Parameters["@Population"].Value = system.Population;
                                                    insertCmd.Parameters["@GovernmentId"].Value = system.Government;
                                                    insertCmd.Parameters["@AllegianceId"].Value = system.Allegiance;
                                                    insertCmd.Parameters["@State"].Value = system.State;
                                                    insertCmd.Parameters["@Security"].Value = system.Security;
                                                    insertCmd.Parameters["@PrimaryEconomyId"].Value = system.PrimaryEconomy;
                                                    insertCmd.Parameters["@NeedsPermit"].Value = system.NeedsPermit;
                                                    insertCmd.Parameters["@EddbUpdatedAt"].Value = system.EDDBUpdatedAt;
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
                                                System.Diagnostics.Trace.WriteLine("EDDB Count " + c + " Delta " + (Environment.TickCount - lasttc) + " info " + hasinfo + " update " + updated + " new " + inserted);
                                                lasttc = Environment.TickCount;
                                            }
                                        }

                                        txn.Commit();
                                    }
                                    catch
                                    {
                                        ExtendedControls.MessageBoxTheme.Show("There is a problem using the SQLite systems file." + Environment.NewLine +
                                                        "Try again, or use safe mode to delete the system file", "EDDB Sync Error");
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
                }
            }
            catch( Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Cannot read eddb file " +ex);
            }

            return updated + inserted;
        }


        // Called from EDDiscoveryController, in back thread, to determine what sync to do..

        public static void DetermineIfEDDBSyncRequired(EliteDangerousCore.EDSM.SystemClassEDSM.SystemsSyncState state)
        {
            DateTime time = EliteDangerousCore.EDDB.SystemClassEDDB.GetLastEDDBDownloadTime();

            if (DateTime.UtcNow.Subtract(time).TotalDays > 6.5)     // Get EDDB data once every week.
                state.perform_eddb_sync = true;
        }

        // Called from DoPerformSync, in back thread

        public static long PerformEDDBFullSync(Func<bool> PendingClose, Action<int, string> ReportProgress, Action<string> LogLine, Action<string> LogLineHighlight)
        {
            LogLine("Get systems from EDDB.");

            string eddbdir = Path.Combine(EliteConfigInstance.InstanceOptions.AppDataDirectory, "eddb");
            if (Directory.Exists(eddbdir))          // clean up old eddb folder
                Directory.Delete(eddbdir, true);

            string systemFileName = Path.Combine(eddbdir, Path.Combine(EliteConfigInstance.InstanceOptions.AppDataDirectory,"eddbsystems.json"));

            bool success = BaseUtils.DownloadFile.HTTPDownloadFile(EliteConfigInstance.InstanceConfig.EDDBSystemsURL, systemFileName, false, out bool newfile);

            if (success)
            {
                if (PendingClose())
                    return 0;

                LogLine("Resyncing all downloaded EDDB data with local database." + Environment.NewLine + "This will take a while.");

                long updates = ParseEDDBUpdateSystems(systemFileName, LogLineHighlight);

                LogLine("Local database updated with EDDB data, " + updates + " systems updated");
                SQLiteConnectionSystem.PutSettingString("EDDBSystemsTime", DateTime.UtcNow.Ticks.ToString());

                BaseUtils.FileHelpers.DeleteFileNoError(systemFileName);       // remove file - don't hold in storage

                GC.Collect();
                return updates;
            }
            else
                LogLineHighlight("Failed to download EDDB Systems. Will try again next run.");

            return 0;
        }

        static public DateTime GetLastEDDBDownloadTime()
        {
            string timestr = SQLiteConnectionSystem.GetSettingString("EDDBSystemsTime", "0");
            return new DateTime(Convert.ToInt64(timestr), DateTimeKind.Utc);
        }

        static public void ForceEDDBFullUpdate()
        {
            SQLiteConnectionSystem.PutSettingString("EDDBSystemsTime", "0");
        }
    }
}
