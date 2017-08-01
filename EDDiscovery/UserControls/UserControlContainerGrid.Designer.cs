namespace EDDiscovery.UserControls
{
    partial class UserControlContainerGrid
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.rollUpPanel1 = new ExtendedControls.RollUpPanel();
            this.comboBoxGridSelector = new ExtendedControls.ComboBoxCustom();
            this.panelPlayfield = new System.Windows.Forms.Panel();
            this.buttonExtDelete = new ExtendedControls.ButtonExt();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rollUpPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rollUpPanel1
            // 
            this.rollUpPanel1.Controls.Add(this.buttonExtDelete);
            this.rollUpPanel1.Controls.Add(this.comboBoxGridSelector);
            this.rollUpPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.rollUpPanel1.HiddenMarkerWidth = 0;
            this.rollUpPanel1.Location = new System.Drawing.Point(0, 0);
            this.rollUpPanel1.Name = "rollUpPanel1";
            this.rollUpPanel1.PinState = true;
            this.rollUpPanel1.RolledUpHeight = 5;
            this.rollUpPanel1.RollPixelStep = 5;
            this.rollUpPanel1.RollUpDelay = 1000;
            this.rollUpPanel1.Size = new System.Drawing.Size(912, 32);
            this.rollUpPanel1.TabIndex = 1;
            this.rollUpPanel1.UnrolledHeight = 32;
            this.rollUpPanel1.UnrollHoverDelay = 1000;
            // 
            // comboBoxGridSelector
            // 
            this.comboBoxGridSelector.ArrowWidth = 1;
            this.comboBoxGridSelector.BorderColor = System.Drawing.Color.White;
            this.comboBoxGridSelector.ButtonColorScaling = 0.5F;
            this.comboBoxGridSelector.DataSource = null;
            this.comboBoxGridSelector.DisplayMember = "";
            this.comboBoxGridSelector.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxGridSelector.DropDownHeight = 106;
            this.comboBoxGridSelector.DropDownWidth = 164;
            this.comboBoxGridSelector.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxGridSelector.ItemHeight = 13;
            this.comboBoxGridSelector.Location = new System.Drawing.Point(4, 4);
            this.comboBoxGridSelector.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxGridSelector.Name = "comboBoxGridSelector";
            this.comboBoxGridSelector.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxGridSelector.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxGridSelector.ScrollBarWidth = 16;
            this.comboBoxGridSelector.SelectedIndex = -1;
            this.comboBoxGridSelector.SelectedItem = null;
            this.comboBoxGridSelector.SelectedValue = null;
            this.comboBoxGridSelector.Size = new System.Drawing.Size(164, 21);
            this.comboBoxGridSelector.TabIndex = 2;
            this.comboBoxGridSelector.TabStop = false;
            this.toolTip1.SetToolTip(this.comboBoxGridSelector, "Select Panel to open");
            this.comboBoxGridSelector.ValueMember = "";
            this.comboBoxGridSelector.SelectedIndexChanged += new System.EventHandler(this.comboBoxGridSelector_SelectedIndexChanged);
            // 
            // panelPlayfield
            // 
            this.panelPlayfield.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlayfield.Location = new System.Drawing.Point(0, 32);
            this.panelPlayfield.Name = "panelPlayfield";
            this.panelPlayfield.Size = new System.Drawing.Size(912, 612);
            this.panelPlayfield.TabIndex = 2;
            // 
            // buttonExtDelete
            // 
            this.buttonExtDelete.BorderColorScaling = 1.25F;
            this.buttonExtDelete.ButtonColorScaling = 0.5F;
            this.buttonExtDelete.ButtonDisabledScaling = 0.5F;
            this.buttonExtDelete.Location = new System.Drawing.Point(185, 4);
            this.buttonExtDelete.Name = "buttonExtDelete";
            this.buttonExtDelete.Size = new System.Drawing.Size(26, 24);
            this.buttonExtDelete.TabIndex = 3;
            this.buttonExtDelete.Text = "X";
            this.toolTip1.SetToolTip(this.buttonExtDelete, "Remove selected panel");
            this.buttonExtDelete.UseVisualStyleBackColor = true;
            this.buttonExtDelete.Click += new System.EventHandler(this.buttonExtDelete_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // GridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelPlayfield);
            this.Controls.Add(this.rollUpPanel1);
            this.Name = "GridControl";
            this.Size = new System.Drawing.Size(912, 644);
            this.rollUpPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.RollUpPanel rollUpPanel1;
        private ExtendedControls.ComboBoxCustom comboBoxGridSelector;
        private System.Windows.Forms.Panel panelPlayfield;
        private ExtendedControls.ButtonExt buttonExtDelete;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
