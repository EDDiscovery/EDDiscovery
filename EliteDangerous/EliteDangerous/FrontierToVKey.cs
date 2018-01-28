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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EliteDangerousCore
{
    static public class FrontierKeyConversion
    {
        static private Dictionary<string,uint> nametoscan = null;
        static private bool mapped = false;

        // lovely frontier uses strange naming.. its dependent on input language, except for some locals (ESP) it isn't.
        // do this is the best i can do.. tested UK,USA,FR,DE,IT,ESP,POL.
        // will probably need more bodging.

        static public string FrontierToKeys(string frontiername)
        {
            if (nametoscan == null)
            {
                Dictionary<char, uint> chartoscancode = KeyObjectExtensions.CharToScanCode();
                //ScancodeToFrontierName(chartoscancode);
                nametoscan = FrontierNameToScancode(chartoscancode);
                mapped = IsFullyMapped(chartoscancode);
                //System.Diagnostics.Debug.WriteLine("Mapped " + mapped);
            }

            string output;

            if (frontiername.StartsWith("Key_"))
            {
                output = frontiername.Substring(4);

                int num;

                if (output.Length == 1 && ((output[0] >= '0' && output[0] <= '9') || (output[0] >= 'A' && output[0] <= 'Z')))
                {
                    // no action
                }
                else if (output.StartsWith("Numpad_") && char.IsDigit(output[7]))
                    output = "NumPad" + output[7];
                else if (output.StartsWith("F") && int.TryParse(output.Substring(1), out num))
                    output = "F" + num;
                else
                {
                    int i = Array.FindIndex(frontiertovkeyname, x => x.Item2.Equals(output));

                    if (i >= 0)
                        return frontiertovkeyname[i].Item1;

                    uint sc = 0;

//                    System.Diagnostics.Debug.WriteLine("Layout " + InputLanguage.CurrentInputLanguage.LayoutName);

                    if (InputLanguage.CurrentInputLanguage.LayoutName != "Spanish" && mapped && nametoscan.ContainsKey(output))       // spanish uses default names
                        sc = nametoscan[output];
                    else
                    {
                        i = Array.FindIndex(defaultscancodes, x => x.Item1.Equals(output));

                        if (i >= 0)
                            sc = defaultscancodes[i].Item2;
                    }

                    if (sc != 0)      // if we have a code, convert it to a Vkey.
                    {
                        // System.Diagnostics.Debug.WriteLine("Name {0} ->SC {1:x}", output, sc);

                        uint v = BaseUtils.Win32.UnsafeNativeMethods.MapVirtualKey(sc, 3);

                        if (v != 0)
                        {
                            // System.Diagnostics.Debug.WriteLine("        .. {0} -> VK {1:x} {2}", sc, v, ((Keys)v).VKeyToString());
                            output = ((Keys)v).VKeyToString();
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Scan code converstion failed {0} {1:x}", frontiername , sc);
                            output = "Unknown_SCMap_" + frontiername + "_" + InputLanguage.CurrentInputLanguage.LayoutName;
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Frontier Mapping failed {0}", frontiername);
                        output = "Unknown_Key_" + frontiername + "_" + InputLanguage.CurrentInputLanguage.LayoutName;
                    }
                }
            }
            else
                output = "Unknown_Format_" + frontiername;

            return output;

        }

        static Tuple<string, string>[] frontiertovkeyname = new Tuple<string, string>[]
        {
            new Tuple<string,string>(Keys.Up.VKeyToString()          ,"UpArrow"),          // FD naming
            new Tuple<string,string>(Keys.Down.VKeyToString()        ,"DownArrow"),
            new Tuple<string,string>(Keys.Left.VKeyToString()        ,"LeftArrow"),
            new Tuple<string,string>(Keys.Right.VKeyToString()       ,"RightArrow"),
            new Tuple<string,string>(Keys.Return.VKeyToString()      ,"Enter"),
            new Tuple<string,string>(Keys.Capital.VKeyToString()     ,"CapsLock"),
            new Tuple<string,string>(Keys.NumLock.VKeyToString()     ,"NumLock"),
            new Tuple<string,string>(Keys.Subtract.VKeyToString()    ,"Numpad_Subtract"),
            new Tuple<string,string>(Keys.Divide.VKeyToString()      ,"Numpad_Divide"),
            new Tuple<string,string>(Keys.Multiply.VKeyToString()    ,"Numpad_Multiply"),
            new Tuple<string,string>(Keys.Add.VKeyToString()         ,"Numpad_Add"),
            new Tuple<string,string>(Keys.Decimal.VKeyToString()     ,"Numpad_Decimal"),
            new Tuple<string,string>(Keys.Insert.VKeyToString()     ,"Insert"),
            new Tuple<string,string>(Keys.Home.VKeyToString()     ,"Home"),
            new Tuple<string,string>(Keys.PageUp.VKeyToString()     ,"PageUp"),
            new Tuple<string,string>(Keys.Delete.VKeyToString()     ,"Delete"),
            new Tuple<string,string>(Keys.End.VKeyToString()     ,"End"),
            new Tuple<string,string>(Keys.PageDown.VKeyToString()     ,"PageDown"),
            new Tuple<string,string>("NumEnter", "Numpad_Enter"),
            new Tuple<string,string>(Keys.Space.VKeyToString(), "Space"),
            new Tuple<string,string>(Keys.Tab.VKeyToString(), "Tab"),

            new Tuple<string,string>(Keys.LShiftKey.VKeyToString(),"LeftShift"),
            new Tuple<string,string>(Keys.LControlKey.VKeyToString(),"LeftControl"),
            new Tuple<string,string>(Keys.LMenu.VKeyToString(),"LeftAlt"),
            new Tuple<string,string>(Keys.RShiftKey.VKeyToString(),"RightShift"),
            new Tuple<string,string>(Keys.RControlKey.VKeyToString(),"RightControl"),
            new Tuple<string,string>(Keys.RMenu.VKeyToString(),"RightAlt"),
            new Tuple<string,string>(Keys.Back.VKeyToString(),"Backspace"),
         };

        static Tuple<string, uint>[] defaultscancodes = new Tuple<string, uint>[]       // used on some layouts (ESP) instead of local names.. no idea how its chosen
        {
            new Tuple<string, uint>("Grave",0x29),

            new Tuple<string, uint>("Minus",0x0c),
            new Tuple<string, uint>("Equals",0x0d),

            new Tuple<string, uint>("LeftBracket",0x1a),
            new Tuple<string, uint>("RightBracket",0x1b),

            new Tuple<string, uint>("SemiColon",0x27),
            new Tuple<string, uint>("Apostrophe",0x28),
            new Tuple<string, uint>("Hash",0x2b),

            new Tuple<string, uint>("Comma",0x33),
            new Tuple<string, uint>("Period",0x34),
            new Tuple<string, uint>("Slash",0x35),

            new Tuple<string, uint>("BackSlash",0x56),

        };


        static public Dictionary<uint, string> ScancodeToFrontierName(Dictionary<char, uint> chartosc)  // list of scan codes vs frontier names
        {
            Dictionary<uint, string> scn = new Dictionary<uint, string>();

            foreach (KeyValuePair<char, uint> kv in chartosc)
            {
                if ((kv.Value & 0xff) == kv.Value)
                {
                    string str = "";
                    str += kv.Key;

                    int i = Array.FindIndex(chartofrontiername, x => x.Item1.Equals(str));

                    if (i >= 0)
                        str = chartofrontiername[i].Item2;

                    System.Diagnostics.Debug.WriteLine("Scan code {0:x} = '{1}'", kv.Value, str);
                }
            }

            return scn;
        }

        static public bool IsFullyMapped(Dictionary<char, uint> chartosc)   // tell me if you have these keys in the name table
        {
            bool fullymapped = chartosc.ContainsValue(0x29) && chartosc.ContainsValue(0xc) && chartosc.ContainsValue(0xd)
                            && chartosc.ContainsValue(0x1a) && chartosc.ContainsValue(0x1b) && chartosc.ContainsValue(0x27)
                            && chartosc.ContainsValue(0x28) && chartosc.ContainsValue(0x2b) && chartosc.ContainsValue(0x33)
                            && chartosc.ContainsValue(0x34) && chartosc.ContainsValue(0x35);

            // USA does not support 0x56, so we don't call that a fully mapped key fullymapped = fullymapped && chartosc.ContainsValue(0x56);

            return fullymapped;
        }

        static public Dictionary<string, uint> FrontierNameToScancode(Dictionary<char, uint> chartosc)  // given a name, give me the scan code..
        {
            Dictionary<string, uint> scn = new Dictionary<string, uint>();

            foreach (KeyValuePair<char, uint> kv in chartosc)
            {
                if ((kv.Value & 0xff) == kv.Value)      // if not shifted..
                {
                    string str = "";
                    str += kv.Key;

                    int i = Array.FindIndex(chartofrontiername, x => x.Item1.Equals(str));

                    if (i >= 0)
                    {
                        str = chartofrontiername[i].Item2;
                    }

                   // System.Diagnostics.Debug.WriteLine("'{0}' Scan code {1:x}", str, kv.Value);
                    scn[str] = kv.Value;
                }
            }


            return scn;
        }

        static Tuple<string, string>[] chartofrontiername = new Tuple<string, string>[]
        {
        new Tuple<string,string>("`","Grave"),
        new Tuple<string,string>("-","Minus"),
        new Tuple<string,string>("=","Equals"),
        new Tuple<string,string>("[","LeftBracket"),
        new Tuple<string,string>("]","RightBracket"),
        new Tuple<string,string>(";","SemiColon"),
        new Tuple<string,string>("'","Apostrophe"),
        new Tuple<string,string>("#","Hash"),
        new Tuple<string,string>(",","Comma"),
        new Tuple<string,string>(".","Period"),
        new Tuple<string,string>("/","Slash"),
        new Tuple<string,string>("\\","BackSlash"),

        // DE 28-12-2017 
        new Tuple<string,string>("^","Circumflex"),
        //ß
        new Tuple<string,string>("∩","Acute"),
        //ü
        new Tuple<string,string>("+","Plus"),
        //ö,ä
        new Tuple<string,string>("#","Hash"),
        //Comma,Period,Minus
        new Tuple<string,string>("<","LessThan"),
            
        // FR 29 12 2017
        new Tuple<string,string>("²","SuperscriptTwo"),
        new Tuple<string,string>(")","RightParenthesis"),
        //= Equals
        //^ Cirumflex
        new Tuple<string,string>("$","Dollar"),
        //m auto
        //ù auto
        new Tuple<string,string>("*","Asterisk"),
        //Semicolon
        new Tuple<string,string>(":","Colon"),
        new Tuple<string,string>("!","ExclamationPoint"),

            // IT 29 12 2017
            //Backslash
            //Apostrophe
            //ì auto
            //è
            //+
            //ò
            //à
            //ù auto
            //Comma
            //Period 
            //Minus
            //LessThan
        };

#if DEBUG

        static public void Check()
        {
            InputLanguage l = InputLanguage.CurrentInputLanguage;
            if (l.Culture.Name.Contains("es-"))
                CheckESP();
            if (l.Culture.Name.Contains("en-GB"))
                CheckUKUS(false);
            if (l.Culture.Name.Contains("en-US"))
                CheckUKUS(true);
            if (l.Culture.Name.Contains("fr-"))
                CheckFR();
            if (l.Culture.Name.Contains("it-"))
                CheckIT();
            if (l.Culture.Name.Contains("de-"))
                CheckDE();
            if (l.Culture.Name.Contains("pl-"))
                CheckPOL();
        }

        static public void CheckUKUS(bool usa)
        {
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
            
            if ( usa )
            {
                Check(Keys.Oemtilde, "Key_Grave");

                Check(Keys.OemMinus, "Key_Minus");
                Check(Keys.Oemplus, "Key_Equals");

                Check(Keys.OemOpenBrackets, "Key_LeftBracket");
                Check(Keys.Oem6, "Key_RightBracket");

                Check(Keys.Oem1, "Key_SemiColon");
                Check(Keys.Oem7, "Key_Apostrophe");
                Check(Keys.Oem5, "Key_BackSlash");

                Check(Keys.Oemcomma, "Key_Comma");
                Check(Keys.OemPeriod, "Key_Period");
                Check(Keys.OemQuestion, "Key_Slash");

               // No SC 56 on USA Check(Keys.OemBackslash, "Key_BackSlash");
            }
            else
            {
                Check(Keys.Oem8, "Key_Grave");

                Check(Keys.OemMinus, "Key_Minus");
                Check(Keys.Oemplus, "Key_Equals");

                Check(Keys.Oem4, "Key_LeftBracket");
                Check(Keys.Oem6, "Key_RightBracket");

                Check(Keys.Oem1, "Key_SemiColon");
                Check(Keys.Oem3, "Key_Apostrophe");
                Check(Keys.Oem7, "Key_Hash");

                Check(Keys.Oemcomma, "Key_Comma");
                Check(Keys.OemPeriod, "Key_Period");
                Check(Keys.Oem2, "Key_Slash");

                Check(Keys.Oem5, "Key_BackSlash");
            }
        }

        static public void CheckESP()
        {
            Check(Keys.Oem5, "Key_Grave");

            Check(Keys.OemOpenBrackets, "Key_Minus");
            Check(Keys.Oem6, "Key_Equals");

            Check(Keys.Oem1, "Key_LeftBracket");
            Check(Keys.Oemplus, "Key_RightBracket");

            Check(Keys.Oemtilde, "Key_SemiColon");
            Check(Keys.Oem7, "Key_Apostrophe");
            Check(Keys.OemQuestion, "Key_Hash");

            Check(Keys.Oemcomma, "Key_Comma");
            Check(Keys.OemPeriod, "Key_Period");
            Check(Keys.OemMinus, "Key_Slash");

            Check(Keys.OemBackslash, "Key_BackSlash");

        }

        static public void CheckPOL()
        {
            Check(Keys.Oemtilde, "Key_Grave");

            Check(Keys.Oemplus, "Key_Minus");
            Check(Keys.OemQuestion, "Key_Equals");

            Check(Keys.OemOpenBrackets, "Key_LeftBracket");
            Check(Keys.Oem6, "Key_RightBracket");

            Check(Keys.Oem1, "Key_SemiColon");
            Check(Keys.Oem7, "Key_Apostrophe");
            Check(Keys.Oem5, "Key_Hash");  

            Check(Keys.Oemcomma, "Key_Comma");
            Check(Keys.OemPeriod, "Key_Period");
            Check(Keys.OemMinus, "Key_Slash");

            Check(Keys.OemBackslash, "Key_BackSlash");

        }

        static public void CheckDE()
        {
            Check(Keys.Oem5, "Key_Circumflex");
            Check(Keys.OemOpenBrackets, "Key_ß");
            Check(Keys.Oem6, "Key_Acute");
            Check(Keys.Oem1, "Key_ü");
            Check(Keys.Oemplus, "Key_Plus");
            Check(Keys.Oemtilde, "Key_ö");
            Check(Keys.Oem7, "Key_ä");
            Check(Keys.OemQuestion, "Key_Hash");
            Check(Keys.Oemcomma, "Key_Comma");
            Check(Keys.OemPeriod, "Key_Period");
            Check(Keys.OemMinus, "Key_Minus");
            Check(Keys.OemBackslash, "Key_LessThan");

        }

        static public void CheckFR()
        {
            Check(Keys.Oem7, "Key_SuperscriptTwo");
            Check(Keys.OemOpenBrackets, "Key_RightParenthesis");
            Check(Keys.Oemplus, "Key_Equals");
            Check(Keys.Oem6, "Key_Circumflex");
            Check(Keys.Oem1, "Key_Dollar");
            Check(Keys.M, "Key_M");
            Check(Keys.Oemtilde, "Key_ù");
            Check(Keys.Oem5, "Key_Asterisk");
            Check(Keys.OemPeriod, "Key_SemiColon");
            Check(Keys.OemQuestion, "Key_Colon");
            Check(Keys.Oem8, "Key_ExclamationPoint");
            Check(Keys.OemBackslash, "Key_LessThan");

        }

        static public void CheckIT()
        {
            Check(Keys.Oem5, "Key_BackSlash");

            Check(Keys.OemOpenBrackets, "Key_Apostrophe");
            Check(Keys.Oem6, "Key_ì");

            Check(Keys.Oem1, "Key_è");
            Check(Keys.Oemplus, "Key_Plus");

            Check(Keys.Oemtilde, "Key_ò");
            Check(Keys.Oem7, "Key_à");
            Check(Keys.OemQuestion, "Key_ù");

            Check(Keys.Oemcomma, "Key_Comma");
            Check(Keys.OemMinus, "Key_Minus");

            Check(Keys.OemBackslash, "Key_LessThan");

        }

        static private void Check(Keys k, string key)
        {
            string output = FrontierToKeys(key);
            Keys kc = output.ToVkey();
            string check = "";
            if (kc != k)
                check = "********** ERROR";

            System.Diagnostics.Debug.WriteLine("     Key {0} => {1} {2} {3}" + Environment.NewLine, key, output, kc, check);
        }
#endif

    }

}
