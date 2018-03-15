using BaseUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetLogEntry
{
    public static class BindingsFile
    {
        public static void Bindings(string filename)
        {
            using (Stream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                List<string> bindings = new List<string>();
                List<string> say = new List<string>();
                List<string> saydef = new List<string>();

                using (StreamReader sr = new StreamReader(fs))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        int i = s.IndexOf("KEY ", StringComparison.InvariantCultureIgnoreCase);
                        if (i >= 0 && i < 16)
                        {
                            s = s.Substring(i + 4).Trim();
                            if (!bindings.Contains(s))
                                bindings.Add(s);
                        }
                        i = s.IndexOf("Say ", StringComparison.InvariantCultureIgnoreCase);
                        if (i >= 0 && i < 16)
                        {
                            s = s.Substring(i + 4).Trim();
                            if (!say.Contains(s))
                                say.Add(s);
                        }
                        i = s.IndexOf("Static say_", StringComparison.InvariantCultureIgnoreCase);
                        if (i >= 0 && i < 16)
                        {
                            //Console.WriteLine("saw " + s);
                            s = s.Substring(i + 7).Trim();
                            i = s.IndexOf(" ");
                            if (i >= 0)
                                s = s.Substring(0, i);
                            if (!saydef.Contains(s))
                                saydef.Add(s);
                        }
                    }
                }

                bindings.Sort();

                Console.WriteLine("*** Bindings:");
                foreach (string s in bindings)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine("*** Say definitions:");
                foreach (string s in saydef)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine("*** Say commands:");
                foreach (string s in say)
                {
                    Console.WriteLine(s);
                }
            }
        }


        public static void DeviceMappings(string filename)
        {
            try
            {
                XElement bindings = XElement.Load(filename);

                System.Diagnostics.Debug.WriteLine("Top " + bindings.NodeType + " " + bindings.Name);

                Console.WriteLine("Dictionary<Tuple<int, int>, string> ctrls = new Dictionary<Tuple<int, int>, string>()" + Environment.NewLine + "{" + Environment.NewLine);

                foreach (XElement x in bindings.Elements())
                {
                    string ctrltype = x.Name.LocalName;
                    List<Tuple<int, int>> pv = new List<Tuple<int, int>>();

                    int pid = 0;
                    int vid = 0;

                    int.TryParse(x.Element("PID").Value, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out pid);
                    int.TryParse(x.Element("VID").Value, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out vid);

                    pv.Add(new Tuple<int, int>(pid, vid));

                    foreach (XElement y in x.Elements())
                    {
                        if (y.Name.LocalName.Equals("Alternative"))
                        {
                            int.TryParse(y.Element("PID").Value, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out pid);
                            int.TryParse(y.Element("VID").Value, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out vid);

                            pv.Add(new Tuple<int, int>(pid, vid));
                        }
                    }

                    System.Diagnostics.Debug.WriteLine("Ctrl " + ctrltype);
                    foreach (Tuple<int, int> v in pv)
                        System.Diagnostics.Debug.WriteLine("  " + v.Item1.ToString("x") + " " + v.Item2.ToString("x"));

                    foreach (Tuple<int, int> v in pv)
                    {
                        System.Diagnostics.Debug.WriteLine("  " + v.Item1.ToString("x") + " " + v.Item2.ToString("x"));
                        Console.WriteLine("     {  new Tuple<int,int>(0x" + v.Item1.ToString("X") + ", 0x" + v.Item2.ToString("X") + "), \"" + ctrltype + "\" },");
                    }
                }

                Console.WriteLine("};");
            }

            catch
            {

            }

            //example..
            Dictionary<Tuple<int, int>, string> ct2rls = new Dictionary<Tuple<int, int>, string>()
            {
                { new Tuple<int,int>(1,1), "Fred" },
            };
        }


    }
}
