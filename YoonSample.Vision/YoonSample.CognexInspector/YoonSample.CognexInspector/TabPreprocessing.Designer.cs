namespace YoonSample.CognexInspector
{
    partial class TabPreprocessing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabPreprocessing));
            this.checkBox_IsUseFiltering = new System.Windows.Forms.CheckBox();
            this.button_SettingFiltering = new System.Windows.Forms.Button();
            this.label_TextFiltering = new System.Windows.Forms.Label();
            this.button_ProcessPreprocess = new System.Windows.Forms.Button();
            this.checkBox_IsUsePreprocessing = new System.Windows.Forms.CheckBox();
            this.checkBox_IsUseSobel = new System.Windows.Forms.CheckBox();
            this.checkBox_IsUseImageConvert = new System.Windows.Forms.CheckBox();
            this.button_SettingSobel = new System.Windows.Forms.Button();
            this.label_TextSobel = new System.Windows.Forms.Label();
            this.button_SettingImageConvert = new System.Windows.Forms.Button();
            this.label_IsUsePreprocessing = new System.Windows.Forms.Label();
            this.label_TextImageConvert = new System.Windows.Forms.Label();
            this.label_TextSetting = new System.Windows.Forms.Label();
            this.cogDisplay_ProcessView = new Cognex.VisionPro.Display.CogDisplay();
            this.cogDisplay_PreviousView = new Cognex.VisionPro.Display.CogDisplay();
            this.label_TextInspectTest = new System.Windows.Forms.Label();
            this.button_UpdateSetting = new System.Windows.Forms.Button();
            this.propertyGrid_ParamSetting = new System.Windows.Forms.PropertyGrid();
            this.label_TextAdvancedSetting = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ProcessView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_PreviousView)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBox_IsUseFiltering
            // 
            this.checkBox_IsUseFiltering.AutoSize = true;
            this.checkBox_IsUseFiltering.Location = new System.Drawing.Point(186, 509);
            this.checkBox_IsUseFiltering.Name = "checkBox_IsUseFiltering";
            this.checkBox_IsUseFiltering.Size = new System.Drawing.Size(50, 18);
            this.checkBox_IsUseFiltering.TabIndex = 329;
            this.checkBox_IsUseFiltering.Text = "Use";
            this.checkBox_IsUseFiltering.UseVisualStyleBackColor = true;
            // 
            // button_SettingFiltering
            // 
            this.button_SettingFiltering.Location = new System.Drawing.Point(244, 506);
            this.button_SettingFiltering.Name = "button_SettingFiltering";
            this.button_SettingFiltering.Size = new System.Drawing.Size(150, 25);
            this.button_SettingFiltering.TabIndex = 328;
            this.button_SettingFiltering.Text = "Setting";
            this.button_SettingFiltering.UseVisualStyleBackColor = true;
            this.button_SettingFiltering.Click += new System.EventHandler(this.button_SettingFiltering_Click);
            // 
            // label_TextFiltering
            // 
            this.label_TextFiltering.AutoSize = true;
            this.label_TextFiltering.Location = new System.Drawing.Point(18, 511);
            this.label_TextFiltering.Name = "label_TextFiltering";
            this.label_TextFiltering.Size = new System.Drawing.Size(73, 14);
            this.label_TextFiltering.TabIndex = 327;
            this.label_TextFiltering.Text = "2. Filtering";
            // 
            // button_ProcessPreprocess
            // 
            this.button_ProcessPreprocess.Location = new System.Drawing.Point(795, 54);
            this.button_ProcessPreprocess.Name = "button_ProcessPreprocess";
            this.button_ProcessPreprocess.Size = new System.Drawing.Size(100, 25);
            this.button_ProcessPreprocess.TabIndex = 326;
            this.button_ProcessPreprocess.Text = "Process";
            this.button_ProcessPreprocess.UseVisualStyleBackColor = true;
            this.button_ProcessPreprocess.Click += new System.EventHandler(this.button_ProcessPreprocess_Click);
            // 
            // checkBox_IsUsePreprocessing
            // 
            this.checkBox_IsUsePreprocessing.AutoSize = true;
            this.checkBox_IsUsePreprocessing.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_IsUsePreprocessing.Location = new System.Drawing.Point(480, 15);
            this.checkBox_IsUsePreprocessing.Name = "checkBox_IsUsePreprocessing";
            this.checkBox_IsUsePreprocessing.Size = new System.Drawing.Size(56, 22);
            this.checkBox_IsUsePreprocessing.TabIndex = 325;
            this.checkBox_IsUsePreprocessing.Text = "Yes";
            this.checkBox_IsUsePreprocessing.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsUseSobel
            // 
            this.checkBox_IsUseSobel.AutoSize = true;
            this.checkBox_IsUseSobel.Location = new System.Drawing.Point(186, 540);
            this.checkBox_IsUseSobel.Name = "checkBox_IsUseSobel";
            this.checkBox_IsUseSobel.Size = new System.Drawing.Size(50, 18);
            this.checkBox_IsUseSobel.TabIndex = 324;
            this.checkBox_IsUseSobel.Text = "Use";
            this.checkBox_IsUseSobel.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsUseImageConvert
            // 
            this.checkBox_IsUseImageConvert.AutoSize = true;
            this.checkBox_IsUseImageConvert.Location = new System.Drawing.Point(186, 478);
            this.checkBox_IsUseImageConvert.Name = "checkBox_IsUseImageConvert";
            this.checkBox_IsUseImageConvert.Size = new System.Drawing.Size(50, 18);
            this.checkBox_IsUseImageConvert.TabIndex = 323;
            this.checkBox_IsUseImageConvert.Text = "Use";
            this.checkBox_IsUseImageConvert.UseVisualStyleBackColor = true;
            // 
            // button_SettingSobel
            // 
            this.button_SettingSobel.Location = new System.Drawing.Point(244, 537);
            this.button_SettingSobel.Name = "button_SettingSobel";
            this.button_SettingSobel.Size = new System.Drawing.Size(150, 25);
            this.button_SettingSobel.TabIndex = 322;
            this.button_SettingSobel.Text = "Setting";
            this.button_SettingSobel.UseVisualStyleBackColor = true;
            this.button_SettingSobel.Click += new System.EventHandler(this.button_SettingSobel_Click);
            // 
            // label_TextSobel
            // 
            this.label_TextSobel.AutoSize = true;
            this.label_TextSobel.Location = new System.Drawing.Point(18, 542);
            this.label_TextSobel.Name = "label_TextSobel";
            this.label_TextSobel.Size = new System.Drawing.Size(58, 14);
            this.label_TextSobel.TabIndex = 321;
            this.label_TextSobel.Text = "3. Sobel";
            // 
            // button_SettingImageConvert
            // 
            this.button_SettingImageConvert.Location = new System.Drawing.Point(244, 475);
            this.button_SettingImageConvert.Name = "button_SettingImageConvert";
            this.button_SettingImageConvert.Size = new System.Drawing.Size(150, 25);
            this.button_SettingImageConvert.TabIndex = 320;
            this.button_SettingImageConvert.Text = "Setting";
            this.button_SettingImageConvert.UseVisualStyleBackColor = true;
            this.button_SettingImageConvert.Click += new System.EventHandler(this.button_SettingImageConvert_Click);
            // 
            // label_IsUsePreprocessing
            // 
            this.label_IsUsePreprocessing.AutoSize = true;
            this.label_IsUsePreprocessing.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_IsUsePreprocessing.Location = new System.Drawing.Point(30, 15);
            this.label_IsUsePreprocessing.Name = "label_IsUsePreprocessing";
            this.label_IsUsePreprocessing.Size = new System.Drawing.Size(387, 18);
            this.label_IsUsePreprocessing.TabIndex = 319;
            this.label_IsUsePreprocessing.Text = "Would you like to use \"Image Preprocessing\"?";
            // 
            // label_TextImageConvert
            // 
            this.label_TextImageConvert.AutoSize = true;
            this.label_TextImageConvert.Location = new System.Drawing.Point(18, 480);
            this.label_TextImageConvert.Name = "label_TextImageConvert";
            this.label_TextImageConvert.Size = new System.Drawing.Size(117, 14);
            this.label_TextImageConvert.TabIndex = 318;
            this.label_TextImageConvert.Text = "1. Image Convert";
            // 
            // label_TextSetting
            // 
            this.label_TextSetting.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextSetting.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextSetting.ForeColor = System.Drawing.Color.White;
            this.label_TextSetting.Location = new System.Drawing.Point(5, 435);
            this.label_TextSetting.Name = "label_TextSetting";
            this.label_TextSetting.Size = new System.Drawing.Size(280, 25);
            this.label_TextSetting.TabIndex = 317;
            this.label_TextSetting.Text = "Setting.";
            this.label_TextSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cogDisplay_ProcessView
            // 
            this.cogDisplay_ProcessView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cogDisplay_ProcessView.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_ProcessView.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_ProcessView.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_ProcessView.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_ProcessView.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_ProcessView.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_ProcessView.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_ProcessView.Location = new System.Drawing.Point(455, 85);
            this.cogDisplay_ProcessView.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_ProcessView.MouseWheelSensitivity = 1D;
            this.cogDisplay_ProcessView.Name = "cogDisplay_ProcessView";
            this.cogDisplay_ProcessView.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_ProcessView.OcxState")));
            this.cogDisplay_ProcessView.Size = new System.Drawing.Size(440, 330);
            this.cogDisplay_ProcessView.TabIndex = 316;
            // 
            // cogDisplay_PreviousView
            // 
            this.cogDisplay_PreviousView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cogDisplay_PreviousView.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_PreviousView.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_PreviousView.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_PreviousView.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_PreviousView.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_PreviousView.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_PreviousView.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_PreviousView.Location = new System.Drawing.Point(10, 85);
            this.cogDisplay_PreviousView.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_PreviousView.MouseWheelSensitivity = 1D;
            this.cogDisplay_PreviousView.Name = "cogDisplay_PreviousView";
            this.cogDisplay_PreviousView.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_PreviousView.OcxState")));
            this.cogDisplay_PreviousView.Size = new System.Drawing.Size(440, 330);
            this.cogDisplay_PreviousView.TabIndex = 315;
            // 
            // label_TextInspectTest
            // 
            this.label_TextInspectTest.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextInspectTest.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextInspectTest.ForeColor = System.Drawing.Color.White;
            this.label_TextInspectTest.Location = new System.Drawing.Point(5, 50);
            this.label_TextInspectTest.Name = "label_TextInspectTest";
            this.label_TextInspectTest.Size = new System.Drawing.Size(280, 25);
            this.label_TextInspectTest.TabIndex = 314;
            this.label_TextInspectTest.Text = "Inspection Test.";
            this.label_TextInspectTest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_UpdateSetting
            // 
            this.button_UpdateSetting.Location = new System.Drawing.Point(1240, 50);
            this.button_UpdateSetting.Name = "button_UpdateSetting";
            this.button_UpdateSetting.Size = new System.Drawing.Size(80, 25);
            this.button_UpdateSetting.TabIndex = 382;
            this.button_UpdateSetting.Text = "Update";
            this.button_UpdateSetting.UseVisualStyleBackColor = true;
            this.button_UpdateSetting.Click += new System.EventHandler(this.button_UpdateSetting_Click);
            // 
            // propertyGrid_ParamSetting
            // 
            this.propertyGrid_ParamSetting.Location = new System.Drawing.Point(920, 80);
            this.propertyGrid_ParamSetting.Name = "propertyGrid_ParamSetting";
            this.propertyGrid_ParamSetting.Size = new System.Drawing.Size(400, 500);
            this.propertyGrid_ParamSetting.TabIndex = 381;
            // 
            // label_TextAdvancedSetting
            // 
            this.label_TextAdvancedSetting.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextAdvancedSetting.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextAdvancedSetting.ForeColor = System.Drawing.Color.White;
            this.label_TextAdvancedSetting.Location = new System.Drawing.Point(920, 50);
            this.label_TextAdvancedSetting.Name = "label_TextAdvancedSetting";
            this.label_TextAdvancedSetting.Size = new System.Drawing.Size(320, 25);
            this.label_TextAdvancedSetting.TabIndex = 380;
            this.label_TextAdvancedSetting.Text = "Advanced Setting.";
            this.label_TextAdvancedSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormTab_Preprocessing
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1350, 600);
            this.Controls.Add(this.button_UpdateSetting);
            this.Controls.Add(this.propertyGrid_ParamSetting);
            this.Controls.Add(this.label_TextAdvancedSetting);
            this.Controls.Add(this.checkBox_IsUseFiltering);
            this.Controls.Add(this.button_SettingFiltering);
            this.Controls.Add(this.label_TextFiltering);
            this.Controls.Add(this.button_ProcessPreprocess);
            this.Controls.Add(this.checkBox_IsUsePreprocessing);
            this.Controls.Add(this.checkBox_IsUseSobel);
            this.Controls.Add(this.checkBox_IsUseImageConvert);
            this.Controls.Add(this.button_SettingSobel);
            this.Controls.Add(this.label_TextSobel);
            this.Controls.Add(this.button_SettingImageConvert);
            this.Controls.Add(this.label_IsUsePreprocessing);
            this.Controls.Add(this.label_TextImageConvert);
            this.Controls.Add(this.label_TextSetting);
            this.Controls.Add(this.cogDisplay_ProcessView);
            this.Controls.Add(this.cogDisplay_PreviousView);
            this.Controls.Add(this.label_TextInspectTest);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormTab_Preprocessing";
            this.Text = "FormTab_Preprocessing";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTab_Preprocessing_FormClosed);
            this.Load += new System.EventHandler(this.FormTab_Preprocessing_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ProcessView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_PreviousView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_IsUseFiltering;
        private System.Windows.Forms.Button button_SettingFiltering;
        private System.Windows.Forms.Label label_TextFiltering;
        private System.Windows.Forms.Button button_ProcessPreprocess;
        private System.Windows.Forms.CheckBox checkBox_IsUsePreprocessing;
        private System.Windows.Forms.CheckBox checkBox_IsUseSobel;
        private System.Windows.Forms.CheckBox checkBox_IsUseImageConvert;
        private System.Windows.Forms.Button button_SettingSobel;
        private System.Windows.Forms.Label label_TextSobel;
        private System.Windows.Forms.Button button_SettingImageConvert;
        private System.Windows.Forms.Label label_IsUsePreprocessing;
        private System.Windows.Forms.Label label_TextImageConvert;
        private System.Windows.Forms.Label label_TextSetting;
        internal Cognex.VisionPro.Display.CogDisplay cogDisplay_ProcessView;
        internal Cognex.VisionPro.Display.CogDisplay cogDisplay_PreviousView;
        private System.Windows.Forms.Label label_TextInspectTest;
        private System.Windows.Forms.Button button_UpdateSetting;
        private System.Windows.Forms.PropertyGrid propertyGrid_ParamSetting;
        private System.Windows.Forms.Label label_TextAdvancedSetting;
    }
}