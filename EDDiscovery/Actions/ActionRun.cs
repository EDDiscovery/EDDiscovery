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
        class ProgramQueue
        {
            public ActionProgram ap;
            public HistoryEntry he;
            public int stepnumber;
        };
           
        private Queue<ProgramQueue> progqueue = new Queue<ProgramQueue>();

        private ProgramQueue progcurrent = null;

        private EDDiscoveryForm discoveryform = null;

        bool executing = false;         // because Execute can be reentrant, stop it running on top of itself.
      
        Timer restarttick = new Timer();

        public ActionRun(EDDiscoveryForm ed)
        {
            restarttick.Interval = 100;
            restarttick.Tick += Tick_Tick;
            discoveryform = ed;
        }

        public void Add( ActionProgram r , HistoryEntry h = null )          // WE take a copy of each program, so each invocation of program action has a unique instance in case
        {                                                                   // it has private variables in either action or program.
            progqueue.Enqueue( new ProgramQueue() { ap = new ActionProgram(r), he = h, stepnumber = 0 } );
        }

        public Action GetNextAction()
        {
            if (progcurrent == null || progcurrent.ap.Count < progcurrent.stepnumber)        // no program, or step number beyond program count
            {
                if (progqueue.Count > 0)        // we have more programs.. queue.
                {
                    progcurrent = progqueue.Dequeue();
                }
                else
                    progcurrent = null;
            }

            return (progcurrent != null) ? progcurrent.ap.GetStep(progcurrent.stepnumber++) : null;
        }

        private void Execute(bool dontpause, bool notimeout )       // MAIN thread only..     if nopausing, actions won't pause until complete. 
        {
            executing = true;

            Action ac;

            System.Diagnostics.Stopwatch timetaken = new System.Diagnostics.Stopwatch();
            timetaken.Start();

            while ((ac = GetNextAction()) != null)
            {
                if (!ac.ExecuteAction(progcurrent.he, discoveryform, dontpause))          // if execute says, stop, i'm waiting for something
                {
                    return;     // exit, with executing set true.  ResumeAfterPause will restart it.
                }

                if ( !notimeout && timetaken.ElapsedMilliseconds > 100 )  // no more than 100ms per go to stop the main thread being blocked
                {
                    restarttick.Start();
                    break;
                }
            }

            executing = false;
        }

        public void ExecuteSynchronous()        // excute list until complete. do not timeout, do not allow pauses in actions to hold the queue
        {
            Execute(true, true);
        }

        public void ExecuteAsynchronous()       // async, may exit before completion.. either timer or pause will start it again
        {
            //Execute(false, false);
            Execute(false, true);
        }

        public void ResumeAfterPause()          // used when async..
        {
            if (executing)
                Execute(false,false);
        }

        private void Tick_Tick(object sender, EventArgs e) // used when async
        {
            restarttick.Stop();
            Execute(false,false);
        }
    }
}
