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
using Conditions;

namespace ActionLanguage
{
    // this class allows programs to be run, either sync or async..

    public class ActionRun
    {
        public bool AsyncMode { get; set; } = true;         // set this for non async mode -debug only

        private BaseUtils.LogToFile logger = null;
        private bool logtologline = false;
        
        private List<ActionProgramRun> progqueue = new List<ActionProgramRun>();
        private ActionProgramRun progcurrent = null;

        private ActionCoreController actioncontroller = null;
        private ActionFileList actionfilelist = null;

        private bool executing = false;         // Records is executing
        private Timer restarttick = new Timer();

        public ActionRun(ActionCoreController ed, ActionFileList afl)
        {
            restarttick.Interval = 100;
            restarttick.Tick += Tick_Tick;
            actioncontroller = ed;
            actionfilelist = afl;
        }

        // now = true, run it immediately, else run at end of queue.  Optionally pass in handles and dialogs in case its a sub prog

        public void Run(bool now, ActionFile fileset, ActionProgram r, ConditionVariables inputparas,
                                ConditionPersistentData fh = null, Dictionary<string, ExtendedControls.ConfigurableForm> d = null, bool closeatend = true)
        {
            if (now)
            {
                if (progcurrent != null)                    // if running, push the current one back onto the queue to be picked up
                    progqueue.Insert(0, progcurrent);

                progcurrent = new ActionProgramRun(fileset, r, inputparas, this, actioncontroller);   // now we run this.. no need to push to stack

                progcurrent.PrepareToRun(new ConditionVariables(progcurrent.inputvariables, actioncontroller.Globals, fileset.filevariables),
                                                fh == null ? new ConditionPersistentData() : fh, d == null ? new Dictionary<string, ExtendedControls.ConfigurableForm>() : d, closeatend);        // if no filehandles, make them and close at end
            }
            else
            {
                progqueue.Add(new ActionProgramRun(fileset, r, inputparas, this, actioncontroller));
            }
        }

        public void Execute()  
        {
            if (!executing)        // someone else, asked for us to run.. we don't, as there is a pause, and we wait until the pause completes
            {
                DoExecute();
            }
        }

        public void ResumeAfterPause()          // used when async..
        {
            if (executing) // must be in an execute state
            {
                DoExecute();
            }
        }

        public void DebugTrace(bool ll, string file = null)
        {
            logtologline = ll;
            if (logger != null)
            {
                logger.Dispose();
                logger = null;
            }

            if (file != null)
                logger = new BaseUtils.LogToFile(file);
        }


