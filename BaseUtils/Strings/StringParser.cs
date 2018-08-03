/*
 * Copyright © 2018 EDDiscovery development team
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    [System.Diagnostics.DebuggerDisplay("Action {line.Substring(pos)} : ({line})")]
    public class StringParser
    {
        private int pos;        // always left after an operation on the next non space char
        private string line;

        #region Init and basic status

        public StringParser(string l, int p = 0)
        {
            line = l;
            pos = p;
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

        public char GetChar()       // or z\0 if not EOL
        {
            return (pos < line.Length) ? line[pos++] : char.MinValue;
        }

        public bool IsStringMoveOn(string s, StringComparison sc = StringComparison.InvariantCulture)
        {
            if (line.Substring(pos).StartsWith(s,sc))
            {
                pos += s.Length;
                SkipSpace();
                return true;
            }
            else
                return false;
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

        public bool IsAnyCharMoveOn(string t)   // any char in t is acceptable
        {
            if (pos < line.Length && t.Contains(line[pos]))
            {
                pos++;
                SkipSpace();
                return true;
            }
            else
                return false;
        }

        public bool IsCharMoveOnOrEOL(char t) // if at EOL, or separ is space (space is auto removed so therefore okay) or separ (and move)
        {
            return IsEOL || t == ' ' || IsCharMoveOn(t);       
        }

        public bool SkipUntil( char[] chars)
        {
            while (pos < line.Length && Array.IndexOf(chars, line[pos]) == -1)
                pos++;

            return pos < line.Length;
        }

        #endregion

        #region WORDs bare or quoted

        // WORD defined by terminators. options to lowercase it and de-escape it

        public string NextWord(string terminators = " ", System.Globalization.CultureInfo lowercase = null, bool replacescape = false)
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

                if (lowercase!=null)
                    ret = ret.ToLower(lowercase);

                return (replacescape) ? ret.ReplaceEscapeControlChars() : ret;
            }
        }

        public string NextWordLCInvariant(string terminators = " ", bool replaceescape = false)
        {
            return NextWord(terminators, lowercase: System.Globalization.CultureInfo.InvariantCulture, replacescape:replaceescape);
        }

        // NextWord with a fixed space comma (or other) terminator.  Fails if not a separ list

        public string NextWordComma(System.Globalization.CultureInfo lowercase = null, bool replaceescape = false, char separ = ',')
        {
            string res = NextWord(" " + separ, lowercase, replaceescape);
            return IsCharMoveOnOrEOL(separ) ? res : null;
        }

        public string NextWordCommaLCInvariant(bool replaceescape = false, char separ = ',')    // nicer quicker way to specify
        {
            return NextWordComma(System.Globalization.CultureInfo.InvariantCulture, replaceescape, separ);
        }

        // Take a " or ' quoted string, or a WORD defined by terminators. options to lowercase it and de-escape it

        public string NextQuotedWord(string nonquoteterminators = " ", System.Globalization.CultureInfo lowercase = null, bool replaceescape = false)
        {
            if (pos < line.Length)
            {
                if (line[pos] == '"' || line[pos] == '\'')
                {
                    char quote = line[pos++];

                    string ret = "";
                    
                    while (true)
                    {
                        int nextslash = line.IndexOf("\\", pos);
                        int nextquote = line.IndexOf(quote, pos);

                        if (nextslash >= 0 && nextslash < nextquote)        // slash first..
                        {
                            if (nextslash + 1 >= line.Length)               // slash at end of line, uhoh
                                return null;

                            if (line[nextslash + 1] == quote)                 // if \", its just a "
                                ret += line.Substring(pos, nextslash - pos) + quote; // copy up to slash, but not the slash, then add the quote
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

                            if (lowercase != null)
                                ret = ret.ToLower(lowercase);

                            return (replaceescape) ? ret.ReplaceEscapeControlChars() : ret;
                        }
                    }
                }
                else
                    return NextWord(nonquoteterminators, lowercase, replaceescape);
            }
            else
                return null;
        }

        // NextQuotedWord with a fixed space comma terminator.  Fails if not a comma separ list

        public string NextQuotedWordComma(System.Globalization.CultureInfo lowercase = null, bool replaceescape = false, char separ = ',' )           // comma separ
        {
            string res = NextQuotedWord(" " + separ, lowercase, replaceescape);
            return IsCharMoveOnOrEOL(separ) ? res : null;
        }

        // if quoted, take the quote string, else take the rest, space stripped.

        public string NextQuotedWordOrLine(System.Globalization.CultureInfo lowercase = null, bool replaceescape = false)
        {
            if (pos < line.Length)
            {
                if (line[pos] == '"' || line[pos] == '\'')
                    return NextQuotedWord("", lowercase, replaceescape);
                else
                {
                    string ret = line.Substring(pos).Trim();
                    pos = line.Length;

                    if (lowercase != null)
                        ret = ret.ToLower(lowercase);

                    return (replaceescape) ? ret.ReplaceEscapeControlChars() : ret;
                  }
            }
            else
                return null;
        }

        #endregion

        #region List of quoted words

        // Read a list of optionally quoted strings, seperated by separ char.  Stops at EOL or on error.  Check IsEOL if you care about an Error

        public List<string> NextQuotedWordListSepar(System.Globalization.CultureInfo lowercase = null, bool replaceescape = false, char separ = ',')
        {
            List<string> list = new List<string>();

            string r;
            while ((r = NextQuotedWordComma(lowercase, replaceescape, separ)) != null)
            {
                list.Add(r);
            }

            return list;
        }

        // Read a quoted word list off, supporting multiple separ chars, and with multiple other terminators, and the lowercard/replaceescape options
        // null list on error
        public List<string> NextQuotedWordList(System.Globalization.CultureInfo lowercase = null, bool replaceescape = false , 
                                        string separchars = ",", string otherterminators = " ", bool separoptional = false)
        {
            List<string> ret = new List<string>();

            do
            {
                string v = NextQuotedWord(separchars + otherterminators,lowercase,replaceescape);
                if (v == null)
                    return null;

                ret.Add(v);

                if (separoptional)  // if this is set, we these are optional and if not present won't bork it.
                {
                    IsAnyCharMoveOn(separchars);   // remove it if its there
                }
                else if (!IsEOL && !IsAnyCharMoveOn(separchars))   // either not EOL, or its not a terminator, fail
                    return null;

            } while (!IsEOL);

            return ret;
        }

        // Read a quoted word list off, with a ) termination

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

        #endregion

        #region optional Brackets quoted word... word or "word" or ( ("group of words" txt "words" txt ) ) just like a c# expression

        // returns a tuple list, bool = true if string, false if text 
        public List<Tuple<string,bool>> NextOptionallyBracketedQuotedWords(System.Globalization.CultureInfo lowercase = null, bool replacescape = false)       // null in error
        {
            if (pos < line.Length)
            {
                if (IsCharMoveOn('('))  // if (, we go multi bracketed
                {
                    List<Tuple<string, bool>> slist = new List<Tuple<string, bool>>();
                    string acc = "";
                    int bracketlevel = 1;

                    while (pos < line.Length)
                    {
                        if (line[pos] == '"' || line[pos] == '\'')
                        {
                            if (acc.Length > 0)
                            {
                                slist.Add(new Tuple<string, bool>(acc, false));
                                acc = "";
                            }

                            string s = NextQuotedWord(lowercase: lowercase, replaceescape: replacescape);
                            if (s == null)
                                return null;

                            slist.Add(new Tuple<string, bool>(s, true));
                        }
                        else if (IsCharMoveOn(')'))
                        {
                            if (--bracketlevel == 0)
                            {
                                if (acc.Length > 0)
                                    slist.Add(new Tuple<string, bool>(acc, false));
                                return slist;
                            }
                        }
                        else if (IsCharMoveOn('('))
                        {
                            bracketlevel++;
                        }
                        else
                            acc += line[pos++];
                    }
                }
                else
                {
                    string s = NextQuotedWord(lowercase: lowercase, replaceescape: replacescape);
                    if ( s != null )
                        return new List<Tuple<string,bool>>() { new Tuple<string,bool>(s,true) };
                }
            }

            return null;
        }

        #endregion

        #region Numbers and Bools

        public bool? NextBool(string terminators = " ")
        {
            string s = NextWord(terminators);
            return s?.InvariantParseBoolNull();
        }

        public bool? NextBoolComma(string terminators = " ", char separ = ',')
        {
            bool? res = NextBool(terminators);
            return IsCharMoveOnOrEOL(separ) ? res : null;
        }

        public double? NextDouble(string terminators = " ")
        {
            string s = NextWord(terminators);
            return s?.InvariantParseDoubleNull();
        }

        public double? NextDoubleComma(string terminators = " ", char separ = ',')
        {
            double? res = NextDouble(terminators);
            return IsCharMoveOnOrEOL(separ) ? res : null;
        }

        public int? NextInt(string terminators = " ")
        {
            string s = NextWord(terminators);
            return s?.InvariantParseIntNull();
        }

        public int? NextIntComma(string terminators = " ", char separ = ',')
        {
            int? res = NextInt(terminators);
            return IsCharMoveOnOrEOL(separ) ? res : null;
        }

        public long? NextLong(string terminators = " ")
        {
            string s = NextWord(terminators);
            return s?.InvariantParseLongNull();
        }

        public long? NextLongComma(string terminators = " ", char separ = ',')
        {
            long? res = NextLong(terminators);
            return IsCharMoveOnOrEOL(separ) ? res : null;
        }

        #endregion

        #region Reversing

        public bool ReverseBack( bool quotes = true, bool brackets = true)      // one or both must be true
        {
            System.Diagnostics.Debug.Assert(quotes || brackets);
            int bracketlevel = 0;
            bool inquotes = false;

            while( pos > 0 )
            {
                pos--;
                char c = line[pos];

                if (!inquotes)
                {
                    if (brackets && c == ')')
                    {
                        bracketlevel++;
                    }
                    else if (brackets && c == '(')
                    {
                        bracketlevel--;
                        if (bracketlevel <= 0)
                            return true;
                    }
                    else if (quotes && c == '"')
                    {
                        inquotes = true;
                    }
                }
                else if (quotes && c == '"')
                {
                    if ( pos>0 && line[pos-1] != '\\')
                    {
                        inquotes = !inquotes;

                        if (!inquotes && bracketlevel == 0)
                            return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Find

        // Move pointer to string if found

        public bool Find(string s)      // move position to string, this will be the next read..
        {
            int indexof = line.IndexOf(s, pos);
            if (indexof != -1)
                pos = indexof;
            return (indexof != -1);
        }

        // Static wrappers

        public static string FirstQuotedWord(string s, string limits, string def = "", string prefix = "", string postfix = "")
        {
            if (s != null)
            {
                StringParser k1 = new StringParser(s);
                return prefix + k1.NextQuotedWord(limits) + postfix;
            }
            else
                return def;
        }

        public static List<string> ParseWordList(string s, System.Globalization.CultureInfo lowercase = null, bool replaceescape = false, char separ = ',')
        {
            StringParser sp = new StringParser(s);
            return sp.NextQuotedWordListSepar(lowercase, replaceescape, separ);
        }

        #endregion

    }
}
