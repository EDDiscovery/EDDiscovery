using BaseUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogEntry
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Path CMDRname [options]\n" +
                                  "Options: FSD name x y z [A]\n" +
                                  "Options: Loc name x y z\n" +
                                  "Options: Interdiction name success isplayer combatrank faction power\n" +
                                  "Options: Docked, Undocked, Touchdown, Liftoff, CommitCrime MissionCompleted MissionCompleted2 MiningRefined\n" +
                                  "Options: ScanPlanet ScanStar NavBeaconScan ScanEarth SellShipOnRebuy SearchANdRescue MissionRedirected\n" +
                                  "Options: RepairDrone CommunityGoal\n" +
                                  "Options: MusicNormal MusicGalMap MusicSysMap\n" +
                                  "Options: Friends Name\n" +
                                  "Options: FuelScoop amount total\n" +
                                  "Or EDDBSTARS <filename> or EDDBPLANETS or EDDBSTARNAMES for the eddb dump\n" +
                                  "Or Phoneme <filename> <fileout> for EDDI phoneme tx\n" +
                                  "Or Voicerecon <filename>" +
                                  "Path = <pathto>Journal.<name>.log (example c:\\test\\Journal.test1.log)\n" +
                                  "x y z = position as double\n" +
                                  "A means auto mode, Space make a new system Return enters chatter\n"
                                  );
                return;
            }

            string filename = args[0];
            string cmdrname = args[1];

            if (args.Length >= 2 && filename.Equals("EDDBSTARS", StringComparison.InvariantCultureIgnoreCase))
            {
                EDDBLog(cmdrname, "\"Star\"", "\"spectral_class\"", "Star class ");
                return;
            }
            if (args.Length >= 2 && filename.Equals("EDDBPLANETS", StringComparison.InvariantCultureIgnoreCase))
            {
                EDDBLog(cmdrname, "\"Planet\"", "\"type_name\"", "Planet class");
                return;
            }

            if (args.Length >= 2 && filename.Equals("EDDBSTARNAMES", StringComparison.InvariantCultureIgnoreCase))
            {
                EDDBLog(cmdrname, "\"Star\"", "\"name\"", "Star Name");
                return;
            }

            if (args.Length >= 2 && filename.Equals("voicerecon", StringComparison.InvariantCultureIgnoreCase))
            {
                Bindings(cmdrname);
                return;
            }

            if (args.Length >= 3 && filename.Equals("Phoneme", StringComparison.InvariantCultureIgnoreCase))
            {
                Phoneme(cmdrname, args[2]);
                return;
            }

            if (args.Length < 3)
            {
                Console.WriteLine("More parameters see help run without options");
                return;
            }

            if (!File.Exists(filename))
            {
                using (Stream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter sr = new StreamWriter(fs))
                    {
                        string line = "{ " + TimeStamp() + "\"event\":\"Fileheader\", \"part\":1, \"language\":\"English\\\\UK\", \"gameversion\":\"2.2 (Beta 2)\", \"build\":\"r121783/r0 \" }";
                        sr.WriteLine(line);
                        Console.WriteLine(line);

                        string line2 = "{ " + TimeStamp() + "\"event\":\"LoadGame\", \"Commander\":\"" + cmdrname + "\", \"Ship\":\"Anaconda\", \"ShipID\":14, \"GameMode\":\"Open\", \"Credits\":18670609, \"Loan\":0 }";
                        sr.WriteLine(line2);
                        Console.WriteLine(line2);
                    }
                }
            }

            string writetype = args[2];

            string[] optparas = new string[args.Length - 3];
            Array.Copy(args, 3, optparas, 0, args.Length - 3);

            Random rnd = new Random();

            string lineout = null;      //quick writer

            if (writetype.Equals("FSD", StringComparison.InvariantCultureIgnoreCase))
                FSDJump(optparas, filename);
            else if (writetype.Equals("LOC", StringComparison.InvariantCultureIgnoreCase))
                Loc(optparas, filename);
            else if (writetype.Equals("Interdiction", StringComparison.InvariantCultureIgnoreCase))
                Interdiction(optparas, filename);
            else if (writetype.Equals("Docked", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + "\"event\":\"Docked\", " +
                    "\"StationName\":\"Jameson Memorial\", " +
                    "\"StationType\":\"Orbis\", \"StarSystem\":\"Shinrarta Dezhra\", \"Faction\":\"The Pilots Federation\", \"Allegiance\":\"Independent\", \"Economy\":\"$economy_HighTech;\", \"Economy_Localised\":\"High tech\", \"Government\":\"$government_Democracy;\", \"Government_Localised\":\"Democracy\", \"Security\":\"$SYSTEM_SECURITY_high;\", \"Security_Localised\":\"High Security\" }";
            else if (writetype.Equals("Undocked", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + "\"event\":\"Undocked\", " + "\"StationName\":\"Jameson Memorial\",\"StationType\":\"Orbis\" }";
            else if (writetype.Equals("Liftoff", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + "\"event\":\"Liftoff\", " + "\"Latitude\":7.141173, \"Longitude\":95.256424 }";
            else if (writetype.Equals("Touchdown", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + "\"event\":\"Touchdown\", " + "\"Latitude\":7.141173, \"Longitude\":95.256424 }";
            else if (writetype.Equals("CommitCrime", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + "\"event\":\"CommitCrime\", \"CrimeType\":\"collidedAtSpeedInNoFireZone\", \"Faction\":\"Revolutionary Party of Ciguri\", \"Fine\":100 }";
            else if (writetype.Equals("MissionCompleted", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "MissionCompleted") + F("Faction", "whooo") + F("Name", "Mission_massacre") +
                            F("MissingID", "29292") + F("Reward", "82272") + " \"CommodityReward\":[ { \"Name\": \"CoolingHoses\", \"Count\": 4 } ] }";
            else if (writetype.Equals("MissionCompleted2", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "MissionCompleted") + F("Faction", "whooo") + F("Name", "Mission_massacre") +
                            F("MissingID", "29292") + F("Reward", "82272") + " \"CommodityReward\":[ { \"Name\": \"CoolingHoses\", \"Count\": 4 } , { \"Name\": \"Fish\", \"Count\": 10 } ] }";
            else if (writetype.Equals("MiningRefined", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "MiningRefined") + FF("Type", "Gold") + " }";
            else if (writetype.Equals("EngineerCraft", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "EngineerCraft") + F("Engineer", "Robert") + F("Blueprint", "FSD_LongRange")
                    + F("Level", "5") + "\"Ingredients\":{ \"magneticemittercoil\":1, \"arsenic\":1, \"chemicalmanipulators\":1, \"dataminedwake\":1 } }";
            else if (writetype.Equals("NavBeaconScan", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "NavBeaconScan") + FF("NumBodies", "3") + " }";
            else if (writetype.Equals("ScanPlanet", StringComparison.InvariantCultureIgnoreCase))
            {
                int rn = rnd.Next(10);
                lineout = "{ " + TimeStamp() + F("event", "Scan") + "\"BodyName\":\"Merope " + rn + "\", \"DistanceFromArrivalLS\":639.245483, \"TidalLock\":true, \"TerraformState\":\"\", \"PlanetClass\":\"Metal rich body\", \"Atmosphere\":\"\", \"AtmosphereType\":\"None\", \"Volcanism\":\"rocky magma volcanism\", \"MassEM\":0.010663, \"Radius\":1163226.500000, \"SurfaceGravity\":3.140944, \"SurfaceTemperature\":1068.794067, \"SurfacePressure\":0.000000, \"Landable\":true, \"Materials\":[ { \"Name\":\"iron\", \"Percent\":36.824127 }, { \"Name\":\"nickel\", \"Percent\":27.852226 }, { \"Name\":\"chromium\", \"Percent\":16.561033 }, { \"Name\":\"zinc\", \"Percent\":10.007420 }, { \"Name\":\"selenium\", \"Percent\":2.584032 }, { \"Name\":\"tin\", \"Percent\":2.449526 }, { \"Name\":\"molybdenum\", \"Percent\":2.404594 }, { \"Name\":\"technetium\", \"Percent\":1.317050 } ], \"SemiMajorAxis\":1532780800.000000, \"Eccentricity\":0.000842, \"OrbitalInclination\":-1.609496, \"Periapsis\":179.381393, \"OrbitalPeriod\":162753.062500, \"RotationPeriod\":162754.531250, \"AxialTilt\":0.033219 }";
            }
            else if (writetype.Equals("ScanStar", StringComparison.InvariantCultureIgnoreCase))
            {
                int rn = rnd.Next(10);
                lineout = "{ " + TimeStamp() + F("event", "Scan") + "\"BodyName\":\"Merope A\", \"DistanceFromArrivalLS\":0.000000, \"StarType\":\"B\", \"StellarMass\":8.597656, \"Radius\":2854249728.000000, \"AbsoluteMagnitude\":1.023468, \"Age_MY\":182, \"SurfaceTemperature\":23810.000000, \"Luminosity\":\"IV\", \"SemiMajorAxis\":12404761034752.000000, \"Eccentricity\":0.160601, \"OrbitalInclination\":18.126791, \"Periapsis\":49.512009, \"OrbitalPeriod\":54231617536.000000, \"RotationPeriod\":110414.359375, \"AxialTilt\":0.000000 }";
            }
            else if (writetype.Equals("ScanEarth", StringComparison.InvariantCultureIgnoreCase))
            {
                int rn = rnd.Next(10);
                lineout = "{ " + TimeStamp() + F("event", "Scan") + "\"BodyName\":\"Merope " + rn + "\", \"DistanceFromArrivalLS\":901.789856, \"TidalLock\":false, \"TerraformState\":\"Terraformed\", \"PlanetClass\":\"Earthlike body\", \"Atmosphere\":\"\", \"AtmosphereType\":\"EarthLike\", \"AtmosphereComposition\":[ { \"Name\":\"Nitrogen\", \"Percent\":92.386833 }, { \"Name\":\"Oxygen\", \"Percent\":7.265749 }, { \"Name\":\"Water\", \"Percent\":0.312345 } ], \"Volcanism\":\"\", \"MassEM\":0.840000, \"Radius\":5821451.000000, \"SurfaceGravity\":9.879300, \"SurfaceTemperature\":316.457062, \"SurfacePressure\":209183.453125, \"Landable\":false, \"SemiMajorAxis\":264788426752.000000, \"Eccentricity\":0.021031, \"OrbitalInclination\":13.604733, \"Periapsis\":73.138206, \"OrbitalPeriod\":62498732.000000, \"RotationPeriod\":58967.023438, \"AxialTilt\":-0.175809 }";
            }
            else if (writetype.Equals("AFMURepairs", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "AfmuRepairs") + "\"Module\":\"$modularcargobaydoor_name;\", \"Module_Localised\":\"Cargo Hatch\", \"FullyRepaired\":true, \"Health\":1.000000 }";

            else if (writetype.Equals("SellShipOnRebuy", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "SellShipOnRebuy") + "\"ShipType\":\"Dolphin\", \"System\":\"Shinrarta Dezhra\", \"SellShipId\":4, \"ShipPrice\":4110183 }";

            else if (writetype.Equals("SearchAndRescue", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "SearchAndRescue") + "\"Name\":\"Fred\", \"Count\":50, \"Reward\":4110183 }";

            else if (writetype.Equals("MissionRedirected", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "MissionRedirected") + "\"MissionID\": 65367315, \"NewDestinationStation\": \"Metcalf Orbital\", \"OldDestinationStation\": \"Cuffey Orbital\", \"NewDestinationSystem\": \"Cemiess\", \"OldDestinationSystem\": \"Vequess\" }";

            else if (writetype.Equals("RepairDrone", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "RepairDrone") + "\"HullRepaired\": 0.23, \"CockpitRepaired\": 0.1,  \"CorrosionRepaired\": 0.5 }";

            else if (writetype.Equals("CommunityGoal", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "CommunityGoal") + "\"CurrentGoals\":[ { \"CGID\":726, \"Title\":\"Alliance Research Initiative - Trade\", \"SystemName\":\"Kaushpoos\", \"MarketName\":\"Neville Horizons\", \"Expiry\":\"2017-08-17T14:58:14Z\", \"IsComplete\":false, \"CurrentTotal\":10062, \"PlayerContribution\":562, \"NumContributors\":101, \"TopRankSize\":10, \"PlayerInTopRank\":false, \"TierReached\":\"Tier 1\", \"PlayerPercentileBand\":50, \"Bonus\":200000 } ] }";

            else if (writetype.Equals("MusicNormal", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "Music") + FF("MusicTrack", "NoTrack") + " }";
            else if (writetype.Equals("MusicSysMap", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "Music") + FF("MusicTrack", "SystemMap") + " }";
            else if (writetype.Equals("MusicGalMap", StringComparison.InvariantCultureIgnoreCase))
                lineout = "{ " + TimeStamp() + F("event", "Music") + FF("MusicTrack", "GalaxyMap") + " }";
            else if (writetype.Equals("Friends", StringComparison.InvariantCultureIgnoreCase) && optparas.Length >= 1)
                lineout = "{ " + TimeStamp() + F("event", "Friends") + F("Status", "Online") + FF("Name", optparas[0]) + " }";
            else if (writetype.Equals("FuelScoop", StringComparison.InvariantCultureIgnoreCase) && optparas.Length >= 2)
                lineout = "{ " + TimeStamp() + F("event", "FuelScoop") + F("Scooped", optparas[0]) + FF("Total", optparas[1]) + " }";

            if (lineout != null)
                Write(filename, lineout);
        }

        static void Loc(string[] args, string filename)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("More parameters");
                return;
            }

            double x = double.NaN, y = 0, z = 0;
            string starnameroot = args[0];

            if (!double.TryParse(args[1], out x) || !double.TryParse(args[2], out y) || !double.TryParse(args[3], out z))
            {
                Console.WriteLine("X,y,Z must be numbers");
                return;
            }

            string line = "{ " + TimeStamp() + "\"event\":\"Location\", " +
                "\"StarSystem\":\"" + starnameroot +
                "\", \"StarPos\":[" + x.ToString("0.00") + ", " + y.ToString("0.00") + ", " + z.ToString("0.00") +
                "], \"Allegiance\":\"\", \"Economy\":\"$economy_None;\", \"Economy_Localised\":\"None\", \"Government\":\"$government_None;\", \"Government_Localised\":\"None\", \"Security\":\"$SYSTEM_SECURITY_low;\", \"Security_Localised\":\"Low Security\" }";

            Write(filename, line);
        }

        //                                  "Options: Interdiction Loc name success isplayer combatrank faction power\n" +
        static void Interdiction(string[] args, string filename)
        {
            if (args.Length < 6)
            {
                Console.WriteLine("More parameters");
                return;
            }

            string line = "{ " + TimeStamp() + "\"event\":\"Interdiction\", " +
                "\"Success\":\"" + args[1] + "\", " +
                "\"Interdicted\":\"" + args[0] + "\", " +
                "\"IsPlayer\":\"" + args[2] + "\", " +
                "\"CombatRank\":\"" + args[3] + "\", " +
                "\"Faction\":\"" + args[4] + "\", " +
                "\"Power\":\"" + args[5] + "\" }";

            Write(filename, line);
        }

        static void FSDJump(string[] args, string filename)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("More parameters");
                return;
            }

            bool autojump = false;
            double x = double.NaN, y = 0, z = 0;
            string starnameroot = args[0];

            if (!double.TryParse(args[1], out x) || !double.TryParse(args[2], out y) || !double.TryParse(args[3], out z))
            {
                Console.WriteLine("X,y,Z must be numbers");
                return;
            }

            autojump = (args.Length >= 5) ? args[4].Equals("A", StringComparison.InvariantCultureIgnoreCase) : false;

            Console.WriteLine("In file " + filename);

            do
            {
                string starname = starnameroot + "_" + z.ToString("0");

                string line = "{ " + TimeStamp() + "\"event\":\"FSDJump\", \"StarSystem\":\"" + starname +
                "\", \"StarPos\":[" + x.ToString("0.00") + ", " + y.ToString("0.00") + ", " + z.ToString("0.00") +
                "], \"Allegiance\":\"\", \"Economy\":\"$economy_None;\", \"Economy_Localised\":\"None\", \"Government\":\"$government_None;\"," +
                "\"Government_Localised\":\"None\", \"Security\":\"$SYSTEM_SECURITY_low;\", \"Security_Localised\":\"Low Security\"," +
                "\"JumpDist\":10.791, \"FuelUsed\":0.790330, \"FuelLevel\":6.893371 }";

                Write(filename, line);
                z = z + 100;

                if (autojump)
                {
                    while (true)
                    {
                        ConsoleKeyInfo k = Console.ReadKey();

                        if (k.Key == ConsoleKey.Escape)
                        {
                            autojump = false;
                            break;
                        }
                        else if (k.Key == ConsoleKey.Spacebar)
                        {
                            break;
                        }
                    }
                }

            } while (autojump);
        }

        public static string TimeStamp()
        {
            DateTime dt = DateTime.Now.ToUniversalTime();
            return "\"timestamp\":\"" + dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'") + "\", ";
        }

        public static string F(string name, string v)
        {
            DateTime dt = DateTime.Now.ToUniversalTime();
            return "\"" + name + "\":\"" + v + "\", ";
        }

        public static string FF(string name, string v)      // no final comma
        {
            DateTime dt = DateTime.Now.ToUniversalTime();
            return "\"" + name + "\":\"" + v + "\"";
        }


        public static void Write(string filename, string line)
        {
            using (Stream fs = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    sr.WriteLine(line);
                    Console.WriteLine(line);
                }
            }
        }

        public static void EDDBLog(string filename, string groupname, string field, string title)
        {
            using (Stream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                Dictionary<string, string> types = new Dictionary<string, string>();

                using (StreamReader sr = new StreamReader(fs))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (s.Contains(groupname))
                        {
                            string v = GetField(s, field);

                            if (v != null)
                            {
                                types[v] = v;
                                //Console.WriteLine("Star " + v);
                            }
                        }
                    }
                }

                foreach (string s in types.Values)
                {
                    Console.WriteLine(title + " " + s);

                }
            }
        }


        public static string GetField(string s, string f)
        {
            int i = s.IndexOf(f);
            if (i >= 0)
            {
                //Console.WriteLine(s);
                i += f.Length;

                if (s.Substring(i, 5).Equals(":null"))
                    return "Null";

                //Console.WriteLine(s.Substring(i, 20));
                i = s.IndexOf("\"", i);
                //Console.WriteLine(s.Substring(i, 20));
                if (i >= 0)
                {
                    int j = s.IndexOf("\"", i + 1);

                    if (j >= 0)
                    {
                        string ret = s.Substring(i + 1, j - i - 1);
                        return ret;
                    }
                }
            }

            return null;
        }

        public static void Phoneme(string filename, string fileout)
        {
            using (Stream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (Stream fout = new FileStream(fileout, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        using (StreamWriter sw = new StreamWriter(fout, Encoding.Unicode))
                        {
                            string s;
                            int sn = 1;

                            while ((s = sr.ReadLine()) != null)
                            {
                                StringParser sp = new StringParser(s);

                                if (sp.IsCharMoveOn('{'))
                                {
                                    string name = sp.NextQuotedWord();

                                    if (name != null)
                                    {
                                        string[] namelist = name.Split(' ');

                                        if (sp.IsCharMoveOn(',') && sp.Find("{") && sp.IsCharMoveOn('{'))
                                        {
                                            List<string> strings = new List<string>();

                                            while (sp.PeekChar() == '"')
                                            {
                                                strings.Add(sp.NextQuotedWord());
                                                sp.IsCharMoveOn(',');
                                            }

                                            string o = "Static say_tx_star" + (sn++) + " = \"R;\\b(" + name + ")\\b;";
                                            sw.Write(o);

                                            for (int i = 0; i < strings.Count; i++)
                                            {
                                                o = "<phoneme alphabet='ipa' ph = '" + strings[i] + "'>" + namelist[i] + "</phoneme>";
                                                sw.Write(o);
                                            }

                                            sw.Write("\"");
                                            sw.WriteLine();
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        public static void Bindings(string filename)
        {
            using (Stream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                List<string> bindings = new List<string>();
                List<string> say = new List<string>();
                List<string> saydef = new List<string>();

                using (StreamReader sr = new StreamReader(fs))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        int i = s.IndexOf("KEY ", StringComparison.InvariantCultureIgnoreCase);
                        if (i >= 0 && i < 16)
                        {
                            s = s.Substring(i + 4).Trim();
                            if (!bindings.Contains(s))
                                bindings.Add(s);
                        }
                        i = s.IndexOf("Say ", StringComparison.InvariantCultureIgnoreCase);
                        if (i >= 0 && i < 16)
                        {
                            s = s.Substring(i + 4).Trim();
                            if (!say.Contains(s))
                                say.Add(s);
                        }
                        i = s.IndexOf("Static say_", StringComparison.InvariantCultureIgnoreCase);
                        if (i >= 0 && i < 16)
                        {
                            //Console.WriteLine("saw " + s);
                            s = s.Substring(i + 7).Trim();
                            i = s.IndexOf(" ");
                            if (i >= 0)
                                s = s.Substring(0,i);
                            if (!saydef.Contains(s))
                                saydef.Add(s);
                        }
                    }
                }

                bindings.Sort();

                Console.WriteLine("*** Bindings:");
                foreach (string s in bindings)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine("*** Say definitions:");
                foreach (string s in saydef)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine("*** Say commands:");
                foreach (string s in say)
                {
                    Console.WriteLine(s);
                }
            }
        }
    }
}