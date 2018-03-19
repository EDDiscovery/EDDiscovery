using BaseUtils.Win32Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{

    public static class PromptSingleLine
    {
        public static string ShowDialog(Form p,
                            string lab1, string defaultValue1, string caption, Icon ic, bool multiline = false, string tooltip = null)
        {
            List<string> r = PromptMultiLine.ShowDialog(p, caption, ic, new string[] { lab1 }, 
                    new string[] { defaultValue1 }, multiline, tooltip != null ? new string[] { tooltip } : null);

            return r?[0];
        }
    }

    public static class PromptMultiLine
    {
        // lab sets the items, def can be less or null
        public static List<string> ShowDialog(Form p, string caption, Icon ic, string[] lab, string[] def, bool multiline = false, string[] tooltips = null)
        {
            ITheme theme = ThemeableFormsInstance.Instance;

            int vstart = theme.WindowsFrame ? 20 : 40;
            int vspacing = multiline ? 80 : 40;
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
                Icon = ic
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
                    Multiline = multiline,      // set before height!
                    Height = vspacing - 20,
                    ScrollBars = (multiline) ? ScrollBars.Vertical : ScrollBars.None,
                    WordWrap = multiline
                };
                outer.Controls.Add(lbs[i]);
                outer.Controls.Add(tbs[i]);

                if (tooltips != null && i < tooltips.Length)
                {
                    tt.SetToolTip(lbs[i], tooltips[i]);
                    tbs[i].SetTipDynamically(tt, tooltips[i]);      // no container here, set tool tip on text boxes using this
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

            theme.ApplyToFormStandardFontSize(prompt);

            if (prompt.ShowDialog(p) == DialogResult.OK)
            {
                var r = (from ExtendedControls.TextBoxBorder t in tbs select t.Text).ToList();
                return r;
            }
            else
                return null;
        }
    }

    public class ConfigurableForm : DraggableForm
    {
        public event Action<string, string, Object> Trigger;        // returns dialog logical name, name of control, caller tag object

        private List<Entry> entries;
        private Object callertag;
        private string logicalname;
        private bool ProgClose = false;
        private System.Drawing.Point lastpos; // used for dynamically making the list up

        public class Entry
        {
            public string controlname;
            public Type controltype;
            public string text;
            public System.Drawing.Point pos;
            public System.Drawing.Size size;
            public string tooltip;                      // can be null.

            // ButtonExt, TextBoxBorder, Label, CheckBoxCustom, DateTime (t=time)
            public Entry(string nam, Type c, string t, System.Drawing.Point p, System.Drawing.Size s, string tt)
            {
                controltype = c; text = t; pos = p; size = s; tooltip = tt; controlname = nam; customdateformat = "long";
            }

            // ComboBoxCustom
            public Entry(string nam, string t, System.Drawing.Point p, System.Drawing.Size s, string tt, List<string> comboitems, Size? sz = null)
            {
                controltype = typeof(ExtendedControls.ComboBoxCustom); text = t; pos = p; size = s; tooltip = tt; controlname = nam;
                comboboxitems = string.Join(",", comboitems);
                comboboxdropdownsize = sz;
            }

            public bool checkboxchecked;        // fill in for checkbox
            public bool textboxmultiline;       // fill in for textbox
            public bool clearonfirstchar;       // fill in for textbox
            public string comboboxitems;        // fill in for combobox
            public Size? comboboxdropdownsize;  // may be null, fill in for combobox
            public string customdateformat;     // fill in for datetimepicker

            public Control control; // used by Show  no need to set.
        }

        private System.ComponentModel.IContainer components = null;     // replicate normal component container, so controls which look this
                                                                        // up for finding the tooltip can (TextBoxBorder)

        #region Public interface

        public ConfigurableForm()
        {
            this.components = new System.ComponentModel.Container();
            entries = new List<Entry>();
            lastpos = new System.Drawing.Point(0, 0);
        }

        public string Add(string instr)       // add a string definition dynamically add to list.  errmsg if something is wrong
        {
            Entry e;
            string errmsg = MakeEntry(instr, out e, ref lastpos);
            if (errmsg == null)
                entries.Add(e);
            return errmsg;
        }

        public void Add(Entry e )               // add an entry..
        {
            entries.Add(e);
        }

        public Entry Last { get { return entries.Last(); } }

        public DialogResult ShowDialog(Form p, Icon icon, System.Drawing.Size size, System.Drawing.Point pos, string caption, string lname = null, Object callertag = null)
        {
            Show(icon, size, pos, caption, lname, callertag);
            return ShowDialog(p);
        }

        public void Show(Form p, Icon icon, System.Drawing.Size size, System.Drawing.Point pos, string caption, string lname = null, Object callertag = null)
        {
            Show(icon, size, pos, caption, lname, callertag);
            Show(p);
        }

        public new void Close()     // program close.. allow it to close properly
        {
            ProgClose = true;
            base.Close();
        }

        public string Get(string controlname)      // return value of dialog control
        {
            Entry t = entries.Find(x => x.controlname.Equals(controlname, StringComparison.InvariantCultureIgnoreCase));
            if (t != null)
            {
                Control c = t.control;
                if (c is ExtendedControls.TextBoxBorder)
                    return (c as ExtendedControls.TextBoxBorder).Text;
                else if (c is ExtendedControls.CheckBoxCustom)
                    return (c as ExtendedControls.CheckBoxCustom).Checked ? "1" : "0";
                else if (c is ExtendedControls.CustomDateTimePicker)
                    return (c as ExtendedControls.CustomDateTimePicker).Value.ToString("yyyy/dd/MM HH:mm:ss",System.Globalization.CultureInfo.InvariantCulture);
                else if (c is ExtendedControls.ComboBoxCustom)
                {
                    ExtendedControls.ComboBoxCustom cb = c as ExtendedControls.ComboBoxCustom;
                    return (cb.SelectedIndex != -1) ? cb.Text : "";
                }
            }

            return null;
        }

        public DateTime? GetDateTime(string controlname)
        {
            Entry t = entries.Find(x => x.controlname.Equals(controlname, StringComparison.InvariantCultureIgnoreCase) );
            if (t != null)
            {
                CustomDateTimePicker c = t.control as CustomDateTimePicker;
                if (c!= null)
                    return c.Value;
            }

            return null;
        }

        public bool Set(string controlname, string value)      // set value of dialog control
        {
            Entry t = entries.Find(x => x.controlname.Equals(controlname, StringComparison.InvariantCultureIgnoreCase));
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
                else if (c is ExtendedControls.ComboBoxCustom)
                {
                    ExtendedControls.ComboBoxCustom cb = c as ExtendedControls.ComboBoxCustom;
                    if (cb.Items.Contains(value))
                    {
                        cb.Enabled = false;
                        cb.SelectedItem = value;
                        cb.Enabled = true;
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Implementation

        static private string MakeEntry(string instr, out Entry entry, ref System.Drawing.Point lastpos)
        {
            entry = null;

            BaseUtils.StringParser sp = new BaseUtils.StringParser(instr);

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
            else if (type.Equals("combobox"))
                ctype = typeof(ExtendedControls.ComboBoxCustom);
            else if (type.Equals("datetime"))
                ctype = typeof(ExtendedControls.CustomDateTimePicker);
            else
                return "Unknown control type " + type;

            string text = sp.NextQuotedWordComma();     // normally text..

            if (text == null)
                return "Missing text";

            int? x = sp.NextWordComma().InvariantParseIntNullOffset(lastpos.X);
            int? y = sp.NextWordComma().InvariantParseIntNullOffset(lastpos.Y);
            int? w = sp.NextWordComma().InvariantParseIntNull();
            int? h = sp.NextWordComma().InvariantParseIntNull();

            if (x == null || y == null || w == null || h == null)
                return "Missing position/size";

            string tip = sp.NextQuotedWordComma();      // tip can be null

            entry = new ConfigurableForm.Entry(name, ctype,
                        text, new System.Drawing.Point(x.Value, y.Value), new System.Drawing.Size(w.Value, h.Value), tip);

            if (type.Contains("textbox") && tip != null)
            {
                int? v = sp.NextWordComma().InvariantParseIntNull();
                entry.textboxmultiline = v.HasValue && v.Value != 0;
            }

            if (type.Contains("checkbox") && tip != null)
            {
                int? v = sp.NextWordComma().InvariantParseIntNull();
                entry.checkboxchecked = v.HasValue && v.Value != 0;

                v = sp.NextWordComma().InvariantParseIntNull();
                entry.clearonfirstchar = v.HasValue && v.Value != 0;
            }

            if (type.Contains("combobox"))
            {
                entry.comboboxitems = sp.LineLeft.Trim();
                if (tip == null || entry.comboboxitems.Length == 0)
                    return "Missing paramters for combobox";
            }

            if (type.Contains("datetime"))
            {
                entry.customdateformat = sp.NextWord();
            }

            lastpos = new System.Drawing.Point(x.Value, y.Value);
            return null;
        }

        private void Show(Icon icon, System.Drawing.Size size, System.Drawing.Point pos, string caption, string lname, Object callertag)
        {
            this.logicalname = lname;    // passed back to caller via trigger
            this.callertag = callertag;      // passed back to caller via trigger

            ITheme theme = ThemeableFormsInstance.Instance;

            FormBorderStyle = FormBorderStyle.FixedDialog;

            if (theme.WindowsFrame)
            {
                size.Height += 50;
            }

            Size = size;

            if ( pos.X==-999)
                StartPosition = FormStartPosition.CenterScreen;
            else
            {
                Location = pos;
                StartPosition = FormStartPosition.Manual;
            }

            Panel outer = new Panel() { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle };
            outer.MouseDown += FormMouseDown;
            outer.MouseUp += FormMouseUp;

            Controls.Add(outer);

            this.Text = caption;

            Label textLabel = new Label() { Left = 4, Top = 8, Width = Width - 50, Text = caption };
            textLabel.MouseDown += FormMouseDown;
            textLabel.MouseUp += FormMouseUp;

            if (!theme.WindowsFrame)
                outer.Controls.Add(textLabel);

            ToolTip tt = new ToolTip(components);
            tt.ShowAlways = true;
            for (int i = 0; i < entries.Count; i++)
            {
                Entry ent = entries[i];
                Control c = (Control)Activator.CreateInstance(ent.controltype);
                ent.control = c;
                c.Size = ent.size;
                c.Location = ent.pos;
                if (!(c is ExtendedControls.ComboBoxCustom || c is ExtendedControls.CustomDateTimePicker) )        // everything but get text
                    c.Text = ent.text;
                c.Tag = ent;     // point control tag at ent structure
                outer.Controls.Add(c);
                if (ent.tooltip != null)
                    tt.SetToolTip(c, ent.tooltip);

                if (c is ExtendedControls.ButtonExt)
                {
                    ExtendedControls.ButtonExt b = c as ExtendedControls.ButtonExt;
                    b.Click += (sender, ev) =>
                    {
                        Entry en = (Entry)(((Control)sender).Tag);
                        Trigger?.Invoke(logicalname, en.controlname, this.callertag);       // pass back the logical name of dialog, the name of the control, the caller tag
                    };
                }

                if (c is ExtendedControls.TextBoxBorder)
                {
                    ExtendedControls.TextBoxBorder tb = c as ExtendedControls.TextBoxBorder;
                    tb.Multiline = tb.WordWrap = ent.textboxmultiline;
                    tb.ClearOnFirstChar = ent.clearonfirstchar;
                }

                if (c is ExtendedControls.CheckBoxCustom)
                {
                    ExtendedControls.CheckBoxCustom cb = c as ExtendedControls.CheckBoxCustom;
                    cb.Checked = ent.checkboxchecked;
                    cb.Click += (sender, ev) =>
                    {
                        Entry en = (Entry)(((Control)sender).Tag);
                        Trigger?.Invoke(logicalname, en.controlname, this.callertag);       // pass back the logical name of dialog, the name of the control, the caller tag
                    };
                }

                if (c is ExtendedControls.CustomDateTimePicker)
                {
                    ExtendedControls.CustomDateTimePicker dt = c as ExtendedControls.CustomDateTimePicker;
                    DateTime t;
                    if (DateTime.TryParse(ent.text, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out t))     // assume local, so no conversion
                        dt.Value = t; 

                    switch (ent.customdateformat.ToLower())
                    {
                        case "short":
                            dt.Format = DateTimePickerFormat.Short;
                            break;
                        case "long":
                            dt.Format = DateTimePickerFormat.Long;
                            break;
                        case "time":
                            dt.Format = DateTimePickerFormat.Time;
                            break;
                        default:
                            dt.CustomFormat = ent.customdateformat;
                            break;
                    }
                }

                if (c is ExtendedControls.ComboBoxCustom)
                {
                    ExtendedControls.ComboBoxCustom cb = c as ExtendedControls.ComboBoxCustom;
                    if (ent.comboboxdropdownsize != null)
                    {
                        cb.DropDownHeight = ent.comboboxdropdownsize.Value.Height;
                        cb.DropDownWidth = ent.comboboxdropdownsize.Value.Width;
                    }

                    cb.Items.AddRange(ent.comboboxitems.Split(','));
                    if (cb.Items.Contains(ent.text))
                        cb.SelectedItem = ent.text;
                    cb.SelectedIndexChanged += (sender, ev) =>
                    {
                        Control ctr = (Control)sender;
                        if (ctr.Enabled)
                        {
                            Entry en = (Entry)(ctr.Tag);
                            Trigger?.Invoke(logicalname, en.controlname, this.callertag);       // pass back the logical name of dialog, the name of the control, the caller tag
                        }
                    };

                }
            }

            ShowInTaskbar = false;

            this.Icon = icon;

            theme.ApplyToFormStandardFontSize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (ProgClose == false)
            {
                e.Cancel = true; // stop it working. program does the close
                Trigger?.Invoke(logicalname, "Cancel", callertag);
            }
            else
                base.OnFormClosing(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Trigger?.Invoke(logicalname, "Escape", callertag);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FormMouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void FormMouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        #endregion

    }
}
