using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2
{
    public class EDCommander
    {
        private int nr;
        private string name;
        private string apikey;
        private string netLogPath;

        public EDCommander(int id, string Name, string APIKey)
        {
            this.nr = id;
            this.name = Name;
            this.apikey = APIKey;
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

        public string NetLogPath
        {
            get
            {
                return netLogPath;
            }

            set
            {
                netLogPath = value;
            }
        }
    }
}
