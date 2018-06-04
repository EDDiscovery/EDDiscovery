using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public class Processes
    {
        private Dictionary<int, Process> processes;

        public Processes()
        {
            processes = new Dictionary<int, Process>();
        }

        public void CloseAll()
        {
            foreach (int i in processes.Keys)
            {
                processes[i].Close();
            }

            processes.Clear();
        }

        public int StartProcess(string proc, string cmdline)
        {
            Process p = new Process();
            p.StartInfo.FileName = proc;
            p.StartInfo.Arguments = cmdline;
            bool ok = p.Start();
            if (ok)
            {
                processes[p.Id] = p;
                return p.Id;
            }
            else
                return 0;
        }

        public int FindProcess(string proc)
        {
            Process[] curprocs = Process.GetProcesses();
            Process f = Array.Find(curprocs, x => x.ProcessName.Equals(proc, StringComparison.InvariantCultureIgnoreCase));
            if (f != null)
            {
                processes[f.Id] = f;
                return f.Id;
            }
            else
                return 0;
        }

        static public string[] ListProcesses()
        {
            Process[] curprocs = Process.GetProcesses();
            return (from x in curprocs select x.ProcessName).ToArray();
        }

        public enum ProcessResult { OK, UnknownPID, Timeout, NotExited };

        public ProcessResult WaitForProcess(int pid, int timeout)
        {
            if (processes.ContainsKey(pid))
            {
                return processes[pid].WaitForExit(timeout) ? ProcessResult.OK : ProcessResult.Timeout;
            }
            else
                return ProcessResult.UnknownPID;
        }

        public ProcessResult KillProcess(int pid)
        {
            if (processes.ContainsKey(pid))
            {
                processes[pid].Kill();
                return ProcessResult.OK;
            }
            else
                return ProcessResult.UnknownPID;
        }

        public ProcessResult CloseProcess(int pid)
        {
            if (processes.ContainsKey(pid))
            {
                processes[pid].CloseMainWindow();
                return ProcessResult.OK;
            }
            else
                return ProcessResult.UnknownPID;
        }

        public ProcessResult HasProcessExited(int pid, out int exitcode)
        {
            exitcode = 0;
            if (processes.ContainsKey(pid))
            {
                if (processes[pid].HasExited)
                {
                    exitcode = processes[pid].ExitCode;
                    return ProcessResult.OK;
                }
                else
                    return ProcessResult.NotExited;
            }
            else
                return ProcessResult.UnknownPID;
        }


    }
}
