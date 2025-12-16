/*
 * Copyright © 2024-2025 EDDiscovery development team
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
using EliteDangerousCore.Spansh;
using QuickJSON;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static EliteDangerousCore.StarScan;

namespace EDDiscovery.UserControls
{
    public partial class UserControlZMQPanel : UserControlCommonBase
    {
        private Actions.ActionController actioncontroller;
        
        private string pluginfolder;
        private JToken config;
        private bool configgood;
        private int pluginapiversion;
        private bool hiddeneddui;
        private bool exitreceived = false;
        private bool initreceived = false;

        const int APIVERSION = 1;

        private System.Diagnostics.Process exeprocess;
        private NetMQUtils.NetMQJsonServer zmqconnection;
        private bool Running => (zmqconnection?.Running??false) && initreceived && !exitreceived;

        public UserControlZMQPanel()
        {
            InitializeComponent();
        }

        #region UCCB IF

        // called right after creation, before anything else
        protected override void Creation(PanelInformation.PanelInfo p)
        {
            base.Creation(p);
            System.Diagnostics.Debug.WriteLine($"MZQ panel create class {p.WindowTitle} db {DBBaseName}");
            DBBaseName = "MZQPanel" + p.PopoutID + ":";

            // the panel is created with the panel tag info set to the folder where the config file is in

            pluginfolder = p.Tag as string;

            // ensure we have a good config file, if not, set panelgood to bad and make an empty config JSON

            string configfilepath = Path.Combine(pluginfolder, "config.json");
            string configfilecontents =  BaseUtils.FileHelpers.TryReadAllTextFromFile(configfilepath);
            config = configfilecontents != null ? JToken.Parse(configfilecontents, JToken.ParseOptions.CheckEOL | JToken.ParseOptions.AllowTrailingCommas) : null;
            configgood = config != null;
            if (!configgood)
                config = new JToken();

            hiddeneddui = config["Panel"].I("Hidden").Bool(false);      // set if we want the EDD UI to be hidden, which may be if we are operating a python UI
        }

        protected override void Init()
        {
            System.Diagnostics.Debug.WriteLine($"ZMQ panel Init {DBBaseName}");

            SelectView(false);      // view log
            configurableUC.Dock = DockStyle.Fill;
            panelLog.Dock = DockStyle.Fill;

            if (configgood)
            {
                DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
                DiscoveryForm.OnNewUIEvent += Discoveryform_OnNewUIEvent;
                DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
                DiscoveryForm.OnNewJournalEntryUnfiltered += DiscoveryForm_OnNewJournalEntryUnfiltered;
                DiscoveryForm.ScreenShotCaptured += Discoveryform_ScreenShotCaptured;
                DiscoveryForm.OnNewTarget += Discoveryform_OnNewTarget;

                actioncontroller = DiscoveryForm.MakeAC(this.FindForm(), pluginfolder, null, null, null, (s)=> Log(s));     // action files are in this folder, don't allow management
            }
            else
                Log("Missing panel config.json file");

        }

        // from config file
        public override bool SupportTransparency { get { return config["Panel"].I("SupportTransparency").Bool(false); } } 
        public override bool DefaultTransparent { get { return config["Panel"].I("DefaultTransparent").Bool(false);} }
        
        // Action UI layout is done AFTER init as themeing is done between init and SetTransparency/LoadLayout
        // the UC sizes and themes itself
        protected override void LoadLayout()
        {
            if (!configgood)     // abort in bad state
                return; 

            configurableUC.Init("UC", "");

            actioncontroller.ReLoad();

            ActionLanguage.ActionFile af = actioncontroller.GetFile("UIInterface");
            if (af != null)
                af.Dialogs["UC"] = configurableUC;      // add the UC as a static dialog to the file
            else
                Log($"Missing UIInterface.act action file in ZMQ plugin folder {pluginfolder}");

            // hook any returns from action files to a report to the ZMQ
            actioncontroller.AddReturnCallBack((actf, fname, str) =>
            {
                System.Diagnostics.Debug.WriteLine($"ZMQ Panel Return closing return {af.Name} {fname} {str} ");
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

        // On Initial display, start up the ZMQ system
        protected override void InitialDisplay()
        {
            if (!configgood) // abort in bad situation
                return;

            bool good = false;

            JObject pythonconfig = config["Python"].Object();       // are we running a python program
            string startpyfile = pythonconfig?["Start"].StrNull();
            string pythonversion = pythonconfig["Version"].StrNull();     // see if forced version, may be null which means no force

            if (pythonconfig != null )      // if its a python launch
            {
                string[] pycontents = startpyfile != null ? BaseUtils.FileHelpers.TryReadAllLinesFromFile(Path.Combine(pluginfolder, startpyfile)) : null;

                if (pycontents != null && pycontents.Length > 1)
                {
                    int tries = 3;
                    while (tries-->0)
                    {
                        if (PythonModuleCheck() == false)
                        {
                            JArray modlist = pythonconfig["Modules"].Array();

                            if (modlist != null)
                            {
                                Log($"Module check failed, trying to install modules: {string.Join(", ", modlist)}");

                                string modulecheckfile = Path.Combine(pluginfolder, "pymodcheck.py");
                                string script = pycontents[0] + "\r\n" +       // shebang from start file
                                            "import subprocess\r\nimport sys\r\n" +
                                            "def install(package): subprocess.check_call([sys.executable, \"-m\", \"pip\", \"install\", package])\r\n";

                                foreach (var x in modlist)
                                {
                                    script += "print('Install Package ' + '" + x.Str() + "')\r\n";
                                    script += "install('" + x.Str() + "')\r\n";
                                }

                                File.WriteAllText(modulecheckfile, script);

                                var output = (Tuple<string, string>)BaseUtils.PythonLaunch.PyExeLaunch(modulecheckfile, "", pluginfolder, null, true, false, pythonversion);

                                if (output != null)
                                {
                                    Log($"Module check ran, gave:");
                                    Log(output.Item1);
                                    Log(output.Item2);
                                    FileHelpers.DeleteFileNoError(modulecheckfile);
                                }
                                else
                                {
                                    Log($"Module check failed to run. Check your python install. Ensure py.exe is installed and runnable from the command line");
                                    break;
                                }
                            }
                            else
                            {
                                Log("Modules check failed, check your python install. Manually add modules if required");
                            }
                        }
                        else
                        {
                            good = true;
                            break;
                        }
                    }

                    if ( tries < 0)
                        Log("Modules check failed - panel cannot be run. Check your python installation.  Make sure py.exe works from the command line.");
                }
                else
                {
                    Log($"Missing start file {startpyfile}");
                }
            }

            if (good)
            {
                try
                {
                    int socketnumber = EDDOptions.Instance.ZMQPort;
                    zmqconnection = new NetMQUtils.NetMQJsonServer();
                    zmqconnection.Received += (list) => { 
                        //System.Diagnostics.Debug.WriteLine($"Received {list[0].ToString()}");
                        this.BeginInvoke((MethodInvoker)delegate { HandleClientMessages(list); }); };

                    zmqconnection.Accepted += () => { System.Diagnostics.Debug.WriteLine($"Accepted"); };
                    zmqconnection.Disconnected += () => { System.Diagnostics.Debug.WriteLine($"Disconnected"); this.BeginInvoke((MethodInvoker)delegate { Log("ZMQ Disconnected"); }); };

                    string threadname = "NetMQPoller:" + DBBaseName;
                    if (zmqconnection.Init("tcp://localhost", socketnumber, threadname))
                    {
                        // socket numbers <10000 do not launch the exe, instead you should have the debugger running it ready to connect 
                        if (socketnumber >= 10000)
                        {
                            if (startpyfile != null)      // if starting python
                            {
                                bool createnowindow = pythonconfig["CreateNoWindow"].Bool(false);

                                exeprocess = (System.Diagnostics.Process)BaseUtils.PythonLaunch.PyExeLaunch(startpyfile, $"{socketnumber.ToStringInvariant()} {DisplayNumber}", pluginfolder, null, false, createnowindow, pythonversion);

                                if (exeprocess == null)
                                {
                                    Log($"Cannot launch {pluginfolder} / {startpyfile}");
                                    zmqconnection.Close();
                                }
                                else
                                {
                                    EDDOptions.Instance.ZMQPort++;      // python launched, next window gets another port number
                                    Log($"Running plugin on port {socketnumber}, awaiting connection");
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.Assert(false, "To add exe launch");
                            }
                        }
                        else
                        {
                            Log("Running in debug mode, run plugin in your IDE now");
                        }

                        configurableUC.TriggerAdv += ConfigurableUC_TriggerAdv;
                    }
                    else
                    {
                        Log($"Server failed to start on socket number {socketnumber} - probably another panel is active without having been closed properly. Close using task manager, close this panel and retry");
                        zmqconnection = null;
                    }
                }
                catch (Exception ex)
                {
                    Log($"Cannot launch {pluginfolder} exception {ex}");
                    zmqconnection.Close();
                }
            }
        }

        public override bool AllowClose() 
        {
            if ( Running )     // protect against close before server running, only if we have initialised
            {
                //userclosing = true;     // user wanted close, note. exit will ask for another close, and with exitreceived, it will accept

                System.Diagnostics.Debug.WriteLine($"ZMQ {DBBaseName} AllowClose Send terminate");

                SendTerminate();

                MSTicks ms = new MSTicks(1000);

                System.Diagnostics.Debug.WriteLine($"ZMQ {DBBaseName} wait for exit received back for a while");

                // we horribly just sit here waiting for the plugin to respond with exit
                // if we don't then the panel will close before the exit is received
                // we can't pend the close as this gets called by the tabs/splitters as well and they can't be pended.

                while (!exitreceived && zmqconnection.Running && !ms.TimedOut)        
                {
                    Application.DoEvents();
                    System.Diagnostics.Debug.WriteLine("ZMQ waiting for timeout");
                    System.Threading.Thread.Sleep(50);      // do not do  as we will end up double closing
                }

                System.Diagnostics.Debug.WriteLine($"ZMQ {DBBaseName} did it received exit {exitreceived}");
            }

            return true;
        }

        protected override void Closing()
        {
            //System.Diagnostics.Debug.WriteLine($"ZMQ Closing {Environment.StackTrace}");
            System.Diagnostics.Debug.WriteLine($"ZMQ {DBBaseName} Closing ");

            if (!configgood)
                return;

            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewJournalEntryUnfiltered -= DiscoveryForm_OnNewJournalEntryUnfiltered;
            DiscoveryForm.ScreenShotCaptured -= Discoveryform_ScreenShotCaptured;
            DiscoveryForm.OnNewTarget -= Discoveryform_OnNewTarget;

            System.Diagnostics.Debug.WriteLine($"ZMQ {DBBaseName} close zmq ");
            zmqconnection?.Close();

            if (exeprocess != null && !exeprocess.HasExited)
            {
                exeprocess.Kill();
            }

            System.Diagnostics.Debug.WriteLine($"ZMQ {DBBaseName} close action controller");
            actioncontroller.CloseDown();       // with persistent vars if required
            System.Diagnostics.Debug.WriteLine($"ZMQ {DBBaseName} finish close");

            configgood = false;
        }

        // gets called before load layout, so ignore until running, called when moves from transparent or back
        protected override void SetTransparency(bool ison, Color curcol)
        {
            if (Running)
            {
                JObject reply = new JObject
                {
                    ["responsetype"] = "settransparency",
                    ["transparencymode"] = TransparentMode.ToString(),
                    ["istransparent"] = ison,
                    ["currentbackground"] = System.Drawing.ColorTranslator.ToHtml(curcol)
                };

                System.Diagnostics.Debug.WriteLine($"ZMQ Panel transparency changed {ison} {curcol}");

                zmqconnection.Send(reply);
            }
        }

        // gets called before load layout, so ignore until running, called when user changes mode from on->off->on
        protected override void TransparencyModeChanged(bool on)
        {
            if (Running)
            {
                JObject reply = new JObject
                {
                    ["responsetype"] = "transparencymodechanged",
                    ["transparencymode"] = TransparentMode.ToString(),
                    ["istransparent"] = on,
                };

                System.Diagnostics.Debug.WriteLine($"ZMQ Panel transparency mode changed {on}");
                zmqconnection.Send(reply);
            }
        }


        #endregion

        #region From client


        // Message received from plugin, in UI thread due to thunk above
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
                            pluginapiversion = json["apiversion"].Int(0);

                            string curver = System.Reflection.Assembly.GetExecutingAssembly().GetAssemblyVersionString();
                            JObject reply = new JObject
                            {
                                ["responsetype"] = "start",
                                ["eddversion"] = curver,
                                ["apiversion"] = APIVERSION,
                                ["historylength"] = DiscoveryForm.History.Count,
                                ["commander"] = commander,
                                ["config"] = GetSetting("Config", ""),
                                ["transparencymode"] = TransparentMode.ToString(),
                                ["istransparent"] = IsCurrentlyTransparent,
                                ["transparencycolorkey"] = System.Drawing.ColorTranslator.ToHtml(TransparentKey)
                            };

                            exitreceived = false;
                            initreceived = true;
                            zmqconnection.Send(reply);
                            Log($"Connected with version {json["version"].Str()} at API {pluginapiversion}");
                            SelectView(true);

                            if (hiddeneddui)
                                FindForm()?.Hide();
                            break;
                        }

                    case "exit":
                        {
                            string reason = json["reason"].Str();
                            if (reason.HasChars())
                                Log("Panel requested termination due to " + reason,false);  // don't force visibility

                            string config = json["config"].Str();
                            if (config.HasChars())
                            {
                                PutSetting("Config", config);
                                System.Diagnostics.Debug.WriteLine($"ZMQ config {config}");
                            }

                            exitreceived = true;

                            bool closewindow = json["close"].Bool(false);

                            if (closewindow)        // if asked to close, close it
                            {
                                if ( !IsClosed)     // paranoia
                                    RequestClose();
                            }
                            else if (hiddeneddui)       // else make panel visible 
                                FindForm()?.Show();
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
                                jo["InfoText"] = he.GetInfo();
                                jo["DetailedText"] = he.GetDetailed();
                                rows.Add(jo);
                                //System.Diagnostics.Debug.WriteLine($"Return {jo.ToString(true)}");
                                //System.Diagnostics.Debug.WriteLine($"Return {jo.ToString()}");
                            }
                            zmqconnection.Send(reply);
                        }

                        break;

                    case "journal":
                        {
                            int last = json["last"].Int();
                            int cmdrid = DiscoveryForm.History.CommanderId;
                            int jcount = 0;
                            JArray output = null;

                            if ( DiscoveryForm.History.HistoryLoaded)
                            {
                                var tlulist = EliteDangerousCore.DB.TravelLogUnit.GetCommander(cmdrid);
                                jcount = tlulist.Count;
                                int tlunumber = jcount - last - 1;

                                if (tlunumber > 0 && tlunumber < jcount)
                                {
                                    var tabledata = JournalEntry.GetTableData(new System.Threading.CancellationToken(), cmdrid, tluid: tlulist[tlunumber].ID);
                                    output = (JArray)JToken.FromObject(tabledata.Select(x => x.Json), true);
                                }
                            }

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["last"] = last,
                                ["count"] = jcount,
                                ["commander"] = commander,
                                ["journal"] = output,
                            };
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
                            int entry = json["entry"].Int();
                            HistoryEntry he = GetHEByIndex(entry);

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["entry"] = entry,
                                ["entryreturned"] = he?.Index ?? -1,
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

                            //System.Diagnostics.Debug.WriteLine($"Mission response {reply.ToString(true)}");

                            zmqconnection.Send(reply);
                        }
                        break;

                    case "ship":
                        {
                            int entry = json["entry"].Int();
                            HistoryEntry he = GetHEByIndex(entry);

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["entry"] = entry,
                                ["entryreturned"] = he?.Index ?? -1,
                                ["ship"] = he == null ? null : JToken.FromObject(he.ShipInformation, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),
                            };
                            //System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
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
                            //System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;

                    case "suitsweapons":
                        {
                            int entry = json["entry"].Int();

                            HistoryEntry he = GetHEByIndex(entry);

                            var sl = he != null ? hl.SuitList.Suits(he.Suits) : null;
                            var wp = he != null ? hl.WeaponList.Weapons(he.Weapons) : null;
                            var lo = he != null ? hl.SuitLoadoutList.Loadouts(he.Loadouts) : null;

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["entry"] = entry,
                                ["entryreturned"] = he?.Index ?? -1,
                                ["suits"] = sl == null ? null : JToken.FromObject(sl, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),
                                ["weapons"] = wp == null ? null : JToken.FromObject(wp, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),
                                ["loadouts"] = lo == null ? null : JToken.FromObject(lo, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),

                            };

                            //System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;

                    case "carrier":
                        {
                            var cr = hl.FleetCarrier;

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["carrier"] = cr == null ? null : JToken.FromObject(cr, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),

                            };
                            //System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
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
                            //System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;

                    case "stationshipyardoutfitting":
                        {
                            string station = json["station"].StrNull();
                            var sy = station != null ? hl.Shipyards.Get(station) : null;
                            var of = station != null ? hl.Outfitting.Get(station) : null;
                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["station"] = station,
                                ["shipyard"] = sy == null ? null : JToken.FromObject(sy, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),
                                ["outfitting"] = of == null ? null : JToken.FromObject(of, true, null, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public),
                            };
                            System.Diagnostics.Debug.WriteLine($"Return {reply.ToString(true)}");
                            zmqconnection.Send(reply);
                        }
                        break;

                    case "shipyards":
                        {
                            var cr = hl.Shipyards.GetFilteredList(true);        // filter the list, latests first, without repeats

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
                            var cr = hl.Outfitting.GetFilteredList(true);

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

                    case "spanshdump":
                        {
                            string system = json["system"].Str();
                            long? systemid = json["systemid"].LongNull();
                            bool weblookup = json["weblookup"].Bool();
                            bool cachelookup = json["cachelookup"].Bool();
                            var sys = new SystemClass(system, systemid > 0 ? systemid : default(long?));
                            var dump = await SpanshClass.GetSpanshDumpAsync(sys, weblookup, cachelookup);
                            if (IsClosed)
                                return;

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["system"] = system,
                                ["systemid"] = systemid,
                                ["dump"] = dump,
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
                            int entry = json["entry"].Int();
                            HistoryEntry he = GetHEByIndex(entry);

                            var cr = he!=null ? hl.MaterialCommoditiesMicroResources.Get(he.MaterialCommodity) : null;

                            JObject reply = new JObject
                            {
                                ["responsetype"] = request,
                                ["entry"] = entry,
                                ["entryreturned"] = he?.Index ?? -1,
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
                                Log($"Panel suspend unknown control");
                        }
                        break;
                    case "uiresume":
                        {
                            if (!configurableUC.Resume(controlname))
                                Log($"Panel resume unknown control");
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
                                        Log($"Panel error making new control: {str}");
                                }

                                configurableUC.UpdateEntries();
                            }
                            else
                                Log($"Panel error making new control: missing control array");

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
                                        Log($"Panel error removing {str}");
                                }

                                configurableUC.UpdateEntries();
                            }
                            else
                                Log($"Panel error removing controls: missing control array");
                        }
                        break;
                    case "uiaddsetrows":
                        {
                            var changelist = json["changelist"].Array();
                            string err = changelist != null ? configurableUC.AddSetRows(controlname, changelist) : "No change list";
                            if (err != null)
                                Log($"Panel error addsetrows {controlname} : {err}");
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
                                            Log($"Panel error insertcolumns {controlname}");
                                    }
                                }
                            }
                            else
                                Log($"Panel error insertcolumns {controlname} no columns");
                        }
                        break;
                    case "uiremovecolumns":
                        {
                            int pos = json["position"].Int(0);
                            int count = json["count"].Int(1);
                            if (!configurableUC.RemoveColumns(controlname,pos,count))
                                Log($"Panel error removecolumns {controlname}");
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
                                    Log($"Panel error rightclickmenu unknown control");
                            }
                            catch
                            {
                                Log($"Panel error rightclickmenu missing fields");
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
                                Log($"Panel error getcolumnsetting unknown control");

                        }
                        break;
                    case "uisetcolumnssetting":
                        {
                            JToken tk = json["settings"];
                            if ( tk == null || !configurableUC.SetDGVColumnSettings(controlname,tk))
                                Log($"Panel error setcolumnsetting missing data or unknown control");
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
                                Log($"Panel error setdgvsetting missing data or unknown control");
                        }
                        break;
                    case "uisetwordwrap":
                        {
                            bool? ww = json["wordwrap"].BoolNull();
                            if (ww.HasValue && configurableUC.SetWordWrap(controlname,ww.Value))
                            {

                            }
                            else
                                Log($"Panel error setwordwrap missing data or unknown control");
                        }
                        break;
                    case "uiclear":
                        {
                            if (!configurableUC.Clear(controlname))
                                Log($"Panel error clearing {controlname}");
                        }
                        break;
                    case "uiremoverows":
                        {
                            int rowstart = json["rowstart"].Int();
                            int count = json["count"].Int();
                            int ret = configurableUC.RemoveRows(controlname, rowstart, count);
                            if ( ret < 0 )
                                Log($"Panel error removing rows {controlname}");
                        }
                        break;
                    case "uienable":
                        {
                            bool state = json["state"].Bool(false);
                            if (!configurableUC.SetEnable(controlname, state))
                                Log($"Panel error enable state {controlname}");
                        }
                        break;
                    case "uivisible":
                        {
                            bool state = json["state"].Bool(false);
                            if (!configurableUC.SetVisible(controlname, state))
                                Log($"Panel error visible state {controlname}");
                        }
                        break;
                    case "uiposition":
                        {
                            if (!configurableUC.SetPosition(controlname, new Point(json["x"].Int(), json["y"].Int())))
                                Log($"Panel error position {controlname}");

                        }
                        break;
                    case "uisize":
                        {
                            if (!configurableUC.SetSize(controlname, new Size(json["width"].Int(), json["height"].Int())))
                                Log($"Panel error size {controlname}");
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


        HistoryEntry GetHEByIndex(int entry)
        {
            HistoryList hl = DiscoveryForm.History;
            if (entry < 0 || entry >= hl.Count)
                entry = hl.Count - 1;
            return hl.Count>0 ? hl[entry] : null;
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
            JObject reply = new JObject     // already triaged we are running
            {
                ["responsetype"] = "terminate",
            };

            zmqconnection.Send(reply);
        }

        private void Discoveryform_OnHistoryChange()
        {
            if (Running)
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
            if (Running)
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
            JToken jo = JToken.FromObject(obj, true, new Type[] { typeof(Bitmap), typeof(Image) }, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            System.Diagnostics.Debug.WriteLine($"ZMQ Received NewJournalEntryUnfiltered {jo.ToString()}");
            if (Running)
            {

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
            if (Running)
            {

                JObject reply = new JObject
                {
                    ["responsetype"] = "historypush",
                    ["firstrow"] = he.Index,
                    ["length"] = 1,
                    ["commander"] = EliteDangerousCore.EDCommander.Current.Name,
                    ["rows"] = new JArray()
                };

                JToken jo = JToken.FromObject(he, true, new Type[] { typeof(Bitmap), typeof(Image),
                                                        typeof(EliteDangerousCore.EDCommander) }, 8, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                jo["InfoText"] = he.GetInfo();      // add in these extra coded fields
                jo["DetailedText"] = he.GetDetailed();

                reply["rows"].Array().Add(jo);

                zmqconnection.Send(reply);
            }
        }

        private void Discoveryform_OnNewUIEvent(EliteDangerousCore.UIEvent uievent)
        {
            if (Running)
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
            if (Running)
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
            if (Running)
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
                System.Diagnostics.Debug.WriteLine($"ZMQ Swap to dialog view {dialog}");
            }
        }

        // print to log file, and force visibility normally
        public void Log(string logtext, bool showlog = true)
        {
            if (!IsClosed)
            {
                System.Diagnostics.Debug.WriteLine($"ZMQ Log: {showlog} : {logtext}");
                extRichTextBoxErrorLog.AppendText(logtext + Environment.NewLine);
                if (showlog)
                {
                    SelectView(false);
                }
            }
        }

        private void extButtonViewDialog_Click(object sender, EventArgs e)
        {
            SelectView(true);
        }

        // for python, run the ModulesCheck py.exe to check for modules ok
        private bool PythonModuleCheck()
        {
            JObject pythonconfig = config["Python"].Object();       // are we running a python program

            if (pythonconfig["ModulesCheck"] != null)           // if it wants a module check procedure
            {
                string modulecheckfile = Path.Combine(pluginfolder, pythonconfig["ModulesCheck"].Str());

                string version = pythonconfig["Version"].StrNull();     // see if forced version, may be null which means no force

                // launch the check file.. this also checked py.exe is available

                var output = (Tuple<string, string>)BaseUtils.PythonLaunch.PyExeLaunch(modulecheckfile, "", pluginfolder, null, true, true, version);

                if (output == null)
                {
                    Log("ERROR: py.exe is not available. Check your python installation. Make sure during install you clicked on \"py Launcher\"");
                    return false;
                }
                else if (!output.Item1.Contains("Module Check OK"))   // must contain this
                {
                    Log("ERROR: Python Module check gave:");
                    Log(output.Item1);
                    Log(output.Item2);
                    return false;
                }
                else
                    Log("Success. Python Module check ran successfully");
            }

            return true;

        }


        #endregion

    }
}
