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


import { RequestTexts, FillTextsTable } from "/texts/textsrequests.js"
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
    WriteNav(nav[0], 6);

    var div = CreateDiv("menubutton", "menubutton1");

    div.appendChild(CreateImage("/Images/menu.png", "Menu", null, togglemenu, null, null, "menubutton"));

    WriteMenu(div, "textsmenu", "navmenu", [
        ["submenu", "statussize", "Set grid display width..", "submenutextsdisplaysize"],
        ["checkbox", "showinfo", "Show system info messages", textsmenuchange, true],
        ["checkbox", "shownfz", "Show no fire zone messages", textsmenuchange, true],
     //   ["checkbox", "showec", "Show entering channel messages", textsmenuchange, true],

    ]);

    WriteMenu(document.body, "submenutextsdisplaysize", "navmenu",
        [
            ["radio", "100", "Full Width", textsdisplaysizedisplaychange, "textsdisplaysizegroup", "70"],
            ["radio", "85", "85%", textsdisplaysizedisplaychange, "textsdisplaysizegroup"],
            ["radio", "80", "80%", textsdisplaysizedisplaychange, "textsdisplaysizegroup"],
            ["radio", "75", "75%", textsdisplaysizedisplaychange, "textsdisplaysizegroup"],
            ["radio", "70", "70%", textsdisplaysizedisplaychange, "textsdisplaysizegroup"],
            ["radio", "60", "60%", textsdisplaysizedisplaychange, "textsdisplaysizegroup"],
            ["radio", "50", "50%", textsdisplaysizedisplaychange, "textsdisplaysizegroup"],
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
    RequestTexts(websocket,-1,200);
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

    if (jdata.responsetype == "textschanged") 
    {
        console.log("Texts changed");
        RequestTexts(websocket,-1,200);
    }
    else if (jdata.responsetype == "texts")
    {
        FillTable(jdata, false)
    }
    else if (jdata.responsetype == "textspush") 
    {
        FillTable(jdata, true);
    }
    else if (jdata.responsetype == "status")    // we requested a status or status was pushed, update screen
    {
     //   console.log("status " + evt.data);
        FillSystemTable(jdata)
    }
}

function FillTable(jdata, insert)
{
    var showinfo = GetMenuItemCheckState("textsmenu", "showinfo");
    var shownfz = GetMenuItemCheckState("textsmenu", "shownfz");
 //   var showec = GetMenuItemCheckState("textsmenu", "showec");
    FillTextsTable(jdata, insert, showinfo, shownfz);
}

function togglemenu()
{
    ToggleMenu("textsmenu");
}

function setDisplaySize()
{
    var stardisplaysize = FetchNumber("submenutextsdisplaysize.textsdisplaysizegroup.radiostate", "70");

    var leftside = document.getElementsByClassName("textstable")[0];
    var rightside = document.getElementsByTagName('aside')[0];
    leftside.style.width = stardisplaysize + "%";
    rightside.style.visibility = stardisplaysize != "100" ? "visible" : "hidden";
    rightside.style.width = (100 - 4 - stardisplaysize) + "%";      // 4 comes from the .aside margin-right
}

function textsdisplaysizedisplaychange(mouseevent)
{
    var ct = mouseevent.currentTarget;
    console.log("Hit journal size" + ct.id + " store " + ct.tag[1] + " into " + ct.tag[0]);
    StoreState(ct.tag[0], ct.tag[1]);
    setDisplaySize();
    CloseMenus();
} 

function textsmenuchange(mouseevent)
{
    var ct = mouseevent.currentTarget;
    console.log("MI " + ct.id + " tag " + ct.tag);
    if (ct.tag != null)
        StoreState(ct.tag, ct.checked);
    CloseMenus();
    RequestTexts(websocket, -1, 200);
}

