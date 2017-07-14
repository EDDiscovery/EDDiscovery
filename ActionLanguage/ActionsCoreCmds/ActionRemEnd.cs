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

namespace ActionLanguage
{
    public class ActionRem : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }
        public override bool ConfigurationMenuInUse { get { return false; } }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            return true;
        }
    }

    public class ActionFullLineComment : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }
        public override bool ConfigurationMenuInUse { get { return false; } }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            return true;
        }
    }

    public class ActionEnd : ActionBase
    {
        public override bool ConfigurationMenuInUse { get { return false; } }
        public override string DisplayedUserData { get { return null; } }        // null if you dont' want to display

        public override string VerifyActionCorrect()
        {
            return (UserData.Length == 0) ? null : " Text after end is not allowed";
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            ap.TerminateCurrentProgram();
            return true;
        }
    }
}

