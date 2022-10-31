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
            this.checkBoxCustomStars = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomPlanets = new ExtendedControls.ExtCheckBox();
            this.comboBoxTimeMode = new ExtendedControls.ExtComboBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxCustomStars
            // 
            this.checkBoxCustomStars.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCustomStars.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomStars.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomStars.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomStars.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomStars.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxCustomStars.Image = global::EDDiscovery.Icons.Controls.Stars;
            this.checkBoxCustomStars.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomStars.ImageIndeterminate = null;
            this.checkBoxCustomStars.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomStars.ImageUnchecked = null;
            this.checkBoxCustomStars.Location = new System.Drawing.Point(140, 1);
            this.checkBoxCustomStars.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.checkBoxCustomStars.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomStars.Name = "checkBoxCustomStars";
            this.checkBoxCustomStars.Size = new System.Drawing.Size(24, 24);
            this.checkBoxCustomStars.TabIndex = 6;
            this.checkBoxCustomStars.TickBoxReductionRatio = 0.75F;
            this.checkBoxCustomStars.UseVisualStyleBackColor = true;
            this.checkBoxCustomStars.Visible = false;
            this.checkBoxCustomStars.CheckedChanged += new System.EventHandler(this.checkBoxCustomStars_CheckedChanged);
            // 
            // checkBoxCustomPlanets
            // 
            this.checkBoxCustomPlanets.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCustomPlanets.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCustomPlanets.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomPlanets.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomPlanets.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomPlanets.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomPlanets.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.checkBoxCustomPlanets.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.checkBoxCustomPlanets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxCustomPlanets.Image = global::EDDiscovery.Icons.Controls.Planets;
            this.checkBoxCustomPlanets.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomPlanets.ImageIndeterminate = null;
            this.checkBoxCustomPlanets.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomPlanets.ImageUnchecked = null;
            this.checkBoxCustomPlanets.Location = new System.Drawing.Point(108, 1);
            this.checkBoxCustomPlanets.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.checkBoxCustomPlanets.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomPlanets.Name = "checkBoxCustomPlanets";
            this.checkBoxCustomPlanets.Size = new System.Drawing.Size(24, 24);
            this.checkBoxCustomPlanets.TabIndex = 5;
            this.checkBoxCustomPlanets.TickBoxReductionRatio = 0.75F;
            this.checkBoxCustomPlanets.UseVisualStyleBackColor = true;
            this.checkBoxCustomPlanets.Visible = false;
            this.checkBoxCustomPlanets.CheckedChanged += new System.EventHandler(this.checkBoxCustomPlanets_CheckedChanged);
            // 
            // comboBoxTimeMode
            // 
            this.comboBoxTimeMode.BorderColor = System.Drawing.Color.Red;
            this.comboBoxTimeMode.ButtonColorScaling = 0.5F;
            this.comboBoxTimeMode.DataSource = null;
            this.comboBoxTimeMode.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxTimeMode.DisplayMember = "";
            this.comboBoxTimeMode.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxTimeMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxTimeMode.Location = new System.Drawing.Point(0, 4);
            this.comboBoxTimeMode.Margin = new System.Windows.Forms.Padding(0, 4, 8, 1);
            this.comboBoxTimeMode.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxTimeMode.Name = "comboBoxTimeMode";
            this.comboBoxTimeMode.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxTimeMode.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxTimeMode.SelectedIndex = -1;
            this.comboBoxTimeMode.SelectedItem = null;
            this.comboBoxTimeMode.SelectedValue = null;
            this.comboBoxTimeMode.Size = new System.Drawing.Size(100, 21);
            this.comboBoxTimeMode.TabIndex = 1;
            this.comboBoxTimeMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxTimeMode.ValueMember = "";
            this.comboBoxTimeMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxTimeMode_SelectedIndexChanged);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.comboBoxTimeMode);
            this.flowLayoutPanel2.Controls.Add(this.checkBoxCustomPlanets);
            this.flowLayoutPanel2.Controls.Add(this.checkBoxCustomStars);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(800, 32);
            this.flowLayoutPanel2.TabIndex = 9;
            // 
            // StatsTimeUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel2);
            this.Name = "StatsTimeUserControl";
            this.Size = new System.Drawing.Size(800, 32);
            this.Load += new System.EventHandler(this.UserControlStatsTime_Load);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal ExtendedControls.ExtComboBox comboBoxTimeMode;
        private ExtendedControls.ExtCheckBox checkBoxCustomStars;
        private ExtendedControls.ExtCheckBox checkBoxCustomPlanets;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    }
}
