/*
 * Copyright © 2015 - 2023 EDDiscovery development team
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

using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.GMO;
using QuickJSON;
using System;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class EDDiscoveryForm
    {
        private EDDDLLInterfaces.EDDDLLIF.EDDCallBacks DLLCallBacks;
        private Tuple<string, string, string, string> dllresults;   // hold results between load and shown
        private string dllsalloweddisallowed; // holds DLL allowed between load and shown

        // called on Load
        public void DLLStart()
        {
            EliteDangerousCore.DLL.EDDDLLAssemblyFinder.AssemblyFindPaths.Add(EDDOptions.Instance.DLLAppDirectory());      // any needed assemblies from here
            var dllexe = EDDOptions.Instance.DLLExeDirectory();     // and possibly from here, may not be present
            if (dllexe != null)
                EliteDangerousCore.DLL.EDDDLLAssemblyFinder.AssemblyFindPaths.Add(dllexe);
            AppDomain.CurrentDomain.AssemblyResolve += EliteDangerousCore.DLL.EDDDLLAssemblyFinder.AssemblyResolve;

            DLLManager = new EliteDangerousCore.DLL.EDDDLLManager();
            DLLCallBacks = new EDDDLLInterfaces.EDDDLLIF.EDDCallBacks();
            DLLCallBacks.ver = 4;       // explicitly,this is what we do
            System.Diagnostics.Debug.Assert(DLLCallBacks.ver == EDDDLLInterfaces.EDDDLLIF.DLLCallBackVersion, "***** Updated EDD DLL IF but not updated callbacks");
            DLLCallBacks.RequestHistory = DLLRequestHistory;
            DLLCallBacks.RunAction = DLLRunAction;
            DLLCallBacks.GetShipLoadout = DLLGetShipLoadout;
            DLLCallBacks.WriteToLog = (s) => LogLine(s);
            DLLCallBacks.WriteToLogHighlight = (s) => LogLineHighlight(s);
            DLLCallBacks.RequestScanData = DLLRequestScanData;
            DLLCallBacks.RequestScanDataExt = DLLRequestScanDataExt;
            DLLCallBacks.GetSuitsWeaponsLoadouts = DLLGetSuitWeaponsLoadout;
            DLLCallBacks.GetCarrierData = DLLGetCarrierData;
            DLLCallBacks.GetVisitedList = DLLGetVisitedList;
            DLLCallBacks.GetShipyards = DLLGetShipyards;
            DLLCallBacks.GetOutfitting = DLLGetOutfitting;
            DLLCallBacks.GetTarget = DLLGetTarget;
            DLLCallBacks.GetGMOs = DLLGetGMOs;
            DLLCallBacks.AddPanel = (id, paneltype, wintitle, refname, description, image) =>
            {
                // registered panels, search the stored list, see if there, then it gets the index, else its added to the list
                int panelid = EDDConfig.Instance.FindCreatePanelID(id);

                // IF we had more versions of IEDDPanelExtensions in future, we would add more clauses here and have other UserControlExtPanel classes to handle them

                if (typeof(EDDDLLInterfaces.EDDDLLIF.IEDDPanelExtension).IsAssignableFrom(paneltype))
                {
                    AddPanel(panelid, typeof(UserControls.UserControlExtPanel), paneltype, wintitle, refname, description, image,false);
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine($"***** DLL unknown panel interface type - ignoring {id}");
                }
            };

            dllsalloweddisallowed = EDDConfig.Instance.DLLPermissions;

            dllresults = DLLLoad();       // we run it, and keep the results for processing in Shown
        }

        // load DLLs
        public Tuple<string, string, string, string> DLLLoad()
        {
            System.Diagnostics.Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Load DLL");

            string verstring = EDDApplicationContext.AppVersion;
            string[] options = new string[] { EDDDLLInterfaces.EDDDLLIF.FLAG_HOSTNAME + "EDDiscovery",
                                              EDDDLLInterfaces.EDDDLLIF.FLAG_JOURNALVERSION + EliteDangerousCore.DLL.EDDDLLCallerHE.JournalVersion.ToStringInvariant(),
                                              EDDDLLInterfaces.EDDDLLIF.FLAG_CALLBACKVERSION + DLLCallBacks.ver.ToStringInvariant(),
                                              EDDDLLInterfaces.EDDDLLIF.FLAG_CALLVERSION + EliteDangerousCore.DLL.EDDDLLCaller.DLLCallerVersion.ToStringInvariant(),
                                              EDDDLLInterfaces.EDDDLLIF.FLAG_PANELCALLBACKVERSION + UserControls.UserControlExtPanel.PanelCallBackVersion.ToStringInvariant(),
                                            };

            string[] dllpaths = new string[] { EDDOptions.Instance.DLLAppDirectory(), EDDOptions.Instance.DLLExeDirectory() };
            bool[] autodisallow = new bool[] { false, true };
            return DLLManager.Load(dllpaths, autodisallow, verstring, options, DLLCallBacks, ref dllsalloweddisallowed,
                                                             (name) => UserDatabase.Instance.GetSettingString("DLLConfig_" + name, ""), (name, set) => UserDatabase.Instance.PutSettingString("DLLConfig_" + name, set));
        }



        // called on Show
        public void DLLVerify()
        {
            if (dllresults.Item3.HasChars())       // new DLLs
            {
                string[] list = dllresults.Item3.Split(',');
                bool changed = false;
                foreach (var dll in list)
                {
                    if (ExtendedControls.MessageBoxTheme.Show(this,
                                    string.Format(("The following application extension DLL have been found" + Environment.NewLine +
                                    "Do you wish to allow it to be used?" + Environment.NewLine + Environment.NewLine +
                                    "{0} " + Environment.NewLine
                                    ).T(EDTx.EDDiscoveryForm_DLLW), dll),
                                    "Warning".T(EDTx.Warning),
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        dllsalloweddisallowed = dllsalloweddisallowed.AppendPrePad("+" + dll, ",");
                        changed = true;
                    }
                    else
                    {
                        dllsalloweddisallowed = dllsalloweddisallowed.AppendPrePad("-" + dll, ",");
                    }
                }

                if (changed)
                {
                    DLLManager.UnLoad();
                    System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF Reload DLL");
                    dllresults = DLLLoad();
                }
            }

            EDDConfig.Instance.DLLPermissions = dllsalloweddisallowed;        // write back the permission string

            if (dllresults.Item1.HasChars())   // ok
                LogLine(string.Format("DLLs loaded: {0}".T(EDTx.EDDiscoveryForm_DLLL), dllresults.Item1));
            if (dllresults.Item2.HasChars())   // failed
                LogLineHighlight(string.Format("DLLs failed to load: {0}".T(EDTx.EDDiscoveryForm_DLLF), dllresults.Item2));
            if (dllresults.Item4.HasChars())   // failed
                LogLine(string.Format("DLLs disabled: {0}".T(EDTx.EDDiscoveryForm_DLLDIS), dllresults.Item4));
        }


        public bool DLLRunAction(string eventname, string paras)
        {
            System.Diagnostics.Debug.WriteLine("Run " + eventname + "(" + paras + ")");
            actioncontroller.ActionRun(Actions.ActionEventEDList.DLLEvent(eventname), new BaseUtils.Variables(paras, BaseUtils.Variables.FromMode.MultiEntryComma));
            return true;
        }

        private bool DLLRequestHistory(long index, bool isjid, out EDDDLLInterfaces.EDDDLLIF.JournalEntry f)
        {
            HistoryEntry he = isjid ? History.GetByJID(index) : History.GetByEntryNo((int)index);
            f = EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(History, he);
            return he != null;
        }
        private string DLLGetShipLoadout(string name)
        {
            if ( name.EqualsIIC("All"))
            {
                JObject ships = new JObject();
                int index = 0;
                foreach( var sh in History.ShipInformationList.Ships)
                {
                    ships[index++.ToStringInvariant()] = JToken.FromObject(sh.Value, true, new Type[] { }, 5, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                }

                //BaseUtils.FileHelpers.TryWriteToFile(@"c:\code\dllshiploadout.json", ships.ToString(true));
                return ships.ToString();
            }
            else
            {
                var sh = !name.IsEmpty() ? History.ShipInformationList.GetShipByFullInfoMatch(name) : History.ShipInformationList.CurrentShip;
                var ret = sh != null ? JToken.FromObject(sh, true, new Type[] { }, 5, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).ToString(true) : null;
                //BaseUtils.FileHelpers.TryWriteToFile(@"c:\code\dllshiploadout.json", ret);
                return ret;
            }
        }

        private string DLLGetTarget()
        {
            var hastarget = EliteDangerousCore.DB.TargetClass.GetTargetPosition(out string name, out double x, out double y, out double z);
            JObject jo = hastarget ? new JObject() { ["Name"] = name, ["X"] = x, ["Y"] = y, ["Z"] = z } : new JObject();
            return jo.ToString();
        }

        // Note ASYNC so we must use data return method
        // 14/1/25 bool means spansh then edsm
        private async void DLLRequestScanData(object requesttag, object usertag, string systemname, bool spanshthenedsmlookup)
        {
            var dll = DLLManager.FindCSharpCallerByStackTrace();    // need to find who called - use the stack to trace the culprit

            if (dll != null)
            {
                var syslookup = systemname.IsEmpty() ? History.CurrentSystem()?.Name : systemname;      // get a name

                JToken json = null;

                if (syslookup.HasChars())
                {
                    var sc = History.StarScan;

                    var sysclass = new SystemClass(syslookup);

                    WebExternalDataLookup wlu = spanshthenedsmlookup ? EliteDangerousCore.WebExternalDataLookup.SpanshThenEDSM : EliteDangerousCore.WebExternalDataLookup.None;

                   //wlu = WebExternalDataLookup.EDSM; //debug

                    // async lookup
                    var snode = await sc.FindSystemAsync(sysclass, wlu);

                    if (snode != null)
                    {
                        json = JToken.FromObject(snode, true, new Type[] { typeof(System.Drawing.Image) }, 24, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    }
                }

                if ( json == null )
                    json = new JObject();   // default return

                BaseUtils.FileHelpers.TryWriteToFile(@"c:\code\dllscan.json", json.ToString(true));
                dll.DataResult(requesttag, usertag, json.ToString());
            }

        }

        private async void DLLRequestScanDataExt(object requesttag, object usertag, string systemname, long systemaddress, int weblookup, string _)
        {
            var dll = DLLManager.FindCSharpCallerByStackTrace();    // need to find who called - use the stack to trace the culprit

            if (dll != null)
            {
                //systemaddress = 10477373803; weblookup = 1; // debug

                ISystem sysc = systemname.IsEmpty() && systemaddress == 0 ? History.CurrentSystem() :
                                        new SystemClass(systemname, systemaddress > 0 ? systemaddress : default(long?));

                JToken json = null;

                if (sysc != null)
                {
                    var sc = History.StarScan;

                    WebExternalDataLookup wlu = weblookup == 3 ? WebExternalDataLookup.SpanshThenEDSM : weblookup == 2 ? WebExternalDataLookup.Spansh :
                                                    weblookup == 1 ? WebExternalDataLookup.EDSM : WebExternalDataLookup.None;
                    // async lookup
                    var snode = await sc.FindSystemAsync(sysc, wlu);

                    if (snode != null)
                    {
                        json = JToken.FromObject(snode, true, new Type[] { typeof(System.Drawing.Image) }, 24, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    }
                }

                if (json == null)
                    json = new JObject();   // default return

                BaseUtils.FileHelpers.TryWriteToFile(@"c:\code\dllscan.json", json.ToString(true));
                dll.DataResult(requesttag, usertag, json.ToString());
            }

        }


        private string DLLGetGMOs(string ctrlstring)
        {
            JToken jt = null;
            if (ctrlstring.EqualsIIC("all"))
            {
                jt = JToken.FromObject(GalacticMapping.AllObjects, true);
            }
            else if (ctrlstring.EqualsIIC("visible"))
            {
                jt = JToken.FromObject(GalacticMapping.VisibleMapObjects, true);
            }
            else if (ctrlstring.StartsWithIIC("name="))
            {
                jt = JToken.FromObject(GalacticMapping.FindDescriptiveNames(ctrlstring.Substring(5),true,false), true);
            }
            else if (ctrlstring.StartsWithIIC("systemname="))
            {
                jt = JToken.FromObject(GalacticMapping.FindSystems(ctrlstring.Substring(11)), true);
            }

            string output = jt?.ToString(true) ?? "ERROR";

            return output;
        }


        private string DLLGetSuitWeaponsLoadout()
        {
            var wlist = JToken.FromObject(History.WeaponList.weapons.Get(History.GetLast?.Weapons ?? 0), true, new Type[] { typeof(System.Drawing.Image) }, 12, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var slist = JToken.FromObject(History.SuitList.Suits(History.GetLast?.Suits ?? 0), true, new Type[] { typeof(System.Drawing.Image) }, 12, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var sloadoutlist = JToken.FromObject(History.SuitLoadoutList.Loadouts(History.GetLast?.Loadouts ?? 0), true, new Type[] { typeof(System.Drawing.Image) }, 12, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            JObject ret = new JObject();
            ret["Weapons"] = wlist;
            ret["Suits"] = slist;
            ret["Loadouts"] = sloadoutlist;
            //BaseUtils.FileHelpers.TryWriteToFile(@"c:\code\dllsuits.json", ret.ToString(true));
            return ret.ToString();
        }

        private string DLLGetCarrierData()
        {
            var carrier = JToken.FromObject(History.Carrier, true, new Type[] { typeof(System.Drawing.Image) }, 12, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            //BaseUtils.FileHelpers.TryWriteToFile(@"c:\code\dllcarrier.json", carrier.ToString(true));
            return carrier.ToString();
        }

        private class VisitedSystem     // for JSON export
        {
            public string Name;
            public double X; public double Y; public double Z;
            public long SA;
            public VisitedSystem(string n,long addr,double x,double y,double z) { Name = n;SA = addr;X = x;Y = y;Z = z; }
        };

        private string DLLGetVisitedList(int howmany)
        {
            var list = History.Visited.Values;
            int toskip = howmany > list.Count || howmany < 0 ? list.Count : list.Count-howmany;
            var vlist = list.Skip(toskip).Select(x => new VisitedSystem(x.System.Name,x.System.SystemAddress??-1,x.System.X,x.System.Y,x.System.Z)).ToList();
            var visited = JToken.FromObject(vlist, true, new Type[] { typeof(System.Drawing.Image) }, 12, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var str = visited.ToString();
            //BaseUtils.FileHelpers.TryWriteToFile(@"c:\code\dllvisited.json", str);
            return str;
        }
        private string DLLGetShipyards()
        {
            var shipyards = JToken.FromObject(History.Shipyards, true, new Type[] { typeof(System.Drawing.Image) }, 12, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            //BaseUtils.FileHelpers.TryWriteToFile(@"c:\code\dllshipyards.json", shipyards.ToString(true));
            return shipyards.ToString();
        }
        private string DLLGetOutfitting()
        {
            var outfitting = JToken.FromObject(History.Outfitting, true, new Type[] { typeof(System.Drawing.Image) }, 12, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            //BaseUtils.FileHelpers.TryWriteToFile(@"c:\code\dlloutfitting.json", outfitting.ToString(true));
            return outfitting.ToString();
        }



    }
}
