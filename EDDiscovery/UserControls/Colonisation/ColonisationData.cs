/*
 * Copyright 2025 - 2025 EDDiscovery development team
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

using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System.Collections.Generic;
using System.Linq;

namespace EDDiscovery.UserControls.Colonisation
{
    [System.Diagnostics.DebuggerDisplay("{System.Name} np {Ports.Count}")]
    public class ColonisationSystemData
    {
        public ISystem System { get; set; }
        public Dictionary<long, ColonisationPortData> Ports { get; set; } = new Dictionary<long, ColonisationPortData>();

        public HistoryEntry LastLocOrJump { get; set; }

        public bool ClaimReleased { get; set; }
        public bool BeaconDeployed { get; set; }

        public static bool AddColonisationEntry(Dictionary<long,ColonisationSystemData> systems, HistoryEntry he, List<HistoryEntry> hl)
        {
            if (he.journalEntry is JournalColonisationSystemClaim jas)
            {
                ColonisationSystemData.MakeSystem(systems, hl, new SystemClass(jas.StarSystem, jas.SystemAddress));
                return true;
            }
            else if (he.journalEntry is JournalColonisationSystemClaimRelease cr)
            {
                if (systems.TryGetValue(cr.SystemAddress, out ColonisationSystemData sr))
                {
                    sr.ClaimReleased = true;
                    return true;
                }
            }
            else if (he.journalEntry is JournalColonisationConstructionDepot cd)
            {
                if (cd.MarketID == he.Status.MarketID)        // double check on location..
                {
                    var port = ColonisationSystemData.MakeSystemAndPort(systems, hl , he.System, cd.MarketID, he.WhereAmI);
                    port.State = cd;
                    //global::System.Diagnostics.Debug.WriteLine($"Colonisation Depot {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID} CMID {cd.MarketID} {cd.ConstructionProgress}");
                    return true;
                }
                else
                {
                    // if it disagrees, its due to the renaming of the station and market ID on progress  = 1
                    if (cd.ConstructionProgress < 1)
                        global::System.Diagnostics.Debug.WriteLine($"Colonisation Depot ERROR Rename without progress=1 {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID} CMID {cd.MarketID} {cd.ConstructionProgress}");

                    var system = ColonisationSystemData.MakeSystem(systems, hl, he.System);

                    var port = system.Ports.Values.ToList().Find(x => x.Name.Contains(he.WhereAmI)); // see if we can find a port with the text in it

                    if (port != null)
                    {
                        //global::System.Diagnostics.Debug.WriteLine($"Colonisation Depot Rename Port {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID} CMID {cd.MarketID} {cd.ConstructionProgress}");
                        port.Name = he.WhereAmI;
                        port.MarketID = he.Status.MarketID.Value;
                        return true;
                    }
                    else
                    {
                        global::System.Diagnostics.Debug.WriteLine($"Colonisation Depot ERROR no port found with associated text {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID} CMID {cd.MarketID} {cd.ConstructionProgress}");
                    }
                }
            }
            else if (he.journalEntry is JournalColonisationContribution cb)
            {
                if (cb.MarketID == he.Status.MarketID)        // double check on location..
                {
                    var port = ColonisationSystemData.MakeSystemAndPort(systems, hl, he.System, cb.MarketID, he.WhereAmI);
                    port.Contributions.Add(cb);
                    //global::System.Diagnostics.Debug.WriteLine($"Colonisation Contribution {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID} CMID {cb.MarketID}");
                    return true;
                }
                else
                {
                    global::System.Diagnostics.Debug.WriteLine($"Colonisation Contribution ERROR {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID} CMID {cb.MarketID}");
                }
            }
            else if (he.journalEntry is JournalColonisationBeaconDeployed br)
            {
                if (systems.TryGetValue(he.System.SystemAddress.Value, out ColonisationSystemData sr))
                {
                    sr.BeaconDeployed = true;
                    return true;
                }
            }

            else if (he.journalEntry is JournalLocOrJump fsd)     // update the FSD entry
            {
                if (fsd.SystemAddress.HasValue)
                {
                    if (systems.TryGetValue(fsd.SystemAddress.Value, out ColonisationSystemData sr))
                    {
                        sr.LastLocOrJump = he;
                        return true;
                    }
                }
            }

            else if (he.journalEntry is ILocDocked ld && ld.Docked == true)         // for docked ILocDocked types
            {
                if (ld.SystemAddress.HasValue && ld.MarketID.HasValue)
                {
                    if (systems.TryGetValue(ld.SystemAddress.Value, out ColonisationSystemData sr))
                    {
                        if (sr.Ports.TryGetValue(ld.MarketID.Value, out ColonisationPortData port))
                        {
                            port.LastDockedOrLocation = ld;
                            global::System.Diagnostics.Debug.WriteLine($"Colonisation LocDock {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID} {port.Name} {port.MarketID}");

                            return true;
                        }
                        else
                        {
                            //global::System.Diagnostics.Debug.WriteLine($"Colonisation LocDock Not Found {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID}");
                        }
                    }
                }

            }

            return false;
        }


        private static ColonisationSystemData MakeSystem(Dictionary<long, ColonisationSystemData> systems, List<HistoryEntry> hl, ISystem system )
        {
            if (!systems.TryGetValue(system.SystemAddress.Value, out ColonisationSystemData sr))
            {
                systems.Add(system.SystemAddress.Value, sr = new ColonisationSystemData() { System = system });
                sr.LastLocOrJump = hl.Find(x => x.IsFSD && x.System.SystemAddress == system.SystemAddress);    // may be null
            }
            return sr;
        }

        private static ColonisationPortData MakeSystemAndPort(Dictionary<long, ColonisationSystemData> systems, List<HistoryEntry> hl, 
                            ISystem system, long marketid, string marketname)
        {
            ColonisationSystemData sys = MakeSystem(systems, hl, system);

            if (!sys.Ports.TryGetValue(marketid, out ColonisationPortData port))        // if not in there
            {
                // make it, note we can't have docked at it if its
                port = new ColonisationPortData() { Name = marketname, MarketID = marketid };
                sys.Ports.Add(marketid, port);

                // find last loc docked, check its docked (may be location not docked), and then see if the name 
                // matches or the current market name equals the prefix (caused during renaming of the station when completed)
                var hef = hl.Find(x => (x.journalEntry is ILocDocked xa ) && xa.Docked && (xa.StationName == marketname || xa.StationName.Contains(": " + marketname)));
                port.LastDockedOrLocation = hef?.journalEntry as ILocDocked;
            }
            else
            {
                if ( port.Name != marketname)
                {
                    global::System.Diagnostics.Debug.WriteLine($"Colonisation SystemAndPort {system.Name}:{marketid} {marketname} changed name from {port.Name}");
                    port.Name = marketname;
                }
            }

            return port;
        }
    }

    [System.Diagnostics.DebuggerDisplay("{Name} {State.ConstructionProgress}")]
    public class ColonisationPortData
    {
        public string Name { get; set; }                                    // name of port. Found by looking at HE information when depot is received.  Can change when progress=1
        public long MarketID { get; set; }                                  // market ID of port - can change when progress=1
        public JournalColonisationConstructionDepot State { get; set; }     // holds last Depot IF any - ignore MarketID, may be wrong, progress, Complete/Failed and resources
        public List<JournalColonisationContribution> Contributions { get; set; } = new List<JournalColonisationContribution>();
        public ILocDocked LastDockedOrLocation { get; set; }
    }

}
