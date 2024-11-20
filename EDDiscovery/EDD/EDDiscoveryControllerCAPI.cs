/*
 * Copyright © 2015 - 2023 EDDiscovery development team
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using EliteDangerousCore;

namespace EDDiscovery
{
    public partial class EDDiscoveryController
    {

        public void DoCAPI(string station, string system, bool? allowcobramkiv)
        {
            // don't hold up the main thread, do it in a task, as its a HTTP operation

            System.Threading.Tasks.Task.Run(() =>
            {
                bool donemarket = false, doneshipyard = false;

                for (int tries = 3; tries >= 1 && (donemarket == false || doneshipyard == false); tries--)
                {
                    Thread.Sleep(10000);        // for the first go, give the CAPI servers a chance to update, for the next goes, spread out the requests

                    if (!donemarket)
                    {
                        string marketjson = FrontierCAPI.Market(out DateTime servertime);

                        if ( marketjson != null )
                        {
                            //System.IO.File.WriteAllText(@"c:\code\market.json", marketjson);

                            CAPI.Market mk = new CAPI.Market(marketjson);
                            if (mk.IsValid && station.Equals(mk.Name, StringComparison.InvariantCultureIgnoreCase))
                            {
                                System.Diagnostics.Trace.WriteLine($"CAPI got market {mk.Name}");

                                servertime = servertime.Year < 2020 ? DateTime.UtcNow : servertime;     // it may be MinDate if frontier changes something, protect

                                var entry = new EliteDangerousCore.JournalEvents.JournalEDDCommodityPrices(servertime,
                                                mk.ID, mk.Name, system, EDCommander.CurrentCmdrID, mk.Commodities);

                                var jo = entry.ToJSON();        // get json of it, and add it to the db
                                entry.Add(jo);

                                InvokeAsyncOnUiThread(() =>
                                {
                                    Debug.Assert(System.Windows.Forms.Application.MessageLoop);
                                    NewJournalEntryFromScanner(entry, null);                // then push it thru. this will cause another set of calls to NewEntry First/Second
                                                                                            // EDDN handler will pick up EDDCommodityPrices and send it.
                                });

                                donemarket = true;
                                Thread.Sleep(500);      // space the next check out a bit
                            }
                            else
                                LogLine($"MK is valid {mk.IsValid} station {mk.Name}");
                        }
                    }

                    if (!donemarket)
                    {
                        LogLine("CAPI failed to get market data" + (tries > 1 ? ", retrying" : ", give up"));
                    }

                    if (!doneshipyard)
                    {
                        string shipyardjson = FrontierCAPI.Shipyard(out DateTime servertime);

                        if (shipyardjson != null)
                        {
                            CAPI.Shipyard sh = new CAPI.Shipyard(shipyardjson);
                            
                            //System.IO.File.WriteAllText(@"c:\code\shipyard.json", shipyardjson);

                            if (sh.IsValid && station.Equals(sh.Name, StringComparison.InvariantCultureIgnoreCase))
                            {
                                System.Diagnostics.Trace.WriteLine($"CAPI got shipyard {sh.Name}");

                                servertime = servertime.Year < 2020 ? DateTime.UtcNow : servertime;     // it may be MinDate if frontier changes something, protect

                                var modules = sh.GetModules();
                                if ( modules?.Count > 0 )
                                {
                                    var list = modules.Select(x => new Tuple<long, string, long>(x.ID, x.Name.ToLowerInvariant(), x.Cost)).ToArray();
                                    var outfitting = new EliteDangerousCore.JournalEvents.JournalOutfitting(servertime, station, system, sh.ID, list, EDCommander.CurrentCmdrID);

                                    var jo = outfitting.ToJSON();        // get json of it, and add it to the db
                                    outfitting.Add(jo);

                                    InvokeAsyncOnUiThread(() =>
                                    {
                                        NewJournalEntryFromScanner(outfitting,null);                // then push it thru. this will cause another set of calls to NewEntry First/Second, then EDDN will send it
                                    });
                                }

                                var shipyard = sh.GetShips();

                                if ( shipyard?.Count > 0 && allowcobramkiv.HasValue)
                                {
                                    var list = shipyard.Select(x => new Tuple<long, string, long>(x.ID, x.Name.ToLowerInvariant(), x.BaseValue)).ToArray();
                                    var shipyardevent = new EliteDangerousCore.JournalEvents.JournalShipyard(servertime, station, system, sh.ID, list, EDCommander.CurrentCmdrID, allowcobramkiv.Value);

                                    var jo = shipyardevent.ToJSON();        // get json of it, and add it to the db
                                    shipyardevent.Add(jo);

                                    InvokeAsyncOnUiThread(() =>
                                    {
                                        NewJournalEntryFromScanner(shipyardevent,null);                // then push it thru. this will cause another set of calls to NewEntry First/Second, then EDDN will send it
                                    });
                                }

                                doneshipyard = true;
                            }
                        }
                    }

                    if (!doneshipyard)
                    {
                        LogLine("CAPI failed to get shipyard data" + (tries > 1 ? ", retrying" : ", give up"));
                    }
                }

            });

        }
    }
}
