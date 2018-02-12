/*
 * Copyright © 2017 EDDiscovery development team
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
namespace EDDiscovery.Forms
{
    partial class ImportSphere
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
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.buttonOK = new ExtendedControls.ButtonExt();
            this.txtsphereRadius = new ExtendedControls.NumberBoxDouble();
            this.labelExt8 = new System.Windows.Forms.Label();
            this.txtExportVisited = new ExtendedControls.AutoCompleteTextBox();
            this.btnExportTravel = new ExtendedControls.ButtonExt();
            this.labelExt9 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(277, 86);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 22;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(358, 86);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 21;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // txtsphereRadius
            // 
            this.txtsphereRadius.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtsphereRadius.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtsphereRadius.BackErrorColor = System.Drawing.Color.Red;
            this.txtsphereRadius.BorderColor = System.Drawing.Color.Transparent;
            this.txtsphereRadius.BorderColorScaling = 0.5F;
            this.txtsphereRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtsphereRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.txtsphereRadius.DelayBeforeNotification = 0;
            this.txtsphereRadius.Format = "0.#";
            this.txtsphereRadius.InErrorCondition = false;
            this.txtsphereRadius.Location = new System.Drawing.Point(85, 51);
            this.txtsphereRadius.Maximum = 100000D;
            this.txtsphereRadius.Minimum = 0D;
            this.txtsphereRadius.Multiline = false;
            this.txtsphereRadius.Name = "txtsphereRadius";
            this.txtsphereRadius.ReadOnly = false;
            this.txtsphereRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtsphereRadius.SelectionLength = 0;
            this.txtsphereRadius.SelectionStart = 0;
            this.txtsphereRadius.Size = new System.Drawing.Size(83, 20);
            this.txtsphereRadius.TabIndex = 19;
            this.txtsphereRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtsphereRadius.Value = 10D;
            this.txtsphereRadius.WordWrap = true;
            // 
            // labelExt8
            // 
            this.labelExt8.AutoSize = true;
            this.labelExt8.Location = new System.Drawing.Point(16, 54);
            this.labelExt8.Name = "labelExt8";
            this.labelExt8.Size = new System.Drawing.Size(40, 13);
            this.labelExt8.TabIndex = 20;
            this.labelExt8.Text = "Radius";
            // 
            // txtExportVisited
            // 
            this.txtExportVisited.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtExportVisited.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtExportVisited.BackErrorColor = System.Drawing.Color.Red;
            this.txtExportVisited.BorderColor = System.Drawing.Color.Transparent;
            this.txtExportVisited.BorderColorScaling = 0.5F;
            this.txtExportVisited.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExportVisited.ControlBackground = System.Drawing.SystemColors.Control;
            this.txtExportVisited.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.txtExportVisited.DropDownBorderColor = System.Drawing.Color.Green;
            this.txtExportVisited.DropDownHeight = 200;
            this.txtExportVisited.DropDownItemHeight = 20;
            this.txtExportVisited.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.txtExportVisited.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.txtExportVisited.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.txtExportVisited.DropDownWidth = 0;
            this.txtExportVisited.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.txtExportVisited.InErrorCondition = false;
            this.txtExportVisited.Location = new System.Drawing.Point(86, 25);
            this.txtExportVisited.Multiline = false;
            this.txtExportVisited.Name = "txtExportVisited";
            this.txtExportVisited.ReadOnly = false;
            this.txtExportVisited.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtExportVisited.SelectionLength = 0;
            this.txtExportVisited.SelectionStart = 0;
            this.txtExportVisited.Size = new System.Drawing.Size(189, 20);
            this.txtExportVisited.TabIndex = 16;
            this.txtExportVisited.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtExportVisited.WordWrap = true;
            // 
            // btnExportTravel
            // 
            this.btnExportTravel.Location = new System.Drawing.Point(302, 25);
            this.btnExportTravel.Name = "btnExportTravel";
            this.btnExportTravel.Size = new System.Drawing.Size(133, 23);
            this.btnExportTravel.TabIndex = 17;
            this.btnExportTravel.Text = "Current system";
            this.btnExportTravel.UseVisualStyleBackColor = true;
            this.btnExportTravel.Click += new System.EventHandler(this.btnExportTravel_Click);
            // 
            // labelExt9
            // 
            this.labelExt9.AutoSize = true;
            this.labelExt9.Location = new System.Drawing.Point(16, 28);
            this.labelExt9.Name = "labelExt9";
            this.labelExt9.Size = new System.Drawing.Size(41, 13);
            this.labelExt9.TabIndex = 18;
            this.labelExt9.Text = "System";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtExportVisited);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.labelExt9);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Controls.Add(this.btnExportTravel);
            this.panel1.Controls.Add(this.txtsphereRadius);
            this.panel1.Controls.Add(this.labelExt8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(455, 123);
            this.panel1.TabIndex = 23;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // ImportSphere
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 123);
            this.Controls.Add(this.panel1);
            this.Name = "ImportSphere";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import sphere systems";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.AutoCompleteTextBox txtExportVisited;
        private ExtendedControls.ButtonExt btnExportTravel;
        private ExtendedControls.NumberBoxDouble txtsphereRadius;
        private System.Windows.Forms.Label labelExt9;
        private System.Windows.Forms.Label labelExt8;
        private ExtendedControls.ButtonExt buttonCancel;
        private ExtendedControls.ButtonExt buttonOK;
        private System.Windows.Forms.Panel panel1;
    }
}