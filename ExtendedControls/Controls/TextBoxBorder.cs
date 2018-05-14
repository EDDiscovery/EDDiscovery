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
using BaseUtils.Win32Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace ExtendedControls
{
    public class TextBoxBorder : Control
    {
        // BorderColour != transparent to use ours
        // BorderStyle to set textbox style..  None for off.  Can use both if you wish 

        public Color BorderColor { get { return bordercolor; } set { bordercolor = value; Reposition(); Invalidate(true); } }
        public float BorderColorScaling { get; set; } = 0.5F;
        public System.Windows.Forms.BorderStyle BorderStyle { get { return textbox.BorderStyle; } set { textbox.BorderStyle = value; Reposition(); Invalidate(true); } }

        public override Color ForeColor { get { return textbox.ForeColor; } set { textbox.ForeColor = value; Invalidate(true); } }
        public override Color BackColor { get { return backnormalcolor; } set { backnormalcolor = value; if (!inerrorcondition) { textbox.BackColor = backnormalcolor; Invalidate(true); } } }
        public Color BackErrorColor { get { return backerrorcolor; } set { backerrorcolor = value; if (inerrorcondition) { textbox.BackColor = backerrorcolor; Invalidate(true); } } }
        public bool InErrorCondition { get { return inerrorcondition; } set { if (inerrorcondition != value) { inerrorcondition = value; textbox.BackColor = inerrorcondition ? backerrorcolor : backnormalcolor; Invalidate(true); } } }
        public Color ControlBackground { get { return controlbackcolor; } set { controlbackcolor = value; Invalidate(true); } } // colour of unfilled control area if border is on or button

        public bool WordWrap { get { return textbox.WordWrap; } set { textbox.WordWrap = value; } }
        public bool Multiline { get { return textbox.Multiline; } set { textbox.Multiline = value; Reposition(); } }
        public bool ReadOnly { get { return textbox.ReadOnly; } set { textbox.ReadOnly = value; } }
        public bool ClearOnFirstChar { get; set; } = false;
        public System.Windows.Forms.ScrollBars ScrollBars { get { return textbox.ScrollBars; } set { textbox.ScrollBars = value; } }

        public override string Text { get { return textbox.Text; } set { textbox.Text = value; } }

        public void SelectAll() { textbox.SelectAll(); }
        public int SelectionStart { get { return textbox.SelectionStart; } set { textbox.SelectionStart = value; } }
        public int SelectionLength { get { return textbox.SelectionLength; } set { textbox.SelectionLength = value; } }
        public void Select(int s, int e) { textbox.Select(s, e); }
        public string SelectedText { get { return textbox.SelectedText; } }

        public HorizontalAlignment TextAlign { get { return textbox.TextAlign; } set { textbox.TextAlign = value; } }

        public System.Windows.Forms.AutoCompleteMode AutoCompleteMode { get { return textbox.AutoCompleteMode; } set { textbox.AutoCompleteMode = value; } }
        public System.Windows.Forms.AutoCompleteSource AutoCompleteSource { get { return textbox.AutoCompleteSource; } set { textbox.AutoCompleteSource = value; } }

        public void SetTipDynamically(ToolTip t, string text) { t.SetToolTip(textbox, text); } // only needed for dynamic changes..

        public Action<TextBoxBorder> ReturnPressed;                              // fires if return pressed

        protected TextBox textbox;
        private Color bordercolor = Color.Transparent;
        private Color controlbackcolor = SystemColors.Control;
        private Color backnormalcolor;        // normal back colour..
        private Color backerrorcolor;        // error back colour..
        private bool inerrorcondition;          // if in error condition

        private char lastkey;               // records key presses
        private int keyspressed = 0;

        public TextBoxBorder() : base()
        {
            this.GotFocus += TextBoxBorder_GotFocus;
            textbox = new TextBox();
            textbox.BorderStyle = BorderStyle.FixedSingle;
            backerrorcolor = Color.Red;
            backnormalcolor = textbox.BackColor;
            inerrorcondition = false;

            // Enter and Leave is handled by this wrapper control itself, since when we leave the textbox, we leave this
            textbox.Click += Textbox_Click;
            textbox.DoubleClick += Textbox_DoubleClick;
            textbox.KeyUp += Textbox_KeyUp;
            textbox.KeyDown += Textbox_KeyDown;
            textbox.KeyPress += Textbox_KeyPress;
            textbox.MouseClick += Textbox_MouseClick;
            textbox.MouseDoubleClick += Textbox_MouseDoubleClick;
            textbox.MouseUp += Textbox_MouseUp;
            textbox.MouseDown += Textbox_MouseDown;
            textbox.MouseMove += Textbox_MouseMove;
            textbox.MouseEnter += Textbox_MouseEnter;
            textbox.MouseLeave += Textbox_MouseLeave;
            textbox.TextChanged += Textbox_TextChanged;
            textbox.Validating += Textbox_Validating;
            textbox.Validated += Textbox_Validated;
            Controls.Add(textbox);
        }

        private void TextBoxBorder_GotFocus(object sender, EventArgs e)
        {
            textbox.Focus();
        }

        const int borderoffset = 3;

        private bool OurBorder { get { return !BorderColor.IsFullyTransparent(); } }

        private void Reposition()
        {
            if (ClientRectangle.Width > 0)
            {
                int offset = OurBorder ? borderoffset : 0;
                textbox.Location = new Point(offset, offset);
                textbox.Size = new Size(ClientRectangle.Width - offset * 2, ClientRectangle.Height - offset * 2);
                this.Height = textbox.Height + offset * 2;
                //System.Diagnostics.Debug.WriteLine("Repos " + Name + ":" + ClientRectangle.Size + " " + textbox.Location + " " + textbox.Size + " " + BorderColor + " " + textbox.BorderStyle);
            }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            Reposition();

        }

        bool firstpaint = true;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (firstpaint)
            {
                System.ComponentModel.IContainer ic = this.GetParentContainerComponents();

                ic?.CopyToolTips(this, new Control[] { textbox });

                firstpaint = false;
            }

            using (Brush highlight = new SolidBrush(controlbackcolor))
            {
                e.Graphics.FillRectangle(highlight, ClientRectangle);
            }

            base.OnPaint(e);

            //System.Diagnostics.Debug.WriteLine("Repaint" + Name + ":" + ClientRectangle.Size + " " + textbox.Location + " " + textbox.Size + " " + BorderColor + " " + textbox.BorderStyle);

            if (OurBorder)
            {
                Rectangle clientborder = new Rectangle(0, 0, ClientRectangle.Width, textbox.Height + borderoffset*2);       // paint it around the actual area of the textbox, not just bit

                Color color1 = BorderColor;
                Color color2 = BorderColor.Multiply(BorderColorScaling);

                GraphicsPath g1 = ControlHelpersStaticFunc.RectCutCorners(clientborder.X + 1, clientborder.Y + 1, clientborder.Width - 2, clientborder.Height - 1, 1, 1);
                using (Pen pc1 = new Pen(color1, 1.0F))
                    e.Graphics.DrawPath(pc1, g1);

                GraphicsPath g2 = ControlHelpersStaticFunc.RectCutCorners(clientborder.X, clientborder.Y, clientborder.Width, clientborder.Height - 1, 2, 2);
                using (Pen pc2 = new Pen(color2, 1.0F))
                    e.Graphics.DrawPath(pc2, g2);
            }
        }

        public void NumericKeyPressHandler(KeyPressEventArgs e)     // given a keypress, validate it for number format..
        {
            const char vbBack = '\u0008';

            System.Globalization.NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();

            TextBoxBorder tempBox = this;

            if (Char.IsDigit(e.KeyChar))
            {
                if (tempBox.Text.Length != 0)
                {
                    if (tempBox.SelectionStart == 0 && (tempBox.Text[0].ToString()) == negativeSign && tempBox.SelectionLength == 0)
                        e.Handled = true;
                }
            }
            else if (keyInput.Equals(negativeSign))
            {
                if (tempBox.SelectionStart != 0 || (tempBox.Text.Contains(negativeSign) && !tempBox.SelectedText.Contains(negativeSign)))
                    e.Handled = true;
            }
            else if (keyInput.Equals(decimalSeparator))
            {
                if (tempBox.Text.Length != 0)
                {
                    if (tempBox.SelectionStart == 0 && (tempBox.Text[0].ToString()) == negativeSign && !tempBox.SelectedText.Contains(negativeSign) || tempBox.Text.Contains(decimalSeparator) && !tempBox.SelectedText.Contains(decimalSeparator))
                        e.Handled = true;

                }
                // Decimal separator is OK
            }
            else if (e.KeyChar == vbBack)
            {
                // Backspace key is OK
            }
            else
            {
                // Consume this invalid key and beep.
                e.Handled = true;
            }
        }

        #region Supported Events

        // intercept most events and warn if used.. 
        // done this way because you can't hide events from the underlying control class (c# does not support protected inheritance), 
        // and we need to know if someone uses one we do not support
        // LEAVE in the commented out ones which we do support.. this list is going to be useful for other controls which we wish
        // to make

        public new event EventHandler BackColorChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler BackgroundImageChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler BackgroundImageLayout { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler BindingContextChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler CausesValidationChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler ChangeUICues { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event EventHandler Click { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler ClientSizeChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler ContextMenuStripChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler ControlAdded { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler ControlRemoved { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler CursorChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler DockChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event EventHandler DoubleClick { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler DragDrop { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler DragEnter { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler DragLeave { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler DragOver { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler EnabledChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event EventHandler Enter { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler FontChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler ForeColorChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler GiveFeedback { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler HelpRequested { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler ImeModeChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event KeyEventHandler KeyDown { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event KeyPressEventHandler KeyPress { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event KeyEventHandler KeyUp { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler Layout { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event EventHandler Leave { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler LocationChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler MarginChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler MouseCaptureChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event EventHandler MouseClick { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event EventHandler MouseDoubleClick { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event MouseEventHandler MouseDown { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event MouseEventHandler MouseEnter { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event MouseEventHandler MouseLeave { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event MouseEventHandler MouseMove { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event MouseEventHandler MouseUp { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler Move { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler PaddingChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler ParentChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler PreviewKeyDown { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler QueryAccessibilityHelp { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler QueryContinueDrag { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler RegionChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler Resize { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler RightToLeftChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler SizeChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler StyleChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler SystemColorsChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler TabIndexChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler TabStopChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event EventHandler TextChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event System.ComponentModel.CancelEventHandler Validating { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        //public new event EventHandler Validated { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }
        public new event EventHandler VisibleChanged { add { EventWarn(System.Reflection.MethodBase.GetCurrentMethod().Name); } remove { System.Diagnostics.Debug.Assert(true); } }

        void EventWarn(string method)
        {
            System.Diagnostics.Debug.WriteLine("*** Event " + method + " NOT SUPPORTED ");
            System.Diagnostics.Debug.Assert(false);
        }

        private void Textbox_Validated(object sender, EventArgs e)
        {
            OnValidated(e);
        }

        private void Textbox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OnValidating(e);
        }

        private void Textbox_DoubleClick(object sender, EventArgs e)
        {
            OnDoubleClick(e);
        }

        private void Textbox_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        private void Textbox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnMouseDoubleClick(e);
        }

        private void Textbox_MouseClick(object sender, MouseEventArgs e)
        {
            OnMouseClick(e);
        }

        private void Textbox_MouseEnter(object sender, EventArgs e)
        {
            OnMouseEnter(e);
        }

        private void Textbox_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);
        }

        private void Textbox_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        private void Textbox_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e);
        }

        private void Textbox_MouseUp(object sender, MouseEventArgs e)
        {
            OnMouseUp(e);
        }

        private void Textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            lastkey = e.KeyChar;
            keyspressed++;

            if (e.KeyChar == '\r')
            {
                ReturnPressed?.Invoke(this);
            }

            OnKeyPress(e);
        }

        private void Textbox_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void Textbox_KeyUp(object sender, KeyEventArgs e)
        {
            OnKeyUp(e);
        }

        bool nonreentrantchange = true;
        protected virtual void Textbox_TextChanged(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Text changed " + nonreentrantchange);
            if (nonreentrantchange == true)
            {
                if (ClearOnFirstChar && keyspressed == 1)
                {
                    nonreentrantchange = false;
                    textbox.Text = "" + lastkey;
                    textbox.Select(1, 1);
                    nonreentrantchange = true;
                }

                OnTextChanged(e);
            }
        }

        #endregion

    }
}
