using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EDDiscovery2.DB
{
    public class SystemNoteClass
    {
        public long id;
        public long Journalid;              //Journalid = 0, Name set, system marker
        public string Name;                 //Journalid <>0, Name clear, journal marker
        public DateTime Time;
        public string Note;

        public SystemNoteClass()
        {
        }

        public SystemNoteClass(DataRow dr)
        {
            id = (long)dr["id"];
            Journalid = (long)dr["journalid"];
            Name = (string)dr["Name"];
            Time = (DateTime)dr["Time"];
            Note = (string)dr["Note"];
        }


        public bool Add()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                bool ret = Add(cn);
                return ret;
            }
        }

        private bool Add(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into SystemNote (Name, Time, Note, journalid) values (@name, @time, @note, @journalid)")) //TBD
            {
                cmd.AddParameterWithValue("@name", Name);
                cmd.AddParameterWithValue("@time", Time);
                cmd.AddParameterWithValue("@note", Note);
                cmd.AddParameterWithValue("@journalid", Journalid);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from SystemNote"))
                {
                    id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }

                globalSystemNotes.Add(this);
                return true;
            }
        }

        public bool Update()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Update SystemNote set Name=@Name, Time=@Time, Note=@Note, Journalid=@journalid  where ID=@id")) //TBD
            {
                cmd.AddParameterWithValue("@ID", id);
                cmd.AddParameterWithValue("@Name", Name);
                cmd.AddParameterWithValue("@Note", Note);
                cmd.AddParameterWithValue("@Time", Time);
                cmd.AddParameterWithValue("@journalid", Journalid);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);
            }

            GetAllSystemNotes();

            return true;
        }

        public static List<SystemNoteClass> globalSystemNotes = new List<SystemNoteClass>();

        public static bool GetAllSystemNotes()
        {
            try
            {
                using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
                {
                    using (DbCommand cmd = cn.CreateCommand("select * from SystemNote"))
                    {
                        DataSet ds = SQLiteDBClass.SQLQueryText(cn, cmd);
                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            return false;
                        }

                        globalSystemNotes.Clear();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SystemNoteClass sys = new SystemNoteClass(dr);
                            globalSystemNotes.Add(sys);
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

        public static SystemNoteClass GetNoteOnSystem(string name)      // case insensitive.. null if not there  matches journalid=0,
        {
            return globalSystemNotes.FindLast(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && x.Journalid == 0 );
        }

        public static SystemNoteClass GetNoteOnJournalEntry(long jid)   
        {
            if (jid > 0)
                return globalSystemNotes.FindLast(x => x.Journalid == jid);
            else
                return null;
        }


    }
}
