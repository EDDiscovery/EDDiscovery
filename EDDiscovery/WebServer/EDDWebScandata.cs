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
using QuickJSON;
using BaseUtils.WebServer;
using EliteDangerousCore;
using System;
using System.Drawing;
using System.Linq;
using System.Net;

namespace EDDiscovery.WebServer
{
    public class ScanDataRequest : IJSONNode
    {
        private EDDiscoveryForm discoveryform;

        public ScanDataRequest(EDDiscoveryForm f)
        {
            discoveryform = f;
        }

        public JToken Notify()                    // indicate changed scan data
        {
            JObject response = new JObject();
            response["responsetype"] = "scandatachanged";
            return response;
        }

        public JToken Response(string key, JToken message, HttpListenerRequest request) // request indicator state
        {
            int entry = message["entry"].Int(0);
            bool edsm = message["edsm"].Bool(false);
            bool spansh = message["spansh"].Bool(false);        // 17.0 new spansh
            System.Diagnostics.Debug.WriteLine("scandata Request " + key + " Entry" + entry + " EDSM " + edsm);
            return MakeResponse(entry, "scandata", edsm, spansh);
        }

        //EliteDangerousCore.UIEvents.UIOverallStatus status,
        public JToken MakeResponse(int entry, string type, bool edsm, bool spansh)       // entry = -1 means latest
        {
            if (discoveryform.InvokeRequired)
            {
                return (JToken)discoveryform.Invoke(new Func<JToken>(() => MakeResponse(entry, type, edsm , spansh)));
            }
            else
            {
                JObject response = new JObject();
                response["responsetype"] = type;

                var hl = discoveryform.History;
                if (hl.Count == 0)
                {
                    response["entry"] = -1;
                }
                else
                {
                    if (entry < 0 || entry >= hl.Count)
                        entry = hl.Count - 1;

                    response["entry"] = entry;

                    HistoryEntry he = hl[entry];

                    var lookup = edsm ? (spansh ? EliteDangerousCore.WebExternalDataLookup.SpanshThenEDSM : WebExternalDataLookup.EDSM) :
                                 spansh ? EliteDangerousCore.WebExternalDataLookup.Spansh : EliteDangerousCore.WebExternalDataLookup.None;

                    var scannode = discoveryform.History.StarScan.FindSystemSynchronous(he.System, lookup);  

                    var bodylist = scannode?.Bodies.ToList();       // may be null

                    response["Count"] = bodylist?.Count ?? 0;

                    JArray jbodies = new JArray();
                    foreach (var body in bodylist.EmptyIfNull())
                    {
                        JObject jo = new JObject()
                        {
                            ["NodeType"] = body.NodeType.ToString(),
                            ["FullName"] = body.BodyDesignator,
                            ["OwnName"] = body.OwnName,
                            ["CustomName"] = body.BodyName,
                            ["CustomNameOrOwnName"] = body.BodyNameOrOwnName,
                            ["Level"] = body.Level,
                            ["BodyID"] = body.BodyID,
                        };

                        JToken jdata = null;

                        if (body.ScanData != null)
                        {
                            jdata = JToken.FromObject(body.ScanData, true, null, 5, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly);
                        }
                        jo["Scan"] = jdata;

                        jbodies.Add(jo);
                    }

                    response["Bodies"] = jbodies;
                }

                return response;
            }
        }
    }

}



