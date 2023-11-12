/*
 * Copyright © 2019-2023 EDDiscovery development team
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
 */

using QuickJSON;
using BaseUtils.WebServer;
using EliteDangerousCore;
using EliteDangerousCore.UIEvents;
using System.Net;
using System;

namespace EDDiscovery.WebServer
{
    public class GenericUIPush 
    {

        public JToken Refresh(UIEvent stat)     // request push of new states
        {
            return NewIRec(stat, "genericui");
        }

        //
        public JToken NewIRec(UIEvent stat, string type)       // entry = -1 means latest
        {
            JObject response = new JObject();
            response["responsetype"] = type;

            JToken objjson = JToken.FromObject(stat, true, new Type[] { typeof(System.Drawing.Image) }, 5, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            response["data"] = objjson;

            System.Diagnostics.Debug.WriteLine($"Web GenericUI {response.ToString(true)}");

            return response;
        }
    }

}



