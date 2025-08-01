/*
 * Copyright © 2016 - 2025 EDDiscovery development team
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

using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlForm : Forms.DraggableFormPos
    {
        public UserControlCommonBase UserControl { get; private set; }
        public bool IsLoaded { get; private set; } = false;         // After shown, but before closing

        public bool IsTransparencySupported { get { return UserControl?.SupportTransparency ?? false; } }

        public enum TransparencyMode { Off, On, OnClickThru, OnFullyTransparent };
        public TransparencyMode TransparentMode { get; private set; } = TransparencyMode.Off;

        public bool IsTransparentModeOn { get { return TransparentMode != TransparencyMode.Off; } }

        public bool IsCurrentlyTransparent {  get { return IsTransparentModeOn && !inpanelshow; } }

        public bool IsClickThruOn { get { return TransparentMode == TransparencyMode.OnClickThru || TransparentMode == TransparencyMode.OnFullyTransparent; } }

        public bool DisplayTitle { get; private set; }  = true;            // we are displaying the title
        public string DBRefName { get; private set; }
        public string WinTitle { get; private set; }

        public int PopOutNumber {  get { return UserControl.DisplayNumber - UserControlCommonBase.DisplayNumberPopOuts; } }     // 0, 1, 2 etc

        public PanelInformation.PanelIDs PanelID { get { return UserControl?.PanelID ?? PanelInformation.PanelIDs.GroupMarker; } }  // May not be open, if so, return an impossible

        public UserControlForm()
        {
            TopMostChanged += SaveTopMost;

            InitializeComponent();

            checkmousepositiontimer.Interval = 500;
            checkmousepositiontimer.Tick += CheckMouse;

            extButtonDrawnHelp.Image = ExtendedControls.TabStrip.HelpIcon;
                extButtonDrawnHelp.Text = "";
        }


        #region Public Interface

        public void Init(UserControlCommonBase c, string title, bool winborder, string refkey, bool deftopmostp )
        {
            //System.Diagnostics.Debug.WriteLine("UCF Init+");
            DBRefName = RestoreFormPositionRegKey = "PopUpForm" + refkey;      // Keys for form position and form properties are based on this naming (PopUpFormBookmarks).  Note UCCB using Get/Put Settings are based on another system (see UCCB)

            UserControl = c;
            UserControl.Dock = DockStyle.Fill;
            UserControl.Margin = new Padding(0);
            UserControl.Padding = new Padding(0);
            this.Controls.Add(c);
            Controls.SetChildIndex(UserControl, 0);

            WinTitle = label_title.Text = this.Text = title;            // label index always contains the wintitle, but may not be shown

            this.Name = refkey;
            deftopmost = deftopmostp;

            labelControlText.Text = "";                                 // always starts blank..

            this.ShowInTaskbar = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(DBRefName + "Taskbar", true);

            DisplayTitle = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(DBRefName + "ShowTitle", true);

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                idk = DirectInputDevices.InputDeviceKeyboard.CreateKeyboard();
            }

            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip,this);

            if (IsTransparencySupported)
                TransparentMode = (TransparencyMode)EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(DBRefName + "Transparent", UserControl.DefaultTransparent ? (int)TransparencyMode.On : (int)TransparencyMode.Off);

            lasttransparentmodereported = IsTransparentModeOn;      // record what we started with

            bool wantedTopMost = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(DBRefName + "TopMost", deftopmost);
            TopMost = wantedTopMost;
        }

        // call to update the control text in the title area
        public void SetControlText(string text)
        {
            labelControlText.Location = new Point(label_title.Location.X + label_title.Width + 16, labelControlText.Location.Y);
            labelControlText.Text = text;
            this.Text = WinTitle + " " + text;
        }

        // call to change the transparency mode

        public void SetTransparency(TransparencyMode t)
        {
            if (IsTransparencySupported)
            {
                TransparentMode = t;
                EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(DBRefName + "Transparent", (int)TransparentMode);

                UpdateTransparencyControls();

                if (lasttransparentmodereported != IsTransparentModeOn)     // if we changed major mode, inform the panel so it can redraw 
                {
                    lasttransparentmodereported = IsTransparentModeOn;
                    UserControl.TransparencyModeChanged(IsTransparentModeOn);
                }
            }
        }

        public void SetShowTitleInTransparency(bool t)
        {
            DisplayTitle = t;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(DBRefName + "ShowTitle", DisplayTitle);
            UpdateTransparencyControls();
            UserControl.onControlTextVisibilityChanged(DisplayTitle);            
        }

        public bool IsControlTextVisible()
        {
            return DisplayTitle;
        }

        public void SetTopMost(bool t)
        {
            if ( TopMost != t )
            { 
                TopMost = t;        // this calls Win32.SetWindowPos, which then plays with the actual topmost bit in windows extended style
                                    // and loses the transparency bit!  So therefore
                UpdateTransparencyControls();   // need to reestablish correct transparency again
            }
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(DBRefName + "TopMost", TopMost);
        }

        private void SaveTopMost(SmartSysMenuForm sys)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(DBRefName + "TopMost", TopMost);
        }

        public void SetShowInTaskBar(bool t)
        {
            this.ShowInTaskbar = t;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(DBRefName + "Taskbar", t);
            UpdateTransparencyControls();   // redraw
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


        // Called by pop out forms when theme has changed, to give us a chance to retheme
        public void OnThemeChanged()
        {
            Font = Theme.Current.GetFont;
            ExtendedControls.Theme.Current.ApplyStd(UserControl);
            UpdateTransparencyControls();
        }

        #endregion

        #region View Implementation

        // we need to do this in OnHandleCreate as toggle show title bar causes the window to be recreated, losing the sys menu
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            var sysMenus = new List<SystemMenuItem>()
            {
                    new SystemMenuItem(),
                    new SystemMenuItem("Show icon in Task&Bar for window", x=>{ x.SetCheck(this.ShowInTaskbar); },x=>{ button_taskbaricon_Click(extButtonDrawnTaskBarIcon, EventArgs.Empty); }),
                    new SystemMenuItem("Show Title Bar", x=>{ x.SetCheck(DisplayTitle); },x=>{ button_showtitle_Click(extButtonDrawnShowTitle, EventArgs.Empty);; }),
            };

            if (IsTransparencySupported)
            {
                sysMenus.AddRange(new List<SystemMenuItem>
                        {
                        new SystemMenuItem("Transparency",
                                new List<SystemMenuItem>() {
                                        new SystemMenuItem("Off", x=>{ x.SetCheck(TransparentMode == TransparencyMode.Off); },x => { TransparentMode = TransparencyMode.Off; SetTransparency(TransparentMode); }),
                                        new SystemMenuItem("On", x=>{ x.SetCheck(TransparentMode == TransparencyMode.On);},x => { TransparentMode = TransparencyMode.On; SetTransparency(TransparentMode); }),
                                        new SystemMenuItem("On, Click to activate", x=>{ x.SetCheck(TransparentMode == TransparencyMode.OnClickThru);},x => { TransparentMode = TransparencyMode.OnClickThru; SetTransparency(TransparentMode); }),
                                        new SystemMenuItem("On, Click to activate, Fully Transparent", x=>{ x.SetCheck(TransparentMode == TransparencyMode.OnFullyTransparent);},x => { TransparentMode = TransparencyMode.OnFullyTransparent; SetTransparency(TransparentMode); })
                                        })
                        });
            }

            InstallSystemMenuItems(sysMenus);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateTransparencyControls();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            //System.Diagnostics.Debug.WriteLine($"UCF {Name} Shown");

            // as launched, it may not be in front (as its launched from a control).. bring to front
            this.BringToFront();

            // tell UC of events in UCCB order
            if (UserControl != null)
            {
                UserControl.TransparencyModeChanged(IsTransparentModeOn);       
                UserControl.CallLoadLayout();
                UserControl.CallInitialDisplay();
            }

            IsLoaded = true;
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
                checkmousepositiontimer.Stop();
                UserControl?.CallCloseDown();                   // it may be null
            }
            else
                e.Cancel = true;                            // cancel close

            base.OnFormClosing(e);
        }

        #endregion

        #region Transparency control

        private void UpdateTransparencyControls()
        {
            bool wantawindowsborder = Theme.Current.WindowsFrame;
            bool showwindowsborder = wantawindowsborder && !IsCurrentlyTransparent;
            bool showcontrols = inpanelshow && !wantawindowsborder;
            bool showourtitle = DisplayTitle || showcontrols;
            bool showstatusbar = !wantawindowsborder && inpanelshow;

            System.Diagnostics.Debug.WriteLine($"UCF UpdateTranparency wb:{wantawindowsborder} istr:{IsCurrentlyTransparent} showwb:{showwindowsborder} showc:{showcontrols} showtitle:{showourtitle} showsb:{showstatusbar}");

            FormBorderStyle = showwindowsborder ? FormBorderStyle.Sizable : FormBorderStyle.None;

            panelTopArea.Visible = showcontrols || showourtitle;            // we vanish this if both are off
            panelControls.Visible = showcontrols;
            panelTitleControlText.Visible = showourtitle;

            statusStripBottom.Visible = showstatusbar;

            this.TransparencyKey = Theme.Current.TransparentColorKey;
            Color curbackground = IsCurrentlyTransparent ? TransparencyKey : Theme.Current.Form;

            //System.Diagnostics.Debug.WriteLine($".. bc:{curbackground}");

            this.BackColor = curbackground;
            statusStripBottom.BackColor = curbackground;
            extButtonDrawnTaskBarIcon.BackColor = extButtonDrawnTransparentMode.BackColor = extButtonDrawnClose.BackColor =
                    extButtonDrawnMinimize.BackColor = extButtonDrawnOnTop.BackColor = extButtonDrawnShowTitle.BackColor = extButtonDrawnHelp.BackColor =
                    panelControls.BackColor = panelTitleControlText.BackColor = panelTopArea.BackColor = curbackground;

            label_title.ForeColor = labelControlText.ForeColor = IsCurrentlyTransparent ? Theme.Current.SPanelColor : Theme.Current.LabelColor;

            if (TransparentMode == TransparencyMode.On)
                extButtonDrawnTransparentMode.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Transparent;
            else if (TransparentMode == TransparencyMode.OnClickThru)
                extButtonDrawnTransparentMode.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.TransparentClickThru;
            else if (TransparentMode == TransparencyMode.OnFullyTransparent)
                extButtonDrawnTransparentMode.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.FullyTransparent;
            else
                extButtonDrawnTransparentMode.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.NotTransparent;
            
            extButtonDrawnTransparentMode.Visible = IsTransparencySupported;
            extButtonDrawnTaskBarIcon.ImageSelected = this.ShowInTaskbar ? ExtendedControls.ExtButtonDrawn.ImageType.WindowInTaskBar : ExtendedControls.ExtButtonDrawn.ImageType.WindowNotInTaskBar;
            extButtonDrawnShowTitle.ImageSelected = DisplayTitle ? ExtendedControls.ExtButtonDrawn.ImageType.Captioned : ExtendedControls.ExtButtonDrawn.ImageType.NotCaptioned;
            extButtonDrawnOnTop.ImageSelected = TopMost ? ExtendedControls.ExtButtonDrawn.ImageType.OnTop : ExtendedControls.ExtButtonDrawn.ImageType.Floating;

            UserControl?.SetTransparency(IsCurrentlyTransparent, curbackground);     // tell the UCCB about the current state

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                // if in transparent click thru, we set transparent style.. else clear it.
                BaseUtils.Win32.UnsafeNativeMethods.ChangeWindowLong(this.Handle, BaseUtils.Win32.UnsafeNativeMethods.GWL.ExStyle,
                                    BaseUtils.Win32Constants.WS_EX.TRANSPARENT,
                                    IsCurrentlyTransparent && TransparentMode == TransparencyMode.OnFullyTransparent ? BaseUtils.Win32Constants.WS_EX.TRANSPARENT : 0);
            }

            // if we don't have a windows border, or we do but transparent mode is on (meaning the border will disappear when mouse is away)
            if (!showwindowsborder || IsTransparentModeOn)
            {
                //System.Diagnostics.Debug.WriteLine("UCF Timer on");
                checkmousepositiontimer.Start();
            }
            else
            {
                checkmousepositiontimer.Stop();
                // System.Diagnostics.Debug.WriteLine("UCF Timer off");
            }

            // foreach (Control c in Controls) System.Diagnostics.Debug.WriteLine($"UCF Control {c.Name} {c.Bounds} {c.BackColor}");
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

        private void label_title_Click(object sender, EventArgs e)
        {
            ShowSystemMenu(label_title.PointToScreen(new Point(0, label_title.Height)));

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

                        if ( inpanelshow)     // if in panel show now, and was transparent, change its mode
                            UpdateTransparencyControls();
                    }

                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine(Environment.TickCount + "Out of area");

                    if (inpanelshow)
                    {
                        inpanelshow = false;
                        UpdateTransparencyControls();
                    }
                }
            }
        }


        #endregion

        #region Resizing

        const int UCPaddingWidth = 3;
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

        #region Others


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
        private bool lasttransparentmodereported;   // what we have reported for the mode change to minimise TransparencyModeChanged

        private Timer checkmousepositiontimer = new Timer();      // timer to monitor for entry into form when transparent.. only sane way in forms
        private bool deftopmost;

        private DirectInputDevices.InputDeviceKeyboard idk;     // used to sniff in transparency mode

    }
}
