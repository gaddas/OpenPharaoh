namespace OpenPharaohResourceViewer
{
    partial class frmInspect555
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
            this.scrollOffset = new System.Windows.Forms.VScrollBar();
            this.tbHeight = new System.Windows.Forms.TrackBar();
            this.tbWidth = new System.Windows.Forms.TrackBar();
            this.lblHeight = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.nudItemsCount = new System.Windows.Forms.NumericUpDown();
            this.lblOffset = new System.Windows.Forms.Label();
            this.pictureBox1 = new OpenPharaohResourceViewer.ExtendedPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudItemsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 50);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // scrollOffset
            // 
            this.scrollOffset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scrollOffset.LargeChange = 16;
            this.scrollOffset.Location = new System.Drawing.Point(692, 0);
            this.scrollOffset.Name = "scrollOffset";
            this.scrollOffset.Size = new System.Drawing.Size(24, 330);
            this.scrollOffset.TabIndex = 2;
            this.scrollOffset.ValueChanged += new System.EventHandler(this.scrollOffset_ValueChanged);
            // 
            // tbHeight
            // 
            this.tbHeight.LargeChange = 8;
            this.tbHeight.Location = new System.Drawing.Point(119, 29);
            this.tbHeight.Maximum = 512;
            this.tbHeight.Minimum = 1;
            this.tbHeight.Name = "tbHeight";
            this.tbHeight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbHeight.Size = new System.Drawing.Size(45, 143);
            this.tbHeight.TabIndex = 3;
            this.tbHeight.TickFrequency = 32;
            this.tbHeight.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbHeight.Value = 32;
            this.tbHeight.Scroll += new System.EventHandler(this.tbHeight_Scroll);
            // 
            // tbWidth
            // 
            this.tbWidth.LargeChange = 8;
            this.tbWidth.Location = new System.Drawing.Point(1, 164);
            this.tbWidth.Maximum = 512;
            this.tbWidth.Minimum = 1;
            this.tbWidth.Name = "tbWidth";
            this.tbWidth.Size = new System.Drawing.Size(130, 45);
            this.tbWidth.TabIndex = 4;
            this.tbWidth.TickFrequency = 32;
            this.tbWidth.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbWidth.Value = 32;
            this.tbWidth.Scroll += new System.EventHandler(this.tbWidth_Scroll);
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(83, 106);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(19, 13);
            this.lblHeight.TabIndex = 5;
            this.lblHeight.Text = "32";
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(43, 134);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(19, 13);
            this.lblWidth.TabIndex = 6;
            this.lblWidth.Text = "32";
            // 
            // nudItemsCount
            // 
            this.nudItemsCount.Location = new System.Drawing.Point(12, 68);
            this.nudItemsCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudItemsCount.Name = "nudItemsCount";
            this.nudItemsCount.Size = new System.Drawing.Size(60, 20);
            this.nudItemsCount.TabIndex = 8;
            this.nudItemsCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudItemsCount.ValueChanged += new System.EventHandler(this.nudItemsCount_ValueChanged);
            // 
            // lblOffset
            // 
            this.lblOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOffset.AutoSize = true;
            this.lblOffset.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblOffset.Location = new System.Drawing.Point(9, 308);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(16, 16);
            this.lblOffset.TabIndex = 9;
            this.lblOffset.Text = "0";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.pictureBox1.Location = new System.Drawing.Point(170, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(519, 330);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // frmInspect555
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(715, 330);
            this.Controls.Add(this.lblOffset);
            this.Controls.Add(this.nudItemsCount);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.tbWidth);
            this.Controls.Add(this.tbHeight);
            this.Controls.Add(this.scrollOffset);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmInspect555";
            this.Text = "OpenPharaoh 555 Viewer";
            this.Shown += new System.EventHandler(this.frmInspect555_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.tbHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudItemsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private ExtendedPictureBox pictureBox1;
        private System.Windows.Forms.VScrollBar scrollOffset;
        private System.Windows.Forms.TrackBar tbHeight;
        private System.Windows.Forms.TrackBar tbWidth;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.NumericUpDown nudItemsCount;
        private System.Windows.Forms.Label lblOffset;
    }
}

