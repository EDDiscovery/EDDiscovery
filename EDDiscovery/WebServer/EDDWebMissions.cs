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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;

namespace EDDiscovery.WebServer
{
    public class MissionRequest : IJSONNode
    {
        private EDDiscoveryForm discoveryform;

        public MissionRequest(EDDiscoveryForm f)
        {
            discoveryform = f;
        }

        public JToken Notify()       // a full refresh of journal history
        {
            JObject response = new JObject();
            response["responsetype"] = "missionschanged";
            return response;
        }

        public JToken Response(string key, JToken message, HttpListenerRequest request) // request indicator state
        {
            int entry = message["entry"].Int(0);
            System.Diagnostics.Debug.WriteLine("missions Request " + key + " Entry" + entry );
            return MakeResponse(entry, "missionslist");
        }

        //EliteDangerousCore.UIEvents.UIOverallStatus status,
        public JToken MakeResponse(int entry, string type)       // entry = -1 means latest
        {
            if (discoveryform.InvokeRequired)
            {
                return (JToken)discoveryform.Invoke(new Func<JToken>(() => MakeResponse(entry, type)));
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

                    List<MissionState> ml = discoveryform.History.MissionListAccumulator.GetMissionList(he.MissionList);

                    DateTime hetime = he.EventTimeUTC;

                    List<MissionState> mcurrent = MissionListAccumulator.GetAllCurrentMissions(ml, hetime);

                    JArray cur = new JArray();

                    foreach (MissionState ms in mcurrent)
                    {
                        cur.Add(MissionInfo(ms,false));
                    }

                    response["current"] = cur;

                    List<MissionState> mprev = MissionListAccumulator.GetAllExpiredMissions(ml, hetime);

                    JArray prev = new JArray();

                    foreach (MissionState ms in mprev)
                    {
                        prev.Add(MissionInfo(ms,true));
                    }

                    response["previous"] = prev;
                }

                return response;
            }
        }

        private JArray MissionInfo( MissionState ms , bool previousmissions )
        {
            JArray e = new JArray
            {
                JournalFieldNaming.ShortenMissionName(ms.Mission.LocalisedName) ,
                EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ms.Mission.EventTimeUTC).ToString(),
                EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ms.Mission.Expiry).ToString(),
                ms.OriginatingSystem + ": " + ms.OriginatingStation,
                ms.Mission.Faction,
                ms.DestinationSystemStationSettlement(),
                ms.Mission.TargetFaction,
                previousmissions ? ms.StateText() : ms.Mission.Reward.GetValueOrDefault().ToString("N0"),
                ms.MissionInfoColumn()
            };

            return e;
        }
    }

}



