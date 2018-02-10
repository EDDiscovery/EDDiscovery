/*
 * Copyright © 2017 EDDiscovery development team
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
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public interface IDiscoveryController
    {
        #region Properties
        HistoryList history { get; }
        string LogText { get; }
        bool PendingClose { get; }
        GalacticMapping galacticMapping { get; }
        #endregion

        #region Events
        event Action<HistoryList> OnHistoryChange;
        event Action<HistoryEntry, HistoryList> OnNewEntry;
        event Action<EliteDangerousCore.JournalEntry> OnNewJournalEntry;
        event Action<string, Color> OnNewLogEntry;
        #endregion

        #region Logging
        void LogLine(string text);
        void LogLineHighlight(string text);
        void LogLineSuccess(string text);
        void LogLineColor(string text, Color color);
        void ReportProgress(int percent, string message);
        #endregion

        #region History
        bool RefreshHistoryAsync(string netlogpath = null, bool forcenetlogreload = false, bool forcejournalreload = false, int? currentcmdr = null);
        void RefreshDisplays();
        void RecalculateHistoryDBs();
        #endregion
    }
}
