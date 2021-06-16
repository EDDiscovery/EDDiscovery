﻿/*
 * Copyright © 2019 EDDiscovery development team
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
 
using BaseUtils;
using BaseUtils.WebServer;
using EliteDangerousCore;
using EliteDangerousCore.UIEvents;
using BaseUtils.JSON;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

// how to debug
// -wsf c:\code\eddiscovery\WebSite\EDD  to point EDD at the website
// open the SLN in the website in another VS instance
// use ISS Express (Google Chrome) to run the website - you will be able to single step etc
// until the js file is needed, a debug point will show not bound.  thats okay.
// use the console inspector control-shift-I to debug also in chrome

namespace EDDiscovery.WebServer
{
    // JSON Websockets Interface definition:
    // Query requesttype=journal : fields start, length
    //          responsetype = journalrequest, fields   : firstrow = -1 none, or first row number
    //                                                  : Commander
    //                                                  : rows[] containing journaliconpath, eventtimeutc, summary, info, note
    // Push responsetype = journalrefresh, fields as per journalrequest
    // Push responsetype = journalpush, fields as per journalrequest, insert at front, new rows
    //
    // Query requesttype=status : fields entry number
    //          responsetype = status, fields           : entry = -1 none, or entry number
    //                                                  : SystemData object containing records
    //                                                  : EDDB object
    //                                                  : Ship object
    //                                                  : Travel object
    //                                                  : Bodyname, HomeDist, SolDist, GameMode, Credits, Commander, Mode
    // Push responsetype = status, fields as above
    //
    // Query requesttype=indicator. No fields
    //          responsetype = indicator, fields        : Various status fields
    //
    // Push responsetype = indicatorpush, fields as above
    //
    // Query requesttype= presskey, fields              : key = binding name
    //          responsetype = status, 100 or 400.
    //
    // Query requesttype=scandata : fields entry number
    //          responsetype = entry, fields..          : entry = -1 none, or entry number. See code for fields
    //                                                  

    public class EDDWebServer
    {
        public Action<string> LogIt;
        public int Port { get { return port; } set { port = value; } }
        public bool Running { get { return httpws != null; } }

        private int port = 0;

        Server httpws;
        HTTPDispatcher httpdispatcher;

        HTTPFileNode mainwebsitefiles;
        HTTPZipNode mainwebsitezipfiles;

        EDDIconNodes iconnodes;
        EDDScanDisplay scandisplay;
        JSONDispatcher jsondispatch;

        JournalRequest journalsender;
        StatusRequest statussender;
        IndicatorRequest indicator;
        ScanDataRequest scandata;
        PressKeyRequest presskey;

        EDDiscoveryForm discoveryform;

        public EDDWebServer(EDDiscoveryForm frm)
        {
            discoveryform = frm;
        }

        public bool Start(string servefrom)       // null if okay
        {
            // HTTP server
            httpws = new Server("http://*:" + port.ToStringInvariant() + "/");
            httpws.ServerLog = (s) => { LogIt?.Invoke(s); };

            httpdispatcher = new HTTPDispatcher();
            httpdispatcher.RootNodeTranslation = "/index.html";

            // Serve ICONS from path - order is important - first gets first dibs at the path
            iconnodes = new EDDIconNodes();
            httpdispatcher.AddPartialPathNode("/journalicons/", iconnodes);     // journal icons come from this dynamic source
            httpdispatcher.AddPartialPathNode("/statusicons/", iconnodes);     // status icons come from this dynamic source
            httpdispatcher.AddPartialPathNode("/images/", iconnodes);           // give full eddicons path

            // scan display
            scandisplay = new EDDScanDisplay(discoveryform,httpws);
            httpdispatcher.AddPartialPathNode("/systemmap/", scandisplay);   // serve a display of this system

            if (servefrom.Contains(".zip"))                                 // fall back, none of the above matching
            {
                mainwebsitezipfiles = new HTTPZipNode(servefrom);
                httpdispatcher.AddPartialPathNode("/", mainwebsitezipfiles);
            }
            else
            {
                mainwebsitefiles = new HTTPFileNode(servefrom);
                httpdispatcher.AddPartialPathNode("/", mainwebsitefiles);
            }

            // add to the server a HTTP responser
            httpws.AddHTTPResponder((lr, lrdata) => { return httpdispatcher.Response(lr); }, httpdispatcher);

            // JSON dispatcher..
            jsondispatch = new JSONDispatcher();

            journalsender = new JournalRequest(discoveryform);
            jsondispatch.Add("journal", journalsender);      // event journal

            statussender = new StatusRequest(discoveryform);
            jsondispatch.Add("status", statussender);   // event status

            indicator = new IndicatorRequest();
            jsondispatch.Add("indicator", indicator);   // indicator query

            scandata = new ScanDataRequest(discoveryform);
            jsondispatch.Add("scandata", scandata);   // indicator query

            presskey = new PressKeyRequest(discoveryform);
            jsondispatch.Add("presskey", presskey);   // and a key press

            // add for protocol EDDJSON the responder.

            httpws.AddWebSocketsResponder("EDDJSON",
                    (ctx, ws, wsrr, buf, lrdata) => { jsondispatch.Response(ctx, ws, wsrr, buf, lrdata); },
                    jsondispatch);

            bool ok = httpws.Run();

            if (ok)
            {
                discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
                discoveryform.OnNewUIEvent += Discoveryform_OnNewUIEvent;
                discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            }
            else
                httpws = null;

            return ok;
        }

        public bool Stop()      // note it does not wait for threadpools
        {
            if (httpws != null)
            {
                httpws.Stop();
                httpws = null;
                discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
                discoveryform.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
                discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            }

            return true;
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            httpws.SendWebSockets(journalsender.Refresh(-1, 50), false); // refresh history
            httpws.SendWebSockets(statussender.Refresh(-1), false); // and status
            httpws.SendWebSockets(scandata.Refresh(-1), false); // and scan data
        }

        private void Discoveryform_OnNewUIEvent(UIEvent obj)
        {
            if ( obj.EventTypeID == UITypeEnum.OverallStatus )
            {
                httpws.SendWebSockets(indicator.Refresh(obj as UIOverallStatus),false); // push indicator push
            }
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            httpws.SendWebSockets(journalsender.Push(), false); // refresh history
            httpws.SendWebSockets(statussender.Push(), false); // refresh status
            if ( he.EntryType == JournalTypeEnum.Scan || he.EntryType == JournalTypeEnum.FSSSignalDiscovered || he.EntryType == JournalTypeEnum.SAASignalsFound ||
                        he.EntryType == JournalTypeEnum.SAAScanComplete)
            {
                httpws.SendWebSockets(scandata.Push(), false); // refresh status
                httpws.SendWebSockets(scandisplay.Notify(), false); // tell it its changed
            }
        }

        // deal with the icon roots

        class EDDIconNodes : IHTTPNode
        {
            public NodeResponse Response(string partialpath, HttpListenerRequest request)
            {
                System.Diagnostics.Debug.WriteLine("Serve icon " + partialpath);

                if (partialpath.Contains(".png"))
                {
                    string nopng = partialpath.Replace(".png", "");

                    Bitmap img;

                    if ( nopng.Contains("."))       // if path, use it
                        img = BaseUtils.Icons.IconSet.GetIcon(nopng) as Bitmap;
                    else if (BaseUtils.Icons.IconSet.Instance.Contains("Journal." + nopng))  // no path, may be a journal one
                        img = BaseUtils.Icons.IconSet.GetIcon("Journal." + nopng) as Bitmap;
                    else if (BaseUtils.Icons.IconSet.Instance.Contains("General." + nopng))  // no path, may be a journal one
                        img = BaseUtils.Icons.IconSet.GetIcon("General." + nopng) as Bitmap;
                    else
                        img = BaseUtils.Icons.IconSet.GetIcon(nopng) as Bitmap;

                    //try       // debug only
                    //{
                    Bitmap bmpclone = img.Clone() as Bitmap;
                    var cnv = bmpclone.ConvertTo(System.Drawing.Imaging.ImageFormat.Png);   // this converts to png and returns the raw PNG bytes..
                    return new NodeResponse(cnv, "image/png");
                    //}
                    //catch (Exception ex)
                    //{
                    //System.Diagnostics.Debug.WriteLine("..Convert exception " + ex);
                    //return null;
                    //}
                }

                return null;
            }
        }

        class EDDScanDisplay : IHTTPNode
        {
            private EDDiscoveryForm discoveryform;
            private Server server;

            public EDDScanDisplay(EDDiscoveryForm f, Server serverp)
            {
                discoveryform = f;
                server = serverp;
            }

            public JToken Notify()       // a full refresh of journal history
            {
                JObject response = new JObject();
                response["responsetype"] = "systemmapchanged";
                return response;
            }

            public NodeResponse Response(string partialpath, HttpListenerRequest request)
            {
                System.Diagnostics.Debug.WriteLine("Serve Scan Display " + partialpath);
                foreach (var k in request.QueryString.AllKeys)
                    System.Diagnostics.Debug.WriteLine("Key {0} = {1}", k, request.QueryString[k]);

                int entry = (request.QueryString["entry"] ?? "-1").InvariantParseInt(-1);
                bool checkEDSM = (request.QueryString["EDSM"] ?? "false").InvariantParseBool(false);

                Bitmap img = null;
                JObject response = new JObject();
                response["responsetype"] = "scandisplayobjects";
                JArray objectlist = new JArray();

                var hl = discoveryform.history;
                if (hl.Count > 0)
                {
                    if (entry < 0 || entry >= hl.Count)
                        entry = hl.Count - 1;

                    // seen instances of exceptions accessing icons in different threads.  so push up to discovery form. need to investigate.
                    discoveryform.Invoke((MethodInvoker)delegate
                    {
                        StarScan.SystemNode sn = hl.StarScan.FindSystemSynchronous(hl.EntryOrder()[entry].System, checkEDSM);

                        if (sn != null)
                        {
                            int starsize = (request.QueryString["starsize"] ?? "48").InvariantParseInt(48);
                            int width = (request.QueryString["width"] ?? "800").InvariantParseInt(800);
                            SystemDisplay sd = new SystemDisplay();
                            sd.ShowMoons = (request.QueryString["showmoons"] ?? "true").InvariantParseBool(true);
                            sd.ShowOverlays = (request.QueryString["showbodyicons"] ?? "true").InvariantParseBool(true);
                            sd.ShowMaterials = (request.QueryString["showmaterials"] ?? "true").InvariantParseBool(true);
                            sd.ShowAllG = (request.QueryString["showgravity"] ?? "true").InvariantParseBool(true);
                            sd.ShowHabZone = (request.QueryString["showhabzone"] ?? "true").InvariantParseBool(true);
                            sd.ShowStarClasses = (request.QueryString["showstarclass"] ?? "true").InvariantParseBool(true);
                            sd.ShowPlanetClasses = (request.QueryString["showplanetclass"] ?? "true").InvariantParseBool(true);
                            sd.ShowDist = (request.QueryString["showdistance"] ?? "true").InvariantParseBool(true);
                            sd.ShowEDSMBodies = checkEDSM;
                            sd.SetSize(starsize);
                            sd.Font = new Font("MS Sans Serif", 8.25f);
                            sd.LargerFont = new Font("MS Sans Serif", 10f);
                            sd.FontUnderlined = new Font("MS Sans Serif", 8.25f, FontStyle.Underline);
                            ExtendedControls.ExtPictureBox imagebox = new ExtendedControls.ExtPictureBox();
                            sd.DrawSystem(imagebox, width, sn, null, null);
                            imagebox.Render();

                            foreach (var e in imagebox.Elements)
                            {
                                if (e.ToolTipText.HasChars())
                                {
                                    System.Diagnostics.Debug.WriteLine("{0} = {1}", e.Location, e.ToolTipText);
                                    objectlist.Add(new JObject() { ["left"] = e.Position.X, ["top"] = e.Position.Y, ["right"] = e.Location.Right, ["bottom"] = e.Location.Bottom, ["text"] = e.ToolTipText });
                                }
                            }

                            img = imagebox.Image.Clone() as Bitmap;
                            imagebox.Dispose();
                            }
                    });

                }
                else
                {
                    discoveryform.Invoke((MethodInvoker)delegate
                    {
                        img = BaseUtils.Icons.IconSet.GetIcon("Bodies.Unknown") as Bitmap;
                    });
                }

                response["objectlist"] = objectlist;
                server.SendWebSockets(response, false); // refresh history

                Bitmap bmpclone = img.Clone() as Bitmap;
                var cnv = bmpclone.ConvertTo(System.Drawing.Imaging.ImageFormat.Png);   // this converts to png and returns the raw PNG bytes..
                return new NodeResponse(cnv, "image/png");
            }
        }

        public class JournalRequest : IJSONNode
        {
            private EDDiscoveryForm discoveryform;
            public JournalRequest(EDDiscoveryForm f)
            {
                discoveryform = f;
            }

            public JToken Response(string key, JToken message, HttpListenerRequest request)     // response to requesttype=journal
            {
                System.Diagnostics.Debug.WriteLine("Journal Request " + key + " Fields " + message.ToString());

                int startindex = message["start"].Int(0);
                int length = message["length"].Int(0);

                return MakeResponse(startindex, length , "journalrequest" );      // responsetype = journalrequest
            }

            public JToken Refresh(int startindex, int length)       // a full refresh of journal history
            {
                return MakeResponse(startindex, length, "journalrefresh");
            }

            public JToken Push()                                    // push latest entry
            {
                return MakeResponse(-1, 1, "journalpush");
            }

            private JToken MakeResponse(int startindex, int length, string rt)     // generate a response over this range
            {
                if (discoveryform.InvokeRequired)
                {
                    return (JToken)discoveryform.Invoke(new Func<JToken>(() => MakeResponse(startindex, length, rt)));
                }
                else
                {
                    JToken response;

                    var hl = discoveryform.history;
                    if (hl.Count == 0)
                    {
                        response = new JObject();
                        response["responsetype"] = rt;
                        response["firstrow"] = -1;
                    }
                    else
                    {
                        if (startindex < 0 || startindex >= hl.Count)
                            startindex = hl.Count - 1;

                        response = NewJRec(hl, rt, startindex, length);
                    }

                    response["Commander"] = EDCommander.Current.Name;

                    return response;
                }
            }

            public JToken NewJRec(HistoryList hl, string type, int startindex, int length)
            {
                JObject response = new JObject();
                response["responsetype"] = type;
                response["firstrow"] = startindex;

                JArray jarray = new JArray();
                for (int i = startindex; i > Math.Max(-1, startindex - length); i--)
                {
                    EliteDangerousCore.HistoryEntry he = hl.EntryOrder()[i];

                    JArray jent = new JArray();
                    jent.Add(he.journalEntry.GetIconPackPath);
                    jent.Add(he.journalEntry.EventTimeUTC);
                    he.FillInformation(out string info, out string detailed);
                    string note = (he.SNC != null) ? he.SNC.Note : "";
                    jent.Add(he.EventSummary);
                    jent.Add(info);
                    jent.Add(note);
                    jarray.Add(jent);
                }

                response["rows"] = jarray;
                return response;
            }
        }

        public class StatusRequest : IJSONNode
        {
            private EDDiscoveryForm discoveryform;

            public StatusRequest(EDDiscoveryForm f)
            {
                discoveryform = f;
            }

            public JToken Refresh(int entry)        // -1 mean latest
            {
                return MakeResponse(entry, "status");
            }

            public JToken Push()                                    // push latest entry
            {
                return MakeResponse(-1, "status");
            }

            public JToken Response(string key, JToken message, HttpListenerRequest request)
            {
                System.Diagnostics.Debug.WriteLine("Status Request " + key + " Fields " + message.ToString());
                int entry = message["entry"].Int(0);
                return MakeResponse(entry, "status");
            }

            private JToken MakeResponse(int entry, string rt)
            {
                if (discoveryform.InvokeRequired)
                {
                    return (JToken)discoveryform.Invoke(new Func<JToken>(() => MakeResponse(entry, rt)));
                }
                else
                {
                    JToken response = null;

                    var hl = discoveryform.history;
                    if (hl.Count == 0)
                    {
                        response = new JObject();
                        response["responsetype"] = rt;
                        response["entry"] = -1;
                    }
                    else
                    {
                        if (entry < 0 || entry >= hl.Count)
                            entry = hl.Count - 1;

                        response = NewSRec(hl, rt, entry);
                    }

                    return response;
                }
            }

            public JToken NewSRec(EliteDangerousCore.HistoryList hl, string type, int entry)       // entry = -1 means latest
            {
                HistoryEntry he = hl.EntryOrder()[entry];

                JObject response = new JObject();
                response["responsetype"] = type;
                response["entry"] = entry;

                JObject systemdata = new JObject();
                systemdata["System"] = he.System.Name;
                systemdata["SystemAddress"] = he.System.SystemAddress;
                systemdata["PosX"] = he.System.X.ToStringInvariant("0.00");
                systemdata["PosY"] = he.System.Y.ToStringInvariant("0.00");
                systemdata["PosZ"] = he.System.Z.ToStringInvariant("0.00");
                systemdata["EDSMID"] = he.System.EDSMID.ToStringInvariant();
                systemdata["VisitCount"] = hl.GetVisitsCount(he.System.Name);
                response["SystemData"] = systemdata;

                // TBD.. if EDSMID = 0 , we may not have looked at it in the historywindow, do we want to do a lookup?

                JObject sysstate = new JObject();

                hl.ReturnSystemInfo(he, out string allegiance, out string economy, out string gov, out string faction, out string factionstate, out string security);
                sysstate["State"] = factionstate;
                sysstate["Allegiance"] = allegiance;
                sysstate["Gov"] = gov;
                sysstate["Economy"] = economy;
                sysstate["Faction"] = faction;
                sysstate["Security"] = security;
                sysstate["MarketID"] = he.MarketID;
                response["EDDB"] = sysstate;

                var mcl = hl.MaterialCommoditiesMicroResources.Get(he.MaterialCommodity);

                int cargocount = MaterialCommoditiesMicroResourceList.CargoCount(mcl);
                string shipname = "N/A", fuel = "N/A", range = "N/A", tanksize = "N/A";
                string cargo = cargocount.ToStringInvariant();

                ShipInformation si = he.ShipInformation;
                if (si != null)
                {
                    shipname = si.ShipFullInfo(cargo: false, fuel: false);
                    if (si.FuelLevel > 0)
                        fuel = si.FuelLevel.ToStringInvariant("0.#");
                    if (si.FuelCapacity > 0 )
                        tanksize = si.FuelCapacity.ToStringInvariant("0.#");

                    EliteDangerousCalculations.FSDSpec fsd = si.GetFSDSpec();
                    if (fsd != null)
                    {
                        EliteDangerousCalculations.FSDSpec.JumpInfo ji = fsd.GetJumpInfo(cargocount,
                                                                    si.ModuleMass() + si.HullMass(), si.FuelLevel, si.FuelCapacity / 2);
                        range = ji.cursinglejump.ToString("N2") + "ly";
                    }

                    int cargocap = si.CargoCapacity();

                    if ( cargocap > 0)
                        cargo += "/" + cargocap.ToStringInvariant();
                }

                JObject ship = new JObject();
                ship["Ship"] = shipname;
                ship["Fuel"] = fuel;
                ship["Range"] = range;
                ship["TankSize"] = tanksize;
                ship["Cargo"] = cargo;
                ship["Data"] = MaterialCommoditiesMicroResourceList.DataCount(mcl).ToStringInvariant();
                ship["Materials"] = MaterialCommoditiesMicroResourceList.MaterialsCount(mcl).ToStringInvariant();
                ship["MicroResources"] = MaterialCommoditiesMicroResourceList.MicroResourcesCount(mcl).ToStringInvariant();
                response["Ship"] = ship;

                JObject travel = new JObject();

                if (he.isTravelling)
                {
                    travel["Dist"] = he.TravelledDistance.ToStringInvariant("0.0");
                    travel["Jumps"] = he.Travelledjumps.ToStringInvariant();
                    travel["UnknownJumps"] = he.TravelledMissingjump.ToStringInvariant();
                    travel["Time"] = he.TravelledSeconds.ToString();
                }
                else
                    travel["Time"] = travel["Jumps"] = travel["Dist"] = "";

                response["Travel"] = travel;

                response["Bodyname"] = he.WhereAmI;

                if (he.System.HasCoordinate)         // cursystem has them?
                {
                    response["HomeDist"] = he.System.Distance(EDCommander.Current.HomeSystemIOrSol).ToString("0.##");
                    response["SolDist"] = he.System.Distance(0, 0, 0).ToString("0.##");
                }
                else
                    response["SolDist"] = response["HomeDist"] = "-";

                response["GameMode"] = he.GameModeGroup;
                response["Credits"] = he.Credits.ToStringInvariant();
                response["Commander"] = EDCommander.Current.Name;
                response["Mode"] = he.TravelState.ToString();

                return response;
            }
        }

        public class IndicatorRequest : IJSONNode
        {
            UIOverallStatus uistate;

            public JToken Refresh(UIOverallStatus stat)     // request push of new states
            {
                uistate = stat;
                return NewIRec(stat, "indicatorpush");
            }

            public JToken Response(string key, JToken message, HttpListenerRequest request) // request indicator state
            {
                System.Diagnostics.Debug.WriteLine("indicator Request " + key + " Fields " + message.ToString());
                return NewIRec(uistate, "indicator");
            }

            //EliteDangerousCore.UIEvents.UIOverallStatus status,
            public JToken NewIRec(UIOverallStatus stat, string type)       // entry = -1 means latest
            {
                JObject response = new JObject();
                response["responsetype"] = type;

                if (stat == null) // because we don't have one
                {
                    response["ShipType"] = "None";      // sending this clears the indicators
                }
                else
                {
                    response["ShipType"] = stat.MajorMode.ToString();
                    response["Mode"] = stat.Mode.ToString();
                    response["GUIFocus"] = stat.Focus.ToString();

                    JArray pips = new JArray();
                    pips.Add(stat.Pips.Systems);
                    pips.Add(stat.Pips.Engines);
                    pips.Add(stat.Pips.Weapons);
                    response["Pips"] = pips;
                    response["ValidPips"] = stat.Pips.Valid;

                    response["Lights"] = stat.Flags.Contains(UITypeEnum.Lights);
                    response["Firegroup"] = stat.Firegroup;
                    response["HasLatLong"] = stat.Flags.Contains(UITypeEnum.HasLatLong);

                    JArray pos = new JArray();
                    pos.Add(stat.Pos.Latitude);
                    pos.Add(stat.Pos.Longitude);
                    pos.Add(stat.Pos.Altitude);
                    pos.Add(stat.Heading); // heading
                    response["Position"] = pos;
                    response["ValidPosition"] = stat.Pos.ValidPosition;
                    response["ValidAltitude"] = stat.Pos.ValidAltitude;
                    response["ValidHeading"] = stat.ValidHeading;
                    response["AltitudeFromAverageRadius"] = stat.Pos.AltitudeFromAverageRadius;
                    response["PlanetRadius"] = stat.PlanetRadius;
                    response["ValidPlanetRadius"] = stat.ValidRadius;

                    // main ship

                    response["Docked"] = stat.Flags.Contains(UITypeEnum.Docked);       // S
                    response["Landed"] = stat.Flags.Contains(UITypeEnum.Landed);  // S
                    response["LandingGear"] = stat.Flags.Contains(UITypeEnum.LandingGear);   // S
                    response["ShieldsUp"] = stat.Flags.Contains(UITypeEnum.ShieldsUp);         //S
                    response["Supercruise"] = stat.Flags.Contains(UITypeEnum.Supercruise);   //S
                    response["FlightAssist"] = stat.Flags.Contains(UITypeEnum.FlightAssist);     //S
                    response["HardpointsDeployed"] = stat.Flags.Contains(UITypeEnum.HardpointsDeployed); //S
                    response["InWing"] = stat.Flags.Contains(UITypeEnum.InWing);   // S
                    response["CargoScoopDeployed"] = stat.Flags.Contains(UITypeEnum.CargoScoopDeployed);   // S
                    response["SilentRunning"] = stat.Flags.Contains(UITypeEnum.SilentRunning);    // S
                    response["ScoopingFuel"] = stat.Flags.Contains(UITypeEnum.ScoopingFuel);     // S

                    // srv

                    response["SrvHandbrake"] = stat.Flags.Contains(UITypeEnum.SrvHandbrake);
                    response["SrvTurret"] = stat.Flags.Contains(UITypeEnum.SrvTurret);
                    response["SrvUnderShip"] = stat.Flags.Contains(UITypeEnum.SrvUnderShip);
                    response["SrvDriveAssist"] = stat.Flags.Contains(UITypeEnum.SrvDriveAssist);
                    response["SrvHighBeam"] = stat.Flags.Contains(UITypeEnum.SrvHighBeam);

                    // main ship
                    response["FsdMassLocked"] = stat.Flags.Contains(UITypeEnum.FsdMassLocked);
                    response["FsdCharging"] = stat.Flags.Contains(UITypeEnum.FsdCharging);
                    response["FsdCooldown"] = stat.Flags.Contains(UITypeEnum.FsdCooldown);
                    response["FsdJump"] = stat.Flags.Contains(UITypeEnum.FsdJump);

                    // all ships

                    response["LowFuel"] = stat.Flags.Contains(UITypeEnum.LowFuel);

                    // main ship
                    response["OverHeating"] = stat.Flags.Contains(UITypeEnum.OverHeating);
                    response["IsInDanger"] = stat.Flags.Contains(UITypeEnum.IsInDanger);
                    response["BeingInterdicted"] = stat.Flags.Contains(UITypeEnum.BeingInterdicted);
                    response["HUDInAnalysisMode"] = stat.Flags.Contains(UITypeEnum.HUDInAnalysisMode);
                    response["NightVision"] = stat.Flags.Contains(UITypeEnum.NightVision);

                    response["GlideMode"] = stat.Flags.Contains(UITypeEnum.GlideMode);  // odyssey

                    // all

                    response["LegalState"] = stat.LegalState;
                    response["BodyName"] = stat.BodyName;

                    // Odyssey on foot
                    response["AimDownSight"] = stat.Flags.Contains(UITypeEnum.AimDownSight);
                    response["BreathableAtmosphere"] = stat.Flags.Contains(UITypeEnum.BreathableAtmosphere);

                    response["Oxygen"] = stat.Oxygen;
                    response["Gravity"] = stat.Gravity;
                    response["Health"] = stat.Health;
                    response["Temperature"] = stat.Temperature;
                    response["TemperatureState"] = stat.TemperatureState.ToString();
                    response["SelectedWeapon"] = stat.SelectedWeapon; ;
                    response["SelectedWeaponLocalised"] = stat.SelectedWeapon_Localised;

                }

                return response;
            }
        }

        public class ScanDataRequest : IJSONNode
        {
            private EDDiscoveryForm discoveryform;

            public ScanDataRequest(EDDiscoveryForm f)
            {
                discoveryform = f;
            }

            public JToken Refresh(int entry)        // -1 mean latest
            {
                return MakeResponse(entry, "scandata");
            }

            public JToken Push()                    // push latest entry
            {
                return MakeResponse(-1, "scandata");
            }

            public JToken Response(string key, JToken message, HttpListenerRequest request) // request indicator state
            {
                System.Diagnostics.Debug.WriteLine("scandata Request " + key + " Fields " + message.ToString());
                int entry = message["entry"].Int(0);
                return MakeResponse(entry, "scandata");
            }

            //EliteDangerousCore.UIEvents.UIOverallStatus status,
            public JToken MakeResponse(int entry, string type)       // entry = -1 means latest
            {
                if (discoveryform.InvokeRequired)
                {
                    return (JToken)discoveryform.Invoke(new Func<JToken>(() => MakeResponse(entry, type)));
                }
                else
                {
                    JObject response = new JObject();
                    response["responsetype"] = type;

                    var hl = discoveryform.history;
                    if (hl.Count == 0)
                    {
                        response["entry"] = -1;
                    }
                    else
                    {
                        if (entry < 0 || entry >= hl.Count)
                            entry = hl.Count - 1;

                        response["entry"] = entry;

                        HistoryEntry he = hl[entry];

                        var scannode = discoveryform.history.StarScan.FindSystemSynchronous(he.System,false);        // get data without EDSM - don't want a web lookup
                        var bodylist = scannode?.Bodies.ToList();       // may be null

                        response["Count"] = bodylist?.Count ?? 0;

                        JArray jbodies = new JArray();
                        foreach( var body in bodylist.EmptyIfNull())
                        {
                            JObject jo = new JObject()
                            {
                                ["NodeType"] = body.NodeType.ToString(),
                                ["FullName"] = body.FullName,
                                ["OwnName"] = body.OwnName,
                                ["CustomName"] = body.CustomName,
                                ["CustomNameOrOwnName"] = body.CustomNameOrOwnname,
                                ["Level"] = body.Level,
                                ["BodyID"] = body.BodyID,
                            };

                            JToken jdata = null;

                            if ( body.ScanData != null )
                            {
                                jdata = JToken.FromObject(body.ScanData, true, null, 5, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly);
                            }
                            jo["Scan"] = jdata;

                            jbodies.Add(jo);
                        }

                        response["Bodies"] = jbodies;
                    }

                    return response;
                }
            }
        }

        public class PressKeyRequest : IJSONNode
        {
            private EDDiscoveryForm discoveryform;

            public PressKeyRequest(EDDiscoveryForm f)
            {
                discoveryform = f;
            }
            
            public JToken Response(string key, JToken message, HttpListenerRequest request)
            {
                System.Diagnostics.Debug.WriteLine("press key Request " + key + " Fields " + message.ToString());
                JObject response = new JObject();
                response["responsetype"] = "presskey";
                response["status"] = "400";

                string keyname = (string)message["key"];

                var bindings = discoveryform.ActionController.FrontierBindings;

                string pname = "elitedangerous64";

                if (bindings.KeyNames.Contains(keyname))       // first check its a valid name..
                {
                    List<Tuple<BindingsFile.Device, BindingsFile.Assignment>> matches
                                = bindings.FindAssignedFunc(keyname, BindingsFile.KeyboardDeviceName);   // just give me keyboard bindings, thats all i can do

                    if (matches != null)      // null if no matches to keyboard is found
                    {
                        Keys[] keys = (from x in matches[0].Item2.keys select x.Key.ToVkey()).ToArray();        // bindings returns keys

                        if (!keys.Contains(Keys.None)) // if no errors
                        {
                            string keyseq = keys.GenerateSequence();
                            System.Diagnostics.Debug.WriteLine("Frontier " + keyname + "->" + keyseq + " to " + pname);
                            string res = BaseUtils.EnhancedSendKeys.SendToProcess(keyseq, 100, 100, 100, pname);
                            response["status"] = res.HasChars() ? "100" : "400";
                        }


                    }
                }
                else 
                {
                    string res = BaseUtils.EnhancedSendKeys.SendToProcess(keyname, 100, 100, 100, pname);
                    response["status"] = res.HasChars() ? "100" : "400";
                }

                return response;
            }
        }
    }
}
