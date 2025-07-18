namespace EDDiscovery.UserControls.Colonisation
{
    partial class ColonisationSystemDisplay
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
            EliteDangerousCore.SystemDisplay systemDisplay1 = new EliteDangerousCore.SystemDisplay();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extCheckBoxSystemShow = new ExtendedControls.ExtCheckBox();
            this.scanDisplayBodyFiltersButton = new EDDiscovery.UserControls.ScanDisplayBodyFiltersButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.scanDisplayConfigureButton = new EDDiscovery.UserControls.ScanDisplayConfigureButton();
            this.extPanelGradientFill1 = new ExtendedControls.ExtPanelGradientFill();
            this.labelDataPosition = new ExtendedControls.LabelData();
            this.labelDataGov = new ExtendedControls.LabelData();
            this.labelDataFaction = new ExtendedControls.LabelData();
            this.extLabelClaimReleased = new ExtendedControls.ExtLabel();
            this.extLabelBeaconDeployed = new ExtendedControls.ExtLabel();
            this.edsmSpanshButton = new EDDiscovery.UserControls.EDSMSpanshButton();
            this.scanDisplayUserControl = new EDDiscovery.UserControls.ScanDisplayUserControl();
            this.labelDataName = new ExtendedControls.LabelData();
            this.extPanelGradientFill1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 20000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // extCheckBoxSystemShow
            // 
            this.extCheckBoxSystemShow.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxSystemShow.BackColor = System.Drawing.SystemColors.Control;
            this.extCheckBoxSystemShow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxSystemShow.ButtonGradientDirection = 90F;
            this.extCheckBoxSystemShow.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxSystemShow.CheckBoxGradientDirection = 225F;
            this.extCheckBoxSystemShow.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxSystemShow.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxSystemShow.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxSystemShow.Checked = true;
            this.extCheckBoxSystemShow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.extCheckBoxSystemShow.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxSystemShow.DisabledScaling = 0.5F;
            this.extCheckBoxSystemShow.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxSystemShow.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxSystemShow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxSystemShow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxSystemShow.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.extCheckBoxSystemShow.Image = global::EDDiscovery.Icons.Controls.Scan_Star;
            this.extCheckBoxSystemShow.ImageIndeterminate = null;
            this.extCheckBoxSystemShow.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxSystemShow.ImageUnchecked = null;
            this.extCheckBoxSystemShow.Location = new System.Drawing.Point(4, 51);
            this.extCheckBoxSystemShow.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxSystemShow.MouseOverScaling = 1.3F;
            this.extCheckBoxSystemShow.MouseSelectedScaling = 1.3F;
            this.extCheckBoxSystemShow.Name = "extCheckBoxSystemShow";
            this.extCheckBoxSystemShow.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxSystemShow.TabIndex = 33;
            this.extCheckBoxSystemShow.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxSystemShow, "Select another system to view");
            this.extCheckBoxSystemShow.UseVisualStyleBackColor = false;
            // 
            // scanDisplayBodyFiltersButton
            // 
            this.scanDisplayBodyFiltersButton.BackColor2 = System.Drawing.Color.Red;
            this.scanDisplayBodyFiltersButton.ButtonDisabledScaling = 0.5F;
            this.scanDisplayBodyFiltersButton.GradientDirection = 90F;
            this.scanDisplayBodyFiltersButton.Image = global::EDDiscovery.Icons.Controls.EventFilter;
            this.scanDisplayBodyFiltersButton.Location = new System.Drawing.Point(40, 51);
            this.scanDisplayBodyFiltersButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.scanDisplayBodyFiltersButton.MouseOverScaling = 1.3F;
            this.scanDisplayBodyFiltersButton.MouseSelectedScaling = 1.3F;
            this.scanDisplayBodyFiltersButton.Name = "scanDisplayBodyFiltersButton";
            this.scanDisplayBodyFiltersButton.SettingsSplittingChar = ';';
            this.scanDisplayBodyFiltersButton.Size = new System.Drawing.Size(28, 28);
            this.scanDisplayBodyFiltersButton.TabIndex = 32;
            this.toolTip.SetToolTip(this.scanDisplayBodyFiltersButton, "Configure scan display body filters");
            this.scanDisplayBodyFiltersButton.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 250;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ShowAlways = true;
            // 
            // scanDisplayConfigureButton
            // 
            this.scanDisplayConfigureButton.BackColor2 = System.Drawing.Color.Red;
            this.scanDisplayConfigureButton.ButtonDisabledScaling = 0.5F;
            this.scanDisplayConfigureButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.scanDisplayConfigureButton.GradientDirection = 90F;
            this.scanDisplayConfigureButton.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.scanDisplayConfigureButton.Location = new System.Drawing.Point(76, 51);
            this.scanDisplayConfigureButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.scanDisplayConfigureButton.MouseOverScaling = 1.3F;
            this.scanDisplayConfigureButton.MouseSelectedScaling = 1.3F;
            this.scanDisplayConfigureButton.Name = "scanDisplayConfigureButton";
            this.scanDisplayConfigureButton.SettingsSplittingChar = ';';
            this.scanDisplayConfigureButton.Size = new System.Drawing.Size(28, 28);
            this.scanDisplayConfigureButton.TabIndex = 31;
            this.toolTip1.SetToolTip(this.scanDisplayConfigureButton, "Configure scan display");
            this.scanDisplayConfigureButton.UseVisualStyleBackColor = true;
            // 
            // extPanelGradientFill1
            // 
            this.extPanelGradientFill1.ChildrenThemed = true;
            this.extPanelGradientFill1.Controls.Add(this.extCheckBoxSystemShow);
            this.extPanelGradientFill1.Controls.Add(this.labelDataName);
            this.extPanelGradientFill1.Controls.Add(this.labelDataPosition);
            this.extPanelGradientFill1.Controls.Add(this.scanDisplayBodyFiltersButton);
            this.extPanelGradientFill1.Controls.Add(this.scanDisplayConfigureButton);
            this.extPanelGradientFill1.Controls.Add(this.labelDataGov);
            this.extPanelGradientFill1.Controls.Add(this.labelDataFaction);
            this.extPanelGradientFill1.Controls.Add(this.extLabelClaimReleased);
            this.extPanelGradientFill1.Controls.Add(this.extLabelBeaconDeployed);
            this.extPanelGradientFill1.Controls.Add(this.edsmSpanshButton);
            this.extPanelGradientFill1.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelGradientFill1.FlowDirection = null;
            this.extPanelGradientFill1.GradientDirection = 0F;
            this.extPanelGradientFill1.Location = new System.Drawing.Point(0, 0);
            this.extPanelGradientFill1.Name = "extPanelGradientFill1";
            this.extPanelGradientFill1.PaintTransparentColor = System.Drawing.Color.Transparent;
            this.extPanelGradientFill1.Size = new System.Drawing.Size(973, 112);
            this.extPanelGradientFill1.TabIndex = 3;
            this.extPanelGradientFill1.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.extPanelGradientFill1.ThemeColorSet = -1;
            // 
            // labelDataPosition
            // 
            this.labelDataPosition.BorderColor = System.Drawing.Color.Orange;
            this.labelDataPosition.BorderWidth = 1;
            this.labelDataPosition.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataPosition.Data = null;
            this.labelDataPosition.DataFont = null;
            this.labelDataPosition.InterSpacing = 4;
            this.labelDataPosition.Location = new System.Drawing.Point(325, 3);
            this.labelDataPosition.Name = "labelDataPosition";
            this.labelDataPosition.NoDataText = null;
            this.labelDataPosition.Size = new System.Drawing.Size(315, 23);
            this.labelDataPosition.TabIndex = 1;
            this.labelDataPosition.TabSpacingData = 0;
            this.labelDataPosition.Text = "Position: X:{N4}, Y:{N4}, Z:{N4}";
            // 
            // labelDataGov
            // 
            this.labelDataGov.BorderColor = System.Drawing.Color.Orange;
            this.labelDataGov.BorderWidth = 1;
            this.labelDataGov.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataGov.Data = null;
            this.labelDataGov.DataFont = null;
            this.labelDataGov.InterSpacing = 4;
            this.labelDataGov.Location = new System.Drawing.Point(325, 28);
            this.labelDataGov.Name = "labelDataGov";
            this.labelDataGov.NoDataText = null;
            this.labelDataGov.Size = new System.Drawing.Size(410, 23);
            this.labelDataGov.TabIndex = 1;
            this.labelDataGov.TabSpacingData = 0;
            this.labelDataGov.Text = "Gov: {} Allegiance: {} Economy: {} Sec: {}";
            // 
            // labelDataFaction
            // 
            this.labelDataFaction.BorderColor = System.Drawing.Color.Orange;
            this.labelDataFaction.BorderWidth = 1;
            this.labelDataFaction.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataFaction.Data = null;
            this.labelDataFaction.DataFont = null;
            this.labelDataFaction.InterSpacing = 4;
            this.labelDataFaction.Location = new System.Drawing.Point(4, 27);
            this.labelDataFaction.Name = "labelDataFaction";
            this.labelDataFaction.NoDataText = null;
            this.labelDataFaction.Size = new System.Drawing.Size(315, 23);
            this.labelDataFaction.TabIndex = 1;
            this.labelDataFaction.TabSpacingData = 0;
            this.labelDataFaction.Text = "System Faction: {} State: {}";
            // 
            // extLabelClaimReleased
            // 
            this.extLabelClaimReleased.AutoSize = true;
            this.extLabelClaimReleased.Location = new System.Drawing.Point(446, 54);
            this.extLabelClaimReleased.Name = "extLabelClaimReleased";
            this.extLabelClaimReleased.Size = new System.Drawing.Size(80, 13);
            this.extLabelClaimReleased.TabIndex = 0;
            this.extLabelClaimReleased.Text = "Claim Released";
            // 
            // extLabelBeaconDeployed
            // 
            this.extLabelBeaconDeployed.AutoSize = true;
            this.extLabelBeaconDeployed.Location = new System.Drawing.Point(322, 54);
            this.extLabelBeaconDeployed.Name = "extLabelBeaconDeployed";
            this.extLabelBeaconDeployed.Size = new System.Drawing.Size(92, 13);
            this.extLabelBeaconDeployed.TabIndex = 0;
            this.extLabelBeaconDeployed.Text = "Beacon Deployed";
            // 
            // edsmSpanshButton
            // 
            this.edsmSpanshButton.BackColor2 = System.Drawing.Color.Red;
            this.edsmSpanshButton.ButtonDisabledScaling = 0.5F;
            this.edsmSpanshButton.GradientDirection = 90F;
            this.edsmSpanshButton.Image = global::EDDiscovery.Icons.Controls.EDSMSpansh;
            this.edsmSpanshButton.Location = new System.Drawing.Point(112, 51);
            this.edsmSpanshButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.edsmSpanshButton.MouseOverScaling = 1.3F;
            this.edsmSpanshButton.MouseSelectedScaling = 1.3F;
            this.edsmSpanshButton.Name = "edsmSpanshButton";
            this.edsmSpanshButton.SettingsSplittingChar = ';';
            this.edsmSpanshButton.Size = new System.Drawing.Size(28, 28);
            this.edsmSpanshButton.TabIndex = 30;
            this.edsmSpanshButton.UseVisualStyleBackColor = true;
            // 
            // scanDisplayUserControl
            // 
            this.scanDisplayUserControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.scanDisplayUserControl.Location = new System.Drawing.Point(0, 112);
            this.scanDisplayUserControl.Name = "scanDisplayUserControl";
            this.scanDisplayUserControl.Size = new System.Drawing.Size(973, 361);
            systemDisplay1.BackColor = System.Drawing.Color.Black;
            systemDisplay1.ContextMenuStripBelts = null;
            systemDisplay1.ContextMenuStripMats = null;
            systemDisplay1.ContextMenuStripPlanetsMoons = null;
            systemDisplay1.ContextMenuStripSignals = null;
            systemDisplay1.ContextMenuStripStars = null;
            systemDisplay1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            systemDisplay1.FontUnderlined = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            systemDisplay1.HideFullMaterials = false;
            systemDisplay1.LabelColor = System.Drawing.Color.DarkOrange;
            systemDisplay1.LargerFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            systemDisplay1.NoPlanetStarsOnSameLine = true;
            systemDisplay1.ShowAllG = true;
            systemDisplay1.ShowDist = true;
            systemDisplay1.ShowHabZone = true;
            systemDisplay1.ShowMaterials = true;
            systemDisplay1.ShowMoons = true;
            systemDisplay1.ShowOnlyMaterialsRare = false;
            systemDisplay1.ShowOverlays = true;
            systemDisplay1.ShowPlanetClasses = true;
            systemDisplay1.ShowPlanetMass = true;
            systemDisplay1.ShowStarAge = true;
            systemDisplay1.ShowStarClasses = true;
            systemDisplay1.ShowStarMass = true;
            systemDisplay1.ShowWebBodies = false;
            systemDisplay1.ValueLimit = 50000;
            this.scanDisplayUserControl.SystemDisplay = systemDisplay1;
            this.scanDisplayUserControl.TabIndex = 2;
            // 
            // labelDataName
            // 
            this.labelDataName.BorderColor = System.Drawing.Color.Orange;
            this.labelDataName.BorderWidth = 1;
            this.labelDataName.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataName.Data = null;
            this.labelDataName.DataFont = null;
            this.labelDataName.InterSpacing = 4;
            this.labelDataName.Location = new System.Drawing.Point(4, 3);
            this.labelDataName.Name = "labelDataName";
            this.labelDataName.NoDataText = null;
            this.labelDataName.Size = new System.Drawing.Size(315, 23);
            this.labelDataName.TabIndex = 1;
            this.labelDataName.TabSpacingData = 0;
            this.labelDataName.Text = "Name: {}";
            // 
            // ColonisationSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.scanDisplayUserControl);
            this.Controls.Add(this.extPanelGradientFill1);
            this.Name = "ColonisationSystem";
            this.Size = new System.Drawing.Size(973, 473);
            this.extPanelGradientFill1.ResumeLayout(false);
            this.extPanelGradientFill1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.LabelData labelDataPosition;
        private ExtendedControls.ExtLabel extLabelClaimReleased;
        private ExtendedControls.ExtLabel extLabelBeaconDeployed;
        private ExtendedControls.LabelData labelDataFaction;
        private ScanDisplayUserControl scanDisplayUserControl;
        private ExtendedControls.ExtPanelGradientFill extPanelGradientFill1;
        private EDSMSpanshButton edsmSpanshButton;
        private ScanDisplayBodyFiltersButton scanDisplayBodyFiltersButton;
        private System.Windows.Forms.ToolTip toolTip;
        private ScanDisplayConfigureButton scanDisplayConfigureButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private ExtendedControls.ExtCheckBox extCheckBoxSystemShow;
        private ExtendedControls.LabelData labelDataGov;
        private ExtendedControls.LabelData labelDataName;
    }
}
