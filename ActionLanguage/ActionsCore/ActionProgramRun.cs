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
using Conditions;

namespace ActionLanguage
{
    // this is the run time context of a program, holding run time data

    public class ActionProgramRun : ActionProgram
    {
        // used during execution.. filled in on program objects associated with an execution
        public ActionCoreController actioncontroller;                     // core controller.
        public ConditionFunctions functions;                   // function handler
        public ActionFile actionfile;                       // what file it came from..

        private ConditionVariables currentvars;      // set up by ActionRun at invokation so they have the latest globals, see Run line 87 ish

        private ConditionPersistentData conditionpersistentdata;
        public Dictionary<string, ExtendedControls.ConfigurableForm> dialogs;
        private bool closehandlesatend;

        public bool ClosingHandlesAtEnd { get { return closehandlesatend; } }

        private ConditionVariables inputvars;        // input vars to this program, never changed

        private ActionRun actionrun;                         // who is running it..

        private int nextstepnumber;     // the next step to execute, 0 based

        public enum ExecState { On, Off, OffForGood }
        private ExecState[] execstate = new ExecState[50];
        private ActionBase.ActionType[] exectype = new ActionBase.ActionType[50];   // type of level
        private int[] execlooppos = new int[50];            // if not -1, on level down, go back to this step.
        private int execlevel = 0;

        private bool continueonerrors = false;
        private string errlist = null;

        public ActionProgramRun(ActionFile af, // associated file
                                ActionProgram r,  // the program
                                ConditionVariables iparas,             // input variables to the program only.. not globals
                                ActionRun runner, // who is running it..
                                ActionCoreController ed) : base(r.Name)      // allow a pause
        {
            actionfile = af;
            actionrun = runner;
            actioncontroller = ed;
            execlevel = 0;
            execstate[execlevel] = ExecState.On;
            nextstepnumber = 0;

            //System.Diagnostics.Debug.WriteLine("Run " + actionfile.name + "::" + r.Name);
            //ActionData.DumpVars(gvars, " Func Var:");

            inputvars = iparas;             // current vars is set up by ActionRun at the point of invokation to have the latests globals

            List<ActionBase> psteps = new List<ActionBase>();
            ActionBase ac;
            for (int i = 0; (ac = r.GetStep(i)) != null; i++)
                psteps.Add(ActionBase.CreateCopy(ac));

            programsteps = psteps;
        }

        public ConditionVariables inputvariables { get { return inputvars; } }

        #region Variables

        public ConditionVariables variables { get { return currentvars; } }
        public string this[string s] { get { return currentvars[s]; } set { currentvars[s] = value; } }
        public void DeleteVariableWildcard(string v) { currentvars.DeleteWildcard(v); }
        public bool VarExist(string v) { return currentvars.Exists(v); }
        public void Add(ConditionVariables v) { currentvars.Add(v); }
        public void AddDataOfType(Object o, Type t, string n) { currentvars.AddDataOfType(o, t, n, 5); }

        #endregion

        #region Exec control

        public void PrepareToRun( ConditionVariables v , ConditionPersistentData fh , Dictionary<string, ExtendedControls.ConfigurableForm> d, bool chae = true)
        {
            currentvars = v;
            conditionpersistentdata = fh;
            closehandlesatend = chae;
            functions = new ConditionFunctions(currentvars, conditionpersistentdata);           // point the functions at our variables and our files..
            dialogs = d; 
        }

        public void Terminated()
        {
            if (closehandlesatend)
            {
                conditionpersistentdata.CloseAll();
                foreach (string s in dialogs.Keys)
                    dialogs[s].Close();

                dialogs.Clear();
            }

            //System.Diagnostics.Debug.WriteLine("Program " + actionfile.name + "::" + Name + " terminated, handle close " + closehandlesatend);
        }

        public ActionBase GetNextStep()
        {
            if (nextstepnumber < Count)
                return programsteps[nextstepnumber++];
            else
                return null;
        }

        public ActionBase GetLastStep()
        {
            if (nextstepnumber > 0 && nextstepnumber <= Count)
                return programsteps[nextstepnumber-1];
            else
                return null;
        }

        public string Location { get
            {
                int lineno = GetLastStep() != null ? GetLastStep().LineNumber : 0;

                return "Step " + nextstepnumber.ToStringInvariant() + " in " + actionfile.name + "::" + Name + ":" + lineno.ToStringInvariant();
            } }

        public int ExecLevel { get { return execlevel; } }

        public bool IsProgramFinished { get { return nextstepnumber >= Count; } }

        public void TerminateCurrentProgram()          // stop this program
        {
            nextstepnumber = Count;
        }

        public bool IsExecuteOn { get { return execstate[execlevel] == ExecState.On; } }
        public bool IsExecuteOff { get { return execstate[execlevel] == ExecState.Off; } }
        public bool IsExecutingType(ActionBase.ActionType ty) { return exectype[execlevel] == ty; }

