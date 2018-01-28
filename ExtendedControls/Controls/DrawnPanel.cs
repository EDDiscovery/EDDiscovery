/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using ExtendedControls.Controls.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtendedControls
{
    /// <summary>
    /// Represents a Windows <see cref="Button"/>-style <see cref="Control"/> that is drawn with enumerated graphics
    /// (see <see cref="ImageSelected"/>) or <see cref="Control.Text"/>.
    /// </summary>
    [DefaultEvent(nameof(Click)), DefaultProperty(nameof(ImageSelected)), Designer(typeof(DrawnPanelDesigner))]
    public class DrawnPanel : Control, IButtonControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawnPanel"/> class.
        /// </summary>
        /// <seealso cref="DrawnPanelNoTheme"/>
        public DrawnPanel() : base()
        {
            base.BackgroundImageLayout = ImageLayout.Zoom;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |        // "Free" double-buffering (1/3).
                ControlStyles.OptimizedDoubleBuffer |       // "Free" double-buffering (2/3).
                ControlStyles.ResizeRedraw |                // Invalidate after a resize or if the Padding changes.
                ControlStyles.Selectable |                  // We can receive focus from mouse-click or tab (see the Selectable prop).
                ControlStyles.SupportsTransparentBackColor |// BackColor.A can be less than 255.
                ControlStyles.UserPaint |                   // "Free" double-buffering (3/3); OnPaintBackground and OnPaint are needed.
                ControlStyles.UseTextForAccessibility,      // Use Text for the mnemonic char (and accessibility) if not empty, else the previous Label in the tab order.
                true);

            // We have to handle MouseClick-to-Click logic, otherwise Click fires for any mouse button. Double-click is fully ignored.
            SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, false);
        }


        // events
        /// <summary>
        /// This event is unsupported and should not be utilized.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler DoubleClick { add { base.DoubleClick += value; } remove { base.DoubleClick -= value; } }
        /// <summary>
        /// Occurs when the value of the <see cref="ImageSelected"/> property is changed.
        /// </summary>
        [Category("PropertyChanged"), Description("Occurs when the value of the ImageSelected property is changed.")]
        public event EventHandler ImageSelectedChanged;
        /// <summary>
        /// This event is unsupported and should not be utilized.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event MouseEventHandler MouseDoubleClick { add { base.MouseDoubleClick += value; } remove { base.MouseDoubleClick -= value; } }


        // properties
        /// <summary>
        /// Gets or sets a value indicating whether the ellipsis character (...) appears at the right edge of the
        /// control, denoting that the control text extends beyond the specified width of the control. This value is
        /// <c>false</c> by default.
        /// </summary>
        [Category("Behavior"), DefaultValue(false),
            Description("Enables the automatic handling of text that extends beyond the width of the DrawnPanel.")]
        public bool AutoEllipsis { get; set; } = false;
        /// <summary>
        /// This property is unsupported and should not be utilized. Use <see cref="Image"/> instead.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }
        /// <summary>
        /// Gets or sets the background <see cref="Image"/> layout displayed on the <see cref="DrawnPanel"/>. The
        /// default value is <see cref="ImageLayout.Zoom"/>.
        /// </summary>
        [DefaultValue(typeof(ImageLayout), nameof(ImageLayout.Zoom))]
        public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }
        /// <summary>
        /// Gets or sets a value representing the background image that this <see cref="DrawnPanel"/> will display. The
        /// default value is <c>null</c>.
        /// </summary>
        [Category("Appearance"), DefaultValue(null), Description("The background image displayed on the control.")]
        public Image Image
        {
            get { return _Image; }
            set
            {
                if (_Image != value)
                {
                    _Image = value;
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// Gets or sets a value indicating which <see cref="ImageType"/> this <see cref="DrawnPanel"/> will display.
        /// The default value is <see cref="ImageType.Close"/>.
        /// </summary>
        [Category("Appearance"), DefaultValue(ImageType.Close), Description("The foreground ImageType displayed on the control.")]
        public ImageType ImageSelected
        {
            get { return _ImageSelected; }
            set
            {
                if (_ImageSelected != value)
                {
                    _ImageSelected = value;
                    OnImageSelectedChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Gets or sets a value that represents the <see cref="Control.ForeColor"/> used when the mouse hovers this
        /// <see cref="DrawnPanel"/>. The default value is <see cref="Color.White"/>.
        /// </summary>
        /// <seealso cref="MouseSelectedColor"/>
        /// <seealso cref="MouseSelectedColorEnable"/>
        [Category("Appearance"), DefaultValue(typeof(Color), nameof(Color.White)),
            Description("The ForeColor used while the mouse cursor is hovering over the control.")]
        public Color MouseOverColor { get; set; } = Color.White;
        /// <summary>
        /// Gets or sets a value that represents the <see cref="Control.ForeColor"/> when the left mouse button is
        /// depressed on this <see cref="DrawnPanel"/>. The default value is <see cref="Color.Green"/>.
        /// </summary>
        /// <seealso cref="MouseOverColor"/>
        /// <seealso cref="MouseSelectedColorEnable"/>
        [Category("Appearance"), DefaultValue(typeof(Color), nameof(Color.Green)),
            Description("The ForeColor used while the left mouse button is clicking on the control")]
        public Color MouseSelectedColor { get; set; } = Color.Green;
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Control.ForeColor"/> will be updated in response to
        /// certain mouse events. The default value is <c>true</c>.
        /// </summary>
        /// <seealso cref="MouseOverColor"/>
        /// <seealso cref="MouseSelectedColor"/>
        [Category("Appearance"), DefaultValue(true),
            Description("Whether or not mouse events will trigger foreground color changes.")]
        public bool MouseSelectedColorEnable { get; set; } = true;
        /// <summary>
        /// Gets or sets a value that indicates how bright the <see cref="Control.ForeColor"/> will be when this
        /// <see cref="Control"/> is not <see cref="Control.Enabled"/>. The default value is <c>0.25</c>.
        /// </summary>
        [Category("Appearance"), DefaultValue(0.25F),
            Description("This multiplication factor will be used for the foreground color when the control is disabled.")]
        public float PanelDisabledScaling { get; set; } = 0.25F;
        /// <summary>
        /// Gets or sets a value that indicates whether or not the <see cref="DrawnPanel"/> can be focused. The default
        /// value is <c>true</c>. Set this to <c>false</c> for instances that are simulating caption controls, provided
        /// that a keyboard-accessible method is available to simulate the click action (alt-space system menu, etc).
        /// When set to <c>false</c>, the <see cref="Control.TabStop"/> property will also be set to <c>false</c>.
        /// </summary>
        [Category("Behavior"), DefaultValue(true), Description("Whether or not the control can receive focus."), RefreshProperties(RefreshProperties.All)]
        public bool Selectable
        {
            get { return _Selectable; }
            set
            {
                if (_Selectable != value)
                {
                    if (!value)
                        this.TabStop = false;
                    _Selectable = value;
                    SetStyle(ControlStyles.Selectable, value);
                }
            }
        }
        /// <summary>
        /// Gets or sets the alignment of the <see cref="Control.Text"/> displayed on the <see cref="DrawnPanel"/> when
        /// <see cref="ImageSelected"/> is set to one of the text options. The default value is
        /// <see cref="ContentAlignment.MiddleCenter"/>.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(ContentAlignment), nameof(ContentAlignment.MiddleCenter)),
            Description("The alignment of the text that will be displayed on the control when ImageSelected is set to one of the text options.")]
        public ContentAlignment TextAlign
        {
            get { return _TextAlign; }
            set
            {
                if (_TextAlign != value)
                {
                    _TextAlign = value;
                    if (_ImageSelected == ImageType.Text || _ImageSelected == ImageType.InverseText)
                        Invalidate();
                }
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the first character that is preceded by an ampersand (&amp;) is
        /// used as the mnemonic key of the control even if the <see cref="Control.Text"/> is not displayed. The
        /// default value is <c>true</c>.
        /// </summary>
        [Category("Appearance"), DefaultValue(true),
            Description("If true, the first character that is preceded by an ampersand (&) is used as the mnemonic key of the control, even if the Text is not displayed.")]
        public bool UseMnemonic { get; set; } = true;


        /// <summary>
        /// Specifies the available image types to be displayed on a <see cref="DrawnPanel"/>.
        /// </summary>
        /// <seealso cref="DrawnPanel.ImageSelected"/>
        public enum ImageType
        {
            /// <summary>Draws an 'X' on the <see cref="DrawnPanel"/>.</summary>
            [Description("Draws an 'X' on the control.")]
            Close,
            /// <summary>Draws an '_' on the <see cref="DrawnPanel"/>.</summary>
            [Description("Draws an '_' on the control.")]
            Minimize,
            /// <summary>Draws a filled square on the <see cref="DrawnPanel"/>.</summary>
            [Description("Draws a filled square on the control.")]
            OnTop,
            /// <summary>Draws a square outline on the <see cref="DrawnPanel"/>.</summary>
            [Description("Draws a square outline.")]
            Floating,
            /// <summary>Draws 3 thin parallel diagonal lines from the bottom edge to the right edge.</summary>
            [Description("Draws 3 thin parallel lines from the bottom edge to the right edge.")]
            Gripper,
            /// <summary>Draws an EDDB logo on the <see cref="DrawnPanel"/> with inverted colors.</summary>
            [Description("Draws an EDDB logo with inverted colors.")]
            EDDB,
            /// <summary>Draws an EDSM logo on the <see cref="DrawnPanel"/>.</summary>
            [Description("Draws an EDSM logo.")]
            EDSM,
            /// <summary>Draws a Ross logo on the <see cref="DrawnPanel"/> with inverted colors.</summary>
            [Description("Draws a Ross logo with inverted colors.")]
            Ross,
            /// <summary>Draws the current <see cref="Control.Text"/> on the <see cref="DrawnPanel"/> with inverted colors.</summary>
            [Description("Draws the control Text on the control with inverted colors.")]
            InverseText,
            /// <summary>Draws two perpendicular double-ended arrows that cross in the center.</summary>
            [Description("Draws two perpendicular double-ended arrows that cross in the center.")]
            Move,
            /// <summary>Draws the current <see cref="Control.Text"/> on the <see cref="DrawnPanel"/>.</summary>
            [Description("Draws the current control Text on the control.")]
            Text,
            /// <summary>Draws an empty <see cref="DrawnPanel"/>, honoring <see cref="Image"/>.</summary>
            [Description("Draws an empty control, honoring DrawnImage.")]
            None,
            /// <summary>Draws a 'T' on the <see cref="DrawnPanel"/>.</summary>
            [Description("Draws a 'T' on the control.")]
            NotTransparent,
            /// <summary>Draws a 'T' on the <see cref="DrawnPanel"/> with a square outline.</summary>
            [Description("Draws a 'T' on the control with a square outline.")]
            Transparent,
            /// <summary>Draws 'Tc' on the <see cref="DrawnPanel"/> with a square outline.</summary>
            [Description("Draws 'Tc' on the control with a square outline.")]
            TransparentClickThru,
            /// <summary>Draws 'Tf' on the <see cref="DrawnPanel"/> with a square outline.</summary>
            [Description("Draws 'Tf' on the control with a square outline.")]
            FullyTransparent,
            /// <summary>Draws a thin horizontal rectangle outline with two filled squares on the left.</summary>
            [Description("Draws a thin horizontal rectangle outline with two filled squares on the left.")]
            WindowInTaskBar,
            /// <summary>Draws a thin horizontal rectangle outline on the <see cref="DrawnPanel"/>.</summary>
            [Description("Draws a thin horizontal rectangle outline on the control.")]
            WindowNotInTaskBar,
            /// <summary>Draws a 'C' on the <see cref="DrawnPanel"/> with a square outline.</summary>
            [Description("Draws a C on the DrawnPanel with a square outline.")]
            Captioned,
            /// <summary>Draws a 'C' on the <see cref="DrawnPanel"/>.</summary>
            [Description("Draws a C on the DrawnPanel.")]
            NotCaptioned,
            /// <summary>Draws two thin horizontal bars along the top edge of the <see cref="DrawnPanel"/>.</summary>
            [Description("Draws two thin horizontal bars along the top edge of the control")]
            Bars,
            /// <summary>Draws a single simple window panel on the <see cref="DrawnPanel"/>.</summary><seealso cref="Restore"/><seealso cref="Minimize"/>
            [Description("Draws a single simple window panel on the control.")]
            Maximize,
            /// <summary>Draws two simple overlapping window panels on the <see cref="DrawnPanel"/>.</summary><seealso cref="Maximize"/><seealso cref="Minimize"/>
            [Description("Draws two simple overlapping window panels on the control.")]
            Restore
        };


        #region IButtonControl support

        /// <summary>
        /// Gets or sets a value that is returned to the parent <see cref="Form"/> when the <see cref="DrawnPanel"/> is
        /// clicked.
        /// </summary>
        [Category("Behavior"), DefaultValue(typeof(DialogResult), nameof(DialogResult.None)),
            Description("The dialog-box result produced in a form by clicking the DrawnPanel.")]
        public DialogResult DialogResult { get; set; } = DialogResult.None;

        /// <summary>
        /// Receive notifications when the <see cref="DrawnPanel"/> is the default button such that the appearance can
        /// be adjusted.
        /// </summary>
        /// <param name="value"><c>true</c> if the <see cref="DrawnPanel"/> is the default button; <c>false</c>
        /// otherwise.</param>
        public virtual void NotifyDefault(bool value)
        {
            // TODO: we're possibly the default "button" on a form. Paint boldly or something.
        }

        /// <summary>
        /// Generates a <see cref="Control.Click"/> event for the <see cref="DrawnPanel"/>.
        /// </summary>
        public virtual void PerformClick()
        {
            // CanSelect uses (ControlStyles.Selectable && Enabled && Visible) for control and all parents. Negate only the Selectable check when (!_Selectable).
            if ((_Selectable && this.CanSelect) || (!_Selectable && this.Enabled && this.Visible && Parent.CanSelect))
                OnClick(EventArgs.Empty);
        }

        #endregion


        public void SetDrawnBitmapRemapTable(ColorMap[] remap, float[][] colormatrix = null)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.Name ?? nameof(DrawnPanel));

            drawnImageAttributesDisabled?.Dispose();
            drawnImageAttributesDisabled = null;
            drawnImageAttributesEnabled?.Dispose();
            drawnImageAttributesEnabled = null;

            ControlHelpersStaticFunc.ComputeDrawnPanel(out drawnImageAttributesEnabled, out drawnImageAttributesDisabled, PanelDisabledScaling, remap, colormatrix);

            if (_Image != null)
                Invalidate();
        }


        #region Implementation

        private enum DrawState { Disabled = -1, Normal = 0, Hover, Click };

        private ImageAttributes drawnImageAttributesDisabled = null;        // Image override (colour etc) while !Enabled.
        private ImageAttributes drawnImageAttributesEnabled = null;         // Image override (colour etc) while Enabled.
        private DrawState drawState = DrawState.Normal;                     // The current state of our control.

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]                   // Visible via Image
        private Image _Image = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]                   // Visible via ImageSelected
        private ImageType _ImageSelected = ImageType.Close;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]                   // Visible via Selectable
        private bool _Selectable = true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]                   // Visible via TextAlign
        private ContentAlignment _TextAlign = ContentAlignment.MiddleCenter;


        /// <summary>
        /// Gets the internal spacing, in pixels, of the contents of a control. By default, this is 4 on every side.
        /// </summary>
        protected override Padding DefaultPadding { get { return new Padding(4); } }

        /// <summary>
        /// Gets the default size of the control. By default, this is 24x24.
        /// </summary>
        protected override Size DefaultSize { get { return new Size(24, 24); } }



        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DrawnPanel"/> and optionally releases the managed
        /// resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to
        /// release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // Do not dispose of _Image, as it *should* be a managed resource.
            if (disposing)
            {
                drawnImageAttributesDisabled?.Dispose();
                drawnImageAttributesEnabled?.Dispose();
            }
            ImageSelectedChanged = null;

            _Image = null;
            drawnImageAttributesDisabled = null;
            drawnImageAttributesEnabled = null;

            base.Dispose(disposing);
        }


        /// <summary>
        /// Raises the <see cref="Control.Click"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> containing the event data.</param>
        protected override void OnClick(EventArgs e)
        {
            Form f = this.FindForm();
            if (f != null)
                f.DialogResult = this.DialogResult;

            base.OnClick(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.EnabledChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> containing the event data.</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            if (!Enabled)
                SetDrawState(DrawState.Disabled);
            else
                SetDrawState(DrawState.Normal);
        }

        /// <summary>
        /// Raises the <see cref="Control.GotFocus"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> containing the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();   // Invalidate to update the focus rectangle.
        }

        /// <summary>
        /// Raises the <see cref="ImageSelectedChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> containing the event data.</param>
        protected virtual void OnImageSelectedChanged(EventArgs e)
        {
            ImageSelectedChanged?.Invoke(this, e);
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="KeyEventArgs"/> containing the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                SetDrawState(DrawState.Click);
                e.Handled = true;
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="KeyEventArgs"/> containing the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                OnClick(EventArgs.Empty);
                SetDrawState(DrawState.Normal);
            }
            e.Handled = true;

            base.OnKeyUp(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.LostFocus"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> containing the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();   // Invalidate to update the focus rectangle.
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseClick"/> event.
        /// </summary>
        /// <param name="e">A <see cref="MouseEventArgs"/> containing the event data.</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !GetStyle(ControlStyles.StandardClick))
                OnClick(e);

            base.OnMouseClick(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="mevent">A <see cref="MouseEventArgs"/> containing the event data.</param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            //System.Diagnostics.Debug.WriteLine("DP MD");
            if (mevent.Button == MouseButtons.Left)
                SetDrawState(DrawState.Click);

            base.OnMouseDown(mevent);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> containing the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            // If Disabled, this method doesn't get called, so we're safe to always switch to Normal.
            // Also, if a mouse button is depressed when the mouse leaves, this doesn't get called until the button gets released.
            SetDrawState(DrawState.Normal);

            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseMove"/> event.
        /// </summary>
        /// <param name="mevent">A <see cref="MouseEventArgs"/> containing the event data.</param>
        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            // Allow a very long click to come and go from our bounds, updating the draw state the whole time.
            if (ClientRectangle.Contains(mevent.Location))
                SetDrawState(mevent.Button == MouseButtons.Left ? DrawState.Click : DrawState.Hover);
            else
                SetDrawState(DrawState.Normal);       // OnMouseLeave doesn't actually fire until /after/ LMB is released, so clear this here.

            base.OnMouseMove(mevent);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseUp"/> event.
        /// </summary>
        /// <param name="mevent">A <see cref="MouseEventArgs"/> containing the event data.</param>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            //System.Diagnostics.Debug.WriteLine("DP MU");
            if (drawState == DrawState.Click)
            {
                OnMouseClick(mevent);
                SetDrawState(DrawState.Hover);
            }   

            base.OnMouseUp(mevent);
        }

        /// <summary>
        /// Raises the <see cref="Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //Debug.WriteLine($"{nameof(DrawnPanel)}.{nameof(OnPaint)} ({this.Name ?? "unnamed"}): Enabled {Enabled}, State {_DrawState}");

            Color cFore = this.ForeColor;
            switch (drawState)
            {
                case DrawState.Disabled:
                    cFore = this.ForeColor.Average(this.BackColor, PanelDisabledScaling);
                    break;
                case DrawState.Hover:
                    if (MouseSelectedColorEnable)
                        cFore = this.MouseOverColor;
                    break;
                case DrawState.Click:
                    if (MouseSelectedColorEnable)
                        cFore = this.MouseSelectedColor;
                    break;
            }

            if (_ImageSelected != ImageType.None)
            {
                var rcClip = new Rectangle(this.ClientRectangle.Left + this.Padding.Left, this.ClientRectangle.Top + this.Padding.Top,
                    this.ClientRectangle.Width - this.Padding.Horizontal, this.ClientRectangle.Height - this.Padding.Vertical);
                int shortestDim = Math.Min(rcClip.Width, rcClip.Height);
                var sqClip = new Rectangle(rcClip.Left + (int)Math.Round((float)(rcClip.Width - shortestDim) / 2),  // Largest square that fits entirely inside rcClip, centered.
                    rcClip.Top + (int)Math.Round((float)(rcClip.Height - shortestDim) / 2), shortestDim, shortestDim);
                int centrehorzpx = (this.ClientRectangle.Width - 1) / 2;
                int centrevertpx = (this.ClientRectangle.Height - 1) / 2;

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                switch (_ImageSelected)
                {
                    case ImageType.Close:
                        {
                            // Draw a centered 'X'
                            using (var p2 = new Pen(cFore, 2.0F))
                            {
                                e.Graphics.DrawLine(p2, new Point(sqClip.Left, sqClip.Top), new Point(sqClip.Right, sqClip.Bottom));
                                e.Graphics.DrawLine(p2, new Point(sqClip.Left, sqClip.Bottom), new Point(sqClip.Right, sqClip.Top));
                            }
                            break;
                        }
                    case ImageType.Minimize:
                        {
                            // Draw an '_'
                            using (var p2 = new Pen(cFore, 2.0F))
                                e.Graphics.DrawLine(p2, new Point(rcClip.Left, rcClip.Bottom), new Point(rcClip.Right, rcClip.Bottom));
                            break;
                        }
                    case ImageType.OnTop:
                        {
                            // Draw a filled square
                            using (Brush bFore = new SolidBrush(cFore))
                                e.Graphics.FillRectangle(bFore, sqClip);
                            break;
                        }
                    case ImageType.Floating:
                        {
                            // Draw an outlined square
                            using (var p2 = new Pen(cFore, 2.0F))
                                e.Graphics.DrawRectangle(p2, sqClip);
                            break;
                        }
                    case ImageType.TransparentClickThru:
                        {
                            // Draw a square outline, with 'Tc' drawn inside
                            using (var p2 = new Pen(cFore, 2.0F))
                            {
                                e.Graphics.DrawRectangle(p2, sqClip);

                                e.Graphics.DrawLine(p2, new Point(sqClip.Left + 2, sqClip.Top + 2), new Point(sqClip.Right - 2, sqClip.Top + 2));
                                e.Graphics.DrawLine(p2, new Point(centrehorzpx, sqClip.Top + 2), new Point(centrehorzpx, sqClip.Bottom - 2));

                                // TODO: This sucks at a large size
                                e.Graphics.DrawLine(p2, new Point(centrehorzpx + 2, sqClip.Top + 6), new Point(centrehorzpx + 6, sqClip.Top + 6));
                                e.Graphics.DrawLine(p2, new Point(centrehorzpx + 2, sqClip.Top + 6), new Point(centrehorzpx + 2, sqClip.Top + 10));
                                e.Graphics.DrawLine(p2, new Point(centrehorzpx + 2, sqClip.Top + 10), new Point(centrehorzpx + 6, sqClip.Top + 10));
                            }
                            break;
                        }
                    case ImageType.FullyTransparent:
                        {
                            // Draw a square outline, with 'Tf' drawn inside
                            using (var p2 = new Pen(cFore, 2.0F))
                            {
                                e.Graphics.DrawRectangle(p2, sqClip);

                                e.Graphics.DrawLine(p2, new Point(sqClip.Left + 2, sqClip.Top + 2), new Point(sqClip.Right - 2, sqClip.Top + 2));
                                e.Graphics.DrawLine(p2, new Point(centrehorzpx, sqClip.Top + 2), new Point(centrehorzpx, sqClip.Bottom - 2));

                                // TODO: This sucks at a large size
                                e.Graphics.DrawLine(p2, new Point(centrehorzpx + 2, sqClip.Top + 6), new Point(centrehorzpx + 6, sqClip.Top + 6));
                                e.Graphics.DrawLine(p2, new Point(centrehorzpx + 2, sqClip.Top + 6), new Point(centrehorzpx + 2, sqClip.Top + 11));
                                e.Graphics.DrawLine(p2, new Point(centrehorzpx + 2, sqClip.Top + 8), new Point(centrehorzpx + 6, sqClip.Top + 8));
                            }
                            break;
                        }
                    case ImageType.NotTransparent:
                    case ImageType.Transparent:
                        {
                            // Draw a 'T', with an optional square outline
                            using (var p2 = new Pen(cFore, 2.0F))
                            {
                                if (_ImageSelected == ImageType.Transparent)
                                    e.Graphics.DrawRectangle(p2, sqClip);

                                e.Graphics.DrawLine(p2, new Point(sqClip.Left + 2, sqClip.Top + 3), new Point(sqClip.Right - 2, sqClip.Top + 3));
                                e.Graphics.DrawLine(p2, new Point(centrehorzpx, sqClip.Top + 3), new Point(centrehorzpx, sqClip.Bottom - 3));
                            }
                            break;
                        }
                    case ImageType.NotCaptioned:
                    case ImageType.Captioned:
                        {
                            // Draw a 'C', with an optional square outline
                            using (var p2 = new Pen(cFore, 2.0F))
                            {
                                int off = Math.Max(2, (int)(sqClip.Width * 0.1));
                                if (_ImageSelected == ImageType.Captioned)
                                    e.Graphics.DrawRectangle(p2, sqClip);

                                e.Graphics.DrawLine(p2, new Point(sqClip.Left + off, sqClip.Top + off), new Point(sqClip.Right - off, sqClip.Top + off));
                                e.Graphics.DrawLine(p2, new Point(sqClip.Left + off, sqClip.Bottom - off), new Point(sqClip.Right - off, sqClip.Bottom - off));
                                e.Graphics.DrawLine(p2, new Point(sqClip.Left + off, sqClip.Top + off), new Point(sqClip.Left + off, sqClip.Bottom - off));
                            }
                            break;
                        }
                    case ImageType.Gripper:
                        {
                            // Draw 3 thin parallel diagonal lines from the bottom edge to the right edge
                            int mDim = (int)Math.Round(shortestDim / 6f);

                            using (var p1 = new Pen(cFore, 1.0F))
                            {
                                for (int i = 0; i < 3; i++)
                                    e.Graphics.DrawLine(p1, new Point(rcClip.Right - i * mDim, rcClip.Bottom), new Point(rcClip.Right, rcClip.Bottom - i * mDim));
                            }
                            break;
                        }
                    case ImageType.EDDB:
                        {
                            // Draw an inverted arrow extending from the middle/bottom-right to the middle/bottom-center, then "curved" up to end at the middle/top-center
                            int mDim = (int)Math.Round(shortestDim / 6f);
                            var btmRight = new Point(rcClip.Right, rcClip.Bottom - mDim);
                            var btmCenter = new Point(centrehorzpx - 1, rcClip.Bottom - mDim);
                            var topCenter = new Point(centrehorzpx - 1, rcClip.Top + mDim);
                            var topLeftCenter = new Point(centrehorzpx - 1 - mDim, topCenter.Y + mDim);
                            var topRightCenter = new Point(centrehorzpx - 1 + mDim, topCenter.Y + mDim);

                            cFore = this.BackColor;     // INVERTED

                            using (Pen pb = new Pen(cFore, 2.0F))
                            {
                                e.Graphics.DrawLine(pb, btmRight, btmCenter);
                                e.Graphics.DrawLine(pb, btmCenter, topCenter);
                                e.Graphics.DrawLine(pb, topCenter, topLeftCenter);
                                e.Graphics.DrawLine(pb, topCenter, topRightCenter);
                            }
                            break;
                        }
                    case ImageType.EDSM:
                        {
                            // TODO: someone want to give me a resource to use? It seems the only places that use this all set and use Text appropriately.
                            break;
                        }
                    case ImageType.Ross:
                        {
                            // Draw an inverted thick '┌'-style symbol
                            cFore = this.BackColor;     // INVERTED
                            int mDim = (int)Math.Round(shortestDim / 6f);
                            using (Pen pb = new Pen(cFore, 3.0F))
                            {
                                e.Graphics.DrawLine(pb, new Point(rcClip.Left + mDim, rcClip.Bottom), new Point(rcClip.Left + mDim, rcClip.Top + 4));
                                e.Graphics.DrawLine(pb, new Point(rcClip.Left + mDim, rcClip.Top + 4), new Point(centrehorzpx + 2, rcClip.Top + 4));
                            }
                            break;
                        }
                    case ImageType.InverseText:
                        cFore = this.BackColor;     // INVERTED
                        goto case ImageType.Text;   // FALL THROUGH
                    case ImageType.Text:
                        {
                            // Draw Text, potentially inverted
                            if (!string.IsNullOrWhiteSpace(this.Text))
                            {
                                using (var fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(RtlTranslateAlignment(TextAlign)))
                                using (var textb = new SolidBrush(cFore))
                                {
                                    if (this.UseMnemonic)
                                        fmt.HotkeyPrefix = this.ShowKeyboardCues ? HotkeyPrefix.Show : HotkeyPrefix.Hide;
                                    if (this.AutoEllipsis)
                                        fmt.Trimming = StringTrimming.EllipsisCharacter;
                                    e.Graphics.DrawString(this.Text, this.Font, textb, this.ClientRectangle, fmt);
                                }
                            }
                            break;
                        }
                    case ImageType.Move:
                        {
                            // Draw two perpendicular (1 vertical, 1 horizontal) double-ended arrows that cross in the center
                            int mDim = (int)Math.Round(shortestDim / 8f);
                            var btmCenter = new Point(centrehorzpx + 1, sqClip.Bottom);
                            var topCenter = new Point(centrehorzpx + 1, sqClip.Top);
                            var lftCenter = new Point(sqClip.Left, centrevertpx + 1);
                            var rgtCenter = new Point(sqClip.Right, centrevertpx + 1);

                            using (var p1 = new Pen(cFore, 1.0F))
                            using (var p2 = new Pen(cFore, 2.0F))
                            {
                                e.Graphics.DrawLine(p2, btmCenter, topCenter);
                                e.Graphics.DrawLine(p2, lftCenter, rgtCenter);

                                e.Graphics.DrawLine(p1, btmCenter, new Point(centrehorzpx - mDim, sqClip.Bottom - mDim));
                                e.Graphics.DrawLine(p1, btmCenter, new Point(centrehorzpx + mDim, sqClip.Bottom - mDim));
                                e.Graphics.DrawLine(p1, topCenter, new Point(centrehorzpx - mDim, sqClip.Top + mDim));
                                e.Graphics.DrawLine(p1, topCenter, new Point(centrehorzpx + mDim, sqClip.Top + mDim));

                                e.Graphics.DrawLine(p1, lftCenter, new Point(sqClip.Left + mDim, centrevertpx - mDim));
                                e.Graphics.DrawLine(p1, lftCenter, new Point(sqClip.Left + mDim, centrevertpx + mDim));
                                e.Graphics.DrawLine(p1, rgtCenter, new Point(sqClip.Right - mDim, centrevertpx - mDim));
                                e.Graphics.DrawLine(p1, rgtCenter, new Point(sqClip.Right - mDim, centrevertpx + mDim));
                            }
                            break;
                        }
                    case ImageType.WindowNotInTaskBar:
                    case ImageType.WindowInTaskBar:
                        {
                            // Draw a thin horizontal rectangle outline, possibly with two small filled squares in the left
                            int mDim = (int)(((float)shortestDim / 3f) + 1);
                            int top = centrevertpx - mDim / 2;

                            using (var p1 = new Pen(cFore, 1.0F))
                                e.Graphics.DrawRectangle(p1, new Rectangle(rcClip.Left, top, rcClip.Width, mDim));

                            if (_ImageSelected == ImageType.WindowInTaskBar)
                            {
                                var off = (mDim <= 8) ? 1 : 3;      // whether or not a space exists between top and rcSq.
                                var rcSq = new Rectangle(rcClip.Left + off, top + off, 1 + mDim - (off * 2), 1 + mDim - (off * 2));

                                using (Brush bbck = new SolidBrush(cFore))
                                {
                                    e.Graphics.FillRectangle(bbck, rcSq);
                                    rcSq.Offset(mDim + off, 0);
                                    e.Graphics.FillRectangle(bbck, rcSq);
                                }
                            }
                            break;
                        }
                    case ImageType.Bars:
                        {
                            // Draw two thin horizontal bars along the top edge
                            using (var p1 = new Pen(cFore, 1.0F))
                            {
                                e.Graphics.DrawLine(p1, new Point(rcClip.Left, rcClip.Top), new Point(rcClip.Right, rcClip.Top));
                                e.Graphics.DrawLine(p1, new Point(rcClip.Left, rcClip.Top + 2), new Point(rcClip.Right, rcClip.Top + 2));
                            }
                            break;
                        }
                    case ImageType.Maximize:
                        {
                            // Draw a thin square outline with a slightly thicker top edge
                            using (var p1 = new Pen(cFore, 1.0F))
                                e.Graphics.DrawRectangle(p1, sqClip);
                            using (var p2 = new Pen(cFore, 2.0F))
                                e.Graphics.DrawLine(p2, new Point(sqClip.Left, sqClip.Top + 1), new Point(sqClip.Right, sqClip.Top + 1));
                            break;
                        }
                    case ImageType.Restore:
                        {
                            // Draw two overlapping, but identically-sized with an offset, thin square outlines each with slightly thicker top edges
                            int iDim = (int)Math.Round((float)shortestDim * 2 / 3);    // Length of a "window" edge

                            using (var p1 = new Pen(cFore, 1.0F))
                            using (var p2 = new Pen(cFore, 2.0F))
                            {
                                // lower-left foreground "window", clockwise from top-left
                                e.Graphics.DrawLine(p2, new Point(sqClip.Left, sqClip.Bottom - iDim), new Point(sqClip.Left + iDim, sqClip.Bottom - iDim));
                                e.Graphics.DrawLine(p1, new Point(sqClip.Left + iDim, sqClip.Bottom - iDim), new Point(sqClip.Left + iDim, sqClip.Bottom));
                                e.Graphics.DrawLine(p1, new Point(sqClip.Left + iDim, sqClip.Bottom), new Point(sqClip.Left, sqClip.Bottom));
                                e.Graphics.DrawLine(p1, new Point(sqClip.Left, sqClip.Bottom), new Point(sqClip.Left, sqClip.Bottom - iDim));

                                // upper-right background "window" clockwise from (obscured!) bottom-left
                                e.Graphics.DrawLine(p1, new Point(sqClip.Right - iDim, sqClip.Bottom - iDim), new Point(sqClip.Right - iDim, sqClip.Top));
                                e.Graphics.DrawLine(p2, new Point(sqClip.Right - iDim, sqClip.Top), new Point(sqClip.Right, sqClip.Top));
                                e.Graphics.DrawLine(p1, new Point(sqClip.Right, sqClip.Top), new Point(sqClip.Right, sqClip.Top + iDim));
                                e.Graphics.DrawLine(p1, new Point(sqClip.Right, sqClip.Top + iDim), new Point(sqClip.Left + iDim, sqClip.Top + iDim));
                            }
                            break;
                        }

                    default:
                        throw new NotImplementedException($"ImageType ({_ImageSelected}) painting is apparantly not implemented; please add support for it.");
                }

                e.Graphics.SmoothingMode = SmoothingMode.Default;
            }

            // Draw a focus rectangle. CanFocus: (IsHandleCreated && Visible && Enabled)
            if (this.CanFocus && this.Focused && this.ShowFocusCues)
            {
                using (var p = new Pen(cFore))
                {
                    var rcFocus = new Rectangle(new Point(1, 1), new Size(this.ClientSize.Width - 3, this.ClientSize.Height - 3));
                    e.Graphics.DrawRectangle(p, rcFocus);   // Draw a rectangle outline 1px smaller than ClientSize ...

                    rcFocus.Inflate(-1, -1);
                    p.DashStyle = DashStyle.Dash;
                    p.DashPattern = new[] { 1f, 1f };
                    e.Graphics.DrawRectangle(p, rcFocus);   // Then draw a dashed rectangle outline 1px smaller than that.
                }
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// Paints the background of the <see cref="DrawnPanel"/>.
        /// </summary>
        /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            switch (_ImageSelected)
            {
                // Manually paint the background for types with inverted colors. Hope that you didn't want BackgroundImage.
                case ImageType.EDDB:
                case ImageType.InverseText:
                case ImageType.Ross:
                    {
                        // This colour is used for the background of the inverted image types.
                        Color cFore = this.ForeColor;
                        switch (drawState)
                        {
                            case DrawState.Disabled:
                                cFore = cFore.Average(this.BackColor, PanelDisabledScaling);
                                break;
                            case DrawState.Hover:
                                if (MouseSelectedColorEnable)
                                    cFore = this.MouseOverColor;
                                break;
                            case DrawState.Click:
                                if (MouseSelectedColorEnable)
                                    cFore = this.MouseSelectedColor;
                                break;
                        }

                        using (var b = new SolidBrush(cFore))
                            e.Graphics.FillRectangle(b, this.ClientRectangle);
                        break;
                    }

                // Otherwise, base can handle it.
                default:
                    {
                        base.OnPaintBackground(e);
                        break;
                    }
            }

            if (_Image != null)
            {
                ImageAttributes iattrib = this.Enabled ? drawnImageAttributesEnabled : drawnImageAttributesDisabled;

                switch (base.BackgroundImageLayout)
                {
                    case ImageLayout.None:      // Image {0,0} = ClientRectangle {0,0} with no scaling
                        {
                            // TODO: honour RightToLeft; Image {1,0} = ClientRect {1,0}
                            var dstRc = this.ClientRectangle;
                            dstRc.Intersect(new Rectangle(Point.Empty, _Image.Size));
                            if (iattrib != null)
                                e.Graphics.DrawImage(_Image, dstRc, 0, 0, dstRc.Width, dstRc.Height, GraphicsUnit.Pixel, iattrib);
                            else
                                e.Graphics.DrawImage(_Image, dstRc, 0, 0, dstRc.Width, dstRc.Height, GraphicsUnit.Pixel);
                            break;
                        }
                    case ImageLayout.Tile:      // Image {0,0} = ClientRectangle {0,0} with no scaling, and repeated to fill
                        {
                            // TODO: Honour iattrib here? Doesn't seem fully possible?
                            using (var tb = new TextureBrush(_Image, WrapMode.Tile))
                                e.Graphics.FillRectangle(tb, this.ClientRectangle);
                            break;
                        }
                    case ImageLayout.Center:    // Image center = ClientRectangle center with no scaling
                        {
                            var imgRc = new Rectangle(Point.Empty, _Image.Size);
                            var dstRc = this.ClientRectangle;

                            if (_Image.Width > this.ClientSize.Width)
                            {
                                imgRc.X = (_Image.Width - this.ClientSize.Width) / 2;
                                imgRc.Width = dstRc.Width;
                            }
                            else
                            {
                                dstRc.X = (this.ClientSize.Width - _Image.Width) / 2;
                                dstRc.Width = imgRc.Width;
                            }
                            if (_Image.Height > this.ClientSize.Height)
                            {
                                imgRc.Y = (_Image.Height - this.ClientSize.Height) / 2;
                                imgRc.Height = dstRc.Height;
                            }
                            else
                            {
                                dstRc.Y = (this.ClientSize.Height - _Image.Height) / 2;
                                dstRc.Height = imgRc.Height;
                            }
                            if (iattrib != null)
                                e.Graphics.DrawImage(_Image, dstRc, imgRc.X, imgRc.Y, imgRc.Width, imgRc.Height, GraphicsUnit.Pixel, iattrib);
                            else
                                e.Graphics.DrawImage(_Image, dstRc, imgRc.X, imgRc.Y, imgRc.Width, imgRc.Height, GraphicsUnit.Pixel);
                            break;
                        }
                    case ImageLayout.Stretch:   // Pin the image corners to our corners without any concerns for aspect ratio
                        {
                            if (iattrib != null)
                                e.Graphics.DrawImage(_Image, this.ClientRectangle, 0, 0, _Image.Width, _Image.Height, GraphicsUnit.Pixel, iattrib);
                            else
                                e.Graphics.DrawImage(_Image, this.ClientRectangle, 0, 0, _Image.Width, _Image.Height, GraphicsUnit.Pixel);
                            break;
                        }
                    case ImageLayout.Zoom:      // Like Stretch, but centered and mindful of the image aspect ratio
                    default:
                        {
                            var dstRc = this.ClientRectangle;
                            var wRatio = (float)dstRc.Width / (float)_Image.Width;
                            var hRatio = (float)dstRc.Height / (float)_Image.Height;
                            if (wRatio < hRatio)
                            {
                                dstRc.Height = (int)((_Image.Height * wRatio) + 0.5);
                                dstRc.Y = (this.ClientSize.Height - dstRc.Height) / 2;
                            }
                            else
                            {
                                dstRc.Width = (int)((_Image.Width * hRatio) + 0.5);
                                dstRc.X = (this.ClientSize.Width - dstRc.Width) / 2;
                            }

                            if (iattrib != null)
                                e.Graphics.DrawImage(_Image, dstRc, 0, 0, _Image.Width, _Image.Height, GraphicsUnit.Pixel, iattrib);
                            else
                                e.Graphics.DrawImage(_Image, dstRc, 0, 0, _Image.Width, _Image.Height, GraphicsUnit.Pixel);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.TextChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (_ImageSelected == ImageType.Text || _ImageSelected == ImageType.InverseText)
            {
                // TODO: AutoSize. Ugh.
                Invalidate();
            }
        }

        /// <summary>
        /// Processes a mnemonic character.
        /// </summary>
        /// <param name="charCode">The character to process.</param>
        /// <returns><c>true</c> if the character was processed as a mnemonic by the control; otherwise, <c>false</c>.</returns>
        protected override bool ProcessMnemonic(char charCode)
        {
            if (this.UseMnemonic && this.Enabled && this.Visible && IsMnemonic(charCode, this.Text))
            {
                PerformClick();
                return true;
            }
            return base.ProcessMnemonic(charCode);
        }

        // Change the current DrawState, and invalidate as needed.
        private void SetDrawState(DrawState value)
        {
            Debug.Assert((Enabled && value != DrawState.Disabled) || (!Enabled && value == DrawState.Disabled),
                $"New DrawState ({value}) doesn't align with Enabled state ({(Enabled ? "Enabled" : "Disabled")}).");

            if (drawState != value)
            {
                var old = drawState;
                drawState = value;
                if (MouseSelectedColorEnable || old == DrawState.Disabled || value == DrawState.Disabled)
                    Invalidate();   // only invalidate if required
            }
        }

        #endregion
    }
}
