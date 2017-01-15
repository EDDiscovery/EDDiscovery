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
        int progmaxcount = 10000;
      
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

        public void Execute()    // MAIN thread only..     
        {
            executing = true;

            System.Diagnostics.Stopwatch timetaken = new System.Diagnostics.Stopwatch();
            timetaken.Start();

            while( true )
            {
                //TBD
                if (progmaxcount > 0 && --progmaxcount == 0 ) { System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " **** PROG EXCEEDED RUN LIMIT" ); TerminateCurrentProgram(); }
                // TBD

                if (progcurrent != null)
                {
                    if (progcurrent.GetErrorList != null)       // any errors pending, handle
                    {
                        discoveryform.LogLine("Error at " + progcurrent.Location + ":" + Environment.NewLine + progcurrent.GetErrorList);
                        TerminateCurrentProgram();              // clear current program..
                    }
                    else if (progcurrent.IsProgramFinished)        // if current program ran out, cancel it
                    {
                        // this catches a LOOP without a statement at the end..  or a DO without a WHILE at the end..
                        if (progcurrent.ExecLevel > 0 && progcurrent.LevelUp(progcurrent.ExecLevel, null)) // see if we have any pending LOOP (or a DO without a while) and continue..
                            continue;       // errors or movement causes it to go back.. errors will be picked up above

                        progcurrent = null;         // cancel it
                    }
                }

                while (progcurrent == null && progqueue.Count > 0)    // if no program,but something in queue
                {
                    progcurrent = progqueue[0];
                    progqueue.RemoveAt(0);

                    if (progcurrent.IsProgramFinished)          // reject empty programs..
                    {
                        progcurrent = null;
                        continue;       // and try again
                    }
                }

                if (progcurrent == null)        // Still nothing, game over
                    break;

                Action ac = progcurrent.GetNextStep();      // get the step. move PC on.

                if (ac.LevelUp > 0 && progcurrent.LevelUp(ac.LevelUp, ac) )        // level up..
                {
                    System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Abort Lv" + progcurrent.ExecLevel + " e " + (progcurrent.IsExecuteOn ? "1" : "0") + " up " + ac.LevelUp + ": " + progcurrent.StepNumber + " " + ac.Name + " " + ac.DisplayedUserData);
                    continue;
                }

                System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Exec  Lv" + progcurrent.ExecLevel + " e " + (progcurrent.IsExecuteOn ? "1" : "0") + " up " + ac.LevelUp + ": " + progcurrent.StepNumber + " " + ac.Name + " " + ac.DisplayedUserData);

                if (progcurrent.DoExecute(ac))       // execute is on.. 
                {
                    if (ac.Type == Action.ActionType.Call)     // Call needs to pass info back up thru to us, need a different call
                    {
                        ActionCall acall = ac as ActionCall;
                        string prog;
                        Dictionary<string, string> paravars;
                        if (acall.ExecuteCallAction(progcurrent, out prog, out paravars)) // if execute ok
                        {
                            System.Diagnostics.Debug.WriteLine("Call " + prog + " with " + ActionVariables.ToString(paravars));

                            Tuple<ActionFile, ActionProgram> ap = actionfilelist.FindProgram(prog, progcurrent.actionfile);          // find program using this name, prefer this action file first

                            if (ap != null)
                            {
                                Dictionary<string, string> callvars = new Dictionary<string, string>(progcurrent.startvars);
                                ActionVariables.AddVars(callvars, paravars);
                                RunNow(ap.Item1, ap.Item2, progcurrent.historylist, progcurrent.historyentry, callvars);
                            }
                            else
                                progcurrent.ReportError("Call cannot find " + prog);
                        }
                    }
                    else if (ac.Type == Action.ActionType.Return)     // Return needs to pass info back up thru to us, need a different call
                    {
                        ActionReturn ar = ac as ActionReturn;
                        string retstr;
                        if ( ar.ExecuteActionReturn(progcurrent,out retstr) )
                        {
                            progcurrent = null;
                            if (progqueue.Count > 0)        // pass return value if program is there..
                                progqueue[0].currentvars["ReturnValue"] = retstr;

                            continue;       // back to top, next action from returned function.
                        }
                    }
                    else if (!ac.ExecuteAction(progcurrent))      // if execute says, stop, i'm waiting for something
                    {
                        return;     // exit, with executing set true.  ResumeAfterPause will restart it.
                    }
                }

                if (async && timetaken.ElapsedMilliseconds > 100)  // no more than 100ms per go to stop the main thread being blocked
                {
                    System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " *** SUSPEND");
                    restarttick.Start();
                    break;
                }
            }

            executing = false;
        }

        private void Tick_Tick(object sender, EventArgs e) // used when async
        {
            restarttick.Stop();
            System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " *** RESUME");
            Execute();
        }

        public void ResumeAfterPause()          // used when async..
        {
            if (executing)
                Execute();
        }

        private void TerminateCurrentProgram()          // stop this program, move onto next
        {
            progcurrent = null;
        }
    }
}
