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
            public int stepnumber;
        };
           
        private Queue<ProgramQueue> progqueue = new Queue<ProgramQueue>();

        private ProgramQueue progcurrent = null;

        private EDDiscoveryForm discoveryform = null;

        bool async = false;             // if this Action is an asynchoronous object
        bool executing = false;         // because Execute can be reentrant, stop it running on top of itself.
      
        Timer restarttick = new Timer();

        public ActionRun(EDDiscoveryForm ed, bool asy)
        {
            restarttick.Interval = 100;
            restarttick.Tick += Tick_Tick;
            discoveryform = ed;
            async = asy;
        }

        public void Add( ActionProgram r , HistoryEntry h = null , JSONHelper.JSONFields v = null )          // WE take a copy of each program, so each invocation of program action has a unique instance in case
        {                                                                   // it has private variables in either action or program.
            progqueue.Enqueue( new ProgramQueue() { ap = new ActionProgram(r,discoveryform,h,v,async), stepnumber = 0 } );
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

        public void Execute()    // MAIN thread only..     
        {
            executing = true;

            Action ac;

            System.Diagnostics.Stopwatch timetaken = new System.Diagnostics.Stopwatch();
            timetaken.Start();

            while ((ac = GetNextAction()) != null)
            {
                progcurrent.ap.LevelUp(ac.LevelUp);     // change any level if required.. this stops an IF for instance.

                System.Diagnostics.Debug.WriteLine("Exec Lv" + progcurrent.ap.IfLevel + " e " + progcurrent.ap.DoExecute(ac) + ": " + ac.Name);

                if (progcurrent.ap.DoExecute(ac))       // execute is on.. 
                {
                    if (!ac.ExecuteAction(progcurrent.ap))      // if execute says, stop, i'm waiting for something
                    {
                        return;     // exit, with executing set true.  ResumeAfterPause will restart it.
                    }
                }

                if ( async && timetaken.ElapsedMilliseconds > 10000000000000 )  // no more than 100ms per go to stop the main thread being blocked
                {
                    restarttick.Start();
                    break;
                }
            }

            executing = false;
        }

        public void ResumeAfterPause()          // used when async..
        {
            if (executing)
                Execute();
        }

        private void Tick_Tick(object sender, EventArgs e) // used when async
        {
            restarttick.Stop();
            Execute();
        }
    }
}
