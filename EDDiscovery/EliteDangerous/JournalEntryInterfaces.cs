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
using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
{
    public interface IMaterialCommodityJournalEntry
    {
        void MaterialList(MaterialCommoditiesList mc, SQLiteConnectionUser conn);
    }

    public interface ILedgerJournalEntry
    {
        void Ledger(Ledger mcl, SQLiteConnectionUser conn);
    }

    public interface ILedgerNoCashJournalEntry
    {
        void LedgerNC(Ledger mcl, SQLiteConnectionUser conn);
    }

    public interface IShipInformation
    {
        void ShipInformation(ShipInformationList shp, SQLiteConnectionUser conn);
    }

    public interface IMissions
    {
        void UpdateMissions(MissionListAccumulator mlist, ISystem sys, string body, SQLiteConnectionUser conn);
    }
}
