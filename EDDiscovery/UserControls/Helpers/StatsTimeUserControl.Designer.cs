/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using ExtendedControls;

namespace EDDiscovery.UserControls
{
    public partial class StatsTimeUserControl
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
            this.panelControls = new System.Windows.Forms.Panel();
            this.CustomDateTimePickerTo = new ExtendedControls.ExtDateTimePicker();
            this.CustomDateTimePickerFrom = new ExtendedControls.ExtDateTimePicker();
            this.checkBoxCustomStars = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomPlanets = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomGraph = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomText = new ExtendedControls.ExtCheckBox();
            this.comboBoxTimeMode = new ExtendedControls.ExtComboBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControls
            // 
            this.panelControls.Controls.Add(this.CustomDateTimePickerTo);
            this.panelControls.Controls.Add(this.CustomDateTimePickerFrom);
            this.panelControls.Controls.Add(this.checkBoxCustomStars);
            this.panelControls.Controls.Add(this.checkBoxCustomPlanets);
            this.panelControls.Controls.Add(this.checkBoxCustomGraph);
            this.panelControls.Controls.Add(this.checkBoxCustomText);
            this.panelControls.Controls.Add(this.comboBoxTimeMode);
            this.panelControls.Controls.Add(this.labelTime);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(800, 32);
            this.panelControls.TabIndex = 6;
            // 
            // CustomDateTimePickerTo
            // 
            this.CustomDateTimePickerTo.BorderColor = System.Drawing.Color.Transparent;
            this.CustomDateTimePickerTo.BorderColorScaling = 0.5F;
            this.CustomDateTimePickerTo.Checked = false;
            this.CustomDateTimePickerTo.CustomFormat = "dd MMMM yyyy";
            this.CustomDateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.CustomDateTimePickerTo.Location = new System.Drawing.Point(284, 2);
            this.CustomDateTimePickerTo.Name = "CustomDateTimePickerTo";
            this.CustomDateTimePickerTo.SelectedColor = System.Drawing.Color.Yellow;
            this.CustomDateTimePickerTo.ShowCheckBox = false;
            this.CustomDateTimePickerTo.ShowUpDown = false;
            this.CustomDateTimePickerTo.Size = new System.Drawing.Size(120, 24);
            this.CustomDateTimePickerTo.TabIndex = 8;
            this.CustomDateTimePickerTo.TextBackColor = System.Drawing.Color.DarkBlue;
            this.CustomDateTimePickerTo.Value = new System.DateTime(2017, 7, 10, 21, 10, 15, 925);
            this.CustomDateTimePickerTo.Visible = false;
            this.CustomDateTimePickerTo.ValueChanged += new System.EventHandler(this.customDateTimePickerTo_ValueChanged);
            // 
            // CustomDateTimePickerFrom
            // 
            this.CustomDateTimePickerFrom.BorderColor = System.Drawing.Color.Transparent;
            this.CustomDateTimePickerFrom.BorderColorScaling = 0.5F;
            this.CustomDateTimePickerFrom.Checked = false;
            this.CustomDateTimePickerFrom.CustomFormat = "dd MMMM yyyy";
            this.CustomDateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.CustomDateTimePickerFrom.Location = new System.Drawing.Point(159, 2);
            this.CustomDateTimePickerFrom.Name = "CustomDateTimePickerFrom";
            this.CustomDateTimePickerFrom.SelectedColor = System.Drawing.Color.Yellow;
            this.CustomDateTimePickerFrom.ShowCheckBox = false;
            this.CustomDateTimePickerFrom.ShowUpDown = false;
            this.CustomDateTimePickerFrom.Size = new System.Drawing.Size(120, 24);
            this.CustomDateTimePickerFrom.TabIndex = 7;
            this.CustomDateTimePickerFrom.TextBackColor = System.Drawing.Color.DarkBlue;
            this.CustomDateTimePickerFrom.Value = new System.DateTime(2017, 7, 10, 21, 10, 15, 925);
            this.CustomDateTimePickerFrom.Visible = false;
            this.CustomDateTimePickerFrom.ValueChanged += new System.EventHandler(this.customDateTimePickerFrom_ValueChanged);
            // 
            // checkBoxCustomStars
            // 
            this.checkBoxCustomStars.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCustomStars.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomStars.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomStars.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomStars.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxCustomStars.FontNerfReduction = 0.5F;
            this.checkBoxCustomStars.Image = global::EDDiscovery.Icons.Controls.StatsTime_Stars;
            this.checkBoxCustomStars.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomStars.Location = new System.Drawing.Point(442, 1);
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
            this.checkBoxCustomPlanets.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.checkBoxCustomPlanets.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.checkBoxCustomPlanets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxCustomPlanets.FontNerfReduction = 0.5F;
            this.checkBoxCustomPlanets.Image = global::EDDiscovery.Icons.Controls.StatsTime_Planets;
            this.checkBoxCustomPlanets.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomPlanets.Location = new System.Drawing.Point(410, 1);
            this.checkBoxCustomPlanets.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomPlanets.Name = "checkBoxCustomPlanets";
            this.checkBoxCustomPlanets.Size = new System.Drawing.Size(26, 26);
            this.checkBoxCustomPlanets.TabIndex = 5;
            this.checkBoxCustomPlanets.TickBoxReductionSize = 10;
            this.checkBoxCustomPlanets.UseVisualStyleBackColor = true;
            this.checkBoxCustomPlanets.Visible = false;
            this.checkBoxCustomPlanets.CheckedChanged += new System.EventHandler(this.checkBoxCustomPlanets_CheckedChanged);
            // 
            // checkBoxCustomGraph
            // 
            this.checkBoxCustomGraph.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCustomGraph.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomGraph.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomGraph.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomGraph.FontNerfReduction = 0.5F;
            this.checkBoxCustomGraph.Image = global::EDDiscovery.Icons.Controls.StatsTime_Graph;
            this.checkBoxCustomGraph.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomGraph.Location = new System.Drawing.Point(510, 1);
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
            this.checkBoxCustomText.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomText.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomText.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomText.FontNerfReduction = 0.5F;
            this.checkBoxCustomText.Image = global::EDDiscovery.Icons.Controls.StatsTime_Text;
            this.checkBoxCustomText.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomText.Location = new System.Drawing.Point(478, 1);
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
            this.comboBoxTimeMode.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxTimeMode.DisplayMember = "";
            this.comboBoxTimeMode.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxTimeMode.DropDownHeight = 200;
            this.comboBoxTimeMode.DropDownWidth = 1;
            this.comboBoxTimeMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxTimeMode.ItemHeight = 13;
            this.comboBoxTimeMode.Location = new System.Drawing.Point(53, 4);
            this.comboBoxTimeMode.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxTimeMode.Name = "comboBoxTimeMode";
            this.comboBoxTimeMode.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxTimeMode.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxTimeMode.ScrollBarWidth = 16;
            this.comboBoxTimeMode.SelectedIndex = -1;
            this.comboBoxTimeMode.SelectedItem = null;
            this.comboBoxTimeMode.SelectedValue = null;
            this.comboBoxTimeMode.Size = new System.Drawing.Size(100, 21);
            this.comboBoxTimeMode.TabIndex = 1;
            this.comboBoxTimeMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // StatsTimeUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControls);
            this.Name = "StatsTimeUserControl";
            this.Size = new System.Drawing.Size(800, 32);
            this.Load += new System.EventHandler(this.UserControlStatsTime_Load);
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControls;
        internal ExtendedControls.ExtComboBox comboBoxTimeMode;
        private System.Windows.Forms.Label labelTime;
        private ExtendedControls.ExtCheckBox checkBoxCustomStars;
        private ExtendedControls.ExtCheckBox checkBoxCustomPlanets;
        internal ExtendedControls.ExtDateTimePicker CustomDateTimePickerTo;
        internal ExtendedControls.ExtDateTimePicker CustomDateTimePickerFrom;
        private ExtCheckBox checkBoxCustomGraph;
        private ExtCheckBox checkBoxCustomText;
    }
}
