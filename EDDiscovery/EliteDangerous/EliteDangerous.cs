using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;
using EDDiscovery.DB;
using Newtonsoft.Json.Linq;
using EDDiscovery2.EDDB;

namespace EDDiscovery2
{
    public enum EDGovernment
    {
        Unknown = 0,
        Anarchy=1,
        Communism = 2,
        Confederacy =3,
        Corporate = 4,
        Cooperative = 5,
        Democracy = 6,
        Dictatorship, 
        Feudal,
        Imperial,
        Patronage,
        Colony,
        Prison_Colony,
        Theocracy,
        None,

    }

    public enum EDAllegiance
    {
        Unknown = 0,
        Alliance = 1,
        Anarchy =2,
        Empire = 3,
        Federation = 4,
        Independent = 5,
        None = 6,
    }

    public enum EDState
    {
        Unknown = 0,
        None = 1,
        Boom,
        Bust,
        Civil_Unrest,
        Civil_War,
        Expansion,
        Lockdown,
        Outbreak,
        War,
    }

    public enum EDSecurity
    {
        Unknown = 0,
        Low,
        Medium,
        High,
    }

    public enum EDStationType
    {
        Unknown = 0,
        Civilian_Outpost = 1,
        Commercial_Outpost	=2,
        Coriolis_Starport = 3,
        Industrial_Outpost	=4,
        Military_Outpost	=5,
        Mining_Outpost	 = 6,
        Ocellus_Starport	=7,
        Orbis_Starport	 = 8,
        Scientific_Outpost	=9,
        Unsanctioned_Outpost	= 10,
        Unknown_Outpost	= 11,
        Unknown_Starport = 12,
    }

    public enum EDEconomy
    {
        Unknown = 0,
        Agriculture  =1,
        Extraction = 2,
        High_Tech	=3,
        Industrial =4,
        Military = 5,
        Refinery =6,
        Service = 7,
        Terraforming = 8,
        Tourism = 9,
        None = 10,

    }



    public class EliteDangerous
    {
        static public string EDFileName;
        static public string EDLaunchFileName;
        static public string EDDirectory;
        //static public string EDVersion;
        static public bool EDRunning = false;
        static public bool EDLaunchRunning = false;
        static public bool Beta = false;

        static public bool CheckED()
        {
            try
            {
                Process[] processes32 = Process.GetProcessesByName("EliteDangerous32");
                Process[] processes64 = Process.GetProcessesByName("EliteDangerous64");

                Process[] processes = processes32;

                if (processes == null || processes32.Length == 0)
                    processes = processes64;

                if (processes == null)
                {
                    EDRunning = false;
                }
                else if (processes.Length == 0)
                {
                    EDRunning = false;

                    SQLiteDBClass db = new SQLiteDBClass();

                    if (EDDirectory == null || EDDirectory.Equals(""))
                        EDDirectory = db.GetSettingString("EDDirectory", "");
                }
                else
                {
                    string processFilename = null;
                    try
                    {
                        int id = processes[0].Id;
                        processFilename = GetMainModuleFilepath(id);
                        EDFileName = processFilename;
                        //processFilename = processes[0].MainModule.FileName;
                    }
                    catch (Win32Exception)
                    {
                    }

                    EDDirectory = Path.GetDirectoryName(EDFileName);
                    if (EDDirectory != null)
                    {
                        SQLiteDBClass db = new SQLiteDBClass();
                        if (EDDirectory.Contains("PUBLIC_TEST_SERVER")) // BETA
                        {
                            db.PutSettingString("EDDirectoryBeta", EDDirectory);
                            Beta = true;
                        }
                        else
                        {
                            Beta = false;
                            db.PutSettingString("EDDirectory", EDDirectory);
                        }
                    }


                    EDRunning = true;

                }

                //processes = Process.GetProcessesByName("EDLaunch");

                //if (processes == null)
                //{
                //    EDLaunchRunning = false;
                //}
                //else if (processes.Length == 0)
                //{
                //    EDLaunchRunning = false;
                //}
                //else
                //{

                //    EDLaunchFileName = ProcessExecutablePath(processes[0]);
                //    EDLaunchRunning = true;

                //}

                return EDRunning;
            }
            catch (Exception)
            {
            }
            return false;
        }

        private static  string GetMainModuleFilepath(int processId)
        {
            string wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process WHERE ProcessId = " + processId;
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            {
                using (var results = searcher.Get())
                {
                    foreach (ManagementObject mo in results)
                    {
                        if (mo != null)
                        {
                            return (string)mo["ExecutablePath"];
                        }
                    }
                }
            }
            return null;
        }

        static public bool CheckStationLogging()
        {
            if (EDDirectory == null)
                return true;

            if (EDDirectory.Equals(""))
                return true;


            if (!Directory.Exists(EDDirectory)) // For safety.
                return true;

            try
            {


                string filename = Path.Combine(EDDirectory, "AppConfig.xml");

                using (Stream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = line.Trim().Replace(" ", "");
                            if (line.Contains("VerboseLogging=\"1\""))
                            {
                                return true;
                            }
                        }
                    }
                }



