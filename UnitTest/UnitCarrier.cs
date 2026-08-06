using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public static class UnitTestCarrier
    {
        [BaseUtils.UnitTests.Test]
        public static void TestCarrier()
        {
            CheckSection("Carrier");
            {
                string t1 = @"{ ""timestamp"":""2022-12-06T22:41:40Z"", ""event"":""CarrierCrewServices"", ""CarrierID"":3709149696, ""CrewRole"":""PioneerSupplies"", ""Operation"":""Pause"", ""CrewName"":""Mia Leach"" }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalCarrierCrewServices;
                Check(je != null);
                Check(je.CrewRole == CarrierDefinitions.ServiceType.PioneerSupplies);
                Check(je.Operation == CarrierDefinitions.ServiceOperationType.Pause);
            }
            {
                string t1 = @" { ""timestamp"":""2022-09-28T13:08:39Z"", ""event"":""CarrierModulePack"", ""CarrierID"":3709149696, ""Operation"":""BuyPack"", ""PackTheme"":""ExplosiveWeaponry"", ""PackTier"":1, ""Cost"":15933475 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalCarrierModulePack;
                Check(je != null);
                Check(je.Operation == CarrierDefinitions.ModulePackOperationType.BuyPack);
                Check(je.FriendlyOperation == "Buy Pack");
            }
            {
                string t1 = @"{ ""timestamp"":""2020-10-30T07:59:32Z"", ""event"":""CarrierModulePack"", ""CarrierID"":3700945408, ""Operation"":""SellPack"", ""PackTheme"":""Mining Tools"", ""PackTier"":2, ""Refund"":5395563 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalCarrierModulePack;
                Check(je != null);
                Check(je.Operation == CarrierDefinitions.ModulePackOperationType.SellPack);
                Check(je.PackTheme == "Mining Tools");
            }
            {
                string t1 = @"{ ""timestamp"":""2021-03-14T04:20:54Z"", ""event"":""CarrierShipPack"", ""CarrierID"":3703900160, ""Operation"":""SellPack"", ""PackTheme"":""Zorgon Peterson - Cargo"", ""PackTier"":1, ""Refund"":1627160 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalCarrierShipPack;
                Check(je != null);
                Check(je.Operation == CarrierDefinitions.ShipPackOperationType.SellPack);
                Check(je.PackTier == 1);
                Check(je.Refund == 1627160);
            }

        }
    }
}
