/*
 * Copyright © 2015 - 2018 EDDiscovery development team
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

namespace EDDiscovery.DLL
{
    public class EDDDLLManager
    {
        public int Count { get { return dlls.Count; } }

        private List<EDDDLLCaller> dlls = new List<EDDDLLCaller>();

        // return loaded, failed, notallowed
        public Tuple<string,string,string> Load(string directory, string ourversion, string dllfolder, EDDDLLIF.EDDCallBacks callbacks, string allowed)
        {
            string loaded = "";
            string failed = "";
            string notallowed = "";

            if (!Directory.Exists(directory))
                failed = "DLL Folder does not exist";
            else
            {
                FileInfo[] allFiles = Directory.EnumerateFiles(directory, "*.dll", SearchOption.TopDirectoryOnly).Select(f => new FileInfo(f)).OrderBy(p => p.LastWriteTime).ToArray();

                string[] allowedfiles = allowed.Split(',');

                foreach (FileInfo f in allFiles)
                {
                    string filename = System.IO.Path.GetFileNameWithoutExtension(f.FullName);

                    EDDDLLCaller caller = new EDDDLLCaller();

                    if (caller.Load(f.FullName))        // if loaded (meaning it loaded, and its got EDDInitialise)
                    {
                        if (allowed.Equals("All", StringComparison.InvariantCultureIgnoreCase) || allowedfiles.Contains(filename, StringComparer.InvariantCultureIgnoreCase))    // if allowed..
                        {
                            if (caller.Init(ourversion, dllfolder, callbacks))       // must init
                            {
                                dlls.Add(caller);
                                loaded = ObjectExtensionsStrings.AppendPrePad(loaded, caller.Name, ",");
                            }
                            else
                                failed += ObjectExtensionsStrings.AppendPrePad(failed, caller.Name, ",");
                        }
                        else
                        {
                            notallowed += ObjectExtensionsStrings.AppendPrePad(notallowed, caller.Name, ",");
                        }
                    }
                }
            }

            return new Tuple<string, string,string>(loaded,failed,notallowed);
        }

        public void UnLoad()
        {
            foreach (EDDDLLCaller caller in dlls)
            {
                caller.UnLoad();
            }

            dlls.Clear();
        }

        public void Refresh(string cmdr, EDDDLLIF.JournalEntry je)
        {
            foreach (EDDDLLCaller caller in dlls)
            {
                caller.Refresh(cmdr, je);
            }
        }

        public void NewJournalEntry(EDDDLLIF.JournalEntry nje)
        {
            foreach (EDDDLLCaller caller in dlls)
            {
                caller.NewJournalEntry(nje);
            }
        }

        private EDDDLLCaller FindCaller(string name)
        {
            return dlls.Find(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        // item1 = true if found, item2 = true if caller implements.
        public Tuple<bool, bool> ActionJournalEntry(string dllname, EDDDLLIF.JournalEntry nje)
        {
            if (dllname.Equals("All", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (EDDDLLCaller caller in dlls)
                    caller.ActionJournalEntry(nje);

                return new Tuple<bool, bool>(true, true);
            }
            else
            {
                EDDDLLCaller caller = FindCaller(dllname);
                return caller != null ? new Tuple<bool, bool>(true, caller.ActionJournalEntry(nje)) : new Tuple<bool, bool>(false, false);
            }
        }

        // NULL/False if no DLL found, or <string,true> if DLL found, string may be null if DLL does not implement action command
        public Tuple<string, bool> ActionCommand(string dllname, string cmd, string[] paras)
        {
            if (dllname.Equals("All", StringComparison.InvariantCultureIgnoreCase))
            {
                string ret = "";

                foreach (EDDDLLCaller caller in dlls)
                {
                    string r = caller.ActionCommand(cmd, paras);
                    if (r != null)
                        ret += r + ";";
                }

                return new Tuple<string, bool>(ret, true);
            }
            else
            {
                EDDDLLCaller caller = FindCaller(dllname);
                return caller != null ? new Tuple<string, bool>(caller.ActionCommand(cmd, paras), true) : new Tuple<string, bool>(null, false);
            }
        }
    }
}

