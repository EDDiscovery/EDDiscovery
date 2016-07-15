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
        public string Name;
        public DateTime Time;
        public string Note;


        public SystemNoteClass()
        {
        }

        public SystemNoteClass(DataRow dr)
        {
            id = (long)dr["id"];
            Name = (string)dr["Name"];
            Time = (DateTime)dr["Time"];
            Note = (string)dr["Note"];
        }


        public bool Add()
        {
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
            {
                bool ret = Add(cn);
                return ret;
            }
        }

        private bool Add(SQLiteConnectionED cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into SystemNote (Name, Time, Note) values (@name, @time, @note)"))
            {
                cmd.AddParameterWithValue("@name", Name);
                cmd.AddParameterWithValue("@time", Time);
                cmd.AddParameterWithValue("@note", Note);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from SystemNote"))
                {
                    id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }
                
                globalSystemNotes[Name.ToLower()]= this;
                return true;
            }
        }

        public bool Update()
        {
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnectionED cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Update SystemNote set Name=@Name, Time=@Time, Note=@Note  where ID=@id"))
            {
                cmd.AddParameterWithValue("@ID", id);
                cmd.AddParameterWithValue("@Name", Name);
                cmd.AddParameterWithValue("@Note", Note);
                cmd.AddParameterWithValue("@Time", Time);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);
                globalSystemNotes[Name.ToLower()] = this;

                return true;
            }
        }

        public static Dictionary<string, SystemNoteClass> globalSystemNotes = new Dictionary<string, SystemNoteClass>();

        public static bool GetAllSystemNotes()
        {
            try
            {
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
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
                            globalSystemNotes[sys.Name.ToLower()] = sys;
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

        public static string GetSystemNote(string name)      // case insensitive.. null if not there
        {
            string lname = name.ToLower();
            if (globalSystemNotes.ContainsKey(lname))
                return globalSystemNotes[lname].Note;
            else
                return null;
        }

        public static string GetSystemNoteOrEmpty(string name)      // case insensitive.. empty string if not there
        {
            string lname = name.ToLower();
            if (globalSystemNotes.ContainsKey(lname))
                return globalSystemNotes[lname].Note;
            else
                return "";
        }

        public static SystemNoteClass GetSystemNoteClass(string name)      // case insensitive.. null if not there
        {
            string lname = name.ToLower();
            if (globalSystemNotes.ContainsKey(lname))
                return globalSystemNotes[lname];
            else
                return null;
        }

    }
}
