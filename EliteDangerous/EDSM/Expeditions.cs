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
using System;

namespace EliteDangerousCore.EDSM
{
    public partial class EDSMClass
    {
        #region public static SavedRouteClass[] Expeditions

        public static SavedRouteClass[] Expeditions
        {
            get
            {
                // ReadMe:
                //      When logged into EDSM, start and end dates will be localized. Either set EDSM to use UTC or go incognito.
                //      For TBD entries: duplicate the prior system, and mark with a "TBD" comment for searching.
                //      Be extremely careful when changing any route names.
                return new SavedRouteClass[] {
                    #region 01: Distant Worlds (3302)
                    // https://www.edsm.net/en/expeditions/summary/id/1/name/Distant+Worlds+Expedition+%282016%29
                    new SavedRouteClass(
                        "Distant Worlds (3302)",
                        "Pallaeni",
                        "Fine Ring Sector JH-V c2-4",
                        "NGC 6530 WFI 16706",
                        "Omega Sector EL-Y d60",
                        "Eagle Sector EL-Y d203",
                        "NGC 6357 Sector DL-Y e22",
                        "Blaa Hypai UC-G c12-6",
                        "Greae Phio LS-L c23-221",
                        "Speamoea WU-E d12-543",
                        "Athaip CR-C b55-4",
                        "Myriesly EC-B c27-381",
                        "Stuemeae KM-W c1-342",
                        "Nyuena JS-B d342",
                        "Phipoea DD-F c26-1311",
                        "Dryao Chrea VU-P e5-7481",
                        "Eorld Byoe YA-W e2-4084",
                        "Eok Gree TO-Q e5-3167",
                        "Pheia Briae DK-A e303",
                        "Greeroi MD-Q d6-5",
                        "Rendezvous Point",
                        "Oupailks BB-M c8-5",
                        "Qautheia BA-A e0",
                        "Cheae Eurl AA-A e0",
                        "Beagle Point"
                    )
                    {
                        StartDate = new DateTime(2016, 1, 14, 20, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2016, 6, 5, 0, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                    },
                    #endregion

                    #region 02: Crab Nebula Expedition
                    // https://www.edsm.net/en/expeditions/summary/id/2/name/Crab+Nebula+Expedition
                    new SavedRouteClass(
                        "Crab Nebula Expedition",
                        "Epsilon Indi",
                        "Merope",
                        "Betelgeuse",
                        "Trapezium Sector DQ-X c1-9",
                        "2MASS J06410581+0952478",
                        "ALS 299",
                        "Seagull Sector MX-U c2-3",
                        "Rosette Sector HW-W c1-3",
                        "Jellyfish Sector DL-Y d50",
                        "Monkey Head Sector KC-V c2-3",
                        "Crab Pulsar"
                    )
                    {
                        StartDate = new DateTime(2016, 6, 11, 19, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2016, 7, 18, 20, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 03: Sagittarius-Carina Mission
                    // https://www.edsm.net/en/expeditions/summary/id/3/name/Sagittarius-Carina+Mission+counter-clockwise+%28camp+0+-%3E+36%29
                    new SavedRouteClass(
                        "Sagittarius-Carina Mission",
                        "Sol",
                        "NGC 6530 WFI 16706",
                        "Syralia JT-V b7-0",
                        "CSI-61-15434",
                        "CPD-65 2513",
                        "Thaile HW-V e2-7",
                        "GCRV 6807",
                        "Prue Hypa CL-Y g2",
                        "Pueliae IT-H d10-1",
                        "Grie Hypai DL-Y g2",
                        "Eock Prau WD-T d3-6",
                        "Mycapp TX-U d2-4",
                        "Eembaitl DL-Y d13",
                        "Hypaa Byio ZE-A g1",
                        "Gooroa PT-Q e5-5",
                        "Braitu EG-Y g1",
                        "Suvaa NL-P d5-29",
                        "Truechea SD-T d3-14",
                        "Hyphielie GR-N d6-9",
                        "Cho Eur QY-S e3-2",
                        "Fleckia FI-Z d1-6",
                        "Beagle Point",
                        "Myeia Thaa ZE-R d4-0",
                        "Syriae Thaa PJ-I d9-1",
                        "Pyrie Eurk QX-U e2-0",
                        "Cho Thua SF-W b29-0",
                        "Pyriveae FK-C d14-72",
                        "Tyroerts RX-U d2-0",
                        "Eactaify GD-A d14-18",
                        "Preia Flyuae XY-A e1865",
                        "13 MU SAGITTARII",
                        "Chraisa AY-F d12-133",
                        "Dryio Bloo PZ-W d2-1161",
                        "Stuemiae BB-O e6-61",
                        "Hypua Flyoae WU-X e1-4448",
                        "Oephaif RJ-G d11-408",
                        "Froaln II-W b7-1",
                        "Eodgorph IN-Z c14-32",
                        "CSI-06-19031",
                        "Omega Sector EL-Y d60",
                        "HIP 72043"
                    )
                    {
                        StartDate = new DateTime(2015, 8, 1, 19, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 8, 1, 12, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 04: Sagittarius-Carina Return
                    // https://www.edsm.net/en/expeditions/summary/id/4/name/Sagittarius-Carina+Mission+clockwise+%28camp+36+-%3E+0%29
                    new SavedRouteClass(
                        "Sagittarius-Carina Return",
                        "HIP 72043",
                        "Omega Sector EL-Y d60",
                        "CSI-06-19031",
                        "Eodgorph IN-Z c14-32",
                        "Froaln II-W b7-1",
                        "Oephaif RJ-G d11-408",
                        "Hypua Flyoae WU-X e1-4448",
                        "Stuemiae BB-O e6-61",
                        "Dryio Bloo PZ-W d2-1161",
                        "Chraisa AY-F d12-133",
                        "13 MU SAGITTARII",
                        "Preia Flyuae XY-A e1865",
                        "Eactaify GD-A d14-18",
                        "Tyroerts RX-U d2-0",
                        "Pyriveae FK-C d14-72",
                        "Cho Thua SF-W b29-0",
                        "Pyrie Eurk QX-U e2-0",
                        "Syriae Thaa PJ-I d9-1",
                        "Myeia Thaa ZE-R d4-0",
                        "Beagle Point",
                        "Fleckia FI-Z d1-6",
                        "Cho Eur QY-S e3-2",
                        "Hyphielie GR-N d6-9",
                        "Truechea SD-T d3-14",
                        "Suvaa NL-P d5-29",
                        "Braitu EG-Y g1",
                        "Gooroa PT-Q e5-5",
                        "Hypaa Byio ZE-A g1",
                        "Eembaitl DL-Y d13",
                        "Mycapp TX-U d2-4",
                        "Eock Prau WD-T d3-6",
                        "Grie Hypai DL-Y g2",
                        "Pueliae IT-H d10-1",
                        "Prue Hypa CL-Y g2",
                        "GCRV 6807",
                        "Thaile HW-V e2-7",
                        "CPD-65 2513",
                        "CSI-61-15434",
                        "Syralia JT-V b7-0",
                        "NGC 6530 WFI 16706",
                        "Sol"
                    )
                    {
                        StartDate = new DateTime(2015, 8, 1, 19, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 8, 1, 12, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 05: Western Expedition 3302
                    // https://www.edsm.net/en/expeditions/summary/id/5/name/Western+Expedition+3302
                    new SavedRouteClass(
                        "Western Expedition 3302",
                        "Beagle Point",
                        "Myeia Thaa ZE-R d4-0",
                        "Thueche LS-A d1-54",
                        "Thueche LS-A d1-54",   // TBD but never updated.
                        "Sol"
                    )
                    {
                        StartDate = new DateTime(2016, 4, 2, 0, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2016, 9, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 06: Borderlands Venture
                    // https://www.edsm.net/en/expeditions/summary/id/6/name/Borderlands+Venture
                    new SavedRouteClass(
                        "Borderlands Venture",
                        "Jitabos",
                        "Prooe Flyiae BK-P c22-0"
                    )
                    {
                        StartDate = new DateTime(2016, 6, 18, 14, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2018, 7, 31, 0, 42, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 07: Crab Nebula Expedition (Return 1)
                    // https://www.edsm.net/en/expeditions/summary/id/7/name/Crab+Nebula+Expedition+%28Return+1%29
                    new SavedRouteClass(
                        "Crab Nebula Expedition (Return 1)",
                        "Crab Pulsar",
                        "Trifid of the North Sector HR-W d1-8",
                        "Flaming Star Sector GW-W c1-5",
                        "California Sector BQ-Y d35",
                        "LBN 623 Sector FW-W d1-100",
                        "Epsilon Indi"
                    )
                    {
                        StartDate = new DateTime(2016, 7, 1, 20, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2016, 7, 18, 20, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 08: Crab Nebula Expedition (Return 2)
                    // https://www.edsm.net/en/expeditions/summary/id/8/name/Crab+Nebula+Expedition+%28Return+2%29
                    new SavedRouteClass(
                        "Crab Nebula Expedition (Return 2)",
                        "Crab Pulsar",
                        "NGC 1931 Sector PD-S b4-0",
                        "Soul Sector JC-V c2-16",
                        "Heart Sector BA-A e16",
                        "NGC 281 Sector PD-S b4-0",
                        "Bubble Sector LX-T b3-1",
                        "S171 19",
                        "Cave Sector JH-V c2-8",
                        "2MASS J21384622+5720380",
                        "North America Sector WU-O b6-2",
                        "Sadr Region Sector DL-Y d1",
                        "Veil East Sector DL-Y d37",
                        "BD+22 3878",
                        "Epsilon Indi"
                    )
                    {
                        StartDate = new DateTime(2016, 7, 1, 20, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2016, 8, 9, 20, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 09: August Exodus - A Jaunt to Jaques
                    // https://www.edsm.net/en/expeditions/summary/id/9/name/August+Exodus+-+Jaunt+to+Jaques
                    new SavedRouteClass(
                        "August Exodus - A Jaunt to Jaques",
                        "Ocshooit",
                        "Thor's Eye",
                        "Herschel 36",
                        "Omega Sector EL-Y d60",
                        "Eagle Sector EL-Y d203",
                        "Boewnst AA-A h33",
                        "Colonia",
                        "Eephaills SG-C c1-177",
                        "PSR J1846-0258",
                        "Flyiedgai UL-C d13-41",
                        "CSI-06-19031",
                        "BD+22 3878",
                        "He Xians"
                    )
                    {
                        StartDate = new DateTime(2016, 7, 24, 19, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2016, 10, 4, 19, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 10: Small Worlds Expedition
                    // https://www.edsm.net/en/expeditions/summary/id/10/name/Small+Worlds+Expedition
                    new SavedRouteClass(
                        "Small Worlds Expedition",
                        "Agarla",
                        "T Tauri",
                        "LBN 623 Sector JH-V c2-21",
                        "R CrA Sector DL-Y d12",
                        "Fine Ring Sector JH-V c2-4",
                        "KN Muscae",
                        "CD-39 6137",
                        "Blue planetary Sector NI-S b4-0",
                        "Bug Sector FW-W d1-28",
                        "Hen 2-215",
                        "NGC 6565 Sector KC-V c2-6"
                    )
                    {
                        StartDate = new DateTime(2016, 9, 17, 19, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2016, 10, 8, 20, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 11: Galactic Nebula Expedition
                    // https://www.edsm.net/en/expeditions/summary/id/11/name/Galactic+Nebula+Expedition
                    new SavedRouteClass(
                        "Galactic Nebula Expedition",
                        "BD+22 3878",
                        "Veil West Sector DL-Y d68",
                        "NGC 6820 Sector GW-W d1-0",
                        "Traikeou QJ-P d6-1",
                        "Prai Hypoo JA-E c11",
                        "Flyai Eaescs BG-N d7-1",
                        "Pra Eaewsy FB-I b53-0",
                        "Boeppy XZ-X d1-18",
                        "Oephaif KD-K d8-1878",
                        "Eol Prou PX-T d3-1752",
                        "Dryooe Prou GL-Y d369",
                        "Screaki EG-X e1-3485",
                        "Zuni KC-U e3-7670",
                        "Sagittarius A*"
                    )
                    {
                        StartDate = new DateTime(2016, 9, 3, 0, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2016, 11, 26, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 12: Colonia Core Circuit Expedition
                    // https://www.edsm.net/en/expeditions/summary/id/12/name/Colonia+Core+Circuit+Expedition+3302
                    new SavedRouteClass(
                        "Colonia Core Circuit Expedition",
                        "Colonia",
                        "Kyloaln GR-W e1-5394",
                        "Dumbaa XJ-R e4-5596",
                        "Great Annihilator",
                        "Athaip DW-N e6-3063",
                        "Myriesly EC-B c27-381",
                        "Sagittarius A*",
                        "Nyuena JS-B d342",
                        "Zunuae HL-Y e6903",
                        "Phroi Flyuae MN-S e4-4719",
                        "Wepae XE-Q e5-236",
                        "Screakou EW-N e6-5997",
                        "Kyloarph DV-Y e4134"
                    )
                    {
                        StartDate = new DateTime(2016, 11, 6, 0, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2016, 12, 30, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 13: S.H.E.P.A.R.D. Mission
                    // https://www.edsm.net/en/expeditions/summary/id/13/name/S.H.E.P.A.R.D.+Mission
                    new SavedRouteClass(
                        "S.H.E.P.A.R.D. Mission",
                        "Sol",
                        "MX Orionis",
                        "VY Canis Majoris",
                        "NGC 4463 Sector RY-R e4-5",
                        "CSI-61-15434",
                        "Eta Carina Sector IR-W d1-18",
                        "GCRV 6807",
                        "Pueliae NB-R c19-0",
                        "Oeshorps AM-C b13-0",
                        "Oob Free BV-X b43-0",
                        "Eock Bluae HV-Y d7",
                        "Systipoi GP-F b56-1",
                        "Flyai Flyuae WO-A c0",
                        "Roefaa AT-R b9-3",
                        "Qautheia BA-A e0",
                        "Beagle Point",
                        "Syriae Thaa PJ-I d9-1",
                        "Pyrivo DL-Y g5",
                        "Dryiquae OI-B d13-4",
                        "Bleethue NH-V d2-308",
                        "Phraa Byoe TB-B d14-25",
                        "Hypuae Briae AA-A b30-8",
                        "Aowroae GW-C c9",
                        "Scheau Bli AA-U b47-1",
                        "Eorld Byoe YA-W e2-4084",
                        "Dryuae Chruia AA-F c25-3007",
                        "Juenae WJ-R e4-898",
                        "Aunairm MH-V e2-658",
                        "Shrogaei DL-P e5-3604",
                        "Great Annihilator",
                        "Sagittarius A*",
                        "Eok Pruae PI-S e4-2295",
                        "Dryaa Flyi FH-U e3-9159",
                        "Byua Aim MR-W e1-91",
                        "Prua Phoe US-B d61",
                        "NGC 6820 Sector PD-S b4-6",
                        "Csi+19-20201",
                        "Veil West Sector DL-Y d66"
                    )
                    {
                        StartDate = new DateTime(2016, 11, 5, 18, 30, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 5, 5, 18, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 14: Christmas Carriers Convoy
                    // https://www.edsm.net/en/expeditions/summary/id/14/name/Christmas+Carriers+Convoy
                    new SavedRouteClass(
                        "Christmas Carriers Convoy",
                        "LTT 9846",
                        "Blu Thua AI-A c14-10",
                        "Lagoon Sector NI-S b4-10",
                        "Eagle Sector IR-W d1-117",
                        "Skaudai CH-B d14-34",
                        "Gru Hypue KS-T d3-31",
                        "Boewnst KS-S c20-959",
                        "Colonia"
                    )
                    {
                        StartDate = new DateTime(2016, 12, 2, 19, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 1, 2, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 15: Heavyweight Champions Circuit
                    // https://www.edsm.net/en/expeditions/summary/id/15/name/Heavyweight+Champions+Circuit
                    new SavedRouteClass(
                        "Heavyweight Champions Circuit",
                        "BD+31 2290",
                        "53 Tauri",
                        "Taygeta",
                        "HIP 12009",
                        "HD 210953",
                        "Plaa Eurk CF-A e3",
                        "Outorst VV-D d12-2",
                        "IC 1396 Sector TI-B d3",
                        "HIP 102167",
                        "Swoiwns ZK-W d2-26",
                        "HD 165921",
                        "HD 148937",
                        "HD 99399",
                        "HIP 38064",
                        "HIP 40430",
                        "BD+31 2290"
                    )
                    {
                        StartDate = new DateTime(2017, 1, 7, 19, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 2, 10, 20, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 16: La Grande Expédition Remlok
                    // https://www.edsm.net/en/expeditions/summary/id/16/name/The+Great+Remlok+Expedition+%7C+La+Grande+Exp%C3%A9dition+Remlok
                    new SavedRouteClass(
                        "La Grande Expédition Remlok",
                        "Tellus",
                        "HIP 72043",
                        "Nervi",
                        "Wredguia SW-U b57-1",
                        "Star Of India",
                        "Thailio AA-A h43",
                        "Traikeou AA-A h0",
                        "Flyai Eaescs BG-N d7-8",
                        "Boekh ZP-O e6-101",
                        "Eoch Flyuae HA-W b32-41",
                        "Eoch Pruae BP-A d994",
                        "Eoch Pruae CA-Z d832",
                        "Colonia",
                        "Sagittarius A*",
                        "Athaip GW-M c23-2801",
                        "Speamoea WU-E d12-543",
                        "Dryau Aowsy MR-W d1-6259",
                        "Froarks GM-D d12-355",
                        "Gria Hypue UG-S d5-779",
                        "Syralia JT-V b7-0",
                        "Blu Thua GI-B b55-2",
                        "Blu Thua JS-J d9-1",
                        "Praea Euq UU-F d11-0",
                        "Praea Euq KS-J d9-14",
                        "Eravate"
                    )
                    {
                        StartDate = new DateTime(2017, 1, 14, 22, 30, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 3, 31, 22, 59, 59, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 17: Mercury 7 Expedition
                    // https://www.edsm.net/en/expeditions/summary/id/17/name/Mercury+7+Expedition
                    new SavedRouteClass(
                        "Mercury 7 Expedition",
                        "Colonia",
                        "Wepio OD-B e68",
                        "Hypi Fraae VE-A b55-12",
                        "Dryu Chraea FH-D d12-49",
                        "Eos Brai KR-W e1-4",
                        "Hypi Bra CH-M d7-85",
                        "Smooraei AR-T d4-49",
                        "Tyreanie GU-D d13-56",
                        "Aiphaisty ZG-C c13-14",
                        "Oob Brue RI-U b9-0",
                        "Aiphaitls TS-U e2-61",
                        "Dryae Greau AA-A h19",
                        "Juenoe EG-Y g1913",
                        "Agnairt LN-T e3-3751",
                        "Ratraii"
                    )
                    {
                        StartDate = new DateTime(2017, 3, 5, 20, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 6, 11, 18, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 18: Pioniere and eXplorer - Die Expedition (PaX-DE)
                    // https://www.edsm.net/en/expeditions/summary/id/18/name/Pioniere+and+eXplorer+-+Die+Expedition+%28PaX-DE%29
                    new SavedRouteClass(
                        "Pioniere and eXplorer - Die Expedition (PaX-DE)",
                        "Toluku",
                        "Blu Thua AI-A c14-10",
                        "Lagoon Sector NI-S b4-10",
                        "Eagle Sector IR-W d1-117",
                        "Skaudai CH-B d14-34",
                        "Gru Hypue KS-T d3-31",
                        "Boewnst KS-S c20-959",
                        "Colonia",
                        "Dryo Aob ZZ-F d11-50",
                        "Bleou Airgh ZJ-X d2-0",
                        "Zejoo RR-Q b52-6",
                        "Dehoae TU-W b19-0",
                        "Thraikai TX-A b30-0",
                        "Drojai MC-B d1-35",
                        "Synuefoi MN-A d1-71",
                        "Rho Cassiopeiae",
                        "Toluku",
                        "Epsilon Indi"
                    )
                    {
                        StartDate = new DateTime(2017, 2, 5, 19, 35, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 4, 29, 21, 30, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 19: Land of Giants Expedition
                    // https://www.edsm.net/en/expeditions/summary/id/19/name/Land+of+Giants+Expedition
                    new SavedRouteClass(
                        "Land of Giants Expedition",
                        "Colonia",
                        "Eol Flyou WJ-R e4-43",
                        "Sphoepps MI-Z d1-25",
                        "Floaln OT-Z d13-7",
                        "Floalk AA-A h35",
                        "Floagh YP-X e1-4",
                        "Stuelua SI-B e12",
                        "Grie Hypooe ZA-W e2-6",
                        "Floasly KD-H d11-62",
                        "Eephaills KX-U f2-733"
                    )
                    {
                        StartDate = new DateTime(2017, 7, 9, 12, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 8, 10, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 20: Local Sightseeing Tour
                    // https://www.edsm.net/en/expeditions/summary/id/20/name/Local+Sightseeing+Tour
                    new SavedRouteClass(
                        "Local Sightseeing Tour",
                        "I Carinae",
                        "Synuefe TP-F b44-0",
                        "HIP 17403",
                        "Betelgeuse",
                        "Eskimo Sector UJ-Q b5-0",
                        "Witch Head Sector GW-W c1-9",
                        "V951 Orionis",
                        "Trapezium Sector CB-U b4-0",
                        "Trapezium Sector AF-Z c4",
                        "California Sector BA-A e6",
                        "North America Sector JH-V c2-26",
                        "Sadr Region Sector DL-Y d1",
                        "Veil West Sector KC-V c2-18",
                        "BD+22 3878",
                        "Epsilon Indi"
                    )
                    {
                        StartDate = new DateTime(2017, 5, 6, 17, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 7, 1, 20, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 21: Helicon's Peak Expedition
                    // https://www.edsm.net/en/expeditions/summary/id/21/name/Helicon%27s+Peak+Expedition
                    new SavedRouteClass(
                        "Helicon's Peak Expedition",
                        "Maia",
                        "HIP 11792",
                        "IC 289 Sector DL-Y d17",
                        "NSV 1056",
                        "HD 19820",
                        "Hegeia EG-Y g2",
                        "IC 1805 Sector AV-O c6-6",
                        "IC 1805 Sector DQ-Y e3",
                        "Soul Sector EL-Y d7",
                        "Heart Sector IR-V b2-0",
                        "Eafots RA-G b11-0",
                        "Eafots LZ-H b10-0",
                        "Eafots EU-R c4-1",
                        "Eafots GL-Y e2",
                        "Myoideau UA-F d11-0",
                        "Myoideau RF-N c20-1",
                        "Myoideau EN-S c17-1",
                        "Myoideau ZG-U c16-0",
                        "Myoideau XL-U c16-0",
                        "Myoideau OZ-X c14-0",
                        "Myoideau CH-D c12-0",
                        "Phroea Hypa SA-U d4-0",
                        "Myoideau GW-N c6-0",
                        "Myoideau DL-P c5-0",
                        "Myoideau KM-W d1-2",
                        "Phroea Hypa PE-B b6-0",
                        "Schee Hypa WS-L b55-0",
                        "Schee Hypa FN-I c26-0",
                        "Schee Hypa IU-S b51-0",
                        "Schee Hypa RO-P c22-0",
                        "Schee Hypa JC-T c20-0",
                        "Schee Hypa SL-J d10-1",
                        "Hypoae Aescs UG-J b36-0",
                        "Hypoae Aescs KX-L d7-5",
                        "Hypoae Aescs XF-W c15-0",
                        "Hypoae Aescs TZ-X c14-0",
                        "Hypoae Aescs RO-Z c13-0",
                        "Hypoae Aescs CL-P d5-0",
                        "Hypoae Aescs YE-R d4-2",
                        "Syreadiae JX-F c0"
                    )
                    {
                        StartDate = new DateTime(2017, 5, 6, 22, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 6, 6, 22, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 22: Small Worlds Expedition 2
                    // https://www.edsm.net/en/expeditions/summary/id/22/name/Small+Worlds+Expedition+2
                    new SavedRouteClass(
                        "Small Worlds Expedition 2",
                        "BD+31 2290",
                        "T Tauri",
                        "HIP 23759",
                        "GMM2008 22",
                        "LBN 623 Sector FW-W d1-100",
                        "Veil West Sector DL-Y d68",
                        "Blinking Sector PD-S b4-0",
                        "Ring Sector NI-S b4-0",
                        "Blue Flash Sector GW-W c1-0",
                        "Lan 111",
                        "Crescent Sector HR-W d1-28",
                        "NGC 6820 Sector PD-S b4-6"
                    )
                    {
                        StartDate = new DateTime(2017, 6, 3, 18, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 7, 8, 18, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 23: Miskatonic University Galactic Expedition (3303-3304)
                    // https://www.edsm.net/en/expeditions/summary/id/23/name/Miskatonic+University+Galactic+Expedition+3303+-+3304
                    new SavedRouteClass(
                        "Miskatonic University Galactic Expedition (3303-3304)",
                        "Sothis",
                        "Celaeno",
                        "Betelgeuse",
                        "Rigel",
                        "Wezen",
                        "Hyades Sector AA-P b6-5",
                        "Elysia",
                        "Sargas",
                        "Algol",
                        "Aldebaran",
                        "Arcturus",
                        "EZ Aquarii",
                        "Fomalhaut",
                        "Castor",
                        "Vega"
                    )
                    {
                        StartDate = new DateTime(2017, 8, 20, 23, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2018, 5, 15, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 24: Summer Great Expedition (Route A)
                    // https://www.edsm.net/en/expeditions/summary/id/24/name/Summer+Great+Expedition+%28Route+A%29
                    new SavedRouteClass(
                        "Summer Great Expedition (Route A)",
                        "HR 6421",
                        "Eta Carina Sector MX-U c2-8",
                        "Plaa Aec IZ-N c20-1",
                        "Eord Prau UJ-P d6-795",
                        "Pheia Auscs PT-G d11-445",
                        "Greeroi MD-Q d6-5",
                        "Rendezvous Point",
                        "Qautheia BA-A e0",
                        "Cheae Eurl AA-A e0",
                        "Beagle Point"
                    )
                    {
                        StartDate = new DateTime(2017, 5, 28, 19, 15, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 7, 9, 19, 15, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 25: Summer Great Expedition (Route B)
                    // https://www.edsm.net/en/expeditions/summary/id/25/name/Summer+Great+Expedition+%28Route+B%29
                    new SavedRouteClass(
                        "Summer Great Expedition (Route B)",
                        "HR 6421",
                        "Eta Carina Sector MX-U c2-8",
                        "Plaa Aec IZ-N c20-1",
                        "Sagittarius A*",
                        "Edge Fraternity Landing",
                        "Pru Aescs HW-S b31-2",
                        "Jitabos"
                    )
                    {
                        StartDate = new DateTime(2017, 5, 28, 19, 15, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 7, 9, 19, 15, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    // 26: Not public? Deleted? Nonexistent?

                    #region 27: Devos Expedition Program #1
                    // https://www.edsm.net/en/expeditions/summary/id/27/name/Devos+Expedition+Program+%231
                    new SavedRouteClass(
                        "Devos Expedition Program #1",
                        "Kaushpoos",
                        "Ophiuchus Dark Region B Sector PD-S b4-2",
                        "Lagoon Sector NI-S b4-10",
                        "NGC 6530 WFI 16706",
                        "Traikaae KT-P d6-10",
                        "Omega Sector VE-Q b5-15"
                    )
                    {
                        StartDate = new DateTime(2017, 7, 22, 22, 30, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 9, 22, 22, 30, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    // 28: Not public? Deleted? Nonexistent?

                    #region 29: Beagle Point Expedition
                    // https://www.edsm.net/en/expeditions/summary/id/29/name/Beagle+Point+Expedition
                    new SavedRouteClass(
                        "Beagle Point Expedition",
                        "Colonia",
                        "Kojeara",
                        "Schee Flyi BF-A e7011",
                        "Athaip ZP-P e5-6688",
                        "Stuemeae KM-W c1-342",
                        "Eorld Byoe YA-W e2-4084",
                        "Eok Gree TO-Q e5-3167",
                        "Pheia Briae DK-A e303",
                        "Greeroi MD-Q d6-5",
                        "Rendezvous Point",
                        "Oupailks BB-M c8-5",
                        "Qautheia BA-A e0",
                        "Cheae Eurl AA-A e0",
                        "Beagle Point",
                        "Myeia Thaa ZE-R d4-0",
                        "Pyrie Eurk QX-U e2-0",
                        "Pyriveae FK-C d14-72",
                        "Tyroerts AA-A g2",
                        "Eactaify EG-Y g5",
                        "Preia Flyuae XY-A e1865",
                        "13 MU SAGITTARII",
                        "15 Sagittarii",
                        "Dryio Bloo PZ-W d2-1161",
                        "Stuemiae BB-O e6-61",
                        "Hypua Flyoae WU-X e1-4448",
                        "Dryooe Prou HH-C d1536",
                        "Centralis"
                    )
                    {
                        StartDate = new DateTime(2018, 1, 20, 12, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2018, 9, 15, 12, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 30: DSN Luxury Tour
                    // https://www.edsm.net/en/expeditions/summary/id/30/name/DSN+Luxury+Tour
                    new SavedRouteClass(
                        "DSN Luxury Tour",
                        "T Tauri",
                        "Lagoon Sector FW-W d1-122",
                        "Trifid Sector IR-W d1-52",
                        "Omega Sector VE-Q b5-15",
                        "Eagle Sector IR-W d1-105",
                        "Crescent Sector GW-W c1-8",
                        "Sadr Region Sector GW-W c1-22",
                        "FW Cephei",
                        "GM Cephei",
                        "NGC 7822 Sector BQ-Y d12",
                        "Heart Sector IR-V b2-0",
                        "Soul Sector EL-Y d7",
                        "Crab Sector DL-Y d9",
                        "Jellyfish Sector FB-X c1-5",
                        "Rosette Sector CQ-Y d59",
                        "Seagull Sector DL-Y d3",
                        "Thor's Helmet Sector FB-X c1-5",
                        "Flaming Star Sector LX-T b3-0",
                        "PMD2009 48",
                        "HIP 23759",
                        "Pencil Sector EL-Y d5"
                    )
                    {
                        StartDate = new DateTime(2017, 10, 22, 0, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 12, 17, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 31: Minerva Centaurus Expedition
                    // https://www.edsm.net/en/expeditions/summary/id/31/name/Minerva+Centaurus+Expedition
                    new SavedRouteClass(
                        "Minerva Centaurus Expedition",
                        "HIP 72043",
                        "Nyeakio RU-V d3-21",
                        "Prooe Phio PZ-Z c0",
                        "Droeth FS-K c8-36",
                        "Phrooe Flyuae QE-Y d1-4",
                        "Oob Chrea CV-U b30-3",
                        "Kyloopeia BA-A g4",
                        "Byeethiae TM-L c21-11",
                        "Preou Free VC-F c26-2",
                        "Byooe Aoscs BG-F d11-14",
                        "Throefuae VZ-O e6-16",
                        "Plio Broae JM-W d1-100",
                        "Byeia Aoscs PS-S c17-6",
                        "Bleethai UP-J c24-58",
                        "Dryoea Gree BW-B a40-5",
                        "Hypiae Ausms JU-I c12-1",
                        "Colonia"
                    )
                    {
                        StartDate = new DateTime(2017, 11, 19, 19, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2018, 3, 11, 19, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 32: The Dead End's Circumnavigation Expedition
                    // https://www.edsm.net/en/expeditions/summary/id/32/name/The+Dead+End%27s+Circumnavigation+Expedition
                    new SavedRouteClass(
                        "Dead End's Circumnavigation",
                        "HIP 23759",
                        "Crab Sector DL-Y d9",
                        "3 Geminorum",
                        "Angosk DL-P d5-0",
                        "Angosk OM-W d1-0",
                        "Lyed YJ-I d9-0",
                        "Hypuae Euq ZK-P d5-0",
                        "Aicods KD-K d8-3",
                        "Syroifoe CL-Y g1",
                        "HIP 117078",
                        "Spongou FA-A e2",
                        "Cyuefai BC-D d12-4",
                        "Cyuefoo LC-D d12-0",
                        "Byaa Thoi EW-E d11-0",
                        "Byaa Thoi GC-D d12-0",
                        "Auzorts NR-N d6-0",
                        "Lyruewry BK-R d4-12",
                        "Hypou Chreou RS-S c17-6",
                        "Hypiae Brue DI-D c12-0",
                        "Sphiesi HX-L d7-0",
                        "Flyae Proae IN-S e4-1",
                        "Footie AA-A g0",
                        "Oedgaf DL-Y g0",
                        "Gria Bloae YE-A g0",
                        "Exahn AZ-S d3-8",
                        "Chua Eop ZC-T c20-0",
                        "Beagle Point",
                        "Cheae Eurl AA-A e0",
                        "Hyphielia QH-K c22-0",
                        "Praei Bre WO-R d4-3",
                        "Suvua FG-Y f0",
                        "Hypaa Byio ZE-A g1",
                        "Eembaitl DL-Y d13",
                        "Synookaea MX-L d7-0",
                        "Blea Airgh EI-B d13-1",
                        "Ood Fleau ZJ-I d9-0",
                        "Plae Eur DW-E d11-0",
                        "Haffner 18 LSS 27",
                        "Achrende"
                    )
                    {
                        StartDate = new DateTime(2017, 10, 14, 0, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2018, 10, 13, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 33: Distant Friends Expedition
                    // https://www.edsm.net/en/expeditions/summary/id/33/name/Distant+Friends+Expedition
                    new SavedRouteClass(
                        "Distant Friends Expedition",
                        "Sol",
                        "Beagle Point"
                    )
                    {
                        StartDate = new DateTime(2017, 11, 7, 12, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2018, 1, 7, 16, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 34: Knights of Karma INRA Expedition
                    // https://www.edsm.net/en/expeditions/summary/id/34/name/Knights+of+Karma+INRA+Expedition
                    new SavedRouteClass(
                        "Knights of Karma INRA Expedition",
                        "Diaguandri",
                        "HIP 15329",
                        "Hermitage",
                        "Alnath",
                        "HIP 59382",
                        "HIP 7158",
                        "LP 389-95",
                        "Conn",
                        "HIP 16824",
                        "HIP 12099",
                        "12 Trianguli",
                        "Qa'wakana"
                    )
                    {
                        // Yes, it really was only a few hours long...
                        StartDate = new DateTime(2017, 11, 25, 17, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 11, 25, 23, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 35: Orange Run
                    // https://www.edsm.net/en/expeditions/summary/id/35/name/Orange+run
                    new SavedRouteClass(
                        "Orange Run",
                        "Tembala",
                        "HD 175876",
                        "Lagoon Sector NI-S b4-10",
                        "Blae Hypue VN-S d4-21",
                        "Blua Hypa HT-F d12-1226",
                        "Boewnst KS-S c20-959",
                        "Colonia",
                        "Dryaea Flee HR-W e1-124",
                        "Scheau Flyi KR-W e1-6736",
                        "Shrogea MH-V e2-1763",
                        "Sagittarius A*",
                        "Oupailks CW-L c8-1",
                        "Cheae Euq ER-L c21-0",
                        "Beagle Point",
                        "Greae Bluae YE-A g3",
                        "Oedgaf DL-Y g0",
                        "Flyoo Groa SO-Z e0",
                        "Hypi Bra TI-B d25",
                        "Eos Brai KR-W e1-4",
                        "Byua Aeb BA-A g2",
                        "Thraikio ON-B d13-2",
                        "Byaa Thoi GC-D d12-0",
                        "Blia Thio RY-S e3-5",
                        "Plooe Thio OA-A d14",
                        "Bubble Sector FB-X c1-25",
                        "Heart Sector ZE-A e11",
                        "Soul Sector EL-Y d7",
                        "S171 36",
                        "Tembala"
                    )
                    {
                        StartDate = new DateTime(2017, 12, 15, 15, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2018, 4, 24, 15, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 36: Christmas Carriers Convoy 2
                    // https://www.edsm.net/en/expeditions/summary/id/36/name/Christmas+Carriers+Convoy+2
                    new SavedRouteClass(
                        "Christmas Carriers 2",
                        "Haritanis",
                        "Blu Thua AI-A c14-10",
                        "Lagoon Sector NI-S b4-10",
                        "Eagle Sector IR-W d1-117",
                        "Skaudai CH-B d14-34",
                        "Gru Hypue KS-T d3-31",
                        "Boewnst KS-S c20-959",
                        "Centralis"
                    )
                    {
                        StartDate = new DateTime(2017, 12, 2, 0, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 12, 24, 23, 59, 59, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 37: Christmas Delights
                    // https://www.edsm.net/en/expeditions/summary/id/37/name/Christmas+Delights
                    new SavedRouteClass(
                        "Christmas Delights",
                        "Wapiya",
                        "Karsuki Ti",
                        "Jaroua",
                        "Goman",
                        "Any Na",
                        "Arouca",
                        "Deuringas",
                        "Aegaeon",
                        "Witchhaul",
                        "Hecate",
                        "Thrutis",
                        "Kongga",
                        "Esuseku",
                        "Harma",
                        "Kamitra",
                        "Njambalba",
                        "Wapiya"
                    )
                    {
                        StartDate = new DateTime(2017, 12, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2017, 12, 24, 23, 59, 59, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion

                    #region 38: Enigma Expedition
                    // https://www.edsm.net/en/expeditions/summary/id/38/name/Enigma+Expedition
                    new SavedRouteClass(
                        "Enigma Expedition",
                        "Jackson's Lighthouse",
                        "CSI-21-22270",
                        "Mammon",
                        "46 Upsilon Sagittarii",
                        "HD 175876",
                        "Red Spider Sector UJ-Q b5-0",
                        "Thor's Eye",
                        "Omega Sector PD-S b4-0",
                        "Rohini",
                        "Pru Aescs HW-S b31-2",
                        "Skaudai AM-B d14-138",
                        "Nuekuae AA-A h52",
                        "Blaa Phoe NC-D d12-230",
                        "Boewnst KS-S c20-959",
                        "Kashyapa",
                        "Colonia"
                    )
                    {
                        StartDate = new DateTime(2018, 1, 12, 19, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                        EndDate = new DateTime(2018, 2, 10, 22, 0, 0, DateTimeKind.Utc).ToLocalTime()
                    },
                    #endregion
                };
            }
        }

        #endregion
    }
}
