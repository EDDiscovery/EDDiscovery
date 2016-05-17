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
            var db = new SQLiteDBClass();
            using (DbConnection cn = db.CreateConnection())
            {
                return Add(cn);
            }
        }

        private bool Add(DbConnection cn)
        {
            using (DbCommand cmd = cn.CreateCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Insert into SystemNote (Name, Time, Note) values (@name, @time, @note)";
                cmd.AddParameterWithValue("@name", Name);
                cmd.AddParameterWithValue("@time", Time);
                cmd.AddParameterWithValue("@note", Note);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);
                SQLiteDBClass.TouchSystem(cn, Name);

                using (DbCommand cmd2 = cn.CreateCommand())
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
            var db = new SQLiteDBClass();
            using (DbConnection cn = db.CreateConnection())
            {
                return Update(cn);
            }
        }

        private bool Update(DbConnection cn)
        {
            using (DbCommand cmd = cn.CreateCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Update SystemNote set Name=@Name, Time=@Time, Note=@Note  where ID=@id";
                cmd.AddParameterWithValue("@ID", id);
                cmd.AddParameterWithValue("@Name", Name);
                cmd.AddParameterWithValue("@Note", Note);
                cmd.AddParameterWithValue("@Time", Time);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);
                SQLiteDBClass.TouchSystem(cn, Name);
                SQLiteDBClass.globalSystemNotes[Name.ToLower()] = this;

                return true;
            }
        }

    }
}
