namespace YoonSample.CognexInspector
{
    partial class Form_CogIPTwoImageSubstract
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
            this.cogIPTwoImageSubtractEditV2 = new Cognex.VisionPro.ImageProcessing.CogIPTwoImageSubtractEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogIPTwoImageSubtractEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogIPTwoImageSubtractEditV2
            // 
            this.cogIPTwoImageSubtractEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogIPTwoImageSubtractEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogIPTwoImageSubtractEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogIPTwoImageSubtractEditV2.Name = "cogIPTwoImageSubtractEditV2";
            this.cogIPTwoImageSubtractEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogIPTwoImageSubtractEditV2.SuspendElectricRuns = false;
            this.cogIPTwoImageSubtractEditV2.TabIndex = 0;
            // 
            // Form_CogIPTwoImageSubstract
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogIPTwoImageSubtractEditV2);
            this.Name = "Form_CogIPTwoImageSubstract";
            this.Text = "Form_CogIPTwoImageSubstract";
            ((System.ComponentModel.ISupportInitialize)(this.cogIPTwoImageSubtractEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.ImageProcessing.CogIPTwoImageSubtractEditV2 cogIPTwoImageSubtractEditV2;
    }
}