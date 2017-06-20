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
    public class ComboBoxCustomDropdown : Form
    {
        #region Public interfaces

        #region ctors

        public ComboBoxCustomDropdown() : this(string.Empty) { }

        public ComboBoxCustomDropdown(string name)
        {
            string nullsafename = name ?? string.Empty;

            FormBorderStyle = FormBorderStyle.None;
            Name = nullsafename;
            Padding = new Padding(0);
            ShowInTaskbar = false;

            _listcontrol = new ListControlCustom();
            _listcontrol.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _listcontrol.Dock = DockStyle.Fill;
            _listcontrol.Margin = new Padding(0);
            _listcontrol.Name = nullsafename;
            _listcontrol.Visible = true;
            _listcontrol.SelectedIndexChanged += _listcontrol_SelectedIndexChanged;
            _listcontrol.KeyPressed += _listcontrol_KeyPressed;
            _listcontrol.OtherKeyPressed += _listcontrol_OtherKeyPressed;
            Controls.Add(_listcontrol);
        }

        #endregion // ctors

        #region Properties

        public Color MouseOverBackgroundColor { get { return _listcontrol.MouseOverBackgroundColor; } set { _listcontrol.MouseOverBackgroundColor = value; } }
        public int SelectedIndex { get { return _listcontrol.SelectedIndex; } set { _listcontrol.SelectedIndex = value; } }
        public Color SelectionBackColor { get { return _listcontrol.SelectionBackColor; } set { _listcontrol.SelectionBackColor = value; BackColor = value; } }
        public List<string> Items { get { return _listcontrol.Items; } set { _listcontrol.Items = value; } }
        public Color BorderColor { get { return _listcontrol.BorderColor; } set { _listcontrol.BorderColor = value; } }
        public Color ScrollBarColor { get { return _listcontrol.ScrollBarColor; } set { _listcontrol.ScrollBarColor = value; } }
        public Color ScrollBarButtonColor { get { return _listcontrol.ScrollBarButtonColor; } set { _listcontrol.ScrollBarButtonColor = value; } }
        public FlatStyle FlatStyle { get { return _listcontrol.FlatStyle; } set { _listcontrol.FlatStyle = value; } }
        public new Font Font { get { return base.Font; } set { base.Font = value; _listcontrol.Font = value; } }

        public int ItemHeight { get { return _listcontrol.ItemHeight; } set { _listcontrol.ItemHeight = value; } }

        #endregion // Properties

        #region Events

        public event EventHandler DropDown;             // ✓
        public event EventHandler SelectedIndexChanged; // ✓
        public event KeyPressEventHandler KeyPressed;   // ✓
        public event KeyEventHandler OtherKeyPressed;   // ✓

        #endregion // Events

        #endregion // Public interfaces


        #region Implementation

        #region Fields

        private ListControlCustom _listcontrol;     // ✓ (this.Controls)

        #endregion // Fields

        #region OnEvent overrides

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            Close();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            DropDown(this, e);
        }

        #endregion // OnEvent overrides

        #region Event handlers

        private void _listcontrol_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndexChanged?.Invoke(this, e);
            Close();
        }

        private void _listcontrol_KeyPressed(object sender, KeyPressEventArgs e)
        {
            KeyPressed?.Invoke(this, e);
        }

        private void _listcontrol_OtherKeyPressed(object sender, KeyEventArgs e)
        {
            OtherKeyPressed?.Invoke(this, e);
        }

        #endregion // Event handlers

        #region Methods

        protected override void Dispose(bool disposing)
        {
            DropDown = null;
            KeyPressed = null;
            OtherKeyPressed = null;
            SelectedIndexChanged = null;
            base.Dispose(disposing);
        }

        #endregion // Methods

        #endregion // Implementation
    }
}
