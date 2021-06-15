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
    RequestImage(-1);
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
    else if (jdata.responsetype == "systemmapchanged")    // scan display changed, rerequest URL
    {
        console.log("system map changed " + evt.data);
        RequestImage(-1);
    }
}

function onError(evt)
{
    console.log("Web Error " + evt.data);
}

function RequestImage(entry)
{
    var jimgdiv = document.getElementById("scanbmp");
    removeChildren(jimgdiv);

    var showmoon = getmenuitemcheckstate("scandisplaymenu", "moon");
    var bodyicons = getmenuitemcheckstate("scandisplaymenu", "bodyicons");
    var showmaterials = getmenuitemcheckstate("scandisplaymenu", "materials");
    var gvalue = getmenuitemcheckstate("scandisplaymenu", "gvalue");
    var habzone = getmenuitemcheckstate("scandisplaymenu", "habzone");
    var starclass = getmenuitemcheckstate("scandisplaymenu", "starclass");
    var planetclass = getmenuitemcheckstate("scandisplaymenu", "planetclass");
    var distance = getmenuitemcheckstate("scandisplaymenu", "distance");
    var edsm = getmenuitemcheckstate("scandisplaymenu", "edsm");

    var width = jimgdiv.clientWidth;

    var size = fetchstate("submenusize.sizegroup.radiostate", "48");

    var req = "/systemmap/image.png?entry=" + entry + "&width=" + width + "&starsize=" + size + "&showmoons=" + showmoon + "&showbodyicons=" + bodyicons +
        "&showmaterials=" + showmaterials + "&showgravity=" + gvalue + "&showhabzone=" + habzone + "&showstarclass=" + starclass + "&showplanetclass=" + planetclass +
        "&showdistance=" + distance + "&EDSM=" + edsm;

    console.log("Request " + req);

    var img = CreateImage(req, "Scan display");
    //var img = CreateImage("/Images/EdLogo600.png", "EDLogo", 1200);
    jimgdiv.appendChild(img);
}

function scandisplaychange(mouseevent)
{
    var ct = mouseevent.currentTarget;
    console.log("MI " + ct.id + " tag " + ct.tag);
    if (ct.tag != null)
        storestate(ct.tag, ct.checked);
    closemenus();
    RequestImage(-1);
}

function sizedisplaychange(mouseevent)
{
    var ct = mouseevent.currentTarget;
    console.log("Hit size" + ct.id + " store " + ct.tag[1] + " into " + ct.tag[0]);
    storestate(ct.tag[0], ct.tag[1]);
    closemenus();
    RequestImage(-1);
}

