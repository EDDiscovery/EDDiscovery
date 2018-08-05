﻿using BaseUtils;
using System;
using System.IO;
using System.Linq;

namespace NetLogEntry
{
    class Program
    {
        static void Main(string[] stringargs)
        {
            CommandArgs args = new CommandArgs(stringargs);

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

            if (arg1 == null)
            {
                Help();
                return;
            }

            if (arg1.Equals("StatusMove", StringComparison.InvariantCultureIgnoreCase))
            {
                Status.StatusMove(args);
                return;
            }

            if (arg1.Equals("Status", StringComparison.InvariantCultureIgnoreCase))
            {
                Status.StatusSet(args);
                return;
            }

            if (args.Left < 1)
            {
                Help();
                return;
            }

            if (arg1.Equals("EDDBSTARS", StringComparison.InvariantCultureIgnoreCase))
            {
                EDDB.EDDBLog(args.Next, "\"Star\"", "\"spectral_class\"", "Star class ");
            }
            else if (arg1.Equals("EDDBPLANETS", StringComparison.InvariantCultureIgnoreCase))
            {
                EDDB.EDDBLog(args.Next, "\"Planet\"", "\"type_name\"", "Planet class");
            }
            else if (arg1.Equals("EDDBSTARNAMES", StringComparison.InvariantCultureIgnoreCase))
            {
                EDDB.EDDBLog(args.Next, "\"Star\"", "\"name\"", "Star Name");
            }
            else if (arg1.Equals("voicerecon", StringComparison.InvariantCultureIgnoreCase))
            {
                BindingsFile.Bindings(args.Next);
            }
            else if (arg1.Equals("devicemappings", StringComparison.InvariantCultureIgnoreCase))
            {
                BindingsFile.DeviceMappings(args.Next);
            }
            else if (arg1.Equals("Phoneme", StringComparison.InvariantCultureIgnoreCase))
            {
                if (args.Left >= 1)
                    Speech.Phoneme(args.Next, args.Next);
            }
            else if (arg1.Equals("Corolisships", StringComparison.InvariantCultureIgnoreCase))
            {
                FileInfo[] allFiles = Directory.EnumerateFiles(args.Next, "*.json", SearchOption.AllDirectories).Select(f => new FileInfo(f)).OrderBy(p => p.FullName).ToArray();


                string ret = CorolisData.ProcessShips(allFiles);
                Console.WriteLine(ret);
            }
            else if (arg1.Equals("Corolisship", StringComparison.InvariantCultureIgnoreCase))
            {
                FileInfo[] allFiles = Directory.EnumerateFiles(".", args.Next, SearchOption.AllDirectories).Select(f => new FileInfo(f)).OrderBy(p => p.FullName).ToArray();


                string ret = CorolisData.ProcessShips(allFiles);
                Console.WriteLine(ret);
            }
            else if (arg1.Equals("Corolismodules", StringComparison.InvariantCultureIgnoreCase))
            {
                FileInfo[] allFiles = Directory.EnumerateFiles(args.Next, "*.json", SearchOption.AllDirectories).Select(f => new FileInfo(f)).OrderBy(p => p.FullName).ToArray();

                string ret = CorolisData.ProcessModules(allFiles);
                Console.WriteLine(ret);
            }
            else if (arg1.Equals("Corolismodule", StringComparison.InvariantCultureIgnoreCase))
            {
                FileInfo[] allFiles = Directory.EnumerateFiles(".", args.Next, SearchOption.AllDirectories).Select(f => new FileInfo(f)).OrderBy(p => p.FullName).ToArray();

                string ret = CorolisData.ProcessModules(allFiles);
                Console.WriteLine(ret);
            }
            else if (arg1.Equals("FrontierData", StringComparison.InvariantCultureIgnoreCase))
            {
                string ret = FrontierData.Process(args.Next);
                Console.WriteLine(ret);
            }
            else if (arg1.Equals("scantranslate", StringComparison.InvariantCultureIgnoreCase))
            {
                string path = args.Next;
                FileInfo[] allFiles = Directory.EnumerateFiles(".", path, SearchOption.TopDirectoryOnly).Select(f => new FileInfo(f)).OrderBy(p => p.FullName).ToArray();
                bool combine = false;
                bool showrepeat = false;

                while( args.More )
                {
                    string a = args.Next.ToLowerInvariant();
                    if (a == "combine")
                        combine = true;
                    if (a == "showrepeats" )
                        showrepeat = true;
                }

                string ret = ScanTranslate.Process(allFiles,combine, showrepeat);
                Console.WriteLine(ret);
            }
            else
            {
                Journal.JournalEntry(arg1, args.Next, args, repeatdelay);
            }
        }

        static void Help()
        {
            Console.WriteLine("[-keyrepeat]|[-repeat ms]\n" +
                              "JournalPath CMDRname Options..\n" +
                              "Travel   FSD name x y z (x y z is position as double)\n" +
                              "         FSDTravel name x y z destx desty destz percentint \n" +
                              "         Loc name x y z\n" +
                              "         Docked, Undocked, Touchdown, Liftoff\n" +
                              "         FuelScoop amount total\n" +
                              "         JetConeBoost\n" +
                              "Missions MissionAccepted/MissionCompleted faction victimfaction id\n" +
                              "         MissionRedirected newsystem newstation id\n" +
                              "         Bounty faction reward\n" +
                              "         CommitCrime faction amount\n" +
                              "         Interdiction name success isplayer combatrank faction power\n" +
                              "         FactionKillBond faction victimfaction reward\n" +
                              "         CapShipBond faction victimfaction reward\n" +
                              "Scans    ScanPlanet name\n" +
                              "         ScanStar ScanEarth\n" +
                              "         NavBeaconScan\n" +
                              "Ships    SellShipOnRebuy\n" +
                              "Others   SearchANdRescue MiningRefined\n" +
                              "         RepairDrone CommunityGoal\n" +
                              "         MusicNormal MusicGalMap MusicSysMap\n" +
                              "         Friends Name\n" +
                              "         Died\n" +
                              "         Resurrect cost\n" +
                              "         PowerPlay UnderAttack\n" +
                              "         CargoDepot missionid updatetype(Collect,Deliver,WingUpdate) count total\n" +
                              "         FighterDestroyed FigherRebuilt NpcCrewRank NpcCrewPaidWage LaunchDrone\n" +
                              "         Market ModuleInfo Outfitting Shipyard (use NOFILE after to say don't write the file)\n" +
                              "EDDBSTARS <filename> or EDDBPLANETS or EDDBSTARNAMES for the eddb dump\n" +
                              "Phoneme <filename> <fileout> for EDDI phoneme tx\n" +
                              "Voicerecon <filename>\n" +
                              "DeviceMappings <filename>\n" +
                              "StatusMove <various paras see entry>\n" +
                              "Status <Status flags>...  multiple ones are: supercruise, landed, fight (see code)\n" +
                              "CorolisModules rootfolder - process corolis-data\\modules\\<folder>\n" +
                              "CorolisModule name - process corolis-data\\modules\\<folder>\n" +
                              "CorolisShips rootfolder - process corolis-data\\ships\n" +
                              "CorolisShip name - process corolis-data\\ships\n" +
                              "Coroliseng rootfolder - process corolis-data\\modifications\n" +
                              "FrontierData rootfolder - process cvs file exports of frontier data\n" +
                              "scantranslate filespecwildcard [Combine] [ShowRepeats]- process source files and look for .Tx definitions\n"
                              );

        }


    }
}