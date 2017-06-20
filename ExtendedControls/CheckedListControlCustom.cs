/*
 * Copyright © 2016 EDDiscovery development team
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class CheckedListControlCustom : Form
    {
        #region Public interfaces

        #region ctors

        public CheckedListControlCustom()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            AutoSize = false;
            Padding = new Padding(0);

            clb.Visible = true;
            clb.CheckOnClick = true;
            clb.BackColor = Color.Orange;
            Controls.Add(clb);

            clb.ItemCheck += Clb_ItemCheck;
            IsOpen = true;
        }

        #endregion // ctors

        #region Properties

        public CheckedListBox.ObjectCollection Items { get { return clb.Items; } }

        public bool IsOpen { get; set; } = false;

        #endregion // Properties

        #region Events

        public event ItemCheckEventHandler CheckedChanged;

        #endregion // Events

        #region Methods

        public void PositionSize(Point p, Size s)
        {
            SetPosition = p;
            SetSize = s;
        }

        public void PositionBelow(Control b, Size s)
        {
            Point p = b.PointToScreen(new Point(0, b.Size.Height));
            SetPosition = p;
            SetSize = s;
        }

        public void SetColour(Color backcolour, Color textc)
        {
            BackColor = backcolour;
            clb.BackColor = backcolour;
            clb.ForeColor = textc;
        }

        public void SetChecked(string value, int ignore = 0)
        {
            ignorechangeevent = true;
            string[] list = value.Split(';');

            for (int i = ignore; i < clb.Items.Count; i++)
            {
                if (list.Contains(clb.Items[i]) || value.Equals("All"))
                    clb.SetItemChecked(i, true);
            }
            ignorechangeevent = false;
        }

        public void SetChecked(bool c, int ignore = 0, int count = 0)
        {
            ignorechangeevent = true;
            if (count == 0)
                count = clb.Items.Count - ignore;

            for (int i = ignore; count-- > 0; i++)
                clb.SetItemChecked(i, c);
            ignorechangeevent = false;
        }

        public string GetChecked(int ignore = 0)
        {
            string ret = "";

            int total = 0;
            for (int i = ignore; i < clb.Items.Count; i++)
            {
                if (clb.GetItemCheckState(i) == CheckState.Checked)
                {
                    ret += clb.Items[i] + ";";
                    total++;
                }
            }

            if (total == clb.Items.Count - ignore)
                ret = "All";
            if (ret.Length == 0)
                ret = "None";

            return ret;
        }

        #endregion // Methods

        #endregion // Public interfaces


        #region Implementation

        #region Fields

        private CheckedListBox clb = new CheckedListBox();  // ✓ (this.Controls)

        private Point SetPosition;
        private Size SetSize;
        private bool ignorechangeevent = false;

        #endregion // Fields

        #region OnEvent overrides & event dispatchers

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Location = SetPosition;
            Size = SetSize;
            clb.Size = SetSize;
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            Close();
            IsOpen = false;
        }


        protected virtual void OnCheckedChanged(ItemCheckEventArgs args)
        {
            CheckedChanged?.Invoke(this, args);
        }

        #endregion // OnEvent overrides & event dispatchers

        #region Event handlers

        private void Clb_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!ignorechangeevent)
                OnCheckedChanged(e);
        }

        #endregion // Event handlers

        #endregion // Implementation
    }
}
