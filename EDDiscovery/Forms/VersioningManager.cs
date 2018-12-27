/*
 * Copyright © 2017 EDDiscovery development team
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
using System.IO;
using System.Linq;
using BaseUtils;

namespace EDDiscovery.Versions
{
    public class VersioningManager
    {
        public enum ItemState
        {
            None,
            LocalOnly,
            EDOutOfDate,
            NotPresent,
            UpToDate,
            OutOfDate,
            EDTooOld,
        }

        public class DownloadItem
        {
            public bool HasDownloadedCopy { get { return downloadedfilename != null;  } }
            public string LongDownloadedDescription { get { return downloadedvars != null && downloadedvars.Exists("LongDescription") ? downloadedvars["LongDescription"] : ""; } }

            public string downloadedpath;           // where its stored on disk to be installed from
            public string downloadedfilename;       // filename
            public int[] downloadedversion;
            public Variables downloadedvars;
            public string downloadedserver;         // where to get any additional files from
            public string downloadedserverpath;     // and its path

            public bool HasLocalCopy { get { return localfound; } }
            public string LongLocalDescription { get { return localvars != null && localvars.Exists("LongDescription") ? localvars["LongDescription"] : ""; } }
            public string ShortLocalDescription { get { return localvars != null && localvars.Exists("ShortDescription") ? localvars["ShortDescription"] : ""; } }

            public bool localfound;             // if scanned locally
            public string localfilename;        // always set
            public string localpath;            // always set
            public int[] localversion;          // may be null if file does not have version
            public bool localmodified;          // if local file exists, sha comparison
            public Variables localvars;    //  null, or set if local has variables
            public bool? localenable;       // null, or set if local has variables and a Enable flag

            public ItemState state;

            public string itemname;
            public string itemtype;
        };

        public List<DownloadItem> DownloadItems { private set; get; } = new List<DownloadItem>();

        public VersioningManager()
        {
        }

        public void ReadLocalFiles(string appfolder, string subfolder, string filename , string defaultitemtype)       // DONE FIRST
        {
            string installfolder = System.IO.Path.Combine(appfolder, subfolder);
            if (!System.IO.Directory.Exists(installfolder))
                System.IO.Directory.CreateDirectory(installfolder);

            FileInfo[] allFiles = Directory.EnumerateFiles(installfolder, filename, SearchOption.TopDirectoryOnly).Select(f => new FileInfo(f)).OrderBy(p => p.Name).ToArray();

            foreach (FileInfo f in allFiles)
            {
                try
                {
                    DownloadItem it = new DownloadItem();

                    it.localfound = true;

                    it.itemname = Path.GetFileNameWithoutExtension(f.FullName);
                    it.itemtype = defaultitemtype;

                    it.localfilename = f.FullName;
                    it.localpath = installfolder;

                    it.state = ItemState.LocalOnly;
                    it.localvars = ReadVarsFromFile(f.FullName , out it.localenable);

                    if (it.localvars != null)       // always reads some vars as long as file is there..
                    {
                        if (it.localvars.Exists("Version"))     
                        {
                            it.localversion = it.localvars["Version"].VersionFromString();
                            it.localmodified = !WriteOrCheckSHAFile(it, it.localvars, appfolder, false);
                        }
                        else
                        {
                            it.localversion = new int[] { 0, 0, 0, 0 };
                            it.localmodified = true;
                        }

                        if (it.localvars.Exists("ItemType"))
                            it.itemtype = it.localvars["ItemType"];     // allow file to override name
                    }

                    DownloadItems.Add(it);
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Exception read local files");
                }
            }
        }

        public void ReadInstallFiles(string serverlocation , string serverpath, string folder, string appfolder, string filename, int[] edversion , string defaultitemtype)
        {
            FileInfo[] allFiles = Directory.EnumerateFiles(folder, filename, SearchOption.TopDirectoryOnly).Select(f => new FileInfo(f)).OrderBy(p => p.Name).ToArray();

            foreach (FileInfo f in allFiles)
            {
                try
                {
                    bool? enabled;   // don't care about this in remote files
                    Variables cv = ReadVarsFromFile(f.FullName,out enabled);

                    if (cv != null)
                    {
                        int[] version;

                        if (cv.Exists("LongDescription") && cv.Exists("ShortDescription") &&
                            cv.Exists("Version") && cv.Exists("Location") &&
                            cv.Exists("MinEDVersion") &&
                            (version = cv["Version"].VersionFromString()) != null
                            )
                        {
                            string installfolder = System.IO.Path.Combine(appfolder, cv["Location"]);
                            string localfilename = System.IO.Path.Combine(installfolder, Path.GetFileName(f.FullName));

                            DownloadItem it = DownloadItems.Find(x => x.localfilename.Equals(localfilename, StringComparison.InvariantCultureIgnoreCase));

                            if (it != null)
                            {
                                it.downloadedpath = folder;
                                it.downloadedfilename = f.FullName;
                                it.downloadedvars = cv;
                                it.downloadedversion = version;
                                it.downloadedserver = serverlocation;
                                it.downloadedserverpath = serverpath;

                                it.state = (it.downloadedversion.CompareVersion(it.localversion) > 0) ? ItemState.OutOfDate : ItemState.UpToDate;
                            }
                            else
                            {
                                it = new DownloadItem()
                                {
                                    itemname = Path.GetFileNameWithoutExtension(f.FullName),
                                    itemtype = cv.Exists("ItemType") ? cv["ItemType"] : defaultitemtype,       // use file description of it, or use default

                                    downloadedpath = folder,
                                    downloadedfilename = f.FullName,
                                    downloadedversion = version,
                                    downloadedvars = cv,
                                    downloadedserver = serverlocation,
                                    downloadedserverpath = serverpath,

                                    localfilename = localfilename,          // set these so it knows where to install..
                                    localpath = installfolder,

                                    state = ItemState.NotPresent,
                                };

                                DownloadItems.Add(it);
                            }

                            int[] minedversion = cv["MinEDVersion"].VersionFromString();

                            if (minedversion.CompareVersion(edversion) > 0)     // if midedversion > edversion can't install
                                it.state = ItemState.EDOutOfDate;

                            if ( cv.Exists("MaxEDInstallVersion"))      
                            {
                                int[] maxedinstallversion = cv["MaxEDInstallVersion"].VersionFromString();

                                if (maxedinstallversion.CompareVersion(edversion) <= 0) // if maxedinstallversion 
                                    it.state = ItemState.EDTooOld;
                            }

                        }
                    }
                }
                catch { }
            }
        }

        private Variables ReadVarsFromFile(string file, out bool? enable)
        {
            return ActionLanguage.ActionFile.ReadVarsAndEnableFromFile(file, out enable);      // note other files share the actionfile Enabled and INSTALL format.. not the other bits
        }

        static public bool SetEnableFlag(DownloadItem item, bool enable, string appfolder)      // false if could not change the flag
        {
            if (File.Exists(item.localfilename))    // if its there..
            { 
                if (ActionLanguage.ActionFile.SetEnableFlag(item.localfilename, enable))     // if enable flag was changed..
                {
                    if (!item.localmodified)      // if was not local modified, lets set the SHA so it does not appear local modified just because of the enable
                        WriteOrCheckSHAFile(item, item.localvars, appfolder, true);

                    return true;
                }
            }

            return false;
        }

        public bool InstallFiles(DownloadItem item, string appfolder)
        {
            try
            {
                List<string[]> downloads = (from k in item.downloadedvars.NameEnumuerable where k.StartsWith("OtherFile") select item.downloadedvars[k].Split(';')).ToList();

                if (downloads.Count > 0)        // we have downloads..
                {
                    List<string> files = (from a in downloads where a.Length == 2 select a[0]).ToList();        // split them apart and get file names

                    BaseUtils.GitHubClass ghc = new BaseUtils.GitHubClass(item.downloadedserver);

                    string tempfolder = Path.GetTempPath();

                    if (ghc.Download(tempfolder, item.downloadedserverpath, files))     // download to temp folder..
                    {
                        foreach (string[] entry in downloads)                           // copy in
                        {
                            if (entry.Length == 2)
                            {
                                string folder = Path.Combine(appfolder, entry[1]);
                                if (!Directory.Exists(folder))      // ensure the folder exists
                                    Directory.CreateDirectory(folder);
                                string outfile = Path.Combine(folder, entry[0]);
                                string source = Path.Combine(tempfolder, entry[0]);
                                System.Diagnostics.Debug.WriteLine("Downloaded and installed " + outfile);
                                File.Copy(source, outfile, true);
                            }
                        }
                    }
                    else
                        return false;
                }

                foreach (string key in item.downloadedvars.NameEnumuerable)  // these first, they are not the controller files
                {
                    if (key.StartsWith("DisableOther"))
                    {
                        DownloadItem other = DownloadItems.Find(x => x.itemname.Equals(item.downloadedvars[key]));

                        if (other != null && other.localfilename != null)
                            SetEnableFlag(other, false, appfolder); // don't worry if it fails..
                    }
                }

                File.Copy(item.downloadedfilename, item.localfilename, true);

                WriteOrCheckSHAFile(item, item.downloadedvars, appfolder, true);

                return true;
            }
            catch
            {
                return false;
            }
        }

        static public bool DeleteInstall(DownloadItem item, string appfolder)
        {
            try
            {
                foreach (string key in item.localvars.NameEnumuerable)  // these first, they are not the controller files
                {
                    if (key.StartsWith("OtherFile"))
                    {
                        string[] parts = item.localvars[key].Split(';');
                        string o = Path.Combine(new string[] { appfolder, parts[1], parts[0] });
                        File.Delete(o);
                    }
                }

                File.Delete(item.localfilename);
                string shafile = Path.Combine(item.localpath, item.itemname + ".sha");
                File.Delete(shafile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // true for write, for read its true if the same..

        static bool WriteOrCheckSHAFile(DownloadItem it, Variables vars, string appfolder, bool write)
        {
            try
            {
                List<string> filelist = new List<string>() { it.localfilename };

                foreach (string key in vars.NameEnumuerable)  // these first, they are not the controller files
                {
                    if (key.StartsWith("OtherFile"))
                    {
                        string[] parts = vars[key].Split(';');
                        string o = Path.Combine(new string[] { appfolder, parts[1], parts[0] });
                        filelist.Add(o);
                    }
                }

                string shacurrent = BaseUtils.SHA.CalcSha1(filelist.ToArray());

                string shafile = Path.Combine(it.localpath, it.itemname + ".sha");

                if (write)
                {
                    using (StreamWriter sr = new StreamWriter(shafile))         // read directly from file..
                    {
                        sr.Write(shacurrent);
                    }

                    return true;
                }
                else
                {
                    if (File.Exists(shafile))       // if there is no SHA file, its local, prob under dev, so its false.  SHA is only written by install
                    {
                        using (StreamReader sr = new StreamReader(shafile))         // read directly from file..
                        {
                            string shastored = sr.ReadToEnd();
                            sr.Close();

                            if (shastored.Equals(shacurrent))
                                return true;
                        }
                    }
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("BUG BUG");
            }

            return false;
        }

        class SortIt : IComparer<DownloadItem>
        {
            public int Compare(DownloadItem our, DownloadItem other)
            {
                return our.itemname.CompareTo(other.itemname);
            }
        }

        public void Sort()
        {
            DownloadItems.Sort(new SortIt());
        }

    }
}
