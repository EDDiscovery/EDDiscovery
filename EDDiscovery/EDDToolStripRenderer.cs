﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EDDiscovery2
{
    public class EDDToolStripRenderer : ToolStripProfessionalRenderer//ToolStripSystemRenderer
    {
        protected ExtendedControls.CustomColourTable _colortable;

        public Color colMenuBackground { get { return _colortable.colMenuBackground; } set { _colortable.colMenuBackground = value; } }
        public Color colToolStripBorder { get { return _colortable.colToolStripBorder; } set { _colortable.colToolStripBorder = value; } }
        public Color colToolStripBackGround { get { return _colortable.colToolStripBackGround; } set { _colortable.colToolStripBackGround = value; } }
        public Color colToolStripSeparator { get { return _colortable.colToolStripSeparator; } set { _colortable.colToolStripSeparator = value; } }
        public Color colMenuSelectedBack { get { return _colortable.colMenuSelectedBack; } set { _colortable.colMenuSelectedBack = value; } }
        public Color colMenuSelectedText { get { return _colortable.colMenuSelectedText; } set { _colortable.colMenuSelectedText = value; } }
        public Color Dark { get { return _colortable.dark; } set { _colortable.dark = value; } }
        public Color colMenuText { get { return _colortable.colMenuText; } set { _colortable.colMenuText = value; } }
        public Color ButtonSelectedBorder { get { return _colortable.colButtonSelectedBorder; } set { _colortable.colButtonSelectedBorder = value; } }
        public Color ButtonSelectBackLight { get { return _colortable.colButtonSelectBackLight; } set { _colortable.colButtonSelectBackLight = value; } }
        public Color ButtonSelectBackDark { get { return _colortable.colButtonSelectBackDark; } set { _colortable.colButtonSelectBackDark = value; } }

        public EDDToolStripRenderer() : base(new ExtendedControls.CustomColourTable())
        {
            this._colortable = (ExtendedControls.CustomColourTable)ColorTable;
            this.RoundedEdges = true;
        }

#if false
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is MenuStrip)
            {
                using (SolidBrush br = new SolidBrush(colMenuBackground))
                    e.Graphics.FillRectangle(br, e.AffectedBounds);
            }
            else if (e.ToolStrip is StatusStrip)
            {
                e.Graphics.FillRectangle(new SolidBrush(colToolStripBackGround), e.AffectedBounds);
            }
            else if (e.ToolStrip is ToolStripDropDownMenu)
            {
                Rectangle rc = e.AffectedBounds;
                rc.Inflate(-1, -1);
                e.Graphics.FillRectangle(new SolidBrush(colMenuBackground), rc);
                if (e.ToolStrip is ToolStripDropDownMenu)
                    e.Graphics.FillRectangle(new SolidBrush(colMenuSelectedBack), 1, 1, 24, e.AffectedBounds.Height - 1);
            }
            else
            {
                Rectangle rc = e.AffectedBounds;
                rc.Inflate(-1, -1);
                e.Graphics.FillRectangle(new SolidBrush(colToolStripBackGround), rc);
                //if (e.ToolStrip is ToolStripDropDownMenu)
                //    e.Graphics.FillRectangle(new SolidBrush(Border), 1, 1, 24, e.AffectedBounds.Height - 1);
            }
        }
