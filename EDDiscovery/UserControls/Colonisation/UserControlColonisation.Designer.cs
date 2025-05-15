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
            ExtendedControls.TabStyleSquare tabStyleSquare1 = new ExtendedControls.TabStyleSquare();
            this.extTabControl = new ExtendedControls.ExtTabControl();
            this.SuspendLayout();
            // 
            // extTabControl
            // 
            this.extTabControl.AllowDragReorder = false;
            this.extTabControl.AutoForceUpdate = true;
            this.extTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extTabControl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extTabControl.Location = new System.Drawing.Point(0, 0);
            this.extTabControl.Name = "extTabControl";
            this.extTabControl.PaintTransparentColor = System.Drawing.Color.Transparent;
            this.extTabControl.SelectedIndex = 0;
            this.extTabControl.Size = new System.Drawing.Size(496, 383);
            this.extTabControl.TabBackgroundGradientDirection = 0F;
            this.extTabControl.TabControlBorderColor = System.Drawing.Color.DarkGray;
            this.extTabControl.TabControlBorderColor2 = System.Drawing.Color.DarkGray;
            this.extTabControl.TabDisabledScaling = 0.5F;
            this.extTabControl.TabGradientDirection = 90F;
            this.extTabControl.TabIndex = 2;
            this.extTabControl.TabMouseOverColor = System.Drawing.Color.White;
            this.extTabControl.TabMouseOverColor2 = System.Drawing.Color.White;
            this.extTabControl.TabNotSelectedBorderColor = System.Drawing.Color.Gray;
            this.extTabControl.TabNotSelectedColor = System.Drawing.Color.Gray;
            this.extTabControl.TabNotSelectedColor2 = System.Drawing.Color.Gray;
            this.extTabControl.TabSelectedColor = System.Drawing.Color.LightGray;
            this.extTabControl.TabSelectedColor2 = System.Drawing.Color.Gray;
            this.extTabControl.TabStyle = tabStyleSquare1;
            this.extTabControl.TextNotSelectedColor = System.Drawing.SystemColors.ControlText;
            this.extTabControl.TextSelectedColor = System.Drawing.SystemColors.ControlText;
            this.extTabControl.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.extTabControl.ThemeColorSet = 1;
            // 
            // UserControlColonisation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extTabControl);
            this.Name = "UserControlColonisation";
            this.Size = new System.Drawing.Size(496, 383);
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.ExtTabControl extTabControl;
    }
}
