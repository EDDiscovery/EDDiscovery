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
        private ActionProgramList programs = null;

        bool async = false;             // if this Action is an asynchoronous object
        bool executing = false;         // Records is executing
      
        Timer restarttick = new Timer();

        public ActionRun(EDDiscoveryForm ed, ActionProgramList pl, bool asy)
        {
            restarttick.Interval = 100;
            restarttick.Tick += Tick_Tick;
            discoveryform = ed;
            programs = pl;
            async = asy;
        }

        //h may be null if not associated with a entry

        public void Add( ActionProgram r , HistoryList hl, HistoryEntry h, Dictionary<string,string> v )          // WE take a copy of each program, so each invocation of program action has a unique instance in case
        {                                                                   // it has private variables in either action or program.
            progqueue.Add( new ActionProgramRun(r,discoveryform,hl,h,v,async) );
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

            while ((ac = GetNextAction()) != null)
            {
                System.Diagnostics.Debug.WriteLine("Exec Lv" + progcurrent.ExecLevel + " e " + (progcurrent.IsExecuteOn ? "1" : "0") + " up " + ac.LevelUp + ": " + progcurrent.StepNumber + " " + ac.Name + " " + ac.DisplayedUserData);

                if (ac.LevelUp > 0 && progcurrent.LevelUp(ac))          // level up, if it returns true, it has changed position, so go back and get next
                    continue;

                if (progcurrent.GetErrorList != null)       // any errors due to above.. get next statement from next program
                {
                    discoveryform.LogLine("Error at " + progcurrent.Location + ":" + Environment.NewLine + progcurrent.GetErrorList);
                    TerminateCurrentProgram();
                    continue; // get next program
                }

                if (progcurrent.DoExecute(ac))       // execute is on.. 
                {
                    if (!ac.ExecuteAction(progcurrent))      // if execute says, stop, i'm waiting for something
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

        private void Tick_Tick(object sender, EventArgs e) // used when async
        {
            restarttick.Stop();
            Execute();
        }
    }
}
