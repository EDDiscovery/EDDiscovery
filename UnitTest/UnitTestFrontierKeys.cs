using EliteDangerousCore;
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
    public static class UnitTestFrontierKeys
    {
        [BaseUtils.UnitTests.Test(200)]
        public static void TestFrontierToVKey()
        {
            int tested = 0;
            foreach (var cult in FrontierKeyConversion.SupportedLayoutCultures)
            {
                string layoutname = cult.Item1;
                CheckSection($"FrontierVKey Checking {layoutname}");

                CheckFrontierToVkey(layoutname,Keys.Up, "Key_UpArrow");
                CheckFrontierToVkey(layoutname,Keys.Down, "Key_DownArrow");
                CheckFrontierToVkey(layoutname,Keys.Left, "Key_LeftArrow");
                CheckFrontierToVkey(layoutname,Keys.Right, "Key_RightArrow");
                CheckFrontierToVkey(layoutname,Keys.Back, "Key_Backspace");
                CheckFrontierToVkey(layoutname,Keys.Insert, "Key_Insert");
                CheckFrontierToVkey(layoutname,Keys.Home, "Key_Home");
                CheckFrontierToVkey(layoutname,Keys.PageUp, "Key_PageUp");
                CheckFrontierToVkey(layoutname,Keys.PageDown, "Key_PageDown");
                CheckFrontierToVkey(layoutname,Keys.Delete, "Key_Delete");
                CheckFrontierToVkey(layoutname,Keys.End, "Key_End");
                CheckFrontierToVkey(layoutname,Keys.Space, "Key_Space");
                CheckFrontierToVkey(layoutname,Keys.F1, "Key_F1");
                CheckFrontierToVkey(layoutname,Keys.F12, "Key_F12");

                CheckFrontierToVkey(layoutname,Keys.Tab, "Key_Tab");
                CheckFrontierToVkey(layoutname,Keys.Capital, "Key_CapsLock");
                CheckFrontierToVkey(layoutname,Keys.LShiftKey, "Key_LeftShift");
                CheckFrontierToVkey(layoutname,Keys.RShiftKey, "Key_RightShift");
                CheckFrontierToVkey(layoutname,Keys.LControlKey, "Key_LeftControl");
                CheckFrontierToVkey(layoutname,Keys.RControlKey, "Key_RightControl");
                CheckFrontierToVkey(layoutname,Keys.LMenu, "Key_LeftAlt");
                CheckFrontierToVkey(layoutname,Keys.RMenu, "Key_RightAlt");

                CheckFrontierToVkey(layoutname,Keys.NumPad0, "Key_Numpad_0");
                CheckFrontierToVkey(layoutname,Keys.NumPad9, "Key_Numpad_9");
                CheckFrontierToVkey(layoutname,KeyObjectExtensions.NumEnter, "Key_Numpad_Enter");
                CheckFrontierToVkey(layoutname,Keys.Multiply, "Key_Numpad_Multiply");
                CheckFrontierToVkey(layoutname,Keys.Add, "Key_Numpad_Add");
                CheckFrontierToVkey(layoutname,Keys.Subtract, "Key_Numpad_Subtract");
                CheckFrontierToVkey(layoutname,Keys.Decimal, "Key_Numpad_Decimal");
                CheckFrontierToVkey(layoutname,Keys.NumLock, "Key_NumLock");

                CheckFrontierToVkey(layoutname,Keys.PrintScreen, "Key_SYSRQ");

                // 6/4/22 confirmed
                // Keys always listed in row order, top row first, middle row, bottom row
                // each keyboard layout is helpfully having different oem assigned to different keys! (crap)
                // Elite was used to see what frontier names were mapped to these oem keys, and http://kbdlayout.info/ was used to find the oem key assigned

                failed.Clear();

                if (layoutname == "Portuguese")
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_BackSlash");
                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_Apostrophe");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_«");

                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Plus");
                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_Acute");

                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_ç");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_º");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Tilde");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_LessThan");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (layoutname.Contains("Portuguese (Brazil ABNT"))
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_Apostrophe");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Equals");

                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_Acute");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_LeftBracket");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_ç");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_Tilde");
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_RightBracket");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_BackSlash");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_SemiColon");
                    tested++;
                }

                else if (layoutname == "Turkish Q")
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_DoubleQuote");
                    CheckFrontierToVkey(layoutname, Keys.Oem8, "Key_Asterisk");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");

                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_ğ");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_ü");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_ş");
                    CheckFrontierToVkey(layoutname, Keys.I, "Key_I");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_LessThan");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_ö");
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_ç");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    tested++;
                }

                else if (layoutname == "Swedish")
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_§");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Plus");
                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_Acute");

                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_å");
                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_Umlaut");

                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_ö");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_ä");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Apostrophe");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_LessThan");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (layoutname == "US" || layoutname == "United States-International")
                {

                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_Grave");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Equals");

                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_LeftBracket");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_RightBracket");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_SemiColon");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_Apostrophe");

                    // oem 102 is showing KeyBackslash, same as Oem 5. Table maps it to scan code 56
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_BackSlash");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");  // ok
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");    //ok
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Slash");  //ok
                    tested++;
                }

                else if (layoutname == "United Kingdom")
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem8, "Key_Grave");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Equals");

                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_LeftBracket");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_RightBracket");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_SemiColon");
                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_Apostrophe");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_Hash");

                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_BackSlash");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Slash");
                    tested++;
                }

                else if (layoutname == "German")
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_Circumflex");
                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_ß");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_Acute");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_ü");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Plus");

                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_ö");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_ä");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Hash");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_LessThan");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (layoutname.Contains("Polish"))
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_Grave");

                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Equals");

                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_LeftBracket");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_RightBracket");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_SemiColon");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_Apostrophe");
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_Hash");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_BackSlash");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.OemQuestion, "Key_Slash");
                    tested++;
                }

                else if (layoutname == "Spanish")
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_Grave");
                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_Minus");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_Equals");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_LeftBracket");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_RightBracket");

                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_SemiColon");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_Apostrophe");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Hash");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_BackSlash");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Slash");
                    tested++;
                }

                else if (layoutname.Contains("Belgium"))
                {
                    CheckFrontierToVkey(layoutname, Keys.OemQuotes, "Key_SuperscriptTwo");
                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_RightParenthesis");     // OB
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");

                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_Circumflex");
                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_Dollar");

                    CheckFrontierToVkey(layoutname, Keys.M, "Key_M");
                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_ù");
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_µ");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_LessThan");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_SemiColon");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Colon");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Equals");
                    tested++;
                }


                else if (layoutname.Contains("Canadian French"))
                {
                }
                else if (layoutname.Contains("French"))
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_SuperscriptTwo");
                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_RightParenthesis");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Equals");

                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_Circumflex");
                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_Dollar");

                    CheckFrontierToVkey(layoutname, Keys.M, "Key_M");
                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_ù");
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_Asterisk");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_LessThan");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_SemiColon");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Colon");
                    CheckFrontierToVkey(layoutname, Keys.Oem8, "Key_ExclamationPoint");
                    tested++;
                }

                else if (layoutname.Contains("Italian"))
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_BackSlash");
                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_Apostrophe");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_ì");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_è");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Plus");

                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_ò");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_à");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_ù");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_LessThan");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (layoutname.Contains("Norwegian"))
                {

                    tested++;
                }

                else if (layoutname.Contains("Finnish"))
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_§");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Plus");
                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_Acute");

                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_å");
                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_Umlaut");

                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_ö");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_ä");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Apostrophe");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_LessThan");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (layoutname.Contains("Ukrainian (Enhanced)"))
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_ё");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Equals");

                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_х");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_ї");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_ж");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_є");
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_BackSlash");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_ґ");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_б");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_ю");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Period");
                    tested++;
                }

                else if (layoutname.Contains("Czech")) //rechecked 7th
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_SemiColon");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Equals");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Acute");

                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_ú");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_RightParenthesis");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_ů");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_§");
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_Umlaut");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_BackSlash");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (layoutname.Contains("Greek")) // 7/4/22
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_Grave");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Equals");

                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_LeftBracket");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_RightBracket");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_΄");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_Apostrophe");
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_BackSlash");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_LessThan");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Slash");
                    tested++;
                }


                else if (layoutname.Contains("Lithuanian"))     // 7/4/22
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_Grave");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Underline");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Plus");

                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_į");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_“");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_ų");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_ė");
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_|");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_BackSlash");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_č");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_š");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_ę");
                    tested++;
                }

                else if (layoutname.Contains("Slovak")) // 7/4/22
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_SemiColon");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Equals");
                    CheckFrontierToVkey(layoutname, Keys.Oem8, "Key_Acute");

                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_ú");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_ä");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_ô");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_§");
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_ň");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_Ampersand");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (layoutname.Contains("Slovenian"))  // 7/4/22
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_¸");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Apostrophe");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Plus");

                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_š");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_đ");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_č");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_ć");
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_ž");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_LessThan");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                else if (layoutname.Contains("Romanian (Standard)"))    // 7/4/22
                {
                    CheckFrontierToVkey(layoutname, Keys.Oem3, "Key_RightBracket");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Plus");
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Apostrophe");

                    CheckFrontierToVkey(layoutname, Keys.Oem4, "Key_ă");
                    CheckFrontierToVkey(layoutname, Keys.Oem6, "Key_î");

                    CheckFrontierToVkey(layoutname, Keys.Oem1, "Key_ş");
                    CheckFrontierToVkey(layoutname, Keys.Oem7, "Key_ţ");
                    CheckFrontierToVkey(layoutname, Keys.Oem5, "Key_â");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_LessThan");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.Oem2, "Key_Minus");
                    tested++;
                }
                else if (layoutname == "Danish")
                {
                    CheckFrontierToVkey(layoutname, Keys.OemPipe, "Key_Half");      // oem5
                    CheckFrontierToVkey(layoutname, Keys.Oemplus, "Key_Plus");
                    CheckFrontierToVkey(layoutname, Keys.OemOpenBrackets, "Key_Acute");
                    CheckFrontierToVkey(layoutname, Keys.OemCloseBrackets, "Key_å");
                    CheckFrontierToVkey(layoutname, Keys.OemSemicolon, "Key_Umlaut");
                    CheckFrontierToVkey(layoutname, Keys.Oemtilde, "Key_æ");
                    CheckFrontierToVkey(layoutname, Keys.OemQuotes, "Key_ø");
                    CheckFrontierToVkey(layoutname, Keys.OemQuestion, "Key_Apostrophe");

                    CheckFrontierToVkey(layoutname, Keys.Oem102, "Key_LessThan");
                    CheckFrontierToVkey(layoutname, Keys.Oemcomma, "Key_Comma");
                    CheckFrontierToVkey(layoutname, Keys.OemPeriod, "Key_Period");
                    CheckFrontierToVkey(layoutname, Keys.OemMinus, "Key_Minus");
                    tested++;
                }

                // some are missing I know
                else
                {
                    //   CheckThat(true).IsFalse();      // bad unknown test language
                }
            }

            CheckThat(tested).Is(23);
        }

        static List<string> failed = new List<string>();

        [System.Diagnostics.DebuggerHidden]
        static private void CheckFrontierToVkey(string layoutname, Keys k, string frontierkey)
        {
            //string vkeyname = FrontierKeyConversion.FrontierToKeys(frontierkey);
            //Keys kc = vkeyname.ToVkey();
            //CheckThat(kc).Equals(k);

            string vkeyname2 = FrontierKeyConversion.FrontierToKeys(layoutname,frontierkey);
            Keys kc2 = vkeyname2.ToVkey();
            CheckThat(kc2).Equals(k);

            string frontierback = FrontierKeyConversion.KeysToFrontier(layoutname, vkeyname2);
            if ( frontierback != frontierkey)
            {
                failed.Add(frontierkey);                                            // to create key name tables
                System.Diagnostics.Debug.WriteLine($"{InputLanguage.CurrentInputLanguage.LayoutName} error Key {frontierkey} vkey {vkeyname2} back as {frontierback}");
            }
            CheckThat(frontierback).Equals(frontierkey);
        }
    }
}
