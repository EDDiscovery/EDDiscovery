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

namespace EDDiscovery.UserControls
{
    public partial class UserControlForm : Forms.DraggableFormPos
    {
        public UserControlCommonBase UserControl { get; private set; }
        public bool IsLoaded { get; private set; } = false;         // After shown, but before closing

        public enum TransparencyMode { Off, On, OnClickThru, OnFullyTransparent };

        public TransparencyMode TransparentMode { get; private set; } = TransparencyMode.Off;
        public Color TransparencyColorKey { get; private set; }  = Color.Transparent;     // if required, the control could modify this during its Init
        public bool IsTransparentModeOn { get { return TransparentMode != TransparencyMode.Off; } }
        public bool IsClickThruOn { get { return TransparentMode == TransparencyMode.OnClickThru || TransparentMode == TransparencyMode.OnFullyTransparent; } }

        public bool DisplayTitle { get; private set; }  = true;            // we are displaying the title
        public string DBRefName { get; private set; }
        public string WinTitle { get; private set; }

        public bool IsTransparencySupported { get { return !TransparencyColorKey.IsFullyTransparent(); } }

        public PanelInformation.PanelIDs PanelID { get { return UserControl?.panelid ?? PanelInformation.PanelIDs.GroupMarker; } }  // May not be open, if so, return an impossible

        public UserControlForm()
        {
            AdditionalSysMenus = new List<string>() { "&Transparent", "Show icon in Task&Bar for window" };
            AdditionalSysMenuSelected += SystemMenu;        // DO this first, enable extra system menu options for SmartSysMenuForm
            TopMostChanged += SaveTopMost;

            InitializeComponent();

            checkmousepositiontimer.Interval = 500;
            checkmousepositiontimer.Tick += CheckMouse;

            extButtonDrawnHelp.Image = ExtendedControls.TabStrip.HelpIcon;
            extButtonDrawnHelp.Text = "";
        }

        #region Public Interface

        public void Init(UserControlCommonBase c, string title, bool winborder, string rf, bool deftopmostp ,
                         Color labelnormal , Color labeltransparent, Color transparentkey )
        {
            //System.Diagnostics.Debug.WriteLine("UCF Init+");
            RestoreFormPositionRegKey = "PopUpForm" + rf;      // position remember key

            UserControl = c;
            UserControl.Dock = DockStyle.None;
            UserControl.Location = new Point(0, 10);
            UserControl.Size = new Size(200, 200);
            this.Controls.Add(c);
            labelnormalcolour = labelnormal;
            labeltransparentcolour = labeltransparent;

            TransparencyColorKey = UserControl.SupportTransparency ? transparentkey : Color.Transparent;
            WinTitle = label_title.Text = this.Text = title;            // label index always contains the wintitle, but may not be shown

            curwindowsborder = defwindowsborder = winborder;
            DBRefName = "PopUpForm" + rf;
            this.Name = rf;
            deftopmost = deftopmostp;

            labelControlText.Text = "";                                 // always starts blank..

            this.ShowInTaskbar = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DBRefName + "Taskbar", true);

            DisplayTitle = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DBRefName + "ShowTitle", true);

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                idk = DirectInputDevices.InputDeviceKeyboard.CreateKeyboard();
            }

            UpdateControls();

            Invalidate();

            var enumlisttt = new Enum[] {
                        EDTx.UserControlForm_extButtonDrawnShowTitle_ToolTip, EDTx.UserControlForm_extButtonDrawnMinimize_ToolTip, EDTx.UserControlForm_extButtonDrawnOnTop_ToolTip,
                        EDTx.UserControlForm_extButtonDrawnTaskBarIcon_ToolTip, EDTx.UserControlForm_extButtonDrawnTransparentMode_ToolTip,
                        EDTx.UserControlForm_extButtonDrawnClose_ToolTip};
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);
            //System.Diagnostics.Debug.WriteLine("UCF Init-");
        }

        public void SetControlText(string text)
        {
            labelControlText.Location = new Point(label_title.Location.X + label_title.Width + 16, labelControlText.Location.Y);
            labelControlText.Text = text;
            this.Text = WinTitle + " " + text;
        }

        public void SetTransparency(TransparencyMode t)
        {
            if (IsTransparencySupported)
            {
                TransparentMode = t;
                UpdateTransparency();
                UserDatabase.Instance.PutSettingInt(DBRefName + "Transparent", (int)TransparentMode);
            }
        }

        public void SetShowTitleInTransparency(bool t)
        {
            DisplayTitle = t;
            UpdateControls();
            UserDatabase.Instance.PutSettingBool(DBRefName + "ShowTitle", DisplayTitle);
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
            foreach(Control c in panelControls.Controls)
            {
                if (c.Visible) retval += c.Width;
            }
            return retval;
        }

