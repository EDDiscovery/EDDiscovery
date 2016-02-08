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
        private bool _canSkipSlowUpdates = false;
        public Dictionary<int, EDCommander> listcmdr;
        private int currentCmdrID=0;

        SQLiteDBClass _db = new SQLiteDBClass();

        public EDDConfig()
        {
            LogIndex = DateTime.Now.ToString("yyyyMMdd");
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
                _db.PutSettingBool("EDSMDistances", value);
            }
        }

        public int CurrentCmdrID
        {
            get
            {
                return currentCmdrID;
            }

            set
            {
                currentCmdrID = value;
            }
        }

        public EDCommander CurrentCommander
        {
            get
            {
                if (listcmdr == null)
                    Update();

                return listcmdr[CurrentCmdrID];
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
                _db.PutSettingBool("EDSMLog", value);
            }
        }

        public bool CanSkipSlowUpdates
        {
            get
            {
                return _canSkipSlowUpdates;
            }
            set
            {
                _canSkipSlowUpdates = value;
                _db.PutSettingBool("CanSkipSlowUpdates", value);
            }
        }



        public void Update()
        {
            try
            {
                _useDistances = _db.GetSettingBool("EDSMDistances", false);
                _EDSMLog = _db.GetSettingBool("EDSMLog", false);
                _canSkipSlowUpdates = _db.GetSettingBool("CanSkipSlowUpdates", false);
                LoadCommanders();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("EDDConfig.Update()" + ":" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

        }


        private void LoadCommanders()
        {
            if (listcmdr == null)
                listcmdr = new Dictionary<int, EDCommander>();

            listcmdr.Clear();

            // Migrate old settigns.
            string apikey =  _db.GetSettingString("EDSMApiKey", "");
            string commanderName =  _db.GetSettingString("CommanderName", "");

           
           

            EDCommander cmdr = new EDCommander(0, _db.GetSettingString("EDCommanderName0", commanderName),  _db.GetSettingString("EDCommanderApiKey0", apikey));
            listcmdr[cmdr.Nr] = cmdr;


            for (int ii = 1; ii < 5; ii++)
            {
                cmdr = new EDCommander(ii, _db.GetSettingString("EDCommanderName"+ii.ToString(), ""), _db.GetSettingString("EDCommanderApiKey" + ii.ToString(), ""));
                listcmdr[cmdr.Nr] = cmdr;
            }

        }

        public void StoreCommanders(List<EDCommander> listcmdr)
        {
            foreach (EDCommander cmdr in listcmdr)
            {
                _db.PutSettingString("EDCommanderName" + cmdr.Nr.ToString(), cmdr.Name);
                _db.PutSettingString("EDCommanderApiKey" + cmdr.Nr.ToString(), cmdr.APIKey);
            }
        }
    }
}
