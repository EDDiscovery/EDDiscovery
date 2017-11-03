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
using Conditions;
using ActionLanguage;

namespace EDDiscovery.Actions
{
    public class ActionKeyED : ActionKey        // extends Key
    {
        static public string Menu(Form parent, System.Drawing.Icon ic, string userdata, EliteDangerousCore.BindingsFile bf)
        {
            // use bf to get new list of stuff, pass thru to null TBD
            List<string> example = new List<string>() { "{one}", "{two}" };
            return Menu(parent, ic, userdata, example, BFParser);
         
        }

        static Tuple<string,int,string> BFParser(string s)
        {
            return new Tuple<string,int,string>(null,0,null);
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)    // override again to expand any functionality
        {
            ActionController ac = cp as ActionController;

            string ud = Menu(parent, cp.Icon, userdata , ac.DiscoveryForm.FrontierBindings );      // base has no additional keys
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
            return ExecuteAction(ap, null); //base, TBD pass in tx funct
        }
    }
}
