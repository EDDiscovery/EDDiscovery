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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EliteDangerousCore.DB;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    public class UserControlCommonBase : UserControl
    {       
        public const int DisplayNumberHistoryGrid = 0;              // special codes, reserved for history grid
        protected const int DisplayNumberHistoryBotLeft = 1000;     // historical numbers.. don't want to change
        protected const int DisplayNumberHistoryBotRight = 1001;
        protected const int DisplayNumberHistoryMidRight = 1002;
        protected const int DisplayNumberHistoryTopRight = 1003;

        public const int DisplayNumberPrimaryTab = 0;               // tabs are 0, or 100+.  0 for the first, 100+ for repeats
        public const int DisplayNumberStartExtraTabs = 100;         // Excepting travelgrid which due to history grid using zero always starts at 100.
        public const int DisplayNumberStartExtraTabsMax = 199;         // Excepting travelgrid which due to history grid using zero always starts at 100.

        public const int DisplayNumberPopOuts = 1;                  // pop outs are 1-99.. of each specific type.

        // When a grid is open, displaynumber for its children is set by CreatePanel in UserControlContainerGrid
        // 1050 + grid displaynumber * 100 + index of type in grid
        // so, if a pop out grid of second instance, = 2, would get 1050+2000+index of type in grid
        // each grid gets 100 unique numbers, so 100 instances of each type in grid
        // this works for recursive grids to a enough for 158real world purposes.

        protected int DisplayNumberOfGridInstance(int numopenedinside)      // grid children are assigned this range..
        { return 1050 + displaynumber * 100 + numopenedinside; }

        // Common parameters of a UCCB

        public int displaynumber { get; protected set; }
        public EDDiscoveryForm discoveryform { get; protected set; }
        public IHistoryCursor uctg { get; protected set; }

        // in calling order..
        public void Init(EDDiscoveryForm ed, IHistoryCursor thc, int dn)
        {
            System.Diagnostics.Debug.WriteLine("Open UCCB " + this.Name + " of " + this.GetType().Name + " with " + dn);
            discoveryform = ed;
            displaynumber = dn;
            uctg = thc;
            Init();
        }    

        public virtual void Init() { }              // start up, called by above Init
        public virtual void LoadLayout() { }        // then a chance to load a layout
        public virtual void InitialDisplay() { }    // then after the themeing, do the initial display
        public virtual void Closing() { }           // close it

        public virtual void ChangeCursorType(IHistoryCursor thc) { }     // optional, cursor has changed

        public virtual Color ColorTransparency { get { return Color.Transparent; } }        // override to say support transparency, and what colour you want.
        public virtual void SetTransparency(bool ison, Color curcol) { }                      // set on/off transparency of components.

        public bool IsFloatingWindow { get { return this.FindForm() is Forms.UserControlForm; } }   // ultimately its a floating window

        public void SetControlText(string s)            // used to set heading text in either the form of the tabstrip
        {
            if (this.Parent is ExtendedControls.TabStrip)
                ((ExtendedControls.TabStrip)(this.Parent)).SetControlText(s);
            else if (this.Parent is Forms.UserControlForm)
                ((Forms.UserControlForm)(this.Parent)).SetControlText(s);
            else if (this.Parent is UserControlContainerResizable)
                ((UserControlContainerResizable)(this.Parent)).SetControlText(s);
        }

        public bool IsTransparent
        {
            get
            {
                if (this.Parent is Forms.UserControlForm)
                    return ((Forms.UserControlForm)(this.Parent)).IsTransparent;
                else
                    return false;
            }
        }

        #region Resize

        public bool ResizingNow = false;                                            // FUNCTIONS to allow a form to grow temporarily.  Does not work when inside the panels

        public void RequestTemporaryMinimumSize(Size w)         // w is UC area
        { 
            if (this.Parent is Forms.UserControlForm)
            {
                ResizingNow = true;
                ((Forms.UserControlForm)(this.Parent)).RequestTemporaryMinimiumSize(w);
                ResizingNow = false;
            }
        }

        public void RequestTemporaryResizeExpand(Size w)        // by this area expand
        {
            if (this.Parent is Forms.UserControlForm)
            {
                ResizingNow = true;
                ((Forms.UserControlForm)(this.Parent)).RequestTemporaryResizeExpand(w);
                ResizingNow = false;
            }
        }

        public void RequestTemporaryResize(Size w)              // w is the UC area
        {
            if (this.Parent is Forms.UserControlForm)
            {
                ResizingNow = true;
                ((Forms.UserControlForm)(this.Parent)).RequestTemporaryResize(w);
                ResizingNow = false;
            }
        }

        public void RevertToNormalSize()                        // and to revert
        {
            if (this.Parent is Forms.UserControlForm)
            {
                ResizingNow = true;
                ((Forms.UserControlForm)(this.Parent)).RevertToNormalSize();
                ResizingNow = false;
            }
        }

        public bool IsInTemporaryResize                         // have we grown?
        { get
            {
                return (this.Parent is Forms.UserControlForm) ? ((Forms.UserControlForm)(this.Parent)).IsTemporaryResized : false;
            }
        }

        #endregion

        #region DGV Column helpers - used to save/size the DGV of user controls dynamically.

        public void DGVLoadColumnLayout(DataGridView dgv, string root)
        {
            if (SQLiteConnectionUser.keyExists(root + "1"))        // if stored values, set back to what they were..
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    string k = root + (i + 1).ToString();
                    int w = SQLiteDBClass.GetSettingInt(k, -1);
                    if (w >= 10)        // in case something is up (min 10 pixels)
                        dgv.Columns[i].Width = w;
                    //System.Diagnostics.Debug.WriteLine("Load {0} {1} {2} {3}", Name, k, w, dgv.Columns[i].Width);
                }
            }
        }

        public void DGVSaveColumnLayout(DataGridView dgv, string root)
        {
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                string k = root + (i + 1).ToString();
                SQLiteDBClass.PutSettingInt(k, dgv.Columns[i].Width);
                //System.Diagnostics.Debug.WriteLine("Save {0} {1} {2}", Name, k, dgv.Columns[i].Width);
            }
        }

        #endregion
    }
}
