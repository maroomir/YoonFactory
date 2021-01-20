namespace RobotIntegratedVision
{
    partial class Form_CogBlob
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
            this.cogBlobEditV2 = new Cognex.VisionPro.Blob.CogBlobEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogBlobEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogBlobEditV2
            // 
            this.cogBlobEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogBlobEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogBlobEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogBlobEditV2.Name = "cogBlobEditV2";
            this.cogBlobEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogBlobEditV2.SuspendElectricRuns = false;
            this.cogBlobEditV2.TabIndex = 0;
            // 
            // Form_CogBlob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogBlobEditV2);
            this.Name = "Form_CogBlob";
            this.Text = "Form_CogBlob";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_CogBlob_FormClosing);
            this.Load += new System.EventHandler(this.Form_CogBlob_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogBlobEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.Blob.CogBlobEditV2 cogBlobEditV2;
    }
}