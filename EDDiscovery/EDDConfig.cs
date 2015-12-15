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
        SQLiteDBClass db = new SQLiteDBClass();

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

        public void Update()
        {
            _useDistances = db.GetSettingBool("EDSMDistances", false);
        }

        
    }
}
