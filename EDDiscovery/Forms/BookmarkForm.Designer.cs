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
            this.labelName = new System.Windows.Forms.Label();
            this.labelCo = new System.Windows.Forms.Label();
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
            this.buttonCancel = new ExtendedControls.ExtButton();
            this.buttonDelete = new ExtendedControls.ExtButton();
            this.labelTravelNote = new System.Windows.Forms.Label();
            this.labelTravelNoteEdit = new System.Windows.Forms.Label();
            this.textBoxTravelNote = new ExtendedControls.ExtTextBox();
            this.checkBoxTarget = new ExtendedControls.ExtCheckBox();
            this.buttonEDSM = new ExtendedControls.ExtButton();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.labelBadSystem = new System.Windows.Forms.Label();
            this.textBoxName = new ExtendedControls.ExtTextBoxAutoComplete();
            this.SurfaceBookmarks = new EDDiscovery.Forms.SurfaceBookmarkUserControl();
            this.panelOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(9, 16);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name";
            // 
            // labelCo
            // 
            this.labelCo.AutoSize = true;
            this.labelCo.Location = new System.Drawing.Point(9, 46);
            this.labelCo.Name = "labelCo";
            this.labelCo.Size = new System.Drawing.Size(66, 13);
            this.labelCo.TabIndex = 0;
            this.labelCo.Text = "Co-ordinates";
            // 
            // labelBookmarkNotes
            // 
            this.labelBookmarkNotes.Location = new System.Drawing.Point(9, 104);
            this.labelBookmarkNotes.Name = "labelBookmarkNotes";
            this.labelBookmarkNotes.Size = new System.Drawing.Size(121, 69);
            this.labelBookmarkNotes.TabIndex = 0;
            this.labelBookmarkNotes.Text = "Bookmark Notes";
            // 
            // labelTimeMade
            // 
            this.labelTimeMade.AutoSize = true;
            this.labelTimeMade.Location = new System.Drawing.Point(9, 76);
            this.labelTimeMade.Name = "labelTimeMade";
            this.labelTimeMade.Size = new System.Drawing.Size(60, 13);
            this.labelTimeMade.TabIndex = 0;
            this.labelTimeMade.Text = "Time Made";
            // 
            // textBoxBookmarkNotes
            // 
            this.textBoxBookmarkNotes.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBookmarkNotes.BorderColorScaling = 0.5F;
            this.textBoxBookmarkNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBookmarkNotes.HideScrollBar = true;
            this.textBoxBookmarkNotes.Location = new System.Drawing.Point(139, 104);
            this.textBoxBookmarkNotes.Name = "textBoxBookmarkNotes";
            this.textBoxBookmarkNotes.ReadOnly = false;
            this.textBoxBookmarkNotes.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.textBoxBookmarkNotes.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.textBoxBookmarkNotes.ScrollBarBackColor = System.Drawing.SystemColors.Control;
            this.textBoxBookmarkNotes.ScrollBarBorderColor = System.Drawing.Color.White;
            this.textBoxBookmarkNotes.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxBookmarkNotes.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxBookmarkNotes.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.textBoxBookmarkNotes.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.textBoxBookmarkNotes.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.textBoxBookmarkNotes.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.textBoxBookmarkNotes.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.textBoxBookmarkNotes.ScrollBarWidth = 20;
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
            this.textBoxX.BorderColorScaling = 0.5F;
            this.textBoxX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxX.ClearOnFirstChar = false;
            this.textBoxX.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxX.InErrorCondition = false;
            this.textBoxX.Location = new System.Drawing.Point(139, 43);
            this.textBoxX.Multiline = false;
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.ReadOnly = true;
            this.textBoxX.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxX.SelectionLength = 0;
            this.textBoxX.SelectionStart = 0;
            this.textBoxX.Size = new System.Drawing.Size(80, 20);
            this.textBoxX.TabIndex = 4;
            this.textBoxX.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxX.WordWrap = true;
            this.textBoxX.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxY
            // 
            this.textBoxY.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxY.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxY.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxY.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxY.BorderColorScaling = 0.5F;
            this.textBoxY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxY.ClearOnFirstChar = false;
            this.textBoxY.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxY.InErrorCondition = false;
            this.textBoxY.Location = new System.Drawing.Point(273, 43);
            this.textBoxY.Multiline = false;
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.ReadOnly = true;
            this.textBoxY.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxY.SelectionLength = 0;
            this.textBoxY.SelectionStart = 0;
            this.textBoxY.Size = new System.Drawing.Size(80, 20);
            this.textBoxY.TabIndex = 5;
            this.textBoxY.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxY.WordWrap = true;
            this.textBoxY.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxZ
            // 
            this.textBoxZ.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxZ.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxZ.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxZ.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxZ.BorderColorScaling = 0.5F;
            this.textBoxZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxZ.ClearOnFirstChar = false;
            this.textBoxZ.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxZ.InErrorCondition = false;
            this.textBoxZ.Location = new System.Drawing.Point(395, 43);
            this.textBoxZ.Multiline = false;
            this.textBoxZ.Name = "textBoxZ";
            this.textBoxZ.ReadOnly = true;
            this.textBoxZ.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxZ.SelectionLength = 0;
            this.textBoxZ.SelectionStart = 0;
            this.textBoxZ.Size = new System.Drawing.Size(80, 20);
            this.textBoxZ.TabIndex = 6;
            this.textBoxZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxZ.WordWrap = true;
            this.textBoxZ.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(115, 46);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(15, 13);
            this.labelX.TabIndex = 0;
            this.labelX.Text = "x:";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(248, 46);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(15, 13);
            this.labelY.TabIndex = 0;
            this.labelY.Text = "y:";
            // 
            // labelZ
            // 
            this.labelZ.AutoSize = true;
            this.labelZ.Location = new System.Drawing.Point(370, 46);
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
            this.textBoxTime.BorderColorScaling = 0.5F;
            this.textBoxTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTime.ClearOnFirstChar = false;
            this.textBoxTime.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxTime.InErrorCondition = false;
            this.textBoxTime.Location = new System.Drawing.Point(139, 73);
            this.textBoxTime.Multiline = false;
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.ReadOnly = true;
            this.textBoxTime.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxTime.SelectionLength = 0;
            this.textBoxTime.SelectionStart = 0;
            this.textBoxTime.Size = new System.Drawing.Size(246, 20);
            this.textBoxTime.TabIndex = 7;
            this.textBoxTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxTime.WordWrap = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(782, 527);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(80, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(691, 527);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDelete.Location = new System.Drawing.Point(13, 527);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 9;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // labelTravelNote
            // 
            this.labelTravelNote.Location = new System.Drawing.Point(6, 211);
            this.labelTravelNote.Name = "labelTravelNote";
            this.labelTravelNote.Size = new System.Drawing.Size(124, 49);
            this.labelTravelNote.TabIndex = 0;
            this.labelTravelNote.Text = "Travel History Note";
            // 
            // labelTravelNoteEdit
            // 
            this.labelTravelNoteEdit.Location = new System.Drawing.Point(9, 269);
            this.labelTravelNoteEdit.Name = "labelTravelNoteEdit";
            this.labelTravelNoteEdit.Size = new System.Drawing.Size(121, 46);
            this.labelTravelNoteEdit.TabIndex = 0;
            this.labelTravelNoteEdit.Text = "(Edit on Travel Screen)";
            // 
            // textBoxTravelNote
            // 
            this.textBoxTravelNote.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxTravelNote.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxTravelNote.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxTravelNote.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTravelNote.BorderColorScaling = 0.5F;
            this.textBoxTravelNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTravelNote.ClearOnFirstChar = false;
            this.textBoxTravelNote.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxTravelNote.InErrorCondition = false;
            this.textBoxTravelNote.Location = new System.Drawing.Point(139, 213);
            this.textBoxTravelNote.Multiline = true;
            this.textBoxTravelNote.Name = "textBoxTravelNote";
            this.textBoxTravelNote.ReadOnly = true;
            this.textBoxTravelNote.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxTravelNote.SelectionLength = 0;
            this.textBoxTravelNote.SelectionStart = 0;
            this.textBoxTravelNote.Size = new System.Drawing.Size(726, 87);
            this.textBoxTravelNote.TabIndex = 8;
            this.textBoxTravelNote.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxTravelNote.WordWrap = true;
            // 
            // checkBoxTarget
            // 
            this.checkBoxTarget.AutoSize = true;
            this.checkBoxTarget.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxTarget.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxTarget.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxTarget.FontNerfReduction = 0.5F;
            this.checkBoxTarget.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxTarget.Location = new System.Drawing.Point(584, 19);
            this.checkBoxTarget.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxTarget.Name = "checkBoxTarget";
            this.checkBoxTarget.Size = new System.Drawing.Size(87, 17);
            this.checkBoxTarget.TabIndex = 10;
            this.checkBoxTarget.Text = "Make Target";
            this.checkBoxTarget.TickBoxReductionSize = 10;
            this.checkBoxTarget.UseVisualStyleBackColor = true;
            // 
            // buttonEDSM
            // 
            this.buttonEDSM.Location = new System.Drawing.Point(488, 16);
            this.buttonEDSM.Name = "buttonEDSM";
            this.buttonEDSM.Size = new System.Drawing.Size(75, 23);
            this.buttonEDSM.TabIndex = 11;
            this.buttonEDSM.Text = "EDSM";
            this.buttonEDSM.UseVisualStyleBackColor = true;
            this.buttonEDSM.Click += new System.EventHandler(this.buttonEDSM_Click);
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.labelBadSystem);
            this.panelOuter.Controls.Add(this.textBoxName);
            this.panelOuter.Controls.Add(this.SurfaceBookmarks);
            this.panelOuter.Controls.Add(this.buttonEDSM);
            this.panelOuter.Controls.Add(this.labelName);
            this.panelOuter.Controls.Add(this.checkBoxTarget);
            this.panelOuter.Controls.Add(this.labelCo);
            this.panelOuter.Controls.Add(this.buttonDelete);
            this.panelOuter.Controls.Add(this.labelX);
            this.panelOuter.Controls.Add(this.buttonCancel);
            this.panelOuter.Controls.Add(this.labelY);
            this.panelOuter.Controls.Add(this.buttonOK);
            this.panelOuter.Controls.Add(this.labelZ);
            this.panelOuter.Controls.Add(this.textBoxZ);
            this.panelOuter.Controls.Add(this.labelBookmarkNotes);
            this.panelOuter.Controls.Add(this.textBoxY);
            this.panelOuter.Controls.Add(this.labelTravelNote);
            this.panelOuter.Controls.Add(this.textBoxX);
            this.panelOuter.Controls.Add(this.labelTravelNoteEdit);
            this.panelOuter.Controls.Add(this.textBoxTime);
            this.panelOuter.Controls.Add(this.labelTimeMade);
            this.panelOuter.Controls.Add(this.textBoxBookmarkNotes);
            this.panelOuter.Controls.Add(this.textBoxTravelNote);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 0);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(874, 564);
            this.panelOuter.TabIndex = 12;
            this.panelOuter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // labelBadSystem
            // 
            this.labelBadSystem.AutoSize = true;
            this.labelBadSystem.Location = new System.Drawing.Point(482, 22);
            this.labelBadSystem.Name = "labelBadSystem";
            this.labelBadSystem.Size = new System.Drawing.Size(0, 13);
            this.labelBadSystem.TabIndex = 14;
            // 
            // textBoxName
            // 
            this.textBoxName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxName.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxName.BorderColorScaling = 0.5F;
            this.textBoxName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxName.ClearOnFirstChar = false;
            this.textBoxName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxName.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.textBoxName.DropDownBorderColor = System.Drawing.Color.Green;
            this.textBoxName.DropDownHeight = 200;
            this.textBoxName.DropDownItemHeight = 13;
            this.textBoxName.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.textBoxName.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.textBoxName.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.textBoxName.DropDownWidth = 0;
            this.textBoxName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxName.InErrorCondition = false;
            this.textBoxName.Location = new System.Drawing.Point(139, 16);
            this.textBoxName.Multiline = false;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.ReadOnly = false;
            this.textBoxName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxName.SelectionLength = 0;
            this.textBoxName.SelectionStart = 0;
            this.textBoxName.Size = new System.Drawing.Size(336, 20);
            this.textBoxName.TabIndex = 13;
            this.textBoxName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxName.WordWrap = true;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // SurfaceBookmarks
            // 
            this.SurfaceBookmarks.Location = new System.Drawing.Point(13, 310);
            this.SurfaceBookmarks.Name = "SurfaceBookmarks";
            this.SurfaceBookmarks.Size = new System.Drawing.Size(852, 205);
            this.SurfaceBookmarks.TabIndex = 12;
            // 
            // BookmarkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 564);
            this.Controls.Add(this.panelOuter);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "BookmarkForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create New Bookmark";
            this.panelOuter.ResumeLayout(false);
            this.panelOuter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelCo;
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
        private ExtendedControls.ExtButton buttonCancel;
        private ExtendedControls.ExtButton buttonDelete;
        private System.Windows.Forms.Label labelTravelNote;
        private System.Windows.Forms.Label labelTravelNoteEdit;
        private ExtendedControls.ExtTextBox textBoxTravelNote;
        private ExtendedControls.ExtCheckBox checkBoxTarget;
        private ExtendedControls.ExtButton buttonEDSM;
        private System.Windows.Forms.Panel panelOuter;
        private SurfaceBookmarkUserControl SurfaceBookmarks;
        private ExtendedControls.ExtTextBoxAutoComplete textBoxName;
        private System.Windows.Forms.Label labelBadSystem;
    }
}