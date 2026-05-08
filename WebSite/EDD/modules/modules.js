/*
 * Copyright 2026-2026 Robbyxp1 @ github.com
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
import { CreateImage, CreatePara, CreateDiv } from "/jslib/elements.js"
import { WSURIFromLocation } from "/jslib/websockets.js"
import { ShowPopup, HidePopup } from "/jslib/popups.js"
import { Debounce } from "/jslib/debounce.js"
import { SetupTheme, GetThemeColor } from "/theme.js"

var websocket;

export function OnLoad()
{
    SetupTheme();

    var header = document.getElementsByTagName("header");
    WriteHeader(header[0]);
    var nav = document.getElementsByTagName("nav");
    WriteNav(nav[0], 6);

    var footer = document.getElementsByTagName("footer");
    WriteFooter(footer[0],null);

    var uri = WSURIFromLocation()
    console.log("WS URI:" + uri);
    websocket = new WebSocket(uri, "EDDJSON");
	websocket.onopen = function (evt) { onOpen(evt) };
	websocket.onclose = function (evt) { onClose(evt) };
	websocket.onmessage = function (evt) { onMessage(evt) };
    websocket.onerror = function (evt) { onError(evt) };

    // a click on this is the same as the img, so handle it
    document.getElementById("moduleimagearea").onclick = clickonscanbackground;     

    // on window resize, we debounce it, and 500ms later ask for a new image at the right size
    window.onresize = Debounce(()=> {console.log("Resized Modules");RequestImage();},500);
}

document.body.onload = OnLoad;

function onOpen(evt)
{
    RequestImage();
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

    if (jdata.responsetype == "moduleschanged") 
    {
        RequestImage();
    } 
    else if (jdata.responsetype == "moduledisplayobjects")    // scan display changed, rerequest URL
    {
        console.log("New moduledisplay objects received");
        lastobjectlist = jdata;
    }
}

function RequestImage()
{
    var jimgdiv = document.getElementById("modulebmp");
    var width = jimgdiv.clientWidth;
    var color = GetThemeColor("--textcolor");

    console.log("Width for module display is " + width);

    lastobjectlist = null;      // indicate don't have a list now

    var req = "/modulemap/image.png?textcolor=" + encodeURIComponent(color) + "&width=" + width;
    var img = jimgdiv.childNodes[0];
    console.log("Reload image source" + req);
    img.src = req;
}

var lastobjectlist = null;     // containing responsetype and objectlist[] (left,right,top,bottom,text)

function clickonscanbackground(mouseevent)      
{
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
                var jimgdiv = document.getElementById("modulebmp");
                //  console.log("Object " + x.left + " " + x.top + " " + x.text);
                ShowPopup("moduleobjectnotification", CreatePara(x.text), null, null);
                return;
            }
        }
    }

    HidePopup("moduleobjectnotification");
}