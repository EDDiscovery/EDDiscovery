/*
 * Copyright © 2015 - 2022 EDDiscovery development team
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

namespace EDDiscovery.Forms
{
    // inherit and it will save the form position for you. You supply the RestoreFormPositionRegKey in your constructor.
    public partial class DraggableFormPos : ExtendedControls.DraggableForm
    {
        protected bool FormIsMaximised { get { return formMax; } }                  
        protected string RestoreFormPositionRegKey { private get; set; } = null;        // must be set in constructor of derived class
        protected bool FormShownOnce { get; private set; } = false;                     // its been up once
        public bool IsTemporaryResized { get; private set; } = false;                   // We are in a resize..

        public DraggableFormPos()
        { 
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        #region Temp Resizing Ask for a different area but don't save it

        public void RequestTemporaryResize(Size w)                  // Size w is the client area wanted inside the window (in total)
        {
            if (!IsTemporaryResized)
            {
                IsTemporaryResized = true;      // disable resize saving..

                int widthoutsideclient = (Bounds.Size.Width - ClientRectangle.Width);       // add on bounds are
                int heightoutsideclient = (Bounds.Size.Height - ClientRectangle.Height);
                this.Size = new Size(w.Width + widthoutsideclient, w.Height + heightoutsideclient); // will cause a resize but will be ignored
            }
        }

        public void RevertToNormalSize()
        {
            if (IsTemporaryResized)
            {
                this.Size = new Size(formWidth, formHeight);        // restore to saved defaults.. will cause a resize event but will be ignored.
                IsTemporaryResized = false;         // and not resized..
            }
        }

        #endregion

        #region Implementation

        private bool formMax;
        private int formWidth;
        private int formHeight;
        private int formTop;
        private int formLeft;

        private void RestoreFormPosition()
        {
            if (this.DesignMode || !EDDOptions.Instanced)       // this stops the designer barfing.
                return;

            var top = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(RestoreFormPositionRegKey+"Top", int.MinValue);

            if (top != int.MinValue && EDDOptions.Instance.NoWindowReposition == false)
            {
                var left = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(RestoreFormPositionRegKey+"Left", 0);
                var height = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(RestoreFormPositionRegKey+"Height", 800);
                var width = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(RestoreFormPositionRegKey+"Width", 800);

                //System.Diagnostics.Debug.WriteLine("Restore {0},{1} {2},{3} {4}", left, top, width, height, RestoreFormPositionRegKey);

                // Adjust so window fits on screen; just in case user unplugged a monitor or something

                var screen = SystemInformation.VirtualScreen;
                if (height > screen.Height) height = screen.Height;
                if (top + height > screen.Height + screen.Top) top = screen.Height + screen.Top - height;
                if (width > screen.Width) width = screen.Width;
                if (left + width > screen.Width + screen.Left) left = screen.Width + screen.Left - width;
                if (top < screen.Top) top = screen.Top;
                if (left < screen.Left) left = screen.Left;

                //System.Diagnostics.Debug.WriteLine("Bounded {0},{1} {2},{3}", left, top, width, height);

                this.Top = top;
                this.Left = left;
                this.Height = height;
                this.Width = width;

                this.CreateParams.X = this.Left;
                this.CreateParams.Y = this.Top;
                this.StartPosition = FormStartPosition.Manual;

                formMax = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(RestoreFormPositionRegKey+"Max", false);

                if (formMax)
                    this.WindowState = FormWindowState.Maximized;
            }

            formLeft = Left;
            formTop = Top;
            formHeight = Height;
            formWidth = Width;
        }

        private void SaveFormPosition()
        {
            //System.Diagnostics.Debug.WriteLine("Store {0},{1} {2},{3} {4}", formLeft, formTop, formWidth, formHeight, RestoreFormPositionRegKey);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(RestoreFormPositionRegKey+"Max", formMax);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(RestoreFormPositionRegKey+"Width", formWidth);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(RestoreFormPositionRegKey+"Height", formHeight);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(RestoreFormPositionRegKey+"Top", formTop);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(RestoreFormPositionRegKey+"Left", formLeft);
        }

        private void RecordFormPosition()     // HOOK into Resize (for Max) AND ResizeEnd (for drag and size)
        {
            if (FormWindowState.Maximized == WindowState)       // if maximized, note..
            {
                formMax = true;
            }
            else if (FormWindowState.Normal == WindowState) // if normal, size size..  If minimise, don't save
            {
                formLeft = this.Left;
                formTop = this.Top;
                formWidth = this.Width;
                formHeight = this.Height;
                formMax = false;
            }
        }

        protected override void OnLoad(EventArgs e)         // on load we restore
        {
            RestoreFormPosition();

            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)        // some classes need this to screen out stuff as well as us
        {
            FormShownOnce = true;
            base.OnShown(e);
        }

        protected override void OnResize(EventArgs e)       // this is a resize or a max..
        {
            base.OnResize(e);

            if (FormShownOnce && !IsTemporaryResized) // to make sure that we have been shown and we are saving size
                RecordFormPosition();
        }

        protected override void OnResizeEnd(EventArgs e)    // this is a resize or a location change.
        {
            base.OnResizeEnd(e);

            if (FormShownOnce && !IsTemporaryResized) // to make sure that we have been shown and we are saving size
                RecordFormPosition();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (!e.Cancel)
                SaveFormPosition();
        }

        #endregion
    }
}
