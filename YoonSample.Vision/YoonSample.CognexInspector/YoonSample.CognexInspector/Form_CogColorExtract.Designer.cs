namespace RobotIntegratedVision
{
    partial class Form_CogColorExtract
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
            this.cogColorExtractorEditV2 = new Cognex.VisionPro.ColorExtractor.CogColorExtractorEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogColorExtractorEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogColorExtractorEditV2
            // 
            this.cogColorExtractorEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogColorExtractorEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogColorExtractorEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogColorExtractorEditV2.Name = "cogColorExtractorEditV2";
            this.cogColorExtractorEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogColorExtractorEditV2.SuspendElectricRuns = false;
            this.cogColorExtractorEditV2.TabIndex = 0;
            // 
            // Form_CogColorExtract
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogColorExtractorEditV2);
            this.Name = "Form_CogColorExtract";
            this.Text = "Form_CogColorExtract";
            ((System.ComponentModel.ISupportInitialize)(this.cogColorExtractorEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.ColorExtractor.CogColorExtractorEditV2 cogColorExtractorEditV2;
    }
}