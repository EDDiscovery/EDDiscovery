/*
 * Copyright © 2016 EDDiscovery development team
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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EliteDangerousCore
{
    // [System.Diagnostics.DebuggerDisplay("MatDB {category} {name} {fdname} {type} {shortname}")]
    public class MaterialCommodityDB
    {
        public string category { get; set; }                // either Commodity, or one of the Category types from the MaterialCollected type.
        public string name { get; set; }                    // name of it in nice text
        public string fdname { get; set; }                  // fdnames
        public string type { get; set; }                    // and its type, for materials its rarity, for commodities its group ("Metals" etc).
        public string shortname { get; set; }               // short abv. name
        public Color colour { get; set; }                   // colour if its associated with one
        public int flags { get; set; }                      // now out of date.. keep for now to limit code changes

        public static string CommodityCategory = "Commodity";
        public static string MaterialRawCategory = "Raw";
        public static string MaterialEncodedCategory = "Encoded";
        public static string MaterialManufacturedCategory = "Manufactured";

        public static string CommodityTypeRareGoods = "Rare Goods";

        private static Dictionary<string, MaterialCommodityDB> cachelist = new Dictionary<string, MaterialCommodityDB>();

        public static MaterialCommodityDB GetCachedMaterial(string fdname)
        {
            return cachelist.ContainsKey(fdname) ? cachelist[fdname] : null;
        }

        public static MaterialCommodityDB GetCachedMaterialByShortName(string shortname)
        {
            List<MaterialCommodityDB> lst = cachelist.Values.ToList();
            int i = lst.FindIndex(x => x.shortname.Equals(shortname));
            return i >= 0 ? lst[i] : null;
        }
        
        public MaterialCommodityDB()
        {
        }

        public MaterialCommodityDB(string cs, string n, string fd, string t, string shortn, Color cl, int fl)
        {
            category = cs;
            name = n;
            fdname = fd;
            type = t;
            shortname = shortn;
            colour = cl;
            flags = fl;
        }

        public void SetCache()
        {
            cachelist[this.fdname.ToLower()] = this;
        }

        public static MaterialCommodityDB EnsurePresent(string cat, string fdname, SQLiteConnectionUser conn)  // By FDNAME
        {
            if (!cachelist.ContainsKey(fdname.ToLower()))
            {
                MaterialCommodityDB mcdb = new MaterialCommodityDB(cat, fdname.SplitCapsWordFull(), fdname, "", "", Color.Green, 0);
                mcdb.SetCache();
                System.Diagnostics.Debug.WriteLine("Material not present: " + cat + "," + fdname );
            }

            return cachelist[fdname.ToLower()];
        }


        #region Initial setup

        static Color CByType(string s)
        {
            if (s == "Very Rare")
                return Color.Red;
            if (s == "Rare")
                return Color.Yellow;
            if (s == "Very Common")
                return Color.Cyan;
            if (s == "Common")
                return Color.Green;
            if (s == "Standard")
                return Color.SandyBrown;
            if (s == "Unknown")
                return Color.Red;
            System.Diagnostics.Debug.Assert(false);
            return Color.Black;
        }

        private static bool AddRare(string fdname, string aliasname)
        {
            return Add(CommodityCategory, Color.Green, aliasname, CommodityTypeRareGoods, "", fdname);
        }

        private static bool AddRaw(string name, string typeofit, string shortname)
        {
            return Add(MaterialRawCategory, CByType(typeofit), name, typeofit, shortname);
        }

        private static bool AddEnc(string name, string typeofit, string shortname, string fdname="")
        {
            return Add(MaterialEncodedCategory, CByType(typeofit), name, typeofit, shortname, fdname);
        }

        private static bool AddManu(string name, string typeofit, string shortname , string fdname="")
        {
            return Add(MaterialManufacturedCategory, CByType(typeofit), name, typeofit, shortname , fdname);
        }

        private static bool AddCommodity(string aliasnamelist, string typeofit, string fdname = "")        // fdname only if not a list.
        {
            return AddList(CommodityCategory, Color.Green, aliasnamelist, typeofit, "", fdname);
        }

        private static bool AddCommodityO(string typeofit, string fdname, string aliasnamelist)        // for FD order
        {
            return AddList(CommodityCategory, Color.Green, aliasnamelist, typeofit, "", fdname);
        }

        // fdname only useful if aliasname is not a list.
        private static bool AddList(string category, Color colour, string aliasnamelist, string typeofit, string shortname = "" , string fdname = "")
        {
            string[] list = aliasnamelist.Split(';');

            foreach (string name in list)
            {
                if (name.Length > 0)   // just in case a semicolon slips thru
                {
                    if (!Add(category, colour, name, typeofit, shortname, fdname))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool Add(string catname, Color colour, string aliasname, string typeofit, string shortname = "", string fdName = "")
        {
            string fdn = (fdName.Length > 0) ? fdName : aliasname.FDName();
            MaterialCommodityDB mc = new MaterialCommodityDB(catname, aliasname, fdn, typeofit, shortname, colour, 0);
            mc.SetCache();
            return true;
        }

        public static void SetUpInitialTable()
        {
            AddRaw("Antimony", "Very Rare", "Sb");
            AddRaw("Polonium", "Very Rare", "Po");
            AddRaw("Ruthenium", "Very Rare", "Ru");
            AddRaw("Technetium", "Very Rare", "Tc");
            AddRaw("Tellurium", "Very Rare", "Te");

            AddRaw( "Yttrium", "Rare", "Y");
            AddRaw( "Cadmium", "Rare", "Cd");
            AddRaw( "Mercury", "Rare", "Hg");
            AddRaw( "Molybdenum", "Rare", "Mo");
            AddRaw( "Tin", "Rare", "Sn");

            AddRaw("Carbon", "Very Common", "C");
            AddRaw("Iron", "Very Common", "Fe");
            AddRaw("Nickel", "Very Common", "Ni");
            AddRaw("Phosphorus", "Very Common", "P");
            AddRaw("Sulphur", "Very Common", "S");

            AddRaw( "Chromium", "Common", "Cr");
            AddRaw( "Germanium", "Common", "Ge");
            AddRaw( "Manganese", "Common", "Mn");
            AddRaw( "Vanadium", "Common", "V");
            AddRaw( "Zinc", "Common", "Zn");
                               
            AddRaw("Niobium", "Standard", "Nb");        // realign to Anthors standard
            AddRaw("Tungsten", "Standard", "W");
            AddRaw("Arsenic", "Standard", "As");
            AddRaw("Selenium", "Standard", "Se");
            AddRaw("Zirconium", "Standard", "Zr");
                                     
            AddCommodity("Explosives;Hydrogen Fuel;Hydrogen Peroxide;Liquid Oxygen;Mineral Oil;Nerve Agents;Pesticides;Surface Stabilisers;Synthetic Reagents;Water", "Chemicals");

            AddCommodity("Clothing;Consumer Technology;Domestic Appliances;Evacuation Shelter;Survival Equipment", "Consumer Items");

            AddCommodity("Algae;Animal Meat;Coffee;Fish;Food Cartridges;Fruit and Vegetables;Grain;Synthetic Meat;Tea", "Foods");

            string im = "Industrial Materials";
            AddCommodity("Ceramic Composites;CMM Composite;Insulating Membrane;Neofabric Insulation;Polymers;Semiconductors;Superconductors", im);
            AddCommodity("Meta-Alloys", im, "metaalloys");
            AddCommodity("Micro-Weave Cooling Hoses", im, "coolinghoses");

            string ld = "Legal Drugs";
            AddCommodity("Beer;Bootleg Liquor;Liquor;Tobacco;Wine;Lavian Brandy", ld);
            AddCommodity("Narcotics", ld, "basicnarcotics");

            string m = "Machinery";
            AddCommodity("Articulation Motors;Atmospheric Processors;Building Fabricators;Crop Harvesters;Emergency Power Cells;Exhaust Manifold;Geological Equipment", m);
            AddCommodity("Heatsink Interlink;HN Shock Mount;Ion Distributor;Magnetic Emitter Coil;Marine Equipment", m);
            AddCommodity("Microbial Furnaces;Mineral Extractors;Modular Terminals;Power Converter;Power Generators;Power Transfer Bus", m);
            AddCommodity("Radiation Baffle;Reinforced Mounting Plate;Skimmer Components;Thermal Cooling Units;Water Purifiers", m);
            AddCommodity("Energy Grid Assembly", m, "powergridassembly");

            string md = "Medicines";
            AddCommodity("Advanced Medicines;Basic Medicines;Combat Stabilisers;Performance Enhancers;Progenitor Cells", md);
            AddCommodity("Agri-Medicines", md, "agriculturalmedicines");

            AddCommodity("Aluminium;Beryllium;Bismuth;Cobalt;Copper;Gallium;Gold;Hafnium 178;Indium;Lanthanum;Lithium;Osmium;Palladium;Platinum;Praseodymium;Samarium;Silver;Tantalum;Thallium;Thorium;Titanium;Uranium", "Metals");

            string mi = "Minerals";
            AddCommodity("Bauxite;Bertrandite;Bromellite;Coltan;Cryolite;Gallite;Goslarite;Methane Clathrate", mi);
            AddCommodity("Methanol Monohydrate", mi, "methanolmonohydratecrystals");
            AddCommodity("Indite;Jadeite;Lepidolite;Lithium Hydroxide;Moissanite;Painite;Pyrophyllite;Rutile;Taaffeite;Uraninite", mi);
            AddCommodity("Low Temperature Diamonds", mi, "lowtemperaturediamond");

            string sv = "Salvage";
            AddCommodity("Ai Relics;Ancient Artefact;Antimatter Containment Unit;Antiquities;Assault Plans;Black Box;Commercial Samples;Data Core;Diplomatic Bag;Encrypted Correspondence;Encrypted Data Storage;Experimental Chemicals;Fossil Remnants", sv);
            AddCommodity("Galactic Travel Guide;Geological Samples;Hostage;Military Intelligence;Military Plans;Mysterious Idol;Occupied CryoPod;Occupied Escape Pod;Personal Effects;Political Prisoner;Precious Gems;Prohibited Research Materials;Prototype Tech", sv);
            AddCommodity("Rare Artwork;Rebel Transmissions;Salvageable Wreckage;Sap 8 Core Container;Scientific Research;Scientific Samples;Space Pioneer Relics;Tactical Data;Technical Blueprints;Trade Data;Unknown Probe;Unstable Data Core", sv);
            AddCommodity("Large Survey Data Cache", sv, "largeexplorationdatacash");
            AddCommodity("Small Survey Data Cache", sv, "smallexplorationdatacash");
            AddCommodity("Small Survey Data Cache", sv, "smallexplorationdatacash");
            AddCommodity("Small Survey Data Cache", sv, "smallexplorationdatacash");

            AddCommodityO(sv, "AncientRelic", "Ancient Relic");
            AddCommodityO(sv, "AncientOrb","Ancient Orb");
            AddCommodityO(sv, "AncientCasket","Ancient Casket");
            AddCommodityO(sv, "AncientTablet", "Ancient Tablet");
            AddCommodityO(sv, "AncientUrn","Ancient Urn");
            AddCommodityO(sv, "AncientTotem","Ancient Totem");

            AddCommodity("Imperial Slaves;Slaves", "Slavery");

            string tc = "Technology";
            AddCommodity("Advanced Catalysers;Animal Monitors;Aquaponic Systems;Bioreducing Lichen;Computer Components", tc);
            AddCommodity("Auto-Fabricators", tc, "autofabricators");
            AddCommodity("Land Enrichment Systems;Medical Diagnostic Equipment;Micro Controllers", tc);
            AddCommodity("Nanobreakers;Resonating Separators;Robotics;Structural Regulators;Telemetry Suite",tc);
            AddCommodity("H.E. Suits", tc, "hazardousenvironmentsuits");
            AddCommodity("Hardware Diagnostic Sensor", tc, "diagnosticsensor");
            AddCommodity("Muon Imager", tc, "mutomimager");

            AddCommodity("Conductive Fabrics;Leather;Military Grade Fabrics;Natural Fabrics;Synthetic Fabrics", "Textiles");

            AddCommodity("Biowaste;Chemical Waste;Scrap;Toxic Waste", "Waste");

            string wp = "Weapons";
            AddCommodity("Battle Weapons;Landmines;Personal Weapons;Reactive Armour", wp);
            AddCommodity("Non-Lethal Weapons", wp, "nonlethalweapons");
           
            AddRare("aepyornisegg", "Aepyornis Egg"); //!

            AddRare("aerialedenapple", "Eden Apples of Aerial");
            AddRare("aganipperush", "Aganippe Rush");
            AddRare("alacarakmoskinart", "Alacarakmo Skin Art");
            AddRare("albinoquechuamammoth", "Albino Quechua Mammoth");
            AddRare("alieneggs", "Leathery Eggs");
            AddRare("altairianskin", "Altairian Skin");
            AddRare("alyabodilysoap", "Alya Body Soap");
            AddRare("anduligafireworks", "Anduliga Fire Works");
            AddRare("anynacoffee", "Any Na Coffee");
            AddRare("aroucaconventualsweets", "Arouca Conventual Sweets");
            AddRare("azcancriformula42", "Az Cancri Formula 42");
            AddRare("baltahsinevacuumkrill", "Baltah sine Vacuum Krill");
            AddRare("bankiamphibiousleather", "Banki Amphibious Leather");
            AddRare("bastsnakegin", "Bast Snake Gin");
            AddRare("belalansrayleather", "Belalans Ray Leather");
            AddRare("bluemilk", "Azure Milk");
            AddRare("borasetanipathogenetics", "Borasetani Pathogenetics");
            AddRare("burnhambiledistillate", "Burnham Bile Distillate");
            AddRare("cd75catcoffee", "CD-75 Kitten Brand Coffee");
            AddRare("centaurimegagin", "Centauri Mega Gin");
            AddRare("ceremonialheiketea", "Ceremonial Heike Tea");
            AddRare("cetirabbits", "Ceti Rabbits");
            AddRare("chameleoncloth", "Chameleon Cloth");
            AddRare("chateaudeaegaeon", "Chateau De Aegaeon");
            AddRare("cherbonesbloodcrystals", "Cherbones Blood Crystals");
            AddRare("chieridanimarinepaste", "Chi Eridani Marine Paste");
            AddRare("coquimspongiformvictuals", "Coquim Spongiform Victuals");
            AddRare("crystallinespheres", "Crystalline Spheres");
            AddRare("damnacarapaces", "Damna Carapaces");
            AddRare("deltaphoenicispalms", "Delta Phoenicis Palms");
            AddRare("deuringastruffles", "Deuringas Truffles");
            AddRare("disomacorn", "Diso Ma Corn");
            AddRare("eleuthermals", "Eleu Thermals");
            AddRare("eraninpearlwhisky", "Eranin Pearl Whiskey");
            AddRare("eshuumbrellas", "Eshu Umbrellas");
            AddRare("esusekucaviar", "Esuseku Caviar");
            AddRare("ethgrezeteabuds", "Ethgreze Tea Buds");
            AddRare("fujintea", "Fujin Tea");
            AddRare("galactictravelguide", "Galactic Travel Guide");
            AddRare("geawendancedust", "Geawen Dance Dust");
            AddRare("gerasiangueuzebeer", "Gerasian Gueuze Beer");
            AddRare("giantirukamasnails", "Giant Irukama Snails");
            AddRare("giantverrix", "Giant Verrix");
            AddRare("gilyasignatureweapons", "Gilya Signature Weapons");
            AddRare("gomanyauponcoffee", "Goman Yaupon Coffee");
            AddRare("haidneblackbrew", "Haidne Black Brew");
            AddRare("havasupaidreamcatcher", "Havasupai Dream Catcher");
            AddRare("helvetitjpearls", "Helvetitj Pearls");
            AddRare("hip10175bushmeat", "Hip 10175 Bush Meat");
            AddRare("hip118311swarm", "Hip 118311 Swarm");
            AddRare("hipproto-squid", "HIP Proto-Squid"); 
            AddRare("hiporganophosphates", "Hip Organophosphates");
            AddRare("holvaduellingblades", "Holva Duelling Blades");
            AddRare("honestypills", "Honesty Pills");
            AddRare("hr7221wheat", "HR 7221 Wheat");
            AddRare("indibourbon", "Indi Bourbon");
            AddRare("jaquesquinentianstill", "Jaques Quinentian Still");
            AddRare("jaradharrepuzzlebox", "Jaradharre Puzzle Box");
            AddRare("jarouarice", "Jaroua Rice");
            AddRare("jotunmookah", "Jotun Mookah");
            AddRare("kachiriginleaches", "Kachirigin Filter Leeches");
            AddRare("kamitracigars", "Kamitra Cigars");
            AddRare("kamorinhistoricweapons", "Kamorin Historic Weapons");
            AddRare("karetiicouture", "Karetii Couture");
            AddRare("karsukilocusts", "Karsuki Locusts");
            AddRare("kinagoviolins", "Kinago Violins");
            AddRare("konggaale", "Kongga Ale");
            AddRare("korrokungpellets", "Koro Kung Pellets");                   // Note that the (revised) identifier has two 'r's while the alias has one.
            AddRare("lavianbrandy", "Lavian Brandy");
            AddRare("leestianeviljuice", "Leestian Evil Juice");
            AddRare("voidextractcoffee", "Void Extract Coffee");
            AddRare("livehecateseaworms", "Live Hecate Sea Worms");
            AddRare("ltthypersweet", "Ltt Hypersweet");
            AddRare("lyraeweed", "Lyrae Weed");
            AddRare("masterchefs", "Master Chefs");
            AddRare("mechucoshightea", "Mechucos High Tea");
            AddRare("medbstarlube", "Medb Starlube");
            AddRare("mokojingbeastfeast", "Mokojing Beast Feast");
            AddRare("momusbogspaniel", "Momus Bog Spaniel");
            AddRare("motronaexperiencejelly", "Motrona Experience Jelly");
            AddRare("mukusubiichitin-os", "Mukusubii Chitin-Os");
            AddRare("mulachigiantfungus", "Mulachi Giant Fungus");
            AddRare("neritusberries", "Neritus Berries");
            AddRare("ngadandarifireopals", "Ngadandari Fire Opals");
            AddRare("ngunamodernantiques", "Nguna Modern Antiques");
            AddRare("njangarisaddles", "Njangari Saddles");
            AddRare("noneuclidianexotanks", "Non Euclidian Exotanks");
            AddRare("ochoengchillies", "Ochoeng Chillies");
            AddRare("onionhead", "Onion Head");
            AddRare("onionheadalphastrain", "Onionhead Alpha Strain");
            AddRare("onionheadbetastrain", "Onionhead Beta Strain");
            AddRare("ophiuchexinoartefacts", "Ophiuch Exino Artefacts");
            AddRare("orrerianviciousbrew", "Orrerian Vicious Brew");
            AddRare("pantaaprayersticks", "Pantaa Prayer Sticks");
            AddRare("pavoniseargrubs", "Pavonis Ear Grubs");
            AddRare("rajukrustoves", "Rajukru Multi-Stoves");
            AddRare("rapabaosnakeskins", "Rapa Bao Snake Skins");
            AddRare("rusanioldsmokey", "Rusani Old Smokey");
            AddRare("sanumameat", "Sanuma Decorative Meat");
            AddRare("saxonwine", "Saxon Wine");
            AddRare("soontillrelics", "Soontill Relics");
            AddRare("sothiscrystallinegold", "Sothis Crystalline Gold");
            AddRare("tanmarktranquiltea", "Tanmark Tranquil Tea");
            AddRare("tarachspice", "Tarach Spice");
            AddRare("taurichimes", "Tauri Chimes");
            AddRare("terramaterbloodbores", "Terra Mater Blood Bores");
            AddRare("thehuttonmug", "The Hutton Mug");
            AddRare("thrutiscream", "Thrutis Cream");
            AddRare("tiegfriessynthsilk", "Tiegfries Synth Silk");
            AddRare("tiolcewaste2pasteunits", "Tiolce Waste2paste Units");
            AddRare("toxandjivirocide", "Toxandji Virocide");
            AddRare("transgeniconionhead", "Lucan Onion Head");
            AddRare("uszaiantreegrub", "Uszaian Tree Grub");
            AddRare("utgaroarmillenialeggs", "Utgaroar Millennial Eggs");       // Note that the (revised) identifier has one 'n' while the alias has two.
            AddRare("uzumokulow-gwings", "Uzumoku Low-G Wings");
            AddRare("vanayequiceratomorphafur", "Vanayequi Ceratomorpha Fur");
            AddRare("vegaslimweed", "Vega Slimweed");
            AddRare("vherculisbodyrub", "V Herculis Body Rub");
            AddRare("vidavantianlace", "Vidavantian Lace");
            AddRare("volkhabbeedrones", "Volkhab Bee Drones");
            AddRare("watersofshintara", "Waters Of shintara");
            AddRare("wheemetewheatcakes", "Wheemete Wheat Cakes");
            AddRare("witchhaulkobebeef", "Witchhaul Kobe Beef");
            AddRare("wolffesh", "Wolf Fesh");
            AddRare("wulpahyperboresystems", "Wulpa Hyperbore Systems");
            AddRare("wuthielokufroth", "Wuthielo ku froth");
            AddRare("xihecompanions", "Xihe Biomorphic Companions");
            AddRare("yasokondileaf", "Yaso Kondi Leaf");
            AddRare("zeesszeantglue", "Zeessze Ant Grub Glue");
            AddRare("trinketsofhiddenfortune", "Trinkets Of Hidden Fortune");

            // very common data
            AddEnc( "Anomalous Bulk Scan Data", "Very Common", "ABSD", "bulkscandata");
            AddEnc( "Atypical Disrupted Wake Echoes", "Very Common", "ADWE", "disruptedwakeechoes");
            AddEnc( "Distorted Shield Cycle Recordings", "Very Common", "DSCR", "shieldcyclerecordings");
            AddEnc( "Exceptional Scrambled Emission Data", "Very Common", "ESED", "scrambledemissiondata");
            AddEnc( "Specialised Legacy Firmware", "Very Common", "SLF", "legacyfirmware");
            AddEnc( "Unusual Encrypted Files", "Very Common", "UEF", "encryptedfiles");
            // common data
            AddEnc( "Anomalous FSD Telemetry", "Common", "AFT", "fsdtelemetry");
            AddEnc( "Inconsistent Shield Soak Analysis", "Common", "ISSA", "shieldsoakanalysis");
            AddEnc( "Irregular Emission Data", "Common", "IED", "archivedemissiondata");
            AddEnc( "Modified Consumer Firmware", "Common", "MCF", "consumerfirmware");
            AddEnc( "Tagged Encryption Codes", "Common", "TEC", "encryptioncodes");
            AddEnc( "Unidentified Scan Archives", "Common", "USA", "scanarchives");
            AddEnc( "Pattern Beta Obelisk Data", "Common", "PBOD", "ancientculturaldata");
            AddEnc( "Pattern Gamma Obelisk Data", "Common", "PGOD", "ancienthistoricaldata");
            // standard data
            AddEnc( "Classified Scan Databanks", "Standard", "CSD", "scandatabanks");
            AddEnc( "Cracked Industrial Firmware", "Standard", "CIF", "industrialfirmware");
            AddEnc( "Open Symmetric Keys", "Standard", "OSK", "symmetrickeys");
            AddEnc( "Strange Wake Solutions", "Standard", "SWS", "wakesolutions");
            AddEnc( "Unexpected Emission Data", "Standard", "UED", "emissiondata");
            AddEnc( "Untypical Shield Scans", "Standard", "USS", "shielddensityreports");
            AddEnc( "Peculiar Shield Frequency Data", "Standard", "SFD", "shieldfrequencydata");
            AddEnc( "Classified Scan Fragment", "Standard", "CFSD", "classifiedscandata");
            AddEnc( "Abnormal Compact Emissions Data", "Standard", "CED", "compactemissionsdata");
            AddEnc( "Modified Embedded Firmware", "Standard", "EFW", "embeddedfirmware");
            AddEnc( "Pattern Alpha Obelisk Data", "Standard", "PAOD", "ancientbiologicaldata");
            // rare data
            AddEnc( "Aberrant Shield Pattern Analysis", "Rare", "ASPA", "shieldpatternanalysis");
            AddEnc( "Atypical Encryption Archives", "Rare", "AEA", "encryptionarchives");
            AddEnc( "Decoded Emission Data", "Rare", "DED");
            AddEnc( "Divergent Scan Data", "Rare", "DSD", "encodedscandata");
            AddEnc( "Eccentric Hyperspace Trajectories", "Rare", "EHT", "hyperspacetrajectories");
            AddEnc( "Security Firmware Patch", "Rare", "SFP", "securityfirmware");
            AddEnc( "Pattern Delta Obelisk Data", "Rare", "PDOD", "ancientlanguagedata");
            // very rare data
            AddEnc( "Adaptive Encryptors Capture", "Very Rare", "AEC", "adaptiveencryptors");
            AddEnc( "Datamined Wake Exceptions", "Very Rare", "DWEx", "dataminedwake");
            AddEnc( "Peculiar Shield Frequency Data", "Very Rare", "PSFD", "shieldfrequencydata");
            AddEnc( "Pattern Epsilon Obelisk Data", "Very Rare", "PSFD", "ancienttechnologicaldata");
            //very common manufactured
            AddManu( "Basic Conductors", "Very Common", "BaC");
            AddManu( "Chemical Storage Units", "Very Common", "CSU");
            AddManu( "Compact Composites", "Very Common", "CC");
            AddManu( "Crystal Shards", "Very Common", "CS");
            AddManu( "Grid Resistors", "Very Common", "GR");
            AddManu( "Heat Conduction Wiring", "Very Common", "HCW");
            AddManu( "Mechanical Scrap", "Very Common", "MS");
            AddManu( "Salvaged Alloys", "Very Common", "SAll");
            AddManu( "Worn Shield Emitters", "Very Common", "WSE");
            AddManu( "Thermic Alloys", "Very Common", "ThA");
            // common manufactured
            AddManu("Chemical Processors", "Common", "CP");
            AddManu("Conductive Components", "Common", "CCo");
            AddManu("Filament Composites", "Common", "FiC");
            AddManu("Flawed Focus Crystals", "Common", "FFC", "uncutfocuscrystals");
            AddManu("Galvanising Alloys", "Common", "GA");
            AddManu("Heat Dispersion Plate", "Common", "HDP");
            AddManu("Heat Resistant Ceramics", "Common", "HRC");
            AddManu("Hybrid Capacitors", "Common", "HC");
            AddManu("Mechanical Equipment", "Common", "ME");
            AddManu("Shield Emitters", "Common", "SE");

            // standard manufactured
            AddManu( "Chemical Distillery", "Standard", "CD");
            AddManu( "Conductive Ceramics", "Standard", "CCe");
            AddManu( "Electrochemical Arrays", "Standard", "EA");
            AddManu( "Focus Crystals", "Standard", "FoC");
            AddManu( "Heat Exchangers", "Standard", "HE");
            AddManu( "High Density Composites", "Standard", "HDC");
            AddManu( "Mechanical Components", "Standard", "MC");
            AddManu( "Phase Alloys", "Standard", "PA");
            AddManu( "Precipitated Alloys", "Standard", "PAll");
            AddManu( "Shielding Sensors", "Standard", "SS");
            // rare manufactured
            AddManu( "Chemical Manipulators", "Rare", "CM");
            AddManu( "Compound Shielding", "Rare", "CoS");
            AddManu( "Conductive Polymers", "Rare", "CPo");
            AddManu( "Configurable Components", "Rare", "CCom");
            AddManu( "Heat Vanes", "Rare", "HV");
            AddManu( "Polymer Capacitors", "Rare", "PCa");
            AddManu( "Proto Light Alloys", "Rare", "PLA");
            AddManu( "Refined Focus Crystals", "Rare", "RFC");
            AddManu( "Tempered Alloys", "Rare", "TeA");
            AddManu( "Proprietary Composites", "Rare", "FPC", "fedproprietarycomposites");
            // very rare manufactured
            AddManu( "Core Dynamics Composites", "Very Rare", "FCC", "fedcorecomposites");
            AddManu( "Biotech Conductors", "Very Rare", "BiC");
            AddManu( "Exquisite Focus Crystals", "Very Rare", "EFC");
            AddManu( "Imperial Shielding", "Very Rare", "IS");
            AddManu( "Improvised Components", "Very Rare", "IC");
            AddManu( "Military Grade Alloys", "Very Rare", "MGA");
            AddManu( "Military Supercapacitors", "Very Rare", "MSC");
            AddManu( "Pharmaceutical Isolators", "Very Rare", "PI");
            AddManu( "Proto Heat Radiators", "Very Rare", "PHR");
            AddManu( "Proto Radiolic Alloys", "Very Rare", "PRA");

            //Unknowns, data from INARA and cometbourne, July 17
            // Renamed in 2.4
            AddRare("unknownartifact", "Thargoid Sensor");
            AddRare("unknownprobe", "Thargoid Probe");
            AddRare("unknownlink", "Thargoid Link");
            AddRare("unknownbiologicalmatter", "Thargoid Biological Matter");
            AddRare("unknownresin", "Thargoid Resin");
            AddRare("unknowntechnologysamples", "Thargoid Technology Samples");

            AddManu("Thargoid Carapace", "Common", "UKCP", "unknowncarapace");
            AddManu("Thargoid Energy Cell", "Standard", "UKEC", "unknownenergycell");
            AddManu("Thargoid Organic Circuitry", "Very Rare", "UKOC", "unknownorganiccircuitry");
            AddManu("Thargoid Technology Components", "Rare", "UKTC", "unknowntechnologycomponents");
            AddManu("Sensor Fragment", "Very Rare", "UES", "unknownenergysource");

            AddEnc("Thargoid Material Composition Data", "Standard", "UMCD", "tg_compositiondata");
            AddEnc("Thargoid Structural Data", "Common", "UKSD", "tg_structuraldata");
            AddEnc("Thargoid Residue Data", "Rare", "URDA", "tg_residuedata");
            AddEnc("Thargoid Ship Signature", "Standard", "USSig", "unknownshipsignature");
            AddEnc("Thargoid Wake Data", "Rare", "UWD", "unknownwakedata");

            // INARA - no idea what the FD IDs are

            //Unknown Material Composition Data   Standard
            //Unknown Residue Data Analysis Rare
            //Unknown Structural Data Common
            //Unknown Technology Components


            //CheckAnthor(); // Check here..
            //CheckEDData();

            // beyond Anthor but seen in logs
            AddCommodity("Drones", "Drones");

            //foreach (MaterialCommodityDB d in cachelist.Values) System.Diagnostics.Debug.WriteLine(string.Format("{0},{1},{2},{3}", d.fdname, d.name, d.category, d.type));
        }


        static Dictionary<string, string> fdnamemangling = new Dictionary<string, string>() // Key: old_identifier, Value: new_identifier
        {
            //2.2 to 2.3 changed some of the identifier names.. change the 2.2 ones to 2.3!  Anthor data from his materials db file

            { "aberrantshieldpatternanalysis"       ,  "shieldpatternanalysis" },
            { "adaptiveencryptorscapture"           ,  "adaptiveencryptors" },
            { "alyabodysoap"                        ,  "alyabodilysoap" },
            { "anomalousbulkscandata"               ,  "bulkscandata" },
            { "anomalousfsdtelemetry"               ,  "fsdtelemetry" },
            { "atypicaldisruptedwakeechoes"         ,  "disruptedwakeechoes" },
            { "atypicalencryptionarchives"          ,  "encryptionarchives" },
            { "azuremilk"                           ,  "bluemilk" },
            { "cd-75kittenbrandcoffee"              ,  "cd75catcoffee" },
            { "crackedindustrialfirmware"           ,  "industrialfirmware" },
            { "dataminedwakeexceptions"             ,  "dataminedwake" },
            { "distortedshieldcyclerecordings"      ,  "shieldcyclerecordings" },
            { "eccentrichyperspacetrajectories"     ,  "hyperspacetrajectories" },
            { "edenapplesofaerial"                  ,  "aerialedenapple" },
            { "eraninpearlwhiskey"                  ,  "eraninpearlwhisky" },
            { "exceptionalscrambledemissiondata"    ,  "scrambledemissiondata" },
            { "inconsistentshieldsoakanalysis"      ,  "shieldsoakanalysis" },
            { "kachiriginfilterleeches"             ,  "kachiriginleaches" },
            { "korokungpellets"                     ,  "korrokungpellets" },
            { "leatheryeggs"                        ,  "alieneggs" },
            { "lucanonionhead"                      ,  "transgeniconionhead" },
            { "modifiedconsumerfirmware"            ,  "consumerfirmware" },
            { "modifiedembeddedfirmware"            ,  "embeddedfirmware" },
            { "opensymmetrickeys"                   ,  "symmetrickeys" },
            { "peculiarshieldfrequencydata"         ,  "shieldfrequencydata" },
            { "rajukrumulti-stoves"                 ,  "rajukrustoves" },
            { "sanumadecorativemeat"                ,  "sanumameat" },
            { "securityfirmwarepatch"               ,  "securityfirmware" },
            { "specialisedlegacyfirmware"           ,  "legacyfirmware" },
            { "strangewakesolutions"                ,  "wakesolutions" },
            { "taggedencryptioncodes"               ,  "encryptioncodes" },
            { "unidentifiedscanarchives"            ,  "scanarchives" },
            { "unusualencryptedfiles"               ,  "encryptedfiles" },
            { "utgaroarmillennialeggs"              ,  "utgaroarmillenialeggs" },
            { "xihebiomorphiccompanions"            ,  "xihecompanions" },
            { "zeesszeantgrubglue"                  ,  "zeesszeantglue" },

            //aliases found in commodities files from Anthor, change to last name in list.
            {"micro-weavecoolinghoses","coolinghoses"},
            {"atmosphericextractors","atmosphericprocessors"},
            {"marinesupplies","marineequipment"},
            {"heliostaticfurnaces","microbialfurnaces"},
            {"skimercomponents","skimmercomponents"},
            {"energygridassembly","powergridassembly"},
            {"powertransferconduits","powertransferbus"},
            {"methanolmonohydrate","methanolmonohydratecrystals"},
            {"terrainenrichmentsystems","landenrichmentsystems"},
            {"muonimager","mutomimager"},
            {"hardwarediagnosticsensor","diagnosticsensor"},
            {"usscargotradedata","tradedata"},
            {"usscargoblackbox","blackbox"},
            {"usscargomilitaryplans","militaryplans"},
            {"usscargoancientartefact","ancientartefact"},
            {"usscargorareartwork","rareartwork"},
            {"usscargoexperimentalchemicals","experimentalchemicals"},
            {"usscargorebeltransmissions","rebeltransmissions"},
            {"usscargoprototypetech","prototypetech"},
            {"usscargotechnicalblueprints","technicalblueprints"},
            {"unknownartifact2","unknownprobe"},
            {"unknownartifact3","unknownlink"},

            //From logs
            {"encripteddatastorage", "encrypteddatastorage" },
        };

        static public string FDNameTranslation(string old)
        {
            old = old.ToLower();
            if (fdnamemangling.ContainsKey(old))
            {
                //System.Diagnostics.Debug.WriteLine("Sub " + old);
                return fdnamemangling[old];
            }
            else
                return old;
        }



        #endregion

#if false

        #region Anthor check 30 may 2017        
        // **** KEEP THIS, We will periodically ask Anthor for his files and then we can double check. 

        // Or, just review the second ~half of https://github.com/EDSM-NET/Alias/blob/master/Station/Commodity/Type.php

        // WARNING !!!!

        // WARNING !!!!

        // WARNING !!!!

        // WARNING !!!!

        // Use Robby ProcessAnthorData program to generate these tables from Anthors PHP - don't do it manually.!!!!!!!!!!!!!!!!

        class Alias {
            public string fdname; public string alias; public int num;
            public Alias(string f, string a, int n ) { fdname = f;alias = a;num = n; }
        };


        static List<Alias> EDSM_User_Alias_Materials = new List<Alias>() {
            new Alias("antimony","Antimony",1),
            new Alias("arsenic","Arsenic",2),
            new Alias("basicconductors","Basic Conductors",3),
            new Alias("biotechconductors","Biotech Conductors",4),
            new Alias("cadmium","Cadmium",5),
            new Alias("carbon","Carbon",6),
            new Alias("chemicaldistillery","Chemical Distillery",7),
            new Alias("chemicalmanipulators","Chemical Manipulators",8),
            new Alias("chemicalprocessors","Chemical Processors",9),
            new Alias("chemicalstorageunits","Chemical Storage Units",10),
            new Alias("chromium","Chromium",11),
            new Alias("compactcomposites","Compact Composites",12),
            new Alias("compoundshielding","Compound Shielding",13),
            new Alias("conductiveceramics","Conductive Ceramics",14),
            new Alias("conductivecomponents","Conductive Components",15),
            new Alias("conductivepolymers","Conductive Polymers",16),
            new Alias("configurablecomponents","Configurable Components",17),
            new Alias("fedcorecomposites","Core Dynamics Composites",18),
            new Alias("crystalshards","Crystal Shards",19),
            new Alias("electrochemicalarrays","Electrochemical Arrays",20),
            new Alias("exquisitefocuscrystals","Exquisite Focus Crystals",21),
            new Alias("filamentcomposites","Filament Composites",22),
            new Alias("uncutfocuscrystals","Flawed Focus Crystals",23),
            new Alias("focuscrystals","Focus Crystals",24),
            new Alias("galvanisingalloys","Galvanising Alloys",25),
            new Alias("germanium","Germanium",26),
            new Alias("gridresistors","Grid Resistors",27),
            new Alias("heatconductionwiring","Heat Conduction Wiring",28),
            new Alias("heatdispersionplate","Heat Dispersion Plate",29),
            new Alias("heatexchangers","Heat Exchangers",30),
            new Alias("heatresistantceramics","Heat Resistant Ceramics",31),
            new Alias("heatvanes","Heat Vanes",32),
            new Alias("highdensitycomposites","High Density Composites",33),
            new Alias("hybridcapacitors","Hybrid Capacitors",34),
            new Alias("imperialshielding","Imperial Shielding",35),
            new Alias("improvisedcomponents","Improvised Components",36),
            new Alias("iron","Iron",37),
            new Alias("manganese","Manganese",38),
            new Alias("mechanicalcomponents","Mechanical Components",39),
            new Alias("mechanicalequipment","Mechanical Equipment",40),
            new Alias("mechanicalscrap","Mechanical Scrap",41),
            new Alias("mercury","Mercury",42),
            new Alias("militarygradealloys","Military Grade Alloys",43),
            new Alias("militarysupercapacitors","Military Supercapacitors",44),
            new Alias("molybdenum","Molybdenum",45),
            new Alias("nickel","Nickel",46),
            new Alias("niobium","Niobium",47),
            new Alias("pharmaceuticalisolators","Pharmaceutical Isolators",48),
            new Alias("phasealloys","Phase Alloys",49),
            new Alias("phosphorus","Phosphorus",50),
            new Alias("polonium","Polonium",51),
            new Alias("polymercapacitors","Polymer Capacitors",52),
            new Alias("precipitatedalloys","Precipitated Alloys",53),
            new Alias("fedproprietarycomposites","Proprietary Composites",54),
            new Alias("protoheatradiators","Proto Heat Radiators",55),
            new Alias("protolightalloys","Proto Light Alloys",56),
            new Alias("protoradiolicalloys","Proto Radiolic Alloys",57),
            new Alias("refinedfocuscrystals","Refined Focus Crystals",58),
            new Alias("ruthenium","Ruthenium",59),
            new Alias("salvagedalloys","Salvaged Alloys",60),
            new Alias("selenium","Selenium",61),
            new Alias("shieldemitters","Shield Emitters",62),
            new Alias("shieldingsensors","Shielding Sensors",63),
            new Alias("sulphur","Sulphur",64),
            new Alias("technetium","Technetium",65),
            new Alias("tellurium","Tellurium",66),
            new Alias("temperedalloys","Tempered Alloys",67),
            new Alias("thermicalloys","Thermic Alloys",68),
            new Alias("tin","Tin",69),
            new Alias("tungsten","Tungsten",70),
            new Alias("unknownenergysource","Unknown Fragment",71),
            new Alias("vanadium","Vanadium",72),
            new Alias("wornshieldemitters","Worn Shield Emitters",73),
            new Alias("yttrium","Yttrium",74),
            new Alias("zinc","Zinc",75),
            new Alias("zirconium","Zirconium",76),
        };
        static List<Alias> EDSM_User_Alias_Data = new List<Alias>() {
            new Alias("shieldpatternanalysis","Aberrant Shield Pattern Analysis",1),
            new Alias("compactemissionsdata","Abnormal Compact Emission Data",2),
            new Alias("adaptiveencryptors","Adaptive Encryptors Capture",3),
            new Alias("bulkscandata","Anomalous Bulk Scan Data",4),
            new Alias("fsdtelemetry","Anomalous FSD Telemetry",5),
            new Alias("disruptedwakeechoes","Atypical Disrupted Wake Echoes",6),
            new Alias("encryptionarchives","Atypical Encryption Archives",7),
            new Alias("scandatabanks","Classified Scan Databanks",8),
            new Alias("classifiedscandata","Classified Scan Fragment",9),
            new Alias("industrialfirmware","Cracked Industrial Firmware",10),
            new Alias("dataminedwake","Datamined Wake Exceptions",11),
            new Alias("decodedemissiondata","Decoded Emission Data",12),
            new Alias("shieldcyclerecordings","Distorted Shield Cycle Recordings",13),
            new Alias("encodedscandata","Divergent Scan Data",14),
            new Alias("hyperspacetrajectories","Eccentric Hyperspace Trajectories",15),
            new Alias("scrambledemissiondata","Exceptional Scrambled Emission Data",16),
            new Alias("shieldsoakanalysis","Inconsistent Shield Soak Analysis",17),
            new Alias("archivedemissiondata","Irregular Emission Data",18),
            new Alias("consumerfirmware","Modified Consumer Firmware",19),
            new Alias("embeddedfirmware","Modified Embedded Firmware",20),
            new Alias("symmetrickeys","Open Symmetric Keys",21),
            new Alias("ancientbiologicaldata","Pattern Alpha Obelisk Data",22),
            new Alias("ancientculturaldata","Pattern Beta Obelisk Data",23),
            new Alias("ancientlanguagedata","Pattern Delta Obelisk Data",24),
            new Alias("ancienttechnologicaldata","Pattern Epsilon Obelisk Data",25),
            new Alias("ancienthistoricaldata","Pattern Gamma Obelisk Data",26),
            new Alias("shieldfrequencydata","Peculiar Shield Frequency Data",27),
            new Alias("securityfirmware","Security Firmware Patch",28),
            new Alias("legacyfirmware","Specialised Legacy Firmware",29),
            new Alias("wakesolutions","Strange Wake Solutions",30),
            new Alias("encryptioncodes","Tagged Encryption Codes",31),
            new Alias("emissiondata","Unexpected Emission Data",32),
            new Alias("scanarchives","Unidentified Scan Archives",33),
            new Alias("unknownenergysource","Unknown Fragment",34),
            new Alias("shielddensityreports","Untypical Shield Scans",35),
            new Alias("encryptedfiles","Unusual Encrypted Files",36),
            new Alias("unknownshipsignature","Unknown Ship Signature",37),
            new Alias("unknownwakedata","Unknown Wake Data",38),
        };

        static string getGradeMat(string alias)
        {
            int number = EDSM_User_Alias_Materials.Find(x => x.alias.Equals(alias)).num;

            if (Array.IndexOf(new int[] { 1, 4, 18, 21, 35, 36, 43, 44, 48, 51, 55, 57, 59, 65, 66, 71 }, number) >= 0)
            {
                return "Very Rare";
            }

            if (Array.IndexOf(new int[] { 5, 8, 13, 16, 17, 32, 42, 45, 52, 54, 56, 58, 67, 69, 74 }, number) >= 0)
            {
                return "Rare";
            }

            if (Array.IndexOf(new int[] { 2, 7, 14, 20, 24, 30, 33, 39, 47, 49, 53, 61, 63, 70, 76 }, number) >= 0)
            {
                return "Standard";
            }

            if (Array.IndexOf(new int[] { 9, 11, 15, 22, 23, 25, 26, 29, 31, 34, 38, 40, 62, 72, 75 }, number) >= 0)
            {
                return "Common";
            }

            return "Very Common";
        }


        static string getGradeData(string alias)
        {
            int number = EDSM_User_Alias_Data.Find(x => x.alias.Equals(alias)).num;

            if (Array.IndexOf(new int[] { 2, 3, 9, 11, 20, 25, 27, 38 }, number) >= 0)
            {
                return "Very Rare";
            }

            if (Array.IndexOf(new int[] { 1, 7, 12, 14, 15, 24, 28, 37 }, number) >= 0)
            {
                return "Rare";
            }

            if (Array.IndexOf(new int[] { 8, 10, 21, 22, 30, 32, 35 }, number) >= 0)
            {
                return "Standard";
            }

            if (Array.IndexOf(new int[] { 5, 17, 18, 19, 23, 31, 33 }, number) >= 0)
            {
                return "Common";
            }

            return "Very Common";
        }

        static List<Alias> EDSM_System_Station_Commodities_Alias_Type = new List<Alias>() {
            new Alias("explosives","Explosives",101),
            new Alias("hydrogenfuel","Hydrogen Fuel",102),
            new Alias("mineraloil","Mineral Oil",103),
            new Alias("pesticides","Pesticides",104),
            new Alias("syntheticreagents","Synthetic Reagents",105),
            new Alias("surfacestabilisers","Surface Stabilisers",106),
            new Alias("nerveagents","Nerve Agents",107),
            new Alias("deltaphoenicispalms","Delta Phoenicis Palms",108),
            new Alias("toxandjivirocide","Toxandji Virocide",109),
            new Alias("anduligafireworks","Anduliga Fire Works",110),
            new Alias("hiporganophosphates","HIP Organophosphates",111),
            new Alias("korokungpellets","Koro Kung Pellets",112),
            new Alias("water","Water",113),
            new Alias("hydrogenperoxide","Hydrogen Peroxide",114),
            new Alias("liquidoxygen","Liquid Oxygen",115),
            new Alias("clothing","Clothing",201),
            new Alias("consumertechnology","Consumer Technology",202),
            new Alias("domesticappliances","Domestic Appliances",203),
            new Alias("evacuationshelter","Evacuation Shelter",204),
            new Alias("alacarakmoskinart","Alacarakmo Skin Art",205),
            new Alias("eleuthermals","Eleu Thermals",206),
            new Alias("eshuumbrellas","Eshu Umbrellas",207),
            new Alias("karetiicouture","Karetii Couture",208),
            new Alias("njangarisaddles","Njangari Saddles",209),
            new Alias("kinagoviolins","Kinago Violins",210),
            new Alias("ngunamodernantiques","Nguna Modern Antiques",211),
            new Alias("rajukrumulti-stoves","Rajukru Multi-Stoves",212),
            new Alias("tiolcewaste2pasteunits","Tiolce Waste2Paste Units",213),
            new Alias("ophiuchexinoartefacts","Ophiuch Exino Artefacts",214),
            new Alias("havasupaidreamcatcher","Havasupai Dream Catcher",215),
            new Alias("jaradharrepuzzlebox","Jaradharre Puzzle Box",216),
            new Alias("uzumokulow-gwings","Uzumoku Low-G Wings",217),
            new Alias("altairianskin","Altairian Skin",218),
            new Alias("jotunmookah","Jotun Mookah",219),
            new Alias("zeesszeantgrubglue","Zeessze Ant Grub Glue",220),
            new Alias("momusbogspaniel","Momus Bog Spaniel",221),
            new Alias("leatheryeggs","Leathery Eggs",222),
            new Alias("vidavantianlace","Vidavantian Lace",223),
            new Alias("jaquesquinentianstill","Jaques Quinentian Still",224),
            new Alias("soontillrelics","Soontill Relics",225),
            new Alias("thehuttonmug","The Hutton Mug",226),
            new Alias("crystallinespheres","Crystalline Spheres",227),
            new Alias("survivalequipment","Survival Equipment",228),
            new Alias("beer","Beer",301),
            new Alias("liquor","Liquor",302),
            new Alias("basicnarcotics","Narcotics",303),
            new Alias("tobacco","Tobacco",304),
            new Alias("wine","Wine",305),
            new Alias("eraninpearlwhiskey","Eranin Pearl Whiskey",306),
            new Alias("lucanonionhead","Lucan Onion Head",307),
            new Alias("motronaexperiencejelly","Motrona Experience Jelly",308),
            new Alias("onionhead","Onion Head",309),
            new Alias("rusanioldsmokey","Rusani Old Smokey",310),
            new Alias("tarachspice","Tarach Spice",311),
            new Alias("wolffesh","Wolf Fesh",312),
            new Alias("wuthielokufroth","Wuthielo Ku Froth",313),
            new Alias("bootlegliquor","Bootleg Liquor",314),
            new Alias("lavianbrandy","Lavian Brandy",315),
            new Alias("lyraeweed","Lyrae Weed",316),
            new Alias("konggaale","Kongga Ale",317),
            new Alias("bastsnakegin","Bast Snake Gin",318),
            new Alias("thrutiscream","Thrutis Cream",319),
            new Alias("kamitracigars","Kamitra Cigars",320),
            new Alias("yasokondileaf","Yaso Kondi Leaf",321),
            new Alias("chateaudeaegaeon","Chateau De Aegaeon",322),
            new Alias("saxonwine","Saxon Wine",323),
            new Alias("centaurimegagin","Centauri Mega Gin",324),
            new Alias("geawendancedust","Geawen Dance Dust",325),
            new Alias("gerasiangueuzebeer","Gerasian Gueuze Beer",326),
            new Alias("burnhambiledistillate","Burnham Bile Distillate",327),
            new Alias("pavoniseargrubs","Pavonis Ear Grubs",328),
            new Alias("indibourbon","Indi Bourbon",329),
            new Alias("leestianeviljuice","Leestian Evil Juice",330),
            new Alias("azuremilk","Azure Milk",331),
            new Alias("onionheadalphastrain","Onionhead Alpha Strain",332),
            new Alias("onionheadbetastrain","Onionhead Beta Strain",333),
            new Alias("algae","Algae",401),
            new Alias("animalmeat","Animal Meat",402),
            new Alias("coffee","Coffee",403),
            new Alias("fish","Fish",404),
            new Alias("foodcartridges","Food Cartridges",405),
            new Alias("fruitandvegetables","Fruit and Vegetables",406),
            new Alias("grain","Grain",407),
            new Alias("syntheticmeat","Synthetic Meat",408),
            new Alias("tea","Tea",409),
            new Alias("hip10175bushmeat","HIP 10175 Bush Meat",410),
            new Alias("albinoquechuamammoth","Albino Quechua Mammoth",411),
            new Alias("utgaroarmillennialeggs","Utgaroar Millennial Eggs",412),
            new Alias("witchhaulkobebeef","Witchhaul Kobe Beef",413),
            new Alias("karsukilocusts","Karsuki Locusts",414),
            new Alias("giantirukamasnails","Giant Irukama Snails",415),
            new Alias("baltahsinevacuumkrill","Baltah Sine Vacuum Krill",416),
            new Alias("cetirabbits","Ceti Rabbits",417),
            new Alias("anynacoffee","Any Na Coffee",418),
            new Alias("cd-75kittenbrandcoffee","CD-75 Kitten Brand Coffee",419),
            new Alias("gomanyauponcoffee","Goman Yaupon Coffee",420),
            new Alias("chieridanimarinepaste","Chi Eridani Marine Paste",421),
            new Alias("esusekucaviar","Esuseku Caviar",422),
            new Alias("livehecateseaworms","Live Hecate Sea Worms",423),
            new Alias("helvetitjpearls","Helvetitj Pearls",424),
            new Alias("hipproto-squid","HIP Proto-Squid",425),
            new Alias("coquimspongiformvictuals","Coquim Spongiform Victuals",426),
            new Alias("edenapplesofaerial","Eden Apples Of Aerial",427),
            new Alias("neritusberries","Neritus Berries",428),
            new Alias("ochoengchillies","Ochoeng Chillies",429),
            new Alias("deuringastruffles","Deuringas Truffles",430),
            new Alias("hr7221wheat","HR 7221 Wheat",431),
            new Alias("jarouarice","Jaroua Rice",432),
            new Alias("sanumadecorativemeat","Sanuma Decorative Meat",433),
            new Alias("ethgrezeteabuds","Ethgreze Tea Buds",434),
            new Alias("ceremonialheiketea","Ceremonial Heike Tea",435),
            new Alias("tanmarktranquiltea","Tanmark Tranquil Tea",436),
            new Alias("aepyornisegg","Aepyornis Egg",437),
            new Alias("haidneblackbrew","Haidne Black Brew",438),
            new Alias("voidextractcoffee","Void Extract Coffee",439),
            new Alias("ltthypersweet","LTT Hypersweet",440),
            new Alias("mechucoshightea","Mechucos High Tea",441),
            new Alias("mokojingbeastfeast","Mokojing Beast Feast",442),
            new Alias("mukusubiichitin-os","Mukusubii Chitin-Os",443),
            new Alias("mulachigiantfungus","Mulachi Giant Fungus",444),
            new Alias("wheemetewheatcakes","Wheemete Wheat Cakes",445),
            new Alias("aroucaconventualsweets","Arouca Conventual Sweets",446),
            new Alias("orrerianviciousbrew","Orrerian Vicious Brew",447),
            new Alias("uszaiantreegrub","Uszaian Tree Grub",448),
            new Alias("disomacorn","Diso Ma Corn",449),
            new Alias("polymers","Polymers",501),
            new Alias("semiconductors","Semiconductors",502),
            new Alias("superconductors","Superconductors",503),
            new Alias("metaalloys","Meta-Alloys",504),
            new Alias("ceramiccomposites","Ceramic Composites",505),
            new Alias("medbstarlube","Medb Starlube",506),
            new Alias("insulatingmembrane","Insulating Membrane",507),
            new Alias("cmmcomposite","CMM Composite",508),
            new Alias("coolinghoses","Micro-Weave Cooling Hoses",509),
            new Alias("neofabricinsulation","Neofabric Insulation",510),
            new Alias("atmosphericprocessors","Atmospheric Processors",601),
            new Alias("cropharvesters","Crop Harvesters",602),
            new Alias("marineequipment","Marine Equipment",603),
            new Alias("microbialfurnaces","Microbial Furnaces",604),
            new Alias("mineralextractors","Mineral Extractors",605),
            new Alias("powergenerators","Power Generators",606),
            new Alias("waterpurifiers","Water Purifiers",607),
            new Alias("thermalcoolingunits","Thermal Cooling Units",608),
            new Alias("skimmercomponents","Skimmer Components",609),
            new Alias("geologicalequipment","Geological Equipment",610),
            new Alias("buildingfabricators","Building Fabricators",611),
            new Alias("volkhabbeedrones","Volkhab Bee Drones",612),
            new Alias("wulpahyperboresystems","Wulpa Hyperbore Systems",613),
            new Alias("noneuclidianexotanks","Non Euclidian Exotanks",614),
            new Alias("giantverrix","Giant Verrix",615),
            new Alias("articulationmotors","Articulation Motors",616),
            new Alias("hnshockmount","HN Shock Mount",617),
            new Alias("emergencypowercells","Emergency Power Cells",618),
            new Alias("powerconverter","Power Converter",619),
            new Alias("powergridassembly","Energy Grid Assembly",620),
            new Alias("powertransferbus","Power Transfer Bus",621),
            new Alias("radiationbaffle","Radiation Baffle",622),
            new Alias("exhaustmanifold","Exhaust Manifold",623),
            new Alias("reinforcedmountingplate","Reinforced Mounting Plate",624),
            new Alias("heatsinkinterlink","Heatsink Interlink",625),
            new Alias("magneticemittercoil","Magnetic Emitter Coil",626),
            new Alias("modularterminals","Modular Terminals",627),
            new Alias("agriculturalmedicines","Agri-Medicines",701),
            new Alias("basicmedicines","Basic Medicines",702),
            new Alias("combatstabilisers","Combat Stabilisers",703),
            new Alias("performanceenhancers","Performance Enhancers",704),
            new Alias("progenitorcells","Progenitor Cells",705),
            new Alias("terramaterbloodbores","Terra Mater Blood Bores",706),
            new Alias("kachiriginfilterleeches","Kachirigin Filter Leeches",707),
            new Alias("aganipperush","Aganippe Rush",708),
            new Alias("watersofshintara","Waters Of Shintara",709),
            new Alias("honestypills","Honesty Pills",710),
            new Alias("vherculisbodyrub","V Herculis Body Rub",711),
            new Alias("vegaslimweed","Vega Slimweed",712),
            new Alias("taurichimes","Tauri Chimes",713),
            new Alias("pantaaprayersticks","Pantaa Prayer Sticks",714),
            new Alias("fujintea","Fujin Tea",715),
            new Alias("alyabodysoap","Alya Body Soap",716),
            new Alias("advancedmedicines","Advanced Medicines",717),
            new Alias("aluminium","Aluminium",801),
            new Alias("beryllium","Beryllium",802),
            new Alias("cobalt","Cobalt",803),
            new Alias("copper","Copper",804),
            new Alias("gallium","Gallium",805),
            new Alias("gold","Gold",806),
            new Alias("indium","Indium",807),
            new Alias("lithium","Lithium",808),
            new Alias("palladium","Palladium",809),
            new Alias("platinum","Platinum",810),
            new Alias("silver","Silver",811),
            new Alias("tantalum","Tantalum",812),
            new Alias("titanium","Titanium",813),
            new Alias("uranium","Uranium",814),
            new Alias("osmium","Osmium",815),
            new Alias("thorium","Thorium",816),
            new Alias("thallium","Thallium",817),
            new Alias("lanthanum","Lanthanum",818),
            new Alias("bismuth","Bismuth",819),
            new Alias("hafnium178","Hafnium 178",820),
            new Alias("sothiscrystallinegold","Sothis Crystalline Gold",821),
            new Alias("praseodymium","Praseodymium",822),
            new Alias("samarium","Samarium",823),
            new Alias("bauxite","Bauxite",901),
            new Alias("bertrandite","Bertrandite",902),
            new Alias("coltan","Coltan",903),
            new Alias("gallite","Gallite",904),
            new Alias("indite","Indite",905),
            new Alias("lepidolite","Lepidolite",906),
            new Alias("rutile","Rutile",907),
            new Alias("uraninite","Uraninite",908),
            new Alias("painite","Painite",909),
            new Alias("pyrophyllite","Pyrophyllite",910),
            new Alias("moissanite","Moissanite",911),
            new Alias("goslarite","Goslarite",912),
            new Alias("cryolite","Cryolite",913),
            new Alias("cherbonesbloodcrystals","Cherbones Blood Crystals",914),
            new Alias("ngadandarifireopals","Ngadandari Fire Opals",915),
            new Alias("taaffeite","Taaffeite",916),
            new Alias("jadeite","Jadeite",917),
            new Alias("bromellite","Bromellite",918),
            new Alias("lowtemperaturediamond","Low Temperature Diamonds",919),
            new Alias("methanolmonohydratecrystals","Methanol Monohydrate",920),
            new Alias("lithiumhydroxide","Lithium Hydroxide",921),
            new Alias("methaneclathrate","Methane Clathrate",922),
            new Alias("imperialslaves","Imperial Slaves",1001),
            new Alias("slaves","Slaves",1002),
            new Alias("masterchefs","Master Chefs",1003),
            new Alias("advancedcatalysers","Advanced Catalysers",1101),
            new Alias("animalmonitors","Animal Monitors",1102),
            new Alias("aquaponicsystems","Aquaponic Systems",1103),
            new Alias("autofabricators","Auto-Fabricators",1104),
            new Alias("bioreducinglichen","Bioreducing Lichen",1105),
            new Alias("computercomponents","Computer Components",1106),
            new Alias("hazardousenvironmentsuits","H.E. Suits",1107),
            new Alias("landenrichmentsystems","Land Enrichment Systems",1108),
            new Alias("resonatingseparators","Resonating Separators",1109),
            new Alias("robotics","Robotics",1110),
            new Alias("structuralregulators","Structural Regulators",1111),
            new Alias("mutomimager","Muon Imager",1112),
            new Alias("xihebiomorphiccompanions","Xihe Biomorphic Companions",1113),
            new Alias("azcancriformula42","Az Cancri Formula 42",1114),
            new Alias("nanobreakers","Nanobreakers",1115),
            new Alias("telemetrysuite","Telemetry Suite",1116),
            new Alias("microcontrollers","Micro Controllers",1117),
            new Alias("iondistributor","Ion Distributor",1118),
            new Alias("diagnosticsensor","Hardware Diagnostic Sensor",1119),
            new Alias("medicaldiagnosticequipment","Medical Diagnostic Equipment",1120),
            new Alias("leather","Leather",1201),
            new Alias("naturalfabrics","Natural Fabrics",1202),
            new Alias("syntheticfabrics","Synthetic Fabrics",1203),
            new Alias("belalansrayleather","Belalans Ray Leather",1204),
            new Alias("damnacarapaces","Damna Carapaces",1205),
            new Alias("rapabaosnakeskins","Rapa Bao Snake Skins",1206),
            new Alias("vanayequiceratomorphafur","Vanayequi Ceratomorpha Fur",1207),
            new Alias("bankiamphibiousleather","Banki Amphibious Leather",1208),
            new Alias("tiegfriessynthsilk","Tiegfries Synth Silk",1209),
            new Alias("chameleoncloth","Chameleon Cloth",1210),
            new Alias("conductivefabrics","Conductive Fabrics",1211),
            new Alias("militarygradefabrics","Military Grade Fabrics",1212),
            new Alias("biowaste","Biowaste",1301),
            new Alias("chemicalwaste","Chemical Waste",1302),
            new Alias("scrap","Scrap",1303),
            new Alias("toxicwaste","Toxic Waste",1304),
            new Alias("nonlethalweapons","Non-lethal Weapons",1401),
            new Alias("personalweapons","Personal Weapons",1402),
            new Alias("reactivearmour","Reactive Armour",1403),
            new Alias("battleweapons","Battle Weapons",1404),
            new Alias("kamorinhistoricweapons","Kamorin Historic Weapons",1405),
            new Alias("landmines","Landmines",1406),
            new Alias("borasetanipathogenetics","Borasetani Pathogenetics",1407),
            new Alias("hip118311swarm","HIP 118311 Swarm",1408),
            new Alias("holvaduellingblades","Holva Duelling Blades",1409),
            new Alias("gilyasignatureweapons","Gilya Signature Weapons",1410),
            new Alias("airelics","Ai Relics",1601),
            new Alias("antiquities","Antiquities",1602),
            new Alias("sap8corecontainer","Sap 8 Core Container",1603),
            new Alias("trinketsofhiddenfortune","Trinkets Of Hidden Fortune",1604),
            new Alias("tradedata","Trade Data",1605),
            new Alias("occupiedcryopod","Occupied CryoPod",1606),
            new Alias("blackbox","Black Box",1607),
            new Alias("militaryplans","Military Plans",1608),
            new Alias("ancientartefact","Ancient Artefact",1609),
            new Alias("rareartwork","Rare Artwork",1610),
            new Alias("experimentalchemicals","Experimental Chemicals",1611),
            new Alias("rebeltransmissions","Rebel Transmissions",1612),
            new Alias("prototypetech","Prototype Tech",1613),
            new Alias("technicalblueprints","Technical Blueprints",1614),
            new Alias("unknownartifact","Unknown Artefact",1615),
            new Alias("militaryintelligence","Military Intelligence",1616),
            new Alias("salvageablewreckage","Salvageable Wreckage",1617),
            new Alias("encrypteddatastorage","Encrypted Data Storage",1618),
            new Alias("personaleffects","Personal Effects",1619),
            new Alias("commercialsamples","Commercial Samples",1620),
            new Alias("tacticaldata","Tactical Data",1621),
            new Alias("assaultplans","Assault Plans",1622),
            new Alias("encryptedcorrespondence","Encrypted Correspondence",1623),
            new Alias("diplomaticbag","Diplomatic Bag",1624),
            new Alias("scientificresearch","Scientific Research",1625),
            new Alias("scientificsamples","Scientific Samples",1626),
            new Alias("politicalprisoner","Political Prisoner",1627),
            new Alias("hostage","Hostage",1628),
            new Alias("geologicalsamples","Geological Samples",1629),
            new Alias("unstabledatacore","Unstable Data Core",1630),
            new Alias("occupiedescapepod","Occupied Escape Pod",1631),
            new Alias("datacore","Data Core",1632),
            new Alias("galactictravelguide","Galactic Travel Guide",1633),
            new Alias("mysteriousidol","Mysterious Idol",1634),
            new Alias("prohibitedresearchmaterials","Prohibited Research Materials",1635),
            new Alias("antimattercontainmentunit","Antimatter Containment Unit",1636),
            new Alias("spacepioneerrelics","Space Pioneer Relics",1637),
            new Alias("fossilremnants","Fossil Remnants",1638),
            new Alias("unknownprobe","Unknown Probe",1639),
            new Alias("preciousgems","Precious Gems",1640),
            new Alias("unknownartifact","Unknown Artefact",1615),
            new Alias("unknownlink","Unknown Link",1641),
            new Alias("unknownbiologicalmatter","Unknown Biological Matter",1642),
            new Alias("unknownresin","Unknown Resin",1643),
            new Alias("unknowntechnologysamples","Unknown Technology Samples",1644),
        };



        static void CheckAnthor()
        {
            foreach (Alias a in EDSM_User_Alias_Materials)
            {
                string realname = a.alias;
                string fdname = a.fdname;
                string grade = getGradeMat(a.alias);

                //System.Diagnostics.Debug.WriteLine(a.num + ":" + realname + "=" + a.fdname + "=" + grade);

                MaterialCommodityDB db = GetCachedMaterial(fdname);
                if (db == null)
                    System.Diagnostics.Debug.WriteLine("  **1 NOT FOUND BY FDNAME: " + fdname);
                else if (db.name != realname)
                    System.Diagnostics.Debug.WriteLine("  **1 Alias name disagres " + db.name + " vs " + realname);
                else if (db.type != grade)
                    System.Diagnostics.Debug.WriteLine("  **1 Type disagres " + db.type + " vs " + grade);

            }

            foreach (Alias a in EDSM_User_Alias_Data)
            {
                string realname = a.alias;
                string fdname = a.fdname;
                string grade = getGradeData(a.alias);

                //System.Diagnostics.Debug.WriteLine(a.num + ":" + realname + "=" + a.fdname + "=" + grade);

                MaterialCommodityDB db = GetCachedMaterial(fdname);
                if (db == null)
                    System.Diagnostics.Debug.WriteLine("  **2 NOT FOUND BY FDNAME: " + fdname);
                else if (db.name != realname)
                    System.Diagnostics.Debug.WriteLine("  **2 Alias name disagres " + db.name + " vs " + realname);
            }

            
            foreach (Alias a in EDSM_System_Station_Commodities_Alias_Type)
            {
                string realname = a.alias;
                string fdname = a.fdname;

                //System.Diagnostics.Debug.WriteLine(a.num + ":" + realname + "=" + a.fdname);

                MaterialCommodityDB db = GetCachedMaterial(fdname);
                if (db == null)
                    System.Diagnostics.Debug.WriteLine("  **3 NOT FOUND BY FDNAME: " + fdname);
                else if (!db.name.Equals(realname,StringComparison.InvariantCultureIgnoreCase))
                    System.Diagnostics.Debug.WriteLine("  **3 Alias name disagres " + db.name + " vs " + realname);
            }

            foreach (KeyValuePair<string, MaterialCommodityDB> k in cachelist)
            {
                string fdname = k.Value.fdname;
                Alias a = EDSM_User_Alias_Data.Find(x => x.fdname.Equals(fdname));

                if ( a != null )
                {
                    if (!k.Value.category.Equals(MaterialEncodedCategory) && !k.Value.category.Equals(MaterialManufacturedCategory))
                    {
                        System.Diagnostics.Debug.WriteLine("  4. ** Category not matched " + k.Value.category);
                    }
                }
                else
                {
                    a = EDSM_User_Alias_Materials.Find(x => x.fdname.Equals(fdname));

                    if ( a != null )
                    {
                        if (!k.Value.category.Equals(MaterialRawCategory) && !k.Value.category.Equals(MaterialManufacturedCategory))
                        {
                            System.Diagnostics.Debug.WriteLine("  5. ** Category not matched " + k.Value.category + "," + k.Value.fdname);
                        }
                    }
                    else
                    {
                        a = EDSM_System_Station_Commodities_Alias_Type.Find(x => x.fdname.Equals(fdname));

                        if (a != null)
                        {
                            if (!k.Value.category.Equals(CommodityCategory))
                            {
                                System.Diagnostics.Debug.WriteLine("  6. ** Category not matched " + k.Value.category);
                            }

                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Failed to match " + fdname);
                        }
                    }
                }

                foreach( KeyValuePair<string,string> mk in fdnamemangling )
                {
                    MaterialCommodityDB p = GetCachedMaterial(mk.Value);

                    if ( p == null )
                    {
                        System.Diagnostics.Debug.WriteLine("Mangle " + mk.Value + " does not exist");
                    }
                }
            }

        }

        #endregion

        #region ED Check

        static void CheckEDData()
        {
            ExportImport.CVSFile cvs = new ExportImport.CVSFile();
            if (cvs.Read(@"c:\code\Docs\23Materials.csv"))
            {
                foreach (ExportImport.CVSFile.Line l in cvs.rows)
                {
                    string matname = l.cells[1];

                    MaterialCommodityDB entry = GetCachedMaterial(matname.ToLower());
                    if (entry != null)
                    {
                        string fdlongname = l.cells[4].Trim();  // some spurious spaces seen
                        if (entry.name.Equals(fdlongname))
                        {
                            //System.Diagnostics.Debug.WriteLine("FD data " + matname + " is in DB, name matches");
                        }
                        else
                            System.Diagnostics.Debug.WriteLine("FD data " + matname + " is in DB, name NOT matches " + l.cells[4] + " vs " + entry.name);
                    }
                    else
                        System.Diagnostics.Debug.WriteLine("FD data " + matname + " is NOT in DB");
                }

            }

            if (cvs.Read(@"c:\code\Docs\23commods.csv"))
            {
                foreach (ExportImport.CVSFile.Line l in cvs.rows)
                {
                    string category = l.cells[1];
                    string commodname = l.cells[2].ToLower();
                    string fdlongname = l.cells[3];

                    MaterialCommodityDB entry = GetCachedMaterial(commodname);
                    if (entry != null)
                    {
                        if (entry.name.Equals(fdlongname, StringComparison.InvariantCultureIgnoreCase))
                        {
                            //System.Diagnostics.Debug.WriteLine("FD data " + matname + " is in DB, name matches");
                        }
                        else
                            System.Diagnostics.Debug.WriteLine("FD data " + commodname + " is in DB, name NOT matches " + fdlongname + " vs " + entry.name);
                    }
                    else if (fdnamemangling.ContainsKey(commodname))
                    {
                        System.Diagnostics.Debug.WriteLine("FD data " + commodname + " is NAME mangled to " + fdnamemangling[commodname]);

                    }
                    else
                        System.Diagnostics.Debug.WriteLine("FD data " + commodname + " is NOT in DB");
                }

            }
        }

        #endregion

#endif


    }
}


