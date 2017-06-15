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

/*Code parts from : http://www.codeproject.com/Articles/91387/Painting-Your-Own-Tabs-Second-Edition
* These parts are provided under the Code Project Open Licence (CPOL)
* See http://www.codeproject.com/info/cpol10.aspx for details
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class TabControlCustom : TabControl
    {
        #region Public interfaces

        #region ctors

        public TabControlCustom() { }

        #endregion // ctors

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

        public int MinimumTabWidth { set { SendMessage(Win32Constants.TCM.SETMINTABWIDTH, IntPtr.Zero, (IntPtr)value); } }
        public override Font Font { get { return base.Font; } set { ChangeFont(value); } }

        // Auto Invalidates
        // style is Flat, Popup (gradient) and System
        public FlatStyle FlatStyle { get { return flatstyle; } set { ChangeFlatStyle(value); } }

        // attach a tab style class which determines the shape and formatting.
        public TabStyleCustom TabStyle { get { return tabstyle; } set { ChangeTabStyle(value); } }

        #endregion // Properties

        #region Methods

        public int CalculateMinimumTabWidth()                                // given fonts and the tab text, whats the minimum width?
        {
            Graphics gr = Parent.CreateGraphics();

            int minsize = 0;

            foreach (TabPage p in TabPages)
            {
                //Console.WriteLine("Text is " + p.Text);
                SizeF sz = gr.MeasureString(p.Text, Font);

                if (sz.Width > minsize)
                    minsize = (int)sz.Width + 1;  // +1 due to float round down..
            }

            gr.Dispose();

            return minsize;
        }

        #endregion // Methods

        #endregion // Public interfaces


        #region Implementation

        #region Fields

        private TabStyleCustom tabstyle = new TabStyleSquare(); // ✓ change for the shape of tabs.

        private Bitmap backImageControlBitmap = null;           // ✓ (CleanUp)
        private Bitmap backImageBitmap;                         // ✓ (CleanUp)
        private Graphics backImageGraphics;                     // ✓ (CleanUp)
        private Bitmap tabImageBitmap;                          // ✓ (CleanUp)
        private Graphics tabImageGraphics;                      // ✓ (CleanUp)

        private Rectangle tabcontrolborder;
        private FlatStyle flatstyle = FlatStyle.System;
        private int mouseover = -1;                             // where the mouse is hovering

        #endregion // Fields

        #region OnEvent overrides

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
            base.OnMouseLeave(e);

            if (mouseover != -1)
            {
                mouseover = -1;

                if (flatstyle != FlatStyle.System)
                    Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!e.ClipRectangle.Equals(ClientRectangle))      // always repaint whole
            {
                Invalidate();
                return;
            }

            if (backImageControlBitmap == null)    // First time, we have size..
            {
                backImageControlBitmap = new Bitmap(Width, Height);
                Graphics backGraphics = Graphics.FromImage(backImageControlBitmap);
                PaintTransparentBackground(backGraphics, ClientRectangle);    // force the paint of the background into this bitmap.

                backImageBitmap = new Bitmap(Width, Height);
                backImageGraphics = Graphics.FromImage(backImageBitmap);

                tabImageBitmap = new Bitmap(Width, Height);
                tabImageGraphics = Graphics.FromImage(tabImageBitmap);

                tabcontrolborder = new Rectangle(0, DisplayRectangle.Y - 2, ClientRectangle.Width - 1, DisplayRectangle.Height + 4);
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
                backImageGraphics.DrawImage(tabImageBitmap,
                    new Rectangle(0, 0, tabImageBitmap.Width, tabImageBitmap.Height),
                    0, 0, tabImageBitmap.Width, tabImageBitmap.Height, GraphicsUnit.Pixel,
                                                   alphaAttributes);
            }

            backImageGraphics.Flush();

            e.Graphics.DrawImageUnscaled(backImageBitmap, 0, 0);
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
            if (Width > 0 && Height > 0)
            {
                CleanUp();                          // Ensures we start from scratch..
            }
            base.OnMove(e);
            Invalidate();
        }

        #endregion // OnEvent overrides

        #region Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CleanUp();
            }
            tabstyle = null;
            base.Dispose(disposing);
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

        private void ChangeTabStyle(TabStyleCustom fs)
        {
            tabstyle = fs;
            CleanUp();              // start afresh
            Invalidate();           // and repaint
        }

        private IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(Handle, msg, wparam, lparam);
            WndProc(ref message);
            return message.Result;
        }

        private void ChangeFlatStyle(FlatStyle fs)
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw, (fs != FlatStyle.System));

            // asking for a font set seems to make it set size better during start up
            SendMessage(Win32Constants.WM.SETFONT, Font.ToHfont(), (IntPtr)(-1));
            SendMessage(Win32Constants.WM.FONTCHANGE, IntPtr.Zero, IntPtr.Zero);

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

        private void DrawTab(int i, Graphics gr, bool selected, bool mouseover)
        {
            if (TabStyle == null)
                throw new ArgumentNullException("Custom style not attached");

            Color tabc1 = (Enabled) ? ((selected) ? TabSelectedColor : ((mouseover) ? TabMouseOverColor : TabNotSelectedColor)) : TabNotSelectedColor.Multiply(TabDisabledScaling);
            Color tabc2 = (FlatStyle == FlatStyle.Popup) ? tabc1.Multiply(TabColorScaling) : tabc1;
            Color taboutline = (selected) ? TabControlBorderColor : TabNotSelectedBorderColor;

            Image tabimage = null;

            TabStyle.DrawTab(gr, GetTabRect(i), i, selected, tabc1, tabc2, taboutline, Alignment);

            if (ImageList != null && TabPages[i].ImageIndex >= 0 && TabPages[i].ImageIndex < ImageList.Images.Count)
                tabimage = ImageList.Images[TabPages[i].ImageIndex];

            Color tabtextc = (Enabled) ? ((selected) ? TextSelectedColor : TextNotSelectedColor) : TextNotSelectedColor.Multiply(TabDisabledScaling);
            TabStyle.DrawText(gr, GetTabRect(i), i, selected, tabtextc, TabPages[i].Text, Font, tabimage);

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

        #endregion // Methods

        #endregion // Implementation
    }
}