#endif


        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (!(e.ToolStrip is StatusStrip))
            {

                if (!(e.ToolStrip is MenuStrip))
                    e.Graphics.DrawRectangle(new Pen(colToolStripBorder), 0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
                if (!(e.ToolStrip is MenuStrip || e.ToolStrip.Parent is Form) && e.ToolStrip.Parent != null) //|| e.ToolStrip is StatusStrip
                {
                    e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    using (Bitmap bmp = new Bitmap(4, 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    {
                        using (Graphics g = Graphics.FromImage(bmp))
                            g.Clear(Color.Transparent);
                        //bmp.SetPixel(0, 0, e.ToolStrip.Parent.BackColor); //
                        bmp.SetPixel(1, 0, Color.FromArgb(175, colToolStripBorder));//e.ToolStrip.Parent.BackColor));
                        bmp.SetPixel(0, 1, Color.FromArgb(175, colToolStripBorder)); //e.ToolStrip.Parent.BackColor));
                        //bmp.SetPixel(2, 0, Color.FromArgb(75, e.ToolStrip.Parent.BackColor));
                        //bmp.SetPixel(0, 2, Color.FromArgb(75, e.ToolStrip.Parent.BackColor));
                        bmp.SetPixel(2, 1, Color.FromArgb(200, colToolStripBorder));
                        bmp.SetPixel(1, 2, Color.FromArgb(200, colToolStripBorder));
                        bmp.SetPixel(1, 1, Color.FromArgb(175, colToolStripBorder));
                        e.Graphics.DrawImage(bmp, 0, 0);
                        bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        e.Graphics.DrawImage(bmp, e.ToolStrip.Width - 4, 0);
                        bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        e.Graphics.DrawImage(bmp, e.ToolStrip.Width - 4, e.ToolStrip.Height - 4);
                        bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        e.Graphics.DrawImage(bmp, 0, e.ToolStrip.Height - 4);
                    }
                }
            }
        }

#if false
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Enabled)
            {
                if (e.Item.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, ButtonSelectBackDark)), 1, 1, e.Item.Width - 2, e.Item.Height - 2);
                }
                if (e.Item.Pressed)
                {
                    e.Graphics.FillRectangle(new SolidBrush(colMenuSelectedBack), 1, 1, e.Item.Width - 2, e.Item.Height - 1); //.ContentRectangle);
                    e.Graphics.DrawLine(new Pen(colMenuSelectedBack), 1, 0, e.Item.Width - 3, 0);
                }
            }
        }
#endif

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = colMenuText;

            if (e.ToolStrip is StatusStrip)
            {
                e.TextColor = colMenuText;
            }
            if (e.Item.Selected)
            {
                e.TextColor = colMenuSelectedText;
            }
            else if (e.Item.Pressed)
            {
                e.TextColor = colMenuSelectedText;
            }
            base.OnRenderItemText(e);
        }

