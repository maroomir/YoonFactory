using Cognex.VisionPro;
using Cognex.VisionPro.PMAlign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using YoonFactory.Cognex;
using System.IO;

namespace YoonSample.CognexInspector
{
    public partial class Form_CogPatternAlign : Form
    {
        public eYoonCognexType ToolType = eYoonCognexType.PMAlign;
        public eLabelInspect CogToolLabel;
        public CogPMAlignTool CogTool;
        public ICogImage CogImageSource;
        public event PassCogToolCallback OnUpdateCogToolEvent;

        public Form_CogPatternAlign()
        {
            InitializeComponent();
        }

        private void Form_CogPatternAlign_Load(object sender, EventArgs e)
        {
            if(CogImageSource == null || CogTool == null)
            {
                Close();
                return;
            }

            cogPMAlignEditV2.Subject = CogTool;
            cogPMAlignEditV2.Subject.InputImage = CogImageSource;
        }

        private void Form_CogPatternAlign_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CogImageSource == null || CogTool == null)
                return;

            DialogResult result = MessageBox.Show("Save This Cognex Tool?", "", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                ////  Working 영역에 Vpp File 별도 저장 (백업 및 확인용)
                string strFilePath = Path.Combine(CommonClass.strCurrentWorkingDirectory, string.Format("{0}.vpp", ToolType.ToString()));
                if (ToolFactory.SaveCognexToolToVpp(CogTool, strFilePath))
                    CogTool = ToolFactory.LoadCognexToolFromVpp(strFilePath) as CogPMAlignTool;
                if (CogTool == null) return;
                ////  Main Form에 Cognex Tool 전달
                OnUpdateCogToolEvent(this, new CogToolArgs(ToolType, CogToolLabel, CogTool, CogTool.InputImage));
                Thread.Sleep(100);
            }
        }
    }
}
