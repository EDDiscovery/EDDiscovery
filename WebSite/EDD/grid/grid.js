/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

function OnLoad()
{
    var uri = WSURIFromLocation();
    console.log("WS URI:" + uri);
    websocket = new WebSocket(uri, "EDDJSON");

    websocket.onopen = function (evt) { onOpen(evt) };
	websocket.onclose = function (evt) { onClose(evt) };
	websocket.onmessage = function (evt) { onMessage(evt) };
    websocket.onerror = function (evt) { onError(evt) };

    indicatoriconsize = 64;
    highlight_nav_tab(2);

}

function onOpen(evt)
{
    currentshiptype = "";
    RequestStatus(-1);
    RequestIndicator();
}

function onClose(evt)
{
}

function onMessage(evt)
{
    console.log("Web Response " + evt.data);
	jdata = JSON.parse(evt.data);

    if (jdata.responsetype == "indicator" || jdata.responsetype == "indicatorpush")
    {
        HandleIndicatorMessage(jdata, "Status", "Actions", null);
    }
}

function onError(evt)
{
    console.log("Web Error " + evt.data);
}

