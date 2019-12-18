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

using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;

namespace EliteDangerousCore.IGAU
{
    public class IGAUClass : BaseUtils.HttpCom
    {
        public string commanderName;

        private readonly string IGAU_address = "https://ddss70885k.execute-api.us-west-1.amazonaws.com/Prod";

        public IGAUClass()
        {
            httpserveraddress = IGAU_address;
        }

        public JObject CreateIGAUMessage(JournalScan journal, string timestamp, string name_localised, string system)
        {
            JObject detail = new JObject();
            detail["input_1"] = timestamp;
            detail["input_2"] = name_localised;
            detail["input_3"] = system;
            JObject msg = new JObject();
            msg["input_values"] = detail;
            return msg;
        }

        public bool PostMessage(JObject msg, out bool recordSet)
        {
            recordSet = false;

            if (edcmdr.SyncToIGAU.IsEmpty() )
                return false;

            try
            {
                BaseUtils.ResponseData resp = RequestPost(msg.ToString(), "");

                JObject result = JObject.Parse(resp.Body);
                JObject res = (JObject)result["response"];

            }
            catch (System.Net.WebException ex)
            {
                System.Net.HttpWebResponse response = ex.Response as System.Net.HttpWebResponse;
                System.Diagnostics.Trace.WriteLine($"IGAU Data Transmission Failed");
                return false;
            }
        }
    }
}
