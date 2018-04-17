/*g
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
using System.Windows.Forms;

namespace ExtendedControls
{
    public abstract class NumberBox<T> : TextBoxBorder
    {
        public string Format { get { return format; } set { format = value; base.Text = ConvertToString(Value); } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Globalization.CultureInfo FormatCulture { get { return culture; } set { culture = value; } }

        public int DelayBeforeNotification { get; set; } = 0;
        public T Minimum { get; set; }
        public T Maximum { get; set; }

        public bool IsValid { get { T v;  return ConvertFromString(base.Text,out v); } }        // is the text a valid value?

        public void SetComparitor( NumberBox<T> other, int compare)         // aka -2 (<=) -1(<) 0 (=) 1 (>) 2 (>=)
        {
            othernumber = other;
            othercomparision = compare;
            InErrorCondition = !IsValid;
        }

        public void SetBlank()          // Blanks it, but does not declare an error
        {
            ignorechange = true;
            base.Text = "";
            InErrorCondition = false;
            ignorechange = false;
        }

        public void SetNonBlank()       // restores it to its last value
        {
            base.Text = ConvertToString(Value);
        }

        public event EventHandler ValueChanged              // fired (first) when value is changed to a new valid value. Can be delayed by DelayBeforeNotification
        {
            add { Events.AddHandler(EVENT_VALUECHANGED, value); }
            remove { Events.RemoveHandler(EVENT_VALUECHANGED, value); }
        }

        public Action<bool> ValidityChanged;                    // fires (second) if validity changes

        // Finally, Use TextChanged to see all changes, then you can check for IsValid.  Fires after ValueChanged/ValidityChanged

        public T Value                                          // will fire a ValueChanged event
        {
            get { return number; }
            set
            {
                number = value;
                base.Text = ConvertToString(number);            // triggers change text event, which sets validity
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public T ValueNoChange                                  //will not fire a ValueChanged event
        {
            get { return number; }
            set
            {
                number = value;
                ignorechange = true;
                base.Text = ConvertToString(number);            // triggers change text event but its ignored
                ignorechange = false;
                InErrorCondition = !IsValid;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text { get { return base.Text; } set { System.Diagnostics.Debug.Assert(false, "Can't set Number box"); } }       // can't set Text, only read..

        #region Implementation

        private T number;
        protected bool ignorechange = false;
        private string format = "N";
        private System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;
        private Timer timer;
        private static readonly object EVENT_VALUECHANGED = new object();
        protected NumberBox<T> othernumber { get; set; } = null;             // attach to another box for validation
        protected int othercomparision { get; set; } = 0;              // aka -2 (<=) -1(<) 0 (=) 1 (>) 2 (>=)

        protected abstract string ConvertToString(T v);
        protected abstract bool ConvertFromString(string t, out T number);
        protected abstract bool AllowedChar(char c);

        public NumberBox()
        {
            timer = new Timer();
            timer.Tick += Timer_Tick;
        }

        public new void Dispose()
        {
            timer.Dispose();
            base.Dispose();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!ignorechange)
            {
                T newvalue;

                if (ConvertFromString(Text, out newvalue))
                {
                    number = newvalue;

                    if (DelayBeforeNotification <= 0)
                    {
                        EventHandler handler = (EventHandler)Events[EVENT_VALUECHANGED];
                        if (handler != null) handler(this, new EventArgs());
                    }
                    else
                    {
                        timer.Interval = DelayBeforeNotification;
                        timer.Start();
                    }

                    if (InErrorCondition)
                        ValidityChanged?.Invoke(true);

                    InErrorCondition = false;
                }
                else
                {                               // Invalid, indicate
                    if (!InErrorCondition)
                        ValidityChanged?.Invoke(false);
                    InErrorCondition = true;
                }
            }

            base.OnTextChanged(e);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            EventHandler handler = (EventHandler)Events[EVENT_VALUECHANGED];
            if (handler != null) handler(this, new EventArgs());
        }

        protected override void OnKeyPress(KeyPressEventArgs e) // limit keys to whats allowed for a double
        {
            if (AllowedChar(e.KeyChar))
            {
                base.OnKeyPress(e);
            }
            else
            {
                e.Handled = true;
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            if (!IsValid)           // if text box is not valid, go back to the original colour with no chanve event
                ValueNoChange = number;

            base.OnLeave(e);
        }

        #endregion
    }

    public class NumberBoxDouble : NumberBox<double>
    {
        public NumberBoxDouble()
        {
            ValueNoChange = 0;
            Minimum = double.MinValue;
            Maximum = double.MaxValue;
        }

        protected override string ConvertToString(double v)
        {
            return v.ToString(Format, FormatCulture);
        }
        protected override bool ConvertFromString(string t, out double number)
        {
            bool ok = double.TryParse(t, System.Globalization.NumberStyles.Float, FormatCulture, out number) &&
                number >= Minimum && number <= Maximum;
            if (ok && othernumber != null)
                ok = number.CompareTo(othernumber.Value, othercomparision);
            return ok;
        }

        protected override bool AllowedChar(char c)
        {
            return (char.IsDigit(c) || c == 8 ||
                (c == FormatCulture.NumberFormat.CurrencyDecimalSeparator[0] && Text.IndexOf(FormatCulture.NumberFormat.CurrencyDecimalSeparator) == -1) ||
                (c == FormatCulture.NumberFormat.NegativeSign[0] && SelectionStart == 0 && Minimum < 0));
        }
    }

    public class NumberBoxLong : NumberBox<long>
    {
        public NumberBoxLong()
        {
            ValueNoChange = 0;
            Minimum = long.MinValue;
            Maximum = long.MaxValue;
            Format = "D";
        }

        protected override string ConvertToString(long v)
        {
            return v.ToString(Format, FormatCulture);
        }
        protected override bool ConvertFromString(string t, out long number)
        {
            bool ok = long.TryParse(t, System.Globalization.NumberStyles.Integer, FormatCulture, out number) &&
                            number >= Minimum && number <= Maximum;
            if (ok && othernumber != null)
                ok = number.CompareTo(othernumber.Value, othercomparision);
            return ok;
        }

        protected override bool AllowedChar(char c)
        {
            return (char.IsDigit(c) || c == 8 ||
                (c == FormatCulture.NumberFormat.NegativeSign[0] && SelectionStart == 0 && Minimum < 0));
        }
    }
}

