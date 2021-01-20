namespace RobotIntegratedVision
{
    partial class Form_CogIPTwoImageAdd
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
            this.cogIPTwoImageAddEditV2 = new Cognex.VisionPro.ImageProcessing.CogIPTwoImageAddEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogIPTwoImageAddEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogIPTwoImageAddEditV2
            // 
            this.cogIPTwoImageAddEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogIPTwoImageAddEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogIPTwoImageAddEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogIPTwoImageAddEditV2.Name = "cogIPTwoImageAddEditV2";
            this.cogIPTwoImageAddEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogIPTwoImageAddEditV2.SuspendElectricRuns = false;
            this.cogIPTwoImageAddEditV2.TabIndex = 0;
            // 
            // Form_CogIPTwoImageAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogIPTwoImageAddEditV2);
            this.Name = "Form_CogIPTwoImageAdd";
            this.Text = "Form_CogIPTwoImageAdd";
            ((System.ComponentModel.ISupportInitialize)(this.cogIPTwoImageAddEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.ImageProcessing.CogIPTwoImageAddEditV2 cogIPTwoImageAddEditV2;
    }
}