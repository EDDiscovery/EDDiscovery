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

function onError(evt)
{
    console.log("Web Error " + evt.data);
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

    var req = "/systemmap/image.png?entry=" + entry + "&width=" + width + "&starsize=" + size + "&showmoons=" + showmoon + "&showbodyicons=" + bodyicons +
        "&showmaterials=" + showmaterials + "&showgravity=" + gvalue + "&showhabzone=" + habzone + "&showstarclass=" + starclass + "&showplanetclass=" + planetclass +
        "&showdistance=" + distance + "&EDSM=" + edsm;

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

function imageclick(mouseevent)
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
                var jimgdiv = document.getElementById("scanbmp");
                var neartop = mouseevent.offsetY < 3*jimgdiv.clientHeight / 4;
              //  console.log("Object " + x.left + " " + x.top + " " + x.text);
                var size = FetchNumber("submenusize.sizegroup.radiostate", "48");
                ShowPopup("scanobjectnotification", CreatePara(x.text), null, null);
                return;
            }
        }
    }

    HidePopup("scanobjectnotification");
}
