
// Request scan data

function RequestScanData(entryno)
{
    console.log("Request scan data on " + entryno);
    entryno = 16404;
    var msg = {
        requesttype: "scandata",
        entry: entryno,	// -1 means send me the latest journal entry first
    };

    websocket.send(JSON.stringify(msg));
}



function FillScanTable(jdata)
{
    var stable = document.getElementById("scans");

    removeChildren(stable);

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
            var bddetails = "";

            if (sn.NodeType == "ring")
            {
            }
            else if (sn.NodeType == "beltcluster")
            {
                bdclass = "Belt Cluster";
                if (scandata.ScanType == "Detailed")
                    bddetails = "Scanned";
                else
                    bddetails = "No scan data available";

                bddist = scandata.DistanceFromArrivalLS;
            }
            else if (scandata.IsStar)
            {
                bdclass = scandata.StarTypeText;
                if (scandata.nSemiMajorAxis != null)
                    bddist = (scandata.nSemiMajorAxis / oneAU_m).toFixed(2) + "AU (" + (scandata.nSemiMajorAxis / oneAU_LS).ToFixed(1) + "ls)";
                else
                    bddist = "Main";

                if (scandata.nStellarMass != null)
                    bddetails = Append(bddetails, "Mass: " + scandata.nStellarMass.toFixed(2));
                if (scandata.nRadius != null)
                    bddetails = Append(bddetails, "Radius: " + (scandata.nRadius / oneSolRadius_m).toFixed(2));
                if (scandata.nSurfaceTemperature != null)
                    bddetails = Append(bddetails, "Temperature: " + scandata.nSurfaceTemperature.toFixed(0) + "K");
            }
            else if (scandata.IsPlanet)
            {
                bdclass = scandata.PlanetTypeText;
                if (scandata.Terraformable)
                    bdclass = "Terraformable " + bdclass;

                if (sn.Level >= 2)
                {
                    bdclass += " Moon";

                    if (scandata.nSemiMajorAxis != null)
                        bddist = (scandata.nSemiMajorAxis / oneLS_m).toFixed(1) + "ls (" + (scandata.nSemiMajorAxis/1000).toFixed(0) + "km)";
                }
                else
                {
                    bddist = (scandata.DistanceFromArrivalLS / oneAU_LS).toFixed(2) + "AU (" + scandata.DistanceFromArrivalLS.toFixed(1) + "ls)";
                }

                if (scandata.nRadius != null)
                    bddetails = Append(bddetails, "Radius: " + (scandata.nRadius / oneEarthRadius_m).toFixed(2));

                if (scandata.nSurfaceTemperature != null)
                    bddetails = Append(bddetails, "Temperature: " + scandata.nSurfaceTemperature.toFixed(0) + "K");

                if (scandata.Atmosphere != null && scandata.Atmosphere != "None")
                {
                    bddetails = Append(bddetails, "Atmosphere: " + scandata.Atmosphere);
                    if (scandata.nSurfacePressure != null)
                        bddetails = Append(bddetails, (scandata.nSurfacePressure / oneAtmosphere_Pa).toFixed(3) + "Pa.");
                }

                if (scandata.IsLandable)
                {
                    if (scandata.nSurfaceGravity != null)
                        bddetails = Append(bddetails, "Landable: " + (scandata.nSurfaceGravity / oneGee_m_s2).toFixed(2) + "g");
                    else
                        bddetails = Append(bddetails, "Landable");
                }

                if (scandata.Volcanism != null)
                {
                    bddetails = Append(bddetails, "Volcanism: " + scandata.Volcanism);
                }

                if (scandata.Mapped)
                {
                    bddetails = Append(bddetails, "Mapped");
                }

            }

            if (bdclass != "")
            {
                bddetails = Append(bddetails, "Value " + scandata.EstimatedValue);

                var imagename = scandata.IsStar ? scandata.StarTypeImageName : scandata.PlanetClassImageName;
                var image = CreateImage("/images/" + imagename + ".png", imagename, 48);
                var t1 = document.createTextNode(scandata.BodyDesignationOrName);
                var t2 = document.createTextNode(bdclass);
                var t3 = document.createTextNode(bddist);
                var t4 = document.createTextNode(bddetails);

                stable.appendChild(tablerowmultitdlist([image, t1, t2, t3, t4]));
            }
        }
    }
}