
namespace YoonSample.ImageViewer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.imageViewer_Main = new YoonFactory.Viewer.ImageViewer();
            this.panel_StatusBar = new System.Windows.Forms.Panel();
            this.button_Live = new System.Windows.Forms.Button();
            this.button_Capture = new System.Windows.Forms.Button();
            this.button_Open = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_Exit = new System.Windows.Forms.Button();
            this.comboBox_Camera = new System.Windows.Forms.ComboBox();
            this.button_SetCamera = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // imageViewer_Main
            // 
            this.imageViewer_Main.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("imageViewer_Main.BackgroundImage")));
            this.imageViewer_Main.BitCount = 8;
            this.imageViewer_Main.CurrentPos = new System.Drawing.Point(0, 0);
            this.imageViewer_Main.IsDefectROI = false;
            this.imageViewer_Main.IsDrawing = false;
            this.imageViewer_Main.IsEnabledDraw = false;
            this.imageViewer_Main.IsEnabledROI = false;
            this.imageViewer_Main.IsGuideLine = false;
            this.imageViewer_Main.IsMeasure = false;
            this.imageViewer_Main.IsObservationROI = false;
            this.imageViewer_Main.IsProfile = false;
            this.imageViewer_Main.IsROIDraw = false;
            this.imageViewer_Main.KindView = 0;
            this.imageViewer_Main.Location = new System.Drawing.Point(5, 45);
            this.imageViewer_Main.Name = "imageViewer_Main";
            this.imageViewer_Main.PixFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
            this.imageViewer_Main.ROI = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.imageViewer_Main.ScrollPosition = new System.Drawing.Point(0, 0);
            this.imageViewer_Main.Size = new System.Drawing.Size(640, 480);
            this.imageViewer_Main.StartPos = new System.Drawing.Point(0, 0);
            this.imageViewer_Main.TabIndex = 0;
            this.imageViewer_Main.Zoom = 0D;
            // 
            // panel_StatusBar
            // 
            this.panel_StatusBar.BackColor = System.Drawing.Color.Beige;
            this.panel_StatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_StatusBar.Location = new System.Drawing.Point(0, 531);
            this.panel_StatusBar.Name = "panel_StatusBar";
            this.panel_StatusBar.Size = new System.Drawing.Size(784, 30);
            this.panel_StatusBar.TabIndex = 1;
            // 
            // button_Live
            // 
            this.button_Live.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Live.Location = new System.Drawing.Point(653, 52);
            this.button_Live.Name = "button_Live";
            this.button_Live.Size = new System.Drawing.Size(120, 30);
            this.button_Live.TabIndex = 3;
            this.button_Live.Text = "Live";
            this.button_Live.UseVisualStyleBackColor = true;
            this.button_Live.Click += new System.EventHandler(this.button_Live_Click);
            // 
            // button_Capture
            // 
            this.button_Capture.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Capture.Location = new System.Drawing.Point(653, 90);
            this.button_Capture.Name = "button_Capture";
            this.button_Capture.Size = new System.Drawing.Size(120, 30);
            this.button_Capture.TabIndex = 4;
            this.button_Capture.Text = "Capture";
            this.button_Capture.UseVisualStyleBackColor = true;
            this.button_Capture.Click += new System.EventHandler(this.button_Capture_Click);
            // 
            // button_Open
            // 
            this.button_Open.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Open.Location = new System.Drawing.Point(653, 126);
            this.button_Open.Name = "button_Open";
            this.button_Open.Size = new System.Drawing.Size(120, 30);
            this.button_Open.TabIndex = 6;
            this.button_Open.Text = "Open";
            this.button_Open.UseVisualStyleBackColor = true;
            this.button_Open.Click += new System.EventHandler(this.button_Open_Click);
            // 
            // button_Save
            // 
            this.button_Save.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Save.Location = new System.Drawing.Point(653, 164);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(120, 30);
            this.button_Save.TabIndex = 7;
            this.button_Save.Text = "Save";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_Exit
            // 
            this.button_Exit.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Exit.Location = new System.Drawing.Point(653, 202);
            this.button_Exit.Name = "button_Exit";
            this.button_Exit.Size = new System.Drawing.Size(120, 30);
            this.button_Exit.TabIndex = 8;
            this.button_Exit.Text = "Exit";
            this.button_Exit.UseVisualStyleBackColor = true;
            this.button_Exit.Click += new System.EventHandler(this.button_Exit_Click);
            // 
            // comboBox_Camera
            // 
            this.comboBox_Camera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox_Camera.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_Camera.FormattingEnabled = true;
            this.comboBox_Camera.Location = new System.Drawing.Point(5, 5);
            this.comboBox_Camera.Name = "comboBox_Camera";
            this.comboBox_Camera.Size = new System.Drawing.Size(500, 24);
            this.comboBox_Camera.TabIndex = 9;
            // 
            // button_SetCamera
            // 
            this.button_SetCamera.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_SetCamera.Location = new System.Drawing.Point(510, 3);
            this.button_SetCamera.Name = "button_SetCamera";
            this.button_SetCamera.Size = new System.Drawing.Size(120, 30);
            this.button_SetCamera.TabIndex = 10;
            this.button_SetCamera.Text = "Set";
            this.button_SetCamera.UseVisualStyleBackColor = true;
            this.button_SetCamera.Click += new System.EventHandler(this.button_SetCamera_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.ControlBox = false;
            this.Controls.Add(this.button_SetCamera);
            this.Controls.Add(this.comboBox_Camera);
            this.Controls.Add(this.button_Exit);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.button_Open);
            this.Controls.Add(this.button_Capture);
            this.Controls.Add(this.button_Live);
            this.Controls.Add(this.panel_StatusBar);
            this.Controls.Add(this.imageViewer_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "Image Viewer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private YoonFactory.Viewer.ImageViewer imageViewer_Main;
        private System.Windows.Forms.Panel panel_StatusBar;
        private System.Windows.Forms.Button button_Live;
        private System.Windows.Forms.Button button_Capture;
        private System.Windows.Forms.Button button_Open;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_Exit;
        private System.Windows.Forms.ComboBox comboBox_Camera;
        private System.Windows.Forms.Button button_SetCamera;
    }
}

