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
    ShowPopup("lostconnection");
}

function onError(evt)
{
    console.log("Web Error " + evt.data);
    ShowPopup("lostconnection");
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
        console.log("New scandata received");
        lastscandata = jdata;
        FillScan();
    }
    else if (jdata.responsetype == "scandatachanged")    // system notified scan data changed
    {
        console.log("scandata informed changed");
        RequestScanData(-1);
    }
}

function FillScan()
{
    var showmaterials = GetMenuItemCheckState("scanmenu", "materials");
    var showvalue = GetMenuItemCheckState("scanmenu", "value");

    //  console.log("scandata stars " + evt.data);
    FillScanTable(lastscandata, showmaterials, showvalue);
}

function scanmenuchange(mouseevent)
{
    var ct = mouseevent.currentTarget;
    console.log("MI " + ct.id + " tag " + ct.tag);
    if (ct.tag != null)
        StoreState(ct.tag, ct.checked);
    CloseMenus();
    FillScan();
}