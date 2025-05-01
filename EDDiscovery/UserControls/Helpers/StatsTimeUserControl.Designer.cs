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
            this.comboBoxTimeMode = new ExtendedControls.ExtComboBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonPlanet = new ExtendedControls.ExtButton();
            this.extButtonStar = new ExtendedControls.ExtButton();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxTimeMode
            // 
            this.comboBoxTimeMode.BorderColor = System.Drawing.Color.Red;
            this.comboBoxTimeMode.DataSource = null;
            this.comboBoxTimeMode.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxTimeMode.DisplayMember = "";
            this.comboBoxTimeMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxTimeMode.Location = new System.Drawing.Point(0, 4);
            this.comboBoxTimeMode.Margin = new System.Windows.Forms.Padding(0, 4, 8, 1);
            this.comboBoxTimeMode.Name = "comboBoxTimeMode";
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
            this.flowLayoutPanel2.Controls.Add(this.extButtonPlanet);
            this.flowLayoutPanel2.Controls.Add(this.extButtonStar);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(800, 32);
            this.flowLayoutPanel2.TabIndex = 9;
            // 
            // extButtonPlanet
            // 
            this.extButtonPlanet.Enabled = false;
            this.extButtonPlanet.Image = global::EDDiscovery.Icons.Controls.Planets;
            this.extButtonPlanet.Location = new System.Drawing.Point(111, 3);
            this.extButtonPlanet.Name = "extButtonPlanet";
            this.extButtonPlanet.Size = new System.Drawing.Size(24, 24);
            this.extButtonPlanet.TabIndex = 7;
            this.extButtonPlanet.UseVisualStyleBackColor = true;
            this.extButtonPlanet.Visible = false;
            this.extButtonPlanet.Click += new System.EventHandler(this.extButtonPlanet_Click);
            // 
            // extButtonStar
            // 
            this.extButtonStar.Enabled = false;
            this.extButtonStar.Image = global::EDDiscovery.Icons.Controls.Stars;
            this.extButtonStar.Location = new System.Drawing.Point(141, 3);
            this.extButtonStar.Name = "extButtonStar";
            this.extButtonStar.Size = new System.Drawing.Size(24, 24);
            this.extButtonStar.TabIndex = 8;
            this.extButtonStar.UseVisualStyleBackColor = true;
            this.extButtonStar.Visible = false;
            this.extButtonStar.Click += new System.EventHandler(this.extButtonStar_Click);
            // 
            // StatsTimeUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel2);
            this.Name = "StatsTimeUserControl";
            this.Size = new System.Drawing.Size(800, 32);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal ExtendedControls.ExtComboBox comboBoxTimeMode;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private ExtButton extButtonPlanet;
        private ExtButton extButtonStar;
    }
}
