/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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

using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    // inherit and it will save the form position for you

    public partial class DraggableFormPos : ExtendedControls.DraggableForm
    {
        protected bool FormIsMaximised { get { return formMax; } }
        protected string RestoreFormPositionRegKey { private get; set; } = null;
        protected bool FormShownOnce { get; private set; } = false;
        public bool IsTemporaryResized { get; private set; } = false;

        public DraggableFormPos()
        { 
            InitializeComponent();
        }

        // Ask for a bigger area but don't save it

        public void RequestTemporaryResize(Size w)                  // Size w is the client area above
        {
            if (!IsTemporaryResized)
            {
                IsTemporaryResized = true;      // disable resize saving..

                int widthoutsideclient = (Bounds.Size.Width - ClientRectangle.Width);
                int heightoutsideclient = (Bounds.Size.Height - ClientRectangle.Height);
                int heightlosttoothercontrols = 0;// UserControl.Location.Y + statusStripBottom.Height; // and the area used by the other bits of the window outside the user control
                this.Size = new Size(w.Width + widthoutsideclient, w.Height + heightlosttoothercontrols + heightoutsideclient);
            }
        }

        public void RevertToNormalSize()
        {
            if (IsTemporaryResized)
            {
                this.Size = new Size(formWidth, formHeight);        // restore to saved defaults
                IsTemporaryResized = false;         // and not resized..
            }
        }

        private bool formMax;
        private int formWidth;
        private int formHeight;
        private int formTop;
        private int formLeft;

        private void RestoreFormPosition()
        {
            var top = SQLiteDBClass.GetSettingInt(RestoreFormPositionRegKey+"Top", -999);

            if (top != -999 && EDDOptions.Instance.NoWindowReposition == false)
            {
                var left = SQLiteDBClass.GetSettingInt(RestoreFormPositionRegKey+"Left", 0);
                var height = SQLiteDBClass.GetSettingInt(RestoreFormPositionRegKey+"Height", 800);
                var width = SQLiteDBClass.GetSettingInt(RestoreFormPositionRegKey+"Width", 800);

                System.Diagnostics.Debug.WriteLine("Restore {0},{1} {2},{3}", left, top, width, height);

                // Adjust so window fits on screen; just in case user unplugged a monitor or something

                var screen = SystemInformation.VirtualScreen;
                if (height > screen.Height) height = screen.Height;
                if (top + height > screen.Height + screen.Top) top = screen.Height + screen.Top - height;
                if (width > screen.Width) width = screen.Width;
                if (left + width > screen.Width + screen.Left) left = screen.Width + screen.Left - width;
                if (top < screen.Top) top = screen.Top;
                if (left < screen.Left) left = screen.Left;

                System.Diagnostics.Debug.WriteLine("Bounded {0},{1} {2},{3}", left, top, width, height);

                this.Top = top;
                this.Left = left;
                this.Height = height;
                this.Width = width;

                this.CreateParams.X = this.Left;
                this.CreateParams.Y = this.Top;
                this.StartPosition = FormStartPosition.Manual;

                formMax = SQLiteDBClass.GetSettingBool(RestoreFormPositionRegKey+"Max", false);

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
            System.Diagnostics.Debug.WriteLine("Store {0},{1} {2},{3}", formLeft, formTop, formWidth, formHeight);
            SQLiteDBClass.PutSettingBool(RestoreFormPositionRegKey+"Max", formMax);
            SQLiteDBClass.PutSettingInt(RestoreFormPositionRegKey+"Width", formWidth);
            SQLiteDBClass.PutSettingInt(RestoreFormPositionRegKey+"Height", formHeight);
            SQLiteDBClass.PutSettingInt(RestoreFormPositionRegKey+"Top", formTop);
            SQLiteDBClass.PutSettingInt(RestoreFormPositionRegKey+"Left", formLeft);
        }

        private void RecordFormPosition()     // HOOK into Resize (for Max) AND ResizeEnd (for drag and size)
        {
            System.Diagnostics.Debug.WriteLine("Resize Event {0} {1},{2} {3},{4}", WindowState, Left, Top, Width, Height);

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

        private void DraggableFormPos_Load(object sender, EventArgs e)      // on load we restore
        {
            RestoreFormPosition();
        }

        private void DraggableFormPos_Shown(object sender, EventArgs e)     // some classes need this to screen out stuff as well as us
        {
            FormShownOnce = true;
        }

        private void DraggableFormPos_Resize(object sender, EventArgs e)        // this is a resize or a max..
        {
            if (FormShownOnce && !IsTemporaryResized) // to make sure that we have been shown and we are saving size
                RecordFormPosition();
        }

        private void DraggableFormPos_ResizeEnd(object sender, EventArgs e)     // this is a resize of a location change.
        {
            if (FormShownOnce && !IsTemporaryResized) // to make sure that we have been shown and we are saving size
                RecordFormPosition();
        }

        private void DraggableFormPos_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFormPosition();     // even if its cancelled, still s
        }

        
    }
}
