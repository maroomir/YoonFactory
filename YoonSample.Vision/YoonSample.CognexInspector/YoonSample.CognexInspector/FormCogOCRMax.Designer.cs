namespace YoonSample.CognexInspector
{
    partial class Form_CogOCRMax
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
            this.cogOCRMaxEditV2 = new Cognex.VisionPro.OCRMax.CogOCRMaxEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogOCRMaxEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogOCRMaxEditV2
            // 
            this.cogOCRMaxEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogOCRMaxEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogOCRMaxEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogOCRMaxEditV2.Name = "cogOCRMaxEditV2";
            this.cogOCRMaxEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogOCRMaxEditV2.SuspendElectricRuns = false;
            this.cogOCRMaxEditV2.TabIndex = 0;
            // 
            // Form_CogOCRMax
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogOCRMaxEditV2);
            this.Name = "Form_CogOCRMax";
            this.Text = "Form_CogOCRMax";
            ((System.ComponentModel.ISupportInitialize)(this.cogOCRMaxEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.OCRMax.CogOCRMaxEditV2 cogOCRMaxEditV2;
    }
}