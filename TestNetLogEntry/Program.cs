using BaseUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetLogEntry
{
    class Args
    {
        private string[] args;
        private int pos;

        public Args(string[] a)
        {
            args = a;
            pos = 0;
        }

        public Args(Args other)
        {
            args = other.args;
            pos = other.pos;
        }

        public string Next { get { return (pos < args.Length) ? args[pos++] : null; } }
        public string this[int v] { get { int left = args.Length - pos; return (v < left) ? args[pos + v] : null; } }
        public int Left { get { return args.Length - pos; } }
        public void Remove() { if (pos < args.Length) pos++; }
    }

    class Program
    {
        static void Main(string[] stringargs)
        {
            Args args = new Args(stringargs);

            int repeatdelay = 0;

            while (true) // read optional args
            {
                string opt = (args.Left > 0) ? args[0] : null;

                if (opt != null)
                {
                    if (opt.Equals("-keyrepeat", StringComparison.InvariantCultureIgnoreCase))
                    {
                        repeatdelay = -1;
                        args.Remove();
                    }
                    else if (opt.Equals("-repeat", StringComparison.InvariantCultureIgnoreCase) && args.Left >= 1)
                    {
                        args.Remove();
                        if (!int.TryParse(args.Next, out repeatdelay))
                        {
                            Console.WriteLine("Bad repeat delay\n");
                            return;
                        }
                    }
                    else
                        break;
                }
                else
                    break;
            }

            string arg1 = args.Next;

            if (arg1 == null )
            {
                Help();
                return;
            }

            if (arg1.Equals("StatusJSON", StringComparison.InvariantCultureIgnoreCase))
            {
                StatusJSON(args);
                return;
            }

            if ( args.Left < 1)
            {
                Help();
                return;
            }

            if (arg1.Equals("EDDBSTARS", StringComparison.InvariantCultureIgnoreCase))
            {
                EDDBLog(args.Next, "\"Star\"", "\"spectral_class\"", "Star class ");
            }
            else if (arg1.Equals("EDDBPLANETS", StringComparison.InvariantCultureIgnoreCase))
            {
                EDDBLog(args.Next, "\"Planet\"", "\"type_name\"", "Planet class");
            }
            else if (arg1.Equals("EDDBSTARNAMES", StringComparison.InvariantCultureIgnoreCase))
            {
                EDDBLog(args.Next, "\"Star\"", "\"name\"", "Star Name");
            }
            else if (arg1.Equals("voicerecon", StringComparison.InvariantCultureIgnoreCase))
            {
                Bindings(args.Next);
            }
            else if (arg1.Equals("devicemappings", StringComparison.InvariantCultureIgnoreCase))
            {
                DeviceMappings(args.Next);
            }
            else if (arg1.Equals("Phoneme", StringComparison.InvariantCultureIgnoreCase))
            {
                if (args.Left >= 1)
                    Phoneme(args.Next, args.Next);
            }
            else
            {
                JournalEntry(arg1, args.Next, args, repeatdelay);
            }
        }

        static void JournalEntry(string filename, string cmdrname, Args argsentry, int repeatdelay)
        {
            if (argsentry.Left == 0)
            {
                Help();
                Console.WriteLine("** Minimum 3 parameters of filename, cmdrname, journalentrytype");
                return;
            }

            int repeatcount = 0;

            while (true)
            {
                Args args = new Args(argsentry);

                string writetype = args.Next; // min 3

                Random rnd = new Random();

                string lineout = null;      //quick writer

                if (writetype.Equals("FSD", StringComparison.InvariantCultureIgnoreCase))
                    lineout = FSDJump(args, repeatcount);
                else if (writetype.Equals("FSDTravel", StringComparison.InvariantCultureIgnoreCase))
                    lineout = FSDTravel(args);
                else if (writetype.Equals("LOC", StringComparison.InvariantCultureIgnoreCase))
                    lineout = Loc(args);
                else if (writetype.Equals("Interdiction", StringComparison.InvariantCultureIgnoreCase))
                    lineout = Interdiction(args);
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
                else if (writetype.Equals("ScanPlanet", StringComparison.InvariantCultureIgnoreCase) && args.Left >= 1)
                {
                    lineout = "{ " + TimeStamp() + F("event", "Scan") + "\"BodyName\":\"" + args.Next + "x" + repeatcount + "\", \"DistanceFromArrivalLS\":639.245483, \"TidalLock\":true, \"TerraformState\":\"\", \"PlanetClass\":\"Metal rich body\", \"Atmosphere\":\"\", \"AtmosphereType\":\"None\", \"Volcanism\":\"rocky magma volcanism\", \"MassEM\":0.010663, \"Radius\":1163226.500000, \"SurfaceGravity\":3.140944, \"SurfaceTemperature\":1068.794067, \"SurfacePressure\":0.000000, \"Landable\":true, \"Materials\":[ { \"Name\":\"iron\", \"Percent\":36.824127 }, { \"Name\":\"nickel\", \"Percent\":27.852226 }, { \"Name\":\"chromium\", \"Percent\":16.561033 }, { \"Name\":\"zinc\", \"Percent\":10.007420 }, { \"Name\":\"selenium\", \"Percent\":2.584032 }, { \"Name\":\"tin\", \"Percent\":2.449526 }, { \"Name\":\"molybdenum\", \"Percent\":2.404594 }, { \"Name\":\"technetium\", \"Percent\":1.317050 } ], \"SemiMajorAxis\":1532780800.000000, \"Eccentricity\":0.000842, \"OrbitalInclination\":-1.609496, \"Periapsis\":179.381393, \"OrbitalPeriod\":162753.062500, \"RotationPeriod\":162754.531250, \"AxialTilt\":0.033219 }";
                }
                else if (writetype.Equals("ScanStar", StringComparison.InvariantCultureIgnoreCase))
                {
                    lineout = "{ " + TimeStamp() + F("event", "Scan") + "\"BodyName\":\"Merope A" + repeatcount + "\", \"DistanceFromArrivalLS\":0.000000, \"StarType\":\"B\", \"StellarMass\":8.597656, \"Radius\":2854249728.000000, \"AbsoluteMagnitude\":1.023468, \"Age_MY\":182, \"SurfaceTemperature\":23810.000000, \"Luminosity\":\"IV\", \"SemiMajorAxis\":12404761034752.000000, \"Eccentricity\":0.160601, \"OrbitalInclination\":18.126791, \"Periapsis\":49.512009, \"OrbitalPeriod\":54231617536.000000, \"RotationPeriod\":110414.359375, \"AxialTilt\":0.000000 }";
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
                else if (writetype.Equals("Friends", StringComparison.InvariantCultureIgnoreCase) && args.Left >= 1)
                    lineout = "{ " + TimeStamp() + F("event", "Friends") + F("Status", "Online") + FF("Name", args.Next) + " }";
                else if (writetype.Equals("FuelScoop", StringComparison.InvariantCultureIgnoreCase) && args.Left >= 2)
                {
                    string scoop = args.Next;
                    string total = args.Next;
                    lineout = "{ " + TimeStamp() + F("event", "FuelScoop") + F("Scooped", scoop) + FF("Total", total) + " }";
                }
                else if (writetype.Equals("JetConeBoost", StringComparison.InvariantCultureIgnoreCase))
                    lineout = "{ " + TimeStamp() + F("event", "JetConeBoost") + FF("BoostValue", "1.5") + " }";
                else if (writetype.Equals("FighterDestroyed", StringComparison.InvariantCultureIgnoreCase))
                    lineout = "{ " + TimeStamp() + FF("event", "FighterDestroyed") + " }";
                else if (writetype.Equals("FighterRebuilt", StringComparison.InvariantCultureIgnoreCase))
                    lineout = "{ " + TimeStamp() + F("event", "FighterRebuilt") + FF("Loadout", "Fred") + " }";
                else if (writetype.Equals("NpcCrewPaidWage", StringComparison.InvariantCultureIgnoreCase))
                    lineout = "{ " + TimeStamp() + F("event", "NpcCrewPaidWage") + F("NpcCrewId", 1921) + FF("Amount", 10292) + " }";
                else if (writetype.Equals("NpcCrewRank", StringComparison.InvariantCultureIgnoreCase))
                    lineout = "{ " + TimeStamp() + F("event", "NpcCrewRank") + F("NpcCrewId", 1921) + FF("RankCombat", 4) + " }";
                else if (writetype.Equals("LaunchDrone", StringComparison.InvariantCultureIgnoreCase))
                    lineout = "{ " + TimeStamp() + F("event", "LaunchDrone") + FF("Type", "FuelTransfer") + " }";
                else if (writetype.Equals("Market", StringComparison.InvariantCultureIgnoreCase))
                    lineout = Market(Path.GetDirectoryName(filename), args.Next);
                else if (writetype.Equals("ModuleInfo", StringComparison.InvariantCultureIgnoreCase))
                    lineout = ModuleInfo(Path.GetDirectoryName(filename), args.Next);
                else if (writetype.Equals("Outfitting", StringComparison.InvariantCultureIgnoreCase))
                    lineout = Outfitting(Path.GetDirectoryName(filename), args.Next);
                else if (writetype.Equals("Shipyard", StringComparison.InvariantCultureIgnoreCase))
                    lineout = Shipyard(Path.GetDirectoryName(filename), args.Next);
                else if (writetype.Equals("PowerPlay", StringComparison.InvariantCultureIgnoreCase))
                    lineout = "{ " + TimeStamp() + F("event", "PowerPlay") + F("Power", "Fred") + F("Rank", 10) + F("Merits", 10) + F("Votes", 2) + FF("TimePledged", 433024) + " }";
                else if (writetype.Equals("UnderAttack", StringComparison.InvariantCultureIgnoreCase))
                    lineout = "{ " + TimeStamp() + F("event", "UnderAttack") + FF("Target", "Fighter") + " }";
                else if (writetype.Equals("CargoDepot", StringComparison.InvariantCultureIgnoreCase))
                    lineout = CargoDepot(args);
                else
                {
                    Help();
                    Console.WriteLine("** Unrecognised journal type");
                    break;
                }

                if (lineout != null)
                {
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

                    Write(filename, lineout);
                }
                else
                    break;

                if (repeatdelay == -1)
                {
                    ConsoleKeyInfo k = Console.ReadKey();

                    if (k.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }
                else if (repeatdelay > 0)
                {
                    System.Threading.Thread.Sleep(repeatdelay);

                    if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape)
                        break;
                }
                else
                    break;

                repeatcount++;
            }
        }

        static void Help()
        {
            Console.WriteLine("[-keyrepeat]|[-repeat ms]\n" +
                              "JournalPath CMDRname Options..\n" +
                              "     FSD name x y z (x y z is position as double)\n" +
                              "     FSDTravel name x y z destx desty destz percentint \n" +
                              "     Loc name x y z\n" +
                              "     Interdiction name success isplayer combatrank faction power\n" +
                              "     Docked, Undocked, Touchdown, Liftoff, CommitCrime MissionCompleted MissionCompleted2 MiningRefined\n" +
                              "     ScanPlanet name\n" +
                              "     ScanStar NavBeaconScan ScanEarth SellShipOnRebuy SearchANdRescue MissionRedirected\n" +
                              "     RepairDrone CommunityGoal\n" +
                              "     MusicNormal MusicGalMap MusicSysMap\n" +
                              "     Friends Name\n" +
                              "     FuelScoop amount total\n" +
                              "     JetConeBoost\n" +
                              "     PowerPlay UnderAttack\n" +
                              "     CargoDepot missionid updatetype(Collect,Deliver,WingUpdate) count total\n" +
                              "     FighterDestroyed FigherRebuilt NpcCrewRank NpcCrewPaidWage LaunchDrone\n" +
                              "     Market ModuleInfo Outfitting Shipyard (use NOFILE after to say don't write the file)\n" +
                              "EDDBSTARS <filename> or EDDBPLANETS or EDDBSTARNAMES for the eddb dump\n" +
                              "Phoneme <filename> <fileout> for EDDI phoneme tx\n" +
                              "Voicerecon <filename>\n" +
                              "DeviceMappings <filename>\n"+
                              "StatusJSON <various paras see entry>\n"
                              );

        }

        static string Loc(Args args)
        {
            if (args.Left < 4)
            {
                Console.WriteLine("** More parameters");
                return null;
            }

            double x = double.NaN, y = 0, z = 0;
            string starnameroot = args.Next;

            if (!double.TryParse(args.Next, out x) || !double.TryParse(args.Next, out y) || !double.TryParse(args.Next, out z))
            {
                Console.WriteLine("X,y,Z must be numbers");
                return null;
            }

            return "{ " + TimeStamp() + "\"event\":\"Location\", " +
                "\"StarSystem\":\"" + starnameroot +
                "\", \"StarPos\":[" + x.ToString("0.000000") + ", " + y.ToString("0.000000") + ", " + z.ToString("0.000000") +
                "], \"Allegiance\":\"\", \"Economy\":\"$economy_None;\", \"Economy_Localised\":\"None\", \"Government\":\"$government_None;\", \"Government_Localised\":\"None\", \"Security\":\"$SYSTEM_SECURITY_low;\", \"Security_Localised\":\"Low Security\" }";
        }

        //                                  "Options: Interdiction Loc name success isplayer combatrank faction power\n" +
        static string Interdiction(Args args)
        {
            if (args.Left < 6)
            {
                Console.WriteLine("** More parameters");
                return null;
            }

            return "{ " + TimeStamp() + "\"event\":\"Interdiction\", " +
                "\"Success\":\"" + args[1] + "\", " +
                "\"Interdicted\":\"" + args[0] + "\", " +
                "\"IsPlayer\":\"" + args[2] + "\", " +
                "\"CombatRank\":\"" + args[3] + "\", " +
                "\"Faction\":\"" + args[4] + "\", " +
                "\"Power\":\"" + args[5] + "\" }";
        }

        static string FSDJump(Args args, int repeatcount)
        {
            if (args.Left < 4)
            {
                Console.WriteLine("** More parameters: file cmdrname fsd x y z");
                return null;
            }

            double x = double.NaN, y = 0, z = 0;
            string starnameroot = args.Next;

            if (!double.TryParse(args.Next, out x) || !double.TryParse(args.Next, out y) || !double.TryParse(args.Next, out z))
            {
                Console.WriteLine("** X,Y,Z must be numbers");
                return null;
            }

            z = z + 100 * repeatcount;

            string starname = starnameroot + ((z > 0) ? "_" + z.ToString("0") : "");

            return "{ " + TimeStamp() + "\"event\":\"FSDJump\", \"StarSystem\":\"" + starname +
            "\", \"StarPos\":[" + x.ToString("0.000000") + ", " + y.ToString("0.000000") + ", " + z.ToString("0.000000") +
            "], \"Allegiance\":\"\", \"Economy\":\"$economy_None;\", \"Economy_Localised\":\"None\", \"Government\":\"$government_None;\"," +
            "\"Government_Localised\":\"None\", \"Security\":\"$SYSTEM_SECURITY_low;\", \"Security_Localised\":\"Low Security\"," +
            "\"JumpDist\":10.791, \"FuelUsed\":0.790330, \"FuelLevel\":6.893371 }";
        }

        static string FSDTravel(Args args)
        {
            if (args.Left < 8)
            {
                Console.WriteLine("** More parameters");
                return null;
            }

            double x = double.NaN, y = 0, z = 0, dx=0,dy=0,dz=0;
            double percent = 0;
            string starnameroot = args.Next;

            if (!double.TryParse(args.Next, out x) || !double.TryParse(args.Next, out y) || !double.TryParse(args.Next, out z) ||
                !double.TryParse(args.Next, out dx) || !double.TryParse(args.Next, out dy) || !double.TryParse(args.Next, out dz) || 
                !double.TryParse(args.Next,out percent) )
            {
                Console.WriteLine("** X,Y,Z,dx,dy,dz,percent must be numbers");
                return null;
            }

            Console.WriteLine("{0} {1} {2}", dx, dy, dz);

            x = (dx - x) * percent / 100.0 + x;
            y = (dy - y) * percent / 100.0 + y;
            z = (dz - z) * percent / 100.0 + z;

            string starname = starnameroot + percent.ToString("0");

            return "{ " + TimeStamp() + "\"event\":\"FSDJump\", \"StarSystem\":\"" + starname +
            "\", \"StarPos\":[" + x.ToString("0.000000") + ", " + y.ToString("0.000000") + ", " + z.ToString("0.000000") +
            "], \"Allegiance\":\"\", \"Economy\":\"$economy_None;\", \"Economy_Localised\":\"None\", \"Government\":\"$government_None;\"," +
            "\"Government_Localised\":\"None\", \"Security\":\"$SYSTEM_SECURITY_low;\", \"Security_Localised\":\"Low Security\"," +
            "\"JumpDist\":10.791, \"FuelUsed\":0.790330, \"FuelLevel\":6.893371 }";
        }

        static string Market(string mpath, string opt)
        {
            string mline = "{ " + TimeStamp() + F("event", "Market") + F("MarketID", 12345678) + F("StationName", "Columbus") + FF("StarSystem", "Sol");
            string market = mline + ", " + TestNetLogEntry.Properties.Resources.Market;

            if (opt == null || opt.Equals("NOFILE", StringComparison.InvariantCultureIgnoreCase) == false)
                File.WriteAllText(Path.Combine(mpath, "Market.json"), market);

            return mline + " }";
        }

        static string Outfitting(string mpath, string opt)
        {
            //{ "timestamp":"2018-01-28T23:45:39Z", "event":"Outfitting", "MarketID":3229009408, "StationName":"Mourelle Gateway", "StarSystem":"G 65-9",
            string jline = "{ " + TimeStamp() + F("event", "Outfitting") + F("MarketID", 12345678) + F("StationName", "Columbus") + FF("StarSystem", "Sol");
            string fline = jline + ", " + TestNetLogEntry.Properties.Resources.Outfitting;

            if (opt == null || opt.Equals("NOFILE", StringComparison.InvariantCultureIgnoreCase) == false)
                File.WriteAllText(Path.Combine(mpath, "Outfitting.json"), fline);

            return jline + " }";
        }

        static string CargoDepot(Args args)
        {
            try
            {
                int missid = int.Parse(args.Next);
                string type = args.Next;
                int countcol = int.Parse(args.Next);
                int countdel = int.Parse(args.Next);
                int total = int.Parse(args.Next);

                return "{ " + TimeStamp() + F("event", "CargoDepot") + F("MissionID", missid) + F("UpdateType", type) +
                                F("StartMarketID", 12) + F("EndMarketID", 13) +
                                F("ItemsCollected", countcol) +
                                F("ItemsDelivered", countdel) +
                                F("TotalItemsToDeliver", total) +
                                F("Progress", (double)countcol / (double)total, true) + " }";
            }
            catch
            {
                Console.WriteLine("missionid type col del total");
                return null;
            }
        }

        static string Shipyard(string mpath, string opt)
        {
            // { "timestamp":"2018-01-26T03:47:33Z", "event":"Shipyard", "MarketID":128004608, "StationName":"Vonarburg Co-operative", "StarSystem":"Wyrd",

            string jline = "{ " + TimeStamp() + F("event", "Shipyard") + F("MarketID", 12345678) + F("StationName", "Columbus") + FF("StarSystem", "Sol");
            string fline = jline + ", " + TestNetLogEntry.Properties.Resources.Shipyard;

            if (opt == null || opt.Equals("NOFILE", StringComparison.InvariantCultureIgnoreCase) == false)
                File.WriteAllText(Path.Combine(mpath, "Shipyard.json"), fline);

            return jline + " }";
        }


        static string ModuleInfo(string mpath, string opt)
        {
            string mline = "{ " + TimeStamp() + FF("event", "ModuleInfo");
            string market = mline + ", " + TestNetLogEntry.Properties.Resources.ModulesInfo;

            if (opt == null || opt.Equals("NOFILE", StringComparison.InvariantCultureIgnoreCase) == false)
                File.WriteAllText(Path.Combine(mpath, "ModulesInfo.json"), market);     // note the plural

            return mline + " }";
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
                                s = s.Substring(0, i);
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


        public static void DeviceMappings(string filename)
        {
            try
            {
                XElement bindings = XElement.Load(filename);

                System.Diagnostics.Debug.WriteLine("Top " + bindings.NodeType + " " + bindings.Name);

                Console.WriteLine("Dictionary<Tuple<int, int>, string> ctrls = new Dictionary<Tuple<int, int>, string>()" + Environment.NewLine + "{" + Environment.NewLine);

                foreach (XElement x in bindings.Elements())
                {
                    string ctrltype = x.Name.LocalName;
                    List<Tuple<int, int>> pv = new List<Tuple<int, int>>();

                    int pid = 0;
                    int vid = 0;

                    int.TryParse(x.Element("PID").Value, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out pid);
                    int.TryParse(x.Element("VID").Value, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out vid);

                    pv.Add(new Tuple<int, int>(pid, vid));

                    foreach (XElement y in x.Elements())
                    {
                        if (y.Name.LocalName.Equals("Alternative"))
                        {
                            int.TryParse(y.Element("PID").Value, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out pid);
                            int.TryParse(y.Element("VID").Value, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out vid);

                            pv.Add(new Tuple<int, int>(pid, vid));
                        }
                    }

                    System.Diagnostics.Debug.WriteLine("Ctrl " + ctrltype);
                    foreach (Tuple<int, int> v in pv)
                        System.Diagnostics.Debug.WriteLine("  " + v.Item1.ToString("x") + " " + v.Item2.ToString("x"));

                    foreach (Tuple<int, int> v in pv)
                    {
                        System.Diagnostics.Debug.WriteLine("  " + v.Item1.ToString("x") + " " + v.Item2.ToString("x"));
                        Console.WriteLine("     {  new Tuple<int,int>(0x" + v.Item1.ToString("X") + ", 0x" + v.Item2.ToString("X") + "), \"" + ctrltype + "\" },");
                    }
                }

                Console.WriteLine("};");
            }

            catch
            {

            }

            //example..
            Dictionary<Tuple<int, int>, string> ct2rls = new Dictionary<Tuple<int, int>, string>()
            {
                { new Tuple<int,int>(1,1), "Fred" },
            };


        }

        public static void StatusJSON(Args args)
        {
            long flags = 0;

            double latitude = 0;
            double longitude = 0;
            double latstep = 0;
            double longstep = 0;
            double heading = 0;
            double headstep = 1;
            int steptime = 100;

            if (!double.TryParse(args.Next, out latitude) || !double.TryParse(args.Next, out longitude) ||
                !double.TryParse(args.Next, out latstep) || !double.TryParse(args.Next, out longstep) ||
                !double.TryParse(args.Next, out heading) || !double.TryParse(args.Next, out headstep) ||
                !int.TryParse(args.Next, out steptime))
            {
                Console.WriteLine("** More/Wrong parameters: statusjson lat long latstep lonstep heading headstep steptimems");
                return;
            }

            while (true)
            {
                //{ "timestamp":"2018-03-01T21:51:36Z", "event":"Status", "Flags":18874376, 
                //"Pips":[4,8,0], "FireGroup":1, "GuiFocus":0, "Latitude":-18.978821, "Longitude":-123.642052, "Heading":308, "Altitude":20016 }

                string j = "{ " + TimeStamp() + F("event", "Status") + F("Flags", flags) + F("Pips",new int[] { 4,8,0}) + 
                            F("FireGroup",1) + F("GuiFocus",0) + F("Latitude",latitude) + F("Longitude",longitude) + F("Heading",heading) + FF("Altitude",20) 
                            + "}";

                File.WriteAllText("Status.json", j);
                System.Threading.Thread.Sleep(steptime);

                if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    break;
                }

                latitude += latstep;
                longitude = longitude + longstep;
                heading = (heading + headstep) % 360;

            }
        }

        #region Helpers for journal writing

        public static string TimeStamp()
        {
            DateTime dt = DateTime.Now.ToUniversalTime();
            return "\"timestamp\":\"" + dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'") + "\", ";
        }

        public static string F(string name, long v , bool term = false)
        {
            return "\"" + name + "\":" + v + (term ? " " : ", ");
        }

        public static string F(string name, double v, bool term = false)
        {
            return "\"" + name + "\":" + v.ToString("0.######") + (term ? " " : "\", ");
        }

        public static string F(string name, string v , bool term = false)
        {
            return "\"" + name + "\":\"" + v + (term ? "\" " : "\", ");
        }

        public static string F(string name, int[] array, bool end = false)
        {
            string s = "";
            foreach( int a in array )
            {
                if (s.Length > 0)
                    s += ", ";

                s += a.ToString();
            }

            return "\"" + name + "\":[" + s + "]" + (end? "" : ", ");
        }

        public static string FF(string name, string v)      // no final comma
        {
            return F(name, v, true);
        }

        public static string FF(string name, long v)      // no final comma
        {
            return F(name, v, true);
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

        #endregion
    }
}