/*
 * Copyright © 2017 EDDiscovery development team
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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using EDDiscovery2;
using System;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class AutoCompleteDGVColumn : DataGridViewColumn
    {
        public AutoCompleteDGVColumn() : base(new AutoCompleteDGVCell()) { }

        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(AutoCompleteDGVCell)))
                    throw new InvalidCastException("value is not an AutoCompleteDGVCell");
                base.CellTemplate = value;
            }
        }
    }

    public class AutoCompleteDGVCell : DataGridViewTextBoxCell
    {
        public AutoCompleteDGVCell() : base() { }

        public override object DefaultNewRowValue { get { return string.Empty; } }

        public override Type EditType { get { return typeof(AutoCompleteDGVEditControl); } }

        public override Type ValueType { get { return typeof(string); } }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            var ctl = DataGridView.EditingControl as AutoCompleteDGVEditControl;
            if (Value == null)
                ctl.Text = (string)DefaultNewRowValue;
            else
                ctl.Text = (string)Value;
        }
    }

    public class AutoCompleteDGVEditControl : AutoCompleteTextBox, IDataGridViewEditingControl
    {
        private DataGridView dataGridView;
        private bool valueChanged = false;
        private int rowIndex;

        public AutoCompleteDGVEditControl() { }

        #region IDataGridViewEditingControl properties

        public DataGridView EditingControlDataGridView { get { return dataGridView; } set { dataGridView = value; } }

        public object EditingControlFormattedValue
        {
            get { return Text; }
            set
            {
                if (value == null || string.IsNullOrEmpty((string)value))
                    Text = string.Empty;
                else
                    Text = ((string)value).Trim();
            }
        }

        public int EditingControlRowIndex { get { return rowIndex; } set { rowIndex = value; } }

        public bool EditingControlValueChanged { get { return valueChanged; } set { valueChanged = value; } }

        public Cursor EditingPanelCursor { get { return base.Cursor; } }

        public bool RepositionEditingControlOnValueChange { get { return false; } }

        #endregion

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dgvStyle)
        {
            EDDTheme.Instance.ApplyToControls(Parent);
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public bool EditingControlWantsInputKey(Keys key, bool dgvWantsInputKey)
        {
            // TODO: need to get these keys right to handle up/down and enter appropriately, along with the usual typing.
            return !dgvWantsInputKey;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            if (selectAll)
                SelectAll();
            else
                SelectionStart = SelectionLength = 0;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            valueChanged = true;
            EditingControlDataGridView?.NotifyCurrentCellDirty(true);
        }
    }
}
