using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class TranslatorExtensions
{
    static public string Tx(this string s)              // use the text, alpha numeric only, as the translation id
    {
        return BaseUtils.Translator.Instance.Translate(s, s.FirstAlphaNumericText());
    }

    static public string Tx(this string s, Object c, string id)     // use the type plus an id
    {
        return BaseUtils.Translator.Instance.Translate(s, c.GetType().Name, id);
    }

    static public string Txb(this string s, Object c, string id)     // use the base type plus an id
    {
        return BaseUtils.Translator.Instance.Translate(s, c.GetType().BaseType.Name, id);
    }

    static public string Tx(this string s, Type c)    // use a type definition using the string as the id
    {
        return BaseUtils.Translator.Instance.Translate(s, c.Name, s.FirstAlphaNumericText());
    }

    static public string Tx(this string s, Object c)              // use the object type with string as id
    {
        return BaseUtils.Translator.Instance.Translate(s, c.GetType().Name, s.FirstAlphaNumericText());
    }

    static public string Txb(this string s, Object c)     // use the base type using the string as the id
    {
        return BaseUtils.Translator.Instance.Translate(s, c.GetType().BaseType.Name, s.FirstAlphaNumericText());
    }

    static public string Tx(this string s, Type c, string id)    // use a type definition with id
    {
        return BaseUtils.Translator.Instance.Translate(s, c.Name, id);
    }

}

namespace BaseUtils
{
    // specials : if text in a control = <code> its presumed its a code filled in entry and not suitable for translation
    // in translator file, .Label means use the previous first word prefix stored, for shortness
    // using Label: "English" @ means for debug, replace @ with <english> as the foreign word in the debug build. In release, just use the in-code text

    public class Translator
    {
        static public Translator Instance { get
            {
                if (instance == null)
                    instance = new Translator();
                return instance; }
        }

        static Translator instance;

        LogToFile logger = null;

        Dictionary<string, string> translations = null;         // translation result can be null, which means, use the in-game english string
        List<string> ExcludedControls = new List<string>();

        private Translator()
        {
        }

        // You can call this multiple times if required for debugging purposes

