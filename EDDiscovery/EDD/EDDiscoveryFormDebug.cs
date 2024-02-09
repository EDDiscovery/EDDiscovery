/*
 * Copyright 2015-2024 EDDiscovery development team
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


using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace EDDiscovery
{
    public partial class EDDiscoveryForm
    {
        public static void PostInitDebug()
        {
            if (EDDOptions.Instance.OutputEventHelp != null)        // help for events, going to have to do this frequently, so keep
            {
                string fn = EDDOptions.Instance.OutputEventHelp;
                string colon = " : ";
                string prefix = "    ";
                int ll = 80;

                if (EDDOptions.Instance.OutputEventHelp.StartsWith("G:"))
                {
                    fn = fn.Substring(2);
                    colon = " | ";
                    ll = int.MaxValue;
                    prefix = "";
                }

                string s = "All Journal Events" + Environment.NewLine;

                var excllist = new Type[] { typeof(System.Drawing.Icon), typeof(System.Drawing.Image), typeof(System.Drawing.Bitmap), typeof(QuickJSON.JObject) };

                var infoe = BaseUtils.TypeHelpers.GetPropertyFieldNames(typeof(JournalEntry), "EventClass_", fields: true, linelen: ll, excludepropertytypes: excllist);
                foreach (var ix in infoe)
                {
                    s += prefix + ix.Name + colon + ix.Help + Environment.NewLine;
                }

                s += Environment.NewLine;

                foreach (var x in Enum.GetValues(typeof(JournalTypeEnum)))
                {
                    //var x = "CreateSuitLoadout";
                    JournalEntry je = JournalEntry.CreateJournalEntry(x.ToString(), DateTime.UtcNow);

                    if (!(je is JournalUnknown))
                    {
                        s += Environment.NewLine + "Event: " + x + Environment.NewLine;
                        var info = BaseUtils.TypeHelpers.GetPropertyFieldNames(je.GetType(), "EventClass_", excludedeclaretypes: new Type[] { typeof(JournalEntry) },
                                                            fields: true, linelen: ll, excludepropertytypes: excllist);
                        foreach (var ix in info)
                        {
                            s += prefix + ix.Name + colon + ix.Help + Environment.NewLine;
                        }
                    }
                }

                s += Environment.NewLine;
                s += Environment.NewLine + "All UI Events" + Environment.NewLine;
                var infoui = BaseUtils.TypeHelpers.GetPropertyFieldNames(typeof(UIEvent), "EventClass_", fields: true, linelen: ll, excludepropertytypes: excllist);
                foreach (var ix in infoui)
                {
                    s += prefix + ix.Name + colon + ix.Help + Environment.NewLine;
                }

                s += Environment.NewLine;

                foreach (var x in Enum.GetValues(typeof(UITypeEnum)))
                {
                    UIEvent ui = UIEvent.CreateEvent(x.ToString(), DateTime.UtcNow, false);
                    s += Environment.NewLine + "UIEvent: UI" + x + Environment.NewLine;
                    var info = BaseUtils.TypeHelpers.GetPropertyFieldNames(ui.GetType(), "EventClass_", excludedeclaretypes: new Type[] { typeof(UIEvent) },
                                        fields: true, linelen: ll, excludepropertytypes: excllist);
                    foreach (var ix in info)
                    {
                        s += prefix + ix.Name + colon + ix.Help + Environment.NewLine;
                    }
                }

                File.WriteAllText(fn, s);
            }

        }

        //   var comitems = MaterialCommodityMicroResourceType.GetCommodities(MaterialCommodityMicroResourceType.SortMethod.AlphabeticalRaresLast);

        //System.Diagnostics.Debug.WriteLine($"Post Init debug");
        //var sp = new EliteDangerousCore.Spansh.SpanshClass();


        //JToken tk = JToken.Parse(BaseUtils.FileHelpers.TryReadAllTextFromFile(@"c:\code\spanshmodtypes"));
        //JArray mt = tk["values"].Array();
        //foreach (var m in mt)
        //{
        //    string name = m.Str().Replace(" ", "").Replace("-", "_");
        //    System.Diagnostics.Debug.Write($"{name},");
        //}





        //   var list = sp.GetServices("Scirth", new string[] { "Apex Interstellar", "Black Market" }, 12);
        // var list = sp.GetServices("Scirth", new string[] { "Interstellar Factors Contact" }, 12);

        // var sys = SystemCache.FindSystem("Lembava", WebExternalDataLookup.Spansh);

        //  var isy = sp.GetSystem("sol");
        //var queryid = sp.RequestRoadToRiches("Sol", "Col 359 Sector BF-Z d136", 30, 25, 100, false, true, true, 1000000, 100000);

        // while (true)
        // {
        //     System.Threading.Thread.Sleep(2000);
        //     var resp = sp.TryGetRoadToRiches(queryid);
        // }Aaw

        // var ret = sp.GetStations("Sol", 4);

        //SystemClass sol = new SystemClass("Sol", 10477373803);
        //sp.GetBodies(sol);
        // sp.GetStationsByDump(sol,10000000,false);

        //EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();
        //sp.GetSystem("Sol");

        //EDSMClass edsm = new EDSMClass();
        // edsm.GetSystem("Sol");

        //var permitlist = SystemsDB.GetListPermitSystems();
        //foreach (var x in permitlist)
        //    System.Diagnostics.Debug.WriteLine($"{x.Name} {x.SystemAddress} {x.X} {x.Y} {x.Z}");
    }
}
