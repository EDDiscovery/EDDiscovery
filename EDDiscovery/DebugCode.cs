/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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
using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    static class DebugCode      // Class to hold some long term debug code called by command line options.. 
    {
        public static void ReadCmdLineJournal(string file)          //DEBUG ONLY FROM COMMAND LINE
        {
            System.IO.StreamReader filejr = new System.IO.StreamReader(file);
            string line;
            string system = "";
            StarScan ss = new StarScan();

            Dictionary<string, string> items = new Dictionary<string, string>();

            while ((line = filejr.ReadLine()) != null)
            {
                if (line.Equals("END"))
                    break;
                //System.Diagnostics.Trace.WriteLine(line);

                if (line.Length > 0)
                {
                    JObject jo = (JObject)JObject.Parse(line);

                    //JSONPrettyPrint jpp = new JSONPrettyPrint(EliteDangerous.JournalFieldNaming.StandardConverters(), "event;timestamp", "_Localised", (string)jo["event"]);
                    //string s = jpp.PrettyPrintStr(line, 80);
                    //System.Diagnostics.Trace.WriteLine(s);

                    EliteDangerous.JournalEntry je = EliteDangerous.JournalEntry.CreateJournalEntry(line);
                    //System.Diagnostics.Trace.WriteLine(je.EventTypeStr);

                    if (je.EventTypeID == JournalTypeEnum.Location)
                    {
                        EDDiscovery.EliteDangerous.JournalEvents.JournalLocOrJump jl = je as EDDiscovery.EliteDangerous.JournalEvents.JournalLocOrJump;
                        system = jl.StarSystem;
                    }
                    else if (je.EventTypeID == JournalTypeEnum.FSDJump)
                    {
                        EDDiscovery.EliteDangerous.JournalEvents.JournalFSDJump jfsd = je as EDDiscovery.EliteDangerous.JournalEvents.JournalFSDJump;
                        system = jfsd.StarSystem;

                    }
                    else if (je.EventTypeID == JournalTypeEnum.Scan)
                    {
                        ss.Process(je as EliteDangerous.JournalEvents.JournalScan, new SystemClass(system));
                    }
                }

            }
        }

        public static void ReadJSON(string file)          //DEBUG ONLY FROM COMMAND LINE
        {
            Stopwatch t = new Stopwatch();

            t.Start();

            System.Diagnostics.Debug.WriteLine("HelloThere123_HowAreYou AndSome1232 and some more".SplitCapsWord());

            string st = "$int_stellarbodydiscoveryscanner_medium_class2";

            for (long i = 0; i < 56000; i++)
            {
                string p = JournalFieldNaming.GetBetterItemNameEvents(st);
            }

            long elapsed = t.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("Elapsed " + elapsed);


            System.IO.StreamReader filejr = new System.IO.StreamReader(file);
            string line;

            Dictionary<string, string> items = new Dictionary<string, string>();

            while ((line = filejr.ReadLine()) != null)
            {
                if (line.Equals("END"))
                    break;
                //System.Diagnostics.Trace.WriteLine(line);

                if (line.Length > 0)
                {
                    JObject jo = (JObject)JObject.Parse(line);

                    string s = JSONHelper.GetStringNull(jo["BuyItem"]);

                    if (s != null)
                    {
                        items[s] = s;
                    }

                    s = JSONHelper.GetStringNull(jo["SellItem"]);

                    if (s != null)
                    {
                        items[s] = s;
                    }

                    s = JSONHelper.GetStringNull(jo["Name"]);

                    if (s != null)
                    {
                        items[s] = s;
                    }

                    s = JSONHelper.GetStringNull(jo["FromItem"]);

                    if (s != null)
                    {
                        items[s] = s;
                    }
                    s = JSONHelper.GetStringNull(jo["ToItem"]);

                    if (s != null)
                    {
                        items[s] = s;
                    }
                }
            }

            Dictionary<string, string> cutitems = new Dictionary<string, string>();

            foreach (string i in items.Keys.ToList())
            {
                if (i.Length > 5)
                {
                    string s = i.Substring(5);
                    int underscore = s.IndexOf('_');
                    s = s.Substring(0, underscore);
                    cutitems[s] = s;
                }
            }
            List<string> l = cutitems.Keys.ToList();
            l.Sort();
            foreach (string i in l )
            {
                System.Diagnostics.Debug.WriteLine("            {\"" + i + "\",     \"" + i + "\"},");

            }
        }
    }
}
