namespace DialogTest
{
    partial class TestSelectionPanel
    { 
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestSelectionPanel));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panelSelectionList1 = new ExtendedControls.PanelSelectionList();
            this.autoCompleteTextBox1 = new ExtendedControls.AutoCompleteTextBox();
            this.comboBoxCustom1 = new ExtendedControls.ComboBoxCustom();
            this.SuspendLayout();
            // 
            // panelSelectionList1
            // 
            this.panelSelectionList1.BorderColor = System.Drawing.Color.DarkRed;
            this.panelSelectionList1.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("panelSelectionList1.Items")));
            this.panelSelectionList1.Location = new System.Drawing.Point(97, 30);
            this.panelSelectionList1.Name = "panelSelectionList1";
            this.panelSelectionList1.SelectionMarkColor = System.Drawing.Color.Blue;
            this.panelSelectionList1.SelectionSize = 8;
            this.panelSelectionList1.Size = new System.Drawing.Size(281, 78);
            this.panelSelectionList1.TabIndex = 0;
            // 
            // autoCompleteTextBox1
            // 
            this.autoCompleteTextBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.autoCompleteTextBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.autoCompleteTextBox1.BorderColor = System.Drawing.Color.Transparent;
            this.autoCompleteTextBox1.BorderColorScaling = 0.5F;
            this.autoCompleteTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.autoCompleteTextBox1.ControlBackground = System.Drawing.SystemColors.Control;
            this.autoCompleteTextBox1.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.autoCompleteTextBox1.DropDownBorderColor = System.Drawing.Color.Green;
            this.autoCompleteTextBox1.DropDownHeight = 200;
            this.autoCompleteTextBox1.DropDownItemHeight = 20;
            this.autoCompleteTextBox1.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.autoCompleteTextBox1.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.autoCompleteTextBox1.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.autoCompleteTextBox1.DropDownWidth = 0;
            this.autoCompleteTextBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.autoCompleteTextBox1.Location = new System.Drawing.Point(97, 138);
            this.autoCompleteTextBox1.Multiline = false;
            this.autoCompleteTextBox1.Name = "autoCompleteTextBox1";
            this.autoCompleteTextBox1.ReadOnly = false;
            this.autoCompleteTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.autoCompleteTextBox1.SelectionLength = 0;
            this.autoCompleteTextBox1.SelectionStart = 0;
            this.autoCompleteTextBox1.Size = new System.Drawing.Size(210, 20);
            this.autoCompleteTextBox1.TabIndex = 1;
            this.autoCompleteTextBox1.Text = "autoCompleteTextBox1";
            this.autoCompleteTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.autoCompleteTextBox1.WordWrap = true;
            // 
            // comboBoxCustom1
            // 
            this.comboBoxCustom1.ArrowWidth = 1;
            this.comboBoxCustom1.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustom1.ButtonColorScaling = 0.5F;
            this.comboBoxCustom1.DataSource = null;
            this.comboBoxCustom1.DisplayMember = "";
            this.comboBoxCustom1.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustom1.DropDownHeight = 106;
            this.comboBoxCustom1.DropDownWidth = 281;
            this.comboBoxCustom1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxCustom1.ItemHeight = 13;
            this.comboBoxCustom1.Location = new System.Drawing.Point(97, 200);
            this.comboBoxCustom1.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustom1.Name = "comboBoxCustom1";
            this.comboBoxCustom1.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustom1.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustom1.ScrollBarWidth = 16;
            this.comboBoxCustom1.SelectedIndex = -1;
            this.comboBoxCustom1.SelectedItem = null;
            this.comboBoxCustom1.SelectedValue = null;
            this.comboBoxCustom1.Size = new System.Drawing.Size(281, 21);
            this.comboBoxCustom1.TabIndex = 2;
            this.comboBoxCustom1.Text = "comboBoxCustom1";
            this.comboBoxCustom1.ValueMember = "";
            // 
            // TestSelectionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 334);
            this.Controls.Add(this.comboBoxCustom1);
            this.Controls.Add(this.autoCompleteTextBox1);
            this.Controls.Add(this.panelSelectionList1);
            this.Name = "TestSelectionPanel";
            this.Text = "TestAutoComplete";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private ExtendedControls.PanelSelectionList panelSelectionList1;
        private ExtendedControls.AutoCompleteTextBox autoCompleteTextBox1;
        private ExtendedControls.ComboBoxCustom comboBoxCustom1;
    }
}