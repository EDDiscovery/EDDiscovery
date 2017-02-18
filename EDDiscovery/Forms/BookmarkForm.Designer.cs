﻿/*
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
namespace EDDiscovery2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BookmarkForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelBookmarkNotes = new System.Windows.Forms.Label();
            this.labelTimeMade = new System.Windows.Forms.Label();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.textBoxZ = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxTime = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.labelTravelNote = new System.Windows.Forms.Label();
            this.labelTravelNoteEdit = new System.Windows.Forms.Label();
            this.textBoxTravelNote = new System.Windows.Forms.TextBox();
            this.checkBoxTarget = new System.Windows.Forms.CheckBox();
            this.buttonEDSM = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Co-ordinates";
            // 
            // labelBookmarkNotes
            // 
            this.labelBookmarkNotes.AutoSize = true;
            this.labelBookmarkNotes.Location = new System.Drawing.Point(13, 147);
            this.labelBookmarkNotes.Name = "labelBookmarkNotes";
            this.labelBookmarkNotes.Size = new System.Drawing.Size(86, 13);
            this.labelBookmarkNotes.TabIndex = 0;
            this.labelBookmarkNotes.Text = "Bookmark Notes";
            // 
            // labelTimeMade
            // 
            this.labelTimeMade.AutoSize = true;
            this.labelTimeMade.Location = new System.Drawing.Point(13, 76);
            this.labelTimeMade.Name = "labelTimeMade";
            this.labelTimeMade.Size = new System.Drawing.Size(60, 13);
            this.labelTimeMade.TabIndex = 0;
            this.labelTimeMade.Text = "Time Made";
            // 
            // textBoxNotes
            // 
            this.textBoxNotes.Location = new System.Drawing.Point(116, 147);
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.Name = "textBoxNotes";
            this.textBoxNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxNotes.Size = new System.Drawing.Size(336, 115);
            this.textBoxNotes.TabIndex = 0;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(116, 13);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.ReadOnly = true;
            this.textBoxName.Size = new System.Drawing.Size(246, 20);
            this.textBoxName.TabIndex = 3;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(116, 43);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.ReadOnly = true;
            this.textBoxX.Size = new System.Drawing.Size(54, 20);
            this.textBoxX.TabIndex = 4;
            this.textBoxX.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxY
            // 
            this.textBoxY.Location = new System.Drawing.Point(209, 43);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.ReadOnly = true;
            this.textBoxY.Size = new System.Drawing.Size(54, 20);
            this.textBoxY.TabIndex = 5;
            this.textBoxY.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxZ
            // 
            this.textBoxZ.Location = new System.Drawing.Point(308, 43);
            this.textBoxZ.Name = "textBoxZ";
            this.textBoxZ.ReadOnly = true;
            this.textBoxZ.Size = new System.Drawing.Size(54, 20);
            this.textBoxZ.TabIndex = 6;
            this.textBoxZ.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(95, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "x:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(188, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "y:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(287, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "z:";
            // 
            // textBoxTime
            // 
            this.textBoxTime.Location = new System.Drawing.Point(116, 73);
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.ReadOnly = true;
            this.textBoxTime.Size = new System.Drawing.Size(246, 20);
            this.textBoxTime.TabIndex = 7;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(377, 361);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(275, 361);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(17, 361);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 9;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // labelTravelNote
            // 
            this.labelTravelNote.AutoSize = true;
            this.labelTravelNote.Location = new System.Drawing.Point(13, 280);
            this.labelTravelNote.Name = "labelTravelNote";
            this.labelTravelNote.Size = new System.Drawing.Size(98, 13);
            this.labelTravelNote.TabIndex = 0;
            this.labelTravelNote.Text = "Travel History Note";
            // 
            // labelTravelNoteEdit
            // 
            this.labelTravelNoteEdit.Location = new System.Drawing.Point(13, 305);
            this.labelTravelNoteEdit.Name = "labelTravelNoteEdit";
            this.labelTravelNoteEdit.Size = new System.Drawing.Size(79, 38);
            this.labelTravelNoteEdit.TabIndex = 0;
            this.labelTravelNoteEdit.Text = "(Edit on Travel Screen)";
            // 
            // textBoxTravelNote
            // 
            this.textBoxTravelNote.Location = new System.Drawing.Point(116, 280);
            this.textBoxTravelNote.Multiline = true;
            this.textBoxTravelNote.Name = "textBoxTravelNote";
            this.textBoxTravelNote.ReadOnly = true;
            this.textBoxTravelNote.Size = new System.Drawing.Size(336, 63);
            this.textBoxTravelNote.TabIndex = 8;
            // 
            // checkBoxTarget
            // 
            this.checkBoxTarget.AutoSize = true;
            this.checkBoxTarget.Location = new System.Drawing.Point(275, 112);
            this.checkBoxTarget.Name = "checkBoxTarget";
            this.checkBoxTarget.Size = new System.Drawing.Size(87, 17);
            this.checkBoxTarget.TabIndex = 10;
            this.checkBoxTarget.Text = "Make Target";
            this.checkBoxTarget.UseVisualStyleBackColor = true;
            // 
            // buttonEDSM
            // 
            this.buttonEDSM.Location = new System.Drawing.Point(116, 106);
            this.buttonEDSM.Name = "buttonEDSM";
            this.buttonEDSM.Size = new System.Drawing.Size(75, 23);
            this.buttonEDSM.TabIndex = 11;
            this.buttonEDSM.Text = "EDSM";
            this.buttonEDSM.UseVisualStyleBackColor = true;
            this.buttonEDSM.Click += new System.EventHandler(this.buttonEDSM_Click);
            // 
            // BookmarkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 400);
            this.Controls.Add(this.buttonEDSM);
            this.Controls.Add(this.checkBoxTarget);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxZ);
            this.Controls.Add(this.textBoxY);
            this.Controls.Add(this.textBoxX);
            this.Controls.Add(this.textBoxTime);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.textBoxTravelNote);
            this.Controls.Add(this.textBoxNotes);
            this.Controls.Add(this.labelTimeMade);
            this.Controls.Add(this.labelTravelNoteEdit);
            this.Controls.Add(this.labelTravelNote);
            this.Controls.Add(this.labelBookmarkNotes);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BookmarkForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create New Bookmark";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelBookmarkNotes;
        private System.Windows.Forms.Label labelTimeMade;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.TextBox textBoxZ;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxTime;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Label labelTravelNote;
        private System.Windows.Forms.Label labelTravelNoteEdit;
        private System.Windows.Forms.TextBox textBoxTravelNote;
        private System.Windows.Forms.CheckBox checkBoxTarget;
        private System.Windows.Forms.Button buttonEDSM;
    }
}