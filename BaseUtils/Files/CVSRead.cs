/*
 * Copyright © 2017 EDDiscovery development team
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
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public class CVSRead
    {
        StreamReader indata;

        public CVSRead(StreamReader s)
        {
            indata = s;
        }

        public enum State { EOF, Item, ItemEOL };

        public State Next( out string s )
        {
            s = "";

            if (indata.EndOfStream)
                return State.EOF;

            int c;

            if ( indata.Peek() == '"')
            {
                indata.Read();

                while ((c = indata.Read()) != -1 && (c != '"' || indata.Peek() == '"'))
                {
                    //System.Diagnostics.Debug.WriteLine("Char " + c);
                    if ( c == '"')  // ""
                    {
                        indata.Read();
                        s += (char)c;
                    }
                    else if (c == '\r')
                    {
                        s += "\r\n";
                    }
                    else 
                    {
                        s += (char)c;
                    }
                }
            }
            else
            {
                while ((c = indata.Peek()) != -1 && c != ',' && c != '\r')
                {
                    //System.Diagnostics.Debug.WriteLine("NChar " + c);
                    s += (char)c;
                    indata.Read();
                }
            }

            int e = indata.Read();
            //System.Diagnostics.Debug.WriteLine("SChar " + e);

            if (e == '\r')
            {
                e = indata.Read();
                return State.ItemEOL;
            }
            else
                return State.Item;
        }

        void RemoveSpaces()
        {
            while ((char)indata.Peek() == ' ')
                indata.Read();
        }
    }

    public class CVSFile
    {
        public class Line
        {
            public Line()
            {
                cells = new List<string>();
            }

            public List<string> cells;
        }

        public List<Line> rows;

        public bool Read(string file)
        {
            rows = new List<Line>();

            try
            {
                using (Stream s = File.OpenRead(file))
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        CVSRead cvs = new CVSRead(sr);

                        CVSRead.State st;

                        Line l = new Line();

                        string str;
                        while ((st = cvs.Next(out str)) != CVSRead.State.EOF)
                        {
                            l.cells.Add(str);

                            if (st == CVSRead.State.ItemEOL)
                            {
                                rows.Add(l);
                                l = new Line();
                            }
                        }

                        if (l.cells.Count > 0)
                            rows.Add(l);

                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}


