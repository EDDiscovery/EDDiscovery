namespace UnitTest
{
    partial class UnitTests
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
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.bindingsEditor = new EliteDangerousCore.BindingsEditor();
            this.panelTest = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.extButtonDrawn1 = new ExtendedControls.ExtButtonDrawn();
            this.imageViewer1 = new ExtendedControls.ImageViewer();
            this.panelTest.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Left;
            this.richTextBoxLog.Font = new System.Drawing.Font("Cascadia Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLog.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(1250, 74);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.Text = "";
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Location = new System.Drawing.Point(1397, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(80, 24);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // bindingsEditor
            // 
            this.bindingsEditor.ChangedBindings = null;
            this.bindingsEditor.ChangedDefault = null;
            this.bindingsEditor.DeviceInput = null;
            this.bindingsEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bindingsEditor.Location = new System.Drawing.Point(0, 98);
            this.bindingsEditor.Name = "bindingsEditor";
            this.bindingsEditor.Size = new System.Drawing.Size(1505, 895);
            this.bindingsEditor.TabIndex = 3;
            // 
            // panelTest
            // 
            this.panelTest.Controls.Add(this.imageViewer1);
            this.panelTest.Controls.Add(this.richTextBoxLog);
            this.panelTest.Controls.Add(this.buttonStart);
            this.panelTest.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTest.Location = new System.Drawing.Point(0, 24);
            this.panelTest.Name = "panelTest";
            this.panelTest.Size = new System.Drawing.Size(1505, 74);
            this.panelTest.TabIndex = 4;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.extButtonDrawn1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1505, 24);
            this.panelTop.TabIndex = 5;
            // 
            // extButtonDrawn1
            // 
            this.extButtonDrawn1.AutoEllipsis = false;
            this.extButtonDrawn1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawn1.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawn1.BorderWidth = 1;
            this.extButtonDrawn1.ButtonDisabledScaling = 0.25F;
            this.extButtonDrawn1.Image = null;
            this.extButtonDrawn1.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Close;
            this.extButtonDrawn1.Location = new System.Drawing.Point(1469, 4);
            this.extButtonDrawn1.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawn1.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawn1.MouseSelectedColorEnable = true;
            this.extButtonDrawn1.Name = "extButtonDrawn1";
            this.extButtonDrawn1.Selectable = true;
            this.extButtonDrawn1.Size = new System.Drawing.Size(18, 18);
            this.extButtonDrawn1.TabIndex = 0;
            this.extButtonDrawn1.Text = "extButtonDrawn1";
            this.extButtonDrawn1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawn1.UseMnemonic = true;
            this.extButtonDrawn1.Click += new System.EventHandler(this.extButtonDrawnClose_Click);
            // 
            // imageViewer1
            // 
            this.imageViewer1.Location = new System.Drawing.Point(1301, 14);
            this.imageViewer1.MaxZoom = 400;
            this.imageViewer1.MinZoom = 10;
            this.imageViewer1.Name = "imageViewer1";
            this.imageViewer1.Size = new System.Drawing.Size(496, 369);
            this.imageViewer1.TabIndex = 3;
            this.imageViewer1.ZoomIncrement = 10;
            // 
            // UnitTests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1505, 993);
            this.Controls.Add(this.bindingsEditor);
            this.Controls.Add(this.panelTest);
            this.Controls.Add(this.panelTop);
            this.Name = "UnitTests";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Unit Test";
            this.panelTest.ResumeLayout(false);
            this.panelTest.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonStart;
        private EliteDangerousCore.BindingsEditor bindingsEditor;
        private System.Windows.Forms.Panel panelTest;
        private ExtendedControls.ImageViewer imageViewer1;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtButtonDrawn extButtonDrawn1;
    }
}

