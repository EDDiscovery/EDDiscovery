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

using Conditions;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ActionLanguage
{
    public class ActionPackEditBase : UserControl
    {
        public System.Action RefreshEvent;
        public Condition cd;    // which is a copy of the Program condition, so it can edited

        public virtual void Init(Condition cond, List<string> events, ActionCoreController cp, string appfolder, ActionFile file,
                        System.Func<string, List<string>> func, Icon ic , ToolTip toolTip)
        { }

        public virtual string ID() { return ""; }
        public virtual void UpdateProgramList(string[] proglist) { }

        public virtual new void Dispose()
        {
            base.Dispose();
        }

        public void RefreshIt()
        {
            RefreshEvent?.Invoke();
        }
    }
}