
// Indicators

var indicatoriconsize = 32;     //crappy but effective - set icon size globally

function RequestIndicator()
{
    console.log("Request indicators");
    var msg = {
        requesttype: "indicator",
    };

    websocket.send(JSON.stringify(msg));
}

function CreateIndicator(itype, enableit = true, tooltip = null)
{
    if (enableit)
    {
        if ( tooltip == null )
            tooltip = itype.replace(/([a-z](?=[A-Z]))/g, '$1 ');

        return CreateImage("/statusicons/" + itype + ".png", itype, indicatoriconsize, null, [itype, null], tooltip);
    } 
    else
        return null;
}

function CreateAction(name, bindingname = null, enableit = true, flashit = 0, confirmit = false, tooltip = null)
{
    if (enableit)
    {
        if (bindingname == null)
            bindingname = name;

        if ( tooltip == null )
            tooltip = bindingname.replace(/([a-z](?=[A-Z]))/g, '$1 ');

        //console.log("create action image name:" + itype + " a:" + action + " ");
        return CreateImage("/statusicons/" + name + ".png", name, indicatoriconsize, ClickActionItem, [name, bindingname, flashit, confirmit], tooltip);
    }
    else
        return null;
}

function CreateActionButton(name, bindingname = null, enableit = true, tooltip = null)
{
    return CreateAction(name, bindingname, enableit, 250, null, tooltip);
}

var currentshiptype;
var currentinwing;
var currentsupercruise;
var currentlanded;
var currentdocked;

// names of elements
function HandleIndicatorMessage(jdata, statuselement, actionelement, statusotherelement)
{
    var guifocus = jdata["GUIFocus"];

    if (guifocus == "GalaxyMap")                    // translate the GUI focus to appropriate true tags for the set indicator code
        jdata["GalaxyMapOpen"] = true;
    if (guifocus == "SystemMap")
        jdata["SystemMapOpen"] = true;

    console.log("Indicators Mod" + JSON.stringify(jdata));

    var newshiptype = jdata["ShipType"];
    var newinwing = jdata["InWing"] != null && jdata["InWing"] == true;
    var newsupercruise = jdata["Supercruise"] != null && jdata["Supercruise"] == true;
    var newlanded = jdata["Landed"] != null && jdata["Landed"] == true;
    var newdocked = jdata["Docked"] != null && jdata["Docked"] == true;

    if (newshiptype != currentshiptype || newinwing != currentinwing || newsupercruise != currentsupercruise || newlanded != currentlanded || newdocked != currentdocked)
    {
        currentshiptype = newshiptype;       // SRV, MainShip, Fighter or None.
        currentinwing = newinwing;
        currentsupercruise = newsupercruise;
        currentlanded = newlanded;
        currentdocked = newdocked;
        SetupIndicators(jdata, document.getElementById(statuselement), document.getElementById(actionelement));
    }

    SetIndicatorState(jdata, document.getElementById(statuselement));
    SetIndicatorState(jdata, document.getElementById(actionelement));

    if (statusotherelement != null)
    {
        var tstatusother = document.getElementById(statusotherelement);

        removeChildren(tstatusother);

        if (jdata["LegalState"] != null)
            tstatusother.appendChild(CreatePara("Legal State: " + jdata["LegalState"]));
        if (jdata["Firegroup"] >= 0 && newshiptype == "MainShip")
            tstatusother.appendChild(CreatePara("Fire Group: " + "ABCDEFGHIJK"[jdata["Firegroup"]]));
        if (jdata["ValidPips"])
        {
            tstatusother.appendChild(CreatePara("Pips: " + "S:" + jdata["Pips"][0] + " E:" + jdata["Pips"][1] + " W:" + jdata["Pips"][2]));
        }
        if (jdata["ValidPosition"])
        {
            tstatusother.appendChild(CreatePara("Pos: " + jdata["Position"][0] + ", " + jdata["Position"][1]));
            if (jdata["ValidAltitude"])
            {
                var alt = jdata["Position"][2];
                if (alt > 5000)
                    tstatusother.appendChild(CreatePara("Alt: " + (alt / 1000.0) + "km"));
                else
                    tstatusother.appendChild(CreatePara("Alt: " + alt + "m"));
            }

            if (jdata["ValidHeading"])
                tstatusother.appendChild(CreatePara("Hdr: " + jdata["Position"][3]));
        }

        if (jdata["ValidPlanetRadius"])
            tstatusother.appendChild(CreatePara("Radius: " + jdata["PlanetRadius"] / 1000.0 + "km"));
    }

}

