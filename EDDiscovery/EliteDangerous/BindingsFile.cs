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
            public string device;
            public string key;
        }

        [System.Diagnostics.DebuggerDisplay("Assignment {assignedfunc} {modifiersrequired.Count}")]
        public class Assignment
        {
            public string assignedfunc;
            public List<DeviceKeyPair> modifiersrequired;  // null if none, otherwise list of other keys needed..

            public override string ToString()
            {
                StringBuilder s = new StringBuilder(64);
                s.Append(assignedfunc);
                if (modifiersrequired != null)
                {
                    s.Append("(");
                    bool comma = false;
                    foreach (DeviceKeyPair dkp in modifiersrequired)
                    {
                        if (comma)
                            s.Append(",");

                        s.AppendFormat(dkp.device + ":" + dkp.key);
                        comma = true;
                    }
                    s.Append(")");
                }

                return s.ToNullSafeString();
            }
        }

        [System.Diagnostics.DebuggerDisplay("Device {name} {assignments.Count}")]
        public class Device
        {
            public string name;
            public Dictionary<string, List<Assignment>> assignments;     // given a primary key, give a list of assignments on it.

            public List<Assignment> Find( string name )
            {
                return assignments.ContainsKey(name) ? assignments[name] : null;
            }
        }

        Dictionary<string, Device> devices = new Dictionary<string, Device>();
        Dictionary<string, string> values = new Dictionary<string, string>();

        public Device EnsureDevice(string name )
        {
            if (devices.ContainsKey(name))
                return devices[name];
            else
            {
                Device d = new Device() { name = name, assignments = new Dictionary<string, List<Assignment>>() };
                devices[name] = d;
                return d;
            }
        }

        public void AssignToDevice(XElement d, XElement map )
        {
            XAttribute device = map.Attribute("Device");
            XAttribute key = map.Attribute("Key");
            if ( device != null && key != null && key.Value.Length>0 )
            {
                Device dv = EnsureDevice(device.Value);

                if (!dv.assignments.ContainsKey(key.Value))
                    dv.assignments[key.Value] = new List<Assignment>();

                List<DeviceKeyPair> dvp = new List<DeviceKeyPair>();

                foreach (XElement y in map.Descendants())
                {
                    if (y.Name == "Modifier")
                        dvp.Add(new DeviceKeyPair() { device = y.Attribute("Device").Value, key = y.Attribute("Key").Value });
                }

                string func = d.Name.ToString();
                Assignment a = new Assignment() { assignedfunc = func, modifiersrequired = (dvp.Count > 0) ? dvp : null };

                dv.assignments[key.Value].Add(a);
            }
        }

        public string Mappings(string devicename )
        {
            StringBuilder s = new StringBuilder(128);
            if ( devices.ContainsKey(devicename))
            {
                foreach( string keyname in devices[devicename].assignments.Keys)
                {
                    List<Assignment> la = devices[devicename].assignments[keyname];
                    foreach (Assignment a in la)
                    {
                        s.AppendFormat("{0} = {1}", keyname, a.ToString());
                        s.AppendLine();
                    }
                }
            }
            return s.ToString();
        }

        string ChkValue(string s)
        {
            return (s == "Value") ? "" : ("." + s);
        }

        public bool LoadBindingsFile()
        {
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
                        System.Diagnostics.Debug.WriteLine("Device " + s + Environment.NewLine + Mappings(s));
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

        public Device FindDevice(string name, Guid instanceguid, Guid productguid )    // best match
        {
            Device bestmatch = null;
            int besttotal = 0;

            foreach( Device dv in devices.Values)
            {
                if (dv.name.Equals(name, StringComparison.InvariantCultureIgnoreCase))      // exact match
                    return dv;

                if (dv.name.Equals(GuidExtract(instanceguid, false), StringComparison.InvariantCultureIgnoreCase))
                    return dv;
                if (dv.name.Equals(GuidExtract(instanceguid, true), StringComparison.InvariantCultureIgnoreCase))
                    return dv;
                if (dv.name.Equals(GuidExtract(productguid, false), StringComparison.InvariantCultureIgnoreCase))
                    return dv;
                if (dv.name.Equals(GuidExtract(productguid, true), StringComparison.InvariantCultureIgnoreCase))
                    return dv;

                int total = dv.name.ToLower().ApproxMatch(name.ToLower(),4);
                if ( total > besttotal )
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

            if ( rev && s.Length == 8)
            {
                s = s.Substring(4, 4) + s.Substring(0, 4);
            }

            return s;
        }
    }
}
