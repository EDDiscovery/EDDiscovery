using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
            using (SQLiteConnection cn = SQLiteDBClass.CreateConnection(true))
            {
                bool ret = Add(cn);
                cn.Close();
                return ret;
            }
        }

        private bool Add(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("Insert into SystemNote (Name, Time, Note) values (@name, @time, @note)",cn))
            {
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@time", Time);
                cmd.Parameters.AddWithValue("@note", Note);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (SQLiteCommand cmd2 = SQLiteDBClass.CreateCommand("Select Max(id) as id from SystemNote",cn))
                {
                    id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }
                
                globalSystemNotes[Name.ToLower()]= this;
                return true;
            }
        }

        public bool Update()
        {
            using (SQLiteConnection cn = SQLiteDBClass.CreateConnection())
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("Update SystemNote set Name=@Name, Time=@Time, Note=@Note  where ID=@id",cn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Note", Note);
                cmd.Parameters.AddWithValue("@Time", Time);

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
                using (SQLiteConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("select * from SystemNote",cn))
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
