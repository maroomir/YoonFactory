namespace RobotIntegratedVision
{
    partial class FormTab_PatternMatching
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTab_PatternMatching));
            this.label_TextAlignResult = new System.Windows.Forms.Label();
            this.dataGridView_AlignResult = new System.Windows.Forms.DataGridView();
            this.button_ProcessPM = new System.Windows.Forms.Button();
            this.cogDisplay_SecondPattern = new Cognex.VisionPro.Display.CogDisplay();
            this.label_TextAddOption = new System.Windows.Forms.Label();
            this.button_SettingSecondPattern = new System.Windows.Forms.Button();
            this.button_SettingMainPattern = new System.Windows.Forms.Button();
            this.checkBox_IsUseMainPattern = new System.Windows.Forms.CheckBox();
            this.cogDisplay_MainPattern = new Cognex.VisionPro.Display.CogDisplay();
            this.label_TextPattern = new System.Windows.Forms.Label();
            this.checkBox_IsUsePM = new System.Windows.Forms.CheckBox();
            this.label_TextSetting = new System.Windows.Forms.Label();
            this.cogDisplay_ProcessView = new Cognex.VisionPro.Display.CogDisplay();
            this.label_IsUsePM = new System.Windows.Forms.Label();
            this.label_TextInspectionTest = new System.Windows.Forms.Label();
            this.checkBox_IsUseSecondPattern = new System.Windows.Forms.CheckBox();
            this.cogDisplay_ThirdPattern = new Cognex.VisionPro.Display.CogDisplay();
            this.cogDisplay_ForthPattern = new Cognex.VisionPro.Display.CogDisplay();
            this.button_SettingThirdPattern = new System.Windows.Forms.Button();
            this.button_SettingForthPattern = new System.Windows.Forms.Button();
            this.checkBox_IsUseThirdPattern = new System.Windows.Forms.CheckBox();
            this.checkBox_IsUseForthPattern = new System.Windows.Forms.CheckBox();
            this.button_SetSourceImage = new System.Windows.Forms.Button();
            this.comboBox_SelectSourceImage = new System.Windows.Forms.ComboBox();
            this.label_Text_SourceImage = new System.Windows.Forms.Label();
            this.checkBox_IsUseMultiPatternInspection = new System.Windows.Forms.CheckBox();
            this.button_UpdateSetting = new System.Windows.Forms.Button();
            this.propertyGrid_ParamSetting = new System.Windows.Forms.PropertyGrid();
            this.label_TextAdvancedSetting = new System.Windows.Forms.Label();
            this.checkBox_IsCheckAlignOffset = new System.Windows.Forms.CheckBox();
            this.button_SetOrigin = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_AlignResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_SecondPattern)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_MainPattern)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ProcessView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ThirdPattern)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ForthPattern)).BeginInit();
            this.SuspendLayout();
            // 
            // label_TextAlignResult
            // 
            this.label_TextAlignResult.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextAlignResult.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextAlignResult.ForeColor = System.Drawing.Color.White;
            this.label_TextAlignResult.Location = new System.Drawing.Point(455, 484);
            this.label_TextAlignResult.Name = "label_TextAlignResult";
            this.label_TextAlignResult.Size = new System.Drawing.Size(280, 25);
            this.label_TextAlignResult.TabIndex = 341;
            this.label_TextAlignResult.Text = "Align Result.";
            this.label_TextAlignResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridView_AlignResult
            // 
            this.dataGridView_AlignResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_AlignResult.Location = new System.Drawing.Point(455, 512);
            this.dataGridView_AlignResult.Name = "dataGridView_AlignResult";
            this.dataGridView_AlignResult.RowHeadersWidth = 62;
            this.dataGridView_AlignResult.RowTemplate.Height = 23;
            this.dataGridView_AlignResult.Size = new System.Drawing.Size(480, 80);
            this.dataGridView_AlignResult.TabIndex = 340;
            // 
            // button_ProcessPM
            // 
            this.button_ProcessPM.Location = new System.Drawing.Point(845, 50);
            this.button_ProcessPM.Name = "button_ProcessPM";
            this.button_ProcessPM.Size = new System.Drawing.Size(90, 25);
            this.button_ProcessPM.TabIndex = 339;
            this.button_ProcessPM.Text = "Process";
            this.button_ProcessPM.UseVisualStyleBackColor = true;
            this.button_ProcessPM.Click += new System.EventHandler(this.button_ProcessPM_Click);
            // 
            // cogDisplay_SecondPattern
            // 
            this.cogDisplay_SecondPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cogDisplay_SecondPattern.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_SecondPattern.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_SecondPattern.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_SecondPattern.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_SecondPattern.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_SecondPattern.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_SecondPattern.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_SecondPattern.Location = new System.Drawing.Point(250, 95);
            this.cogDisplay_SecondPattern.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_SecondPattern.MouseWheelSensitivity = 1D;
            this.cogDisplay_SecondPattern.Name = "cogDisplay_SecondPattern";
            this.cogDisplay_SecondPattern.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_SecondPattern.OcxState")));
            this.cogDisplay_SecondPattern.Size = new System.Drawing.Size(180, 180);
            this.cogDisplay_SecondPattern.TabIndex = 336;
            // 
            // label_TextAddOption
            // 
            this.label_TextAddOption.AutoSize = true;
            this.label_TextAddOption.Location = new System.Drawing.Point(10, 522);
            this.label_TextAddOption.Name = "label_TextAddOption";
            this.label_TextAddOption.Size = new System.Drawing.Size(131, 14);
            this.label_TextAddOption.TabIndex = 335;
            this.label_TextAddOption.Text = "2. Additional Option";
            // 
            // button_SettingSecondPattern
            // 
            this.button_SettingSecondPattern.Location = new System.Drawing.Point(315, 274);
            this.button_SettingSecondPattern.Name = "button_SettingSecondPattern";
            this.button_SettingSecondPattern.Size = new System.Drawing.Size(100, 25);
            this.button_SettingSecondPattern.TabIndex = 334;
            this.button_SettingSecondPattern.Text = "2nd Pattern";
            this.button_SettingSecondPattern.UseVisualStyleBackColor = true;
            this.button_SettingSecondPattern.Click += new System.EventHandler(this.button_SettingSecondPattern_Click);
            // 
            // button_SettingMainPattern
            // 
            this.button_SettingMainPattern.Location = new System.Drawing.Point(88, 277);
            this.button_SettingMainPattern.Name = "button_SettingMainPattern";
            this.button_SettingMainPattern.Size = new System.Drawing.Size(100, 25);
            this.button_SettingMainPattern.TabIndex = 333;
            this.button_SettingMainPattern.Text = "Main Pattern";
            this.button_SettingMainPattern.UseVisualStyleBackColor = true;
            this.button_SettingMainPattern.Click += new System.EventHandler(this.button_SettingMainPattern_Click);
            // 
            // checkBox_IsUseMainPattern
            // 
            this.checkBox_IsUseMainPattern.AutoSize = true;
            this.checkBox_IsUseMainPattern.Location = new System.Drawing.Point(30, 281);
            this.checkBox_IsUseMainPattern.Name = "checkBox_IsUseMainPattern";
            this.checkBox_IsUseMainPattern.Size = new System.Drawing.Size(50, 18);
            this.checkBox_IsUseMainPattern.TabIndex = 332;
            this.checkBox_IsUseMainPattern.Text = "Use";
            this.checkBox_IsUseMainPattern.UseVisualStyleBackColor = true;
            // 
            // cogDisplay_MainPattern
            // 
            this.cogDisplay_MainPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cogDisplay_MainPattern.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_MainPattern.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_MainPattern.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_MainPattern.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_MainPattern.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_MainPattern.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_MainPattern.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_MainPattern.Location = new System.Drawing.Point(30, 95);
            this.cogDisplay_MainPattern.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_MainPattern.MouseWheelSensitivity = 1D;
            this.cogDisplay_MainPattern.Name = "cogDisplay_MainPattern";
            this.cogDisplay_MainPattern.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_MainPattern.OcxState")));
            this.cogDisplay_MainPattern.Size = new System.Drawing.Size(180, 180);
            this.cogDisplay_MainPattern.TabIndex = 331;
            // 
            // label_TextPattern
            // 
            this.label_TextPattern.AutoSize = true;
            this.label_TextPattern.Location = new System.Drawing.Point(10, 80);
            this.label_TextPattern.Name = "label_TextPattern";
            this.label_TextPattern.Size = new System.Drawing.Size(70, 14);
            this.label_TextPattern.TabIndex = 330;
            this.label_TextPattern.Text = "1. Pattern";
            // 
            // checkBox_IsUsePM
            // 
            this.checkBox_IsUsePM.AutoSize = true;
            this.checkBox_IsUsePM.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_IsUsePM.Location = new System.Drawing.Point(480, 15);
            this.checkBox_IsUsePM.Name = "checkBox_IsUsePM";
            this.checkBox_IsUsePM.Size = new System.Drawing.Size(56, 22);
            this.checkBox_IsUsePM.TabIndex = 328;
            this.checkBox_IsUsePM.Text = "Yes";
            this.checkBox_IsUsePM.UseVisualStyleBackColor = true;
            // 
            // label_TextSetting
            // 
            this.label_TextSetting.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextSetting.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextSetting.ForeColor = System.Drawing.Color.White;
            this.label_TextSetting.Location = new System.Drawing.Point(10, 50);
            this.label_TextSetting.Name = "label_TextSetting";
            this.label_TextSetting.Size = new System.Drawing.Size(280, 25);
            this.label_TextSetting.TabIndex = 326;
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
            this.cogDisplay_ProcessView.Location = new System.Drawing.Point(455, 108);
            this.cogDisplay_ProcessView.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_ProcessView.MouseWheelSensitivity = 1D;
            this.cogDisplay_ProcessView.Name = "cogDisplay_ProcessView";
            this.cogDisplay_ProcessView.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_ProcessView.OcxState")));
            this.cogDisplay_ProcessView.Size = new System.Drawing.Size(480, 360);
            this.cogDisplay_ProcessView.TabIndex = 325;
            // 
            // label_IsUsePM
            // 
            this.label_IsUsePM.AutoSize = true;
            this.label_IsUsePM.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_IsUsePM.Location = new System.Drawing.Point(30, 15);
            this.label_IsUsePM.Name = "label_IsUsePM";
            this.label_IsUsePM.Size = new System.Drawing.Size(355, 18);
            this.label_IsUsePM.TabIndex = 324;
            this.label_IsUsePM.Text = "Would you like to use \"Pattern Matching\"?";
            // 
            // label_TextInspectionTest
            // 
            this.label_TextInspectionTest.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextInspectionTest.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextInspectionTest.ForeColor = System.Drawing.Color.White;
            this.label_TextInspectionTest.Location = new System.Drawing.Point(455, 50);
            this.label_TextInspectionTest.Name = "label_TextInspectionTest";
            this.label_TextInspectionTest.Size = new System.Drawing.Size(280, 25);
            this.label_TextInspectionTest.TabIndex = 323;
            this.label_TextInspectionTest.Text = "Inspection Test.";
            this.label_TextInspectionTest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBox_IsUseSecondPattern
            // 
            this.checkBox_IsUseSecondPattern.AutoSize = true;
            this.checkBox_IsUseSecondPattern.Location = new System.Drawing.Point(250, 277);
            this.checkBox_IsUseSecondPattern.Name = "checkBox_IsUseSecondPattern";
            this.checkBox_IsUseSecondPattern.Size = new System.Drawing.Size(50, 18);
            this.checkBox_IsUseSecondPattern.TabIndex = 342;
            this.checkBox_IsUseSecondPattern.Text = "Use";
            this.checkBox_IsUseSecondPattern.UseVisualStyleBackColor = true;
            // 
            // cogDisplay_ThirdPattern
            // 
            this.cogDisplay_ThirdPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cogDisplay_ThirdPattern.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_ThirdPattern.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_ThirdPattern.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_ThirdPattern.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_ThirdPattern.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_ThirdPattern.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_ThirdPattern.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_ThirdPattern.Location = new System.Drawing.Point(30, 303);
            this.cogDisplay_ThirdPattern.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_ThirdPattern.MouseWheelSensitivity = 1D;
            this.cogDisplay_ThirdPattern.Name = "cogDisplay_ThirdPattern";
            this.cogDisplay_ThirdPattern.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_ThirdPattern.OcxState")));
            this.cogDisplay_ThirdPattern.Size = new System.Drawing.Size(180, 180);
            this.cogDisplay_ThirdPattern.TabIndex = 350;
            // 
            // cogDisplay_ForthPattern
            // 
            this.cogDisplay_ForthPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cogDisplay_ForthPattern.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_ForthPattern.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_ForthPattern.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_ForthPattern.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_ForthPattern.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_ForthPattern.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_ForthPattern.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_ForthPattern.Location = new System.Drawing.Point(250, 303);
            this.cogDisplay_ForthPattern.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_ForthPattern.MouseWheelSensitivity = 1D;
            this.cogDisplay_ForthPattern.Name = "cogDisplay_ForthPattern";
            this.cogDisplay_ForthPattern.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_ForthPattern.OcxState")));
            this.cogDisplay_ForthPattern.Size = new System.Drawing.Size(180, 180);
            this.cogDisplay_ForthPattern.TabIndex = 351;
            // 
            // button_SettingThirdPattern
            // 
            this.button_SettingThirdPattern.Location = new System.Drawing.Point(88, 485);
            this.button_SettingThirdPattern.Name = "button_SettingThirdPattern";
            this.button_SettingThirdPattern.Size = new System.Drawing.Size(100, 25);
            this.button_SettingThirdPattern.TabIndex = 352;
            this.button_SettingThirdPattern.Text = "3rd Pattern";
            this.button_SettingThirdPattern.UseVisualStyleBackColor = true;
            this.button_SettingThirdPattern.Click += new System.EventHandler(this.button_SettingThirdPattern_Click);
            // 
            // button_SettingForthPattern
            // 
            this.button_SettingForthPattern.Location = new System.Drawing.Point(315, 485);
            this.button_SettingForthPattern.Name = "button_SettingForthPattern";
            this.button_SettingForthPattern.Size = new System.Drawing.Size(100, 25);
            this.button_SettingForthPattern.TabIndex = 353;
            this.button_SettingForthPattern.Text = "4th Pattern";
            this.button_SettingForthPattern.UseVisualStyleBackColor = true;
            this.button_SettingForthPattern.Click += new System.EventHandler(this.button_SettingForthPattern_Click);
            // 
            // checkBox_IsUseThirdPattern
            // 
            this.checkBox_IsUseThirdPattern.AutoSize = true;
            this.checkBox_IsUseThirdPattern.Location = new System.Drawing.Point(30, 489);
            this.checkBox_IsUseThirdPattern.Name = "checkBox_IsUseThirdPattern";
            this.checkBox_IsUseThirdPattern.Size = new System.Drawing.Size(50, 18);
            this.checkBox_IsUseThirdPattern.TabIndex = 354;
            this.checkBox_IsUseThirdPattern.Text = "Use";
            this.checkBox_IsUseThirdPattern.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsUseForthPattern
            // 
            this.checkBox_IsUseForthPattern.AutoSize = true;
            this.checkBox_IsUseForthPattern.Location = new System.Drawing.Point(250, 489);
            this.checkBox_IsUseForthPattern.Name = "checkBox_IsUseForthPattern";
            this.checkBox_IsUseForthPattern.Size = new System.Drawing.Size(50, 18);
            this.checkBox_IsUseForthPattern.TabIndex = 355;
            this.checkBox_IsUseForthPattern.Text = "Use";
            this.checkBox_IsUseForthPattern.UseVisualStyleBackColor = true;
            // 
            // button_SetSourceImage
            // 
            this.button_SetSourceImage.Location = new System.Drawing.Point(865, 80);
            this.button_SetSourceImage.Name = "button_SetSourceImage";
            this.button_SetSourceImage.Size = new System.Drawing.Size(70, 25);
            this.button_SetSourceImage.TabIndex = 357;
            this.button_SetSourceImage.Text = "Set";
            this.button_SetSourceImage.UseVisualStyleBackColor = true;
            this.button_SetSourceImage.Click += new System.EventHandler(this.button_SetSourceImage_Click);
            // 
            // comboBox_SelectSourceImage
            // 
            this.comboBox_SelectSourceImage.FormattingEnabled = true;
            this.comboBox_SelectSourceImage.Location = new System.Drawing.Point(570, 81);
            this.comboBox_SelectSourceImage.Name = "comboBox_SelectSourceImage";
            this.comboBox_SelectSourceImage.Size = new System.Drawing.Size(280, 22);
            this.comboBox_SelectSourceImage.TabIndex = 356;
            // 
            // label_Text_SourceImage
            // 
            this.label_Text_SourceImage.AutoSize = true;
            this.label_Text_SourceImage.Location = new System.Drawing.Point(455, 84);
            this.label_Text_SourceImage.Name = "label_Text_SourceImage";
            this.label_Text_SourceImage.Size = new System.Drawing.Size(103, 14);
            this.label_Text_SourceImage.TabIndex = 358;
            this.label_Text_SourceImage.Text = "Source Image :";
            // 
            // checkBox_IsUseMultiPatternInspection
            // 
            this.checkBox_IsUseMultiPatternInspection.AutoSize = true;
            this.checkBox_IsUseMultiPatternInspection.Location = new System.Drawing.Point(30, 545);
            this.checkBox_IsUseMultiPatternInspection.Name = "checkBox_IsUseMultiPatternInspection";
            this.checkBox_IsUseMultiPatternInspection.Size = new System.Drawing.Size(282, 18);
            this.checkBox_IsUseMultiPatternInspection.TabIndex = 359;
            this.checkBox_IsUseMultiPatternInspection.Text = "Use Multi-Pattern (2nd, 3rd, 4th Pattern)";
            this.checkBox_IsUseMultiPatternInspection.UseVisualStyleBackColor = true;
            // 
            // button_UpdateSetting
            // 
            this.button_UpdateSetting.Location = new System.Drawing.Point(1235, 50);
            this.button_UpdateSetting.Name = "button_UpdateSetting";
            this.button_UpdateSetting.Size = new System.Drawing.Size(80, 25);
            this.button_UpdateSetting.TabIndex = 382;
            this.button_UpdateSetting.Text = "Update";
            this.button_UpdateSetting.UseVisualStyleBackColor = true;
            this.button_UpdateSetting.Click += new System.EventHandler(this.button_UpdateSetting_Click);
            // 
            // propertyGrid_ParamSetting
            // 
            this.propertyGrid_ParamSetting.Location = new System.Drawing.Point(965, 80);
            this.propertyGrid_ParamSetting.Name = "propertyGrid_ParamSetting";
            this.propertyGrid_ParamSetting.Size = new System.Drawing.Size(350, 513);
            this.propertyGrid_ParamSetting.TabIndex = 381;
            // 
            // label_TextAdvancedSetting
            // 
            this.label_TextAdvancedSetting.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextAdvancedSetting.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextAdvancedSetting.ForeColor = System.Drawing.Color.White;
            this.label_TextAdvancedSetting.Location = new System.Drawing.Point(965, 50);
            this.label_TextAdvancedSetting.Name = "label_TextAdvancedSetting";
            this.label_TextAdvancedSetting.Size = new System.Drawing.Size(270, 25);
            this.label_TextAdvancedSetting.TabIndex = 380;
            this.label_TextAdvancedSetting.Text = "Advanced Setting.";
            this.label_TextAdvancedSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBox_IsCheckAlignOffset
            // 
            this.checkBox_IsCheckAlignOffset.AutoSize = true;
            this.checkBox_IsCheckAlignOffset.Location = new System.Drawing.Point(30, 570);
            this.checkBox_IsCheckAlignOffset.Name = "checkBox_IsCheckAlignOffset";
            this.checkBox_IsCheckAlignOffset.Size = new System.Drawing.Size(168, 18);
            this.checkBox_IsCheckAlignOffset.TabIndex = 383;
            this.checkBox_IsCheckAlignOffset.Text = "Use Align Offset Check";
            this.checkBox_IsCheckAlignOffset.UseVisualStyleBackColor = true;
            // 
            // button_SetOrigin
            // 
            this.button_SetOrigin.Location = new System.Drawing.Point(755, 50);
            this.button_SetOrigin.Name = "button_SetOrigin";
            this.button_SetOrigin.Size = new System.Drawing.Size(80, 25);
            this.button_SetOrigin.TabIndex = 384;
            this.button_SetOrigin.Text = "Origin";
            this.button_SetOrigin.UseVisualStyleBackColor = true;
            this.button_SetOrigin.Click += new System.EventHandler(this.button_SetOrigin_Click);
            // 
            // FormTab_PatternMatching
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1350, 600);
            this.Controls.Add(this.button_SetOrigin);
            this.Controls.Add(this.checkBox_IsCheckAlignOffset);
            this.Controls.Add(this.button_UpdateSetting);
            this.Controls.Add(this.propertyGrid_ParamSetting);
            this.Controls.Add(this.label_TextAdvancedSetting);
            this.Controls.Add(this.checkBox_IsUseMultiPatternInspection);
            this.Controls.Add(this.label_Text_SourceImage);
            this.Controls.Add(this.button_SetSourceImage);
            this.Controls.Add(this.comboBox_SelectSourceImage);
            this.Controls.Add(this.checkBox_IsUseForthPattern);
            this.Controls.Add(this.checkBox_IsUseThirdPattern);
            this.Controls.Add(this.button_SettingForthPattern);
            this.Controls.Add(this.button_SettingThirdPattern);
            this.Controls.Add(this.cogDisplay_ForthPattern);
            this.Controls.Add(this.cogDisplay_ThirdPattern);
            this.Controls.Add(this.checkBox_IsUseSecondPattern);
            this.Controls.Add(this.label_TextAlignResult);
            this.Controls.Add(this.dataGridView_AlignResult);
            this.Controls.Add(this.button_ProcessPM);
            this.Controls.Add(this.cogDisplay_SecondPattern);
            this.Controls.Add(this.label_TextAddOption);
            this.Controls.Add(this.button_SettingSecondPattern);
            this.Controls.Add(this.button_SettingMainPattern);
            this.Controls.Add(this.checkBox_IsUseMainPattern);
            this.Controls.Add(this.cogDisplay_MainPattern);
            this.Controls.Add(this.label_TextPattern);
            this.Controls.Add(this.checkBox_IsUsePM);
            this.Controls.Add(this.label_TextSetting);
            this.Controls.Add(this.cogDisplay_ProcessView);
            this.Controls.Add(this.label_IsUsePM);
            this.Controls.Add(this.label_TextInspectionTest);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormTab_PatternMatching";
            this.Text = "FormTab_Preprocessing";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTab_PatternMatching_FormClosed);
            this.Load += new System.EventHandler(this.FormTab_PatternMatching_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_AlignResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_SecondPattern)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_MainPattern)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ProcessView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ThirdPattern)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ForthPattern)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_TextAlignResult;
        private System.Windows.Forms.DataGridView dataGridView_AlignResult;
        private System.Windows.Forms.Button button_ProcessPM;
        internal Cognex.VisionPro.Display.CogDisplay cogDisplay_SecondPattern;
        private System.Windows.Forms.Label label_TextAddOption;
        private System.Windows.Forms.Button button_SettingSecondPattern;
        private System.Windows.Forms.Button button_SettingMainPattern;
        private System.Windows.Forms.CheckBox checkBox_IsUseMainPattern;
        internal Cognex.VisionPro.Display.CogDisplay cogDisplay_MainPattern;
        private System.Windows.Forms.Label label_TextPattern;
        private System.Windows.Forms.CheckBox checkBox_IsUsePM;
        private System.Windows.Forms.Label label_TextSetting;
        internal Cognex.VisionPro.Display.CogDisplay cogDisplay_ProcessView;
        private System.Windows.Forms.Label label_IsUsePM;
        private System.Windows.Forms.Label label_TextInspectionTest;
        private System.Windows.Forms.CheckBox checkBox_IsUseSecondPattern;
        internal Cognex.VisionPro.Display.CogDisplay cogDisplay_ThirdPattern;
        internal Cognex.VisionPro.Display.CogDisplay cogDisplay_ForthPattern;
        private System.Windows.Forms.Button button_SettingThirdPattern;
        private System.Windows.Forms.Button button_SettingForthPattern;
        private System.Windows.Forms.CheckBox checkBox_IsUseThirdPattern;
        private System.Windows.Forms.CheckBox checkBox_IsUseForthPattern;
        private System.Windows.Forms.Button button_SetSourceImage;
        private System.Windows.Forms.ComboBox comboBox_SelectSourceImage;
        private System.Windows.Forms.Label label_Text_SourceImage;
        private System.Windows.Forms.CheckBox checkBox_IsUseMultiPatternInspection;
        private System.Windows.Forms.Button button_UpdateSetting;
        private System.Windows.Forms.PropertyGrid propertyGrid_ParamSetting;
        private System.Windows.Forms.Label label_TextAdvancedSetting;
        private System.Windows.Forms.CheckBox checkBox_IsCheckAlignOffset;
        private System.Windows.Forms.Button button_SetOrigin;
    }
}