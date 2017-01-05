using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionPlay : Action
    {
        public ActionPlay(string n, ActionType t, List<string> c, string ud, int lu) : base(n, t, c, ud, lu)
        {
        }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme)
        {
            string promptValue = Prompt.ShowDialog(parent, "File to play", UserData, "Configure Play Command");
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgram ap)
        {
            return true;
        }

        static class Prompt
        {
            public static string ShowDialog(Form p, string text, String defaultValue, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 440,
                    Height = 160,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen,
                };

                Label textLabel = new Label() { Left = 10, Top = 20, Width = 400, Text = text };
                TextBox textBox = new TextBox() { Left = 10, Top = 50, Width = 400 };
                textBox.Text = defaultValue;
                Button confirmation = new Button() { Text = "Ok", Left = 245, Width = 80, Top = 90, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "Cancel", Left = 330, Width = 80, Top = 90, DialogResult = DialogResult.Cancel };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                cancel.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(cancel);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog(p) == DialogResult.OK ? textBox.Text : null;
            }
        }
    }

}
