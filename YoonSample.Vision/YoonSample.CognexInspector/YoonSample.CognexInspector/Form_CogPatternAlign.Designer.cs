namespace RobotIntegratedVision
{
    partial class Form_CogPatternAlign
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
            this.cogPMAlignEditV2 = new Cognex.VisionPro.PMAlign.CogPMAlignEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogPMAlignEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogPMAlignEditV2
            // 
            this.cogPMAlignEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogPMAlignEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogPMAlignEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogPMAlignEditV2.Name = "cogPMAlignEditV2";
            this.cogPMAlignEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogPMAlignEditV2.SuspendElectricRuns = false;
            this.cogPMAlignEditV2.TabIndex = 0;
            // 
            // Form_CogPatternAlign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogPMAlignEditV2);
            this.Name = "Form_CogPatternAlign";
            this.Text = "Form_CogPatternAlign";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_CogPatternAlign_FormClosing);
            this.Load += new System.EventHandler(this.Form_CogPatternAlign_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogPMAlignEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.PMAlign.CogPMAlignEditV2 cogPMAlignEditV2;
    }
}