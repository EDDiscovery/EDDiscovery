/*
 * Copyright 2021-2021 Robbyxp1 @ github.com
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */

// Indicators

import { CreateImage, RemoveChildren, CreatePara } from "/jslib/elements.js"
import { TableRowMultitdlist } from "/jslib/tables.js"

var indicatoriconsize = 32;     //crappy but effective - set icon size statically

export function RequestIndicator()
{
    console.log("Request indicators");
    var msg = {
        requesttype: "indicator",
    };

    websocket.send(JSON.stringify(msg));
}

function createIndicator(itype, enableit = true, tooltip = null)
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

function createAction(name, bindingname = null, enableit = true, flashit = 0, confirmit = false, tooltip = null)
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

function createActionButton(name, bindingname = null, enableit = true, tooltip = null)
{
    return createAction(name, bindingname, enableit, 250, null, tooltip);
}

var currentshiptype;
var currentinwing;
var currentsupercruise;
var currentlanded;
var currentdocked;

var websocket;

export function InitIndicator(websock, size)
{
    currentshiptype = "";
    websocket = websock;
    indicatoriconsize = size;
}

// names of elements
export function HandleIndicatorMessage(jdata, statuselement, actionelement, statusotherelement)
{
    var guifocus = jdata["GUIFocus"];

    if (guifocus == "GalaxyMap")                    // translate the GUI focus to appropriate true tags for the set indicator code
        jdata["GalaxyMapOpen"] = true;
    if (guifocus == "SystemMap")
        jdata["SystemMapOpen"] = true;

    console.log("Indicators Mod" + JSON.stringify(jdata));

    var newshiptype = jdata["ShipType"];                        // really its major mode from odyssey now, backwards compatible naming
    var newinwing = jdata["InWing"] != null && jdata["InWing"] == true;
    var newsupercruise = jdata["Supercruise"] != null && jdata["Supercruise"] == true;
    var newlanded = jdata["Landed"] != null && jdata["Landed"] == true;
    var newdocked = jdata["Docked"] != null && jdata["Docked"] == true;

    if (newshiptype != currentshiptype || newinwing != currentinwing || newsupercruise != currentsupercruise || newlanded != currentlanded || newdocked != currentdocked)
    {
        currentshiptype = newshiptype;       // SRV, MainShip, Fighter etc
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

        RemoveChildren(tstatusother);

        if (jdata["BodyName"] != null && jdata["BodyName"] != "")
            tstatusother.appendChild(CreatePara("Body Name: " + jdata["BodyName"]));

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
        if (jdata["Gravity"] > 0)
            tstatusother.appendChild(CreatePara("Gravity: " + jdata["Gravity"] + "g"));
        if (jdata["Temperature"] > 0)
            tstatusother.appendChild(CreatePara("Temperature: " + jdata["Temperature"] + "K"));
        if (jdata["SelectedWeapon"] != null && jdata["SelectedWeapon"] != "$humanoid_fists" )
            tstatusother.appendChild(CreatePara("Weapon/Tool: " + jdata["SelectedWeaponLocalised"]));
        if (jdata["LegalState"] != null)
            tstatusother.appendChild(CreatePara("Legal State: " + jdata["LegalState"]));
    }

}

