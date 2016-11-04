using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery
{
    public class JSONConverters
    {
        public enum Types
        {
            TScale,     // treat as a double, scale, then print using the string format as per string.Format.  "'text'0.0'more text'"
            TBool,      // Replace a bool/1/0 with true/false value in format is "falsetext;truetext";
            TState,     // if value is empty, use this name in format
            TPrePost,   // prepostfix, format is "Prefix;postfix"   ;postfix can be not given.
            TIndex,     // prepostindex "Prefix;postfix;index offset;value;value;value etc" 
            TLat,       // format as a lat 10 N/S degree 20'30. format can be empty or prefix;postfix as above
            TLong,      // format as a long 10 E/W degree 20'30. format can be empty or prefix;postfix as above
            TMaterialCommodity, // see if the stupid fdname can be resolved to something better.  format can be empty or prefix;postfix as above
        };

        struct Converters
        {
            public Converters(string fn, string nname, Types t , double s, string f, string[] q )
            {
                fieldnames = fn;
                newname = nname;
                converttype = t;
                scale = s;
                format = f;
                eventqual = q;
            }

            public string fieldnames;     // match on any part of this (List searched in backward order)
            public string newname;   // change to this name (optional)
            public Types converttype;
            public double scale;     // scale value by this
            public string format;    // format info
            public string[] eventqual; // null for none, else has to match eventqual passed in to match
        }

        List<Converters> converters;

        public JSONConverters()
        {
            converters = new List<Converters>();
        }

        // name = list of id's to match "one;two;three" or just a single "one"
        // nmane = replace name with this, or null keep name, or "" no name
        // eventq = limit to these events, "one;two;three" or a single "one"

        public void AddScale(string name, double s, string f = "0.0", string nname = null, string eventq = null)
        {
            converters.Add(new Converters(name, nname, Types.TScale, s, f, (eventq != null) ? eventq.Split(';') : null));
        }

        public void AddBool(string name, string falsevalue , string truevalue , string nname = null, string eventq = null)    // converts a true/false bool into these string, with an optional name removal
        {
            converters.Add(new Converters(name, nname, Types.TBool, 0, falsevalue + ";" + truevalue, (eventq != null) ? eventq.Split(';') : null));
        }

        public void AddState(string name, string emptyvalue, string nname = null, string eventq = null)    // adds an empty state and allows the name to be removed
        {
            converters.Add(new Converters(name, nname, Types.TState, 0, emptyvalue, (eventq != null) ? eventq.Split(';') : null));
        }
        
        public void AddPrePostfix(string name, string prepostfix, string nname = null, string eventq = null)    // adds an postfix string to the value and allows the name to be removed
        {
            converters.Add(new Converters(name, nname, Types.TPrePost, 0, prepostfix, (eventq != null) ? eventq.Split(';') : null));
        }
        
        public void AddIndex(string name, string prepostindex, string nname = null, string eventq = null)    // indexer
        {
            converters.Add(new Converters(name, nname, Types.TIndex, 0, prepostindex, (eventq != null) ? eventq.Split(';') : null));
        }

        public void AddSpecial(string name, Types t, string format, string nname = null, string eventq = null)    // indexer
        {
            converters.Add(new Converters(name, nname, t, 0, format, (eventq != null) ? eventq.Split(';') : null));
        }

        public string Convert(string pname, string value , string eventname)
        {
            string displayname = Tools.SplitCapsWord(pname);

            for ( int i = converters.Count-1; i>=0; i--)
            {
                string[] ids = converters[i].fieldnames.Split(';');

                if (Array.FindIndex(ids, x => x.Equals(pname)) != -1 && (converters[i].eventqual ==null ||  Array.FindIndex(converters[i].eventqual,x=>x.Equals(eventname))!=-1 ))
                {
                    string[] formatsplit = converters[i].format.Split(';');

                    switch (converters[i].converttype)
                    {
                        case Types.TBool:
                            bool bv = false;
                            int iv = 0;
                            if (bool.TryParse(value, out bv))
                                value = formatsplit[(bv) ? 1 : 0];
                            else if (int.TryParse(value, out iv))
                                value = formatsplit[(iv != 0) ? 1 : 0];
                            else
                                value = formatsplit[0];       // presume false, may be empty
                            break;

                        case Types.TState:
                            if (value.Length == 0)
                                value = formatsplit[0];
                            break;

                        case Types.TPrePost:
                            if (formatsplit.Length >= 1 && !value.Contains(formatsplit[0]))       // don't repeat
                                value = formatsplit[0] + value;
                            if (formatsplit.Length >= 2 && !value.Contains(formatsplit[1]))       // don't repeat
                                value += formatsplit[1];
                            break;

                        case Types.TIndex:
                            int ix = 0, offset = 0;
                            if (int.TryParse(value, out ix) && formatsplit.Length >= 4 && int.TryParse(formatsplit[2], out offset))
                            {
                                if (ix >= offset && ix < offset + formatsplit.Length - 3)
                                {
                                    value = formatsplit[0] + formatsplit[ix - offset + 3] + formatsplit[1];
                                }
                            }
                            break;

                        case Types.TLat:
                        case Types.TLong:
                            double lv;
                            if (double.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out lv))        // if it does parse, we can convert it
                            {
                                long arcsec = (long)(lv * 60 * 60);          // convert to arc seconds

                                string marker = (arcsec < 0) ? "S" : "N";       // presume lat
                                if (converters[i].converttype == Types.TLong )
                                    marker = (arcsec < 0) ? "W" : "E";       // presume lat
                                arcsec = Math.Abs(arcsec);
                                value = string.Format("{0}°{1} {2}'{3}\"", arcsec / 3600, marker, (arcsec / 60) % 60, arcsec % 60 );
                                if (formatsplit.Length >= 2)
                                    value = formatsplit[0] + value + formatsplit[1];
                            }
                            break;

                        case Types.TMaterialCommodity:
                            EDDiscovery2.DB.MaterialCommodities mc = EDDiscovery2.DB.MaterialCommodities.GetCachedMaterial(value);
                            if (mc != null)
                                value = mc.name;

                            if (formatsplit.Length >= 2)
                                value = formatsplit[0] + value + formatsplit[1];

                            break;

                        default:
                            double v = 0;

                            if (double.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out v))        // if it does parse, we can convert it
                                value = (v * converters[i].scale).ToString(converters[i].format);
                            break;
                    }

                    if (converters[i].newname != null)
                    {
                        displayname = converters[i].newname;
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
