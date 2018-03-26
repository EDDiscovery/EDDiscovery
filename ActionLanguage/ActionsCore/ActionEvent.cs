using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace ActionLanguage
{
    [System.Diagnostics.DebuggerDisplay("Event {TriggerName} {TriggerType} {UIClass}")]
    public class ActionEvent
    {
        public string TriggerName { get; protected set; }             // define an event, a name, a trigger type.  associate for editing with a uiclass
        public string TriggerType { get; protected set; }
        public string UIClass { get; protected set; }

        protected ActionEvent(string n, string c, string u)
        {
            TriggerName = n; TriggerType = c; UIClass = u;
        }

        protected static List<ActionEvent> events = new List<ActionEvent>()
        {
            new ActionEvent("onStartup", "ProgramEvent", "Program"),
            new ActionEvent("onPostStartup","ProgramEvent",  "Program"),
            new ActionEvent("onNonModalDialog", "UserUIEvent", "UI"),
            new ActionEvent("onPlayStarted","ProgramEvent",  "Audio"),
            new ActionEvent("onPlayFinished","ProgramEvent",  "Audio"),
            new ActionEvent("onSayStarted", "ProgramEvent", "Audio"), //5
            new ActionEvent("onSayFinished","ProgramEvent",  "Audio"),
        };

        public static ActionEvent onStartup { get { return events[0]; } }
        public static ActionEvent onPostStartup { get { return events[1]; } }
        public static ActionEvent onNonModalDialog { get { return events[2]; } }
        public static ActionEvent onPlayStarted { get { return events[3]; } }
        public static ActionEvent onPlayFinished { get { return events[4]; } }
        public static ActionEvent onSayStarted { get { return events[5]; } }
        public static ActionEvent onSayFinished { get { return events[6]; } }
    }

}
