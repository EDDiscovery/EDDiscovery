namespace DialogTest
{
    partial class TestCompositeButton
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonExt3 = new ExtendedControls.ButtonExt();
            this.compositeButton1 = new ExtendedControls.CompositeButton();
            this.buttonExt1 = new ExtendedControls.ButtonExt();
            this.buttonExt2 = new ExtendedControls.ButtonExt();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 74);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button1";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::DialogTest.Properties.Resources.edsm32x32;
            this.panel1.Location = new System.Drawing.Point(86, 156);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(48, 48);
            this.panel1.TabIndex = 0;
            // 
            // buttonExt3
            // 
            this.buttonExt3.BackColor = System.Drawing.Color.Chocolate;
            this.buttonExt3.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.buttonExt3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.buttonExt3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonExt3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonExt3.Image = global::DialogTest.Properties.Resources.edlogo24;
            this.buttonExt3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonExt3.Location = new System.Drawing.Point(50, 143);
            this.buttonExt3.Name = "buttonExt3";
            this.buttonExt3.Size = new System.Drawing.Size(117, 65);
            this.buttonExt3.TabIndex = 3;
            this.buttonExt3.Text = "buttonExt3";
            this.buttonExt3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonExt3.UseVisualStyleBackColor = false;
            // 
            // compositeButton1
            // 
            this.compositeButton1.BackColor = System.Drawing.Color.Azure;
            this.compositeButton1.BackgroundImage = global::DialogTest.Properties.Resources.edlogo24;
            this.compositeButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.compositeButton1.Buttons = new ExtendedControls.ButtonExt[] {
        this.buttonExt1,
        this.buttonExt2};
            this.compositeButton1.ButtonSpacing = 8;
            this.compositeButton1.Decals = new System.Windows.Forms.Panel[] {
        this.panel1};
            this.compositeButton1.DecalSpacing = 8;
            this.compositeButton1.Location = new System.Drawing.Point(359, 31);
            this.compositeButton1.Name = "compositeButton1";
            this.compositeButton1.Padding = new System.Windows.Forms.Padding(10);
            this.compositeButton1.Size = new System.Drawing.Size(373, 390);
            this.compositeButton1.TabIndex = 1;
            this.compositeButton1.Text = "This is a test";
            // 
            // buttonExt1
            // 
            this.buttonExt1.BackColor = System.Drawing.Color.BurlyWood;
            this.buttonExt1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonExt1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.buttonExt1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonExt1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonExt1.Image = global::DialogTest.Properties.Resources.galaxy;
            this.buttonExt1.Location = new System.Drawing.Point(134, 332);
            this.buttonExt1.Name = "buttonExt1";
            this.buttonExt1.Size = new System.Drawing.Size(48, 48);
            this.buttonExt1.TabIndex = 0;
            this.buttonExt1.UseVisualStyleBackColor = false;
            // 
            // buttonExt2
            // 
            this.buttonExt2.BackColor = System.Drawing.Color.NavajoWhite;
            this.buttonExt2.FlatAppearance.BorderColor = System.Drawing.Color.LightSalmon;
            this.buttonExt2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.buttonExt2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.buttonExt2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonExt2.Image = global::DialogTest.Properties.Resources.galaxy_gray;
            this.buttonExt2.ImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonExt2.Location = new System.Drawing.Point(190, 332);
            this.buttonExt2.Margin = new System.Windows.Forms.Padding(0);
            this.buttonExt2.Name = "buttonExt2";
            this.buttonExt2.Size = new System.Drawing.Size(48, 48);
            this.buttonExt2.TabIndex = 0;
            this.buttonExt2.Text = "Hello";
            this.buttonExt2.UseVisualStyleBackColor = false;
            // 
            // TestCompositeButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSalmon;
            this.ClientSize = new System.Drawing.Size(1060, 623);
            this.Controls.Add(this.buttonExt3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.compositeButton1);
            this.Name = "TestCompositeButton";
            this.Text = "TestCompositeButton";
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.CompositeButton compositeButton1;
        private ExtendedControls.ButtonExt buttonExt1;
        private ExtendedControls.ButtonExt buttonExt2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private ExtendedControls.ButtonExt buttonExt3;
        private System.Windows.Forms.Panel panel1;
    }
}