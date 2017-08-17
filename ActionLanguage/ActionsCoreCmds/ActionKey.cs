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

namespace ActionLanguage
{
    public class ActionKey: ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        //static string toprogram = "To";

        public bool FromString(string s, out string keys, out ConditionVariables vars)
        {
            vars = new ConditionVariables();

            if (s.IndexOfAny(",\"'".ToCharArray()) == -1)       // these not found
            {
                keys = s;
                return true;
            }
            else
            {
                StringParser p = new StringParser(s);
                keys = p.NextQuotedWord(", ");        // stop at space or comma..

                if (keys != null && (p.IsEOL || (p.IsCharMoveOn(',') && vars.FromString(p, ConditionVariables.FromMode.MultiEntryComma))))   // normalise variable names (true)
                    return true;

                keys = "";
                return false;
            }
        }

        public string ToString(string keys, ConditionVariables cond)
        {
            if (cond.Count > 0)
                return keys.QuoteString(comma: true) + ", " + cond.ToString();
            else
                return keys.QuoteString(comma: true);
        }

        public override string VerifyActionCorrect()
        {
            string saying;
            ConditionVariables vars;
            return FromString(userdata, out saying, out vars) ? null : "Key command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string keys;
            ConditionVariables vars;
            FromString(userdata, out keys, out vars);

            return false;
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string keys;
            ConditionVariables statementvars;
            if (FromString(userdata, out keys, out statementvars))
            {
                string errlist = null;
                ConditionVariables vars = statementvars.ExpandAll(ap.functions, statementvars, out errlist);

                if (errlist == null)
                {
                }
                else
                    ap.ReportError(errlist);
            }
            else
                ap.ReportError("Key command line not in correct format");

            return true;
        }

    }
}
