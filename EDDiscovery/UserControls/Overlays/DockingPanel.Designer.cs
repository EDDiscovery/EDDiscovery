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
    partial class DockingPanel
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
            this.orbisDockingPads = new ExtendedControls.OrbisDockingPads();
            this.fleetCarrierDockingPads = new ExtendedControls.FleetCarrierDockingPads();
            this.contextMenuStripCarrier = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.flipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripCarrier.SuspendLayout();
            this.SuspendLayout();
            // 
            // orbisDockingPads
            // 
            this.orbisDockingPads.BorderColor = System.Drawing.Color.Black;
            this.orbisDockingPads.ForeColor = System.Drawing.Color.Black;
            this.orbisDockingPads.LargePad = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(1)))), ((int)(((byte)(1)))));
            this.orbisDockingPads.Location = new System.Drawing.Point(0, 0);
            this.orbisDockingPads.MediumPad = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.orbisDockingPads.Name = "orbisDockingPads";
            this.orbisDockingPads.NonSelectedIntensity = 0.4F;
            this.orbisDockingPads.SelectedIndex = 0;
            this.orbisDockingPads.Size = new System.Drawing.Size(243, 279);
            this.orbisDockingPads.SmallPad = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.orbisDockingPads.TabIndex = 0;
            // 
            // fleetCarrierDockingPads
            // 
            this.fleetCarrierDockingPads.ContextMenuStrip = this.contextMenuStripCarrier;
            this.fleetCarrierDockingPads.ForeColor = System.Drawing.Color.Black;
            this.fleetCarrierDockingPads.LargePad = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(1)))), ((int)(((byte)(1)))));
            this.fleetCarrierDockingPads.Location = new System.Drawing.Point(318, 33);
            this.fleetCarrierDockingPads.MediumPad = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.fleetCarrierDockingPads.Name = "fleetCarrierDockingPads";
            this.fleetCarrierDockingPads.NonSelected = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.fleetCarrierDockingPads.SelectedIndex = 0;
            this.fleetCarrierDockingPads.Size = new System.Drawing.Size(321, 213);
            this.fleetCarrierDockingPads.SmallPad = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.fleetCarrierDockingPads.TabIndex = 1;
            this.fleetCarrierDockingPads.Text = "fleetCarrierDockingPads1";
            // 
            // contextMenuStripCarrier
            // 
            this.contextMenuStripCarrier.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.flipToolStripMenuItem});
            this.contextMenuStripCarrier.Name = "contextMenuStripCarrier";
            this.contextMenuStripCarrier.Size = new System.Drawing.Size(94, 26);
            // 
            // flipToolStripMenuItem
            // 
            this.flipToolStripMenuItem.Name = "flipToolStripMenuItem";
            this.flipToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.flipToolStripMenuItem.Text = "Flip";
            this.flipToolStripMenuItem.Click += new System.EventHandler(this.flipToolStripMenuItem_Click);
            // 
            // DockingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fleetCarrierDockingPads);
            this.Controls.Add(this.orbisDockingPads);
            this.Name = "DockingPanel";
            this.Size = new System.Drawing.Size(684, 666);
            this.contextMenuStripCarrier.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.OrbisDockingPads orbisDockingPads;
        private ExtendedControls.FleetCarrierDockingPads fleetCarrierDockingPads;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripCarrier;
        private System.Windows.Forms.ToolStripMenuItem flipToolStripMenuItem;
    }
}
