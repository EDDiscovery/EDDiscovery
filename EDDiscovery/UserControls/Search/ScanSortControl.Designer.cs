
namespace EDDiscovery.UserControls.Search
{
    partial class ScanSortControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanSortControl));
            this.labelSort = new System.Windows.Forms.Label();
            this.extTextBoxSortCondition = new ExtendedControls.ExtTextBoxAutoComplete();
            this.extCheckBoxAscending = new ExtendedControls.ExtCheckBox();
            this.SuspendLayout();
            // 
            // labelSort
            // 
            this.labelSort.AutoSize = true;
            this.labelSort.Location = new System.Drawing.Point(4, 5);
            this.labelSort.Name = "labelSort";
            this.labelSort.Size = new System.Drawing.Size(29, 13);
            this.labelSort.TabIndex = 0;
            this.labelSort.Text = "Sort:";
            // 
            // extTextBoxSortCondition
            // 
            this.extTextBoxSortCondition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.extTextBoxSortCondition.AutoCompleteCommentMarker = null;
            this.extTextBoxSortCondition.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.extTextBoxSortCondition.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.extTextBoxSortCondition.BackErrorColor = System.Drawing.Color.Red;
            this.extTextBoxSortCondition.BorderColor = System.Drawing.Color.Transparent;
            this.extTextBoxSortCondition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.extTextBoxSortCondition.ClearOnFirstChar = false;
            this.extTextBoxSortCondition.ControlBackground = System.Drawing.SystemColors.Control;
            this.extTextBoxSortCondition.EndButtonEnable = true;
            this.extTextBoxSortCondition.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("extTextBoxSortCondition.EndButtonImage")));
            this.extTextBoxSortCondition.EndButtonVisible = true;
            this.extTextBoxSortCondition.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extTextBoxSortCondition.InErrorCondition = false;
            this.extTextBoxSortCondition.Location = new System.Drawing.Point(57, 2);
            this.extTextBoxSortCondition.Multiline = false;
            this.extTextBoxSortCondition.Name = "extTextBoxSortCondition";
            this.extTextBoxSortCondition.ReadOnly = false;
            this.extTextBoxSortCondition.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.extTextBoxSortCondition.SelectionLength = 0;
            this.extTextBoxSortCondition.SelectionStart = 0;
            this.extTextBoxSortCondition.Size = new System.Drawing.Size(787, 23);
            this.extTextBoxSortCondition.TabIndex = 1;
            this.extTextBoxSortCondition.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.extTextBoxSortCondition.TextChangedEvent = "";
            this.extTextBoxSortCondition.WordWrap = true;
            // 
            // extCheckBoxAscending
            // 
            this.extCheckBoxAscending.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extCheckBoxAscending.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxAscending.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxAscending.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxAscending.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxAscending.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxAscending.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxAscending.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxAscending.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxAscending.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxAscending.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxAscending.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxAscending.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxAscending.Image = global::EDDiscovery.Icons.Controls.RoundUp;
            this.extCheckBoxAscending.ImageIndeterminate = null;
            this.extCheckBoxAscending.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxAscending.ImageUnchecked = global::EDDiscovery.Icons.Controls.RoundDown;
            this.extCheckBoxAscending.Location = new System.Drawing.Point(863, 0);
            this.extCheckBoxAscending.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.extCheckBoxAscending.Name = "extCheckBoxAscending";
            this.extCheckBoxAscending.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxAscending.TabIndex = 31;
            this.extCheckBoxAscending.TickBoxReductionRatio = 0.75F;
            this.extCheckBoxAscending.UseVisualStyleBackColor = false;
            // 
            // ScanSortControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extCheckBoxAscending);
            this.Controls.Add(this.extTextBoxSortCondition);
            this.Controls.Add(this.labelSort);
            this.Name = "ScanSortControl";
            this.Size = new System.Drawing.Size(900, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSort;
        private ExtendedControls.ExtTextBoxAutoComplete extTextBoxSortCondition;
        private ExtendedControls.ExtCheckBox extCheckBoxAscending;
    }
}
