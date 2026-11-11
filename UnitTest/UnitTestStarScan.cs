/*
 * Copyright 2026-2026 EDDiscovery development team
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

using QuickJSON;
using BaseUtils;
using System;
using System.Linq;
using EliteDangerousCore;
using static BaseUtils.UnitTests.CheckerHelpers;
using System.Collections.Generic;
using System.IO;

namespace UnitTest
{
    public static class UnitTestScanScan
    {

        [BaseUtils.UnitTests.Test]
        public static void TestStarScan()
        {
            CheckSection("ScanScan Basics");

            {
                EliteDangerousCore.StarScan2.SystemNode.AlignParentsName(new System.Collections.Generic.List<BodyParent> {
                                                                        new BodyParent(BodyParent.ParentBodyType.Planet, 2),
                                                                        new BodyParent(BodyParent.ParentBodyType.Null, 1),
                                                                        new BodyParent(BodyParent.ParentBodyType.Star, 0),
                                                                        },
                                                                        "10 b", out List<string> pl); //"Prieluia QI-Q c19-31 10 b"
                Check(pl[0].Contains("Unknown Star") && pl[1] == "Unknown Barycentre" && pl[2].Contains("10") && pl[3] == "b");
            }

            {
                EliteDangerousCore.StarScan2.SystemNode.AlignParentsName(new System.Collections.Generic.List<BodyParent> {
                                                                            new BodyParent(BodyParent.ParentBodyType.Star, 3),
                                                                            new BodyParent(BodyParent.ParentBodyType.Null, 0),
                                                                            },
                                                                            "AB 1 b", out List<string> pl);
                Check(pl[0].Contains("AB") && pl[1] == "1" && pl[2] == "b");  // Skaude AA-A h294 AB 1 a
            }

            {
                EliteDangerousCore.StarScan2.SystemNode.AlignParentsName(new System.Collections.Generic.List<BodyParent> {
                                                                            new BodyParent(BodyParent.ParentBodyType.Null, 1),
                                                                            new BodyParent(BodyParent.ParentBodyType.Star, 0),
                                                                            new BodyParent(BodyParent.ParentBodyType.Null, 1),
                                                                            },
                                                                            "A 1", out List<string> pl);         // HIP 1885 A 1
                Check(pl[0].Contains("Unknown Bary") && pl[1] == "A" && pl[2].Contains("Unknown Bary") && pl[3] == "1");
            }

            {
                EliteDangerousCore.StarScan2.SystemNode.AlignParentsName(new System.Collections.Generic.List<BodyParent> {
                                                                        new BodyParent(BodyParent.ParentBodyType.Null, 1),
                                                                        },
                                                                            "A", out List<string> pl);           // HIP 1885 A
                Check(pl[0].Contains("Unknown Bary") && pl[1] == "A");
            }

            {
                EliteDangerousCore.StarScan2.SystemNode.AlignParentsName(new System.Collections.Generic.List<BodyParent> {
                                                                        new BodyParent(BodyParent.ParentBodyType.Ring, 7),
                                                                        new BodyParent(BodyParent.ParentBodyType.Star, 1),
                                                                        },
                                                                        "B Belt Cluster 4", out List<string> pl);    // Scheau Prao ME-M c22-21 B Belt Cluster 4

                Check(pl[0].Contains("Unknown Star") && pl[1] == "B Belt Cluster" && pl[2] == "4");
            }

            {
                EliteDangerousCore.StarScan2.SystemNode.AlignParentsName(new System.Collections.Generic.List<BodyParent> {
                                                                        new BodyParent(BodyParent.ParentBodyType.Ring, 7),
                                                                        new BodyParent(BodyParent.ParentBodyType.Null, 3),
                                                                        new BodyParent(BodyParent.ParentBodyType.Star, 1),
                                                                        new BodyParent(BodyParent.ParentBodyType.Null, 0),
                                                                        },
                                                                        "B Belt Cluster 4", out List<string> pl);
                Check(pl[0].Contains("Unknown Bary") && pl[1].Contains("Unknown Star") && pl[2] == "Unknown Barycentre" && pl[3] == "B Belt Cluster" && pl[4] == "4");
            }

            {
                EliteDangerousCore.StarScan2.SystemNode.AlignParentsName(new System.Collections.Generic.List<BodyParent> {
                                                                        new BodyParent(BodyParent.ParentBodyType.Null, 2),
                                                                        new BodyParent(BodyParent.ParentBodyType.Star, 1),
                                                                        new BodyParent(BodyParent.ParentBodyType.Null, 0)}, "1", out List<string> pl);
                Check(pl[0].Contains("Unknown Bary") && pl[1].Contains("Unknown Star") && pl[2] == "Unknown Barycentre" && pl[3] == "1");
            }
        }
        [BaseUtils.UnitTests.Test(60)]
        public static void TestStarJsons()
        {
            CheckSection("ScanScan Test Stars JSON");
            //Debugger.OutputControl += "StarScan";        // turn on debugging

            string folder = $@"..\..\..\UnitTest\StarScans\";

            if (Directory.Exists(folder))
            {
                var files = System.IO.Directory.EnumerateFiles(folder, "*.json");
                //var files = System.IO.Directory.EnumerateFiles(folder, "9*.json");
                foreach (var f in files.EmptyIfNull())
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(f);
                    if (!name.StartsWithIIC("Synth"))
                    {
                        var sn = TestScan(null, f, @"c:\code\Images\Dump", false, 1920, false);
                        if (sn != null)
                        {
                            //sn.DumpTree();
                            if ( name == "Leesti")
                            {
                                CheckThat(sn.System.Name).Is("Leesti");
                                CheckThat(sn.FSSSignals.Count).Is(1);
                                CheckThat(sn.OrbitingStations.Count).Is(1);
                                var bd = sn.FindBody(0);
                                CheckThat(bd).IsNotNull();
                                CheckThat(bd.BodyType).Is(BodyDefinitions.BodyType.Star);
                                bd = sn.FindBody(9);
                                CheckThat(bd.BodyType).Is(BodyDefinitions.BodyType.Planet);
                                CheckThat(bd.CanonicalName).Is("Leesti 1 a");
                                CheckThat(bd.OwnName).Is("a");
                            }
                        }
                    }
                }
            }
        }

        static public EliteDangerousCore.StarScan2.SystemNode TestScan(EliteDangerousCore.StarScan2.StarScan ss, string jsonfile, string outpath, bool draweachone, int width, bool showmaterials = true)
        {
            if (File.Exists(jsonfile))
            {
                if (ss == null)     // if create new one each time..
                    ss = new EliteDangerousCore.StarScan2.StarScan();

                uint gen = 1817272;
                uint siggen = 202992;
                var hist = HistoryEntry.CreateFromFile(jsonfile);       // int/historyentry list

                bool outfolderexists = Directory.Exists(outpath);
                ISystem syst = null;
                ss.ProcessFromHistory(hist, (ss2, mhe) =>
                {
                    syst = ss2.GetISystem(mhe.Item2.System.Name);
                    if (syst != null)
                    {
                        var node = ss2.FindSystemSynchronous(syst, false);

                        if (node.BodyGeneration != gen | node.SignalGeneration != siggen)
                        {
                            gen = node.BodyGeneration;
                            siggen = node.SignalGeneration;
                            if (draweachone && outfolderexists)
                            {
                                string path = Path.Combine(outpath, node.System.Name.SafeFileString()) + $"-{mhe.Item1}.png";
                                node.DrawSystemToFile(path, width, showmaterials);
                            }
                        }
                    }
                });

                ss.AssignPending();

                EliteDangerousCore.StarScan2.SystemNode final = ss.FindSystemSynchronous(syst, false);

                if (!draweachone && outfolderexists)
                {
                    string path = Path.Combine(outpath, final.System.Name.SafeFileString()) + ".png";
                    final.DrawSystemToFile(path, width, showmaterials);
                }

                return final;
            }
            else
                return null;
        }


        // DEMO a new BC sorting method
        public const string BCNamingPrefix = "BC of ";
        public static int Comparer(string left, string right, List<string> items)
        {
            int res = 0;
            string lt = left;
            string rt = right;
            string[] rsubparts = null;
            string[] lsubparts = null;

            if (rt.StartsWith(BCNamingPrefix))
                rsubparts = rt.Substring(BCNamingPrefix.Length).SplitTrim(',').Where(x => items.Contains(x)).ToArray();     // split back into parts, need to trim off spaces, only consider other sorted items

            if (lt.StartsWith(BCNamingPrefix))
                lsubparts = lt.Substring(BCNamingPrefix.Length).SplitTrim(',').Where(x => items.Contains(x)).ToArray();     // split back into parts, need to trim off spaces, only consider other sorted items

            if (rsubparts?.Length > 0)      // protect against not having a nodenames in the BC list
            {
                if (lsubparts?.Length > 0)
                    res = lsubparts[0].CompareTo(rsubparts[0]);
                else
                    res = lt == rsubparts[0] ? -1 : lt.CompareTo(rsubparts[0]);
            }
            else if (lsubparts?.Length > 0)
            {
                res = lsubparts[0] == rt ? 1 : lsubparts[0].CompareTo(rt);
            }
            else
            {
                res = lt.CompareTo(rt);
            }

            System.Diagnostics.Debug.WriteLine($"Compare {lt} vs {rt} = {res}");

            return res;
        }

        // demo code, not a test, disabled

        [BaseUtils.UnitTests.Test(-200000)]
        public static void TestStarScanSort1()
        {
            List<string> items = new List<string> { "BC of 1,2,3,D,E", "D", "BC of F,G", "F", "A", "BC of A,B", "C", "E", "B", "G" };
            items.Sort(delegate (string left, string right) { return Comparer(left, right, items); });
            foreach (var s in items)
                System.Diagnostics.Debug.WriteLine($"Item {s}");
        }

        [BaseUtils.UnitTests.Test(-200000)]
        public static void TestStarScanSort2()
        {
            List<string> items = new List<string> { "BC of C", "D", "BC of 1, 4, 5, F", "E", "A", "C", "B", "F" , "BC of 1, 2, 3, D, E"};
            items.Sort(delegate (string left, string right) { return Comparer(left, right, items); });
            foreach (var s in items)
                System.Diagnostics.Debug.WriteLine($"Item {s}");
        }

    }
}
    