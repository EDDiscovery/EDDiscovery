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

var lastobjectlist = null;     // containing responsetype and objectlist[] (left,right,top,bottom,text)

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
    else if (jdata.responsetype == "scandisplayobjects")    // scan display changed, rerequest URL
    {
       // console.log("system objects" + evt.data);
        lastobjectlist = jdata;
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

    lastobjectlist = null;      // indicate don't have a list now

    var img = CreateImage(req, "Scan display", null, imageclick);
    img.id = "scandisplayimage";

    jimgdiv.appendChild(img);       // this causes a get to the server, which sends back an image and the object list
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

function imageclick(mouseevent)
{
    var ct = mouseevent.currentTarget;
    console.log("Click image" + ct.id + " " + mouseevent.clientX + " " + mouseevent.clientY + " " + mouseevent.offsetX + " " + mouseevent.offsetY);

    if (lastobjectlist != null)
    {
        var olist = lastobjectlist.objectlist;
        olist.forEach(function (x)
        {
            if (mouseevent.offsetX >= x.left && mouseevent.offsetX <= x.right && mouseevent.offsetY >= x.top && mouseevent.offsetY <= x.bottom)
            {
                var jimgdiv = document.getElementById("scanbmp");
                var neartop = mouseevent.offsetY < 3*jimgdiv.clientHeight / 4;
              //  console.log("Object " + x.left + " " + x.top + " " + x.text);
                var size = fetchnumber("submenusize.sizegroup.radiostate", "48");
                ShowPopup("scanobjectnotification", CreatePara(x.text), null, null);
            }
        });

    }
}
