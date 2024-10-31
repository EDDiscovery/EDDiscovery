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
using EliteDangerousCore;
using QuickJSON;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlPythonPanel : UserControlCommonBase
    {
        private Actions.ActionController actioncontroller;
        
        private string pluginfolder;
        private JToken config;
        private bool panelgood;

        private System.Diagnostics.Process pythonprocess;
        private NetMQUtils.NetMQJsonServer zmqconnection;

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
            string optcontents =  BaseUtils.FileHelpers.TryReadAllTextFromFile(optfile);
            config = optcontents != null ? JToken.Parse(optcontents, JToken.ParseOptions.CheckEOL | JToken.ParseOptions.AllowTrailingCommas) : null;
            panelgood = config != null;
            if (!panelgood)
                config = new JToken();
        }

        public override void Init()
        {
            System.Diagnostics.Debug.WriteLine($"Python panel Init {DBBaseName}");

            SelectView(false);      // view log
            configurableUC.Dock = DockStyle.Fill;
            panelLog.Dock = DockStyle.Fill;

            if (panelgood)
            {
                DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
                DiscoveryForm.OnNewUIEvent += Discoveryform_OnNewUIEvent;
                DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
                DiscoveryForm.OnNewJournalEntryUnfiltered += DiscoveryForm_OnNewJournalEntryUnfiltered;
                DiscoveryForm.ScreenShotCaptured += Discoveryform_ScreenShotCaptured;
                DiscoveryForm.OnNewTarget += Discoveryform_OnNewTarget;

                actioncontroller = DiscoveryForm.MakeAC(this.FindForm(), pluginfolder, null, null, null, Log);     // action files are in this folder, don't allow management

                Log("Awaiting python connecting");
            }
            else
                Log("Missing python config.json file");
        }

        public override bool SupportTransparency { get { return config["Panel"].I("SupportTransparency").Bool(false); } } 
        public override bool DefaultTransparent { get { return config["Panel"].I("DefaultTransparent").Bool(false);} }
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
            if (!panelgood)
                return;
            configurableUC.Init("UC", "");

            actioncontroller.ReLoad();

            ActionLanguage.ActionFile af = actioncontroller.GetFile("UIInterface");
            if (af != null)
                af.Dialogs["UC"] = configurableUC;      // add the UC
            else
                Log($"Missing UIInterface.act action file in python plugin folder {pluginfolder}");

            // hook any returns from action files to a report to the python
            actioncontroller.AddReturnCallBack((actf, fname, str) =>
            {
                System.Diagnostics.Debug.WriteLine($"Return closing return {af.Name} {fname} {str} ");
                if (zmqconnection != null)
                {
                    JToken reply = JToken.Parse(str);
                    reply["responsetype"] = "runactionprogram";
                    reply["name"] = fname;
                    if (reply != null)
                        zmqconnection.Send(reply);
                    else
                        Log("Bad return format from action program - not in JSON format");
                }
            });

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
            if (!panelgood)
                return;

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
                    Log(output.Item1);
                    Log(output.Item2);
                    //if (output.Item2.HasChars())   // this is standard error
                    //{
                    //    Log("****** Failed to install required python modules ******\r\n\r\n");
                    //    good = false;
                    //    BaseUtils.FileHelpers.DeleteFileNoError(modulecheckfile);
                    //}
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
                    int socketnumber = EDDOptions.Instance.PythonDebugPort;
                    zmqconnection = new NetMQUtils.NetMQJsonServer();
                    zmqconnection.Received += (list) => { 
                        //System.Diagnostics.Debug.WriteLine($"Received {list[0].ToString()}");
                        this.BeginInvoke((MethodInvoker)delegate { HandleClientMessages(list); }); };

                    zmqconnection.Accepted += () => { System.Diagnostics.Debug.WriteLine($"Accepted"); };
                    zmqconnection.Disconnected += () => { System.Diagnostics.Debug.WriteLine($"Disconnected"); this.BeginInvoke((MethodInvoker)delegate { Log("Python Disconnected"); }); };

                    string threadname = "NetMQPoller:" + DBBaseName;
                    if (zmqconnection.Init("tcp://localhost", socketnumber, threadname))
                    {

                        // socket numbers <10000 do not launch python, instead you should have the debugger running it ready to connect 
                        if (socketnumber >= 10000)
                        {
                            pythonprocess = (System.Diagnostics.Process)BaseUtils.PythonLaunch.PyExeLaunch(pyfile, socketnumber.ToStringInvariant(), pluginfolder, null, false);

                            if (pythonprocess == null)
                            {
                                Log($"Cannot launch {pluginfolder} / {pyfile}");
                                zmqconnection.Close();
                            }
                            else
                                EDDOptions.Instance.PythonDebugPort++;      // python launched, next window gets another port number
                        }

                        configurableUC.TriggerAdv += ConfigurableUC_TriggerAdv;

                    }
                    else
                    {
                        Log($"Server failed to start on socket number {socketnumber} - probably a python panel is hanging around without being closed properly. Close using task manager");
                        zmqconnection = null;
                    }
                }
                catch (Exception ex)
                {
                    Log($"Cannot launch {pluginfolder} / {pyfile} exception {ex}");
                    zmqconnection.Close();
                }
            }
        }

        public override bool AllowClose() 
        { 
            if ( zmqconnection?.Running ?? false )     // protect against close before server running
            {
                System.Diagnostics.Debug.WriteLine("Python {DBBaseName} Send terminate");
                SendTerminate();
                MSTicks ms = new MSTicks(1000);

                while ( !exitreceived && zmqconnection.Running && !ms.TimedOut)        // we horribly just sit here waiting for the exit received to be sent..
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(20);
                }
                System.Diagnostics.Debug.WriteLine($"Python {DBBaseName} received exit {exitreceived}");
            }

            return true;
        }

        public override void Closing()
        {
            System.Diagnostics.Debug.WriteLine($"Python {DBBaseName} panel Closing ");

            if (!panelgood)
                return;

            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewJournalEntryUnfiltered -= DiscoveryForm_OnNewJournalEntryUnfiltered;
            DiscoveryForm.ScreenShotCaptured -= Discoveryform_ScreenShotCaptured;
            DiscoveryForm.OnNewTarget -= Discoveryform_OnNewTarget;

            zmqconnection?.Close();

            if (pythonprocess != null && !pythonprocess.HasExited)
            {
                pythonprocess.Kill();
            }

            actioncontroller.CloseDown();       // with persistent vars if required
        }

        #endregion

        #region From client

        private bool exitreceived = false;

        // Message received from python, in UI thread due to thunk above
        private async void HandleClientMessages(List<JToken> list)
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);

            foreach (JObject json in list)
            {
               // System.Diagnostics.Debug.Write($"Received {json.ToString()}");

                string request = json["requesttype"].Str();
                string commander = DiscoveryForm.History.CommanderName();

                string controlname = json["control"].Str();     // commonly used
                string value = json["value"].Str(); // commonly used

                HistoryList hl = DiscoveryForm.History;

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
                            zmqconnection.Send(reply);
                            Log($"Python connected with version {json["pythonversion"].Str()}");
                            SelectView(true);
                            break;
                        }

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
                    case "history":
                        {
                            int start = json["start"].Int();
                            int length = json["length"].Int();

                            // limit length to area available.  0 if out of range.
                            length = start >= 0 && start < DiscoveryForm.History.Count ? Math.Min(length, DiscoveryForm.History.Count - start) : 0;

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["start"] = start,
                                ["length"] = length,
                                ["commander"] = commander,
                                ["rows"] = new JArray()
                            };

                            JArray rows = reply["rows"].Array();

                            while (length-- > 0)
                            {
                                HistoryEntry he = hl[start++];
                                JToken jo = JToken.FromObject(he, true, new Type[] { typeof(Bitmap), typeof(Image),
                                                        typeof(EliteDangerousCore.EDCommander) }, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                                jo["Info"] = he.GetInfo();
                                jo["Detailed"] = he.GetDetailed();
                                rows.Add(jo);
                                //System.Diagnostics.Debug.WriteLine($"Return {jo.ToString(true)}");
                                //System.Diagnostics.Debug.WriteLine($"Return {jo.ToString()}");
                            }
                            zmqconnection.Send(reply);
                        }

                        break;

                    case "historyjid":
                        {
                            long jid = json["jid"].Long();

                            HistoryEntry he = hl.GetByJID(jid);     // null if not there

                            JToken jo = he == null ? null : JToken.FromObject(he, true, new Type[] { typeof(Bitmap), typeof(Image),
                                                        typeof(EliteDangerousCore.EDCommander) }, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["jid"] = jid,
                                ["entry"] = jo,
                            };
                            zmqconnection.Send(reply);
                        }

                        break;

                    case "missions":
                        {
                            HistoryEntry he = GetHE(json["entry"].Int());

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["entry"] = he.Index,
                                ["current"] = he == null ? null : new JArray(),
                                ["previous"] = he == null ? null : new JArray(),
                            };

                            if (he != null)
                            {
                                List<MissionState> ml = hl.MissionListAccumulator.GetMissionList(he.MissionList);

                                List<MissionState> mcurrent = MissionListAccumulator.GetAllCurrentMissions(ml, he.EventTimeUTC);

                                var cur = reply["current"].Array();

                                foreach (MissionState ms in mcurrent)
                                {
                                    JToken jo = JToken.FromObject(ms, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                                    cur.Add(jo);
                                }

                                List<MissionState> mprev = MissionListAccumulator.GetAllExpiredMissions(ml, he.EventTimeUTC);

                                JArray prev = reply["previous"].Array();

                                foreach (MissionState ms in mprev)
                                {
                                    JToken jo = JToken.FromObject(ms, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                                    prev.Add(jo);
                                }
                            }

                            System.Diagnostics.Debug.WriteLine($"Mission response {reply.ToString(true)}");

                            zmqconnection.Send(reply);
                        }
                        break;
                    case "ship":
                        {
                            HistoryEntry he = GetHE(json["entry"].Int());

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["entry"] = he.Index,
                                ["ship"] = he == null ? null : JToken.FromObject(he.ShipInformation, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),
                            };
                            System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;

                    case "shiplist":
                        {
                            var sl = DiscoveryForm.History.ShipInformationList;
                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["shiplist"] = sl == null ? null : JToken.FromObject(sl, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),
                            };
                            System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;

                    case "suitsweapons":
                        {
                            HistoryEntry he = GetHE(json["entry"].Int());

                            var sl = hl.SuitList.Suits(he.Suits);
                            var wp = hl.WeaponList.Weapons(he.Weapons);
                            var lo = hl.SuitLoadoutList.Loadouts(he.Loadouts);

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["entry"] = he.Index,
                                ["suits"] = sl == null ? null : JToken.FromObject(sl, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),
                                ["weapons"] = wp == null ? null : JToken.FromObject(wp, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),
                                ["loadouts"] = lo == null ? null : JToken.FromObject(lo, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),

                            };
                            System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;
                    case "carrier":
                        {
                            var cr = hl.Carrier;

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["carrier"] = cr == null ? null : JToken.FromObject(cr, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),

                            };
                            System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;
                    case "ledger":
                        {
                            var cr = hl.CashLedger;

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["ledger"] = cr == null ? null : JToken.FromObject(cr, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),

                            };
                            System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;

                    case "shipyards":
                        {
                            var cr = hl.Shipyards;

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["shipyards"] = cr == null ? null : JToken.FromObject(cr, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),

                            };
                            System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;

                    case "outfitting":
                        {
                            var cr = hl.Outfitting;

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["outfitting"] = cr == null ? null : JToken.FromObject(cr, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),

                            };
                            System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;

                    case "scandata":
                        {
                            string system = json["system"].Str();
                            long? systemid = json["systemid"].LongNull();
                            WebExternalDataLookup wdl = json["weblookup"].EnumStr<WebExternalDataLookup>(WebExternalDataLookup.None, true);
                            var sc = new SystemClass(system, systemid);
                            var node = await hl.StarScan.FindSystemAsync(sc, wdl);
                            if (IsClosed)
                                return;

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["system"] = system,
                                ["systemid"] = systemid,
                                ["scan"] = node == null ? null : JToken.FromObject(node, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),

                            };
                            System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;

                    case "faction":
                        {
                            string faction = json["faction"].Str();
                            var fs = hl.Stats.GetFaction(faction);

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["faction"] = faction,
                                ["data"] = fs == null ? null : JToken.FromObject(fs, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),

                            };
                            System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;

                    case "factions":
                        {
                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["data"] = JToken.FromObject(hl.Stats.FactionData, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),

                            };
                            System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;

                    case "mcmr":       // commods/mats/mrs
                        {
                            HistoryEntry he = GetHE(json["entry"].Int());

                            var cr = hl.MaterialCommoditiesMicroResources.Get(he.MaterialCommodity);

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["entry"] = he.Index,
                                ["mcmr"] = cr == null ? null : JToken.FromObject(cr, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),
                            };
                            System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;

                    case "uiget":
                        {
                            value = configurableUC.Get(controlname);

                            JObject reply = new JObject
                            {
                                ["responsetype"] = "uiget",
                                ["control"] = controlname,
                                ["value"] = value,
                            };

                            zmqconnection.Send(reply);
                        }
                        break;
                    case "uisuspend":
                        {
                            if (!configurableUC.Suspend(controlname))
                                Log($"PythonPanel suspend unknown control");
                        }
                        break;
                    case "uiresume":
                        {
                            if (!configurableUC.Resume(controlname))
                                Log($"PythonPanel resume unknown control");
                        }
                        break;
                    case "uiset":
                    case "uisetescape":
                        {
                            if (controlname.HasChars() && value != null)
                                configurableUC.Set(controlname, value, request == "uisetescape");
                        }
                        break;
                    case "uiaddtext":
                        {
                            if (controlname.HasChars() && value != null)
                                configurableUC.AddText(controlname, value);
                        }
                        break;
                    case "uiadd":
                        {
                            JArray controldef = json["controldefinitions"].Array();
                            if (controldef?.IsArray ?? false)
                            {
                                foreach (string str in controldef)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Make new control {str}");
                                    string res = str != null ? configurableUC.Add(str) : "Bad string";
                                    if (res != null)
                                        Log($"PythonPanel error making new control: {str}");
                                }

                                configurableUC.UpdateEntries();
                            }
                            else
                                Log($"PythonPanel error making new control: missing control array");

                        }
                        break;
                    case "uiremove":
                        {
                            JArray controldef = json["controllist"].Array();
                            if (controldef?.IsArray ?? false)
                            {
                                foreach (string str in controldef.EmptyIfNull())
                                {
                                    if (str==null || !configurableUC.Remove(str))
                                        Log($"PythonPanel error removing {str}");
                                }

                                configurableUC.UpdateEntries();
                            }
                            else
                                Log($"PythonPanel error removing controls: missing control array");
                        }
                        break;
                    case "uiaddsetrows":
                        {
                            var changelist = json["changelist"].Array();
                            string err = changelist != null ? configurableUC.AddSetRows(controlname, changelist) : "No change list";
                            if (err != null)
                                Log($"PythonPanel error addsetrows {controlname} : {err}");
                        }
                        break;
                    case "uiinsertcolumns":
                        {
                            int pos = json["position"].Int(0);

                            JArray controldef = json["columndefinitions"].Array();
                            if (controldef?.IsArray ?? false)
                            {
                                foreach (JObject col in controldef)
                                {
                                    if (col != null)
                                    {
                                        string coltype = col["type"].Str();
                                        string headertext = col["headertext"].Str();
                                        int fillsize = col["fillsize"].Int(100);
                                        string sortmode = col["sortmode"].Str("Alpha");
                                        if (!configurableUC.InsertColumn(controlname, pos++, coltype, headertext, fillsize, sortmode))
                                            Log($"PythonPanel error insertcolumns {controlname}");
                                    }
                                }
                            }
                            else
                                Log($"PythonPanel error insertcolumns {controlname} no columns");
                        }
                        break;
                    case "uiremovecolumns":
                        {
                            int pos = json["position"].Int(0);
                            int count = json["count"].Int(1);
                            if (!configurableUC.RemoveColumns(controlname,pos,count))
                                Log($"PythonPanel error removecolumns {controlname}");
                        }
                        break;
                    case "uirightclickmenu":
                        {
                            try
                            {
                                string[] tags = json["tags"].Array().Select(x => x.Str()).ToArray();        // could easily except
                                string[] text = json["text"].Array().Select(x => x.Str()).ToArray();
                                if (tags.Length != text.Length)
                                    throw new Exception();
                                if ( !configurableUC.SetRightClickMenu(controlname, tags, text))
                                    Log($"PythonPanel error rightclickmenu unknown control");
                            }
                            catch
                            {
                                Log($"PythonPanel error rightclickmenu missing fields");
                            }
                        }
                        break;
                    case "uigetcolumnssetting":
                        {
                            JToken tk = (JToken)configurableUC.GetDGVColumnSettings(controlname);
                            if ( tk != null)
                            {
                                JObject reply = new JObject
                                {
                                    ["responsetype"] = request,
                                    ["control"] = controlname,
                                    ["settings"] = tk,
                                };
                                zmqconnection.Send(reply);
                            }
                            else
                                Log($"PythonPanel error getcolumnsetting unknown control");

                        }
                        break;
                    case "uisetcolumnssetting":
                        {
                            JToken tk = json["settings"];
                            if ( tk == null || !configurableUC.SetDGVColumnSettings(controlname,tk))
                                Log($"PythonPanel error setcolumnsetting missing data or unknown control");
                         }
                        break;
                    case "uisetdgvsetting":
                        {
                            bool? cr = json["columnreorder"].BoolNull();
                            bool? pcww = json["percolumnwordwrap"].BoolNull();
                            bool? headervis = json["allowheadervisibility"].BoolNull();
                            bool? srs = json["singlerowselect"].BoolNull();
                            if (cr.HasValue && pcww.HasValue && headervis.HasValue && srs.HasValue &&
                                        configurableUC.SetDGVSettings(controlname, cr.Value, pcww.Value, headervis.Value, srs.Value))
                            {

                            }
                            else
                                Log($"PythonPanel error setdgvsetting missing data or unknown control");
                        }
                        break;
                    case "uisetwordwrap":
                        {
                            bool? ww = json["wordwrap"].BoolNull();
                            if (ww.HasValue && configurableUC.SetWordWrap(controlname,ww.Value))
                            {

                            }
                            else
                                Log($"PythonPanel error setwordwrap missing data or unknown control");
                        }
                        break;
                    case "uiclear":
                        {
                            if (!configurableUC.Clear(controlname))
                                Log($"PythonPanel error clearing {controlname}");
                        }
                        break;
                    case "uiremoverows":
                        {
                            int rowstart = json["rowstart"].Int();
                            int count = json["count"].Int();
                            int ret = configurableUC.RemoveRows(controlname, rowstart, count);
                            if ( ret < 0 )
                                Log($"PythonPanel error removing rows {controlname}");
                        }
                        break;
                    case "uienable":
                        {
                            bool state = json["state"].Bool(false);
                            if (!configurableUC.SetEnable(controlname, state))
                                Log($"PythonPanel error enable state {controlname}");
                        }
                        break;
                    case "uivisible":
                        {
                            bool state = json["state"].Bool(false);
                            if (!configurableUC.SetVisible(controlname, state))
                                Log($"PythonPanel error visible state {controlname}");
                        }
                        break;
                    case "uiposition":
                        {
                            if (!configurableUC.SetPosition(controlname, new Point(json["x"].Int(), json["y"].Int())))
                                Log($"PythonPanel error position {controlname}");

                        }
                        break;
                    case "uisize":
                        {
                            if (!configurableUC.SetSize(controlname, new Size(json["width"].Int(), json["height"].Int())))
                                Log($"PythonPanel error size {controlname}");
                        }
                        break;
                    case "uiclosedropdownbutton":
                        {
                            configurableUC.CloseDropDown();
                        }
                        break;

                    case "uimessagebox":
                        {
                            if (Enum.TryParse<MessageBoxButtons>(json["buttons"].Str(), true, out MessageBoxButtons buttons))
                            {
                                if (Enum.TryParse<MessageBoxIcon>(json["icon"].Str(), true, out MessageBoxIcon icon))
                                {
                                    string message = json["message"].Str();
                                    DialogResult res = ExtendedControls.MessageBoxTheme.Show(this.FindForm(), message, json["caption"].Str(), buttons, icon);
                                    JObject reply = new JObject
                                    {
                                        ["responsetype"] = request,
                                        ["message"] = message,
                                        ["response"] = res.ToString(),
                                    };
                                    zmqconnection.Send(reply);
                                }
                            }
                        }
                        break;


                    case "runactionprogram":
                        {
                            Variables vars = new Variables();
                            if (json.Contains("variables"))
                                vars.FromJSON(json["variables"], "");

                            string progname = json["name"].Str("??");

                            bool success = actioncontroller.RunProgram(progname, vars, false);

                            if ( !success )
                            {
                                JObject reply = new JObject
                                {
                                    ["responsetype"] = request,
                                    ["name"] = progname,
                                    ["status"] = "Program not found",
                                };
                                zmqconnection.Send(reply);
                            }
                        }
                        break;

                    case "showlog":
                        SelectView(false);
                        break;

                        //missions
                        //commodities
                        //    shipyard

                    default:
                        Log($"ERROR Unknown request from client {request}");
                        break;
                }
            }
        }


        HistoryEntry GetHE(int entry)
        {
            HistoryList hl = DiscoveryForm.History;
            if (entry < 0 || entry >= hl.Count)
                entry = hl.Count - 1;
            return hl[entry];
        }
        #endregion

        #region UI Events

        private void ConfigurableUC_TriggerAdv(string dialogname, string controlnameevent, object eventdata, object eventdata2, object callertag)
        {
            int firstcolon = controlnameevent.IndexOf(':');

            JObject reply = new JObject()
            {
                ["responsetype"] = "uievent",
            };

            if ( firstcolon == -1)
            {
                reply["control"] = controlnameevent;
            }
            else
            {
                reply["control"] = controlnameevent.Substring(0, firstcolon);
                int secondcolon = controlnameevent.IndexOf(':', firstcolon + 1);
                if ( secondcolon == -1 )
                    reply["event"] = controlnameevent.Substring(firstcolon + 1);
                else
                {
                    reply["event"] = controlnameevent.Substring(firstcolon + 1, secondcolon - firstcolon - 1);
                    reply["data"] = controlnameevent.Substring(secondcolon + 1);
                }
            };

            if (eventdata != null)
            {
                JToken tk = JToken.FromObject(eventdata, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                reply["value"] = tk;
            }
            if (eventdata2 != null)
            {
                JToken tk = JToken.FromObject(eventdata2, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                reply["value2"] = tk;
            }

            zmqconnection.Send(reply);
        }

        #endregion

        #region Events
        private void SendTerminate()
        {
            JObject reply = new JObject
            {
                ["responsetype"] = "terminate",
            };

            zmqconnection.Send(reply);
        }

        private void Discoveryform_OnHistoryChange()
        {
            if (zmqconnection != null)
            {
                JObject reply = new JObject
                {
                    ["responsetype"] = "historyload",
                    ["historylength"] = DiscoveryForm.History.Count,
                    ["commander"] = DiscoveryForm.History.CommanderName(),
                };

                zmqconnection.Send(reply);
            }
        }

        // travel history changed cursor
        public override void ReceiveHistoryEntry(EliteDangerousCore.HistoryEntry he)
        {
            if (zmqconnection != null)
            {
                JObject reply = new JObject
                {
                    ["responsetype"] = "travelhistorymoved",
                    ["row"] = he.Index,
                };

                zmqconnection.Send(reply);
            }
        }

        // new unfiltered journal entry
        private void DiscoveryForm_OnNewJournalEntryUnfiltered(EliteDangerousCore.JournalEntry obj)
        {
            if (zmqconnection != null)
            {
                JToken jo = JToken.FromObject(obj, true, new Type[] { typeof(Bitmap), typeof(Image) }, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                JObject reply = new JObject
                {
                    ["responsetype"] = "journalpush",
                    ["commander"] = EliteDangerousCore.EDCommander.Current.Name,
                    ["journalEntry"] = jo,
                };

                zmqconnection.Send(reply);
            }
        }

        // new history list entry
        private void Discoveryform_OnNewEntry(EliteDangerousCore.HistoryEntry he)
        {
            if (zmqconnection != null)
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

                jo["Info"] = he.GetInfo();      // add in these extra coded fields
                jo["Detailed"] = he.GetDetailed();

                zmqconnection.Send(reply);
            }
        }

        private void Discoveryform_OnNewUIEvent(EliteDangerousCore.UIEvent uievent)
        {
            if (zmqconnection != null)
            {
                QuickJSON.JToken t = QuickJSON.JToken.FromObject(uievent, ignoreunserialisable: true,
                                                            ignored: new Type[] { typeof(Bitmap), typeof(Image) },
                                                            maxrecursiondepth: 3);

                JObject reply = new JObject
                {
                    ["responsetype"] = "edduievent",
                    ["type"] = uievent.GetType().Name,
                    ["event"] = t,
                };

                zmqconnection.Send(reply);
            }
        }

        private void Discoveryform_ScreenShotCaptured(string file, Size size)
        {
            if (zmqconnection != null)
            {
                JObject reply = new JObject
                {
                    ["responsetype"] = "screenshot",
                    ["outfile"] = file,
                    ["width"] = size.Width,
                    ["height"] = size.Height,
                };

                zmqconnection.Send(reply);
            }
        }

        private void Discoveryform_OnNewTarget(object obj)
        {
            if (zmqconnection != null)
            {
                var hastarget = EliteDangerousCore.DB.TargetClass.GetTargetPosition(out string name, out double x, out double y, out double z);

                if (hastarget)
                {
                    JObject reply = new JObject
                    {
                        ["responsetype"] = "newtarget",
                        ["system"] = name,
                        ["X"] = x,
                        ["Y"] = y,
                        ["Z"] = z,
                    };

                    zmqconnection.Send(reply);
                }
            }
        }

        #endregion

        #region Helpers

        public void SelectView(bool dialog)
        {
            if (configurableUC.Visible != dialog)
            {
                panelLog.Visible = !dialog;
                configurableUC.Visible = dialog;
            }
        }

        public void Log(string logtext)
        {
            extRichTextBoxErrorLog.AppendText(logtext + Environment.NewLine);
            SelectView(false);
        }

        private void extButtonViewDialog_Click(object sender, EventArgs e)
        {
            SelectView(true);
        }
        #endregion

    }
}
