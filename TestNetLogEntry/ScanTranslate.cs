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
        public class Definition
        {
            public Definition(string t, string x, string l) { token = t;text = x; firstdeflocation = l; }
            public string token;
            public string text;
            public string firstdeflocation;
        };

        static public Tuple<string, string, bool> ProcessLine(string combinedline, string curline, int txpos, int parapos , bool warnoldidchange)
                                    
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
                        else if (ns == "+Environment.NewLine+Environment.NewLine+")
                            txphrase += "\\r\\n\\r\\n";
                        else if (ns == "+Environment.NewLine+Environment.NewLine")
                            txphrase += "\\r\\n\\r\\n";
                        else if (ns == "+Environment.NewLine+")
                            txphrase += "\\r\\n";
                        else if (ns == "Environment.NewLine+")
                            txphrase += "\\r\\n";
                        else if (ns == "+Environment.NewLine")
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
                        StringParser s1 = new StringParser(combinedline, parapos);

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
                                    if (warnoldidchange && txphrase.FirstAlphaNumericText() != txphrase.ReplaceNonAlphaNumeric())
                                        Console.WriteLine("Warning : Changed ID " + txphrase + "  " + txphrase.FirstAlphaNumericText() + " old " + txphrase.ReplaceNonAlphaNumeric());
                                    keyword = nextword + "."+ txphrase.FirstAlphaNumericText();
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
                                    ok = true;
                                }
                            }
                            else if (s1.IsCharMoveOn(')'))
                            {
                                keyword = txphrase.FirstAlphaNumericText();

                                if (warnoldidchange && keyword != txphrase.ReplaceNonAlphaNumeric())
                                    Console.WriteLine("Warning : Changed ID " + txphrase + "  " + txphrase.FirstAlphaNumericText() + " old " + txphrase.ReplaceNonAlphaNumeric());

                                ok = true;
                            }
                        }
                        else if (s1.IsCharMoveOn(')'))
                        {
                            keyword = txphrase.FirstAlphaNumericText();
                            if (warnoldidchange && keyword != txphrase.ReplaceNonAlphaNumeric())
                                Console.WriteLine("Warning : Changed ID " + txphrase + "  " + txphrase.FirstAlphaNumericText() + " old " + txphrase.ReplaceNonAlphaNumeric());
                            local = false;
                            ok = true;
                        }
                    }
                }
            }

            return (ok) ? new Tuple<string, string, bool>(keyword, txphrase, local) : null;
        }

        static public string Process(FileInfo[] files, bool combinedone, bool showrepeats)            // overall index of items
        {
            string locals = "";
            string globals = "";
            bool doneglobalstitle = false;
            List<Definition> globalsdone = new List<Definition>();
            List<Definition> localsdone = new List<Definition>();

            foreach (var fi in files)
            {
                var utc8nobom = new UTF8Encoding(false);        // give it the default UTF8 no BOM encoding, it will detect BOM or UCS-2 automatically

                using (StreamReader sr = new StreamReader(fi.FullName, utc8nobom))         // read directly from file.. presume UTF8 no bom
                {
                    List<string> classes = new List<string>();
                    List<string> baseclasses = new List<string>();
                    List<int> classeslevel = new List<int>();
                    int bracketlevel = 0;

                    if ( !combinedone )
                        localsdone = new List<Definition>();

                    bool donelocaltitle = false;

                    string line,previoustext="";
                    int lineno = 0;

                    while ((line = sr.ReadLine()) != null)
                    {
                        lineno++;

                        int startpos = previoustext.Length;

                        string combined = previoustext + line;

                        while (true)
                        {
                            bool usebasename = false;

                            int txpos = combined.IndexOf(".Tx(", startpos);
                            int txbpos = combined.IndexOf(".Txb(", startpos);
                            int parapos = txpos + 4;

                            if ( txbpos >= 0 && (txpos ==-1 || txpos > txbpos ))
                            {
                                txpos = txbpos;
                                parapos = txpos + 5;
                                usebasename = true;
                            }

                            if (txpos != -1)
                            {
                                Tuple<string, string, bool> ret = ProcessLine(combined, line, txpos, parapos, false);

                                string localtext = "";
                                string globaltext = "";

                                if (ret == null)
                                    localtext = fi.FullName + ":" + lineno + ":Miss formed line around " + combined.Mid(txpos, 30) + Environment.NewLine;
                                else
                                {
                                    bool local = ret.Item3;

                                    string classprefix = "";

                                    if ( !ret.Item1.Contains("."))     // typeof is already has class name sksksk.skksks 
                                        classprefix = local ? (usebasename ? (baseclasses.Count > 0 ? (baseclasses.Last()+".") : null) : (classes.Count > 0 ? (classes.Last()+".") : null)) : "";

                                    if (classprefix == null)
                                    {
                                        localtext = fi.FullName + ":" + lineno + ":ERROR: No class to assign name to - probably not reading {} properly " + ret.Item1 + Environment.NewLine;
                                    }
                                    else
                                    {
                                        string id = classprefix + ret.Item1;

                                        Definition def = (local ? localsdone : globalsdone).Find(x => x.token.Equals(id, StringComparison.InvariantCultureIgnoreCase));

                                        string res = "";
                                        if (def != null)
                                        {
                                            if (def.text != ret.Item2)
                                                res = fi.FullName + ":" + lineno + ":ERROR: ID has different text " + id + " " + ret.Item2.AlwaysQuoteString() + " orginal " + def.text.AlwaysQuoteString() + " at " + def.firstdeflocation;
                                            else if (showrepeats)
                                                res = "//Repeat " + id + " " + ret.Item2.AlwaysQuoteString();
                                            else
                                                res = null;     // indicate no output
                                        }
                                        else
                                        {
                                            (local ? localsdone : globalsdone).Add(new Definition(id, ret.Item2, fi.FullName + ":" + lineno));
                                        }

                                        if (res != null)
                                        {
                                            if (!res.HasChars())
                                                res = id + ": " + ret.Item2.AlwaysQuoteString() + " @";

                                            res += Environment.NewLine;

                                            if (local)
                                                localtext = res;
                                            else
                                                globaltext = res;
                                        }
                                    }
                                }

                                if (localtext.HasChars())
                                {
                                    if (!donelocaltitle)
                                    {
                                        locals += "///////////////////////////////////////////////////// " + (classes.Count>0 ? classes[0] : "?")+ Environment.NewLine;
                                        donelocaltitle = true;
                                    }
                                    locals += localtext;
                                }

                                if ( globaltext.HasChars())
                                { 
                                    if (!doneglobalstitle)
                                    {
                                        globals += "///////////////////////////////////////////////////// Globals" + Environment.NewLine;
                                        doneglobalstitle = true;
                                    }
                                    globals += globaltext;
                                }

                                startpos = parapos;
                            }
                            else
                                break;
                        }

                        previoustext += line;
                        if (previoustext.Length > 20000)
                            previoustext = previoustext.Substring(10000);


                        line = line.Trim();

                        int clspos = line.IndexOf("partial class ");
                        if (clspos == -1)
                            clspos = line.IndexOf("public class ");
                        if (clspos == -1)
                            clspos = line.IndexOf("abstract class ");

                        if (clspos >= 0)
                        {
                            StringParser sp = new StringParser(line, clspos);
                            sp.NextWord(" ");
                            sp.NextWord(" ");
                            classes.Add(sp.NextWord(":").Trim());
                            baseclasses.Add(sp.IsCharMoveOn(':') ? sp.NextWord(",") : null);
                            classeslevel.Add(bracketlevel);
                            System.Diagnostics.Debug.WriteLine(lineno + " {" + bracketlevel * 4 + " Push " + classes.Last() + " " + baseclasses.Last());
                        }

                        if (line.StartsWith("{"))
                        {
                            if (line.Length == 1 || !line.Substring(1).Trim().StartsWith("}"))
                            {
                                System.Diagnostics.Debug.WriteLine(lineno + " {" + bracketlevel * 4);
                                bracketlevel++;
                            }
                            else
                                System.Diagnostics.Debug.WriteLine(lineno + " Rejected {" + bracketlevel * 4);
                        }

                        if (line.StartsWith("}"))
                        {
                            if (line.Length == 1 || line.Substring(1).Trim()[0] == '/' || line.Substring(1).Trim()[0] == ';')
                            {
                                if (classeslevel.Count > 0 && classeslevel.Last() == bracketlevel - 1)
                                {
                                    System.Diagnostics.Debug.WriteLine(lineno + " Pop {" + (bracketlevel-1) * 4 + " " + classes.Last());
                                    classes.RemoveAt(classes.Count - 1);
                                    classeslevel.RemoveAt(classeslevel.Count - 1);
                                    baseclasses.RemoveAt(baseclasses.Count - 1);
                                }
                                bracketlevel--;
                                System.Diagnostics.Debug.WriteLine(lineno + " }" + bracketlevel * 4);
                            }
                            else
                                System.Diagnostics.Debug.WriteLine(lineno + " Rejected }" + bracketlevel * 4);

                        }

                    }
                }
            }

            return locals + globals;
        }
    }
}


