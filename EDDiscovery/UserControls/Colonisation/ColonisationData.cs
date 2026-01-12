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

using CAPI;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDDiscovery.UserControls.Colonisation
{
    public class ColonisationData
    {
        public Dictionary<long, ColonisationSystemData> Systems { get; set; } = new Dictionary<long, ColonisationSystemData>();
        public ColonisationSystemData LastCreatedSystem { get; set; } = null;

        private HashSet<long> normalsystem = new HashSet<long>();

        public class Ret
        {
            public bool newsystem;
            public ColonisationSystemData csd;
            public ColonisationPortData port;
            public Ret(bool nw, ColonisationSystemData c, ColonisationPortData p) { newsystem = nw; csd = c; port = p; }

        }

        public void Clear()
        {
            Systems.Clear();
            normalsystem.Clear();
            LastCreatedSystem = null;
        }

        // return if system list has been added/removed
        // and return if a system has been modified (add or modified) - may be null
        // and return if a port has been modified (add or modified) - may be null
        public Ret Add(HistoryEntry he, List<HistoryEntry> hl, bool debug = false)
        {
            if (!he.System.SystemAddress.HasValue)      // check we have a system value to do work with, we may have come in 1/2 way thru and not know the system yet
            {
            }
            else if (he.journalEntry is JournalColonisationSystemClaim jas)
            {
                global::System.Diagnostics.Debug.WriteLine($"{he.EventTimeUTC} Colonisation claim {jas.StarSystem}");
                bool newsystem = MakeSystem(hl, new SystemClass(jas.StarSystem, jas.SystemAddress), out ColonisationSystemData sys);
                return new Ret(newsystem, sys, null);
            }
            else if (he.journalEntry is JournalColonisationBeaconDeployed br)
            {
                global::System.Diagnostics.Debug.WriteLine($"{he.EventTimeUTC} Colonisation beacon deployed {he.System.Name}");
                if (Systems.TryGetValue(he.System.SystemAddress.Value, out ColonisationSystemData sys))
                {
                    sys.BeaconDeployed = true;
                    return new Ret(false, sys, null);
                }
            }
            else if (he.journalEntry is ILocDocked ld && ld.Docked == true)         // for docked ILocDocked types
            {
                //global::System.Diagnostics.Debug.WriteLine($"{he.EventTimeUTC} {ld.MarketID} {ld.MarketClass()} Colonisation docked `{ld.StationName}` {ld.FDStationType} @ {ld.StarSystem} ");

                // if we have a valid system and market, and not in our list of normal systems to ignore
                if (ld.SystemAddress.HasValue && ld.MarketID.HasValue && !normalsystem.Contains(ld.SystemAddress.Value))
                {
                    var cls = ld.MarketClass();
                    if (cls == StationDefinitions.Classification.NormalPort)       // is this market ID a normal port?
                    {
                        normalsystem.Add(ld.SystemAddress.Value);           // add to banned list
                        if (Systems.ContainsKey(ld.SystemAddress.Value))   // if we already added it..
                        {
                            global::System.Diagnostics.Debug.WriteLine($"{he.EventTimeUTC} {ld.MarketID} {ld.MarketClass()} Colonisation docked remove previous system as normal port found `{ld.StationName}` {ld.FDStationType} @ {ld.StarSystem}");
                            Systems.Remove(ld.SystemAddress.Value);
                            return new Ret(true, null, null);
                        }
                        else
                        {       // no action, normal return
                        }

                    }
                    // if its a colonisation type port

                    else if (cls == StationDefinitions.Classification.SpaceConstructionDepot || cls == StationDefinitions.Classification.ColonisationShip || cls == StationDefinitions.Classification.ColonisationPort)
                    {
                        if (!Systems.ContainsKey(ld.SystemAddress.Value)) global::System.Diagnostics.Debug.WriteLine($"{he.EventTimeUTC} {ld.MarketID} {ld.MarketClass()} Colonisation docked created new system entry`{ld.StationName}` {ld.FDStationType} @ {ld.StarSystem}");

                        bool newsystem = MakeSystem(hl, he.System, out ColonisationSystemData sys);     // make the system
                        bool newport = MakePort(sys, hl, ld.MarketID.Value, he.WhereAmI, out ColonisationPortData port);    // make the port..

                        port.LastDockedOrLocation = ld;
                        global::System.Diagnostics.Debug.WriteLine($"Colonisation LocDock on Port {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID} {port.Name} {port.MarketID}");
                        return new Ret(newsystem, sys, port);
                    }
                }
            }
            else if (he.journalEntry is JournalColonisationSystemClaimRelease cr)
            {
                if (Systems.TryGetValue(cr.SystemAddress, out ColonisationSystemData sys))
                {
                    sys.ClaimReleased = true;
                    return new Ret(false, sys, null);
                }
            }
            else if (he.journalEntry is JournalColonisationConstructionDepot cd)
            {
                var classify = StationDefinitions.Classify(cd.MarketID, StationDefinitions.StarportTypes.Unknown);
                global::System.Diagnostics.Debug.WriteLine($"{he.EventTimeUTC} {cd.MarketID} {classify} Colonisation depot `{he.Status.StationName_Localised}` {he.Status.FDStationType} {cd.ConstructionProgress * 100.0:N2} @ {he.System.Name} ");

                if (cd.MarketID == he.Status.MarketID)        // double check on location..
                {
                    bool newsystem = MakeSystem(hl, he.System, out ColonisationSystemData sys);     // make the system
                    bool newport = MakePort(sys, hl, cd.MarketID, he.WhereAmI, out ColonisationPortData port);
                    port.State = cd;
                    //global::System.Diagnostics.Debug.WriteLine($"Colonisation Depot {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID} CMID {cd.MarketID} {cd.ConstructionProgress}");
                    return new Ret(newsystem, sys, port);
                }
                else
                {
                    // if it disagrees, its due to the renaming of the station and market ID on progress  = 1
                    if (cd.ConstructionProgress < 1)
                        global::System.Diagnostics.Debug.WriteLine($"Colonisation Depot ERROR Rename without progress=1 {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID} CMID {cd.MarketID} {cd.ConstructionProgress}");

                    bool newsystem = MakeSystem(hl, he.System, out ColonisationSystemData sys);     // make the system

                    var port = sys.Ports.Values.ToList().Find(x => x.Name.Contains(he.WhereAmI)); // see if we can find a port with the text in it

                    if (port != null)
                    {
                        //global::System.Diagnostics.Debug.WriteLine($"Colonisation Depot Rename Port {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID} CMID {cd.MarketID} {cd.ConstructionProgress}");
                        port.Name = he.WhereAmI;
                        port.MarketID = he.Status.MarketID.Value;
                        return new Ret(newsystem, sys, port);
                    }
                    else
                    {
                        global::System.Diagnostics.Debug.WriteLine($"Colonisation Depot ERROR no port found with associated text {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID} CMID {cd.MarketID} {cd.ConstructionProgress}");
                        return new Ret(newsystem, sys, null);
                    }
                }
            }
            else if (he.journalEntry is JournalColonisationContribution cb)
            {
                if (cb.MarketID == he.Status.MarketID)        // double check on location..
                {
                    bool newsystem = MakeSystem(hl, he.System, out ColonisationSystemData sys);     // make the system
                    bool newport = MakePort(sys, hl, cb.MarketID, he.WhereAmI, out ColonisationPortData port);
                    port.Contributions.Add(cb);
                    global::System.Diagnostics.Debug.WriteLine($"{he.EventTimeUTC} Colonisation Contribution {he.System} {he.WhereAmI} {he.Status.MarketID} CMID {cb.MarketID}");
                    return new Ret(newsystem, sys, port);
                }
                else
                {
                    global::System.Diagnostics.Debug.WriteLine($"Colonisation Contribution ERROR {he.EventTimeUTC} {he.System} {he.WhereAmI} {he.Status.MarketID} CMID {cb.MarketID}");
                }
            }

            else if (he.journalEntry is JournalLocOrJump fsd)     // on Location or FSDJump, update the loc or jump entry
            {
                if (fsd.SystemAddress.HasValue)
                {
                    if (Systems.TryGetValue(fsd.SystemAddress.Value, out ColonisationSystemData sys))
                    {
                        sys.LastLocOrJump = he;
                        global::System.Diagnostics.Debug.WriteLine($"{he.EventTimeUTC} Colonisation FSD/Loc update info on {he.System} {he.WhereAmI}");
                        return new Ret(false, sys, null);
                    }
                }
            }

            return new Ret(false, null, null);
        }

        // make a system and return if its new.
        private bool MakeSystem(List<HistoryEntry> hl, ISystem system, out ColonisationSystemData sys)
        {
            if (!Systems.TryGetValue(system.SystemAddress.Value, out sys))
            {
                Systems.Add(system.SystemAddress.Value, sys = new ColonisationSystemData() { System = system });
                sys.LastLocOrJump = hl.Find(x => x.IsFSD && x.System.SystemAddress == system.SystemAddress);    // may be null
                LastCreatedSystem = sys;
                return true;
            }
            return false;
        }

        private static bool MakePort( ColonisationSystemData sys, List<HistoryEntry> hl, long marketid, string marketname, out ColonisationPortData port)
        {
            if (!sys.Ports.TryGetValue(marketid, out port))        // if not in there
            {
                // make it, note we can't have docked at it if its
                port = new ColonisationPortData() { Name = marketname, MarketID = marketid };
                sys.Ports.Add(marketid, port);

                // find last loc docked, check its docked (may be location not docked), and then see if the name 
                // matches or the current market name equals the prefix (caused during renaming of the station when completed)
                var hef = hl.Find(x => (x.journalEntry is ILocDocked xa) && xa.Docked && (xa.StationName == marketname || xa.StationName.Contains(": " + marketname)));
                port.LastDockedOrLocation = hef?.journalEntry as ILocDocked;

                return true;
            }
            else
            {
                if (port.Name != marketname)
                {
                    global::System.Diagnostics.Debug.WriteLine($"Colonisation SystemAndPort {sys.System.Name}:{marketid} {marketname} changed name from {port.Name}");
                    port.Name = marketname;
                }

                return false;
            }
        }
    }


    [System.Diagnostics.DebuggerDisplay("{System.Name} np {Ports.Count}")]
    public class ColonisationSystemData
    {
        public ISystem System { get; set; }
        public Dictionary<long, ColonisationPortData> Ports { get; set; } = new Dictionary<long, ColonisationPortData>();

        public HistoryEntry LastLocOrJump { get; set; }

        public bool ClaimReleased { get; set; }
        public bool BeaconDeployed { get; set; }
    }

    [System.Diagnostics.DebuggerDisplay("{Name} {State?.ConstructionProgress}")]
    public class ColonisationPortData
    {
        public string Name { get; set; }                                    // name of port. Found by looking at HE information when depot is received.  Can change when progress=1
        public long MarketID { get; set; }                                  // market ID of port - can change when progress=1
        public JournalColonisationConstructionDepot State { get; set; }     // holds last Depot IF any - ignore MarketID, may be wrong, progress, Complete/Failed and resources
        public List<JournalColonisationContribution> Contributions { get; set; } = new List<JournalColonisationContribution>();
        public ILocDocked LastDockedOrLocation { get; set; }
    }

}
