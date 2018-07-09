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
        static public Tuple<string,bool> ProcessLine(string combinedline, string curline, int txpos, List<string> localsdone, List<string> globalsdone)
        {
            bool ok = false;
            bool local = true;
            string txphrase = "";
            string keyword = "";

            StringParser s0 = new StringParser(combinedline, txpos);

            if (s0.ReverseBack())
            {
                var res = s0.NextOptionallyBracketedQuotedWords();

                if (res != null)
                {
                    foreach (var t in res)
                    {
                        string ns = t.Item1.Replace(" ", "");

                        if (t.Item2)
                            txphrase += t.Item1;
                        else if (ns == "+Environment.NewLine+")
                            txphrase += "\\r\\n";
                        else if (ns == "+")
                            txphrase += "";
                        else
                        {
                            txphrase = null;
                            break;
                        }
                    }

                    if (txphrase != null)
                    {
                        StringParser s1 = new StringParser(combinedline, txpos + 4);

                        string nextword = s1.NextWord(",)");

                        if (nextword != null && nextword.StartsWith("typeof("))
                        {
                            nextword = nextword.Substring(7);

                            if ( s1.IsCharMoveOn(')'))
                            {
                                if (s1.IsCharMoveOn(','))
                                {
                                    keyword = s1.NextQuotedWord(")");

                                    if (keyword != null)
                                    {
                                        keyword = nextword + "." + keyword;
                                        ok = true;
                                    }
                                }
                                else if (s1.IsCharMoveOn(')'))
                                {
                                    keyword = nextword + "."+ txphrase.ReplaceNonAlphaNumeric();
                                    ok = true;
                                }
                            }
                        }
                        else if (nextword != null && (nextword == "this" || nextword == "t"))
                        {
                            if (s1.IsCharMoveOn(','))
                            {
                                keyword = s1.NextQuotedWord(")");

                                if (keyword != null)
                                {
                                    keyword = "." + keyword;
                                    ok = true;
                                }
                            }
                            else if (s1.IsCharMoveOn(')'))
                            {
                                keyword = "." + txphrase.ReplaceNonAlphaNumeric();
                                ok = true;
                            }
                        }
                        else if (s1.IsCharMoveOn(')'))
                        {
                            keyword = txphrase.ReplaceNonAlphaNumeric();
                            local = false;
                            ok = true;
                        }
                    }
                }
            }

            if (!ok)
                return new Tuple<string, bool>("Miss formed " + curline + Environment.NewLine,false);
            else if ( local )
            {
                if (localsdone.Contains(keyword))
                {
                    return new Tuple<string, bool>(null, false);
                }
                else
                {
                    localsdone.Add(keyword);
                    return new Tuple<string, bool>(keyword + ": " + txphrase.AlwaysQuoteString() + " @" + Environment.NewLine,false);
                }
            }
            else
            {
                if ( globalsdone.Contains(keyword))
                {
                    return new Tuple<string, bool>(null, true);
                }
                else
                {
                    globalsdone.Add(keyword);
                    return new Tuple<string, bool>(keyword + ": " + txphrase.AlwaysQuoteString() + " @" + Environment.NewLine,true);
                }
            }
           
        }

        static public string Process(FileInfo[] files)            // overall index of items
        {
            string locals = "";
            string globals = "////////////////// Globals " + Environment.NewLine;
            string designers = "";
            List<string> globalsdone = new List<string>();

            foreach (var fi in files)
            {
                var utc8nobom = new UTF8Encoding(false);        // give it the default UTF8 no BOM encoding, it will detect BOM or UCS-2 automatically

                using (StreamReader sr = new StreamReader(fi.FullName, utc8nobom))         // read directly from file.. presume UTF8 no bom
                {
                    string classname = "";

                    List<string> done = new List<string>();

                    bool donelocaltitle = false;
                    bool donedesignerstitle = false;

                    string line,previoustext="";

                    while ((line = sr.ReadLine()) != null)
                    {
                        int startpos = previoustext.Length;

                        string combined = previoustext + line;

                        while (true)
                        {
                            int txpos = combined.IndexOf(".Tx(", startpos);

                            if (txpos != -1)
                            {
                                Tuple<string, bool> ret = ProcessLine(combined, line, txpos,done , globalsdone);

                                if (ret.Item1 != null)
                                {
                                    if (ret.Item2 == false)
                                    {
                                        if (!donelocaltitle)
                                        {
                                            locals += "///////////////////////////////////////////////////// " + Path.GetFileNameWithoutExtension(fi.FullName) + Environment.NewLine;
                                            donelocaltitle = true;
                                        }
                                        locals += ret.Item1;
                                    }
                                    else
                                        globals += ret.Item1;
                                }

                                startpos = txpos + 4;
                            }
                            else
                                break;
                        }

                        previoustext += line;
                        if (previoustext.Length > 20000)
                            previoustext = previoustext.Substring(10000);


                        int textpos = line.IndexOf(".Text = ");
                        int thispos = line.IndexOf("this.");

                        if ( textpos > 0 && thispos > 0)
                        {
                            StringParser p = new StringParser(line.Substring(textpos + 7));
                            string text = p.NextQuotedWord();
                            //Console.WriteLine(line);

                            int namelength = textpos - thispos - 5;

                            if (namelength > 0)
                            {
                                string name = line.Substring(thispos + 5, namelength);

                                if (text != "<code>" &&
                                    !name.Contains("comboBox", StringComparison.InvariantCultureIgnoreCase) &&
                                    !name.Contains("Vscroll", StringComparison.InvariantCultureIgnoreCase) &&
                                    !name.Contains("TextBox", StringComparison.InvariantCultureIgnoreCase) &&
                                    !name.Contains("RichText", StringComparison.InvariantCultureIgnoreCase)
                                    )
                                {
                                    if (!donedesignerstitle)
                                    {
                                        designers += "///////////////////////////////////////////////////// " + Path.GetFileNameWithoutExtension(fi.FullName) + Environment.NewLine;
                                        donedesignerstitle = true;
                                    }

                                    designers += classname + "." + name + ": " + text.AlwaysQuoteString() + " @" + Environment.NewLine;

                                }
                            }
                        }

                        int clspos = line.IndexOf("partial class ");

                        if (clspos >= 0)
                            classname = line.Substring(clspos + 13).Trim();

                        // TBD tooltips            this.toolTip.SetToolTip(this.checkBoxMoveToTop, "Select if cursor moves to top entry when a new entry is received");

                    }
                }
            }

            return locals + Environment.NewLine + globals + Environment.NewLine + designers;
        }
    }
}
