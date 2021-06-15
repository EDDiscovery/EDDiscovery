/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

function OnLoad()
{
    var uri = WSURIFromLocation()
    console.log("WS URI:" + uri);
    websocket = new WebSocket(uri, "EDDJSON");
	websocket.onopen = function (evt) { onOpen(evt) };
	websocket.onclose = function (evt) { onClose(evt) };
	websocket.onmessage = function (evt) { onMessage(evt) };
    websocket.onerror = function (evt) { onError(evt) };
}

function onOpen(evt)
{
    RequestStatus(-1);
    RequestScanData(-1);
}

function onClose(evt)
{
}

var lastscandata;       // keep last scan data

function onMessage(evt)
{
	jdata = JSON.parse(evt.data);

    if (jdata.responsetype == "status")    // we requested a status or status was pushed, update screen
    {
       // console.log("scandata status " + evt.data);
        FillSystemTable(jdata);
    }
    else if (jdata.responsetype == "scandata")    // we requested a status or status was pushed, update screen
    {
        lastscandata = jdata;
        FillScan();
    }
}

function onError(evt)
{
    console.log("Web Error " + evt.data);
}

function FillScan()
{
    var showmaterials = getmenuitemcheckstate("scanmenu", "materials");
    var showvalue = getmenuitemcheckstate("scanmenu", "value");

    //  console.log("scandata stars " + evt.data);
    FillScanTable(lastscandata, showmaterials, showvalue);
}

function scanmenuchange(mouseevent)
{
    var ct = mouseevent.currentTarget;
    console.log("MI " + ct.id + " tag " + ct.tag);
    if (ct.tag != null)
        storestate(ct.tag, ct.checked);
    closemenus();
    FillScan();
}