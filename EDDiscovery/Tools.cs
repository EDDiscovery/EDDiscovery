using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery
{
    public class Tools
    {

        public static void TextBox_Numeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char vbBack = '\u0008';

            System.Globalization.NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();

            if (sender is TextBoxBase)
            {
                TextBoxBase tempBox = (TextBox)sender;
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
        }


        public static void TextBox_Int_KeyPress(System.Object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            const char vbBack = '\u0008';

            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();
            if (sender is TextBoxBase)
            {
                TextBoxBase tempBox = (TextBox)sender;
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
                    // Decimal separator is OK
                    if (tempBox.SelectionStart != 0 || (tempBox.Text.Contains(negativeSign) && !tempBox.SelectedText.Contains(negativeSign)))
                        e.Handled = true;
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
        }

        public static string appfolder = "EDDiscovery";

        static internal string GetAppDataDirectory()
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["StoreDataInProgramDirectory"] == "true")
                {
                    return Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Data");
                }
                else
                {
                    string datapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appfolder) + Path.DirectorySeparatorChar;

                    if (!Directory.Exists(datapath))
                        Directory.CreateDirectory(datapath);


                    return datapath;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "GetAppDataDirectory Exception", System.Windows.Forms.MessageBoxButtons.OK);
                return null;
            }
        }



        static public bool GetBool(JToken jToken)
        {
            if (IsNullOrEmptyT(jToken))
                return false;
            return jToken.Value<bool>();
        }

        static public float GetFloat(JToken jToken)
        {
            if (IsNullOrEmptyT(jToken))
                return 0f;
            return jToken.Value<float>();
        }


        static public int GetInt(JToken jToken)
        {
            if (IsNullOrEmptyT(jToken))
                return 0;
            return jToken.Value<int>();
        }


        static public string GetString(JToken jToken)
        {
            if (IsNullOrEmptyT(jToken))
                return null;
            return jToken.Value<string>();
        }

        static public string GetStringOrDefault(JToken jToken,string def)
        {
            if (IsNullOrEmptyT(jToken))
                return def;
            return jToken.Value<string>();
        }


        static public  bool IsNullOrEmptyT(JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }

        static public string WordWrap(string input, int linelen )
        {
            String[] split = input.Split(new char[] { ' '});

            string ans = "";
            int l = 0;
            for( int i = 0; i < split.Length; i++ )
            {
                ans += split[i];
                l += split[i].Length;
                if (l > linelen)
                {
                    ans += Environment.NewLine;
                    l = 0;
                }
                else
                    ans += " ";
            }

            return ans;
        }

        static public string StackTrace(string trace, string enclosingfunc, int lines )
        {
            int offset = trace.IndexOf(enclosingfunc);

            string ret = "";

            if (offset != -1)
            {
                CutLine(ref trace, offset);

                while (lines-- > 0)
                {
                    string l = CutLine(ref trace, 0);
                    if (l != "")
                    {
                        if (ret != "")
                            ret = ret + Environment.NewLine + l;
                        else
                            ret = l;
                    }
                    else
                        break;
                }
            }
            else
                ret = trace;

            return ret;
        }

        static public string CutLine( ref string trace, int offset )
        {
            int nloffset = trace.IndexOf(Environment.NewLine, offset);
            string ret;
            if ( nloffset != -1 )
            {
                ret = trace.Substring(offset, nloffset - offset);
                trace = trace.Substring(nloffset);
                if (trace.Length >= Environment.NewLine.Length)
                    trace = trace.Substring(Environment.NewLine.Length);
            }
            else
            {
                ret = trace;
                trace = "";
            }

            return ret;
        }
    }
}
