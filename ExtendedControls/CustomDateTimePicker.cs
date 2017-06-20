﻿/*
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
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class CustomDateTimePicker : Control
    {
        #region Public interfaces

        #region ctors

        public CustomDateTimePicker()
        {
            SetStyle(ControlStyles.Selectable, true);
            Recalc();
            Controls.Add(updown);
            Controls.Add(calendaricon);
            Controls.Add(checkbox);

            updown.Selected += Updown_Selected;
            calendaricon.MouseClick += Calendaricon_MouseClick;
            calendar.DateSelected += Calendar_DateSelected;
            checkbox.CheckedChanged += Checkbox_CheckedChanged;
        }

        #endregion

        #region Properties

        public UpDown updown { get; } = new UpDown();                            // ✓ (this.Controls) for setting colours
        public CheckBoxCustom checkbox { get; } = new CheckBoxCustom();          // ✓ (this.Controls) for setting colours, note background is forces to TextBackColour

        public DateTime Value { get { return datetimevalue; } set { datetimevalue = value; Invalidate(); } }

        // Fore = colour of text
        // Back = colour of background of control
        public Color TextBackColor { get { return textbackcolor; } set { textbackcolor = checkbox.BackColor = calendaricon.BackColor = value; Invalidate(true); } }
        public Color SelectedColor { get; set; } = Color.Yellow;    // colour when item is selected.
        public Color BorderColor { get { return bordercolor; } set { bordercolor = value; PerformLayout(); } }
        public float BorderColorScaling { get; set; } = 0.5F;           // Popup style only

        public string CustomFormat { get { return customformat; } set { customformat = value; Recalc(); } }
        public DateTimePickerFormat Format { get { return format; } set { SetFormat(value); } }
        public bool ShowUpDown { get { return showupdown; } set { showupdown = value; PerformLayout(); } }
        public bool ShowCheckBox { get { return showcheckbox; } set { showcheckbox = value; PerformLayout(); } }
        public bool Checked { get { return checkbox.Checked; } set { checkbox.Checked = value; } }

        #endregion // Properties

        #region Events

        public event EventHandler ValueChanged; // ✓

        #endregion // Events

        #region Methods

        public void UpDown(int dir)
        {
            if (selectedpart != -1)
            {
                Parts p = partlist[selectedpart];
                if (p.ptype == PartsTypes.Day)
                    datetimevalue = datetimevalue.AddDays(dir);
                else if (p.ptype == PartsTypes.Month)
                    datetimevalue = datetimevalue.AddMonths(dir);
                else if (p.ptype == PartsTypes.Year)
                    datetimevalue = datetimevalue.AddYears(dir);
                else if (p.ptype == PartsTypes.Hours)
                    datetimevalue = datetimevalue.AddHours(dir);
                else if (p.ptype == PartsTypes.Mins)
                    datetimevalue = datetimevalue.AddMinutes(dir);
                else if (p.ptype == PartsTypes.Seconds)
                    datetimevalue = datetimevalue.AddSeconds(dir);
                else if (p.ptype == PartsTypes.AmPm)
                    datetimevalue = datetimevalue.AddHours((datetimevalue.Hour >= 12) ? -12 : 12);
                else
                    return;

                OnValueChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        #endregion // Methods

        #endregion // Public interfaces


        #region Implementation

        #region Fields

        private MonthCalendar calendar = new MonthCalendar();           // ✓ (FindForm().Controls) calendar, when rendered by window, is colour locked
        private PictureBox calendaricon = new PictureBox();             // ✓ (this.Controls) does not need colour.. icon

        private List<Parts> partlist = new List<Parts>();               // ✓

        private DateTime datetimevalue = DateTime.Now;
        private DateTimePickerFormat format = DateTimePickerFormat.Long;
        private string customformat = CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern;
        private bool showupdown = false;
        private bool showcheckbox = false;
        private Color textbackcolor = Color.DarkBlue;
        private Color bordercolor = Color.Transparent;
        private int xstart = 0;                             // where the text starts

        private int selectedpart = -1;
        private string keybuffer;

        #endregion // Fields

        #region OnEvent overrides & dispatchers

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            //System.Diagnostics.Debug.WriteLine("Key down" + e.KeyCode);

            if (e.KeyCode == Keys.Up)
                UpDown(1);
            else if (e.KeyCode == Keys.Down)
                UpDown(-1);
            else if (e.KeyCode == Keys.Left && selectedpart > 0)
            {
                int findprev = selectedpart - 1; // back 1
                while (findprev >= 0 && partlist[findprev].ptype < PartsTypes.Day)       // back until valid
                    findprev--;

                if (findprev >= 0)
                {
                    selectedpart = findprev;
                    Invalidate();
                }
            }
            else if (e.KeyCode == Keys.Right && selectedpart < partlist.Count - 1)
            {
                int findnext = selectedpart + 1; // fwd 1
                while (findnext < partlist.Count && partlist[findnext].ptype < PartsTypes.Day)       // fwd until valid
                    findnext++;

                if (findnext < partlist.Count)
                {
                    selectedpart = findnext;
                    Invalidate();
                }
            }
            else if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
            {
                keybuffer += (char)((e.KeyCode - Keys.D0) + '0');
                if (!TryNum(keybuffer))
                {
                    keybuffer = "";
                    keybuffer += (char)((e.KeyCode - Keys.D0) + '0');
                    TryNum(keybuffer);
                }
            }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            if (ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
            {
                int offset = BorderColor.IsFullyTransparent() ? 0 : 2;
                int height = ClientRectangle.Height - offset * 2;
                checkbox.Visible = ShowCheckBox;
                checkbox.Location = new Point(offset + 2, offset);
                checkbox.Size = new Size(height, height);

                updown.Visible = ShowUpDown;
                updown.Location = new Point(ClientRectangle.Width - ClientRectangle.Height - offset, offset);
                updown.Size = new Size(height, height);

                calendaricon.Image = Properties.Resources.Calendar;
                //calendaricon.Image = DialogTest.Properties.Resources.Calendar;
                calendaricon.Location = new Point(ClientRectangle.Width - offset - calendaricon.Image.Width - 4, ClientRectangle.Height / 2 - calendaricon.Image.Height / 2);
                calendaricon.Size = calendaricon.Image.Size;
                calendaricon.Visible = !ShowUpDown;
                calendaricon.SizeMode = PictureBoxSizeMode.CenterImage;

                xstart = (showcheckbox ? (checkbox.Right + 2) : 2) + (BorderColor.IsFullyTransparent() ? 2 : 0);

                Recalc();   // cause anything might have changed, like fonts
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (selectedpart != -1)
            {
                selectedpart = -1;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            //System.Diagnostics.Debug.WriteLine("Mouse down");

            for (int i = 0; i < partlist.Count; i++)
            {
                if (partlist[i].ptype >= PartsTypes.Day && mevent.X >= partlist[i].xpos + xstart && mevent.X <= partlist[i].endx + xstart)
                {
                    //System.Diagnostics.Debug.WriteLine(".. on " + i);
                    if (selectedpart == i)      // click again, increment
                    {
                        Focus();
                        UpDown((mevent.Button == MouseButtons.Right) ? -1 : 1);
                        break;
                    }
                    else
                    {
                        selectedpart = i;
                        Focus();
                        Invalidate();
                        break;
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle drawarea = ClientRectangle;

            if (!BorderColor.IsFullyTransparent())
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                Color color1 = BorderColor;
                Color color2 = BorderColor.Multiply(BorderColorScaling);

                using (GraphicsPath g1 = RectCutCorners(1, 1, ClientRectangle.Width - 2, ClientRectangle.Height - 1, 1, 1))
                using (Pen pc1 = new Pen(color1, 1.0F))
                    e.Graphics.DrawPath(pc1, g1);

                using (GraphicsPath g2 = RectCutCorners(0, 0, ClientRectangle.Width, ClientRectangle.Height - 1, 2, 2))
                using (Pen pc2 = new Pen(color2, 1.0F))
                    e.Graphics.DrawPath(pc2, g2);

                drawarea.Inflate(-2, -2);
            }

            using (Brush br = new SolidBrush(TextBackColor))
                e.Graphics.FillRectangle(br, drawarea);

            using (Brush textb = new SolidBrush(ForeColor))
            {
                for (int i = 0; i < partlist.Count; i++)
                {
                    Parts p = partlist[i];

                    string t = (p.ptype == PartsTypes.Text) ? p.maxstring : datetimevalue.ToString(p.format);

                    if (i == selectedpart)
                    {
                        using (Brush br = new SolidBrush(SelectedColor))
                            e.Graphics.FillRectangle(br, new Rectangle(p.xpos + xstart, drawarea.Y, p.endx - p.xpos, drawarea.Height));
                    }

                    e.Graphics.DrawString(t, Font, textb, new Point(p.xpos + xstart, drawarea.Y + 2));
                }
            }
        }


        protected virtual void OnValueChanged(EventArgs args)
        {
            ValueChanged?.Invoke(this, args);
        }

        #endregion // OnEvent overrides & dispatchers

        #region private enum PartsTypes

        private enum PartsTypes
        {
            Text,
            DayName,
            Day,
            Month,
            Year,
            Hours,
            Mins,
            Seconds,
            AmPm
        }

        #endregion // private enum PartsTypes

        #region private class Parts : object

        private class Parts
        {
            public PartsTypes ptype;
            public string maxstring;
            public string format;
            public int xpos;
            public int endx;
        };

        #endregion // private class Parts : object

        #region Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                partlist?.Clear();
            }
            partlist = null;
            ValueChanged = null;
            base.Dispose(disposing);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
                return true;
            else
                return base.IsInputKey(keyData);
        }


        private void SetFormat(DateTimePickerFormat f)
        {
            format = f;
            if (format == DateTimePickerFormat.Long)
                customformat = CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern.Trim();
            else if (format == DateTimePickerFormat.Short)
                customformat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.Trim();
            else if (format == DateTimePickerFormat.Time)
                customformat = CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.Trim();

            Recalc();
        }

        private void Recalc()
        {
            //System.Diagnostics.Debug.WriteLine(Name + " Format " + customformat);
            partlist.Clear();

            int xpos = 0;

            using (Graphics e = CreateGraphics())
            {
                string fmt = customformat;

                while (fmt.Length>0)
                {
                    Parts p = null;

                    if ( fmt[0] == '\'')
                    {
                        int index = fmt.IndexOf('\'', 1);
                        if (index == -1)
                            index = fmt.Length;

                        p = new Parts() { maxstring = fmt.Substring(1, index - 1), ptype = PartsTypes.Text };
                        fmt = (index < fmt.Length) ? fmt.Substring(index + 1) : "";
                    }
                    else if (fmt.StartsWith("dddd"))
                        p = Make(ref fmt, 4, PartsTypes.DayName, Max(CultureInfo.CurrentCulture.DateTimeFormat.DayNames));
                    else if (fmt.StartsWith("ddd"))
                        p = Make(ref fmt, 3, PartsTypes.DayName, Max(CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames));
                    else if (fmt.StartsWith("dd"))
                        p = Make(ref fmt, 2, PartsTypes.Day, "99");
                    else if (fmt.StartsWith("d"))
                        p = Make(ref fmt, 1, PartsTypes.Day, "99");
                    else if (fmt.StartsWith("MMMM"))
                        p = Make(ref fmt, 4, PartsTypes.Month, Max(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames));
                    else if (fmt.StartsWith("MMM"))
                        p = Make(ref fmt, 3, PartsTypes.Month, Max(CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames));
                    else if (fmt.StartsWith("MM"))
                        p = Make(ref fmt, 2, PartsTypes.Month, "99");
                    else if (fmt.StartsWith("M"))
                        p = Make(ref fmt, 1, PartsTypes.Month, "99");
                    else if (fmt.StartsWith("HH", StringComparison.InvariantCultureIgnoreCase))
                        p = Make(ref fmt, 2, PartsTypes.Hours, "99");
                    else if (fmt.StartsWith("H", StringComparison.InvariantCultureIgnoreCase))
                        p = Make(ref fmt, 1, PartsTypes.Hours, "99");
                    else if (fmt.StartsWith("mm"))
                        p = Make(ref fmt, 2, PartsTypes.Mins, "99");
                    else if (fmt.StartsWith("m"))
                        p = Make(ref fmt, 1, PartsTypes.Mins, "99");
                    else if (fmt.StartsWith("ss"))
                        p = Make(ref fmt, 2, PartsTypes.Seconds, "99");
                    else if (fmt.StartsWith("s"))
                        p = Make(ref fmt, 1, PartsTypes.Seconds, "99");
                    else if (fmt.StartsWith("tt"))
                        p = Make(ref fmt, 2, PartsTypes.AmPm, "AM");
                    else if (fmt.StartsWith("t"))
                        p = Make(ref fmt, 1, PartsTypes.AmPm, "AM");
                    else if (fmt.StartsWith("yyyyy"))
                        p = Make(ref fmt, 5, PartsTypes.Year, "99999");
                    else if (fmt.StartsWith("yyyy"))
                        p = Make(ref fmt, 4, PartsTypes.Year, "9999");
                    else if (fmt.StartsWith("yyy"))
                        p = Make(ref fmt, 3, PartsTypes.Year, "9999");
                    else if (fmt.StartsWith("yy"))
                        p = Make(ref fmt, 2, PartsTypes.Year, "99");
                    else if (fmt.StartsWith("y"))
                        p = Make(ref fmt, 1, PartsTypes.Year, "99");
                    else if ( fmt[0] != ' ')
                    {
                        p = new Parts() { maxstring = fmt.Substring(0,1), ptype = PartsTypes.Text }; 
                        fmt = fmt.Substring(1).Trim();
                    }
                    else
                        fmt = fmt.Substring(1).Trim();

                    if (p != null)
                    {
                        p.xpos = xpos;
                        SizeF sz = e.MeasureString(p.maxstring, Font);
                        int width = (int)(sz.Width + 1);
                        p.endx = xpos + width;
                        xpos = p.endx + 1;
                        partlist.Add(p);
                    }
                }
            }
        }

        private Parts Make(ref string c, int len, PartsTypes t, string maxs)
        {
            Parts p = new Parts() { format = c.Substring(0, len) + " ", ptype = t, maxstring = maxs }; // space at end seems to make multi ones work
            c = c.Substring(len);
            return p;
        }

        private string Max(string[] a)
        {
            string m = "";
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].Length > m.Length)
                    m = a[i];
            }
            return m;
        }

        private GraphicsPath RectCutCorners(int x, int y, int width, int height, int roundnessleft, int roundnessright)
        {
            GraphicsPath gr = new GraphicsPath();

            gr.AddLine(x + roundnessleft, y, x + width - 1 - roundnessright, y);
            gr.AddLine(x + width - 1, y + roundnessright, x + width - 1, y + height - 1 - roundnessright);
            gr.AddLine(x + width - 1 - roundnessright, y + height - 1, x + roundnessleft, y + height - 1);
            gr.AddLine(x, y + height - 1 - roundnessleft, x, y + roundnessleft);
            gr.AddLine(x, y + roundnessleft, x + roundnessleft, y);         // close figure manually, closing it with a break does not seem to work
            return gr;
        }

        private bool TryNum(string s)
        {
            int newvalue;
            int.TryParse(s, out newvalue);
            DateTime nv = DateTime.Now;

            Parts p = partlist[selectedpart];

            try
            {
                if (p.ptype == PartsTypes.Day)
                    nv = new DateTime(datetimevalue.Year, datetimevalue.Month, newvalue, datetimevalue.Hour, datetimevalue.Minute, datetimevalue.Second);
                else if (p.ptype == PartsTypes.Month)
                    nv = new DateTime(datetimevalue.Year, newvalue, datetimevalue.Day, datetimevalue.Hour, datetimevalue.Minute, datetimevalue.Second);
                else if (p.ptype == PartsTypes.Year)
                    nv = new DateTime(newvalue, datetimevalue.Month, datetimevalue.Day, datetimevalue.Hour, datetimevalue.Minute, datetimevalue.Second);
                else if (p.ptype == PartsTypes.Hours)
                    nv = new DateTime(datetimevalue.Year, datetimevalue.Month, datetimevalue.Day, newvalue, datetimevalue.Minute, datetimevalue.Second);
                else if (p.ptype == PartsTypes.Mins)
                    nv = new DateTime(datetimevalue.Year, datetimevalue.Month, datetimevalue.Day, datetimevalue.Hour, newvalue, datetimevalue.Second);
                else if (p.ptype == PartsTypes.Seconds)
                    nv = new DateTime(datetimevalue.Year, datetimevalue.Month, datetimevalue.Day, datetimevalue.Hour, datetimevalue.Minute, newvalue);

                datetimevalue = nv;
                OnValueChanged(EventArgs.Empty);
                Invalidate();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion // Methods

        #region Event handlers

        private void Calendaricon_MouseClick(object sender, MouseEventArgs e)
        {
            Point screencoord = PointToScreen(new Point(0, Height));     // screen co-ord of our control at the bottom left
            Form f = FindForm();
            Point formpoint = f.PointToClient(screencoord);             // now in form client terms
            calendar.Location = formpoint;
            //calendar.Size = new Size(ClientRectangle.Width, ClientRectangle.Width );
            calendar.MaxSelectionCount = 1;
            calendar.Focus();
            calendar.SetDate(datetimevalue);
            calendar.Show();
            f.Controls.Add(calendar);
            FindForm().Controls.SetChildIndex(calendar, 0);
            selectedpart = -1;
            Invalidate();
        }

        private void Calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            datetimevalue = new DateTime(e.Start.Year, e.Start.Month, e.Start.Day, datetimevalue.Hour, datetimevalue.Minute, datetimevalue.Second);
            //System.Diagnostics.Debug.WriteLine("Set to " + Value.ToLongTimeString() + " " + Value.ToLongDateString());
            calendar.Hide();
            FindForm().Controls.Remove(calendar);
            OnValueChanged(EventArgs.Empty);
            Invalidate();
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            OnValueChanged(EventArgs.Empty);
        }

        private void Updown_Selected(object sender, MouseEventArgs me)   // up down triggered, delta tells you which way
        {
            if (me.Delta > 0)
                UpDown(1);
            else if (me.Delta < 0)
                UpDown(-1);
        }

        #endregion // Event handlers

        #endregion // Implementation
    }
}
