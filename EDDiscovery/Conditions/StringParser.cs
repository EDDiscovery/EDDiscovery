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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public class StringParser
    {
        private int pos;        // always left after an operation on the next non space char
        private string line;

        public StringParser(string l)
        {
            line = l;
            pos = 0;
            SkipSpace();
        }

        public int Position { get { return pos; } }
        public string LineLeft { get { return line.Substring(pos); } }
        public bool IsEOL { get { return pos == line.Length; } }

        public bool IncSkipSpace()
        {
            if (pos < line.Length)
                pos++;

            return SkipSpace();
        }

        public bool SkipSpace()
        {
            while (pos < line.Length && char.IsWhiteSpace(line[pos]))
                pos++;

            return pos == line.Length;
        }

        public char PeekChar()
        {
            return (pos < line.Length) ? line[pos] : ' ';
        }

        public bool IsCharMoveOn(char t)
        {
            if (pos < line.Length && line[pos] == t)
            {
                pos++;
                SkipSpace();
                return true;
            }
            else
                return false;
        }

        public string NextWord(string terminators = " " , bool lowercase = false)
        {
            if (pos >= line.Length)     // null if there is nothing..
                return null;
            else
            {
                int start = pos;

                while (pos < line.Length && terminators.IndexOf(line[pos]) == -1)
                    pos++;

                string ret = line.Substring(start, pos - start);

                SkipSpace();

                return (lowercase) ? ret.ToLower() : ret;
            }
        }

        public string NextQuotedWord(string nonquoteterminators = " ")
        {
            if (pos < line.Length)
            {
                if (line[pos] == '"')
                {
                    string ret = "";
                    pos++;

                    while (true)
                    {
                        int nextslash = line.IndexOf("\\", pos);
                        int nextquote = line.IndexOf('"', pos);

                        if (nextslash >= 0 && nextslash < nextquote)        // slash first..
                        {
                            if (nextslash + 1 >= line.Length)               // slash at end of line, uhoh
                                return null;

                            if (line[nextslash + 1] == '"')                 // if \", its just a "
                                ret += line.Substring(pos, nextslash - pos) + "\""; // copy up to slash, but not the slash, then add the quote
                            else
                                ret += line.Substring(pos, nextslash + 2 - pos);    // copy all, include the next char

                            pos = nextslash + 2;                        // and skip over the slash and the next char
                        }
                        else if (nextquote == -1)                     // must have a quote somewhere..
                            return null;
                        else
                        {
                            ret += line.Substring(pos, nextquote - pos);    // quote, end of line, copy up and remove it
                            pos = nextquote + 1;
                            SkipSpace();
                            return ret;
                        }
                    }
                }
                else
                    return NextWord(nonquoteterminators);
            }
            else
                return null;
        }

        public List<string> NextQuotedWordList()        // empty list on error
        {
            List<string> ret = new List<string>();

            do
            {
                string v = NextQuotedWord(", ");
                if (v == null)
                    return null;

                ret.Add(v);

                if (!IsEOL && !IsCharMoveOn(','))   // either EOL, or its a comma matey
                    return null;

            } while (!IsEOL);

            return ret;
        }

        public List<string> NextOptionallyBracketedList()       // empty list on error
        {
            List<string> sl = new List<string>();
            if (pos < line.Length)
            {
                if (IsCharMoveOn('('))  // if (, we go multi bracketed
                {
                    while (true)
                    {
                        string s = NextQuotedWord("), ");
                        if (s == null) // failed to get a word, error
                        {
                            sl.Clear();
                            break;
                        }
                        else
                            sl.Add(s);

                        if (IsCharMoveOn(')'))      // ), end of word list, move over and stop
                            break;
                        else if (!IsCharMoveOn(','))    // must be ,
                        {
                            sl.Clear();     // cancel list and stop, error
                            break;
                        }
                    }
                }
                else
                {
                    string s = NextQuotedWord("), ");
                    if (s != null)
                        sl.Add(s);
                }
            }

            return sl;
        }

        public int? GetInt(string terminators = ", ")
        {
            string s = NextWord(terminators);
            int i;
            if (s != null && s.InvariantParse(out i))
                return i;
            else
                return null;
        }

        public long? GetLong(string terminators = ", ")
        {
            string s = NextWord(terminators);
            long i;
            if (s != null && s.InvariantParse(out i))
                return i;
            else
                return null;
        }

    }

}
