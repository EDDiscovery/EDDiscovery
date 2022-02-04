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
import { CreateImage, CreatePara, CreateDiv } from "/jslib/elements.js"
import { WriteMenu, ToggleMenu, GetMenuItemCheckState, CloseMenus } from "/jslib/menus.js"
import { WSURIFromLocation } from "/jslib/websockets.js"
import { FetchState, StoreState, FetchNumber } from "/jslib/localstorage.js"
import { ShowPopup, HidePopup } from "/jslib/popups.js"

var websocket;

function OnLoad()
{
    var header = document.getElementsByTagName("header");
    WriteHeader(header[0]);
    var nav = document.getElementsByTagName("nav");
    WriteNav(nav[0], 1);

    var div = CreateDiv("menubutton", "menubutton1");

    div.appendChild(CreateImage("/Images/menu.png", "Menu", null, togglemenu, null, null, "menubutton"));

    WriteMenu(div, "scandisplaymenu", "navmenu",
        [
            ["checkbox", "moon", "Show Moons", scandisplaychange, true],
            ["checkbox", "bodyicons", "Show Body Icons", scandisplaychange, true],
            ["checkbox", "materials", "Show Materials", scandisplaychange, true],
            ["checkbox", "gvalue", "Show G on all planets", scandisplaychange, true],
            ["checkbox", "habzone", "Show Habzones", scandisplaychange, true],
            ["checkbox", "starclass", "Show classes of stars", scandisplaychange, true],
            ["checkbox", "planetclass", "Show classes of planets", scandisplaychange, true],
            ["checkbox", "distance", "Show distance of bodies", scandisplaychange, true],
            ["checkbox", "edsm", "Check EDSM", scandisplaychange, false],
            ["button", "value", "Set Valuable Limit", setvaluelimit, false],
            ["submenu", "size", "Set body image size..", "submenusize"],
            ["submenu", "statussize", "Set star display width..", "submenustardisplaysize"],
        ]);

    nav[0].appendChild(div);

    /* attach to mainbody, not div, because we need page absolute positioning */

    WriteMenu(document.body, "submenusize", "navmenu",
        [
            ["radio", "16", "16", sizedisplaychange, "sizegroup", "48"],
            ["radio", "32", "32", sizedisplaychange, "sizegroup"],
            ["radio", "48", "48", sizedisplaychange, "sizegroup"],
            ["radio", "64", "64", sizedisplaychange, "sizegroup"],
            ["radio", "96", "96", sizedisplaychange, "sizegroup"],
            ["radio", "128", "128", sizedisplaychange, "sizegroup"],
            ["radio", "160", "160", sizedisplaychange, "sizegroup"],
        ]);

    WriteMenu(document.body, "submenustardisplaysize", "navmenu",
        [
            ["radio", "100", "Full Width", stardisplaysizedisplaychange, "stardisplaysizegroup", "70"],
            ["radio", "85", "85%", stardisplaysizedisplaychange, "stardisplaysizegroup"],
            ["radio", "80", "80%", stardisplaysizedisplaychange, "stardisplaysizegroup"],
            ["radio", "75", "75%", stardisplaysizedisplaychange, "stardisplaysizegroup"],
            ["radio", "70", "70%", stardisplaysizedisplaychange, "stardisplaysizegroup"],
            ["radio", "60", "60%", stardisplaysizedisplaychange, "stardisplaysizegroup"],
            ["radio", "50", "50%", stardisplaysizedisplaychange, "stardisplaysizegroup"],
        ]);

    var footer = document.getElementsByTagName("footer");
    WriteFooter(footer[0], null);

    var uri = WSURIFromLocation()
    console.log("WS URI:" + uri);
    websocket = new WebSocket(uri, "EDDJSON");
	websocket.onopen = function (evt) { onOpen(evt) };
	websocket.onclose = function (evt) { onClose(evt) };
	websocket.onmessage = function (evt) { onMessage(evt) };
    websocket.onerror = function (evt) { onError(evt) };

    setDisplaySize();

    document.getElementById("scanimagearea").onclick = clickonscanbackground;           // a click on this is the same as the img, so handle it
    document.getElementsByTagName('aside')[0].onclick = cancelmenupopup;                // a click here cancels the menu/popup

    document.getElementById("valuedialog_cancel").onclick = cancelvalue;
    document.getElementById("valuedialog_ok").onclick = setvalue;

}

document.body.onload = OnLoad;

function onOpen(evt)
{
    RequestStatus(websocket, -1);
    RequestImage(-1);
}

function onClose(evt)
{
    ShowPopup("lostconnection");
}

function onError(evt)
{
    console.log("Web Error " + evt.data);
    ShowPopup("lostconnection");
}

var lastobjectlist = null;     // containing responsetype and objectlist[] (left,right,top,bottom,text)

function onMessage(evt)
{
	var jdata = JSON.parse(evt.data);

    if (jdata.responsetype == "status")    // we requested a status or status was pushed, update screen
    {
        console.log("scandata status changed");
        FillSystemTable(jdata);
    }
    else if (jdata.responsetype == "systemmapchanged")    // scan display changed, rerequest URL
    {
        console.log("Informed scan display changed");
        RequestImage(-1);
    }
    else if (jdata.responsetype == "scandisplayobjects")    // scan display changed, rerequest URL
    {
        console.log("New scandisplay objects received");
        lastobjectlist = jdata;
    }
}