                // Check ED local filename too
                filename = Path.Combine(EDDirectory, "AppConfigLocal.xml");
                if (!File.Exists(filename))
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(filename))
                        {
                            writer.WriteLine("<AppConfig>");
                            writer.WriteLine(" <Network");
                            writer.WriteLine("  VerboseLogging=\"1\">");
                            writer.WriteLine(" </Network>");
                            writer.WriteLine("</AppConfig>");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine("CheckStationLogging exception: " + ex.Message);
                    }
                }


                if (File.Exists(filename))
                {
                    using (Stream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                line = line.Trim().Replace(" ", "");
                                if (line.Contains("VerboseLogging=\"1\""))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }


                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                return false;
            }
        }




        static private string ProcessExecutablePath(Process process)
        {

                string query = "SELECT ExecutablePath, ProcessID FROM Win32_Process";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                foreach (ManagementObject item in searcher.Get())
                {
                    object id = item["ProcessID"];
                    object path = item["ExecutablePath"];

                    if (path != null && id.ToString() == process.Id.ToString())
                    {
                        return path.ToString();
                    }
                }


            return "";
        }



        static public EDGovernment Government2ID(JToken jt)
        {

            if (jt == null)
                return EDGovernment.Unknown;


            if (jt.Type != JTokenType.String)
                return EDGovernment.Unknown;


            string str = jt.Value<string>();

            foreach (var govid in Enum.GetValues(typeof(EDGovernment)))
            {
                if (str.Equals(govid.ToString().Replace("_", " ")))
                    return (EDGovernment)govid;
                //System.Console.WriteLine(govid.ToString());
            }

            return EDGovernment.Unknown;
        }

        static public EDAllegiance Allegiance2ID(JToken jt)
        {

            if (jt == null)
                return EDAllegiance.Unknown;


            if (jt.Type != JTokenType.String)
                return EDAllegiance.Unknown;


            string str = jt.Value<string>();

            foreach (var govid in Enum.GetValues(typeof(EDAllegiance)))
            {
                if (str.Equals(govid.ToString().Replace("_", " ")))
                    return (EDAllegiance)govid;
                //System.Console.WriteLine(govid.ToString());
            }

            return EDAllegiance.Unknown;
        }


        static public EDState EDState2ID(JToken jt)
        {

            if (jt == null)
                return EDState.Unknown;


            if (jt.Type != JTokenType.String)
                return EDState.Unknown;


            string str = jt.Value<string>();

            foreach (var govid in Enum.GetValues(typeof(EDState)))
            {
                if (str.Equals(govid.ToString().Replace("_", " ")))
                    return (EDState)govid;
                //System.Console.WriteLine(govid.ToString());
            }

            return EDState.Unknown;
        }


        static public EDSecurity EDSecurity2ID(JToken jt)
        {

            if (jt == null)
                return EDSecurity.Unknown;


            if (jt.Type != JTokenType.String)
                return EDSecurity.Unknown;


            string str = jt.Value<string>();

            foreach (var govid in Enum.GetValues(typeof(EDSecurity)))
            {
                if (str.Equals(govid.ToString().Replace("_", " ")))
                    return (EDSecurity)govid;
                //System.Console.WriteLine(govid.ToString());
            }

            return EDSecurity.Unknown;
        }

        static public EDStationType EDStationType2ID(JToken jt)
        {

            if (jt == null)
                return EDStationType.Unknown;


            if (jt.Type != JTokenType.String)
                return EDStationType.Unknown;


            string str = jt.Value<string>();

            foreach (var govid in Enum.GetValues(typeof(EDStationType)))
            {
                if (str.Equals(govid.ToString().Replace("_", " ")))
                    return (EDStationType)govid;
                //System.Console.WriteLine(govid.ToString());
            }

            return EDStationType.Unknown;
        }

        static public EDEconomy EDEconomy2ID(JToken jt)
        {

            if (jt == null)
                return EDEconomy.Unknown;


            if (jt.Type != JTokenType.String)
                return EDEconomy.Unknown;


            string str = jt.Value<string>();

            return String2Economy(str);
        }

        static public EDEconomy String2Economy(string str)
        {
            foreach (var govid in Enum.GetValues(typeof(EDEconomy)))
            {
                if (str.Equals(govid.ToString().Replace("___", ".").Replace("__", "-").Replace("_", " ")))
                    return (EDEconomy)govid;
                //System.Console.WriteLine(govid.ToString());
            }

            return EDEconomy.Unknown;
        }


        static public List<EDEconomy> EDEconomies2ID(JArray ja)
        {
            List<EDEconomy> economies = new List<EDEconomy>();

            if (ja == null)
                return null;

            for (int ii = 0; ii < ja.Count; ii++)
            {
                string ecstr = ja[ii].Value<string>();
                economies.Add(String2Economy(ecstr));

            }
            return economies;
        }




        static public Commodity String2Commodity(string str)
        {

            var v = EDDBClass.commodities.FirstOrDefault(m => m.Value.name == str).Value;

            return v;
        }

        static public List<Commodity> EDCommodities2ID(JArray ja)
        {
            List<Commodity> commodity = new List<Commodity>();

            if (ja == null)
                return null;

            for (int ii = 0; ii < ja.Count; ii++)
            {
                string ecstr = ja[ii].Value<string>();

                commodity.Add(String2Commodity(ecstr));

            }
            return commodity;
        }

       




    }
}
