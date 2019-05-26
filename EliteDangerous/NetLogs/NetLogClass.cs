/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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

using EliteDangerousCore.JournalEvents;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Common;
using Newtonsoft.Json.Linq;

namespace EliteDangerousCore
{
    public class NetLogClass
    {
        public delegate void NetLogEventHandler(JournalLocOrJump vsc);

        Dictionary<string, NetLogFileReader> netlogreaders = new Dictionary<string, NetLogFileReader>();

        public NetLogClass()
        {
        }

        static public void ParseFiles(string datapath, out string error, int defaultMapColour, Func<bool> cancelRequested, Action<int, string> updateProgress, bool forceReload = false, Dictionary<string, NetLogFileReader> netlogreaders = null, int currentcmdrid = -1)
        {
            error = null;

            if (datapath == null)
            {
                error = "Netlog directory not set!";
                return;
            }

            if (!Directory.Exists(datapath))   // if logfiles directory is not found
            {
                error = "Netlog directory is not present!";
                return; 
            }

            if (netlogreaders == null)
            {
                netlogreaders = new Dictionary<string, NetLogFileReader>();
            }

            if (currentcmdrid < 0)
            {
                currentcmdrid = EDCommander.CurrentCmdrID;
            }

            // TLUs
            List<TravelLogUnit> tlus = TravelLogUnit.GetAll();
            Dictionary<string, TravelLogUnit> netlogtravelogUnits = tlus.Where(t => t.type == TravelLogUnit.NetLogType).GroupBy(t => t.Name).Select(g => g.First()).ToDictionary(t => t.Name);
            Dictionary<long, string> travellogunitid2name = netlogtravelogUnits.Values.ToDictionary(t => t.id, t => t.Name);
            Dictionary<string, List<JournalLocOrJump>> vsc_lookup = JournalEntry.GetAll().OfType<JournalLocOrJump>().GroupBy(v => v.TLUId).Where(g => travellogunitid2name.ContainsKey(g.Key)).ToDictionary(g => travellogunitid2name[g.Key], g => g.ToList());

            // list of systems in journal, sorted by time
            List<JournalLocOrJump> vsSystemsEnts = JournalEntry.GetAll(currentcmdrid).OfType<JournalLocOrJump>().OrderBy(j => j.EventTimeUTC).ToList();

            // order by file write time so we end up on the last one written
            FileInfo[] allFiles = Directory.EnumerateFiles(datapath, "netLog.*.log", SearchOption.AllDirectories).Select(f => new FileInfo(f)).OrderBy(p => p.LastWriteTime).ToArray();

            List<NetLogFileReader> readersToUpdate = new List<NetLogFileReader>();

            for (int i = 0; i < allFiles.Length; i++)
            {
                FileInfo fi = allFiles[i];

                var reader = OpenFileReader(fi, netlogtravelogUnits, vsc_lookup, netlogreaders);

                if (!netlogtravelogUnits.ContainsKey(reader.TravelLogUnit.Name))
                {
                    netlogtravelogUnits[reader.TravelLogUnit.Name] = reader.TravelLogUnit;
                    reader.TravelLogUnit.Add();
                }

                if (!netlogreaders.ContainsKey(reader.TravelLogUnit.Name))
                {
                    netlogreaders[reader.TravelLogUnit.Name] = reader;
                }

                if (forceReload)
                {
                    // Force a reload of the travel log
                    reader.TravelLogUnit.Size = 0;
                }

                if (reader.filePos != fi.Length || i == allFiles.Length - 1)  // File not already in DB, or is the last one
                {
                    readersToUpdate.Add(reader);
                }
            }

            for (int i = 0; i < readersToUpdate.Count; i++)
            {
                using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
                {
                    int ji = 0;

                    NetLogFileReader reader = readersToUpdate[i];
                    updateProgress(i * 100 / readersToUpdate.Count, reader.TravelLogUnit.Name);

                    UserDatabase.Instance.ExecuteWithDatabase(usetxn: true, mode: SQLLiteExtensions.SQLExtConnection.AccessMode.ReaderWriter, action: db =>
                    {
                        foreach (JObject jo in reader.ReadSystems(cancelRequested, currentcmdrid))
                        {
                            jo["EDDMapColor"] = defaultMapColour;

                            JournalLocOrJump je = new JournalFSDJump(jo);
                            je.SetTLUCommander(reader.TravelLogUnit.id, currentcmdrid);

                            while (ji < vsSystemsEnts.Count && vsSystemsEnts[ji].EventTimeUTC < je.EventTimeUTC)
                            {
                                ji++;   // move to next entry which is bigger in time or equal to ours.
                            }

                            JournalLocOrJump prev = (ji > 0 && (ji - 1) < vsSystemsEnts.Count) ? vsSystemsEnts[ji - 1] : null;
                            JournalLocOrJump next = ji < vsSystemsEnts.Count ? vsSystemsEnts[ji] : null;

                            bool previssame = (prev != null && prev.StarSystem.Equals(je.StarSystem, StringComparison.CurrentCultureIgnoreCase) && (!prev.HasCoordinate || !je.HasCoordinate || (prev.StarPos - je.StarPos).LengthSquared < 0.01));
                            bool nextissame = (next != null && next.StarSystem.Equals(je.StarSystem, StringComparison.CurrentCultureIgnoreCase) && (!next.HasCoordinate || !je.HasCoordinate || (next.StarPos - je.StarPos).LengthSquared < 0.01));

                            // System.Diagnostics.Debug.WriteLine("{0} {1} {2}", ji, vsSystemsEnts[ji].EventTimeUTC, je.EventTimeUTC);

                            if (!(previssame || nextissame))
                            {
                                je.Add(jo, db);
                                System.Diagnostics.Debug.WriteLine("Add {0} {1}", je.EventTimeUTC, jo.ToString());
                            }
                        }

                        db.Commit();

                        reader.TravelLogUnit.Update();
                    });

                    if (updateProgress != null)
                    {
                        updateProgress((i + 1) * 100 / readersToUpdate.Count, reader.TravelLogUnit.Name);
                    }
                }
            }
        }

        private static NetLogFileReader OpenFileReader(FileInfo fi, Dictionary<string, TravelLogUnit> tlu_lookup = null, Dictionary<string, List<JournalLocOrJump>> vsc_lookup = null, Dictionary<string, NetLogFileReader> netlogreaders = null)
        {
            NetLogFileReader reader;
            TravelLogUnit tlu;
            List<JournalLocOrJump> vsclist = null;

            if (vsc_lookup != null && vsc_lookup.ContainsKey(fi.Name))
            {
                vsclist = vsc_lookup[fi.Name];
            }

            if (netlogreaders != null && netlogreaders.ContainsKey(fi.Name))
            {
                return netlogreaders[fi.Name];
            }
            else if (tlu_lookup != null && tlu_lookup.ContainsKey(fi.Name))
            {
                tlu = tlu_lookup[fi.Name];
                tlu.Path = fi.DirectoryName;
                reader = new NetLogFileReader(tlu, vsclist);
            }
            else if (TravelLogUnit.TryGet(fi.Name, out tlu))
            {
                tlu.Path = fi.DirectoryName;
                reader = new NetLogFileReader(tlu, vsclist);
            }
            else
            {
                reader = new NetLogFileReader(fi.FullName);
            }

            if (netlogreaders != null)
            {
                netlogreaders[fi.Name] = reader;
            }

            return reader;
        }

   
    }
}
