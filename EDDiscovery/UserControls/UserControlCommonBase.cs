﻿/*
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
        public virtual void Init( EDDiscoveryForm ed, int displayno) { }
        public virtual void Display(HistoryEntry current, HistoryList history) { }

        public virtual void LoadLayout() { }
        public virtual void Closing() { }

        public virtual Color ColorTransparency { get { return Color.Transparent; } }        // override to say support transparency, and what colour you want.
        public virtual void SetTransparency(bool on, Color curcol) { }                      // set on/off transparency of components.

        public void SetControlText(string s)            // used to set heading text in either the form of the tabstrip
        {
            if (this.Parent is ExtendedControls.TabStrip)
                ((ExtendedControls.TabStrip)(this.Parent)).SetControlText(s);
            else if (this.Parent is Forms.UserControlForm)
                ((Forms.UserControlForm)(this.Parent)).SetControlText(s);
        }

        public bool IsTransparent
        {
            get
            {
                if (this.Parent is Forms.UserControlForm)
                    return ((Forms.UserControlForm)(this.Parent)).istransparent;
                else
                    return false;
            }
        }



        #region Resize

        public bool inresizeduetoexpand = false;                                            // FUNCTIONS to allow a form to grow temporarily.  Does not work when inside the panels

        public void RequestTemporaryMinimumSize(Size w)         // w is client area
        {
            if (this.Parent is Forms.UserControlForm)
            {
                inresizeduetoexpand = true;
                ((Forms.UserControlForm)(this.Parent)).RequestTemporaryMinimiumSize(w);
                inresizeduetoexpand = false;
            }
        }

        public void RequestTemporaryResizeExpand(Size w)        // by this client size
        {
            if (this.Parent is Forms.UserControlForm)
            {
                inresizeduetoexpand = true;
                ((Forms.UserControlForm)(this.Parent)).RequestTemporaryResizeExpand(w);
                inresizeduetoexpand = false;
            }
        }

        public void RequestTemporaryResize(Size w)              // w is client area
        {
            if (this.Parent is Forms.UserControlForm)
            {
                inresizeduetoexpand = true;
                ((Forms.UserControlForm)(this.Parent)).RequestTemporaryResize(w);
                inresizeduetoexpand = false;
            }
        }

        public void RevertToNormalSize()                        // and to revert
        {
            if (this.Parent is Forms.UserControlForm)
            {
                inresizeduetoexpand = true;
                ((Forms.UserControlForm)(this.Parent)).RevertToNormalSize();
                inresizeduetoexpand = false;
            }
        }

        public bool IsInTemporaryResize                         // have we grown?
        { get
            {
                return (this.Parent is Forms.UserControlForm) ? ((Forms.UserControlForm)(this.Parent)).istemporaryresized : false;
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
