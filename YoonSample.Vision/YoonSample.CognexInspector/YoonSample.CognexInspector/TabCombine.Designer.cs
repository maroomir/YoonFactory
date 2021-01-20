namespace RobotIntegratedVision
{
    partial class FormTab_Combine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTab_Combine));
            this.button_ProcessCombine = new System.Windows.Forms.Button();
            this.cogDisplay_ObjectView = new Cognex.VisionPro.Display.CogDisplay();
            this.cogDisplay_SourceView = new Cognex.VisionPro.Display.CogDisplay();
            this.label_TextCombine = new System.Windows.Forms.Label();
            this.checkBox_IsUseCombine = new System.Windows.Forms.CheckBox();
            this.label_IsUseCombine = new System.Windows.Forms.Label();
            this.button_SetSourceImage = new System.Windows.Forms.Button();
            this.comboBox_SelectSourceImage = new System.Windows.Forms.ComboBox();
            this.radioButton_CombineAdd = new System.Windows.Forms.RadioButton();
            this.radioButton_CombineOverlapMin = new System.Windows.Forms.RadioButton();
            this.radioButton_CombineSubtract = new System.Windows.Forms.RadioButton();
            this.radioButton_CombineOverlapMax = new System.Windows.Forms.RadioButton();
            this.button_SetObjectImage = new System.Windows.Forms.Button();
            this.comboBox_SelectObjectImage = new System.Windows.Forms.ComboBox();
            this.cogDisplay_CombineView = new Cognex.VisionPro.Display.CogDisplay();
            this.label_TextCombineOption = new System.Windows.Forms.Label();
            this.label_TextSource = new System.Windows.Forms.Label();
            this.label_TextObject = new System.Windows.Forms.Label();
            this.label_TextAdvancedSetting = new System.Windows.Forms.Label();
            this.propertyGrid_ParamSetting = new System.Windows.Forms.PropertyGrid();
            this.button_UpdateSetting = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ObjectView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_SourceView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_CombineView)).BeginInit();
            this.SuspendLayout();
            // 
            // button_ProcessCombine
            // 
            this.button_ProcessCombine.Location = new System.Drawing.Point(760, 60);
            this.button_ProcessCombine.Name = "button_ProcessCombine";
            this.button_ProcessCombine.Size = new System.Drawing.Size(100, 25);
            this.button_ProcessCombine.TabIndex = 343;
            this.button_ProcessCombine.Text = "Process";
            this.button_ProcessCombine.UseVisualStyleBackColor = true;
            this.button_ProcessCombine.Click += new System.EventHandler(this.button_ProcessCombine_Click);
            // 
            // cogDisplay_ObjectView
            // 
            this.cogDisplay_ObjectView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cogDisplay_ObjectView.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_ObjectView.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_ObjectView.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_ObjectView.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_ObjectView.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_ObjectView.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_ObjectView.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_ObjectView.Location = new System.Drawing.Point(10, 355);
            this.cogDisplay_ObjectView.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_ObjectView.MouseWheelSensitivity = 1D;
            this.cogDisplay_ObjectView.Name = "cogDisplay_ObjectView";
            this.cogDisplay_ObjectView.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_ObjectView.OcxState")));
            this.cogDisplay_ObjectView.Size = new System.Drawing.Size(320, 240);
            this.cogDisplay_ObjectView.TabIndex = 334;
            // 
            // cogDisplay_SourceView
            // 
            this.cogDisplay_SourceView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cogDisplay_SourceView.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_SourceView.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_SourceView.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_SourceView.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_SourceView.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_SourceView.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_SourceView.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_SourceView.Location = new System.Drawing.Point(10, 75);
            this.cogDisplay_SourceView.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_SourceView.MouseWheelSensitivity = 1D;
            this.cogDisplay_SourceView.Name = "cogDisplay_SourceView";
            this.cogDisplay_SourceView.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_SourceView.OcxState")));
            this.cogDisplay_SourceView.Size = new System.Drawing.Size(320, 240);
            this.cogDisplay_SourceView.TabIndex = 333;
            // 
            // label_TextCombine
            // 
            this.label_TextCombine.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextCombine.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextCombine.ForeColor = System.Drawing.Color.White;
            this.label_TextCombine.Location = new System.Drawing.Point(385, 60);
            this.label_TextCombine.Name = "label_TextCombine";
            this.label_TextCombine.Size = new System.Drawing.Size(280, 25);
            this.label_TextCombine.TabIndex = 332;
            this.label_TextCombine.Text = "Combine.";
            this.label_TextCombine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBox_IsUseCombine
            // 
            this.checkBox_IsUseCombine.AutoSize = true;
            this.checkBox_IsUseCombine.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_IsUseCombine.Location = new System.Drawing.Point(480, 15);
            this.checkBox_IsUseCombine.Name = "checkBox_IsUseCombine";
            this.checkBox_IsUseCombine.Size = new System.Drawing.Size(56, 22);
            this.checkBox_IsUseCombine.TabIndex = 330;
            this.checkBox_IsUseCombine.Text = "Yes";
            this.checkBox_IsUseCombine.UseVisualStyleBackColor = true;
            // 
            // label_IsUseCombine
            // 
            this.label_IsUseCombine.AutoSize = true;
            this.label_IsUseCombine.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_IsUseCombine.Location = new System.Drawing.Point(30, 15);
            this.label_IsUseCombine.Name = "label_IsUseCombine";
            this.label_IsUseCombine.Size = new System.Drawing.Size(287, 18);
            this.label_IsUseCombine.TabIndex = 329;
            this.label_IsUseCombine.Text = "Would you like to use \"Combine\"?";
            // 
            // button_SetSourceImage
            // 
            this.button_SetSourceImage.Location = new System.Drawing.Point(245, 48);
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
            this.comboBox_SelectSourceImage.Location = new System.Drawing.Point(71, 49);
            this.comboBox_SelectSourceImage.Name = "comboBox_SelectSourceImage";
            this.comboBox_SelectSourceImage.Size = new System.Drawing.Size(170, 22);
            this.comboBox_SelectSourceImage.TabIndex = 359;
            // 
            // radioButton_CombineAdd
            // 
            this.radioButton_CombineAdd.AutoSize = true;
            this.radioButton_CombineAdd.Location = new System.Drawing.Point(401, 514);
            this.radioButton_CombineAdd.Name = "radioButton_CombineAdd";
            this.radioButton_CombineAdd.Size = new System.Drawing.Size(49, 18);
            this.radioButton_CombineAdd.TabIndex = 363;
            this.radioButton_CombineAdd.TabStop = true;
            this.radioButton_CombineAdd.Text = "Add";
            this.radioButton_CombineAdd.UseVisualStyleBackColor = true;
            // 
            // radioButton_CombineOverlapMin
            // 
            this.radioButton_CombineOverlapMin.AutoSize = true;
            this.radioButton_CombineOverlapMin.Location = new System.Drawing.Point(477, 514);
            this.radioButton_CombineOverlapMin.Name = "radioButton_CombineOverlapMin";
            this.radioButton_CombineOverlapMin.Size = new System.Drawing.Size(46, 18);
            this.radioButton_CombineOverlapMin.TabIndex = 364;
            this.radioButton_CombineOverlapMin.TabStop = true;
            this.radioButton_CombineOverlapMin.Text = "Min";
            this.radioButton_CombineOverlapMin.UseVisualStyleBackColor = true;
            // 
            // radioButton_CombineSubtract
            // 
            this.radioButton_CombineSubtract.AutoSize = true;
            this.radioButton_CombineSubtract.Location = new System.Drawing.Point(639, 514);
            this.radioButton_CombineSubtract.Name = "radioButton_CombineSubtract";
            this.radioButton_CombineSubtract.Size = new System.Drawing.Size(78, 18);
            this.radioButton_CombineSubtract.TabIndex = 365;
            this.radioButton_CombineSubtract.TabStop = true;
            this.radioButton_CombineSubtract.Text = "Subtract";
            this.radioButton_CombineSubtract.UseVisualStyleBackColor = true;
            // 
            // radioButton_CombineOverlapMax
            // 
            this.radioButton_CombineOverlapMax.AutoSize = true;
            this.radioButton_CombineOverlapMax.Location = new System.Drawing.Point(552, 514);
            this.radioButton_CombineOverlapMax.Name = "radioButton_CombineOverlapMax";
            this.radioButton_CombineOverlapMax.Size = new System.Drawing.Size(50, 18);
            this.radioButton_CombineOverlapMax.TabIndex = 366;
            this.radioButton_CombineOverlapMax.TabStop = true;
            this.radioButton_CombineOverlapMax.Text = "Max";
            this.radioButton_CombineOverlapMax.UseVisualStyleBackColor = true;
            // 
            // button_SetObjectImage
            // 
            this.button_SetObjectImage.Location = new System.Drawing.Point(245, 328);
            this.button_SetObjectImage.Name = "button_SetObjectImage";
            this.button_SetObjectImage.Size = new System.Drawing.Size(70, 25);
            this.button_SetObjectImage.TabIndex = 368;
            this.button_SetObjectImage.Text = "Set";
            this.button_SetObjectImage.UseVisualStyleBackColor = true;
            this.button_SetObjectImage.Click += new System.EventHandler(this.button_SetObjectImage_Click);
            // 
            // comboBox_SelectObjectImage
            // 
            this.comboBox_SelectObjectImage.FormattingEnabled = true;
            this.comboBox_SelectObjectImage.Location = new System.Drawing.Point(71, 329);
            this.comboBox_SelectObjectImage.Name = "comboBox_SelectObjectImage";
            this.comboBox_SelectObjectImage.Size = new System.Drawing.Size(170, 22);
            this.comboBox_SelectObjectImage.TabIndex = 367;
            // 
            // cogDisplay_CombineView
            // 
            this.cogDisplay_CombineView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cogDisplay_CombineView.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_CombineView.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_CombineView.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_CombineView.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_CombineView.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_CombineView.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_CombineView.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_CombineView.Location = new System.Drawing.Point(385, 90);
            this.cogDisplay_CombineView.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_CombineView.MouseWheelSensitivity = 1D;
            this.cogDisplay_CombineView.Name = "cogDisplay_CombineView";
            this.cogDisplay_CombineView.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_CombineView.OcxState")));
            this.cogDisplay_CombineView.Size = new System.Drawing.Size(480, 360);
            this.cogDisplay_CombineView.TabIndex = 369;
            // 
            // label_TextCombineOption
            // 
            this.label_TextCombineOption.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextCombineOption.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextCombineOption.ForeColor = System.Drawing.Color.White;
            this.label_TextCombineOption.Location = new System.Drawing.Point(385, 475);
            this.label_TextCombineOption.Name = "label_TextCombineOption";
            this.label_TextCombineOption.Size = new System.Drawing.Size(280, 25);
            this.label_TextCombineOption.TabIndex = 370;
            this.label_TextCombineOption.Text = "Option.";
            this.label_TextCombineOption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_TextSource
            // 
            this.label_TextSource.AutoSize = true;
            this.label_TextSource.Location = new System.Drawing.Point(10, 51);
            this.label_TextSource.Name = "label_TextSource";
            this.label_TextSource.Size = new System.Drawing.Size(59, 14);
            this.label_TextSource.TabIndex = 371;
            this.label_TextSource.Text = "Source :";
            // 
            // label_TextObject
            // 
            this.label_TextObject.AutoSize = true;
            this.label_TextObject.Location = new System.Drawing.Point(10, 333);
            this.label_TextObject.Name = "label_TextObject";
            this.label_TextObject.Size = new System.Drawing.Size(57, 14);
            this.label_TextObject.TabIndex = 372;
            this.label_TextObject.Text = "Object :";
            // 
            // label_TextAdvancedSetting
            // 
            this.label_TextAdvancedSetting.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextAdvancedSetting.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextAdvancedSetting.ForeColor = System.Drawing.Color.White;
            this.label_TextAdvancedSetting.Location = new System.Drawing.Point(900, 60);
            this.label_TextAdvancedSetting.Name = "label_TextAdvancedSetting";
            this.label_TextAdvancedSetting.Size = new System.Drawing.Size(320, 25);
            this.label_TextAdvancedSetting.TabIndex = 374;
            this.label_TextAdvancedSetting.Text = "Advanced Setting.";
            this.label_TextAdvancedSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // propertyGrid_ParamSetting
            // 
            this.propertyGrid_ParamSetting.Location = new System.Drawing.Point(900, 90);
            this.propertyGrid_ParamSetting.Name = "propertyGrid_ParamSetting";
            this.propertyGrid_ParamSetting.Size = new System.Drawing.Size(400, 500);
            this.propertyGrid_ParamSetting.TabIndex = 375;
            // 
            // button_UpdateSetting
            // 
            this.button_UpdateSetting.Location = new System.Drawing.Point(1220, 60);
            this.button_UpdateSetting.Name = "button_UpdateSetting";
            this.button_UpdateSetting.Size = new System.Drawing.Size(80, 25);
            this.button_UpdateSetting.TabIndex = 376;
            this.button_UpdateSetting.Text = "Update";
            this.button_UpdateSetting.UseVisualStyleBackColor = true;
            this.button_UpdateSetting.Click += new System.EventHandler(this.button_UpdateSetting_Click);
            // 
            // FormTab_Combine
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1350, 600);
            this.Controls.Add(this.button_UpdateSetting);
            this.Controls.Add(this.propertyGrid_ParamSetting);
            this.Controls.Add(this.label_TextAdvancedSetting);
            this.Controls.Add(this.label_TextObject);
            this.Controls.Add(this.label_TextSource);
            this.Controls.Add(this.label_TextCombineOption);
            this.Controls.Add(this.cogDisplay_CombineView);
            this.Controls.Add(this.button_SetObjectImage);
            this.Controls.Add(this.comboBox_SelectObjectImage);
            this.Controls.Add(this.radioButton_CombineOverlapMax);
            this.Controls.Add(this.radioButton_CombineSubtract);
            this.Controls.Add(this.radioButton_CombineOverlapMin);
            this.Controls.Add(this.radioButton_CombineAdd);
            this.Controls.Add(this.button_SetSourceImage);
            this.Controls.Add(this.comboBox_SelectSourceImage);
            this.Controls.Add(this.button_ProcessCombine);
            this.Controls.Add(this.cogDisplay_ObjectView);
            this.Controls.Add(this.cogDisplay_SourceView);
            this.Controls.Add(this.label_TextCombine);
            this.Controls.Add(this.checkBox_IsUseCombine);
            this.Controls.Add(this.label_IsUseCombine);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormTab_Combine";
            this.Text = "FormTab_Preprocessing";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTab_Combine_FormClosed);
            this.Load += new System.EventHandler(this.FormTab_Combine_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ObjectView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_SourceView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_CombineView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ProcessCombine;
        internal Cognex.VisionPro.Display.CogDisplay cogDisplay_ObjectView;
        internal Cognex.VisionPro.Display.CogDisplay cogDisplay_SourceView;
        private System.Windows.Forms.Label label_TextCombine;
        private System.Windows.Forms.CheckBox checkBox_IsUseCombine;
        private System.Windows.Forms.Label label_IsUseCombine;
        private System.Windows.Forms.Button button_SetSourceImage;
        private System.Windows.Forms.ComboBox comboBox_SelectSourceImage;
        private System.Windows.Forms.RadioButton radioButton_CombineAdd;
        private System.Windows.Forms.RadioButton radioButton_CombineOverlapMin;
        private System.Windows.Forms.RadioButton radioButton_CombineSubtract;
        private System.Windows.Forms.RadioButton radioButton_CombineOverlapMax;
        private System.Windows.Forms.Button button_SetObjectImage;
        private System.Windows.Forms.ComboBox comboBox_SelectObjectImage;
        internal Cognex.VisionPro.Display.CogDisplay cogDisplay_CombineView;
        private System.Windows.Forms.Label label_TextCombineOption;
        private System.Windows.Forms.Label label_TextSource;
        private System.Windows.Forms.Label label_TextObject;
        private System.Windows.Forms.Label label_TextAdvancedSetting;
        private System.Windows.Forms.PropertyGrid propertyGrid_ParamSetting;
        private System.Windows.Forms.Button button_UpdateSetting;
    }
}