using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2
{
    public class EDDConfig
    {
        private bool _useDistances;
        private bool _EDSMLog;
        readonly public string LogIndex;

        SQLiteDBClass db = new SQLiteDBClass();

        public EDDConfig()
        {
            LogIndex = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        }

        public bool UseDistances
        {
            get
            {
                return _useDistances;
            }

            set
            {
                _useDistances = value;
                db.PutSettingBool("EDSMDistances", value);

            }
        }

        public bool EDSMLog
        {
            get
            {
                return _EDSMLog;
            }

            set
            {
                _EDSMLog = value;
                db.PutSettingBool("EDSMLog", value);
            }
        }

        public void Update()
        {
            _useDistances = db.GetSettingBool("EDSMDistances", false);
            _EDSMLog = db.GetSettingBool("EDSMLog", false);
        }

        
    }
}
