/*
 * Copyright © 2019-2023 EDDiscovery development team
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

using BaseUtils.WebServer;
using EliteDangerousCore;
using EliteDangerousCore.UIEvents;
using System;

// how to debug
// -wsf c:\code\eddiscovery\WebSite\EDD  to point EDD at the website
// open the SLN in the website in another VS instance
// use ISS Express (Google Chrome) to run the website - you will be able to single step etc. In EDD Debug Properties, the Start URL is localhost:6502
// until the js file is needed, a debug point will show not bound.  thats okay.
// use the console inspector control-shift-I to debug also in chrome

namespace EDDiscovery.WebServer
{
    // JSON Websockets Interface definition:
    // Query is from web client
    // Push is from EDD spontaneous
    //
    //-------------------------------------------------------------------------------------------------------------------
    // Query requesttype=journal : fields start, length
    //          responsetype = journalrequest, fields   : firstrow = -1 none, or first row number
    //                                                  : Commander
    //                                                  : rows[] containing journaliconpath, eventtimeutc, summary, info, note
    // Push responsetype = journalrefresh, fields as per journalrequest
    // Push responsetype = journalpush, fields as per journalrequest, insert at front, new rows
    //-------------------------------------------------------------------------------------------------------------------
    // Query requesttype=status : fields entry number
    //          responsetype = status, fields           : entry = -1 none, or entry number
    //                                                  : SystemData object containing records
    //                                                  : EDDB object
    //                                                  : Ship object
    //                                                  : Travel object
    //                                                  : Bodyname, HomeDist, SolDist, GameMode, Credits, Commander, Mode
    // Push responsetype = status, fields as above
    //-------------------------------------------------------------------------------------------------------------------
    // Query requesttype=indicator. No fields
    //          responsetype = indicator, fields        : Various status fields taken fro UIOverallStatus
    //
    // Push responsetype = indicatorpush, fields as above
    //-------------------------------------------------------------------------------------------------------------------
    // Query requesttype = presskey, fields              : key = binding name optional keydelay, shiftdelay, updelay
    //          responsetype = status, 100 or 400.
    //-------------------------------------------------------------------------------------------------------------------
    // Query requesttype = texts, fields entry, length - from this history entry, report this number of send or receive text's
    //          responsetype = texts, fields            : entry = as per above
    //                                                  : length = number of reports
    //                                                  : entries = array of entries. Each entry is an array itself, of [time,"Send Text", tofield, message]
    //                                                                                or [time,channel,from localised, message localised]
    // Push responsetype="textchanged" when history has been refreshed
    //-------------------------------------------------------------------------------------------------------------------
    // Query requesttype = missions, fields entry - report information on missions at this entry, or -1 for latest mission list
    //          responsetype = missionslist, fields     : entry = reported entry number
    //                                                  : length = number of reports
    //                                                  : current = array of current missions, each one an array containing mission data
    //                                                  : previous = array of previous missions, each one an array containing mission data
    // Push responsetype="missionschanged" when history has been refreshed
    //-------------------------------------------------------------------------------------------------------------------
    // Query requesttype=scandata : fields entry number, "edsm" true/false, "spansh" true/false (EDD 17.0)
    //          responsetype = entry, objectlist..          : entry = -1 none, or entry number. See code for fields
    //
    // Push responsetype=scandatachanged                : indicate scan data has changed
    //-------------------------------------------------------------------------------------------------------------------
    // Push responsetype = genericui, fields are:       (From EDD 17.0 Nov 12 2023)
    //      data.EventTimeUTC,
    //      data.EventTypeID 
    // EventTypeID=FSDTarget, property data.FSDTarget containing properties StarSystem, StarClass, EDStarClass, SystemAddress, RemainingJumpsInRoute, FriendlStarClass
    // EventTypeID=Music, properties Track, MusicTrackID
    // EventTypeID=UnderAttack, properties Target
    // EventTypeID=ShipTargeted, property data.ShipTargeted containing properties TargetLocked=false always (Normal ShipTargeted comes thru the journal interface)
    // EventTypeID=NavRouteClear, property data.NavRouteClear, no properties.
    // Ones containing data."EventID" contain other game data: CommanderID, EventTimeLocal, IsBeta, IsOdyssey, GameVersion, Build.
    //
    //-------------------------------------------------------------------------------------------------------------------
    // .png load of image from /systemmap/ folder - image name is not important, encoded query field /systemmap/map.png?entry=n&EDSM=true&SPANSH=true  (SPANSH from 17.0)
    //
    // Push responsetype=systemmapchanged               : indicate scan system map has changed

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
        MissionRequest missions;
        TextsRequest texts;

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
            scandisplay = new EDDScanDisplay(discoveryform, httpws);
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
            jsondispatch.Add("scandata", scandata);   // scan data 

            presskey = new PressKeyRequest(discoveryform);
            jsondispatch.Add("presskey", presskey);   // and a key press

            missions = new MissionRequest(discoveryform);
            jsondispatch.Add("missions", missions);   // mission list

            texts = new TextsRequest(discoveryform);
            jsondispatch.Add("texts", texts);   // text messages

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

        private void Discoveryform_OnHistoryChange()
        {
            httpws.SendWebSockets(journalsender.Refresh(-1, 50), false); // refresh history
            httpws.SendWebSockets(statussender.Refresh(-1), false); // and status
            httpws.SendWebSockets(scandata.Notify(), false); // tell it its changed
            httpws.SendWebSockets(scandisplay.Notify(), false); // tell it its changed
            httpws.SendWebSockets(missions.Notify(), false); // tell it its changed
            httpws.SendWebSockets(texts.Notify(), false); // tell it its changed
        }

        private void Discoveryform_OnNewUIEvent(UIEvent obj)
        {
            if (obj.EventTypeID == UITypeEnum.OverallStatus)
            {
                httpws.SendWebSockets(indicator.Refresh(obj as UIOverallStatus), false); // push indicator push
            }
            else if ( obj.EventTypeID == UITypeEnum.FSDTarget || obj.EventTypeID == UITypeEnum.Music || obj.EventTypeID == UITypeEnum.UnderAttack || obj.EventTypeID == UITypeEnum.ShipTargeted ||
                        obj.EventTypeID == UITypeEnum.NavRouteClear )
            {
                var uipush = new GenericUIPush();
                httpws.SendWebSockets(uipush.Refresh(obj), false); // push json of event 
            }
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he)
        {
            httpws.SendWebSockets(journalsender.Push(), false); // refresh history
            httpws.SendWebSockets(statussender.Push(), false); // refresh status

            if (he.EntryType == JournalTypeEnum.ReceiveText || he.EntryType == JournalTypeEnum.SendText)
                httpws.SendWebSockets(texts.Push(), false);

            if (he.EntryType == JournalTypeEnum.Scan || he.EntryType == JournalTypeEnum.FSSSignalDiscovered || he.EntryType == JournalTypeEnum.SAASignalsFound ||
                        he.EntryType == JournalTypeEnum.SAAScanComplete)
            {
                httpws.SendWebSockets(scandata.Notify(), false); // refresh status
                httpws.SendWebSockets(scandisplay.Notify(), false); // tell it its changed
            }

            if ( he.journalEntry is IMissions )
            {
                httpws.SendWebSockets(missions.Notify(), false); // tell it its changed
            }
        }
    }
}
