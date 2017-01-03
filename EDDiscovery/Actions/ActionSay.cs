using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionSay : Action
    {
        public ActionSay(string n, List<string> c , string ud, int lu) : base(n,c,ud,lu)
        {
        }

        string flagOnWaitComplete = "WaitComplete";

        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme)
        {
            Tuple<string,bool> promptValue = Prompt.ShowDialog(parent, "Set Text to say (use ; to separate randomly selectable phrases)", UserData, IsFlag(flagOnWaitComplete) , "Configure Say Command" , theme);

            if (promptValue != null)
            {
                SetFlag(flagOnWaitComplete, promptValue.Item2);
                userdata = promptValue.Item1;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction()
        {
            return true;
        }

        static class Prompt
        {
            public static Tuple<string,bool> ShowDialog(Form p, string text, String defaultValue, bool waitcomplete, string caption, EDDiscovery2.EDDTheme theme)
            {
                Form prompt = new Form()
                {
                    Width = 440,
                    Height = 160,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen,
                };

                Panel outerpanel = new Panel() { Left = 0, Top = 0, Width = 420, Height = 150, BorderStyle = BorderStyle.FixedSingle};
                prompt.Controls.Add(outerpanel);

                Label textLabel = new Label() { Left = 10, Top = 20, Width = 400, Text = text };
                ExtendedControls.TextBoxBorder textBox = new ExtendedControls.TextBoxBorder() { Left = 10, Top = 50, Width = 400, Text = defaultValue };
                ExtendedControls.CheckBoxCustom checkBox1 = new ExtendedControls.CheckBoxCustom() { Left = 10, Top = 75, Width = 400, Height = 20, Text = "Wait until complete", Checked = waitcomplete };
                ExtendedControls.ButtonExt confirmation = new ExtendedControls.ButtonExt() { Text = "Ok", Left = 330, Width = 80, Top = 90, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                ExtendedControls.ButtonExt cancel = new ExtendedControls.ButtonExt() { Text = "Cancel", Left = 245, Width = 80, Top = 90, DialogResult = DialogResult.Cancel };
                cancel.Click += (sender, e) => { prompt.Close(); };
                outerpanel.Controls.AddRange(new Control[] { textBox, confirmation , cancel, textLabel, checkBox1 });

                prompt.AcceptButton = confirmation;

                theme.ApplyToForm(prompt, System.Drawing.SystemFonts.DefaultFont);

                return prompt.ShowDialog(p) == DialogResult.OK ? new Tuple<string,bool>(textBox.Text,checkBox1.Checked) : null;
            }
        }
    }
}
