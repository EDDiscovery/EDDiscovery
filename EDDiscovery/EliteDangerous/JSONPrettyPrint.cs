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
        List<string[]> eventqual; // null for none, else has to match eventqual passed in to match

        public JSONConverters()
        {
            names = new List<string>();
            newname = new List<string>();
            scale = new List<double>();
            format = new List<string>();
            eventqual = new List<string[]>();
        }

        // name = list of id's to match "one;two;three" or just a single "one"
        // nmane = replace name with this, or null keep name, or "" no name
        // eventq = limit to these events, "one;two;three" or a single "one"

        // Scale - f can be a full format string "'hello' 0.0 'postfix'"
        public void AddScale(string name, double s, string f = "0.0", string nname = null, string eventq = null)
        {
            names.Add(name);
            newname.Add(nname);
            scale.Add(s);
            format.Add(f);
            eventqual.Add((eventq != null) ? eventq.Split(';') : null);
        }

        public void AddBool(string name, string falsevalue , string truevalue , string nname = null, string eventq = null)    // converts a true/false bool into these string, with an optional name removal
        {
            names.Add(name);
            newname.Add(nname);
            scale.Add(0);
            format.Add("B;" + falsevalue + ";" + truevalue);
            eventqual.Add((eventq != null) ? eventq.Split(';') : null);
        }

        public void AddState(string name, string emptyvalue, string nname = null, string eventq = null)    // adds an empty state and allows the name to be removed
        {
            names.Add(name);
            newname.Add(nname);
            scale.Add(0);
            format.Add("S;" + emptyvalue);
            eventqual.Add((eventq != null) ? eventq.Split(';') : null);
        }

        // prepostfix "Prefix;postfix"   ;postfix can be not given.
        public void AddPrePostfix(string name, string prepostfix, string nname = null, string eventq = null)    // adds an postfix string to the value and allows the name to be removed
        {
            names.Add(name);
            newname.Add(nname);
            scale.Add(0);
            format.Add("P;" + prepostfix);
            eventqual.Add((eventq != null) ? eventq.Split(';') : null);
        }

        // prepostindex "Prefix;postfix;index offset;value;value;value etc" 
        public void AddIndex(string name, string prepostindex, string nname = null, string eventq = null)    // indexer
        {
            names.Add(name);
            newname.Add(nname);
            scale.Add(0);
            format.Add("I;" + prepostindex);
            eventqual.Add((eventq != null) ? eventq.Split(';') : null);
        }


        public string Convert(string pname, string value , string eventname)
        {
            string displayname = Tools.SplitCapsWord(pname);

            for ( int i = names.Count-1; i>=0; i--)
            {
                string[] ids = names[i].Split(';');

                if (Array.FindIndex(ids, x => x.Equals(pname)) != -1 && (eventqual[i] ==null ||  Array.FindIndex(eventqual[i],x=>x.Equals(eventname))!=-1 ))
                {
                    if (format[i][0] == 'B')        // BOOLEAN
                    {
                        string[] booleanvalues = format[i].Split(';');

                        bool bv = false;
                        int iv = 0;
                        if (bool.TryParse(value, out bv))
                            value = booleanvalues[(bv) ? 2 : 1];
                        else if (int.TryParse(value, out iv))
                            value = booleanvalues[(iv != 0) ? 2 : 1];
                        else
                            value = booleanvalues[1];       // presume false, may be empty
                    }
                    else if (format[i][0] == 'S')   // State
                    {
                        string[] statevalues = format[i].Split(';');

                        if (value.Length == 0)
                            value = statevalues[1];
                    }
                    else if (format[i][0] == 'P')   // pre-Postfix
                    {
                        string[] statevalues = format[i].Split(';');

                        if (statevalues.Length >= 2 && !value.Contains(statevalues[1]))       // don't repeat
                            value = statevalues[1] + value;
                        if (statevalues.Length >= 3 && !value.Contains(statevalues[2]))       // don't repeat
                            value += statevalues[2];
                    }
                    else if (format[i][0] == 'I')   // index
                    {
                        string[] statevalues = format[i].Split(';');        // 0 = I, 1 = pre, 2 = post, 3 = offset, 4 = index from 0 on

                        int iv = 0,offset = 0;
                        if (int.TryParse(value, out iv) && statevalues.Length>=5 && int.TryParse(statevalues[3],out offset))
                        {
                            if ( iv >= offset && iv < offset + statevalues.Length - 4)
                            {
                                value = statevalues[1] + statevalues[iv - offset + 4] + statevalues[2];
                            }
                        }
                    }
                    else
                    {                               // VALUE, presume double
                        double v = 0;

                        if (double.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out v))        // if it does parse, we can convert it
                            value = (v * scale[i]).ToString(format[i]);
                    }

                    if (newname[i] != null)
                    {
                        displayname = newname[i];
                    }

                    break;
                }
            }

            //System.Diagnostics.Trace.WriteLine(string.Format("{0} {1} ", displayname , value ));

            string ret = ((displayname.Length > 0) ? displayname + ":" : "") + value;

            return ret;
        }

    };

    public class JSONPrettyPrint                        // THIS is a generic JSON pretty print - keep journals out of it..
    {
        private string[] removeitems;                   // never to be printed
        private string[] duplicatepostfixremove;        // for _localised strings
        private JSONConverters jconvertvalue;           // converter
        private int maxlinelen;
        private string eventtype;                       // the event type string ("SellExplorationData")

        public JSONPrettyPrint( JSONConverters c , string removeit , string duplist , string eventp)
        {
            jconvertvalue = c;
            removeitems = removeit.Split(';');
            duplicatepostfixremove = duplist.Split(';');
            eventtype = eventp;
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

        public string PrettyPrint(string json , int maxlinel)
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
            //System.Diagnostics.Trace.WriteLine("parent JT " + jt.Path + " is a " + jt.Type.ToString());
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

                JTokenType[] decodeable = { JTokenType.Boolean, JTokenType.Date, JTokenType.Integer, JTokenType.String , JTokenType.Float , JTokenType.TimeSpan };

                foreach (JToken jc in jt.Children())
                {
                    //System.Diagnostics.Trace.WriteLine(string.Format(" >> Child {0} : {1} {2}", cno, jc.Path, jc.Type.ToString()));
                    if (jc.HasValues)
                    {
                        ExpandTokens(jc, ref outstr, ref linelen, cno , totalchildren);
                    }
                    else if ( Array.FindIndex(decodeable,x=>x==jc.Type )!=-1 )
                    {
                        string value = jc.Value<string>();

                        if (jconvertvalue != null)                                  // if converter, pass in to process
                            value = jconvertvalue.Convert(name, value, eventtype);
                        else if ( !isarray )                                        // if no converter, array elements do are not named..
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
