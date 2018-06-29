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
        return BaseUtils.Translator.Instance.Translate(s, s.ReplaceNonAlphaNumeric());
    }

    static public string Tx(this string s, Object c, string id)     // use the type plus an id
    {
        return BaseUtils.Translator.Instance.Translate(s, c.GetType().Name, id);
    }

    static public string Tx(this string s, Object c)              // use the type with string as id
    {
        return BaseUtils.Translator.Instance.Translate(s, c.GetType().Name, s.ReplaceNonAlphaNumeric());
    }

    static public string Tx(this string s, Type c, string id)    // use a type definition with id
    {
        return BaseUtils.Translator.Instance.Translate(s, c.Name, id);
    }
}

namespace BaseUtils
{
    // specials : if text = <code> its presumed its a code filled in entry and not suitable for translation
    // in translator file, .Label means use the previous first word prefix stored, for shortness
    // using Label: "English" @ means for debug, replace @ with <english> as the foreign word

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

        Dictionary<string, string> translations = null;
        List<string> ExcludedControls = new List<string>();

        private Translator()
        {
        }

        public void LoadTranslation(string language, CultureInfo uicurrent, string txfolder)
        {
#if DEBUG
            logger = new LogToFile();
            logger.SetFile(txfolder, "ids.txt", false);
#endif
            List<string> languages = Languages(txfolder);

            // uicurrent = CultureInfo.CreateSpecificCulture("es"); // debug

            if (language == "Auto")
            {
                language = FindLanguage(languages, uicurrent.Name);

                if (language == null)
                    language = FindLanguage(languages, uicurrent.TwoLetterISOLanguageName);

                if (language == null)
                    return;
            }

            string langfile = Path.Combine(txfolder, language + ".tlf");

            if (File.Exists(langfile))
            {
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

                            string id = s.NextWord(" :").ToLower();

                            if (s.IsCharMoveOn(':'))
                            {
                                string english = s.NextQuotedWord(replaceescape: true);
                                string foreign = null;

                                if (s.IsStringMoveOn("=>"))
                                    foreign = s.NextQuotedWord(replaceescape: true);
                                else if (s.IsCharMoveOn('@'))      // debug marker
                                    foreign = "<" + english + ">";

                                if (foreign != null)
                                {
                                    //  System.Diagnostics.Debug.WriteLine("{0}: {1} => {2}", id, english, foreign);

                                    //                                    make LF work

                                    if (id.StartsWith(".") && prefix.HasChars())
                                        id = prefix + id;
                                    else
                                        prefix = id.Word(new char[] { '.' });

                                    if (!translations.ContainsKey(id))
                                    {
                                        //logger?.WriteLine(string.Format("New {0}: \"{1}\" => \"{2}\"", id, english, foreign));
                                        translations[id] = foreign;
                                    }
                                    else
                                    {
                                        logger?.WriteLine(string.Format("*** Translator Repeat {0}: \"{1}\" => \"{2}\"", id, english, foreign));
                                        System.Diagnostics.Debug.WriteLine("*** Translator Repeat {0}: {1} => {2}", id, english, foreign);
                                    }
                                }
                                else
                                {
                                    logger?.WriteLine(string.Format("*** Translator ID but no translation \"{0}\": \"{1}\"", id, english));
                                    System.Diagnostics.Debug.WriteLine("*** Translator ID but no translation {0}: {1}", id, english);
                                }
                            }
                        }
                    }
                }
                catch { }
            }
        }

        public void AddExcludedControls(string [] s)
        {
            ExcludedControls.AddRange(s);
        }

        static public List<string> Languages(string txfolder)
        {
            FileInfo[] allFiles = Directory.EnumerateFiles(txfolder, "*.tlf", SearchOption.TopDirectoryOnly).Select(f => new FileInfo(f)).OrderBy(p => p.LastWriteTime).ToArray();

            return allFiles.Select(x => Path.GetFileNameWithoutExtension(x.Name)).ToList();
        }

        static public string FindLanguage(List<string> lang, string isoname)
        {
            return lang.Find(x =>
            {
                int dash = x.IndexOf('-');
                return dash != -1 && x.Substring(dash + 1).Equals(isoname);
            });
        }

        public string Translate(string normal, string id)
        {
            string key = id.ToLower();
            if (translations != null && !normal.Equals("<code>") )
            {
                if (translations.ContainsKey(key))
                {
                    //System.Diagnostics.Debug.WriteLine("Translate: \"{0}\" => \"{1}\"", id, translations[id]);
                    return translations[key];
                }
                else
                {
                    logger?.WriteLine(string.Format("{0}: \"{1}\" @", id, normal.EscapeControlChars()));
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
