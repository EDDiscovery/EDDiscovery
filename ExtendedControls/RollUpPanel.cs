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
        public int RollUpDelay { get; set; } = 1000;
        public int UnrollHoverDelay { get; set; } = 1000;
        public int UnrolledHeight { get; set; } = 40;
        public int RolledUpHeight { get; set; } = 5;

        public Color HiddenMarkerColor { get { return hiddenmarkercolor; } set { hiddenmarkercolor = value; if (hiddenmarker != null) hiddenmarker.ForeColor = value; } }
        public Color HiddenMarkerMouseOverColor { get { return hiddenmarkermouseovercolor; } set { hiddenmarkermouseovercolor = value; if (hiddenmarker != null) hiddenmarker.MouseOverColor = value; } }

        CheckBoxCustom pinbutton;
        DrawnPanelNoTheme hiddenmarker;
        private Color hiddenmarkercolor = Color.Red;
        private Color hiddenmarkermouseovercolor = Color.Green;


        enum Mode { None, PauseBeforeRollDown, PauseBeforeRollUp, RollUp, RollDown};
        Mode mode;
        Timer timer;

        public RollUpPanel()
        {
            SuspendLayout();
            pinbutton = new CheckBoxCustom();
            pinbutton.Appearance = Appearance.Normal;
            pinbutton.FlatStyle = FlatStyle.Popup;
            pinbutton.Size = new Size(24, 24);
            pinbutton.Image = ExtendedControls.Properties.Resources.pindownred;
            pinbutton.ImageUnchecked = ExtendedControls.Properties.Resources.pinupred;

            hiddenmarker = new DrawnPanelNoTheme();
            hiddenmarker.Location = new Point(0, 0);
            hiddenmarker.ImageSelected = DrawnPanel.ImageType.Bars;
            hiddenmarker.Size = new Size(100, 3);
            hiddenmarker.Visible = false;
            hiddenmarker.MouseOverColor = HiddenMarkerMouseOverColor;
            hiddenmarker.ForeColor = HiddenMarkerColor;

            Controls.Add(pinbutton);
            Controls.Add(hiddenmarker);

            ResumeLayout();

            mode = Mode.None; 
            timer = new Timer();
            timer.Tick += Timer_Tick;
        }

        public void SetPinState(bool state)
        {
            pinbutton.Checked = state;
            if (state)
                RollDown();
        }

        public void SetPinImages(Image up , Image down)
        {
            pinbutton.Image = up;
            pinbutton.ImageUnchecked = down;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            e.Control.MouseEnter += Control_MouseEnter;
            e.Control.MouseLeave += Control_MouseLeave;
        }

        public void RollDown()
        {
            timer.Stop();

            if (Height < UnrolledHeight)
            {
                System.Diagnostics.Debug.WriteLine(Environment.TickCount + " roll down, start animating");
                mode = Mode.RollDown;
                timer.Interval = 25;
                timer.Start();

                foreach (Control c in Controls)
                {
                    if (!Object.ReferenceEquals(c, hiddenmarker))
                        c.Visible = true;
                }
            }
            else
            {
                mode = Mode.None;
                System.Diagnostics.Debug.WriteLine(Environment.TickCount + " ignore roll down at max h");
            }
        }

        public void RollUp()
        {
            timer.Stop();

            if (Height > RolledUpHeight)
            {
                System.Diagnostics.Debug.WriteLine(Environment.TickCount + " roll up, start animating");
                mode = Mode.RollUp;
                timer.Interval = 25;
                timer.Start();
            }
            else
            {
                mode = Mode.None;
                System.Diagnostics.Debug.WriteLine(Environment.TickCount + " ignore roll up at min h");
            }
        }


        public void StartRollUpTimer()
        {
            if (!pinbutton.Checked && RollUpDelay > 0 && mode == Mode.None && Height != RolledUpHeight)
            {
                timer.Stop();
                timer.Interval = RollUpDelay;
                timer.Start();
                mode = Mode.PauseBeforeRollUp;
                System.Diagnostics.Debug.WriteLine(Environment.TickCount + " Start roll up timer");
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
                System.Diagnostics.Debug.WriteLine(Environment.TickCount + " begin wait for unroll");
            }
        }

        bool inarea = false;

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            inarea = true;
            StartRollDownTimer();
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
            inarea = true;
            StartRollDownTimer();
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            inarea = false;
            base.OnMouseLeave(eventargs);
            StartRollUpTimer();
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
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
                hiddenmarker.Width = ClientRectangle.Width;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (mode == Mode.RollUp)
            {
                int h = Height - 2;
                h = Math.Max(Math.Min(h, UnrolledHeight), RolledUpHeight);
                Height = h;

                if (h == RolledUpHeight)
                {
                    timer.Stop();
                    foreach (Control c in Controls)
                    {
                        if (!Object.ReferenceEquals(c, hiddenmarker))
                            c.Visible = false;
                    }

                    hiddenmarker.Visible = true;
                    System.Diagnostics.Debug.WriteLine(Environment.TickCount + " At min h");

                    mode = Mode.None;
                }
            }
            else if (mode == Mode.RollDown)
            {
                int h = Height + 2;
                h = Math.Max(Math.Min(h, UnrolledHeight), RolledUpHeight);
                Height = h;

                if (h == UnrolledHeight)
                {
                    timer.Stop();
                    mode = Mode.None;
                }

                if (!inarea && !pinbutton.Checked)      // but not in area now, and not held.. so start roll up procedure
                    StartRollUpTimer();
            }
            else if (mode == Mode.PauseBeforeRollDown)
            {
                mode = Mode.None;
                timer.Stop();
                RollDown();
            }
            else if (mode == Mode.PauseBeforeRollUp)
            {
                mode = Mode.None;
                timer.Stop();
                if (!inarea && !pinbutton.Checked)
                {
                    RollUp();
                }
                else
                    System.Diagnostics.Debug.WriteLine("Ignore roll up.. area " + inarea + " checked " + pinbutton.Checked);
            }
        }


    }
}