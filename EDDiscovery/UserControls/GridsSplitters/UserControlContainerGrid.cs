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
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    // tbd paneladded/removed
    public partial class UserControlContainerGrid : UserControlCommonBase        // circular, huh! neat!
    {
        public UserControlTravelGrid GetTravelGrid { get { return GetUserControl<UserControlTravelGrid>(PanelInformation.PanelIDs.TravelGrid); } }

        private T GetUserControl<T>(PanelInformation.PanelIDs p) where T : class
        {
            var uccrfound = uccrlist.Find(x => x.UCCB.PanelID == p);
            return uccrfound != null ? uccrfound.UCCB as T : null;
        }
        public UserControlCommonBase GetUserControl(PanelInformation.PanelIDs p)
        {
            var uccrfound = uccrlist.Find(x => x.UCCB.PanelID == p);
            return uccrfound != null ? uccrfound.UCCB: null;
        }


        public UserControlContainerGrid()
        {
            InitializeComponent();
        }

        #region UCCB interface

        public override void Init()
        {
            DBBaseName = "GridControl";

            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip,this);
            rollUpPanelMenu.SetToolTip(toolTip);    // use the defaults

            //System.Diagnostics.Debug.WriteLine("Grid Restore from " + DBWindowNames);

            var windows = GetSavedSettings();

            if (windows != null )
            {
                foreach (var n in windows)
                {
                    UserControlCommonBase uccb = PanelInformation.Create(n.Item1);
                    if ( uccb != null)
                        CreateInitPanel(uccb);
                }
            }

            rollUpPanelMenu.PinState = GetSetting(dbRolledUp, true);

            ResumeLayout();
            UpdateButtons();

            // contract states the PanelAndPopOuts OR the MajorTabControl will now theme and size it.
        }

        public override bool SupportTransparency { get { return true; } }

        public override void SetTransparency(bool on, Color curcol)
        {
            this.BackColor = curcol;
            panelPlayfield.BackColor = curcol;
            rollUpPanelMenu.BackColor = curcol;
            rollUpPanelMenu.ShowHiddenMarker = !on;

            foreach (UserControlContainerResizable r in uccrlist)
            {
                r.BorderColor = on ? Color.Transparent : ExtendedControls.Theme.Current.GridBorderLines;
                UserControlCommonBase uc = r.UCCB;
                uc.SetTransparency(on, curcol);
            }
        }

        public override void LoadLayout()   // init and themeing done, now we can complete the UCCB process, set positioning etc.
        {
            var windows = GetSavedSettings();

            if (windows != null)
            {
                int i = 0;

                foreach (UserControlContainerResizable u in uccrlist)
                {
                    UserControlCommonBase uc = u.UCCB;
                    LoadLayoutPanel(u, uc, windows[i].Item2, windows[i].Item3);
                    i++;
                }

                for (i = 0; i < windows.Count; i++)
                {
                    int item = windows[i].Item4;        // item index

                    if (item >= 0 && item < uccrlist.Count)
                    {
                        UserControlContainerResizable uccr = uccrlist[item];
                        //System.Diagnostics.Debug.WriteLine("Set " + uccr.control.GetType().Name + " to " + i);
                        panelPlayfield.Controls.SetChildIndex(uccr, i);
                    }
                }

                Update();        // need this to FORCE a full refresh in case there are lots of windows
            }
        }
        
        public override void InitialDisplay()
        {
            foreach (UserControlContainerResizable r in uccrlist)
            {
                UserControlCommonBase uc = r.UCCB;
                uc.InitialDisplay();
            }
        }

        public override bool AllowClose()       // grid is closing, does the consistuent panels allow close?
        {
            foreach (UserControlContainerResizable r in uccrlist)   // save in uccr list
            {
                UserControlCommonBase uc = r.UCCB;
                if (uc.AllowClose() == false)
                    return false;
            }
            return true;
        }

        public override void Closing()
        {
            //System.Diagnostics.Debug.WriteLine("Grid Saving to " + DbWindows);
            string s = "", p = "";
            foreach (UserControlContainerResizable r in uccrlist)   // save in uccr list
            {
                UserControlCommonBase uc = r.UCCB;

                s = s.AppendPrePad(((int)uc.PanelID).ToStringInvariant(), ",");
                p = p.AppendPrePad(r.Location.X + "," + r.Location.Y + "," + r.Size.Width + "," + r.Size.Height, ",");
                
                //System.Diagnostics.Debug.WriteLine("  Save " + uc.GetType().Name + " at " + r.Location + " sz " + r.Size);

                uc.CloseDown();
            }

            PutSetting(dbWindowNames, s);
            PutSetting(dbPositionSize, p);
            PutSetting(dbRolledUp, rollUpPanelMenu.PinState);

            string z = "";

            foreach (Control c in panelPlayfield.Controls)
            {
                if (c is UserControlContainerResizable)
                {
                    int index = uccrlist.FindIndex(x => x.Equals(c));
                    z = z.AppendPrePad(index.ToStringInvariant(), ",");
                    //System.Diagnostics.Debug.WriteLine("Order.." + index + "=" + (c as UserControlContainerResizable).control.GetType().Name );
                }
            }

            PutSetting(dbZOrder, z);
            //System.Diagnostics.Debug.WriteLine("---- END Grid Saving to " + DbWindows);
        }

        public override UserControlCommonBase Find(PanelInformation.PanelIDs p)              // find UCCB of this type in
        {
            foreach( var x in uccrlist)
            {
                if ( x.UCCB != null )
                {
                    var f = x.UCCB.Find(p);     // need to use find on it, since it may be embedded the same
                    if (f != null)
                        return f;
                }
            }
            return null;
        }
        
        #endregion

        #region Panel control

        private UserControlContainerResizable CreateInitPanel(UserControlCommonBase uccb)
        {
            uccb.AutoScaleMode = AutoScaleMode.Inherit;     // as per major tab control, we set mode to inherit to prevent multi scaling
            uccb.RequestPanelOperation += GridRequestAction;

            UserControlContainerResizable uccr = new UserControlContainerResizable();

            PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID(uccb.PanelID);
            uccr.Init(uccb, pi.WindowTitle);
            uccr.ResizeStart += ResizeStart;
            uccr.ResizeEnd += ResizeEnd;
            uccr.BorderColor = ExtendedControls.Theme.Current.GridBorderLines;
            uccr.SelectedBorderColor = ExtendedControls.Theme.Current.TextBlockHighlightColor;

            int numopenedinsidealready = uccrlist.Count(x => x.UCCB.GetType().Equals(uccb.GetType()));    // how many others are there BEFORE add

            uccrlist.Add(uccr);

            int dnum = DisplayNumberOfGrid(numopenedinsidealready);      // will be at least 1 (si

            panelPlayfield.Controls.Add(uccr);

            System.Diagnostics.Trace.WriteLine("Grid Make " + uccb.GetType().Name + " " + dnum + " " + numopenedinsidealready);

            uccb.Init(DiscoveryForm, dnum);

            return uccr;
        }

        private void LoadLayoutPanel(UserControlContainerResizable uccr, UserControlCommonBase uccb, Point pos, Size size)
        {
            //System.Diagnostics.Trace.WriteLine("GD:Cursor/Load/Init " + uccb.GetType().Name + " to " + pos + " " + size);
            uccb.LoadLayout();
            uccb.InitialDisplay();

            uccr.Location = pos;        // must set on load layout, themeing is defined between init and load layout and can shift numbers
            uccr.Size = size;
        }

        public bool ClosePanel(UserControlContainerResizable uccr)
        {
            UserControlCommonBase uc = uccr.UCCB;

            if (uc.AllowClose())
            {
                System.Diagnostics.Trace.WriteLine($"Grid Close {uc.PanelID} dno {DisplayNumber}");
                uc.CloseDown();
                panelPlayfield.Controls.Remove(uccr);
                uccrlist.Remove(uccr);
                Invalidate();
                uc.Dispose();
                uccr.Dispose();
                UpdateButtons();
                return true;
            }
            else
                return false;
        }

        // called by the panels to do something - pass to siblings, and then work out if we should pass upwards
        // a panel may claim the event, in which case its not sent up
        //SYNC with splitter

        private PanelActionState GridRequestAction(UserControlCommonBase sender, object actionobj)
        {
            //System.Diagnostics.Debug.WriteLine($"Grid {DisplayNumber} request action {actionobj}");

            PanelActionState retstate = PanelActionState.NotHandled;

            foreach (var uccr in uccrlist)
            {
                if (uccr.UCCB != sender)        // don't send to sender
                {
                    //System.Diagnostics.Debug.WriteLine($"...grid {uccr.UCCB.PanelID} perform operation {actionobj}");
                    var state = uccr.UCCB.PerformPanelOperation(sender, actionobj);
                    //System.Diagnostics.Debug.WriteLine($"...grid {uccr.UCCB.PanelID} perform operation {actionobj} result {state}");

                    if (IsPASResult(state))
                    {
                        retstate = state;
                        //System.Diagnostics.Debug.WriteLine($"...grid {uccr.UCCB.PanelID} claimed this operation {actionobj} result {state}");
                        break;
                    }
                }
            }

            if (!IsPASResult(retstate))
            {
                //System.Diagnostics.Debug.WriteLine($".. grid no claim on {actionobj}, pass on up the chain");
                return RequestPanelOperation.Invoke(sender, actionobj);     // and pass up with us as the sender
            }
            else
                return retstate;
        }

        // called from above for us to do something, work out if we should pass it down
        // we don't pass up some travel grid stuff if we have a travel grid ourselves
        // sender can't be us since we are being called from above.
        //SYNC with splitter
        public override PanelActionState PerformPanelOperation(UserControlCommonBase sender, object actionobj)
        {
            //System.Diagnostics.Debug.WriteLine($"Grid {DisplayNumber} perform action {actionobj}");

            if (IsOperationHistoryPush(actionobj) && GetUserControl(PanelInformation.PanelIDs.TravelGrid) != null)
            {
                //System.Diagnostics.Debug.WriteLine($".. blocked because we have a TH in the grid for {actionobj}");
                return PanelActionState.NotHandled;
            }

            bool handled = false;

            foreach (var uccr in uccrlist)
            {
                //System.Diagnostics.Debug.WriteLine($"...grid {uccr.UCCB.PanelID} perform action from above {actionobj}");
                var state = uccr.UCCB.PerformPanelOperation(sender, actionobj);     // pass to it
                //System.Diagnostics.Debug.WriteLine($"...grid {uccr.UCCB.PanelID} perform action from above {actionobj} result {state}");

                if ( state != PanelActionState.NotHandled)       // if we said something to it other than NotHandled
                {
                    handled = true;
                    if (IsPASResult(state))
                    {
                        //System.Diagnostics.Debug.WriteLine($"...grid {uccr.UCCB.PanelID} claimed this operation {actionobj} result {state}");
                        return state;
                    }
                }
            }

            return handled ? PanelActionState.HandledContinue : PanelActionState.NotHandled;
        }

        #endregion

        #region Open/Close

        void UpdateButtons()
        {
            buttonExtDelete.Enabled = (from x in uccrlist where x.Selected select x).Count() > 0;
            buttonExtTile.Enabled = uccrlist.Count > 0;
        }

        private void Select(UserControlContainerResizable uccr)
        {
            foreach (UserControlContainerResizable r in uccrlist)
            {
                if (Object.ReferenceEquals(r, uccr))
                {
                    if (!r.Selected)
                    {
                        r.Selected = true;
                        r.BringToFront();
                    }
                }
                else if (r.Selected)
                {
                    r.Selected = false;
                }
            }

            UpdateButtons();
        }

        #endregion

        #region Panel reactions

        bool wasselected = false;
        private void ResizeStart(UserControlContainerResizable uccr)
        {
            wasselected = uccr.Selected;
            Select(uccr);
        }

        private void ResizeEnd(UserControlContainerResizable uccr, bool dragged)
        {
            if (!dragged && wasselected)
                Select(null);

            uccr.Refresh();
        }

        #endregion

        #region Clicks

        private void buttonExtPopOut_Click(object sender, EventArgs e)
        {
            popoutdropdown = new ExtendedControls.ExtListBoxForm("", true);

            var list = PanelInformation.GetUserSelectablePanelInfo(EDDConfig.Instance.SortPanelsByName, true);
            popoutdropdown.ImageItems = list.Select(x => x.TabIcon).ToList();
            popoutdropdown.Items = list.Select(x => x.Description).ToList();
            popoutdropdown.ItemSeperators = PanelInformation.GetUserSelectableSeperatorIndex(EDDConfig.Instance.SortPanelsByName, true);
            popoutdropdown.FlatStyle = FlatStyle.Popup;
            popoutdropdown.PositionBelow(buttonExtPopOut);
            popoutdropdown.FitImagesToItemHeight = true;

            PanelInformation.PanelIDs[] pids = list.Select(x => x.PopoutID).ToArray();

            popoutdropdown.SelectedIndexChanged += (s, ea, key) =>
            {
                UserControlCommonBase uccb = PanelInformation.Create(pids[popoutdropdown.SelectedIndex]);
                UserControlContainerResizable uccr = CreateInitPanel(uccb);

                // uccb init done above, contract states we now scale then theme.
                
                var scale = this.FindForm().CurrentAutoScaleFactor();
                //System.Diagnostics.Trace.WriteLine($"Grid apply scaling to {uccr.Name} {scale}");
                uccr.Scale(scale);       // scale and

                ExtendedControls.Theme.Current.ApplyStd(uccr);

                LoadLayoutPanel(uccr, uccr.UCCB,
                                            new Point((uccrlist.Count % 5) * 50, (uccrlist.Count % 5) * 50),
                                            new Size(Math.Min(300, panelPlayfield.Width - 10), Math.Min(300, panelPlayfield.Height - 10)));


                Select(null);
                uccr.Selected = true;
                uccr.BringToFront();
                UpdateButtons();
            };

            ExtendedControls.Theme.Current.ApplyStd(popoutdropdown,true);
            popoutdropdown.SelectionBackColor = ExtendedControls.Theme.Current.ButtonBackColor;
            popoutdropdown.Show(this);
        }

        private void buttonExtDelete_Click(object sender, EventArgs e)
        {
            UserControlContainerResizable sel = uccrlist.Find(x => x.Selected);

            if ( sel != null )
                ClosePanel(sel);
        }

        private void buttonExtTile_Click(object sender, EventArgs e)
        {
            int w = panelPlayfield.Width;
            int h = panelPlayfield.Height;
            int bands = (h + 299) / 300;

            int cols;
            int rows = bands;
            if (bands >= uccrlist.Count)
            {
                cols = 1;
                rows = uccrlist.Count;
            }
            else
            {
                cols = (uccrlist.Count + bands - 1) / bands;
                rows = (uccrlist.Count + cols - 1) / cols;
            }

            //System.Diagnostics.Debug.WriteLine("Bands " + bands + " C " + cols + " R " + rows);
            
            h = h / rows;
            w = w / cols;

            for (int r = 0; r < bands; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int i = r * cols + c;
                    if (i < uccrlist.Count)
                    {
                        UserControlContainerResizable uccr = uccrlist[i];
                        uccr.Location = new Point(c * w, r * h);
                        uccr.Size = new Size(w, h);
                    }
                }
            }
        }

        private void panelPlayfield_MouseClick(object sender, MouseEventArgs e)
        {
            Select(null);
        }

        #endregion

        #region Saving

        List<Tuple<PanelInformation.PanelIDs, Point, Size, int>> GetSavedSettings()
        {
            string[] names = GetSetting(dbWindowNames, "").Split(',');
            int[] positions;
            int[] zorder;
            string pos = GetSetting(dbPositionSize, "");
            string zo = GetSetting(dbZOrder, "");

            if (pos.RestoreArrayFromString(out positions) && zo.RestoreArrayFromString(out zorder, 0, names.Length - 1) &&
                        names.Length == zorder.Length && positions.Length == 4 * names.Length)
            {
                var ret = new List<Tuple<PanelInformation.PanelIDs, Point, Size, int>>();
                for (int i = 0; i < names.Length; i++)
                {
                    PanelInformation.PanelIDs pid = (PanelInformation.PanelIDs)names[i].InvariantParseInt(0); // convert to panel ID, default is 0 (log)
                    int ppos = i * 4;
                    ret.Add(new Tuple<PanelInformation.PanelIDs, Point, Size, int>(pid, new Point(positions[ppos++], positions[ppos++]),
                                    new Size(positions[ppos++], positions[ppos++]), zorder[i]));
                }
                return ret;
            }
            else
                return null;
        }

        #endregion

        private List<UserControlContainerResizable> uccrlist = new List<UserControlContainerResizable>();

        private string dbWindowNames = "Windows";
        private string dbPositionSize = "Positons";     //sic, leave
        private string dbZOrder = "ZOrder";
        private string dbRolledUp = "RolledUp";

        private ExtendedControls.ExtListBoxForm popoutdropdown;

    }
}
