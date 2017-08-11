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
        public Condition cd;

        public ActionFile actionfile;

        public ActionCoreController actioncorecontroller;
        public string applicationfolder;
        public System.Drawing.Icon Icon;

        public System.Action<ActionPackEditBase> RemoveItem;
        public System.Action RefreshEvent;

        public const int panelxmargin = 3;
        public const int panelymargin = 1;

        protected ActionPackEditorForm.AdditionalNames onAdditionalNames;

        public void Init(Condition cond, List<string> events, ActionCoreController cp, string appfolder, ActionFile file, ActionPackEditorForm.AdditionalNames func, Icon ic)
        {
            Icon = ic;
            onAdditionalNames = func;
            cd = new Condition(cond);        // full clone, we can now modify it.
            actionfile = file;
            actioncorecontroller = cp;
            applicationfolder = appfolder;
            InitSub(events);
        }

        public virtual string ID() { return ""; }
        public virtual void InitSub(List<string> events) { }
        public virtual void EventChanged() { }
        public virtual void UpdateProgramList(string[] proglist) { }

        public virtual new void Dispose()
        {
            base.Dispose();
        }

        public void RefreshIt()
        {
            RefreshEvent?.Invoke();
        }

        public void RemoveIt()
        {
            RemoveItem?.Invoke(this);
        }
    }
}