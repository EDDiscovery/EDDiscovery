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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery
{
    public class JSONConverters
    {
        private enum Types
        {
            TScale,     // treat as a double, scale, then print using the string format as per string.Format.  "'text'0.0'more text'"
            TBool,      // Replace a bool/1/0 with true/false value in format is "falsetext;truetext";
            TState,     // if value is empty, use this name in format
            TPrePost,   // prepostfix, format is "Prefix;postfix"   ;postfix can be not given.
            TIndex,     // prepostindex "Prefix;postfix;index offset;value;value;value etc" 
        };

        public delegate string Replacer(string s);

        class Converters
        {
            public Converters(string fn, string nname, Types t , double s, string f, string[] q , Replacer r )
            {
                fieldnames = fn;
                newname = nname;
                converttype = t;
                scale = s;
                format = f;
                formatsplit = f.Split(';');
                eventqual = q == null ? null : new HashSet<string>(q);
                replacer = r;
            }

            public string fieldnames;     // match on any part of this (List searched in backward order)
            public string newname;   // change to this name (optional)
            public Types converttype;
            public double scale;     // scale value by this
            public string format;    // format info
            public string[] formatsplit;
            public HashSet<string> eventqual; // null for none, else has to match eventqual passed in to match
            public Replacer replacer;
        }

        Dictionary<string, List<Converters>> convertersdict;

        public JSONConverters()
        {
            convertersdict = new Dictionary<string, List<Converters>>();
        }

        // name = list of id's to match "one;two;three" or just a single "one"
        // nmane = replace name with this, or null keep name, or "" no name
        // eventq = limit to these events, "one;two;three" or a single "one"

        private void Add(Converters cv)
        {
            foreach (string fn in cv.fieldnames.Split(';'))
            {
                if (!convertersdict.ContainsKey(fn))
                {
                    convertersdict[fn] = new List<Converters>();
                }
                convertersdict[fn].Add(cv);
            }
        }

        public void AddScale(string name, double s, string f = "0.0", string nname = null, string eventq = null)
        {
            Add(new Converters(name, nname, Types.TScale, s, f, (eventq != null) ? eventq.Split(';') : null , null));
        }

        public void AddBool(string name, string falsevalue , string truevalue , string nname = null, string eventq = null)    // converts a true/false bool into these string, with an optional name removal
        {
            Add(new Converters(name, nname, Types.TBool, 0, falsevalue + ";" + truevalue, (eventq != null) ? eventq.Split(';') : null, null));
        }

        public void AddState(string name, string emptyvalue, string nname = null, string eventq = null)    // adds an empty state and allows the name to be removed
        {
            Add(new Converters(name, nname, Types.TState, 0, emptyvalue, (eventq != null) ? eventq.Split(';') : null , null));
        }
        
        public void AddPrePostfix(string name, string prepostfix, string nname = null, string eventq = null, Replacer replacer = null)    // adds an postfix string to the value and allows the name to be removed
        {
            Add(new Converters(name, nname, Types.TPrePost, 0, prepostfix, (eventq != null) ? eventq.Split(';') : null , replacer));
        }
        
        public void AddIndex(string name, string prepostindex, string nname = null, string eventq = null)    // indexer
        {
            Add(new Converters(name, nname, Types.TIndex, 0, prepostindex, (eventq != null) ? eventq.Split(';') : null , null));
        }

        public string Convert(string pname, string value , string eventname)
        {
            string displayname = null;          // null means use default pname, if set, override with own name

            if (convertersdict.ContainsKey(pname))
            {
                List<Converters> cvlist = convertersdict[pname];

                for (int i = cvlist.Count - 1; i >= 0; i--)
                {
                    Converters cv = cvlist[i];

                    if (cv.eventqual != null && !cv.eventqual.Contains(eventname))
                        continue;

                    string[] formatsplit = cv.formatsplit;

                    switch (cv.converttype)
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
                            if (cv.replacer != null)                                            // supports replacer.. call if required.
                                value = cv.replacer(value);

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

                        default:
                            double v = 0;

                            if (double.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out v))        // if it does parse, we can convert it
                                value = (v * cv.scale).ToString(cv.format);
                            break;
                    }

                    if (cv.newname != null)
                    {
                        displayname = cv.newname;
                    }

                    break;
                }
            }

            if ( displayname == null )          // not set by converter, use own name split
                displayname = pname.SplitCapsWord();

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
            removeitems = removeit?.Split(';');
            duplicatepostfixremove = duplist?.Split(';');
            eventtype = eventp;
        }

        private void TrimEnd(StringBuilder sb)
        {
            int end = sb.Length;

            while (end > 0 && char.IsWhiteSpace(sb[end - 1]))
                end--;

            if (end > 0 && sb[end - 1] == ',')
                end--;

            if (end < sb.Length)
                sb.Remove(end, sb.Length - end);
        }

        private void LF(StringBuilder sb, ref int linelen , bool forcelf = false )
        {
            if (( forcelf && sb.Length>0) || sb.Length - linelen > maxlinelen)       // not too many on one line.
            {
                sb.Append(Environment.NewLine);
                linelen = sb.Length;
            }
        }

        bool InDupList(HashSet<string> names, string name)
        {
            foreach (string l in duplicatepostfixremove)
            {
                if (names.Contains(name + l))
                    return true;
            }

            return false;
        }

        public string PrettyPrintStr(string json, int maxlinel)
        {
            try
            {
                JObject jo = JObject.Parse(json);  // Create a clone
                return PrettyPrint(jo, maxlinel);
            }
            catch ( Exception ex )
            {
                return "Report problem to EDDiscovery team, did not print properly: " + ex.Message + ex.StackTrace;
            }
        }

        public string PrettyPrint(JObject jo, int maxlinel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                maxlinelen = maxlinel;
                int linelen = 0;
                int nc = 1;

                HashSet<string> names = new HashSet<string>(jo.Properties().Select(p => p.Name));

                int childcount = jo.Count;
                foreach (JProperty jc in jo.Properties())
                {
                    if (!InDupList(names, jc.Name))
                        ExpandTokens(jc, sb, ref linelen, nc, childcount);

                    nc++;
                }

                TrimEnd(sb);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "Report problem to EDDiscovery team, did not print properly: " + ex.Message + ex.StackTrace;
            }
        }

        private void ExpandTokens(JToken jt, StringBuilder sb, ref int linelen, int childno , int siblings)
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

                int totalchildren = jt is JContainer ? ((JContainer)jt).Count : 0;

                bool isarray = jt is JArray;
                bool isobject = jt is JObject;

                LF(sb, ref linelen);

                if (isarray)            
                {
                    if (totalchildren >= 1 && jt is JObject )
                        LF(sb, ref linelen,true);

                    sb.Append(name);
                    sb.Append("(");
                }
                if (isobject)
                    sb.Append("{");

                int cno = 1;

                JTokenType[] decodeable = { JTokenType.Boolean, JTokenType.Date, JTokenType.Integer, JTokenType.String , JTokenType.Float , JTokenType.TimeSpan };

                foreach (JToken jc in jt.Children())
                {
                    //System.Diagnostics.Trace.WriteLine(string.Format(" >> Child {0} : {1} {2}", cno, jc.Path, jc.Type.ToString()));
                    if (jc.HasValues)
                    {
                        ExpandTokens(jc, sb, ref linelen, cno , totalchildren);
                    }
                    else if ( Array.FindIndex(decodeable,x=>x==jc.Type )!=-1 )
                    {
                        string value = jc.Value<string>();

                        if (jconvertvalue != null)                                  // if converter, pass in to process
                            value = jconvertvalue.Convert(name, value, eventtype);
                        else if ( !isarray )                                        // if no converter, array elements do are not named..
                            value = name + ":" + value;

                        sb.Append(value);
                        sb.Append(", ");
                    }
                    cno++;
                }

                if (isarray)
                {
                    TrimEnd(sb);
                    sb.Append("), ");
                }
                if (isobject)
                {
                    TrimEnd(sb);
                    sb.Append("},");
                    sb.Append(Environment.NewLine);
                }
            }
        }
    }
}
