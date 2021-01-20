namespace RobotIntegratedVision
{
    partial class Form_CogCalibCheckerboard
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
            this.cogCalibCheckerboardEditV2 = new Cognex.VisionPro.CalibFix.CogCalibCheckerboardEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogCalibCheckerboardEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogCalibCheckerboardEditV2
            // 
            this.cogCalibCheckerboardEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogCalibCheckerboardEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogCalibCheckerboardEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogCalibCheckerboardEditV2.Name = "cogCalibCheckerboardEditV2";
            this.cogCalibCheckerboardEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogCalibCheckerboardEditV2.SuspendElectricRuns = false;
            this.cogCalibCheckerboardEditV2.TabIndex = 0;
            // 
            // Form_CogCalibCheckerboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogCalibCheckerboardEditV2);
            this.Name = "Form_CogCalibCheckerboard";
            this.Text = "Form_CogCalibCheckerboardTool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_CogCalibCheckerboard_FormClosing);
            this.Load += new System.EventHandler(this.Form_CogCalibCheckerboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogCalibCheckerboardEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.CalibFix.CogCalibCheckerboardEditV2 cogCalibCheckerboardEditV2;
    }
}