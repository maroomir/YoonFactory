namespace RobotIntegratedVision
{
    partial class Form_CogImageConvert
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
            this.cogImageConvertEditV2 = new Cognex.VisionPro.ImageProcessing.CogImageConvertEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogImageConvertEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogImageConvertEditV2
            // 
            this.cogImageConvertEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogImageConvertEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogImageConvertEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogImageConvertEditV2.Name = "cogImageConvertEditV2";
            this.cogImageConvertEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogImageConvertEditV2.SuspendElectricRuns = false;
            this.cogImageConvertEditV2.TabIndex = 0;
            // 
            // Form_CogImageConvert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogImageConvertEditV2);
            this.Name = "Form_CogImageConvert";
            this.Text = "Form_CogImageConvert";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_CogImageConvert_FormClosing);
            this.Load += new System.EventHandler(this.Form_CogImageConvert_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogImageConvertEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.ImageProcessing.CogImageConvertEditV2 cogImageConvertEditV2;
    }
}