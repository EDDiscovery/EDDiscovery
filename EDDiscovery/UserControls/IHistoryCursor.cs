/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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

namespace EDDiscovery.UserControls
{
    // Any UCs wanting to be a cursor, must implement this interface

    public delegate void ChangedSelectionHEHandler(HistoryEntry he, HistoryList hl, bool selectedEntry);

    public interface IHistoryCursor
    {
        event ChangedSelectionHEHandler OnTravelSelectionChanged;   // as above, different format, for certain older controls
        void FireChangeSelection();                                 // fire a change sel event to everyone
        void GotoPosByJID(long jid);                                // goto a pos by JID
        HistoryEntry GetCurrentHistoryEntry { get; }                // whats your current entry, null if not
    }
}
