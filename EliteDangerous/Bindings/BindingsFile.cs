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
                System.Diagnostics.Trace.WriteLine("Bindings file " + sel);

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

        public Device FindDevice(string name, Guid instanceguid, Guid productguid, int productid, int vendorid)    // best match of physical device info to our binding devices
        {
            Device bestmatch = null;
            int besttotal = 0;

            string frontiername = devicemapping.ContainsKey(new Tuple<int, int>(productid, vendorid)) ? devicemapping[new Tuple<int, int>(productid, vendorid)] : null;

            foreach (Device dv in devices.Values)
            {
                if (dv.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))      // exact match
                    return dv;

                if (frontiername != null && dv.Name.Equals(frontiername, StringComparison.InvariantCultureIgnoreCase))
                    return dv;

                if (dv.Name.Equals(GuidExtract(instanceguid, false), StringComparison.InvariantCultureIgnoreCase))
                    return dv;
                if (dv.Name.Equals(GuidExtract(instanceguid, true), StringComparison.InvariantCultureIgnoreCase))
                    return dv;
                if (dv.Name.Equals(GuidExtract(productguid, false), StringComparison.InvariantCultureIgnoreCase))
                    return dv;
                if (dv.Name.Equals(GuidExtract(productguid, true), StringComparison.InvariantCultureIgnoreCase))
                    return dv;

                int total = dv.Name.SplitCapsWord().ToLowerInvariant().ApproxMatch(name.ToLowerInvariant(), 4);
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

        // From frontier DeviceMapping.xml table 11/Jan/2018
        Dictionary<Tuple<int, int>, string> devicemapping = new Dictionary<Tuple<int, int>, string>()
        {
             {  new Tuple<int,int>(0x28E, 0x45E), "GamePad" },
             {  new Tuple<int,int>(0x28F, 0x45E), "GamePad" },
             {  new Tuple<int,int>(0x2FF, 0x45E), "GamePad" },
             {  new Tuple<int,int>(0x5D04, 0x24C6), "GamePad" },
             {  new Tuple<int,int>(0x2A1, 0x45E), "GamePad" },
             {  new Tuple<int,int>(0x4716, 0x738), "GamePad" },
             {  new Tuple<int,int>(0x213, 0xE6F), "GamePad" },
             {  new Tuple<int,int>(0x291, 0x45E), "GamePad" },
             {  new Tuple<int,int>(0x719, 0x45E), "GamePad" },
             {  new Tuple<int,int>(0xF07, 0x44F), "GamePad" },
             {  new Tuple<int,int>(0xB326, 0x44F), "GamePad" },
             {  new Tuple<int,int>(0xC21D, 0x46D), "GamePad" },
             {  new Tuple<int,int>(0xC21E, 0x46D), "GamePad" },
             {  new Tuple<int,int>(0xC21F, 0x46D), "GamePad" },
             {  new Tuple<int,int>(0xC242, 0x46D), "GamePad" },
             {  new Tuple<int,int>(0xCA84, 0x46D), "GamePad" },
             {  new Tuple<int,int>(0x4540, 0x738), "GamePad" },
             {  new Tuple<int,int>(0x4556, 0x738), "GamePad" },
             {  new Tuple<int,int>(0x4718, 0x738), "GamePad" },
             {  new Tuple<int,int>(0x4726, 0x738), "GamePad" },
             {  new Tuple<int,int>(0x4728, 0x738), "GamePad" },
             {  new Tuple<int,int>(0x4738, 0x738), "GamePad" },
             {  new Tuple<int,int>(0x4740, 0x738), "GamePad" },
             {  new Tuple<int,int>(0x6040, 0x738), "GamePad" },
             {  new Tuple<int,int>(0xB726, 0x738), "GamePad" },
             {  new Tuple<int,int>(0xBEEF, 0x738), "GamePad" },
             {  new Tuple<int,int>(0xCB02, 0x738), "GamePad" },
             {  new Tuple<int,int>(0xCB03, 0x738), "GamePad" },
             {  new Tuple<int,int>(0xF738, 0x738), "GamePad" },
             {  new Tuple<int,int>(0x8802, 0xC12), "GamePad" },
             {  new Tuple<int,int>(0x8809, 0xC12), "GamePad" },
             {  new Tuple<int,int>(0x880A, 0xC12), "GamePad" },
             {  new Tuple<int,int>(0x5, 0xE6F), "GamePad" },
             {  new Tuple<int,int>(0x6, 0xE6F), "GamePad" },
             {  new Tuple<int,int>(0x105, 0xE6F), "GamePad" },
             {  new Tuple<int,int>(0x113, 0xE6F), "GamePad" },
             {  new Tuple<int,int>(0x201, 0xE6F), "GamePad" },
             {  new Tuple<int,int>(0x21F, 0xE6F), "GamePad" },
             {  new Tuple<int,int>(0x301, 0xE6F), "GamePad" },
             {  new Tuple<int,int>(0x401, 0xE6F), "GamePad" },
             {  new Tuple<int,int>(0x201, 0xE8F), "GamePad" },
             {  new Tuple<int,int>(0x3008, 0xE8F), "GamePad" },
             {  new Tuple<int,int>(0xA, 0xF0D), "GamePad" },
             {  new Tuple<int,int>(0xD, 0xF0D), "GamePad" },
             {  new Tuple<int,int>(0x16, 0xF0D), "GamePad" },
             {  new Tuple<int,int>(0x202, 0xF30), "GamePad" },
             {  new Tuple<int,int>(0x8888, 0xF30), "GamePad" },
             {  new Tuple<int,int>(0xFF0C, 0x102C), "GamePad" },
             {  new Tuple<int,int>(0x4, 0x12AB), "GamePad" },
             {  new Tuple<int,int>(0x301, 0x12AB), "GamePad" },
             {  new Tuple<int,int>(0x8809, 0x12AB), "GamePad" },
             {  new Tuple<int,int>(0x4748, 0x1430), "GamePad" },
             {  new Tuple<int,int>(0x8888, 0x1430), "GamePad" },
             {  new Tuple<int,int>(0x601, 0x146B), "GamePad" },
             {  new Tuple<int,int>(0x37, 0x1532), "GamePad" },
             {  new Tuple<int,int>(0x3F00, 0x15E4), "GamePad" },
             {  new Tuple<int,int>(0x3F0A, 0x15E4), "GamePad" },
             {  new Tuple<int,int>(0x3F10, 0x15E4), "GamePad" },
             {  new Tuple<int,int>(0xBEEF, 0x162E), "GamePad" },
             {  new Tuple<int,int>(0xFD00, 0x1689), "GamePad" },
             {  new Tuple<int,int>(0xFD01, 0x1689), "GamePad" },
             {  new Tuple<int,int>(0x2, 0x1BAD), "GamePad" },
             {  new Tuple<int,int>(0x3, 0x1BAD), "GamePad" },
             {  new Tuple<int,int>(0xF016, 0x1BAD), "GamePad" },
             {  new Tuple<int,int>(0xF023, 0x1BAD), "GamePad" },
             {  new Tuple<int,int>(0xF028, 0x1BAD), "GamePad" },
             {  new Tuple<int,int>(0xF038, 0x1BAD), "GamePad" },
             {  new Tuple<int,int>(0xF900, 0x1BAD), "GamePad" },
             {  new Tuple<int,int>(0xF901, 0x1BAD), "GamePad" },
             {  new Tuple<int,int>(0xF903, 0x1BAD), "GamePad" },
             {  new Tuple<int,int>(0x5000, 0x24C6), "GamePad" },
             {  new Tuple<int,int>(0x5300, 0x24C6), "GamePad" },
             {  new Tuple<int,int>(0x5303, 0x24C6), "GamePad" },
             {  new Tuple<int,int>(0x5500, 0x24C6), "GamePad" },
             {  new Tuple<int,int>(0x5501, 0x24C6), "GamePad" },
             {  new Tuple<int,int>(0x5506, 0x24C6), "GamePad" },
             {  new Tuple<int,int>(0x5B02, 0x24C6), "GamePad" },
             {  new Tuple<int,int>(0x2D1, 0x45E), "GamePad" },
             {  new Tuple<int,int>(0x317, 0x7B5), "BlackWidow" },
             {  new Tuple<int,int>(0xC28, 0x6A3), "SaitekAV8R03" },
             {  new Tuple<int,int>(0x461, 0x6A3), "SaitekAV8R03" },
             {  new Tuple<int,int>(0x2215, 0x738), "SaitekX55Joystick" },
             {  new Tuple<int,int>(0x2221, 0x738), "SaitekX56Joystick" },
             {  new Tuple<int,int>(0xA215, 0x738), "SaitekX55Throttle" },
             {  new Tuple<int,int>(0xA221, 0x738), "SaitekX56Throttle" },
             {  new Tuple<int,int>(0x762, 0x6A3), "SaitekX52Pro" },
             {  new Tuple<int,int>(0x75C, 0x6A3), "SaitekX52" },
             {  new Tuple<int,int>(0x255, 0x6A3), "SaitekX52" },
             {  new Tuple<int,int>(0xC2AA, 0x46D), "LogitechG940Pedals" },
             {  new Tuple<int,int>(0xC2A8, 0x46D), "LogitechG940Joystick" },
             {  new Tuple<int,int>(0xC2A9, 0x46D), "LogitechG940Throttle" },
             {  new Tuple<int,int>(0x402, 0x44F), "ThrustMasterWarthogJoystick" },
             {  new Tuple<int,int>(0x404, 0x44F), "ThrustMasterWarthogThrottle" },
             {  new Tuple<int,int>(0xFFFF, 0x44F), "ThrustMasterWarthogCombined" },
             {  new Tuple<int,int>(0x1302, 0x738), "SaitekFLY5" },
             {  new Tuple<int,int>(0xB108, 0x44F), "ThrustMasterTFlightHOTASX" },
             {  new Tuple<int,int>(0xB67C, 0x44F), "ThrustMasterHOTAS4" },
             {  new Tuple<int,int>(0xB10A, 0x44F), "T16000M" },
             {  new Tuple<int,int>(0xB687, 0x44F), "T16000MTHROTTLE" },
             {  new Tuple<int,int>(0xB679, 0x44F), "T-Rudder" },
             {  new Tuple<int,int>(0xC215, 0x46D), "LogitechExtreme3DPro" },
             {  new Tuple<int,int>(0xC121, 0x25F0), "GioteckPS3WiredController" },
             {  new Tuple<int,int>(0x53C, 0x6A3), "SaitekX45" },
             {  new Tuple<int,int>(0x8037, 0x2341), "EDTracker" },
             {  new Tuple<int,int>(0xC219, 0x46D), "Logitech710WirelessGamepad" },
             {  new Tuple<int,int>(0xC285, 0x46D), "LogitechWingManStrikeForce3D" },
             {  new Tuple<int,int>(0x5F0D, 0x6A3), "SaitekP2600RumbleForce" },
             {  new Tuple<int,int>(0xFF0C, 0x6A3), "SaitekP2500RumbleForce" },
             {  new Tuple<int,int>(0x353E, 0x6A3), "SaitekCyborgEvoWireless" },
             {  new Tuple<int,int>(0x764, 0x6A3), "SaitekProFlightCombatRudderPedals" },
             {  new Tuple<int,int>(0x763, 0x6A3), "SaitekProFlightRudderPedals" },
             {  new Tuple<int,int>(0xBEAD, 0x1234), "vJoy" },
             {  new Tuple<int,int>(0xF1, 0x68E), "CHProThrottle1" },
             {  new Tuple<int,int>(0xC0F1, 0x68E), "CHProThrottle2" },
             {  new Tuple<int,int>(0xF3, 0x68E), "CHFighterStick" },
             {  new Tuple<int,int>(0xC0F2, 0x68E), "CHProPedals" },
             {  new Tuple<int,int>(0xC0F4, 0x68E), "CHCombatStick" },
             {  new Tuple<int,int>(0xF4, 0x68E), "CHCombatStick" },
             {  new Tuple<int,int>(0x3, 0xE8F), "TrustPredator" },
             {  new Tuple<int,int>(0x268, 0x54C), "Playstation3Controller" },
             {  new Tuple<int,int>(0x460, 0x6A3), "SaitekST290Pro" },
             {  new Tuple<int,int>(0x3001, 0x47D), "XterminatorDualControl" },
             {  new Tuple<int,int>(0x1B, 0x45E), "SideWinderForceFeedback2" },
             {  new Tuple<int,int>(0x8036, 0x2341), "ArduinoLeonardo" },
             {  new Tuple<int,int>(0x11, 0x4D8), "SlawFlightControlRudder" },
             {  new Tuple<int,int>(0x211, 0x2833), "OculusTouch" },
             {  new Tuple<int,int>(0xBA0, 0x54C), "DualShock4" },
             {  new Tuple<int,int>(0x5C4, 0x54C), "DualShock4" },
             };
    }
}
