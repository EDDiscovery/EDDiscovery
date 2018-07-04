using BaseUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogEntry
{
    public static class ScanTranslate
    {
        static public string ProcessLine(string line, int txpos, List<string> done)
        {
            bool ok = false;
            bool dotit = true;
            string txphrase = "";
            string keyword = "";

            StringParser s0 = new StringParser(line, txpos);

            if (s0.ReverseBack())
            {
                var res = s0.NextOptionallyBracketedQuotedWords();

                if (res != null)
                {
                    foreach (var t in res)
                    {
                        if (t.Item2)
                            txphrase += t.Item1;
                        else if (t.Item1.Contains("Environment.NewLine"))
                            txphrase += "\\r\\n";
                        else
                        {
                            txphrase = null;
                            break;
                        }
                    }

                    if (txphrase != null)
                    {
                        StringParser s1 = new StringParser(line, txpos + 4);

                        string nextword = s1.NextWord(",)");
                        if (nextword != null && nextword == "this")
                        {
                            if (s1.IsCharMoveOn(','))
                            {
                                keyword = s1.NextQuotedWord(")");

                                if (keyword != null)
                                {
                                    ok = true;
                                }
                            }
                            else if (s1.IsCharMoveOn(')'))
                            {
                                keyword = txphrase.ReplaceNonAlphaNumeric();
                                ok = true;
                            }
                        }
                        else if (s1.IsCharMoveOn(')'))
                        {
                            keyword = txphrase.ReplaceNonAlphaNumeric();
                            dotit = false;
                            ok = true;
                        }
                    }
                }
            }

            if (dotit)
                keyword = "." + keyword;

            if (!ok)
                return "Miss formed " + line + Environment.NewLine;
            else if ( done.Contains(keyword))
            {
                //return "Repeat " + keyword + Environment.NewLine;
                return "";
            }
            else
            {
                done.Add(keyword);
                return keyword + ": " + txphrase.AlwaysQuoteString() + " @" + Environment.NewLine;
            }
        }

        static public string Process(FileInfo[] files)            // overall index of items
        {
            string ret = "";

            foreach (var fi in files)
            {
                var utc8nobom = new UTF8Encoding(false);        // give it the default UTF8 no BOM encoding, it will detect BOM or UCS-2 automatically

                using (StreamReader sr = new StreamReader(fi.FullName, utc8nobom))         // read directly from file.. presume UTF8 no bom
                {
                    List<string> done = new List<string>();

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        int startpos = 0;

                        while (true)
                        {
                            int txpos = line.IndexOf(".Tx(", startpos);

                            if (txpos != -1)
                            {
                                ret += ProcessLine(line, txpos,done);
                                startpos = txpos + 4;
                            }
                            else
                                break;
                        }

                    }
                }
            }

            return ret;
        }
    }
}
