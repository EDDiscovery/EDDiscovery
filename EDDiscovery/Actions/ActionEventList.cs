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
        protected ActionEventEDList(string a, string b, string c) :base(a,b,c)
        {
        }

        public static List<ActionEvent> events = new List<ActionEvent>()
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
            new ActionEventEDList("onEliteUIEvent", "EliteUIEvent", "EliteUI"),  //15
            //TBD for 9.0 new ActionEventEDList("onVoiceInput", "Voice", "Voice"), 

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
        public static ActionEvent onPanelChange { get { return events[13]; } }
        public static ActionEvent onHistorySelection { get { return events[14]; } }
        public static ActionEvent onUIEvent { get { return events[15]; } }
        public static ActionEvent onVoiceInput { get { return events[0]; } }   // TBD for voice, fix index

        public static ActionEvent RefreshJournal(EliteDangerousCore.HistoryEntry he) { return new ActionEventEDList(he.journalEntry.EventTypeStr, "onRefresh", ""); }
        public static ActionEvent NewEntry(EliteDangerousCore.HistoryEntry he) { return new ActionEventEDList(he.journalEntry.EventTypeStr, "NewEntry", ""); }
        public static ActionEvent EventCmd(EliteDangerousCore.HistoryEntry he) { return new ActionEventEDList(he.journalEntry.EventTypeStr, "ActionProgram", ""); }
        public static ActionEvent UserRightClick(EliteDangerousCore.HistoryEntry he) { return new ActionEventEDList(he.journalEntry.EventTypeStr, "UserRightClick", ""); }

        public static List<ActionEvent> EventsFromNames(List<string> alist, string uiclname)
        {
            List<ActionEvent> ae = new List<ActionEvent>();
            foreach (string s in alist)
                ae.Add(new ActionEventEDList(s, "", uiclname));
            return ae;
        }
    }
}
