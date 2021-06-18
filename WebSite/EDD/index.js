/*
 * Copyright 2021-2021 Robbyxp1 @ github.com
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


import { RequestJournal, FillJournalTable, ClearJournalTable, RequestMore, JournalScrolled } from "/journal/journal.js"
import { RequestStatus, FillSystemTable } from "/systemtable/systemtable.js"
import { WriteHeader, WriteNav, WriteFooter } from "/header.js"
import { WSURIFromLocation } from "/jslib/websockets.js"
import { ShowPopup } from "/jslib/popups.js"
    
var websocket;

export function OnLoad()
{
    var header = document.getElementsByTagName("header");
    WriteHeader(header[0]);
    var nav = document.getElementsByTagName("nav");
    WriteNav(nav[0],0);
    var footer = document.getElementsByTagName("footer");
    WriteFooter(footer[0],[["+1000",request1000more]]);

    var js = document.getElementById("journalscroll");
    js.onscroll = journalscrolled;

    var uri = WSURIFromLocation()
    console.log("WS URI:" + uri);
    websocket = new WebSocket(uri, "EDDJSON");
	websocket.onopen = function (evt) { onOpen(evt) };
	websocket.onclose = function (evt) { onClose(evt) };
	websocket.onmessage = function (evt) { onMessage(evt) };
    websocket.onerror = function (evt) { onError(evt) };
}

document.body.onload = OnLoad;

function onOpen(evt)
{
    RequestJournal(websocket,-1, 50);
    RequestStatus(websocket,-1);
}

function onClose(evt)
{
    console.log("Closed " + evt.data);
    ShowPopup("lostconnection");
}

function onError(evt)
{
    console.log("Web Error " + evt.data);
    ShowPopup("lostconnection");
}


function onMessage(evt)
{
	var jdata = JSON.parse(evt.data);

    if (jdata.responsetype == "journalrequest") // we requested "journal", records requested back
    {
       FillJournalTable(jdata, false, clickrequest)
    }
    else if (jdata.responsetype == "journalpush")   // EDD sent a new journal record
    {
        FillJournalTable(jdata, true, clickrequest)
    }
    else if (jdata.responsetype == "journalrefresh") // EDD has changed the history, start again
    {
      //  console.log("Journal refresh " + evt.data);
        ClearJournalTable();
        FillJournalTable(jdata, false, clickrequest)
    }
    else if (jdata.responsetype == "status")    // we requested a status or status was pushed, update screen
    {
     //   console.log("status " + evt.data);
        FillSystemTable(jdata)
    }
}

function request1000more()
{
    RequestMore(websocket,1000);
}

function clickrequest(e)
{
    console.log("Clicked" + e.target.tag);
    RequestStatus(websocket, e.target.tag);
}

function journalscrolled(e)      // called by article on scrolling
{
    var journalscroll = e.currentTarget;
    JournalScrolled(websocket, journalscroll);

}
