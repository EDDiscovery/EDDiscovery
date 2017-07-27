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
using EDDiscovery.CompanionAPI;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;

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

namespace EDDiscovery.EGO
{
    public class EGOClass : BaseUtils.HttpCom
    {
        public string commanderName;

        private readonly string application_id = "6";
        private readonly string auth_code = "2c9b44c6ebeb84bf3772991561b1e500f7578603";
        private readonly string ego_address = "http://www.elitegalaxyonline.com/gravityformsapi/forms/47/submissions/";

        private string ego_apikey;
        private string ego_username;

        public EGOClass()
        {
            _serverAddress = ego_address;
            ego_apikey = EDCommander.Current.EGOAPIKey;
            ego_username = EDCommander.Current.EGOName;
        }

        public JObject CreateEGOMessage(JournalScan journal, string starSystem, double x, double y, double z)
        {
            JObject detail = new JObject();
            detail["input_1"] = ego_username;
            detail["input_2"] = ego_apikey;
            detail["input_3"] = application_id;
            detail["input_4"] = auth_code;
            detail["input_5"] = journal.GetJson().ToString().Replace("\"", "\\\"");
            detail["input_6"] = starSystem;
            detail["input_7"] = x;
            detail["input_8"] = y;
            detail["input_9"] = z;
            JObject msg = new JObject();
            msg["input_values"] = detail;
            return msg;
        }

        public bool PostMessage(JObject msg, ref bool recordSet)
        {
            try
            {
                BaseUtils.ResponseData resp = RequestPost(msg.ToString(), "");

                JObject result = JObject.Parse(resp.Body);
                JObject res = (JObject)result["response"];
                if ((bool)res["is_valid"])
                {
                    JObject conf = (JObject)res["confirmation_message"];
                    recordSet = (bool)conf["unique_record_holder"];
                    return true;
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine($"EGO message post failed - status: {res["validation_messages"].ToNullSafeString()}\nEGO Message: {msg.ToString()}");
                    return false;
                }
            }
            catch (System.Net.WebException ex)
            {
                System.Net.HttpWebResponse response = ex.Response as System.Net.HttpWebResponse;
                System.Diagnostics.Trace.WriteLine($"EGO message post failed - status: {response?.StatusCode.ToString() ?? ex.Status.ToString()}\nEGO Message: {msg.ToString()}");
                return false;
            }
        }
    }
}
