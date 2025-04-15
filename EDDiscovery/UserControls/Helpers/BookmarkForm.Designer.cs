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
namespace EDDiscovery.UserControls
{
    partial class BookmarkForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BookmarkForm));
            this.labelName = new System.Windows.Forms.Label();
            this.labelBookmarkNotes = new System.Windows.Forms.Label();
            this.labelTimeMade = new System.Windows.Forms.Label();
            this.textBoxBookmarkNotes = new ExtendedControls.ExtRichTextBox();
            this.textBoxX = new ExtendedControls.ExtTextBox();
            this.textBoxY = new ExtendedControls.ExtTextBox();
            this.textBoxZ = new ExtendedControls.ExtTextBox();
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelZ = new System.Windows.Forms.Label();
            this.textBoxTime = new ExtendedControls.ExtTextBox();
            this.buttonOK = new ExtendedControls.ExtButton();
            this.buttonDelete = new ExtendedControls.ExtButton();
            this.labelTravelNote = new System.Windows.Forms.Label();
            this.textBoxTravelNote = new ExtendedControls.ExtRichTextBox();
            this.checkBoxTarget = new ExtendedControls.ExtCheckBox();
            this.labelBadSystem = new System.Windows.Forms.Label();
            this.textBoxName = new ExtendedControls.ExtTextBoxAutoComplete();
            this.statusStripCustom = new ExtendedControls.ExtStatusStrip();
            this.extPanelScroll = new ExtendedControls.ExtPanelScroll();
            this.extScrollBar1 = new ExtendedControls.ExtScrollBar();
            this.extButtonSpanshSystem = new ExtendedControls.ExtButton();
            this.extButtonInaraSystem = new ExtendedControls.ExtButton();
            this.extButtonEDSMSystem = new ExtendedControls.ExtButton();
            this.panelTags = new System.Windows.Forms.Panel();
            this.SurfaceBookmarks = new EDDiscovery.UserControls.SurfaceBookmarkUserControl();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extButtonDrawnClose = new ExtendedControls.ExtButtonDrawn();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelBotButtons = new System.Windows.Forms.Panel();
            this.extPanelScroll.SuspendLayout();
            this.panelOuter.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelBotButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(9, 18);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name";
            // 
            // labelBookmarkNotes
            // 
            this.labelBookmarkNotes.Location = new System.Drawing.Point(9, 173);
            this.labelBookmarkNotes.Name = "labelBookmarkNotes";
            this.labelBookmarkNotes.Size = new System.Drawing.Size(121, 69);
            this.labelBookmarkNotes.TabIndex = 0;
            this.labelBookmarkNotes.Text = "Bookmark Notes";
            // 
            // labelTimeMade
            // 
            this.labelTimeMade.AutoSize = true;
            this.labelTimeMade.Location = new System.Drawing.Point(9, 138);
            this.labelTimeMade.Name = "labelTimeMade";
            this.labelTimeMade.Size = new System.Drawing.Size(60, 13);
            this.labelTimeMade.TabIndex = 0;
            this.labelTimeMade.Text = "Time Made";
            // 
            // textBoxBookmarkNotes
            // 
            this.textBoxBookmarkNotes.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBookmarkNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBookmarkNotes.DetectUrls = true;
            this.textBoxBookmarkNotes.HideScrollBar = true;
            this.textBoxBookmarkNotes.Location = new System.Drawing.Point(139, 175);
            this.textBoxBookmarkNotes.Name = "textBoxBookmarkNotes";
            this.textBoxBookmarkNotes.ReadOnly = false;
            this.textBoxBookmarkNotes.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 " +
    "Microsoft Sans Serif;}}\r\n{\\*\\generator Riched20 10.0.19041}\\viewkind4\\uc1 \r\n\\par" +
    "d\\f0\\fs17\\par\r\n}\r\n";
            this.textBoxBookmarkNotes.ShowLineCount = false;
            this.textBoxBookmarkNotes.Size = new System.Drawing.Size(726, 103);
            this.textBoxBookmarkNotes.TabIndex = 0;
            this.textBoxBookmarkNotes.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.textBoxBookmarkNotes.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // textBoxX
            // 
            this.textBoxX.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxX.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxX.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxX.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxX.ClearOnFirstChar = false;
            this.textBoxX.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxX.EndButtonEnable = true;
            this.textBoxX.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxX.EndButtonImage")));
            this.textBoxX.EndButtonSize16ths = 10;
            this.textBoxX.EndButtonVisible = false;
            this.textBoxX.InErrorCondition = false;
            this.textBoxX.Location = new System.Drawing.Point(139, 48);
            this.textBoxX.Multiline = false;
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.ReadOnly = true;
            this.textBoxX.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxX.SelectionLength = 0;
            this.textBoxX.SelectionStart = 0;
            this.textBoxX.Size = new System.Drawing.Size(157, 20);
            this.textBoxX.TabIndex = 4;
            this.textBoxX.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxX.TextNoChange = "";
            this.textBoxX.WordWrap = true;
            this.textBoxX.TextChanged += new System.EventHandler(this.textBox_TextNumChanged);
            // 
            // textBoxY
            // 
            this.textBoxY.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxY.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxY.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxY.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxY.ClearOnFirstChar = false;
            this.textBoxY.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxY.EndButtonEnable = true;
            this.textBoxY.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxY.EndButtonImage")));
            this.textBoxY.EndButtonSize16ths = 10;
            this.textBoxY.EndButtonVisible = false;
            this.textBoxY.InErrorCondition = false;
            this.textBoxY.Location = new System.Drawing.Point(139, 77);
            this.textBoxY.Multiline = false;
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.ReadOnly = true;
            this.textBoxY.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxY.SelectionLength = 0;
            this.textBoxY.SelectionStart = 0;
            this.textBoxY.Size = new System.Drawing.Size(157, 20);
            this.textBoxY.TabIndex = 5;
            this.textBoxY.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxY.TextNoChange = "";
            this.textBoxY.WordWrap = true;
            this.textBoxY.TextChanged += new System.EventHandler(this.textBox_TextNumChanged);
            // 
            // textBoxZ
            // 
            this.textBoxZ.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxZ.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxZ.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxZ.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxZ.ClearOnFirstChar = false;
            this.textBoxZ.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxZ.EndButtonEnable = true;
            this.textBoxZ.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxZ.EndButtonImage")));
            this.textBoxZ.EndButtonSize16ths = 10;
            this.textBoxZ.EndButtonVisible = false;
            this.textBoxZ.InErrorCondition = false;
            this.textBoxZ.Location = new System.Drawing.Point(139, 106);
            this.textBoxZ.Multiline = false;
            this.textBoxZ.Name = "textBoxZ";
            this.textBoxZ.ReadOnly = true;
            this.textBoxZ.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxZ.SelectionLength = 0;
            this.textBoxZ.SelectionStart = 0;
            this.textBoxZ.Size = new System.Drawing.Size(157, 20);
            this.textBoxZ.TabIndex = 6;
            this.textBoxZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxZ.TextNoChange = "";
            this.textBoxZ.WordWrap = true;
            this.textBoxZ.TextChanged += new System.EventHandler(this.textBox_TextNumChanged);
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(9, 51);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(15, 13);
            this.labelX.TabIndex = 0;
            this.labelX.Text = "x:";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(8, 80);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(15, 13);
            this.labelY.TabIndex = 0;
            this.labelY.Text = "y:";
            // 
            // labelZ
            // 
            this.labelZ.AutoSize = true;
            this.labelZ.Location = new System.Drawing.Point(8, 107);
            this.labelZ.Name = "labelZ";
            this.labelZ.Size = new System.Drawing.Size(15, 13);
            this.labelZ.TabIndex = 0;
            this.labelZ.Text = "z:";
            // 
            // textBoxTime
            // 
            this.textBoxTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxTime.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxTime.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTime.ClearOnFirstChar = false;
            this.textBoxTime.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxTime.EndButtonEnable = true;
            this.textBoxTime.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxTime.EndButtonImage")));
            this.textBoxTime.EndButtonSize16ths = 10;
            this.textBoxTime.EndButtonVisible = false;
            this.textBoxTime.InErrorCondition = false;
            this.textBoxTime.Location = new System.Drawing.Point(139, 136);
            this.textBoxTime.Multiline = false;
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.ReadOnly = true;
            this.textBoxTime.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxTime.SelectionLength = 0;
            this.textBoxTime.SelectionStart = 0;
            this.textBoxTime.Size = new System.Drawing.Size(246, 20);
            this.textBoxTime.TabIndex = 7;
            this.textBoxTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxTime.TextNoChange = "";
            this.textBoxTime.WordWrap = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(766, 8);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 24);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "%OK%";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(10, 8);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(100, 24);
            this.buttonDelete.TabIndex = 9;
            this.buttonDelete.Text = "%Delete%";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // labelTravelNote
            // 
            this.labelTravelNote.Location = new System.Drawing.Point(6, 289);
            this.labelTravelNote.Name = "labelTravelNote";
            this.labelTravelNote.Size = new System.Drawing.Size(124, 49);
            this.labelTravelNote.TabIndex = 0;
            this.labelTravelNote.Text = "Travel History Note";
            // 
            // textBoxTravelNote
            // 
            this.textBoxTravelNote.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTravelNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTravelNote.DetectUrls = true;
            this.textBoxTravelNote.HideScrollBar = true;
            this.textBoxTravelNote.Location = new System.Drawing.Point(139, 291);
            this.textBoxTravelNote.Name = "textBoxTravelNote";
            this.textBoxTravelNote.ReadOnly = true;
            this.textBoxTravelNote.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 " +
    "Microsoft Sans Serif;}}\r\n{\\*\\generator Riched20 10.0.19041}\\viewkind4\\uc1 \r\n\\par" +
    "d\\f0\\fs17\\par\r\n}\r\n";
            this.textBoxTravelNote.ShowLineCount = false;
            this.textBoxTravelNote.Size = new System.Drawing.Size(726, 87);
            this.textBoxTravelNote.TabIndex = 8;
            this.textBoxTravelNote.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.textBoxTravelNote.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // checkBoxTarget
            // 
            this.checkBoxTarget.AutoSize = true;
            this.checkBoxTarget.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxTarget.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxTarget.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxTarget.ImageIndeterminate = null;
            this.checkBoxTarget.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxTarget.ImageUnchecked = null;
            this.checkBoxTarget.Location = new System.Drawing.Point(694, 24);
            this.checkBoxTarget.Name = "checkBoxTarget";
            this.checkBoxTarget.Size = new System.Drawing.Size(87, 17);
            this.checkBoxTarget.TabIndex = 10;
            this.checkBoxTarget.Text = "Make Target";
            this.checkBoxTarget.TickBoxReductionRatio = 0.75F;
            this.checkBoxTarget.UseVisualStyleBackColor = true;
            // 
            // labelBadSystem
            // 
            this.labelBadSystem.AutoSize = true;
            this.labelBadSystem.Location = new System.Drawing.Point(482, 24);
            this.labelBadSystem.Name = "labelBadSystem";
            this.labelBadSystem.Size = new System.Drawing.Size(0, 13);
            this.labelBadSystem.TabIndex = 14;
            // 
            // textBoxName
            // 
            this.textBoxName.AutoCompleteCommentMarker = null;
            this.textBoxName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxName.AutoCompleteTimeout = 500;
            this.textBoxName.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxName.ClearOnFirstChar = false;
            this.textBoxName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxName.EndButtonEnable = false;
            this.textBoxName.EndButtonSize16ths = 10;
            this.textBoxName.EndButtonVisible = false;
            this.textBoxName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxName.InErrorCondition = false;
            this.textBoxName.Location = new System.Drawing.Point(139, 18);
            this.textBoxName.Multiline = false;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.ReadOnly = false;
            this.textBoxName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxName.SelectionLength = 0;
            this.textBoxName.SelectionStart = 0;
            this.textBoxName.Size = new System.Drawing.Size(336, 20);
            this.textBoxName.TabIndex = 13;
            this.textBoxName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxName.TextChangedEvent = "";
            this.textBoxName.TextNoChange = "";
            this.textBoxName.WordWrap = true;
            // 
            // statusStripCustom
            // 
            this.statusStripCustom.Location = new System.Drawing.Point(0, 766);
            this.statusStripCustom.Name = "statusStripCustom";
            this.statusStripCustom.Size = new System.Drawing.Size(899, 22);
            this.statusStripCustom.TabIndex = 32;
            // 
            // extPanelScroll
            // 
            this.extPanelScroll.Controls.Add(this.extScrollBar1);
            this.extPanelScroll.Controls.Add(this.extButtonSpanshSystem);
            this.extPanelScroll.Controls.Add(this.extButtonInaraSystem);
            this.extPanelScroll.Controls.Add(this.extButtonEDSMSystem);
            this.extPanelScroll.Controls.Add(this.panelTags);
            this.extPanelScroll.Controls.Add(this.textBoxTravelNote);
            this.extPanelScroll.Controls.Add(this.textBoxBookmarkNotes);
            this.extPanelScroll.Controls.Add(this.labelTimeMade);
            this.extPanelScroll.Controls.Add(this.textBoxTime);
            this.extPanelScroll.Controls.Add(this.textBoxX);
            this.extPanelScroll.Controls.Add(this.labelTravelNote);
            this.extPanelScroll.Controls.Add(this.textBoxY);
            this.extPanelScroll.Controls.Add(this.labelBookmarkNotes);
            this.extPanelScroll.Controls.Add(this.textBoxZ);
            this.extPanelScroll.Controls.Add(this.labelZ);
            this.extPanelScroll.Controls.Add(this.labelY);
            this.extPanelScroll.Controls.Add(this.labelX);
            this.extPanelScroll.Controls.Add(this.checkBoxTarget);
            this.extPanelScroll.Controls.Add(this.labelName);
            this.extPanelScroll.Controls.Add(this.SurfaceBookmarks);
            this.extPanelScroll.Controls.Add(this.textBoxName);
            this.extPanelScroll.Controls.Add(this.labelBadSystem);
            this.extPanelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelScroll.FlowControlsLeftToRight = false;
            this.extPanelScroll.Location = new System.Drawing.Point(0, 0);
            this.extPanelScroll.Name = "extPanelScroll";
            this.extPanelScroll.Size = new System.Drawing.Size(897, 702);
            this.extPanelScroll.TabIndex = 15;
            this.extPanelScroll.VerticalScrollBarDockRight = true;
            this.extPanelScroll.MouseDown += new System.Windows.Forms.MouseEventHandler(this.extPanelScroll_MouseDown);
            this.extPanelScroll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.extPanelScroll_MouseUp);
            // 
            // extScrollBar1
            // 
            this.extScrollBar1.AlwaysHideScrollBar = false;
            this.extScrollBar1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBar1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBar1.ArrowDownDrawAngle = 270F;
            this.extScrollBar1.ArrowUpDrawAngle = 90F;
            this.extScrollBar1.BorderColor = System.Drawing.Color.White;
            this.extScrollBar1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBar1.HideScrollBar = false;
            this.extScrollBar1.LargeChange = 10;
            this.extScrollBar1.Location = new System.Drawing.Point(878, 0);
            this.extScrollBar1.Maximum = -12;
            this.extScrollBar1.Minimum = 0;
            this.extScrollBar1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBar1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBar1.Name = "extScrollBar1";
            this.extScrollBar1.Size = new System.Drawing.Size(19, 702);
            this.extScrollBar1.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBar1.SmallChange = 1;
            this.extScrollBar1.TabIndex = 15;
            this.extScrollBar1.Text = "extScrollBar1";
            this.extScrollBar1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBar1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBar1.ThumbDrawAngle = 0F;
            this.extScrollBar1.Value = -12;
            this.extScrollBar1.ValueLimited = -12;
            // 
            // extButtonSpanshSystem
            // 
            this.extButtonSpanshSystem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonSpanshSystem.Image = global::EDDiscovery.Icons.Controls.spansh;
            this.extButtonSpanshSystem.Location = new System.Drawing.Point(617, 16);
            this.extButtonSpanshSystem.Name = "extButtonSpanshSystem";
            this.extButtonSpanshSystem.Padding = new System.Windows.Forms.Padding(2);
            this.extButtonSpanshSystem.Size = new System.Drawing.Size(48, 28);
            this.extButtonSpanshSystem.TabIndex = 17;
            this.extButtonSpanshSystem.Click += new System.EventHandler(this.extButtonSpanshSystem_Click);
            // 
            // extButtonInaraSystem
            // 
            this.extButtonInaraSystem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonInaraSystem.Image = global::EDDiscovery.Icons.Controls.Inara;
            this.extButtonInaraSystem.Location = new System.Drawing.Point(551, 16);
            this.extButtonInaraSystem.Name = "extButtonInaraSystem";
            this.extButtonInaraSystem.Padding = new System.Windows.Forms.Padding(2);
            this.extButtonInaraSystem.Size = new System.Drawing.Size(48, 28);
            this.extButtonInaraSystem.TabIndex = 18;
            this.extButtonInaraSystem.Click += new System.EventHandler(this.extButtonInaraSystem_Click);
            // 
            // extButtonEDSMSystem
            // 
            this.extButtonEDSMSystem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonEDSMSystem.Image = global::EDDiscovery.Icons.Controls.EDSM;
            this.extButtonEDSMSystem.Location = new System.Drawing.Point(485, 16);
            this.extButtonEDSMSystem.Name = "extButtonEDSMSystem";
            this.extButtonEDSMSystem.Padding = new System.Windows.Forms.Padding(2);
            this.extButtonEDSMSystem.Size = new System.Drawing.Size(48, 28);
            this.extButtonEDSMSystem.TabIndex = 19;
            this.extButtonEDSMSystem.Click += new System.EventHandler(this.extButtonEDSMSystem_Click);
            // 
            // panelTags
            // 
            this.panelTags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTags.Location = new System.Drawing.Point(447, 131);
            this.panelTags.Name = "panelTags";
            this.panelTags.Size = new System.Drawing.Size(353, 32);
            this.panelTags.TabIndex = 16;
            this.panelTags.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTags_MouseDown);
            // 
            // SurfaceBookmarks
            // 
            this.SurfaceBookmarks.Location = new System.Drawing.Point(13, 392);
            this.SurfaceBookmarks.Name = "SurfaceBookmarks";
            this.SurfaceBookmarks.Size = new System.Drawing.Size(852, 289);
            this.SurfaceBookmarks.TabIndex = 12;
            this.SurfaceBookmarks.TagFilter = null;
            // 
            // extButtonDrawnClose
            // 
            this.extButtonDrawnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnClose.AutoEllipsis = false;
            this.extButtonDrawnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnClose.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnClose.BorderWidth = 1;
            this.extButtonDrawnClose.Image = null;
            this.extButtonDrawnClose.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Close;
            this.extButtonDrawnClose.Location = new System.Drawing.Point(872, 2);
            this.extButtonDrawnClose.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnClose.MouseSelectedColorEnable = true;
            this.extButtonDrawnClose.Name = "extButtonDrawnClose";
            this.extButtonDrawnClose.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnClose.Selectable = false;
            this.extButtonDrawnClose.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnClose.TabIndex = 26;
            this.extButtonDrawnClose.TabStop = false;
            this.extButtonDrawnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.extButtonDrawnClose, "Close");
            this.extButtonDrawnClose.UseMnemonic = true;
            this.extButtonDrawnClose.Click += new System.EventHandler(this.extButtonDrawnClose_Click);
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.extPanelScroll);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 28);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(899, 704);
            this.panelOuter.TabIndex = 17;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.labelTitle);
            this.panelTop.Controls.Add(this.extButtonDrawnClose);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(899, 28);
            this.panelTop.TabIndex = 17;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(4, 6);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(43, 13);
            this.labelTitle.TabIndex = 27;
            this.labelTitle.Text = "<code>";
            // 
            // panelBotButtons
            // 
            this.panelBotButtons.Controls.Add(this.buttonOK);
            this.panelBotButtons.Controls.Add(this.buttonDelete);
            this.panelBotButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBotButtons.Location = new System.Drawing.Point(0, 732);
            this.panelBotButtons.Name = "panelBotButtons";
            this.panelBotButtons.Size = new System.Drawing.Size(899, 34);
            this.panelBotButtons.TabIndex = 17;
            // 
            // BookmarkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 788);
            this.Controls.Add(this.panelOuter);
            this.Controls.Add(this.panelBotButtons);
            this.Controls.Add(this.statusStripCustom);
            this.Controls.Add(this.panelTop);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "BookmarkForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "<code>";
            this.extPanelScroll.ResumeLayout(false);
            this.extPanelScroll.PerformLayout();
            this.panelOuter.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBotButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelBookmarkNotes;
        private System.Windows.Forms.Label labelTimeMade;
        private ExtendedControls.ExtRichTextBox textBoxBookmarkNotes;
        private ExtendedControls.ExtTextBox textBoxX;
        private ExtendedControls.ExtTextBox textBoxY;
        private ExtendedControls.ExtTextBox textBoxZ;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelZ;
        private ExtendedControls.ExtTextBox textBoxTime;
        private ExtendedControls.ExtButton buttonOK;
        private ExtendedControls.ExtButton buttonDelete;
        private System.Windows.Forms.Label labelTravelNote;
        private ExtendedControls.ExtRichTextBox textBoxTravelNote;
        private ExtendedControls.ExtCheckBox checkBoxTarget;
        private SurfaceBookmarkUserControl SurfaceBookmarks;
        private ExtendedControls.ExtTextBoxAutoComplete textBoxName;
        private System.Windows.Forms.Label labelBadSystem;
        private ExtendedControls.ExtStatusStrip statusStripCustom;
        private ExtendedControls.ExtPanelScroll extPanelScroll;
        private ExtendedControls.ExtScrollBar extScrollBar1;
        private System.Windows.Forms.Panel panelTags;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelOuter;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBotButtons;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnClose;
        private System.Windows.Forms.Label labelTitle;
        private ExtendedControls.ExtButton extButtonSpanshSystem;
        private ExtendedControls.ExtButton extButtonInaraSystem;
        private ExtendedControls.ExtButton extButtonEDSMSystem;
    }
}
