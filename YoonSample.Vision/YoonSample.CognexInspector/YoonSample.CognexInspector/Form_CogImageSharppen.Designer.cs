namespace RobotIntegratedVision
{
    partial class Form_CogImageSharppen
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
            this.cogImageSharpnessEditV2 = new Cognex.VisionPro.ImageProcessing.CogImageSharpnessEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogImageSharpnessEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogImageSharpnessEditV2
            // 
            this.cogImageSharpnessEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogImageSharpnessEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogImageSharpnessEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogImageSharpnessEditV2.Name = "cogImageSharpnessEditV2";
            this.cogImageSharpnessEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogImageSharpnessEditV2.SuspendElectricRuns = false;
            this.cogImageSharpnessEditV2.TabIndex = 0;
            // 
            // Form_CogImageProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogImageSharpnessEditV2);
            this.Name = "Form_CogImageProcessing";
            this.Text = "Form_CogImageProcessing";
            ((System.ComponentModel.ISupportInitialize)(this.cogImageSharpnessEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.ImageProcessing.CogImageSharpnessEditV2 cogImageSharpnessEditV2;
    }
}