using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery
{
    public class JSONConverters
    {
        List<string> names;     // match on any part of this (List searched in backward order)
        List<string> newname;   // change to this name (optional)
        List<double> scale;     // scale by this
        List<string> format;    // print in this format (0.0) use 'ls' to put postfix/prefix, or holds 'B;false;true' if boolean conversion
 
        public JSONConverters()
        {
            names = new List<string>();
            newname = new List<string>();
            scale = new List<double>();
            format = new List<string>();
        }

        public void Add(string name, double s, string f = "0.0", string nname = null)
        {
            names.Add(name);
            newname.Add(nname);
            scale.Add(s);
            format.Add(f);
        }

        public void AddBool(string name, string falsevalue , string truevalue , string nname = null)    // converts a true/false bool into these string, with an optional name removal
        {
            names.Add(name);
            newname.Add(nname);
            scale.Add(0);
            format.Add("B;" + falsevalue + ";" + truevalue);
        }

        public void AddState(string name, string emptyvalue, string nname = null)    // adds an empty state and allows the name to be removed
        {
            names.Add(name);
            newname.Add(nname);
            scale.Add(0);
            format.Add("S;" + emptyvalue);
        }

        public void AddPostfix(string name, string postfix, string nname = null)    // adds an postfix string to the value and allows the name to be removed
        {
            names.Add(name);
            newname.Add(nname);
            scale.Add(0);
            format.Add("P;" + postfix);
        }

        public string Convert(string pname, string value , bool noname = false)
        {
            for( int i = names.Count-1; i>=0; i--)
            {
                int offset = pname.IndexOf(names[i]);
                if (offset>=0)
                {
                    if (format[i][0] == 'B')        // BOOLEAN
                    {
                        string[] booleanvalues = format[i].Split(';');

                        bool bv = false;
                        int iv = 0;
                        if (bool.TryParse(value, out bv))
                            value = booleanvalues[(bv) ? 2 : 1];
                        else if (int.TryParse(value, out iv))
                            value = booleanvalues[(iv!=0) ? 2 : 1];
                        else
                            value = booleanvalues[1];       // presume false, may be empty
                    }
                    else if (format[i][0] == 'S')   // State
                    {
                        string[] statevalues = format[i].Split(';');

                        if (value.Length == 0)
                            value = statevalues[1];
                    }
                    else if (format[i][0] == 'P')   // Postfix
                    {
                        string[] statevalues = format[i].Split(';');

                        if ( !value.Contains(statevalues[1]))       // don't repeat
                            value += statevalues[1];
                    }
                    else
                    {                               // VALUE, presume double
                        double v = 0;

                        if (double.TryParse(value, out v))        // if it does parse, we can convert it
                            value = (v * scale[i]).ToString(format[i]);
                    }

                    if (newname[i] != null)
                    {
                        if (offset == 0 )                // if its the whole of it, replace whole
                            pname = newname[i];
                        else
                            pname = Tools.SplitCapsWord( pname.Substring(0, offset) + newname[i] );  // else replace just the end part..
                    }
                    else
                        pname = Tools.SplitCapsWord(pname);

                    break;
                }
            }


            string ret = ((pname.Length > 0 && !noname) ? pname + ":" : "") + value;

            return ret;
        }

        public static JSONConverters StandardConverters()
        {
            JSONConverters jc = new JSONConverters();
            jc.Add("ArrivalLS", 1.0, "0.0'ls'","Arrival");
            jc.Add("MassEM", 1.0, "0.0'em'", "Mass");
            jc.Add("MassMT", 1.0, "0.0'mt'", "Mass");
            jc.Add("SurfacePressure", 1.0, "0.0'p'");
            jc.Add("Radius", 1.0 / 1000, "0.0'km'");
            jc.Add("InnerRad", 1.0 / 1000, "0.0'km'", "Inner Radius");
            jc.Add("OuterRad", 1.0 / 1000, "0.0'km'", "Outer Radius");
            jc.Add("OrbitalPeriod", 1.0 / 86400, "0.0' days'");
            jc.Add("RotationPeriod", 1.0 / 86400, "0.0' days'");
            jc.Add("SurfaceGravity", 1.0 / 9.8, "0.0'g'");
            jc.Add("SurfaceTemperature", 1.0, "0.0'K'");
            jc.AddBool("TidalLock", "Not Tidally Locked", "Tidally Locked", ""); // remove name
            jc.AddBool("Landable", "Not Landable", "Landable", ""); // remove name
            jc.AddState("TerraformState", "Not Terrraformable", "");    // remove name
            jc.AddState("Atmosphere", "No Atmosphere", "");
            jc.AddState("Volcanism", "No Volcanism", "");
            jc.AddPostfix("StationType", " Type", "");
            jc.AddPostfix("StationName", " Station", "");
            jc.AddPostfix("StarSystem", " Star System", "");
            jc.AddPostfix("Allegiance", " Allegiance", "");
            jc.AddPostfix("Security", " Security", "");
            jc.AddPostfix("Faction", " Faction", "");
            jc.AddPostfix("Government", " Government", "");
            jc.AddPostfix("Economy", " Economy", "");
            jc.AddBool("Docked", "Not Docked", "Docked", "");   // remove name
            return jc;
        }

    };

    public class JSONPrettyPrint
    {
        private string[] removeitems;
        private string[] duplicatepostfixremove;
        private JSONConverters jconvertvalue;
        private int maxlinelen;

        public JSONPrettyPrint( JSONConverters c , string r , string d )
        {
            jconvertvalue = c;
            removeitems = r.Split(';');
            duplicatepostfixremove = d.Split(';');
        }

        private string TrimEnd(string outstr)
        {
            outstr = outstr.TrimEnd();
            if (outstr.Length > 0 && outstr[outstr.Length - 1] == ',')
                outstr = outstr.Substring(0, outstr.Length - 1);

            return outstr;
        }

        private void LF(ref string str, ref int linelen , bool forcelf = false )
        {
            if (( forcelf && str.Length>0) || str.Length - linelen > maxlinelen)       // not too many on one line.
            {
                str += Environment.NewLine;
                linelen = str.Length;
            }
        }

        bool InDupList(JToken jt, string name)
        {
            foreach (string l in duplicatepostfixremove)
            {
                foreach( JToken jc in jt.Children())
                {
                    if (jc.Path.Contains(name + l))
                        return true;
                }
            }
            return false;
        }

        public string PrettyPrint(string json , int maxlinel = 40)
        {
            string outstr = "";

            try
            {
                maxlinelen = maxlinel;
                JObject jo = JObject.Parse(json);  // Create a clone
                int linelen = 0;
                int nc = 1;
                foreach (JToken jc in jo.Children())
                {
                    if ( !InDupList(jo,jc.Path))
                        ExpandTokens(jc, ref outstr, ref linelen, nc, jo.Children().Count());

                    nc++;
                }

                outstr = TrimEnd(outstr);
            }
            catch (Exception)
            {
                outstr = "Report problem to EDDiscovery team, did not print properly";
            }

            return outstr;
        }

        private void ExpandTokens(JToken jt, ref string outstr, ref int linelen, int childno , int siblings)
        {
            if (jt.HasValues)
            {
                string name = jt.Path;

                if (removeitems.Contains(name))                 // don't print these
                    return;

                int dot = name.IndexOf('.');                            // any dot notation remove
                if (dot >= 0)
                    name = name.Substring(dot + 1);

                foreach (string l in duplicatepostfixremove)        // see if its a duplicate, if so, remove the postfix
                {
                    int localisedindex = name.IndexOf(l);

                    if (localisedindex >= 0)
                    {
                        name = name.Substring(0, localisedindex);     // cut out all past there.
                        break;
                    }
                }

                //System.Diagnostics.Trace.WriteLine(string.Format("{0}", jt.Type.ToString()));
                //System.Diagnostics.Trace.WriteLine("   " + childno + " : " + siblings + " : " + name + ":" + jt.Type.ToString() + "!");

                int totalchildren = jt.Children().Count();

                bool isarray = jt is JArray;
                bool isobject = jt is JObject;

                LF(ref outstr, ref linelen);

                if (isarray)
                {
                    if (totalchildren >= 1 && jt.Children().First() is JObject )
                        LF(ref outstr, ref linelen,true);

                    outstr += name + "(";
                }
                if (isobject)
                    outstr += "{";

                int cno = 1;

                foreach (JToken jc in jt.Children())
                {
                    //System.Diagnostics.Trace.WriteLine(string.Format(" >> {0} : {1}", cno, jc.Type.ToString()));
                    if (jc.HasValues)
                    {
                        ExpandTokens(jc, ref outstr, ref linelen, cno , totalchildren);
                    }
                    else
                    {
                        string value = jc.Value<string>();

                        if (jconvertvalue != null)
                            value = jconvertvalue.Convert(name, value, isarray);
                        else if ( !isarray )
                            value = name + ":" + value;

                        outstr +=  value + ", ";
                    }
                    cno++;
                }

                if (isarray)
                    outstr = TrimEnd(outstr) + "), ";
                if (isobject)
                    outstr = TrimEnd(outstr) + "}," + Environment.NewLine;
            }
        }
    }
}
