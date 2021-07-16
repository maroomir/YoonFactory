namespace YoonSample.CognexInspector
{
    partial class TabObjectExtract
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabObjectExtract));
            this.button_ProcessObjectExtract = new System.Windows.Forms.Button();
            this.button_SettingColorSegment = new System.Windows.Forms.Button();
            this.button_SettingBlob = new System.Windows.Forms.Button();
            this.cogDisplay_ProcessView = new Cognex.VisionPro.Display.CogDisplay();
            this.cogDisplay_PrevView = new Cognex.VisionPro.Display.CogDisplay();
            this.label_TextSetting = new System.Windows.Forms.Label();
            this.label_TextInspectionTest = new System.Windows.Forms.Label();
            this.checkBox_IsUseObjectExtract = new System.Windows.Forms.CheckBox();
            this.label_IsUseObjectExtract = new System.Windows.Forms.Label();
            this.label_TextColorSegment = new System.Windows.Forms.Label();
            this.label_TextBlob = new System.Windows.Forms.Label();
            this.checkBox_IsUseColorSegment = new System.Windows.Forms.CheckBox();
            this.checkBox_IsUseBlob = new System.Windows.Forms.CheckBox();
            this.label_Text_SourceImage = new System.Windows.Forms.Label();
            this.button_SetSourceImage = new System.Windows.Forms.Button();
            this.comboBox_SelectSourceImage = new System.Windows.Forms.ComboBox();
            this.label_TextCombineOption = new System.Windows.Forms.Label();
            this.radioButton_CombineOverlapMin = new System.Windows.Forms.RadioButton();
            this.radioButton_CombineOverlapMax = new System.Windows.Forms.RadioButton();
            this.comboBox_SelectBlobSource = new System.Windows.Forms.ComboBox();
            this.comboBox_SelectColorSegmentSource = new System.Windows.Forms.ComboBox();
            this.button_UpdateSetting = new System.Windows.Forms.Button();
            this.propertyGrid_ParamSetting = new System.Windows.Forms.PropertyGrid();
            this.label_TextAdvancedSetting = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ProcessView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_PrevView)).BeginInit();
            this.SuspendLayout();
            // 
            // button_ProcessObjectExtract
            // 
            this.button_ProcessObjectExtract.Location = new System.Drawing.Point(790, 60);
            this.button_ProcessObjectExtract.Name = "button_ProcessObjectExtract";
            this.button_ProcessObjectExtract.Size = new System.Drawing.Size(100, 25);
            this.button_ProcessObjectExtract.TabIndex = 343;
            this.button_ProcessObjectExtract.Text = "Process";
            this.button_ProcessObjectExtract.UseVisualStyleBackColor = true;
            this.button_ProcessObjectExtract.Click += new System.EventHandler(this.button_ProcessObjectExtract_Click);
            // 
            // button_SettingColorSegment
            // 
            this.button_SettingColorSegment.Location = new System.Drawing.Point(455, 524);
            this.button_SettingColorSegment.Name = "button_SettingColorSegment";
            this.button_SettingColorSegment.Size = new System.Drawing.Size(100, 25);
            this.button_SettingColorSegment.TabIndex = 336;
            this.button_SettingColorSegment.Text = "Setting";
            this.button_SettingColorSegment.UseVisualStyleBackColor = true;
            this.button_SettingColorSegment.Click += new System.EventHandler(this.button_SettingColorSegment_Click);
            // 
            // button_SettingBlob
            // 
            this.button_SettingBlob.Location = new System.Drawing.Point(455, 489);
            this.button_SettingBlob.Name = "button_SettingBlob";
            this.button_SettingBlob.Size = new System.Drawing.Size(100, 25);
            this.button_SettingBlob.TabIndex = 335;
            this.button_SettingBlob.Text = "Setting";
            this.button_SettingBlob.UseVisualStyleBackColor = true;
            this.button_SettingBlob.Click += new System.EventHandler(this.button_SettingBlob_Click);
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
            this.cogDisplay_ProcessView.Location = new System.Drawing.Point(455, 90);
            this.cogDisplay_ProcessView.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_ProcessView.MouseWheelSensitivity = 1D;
            this.cogDisplay_ProcessView.Name = "cogDisplay_ProcessView";
            this.cogDisplay_ProcessView.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_ProcessView.OcxState")));
            this.cogDisplay_ProcessView.Size = new System.Drawing.Size(440, 330);
            this.cogDisplay_ProcessView.TabIndex = 334;
            // 
            // cogDisplay_PrevView
            // 
            this.cogDisplay_PrevView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cogDisplay_PrevView.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_PrevView.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_PrevView.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_PrevView.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_PrevView.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_PrevView.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_PrevView.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_PrevView.Location = new System.Drawing.Point(10, 90);
            this.cogDisplay_PrevView.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_PrevView.MouseWheelSensitivity = 1D;
            this.cogDisplay_PrevView.Name = "cogDisplay_PrevView";
            this.cogDisplay_PrevView.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_PrevView.OcxState")));
            this.cogDisplay_PrevView.Size = new System.Drawing.Size(440, 330);
            this.cogDisplay_PrevView.TabIndex = 333;
            // 
            // label_TextSetting
            // 
            this.label_TextSetting.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextSetting.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextSetting.ForeColor = System.Drawing.Color.White;
            this.label_TextSetting.Location = new System.Drawing.Point(5, 452);
            this.label_TextSetting.Name = "label_TextSetting";
            this.label_TextSetting.Size = new System.Drawing.Size(280, 25);
            this.label_TextSetting.TabIndex = 332;
            this.label_TextSetting.Text = "Setting.";
            this.label_TextSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_TextInspectionTest
            // 
            this.label_TextInspectionTest.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextInspectionTest.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextInspectionTest.ForeColor = System.Drawing.Color.White;
            this.label_TextInspectionTest.Location = new System.Drawing.Point(455, 60);
            this.label_TextInspectionTest.Name = "label_TextInspectionTest";
            this.label_TextInspectionTest.Size = new System.Drawing.Size(280, 25);
            this.label_TextInspectionTest.TabIndex = 331;
            this.label_TextInspectionTest.Text = "Inspection Test.";
            this.label_TextInspectionTest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBox_IsUseObjectExtract
            // 
            this.checkBox_IsUseObjectExtract.AutoSize = true;
            this.checkBox_IsUseObjectExtract.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_IsUseObjectExtract.Location = new System.Drawing.Point(480, 15);
            this.checkBox_IsUseObjectExtract.Name = "checkBox_IsUseObjectExtract";
            this.checkBox_IsUseObjectExtract.Size = new System.Drawing.Size(56, 22);
            this.checkBox_IsUseObjectExtract.TabIndex = 330;
            this.checkBox_IsUseObjectExtract.Text = "Yes";
            this.checkBox_IsUseObjectExtract.UseVisualStyleBackColor = true;
            // 
            // label_IsUseObjectExtract
            // 
            this.label_IsUseObjectExtract.AutoSize = true;
            this.label_IsUseObjectExtract.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_IsUseObjectExtract.Location = new System.Drawing.Point(30, 15);
            this.label_IsUseObjectExtract.Name = "label_IsUseObjectExtract";
            this.label_IsUseObjectExtract.Size = new System.Drawing.Size(333, 18);
            this.label_IsUseObjectExtract.TabIndex = 329;
            this.label_IsUseObjectExtract.Text = "Would you like to use \"Object Extract\"?";
            // 
            // label_TextColorSegment
            // 
            this.label_TextColorSegment.AutoSize = true;
            this.label_TextColorSegment.Location = new System.Drawing.Point(21, 529);
            this.label_TextColorSegment.Name = "label_TextColorSegment";
            this.label_TextColorSegment.Size = new System.Drawing.Size(148, 14);
            this.label_TextColorSegment.TabIndex = 342;
            this.label_TextColorSegment.Text = "2. Color Segmentation";
            // 
            // label_TextBlob
            // 
            this.label_TextBlob.AutoSize = true;
            this.label_TextBlob.Location = new System.Drawing.Point(21, 494);
            this.label_TextBlob.Name = "label_TextBlob";
            this.label_TextBlob.Size = new System.Drawing.Size(50, 14);
            this.label_TextBlob.TabIndex = 341;
            this.label_TextBlob.Text = "1. Blob";
            // 
            // checkBox_IsUseColorSegment
            // 
            this.checkBox_IsUseColorSegment.AutoSize = true;
            this.checkBox_IsUseColorSegment.Location = new System.Drawing.Point(200, 529);
            this.checkBox_IsUseColorSegment.Name = "checkBox_IsUseColorSegment";
            this.checkBox_IsUseColorSegment.Size = new System.Drawing.Size(15, 14);
            this.checkBox_IsUseColorSegment.TabIndex = 338;
            this.checkBox_IsUseColorSegment.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsUseBlob
            // 
            this.checkBox_IsUseBlob.AutoSize = true;
            this.checkBox_IsUseBlob.Location = new System.Drawing.Point(200, 494);
            this.checkBox_IsUseBlob.Name = "checkBox_IsUseBlob";
            this.checkBox_IsUseBlob.Size = new System.Drawing.Size(15, 14);
            this.checkBox_IsUseBlob.TabIndex = 337;
            this.checkBox_IsUseBlob.UseVisualStyleBackColor = true;
            // 
            // label_Text_SourceImage
            // 
            this.label_Text_SourceImage.AutoSize = true;
            this.label_Text_SourceImage.Location = new System.Drawing.Point(10, 63);
            this.label_Text_SourceImage.Name = "label_Text_SourceImage";
            this.label_Text_SourceImage.Size = new System.Drawing.Size(103, 14);
            this.label_Text_SourceImage.TabIndex = 361;
            this.label_Text_SourceImage.Text = "Source Image :";
            // 
            // button_SetSourceImage
            // 
            this.button_SetSourceImage.Location = new System.Drawing.Point(370, 60);
            this.button_SetSourceImage.Name = "button_SetSourceImage";
            this.button_SetSourceImage.Size = new System.Drawing.Size(70, 25);
            this.button_SetSourceImage.TabIndex = 360;
            this.button_SetSourceImage.Text = "Set";
            this.button_SetSourceImage.UseVisualStyleBackColor = true;
            this.button_SetSourceImage.Click += new System.EventHandler(this.button_SetSourceImage_Click);
            // 
            // comboBox_SelectSourceImage
            // 
            this.comboBox_SelectSourceImage.FormattingEnabled = true;
            this.comboBox_SelectSourceImage.Location = new System.Drawing.Point(120, 60);
            this.comboBox_SelectSourceImage.Name = "comboBox_SelectSourceImage";
            this.comboBox_SelectSourceImage.Size = new System.Drawing.Size(240, 22);
            this.comboBox_SelectSourceImage.TabIndex = 359;
            // 
            // label_TextCombineOption
            // 
            this.label_TextCombineOption.AutoSize = true;
            this.label_TextCombineOption.Location = new System.Drawing.Point(21, 562);
            this.label_TextCombineOption.Name = "label_TextCombineOption";
            this.label_TextCombineOption.Size = new System.Drawing.Size(124, 14);
            this.label_TextCombineOption.TabIndex = 362;
            this.label_TextCombineOption.Text = "3. Combine Option";
            // 
            // radioButton_CombineOverlapMin
            // 
            this.radioButton_CombineOverlapMin.AutoSize = true;
            this.radioButton_CombineOverlapMin.Location = new System.Drawing.Point(200, 560);
            this.radioButton_CombineOverlapMin.Name = "radioButton_CombineOverlapMin";
            this.radioButton_CombineOverlapMin.Size = new System.Drawing.Size(46, 18);
            this.radioButton_CombineOverlapMin.TabIndex = 364;
            this.radioButton_CombineOverlapMin.TabStop = true;
            this.radioButton_CombineOverlapMin.Text = "Min";
            this.radioButton_CombineOverlapMin.UseVisualStyleBackColor = true;
            // 
            // radioButton_CombineOverlapMax
            // 
            this.radioButton_CombineOverlapMax.AutoSize = true;
            this.radioButton_CombineOverlapMax.Location = new System.Drawing.Point(357, 560);
            this.radioButton_CombineOverlapMax.Name = "radioButton_CombineOverlapMax";
            this.radioButton_CombineOverlapMax.Size = new System.Drawing.Size(50, 18);
            this.radioButton_CombineOverlapMax.TabIndex = 366;
            this.radioButton_CombineOverlapMax.TabStop = true;
            this.radioButton_CombineOverlapMax.Text = "Max";
            this.radioButton_CombineOverlapMax.UseVisualStyleBackColor = true;
            // 
            // comboBox_SelectBlobSource
            // 
            this.comboBox_SelectBlobSource.FormattingEnabled = true;
            this.comboBox_SelectBlobSource.Location = new System.Drawing.Point(224, 490);
            this.comboBox_SelectBlobSource.Name = "comboBox_SelectBlobSource";
            this.comboBox_SelectBlobSource.Size = new System.Drawing.Size(220, 22);
            this.comboBox_SelectBlobSource.TabIndex = 367;
            // 
            // comboBox_SelectColorSegmentSource
            // 
            this.comboBox_SelectColorSegmentSource.FormattingEnabled = true;
            this.comboBox_SelectColorSegmentSource.Location = new System.Drawing.Point(224, 525);
            this.comboBox_SelectColorSegmentSource.Name = "comboBox_SelectColorSegmentSource";
            this.comboBox_SelectColorSegmentSource.Size = new System.Drawing.Size(220, 22);
            this.comboBox_SelectColorSegmentSource.TabIndex = 368;
            // 
            // button_UpdateSetting
            // 
            this.button_UpdateSetting.Location = new System.Drawing.Point(1240, 60);
            this.button_UpdateSetting.Name = "button_UpdateSetting";
            this.button_UpdateSetting.Size = new System.Drawing.Size(80, 25);
            this.button_UpdateSetting.TabIndex = 379;
            this.button_UpdateSetting.Text = "Update";
            this.button_UpdateSetting.UseVisualStyleBackColor = true;
            this.button_UpdateSetting.Click += new System.EventHandler(this.button_UpdateSetting_Click);
            // 
            // propertyGrid_ParamSetting
            // 
            this.propertyGrid_ParamSetting.Location = new System.Drawing.Point(920, 90);
            this.propertyGrid_ParamSetting.Name = "propertyGrid_ParamSetting";
            this.propertyGrid_ParamSetting.Size = new System.Drawing.Size(400, 500);
            this.propertyGrid_ParamSetting.TabIndex = 378;
            // 
            // label_TextAdvancedSetting
            // 
            this.label_TextAdvancedSetting.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextAdvancedSetting.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextAdvancedSetting.ForeColor = System.Drawing.Color.White;
            this.label_TextAdvancedSetting.Location = new System.Drawing.Point(920, 60);
            this.label_TextAdvancedSetting.Name = "label_TextAdvancedSetting";
            this.label_TextAdvancedSetting.Size = new System.Drawing.Size(320, 25);
            this.label_TextAdvancedSetting.TabIndex = 377;
            this.label_TextAdvancedSetting.Text = "Advanced Setting.";
            this.label_TextAdvancedSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormTab_ObjectExtract
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1350, 600);
            this.Controls.Add(this.button_UpdateSetting);
            this.Controls.Add(this.propertyGrid_ParamSetting);
            this.Controls.Add(this.label_TextAdvancedSetting);
            this.Controls.Add(this.comboBox_SelectColorSegmentSource);
            this.Controls.Add(this.comboBox_SelectBlobSource);
            this.Controls.Add(this.radioButton_CombineOverlapMax);
            this.Controls.Add(this.radioButton_CombineOverlapMin);
            this.Controls.Add(this.label_TextCombineOption);
            this.Controls.Add(this.label_Text_SourceImage);
            this.Controls.Add(this.button_SetSourceImage);
            this.Controls.Add(this.comboBox_SelectSourceImage);
            this.Controls.Add(this.button_ProcessObjectExtract);
            this.Controls.Add(this.label_TextColorSegment);
            this.Controls.Add(this.label_TextBlob);
            this.Controls.Add(this.checkBox_IsUseColorSegment);
            this.Controls.Add(this.checkBox_IsUseBlob);
            this.Controls.Add(this.button_SettingColorSegment);
            this.Controls.Add(this.button_SettingBlob);
            this.Controls.Add(this.cogDisplay_ProcessView);
            this.Controls.Add(this.cogDisplay_PrevView);
            this.Controls.Add(this.label_TextSetting);
            this.Controls.Add(this.label_TextInspectionTest);
            this.Controls.Add(this.checkBox_IsUseObjectExtract);
            this.Controls.Add(this.label_IsUseObjectExtract);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormTab_ObjectExtract";
            this.Text = "FormTab_Preprocessing";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTab_ObjectExtract_FormClosed);
            this.Load += new System.EventHandler(this.FormTab_ObjectExtract_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ProcessView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_PrevView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ProcessObjectExtract;
        private System.Windows.Forms.Button button_SettingColorSegment;
        private System.Windows.Forms.Button button_SettingBlob;
        internal Cognex.VisionPro.Display.CogDisplay cogDisplay_ProcessView;
        internal Cognex.VisionPro.Display.CogDisplay cogDisplay_PrevView;
        private System.Windows.Forms.Label label_TextSetting;
        private System.Windows.Forms.Label label_TextInspectionTest;
        private System.Windows.Forms.CheckBox checkBox_IsUseObjectExtract;
        private System.Windows.Forms.Label label_IsUseObjectExtract;
        private System.Windows.Forms.Label label_TextColorSegment;
        private System.Windows.Forms.Label label_TextBlob;
        private System.Windows.Forms.CheckBox checkBox_IsUseColorSegment;
        private System.Windows.Forms.CheckBox checkBox_IsUseBlob;
        private System.Windows.Forms.Label label_Text_SourceImage;
        private System.Windows.Forms.Button button_SetSourceImage;
        private System.Windows.Forms.ComboBox comboBox_SelectSourceImage;
        private System.Windows.Forms.Label label_TextCombineOption;
        private System.Windows.Forms.RadioButton radioButton_CombineOverlapMin;
        private System.Windows.Forms.RadioButton radioButton_CombineOverlapMax;
        private System.Windows.Forms.ComboBox comboBox_SelectBlobSource;
        private System.Windows.Forms.ComboBox comboBox_SelectColorSegmentSource;
        private System.Windows.Forms.Button button_UpdateSetting;
        private System.Windows.Forms.PropertyGrid propertyGrid_ParamSetting;
        private System.Windows.Forms.Label label_TextAdvancedSetting;
    }
}