using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace EDDiscovery2
{
        partial class FormMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMap));
            this.glControl = new OpenTK.GLControl();
            this.textboxFrom = new System.Windows.Forms.TextBox();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.labelSystemCoords = new System.Windows.Forms.Label();
            this.toolStripShowAllStars = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonLastKnownPosition = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDrawLines = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShowAllStars = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStations = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGrid = new System.Windows.Forms.ToolStripButton();
            this.buttonSetDefault = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripShowAllStars.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl
            // 
            this.glControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Location = new System.Drawing.Point(0, 44);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(918, 485);
            this.glControl.TabIndex = 0;
            this.glControl.VSync = false;
            this.glControl.Load += new System.EventHandler(this.glControl_Load);
            this.glControl.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl_Paint);
            this.glControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseDown);
            this.glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseMove);
            this.glControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.glControl_OnMouseWheel);
            this.glControl.Resize += new System.EventHandler(this.glControl_Resize);
            // 
            // textboxFrom
            // 
            this.textboxFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxFrom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textboxFrom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textboxFrom.Location = new System.Drawing.Point(365, 7);
            this.textboxFrom.Name = "textboxFrom";
            this.textboxFrom.Size = new System.Drawing.Size(125, 20);
            this.textboxFrom.TabIndex = 16;
            this.textboxFrom.Text = "Sol";
            // 
            // buttonCenter
            // 
            this.buttonCenter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCenter.Location = new System.Drawing.Point(496, 5);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(75, 23);
            this.buttonCenter.TabIndex = 17;
            this.buttonCenter.Text = "Center";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // labelSystemCoords
            // 
            this.labelSystemCoords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSystemCoords.AutoSize = true;
            this.labelSystemCoords.Location = new System.Drawing.Point(658, 10);
            this.labelSystemCoords.Name = "labelSystemCoords";
            this.labelSystemCoords.Size = new System.Drawing.Size(57, 13);
            this.labelSystemCoords.TabIndex = 18;
            this.labelSystemCoords.Text = "Sol x=0.00";
            // 
            // toolStripShowAllStars
            // 
            this.toolStripShowAllStars.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonLastKnownPosition,
            this.toolStripButtonDrawLines,
            this.toolStripButtonShowAllStars,
            this.toolStripButtonStations,
            this.toolStripButtonGrid});
            this.toolStripShowAllStars.Location = new System.Drawing.Point(0, 0);
            this.toolStripShowAllStars.Name = "toolStripShowAllStars";
            this.toolStripShowAllStars.Size = new System.Drawing.Size(918, 31);
            this.toolStripShowAllStars.TabIndex = 19;
            this.toolStripShowAllStars.Text = "toolStrip1";
            // 
            // toolStripButtonLastKnownPosition
            // 
            this.toolStripButtonLastKnownPosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLastKnownPosition.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLastKnownPosition.Image")));
            this.toolStripButtonLastKnownPosition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLastKnownPosition.Name = "toolStripButtonLastKnownPosition";
            this.toolStripButtonLastKnownPosition.Size = new System.Drawing.Size(23, 28);
            this.toolStripButtonLastKnownPosition.Text = "Last known position";
            this.toolStripButtonLastKnownPosition.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButtonDrawLines
            // 
            this.toolStripButtonDrawLines.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDrawLines.BackgroundImage")));
            this.toolStripButtonDrawLines.CheckOnClick = true;
            this.toolStripButtonDrawLines.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDrawLines.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDrawLines.Image")));
            this.toolStripButtonDrawLines.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDrawLines.Name = "toolStripButtonDrawLines";
            this.toolStripButtonDrawLines.Size = new System.Drawing.Size(23, 28);
            this.toolStripButtonDrawLines.Text = "Draw lines";
            this.toolStripButtonDrawLines.Click += new System.EventHandler(this.toolStripButtonDrawLines_Click);
            // 
            // toolStripButtonShowAllStars
            // 
            this.toolStripButtonShowAllStars.Checked = true;
            this.toolStripButtonShowAllStars.CheckOnClick = true;
            this.toolStripButtonShowAllStars.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonShowAllStars.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShowAllStars.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonShowAllStars.Image")));
            this.toolStripButtonShowAllStars.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowAllStars.Name = "toolStripButtonShowAllStars";
            this.toolStripButtonShowAllStars.Size = new System.Drawing.Size(23, 28);
            this.toolStripButtonShowAllStars.Text = "Show all stars";
            this.toolStripButtonShowAllStars.Click += new System.EventHandler(this.toolStripButtonShowAllStars_Click);
            // 
            // toolStripButtonStations
            // 
            this.toolStripButtonStations.Checked = true;
            this.toolStripButtonStations.CheckOnClick = true;
            this.toolStripButtonStations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonStations.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStations.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStations.Image")));
            this.toolStripButtonStations.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonStations.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStations.Name = "toolStripButtonStations";
            this.toolStripButtonStations.Size = new System.Drawing.Size(28, 28);
            this.toolStripButtonStations.Text = "Stations";
            this.toolStripButtonStations.Click += new System.EventHandler(this.toolStripButtonStations_Click);
            // 
            // toolStripButtonGrid
            // 
            this.toolStripButtonGrid.CheckOnClick = true;
            this.toolStripButtonGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGrid.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGrid.Image")));
            this.toolStripButtonGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGrid.Name = "toolStripButtonGrid";
            this.toolStripButtonGrid.Size = new System.Drawing.Size(23, 28);
            this.toolStripButtonGrid.Text = "Grid";
            this.toolStripButtonGrid.ToolTipText = "Show Grid";
            this.toolStripButtonGrid.Click += new System.EventHandler(this.toolStripButtonGrud_Click);
            // 
            // buttonSetDefault
            // 
            this.buttonSetDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetDefault.Location = new System.Drawing.Point(577, 5);
            this.buttonSetDefault.Name = "buttonSetDefault";
            this.buttonSetDefault.Size = new System.Drawing.Size(75, 23);
            this.buttonSetDefault.TabIndex = 20;
            this.buttonSetDefault.Text = "Set Default";
            this.buttonSetDefault.UseCompatibleTextRendering = true;
            this.buttonSetDefault.UseVisualStyleBackColor = true;
            this.buttonSetDefault.Click += new System.EventHandler(this.buttonSetDefault_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 532);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(918, 22);
            this.statusStrip.TabIndex = 21;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(35, 17);
            this.statusLabel.Text = "x=0.0";
            // 
            // FormMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 554);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.buttonSetDefault);
            this.Controls.Add(this.labelSystemCoords);
            this.Controls.Add(this.buttonCenter);
            this.Controls.Add(this.textboxFrom);
            this.Controls.Add(this.glControl);
            this.Controls.Add(this.toolStripShowAllStars);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMap";
            this.Text = "3D Star Map";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMap_FormClosing);
            this.Load += new System.EventHandler(this.FormMap_Load);
            this.toolStripShowAllStars.ResumeLayout(false);
            this.toolStripShowAllStars.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            }


            #endregion

            private OpenTK.GLControl glControl;
            internal TextBox textboxFrom;
            private Button buttonCenter;
            private Label labelSystemCoords;
        private ToolStrip toolStripShowAllStars;
        private ToolStripButton toolStripButtonLastKnownPosition;
        private ToolStripButton toolStripButtonDrawLines;
        private Button buttonSetDefault;
        private ToolStripButton toolStripButtonShowAllStars;
        private ToolStripButton toolStripButtonStations;
        private ToolStripButton toolStripButtonGrid;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
    }
    }