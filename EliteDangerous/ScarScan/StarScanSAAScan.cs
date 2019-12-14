/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
 
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;

namespace EliteDangerousCore
{
    public partial class StarScan
    {
        // used by historylist directly for a single update during play, in foreground..  Also used by above.. so can be either in fore/back
        public bool AddSAAScanToBestSystem(JournalSAAScanComplete jsaa, int startindex, List<HistoryEntry> hl)
        {
            for (int j = startindex; j >= 0; j--)
            {
                HistoryEntry he = hl[j];

                if (he.IsLocOrJump)
                {
                    JournalLocOrJump jl = (JournalLocOrJump)he.journalEntry;
                    string designation = GetBodyDesignationSAAScan(jsaa, he.System.Name);

                    if (IsStarNameRelated(he.System.Name, designation))       // if its part of the name, use it
                    {
                        jsaa.BodyDesignation = designation;
                        return ProcessSAAScan(jsaa, he.System, true);
                    }
                    else if (jl != null && IsStarNameRelated(jl.StarSystem, designation))
                    {
                        // Ignore scans where the system name has changed
                        return false;
                    }
                }
            }

            jsaa.BodyDesignation = GetBodyDesignationSAAScan(jsaa, hl[startindex].System.Name);
            return ProcessSAAScan(jsaa, hl[startindex].System, true);         // no relationship, add..
        }

        private bool ProcessSAAScan(JournalSAAScanComplete jsaa, ISystem sys, bool reprocessPrimary = false)  // background or foreground.. FALSE if you can't process it
        {
            SystemNode sn = GetOrCreateSystemNode(sys);
            ScanNode relatednode = null;

            if (sn.NodesByID.ContainsKey((int)jsaa.BodyID))
            {
                relatednode = sn.NodesByID[(int)jsaa.BodyID];
                if (relatednode.ScanData != null && relatednode.ScanData.BodyDesignation != null)
                {
                    jsaa.BodyDesignation = relatednode.ScanData.BodyDesignation;
                }
            }
            else if (jsaa.BodyDesignation != null && jsaa.BodyDesignation != jsaa.BodyName)
            {
                foreach (var body in sn.Bodies)
                {
                    if (body.fullname == jsaa.BodyDesignation)
                    {
                        relatednode = body;
                        break;
                    }
                }
            }

            if (relatednode == null)
            {
                foreach (var body in sn.Bodies)
                {
                    if ((body.fullname == jsaa.BodyName || body.customname == jsaa.BodyName) &&
                        (body.fullname != sys.Name || body.level != 0))
                    {
                        relatednode = body;
                        break;
                    }
                }
            }

            if (relatednode != null)
            {
                relatednode.IsMapped = true;        // keep data here since we can get scans replaced later..
                relatednode.WasMappedEfficiently = jsaa.ProbesUsed <= jsaa.EfficiencyTarget;
                //System.Diagnostics.Debug.WriteLine("Setting SAA Scan for " + jsaa.BodyName + " " + sys.Name + " to Mapped: " + relatedScan.WasMappedEfficiently);

                if (relatednode.ScanData != null)       // if we have a scan, set its values - this keeps the calculation self contained in the class.
                {
                    relatednode.ScanData.SetMapped(relatednode.IsMapped, relatednode.WasMappedEfficiently);
                    //System.Diagnostics.Debug.WriteLine(".. passing down to scan " + relatedScan.ScanData.ScanType);
                }

                return true; // We already have the scan
            }

            return false;
        }

        private string GetBodyDesignationSAAScan(JournalSAAScanComplete je, string system)
        {
            if (je.BodyName == null || system == null)
                return null;

            string bodyname = je.BodyName;
            int bodyid = (int)je.BodyID;

            if (bodyIdDesignationMap.ContainsKey(system) && bodyIdDesignationMap[system].ContainsKey(bodyid) && bodyIdDesignationMap[system][bodyid].NameEquals(bodyname))
            {
                return bodyIdDesignationMap[system][bodyid].Designation;
            }

            if (planetDesignationMap.ContainsKey(system) && planetDesignationMap[system].ContainsKey(bodyname))
            {
                return planetDesignationMap[system][bodyname];
            }

            if (bodyname.Equals(system, StringComparison.InvariantCultureIgnoreCase) || bodyname.StartsWith(system + " ", StringComparison.InvariantCultureIgnoreCase))
            {
                return bodyname;
            }

            return bodyname;
        }


        #region FSS DISCOVERY *************************************************************

        public void SetFSSDiscoveryScan(JournalFSSDiscoveryScan je, ISystem sys)
        {
            SystemNode sn = GetOrCreateSystemNode(sys);
            sn.FSSTotalBodies = je.BodyCount;
        }

        #endregion

    }
}
