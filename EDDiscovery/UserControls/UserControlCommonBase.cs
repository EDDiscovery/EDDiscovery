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
        public const int DisplayNumberPrimaryTab = 0;               // tabs are 0, or 100+.  0 for the first, 100+ for repeats
        public const int DisplayNumberPopOuts = 1;                  // pop outs are 1-99.. of each specific type.
        public const int DisplayNumberStartExtraTabs = 100;         // extra tabs are assigned here
        public const int DisplayNumberStartExtraTabsMax = 199;

        // When a grid or splitter is open, displaynumber for its children based on its own number
        // 1050 is historical.. 1000..1049 was reserved for the previous history window splitters

        protected int DisplayNumberOfSplitter(int numopenedinside)  // splitter children are assigned this range..
        { return 1050 + displaynumber * 100 + numopenedinside; }

        protected int DisplayNumberOfGrid(int numopenedinside)      // grid children are assigned this range..  allow range for splitters.
        { return 1050 + (DisplayNumberStartExtraTabsMax+1) * 100 + displaynumber * 100 + numopenedinside; }

        static public string DBName(int dno, string basename, string itemname = "")
        { return EDDProfiles.Instance.UserControlsPrefix + basename + ((dno> 0) ? dno.ToString() : "") + itemname; }

        protected string DBName(string basename, string itemname = "")
        { return DBName(displaynumber, basename, itemname);}

        // Common parameters of a UCCB

        public int displaynumber { get; protected set; }
        public EDDiscoveryForm discoveryform { get; protected set; }
        public IHistoryCursor uctg { get; protected set; }          // valid at loadlayout
        private bool IsClosed { get; set; }

        // in calling order..
        public void Init(EDDiscoveryForm ed, int dn)
        {
            //System.Diagnostics.Debug.WriteLine("Open UCCB " + this.Name + " of " + this.GetType().Name + " with " + dn);
            discoveryform = ed;
            displaynumber = dn;
            Init();
        }

        public virtual void Init() { }              // start up, called by above Init.  no cursor available
        // themeing and scaling happens at this point.  Init has a chance to make new controls if required to be autothemed/scaled.
        // contract is in majortabcontrol::CreateTab, PanelAndPopOuts::PopOut, SplitterControl::OnPostCreateTab
        public virtual void SetTransparency(bool ison, Color curcol) { }  // set on/off transparency of components - occurs before SetCursor/LoadLayout/InitialDisplay in a pop out form
        public virtual void SetCursor(IHistoryCursor cur) { uctg = cur; }       // cursor is set..  Most UCs don't need to implement this.
        public virtual void LoadLayout() { }        // then a chance to load a layout. cursor available
        public virtual void InitialDisplay() { }    // do the initial display
        public virtual void Closing() { }           // DO NOT USE DIRECTLY - USE CLOSEDOWN()

        // end calling order.

        public virtual void ChangeCursorType(IHistoryCursor thc) { }     // implement if you call the uctg

        public virtual Color ColorTransparency { get { return Color.Transparent; } }  // override to say support transparency, and what colour you want.

        // Close

        public void CloseDown()     // Call to close down.
        {
            Closing();
            IsClosed = true;
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

        public bool IsControlTextVisible()
        {
            if (this.Parent is Forms.UserControlForm)
                return ((Forms.UserControlForm)(this.Parent)).IsControlTextVisible();
            else
                return true;    // else presume true
        }

        public bool HasControlTextArea()
        {
            return (this.Parent is ExtendedControls.TabStrip) || (this.Parent is Forms.UserControlForm) || (this.Parent is UserControlContainerResizable);
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

        // need to cope with older saves using int column widths instead of fill percentages

        private double[] DGVColumnSizes(DataGridView dgv, string root, out int hw, out bool useint)
        {
            hw = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(root + "HW", -1);     // SET Row header width first
            useint = false;

            if (hw == -1)
                return null;

            hw = Math.Max(10, hw);

            // first see if we have a full collection of ints (old style) or floats (new style)
            // we may have a mixture or not a full set..

            int intpresent = 0;
            int inttotal = 0;
            int[] widths = new int[dgv.Columns.Count];      // older saves used ints to save column sizes
            double[] fillw = new double[dgv.Columns.Count]; // newer ones save the fill weight
            int doublepresent = 0;

            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                string k = root + (i + 1).ToString();
                widths[i] = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(k, -1);
                intpresent += widths[i] >= 10 ? 1 : 0;
                inttotal += widths[i];

                fillw[i] = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(k, double.NaN);
                if (!double.IsNaN(fillw[i]) && fillw[i] > 0)
                    doublepresent++;
            }

            if (doublepresent == dgv.ColumnCount)   // a full set of doubles
            {
                return fillw;
            }
            else if (intpresent == dgv.ColumnCount) // full set of ints 
            {
                for (int i = 0; i < dgv.ColumnCount; i++)
                    fillw[i] = (float)widths[i] / (float)inttotal;
                useint = true;
                return fillw;
            }
            else
                return null;    // no valid settings
        }

        public void DGVLoadColumnLayout(DataGridView dgv, string root)
        {
            dgv.SuspendLayout();

            var fillw = DGVColumnSizes(dgv, root, out int hw, out bool useint);

            if (fillw != null)
            {
                dgv.RowHeadersWidth = hw;

                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    dgv.Columns[i].FillWeight = (float)fillw[i];
              //      System.Diagnostics.Debug.WriteLine("Load {0} {1} {2} {3}", i, Name, root, dgv.Columns[i].FillWeight);
                }
            }

            dgv.ResumeLayout();
        }

        public void DGVSaveColumnLayout(DataGridView dgv, string root)
        {
            var fillw = DGVColumnSizes(dgv, root, out int hw, out bool useint);

            bool storeback = false;

            if (fillw == null || hw != dgv.RowHeadersWidth || useint)
                storeback = true;
            else
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    if ( Math.Abs(fillw[i] - dgv.Columns[i].FillWeight) > 0.00001)
                    {
                        storeback = true;
                        break;
                    }
                }
            }

            if (storeback)  // using this to be able to see when a store back occurs for debugging.. we could of course later just always store back
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    string k = root + (i + 1).ToString();
                    EliteDangerousCore.DB.UserDatabase.Instance.DeleteKey(k);
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(k, dgv.Columns[i].FillWeight);
                    if ( fillw != null )  System.Diagnostics.Debug.WriteLine("Save {0} {1} {2} {3} vs {4}", Name, k, dgv.Columns[i].Width, dgv.Columns[i].FillWeight, fillw[i]);
                }

                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(root + "HW", dgv.RowHeadersWidth);
                System.Diagnostics.Debug.WriteLine("Save {0} HW {1}", Name, dgv.RowHeadersWidth);
            }
        }


        #endregion
    }
}
