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
import { CreateDiv, CreateImage } from "/jslib/elements.js"
import { FetchNumber, StoreState } from "/jslib/localstorage.js"
import { WriteMenu, ToggleMenu, GetMenuItemCheckState, CloseMenus } from "/jslib/menus.js"

    
var websocket;

export function OnLoad()
{
    var header = document.getElementsByTagName("header");
    WriteHeader(header[0]);
    var nav = document.getElementsByTagName("nav");
    WriteNav(nav[0], 0);

    var div = CreateDiv("menubutton", "menubutton1");

    div.appendChild(CreateImage("/Images/menu.png", "Menu", null, togglemenu, null, null, "menubutton"));

    WriteMenu(div, "journalmenu", "navmenu", [
        ["submenu", "statussize", "Set grid display width..", "submenujournaldisplaysize"],
    ]);

    WriteMenu(document.body, "submenujournaldisplaysize", "navmenu",
        [
            ["radio", "100", "Full Width", journaldisplaysizedisplaychange, "journaldisplaysizegroup", "70"],
            ["radio", "85", "85%", journaldisplaysizedisplaychange, "journaldisplaysizegroup"],
            ["radio", "80", "80%", journaldisplaysizedisplaychange, "journaldisplaysizegroup"],
            ["radio", "75", "75%", journaldisplaysizedisplaychange, "journaldisplaysizegroup"],
            ["radio", "70", "70%", journaldisplaysizedisplaychange, "journaldisplaysizegroup"],
            ["radio", "60", "60%", journaldisplaysizedisplaychange, "journaldisplaysizegroup"],
            ["radio", "50", "50%", journaldisplaysizedisplaychange, "journaldisplaysizegroup"],
        ]);

    nav[0].appendChild(div);

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

    setDisplaySize();
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

function togglemenu()
{
    ToggleMenu("journalmenu");
}

function setDisplaySize()
{
    var stardisplaysize = FetchNumber("submenujournaldisplaysize.journaldisplaysizegroup.radiostate", "70");

    var leftside = document.getElementsByClassName("journaltable")[0];
    var rightside = document.getElementsByTagName('aside')[0];
    leftside.style.width = stardisplaysize + "%";
    rightside.style.visibility = stardisplaysize != "100" ? "visible" : "hidden";
    rightside.style.width = (100 - 4 - stardisplaysize) + "%";      // 4 comes from the .aside margin-right
}

function journaldisplaysizedisplaychange(mouseevent)
{
    var ct = mouseevent.currentTarget;
    console.log("Hit journal size" + ct.id + " store " + ct.tag[1] + " into " + ct.tag[0]);
    StoreState(ct.tag[0], ct.tag[1]);
    setDisplaySize();
    CloseMenus();
} 

