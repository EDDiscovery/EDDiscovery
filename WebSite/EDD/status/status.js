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
import { RequestIndicator, HandleIndicatorMessage, InitIndicator  } from "/indicators/indicators.js"

var websocket;

function OnLoad()
{
    var header = document.getElementsByTagName("header");
    WriteHeader(header[0]);
    var nav = document.getElementsByTagName("nav");
    WriteNav(nav[0], 3);
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
    InitIndicator(websocket, 32);
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

