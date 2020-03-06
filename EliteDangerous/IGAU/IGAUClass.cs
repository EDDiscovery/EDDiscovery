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
        private readonly string App_Version;
        private readonly string App_Name;

        private readonly string igau_address = "https://ddss70885k.execute-api.us-west-1.amazonaws.com/Prod";

        public IGAUClass()
        {
            httpserveraddress = igau_address;
            App_Name = "EDDiscovery";
            var assemblyFullName = Assembly.GetEntryAssembly().FullName;
            App_Version = assemblyFullName.Split(',')[1].Split('=')[1];
        }

        public string Name_Stripped { get; private set; }
        public string Name_Lower { get; private set; }

        public JObject CreateIGAUMessage(string timestamp, long EntryID, string Name, string Name_Localised, string System, long SystemAddress)
        {
            Name_Stripped = Name.Replace("$", string.Empty).Replace(";", string.Empty);
            // IGAUClass.cs(53,41,53,54): error CS1503: Argument 1: cannot convert from 'string' to 'System.Globalization.CultureInfo'
            // Name_Lower = string.ToLower(Name_Stripped);
            JObject detail = new JObject();
            detail["input_1"] = timestamp;
            detail["input_2"] = EntryID;
            //detail["input_3"] = Name;
            detail["input_3"] = Name_Stripped;
            //detail["input_3"] = Name_Lower;
            detail["input_4"] = Name_Localised;
            detail["input_5"] = System;
            detail["input_6"] = SystemAddress;
            detail["input_7"] = App_Name;
            detail["input_8"] = App_Version;
            JObject msg = new JObject();
            msg["input_values"] = detail;
            return msg;
        }

        public bool PostMessage(JObject msg, out bool recordSet)
        {
            recordSet = false;

            if (igau_address.IsEmpty())
                return false;

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
                    System.Diagnostics.Trace.WriteLine($"IGAU message post failed - status: {res["validation_messages"].ToNullSafeString()}\nIGAU Message: {msg.ToString()}");
                    return false;
                }
            }
            catch (System.Net.WebException ex)
            {
                System.Net.HttpWebResponse response = ex.Response as System.Net.HttpWebResponse;
                System.Diagnostics.Trace.WriteLine($"IGAU message post failed - status: {response?.StatusCode.ToString() ?? ex.Status.ToString()}\nIGAU Message: {msg.ToString()}");
                return false;
            }
        }
    }
}
