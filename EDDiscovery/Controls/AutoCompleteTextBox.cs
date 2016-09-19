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
        // programtic change of text does not make autocomplete execute.
        public override string Text { get { return base.Text; } set { disableauto = true; base.Text = value; disableauto = false; } }

        public int DropDownWidth { get; set; } = 0;     // means auto size
        public int DropDownHeight { get; set; } = 200;
        public int DropDownItemHeight { get; set; } = 20;
        public Color DropDownBackgroundColor { get; set; } = Color.Gray;
        public Color DropDownBorderColor { get; set; } = Color.Green;
        public Color DropDownScrollBarColor { get; set; } = Color.LightGray;
        public Color DropDownScrollBarButtonColor { get; set; } = Color.LightGray;
        public Color DropDownMouseOverBackgroundColor { get; set; } = Color.Red;
        public FlatStyle FlatStyle { get; set; } = FlatStyle.System;


        private System.Windows.Forms.Timer waitforautotimer;
        private bool inautocomplete = false;
        private string autocompletestring;
        private bool restartautocomplete = false;
        private System.Threading.Thread ThreadAutoComplete;
        private PerformAutoComplete func = null;
        private List<string> autocompletestrings = null;
        ComboBoxCustomDropdown _cbdropdown;
        private bool isActivated = false;
        private bool disableauto = false;

        public delegate List<string> PerformAutoComplete(string input);

        public AutoCompleteTextBox() : base()
        {
            TextChanged += TextChangeEventHandler;
            waitforautotimer = new System.Windows.Forms.Timer();
            waitforautotimer.Interval = 200;
            waitforautotimer.Tick += TimeOutTick;
        }

        public void SetAutoCompletor(PerformAutoComplete p)
        {
            func = p;
        }

        protected void TextChangeEventHandler(object sender, EventArgs e)
        {
            if (func != null && !isActivated && !disableauto)
            {
                if (!inautocomplete)
                {
                    //Console.WriteLine("{0} Start timer", Environment.TickCount % 10000);
                    waitforautotimer.Stop();
                    waitforautotimer.Start();
                    autocompletestring = String.Copy(this.Text);    // a copy in case the text box changes it after complete starts
                }
                else
                {
                    //Console.WriteLine("{0} in ac, go again", Environment.TickCount % 10000);
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
                //Console.WriteLine("{0} Begin AC", Environment.TickCount % 10000);
                restartautocomplete = false;
                autocompletestrings = func(string.Copy(autocompletestring));    // pass a copy, in case we change it out from under it
                //Console.WriteLine("{0} finish func ret {1} restart {2}", Environment.TickCount % 10000, autocompletestrings.Count, restartautocomplete);
            } while (restartautocomplete == true);

            //Console.WriteLine("{0} Finish AC", Environment.TickCount % 10000);
            Invoke((MethodInvoker)delegate { AutoCompleteFinished(); });
        }


        private void AutoCompleteFinished()
        {
            //Console.WriteLine("{0} Show results {1}", Environment.TickCount % 10000, autocompletestrings.Count);
            inautocomplete = false;

            if (autocompletestrings.Count > 0)
            {
                if (_cbdropdown != null)
                {
                    _cbdropdown.Close();
                }

                _cbdropdown = new ComboBoxCustomDropdown();

                int fittableitems = this.DropDownHeight / this.DropDownItemHeight;

                if (fittableitems == 0)
                {
                    fittableitems = 5;
                }

                if (fittableitems > autocompletestrings.Count())                             // no point doing more than we have..
                    fittableitems = autocompletestrings.Count();

                _cbdropdown.Size = new Size(this.DropDownWidth > 0 ? this.DropDownWidth : this.Width, fittableitems * this.DropDownItemHeight + 4);

                _cbdropdown.SelectionBackColor = this.DropDownBackgroundColor;
                _cbdropdown.ForeColor = this.ForeColor;
                _cbdropdown.BackColor = this.DropDownBorderColor;
                _cbdropdown.BorderColor = this.DropDownBorderColor;
                _cbdropdown.Items = autocompletestrings;
                _cbdropdown.ItemHeight = this.DropDownItemHeight;
                _cbdropdown.SelectedIndex = 0;
                _cbdropdown.FlatStyle = this.FlatStyle;
                _cbdropdown.Font = this.Font;
                _cbdropdown.ScrollBarColor = this.DropDownScrollBarColor;
                _cbdropdown.ScrollBarButtonColor = this.DropDownScrollBarButtonColor;
                _cbdropdown.MouseOverBackgroundColor = this.DropDownMouseOverBackgroundColor;

                _cbdropdown.DropDown += _cbdropdown_DropDown;
                _cbdropdown.SelectedIndexChanged += _cbdropdown_SelectedIndexChanged;
                _cbdropdown.KeyPressed += _cbdropdown_KeyPressed;
                _cbdropdown.OtherKeyPressed += _cbdropdown_OtherKeyPressed;
                _cbdropdown.Deactivate += _cbdropdown_Deactivate;

                Control parent = this.Parent;
                while (parent != null && !(parent is Form))
                {
                    parent = parent.Parent;
                }

                _cbdropdown.Show(parent);
            }
        }

        private void _cbdropdown_DropDown(object sender, EventArgs e)
        {
            Point location = this.PointToScreen(new Point(0, 0));
            _cbdropdown.Location = new Point(location.X, location.Y + this.Height + this.BorderOffset);
            isActivated = true;
            this.Invalidate(true);
        }

        private void _cbdropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedindex = _cbdropdown.SelectedIndex;
            this.Text = _cbdropdown.Items[selectedindex];
            this.Select(this.Text.Length, this.Text.Length);
            _cbdropdown.Close();
            isActivated = false;
            this.Invalidate(true);
            Focus();
        }

        private void _cbdropdown_KeyPressed(object sender, KeyPressEventArgs e)
        {
            _cbdropdown.Close();
            isActivated = false;
            base.Text += e.KeyChar;     // to trigger an autocomplete, bypassing the Text override
            this.Select(this.Text.Length, this.Text.Length);
            Focus();
        }

        private void _cbdropdown_OtherKeyPressed(object sender, KeyEventArgs e)
        {
            _cbdropdown.Close();
            isActivated = false;

            if (e.KeyCode == Keys.Back && this.Text.Length > 0)
            {
                base.Text = this.Text.Substring(0, this.Text.Length - 1);       // we want to trigger a autocomplete, so use the base func
                this.Select(this.Text.Length, this.Text.Length);
                Focus();
            }
        }

        private void _cbdropdown_Deactivate(object sender, EventArgs e)
        {
            isActivated = false;
            this.Invalidate(true);
        }
    }

}
