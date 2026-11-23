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
    partial class TravelPanel
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
            this.extPictureBox = new ExtendedControls.ExtPictureBox();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonAlignment = new ExtendedControls.ExtButton();
            this.edsmSpanshButton = new EDDiscovery.UserControls.EDSMSpanshButton();
            this.extButtonFont = new ExtendedControls.ExtButton();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.extButtonShowControl = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBox)).BeginInit();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // extPictureBox
            // 
            this.extPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPictureBox.FillColor = System.Drawing.Color.Transparent;
            this.extPictureBox.FreezeTracking = false;
            this.extPictureBox.Location = new System.Drawing.Point(0, 30);
            this.extPictureBox.Name = "extPictureBox";
            this.extPictureBox.Size = new System.Drawing.Size(684, 636);
            this.extPictureBox.TabIndex = 1;
            // 
            // panelControls
            // 
            this.panelControls.AutoSize = true;
            this.panelControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelControls.BackColor = System.Drawing.SystemColors.Control;
            this.panelControls.Controls.Add(this.extButtonShowControl);
            this.panelControls.Controls.Add(this.extButtonAlignment);
            this.panelControls.Controls.Add(this.edsmSpanshButton);
            this.panelControls.Controls.Add(this.extButtonFont);
            this.panelControls.Controls.Add(this.extCheckBoxWordWrap);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(684, 30);
            this.panelControls.TabIndex = 33;
            // 
            // extButtonAlignment
            // 
            this.extButtonAlignment.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonAlignment.BackColor2 = System.Drawing.Color.Red;
            this.extButtonAlignment.ButtonDisabledScaling = 0.5F;
            this.extButtonAlignment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonAlignment.GradientDirection = 90F;
            this.extButtonAlignment.Image = global::EDDiscovery.Icons.Controls.AlignCentre;
            this.extButtonAlignment.Location = new System.Drawing.Point(37, 1);
            this.extButtonAlignment.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonAlignment.MouseOverScaling = 1.3F;
            this.extButtonAlignment.MouseSelectedScaling = 1.3F;
            this.extButtonAlignment.Name = "extButtonAlignment";
            this.extButtonAlignment.Size = new System.Drawing.Size(28, 28);
            this.extButtonAlignment.TabIndex = 29;
            this.extButtonAlignment.UseVisualStyleBackColor = false;
            this.extButtonAlignment.Click += new System.EventHandler(this.extButtonAlignment_Click);
            // 
            // edsmSpanshButton
            // 
            this.edsmSpanshButton.ButtonGradientDirection = 90F;
            this.edsmSpanshButton.CheckBoxColor = System.Drawing.Color.Gray;
            this.edsmSpanshButton.CheckBoxGradientDirection = 225F;
            this.edsmSpanshButton.CheckBoxInnerColor = System.Drawing.Color.White;
            this.edsmSpanshButton.CheckColor = System.Drawing.Color.DarkBlue;
            this.edsmSpanshButton.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.edsmSpanshButton.DisabledScaling = 0.5F;
            this.edsmSpanshButton.Image = global::EDDiscovery.Icons.Controls.EDSMSpansh;
            this.edsmSpanshButton.ImageIndeterminate = null;
            this.edsmSpanshButton.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.edsmSpanshButton.ImageUnchecked = null;
            this.edsmSpanshButton.Location = new System.Drawing.Point(71, 1);
            this.edsmSpanshButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.edsmSpanshButton.MouseOverScaling = 1.3F;
            this.edsmSpanshButton.MouseSelectedScaling = 1.3F;
            this.edsmSpanshButton.Name = "edsmSpanshButton";
            this.edsmSpanshButton.Size = new System.Drawing.Size(28, 28);
            this.edsmSpanshButton.TabIndex = 35;
            this.edsmSpanshButton.TickBoxReductionRatio = 0.75F;
            this.edsmSpanshButton.UseVisualStyleBackColor = true;
            // 
            // extButtonFont
            // 
            this.extButtonFont.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonFont.BackColor2 = System.Drawing.Color.Red;
            this.extButtonFont.ButtonDisabledScaling = 0.5F;
            this.extButtonFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonFont.GradientDirection = 90F;
            this.extButtonFont.Image = global::EDDiscovery.Icons.Controls.Font;
            this.extButtonFont.Location = new System.Drawing.Point(105, 1);
            this.extButtonFont.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonFont.MouseOverScaling = 1.3F;
            this.extButtonFont.MouseSelectedScaling = 1.3F;
            this.extButtonFont.Name = "extButtonFont";
            this.extButtonFont.Size = new System.Drawing.Size(28, 28);
            this.extButtonFont.TabIndex = 29;
            this.extButtonFont.UseVisualStyleBackColor = false;
            this.extButtonFont.Click += new System.EventHandler(this.extButtonFont_Click);
            // 
            // extCheckBoxWordWrap
            // 
            this.extCheckBoxWordWrap.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxWordWrap.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxWordWrap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxWordWrap.ButtonGradientDirection = 90F;
            this.extCheckBoxWordWrap.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckBoxGradientDirection = 225F;
            this.extCheckBoxWordWrap.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWordWrap.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWordWrap.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxWordWrap.DisabledScaling = 0.5F;
            this.extCheckBoxWordWrap.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxWordWrap.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxWordWrap.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxWordWrap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxWordWrap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxWordWrap.Image = global::EDDiscovery.Icons.Controls.WordWrapOn;
            this.extCheckBoxWordWrap.ImageIndeterminate = null;
            this.extCheckBoxWordWrap.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxWordWrap.ImageUnchecked = global::EDDiscovery.Icons.Controls.WordWrapOff;
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(139, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxWordWrap.MouseOverScaling = 1.3F;
            this.extCheckBoxWordWrap.MouseSelectedScaling = 1.3F;
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 34;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // extButtonShowControl
            // 
            this.extButtonShowControl.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonShowControl.BackColor2 = System.Drawing.Color.Red;
            this.extButtonShowControl.ButtonDisabledScaling = 0.5F;
            this.extButtonShowControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonShowControl.GradientDirection = 90F;
            this.extButtonShowControl.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.extButtonShowControl.Location = new System.Drawing.Point(3, 1);
            this.extButtonShowControl.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonShowControl.MouseOverScaling = 1.3F;
            this.extButtonShowControl.MouseSelectedScaling = 1.3F;
            this.extButtonShowControl.Name = "extButtonShowControl";
            this.extButtonShowControl.Size = new System.Drawing.Size(28, 28);
            this.extButtonShowControl.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonShowControl, "Display Settings");
            this.extButtonShowControl.UseVisualStyleBackColor = false;
            this.extButtonShowControl.Click += new System.EventHandler(this.extButtonShowControl_Click);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // TravelPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extPictureBox);
            this.Controls.Add(this.panelControls);
            this.Name = "TravelPanel";
            this.Size = new System.Drawing.Size(684, 666);
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBox)).EndInit();
            this.panelControls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtPictureBox extPictureBox;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private ExtendedControls.ExtButton extButtonAlignment;
        private EDSMSpanshButton edsmSpanshButton;
        private ExtendedControls.ExtButton extButtonFont;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private ExtendedControls.ExtButton extButtonShowControl;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
