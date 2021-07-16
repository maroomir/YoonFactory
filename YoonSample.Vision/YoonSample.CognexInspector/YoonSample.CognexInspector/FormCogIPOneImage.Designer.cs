namespace YoonSample.CognexInspector
{
    partial class Form_CogIPOneImage
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
            this.cogIPOneImageEditV2 = new Cognex.VisionPro.ImageProcessing.CogIPOneImageEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogIPOneImageEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogIPOneImageEditV2
            // 
            this.cogIPOneImageEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogIPOneImageEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogIPOneImageEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogIPOneImageEditV2.Name = "cogIPOneImageEditV2";
            this.cogIPOneImageEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogIPOneImageEditV2.SuspendElectricRuns = false;
            this.cogIPOneImageEditV2.TabIndex = 0;
            // 
            // Form_CogIPOneImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogIPOneImageEditV2);
            this.Name = "Form_CogIPOneImage";
            this.Text = "Form_CogIPOneImage";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_CogIPOneImage_FormClosing);
            this.Load += new System.EventHandler(this.Form_CogIPOneImage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogIPOneImageEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.ImageProcessing.CogIPOneImageEditV2 cogIPOneImageEditV2;
    }
}