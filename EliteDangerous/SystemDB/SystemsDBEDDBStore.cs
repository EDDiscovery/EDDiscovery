/*
 * Copyright © 2015 - 2019 EDDiscovery development team
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

using Newtonsoft.Json;
using System;
using System.IO;
using SQLLiteExtensions;
using System.Data.Common;
using System.Data;
using Newtonsoft.Json.Linq;

namespace EliteDangerousCore.DB
{
    public partial class SystemsDB
    {
        public static long ParseEDDBJSONFile(string filename, Func<bool> cancelRequested)
        {
            using (StreamReader sr = new StreamReader(filename))         // read directly from file..
                return ParseEDDBJSON(sr, cancelRequested);
        }

        public static long ParseEDDBJSONString(string data, Func<bool> cancelRequested)
        {
            using (StringReader sr = new StringReader(data))         // read directly from file..
                return ParseEDDBJSON(sr, cancelRequested);
        }


        public static long ParseEDDBJSON(TextReader tr, Func<bool> cancelRequested)
        {
            long updated = 0;

            bool eof = false;

            while (!eof)
            {
                SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.ReaderWriter);

                SQLExtTransactionLock<SQLiteConnectionSystem> tl = new SQLExtTransactionLock<SQLiteConnectionSystem>();
                tl.OpenWriter();

                DbTransaction txn = cn.BeginTransaction();

                DbCommand selectCmd = cn.CreateSelect("EDDB","eddbupdatedat", "edsmid = @edsmid", inparas:new string[] { "edsmid:int64" }, limit:"1", tx:txn);   // 1 return matching ID

                string[] dbfields = { "edsmid", "eddbid", "eddbupdatedat", "population",
                                        "faction", "government", "allegiance", "state",
                                        "security", "primaryeconomy", "needspermit", "power",
                                        "powerstate" , "properties" };
                DbType[] dbfieldtypes = { DbType.Int64, DbType.Int64, DbType.Int64, DbType.Int64,
                                          DbType.String, DbType.Int64, DbType.Int64, DbType.Int64,
                                          DbType.Int64, DbType.Int64, DbType.Int64, DbType.String ,
                                          DbType.String ,DbType.String };

                DbCommand replaceCmd = cn.CreateReplace("EDDB", dbfields, dbfieldtypes, txn);

                while (!SQLiteConnectionSystem.IsReadWaiting)
                {
                    string line = tr.ReadLine();

                    if (line == null)  // End of stream
                    {
                        eof = true;
                        break;
                    }

                    try
                    {
                        JObject jo = JObject.Parse(line);
                        long jsonupdatedat = jo["updated_at"].Int();
                        long jsonedsmid = jo["edsm_id"].Long();
                        bool jsonispopulated = jo["is_populated"].Bool();

                        if (jsonispopulated)        // double check that the flag is set - population itself may be zero, for some systems, but its the flag we care about
                        {
                            selectCmd.Parameters[0].Value = jsonedsmid;
                            long dbupdated_at = selectCmd.ExecuteScalar<long>(0);

                            if (dbupdated_at == 0 || jsonupdatedat != dbupdated_at)
                            {
                                replaceCmd.Parameters["@edsmid"].Value = jsonedsmid;
                                replaceCmd.Parameters["@eddbid"].Value = jo["id"].Long();
                                replaceCmd.Parameters["@eddbupdatedat"].Value = jsonupdatedat;
                                replaceCmd.Parameters["@population"].Value = jo["population"].Long();
                                replaceCmd.Parameters["@faction"].Value = jo["controlling_minor_faction"].Str("Unknown");
                                replaceCmd.Parameters["@government"].Value = EliteDangerousTypesFromJSON.Government2ID(jo["government"].Str("Unknown"));
                                replaceCmd.Parameters["@allegiance"].Value = EliteDangerousTypesFromJSON.Allegiance2ID(jo["allegiance"].Str("Unknown"));

                                EDState edstate = EDState.Unknown;

                                try
                                {
                                    if (jo["states"] != null && jo["states"].HasValues)
                                    {
                                        JToken tk = jo["states"].First;     // we take the first one whatever
                                        JObject jostate = (JObject)tk;
                                        edstate = EliteDangerousTypesFromJSON.EDState2ID(jostate["name"].Str("Unknown"));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine("EDDB JSON file exception for states " + ex.ToString());
                                }

                                replaceCmd.Parameters["@state"].Value = edstate;
                                replaceCmd.Parameters["@security"].Value = EliteDangerousTypesFromJSON.EDSecurity2ID(jo["security"].Str("Unknown"));
                                replaceCmd.Parameters["@primaryeconomy"].Value = EliteDangerousTypesFromJSON.EDEconomy2ID(jo["primary_economy"].Str("Unknown"));
                                replaceCmd.Parameters["@needspermit"].Value = jo["needs_permit"].Int(0);
                                replaceCmd.Parameters["@power"].Value = jo["power"].Str("None");
                                replaceCmd.Parameters["@powerstate"].Value = jo["power_state"].Str("N/A");
                                replaceCmd.Parameters["@properties"].Value = RemoveFieldsFromJSON(jo);
                                replaceCmd.ExecuteNonQuery();
                                updated++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("EDDB JSON file exception " + ex.ToString());
                    }
                }

                txn.Commit();
                txn.Dispose();
                selectCmd.Dispose();
                replaceCmd.Dispose();
                tl.Dispose();
                cn.Dispose();
            }

            return updated;
        }

        static private string RemoveFieldsFromJSON(JObject jo)
        {
            jo.Remove("x");
            jo.Remove("y");
            jo.Remove("z");
            jo.Remove("edsm_id");
            jo.Remove("id");
            jo.Remove("name");
            jo.Remove("is_populated");
            jo.Remove("population");
            jo.Remove("government");
            jo.Remove("government_id");
            jo.Remove("allegiance");
            jo.Remove("allegiance_id");
            jo.Remove("state");
            jo.Remove("security");
            jo.Remove("security_id");
            jo.Remove("primary_economy");
            jo.Remove("primary_economy_id");
            jo.Remove("power");
            jo.Remove("power_state");
            jo.Remove("power_state_id");
            jo.Remove("needs_permit");
            jo.Remove("updated_at");
            jo.Remove("controlling_minor_faction");

            return jo.ToString(Formatting.None);
        }
    }
}