#endregion

#region View Implementation

        public void UpdateTransparency()
        {
            bool showtransparent = IsTransparentModeOn && !inpanelshow;    // do we want to be transparent.. mode is on and not in panel show

            curwindowsborder = (!showtransparent && defwindowsborder);    // we have a border if not transparent and we have a default border turned on

            //System.Diagnostics.Debug.WriteLine($"UCF UpdateTranparency border={curwindowsborder} trans={showtransparent}");

            if (beforetransparency.IsFullyTransparent())        // record colour before transparency, dynamically
            {
                beforetransparency = this.BackColor;
                tkey = this.TransparencyKey;
                //System.Diagnostics.Debug.WriteLine("Record colour " + beforetransparency.ToString() + " tkey " + this.TransparencyKey);
            }

            UpdateControls();       // turn on/off controls accordingly

            //System.Diagnostics.Debug.WriteLine(Text + " tr " + transparentmode);

            this.TransparencyKey = (showtransparent) ? TransparencyColorKey : tkey;
            Color togo = (showtransparent) ? TransparencyColorKey : beforetransparency;

            this.BackColor = togo;
            statusStripBottom.BackColor = togo;
            extButtonDrawnTaskBarIcon.BackColor = extButtonDrawnTransparentMode.BackColor = extButtonDrawnClose.BackColor =
                    extButtonDrawnMinimize.BackColor = extButtonDrawnOnTop.BackColor = extButtonDrawnShowTitle.BackColor = extButtonDrawnHelp.BackColor =
                    panelControls.BackColor = panelTitleControlText.BackColor = togo;

            label_title.ForeColor = labelControlText.ForeColor = showtransparent ? labeltransparentcolour : labelnormalcolour;

            UserControl.SetTransparency(showtransparent, togo);     // tell the UCCB about the change

            PerformLayout();        // need to position the UCCB

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                // if in transparent click thru, we set transparent style.. else clear it.
                BaseUtils.Win32.UnsafeNativeMethods.ChangeWindowLong(this.Handle, BaseUtils.Win32.UnsafeNativeMethods.GWL.ExStyle,
                                    WS_EX.TRANSPARENT, showtransparent && TransparentMode == TransparencyMode.OnFullyTransparent ? WS_EX.TRANSPARENT : 0);
            }

            if (showtransparent || inpanelshow)     // timer needed if transparent, or if in panel show
                checkmousepositiontimer.Start();
            else
                checkmousepositiontimer.Stop();

            //System.Diagnostics.Debug.WriteLine("UCF UpdateTrans-");
        }


        // fix up controls to current transparency state
        private void UpdateControls()
        {
            bool showtransparent = IsTransparentModeOn && !inpanelshow;           // are we transparent..

            // border style, if transparent we use the curwindowsborder, else the defwindowsborder, to select the window frame
            FormBorderStyle = curwindowsborder ? FormBorderStyle.Sizable : FormBorderStyle.None;

            // the extensive controls in panel top is shown if we are not in transparent mode with no windows border, or if in transparent mode and we are in a panel show
            panelControls.Visible = (!IsTransparentModeOn && !curwindowsborder) || (IsTransparentModeOn && inpanelshow);

            // the title and control text is visible if DisplayTitle is on, or not transparent
            panelTitleControlText.Visible = (DisplayTitle || !showtransparent);   

            statusStripBottom.Visible = !showtransparent && !curwindowsborder;      // status strip on, when not transparent, and when we don't have border

            extButtonDrawnTaskBarIcon.Visible = extButtonDrawnClose.Visible = extButtonDrawnMinimize.Visible = extButtonDrawnOnTop.Visible = extButtonDrawnShowTitle.Visible = extButtonDrawnHelp.Visible = !showtransparent;

            extButtonDrawnTransparentMode.Visible = IsTransparencySupported && !showtransparent;
            extButtonDrawnShowTitle.Visible = IsTransparencySupported && !showtransparent;

            if (TransparentMode == TransparencyMode.On)
                extButtonDrawnTransparentMode.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Transparent;
            else if (TransparentMode == TransparencyMode.OnClickThru)
                extButtonDrawnTransparentMode.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.TransparentClickThru;
            else if (TransparentMode == TransparencyMode.OnFullyTransparent)
                extButtonDrawnTransparentMode.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.FullyTransparent;
            else
                extButtonDrawnTransparentMode.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.NotTransparent;

            extButtonDrawnTaskBarIcon.ImageSelected = this.ShowInTaskbar ? ExtendedControls.ExtButtonDrawn.ImageType.WindowInTaskBar : ExtendedControls.ExtButtonDrawn.ImageType.WindowNotInTaskBar;
            extButtonDrawnShowTitle.ImageSelected = DisplayTitle ? ExtendedControls.ExtButtonDrawn.ImageType.Captioned : ExtendedControls.ExtButtonDrawn.ImageType.NotCaptioned;
            extButtonDrawnOnTop.ImageSelected = TopMost ? ExtendedControls.ExtButtonDrawn.ImageType.OnTop : ExtendedControls.ExtButtonDrawn.ImageType.Floating;
        }

        const int UCPaddingWidth = 3;

        private void UserControlForm_Layout(object sender, LayoutEventArgs e)
        {
            if (UserControl != null)
            {
                UserControl.Location = new Point(3, panelControls.Visible || panelTitleControlText.Visible ? panelControls.Bottom+1 : 2);
                UserControl.Size = new Size(ClientRectangle.Width - UCPaddingWidth*2, ClientRectangle.Height - UserControl.Location.Y - (curwindowsborder ? 0 : statusStripBottom.Height));
            }
        }

        private void UserControlForm_Shown(object sender, EventArgs e)          // as launched, it may not be in front (as its launched from a control).. bring to front
        {
            //System.Diagnostics.Debug.WriteLine("UCF Shown+");
            this.BringToFront();

            if (IsTransparencySupported)
                TransparentMode = (TransparencyMode)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DBRefName + "Transparent", UserControl.DefaultTransparent ? (int)TransparencyMode.On : (int)TransparencyMode.Off);

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

        public bool AllowClose()
        {
            return UserControl?.AllowClose() ?? true;       // does the UCCB allow close?
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (UserControl?.AllowClose() ?? true)          // does the UCCB allow close?
            {
                IsLoaded = false;

                UserControl?.CloseDown();                   // it may be null
            }
            else
                e.Cancel = true;                            // cancel close

            base.OnFormClosing(e);
        }


