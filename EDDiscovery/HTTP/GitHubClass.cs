﻿/*
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
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery.HTTP;
using EDDiscovery2;
using EDDiscovery2.HTTP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace EDDiscovery.HTTP
{
    public class GitHubClass : HttpCom
    {
        //public string commanderName;
        //private readonly string fromSoftwareVersion;
        // assemblyFullName = Assembly.GetExecutingAssembly().FullName;
        //fromSoftwareVersion = assemblyFullName.Split(',')[1].Split('=')[1];
        //commanderName = EDDConfig.Instance.CurrentCommander.EdsmName;

        private readonly string githubServer = "https://api.github.com/repos/EDDiscovery/EDDiscovery/";

        public delegate void LogLine(string text);
        LogLine logger = null;

        public GitHubClass(LogLine lg = null )
        {
            _serverAddress = githubServer;
            logger = lg;
        }

        public JArray GetAllReleases()
        {

            try
            {
                HttpWebRequest request = WebRequest.Create("https://api.github.com/repos/EDDiscovery/EDDiscovery/releases") as HttpWebRequest;
                request.UserAgent = "TestApp";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string content1 = reader.ReadToEnd();
                    JArray ja = JArray.Parse(content1);
                    return ja;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception: {ex.Message}");
                Trace.WriteLine($"ETrace: {ex.StackTrace}");
                return null;
            }

        }

        public GitHubRelease GetLatestRelease()
        {

            try
            {
                HttpWebRequest request = WebRequest.Create("https://api.github.com/repos/EDDiscovery/EDDiscovery/releases/latest") as HttpWebRequest;
                request.UserAgent = "TestApp";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string content1 = reader.ReadToEnd();
                    JObject ja = JObject.Parse(content1);

                    if (ja != null)
                    {
                        GitHubRelease rel = new GitHubRelease(ja);
                        return rel;
                    }
                    else
                        return null; ;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception: {ex.Message}");
                Trace.WriteLine($"ETrace: {ex.StackTrace}");
                return null;
            }

        }


        public List<GitHubFile> GetDataFiles(string gitdir)
        {
            List<GitHubFile> files = new List<GitHubFile>();
            try
            {
                HttpWebRequest request = WebRequest.Create("https://api.github.com/repos/EDDiscovery/EDDiscoveryData/contents/" + gitdir) as HttpWebRequest;
                request.UserAgent = "TestApp";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string content1 = reader.ReadToEnd();
                    JArray ja = JArray.Parse(content1);

                    if (ja != null)
                    {
                        foreach (JObject jo in ja)
                        {
                            GitHubFile file = new GitHubFile(jo);
                            files.Add(file);
                        }
                        return files;
                    }
                    else
                        return null; ;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception: {ex.Message}");
                Trace.WriteLine($"ETrace: {ex.StackTrace}");
                return null;
            }
        }

        public bool DownloadFiles(List<GitHubFile> files, string DestinationDir)
        {
            if (files == null)
                return true;

            foreach (var file in files)
                if (!DownloadFile(file, DestinationDir))
                    return false;

            return true;
        }

        public bool DownloadNeeded(GitHubFile file, string DestinationDir)
        {
            string destFile = Path.Combine(DestinationDir, file.Name);

            if (!File.Exists(destFile))
                return true;
            else
            {
                // Calculate sha
                string sha = CalcSha1(destFile).ToLower();

                if (sha.Equals(file.sha))
                    return false;
            }

            return true;
        }

        static public string CalcSha1(string[] filenames)
        {
            long length = 0;

            foreach (string filename in filenames)
            {
                FileInfo fi = new FileInfo(filename);
                if (fi == null)
                    return "";

                length += fi.Length;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] header = Encoding.UTF8.GetBytes("blob " + (int)length + "\0");
                ms.Write(header, 0, header.Length);

                foreach (string filename in filenames)
                {
                    using (FileStream fs = new FileStream(filename, FileMode.Open))
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                            ms.Write(br.ReadBytes((int)fs.Length), 0, (int)fs.Length);
                    }
                }

                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] hash = sha1.ComputeHash(ms.ToArray());
                    StringBuilder formatted = new StringBuilder(2 * hash.Length);
                    foreach (byte b in hash)
                    {
                        formatted.AppendFormat("{0:X2}", b);
                    }

                    return formatted.ToNullSafeString();
                }
            }
        }

        static public string CalcSha1(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            using (BinaryReader br = new BinaryReader(fs))
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] header = Encoding.UTF8.GetBytes("blob " + fs.Length + "\0");
                ms.Write(header, 0, header.Length);

                ms.Write(br.ReadBytes((int)fs.Length), 0, (int)fs.Length);

                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] hash = sha1.ComputeHash(ms.ToArray());
                    StringBuilder formatted = new StringBuilder(2 * hash.Length);
                    foreach (byte b in hash)
                    {
                        formatted.AppendFormat("{0:X2}", b);
                    }

                    return formatted.ToNullSafeString();
                }
            }
        }


        public bool DownloadFile(GitHubFile file, string DestinationDir)
        {
            // Check if the file is new/updated first
            if (!DownloadNeeded(file, DestinationDir))
                return true;

            // download.....
            try
            {
                if (logger != null)
                    logger("Download github file " + file.Name);

                WriteLog("Download github file " + file.Name, "");
                string destFile = Path.Combine(DestinationDir, file.Name);

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(file.DownloadURL, destFile);
                }

                return true;
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                    System.Diagnostics.Trace.WriteLine("WebException : " + ex.Message);
                    System.Diagnostics.Trace.WriteLine("Response code : " + httpResponse.StatusCode);
                    System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                    WriteLog("WebException" + ex.Message, "");
                    WriteLog($"HTTP Error code: {httpResponse.StatusCode}", "");

                    return false;
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger("GitHub DownloadFile Exception" + ex.Message);

                WriteLog("GitHub DownloadFile Exception" + ex.Message, "");
                return false;
            }
        }
    }
}