export function SetupIndicators(jdata,tstatus,tactions)
{
    RemoveChildren(tstatus);
    RemoveChildren(tactions);

    var innormalspace = !currentlanded && !currentdocked && !currentsupercruise;
    var notdockedlanded = !currentdocked && !currentlanded;

    console.log("Create Indicators with L:" + currentlanded + " D:" + currentdocked + " N:" + innormalspace + " W:" + currentinwing + " S:" + currentsupercruise);

    if (currentshiptype == "MainShip")
    {
        var statuslist = [
            createIndicator("Docked"), createIndicator("Landed"), createIndicator("ShieldsUp"),
            createIndicator("InWing"), createIndicator("ScoopingFuel", currentsupercruise), 
            createIndicator("LowFuel"), createIndicator("OverHeating"), createIndicator("IsInDanger", !currentdocked, "In Danger"),
            createIndicator("BeingInterdicted", currentsupercruise), createIndicator("FsdCharging", notdockedlanded),
            createIndicator("FsdMassLocked", innormalspace),
            createIndicator("FsdCooldown", currentsupercruise || innormalspace)
        ];

        tstatus.appendChild(TableRowMultitdlist(statuslist));

        var actionlist = [
            createAction("LandingGear", "LandingGearToggle",innormalspace),     // reported..
            createAction("Lights", "ShipSpotLightToggle"),
            createAction("FlightAssist", "ToggleFlightAssist", innormalspace),
            createAction("HardpointsDeployed", "DeployHardpointToggle", notdockedlanded),

            createAction("CargoScoopDeployed", "ToggleCargoScoop", innormalspace),
            createAction("NightVision", "NightVisionToggle", innormalspace),

            createAction("UseBoostJuice", null, innormalspace, 1500),    
            createAction("ShieldCell", "UseShieldCell", innormalspace,1000),
            createAction("Chaff", "FireChaffLauncher", innormalspace,1000),
            createAction("HeatSink", "DeployHeatSink", innormalspace,1000),
            createAction("ChargeECM", null, innormalspace, 1500),

            createAction("Supercruise", null, notdockedlanded),  // reported
            createActionButton("HyperSuperCombination", null, notdockedlanded), // not reported
            createActionButton("OrbitLinesToggle"),

            createActionButton("CyclePreviousTarget"),
            createActionButton("CycleNextTarget"),
            createActionButton("SelectHighestThreat", null, innormalspace),
            createActionButton("CyclePreviousHostileTarget", null, innormalspace),
            createActionButton("CycleNextHostileTarget", null, innormalspace),

            createActionButton("CyclePreviousSubsystem", null, innormalspace),
            createActionButton("CycleNextSubsystem", null, innormalspace),

            createActionButton("TargetWingman0", null, currentinwing),
            createActionButton("TargetWingman1", null, currentinwing),
            createActionButton("TargetWingman2", null, currentinwing),
            createActionButton("SelectTargetsTarget", null, currentinwing),
            createActionButton("WingNavLock", null, currentinwing),

            createActionButton("TargetNextRouteSystem",null,currentsupercruise ),

            createActionButton("CycleFireGroupPrevious"),
            createActionButton("CycleFireGroupNext"),

            createActionButton("IncreaseSystemsPower", null, !currentdocked),
            createActionButton("IncreaseEnginesPower", null, !currentdocked),
            createActionButton("IncreaseWeaponsPower", null, !currentdocked),
            createActionButton("ResetPowerDistribution", null, !currentdocked),

            createActionButton("OrderDefensiveBehaviour", null, innormalspace),
            createActionButton("OrderAggressiveBehaviour", null, innormalspace),
            createActionButton("OrderFocusTarget", null, innormalspace),
            createActionButton("OrderHoldFire", null, innormalspace),
            createActionButton("OrderHoldPosition", null, innormalspace),
            createActionButton("OrderFollow", null, innormalspace),
            createActionButton("OrderRequestDock", null, innormalspace),
            createActionButton("OpenOrders", null, innormalspace),

            createAction("GalaxyMapOpen"),
            createAction("SystemMapOpen"),
            createActionButton("1", "FocusCommsPanel"),
            createActionButton("1", "QuickCommsPanel"),
            createActionButton("Screenshot", "F10", true, "Screen Shot"),

            createAction("SilentRunning", "ToggleButtonUpInput", innormalspace,0,true, "Silent Running"),
        ];

        tactions.appendChild(TableRowMultitdlist(actionlist))
    }
    else if (currentshiptype == "SRV")
    {
        var statuslist = [
            createIndicator("SrvUnderShip"), createIndicator("LowFuel"), createIndicator("ShieldsUp")
        ];

        var actionlist = [
            createAction( "SrvHandbrake", "AutoBreakBuggyButton"),
            createAction( "SrvTurret", "ToggleBuggyTurretButton"),
            createAction( "SrvDriveAssist", "ToggleDriveAssist"),
            createAction( "Lights", "HeadlightsBuggyButton"),

            createActionButton("RecallDismissShip", "F10"),

            createActionButton( "IncreaseSystemsPower"),
            createActionButton( "IncreaseEnginesPower"),
            createActionButton( "IncreaseWeaponsPower"),
            createActionButton( "ResetPowerDistribution"),

            createAction("GalaxyMapOpen"),
            createAction("SystemMapOpen"),
            createActionButton("Screenshot", "F10", true, "Screen Shot"),
        ];

        tstatus.appendChild(TableRowMultitdlist(statuslist));
        tactions.appendChild(TableRowMultitdlist(actionlist));
    }
    else if (currentshiptype == "Fighter")
    {
        var statuslist = [
            createIndicator("ShieldsUp")
        ]

        var actionlist = [
            createAction("Lights", "ShipSpotLightToggle"),
            createAction("FlightAssist", "ToggleFlightAssist"),
            createAction("NightVision", "NightVisionToggle"),
            createActionButton("IncreaseSystemsPower"),
            createActionButton("IncreaseEnginesPower"),
            createActionButton("IncreaseWeaponsPower"),
            createActionButton("ResetPowerDistribution"),

            createActionButton("OrderDefensiveBehaviour"),
            createActionButton("OrderAggressiveBehaviour"),
            createActionButton("OrderFocusTarget"),
            createActionButton("OrderHoldFire"),
            createActionButton("OrderHoldPosition"),
            createActionButton("OrderFollow"),
            createActionButton("OrderRequestDock"),
            createActionButton("OpenOrders"),

            createAction("GalaxyMapOpen"),
            createAction("SystemMapOpen"),
            createActionButton("Screenshot", "F10", true, "Screen Shot"),
        ];

        tstatus.appendChild(TableRowMultitdlist(statuslist));
        tactions.appendChild(TableRowMultitdlist(actionlist));
    }
    else if (currentshiptype == "OnFoot")
    {
        var statuslist = [
            createIndicator("ShieldsUp")
        ]

        var actionlist = [
            createAction("Reload", "HumanoidReloadButton"),
            createAction("SwitchWeapon", "HumanoidSwitchWeapon"),
            createAction("1", "HumanoidSelectPrimaryWeaponButton"),
            createAction("1", "HumanoidSelectSecondaryWeaponButton"),
            createAction("1", "HumanoidSelectUtilityWeaponButton"),
            createAction("1", "HumanoidSelectPreviousWeaponButton"),
            createAction("1", "HumanoidSelectNextWeaponButton"),
            createAction("1", "HumanoidHideWeaponButton"),
            createAction("1", "HumanoidSelectNextGrenadeTypeButton"),
            createAction("1", "HumanoidSelectPreviousGrenadeTypeButton"),
            createAction("1", "HumanoidToggleFlashlightButton"),
            createAction("ShieldsUp", "HumanoidToggleShieldsButton"),
            createAction("1", "HumanoidSwitchToRechargeTool"),
            createAction("1", "HumanoidSwitchToCompAnalyser"),
            createAction("1", "HumanoidSwitchToSuitTool"),
            createAction("1", "HumanoidToggleToolModeButton"),
            createAction("1", "HumanoidToggleMissionHelpPanelButton"),
            createAction("GalaxyMapOpen", "GalaxyMapOpen_Humanoid"),
            createAction("SystemMapOpen", "SystemMapOpen_Humanoid"),
            createAction("1", "FocusCommsPanel_Humanoid"),
            createAction("1", "QuickCommsPanel_Humanoid"),
            createAction("1", "HumanoidOpenAccessPanelButton"),
            createAction("1", "HumanoidConflictContextualUIButton"),
        ];

        tstatus.appendChild(TableRowMultitdlist(statuslist));
        tactions.appendChild(TableRowMultitdlist(actionlist));
    }
    else if (currentshiptype == "Multicrew")
    {
        tstatus.appendChild(CreatePara("Multicrew not supported yet"));
    }
    else
    {
        tstatus.appendChild(CreatePara("Elite not running/Unknown mode"));
    }
}

export function SetIndicatorState(jdata, tstatus)
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
                        //console.log("Entry is " + z.nodeName + " " + z.tag);

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

export function ClickActionItem(e)
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

