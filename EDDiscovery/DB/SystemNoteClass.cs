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
        public int id;
        public string Name;
        public DateTime Time;
        public string Note;


        public SystemNoteClass()
        {
        }

        public SystemNoteClass(DataRow dr)
        {
            id = (int)(long)dr["id"];
            Name = (string)dr["Name"];
            Time = (DateTime)dr["Time"];
            Note = (string)dr["Note"];
        }


        public bool Add()
        {
            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                return Add(cn);
            }
        }

        private bool Add(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Insert into SystemNote (Name, Time, Note) values (@name, @time, @note)";
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@time", Time);
                cmd.Parameters.AddWithValue("@note", Note);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);
                SystemClass.TouchSystem(cn, Name);

                using (SQLiteCommand cmd2 = new SQLiteCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "Select Max(id) as id from SystemNote";

                    id = (int)(long)SQLiteDBClass.SqlScalar(cn, cmd2);
                }


                globalSystemNotes[Name.ToLower()]= this;
                return true;
            }
        }

        public bool Update()
        {
            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Update SystemNote set Name=@Name, Time=@Time, Note=@Note  where ID=@id";
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Note", Note);
                cmd.Parameters.AddWithValue("@Time", Time);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);
                SystemClass.TouchSystem(cn, Name);
                globalSystemNotes[Name.ToLower()] = this;

                return true;
            }
        }

        public static Dictionary<string, SystemNoteClass> globalSystemNotes = new Dictionary<string, SystemNoteClass>();

        public static bool GetAllSystemNotes()
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        DataSet ds = null;
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "select * from SystemNote";

                        ds = SQLiteDBClass.SqlQueryText(cn, cmd);
                        if (ds.Tables.Count == 0)
                        {
                            return false;
                        }
                        //
                        if (ds.Tables[0].Rows.Count == 0)
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
