/*
 * Copyright © 2017 EDDiscovery development team
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
using BaseUtils;
using AudioExtensions;
using ActionLanguage;
using static EliteDangerousCore.BindingsFile;

namespace EDDiscovery.Actions
{
    public class ActionKeyED : ActionKey        // extends Key
    {
        const string errmsgforbinding = "No keyboard binding for ";

        class AKP : BaseUtils.EnhancedSendKeysParser.IAdditionalKeyParser      // AKP parser to pass to SendKeys
        {
            public EliteDangerousCore.BindingsFile bindingsfile;

            public Tuple<string, int, string> Parse(string s)
            {
                if ( s.Length > 0 && s.StartsWith("{"))     // frontier bindings start with decoration
                {
                    int endindex = s.IndexOf("}");
                    if ( endindex>=0 )                      // valid {}
                    {
                        string binding = s.Substring(1, endindex - 1);

                        if (!bindingsfile.KeyNames.Contains(binding))       // first check its a valid name..
                        {
                            return new Tuple<string, int, string>(null, 0, "Binding name " + binding + " is not an known binding");
                        }

                        List<Tuple<Device, Assignment>> matches 
                                    = bindingsfile.FindAssignedFunc(binding, KeyboardDeviceName);   // just give me keyboard bindings, thats all i can do

                        if ( matches != null )      // null if no matches to keyboard is found
                        {
                            // pick out the keys and convert them from text to Vkey (they are in Vkey naming format)
                            Keys[] keys = (from x in matches[0].Item2.keys select x.Key.ToVkey()).ToArray();        // bindings returns keys

                            if ( !keys.Contains(Keys.None)) // if no errors
                            {
                                string keyseq = keys.GenerateSequence();
                               // System.Diagnostics.Debug.WriteLine("Frontier " + binding + "->" + keyseq);
                                return new Tuple<string, int, string>(keyseq, endindex + 1, null);
                            }
                            else
                            {
                                string[] names = (from x in matches[0].Item2.keys select x.Key).ToArray();
                                return new Tuple<string, int, string>(null, 0, "Key name(s) not recognised: " + String.Join(",",names) );
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("No key binding for " + binding);
                            return new Tuple<string, int, string>(null, 0, errmsgforbinding + binding);
                        }
                    }
                }

                return new Tuple<string, int, string>(null, 0, null);
            }
        }

        static public string Menu(Form parent, System.Drawing.Icon ic, string userdata, EliteDangerousCore.BindingsFile bf)
        {
            List<string> decorated = (from x in bf.KeyNames select "{"+x+"}").ToList();
            decorated.Sort();
            return Menu(parent, ic, userdata, decorated, new AKP() { bindingsfile = bf });
        
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<BaseUtils.TypeHelpers.PropertyNameInfo> eventvars)    // override again to expand any functionality
        {
            ActionController ac = cp as ActionController;

            string ud = Menu(parent, cp.Icon, userdata , ac.FrontierBindings );      // base has no additional keys
            if (ud != null)
            {
                userdata = ud;
                return true;
            }
            else
                return false;
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            ActionController ac = ap.actioncontroller as ActionController;
            return ExecuteAction(ap, new AKP() { bindingsfile = ac.FrontierBindings }); //base
        }

        // check binding in userdata for consistency.
        static public string VerifyBinding(string userdata, EliteDangerousCore.BindingsFile bf)    // empty string okay
        {
            string keys;
            Variables statementvars;
            if (FromString(userdata, out keys, out statementvars))
            {
                // during this check, we don't moan about a binding not being present, since we don't need to..

                string ret = BaseUtils.EnhancedSendKeysParser.VerifyKeys(keys, new AKP() { bindingsfile = bf });

                if (ret.Contains(errmsgforbinding))     // Ignore these..
                    return "";
                else
                    return ret;
            }
            else
                return "Bad Key Line";
        }
    }
}