        public int PushPos { get { return execlooppos[execlevel]; } }
        public void CancelPushPos() { execlooppos[execlevel] = -1; }

        public int StepNumber { get { return nextstepnumber; } }
        public void Goto(int pos) { nextstepnumber = pos; }

        public bool DoExecute(ActionBase ac)      // execute if control state
        {
            return execstate[execlevel] == ExecState.On || ac.Type >= ActionBase.ActionType.If;
        }

        public void PushState(ActionBase.ActionType ty, bool res, bool pushpos = false)
        {
            PushState(ty, res ? ExecState.On : ExecState.Off, pushpos);
        }

        public void PushState(ActionBase.ActionType ty, ExecState ex, bool pushpos = false)
        {
            execlevel++;
            exectype[execlevel] = ty;
            execstate[execlevel] = ex;
            execlooppos[execlevel] = (pushpos) ? (nextstepnumber - 1) : -1;
        }

        public void ChangeState(bool v)
        {
            this.execstate[execlevel] = v ? ExecState.On : ExecState.Off;
        }

        public void ChangeState(ExecState ex)
        {
            this.execstate[execlevel] = ex;
        }

        public void RemoveLevel()
        {
            execlevel = Math.Max(execlevel - 1, 0);
        }

        public void Break()
        {
            for (int i = execlevel; i > 0; i--)
            {
                if (exectype[i] == ActionBase.ActionType.While || exectype[i] == ActionBase.ActionType.Loop || exectype[i] == ActionBase.ActionType.ForEach )             
                {
                    execlooppos[i] = -1;            // while/Loop/Foreach.. expecting to loop back to WHILE or LOOP or FOR on next down push, instead don't
                    execstate[i] = ExecState.OffForGood;        // and we are off for good at the While/Loop level
                    execstate[execlevel] = ExecState.OffForGood;        // and we are off for good at this level.. levels in between must be IFs which won't execute because we executed
                    break;  // ironic break
                }
                else if ( exectype[i] == ActionBase.ActionType.Do )
                {
                    execstate[i] = ExecState.OffForGood;                // DO level is off for good.. this stops ExecutEndDo (at while) trying to loop around
                    execstate[execlevel] = ExecState.OffForGood;        // and we are off for good.
                }
            }
        }

        // true is reported on an error, or we need to get the next action.

        public bool LevelUp(int up, ActionBase action)      // action may be null at end of program
        {
            while (up-- > 0)
            {
                if (execlevel == 0)
                {
                    ReportError("Nested program level error, tring to move up while on level 0");
                    return true;
                }

                if (IsExecutingType(ActionBase.ActionType.Do))                // DO needs a while at level -1..
                {
                    if (action != null && action.Type == ActionBase.ActionType.While)
                    {
                        if (action.LevelUp == 1)                // only 1, otherwise its incorrectly nested
                        {
                            ActionWhile w = action as ActionWhile;
                            if (w.ExecuteEndDo(this))    // if this indicates (due to true) we need to fetch next instruction
                            {
                                return true;
                            }
                            else
                                RemoveLevel();      // else, just remove level.. 
                        }
                        else
                        {
                            ReportError("While incorrectly nested under Do");
                            return true;
                        }
                    }
                    else
                    {
                        ReportError("While missing after Do");
                        return true;
                    }
                }
                else if (IsExecutingType(ActionBase.ActionType.Loop)) // active loop, need to consider if we need to go back
                {
                    // break may have cancelled this
                    if (PushPos >= 0 && ((ActionLoop)GetStep(PushPos)).ExecuteEndLoop(this))      // if true, it wants to move back, so go back and get next value.
                    {
                        return true;
                    }
                    else
                        RemoveLevel();      // else, just remove level.. 
                }
                else if (IsExecutingType(ActionBase.ActionType.ForEach)) // active For, need to consider if we need to go back
                {
                    // break may have cancelled this
                    if (PushPos >= 0 && ((ActionForEach)GetStep(PushPos)).ExecuteEndFor(this))      // if true, it wants to move back, so go back and get next value.
                    {
                        return true;
                    }
                    else
                        RemoveLevel();      // else, just remove level.. 
                }
                else
                {                                               // normal, just see if need to loop back
                    int stepback = PushPos;

                    RemoveLevel();

                    if (stepback >= 0)
                    {
                        Goto(stepback);
                        return true;
                    }
                }
            }

            return false;
        }

        public void ResumeAfterPause()          // used when async..
        {
            System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Resume code " + this.Name);
            actionrun.ResumeAfterPause();
        }

        #endregion

        #region Run time errors
        public bool ReportError(string s)
        {
            currentvars["LastError"] = s;
            if (!continueonerrors)
            {
                if (errlist != null)
                    errlist += Environment.NewLine;
                errlist += s;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Swallowed error " + s);
            }

            return true;    // always true.. so you can use this directly in a return in the commands
        }

        public string GetErrorList { get { return errlist; } }

        public void SetContinueOnErrors(bool v)
        {
            continueonerrors = v;
        }

        #endregion

    }
}
