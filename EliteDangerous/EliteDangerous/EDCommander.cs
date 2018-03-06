﻿/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Data;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EliteDangerousCore.DB;

namespace EliteDangerousCore
{

    /// <summary>
    /// The active commander has been changed event.
    /// </summary>
    public class CurrentCommanderChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The newly active commander.
        /// </summary>
        public EDCommander Commander { get; protected set; }

        /// <summary>
        /// The index of the commander in the list.
        /// </summary>
        public int Index { get; protected set; }

        /// <summary>
        /// Constructs a new CurrentCommanderChangedEventArgs class in preparation to send it off in an event.
        /// </summary>
        /// <param name="index">The index of the commander in the list.</param>
        public CurrentCommanderChangedEventArgs(int index)
        {
            Index = index;
            Commander = EDCommander.GetCommander(index);
        }
    }

    public class EDCommander
    {
        #region Static interface

        #region Events

        /// <summary>
        /// The current commander changed event handler.
        /// </summary>
        public static event EventHandler<CurrentCommanderChangedEventArgs> CurrentCommanderChanged;

        #endregion

        #region Properties

        /// <summary>
        /// ID of the current commander
        /// </summary>
        public static int CurrentCmdrID
        {
            get
            {
                if (_CurrentCommanderID == Int32.MinValue)
                {
                    _CurrentCommanderID = SQLiteConnectionUser.GetSettingInt("ActiveCommander", 0);
                }

                if (_CurrentCommanderID >= 0 && !_Commanders.ContainsKey(_CurrentCommanderID) && _Commanders.Count != 0)
                {
                    _CurrentCommanderID = _Commanders.Values.First().Nr;
                }

                return _CurrentCommanderID;
            }
            set
            {
                if (value != _CurrentCommanderID)
                {
                    if (!_Commanders.ContainsKey(value))
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    _CurrentCommanderID = value;
                    SQLiteConnectionUser.PutSettingInt("ActiveCommander", value);
                    OnCommanderChangedEvent(value);
                }
            }
        }

        /// <summary>
        /// The current commander.
        /// </summary>
        public static EDCommander Current
        {
            get
            {
                return _Commanders[CurrentCmdrID];
            }
        }

        /// <summary>
        /// The number of commanders.
        /// </summary>
        public static int NumberOfCommanders
        {
            get
            {
                return _Commanders.Count;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the commander with the given ID
        /// </summary>
        /// <param name="nr">ID of the commander</param>
        /// <returns>The requested commander</returns>
        public static EDCommander GetCommander(int nr)
        {
            if (_Commanders.ContainsKey(nr))
            {
                return _Commanders[nr];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves the commander with the given ID
        /// </summary>
        /// <param name="nr">ID of the commander</param>
        /// <returns>The requested commander</returns>
        public static EDCommander GetCommander(string name)
        {
            return _Commanders.Values.FirstOrDefault(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool IsCommanderPresent(string name)
        {
            return _Commanders.Values.ToList().FindIndex(x=>x.Name.Equals(name,StringComparison.InvariantCultureIgnoreCase)) != -1;
        }

        /// <summary>
        /// Returns all of the commanders
        /// </summary>
        /// <returns>All commanders</returns>
        public static IEnumerable<EDCommander> GetAll()
        {
            return _Commanders.Values;
        }

        /// <summary>
        /// Returns list of commanders
        /// </summary>
        /// <returns></returns>
        public static List<EDCommander> GetList()
        {
            return _Commanders.Values.OrderBy(v => v.Nr).ToList();
        }

        /// <summary>
        /// Delete a commander from backing storage and refresh instantiated list.
        /// </summary>
        /// <param name="cmdr">The commander to be deleted.</param>
        public static void Delete(int cmdrid)
        {
            _Commanders.Remove(cmdrid);

            using (SQLiteConnectionUser conn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = conn.CreateCommand("UPDATE Commanders SET Deleted = 1 WHERE Id = @Id"))
                {
                    cmd.AddParameterWithValue("@Id", cmdrid);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Delete a commander from backing storage and refresh instantiated list.
        /// </summary>
        /// <param name="cmdr">The commander to be deleted.</param>
        public static void Delete(EDCommander cmdr)
        {
            Delete(cmdr.Nr);
        }

        /// <summary>
        /// Generate a new commander with the specified parameters, save it to backing storage, and refresh the instantiated list.
        /// </summary>
        /// <param name="name">The in-game name for this commander.</param>
        /// <param name="edsmName">The name for this commander as shown on EDSM.</param>
        /// <param name="edsmApiKey">The API key to interface with EDSM.</param>
        /// <param name="journalpath">Where EDD should monitor for this commander's logs.</param>
        /// <returns>The newly-generated commander.</returns>

        public static EDCommander Create(EDCommander other )
        {
            return Create(other.name, other.EdsmName, other.APIKey, other.JournalDir, other.syncToEdsm, other.SyncFromEdsm, other.SyncToEddn, other.SyncToEGO, other.EGOName, other.EGOAPIKey, other.SyncToInara, other.InaraAPIKey);
        }

        public static EDCommander Create(string name = null, string edsmName = null, string edsmApiKey = null, string journalpath = null, 
                                        bool toedsm = true, bool fromedsm = false, bool toeddn = true, bool toego = false, string egoname = null, string egoapi = null, bool toInara= false, string inaraapi = null)
        {
            EDCommander cmdr;

            using (SQLiteConnectionUser conn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = conn.CreateCommand("INSERT INTO Commanders (Name,EdsmName,EdsmApiKey,JournalDir,Deleted, SyncToEdsm, SyncFromEdsm, SyncToEddn, NetLogDir, SyncToEGO, EGOName, EGOAPIKey, SyncToInara, InaraAPIKey ) " +
                                                          "VALUES (@Name,@EdsmName,@EdsmApiKey,@JournalDir,@Deleted, @SyncToEdsm, @SyncFromEdsm, @SyncToEddn, @NetLogDir, @SyncToEGO, @EGOName, @EGOApiKey, @SyncToInara, @InaraAPIKey )"))
                {
                    cmd.AddParameterWithValue("@Name", name ?? "");
                    cmd.AddParameterWithValue("@EdsmName", edsmName ?? name ?? "");
                    cmd.AddParameterWithValue("@EdsmApiKey", edsmApiKey ?? "");
                    cmd.AddParameterWithValue("@JournalDir", journalpath ?? "");
                    cmd.AddParameterWithValue("@Deleted", false);
                    cmd.AddParameterWithValue("@SyncToEdsm", toedsm);
                    cmd.AddParameterWithValue("@SyncFromEdsm", fromedsm);
                    cmd.AddParameterWithValue("@SyncToEddn", toeddn);
                    cmd.AddParameterWithValue("@NetLogDir", "");        // Unused field, null out
                    cmd.AddParameterWithValue("@SyncToEGO", toego);
                    cmd.AddParameterWithValue("@EGOName", egoname ?? "");
                    cmd.AddParameterWithValue("@EGOApiKey", egoapi ?? "");

                    cmd.AddParameterWithValue("@SyncToInara", toInara);
                    cmd.AddParameterWithValue("@InaraApiKey", inaraapi ?? "");

                    cmd.ExecuteNonQuery();
                }

                using (DbCommand cmd = conn.CreateCommand("SELECT Id FROM Commanders WHERE rowid = last_insert_rowid()"))
                {
                    int nr = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (DbCommand cmd = conn.CreateCommand("SELECT * FROM Commanders WHERE rowid = last_insert_rowid()"))
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        {
                            cmdr = new EDCommander(reader);
                        }
                    }
                }

                if (name == null)
                {
                    using (DbCommand cmd = conn.CreateCommand("UPDATE Commanders SET Name = @Name WHERE rowid = last_insert_rowid()"))
                    {
                        cmd.AddParameterWithValue("@Name", cmdr.Name);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            _Commanders[cmdr.Nr] = cmdr;

            return cmdr;
        }

        /// <summary>
        /// Write commander information to storage.
        /// </summary>
        /// <param name="cmdrlist">The new list of <see cref="EDCommander"/> instances.</param>
        /// <param name="reload">Whether to refresh the in-memory list after writing.</param>
        public static void Update(List<EDCommander> cmdrlist, bool reload)
        {
            using (SQLiteConnectionUser conn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = conn.CreateCommand("UPDATE Commanders SET Name=@Name, EdsmName=@EdsmName, EdsmApiKey=@EdsmApiKey, NetLogDir=@NetLogDir, JournalDir=@JournalDir, " +
                                                          "SyncToEdsm=@SyncToEdsm, SyncFromEdsm=@SyncFromEdsm, SyncToEddn=@SyncToEddn, SyncToEGO=@SyncToEGO, EGOName=@EGOName, " +
                                                          "EGOAPIKey=@EGOApiKey, InaraAPIKey=@InaraApiKey, SyncToInara=@SyncToInara WHERE Id=@Id"))
                {
                    cmd.AddParameter("@Id", DbType.Int32);
                    cmd.AddParameter("@Name", DbType.String);
                    cmd.AddParameter("@EdsmName", DbType.String);
                    cmd.AddParameter("@EdsmApiKey", DbType.String);
                    cmd.AddParameter("@NetLogDir", DbType.String);
                    cmd.AddParameter("@JournalDir", DbType.String);
                    cmd.AddParameter("@SyncToEdsm", DbType.Boolean);
                    cmd.AddParameter("@SyncFromEdsm", DbType.Boolean);
                    cmd.AddParameter("@SyncToEddn", DbType.Boolean);
                    cmd.AddParameter("@SyncToEGO", DbType.Boolean);
                    cmd.AddParameter("@EGOName", DbType.String);
                    cmd.AddParameter("@EGOApiKey", DbType.String);
                    cmd.AddParameter("@SyncToInara", DbType.Boolean);
                    cmd.AddParameter("@InaraApiKey", DbType.String);

                    foreach (EDCommander edcmdr in cmdrlist) // potential NRE
                    {
                        cmd.Parameters["@Id"].Value = edcmdr.Nr;
                        cmd.Parameters["@Name"].Value = edcmdr.Name;
                        cmd.Parameters["@EdsmName"].Value = edcmdr.EdsmName;
                        cmd.Parameters["@EdsmApiKey"].Value = edcmdr.APIKey != null ? edcmdr.APIKey : "";
                        cmd.Parameters["@NetLogDir"].Value = ""; // unused field
                        cmd.Parameters["@JournalDir"].Value = edcmdr.JournalDir != null ? edcmdr.JournalDir : "";
                        cmd.Parameters["@SyncToEdsm"].Value = edcmdr.SyncToEdsm;
                        cmd.Parameters["@SyncFromEdsm"].Value = edcmdr.SyncFromEdsm;
                        cmd.Parameters["@SyncToEddn"].Value = edcmdr.SyncToEddn;
                        cmd.Parameters["@SyncToEGO"].Value = edcmdr.SyncToEGO;
                        cmd.Parameters["@EGOName"].Value = edcmdr.EGOName != null ? edcmdr.EGOName : "";
                        cmd.Parameters["@EGOApiKey"].Value = edcmdr.EGOAPIKey != null ? edcmdr.EGOAPIKey : "";
                        cmd.Parameters["@SyncToInara"].Value = edcmdr.SyncToInara;
                        cmd.Parameters["@InaraApiKey"].Value = edcmdr.InaraAPIKey != null ? edcmdr.InaraAPIKey : "";


                        cmd.ExecuteNonQuery();

                        _Commanders[edcmdr.Nr] = edcmdr;
                    }

                    if (reload)
                        Load(true, conn);       // refresh in-memory copy
                }
            }

            // For  some people sharing their user DB between different computers and having different paths to their journals on those computers.
            JObject jo = new JObject();
            foreach (EDCommander cmdr in _commandersDict.Values)
            {
                JObject j = new JObject();
                if (cmdr.JournalDir != null)
                    jo["JournalDir"] = cmdr.JournalDir;
                jo[cmdr.Name] = j;
            }

            using (Stream stream = File.OpenWrite(Path.Combine(EliteDangerousCore.EliteConfigInstance.InstanceOptions.AppDataDirectory, "CommanderPaths.json.tmp")))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    using (JsonTextWriter jwriter = new JsonTextWriter(writer))
                    {
                        jo.WriteTo(jwriter);
                    }
                }
            }

            File.Delete(Path.Combine(EliteDangerousCore.EliteConfigInstance.InstanceOptions.AppDataDirectory, "CommanderPaths.json"));
            File.Move(Path.Combine(EliteDangerousCore.EliteConfigInstance.InstanceOptions.AppDataDirectory, "CommanderPaths.json.tmp"), 
                Path.Combine(EliteDangerousCore.EliteConfigInstance.InstanceOptions.AppDataDirectory, "CommanderPaths.json"));

        }

        /// <summary>
        /// Loads the commanders from storage
        /// </summary>
        /// <param name="write">True if any migrated commanders should be written to storage</param>
        /// <param name="conn">SQLite connection</param>
        public static void Load(bool write = true, SQLiteConnectionUser conn = null)
        {
            if (_commandersDict == null)
                _commandersDict = new Dictionary<int, EDCommander>();

            lock (_commandersDict)
            {
                _commandersDict.Clear();

                var cmdrs = SQLiteConnectionUser.GetCommanders(conn);
                int maxnr = cmdrs.Count == 0 ? 0 : cmdrs.Max(c => c.Nr);

                foreach (EDCommander cmdr in cmdrs)
                {
                    if (!cmdr.Deleted)
                    {
                        _commandersDict[cmdr.Nr] = cmdr;
                    }
                }

                if (_commandersDict.Count == 0)
                {
                    if (write)
                    {
                        Create("Jameson (Default)");
                    }
                    else
                    {
                        _commandersDict[maxnr + 1] = new EDCommander(maxnr + 1, "Jameson (Default)", "", false, false, false, false, "", "", false, "");
                    }
                }
            }

            // For  some people sharing their user DB between different computers and having different paths to their journals on those computers.
            if (File.Exists(Path.Combine(EliteDangerousCore.EliteConfigInstance.InstanceOptions.AppDataDirectory, "CommanderPaths.json")))
            {
                JObject jo;

                using (Stream stream = File.OpenRead(Path.Combine(EliteDangerousCore.EliteConfigInstance.InstanceOptions.AppDataDirectory, "CommanderPaths.json")))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        using (JsonTextReader jreader = new JsonTextReader(reader))
                        {
                            jo = JObject.Load(jreader);
                        }
                    }
                }

                foreach (var kvp in jo)
                {
                    string name = kvp.Key;
                    JObject props = kvp.Value as JObject;
                    EDCommander cmdr = GetCommander(name);
                    if (props != null && cmdr != null)
                    {
                        cmdr.JournalDir = props["JournalDir"].Str(cmdr.JournalDir);
                    }
                }
            }
        }

#endregion

#endregion

#region Private properties and methods
        private static Dictionary<int, EDCommander> _commandersDict;
        private static int _CurrentCommanderID = Int32.MinValue;

        private static Dictionary<int, EDCommander> _Commanders
        {
            get
            {
                if (_commandersDict == null)
                {
                    Load(false);
                }
                return _commandersDict;
            }
        }

        private static void OnCommanderChangedEvent(int commanderIndex)
        {
            var e = new CurrentCommanderChangedEventArgs(commanderIndex);
            CurrentCommanderChanged?.Invoke(null, e);
        }
#endregion

#region Instance
       
        private int nr;
        private bool deleted;
        private string name;
        private string edsmname;
        private string apikey;
        private string egoname;
        private string egoapikey;
        private string inaraapikey;
        private string journalDir;
        private bool syncToEdsm;
        private bool syncFromEdsm;
        private bool syncToEddn;
        private bool syncToEGO;
        private bool syncToInara;

        public EDCommander()
        {
        }

        public EDCommander(DbDataReader reader)
        {
            nr = Convert.ToInt32(reader["Id"]);
            name = Convert.ToString(reader["Name"]);
            edsmname = reader["EDSMName"] == DBNull.Value ? name : Convert.ToString(reader["EDSMName"]) ?? name;
            apikey = Convert.ToString(reader["EdsmApiKey"]);
            deleted = Convert.ToBoolean(reader["Deleted"]);
            journalDir = Convert.ToString(reader["JournalDir"]);
            egoname = Convert.ToString(reader["EGOName"]);
            egoapikey = Convert.ToString(reader["EGOAPIKey"]);
            inaraapikey = Convert.ToString(reader["InaraAPIKey"]);


            syncToEdsm = Convert.ToBoolean(reader["SyncToEdsm"]);
            syncFromEdsm = Convert.ToBoolean(reader["SyncFromEdsm"]);
            syncToEddn = Convert.ToBoolean(reader["SyncToEddn"]);
            syncToEGO = Convert.ToBoolean(reader["SyncToEGO"]);
            syncToInara = Convert.ToBoolean(reader["SyncToInara"]);
        }

        public EDCommander(int id, string Name, string APIKey, bool SyncToEDSM, bool SyncFromEdsm, bool SyncToEddn, bool SyncToEGO, string EGOName, string EGOAPIKey, bool SyncToInara, string InaraAPIKey, string edsmName = null)
        {
            this.nr = id;
            this.name = Name;
            this.edsmname = edsmName ?? Name;
            this.apikey = APIKey;
            this.syncToEdsm = SyncToEDSM;
            this.syncFromEdsm = SyncFromEdsm;
            this.syncToEddn = SyncToEddn;
            this.syncToEGO = SyncToEGO;
            this.egoname = EGOName;
            this.egoapikey = EGOAPIKey;

            this.syncToInara = SyncToInara;
            this.inaraapikey = InaraAPIKey;

        }


        public int Nr { get { return nr; }  private set { nr = value;  } }

        public string Name { get { return name; } set { name = value; } }
        public string EdsmName { get { return edsmname; } set { edsmname = value; } }
        public string APIKey { get { return apikey; } set { apikey = value; } }
        public string EGOName { get { return egoname; } set { egoname = value; } }
        public string EGOAPIKey { get { return egoapikey; } set { egoapikey = value; } }
        public string InaraAPIKey { get { return inaraapikey; } set { inaraapikey = value; } }
        public string JournalDir { get { return journalDir; } set { journalDir = value; } }
        public bool SyncToEdsm { get { return syncToEdsm; } set { syncToEdsm = value; } }
        public bool SyncFromEdsm { get { return syncFromEdsm; } set { syncFromEdsm = value; } }
        public bool SyncToEddn {  get { return syncToEddn; } set { syncToEddn = value;  } }
        public bool SyncToEGO { get { return syncToEGO; } set { syncToEGO = value; } }
        public bool SyncToInara { get { return syncToInara; } set { syncToInara = value; } }
        public bool Deleted { get { return deleted; } set { deleted = value; } }

        public string Info { get
            {
                bool isdisabled;
                bool confirmed = EliteDangerousCore.CompanionAPI.CompanionCredentials.CredentialState(Name, out isdisabled) == EliteDangerousCore.CompanionAPI.CompanionCredentials.State.CONFIRMED;
                return BaseUtils.FieldBuilder.Build(";To EDDN", syncToEddn, ";To EDSM", syncToEdsm, ";From EDSM", syncFromEdsm, ";CAPI" , confirmed, "<;(Disabled)" , isdisabled, ";To EGO", syncToEGO, ";To Inara", syncToInara);
            } }

#endregion
    }
}
