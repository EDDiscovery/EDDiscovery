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
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace ExtendedControls
{
    /// <summary>
    /// Represents a customizable Windows <see cref="Button"/> <see cref="Control"/>.
    /// </summary>
    public class ButtonExt : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonExt"/> class.
        /// </summary>
        /// <remarks>For best results, utilize the <see cref="Control.Click"/> event, or
        /// <see cref="Control.OnClick(EventArgs)"/> for your processing. This event will occur for certain keyboard
        /// inputs (space, enter, or mnemonic), as well as left mouse clicks, but not right mouse clicks.</remarks>
        public ButtonExt() : base() { }

        /// <summary>
        /// This property is unsupported and should not be utilized.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new Image BackgroundImage
        {
            get { return null; }
            set { throw new InvalidOperationException("The BackgroundImage property is not supported by this control. Use Image instead."); }
        }

        // Only Centre and Stretch is supported
        [Category("Appearance"), DefaultValue(ImageLayout.Center),
            Description("Determines Image size in button for Flatstyle!=Standard, Stretch or Center only")]
        public ImageLayout ImageLayout { get { return imagelayout; } set { imagelayout = value; Invalidate(); } }

        /// <summary>
        /// Gets or sets a value that indicates how bright the border color will be when drawn with the
        /// <see cref="FlatStyle.Popup"/> mode, and also either being hovered by the cursor or depressed.
        /// </summary>
        [Category("Appearance"), DefaultValue(1.25F),
            Description("When using FlatStyle.Popup, the FlatAppearance.BorderColor will be multiplied by this amount during a hover or click.")]
        public float BorderColorScaling
        {
            get { return borderColorScaling; }
            set
            {
                if (float.IsNaN(value) || float.IsInfinity(value))
                    return;
                else if (borderColorScaling != value)
                {
                    borderColorScaling = value;
                    if (drawState > DrawState.Normal && FlatStyle == FlatStyle.Popup)
                        Invalidate();
                }
            }
        }
        /// <summary>
        /// Gets or sets a value that indicates how bright the bottom of the background gradient will be when
        /// drawn using <see cref="FlatStyle.Popup"/>.
        /// </summary>
        [Category("Appearance"), DefaultValue(0.5F),
            Description("When using FlatStyle.Popup, the BackColor multiplication factor for the bottom of the background gradient.")]
        public float ButtonColorScaling
        {
            get { return buttonColorScaling; }
            set
            {
                if (float.IsNaN(value) || float.IsInfinity(value))
                    return;
                else if (buttonColorScaling != value)
                {
                    buttonColorScaling = value;
                    if (FlatStyle == FlatStyle.Popup)
                        Invalidate();
                }
            }
        }
        /// <summary>
        /// Gets or sets a value that indicates how bright the button background will be when drawn with
        /// <see cref="FlatStyle.Popup"/> and the control is not <see cref="Control.Enabled"/>.
        /// </summary>
        [Category("Appearance"), DefaultValue(0.5F),
            Description("When using FlatStyle.Popup and not Enabled, this multiplication factor will be used for the background color.")]
        public float ButtonDisabledScaling
        {
            get { return buttonDisabledScaling; }
            set
            {
                if (float.IsNaN(value) || float.IsInfinity(value))
                    return;
                else if (buttonDisabledScaling != value)
                {
                    buttonDisabledScaling = value;
                    if (!Enabled && FlatStyle == FlatStyle.Popup)
                        Invalidate();
                }
            }
        }

        /// <summary>
        /// Occurs when the control is redrawn.
        /// </summary>
        [Category("Appearance"), Description("Occurs when a control needs repainting.")]
        public new event PaintEventHandler Paint { add { base.Paint += value; _CustomPaint += value; } remove { base.Paint -= value; _CustomPaint -= value; } }

        /// <summary>
        /// Specify a <see cref="Color"/> <paramref name="remap"/> table and corresponding <see cref="ColorMatrix"/>.
        /// </summary>
        /// <param name="remap">An array of <see cref="ColorMap"/> items detailing how colors should be remapped in the
        /// provided <see cref="ButtonBase.Image"/>.</param>
        /// <param name="colormatrix">If not null, these values will be used to construct a <see cref="ColorMatrix"/>
        /// for adjusting the intensity of the displayed <see cref="ButtonBase.Image"/> colors.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="remap"/> is <c>null</c>.</exception>
        public void SetDrawnBitmapRemapTable(ColorMap[] remap, float[][] colormatrix = null)
        {
            if (remap == null)
                throw new ArgumentNullException(nameof(remap));

            drawnImageAttributesEnabled?.Dispose();
            drawnImageAttributesDisabled?.Dispose();

            ControlHelpersStaticFunc.ComputeDrawnPanel(out drawnImageAttributesEnabled, out drawnImageAttributesDisabled, buttonDisabledScaling, remap, colormatrix);
        }


        private ImageAttributes drawnImageAttributesEnabled = null;         // Image override (colour etc) for background when using Image while Enabled.
        private ImageAttributes drawnImageAttributesDisabled = null;        // Image override (colour etc) for background when using Image while !Enabled.

        private enum DrawState { Disabled = -1, Normal = 0, Hover, Click };
        private DrawState drawState = DrawState.Normal;                    // The current state of our control, even if base is currently doing the painting.

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]                   // Visible via BorderColorScaling
        private float borderColorScaling = 1.25F;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]                   // Visible via ButtonColorScaling
        private float buttonColorScaling = 0.5F;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]                   // Visible via ButtonDisabledScaling
        private float buttonDisabledScaling = 0.5F;

        private event PaintEventHandler _CustomPaint;                       // base.OnPaint interferes with our custom painting; mimic it to allow others in on the fun.

        private ImageLayout imagelayout = ImageLayout.Center;               // new! image layout

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ButtonExt"/> and optionally releases the managed
        /// resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to
        /// release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                drawnImageAttributesDisabled?.Dispose();
                drawnImageAttributesEnabled?.Dispose();
            }
            _CustomPaint = null;
            drawnImageAttributesDisabled = null;
            drawnImageAttributesEnabled = null;
            base.Dispose(disposing);
        }

        /// <summary>
        /// Raises the <see cref="ButtonExt.Paint"/> event with custom painting logic.
        /// </summary>
        /// <param name="pe">A <see cref="PaintEventArgs"/> that contains the event data.</param>
        protected virtual void OnCustomPaint(PaintEventArgs pe)
        {
            _CustomPaint?.Invoke(this, pe);
        }

        /// <summary>
        /// Raises the <see cref="Control.EnabledChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> containing the event data.</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
                SetDrawState(DrawState.Normal);
            else
                SetDrawState(DrawState.Disabled);

            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyDown"/> event.
        /// </summary>
        /// <param name="kevent">A <see cref="KeyEventArgs"/> containing the event data.</param>
        protected override void OnKeyDown(KeyEventArgs kevent)
        {
            // Enter/Return and mnemonics shall not adjust the appearance, while space gets treated just like the left mouse button.
            if (kevent.KeyData == Keys.Space)
                SetDrawState(DrawState.Click);

            base.OnKeyDown(kevent);
        }

        /// <summary>
        /// Raises the <see cref="Control.KeyUp"/> event.
        /// </summary>
        /// <param name="kevent">A <see cref="KeyEventArgs"/> containing the event data.</param>
        protected override void OnKeyUp(KeyEventArgs kevent)
        {
            if (kevent.KeyData == Keys.Space)
                SetDrawState(DrawState.Normal);

            base.OnKeyUp(kevent);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseDown"/> event.
        /// </summary>
        /// <param name="mevent">A <see cref="MouseEventArgs"/> containing the event data.</param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
                SetDrawState(DrawState.Click);

            base.OnMouseDown(mevent);
        }

        /// <summary>
        /// Raises the <see cref="Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">A <see cref="MouseEventArgs"/> containing the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            SetDrawState(DrawState.Normal);

            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="Control.Click"/> event.
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
            if (drawState == DrawState.Click)
                SetDrawState(DrawState.Hover);

            base.OnMouseUp(mevent);
        }

        /// <summary>
        /// Raises the <see cref="ButtonExt.Paint"/> event.
        /// </summary>
        /// <param name="pe">A <see cref="PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            if (FlatStyle != FlatStyle.Popup)      // popup style uses custom painting.
            {
                base.OnPaint(pe);
            }
            else
            {
                Rectangle border = ClientRectangle;
                border.Width--; border.Height--;

                Rectangle buttonarea = ClientRectangle;
                buttonarea.Inflate(-1, -1);                     // inside it.

               // System.Diagnostics.Debug.WriteLine("Paint " + this.Name + " " + ClientRectangle +" " + border + " " + buttonarea + " c " + BackColor + " " + FlatAppearance.BorderColor);

                Color colBack = Color.Empty;
                Color colBorder = Color.Empty;
                switch (drawState)
                {
                    case DrawState.Disabled:
                        colBack = BackColor.Multiply(buttonDisabledScaling);
                        colBorder = FlatAppearance.BorderColor.Multiply(buttonDisabledScaling);
                        break;
                    case DrawState.Normal:
                    default:
                        colBack = BackColor;
                        colBorder = FlatAppearance.BorderColor;
                        break;
                    case DrawState.Hover:
                        colBack = FlatAppearance.MouseOverBackColor;
                        colBorder = FlatAppearance.BorderColor.Multiply(borderColorScaling);
                        break;
                    case DrawState.Click:
                        colBack = FlatAppearance.MouseDownBackColor;
                        colBorder = FlatAppearance.BorderColor.Multiply(borderColorScaling);
                        break;

                }

                using (var b = new LinearGradientBrush(buttonarea, colBack, colBack.Multiply(buttonColorScaling), 90))
                    pe.Graphics.FillRectangle(b, buttonarea);       // linear grad brushes do not respect smoothing mode, btw

                pe.Graphics.SmoothingMode = SmoothingMode.None;

                using (var p = new Pen(colBorder))
                     pe.Graphics.DrawRectangle(p, border);

                if (Image != null)
                {
                    Size isize = (imagelayout == ImageLayout.Stretch) ? buttonarea.Size : Image.Size;

                    if ((Enabled && drawnImageAttributesEnabled != null) || (!Enabled && drawnImageAttributesDisabled != null))
                    {
                        //System.Diagnostics.Debug.WriteLine("ButtonExt " + this.Name + " Draw image with IA");
                        pe.Graphics.DrawImage(Image, ImageAlign.ImagePositionFromContentAlignment(buttonarea, isize),
                                    0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, (Enabled) ? drawnImageAttributesEnabled : drawnImageAttributesDisabled);
                    }
                    else
                    {
                        pe.Graphics.DrawImage(Image, ImageAlign.ImagePositionFromContentAlignment(buttonarea, isize),
                                    0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel);
                    }
                }

                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                if (!string.IsNullOrEmpty(Text))
                {
                    using (var fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(RtlTranslateAlignment(TextAlign)))
                    using (Brush textb = new SolidBrush((Enabled) ? this.ForeColor : this.ForeColor.Multiply(0.5F)))
                    {
                        if (this.UseMnemonic)
                            fmt.HotkeyPrefix = this.ShowKeyboardCues ? HotkeyPrefix.Show : HotkeyPrefix.Hide;
                        if (this.AutoEllipsis)
                            fmt.Trimming = StringTrimming.EllipsisCharacter;
                        pe.Graphics.DrawString(this.Text, this.Font, textb, buttonarea, fmt);
                    }
                }

                pe.Graphics.SmoothingMode = SmoothingMode.None;

                if (Focused && ShowFocusCues)
                {
                    using (var p = new Pen(colBorder))
                    {
                        Rectangle rcFocus = border;

                        // Thicken the standard border by 1px.
                        rcFocus.Inflate(-1, -1);
                        pe.Graphics.DrawRectangle(p, rcFocus);

                        // Thicken that by an additional 1px, using something similar to ControlPaint.DrawFocusRectangle, but capable of honouring forecolour.
                        rcFocus.Inflate(-1, -1);
                        p.DashStyle = DashStyle.Dash;
                        p.DashPattern = new[] { 1f, 1f };
                        pe.Graphics.DrawRectangle(p, rcFocus);
                    }
                }

                pe.Graphics.SmoothingMode = SmoothingMode.Default;

                this.OnCustomPaint(pe);
            }
        }

        // Change the current DrawState, and invalidate as needed.
        private void SetDrawState(DrawState state)
        {
            if (drawState != state)
            {
                drawState = state;
                if (FlatStyle == FlatStyle.Popup)
                    Invalidate();
            }
        }
    }
}
