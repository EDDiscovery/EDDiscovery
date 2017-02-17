using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{

    public static class PromptSingleLine
    {
        public static string ShowDialog(Form p, EDDiscovery2.EDDTheme theme,
                            string lab1, string defaultValue1, string caption, bool multiline = false)
        {
            List<string> r = PromptMultiLine.ShowDialog(p, theme, caption, new string[] { lab1 }, new string[] { defaultValue1 }, multiline);

            return (r != null) ? r[0] : null;
        }
    }

    public static class PromptDoubleLine
    {
        public static Tuple<string, string> ShowDialog(Form p, EDDiscovery2.EDDTheme theme,
                            string lab1, string lab2, string defaultValue1, string defaultValue2, string caption, bool multiline = false)
        {
            List<string> r = PromptMultiLine.ShowDialog(p, theme, caption, new string[] { lab1, lab2 }, new string[] { defaultValue1, defaultValue2 }, multiline);

            return (r != null) ? new Tuple<string, string>(r[0], r[1]) : null;
        }
    }

    public static class PromptMultiLine
    {
        // lab sets the items, def can be less or null
        public static List<string> ShowDialog(Form p, EDDiscovery2.EDDTheme theme, string caption, string[] lab, string[] def, bool multiline = false)
        {
            int vstart = theme.WindowsFrame ? 20 : 40;
            int vspacing = multiline ? 60 : 40;
            int lw = 80;
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
}
