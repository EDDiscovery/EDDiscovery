using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EDDiscovery.DB
{
    public class JournalsClass
    {
        public long id;
        public int type;
        public string Name;
        public int CommanderId;
        public string Path;
        public int Size;


        public JournalsClass()
        {
        }

        public JournalsClass(DataRow dr)
        {
            id = (long)dr["Id"];
            Name = (string)dr["Name"];
            type = (int)(long)dr["Type"];
            Size = (int)(long)dr["Size"];
            CommanderId = (int)(long)dr["CommanderID"];
            Path = (string)dr["Path"];

        }

        public JournalsClass(DbDataReader dr)
        {
            id = (long)dr["id"];
            Name = (string)dr["Name"];
            type = (int)(long)dr["type"];
            Size = (int)(long)dr["size"];
            CommanderId = (int)(long)dr["CommanderID"];
            Path = (string)dr["Path"];
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
            using (DbCommand cmd = cn.CreateCommand("Insert into Journals (Name, type, size, CommanderID, Path) values (@name, @type, @size, @CommanderID, @Path)"))
            {
                cmd.AddParameterWithValue("@name", Name);
                cmd.AddParameterWithValue("@type", type);
                cmd.AddParameterWithValue("@size", Size);
                cmd.AddParameterWithValue("@CommanderID", CommanderId);
                cmd.AddParameterWithValue("@Path", Path);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from Journals"))
                {
                    id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }

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

        public bool Update(SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("Update Journals set Name=@Name, Type=@type, CommanderID=@CommanderID, size=@size, Path=@Path  where ID=@id", tn))
            {
                cmd.AddParameterWithValue("@ID", id);
                cmd.AddParameterWithValue("@Name", Name);
                cmd.AddParameterWithValue("@Type", type);
                cmd.AddParameterWithValue("@size", Size);
                cmd.AddParameterWithValue("@CommanderID", CommanderId);
                cmd.AddParameterWithValue("@Path", Path);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                return true;
            }
        }

        static public List<JournalsClass> GetAll()
        {
            List<JournalsClass> list = new List<JournalsClass>();

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = cn.CreateCommand("select * from Journals"))
                {
                    DataSet ds = SQLiteDBClass.SQLQueryText(cn, cmd);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return list;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        JournalsClass sys = new JournalsClass(dr);
                        list.Add(sys);
                    }

                    return list;
                }
            }
        }

        public static List<string> GetAllNames()
        {
            List<string> names = new List<string>();
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT DISTINCT Name FROM Journals"))
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            names.Add((string)reader["Name"]);
                        }
                    }
                }
            }
            return names;
        }

        public static JournalsClass Get(string name)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT * FROM Journals WHERE Name = @name ORDER BY Id DESC"))
                {
                    cmd.AddParameterWithValue("@name", name);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new JournalsClass(reader);
                        }
                    }
                }
            }

            return null;
        }

        public static bool TryGet(string name, out JournalsClass tlu)
        {
            tlu = Get(name);
            return tlu != null;
        }
    }

}
