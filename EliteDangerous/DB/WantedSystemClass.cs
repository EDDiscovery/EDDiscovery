/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace EliteDangerousCore.DB
{
    public class WantedSystemClass
    {
        public long id;
        public string system;

        public WantedSystemClass()
        { }

        public WantedSystemClass(DataRow dr)
        {
            id = (long)dr["id"];
            system = (string)dr["systemname"];
        }

        public WantedSystemClass(string SystemName)
        {
            system = SystemName;
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
            using (DbCommand cmd = cn.CreateCommand("Insert into wanted_systems (systemname) values (@systemname)"))
            {
                cmd.AddParameterWithValue("@systemname", system);
                cn.SQLNonQueryText( cmd);

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from wanted_systems"))
                {
                    id = (long)cn.SQLScalar( cmd2);
                }
                return true;
            }
        }

        public bool Delete()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return Delete(cn);
            }
        }

        private bool Delete(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("DELETE FROM wanted_systems WHERE id = @id"))
            {
                cmd.AddParameterWithValue("@id", id);

                cn.SQLNonQueryText( cmd);

                return true;
            }
        }

        public static List<WantedSystemClass> GetAllWantedSystems()     // CAN return null
        {
            try
            {
                using (SQLiteConnectionUser cn = new SQLiteConnectionUser(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Reader))
                {
                    using (DbCommand cmd = cn.CreateCommand("select * from wanted_systems"))
                    {
                        DataSet ds = cn.SQLQueryText( cmd);
                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            return null;
                        }

                        List<WantedSystemClass> retVal = new List<WantedSystemClass>();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            WantedSystemClass sys = new WantedSystemClass(dr);
                            retVal.Add(sys);
                        }

                        return retVal;
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.ToString());
                return null;
            }
        }
    }
}
