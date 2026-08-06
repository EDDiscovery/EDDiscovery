using BaseUtils.UnitTests;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System.IO;
using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public static class UnitTestVarious
    {
        [BaseUtils.UnitTests.Test]
        public static void TestVarious()
        {
            CheckSection("Various");

              {
                string t1 = @"{ ""timestamp"":""2025-07-25T15:21:41Z"", ""event"":""Docked"", ""StationName"":""Lewis Dock"", ""StationType"":""Orbis"", ""Taxi"":false, ""Multicrew"":false, ""StarSystem"":""LHS 3356"", ""SystemAddress"":2869709317553, ""MarketID"":3229061888, ""StationFaction"":{ ""Name"":""EG Union"", ""FactionState"":""War"" }, ""StationGovernment"":""$government_Dictatorship;"", ""StationGovernment_Localised"":""Dictatorship"", ""StationServices"":[ ""dock"", ""autodock"", ""commodities"", ""contacts"", ""exploration"", ""missions"", ""outfitting"", ""crewlounge"", ""rearm"", ""refuel"", ""repair"", ""shipyard"", ""tuning"", ""engineer"", ""missionsgenerated"", ""flightcontroller"", ""stationoperations"", ""powerplay"", ""searchrescue"", ""stationMenu"", ""shop"", ""livery"", ""socialspace"", ""bartender"", ""vistagenomics"", ""pioneersupplies"", ""apexinterstellar"", ""frontlinesolutions"", ""registeringcolonisation"" ], ""StationEconomy"":""$economy_Industrial;"", ""StationEconomy_Localised"":""Industrial"", ""StationEconomies"":[ { ""Name"":""$economy_Industrial;"", ""Name_Localised"":""Industrial"", ""Proportion"":1.000000 } ], ""DistFromStarLS"":30.016441, ""ActiveFine"":true, ""LandingPads"":{ ""Small"":17, ""Medium"":18, ""Large"":9 } }";
                JournalDocked je = JournalEntry.CreateJournalEntry(t1) as JournalDocked;
                Check( je != null);
                Check( je.LandingPads.Large == 9 );
            }
            {
                string t1 = @"{ ""timestamp"":""2020-04-20T13:06:59Z"", ""event"":""ProspectedAsteroid"", ""Materials"":[ { ""Name"":""bertrandite"", ""Proportion"":17.920820 }, { ""Name"":""Samarium"", ""Proportion"":11.915166 }, { ""Name"":""silver"", ""Proportion"":8.885116 } ], ""Content"":""$AsteroidMaterialContent_Medium;"", ""Content_Localised"":""Material Content: Medium"", ""MotherlodeMaterial"":""Carbon"", ""Remaining"":100.000000 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalProspectedAsteroid;
                Check(je != null);
                Check(je.Materials[0].Name.Equals("Bertrandite"));
                Check(je.Materials[0].Proportion == 17.920820);
                Check(je.Content == JournalProspectedAsteroid.AsteroidContent.Medium);
                Check(je.Remaining == 100);
                Check(je.MotherlodeMaterial.Equals("CaRBON"));
                Check(je.MotherlodeMaterial_Localised.Equals("Carbon"));
                Check(je.FriendlyMotherlodeMaterial.Equals("Carbon"));
            }

            {
                string t1 = @"{ ""timestamp"":""2020-04-20T13:06:59Z"", ""event"":""ProspectedAsteroid"", ""Materials"":[ { ""Name"":""bertrandite"", ""Proportion"":17.920820 }, { ""Name"":""Samarium"", ""Proportion"":11.915166 }, { ""Name"":""silver"", ""Proportion"":8.885116 } ], ""Content"":""$AsteroidMaterialContent_Medium;"", ""Content_Localised"":""Material Content: Medium"", ""Remaining"":100.000000 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalProspectedAsteroid;
                Check(je != null);
                Check(je.Materials[0].Name.Equals("Bertrandite"));
                Check(je.Materials[0].Proportion == 17.920820);
                Check(je.Content == JournalProspectedAsteroid.AsteroidContent.Medium);
                Check(je.Remaining == 100);
                Check(je.MotherlodeMaterial == null);
                Check(je.MotherlodeMaterial_Localised == null);
                Check(je.FriendlyMotherlodeMaterial == null);
            }
            {
                string t1 = @"{ ""timestamp"":""2020-04-20T13:06:59Z"", ""event"":""ProspectedAsteroid"", ""Materials"":[ { ""Name"":""bertrandite"", ""Proportion"":17.920820 }, { ""Name"":""Samarium"", ""Proportion"":11.915166 }, { ""Name"":""silver"", ""Proportion"":8.885116 } ], ""Content"":""$AsteroidMaterialContent_Medium;"", ""Content_Localised"":""Material Content: Medium"", ""Remaining"":100.000000 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalProspectedAsteroid;
                Check(je != null);
                Check(je.Materials[0].Name.Equals("Bertrandite"));
                Check(je.Materials[0].Proportion == 17.920820);
                Check(je.Content == JournalProspectedAsteroid.AsteroidContent.Medium);
                Check(je.Remaining == 100);
                Check(je.MotherlodeMaterial == null);
                Check(je.MotherlodeMaterial_Localised == null);
                Check(je.FriendlyMotherlodeMaterial == null);
            }

            {
                string t1 = @"{ ""timestamp"":""2024-09-21T09:09:25Z"", ""event"":""TechnologyBroker"", ""BrokerType"":""sirius""" +
                        @", ""MarketID"":128975207, ""ItemsUnlocked"":[ { ""Name"":""Hpt_HeatSinkLauncher_Turret_Tiny""" +
                        @", ""Name_Localised"":""Heatsink"" } ], ""Commodities"":[  ], ""Materials"":[ { ""Name"":""mechanicalscrap""" +
                        @", ""Name_Localised"":""Mechanical Scrap"", ""Count"":8, ""Category"":""Manufactured"" }" +
                        @", { ""Name"":""niobium"", ""Count"":6, ""Category"":""Raw"" }, { ""Name"":""vanadium""" +
                        @", ""Count"":6, ""Category"":""Raw"" }, { ""Name"":""mechanicalcomponents"", ""Name_Localised"":""Mechanical Components""" +
                        @", ""Count"":5, ""Category"":""Manufactured"" } ] }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalTechnologyBroker;
                Check(je != null);
                CheckThat(je.BrokerType).Is(JournalTechnologyBroker.BrokerTypes.Sirius);
                CheckThat(je.ItemsUnlocked[0].Name).Is("Hpt_HeatSinkLauncher_Turret_Tiny");
                CheckThat(je.ItemsUnlocked[0].Name_Localised).Is("Heatsink");
                CheckThat(je.MaterialList[0].Name).Is("MECHanicalscrap");
                CheckThat(je.MaterialList[0].Name_Localised).Is("Mechanical Scrap");
                CheckThat(je.MaterialList[0].Category).Is(MaterialCommodityMicroResourceType.CatType.Manufactured);
            }
            {
                string t1 = @"{ ""timestamp"":""2024-08-10T04:37:55Z"", ""event"":""ScientificResearch"", ""MarketID"":3221497856" +
@", ""Name"":""industrialfirmware"", ""Name_Localised"":""Cracked Industrial Firmware""" +
@", ""Category"":""Encoded"", ""Count"":198 }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalScientificResearch;
                Check(je != null);
                CheckThat(je.Name).Is("industrialfirmware");
                CheckThat(je.Name_Localised).Is("Cracked Industrial Firmware");
                CheckThat(je.Category).Is(MaterialCommodityMicroResourceType.CatType.Encoded);
                CheckThat(je.Count).Is(198);
            }
            {
                string t1 = @"{ ""timestamp"":""2016-09-20T16:57:27Z"", ""event"":""Repair"", ""Item"":""$int_engine_size8_class5_name;""" + @", ""Cost"":62 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalRepair;
                Check(je != null);
                CheckThat(je.ItemFD).Is("int_engine_size8_class5");
                CheckThat(je.Item).Is("Thrusters Class 8 Rating A");
                CheckThat(je.ItemLocalised).Is(je.Item);
                CheckThat(je.GetInfo()).Contains("Thrusters, Cost: 6");     // not doing the whole line due to localisation

                string t2 = @"{ ""timestamp"":""2025-05-24T16:56:36Z"", ""event"":""Repair"", ""Items"":[ ""$int_engine_size8_class5_name;""" +
@", ""Hull"", ""$int_dronecontrol_collection_size5_class5_name;"", ""$modularcargobaydoor_name;""" +
@", ""$int_refinery_size4_class5_name;"", ""$int_lifesupport_size7_class2_name;""" +
@", ""$int_powerdistributor_size7_class5_name;"", ""$int_multidronecontrol_universal_size7_class3_name;""" +
@", ""$cutter_cockpit_name;"", ""$int_sensors_size7_class2_name;"", ""$int_powerplant_size8_class5_name;""" +
@", ""$int_hyperdrive_overcharge_size7_class5_name;"", ""$int_dronecontrol_collection_size5_class5_name;""" +
@", ""$int_dronecontrol_prospector_size3_class5_name;"", ""$int_detailedsurfacescanner_tiny_name;"" ]" +
@", ""Cost"":10058 }";

               je = JournalEntry.CreateJournalEntry(t2) as JournalRepair;
               Check(je != null);
                CheckThat(je.ItemFD).Is("int_engine_size8_class5");
                CheckThat(je.Item).Is("Thrusters Class 8 Rating A");
                CheckThat(je.ItemLocalised).Is(je.Item);
                CheckThat(je.Items[0].ItemFD).Is("int_engine_size8_class5");
                CheckThat(je.Items[0].Item).Is("Thrusters Class 8 Rating A");
                CheckThat(je.Items[0].ItemLocalised).Is(je.Item);
                CheckThat(je.Items[2].ItemFD).Is("int_dronecontrol_collection_size5_class5");
                CheckThat(je.Items[2].Item).Is("Collector Limpet Controller Class 5 Rating A");
                CheckThat(je.Items[2].ItemLocalised).Is(je.Items[2].Item);
                CheckThat(je.GetInfo()).Contains("Repaired: 15, Cost: 10");     // not doing the whole line due to localisation
                CheckThat(je.GetDetailed()).Contains("Wear And Tear Type");
            }

            {
                string t1 = @"{ ""timestamp"":""2016-09-20T15:16:06Z"", ""event"":""Died"" }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalDied;
                Check(je != null);
                CheckThat(je.Killers).IsNull();

                string t2 = @"{ ""timestamp"":""2016-09-21T00:04:54Z"", ""event"":""Died"", ""KillerName"":""$ShipName_Police_Empire;""" +
@", ""KillerName_Localised"":""Internal Security Service"", ""KillerShip"":""empire_fighter""" +
@", ""KillerRank"":""Master"" }";
                je = JournalEntry.CreateJournalEntry(t2) as JournalDied;
                Check(je != null);

                string t3 = @"{ ""timestamp"":""2024-07-26T21:23:14Z"", ""event"":""Died"", ""KillerName"":""$ShipName_Military_Independent;""" +
@", ""KillerName_Localised"":""System Defence Force"", ""KillerShip"":""viper_mkiv""" +
@", ""KillerRank"":""Deadly"" }";
                je = JournalEntry.CreateJournalEntry(t3) as JournalDied;
                Check(je != null);

                string t4 = @"{ ""timestamp"":""2024-12-06T20:18:35Z"", ""event"":""Died"", ""KillerName"":""$UNKNOWN;""" +
@", ""KillerName_Localised"":""Unknown"", ""KillerShip"":""scout_nq"", ""KillerRank"":""Elite"" }";
                je = JournalEntry.CreateJournalEntry(t4) as JournalDied;
                Check(je != null);

                string t5 =
@"{ ""timestamp"":""2024-12-14T20:02:13Z"", ""event"":""Died"", ""Killers"":[ { ""Name"":""Cmdr SHINOY""" +
@", ""Ship"":""type9"", ""Rank"":""Master"" }, { ""Name"":""Cmdr mcthrust [GPL]""" +
@", ""Ship"":""type9"", ""Rank"":""Competent"" } ] }";
                je = JournalEntry.CreateJournalEntry(t5) as JournalDied;
                Check(je != null);
                string info = je.GetInfo();
                CheckThat(info).Is("Killed by Cmdr SHINOY in ship type Type-9 Heavy rank Master, Cmdr mcthrust [GPL] in ship type Type-9 Heavy rank Competent");

                string t6 = @"{ ""timestamp"":""2024-11-08T23:22:24Z"", ""event"":""Died"", ""KillerShip"":""thargonswarm""" +
@", ""KillerRank"":""Elite"" }";
                je = JournalEntry.CreateJournalEntry(t6) as JournalDied;
                Check(je != null);

                string t8 = @"{ ""timestamp"":""2022-10-09T07:12:55Z"", ""event"":""Died"", ""KillerShip"":""thargon"" }";
                je = JournalEntry.CreateJournalEntry(t8) as JournalDied;
                Check(je != null);

            }
        }

    }
}

