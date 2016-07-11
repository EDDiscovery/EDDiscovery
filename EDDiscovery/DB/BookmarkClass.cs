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
        public long id;
        public string StarName;         // set if associated with a star, else null
        public double x;                // x/y/z always set for render purposes
        public double y;
        public double z;
        public DateTime Time;           
        public string Heading;          // set if region bookmark, else null if its a star
        public string Note;
        public bool isRegion { get { return Heading != null; } }

        public BookmarkClass()
        {
        }
        
        public BookmarkClass(DataRow dr)
        {
            id = (long)dr["id"];
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
            using (SQLiteConnection cn = SQLiteDBClass.CreateConnection(true))      // open connection..
            {
                bool ret = Add(cn);
                cn.Close();
                return ret;
            }
        }

        private bool Add(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("Insert into Bookmarks (StarName, x, y, z, Time, Heading, Note) values (@sname, @xp, @yp, @zp, @time, @head, @note)",cn))
            {
                cmd.Parameters.AddWithValue("@sname", StarName);
                cmd.Parameters.AddWithValue("@xp", x);
                cmd.Parameters.AddWithValue("@yp", y);
                cmd.Parameters.AddWithValue("@zp", z);
                cmd.Parameters.AddWithValue("@time", Time);
                cmd.Parameters.AddWithValue("@head", Heading);
                cmd.Parameters.AddWithValue("@note", Note);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (SQLiteCommand cmd2 = SQLiteDBClass.CreateCommand("Select Max(id) as id from Bookmarks",cn))
                {
                    id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }

                bookmarks.Add(this);
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
            using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("Update Bookmarks set StarName=@sname, x = @xp, y = @yp, z = @zp, Time=@time, Heading = @head, Note=@note  where ID=@id",cn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@sname", StarName);
                cmd.Parameters.AddWithValue("@xp", x);
                cmd.Parameters.AddWithValue("@yp", y);
                cmd.Parameters.AddWithValue("@zp", z);
                cmd.Parameters.AddWithValue("@time", Time);
                cmd.Parameters.AddWithValue("@head", Heading);
                cmd.Parameters.AddWithValue("@note", Note);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                bookmarks.RemoveAll(x => x.id == id);     // remove from list any containing id.
                bookmarks.Add(this);

                return true;
            }
        }

        public bool Delete()
        {
            using (SQLiteConnection cn = SQLiteDBClass.CreateConnection())
            {
                return Delete(cn);
            }
        }

        private bool Delete(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("DELETE FROM Bookmarks WHERE id = @id",cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                bookmarks.RemoveAll(x => x.id == id);     // remove from list any containing id.
                return true;
            }
        }

        public static List<BookmarkClass> bookmarks = new List<BookmarkClass>();

        public static bool GetAllBookmarks()
        {
            try
            {
                using (SQLiteConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("select * from Bookmarks",cn))
                    {
                        DataSet ds = null;

                        ds = SQLiteDBClass.SQLQueryText(cn, cmd);

                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            return false;
                        }

                        bookmarks.Clear();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BookmarkClass bc = new BookmarkClass(dr);
                            bookmarks.Add(bc);
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
    }
}
