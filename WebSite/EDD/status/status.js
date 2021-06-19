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


import { WriteHeader, WriteNav, WriteFooter } from "/header.js"
import { RequestStatus, FillSystemTable } from "/systemtable/systemtable.js"
import { WSURIFromLocation } from "/jslib/websockets.js"
import { ShowPopup, HidePopup } from "/jslib/popups.js"
import { WriteMenu, ToggleMenu, CloseMenus } from "/jslib/menus.js"
import { FetchNumber, StoreState } from "/jslib/localstorage.js"
import { CreateImage, CreatePara, CreateDiv } from "/jslib/elements.js"
import { RequestIndicator, HandleIndicatorMessage, InitIndicator  } from "/indicators/indicators.js"

var websocket;

function OnLoad()
{
    var header = document.getElementsByTagName("header");
    WriteHeader(header[0]);
    var nav = document.getElementsByTagName("nav");
    WriteNav(nav[0], 3);

    var div = CreateDiv("menubutton", "menubutton1");

    div.appendChild(CreateImage("/Images/menu.png", "Menu", null, togglemenu, null, null, "menubutton"));

    WriteMenu(div, "statusmenu", "navmenu",
        [
            ["submenu", "size", "Set Size", "statussizemenu"],
        ]);

    nav[0].appendChild(div);

    /* attach to mainbody, not div, because we need page absolute positioning */

    WriteMenu(document.body, "statussizemenu", "navmenu",
        [
            ["radio", "16", "16", sizedisplaychange, "sizegroup", "48"],
            ["radio", "32", "32", sizedisplaychange, "sizegroup"],
            ["radio", "48", "48", sizedisplaychange, "sizegroup"],
            ["radio", "64", "64", sizedisplaychange, "sizegroup"],
            ["radio", "96", "96", sizedisplaychange, "sizegroup"],
            ["radio", "128", "128", sizedisplaychange, "sizegroup"],
            ["radio", "160", "160", sizedisplaychange, "sizegroup"],
        ]);

    var footer = document.getElementsByTagName("footer");
    WriteFooter(footer[0], null);

    var uri = WSURIFromLocation();
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
    var size = FetchNumber("statussizemenu.sizegroup.radiostate", 32, true);
    InitIndicator(websocket, size);
    RequestStatus(websocket,-1);
    RequestIndicator();
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
    console.log("Web Response " + evt.data);
	var jdata = JSON.parse(evt.data);

    if (jdata.responsetype == "indicator" || jdata.responsetype == "indicatorpush")
    {
        HandleIndicatorMessage(jdata,"Status","Actions","StatusOther");
    }
    else if (jdata.responsetype == "status" || jdata.responsetype == "statuspush" )
    {
        FillSystemTable(jdata);
    }
}

function togglemenu()
{
    ToggleMenu("statusmenu");
}

function sizedisplaychange(mouseevent)
{
    var ct = mouseevent.currentTarget;
    console.log("Hit size" + ct.id + " store " + ct.tag[1] + " into " + ct.tag[0]);
    StoreState(ct.tag[0], ct.tag[1]);
    CloseMenus();
    var size = FetchNumber("statussizemenu.sizegroup.radiostate", 64, true);
    InitIndicator(websocket, size);
    RequestIndicator(websocket);
}



