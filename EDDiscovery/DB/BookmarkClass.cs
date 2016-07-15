using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
            using (SQLiteConnectionED cn = new SQLiteConnectionED())      // open connection..
            {
                bool ret = Add(cn);
                return ret;
            }
        }

        private bool Add(SQLiteConnectionED cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into Bookmarks (StarName, x, y, z, Time, Heading, Note) values (@sname, @xp, @yp, @zp, @time, @head, @note)"))
            {
                cmd.AddParameterWithValue("@sname", StarName);
                cmd.AddParameterWithValue("@xp", x);
                cmd.AddParameterWithValue("@yp", y);
                cmd.AddParameterWithValue("@zp", z);
                cmd.AddParameterWithValue("@time", Time);
                cmd.AddParameterWithValue("@head", Heading);
                cmd.AddParameterWithValue("@note", Note);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from Bookmarks"))
                {
                    id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }

                bookmarks.Add(this);
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
            using (DbCommand cmd = cn.CreateCommand("Update Bookmarks set StarName=@sname, x = @xp, y = @yp, z = @zp, Time=@time, Heading = @head, Note=@note  where ID=@id"))
            {
                cmd.AddParameterWithValue("@ID", id);
                cmd.AddParameterWithValue("@sname", StarName);
                cmd.AddParameterWithValue("@xp", x);
                cmd.AddParameterWithValue("@yp", y);
                cmd.AddParameterWithValue("@zp", z);
                cmd.AddParameterWithValue("@time", Time);
                cmd.AddParameterWithValue("@head", Heading);
                cmd.AddParameterWithValue("@note", Note);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                bookmarks.RemoveAll(x => x.id == id);     // remove from list any containing id.
                bookmarks.Add(this);

                return true;
            }
        }

        public bool Delete()
        {
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
            {
                return Delete(cn);
            }
        }

        private bool Delete(SQLiteConnectionED cn)
        {
            using (DbCommand cmd = cn.CreateCommand("DELETE FROM Bookmarks WHERE id = @id"))
            {
                cmd.AddParameterWithValue("@id", id);
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
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("select * from Bookmarks"))
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
