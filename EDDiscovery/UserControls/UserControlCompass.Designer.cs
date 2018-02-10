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
    partial class UserControlCompass
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxTargetLatitude = new ExtendedControls.TextBoxBorder();
            this.textBoxTargetLongitude = new ExtendedControls.TextBoxBorder();
            this.labelTargetLat = new ExtendedControls.LabelExt();
            this.labelExtTargetLong = new ExtendedControls.LabelExt();
            this.pictureBoxCompass = new ExtendedControls.PictureBoxHotspot();
            this.checkBoxHideTransparent = new ExtendedControls.CheckBoxCustom();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCompass)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // textBoxTargetLatitude
            // 
            this.textBoxTargetLatitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxTargetLatitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxTargetLatitude.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTargetLatitude.BorderColorScaling = 0.5F;
            this.textBoxTargetLatitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTargetLatitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxTargetLatitude.Location = new System.Drawing.Point(52, 0);
            this.textBoxTargetLatitude.Multiline = false;
            this.textBoxTargetLatitude.Name = "textBoxTargetLatitude";
            this.textBoxTargetLatitude.ReadOnly = false;
            this.textBoxTargetLatitude.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxTargetLatitude.SelectionLength = 0;
            this.textBoxTargetLatitude.SelectionStart = 0;
            this.textBoxTargetLatitude.Size = new System.Drawing.Size(55, 20);
            this.textBoxTargetLatitude.TabIndex = 1;
            this.textBoxTargetLatitude.Text = "0";
            this.textBoxTargetLatitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxTargetLatitude, "Latitude of your destination");
            this.textBoxTargetLatitude.WordWrap = true;
            this.textBoxTargetLatitude.Validated += new System.EventHandler(this.textBox_Validated);
            // 
            // textBoxTargetLongitude
            // 
            this.textBoxTargetLongitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxTargetLongitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxTargetLongitude.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTargetLongitude.BorderColorScaling = 0.5F;
            this.textBoxTargetLongitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTargetLongitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxTargetLongitude.Location = new System.Drawing.Point(129, 0);
            this.textBoxTargetLongitude.Multiline = false;
            this.textBoxTargetLongitude.Name = "textBoxTargetLongitude";
            this.textBoxTargetLongitude.ReadOnly = false;
            this.textBoxTargetLongitude.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxTargetLongitude.SelectionLength = 0;
            this.textBoxTargetLongitude.SelectionStart = 0;
            this.textBoxTargetLongitude.Size = new System.Drawing.Size(55, 20);
            this.textBoxTargetLongitude.TabIndex = 2;
            this.textBoxTargetLongitude.Text = "0";
            this.textBoxTargetLongitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxTargetLongitude, "Longitude of your destination");
            this.textBoxTargetLongitude.WordWrap = true;
            this.textBoxTargetLongitude.Validated += new System.EventHandler(this.textBox_Validated);
            // 
            // labelTargetLat
            // 
            this.labelTargetLat.AutoSize = true;
            this.labelTargetLat.Location = new System.Drawing.Point(8, 7);
            this.labelTargetLat.Name = "labelTargetLat";
            this.labelTargetLat.Size = new System.Drawing.Size(38, 13);
            this.labelTargetLat.TabIndex = 4;
            this.labelTargetLat.Text = "Target";
            // 
            // labelExtTargetLong
            // 
            this.labelExtTargetLong.AutoSize = true;
            this.labelExtTargetLong.Location = new System.Drawing.Point(113, 7);
            this.labelExtTargetLong.Name = "labelExtTargetLong";
            this.labelExtTargetLong.Size = new System.Drawing.Size(10, 13);
            this.labelExtTargetLong.TabIndex = 6;
            this.labelExtTargetLong.Text = ",";
            // 
            // pictureBoxCompass
            // 
            this.pictureBoxCompass.Location = new System.Drawing.Point(11, 26);
            this.pictureBoxCompass.Name = "pictureBoxCompass";
            this.pictureBoxCompass.Size = new System.Drawing.Size(295, 50);
            this.pictureBoxCompass.TabIndex = 7;
            // 
            // checkBoxHideTransparent
            // 
            this.checkBoxHideTransparent.AutoSize = true;
            this.checkBoxHideTransparent.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxHideTransparent.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxHideTransparent.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxHideTransparent.FontNerfReduction = 0.5F;
            this.checkBoxHideTransparent.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxHideTransparent.Location = new System.Drawing.Point(191, 4);
            this.checkBoxHideTransparent.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxHideTransparent.Name = "checkBoxHideTransparent";
            this.checkBoxHideTransparent.Size = new System.Drawing.Size(120, 17);
            this.checkBoxHideTransparent.TabIndex = 8;
            this.checkBoxHideTransparent.Text = "Hide In Transparent";
            this.checkBoxHideTransparent.TickBoxReductionSize = 10;
            this.checkBoxHideTransparent.UseVisualStyleBackColor = true;
            this.checkBoxHideTransparent.CheckedChanged += new System.EventHandler(this.checkBoxHideTransparent_CheckedChanged);
            // 
            // UserControlCompass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxHideTransparent);
            this.Controls.Add(this.pictureBoxCompass);
            this.Controls.Add(this.textBoxTargetLongitude);
            this.Controls.Add(this.labelExtTargetLong);
            this.Controls.Add(this.textBoxTargetLatitude);
            this.Controls.Add(this.labelTargetLat);
            this.Name = "UserControlCompass";
            this.Size = new System.Drawing.Size(312, 83);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCompass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private ExtendedControls.LabelExt labelTargetLat;
        private ExtendedControls.TextBoxBorder textBoxTargetLatitude;
        private ExtendedControls.LabelExt labelExtTargetLong;
        private ExtendedControls.TextBoxBorder textBoxTargetLongitude;
        private ExtendedControls.PictureBoxHotspot pictureBoxCompass;
        private ExtendedControls.CheckBoxCustom checkBoxHideTransparent;
    }
}
