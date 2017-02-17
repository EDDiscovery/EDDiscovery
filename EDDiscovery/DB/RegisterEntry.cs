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
