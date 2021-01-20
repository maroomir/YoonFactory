using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using YoonFactory.Cognex;
using System.IO;

namespace YoonSample.CognexInspector
{
    public partial class Form_CogCalibCheckerboard : Form
    {
        public eYoonCognexType ToolType = eYoonCognexType.Calibration;
        public eLabelInspect CogToolLabel;
        public CogCalibCheckerboardTool CogTool;
        public CogImage8Grey CogImageSource;
        public event PassCogToolCallback OnUpdateCogToolEvent;

        public Form_CogCalibCheckerboard()
        {
            InitializeComponent();
        }

        private void Form_CogCalibCheckerboard_Load(object sender, EventArgs e)
        {
            if(CogImageSource == null || CogTool == null)
            {
                Close();
                return;
            }

            cogCalibCheckerboardEditV2.Subject = CogTool;
            cogCalibCheckerboardEditV2.Subject.InputImage = CogImageSource;
        }

        private void Form_CogCalibCheckerboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CogImageSource == null || CogTool == null)
                return;

            DialogResult result = MessageBox.Show("Save This Cognex Tool?", "", MessageBoxButtons.YesNo);

            if(result == DialogResult.Yes)
            {
                ////  Working 영역에 Vpp File 별도 저장 (백업 및 확인용)
                string strFilePath = Path.Combine(CommonClass.strCurrentWorkingDirectory, string.Format("{0}.vpp", ToolType.ToString()));
                if (CogToolFactory.SaveCognexToolToVpp(CogTool, strFilePath))
                    CogTool = CogToolFactory.LoadCognexToolFromVpp(strFilePath) as CogCalibCheckerboardTool;
                if (CogTool == null) return;
                ////  Main Form에 Cognex Tool 전달
                OnUpdateCogToolEvent(this, new CogToolArgs(ToolType, CogToolLabel, CogTool, CogTool.OutputImage));
                Thread.Sleep(100);
            }
        }
    }
}
