using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Data;
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
            using (IDbConnection cn = SQLiteDBClass.CreateConnection())
            {
                return Add(cn);
            }
        }

        private bool Add(IDbConnection cn)
        {
            using (IDbCommand cmd = cn.CreateCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Insert into SystemNote (Name, Time, Note) values (@name, @time, @note)";
                SQLiteDBClass.AddParameter(cmd, "@name", Name);
                SQLiteDBClass.AddParameter(cmd, "@time", Time);
                SQLiteDBClass.AddParameter(cmd, "@note", Note);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                using (IDbCommand cmd2 = cn.CreateCommand())
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
            using (IDbConnection cn = SQLiteDBClass.CreateConnection())
            {
                return Update(cn);
            }
        }

        private bool Update(IDbConnection cn)
        {
            using (IDbCommand cmd = cn.CreateCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Update SystemNote set Name=@Name, Time=@Time, Note=@Note  where ID=@id";
                SQLiteDBClass.AddParameter(cmd, "@ID", id);
                SQLiteDBClass.AddParameter(cmd, "@Name", Name);
                SQLiteDBClass.AddParameter(cmd, "@Note", Note);
                SQLiteDBClass.AddParameter(cmd, "@Time", Time);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);
                SQLiteDBClass.globalSystemNotes[Name.ToLower()] = this;

                return true;
            }
        }

    }
}