function SetupIndicators(jdata,tstatus,tactions)
{
    removeChildren(tstatus);
    removeChildren(tactions);

    var innormalspace = !currentlanded && !currentdocked && !currentsupercruise;
    var notdockedlanded = !currentdocked && !currentlanded;

    console.log("Create Indicators with L:" + currentlanded + " D:" + currentdocked + " N:" + innormalspace + " W:" + currentinwing + " S:" + currentsupercruise);

    if (currentshiptype == "MainShip")
    {
        var statuslist = [
            CreateIndicator("Docked"), CreateIndicator("Landed"), CreateIndicator("ShieldsUp"),
            CreateIndicator("InWing"), CreateIndicator("ScoopingFuel", currentsupercruise), 
            CreateIndicator("LowFuel"), CreateIndicator("OverHeating"), CreateIndicator("IsInDanger", !currentdocked, "In Danger"),
            CreateIndicator("BeingInterdicted", currentsupercruise), CreateIndicator("FsdCharging", notdockedlanded),
            CreateIndicator("FsdMassLocked", innormalspace),
            CreateIndicator("FsdCooldown", currentsupercruise || innormalspace)
        ];

        tstatus.appendChild(tablerowmultitdlist(statuslist));

        var actionlist = [
            CreateAction("LandingGear", "LandingGearToggle",innormalspace),     // reported..
            CreateAction("Lights", "ShipSpotLightToggle"),
            CreateAction("FlightAssist", "ToggleFlightAssist", innormalspace),
            CreateAction("HardpointsDeployed", "DeployHardpointToggle", notdockedlanded),

            CreateAction("CargoScoopDeployed", "ToggleCargoScoop", innormalspace),
            CreateAction("NightVision", "NightVisionToggle", innormalspace),

            CreateAction("UseBoostJuice", null, innormalspace, 1500),    
            CreateAction("ShieldCell", "UseShieldCell", innormalspace,1000),
            CreateAction("Chaff", "FireChaffLauncher", innormalspace,1000),
            CreateAction("HeatSink", "DeployHeatSink", innormalspace,1000),
            CreateAction("ChargeECM", null, innormalspace, 1500),

            CreateAction("Supercruise", null, notdockedlanded),  // reported
            CreateActionButton("HyperSuperCombination", null, notdockedlanded), // not reported
            CreateActionButton("OrbitLinesToggle"),

            CreateActionButton("CyclePreviousTarget"),
            CreateActionButton("CycleNextTarget"),
            CreateActionButton("SelectHighestThreat", null, innormalspace),
            CreateActionButton("CyclePreviousHostileTarget", null, innormalspace),
            CreateActionButton("CycleNextHostileTarget", null, innormalspace),

            CreateActionButton("CyclePreviousSubsystem", null, innormalspace),
            CreateActionButton("CycleNextSubsystem", null, innormalspace),

            CreateActionButton("TargetWingman0", null, currentinwing),
            CreateActionButton("TargetWingman1", null, currentinwing),
            CreateActionButton("TargetWingman2", null, currentinwing),
            CreateActionButton("SelectTargetsTarget", null, currentinwing),
            CreateActionButton("WingNavLock", null, currentinwing),

            CreateActionButton("TargetNextRouteSystem",null,currentsupercruise ),

            CreateActionButton("CycleFireGroupPrevious"),
            CreateActionButton("CycleFireGroupNext"),

            CreateActionButton("IncreaseSystemsPower", null, !currentdocked),
            CreateActionButton("IncreaseEnginesPower", null, !currentdocked),
            CreateActionButton("IncreaseWeaponsPower", null, !currentdocked),
            CreateActionButton("ResetPowerDistribution", null, !currentdocked),

            CreateActionButton("OrderDefensiveBehaviour", null, innormalspace),
            CreateActionButton("OrderAggressiveBehaviour", null, innormalspace),
            CreateActionButton("OrderFocusTarget", null, innormalspace),
            CreateActionButton("OrderHoldFire", null, innormalspace),
            CreateActionButton("OrderHoldPosition", null, innormalspace),
            CreateActionButton("OrderFollow", null, innormalspace),
            CreateActionButton("OrderRequestDock", null, innormalspace),
            CreateActionButton("OpenOrders", null, innormalspace),

            CreateAction("GalaxyMapOpen"),
            CreateAction("SystemMapOpen"),
            CreateActionButton("Screenshot", "F10", true, "Screen Shot"),

            CreateAction("SilentRunning", "ToggleButtonUpInput", innormalspace,0,true, "Silent Running"),
        ];

        tactions.appendChild(tablerowmultitdlist(actionlist))
    }
    else if (currentshiptype == "SRV")
    {
        var statuslist = [
            CreateIndicator("SrvUnderShip"), CreateIndicator("LowFuel"), CreateIndicator("ShieldsUp")
        ];

        var actionlist = [
            CreateAction( "SrvHandbrake", "AutoBreakBuggyButton"),
            CreateAction( "SrvTurret", "ToggleBuggyTurretButton"),
            CreateAction( "SrvDriveAssist", "ToggleDriveAssist"),
            CreateAction( "Lights", "HeadlightsBuggyButton"),

            CreateActionButton("RecallDismissShip", "F10"),

            CreateActionButton( "IncreaseSystemsPower"),
            CreateActionButton( "IncreaseEnginesPower"),
            CreateActionButton( "IncreaseWeaponsPower"),
            CreateActionButton( "ResetPowerDistribution"),

            CreateAction("GalaxyMapOpen"),
            CreateAction("SystemMapOpen"),
            CreateActionButton("Screenshot", "F10", true, "Screen Shot"),
        ];

        tstatus.appendChild(tablerowmultitdlist(statuslist));
        tactions.appendChild(tablerowmultitdlist(actionlist));
    }
    else if (currentshiptype == "Fighter")
    {
        var statuslist = [
            CreateIndicator("ShieldsUp")
        ]

        var actionlist = [
            CreateAction( "Lights", "ShipSpotLightToggle"),
            CreateAction( "FlightAssist", "ToggleFlightAssist"),
            CreateAction( "NightVision", "NightVisionToggle"),
            CreateActionButton( "IncreaseSystemsPower"),
            CreateActionButton( "IncreaseEnginesPower"),
            CreateActionButton( "IncreaseWeaponsPower"),
            CreateActionButton( "ResetPowerDistribution"),

            CreateActionButton( "OrderDefensiveBehaviour"),
            CreateActionButton( "OrderAggressiveBehaviour"),
            CreateActionButton( "OrderFocusTarget"),
            CreateActionButton( "OrderHoldFire"),
            CreateActionButton( "OrderHoldPosition"),
            CreateActionButton( "OrderFollow"),
            CreateActionButton( "OrderRequestDock"),
            CreateActionButton( "OpenOrders"),

            CreateAction("GalaxyMapOpen"),
            CreateAction("SystemMapOpen"),
            CreateActionButton("Screenshot", "F10", true, "Screen Shot"),
        ];

        tstatus.appendChild(tablerowmultitdlist(statuslist));
        tactions.appendChild(tablerowmultitdlist(actionlist));
    }
    else
    {
        tstatus.appendChild(CreatePara("Elite not running"));
    }
}

