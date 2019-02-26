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

// class helps to decide row by row if visible and adds to outlining.
public class Outlining
{
    private int maingroup;
    private int maingroupcount;
    private int scanstartline;
    ExtendedControls.ExtPanelDataGridViewScrollOutlining panel;

    public Outlining(ExtendedControls.ExtPanelDataGridViewScrollOutlining panel)
    {
        this.panel = panel;
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
            panel.Add(maingroup, rowindex, maingroupvisible, true);        // add it in, no update
                                                                           //System.Diagnostics.Debug.WriteLine("Main Roll up" + maingroup + "-" + rowindex + " rolled " + maingroupvisible);
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
            panel.Add(scanstartline, rowindex, false, true);         // scans are rolled up
            linevisible = maingroupvisible;                         // visible if main group was visible
            scanstartline = -1;
        }

        return linevisible;
    }

    public void ProcesslastLine(int rowindex, int rollupolder)
    {
        if (rowindex >= maingroup)
        {
            if (maingroupcount > 0)
            {
                bool maingroupvisible = rollupolder == 0 || maingroupcount < rollupolder;
                panel.Add(maingroup, rowindex, maingroupvisible, true);        // add a terminating group

            }
        }
    }

}
