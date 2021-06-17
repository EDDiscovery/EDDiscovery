
// Status requires a StarData Table

function RequestStatus(entryno)
{
    console.log("Request status on " + entryno);
    var msg = {
        requesttype: "status",
        entry: entryno,	// -1 means send me the latest journal entry first
    };

    websocket.send(JSON.stringify(msg));
}

function FillSystemTable(jdata)
{
    console.log("Fill Status ");

    var asidetable = document.getElementById("StarData");

    RemoveChildren(asidetable);

    var entry = jdata["entry"]

    if (entry >= 0)
    {
        var s = SplitCapsWord(jdata.Mode);

        asidetable.appendChild(TableRow2tdjson(jdata, "Cmdr:", "Commander"));
        asidetable.appendChild(TableRow2tdstring("Mode", s));
        asidetable.appendChild(TableRow2tdjson(jdata, "Star System:", "SystemData", "System"));

        var list = [CreateAnchor("EDSM", "https://www.edsm.net/system?systemName=" + encodeURI(jdata.SystemData.System), true, "edsmetcbuttons"),
            CreateAnchor("EDDB", "https://eddb.io/system/name/" + encodeURI(jdata.SystemData.System), true, "edsmetcbuttons"),
            CreateAnchor("Inara", "https://inara.cz/galaxy-starsystem/?search=" + encodeURI(jdata.SystemData.System), true, "edsmetcbuttons"),
            CreateAnchor("Spansh", "https://spansh.co.uk/system/" + jdata.SystemData.SystemAddress, true, "edsmetcbuttons")
        ];

        asidetable.appendChild(TableRow2tdtextitem("", list));

        asidetable.appendChild(TableRow2tdstring("Position:", jdata.SystemData.PosX + "," + jdata.SystemData.PosY + "," + jdata.SystemData.PosZ));
        asidetable.appendChild(TableRow2tdjson(jdata, "Visits:", "SystemData", "VisitCount"));

        asidetable.appendChild(TableRow2tdjson(jdata, "Body Name:", "Bodyname"));
        if (jdata.EDDB.MarketID != null)
        {
            var list = [CreateAnchor("EDDB", "https://eddb.io/station/market-id/" + jdata.EDDB.MarketID, true, "edsmetcbuttons"),
                CreateAnchor("Spansh", "https://spansh.co.uk/station/" + jdata.EDDB.MarketID, true, "edsmetcbuttons")];

            asidetable.appendChild(TableRow2tdtextitem("", list));
        }

        asidetable.appendChild(TableRow2tdstring("To","Sol: " + jdata["SolDist"] + " Home:" + jdata["HomeDist"]));
        asidetable.appendChild(TableRow2tdjson(jdata, "State", "EDDB", "State"));
        asidetable.appendChild(TableRow2tdjson(jdata, "Allegiance", "EDDB", "Allegiance"));
        asidetable.appendChild(TableRow2tdjson(jdata, "Government", "EDDB", "Gov"));
        asidetable.appendChild(TableRow2tdjson(jdata, "Economy", "EDDB", "Economy"));
        asidetable.appendChild(TableRow2tdjson(jdata, "Faction", "EDDB", "Faction"));
        asidetable.appendChild(TableRow2tdjson(jdata, "Security", "EDDB", "Security"));
        asidetable.appendChild(TableRow2tdstring("Fuel", jdata["Ship"]["Fuel"] + " / " + jdata["Ship"]["TankSize"]));
        asidetable.appendChild(TableRow2tdstring("Mats/Data", jdata["Ship"]["Materials"] + " / " + jdata["Ship"]["Data"]));
        asidetable.appendChild(TableRow2tdjson(jdata, "Materials", "Ship", "Materials"));
        asidetable.appendChild(TableRow2tdjson(jdata, "Credits", "Credits"));
        if (jdata.Travel.Dist != "")
        {
            if (jdata.Travel.UnknownJumps != 0)
                asidetable.appendChild(TableRow2tdstring("Travel", jdata.Travel.Dist + " ly, " + jdata.Travel.Time + ", " + jdata.Travel.Jumps + "(+" + jdata.Travel.UnknownJumps + ")"));
            else
                asidetable.appendChild(TableRow2tdstring("Travel", jdata.Travel.Dist + " ly, " + jdata.Travel.Time + ", " + jdata.Travel.Jumps));
        }
        else
            asidetable.appendChild(TableRow2tdstring("Travel", "-"));

        asidetable.appendChild(TableRow2tdjson(jdata, "Game Mode:", "GameMode"));
    }

}
