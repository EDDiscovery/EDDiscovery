using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace EDDiscovery2._2DMap
{

    public partial class ImageViewer : ScrollableControl
    {

        private static readonly int MinZoom = 20;
        private static readonly int MaxZoom = 400;

        private bool _autoCenter;
        private bool _autoPan;

        private System.Drawing.Image _image;
        private InterpolationMode _interpolationMode;
        private bool _isPanning;
        private bool _sizeToFit;
        private Point _startMousePosition;
        private Point _startScrollPosition;
        private TextureBrush _texture;
        private int _zoom;
        private int _zoomIncrement;

 
 
        public ImageViewer()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.StandardDoubleClick, false);
            this.UpdateStyles();

            this.BackColor = Color.White;
            this.AutoSize = true;
            this.AutoPan = true;
            this.Zoom = 100;
            this.ZoomIncrement = 20;
            this.InterpolationMode = InterpolationMode.Default;
            this.AutoCenter = true;
        }

 
        #region  Events  

        public event EventHandler AutoCenterChanged;
        public event EventHandler AutoPanChanged;
        public event EventHandler ImageChanged;
        public event EventHandler InterpolationModeChanged;
        public event EventHandler InvertMouseChanged;
        public event EventHandler PanEnd;
        public event EventHandler PanStart;
        public event EventHandler SizeToFitChanged;
        public event EventHandler ZoomChanged;
        public event EventHandler ZoomIncrementChanged;

        #endregion  Events  

        #region  Overriden Properties  

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set
            {
                if (base.AutoSize != value)
                {
                    base.AutoSize = value;
                    this.AdjustLayout();
                }
            }
        }

        [DefaultValue(typeof(Color), "White")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        #endregion  Overriden Properties  

        #region  Public Overridden Methods  

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size size;

            if (this.Image != null)
            {
                int width;
                int height;

                // get the size of the image
                width = this.ScaledImageWidth;
                height = this.ScaledImageHeight;

                // add an offset based on padding
                width += this.Padding.Horizontal;
                height += this.Padding.Vertical;

                size = new Size(width, height);
            }
            else
                size = base.GetPreferredSize(proposedSize);

            return size;
        }

        #endregion  Public Overridden Methods  

        #region  Protected Overridden Methods  

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();

                if (_texture != null)
                {
                    _texture.Dispose();
                    _texture = null;
                }

            }
            base.Dispose(disposing);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            bool result;

            if ((keyData & Keys.Right) == Keys.Right | (keyData & Keys.Left) == Keys.Left | (keyData & Keys.Up) == Keys.Up | (keyData & Keys.Down) == Keys.Down)
                result = true;
            else
                result = base.IsInputKey(keyData);

            return result;
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            this.Invalidate();
        }

        protected override void OnDockChanged(EventArgs e)
        {
            base.OnDockChanged(e);

            if (this.Dock != DockStyle.None)
                this.AutoSize = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Left:
                    this.AdjustScroll(-(e.Modifiers == Keys.None ? this.HorizontalScroll.SmallChange : this.HorizontalScroll.LargeChange), 0);
                    break;
                case Keys.Right:
                    this.AdjustScroll(e.Modifiers == Keys.None ? this.HorizontalScroll.SmallChange : this.HorizontalScroll.LargeChange, 0);
                    break;
                case Keys.Up:
                    this.AdjustScroll(0, -(e.Modifiers == Keys.None ? this.VerticalScroll.SmallChange : this.VerticalScroll.LargeChange));
                    break;
                case Keys.Down:
                    this.AdjustScroll(0, e.Modifiers == Keys.None ? this.VerticalScroll.SmallChange : this.VerticalScroll.LargeChange);
                    break;
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (!this.IsPanning && !this.SizeToFit)
            {
                if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.None)
                {
                    if (this.Zoom >= 100)
                        this.Zoom = (int)Math.Round((double)(this.Zoom + 100) / 100) * 100;
                    else if (this.Zoom >= 75)
                        this.Zoom = 100;
                    else
                        this.Zoom = (int)(this.Zoom / 0.75F);
                }
                else if (e.Button == MouseButtons.Right || (e.Button == MouseButtons.Left && Control.ModifierKeys != Keys.None))
                {
                    if (this.Zoom > 100 && this.Zoom <= 125)
                        this.Zoom = 100;
                    else if (this.Zoom > 100)
                        this.Zoom = (int)Math.Round((double)(this.Zoom - 100) / 100) * 100;
                    else
                        this.Zoom = (int)(this.Zoom * 0.75F);
                }
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!this.Focused)
                this.Focus();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left && this.AutoPan && this.Image != null)
            {
                if (!this.IsPanning)
                {
                    _startMousePosition = e.Location;
                    this.IsPanning = true;
                }

                if (this.IsPanning)
                {
                    int x;
                    int y;
                    Point position;

                    x = -_startScrollPosition.X + (_startMousePosition.X - e.Location.X);
                    y = -_startScrollPosition.Y + (_startMousePosition.Y - e.Location.Y);

                    position = new Point(x, y);

                    this.UpdateScrollPosition(position);
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (this.IsPanning)
                this.IsPanning = false;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!this.SizeToFit)
            {
                int increment;

                if (Control.ModifierKeys == Keys.None)
                    increment = this.ZoomIncrement;
                else
                    increment = this.ZoomIncrement * 5;

                if (e.Delta < 0)
                    increment = -increment;

                this.Zoom += increment;
            }
        }

        protected override void OnPaddingChanged(System.EventArgs e)
        {
            base.OnPaddingChanged(e);
            this.AdjustLayout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle innerRectangle;

            // draw the borders
            //ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, this.ForeColor, ButtonBorderStyle.Solid);

            innerRectangle = this.GetInsideViewPort();

            // draw the background
            using (SolidBrush brush = new SolidBrush(this.BackColor))
                e.Graphics.FillRectangle(brush, innerRectangle);


            // draw the image
            if (this.Image != null)
                this.DrawImage(e.Graphics);

            base.OnPaint(e);
        }

        protected override void OnParentChanged(System.EventArgs e)
        {
            base.OnParentChanged(e);
            this.AdjustLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            this.AdjustLayout();

            base.OnResize(e);
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            this.Invalidate();

            base.OnScroll(se);
        }

        #endregion  Protected Overridden Methods  

        #region  Public Methods  

        public virtual Rectangle GetImageViewPort()
        {
            Rectangle viewPort;

            if (this.Image != null)
            {
                Rectangle innerRectangle;
                Point offset;

                innerRectangle = this.GetInsideViewPort();

                if (this.AutoCenter)
                {
                    int x;
                    int y;

                    x = !this.HScroll ? (innerRectangle.Width - (this.ScaledImageWidth + this.Padding.Horizontal)) / 2 : 0;
                    y = !this.VScroll ? (innerRectangle.Height - (this.ScaledImageHeight + this.Padding.Vertical)) / 2 : 0;

                    offset = new Point(x, y);
                }
                else
                    offset = Point.Empty;

                viewPort = new Rectangle(offset.X + innerRectangle.Left + this.Padding.Left, offset.Y + innerRectangle.Top + this.Padding.Top, innerRectangle.Width - (this.Padding.Horizontal + (offset.X * 2)), innerRectangle.Height - (this.Padding.Vertical + (offset.Y * 2)));
            }
            else
                viewPort = Rectangle.Empty;

            return viewPort;
        }

        public Rectangle GetInsideViewPort()
        {
            return this.GetInsideViewPort(false);
        }

        public virtual Rectangle GetInsideViewPort(bool includePadding)
        {
            int left;
            int top;
            int width;
            int height;
   

   
            left = 0;
            top = 0;
            width = this.ClientSize.Width;
            height = this.ClientSize.Height;

            if (includePadding)
            {
                left += this.Padding.Left;
                top += this.Padding.Top;
                width -= this.Padding.Horizontal;
                height -= this.Padding.Vertical;
            }

            return new Rectangle(left, top, width, height);
        }

        public virtual Rectangle GetSourceImageRegion()
        {
            int sourceLeft;
            int sourceTop;
            int sourceWidth;
            int sourceHeight;
            Rectangle viewPort;
            Rectangle region;

            if (this.Image != null)
            {
                viewPort = this.GetImageViewPort();
                sourceLeft = (int)(-this.AutoScrollPosition.X / this.ZoomFactor);
                sourceTop = (int)(-this.AutoScrollPosition.Y / this.ZoomFactor);
                sourceWidth = (int)(viewPort.Width / this.ZoomFactor);
                sourceHeight = (int)(viewPort.Height / this.ZoomFactor);

                region = new Rectangle(sourceLeft, sourceTop, sourceWidth, sourceHeight);
            }
            else
                region = Rectangle.Empty;

            return region;
        }

        public virtual void ZoomToFit()
        {
            if (this.Image != null)
            {
                Rectangle innerRectangle;
                double zoom;
                double aspectRatio;

                this.AutoScrollMinSize = Size.Empty;

                innerRectangle = this.GetInsideViewPort(true);

                if (this.Image.Width > this.Image.Height)
                {
                    aspectRatio = ((double)innerRectangle.Width) / ((double)this.Image.Width);
                    zoom = aspectRatio * 100.0;

                    if (innerRectangle.Height < ((this.Image.Height * zoom) / 100.0))
                    {
                        aspectRatio = ((double)innerRectangle.Height) / ((double)this.Image.Height);
                        zoom = aspectRatio * 100.0;
                    }
                }
                else
                {
                    aspectRatio = ((double)innerRectangle.Height) / ((double)this.Image.Height);
                    zoom = aspectRatio * 100.0;

                    if (innerRectangle.Width < ((this.Image.Width * zoom) / 100.0))
                    {
                        aspectRatio = ((double)innerRectangle.Width) / ((double)this.Image.Width);
                        zoom = aspectRatio * 100.0;
                    }
                }

                this.Zoom = (int)Math.Round(Math.Floor(zoom));
            }
        }

        #endregion  Public Methods  



        [DefaultValue(true), Category("Appearance")]
        public bool AutoCenter
        {
            get { return _autoCenter; }
            set
            {
                if (_autoCenter != value)
                {
                    _autoCenter = value;
                    this.OnAutoCenterChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(true), Category("Behavior")]
        public bool AutoPan
        {
            get { return _autoPan; }
            set
            {
                if (_autoPan != value)
                {
                    _autoPan = value;
                    this.OnAutoPanChanged(EventArgs.Empty);

                    if (value)
                        this.SizeToFit = false;
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size AutoScrollMinSize
        {
            get { return base.AutoScrollMinSize; }
            set { base.AutoScrollMinSize = value; }
        }



        [Category("Appearance"), DefaultValue(null)]
        public virtual Image Image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    this.OnImageChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(InterpolationMode.Default), Category("Appearance")]
        public InterpolationMode InterpolationMode
        {
            get { return _interpolationMode; }
            set
            {
                if (value == InterpolationMode.Invalid)
                    value = InterpolationMode.Default;

                if (_interpolationMode != value)
                {
                    _interpolationMode = value;
                    this.OnInterpolationModeChanged(EventArgs.Empty);
                }
            }
        }


        [DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool IsPanning
        {
            get { return _isPanning; }
            protected set
            {
                if (_isPanning != value)
                {
                    _isPanning = value;
                    _startScrollPosition = this.AutoScrollPosition;

                    if (value)
                    {
                        this.Cursor = Cursors.SizeAll;
                        this.OnPanStart(EventArgs.Empty);
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                        this.OnPanEnd(EventArgs.Empty);
                    }
                }
            }
        }

        [DefaultValue(false), Category("Appearance")]
        public bool SizeToFit
        {
            get { return _sizeToFit; }
            set
            {
                if (_sizeToFit != value)
                {
                    _sizeToFit = value;
                    this.OnSizeToFitChanged(EventArgs.Empty);

                    if (value)
                        this.AutoPan = false;
                }
            }
        }

        [DefaultValue(100), Category("Appearance")]
        public int Zoom
        {
            get { return _zoom; }
            set
            {
                if (value < ImageViewer.MinZoom)
                    value = ImageViewer.MinZoom;
                else if (value > ImageViewer.MaxZoom)
                    value = ImageViewer.MaxZoom;

                if (_zoom != value)
                {
                    _zoom = value;
                    this.OnZoomChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(20), Category("Behavior")]
        public int ZoomIncrement
        {
            get { return _zoomIncrement; }
            set
            {
                if (_zoomIncrement != value)
                {
                    _zoomIncrement = value;
                    this.OnZoomIncrementChanged(EventArgs.Empty);
                }
            }
        }


        #region  Protected Properties  

        protected virtual int ScaledImageHeight
        { get { return this.Image != null ? (int)(this.Image.Size.Height * this.ZoomFactor) : 0; } }

        protected virtual int ScaledImageWidth
        { get { return this.Image != null ? (int)(this.Image.Size.Width * this.ZoomFactor) : 0; } }

        protected virtual double ZoomFactor
        { get { return (double)this.Zoom / 100; } }

        #endregion  Protected Properties  

        #region  Protected Methods  

        protected virtual void AdjustLayout()
        {
            if (this.AutoSize)
                this.AdjustSize();
            else if (this.SizeToFit)
                this.ZoomToFit();
            else if (this.AutoScroll)
                this.AdjustViewPort();
            this.Invalidate();
        }

        protected virtual void AdjustScroll(int x, int y)
        {
            Point scrollPosition;

            scrollPosition = new Point(this.HorizontalScroll.Value + x, this.VerticalScroll.Value + y);

            this.UpdateScrollPosition(scrollPosition);
        }

        protected virtual void AdjustSize()
        {
            if (this.AutoSize && this.Dock == DockStyle.None)
                base.Size = base.PreferredSize;
        }

        protected virtual void AdjustViewPort()
        {
            if (this.AutoScroll && this.Image != null)
                this.AutoScrollMinSize = new Size(this.ScaledImageWidth + this.Padding.Horizontal, this.ScaledImageHeight + this.Padding.Vertical);
        }


        protected virtual void DrawImage(Graphics g)
        {
            g.InterpolationMode = this.InterpolationMode;
            g.DrawImage(this.Image, this.GetImageViewPort(), this.GetSourceImageRegion(), GraphicsUnit.Pixel);
        }

        protected virtual void OnAutoCenterChanged(EventArgs e)
        {
            this.Invalidate();

            if (this.AutoCenterChanged != null)
                this.AutoCenterChanged(this, e);
        }

        protected virtual void OnAutoPanChanged(EventArgs e)
        {
            if (this.AutoPanChanged != null)
                this.AutoPanChanged(this, e);
        }

    

        protected virtual void OnImageChanged(EventArgs e)
        {
            this.AdjustLayout();

            if (this.ImageChanged != null)
                this.ImageChanged(this, e);
        }

        protected virtual void OnInterpolationModeChanged(EventArgs e)
        {
            this.Invalidate();

            if (this.InterpolationModeChanged != null)
                this.InterpolationModeChanged(this, e);
        }

        protected virtual void OnInvertMouseChanged(EventArgs e)
        {
            if (this.InvertMouseChanged != null)
                this.InvertMouseChanged(this, e);
        }

        protected virtual void OnPanEnd(EventArgs e)
        {
            if (this.PanEnd != null)
                this.PanEnd(this, e);
        }

        protected virtual void OnPanStart(EventArgs e)
        {
            if (this.PanStart != null)
                this.PanStart(this, e);
        }

        protected virtual void OnSizeToFitChanged(EventArgs e)
        {
            this.AdjustLayout();

            if (this.SizeToFitChanged != null)
                this.SizeToFitChanged(this, e);
        }

        protected virtual void OnZoomChanged(EventArgs e)
        {
            this.AdjustLayout();

            if (this.ZoomChanged != null)
                this.ZoomChanged(this, e);
        }

        protected virtual void OnZoomIncrementChanged(EventArgs e)
        {
            if (this.ZoomIncrementChanged != null)
                this.ZoomIncrementChanged(this, e);
        }

        protected virtual void UpdateScrollPosition(Point position)
        {
            this.AutoScrollPosition = position;
            this.Invalidate();
            this.OnScroll(new ScrollEventArgs(ScrollEventType.ThumbPosition, 0));
        }

        #endregion  Protected Methods  
    }
}

