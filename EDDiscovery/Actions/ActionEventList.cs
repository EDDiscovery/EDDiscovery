using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionLanguage;

namespace EDDiscovery.Actions
{
    public class ActionEventEDList : ActionLanguage.ActionEvent
    {
        protected ActionEventEDList(string name, string trigtype, string cls) :base(name, trigtype, cls)
        {
        }

        protected new static List<ActionEvent> events = new List<ActionEvent>()
        {
            new ActionEventEDList("onInstall", "ProgramEvent", "Program"),
            new ActionEventEDList("onRefreshStart", "ProgramEvent", "Program"),
            new ActionEventEDList("onRefreshEnd", "ProgramEvent", "Program"),
            new ActionEventEDList("onKeyPress", "KeyPress", "UI"),
            new ActionEventEDList("onShutdown", "ProgramEvent", "Program"),
            new ActionEventEDList("onMenuItem", "UserUIEvent", "UI"),        // 5
            new ActionEventEDList("onTabChange", "UserUIEvent", "UI"),
            new ActionEventEDList("onTimer", "ProgramEvent", "Action"),
            new ActionEventEDList("onEliteInputRaw","EliteUIEvent",  "EliteUI"),
            new ActionEventEDList("onEliteInput", "EliteUIEvent", "EliteUI"), 
            new ActionEventEDList("onEliteInputOff", "EliteUIEvent", "EliteUI"), //10
            new ActionEventEDList("onPopUp", "UserUIEvent", "UI"),
            new ActionEventEDList("onPopDown", "UserUIEvent", "UI"), //12
            new ActionEventEDList("onPanelChange", "UserUIEvent", "UI"),
            new ActionEventEDList("onHistorySelection", "UserUIEvent", "UI"),  //14
            new ActionEventEDList("onEliteUIEvent", "EliteUIEvent", "UIEvents"),  //15
            new ActionEventEDList("onEDDNSync", "ProgramEvent", "Program"),  //16
            new ActionEventEDList("onEGOSync", "ProgramEvent", "Program"),  //17
            new ActionEventEDList("onEDSMSync", "ProgramEvent", "Program"),  //18
            new ActionEventEDList("onVoiceInput", "Voice", "Voice"), //19
            new ActionEventEDList("onVoiceInputFailed", "Voice", "VoiceOther"), //19

            new ActionEventEDList("All","","Misc"),                      // All, special match only
        };

        public static ActionEvent onInstall { get { return events[0]; } }
        public static ActionEvent onRefreshStart { get { return events[1]; } }
        public static ActionEvent onRefreshEnd { get { return events[2]; } }
        public static ActionEvent onKeyPress { get { return events[3]; } }
        public static ActionEvent onShutdown { get { return events[4]; } }
        public static ActionEvent onMenuItem { get { return events[5]; } }
        public static ActionEvent onTabChange { get { return events[6]; } }
        public static ActionEvent onTimer { get { return events[7]; } }
        public static ActionEvent onEliteInputRaw { get { return events[8]; } }
        public static ActionEvent onEliteInput { get { return events[9]; } }
        public static ActionEvent onEliteInputOff { get { return events[10]; } }
        public static ActionEvent onPopUp { get { return events[11]; } }
        public static ActionEvent onPopDown { get { return events[12]; } }
        public static ActionEvent onPanelChange { get { return events[13]; } }          // withdrawn in 10
        public static ActionEvent onHistorySelection { get { return events[14]; } }     // withdrawn in 10
        public static ActionEvent onUIEvent { get { return events[15]; } }
        public static ActionEvent onEDDNSync { get { return events[16]; } }
        public static ActionEvent onEGOSync { get { return events[17]; } }
        public static ActionEvent onEDSMSync { get { return events[18]; } }
        public static ActionEvent onVoiceInput { get { return events[19]; } }
        public static ActionEvent onVoiceInputFailed { get { return events[20]; } }

        // for events marked with run at refresh, get an HE per entry
        public static ActionEvent RefreshJournal(EliteDangerousCore.HistoryEntry he) { return new ActionEventEDList(he.journalEntry.EventTypeStr, "onRefresh", ""); }

        // onNewEntry, journal event
        public static ActionEvent NewEntry(EliteDangerousCore.HistoryEntry he) { return new ActionEventEDList(he.journalEntry.EventTypeStr, "NewEntry", ""); }

        // Event cmd Action - reissue He
        public static ActionEvent EventCmd(EliteDangerousCore.HistoryEntry he) { return new ActionEventEDList(he.journalEntry.EventTypeStr, "ActionProgram", ""); }

        // UI Event
        public static ActionEvent EliteUIEvent(EliteDangerousCore.UIEvent ui) { return new ActionEventEDList("UI" + ui.EventTypeStr, "EliteUIEvent", ""); }

        // DLL Event
        public static ActionEvent DLLEvent(string eventname) { return new ActionEventEDList(eventname, "DLLEvent", ""); }

        // Journal or Travel grid, run actions on this event
        public static ActionEvent UserRightClick(EliteDangerousCore.HistoryEntry he) { return new ActionEventEDList(he.journalEntry.EventTypeStr, "UserRightClick", ""); }

        public static List<ActionEvent> EventsFromNames(List<string> alist, string prefix , string triggertype, string uiclname)
        {
            List<ActionEvent> ae = new List<ActionEvent>();
            foreach (string s in alist)
                ae.Add(new ActionEventEDList(prefix + s, triggertype, uiclname));
            return ae;
        }

        public static List<ActionEvent> StaticDefinedEvents()       
        {
            List<ActionEvent> be = new List<ActionEvent>(ActionEvent.events);
            be.AddRange(ActionEventEDList.events);
            return be;
        }

        public static List<ActionEvent> EventList(bool excludestatics = false, bool excludejournal = false, bool excludeui = false)
        {
            List<ActionEvent> eventlist = new List<ActionEvent>();

            if (!excludejournal)
            {
                List<string> jevents = EliteDangerousCore.JournalEntry.GetNameOfEvents();
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
