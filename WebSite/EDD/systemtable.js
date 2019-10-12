
// Status requires a StarData Table

function RequestStatus(entryno)
{
    console.log("Request status on " + entryno);
    var msg = {
        requesttype: "status",
        entry: entryno,	// -1 means send me the latest journal entry first, followed by length others.  else its the journal index 
    };

    websocket.send(JSON.stringify(msg));
}

function FillSystemTable(jdata)
{
    console.log("Fill Status ");

    var asidetable = document.getElementById("StarData");

    removeChildren(asidetable);

    var entry = jdata["entry"]

    if (entry >= 0)
    {
        asidetable.appendChild(tablerow2tdjson(jdata, "Cmdr:", "Commander"));
        asidetable.appendChild(tablerow2tdjson(jdata, "Star System:", "SystemData", "System"));

        var edsm = jdata.SystemData.EDSMID > 0 ? CreateAnchor("EDSM", "https://www.edsm.net/system/id/" + jdata.SystemData.EDSMID + "/name/" + jdata.SystemData.System, true) : null;
        var eddb = jdata.EDDB.EDDBID > 0 ? CreateAnchor("EDDB", "https://eddb.io/system/" + jdata.EDDB.EDDBID, true) : null;
        var ross = jdata.EDDB.EDDBID > 0 ? CreateAnchor("ROSS", "https://ross.eddb.io/system/update/" + jdata.EDDB.EDDBID, true) : null;

        asidetable.appendChild(tablerow1tdlist([edsm, eddb, ross], "edsmetcbuttons", 2));

        asidetable.appendChild(tablerow2tdjson(jdata, "Body Name:", "Bodyname"));
        asidetable.appendChild(tablerow2tdstring("Position:", jdata.SystemData.PosX + "," + jdata.SystemData.PosY + "," + jdata.SystemData.PosZ));
        asidetable.appendChild(tablerow2tdstring("To","Sol: " + jdata["SolDist"] + " Home:" + jdata["HomeDist"]));
        asidetable.appendChild(tablerow2tdjson(jdata, "State", "EDDB", "State"));
        asidetable.appendChild(tablerow2tdjson(jdata, "Allegiance", "EDDB", "Allegiance"));
        asidetable.appendChild(tablerow2tdjson(jdata, "Government", "EDDB", "Gov"));
        asidetable.appendChild(tablerow2tdjson(jdata, "Economy", "EDDB", "Economy"));
        asidetable.appendChild(tablerow2tdjson(jdata, "Faction", "EDDB", "Faction"));
        asidetable.appendChild(tablerow2tdjson(jdata, "Security", "EDDB", "Security"));
        asidetable.appendChild(tablerow2tdstring("Fuel", jdata["Ship"]["Fuel"] + " / " + jdata["Ship"]["TankSize"]));
        asidetable.appendChild(tablerow2tdstring("Mats/Data", jdata["Ship"]["Materials"] + " / " + jdata["Ship"]["Data"]));
        asidetable.appendChild(tablerow2tdjson(jdata, "Materials", "Ship", "Materials"));
        asidetable.appendChild(tablerow2tdjson(jdata, "Credits", "Credits"));
        if (jdata.Travel.Dist != "")
        {
            if (jdata.Travel.UnknownJumps != 0)
                asidetable.appendChild(tablerow2tdstring("Travel", jdata.Travel.Dist + " ly, " + jdata.Travel.Time + ", " + jdata.Travel.Jumps + "(+" + jdata.Travel.UnknownJumps + ")"));
            else
                asidetable.appendChild(tablerow2tdstring("Travel", jdata.Travel.Dist + " ly, " + jdata.Travel.Time + ", " + jdata.Travel.Jumps));
        }
        else
            asidetable.appendChild(tablerow2tdstring("Travel", "-"));
    }

}
