/*
 * Copyright © 2017-2019 EDDiscovery development team
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

using EliteDangerousCore.DB;

namespace EliteDangerousCore
{
    public interface IMaterialJournalEntry
    {
        void UpdateMaterials(MaterialCommoditiesList mc, IUserDatabase userdb);
    }

    public interface ICommodityJournalEntry
    {
        void UpdateCommodities(MaterialCommoditiesList mc, IUserDatabase userdb);
    }

    public interface ILedgerJournalEntry
    {
        void Ledger(Ledger mcl, IUserDatabase userdb);
    }

    public interface ILedgerNoCashJournalEntry
    {
        void LedgerNC(Ledger mcl, IUserDatabase userdb);
    }

    public interface IShipInformation
    {
        void ShipInformation(ShipInformationList shp, string whereami, ISystem system, IUserDatabase userdb);
    }

    public interface IBodyNameAndID
    {
        string Body { get; }
        string BodyType { get; }
        int? BodyID { get; }
        string BodyDesignation { get; set; }
        string StarSystem { get; }
        long? SystemAddress { get; }
    }

    public interface IMissions
    {
        void UpdateMissions(MissionListAccumulator mlist, ISystem sys, string body, IUserDatabase conn);
    }

    public interface ISystemStationEntry
    {
        bool IsTrainingEvent { get; }
    }

    public interface IAdditionalFiles
    {
        bool ReadAdditionalFiles(string directory, bool inhistoryparse, ref Newtonsoft.Json.Linq.JObject jo);     // true if your happy, you can replace jo..
    }

}