function SetIndicatorState(jdata, tstatus)
{
    //const keys = Object.keys(jdata);
    //if (keys.indexOf(z.tag) != -1)
    //{
    //    console.log(".. " + z.tag + " value is " + jdata[z.tag]);
    //}

    tstatus.childNodes.forEach(function (x)     // tr's
    {
        x.childNodes.forEach(function (y)       // td's
        {
            y.childNodes.forEach(function (y1)       // Div due to toolbar
            {
                y1.childNodes.forEach(function (z)   // imgs and spans
                {
                    if (z.nodeName == "IMG" && z.tag != null)
                    {
                        console.log("Entry is " + z.nodeName + " " + z.tag);

                        var indicator = z.tag[0];

                        if (indicator != null)      // presuming this works if z.tag is not defined.
                        {
                            //console.log("..1 " + z.tag + " value is " + jdata[z.tag]);

                            if (jdata[indicator] != null && jdata[indicator] == true)
                            {
                                z.classList.add("entryselected");       // using a class means it does not mess up all the other properties.
                            }
                            else
                            {
                                z.classList.remove("entryselected");
                            }
                        }
                    }
                });
            });
        });
    });

}

function ClickActionItem(e)
{
    if (e.target.tag[3])
    {
        if (!confirm("Confirm " + e.target.tag[0]))
            return;
    } 

    console.log("Press key " + e.target.tag[1]);
    var msg = {
        requesttype: "presskey",
        key: e.target.tag[1],
    };

    websocket.send(JSON.stringify(msg));

    if (e.target.tag[2]>0)
    {
        e.target.classList.add("entryselected");       // using a class means it does not mess up all the other properties.
        console.log("Flash it! " + e.target.tag[2]);
        setTimeout(function (f)
        {
            console.log("Flash off!")
            e.target.classList.remove("entryselected");       // using a class means it does not mess up all the other properties.
        }, e.target.tag[2]);
    }
}

