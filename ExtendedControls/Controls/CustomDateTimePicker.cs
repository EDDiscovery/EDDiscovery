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
        // its up to you how to use this, what tz info it has.  tries to keep the kind
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

        public event EventHandler ValueChanged
        {
            add { Events.AddHandler(EVENT_VALUECHANGED, value); }
            remove { Events.RemoveHandler(EVENT_VALUECHANGED, value); }
        }

        private static readonly object EVENT_VALUECHANGED = new object();

        private DateTime datetimevalue = DateTime.Now;
        private DateTimePickerFormat format = DateTimePickerFormat.Long;
        private string customformat = CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern;
        private bool showupdown = false;
        private bool showcheckbox = false;
        private Color textbackcolor = Color.DarkBlue;
        private Color bordercolor = Color.Transparent;

        public UpDown updown = new UpDown();                            // for setting colours
        public CheckBoxCustom checkbox = new CheckBoxCustom();          // for setting colours, note background is forces to TextBackColour

        private MonthCalendar calendar = new MonthCalendar();           // calendar, when rendered by window, is colour locked
        private PictureBox calendaricon = new PictureBox();             // does not need colour.. icon

        private int xstart = 0;                             // where the text starts

        enum PartsTypes { Text, DayName, Day, Month, Year, Hours, Mins, Seconds, AmPm }
        class Parts
        {
            public PartsTypes ptype;
            public string maxstring;
            public string format;
            public int xpos;
            public int endx;
        };

        List<Parts> partlist = new List<Parts>();
        int selectedpart = -1;

        public CustomDateTimePicker()
        {
            SetStyle(ControlStyles.Selectable, true);
            Recalc();
            Controls.Add(updown);
            Controls.Add(calendaricon);
            Controls.Add(checkbox);

            updown.Selected += OnUpDown;
            calendaricon.MouseClick += Calendaricon_MouseClick;
            calendar.DateSelected += Calendar_DateSelected;
            calendar.Visible = false;
            checkbox.CheckedChanged += Checkbox_CheckedChanged;
        }

        void SetFormat(DateTimePickerFormat f)
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

        void Recalc()    
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
                        SizeF sz = e.MeasureString(p.maxstring, this.Font);
                        int width = (int)(sz.Width + 1);
                        p.endx = xpos + width;
                        xpos = p.endx + 1;
                        partlist.Add(p);
                    }
                }
            }
        }

        Parts Make(ref string c, int len, PartsTypes t, string maxs)
        {
            Parts p = new Parts() { format = c.Substring(0, len) + " ", ptype = t, maxstring = maxs }; // space at end seems to make multi ones work
            c = c.Substring(len);
            return p;
        }

        string Max(string[] a)
        {
            string m = "";
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].Length > m.Length)
                    m = a[i];
            }
            return m;
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

                calendaricon.Image = ExtendedControls.Properties.Resources.Calendar;
                //calendaricon.Image = DialogTest.Properties.Resources.Calendar;
                calendaricon.Location = new Point(ClientRectangle.Width - offset - calendaricon.Image.Width - 4, ClientRectangle.Height / 2 - calendaricon.Image.Height / 2);

                calendaricon.Size = calendaricon.Image.Size;
                calendaricon.Visible = !ShowUpDown;
                calendaricon.SizeMode = PictureBoxSizeMode.CenterImage;

                xstart = (showcheckbox ? (checkbox.Right + 2) : 2) + (BorderColor.IsFullyTransparent() ? 2 : 0);

                Recalc();   // cause anything might have changed, like fonts
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

                using (GraphicsPath g1 = ControlHelpersStaticFunc.RectCutCorners(1, 1, ClientRectangle.Width - 2, ClientRectangle.Height - 1, 1, 1))
                using (Pen pc1 = new Pen(color1, 1.0F))
                    e.Graphics.DrawPath(pc1, g1);

                using (GraphicsPath g2 = ControlHelpersStaticFunc.RectCutCorners(0, 0, ClientRectangle.Width, ClientRectangle.Height - 1, 2, 2))
                using (Pen pc2 = new Pen(color2, 1.0F))
                    e.Graphics.DrawPath(pc2, g2);

                drawarea.Inflate(-2, -2);
            }

            using (Brush br = new SolidBrush(this.TextBackColor))
                e.Graphics.FillRectangle(br, drawarea);

            using (Brush textb = new SolidBrush(this.ForeColor))
            {
                for (int i = 0; i < partlist.Count; i++)
                {
                    Parts p = partlist[i];

                    string t = (p.ptype == PartsTypes.Text) ? p.maxstring : datetimevalue.ToString(p.format);

                    if (i == selectedpart)
                    {
                        using (Brush br = new SolidBrush(this.SelectedColor))
                            e.Graphics.FillRectangle(br, new Rectangle(p.xpos + xstart, drawarea.Y, p.endx - p.xpos, drawarea.Height));
                    }

                    e.Graphics.DrawString(t, this.Font, textb, new Point(p.xpos + xstart, drawarea.Y+2));
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            //System.Diagnostics.Debug.WriteLine("Mouse down");

            for (int i = 0; i < partlist.Count; i++)
            {
                if (partlist[i].ptype >= PartsTypes.Day && mevent.X >= partlist[i].xpos+xstart && mevent.X <= partlist[i].endx+xstart)
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

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right )
                return true;
            else
                return base.IsInputKey(keyData);
        }

        string keybuffer;

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
                int findprev = selectedpart-1; // back 1
                while (findprev >= 0 && partlist[findprev].ptype < PartsTypes.Day)       // back until valid
                    findprev--;

                if (findprev >= 0)
                {
                    selectedpart = findprev;
                    Invalidate();
                }
            }
            else if (e.KeyCode == Keys.Right && selectedpart < partlist.Count - 1 )
            {
                int findnext = selectedpart + 1; // fwd 1
                while (findnext < partlist.Count && partlist[findnext].ptype < PartsTypes.Day)       // fwd until valid
                    findnext++;

                if (findnext < partlist.Count )
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

        private bool TryNum(string s)
        {
            int newvalue;
            int.TryParse(s, out newvalue);
            DateTime nv = DateTime.Now;

            Parts p = partlist[selectedpart];

            try
            {
                if (p.ptype == PartsTypes.Day)
                    nv = new DateTime(datetimevalue.Year, datetimevalue.Month, newvalue, datetimevalue.Hour, datetimevalue.Minute, datetimevalue.Second, datetimevalue.Kind);
                else if (p.ptype == PartsTypes.Month)
                    nv = new DateTime(datetimevalue.Year, newvalue, datetimevalue.Day, datetimevalue.Hour, datetimevalue.Minute, datetimevalue.Second, datetimevalue.Kind);
                else if (p.ptype == PartsTypes.Year)
                    nv = new DateTime(newvalue, datetimevalue.Month, datetimevalue.Day, datetimevalue.Hour, datetimevalue.Minute, datetimevalue.Second, datetimevalue.Kind);
                else if (p.ptype == PartsTypes.Hours)
                    nv = new DateTime(datetimevalue.Year, datetimevalue.Month, datetimevalue.Day, newvalue, datetimevalue.Minute, datetimevalue.Second, datetimevalue.Kind);
                else if (p.ptype == PartsTypes.Mins)
                    nv = new DateTime(datetimevalue.Year, datetimevalue.Month, datetimevalue.Day, datetimevalue.Hour, newvalue, datetimevalue.Second, datetimevalue.Kind);
                else if (p.ptype == PartsTypes.Seconds)
                    nv = new DateTime(datetimevalue.Year, datetimevalue.Month, datetimevalue.Day, datetimevalue.Hour, datetimevalue.Minute, newvalue, datetimevalue.Kind);

                datetimevalue = nv;

                EventHandler handler = (EventHandler)Events[EVENT_VALUECHANGED];
                if (handler != null) handler(this, new EventArgs());

                Invalidate();
                return true;
            }
            catch
            {
                return false;
            }
        }


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

                EventHandler handler = (EventHandler)Events[EVENT_VALUECHANGED];
                if (handler != null) handler(this, new EventArgs());

                Invalidate();
            }
        }

        protected void OnUpDown(object sender, MouseEventArgs me)   // up down triggered, delta tells you which way
        {
            if (me.Delta > 0)
                UpDown(1);
            else
                UpDown(-1);
        }

        private void Calendaricon_MouseClick(object sender, MouseEventArgs e)
        {
            if (calendar.Visible == false)
            {
                Point screencoord = PointToScreen(new Point(0, Height));     // screen co-ord of our control at the bottom left
                Form f = FindForm();
                Point formpoint = f.PointToClient(screencoord);             // now in form client terms
                calendar.Location = formpoint.PositionWithinRectangle(calendar.Size, f.ClientRectangle);
                //calendar.Size = new Size(ClientRectangle.Width, ClientRectangle.Width );
                calendar.MaxSelectionCount = 1;
                calendar.Focus();
                calendar.SetDate(datetimevalue);
                calendar.Show();
                f.Controls.Add(calendar);
                this.FindForm().Controls.SetChildIndex(calendar, 0);
                selectedpart = -1;
                Invalidate();
            }
        }

        private void Calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            datetimevalue = new DateTime(e.Start.Year, e.Start.Month, e.Start.Day, datetimevalue.Hour, datetimevalue.Minute, datetimevalue.Second, datetimevalue.Kind);
            //System.Diagnostics.Debug.WriteLine("Set to " + Value.ToLongTimeString() + " " + Value.ToLongDateString());
            calendar.Visible = false;
            calendar.Hide();
            this.FindForm().Controls.Remove(calendar);
            Invalidate();

            EventHandler handler = (EventHandler)Events[EVENT_VALUECHANGED];
            if (handler != null) handler(this, new EventArgs());
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (selectedpart != -1)
            {
                selectedpart = -1;
                Invalidate();
            }
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[EVENT_VALUECHANGED];
            if (handler != null) handler(this, new EventArgs());
        }

        // NOT IN USE
        #region Icon Replacement
        public void ReplaceIconsNOTINUSE(Func<string, Image> getIcon)
        {
            calendaricon.Image = getIcon("CalendarIcon");
            int offset = BorderColor.IsFullyTransparent() ? 0 : 2;
            calendaricon.Location = new Point(ClientRectangle.Width - offset - calendaricon.Image.Width - 4, ClientRectangle.Height / 2 - calendaricon.Image.Height / 2);
            calendaricon.Size = calendaricon.Image.Size;
        }
        #endregion
    }
}
