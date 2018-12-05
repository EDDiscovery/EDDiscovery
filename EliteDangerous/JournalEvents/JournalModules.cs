/*
 * Copyright © 2016-2018 EDDiscovery development team
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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [System.Diagnostics.DebuggerDisplay("{ShipId} {Ship} {ShipModules.Count}")]
    [JournalEntryType(JournalTypeEnum.Loadout)]
    public class JournalLoadout : JournalEntry, IShipInformation
    {
        public JournalLoadout(JObject evt) : base(evt, JournalTypeEnum.Loadout)
        {
            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["ShipID"].Int();
            ShipName = evt["ShipName"].Str();
            ShipIdent = evt["ShipIdent"].Str();
            HullValue = evt["HullValue"].LongNull();
            HullHealth = evt["HullHealth"].DoubleNull();
            if (HullHealth != null)
                HullHealth *= 100.0;        // convert to 0-100
            ModulesValue = evt["ModulesValue"].LongNull();
            Rebuy = evt["Rebuy"].LongNull();
            Hot = evt["Hot"].BoolNull();    // 3.3

            ShipModules = new List<ShipModule>();

            JArray jmodules = (JArray)evt["Modules"];
            if (jmodules != null)       // paranoia
            {
                foreach (JObject jo in jmodules)
                {
                    ShipModule.EngineeringData engineering = null;

                    JObject jeng = (JObject)jo["Engineering"];
                    if (jeng != null)
                    {
                        engineering = new ShipModule.EngineeringData(jeng);
                    }

                    string slotfdname = JournalFieldNaming.NormaliseFDSlotName(jo["Slot"].Str());
                    string itemfdname = JournalFieldNaming.NormaliseFDItemName(jo["Item"].Str());

                    ShipModule module = new ShipModule(JournalFieldNaming.GetBetterSlotName(slotfdname),
                                                        slotfdname,
                                                        JournalFieldNaming.GetBetterItemName(itemfdname),
                                                        itemfdname,
                                                        jo["On"].BoolNull(),
                                                        jo["Priority"].IntNull(),
                                                        jo["AmmoInClip"].IntNull(),
                                                        jo["AmmoInHopper"].IntNull(),
                                                        jo["Health"].DoubleNull(),
                                                        jo["Value"].IntNull(),
                                                        null,  //power not received here
                                                        engineering);
                    ShipModules.Add(module);
                }

                ShipModules = ShipModules.OrderBy(x => x.Slot).ToList();            // sort for presentation..
            }
        }

        public string Ship { get; set; }        // type, pretty name fer-de-lance
        public string ShipFD { get; set; }        // type,  fdname
        public int ShipId { get; set; }
        public string ShipName { get; set; } // : user-defined ship name
        public string ShipIdent { get; set; } //   user-defined ship ID string
        public long? HullValue { get; set; }   //3.0
        public double? HullHealth { get; set; }   //3.3, 1.0-0.0, multipled by 100.0
        public long? ModulesValue { get; set; }   //3.0
        public long? Rebuy { get; set; }   //3.0
        public bool? Hot { get; set; }   //3.3

        public List<ShipModule> ShipModules;

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.Loadout(ShipId, Ship, ShipFD, ShipName, ShipIdent, ShipModules, HullValue ?? 0, ModulesValue ?? 0, Rebuy ?? 0, Hot);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Ship:".Txb(this), Ship, "Name:".Txb(this), ShipName, "Ident:".Txb(this), ShipIdent, ";Hot", Hot,
                "Modules:".Tx(this), ShipModules.Count, "Hull Health;%;N1".Txb(this), HullHealth, "Hull:; cr;N0".Txb(this), HullValue, "Modules:; cr;N0".Txb(this), ModulesValue, "Rebuy:; cr;N0".Txb(this), Rebuy);
            detailed = "";

            foreach (ShipModule m in ShipModules)
            {
                if (detailed.Length > 0)
                    detailed += Environment.NewLine;

                detailed += BaseUtils.FieldBuilder.Build("", m.Slot, "<:", m.Item, "", m.PE(), "Blueprint:".Txb(this), m.Engineering?.FriendlyBlueprintName, "<+", m.Engineering?.ExperimentalEffect_Localised, "< ", m.Engineering?.Engineer);
            }
        }
    }


    [JournalEntryType(JournalTypeEnum.ModuleBuy)]
    public class JournalModuleBuy : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalModuleBuy(JObject evt ) : base(evt, JournalTypeEnum.ModuleBuy)
        {
            SlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["Slot"].Str());
            Slot = JournalFieldNaming.GetBetterSlotName(SlotFD);

            BuyItemFD = JournalFieldNaming.NormaliseFDItemName(evt["BuyItem"].Str());
            BuyItem = JournalFieldNaming.GetBetterItemName(BuyItemFD);
            BuyItemLocalised = JournalFieldNaming.CheckLocalisation(evt["BuyItem_Localised"].Str(),BuyItem);
            BuyPrice = evt["BuyPrice"].Long();

            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["ShipID"].Int();

            SellItemFD = JournalFieldNaming.NormaliseFDItemName(evt["SellItem"].Str());
            SellItem = JournalFieldNaming.GetBetterItemName(SellItemFD);
            SellItemLocalised = JournalFieldNaming.CheckLocalisation(evt["SellItem_Localised"].Str(),SellItem);
            SellPrice = evt["SellPrice"].LongNull();

            StoredItemFD = JournalFieldNaming.NormaliseFDItemName(evt["StoredItem"].Str());
            StoredItem = JournalFieldNaming.GetBetterItemName(StoredItemFD);
            StoredItemLocalised = JournalFieldNaming.CheckLocalisation(evt["StoredItem_Localised"].Str(),StoredItem);

            MarketID = evt["MarketID"].LongNull();
        }

        public string Slot { get; set; }
        public string SlotFD { get; set; }

        public string Ship { get; set; }
        public string ShipFD { get; set; }
        public int ShipId { get; set; }

        public string BuyItem { get; set; }
        public string BuyItemFD { get; set; }
        public string BuyItemLocalised { get; set; }
        public long BuyPrice { get; set; }

        public string SellItem { get; set; }                    // if sold previous one
        public string SellItemFD { get; set; }                    // if sold previous one
        public string SellItemLocalised { get; set; }
        public long? SellPrice { get; set; }

        public string StoredItem { get; set; }                  // if stored previous one
        public string StoredItemFD { get; set; }                  // if stored previous one
        public string StoredItemLocalised { get; set; }         // if stored previous one

        public long? MarketID { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (BuyItemLocalised.Length > 0) ? BuyItemLocalised : BuyItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s + " on " + Ship, -BuyPrice + ( SellPrice??0) );
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleBuy(this);
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            
            info = BaseUtils.FieldBuilder.Build("", BuyItemLocalised, "< into ".Txb(this), Slot, "Cost:; cr;N0".Txb(this), BuyPrice);
            if (SellItem.Length > 0)
                info += ", " + BaseUtils.FieldBuilder.Build("Sold:".Txb(this), SellItemLocalised, "Price:; cr;N0".Txb(this), SellPrice);
            if (StoredItem.Length > 0)
                info += ", " + BaseUtils.FieldBuilder.Build("Stored:".Txb(this), StoredItemLocalised);

            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.ModuleSell)]
    public class JournalModuleSell : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalModuleSell(JObject evt) : base(evt, JournalTypeEnum.ModuleSell)
        {
            SlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["Slot"].Str());
            Slot = JournalFieldNaming.GetBetterSlotName(SlotFD);

            SellItemFD = JournalFieldNaming.NormaliseFDItemName(evt["SellItem"].Str());
            SellItem = JournalFieldNaming.GetBetterItemName(SellItemFD);
            SellItemLocalised = JournalFieldNaming.CheckLocalisation(evt["SellItem_Localised"].Str(), SellItem);

            SellPrice = evt["SellPrice"].Long();

            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["ShipID"].Int();

            MarketID = evt["MarketID"].LongNull();
        }

        public string Slot { get; set; }
        public string SlotFD { get; set; }
        public string SellItem { get; set; }
        public string SellItemFD { get; set; }
        public string SellItemLocalised { get; set; }
        public long SellPrice { get; set; }
        public string Ship { get; set; }
        public string ShipFD { get; set; }
        public int ShipId { get; set; }
        public long? MarketID { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (SellItemLocalised.Length > 0) ? SellItemLocalised : SellItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s + " on " + Ship, SellPrice);
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleSell(this);
        }
        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("", SellItemLocalised, "< from ".Txb(this), Slot, "Price:; cr;N0".Txb(this), SellPrice);
            detailed = "";
        }

    }

    [JournalEntryType(JournalTypeEnum.ModuleSellRemote)]
    public class JournalModuleSellRemote : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalModuleSellRemote(JObject evt) : base(evt, JournalTypeEnum.ModuleSellRemote)
        {
            Slot = evt["StorageSlot"].Str();         // this is NOT a ship slot name, just a index

            SellItemFD = JournalFieldNaming.NormaliseFDItemName(evt["SellItem"].Str());
            SellItem = JournalFieldNaming.GetBetterItemName(SellItemFD);
            SellItemLocalised = JournalFieldNaming.CheckLocalisation(evt["SellItem_Localised"].Str(), SellItem);

            SellPrice = evt["SellPrice"].Long();

            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["ShipID"].Int();

            ServerId = evt["ServerId"].Int();
        }

        public string Slot { get; set; }
        public string SellItem { get; set; }
        public string SellItemFD { get; set; }
        public string SellItemLocalised { get; set; }
        public long SellPrice { get; set; }
        public string ShipFD { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public int ServerId { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (SellItemLocalised.Length > 0) ? SellItemLocalised : SellItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, SellItemLocalised + " on " + Ship, SellPrice);
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleSellRemote(this);
        }

        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("Item:".Txb(this), SellItemLocalised, "Price:; cr;N0".Txb(this), SellPrice);
            detailed = BaseUtils.FieldBuilder.Build("Ship:".Txb(this), Ship);
        }
    }

    [JournalEntryType(JournalTypeEnum.ModuleRetrieve)]
    public class JournalModuleRetrieve : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalModuleRetrieve(JObject evt) : base(evt, JournalTypeEnum.ModuleRetrieve)
        {
            SlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["Slot"].Str());
            Slot = JournalFieldNaming.GetBetterSlotName(SlotFD);

            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["ShipID"].Int();

            RetrievedItemFD = JournalFieldNaming.NormaliseFDItemName(evt["RetrievedItem"].Str());
            RetrievedItem = JournalFieldNaming.GetBetterItemName(RetrievedItemFD);
            RetrievedItemLocalised = JournalFieldNaming.CheckLocalisation(evt["RetrievedItem_Localised"].Str(), RetrievedItem);

            EngineerModifications = evt["EngineerModifications"].Str().SplitCapsWordFull();

            SwapOutItemFD = JournalFieldNaming.NormaliseFDItemName(evt["SwapOutItem"].Str());
            SwapOutItem = JournalFieldNaming.GetBetterItemName(SwapOutItemFD);
            SwapOutItemLocalised = JournalFieldNaming.CheckLocalisation(evt["SwapOutItem_Localised"].Str(), SwapOutItem);

            Cost = evt["Cost"].Long();

            Hot = evt["Hot"].BoolNull();
            Level = evt["Level"].IntNull();
            Quality = evt["Quality"].DoubleNull();

            MarketID = evt["MarketID"].LongNull();
        }

        public string Slot { get; set; }
        public string SlotFD { get; set; }
        public string Ship { get; set; }
        public string ShipFD { get; set; }
        public int ShipId { get; set; }
        public string RetrievedItem { get; set; }
        public string RetrievedItemFD { get; set; }
        public string RetrievedItemLocalised { get; set; }
        public string EngineerModifications { get; set; }
        public string SwapOutItem { get; set; }
        public string SwapOutItemFD { get; set; }
        public string SwapOutItemLocalised { get; set; }
        public long Cost { get; set; }
        public double? Quality { get; set; }
        public int? Level { get; set; }
        public bool? Hot { get; set; }
        public long? MarketID { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (RetrievedItemLocalised.Length > 0) ? RetrievedItemLocalised : RetrievedItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s + " on " + Ship, -Cost);
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleRetrieve(this);
        }

        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("", RetrievedItemLocalised, "< into ".Txb(this), Slot, ";(Hot)".Txb(this), Hot);
            if (Cost > 0)
                info += " " + BaseUtils.FieldBuilder.Build("Cost:; cr;N0".Txb(this), Cost);

            if (SwapOutItem.Length > 0)
                info += ", " + BaseUtils.FieldBuilder.Build("Stored:".Txb(this), SwapOutItemLocalised);
            detailed = "";
        }

    }



    [JournalEntryType(JournalTypeEnum.ModuleStore)]
    public class JournalModuleStore : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalModuleStore(JObject evt) : base(evt, JournalTypeEnum.ModuleStore)
        {
            SlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["Slot"].Str());
            Slot = JournalFieldNaming.GetBetterSlotName(SlotFD);

            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["ShipID"].Int();

            StoredItemFD = JournalFieldNaming.NormaliseFDItemName(evt["StoredItem"].Str());
            StoredItem = JournalFieldNaming.GetBetterItemName(StoredItemFD);
            StoredItemLocalised = JournalFieldNaming.CheckLocalisation(evt["StoredItem_Localised"].Str(), StoredItem);

            EngineerModifications = evt["EngineerModifications"].StrNull().SplitCapsWordFull();

            ReplacementItemFD = JournalFieldNaming.NormaliseFDItemName(evt["ReplacementItem"].Str());
            ReplacementItem = JournalFieldNaming.GetBetterItemName(ReplacementItemFD);
            ReplacementItemLocalised = JournalFieldNaming.CheckLocalisation(evt["ReplacementItem_Localised"].Str(), ReplacementItem);

            Cost = evt["Cost"].LongNull();

            Hot = evt["Hot"].BoolNull();
            Level = evt["Level"].IntNull();
            Quality = evt["Quality"].DoubleNull();

            MarketID = evt["MarketID"].LongNull();
        }

        public string Slot { get; set; }
        public string SlotFD { get; set; }
        public string Ship { get; set; }
        public string ShipFD { get; set; }
        public int ShipId { get; set; }
        public string StoredItem { get; set; }
        public string StoredItemFD { get; set; }
        public string StoredItemLocalised { get; set; }
        public string EngineerModifications { get; set; }
        public string ReplacementItem { get; set; }
        public string ReplacementItemFD { get; set; }
        public string ReplacementItemLocalised { get; set; }
        public long? Cost { get; set; }
        public double? Quality { get; set; }
        public int? Level { get; set; }
        public bool? Hot { get; set; }
        public long? MarketID { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (StoredItemLocalised.Length > 0) ? StoredItemLocalised : StoredItem;
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s + " on ".Txb(this) + Ship, -Cost);
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleStore(this);
        }

        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("", StoredItemLocalised, "< from ".Txb(this), Slot, ";(Hot)".Txb(this), Hot, "Cost:; cr;N0".Txb(this), Cost);
            if (ReplacementItem.Length > 0)
                info = ", " + BaseUtils.FieldBuilder.Build("Replaced by:".Txb(this), ReplacementItemLocalised);
            detailed = BaseUtils.FieldBuilder.Build("Modifications:".Txb(this), EngineerModifications);
        }
    }


    [JournalEntryType(JournalTypeEnum.ModuleSwap)]
    public class JournalModuleSwap : JournalEntry, IShipInformation
    {
        public JournalModuleSwap(JObject evt) : base(evt, JournalTypeEnum.ModuleSwap)
        {
            FromSlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["FromSlot"].Str());
            FromSlot = JournalFieldNaming.GetBetterSlotName(FromSlotFD);

            ToSlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["ToSlot"].Str());
            ToSlot = JournalFieldNaming.GetBetterSlotName(ToSlotFD);

            FromItemFD = JournalFieldNaming.NormaliseFDItemName(evt["FromItem"].Str());
            FromItem = JournalFieldNaming.GetBetterItemName(FromItemFD);
            FromItemLocalised = JournalFieldNaming.CheckLocalisation(evt["FromItem_Localised"].Str(), FromItem);

            ToItemFD = JournalFieldNaming.NormaliseFDItemName(evt["ToItem"].Str());
            ToItem = JournalFieldNaming.GetBetterItemName(ToItemFD);
            if (ToItem.Equals("Null"))      // Frontier bug.. something Null is here.. remove
                ToItem = ToItemFD = "";
            ToItemLocalised = JournalFieldNaming.CheckLocalisation(evt["ToItem_Localised"].Str(), ToItem);        // if ToItem is null or not there, this won't be

            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["ShipID"].Int();

            MarketID = evt["MarketID"].LongNull();
        }

        public string FromSlot { get; set; }
        public string FromSlotFD { get; set; }
        public string ToSlot { get; set; }
        public string ToSlotFD { get; set; }
        public string FromItem { get; set; }
        public string FromItemFD { get; set; }
        public string FromItemLocalised { get; set; }
        public string ToItem { get; set; }
        public string ToItemFD { get; set; }
        public string ToItemLocalised { get; set; }
        public string Ship { get; set; }
        public string ShipFD { get; set; }
        public int ShipId { get; set; }
        public long? MarketID { get; set; }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleSwap(this);
        }

        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("Slot:".Txb(this), FromSlot, "< to ".Txb(this), ToSlot, "Item:".Txb(this), FromItemLocalised);
            if (ToItem.Length > 0)
                info += ", Swapped with ".Txb(this) + ToItemLocalised;
            detailed = "";
        }
    }



    [System.Diagnostics.DebuggerDisplay("{ShipId} {Ship} {ShipModules.Count}")]
    [JournalEntryType(JournalTypeEnum.ModuleInfo)]
    public class JournalModuleInfo : JournalEntry, IAdditionalFiles
    {
        public JournalModuleInfo(JObject evt) : base(evt, JournalTypeEnum.ModuleInfo)
        {
            Rescan(evt);
        }

        public void Rescan(JObject evt)
        {
            ShipModules = new List<ShipModule>();

            JArray jmodules = (JArray)evt["Modules"];
            if (jmodules != null)
            {
                foreach (JObject jo in jmodules)
                {
                    string slotfdname = JournalFieldNaming.NormaliseFDSlotName(jo["Slot"].Str());
                    string itemfdname = JournalFieldNaming.NormaliseFDItemName(jo["Item"].Str());

                    ShipModule module = new ShipModule(
                                                        JournalFieldNaming.GetBetterSlotName(slotfdname),
                                                        slotfdname,
                                                        JournalFieldNaming.GetBetterItemName(itemfdname),
                                                        itemfdname,
                                                        null, // unknown
                                                        jo["Priority"].IntNull(),
                                                        null, // aclip
                                                        null, // ahooper
                                                        null, // health
                                                        null, // Value
                                                        jo["Power"].DoubleNull(),
                                                        null //engineering
                                                        );
                    ShipModules.Add(module);
                }

                ShipModules = ShipModules.OrderBy(x => x.Slot).ToList();            // sort for presentation..
            }
        }

        public bool ReadAdditionalFiles(string directory, bool historyrefreshparse, ref JObject jo)
        {
            JObject jnew = ReadAdditionalFile(System.IO.Path.Combine(directory, "ModulesInfo.json"), waitforfile: !historyrefreshparse, checktimestamptype: true);
            if (jnew != null)        // new json, rescan
            {
                jo = jnew;      // replace current
                Rescan(jo);
            }
            return jnew != null;
        }

        public List<ShipModule> ShipModules;

        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("Modules:".Tx(this), ShipModules.Count);
            detailed = "";

            foreach (ShipModule m in ShipModules)
            {
                double? power = (m.Power.HasValue && m.Power.Value > 0) ? m.Power : null;

                detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build("", m.Slot, "<:", m.Item, "; MW;0.###", power), Environment.NewLine);
            }
        }
    }


    [JournalEntryType(JournalTypeEnum.StoredModules)]
    public class JournalStoredModules : JournalEntry, IShipInformation
    {
        public JournalStoredModules(JObject evt) : base(evt, JournalTypeEnum.StoredModules)
        {
            StationName = evt["StationName"].Str();
            StarSystem = evt["StarSystem"].Str();
            MarketID = evt["MarketID"].LongNull();

            ModuleItems = evt["Items"]?.ToObjectProtected<ModulesInStore.StoredModule[]>();

            if (ModuleItems != null)
            {
                foreach (ModulesInStore.StoredModule i in ModuleItems)
                    i.Normalise();
            }
        }

        public string StationName { get; set; }
        public string StarSystem { get; set; }
        public long? MarketID { get; set; }

        public ModulesInStore.StoredModule[] ModuleItems { get; set; }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.UpdateStoredModules(this);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Total:".Txb(this), ModuleItems?.Count());
            detailed = "";

            if (ModuleItems != null)
            {
                foreach (ModulesInStore.StoredModule m in ModuleItems)
                {
                    detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build("", m.Name, "< at ".Tx(this), m.StarSystem,
                                "Transfer Cost:; cr;N0".Txb(this), m.TransferCost,
                                "Time:".Txb(this), m.TransferTimeString,
                                "Value:; cr;N0".Txb(this), m.TransferCost, ";(Hot)".Txb(this), m.Hot), System.Environment.NewLine);
                }
            }
        }
    }


    [JournalEntryType(JournalTypeEnum.MassModuleStore)]
    public class JournalMassModuleStore : JournalEntry, IShipInformation
    {
        public JournalMassModuleStore(JObject evt) : base(evt, JournalTypeEnum.MassModuleStore)
        {
            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["ShipID"].Int();
            ModuleItems = evt["Items"]?.ToObjectProtected<ModuleItem[]>();
            MarketID = evt["MarketID"].LongNull();

            if (ModuleItems != null)
            {
                foreach (ModuleItem i in ModuleItems)
                {
                    i.SlotFD = JournalFieldNaming.NormaliseFDSlotName(i.Slot);
                    i.Slot = JournalFieldNaming.GetBetterSlotName(i.SlotFD);
                    i.NameFD = JournalFieldNaming.NormaliseFDItemName(i.Name);
                    i.Name = JournalFieldNaming.GetBetterItemName(i.NameFD);
                }
            }
        }

        public string Ship { get; set; }
        public string ShipFD { get; set; }
        public int ShipId { get; set; }
        public long? MarketID { get; set; }

        public ModuleItem[] ModuleItems { get; set; }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.MassModuleStore(this);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Total modules:".Txb(this), ModuleItems?.Count());
            detailed = "";

            if (ModuleItems != null)
                foreach (ModuleItem m in ModuleItems)
                {
                    detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build("", m.Name, ";(Hot)".Txb(this), m.Hot), ", ");
                }
        }

        public class ModuleItem
        {
            public string SlotFD;
            public string Slot;
            public string NameFD;
            public string Name;
            public string EngineerModifications;
            public double? Quality { get; set; }
            public int? Level { get; set; }
            public bool? Hot { get; set; }
        }
    }

    [JournalEntryType(JournalTypeEnum.FetchRemoteModule)]
    public class JournalFetchRemoteModule : JournalEntry, ILedgerJournalEntry
    {
        public JournalFetchRemoteModule(JObject evt) : base(evt, JournalTypeEnum.FetchRemoteModule)
        {
            StorageSlot = evt["StorageSlot"].Str();          // Slot number, not a slot on our ship

            StoredItemFD = JournalFieldNaming.NormaliseFDItemName(evt["StoredItem"].Str());
            StoredItem = JournalFieldNaming.GetBetterItemName(StoredItemFD);
            StoredItemLocalised = JournalFieldNaming.CheckLocalisation(evt["StoredItem_Localised"].Str(), StoredItem);

            TransferCost = evt["TransferCost"].Long();

            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["ShipID"].Int();

            ServerId = evt["ServerId"].Int();
            nTransferTime = evt["TransferTime"].IntNull();
            FriendlyTransferTime = nTransferTime.HasValue ? nTransferTime.Value.SecondsToString() : "";
        }

        public string StorageSlot { get; set; }
        public string StoredItem { get; set; }
        public string StoredItemFD { get; set; }
        public string StoredItemLocalised { get; set; }
        public long TransferCost { get; set; }
        public string ShipFD { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public int ServerId { get; set; }
        public int? nTransferTime { get; set; }
        public string FriendlyTransferTime { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, StoredItemLocalised + " on " + Ship, -TransferCost);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", StoredItemLocalised, "Cost:; cr;N0".Txb(this), TransferCost, "Into ship:".Txb(this), Ship, "Transfer Time:".Txb(this), FriendlyTransferTime);
            detailed = "";
        }
    }


}
