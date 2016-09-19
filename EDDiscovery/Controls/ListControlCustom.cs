using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ExtendedControls
{
    public class ListControlCustom : Control
    {
        // BackColor paints the whole control - set Transparent if you don't want this. (but its a fake transparent note).

        public List<string> Items { get; set; }
        public bool FitToItemsHeight { get; set; } = true;                    // if set, move the border to integer of item height.
        public int ScrollBarWidth { get; set; } = 16;
        public int ItemHeight { get; set; } = 20;
        public FlatStyle FlatStyle { get { return flatstyle; } set { SetFlatStyle(value); } }
        FlatStyle flatstyle = FlatStyle.System;

        public Color SelectionBackColor { get; set; } = Color.Gray;     // the area actually used (Not system)
        public Color BorderColor { get; set; } = Color.Red;             // not system
        public float GradientColorScaling { get; set; } = 0.5F;
        public Color ScrollBarColor { get; set; } = Color.LightGray;    // not system
        public Color ScrollBarButtonColor { get; set; } = Color.LightGray;    // not system
        public Color MouseOverBackgroundColor { get; set; } = Color.Silver;

        public int SelectedIndex { get; set; } = -1;

        public delegate void OnSelectedIndexChanged(object sender, EventArgs e);
        public event OnSelectedIndexChanged SelectedIndexChanged;

        public delegate void OnKeyPressed(object sender, KeyPressEventArgs e);
        public event OnKeyPressed KeyPressed;

        public delegate void OnAnyOtherKeyPressed(object sender, KeyEventArgs e);
        public event OnAnyOtherKeyPressed OtherKeyPressed;

        ListBox lbsys;

        #region Implementation

        public ListControlCustom() : base()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
            Items = new List<string>();
            vScrollBar = new VScrollBarCustom();
            vScrollBar.SmallChange = 1;
            vScrollBar.LargeChange = 1;
            Controls.Add(vScrollBar);
            vScrollBar.Visible = false;
            vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(vScroll);

            lbsys = new ListBox();
            this.Controls.Add(lbsys);
            lbsys.SelectedIndexChanged += lbsys_SelectedIndexChanged;
        }

        public void SetFlatStyle( FlatStyle v)
        {
            flatstyle = v;
            lbsys.Visible = (flatstyle == FlatStyle.System);
            this.Invalidate();
            lbsys.Items.AddRange(this.Items.ToArray());
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            if (lbsys != null && flatstyle == FlatStyle.System && this.Width > 0)
            {
                lbsys.Width = this.Width;
                lbsys.Height = this.Height;
            }
        }

        private void CalculateLayout()
        {
            bordersize = 0;

            if (FlatStyle != FlatStyle.System && BorderColor != Color.Transparent)
                bordersize = 2;

            int items = (Items != null) ? Items.Count() : 0;
            itemslayoutestimatedon = items;

            displayableitems = (ClientRectangle.Height-bordersize*2) / ItemHeight;            // number of items to display

            if (items > 0 && displayableitems > items)
                displayableitems = items;

            mainarea = new Rectangle(bordersize, bordersize, 
                            ClientRectangle.Width - bordersize * 2, 
                            (FitToItemsHeight) ? (displayableitems * ItemHeight) : (ClientRectangle.Height - bordersize*2));
            borderrect = mainarea;
            borderrect.Inflate(bordersize,bordersize);
            borderrect.Width--; borderrect.Height--;        // adjust to rect not area.

            if ( items > displayableitems )
            {
                vScrollBar.Location = new Point(mainarea.Right - ScrollBarWidth, mainarea.Y);
                mainarea.Width -= ScrollBarWidth;
                vScrollBar.Size = new Size(ScrollBarWidth, mainarea.Height);
                vScrollBar.Minimum = 0;
                vScrollBar.Maximum = Items.Count - displayableitems;
                vScrollBar.Visible = true;
                vScrollBar.FlatStyle = FlatStyle;

                vScrollBar.SliderColor = ScrollBarColor;
                vScrollBar.BackColor = ScrollBarColor;
                vScrollBar.BorderColor = vScrollBar.ThumbBorderColor = vScrollBar.ArrowBorderColor = BorderColor;
                vScrollBar.ArrowButtonColor = vScrollBar.ThumbButtonColor = ScrollBarButtonColor;
                vScrollBar.MouseOverButtonColor = Multiply(ScrollBarButtonColor, 1.4F);
                vScrollBar.MousePressedButtonColor = Multiply(ScrollBarButtonColor, 1.5F);
                vScrollBar.ForeColor = Multiply(ScrollBarButtonColor, 0.25F);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.FlatStyle == FlatStyle.System)
                return;

            if (Items != null && itemslayoutestimatedon != Items.Count())  // item count changed, rework it out.
                CalculateLayout();
            if (firstindex < 0)                                           // if invalid (at start)
            {
                if (SelectedIndex == -1 || Items == null)                  // screen out null..
                    firstindex = 0;
                else
                {
                    firstindex = SelectedIndex;

                    if (firstindex + displayableitems > Items.Count)        // no point leaving the display half populated
                    {
                        firstindex = Items.Count - displayableitems;        // go back..
                        if (firstindex < 0)                                 // if too far (because displayable items > list size)
                            firstindex = 0;
                    }
                }

                vScrollBar.Value = firstindex;
            }

            focusindex = (focusindex >= 0) ? focusindex : ((SelectedIndex > 0) ? SelectedIndex : 0);

            Pen p = new Pen(this.BorderColor);
            for (int i = 0; i < bordersize; i++)
            {
                var brect = new Rectangle(borderrect.Left + i, borderrect.Top + i, borderrect.Width - i * 2, borderrect.Height - i * 2);
                e.Graphics.DrawRectangle(p, borderrect);
            }
            p.Dispose();

            Brush backb;

            if ( this.SelectionBackColor != Color.Transparent)
            {
                Color c1 = SelectionBackColor;
                if (FlatStyle == FlatStyle.Popup)
                    backb = new System.Drawing.Drawing2D.LinearGradientBrush(mainarea, c1, Multiply(c1, GradientColorScaling), 90);
                else
                    backb = new SolidBrush(c1);

                e.Graphics.FillRectangle(backb, mainarea);
                backb.Dispose();
            }

            if (Items != null && Items.Count > 0)
            {
                Rectangle pos = mainarea;
                pos.Height = ItemHeight;
                int offset = 0;

                Brush textb = new SolidBrush(this.ForeColor);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

               // Console.WriteLine("Paint {0} {1}", focusindex, firstindex);
                foreach (string s in Items)
                {   // if not fitting to items height, 
                    if (offset >= firstindex && offset < firstindex + displayableitems + (FitToItemsHeight ? 0:1))
                    {
                        if (offset == focusindex)
                        {
                            using (Brush highlight = new SolidBrush(MouseOverBackgroundColor))
                            {
                                e.Graphics.FillRectangle(highlight, pos);
                            }
                        }

                        e.Graphics.DrawString(s, this.Font, textb, pos);

                        pos.Y += ItemHeight;
                    }

                    offset++;
                }

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                textb.Dispose();
            }
        }


        protected void vScroll(object sender, ScrollEventArgs e)
        {
            if (firstindex != e.NewValue)
            {
                firstindex = e.NewValue;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            int items = (Items != null) ? Items.Count() : 0;

            if ( items > 0 )                                // if any items.. just to check
            { 
                int index = firstindex + e.Location.Y / ItemHeight;

                if (index >= items)                 // due to the few pixels for border.  we let them have this
                    index = items - 1;

                SelectedIndex = index;
                if (SelectedIndexChanged != null)
                    SelectedIndexChanged(this, new EventArgs());
            }
        }

        protected void ScrollUpOne()
        {
            if (firstindex > 0)
            {
                firstindex--;
                vScrollBar.Value = firstindex;
                Invalidate();
            }
        }
        protected void ScrollDownOne()
        {
            if (Items != null && firstindex < Items.Count() - displayableitems)
            {
                firstindex++;
                vScrollBar.Value = firstindex;
                Invalidate();
            }
        }

        protected void FocusUpOne()
        {
            if (focusindex > 0)
            {
                focusindex--;
                Invalidate();
                if (focusindex < firstindex)
                    ScrollUpOne();
            }
        }

        protected void FocusDownOne()
        {
            if (Items != null && focusindex < Items.Count()-1)
            {
                focusindex++;
                Invalidate();
                if (focusindex >= firstindex + displayableitems)
                    ScrollDownOne();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (flatstyle == FlatStyle.System)
                return;

            if (e.Delta > 0)
                ScrollUpOne();
            else
                ScrollDownOne();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (flatstyle == FlatStyle.System)
                return;

            int y = e.Location.Y;
            int index = (y / ItemHeight) + firstindex;
            focusindex = index;
            Invalidate();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)        // grab these nav keys
                return true;
            else
                return base.IsInputKey(keyData);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (KeyPressed != null)
               KeyPressed(this, e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            //Console.WriteLine("Key press " + e.KeyCode + " Focus " + Focused );

            if (flatstyle != FlatStyle.System)
            {
                if (e.KeyCode == Keys.Enter || (e.Alt && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)))
                {
                    SelectedIndex = focusindex;
                    if (SelectedIndexChanged != null)
                        SelectedIndexChanged(this, new EventArgs());
                }
                else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Right)
                    FocusDownOne();
                else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left)
                    FocusUpOne();
            }

            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                if (OtherKeyPressed != null)
                    OtherKeyPressed(this, e);
            }
        }

        private void lbsys_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndex = lbsys.SelectedIndex;
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, new EventArgs());
        }


        public void Repaint()
        {
            this.Invalidate(true);
        }

        private byte limit(float a) { if (a > 255F) return 255; else return (byte)a; }
        public Color Multiply(Color from, float m) { return Color.FromArgb(from.A, limit((float)from.R * m), limit((float)from.G * m), limit((float)from.B * m)); }

        #endregion

        private VScrollBarCustom vScrollBar;
        private Rectangle borderrect, mainarea;
        private int bordersize;
        private int itemslayoutestimatedon = -1;
        private int displayableitems = -1;
        private int firstindex = -1;
        private int focusindex = -1;
    }
}



