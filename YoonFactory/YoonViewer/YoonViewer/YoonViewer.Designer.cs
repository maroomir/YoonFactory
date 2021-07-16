namespace YoonFactory.Viewer
{
    partial class ImageViewer
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageViewer));
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem_FixToScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Zoom10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Zoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Zoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Zoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Zoom400 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem_ROI = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // hScrollBar
            // 
            this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar.Location = new System.Drawing.Point(0, 460);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(640, 20);
            this.hScrollBar.TabIndex = 0;
            this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Scroll);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(620, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(20, 460);
            this.vScrollBar.TabIndex = 1;
            this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripMenuItem_FixToScreen,
            this.toolStripMenuItem_Zoom10,
            this.toolStripMenuItem_Zoom50,
            this.toolStripMenuItem_Zoom100,
            this.toolStripMenuItem_Zoom200,
            this.toolStripMenuItem_Zoom400,
            this.toolStripSeparator2,
            this.toolStripMenuItem_ROI});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(162, 170);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
            // 
            // toolStripMenuItem_FixToScreen
            // 
            this.toolStripMenuItem_FixToScreen.Name = "toolStripMenuItem_FixToScreen";
            this.toolStripMenuItem_FixToScreen.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItem_FixToScreen.Tag = "0";
            this.toolStripMenuItem_FixToScreen.Text = "Fix To Screen";
            this.toolStripMenuItem_FixToScreen.Click += new System.EventHandler(this.OnButtonZoomClick);
            // 
            // toolStripMenuItem_Zoom10
            // 
            this.toolStripMenuItem_Zoom10.Name = "toolStripMenuItem_Zoom10";
            this.toolStripMenuItem_Zoom10.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItem_Zoom10.Tag = "1";
            this.toolStripMenuItem_Zoom10.Text = "10%";
            this.toolStripMenuItem_Zoom10.Click += new System.EventHandler(this.OnButtonZoomClick);
            // 
            // toolStripMenuItem_Zoom50
            // 
            this.toolStripMenuItem_Zoom50.Name = "toolStripMenuItem_Zoom50";
            this.toolStripMenuItem_Zoom50.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItem_Zoom50.Tag = "2";
            this.toolStripMenuItem_Zoom50.Text = "50%";
            this.toolStripMenuItem_Zoom50.Click += new System.EventHandler(this.OnButtonZoomClick);
            // 
            // toolStripMenuItem_Zoom100
            // 
            this.toolStripMenuItem_Zoom100.Name = "toolStripMenuItem_Zoom100";
            this.toolStripMenuItem_Zoom100.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItem_Zoom100.Tag = "3";
            this.toolStripMenuItem_Zoom100.Text = "100%";
            this.toolStripMenuItem_Zoom100.Click += new System.EventHandler(this.OnButtonZoomClick);
            // 
            // toolStripMenuItem_Zoom200
            // 
            this.toolStripMenuItem_Zoom200.Name = "toolStripMenuItem_Zoom200";
            this.toolStripMenuItem_Zoom200.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItem_Zoom200.Tag = "4";
            this.toolStripMenuItem_Zoom200.Text = "200%";
            this.toolStripMenuItem_Zoom200.Click += new System.EventHandler(this.OnButtonZoomClick);
            // 
            // toolStripMenuItem_Zoom400
            // 
            this.toolStripMenuItem_Zoom400.Name = "toolStripMenuItem_Zoom400";
            this.toolStripMenuItem_Zoom400.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItem_Zoom400.Tag = "5";
            this.toolStripMenuItem_Zoom400.Text = "400%";
            this.toolStripMenuItem_Zoom400.Click += new System.EventHandler(this.OnButtonZoomClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(158, 6);
            // 
            // toolStripMenuItem_ROI
            // 
            this.toolStripMenuItem_ROI.Name = "toolStripMenuItem_ROI";
            this.toolStripMenuItem_ROI.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItem_ROI.Text = "Regin Of Inspect";
            this.toolStripMenuItem_ROI.Click += new System.EventHandler(this.OnButtonROIClick);
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.hScrollBar);
            this.Name = "ImageViewer";
            this.Size = new System.Drawing.Size(640, 480);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageViewer_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ImageViewer_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageViewer_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageViewer_MouseMove);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HScrollBar hScrollBar;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_FixToScreen;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Zoom10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Zoom50;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Zoom100;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Zoom200;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Zoom400;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_ROI;
    }
}
