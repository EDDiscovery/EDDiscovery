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

using QuickJSON;
using BaseUtils.WebServer;
using EliteDangerousCore;
using System;
using System.Net;

namespace EDDiscovery.WebServer
{
    public class JournalRequest : IJSONNode
    {
        private EDDiscoveryForm discoveryform;
        public JournalRequest(EDDiscoveryForm f)
        {
            discoveryform = f;
        }

        public JToken Response(string key, JToken message, HttpListenerRequest request)     // response to requesttype=journal
        {
            System.Diagnostics.Debug.WriteLine("Journal Request " + key + " Fields " + message.ToString());

            int startindex = message["start"].Int(0);
            int length = message["length"].Int(0);

            return MakeResponse(startindex, length, "journalrequest");      // responsetype = journalrequest
        }

        public JToken Refresh(int startindex, int length)       // a full refresh of journal history
        {
            return MakeResponse(startindex, length, "journalrefresh");
        }

        public JToken Push()                                    // push latest entry
        {
            return MakeResponse(-1, 1, "journalpush");
        }

        private JToken MakeResponse(int startindex, int length, string rt)     // generate a response over this range
        {
            if (discoveryform.InvokeRequired)
            {
                return (JToken)discoveryform.Invoke(new Func<JToken>(() => MakeResponse(startindex, length, rt)));
            }
            else
            {
                JToken response;

                var hl = discoveryform.history;
                if (hl.Count == 0)
                {
                    response = new JObject();
                    response["responsetype"] = rt;
                    response["firstrow"] = -1;
                }
                else
                {
                    if (startindex < 0 || startindex >= hl.Count)
                        startindex = hl.Count - 1;

                    response = NewJRec(hl, rt, startindex, length);
                }

                response["Commander"] = EDCommander.Current.Name;

                return response;
            }
        }

        public JToken NewJRec(HistoryList hl, string type, int startindex, int length)
        {
            JObject response = new JObject();
            response["responsetype"] = type;
            response["firstrow"] = startindex;

            JArray jarray = new JArray();
            for (int i = startindex; i > Math.Max(-1, startindex - length); i--)
            {
                EliteDangerousCore.HistoryEntry he = hl.EntryOrder()[i];

                JArray jent = new JArray();
                jent.Add(he.journalEntry.GetIconPackPath);
                jent.Add(he.journalEntry.EventTimeUTC);
                he.FillInformation(out string info, out string detailed);
                jent.Add(he.EventSummary);
                jent.Add(info);
                jent.Add(he.GetNoteText);
                jarray.Add(jent);
            }

            response["rows"] = jarray;
            return response;
        }
    }

}



