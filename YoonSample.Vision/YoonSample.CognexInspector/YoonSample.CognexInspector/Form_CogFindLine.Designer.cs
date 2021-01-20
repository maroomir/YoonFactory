namespace RobotIntegratedVision
{
    partial class Form_CogFindLine
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
            this.cogFindLineEditV2 = new Cognex.VisionPro.Caliper.CogFindLineEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogFindLineEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogFindLineEditV2
            // 
            this.cogFindLineEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogFindLineEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogFindLineEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogFindLineEditV2.Name = "cogFindLineEditV2";
            this.cogFindLineEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogFindLineEditV2.SuspendElectricRuns = false;
            this.cogFindLineEditV2.TabIndex = 0;
            // 
            // Form_CogFindLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogFindLineEditV2);
            this.Name = "Form_CogFindLine";
            this.Text = "Form_CogFindLine";
            ((System.ComponentModel.ISupportInitialize)(this.cogFindLineEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.Caliper.CogFindLineEditV2 cogFindLineEditV2;
    }
}