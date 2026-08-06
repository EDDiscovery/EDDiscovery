using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public static class UnitTestMCMR
    {
        [BaseUtils.UnitTests.Test]
        public static void TestMCMR()
        {
            CheckSection("Commodities");
            { // demo FDName discrete 
                string t1 = @"{ ""timestamp"":""2025 - 07 - 25T15: 53:43Z"", ""event"":""MarketBuy"", ""MarketID"":3230473472, ""Type"":""Gold"", ""Count"":100, ""BuyPrice"":42604, ""TotalCost"":4260400 }";
                JournalMarketBuy je = JournalEntry.CreateJournalEntry(t1) as JournalMarketBuy;
                Check(je != null);
                Check(je.MarketID == 3230473472);
                Check(je.Type == new MCFDName("gOld"));
                Check(je.Count == 100);
                Check(je.BuyPrice == 42604);

            }

            CheckSection("Microresources");

            {
                string t1 = @"{ ""timestamp"":""2024-11-16T15:25:18Z"", ""event"":""ShipLocker"", ""Items"":[ { ""Name"":""chemicalsample"", ""Name_Localised"":""Chemical Sample"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""gmeds"", ""Name_Localised"":""G-Meds"", ""OwnerID"":0, ""Count"":3 }, { ""Name"":""healthmonitor"", ""Name_Localised"":""Health Monitor"", ""OwnerID"":145391, ""Count"":2 }, { ""Name"":""healthmonitor"", ""Name_Localised"":""Health Monitor"", ""OwnerID"":0, ""Count"":29 }, { ""Name"":""insight"", ""OwnerID"":0, ""Count"":18 }, { ""Name"":""insightdatabank"", ""Name_Localised"":""Insight Data Bank"", ""OwnerID"":0, ""Count"":3 }, { ""Name"":""personalcomputer"", ""Name_Localised"":""Personal Computer"", ""OwnerID"":0, ""Count"":2 }, { ""Name"":""compactlibrary"", ""Name_Localised"":""Compact Library"", ""OwnerID"":0, ""Count"":39 }, { ""Name"":""hush"", ""OwnerID"":0, ""Count"":2 }, { ""Name"":""infinity"", ""OwnerID"":0, ""Count"":21 }, { ""Name"":""insightentertainmentsuite"", ""Name_Localised"":""Insight Entertainment Suite"", ""OwnerID"":0, ""Count"":18 }, { ""Name"":""lazarus"", ""OwnerID"":0, ""Count"":8 }, { ""Name"":""nutritionalconcentrate"", ""Name_Localised"":""Nutritional Concentrate"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""personaldocuments"", ""Name_Localised"":""Personal Documents"", ""OwnerID"":0, ""Count"":2 }, { ""Name"":""push"", ""OwnerID"":0, ""Count"":3 }, { ""Name"":""syntheticpathogen"", ""Name_Localised"":""Synthetic Pathogen"", ""OwnerID"":0, ""Count"":4 }, { ""Name"":""universaltranslator"", ""Name_Localised"":""Universal Translator"", ""OwnerID"":0, ""Count"":11 }, { ""Name"":""weaponschematic"", ""Name_Localised"":""Weapon Schematic"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""agriculturalprocesssample"", ""Name_Localised"":""Agricultural Process Sample"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""compressionliquefiedgas"", ""Name_Localised"":""Compression-Liquefied Gas"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""degradedpowerregulator"", ""Name_Localised"":""Degraded Power Regulator"", ""OwnerID"":0, ""Count"":22 }, { ""Name"":""largecapacitypowerregulator"", ""Name_Localised"":""Power Regulator"", ""OwnerID"":0, ""Count"":4 } ], ""Components"":[ { ""Name"":""circuitboard"", ""Name_Localised"":""Circuit Board"", ""OwnerID"":0, ""MissionID"":787859621, ""Count"":1 }, { ""Name"":""graphene"", ""OwnerID"":0, ""Count"":9 }, { ""Name"":""circuitboard"", ""Name_Localised"":""Circuit Board"", ""OwnerID"":0, ""Count"":9 }, { ""Name"":""circuitswitch"", ""Name_Localised"":""Circuit Switch"", ""OwnerID"":0, ""Count"":33 }, { ""Name"":""electricalfuse"", ""Name_Localised"":""Electrical Fuse"", ""OwnerID"":0, ""Count"":8 }, { ""Name"":""electricalwiring"", ""Name_Localised"":""Electrical Wiring"", ""OwnerID"":0, ""Count"":7 }, { ""Name"":""encryptedmemorychip"", ""Name_Localised"":""Encrypted Memory Chip"", ""OwnerID"":0, ""Count"":12 }, { ""Name"":""epoxyadhesive"", ""Name_Localised"":""Epoxy Adhesive"", ""OwnerID"":0, ""Count"":16 }, { ""Name"":""memorychip"", ""Name_Localised"":""Memory Chip"", ""OwnerID"":0, ""Count"":12 }, { ""Name"":""metalcoil"", ""Name_Localised"":""Metal Coil"", ""OwnerID"":0, ""Count"":16 }, { ""Name"":""microsupercapacitor"", ""Name_Localised"":""Micro Supercapacitor"", ""OwnerID"":0, ""Count"":11 }, { ""Name"":""microtransformer"", ""Name_Localised"":""Micro Transformer"", ""OwnerID"":0, ""Count"":10 }, { ""Name"":""motor"", ""OwnerID"":0, ""Count"":11 }, { ""Name"":""opticalfibre"", ""Name_Localised"":""Optical Fibre"", ""OwnerID"":0, ""Count"":19 }, { ""Name"":""opticallens"", ""Name_Localised"":""Optical Lens"", ""OwnerID"":0, ""Count"":7 }, { ""Name"":""scrambler"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""transmitter"", ""OwnerID"":0, ""Count"":2 }, { ""Name"":""electromagnet"", ""OwnerID"":0, ""Count"":10 }, { ""Name"":""oxygenicbacteria"", ""Name_Localised"":""Oxygenic Bacteria"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""microelectrode"", ""OwnerID"":0, ""Count"":22 }, { ""Name"":""ionbattery"", ""Name_Localised"":""Ion Battery"", ""OwnerID"":0, ""Count"":17 } ], "
                                + @"""Consumables"":[ { ""Name"":""healthpack"", ""Name_Localised"":""Medkit"", ""OwnerID"":0, ""Count"":100 }, { ""Name"":""energycell"", ""Name_Localised"":""Energy Cell"", ""OwnerID"":0, ""Count"":100 }, { ""Name"":""amm_grenade_emp"", ""Name_Localised"":""Shield Disruptor"", ""OwnerID"":0, ""Count"":100 }, { ""Name"":""amm_grenade_frag"", ""Name_Localised"":""Frag Grenade"", ""OwnerID"":0, ""Count"":100 }, { ""Name"":""amm_grenade_shield"", ""Name_Localised"":""Shield Projector"", ""OwnerID"":0, ""Count"":100 }, { ""Name"":""bypass"", ""Name_Localised"":""E-Breach"", ""OwnerID"":0, ""Count"":100 } ], "
                                + @"""Data"":[ { ""Name"":""internalcorrespondence"", ""Name_Localised"":""Internal Correspondence"", ""OwnerID"":0, ""Count"":5 }, { ""Name"":""biometricdata"", ""Name_Localised"":""Biometric Data"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""nocdata"", ""Name_Localised"":""NOC Data"", ""OwnerID"":0, ""Count"":5 }, { ""Name"":""airqualityreports"", ""Name_Localised"":""Air Quality Reports"", ""OwnerID"":145391, ""Count"":4 }, { ""Name"":""airqualityreports"", ""Name_Localised"":""Air Quality Reports"", ""OwnerID"":0, ""Count"":5 }, { ""Name"":""atmosphericdata"", ""Name_Localised"":""Atmospheric Data"", ""OwnerID"":0, ""Count"":3 }, { ""Name"":""blacklistdata"", ""Name_Localised"":""Blacklist Data"", ""OwnerID"":0, ""Count"":4 }, { ""Name"":""censusdata"", ""Name_Localised"":""Census Data"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""chemicalexperimentdata"", ""Name_Localised"":""Chemical Experiment Data"", ""OwnerID"":0, ""Count"":2 }, { ""Name"":""combattrainingmaterial"", ""Name_Localised"":""Combat Training Material"", ""OwnerID"":145391, ""Count"":1 }, { ""Name"":""factionnews"", ""Name_Localised"":""Faction News"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""genesequencingdata"", ""Name_Localised"":""Gene Sequencing Data"", ""OwnerID"":0, ""Count"":2 }, { ""Name"":""maintenancelogs"", ""Name_Localised"":""Maintenance Logs"", ""OwnerID"":0, ""Count"":5 }, { ""Name"":""maintenancelogs"", ""Name_Localised"":""Maintenance Logs"", ""OwnerID"":145391, ""Count"":9 }, { ""Name"":""manufacturinginstructions"", ""Name_Localised"":""Manufacturing Instructions"", ""OwnerID"":0, ""Count"":4 }, { ""Name"":""medicalrecords"", ""Name_Localised"":""Medical Records"", ""OwnerID"":145391, ""Count"":1 }, { ""Name"":""medicalrecords"", ""Name_Localised"":""Medical Records"", ""OwnerID"":0, ""Count"":4 }, { ""Name"":""mineralsurvey"", ""Name_Localised"":""Mineral Survey"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""mineralsurvey"", ""Name_Localised"":""Mineral Survey"", ""OwnerID"":145391, ""Count"":2 }, { ""Name"":""mininganalytics"", ""Name_Localised"":""Mining Analytics"", ""OwnerID"":0, ""Count"":7 }, { ""Name"":""operationalmanual"", ""Name_Localised"":""Operational Manual"", ""OwnerID"":0, ""Count"":2 }, { ""Name"":""patrolroutes"", ""Name_Localised"":""Patrol Routes"", ""OwnerID"":145391, ""Count"":2 }, { ""Name"":""propaganda"", ""OwnerID"":0, ""Count"":2 }, { ""Name"":""radioactivitydata"", ""Name_Localised"":""Radioactivity Data"", ""OwnerID"":0, ""Count"":4 }, { ""Name"":""reactoroutputreview"", ""Name_Localised"":""Reactor Output Review"", ""OwnerID"":145391, ""Count"":5 }, { ""Name"":""recyclinglogs"", ""Name_Localised"":""Recycling Logs"", ""OwnerID"":145391, ""Count"":2 }, { ""Name"":""riskassessments"", ""Name_Localised"":""Risk Assessments"", ""OwnerID"":145391, ""Count"":7 }, { ""Name"":""securityexpenses"", ""Name_Localised"":""Security Expenses"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""securityexpenses"", ""Name_Localised"":""Security Expenses"", ""OwnerID"":145391, ""Count"":2 }, { ""Name"":""surveilleancelogs"", ""Name_Localised"":""Surveillance Logs"", ""OwnerID"":0, ""Count"":3 }, { ""Name"":""topographicalsurveys"", ""Name_Localised"":""Topographical Surveys"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""troopdeploymentrecords"", ""Name_Localised"":""Troop Deployment Records"", ""OwnerID"":145391, ""Count"":1 }, { ""Name"":""weaponinventory"", ""Name_Localised"":""Weapon Inventory"", ""OwnerID"":145391,""Count"":3 }, { ""Name"":""weaponinventory"", ""Name_Localised"":""Weapon Inventory"", ""OwnerID"":0, ""Count"":5 }, { ""Name"":""weapontestdata"", ""Name_Localised"":""Weapon Test Data"", ""OwnerID"":145391, ""Count"":4 }, { ""Name"":""pharmaceuticalpatents"", ""Name_Localised"":""Pharmaceutical Patents"", ""OwnerID"":0, ""Count"":2 } ] }";
                JournalShipLocker je = JournalEntry.CreateJournalEntry(t1) as JournalShipLocker;
                Check(je != null);
                Check(je.Items[0].Name.Equals("ChemicalSample"));
                Check(je.Consumables[0].Name.Equals("healthpack"));
                Check(je.Data[0].Name.Equals("internalcorrespondence"));
            }

            {
                string t1 = @"{ ""timestamp"":""2023-04-11T11:59:16Z"", ""event"":""BuyMicroResources"", ""Name"":""amm_grenade_emp""" +
                            @", ""Name_Localised"":""Shield Disruptor"", ""Category"":""Consumable"", ""Count"":81" +
                            @", ""Price"":162000, ""MarketID"":3511330304 }";


                var je = JournalEntry.CreateJournalEntry(t1) as JournalBuyMicroResources;
                CheckThat(je).IsNotNull();
                CheckThat(je.Items[0].Name).Is("amm_grenade_emp");
                CheckThat(je.Items[0].Name_Localised).Is("Shield Disruptor");
                CheckThat(je.Items[0].Category).Is(MaterialCommodityMicroResourceType.CatType.Consumable);
                CheckThat(je.Items[0].Count).Is(81);

            }

            {
                string t1 = @"{ ""timestamp"":""2024-08-07T23:26:18Z"", ""event"":""BuyMicroResources"", ""TotalCount"":46" +
                        @", ""MicroResources"":[ { ""Name"":""compressionliquefiedgas"", ""Name_Localised"":""Compression-Liquefied Gas""" +
                        @", ""Category"":""Item"", ""Count"":46 } ], ""Price"":161000000, ""MarketID"":3703465728 }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalBuyMicroResources;
                CheckThat(je).IsNotNull();
                CheckThat(je.Items[0].Name).Is("compressionliquefiedgas");
                CheckThat(je.Items[0].Name_Localised).Is("Compression-Liquefied Gas");
                CheckThat(je.Items[0].Category).Is(MaterialCommodityMicroResourceType.CatType.Item);
                CheckThat(je.Items[0].Count).Is(46);

            }
            {
                string t1 = @"{ ""timestamp"":""2024-08-07T23:51:32Z"", ""event"":""BuyMicroResources"", ""TotalCount"":80" +
                            @", ""MicroResources"":[ { ""Name"":""weaponschematic"", ""Name_Localised"":""Weapon Schematic""" +
                            @", ""Category"":""Item"", ""Count"":2 }, { ""Name"":""titaniumplating"", ""Name_Localised"":""Titanium Plating""" +
                            @", ""Category"":""Component"", ""Count"":12 }, { ""Name"":""tungstencarbide"", ""Name_Localised"":""Tungsten Carbide""" +
                            @", ""Category"":""Component"", ""Count"":14 }, { ""Name"":""smearcampaignplans""" +
                            @", ""Name_Localised"":""Smear Campaign Plans"", ""Category"":""Data"", ""Count"":5 }" +
                            @", { ""Name"":""graphene"", ""Category"":""Component"", ""Count"":41 }, { ""Name"":""carbonfibreplating""" +
                            @", ""Name_Localised"":""Carbon Fibre Plating"", ""Category"":""Component"", ""Count"":6 } ]" +
                            @", ""Price"":10992500, ""MarketID"":3701182208 }";


                var je = JournalEntry.CreateJournalEntry(t1) as JournalBuyMicroResources;
                CheckThat(je).IsNotNull();
                CheckThat(je.Items[0].Name).Is("weaponschematic");
                CheckThat(je.Items[0].Count).Is(2);
                CheckThat(je.Items[0].Category).Is(MaterialCommodityMicroResourceType.CatType.Item);
                CheckThat(je.Items[1].Name).Is("titaniumplating");
                CheckThat(je.Items[1].Count).Is(12);
                CheckThat(je.Items[1].Category).Is(MaterialCommodityMicroResourceType.CatType.Component);
                CheckThat(je.Items[5].Name).Is("carbonfibreplating");
                CheckThat(je.Items[5].Count).Is(6);
                CheckThat(je.Items[5].Category).Is(MaterialCommodityMicroResourceType.CatType.Component);

            }
            {
                string t1 =@"{ ""timestamp"":""2024-08-11T23:00:26Z"", ""event"":""SellMicroResources"", ""TotalCount"":21" +
                        @", ""MicroResources"":[ { ""Name"":""hush"", ""Category"":""Item"", ""Count"":9 }" +
                        @", { ""Name"":""inertiacanister"", ""Name_Localised"":""Inertia Canister"", ""Category"":""Item""" +
                        @", ""Count"":1 }, { ""Name"":""lazarus"", ""Category"":""Item"", ""Count"":7 }" +
                        @", { ""Name"":""universaltranslator"", ""Name_Localised"":""Universal Translator""" +
                        @", ""Category"":""Item"", ""Count"":1 }, { ""Name"":""chemicalsample"", ""Name_Localised"":""Chemical Sample""" +
                        @", ""Category"":""Item"", ""Count"":2 }, { ""Name"":""degradedpowerregulator""" +
                        @", ""Name_Localised"":""Degraded Power Regulator"", ""Category"":""Item"", ""Count"":1 } ]" +
                        @", ""Price"":1680000, ""MarketID"":3221362432 }";


                var je = JournalEntry.CreateJournalEntry(t1) as JournalSellMicroResources;
                CheckThat(je).IsNotNull();
                CheckThat(je.Items[0].Name).Is("hush");
                CheckThat(je.Items[0].Count).Is(9);
                CheckThat(je.Items[0].Category).Is(MaterialCommodityMicroResourceType.CatType.Item);
                CheckThat(je.Items[5].Name).Is("degradedpowerregulator");
                CheckThat(je.Items[5].Count).Is(1);
                CheckThat(je.Items[5].Category).Is(MaterialCommodityMicroResourceType.CatType.Item);

            }
            {
                string t1 =@"{ ""timestamp"":""2025-09-03T14:45:08Z"", ""event"":""TradeMicroResources"", ""Offered"":[ { ""Name"":""microelectrode""" +
                            @", ""Name_Localised"":""Микроэлектрод"", ""Category"":""Component"", ""Count"":3 } ]" +
                            @", ""TotalCount"":3, ""Received"":""motor"", ""Received_Localised"":""Мотор"", ""Count"":5" +
                        @", ""Category"":""Component"", ""MarketID"":3225744896 }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalTradeMicroResources;
                CheckThat(je).IsNotNull();
                CheckThat(je.Offered[0].Name).Is("microelectrode");
                CheckThat(je.Offered[0].Name_Localised).Is("Микроэлектрод");
                CheckThat(je.Offered[0].Category).Is(MaterialCommodityMicroResourceType.CatType.Component);
                CheckThat(je.Offered[0].Count).Is(3);
                CheckThat(je.Received).Is("motor");
                CheckThat(je.Received_Localised).Is("Мотор");
                CheckThat(je.Category).Is(MaterialCommodityMicroResourceType.CatType.Component);
                CheckThat(je.Count).Is(5);
            }

            {
                string t1 =@"{ ""timestamp"":""2025-03-07T17:57:12Z"", ""event"":""Backpack"", ""Items"":[  ]" +
                            @", ""Components"":[  ], ""Consumables"":[ { ""Name"":""healthpack"", ""Name_Localised"":""Medkit""" +
                            @", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""energycell"", ""Name_Localised"":""Energy Cell""" +
                            @", ""OwnerID"":0, ""Count"":3 }, { ""Name"":""amm_grenade_emp"", ""Name_Localised"":""Shield Disruptor""" +
                            @", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""amm_grenade_frag"", ""Name_Localised"":""Frag Grenade""" +
                            @", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""amm_grenade_shield"", ""Name_Localised"":""Shield Projector""" +
                            @", ""OwnerID"":0, ""Count"":1 } ], ""Data"":[  ] }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalBackpack;
                CheckThat(je).IsNotNull();
                CheckThat(je.Items.Length).Is(0);
                CheckThat(je.Components.Length).Is(0);
                CheckThat(je.Consumables[0].Name).Is("HEALTHPACK");
                CheckThat(je.Consumables[0].Name_Localised).Is("Medkit");
                CheckThat(je.Consumables[0].OwnerID).Is(0UL);
                CheckThat(je.Consumables[0].Count).Is(1);
                CheckThat(je.Consumables[0].Category).Is(MaterialCommodityMicroResourceType.CatType.Consumable);
                CheckThat(je.Data.Length).Is(0);

            }
            {
                string t1 =@"{ ""timestamp"":""2021-05-30T04:06:07Z"", ""event"":""Backpack"", ""Items"":[  ]" +
                        @", ""Components"":[ { ""Name"":""metalcoil"", ""Name_Localised"":""Metal Coil""" +
                        @", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""microtransformer"", ""Name_Localised"":""Micro Transformer""" +
                        @", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""motor"", ""OwnerID"":0, ""Count"":1 }" +
                        @", { ""Name"":""opticalfibre"", ""Name_Localised"":""Optical Fibre"", ""OwnerID"":0" +
                        @", ""Count"":1 }, { ""Name"":""electromagnet"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""microelectrode""" +
                        @", ""OwnerID"":0, ""Count"":1 } ], ""Consumables"":[ { ""Name"":""healthpack""" +
                        @", ""Name_Localised"":""Medkit"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""energycell""" +
                        @", ""Name_Localised"":""Energy Cell"", ""OwnerID"":0, ""Count"":2 }, { ""Name"":""amm_grenade_emp""" +
                        @", ""Name_Localised"":""Shield Disruptor"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""amm_grenade_frag""" +
                        @", ""Name_Localised"":""Frag Grenade"", ""OwnerID"":0, ""Count"":2 }, { ""Name"":""amm_grenade_shield""" +
                        @", ""Name_Localised"":""Shield Projector"", ""OwnerID"":0, ""Count"":1 } ], ""Data"":[ { ""Name"":""mineralsurvey""" +
                        @", ""Name_Localised"":""Mineral Survey"", ""OwnerID"":0, ""Count"":1 }, { ""Name"":""operationalmanual""" +
                        @", ""Name_Localised"":""Operational Manual"", ""OwnerID"":0, ""Count"":1 } ] }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalBackpack;
                CheckThat(je).IsNotNull();
                CheckThat(je.Items.Length).Is(0);
                CheckThat(je.Components[0].Name).Is("metalcoil");
                CheckThat(je.Components[0].Category).Is(MaterialCommodityMicroResourceType.CatType.Component);
                CheckThat(je.Consumables[0].Name).Is("HEALTHPACK");
                CheckThat(je.Consumables[0].Name_Localised).Is("Medkit");
                CheckThat(je.Consumables[0].OwnerID).Is(0UL);
                CheckThat(je.Consumables[0].Count).Is(1);
                CheckThat(je.Consumables[0].Category).Is(MaterialCommodityMicroResourceType.CatType.Consumable);
                CheckThat(je.Data[0].Name).Is("mineralsurvey");
                CheckThat(je.Data[0].Category).Is(MaterialCommodityMicroResourceType.CatType.Data);


            }
            {
                string t1 = @"{ ""timestamp"":""2025-11-26T01:38:31Z"", ""event"":""Resupply"" }";      // all are empty
                var je = JournalEntry.CreateJournalEntry(t1) as JournalResupply;
                CheckThat(je).IsNotNull();
                CheckThat(je.Items).IsNull();
                CheckThat(je.Data).IsNull();
            }
            {
                string t1 = @"{ ""timestamp"":""2021-04-29T03:28:37Z"", ""event"":""CollectItems"", ""Name"":""infinity""" +
@", ""Type"":""Item"", ""OwnerID"":0, ""Count"":1, ""Stolen"":true }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalCollectItems;
                CheckThat(je).IsNotNull();
                CheckThat(je.Resource.Name).Is("infinity");
                CheckThat(je.Resource.Category).Is(MaterialCommodityMicroResourceType.CatType.Item);
                CheckThat(je.Resource.OwnerID).Is(0UL);
                CheckThat(je.Stolen).Is(true);

            }
            {
                string t1 =@"{ ""timestamp"":""2023-04-29T19:12:11Z"", ""event"":""DropItems"", ""Name"":""geneticsample""" +
@", ""Name_Localised"":""Biological Sample"", ""Type"":""Item"", ""OwnerID"":0, ""MissionID"":922112975" +
@", ""Count"":1 }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalDropItems;
                CheckThat(je).IsNotNull();
                CheckThat(je.Resource.Name).Is("geneticsample");
                CheckThat(je.Resource.Name_Localised).Is("Biological Sample");
                CheckThat(je.Resource.Category).Is(MaterialCommodityMicroResourceType.CatType.Item);
                CheckThat(je.Resource.OwnerID).Is(0UL);
                CheckThat(je.Resource.MissionID).Is(new MissionID(922112975UL));
                CheckThat(je.Resource.Count).Is(1);

            }
            {
                string t1 = @"{ ""timestamp"":""2024-03-29T15:28:27Z"", ""event"":""UseConsumable"", ""Name"":""energycell""" +
                            @", ""Name_Localised"":""Energy Cell"", ""Type"":""Consumable"" }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalUseConsumable;
                CheckThat(je).IsNotNull();
                CheckThat(je.Resource.Name).Is("energycell");
                CheckThat(je.Resource.Name_Localised).Is("Energy Cell");
                CheckThat(je.Resource.Category).Is(MaterialCommodityMicroResourceType.CatType.Consumable);

            }

            {
                string t1 = @"{ ""timestamp"":""2024-11-11T15:30:29Z"", ""event"":""DeliverPowerMicroResources""" +
                            @", ""TotalCount"":239, ""MicroResources"":[ { ""Name"":""powerpropagandadata""" +
                            @", ""Name_Localised"":""Información política de la potencia"", ""Category"":""Data""" +
                            @", ""Count"":239 } ], ""MarketID"":3225336320 }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalDeliverPowerMicroResources;
                CheckThat(je).IsNotNull();
                CheckThat(je.MicroResources[0].Name).Is("powerpropagandadata");
                CheckThat(je.MicroResources[0].Name_Localised).Is("Información política de la potencia");
                CheckThat(je.MicroResources[0].Category).Is(MaterialCommodityMicroResourceType.CatType.Data);
                CheckThat(je.MicroResources[0].Count).Is(239);
                CheckThat(je.MarketID).Is(3225336320UL);

            }
            {
                string t1 = @"{ ""timestamp"":""2024-10-31T18:51:14Z"", ""event"":""RequestPowerMicroResources""" +
                    @", ""TotalCount"":10, ""MicroResources"":[ { ""Name"":""powerpreparationspyware""" +
                @", ""Name_Localised"":""Power Injection Malware"", ""Category"":""Data"", ""Count"":10 } ]" +
                @", ""MarketID"":3511846912 }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalRequestPowerMicroResources;
                CheckThat(je).IsNotNull();
                CheckThat(je.MicroResources[0].Name).Is("powerpreparationspyware");
                CheckThat(je.MicroResources[0].Name_Localised).Is("Power Injection Malware");
                CheckThat(je.MicroResources[0].Category).Is(MaterialCommodityMicroResourceType.CatType.Data);
                CheckThat(je.MicroResources[0].Count).Is(10);
                CheckThat(je.MarketID).Is(3511846912UL);


            }

            CheckSection("Materials");

            {
                string t1 = @"{ ""timestamp"":""2025-07-25T15:04:51Z"", ""event"":""Materials"", 
""Raw"":[ { ""Name"":""nickel"", ""Count"":255 }, { ""Name"":""tin"", ""Count"":38 }, { ""Name"":""vanadium"", ""Count"":143 }, { ""Name"":""iron"", ""Count"":127 }, { ""Name"":""manganese"", ""Count"":59 }, { ""Name"":""sulphur"", ""Count"":160 }, { ""Name"":""phosphorus"", ""Count"":265 }, { ""Name"":""chromium"", ""Count"":56 }, { ""Name"":""germanium"", ""Count"":40 }, { ""Name"":""cadmium"", ""Count"":35 }, { ""Name"":""molybdenum"", ""Count"":13 }, { ""Name"":""zirconium"", ""Count"":22 }, { ""Name"":""niobium"", ""Count"":23 }, { ""Name"":""zinc"", ""Count"":128 }, { ""Name"":""arsenic"", ""Count"":29 }, { ""Name"":""polonium"", ""Count"":18 }, { ""Name"":""yttrium"", ""Count"":38 }, { ""Name"":""mercury"", ""Count"":13 }, { ""Name"":""carbon"", ""Count"":48 }, { ""Name"":""tungsten"", ""Count"":12 }, { ""Name"":""selenium"", ""Count"":5 }, { ""Name"":""ruthenium"", ""Count"":24 }, { ""Name"":""technetium"", ""Count"":9 }, { ""Name"":""antimony"", ""Count"":10 }, { ""Name"":""tellurium"", ""Count"":3 }, { ""Name"":""rhenium"", ""Count"":15 }, { ""Name"":""lead"", ""Count"":21 }, { ""Name"":""boron"", ""Count"":6 } ], 
""Manufactured"":[ { ""Name"":""focuscrystals"", ""Name_Localised"":""Focus Crystals"", ""Count"":107 }, { ""Name"":""chemicaldistillery"", ""Name_Localised"":""Chemical Distillery"", ""Count"":81 }, { ""Name"":""precipitatedalloys"", ""Name_Localised"":""Precipitated Alloys"", ""Count"":69 }, { ""Name"":""compoundshielding"", ""Name_Localised"":""Compound Shielding"", ""Count"":80 }, { ""Name"":""protolightalloys"", ""Name_Localised"":""Proto Light Alloys"", ""Count"":54 }, { ""Name"":""refinedfocuscrystals"", ""Name_Localised"":""Refined Focus Crystals"", ""Count"":32 }, { ""Name"":""mechanicalscrap"", ""Name_Localised"":""Mechanical Scrap"", ""Count"":37 }, { ""Name"":""temperedalloys"", ""Name_Localised"":""Tempered Alloys"", ""Count"":30 }, { ""Name"":""heatresistantceramics"", ""Name_Localised"":""Heat Resistant Ceramics"", ""Count"":31 }, { ""Name"":""gridresistors"", ""Name_Localised"":""Grid Resistors"", ""Count"":33 }, { ""Name"":""hybridcapacitors"", ""Name_Localised"":""Hybrid Capacitors"", ""Count"":33 }, { ""Name"":""conductivecomponents"", ""Name_Localised"":""Conductive Components"", ""Count"":53 }, { ""Name"":""exquisitefocuscrystals"", ""Name_Localised"":""Exquisite Focus Crystals"", ""Count"":3 }, { ""Name"":""thermicalloys"", ""Name_Localised"":""Thermic Alloys"", ""Count"":34 }, { ""Name"":""conductivepolymers"", ""Name_Localised"":""Conductive Polymers"", ""Count"":11 }, { ""Name"":""uncutfocuscrystals"", ""Name_Localised"":""Flawed Focus Crystals"", ""Count"":117 }, { ""Name"":""electrochemicalarrays"", ""Name_Localised"":""Electrochemical Arrays"", ""Count"":21 }, { ""Name"":""basicconductors"", ""Name_Localised"":""Basic Conductors"", ""Count"":28 }, { ""Name"":""mechanicalequipment"", ""Name_Localised"":""Mechanical Equipment"", ""Count"":28 }, { ""Name"":""conductiveceramics"", ""Name_Localised"":""Conductive Ceramics"", ""Count"":45 }, { ""Name"":""chemicalmanipulators"", ""Name_Localised"":""Chemical Manipulators"", ""Count"":3 }, { ""Name"":""shieldemitters"", ""Name_Localised"":""Shield Emitters"", ""Count"":245 }, { ""Name"":""chemicalprocessors"", ""Name_Localised"":""Chemical Processors"", ""Count"":108 }, { ""Name"":""chemicalstorageunits"", ""Name_Localised"":""Chemical Storage Units"", ""Count"":12 }, { ""Name"":""galvanisingalloys"", ""Name_Localised"":""Galvanising Alloys"", ""Count"":141 }, { ""Name"":""salvagedalloys"", ""Name_Localised"":""Salvaged Alloys"", ""Count"":72 }, { ""Name"":""phasealloys"", ""Name_Localised"":""Phase Alloys"", ""Count"":198 }, { ""Name"":""configurablecomponents"", ""Name_Localised"":""Configurable Components"", ""Count"":9 }, { ""Name"":""wornshieldemitters"", ""Name_Localised"":""Worn Shield Emitters"", ""Count"":111 }, { ""Name"":""mechanicalcomponents"", ""Name_Localised"":""Mechanical Components"", ""Count"":34 }, { ""Name"":""fedproprietarycomposites"", ""Name_Localised"":""Proprietary Composites"", ""Count"":46 }, { ""Name"":""crystalshards"", ""Name_Localised"":""Crystal Shards"", ""Count"":69 }, { ""Name"":""filamentcomposites"", ""Name_Localised"":""Filament Composites"", ""Count"":36 }, { ""Name"":""compactcomposites"", ""Name_Localised"":""Compact Composites"", ""Count"":39 }, { ""Name"":""heatconductionwiring"", ""Name_Localised"":""Heat Conduction Wiring"", ""Count"":26 }, { ""Name"":""fedcorecomposites"", ""Name_Localised"":""Core Dynamics Composites"", ""Count"":17 }, { ""Name"":""protoheatradiators"", ""Name_Localised"":""Proto Heat Radiators"", ""Count"":27 }, { ""Name"":""polymercapacitors"", ""Name_Localised"":""Polymer Capacitors"", ""Count"":22 }, { ""Name"":""shieldingsensors"", ""Name_Localised"":""Shielding Sensors"", ""Count"":190 }, { ""Name"":""highdensitycomposites"", ""Name_Localised"":""High Density Composites"", ""Count"":65 }, { ""Name"":""heatdispersionplate"", ""Name_Localised"":""Heat Dispersion Plate"", ""Count"":24 }, { ""Name"":""heatvanes"", ""Name_Localised"":""Heat Vanes"", ""Count"":24 }, { ""Name"":""heatexchangers"", ""Name_Localised"":""Heat Exchangers"", ""Count"":12 }, { ""Name"":""unknownenergysource"", ""Name_Localised"":""Sensor Fragment"", ""Count"":29 }, { ""Name"":""pharmaceuticalisolators"", ""Name_Localised"":""Pharmaceutical Isolators"", ""Count"":5 }, { ""Name"":""militarysupercapacitors"", ""Name_Localised"":""Military Supercapacitors"", ""Count"":12 }, { ""Name"":""militarygradealloys"", ""Name_Localised"":""Military Grade Alloys"", ""Count"":24 } ], 
""Encoded"":[ { ""Name"":""emissiondata"", ""Name_Localised"":""Unexpected Emission Data"", ""Count"":60 }, { ""Name"":""shieldpatternanalysis"", ""Name_Localised"":""Aberrant Shield Pattern Analysis"", ""Count"":90 }, { ""Name"":""shieldcyclerecordings"", ""Name_Localised"":""Distorted Shield Cycle Recordings"", ""Count"":90 }, { ""Name"":""decodedemissiondata"", ""Name_Localised"":""Decoded Emission Data"", ""Count"":81 }, { ""Name"":""bulkscandata"", ""Name_Localised"":""Anomalous Bulk Scan Data"", ""Count"":48 }, { ""Name"":""shieldsoakanalysis"", ""Name_Localised"":""Inconsistent Shield Soak Analysis"", ""Count"":75 }, { ""Name"":""shielddensityreports"", ""Name_Localised"":""Untypical Shield Scans"", ""Count"":96 }, { ""Name"":""encodedscandata"", ""Name_Localised"":""Divergent Scan Data"", ""Count"":5 }, { ""Name"":""scrambledemissiondata"", ""Name_Localised"":""Exceptional Scrambled Emission Data"", ""Count"":5 }, { ""Name"":""legacyfirmware"", ""Name_Localised"":""Specialised Legacy Firmware"", ""Count"":11 }, { ""Name"":""shieldfrequencydata"", ""Name_Localised"":""Peculiar Shield Frequency Data"", ""Count"":12 }, { ""Name"":""scanarchives"", ""Name_Localised"":""Unidentified Scan Archives"", ""Count"":27 }, { ""Name"":""archivedemissiondata"", ""Name_Localised"":""Irregular Emission Data"", ""Count"":31 }, { ""Name"":""consumerfirmware"", ""Name_Localised"":""Modified Consumer Firmware"", ""Count"":2 }, { ""Name"":""encryptioncodes"", ""Name_Localised"":""Tagged Encryption Codes"", ""Count"":6 }, { ""Name"":""wakesolutions"", ""Name_Localised"":""Strange Wake Solutions"", ""Count"":3 }, { ""Name"":""fsdtelemetry"", ""Name_Localised"":""Anomalous FSD Telemetry"", ""Count"":40 }, { ""Name"":""encryptionarchives"", ""Name_Localised"":""Atypical Encryption Archives"", ""Count"":18 }, { ""Name"":""symmetrickeys"", ""Name_Localised"":""Open Symmetric Keys"", ""Count"":3 }, { ""Name"":""adaptiveencryptors"", ""Name_Localised"":""Adaptive Encryptors Capture"", ""Count"":3 }, { ""Name"":""industrialfirmware"", ""Name_Localised"":""Cracked Industrial Firmware"", ""Count"":11 }, { ""Name"":""disruptedwakeechoes"", ""Name_Localised"":""Atypical Disrupted Wake Echoes"", ""Count"":40 }, { ""Name"":""securityfirmware"", ""Name_Localised"":""Security Firmware Patch"", ""Count"":5 }, { ""Name"":""classifiedscandata"", ""Name_Localised"":""Classified Scan Fragment"", ""Count"":3 }, 
{ ""Name"":""hyperspacetrajectories"", ""Name_Localised"":""Eccentric Hyperspace Trajectories"", ""Count"":11 }, 
{ ""Name"":""dataminedwake"", ""Name_Localised"":""Datamined Wake Exceptions"", ""Count"":5 }, { ""Name"":""scandatabanks"", ""Name_Localised"":""Classified Scan Databanks"", ""Count"":28 }, { ""Name"":""compactemissionsdata"", ""Name_Localised"":""Abnormal Compact Emissions Data"", ""Count"":11 }, { ""Name"":""embeddedfirmware"", ""Name_Localised"":""Modified Embedded Firmware"", ""Count"":4 }, { ""Name"":""tg_interdictiondata"", ""Name_Localised"":""Thargoid Interdiction Telemetry"", ""Count"":18 } ] }
";

                JournalMaterials je = JournalEntry.CreateJournalEntry(t1) as JournalMaterials;
                Check(je != null);
                Check(je.Raw[0].Name.Equals("nickel"));
                Check(je.Raw[0].Count == 255);
                Check(je.Manufactured[1].Name.Equals("chemicaldistillery"));
                Check(je.Manufactured[1].Name_Localised.Equals("Chemical Distillery"));
                Check(je.Manufactured[1].Count == 81);
                Check(je.Encoded[1].Name.Equals("shieldpatternanalysis"));
                Check(je.Encoded[1].Name_Localised.Equals("Aberrant Shield Pattern Analysis"));
                Check(je.Encoded[1].Count == 90);
            }

            {
                string t1 = @"{ ""timestamp"":""2024-09-18T18:09:11Z"", ""event"":""MaterialCollected"", ""Category"":""Manufactured""" +
                        @", ""Name"":""imperialshielding"", ""Name_Localised"":""Imperial Shielding"", ""Count"":3 }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalMaterialCollected;
                CheckThat(je).IsNotNull();
                CheckThat(je.Name).Is("imperialshielding");
                CheckThat(je.Name_Localised).Is("Imperial Shielding");
                CheckThat(je.Count).Is(3);
                CheckThat(je.Category).Is(MaterialCommodityMicroResourceType.CatType.Manufactured);


            }
            {
                string t1 = @"{ ""timestamp"":""2024-11-10T17:38:46Z"", ""event"":""MaterialDiscarded"", ""Category"":""Data""" +
                            @", ""Name"":""surveilleancelogs"", ""Name_Localised"":""Registros de vigilancia""" +
                            @", ""Count"":4 }";


                var je = JournalEntry.CreateJournalEntry(t1) as JournalMaterialDiscarded;
                CheckThat(je).IsNotNull();
                CheckThat(je.Name).Is("surveilleancelogs");
                CheckThat(je.Name_Localised).Is("Registros de vigilancia");
                CheckThat(je.Count).Is(4);
                CheckThat(je.Category).Is(MaterialCommodityMicroResourceType.CatType.Data);


            }
            {
                string t1 = @"{ ""timestamp"":""2024-05-20T03:45:52Z"", ""event"":""MaterialDiscovered"", ""Category"":""Manufactured""" +
                            @", ""Name"":""fedcorecomposites"", ""Name_Localised"":""Core Dynamics Composites""" +
                        @", ""DiscoveryNumber"":19 }";


                var je = JournalEntry.CreateJournalEntry(t1) as JournalMaterialDiscovered;
                CheckThat(je).IsNotNull();
                CheckThat(je).IsNotNull();
                CheckThat(je.Name).Is("fedcorecomposites");
                CheckThat(je.Name_Localised).Is("Core Dynamics Composites");
                CheckThat(je.DiscoveryNumber).Is(19);
                CheckThat(je.Category).Is(MaterialCommodityMicroResourceType.CatType.Manufactured);


            }

            {
                string t1 =@"{ ""timestamp"":""2022-11-02T17:58:21Z"", ""event"":""MaterialTrade"", ""MarketID"":3227528960" +
                            @", ""TraderType"":""manufactured"", ""Paid"":{ ""Material"":""fedcorecomposites""" +
                            @", ""Material_Localised"":""Core Dynamics Composites"", ""Category"":""Manufactured""" +
                            @", ""Quantity"":18 }, ""Received"":{ ""Material"":""exquisitefocuscrystals"", ""Material_Localised"":""Exquisite Focus Crystals""" +
                            @", ""Category"":""Manufactured"", ""Quantity"":3 } }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalMaterialTrade;
                CheckThat(je).IsNotNull();
                CheckThat(je.Paid.Material).Is("fedcorecomposites");
                CheckThat(je.Paid.Material_Localised).Is("Core Dynamics Composites");
                CheckThat(je.Paid.Category).Is(MaterialCommodityMicroResourceType.CatType.Manufactured);
                CheckThat(je.Paid.Quantity).Is(18);
                CheckThat(je.Received.Material).Is("exquisitefocuscrystals");
                CheckThat(je.Received.Material_Localised).Is("Exquisite Focus Crystals");
                CheckThat(je.Received.Category).Is(MaterialCommodityMicroResourceType.CatType.Manufactured);
                CheckThat(je.Received.Quantity).Is(3);

            }
            {
                string t1 =@"{ ""timestamp"":""2022-09-22T03:53:29Z"", ""event"":""Synthesis"", ""Name"":""Heat Sink Basic""" +
                    @", ""Materials"":[ { ""Name"":""basicconductors"", ""Name_Localised"":""Basic Conductors""" +
                    @", ""Count"":2 }, { ""Name"":""heatconductionwiring"", ""Name_Localised"":""Heat Conduction Wiring""" +
                    @", ""Count"":2 } ] }";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalSynthesis;
                CheckThat(je).IsNotNull();
                CheckThat(je.Name).Is("Heat Sinks");
                //CheckThat(je.FDName).Is("Heat Sink Basic");
                CheckThat(je.Materials[new MCFDName("BASICconductors")]).Is(2);


            }
        }
    }
}
