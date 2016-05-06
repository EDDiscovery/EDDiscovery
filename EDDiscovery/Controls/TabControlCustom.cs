using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*Code parts from : http://www.codeproject.com/Articles/91387/Painting-Your-Own-Tabs-Second-Edition
 * These parts are provided under the Code Project Open Licence (CPOL)
 * See http://www.codeproject.com/info/cpol10.aspx for details
 */

namespace ExtendedControls
{
    class TabControlCustom : TabControl
    {
        #region Properties

        // Invalidate to repaint - only useful if not system Flatstyle
        public Color TabControlBorderColor { get; set; } = Color.DarkGray;       //Selected tabs are outlined in this
        public Color TabControlBorderBrightColor { get; set; } = Color.LightGray;

        public Color TabNotSelectedBorderColor { get; set; } = Color.Gray;     //Unselected tabs are outlined in this

        public Color TabNotSelectedColor { get; set; } = Color.Gray;            // tabs are filled with this
        public Color TabSelectedColor { get; set; } = Color.LightGray;
        public Color TabMouseOverColor { get; set; } = Color.White;

        public Color TextSelectedColor { get; set; } = SystemColors.ControlText;             // text is painted in this..
        public Color TextNotSelectedColor { get; set; } = SystemColors.ControlText;

        public float TabColorScaling { get; set; } = 0.5F;                      // gradiant fill..
        public float TabDisabledScaling { get; set; } = 0.5F;                   // how much darker if not selected.
        public float TabOpaque { get; set; } = 100F;                            // is the tab area opaque?

        public int MinimumTabWidth { set { SendMessage(0x1300 + 49, IntPtr.Zero, (IntPtr)value); } }
        public override Font Font { get { return base.Font; } set { ChangeFont(value); } }

        // Auto Invalidates
        // style is Flat, Popup (gradient) and System
        public FlatStyle FlatStyle { get { return flatstyle; } set { ChangeFlatStyle(value); } }

        // attach a tab style class which determines the shape and formatting.
        public TabStyleCustom TabStyle { get { return tabstyle; } set { ChangeTabStyle(value); } }

        #endregion

        #region Initialisation
        public TabControlCustom() : base()
        {
        }
        #endregion

        #region ChangeStyles

        private void ChangeTabStyle(TabStyleCustom fs)
        {
            tabstyle = fs;
            CleanUp();              // start afresh
            Invalidate();           // and repaint
        }

        public const int WM_SETFONT = 0x30;
        public const int WM_FONTCHANGE = 0x1d;

        private IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(this.Handle, msg, wparam, lparam);
            this.WndProc(ref message);
            return message.Result;
        }

