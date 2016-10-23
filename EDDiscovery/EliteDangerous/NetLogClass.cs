using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery.DB;
using EDDiscovery2;
using EDDiscovery2.DB;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.Common;
using Newtonsoft.Json.Linq;

namespace EDDiscovery
{
    public class NetLogClass
    {
        public delegate void NetLogEventHandler(JournalLocOrJump vsc);

        Dictionary<string, NetLogFileReader> netlogreaders = new Dictionary<string, NetLogFileReader>();

        public NetLogClass(EDDiscoveryForm ds)
        {
        }

        public static List<JournalEntry> ParseFiles(string datapath, out string error, int defaultMapColour, Func<bool> cancelRequested, Action<int, string> updateProgress, bool forceReload = false, Dictionary<string, NetLogFileReader> netlogreaders = null, int currentcmdrid = -1)
        {
            error = null;

            if (datapath == null)
            {
                error = "Netlog directory not set!";
                return null;
            }

            if (!Directory.Exists(datapath))   // if logfiles directory is not found
            {
                error = "Netlog directory is not present!";
                return null;
            }

            if (netlogreaders == null)
            {
                netlogreaders = new Dictionary<string, NetLogFileReader>();
            }

            if (currentcmdrid < 0)
            {
                currentcmdrid = EDDConfig.Instance.CurrentCmdrID;
            }

            List<JournalLocOrJump> visitedSystems = new List<JournalLocOrJump>();
            List<TravelLogUnit> tlus = TravelLogUnit.GetAll();
            Dictionary<string, TravelLogUnit> netlogtravelogUnits = tlus.Where(t => t.type == 1).GroupBy(t => t.Name).Select(g => g.First()).ToDictionary(t => t.Name);
            Dictionary<long, string> travellogunitid2name = netlogtravelogUnits.Values.ToDictionary(t => t.id, t => t.Name);
            Dictionary<string, List<JournalLocOrJump>> vsc_lookup = JournalEntry.GetAll().OfType<JournalLocOrJump>().GroupBy(v => v.TLUId).Where(g => travellogunitid2name.ContainsKey(g.Key)).ToDictionary(g => travellogunitid2name[g.Key], g => g.ToList());
            HashSet<long> netlogtluids = new HashSet<long>(netlogtravelogUnits.Values.Select(t => t.id).ToList());
            HashSet<long> journaltluids = new HashSet<long>(tlus.Where(t => t.type == 3).Select(t => t.id).ToList());
            List<JournalLocOrJump> vsSystemsEnts = JournalEntry.GetAll(currentcmdrid).OfType<JournalLocOrJump>().OrderBy(j => j.EventTimeUTC).ToList();
            List<JournalLocOrJump> vsSystemsList = vsSystemsEnts.Where(j => netlogtluids.Contains(j.TLUId)).ToList();
            List<JournalLocOrJump> journalEnts = vsSystemsEnts.Where(j => journaltluids.Contains(j.TLUId)).ToList();

            if (vsSystemsList != null && forceReload == false)
            {
                JournalLocOrJump last = visitedSystems.LastOrDefault();
                int ji = 0;

                foreach (JournalLocOrJump vs in vsSystemsList)
                {
                    while (ji < journalEnts.Count && journalEnts[ji].EventTimeUTC < vs.EventTimeUTC)
                    {
                        ji++;
                    }

                    JournalLocOrJump prev = (ji > 0 && (ji - 1) < journalEnts.Count) ? journalEnts[ji + 1] : null;
                    JournalLocOrJump next = ji < journalEnts.Count ? journalEnts[ji] : null;

                    bool lastissame = (last != null && last.StarSystem.Equals(vs.StarSystem, StringComparison.CurrentCultureIgnoreCase) && (!last.HasCoordinate || !vs.HasCoordinate || (last.StarPos - vs.StarPos).LengthSquared < 0.001));
                    bool previssame = (prev != null && prev.StarSystem.Equals(vs.StarSystem, StringComparison.CurrentCultureIgnoreCase) && (!prev.HasCoordinate || !vs.HasCoordinate || (prev.StarPos - vs.StarPos).LengthSquared < 0.001));
                    bool nextissame = (next != null && next.StarSystem.Equals(vs.StarSystem, StringComparison.CurrentCultureIgnoreCase) && (!next.HasCoordinate || !vs.HasCoordinate || (next.StarPos - vs.StarPos).LengthSquared < 0.001));

                    if (!(lastissame || previssame || nextissame))
                        visitedSystems.Add(vs);

                    last = vs;
                }
            }

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

            using (SQLiteConnectionUserUTC cn = new SQLiteConnectionUserUTC())
            {
                for (int i = 0; i < readersToUpdate.Count; i++)
                {
                    int ji = 0;

                    NetLogFileReader reader = readersToUpdate[i];
                    updateProgress(i * 100 / readersToUpdate.Count, reader.TravelLogUnit.Name);

                    using (DbTransaction tn = cn.BeginTransaction())
                    {
                        foreach (JObject jo in reader.ReadSystems(cancelRequested, currentcmdrid))
                        {
                            jo["EDDMapColor"] = defaultMapColour;

                            JournalLocOrJump je = new JournalFSDJump(jo)
                            {
                                TLUId = (int)reader.TravelLogUnit.id,
                                CommanderId = currentcmdrid,
                            };

                            while (ji < journalEnts.Count && journalEnts[ji].EventTimeUTC < je.EventTimeUTC)
                            {
                                ji++;
                            }

                            JournalLocOrJump prev = (ji > 0 && (ji - 1) < vsSystemsEnts.Count) ? vsSystemsEnts[ji - 1] : null;
                            JournalLocOrJump next = ji < vsSystemsEnts.Count ? vsSystemsEnts[ji] : null;

                            bool previssame = (prev != null && prev.StarSystem.Equals(je.StarSystem, StringComparison.CurrentCultureIgnoreCase) && (!prev.HasCoordinate || !je.HasCoordinate || (prev.StarPos - je.StarPos).LengthSquared < 0.001));
                            bool nextissame = (next != null && next.StarSystem.Equals(je.StarSystem, StringComparison.CurrentCultureIgnoreCase) && (!next.HasCoordinate || !je.HasCoordinate || (next.StarPos - je.StarPos).LengthSquared < 0.001));
                            bool nextissametime = (next != null && next.EventTimeUTC == je.EventTimeUTC);

                            if (!(previssame || nextissame))
                            {
                                je.Add(cn, tn);
                                visitedSystems.Add(je);
                            }
                            
                            if (!previssame && nextissame && nextissametime && netlogtluids.Contains(next.TLUId))
                            {
                                visitedSystems.Add(next);
                            }
                        }

                        tn.Commit();

                        reader.TravelLogUnit.Update();
                    }

                    if (updateProgress != null)
                    {
                        updateProgress((i + 1) * 100 / readersToUpdate.Count, reader.TravelLogUnit.Name);
                    }
                }
            }

            return visitedSystems.ToList<JournalEntry>();
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
