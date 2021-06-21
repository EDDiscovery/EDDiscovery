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


import { FillMissionsTable, RequestMissions } from "/missions/missionrequests.js"
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
    WriteNav(nav[0], 5);

    var div = CreateDiv("menubutton", "menubutton1");

    div.appendChild(CreateImage("/Images/menu.png", "Menu", null, togglemenu, null, null, "menubutton"));

    WriteMenu(div, "missionmenu", "navmenu", [
        ["submenu", "statussize", "Set grid display width..", "submenumissiondisplaysize"],
    ]);

    WriteMenu(document.body, "submenumissiondisplaysize", "navmenu",
        [
            ["radio", "100", "Full Width", missiondisplaysizedisplaychange, "missiondisplaysizegroup", "70"],
            ["radio", "85", "85%", missiondisplaysizedisplaychange, "missiondisplaysizegroup"],
            ["radio", "80", "80%", missiondisplaysizedisplaychange, "missiondisplaysizegroup"],
            ["radio", "75", "75%", missiondisplaysizedisplaychange, "missiondisplaysizegroup"],
            ["radio", "70", "70%", missiondisplaysizedisplaychange, "missiondisplaysizegroup"],
            ["radio", "60", "60%", missiondisplaysizedisplaychange, "missiondisplaysizegroup"],
            ["radio", "50", "50%", missiondisplaysizedisplaychange, "missiondisplaysizegroup"],
        ]);

    nav[0].appendChild(div);

    var footer = document.getElementsByTagName("footer");
    WriteFooter(footer[0],null);

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
    RequestMissions(websocket, -1);
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

    if (jdata.responsetype == "missionslist") 
    {
        FillMissionsTable(jdata);
    } 
    else if (jdata.responsetype == "missionschanged") 
    {
        console.log("Missions changed");
        RequestMissions(websocket, -1);
    }
    else if (jdata.responsetype == "status")    // we requested a status or status was pushed, update screen
    {
     //   console.log("status " + evt.data);
        FillSystemTable(jdata)
    }
}


function togglemenu()
{
    ToggleMenu("missionmenu");
}

function setDisplaySize()
{
    var stardisplaysize = FetchNumber("submenumissiondisplaysize.missiondisplaysizegroup.radiostate", "70");

    var leftside = document.getElementsByClassName("missiontable")[0];
    var rightside = document.getElementsByTagName('aside')[0];
    leftside.style.width = stardisplaysize + "%";
    rightside.style.visibility = stardisplaysize != "100" ? "visible" : "hidden";
    rightside.style.width = (100 - 4 - stardisplaysize) + "%";      // 4 comes from the .aside margin-right
}

function missiondisplaysizedisplaychange(mouseevent)
{
    var ct = mouseevent.currentTarget;
    console.log("Hit journal size" + ct.id + " store " + ct.tag[1] + " into " + ct.tag[0]);
    StoreState(ct.tag[0], ct.tag[1]);
    setDisplaySize();
    CloseMenus();
} 