//// look for designer items.. experimental..

//int textpos = line.IndexOf(".Text = ");
//int thispos = line.IndexOf("this.");

//if ( textpos > 0 && thispos > 0)
//{
//    StringParser p = new StringParser(line.Substring(textpos + 7));
//    string text = p.NextQuotedWord();
//    //Console.WriteLine(line);

//    int namelength = textpos - thispos - 5;

//    if (namelength > 0)
//    {
//        string name = line.Substring(thispos + 5, namelength);

//        if (text != "<code>" &&
//            !name.Contains("comboBox", StringComparison.InvariantCultureIgnoreCase) &&
//            !name.Contains("Vscroll", StringComparison.InvariantCultureIgnoreCase) &&
//            !name.Contains("TextBox", StringComparison.InvariantCultureIgnoreCase) &&
//            !name.Contains("RichText", StringComparison.InvariantCultureIgnoreCase)
//            )
//        {
//            if (!donedesignerstitle)
//            {
//                designers += "///////////////////////////////////////////////////// " + Path.GetFileNameWithoutExtension(fi.FullName) + Environment.NewLine;
//                donedesignerstitle = true;
//            }

//            designers += classes.Last() + "." + name + ": " + text.AlwaysQuoteString() + " @" + Environment.NewLine;

//        }
//    }
//}

//// TBD tooltips            this.toolTip.SetToolTip(this.checkBoxMoveToTop, "Select if cursor moves to top entry when a new entry is received");

