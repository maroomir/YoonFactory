using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using System;
using System.Windows.Forms;
using YoonFactory;
using YoonFactory.Cognex;

namespace YoonSample.CognexInspector
{
    public partial class TabPreprocessing : Form, IInspectionTab
    {
        private int m_nIndex = -1;
        private eTypeInspect m_nType = eTypeInspect.None;
        private CognexImage m_pCogImageSource = null;
        private CognexImage m_pCogImageProcessing = null;
        private CognexImage m_pCogImageResult = null;

        public event PassImageCallback OnUpdateResultImageEvent;

        public TabPreprocessing(int nIndex)
        {
            InitializeComponent();

            m_nIndex = nIndex;
            m_nType = eTypeInspect.Preprocessing;
        }

        private void FormTab_Preprocessing_Load(object sender, EventArgs e)
        {
            cogDisplay_PreviousView.AutoFit = true;
            cogDisplay_ProcessView.AutoFit = true;

            //// 유효성 체크
            if (!CommonClass.pParamTemplate.ContainsKey(m_nType)
                && !CommonClass.pCogToolTemplate.ContainsKey(m_nType))
            {
                CommonClass.pCLM.Write("Preprocessing Load Failure : Inspection Info isnot valid");
                Close();
                return;
            }

            //// Form 초기화
            ParameterInspectionPreprocessing pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionPreprocessing;
            checkBox_IsUsePreprocessing.Checked = pParam.IsUse;
            checkBox_IsUseImageConvert.Checked = pParam.IsUseImageConvert;
            checkBox_IsUseSobel.Checked = pParam.IsUseSobelEdge;
            checkBox_IsUseFiltering.Checked = pParam.IsUseImageFilter;

            //// Grid 초기화
            propertyGrid_ParamSetting.SelectedObject = pParam;

            //// 이벤트 초기화
            checkBox_IsUsePreprocessing.Click += OnCheckBoxEnableClick;
            checkBox_IsUseImageConvert.Click += OnCheckBoxEnableClick;
            checkBox_IsUseSobel.Click += OnCheckBoxEnableClick;
            checkBox_IsUseFiltering.Click += OnCheckBoxEnableClick;
        }

        private void FormTab_Preprocessing_FormClosed(object sender, FormClosedEventArgs e)
        {
            ////  Variable Clear
        }

        public void OnCheckBoxEnableClick(object sender, EventArgs e)
        {
            CheckBox pBox = (CheckBox)sender;
            ParameterInspectionPreprocessing pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionPreprocessing;
            switch (pBox.Name)
            {
                case "checkBox_IsUsePreprocessing":
                    pParam.IsUse = pBox.Checked;
                    break;
                case "checkBox_IsUseImageConvert":
                    pParam.IsUseImageConvert = pBox.Checked;
                    break;
                case "checkBox_IsUseSobel":
                    pParam.IsUseSobelEdge = pBox.Checked;
                    break;
                case "checkBox_IsUseFiltering":
                    pParam.IsUseImageFilter = pBox.Checked;
                    break;
                default:
                    break;
            }

            //// 변경 즉시 반영 (적용지연 에러 발생에 대한 대처사항)
            CommonClass.pParamTemplate[m_nType].SetParameter(pParam, typeof(ParameterInspectionPreprocessing));
            OnInspectionParameterUpdate(sender, e);
        }

        public void OnCognexImageUpdate(object sender, CogImageArgs e)
        {
            // Do not Use
        }

        public void OnCognexImageDownload(object sender, CogImageArgs e)
        {
            if (e.InspectType != eTypeInspect.Preprocessing) return;
            m_pCogImageSource = e.Image.Clone() as CognexImage;

            switch (sender)
            {
                case Button pButtonUpdate:
                    cogDisplay_PreviousView.StaticGraphics.Clear();
                    cogDisplay_PreviousView.InteractiveGraphics.Clear();
                    cogDisplay_PreviousView.Image = m_pCogImageSource.CogImage;
                    break;
                case IInspectionTab pTabInsp:
                    cogDisplay_PreviousView.StaticGraphics.Clear();
                    cogDisplay_PreviousView.InteractiveGraphics.Clear();
                    cogDisplay_PreviousView.Image = m_pCogImageSource.CogImage;
                    break;
            }
        }

        public void OnCognexToolDownload(object sender, CogToolArgs e)
        {
            // Do not Use
        }

