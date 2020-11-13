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
using EDDiscovery.UserControls;
using BaseUtils.Win32Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EliteDangerousCore.DB;

namespace EDDiscovery.Forms
{
    public partial class UserControlForm : Forms.DraggableFormPos
    {
        public UserControlCommonBase UserControl;
        public bool IsLoaded { get; private set; } = false;         // After shown, but before closing

        public enum TransparencyMode { Off, On, OnClickThru, OnFullyTransparent };

        public TransparencyMode TransparentMode = TransparencyMode.Off;
        public Color TransparencyColorKey = Color.Transparent;     // if required, the control could modify this during its Init
        public bool IsTransparent { get { return TransparentMode != TransparencyMode.Off; } }
        public bool IsClickThruOn { get { return TransparentMode == TransparencyMode.OnClickThru || TransparentMode == TransparencyMode.OnFullyTransparent; } }

        public bool DisplayTitle = true;            // we are displaying the title
        public string DBRefName;
        public string WinTitle;

        private bool inpanelshow = false;       // if we are in a panel show when we were transparent
        private bool defwindowsborder;
        private bool curwindowsborder;          // applied setting
        private Color beforetransparency = Color.Transparent;
        private Color tkey = Color.Transparent;
        private Color labelnormalcolour, labeltransparentcolour;

        private Timer timer = new Timer();      // timer to monitor for entry into form when transparent.. only sane way in forms
        private bool deftopmost, deftransparent;

        private DirectInputDevices.InputDeviceKeyboard idk;     // used to sniff in transparency mode

        public bool IsTransparencySupported { get { return !TransparencyColorKey.IsFullyTransparent(); } }

        public UserControlForm()
        {
            AdditionalSysMenus = new List<string>() { "&Transparent", "Show icon in Task&Bar for window" };
            AdditionalSysMenuSelected += SystemMenu;        // DO this first, enable extra system menu options for SmartSysMenuForm
            TopMostChanged += SaveTopMost;

            InitializeComponent();

            timer.Interval = 500;
            timer.Tick += CheckMouse;

            extButtonDrawnHelp.Image = ExtendedControls.TabStrip.HelpIcon;
            extButtonDrawnHelp.Text = "";
        }

        #region Public Interface

        public void Init(EDDiscovery.UserControls.UserControlCommonBase c, string title, bool winborder, string rf, bool deftopmostp ,
                         bool deftransparentp , Color labelnormal , Color labeltransparent, Color transparentkey )
        {
            //System.Diagnostics.Debug.WriteLine("UCF Init+");
            RestoreFormPositionRegKey = "PopUpForm" + rf;      // position remember key

            UserControl = c;
            c.Dock = DockStyle.None;
            c.Location = new Point(0, 10);
            c.Size = new Size(200, 200);
            this.Controls.Add(c);
            deftransparent = deftransparentp;   // only applied if allowed to be transparent.
            labelnormalcolour = labelnormal;
            labeltransparentcolour = labeltransparent;

            TransparencyColorKey = c.SupportTransparency ? transparentkey : Color.Transparent;
            WinTitle = label_index.Text = this.Text = title;            // label index always contains the wintitle, but may not be shown

            curwindowsborder = defwindowsborder = winborder;
            DBRefName = "PopUpForm" + rf;
            this.Name = rf;
            deftopmost = deftopmostp;
            deftransparent = false;

            labelControlText.Text = "";                                 // always starts blank..

            this.ShowInTaskbar = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DBRefName + "Taskbar", true);

            DisplayTitle = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DBRefName + "ShowTitle", true);

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                idk = DirectInputDevices.InputDeviceKeyboard.CreateKeyboard();
            }

            UpdateControls();

            Invalidate();

            BaseUtils.Translator.Instance.Translate(toolTip1,this);
            //System.Diagnostics.Debug.WriteLine("UCF Init-");
        }

        public void SetControlText(string text)
        {
            labelControlText.Location = new Point(label_index.Location.X + label_index.Width + 16, labelControlText.Location.Y);
            labelControlText.Text = text;
            this.Text = WinTitle + " " + text;
        }

        public void SetTransparency(TransparencyMode t)
        {
            if (IsTransparencySupported)
            {
                TransparentMode = t;
                UpdateTransparency();
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DBRefName + "Transparent", (int)TransparentMode);
            }
        }

        public void SetShowTitleInTransparency(bool t)
        {
            DisplayTitle = t;
            UpdateControls();
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DBRefName + "ShowTitle", DisplayTitle);
            UserControl.onControlTextVisibilityChanged(DisplayTitle);            
        }

        public bool IsControlTextVisible()
        {
            return DisplayTitle;
        }

        public void SetTopMost(bool t)
        {
            TopMost = t;        // this calls Win32.SetWindowPos, which then plays with the actual topmost bit in windows extended style
                                // and loses the transparency bit!  So therefore
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DBRefName + "TopMost", TopMost);
            UpdateTransparency();   // need to reestablish correct transparency again
        }

        private void SaveTopMost(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DBRefName + "TopMost", TopMost);
        }

        public void SetShowInTaskBar(bool t)
        {
            this.ShowInTaskbar = t;
            UpdateControls();
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DBRefName + "Taskbar", t);
        }

        public UserControlCommonBase FindUserControl(Type c)
        {
            if (UserControl != null && UserControl.GetType().Equals(c))
                return UserControl;
            else
                return null;
        }

        public int TitleBarMinWidth()
        {
            int retval = 0;
            foreach(Control c in panelTop.Controls)
            {
                if (c.Visible) retval += c.Width;
            }
            return retval;
        }

