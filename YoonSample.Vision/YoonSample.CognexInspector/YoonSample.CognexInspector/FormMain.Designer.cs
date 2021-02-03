
namespace YoonSample.CognexInspector
{
    partial class FormMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel_SimulationSheet = new System.Windows.Forms.Panel();
            this.dataGridView_Result = new System.Windows.Forms.DataGridView();
            this.button_ProcessInspection = new System.Windows.Forms.Button();
            this.label_Result = new System.Windows.Forms.Label();
            this.label_TextResult = new System.Windows.Forms.Label();
            this.label_TextProcessTest = new System.Windows.Forms.Label();
            this.panel_InspectManager = new System.Windows.Forms.Panel();
            this.button_EditInspection = new System.Windows.Forms.Button();
            this.dataGridView_SelectInspection = new System.Windows.Forms.DataGridView();
            this.label_InspectionManager = new System.Windows.Forms.Label();
            this.tabControl_Setting = new System.Windows.Forms.TabControl();
            this.textBox_InspectionMessage = new System.Windows.Forms.TextBox();
            this.panel_Title = new System.Windows.Forms.Panel();
            this.button_Load = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.bindingSource_Inspection = new System.Windows.Forms.BindingSource(this.components);
            this.panel_SimulationSheet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Result)).BeginInit();
            this.panel_InspectManager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_SelectInspection)).BeginInit();
            this.panel_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_Inspection)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_SimulationSheet
            // 
            this.panel_SimulationSheet.Controls.Add(this.dataGridView_Result);
            this.panel_SimulationSheet.Controls.Add(this.button_ProcessInspection);
            this.panel_SimulationSheet.Controls.Add(this.label_Result);
            this.panel_SimulationSheet.Controls.Add(this.label_TextResult);
            this.panel_SimulationSheet.Controls.Add(this.label_TextProcessTest);
            this.panel_SimulationSheet.Location = new System.Drawing.Point(0, 325);
            this.panel_SimulationSheet.Name = "panel_SimulationSheet";
            this.panel_SimulationSheet.Size = new System.Drawing.Size(300, 495);
            this.panel_SimulationSheet.TabIndex = 304;
            // 
            // dataGridView_Result
            // 
            this.dataGridView_Result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Result.Location = new System.Drawing.Point(7, 65);
            this.dataGridView_Result.Name = "dataGridView_Result";
            this.dataGridView_Result.RowTemplate.Height = 23;
            this.dataGridView_Result.Size = new System.Drawing.Size(285, 427);
            this.dataGridView_Result.TabIndex = 294;
            // 
            // button_ProcessInspection
            // 
            this.button_ProcessInspection.Location = new System.Drawing.Point(210, 30);
            this.button_ProcessInspection.Name = "button_ProcessInspection";
            this.button_ProcessInspection.Size = new System.Drawing.Size(80, 23);
            this.button_ProcessInspection.TabIndex = 293;
            this.button_ProcessInspection.Text = "Process";
            this.button_ProcessInspection.UseVisualStyleBackColor = true;
            // 
            // label_Result
            // 
            this.label_Result.AutoSize = true;
            this.label_Result.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Result.ForeColor = System.Drawing.Color.Lime;
            this.label_Result.Location = new System.Drawing.Point(69, 30);
            this.label_Result.Name = "label_Result";
            this.label_Result.Size = new System.Drawing.Size(46, 25);
            this.label_Result.TabIndex = 292;
            this.label_Result.Text = "OK";
            // 
            // label_TextResult
            // 
            this.label_TextResult.AutoSize = true;
            this.label_TextResult.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextResult.Location = new System.Drawing.Point(4, 35);
            this.label_TextResult.Name = "label_TextResult";
            this.label_TextResult.Size = new System.Drawing.Size(62, 16);
            this.label_TextResult.TabIndex = 291;
            this.label_TextResult.Text = "Result :";
            // 
            // label_TextProcessTest
            // 
            this.label_TextProcessTest.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_TextProcessTest.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TextProcessTest.ForeColor = System.Drawing.Color.White;
            this.label_TextProcessTest.Location = new System.Drawing.Point(0, 0);
            this.label_TextProcessTest.Name = "label_TextProcessTest";
            this.label_TextProcessTest.Size = new System.Drawing.Size(200, 25);
            this.label_TextProcessTest.TabIndex = 290;
            this.label_TextProcessTest.Text = "Process Test.";
            this.label_TextProcessTest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel_InspectManager
            // 
            this.panel_InspectManager.Controls.Add(this.button_EditInspection);
            this.panel_InspectManager.Controls.Add(this.dataGridView_SelectInspection);
            this.panel_InspectManager.Controls.Add(this.label_InspectionManager);
            this.panel_InspectManager.Location = new System.Drawing.Point(0, 40);
            this.panel_InspectManager.Name = "panel_InspectManager";
            this.panel_InspectManager.Size = new System.Drawing.Size(300, 280);
            this.panel_InspectManager.TabIndex = 303;
            // 
            // button_EditInspection
            // 
            this.button_EditInspection.Location = new System.Drawing.Point(225, 2);
            this.button_EditInspection.Name = "button_EditInspection";
            this.button_EditInspection.Size = new System.Drawing.Size(70, 25);
            this.button_EditInspection.TabIndex = 295;
            this.button_EditInspection.Text = "Edit";
            this.button_EditInspection.UseVisualStyleBackColor = true;
            // 
            // dataGridView_SelectInspection
            // 
            this.dataGridView_SelectInspection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_SelectInspection.Location = new System.Drawing.Point(0, 30);
            this.dataGridView_SelectInspection.Name = "dataGridView_SelectInspection";
            this.dataGridView_SelectInspection.RowHeadersWidth = 62;
            this.dataGridView_SelectInspection.RowTemplate.Height = 23;
            this.dataGridView_SelectInspection.Size = new System.Drawing.Size(300, 250);
            this.dataGridView_SelectInspection.TabIndex = 291;
            // 
            // label_InspectionManager
            // 
            this.label_InspectionManager.BackColor = System.Drawing.Color.RoyalBlue;
            this.label_InspectionManager.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_InspectionManager.ForeColor = System.Drawing.Color.White;
            this.label_InspectionManager.Location = new System.Drawing.Point(0, 0);
            this.label_InspectionManager.Name = "label_InspectionManager";
            this.label_InspectionManager.Size = new System.Drawing.Size(200, 25);
            this.label_InspectionManager.TabIndex = 290;
            this.label_InspectionManager.Text = "Inspection Manager.";
            this.label_InspectionManager.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl_Setting
            // 
            this.tabControl_Setting.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabControl_Setting.Location = new System.Drawing.Point(300, 40);
            this.tabControl_Setting.Name = "tabControl_Setting";
            this.tabControl_Setting.SelectedIndex = 0;
            this.tabControl_Setting.Size = new System.Drawing.Size(1360, 630);
            this.tabControl_Setting.TabIndex = 302;
            // 
            // textBox_InspectionMessage
            // 
            this.textBox_InspectionMessage.BackColor = System.Drawing.Color.Beige;
            this.textBox_InspectionMessage.Location = new System.Drawing.Point(300, 670);
            this.textBox_InspectionMessage.Multiline = true;
            this.textBox_InspectionMessage.Name = "textBox_InspectionMessage";
            this.textBox_InspectionMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_InspectionMessage.Size = new System.Drawing.Size(1360, 150);
            this.textBox_InspectionMessage.TabIndex = 301;
            // 
            // panel_Title
            // 
            this.panel_Title.Controls.Add(this.button_Save);
            this.panel_Title.Controls.Add(this.button_Load);
            this.panel_Title.Location = new System.Drawing.Point(1, 1);
            this.panel_Title.Name = "panel_Title";
            this.panel_Title.Size = new System.Drawing.Size(1660, 40);
            this.panel_Title.TabIndex = 305;
            // 
            // button_Load
            // 
            this.button_Load.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Load.Location = new System.Drawing.Point(1351, 5);
            this.button_Load.Name = "button_Load";
            this.button_Load.Size = new System.Drawing.Size(150, 30);
            this.button_Load.TabIndex = 0;
            this.button_Load.Text = "Load";
            this.button_Load.UseVisualStyleBackColor = true;
            // 
            // button_Save
            // 
            this.button_Save.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Save.Location = new System.Drawing.Point(1507, 5);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(150, 30);
            this.button_Save.TabIndex = 1;
            this.button_Save.Text = "Save";
            this.button_Save.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1664, 821);
            this.Controls.Add(this.panel_Title);
            this.Controls.Add(this.panel_SimulationSheet);
            this.Controls.Add(this.panel_InspectManager);
            this.Controls.Add(this.tabControl_Setting);
            this.Controls.Add(this.textBox_InspectionMessage);
            this.Name = "FormMain";
            this.Text = "Main Form";
            this.panel_SimulationSheet.ResumeLayout(false);
            this.panel_SimulationSheet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Result)).EndInit();
            this.panel_InspectManager.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_SelectInspection)).EndInit();
            this.panel_Title.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_Inspection)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_SimulationSheet;
        private System.Windows.Forms.DataGridView dataGridView_Result;
        private System.Windows.Forms.Button button_ProcessInspection;
        private System.Windows.Forms.Label label_Result;
        private System.Windows.Forms.Label label_TextResult;
        private System.Windows.Forms.Label label_TextProcessTest;
        private System.Windows.Forms.Panel panel_InspectManager;
        private System.Windows.Forms.Button button_EditInspection;
        private System.Windows.Forms.DataGridView dataGridView_SelectInspection;
        private System.Windows.Forms.Label label_InspectionManager;
        private System.Windows.Forms.TabControl tabControl_Setting;
        private System.Windows.Forms.TextBox textBox_InspectionMessage;
        private System.Windows.Forms.Panel panel_Title;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_Load;
        private System.Windows.Forms.BindingSource bindingSource_Inspection;
    }
}