#if false
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            if (e.Vertical)
            {

                e.Graphics.DrawLine(new Pen(colToolStripSeparator), 2, 0, 2, e.Item.ContentRectangle.Bottom);

                if (!(e.ToolStrip is StatusStrip))
                {
                    e.Graphics.DrawLine(new Pen(colToolStripSeparator), 3, 0, 3, e.Item.ContentRectangle.Bottom);
                }
            }
            else
            {

                e.Graphics.DrawLine(new Pen(colToolStripSeparator), 0, 2, e.Item.ContentRectangle.Right, 2);
                if (!(e.ToolStrip is StatusStrip))
                {
                    e.Graphics.DrawLine(new Pen(colToolStripSeparator), 0, 3, e.Item.ContentRectangle.Right, 3);
                }
            }
        }

        protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
        {
            //base.OnRenderItemBackground(e);
            e.Graphics.Clear(Dark);
        }
        protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
        {

        }

        protected override void OnRenderToolStripStatusLabelBackground(ToolStripItemRenderEventArgs e)
        {

        }

        protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
        {
            //base.OnRenderToolStripContentPanelBackground(e);
            e.Graphics.Clear(Dark);
        }

        
        protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
        {
            //base.OnRenderToolStripPanelBackground(e);
            e.Graphics.Clear(Dark);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected || e.Item.Pressed || (e.Item as ToolStripButton).Checked)
            {
                Point[] pta = new Point[] { new Point(2,1), new Point(e.Item.Width - 3,1),
                                            new Point(e.Item.Width - 2, 2), new Point(e.Item.Width - 2, e.Item.Height - 3),
                                            new Point(e.Item.Width - 3, e.Item.Height - 2), new Point(2, e.Item.Height - 2),
                                            new Point(1, e.Item.Height - 3), new Point(1, 2)};

                if (e.Item.Pressed)
                    e.Graphics.FillRectangle(new SolidBrush(ButtonSelectBackLight), e.Item.ContentRectangle);
                else if ((e.Item as ToolStripButton).Checked)
                {
                    using (Brush br = new LinearGradientBrush(e.Item.ContentRectangle, ButtonSelectBackLight, ButtonSelectBackDark, 90))
                        e.Graphics.FillRectangle(br, e.Item.ContentRectangle);
                    e.Graphics.DrawPolygon(new Pen(ButtonSelectedBorder), pta);
                }
                if(e.Item.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100,ButtonSelectedBorder)), e.Item.ContentRectangle);
                    e.Graphics.DrawPolygon(new Pen(Dark), pta);
                }
                
            }
        }

        
        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            Rectangle rc = e.ImageRectangle;
            rc.Offset(-2, 0);

            if (e.Item is ToolStripMenuItem)
            {
                //if (e.Item.Pressed)
                //    e.Graphics.FillRectangle(new SolidBrush(ButtonSelectBackLight), rc);
                //else
                {
                    if ((e.Item as ToolStripMenuItem).Checked)
                    {
                        using (Brush br = new LinearGradientBrush(rc, ButtonSelectBackLight, ButtonSelectBackDark, 90))
                            e.Graphics.FillRectangle(br, rc);
                        e.Graphics.DrawRectangle(new Pen(ButtonSelectedBorder), rc);
                    }
                }
            }
            

            if (e.Item.Image == null)
            {
                e.Graphics.DrawImage(EDDiscovery.Properties.Resources.imgCheck, rc); // e.Image, rc);
            }
            else
            {
                e.Graphics.DrawImage(e.Item.Image, rc);
            }
        }
#endif

        protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
        {
            if (!(e.ToolStrip is StatusStrip))
                base.OnRenderStatusStripSizingGrip(e);
            
            /*
            int kk = 5;
            Rectangle bnd = e.AffectedBounds;

            using (Brush br = new SolidBrush(colMenuSelectedBack))
            {
                for (int yy = 1; yy < 5; yy++)
                {
                    for (int xx = 1; xx < kk; xx++)
                    {
                        e.Graphics.FillRectangle(br, bnd.Right - 3 * xx, bnd.Bottom - 3 * yy, 2, 2);
                    }
                    kk--;
                }
            }
             */
            
            using (Pen p1 = new Pen(ColorTable.GripDark))
            {
                int rightpx = e.AffectedBounds.Right - 1;
                int bottompx = e.AffectedBounds.Bottom - 1;
                int msize = 5;
                int rightmarginpx = rightpx - msize;
                int bottommarginpx = bottompx - msize;

                for (int i = 0; i < 3; i++)
                {
                    e.Graphics.DrawLine(p1, new Point(rightmarginpx - i * msize, bottompx), new Point(rightpx, bottommarginpx - i * msize));
                }
            }
        }

