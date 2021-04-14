/*
 * Copyright © 2019 EDDiscovery development team
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
using System.Collections.Generic;

// class helps to decide row by row if visible and adds to outlining.
public class Outlining
{
    private int maingroup;
    private int maingroupcount;
    private int scanstartline;

    // we accumulate a set of outlines then add at end
    List<ExtendedControls.ExtPanelDataGridViewScrollOutlining.Outline> outlines = new List<ExtendedControls.ExtPanelDataGridViewScrollOutlining.Outline>();

    public Outlining()
    {
        maingroup = 0;
        maingroupcount = 0;
        scanstartline = -1;
    }

    public bool Process(HistoryEntry he, int rowindex, bool scanrollup, int rollupolder)
    {
        bool fsstype = he.EntryType == JournalTypeEnum.CodexEntry || he.EntryType == JournalTypeEnum.Scan || he.EntryType == JournalTypeEnum.FSSDiscoveryScan
                        || he.EntryType == JournalTypeEnum.FSSAllBodiesFound || he.EntryType == JournalTypeEnum.FSSSignalDiscovered ||
                        he.EntryType == JournalTypeEnum.SAAScanComplete || he.EntryType == JournalTypeEnum.FuelScoop;

        bool maingroupvisible = rollupolder == 0 || maingroupcount < rollupolder;       // first calc if main group is rolled up

        bool linevisible = maingroupvisible && scanstartline == -1;     // visible if not rolled up and not in a scan

        if (he.EntryType == JournalTypeEnum.Fileheader)                 // main group boundary
        {
            outlines.Add(new ExtendedControls.ExtPanelDataGridViewScrollOutlining.Outline() { start = maingroup, end = rowindex, expanded = maingroupvisible });

            //System.Diagnostics.Debug.WriteLine("Fileheader Roll up" + maingroup + "-" + rowindex + " rolled " + maingroupvisible);
            maingroup = rowindex + 1;
            maingroupcount++;
            maingroupvisible = linevisible = true; // override to show this line - maingroup as well in case co-indident with scan end
        }
        else if (fsstype)
        {
            if (scanstartline == -1 && scanrollup)
            {
                scanstartline = rowindex;
                linevisible = false;        // first entry of scan is not visible
            }
        }

        if (scanrollup && scanstartline != -1 && !fsstype)        // scan roll up in operation but not scan
        {
            outlines.Add(new ExtendedControls.ExtPanelDataGridViewScrollOutlining.Outline() { start = scanstartline, end = rowindex, expanded = false });
            linevisible = maingroupvisible;                         // visible if main group was visible
            scanstartline = -1;
        }

        return linevisible;
    }

    public void Finished(int rowindex, int rollupolder, ExtendedControls.ExtPanelDataGridViewScrollOutlining panel)
    {
        if (rowindex >= maingroup)
        {
            if (maingroupcount > 0)
            {
                bool maingroupvisible = rollupolder == 0 || maingroupcount < rollupolder;
                outlines.Add(new ExtendedControls.ExtPanelDataGridViewScrollOutlining.Outline() { start = maingroup, end = rowindex, expanded = maingroupvisible });
            }
        }

        panel.Add(outlines);

    }

}
