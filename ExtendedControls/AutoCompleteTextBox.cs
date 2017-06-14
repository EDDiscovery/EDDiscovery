/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class AutoCompleteTextBox : TextBoxBorder
    {
        #region Public interfaces

        #region ctors

        public AutoCompleteTextBox()
        {
            waitforautotimer = new Timer();
            waitforautotimer.Interval = 200;
            waitforautotimer.Tick += Waitforautotimer_Tick;
        }

        #endregion // ctors

        #region Properties

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

        #endregion // Properties

        #region Delegates

        public delegate List<string> PerformAutoComplete(string input, AutoCompleteTextBox t);

        #endregion // Delegates

        #region Methods

        // Sometimes, the user is quicker than the timer, and has commited to a selection before the results even come back.
        public void AbortAutoComplete()
        {
            if (waitforautotimer.Enabled)
                waitforautotimer.Stop();
            else if (isActivated && _cbdropdown != null)
            {
                isActivated = false;
                _cbdropdown.Close();
                Invalidate(true);
            }
        }

        public void SetAutoCompletor(PerformAutoComplete p)
        {
            func = p;
        }

        #endregion // Methods

        #endregion // Public interfaces


        #region Implementation

        #region Fields

        private Timer waitforautotimer;                     // ✓
        private ComboBoxCustomDropdown _cbdropdown;         // ✓

        private System.Threading.Thread ThreadAutoComplete; // TODO: this is nullified in Dispose, without regard to stopping it. It should be ok, right?

        private PerformAutoComplete func = null;            // ✓
        private List<string> autocompletestrings = null;    // ✓

        private bool inautocomplete = false;
        private string autocompletestring = string.Empty;
        private bool restartautocomplete = false;
        private bool isActivated = false;
        private bool disableauto = false;

        #endregion // Fields

        #region OnEvent overrides

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            if (func != null)
            {
                waitforautotimer.Stop();
                restartautocomplete = false;

                if (ThreadAutoComplete != null && ThreadAutoComplete.IsAlive)
                {
                    ThreadAutoComplete.Join();
                }
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (func != null && !isActivated && !disableauto)
            {
                autocompletestring = string.Copy(Text);    // a copy in case the text box changes it after complete starts

                if (!inautocomplete)
                {
                    //Console.WriteLine("{0} Start timer", Environment.TickCount % 10000);
                    waitforautotimer.Stop();
                    waitforautotimer.Start();
                }
                else
                    restartautocomplete = true;
            }
        }

        #endregion // OnEvent overrides

        #region Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (waitforautotimer != null)
                {
                    if (waitforautotimer.Enabled)
                        waitforautotimer.Stop();
                    waitforautotimer.Dispose();
                }
                autocompletestrings?.Clear();
                _cbdropdown?.Dispose();
            }

            autocompletestrings = null;
            _cbdropdown = null;
            func = null;
            ThreadAutoComplete = null;
            waitforautotimer = null;

            base.Dispose(disposing);
        }

        [STAThread]
        private void AutoComplete()
        {
            try
            {
                do
                {
                    //Console.WriteLine("{0} Begin AC", Environment.TickCount % 10000);
                    restartautocomplete = false;
                    autocompletestrings = func(string.Copy(autocompletestring), this);    // pass a copy, in case we change it out from under it
                                                                                          //Console.WriteLine("{0} finish func ret {1} restart {2}", Environment.TickCount % 10000, autocompletestrings.Count, restartautocomplete);
                } while (restartautocomplete == true);

                BeginInvoke((MethodInvoker)delegate { AutoCompleteFinished(); });
            }
            catch (System.Threading.ThreadAbortException) { }
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

                int fittableitems = DropDownHeight / DropDownItemHeight;

                if (fittableitems == 0)
                {
                    fittableitems = 5;
                }

                if (fittableitems > autocompletestrings.Count())                             // no point doing more than we have..
                    fittableitems = autocompletestrings.Count();

                _cbdropdown.Size = new Size(DropDownWidth > 0 ? DropDownWidth : Width, fittableitems * DropDownItemHeight + 4);

                _cbdropdown.SelectionBackColor = DropDownBackgroundColor;
                _cbdropdown.ForeColor = ForeColor;
                _cbdropdown.BackColor = DropDownBorderColor;
                _cbdropdown.BorderColor = DropDownBorderColor;
                _cbdropdown.Items = autocompletestrings;
                _cbdropdown.ItemHeight = DropDownItemHeight;
                _cbdropdown.SelectedIndex = 0;
                _cbdropdown.FlatStyle = FlatStyle;
                _cbdropdown.Font = Font;
                _cbdropdown.ScrollBarColor = DropDownScrollBarColor;
                _cbdropdown.ScrollBarButtonColor = DropDownScrollBarButtonColor;
                _cbdropdown.MouseOverBackgroundColor = DropDownMouseOverBackgroundColor;

                _cbdropdown.DropDown += _cbdropdown_DropDown;
                _cbdropdown.SelectedIndexChanged += _cbdropdown_SelectedIndexChanged;
                _cbdropdown.KeyPressed += _cbdropdown_KeyPressed;
                _cbdropdown.OtherKeyPressed += _cbdropdown_OtherKeyPressed;
                _cbdropdown.Deactivate += _cbdropdown_Deactivate;

                Control parent = Parent;
                while (parent != null && !(parent is Form))
                {
                    parent = parent.Parent;
                }

                _cbdropdown.Show(parent);
            }
        }

        #endregion // Methods

        #region Event handlers

        private void Waitforautotimer_Tick(object sender, EventArgs e)
        {
            waitforautotimer.Stop();
            inautocomplete = true;

            ThreadAutoComplete = new System.Threading.Thread(new System.Threading.ThreadStart(AutoComplete));
            ThreadAutoComplete.Name = "AutoComplete";
            ThreadAutoComplete.SetApartmentState(System.Threading.ApartmentState.STA);
            ThreadAutoComplete.Start();
        }


        private void _cbdropdown_DropDown(object sender, EventArgs e)
        {
            Point location = PointToScreen(new Point(0, 0));
            _cbdropdown.Location = new Point(location.X, location.Y + Height + BorderOffset);
            isActivated = true;
            Invalidate(true);
        }

        private void _cbdropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedindex = _cbdropdown.SelectedIndex;
            if (selectedindex >= 0)
            {
                Text = _cbdropdown.Items[selectedindex];
                Select(Text.Length, Text.Length);
                _cbdropdown.Close();
                isActivated = false;
                Invalidate(true);
                Focus();
            }
        }

        private void _cbdropdown_KeyPressed(object sender, KeyPressEventArgs e)
        {
            _cbdropdown.Close();
            isActivated = false;
            base.Text += e.KeyChar;     // to trigger an autocomplete, bypassing the Text override
            Select(Text.Length, Text.Length);
            Focus();
        }

        private void _cbdropdown_OtherKeyPressed(object sender, KeyEventArgs e)
        {
            _cbdropdown.Close();
            isActivated = false;

            if (e.KeyCode == Keys.Back && Text.Length > 0)
            {
                base.Text = Text.Substring(0, Text.Length - 1);       // we want to trigger a autocomplete, so use the base func
                Select(Text.Length, Text.Length);
                Focus();
            }
        }

        private void _cbdropdown_Deactivate(object sender, EventArgs e)
        {
            isActivated = false;
            Invalidate(true);
        }

        #endregion // Event handlers

        #endregion // Implementation
    }
}
