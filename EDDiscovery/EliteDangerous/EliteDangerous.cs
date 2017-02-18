﻿/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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



    public class EliteDangerousClass
    {
        static public string EDDirectory;
        static public bool EDRunning = false;
        static public bool checkedfordefaultfolder = false;

        static public bool CheckED()
        {
            string EDFileName = null;

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

                    if (EDDirectory == null || EDDirectory.Equals(""))
                    {
                        if (!checkedfordefaultfolder)
                        {
                            checkedfordefaultfolder = true;                 // do it once, but no need to keep on doing it.. only this class can set it once the process starts
                            EDDirectory = EDDConfig.UserPaths.EDDirectory;
                        }
                    }
                }
                else
                {
                    string processFilename = null;
                    try
                    {
                        int id = processes[0].Id;
                        processFilename = GetMainModuleFilepath(id);        // may return null if id not found (seen this)

                        if (processFilename != null)
                            EDFileName = processFilename;
                    }
                    catch (Win32Exception)
                    {
                    }

                    if (EDFileName != null)                                 // if found..
                    {
                        string newfolder = Path.GetDirectoryName(EDFileName);

                        if ( newfolder != null && !newfolder.Equals(EDDirectory) )
                        {
                            EDDirectory = newfolder;
                            EDDConfig.UserPaths.EDDirectory = EDDirectory;
                            EDDConfig.UserPaths.Save();
                        }

                        EDRunning = true;
                    }
                }

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
                if (searcher != null)           // seen it return null
                {
                    using (var results = searcher.Get())
                    {
                        if (results != null)
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
                }
            }
            return null;
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



    }
}
