
// Request scan data

function RequestScanData(entryno)
{
    console.log("Request scan data on " + entryno);
    var msg = {
        requesttype: "scandata",
        entry: entryno,	// -1 means send me the latest journal entry first
    };

    websocket.send(JSON.stringify(msg));
}



function FillScanTable(jdata, showmaterials, showvalue)
{
    var stable = document.getElementById("scans");

    removeChildren(stable);

    if (jdata.Bodies == undefined)
        return;

    var oneLS_m = 299792458;
    var oneAU_m = 149597870700;
    var oneAU_LS = oneAU_m / oneLS_m;
    var oneSolRadius_m = 695700000; // 695,700km
    var oneEarthRadius_m = 6371000;
    var oneAtmosphere_Pa = 101325;
    var oneGee_m_s2 = 9.80665;

    for (var e = 0; e < jdata.Bodies.length; e++ )
    {
        var sn = jdata.Bodies[e];
        var scandata = sn.Scan;

        if (scandata != null)
        {
           // console.log("STable " + e + " " + sn.NodeType);

            var bdclass = "";
            var bddist = "";
            var bddetails = new Array(2);
            bddetails[0] = "";
            bddetails[1] = "";
            var imagename = "";

            if (sn.NodeType == "ring")
            {
            }
            else if (sn.NodeType == "beltcluster")
            {
                bdclass = "Belt Cluster";
                if (scandata.ScanType == "Detailed")
                    bddetails[0] = "Scanned";
                else
                    bddetails[0] = "No scan data available";

                bddist = scandata.DistanceFromArrivalLS.toFixed(2) + "ls";

                imagename = "Controls.Scan.Bodies.Belt";
            }
            else if (scandata.IsStar)
            {
                bdclass = scandata.StarTypeText;
                if (scandata.nSemiMajorAxis != null)
                    bddist = (scandata.nSemiMajorAxis / oneAU_m).toFixed(2) + "AU (" + (scandata.nSemiMajorAxis / oneLS_m).toFixed(1) + "ls)";
                else
                    bddist = "Main";

                if (scandata.nStellarMass != null)
                    bddetails[0] = Append(bddetails[0], "Mass: " + scandata.nStellarMass.toFixed(2));
                if (scandata.nRadius != null)
                    bddetails[0] = Append(bddetails[0], "Radius: " + (scandata.nRadius / oneSolRadius_m).toFixed(2));
                if (scandata.nSurfaceTemperature != null)
                    bddetails[0] = Append(bddetails[0], "Temperature: " + scandata.nSurfaceTemperature.toFixed(0) + "K");

                imagename = scandata.StarTypeImageName;
            }
            else if (scandata.IsPlanet)
            {
                bdclass = scandata.PlanetTypeText;
                if (scandata.Terraformable)
                    bdclass = "Terraformable " + bdclass;

                if (sn.Level >= 2)
                {
                    bdclass += " Moon";

                    // tbd distance from arrival as well?
                    if (scandata.nSemiMajorAxis != null)
                        bddist = (scandata.nSemiMajorAxis / oneLS_m).toFixed(1) + "ls (" + (scandata.nSemiMajorAxis/1000).toFixed(0) + "km)";
                }
                else
                {
                    bddist = (scandata.DistanceFromArrivalLS / oneAU_LS).toFixed(2) + "AU (" + scandata.DistanceFromArrivalLS.toFixed(1) + "ls)";
                }

                if (scandata.nRadius != null)
                    bddetails[0] = Append(bddetails[0], "Radius: " + (scandata.nRadius / oneEarthRadius_m).toFixed(2));

                if (scandata.nSurfaceTemperature != null)
                    bddetails[0] = Append(bddetails[0], "Temperature: " + scandata.nSurfaceTemperature.toFixed(0) + "K");

                if (scandata.Atmosphere != null && scandata.Atmosphere != "None")
                {
                    bddetails[0] = Append(bddetails[0], "Atmosphere: " + scandata.Atmosphere);
                    if (scandata.nSurfacePressure != null)
                        bddetails[0] = Append(bddetails[0], (scandata.nSurfacePressure / oneAtmosphere_Pa).toFixed(3) + "Pa.");
                }

                if (scandata.IsLandable)
                {
                    if (scandata.nSurfaceGravity != null)
                        bddetails[0] = Append(bddetails[0], "Landable: " + (scandata.nSurfaceGravity / oneGee_m_s2).toFixed(2) + "g");
                    else
                        bddetails[0] = Append(bddetails[0], "Landable");
                }

                if (scandata.Volcanism != null)
                {
                    bddetails[0] = Append(bddetails[0], "Volcanism: " + scandata.Volcanism);
                }

                if (scandata.Mapped)
                {
                    bddetails[0] = Append(bddetails[0], "Mapped");
                }

                if (showmaterials && scandata.Materials != null )
                {
                    const keys = Object.keys(scandata.Materials);

                    var t = "";
                    keys.forEach((key, index) =>
                    {
                        console.log(`${key}: ${scandata.Materials[key]}`);
                        t = Append(t, key, ', ');
                    });

                    if (t != "")
                        bddetails[1] = Append(bddetails[1], "Mats:" + t);
                }

                imagename = scandata.PlanetClassImageName;
            }

            if (bdclass != "")
            {
                if ( showvalue)
                    bddetails[0] = Append(bddetails[0], "Value " + scandata.EstimatedValue);

            //    console.log("Image name " + imagename);
                var image = CreateImage("/images/" + imagename + ".png", imagename, 48);

                stable.appendChild(tablerowmultitdlist([image, scandata.BodyDesignationOrName, bdclass, bddist, bddetails]));
            }
        }
    }
}