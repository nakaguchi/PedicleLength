namespace PedicleLengthCS {
    partial class MainForm {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.BtnOpenDicom = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.TbrSliceIdx = new System.Windows.Forms.TrackBar();
            this.LblSliceIdx = new System.Windows.Forms.Label();
            this.LblWindowLevel = new System.Windows.Forms.Label();
            this.TbrWindowLevel = new System.Windows.Forms.TrackBar();
            this.LblWindowWidth = new System.Windows.Forms.Label();
            this.TbrWindowWidth = new System.Windows.Forms.TrackBar();
            this.LbxPoints = new System.Windows.Forms.ListBox();
            this.LblVoxel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LblLength = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LblCursor = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbrSliceIdx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbrWindowLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbrWindowWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnOpenDicom
            // 
            this.BtnOpenDicom.Location = new System.Drawing.Point(12, 12);
            this.BtnOpenDicom.Name = "BtnOpenDicom";
            this.BtnOpenDicom.Size = new System.Drawing.Size(161, 31);
            this.BtnOpenDicom.TabIndex = 0;
            this.BtnOpenDicom.Text = "Open Dicom Folder";
            this.BtnOpenDicom.UseVisualStyleBackColor = true;
            this.BtnOpenDicom.Click += new System.EventHandler(this.BtnOpenDicom_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.pictureBox1.Location = new System.Drawing.Point(179, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 510);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // TbrSliceIdx
            // 
            this.TbrSliceIdx.Location = new System.Drawing.Point(12, 106);
            this.TbrSliceIdx.Name = "TbrSliceIdx";
            this.TbrSliceIdx.Size = new System.Drawing.Size(161, 45);
            this.TbrSliceIdx.TabIndex = 2;
            this.TbrSliceIdx.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TbrSliceIdx.ValueChanged += new System.EventHandler(this.SliderChanged);
            // 
            // LblSliceIdx
            // 
            this.LblSliceIdx.AutoSize = true;
            this.LblSliceIdx.Location = new System.Drawing.Point(13, 94);
            this.LblSliceIdx.Name = "LblSliceIdx";
            this.LblSliceIdx.Size = new System.Drawing.Size(61, 12);
            this.LblSliceIdx.TabIndex = 3;
            this.LblSliceIdx.Text = "Slice index";
            // 
            // LblWindowLevel
            // 
            this.LblWindowLevel.AutoSize = true;
            this.LblWindowLevel.Location = new System.Drawing.Point(13, 140);
            this.LblWindowLevel.Name = "LblWindowLevel";
            this.LblWindowLevel.Size = new System.Drawing.Size(74, 12);
            this.LblWindowLevel.TabIndex = 5;
            this.LblWindowLevel.Text = "Window Level";
            // 
            // TbrWindowLevel
            // 
            this.TbrWindowLevel.LargeChange = 10;
            this.TbrWindowLevel.Location = new System.Drawing.Point(12, 152);
            this.TbrWindowLevel.Maximum = 2000;
            this.TbrWindowLevel.Minimum = -2000;
            this.TbrWindowLevel.Name = "TbrWindowLevel";
            this.TbrWindowLevel.Size = new System.Drawing.Size(161, 45);
            this.TbrWindowLevel.TabIndex = 4;
            this.TbrWindowLevel.TickFrequency = 100;
            this.TbrWindowLevel.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TbrWindowLevel.ValueChanged += new System.EventHandler(this.SliderChanged);
            // 
            // LblWindowWidth
            // 
            this.LblWindowWidth.AutoSize = true;
            this.LblWindowWidth.Location = new System.Drawing.Point(13, 185);
            this.LblWindowWidth.Name = "LblWindowWidth";
            this.LblWindowWidth.Size = new System.Drawing.Size(75, 12);
            this.LblWindowWidth.TabIndex = 7;
            this.LblWindowWidth.Text = "Window Width";
            // 
            // TbrWindowWidth
            // 
            this.TbrWindowWidth.LargeChange = 10;
            this.TbrWindowWidth.Location = new System.Drawing.Point(12, 198);
            this.TbrWindowWidth.Maximum = 3000;
            this.TbrWindowWidth.Minimum = 1;
            this.TbrWindowWidth.Name = "TbrWindowWidth";
            this.TbrWindowWidth.Size = new System.Drawing.Size(161, 45);
            this.TbrWindowWidth.TabIndex = 6;
            this.TbrWindowWidth.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TbrWindowWidth.Value = 300;
            this.TbrWindowWidth.ValueChanged += new System.EventHandler(this.SliderChanged);
            // 
            // LbxPoints
            // 
            this.LbxPoints.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LbxPoints.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LbxPoints.FormattingEnabled = true;
            this.LbxPoints.Location = new System.Drawing.Point(15, 268);
            this.LbxPoints.Name = "LbxPoints";
            this.LbxPoints.Size = new System.Drawing.Size(158, 199);
            this.LbxPoints.TabIndex = 10;
            this.LbxPoints.Click += new System.EventHandler(this.LbxPoints_Click);
            // 
            // LblVoxel
            // 
            this.LblVoxel.AutoSize = true;
            this.LblVoxel.Location = new System.Drawing.Point(12, 50);
            this.LblVoxel.Name = "LblVoxel";
            this.LblVoxel.Size = new System.Drawing.Size(34, 12);
            this.LblVoxel.TabIndex = 11;
            this.LblVoxel.Text = "Voxel";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(12, 509);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Length";
            // 
            // LblLength
            // 
            this.LblLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LblLength.AutoSize = true;
            this.LblLength.Font = new System.Drawing.Font("ＭＳ ゴシック", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LblLength.Location = new System.Drawing.Point(55, 497);
            this.LblLength.Name = "LblLength";
            this.LblLength.Size = new System.Drawing.Size(102, 27);
            this.LblLength.TabIndex = 13;
            this.LblLength.Text = "   0.0";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(150, 509);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "mm";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(12, 252);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Point list";
            // 
            // LblCursor
            // 
            this.LblCursor.AutoSize = true;
            this.LblCursor.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LblCursor.Location = new System.Drawing.Point(12, 66);
            this.LblCursor.Name = "LblCursor";
            this.LblCursor.Size = new System.Drawing.Size(39, 12);
            this.LblCursor.TabIndex = 16;
            this.LblCursor.Text = "Cursor";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 534);
            this.Controls.Add(this.LblCursor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LblLength);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LblVoxel);
            this.Controls.Add(this.LbxPoints);
            this.Controls.Add(this.LblWindowWidth);
            this.Controls.Add(this.TbrWindowWidth);
            this.Controls.Add(this.LblWindowLevel);
            this.Controls.Add(this.TbrWindowLevel);
            this.Controls.Add(this.LblSliceIdx);
            this.Controls.Add(this.TbrSliceIdx);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.BtnOpenDicom);
            this.Name = "MainForm";
            this.Text = "Pedicle Length";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbrSliceIdx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbrWindowLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbrWindowWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOpenDicom;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TrackBar TbrSliceIdx;
        private System.Windows.Forms.Label LblSliceIdx;
        private System.Windows.Forms.Label LblWindowLevel;
        private System.Windows.Forms.TrackBar TbrWindowLevel;
        private System.Windows.Forms.Label LblWindowWidth;
        private System.Windows.Forms.TrackBar TbrWindowWidth;
        private System.Windows.Forms.ListBox LbxPoints;
        private System.Windows.Forms.Label LblVoxel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LblLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LblCursor;
    }
}

