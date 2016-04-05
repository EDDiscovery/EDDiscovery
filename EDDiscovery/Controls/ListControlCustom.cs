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
    class ListControlCustom : Control
    {
        public List<string> Items { get; set; }
        public int ScrollBarWidth { get; set; } = 16;
        public int ItemHeight { get; set; } = 20;
        public FlatStyle FlatStyle { get; set; } = FlatStyle.System;
        public float GradientColorScaling { get; set; } = 0.5F;
        public Color BorderColor { get; set; } = Color.Red;
        public int SelectedIndex { get; set; } = -1;

        public delegate void OnSelectedIndexChanged(object sender, EventArgs e);
        public event OnSelectedIndexChanged SelectedIndexChanged;

        private byte limit(float a) { if (a > 255F) return 255; else return (byte)a; }
        public Color Multiply(Color from, float m) { return Color.FromArgb(from.A, limit((float)from.R * m), limit((float)from.G * m), limit((float)from.B * m)); }

        private int firstindex = -1;

        private System.Windows.Forms.VScrollBar vScrollBar;

        public ListControlCustom() : base()
        {
            Items = new List<string>();
            vScrollBar = new VScrollBar();
            vScrollBar.SmallChange = 1;
            vScrollBar.LargeChange = 1;
            Controls.Add(vScrollBar);
            vScrollBar.Visible = false;
            vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(vScroll);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle border = new Rectangle(0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

            Rectangle area = ClientRectangle;
            int extraborder = (FlatStyle != FlatStyle.System) ? 1 : 0;
            area.Inflate(-extraborder, -extraborder);

            int displayableitems = DisplayableItems();

            if ( firstindex < 0 )                                           // if invalid (at start)
            {
                if (SelectedIndex == -1 || Items == null )                  // screen out null..
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
            }

            if (Items != null && displayableitems < Items.Count)                        // if too few..
            {
                vScrollBar.Location = new Point(ClientRectangle.Width - ScrollBarWidth - extraborder,
                                                ClientRectangle.Y + extraborder);
                vScrollBar.Size = new Size(ScrollBarWidth, area.Height);
                area.Width -= ScrollBarWidth;

                vScrollBar.Minimum = 0;
                vScrollBar.Value = firstindex;
                vScrollBar.Maximum = Items.Count - displayableitems;
                vScrollBar.Visible = true;
            }

            if (FlatStyle != FlatStyle.System || !ComboBoxRenderer.IsSupported)
            {
                Pen p = new Pen(this.BorderColor);
                e.Graphics.DrawRectangle(p, border);
                p.Dispose();

                Brush backb;

                if (FlatStyle == FlatStyle.Popup)
                    backb = new System.Drawing.Drawing2D.LinearGradientBrush(area, this.BackColor, Multiply(this.BackColor, GradientColorScaling), 90);
                else
                    backb = new SolidBrush(this.BackColor);

                e.Graphics.FillRectangle(backb, area);
                backb.Dispose();
            }

            if (Items != null && Items.Count > 0)
            {
                Rectangle pos = area;
                pos.Height = ItemHeight;
                int offset = 0;

                Brush textb = new SolidBrush(this.ForeColor);

                foreach (string s in Items)
                {
                    if (offset >= firstindex && offset < firstindex + displayableitems)
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

                textb.Dispose();
            }
        }


        public int DisplayableItems()
        {
            int extraborder = (FlatStyle != FlatStyle.System) ? 2 : 0;
            int displayableitems = (ClientRectangle.Height - extraborder) / ItemHeight;
            return displayableitems;
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
            SelectedIndex = firstindex + e.Location.Y / ItemHeight;

            if ( SelectedIndexChanged != null )
                SelectedIndexChanged(this, new EventArgs());
        }

        protected void GoUpOne()
        {
            if (firstindex > 0)
            {
                firstindex--;
                Invalidate();
            }
        }
        protected void GoDownOne()
        {
            if (Items != null && firstindex < Items.Count() - DisplayableItems())
            {
                firstindex++;
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

    }
}



