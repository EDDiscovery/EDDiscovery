/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public class UserControlCommonBase : UserControl
    {
        public const int DisplayNumberPrimaryTab = 0;               // tabs are 0, or 100+.  0 for the first, 100+ for repeats
        public const int DisplayNumberPopOuts = 1;                  // pop outs are 1-99.. of each specific type.
        public const int DisplayNumberStartExtraTabs = 100;         // extra tabs are assigned here
        public const int DisplayNumberStartExtraTabsMax = 199;

        // When a grid or splitter is open, displaynumber for its children based on its own number
        // 1050 is historical.. 1000..1049 was reserved for the previous history window splitters

        protected int DisplayNumberOfSplitter(int numopenedinside)  // splitter children are assigned this range..
        { return 1050 + displaynumber * 100 + numopenedinside; }

        protected int DisplayNumberOfGrid(int numopenedinside)      // grid children are assigned this range..  allow range for splitters.
        { return 1050 + (DisplayNumberStartExtraTabsMax + 1) * 100 + displaynumber * 100 + numopenedinside; }

        // Common parameters of a UCCB

        public PanelInformation.PanelIDs panelid { get; private set; }  // set on creation 
        public int displaynumber { get; private set; }                // set on Init
        public EDDiscoveryForm discoveryform { get; private set; }    // set on Init    
        public IHistoryCursor uctg { get; protected set; }            // valid at loadlayout
        public bool IsClosed { get; private set; }                    // set after CloseDown called. Use this if your doing await stuff which may mean your class gets called after close

        /////////////////////////////////////////////////////////////// in calling order..
        
        // called when class is created. Override to get panel info if required

        public virtual void Creation(PanelInformation.PanelInfo p)      
        {
            System.Diagnostics.Debug.WriteLine("Create UCCB " + this.Name + " of " + this.GetType().Name + " with " + p.PopoutID);
            panelid = p.PopoutID;
        }

        // called after set up on init

        public void Init(EDDiscoveryForm ed, int dn)
        {
            System.Diagnostics.Debug.WriteLine("Open UCCB " + this.Name + " of " + this.GetType().Name + " with " + dn);
            discoveryform = ed;
            displaynumber = dn;
            Init();
        }

        public virtual void Init() { }              // start up, called by above Init.  no cursor available

        // For forms, the transparency key color is set by theme during UserControlForm init. The Init function above for a UC could override if required
        // themeing and scaling happens at this point.  Init has a chance to make new controls if required to be autothemed/scaled.
        // contract is in majortabcontrol::CreateTab, PanelAndPopOuts::PopOut, SplitterControl::OnPostCreateTab

        public virtual void SetTransparency(bool ison, Color curcol) { }  // set on/off transparency of components - occurs before SetCursor/LoadLayout/InitialDisplay in a pop out form
        public virtual void SetCursor(IHistoryCursor cur) { uctg = cur; }       // cursor is set..  Most UCs don't need to implement this.
        public virtual void LoadLayout() { }        // then a chance to load a layout. cursor available
        public virtual void InitialDisplay() { }    // do the initial display
        public virtual void Closing() { }           // Panel is closing, save stuff. Note to users- DO NOT USE DIRECTLY - USE CLOSEDOWN()

        // end calling order.

        public virtual void ChangeCursorType(IHistoryCursor thc) { }     // implement if you call the uctg

        public virtual bool SupportTransparency { get { return false; } }  // override to say support transparency
        public virtual bool DefaultTransparent { get { return false; } }  // override to say default to be transparent

        public virtual bool AllowClose() { return true; }   

        // Close

        public void CloseDown()     // Call to close down.
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


        public bool IsFloatingWindow { get { return this.FindForm() is UserControlForm; } }   // ultimately its a floating window

        public virtual string HelpKeyOrAddress() { return panelid.ToString(); }     // default help key is panel id as a string

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

        public bool IsControlTextVisible()
        {
            if (this.Parent is UserControlForm)
                return ((UserControlForm)(this.Parent)).IsControlTextVisible();
            else
                return true;    // else presume true
        }

        public bool HasControlTextArea()
        {
            return (this.Parent is ExtendedControls.TabStrip) || (this.Parent is UserControlForm) || (this.Parent is UserControlContainerResizable);
        }

        public virtual void onControlTextVisibilityChanged(bool newvalue)       // override to know
        {
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
                discoveryform.LogLineHighlight("Copying text to clipboard failed".T(EDTx.UserControlCommonBase_Copyingtexttoclipboardfailed));
            }
        }

        public bool IsTransparentModeOn           // this means the transparent mode is on, not that its currently transparent.
        {
            get
            {
                if (this.Parent is UserControlForm)
                    return ((UserControlForm)(this.Parent)).IsTransparentModeOn;
                else
                    return false;
            }
        }

        public void UpdateTransparency()
        {
            if (this.Parent is UserControlForm)
                ((UserControlForm)(this.Parent)).UpdateTransparency();
        }

        public virtual UserControlCommonBase Find( PanelInformation.PanelIDs p)      // find a UCCB of type T - this simple case just compares, overriden in splitter/grid
        {
            //System.Diagnostics.Debug.WriteLine($"UCCB Find of {t.Name} on {this.GetType().Name}");
            return panelid == p ? this : null;
        }

        #region Resize

        public bool ResizingNow = false;                                            // FUNCTIONS to allow a form to grow temporarily.  Does not work when inside the panels

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
            string name = DBName(displaynumber, DBBaseName, itemname);
            var res = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(name, defaultvalue);

          //  System.Diagnostics.Debug.WriteLine("Get DB Name " + defaultvalue.GetType().Name + ": " + name + ": " + res);
            return res;
        }

        public bool PutSetting<T>(string itemname, T value)
        {
            string name = DBName(displaynumber, DBBaseName, itemname);
           // System.Diagnostics.Debug.WriteLine("Set DB Name " + name + ": " + value);
            return EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(name, value);
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
            return DBName(displaynumber, DBBaseName + auxname, "DGVCol");
        }

        public bool DGVLoadColumnLayout(DataGridView dgv, string auxname = "")
        {
            string root = DBName(displaynumber, DBBaseName + auxname, "DGVCol");
            //System.Diagnostics.Debug.WriteLine("Get Column Name " + root);
            return dgv.LoadColumnSettings(root, (a) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(a, int.MinValue),
                                        (b) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(b, double.MinValue));
        }

        public void DGVSaveColumnLayout(DataGridView dgv, string auxname = "")
        {
            string root = DBName(displaynumber, DBBaseName + auxname, "DGVCol");
            //System.Diagnostics.Debug.WriteLine("Set Column Name " + root);
            dgv.SaveColumnSettings(root, (a,b) => EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(a, b),
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

        public class DBSettingsSaver             // instance this class and you can pass the class to another class, allowing it to use your UCCB generic get/save
        {                                        // with a defined extra itemname.  this seems the only way to pass generic delegates
            public DBSettingsSaver(UserControlCommonBase b, string itemname)
            {
                root = itemname;
                ba = b;
            }
            public T GetSetting<T>(string key, T defaultvalue)
            {
                return ba.GetSetting(root+key, defaultvalue);
            }

            public bool PutSetting<T>(string key, T value)
            {
                return ba.PutSetting(root+key, value);
            }

            private string root;
            private UserControlCommonBase ba;
        }

        #endregion
    }
}
