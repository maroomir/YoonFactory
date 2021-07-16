namespace YoonSample.CognexInspector
{
    partial class Form_CogLineMax
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
            this.cogLineMaxEditV2 = new Cognex.VisionPro.LineMax.CogLineMaxEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogLineMaxEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // cogLineMaxEditV2
            // 
            this.cogLineMaxEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogLineMaxEditV2.Location = new System.Drawing.Point(0, 0);
            this.cogLineMaxEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogLineMaxEditV2.Name = "cogLineMaxEditV2";
            this.cogLineMaxEditV2.Size = new System.Drawing.Size(800, 450);
            this.cogLineMaxEditV2.SuspendElectricRuns = false;
            this.cogLineMaxEditV2.TabIndex = 0;
            // 
            // Form_CogLineMax
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogLineMaxEditV2);
            this.Name = "Form_CogLineMax";
            this.Text = "Form_CogLineMax";
            ((System.ComponentModel.ISupportInitialize)(this.cogLineMaxEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.LineMax.CogLineMaxEditV2 cogLineMaxEditV2;
    }
}