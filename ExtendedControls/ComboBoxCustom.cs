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
    public class ComboBoxCustomDropdown : Form
    {
        private ListControlCustom _listcontrol;

        public event EventHandler DropDown;
        public event EventHandler SelectedIndexChanged;
        public event KeyPressEventHandler KeyPressed;
        public event KeyEventHandler OtherKeyPressed;

        public Color MouseOverBackgroundColor { get { return _listcontrol.MouseOverBackgroundColor; } set { _listcontrol.MouseOverBackgroundColor = value; } }
        public int SelectedIndex { get { return _listcontrol.SelectedIndex; } set { _listcontrol.SelectedIndex = value; } }
        public Color SelectionBackColor { get { return _listcontrol.SelectionBackColor; } set { _listcontrol.SelectionBackColor = value; this.BackColor = value; } }
        public List<string> Items { get { return _listcontrol.Items; } set { _listcontrol.Items = value; } }
        public Color BorderColor { get { return _listcontrol.BorderColor; } set { _listcontrol.BorderColor = value; } }
        public Color ScrollBarColor { get { return _listcontrol.ScrollBarColor; } set { _listcontrol.ScrollBarColor = value; } }
        public Color ScrollBarButtonColor { get { return _listcontrol.ScrollBarButtonColor; } set { _listcontrol.ScrollBarButtonColor = value; } }
        public FlatStyle FlatStyle { get { return _listcontrol.FlatStyle; } set { _listcontrol.FlatStyle = value; } }
        public new Font Font { get { return base.Font; } set { base.Font = value; _listcontrol.Font = value; } }

        public int ItemHeight { get { return _listcontrol.ItemHeight; } set { _listcontrol.ItemHeight = value; } }

        public ComboBoxCustomDropdown(string name = "")
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this._listcontrol = new ListControlCustom();
            this.Name = this._listcontrol.Name = name;
            this._listcontrol.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this._listcontrol.Dock = DockStyle.Fill;
            this._listcontrol.Visible = true;
            this._listcontrol.SelectedIndexChanged += _listcontrol_SelectedIndexChanged;
            this._listcontrol.KeyPressed += _listcontrol_KeyPressed;
            this._listcontrol.OtherKeyPressed += _listcontrol_OtherKeyPressed;
            this._listcontrol.Margin = new Padding(0);
            this.Padding = new Padding(0);
            this.Controls.Add(this._listcontrol);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            DropDown(this, e);
        }

        private void _listcontrol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( SelectedIndexChanged!=null)
                SelectedIndexChanged(this, e);
            this.Close();
        }

        private void _listcontrol_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if ( KeyPressed != null )
                KeyPressed(this, e);
        }

        private void _listcontrol_OtherKeyPressed(object sender, KeyEventArgs e)
        {
            if (OtherKeyPressed != null)
                OtherKeyPressed(this, e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            this.Close();
        }
    }

    public class ComboBoxCustom : Control
    {
        public class ObjectCollection : IList<string>, ICollection<string>
        {
            private IList _explicitCollection;
            private ComboBox _combobox;

            private IList _collection
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


            public ObjectCollection(ComboBox cb)
            {
                this._combobox = cb;
            }

            public ObjectCollection(IList<string> vals)
            {
                _explicitCollection = vals as IList;
            }

            public string this[int index]
            {
                get
                {
                    return _collection[index].ToNullSafeString();
                }

                set
                {
                    _collection[index] = value;
                }
            }

            public int Count
            {
                get
                {
                    return _collection.Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return _collection.IsReadOnly;
                }
            }

            public void Add(string item)
            {
                _collection.Add(item);
            }

            public void AddRange(IEnumerable<string> items)
            {
                foreach (var val in items)
                {
                    _collection.Add(val);
                }
            }

            public void Clear()
            {
                _collection.Clear();
            }

            public bool Contains(string item)
            {
                return _collection.Contains(item);
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                _collection.OfType<object>().Select(v => v.ToNullSafeString()).ToList().CopyTo(array, arrayIndex);
            }

            public IEnumerator<string> GetEnumerator()
            {
                return _collection.OfType<object>().Select(v => v.ToNullSafeString()).GetEnumerator();
            }

            public int IndexOf(string item)
            {
                return _collection.IndexOf(item);
            }

            public void Insert(int index, string item)
            {
                _collection.Insert(index, item);
            }

            public bool Remove(string item)
            {
                _collection.Remove(item);
                return true;
            }

            public void RemoveAt(int index)
            {
                _collection.RemoveAt(index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public static implicit operator ObjectCollection(List<string> vals)
            {
                return new ObjectCollection(vals);
            }
        }

        protected ObjectCollection _items;

        private ComboBox _cbsystem;
        private Rectangle topBoxTextArea, arrowRectangleArea, topBoxOutline, topBoxTextTotalArea;
        private Point arrowpt1, arrowpt2, arrowpt3;
        private Point arrowpt1c, arrowpt2c, arrowpt3c;
        private bool isActivated = false;
        private bool mouseover = false;
        private ComboBoxCustomDropdown _customdropdown;

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
        public new System.Drawing.Size Size { get { return _cbsystem.Size; } set { _cbsystem.Size = value; base.Size = value; } }

        public event EventHandler SelectedIndexChanged;

        public ComboBoxCustom()
        {
            //Text = "";
            this._cbsystem = new ComboBox();
            this._cbsystem.Dock = DockStyle.Fill;
            this._cbsystem.SelectedIndexChanged += _cbsystem_SelectedIndexChanged;
            this._cbsystem.DropDownStyle = ComboBoxStyle.DropDownList;
            this._cbsystem.MouseLeave += _cbsystem_MouseLeave;
            this._cbsystem.MouseEnter += _cbsystem_MouseEnter;
            this._items = new ObjectCollection(this._cbsystem);
            this.Controls.Add(this._cbsystem);
        }

        public ComboBox GetInternalSystemControl { get { return this._cbsystem; }  }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.FlatStyle != FlatStyle.System)
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

                SizeF sz = e.Graphics.MeasureString("Represent THIS", this.Font);
                topBoxTextArea.Y += (topBoxTextArea.Height - (int)sz.Height) / 2;

                Brush textb;
                Pen p, p2;

                bool todraw = Enabled && Items.Count > 0;

                if (todraw)
                {
                    textb = new SolidBrush(this.ForeColor);
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

                if (p2 != null )
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

                e.Graphics.DrawString(this.Text, this.Font, textb, topBoxTextArea, f);

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                bbck.Dispose();

                textb.Dispose();
                p.Dispose();

                if (p2 != null)
                    p2.Dispose();
            }
        }

        public void Repaint()
        {
            if (this.FlatStyle == FlatStyle.System)
            {
                _cbsystem.Visible = true;
                this.Invalidate(true);
            }
            else
            {
                _cbsystem.Visible = false;
                this.Invalidate(true);
            }
        }

        private void _cbsystem_MouseEnter(object sender, EventArgs e)       // if cbsystem is active, fired.. pass onto our ME handler
        {
            //System.Diagnostics.Debug.WriteLine("CB sys Mouse enter " + _cbsystem.Size + " "  + this.Size);
            base.OnMouseEnter(e);
        }

        private void _cbsystem_MouseLeave(object sender, EventArgs e)       // if cbsystem is active, fired.. pass onto our ML handler.
        {
            //System.Diagnostics.Debug.WriteLine("CB sys Mouse leave");
            base.OnMouseLeave(e);
        }

        protected override void OnMouseEnter(EventArgs eventargs)           // ours is active.  Fired when entered
        {
            //System.Diagnostics.Debug.WriteLine("CBC Enter , visible " + _cbsystem.Visible);
            if (!_cbsystem.Visible)
            {
                base.OnMouseEnter(eventargs);
                mouseover = true;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            //System.Diagnostics.Debug.WriteLine("CBC Leave, activated" + isActivated + " visible " + _cbsystem.Visible);

            if (!_cbsystem.Visible)
            {
                if (isActivated == false)
                    base.OnMouseLeave(eventargs);

                mouseover = false;
                Invalidate();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            //System.Diagnostics.Debug.WriteLine("Key press " + e.KeyCode + " Focus " + Focused );

            if (this.FlatStyle != FlatStyle.System)
            {
                if (SelectedIndex < 0)
                    SelectedIndex = 0;

                if (e.Alt && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
                    Activate();
                else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left)
                {
                    if ( SelectedIndex>0)
                    {
                        _cbsystem.SelectedIndex = SelectedIndex-1;            // triggers _cbsystem_SelectedIndexChanged
                    }
                }
                else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Right)
                {
                    if ( SelectedIndex<this.Items.Count-1)
                    {
                        _cbsystem.SelectedIndex = SelectedIndex+1;            // triggers _cbsystem_SelectedIndexChanged
                    }
                }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (this.FlatStyle != FlatStyle.System && (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right))        // grab these nav keys
                return true;
            else
                return base.IsInputKey(keyData);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Activate();
        }

        private void Activate()
        {
            if (Items.Count == 0 || !Enabled)
                return;

            _customdropdown = new ComboBoxCustomDropdown(this.Name);

            int fittableitems = this.DropDownHeight / this.ItemHeight;

            if (fittableitems == 0)
            {
                fittableitems = 5;
            }

            if (fittableitems > this.Items.Count())                             // no point doing more than we have..
                fittableitems = this.Items.Count();

            _customdropdown.Size = new Size(this.DropDownWidth > 9 ? this.DropDownWidth : this.Width, fittableitems * this.ItemHeight + 4);

            _customdropdown.SelectionBackColor = this.DropDownBackgroundColor;
            _customdropdown.MouseOverBackgroundColor = this.MouseOverBackgroundColor;
            _customdropdown.ForeColor = this.ForeColor;
            _customdropdown.BackColor = this.BorderColor;
            _customdropdown.BorderColor = this.BorderColor;
            _customdropdown.Items = this.Items.ToList();
            _customdropdown.ItemHeight = this.ItemHeight;
            _customdropdown.SelectedIndex = this.SelectedIndex;
            _customdropdown.FlatStyle = this.FlatStyle;
            _customdropdown.Font = this.Font;
            _customdropdown.ScrollBarColor = this.ScrollBarColor;
            _customdropdown.ScrollBarButtonColor = this.ScrollBarButtonColor;

            _customdropdown.DropDown += _customdropdown_DropDown;
            _customdropdown.SelectedIndexChanged += _customdropdown_SelectedIndexChanged;
            _customdropdown.OtherKeyPressed += _customdropdown_OtherKeyPressed;
            _customdropdown.Deactivate += _customdropdown_Deactivate;

            Control parent = this.Parent;
            while (parent != null && !(parent is Form))
            {
                parent = parent.Parent;
            }

            _customdropdown.Show(parent);
             
            // enforce size.. some reason SHow is scaling it probably due to autosizing.. can't turn off. force back
            _customdropdown.Size = new Size(this.DropDownWidth > 9 ? this.DropDownWidth : this.Width, fittableitems * this.ItemHeight + 4);
        }

        private void _customdropdown_Deactivate(object sender, EventArgs e)
        {
            isActivated = false;
            this.Invalidate(true);
        }

        private void _customdropdown_DropDown(object sender, EventArgs e)
        {
            Point location = this.PointToScreen(new Point(0, 0));

            int botscr = Screen.FromControl(this).WorkingArea.Height;
            int botcontrol = location.Y + this.Height + _customdropdown.Height;

            if (botcontrol < botscr)
                _customdropdown.Location = new Point(location.X, location.Y + this.Height);
            else
                _customdropdown.Location = new Point(location.X, location.Y -_customdropdown.Height);

            isActivated = true;
            this.Invalidate(true);
        }

        private void _customdropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedindex = _customdropdown.SelectedIndex;
            _customdropdown.Close();
            isActivated = false;
            this.Invalidate(true);
            if (_cbsystem.SelectedIndex != selectedindex)
                _cbsystem.SelectedIndex = selectedindex; // triggers _cbsystem_SelectedIndexChanged, but only if we change the index..
            else
                _cbsystem_SelectedIndexChanged(sender, e);      // otherwise, fire it off manually.. this is what the system box does, if the user clicks on it, fires it off
            Focus();

            base.OnMouseLeave(e);    // same as mouse 
        }

        private void _customdropdown_OtherKeyPressed(object sender, KeyEventArgs e)
        {
            if ( e.KeyCode == Keys.Escape )
            {
                _customdropdown.Close();
                isActivated = false;
                this.Invalidate(true);
            }
        }

        private void _cbsystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Text = _cbsystem.Text;
            this.Invalidate(true);

            if (this.Enabled && SelectedIndexChanged != null)
            {
                SelectedIndexChanged(this, e);
            }

            base.OnMouseLeave(e);    // same as mouse 
        }
    }
}
