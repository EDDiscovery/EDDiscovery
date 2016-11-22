namespace EDDiscovery.Forms
{
    partial class FindMaterialsForm
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
            this.buttonSearch = new ExtendedControls.ButtonExt();
            this.labelExt1 = new ExtendedControls.LabelExt();
            this.comboBoxMaxLy = new ExtendedControls.ComboBoxCustom();
            this.textBoxBorder1 = new ExtendedControls.TextBoxBorder();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSearch
            // 
            this.buttonSearch.BorderColorScaling = 1.25F;
            this.buttonSearch.ButtonColorScaling = 0.5F;
            this.buttonSearch.ButtonDisabledScaling = 0.5F;
            this.buttonSearch.Location = new System.Drawing.Point(134, 28);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 3;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // labelExt1
            // 
            this.labelExt1.AutoSize = true;
            this.labelExt1.Location = new System.Drawing.Point(9, 9);
            this.labelExt1.Name = "labelExt1";
            this.labelExt1.Size = new System.Drawing.Size(84, 13);
            this.labelExt1.TabIndex = 2;
            this.labelExt1.Text = "Max ly to search";
            // 
            // comboBoxMaxLy
            // 
            this.comboBoxMaxLy.ArrowWidth = 1;
            this.comboBoxMaxLy.BorderColor = System.Drawing.Color.White;
            this.comboBoxMaxLy.ButtonColorScaling = 0.5F;
            this.comboBoxMaxLy.DataSource = null;
            this.comboBoxMaxLy.DisplayMember = "";
            this.comboBoxMaxLy.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxMaxLy.DropDownHeight = 106;
            this.comboBoxMaxLy.DropDownWidth = 75;
            this.comboBoxMaxLy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxMaxLy.ItemHeight = 13;
            this.comboBoxMaxLy.Location = new System.Drawing.Point(12, 28);
            this.comboBoxMaxLy.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxMaxLy.Name = "comboBoxMaxLy";
            this.comboBoxMaxLy.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxMaxLy.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxMaxLy.ScrollBarWidth = 16;
            this.comboBoxMaxLy.SelectedIndex = -1;
            this.comboBoxMaxLy.SelectedItem = null;
            this.comboBoxMaxLy.SelectedValue = null;
            this.comboBoxMaxLy.Size = new System.Drawing.Size(75, 23);
            this.comboBoxMaxLy.TabIndex = 1;
            this.comboBoxMaxLy.Text = "comboBoxCustom1";
            this.comboBoxMaxLy.ValueMember = "";
            // 
            // textBoxBorder1
            // 
            this.textBoxBorder1.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorder1.BorderColorScaling = 0.5F;
            this.textBoxBorder1.Location = new System.Drawing.Point(283, 12);
            this.textBoxBorder1.Multiline = true;
            this.textBoxBorder1.Name = "textBoxBorder1";
            this.textBoxBorder1.Size = new System.Drawing.Size(302, 49);
            this.textBoxBorder1.TabIndex = 0;
            this.textBoxBorder1.Text = "J1 = Germanium + 2*Vanadium\r\nJ2 = Germanium + Vanadium + 2*Cadmium + Niobium\r\nJ3 " +
    "= Arsenic + 3*Niobium + Polonium + Yttrium";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 90);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(573, 301);
            this.dataGridView1.TabIndex = 4;
            // 
            // FindMaterialsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 403);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.labelExt1);
            this.Controls.Add(this.comboBoxMaxLy);
            this.Controls.Add(this.textBoxBorder1);
            this.Name = "FindMaterialsForm";
            this.Text = "FindJumpBoostForm";
            this.Load += new System.EventHandler(this.FindMaterialsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.TextBoxBorder textBoxBorder1;
        private ExtendedControls.ComboBoxCustom comboBoxMaxLy;
        private ExtendedControls.LabelExt labelExt1;
        private ExtendedControls.ButtonExt buttonSearch;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}