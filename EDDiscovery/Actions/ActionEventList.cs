/*
 * Copyright © 2019 EDDiscovery development team
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
using ActionLanguage;

namespace EDDiscovery.Actions
{
    public class ActionEventEDList : ActionLanguage.ActionEvent
    {
        protected ActionEventEDList(string name, string trigtype, string cls, List<BaseUtils.TypeHelpers.PropertyNameInfo> vars) : base(name, trigtype, cls, vars)
        {
        }

        protected static List<ActionEvent> eddevents = new List<ActionEvent>()
        {
            new ActionEventEDList("onInstall", "ProgramEvent", "Program", null
                ),
            new ActionEventEDList("onRefreshStart", "ProgramEvent", "Program",null ),
            new ActionEventEDList("onRefreshEnd", "ProgramEvent", "Program"  , null),
            new ActionEventEDList("onKeyPress", "KeyPress", "UI",
                new List<BaseUtils.TypeHelpers.PropertyNameInfo>()
                {
                    new BaseUtils.TypeHelpers.PropertyNameInfo("KeyPress", "Logical name of key", BaseUtils.ConditionEntry.MatchType.Equals, "Event Variable")
                }
                ),
            new ActionEventEDList("onShutdown", "ProgramEvent", "Program", null ),
            new ActionEventEDList("onMenuItem", "UserUIEvent", "UI",
                new List<BaseUtils.TypeHelpers.PropertyNameInfo>()
                {
                    new BaseUtils.TypeHelpers.PropertyNameInfo("MenuText", "Menu text", BaseUtils.ConditionEntry.MatchType.Contains, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("MenuName", "Logical name given to menu", BaseUtils.ConditionEntry.MatchType.Contains, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("TopLevelMenuName", "Name of top level menu", BaseUtils.ConditionEntry.MatchType.Contains, "Event Variable")
                }
                ),        // 5
            new ActionEventEDList("onTabChange", "UserUIEvent", "UI",
                new List<BaseUtils.TypeHelpers.PropertyNameInfo>()
                {
                    new BaseUtils.TypeHelpers.PropertyNameInfo("TabName", "Tab changed to", BaseUtils.ConditionEntry.MatchType.Contains, "Event Variable"),
                }
                ),
            new ActionEventEDList("onTimer", "ProgramEvent", "Action",
                new List<BaseUtils.TypeHelpers.PropertyNameInfo>()
                {
                    new BaseUtils.TypeHelpers.PropertyNameInfo("TimerName", "Timer which has timed out", BaseUtils.ConditionEntry.MatchType.Equals, "Event Variable")
                }
                ),
            new ActionEventEDList("onEliteInputRaw","EliteUIEvent",  "EliteUI",
                new List<BaseUtils.TypeHelpers.PropertyNameInfo>()
                {
                    new BaseUtils.TypeHelpers.PropertyNameInfo("Device", "Logical device name", BaseUtils.ConditionEntry.MatchType.Contains, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("EventName", "Name of event, Key_<> or Joy_<> or JoyPOV<num><dir> or Joy_<axis>Axis", BaseUtils.ConditionEntry.MatchType.Contains, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("Pressed", "Boolean, If key, is pressed", BaseUtils.ConditionEntry.MatchType.IsTrue, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("Value", "Value of joystick axis or POV direction", BaseUtils.ConditionEntry.MatchType.NumericEquals, "Event Variable"),
                }
                ),
            new ActionEventEDList("onEliteInput", "EliteUIEvent", "EliteUI",
                new List<BaseUtils.TypeHelpers.PropertyNameInfo>()
                {
                    new BaseUtils.TypeHelpers.PropertyNameInfo("Binding", "Logical binding name", BaseUtils.ConditionEntry.MatchType.Contains, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("BindingList", "List of bindings", BaseUtils.ConditionEntry.MatchType.Contains, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("Device", "Logical device name", BaseUtils.ConditionEntry.MatchType.Contains, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("EventName", "Name of event, Key_<> or Joy_<> or JoyPOV<num><dir> or Joy_<axis>Axis", BaseUtils.ConditionEntry.MatchType.Contains, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("Pressed", "Boolean, If key, is pressed", BaseUtils.ConditionEntry.MatchType.IsTrue, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("Value", "Value of joystick axis or POV direction", BaseUtils.ConditionEntry.MatchType.NumericEquals, "Event Variable"),
                }
                ),
            new ActionEventEDList("onEliteInputOff", "EliteUIEvent", "EliteUI",
                new List<BaseUtils.TypeHelpers.PropertyNameInfo>()
                {
                    new BaseUtils.TypeHelpers.PropertyNameInfo("Binding", "Logical binding name, key off", BaseUtils.ConditionEntry.MatchType.Contains, "Event Variable")
                }
                ), //10
            new ActionEventEDList("onPopUp", "UserUIEvent", "UI",
                new List<BaseUtils.TypeHelpers.PropertyNameInfo>()
                {
                    new BaseUtils.TypeHelpers.PropertyNameInfo("PopOutName", "Logical Name of popout", BaseUtils.ConditionEntry.MatchType.Equals, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("PopOutTitle", "Title of popout", BaseUtils.ConditionEntry.MatchType.Equals, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("PopOutIndex", "Index of popout,0 on", BaseUtils.ConditionEntry.MatchType.NumericEquals, "Event Variable"),
                }
                ),
            new ActionEventEDList("onPopDown", "UserUIEvent", "UI",
                new List<BaseUtils.TypeHelpers.PropertyNameInfo>()
                {
                    new BaseUtils.TypeHelpers.PropertyNameInfo("PopOutName", "Logical Name of popout", BaseUtils.ConditionEntry.MatchType.Equals, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("PopOutTitle", "Title of popout", BaseUtils.ConditionEntry.MatchType.Equals, "Event Variable"),
                }
                ), //12
            new ActionEventEDList("onPanelChange", "UserUIEvent", "UI", null),
            new ActionEventEDList("onHistorySelection", "UserUIEvent", "UI", null),
            new ActionEventEDList("onEliteUIEvent", "EliteUIEvent", "UIEvents",
                new List<BaseUtils.TypeHelpers.PropertyNameInfo>()
                {
                    new BaseUtils.TypeHelpers.PropertyNameInfo("EventClass_UIDisplayed", "Indicates if UI events are shown in the journal", BaseUtils.ConditionEntry.MatchType.IsTrue, "Event Variable"),
                }
                ),
            new ActionEventEDList("onEDDNSync", "ProgramEvent", "Program", null ),  //16
            new ActionEventEDList("onIGAUSync", "ProgramEvent", "Program", null ),  //17
            new ActionEventEDList("onEDSMSync", "ProgramEvent", "Program", null ),  //18
            new ActionEventEDList("onVoiceInput", "Voice", "Voice",
                new List<BaseUtils.TypeHelpers.PropertyNameInfo>()
                {
                    new BaseUtils.TypeHelpers.PropertyNameInfo("VoiceInput", "Voice text recognised", BaseUtils.ConditionEntry.MatchType.MatchSemicolonList, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("VoiceConfidence", "% confidence in recognition", BaseUtils.ConditionEntry.MatchType.NumericGreaterEqual, "Event Variable"),
                }
                ), //19
            new ActionEventEDList("onVoiceInputFailed", "Voice", "VoiceOther",
                new List<BaseUtils.TypeHelpers.PropertyNameInfo>()
                {
                    new BaseUtils.TypeHelpers.PropertyNameInfo("VoiceInput", "Voice text not recognised", BaseUtils.ConditionEntry.MatchType.MatchSemicolonList, "Event Variable"),
                    new BaseUtils.TypeHelpers.PropertyNameInfo("VoiceConfidence", "% confidence in recognition", BaseUtils.ConditionEntry.MatchType.NumericGreaterEqual, "Event Variable"),
                }
                ), //19

            new ActionEventEDList("All","","Misc",null),                      // All, special match only
        };

        public static ActionEvent onInstall { get { return eddevents[0]; } }
        public static ActionEvent onRefreshStart { get { return eddevents[1]; } }
        public static ActionEvent onRefreshEnd { get { return eddevents[2]; } }
        public static ActionEvent onKeyPress { get { return eddevents[3]; } }
        public static ActionEvent onShutdown { get { return eddevents[4]; } }
        public static ActionEvent onMenuItem { get { return eddevents[5]; } }
        public static ActionEvent onTabChange { get { return eddevents[6]; } }
        public static ActionEvent onTimer { get { return eddevents[7]; } }
        public static ActionEvent onEliteInputRaw { get { return eddevents[8]; } }
        public static ActionEvent onEliteInput { get { return eddevents[9]; } }
        public static ActionEvent onEliteInputOff { get { return eddevents[10]; } }
        public static ActionEvent onPopUp { get { return eddevents[11]; } }
        public static ActionEvent onPopDown { get { return eddevents[12]; } }
        public static ActionEvent onPanelChange { get { return eddevents[13]; } }          // withdrawn in 10
        public static ActionEvent onHistorySelection { get { return eddevents[14]; } }     // withdrawn in 10
        public static ActionEvent onUIEvent { get { return eddevents[15]; } }
        public static ActionEvent onEDDNSync { get { return eddevents[16]; } }
        public static ActionEvent onIGAUSync { get { return eddevents[17]; } }
        public static ActionEvent onEDSMSync { get { return eddevents[18]; } }
        public static ActionEvent onVoiceInput { get { return eddevents[19]; } }
        public static ActionEvent onVoiceInputFailed { get { return eddevents[20]; } }

        // for events marked with run at refresh, get an HE per entry
        public static ActionEvent RefreshJournal(EliteDangerousCore.HistoryEntry he) { return new ActionEventEDList(he.journalEntry.EventTypeStr, "onRefresh", "",null); }

        // onNewEntry, journal event
        public static ActionEvent NewEntry(EliteDangerousCore.HistoryEntry he) { return new ActionEventEDList(he.journalEntry.EventTypeStr, "NewEntry", "", null); }

        // Event cmd Action - reissue He
        public static ActionEvent EventCmd(EliteDangerousCore.HistoryEntry he) { return new ActionEventEDList(he.journalEntry.EventTypeStr, "ActionProgram", "", null); }

        // UI Event
        public static ActionEvent EliteUIEvent(EliteDangerousCore.UIEvent ui) { return new ActionEventEDList("UI" + ui.EventTypeStr, "EliteUIEvent", "" , null); }

        // DLL Event
        public static ActionEvent DLLEvent(string eventname) { return new ActionEventEDList(eventname, "DLLEvent", "", null); }

        // Journal or Travel grid, run actions on this event
        public static ActionEvent UserRightClick(EliteDangerousCore.HistoryEntry he) { return new ActionEventEDList(he.journalEntry.EventTypeStr, "UserRightClick", "", null); }

        public static List<ActionEvent> EventsFromNames(List<string> alist, string prefix , string triggertype, string uiclname)
        {
            List<ActionEvent> ae = new List<ActionEvent>();
            foreach (string s in alist)
                ae.Add(new ActionEventEDList(prefix + s, triggertype, uiclname, null));
            return ae;
        }

        public static List<ActionEvent> StaticDefinedEvents()       
        {
            List<ActionEvent> be = new List<ActionEvent>(ActionEvent.events);
            be.AddRange(ActionEventEDList.eddevents);
            return be;
        }

        public static List<ActionEvent> EventList(bool excludestatics = false, bool excludejournal = false, bool excludeui = false, bool excludejournaluitranslatedevents = false)
        {
            List<ActionEvent> eventlist = new List<ActionEvent>();

            if (!excludejournal)
            {
                List<string> jevents = EliteDangerousCore.JournalEntry.GetNameOfEvents();
                if (excludejournaluitranslatedevents)
                {
                    jevents.Remove("Music");        // manually synchronise with EDJournalReader . 
                    jevents.Remove("UnderAttack"); 
                    jevents.Remove("FSDTarget");   
                }

                jevents.Sort();
                eventlist.AddRange(ActionEventEDList.EventsFromNames(jevents, "", "NewEntry", "Journal"));      // presume NewEntry as its the most prevalent.  Trigger type is not used in this circumstance except in Perform
            }

            if ( !excludeui )
            {
                List<string> uievents = Enum.GetNames(typeof(EliteDangerousCore.UITypeEnum)).ToList();
                uievents.Sort();
                eventlist.AddRange(ActionEventEDList.EventsFromNames(uievents, "UI" , "EliteUIEvent", "UIEvents"));
            }

            if (!excludestatics)
            {
                eventlist.AddRange(ActionEventEDList.StaticDefinedEvents());
            }

            return eventlist;
        }
    }
}
