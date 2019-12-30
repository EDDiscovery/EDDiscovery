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
        public bool AddSAASignalsFoundToBestSystem(JournalSAASignalsFound jsaa, int startindex, List<HistoryEntry> hl)
        {
            if (jsaa.Signals == null)       // be paranoid, don't add if null signals
                return false;

            for (int j = startindex; j >= 0; j--)
            {
                HistoryEntry he = hl[j];

                if (he.IsLocOrJump)
                {
                    JournalLocOrJump jl = (JournalLocOrJump)he.journalEntry;
                    string designation = GetBodyDesignationSAASignalsFound(jsaa, he.System.Name);

                    if (IsStarNameRelated(he.System.Name, designation))       // if its part of the name, use it
                    {
                        jsaa.BodyDesignation = designation;
                        return ProcessSAASignalsFound(jsaa, he.System, true);
                    }
                    else if (jl != null && IsStarNameRelated(jl.StarSystem, designation))
                    {
                        // Ignore scans where the system name has changed
                        return false;
                    }
                }
            }

            jsaa.BodyDesignation = GetBodyDesignationSAASignalsFound(jsaa, hl[startindex].System.Name);
            return ProcessSAASignalsFound(jsaa, hl[startindex].System, true);         // no relationship, add..
        }

        private bool ProcessSAASignalsFound(JournalSAASignalsFound jsaa, ISystem sys, bool reprocessPrimary = false)  // background or foreground.. FALSE if you can't process it
        {
            SystemNode sn = GetOrCreateSystemNode(sys);
            ScanNode relatednode = null;

            if (sn.NodesByID.ContainsKey((int)jsaa.BodyID)) // find by ID
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
                bool ringname = jsaa.BodyName.EndsWith("A Ring") || jsaa.BodyName.EndsWith("B Ring") || jsaa.BodyName.EndsWith("C Ring") | jsaa.BodyName.EndsWith("D Ring");
                string ringcutname = ringname ? jsaa.BodyName.Left(jsaa.BodyName.Length - 6).TrimEnd() : null;

                foreach (var body in sn.Bodies)
                {
                    if ((body.fullname == jsaa.BodyName || body.customname == jsaa.BodyName) &&
                        (body.fullname != sys.Name || body.level != 0))
                    {
                        relatednode = body;
                        break;
                    }
                    else if (ringcutname != null && body.fullname.Equals(ringcutname))
                    {
                        relatednode = body;
                        break;
                    }
                }
            }

            if (relatednode != null )
            {
                //System.Diagnostics.Debug.WriteLine("Setting SAA Signals Found for " + jsaa.BodyName + " @ " + sys.Name + " body "  + jsaa.BodyDesignation);

                if (relatednode.Signals == null)
                    relatednode.Signals = new List<JournalSAASignalsFound.SAASignal>();

                relatednode.Signals.AddRange(jsaa.Signals); // add signals to list of signals of this entity

                return true; // all ok
            }

            return false;
        }

        private string GetBodyDesignationSAASignalsFound(JournalSAASignalsFound je, string system)
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