        private void ChangeFlatStyle(FlatStyle fs)
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                                ControlStyles.Opaque | ControlStyles.ResizeRedraw, (fs != FlatStyle.System));

            // asking for a font set seems to make it set size better during start up
            SendMessage(WM_SETFONT, (IntPtr)this.Font.ToHfont(), (IntPtr)(-1));
            SendMessage(WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero);

            flatstyle = fs;
            CleanUp();              // start afresh
            Invalidate();           // and repaint
        }

        private void ChangeFont(Font fs)
        {         // going back to normal mode seems to allow the tab to size properly to the font
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                                ControlStyles.Opaque | ControlStyles.ResizeRedraw, false);

            base.Font = fs;
            CleanUp();

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                                ControlStyles.Opaque | ControlStyles.ResizeRedraw, (flatstyle != FlatStyle.System));
        }

        public int CalculateMinimumTabWidth()                                // given fonts and the tab text, whats the minimum width?
        {
            Graphics gr = Parent.CreateGraphics();

            int minsize = 0;

            foreach (TabPage p in TabPages)
            {
                //Console.WriteLine("Text is " + p.Text);
                SizeF sz = gr.MeasureString(p.Text, this.Font);

                if (sz.Width > minsize)
                    minsize = (int)sz.Width+1;  // +1 due to float round down..
            }

            gr.Dispose();

            return minsize;
        }

        #endregion

        #region Mouse

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int currentmouseover = mouseover;

            if (GetTabRect(SelectedIndex).Contains(e.Location))
            {
                mouseover = SelectedIndex;
            }
            else
            {
                mouseover = -1;
                for (int i = 0; i < TabCount; i++)
                {
                    if (i != SelectedIndex && GetTabRect(i).Contains(e.Location))
                    {
                        mouseover = i;
                    }
                }
            }

            if (mouseover != currentmouseover && flatstyle != FlatStyle.System)
                Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (mouseover != -1)
            {
                mouseover = -1;

                if (flatstyle != FlatStyle.System)
                    Invalidate();
            }
        }

        #endregion

        #region CustomPainting
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!e.ClipRectangle.Equals(this.ClientRectangle))      // always repaint whole
            {
                Invalidate();
                return;
            }

            if (backImageControlBitmap == null)    // First time, we have size..
            {
                //this.ItemSize = new Size(250, 20);
                Size sz = this.ItemSize;
                Rectangle p1, p2;
                p1 = GetTabRect(0);
                p2 = GetTabRect(1);

                backImageControlBitmap = new Bitmap(Width, Height);
                Graphics backGraphics = Graphics.FromImage(backImageControlBitmap);
                PaintTransparentBackground(backGraphics, ClientRectangle);    // force the paint of the background into this bitmap.

                backImageBitmap = new Bitmap(Width, Height);
                backImageGraphics = Graphics.FromImage(backImageBitmap);

                tabImageBitmap = new Bitmap(this.Width, this.Height);
                tabImageGraphics = Graphics.FromImage(this.tabImageBitmap);

                int topheight = DisplayRectangle.Y;
                int bottomborder = ClientRectangle.Height - DisplayRectangle.Height - topheight;
                int sideborders = DisplayRectangle.X;

                tabcontrolborder = new Rectangle(0, topheight - 2, ClientRectangle.Width - 1, ClientRectangle.Height - (topheight - 2) - 1);
            }

            backImageGraphics.Clear(Color.Transparent);
            backImageGraphics.DrawImageUnscaled(backImageControlBitmap, 0, 0);

            tabImageGraphics.Clear(Color.Transparent);          // so I tried just drawing on the backImage.. but it did not work. No idea

            DrawBorder(tabImageGraphics);

            if (TabCount > 0)
            {
                for (int i = TabCount - 1; i >= 0; i--)
                {
                    if (i != SelectedIndex)
                        DrawTab(i, tabImageGraphics, false, mouseover == i);
                }

                DrawTab(SelectedIndex, tabImageGraphics, true, false);     // we paint the selected one last, in case it overwrites the other ones.
            }

            tabImageGraphics.Flush();

            ColorMatrix alphaMatrix = new ColorMatrix();
            alphaMatrix.Matrix00 = alphaMatrix.Matrix11 = alphaMatrix.Matrix22 = alphaMatrix.Matrix44 = 1;
            alphaMatrix.Matrix33 = TabOpaque / 100F;

            // Create a new image attribute object and set the color matrix to
            // the one just created
            using (ImageAttributes alphaAttributes = new ImageAttributes())
            {
                alphaAttributes.SetColorMatrix(alphaMatrix);
                this.backImageGraphics.DrawImage(this.tabImageBitmap,
                    new Rectangle(0, 0, this.tabImageBitmap.Width, this.tabImageBitmap.Height),
                    0, 0, this.tabImageBitmap.Width, this.tabImageBitmap.Height, GraphicsUnit.Pixel,
                                                   alphaAttributes);
            }

            backImageGraphics.Flush();

            e.Graphics.DrawImageUnscaled(backImageBitmap, 0, 0);
        }


        private void DrawTab(int i, Graphics gr, bool selected, bool mouseover)
        {
            if (TabStyle == null)
                throw new ArgumentNullException("Custom style not attached");

            Color tabc1 = (Enabled) ? ((selected) ? TabSelectedColor : ((mouseover) ? TabMouseOverColor : TabNotSelectedColor)) : Multiply(TabNotSelectedColor, TabDisabledScaling);
            Color tabc2 = (FlatStyle == FlatStyle.Popup) ? Multiply(tabc1, TabColorScaling) : tabc1;
            Color taboutline = (selected) ? TabControlBorderColor : TabNotSelectedBorderColor;
            TabStyle.DrawTab(gr, GetTabRect(i), i, selected, tabc1, tabc2, taboutline);

            Color tabtextc = (Enabled) ? ((selected) ? TextSelectedColor : TextNotSelectedColor) : Multiply(TextNotSelectedColor, TabDisabledScaling);
            TabStyle.DrawText(gr, GetTabRect(i), i, selected, tabtextc, this.TabPages[i].Text, Font);

            gr.SmoothingMode = SmoothingMode.Default;
        }

        private void DrawBorder(Graphics gr)
        {
            Pen dark = new Pen(TabControlBorderColor, 1.0F);
            Pen bright = new Pen(TabControlBorderBrightColor, 1.0F);
            Pen bright2 = new Pen(TabControlBorderBrightColor, 2.0F);

            Rectangle b1 = new Rectangle(tabcontrolborder.X, tabcontrolborder.Y, tabcontrolborder.Width - 2, tabcontrolborder.Height);
            gr.DrawRectangle(dark, b1);

            b1.Inflate(-1, -1);
            gr.DrawRectangle(bright, b1);

            gr.DrawLine(bright2, b1.X + 2, b1.Y + 1, b1.X + 2, b1.Y + b1.Height);
            gr.DrawLine(bright2, b1.X + 3, b1.Y + b1.Height - 1, b1.X + b1.Width, b1.Y + b1.Height - 1);
            gr.DrawLine(bright2, tabcontrolborder.Width, tabcontrolborder.Y, tabcontrolborder.Width, tabcontrolborder.Y + tabcontrolborder.Height + 1);
            bright.Dispose();
            bright2.Dispose();
            dark.Dispose();
        }

        protected void PaintTransparentBackground(Graphics graphics, Rectangle clipRect)
        {
            if ((Parent != null))
            {
                clipRect.Offset(Location);
                GraphicsState state = graphics.Save();
                graphics.TranslateTransform((float)-Location.X, (float)-Location.Y);
                graphics.SmoothingMode = SmoothingMode.HighSpeed;

                PaintEventArgs e = new PaintEventArgs(graphics, clipRect);
                try
                {
                    InvokePaintBackground(Parent, e); // we force it to paint into our bitmap
                    InvokePaint(Parent, e);   // which we do not use.
                }
                finally
                {
                    graphics.Restore(state);
                    clipRect.Offset(-Location.X, -Location.Y);
                }
            }
        }

        #endregion

        #region Helpers
        private byte limit(float a) { if (a > 255F) return 255; else return (byte)a; }
        public Color Multiply(Color from, float m) { return Color.FromArgb(from.A, limit((float)from.R * m), limit((float)from.G * m), limit((float)from.B * m)); }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                CleanUp();
            }
        }
        protected override void OnResize(EventArgs e)       // resize means we need to start again
        {
            if (Width > 0 && Height > 0)
            {
                CleanUp();
            }

            base.OnResize(e);
        }

        protected override void OnMove(EventArgs e)
        {
            if (this.Width > 0 && this.Height > 0)
            {
                CleanUp();                          // Ensures we start from scratch..
            }
            base.OnMove(e);
            this.Invalidate();
        }

        private void CleanUp()
        {
            if (backImageControlBitmap != null)
            {
                backImageControlBitmap.Dispose();
                backImageControlBitmap = null;
            }
            if (backImageBitmap != null)
            {
                backImageBitmap.Dispose();
                backImageBitmap = null;
            }
            if (backImageGraphics != null)
            {
                backImageGraphics.Dispose();
                backImageGraphics = null;
            }
            if (tabImageBitmap != null)
            {
                tabImageBitmap.Dispose();
                tabImageBitmap = null;
            }
            if (tabImageGraphics != null)
            {
                tabImageGraphics.Dispose();
                tabImageGraphics = null;
            }
        }

        #endregion

        #region Members
        private Bitmap backImageControlBitmap = null;
        private Bitmap backImageBitmap;
        private Graphics backImageGraphics;
        private Bitmap tabImageBitmap;
        private Graphics tabImageGraphics;
        private Rectangle tabcontrolborder;
        private FlatStyle flatstyle = FlatStyle.System;
        private TabStyleCustom tabstyle = new TabStyleSquare();    // change for the shape of tabs.
        private int mouseover = -1;                                 // where the mouse if hovering
        #endregion

    }
}