function RequestImage(entry)
{
    var jimgdiv = document.getElementById("scanbmp");

    var showmoon = GetMenuItemCheckState("scandisplaymenu", "moon");
    var bodyicons = GetMenuItemCheckState("scandisplaymenu", "bodyicons");
    var showmaterials = GetMenuItemCheckState("scandisplaymenu", "materials");
    var gvalue = GetMenuItemCheckState("scandisplaymenu", "gvalue");
    var habzone = GetMenuItemCheckState("scandisplaymenu", "habzone");
    var starclass = GetMenuItemCheckState("scandisplaymenu", "starclass");
    var planetclass = GetMenuItemCheckState("scandisplaymenu", "planetclass");
    var distance = GetMenuItemCheckState("scandisplaymenu", "distance");
    var edsm = GetMenuItemCheckState("scandisplaymenu", "edsm");

    var width = jimgdiv.clientWidth;

    var size = FetchState("submenusize.sizegroup.radiostate", "48");

    var valuelimit = FetchState("scandisplay_valuelimit", 50001);
    console.log("Value limit is " + valuelimit);

    var req = "/systemmap/image.png?entry=" + entry + "&width=" + width + "&starsize=" + size + "&showmoons=" + showmoon + "&showbodyicons=" + bodyicons +
        "&showmaterials=" + showmaterials + "&showgravity=" + gvalue + "&showhabzone=" + habzone + "&showstarclass=" + starclass + "&showplanetclass=" + planetclass +
        "&showdistance=" + distance + "&EDSM=" + edsm + "&valuelimit=" + valuelimit + "&reqtime=" + new Date().getTime();

    lastobjectlist = null;      // indicate don't have a list now
    var img = jimgdiv.childNodes[0];
    console.log("Reload image source" + req);
    img.src = req;
}

function scandisplaychange(mouseevent)
{
    var ct = mouseevent.currentTarget;
    console.log("MI " + ct.id + " tag " + ct.tag);
    if (ct.tag != null)
        StoreState(ct.tag, ct.checked);
    CloseMenus();
    RequestImage(-1);
}

function sizedisplaychange(mouseevent)
{
    var ct = mouseevent.currentTarget;
    console.log("Hit size" + ct.id + " store " + ct.tag[1] + " into " + ct.tag[0]);
    StoreState(ct.tag[0], ct.tag[1]);
    CloseMenus();
    RequestImage(-1);
}

function stardisplaysizedisplaychange(mouseevent)
{
    var ct = mouseevent.currentTarget;
    console.log("Hit Star size" + ct.id + " store " + ct.tag[1] + " into " + ct.tag[0]);
    StoreState(ct.tag[0], ct.tag[1]);
    setDisplaySize();
    CloseMenus();
    RequestImage(-1);
}

function setvaluelimit(mouseevent)
{
    CloseMenus();
    var inputbox = document.getElementById("valuedialog_value");
    inputbox.value = FetchState("scandisplay_valuelimit", 50001);
    ShowPopup("valuedialog",null,null,null,false);
}

function cancelvalue()
{
    HidePopup("valuedialog",false);
}

function setvalue()
{
    var inputbox = document.getElementById("valuedialog_value");
    StoreState("scandisplay_valuelimit", inputbox.value);
    HidePopup("valuedialog",false);
    RequestImage(-1);
}


function togglemenu()
{
    HidePopup("scanobjectnotification");
    ToggleMenu("scandisplaymenu");
}


function clickonscanbackground(mouseevent)      // since image is on scanimage div its the same click offset
{
    CloseMenus();

    var ct = mouseevent.currentTarget;
    console.log("Click image" + ct.id + " " + mouseevent.clientX + " " + mouseevent.clientY + " " + mouseevent.offsetX + " " + mouseevent.offsetY);

    if (lastobjectlist != null)
    {
        var olist = lastobjectlist.objectlist;
        var len = olist.length;

        for (var i = 0; i < olist.length; i++)      //can't use a foreach, as you can't break them
        {
            var x = olist[i];

            if (mouseevent.offsetX >= x.left && mouseevent.offsetX <= x.right && mouseevent.offsetY >= x.top && mouseevent.offsetY <= x.bottom)
            {
                var jimgdiv = document.getElementById("scanbmp");
                var neartop = mouseevent.offsetY < 3 * jimgdiv.clientHeight / 4;
                //  console.log("Object " + x.left + " " + x.top + " " + x.text);
                ShowPopup("scanobjectnotification", CreatePara(x.text), null, null);
                return;
            }
        }
    }

    HidePopup("scanobjectnotification");
}

function cancelmenupopup()
{
    CloseMenus();
    HidePopup("scanobjectnotification");
}

function setDisplaySize()
{
    var stardisplaysize = FetchNumber("submenustardisplaysize.stardisplaysizegroup.radiostate", "70");

    var leftside = document.getElementsByClassName("scanimage")[0];
    var rightside = document.getElementsByTagName('aside')[0];
    leftside.style.width = stardisplaysize + "%";
    rightside.style.visibility = stardisplaysize != "100" ? "visible" : "hidden";
    rightside.style.width = (100 - 4 - stardisplaysize) + "%";      // 4 comes from the .aside margin-right
}
