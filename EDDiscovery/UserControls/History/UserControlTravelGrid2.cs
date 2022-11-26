/*
 * Copyright © 2016 - 2021 EDDiscovery development team
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

using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlTravelGrid
    {
        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(false, new string[] { "View", "FSD Jumps only", "With Notes only", "With Notes, no repeat",        //0-3
                                            "Scans All Bodies", "Scans Planets", "Scans Stars", "Scans Rings" },       //4-7
                suggestedfilenames: new string[] { "TravelGrid", "FSDJumps", "TravelGrid", "TravelGrid", "Scans", "ScanPlanet", "ScanStars", "ScanRings" }
                );

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (frm.SelectedIndex == 7)
                {
                    var saaentries = JournalEntry.GetByEventType(JournalTypeEnum.SAASignalsFound, EDCommander.CurrentCmdrID, frm.StartTimeUTC, frm.EndTimeUTC).ConvertAll(x => (JournalSAASignalsFound)x);
                    var scanentries = JournalEntry.GetByEventType(JournalTypeEnum.Scan, EDCommander.CurrentCmdrID, frm.StartTimeUTC, frm.EndTimeUTC).ConvertAll(x => (JournalScan)x);

                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                    grd.SetCSVDelimiter(frm.Comma);

                    string[] headers1 = { "", "", "", "", "","",
                            "Icy Ring" , "","","","","","","","","",
                            "Rocky","","","","","","","",
                            "Metal Rich","","","","",
                            "Metalic","","","","",
                            "Other"
                        };

                    string[] headers2 = { "Time", "BodyName", "Ring Types", "Mass MT", "Inner Rad (ls)","Outer Rad (ls)",

                            // icy ring
                            "Water", "Liquid Oxygen", "Methanol Mono", "Methane","Bromellite", "Grandidierite", "Low Temp Diamonds", "Void Opals","Alexandrite" , "Tritium",
                            // rocky
                            "Bauxite","Indite","Alexandrite","Monazite","Musgravite","Benitoite","Serendibite","Rhodplumsite",
                            // metal rich
                            "Rhodplumsite","Serendibite","Platinum","Monazite","Painite",
                            // metalic
                            "Rhodplumsite","Serendibite","Platinum","Monazite","Painite",
                            // others
                            "Geological","Biological","Thargoid","Human","Guardian",
                        };

                    string[] fdname = { "Water", "LiquidOxygen", "methanolmonohydratecrystals", "MethaneClathrate",     // icy
                                            "Bromellite", "Grandidierite", "lowtemperaturediamond", "Opal",
                                            "Alexandrite", "Tritium",
                                            "Bauxite","Indite","Alexandrite","Monazite","Musgravite","Benitoite","Serendibite","Rhodplumsite",          // rocky
                                            "Rhodplumsite","Serendibite","Platinum","Monazite","Painite",   // metal rich  
                                            "Rhodplumsite","Serendibite","Platinum","Monazite","Painite", // metalic
                                            "$SAA_SignalType_Geological;","$SAA_SignalType_Biological;","$SAA_SignalType_Thargoid;","$SAA_SignalType_Human;","$SAA_SignalType_Guardian;"
                                          };

                    grd.GetLineHeader = (row) => { return row == 1 ? headers2 : row == 0 ? headers1 : null; };
                    grd.GetLineStatus = (row) =>
                    {
                        if (row < saaentries.Count)
                        {
                            for (int rp = row + 1; rp < saaentries.Count; rp++)
                            {
                                if (saaentries[rp].BodyName == saaentries[row].BodyName && (saaentries[rp].EventTimeUTC - saaentries[row].EventTimeUTC) < new TimeSpan(30, 0, 0, 0))
                                    return BaseUtils.CSVWriteGrid.LineStatus.Skip; // if matches one in front, and its less than 30 days from it, ignore
                            }

                            return BaseUtils.CSVWriteGrid.LineStatus.OK;
                        }
                        else
                            return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                    };

                    grd.GetLine = (r) =>
                    {
                        var entry = saaentries[r];
                        string signals = string.Join(",", entry.Signals.Select(x => x.Type_Localised));

                        JournalScan scanof = scanentries.Find(x => x.FindRing(entry.BodyName) != null);

                        bool showrocky = true, showmr = true, showmetalic = true, showicy = true;       // only used if item appears more than once below

                        string ringtype = "", mass = "", innerrad = "", outerrad = "";
                        if (scanof != null)
                        {
                            var ri = scanof.FindRing(entry.BodyName);
                            ringtype = ri.RingClassID.ToString().SplitCapsWordFull();

                            if (ri.RingClassID == JournalScan.StarPlanetRing.RingClassEnum.Metalic)
                            {
                                showicy = showrocky = showmr = false;
                            }
                            else if (ri.RingClassID == JournalScan.StarPlanetRing.RingClassEnum.MetalRich)
                            {
                                showicy = showrocky = showmetalic = false;
                            }
                            else if (ri.RingClassID == JournalScan.StarPlanetRing.RingClassEnum.Rocky)
                            {
                                showicy = showmetalic = showmr = false;
                            }
                            else if (ri.RingClassID == JournalScan.StarPlanetRing.RingClassEnum.Icy)
                            {
                                showrocky = showmetalic = showmr = false;
                            }

                            mass = ri.MassMT.ToStringInvariant();
                            innerrad = (ri.InnerRad / BodyPhysicalConstants.oneLS_m).ToStringInvariant("N3");
                            outerrad = (ri.OuterRad / BodyPhysicalConstants.oneLS_m).ToStringInvariant("N3");
                        }

                        //string sig = string.Join(",", entry.Signals.Select(x=>x.Type)); // debug

                        return new object[] { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(entry.EventTimeUTC), entry.BodyName, ringtype,
                                mass,innerrad,outerrad ,
                                entry.ContainsStr(fdname[0]),entry.ContainsStr(fdname[1]),entry.ContainsStr(fdname[2]),entry.ContainsStr(fdname[3]),
                                entry.ContainsStr(fdname[4]),entry.ContainsStr(fdname[5]),entry.ContainsStr(fdname[6]),entry.ContainsStr(fdname[7]),
                                entry.ContainsStr(fdname[8], showicy), entry.ContainsStr(fdname[9]),

                                // "Bauxite","Indite","Alexandrite","Monazite", // rocky
                                entry.ContainsStr(fdname[10]),entry.ContainsStr(fdname[11]),entry.ContainsStr(fdname[12],showrocky),entry.ContainsStr(fdname[13], showrocky),
                                // "Musgravite","Benitoite","Serendibite","Rhodplumsite",          // rocky
                                entry.ContainsStr(fdname[14]),entry.ContainsStr(fdname[15]),entry.ContainsStr(fdname[16],showrocky),entry.ContainsStr(fdname[17],showrocky),

                                // "Rhodplumsite","Serendibite","Platinum","Monazite","Painite",   // metal rich  
                                entry.ContainsStr(fdname[18],showmr),entry.ContainsStr(fdname[19],showmr),entry.ContainsStr(fdname[20],showmr),entry.ContainsStr(fdname[21],showmr),entry.ContainsStr(fdname[22],showmr),

                                // "Rhodplumsite","Serendibite","Platinum","Monazite","Painite", // metalic
                                entry.ContainsStr(fdname[23],showmetalic),entry.ContainsStr(fdname[24],showmetalic),entry.ContainsStr(fdname[25],showmetalic),entry.ContainsStr(fdname[26],showmetalic),entry.ContainsStr(fdname[27],showmetalic),

                                entry.ContainsStr(fdname[28]),entry.ContainsStr(fdname[29]),entry.ContainsStr(fdname[30]),entry.ContainsStr(fdname[31]),entry.ContainsStr(fdname[32]),
                            };
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());


                }
                else if (frm.SelectedIndex >= 4)
                {
                    var entries = JournalEntry.GetByEventType(JournalTypeEnum.Scan, EDCommander.CurrentCmdrID, frm.StartTimeUTC, frm.EndTimeUTC);
                    var scans = entries.ConvertAll(x => (JournalScan)x);

                    bool ShowStars = frm.SelectedIndex == 4 || frm.SelectedIndex == 6;
                    bool ShowPlanets = frm.SelectedIndex == 4 || frm.SelectedIndex == 5;
                    bool ShowBeltClusters = frm.SelectedIndex == 4;

                    if (ShowPlanets)
                    {
                        // because they come from the DB, they don't have the mapped/efficient mapped flags set. So we need to set them
                        List<JournalSAAScanComplete> mappings = JournalEntry.GetByEventType(JournalTypeEnum.SAAScanComplete, EDCommander.CurrentCmdrID, frm.StartTimeUTC, frm.EndTimeUTC).ConvertAll(x => (JournalSAAScanComplete)x);

                        foreach (var m in mappings)
                        {
                            var scan = scans.Find(x => x.BodyID == m.BodyID && x.BodyName == m.BodyName);
                            if (scan != null)
                            {
                                //System.Diagnostics.Debug.WriteLine($"Set mapping {scan.BodyName}");
                                scan.SetMapped(true, m.ProbesUsed <= m.EfficiencyTarget);
                            }
                        }
                    }

                    if (!CSVHelpers.OutputScanCSV(scans, frm.Path, frm.Comma, frm.IncludeHeader, ShowStars, ShowPlanets, ShowPlanets, ShowBeltClusters))
                        throw new Exception();      // throw to get to scan excel error

                    if (frm.AutoOpen)
                        System.Diagnostics.Process.Start(frm.Path);


                }
                else
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                    grd.SetCSVDelimiter(frm.Comma);

                    List<SystemNoteClass> sysnotecache = new List<SystemNoteClass>();
                    string[] colh = null;

                    grd.GetLineStatus += delegate (int r)
                    {
                        if (r < dataGridViewTravel.Rows.Count)
                        {
                            HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Tag;
                            return (dataGridViewTravel.Rows[r].Visible &&
                                    he.EventTimeUTC.CompareTo(frm.StartTimeUTC) >= 0 &&
                                    he.EventTimeUTC.CompareTo(frm.EndTimeUTC) <= 0) ? BaseUtils.CSVWriteGrid.LineStatus.OK : BaseUtils.CSVWriteGrid.LineStatus.Skip;
                        }
                        else
                            return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                    };

                    if (frm.SelectedIndex == 1)     // export fsd jumps
                    {
                        colh = new string[] { "Time", "Name", "X", "Y", "Z", "Distance", "Fuel Used", "Fuel Left", "Boost", "Note" };

                        grd.VerifyLine += delegate (int r)      // addition qualifier for FSD jump
                        {
                            HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Tag;
                            return he.EntryType == JournalTypeEnum.FSDJump;
                        };

                        grd.GetLine += delegate (int r)
                        {
                            HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Tag;
                            EliteDangerousCore.JournalEvents.JournalFSDJump fsd = he.journalEntry as EliteDangerousCore.JournalEvents.JournalFSDJump;

                            return new Object[] {
                                EDDConfig.Instance.ConvertTimeToSelectedFromUTC(fsd.EventTimeUTC),
                                fsd.StarSystem,
                                fsd.StarPos.X,
                                fsd.StarPos.Y,
                                fsd.StarPos.Z,
                                fsd.JumpDist,
                                fsd.FuelUsed,
                                fsd.FuelLevel,
                                fsd.BoostUsed,
                                he.GetNoteText,
                            };

                        };
                    }
                    else
                    {
                        colh = new string[] { "Time", "Event", "System", "Body",            //0
                                              "Ship", "Summary", "Description", "Detailed Info",        //4
                                              "Note", "Travel Dist", "Travel Time", "Travel Jumps",     //8
                                              "Travelled MisJumps" , "X", "Y","Z" ,     //12
                                              "JID", "EDSMID"};             //16

                        grd.GetLine += delegate (int r)
                        {
                            HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Tag;
                            he.FillInformation(out string EventDescription, out string EventDetailedInfo);
                            return new Object[] {
                                EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC),
                                he.EventSummary,
                                (he.System != null) ? he.System.Name : "Unknown",    // paranoia
                                he.WhereAmI,
                                he.ShipInformation != null ? he.ShipInformation.Name : "Unknown",
                                he.EventSummary,
                                EventDescription,
                                EventDetailedInfo,
                                dataGridViewTravel.Rows[r].Cells[4].Value,
                                he.isTravelling ? he.TravelledDistance.ToString("0.0") : "",
                                he.isTravelling ? he.TravelledSeconds.ToString() : "",
                                he.isTravelling ? he.Travelledjumps.ToString() : "",
                                he.isTravelling ? he.TravelledMissingjump.ToString() : "",
                                he.System.X,
                                he.System.Y,
                                he.System.Z,
                                he.Journalid,
                                he.System.EDSMID,
                            };
                        };

                        if (frm.SelectedIndex == 2 || frm.SelectedIndex == 3)     // export notes
                        {
                            grd.VerifyLine += delegate (int r)      // second hook to reject line
                            {
                                HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Tag;
                                if (he.journalEntry.SNC != null)
                                {
                                    if (sysnotecache.Contains(he.journalEntry.SNC))
                                        return false;
                                    else
                                    {
                                        if (frm.SelectedIndex == 3)
                                            sysnotecache.Add(he.journalEntry.SNC);
                                        return true;
                                    }
                                }
                                else
                                    return false;
                            };
                        }
                    }

                    grd.GetHeader += delegate (int c)
                    {
                        return (c < colh.Length && frm.IncludeHeader) ? colh[c] : null;
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }

        }
    }
}
