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
namespace EDDiscovery.UserControls
{
    partial class UserControlColonisation
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
            this.extPanelGradientFillToolbar = new ExtendedControls.ExtPanelGradientFill();
            this.extComboBoxSystemSel = new ExtendedControls.ExtComboBox();
            this.extPanelGradientFillUCCP = new ExtendedControls.ExtPanelGradientFill();
            this.extPanelGradientFillToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // extPanelGradientFillToolbar
            // 
            this.extPanelGradientFillToolbar.ChildrenThemed = true;
            this.extPanelGradientFillToolbar.Controls.Add(this.extComboBoxSystemSel);
            this.extPanelGradientFillToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelGradientFillToolbar.FlowDirection = null;
            this.extPanelGradientFillToolbar.GradientDirection = 0F;
            this.extPanelGradientFillToolbar.Location = new System.Drawing.Point(0, 0);
            this.extPanelGradientFillToolbar.Name = "extPanelGradientFillToolbar";
            this.extPanelGradientFillToolbar.PaintTransparentColor = System.Drawing.Color.Transparent;
            this.extPanelGradientFillToolbar.Size = new System.Drawing.Size(496, 32);
            this.extPanelGradientFillToolbar.TabIndex = 0;
            this.extPanelGradientFillToolbar.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.extPanelGradientFillToolbar.ThemeColorSet = -1;
            // 
            // extComboBoxSystemSel
            // 
            this.extComboBoxSystemSel.BackColor2 = System.Drawing.Color.Red;
            this.extComboBoxSystemSel.BorderColor = System.Drawing.Color.White;
            this.extComboBoxSystemSel.ControlBackground = System.Drawing.SystemColors.Control;
            this.extComboBoxSystemSel.DataSource = null;
            this.extComboBoxSystemSel.DisableBackgroundDisabledShadingGradient = false;
            this.extComboBoxSystemSel.DisabledScaling = 0.5F;
            this.extComboBoxSystemSel.DisplayMember = "";
            this.extComboBoxSystemSel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extComboBoxSystemSel.GradientDirection = 90F;
            this.extComboBoxSystemSel.Location = new System.Drawing.Point(13, 4);
            this.extComboBoxSystemSel.MouseOverScalingColor = 1.3F;
            this.extComboBoxSystemSel.Name = "extComboBoxSystemSel";
            this.extComboBoxSystemSel.SelectedIndex = -1;
            this.extComboBoxSystemSel.SelectedItem = null;
            this.extComboBoxSystemSel.SelectedValue = null;
            this.extComboBoxSystemSel.Size = new System.Drawing.Size(216, 21);
            this.extComboBoxSystemSel.TabIndex = 0;
            this.extComboBoxSystemSel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extComboBoxSystemSel.ValueMember = "";
            this.extComboBoxSystemSel.SelectedIndexChanged += new System.EventHandler(this.extComboBoxSystemSel_SelectedIndexChanged);
            // 
            // extPanelGradientFillUCCP
            // 
            this.extPanelGradientFillUCCP.ChildrenThemed = true;
            this.extPanelGradientFillUCCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelGradientFillUCCP.FlowDirection = null;
            this.extPanelGradientFillUCCP.GradientDirection = 0F;
            this.extPanelGradientFillUCCP.Location = new System.Drawing.Point(0, 32);
            this.extPanelGradientFillUCCP.Name = "extPanelGradientFillUCCP";
            this.extPanelGradientFillUCCP.PaintTransparentColor = System.Drawing.Color.Transparent;
            this.extPanelGradientFillUCCP.Size = new System.Drawing.Size(496, 351);
            this.extPanelGradientFillUCCP.TabIndex = 0;
            this.extPanelGradientFillUCCP.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.extPanelGradientFillUCCP.ThemeColorSet = -1;
            // 
            // UserControlColonisation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extPanelGradientFillUCCP);
            this.Controls.Add(this.extPanelGradientFillToolbar);
            this.Name = "UserControlColonisation";
            this.Size = new System.Drawing.Size(496, 383);
            this.extPanelGradientFillToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPanelGradientFill extPanelGradientFillToolbar;
        private ExtendedControls.ExtPanelGradientFill extPanelGradientFillUCCP;
        private ExtendedControls.ExtComboBox extComboBoxSystemSel;
    }
}
