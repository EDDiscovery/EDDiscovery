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
using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
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

        private static bool AddNewRare(string fdname, string name)
        {
            return AddNewTypeF(CommodityCategory, Color.Green, name, CommodityTypeRareGoods, "", fdname);
        }

        private static bool AddNewType(string c, string namelist, string t)
        {
            return AddNewTypeC(c, Color.Green, namelist, t);
        }

        private static bool AddNewTypeC(string c, Color cl, string namelist, string t, string sn = "")
        {
            string[] list = namelist.Split(';');

            foreach (string name in list)
            {
                if (name.Length > 0)   // just in case a semicolon slips thru
                {
                    if (!AddNewTypeF(c, cl, name, t, sn))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool AddNewTypeF(string c, Color cl, string name, string t, string sn = "", string fdName = "")
        {
            string fdn = (fdName.Length > 0) ? fdName : name.FDName();
            MaterialCommodityDB mc = new MaterialCommodityDB(c, name, fdn, t, sn, cl, 0);
            mc.SetCache();
            return true;
        }

        public static void SetUpInitialTable()
        {
            AddNewTypeC(MaterialRawCategory, Color.Red, "Antimony", "Very Rare", "Sb");
            AddNewTypeC(MaterialRawCategory, Color.Red, "Polonium", "Very Rare", "Po");
            AddNewTypeC(MaterialRawCategory, Color.Red, "Ruthenium", "Very Rare", "Ru");
            AddNewTypeC(MaterialRawCategory, Color.Red, "Technetium", "Very Rare", "Tc");
            AddNewTypeC(MaterialRawCategory, Color.Red, "Tellurium", "Very Rare", "Te");

            AddNewTypeC(MaterialRawCategory, Color.Yellow, "Yttrium", "Rare", "Y");
            AddNewTypeC(MaterialRawCategory, Color.Yellow, "Cadmium", "Rare", "Cd");
            AddNewTypeC(MaterialRawCategory, Color.Yellow, "Mercury", "Rare", "Hg");
            AddNewTypeC(MaterialRawCategory, Color.Yellow, "Molybdenum", "Rare", "Mo");
            AddNewTypeC(MaterialRawCategory, Color.Yellow, "Tin", "Rare", "Sn");

            AddNewTypeC(MaterialRawCategory, Color.Cyan, "Carbon", "Very Common", "C");
            AddNewTypeC(MaterialRawCategory, Color.Cyan, "Iron", "Very Common", "Fe");
            AddNewTypeC(MaterialRawCategory, Color.Cyan, "Nickel", "Very Common", "Ni");
            AddNewTypeC(MaterialRawCategory, Color.Cyan, "Phosphorus", "Very Common", "P");
            AddNewTypeC(MaterialRawCategory, Color.Cyan, "Sulphur", "Very Common", "S");

            AddNewTypeC(MaterialRawCategory, Color.Green, "Chromium", "Common", "Cr");
            AddNewTypeC(MaterialRawCategory, Color.Green, "Germanium", "Common", "Ge");
            AddNewTypeC(MaterialRawCategory, Color.Green, "Manganese", "Common", "Mn");
            AddNewTypeC(MaterialRawCategory, Color.Green, "Vanadium", "Common", "V");
            AddNewTypeC(MaterialRawCategory, Color.Green, "Zinc", "Common", "Zn");

            AddNewTypeC(MaterialRawCategory, Color.SandyBrown, "Niobium", "Standard", "Nb");        // realign to Anthors standard
            AddNewTypeC(MaterialRawCategory, Color.SandyBrown, "Tungsten", "Standard", "W");
            AddNewTypeC(MaterialRawCategory, Color.SandyBrown, "Arsenic", "Standard", "As");
            AddNewTypeC(MaterialRawCategory, Color.SandyBrown, "Selenium", "Standard", "Se");
            AddNewTypeC(MaterialRawCategory, Color.SandyBrown, "Zirconium", "Standard", "Zr");

            AddNewType(CommodityCategory, "Explosives;Hydrogen Fuel;Hydrogen Peroxide;Liquid Oxygen;Mineral Oil;Nerve Agents;Pesticides;Surface Stabilisers;Synthetic Reagents;Water", "Chemicals");
            AddNewType(CommodityCategory, "Clothing;Consumer Technology;Domestic Appliances;Evacuation Shelter;Survival Equipment", "Consumer Items");
            AddNewType(CommodityCategory, "Algae;Animal Meat;Coffee;Fish;Food Cartridges;Fruit and Vegetables;Grain;Synthetic Meat;Tea", "Foods");
            AddNewType(CommodityCategory, "Ceramic Composites;CMM Composite;Insulating Membrane;Meta-Alloys;Micro-Weave Cooling Hoses;Cooling Hoses;Neofabric Insulation;Polymers;Semiconductors;Superconductors", "Industrial Materials");
            AddNewType(CommodityCategory, "Beer;Bootleg Liquor;Liquor;Narcotics;Tobacco;Wine;Lavian Brandy", "Legal Drugs");
            AddNewType(CommodityCategory, "Articulation Motors;Atmospheric Processors;Building Fabricators;Crop Harvesters;Emergency Power Cells;Energy Grid Assembly;Exhaust Manifold;Geological Equipment", "Machinery");
            AddNewType(CommodityCategory, "Heatsink Interlink;HN Shock Mount;Ion Distributor;Magnetic Emitter Coil;Marine Equipment", "Machinery");
            AddNewType(CommodityCategory, "Microbial Furnaces;Mineral Extractors;Modular Terminals;Power Converter;Power Generators;Power Transfer Bus", "Machinery");
            AddNewType(CommodityCategory, "Radiation Baffle;Reinforced Mounting Plate;Skimmer Components;Thermal Cooling Units;Water Purifiers", "Machinery");
            AddNewType(CommodityCategory, "Advanced Medicines;Agri-Medicines;Basic Medicines;Combat Stabilisers;Performance Enhancers;Progenitor Cells", "Medicines");
            AddNewType(CommodityCategory, "Aluminium;Beryllium;Bismuth;Cobalt;Copper;Gallium;Gold;Hafnium 178;Indium;Lan;hanum;Lithium;Osmium;Palladium;Platinum;Praseodymium;Samarium;Silver;Tantalum;Thallium;Thorium;Titanium;Uranium", "Metals");
            AddNewType(CommodityCategory, "Bauxite;Bertrandite;Bromellite;Coltan;Cryolite;Gallite;Goslarite;Methane Clathrate;Methanol Monohydrate Crystals", "Minerals");
            AddNewType(CommodityCategory, "Indite;Jadeite;Lepidolite;Lithium Hydroxide;Low Temperature Diamonds;Methane ;lathrate;Methanol Monohydrate;Moissanite;Painite;Pyrophyllite;Rutile;Taaffeite;Uraninite", "Minerals");
            AddNewType(CommodityCategory, "Ai Relics;Ancient Artefact;Antimatter Containment Unit;Antiquities;Assault Plans;Black Box;Commercial Samples;Data Core;Diplomatic Bag;Encrypted Correspondence;Encrypted Data Storage;Experimental Chemicals;Fossil Remnants;Ancient Orb;Ancient Casket;Ancient Relic", "Salvage");
            AddNewType(CommodityCategory, "Galactic Travel Guide;Geological Samples;Hostage;Military Intelligence;Military Plans (USS Cargo);Mysterious Idol;Occupied CryoPod;Occupied Escape Pod;Personal Effects;Political Prisoner;Precious Gems;Prohibited Research Materials;Prototype Tech", "Salvage");
            AddNewType(CommodityCategory, "Rare Artwork;Rebel Transmissions;Salvageable Wreckage;Sap 8 Core Container;Scientific Research;Scientific Samples;Space Pioneer Relics;Tactical Data;Technical Blueprints;Trade Data;Unknown Artefact;Unknown Probe;Unstable Data Core", "Salvage");
            AddNewType(CommodityCategory, "Imperial Slaves;Slaves", "Slavery");
            AddNewType(CommodityCategory, "Advanced Catalysers;Animal Monitors;Aquaponic Systems;Auto-Fabricators;Bioreducing Lichen;Computer Components", "Technology");
            AddNewType(CommodityCategory, "H.E. Suits;Hardware Diagnostic Sensor;Land Enrichment Systems;Medical Diagnostic Equipment;Micro Controllers;Muon Imager", "Technology");
            AddNewType(CommodityCategory, "Nanobreakers;Resonating Separators;Robotics;Structural Regulators;Telemetry Suite", "Technology");
            AddNewType(CommodityCategory, "Conductive Fabrics;Leather;Military Grade Fabrics;Natural Fabrics;Synthetic Fabrics", "Textiles");
            AddNewType(CommodityCategory, "Biowaste;Chemical Waste;Scrap;Toxic Waste", "Waste");
            AddNewType(CommodityCategory, "Battle Weapons;Landmines;Non-lethal Weapons;Personal Weapons;Reactive Armour", "Weapons");
            AddNewType(CommodityCategory, "Drones", "Drones");


            AddNewRare("advert1", "Advert 1");
            AddNewRare("aerialedenapple", "Aerial Eden Apple");
            AddNewRare("aganipperush", "Aganippe Rush");
            AddNewRare("alacarakmoskinart", "Alacarakmo Skin Art");
            AddNewRare("albinoquechuamammoth", "Albino Quechua Mammoth");
            AddNewRare("alieneggs", "Alien Eggs");
            AddNewRare("altairianskin", "Altairian Skin");
            AddNewRare("alyabodilysoap", "Alya Body Soap");
            AddNewRare("anduligafireworks", "Anduliga Fireworks");
            AddNewRare("anynacoffee", "Any Na Coffee");
            AddNewRare("aroucaconventualsweets", "Arouca Conventual Sweets");
            AddNewRare("azcancriformula42", "Azcancri Formula 42");
            AddNewRare("bakedgreebles", "Baked Greebles");
            AddNewRare("baltahsinevacuumkrill", "Baltah'sine Vacuum Krill");
            AddNewRare("bankiamphibiousleather", "Banki Amphibious Leather");
            AddNewRare("bastsnakegin", "Bast Snake Gin");
            AddNewRare("belalansrayleather", "Belalans Ray Leather");
            AddNewRare("bluemilk", "Blue Milk");
            AddNewRare("borasetanipathogenetics", "Borasetani Pathogenetics");
            AddNewRare("buckyballbeermats", "Buckyball beermats");
            AddNewRare("burnhambiledistillate", "Burnham Bile Distillate");
            AddNewRare("cd75catcoffee", "CD75 Cat Coffee");
            AddNewRare("centaurimegagin", "Centaurimegagin");
            AddNewRare("ceremonialheiketea", "Ceremonial Heike Tea");
            AddNewRare("cetiaepyornisegg", "Cetiaepyornis Egg");
            AddNewRare("cetirabbits", "Ceti Rabbits");
            AddNewRare("chameleoncloth", "Chameleon Cloth");
            AddNewRare("chateaudeaegaeon", "Chateau De Aegaeon");
            AddNewRare("cherbonesbloodcrystals", "Cherbones Blood Crystals");
            AddNewRare("chieridanimarinepaste", "Chi Eridani Marine Paste");
            AddNewRare("coquimspongiformvictuals", "Coquim Spongiform Victuals");
            AddNewRare("cromsilverfesh", "Crom Silver Fesh");
            AddNewRare("crystallinespheres", "Crystalline Spheres");
            AddNewRare("damnacarapaces", "Damna Carapaces");
            AddNewRare("deltaphoenicispalms", "Delta Phoenicis Palms");
            AddNewRare("deuringastruffles", "Deuringas Truffles");
            AddNewRare("disomacorn", "Diso Ma Corn");
            AddNewRare("eleuthermals", "Eleu Thermals");
            AddNewRare("eraninpearlwhisky", "Eranin Pearl Whisky");
            AddNewRare("eshuumbrellas", "Eshu Umbrellas");
            AddNewRare("esusekucaviar", "Esuseku Caviar");
            AddNewRare("ethgrezeteabuds", "Ethgreze Tea Buds");
            AddNewRare("fujintea", "Fujin Tea");
            AddNewRare("galactictravelguide", "Galactic Travel Guide");
            AddNewRare("geawendancedust", "Geawen Dance Dust");
            AddNewRare("gerasiangueuzebeer", "Gerasian Gueuze Beer");
            AddNewRare("giantirukamasnails", "Gian Tirukama Snails");
            AddNewRare("giantverrix", "Giant Verrix");
            AddNewRare("gilyasignatureweapons", "Gilya Signature Weapons");
            AddNewRare("gomanyauponcoffee", "Goman Yaupon Coffee");
            AddNewRare("haidneblackbrew", "Haidne Black Brew");
            AddNewRare("havasupaidreamcatcher", "Havasupai Dream Catcher");
            AddNewRare("helvetitjpearls", "Helvetitj Pearls");
            AddNewRare("hip10175bushmeat", "Hip10175 Bushmeat");
            AddNewRare("hip118311swarm", "Hip118311 Wwarm");
            AddNewRare("hip41181squid", "Hip41181 Squid");
            AddNewRare("hiporganophosphates", "Hip Organophosphates");
            AddNewRare("holvaduellingblades", "Holva Duelling Blades");
            AddNewRare("honestypills", "Honesty Pills");
            AddNewRare("hr7221wheat", "Hr7221 Wheat");
            AddNewRare("indibourbon", "Indi Bourbon");
            AddNewRare("jaquesquinentianstill", "Jaques Quinentian Still");
            AddNewRare("jaradharrepuzzlebox", "Jaradharre Puzzle Box");
            AddNewRare("jarouarice", "Jaroua Rice");
            AddNewRare("jotunmookah", "Jotun Mookah");
            AddNewRare("kachiriginleaches", "Kachirigin Leaches");
            AddNewRare("kamitracigars", "Kamitra Cigars");
            AddNewRare("kamorinhistoricweapons", "Kamorin Historic Weapons");
            AddNewRare("karetiicouture", "Karetii Couture");
            AddNewRare("karsukilocusts", "Karsuki Locusts");
            AddNewRare("kinagoinstruments", "Kinago Instruments");
            AddNewRare("konggaale", "Kongga Ale");
            AddNewRare("korrokungpellets", "Korro Kung Pellets");
            AddNewRare("lavianbrandy", "Lavian Brandy");
            AddNewRare("leestianeviljuice", "Leestian Evil Juice");
            AddNewRare("lftvoidextractcoffee", "Lft Void Extract Coffee");
            AddNewRare("livehecateseaworms", "Live Hecate Seaworms");
            AddNewRare("ltthypersweet", "Ltt Hypersweet");
            AddNewRare("lyraeweed", "Lyrae Weed");
            AddNewRare("masterchefs", "Master Chefs");
            AddNewRare("mechucoshightea", "Mechucos High Tea");
            AddNewRare("medbstarlube", "Medb Starlube");
            AddNewRare("mokojingbeastfeast", "Mokojing Beast Feast");
            AddNewRare("momusbogspaniel", "Momus Bog Spaniel");
            AddNewRare("motronaexperiencejelly", "Motrona Experience Jelly");
            AddNewRare("mukusubiichitinos", "Mukusubii Chitinos");
            AddNewRare("mulachigiantfungus", "Mulachigiantfungus");
            AddNewRare("neritusberries", "Neritusberries");
            AddNewRare("ngadandarifireopals", "Ngadandarifireopals");
            AddNewRare("ngunamodernantiques", "Ngunamodernantiques");
            AddNewRare("njangarisaddles", "Njangarisaddles");
            AddNewRare("noneuclidianexotanks", "Noneuclidianexotanks");
            AddNewRare("ochoengchillies", "Ochoengchillies");
            AddNewRare("onionhead", "Onion Head");
            AddNewRare("onionheada", "Onion Head A");
            AddNewRare("onionheadb", "Onion Head B");
            AddNewRare("onionheadc", "Onion Head C");
            AddNewRare("onionheadd", "Onion Head D");
            AddNewRare("onionheadderivatives", "Onion Head Derivatives");
            AddNewRare("onionheade", "Onion Head E");
            AddNewRare("onionheadsamples", "Onion Head Samples");
            AddNewRare("ophiuchiexinoartefacts", "Ophiuchi Exino Artefacts");
            AddNewRare("orrerianviciousbrew", "Orrerian Vicious Brew");
            AddNewRare("pantaaprayersticks", "Pantaa Prayer Sticks");
            AddNewRare("pavoniseargrubs", "Pavonis Ear Grubs");
            AddNewRare("personalgifts", "Personal Gifts");
            AddNewRare("rajukrustoves", "Rajukru Stoves");
            AddNewRare("rapabaosnakeskins", "Rapabao Snakeskins");
            AddNewRare("rusanioldsmokey", "Rusani Old Smokey");
            AddNewRare("sanumameat", "Sanuma Meat");
            AddNewRare("saxonwine", "Saxon Wine");
            AddNewRare("shanscharisorchid", "Shans Charis Orchid");
            AddNewRare("soontillrelics", "Soontill Relics");
            AddNewRare("sothiscrystallinegold", "Sothis Crystalline Gold");
            AddNewRare("tanmarktranquiltea", "Tanmark Tranquil Tea");
            AddNewRare("tarachtorspice", "Tarachtor Spice");
            AddNewRare("taurichimes", "Tauri Chimes");
            AddNewRare("terramaterbloodbores", "Terra Mater Blood Bores");
            AddNewRare("thehuttonmug", "The Hutton Mug");
            AddNewRare("thrutiscream", "Thrutis Cream");
            AddNewRare("tiegfriessynthsilk", "Tieg Fries Synthsilk");
            AddNewRare("tiolcewaste2pasteunits", "Tiolce Waste2paste Units");
            AddNewRare("toxandjivirocide", "ToxanDji Virocide");
            AddNewRare("transgeniconionhead", "Transgenic Onion Head");
            AddNewRare("uszaiantreegrub", "Uszaian Tree Grub");
            AddNewRare("utgaroarmillenialeggs", "Utgaroar Millenial Eggs");
            AddNewRare("uzumokulowgwings", "Uzumoku Low-G Wings");
            AddNewRare("vacuumkrill", "Vacuum Krill");
            AddNewRare("vanayequirhinofur", "Vanayequi Rhino Fur");
            AddNewRare("vegaslimweed", "Vega Slimweed");
            AddNewRare("vherculisbodyrub", "Vherculis Body Rub");
            AddNewRare("vidavantianlace", "Vidavantian Lace");
            AddNewRare("voidworms", "Void Worms");
            AddNewRare("volkhabbeedrones", "Volkhabbee Drones");
            AddNewRare("watersofshintara", "Waters Of shintara");
            AddNewRare("wheemetewheatcakes", "Wheemete Wheat Cakes");
            AddNewRare("witchhaulkobebeef", "Witchhaul Kobe Beef");
            AddNewRare("wolf1301fesh", "Wolf1301 Fesh");
            AddNewRare("wulpahyperboresystems", "Wulpa Hyperbore Systems");
            AddNewRare("wuthielokufroth", "Wuthielo ku froth");
            AddNewRare("xihecompanions", "Xihe Companions");
            AddNewRare("yasokondileaf", "Yaso Kondi Leaf");
            AddNewRare("zeesszeantglue", "Zeessze Ant Glue");

            // very common data
            AddNewTypeF(MaterialEncodedCategory, Color.Cyan, "Anomalous Bulk Scan Data", "Very Common", "ABSD", "bulkscandata");
            AddNewTypeF(MaterialEncodedCategory, Color.Cyan, "Atypical Disrupted Wake Echoes", "Very Common", "ADWE", "disruptedwakeechoes");
            AddNewTypeF(MaterialEncodedCategory, Color.Cyan, "Distorted Shield Cycle Recordings", "Very Common", "DSCR", "shieldcyclerecordings");
            AddNewTypeF(MaterialEncodedCategory, Color.Cyan, "Exceptional Scrambled Emission Data", "Very Common", "ESED", "scrambledemissiondata");
            AddNewTypeF(MaterialEncodedCategory, Color.Cyan, "Specialised Legacy Firmware", "Very Common", "SLF", "legacyfirmware");
            AddNewTypeF(MaterialEncodedCategory, Color.Cyan, "Unusual Encrypted Files", "Very Common", "UEF", "encryptedfiles");
            // common data
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Anomalous FSD Telemetry", "Common", "AFT", "fsdtelemetry");
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Inconsistent Shield Soak Analysis", "Common", "ISSA", "shieldsoakanalysis");
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Irregular Emission Data", "Common", "IED", "archivedemissiondata");
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Modified Consumer Firmware", "Common", "MCF", "consumerfirmware");
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Tagged Encryption Codes", "Common", "TEC", "encryptioncodes");
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Unidentified Scan Archives", "Common", "USA", "scanarchives");
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Pattern Beta Obelisk Data", "Common", "PBOD", "ancientculturaldata");
            AddNewTypeF(MaterialEncodedCategory, Color.Green, "Pattern Gamma Obelisk Data", "Common", "PGOD", "ancienthistoricaldata");
            // standard data
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Classified Scan Databanks", "Standard", "CSD", "scandatabanks");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Cracked Industrial Firmware", "Standard", "CIF", "industrialfirmware");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Open Symmetric Keys", "Standard", "OSK", "symmetrickeys");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Strange Wake Solutions", "Standard", "SWS", "wakesolutions");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Unexpected Emission Data", "Standard", "UED", "emissiondata");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Untypical Shield Scans", "Standard", "USS", "shielddensityreports");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Peculiar Shield Frequency Data", "Standard", "SFD", "shieldfrequencydata");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Classified Scan Fragment", "Standard", "CFSD", "classifiedscandata");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Abnormal Compact Emission Data", "Standard", "CED", "compactemissionsdata");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Modified Embedded Firmware", "Standard", "EFW", "embeddedfirmware");
            AddNewTypeF(MaterialEncodedCategory, Color.SandyBrown, "Pattern Alpha Obelisk Data", "Standard", "PAOD", "ancientbiologicaldata");

            // rare data
            AddNewTypeF(MaterialEncodedCategory, Color.Yellow, "Aberrant Shield Pattern Analysis", "Rare", "ASPA", "shieldpatternanalysis");
            AddNewTypeF(MaterialEncodedCategory, Color.Yellow, "Atypical Encryption Archives", "Rare", "AEA", "encryptionarchives");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Decoded Emission Data", "Rare", "DED");
            AddNewTypeF(MaterialEncodedCategory, Color.Yellow, "Divergent Scan Data", "Rare", "DSD", "encodedscandata");
            AddNewTypeF(MaterialEncodedCategory, Color.Yellow, "Eccentric Hyperspace Trajectories", "Rare", "EHT", "hyperspacetrajectories");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Pattern Alpha Obelisk Data", "Rare", "ODA");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Pattern Beta Obelisk Data", "Rare", "ODB");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Pattern Gamma Obelisk Data", "Rare", "ODG");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Pattern Delta Obelisk Data", "Rare", "ODD");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Pattern Epsilon Obelisk Data", "Rare", "ODE");
            AddNewTypeF(MaterialEncodedCategory, Color.Yellow, "Security Firmware Patch", "Rare", "SFP", "securityfirmware");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Unknown Ship Signature", "Rare", "USSig");
            AddNewTypeC(MaterialEncodedCategory, Color.Yellow, "Unknown Wake Data", "Rare", "UWD");
            AddNewTypeF(MaterialEncodedCategory, Color.Yellow, "Pattern Delta Obelisk Data", "Rare", "PDOD", "ancientlanguagedata");
            // very rare data
            AddNewTypeC(MaterialEncodedCategory, Color.Red, "Abnormal Compact Emission Data", "Very Rare", "ACED");
            AddNewTypeF(MaterialEncodedCategory, Color.Red, "Adaptive Encryptors Capture", "Very Rare", "AEC", "adaptiveencryptors");
            AddNewTypeC(MaterialEncodedCategory, Color.Red, "Classified Scan Fragment", "Very Rare", "CSF");
            AddNewTypeF(MaterialEncodedCategory, Color.Red, "Datamined Wake Exceptions", "Very Rare", "DWEx", "dataminedwake");
            AddNewTypeC(MaterialEncodedCategory, Color.Red, "Modified Embedded Firmware", "Very Rare", "MEF");
            AddNewTypeF(MaterialEncodedCategory, Color.Red, "Peculiar Shield Frequency Data", "Very Rare", "PSFD", "shieldfrequencydata");
            AddNewTypeF(MaterialEncodedCategory, Color.Red, "Pattern Epsilon Obelisk Data", "Very Rare", "PSFD", "ancienttechnologicaldata");
             

            //very common manufactured
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Basic Conductors", "Very Common", "BaC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Chemical Storage Units", "Very Common", "CSU");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Compact Composites", "Very Common", "CC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Crystal Shards", "Very Common", "CS");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Grid Resistors", "Very Common", "GR");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Heat Conduction Wiring", "Very Common", "HCW");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Mechanical Scrap", "Very Common", "MS");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Salvaged Alloys", "Very Common", "SAll");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Worn Shield Emitters", "Very Common", "WSE");
            AddNewTypeC(MaterialManufacturedCategory, Color.Cyan, "Thermic Alloys", "Very Common", "ThA");
            // common manufactured
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Chemical Processors", "Common", "CP");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Conductive Components", "Common", "CCo");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Filament Composites", "Common", "FiC");
            AddNewTypeF(MaterialManufacturedCategory, Color.Green, "Flawed Focus Crystals", "Common", "FFC", "uncutfocuscrystals");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Galvanising Alloys", "Common", "GA");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Heat Dispersion Plate", "Common", "HDP");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Heat Resistant Ceramics", "Common", "HRC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Hybrid Capacitors", "Common", "HC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Mechanical Equipment", "Common", "ME");
            AddNewTypeC(MaterialManufacturedCategory, Color.Green, "Shield Emitters", "Common", "SE");

            // standard manufactured
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Chemical Distillery", "Standard", "CD");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Conductive Ceramics", "Standard", "CCe");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Electrochemical Arrays", "Standard", "EA");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Focus Crystals", "Standard", "FoC");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Heat Exchangers", "Standard", "HE");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "High Density Composites", "Standard", "HDC");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Mechanical Components", "Standard", "MC");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Phase Alloys", "Standard", "PA");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Precipitated Alloys", "Standard", "PAll");
            AddNewTypeC(MaterialManufacturedCategory, Color.SandyBrown, "Shielding Sensors", "Standard", "SS");
            // rare manufactured
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Chemical Manipulators", "Rare", "CM");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Compound Shielding", "Rare", "CoS");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Conductive Polymers", "Rare", "CPo");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Configurable Components", "Rare", "CCom");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Heat Vanes", "Rare", "HV");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Polymer Capacitors", "Rare", "PCa");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Proprietary Composites", "Rare", "PCo");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Proto Light Alloys", "Rare", "PLA");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Refined Focus Crystals", "Rare", "RFC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Yellow, "Tempered Alloys", "Rare", "TeA");
            AddNewTypeF(MaterialManufacturedCategory, Color.Yellow, "Proprietary Composites", "Rare", "FPC", "fedproprietarycomposites");
            // very rare manufactured
            AddNewTypeF(MaterialManufacturedCategory, Color.Red, "Core Dynamics Composites", "Very Rare", "FCC", "fedcorecomposites");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Biotech Conductors", "Very Rare", "BiC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Core Dynamics Composites", "Very Rare", "CDC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Exquisite Focus Crystals", "Very Rare", "EFC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Imperial Shielding", "Very Rare", "IS");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Improvised Components", "Very Rare", "IC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Military Grade Alloys", "Very Rare", "MGA");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Military Supercapacitors", "Very Rare", "MSC");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Pharmaceutical Isolators", "Very Rare", "PI");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Proto Heat Radiators", "Very Rare", "PHR");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Proto Radiolic Alloys", "Very Rare", "PRA");
            AddNewTypeC(MaterialManufacturedCategory, Color.Red, "Unknown Fragment", "Very Rare", "UF");
            AddNewTypeF(MaterialManufacturedCategory, Color.Red, "Unknown Fragment", "Very Rare", "UES", "unknownenergysource");
        }

        #endregion

