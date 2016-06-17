using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace EDDiscovery2.DB
{
    public class BookmarkClass
    {
        public int id;
        public string StarName;         // set if associated with a star, else null
        public double x;                // x/y/z always set for render purposes
        public double y;
        public double z;
        public DateTime Time;           
        public string Heading;          // set if not associated with a star, else null if its a star
        public string Note;

        public BookmarkClass()
        {
        }
        
        public BookmarkClass(DataRow dr)
        {
            id = (int)(long)dr["id"];
            if (System.DBNull.Value != dr["StarName"] )
                StarName = (string)dr["StarName"];
            x = (double)dr["x"];
            y = (double)dr["y"];
            z = (double)dr["z"];
            Time = (DateTime)dr["Time"];
            if (System.DBNull.Value != dr["Heading"])
                Heading = (string)dr["Heading"];
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
                cmd.CommandText = "Insert into Bookmarks (StarName, x, y, z, Time, Heading, Note) values (@sname, @xp, @yp, @zp, @time, @head, @note)";
                cmd.Parameters.AddWithValue("@sname", StarName);
                cmd.Parameters.AddWithValue("@xp", x);
                cmd.Parameters.AddWithValue("@yp", y);
                cmd.Parameters.AddWithValue("@zp", z);
                cmd.Parameters.AddWithValue("@time", Time);
                cmd.Parameters.AddWithValue("@head", Heading);
                cmd.Parameters.AddWithValue("@note", Note);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                using (SQLiteCommand cmd2 = new SQLiteCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "Select Max(id) as id from Bookmarks";
                    id = (int)(long)SQLiteDBClass.SqlScalar(cn, cmd2);
                }

                SQLiteDBClass.bookmarks.Add(this);
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
                cmd.CommandText = "Update Bookmarks set StarName=@sname, x = @xp, y = @yp, z = @zp, Time=@time, Heading = @head, Note=@note  where ID=@id";
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@sname", StarName);
                cmd.Parameters.AddWithValue("@xp", x);
                cmd.Parameters.AddWithValue("@yp", y);
                cmd.Parameters.AddWithValue("@zp", z);
                cmd.Parameters.AddWithValue("@time", Time);
                cmd.Parameters.AddWithValue("@head", Heading);
                cmd.Parameters.AddWithValue("@note", Note);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                SQLiteDBClass.bookmarks.RemoveAll(x => x.id == id);     // remove from list any containing id.
                SQLiteDBClass.bookmarks.Add(this);

                return true;
            }
        }

        public bool Delete()
        {
            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                return Delete(cn);
            }
        }

        private bool Delete(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "DELETE FROM Bookmarks WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                SQLiteDBClass.bookmarks.RemoveAll(x => x.id == id);     // remove from list any containing id.
                return true;
            }
        }

    }
}
