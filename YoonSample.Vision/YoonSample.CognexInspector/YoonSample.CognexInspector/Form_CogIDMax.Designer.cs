namespace RobotIntegratedVision
{
    partial class Form_CogIDMax
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
            this.cogIDEditV2 = new Cognex.VisionPro.ID.CogIDEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogIDEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogIDEditV2
            // 
            this.cogIDEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogIDEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogIDEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogIDEditV2.Name = "cogIDEditV2";
            this.cogIDEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogIDEditV2.SuspendElectricRuns = false;
            this.cogIDEditV2.TabIndex = 0;
            // 
            // Form_CogIDMax
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogIDEditV2);
            this.Name = "Form_CogIDMax";
            this.Text = "Form_CogIDMax";
            ((System.ComponentModel.ISupportInitialize)(this.cogIDEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.ID.CogIDEditV2 cogIDEditV2;
    }
}