#endregion

#region View Implementation

        private void UpdateTransparency()
        {
            //System.Diagnostics.Debug.WriteLine("UCF UpdateTrans+");
            curwindowsborder = (!IsTransparent && defwindowsborder);    // we have a border if not transparent and we have a def border
            bool showtransparent = IsTransparent && !inpanelshow;           // are we transparent..  must not be in panel show

            if (beforetransparency.IsFullyTransparent())        // record colour before transparency, dynamically
            {
                beforetransparency = this.BackColor;
                tkey = this.TransparencyKey;
                //System.Diagnostics.Debug.WriteLine("Record colour " + beforetransparency.ToString() + " tkey " + this.TransparencyKey);
            }

            UpdateControls();

            //System.Diagnostics.Debug.WriteLine(Text + " tr " + transparentmode);

            this.TransparencyKey = (showtransparent) ? TransparencyColorKey : tkey;
            Color togo = (showtransparent) ? TransparencyColorKey : beforetransparency;

            this.BackColor = togo;
            statusStripBottom.BackColor = togo;
            panel_taskbaricon.BackColor = panel_transparent.BackColor = panel_close.BackColor =
                    panel_minimize.BackColor = panel_ontop.BackColor = panel_showtitle.BackColor = extButtonDrawnHelp.BackColor = panelTop.BackColor = togo;

            label_index.ForeColor = labelControlText.ForeColor = showtransparent ? labeltransparentcolour : labelnormalcolour;

            UserControl.SetTransparency(showtransparent, togo);
            PerformLayout();

            // if in transparent click thru, we set transparent style.. else clear it.
            BaseUtils.Win32.UnsafeNativeMethods.ChangeWindowLong(this.Handle, BaseUtils.Win32.UnsafeNativeMethods.GWL.ExStyle,
                                WS_EX.TRANSPARENT, showtransparent && TransparentMode == TransparencyMode.OnFullyTransparent ? WS_EX.TRANSPARENT : 0);

            if (showtransparent || inpanelshow)     // timer needed if transparent, or if in panel show
                timer.Start();
            else
                timer.Stop();

            //System.Diagnostics.Debug.WriteLine("UCF UpdateTrans-");
        }

        private void UpdateControls()
        {
            bool transparent = IsTransparent && !inpanelshow;           // are we transparent..

            FormBorderStyle = curwindowsborder ? FormBorderStyle.Sizable : FormBorderStyle.None;
            panelTop.Visible = !curwindowsborder;       // this also has the effect of removing the label_ and panel_ buttons

            statusStripBottom.Visible = !transparent && !curwindowsborder;      // status strip on, when not transparent, and when we don't have border

            panel_taskbaricon.Visible = panel_close.Visible = panel_minimize.Visible = panel_ontop.Visible = panel_showtitle.Visible = extButtonDrawnHelp.Visible = !transparent;

            panel_transparent.Visible = IsTransparencySupported && !transparent;
            panel_showtitle.Visible = IsTransparencySupported && !transparent;
            panel_showtitle.Visible = IsTransparencySupported && !transparent;

            if (TransparentMode == TransparencyMode.On)
                panel_transparent.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Transparent;
            else if (TransparentMode == TransparencyMode.OnClickThru)
                panel_transparent.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.TransparentClickThru;
            else if (TransparentMode == TransparencyMode.OnFullyTransparent)
                panel_transparent.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.FullyTransparent;
            else
                panel_transparent.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.NotTransparent;

            label_index.Visible = labelControlText.Visible = (DisplayTitle || !transparent);   //  titles are on, or transparent is off

            panel_taskbaricon.ImageSelected = this.ShowInTaskbar ? ExtendedControls.ExtButtonDrawn.ImageType.WindowInTaskBar : ExtendedControls.ExtButtonDrawn.ImageType.WindowNotInTaskBar;
            panel_showtitle.ImageSelected = DisplayTitle ? ExtendedControls.ExtButtonDrawn.ImageType.Captioned : ExtendedControls.ExtButtonDrawn.ImageType.NotCaptioned;
            panel_ontop.ImageSelected = TopMost ? ExtendedControls.ExtButtonDrawn.ImageType.OnTop : ExtendedControls.ExtButtonDrawn.ImageType.Floating;
        }

        const int UCPaddingWidth = 3;

        private void UserControlForm_Layout(object sender, LayoutEventArgs e)
        {
            if (UserControl != null)
            {
                UserControl.Location = new Point(3, curwindowsborder ? 2 : panelTop.Location.Y + panelTop.Height);
                UserControl.Size = new Size(ClientRectangle.Width - UCPaddingWidth*2, ClientRectangle.Height - UserControl.Location.Y - (curwindowsborder ? 0 : statusStripBottom.Height));
            }
        }

        private void UserControlForm_Shown(object sender, EventArgs e)          // as launched, it may not be in front (as its launched from a control).. bring to front
        {
            //System.Diagnostics.Debug.WriteLine("UCF Shown+");
            this.BringToFront();

            if (IsTransparencySupported)
                TransparentMode = (TransparencyMode)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DBRefName + "Transparent", deftransparent ? (int)TransparencyMode.On : (int)TransparencyMode.Off);

            bool wantedTopMost = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DBRefName + "TopMost", deftopmost);
            //kludge 
            SetTopMost(wantedTopMost);
            SetTopMost(!wantedTopMost);
            SetTopMost(wantedTopMost); // this also establishes transparency

            var top = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DBRefName + "Top", -999);
            //System.Diagnostics.Debug.WriteLine("Position Top is {0} {1}", dbrefname, top);

            if (UserControl != null)
            {
                System.Diagnostics.Debug.WriteLine("UCCB Call set curosr, load layout, initial display");
                UserControl.SetCursor(UserControl.discoveryform.PrimaryCursor);
                UserControl.LoadLayout();
                UserControl.InitialDisplay();
            }

            IsLoaded = true;
            //System.Diagnostics.Debug.WriteLine("UCF Shown-");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }

        private void UserControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsLoaded = false;

            if (UserControl != null)
                UserControl.CloseDown();
        }