        private void DoExecute()    // MAIN thread only..     
        {
            executing = true;

            System.Diagnostics.Stopwatch timetaken = new System.Diagnostics.Stopwatch();
            timetaken.Start();

            while( true )
            {
                if (progcurrent != null)
                {
                    if (progcurrent.GetErrorList != null)       // any errors pending, handle
                    {
                        actioncontroller.LogLine("Error: " + progcurrent.GetErrorList + Environment.NewLine + StackTrace() );
                        TerminateToCloseAtEnd();        // terminate up to a close at end entry, which would have started this stack
                    }
                    else if (progcurrent.IsProgramFinished)        // if current program ran out, cancel it
                    {
                        // this catches a LOOP without a statement at the end..  or a DO without a WHILE at the end..
                        if (progcurrent.ExecLevel > 0 && progcurrent.LevelUp(progcurrent.ExecLevel, null)) // see if we have any pending LOOP (or a DO without a while) and continue..
                            continue;       // errors or movement causes it to go back.. errors will be picked up above

                        TerminateCurrent();
                    }
                }

                while (progcurrent == null && progqueue.Count > 0)    // if no program,but something in queue
                {
                    progcurrent = progqueue[0];
                    progqueue.RemoveAt(0);

                    if (progcurrent.variables != null)      // if not null, its because its just been restarted after a call.. reset globals
                    {
                        progcurrent.Add(actioncontroller.Globals); // in case they have been updated...
                        progcurrent.Add(progcurrent.actionfile.filevariables); // in case they have been updated...
                    }
                    else
                    {
                        progcurrent.PrepareToRun(
                                new ConditionVariables(progcurrent.inputvariables, actioncontroller.Globals, progcurrent.actionfile.filevariables),
                                new ConditionPersistentData(),
                                new Dictionary<string, ExtendedControls.ConfigurableForm>(), true); // with new file handles and close at end..
                    }

                    if (progcurrent.IsProgramFinished)          // reject empty programs..
                    {
                        TerminateCurrent();
                        continue;       // and try again
                    }
                }

                if (progcurrent == null)        // Still nothing, game over
                    break;

                ActionBase ac = progcurrent.GetNextStep();      // get the step. move PC on.

                //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " @ " + progcurrent.Location +" Lv " + progcurrent.ExecLevel + " e " + (progcurrent.IsExecuteOn ? "1" : "0") + " up " + ac.LevelUp + " " + progcurrent.PushPos + " " + ac.Name);

                if (ac.LevelUp > 0 && progcurrent.LevelUp(ac.LevelUp, ac) )        // level up..
                {
                    //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Abort Lv" + progcurrent.ExecLevel + " e " + (progcurrent.IsExecuteOn ? "1" : "0") + " up " + ac.LevelUp + ": " + progcurrent.StepNumber + " " + ac.Name + " " + ac.DisplayedUserData);
                    continue;
                }

                if ( logtologline || logger != null )
                {
                    string t = (Environment.TickCount % 10000).ToString("00000") + " ";
                    string index = string.Concat(Enumerable.Repeat(". ", progcurrent.ExecLevel));
                    string s =  progcurrent.GetLastStep().LineNumber.ToString() + (progcurrent.IsExecuteOn ? "+" : "-") + ":" + index + ac.Name + " " + ac.DisplayedUserData;
                    System.Diagnostics.Debug.WriteLine(t+s);
                    if ( logtologline )
                        actioncontroller.LogLine(t + s);
                    if ( logger!= null )
                        logger.Write(s);
                }

                if (progcurrent.DoExecute(ac))       // execute is on.. 
                {
                    if (ac.Type == ActionBase.ActionType.Call)     // Call needs to pass info back up thru to us, need a different call
                    {
                        ActionCall acall = ac as ActionCall;
                        string prog;
                        ConditionVariables paravars;
                        if (acall.ExecuteCallAction(progcurrent, out prog, out paravars)) // if execute ok
                        {
                            //System.Diagnostics.Debug.WriteLine("Call " + prog + " with " + paravars.ToString());

                            Tuple<ActionFile, ActionProgram> ap = actionfilelist.FindProgram(prog, progcurrent.actionfile);          // find program using this name, prefer this action file first

                            if (ap != null)
                            {
                                Run(true,ap.Item1, ap.Item2, paravars , progcurrent.functions.persistentdata,progcurrent.dialogs, false);   // run now with these para vars
                            }
                            else
                                progcurrent.ReportError("Call cannot find " + prog);
                        }
                    }
                    else if (ac.Type == ActionBase.ActionType.Return)     // Return needs to pass info back up thru to us, need a different call
                    {
                        ActionReturn ar = ac as ActionReturn;
                        string retstr;
                        if ( ar.ExecuteActionReturn(progcurrent,out retstr) )
                        {
                            TerminateCurrent();

                            if (progqueue.Count > 0)        // pass return value if program is there..
                                progqueue[0]["ReturnValue"] = retstr;

                            continue;       // back to top, next action from returned function.
                        }
                    }
                    else if (!ac.ExecuteAction(progcurrent))      // if execute says, stop, i'm waiting for something
                    {
                        return;             // exit, with executing set true.  ResumeAfterPause will restart it.
                    }
                }

                if (AsyncMode && timetaken.ElapsedMilliseconds > 100)  // no more than 100ms per go to stop the main thread being blocked
                {
                    //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " *** SUSPEND");
                    restarttick.Start();
                    break;
                }
            }

            executing = false;
        }

        private void Tick_Tick(object sender, EventArgs e) // used when async
        {
            restarttick.Stop();
            //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " *** RESUME");
            Execute();
        }


        public void TerminateAll()          // halt everything
        {
            foreach (ActionProgramRun p in progqueue)       // ensure all have a chance to clean up
                p.Terminated();

            progcurrent = null;
            progqueue.Clear();
            executing = false;
        }

        public string StackTrace()
        {
            string s = "";

            if (progcurrent != null)        // if we are running..
            {
                s += "At " + progcurrent.Location + ": " + progcurrent.GetLastStep().Name;

                if (!progcurrent.ClosingHandlesAtEnd)       // if we are not a base functions, trace back up
                {
                    foreach (ActionProgramRun p in progqueue)       // ensure all have a chance to clean up
                    {
                        s += Environment.NewLine;
                        s += "At " + p.Location + ": " + p.GetLastStep().Name;
                        if (p.ClosingHandlesAtEnd)
                            break;
                    }
                }
            }

            return s;
        }

        public void TerminateToCloseAtEnd()     // halt up to close at end function
        {
            if ( progcurrent != null && !progcurrent.ClosingHandlesAtEnd )     // if its not a top end function
            {
                List<ActionProgramRun> toremove = new List<ActionProgramRun>();
                foreach (ActionProgramRun p in progqueue)       // ensure all have a chance to clean up
                {
                    toremove.Add(p);
                    if (p.ClosingHandlesAtEnd)
                        break;
                }

                foreach (ActionProgramRun apr in toremove)
                {
                    apr.Terminated();
                    progqueue.Remove(apr);
                }
            }

            progcurrent = null;
            executing = false;
        }

        public void TerminateCurrent()
        {
            if (progcurrent != null)
            {
                progcurrent.Terminated();
                progcurrent = null;
            }
        }

        public void WaitTillFinished(int timeout)           // Could be IN ANOTHER THREAD BEWARE
        {
            int t = Environment.TickCount + timeout;
            while( Environment.TickCount < t )
            {
                if (progcurrent == null)
                    break;

                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
