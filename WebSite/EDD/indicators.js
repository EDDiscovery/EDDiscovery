
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

function CreateIndicator(itype, enableit = true)
{
    if (enableit)
        return CreateImage("statusicons/" + itype + ".png", itype, indicatoriconsize, null, [itype, null]);
    else
        return null;
}

function CreateAction(name, bindingname = null, enableit = true, flashit = 0, confirmit = false)
{
    if (enableit)
    {
        if (bindingname == null)
            bindingname = name;

        //console.log("create action image name:" + itype + " a:" + action + " ");
        return CreateImage("statusicons/" + name + ".png", name, indicatoriconsize, ClickActionItem, [name, bindingname, flashit, confirmit]);
    }
    else
        return null;
}

function CreateActionButton(name, bindingname = null, enableit = true)
{
    return CreateAction(name, bindingname, enableit, 250);
}

var shiptypeselected;
var inwingselected;
var supercruiseselected;
var landedselected;
var dockedselected;

// names of elements
function HandleIndicatorMessage(jdata, statuselement, actionelement, statusotherelement)
{
    var guifocus = jdata["GUIFocus"];

    if (guifocus == "GalaxyMap")                    // translate the GUI focus to appropriate true tags for the set indicator code
        jdata["GalaxyMapOpen"] = true;
    if (guifocus == "SystemMap")
        jdata["SystemMapOpen"] = true;

    console.log("Indicators Mod" + JSON.stringify(jdata));

    var shiptype = jdata["ShipType"];

    var inwing = jdata["InWing"] != null && jdata["InWing"] == true;
    var supercruise = jdata["Supercruise"] != null && jdata["Supercruise"] == true;
    var landed = jdata["Landed"] != null && jdata["Landed"] == true;
    var docked = jdata["Docked"] != null && jdata["Docked"] == true;

    if (shiptype != shiptypeselected || inwing != inwingselected || supercruise != supercruiseselected || landed != landedselected || docked != dockedselected)
    {
        shiptypeselected = shiptype;       // SRV, MainShip, Fighter or None.
        inwingselected = inwing;
        supercruiseselected = supercruise;
        SetupIndicators(jdata, document.getElementById(statuselement), document.getElementById(actionelement),
            shiptype, inwing, supercruise, landed,docked);
    }

    SetIndicatorState(jdata, document.getElementById(statuselement));
    SetIndicatorState(jdata, document.getElementById(actionelement));

    if (statusotherelement != null)
    {
        var tstatusother = document.getElementById(statusotherelement);

        removeChildren(tstatusother);

        if (jdata["LegalState"] != null)
            tstatusother.appendChild(CreatePara("Legal State: " + jdata["LegalState"]));
        if (jdata["Firegroup"] >= 0 && shiptype == "MainShip")
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

function SetupIndicators(jdata,tstatus,tactions, shiptype, inwing, supercruise,landed, docked)
{
    removeChildren(tstatus);
    removeChildren(tactions);

    var innormalspace = !landed && !docked && !supercruise;
    console.log("Create Indicators with L:" + landed + " D:" + docked + " N:" + innormalspace + " W:" + inwing);
    if (shiptype == "MainShip")
    {
        var statuslist = [
            CreateIndicator("Docked"), CreateIndicator("Landed"), CreateIndicator("ShieldsUp"),
            CreateIndicator("InWing"), CreateIndicator("ScoopingFuel", supercruise), 
            CreateIndicator("LowFuel"), CreateIndicator("OverHeating"), CreateIndicator("IsInDanger", !docked),
            CreateIndicator("BeingInterdicted", supercruise), CreateIndicator("FsdCharging",!landed && !docked),
            CreateIndicator("FsdMassLocked", innormalspace),
            CreateIndicator("FsdCooldown", supercruise || innormalspace)
        ];

        tstatus.appendChild(tablerowmultitdlist(statuslist));

        var actionlist = [
            CreateAction("LandingGear", "LandingGearToggle",innormalspace),     // reported..
            CreateAction("Lights", "ShipSpotLightToggle"),
            CreateAction("FlightAssist", "ToggleFlightAssist", innormalspace),
            CreateAction("HardpointsDeployed", "DeployHardpointToggle"),

            CreateAction("CargoScoopDeployed", "ToggleCargoScoop", innormalspace),
            CreateAction("NightVision", "NightVisionToggle", innormalspace),

            CreateAction("UseBoostJuice", null, innormalspace, 1500),    
            CreateAction("ShieldCell", "UseShieldCell", innormalspace,1000),
            CreateAction("Chaff", "FireChaffLauncher", innormalspace,1000),
            CreateAction("HeatSink", "DeployHeatSink", innormalspace,1000),
            CreateAction("ChargeECM", null, innormalspace, 1500),

            CreateAction("Supercruise", null, !landed),  // reported
            CreateActionButton("HyperSuperCombination", null, !landed), // not reported
            CreateActionButton("OrbitLinesToggle"),

            CreateActionButton("CyclePreviousTarget"),
            CreateActionButton("CycleNextTarget"),
            CreateActionButton("SelectHighestThreat", null, innormalspace),
            CreateActionButton("CyclePreviousHostileTarget", null, innormalspace),
            CreateActionButton("CycleNextHostileTarget", null, innormalspace),

            CreateActionButton("CyclePreviousSubsystem", null, innormalspace),
            CreateActionButton("CycleNextSubsystem", null, innormalspace),

            CreateActionButton("TargetWingman0", null, inwing),
            CreateActionButton("TargetWingman1", null, inwing),
            CreateActionButton("TargetWingman2", null, inwing),
            CreateActionButton("SelectTargetsTarget", null, inwing),
            CreateActionButton("WingNavLock", null, inwing),

            CreateActionButton("TargetNextRouteSystem",null,supercruise ),

            CreateActionButton("CycleFireGroupPrevious"),
            CreateActionButton("CycleFireGroupNext"),

            CreateActionButton("IncreaseSystemsPower"),
            CreateActionButton("IncreaseEnginesPower"),
            CreateActionButton("IncreaseWeaponsPower"),
            CreateActionButton("ResetPowerDistribution"),

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
            CreateActionButton("Screenshot", "F10"),

            CreateAction("SilentRunning", "ToggleButtonUpInput", innormalspace,0,true),
        ];

        tactions.appendChild(tablerowmultitdlist(actionlist))
    }
    else if (shiptype == "SRV")
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
            CreateActionButton( "Screenshot", "F10"),
        ];

        tstatus.appendChild(tablerowmultitdlist(statuslist));
        tactions.appendChild(tablerowmultitdlist(actionlist));
    }
    else if (shiptype == "Fighter")
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
            CreateActionButton( "Screenshot", "F10"),
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
            y.childNodes.forEach(function (z)   // imgs
            {
                //console.log("Entry is " + z.nodeName + " " + z.tag);

                var indicator = z.tag[0];

                if (indicator != null )      // presuming this works if z.tag is not defined.
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

