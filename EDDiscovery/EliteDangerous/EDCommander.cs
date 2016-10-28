using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EDDiscovery2
{
    public class EDCommander
    {
        private int nr;
        private bool deleted;
        private string name;
        private string edsmname;
        private string apikey;
        private string netLogDir;
        private string journalDir;
        private bool syncToEdsm;
        private bool syncFromEdsm;
        private bool syncToEddn;


        public EDCommander(DbDataReader reader)
        {
            nr = Convert.ToInt32(reader["Id"]);
            name = Convert.ToString(reader["Name"]);
            edsmname = reader["EDSMName"] == DBNull.Value ? name : Convert.ToString(reader["EDSMName"]) ?? name;
            apikey = Convert.ToString(reader["EdsmApiKey"]);
            deleted = Convert.ToBoolean(reader["Deleted"]);
            netLogDir = Convert.ToString(reader["NetLogDir"]);
            journalDir = Convert.ToString(reader["JournalDir"]);

            syncToEdsm = Convert.ToBoolean(reader["SyncToEdsm"]);
            syncFromEdsm = Convert.ToBoolean(reader["SyncFromEdsm"]);
            syncToEddn = Convert.ToBoolean(reader["SyncToEddn"]);

        }

        public EDCommander(int id, string Name, string APIKey, bool SyncToEDSM, bool SyncFromEdsm, bool SyncToEddn, string edsmName = null)
        {
            this.nr = id;
            this.name = Name;
            this.edsmname = edsmName ?? Name;
            this.apikey = APIKey;
            this.syncToEdsm = SyncToEDSM;
            this.syncFromEdsm = SyncFromEdsm;
            this.syncToEddn = SyncToEddn;
        }

        public int Nr
        {
            get
            {
                return nr;
            }

            set
            {
                nr = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string EdsmName
        {
            get
            {
                return edsmname;
            }
            set
            {
                edsmname = value;
            }
        }

        public string APIKey
        {
            get
            {
                return apikey;
            }

            set
            {
                apikey = value;
            }
        }

        public string NetLogDir
        {
            get
            {
                return netLogDir;
            }

            set
            {
                netLogDir = value;
            }
        }

        public string JournalDir
        {
            get
            {
                return journalDir;
            }
            set
            {
                journalDir = value;
            }
        }

        public bool SyncToEdsm
        {
            get
            {
                return syncToEdsm;
            }

            set
            {
                syncToEdsm = value;
            }
        }

        public bool SyncFromEdsm
        {
            get
            {
                return syncFromEdsm;
            }

            set
            {
                syncFromEdsm = value;
            }
        }

        public bool SyncToEddn
        {
            get
            {
                return syncToEddn;
            }

            set
            {
                syncToEddn = value;
            }
        }

        public bool Deleted
        {
            get
            {
                return deleted;
            }

            set
            {
                deleted = value;
            }
        }
    }
}
