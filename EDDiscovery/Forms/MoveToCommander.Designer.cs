/*
 * Copyright © 2016 EDDiscovery development team
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
    partial class MoveToCommander
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonExtCancel = new ExtendedControls.ExtButton();
            this.buttonTransfer = new ExtendedControls.ExtButton();
            this.comboBoxCommanders = new ExtendedControls.ExtComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Move selected history to commander.";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonExtCancel);
            this.panel1.Controls.Add(this.buttonTransfer);
            this.panel1.Controls.Add(this.comboBoxCommanders);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(322, 139);
            this.panel1.TabIndex = 4;
            // 
            // buttonExtCancel
            // 
            this.buttonExtCancel.Location = new System.Drawing.Point(89, 103);
            this.buttonExtCancel.Name = "buttonExtCancel";
            this.buttonExtCancel.Size = new System.Drawing.Size(100, 23);
            this.buttonExtCancel.TabIndex = 4;
            this.buttonExtCancel.Text = "Cancel";
            this.buttonExtCancel.UseVisualStyleBackColor = true;
            this.buttonExtCancel.Click += new System.EventHandler(this.buttonExtCancel_Click);
            // 
            // buttonTransfer
            // 
            this.buttonTransfer.Location = new System.Drawing.Point(205, 103);
            this.buttonTransfer.Name = "buttonTransfer";
            this.buttonTransfer.Size = new System.Drawing.Size(100, 23);
            this.buttonTransfer.TabIndex = 3;
            this.buttonTransfer.Text = "Transfer";
            this.buttonTransfer.UseVisualStyleBackColor = true;
            this.buttonTransfer.Click += new System.EventHandler(this.buttonTransfer_Click);
            // 
            // comboBoxCommanders
            // 
            this.comboBoxCommanders.ArrowWidth = 1;
            this.comboBoxCommanders.BorderColor = System.Drawing.Color.White;
            this.comboBoxCommanders.ButtonColorScaling = 0.5F;
            this.comboBoxCommanders.DataSource = null;
            this.comboBoxCommanders.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCommanders.DisplayMember = "";
            this.comboBoxCommanders.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCommanders.DropDownHeight = 106;
            this.comboBoxCommanders.DropDownWidth = 165;
            this.comboBoxCommanders.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCommanders.ItemHeight = 13;
            this.comboBoxCommanders.Location = new System.Drawing.Point(19, 58);
            this.comboBoxCommanders.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCommanders.Name = "comboBoxCommanders";
            this.comboBoxCommanders.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCommanders.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCommanders.ScrollBarWidth = 16;
            this.comboBoxCommanders.SelectedIndex = -1;
            this.comboBoxCommanders.SelectedItem = null;
            this.comboBoxCommanders.SelectedValue = null;
            this.comboBoxCommanders.Size = new System.Drawing.Size(290, 21);
            this.comboBoxCommanders.TabIndex = 0;
            this.comboBoxCommanders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxCommanders.ValueMember = "";
            this.comboBoxCommanders.SelectedIndexChanged += new System.EventHandler(this.comboBoxCommanders_SelectedIndexChanged);
            // 
            // MoveToCommander
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 139);
            this.Controls.Add(this.panel1);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "MoveToCommander";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MoveToCommander";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtComboBox comboBoxCommanders;
        private System.Windows.Forms.Label label1;
        private ExtendedControls.ExtButton buttonTransfer;
        private System.Windows.Forms.Panel panel1;
        private ExtendedControls.ExtButton buttonExtCancel;
    }
}