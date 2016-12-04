using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.DB
{
    public class RegisterEntry
    {
        public string ValueString { get; private set; }
        public long ValueInt { get; private set; }
        public double ValueDouble { get; private set; }
        public byte[] ValueBlob { get; private set; }

        protected RegisterEntry()
        {
        }

        public RegisterEntry(string stringval = null, byte[] blobval = null, long intval = 0, double floatval = Double.NaN)
        {
            ValueString = stringval;
            ValueBlob = blobval;
            ValueInt = intval;
            ValueDouble = floatval;
        }
    }
}
