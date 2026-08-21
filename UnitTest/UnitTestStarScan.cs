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
            // Debugger.OutputControl += "StarScan";        // turn on debugging

            string folder = $@"..\..\..\UnitTest\StarScans\";

            if (Directory.Exists(folder))
            {
                var files = System.IO.Directory.EnumerateFiles(folder, "*.json");
                foreach (var f in files.EmptyIfNull())
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(f);
                    if (!name.StartsWithIIC("Synth"))
                    {
                        var sn = TestScan(null, f, @"c:\code\Images", false, 1920, false);
                        if (sn != null)
                        {
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

    }
}
    