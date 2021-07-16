namespace YoonSample.CognexInspector
{
    partial class Form_CogIPTwoImageMinMax
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
            this.cogIPTwoImageMinMaxEditV2 = new Cognex.VisionPro.ImageProcessing.CogIPTwoImageMinMaxEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogIPTwoImageMinMaxEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogIPTwoImageMinMaxEditV2
            // 
            this.cogIPTwoImageMinMaxEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogIPTwoImageMinMaxEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogIPTwoImageMinMaxEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogIPTwoImageMinMaxEditV2.Name = "cogIPTwoImageMinMaxEditV2";
            this.cogIPTwoImageMinMaxEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogIPTwoImageMinMaxEditV2.SuspendElectricRuns = false;
            this.cogIPTwoImageMinMaxEditV2.TabIndex = 0;
            // 
            // Form_CogIPTwoImageMinMax
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogIPTwoImageMinMaxEditV2);
            this.Name = "Form_CogIPTwoImageMinMax";
            this.Text = "Form_CogIPTwoImageMinMax";
            ((System.ComponentModel.ISupportInitialize)(this.cogIPTwoImageMinMaxEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.ImageProcessing.CogIPTwoImageMinMaxEditV2 cogIPTwoImageMinMaxEditV2;
    }
}