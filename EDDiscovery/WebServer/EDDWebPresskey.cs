/*
 * Copyright © 2019-2021 EDDiscovery development team
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
 *
 */

using QuickJSON;
using BaseUtils.WebServer;
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using System.Linq;

namespace EDDiscovery.WebServer
{
    public class PressKeyRequest : IJSONNode
    {
        private EDDiscoveryForm discoveryform;

        public PressKeyRequest(EDDiscoveryForm f)
        {
            discoveryform = f;
        }

        public JToken Response(string key, JToken message, HttpListenerRequest request)
        {
            System.Diagnostics.Debug.WriteLine("press key Request " + key + " Fields " + message.ToString());
            JObject response = new JObject();
            response["responsetype"] = "presskey";
            response["status"] = "400";
            int keydelay = message["keydelay"].Int(100);
            int shiftdelay = message["shiftdelay"].Int(100);
            int updelay = message["updelay"].Int(100);

            string keyname = (string)message["key"];

            var bindings = discoveryform.FrontierBindings;

            string pname = "elitedangerous64";

            var entry = bindings.FindAction(keyname, true);   // just give me keyboard bindings, thats all i can do

            if ( entry != null)
            {
                var dkplist = entry.FindKeyboardAssignment();       // must have a keyboard set 

                if (dkplist != null)
                {
                    Keys[] keys = dkplist.Select(k => k.Key.ToVkey()).ToArray();
                    if (!keys.Contains(Keys.None)) // if no errors
                    {
                        string keyseq = keys.GenerateSequence();
                        System.Diagnostics.Debug.WriteLine("Frontier " + keyname + "->" + keyseq + " to " + pname);
                        string res = BaseUtils.EnhancedSendKeys.SendToProcess(keyseq, keydelay, shiftdelay, updelay, pname);
                        response["status"] = res.HasChars() ? "100" : "400";
                    }
                    else
                        response["status"] = "400";
                }
                else
                    response["status"] = "400";
            }
            else
            {
                string res = BaseUtils.EnhancedSendKeys.SendToProcess(keyname, keydelay, shiftdelay, updelay, pname);
                response["status"] = res.HasChars() ? "100" : "400";
            }


            return response;
        }
    }
}



