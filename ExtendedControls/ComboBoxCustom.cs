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
        #region Public interfaces

        #region ctors

        public ComboBoxCustom()
        {
            //Text = "";
            _cbsystem = new ComboBox();
            _cbsystem.Dock = DockStyle.Fill;
            _cbsystem.SelectedIndexChanged += _cbsystem_SelectedIndexChanged;
            _cbsystem.DropDownStyle = ComboBoxStyle.DropDownList;
            _items = new ObjectCollection(_cbsystem);
            Controls.Add(_cbsystem);
        }

        #endregion // ctors

        #region public class ObjectCollection : ICollection<string>, IEnumerable, IEnumerable<string>, IList<string>

        public class ObjectCollection : ICollection<string>, IEnumerable, IEnumerable<string>, IList<string>
        {
            #region Public interfaces

            #region ctors

            public ObjectCollection(ComboBox cb)
            {
                _combobox = cb;
            }

            public ObjectCollection(IList<string> vals)
            {
                _explicitCollection = vals as IList;
            }

            #endregion // ctors

            #region Implicit operators

            public static implicit operator ObjectCollection(List<string> vals)
            {
                return new ObjectCollection(vals);
            }

            #endregion // Implicit operators

            #region Methods

            public void AddRange(IEnumerable<string> items)
            {
                foreach (var val in items)
                {
                    _list.Add(val);
                }
            }

            #endregion // Methods

            #region System.Collections.Generic.ICollection<string> interface

            public int Count { get { return _list.Count; } }

            public bool IsReadOnly { get { return _list.IsReadOnly; } }


            public void Add(string item)
            {
                _list.Add(item);
            }

            public void Clear()
            {
                _list.Clear();
            }

            public bool Contains(string item)
            {
                return _list.Contains(item);
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                _list.OfType<object>().Select(v => (v ?? string.Empty).ToString()).ToList().CopyTo(array, arrayIndex);
            }

            public bool Remove(string item)
            {
                if (_list.Contains(item))
                {
                    _list.Remove(item);
                    return true;
                }
                return false;
            }

            #endregion // System.Collections.Generic.ICollection<string> interface

            #region System.Collections.IEnumerable interface

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion System.Collections.IEnumerable interface

            #region System.Collections.Generic.IEnumerable<string> interface

            public IEnumerator<string> GetEnumerator()
            {
                return _list.OfType<object>().Select(v => (v ?? string.Empty).ToString()).GetEnumerator();
            }

            #endregion // System.Collections.Generic.IEnumerable<string> interface

            #region System.Collections.Generic.IList<string> interface

            public string this[int index] { get { return (_list[index] ?? string.Empty).ToString(); } set { _list[index] = value; } }

            public int IndexOf(string item)
            {
                return _list.IndexOf(item);
            }

            public void Insert(int index, string item)
            {
                _list.Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                _list.RemoveAt(index);
            }

            #endregion // System.Collections.Generic.IList<string> interface

            #endregion // Public interfaces


            #region Implementation

            #region Fields and properties

            private IList _explicitCollection;
            private ComboBox _combobox;

            private IList _list
            {
                get
                {
                    if (_explicitCollection != null)
                    {
                        return _explicitCollection;
                    }
                    else if (_combobox.DisplayMember != null && _combobox.DataSource != null && _combobox.DataSource is ICollection)
                    {
                        return GetMembers((ICollection)_combobox.DataSource, _combobox.DisplayMember).ToList();
                    }
                    else
                    {
                        return _combobox.Items;
                    }
                }
            }

            #endregion // Fields and properties

            #region Methods

            protected IEnumerable<object> GetMembers(ICollection vals, string displaymember)
            {
                foreach (object val in vals)
                {
                    Type t = val.GetType();
                    PropertyInfo pi = t.GetProperty(displaymember);
                    object dm = pi.GetValue(val, new object[0]);
                    yield return dm;
                }
            }

            #endregion // Methods

            #endregion // Implementation
        }

        #endregion // public class ObjectCollection : ICollection<string>, IEnumerable, IEnumerable<string>, IList<string>

        #region Properties

        // ForeColor = text, BackColor = control background
        public Color MouseOverBackgroundColor { get; set; } = Color.Silver;
        public Color BorderColor { get; set; } = Color.White;
        public Color DropDownBackgroundColor { get; set; } = Color.Gray;
        public Color ScrollBarColor { get; set; } = Color.LightGray;
        public Color ScrollBarButtonColor { get; set; } = Color.LightGray;

        public int ArrowWidth { get; set; } = 1;
        public float ButtonColorScaling { get; set; } = 0.5F;           // Popup style only
        public int ScrollBarWidth { get; set; } = 16;
        public FlatStyle FlatStyle { get; set; } = FlatStyle.System;

        //Set width to 1 to scale it to the same width as the Button..
        public int DropDownWidth { get { return _cbsystem.DropDownWidth; } set { _cbsystem.DropDownWidth = value; } }
        public int DropDownHeight { get { return _cbsystem.DropDownHeight; } set { _cbsystem.DropDownHeight = value; } }
        public int ItemHeight { get { return _cbsystem.ItemHeight; } set { _cbsystem.ItemHeight = value; } }

        public int SelectedIndex { get { return _cbsystem.SelectedIndex; } set { _cbsystem.SelectedIndex = value; } }

        public ObjectCollection Items { get { return _items; } set { _items.Clear(); _items.AddRange(value.ToArray()); } }

        public override AnchorStyles Anchor { get { return base.Anchor; } set { base.Anchor = value; _cbsystem.Anchor = value; } }
        public override DockStyle Dock { get { return base.Dock; } set { base.Dock = value; _cbsystem.Dock = value; } }
        public override Font Font { get { return base.Font; } set { base.Font = value; _cbsystem.Font = value; } }
        public override string Text { get { return base.Text; } set { base.Text = value; _cbsystem.Text = value; } }

        // BEWARE SET value/display before DATA SOURCE
        public object DataSource { get { return _cbsystem.DataSource; } set { _cbsystem.DataSource = value; } }
        public string ValueMember { get { return _cbsystem.ValueMember; } set { _cbsystem.ValueMember = value; } }
        public string DisplayMember { get { return _cbsystem.DisplayMember; } set { _cbsystem.DisplayMember = value; } }
        public object SelectedItem { get { return _cbsystem.SelectedItem; } set { _cbsystem.SelectedItem = value; } }
        public object SelectedValue { get { return _cbsystem.SelectedValue; } set { _cbsystem.SelectedValue = value; } }

        public ComboBox GetInternalSystemControl { get { return _cbsystem; } }

        #endregion // Properties

        #region Events

        public event EventHandler SelectedIndexChanged;     // ✓

        #endregion // Events

        #region Methods

        public void Repaint()
        {
            if (FlatStyle == FlatStyle.System)
            {
                _cbsystem.Visible = true;
                Invalidate(true);
            }
            else
            {
                _cbsystem.Visible = false;
                Invalidate(true);
            }
        }

        #endregion // Methods

        #endregion // Public interfaces


        #region Implementation

        #region Fields

        protected ObjectCollection _items;              // ✓

        private ComboBox _cbsystem;                     // ✓ (this.Controls)
        private ComboBoxCustomDropdown _cbdropdown;     // ✓

        private Rectangle topBoxTextArea, arrowRectangleArea, topBoxOutline, topBoxTextTotalArea;
        private Point arrowpt1, arrowpt2, arrowpt3;
        private Point arrowpt1c, arrowpt2c, arrowpt3c;
        private bool isActivated = false;
        private bool mouseover = false;

        #endregion // Fields

        #region OnEvent overrides

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Activate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            //Console.WriteLine("Key press " + e.KeyCode + " Focus " + Focused );

            if (FlatStyle != FlatStyle.System)
            {
                if (SelectedIndex < 0)
                    SelectedIndex = 0;

                if (e.Alt && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
                    Activate();
                else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left)
                {
                    if (SelectedIndex > 0)
                    {
                        _cbsystem.SelectedIndex = SelectedIndex - 1;            // triggers _cbsystem_SelectedIndexChanged
                    }
                }
                else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Right)
                {
                    if (SelectedIndex < Items.Count - 1)
                    {
                        _cbsystem.SelectedIndex = SelectedIndex + 1;            // triggers _cbsystem_SelectedIndexChanged
                    }
                }
            }
        }

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            mouseover = true;
            if (FlatStyle != FlatStyle.System)
                Invalidate();
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            mouseover = false;
            if (FlatStyle != FlatStyle.System)
                Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (FlatStyle != FlatStyle.System)
            {
                int extraborder = 1;

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

                SizeF sz = e.Graphics.MeasureString("Represent THIS", Font);
                topBoxTextArea.Y += (topBoxTextArea.Height - (int)sz.Height) / 2;

                Brush textb;
                Pen p, p2;

                bool todraw = Enabled && Items.Count > 0;

                if (todraw)
                {
                    textb = new SolidBrush(ForeColor);
                    p = new Pen(BorderColor);
                    p2 = new Pen(ForeColor);
                    p2.Width = ArrowWidth;
                }
                else
                {
                    textb = new SolidBrush(ForeColor.Multiply(0.5F));
                    p = new Pen(BorderColor.Multiply(0.5F));
                    p2 = null;
                }

                e.Graphics.DrawRectangle(p, topBoxOutline);

                Color bck;

                if (todraw)
                    bck = (mouseover) ? MouseOverBackgroundColor : BackColor;
                else
                    bck = BackColor.Multiply(0.5F);

                Brush bbck;

                if (FlatStyle == FlatStyle.Popup)
                {
                    bbck = new System.Drawing.Drawing2D.LinearGradientBrush(topBoxTextTotalArea, bck, bck.Multiply(ButtonColorScaling), 90);
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

                StringFormat f = new StringFormat();
                f.Alignment = StringAlignment.Near;
                f.LineAlignment = StringAlignment.Near;
                f.FormatFlags = StringFormatFlags.NoWrap;

                e.Graphics.DrawString(Text, Font, textb, topBoxTextArea, f);

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                bbck.Dispose();

                textb.Dispose();
                p.Dispose();

                if (p2 != null)
                    p2.Dispose();
            }
        }

        #endregion // OnEvent overrides

        #region Methods 

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cbdropdown?.Dispose();
                _items?.Clear();
            }
            _cbdropdown = null;
            _items = null;
            SelectedIndexChanged = null;
            base.Dispose(disposing);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (FlatStyle != FlatStyle.System && (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right))        // grab these nav keys
                return true;
            else
                return base.IsInputKey(keyData);
        }


        private void Activate()
        {
            if (Items.Count == 0 || !Enabled)
                return;

            _cbdropdown = new ComboBoxCustomDropdown(Name);

            int fittableitems = DropDownHeight / ItemHeight;

            if (fittableitems == 0)
            {
                fittableitems = 5;
            }

            if (fittableitems > Items.Count())                             // no point doing more than we have..
                fittableitems = Items.Count();

            _cbdropdown.Size = new Size(DropDownWidth > 9 ? DropDownWidth : Width, fittableitems * ItemHeight + 4);

            _cbdropdown.SelectionBackColor = DropDownBackgroundColor;
            _cbdropdown.MouseOverBackgroundColor = MouseOverBackgroundColor;
            _cbdropdown.ForeColor = ForeColor;
            _cbdropdown.BackColor = BorderColor;
            _cbdropdown.BorderColor = BorderColor;
            _cbdropdown.Items = Items.ToList();
            _cbdropdown.ItemHeight = ItemHeight;
            _cbdropdown.SelectedIndex = SelectedIndex;
            _cbdropdown.FlatStyle = FlatStyle;
            _cbdropdown.Font = Font;
            _cbdropdown.ScrollBarColor = ScrollBarColor;
            _cbdropdown.ScrollBarButtonColor = ScrollBarButtonColor;

            _cbdropdown.DropDown += _cbdropdown_DropDown;
            _cbdropdown.SelectedIndexChanged += _cbdropdown_SelectedIndexChanged;
            _cbdropdown.OtherKeyPressed += _cbdropdown_OtherKeyPressed;
            _cbdropdown.Deactivate += _cbdropdown_Deactivate;

            Control parent = Parent;
            while (parent != null && !(parent is Form))
            {
                parent = parent.Parent;
            }

            _cbdropdown.Show(parent);

            // enforce size.. some reason SHow is scaling it probably due to autosizing.. can't turn off. force back
            _cbdropdown.Size = new Size(DropDownWidth > 9 ? DropDownWidth : Width, fittableitems * ItemHeight + 4);
        }

        #endregion // Methods

        #region Event handlers

        private void _cbdropdown_Deactivate(object sender, EventArgs e)
        {
            isActivated = false;
            Invalidate(true);
        }

        private void _cbdropdown_DropDown(object sender, EventArgs e)
        {
            Point location = PointToScreen(new Point(0, 0));

            int botscr = Screen.FromControl(this).WorkingArea.Height;
            int botcontrol = location.Y + Height + _cbdropdown.Height;

            if (botcontrol < botscr)
                _cbdropdown.Location = new Point(location.X, location.Y + Height);
            else
                _cbdropdown.Location = new Point(location.X, location.Y - _cbdropdown.Height);

            isActivated = true;
            Invalidate(true);
        }

        private void _cbdropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedindex = _cbdropdown.SelectedIndex;
            _cbdropdown.Close();
            isActivated = false;
            Invalidate(true);
            if (_cbsystem.SelectedIndex != selectedindex)
                _cbsystem.SelectedIndex = selectedindex; // triggers _cbsystem_SelectedIndexChanged, but only if we change the index..
            else
                _cbsystem_SelectedIndexChanged(sender, e);      // otherwise, fire it off manually.. this is what the system box does, if the user clicks on it, fires it off
            Focus();
        }

        private void _cbdropdown_OtherKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _cbdropdown.Close();
                isActivated = false;
                Invalidate(true);
            }
        }

        private void _cbsystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            Text = _cbsystem.Text;
            Invalidate(true);

            if (Enabled && SelectedIndexChanged != null)
            {
                SelectedIndexChanged(this, e);
            }
        }

        #endregion // Event handlers

        #endregion // Implementation
    }
}
