namespace YoonSample.CognexInspector
{
    partial class Form_CogOCVMax
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
            this.cogOCVMaxEdit = new Cognex.VisionPro.OCVMax.CogOCVMaxEdit();
            this.SuspendLayout();
            // 
            // cogOCVMaxEdit
            // 
            this.cogOCVMaxEdit.Cursor = System.Windows.Forms.Cursors.Default;
            this.cogOCVMaxEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogOCVMaxEdit.Location = new System.Drawing.Point(0, 0);
            this.cogOCVMaxEdit.MinimumSize = new System.Drawing.Size(490, 376);
            this.cogOCVMaxEdit.Name = "cogOCVMaxEdit";
            this.cogOCVMaxEdit.Size = new System.Drawing.Size(800, 450);
            this.cogOCVMaxEdit.TabIndex = 0;
            this.cogOCVMaxEdit.ToolSyncObject = null;
            // 
            // Form_CogOCVMax
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cogOCVMaxEdit);
            this.Name = "Form_CogOCVMax";
            this.Text = "Form_CogOCVMax";
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.OCVMax.CogOCVMaxEdit cogOCVMaxEdit;
    }
}