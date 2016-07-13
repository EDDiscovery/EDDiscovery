using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ExtendedControls
{
    public class ComboBoxCustom : Control
    {
        // ForeColor = text, BackColor = control background
        public Color MouseOverBackgroundColor { get; set; } = Color.Silver;
        public Color BorderColor { get; set; } = Color.White;
        public Color DropDownBackgroundColor { get; set; } = Color.Gray;
        public Color ScrollBarColor { get; set; } = Color.LightGray;
        public Color ScrollBarButtonColor { get; set; } = Color.LightGray;

        public FlatStyle FlatStyle { get; set; } = FlatStyle.System;
        public int DropDownHeight { get; set; } = 200;
        public int ArrowWidth { get; set; } = 1;
        public float ButtonColorScaling { get; set; } = 0.5F;           // Popup style only
        public List<string> Items { get; set; }
        public int ScrollBarWidth { get; set; } = 16;
        public int ItemHeight { get; set; } = 20;

        public void Repaint() { firstpaint = true; Invalidate(); }     // MUST call after setting any of the above after first paint.

        public int SelectedIndex { get { return selected; } set { UpdateSelected(value); } }

        // attach a IList datasource with a property called DisplayMember
        public object DataSource { get { return datasourceobject; } set { SetDS(value, datasourcedisplaymember); } }
        public string DisplayMember { get { return datasourcedisplaymember; } set { SetDS(datasourceobject, value); } }

        // if Datasource, you get the object.  Else, you get/set it by Items string value.
        public object SelectedItem { get { return GetSelectedObject(); } set { SetSelectedObject(value); } }

        // only required if SelectedValue required.
        public string ValueMember { get { return datasourcevaluemember; } set { datasourcevaluemember = value; } }
        public object SelectedValue { get { return GetSelectedValue(); } }      // only supporting get.

        // events
        public delegate void OnSelectedIndexChanged(object sender, EventArgs e);
        public event OnSelectedIndexChanged SelectedIndexChanged;

        public ComboBoxCustom() : base()
        {
            Items = new List<string>();
            this.Text = "";
            // no need - keep for reference for now. SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (firstpaint)
            {
                dosystem = FlatStyle == FlatStyle.System && ComboBoxRenderer.IsSupported;

                int extraborder = (FlatStyle != FlatStyle.System) ? 1 : 0;

                topBoxTextArea = new Rectangle(ClientRectangle.X + extraborder, ClientRectangle.Y + extraborder,
                                     ClientRectangle.Width - 2 * extraborder - ScrollBarWidth, ClientRectangle.Height - 2 * extraborder);

                arrowRectangleArea = new Rectangle(ClientRectangle.Width - ScrollBarWidth - extraborder, ClientRectangle.Y + extraborder,
                                                   ScrollBarWidth, ClientRectangle.Height - 2 * extraborder);

                topBoxOutline = new Rectangle(ClientRectangle.X, ClientRectangle.Y,
                                                ClientRectangle.Width - 1, ClientRectangle.Height - 1);

                topBoxTextTotalArea = new Rectangle(ClientRectangle.X + extraborder, ClientRectangle.Y + extraborder,
                                                         ClientRectangle.Width - 2 * extraborder, ClientRectangle.Height - 2 * extraborder);

                int hoffset = arrowRectangleArea.Width / 3;
                int voffset = arrowRectangleArea.Height / 3;
                arrowpt1 = new Point(arrowRectangleArea.X + hoffset, arrowRectangleArea.Y + voffset);
                arrowpt2 = new Point(arrowRectangleArea.X + arrowRectangleArea.Width / 2, arrowRectangleArea.Y + arrowRectangleArea.Height - voffset);
                arrowpt3 = new Point(arrowRectangleArea.X + arrowRectangleArea.Width - hoffset, arrowpt1.Y);

                arrowpt1c = new Point(arrowpt1.X, arrowpt2.Y);
                arrowpt2c = new Point(arrowpt2.X, arrowpt1.Y);
                arrowpt3c = new Point(arrowpt3.X, arrowpt2.Y);

                if (!dosystem)
                {
                    SizeF sz = e.Graphics.MeasureString("Represent THIS", this.Font);
                    topBoxTextArea.Y += (topBoxTextArea.Height - (int)sz.Height) / 2;
                }

                firstpaint = false;
            }

            Brush textb;
            Pen p, p2;

            if (Enabled)
            {
                textb = new SolidBrush(this.ForeColor);
                p = new Pen(this.BorderColor);
                p2 = new Pen(this.ForeColor);
                p2.Width = ArrowWidth;
            }
            else
            {
                textb = new SolidBrush(Multiply(this.ForeColor, 0.5F));
                p = new Pen(Multiply(this.BorderColor, 0.5F));
                p2 = null;
            }


            if (dosystem)
            {
                ComboBoxRenderer.DrawTextBox(e.Graphics, topBoxTextArea,
                    this.Text, this.Font, (isActivated) ? ComboBoxState.Pressed : (Enabled) ? ComboBoxState.Normal : ComboBoxState.Disabled);
                ComboBoxRenderer.DrawDropDownButton(e.Graphics, arrowRectangleArea, (Enabled) ? arrowState : ComboBoxState.Disabled);
            }
            else
            {
                e.Graphics.DrawRectangle(p, topBoxOutline);

                Color bck;

                if (Enabled)
                    bck = (mouseover) ? MouseOverBackgroundColor : BackColor;
                else
                    bck = Multiply(BackColor, 0.5F);

                Brush bbck;

                if (FlatStyle == FlatStyle.Popup)
                {
                    bbck = new System.Drawing.Drawing2D.LinearGradientBrush(topBoxTextTotalArea, bck, Multiply(bck, ButtonColorScaling), 90);
                }
                else
                {
                    bbck = new SolidBrush(bck);
                }

                e.Graphics.FillRectangle(bbck, topBoxTextTotalArea);

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                if (p2 != null)
                {
                    if (isActivated)
                    {
                        e.Graphics.DrawLine(p2, arrowpt1c, arrowpt2c);            // the arrow!
                        e.Graphics.DrawLine(p2, arrowpt2c, arrowpt3c);
                    }
                    else
                    {
                        e.Graphics.DrawLine(p2, arrowpt1, arrowpt2);            // the arrow!
                        e.Graphics.DrawLine(p2, arrowpt2, arrowpt3);
                    }
                }

                e.Graphics.DrawString(this.Text, this.Font, textb, topBoxTextArea);

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                bbck.Dispose();
            }

            textb.Dispose();
            p.Dispose();

            if (p2 != null)
                p2.Dispose();
        }

        private void Activate()
        {
            if (!isActivated && Items != null && Items.Count > 0)          // if not activated, and no point if no items
            {
                Point ourposonparent;
                Control parentform = FindParentForm(out ourposonparent);        // Can't add it to parent directly, it might be a group box.
                clc = new ListControlCustom();
                clc.Location = new Point(ourposonparent.X, ourposonparent.Y + ClientRectangle.Height);

                int fittableitems = DropDownHeight / ItemHeight;
                if (fittableitems > Items.Count())                             // no point doing more than we have..
                    fittableitems = Items.Count();

                clc.Size = new Size(ClientRectangle.Width, fittableitems * ItemHeight + ((FlatStyle!=FlatStyle.System) ? 4 : 0)); 
                clc.SelectionBackColor = DropDownBackgroundColor;
                clc.ForeColor = ForeColor;
                clc.BorderColor = BorderColor;
                clc.Items = Items;
                clc.ItemHeight = ItemHeight;
                clc.SelectedIndex = selected;
                clc.FlatStyle = FlatStyle;
                clc.SelectedIndexChanged += UserSelectedIndex;
                clc.Leave += UserLeftList;
                clc.Font = Font;
                clc.ScrollBarColor = ScrollBarColor;
                clc.ScrollBarButtonColor = ScrollBarButtonColor;
                parentform.Controls.Add(clc);
                clc.Show();
                clc.BringToFront();
                clc.Focus();                    // so it can get events such as mouse and keyboard..
                isActivated = true;
                Invalidate();
            }
        }

        private void DeActivate()
        {
            if (isActivated)
            {
                isActivated = false;
                clc.Hide();
                clc.Dispose();
                Invalidate();
                Focus();
            }
        }

        protected void UserSelectedIndex(object sender, EventArgs e)
        {
            int sel = clc.SelectedIndex;
            UpdateSelected(sel);
        }

        protected void UserLeftList(object sender, EventArgs e)
        {
            DeActivate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (topBoxOutline.Contains(e.Location))        // clicked anywhere in box
            {
                if (arrowRectangleArea.Contains(e.Location) && FlatStyle == FlatStyle.System)
                {
                    arrowState = ComboBoxState.Pressed;
                    Invalidate();
                }

                if (!isActivated)
                    Activate();
                else
                    DeActivate();
            }
        }


        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (arrowRectangleArea.Contains(e.Location) && FlatStyle == FlatStyle.System)
            {
                arrowState = ComboBoxState.Normal;
                Invalidate();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (!isActivated)
            {
                if (!mouseover)
                {
                    mouseover = true;
                    Invalidate();
                }
            }
            else if (mouseover)                                         // else if mouse on, needs to go off
            {
                mouseover = false;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (mouseover)
            {
                mouseover = false;
                Invalidate();
            }
        }

        private void UpdateSelected(int v)
        {
            if (v == selected || Items == null)        // either means do nothing
            {
            }
            else if (v >= 0 && v < Items.Count)
            {
                selected = v;
                this.Text = Items[v];
                if (SelectedIndexChanged != null)
                    SelectedIndexChanged(this, new EventArgs());
            }
            else if (v != -1)
            {
                selected = -1;
                this.Text = "";
                if (SelectedIndexChanged != null)
                    SelectedIndexChanged(this, new EventArgs());
            }

            DeActivate();
            Invalidate();       // may not be activated so need to invalidate
        }

        private void UpdateSelected(string v)           // not used as of now - keep
        {
            if (Items != null)
            {
                int offset = 0;

                foreach (string s in Items)
                {
                    if (v != null && s.Equals(v))
                    {
                        UpdateSelected(offset);
                        return;
                    }

                    offset++;
                }

                UpdateSelected(-1);
            }
        }

        Control FindParentForm(out Point ourposonparent)            // Find the form, in case in a groupbox..
        {
            ourposonparent = Location;
            Control p = Parent;

            while (!(p is Form))
            {
                ourposonparent.X += p.Location.X;
                ourposonparent.Y += p.Location.Y;
                p = p.Parent;
            }

            return p;
        }

        protected override void OnResize(EventArgs e)               // just in case a resize occurs
        {
            base.OnResize(e);
            firstpaint = true;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down)        // grab these nav keys
                return true;
            else
                return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Down)
            {
                if (Items != null && SelectedIndex < Items.Count - 1)
                    UpdateSelected(SelectedIndex + 1);
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (SelectedIndex > 0)
                    UpdateSelected(SelectedIndex - 1);

            }
        }

        private void SetDS(object o, string vmember)            // data source or display member is changing..
        {
            bool clearit = false;

            if (o != datasourceobject)                        // if we changed, remember it
            {
                datasourceobject = o;
                clearit = true;
            }

            if (datasourcedisplaymember == null || !datasourcedisplaymember.Equals(vmember))
            {
                datasourcedisplaymember = vmember;
                clearit = true;
            }

            if (clearit)                                            // if we changed, start from scratch
            {
                DeActivate();                                       // just in case..
                Items.Clear();
                Text = "";
                selected = -1;
            }
            // if we changed, and we have members
            if (clearit && datasourcedisplaymember != null && datasourceobject != null)
            {
                IList objl = datasourceobject as IList;             // its an IList, so iterate

                foreach (object oi in objl)
                {
                    Type ti = oi.GetType();
                    PropertyInfo pi = ti.GetProperty(datasourcedisplaymember);

                    if (pi != null)                                 // properties only
                    {
                        if (pi.PropertyType.Name.Equals("String"))  // string properties (for now) only
                        {
                            string s = (string)(pi.GetValue(oi, null));
                            Items.Add(s);                           // add it to items..
                        }
                        else
                            throw new ArgumentException("DisplayMember only string properties supported");
                    }
                    else
                        throw new ArgumentException("DisplayMember supports only properties");
                }
            }

            Repaint();
        }

        private object GetSelectedObject()                                  // if datasource attached, return selected object
        {                                                                   // else return string
            if (datasourceobject != null)
            {
                if (selected >= 0)
                {
                    IList objl = datasourceobject as IList;
                    return objl[selected];
                }
                else
                    return null;
            }
            else if (Items != null && selected >= 0)
                return Items[selected];
            else
                return null;
        }

        private void SetSelectedObject(object value)                        // if datasource attached, set index by object
        {                                                                   // if no datasource, give a string..
            if (datasourceobject != null)
            {
                IList objl = datasourceobject as IList;

                int index = 0;

                foreach (object oi in objl)
                {
                    if (oi == value)            // if object match..
                    {
                        UpdateSelected(index);
                        break;
                    }

                    index++;
                }
            }
            else if (value is String)
            {
                UpdateSelected((string)value);
            }
        }

        private object GetSelectedValue()                                  // if datasource attached, return selected object
        {
            if (datasourceobject != null && selected >= 0 && datasourcevaluemember != null)
            {
                IList objl = datasourceobject as IList;
                object oi = objl[selected];

                Type ti = oi.GetType();
                PropertyInfo pi = ti.GetProperty(datasourcevaluemember);

                if (pi != null)
                    return pi.GetValue(oi, null);         // return value..
            }

            return null;
        }


        // internals
        private int selected = -1;
        private bool isActivated = false;
        private bool mouseover = false;
        private bool firstpaint = true;
        private bool dosystem = false;
        private ComboBoxState arrowState = ComboBoxState.Normal;
        private Rectangle topBoxTextArea, arrowRectangleArea, topBoxOutline, topBoxTextTotalArea;
        private Point arrowpt1, arrowpt2, arrowpt3;
        private Point arrowpt1c, arrowpt2c, arrowpt3c;
        private ListControlCustom clc;
        private object datasourceobject = null;
        private string datasourcedisplaymember = null;
        private string datasourcevaluemember = null;

        private byte limit(float a) { if (a > 255F) return 255; else return (byte)a; }
        public Color Multiply(Color from, float m) { return Color.FromArgb(from.A, limit((float)from.R * m), limit((float)from.G * m), limit((float)from.B * m)); }

    }
}
