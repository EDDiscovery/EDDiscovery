using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.HTTP
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
        }

        public class DownloadItem
        {
            public bool HasDownloadedCopy { get { return downloadedfilename != null;  } }
            public string LongDownloadedDescription { get { return downloadedvars != null && downloadedvars.ContainsKey("LongDescription") ? downloadedvars["LongDescription"] : ""; } }

            public string downloadedpath;
            public string downloadedfilename;
            public int[] downloadedversion;
            public ConditionVariables downloadedvars;

            public bool HasLocalCopy { get { return localfound; } }
            public string LongLocalDescription { get { return localvars != null && localvars.ContainsKey("LongDescription") ? localvars["LongDescription"] : ""; } }
            public string ShortLocalDescription { get { return localvars != null && localvars.ContainsKey("ShortDescription") ? localvars["ShortDescription"] : ""; } }

            public bool localfound;             // if scanned locally
            public string localfilename;        // always set
            public string localpath;            // always set
            public int[] localversion;          // may be null if file does not have version
            public bool localmodified;          // if local file exists, sha comparison
            public ConditionVariables localvars;    //  null, or set if local has variables
            public bool? localenable;       // null, or set if local has variables and a Enable flag

            public ItemState state;

            public string itemname;
            public string itemtype;
        };

        public List<DownloadItem> downloaditems = new List<DownloadItem>();

        public VersioningManager()
        {
        }

        public void ReadLocalFiles(string appfolder, string subfolder, string filename , string itemtype)       // DONE FIRST
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
                    it.itemtype = itemtype;

                    it.localfilename = f.FullName;
                    it.localpath = installfolder;

                    it.state = ItemState.LocalOnly;
                    it.localvars = ReadVarsFromFile(f.FullName , out it.localenable);

                    if (it.localvars != null && it.localvars.ContainsKey("Version"))     // gotta have some
                    {
                        it.localversion = Tools.VersionFromString(it.localvars["Version"]);
                        it.localmodified = !WriteOrCheckSHAFile(it, it.localvars, appfolder, false);
                        downloaditems.Add(it);
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Exception read local files");
                }
            }
        }

        public void ReadInstallFiles(string folder, string appfolder, string filename, int[] edversion , string itemtype)
        {
            FileInfo[] allFiles = Directory.EnumerateFiles(folder, filename, SearchOption.TopDirectoryOnly).Select(f => new FileInfo(f)).OrderBy(p => p.Name).ToArray();

            foreach (FileInfo f in allFiles)
            {
                try
                {
                    bool? enabled;   // don't care about this in remote files
                    ConditionVariables cv = ReadVarsFromFile(f.FullName,out enabled);

                    if (cv != null)
                    {
                        int[] version;

                        if (cv.ContainsKey("LongDescription") && cv.ContainsKey("ShortDescription") &&
                            cv.ContainsKey("Version") && cv.ContainsKey("Location") &&
                            cv.ContainsKey("MinEDVersion") &&
                            (version = Tools.VersionFromString(cv["Version"])) != null
                            )
                        {
                            string installfolder = System.IO.Path.Combine(appfolder, cv["Location"]);
                            string localfilename = System.IO.Path.Combine(installfolder, Path.GetFileName(f.FullName));

                            DownloadItem it = downloaditems.Find(x => x.localfilename.Equals(localfilename, StringComparison.InvariantCultureIgnoreCase));

                            if (it != null)
                            {
                                it.downloadedpath = folder;
                                it.downloadedfilename = f.FullName;
                                it.downloadedvars = cv;
                                it.downloadedversion = version;

                                it.state = (Tools.CompareVersion(it.downloadedversion, it.localversion) > 0) ? ItemState.OutOfDate : ItemState.UpToDate;
                            }
                            else
                            {
                                it = new DownloadItem()
                                {
                                    itemname = Path.GetFileNameWithoutExtension(f.FullName),
                                    itemtype = itemtype,

                                    downloadedpath = folder,
                                    downloadedfilename = f.FullName,
                                    downloadedversion = version,
                                    downloadedvars = cv,

                                    localfilename = localfilename,          // set these so it knows where to install..
                                    localpath = installfolder,

                                    state = ItemState.NotPresent,
                                };

                                downloaditems.Add(it);
                            }

                            int[] minedversion = Tools.VersionFromString(cv["MinEDVersion"]);

                            if (Tools.CompareVersion(minedversion, edversion) > 0)
                                it.state = ItemState.EDOutOfDate;

                        }
                    }
                }
                catch { }
            }
        }

        private ConditionVariables ReadVarsFromFile(string file, out bool? enable)
        {
            using (StreamReader sr = new StreamReader(file))         // read directly from file..
            {
                string json = sr.ReadToEnd();

                JObject jo = (JObject)JObject.Parse(json);

                enable = null;
                if (!JSONHelper.IsNullOrEmptyT(jo["Enabled"]))      // if this has an enabled, note it
                    enable = (bool)jo["Enabled"];

                JArray ivarja = (JArray)jo["Install"];

                if (!JSONHelper.IsNullOrEmptyT(ivarja))
                {
                    ConditionVariables cv = new ConditionVariables();
                    cv.FromJSONObject(ivarja);
                    return cv;
                }
            }

            return null;
        }

        static public bool SetEnableFlag(string file , bool enable)
        {
            try
            { 
                JObject jo;
                using (StreamReader sr = new StreamReader(file))         // read directly from file..
                {
                    string json = sr.ReadToEnd();

                    jo = (JObject)JObject.Parse(json);
                }

                jo["Enabled"] = enable;

                using (StreamWriter sr = new StreamWriter(file))         // read directly from file..
                {
                    sr.Write(jo.ToString(Newtonsoft.Json.Formatting.Indented));
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        static public bool InstallFiles(DownloadItem item, string appfolder)
        {
            try
            {
                foreach (KeyValuePair<string, string> k in item.downloadedvars.values)  // these first, they are not the controller files
                {
                    if (k.Key.StartsWith("OtherFile"))
                    {
                        string[] parts = k.Value.Split(';');
                        string o = Path.Combine(new string[] { appfolder, parts[1], parts[0] });
                        string s = Path.Combine(item.downloadedpath, parts[0]);
                        File.Copy(s, o, true);
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
                foreach (KeyValuePair<string, string> k in item.localvars.values)  // these first, they are not the controller files
                {
                    if (k.Key.StartsWith("OtherFile"))
                    {
                        string[] parts = k.Value.Split(';');
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

        static bool WriteOrCheckSHAFile(DownloadItem it, ConditionVariables vars, string appfolder , bool write)
        {
            try
            {
                List<string> filelist = new List<string>() { it.localfilename };

                foreach (KeyValuePair<string, string> k in vars.values)  // these first, they are not the controller files
                {
                    if (k.Key.StartsWith("OtherFile"))
                    {
                        string[] parts = k.Value.Split(';');
                        string o = Path.Combine(new string[] { appfolder, parts[1], parts[0] });
                        filelist.Add(o);
                    }
                }

                string shacurrent = GitHubClass.CalcSha1(filelist.ToArray());

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
            downloaditems.Sort(new SortIt());
        }

    }
}
