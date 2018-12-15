using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
{
    public class BodyDesignations
    {
        public static Dictionary<string, Dictionary<string, string>> Stars = new Dictionary<string, Dictionary<string, string>>
        {
            { "127 G. Canis Majoris", new Dictionary<string, string> {
                { "127 G. Canis Major C", "127 G. Canis Majoris B" },
            } },
            { "14 Aurigae", new Dictionary<string, string> {
                { "14 Aurigae C", "14 Aurigae B" },
            } },
            { "21 Draco", new Dictionary<string, string> {
                { "Wo 9584 C", "21 Draco B" },
            } },
            { "ADS 4229 ABC", new Dictionary<string, string> {
                { "26 Aurigae C", "ADS 4229 ABC B" },
            } },
            { "BC Arietis", new Dictionary<string, string> {
                { "Gl 113 C", "BC Arietis B" },
            } },
            { "Omicron Capricorni B", new Dictionary<string, string> {
                { "Omicron Capricorni", "Omicron Capricorni B B" },
            } },
            { "5 Aquilae", new Dictionary<string, string> {
                { "5 Aquilae Ab", "5 Aquilae B" },
                { "5 Aquilae B", "5 Aquilae C" },
            } },
            { "CCDM J14234+0827A", new Dictionary<string, string> {
                { "BD+09 2882B", "CCDM J14234+0827A B" },
                { "BD+09 2882C", "CCDM J14234+0827A C" },
            } },
            { "CM Draconis", new Dictionary<string, string> {
                { "CM Draconis Aa", "CM Draconis B" },
                { "Gliese 630.1 B", "CM Draconis C" },
            } },
            { "Gamma Arae", new Dictionary<string, string> {
                { "CPD-56 8225B", "Gamma Arae B" },
                { "CPD-56 8225C", "Gamma Arae C" },
            } },
            { "Set", new Dictionary<string, string> {
                { "Shango", "Set B" },
                { "Oya", "Set C" },
            } },
            { "ADS 9338 AC", new Dictionary<string, string> {
                { "BD+17 2768 C", "ADS 9338 AC B" },
                { "29 Bootis", "ADS 9338 AC C" },
                { "29 Bootis B", "ADS 9338 AC D" },
            } },
            { "Dabih Major", new Dictionary<string, string> {
                { "Beta Capricorni Ab", "Dabih Major B" },
                { "Beta Capricorni B", "Dabih Major C" },
                { "Beta Capricorni C", "Dabih Major D" },
            } },
            { "Mizar", new Dictionary<string, string> {
                { "Mizar Ab", "Mizar B" },
                { "Mizar B", "Mizar C" },
                { "Mizar Bb", "Mizar D" },
            } },
            { "Castor", new Dictionary<string, string> {
                { "Castor Ab", "Castor B" },
                { "Castor B", "Castor C" },
                { "Castor Bb", "Castor D" },
                { "Castor C", "Castor E" },
                { "Castor Cb", "Castor F" },
            } },
            { "39 Tauri", new Dictionary<string, string> {
                { "39 Tauri B", "39 Tauri B" },
            } },
            { "70 Ophiuchi", new Dictionary<string, string> {
                { "70 Ophiuchi B", "70 Ophiuchi B" },
            } },
            { "Asellus Primus", new Dictionary<string, string> {
                { "Asellus Primus B", "Asellus Primus B" },
            } },
            { "Capella", new Dictionary<string, string> {
                { "Capella B", "Capella B" },
            } },
            { "Eta Cassiopeiae", new Dictionary<string, string> {
                { "Eta Cassiopeiae B", "Eta Cassiopeiae B" },
            } },
            { "Firdaus", new Dictionary<string, string> {
                { "Firdaus B", "Firdaus B" },
            } },
            { "Groombridge 34", new Dictionary<string, string> {
                { "Groombridge 34 B", "Groombridge 34 B" },
            } },
            { "Lanaest", new Dictionary<string, string> {
                { "Lanaest B", "Lanaest B" },
            } },
            { "Liaedin", new Dictionary<string, string> {
                { "Liaedin B", "Liaedin B" },
            } },
            { "Miola", new Dictionary<string, string> {
                { "Miola B", "Miola B" },
            } },
            { "Morten-Marte", new Dictionary<string, string> {
                { "Morten-Marte B", "Morten-Marte B" },
            } },
            { "Sirius", new Dictionary<string, string> {
                { "Sirius B", "Sirius B" },
            } },
            { "BD+47 2112", new Dictionary<string, string> {
                { "Gliese 537 B", "BD+47 2112 B" },
            } },
            { "Chi Eridani", new Dictionary<string, string> {
                { "Gl 81 B", "Chi Eridani B" },
            } },
            { "Forculus", new Dictionary<string, string> {
                { "Wolf 853 B", "Forculus B" },
            } },
            { "LFT 992", new Dictionary<string, string> {
                { "Gliese 507 B", "LFT 992 B" },
            } },
            { "Manah", new Dictionary<string, string> {
                { "Gl 150.1 B", "Manah B" },
            } },
            { "Nuakea", new Dictionary<string, string> {
                { "Gliese 678 B", "Nuakea B" },
            } },
            { "Ovid", new Dictionary<string, string> {
                { "Gliese 611 B", "Ovid B" },
            } },
            { "Ququve", new Dictionary<string, string> {
                { "NN 3179 B", "Ququve B" },
            } },
            { "Rho Cancri", new Dictionary<string, string> {
                { "55 Rho-1 Cancri B", "Rho Cancri B" },
            } },
            { "Stopover", new Dictionary<string, string> {
                { "Gl 783 B", "Stopover B" },
            } },
            { "Teshub", new Dictionary<string, string> {
                { "Gliese 59 B", "Teshub B" },
            } },
            { "Witch's Reach", new Dictionary<string, string> {
                { "Witch's Peak B", "Witch's Reach B" },
            } },
            { "Wolf 573", new Dictionary<string, string> {
                { "Gl 596.1 B", "Wolf 573 B" },
            } },
            { "Wyrd", new Dictionary<string, string> {
                { "Gl 508 B", "Wyrd B" },
            } },
            { "Zeessze", new Dictionary<string, string> {
                { "Gl 53 B", "Zeessze B" },
            } },
            { "BU 741", new Dictionary<string, string> {
                { "Gliese 120.1 B", "BU 741 B" },
                { "Gliese 120.1 C", "BU 741 C" },
            } },
            { "LDS 883", new Dictionary<string, string> {
                { "Gliese 118.2 B", "LDS 883 B" },
                { "Gliese 118.2 C", "LDS 883 C" },
            } },
            { "Omicron-2 Eridani", new Dictionary<string, string> {
                { "Omicron Eridani B", "Omicron-2 Eridani B" },
                { "Omicron Eridani C", "Omicron-2 Eridani C" },
            } },
            { "Epsilon Indi", new Dictionary<string, string> {
                { "Epsilon Indi Ba", "Epsilon Indi B" },
            } },
            { "Opala", new Dictionary<string, string> {
                { "LHS 403", "Opala B" },
            } },
            { "Shinrarta Dezhra", new Dictionary<string, string> {
                { "LTT 4550", "Shinrarta Dezhra B" },
            } },
            { "Tun", new Dictionary<string, string> {
                { "Ton", "Tun B" },
            } },
            { "LHS 450", new Dictionary<string, string> {
                { "BD+68 946", "LHS 450 B" },
            } },
            { "Alpha Centauri", new Dictionary<string, string> {
                { "Alpha Centauri B", "Alpha Centauri B" },
                { "Proxima Centauri", "Alpha Centauri C" },
            } },
            { "17 Draconis", new Dictionary<string, string> {
                { "17 Draconis B", "17 Draconis B" },
                { "16 Draconis", "17 Draconis C" },
                { "16 Draconis B", "17 Draconis D" },
            } },
            { "i Bootis", new Dictionary<string, string> {
                { "i Bootis B", "i Bootis B" },
                { "i Bootis C", "i Bootis C" },
                { "Maher", "i Bootis D" },
            } },
            { "Prism", new Dictionary<string, string> {
                { "Ruby", "Prism B" },
                { "Diamond", "Prism C" },
                { "Sapphire", "Prism D" },
            } },
            { "Alkalurops", new Dictionary<string, string> {
                { "51 Mu-2 Bootis B", "Alkalurops B" },
                { "51 Mu-2 Bootis C", "Alkalurops C" },
            } },
            { "Atanua", new Dictionary<string, string> {
                { "Gliese 153 B", "Atanua B" },
                { "Gliese 153 C", "Atanua C" },
            } },
            { "Cuspoor", new Dictionary<string, string> {
                { "Gliese 667 B", "Cuspoor B" },
                { "Gliese 667 C", "Cuspoor C" },
            } },
            { "69 G. Carinae", new Dictionary<string, string> {
                { "Gliese 294 B", "69 G. Carinae B" },
                { "Gliese 294 C", "69 G. Carinae C" },
            } },
            { "EGM 823", new Dictionary<string, string> {
                { "Gliese 60 B", "EGM 823 B" },
                { "Gliese 60 C", "EGM 823 C" },
            } },
            { "Ferez", new Dictionary<string, string> {
                { "Gliese 421 B", "Ferez B" },
                { "Gliese 421 C", "Ferez C" },
            } },
            { "LFT 601", new Dictionary<string, string> {
                { "Gl 319 B", "LFT 601 B" },
                { "Gl 319 C", "LFT 601 C" },
            } },
            { "LHS 115", new Dictionary<string, string> {
                { "Gliese 22 B", "LHS 115 B" },
                { "Gliese 22 C", "LHS 115 C" },
            } },
            { "LHS 1393", new Dictionary<string, string> {
                { "GJ 1047 B", "LHS 1393 B" },
                { "GJ 1047 C", "LHS 1393 C" },
            } },
            { "LHS 1409", new Dictionary<string, string> {
                { "Gliese 100 B", "LHS 1409 B" },
                { "Gliese 100 C", "LHS 1409 C" },
            } },
            { "LHS 2875", new Dictionary<string, string> {
                { "NN 3829 B", "LHS 2875 B" },
                { "NN 3832 C", "LHS 2875 C" },
            } },
            { "LP 356-14", new Dictionary<string, string> {
                { "Gliese 140 B", "LP 356-14 B" },
                { "Gliese 140 C", "LP 356-14 C" },
            } },
            { "PW Hydrae", new Dictionary<string, string> {
                { "NN 3608 B", "PW Hydrae B" },
                { "NN 3609 C", "PW Hydrae C" },
            } },
            { "Sango", new Dictionary<string, string> {
                { "Gliese 586 B", "Sango B" },
                { "Gliese 586 C", "Sango C" },
            } },
            { "Talitha", new Dictionary<string, string> {
                { "Gliese 331 B", "Talitha B" },
                { "Gliese 331 C", "Talitha C" },
            } },
            { "V1090 Herculis", new Dictionary<string, string> {
                { "Gliese 649.1 B", "V1090 Herculis B" },
                { "Gliese 649.1 C", "V1090 Herculis C" },
            } },
            { "9 Aurigae", new Dictionary<string, string> {
                { "9 Aurigae B", "9 Aurigae A" },
                { "9 Aurigae", "9 Aurigae B" },
            } },
            { "IGR J17497-2821", new Dictionary<string, string>
            {
                { "IGR J17497-2821 X", "IGR J17497-2821 B" },
            } },

        };

        public static Dictionary<string, Dictionary<string, string>> Planets = new Dictionary<string, Dictionary<string, string>>
        {
            { "39 Tauri", new Dictionary<string, string> {
                { "Ad Pontes", "39 Tauri A 1" },
            } },
            { "70 Ophiuchi", new Dictionary<string, string> {
                { "Perez", "70 Ophiuchi A 1" },
                { "Denver's Legacy", "70 Ophiuchi A 2" },
                { "Richardson", "70 Ophiuchi A 3" },
            } },
            { "Asellus Primus", new Dictionary<string, string> {
                { "Asellus 1", "Asellus Primus A 1" },
                { "Asellus 2", "Asellus Primus A 2" },
                { "Asellus 3", "Asellus Primus A 3" },
                { "A Ring", "Asellus Primus A 3 A Ring" },
                { "B Ring", "Asellus Primus A 3 B Ring" },
                { "C Ring", "Asellus Primus A 3 C Ring" },
                { "D Ring", "Asellus Primus A 3 D Ring" },
                { "Asellus 3a", "Asellus Primus A 3 a" },
                { "Asellus 3b", "Asellus Primus A 3 b" },
                { "Asellus 3c", "Asellus Primus A 3 c" },
                { "Asellus 4", "Asellus Primus A 4" },
            } },
            { "Capella", new Dictionary<string, string> {
                { "Duval's Grave", "Capella A 6 a" },
                { "Gold", "Capella A 6 b" },
                { "Nobleworld", "Capella A 6 c" },
                { "Molotov's Claim", "Capella A 6 d" },
                { "Lawrence's Grave", "Capella A 6 e" },
            } },
            { "Eta Cassiopeiae", new Dictionary<string, string> {
                { "Rock", "Eta Cassiopeiae A 1" },
                { "Feynman", "Eta Cassiopeiae A 2" },
                { "Trojan", "Eta Cassiopeiae A 3" },
                { "New Moon", "Eta Cassiopeiae A 3 a" },
                { "Between", "Eta Cassiopeiae A 4" },
                { "Navy Central", "Eta Cassiopeiae A 4 a" },
            } },
            { "Firdaus", new Dictionary<string, string> {
                { "Borealis (HD 7199 b)", "Firdaus A 3" },
            } },
            { "Groombridge 34", new Dictionary<string, string> {
                { "Gold", "Groombridge 34 A 1" },
                { "Jaya", "Groombridge 34 A 1 a" },
                { "New America", "Groombridge 34 B 1" },
                { "Lyons", "Groombridge 34 B 2" },
            } },
            { "Lanaest", new Dictionary<string, string> {
                { "Lanaest I", "Lanaest B 1" },
                { "Lanaest II", "Lanaest B 2" },
                { "Lanaest III", "Lanaest B 3" },
            } },
            { "Liaedin", new Dictionary<string, string> {
                { "Ulrich's Rock", "Liaedin A 1" },
                { "Moore's World", "Liaedin A 2" },
                { "Schneider Colony", "Liaedin A 3" },
                { "Camp Ashfield", "Liaedin A 3 a" },
            } },
            { "Miola", new Dictionary<string, string> {
                { "Minta", "Miola A 1" },
            } },
            { "Morten-Marte", new Dictionary<string, string> {
                { "Cullen", "Morten-Marte A 1" },
            } },
            { "Sirius", new Dictionary<string, string> {
                { "Waypoint", "Sirius A 1" },
                { "Lucifer", "Sirius B 1" },
            } },
            { "BD+47 2112", new Dictionary<string, string> {
                { "Derwent", "BD+47 2112 A 1" },
            } },
            { "Chi Eridani", new Dictionary<string, string> {
                { "Shirtuh", "Chi Eridani A 8" },
                { "Fansipan", "Chi Eridani A 8 a" },
            } },
            { "Forculus", new Dictionary<string, string> {
                { "Domus", "Forculus A 1" },
            } },
            { "LFT 992", new Dictionary<string, string> {
                { "Uwei", "LFT 992 A 1" },
            } },
            { "Manah", new Dictionary<string, string> {
                { "Petra", "Manah A 1" },
            } },
            { "Nuakea", new Dictionary<string, string> {
                { "Maweke", "Nuakea A 1" },
                { "Korbu", "Nuakea A 1 a" },
            } },
            { "Ovid", new Dictionary<string, string> {
                { "Kenash", "Ovid A 5" },
                { "Paseguru", "Ovid A 5 a" },
            } },
            { "Ququve", new Dictionary<string, string> {
                { "New America", "Ququve A 1" },
                { "Gold", "Ququve A 2" },
            } },
            { "Rho Cancri", new Dictionary<string, string> {
                { "Rho Cancri I (55 Cnc e)", "Rho Cancri A 1" },
                { "Rho Cancri II (55 Cnc b)", "Rho Cancri A 2" },
                { "Rho Cancri III (55 Cnc c)", "Rho Cancri A 3" },
                { "Rho Cancri IV (55 Cnc f)", "Rho Cancri A 4" },
                { "Rho Cancri V (55 Cnc d)", "Rho Cancri A 5" },
                { "Rho Cancri VI", "Rho Cancri A 6" },
            } },
            { "Stopover", new Dictionary<string, string> {
                { "Stopover Minor", "Stopover A 1" },
                { "Stopover Major", "Stopover A 2" },
            } },
            { "Teshub", new Dictionary<string, string> {
                { "Kerrash", "Teshub A 1" },
            } },
            { "Witch's Reach", new Dictionary<string, string> {
                { "Cauldron", "Witch's Reach A 1" },
            } },
            { "Wolf 573", new Dictionary<string, string> {
                { "Lupon", "Wolf 573 A 1" },
            } },
            { "Wyrd", new Dictionary<string, string> {
                { "Way", "Wyrd A 3" },
                { "Lister", "Wyrd A 4" },
                { "Verse", "Wyrd A 4 a" },
            } },
            { "Zeessze", new Dictionary<string, string> {
                { "Gonzalez's Grave", "Zeessze A 1" },
                { "New America", "Zeessze A 2" },
                { "Macmillian's Hole", "Zeessze A 2 a" },
            } },
            { "BU 741", new Dictionary<string, string> {
                { "Darmen", "BU 741 A 1" },
            } },
            { "LDS 883", new Dictionary<string, string> {
                { "Argovia", "LDS 883 A 2" },
            } },
            { "Omicron-2 Eridani", new Dictionary<string, string> {
                { "Scott's Mine", "Omicron-2 Eridani A 1" },
                { "Steven's Rock", "Omicron-2 Eridani A 2" },
            } },
            { "Epsilon Indi", new Dictionary<string, string> {
                { "Edmond's Rock", "Epsilon Indi A 1" },
                { "Lee", "Epsilon Indi A 2" },
                { "New Africa", "Epsilon Indi A 3" },
                { "Mitterand Hollow", "Epsilon Indi A 3 a" },
                { "Alisonworld", "Epsilon Indi A 4" },
            } },
            { "Opala", new Dictionary<string, string> {
                { "Baker", "Opala A 4" },
                { "Trooper", "Opala A 4 a" },
            } },
            { "Shinrarta Dezhra", new Dictionary<string, string> {
                { "Founders World", "Shinrarta Dezhra A 4" },
            } },
            { "Tun", new Dictionary<string, string> {
                { "TunTon", "Tun A 1" },
            } },
            { "LHS 450", new Dictionary<string, string> {
                { "Darke's Claim", "LHS 450 B 1" },
                { "Camp Schmidt", "LHS 450 B 2" },
            } },
            { "Alpha Centauri", new Dictionary<string, string> {
                { "2045 PC2", "Alpha Centauri AB 1" },
                { "Lagrange", "Alpha Centauri AB 2" },
                { "2042 L1", "Alpha Centauri AB 2 a" },
                { "2071 AC3", "Alpha Centauri AB 3" },
                { "Eden", "Alpha Centauri C 1" },
            } },
            { "17 Draconis", new Dictionary<string, string> {
                { "Limbo", "17 Draconis A 1" },
                { "Eo", "17 Draconis A 2" },
                { "Shepherd", "17 Draconis A 2 a" },
                { "Paradiso", "17 Draconis A 2 b" },
                { "Icemir", "17 Draconis A 3" },
            } },
            { "i Bootis", new Dictionary<string, string> {
                { "Chango", "i Bootis A 1" },
                { "Maher Prime", "i Bootis D 1" },
                { "Dustball", "i Bootis D 1 a" },
            } },
            { "Prism", new Dictionary<string, string> {
                { "Mestra", "Prism A 1" },
                { "Daedalion", "Prism A 2" },
                { "Chione", "Prism A 2 a" },
                { "Neaera", "Prism A 3" },
                { "Eurcyleia", "Prism A 4" },
                { "Anticlea", "Prism A 4 a" },
                { "Amphithea", "Prism A 5" },
            } },
            { "51 Arietis", new Dictionary<string, string> {
                { "Uruk", "51 Arietis 4" },
                { "Adab", "51 Arietis 5" },
            } },
            { "82 Eridani", new Dictionary<string, string> {
                { "Rhatigan", "82 Eridani 1" },
                { "Ifreds Harbour", "82 Eridani 2" },
                { "Teekay 100705", "82 Eridani 3" },
            } },
            { "Achenar", new Dictionary<string, string> {
                { "Achenar 4a", "Achenar 4 a" },
                { "Yamaha's Grave", "Achenar 6 a" },
                { "New World", "Achenar 6 b" },
                { "Conversion", "Achenar 6 c" },
                { "Capitol", "Achenar 6 d" },
            } },
            { "Acihaut", new Dictionary<string, string> {
                { "Ayer", "Acihaut 1" },
                { "Jubach", "Acihaut 1 a" },
            } },
            { "Ackwada", new Dictionary<string, string> {
                { "Lopez's Mine", "Ackwada 1" },
                { "Biggs's Hole", "Ackwada 2" },
                { "Thompson's Planet", "Ackwada 3" },
            } },
            { "Aditi", new Dictionary<string, string> {
                { "Peylow", "Aditi 6" },
                { "Morrow Peek", "Aditi 6 a" },
            } },
            { "Aerial", new Dictionary<string, string> {
                { "Shangjun", "Aerial 1" },
                { "Tyree", "Aerial 1 a" },
            } },
            { "Aganippe", new Dictionary<string, string> {
                { "Boeotia", "Aganippe 1" },
                { "New Thebes", "Aganippe 1 a" },
            } },
            { "Agartha", new Dictionary<string, string> {
                { "Franklin", "Agartha 1" },
            } },
            { "Akhenaten", new Dictionary<string, string> {
                { "Jepochal-G Planet", "Akhenaten 1" },
            } },
            { "Akkadia", new Dictionary<string, string> {
                { "Remo Burkhard", "Akkadia 1" },
                { "Cathkin", "Akkadia 1 a" },
            } },
            { "Alexandrinus", new Dictionary<string, string> {
                { "Londinium", "Alexandrinus 5" },
            } },
            { "Alexis Centauri", new Dictionary<string, string> {
                { "Sophia", "Alexis Centauri 1" },
                { "Pumori", "Alexis Centauri 1 a" },
            } },
            { "Algreit", new Dictionary<string, string> {
                { "Panmore", "Algreit 1" },
            } },
            { "Alioth", new Dictionary<string, string> {
                { "Wicca's World", "Alioth 5 a" },
                { "Turner's World", "Alioth 5 b" },
                { "Argent's Claim", "Alioth 5 c" },
                { "Bifrost", "Alioth 6" },
                { "Mackenzie's Legacy", "Alioth 6 a" },
                { "Ousey Rock", "Alioth 6 b" },
                { "Miller Rock", "Alioth 6 c" },
            } },
            { "Alkaid", new Dictionary<string, string> {
                { "Ashfield's Wreck", "Alkaid 14 a" },
                { "Strauss Rock", "Alkaid 15 a" },
                { "Green Mount", "Alkaid 15 b" },
                { "New Caledonia", "Alkaid 16 a" },
                { "Greenhill", "Alkaid 16 b" },
                { "Honda", "Alkaid 16 c" },
                { "Diamond's Rock", "Alkaid 16 d" },
            } },
            { "Altair", new Dictionary<string, string> {
                { "Darkes Hollow", "Altair 3" },
                { "Biggs Colony", "Altair 4" },
                { "Lowing's Rock", "Altair 5" },
            } },
            { "Amy-Charlotte", new Dictionary<string, string> {
                { "Antissa", "Amy-Charlotte 1" },
            } },
            { "Anahit", new Dictionary<string, string> {
                { "Irkalla", "Anahit 4" },
                { "Nergal", "Anahit 4 a" },
            } },
            { "Anayol", new Dictionary<string, string> {
                { "Solo's Grave", "Anayol 1" },
                { "Gold", "Anayol 2" },
                { "Bush Reward", "Anayol 3" },
            } },
            { "Andancan", new Dictionary<string, string> {
                { "New Bactra", "Andancan 1" },
            } },
            { "Andceeth", new Dictionary<string, string> {
                { "Capitol", "Andceeth 4" },
                { "Mitre", "Andceeth 4 a" },
            } },
            { "Anlave", new Dictionary<string, string> {
                { "Jennings's Legacy", "Anlave 3" },
                { "Anderton's Mine", "Anlave 5" },
                { "Jordan's Rock", "Anlave 6" },
                { "Anderton", "Anlave 7" },
                { "Denver", "Anlave 8" },
            } },
            { "Annwn", new Dictionary<string, string> {
                { "Arawn", "Annwn 1" },
            } },
            { "Apam Napat", new Dictionary<string, string> {
                { "Grahame", "Apam Napat 5" },
            } },
            { "Aparctias", new Dictionary<string, string> {
                { "Cardinal", "Aparctias 1" },
                { "Septentrio", "Aparctias 2" },
            } },
            { "Aramzahd", new Dictionary<string, string> {
                { "Guardinia", "Aramzahd 1" },
                { "Smade's Planet", "Aramzahd 1 a" },
            } },
            { "Arcturus", new Dictionary<string, string> {
                { "Major", "Arcturus 2" },
                { "Richard's Rock", "Arcturus 3" },
                { "Discovery", "Arcturus 4" },
                { "Arcas", "Arcturus 5" },
                { "Masseyworld", "Arcturus 5 a" },
                { "Richardson's Mine", "Arcturus 6 a" },
                { "Jeffries", "Arcturus 6 b" },
                { "Oliver's Mine", "Arcturus 6 c" },
            } },
            { "Arexack", new Dictionary<string, string> {
                { "Tracy's World", "Arexack 1" },
                { "Peter's Eden", "Arexack 2" },
                { "Maxwell Hollow", "Arexack 3" },
            } },
            { "Arinack", new Dictionary<string, string> {
                { "Lagdo", "Arinack 1" },
            } },
            { "Arouca", new Dictionary<string, string> {
                { "Amutria", "Arouca 5" },
            } },
            { "Arque", new Dictionary<string, string> {
                { "New Caledonia", "Arque 1" },
                { "Gold", "Arque 1 a" },
            } },
            { "Artemis", new Dictionary<string, string> {
                { "Laphria", "Artemis 2" },
                { "Agrotera", "Artemis 5" },
                { "Agrotera a", "Artemis 5 a" },
                { "Agrotera b", "Artemis 5 b" },
                { "Agrotera c", "Artemis 5 c" },
            } },
            { "Artume", new Dictionary<string, string> {
                { "Lusardi", "Artume 4" },
                { "Artimi", "Artume 6" },
            } },
            { "Asphodel", new Dictionary<string, string> {
                { "Irving World", "Asphodel 1" },
                { "Irving's Moon", "Asphodel 1 a" },
            } },
            { "Aulin", new Dictionary<string, string> {
                { "Nirvana", "Aulin 2" },
            } },
            { "Aulis", new Dictionary<string, string> {
                { "Chubei", "Aulis 3" },
                { "Khan", "Aulis 3 a" },
            } },
            { "Avalon", new Dictionary<string, string> {
                { "Kipper's Retreat", "Avalon 1" },
                { "Grach", "Avalon 2" },
            } },
            { "Ayethi", new Dictionary<string, string> {
                { "Topaz", "Ayethi 1" },
                { "Raven's Landing", "Ayethi 2" },
            } },
            { "Aymiay", new Dictionary<string, string> {
                { "Camp Diamond", "Aymiay 1" },
                { "Cooperworld", "Aymiay 2" },
                { "Haynes's Wreck", "Aymiay 2 a" },
                { "Baker", "Aymiay 3" },
            } },
            { "Aymifa", new Dictionary<string, string> {
                { "Camp Rance", "Aymifa 1" },
                { "Gold", "Aymifa 2" },
                { "Valhalla", "Aymifa 3" },
                { "Lee Hollow", "Aymifa 4" },
                { "Popov Reward", "Aymifa 5" },
            } },
            { "Azrael", new Dictionary<string, string> {
                { "Rafferty's Paradise", "Azrael 2" },
                { "Noshaq", "Azrael 2 a" },
            } },
            { "Aztlan", new Dictionary<string, string> {
                { "Nexus737", "Aztlan 1" },
            } },
            { "Baker", new Dictionary<string, string> {
                { "Paru", "Baker 1" },
            } },
            { "Baldr", new Dictionary<string, string> {
                { "L.Meristo", "Baldr 2" },
            } },
            { "Baltah'Sine", new Dictionary<string, string> {
                { "Baltha'Sine", "Baltah'Sine 1" },
            } },
            { "Barnard's Star", new Dictionary<string, string> {
                { "Cooke", "Barnard's Star 3" },
                { "Birmingham World", "Barnard's Star 4" },
            } },
            { "Bast", new Dictionary<string, string> {
                { "THFC-Est1882", "Bast 1" },
            } },
            { "BD+22 4939", new Dictionary<string, string> {
                { "Badfort", "BD+22 4939 1" },
            } },
            { "BD+24 543", new Dictionary<string, string> {
                { "Pharos", "BD+24 543 1" },
            } },
            { "BD+26 2184", new Dictionary<string, string> {
                { "Kathy McBrayer", "BD+26 2184 5" },
            } },
            { "BD+31 2290", new Dictionary<string, string> {
                { "Jodi's Rest", "BD+31 2290 1" },
            } },
            { "BD+75 58", new Dictionary<string, string> {
                { "KevlinSyk - 82", "BD+75 58 2" },
            } },
            { "Bedaho", new Dictionary<string, string> {
                { "Strauss Reward", "Bedaho 1" },
                { "Matto", "Bedaho 2" },
            } },
            { "Beldarkri", new Dictionary<string, string> {
                { "Kowden", "Beldarkri 1" },
                { "Koppen", "Beldarkri 1 a" },
            } },
            { "Bento", new Dictionary<string, string> {
                { "GJ 86 b", "Bento 1" },
                { "Makunouchi", "Bento 2" },
                { "Kyaraben", "Bento 2 a" },
            } },
            { "Beta Catonis", new Dictionary<string, string> {
                { "Dalager", "Beta Catonis 5" },
            } },
            { "Beta Hydri", new Dictionary<string, string> {
                { "Camp Schmidt", "Beta Hydri 3" },
                { "Camp Shepard", "Beta Hydri 4" },
                { "Jordan's Legacy", "Beta Hydri 5" },
                { "Homeland", "Beta Hydri 6" },
                { "Endl", "Beta Hydri 8" },
            } },
            { "Betel", new Dictionary<string, string> {
                { "Kava", "Betel 3" },
            } },
            { "Bevan's Hope", new Dictionary<string, string> {
                { "Ithica", "Bevan's Hope 1" },
            } },
            { "Bohmshohm", new Dictionary<string, string> {
                { "Epirus", "Bohmshohm 1" },
                { "Kaliash", "Bohmshohm 1 a" },
            } },
            { "Bolg", new Dictionary<string, string> {
                { "Subartu", "Bolg 1" },
                { "Shubar", "Bolg 2" },
            } },
            { "Breamen", new Dictionary<string, string> {
                { "Chia", "Breamen 1" },
                { "Andromeda9", "Breamen 2" },
            } },
            { "Brohman", new Dictionary<string, string> {
                { "Villist", "Brohman 4" },
                { "Eden 13913", "Brohman 4 a" },
            } },
            { "B'Titus", new Dictionary<string, string> {
                { "Urus", "B'Titus 2" },
            } },
            { "Caelinus", new Dictionary<string, string> {
                { "Caelinus I", "Caelinus 1" },
                { "Caelinus II", "Caelinus 2" },
                { "Caelinus III", "Caelinus 3" },
            } },
            { "Caelottixa", new Dictionary<string, string> {
                { "Caelottixa", "Caelottixa 1" },
            } },
            { "Cai", new Dictionary<string, string> {
                { "Xyile", "Cai 1" },
                { "Trango", "Cai 2 a" },
            } },
            { "Calhuacan", new Dictionary<string, string> {
                { "Stapled Peacock Flesh", "Calhuacan 1" },
                { "Erebus", "Calhuacan 1 a" },
            } },
            { "Canopus", new Dictionary<string, string> {
                { "Maxwell's World", "Canopus 1" },
                { "Rance's Wreck", "Canopus 2" },
            } },
            { "Carcinus", new Dictionary<string, string> {
                { "New Babylon", "Carcinus 4" },
            } },
            { "Cardea", new Dictionary<string, string> {
                { "Raymo's Rendezvous", "Cardea 1" },
                { "Xaubab", "Cardea 1 a" },
            } },
            { "Carnoeck", new Dictionary<string, string> {
                { "Macdara Peter Gamboni", "Carnoeck 2" },
            } },
            { "Carthage", new Dictionary<string, string> {
                { "New Carthage", "Carthage 1" },
                { "Cho", "Carthage 1 a" },
                { "Belus", "Carthage 2" },
                { "Archerbas", "Carthage 3" },
                { "Archerbas r1", "Carthage 3 A Ring" },
            } },
            { "CD-54 471", new Dictionary<string, string> {
                { "Brigsteer", "CD-54 471 1" },
            } },
            { "CD-58 16", new Dictionary<string, string> {
                { "Suontaka", "CD-58 16 1" },
                { "S-One", "CD-58 16 1 a" },
            } },
            { "CD-64 139", new Dictionary<string, string> {
                { "Beorg", "CD-64 139 1" },
            } },
            { "CD-65 76", new Dictionary<string, string> {
                { "Jessica's Folly", "CD-65 76 1" },
                { "Sheol", "CD-65 76 1 a" },
            } },
            { "CD-69 5", new Dictionary<string, string> {
                { "Clements Keep", "CD-69 5 5" },
                { "Ardrain", "CD-69 5 6" },
            } },
            { "CD-70 1960", new Dictionary<string, string> {
                { "Chonzie", "CD-70 1960 1" },
                { "Brodie's Legacy", "CD-70 1960 2" },
            } },
            { "CD-73 12", new Dictionary<string, string> {
                { "Delicious Cinnamon", "CD-73 12 5" },
                { "Scafell", "CD-73 12 6" },
            } },
            { "CD-77 45", new Dictionary<string, string> {
                { "Irrelon Prime", "CD-77 45 3" },
            } },
            { "CD-79 950", new Dictionary<string, string> {
                { "Hellvellyn", "CD-79 950 2" },
                { "Patterdale", "CD-79 950 2 a" },
            } },
            { "Cegreeth", new Dictionary<string, string> {
                { "Morris's Planet", "Cegreeth 1" },
                { "Morris's Claim", "Cegreeth 1 a" },
            } },
            { "Cemiess", new Dictionary<string, string> {
                { "Jade", "Cemiess 4" },
                { "Emerald", "Cemiess 4 a" },
            } },
            { "Cet", new Dictionary<string, string> {
                { "Marie-José Willems", "Cet 1" },
                { "Howard Martyn Wensley", "Cet 2" },
            } },
            { "Chi Herculis", new Dictionary<string, string> {
                { "Apasam", "Chi Herculis 1" },
                { "Kumay", "Chi Herculis 1 a" },
            } },
            { "Chup Kamui", new Dictionary<string, string> {
                { "Blackstar", "Chup Kamui 1" },
                { "Manily", "Chup Kamui 2" },
            } },
            { "Cibola", new Dictionary<string, string> {
                { "Keytree", "Cibola 1" },
                { "Quivira", "Cibola 1 a" },
            } },
            { "Cocijo", new Dictionary<string, string> {
                { "fragLANd", "Cocijo 1" },
                { "Moel Hebog", "Cocijo 1 a" },
            } },
            { "Cockaigne", new Dictionary<string, string> {
                { "Goliard", "Cockaigne 1" },
                { "Mytoses", "Cockaigne 2" },
            } },
            { "Codorain", new Dictionary<string, string> {
                { "Codorain I", "Codorain 1" },
                { "Codorain II", "Codorain 2" },
                { "Codorain III", "Codorain 6" },
                { "Farr's Landing", "Codorain 6 a" },
            } },
            { "Crevit", new Dictionary<string, string> {
                { "Dalraida", "Crevit 1" },
            } },
            { "CT Tucanae", new Dictionary<string, string> {
                { "Abona", "CT Tucanae 3" },
            } },
            { "Dahan", new Dictionary<string, string> {
                { "Hathor", "Dahan 2" },
                { "Hathor r1", "Dahan 2 A Ring" },
                { "Coltan", "Dahan 2 a" },
                { "Zhang's Claim", "Dahan 2 b" },
                { "Smithy's Claim", "Dahan 2 c" },
                { "Jonty's Claim", "Dahan 2 d" },
                { "Wanderer", "Dahan 2 e" },
                { "Dahan 3a", "Dahan 3 a" },
                { "Dahan 3b", "Dahan 3 b" },
                { "Dahan 3c", "Dahan 3 c" },
            } },
            { "Dakvar", new Dictionary<string, string> {
                { "Ebbe", "Dakvar 1" },
                { "Naglo", "Dakvar 3" },
            } },
            { "Dalfur", new Dictionary<string, string> {
                { "Aquila", "Dalfur 7" },
            } },
            { "Darahk", new Dictionary<string, string> {
                { "Darahk II", "Darahk 1" },
                { "Darahk III", "Darahk 2" },
                { "Darahk IV", "Darahk 3" },
                { "Darahk V", "Darahk 4" },
                { "Darahk VI", "Darahk 5" },
                { "Darahk VII", "Darahk 6" },
            } },
            { "Decima", new Dictionary<string, string> {
                { "Fate's Gift", "Decima 1" },
                { "Nanda", "Decima 1 a" },
            } },
            { "Delkar", new Dictionary<string, string> {
                { "Savo", "Delkar 4" },
                { "Anamundi", "Delkar 4 a" },
            } },
            { "Delta Pavonis", new Dictionary<string, string> {
                { "Camp Mitterand", "Delta Pavonis 1" },
                { "Suzuki Reward", "Delta Pavonis 2" },
                { "Reagan's Legacy", "Delta Pavonis 3" },
                { "Gold", "Delta Pavonis 4 a" },
            } },
            { "Delta Phoenicis", new Dictionary<string, string> {
                { "DP I", "Delta Phoenicis 1" },
                { "Anka", "Delta Phoenicis 2" },
                { "Kerkes", "Delta Phoenicis 3" },
                { "Garuda", "Delta Phoenicis 3 a" },
            } },
            { "Difu", new Dictionary<string, string> {
                { "Jontyworld", "Difu 1" },
                { "Etemmu", "Difu 2" },
            } },
            { "Dijkstra", new Dictionary<string, string> {
                { "Frisia", "Dijkstra 4" },
                { "Chembra", "Dijkstra 4 a" },
            } },
            { "Dinda", new Dictionary<string, string> {
                { "Ravenclan's Settlement", "Dinda 1" },
                { "Hood", "Dinda 1 a" },
            } },
            { "Diso", new Dictionary<string, string> {
                { "Birmingham", "Diso 3" },
            } },
            { "Ekhi", new Dictionary<string, string> {
                { "Proudhon's Property", "Ekhi 1" },
                { "Kongur", "Ekhi 1 a" },
            } },
            { "Elysia", new Dictionary<string, string> {
                { "Cronus", "Elysia 1" },
                { "Kailas", "Elysia 1 a" },
            } },
            { "Epsilon Eridani", new Dictionary<string, string> {
                { "Goldstein's Rock", "Epsilon Eridani 1" },
                { "New California", "Epsilon Eridani 2" },
                { "Major's Mine", "Epsilon Eridani 3" },
            } },
            { "Epsilon Hydri", new Dictionary<string, string> {
                { "Offield-Duan", "Epsilon Hydri 5" },
                { "Shan", "Epsilon Hydri 5 a" },
            } },
            { "Epsilon Phoenicis", new Dictionary<string, string> {
                { "Binnein", "Epsilon Phoenicis 5" },
                { "Bormann's Blessing", "Epsilon Phoenicis 6" },
            } },
            { "Eranin", new Dictionary<string, string> {
                { "Azeban", "Eranin 1" },
            } },
            { "Eulexia", new Dictionary<string, string> {
                { "Eurydice", "Eulexia 3" },
                { "Tiamoia", "Eulexia 5" },
            } },
            { "Euryale", new Dictionary<string, string> {
                { "Wen", "Euryale 3" },
                { "EG Prime", "Euryale 4" },
            } },
            { "Eurybia", new Dictionary<string, string> {
                { "Tira Flirble", "Eurybia 1" },
                { "Makalu", "Eurybia 1 a" },
            } },
            { "Exbeur", new Dictionary<string, string> {
                { "Campbellworld", "Exbeur 4" },
                { "Sheehanworld", "Exbeur 5" },
            } },
            { "Exigus", new Dictionary<string, string> {
                { "Walkerworld", "Exigus 1" },
            } },
            { "Exioce", new Dictionary<string, string> {
                { "O'Rourke Colony", "Exioce 1" },
                { "Experiment", "Exioce 1 a" },
                { "Democracy", "Exioce 2" },
                { "Boston's Wreck", "Exioce 2 a" },
            } },
            { "Exphiay", new Dictionary<string, string> {
                { "Discovery", "Exphiay 6" },
                { "Rakhiot", "Exphiay 6 a" },
            } },
            { "Facece", new Dictionary<string, string> {
                { "Mathews's Hole", "Facece 1" },
                { "Coates's Mine", "Facece 2" },
                { "Peters's Wreck", "Facece 3" },
                { "New America", "Facece 4" },
                { "Topaz", "Facece 5" },
            } },
            { "Farack", new Dictionary<string, string> {
                { "KT-LINE", "Farack 1" },
                { "Hemshut", "Farack 2" },
            } },
            { "Fawaol", new Dictionary<string, string> {
                { "Tracy", "Fawaol 1 a" },
                { "Camp Rush", "Fawaol 1 b" },
                { "Distat", "Fawaol 2" },
                { "Griffiths Rock", "Fawaol 3" },
            } },
            { "Fehu", new Dictionary<string, string> {
                { "Likopo", "Fehu 3" },
                { "Doolhof", "Fehu 3 a" },
            } },
            { "Felkan", new Dictionary<string, string> {
                { "Jesstopia", "Felkan 1" },
            } },
            { "Feuma", new Dictionary<string, string> {
                { "New Albion", "Feuma 1" },
            } },
            { "Flesk", new Dictionary<string, string> {
                { "Gaia Mai", "Flesk 1" },
                { "Manod", "Flesk 1 a" },
            } },
            { "Frog", new Dictionary<string, string> {
                { "Thea", "Frog 1" },
                { "Merapi", "Frog 1 a" },
            } },
            { "Fujin", new Dictionary<string, string> {
                { "Futen", "Fujin 1" },
            } },
            { "Futhark", new Dictionary<string, string> {
                { "Chuillin", "Futhark 1" },
                { "Faereal Prime", "Futhark 2" },
            } },
            { "Futhorc", new Dictionary<string, string> {
                { "Graysonia", "Futhorc 2" },
                { "Boeth", "Futhorc 2 a" },
            } },
            { "G 239-25", new Dictionary<string, string> {
                { "Buyukkale", "G 239-25 1" },
            } },
            { "Gateway", new Dictionary<string, string> {
                { "Hope", "Gateway 1" },
                { "Saunder's Rock", "Gateway 1 a" },
                { "de Gaulworld", "Gateway 2 a" },
                { "Wernerworld", "Gateway 2 b" },
                { "Machester's Claim", "Gateway 3 a" },
                { "Graham's Claim", "Gateway 4 a" },
                { "Gupta's Rock", "Gateway 4 b" },
            } },
            { "Gippsworld", new Dictionary<string, string> {
                { "Gippsworld", "Gippsworld 1" },
            } },
            { "Gurney Slade", new Dictionary<string, string> {
                { "Gurney Slade One", "Gurney Slade 1" },
                { "Gurney Slade Two", "Gurney Slade 2" },
                { "Gurney Slade Three", "Gurney Slade 3" },
                { "Birmingham", "Gurney Slade 4" },
                { "Gurney Slade Five", "Gurney Slade 5" },
                { "Gurney Slade Six", "Gurney Slade 6" },
                { "Gurney Slade Seven", "Gurney Slade 7" },
            } },
            { "Gyton's Hope", new Dictionary<string, string> {
                { "Gyton's Landing", "Gyton's Hope 1" },
                { "Pisani", "Gyton's Hope 2" },
            } },
            { "h Draconis", new Dictionary<string, string> {
                { "Gaalai", "h Draconis 1" },
                { "Makalu", "h Draconis 1 a" },
            } },
            { "Hazel", new Dictionary<string, string> {
                { "Grove", "Hazel 3" },
            } },
            { "Hecate", new Dictionary<string, string> {
                { "Harrison XIII", "Hecate 2" },
                { "Tomie", "Hecate 3" },
            } },
            { "Heget", new Dictionary<string, string> {
                { "Ma Mordella", "Heget 1" },
                { "Horta", "Heget 1 a" },
            } },
            { "HERZ 10688", new Dictionary<string, string> {
                { "Melfort", "HERZ 10688 5" },
            } },
            { "HIP 15310", new Dictionary<string, string> {
                { "X-2487", "HIP 15310 1" },
                { "X-2488", "HIP 15310 1 a" },
            } },
            { "Hodack", new Dictionary<string, string> {
                { "Whiterock", "Hodack 1" },
            } },
            { "Hofada", new Dictionary<string, string> {
                { "Ausis", "Hofada 2" },
            } },
            { "Holiacan", new Dictionary<string, string> {
                { "Reaganworld", "Holiacan 1" },
                { "Ouseyworld", "Holiacan 2" },
                { "Lopez's Hole", "Holiacan 3" },
            } },
            { "Hors", new Dictionary<string, string> {
                { "Colon", "Hors 6" },
            } },
            { "Howard", new Dictionary<string, string> {
                { "Rogatino", "Howard 2" },
            } },
            { "Hyperborea", new Dictionary<string, string> {
                { "Boreas", "Hyperborea 4" },
            } },
            { "Indaol", new Dictionary<string, string> {
                { "Graham's Rock", "Indaol 1" },
                { "Morgan's Hole", "Indaol 2" },
            } },
            { "Isinor", new Dictionary<string, string> {
                { "Maodun", "Isinor 1" },
            } },
            { "Isis", new Dictionary<string, string> {
                { "Isis I", "Isis 3" },
                { "Isis II", "Isis 4" },
                { "Isis III", "Isis 5" },
                { "Isis IV", "Isis 6" },
                { "Isis V", "Isis 7" },
                { "Isis VI", "Isis 8" },
                { "Isis VII", "Isis 9" },
                { "Isis VIII", "Isis 10" },
                { "Isis IX", "Isis 11" },
                { "Isis X", "Isis 12" },
            } },
            { "Ithaca", new Dictionary<string, string> {
                { "Huari", "Ithaca 1" },
            } },
            { "Jodrell", new Dictionary<string, string> {
                { "Jodrell I", "Jodrell 1" },
                { "Jodrell II", "Jodrell 2" },
                { "Jodrell III", "Jodrell 3" },
            } },
            { "Jotun", new Dictionary<string, string> {
                { "Faner", "Jotun 1" },
            } },
            { "Jotunheim", new Dictionary<string, string> {
                { "Daisy", "Jotunheim 3" },
            } },
            { "Kappa Fornacis", new Dictionary<string, string> {
                { "Panem", "Kappa Fornacis 3" },
                { "Panes", "Kappa Fornacis 3 a" },
            } },
            { "Kaun", new Dictionary<string, string> {
                { "Kaunen", "Kaun 1" },
            } },
            { "Keries", new Dictionary<string, string> {
                { "Cisse", "Keries 1" },
            } },
            { "La Rochelle", new Dictionary<string, string> {
                { "Seymour Hollow (GJ 832 c)", "La Rochelle 1" },
            } },
            { "Lacaille 9352", new Dictionary<string, string> {
                { "Jennings's Rock", "Lacaille 9352 1" },
                { "Camp Lawrence", "Lacaille 9352 2" },
            } },
            { "Laedla", new Dictionary<string, string> {
                { "Swallowworld", "Laedla 1" },
                { "Ford's Legacy", "Laedla 2" },
                { "Mansfield Colony", "Laedla 3" },
            } },
            { "Laka", new Dictionary<string, string> {
                { "Kai-kapu", "Laka 4" },
                { "Puncak", "Laka 4 a" },
            } },
            { "Lakota", new Dictionary<string, string> {
                { "Teton", "Lakota 1" },
                { "Pada", "Lakota 1 a" },
            } },
            { "Lalande 4141", new Dictionary<string, string> {
                { "Nahtanoj", "Lalande 4141 1" },
            } },
            { "Lalande 6320", new Dictionary<string, string> {
                { "Slieau Whallian", "Lalande 6320 1" },
                { "Elkins", "Lalande 6320 1 a" },
            } },
            { "Lambda Horologii", new Dictionary<string, string> {
                { "Thorgill", "Lambda Horologii 1" },
            } },
            { "Lansbury", new Dictionary<string, string> {
                { "Lansbury - I", "Lansbury 1" },
                { "Lansbury - II", "Lansbury 2" },
                { "Lansbury - III", "Lansbury 3" },
                { "Lansbury - IV", "Lansbury 4" },
                { "Lansbury - V", "Lansbury 5" },
                { "Lansbury - VI", "Lansbury 6" },
            } },
            { "Lausang", new Dictionary<string, string> {
                { "Rheged", "Lausang 8" },
                { "Chappal", "Lausang 8 a" },
            } },
            { "Lave", new Dictionary<string, string> {
                { "Planet Lave", "Lave 1" },
            } },
            { "Leesti", new Dictionary<string, string> {
                { "Leesti", "Leesti 3" },
            } },
            { "LFT 880", new Dictionary<string, string> {
                { "Chayhuac", "LFT 880 1" },
            } },
            { "LHS 1071", new Dictionary<string, string> {
                { "Dr. Mitanek's Eternity", "LHS 1071 1" },
                { "Ugain", "LHS 1071 2" },
            } },
            { "LHS 1167", new Dictionary<string, string> {
                { "Fortuna Fangrim", "LHS 1167 1" },
            } },
            { "LHS 1387", new Dictionary<string, string> {
                { "New Turkey", "LHS 1387 1" },
                { "Ararat", "LHS 1387 1 a" },
            } },
            { "LHS 1573", new Dictionary<string, string> {
                { "Dustbowl", "LHS 1573 1" },
            } },
            { "LHS 1650", new Dictionary<string, string> {
                { "Trileukon", "LHS 1650 1" },
                { "Laila", "LHS 1650 1 a" },
            } },
            { "LHS 2819", new Dictionary<string, string> {
                { "Suyus", "LHS 2819 3" },
            } },
            { "LHS 2884", new Dictionary<string, string> {
                { "Conti", "LHS 2884 1" },
            } },
            { "LHS 2887", new Dictionary<string, string> {
                { "Tambo", "LHS 2887 3" },
            } },
            { "LHS 3006", new Dictionary<string, string> {
                { "Vulcan", "LHS 3006 1" },
                { "Vulcan Ring", "LHS 3006 1 A Ring" },
            } },
            { "LHS 3262", new Dictionary<string, string> {
                { "Ashanti", "LHS 3262 1" },
                { "Akan", "LHS 3262 1 a" },
            } },
            { "LHS 3439", new Dictionary<string, string> {
                { "Mervon", "LHS 3439 2" },
            } },
            { "LHS 3531", new Dictionary<string, string> {
                { "LHS 3531-I", "LHS 3531 2" },
            } },
            { "LHS 3921", new Dictionary<string, string> {
                { "Tilune", "LHS 3921 1" },
            } },
            { "LHS 397", new Dictionary<string, string> {
                { "Cooper Rock", "LHS 397 1" },
                { "Gold", "LHS 397 2" },
            } },
            { "LHS 412", new Dictionary<string, string> {
                { "LHS 412-I", "LHS 412 1" },
                { "LHS 412-II", "LHS 412 2" },
                { "LHS 412-III", "LHS 412 3" },
                { "LHS 412-IV", "LHS 412 4" },
                { "LHS 412-V", "LHS 412 5" },
                { "LHS 412-VI", "LHS 412 6" },
                { "LHS 412-VII", "LHS 412 7" },
            } },
            { "LHS 417", new Dictionary<string, string> {
                { "Nannan", "LHS 417 3" },
            } },
            { "LHS 448", new Dictionary<string, string> {
                { "Vivally", "LHS 448 1" },
            } },
            { "LHS 449", new Dictionary<string, string> {
                { "Charles's Mine", "LHS 449 1" },
                { "Haynes's Mine", "LHS 449 2" },
                { "Gold", "LHS 449 3" },
                { "Simpson's Mine", "LHS 449 4" },
            } },
            { "Liabeze", new Dictionary<string, string> {
                { "Mitterand's World", "Liabeze 1" },
                { "New California", "Liabeze 2" },
            } },
            { "LP 64-194", new Dictionary<string, string> {
                { "Gargarii", "LP 64-194 1" },
            } },
            { "LP 644-15", new Dictionary<string, string> {
                { "Alasdair's World", "LP 644-15 3" },
            } },
            { "LP 98-132", new Dictionary<string, string> {
                { "Anahit", "LP 98-132 1" },
                { "Anahit Ring", "LP 98-132 1 A Ring" },
            } },
            { "LTT 1345", new Dictionary<string, string> {
                { "Pod", "LTT 1345 1" },
            } },
            { "LTT 135", new Dictionary<string, string> {
                { "Crocket", "LTT 135 8" },
                { "Tulaichean", "LTT 135 8 a" },
            } },
            { "LTT 15493", new Dictionary<string, string> {
                { "Sun Dancer", "LTT 15493 1" },
                { "LTT 15493 - I", "LTT 15493 2" },
                { "Jarek's Folly", "LTT 15493 3" },
                { "LTT 15493 - III", "LTT 15493 4" },
                { "LTT 15493 - IV", "LTT 15493 5" },
                { "LTT 15493 - V", "LTT 15493 6" },
            } },
            { "LTT 198", new Dictionary<string, string> {
                { "Dini", "LTT 198 3" },
                { "Skiddaw", "LTT 198 3 a" },
            } },
            { "LTT 2396", new Dictionary<string, string> {
                { "New Utah", "LTT 2396 1" },
            } },
            { "LTT 2771", new Dictionary<string, string> {
                { "Tarlak", "LTT 2771 1" },
            } },
            { "LTT 2952", new Dictionary<string, string> {
                { "Burbidge", "LTT 2952 1 a" },
            } },
            { "LTT 464", new Dictionary<string, string> {
                { "El-Mariesh", "LTT 464 1" },
            } },
            { "LTT 606", new Dictionary<string, string> {
                { "Fraser", "LTT 606 1" },
                { "Moel Lefn", "LTT 606 6" },
            } },
            { "LTT 7669", new Dictionary<string, string> {
                { "Grüne Hölle", "LTT 7669 1" },
            } },
            { "LTT 911", new Dictionary<string, string> {
                { "Jokester's Planet", "LTT 911 1" },
            } },
            { "LTT 9846", new Dictionary<string, string> {
                { "Magna", "LTT 9846 1" },
            } },
            { "Lugh", new Dictionary<string, string> {
                { "Tir na Lugh", "Lugh 6" },
            } },
            { "Luyten 205-128", new Dictionary<string, string> {
                { "Kawasakiworld", "Luyten 205-128 1" },
                { "Schmidt's Mine", "Luyten 205-128 2" },
            } },
            { "Luyten 347-14", new Dictionary<string, string> {
                { "Rance's Wreck", "Luyten 347-14 1" },
            } },
            { "Luyten 674-15", new Dictionary<string, string> {
                { "Bell's World", "Luyten 674-15 1" },
                { "Jordan's Hole", "Luyten 674-15 2" },
            } },
            { "Maausk", new Dictionary<string, string> {
                { "Owain", "Maausk 1" },
                { "Kazbek", "Maausk 1 a" },
            } },
            { "Magec", new Dictionary<string, string> {
                { "Cishan", "Magec 3" },
                { "Ankogel", "Magec 3 a" },
                { "Guayota", "Magec 6" },
                { "Guayota a", "Magec 6 a" },
                { "Guayota b", "Magec 6 b" },
                { "Teide", "Magec 7" },
                { "Teide a", "Magec 7 a" },
                { "Teide b", "Magec 7 b" },
                { "Luo", "Magec 7 c" },
                { "Teide d", "Magec 7 d" },
                { "Achamán", "Magec 8" },
                { "Achamán a", "Magec 8 a" },
                { "Guanche", "Magec 9" },
            } },
            { "Marduk", new Dictionary<string, string> {
                { "Sippar", "Marduk 1" },
                { "Amar", "Marduk 1 a" },
            } },
            { "Meliae", new Dictionary<string, string> {
                { "New Los Angeles", "Meliae 1" },
            } },
            { "Meropis", new Dictionary<string, string> {
                { "Anna Ceri", "Meropis 4" },
                { "Chlos", "Meropis 5" },
            } },
            { "Michael Pantazis", new Dictionary<string, string> {
                { "Elizabeth Pantazis", "Michael Pantazis 1" },
            } },
            { "Minerva", new Dictionary<string, string> {
                { "Eris", "Minerva 1" },
                { "Peiste", "Minerva 2" },
            } },
            { "Miphifa", new Dictionary<string, string> {
                { "Varangia", "Miphifa 1" },
            } },
            { "Miquich", new Dictionary<string, string> {
                { "New California", "Miquich 1" },
                { "Pulag", "Miquich 1 a" },
            } },
            { "Mjolnir", new Dictionary<string, string> {
                { "Sindri", "Mjolnir 1" },
                { "Brokkr", "Mjolnir 1 a" },
            } },
            { "Moirai", new Dictionary<string, string> {
                { "Thea Centauri", "Moirai 1" },
                { "Kangri", "Moirai 1 a" },
            } },
            { "Momus Reach", new Dictionary<string, string> {
                { "Nemesis", "Momus Reach 1" },
                { "Pandora", "Momus Reach 1 a" },
            } },
            { "Morana", new Dictionary<string, string> {
                { "Tellus Tertius", "Morana 6" },
                { "Nansen", "Morana 6 a" },
            } },
            { "Morgor", new Dictionary<string, string> {
                { "Cruachan", "Morgor 2" },
            } },
            { "Munfayl", new Dictionary<string, string> {
                { "More", "Munfayl 1" },
                { "Bondarek", "Munfayl 2" },
            } },
            { "Munshin", new Dictionary<string, string> {
                { "Ocrinox's Opulence", "Munshin 5" },
                { "Damavand", "Munshin 5 a" },
            } },
            { "Myrbat", new Dictionary<string, string> {
                { "Babycures", "Myrbat 1" },
                { "Munros", "Myrbat 2" },
            } },
            { "Nang Ta-khian", new Dictionary<string, string> {
                { "Kopet Dag", "Nang Ta-khian 1" },
                { "Nisa", "Nang Ta-khian 1 a" },
            } },
            { "Nastrond", new Dictionary<string, string> {
                { "Nidhogg", "Nastrond 3" },
            } },
            { "Nat9481", new Dictionary<string, string> {
                { "Evelyn's Haven 1905", "Nat9481 5" },
            } },
            { "NLTT 9949", new Dictionary<string, string> {
                { "Aunios", "NLTT 9949 1" },
                { "Moco", "NLTT 9949 1 a" },
            } },
            { "Nortes", new Dictionary<string, string> {
                { "Ash's Inferis", "Nortes 1" },
                { "Fach", "Nortes 1 a" },
            } },
            { "Orerve", new Dictionary<string, string> {
                { "Simpson's Eden", "Orerve 1" },
            } },
            { "Orrere", new Dictionary<string, string> {
                { "Grey", "Orrere 1" },
            } },
            { "Paul-Friedrichs Star", new Dictionary<string, string> {
                { "Jakobs Hallowed Paradox", "Paul-Friedrichs Star 1" },
            } },
            { "Peregrina", new Dictionary<string, string> {
                { "Eurynomus", "Peregrina 2" },
                { "Undine", "Peregrina 2 a" },
            } },
            { "Perkele", new Dictionary<string, string> {
                { "Volantra", "Perkele 5" },
                { "Kanjut", "Perkele 5 a" },
            } },
            { "Persephone", new Dictionary<string, string> {
                { "Kore", "Persephone 1" },
            } },
            { "Pethes", new Dictionary<string, string> {
                { "Van Lang", "Pethes 1" },
            } },
            { "Phanes", new Dictionary<string, string> {
                { "Wilde's World", "Phanes 3" },
                { "Muztagh", "Phanes 3 a" },
            } },
            { "Phekda", new Dictionary<string, string> {
                { "Haynes's Hole", "Phekda 1 a" },
                { "Nirvana", "Phekda 4 a" },
                { "New California", "Phekda 4 b" },
                { "Topaz", "Phekda 4 c" },
                { "Ousey", "Phekda 5 a" },
                { "Francis's Wreck", "Phekda 6 a" },
            } },
            { "Phiagre", new Dictionary<string, string> {
                { "Coates's Wreck", "Phiagre 2" },
                { "Stevenson", "Phiagre 3" },
                { "Kurinjal", "Phiagre 3 a" },
            } },
            { "Phiince", new Dictionary<string, string> {
                { "Kaptai", "Phiince 1" },
            } },
            { "Pi Mensae", new Dictionary<string, string> {
                { "Trueman's Paradise", "Pi Mensae 3" },
            } },
            { "Pi-fang", new Dictionary<string, string> {
                { "Major's Wreck", "Pi-fang 1" },
                { "Cooper Reward", "Pi-fang 2" },
            } },
            { "PLX 695", new Dictionary<string, string> {
                { "Secular I", "PLX 695 1" },
            } },
            { "Pollux", new Dictionary<string, string> {
                { "Cambridge", "Pollux 2 a" },
            } },
            { "Psi Octantis", new Dictionary<string, string> {
                { "Kanaloa", "Psi Octantis 1" },
                { "Kane", "Psi Octantis 2" },
            } },
            { "Quator", new Dictionary<string, string> {
                { "Camp Hooper", "Quator 1" },
                { "Massey's World", "Quator 2" },
            } },
            { "Quince", new Dictionary<string, string> {
                { "Kosmala Rewards", "Quince 1" },
                { "New America", "Quince 5" },
            } },
            { "Quiness", new Dictionary<string, string> {
                { "Quy", "Quiness 1" },
            } },
            { "Rakapila", new Dictionary<string, string> {
                { "Kendal", "Rakapila 1" },
            } },
            { "Rasmussen", new Dictionary<string, string> {
                { "Rasmussen", "Rasmussen 1" },
                { "Alphubel", "Rasmussen 1 a" },
            } },
            { "Reddot", new Dictionary<string, string> {
                { "Motherlode", "Reddot 1" },
            } },
            { "Reorte", new Dictionary<string, string> {
                { "Home", "Reorte 4" },
                { "Camp Nakamichi", "Reorte 4 a" },
            } },
            { "Reynes", new Dictionary<string, string> {
                { "Ohajiki", "Reynes 1" },
            } },
            { "Riedquat", new Dictionary<string, string> {
                { "Waterloo", "Riedquat 1" },
            } },
            { "Ross 1015", new Dictionary<string, string> {
                { "Der", "Ross 1015 1" },
            } },
            { "Ross 1051", new Dictionary<string, string> {
                { "Brace", "Ross 1051 1" },
            } },
            { "Ross 1057", new Dictionary<string, string> {
                { "Munam-ri", "Ross 1057 1" },
                { "Else", "Ross 1057 1 a" },
            } },
            { "Ross 128", new Dictionary<string, string> {
                { "Grant's Claim", "Ross 128 1" },
            } },
            { "Ross 154", new Dictionary<string, string> {
                { "Aster", "Ross 154 1" },
                { "Merlin", "Ross 154 1 a" },
                { "Dust Ball", "Ross 154 2" },
            } },
            { "Ross 33", new Dictionary<string, string> {
                { "Grüne Hölle", "Ross 33 1" },
            } },
            { "Ross 345", new Dictionary<string, string> {
                { "Brigantia", "Ross 345 1" },
            } },
            { "Ross 444", new Dictionary<string, string> {
                { "Hutton Prime", "Ross 444 1" },
            } },
            { "Ross 780", new Dictionary<string, string> {
                { "Wireworld", "Ross 780 2" },
            } },
            { "Ross 788", new Dictionary<string, string> {
                { "Galava", "Ross 788 1" },
                { "Ambleside", "Ross 788 1 a" },
            } },
            { "Ross 986", new Dictionary<string, string> {
                { "Democracy", "Ross 986 1" },
                { "Diran", "Ross 986 1 a" },
                { "Camp Patrick", "Ross 986 2" },
            } },
            { "Rudjer Boskovic", new Dictionary<string, string> {
                { "New Serbia", "Rudjer Boskovic 1" },
            } },
            { "Run", new Dictionary<string, string> {
                { "Danny's World", "Run 1" },
            } },
            { "Runo", new Dictionary<string, string> {
                { "EAL 1141", "Runo 1" },
            } },
            { "Saffron", new Dictionary<string, string> {
                { "Ectoplasm", "Saffron 3" },
            } },
            { "Samkyha", new Dictionary<string, string> {
                { "Hudson Brooks", "Samkyha 1" },
            } },
            { "Sanna", new Dictionary<string, string> {
                { "Erdogan-4", "Sanna 1" },
                { "Caher", "Sanna 1 a" },
            } },
            { "Sedna", new Dictionary<string, string> {
                { "Gayda", "Sedna 1 a" },
            } },
            { "Shibboleth", new Dictionary<string, string> {
                { "Shibboleth Haven", "Shibboleth 1" },
                { "Kyllikki", "Shibboleth 1 a" },
            } },
            { "Shinigami", new Dictionary<string, string> {
                { "Bodger's World", "Shinigami 1" },
                { "Grutopia", "Shinigami 1 a" },
            } },
            { "Sigma Hydri", new Dictionary<string, string> {
                { "Realm", "Sigma Hydri 1" },
            } },
            { "Skardee", new Dictionary<string, string> {
                { "Skardee I", "Skardee 1" },
                { "Skardee II", "Skardee 2" },
                { "Skardee III", "Skardee 3" },
                { "Skardee IV", "Skardee 4" },
                { "Skardee V", "Skardee 5" },
            } },
            { "Soholia", new Dictionary<string, string> {
                { "Simpson Hollow", "Soholia 1" },
                { "Molotovworld", "Soholia 2" },
                { "Bradley's Legacy", "Soholia 3" },
            } },
            { "Sol", new Dictionary<string, string> {
                { "Mercury", "Sol 1" },
                { "Venus", "Sol 2" },
                { "Earth", "Sol 3" },
                { "Moon", "Sol 3 a" },
                { "Mars", "Sol 4" },
                { "Jupiter", "Sol 5" },
                { "Jupiter Halo Ring", "Sol 5 A Ring" },
                { "Io", "Sol 5 a" },
                { "Europa", "Sol 5 b" },
                { "Ganymede", "Sol 5 c" },
                { "Callisto", "Sol 5 d" },
                { "Saturn", "Sol 6" },
                { "Enceladus", "Sol 6 a" },
                { "Tethys", "Sol 6 b" },
                { "Dione", "Sol 6 c" },
                { "Rhea", "Sol 6 d" },
                { "Titan", "Sol 6 e" },
                { "Iapetus", "Sol 6 f" },
                { "Uranus", "Sol 7" },
                { "Ariel", "Sol 7 a" },
                { "Umbriel", "Sol 7 b" },
                { "Titania", "Sol 7 c" },
                { "Oberon", "Sol 7 d" },
                { "Neptune", "Sol 8" },
                { "Triton", "Sol 8 a" },
                { "Pluto", "Sol 9" },
                { "Charon", "Sol 10" },
                { "90482 Orcus", "Sol 11" },
                { "Vanth", "Sol 11 a" },
                { "(307261) 2002 MS4", "Sol 12" },
                { "Salacia", "Sol 13" },
                { "Actaea", "Sol 13 a" },
                { "Haumea", "Sol 14" },
                { "Hi'iaka", "Sol 14 a" },
                { "Quaoar", "Sol 15" },
                { "Makemake", "Sol 16" },
                { "(225088) 2007 OR10", "Sol 17" },
                { "Eris", "Sol 18" },
                { "Sedna", "Sol 19" },
                { "Persephone", "Sol 20" },
            } },
            { "Solati", new Dictionary<string, string> {
                { "Saena", "Solati 5" },
                { "Halla", "Solati 5 a" },
            } },
            { "Stafkarl", new Dictionary<string, string> {
                { "Jaufurally", "Stafkarl 1" },
            } },
            { "Suddene", new Dictionary<string, string> {
                { "New Rutland", "Suddene 1" },
                { "Horn", "Suddene 2" },
            } },
            { "Summerland", new Dictionary<string, string> {
                { "Henry O'Hare's Haven", "Summerland 6" },
            } },
            { "Surya", new Dictionary<string, string> {
                { "Arka", "Surya 3" },
            } },
            { "Svass", new Dictionary<string, string> {
                { "Rooney World", "Svass 2" },
                { "Moroto", "Svass 2 a" },
            } },
            { "Taevaisa", new Dictionary<string, string> {
                { "Roque", "Taevaisa 1" },
                { "Louannchar", "Taevaisa 4" },
            } },
            { "Tamor", new Dictionary<string, string> {
                { "Lukka", "Tamor 4" },
            } },
            { "Tangaroa", new Dictionary<string, string> {
                { "Mirnipli", "Tangaroa 1" },
                { "Imishli", "Tangaroa 1 a" },
            } },
            { "Tanmark", new Dictionary<string, string> {
                { "Luca L [V4-26]", "Tanmark 1" },
                { "Icemark", "Tanmark 6" },
            } },
            { "Tarach Tor", new Dictionary<string, string> {
                { "Hanandroo", "Tarach Tor 3" },
                { "Moel Sych", "Tarach Tor 4" },
            } },
            { "Tau Ceti", new Dictionary<string, string> {
                { "Saunders's Claim", "Tau Ceti 1" },
                { "Taylor Colony", "Tau Ceti 2" },
                { "Bell's Wreck", "Tau Ceti 3" },
            } },
            { "Tau-1 Eridani", new Dictionary<string, string> {
                { "Paul's Folly", "Tau-1 Eridani 3" },
                { "Pomiu", "Tau-1 Eridani 3 a" },
            } },
            { "Te Uira", new Dictionary<string, string> {
                { "Porsenna", "Te Uira 5" },
                { "Labyrinthia", "Te Uira 6" },
            } },
            { "Tengri", new Dictionary<string, string> {
                { "Keevals Retreat", "Tengri 1" },
                { "Gokturk", "Tengri 2" },
            } },
            { "Terra Mater", new Dictionary<string, string> {
                { "New Terra", "Terra Mater 5" },
            } },
            { "Tethlon", new Dictionary<string, string> {
                { "Tethlon", "Tethlon 1" },
            } },
            { "Tetrian", new Dictionary<string, string> {
                { "Gidim", "Tetrian 5" },
                { "John Bradley Hope", "Tetrian 6" },
            } },
            { "Thalarctos", new Dictionary<string, string> {
                { "Stibium", "Thalarctos 2" },
            } },
            { "Thanatos", new Dictionary<string, string> {
                { "The Land", "Thanatos 1" },
                { "Kamet", "Thanatos 1 a" },
            } },
            { "Themiscrya", new Dictionary<string, string> {
                { "Lily May World", "Themiscrya 1" },
            } },
            { "Thethys", new Dictionary<string, string> {
                { "Sanssouci", "Thethys 1" },
                { "Dejen", "Thethys 1 a" },
            } },
            { "Thule", new Dictionary<string, string> {
                { "Frogspawn", "Thule 2" },
            } },
            { "Tiliala", new Dictionary<string, string> {
                { "Valhalla", "Tiliala 3" },
                { "Saitoro", "Tiliala 3 a" },
                { "Democracy", "Tiliala 3 b" },
            } },
            { "Tilian", new Dictionary<string, string> {
                { "Keita", "Tilian 1" },
                { "Bello", "Tilian 1 a" },
            } },
            { "Tionisla", new Dictionary<string, string> {
                { "New Caledonia", "Tionisla 1" },
            } },
            { "Toci", new Dictionary<string, string> {
                { "Tialli", "Toci 1" },
            } },
            { "Tomas", new Dictionary<string, string> {
                { "Doubt", "Tomas 1" },
            } },
            { "Toor", new Dictionary<string, string> {
                { "Tudras", "Toor 1" },
            } },
            { "Treima", new Dictionary<string, string> {
                { "Haumod", "Treima 1" },
            } },
            { "Ursitoare", new Dictionary<string, string> {
                { "Elizabeth Young's World", "Ursitoare 2" },
                { "Beenkeragh", "Ursitoare 3" },
            } },
            { "Uszaa", new Dictionary<string, string> {
                { "Jameson's Pride", "Uszaa 7" },
            } },
            { "Utgaroar", new Dictionary<string, string> {
                { "Hamlet", "Utgaroar 1" },
            } },
            { "van Maanen's Star", new Dictionary<string, string> {
                { "Major", "van Maanen's Star 1" },
            } },
            { "Vargerson", new Dictionary<string, string> {
                { "Miranda II", "Vargerson 1" },
            } },
            { "Vega", new Dictionary<string, string> {
                { "Popov Reward", "Vega 1" },
                { "Tracy's Havan", "Vega 3" },
                { "Trikora", "Vega 3 a" },
            } },
            { "Vequess", new Dictionary<string, string> {
                { "Halki", "Vequess 1" },
            } },
            { "VESPER-M4", new Dictionary<string, string> {
                { "Slough", "VESPER-M4 7" },
            } },
            { "Viracocha", new Dictionary<string, string> {
                { "Tampu", "Viracocha 1" },
                { "Aural", "Viracocha 1 a" },
            } },
            { "Wakea", new Dictionary<string, string> {
                { "Robertson's Renown", "Wakea 1" },
                { "Meru", "Wakea 1 a" },
            } },
            { "Wolf 1301", new Dictionary<string, string> {
                { "Navia", "Wolf 1301 1" },
            } },
            { "Wolf 1323", new Dictionary<string, string> {
                { "Karpo", "Wolf 1323 1" },
                { "Rattus", "Wolf 1323 2" },
            } },
            { "Wolf 1515", new Dictionary<string, string> {
                { "Gralmond", "Wolf 1515 1" },
            } },
            { "Wolf 248", new Dictionary<string, string> {
                { "Balmhorn", "Wolf 248 1" },
            } },
            { "Wolf 25", new Dictionary<string, string> {
                { "Hishutash", "Wolf 25 1" },
                { "Pagon", "Wolf 25 1" },
            } },
            { "Wolf 294", new Dictionary<string, string> {
                { "Jeffries Rock", "Wolf 294 1" },
            } },
            { "Wolf 359", new Dictionary<string, string> {
                { "Campbell's Claim", "Wolf 359 1" },
                { "Camp Donalds", "Wolf 359 2" },
            } },
            { "Wolf 397", new Dictionary<string, string> {
                { "Rhodius", "Wolf 397 1" },
                { "Trus Madi", "Wolf 397 1 a" },
            } },
            { "Wolf 412", new Dictionary<string, string> {
                { "Haraiva", "Wolf 412 6" },
            } },
            { "Wolf 54", new Dictionary<string, string> {
                { "Gord'Ena", "Wolf 54 1" },
                { "Druman", "Wolf 54 1 a" },
            } },
            { "Wolf 562", new Dictionary<string, string> {
                { "Luke Sears", "Wolf 562 1" },
                { "Kenneos", "Wolf 562 2" },
                { "Laila's Memory", "Wolf 562 3" },
            } },
            { "Wolfberg", new Dictionary<string, string> {
                { "Azille", "Wolfberg 1" },
            } },
            { "Yinjian", new Dictionary<string, string> {
                { "The Hoonage Dominion", "Yinjian 1" },
                { "Markham", "Yinjian 1 a" },
            } },
            { "Ys", new Dictionary<string, string> {
                { "Chroin", "Ys 3" },
                { "Blob", "Ys 4" },
            } },
            { "Zaonce", new Dictionary<string, string> {
                { "Industry", "Zaonce 3" },
            } },
            { "Zeaex", new Dictionary<string, string> {
                { "Cambridge's Hole", "Zeaex 3" },
                { "Ashfield's World", "Zeaex 4" },
                { "Democracy", "Zeaex 5" },
                { "Irvin's Wreck", "Zeaex 6" },
                { "Schmidt Rock", "Zeaex 7 a" },
                { "Ulrich's Mine", "Zeaex 8" },
            } },
            { "Zelada", new Dictionary<string, string> {
                { "New Africa", "Zelada 1" },
                { "Singh Hollow", "Zelada 2" },
            } },
            { "Zephyrus", new Dictionary<string, string> {
                { "Violet World", "Zephyrus 2" },
            } },
            { "Zeta Tucanae", new Dictionary<string, string> {
                { "Furths", "Zeta Tucanae 5" },
                { "Darwyn", "Zeta Tucanae 6" },
            } },
            { "Zeus", new Dictionary<string, string> {
                { "Salisbury", "Zeus 4" },
                { "Mulanje", "Zeus 4 a" },
            } },
            { "Aku", new Dictionary<string, string> {
                { "Kitsu's Rest", "Aku 1" },
            } },
            { "Anapos", new Dictionary<string, string> {
                { "Jackson", "Anapos 1" },
                { "Emilia", "Anapos 5" },
            } },
            { "BD-01 2075", new Dictionary<string, string> {
                { "KT-Lin", "BD-01 2075 1 a" },
            } },
            { "BD-08 1218", new Dictionary<string, string> {
                { "Wolf's Claim", "BD-08 1218 2 a" },
            } },
            { "CD-41 359", new Dictionary<string, string> {
                { "Gem (HD 8535 b)", "CD-41 359 1" },
                { "Garthyre", "CD-41 359 1 a" },
            } },
            { "Fomalhaut", new Dictionary<string, string> {
                { "Lawrence's Hole", "Fomalhaut 1" },
                { "London's Legacy", "Fomalhaut 2" },
                { "Biggs's Hole", "Fomalhaut 3" },
                { "Conversion", "Fomalhaut 4" },
                { "Strauss", "Fomalhaut 5 a" },
            } },
            { "HIP 106006", new Dictionary<string, string> {
                { "Rakus' Memorial", "HIP 106006 1" },
            } },
            { "HIP 112441", new Dictionary<string, string> {
                { "Scheffey's Solace", "HIP 112441 1" },
            } },
            { "HIP 44291", new Dictionary<string, string> {
                { "Karl-Heinz Lechtermann", "HIP 44291 1" },
            } },
            { "LHS 2335", new Dictionary<string, string> {
                { "Jon Harris", "LHS 2335 1" },
            } },
            { "LP 298-42", new Dictionary<string, string> {
                { "Coates Paradise", "LP 298-42 1" },
            } },
            { "LTT 149", new Dictionary<string, string> {
                { "Noon", "LTT 149 2" },
                { "Stardust", "LTT 149 3" },
            } },
            { "LTT 1581", new Dictionary<string, string> {
                { "Grum", "LTT 1581 1" },
            } },
            { "Mat Zemlya", new Dictionary<string, string> {
                { "Sonia's Haven", "Mat Zemlya 2" },
            } },
            { "Mu Arae", new Dictionary<string, string> {
                { "Dawlings Rest", "Mu Arae 1" },
            } },
            { "Ninhursag", new Dictionary<string, string> {
                { "Kerinci", "Ninhursag 1" },
            } },
            { "Ross 905", new Dictionary<string, string> {
                { "Robert Kelley", "Ross 905 1" },
            } },
            { "HIP 3574", new Dictionary<string, string> {
                { "HD 4313 b", "HIP 3574 1" },
            } },
            { "Synuefe MU-V c16-0", new Dictionary<string, string> {
                { "Rich", "Synuefe MU-V c16-0 A 1" },
                { "Aust", "Synuefe MU-V c16-0 A 2" },
            } },
            { "HIP 1254", new Dictionary<string, string> {
                { "Laika", "HIP 1254 1" },
            } },
            { "LHS 188", new Dictionary<string, string> {
                { "Aiden Collar", "LHS 188 1" },
                { "Teayum", "LHS 188 2" },
            } },
            { "Kepler-20", new Dictionary<string, string> {
                { "PER - G8XBE", "Kepler-20 1" },
                { "Rob's Place", "Kepler-20 2" },
                { "Maxine's World", "Kepler-20 3" },
            } },
            { "MOA-2007-BLG-400L", new Dictionary<string, string> {
                { "MOA-2007-BLG-400L b", "MOA-2007-BLG-400L 1" }
            } },
            { "Eol Procul Centauri", new Dictionary<string, string> {
                { "Hutton Moon", "Eol Procul Centauri 1" }
            } },
            { "61 Virginis", new Dictionary<string, string> {
                { "Rubin's Discovery", "61 Virginis 1" }
            } },
            { "Bragurom Du", new Dictionary<string, string> {
                { "Remlok's Claim", "Bragurom Du 2 a" }
            } },
            { "LHS 2610", new Dictionary<string, string> {
                { "Evangeline's Grace", "LHS 2610 4 a" }
            } },
            { "Eowrairsts JA-P c19-0", new Dictionary<string, string> {
                { "Farwalker Point", "Eowrairsts JA-P c19-0 7 a" }
            } },
            { "George Pantazis", new Dictionary<string, string> {
                { "Anew", "George Pantazis A 12" }
            } },
            { "LHS 2038", new Dictionary<string, string> {
                { "Frauke", "LHS 2038 C 1" }
            } },
            { "California Sector HW-W c1-5", new Dictionary<string, string> {
                { "Emma's Hope", "California Sector HW-W c1-5 A 3" }
            } },
            { "CD-43 11917", new Dictionary<string, string> {
                { "Ares", "CD-43 11917 1" }
            } },
            { "Hypuae Aim ZP-R b48-0", new Dictionary<string, string>
            {
                { "Travis Green", "Hypuae Aim ZP-R b48-0 1" }
            } },
            { "HR 7291", new Dictionary<string, string>
            {
                { "Rick's Retreat", "HR 7291 7 d" },
                { "Annette's Paradise", "HR 7291 7 e" }
            } },
            { "Pepper", new Dictionary<string, string>
            {
                { "Pepper", "Pepper A 1" }
            } },
            { "Praea Euq WD-T c3-36", new Dictionary<string, string> {
                { "Miola", "Praea Euq WD-T c3-36 B 9 a" }
            } },
            { "Sapill", new Dictionary<string, string> {
                { "LAWMAN", "Sapill A 3 a" },
            } },
            { "Dubbuennel", new Dictionary<string, string> {
                { "Garibaldi", "Dubbuennel 5" },
            } },
            { "Lambda Aquarii", new Dictionary<string, string> {
                { "Taylor Dale", "Lambda Aquarii 8" },
            } },
            { "Trapezium Sector AF-Z c1", new Dictionary<string, string> {
                { "Mike Meister", "Trapezium Sector AF-Z c1 A 10" },
            } },
            { "HIP 63835", new Dictionary<string, string> {
                { "Josie Rosa", "HIP 63835 CD 8" },
            } },
            { "4 Arietis", new Dictionary<string, string> {
                { "Ziva", "4 Arietis 3 a" },
            } },
        };

        public struct DesigMap
        {
            public string Name { get; set; }
            public string Designation { get; set; }
            public bool CaseSensitive { get; set; }

            public DesigMap(string name, string desig, bool caseSensitive = false)
            {
                Name = name;
                Designation = desig;
                CaseSensitive = caseSensitive;
            }

            public bool NameEquals(string name)
            {
                return Name.Equals(name, CaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public static Dictionary<string, Dictionary<int, DesigMap>> ByBodyId = new Dictionary<string, Dictionary<int, DesigMap>>
        {
            { "9 Aurigae", new Dictionary<int, DesigMap> {
                { 4, new DesigMap("9 Aurigae B", "9 Aurigae A") },
                { 5, new DesigMap("9 Aurigae", "9 Aurigae B") },
                { 6, new DesigMap("9 Aurigae C", "9 Aurigae C") },
                { 7, new DesigMap("9 Aurigae C", "9 Aurigae D") },
                { 8, new DesigMap("9 Aurigae E", "9 Aurigae E") }
            } },
            { "m Centauri", new Dictionary<int, DesigMap> {
                { 1, new DesigMap("m Centauri", "m Centauri A", true) },
                { 2, new DesigMap("M Centauri", "m Centauri B", true) }
            } },
            { "39 Omicron Ophiuchi", new Dictionary<int, DesigMap>
            {
                { 1, new DesigMap("39 Omicron Ophiuchi", "39 Omicron Ophiuchi A") },
                { 2, new DesigMap("39 Omicron Ophiuchi", "39 Omicron Ophiuchi B") }
            } },
            { "74 Psi-1 Piscium", new Dictionary<int, DesigMap>
            {
                { 1, new DesigMap("74 Psi-1 Piscium", "74 Psi-1 Piscium A") },
                { 2, new DesigMap("74 Psi-1 Piscium", "74 Psi-1 Piscium B") }
            } },
            { "UCAC3 70-2386", new Dictionary<int, DesigMap>
            {
                { 1, new DesigMap("UCAC3 70-2386", "UCAC3 70-2386 A") },
                { 2, new DesigMap("UCAC3 70-2386", "UCAC3 70-2386 B") }
            } },

        };
    }
}
