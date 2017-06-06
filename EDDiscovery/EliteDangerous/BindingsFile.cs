using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EDDiscovery.EliteDangerous
{
    public class BindingsFile
    {
        public bool Loaded { get { return devices.Count > 0; } }

        public class DeviceKeyPair
        {
            public Device Device;
            public string Key;
        }

        //[System.Diagnostics.DebuggerDisplay("Assignment ")]
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

            public bool HasKeyAssignment(string key)        // is key in this list
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
        }

        [System.Diagnostics.DebuggerDisplay("Device {name} {assignments.Count}")]
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

        Dictionary<string, Device> devices = new Dictionary<string, Device>();
        Dictionary<string, string> values = new Dictionary<string, string>();

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
                            dvp.Add(new DeviceKeyPair() { Device = FindOrMakeDevice(km), Key = vm });
                    }
                }

                dvp.Insert(0, new DeviceKeyPair() { Device = FindOrMakeDevice(xdevice.Value), Key = key });

                Assignment a = new Assignment() { assignedfunc = d.Name.ToString(), keys = dvp };

                foreach (DeviceKeyPair dkp in dvp)      // all keys mentioned need adding
                {
                    if (!dkp.Device.Assignments.ContainsKey(dkp.Key))
                        dkp.Device.Assignments[dkp.Key] = new List<Assignment>();

                    dkp.Device.Assignments[dkp.Key].Add(a);
                }
            }
        }

        public string Mappings(string devicename)
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
                                if (y.Name == "Binding" || y.Name == "Primary" || y.Name == "Secondary")
                                {
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

                    foreach (string s in devices.Keys)
                    {
                        //System.Diagnostics.Debug.WriteLine("Device " + s + Environment.NewLine + Mappings(s));
                    }

                    foreach (string s in values.Keys)
                    {
                        //System.Diagnostics.Debug.WriteLine("Values " + s + "=" + values[s]);
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

        public Device FindDevice(string name, Guid instanceguid, Guid productguid)    // best match
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

        public List<Assignment> Find(string name, bool partialmatch)
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

        public Dictionary<string,string> BindingValue(string s,bool partial)
        {
            if ( partial )
                return (from v in values where v.Key.Contains(s) select v).ToDictionary(x => x.Key, x => x.Value);
            else
                return (from v in values where v.Key.Equals(s) select v).ToDictionary(x => x.Key, x => x.Value);
        }

    }
}
