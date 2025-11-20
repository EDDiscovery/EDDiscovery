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


using BaseUtils;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using EliteDangerousCore.Spansh;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class EDDiscoveryForm
    {
        public void PreInitDebug()
        {
            EngineeringUnitTest.UnitTest();
            //EngineeringUnitTest.ScanLoadouts();

            //       FileHelpers.ReadWriteTextFile(@"c:\code\chatconsole.act", @"c:\code\o1.act");//, outlf:"\n");
            //       FileHelpers.ReadWriteTextFile(@"c:\code\EDDCanonnPanel.act", @"c:\code\o2.act");//, outlf:"\n");
        }

        public void PostInitDebug()
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

            FactionDefinitions.IDSTx();

            //string system = "Shrogaei YG-L d8-5499";
           // string system = "LHS 3447";
            //string system = "Shumbeia KD-I d10-15";
            //string system = "Dryeae Brai ZR-A d14-24";
            //string system = "Leesti";
            //     string system = "MY Apodis";
          //  string system = "Pallaeni";
            //string system = "Eorm Chruia DT-G d11-4215";
            //string system = "Eorm Chruia DT-G d11-490";
          // EliteDangerousCore.StarScan2.Tests.TestScan(system, $@"c:\code\eddiscovery\elitedangerouscore\elitedangerous\bodies\starscan2\tests", @"c:\code\AA", false,1920);
//
          //    EliteDangerousCore.StarScan2.Tests.TestScans(1920);
        }

        public void PostShownDebug()
        {
            // test code - open all types of panel
            //foreach (PanelInformation.PanelIDs pc in Enum.GetValues(typeof(PanelInformation.PanelIDs))) { if (pc != PanelInformation.PanelIDs.GroupMarker) tabControlMain.EnsureMajorTabIsPresent(pc, false); }
            // test code - close down all panels except tab 0
            //forach (PanelInformation.PanelIDs pc in Enum.GetValues(typeof(PanelInformation.PanelIDs))) { if (pc != PanelInformation.PanelIDs.GroupMarker) { TabPage p = tabControlMain.GetMajorTab(pc); if (p != null && p.TabIndex>0) tabControlMain.RemoveTab(p); } }

            // EliteDangerousCore.JournalTest.CheckAllJournalsGetInfoDescription(@"c:\users\rk\saved games\frontier developments\elite dangerous", "*.log", @"c:\code\out.log");
            // EliteDangerousCore.JournalTest.CheckAllJournalsGetInfoDescription(@"c:\code\logs", "journal*.log", @"c:\code\out.log");
            //    EliteDangerousCore.JournalTest.CheckAllJournalsGetInfoDescription(@"c:\code\logs\atlasgaming", "journal*.log", @"c:\code\out.log");

            //JournalEntry.WriteJournals(new DateTime(2021, 1, 10), new DateTime(2021, 1, 12), @"c:\code\dump.json");

            // JournalEntry.WriteJournals(35,new DateTime(2014, 1, 1), new DateTime(2099, 1, 1), @"c:\code\logs\lens larque");
        }

        public void PostHistoryLoadDebug()  // on UI thread
        {
          //  History.StarScan2.DrawAllSystemsToFolder(null);

            // Dump a systems entried to a debug file. Then use StarScan2.ProcessFromFile / Tests file to check the star scanner
            //string sysname = "Pallaeni";
            //var mhs = History.EntryOrder().Where(x => (x.System.Name.EqualsIIC(sysname)) && (x.journalEntry is IStarScan || x.journalEntry is IBodyFeature || x.journalEntry is JournalFSDJump)).ToList();
            //var jsonlines = mhs.Select(x => x.journalEntry.GetJsonString());
            //BaseUtils.FileHelpers.TryWriteAllLinesToFile($@"c:\code\eddiscovery\elitedangerouscore\elitedangerous\bodies\starscan2\tests\{sysname}.json", jsonlines.ToArray());
        }


        //var sys = History.StarScan.FindSystemSynchronous(new SystemClass("Pyranu QO-Z d13-28", 977516105979), WebExternalDataLookup.Spansh);
        //var sys2 = History.StarScan.FindSystemSynchronous(new SystemClass("Pyranu QO-Z d13-28", 977516105979), WebExternalDataLookup.Spansh);

        //var sys3 = SpanshClass.GetSpanshDump(new SystemClass("Pyranu QO-Z d13-28", 977516105979), true, false);
        //var sys4 = SpanshClass.GetSpanshDump(new SystemClass("Pyranu QO-Z d13-28", 977516105979), false, true);
        //var sys = SystemCache.FindSystem(new SystemClass("Pyranu QO-Z d13-28", 977516105979), WebExternalDataLookup.Spansh);
        //
        //var sy2s = History.StarScan.FindSystemSynchronous(new SystemClass("Pyranu QO-Z d13-28", 977516105979), WebExternalDataLookup.Spansh);

        // var sp = new EliteDangerousCore.Spansh.SpanshClass();
        //var blist = sp.GetBodies(new SystemClass("Pyranu QO-Z d13-28", 977516105979), out int? bodycount);

        //    foreach (StationDefinitions.StationServices en in Enum.GetValues(typeof(StationDefinitions.StationServices)))
        //    {
        //        string english = StationDefinitions.ToEnglish(en);
        //System.Diagnostics.Debug.WriteLine($".{en.ToString()}: \"{english}\" @");
        //    }


        //foreach (ShipSlots.Slot en in Enum.GetValues(typeof(ShipSlots.Slot)))
        //{
        //    string english = ShipSlots.ToEnglish(en);
        //    System.Diagnostics.Debug.WriteLine($".{en.ToString()}: \"{english}\" @");
        // }

        //foreach (var en in Enum.GetValues(typeof(ShipSlots.Slot)))
        //{
        //    string english = en.ToString().SplitCapsWordFull();
        //    System.Diagnostics.Debug.WriteLine($"[Slot.{en.ToString()}] = \"{english}\",");
        //}

        //foreach (var en in Enum.GetValues(typeof(ItemData.ShipModule.ModuleTypes)))
        //{
        //    string english = en.ToString().Replace("AX", "AX ").Replace("_", "-").SplitCapsWordFull();
        //    System.Diagnostics.Debug.WriteLine($".{english.Replace(" ", "_")}: \"{english}\" @");
        //}




        //BaseUtils.GitHubClass ghc = new BaseUtils.GitHubClass(Properties.Resources.URLGithubDownload);
        //BaseUtils.GitHubClass ghd = new BaseUtils.GitHubClass(Properties.Resources.URLGithubDataDownload);
        //    BaseUtils.GitHubRelease rel = ghc.GetLatestRelease();

        //          var dir = ghd.ReadFolder(new System.Threading.CancellationToken(), "Notifications");

        //var download = ghd.DownloadFolder(new System.Threading.CancellationToken(), @"c:\code\github","Notifications","*.*","*.*");
        //    var download = ghd.DownloadFolder(new System.Threading.CancellationToken(), @"c:\code\github","Notifications", "*.*", true);


        //System.Diagnostics.Trace.Write($"A bit more");
        //System.Diagnostics.Trace.Write($"and a line feed \n and a bit more");

        //for (int m = 0; m < 100; m++)
        //{
        //    System.Diagnostics.Trace.WriteLine($"\r\nTrace message {m/2}\r\nFred and jim\nlola\ndenis");
        //    //System.Diagnostics.Trace.WriteLine($"\r\nTrace message {m}");
        //    //System.Diagnostics.Debug.WriteLine($"Trace message {m / 2}\nFred and jim\nlola\ndenis");
        //    System.Threading.Thread.Sleep(200);
        //}

        //  
        //var allreleases = ghc.GetAllReleases(10);
        //var latestrelease = ghc.GetLatestRelease();


        //

        // string url = string.Format(EDDConfig.Instance.SpanshSystemsURL, "_6months");

        //   BaseUtils.HttpCom.DownloadURL(url, @"c:\code\spansh.dmp", true, out bool newfile, initialtimeout:2000);


        // ghc.GetLatestReleaseAsync(RespCallback,this);

        //var res = await ghc.GetLatestReleaseInTask();


        //            var res = await ghc.GetLatestReleaseInTask(new System.Threading.CancellationToken(), 20000);
        //          System.Diagnostics.Debug.WriteLine($"Result {res.StatusCode} {res.Body}");

        //var cancel = new System.Threading.CancellationToken();
        //var res = await ghc.GetLatestReleaseInTask(cancel,20000);
        //System.Diagnostics.Debug.WriteLine($"Result {res.StatusCode} {res.Body}");

        //static void RespCallback(BaseUtils.HttpCom.Response rd, object callerdata)
        //{
        //    System.Diagnostics.Debug.Assert(!System.Windows.Forms.Application.MessageLoop);
        //    System.Diagnostics.Debug.Write($"Async reponse back {rd.Error} {rd.StatusCode} {rd.Body}");
        //    EDDiscoveryForm form = callerdata as EDDiscoveryForm;

        //    form.BeginInvoke(((MethodInvoker)delegate { form.RespCallBack(rd); }));
        //}

        //void RespCallBack(BaseUtils.HttpCom.Response rd)
        //{
        //    System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);
        //    System.Diagnostics.Debug.Write($"Async reponse back in discovery form {rd.Error} {rd.StatusCode} {rd.Body}");

        //}

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

        // for translator, dump out lines
        //        foreach (var en in Enum.GetValues(typeof(ShipModule.ModuleTypes)))
        //        {
        //            string english = en.ToString().Replace("AX", "AX ").Replace("_", "-").SplitCapsWordFull();
        //    System.Diagnostics.Debug.WriteLine($".{english.Replace(" ", "_")}: \"{english}\" @");
        //        }


    }
}