#if false

#region Anthor check 19 may 2017        - **** KEEP THIS, We will periodically ask Anthor for his files and then we can double check

        static Dictionary<int, string> dataalias = new Dictionary<int, string>()
    {
         { 1,  "Aberrant Shield Pattern Analysis"},
         {    2,  "Abnormal Compact Emission Data"},
     {    3,  "Adaptive Encryptors Capture"},
     {    4,  "Anomalous Bulk Scan Data"},
     {    5,  "Anomalous FSD Telemetry"},
     {    6,  "Atypical Disrupted Wake Echoes"},
     {    7,  "Atypical Encryption Archives"},
     {    8,  "Classified Scan Databanks"},
     {    9,  "Classified Scan Fragment"},
     {   10  ,"Cracked Industrial Firmware"},
     {   11  ,"Datamined Wake Exceptions"},
     {   12  ,"Decoded Emission Data"},
     {   13  ,"Distorted Shield Cycle Recordings"},
     {   14  ,"Divergent Scan Data"},
     {   15  ,"Eccentric Hyperspace Trajectories"},
     {   16  ,"Exceptional Scrambled Emission Data"},
     {   17  ,"Inconsistent Shield Soak Analysis"},
     {   18  ,"Irregular Emission Data"},
     {   19  ,"Modified Consumer Firmware"},
     {   20  ,"Modified Embedded Firmware"},
     {   21  ,"Open Symmetric Keys"},
     {   22  ,"Pattern Alpha Obelisk Data"},
     {   23  ,"Pattern Beta Obelisk Data"},
     {   24  ,"Pattern Delta Obelisk Data"},
     {   25  ,"Pattern Epsilon Obelisk Data"},
     {   26  ,"Pattern Gamma Obelisk Data"},
     {   27  ,"Peculiar Shield Frequency Data"},
     {   28  ,"Security Firmware Patch"},
     {   29  ,"Specialised Legacy Firmware"},
     {   30  ,"Strange Wake Solutions"},
     {   31  ,"Tagged Encryption Codes"},
     {   32  ,"Unexpected Emission Data"},
     {   33  ,"Unidentified Scan Archives"},
     {   34  ,"Unknown Fragment"},
     {   35  ,"Untypical Shield Scans"},
     {   36  ,"Unusual Encrypted Files"},
     {   37  ,"Unknown Ship Signature"},
     {   38  ,"Unknown Wake Data"},
    };

        static Dictionary<string, int> fddataalias = new Dictionary<string, int>()
    {
        {"shieldpatternanalysis"            ,1},
        {"compactemissionsdata"             ,2},
        {"adaptiveencryptors"               ,3},
        {"bulkscandata"                     ,4},
        {"fsdtelemetry"                     ,5},
        {"disruptedwakeechoes"              ,6},
        {"encryptionarchives"               ,7},
        {"scandatabanks"                    ,8},
        {"classifiedscandata"               ,9},
        {"industrialfirmware"               ,10},
        {"dataminedwake"                    ,11},
        {"decodedemissiondata"              ,12},
        {"shieldcyclerecordings"            ,13},
        {"encodedscandata"                  ,14},
        {"hyperspacetrajectories"           ,15},
        {"scrambledemissiondata"            ,16},
        {"shieldsoakanalysis"               ,17},
        {"archivedemissiondata"             ,18},
        {"consumerfirmware"                 ,19},
        {"embeddedfirmware"                 ,20},
        {"symmetrickeys"                    ,21},
        {"ancientbiologicaldata"            ,22},
        {"ancientculturaldata"              ,23},
        {"ancientlanguagedata"              ,24},
        {"ancienttechnologicaldata"         ,25},
        {"ancienthistoricaldata"            ,26},
        {"shieldfrequencydata"              ,27},
        {"securityfirmware"                 ,28},
        {"legacyfirmware"                   ,29},
        {"wakesolutions"                    ,30},
        {"encryptioncodes"                  ,31},
        {"emissiondata"                     ,32},
        {"scanarchives"                     ,33},
        {"unknownenergysource"              ,34},
        {"shielddensityreports"             ,35},
        {"encryptedfiles"                   ,36},
        {"unknownshipsignature"             ,37},
        {"unknownwakedata"                  ,38}
    };

        static string getGradeData(string alias)
        {
            int number = fddataalias[alias];

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


        static Dictionary<int, string> matalias = new Dictionary<int, string>()
    {
     {    1,  "Antimony"},
     {    2,  "Arsenic"},
     {    3,  "Basic Conductors"},
     {    4,  "Biotech Conductors"},
     {    5,  "Cadmium"},
     {    6,  "Carbon"},
     {    7,  "Chemical Distillery"},
     {    8,  "Chemical Manipulators"},
     {    9,  "Chemical Processors"},
     {   10,  "Chemical Storage Units"},
     {   11,  "Chromium"},
     {   12,  "Compact Composites"},
     {   13,  "Compound Shielding"},
     {   14,  "Conductive Ceramics"},
     {   15,  "Conductive Components"},
     {   16,  "Conductive Polymers"},
     {   17,  "Configurable Components"},
     {   18,  "Core Dynamics Composites"},
     {   19,  "Crystal Shards"},
     {   20,  "Electrochemical Arrays"},
     {   21,  "Exquisite Focus Crystals"},
     {   22,  "Filament Composites"},
     {   23,  "Flawed Focus Crystals"},
     {   24,  "Focus Crystals"},
     {   25,  "Galvanising Alloys"},
     {   26,  "Germanium"},
     {   27,  "Grid Resistors"},
     {   28,  "Heat Conduction Wiring"},
     {   29,  "Heat Dispersion Plate"},
     {   30,  "Heat Exchangers"},
     {   31,  "Heat Resistant Ceramics"},
     {   32,  "Heat Vanes"},
     {   33,  "High Density Composites"},
     {   34,  "Hybrid Capacitors"},
     {   35,  "Imperial Shielding"},
     {   36,  "Improvised Components"},
     {   37,  "Iron"},
     {   38,  "Manganese"},
     {   39,  "Mechanical Components"},
     {   40,  "Mechanical Equipment"},
     {   41,  "Mechanical Scrap"},
     {   42,  "Mercury"},
     {   43,  "Military Grade Alloys"},
     {   44,  "Military Supercapacitors"},
     {   45,  "Molybdenum"},
     {   46,  "Nickel"},
     {   47,  "Niobium"},
     {   48,  "Pharmaceutical Isolators"},
     {   49,  "Phase Alloys"},
     {   50,  "Phosphorus"},
     {   51,  "Polonium"},
     {   52,  "Polymer Capacitors"},
     {   53,  "Precipitated Alloys"},
     {   54,  "Proprietary Composites"},
     {   55,  "Proto Heat Radiators"},
     {   56,  "Proto Light Alloys"},
     {   57,  "Proto Radiolic Alloys"},
     {   58,  "Refined Focus Crystals"},
     {   59,  "Ruthenium"},
     {   60,  "Salvaged Alloys"},
     {   61,  "Selenium"},
     {   62,  "Shield Emitters"},
     {   63,  "Shielding Sensors"},
     {   64,  "Sulphur"},
     {   65,  "Technetium"},
     {   66,  "Tellurium"},
     {   67,  "Tempered Alloys"},
     {   68,  "Thermic Alloys"},
     {   69,  "Tin"},
     {   70,  "Tungsten"},
     {   71,  "Unknown Fragment"},
     {   72,  "Vanadium"},
     {   73,  "Worn Shield Emitters"},
     {   74,  "Yttrium"},
     {   75,  "Zinc"},
     {   76,  "Zirconium"},
    };

        static Dictionary<string, int> fdmatalias = new Dictionary<string, int>()
    { 
        {"antimony"                , 1},
            {"arsenic"                 , 2},
        {"basicconductors"         , 3},
        {"biotechconductors"       , 4},
        {"cadmium"                 , 5},
        {"carbon"                  , 6},
        {"chemicaldistillery"      , 7},
        {"chemicalmanipulators"    , 8},
        {"chemicalprocessors"      , 9},
        {"chemicalstorageunits"    , 10},
        {"chromium"                , 11},
        {"compactcomposites"       , 12},
        {"compoundshielding"       , 13},
        {"conductiveceramics"      , 14},
        {"conductivecomponents"    , 15},
        {"conductivepolymers"      , 16},
        {"configurablecomponents"  , 17},
        {"fedcorecomposites"       , 18},
        {"crystalshards"           , 19},
        {"electrochemicalarrays"   , 20},
        {"exquisitefocuscrystals"  , 21},
        {"filamentcomposites"      , 22},
        {"uncutfocuscrystals"      , 23},
        {"focuscrystals"           , 24},
        {"galvanisingalloys"       , 25},
        {"germanium"               , 26},
        {"gridresistors"           , 27},
        {"heatconductionwiring"    , 28},
        {"heatdispersionplate"     , 29},
        {"heatexchangers"          , 30},
        {"heatresistantceramics"   , 31},
        {"heatvanes"               , 32},
        {"highdensitycomposites"   , 33},
        {"hybridcapacitors"        , 34},
        {"imperialshielding"       , 35},
        {"improvisedcomponents"    , 36},
        {"iron"                    , 37},
        {"manganese"               , 38},
        {"mechanicalcomponents"    , 39},
        {"mechanicalequipment"     , 40},
        {"mechanicalscrap"         , 41},
        {"mercury"                 , 42},
        {"militarygradealloys"     , 43},
        {"militarysupercapacitors" , 44},
        {"molybdenum"              , 45},
        {"nickel"                  , 46},
        {"niobium"                 , 47},
        {"pharmaceuticalisolators" , 48},
        {"phasealloys"             , 49},
        {"phosphorus"              , 50},
        {"polonium"                , 51},
        {"polymercapacitors"       , 52},
        {"precipitatedalloys"      , 53},
        {"fedproprietarycomposites", 54},
        {"protoheatradiators"      , 55},
        {"protolightalloys"        , 56},
        {"protoradiolicalloys"     , 57},
        {"refinedfocuscrystals"    , 58},
        {"ruthenium"               , 59},
        {"salvagedalloys"          , 60},
        {"selenium"                , 61},
        {"shieldemitters"          , 62},
        {"shieldingsensors"        , 63},
        {"sulphur"                 , 64},
        {"technetium"              , 65},
        {"tellurium"               , 66},
        {"temperedalloys"          , 67},
        {"thermicalloys"           , 68},
        {"tin"                     , 69},
        {"tungsten"                , 70},
        {"unknownenergysource"     , 71},
        {"vanadium"                , 72},
        {"wornshieldemitters"      , 73},
        {"yttrium"                 , 74},
        {"zinc"                    , 75},
        {"zirconium"               , 76},
    };

        static string getGradeMat(string alias)
        {
            int number = fdmatalias[alias];

            if (Array.IndexOf(new int[] { 1, 4, 18, 21, 35, 36, 43, 44, 48, 51, 55, 57, 59, 65, 66, 71 }, number )>=0)
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

        static void CheckAnthor()
        {
            foreach (KeyValuePair<string, int> k in fdmatalias)
            {
                string realname = matalias[k.Value];
                string grade = getGradeMat(k.Key);

                System.Diagnostics.Debug.WriteLine(k.Key + "=" + k.Value + "=" + realname + "=" + grade);

                MaterialCommodityDB db = GetCachedMaterial(k.Key);
                if (db == null)
                    System.Diagnostics.Debug.WriteLine("  ** NOT FOUND BY FDNAME");
                else if (db.name != realname)
                    System.Diagnostics.Debug.WriteLine("  ** Alias name disagres " + db.name + " vs " + realname);
                else if (db.type != grade)
                    System.Diagnostics.Debug.WriteLine("  ** Type disagres " + db.type + " vs " + grade);

            }

            foreach (KeyValuePair<string, int> k in fddataalias)
            {
                string realname = dataalias[k.Value];
                string grade = getGradeData(k.Key);
                System.Diagnostics.Debug.WriteLine(k.Key + "=" + k.Value + "=" + realname +"=" + grade);

                MaterialCommodityDB db = GetCachedMaterial(k.Key);
                if (db == null)
                    System.Diagnostics.Debug.WriteLine("  ** NOT FOUND BY FDNAME");
                else if (db.name != realname)
                    System.Diagnostics.Debug.WriteLine("  ** Alias name disagres " + db.name + " vs " + realname);
            }

        }

#endregion

#endif


    }
}