#if false
        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rc = e.Item.ContentRectangle;
            rc.X = rc.Width - 4; rc.Width = 4;
            
            //rc = new Rectangle(0, 0, 16, 16);
            if (e.Item.Selected)
            {
                using (Brush br = new LinearGradientBrush(e.Item.ContentRectangle, ButtonSelectBackLight, ButtonSelectBackDark, 90))
                    e.Graphics.FillRectangle(br, e.Item.ContentRectangle);
                using (Pen pn = new Pen(ButtonSelectedBorder))
                {
                    e.Graphics.DrawRectangle(new Pen(ButtonSelectedBorder), e.Item.ContentRectangle);
                    e.Graphics.DrawLine(pn, rc.Left - 2, rc.Top, rc.Left - 2, rc.Bottom);
                }
                DrawArrow(new ToolStripArrowRenderEventArgs(e.Graphics, e.Item, rc, Color.Black, ArrowDirection.Down));
            }
            else if (e.Item.Pressed)
            {
                e.Graphics.FillRectangle(new SolidBrush(ButtonSelectBackLight), e.Item.ContentRectangle);
                using (Pen pn = new Pen(ButtonSelectedBorder))
                {
                    e.Graphics.DrawRectangle(pn, e.Item.ContentRectangle);
                    e.Graphics.DrawLine(pn, rc.Left - 2, rc.Top, rc.Left - 2, rc.Bottom);
                }
                DrawArrow(new ToolStripArrowRenderEventArgs(e.Graphics, e.Item, rc, Color.Black, ArrowDirection.Down));

            }
            else
                base.OnRenderSplitButtonBackground(e);
        }

        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rc = e.Item.ContentRectangle;
            rc.Width += 8;
            
            //rc = new Rectangle(0, 0, 16, 16);
            if (e.Item.Selected)
            {
                using (Brush br = new LinearGradientBrush(rc, ButtonSelectBackLight, ButtonSelectBackDark, 90))
                    e.Graphics.FillRectangle(br, rc);
                using (Pen pn = new Pen(ButtonSelectedBorder))
                {
                    e.Graphics.DrawRectangle(new Pen(ButtonSelectedBorder), rc);
                    //e.Graphics.DrawLine(pn, rc.Left - 2, rc.Top, rc.Left - 2, rc.Bottom);
                }
                rc.X = rc.Width - 4; rc.Width = 4;
                DrawArrow(new ToolStripArrowRenderEventArgs(e.Graphics, e.Item, rc, Color.Black, ArrowDirection.Down));
            }
            else if (e.Item.Pressed)
            {
                e.Graphics.FillRectangle(new SolidBrush(ButtonSelectBackLight), rc);
                using (Pen pn = new Pen(ButtonSelectedBorder))
                {
                    e.Graphics.DrawRectangle(pn, rc);
                    //e.Graphics.DrawLine(pn, rc.Left - 2, rc.Top, rc.Left - 2, rc.Bottom);
                }
                rc.X = rc.Width - 4; rc.Width = 4;
                DrawArrow(new ToolStripArrowRenderEventArgs(e.Graphics, e.Item, rc, Color.Black, ArrowDirection.Down));

            }
            //else
            //    base.OnRenderDropDownButtonBackground(e);
        }
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            base.OnRenderArrow(e);
        }
        
        protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
        {
            SmoothingMode sm = e.Graphics.SmoothingMode;
            e.Graphics.FillRectangle(new SolidBrush(colMenuSelectedBack), 3,0,e.Item.Width-3, e.Item.Height);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pn = new Pen(colMenuSelectedBack))
            {
                e.Graphics.DrawLine(pn, 1,  0, 2, 0);
                e.Graphics.DrawLine(pn, 2, 1, 2, 3);
                e.Graphics.DrawLine(pn, 1, e.Item.Height - 1, 2, e.Item.Height - 1);
                e.Graphics.DrawLine(pn, 2, e.Item.Height - 3, 2, e.Item.Height - 2);
            }
            Rectangle rc = new Rectangle(0,e.Item.Height/2, e.Item.Width, e.Item.Height/2);

            e.Graphics.SmoothingMode = sm;
            base.DrawArrow(new ToolStripArrowRenderEventArgs(e.Graphics, e.Item, rc, Color.Black, ArrowDirection.Down));
            e.Graphics.DrawLine(Pens.Black, rc.X + rc.Width / 2 - 2, rc.Y+2, rc.X + rc.Width / 2 + 2, rc.Y+2);           
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            
        }

        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            if (e.Item is ToolStripMenuItem)
            {
                OnRenderItemCheck(e);
                
            }
            else
                base.OnRenderItemImage(e);
        }
#endif
    }
}
