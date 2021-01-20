using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoonFactory.Cognex;

namespace RobotIntegratedVision
{
    public partial class Form_CogImageConvert : Form
    {
        public eYoonCognexType ToolType = eYoonCognexType.Convert;
        public eLabelInspect CogToolLabel;
        public CogImageConvertTool CogTool;
        public ICogImage CogImageSource;
        public event PassCogToolCallback OnUpdateCogToolEvent;

        public Form_CogImageConvert()
        {
            InitializeComponent();
        }

        private void Form_CogImageConvert_Load(object sender, EventArgs e)
        {
            if (CogImageSource == null || CogTool == null)
            {
                Close();
                return;
            }

            cogImageConvertEditV2.Subject = CogTool;
            cogImageConvertEditV2.Subject.InputImage = CogImageSource;
        }

        private void Form_CogImageConvert_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CogImageSource == null || CogTool == null)
                return;

            DialogResult result = MessageBox.Show("Save This Cognex Tool?", "", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                ////  Working 영역에 Vpp File 별도 저장 (백업 및 확인용)
                string strFilePath = Path.Combine(CommonClass.strCurrentWorkingDirectory, string.Format("{0}.vpp", ToolType.ToString()));
                if (CogToolFactory.SaveCognexToolToVpp(CogTool, strFilePath))
                    CogTool = CogToolFactory.LoadCognexToolFromVpp(strFilePath) as CogImageConvertTool;
                if (CogTool == null) return;
                ////  Main Form에 Cognex Tool 전달
                OnUpdateCogToolEvent(this, new CogToolArgs(ToolType, CogToolLabel, CogTool, CogTool.OutputImage));
                Thread.Sleep(100);
            }
        }
    }
}
