namespace EDDiscovery.UserControls
{
    partial class UserControlStatsTime
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlStatsTime));
            this.panelControls = new System.Windows.Forms.Panel();
            this.checkBoxCustomGraph = new ExtendedControls.CheckBoxCustom();
            this.checkBoxCustomText = new ExtendedControls.CheckBoxCustom();
            this.comboBoxTimeMode = new ExtendedControls.ComboBoxCustom();
            this.labelTime = new System.Windows.Forms.Label();
            this.checkBoxCustomStars = new ExtendedControls.CheckBoxCustom();
            this.checkBoxCustomPlanets = new ExtendedControls.CheckBoxCustom();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControls
            // 
            this.panelControls.Controls.Add(this.checkBoxCustomStars);
            this.panelControls.Controls.Add(this.checkBoxCustomPlanets);
            this.panelControls.Controls.Add(this.checkBoxCustomGraph);
            this.panelControls.Controls.Add(this.checkBoxCustomText);
            this.panelControls.Controls.Add(this.comboBoxTimeMode);
            this.panelControls.Controls.Add(this.labelTime);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(800, 27);
            this.panelControls.TabIndex = 6;
            this.panelControls.Paint += new System.Windows.Forms.PaintEventHandler(this.panelControls_Paint);
            // 
            // checkBoxCustomGraph
            // 
            this.checkBoxCustomGraph.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCustomGraph.AutoSize = true;
            this.checkBoxCustomGraph.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomGraph.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomGraph.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomGraph.FontNerfReduction = 0.5F;
            this.checkBoxCustomGraph.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxCustomGraph.Image")));
            this.checkBoxCustomGraph.Location = new System.Drawing.Point(761, 0);
            this.checkBoxCustomGraph.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomGraph.Name = "checkBoxCustomGraph";
            this.checkBoxCustomGraph.Size = new System.Drawing.Size(26, 26);
            this.checkBoxCustomGraph.TabIndex = 4;
            this.checkBoxCustomGraph.TickBoxReductionSize = 10;
            this.checkBoxCustomGraph.UseVisualStyleBackColor = true;
            this.checkBoxCustomGraph.Visible = false;
            this.checkBoxCustomGraph.CheckedChanged += new System.EventHandler(this.checkBoxCustomGraph_CheckedChanged);
            // 
            // checkBoxCustomText
            // 
            this.checkBoxCustomText.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCustomText.AutoSize = true;
            this.checkBoxCustomText.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomText.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomText.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomText.FontNerfReduction = 0.5F;
            this.checkBoxCustomText.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxCustomText.Image")));
            this.checkBoxCustomText.Location = new System.Drawing.Point(729, 0);
            this.checkBoxCustomText.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomText.Name = "checkBoxCustomText";
            this.checkBoxCustomText.Size = new System.Drawing.Size(26, 26);
            this.checkBoxCustomText.TabIndex = 3;
            this.checkBoxCustomText.TickBoxReductionSize = 10;
            this.checkBoxCustomText.UseVisualStyleBackColor = true;
            this.checkBoxCustomText.Visible = false;
            this.checkBoxCustomText.CheckedChanged += new System.EventHandler(this.checkBoxCustomText_CheckedChanged);
            // 
            // comboBoxTimeMode
            // 
            this.comboBoxTimeMode.ArrowWidth = 1;
            this.comboBoxTimeMode.BorderColor = System.Drawing.Color.Red;
            this.comboBoxTimeMode.ButtonColorScaling = 0.5F;
            this.comboBoxTimeMode.DataSource = null;
            this.comboBoxTimeMode.DisplayMember = "";
            this.comboBoxTimeMode.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxTimeMode.DropDownHeight = 200;
            this.comboBoxTimeMode.DropDownWidth = 1;
            this.comboBoxTimeMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxTimeMode.ItemHeight = 13;
            this.comboBoxTimeMode.Location = new System.Drawing.Point(53, 3);
            this.comboBoxTimeMode.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxTimeMode.Name = "comboBoxTimeMode";
            this.comboBoxTimeMode.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxTimeMode.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxTimeMode.ScrollBarWidth = 16;
            this.comboBoxTimeMode.SelectedIndex = -1;
            this.comboBoxTimeMode.SelectedItem = null;
            this.comboBoxTimeMode.SelectedValue = null;
            this.comboBoxTimeMode.Size = new System.Drawing.Size(100, 22);
            this.comboBoxTimeMode.TabIndex = 1;
            this.comboBoxTimeMode.ValueMember = "";
            this.comboBoxTimeMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxTimeMode_SelectedIndexChanged);
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(7, 6);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(30, 13);
            this.labelTime.TabIndex = 2;
            this.labelTime.Text = "Time";
            // 
            // checkBoxCustomStars
            // 
            this.checkBoxCustomStars.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCustomStars.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomStars.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomStars.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomStars.FontNerfReduction = 0.5F;
            this.checkBoxCustomStars.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxCustomStars.Image")));
            this.checkBoxCustomStars.Location = new System.Drawing.Point(191, 0);
            this.checkBoxCustomStars.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomStars.Name = "checkBoxCustomStars";
            this.checkBoxCustomStars.Size = new System.Drawing.Size(26, 26);
            this.checkBoxCustomStars.TabIndex = 6;
            this.checkBoxCustomStars.TickBoxReductionSize = 10;
            this.checkBoxCustomStars.UseVisualStyleBackColor = true;
            this.checkBoxCustomStars.Visible = false;
            this.checkBoxCustomStars.CheckedChanged += new System.EventHandler(this.checkBoxCustomStars_CheckedChanged);
            // 
            // checkBoxCustomPlanets
            // 
            this.checkBoxCustomPlanets.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCustomPlanets.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCustomPlanets.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomPlanets.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomPlanets.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomPlanets.FontNerfReduction = 0.5F;
            this.checkBoxCustomPlanets.Image = ((System.Drawing.Image)(resources.GetObject("checkBoxCustomPlanets.Image")));
            this.checkBoxCustomPlanets.Location = new System.Drawing.Point(159, 0);
            this.checkBoxCustomPlanets.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomPlanets.Name = "checkBoxCustomPlanets";
            this.checkBoxCustomPlanets.Size = new System.Drawing.Size(26, 26);
            this.checkBoxCustomPlanets.TabIndex = 5;
            this.checkBoxCustomPlanets.TickBoxReductionSize = 10;
            this.checkBoxCustomPlanets.UseVisualStyleBackColor = true;
            this.checkBoxCustomPlanets.Visible = false;
            this.checkBoxCustomPlanets.CheckedChanged += new System.EventHandler(this.checkBoxCustomPlanets_CheckedChanged);
            // 
            // UserControlStatsTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControls);
            this.Name = "UserControlStatsTime";
            this.Size = new System.Drawing.Size(800, 27);
            this.Load += new System.EventHandler(this.UserControlStatsTime_Load);
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControls;
        internal ExtendedControls.ComboBoxCustom comboBoxTimeMode;
        private System.Windows.Forms.Label labelTime;
        private ExtendedControls.CheckBoxCustom checkBoxCustomGraph;
        private ExtendedControls.CheckBoxCustom checkBoxCustomText;
        private ExtendedControls.CheckBoxCustom checkBoxCustomStars;
        private ExtendedControls.CheckBoxCustom checkBoxCustomPlanets;
    }
}
