/*e
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
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace EliteDangerousCore
{
    public class BindingsFile : IEnumerable<BindingsFile.Device>
    {
        public bool Loaded { get { return devices.Count > 0; } }

        public HashSet<string> AxisNames { get; private set; } = new HashSet<string>();        // from Bindings in frontier file - all names even if not assigned
        public HashSet<string> KeyNames { get; private set; } = new HashSet<string>();         // from Primary or Secondary in frontier file - all names even if not assigned

        // assignedfunc to device and assignment, may be many
        public Dictionary<string, List<Tuple<Device, Assignment>>> AssignedNames { get; private set; } = new Dictionary<string, List<Tuple<Device, Assignment>>>();

        private Dictionary<string, Device> devices = new Dictionary<string, Device>();
        private Dictionary<string, string> values = new Dictionary<string, string>();

        public const string KeyboardDeviceName = "Keyboard";     // frontier name for keyboard device

        [System.Diagnostics.DebuggerDisplay("DKP {Device.Name} {Key}")]
        public class DeviceKeyPair
        {
            public Device Device;
            public string Key;          // Keyboard: in Keys naming convention - converted from Frontier on input.
        }

        [System.Diagnostics.DebuggerDisplay("Assignment {assignedfunc} Keys {keys.Count}" )]
        public class Assignment
        {
            public List<DeviceKeyPair> keys;  // first is always the primary one
            public string assignedfunc;

            public override string ToString()
            {
                StringBuilder s = new StringBuilder(64);
                if (keys.Count > 1)
                    s.Append("(");

                for (int i = 0; i < keys.Count; i++)
                {
                    if (i >= 1)
                        s.Append(",");

                    s.AppendFormat(keys[i].Device.Name + ":" + keys[i].Key);
                }

                if (keys.Count > 1)
                    s.Append(")");

                s.Append("=" + assignedfunc);

                return s.ToNullSafeString();
            }

            public bool HasKeyAssignment(string key)        // is key in this list (Keys naming convention)
            {
                foreach (DeviceKeyPair k in keys)
                {
                    if (k.Key.Equals(key))
                        return true;
                }

                return false;
            }

            public bool HasKeyAssignment(Assignment other)      // do the keys in other clash with our keys
            {
                foreach (DeviceKeyPair o in other.keys)
                {
                    foreach (DeviceKeyPair k in keys)
                    {
                        if (k.Key.Equals(o.Key))
                            return true;
                    }
                }

                return false;
            }

            public bool KeyAssignementLongerThan(List<BindingsFile.Assignment> others)  // is ours the best keylist (based on length)
            {
                foreach (BindingsFile.Assignment a in others)
                {
                    if (a != this)  // in case we are in the list
                    {
                        if (a.HasKeyAssignment(a))        // do we have a clash of keys, other has keys in our key list..
                        {
                            if (keys.Count < a.keys.Count)  // yes, is our key length less.. then its the others.
                                return false;
                        }
                    }
                }
                return true;
            }

            public int NumberOfKeysWithDevice(string name)
            {
                return (from x in keys where x.Device.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) select x).Count();
            }

            public bool AllOfDevice(string name)
            {
                return NumberOfKeysWithDevice(name) == keys.Count;
            }
        }

        [System.Diagnostics.DebuggerDisplay("Device {Name} {Assignments.Count}")]
        public class Device
        {
            public string Name;
            public Dictionary<string, List<Assignment>> Assignments;     // given a primary key, give a list of assignments on it.

            public List<Assignment> Find(string name, bool partialmatch)
            {
                if (partialmatch)
                {
                    var filtered = Assignments.Where(d => d.Key.Contains(name)).Select(k => k.Value);
                    var singlelist = filtered.SelectMany(x => x).ToList();
                    return singlelist;
                }
                else
                    return Assignments.ContainsKey(name) ? Assignments[name] : null;
            }
        }

        public IEnumerator<Device> GetEnumerator()
        {
            foreach (string e in devices.Keys)
                yield return devices[e];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Device FindOrMakeDevice(string name)
        {
            if (devices.ContainsKey(name))
                return devices[name];
            else
            {
                Device d = new Device() { Name = name, Assignments = new Dictionary<string, List<Assignment>>() };
                devices[name] = d;
                return d;
            }
        }

        private void AssignToDevice(XElement d, XElement map)
        {
            XAttribute xdevice = map.Attribute("Device");
            XAttribute xkey = map.Attribute("Key");
            if (xdevice != null && xkey != null && xkey.Value.Length > 0)
            {
                string key = xkey.Value;

                int povindex = key.IndexOf("POV");    // pov evil.. frontier code these as primary (l/r/u/d) and modifier (l/r/u/d) in no particular order
                string povroot = (povindex > 0) ? key.Truncate(0, povindex + 4) : null;

                List<DeviceKeyPair> dvp = new List<DeviceKeyPair>();

                foreach (XElement y in map.Descendants())
                {
                    if (y.Name == "Modifier")
                    {
                        string km = y.Attribute("Device").Value;
                        string vm = y.Attribute("Key").Value;

                        if (povroot != null && vm.Truncate(0, povroot.Length).Equals(povroot))       // POV pair..  lets adjust so its just the major entry.. makes it easier
                        {
                            if (key.Contains("Left") || vm.Contains("Left"))
                                key = povroot + ((key.Contains("Up") || vm.Contains("Up")) ? "UpLeft" : "DownLeft");
                            else if (key.Contains("Right") || vm.Contains("Right"))
                                key = povroot + ((key.Contains("Up") || vm.Contains("Up")) ? "UpRight" : "DownRight");
                        }
                        else
                            dvp.Add(new DeviceKeyPair() { Device = FindOrMakeDevice(km), Key = FrontierToKeys(km, vm) });
                    }
                }

                dvp.Insert(0, new DeviceKeyPair() { Device = FindOrMakeDevice(xdevice.Value), Key = FrontierToKeys(xdevice.Value, key) });

                Assignment a = new Assignment() { assignedfunc = d.Name.ToString(), keys = dvp };

                if (!AssignedNames.ContainsKey(a.assignedfunc))
                    AssignedNames[a.assignedfunc] = new List<Tuple<Device, Assignment>>();

                AssignedNames[a.assignedfunc].Add(new Tuple<Device, Assignment>(dvp[0].Device, a));

                foreach (DeviceKeyPair dkp in dvp)      // all keys mentioned need adding
                {
                    //System.Diagnostics.Debug.WriteLine("{0} {1}", dkp.Device.Name, a.assignedfunc);

                    if (!dkp.Device.Assignments.ContainsKey(dkp.Key))
                        dkp.Device.Assignments[dkp.Key] = new List<Assignment>();

                    dkp.Device.Assignments[dkp.Key].Add(a);
                }
            }
        }

        private string ChkValue(string s)
        {
            return (s == "Value") ? "" : ("." + s);
        }

        public bool LoadBindingsFile()
        {
            devices = new Dictionary<string, Device>();     // clear
            values = new Dictionary<string, string>();

            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Frontier Developments\Elite Dangerous\Options\Bindings");

            string optsel = System.IO.Path.Combine(path, "StartPreset.start");

            try
            {
                string sel = System.IO.File.ReadAllText(optsel);
                System.Diagnostics.Debug.WriteLine("Bindings file " + sel);

                FileInfo[] allFiles = Directory.EnumerateFiles(path, sel + "*.binds", SearchOption.TopDirectoryOnly).Select(f => new System.IO.FileInfo(f)).OrderByDescending(p => p.LastWriteTime).ToArray();

                if (allFiles.Length >= 1)
                {
                    XElement bindings = XElement.Load(allFiles[0].FullName);

                    foreach (XElement x in bindings.Elements())
                    {
                        //System.Diagnostics.Debug.WriteLine("Reader " + x.NodeType + " " + x.Name);

                        if (x.HasElements)
                        {
                            foreach (XElement y in x.Descendants())
                            {
                                if (y.Name == "Binding")
                                {
                                    AxisNames.Add(x.Name.LocalName);
                                    AssignToDevice(x, y);
                                }
                                else if (y.Name == "Primary" || y.Name == "Secondary")
                                {
                                    //System.Diagnostics.Debug.WriteLine("Binding Point " + x.NodeType + " " + x.Name + " Element " + y.Name);
                                    KeyNames.Add(x.Name.LocalName);
                                    AssignToDevice(x, y);
                                }
                                else
                                {
                                    foreach (XAttribute z in y.Attributes())
                                    {
                                        values[x.Name + "." + y.Name + ChkValue(z.Name.ToString())] = z.Value;
                                    }
                                }
                            }
                        }

                        if (x.HasAttributes)
                        {
                            foreach (XAttribute y in x.Attributes())
                            {
                                values[x.Name + ChkValue(y.Name.ToString())] = y.Value;
                            }
                        }
                    }
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        public Device GetDevice(string name)
        {
            return devices.ContainsKey(name) ? devices[name] : null;
        }

        public Device FindDevice(string name, Guid instanceguid, Guid productguid)    // best match of physical device info to our binding devices
        {
            Device bestmatch = null;
            int besttotal = 0;

            foreach (Device dv in devices.Values)
            {
                if (dv.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))      // exact match
                    return dv;

                if (dv.Name.Equals(GuidExtract(instanceguid, false), StringComparison.InvariantCultureIgnoreCase))
                    return dv;
                if (dv.Name.Equals(GuidExtract(instanceguid, true), StringComparison.InvariantCultureIgnoreCase))
                    return dv;
                if (dv.Name.Equals(GuidExtract(productguid, false), StringComparison.InvariantCultureIgnoreCase))
                    return dv;
                if (dv.Name.Equals(GuidExtract(productguid, true), StringComparison.InvariantCultureIgnoreCase))
                    return dv;

                int total = dv.Name.ToLower().ApproxMatch(name.ToLower(), 4);
                if (total > besttotal)
                {
                    besttotal = total;
                    bestmatch = dv;
                }
            }

            if (bestmatch != null)
                return bestmatch;

            return null;
        }

        public List<Tuple<Device, Assignment>> FindAssignedFunc(string name, string preferreddevice=null)       // NULL if no match found 
        {
            if (AssignedNames.ContainsKey(name))
            {
                List<Tuple<Device, Assignment>> ret = new List<Tuple<Device, Assignment>>();

                foreach (Tuple<Device, Assignment> a in AssignedNames[name])        // search assignments under this name
                {
                    if ( preferreddevice == null || a.Item2.AllOfDevice(preferreddevice) )      // if null, or all of this device.. return
                        ret.Add(new Tuple<Device, Assignment>(a.Item1, a.Item2));
                }

                return ret.Count > 0 ? ret : null;
            }

            return null;
        }

        string GuidExtract(Guid g, bool rev)
        {
            string s = g.ToString();
            int slash = s.IndexOf('-');
            if (slash >= 0)
                s = s.Substring(0, slash);

            if (rev && s.Length == 8)
            {
                s = s.Substring(4, 4) + s.Substring(0, 4);
            }

            return s;
        }

        public List<Assignment> Find(string name, bool partialmatch)        // given a key name, find the list of assignments for it
        {
            List<Assignment> ret = new List<Assignment>();

            foreach (Device dv in devices.Values)
            {
                List<Assignment> f = dv.Find(name, partialmatch);
                if (f != null)
                    ret.AddRange(f);
            }

            return ret;
        }

        public Dictionary<string,string> BindingValue(string s,bool partial)    // given a value name, with partial match flag, find the binding values
        {
            if ( partial )
                return (from v in values where v.Key.Contains(s) select v).ToDictionary(x => x.Key, x => x.Value);
            else
                return (from v in values where v.Key.Equals(s) select v).ToDictionary(x => x.Key, x => x.Value);
        }


        private string Mappings(string devicename)
        {
            StringBuilder s = new StringBuilder(128);
            if (devices.ContainsKey(devicename))
            {
                foreach (string keyname in devices[devicename].Assignments.Keys)
                {
                    foreach (Assignment a in devices[devicename].Assignments[keyname])
                    {
                        s.Append("Key " + keyname + "=>" + a.ToString());
                        s.AppendLine();
                    }
                }
            }
            return s.ToString();
        }

        public string ListBindings()
        {
            string ret = "";
            foreach (string s in devices.Keys)
            {
                ret += s + Environment.NewLine + Mappings(s) + Environment.NewLine;
            }
            return ret;

        }

        public string ListValues()
        {
            return String.Join(Environment.NewLine, (from x in values.Keys select x));
        }

        public string ListKeyNames(string prefix = "", string postfix = "")
        {
            return String.Join(Environment.NewLine, (from x in KeyNames select prefix + x + postfix));
        }


        static private string FrontierToKeys(string device, string frontiername)
        {
            if (device == KeyboardDeviceName)
            {
                string ret = EliteDangerousCore.FrontierKeyConversion.FrontierToKeys(frontiername);
                //System.Diagnostics.Debug.WriteLine("Frontier Name Convert {0} to {1}", frontiername, ret);
                return ret;
            }
            else
                return frontiername;
        }


    }
}
