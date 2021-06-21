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
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using BaseUtils;
using BaseUtils.JSON;
using BaseUtils.WebServer;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;

namespace EDDiscovery.WebServer
{
    public class TextsRequest : IJSONNode
    {
        private EDDiscoveryForm discoveryform;

        public TextsRequest(EDDiscoveryForm f)
        {
            discoveryform = f;
        }

        public JToken Notify()       // a full refresh of journal history
        {
            JObject response = new JObject();
            response["responsetype"] = "textschanged";
            return response;
        }

        public JToken Push()       // a full refresh of journal history
        {
            return MakeResponse(-1, 1, "textspush");
        }

        public JToken Response(string key, JToken message, HttpListenerRequest request) // request indicator state
        {
            int entry = message["entry"].Int(0);
            int length = message["length"].Int(0);
            System.Diagnostics.Debug.WriteLine("texts Request " + key + " Entry " + entry  + " Length " + length);
            return MakeResponse(entry, length, "texts");
        }

        //EliteDangerousCore.UIEvents.UIOverallStatus status,
        public JToken MakeResponse(int entry, int length, string type)       // entry = -1 means latest
        {
            if (discoveryform.InvokeRequired)
            {
                return (JToken)discoveryform.Invoke(new Func<JToken>(() => MakeResponse(entry, length, type)));
            }
            else
            {
                JObject response = new JObject();
                response["responsetype"] = type;

                var hl = discoveryform.history;
                if (hl.Count == 0)
                {
                    response["entry"] = -1;
                }
                else
                {
                    if (entry < 0 || entry >= hl.Count)
                        entry = hl.Count - 1;

                    response["entry"] = entry;

                    JArray cur = new JArray();

                    int count = 0;
                    for( int l = entry; l >= 0 && count < length; l--)
                    {
                        var he = discoveryform.history.EntryOrder()[l];
                        if ( he.EntryType == JournalTypeEnum.ReceiveText )
                        {
                            var rt = he.journalEntry as JournalReceiveText;
                            cur.Add(new JArray() { EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(rt.EventTimeUTC).ToString(),
                                                        rt.Channel, rt.FromLocalised.Alt(rt.From), rt.MessageLocalised });
                            count++;
                        }
                        else if ( he.EntryType == JournalTypeEnum.SendText)
                        {
                            var rt = he.journalEntry as JournalSendText;
                            cur.Add(new JArray() { EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(rt.EventTimeUTC).ToString(),
                                                        "Send Text", rt.To_Localised.Alt(rt.To), rt.Message });
                            count++;
                        }
                    }

                    response["entries"] = cur;
                    response["length"] = count;
                }

                return response;
            }
        }
    }

}