        public void LoadTranslation(string language, CultureInfo uicurrent, string writabletxfolder1, string txfolder2)
        {
#if DEBUG
            if (logger != null)
                logger.Dispose();

            logger = new LogToFile();
            logger.SetFile(writabletxfolder1, "ids.txt", false);
#endif
            translations = null;        // forget any

            List<string> languagesfolder1 = Languages(writabletxfolder1,false);     // full paths
            List<string> languagesfolder2 = Languages(txfolder2,false);

            // uicurrent = CultureInfo.CreateSpecificCulture("es"); // debug

            string langfile = null;

            if (language == "Auto")
            {
                langfile = FindLanguage(languagesfolder1, uicurrent.Name);

                if (langfile == null)
                    langfile = FindLanguage(languagesfolder2, uicurrent.Name);

                if (language == null)
                    langfile = FindLanguage(languagesfolder1, uicurrent.TwoLetterISOLanguageName);

                if (language == null)
                    langfile = FindLanguage(languagesfolder2, uicurrent.TwoLetterISOLanguageName);

                if (langfile == null)
                    return;
            }
            else
            {
                langfile = Path.Combine(writabletxfolder1, language + ".tlf");

                if (!File.Exists(langfile))
                {
                    langfile = Path.Combine(txfolder2, language + ".tlf");

                    if (!File.Exists(langfile))
                        return;
                }
            }

            System.Diagnostics.Debug.WriteLine("Load Language " + langfile);
            logger?.WriteLine("Read " + langfile);

            try
            {
                var utc8nobom = new UTF8Encoding(false);        // give it the default UTF8 no BOM encoding, it will detect BOM or UCS-2 automatically

                StreamReader sr = new StreamReader(langfile, utc8nobom);

                translations = new Dictionary<string, string>();

                string prefix = "";

                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.Length > 0 && !line.StartsWith("//"))
                    {
                        StringParser s = new StringParser(line);

                        string id = s.NextWord(" :");

                        if (id.StartsWith(".") && prefix.HasChars())
                            id = prefix + id;
                        else
                            prefix = id.Word(new char[] { '.' });

                        if (s.IsCharMoveOn(':'))
                        {
                            s.NextQuotedWord(replaceescape: true);  // ignore the english for ref purposes only

                            string foreign = null;
                            bool err = false;

                            if (s.IsStringMoveOn("=>"))
                            {
                                foreign = s.NextQuotedWord(replaceescape: true);
                                err = foreign == null;
                            }
                            else if (s.IsCharMoveOn('@'))
                                foreign = null;
                            else
                                err = false;

                            if ( err == true )
                            {
                                logger?.WriteLine(string.Format("*** Translator ID but no translation {0}", id));
                                System.Diagnostics.Debug.WriteLine("*** Translator ID but no translation {0}", id);
                            }
                            else
                            {
                                if (!translations.ContainsKey(id))
                                {
                                    //logger?.WriteLine(string.Format("New {0}: \"{1}\" => \"{2}\"", id, english, foreign));
                                    translations[id] = foreign;
                                }
                                else
                                {
                                    logger?.WriteLine(string.Format("*** Translator Repeat {0}", id));
                                }

                            }

                        }
                    }
                }
            }
            catch { }
        }

        public void AddExcludedControls(string [] s)
        {
            ExcludedControls.AddRange(s);
        }

        static public List<string> Languages(string txfolder, bool nameonly)
        {
            FileInfo[] allFiles = Directory.EnumerateFiles(txfolder, "*.tlf", SearchOption.TopDirectoryOnly).Select(f => new FileInfo(f)).OrderBy(p => p.LastWriteTime).ToArray();

            return nameonly ? allFiles.Select(x => Path.GetFileNameWithoutExtension(x.Name)).ToList() : allFiles.Select(x => x.Name).ToList();
        }

        static public string FindLanguage(List<string> lang, string isoname)    // take filename part only, see if filename text-<iso> is present
        {
            return lang.Find(x =>
            {
                string filename = Path.GetFileNameWithoutExtension(x);
                int dash = x.IndexOf('-');
                return dash != -1 && x.Substring(dash + 1).Equals(isoname);
            });
        }

        public string Translate(string normal, string id)
        {
            if (translations != null && !normal.Equals("<code>") )
            {
                string key = id;
                if (translations.ContainsKey(key))
                {
#if DEBUG
                    return translations[key] ?? ('\'' + normal + '\'');     // debug more we quote them to show its not translated, else in release we just print
#else
                    return translations[key] ?? normal;
#endif
                }
                else
                {
                    logger?.WriteLine(string.Format("{0}: {1} @", id, normal.EscapeControlChars().AlwaysQuoteString()));
                    translations.Add(key, normal);
                    //System.Diagnostics.Debug.WriteLine("*** Missing Translate ID: {0}: \"{1}\" => \"{2}\"", id, normal.EscapeControlChars(), "<" + normal.EscapeControlChars() + ">");
                    return normal;
                }
            }
            else
                return normal;
        }

        public string Translate(string normal, string root, string id)
        {
            return Translate(normal, root + "." + id);
        }

        public void Translate(Control ctrl, Control[] ignorelist = null)
        {
            Translate(ctrl, ctrl.GetType().Name, ignorelist);
        }

        // Call direct only for debugging, normally use the one above.
        public void Translate(Control ctrl, string subname, Control[] ignorelist , bool debugit = false )
        {
            if (translations != null)
            {
                if (debugit)
                    System.Diagnostics.Debug.WriteLine("T: " + subname + " .. " + ctrl.Name + " (" + ctrl.GetType().Name + ")");

                if ((ignorelist == null || !ignorelist.Contains(ctrl)) && !ExcludedControls.Contains(ctrl.GetType().Name))
                {
                    if (ctrl.Text.HasChars())
                    {
                        string id = (ctrl is GroupBox || ctrl is TabPage) ? (subname + "." + ctrl.Name) : subname;
                        if (debugit)
                            System.Diagnostics.Debug.WriteLine(" -> Check " + id);
                        ctrl.Text = Translate(ctrl.Text, id);
                    }

                    if (ctrl is DataGridView)
                    {
                        DataGridView v = ctrl as DataGridView;
                        foreach (DataGridViewColumn c in v.Columns)
                        {
                            if (c.HeaderText.HasChars())
                                c.HeaderText = Translate(c.HeaderText, subname.AppendPrePad(c.Name, "."));
                        }
                    }

                    if (ctrl is TabPage)
                        subname = subname.AppendPrePad(ctrl.Name, ".");

                    foreach (Control c in ctrl.Controls)
                    {
                        string name = subname;
                        if ( NameControl(c) )
                            name = name.AppendPrePad(c.Name, ".");

                        Translate(c, name, ignorelist, debugit);
                    }
                }
                else
                {
                 //   logger?.WriteLine("Rejected " + subname);
                }
            }
        }
    
        public void Translate(ToolStrip ctrl, Control parent )
        {
            if (translations != null)
            {
                string subname = parent.GetType().Name;

                foreach (ToolStripItem msi in ctrl.Items)
                {
                    Translate(msi, subname);
                }
            }
        }

        private void Translate(ToolStripItem msi, string subname)
        {
            string itemname = msi.Name;

            if (msi.Text.HasChars())
                msi.Text = Translate(msi.Text, subname.AppendPrePad(itemname, "."));

            var ddi = msi as ToolStripDropDownItem;
            if (ddi != null)
            {
                foreach (ToolStripItem dd in ddi.DropDownItems)
                    Translate(dd, subname.AppendPrePad(itemname, "."));
            }
        }

        public void Translate(ToolTip tt, Control parent)
        {
            Translate(parent, tt, parent.GetType().Name);
        }

        public bool NameControl( Control c )
        {
            return c.GetType().Name == "PanelNoTheme" || !(c is Panel || c is DataGridView || c is GroupBox || c is SplitContainer );
        }

        private void Translate(Control ctrl, ToolTip tt, string subname)
        {
            if (translations != null)
            {
                string s = tt.GetToolTip(ctrl);
                if (s.HasChars())
                    tt.SetToolTip(ctrl, Translate(s, subname.AppendPrePad("ToolTip", ".")));

                foreach (Control c in ctrl.Controls)
                {
                    string name = subname;
                    if ( NameControl(c))      // containers don't send thru 
                        name = name.AppendPrePad(c.Name, ".");

                    Translate(c, tt, name);
                }
            }
        }

     }
}
