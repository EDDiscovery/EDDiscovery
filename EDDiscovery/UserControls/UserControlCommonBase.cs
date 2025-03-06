/*
 * Copyright © 2016 - 2022 EDDiscovery development team
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
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{

    public class UserControlCommonBase : UserControl, EliteDangerousCore.DB.IUserDatabaseSettingsSaver
    {
        #region Status

        public const int DisplayNumberPrimaryTab = 0;               // tabs are 0, or 100+.  0 for the first, 100+ for repeats
        public const int DisplayNumberPopOuts = 1;                  // pop outs are 1-99.. of each specific type.
        public const int DisplayNumberStartExtraTabs = 100;         // extra tabs are assigned here
        public const int DisplayNumberStartExtraTabsMax = 199;
        public const int DisplayNumberSplitterStart = 1050;

        // When a grid or splitter is open, displaynumber for its children based on its own number
        // 1050 is historical.. 1000..1049 was reserved for the previous history window splitters

        protected int DisplayNumberOfSplitter(int numopenedinside)  // splitter children are assigned this range..
        { return DisplayNumberSplitterStart + DisplayNumber * 100 + numopenedinside; }

        protected int DisplayNumberOfGrid(int numopenedinside)      // grid children are assigned this range..  allow range for splitters.
        { return DisplayNumberSplitterStart + (DisplayNumberStartExtraTabsMax + 1) * 100 + DisplayNumber * 100 + numopenedinside; }

        // Common parameters of a UCCB

        public PanelInformation.PanelIDs PanelID { get; private set; }  // set on creation 
        public int DisplayNumber { get; private set; }                // set on Init
        public bool IsPrimaryHistoryDisplayNumber { get { return DisplayNumber == DisplayNumberSplitterStart; } }
        public EDDiscoveryForm DiscoveryForm { get; private set; }    // set on Init    
        public UserControlCommonBase ParentUCCB { get; set; }    // only for UCCBs under UCCBs, this is set to the parent UCCB (search)
        public bool IsClosed { get; private set; }                    // set after CloseDown called. Use this if your doing await stuff which may mean your class gets called after close

        public bool IsFloatingWindow { get { return this.FindForm() is UserControlForm; } }   // ultimately its a floating window
        public bool IsControlTextVisible()
        {
            return (this.Parent is UserControlForm) ? ((UserControlForm)(this.Parent)).IsControlTextVisible() : true;
        }
        public bool HasControlTextArea()
        {
            return (this.Parent is ExtendedControls.TabStrip) || (this.Parent is UserControlForm) || (this.Parent is UserControlContainerResizable);
        }

        // this means the transparent mode is on, not that its currently transparent. SetTransparency tells you that.
        public bool IsTransparentModeOn => (this.Parent is UserControlForm) ? ((UserControlForm)(this.Parent)).IsTransparentModeOn : false;

        // this gives you the transparent mode
        public UserControlForm.TransparencyMode TransparentMode => (this.Parent is UserControlForm) ? ((UserControlForm)(this.Parent)).TransparentMode : UserControlForm.TransparencyMode.Off;

        // this gives you the transparent key colour
        public Color TransparentKey => (this.Parent is UserControlForm) ? ((UserControlForm)(this.Parent)).TransparencyKey : Color.Transparent;

        public virtual string HelpKeyOrAddress() { return PanelID.ToString(); }     // default help key is panel id as a string - override to specialise

        #endregion

        #region Lifetime Contract

        // called when class is created. Override to get panel info if required

        public virtual void Creation(PanelInformation.PanelInfo p)
        {
            System.Diagnostics.Debug.WriteLine($"UCCB Create {this.Name}");
            PanelID = p.PopoutID;
        }

        // Called by form/major tab etc to create a panel

        public void Init(EDDiscoveryForm ed, int dn)
        {
            System.Diagnostics.Debug.WriteLine($"UCCB Init {this.Name} with DN {dn}");
            DiscoveryForm = ed;
            DisplayNumber = dn;
            Init();
        }

        // UCCB overrides this to initialise itself.
        // Init has a chance to make new controls if required to be autothemed/scaled.
        // contract is in majortabcontrol::CreateTab, PanelAndPopOuts::PopOut, SplitterControl::OnPostCreateTab, Grid:CreateInitPanel

        public virtual void Init() { }              // start up, called by above Init.  no cursor available

        // after init, themeing and scaling happens at this point.  Item should be in AutoScaleMode.Inherit to prevent double scaling

        // For popout forms, on init, it calls SetTransparency, then TransparencyModeChanged
        // The transparency key color is set by theme during UserControlForm init - you can override if required in Init()
        // everytime the transparency changes (due to user hovering etc) SetTransparency is called
        public virtual void SetTransparency(bool ison, Color curcol) { }

        // For popout forms, on init, TransparentModeChange is called. Then called only then if the user changes the transparent major mode on/off
        public virtual void TransparencyModeChanged(bool on) { }

        // then LoadLayout, InitialDisplay is called
        public virtual void LoadLayout() { }        // then a chance to load a layout. 
        public virtual void InitialDisplay() { }    // do the initial display

        // Panel is closing, save stuff. Note to users- DO NOT USE DIRECTLY - USE CLOSEDOWN()
        public virtual void Closing() { }

        // end calling order.

        #endregion

        #region Virtual overrides

        // override to say support transparency
        public virtual bool SupportTransparency { get { return false; } }
        // override to say default to be transparent
        public virtual bool DefaultTransparent { get { return false; } }
        // override to know
        public virtual void onControlTextVisibilityChanged(bool newvalue)
        {
        }
        // override to prevent closure
        public virtual bool AllowClose() { return true; }

        #endregion

        #region Closedown

        // call to request a close by the panel itself. Only for pop out windows
        public void RequestClose()
        {
            if (this.Parent is UserControlForm)
                ((UserControlForm)(this.Parent)).Close();
            else
                System.Diagnostics.Debug.WriteLine($"*** Can't request close this panel type {this.GetType()}");
        }

        // Call to indicate that the panel is closing, called by Form or Tab Control/Splitter to tell panel its closing down. Calls Closing() which the panel intercepts
        public void CloseDown()
        {
            IsClosed = true;
            Closing();
        }

        protected override void Dispose(bool disposing)     // ensure closed during disposal.
        {
            if (disposing)
            {
                if (!IsClosed)
                {
                    CloseDown();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Panel communication

        // Action request system - replaces UCTG. Allows comms between panels
        public static bool IsOperationForPrimaryTH(object actionobj) { return actionobj is RequestTravelToJID || actionobj is RequestTravelHistoryPos; }
        public static bool IsOperationHistoryPush(object actionobj) { return actionobj is EliteDangerousCore.HistoryEntry; }

        // HistoryEntry - sent by all TG on cursor moves.
        //          Splitter/grid distributes it around the siblings - they response HandledContinue or NotHandled
        //          Sent up to tab - MainTab distributes it to other tabs and forms
        //          All panels must return HandledContinue or NotHandled so no one grabs it

        //  RequestTravelToJID
        //           request travel grid to go to this jid. Success or Failure or NotHandled
        public class RequestTravelToJID 
        { 
            public long JID { get; set; }
            public bool MakeVisible { get; set; } = false;
        };

        // RequestTravelHistoryPos - request primary travel grid to call back directly to sender with the current HE (may be null)
        //           Splitter/grid distributes it around the siblings - if a TG there, they respond true, which stops the distribution (like the main tab will)
        //           If not ack, sent up to tab - Other will send it to maintab only
        //           Panel should return Success when it claims it
        public class RequestTravelHistoryPos { };       // use in Request to ask for your travel grid to send thru an he. TG will return true 

        // PushStars - someone is pushing a system list to expedition or trilat
        //           Splitter/grid distributes it around the siblings - if a recipient is there and uses it, they respond true, which stops the distribution
        //           Sent up to major tab - both types will distribute it to all tabs and the first recepient will cancel it
        //           Distributed to all forms
        //           Panel should return Success when it claims it

        public class PushStars                          // use to push star list to other panels 
        {
            public enum PushType { TriWanted, TriSystems, Expedition };
            public PushType PushTo { get; set; }
            public System.Collections.Generic.List<string> SystemNames { get; set; }
            public System.Collections.Generic.List<EliteDangerousCore.ISystem> SystemList { get; set; } // expedition can use either
            public bool MakeVisible { get; set; } = false;
            public string RouteTitle { get; set; } = null;
        };

        // PanelAction - perform this string action on a tab panel
        //           Sent into all tabs, and to all forms, first one accepting it will cancel it.
        //           Panel should return Success if serviced
        public class PanelAction                    // perform an action
        {
            public const string ImportCSV = "ImportCSV";                // data is the filename string, to expedition panel
            public const string EditNotePrimary = "editnoteprimary";    // no data
            public string Action { get; set; }
            public object Data { get; set; }
        }

        // PushResourceWantedList - synthesis/engineering pushes a list of wants to the resources panel
        //           Send to everyone
        //           Panel which grabs it should return Success
        //
        public class PushResourceWantedList         // use to push resource list to resource panel
        {
            public System.Collections.Generic.Dictionary<EliteDangerousCore.MaterialCommodityMicroResourceType, int> Resources { get; set; }      // push type and amount
        }

        // PushRouteList - route panel is pushing a list of systems calculated
        //           Send to everyone
        //           No Panel should grab it. return HandledContinue or NotHandled so it goes to everyone
        public class PushRouteList                  // use to push star list from route panel
        {
            public System.Collections.Generic.List<EliteDangerousCore.ISystem> Systems { get; set; }
        };

        // TravelHistoryStartStopChanged - someone set a start stop flag
        //           Sent to everone.
        //           Panel should return HandedContinue or NotHandled
        public class TravelHistoryStartStopChanged { }  // push start/stop has been changed

        // Compass Target
        //           compass should return Success when it grabs it
        public class SetCompassTarget
        {
            public string Name { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public enum PanelActionState { 
            NotHandled = 0,     // no one acked it
            HandledContinue = 1, // someone had handled it, but continue
            Success, // someone handled it okay
            Failed,  // someone handled it but it failed
            Cancelled // user has cancelled it
        };
        public static bool IsPASResult(PanelActionState s) => (int)s >= 2;

        // Request action. Return one of the PAS State
        // set up before Init by MajorTabControl, UserControlContainerGrid, UserControlSplitter, PopOuts.cs
        public Func<UserControlCommonBase, object, PanelActionState> RequestPanelOperation;        

        // Request panel operation with this request, if not handled, open this type of tab and try again
        public PanelActionState RequestPanelOperationOpen(PanelInformation.PanelIDs paneltype, object req)
        {
            if (RequestPanelOperation != null)      // not likely, but
            {
                var res = RequestPanelOperation.Invoke(this, req);
                if (res == PanelActionState.NotHandled)   // no-one serviced it, so create an expedition tab, and then reissue
                {
                    DiscoveryForm.SelectTabPage(paneltype, true, false);         // ensure panel is open
                    res = RequestPanelOperation.Invoke(this, req);     // try again
                }

                return res;
            }
            else
                return PanelActionState.NotHandled;
        }

        // panel is asked for operation, return true to indicate its swallowed, or false to say pass it onto next guy. 
        // the default implementation, because its used a lot, tries to go to a HE and if so calls the second entry point ReceiveHistoryEntry
        // either override PerformPanelOperation for the full monty, or override ReceiveHistoryEntry if your just interested in HE receive
        public virtual PanelActionState PerformPanelOperation(UserControlCommonBase sender, object actionobj)
        {
            if (actionobj is EliteDangerousCore.HistoryEntry)
            {
                ReceiveHistoryEntry((EliteDangerousCore.HistoryEntry)actionobj);
                return PanelActionState.HandledContinue;
            }
            else
                return PanelActionState.NotHandled;
        } 
        public virtual void ReceiveHistoryEntry(EliteDangerousCore.HistoryEntry he)
        {
        }

        #endregion

        #region Helpers

        // set the control text for the panel. 
        // for tabstrips, for forms, for resizable containers
        // for other (such as directly in the tab) looks for a labelControlText, and if present, uses that/
        public void SetControlText(string s)            
        {
            if (this.Parent is ExtendedControls.TabStrip)
                ((ExtendedControls.TabStrip)(this.Parent)).SetControlText(s);
            else if (this.Parent is UserControlForm)
                ((UserControlForm)(this.Parent)).SetControlText(s);
            else if (this.Parent is UserControlContainerResizable)
                ((UserControlContainerResizable)(this.Parent)).SetControlText(s);
            else
            {
                var t = this.GetType().GetField("labelControlText", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if ( t != null )
                {
                    dynamic label = t.GetValue(this);
                    label.Text = s;
                }
            }
        }
        
        public void SetClipboardText(string s)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(s))
                    Clipboard.SetText(s, TextDataFormat.Text);
            }
            catch
            {
                DiscoveryForm.LogLineHighlight("Copying text to clipboard failed".T(EDTx.UserControlCommonBase_Copyingtexttoclipboardfailed));
            }
        }
        public void SetClipboardImage(Image s)
        {
            try
            {
                Clipboard.SetImage(s);
            }
            catch
            {
                DiscoveryForm.LogLineHighlight("Copying text to clipboard failed".T(EDTx.UserControlCommonBase_Copyingtexttoclipboardfailed));
            }
        }
        public void SetClipboardImage(string file)
        {
            try
            {
                using (Image s = Image.FromFile(file))
                {
                    Clipboard.SetImage(s);
                }
            }
            catch
            {
                DiscoveryForm.LogLineHighlight("Copying text to clipboard failed".T(EDTx.UserControlCommonBase_Copyingtexttoclipboardfailed));
            }
        }
        public void SetClipboard(DataObject obj)
        {
            try
            {
                Clipboard.SetDataObject(obj);
            }
            catch
            {
                DiscoveryForm.LogLineHighlight("Copying object to clipboard failed");
            }
        }

        public bool ClipboardHasText()
        {
            try
            {
                return Clipboard.ContainsText();
            }
            catch
            {
                DiscoveryForm.LogLineHighlight("Unable to access clipboard");
                return false;
            }
        }

        public string GetClipboardText()
        {
            try
            {
                return Clipboard.GetText();
            }
            catch
            {
                return null;
            }
        }

        // find a UCCB of type T - this simple case just compares, overriden in splitter/grid to find sub parts
        public virtual UserControlCommonBase Find( PanelInformation.PanelIDs p)      
        {
            //System.Diagnostics.Debug.WriteLine($"UCCB Find of {t.Name} on {this.GetType().Name}");
            return PanelID == p ? this : null;
        }

        // force this tab or panel to be visible
        public void MakeVisible()       
        {
            Control c = Parent;
            TabPage p = null;
            while( c != null)
            {
                if (c is TabPage)           // tab page does not have an index, so we need to record its presence and then let tabcontrol use it
                {
                    p = (TabPage)c;
                }
                else if (c is TabControl)
                {
                    ((TabControl)c).SelectedTab = p;
                    Refresh();
                    break;
                }
                else if ( c is Form)        // if we reached form, its a floating window.. 
                {
                    ((Form)c).BringToFront();
                    break;
                }

                c = c.Parent;
            }
        }

        #endregion

        #region Resize

        // FUNCTIONS to allow a form to grow temporarily.  Does not work when inside the panels
        public bool ResizingNow = false;                                            

        public void RequestTemporaryMinimumSize(Size w)         // w is UC area
        { 
            if (this.Parent is UserControlForm)
            {
                ResizingNow = true;
                ((UserControlForm)(this.Parent)).RequestTemporaryMinimiumSize(w);
                ResizingNow = false;
            }
        }

        public void RequestTemporaryResizeExpand(Size w)        // by this area expand
        {
            if (this.Parent is UserControlForm)
            {
                ResizingNow = true;
                ((UserControlForm)(this.Parent)).RequestTemporaryResizeExpand(w);
                ResizingNow = false;
            }
        }

        public void RequestTemporaryResize(Size w)              // w is the UC area
        {
            if (this.Parent is UserControlForm)
            {
                ResizingNow = true;
                ((UserControlForm)(this.Parent)).RequestTemporaryResize(w);
                ResizingNow = false;
            }
        }

        public void RevertToNormalSize()                        // and to revert
        {
            if (this.Parent is UserControlForm)
            {
                ResizingNow = true;
                ((UserControlForm)(this.Parent)).RevertToNormalSize();
                ResizingNow = false;
            }
        }

        public bool IsInTemporaryResize                         // have we grown?
        { get
            {
                return (this.Parent is UserControlForm) ? ((UserControlForm)(this.Parent)).IsTemporaryResized : false;
            }
        }

        #endregion

        #region Data base helpers

        public string DBBaseName { get; set; } = null;          // constructor or init must set this to indicate DB Base name

        // this makes up the name. This is the backwards compatible naming. We may change this in future.

        static private string DBName(int dno, string basename, string itemname = "")
        { return EDDProfiles.Instance.UserControlsPrefix + basename + ((dno > 0) ? dno.ToString() : "") + itemname; }

        // get/put a setting - type needs to be bool, int, double, long, DateTime, string

        public T GetSetting<T>(string itemname, T defaultvalue)
        {
            System.Diagnostics.Debug.Assert(DBBaseName != null);
            //string name = global ? itemname : DBName(DisplayNumber, DBBaseName, itemname);
            string name = DBName(DisplayNumber, DBBaseName, itemname);
            var res = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(name, defaultvalue);

            //  System.Diagnostics.Debug.WriteLine("Get DB Name " + defaultvalue.GetType().Name + ": " + name + ": " + res);
            return res;
        }
        public T GetSettingGlobal<T>(string itemname, T defaultvalue)
        {
            System.Diagnostics.Debug.Assert(DBBaseName != null);
            var res = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(itemname, defaultvalue);
            return res;
        }

        public bool PutSetting<T>(string itemname, T value)
        {
            //string name = global ? itemname : DBName(DisplayNumber, DBBaseName, itemname);
            string name = DBName(DisplayNumber, DBBaseName, itemname);
            // System.Diagnostics.Debug.WriteLine("Set DB Name " + name + ": " + value);
            return EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(name, value);
        }

        public bool PutSettingGlobal<T>(string itemname, T value)
        {
            return EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(itemname, value);
        }

        public bool DeleteSetting(string itemname)
        {
            //string name = global ? itemname : DBName(DisplayNumber, DBBaseName, itemname);
            string name = DBName(DisplayNumber, DBBaseName, itemname);
            // System.Diagnostics.Debug.WriteLine("Set DB Name " + name + ": " + value);
            return EliteDangerousCore.DB.UserDatabase.Instance.DeleteKey(name) == 1;
        }

        public string GetBoolSettingsAsString(params string[] paras)      // make up a bool semicolon control string from items
        {
            string s = "";
            foreach (var p in paras)
            {
                if (GetSetting(p, false))
                    s = s.AppendPrePad(p, ";");
            }
            return s;
        }

        public bool[] GetSettingAsCtrlSet<T>(Func<T,bool> defaultvalue) where T:Enum
        {
            var ctrlset = new bool[Enum.GetNames(typeof(T)).Length];
            foreach (T e in Enum.GetValues(typeof(T)))
            {
                bool def = defaultvalue(e);
                var v = GetSetting(e.ToString(), def);
                //System.Diagnostics.Debug.WriteLine($"{DBBaseName} Get Ctrl Set {e.ToString()} = {v}");
                ctrlset[Convert.ToInt32(e)] = v;
            }

            return ctrlset;
        }

        public void PutBoolSettingsFromString(string res, params string[] paras)    // given a set of semicolon ; parameter names, update them
        {
            string[] set = res.Split(";");
            foreach (var p in paras)
            {
                bool v = Array.IndexOf(set, p) >= 0;
              //  System.Diagnostics.Debug.WriteLine($"{DBBaseName} Put Bool {p} with {v}");
                PutSetting(p, v);
            }
        }

        public string DGVSaveName(string auxname = "")
        {
            return DBName(DisplayNumber, DBBaseName + auxname, "DGVCol");
        }

        public bool DGVLoadColumnLayout(DataGridView dgv, string auxname = "", bool rowheaderselection = false)
        {
            string root = DBName(DisplayNumber, DBBaseName + auxname, "DGVCol");
            //System.Diagnostics.Debug.WriteLine($"DGV Layout Load {root} {auxname}");
            return dgv.LoadColumnSettings(root, rowheaderselection, 
                                        (a) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(a, int.MinValue),
                                        (b) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(b, double.MinValue));
        }

        public void DGVSaveColumnLayout(DataGridView dgv, string auxname = "")
        {
            string root = DBName(DisplayNumber, DBBaseName + auxname, "DGVCol");
            //System.Diagnostics.Debug.WriteLine($"DGV Layout Save {root} {auxname}");
            dgv.SaveColumnSettings(root, 
                                        (a,b) => EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(a, b),
                                        (c,d) => EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(c, d));
        }

        public void DGVTransparent(DataGridView dgv, bool on, Color curcol)
        {
            dgv.BackgroundColor = curcol;

            dgv.ColumnHeadersDefaultCellStyle.BackColor =
            dgv.RowHeadersDefaultCellStyle.BackColor = on ? curcol : ExtendedControls.Theme.Current.GridBorderBack;

            dgv.DefaultCellStyle.BackColor = on ? curcol : ExtendedControls.Theme.Current.GridCellBack;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = on ? curcol : ExtendedControls.Theme.Current.GridCellAltBack;

            dgv.DefaultCellStyle.ForeColor = on ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.GridCellText;
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = on ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.GridCellAltText;

            dgv.ColumnHeadersDefaultCellStyle.ForeColor =
            dgv.RowHeadersDefaultCellStyle.ForeColor = on ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.GridBorderText;

        }


         #endregion
    }
}
