namespace EDDiscovery.UserControls
{
    partial class UserControlScan
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
            this.panelStars = new ExtendedControls.PanelVScroll();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemToolbar = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBoxInfo = new ExtendedControls.RichTextBoxScroll();
            this.imagebox = new ExtendedControls.PictureBoxHotspot();
            this.vScrollBarCustom = new ExtendedControls.VScrollBarCustom();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxSmall = new ExtendedControls.CheckBoxCustom();
            this.checkBoxMedium = new ExtendedControls.CheckBoxCustom();
            this.checkBoxLarge = new ExtendedControls.CheckBoxCustom();
            this.checkBoxMoons = new ExtendedControls.CheckBoxCustom();
            this.checkBoxMaterials = new ExtendedControls.CheckBoxCustom();
            this.checkBoxMaterialsRare = new ExtendedControls.CheckBoxCustom();
            this.checkBoxTiny = new ExtendedControls.CheckBoxCustom();
            this.panelControls = new System.Windows.Forms.Panel();
            this.panelStars.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagebox)).BeginInit();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStars
            // 
            this.panelStars.ContextMenuStrip = this.contextMenuStrip;
            this.panelStars.Controls.Add(this.richTextBoxInfo);
            this.panelStars.Controls.Add(this.imagebox);
            this.panelStars.Controls.Add(this.vScrollBarCustom);
            this.panelStars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStars.InternalMargin = new System.Windows.Forms.Padding(0);
            this.panelStars.Location = new System.Drawing.Point(0, 32);
            this.panelStars.Name = "panelStars";
            this.panelStars.ScrollBarWidth = 20;
            this.panelStars.Size = new System.Drawing.Size(748, 650);
            this.panelStars.TabIndex = 1;
            this.panelStars.VerticalScrollBarDockRight = true;
            this.panelStars.Click += new System.EventHandler(this.panelStars_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemToolbar});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(178, 26);
            // 
            // toolStripMenuItemToolbar
            // 
            this.toolStripMenuItemToolbar.Name = "toolStripMenuItemToolbar";
            this.toolStripMenuItemToolbar.Size = new System.Drawing.Size(177, 22);
            this.toolStripMenuItemToolbar.Text = "Show/Hide Toolbar";
            this.toolStripMenuItemToolbar.Click += new System.EventHandler(this.toolStripMenuItemToolbar_Click);
            // 
            // richTextBoxInfo
            // 
            this.richTextBoxInfo.BorderColor = System.Drawing.Color.Transparent;
            this.richTextBoxInfo.BorderColorScaling = 0.5F;
            this.richTextBoxInfo.HideScrollBar = true;
            this.richTextBoxInfo.Location = new System.Drawing.Point(468, 99);
            this.richTextBoxInfo.Name = "richTextBoxInfo";
            this.richTextBoxInfo.ScrollBarWidth = 20;
            this.richTextBoxInfo.ShowLineCount = false;
            this.richTextBoxInfo.Size = new System.Drawing.Size(200, 100);
            this.richTextBoxInfo.TabIndex = 3;
            // 
            // imagebox
            // 
            this.imagebox.Location = new System.Drawing.Point(0, 0);
            this.imagebox.Name = "imagebox";
            this.imagebox.Size = new System.Drawing.Size(466, 554);
            this.imagebox.TabIndex = 4;
            this.imagebox.TabStop = false;
            // 
            // vScrollBarCustom
            // 
            this.vScrollBarCustom.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom.HideScrollBar = true;
            this.vScrollBarCustom.LargeChange = 20;
            this.vScrollBarCustom.Location = new System.Drawing.Point(728, 0);
            this.vScrollBarCustom.Maximum = -72;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(20, 650);
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 2;
            this.vScrollBarCustom.Text = "vScrollBarCustom1";
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -72;
            this.vScrollBarCustom.ValueLimited = -72;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 20000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            // 
            // checkBoxSmall
            // 
            this.checkBoxSmall.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxSmall.BackColor = System.Drawing.Color.DarkGray;
            this.checkBoxSmall.BackgroundImage = global::EDDiscovery.Properties.Resources.SizeSelectorsSmall;
            this.checkBoxSmall.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxSmall.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxSmall.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxSmall.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxSmall.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxSmall.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.checkBoxSmall.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.checkBoxSmall.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxSmall.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxSmall.FontNerfReduction = 0.5F;
            this.checkBoxSmall.Location = new System.Drawing.Point(184, 0);
            this.checkBoxSmall.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxSmall.Name = "checkBoxSmall";
            this.checkBoxSmall.Size = new System.Drawing.Size(32, 32);
            this.checkBoxSmall.TabIndex = 2;
            this.checkBoxSmall.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxSmall, "Image size small");
            this.checkBoxSmall.UseVisualStyleBackColor = false;
            this.checkBoxSmall.CheckedChanged += new System.EventHandler(this.checkBoxSmall_CheckedChanged);
            // 
            // checkBoxMedium
            // 
            this.checkBoxMedium.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMedium.BackColor = System.Drawing.Color.DarkGray;
            this.checkBoxMedium.BackgroundImage = global::EDDiscovery.Properties.Resources.SizeSelectorsMedium;
            this.checkBoxMedium.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxMedium.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxMedium.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMedium.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMedium.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxMedium.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.checkBoxMedium.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.checkBoxMedium.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxMedium.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxMedium.FontNerfReduction = 0.5F;
            this.checkBoxMedium.Location = new System.Drawing.Point(152, 0);
            this.checkBoxMedium.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxMedium.Name = "checkBoxMedium";
            this.checkBoxMedium.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMedium.TabIndex = 2;
            this.checkBoxMedium.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxMedium, "Image size medium");
            this.checkBoxMedium.UseVisualStyleBackColor = false;
            this.checkBoxMedium.CheckedChanged += new System.EventHandler(this.checkBoxMedium_CheckedChanged);
            // 
            // checkBoxLarge
            // 
            this.checkBoxLarge.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxLarge.BackColor = System.Drawing.Color.DarkGray;
            this.checkBoxLarge.BackgroundImage = global::EDDiscovery.Properties.Resources.SizeSelectorsLarge;
            this.checkBoxLarge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxLarge.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxLarge.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxLarge.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxLarge.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxLarge.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.checkBoxLarge.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.checkBoxLarge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxLarge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxLarge.FontNerfReduction = 0.5F;
            this.checkBoxLarge.Location = new System.Drawing.Point(120, 0);
            this.checkBoxLarge.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxLarge.Name = "checkBoxLarge";
            this.checkBoxLarge.Size = new System.Drawing.Size(32, 32);
            this.checkBoxLarge.TabIndex = 2;
            this.checkBoxLarge.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxLarge, "Image size large");
            this.checkBoxLarge.UseVisualStyleBackColor = false;
            this.checkBoxLarge.CheckedChanged += new System.EventHandler(this.checkBoxLarge_CheckedChanged);
            // 
            // checkBoxMoons
            // 
            this.checkBoxMoons.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMoons.BackColor = System.Drawing.Color.DarkGray;
            this.checkBoxMoons.BackgroundImage = global::EDDiscovery.Properties.Resources.Moon24;
            this.checkBoxMoons.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxMoons.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxMoons.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMoons.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMoons.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxMoons.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.checkBoxMoons.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.checkBoxMoons.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxMoons.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxMoons.FontNerfReduction = 0.5F;
            this.checkBoxMoons.Location = new System.Drawing.Point(76, 0);
            this.checkBoxMoons.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxMoons.Name = "checkBoxMoons";
            this.checkBoxMoons.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMoons.TabIndex = 2;
            this.checkBoxMoons.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxMoons, "Show/Hide Moons");
            this.checkBoxMoons.UseVisualStyleBackColor = false;
            this.checkBoxMoons.CheckedChanged += new System.EventHandler(this.checkBoxMoons_CheckedChanged);
            // 
            // checkBoxMaterials
            // 
            this.checkBoxMaterials.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMaterials.BackColor = System.Drawing.Color.DarkGray;
            this.checkBoxMaterials.BackgroundImage = global::EDDiscovery.Properties.Resources.material;
            this.checkBoxMaterials.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxMaterials.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxMaterials.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMaterials.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMaterials.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxMaterials.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.checkBoxMaterials.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.checkBoxMaterials.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxMaterials.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxMaterials.FontNerfReduction = 0.5F;
            this.checkBoxMaterials.Location = new System.Drawing.Point(0, 0);
            this.checkBoxMaterials.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxMaterials.Name = "checkBoxMaterials";
            this.checkBoxMaterials.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMaterials.TabIndex = 2;
            this.checkBoxMaterials.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxMaterials, "Show/Hide Materials");
            this.checkBoxMaterials.UseVisualStyleBackColor = false;
            this.checkBoxMaterials.CheckedChanged += new System.EventHandler(this.checkBoxMaterials_CheckedChanged);
            // 
            // checkBoxMaterialsRare
            // 
            this.checkBoxMaterialsRare.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMaterialsRare.BackColor = System.Drawing.Color.DarkGray;
            this.checkBoxMaterialsRare.BackgroundImage = global::EDDiscovery.Properties.Resources.materialrare;
            this.checkBoxMaterialsRare.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxMaterialsRare.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxMaterialsRare.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMaterialsRare.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMaterialsRare.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxMaterialsRare.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.checkBoxMaterialsRare.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.checkBoxMaterialsRare.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxMaterialsRare.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxMaterialsRare.FontNerfReduction = 0.5F;
            this.checkBoxMaterialsRare.Location = new System.Drawing.Point(32, 0);
            this.checkBoxMaterialsRare.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxMaterialsRare.Name = "checkBoxMaterialsRare";
            this.checkBoxMaterialsRare.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMaterialsRare.TabIndex = 2;
            this.checkBoxMaterialsRare.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxMaterialsRare, "Show rare materials only");
            this.checkBoxMaterialsRare.UseVisualStyleBackColor = false;
            this.checkBoxMaterialsRare.CheckedChanged += new System.EventHandler(this.checkBoxMaterialsRare_CheckedChanged);
            // 
            // checkBoxTiny
            // 
            this.checkBoxTiny.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxTiny.BackColor = System.Drawing.Color.DarkGray;
            this.checkBoxTiny.BackgroundImage = global::EDDiscovery.Properties.Resources.SizeSelectorsTiny;
            this.checkBoxTiny.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxTiny.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxTiny.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxTiny.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxTiny.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxTiny.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.checkBoxTiny.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.checkBoxTiny.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxTiny.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.checkBoxTiny.FontNerfReduction = 0.5F;
            this.checkBoxTiny.Location = new System.Drawing.Point(216, 0);
            this.checkBoxTiny.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxTiny.Name = "checkBoxTiny";
            this.checkBoxTiny.Size = new System.Drawing.Size(32, 32);
            this.checkBoxTiny.TabIndex = 2;
            this.checkBoxTiny.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxTiny, "Image size teeny tiny");
            this.checkBoxTiny.UseVisualStyleBackColor = false;
            this.checkBoxTiny.CheckedChanged += new System.EventHandler(this.checkBoxTiny_CheckedChanged);
            // 
            // panelControls
            // 
            this.panelControls.ContextMenuStrip = this.contextMenuStrip;
            this.panelControls.Controls.Add(this.checkBoxTiny);
            this.panelControls.Controls.Add(this.checkBoxSmall);
            this.panelControls.Controls.Add(this.checkBoxMedium);
            this.panelControls.Controls.Add(this.checkBoxLarge);
            this.panelControls.Controls.Add(this.checkBoxMoons);
            this.panelControls.Controls.Add(this.checkBoxMaterialsRare);
            this.panelControls.Controls.Add(this.checkBoxMaterials);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(748, 32);
            this.panelControls.TabIndex = 4;
            this.toolTip.SetToolTip(this.panelControls, "Right click on panel to show/hide the toolbar");
            // 
            // UserControlScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelStars);
            this.Controls.Add(this.panelControls);
            this.Name = "UserControlScan";
            this.Size = new System.Drawing.Size(748, 682);
            this.Resize += new System.EventHandler(this.UserControlScan_Resize);
            this.panelStars.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imagebox)).EndInit();
            this.panelControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.PanelVScroll panelStars;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom;
        private ExtendedControls.RichTextBoxScroll richTextBoxInfo;
        private System.Windows.Forms.Panel panelControls;
        private ExtendedControls.CheckBoxCustom checkBoxMaterials;
        private ExtendedControls.CheckBoxCustom checkBoxMoons;
        private ExtendedControls.CheckBoxCustom checkBoxSmall;
        private ExtendedControls.CheckBoxCustom checkBoxMedium;
        private ExtendedControls.CheckBoxCustom checkBoxLarge;
        private ExtendedControls.CheckBoxCustom checkBoxTiny;
        private ExtendedControls.CheckBoxCustom checkBoxMaterialsRare;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemToolbar;
        private ExtendedControls.PictureBoxHotspot imagebox;
    }
}
