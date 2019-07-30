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

    highlight_nav_tab(0);
}

function onOpen(evt)
{
    RequestJournal(-1, 50);
    RequestStatus(-1);
}

function onClose(evt)
{
}

function onMessage(evt)
{
	jdata = JSON.parse(evt.data);

    if (jdata.responsetype == "journalrequest") // we requested "journal", records requested back
    {
        FillJournalTable(jdata, false)
    }
    else if (jdata.responsetype == "journalpush")   // EDD sent a new journal record
    {
        FillJournalTable(jdata, true)
    }
    else if (jdata.responsetype == "journalrefresh") // EDD has changed the history, start again
    {
        console.log("Journal refresh " + evt.data);
        ClearJournalTable();
        FillJournalTable(jdata, false)
    }
    else if (jdata.responsetype == "status")    // we requested a status or status was pushed, update screen
    {
        console.log("status " + evt.data);
        FillSystemTable(jdata)
    }
}

function onError(evt)
{
    console.log("Web Error " + evt.data);
}

function footerbutton1click()
{
    RequestMore(1000);
}

