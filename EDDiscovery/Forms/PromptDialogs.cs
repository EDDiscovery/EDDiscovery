﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{

    public static class PromptSingleLine
    {
        public static string ShowDialog(Form p,
                            string lab1, string defaultValue1, string caption, bool multiline = false, string tooltip = null)
        {
            List<string> r = PromptMultiLine.ShowDialog(p, caption, new string[] { lab1 }, new string[] { defaultValue1 }, multiline, tooltip != null ? new string[] { tooltip } : null);

            return (r != null) ? r[0] : null;
        }
    }

    public static class PromptDoubleLine
    {
        public static Tuple<string, string> ShowDialog(Form p,
                            string lab1, string lab2, string defaultValue1, string defaultValue2, string caption, bool multiline = false, string[] tooltip = null)
        {
            List<string> r = PromptMultiLine.ShowDialog(p, caption, new string[] { lab1, lab2 }, new string[] { defaultValue1, defaultValue2 }, multiline, tooltip);

            return (r != null) ? new Tuple<string, string>(r[0], r[1]) : null;
        }
    }

    public static class PromptMultiLine
    {
        // lab sets the items, def can be less or null
        public static List<string> ShowDialog(Form p, string caption, string[] lab, string[] def, bool multiline = false, string[] tooltips = null)
        {
            EDDiscovery2.EDDTheme theme = EDDiscovery2.EDDTheme.Instance;

            int vstart = theme.WindowsFrame ? 20 : 40;
            int vspacing = multiline ? 60 : 40;
            int lw = 100;
            int lx = 10;
            int tx = 10 + lw + 8;

            Form prompt = new Form()
            {
                Width = 600,
                Height = 90 + vspacing * lab.Length + (theme.WindowsFrame ? 20 : 0),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
            };

            Panel outer = new Panel() { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle };
            prompt.Controls.Add(outer);

            Label textLabel = new Label() { Left = lx, Top = 8, Width = prompt.Width - 50, Text = caption };

            if (!theme.WindowsFrame)
                outer.Controls.Add(textLabel);

            Label[] lbs = new Label[lab.Length];
            ExtendedControls.TextBoxBorder[] tbs = new ExtendedControls.TextBoxBorder[lab.Length];

            ToolTip tt = new ToolTip();
            tt.ShowAlways = true;

            int y = vstart;

            for (int i = 0; i < lab.Length; i++)
            {
                lbs[i] = new Label() { Left = lx, Top = y, Width = lw, Text = lab[i] };
                tbs[i] = new ExtendedControls.TextBoxBorder()
                {
                    Left = tx,
                    Top = y,
                    Width = prompt.Width - 50 - tx,
                    Text = (def != null && i < def.Length) ? def[i] : "",
                    Height = vspacing - 20,
                    Multiline = multiline,
                    ScrollBars = (multiline) ? ScrollBars.Vertical : ScrollBars.None,
                    WordWrap = multiline
                };
                outer.Controls.Add(lbs[i]);
                outer.Controls.Add(tbs[i]);

                if (tooltips != null && i < tooltips.Length)
                {
                    tt.SetToolTip(lbs[i], tooltips[i]);
                    tt.SetToolTip(tbs[i], tooltips[i]);
                }

                y += vspacing;
            }

            ExtendedControls.ButtonExt confirmation = new ExtendedControls.ButtonExt() { Text = "Ok", Left = tbs[0].Right - 80, Width = 80, Top = y, DialogResult = DialogResult.OK };
            outer.Controls.Add(confirmation);
            confirmation.Click += (sender, e) => { prompt.Close(); };

            ExtendedControls.ButtonExt cancel = new ExtendedControls.ButtonExt() { Text = "Cancel", Left = confirmation.Location.X - 90, Width = 80, Top = confirmation.Top, DialogResult = DialogResult.Cancel };
            outer.Controls.Add(cancel);
            cancel.Click += (sender, e) => { prompt.Close(); };

            if (!multiline)
                prompt.AcceptButton = confirmation;

            prompt.CancelButton = cancel;
            prompt.ShowInTaskbar = false;

            theme.ApplyToForm(prompt, System.Drawing.SystemFonts.DefaultFont);

            if (prompt.ShowDialog(p) == DialogResult.OK)
            {
                var r = (from ExtendedControls.TextBoxBorder t in tbs select t.Text).ToList();
                return r;
            }
            else
                return null;
        }
    }

    public class ConfigurableForm : Form
    {
        private Entry[] entries;
        private Object tag;
        private string logicalname;
        public event Action<string, string, Object> Trigger;

        public class Entry
        {
            public string name;
            public Type controltype;
            public string text;
            public System.Drawing.Point pos;
            public System.Drawing.Size size;
            public string tooltip;                      // can be null.

            public Control control;

            public Entry(string nam, Type c, string t, System.Drawing.Point p, System.Drawing.Size s, string tt)
            {
                controltype = c; text = t; pos = p; size = s; tooltip = tt; name = nam;
            }

            public bool checkboxchecked;
            public bool textboxmultiline;
        }

        static public string MakeEntry(string instr , out Entry entry )
        {
            entry = null;

            StringParser sp = new StringParser(instr);

            string name = sp.NextQuotedWordComma();

            if (name == null)
                return "Missing name";

            string type = sp.NextWordComma(lowercase: true);

            Type ctype = null;

            if (type == null)
                return "Missing type";
            else if (type.Equals("button"))
                ctype = typeof(ExtendedControls.ButtonExt);
            else if (type.Equals("textbox"))
                ctype = typeof(ExtendedControls.TextBoxBorder);
            else if (type.Equals("checkbox"))
                ctype = typeof(ExtendedControls.CheckBoxCustom);
            else if (type.Equals("label"))
                ctype = typeof(System.Windows.Forms.Label);
            else
                return "Unknown control type " + type;

            string text = sp.NextQuotedWordComma();

            if (text == null)
                return "Missing text";

            int? x = sp.NextWordComma().InvariantParseIntNull();
            int? y = sp.NextWordComma().InvariantParseIntNull();
            int? w = sp.NextWordComma().InvariantParseIntNull();
            int? h = sp.NextWordComma().InvariantParseIntNull();

            if (x == null || y == null || w == null || h == null)
                return "Missing position/size";

            string tip = sp.NextQuotedWordComma();      // tip can be null

            entry = new Forms.ConfigurableForm.Entry(name, ctype,
                        text, new System.Drawing.Point(x.Value, y.Value), new System.Drawing.Size(w.Value, h.Value), tip);

            if (tip != null)        // if we had a tip, we could have..
            {
                if (type.Contains("textbox"))
                {
                    int? v = sp.NextWordComma().InvariantParseIntNull();
                    entry.textboxmultiline = v.HasValue && v.Value != 0;
                }

                if (type.Contains("checkbox"))
                {
                    int? v = sp.NextWordComma().InvariantParseIntNull();
                    entry.checkboxchecked = v.HasValue && v.Value != 0;
                }
            }

            return null;
        }

        public void Show(Form p, string lname, System.Drawing.Size size, string caption, Entry[] e, Object t)
        {
            logicalname = lname;
            entries = e;
            tag = t;

            EDDiscovery2.EDDTheme theme = EDDiscovery2.EDDTheme.Instance;

            Size = size;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Text = caption;
            StartPosition = FormStartPosition.CenterScreen;

            Panel outer = new Panel() { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle };
            Controls.Add(outer);

            Label textLabel = new Label() { Left = 4, Top = 8, Width = Width - 50, Text = caption };

            if (!theme.WindowsFrame)
                outer.Controls.Add(textLabel);

            ToolTip tt = new ToolTip();
            tt.ShowAlways = true;
            for (int i = 0; i < entries.Length; i++)
            {
                Control c = (Control)Activator.CreateInstance(entries[i].controltype);
                entries[i].control = c;
                c.Size = entries[i].size;
                c.Location = entries[i].pos;
                c.Text = entries[i].text;
                c.Tag = entries[i];
                outer.Controls.Add(c);
                if (entries[i].tooltip != null)
                    tt.SetToolTip(c, entries[i].tooltip);

                if (c is ExtendedControls.ButtonExt)
                {
                    ExtendedControls.ButtonExt b = c as ExtendedControls.ButtonExt;
                    b.Click += (sender, ev) =>
                    {
                        Entry en = (Entry)(((Control)sender).Tag);
                        if (Trigger != null)
                            Trigger(logicalname, en.name, tag);
                    };
                }

                if (c is ExtendedControls.TextBoxBorder)
                {
                    ExtendedControls.TextBoxBorder tb = c as ExtendedControls.TextBoxBorder;
                    tb.Multiline = tb.WordWrap = entries[i].textboxmultiline;
                }

                if (c is ExtendedControls.CheckBoxCustom)
                {
                    ExtendedControls.CheckBoxCustom cb = c as ExtendedControls.CheckBoxCustom;
                    cb.Checked = entries[i].checkboxchecked;
                    cb.Click += (sender, ev) =>
                    {
                        Entry en = (Entry)(((Control)sender).Tag);
                        if (Trigger != null)
                            Trigger(logicalname, en.name, tag);
                    };
                }
            }

            ShowInTaskbar = false;

            theme.ApplyToForm(this, System.Drawing.SystemFonts.DefaultFont);

            Show(p);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Trigger(logicalname, "Escape", tag);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public string Get(string name)
        {
            Entry t = Array.Find(entries, x => x.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (t != null)
            {
                Control c = t.control;
                if (c is ExtendedControls.TextBoxBorder)
                    return (c as ExtendedControls.TextBoxBorder).Text;
                else if (c is ExtendedControls.CheckBoxCustom)
                    return (c as ExtendedControls.CheckBoxCustom).Checked ? "1" : "0";
            }

            return null;
        }

        public bool Set(string name, string value)
        {
            Entry t = Array.Find(entries, x => x.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (t != null)
            {
                Control c = t.control;
                if (c is ExtendedControls.TextBoxBorder)
                {
                    (c as ExtendedControls.TextBoxBorder).Text = value;
                    return true;
                }
                else if (c is ExtendedControls.CheckBoxCustom)
                {
                    (c as ExtendedControls.CheckBoxCustom).Checked = !value.Equals("0");
                    return true;
                }
            }

            return false;
        }
    }
}
