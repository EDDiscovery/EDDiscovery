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
        public List<EDCommander> listCommanders;
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
                if (currentCmdrID >= listCommanders.Count)
                    currentCmdrID = listCommanders.Count - 1;
                return currentCmdrID;
            }

            set
            {
                currentCmdrID = value;
                if (currentCmdrID >= listCommanders.Count)
                    currentCmdrID = listCommanders.Count - 1;
            }
        }

        public EDCommander CurrentCommander
        {
            get
            {
                if (listCommanders == null)
                    Update();

                if (currentCmdrID >= listCommanders.Count)
                    currentCmdrID = listCommanders.Count - 1;


                return listCommanders[CurrentCmdrID];
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
            if (listCommanders == null)
                listCommanders = new List<EDCommander>();

            listCommanders.Clear();

            // Migrate old settigns.
            string apikey =  _db.GetSettingString("EDSMApiKey", "");
            string commanderName =  _db.GetSettingString("CommanderName", "");

           
           

            EDCommander cmdr = new EDCommander(0, _db.GetSettingString("EDCommanderName0", commanderName),  _db.GetSettingString("EDCommanderApiKey0", apikey));
            listCommanders.Add(cmdr);


            for (int ii = 1; ii < 100; ii++)
            {
                cmdr = new EDCommander(ii, _db.GetSettingString("EDCommanderName"+ii.ToString(), ""), _db.GetSettingString("EDCommanderApiKey" + ii.ToString(), ""));
                if (!cmdr.Name.Equals(""))
                    listCommanders.Add(cmdr);
            }

        }

        public void StoreCommanders(List<EDCommander> dictcmdr)
        {
            foreach (EDCommander cmdr in dictcmdr)
            {
                _db.PutSettingString("EDCommanderName" + cmdr.Nr.ToString(), cmdr.Name);
                _db.PutSettingString("EDCommanderApiKey" + cmdr.Nr.ToString(), cmdr.APIKey);
                _db.PutSettingString("EDCommanderNetLogPath" + cmdr.Nr.ToString(), cmdr.NetLogPath);
            }

            LoadCommanders();
        }

        internal EDCommander GetNewCommander()
        {
            int maxnr = 1;
            foreach (EDCommander cmdr in listCommanders)
            {
                maxnr = Math.Max(cmdr.Nr, maxnr);
            }

            return new EDCommander(maxnr+1, "CMDR "+(maxnr + 1).ToString(), "");
        }
    }
}
