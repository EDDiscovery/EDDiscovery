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

using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ScanNode relatedScan = null;

            if (sn.NodesByID.ContainsKey((int)jsaa.BodyID))
            {
                relatedScan = sn.NodesByID[(int)jsaa.BodyID];
                if (relatedScan.ScanData != null && relatedScan.ScanData.BodyDesignation != null)
                {
                    jsaa.BodyDesignation = relatedScan.ScanData.BodyDesignation;
                }
            }
            else if (jsaa.BodyDesignation != null && jsaa.BodyDesignation != jsaa.BodyName)
            {
                foreach (var body in sn.Bodies)
                {
                    if (body.fullname == jsaa.BodyDesignation)
                    {
                        relatedScan = body;
                        break;
                    }
                }
            }

            if (relatedScan == null)
            {
                foreach (var body in sn.Bodies)
                {
                    if ((body.fullname == jsaa.BodyName || body.customname == jsaa.BodyName) &&
                        (body.fullname != sys.Name || body.level != 0))
                    {
                        relatedScan = body;
                        break;
                    }
                }
            }

            if (relatedScan != null)
            {
                relatedScan.IsMapped = true;        // keep data here since we can get scans replaced later..
                relatedScan.WasMappedEfficiently = jsaa.ProbesUsed <= jsaa.EfficiencyTarget;
                //System.Diagnostics.Debug.WriteLine("Setting SAA Scan for " + jsaa.BodyName + " " + sys.Name + " to Mapped: " + relatedScan.WasMappedEfficiently);

                if (relatedScan.ScanData != null)       // if we have a scan, set its values - this keeps the calculation self contained in the class.
                {
                    relatedScan.ScanData.SetMapped(relatedScan.IsMapped, relatedScan.WasMappedEfficiently);
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
    }
}
