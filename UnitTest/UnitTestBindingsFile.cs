using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public static class UnitTestBindingsFile
    {
        [BaseUtils.UnitTests.Test(1000)]
        public static void TestBindingsFile()
        {
            CheckSection("Bindings");

            // the short test one
            string folder = $@"..\..\..\UnitTest\Bindings\";

            {
                File.WriteAllText(Path.Combine(folder, "StartPreset.4.Start"), "KeyboardMouseOnly\r\nKeyboardMouseOnly\r\nCustom\r\nKeyboardMouseOnly\r\n");
                string file = BindingsFile.FindBindingsFile(folder, true);
                CheckThat(file).Contains("Custom.4.2.binds");

                File.WriteAllText(Path.Combine(folder, "StartPreset.4.Start"), "KeyboardMouseOnly\r\nKeyboardMouseOnly\r\nKeyboardMouseOnly\r\nRobDirect\r\n");
                file = BindingsFile.FindBindingsFile(folder, true);
                CheckThat(file).Contains("RobDirect.4.2.binds");
            }
            {
                File.WriteAllText(Path.Combine(folder, "StartPreset.4.Start"), "Test1\r\nTest1\r\nTest1\r\nTest1\r\n");
                string file = BindingsFile.FindBindingsFile(folder, true);
                BindingsFile f = new BindingsFile();
                f.Read(file);
                CompareXML(f.ToXML(), f.FileName);
            }

            // the longer full one

            {
                File.WriteAllText(Path.Combine(folder, "StartPreset.4.Start"), "RobDirect\r\nRobDirect\r\nRobDirect\r\nRobDirect\r\n");
                string file = BindingsFile.FindBindingsFile(folder, true);
                BindingsFile f = new BindingsFile();
                f.Read(file);
                CheckThat(f.FileName).Contains("RobDirect.4.2.binds");
                CompareXML(f.ToXML(), f.FileName);
            }
        }





        [BaseUtils.UnitTests.Test(1001)]
        public static void TestFrontierToVKey()
        {
            InputLanguage defl = InputLanguage.CurrentInputLanguage;
            List<string> done = new List<string>();

            foreach( var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                InputLanguage ll = InputLanguage.FromCulture(culture);

                //System.Diagnostics.Debug.WriteLine($"Culture {culture.Name} uses {ll?.LayoutName}");
            }

            int tested = 0;

            foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
            {
                if (done.Contains(lang.LayoutName))
                {
                    System.Diagnostics.Debug.WriteLine($"Repeat layout {lang.Culture.Name} : {lang.LayoutName}");
                    continue;
                }
                done.Add(lang.LayoutName);

                InputLanguage.CurrentInputLanguage = lang;
                CheckSection($"FrontierVKey Checking {lang.Culture.Name} : {lang.LayoutName}");

                Check(Keys.Up, "Key_UpArrow");
                Check(Keys.Down, "Key_DownArrow");
                Check(Keys.Left, "Key_LeftArrow");
                Check(Keys.Right, "Key_RightArrow");
                Check(Keys.Back, "Key_Backspace");
                Check(Keys.Insert, "Key_Insert");
                Check(Keys.Home, "Key_Home");
                Check(Keys.PageUp, "Key_PageUp");
                Check(Keys.PageDown, "Key_PageDown");
                Check(Keys.Delete, "Key_Delete");
                Check(Keys.End, "Key_End");
                Check(Keys.Space, "Key_Space");
                Check(Keys.F1, "Key_F1");
                Check(Keys.F12, "Key_F12");

                Check(Keys.Tab, "Key_Tab");
                Check(Keys.Capital, "Key_CapsLock");
                Check(Keys.LShiftKey, "Key_LeftShift");
                Check(Keys.RShiftKey, "Key_RightShift");
                Check(Keys.LControlKey, "Key_LeftControl");
                Check(Keys.RControlKey, "Key_RightControl");
                Check(Keys.LMenu, "Key_LeftAlt");
                Check(Keys.RMenu, "Key_RightAlt");

                Check(Keys.NumPad0, "Key_Numpad_0");
                Check(Keys.NumPad9, "Key_Numpad_9");
                Check(KeyObjectExtensions.NumEnter, "Key_Numpad_Enter");
                Check(Keys.Multiply, "Key_Numpad_Multiply");
                Check(Keys.Add, "Key_Numpad_Add");
                Check(Keys.Subtract, "Key_Numpad_Subtract");
                Check(Keys.Decimal, "Key_Numpad_Decimal");
                Check(Keys.NumLock, "Key_NumLock");

                Check(Keys.PrintScreen, "Key_SYSRQ");

                // 6/4/22 confirmed
                // Keys always listed in row order, top row first, middle row, bottom row
                // each keyboard layout is helpfully having different oem assigned to different keys! (crap)
                // Elite was used to see what frontier names were mapped to these oem keys, and http://kbdlayout.info/ was used to find the oem key assigned

                if (lang.LayoutName == "Portuguese")
                {
                    Check(Keys.Oem5, "Key_BackSlash");
                    Check(Keys.Oem4, "Key_Apostrophe");
                    Check(Keys.Oem6, "Key_«");

                    Check(Keys.Oemplus, "Key_Plus");
                    Check(Keys.Oem1, "Key_Acute");

                    Check(Keys.Oem3, "Key_ç");
                    Check(Keys.Oem7, "Key_º");
                    Check(Keys.Oem2, "Key_Tilde");

                    Check(Keys.Oem102, "Key_LessThan");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (lang.LayoutName.Contains("Portuguese (Brazil ABNT"))
                {
                    Check(Keys.Oem3, "Key_Apostrophe");
                    Check(Keys.OemMinus, "Key_Minus");
                    Check(Keys.Oemplus, "Key_Equals");

                    Check(Keys.Oem4, "Key_Acute");
                    Check(Keys.Oem6, "Key_LeftBracket");

                    Check(Keys.Oem1, "Key_ç");
                    Check(Keys.Oem7, "Key_Tilde");
                    Check(Keys.Oem5, "Key_RightBracket");

                    Check(Keys.Oem102, "Key_BackSlash");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.Oem2, "Key_SemiColon");
                    tested++;
                }

                else if (lang.LayoutName == "Turkish Q")
                {
                    Check(Keys.Oem3, "Key_DoubleQuote");
                    Check(Keys.Oem8, "Key_Asterisk");
                    Check(Keys.OemMinus, "Key_Minus");

                    Check(Keys.Oem4, "Key_ğ");
                    Check(Keys.Oem6, "Key_ü");

                    Check(Keys.Oem1, "Key_ş");
                    Check(Keys.I, "Key_I");
                    Check(Keys.Oemcomma, "Key_Comma");

                    Check(Keys.Oem102, "Key_LessThan");
                    Check(Keys.Oem2, "Key_ö");
                    Check(Keys.Oem5, "Key_ç");
                    Check(Keys.OemPeriod, "Key_Period");
                    tested++;
                }

                else if (lang.LayoutName == "Swedish")
                {
                    Check(Keys.Oem5, "Key_§");
                    Check(Keys.Oemplus, "Key_Plus");
                    Check(Keys.Oem4, "Key_Acute");

                    Check(Keys.Oem6, "Key_å");
                    Check(Keys.Oem1, "Key_Umlaut");

                    Check(Keys.Oem3, "Key_ö");
                    Check(Keys.Oem7, "Key_ä");
                    Check(Keys.Oem2, "Key_Apostrophe");

                    Check(Keys.Oem102, "Key_LessThan");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (lang.LayoutName == "Danish")
                {
                    Check(Keys.Oem5, "Key_Half");
                    Check(Keys.Oemplus, "Key_Plus");
                    Check(Keys.Oem4, "Key_Acute");

                    Check(Keys.Oem6, "Key_å");
                    Check(Keys.Oem1, "Key_Umlaut");

                    Check(Keys.Oem3, "Key_æ");
                    Check(Keys.Oem7, "Key_ø");
                    Check(Keys.Oem2, "Key_Apostrophe");

                    Check(Keys.Oem102, "Key_BackSlash");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (lang.LayoutName == "US" || lang.LayoutName == "United States-International")
                {

                    Check(Keys.Oem3, "Key_Grave");
                    Check(Keys.OemMinus, "Key_Minus");
                    Check(Keys.Oemplus, "Key_Equals");

                    Check(Keys.Oem4, "Key_LeftBracket");
                    Check(Keys.Oem6, "Key_RightBracket");

                    Check(Keys.Oem1, "Key_SemiColon");
                    Check(Keys.Oem7, "Key_Apostrophe");

                    // oem 102 is showing KeyBackslash, same as Oem 5. Table maps it to scan code 56
                    Check(Keys.Oem5, "Key_BackSlash");
                    Check(Keys.Oemcomma, "Key_Comma");  // ok
                    Check(Keys.OemPeriod, "Key_Period");    //ok
                    Check(Keys.Oem2, "Key_Slash");  //ok
                    tested++;
                }

                else if (lang.LayoutName == "United Kingdom")
                {
                    Check(Keys.Oem8, "Key_Grave");
                    Check(Keys.OemMinus, "Key_Minus");
                    Check(Keys.Oemplus, "Key_Equals");

                    Check(Keys.Oem4, "Key_LeftBracket");
                    Check(Keys.Oem6, "Key_RightBracket");

                    Check(Keys.Oem1, "Key_SemiColon");
                    Check(Keys.Oem3, "Key_Apostrophe");
                    Check(Keys.Oem7, "Key_Hash");

                    Check(Keys.Oem5, "Key_BackSlash");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.Oem2, "Key_Slash");
                    tested++;
                }

                else if (lang.LayoutName == "German")
                {
                    Check(Keys.Oem5, "Key_Circumflex");
                    Check(Keys.Oem4, "Key_ß");
                    Check(Keys.Oem6, "Key_Acute");

                    Check(Keys.Oem1, "Key_ü");
                    Check(Keys.Oemplus, "Key_Plus");

                    Check(Keys.Oem3, "Key_ö");
                    Check(Keys.Oem7, "Key_ä");
                    Check(Keys.Oem2, "Key_Hash");

                    Check(Keys.Oem102, "Key_LessThan");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (lang.LayoutName == "Spanish")
                {
                    Check(Keys.Oem5, "Key_Grave");
                    Check(Keys.Oem4, "Key_Minus");
                    Check(Keys.Oem6, "Key_Equals");

                    Check(Keys.Oem1, "Key_LeftBracket");
                    Check(Keys.Oemplus, "Key_RightBracket");

                    Check(Keys.Oem3, "Key_SemiColon");
                    Check(Keys.Oem7, "Key_Apostrophe");
                    Check(Keys.Oem2, "Key_Hash");

                    Check(Keys.Oem102, "Key_BackSlash");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.OemMinus, "Key_Slash");
                    tested++;
                }

                else if (lang.LayoutName.Contains("French"))
                {
                    Check(Keys.Oem7, "Key_SuperscriptTwo");
                    Check(Keys.Oem4, "Key_RightParenthesis");
                    Check(Keys.Oemplus, "Key_Equals");

                    Check(Keys.Oem6, "Key_Circumflex");
                    Check(Keys.Oem1, "Key_Dollar");

                    Check(Keys.M, "Key_M");
                    Check(Keys.Oem3, "Key_ù");
                    Check(Keys.Oem5, "Key_Asterisk");

                    Check(Keys.Oem102, "Key_LessThan");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_SemiColon");
                    Check(Keys.Oem2, "Key_Colon");
                    Check(Keys.Oem8, "Key_ExclamationPoint");
                    tested++;
                }

                else if (lang.LayoutName.Contains("Polish"))
                {
                    Check(Keys.Oem3, "Key_Grave");

                    Check(Keys.OemMinus, "Key_Minus");
                    Check(Keys.Oemplus, "Key_Equals");

                    Check(Keys.Oem4, "Key_LeftBracket");
                    Check(Keys.Oem6, "Key_RightBracket");

                    Check(Keys.Oem1, "Key_SemiColon");
                    Check(Keys.Oem7, "Key_Apostrophe");
                    Check(Keys.Oem5, "Key_Hash");

                    Check(Keys.Oem102, "Key_BackSlash");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.OemQuestion, "Key_Slash");
                    tested++;
                }

                else if (lang.LayoutName.Contains("Italian"))
                {
                    Check(Keys.Oem5, "Key_BackSlash");
                    Check(Keys.Oem4, "Key_Apostrophe");
                    Check(Keys.Oem6, "Key_ì");

                    Check(Keys.Oem1, "Key_è");
                    Check(Keys.Oemplus, "Key_Plus");

                    Check(Keys.Oem3, "Key_ò");
                    Check(Keys.Oem7, "Key_à");
                    Check(Keys.Oem2, "Key_ù");

                    Check(Keys.Oem102, "Key_LessThan");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (lang.LayoutName.Contains("Norwegian"))
                {
                    Check(Keys.Oem5, "Key_|");
                    Check(Keys.Oemplus, "Key_Plus");
                    Check(Keys.Oem4, "Key_BackSlash");

                    Check(Keys.Oem6, "Key_å");
                    Check(Keys.Oem1, "Key_Umlaut");

                    Check(Keys.Oem3, "Key_ø");
                    Check(Keys.Oem7, "Key_æ");
                    Check(Keys.Oem2, "Key_Apostrophe");

                    Check(Keys.Oem102, "Key_LessThan");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (lang.LayoutName.Contains("Finnish"))
                {
                    Check(Keys.Oem5, "Key_§");
                    Check(Keys.Oemplus, "Key_Plus");
                    Check(Keys.Oem4, "Key_Acute");

                    Check(Keys.Oem6, "Key_å");
                    Check(Keys.Oem1, "Key_Umlaut");

                    Check(Keys.Oem3, "Key_ö");
                    Check(Keys.Oem7, "Key_ä");
                    Check(Keys.Oem2, "Key_Apostrophe");

                    Check(Keys.Oem102, "Key_LessThan");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (lang.LayoutName.Contains("Ukrainian (Enhanced)"))
                {
                    Check(Keys.Oem3, "Key_ё");
                    Check(Keys.OemMinus, "Key_Minus");
                    Check(Keys.Oemplus, "Key_Equals");

                    Check(Keys.Oem4, "Key_х");
                    Check(Keys.Oem6, "Key_ї");

                    Check(Keys.Oem1, "Key_ж");
                    Check(Keys.Oem7, "Key_є");
                    Check(Keys.Oem5, "Key_BackSlash");

                    Check(Keys.Oem102, "Key_ґ");
                    Check(Keys.Oemcomma, "Key_б");
                    Check(Keys.OemPeriod, "Key_ю");
                    Check(Keys.Oem2, "Key_Period");
                    tested++;
                }

                else if (lang.LayoutName.Contains("Czech")) //rechecked 7th
                {
                    Check(Keys.Oem3, "Key_SemiColon");
                    Check(Keys.Oemplus, "Key_Equals");
                    Check(Keys.Oem2, "Key_Acute");

                    Check(Keys.Oem4, "Key_ú");
                    Check(Keys.Oem6, "Key_RightParenthesis");

                    Check(Keys.Oem1, "Key_ů");
                    Check(Keys.Oem7, "Key_§");
                    Check(Keys.Oem5, "Key_Umlaut");

                    Check(Keys.Oem102, "Key_BackSlash");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (lang.LayoutName.Contains("Greek")) // 7/4/22
                {
                    Check(Keys.Oem3, "Key_Grave");
                    Check(Keys.OemMinus, "Key_Minus");
                    Check(Keys.Oemplus, "Key_Equals");

                    Check(Keys.Oem4, "Key_LeftBracket");
                    Check(Keys.Oem6, "Key_RightBracket");

                    Check(Keys.Oem1, "Key_΄");
                    Check(Keys.Oem7, "Key_Apostrophe");
                    Check(Keys.Oem5, "Key_BackSlash");

                    Check(Keys.Oem102, "Key_LessThan");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.Oem2, "Key_Slash");
                    tested++;
                }


                else if (lang.LayoutName.Contains("Lithuanian"))     // 7/4/22
                {
                    Check(Keys.Oem3, "Key_Grave");
                    Check(Keys.OemMinus, "Key_Underline");
                    Check(Keys.Oemplus, "Key_Plus");

                    Check(Keys.Oem4, "Key_į");
                    Check(Keys.Oem6, "Key_“");

                    Check(Keys.Oem1, "Key_ų");
                    Check(Keys.Oem7, "Key_ė");
                    Check(Keys.Oem5, "Key_|");

                    Check(Keys.Oem102, "Key_BackSlash");
                    Check(Keys.Oemcomma, "Key_č");
                    Check(Keys.OemPeriod, "Key_š");
                    Check(Keys.Oem2, "Key_ę");
                    tested++;
                }

                else if (lang.LayoutName.Contains("Slovak")) // 7/4/22
                {
                    Check(Keys.Oem3, "Key_SemiColon");
                    Check(Keys.Oem2, "Key_Equals");
                    Check(Keys.Oem8, "Key_Acute");

                    Check(Keys.Oem4, "Key_ú");
                    Check(Keys.Oem6, "Key_ä");

                    Check(Keys.Oem1, "Key_ô");
                    Check(Keys.Oem7, "Key_§");
                    Check(Keys.Oem5, "Key_ň");

                    Check(Keys.Oem102, "Key_Ampersand");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (lang.LayoutName.Contains("Slovenian"))  // 7/4/22
                {
                    Check(Keys.Oem3, "Key_¸");
                    Check(Keys.Oem2, "Key_Apostrophe");
                    Check(Keys.Oemplus, "Key_Plus");

                    Check(Keys.Oem4, "Key_š");
                    Check(Keys.Oem6, "Key_đ");

                    Check(Keys.Oem1, "Key_č");
                    Check(Keys.Oem7, "Key_ć");
                    Check(Keys.Oem5, "Key_ž");

                    Check(Keys.Oem102, "Key_LessThan");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (lang.LayoutName.Contains("Romanian (Standard)"))    // 7/4/22
                {
                    Check(Keys.Oem3, "Key_RightBracket");
                    Check(Keys.OemMinus, "Key_Plus");
                    Check(Keys.Oemplus, "Key_Apostrophe");

                    Check(Keys.Oem4, "Key_ă");
                    Check(Keys.Oem6, "Key_î");

                    Check(Keys.Oem1, "Key_ş");
                    Check(Keys.Oem7, "Key_ţ");
                    Check(Keys.Oem5, "Key_â");

                    Check(Keys.Oem102, "Key_LessThan");
                    Check(Keys.Oemcomma, "Key_Comma");
                    Check(Keys.OemPeriod, "Key_Period");
                    Check(Keys.Oem2, "Key_Minus");
                    tested++;
                }
                else
                    CheckThat(true).IsFalse();      // bad unknown test language
            }

            InputLanguage.CurrentInputLanguage = defl;
            CheckThat(tested).Is(21);
        }

        static private void Check(Keys k, string key)
        {
            string output = FrontierKeyConversion.FrontierToKeys(key);
            Keys kc = output.ToVkey();

            CheckThat(kc).Equals(k);
            string check = kc != k ? "********** ERROR" : "";
            System.Diagnostics.Debug.WriteLine($"  Check Key {key} => Frontier: {output} Keyc: {kc} KcNorm: {KeyObjectExtensions.VKeyToString(kc)} {check}");
            if (kc != k)
            {

            }
        }


        public static void CompareXML(string xml, string file)
        {
            string[] xmloutput = xml.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] comparefile = File.ReadAllLines(file);
            bool ok = true;
            for (int i = 0; i < comparefile.Length; i++)
            {
                if (xmloutput.Length > i)
                {
                    if (xmloutput[i].Trim() != comparefile[i].Trim()) // need the trim, hence why its here
                    {
                        System.Diagnostics.Debug.WriteLine($"Difference line {i + 1} {xmloutput[i].Trim()} vs {comparefile[i].Trim()}");
                        ok = false;
                        break;
                    }
                }
                else
                    ok = false;
            }
            CheckThat(ok).IsTrue();
        }
    }
}
