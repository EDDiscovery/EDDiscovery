using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ExtendedControls
{
    class AutoCompleteTextBox : TextBoxBorder
    {
        private System.Windows.Forms.Timer waitforautotimer;
        private bool inautocomplete = false;
        private string autocompletestring;
        private bool restartautocomplete = false;
        private System.Threading.Thread ThreadAutoComplete;
        private PerformAutoComplete func = null;
        private List<string> autocompletestrings = null;

        public delegate List<string> PerformAutoComplete(string input);

        public AutoCompleteTextBox() : base()
        {
            TextChanged += TextChangeEventHandler;
            waitforautotimer = new System.Windows.Forms.Timer();
            waitforautotimer.Interval = 500;
            waitforautotimer.Tick += TimeOutTick;
        }

        public void SetAutoCompletor( PerformAutoComplete p )
        {
            func = p;
        }

        protected void TextChangeEventHandler(object sender, EventArgs e)
        {
            if (func != null)
            {
                if (!inautocomplete)
                {
                    waitforautotimer.Stop();
                    waitforautotimer.Start();
                    autocompletestring = String.Copy(this.Text);    // a copy in case the text box changes it after complete starts
                }
                else
                {
                    autocompletestring = String.Copy(this.Text);
                    restartautocomplete = true;
                }
            }
        }

        void TimeOutTick(object sender, EventArgs e)
        {
            waitforautotimer.Stop();
            inautocomplete = true;

            ThreadAutoComplete = new System.Threading.Thread(new System.Threading.ThreadStart(AutoComplete));
            ThreadAutoComplete.Name = "AutoComplete";
            ThreadAutoComplete.Start();
        }

        private void AutoComplete()
        {
            do
            {
                restartautocomplete = false;
                Console.WriteLine("AutoComplete {0}" , autocompletestring );
                autocompletestrings = func(string.Copy(autocompletestring));    // pass a copy, in case we change it out from under it
                Console.WriteLine("thread Finished");
            } while (restartautocomplete == true);

            Invoke((MethodInvoker)delegate { AutoCompleteFinished(); });
        }

        private void AutoCompleteFinished()
        {
            Console.WriteLine("AutoComplete finished {0}" , autocompletestrings.Count );
            inautocomplete = false;

        }
    }
}
