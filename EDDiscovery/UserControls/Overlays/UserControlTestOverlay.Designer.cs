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
    partial class UserControlTestOverlay
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
            this.extButton1 = new ExtendedControls.ExtButton();
            this.extTabControl1 = new ExtendedControls.ExtTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.extCheckBox1 = new ExtendedControls.ExtCheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.extTabControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // extButton1
            // 
            this.extButton1.Location = new System.Drawing.Point(18, 3);
            this.extButton1.Name = "extButton1";
            this.extButton1.Size = new System.Drawing.Size(75, 23);
            this.extButton1.TabIndex = 1;
            this.extButton1.Text = "extButton1";
            this.extButton1.UseVisualStyleBackColor = true;
            // 
            // extTabControl1
            // 
            this.extTabControl1.AllowDragReorder = false;
            this.extTabControl1.AutoForceUpdate = true;
            this.extTabControl1.Controls.Add(this.tabPage1);
            this.extTabControl1.Controls.Add(this.tabPage2);
            this.extTabControl1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extTabControl1.Location = new System.Drawing.Point(18, 41);
            this.extTabControl1.Name = "extTabControl1";
            this.extTabControl1.SelectedIndex = 0;
            this.extTabControl1.Size = new System.Drawing.Size(200, 100);
            this.extTabControl1.TabControlBorderColor = System.Drawing.Color.DarkGray;
            this.extTabControl1.TabDisabledScaling = 0.5F;
            this.extTabControl1.TabIndex = 2;
            this.extTabControl1.TabMouseOverColor = System.Drawing.Color.White;
            this.extTabControl1.TabNotSelectedBorderColor = System.Drawing.Color.Gray;
            this.extTabControl1.TabNotSelectedColor = System.Drawing.Color.Gray;
            this.extTabControl1.TabSelectedColor = System.Drawing.Color.LightGray;
            this.extTabControl1.TabStyle = tabStyleSquare1;
            this.extTabControl1.TextNotSelectedColor = System.Drawing.SystemColors.ControlText;
            this.extTabControl1.TextSelectedColor = System.Drawing.SystemColors.ControlText;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(192, 74);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // extCheckBox1
            // 
            this.extCheckBox1.AutoSize = true;
            this.extCheckBox1.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBox1.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBox1.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBox1.ImageIndeterminate = null;
            this.extCheckBox1.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBox1.ImageUnchecked = null;
            this.extCheckBox1.Location = new System.Drawing.Point(18, 164);
            this.extCheckBox1.Name = "extCheckBox1";
            this.extCheckBox1.Size = new System.Drawing.Size(95, 17);
            this.extCheckBox1.TabIndex = 3;
            this.extCheckBox1.Text = "extCheckBox1";
            this.extCheckBox1.TickBoxReductionRatio = 0.75F;
            this.extCheckBox1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.extButton1);
            this.panel1.Controls.Add(this.extCheckBox1);
            this.panel1.Controls.Add(this.extTabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(496, 383);
            this.panel1.TabIndex = 4;
            // 
            // UserControlTestOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "UserControlTestOverlay";
            this.Size = new System.Drawing.Size(496, 383);
            this.extTabControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.ExtButton extButton1;
        private ExtendedControls.ExtTabControl extTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private ExtendedControls.ExtCheckBox extCheckBox1;
        private System.Windows.Forms.Panel panel1;
    }
}
