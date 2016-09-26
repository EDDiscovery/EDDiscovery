using EDDiscovery.DB;
using EDDiscovery2.DB;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EDDiscovery
{
    public class HistoryEntry           // DONT store commander ID.. this history is externally filtered on it.
    {                                   
        public int Indexno;            // for display purposes.

        public EliteDangerous.JournalTypeEnum EntryType;
        public long Journalid;

        public ISystem System;         // Must be set! All entries, even if they are not FSD entries.
                                       // The Minimum is name and edsm_id 
                                       // x/y/z can be NANs or position. 
                                       // if edsm_id = 0, no edsm match was found.
                                       // when the front end needs to use it, it will call EnsureSystemEDSM/FillInPositions to see if it can be filled up
                                       // if System.status != SystemStatusEnum.EDSC then its presumed its an inmemory load and the system table can be checked
                                       //       and if edsm_id>0 a load from SystemTable will occur with the edsm_id used
                                       //       if edsm_id=-1 a load from SystemTable will occur with the name used
                                       // SO the journal reader can just read data in that table only, does not need to do a system match

        public DateTime EventTime;
        public string EventSummary;
        public string EventDescription;
        public string EventDetailedInfo;

        public int MapColour;
        public bool EdsmSync;           // synced FSDJump with EDSM?  TBD where is it stored.

        public bool IsFSDJump { get { return EntryType == EliteDangerous.JournalTypeEnum.FSDJump; } }

        public void MakeVSEntry(ISystem sys, DateTime eventt, int m, string dist, string info, int journalid = 0)
        {
            Debug.Assert(sys != null);
            EntryType = EliteDangerous.JournalTypeEnum.FSDJump;
            System = sys;
            EventTime = eventt;
            EventSummary = "Jump to " + System.name;
            EventDescription = dist;
            EventDetailedInfo = info;
            MapColour = m;
            Journalid = journalid;
            EdsmSync = true; 
        }

        public static HistoryEntry FromVSEntry(ISystem sys, DateTime eventt, int m, string dist, string info, int journalid = 0)
        {
            Debug.Assert(sys != null);
            return new HistoryEntry
            {
                EntryType = EliteDangerous.JournalTypeEnum.FSDJump,
                System = sys,
                EventTime = eventt,
                EventSummary = "Jump to " + sys.name,
                EventDescription = dist,
                EventDetailedInfo = info,
                MapColour = m,
                Journalid = journalid,
                EdsmSync = true 
            };
        }

        public void MakeJournalEntry(EliteDangerous.JournalTypeEnum type, long id , ISystem sys, DateTime eventt, string summary , string descr, string info, int m, bool edss)
        {
            EntryType = type; Journalid = id; System = sys; EventTime = eventt; EventSummary = summary; EventDescription = descr; EventDetailedInfo = info;
            MapColour = m; EdsmSync = edss;
        }

        public static HistoryEntry FromJournalEntry(EliteDangerous.JournalTypeEnum type, long id, ISystem sys, DateTime eventt, string summary, string descr, string info, int m, bool edss)
        {
            return new HistoryEntry
            {
                EntryType = type,
                Journalid = id,
                System = sys,
                EventTime = eventt,
                EventSummary = summary,
                EventDescription = descr,
                EventDetailedInfo = info,
                MapColour = m,
                EdsmSync = edss
            };
        }

        public System.Drawing.Bitmap GetIcon
        {  get
            { 
              return EDDiscovery.Properties.Resources.floppy;
            }
        }

        public void EnsureSystemEDSM()        // fill in from EDSM
        {
            if (System.id_edsm >= 0)         // if never tried..
            {
                SystemClass s = SystemClass.EDSMAssign(System, Journalid);

                if (s != null)
                    System = s;
                else
                    System.id_edsm = -1;    // don't try again
            }
        }

        public bool UpdateMapColour(int v)
        {
            MapColour = v;
            if (Journalid != 0)
            {
                //TBD Update journal
            }
            return true;
        }

        public bool UpdateCommanderID(int v)
        {
            //TBD how do we do this..
            if (Journalid != 0)
            {
            }
            return false;
        }

        public bool UpdateEdsmSync()
        {
            //TBD how do we do this..
            if (Journalid != 0)
            {
            }
            EdsmSync = true;
            return false;
        }

    }


    public class HistoryList : IEnumerable<HistoryEntry>
    {
        private List<HistoryEntry> historylist = new List<HistoryEntry>();  // oldest first here

        public void Clear()
        {
            historylist.Clear();
        }

        public void Add(HistoryEntry e)
        {
            historylist.Add(e);
        }

        public List<HistoryEntry> FilterByNumber(int max)
        {
            return historylist.OrderByDescending(s => s.EventTime).Take(max).ToList();
        }

        public List<HistoryEntry> FilterByDate(TimeSpan days)
        {
            var oldestData = DateTime.Now.Subtract(days);
            return (from systems in historylist where systems.EventTime >= oldestData orderby systems.EventTime descending select systems).ToList();
        }

        public int Count { get { return historylist.Count;  } }

        public List<HistoryEntry> EntryOrder
        {
            get
            {
                return historylist;
            }
        }

        public List<HistoryEntry> LastFirst
        {
            get
            {
                return historylist.OrderByDescending(s => s.Indexno).ToList();
            }
        }

        public List<HistoryEntry> OrderByDate
        {
            get
            {
                return historylist.OrderByDescending(s => s.EventTime).ToList();
            }
        }

        public List<HistoryEntry> FilterByNotEDSMSyncedAndFSD
        {
            get
            {
                return (from s in historylist where s.EdsmSync == false && s.IsFSDJump select s).ToList();
            }
        }

        public List<HistoryEntry> FilterByFSDAndPosition
        {
            get
            {
                return (from s in historylist where s.IsFSDJump && s.System.HasCoordinate select s).ToList();
            }
        }

        public HistoryEntry GetLast
        {
            get
            {
                if (historylist.Count > 0)
                    return historylist[historylist.Count - 1];
                else
                    return null;
            }
        }

        public HistoryEntry GetLastFSD
        {
            get
            {
                return historylist.FindLast(x => x.IsFSDJump);
            }
        }

        public HistoryEntry GetLastWithPosition
        {
            get
            {
                return historylist.FindLast(x => x.System.HasCoordinate);
            }
        }


        public int GetVisitsCount(string name)
        {
            return (from row in historylist where row.System.name.Equals(name, StringComparison.InvariantCultureIgnoreCase) select row).Count();
        }

        public int GetFSDJumps( TimeSpan t )
        {
            DateTime tme = DateTime.Now.Subtract(t);
            return (from s in historylist where s.IsFSDJump && s.EventTime>=tme select s).Count();
        }

        public void FillInPositionsFSDJumps()       // call if you want to ensure we have the best posibile position data on FSD Jumps.  Only occurs on pre 2.1 with lazy load of just name/edsmid
        {
            foreach (HistoryEntry he in historylist)
            {
                if (!he.System.HasCoordinate && he.IsFSDJump)
                    he.EnsureSystemEDSM();        // fill in from EDSM if possible - only occurs
            }
        }

        public void CalculateSqDistances(SortedList<double, ISystem> distlist, double x, double y, double z, int maxitems, bool removezerodiststar)
        {
            double dist;
            double dx, dy, dz;
            Dictionary<long, ISystem> systems = distlist.Values.GroupBy(s => s.id).ToDictionary(g => g.Key, g => g.First());

            foreach (HistoryEntry pos in historylist)
            {
                if (pos.System.HasCoordinate && !systems.ContainsKey(pos.System.id))   // if co-ords, and not in list already..
                {
                    dx = (pos.System.x - x);
                    dy = (pos.System.y - y);
                    dz = (pos.System.z - z);
                    dist = dx * dx + dy * dy + dz * dz;

                    if (dist > 0.001 || !removezerodiststar)
                    {
                        if (distlist.Count < maxitems)          // if less than max, add..
                            distlist.Add(dist, pos.System);
                        else if (dist < distlist.Last().Key)   // if last entry (which must be the biggest) is greater than dist..
                        {
                            distlist.Add(dist, pos.System);           // add in
                            distlist.RemoveAt(maxitems);        // remove last..
                        }
                    }
                }
            }
        }

        public HistoryEntry FindByName(string name, bool fsdjump = false)
        {
            if (fsdjump)
                return historylist.FindLast(x => x.System.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            else
                return historylist.FindLast(x => x.System.name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && x.IsFSDJump);
        }

        public ISystem FindSystem(string name, EDSM.GalacticMapping glist = null)        // in system or name
        {
            ISystem ds1 = SystemClass.GetSystem(name);

            if (ds1 == null)
            {
                HistoryEntry vs = FindByName(name);

                if (vs != null)
                    ds1 = vs.System;
                else if (glist != null)
                {
                    EDSM.GalacticMapObject gmo = glist.Find(name, true, true);

                    if (gmo != null && gmo.points.Count > 0)
                    {
                        return new SystemClass(gmo.name, gmo.points[0].X, gmo.points[0].Y, gmo.points[0].Z);        // fudge it into a system
                    }
                }
            }

            return ds1;
        }

        public HistoryEntry PreviousFrom(HistoryEntry e, bool fsdjumps)
        {
            if (e != null)
            {
                int curindex = historylist.IndexOf(e);

                if (curindex > 0)       // no point with index=0, there is no last.
                {
                    int lastindex = historylist.FindLastIndex(curindex - 1, x => (fsdjumps) ? x.IsFSDJump : true);

                    if (lastindex >= 0)
                        return historylist[lastindex];
                }
            }

            return null;
        }

        public IEnumerator<HistoryEntry> GetEnumerator()
        {
            foreach (var e in historylist)
            {
                yield return e;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static HistoryEntry FindNextSystem(List<HistoryEntry> syslist, string sysname, int dir)
        {
            int index = syslist.FindIndex(x => x.System.name.Equals(sysname));

            if (index != -1)
            {
                if (dir == -1)
                {
                    if (index < 1)                                  //0, we go to the end and work from back..
                        index = syslist.Count;

                    int indexn = syslist.FindLastIndex(index - 1, x => x.System.HasCoordinate);

                    if (indexn == -1)                             // from where we were, did not find one, try from back..
                        indexn = syslist.FindLastIndex(x => x.System.HasCoordinate);

                    return (indexn != -1) ? syslist[indexn] : null;
                }
                else
                {
                    index++;

                    if (index == syslist.Count)             // if at end, go to beginning
                        index = 0;

                    int indexn = syslist.FindIndex(index, x => x.System.HasCoordinate);

                    if (indexn == -1)                             // if not found, go to beginning
                        indexn = syslist.FindIndex(x => x.System.HasCoordinate);

                    return (indexn != -1) ? syslist[indexn] : null;
                }
            }
            else
            {
                index = syslist.FindLastIndex(x => x.System.HasCoordinate);
                return (index != -1) ? syslist[index] : null;
            }
        }

        public static HistoryEntry FindLastFSDKnownPosition(List<HistoryEntry> syslist)
        {
            return syslist.FindLast(x => x.System.HasCoordinate && x.IsFSDJump);
        }

        public static HistoryEntry FindByPos(List<HistoryEntry> syslist, Vector3 p, double limit)     // go thru setting the lastknowsystem
        {
            return syslist.FindLast(x => x.System.HasCoordinate &&
                                            Math.Abs(x.System.x - p.X) < limit &&
                                            Math.Abs(x.System.y - p.Y) < limit &&
                                            Math.Abs(x.System.z - p.Z) < limit);
        }

        public static List<HistoryEntry> FilterByJournalEvent(List<HistoryEntry> he , string eventstring)
        {
            if (eventstring.Equals("All"))
                return he;
            else
            {
                string[] events = eventstring.Split(';');
                return (from systems in he where events.Contains(Tools.SplitCapsWord(systems.EntryType.ToString())) select systems).ToList();
            }
        }
    }
}
