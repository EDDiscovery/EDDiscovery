/*
 * Copyright © 2022-2022 EDDiscovery development team
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
 */

using BaseUtils;
using NetMQ;
using NetMQ.Monitoring;
using NetMQ.Sockets;
using QuickJSON;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlPythonPanel : UserControlCommonBase
    {
        private Actions.ActionController actioncontroller;
        
        private string pluginfolder;
        private JToken config;
  
        private Process pythonprocess;
        private DealerSocket server;
        private NetMQPoller poller;
        private NetMQMonitor monitor;

        private bool ServerRunning => server != null;

        public UserControlPythonPanel()
        {
            InitializeComponent();
        }

        #region UCCB IF

        // called right after creation, before anything else
        public override void Creation(PanelInformation.PanelInfo p)
        {
            base.Creation(p);
            System.Diagnostics.Debug.WriteLine($"Python panel create class {p.WindowTitle} db {DBBaseName}");
            DBBaseName = "PythonPanel" + p.PopoutID + ":";
            pluginfolder = p.Tag as string;
            string optfile = Path.Combine(pluginfolder, "config.json");
            string optcontents = BaseUtils.FileHelpers.TryReadAllTextFromFile(optfile);
            config = JToken.Parse(optcontents, JToken.ParseOptions.CheckEOL | JToken.ParseOptions.AllowTrailingCommas);
        }

        public override void Init()
        {
            System.Diagnostics.Debug.WriteLine($"Python panel Init {DBBaseName}");

            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewJournalEntryUnfiltered += DiscoveryForm_OnNewJournalEntryUnfiltered;
            DiscoveryForm.OnThemeChanged += Discoveryform_OnThemeChanged;
            DiscoveryForm.ScreenShotCaptured += Discoveryform_ScreenShotCaptured;
            DiscoveryForm.OnNewTarget += Discoveryform_OnNewTarget;

            actioncontroller = DiscoveryForm.MakeAC(pluginfolder,null,null,Log);     // action files are in this folder, don't allow management

            extRichTextBoxErrorLog.Visible = false;

            Log("Awaiting python connecting");
        }

        public override bool SupportTransparency { get { return config["Panel"].I("SupportTransparency").Bool(false); } } 
        public override bool DefaultTransparent { get { return config["Panel"].I("DefaultTransparent").Bool(false); } }
        // gets called before load layout, will need to keep track for initial display
        public override void SetTransparency(bool ison, Color curcol)
        {
        }
        // When the user changes mode
        public override void TransparencyModeChanged(bool on) 
        { 
        }  

        // Action UI layout is done AFTER init as themeing is done between init and SetTransparency/LoadLayout
        // the UC sizes and themes itself
        public override void LoadLayout()
        {
            configurableUC.Init("UC", "");

            actioncontroller.ReLoad();

            ActionLanguage.ActionFile af = actioncontroller.GetFile("UIInterface");
            if (af != null)
                af.Dialogs["UC"] = configurableUC;      // add the UC
            else
                Log($"Missing UIInterface.act action file in python plugin folder {pluginfolder}");

            actioncontroller.onStartup();
            actioncontroller.CheckWarn();

            while( actioncontroller.Executing)      // ensure complete - it should be, but we need to check
            {
                System.Diagnostics.Debug.WriteLine("Executing");
                Application.DoEvents();
            }

            configurableUC.Themed();                // this completes theming
        }

        // On Initial display, start up the python system
        public override void InitialDisplay()
        {
            string modulecheckfile = Path.Combine(pluginfolder, "pymodcheck.py");
            string pyfile = config["Python"].I("Start").Str();

            // go thru the required modules list, write out a script to install them (ran in user mode, so it will be a per user install)
            // we only check once after install, the modulecheckfile indicates we checked

            bool good = true;

            JArray ja = config["Python"].I("Modules").Array();
            if (ja != null && !File.Exists(modulecheckfile))
            {
                string[] pycontents = pyfile != null ? BaseUtils.FileHelpers.TryReadAllLinesFromFile(Path.Combine(pluginfolder, pyfile)) : null;

                if (pycontents != null && pycontents.Length > 0)        // we have the py start, get its shebang
                {
                    string script = pycontents[0] + "\r\n" +       // shebang from start file
                                "import subprocess\r\nimport sys\r\n" +
                                "def install(package): subprocess.check_call([sys.executable, \"-m\", \"pip\", \"install\", package])\r\n";

                    foreach (var x in ja)
                        script += "install('" + x.Str() + "')\r\n";

                    File.WriteAllText(modulecheckfile, script);

                    var output = (Tuple<string, string>)BaseUtils.PythonLaunch.PyExeLaunch(modulecheckfile, "", pluginfolder, null, true);
                    if (output.Item2.HasChars())   // this is standard error
                    {
                        Log("****** Failed to install required python modules ******\r\n\r\n");
                        Log(output.Item1);
                        Log(output.Item2);
                        good = false;
                        BaseUtils.FileHelpers.DeleteFileNoError(modulecheckfile);
                    }
                }
                else
                {
                    Log($"Cannot read py file {pyfile}");
                    good = false;
                }
            }

            if (good)
            {
                try
                {
                    server = new DealerSocket();
                    int socketnumber = EDDOptions.Instance.PythonDebugPort;
                    string bindstring = "tcp://localhost:" + socketnumber.ToStringInvariant();
                    server.Bind(bindstring);

                    poller = new NetMQPoller();
                    poller.Add(server);
                    poller.RunAsync("NetMQPoller:" + DBBaseName);

                    monitor = new NetMQMonitor(server, $"inproc://addr:" + socketnumber.ToStringInvariant(), SocketEvents.All);
                    monitor.AttachToPoller(poller);

                    server.ReceiveReady += (s, e) => { this.BeginInvoke((MethodInvoker)delegate { ServerReceived(s, e); }); };
                    monitor.Disconnected += (s, e) => { this.BeginInvoke((MethodInvoker)delegate { Monitor_Disconnected(s, e); }); };

                    // socket numbers <10000 do not launch python, instead you should have the debugginer running it ready to connect 
                    if (socketnumber >= 10000)      
                    {
                        pythonprocess = (Process)BaseUtils.PythonLaunch.PyExeLaunch(pyfile, socketnumber.ToStringInvariant(), pluginfolder, null, false);

                        if (pythonprocess == null)
                        {
                            Log($"Cannot launch {pluginfolder} / {pyfile}");
                            CloseNetMQ();
                        }
                        else
                            EDDOptions.Instance.PythonDebugPort++;      // python launched, next window gets another port number
                    }

                    configurableUC.TriggerAdv += ConfigurableUC_TriggerAdv;
                }
                catch (Exception ex)
                {
                    Log($"Cannot launch {pluginfolder} / {pyfile} exception {ex}");
                    CloseNetMQ();
                }
            }
        }

        public override bool AllowClose() 
        { 
            if ( ServerRunning )
            {
                System.Diagnostics.Debug.WriteLine("Python Send terminate");
                SendTerminate();
                while( !exitreceived && ServerRunning)        // we horribly just sit here waiting for the exit received to be sent..
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(20);
                }
                System.Diagnostics.Debug.WriteLine("Python received exit {exitreceived}");
            }

            return true;
        }

        public override void Closing()
        {
            System.Diagnostics.Debug.WriteLine($"Python panel Closing {DBBaseName}");

            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewJournalEntryUnfiltered -= DiscoveryForm_OnNewJournalEntryUnfiltered;
            DiscoveryForm.OnThemeChanged -= Discoveryform_OnThemeChanged;
            DiscoveryForm.ScreenShotCaptured -= Discoveryform_ScreenShotCaptured;
            DiscoveryForm.OnNewTarget -= Discoveryform_OnNewTarget;

            CloseNetMQ();
        }

        private void CloseNetMQ()
        {
            if (server != null)
            {
                monitor.DetachFromPoller();         // temperamental.
                monitor.Stop();
                poller.RemoveAndDispose(server);
                poller.Stop();
                server.Close();
                server = null;
            }

            if (pythonprocess != null && !pythonprocess.HasExited)
            {
                pythonprocess.Kill();
            }
        }

        #endregion

        #region From client

        private bool exitreceived = false;

        // Message received from python
        private void ServerReceived(object sender, NetMQ.NetMQSocketEventArgs e)
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);
            while (server.TryReceiveFrameString(out string clientsend))
            {
                JObject json = JObject.Parse(clientsend);
                if (json != null)
                {
                    string request = json["requesttype"].Str();
                    string commander = DiscoveryForm.History.CommanderName();

                    switch (request)
                    {
                        case "start":
                            {
                                string curver = System.Reflection.Assembly.GetExecutingAssembly().GetAssemblyVersionString();
                                JObject reply = new JObject
                                {
                                    ["responsetype"] = "start",
                                    ["eddversion"] = curver,
                                    ["apiversion"] = 1,
                                    ["historylength"] = DiscoveryForm.History.Count,
                                    ["commander"] = commander,
                                    ["config"] = GetSetting("Config", ""),
                                };
                                SendMessage(reply);
                                Log($"Python connected with version {json["pythonversion"].Str()}");
                                extRichTextBoxErrorLog.Visible = false;
                                configurableUC.Visible = true;
                                break;
                            }

                        case "history":
                            {
                                int start = json["start"].Int();
                                int length = json["length"].Int();

                                if (start >= 0 && start < DiscoveryForm.History.Count)
                                {
                                    length = Math.Min(length, DiscoveryForm.History.Count - start);
                                }
                                else
                                {
                                    start = -1;
                                    length = 0;
                                }

                                JObject reply = new JObject
                                {
                                    ["responsetype"] = "historyrequest",
                                    ["firstrow"] = start,
                                    ["length"] = length,
                                    ["commander"] = commander,
                                    ["Rows"] = new JArray()
                                };

                                while(length-- > 0 )
                                {
                                    EliteDangerousCore.HistoryEntry he = DiscoveryForm.History[start++];
                                    JToken jo = JToken.FromObject(he, true, new Type[] { typeof(Bitmap), typeof(Image), 
                                                    typeof(EliteDangerousCore.EDCommander) }, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                                    jo["Info"] = he.GetInfo();
                                    jo["Detailed"] = he.GetDetailed();
                                    reply["Rows"].Array().Add(jo);
                                }

                                SendMessage(reply);
                            }

                            break;

                        case "exit":
                            {
                                string reason = json["reason"].Str();
                                if (reason.HasChars())
                                    Log("Python requested termination due to " + reason);
                                string config = json["config"].Str();
                                if (config.HasChars())
                                {
                                    PutSetting("Config", config);
                                    Log($"Python config {config}");
                                    System.Diagnostics.Debug.WriteLine($"Python config {config}");
                                }
                                exitreceived = true;
                            }
                            break;

                        case "uiget":
                            {
                                string controlname = json["control"].Str();
                                string value = configurableUC.Get(controlname);

                                JObject reply = new JObject
                                {
                                    ["responsetype"] = "uiget",
                                    ["control"] = controlname,
                                    ["value"] = value,
                                };

                                SendMessage(reply);
                            }
                            break;
                        case "uiset":
                        case "uisetescape":
                            {
                                string controlname = json["control"].Str();
                                string value = json["value"].Str();
                                if (controlname.HasChars() && value != null)
                                    configurableUC.Set(controlname, value, request == "uisetescape");
                            }
                            break;
                        default:
                            Log($"ERROR Unknown request from client {request}");
                            break;
                    }
                }
                else
                    Log("ERROR Server Received bad JSON:" + clientsend);
            }
        }

        private void Monitor_Disconnected(object sender, NetMQMonitorSocketEventArgs e)
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);
            Log("Python Disconnected");
        }
        #endregion

        #region UI Events

        private void ConfigurableUC_TriggerAdv(string dialogname, string controlnameevent, object eventdata, object callertag)
        {
            int firstcolon = controlnameevent.IndexOf(':');

            JObject reply = new JObject()
            {
                ["responsetype"] = "uievent",
            };

            if ( firstcolon == -1)
            {
                reply["controlname"] = controlnameevent;
            }
            else
            {
                reply["controlname"] = controlnameevent.Substring(0, firstcolon);
                int secondcolon = controlnameevent.IndexOf(':', firstcolon + 1);
                if ( secondcolon == -1 )
                    reply["event"] = controlnameevent.Substring(firstcolon + 1);
                else
                {
                    reply["event"] = controlnameevent.Substring(firstcolon + 1,secondcolon-firstcolon-1);
                    reply["data"] = controlnameevent.Substring(secondcolon + 1);
               }
            };

            if ( eventdata != null)
            {
                JToken tk = JToken.FromObject(eventdata, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                reply["value"] = tk;
            }

            SendMessage(reply);
        }

        #endregion

        #region Events
        private void SendTerminate()
        {
            JObject reply = new JObject
            {
                ["responsetype"] = "terminate",
            };

            SendMessage(reply);
        }

        private void Discoveryform_OnHistoryChange()
        {
            JObject reply = new JObject
            {
                ["responsetype"] = "historyload",
                ["historylength"] = DiscoveryForm.History.Count,
                ["commander"] = DiscoveryForm.History.CommanderName(),
            };
            SendMessage(reply);

        }

        // travel history changed cursor
        public override void ReceiveHistoryEntry(EliteDangerousCore.HistoryEntry he)
        {
            JObject reply = new JObject
            {
                ["responsetype"] = "travelhistorymoved",
                ["row"] = he.Index,
            };

            SendMessage(reply);
        }

        private void DiscoveryForm_OnNewJournalEntryUnfiltered(EliteDangerousCore.JournalEntry obj)
        {
            JToken jo = JToken.FromObject(obj, true, new Type[] { typeof(Bitmap), typeof(Image)}, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            JObject reply = new JObject
            {
                ["responsetype"] = "journalpush",
                ["commander"] = EliteDangerousCore.EDCommander.Current.Name,
                ["journalEntry"] = jo,
            };

            SendMessage(reply);

            throw new NotImplementedException();
        }

        private void Discoveryform_OnNewEntry(EliteDangerousCore.HistoryEntry he)
        {
            JObject reply = new JObject
            {
                ["responsetype"] = "historypush",
                ["firstrow"] = he.Index,
                ["length"] = 1,
                ["commander"] = EliteDangerousCore.EDCommander.Current.Name,
                ["Rows"] = new JArray()
            };

            JToken jo = JToken.FromObject(he, true, new Type[] { typeof(Bitmap), typeof(Image),
                                                    typeof(EliteDangerousCore.EDCommander) }, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            reply["Rows"].Array().Add(jo);
            jo["Info"] = he.GetInfo();
            jo["Detailed"] = he.GetDetailed();
            SendMessage(reply);
        }

        private void Discoveryform_OnNewUIEvent(EliteDangerousCore.UIEvent uievent)
        {
            QuickJSON.JToken t = QuickJSON.JToken.FromObject(uievent, ignoreunserialisable: true,
                                                            ignored: new Type[] { typeof(Bitmap), typeof(Image) },
                                                            maxrecursiondepth: 3);
        }

        private void Discoveryform_OnThemeChanged()
        {
            var th = ExtendedControls.Theme.Current;
            var jo = JObject.FromObject(th, true, maxrecursiondepth: 5, membersearchflags: System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        }

        private void Discoveryform_ScreenShotCaptured(string file, Size size)
        {
        }

        private void Discoveryform_OnNewTarget(object obj)
        {
            var hastarget = EliteDangerousCore.DB.TargetClass.GetTargetPosition(out string name, out double x, out double y, out double z);
        }

        #endregion

        #region Helpers

        private void SendMessage(JToken reply)
        {
            if (!server.TrySendFrame(reply.ToString()))
                Log($"ERROR Server failed to send {reply.ToString()}");
        }

        public void Log(string logtext)
        {
            extRichTextBoxErrorLog.AppendText(logtext + Environment.NewLine);
           configurableUC.Visible = false;
           extRichTextBoxErrorLog.Visible = true;
           extRichTextBoxErrorLog.Dock = DockStyle.Fill;
        }

        #endregion
    }
}
