namespace YoonSample.CognexInspector
{
    partial class Form_CogColorSegment
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
            this.cogColorSegmenterEditV2 = new Cognex.VisionPro.ColorSegmenter.CogColorSegmenterEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogColorSegmenterEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogColorSegmenterEditV2
            // 
            this.cogColorSegmenterEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogColorSegmenterEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogColorSegmenterEditV2.MinimumSize = new System.Drawing.Size(500, 370);
            this.cogColorSegmenterEditV2.Name = "cogColorSegmenterEditV2";
            this.cogColorSegmenterEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogColorSegmenterEditV2.Subject = null;
            this.cogColorSegmenterEditV2.SuspendElectricRuns = false;
            this.cogColorSegmenterEditV2.TabIndex = 0;
            // 
            // Form_CogColorSegment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogColorSegmenterEditV2);
            this.Name = "Form_CogColorSegment";
            this.Text = "Form_CogColorSegment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_CogColorSegment_FormClosing);
            this.Load += new System.EventHandler(this.Form_CogColorSegment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogColorSegmenterEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.ColorSegmenter.CogColorSegmenterEditV2 cogColorSegmenterEditV2;
    }
}