/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

import { WriteHeader, WriteNav, WriteFooter } from "/header.js"
import { RequestStatus, FillSystemTable } from "/systemtable/systemtable.js"
import { WSURIFromLocation } from "/jslib/websockets.js"
import { ShowPopup, HidePopup } from "/jslib/popups.js"
import { RequestIndicator, HandleIndicatorMessage, InitIndicator } from "/indicators/indicators.js"

var websocket;

function OnLoad()
{
    var header = document.getElementsByTagName("header");
    WriteHeader(header[0]);
    var nav = document.getElementsByTagName("nav");
    WriteNav(nav[0], 4);
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
    InitIndicator(websocket, 64);
    RequestStatus(websocket,-1);
    RequestIndicator(websocket);
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

function onMessage(evt)
{
    console.log("Grid: Web Response " + evt.data);
	var jdata = JSON.parse(evt.data);

    if (jdata.responsetype == "indicator" || jdata.responsetype == "indicatorpush")
    {
        HandleIndicatorMessage(jdata, "Status", "Actions", "StatusOther");
    }
}