        public void OnCognexToolUpdate(object sender, CogToolArgs e)
        {
            CommonClass.pCogToolTemplate[m_nType][e.ToolType][string.Empty] = e.CogTool;
            OnInspectionParameterUpdate(sender, e);

            m_pCogImageProcessing = e.ContainImage;
        }

        public void OnInspectionParameterUpdate(object sender, EventArgs e)
        {
            //
        }

        public void OnInspectionParameterDownload(object sender, EventArgs e)
        {
            ParameterInspectionPreprocessing pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionPreprocessing;
            {
                checkBox_IsUsePreprocessing.Checked = pParam.IsUse;
                checkBox_IsUseImageConvert.Checked = pParam.IsUseImageConvert;
                checkBox_IsUseSobel.Checked = pParam.IsUseSobelEdge;
                checkBox_IsUseFiltering.Checked = pParam.IsUseImageFilter;
                //// Property Grid Reset
                propertyGrid_ParamSetting.SelectedObject = pParam;
            }
        }
        private void button_ProcessPreprocess_Click(object sender, EventArgs e)
        {
            ParameterInspectionPreprocessing pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionPreprocessing;
            if (!pParam.IsUse) return;

            //// 공용으로 사용하기 전 Process 초기화
            OnInspectionParameterUpdate(sender, e);

            if (CommonClass.ProcessPreprocessing(m_pCogImageSource, ref m_pCogImageResult))
            {
                //// Inspection Info Download
                OnInspectionParameterDownload(sender, e);
                //// Display Update
                cogDisplay_ProcessView.StaticGraphics.Clear();
                cogDisplay_ProcessView.InteractiveGraphics.Clear();
                cogDisplay_ProcessView.Image = m_pCogImageResult.CogImage;
                //// Result Image를 다른 Tab으로 넘기기
                OnUpdateResultImageEvent(this, new CogImageArgs(m_nIndex, m_nType, m_pCogImageResult));
            }
        }

        private void button_SettingImageConvert_Click(object sender, EventArgs e)
        {
            button_SettingImageConvert.Enabled = false; // 중복 실행 방지

            ////  Form 생성하기
            Form_CogImageConvert pCogForm = new Form_CogImageConvert();
            pCogForm.CogImageSource = m_pCogImageSource.CogImage;    // Convert는 무조건 Source 입력
            pCogForm.CogToolLabel = eLabelInspect.None;
            pCogForm.CogTool = CommonClass.pCogToolTemplate[m_nType][eYoonCognexType.Convert][string.Empty] as CogImageConvertTool;
            pCogForm.OnUpdateCogToolEvent += OnCognexToolUpdate;
            pCogForm.Show();

            button_SettingImageConvert.Enabled = true;
        }

        private void button_UpdateSetting_Click(object sender, EventArgs e)
        {
            OnInspectionParameterUpdate(sender, e);
            OnInspectionParameterDownload(sender, e);
        }

        private void button_SettingFiltering_Click(object sender, EventArgs e)
        {
            button_SettingFiltering.Enabled = false;

            ////  Form 생성하기
            Form_CogIPOneImage pCogForm = new Form_CogIPOneImage();
            pCogForm.CogImageSource = (m_pCogImageProcessing != null) ? m_pCogImageProcessing.CogImage : m_pCogImageSource.CogImage;
            pCogForm.CogToolLabel = eLabelInspect.None;
            pCogForm.CogTool = CommonClass.pCogToolTemplate[m_nType][eYoonCognexType.Filtering][string.Empty] as CogIPOneImageTool;
            pCogForm.OnUpdateCogToolEvent += OnCognexToolUpdate;
            pCogForm.Show();

            button_SettingFiltering.Enabled = true;
        }

        private void button_SettingSobel_Click(object sender, EventArgs e)
        {
            button_SettingSobel.Enabled = false;

            ////  Form 생성하기
            Form_CogImageSobelEdge pCogForm = new Form_CogImageSobelEdge();
            pCogForm.CogImageSource = (m_pCogImageProcessing != null) ? m_pCogImageProcessing.CogImage as CogImage8Grey : m_pCogImageSource.CogImage as CogImage8Grey;
            pCogForm.CogToolLabel = eLabelInspect.None;
            pCogForm.CogTool = CommonClass.pCogToolTemplate[m_nType][eYoonCognexType.Sobel][string.Empty] as CogSobelEdgeTool;
            pCogForm.OnUpdateCogToolEvent += OnCognexToolUpdate;
            pCogForm.Show();

            button_SettingSobel.Enabled = true;
        }
    }
}
