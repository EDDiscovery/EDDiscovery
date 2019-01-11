/*
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace EliteDangerousCore.DB
{
    public class SystemNoteClass
    {
        public long id;
        public long Journalid;              //Journalid = 0, Name set, system marker OR Journalid <>0, Name set or clear, journal marker
        public string SystemName;           
        public DateTime Time;
        public string Note { get; private set; }
        public long EdsmId;

        public bool Dirty;                  // NOT DB changed but uncommitted
        public bool FSDEntry;               // is a FSD entry.. used to mark it for EDSM send purposes
        
        public static List<SystemNoteClass> globalSystemNotes = new List<SystemNoteClass>();        // global cache, kept updated

        public SystemNoteClass()
        {
        }

        public SystemNoteClass(DataRow dr)
        {
            id = (long)dr["id"];
            Journalid = (long)dr["journalid"];
            SystemName = (string)dr["Name"];
            Time = (DateTime)dr["Time"];
            Note = (string)dr["Note"];
            EdsmId = (long)dr["EdsmId"];
        }


        private bool AddToDbAndGlobal()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                bool ret = AddToDbAndGlobal(cn);
                return ret;
            }
        }

        private bool AddToDbAndGlobal(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into SystemNote (Name, Time, Note, journalid, edsmid) values (@name, @time, @note, @journalid, @edsmid)"))
            {
                cmd.AddParameterWithValue("@name", SystemName);
                cmd.AddParameterWithValue("@time", Time);
                cmd.AddParameterWithValue("@note", Note);
                cmd.AddParameterWithValue("@journalid", Journalid);
                cmd.AddParameterWithValue("@edsmid", EdsmId);

                cn.SQLNonQueryText( cmd);

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from SystemNote"))
                {
                    id = (long)cn.SQLScalar( cmd2);
                }

                globalSystemNotes.Add(this);

                Dirty = false;

                return true;
            }
        }

        private bool Update()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Update SystemNote set Name=@Name, Time=@Time, Note=@Note, Journalid=@journalid, EdsmId=@EdsmId  where ID=@id"))
            {
                cmd.AddParameterWithValue("@ID", id);
                cmd.AddParameterWithValue("@Name", SystemName);
                cmd.AddParameterWithValue("@Note", Note);
                cmd.AddParameterWithValue("@Time", Time);
                cmd.AddParameterWithValue("@journalid", Journalid);
                cmd.AddParameterWithValue("@EdsmId", EdsmId);

                cn.SQLNonQueryText( cmd);

                Dirty = false;
            }

            return true;
        }

        public bool Delete()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return Delete(cn);
            }
        }

        private bool Delete(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("DELETE FROM SystemNote WHERE id = @id"))
            {
                cmd.AddParameterWithValue("@id", id);
                cn.SQLNonQueryText( cmd);

                globalSystemNotes.RemoveAll(x => x.id == id);     // remove from list any containing id.
                return true;
            }
        }

        // we update our note, time, edsmid and set dirty true.  If on a commit, we write.
        // if commit = true, we write the note to the db, which clears the dirty flag
        public SystemNoteClass UpdateNote(string s, bool commit , DateTime time , long edsmid , bool fsdentry )
        {
            Note = s;
            Time = time;
            EdsmId = edsmid;
            FSDEntry = fsdentry;

            Dirty = true;

            if (commit)
            {
                if (s.Length == 0)        // empty ones delete the note
                {
                    System.Diagnostics.Debug.WriteLine("Delete note " + Journalid + " " + SystemName + " " + Note);
                    Delete();           // delete and remove notes..
                    return null;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Update note " + Journalid + " " + SystemName + " " + Note);
                    Update();
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Note edit in memory " + Journalid + " " + SystemName + " " + Note);
            }

            return this;
        }

        public static void ClearEDSMID()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                using (DbCommand cmd = cn.CreateCommand("UPDATE SystemNote SET EdsmId=0"))
                {
                    cn.SQLNonQueryText( cmd);
                }
            }
        }

        public static bool GetAllSystemNotes()
        {
            try
            {
                using (SQLiteConnectionUser cn = new SQLiteConnectionUser(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Reader))
                {
                    using (DbCommand cmd = cn.CreateCommand("select * from SystemNote"))
                    {
                        DataSet ds = cn.SQLQueryText( cmd);
                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            return false;
                        }

                        globalSystemNotes.Clear();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SystemNoteClass sys = new SystemNoteClass(dr);
                            globalSystemNotes.Add(sys);
                            //System.Diagnostics.Debug.WriteLine("Global note " + sys.Journalid + " " + sys.SystemName + " " + sys.Note);
                        }

                        return true;

                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static void CommitDirtyNotes( Action<SystemNoteClass> actionondirty )   // can be null
        {
            foreach (SystemNoteClass snc in globalSystemNotes)
            {
                if (snc.Dirty)
                {
                    System.Diagnostics.Debug.WriteLine("Commit dirty note " + snc.Journalid + " " + snc.SystemName + " " + snc.Note);
                    snc.Update();       // clears the dirty flag
                    actionondirty?.Invoke(snc);     // pass back in case it needs to do something with it
                }
            }
        }

        public static SystemNoteClass GetNoteOnSystem(string name, long edsmid = -1)      // case insensitive.. null if not there
        {
            return globalSystemNotes.FindLast(x => x.SystemName.Equals(name, StringComparison.InvariantCultureIgnoreCase) && (edsmid <= 0 || x.EdsmId <= 0 || x.EdsmId == edsmid));
        }

        public static SystemNoteClass GetNoteOnJournalEntry(long jid)
        {
            if (jid > 0)
                return globalSystemNotes.FindLast(x => x.Journalid == jid);
            else
                return null;
        }

        public static SystemNoteClass GetSystemNote(long journalid, bool fsd, ISystem sys)
        {
            SystemNoteClass systemnote = SystemNoteClass.GetNoteOnJournalEntry(journalid);

            if (systemnote == null && fsd)      // this is for older system name notes
            {
                systemnote = SystemNoteClass.GetNoteOnSystem(sys.Name, sys.EDSMID);

                if (systemnote != null)      // if found..
                {
                    if (sys.EDSMID > 0 && systemnote.EdsmId <= 0)    // if we have a system id, but snc not set, update it for next time.
                    {
                        systemnote.EdsmId = sys.EDSMID;
                        systemnote.Dirty = true;
                    }
                }
            }

            if (systemnote != null)
            {
//                System.Diagnostics.Debug.WriteLine("HE " + Journalid + " Found note " + +snc.Journalid + " " + snc.SystemName + " " + snc.Note);
            }

            return systemnote;
        }

        public static SystemNoteClass MakeSystemNote(string text, DateTime time, string sysname, long journalid, long edsmid , bool fsdentry )
        {
            SystemNoteClass sys = new SystemNoteClass();
            sys.Note = text;
            sys.Time = time;
            sys.SystemName = sysname;
            sys.Journalid = journalid;                          // any new ones gets a journal id, making the Get always lock it to a journal entry
            sys.EdsmId = edsmid;
            sys.FSDEntry = fsdentry;
            sys.AddToDbAndGlobal();  // adds it to the global cache AND the db
            System.Diagnostics.Debug.WriteLine("made note " + sys.Journalid + " " + sys.SystemName + " " + sys.Note);
            return sys;
        }
    }
}
