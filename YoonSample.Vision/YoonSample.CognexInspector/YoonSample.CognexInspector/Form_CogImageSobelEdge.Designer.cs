namespace RobotIntegratedVision
{
    partial class Form_CogImageSobelEdge
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
            this.cogSobelEdgeEditV2 = new Cognex.VisionPro.ImageProcessing.CogSobelEdgeEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogSobelEdgeEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogSobelEdgeEditV2
            // 
            this.cogSobelEdgeEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogSobelEdgeEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogSobelEdgeEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogSobelEdgeEditV2.Name = "cogSobelEdgeEditV2";
            this.cogSobelEdgeEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogSobelEdgeEditV2.SuspendElectricRuns = false;
            this.cogSobelEdgeEditV2.TabIndex = 0;
            // 
            // Form_CogImageSobelEdge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogSobelEdgeEditV2);
            this.Name = "Form_CogImageSobelEdge";
            this.Text = "Form_CogImageSobel";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_CogImageSobelEdge_FormClosing);
            this.Load += new System.EventHandler(this.Form_CogImageSobelEdge_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogSobelEdgeEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.ImageProcessing.CogSobelEdgeEditV2 cogSobelEdgeEditV2;
    }
}