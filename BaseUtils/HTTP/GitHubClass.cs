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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;


namespace BaseUtils
{
    public class GitHubClass : HttpCom
    {
        public delegate void LogLine(string text);
        LogLine logger = null;

        public GitHubClass(string server, LogLine lg = null )
        {
            httpserveraddress = server; 
            logger = lg;
        }

        public JArray GetAllReleases()
        {

            try
            {
                HttpWebRequest request = WebRequest.Create(httpserveraddress + "releases") as HttpWebRequest;
                request.UserAgent = BrowserInfo.UserAgent;
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
                HttpWebRequest request = WebRequest.Create(httpserveraddress + "releases/latest") as HttpWebRequest;
                request.UserAgent = BrowserInfo.UserAgent;
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

        // NULL if directory not found.  Empty list if files not there
        public List<GitHubFile> ReadDirectory(string gitdir)
        {
            List<GitHubFile> files = new List<GitHubFile>();
            try
            {
                HttpWebRequest request = WebRequest.Create(httpserveraddress + "contents/" + gitdir) as HttpWebRequest;
                request.UserAgent = BrowserInfo.UserAgent;
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
                        return null;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception: {ex.Message}");
                Trace.WriteLine($"ETrace: {ex.StackTrace}");
                return null;
            }
        }

        // DOWNLOAD to folder from git folder with pattern match

        public bool Download(string downloadfolder, string gitdir, string wildcardmatch, bool cleanfolder = true)
        {
            if (cleanfolder)
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(downloadfolder);
                    foreach (FileInfo file in di.GetFiles())
                        file.Delete();
                }
                catch { }   // paranoia
            }

            List<BaseUtils.GitHubFile> files = ReadDirectory(gitdir);       // may except if not there..

            if (files != null)
            {
                files = (from f in files where f.Name.WildCardMatch(wildcardmatch) select f).ToList();
                return DownloadFiles(files, downloadfolder);
            }
            else
                return false;
        }

        // DOWNLOAD to folder with list of files

        public bool Download(string downloadfolder, string gitdir, List<string> matches)
        {
            List<BaseUtils.GitHubFile> files = ReadDirectory(gitdir);

            if (files != null)
            {
                files = (from f in files where matches.Contains(f.Name, StringComparer.InvariantCultureIgnoreCase) select f).ToList();
                return DownloadFiles(files, downloadfolder);
            }
            else
                return false;
        }

        // download with github list

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
                string sha = SHA.CalcSha1(destFile).ToLowerInvariant();

                if (sha.Equals(file.sha))
                    return false;
            }

            return true;
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