#endregion

#region Clicks

        private void button_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button_ontop_Click(object sender, EventArgs e)
        {
            SetTopMost(!TopMost);
        }

        private void button_transparency_Click(object sender, EventArgs e)       // only works if transparency is supported
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

        private void button_taskbaricon_Click(object sender, EventArgs e)
        {
            SetShowInTaskBar(!this.ShowInTaskbar);
        }

        private void button_showtitle_Click(object sender, EventArgs e)
        {
            SetShowTitleInTransparency(!DisplayTitle);
        }

        // best way of knowing your inside the client.. using mouseleave/enter with transparency does not work..
        private void CheckMouse(object sender, EventArgs e)     
        {
            if (IsLoaded)
            {
                //System.Diagnostics.Debug.WriteLine($"UCF Check {Bounds} vs {MousePosition}");
                if (Bounds.Contains(MousePosition))     // bounds of window contains the mouse pointer
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
                    button_transparency_Click(extButtonDrawnTransparentMode, EventArgs.Empty);
                else
                    ExtendedControls.MessageBoxTheme.Show(this, "This panel does not support transparency");
            }
            else
            {
                button_taskbaricon_Click(extButtonDrawnTaskBarIcon, EventArgs.Empty);
            }
        }

        private void extButtonDrawnHelp_Click(object sender, EventArgs e)
        {
            EDDHelp.Help(this, extButtonDrawnHelp.PointToScreen(new Point(0,extButtonDrawnHelp.Height)), UserControl.HelpKeyOrAddress());
        }

        #endregion

        private void label_title_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void labelControlText_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void panel_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        private bool inpanelshow = false;       // if we are in a panel show when we were transparent
        private bool defwindowsborder;          // what we started with border style
        private bool curwindowsborder;          // applied setting
        private Color beforetransparency = Color.Transparent;
        private Color tkey = Color.Transparent;
        private Color labelnormalcolour, labeltransparentcolour;

        private Timer checkmousepositiontimer = new Timer();      // timer to monitor for entry into form when transparent.. only sane way in forms
        private bool deftopmost;

        private DirectInputDevices.InputDeviceKeyboard idk;     // used to sniff in transparency mode
    }

    ///-------------------------------------------------------------------- List of forms

    public class UserControlFormList
    {
        public int Count { get { return forms.Count; } }
        public UserControlForm this[int i] { get { return forms[i]; } }

        public UserControlFormList(EDDiscoveryForm ed)
        {
            forms = new List<UserControlForm>();
            discoveryform = ed;
        }

        public UserControlForm GetByWindowsRefName(string name)
        {
            foreach (UserControlForm u in forms)     // first complete name
            {
                if (u.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) )
                    return u;
            }

            foreach (UserControlForm u in forms)     // then partial start name
            {
                if (u.Name.StartsWith(name, StringComparison.InvariantCultureIgnoreCase) )
                    return u;
            }

            return null;
        }

        public UserControlForm Find(PanelInformation.PanelIDs p)
        {
            foreach (UserControlForm u in forms)     
            {
                if (u.UserControl != null && u.UserControl.panelid == p)
                    return u;
            }

            return null;
        }

        public UserControlForm NewForm()                // a new form is needed
        {
            UserControlForm tcf = new UserControlForm();
            forms.Add(tcf);
            tcf.FormClosed += FormClosedCallback;
            return tcf;
        }

        private void FormClosedCallback(Object sender, FormClosedEventArgs e)       // called when form closes.. by user or by us.  Remove from list
        {
            UserControlForm tcf = (UserControlForm)sender;
            forms.Remove(tcf);
            discoveryform.ActionRun(Actions.ActionEventEDList.onPopDown, new BaseUtils.Variables(new string[] { "PopOutName", tcf.DBRefName.Substring(9), "PopOutTitle", tcf.WinTitle }));
        }

        public int CountOf(PanelInformation.PanelIDs p)
        {
            int count = 0;

            foreach (UserControlForm tcf in forms)
            {
                if (tcf.PanelID == p)
                    count++;
            }

            return count;
        }

        public void ShowAllInTaskBar()
        {
            foreach (UserControlForm ucf in forms)
            {
                if (ucf.IsLoaded)
                    ucf.SetShowInTaskBar(true);
            }
        }

        public void MakeAllOpaque()
        {
            foreach (UserControlForm ucf in forms)
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
            List<UserControlForm> list = new List<UserControlForm>(forms);       // so, closing it ends up calling back to FormCloseCallBack
                                                                                    // and it changes tabforms. So we need a copy to safely do this
            foreach (UserControlForm ucf in list)
            {
                ucf.Close();        // don't change tabforms.. the FormCloseCallBack does this
            }
        }

        public bool AllowClose()
        {
            foreach( var f in forms )
            {
                if (!f.AllowClose())
                    return false;
            }
            return true;
        }

        private List<UserControlForm> forms;
        private EDDiscoveryForm discoveryform;
    }
}
