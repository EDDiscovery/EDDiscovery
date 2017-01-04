using EDDiscovery.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace EDDiscovery.Import
{
    public struct ImportSystem
    {
        public DateTime Timestamp;
        public string SysName;
        public string Notes;
    }

    public class ImportHistory
    {
        private string Delimiter;
        private int? DateCol;
        private int? TimeCol;
        private int? SysNameCol;
        private int? SysNoteCol;
        private bool HeaderRow;
        private string FileName;
        private long Commander;
        private List<ImportSystem> SystemsForImport;
        
        public ImportHistory(string fileName, string delimiter, int? dateCol, int? timeCol, int? nameCol, int? noteCol, bool header, long cmdrID)
        {
            FileName = fileName;
            Delimiter = delimiter;
            DateCol = dateCol;
            TimeCol = timeCol;
            SysNameCol = nameCol;
            SysNoteCol = noteCol;
            HeaderRow = header;
            Commander = cmdrID;
        }

        public bool Import(out string result)
        {
            if (!ReadFile())
            {
                result = "The import file could not be read with specified settings.";
                return false;
            }
            if (DateCol.HasValue)
            {
                foreach (ImportSystem sys in SystemsForImport)
                {
                    if (!ImportJump(sys))
                    {
                        result = string.Format("Travel History import failed at {0}.", sys.SysName);
                        return false;
                    }
                }
            }
            if (SysNoteCol.HasValue)
            {
                foreach (ImportSystem sys in SystemsForImport)
                {
                    if (!ImportNote(sys))
                    {
                        result = string.Format("System Note import failed at {0}.", sys.SysName);
                        return false;
                    }
                    EDDiscovery2.DB.SystemNoteClass.GetAllSystemNotes();
                }
            }
            result = "File imported successfully";
            return true;
        }

        private bool ReadFile()
        {
            SystemsForImport = new List<ImportSystem>();
            using (StreamReader sr = new StreamReader(FileName))
            {

                if (HeaderRow) sr.ReadLine();
                try
                {
                    do
                    {
                        string aLine = sr.ReadLine();
                        string[] values = aLine.Split(new string[] { Delimiter }, StringSplitOptions.None);

                        ImportSystem thisSys = new ImportSystem();

                        if (DateCol.HasValue)
                        {
                            string dateTimeInFile = values[DateCol.Value - 1];
                            if (TimeCol.HasValue)
                            {
                                dateTimeInFile = dateTimeInFile + " " + values[TimeCol.Value - 1];
                            }
                            if (!DateTime.TryParse(dateTimeInFile, out thisSys.Timestamp)) return false;
                        }

                        thisSys.SysName = values[SysNameCol.Value - 1];
                        if (SysNoteCol.HasValue)
                        { thisSys.Notes = values[SysNoteCol.Value - 1].Trim(); }
                        else { thisSys.Notes = null; }

                        SystemsForImport.Add(thisSys);
                    } while (!sr.EndOfStream);
                }
                catch
                { return false; }
            }
            return true;
        }

        private bool ImportJump(ImportSystem sys)
        {
            DateTime eventtime = DateTime.SpecifyKind((DateTime)sys.Timestamp, DateTimeKind.Local).ToUniversalTime();
            try
            {
                using (SQLiteConnectionUser conn = new SQLiteConnectionUser(true, EDDbAccessMode.Writer))
                {
                    using (DbCommand exists = conn.CreateCommand("SELECT 1 FROM JournalEntries WHERE EventTypeId = 280 AND commanderId = @com AND EventTime between @low AND @high"))
                    {
                        AddParameterWithValue(exists, "@com", Commander);
                        AddParameterWithValue(exists, "@low", eventtime.AddSeconds(-10));
                        AddParameterWithValue(exists, "@high", eventtime.AddSeconds(10));
                        DbDataReader rdr = exists.ExecuteReader();
                        if (rdr.Read()) return true;
                    }
                }
                using (SQLiteConnectionUser conn = new SQLiteConnectionUser(true, EDDbAccessMode.Writer))
                {
                    using (DbTransaction txn = conn.BeginTransaction())
                    {
                        using (DbCommand cmd = conn.CreateCommand("Insert into JournalEntries (TravelLogId,CommanderId,EventTypeId,EventType,EventTime,EventData,EdsmId,Synced) " +
                                "values (@tli,@cid,@eti,@et,@etime,@edata,@edsmid,@synced)", txn))
                        {
                            AddParameterWithValue(cmd, "@tli", 0);
                            AddParameterWithValue(cmd, "@cid", Commander);
                            AddParameterWithValue(cmd, "@eti", 280);  //EDDiscovery.EliteDangerous.JournalTypeEnum.FSDJump
                            AddParameterWithValue(cmd, "@et", "FSDJump");

                            JObject je = new JObject();
                            
                            je["timestamp"] = eventtime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
                            je["event"] = "FSDJump";
                            je["StarSystem"] = (sys.SysName);

                            AddParameterWithValue(cmd, "@etime", eventtime);
                            AddParameterWithValue(cmd, "@edata", je.ToString()); 

                            AddParameterWithValue(cmd, "@edsmid", 0);
                            AddParameterWithValue(cmd, "@synced", 0);

                            cmd.ExecuteNonQuery();
                            txn.Commit();
                            return true;
                        }
                    }
                }
            }
            catch
            { return false; }
        }

        private bool ImportNote(ImportSystem sys)
        {
            long noteID = -1;
            long journalID = 0;
            string existingNote = "";

            try
            {
                using (SQLiteConnectionUser conn = new SQLiteConnectionUser(true, EDDbAccessMode.Writer))
                {
                    using (DbCommand exists = conn.CreateCommand("SELECT id, note FROM SystemNote WHERE Name = @sys"))
                    {
                        AddParameterWithValue(exists, "@sys", sys.SysName);
                        DbDataReader rdr = exists.ExecuteReader();
                        if (rdr.Read())
                        {
                            noteID = rdr.GetInt64(0);
                            existingNote = rdr.GetString(1);
                        }
                    }
                    if (noteID < 0)
                    {
                        using (DbCommand getjournal = conn.CreateCommand(string.Format("select max(id) from journalentries where eventtypeid = 280 and eventdata like '%{0}%'", sys.SysName)))
                        {
                            DbDataReader dr = getjournal.ExecuteReader();
                            if (dr.Read())
                            {
                                journalID = dr.GetInt64(0);
                            }
                        }
                    }
                }
                using (SQLiteConnectionUser conn = new SQLiteConnectionUser(true, EDDbAccessMode.Writer))
                {
                    if (noteID > 0)
                    {
                        using (DbTransaction txn = conn.BeginTransaction())
                        {
                            using (DbCommand update = conn.CreateCommand("UPDATE SystemNote SET note = @note WHERE id = @id", txn))
                            {
                                string newNote = String.IsNullOrEmpty(existingNote) ? sys.Notes : existingNote + " | " + sys.Notes;
                                AddParameterWithValue(update, "@note", newNote);
                                AddParameterWithValue(update, "@id", noteID);
                                update.ExecuteNonQuery();
                            }
                            txn.Commit();
                        }
                    }
                    else
                    {
                        using (DbTransaction txn = conn.BeginTransaction())
                        {
                            using (DbCommand insert = conn.CreateCommand("Insert into SystemNote (Name, Time, Note, journalid, edsmid) values (@name, @time, @note, @journalid, @edsmid)", txn))
                            {
                                AddParameterWithValue(insert, "@name", sys.SysName);
                                AddParameterWithValue(insert, "@time", sys.Timestamp);
                                AddParameterWithValue(insert, "@note", sys.Notes);
                                AddParameterWithValue(insert, "@journalid", journalID);
                                AddParameterWithValue(insert, "@edsmid", -1);
                                insert.ExecuteNonQuery();
                            }
                            txn.Commit();
                        }
                    }
                }
                return true;
            }
            catch
            { return false; }
        }

        private static void AddParameterWithValue(DbCommand cmd, string name, object val)
        {
            var par = cmd.CreateParameter();
            par.ParameterName = name;
            par.Value = val;
            cmd.Parameters.Add(par);
        }
    }
}
