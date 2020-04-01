namespace Winform_Interop_UWP_OCR
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.boxClip = new System.Windows.Forms.PictureBox();
            this.boxOCR = new System.Windows.Forms.PictureBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.boxClip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxOCR)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // boxClip
            // 
            this.boxClip.Dock = System.Windows.Forms.DockStyle.Left;
            this.boxClip.Location = new System.Drawing.Point(0, 0);
            this.boxClip.Margin = new System.Windows.Forms.Padding(0);
            this.boxClip.Name = "boxClip";
            this.boxClip.Size = new System.Drawing.Size(426, 345);
            this.boxClip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.boxClip.TabIndex = 1;
            this.boxClip.TabStop = false;
            this.boxClip.Click += new System.EventHandler(this.boxClip_Click);
            // 
            // boxOCR
            // 
            this.boxOCR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxOCR.Location = new System.Drawing.Point(426, 0);
            this.boxOCR.Margin = new System.Windows.Forms.Padding(0);
            this.boxOCR.Name = "boxOCR";
            this.boxOCR.Size = new System.Drawing.Size(404, 345);
            this.boxOCR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.boxOCR.TabIndex = 2;
            this.boxOCR.TabStop = false;
            this.boxOCR.Click += new System.EventHandler(this.boxOCR_Click);
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Location = new System.Drawing.Point(426, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 345);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 345);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.boxOCR);
            this.Controls.Add(this.boxClip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "TestBot V1.0";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.boxClip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxOCR)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.PictureBox boxClip;
        private System.Windows.Forms.PictureBox boxOCR;
        private System.Windows.Forms.Splitter splitter1;
    }
}

