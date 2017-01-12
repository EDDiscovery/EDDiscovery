using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionRun
    {
        private List<ActionProgramRun> progqueue = new List<ActionProgramRun>();
        private ActionProgramRun progcurrent = null;

        private EDDiscoveryForm discoveryform = null;
        private ActionFileList actionfilelist = null;

        bool async = false;             // if this Action is an asynchoronous object
        bool executing = false;         // Records is executing
      
        Timer restarttick = new Timer();

        public ActionRun(EDDiscoveryForm ed, ActionFileList afl, bool asy)
        {
            restarttick.Interval = 100;
            restarttick.Tick += Tick_Tick;
            discoveryform = ed;
            actionfilelist = afl;
            async = asy;
        }

        //historyentry may be null if not associated with a entry

        static public void StandardVars(string trigname, Dictionary<string, string> vars, HistoryEntry he)
        {
            vars["TriggerName"] = trigname;       // Program gets eventname which triggered it..
            vars["CurrentLocalTime"] = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");  // time it was started, US format, to match JSON.
            vars["CurrentUTCTime"] = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss");  // time it was started, US format, to match JSON.
            vars["CurrentCulture"] = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            vars["CurrentCultureInEnglish"] = System.Threading.Thread.CurrentThread.CurrentCulture.EnglishName;
            vars["CurrentCultureISO"] = System.Threading.Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName;

            if (he != null)
            {
                vars["DockedState"] = he.IsDocked ? "1" : "0";
                vars["LandedState"] = he.IsLanded ? "1" : "0";
                vars["StarSystem"] = he.System.name;
                vars["JournalID"] = he.Journalid.ToString();
            }
        }

        // standard add, with multiple var lists, and with optional he, and with standard additional vars..
        public void StandardAdd(ActionFile fileset, string trigname , ActionProgram r, HistoryList hl, HistoryEntry he, List<Dictionary<string, string>> varlist)
        {
            Dictionary<string, string> vars = new Dictionary<string, string>();
            ActionData.AddVars(vars, varlist);
            StandardVars(trigname, vars, he);
            Add(fileset, r, hl, he, vars);
        }

        // v is copied, so the program won't change v.
        // WE take a copy of each program, so each invocation of program action has a unique instance in case
        // it has private variables in either action or program.
        public void Add(ActionFile fileset, ActionProgram r, HistoryList hl, HistoryEntry h, Dictionary<string, string> v)
        {
            progqueue.Add(new ActionProgramRun(fileset, r, this, discoveryform, hl, h, v, async));
        }

        public void RunNow(ActionFile fileset, ActionProgram r, HistoryList hl, HistoryEntry h, Dictionary<string, string> v)
        {
            if (progcurrent != null)                    // if running, push the current one back onto the queue to be picked up
                progqueue.Insert(0, progcurrent);

            progcurrent = new ActionProgramRun(fileset, r, this, discoveryform, hl, h, v, async);   // now we run this.. no need to push to stack
        }

        public bool Call( string name, string parameters, HistoryList hl, HistoryEntry h, Dictionary<string,string> currentvars )      // inside ExecuteAction, will do GetNextAction next..
        {
            // push name to front of queue..  decode parameters to dictionary, add to list, change progcurrent to it.
            //when progcurrent finished, it should resume at the caller at the next steppoint.
            return false;
        }

        public void Execute()    // MAIN thread only..     
        {
            executing = true;

            Action ac;

            System.Diagnostics.Stopwatch timetaken = new System.Diagnostics.Stopwatch();
            timetaken.Start();

            int debugmax = 1000;

            while ((ac = GetNextAction()) != null)
            {
//TBD
                if ( debugmax-- < 1 )  { TerminateCurrentProgram(); continue; }
                // TBD
                if (ac.LevelUp > 0 && progcurrent.LevelUp(ac))          // level up, if it returns true, it has changed position, so go back and get next
                {
                    System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Abort Lv" + progcurrent.ExecLevel + " e " + (progcurrent.IsExecuteOn ? "1" : "0") + " up " + ac.LevelUp + ": " + progcurrent.StepNumber + " " + ac.Name + " " + ac.DisplayedUserData);
                    continue;
                }

                System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Exec  Lv" + progcurrent.ExecLevel + " e " + (progcurrent.IsExecuteOn ? "1" : "0") + " up " + ac.LevelUp + ": " + progcurrent.StepNumber + " " + ac.Name + " " + ac.DisplayedUserData);

                if (progcurrent.GetErrorList != null)       // any errors due to above.. get next statement from next program
                {
                    discoveryform.LogLine("Error at " + progcurrent.Location + ":" + Environment.NewLine + progcurrent.GetErrorList);
                    TerminateCurrentProgram();
                    continue; // get next program
                }

                if (progcurrent.DoExecute(ac))       // execute is on.. 
                {
                    if ( ac.Type == Action.ActionType.Call)     // Call needs to pass info back up thru to us, need a different call
                    {
                        ActionCall acall = ac as ActionCall;
                        string prog;
                        Dictionary<string, string> paravars;
                        if ( acall.ExecuteCallAction(progcurrent, out prog, out paravars) ) // if execute ok
                        {
                            System.Diagnostics.Debug.WriteLine("Call " + prog + " with " + ActionData.ToString(paravars));

                            Tuple<ActionFile, ActionProgram> ap = actionfilelist.FindProgram(prog, progcurrent.actionfile);          // find program using this name, prefer this action file first

                            if (ap != null)
                            {
                                Dictionary<string, string> callvars = new Dictionary<string, string>(progcurrent.startvars);
                                ActionData.AddVars(callvars, paravars);
                                RunNow(ap.Item1, ap.Item2, progcurrent.historylist, progcurrent.historyentry, callvars);
                            }
                            else
                                progcurrent.ReportError("Call cannot find " + prog);
                        }
                    }
                    else if (!ac.ExecuteAction(progcurrent))      // if execute says, stop, i'm waiting for something
                    {
                        return;     // exit, with executing set true.  ResumeAfterPause will restart it.
                    }

                    if (progcurrent.GetErrorList != null)
                    {
                        discoveryform.LogLine("Error at " + progcurrent.Location + ":" + Environment.NewLine + progcurrent.GetErrorList);
                        TerminateCurrentProgram();
                    }
                }

                //if ( async && timetaken.ElapsedMilliseconds > 10000000000000 )  // no more than 100ms per go to stop the main thread being blocked
                //{
                //    restarttick.Start();
                  //  break;
                //}
            }

            executing = false;
        }

        private void Tick_Tick(object sender, EventArgs e) // used when async
        {
            restarttick.Stop();
            Execute();
        }

        public void ResumeAfterPause()          // used when async..
        {
            if (executing)
                Execute();
        }

        private Action GetNextAction()
        {
            if (progcurrent != null && progcurrent.IsProgramFinished)        // if current program ran out, cancel it
                progcurrent = null;

            while (progcurrent == null && progqueue.Count > 0)    // if no program,but something in queue
            {
                progcurrent = progqueue[0];
                progqueue.RemoveAt(0);

                if (progcurrent.IsProgramFinished)          // reject empty programs..
                    progcurrent = null;
            }

            return (progcurrent != null) ? progcurrent.GetNextStep() : null;
        }

        private void TerminateCurrentProgram()          // stop this program, move onto next
        {
            progcurrent = null;
        }
    }
}
