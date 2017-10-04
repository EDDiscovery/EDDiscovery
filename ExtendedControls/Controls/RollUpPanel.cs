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
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class RollUpPanel : Panel
    {
        public int RollUpDelay { get; set; } = 1000;            // before rolling
        public int UnrollHoverDelay { get; set; } = 1000;       // set to large value and forces click to open functionality
        public int UnrolledHeight { get; set; } = 32;
        public int RolledUpHeight { get; set; } = 5;
        public int RollUpAnimationTime { get; set; } = 500;            // animation time
        public bool ShowHiddenMarker { get { return hiddenmarkershow; } set { hiddenmarkershow = value; SetHMViz();} }

        public int HiddenMarkerWidth { get; set; } = 0;   //0 = full width

        public bool PinState { get { return pinbutton.Checked; } set { SetPinState(value); } }

        public event EventHandler DeployStarting;
        public event EventHandler DeployCompleted;
        public event EventHandler RetractStarting;
        public event EventHandler RetractCompleted;

        private CheckBoxCustom pinbutton;        // public so you can theme them with colour/IAs
        private DrawnPanel hiddenmarker;

        long targetrolltickstart;     // when the roll is supposed to be in time
        const int rolltimerinterval = 25;

        Action<RollUpPanel, CheckBoxCustom> PinStateChanged = null;

        enum Mode { None, PauseBeforeRollDown, PauseBeforeRollUp, RollUp, RollDown};
        Mode mode;
        Timer timer;
        bool hiddenmarkershow = true;           // if to show it at all.
        bool hiddenmarkershouldbeshown = false; // if to show it now

        public RollUpPanel()
        {
            SuspendLayout();

            this.Height = UnrolledHeight;

            pinbutton = new CheckBoxCustom();
            pinbutton.Appearance = Appearance.Normal;
            pinbutton.FlatStyle = FlatStyle.Popup;
            pinbutton.Size = new Size(24, 24);
            pinbutton.Image = ExtendedControls.Properties.Resources.pindownwhite2;          //colours 222 and 255 used
            pinbutton.ImageUnchecked = ExtendedControls.Properties.Resources.pinupwhite2;
            pinbutton.Checked = true;
            pinbutton.CheckedChanged += Pinbutton_CheckedChanged;
            pinbutton.Name = "RUP Pinbutton";

            hiddenmarker = new DrawnPanelNoTheme();
            hiddenmarker.Name = "Hidden marker";
            hiddenmarker.Location = new Point(0, 0);
            hiddenmarker.ImageSelected = DrawnPanel.ImageType.Bars;
            hiddenmarker.Size = new Size(100, 3);
            hiddenmarker.Visible = false;
            hiddenmarker.Click += Hiddenmarker_Click;

            Controls.Add(pinbutton);
            Controls.Add(hiddenmarker);

            ResumeLayout();

            mode = Mode.None; 
            timer = new Timer();
            timer.Tick += Timer_Tick;

            pinbutton.Visible = false;
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            pinbutton.BackColor = BackColor;
            hiddenmarker.BackColor = BackColor;
        }

        private void Hiddenmarker_Click(object sender, EventArgs e)
        {
            if ( mode == Mode.PauseBeforeRollDown )
            {
                //System.Diagnostics.Debug.WriteLine("Cancel roll down pause and expand now");
                timer.Stop();
                Timer_Tick(sender, e);
            }
        }

        private void SetHMViz()
        {
            hiddenmarker.Visible = hiddenmarkershow && hiddenmarkershouldbeshown;
        }

        public void SetToolTip(ToolTip t, string ttpin = null, string ttmarker = null)
        {
            if (ttpin == null)
                ttpin = "Pin to stop this menu bar disappearing automatically";
            if ( ttmarker == null )
                ttmarker = "Click or hover over this to unroll the menu bar";
            t.SetToolTip(pinbutton, ttpin);
            t.SetToolTip(hiddenmarker, ttmarker);
        }

        public void SetPinState(bool state)
        {
            pinbutton.Checked = state;
            if (state)
                RollDown();
            else
                RollUp();
        }

        private void Pinbutton_CheckedChanged(object sender, EventArgs e)
        {
            PinStateChanged?.Invoke(this, pinbutton);
        }


        public void SetPinImages(Image up , Image down)
        {
            pinbutton.Image = up;
            pinbutton.ImageUnchecked = down;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            //System.Diagnostics.Debug.WriteLine("Added " + e.Control.Name + " " + e.Control.GetType().Name);
            e.Control.MouseEnter += Control_MouseEnter;
            e.Control.MouseLeave += Control_MouseLeave;
        }

        public void RollDown()
        {
            timer.Stop();

            if (Height < UnrolledHeight)
            {
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " roll down, start animating");
                mode = Mode.RollDown;
                targetrolltickstart = Environment.TickCount;
                timer.Interval = rolltimerinterval;
                DeployStarting?.Invoke(this, EventArgs.Empty);
                timer.Start();

                foreach (Control c in Controls)     // everything except hidden marker visible
                {
                    if (!Object.ReferenceEquals(c, hiddenmarker))
                        c.Visible = true;
                }
            }
            else
            {
                mode = Mode.None;
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " ignore roll down at max h");
            }
        }

        public void RollUp()
        {
            timer.Stop();

            if (Height > RolledUpHeight)
            {
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " roll up, start animating");
                mode = Mode.RollUp;
                targetrolltickstart = Environment.TickCount;
                timer.Interval = rolltimerinterval;
                RetractStarting?.Invoke(this, EventArgs.Empty);
                timer.Start();
            }
            else
            {
                mode = Mode.None;
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " ignore roll up at min h");
            }
        }

        private void StartRollUpTimer()
        {
            if (mode == Mode.None && Height != RolledUpHeight)
            {
                timer.Stop();
                timer.Interval = RollUpDelay;
                timer.Start();
                mode = Mode.PauseBeforeRollUp;
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " Start roll up timer");
            }
        }

        private void StartRollDownTimer()
        {
            if (mode == Mode.None && Height == RolledUpHeight)
            {
                timer.Stop();
                timer.Interval = UnrollHoverDelay;
                timer.Start();
                mode = Mode.PauseBeforeRollDown;
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " begin wait for unroll");
            }
            else if ( mode == Mode.PauseBeforeRollUp )      // if in a roll up.. we stop it
            {
                timer.Stop();
                mode = Mode.None;
                //System.Diagnostics.Debug.WriteLine("Stop roll up timer");
            }
        }

        bool inarea = false;

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            //System.Diagnostics.Debug.WriteLine("RUP Enter panel");
            base.OnMouseEnter(eventargs);
            inarea = true;
            StartRollDownTimer();
            pinbutton.Visible = true;
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("RUP Entered " + sender.GetType().Name);
            inarea = true;
            StartRollDownTimer();
            pinbutton.Visible = true;
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            //System.Diagnostics.Debug.WriteLine("RUP Left panel");
            inarea = false;
            base.OnMouseLeave(eventargs);
            StartRollUpTimer();
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("RUP Left " + sender.GetType().Name);
            inarea = false;
            StartRollUpTimer();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            if (ClientRectangle.Width > 0)
            {
                pinbutton.Left = ClientRectangle.Width - pinbutton.Width - 8;
                pinbutton.Top = 3;

                int hmwidth = Math.Abs(HiddenMarkerWidth);
                if (hmwidth == 0)
                    hmwidth = ClientRectangle.Width;

                hiddenmarker.Left = (HiddenMarkerWidth<0) ? ((ClientRectangle.Width - hmwidth)/2) : 0;
                hiddenmarker.Width = hmwidth;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            double rollpercent = (double)(Environment.TickCount - targetrolltickstart) / RollUpAnimationTime;
            int rolldiff = UnrolledHeight - RolledUpHeight;
            //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " " + rollpercent);

            if (mode == Mode.RollUp)        // roll up animation, move one step on, check for end
            {
                Height = Math.Max((int)(UnrolledHeight - rolldiff * rollpercent), RolledUpHeight);
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " At " + Height);

                if (Height == RolledUpHeight)    // end
                {
                    timer.Stop();
                    foreach (Control c in Controls)
                    {
                        if (!Object.ReferenceEquals(c, hiddenmarker))       // everything hidden but hm
                            c.Visible = false;
                    }

                    hiddenmarkershouldbeshown = true;
                    SetHMViz();
                    //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " At min h");

                    mode = Mode.None;
                    RetractCompleted?.Invoke(this, EventArgs.Empty);
                }
            }
            else if (mode == Mode.RollDown) // roll down animation, move one step on, check for end
            {
                Height = Math.Min((int)(RolledUpHeight + rolldiff * rollpercent), UnrolledHeight);
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " At " + Height);

                if (Height == UnrolledHeight)        // end, everything is already visible.  hide the hidden marker
                {
                    timer.Stop();
                    mode = Mode.None;
                    hiddenmarkershouldbeshown = false;
                    SetHMViz();
                    DeployCompleted?.Invoke(this, EventArgs.Empty);
                }

                if (!inarea && !pinbutton.Checked)      // but not in area now, and not held.. so start roll up procedure
                    StartRollUpTimer();
            }
            else if (mode == Mode.PauseBeforeRollDown) // timer up.. check if in area, if so roll down
            {
                mode = Mode.None;
                timer.Stop();

                if (inarea)
                    RollDown();
                else
                {
                    //System.Diagnostics.Debug.WriteLine("Ignore roll down not in area " + inarea + " checked " + pinbutton.Checked);
                }
            }
            else if (mode == Mode.PauseBeforeRollUp)    // timer up, check if out of area and not pinned, if so roll up
            {
                mode = Mode.None;
                timer.Stop();

                pinbutton.Visible = inarea;     // visible is same as inarea flag..

                if (!inarea && !pinbutton.Checked)      // if not in area, and its not checked..
                    RollUp();
                else
                {
                    //System.Diagnostics.Debug.WriteLine("Ignore roll up.. area " + inarea + " checked " + pinbutton.Checked);
                }
            }
        }


    }
}