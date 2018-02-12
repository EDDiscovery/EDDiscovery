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
            this.labelTargetLat = new ExtendedControls.LabelExt();
            this.labelExtTargetLong = new ExtendedControls.LabelExt();
            this.pictureBoxCompass = new ExtendedControls.PictureBoxHotspot();
            this.checkBoxHideTransparent = new ExtendedControls.CheckBoxCustom();
            this.numberBoxTargetLatitude = new ExtendedControls.NumberBoxDouble();
            this.numberBoxTargetLongitude = new ExtendedControls.NumberBoxDouble();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCompass)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
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
            // numberBoxTargetLatitude
            // 
            this.numberBoxTargetLatitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxTargetLatitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxTargetLatitude.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxTargetLatitude.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxTargetLatitude.BorderColorScaling = 0.5F;
            this.numberBoxTargetLatitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxTargetLatitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxTargetLatitude.DelayBeforeNotification = 0;
            this.numberBoxTargetLatitude.Format = "N4";
            this.numberBoxTargetLatitude.FormatCulture = new System.Globalization.CultureInfo("en-GB");
            this.numberBoxTargetLatitude.InErrorCondition = false;
            this.numberBoxTargetLatitude.Location = new System.Drawing.Point(52, 1);
            this.numberBoxTargetLatitude.Maximum = 180D;
            this.numberBoxTargetLatitude.Minimum = -180D;
            this.numberBoxTargetLatitude.Multiline = false;
            this.numberBoxTargetLatitude.Name = "numberBoxTargetLatitude";
            this.numberBoxTargetLatitude.ReadOnly = false;
            this.numberBoxTargetLatitude.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxTargetLatitude.SelectionLength = 0;
            this.numberBoxTargetLatitude.SelectionStart = 0;
            this.numberBoxTargetLatitude.Size = new System.Drawing.Size(54, 20);
            this.numberBoxTargetLatitude.TabIndex = 9;
            this.numberBoxTargetLatitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxTargetLatitude.Value = 0D;
            this.numberBoxTargetLatitude.WordWrap = true;
            // 
            // numberBoxTargetLongitude
            // 
            this.numberBoxTargetLongitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxTargetLongitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxTargetLongitude.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxTargetLongitude.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxTargetLongitude.BorderColorScaling = 0.5F;
            this.numberBoxTargetLongitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxTargetLongitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxTargetLongitude.DelayBeforeNotification = 0;
            this.numberBoxTargetLongitude.Format = "N4";
            this.numberBoxTargetLongitude.FormatCulture = new System.Globalization.CultureInfo("en-GB");
            this.numberBoxTargetLongitude.InErrorCondition = false;
            this.numberBoxTargetLongitude.Location = new System.Drawing.Point(129, 1);
            this.numberBoxTargetLongitude.Maximum = 180D;
            this.numberBoxTargetLongitude.Minimum = -180D;
            this.numberBoxTargetLongitude.Multiline = false;
            this.numberBoxTargetLongitude.Name = "numberBoxTargetLongitude";
            this.numberBoxTargetLongitude.ReadOnly = false;
            this.numberBoxTargetLongitude.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxTargetLongitude.SelectionLength = 0;
            this.numberBoxTargetLongitude.SelectionStart = 0;
            this.numberBoxTargetLongitude.Size = new System.Drawing.Size(54, 20);
            this.numberBoxTargetLongitude.TabIndex = 10;
            this.numberBoxTargetLongitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxTargetLongitude.Value = 0D;
            this.numberBoxTargetLongitude.WordWrap = true;
            // 
            // UserControlCompass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numberBoxTargetLongitude);
            this.Controls.Add(this.numberBoxTargetLatitude);
            this.Controls.Add(this.checkBoxHideTransparent);
            this.Controls.Add(this.pictureBoxCompass);
            this.Controls.Add(this.labelExtTargetLong);
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
        private ExtendedControls.LabelExt labelExtTargetLong;
        private ExtendedControls.PictureBoxHotspot pictureBoxCompass;
        private ExtendedControls.CheckBoxCustom checkBoxHideTransparent;
        private ExtendedControls.NumberBoxDouble numberBoxTargetLatitude;
        private ExtendedControls.NumberBoxDouble numberBoxTargetLongitude;
    }
}