#endregion

#region Clicks

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel_ontop_Click(object sender, EventArgs e)
        {
            SetTopMost(!TopMost);
        }

        private void panel_transparency_Click(object sender, EventArgs e)       // only works if transparency is supported
        {
            inpanelshow = true; // in case we go transparent, we need to make sure its on.. since it won't be on if the timer is not running

            //nasty.. but nice
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                TransparentMode = (TransparencyMode)(((int)TransparentMode + 1) % Enum.GetValues(typeof(TransparencyMode)).Length);
            }
            else
            {
                TransparentMode = (TransparentMode == TransparencyMode.Off) ? TransparencyMode.On : TransparencyMode.Off;   // no idea what happens in Mono
            }

            SetTransparency(TransparentMode);
        }

        private void panel_taskbaricon_Click(object sender, EventArgs e)
        {
            SetShowInTaskBar(!this.ShowInTaskbar);
        }

        private void panel_showtitle_Click(object sender, EventArgs e)
        {
            SetShowTitleInTransparency(!DisplayTitle);
        }

        private void CheckMouse(object sender, EventArgs e)     // best way of knowing your inside the client.. using mouseleave/enter with transparency does not work..
        {
            if (IsLoaded)
            {
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " Tick " + Name + " " + Text + " " + transparentmode + " " + inpanelshow);
                if (ClientRectangle.Contains(this.PointToClient(MousePosition)))
                {
                    //System.Diagnostics.Debug.WriteLine(Environment.TickCount + "In area");
                    if (!inpanelshow)
                    {
                        if (IsClickThruOn)
                        {
                            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                            {
                                if (idk.IsKeyPressed(EDDConfig.Instance.ClickThruKey, recheck: true))
                                    inpanelshow = true;
                            }
                        }
                        else
                            inpanelshow = true;

                        if (inpanelshow)
                            UpdateTransparency();
                    }

                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine(Environment.TickCount + "Out of area");

                    if (inpanelshow)
                    {
                        inpanelshow = false;
                        UpdateTransparency();
                    }
                }
            }
        }


        #endregion

        #region Resizing

        public new void RequestTemporaryResize(Size w)                  // Size w is the Form UserControl area wanted inside the window (26/1/2018)
        {
            w.Width += UCPaddingWidth * 2 + 2;  //2 is a kludge     We need to add on area used by Form controls..
            w.Height += 2 + UserControl.Location.Y + statusStripBottom.Height;
            base.RequestTemporaryResize(new Size(w.Width, w.Height));        // need to add on control area.
        }

        public void RequestTemporaryMinimiumSize(Size w)            // again, in terms of UserControl area. (26/1/2018)
        {
            w.Width += UCPaddingWidth * 2 + 2;  //2 is a kludge
            w.Height += 2;
            w.Height += UserControl.Location.Y + statusStripBottom.Height;      // add on height for form controls

            if (ClientRectangle.Height < w.Height || ClientRectangle.Width < w.Width)
                base.RequestTemporaryResize(w);
        }

        public void RequestTemporaryResizeExpand(Size w)            // again, in terms of UserControl area. What more do we want.. (26/1/2018)
        {
            w.Width += ClientRectangle.Width;                       // expand 
            w.Height += ClientRectangle.Height;
            base.RequestTemporaryResize(w);
        }

        #endregion

        #region System menu for border windows - added in in Init for smartsysmenu

        void SystemMenu(int v)      // index into array
        {
            if (v == 0)
            {
                if (IsTransparencySupported)
                    panel_transparency_Click(panel_transparent, EventArgs.Empty);
                else
                    ExtendedControls.MessageBoxTheme.Show(this, "This panel does not support transparency");
            }
            else
            {
                panel_taskbaricon_Click(panel_taskbaricon, EventArgs.Empty);
            }
        }

        private void extButtonDrawnHelp_Click(object sender, EventArgs e)
        {
            EDDHelp.Help(this, extButtonDrawnHelp.PointToScreen(new Point(0,extButtonDrawnHelp.Height)), UserControl);
        }

        #endregion

        private void label_index_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void labelControlText_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void panelTop_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }
    }


    public class UserControlFormList
    {
        private List<UserControlForm> tabforms;
        EDDiscoveryForm discoveryform;

        public int Count { get { return tabforms.Count; } }

        public UserControlFormList(EDDiscoveryForm ed)
        {
            tabforms = new List<UserControlForm>();
            discoveryform = ed;
        }

        public UserControlForm this[int i] { get { return tabforms[i]; } }

        public UserControlForm GetByWindowsRefName(string name)
        {
            foreach (UserControlForm u in tabforms)     // first complete name
            {
                if (u.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) )
                    return u;
            }

            foreach (UserControlForm u in tabforms)     // then partial start name
            {
                if (u.Name.StartsWith(name, StringComparison.InvariantCultureIgnoreCase) )
                    return u;
            }

            return null;
        }

        public UserControlForm NewForm()                // a new form is needed
        {
            UserControlForm tcf = new UserControlForm();
            tabforms.Add(tcf);
            tcf.FormClosed += FormClosedCallback;
            return tcf;
        }

        private void FormClosedCallback(Object sender, FormClosedEventArgs e)       // called when form closes.. by user or by us.  Remove from list
        {
            UserControlForm tcf = (UserControlForm)sender;
            tabforms.Remove(tcf);
            discoveryform.ActionRun(Actions.ActionEventEDList.onPopDown, null, new BaseUtils.Variables(new string[] { "PopOutName", tcf.DBRefName.Substring(9), "PopOutTitle", tcf.WinTitle }));
        }

        public List<UserControlCommonBase> GetListOfControls(Type c)
        {
            List<UserControlCommonBase> lc = new List<UserControlCommonBase>();

            foreach (UserControlForm tcf in tabforms)
            {
                if (tcf.IsLoaded)
                {
                    UserControlCommonBase uc = tcf.FindUserControl(c);
                    if (uc != null)
                        lc.Add(uc);
                }
            }

            return lc;
        }

        public int CountOf(Type c)
        {
            int count = 0;

            foreach (UserControlForm tcf in tabforms)
            {
                if (tcf.FindUserControl(c) != null)
                    count++;
            }

            return count;
        }

        public void ShowAllInTaskBar()
        {
            foreach (UserControlForm ucf in tabforms)
            {
                if (ucf.IsLoaded)
                    ucf.SetShowInTaskBar(true);
            }
        }

        public void MakeAllOpaque()
        {
            foreach (UserControlForm ucf in tabforms)
            {
                if (ucf.IsLoaded)
                {
                    ucf.SetTransparency(UserControlForm.TransparencyMode.Off);
                    ucf.SetShowTitleInTransparency(true);
                }
            }
        }

        public void CloseAll()
        {
            List<UserControlForm> list = new List<UserControlForm>(tabforms);       // so, closing it ends up calling back to FormCloseCallBack
                                                                                    // and it changes tabforms. So we need a copy to safely do this
            foreach (UserControlForm ucf in list)
            {
                ucf.Close();        // don't change tabforms.. the FormCloseCallBack does this
            }
        }

    }
}
