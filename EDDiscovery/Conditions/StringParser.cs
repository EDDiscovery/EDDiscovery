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
                        int nextquote = line.IndexOf('"', pos);
                        if (nextquote == -1)
                            return null;    // MUST have a quote..

                        if (line[nextquote - 1] == '\\')        // if \\"
                        {
                            ret += line.Substring(pos, nextquote - 1 - pos) + "\"";
                            pos = nextquote + 1;        // go past the quote.. try again
                        }
                        else
                        {       //End of quoted string
                            ret += line.Substring(pos, nextquote - pos);
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
            if (s != null && int.TryParse(s, out i))
                return i;
            else
                return null;
        }

    }

}
