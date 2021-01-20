namespace RobotIntegratedVision
{
    partial class Form_CogSearchMax
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
            this.cogSearchMaxEditV2 = new Cognex.VisionPro.SearchMax.CogSearchMaxEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogSearchMaxEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogSearchMaxEditV2
            // 
            this.cogSearchMaxEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogSearchMaxEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogSearchMaxEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogSearchMaxEditV2.Name = "cogSearchMaxEditV2";
            this.cogSearchMaxEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogSearchMaxEditV2.SuspendElectricRuns = false;
            this.cogSearchMaxEditV2.TabIndex = 0;
            // 
            // Form_CogSearchMax
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogSearchMaxEditV2);
            this.Name = "Form_CogSearchMax";
            this.Text = "Form_CogSearchMax";
            ((System.ComponentModel.ISupportInitialize)(this.cogSearchMaxEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.SearchMax.CogSearchMaxEditV2 cogSearchMaxEditV2;
    }
}