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
        public FlatStyle FlatStyle { get; set; } = FlatStyle.System;

        public Color SelectionBackColor { get; set; } = Color.Gray;     // the area actually used (Not system)
        public Color BorderColor { get; set; } = Color.Red;             // not system
        public float GradientColorScaling { get; set; } = 0.5F;
        public Color ScrollBarColor { get; set; } = Color.LightGray;    // not system
        public Color ScrollBarButtonColor { get; set; } = Color.LightGray;    // not system

        public int SelectedIndex { get; set; } = -1;

        public delegate void OnSelectedIndexChanged(object sender, EventArgs e);
        public event OnSelectedIndexChanged SelectedIndexChanged;

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
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
        }

        private void CalculateLayout()
        { 
            int bordersize = 0;

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


            if (FlatStyle != FlatStyle.System || !ComboBoxRenderer.IsSupported)
            {
                Pen p = new Pen(this.BorderColor);
                e.Graphics.DrawRectangle(p, borderrect);
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
            }

            if (Items != null && Items.Count > 0)
            {
                Rectangle pos = mainarea;
                pos.Height = ItemHeight;
                int offset = 0;

                Brush textb = new SolidBrush(this.ForeColor);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                foreach (string s in Items)
                {   // if not fitting to items height, 
                    if (offset >= firstindex && offset < firstindex + displayableitems + (FitToItemsHeight ? 0:1))
                    {
                        if (FlatStyle == FlatStyle.System && ComboBoxRenderer.IsSupported)
                        {
                            ComboBoxRenderer.DrawTextBox(e.Graphics, pos, s, this.Font, ComboBoxState.Pressed);
                        }
                        else
                        {
                            e.Graphics.DrawString(s, this.Font, textb, pos);
                        }

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

        protected void GoUpOne()
        {
            if (firstindex > 0)
            {
                firstindex--;
                vScrollBar.Value = firstindex;
                Invalidate();
            }
        }
        protected void GoDownOne()
        {
            if (Items != null && firstindex < Items.Count() - displayableitems)
            {
                firstindex++;
                vScrollBar.Value = firstindex;
                Invalidate();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta > 0)
                GoUpOne();
            else
                GoDownOne();
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
            //Console.WriteLine("Key press " + e.KeyCode + " Focus " + Focused );

            if (e.KeyCode == Keys.Down)
                GoDownOne();
            else if (e.KeyCode == Keys.Up)
                GoUpOne();
        }

        private byte limit(float a) { if (a > 255F) return 255; else return (byte)a; }
        public Color Multiply(Color from, float m) { return Color.FromArgb(from.A, limit((float)from.R * m), limit((float)from.G * m), limit((float)from.B * m)); }

        #endregion

        private VScrollBarCustom vScrollBar;
        private Rectangle borderrect, mainarea;
        private int itemslayoutestimatedon = -1;
        private int displayableitems = -1;
        private int firstindex = -1;
    }
}



