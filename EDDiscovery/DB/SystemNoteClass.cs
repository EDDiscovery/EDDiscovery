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
                SQLiteDBClass.TouchSystem(cn, Name);

                using (SQLiteCommand cmd2 = new SQLiteCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "Select Max(id) as id from SystemNote";

                    id = (int)(long)SQLiteDBClass.SqlScalar(cn, cmd2);
                }


                SQLiteDBClass.globalSystemNotes[Name.ToLower()]= this;
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
                SQLiteDBClass.TouchSystem(cn, Name);
                SQLiteDBClass.globalSystemNotes[Name.ToLower()] = this;

                return true;
            }
        }

    }
}
