using BaseUtils;
using BaseUtils.UnitTests;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using EliteDangerousCore.StarScan2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public static class UnitTestReadAllEventFiles
    {
        [BaseUtils.UnitTests.Test(100)]
        public static void TestEventFiles()
        {
            CheckSection("ShipInformation");

            string pathto = @"c:\code\logs";

            if (  Directory.Exists(pathto))
            {
                ShipList shl = new ShipList();
                var wl = new SuitWeaponList();
                var sl = new SuitList();
                var sll = new SuitLoadoutList();
                var shipyards = new ShipYardList();
                var outfitting = new OutfittingList();
                var starscan = new StarScan();
                var matcommd = new MaterialCommoditiesMicroResourceList();
                var ledger = new Ledger();
                
                List<string> checklist = new List<string>();
                //checklist.AddRange(new string[] { "Cargo", "Missions", "Passengers" });
                //checklist.AddRange(new string[] { "weapon", "suit"});
                // checklist.AddRange(new string[] { "docksrv"});
                //checklist.AddRange(new string[] { "synthesis" });
                checklist.AddRange(new string[] { "scan" });

                string startat = null;// "redeemvoucher";

                foreach (var eventfile in Directory.EnumerateFiles(pathto, "*.event", SearchOption.TopDirectoryOnly).OrderBy(x=>x))
                {

                    string filenameonly = Path.GetFileNameWithoutExtension(eventfile);
                    int cc = checklist.ComparisionContains(filenameonly, StringComparison.InvariantCultureIgnoreCase);

                    if (checklist != null && checklist.Count > 0 && cc== -1)
                    {
                        continue;
                    }

                    if (startat != null && !filenameonly.ContainsIIC(startat))
                    {
                        continue;
                    }
                    else
                        startat = null;

                    FileInfo finfo = new FileInfo(eventfile);

                    System.Diagnostics.Debug.WriteLine($"\r\n{DateTime.UtcNow} Event File {eventfile} length {finfo.Length:N0}");

                    bool first = true;

                    AppTicks.Start("UT");
                    long reportelapsed = 5000;

                    int count = 0;

                    CheckerHelpers.ReadAttedJsonFile(eventfile, (str,idline) => 
                    {
                        string[] gb = idline.Split(';');
                        if ( gb.Length == 5)
                        { 
                            DateTime filetime = DateTime.Parse(gb[0]);

                            //if (t >= EliteReleaseDates.Odyssey1)
                            try
                            {
                                JournalEntry.DefaultBetaFlag = EliteReleaseDates.IsBeta(gb[1], gb[2], filetime);
                                var jd = JournalEntry.CreateJournalEntry(str);

                                if (jd == null)
                                {
                                    System.Diagnostics.Debug.WriteLine($"****Unable to decode {eventfile} : {str}");
                                }
                                else
                                {
                                    if (first)
                                    {
                                     //   System.Diagnostics.Debug.WriteLine($"Event {eventfile} {jd.EventTypeStr} {jd.GetInfo()}");
                                        first = false;
                                    }

                                    if (finfo.Length < 800 * 1000 * 1024 || (filetime <= new DateTime(2018, 1, 1, 0, 0, 0) || filetime >= new DateTime(2024, 1, 1, 0, 0, 0)))
                                    {
                                        var one = jd.GetInfo();
                                        var two = jd.GetDetailed();

                                        var sys = new SystemClass("Sol", new SystemAddress(12344));
                                        shl.Process(jd, "fred", sys, false);
                                        wl.Process(jd, "fred", sys);
                                        sl.Process(jd, "fred", sys);
                                        sll.Process(jd, wl, "fred", sys);
                                        matcommd.Process(jd, jd, false);
                                        shipyards.Process(jd);
                                        outfitting.Process(jd);
                                        ledger.Process(jd);
                                        if (jd is IStarScan ss && filetime >= new DateTime(2024, 1, 1, 0, 0, 0))
                                        {
                                            ss.AddStarScan(starscan, sys);
                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                            catch( Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error {ex}\r\n{ex.StackTrace}");
                            }
                        }

                        var elapsed = AppTicks.TickCountFromStart("UT");
                        if ( elapsed > reportelapsed)
                        {
                            System.Diagnostics.Debug.WriteLine($"Execute time {elapsed} number {count}");
                            reportelapsed += 5000;
                        }

                        if (count++ > 100000000 && false)
                        {
                            System.Diagnostics.Debug.WriteLine("Break");
                            return false;
                        }

                        count++;
                        return true;
                    });

                    var time = AppTicks.TickCountFromStart("UT");

                    System.Diagnostics.Debug.WriteLine($"Execute time {time} number {count} per event {(double)time / count:0.0000}ms, per 1000 {(double)time / (count / 1000):0.00}ms ");

                }
            }
        }
    }
}

