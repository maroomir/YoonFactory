using Cognex.VisionPro;
using Cognex.VisionPro.ColorSegmenter;
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
    public partial class Form_CogColorSegment : Form
    {
        public eYoonCognexType ToolType = eYoonCognexType.ColorSegment;
        public eLabelInspect CogToolLabel;
        public CogColorSegmenterTool CogTool;
        public CogImage24PlanarColor CogImageSource;
        public event PassCogToolCallback OnUpdateCogToolEvent;

        public Form_CogColorSegment()
        {
            InitializeComponent();
        }

        private void Form_CogColorSegment_Load(object sender, EventArgs e)
        {
            if (CogImageSource == null || CogTool == null)
            {
                Close();
                return;
            }

            cogColorSegmenterEditV2.Subject = CogTool;
            cogColorSegmenterEditV2.Subject.InputImage = CogImageSource;
        }

        private void Form_CogColorSegment_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CogImageSource == null || CogTool == null)
                return;

            DialogResult result = MessageBox.Show("Save This Cognex Tool?", "", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                ////  Working 영역에 Vpp File 별도 저장 (백업 및 확인용)
                string strFilePath = Path.Combine(CommonClass.strCurrentWorkingDirectory, string.Format("{0}.vpp", ToolType.ToString()));
                if (CogToolFactory.SaveCognexToolToVpp(CogTool, strFilePath))
                    CogTool = CogToolFactory.LoadCognexToolFromVpp(strFilePath) as CogColorSegmenterTool;
                if (CogTool == null) return;
                ////  Main Form에 Cognex Tool 전달
                OnUpdateCogToolEvent(this, new CogToolArgs(ToolType, CogToolLabel, CogTool, CogTool.Result));
                Thread.Sleep(100);
            }
        }
    }
}
