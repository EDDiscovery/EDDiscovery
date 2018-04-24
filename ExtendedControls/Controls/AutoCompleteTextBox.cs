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
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ExtendedControls
{
    public class AutoCompleteTextBox : TextBoxBorder
    {
        // programtic change of text does not make autocomplete execute.
        public override string Text { get { return base.Text; } set { disableauto = true; base.Text = value; disableauto = false; } }

        public int DropDownWidth { get; set; } = 0;     // means auto size
        public int DropDownHeight { get; set; } = 200;
        public int DropDownItemHeight { get; set; } = 13;
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
        DropDownCustom _cbdropdown;
        int autocompletelastcount = 0;
        private bool disableauto = false;

        public delegate List<string> PerformAutoComplete(string input , AutoCompleteTextBox t);

        public AutoCompleteTextBox() : base()
        {
            TextChanged += TextChangeEventHandler;
            waitforautotimer = new System.Windows.Forms.Timer();
            waitforautotimer.Interval = 200;
            waitforautotimer.Tick += TimeOutTick;
            HandleDestroyed += AutoCompleteTextBox_HandleDestroyed;

            this.Click += AutoCompleteTextBox_Click;
            this.KeyDown += AutoCompleteTextBox_KeyDown;

        }


        // Sometimes, the user is quicker than the timer, and has commited to a selection before the results even come back.
        public void AbortAutoComplete()
        {
            if (waitforautotimer.Enabled)
            {
                waitforautotimer.Stop();
            }
            else if (_cbdropdown != null)
            {
                _cbdropdown.Close();
                _cbdropdown = null;
                Invalidate(true);
            }
        }

        public void SetAutoCompletor(PerformAutoComplete p)
        {
            func = p;
        }

        private void AutoCompleteTextBox_HandleDestroyed(object sender, EventArgs e)
        {
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

        private void TextChangeEventHandler(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("{0} text change event", Environment.TickCount % 10000);
            if (func != null && !disableauto)
            {
                if (!inautocomplete)
                {
                    //System.Diagnostics.Debug.WriteLine("{0} Start timer", Environment.TickCount % 10000);
                    waitforautotimer.Stop();
                    waitforautotimer.Start();
                    autocompletestring = String.Copy(this.Text);    // a copy in case the text box changes it after complete starts
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine("{0} in ac, go again", Environment.TickCount % 10000);
                    autocompletestring = String.Copy(this.Text);
                    restartautocomplete = true;
                }
            }
        }

        private void TimeOutTick(object sender, EventArgs e)
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
                //System.Diagnostics.Debug.WriteLine("{0} Begin AC", Environment.TickCount % 10000);
                restartautocomplete = false;
                autocompletestrings = func(string.Copy(autocompletestring),this);    // pass a copy, in case we change it out from under it
                //System.Diagnostics.Debug.WriteLine("{0} finish func ret {1} restart {2}", Environment.TickCount % 10000, autocompletestrings.Count, restartautocomplete);
            } while (restartautocomplete == true);

            this.BeginInvoke((MethodInvoker)delegate { AutoCompleteFinished(); });
        }

        private void AutoCompleteFinished()
        {
            //System.Diagnostics.Debug.WriteLine("{0} Show results {1}", Environment.TickCount % 10000, autocompletestrings.Count);
            inautocomplete = false;

            int count = autocompletestrings.Count;

            if ( count > 0)
            {
                if ( _cbdropdown != null && (autocompletelastcount < count || autocompletelastcount > count+5))
                {
                    _cbdropdown.Close();
                    _cbdropdown = null;
                }

                if (_cbdropdown == null)
                {
                    _cbdropdown = new DropDownCustom("", false);

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
                    _cbdropdown.Activated += _cbdropdown_DropDown;
                    _cbdropdown.SelectedIndexChanged += _cbdropdown_SelectedIndexChanged;

                    _cbdropdown.Show(FindForm());
                    Focus();                // Major change.. we now keep the focus at all times
                }
                else
                {
                    _cbdropdown.Items.Clear();
                    _cbdropdown.Items = autocompletestrings;
                    _cbdropdown.Refresh();
                }

                autocompletelastcount = count;
            }
            else
            {
                if (_cbdropdown != null)
                {
                    //System.Diagnostics.Debug.WriteLine("{0} Close prev", Environment.TickCount % 10000);
                    _cbdropdown.Close();
                    _cbdropdown = null;
                }
            }
        }

        private void _cbdropdown_DropDown(object sender, EventArgs e)
        {
            Point location = this.PointToScreen(new Point(0, 0));
            _cbdropdown.Location = new Point(location.X, location.Y + this.Height );
            this.Invalidate(true);
        }

        private void _cbdropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedindex = _cbdropdown.SelectedIndex;
            if (selectedindex >= 0 && selectedindex < _cbdropdown.Items.Count)
            {
                this.Text = _cbdropdown.Items[selectedindex];
                this.Select(this.Text.Length, this.Text.Length);
                _cbdropdown.Close();
                _cbdropdown = null;
                this.Invalidate(true);
                Focus();
            }
        }


        private void AutoCompleteTextBox_Click(object sender, EventArgs e)
        {
            if ( _cbdropdown != null )
            {
                _cbdropdown.Close();
                _cbdropdown = null;
            }
        }

        // keys when WE have to focus, which we do all the time now, some need passing onto the control.

        private void AutoCompleteTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("{0} Keypress {1}", Environment.TickCount % 10000 , e.KeyCode);

            if (_cbdropdown != null)        // pass certain keys to the shown drop down
            {
                if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                {
                    _cbdropdown.KeyDownAction(e);
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    _cbdropdown.Close();
                    _cbdropdown = null;
                }
            }
        }


    }

}
