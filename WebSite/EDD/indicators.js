
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

function CreateIndicator(itype)
{
    return CreateImage("statusicons/" + itype + ".png", itype, indicatoriconsize, null, [itype, null]);
}

function CreateAction(enableit, itype, action = null)
{
    if (enableit)
    {
        if (action == null)
            action = itype;

        //console.log("create action image name:" + itype + " a:" + action + " ");
        return CreateImage("statusicons/" + itype + ".png", itype, indicatoriconsize, ClickActionItem, [itype, action]);
    }
    else
        return null;
}

var shiptypeselected;
var inwingselected;
var supercruiseselected;

// names of elements
function HandleIndicatorMessage(jdata, statuselement, actionelement, statusotherelement, slicestatus)
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

    if (shiptype != shiptypeselected || inwing != inwingselected || supercruise != supercruiseselected)
    {
        shiptypeselected = shiptype;       // SRV, MainShip, Fighter or None.
        inwingselected = inwing;
        supercruiseselected = supercruise;
        SetupIndicators(jdata, document.getElementById(statuselement), document.getElementById(actionelement), slicestatus);
    }

    SetIndicatorState(jdata, statuselement);
    SetIndicatorState(jdata, actionelement);

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

function SetupIndicators(jdata,tstatus,tactions, slicestatus)
{
    var shiptype = jdata["ShipType"];

    removeChildren(tstatus);
    removeChildren(tactions);

    var inwing = jdata["InWing"] != null && jdata["InWing"] == true;
    var supercruise = jdata["Supercruise"] != null && jdata["Supercruise"] == true;

    if (shiptype == "MainShip")
    {
        var statuslist = [
            CreateIndicator("Docked"), CreateIndicator("Landed"),
            CreateIndicator("InWing"), CreateIndicator("ScoopingFuel"), CreateIndicator("ShieldsUp"),
            CreateIndicator("LowFuel"), CreateIndicator("OverHeating"), CreateIndicator("IsInDanger"),
            CreateIndicator("BeingInterdicted"), CreateIndicator("FsdMassLocked"), CreateIndicator("FsdCharging"),
            CreateIndicator("FsdCooldown")
        ];

        if (slicestatus)
        {
            tstatus.appendChild(tablerowmultitdlist(statuslist.slice(0, 4)));
            tstatus.appendChild(tablerowmultitdlist(statuslist.slice(4, 8)));
            tstatus.appendChild(tablerowmultitdlist(statuslist.slice(8, 12)));
            tstatus.appendChild(tablerowmultitdlist(statuslist.slice(12, 13)));
        }
        else
            tstatus.appendChild(tablerowmultitdlist(statuslist));

        var actionlist = [
            CreateAction(true, "LandingGear", "LandingGearToggle"),
            CreateAction(true, "Lights", "ShipSpotLightToggle"),
            CreateAction(true, "FlightAssist", "ToggleFlightAssist"),
            CreateAction(true, "HardpointsDeployed", "DeployHardpointToggle"),

            CreateAction(true, "CargoScoopDeployed", "ToggleCargoScoop"),
            CreateAction(!supercruise, "SilentRunning", "ToggleButtonUpInput"),
            CreateAction(true, "NightVision", "NightVisionToggle"),
            CreateAction(!supercruise, "UseBoostJuice"),

            CreateAction(!supercruise, "ShieldCell", "UseShieldCell"),
            CreateAction(!supercruise, "Chaff", "FireChaffLauncher"),
            CreateAction(!supercruise, "HeatSink", "DeployHeatSink"),
            CreateAction(!supercruise, "ChargeECM"),

            CreateAction(true, "Supercruise"),
            CreateAction(true, "HyperSuperCombination"),
            CreateAction(true, "OrbitLinesToggle"),

            CreateAction(true, "CycleNextTarget"),
            CreateAction(true, "CyclePreviousTarget"),
            CreateAction(true, "SelectHighestThreat"),
            CreateAction(true, "CycleNextHostileTarget"),
            CreateAction(true, "CyclePreviousHostileTarget"),

            CreateAction(true, "CycleNextSubsystem"),
            CreateAction(true, "CyclePreviousSubsystem"),

            CreateAction(inwing, "TargetWingman0"),
            CreateAction(inwing, "TargetWingman1"),
            CreateAction(inwing, "TargetWingman2"),
            CreateAction(inwing, "SelectTargetsTarget"),
            CreateAction(inwing, "WingNavLock"),

            CreateAction(supercruise, "TargetNextRouteSystem"),

            CreateAction(true, "CycleFireGroupNext"),
            CreateAction(true, "CycleFireGroupPrevious"),

            CreateAction(true, "GalaxyMapOpen"),
            CreateAction(true, "SystemMapOpen"),
            CreateAction(true, "IncreaseSystemsPower"),
            CreateAction(true, "IncreaseEnginesPower"),
            CreateAction(true, "IncreaseWeaponsPower"),
            CreateAction(true, "ResetPowerDistribution"),

            CreateAction(!supercruise, "OrderDefensiveBehaviour"),
            CreateAction(!supercruise, "OrderAggressiveBehaviour"),
            CreateAction(!supercruise, "OrderFocusTarget"),
            CreateAction(!supercruise, "OrderHoldFire"),
            CreateAction(!supercruise, "OrderHoldPosition"),
            CreateAction(!supercruise, "OrderFollow"),
            CreateAction(!supercruise, "OrderRequestDock"),
            CreateAction(!supercruise, "OpenOrders"),

            CreateAction(true, "Screenshot", "F10"),

        ];

        tactions.appendChild(tablerowmultitdlist(actionlist))
    }
    else if (shiptype == "SRV")
    {
        var statuslist = [
            CreateIndicator("SrvUnderShip"), CreateIndicator("LowFuel"), CreateIndicator("ShieldsUp")
        ];

        var actionlist = [
            CreateAction(true, "SrvHandbrake", "AutoBreakBuggyButton"),
            CreateAction(true, "SrvTurret", "ToggleBuggyTurretButton"),
            CreateAction(true, "SrvDriveAssist", "ToggleDriveAssist"),
            CreateAction(true, "Lights", "HeadlightsBuggyButton"),
            CreateAction(true, "RecallDismissShip", "F10"),

            CreateAction(true, "GalaxyMapOpen"),
            CreateAction(true, "SystemMapOpen"),
            CreateAction(true, "IncreaseSystemsPower"),
            CreateAction(true, "IncreaseEnginesPower"),
            CreateAction(true, "IncreaseWeaponsPower"),
            CreateAction(true, "ResetPowerDistribution"),

            CreateAction(true, "Screenshot", "F10"),
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
            CreateAction(true, "Lights", "ShipSpotLightToggle"),
            CreateAction(true, "FlightAssist", "ToggleFlightAssist"),
            CreateAction(true, "NightVision", "NightVisionToggle"),
            CreateAction(true, "GalaxyMapOpen"),
            CreateAction(true, "SystemMapOpen"),
            CreateAction(true, "IncreaseSystemsPower"),
            CreateAction(true, "IncreaseEnginesPower"),
            CreateAction(true, "IncreaseWeaponsPower"),
            CreateAction(true, "ResetPowerDistribution"),

            CreateAction(true, "OrderDefensiveBehaviour"),
            CreateAction(true, "OrderAggressiveBehaviour"),
            CreateAction(true, "OrderFocusTarget"),
            CreateAction(true, "OrderHoldFire"),
            CreateAction(true, "OrderHoldPosition"),
            CreateAction(true, "OrderFollow"),
            CreateAction(true, "OrderRequestDock"),
            CreateAction(true, "OpenOrders"),

            CreateAction(true, "Screenshot", "F10"),
        ];

        tstatus.appendChild(tablerowmultitdlist(statuslist));
        tactions.appendChild(tablerowmultitdlist(actionlist));
    }
    else
    {       // no type, no data
    }
}

function SetIndicatorState(jdata, element)
{
    //const keys = Object.keys(jdata);
    //if (keys.indexOf(z.tag) != -1)
    //{
    //    console.log(".. " + z.tag + " value is " + jdata[z.tag]);
    //}

    var tstatus = document.getElementById(element);
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
    console.log("Press key " + e.target.tag[1]);
    var msg = {
        requesttype: "presskey",
        key: e.target.tag[1],
    };

    websocket.send(JSON.stringify(msg));
}

