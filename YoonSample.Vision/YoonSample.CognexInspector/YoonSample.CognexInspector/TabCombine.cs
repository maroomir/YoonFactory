using Cognex.VisionPro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoonFactory.Cognex;

namespace RobotIntegratedVision
{
    public partial class FormTab_Combine : Form, IInspectionTab
    {
        private int m_nIndex, m_nIndexModel, m_nIndexJob;
        private int m_nModelNo, m_nJobNo;
        private string m_strModelName, m_strJobName;
        private InspectionInfo m_pInspectionInfo;
        private ICogImage m_pCogImageOrigin;
        private ICogImage m_pCogImagePreprocessing;
        private ICogImage m_pCogImageSourceSelected;
        private ICogImage m_pCogImageObjectSelected;
        private ICogImage m_pCogImageResult;
        // Source Image Selecter 관련
        private List<KeyValuePair<int, eTypeInspect>> m_pListTotalInspFlag;
        public event PassImageCallback OnUpdateResultImageEvent;

        public FormTab_Combine(int nIndexModel, int nIndexJob, int nIndex)
        {
            InitializeComponent();

            m_nIndexModel = nIndexModel;
            m_nIndexJob = nIndexJob;
            m_nIndex = nIndex;

            m_pInspectionInfo = CommonClass.pListModel[m_nIndexModel].JobList[m_nIndexJob].InspectionList[m_nIndex];
            m_nModelNo = CommonClass.pListModel[m_nIndexModel].No;
            m_strModelName = CommonClass.pListModel[m_nIndexModel].Name;
            m_nJobNo = CommonClass.pListModel[m_nIndexModel].JobList[m_nIndexJob].No;
            m_strJobName = CommonClass.pListModel[m_nIndexModel].JobList[m_nIndexJob].Name;
        }

        private void FormTab_Combine_Load(object sender, EventArgs e)
        {
            cogDisplay_SourceView.AutoFit = true;
            cogDisplay_ObjectView.AutoFit = true;
            cogDisplay_CombineView.AutoFit = true;

            //// 유효성 체크
            if(m_pInspectionInfo == null)
            {
                CommonClass.pCLM.Write("Combine Load Failure : Inspection Info isnot valid");
                Close();
                return;
            }

            ////  변수 초기화
            m_pListTotalInspFlag = CommonFunction.GetTotalInspectFlagList(m_nModelNo, m_strModelName, m_nJobNo, m_strJobName);

            ////  Form 초기화
            ParameterInspectionCombine pParam = m_pInspectionInfo.InspectionParam as ParameterInspectionCombine;
            checkBox_IsUseCombine.Checked = pParam.IsUse;
            switch(pParam.CombineType)
            {
                case eTypeProcessTwoImage.Add:
                    radioButton_CombineAdd.Checked = true;
                    break;
                case eTypeProcessTwoImage.OverlapMax:
                    radioButton_CombineOverlapMax.Checked = true;
                    break;
                case eTypeProcessTwoImage.OverlapMin:
                    radioButton_CombineOverlapMin.Checked = true;
                    break;
                case eTypeProcessTwoImage.Subtract:
                    radioButton_CombineSubtract.Checked = true;
                    break;
                default:
                    break;
            }

            ////  Grid 초기화
            propertyGrid_ParamSetting.SelectedObject = pParam;

            ////  View 초기화
            InitComboBoxSource();

            ////  Event 초기화
            checkBox_IsUseCombine.Click += OnCheckBoxEnableClick;
            radioButton_CombineAdd.Click += OnRadioButtonCombineClick;
            radioButton_CombineOverlapMax.Click += OnRadioButtonCombineClick;
            radioButton_CombineOverlapMin.Click += OnRadioButtonCombineClick;
            radioButton_CombineSubtract.Click += OnRadioButtonCombineClick;
        }

        private void FormTab_Combine_FormClosed(object sender, FormClosedEventArgs e)
        {
            //// View 해제
            cogDisplay_SourceView.StaticGraphics.Clear();
            cogDisplay_SourceView.InteractiveGraphics.Clear();
            cogDisplay_SourceView.Dispose();
            cogDisplay_ObjectView.StaticGraphics.Clear();
            cogDisplay_ObjectView.InteractiveGraphics.Clear();
            cogDisplay_ObjectView.Dispose();
            cogDisplay_CombineView.StaticGraphics.Clear();
            cogDisplay_CombineView.InteractiveGraphics.Clear();
            cogDisplay_CombineView.Dispose();

            //// 변수 삭제
            m_pListTotalInspFlag.Clear();
            m_pListTotalInspFlag = null;

            //// 이벤트 해제
            checkBox_IsUseCombine.Click -= OnCheckBoxEnableClick;
            radioButton_CombineAdd.Click -= OnRadioButtonCombineClick;
            radioButton_CombineOverlapMax.Click -= OnRadioButtonCombineClick;
            radioButton_CombineOverlapMin.Click -= OnRadioButtonCombineClick;
            radioButton_CombineSubtract.Click -= OnRadioButtonCombineClick;
        }

        private void InitComboBoxSource()
        {
            ParameterInspectionCombine pParam = m_pInspectionInfo.InspectionParam as ParameterInspectionCombine;

            ////  Source Image 초기화
            comboBox_SelectSourceImage.Items.Clear();
            comboBox_SelectSourceImage.Items.Add("Origin");
            comboBox_SelectSourceImage.Items.Add("CurrentProcessing");
            foreach (KeyValuePair<int, eTypeInspect> pPair in m_pListTotalInspFlag)
            {
                if (pPair.Key == m_pInspectionInfo.No && pPair.Value == m_pInspectionInfo.InspectType)
                    break;
                string strSelectBoxContents = CommonFunction.SetInspectFlagToStringTag(pPair.Key, pPair.Value);
                comboBox_SelectSourceImage.Items.Add(strSelectBoxContents);
            }
            switch (pParam.SelectedSourceLevel)
            {
                case eLevelImageSelection.Origin:
                    comboBox_SelectSourceImage.SelectedIndex = 0;
                    break;
                case eLevelImageSelection.CurrentProcessing:
                    comboBox_SelectSourceImage.SelectedIndex = 1;
                    break;
                case eLevelImageSelection.Custom:
                    comboBox_SelectSourceImage.SelectedItem = CommonFunction.SetInspectFlagToStringTag(pParam.SelectedSourceNo, pParam.SelectedSourceType);
                    break;
            }

            ////  Object Image 초기화
            comboBox_SelectObjectImage.Items.Clear();
            comboBox_SelectObjectImage.Items.Add("Origin");
            comboBox_SelectObjectImage.Items.Add("CurrentProcessing");
            foreach (KeyValuePair<int, eTypeInspect> pPair in m_pListTotalInspFlag)
            {
                if (pPair.Key == m_pInspectionInfo.No && pPair.Value == m_pInspectionInfo.InspectType)
                    break;
                string strSelectBoxContents = CommonFunction.SetInspectFlagToStringTag(pPair.Key, pPair.Value);
                comboBox_SelectObjectImage.Items.Add(strSelectBoxContents);
            }
            switch (pParam.SelectedObjectLevel)
            {
                case eLevelImageSelection.Origin:
                    comboBox_SelectObjectImage.SelectedIndex = 0;
                    break;
                case eLevelImageSelection.CurrentProcessing:
                    comboBox_SelectObjectImage.SelectedIndex = 1;
                    break;
                case eLevelImageSelection.Custom:
                    comboBox_SelectObjectImage.SelectedItem = CommonFunction.SetInspectFlagToStringTag(pParam.SelectedObjectNo, pParam.SelectedObjectType);
                    break;
            }
        }

        public void OnCheckBoxEnableClick(object sender, EventArgs e)
        {
            CheckBox pBox = (CheckBox)sender;
            ParameterInspectionCombine pParam = m_pInspectionInfo.InspectionParam as ParameterInspectionCombine;
            switch (pBox.Name)
            {
                case "checkBox_IsUseCombine":
                    pParam.IsUse = pBox.Checked;
                    break;
                default:
                    break;
            }

            //// 변경 즉시 반영 (적용지연 에러 발생에 대한 대처사항)
            m_pInspectionInfo.InspectionParam = pParam;
            OnInspectionParameterUpdate(sender, e);
        }

        public void OnRadioButtonCombineClick(object sender, EventArgs e)
        {
            RadioButton pButton = (RadioButton)sender;
            ParameterInspectionCombine pParam = m_pInspectionInfo.InspectionParam as ParameterInspectionCombine;
            switch (pButton.Name)
            {
                case "radioButton_CombineAdd":
                    if (pButton.Checked)
                        pParam.CombineType = eTypeProcessTwoImage.Add;
                    break;
                case "radioButton_CombineOverlapMax":
                    if (pButton.Checked)
                        pParam.CombineType = eTypeProcessTwoImage.OverlapMax;
                    break;
                case "radioButton_CombineOverlapMin":
                    if (pButton.Checked)
                        pParam.CombineType = eTypeProcessTwoImage.OverlapMin;
                    break;
                case "radioButton_CombineSubtract":
                    if (pButton.Checked)
                        pParam.CombineType = eTypeProcessTwoImage.Subtract;
                    break;
                default:
                    break;
            }

            //// 변경 즉시 반영 (적용지연 에러 발생에 대한 대처사항)
            m_pInspectionInfo.InspectionParam = pParam;
            OnInspectionParameterUpdate(sender, e);
        }

        public void OnCognexImageUpdate(object sender, CogImageArgs e)
        {
            // Do not Use
        }

        public void OnCognexImageDownload(object sender, CogImageArgs e)
        {
            if (e.InspectType != eTypeInspect.Combine) return;
            if (m_pListTotalInspFlag == null) return;   // ComboBox가 초기화되지 않은 경우

            switch (sender)
            {
                case Button pButtonUpdate:
                    m_pCogImageOrigin = e.CogImage.CopyBase(CogImageCopyModeConstants.CopyPixels);
                    break;
                case IInspectionTab pTabInsp:
                    m_pCogImagePreprocessing = e.CogImage.CopyBase(CogImageCopyModeConstants.CopyPixels);
                    break;
            }
        }

        public void OnCognexToolUpdate(object sender, CogToolArgs e)
        {
            // Cognex Tool Factory 사용 안함
        }

        public void OnCognexToolDownload(object sender, CogToolArgs e)
        {
            // Do not Use
        }

        public void OnInspectionParameterUpdate(object sender, EventArgs e)
        {
            CommonClass.pListModel[m_nIndexModel].JobList[m_nIndexJob].InspectionList[m_nIndex] = m_pInspectionInfo;
            CommonClass.pConfig.SelectedInspectionNo = m_pInspectionInfo.No;
            CommonClass.pConfig.SelectedInspectionType = m_pInspectionInfo.InspectType;
        }

        private void button_UpdateSetting_Click(object sender, EventArgs e)
        {
            OnInspectionParameterUpdate(sender, e);
            OnInspectionParameterDownload(sender, e);
        }

        public void OnInspectionParameterDownload(object sender, EventArgs e)
        {
            m_pInspectionInfo = CommonClass.pListModel[m_nIndexModel].JobList[m_nIndexJob].InspectionList[m_nIndex];

            ParameterInspectionCombine pParam = m_pInspectionInfo.InspectionParam as ParameterInspectionCombine;
            {
                checkBox_IsUseCombine.Checked = pParam.IsUse;
                switch (pParam.CombineType)
                {
                    case eTypeProcessTwoImage.Add:
                        radioButton_CombineAdd.Checked = true;
                        break;
                    case eTypeProcessTwoImage.OverlapMax:
                        radioButton_CombineOverlapMax.Checked = true;
                        break;
                    case eTypeProcessTwoImage.OverlapMin:
                        radioButton_CombineOverlapMin.Checked = true;
                        break;
                    case eTypeProcessTwoImage.Subtract:
                        radioButton_CombineSubtract.Checked = true;
                        break;
                    default:
                        break;
                }
                ////  Grid 초기화
                propertyGrid_ParamSetting.SelectedObject = pParam;
                ////  View 초기화
                InitComboBoxSource();
            }
        }

        private void button_ProcessCombine_Click(object sender, EventArgs e)
        {
            ParameterInspectionCombine pParam = m_pInspectionInfo.InspectionParam as ParameterInspectionCombine;
            if (!pParam.IsUse) return;

            ////  공용으로 사용하기 전 Process 초기화
            OnInspectionParameterUpdate(sender, e);

            if (CommonFunction.ProcessImageCombine(m_pCogImageSourceSelected, m_pCogImageObjectSelected, ref m_pCogImageResult)) // 선택된 Source Image와 Object Image가 들어감
            {
                //// Inspection Info Download
                OnInspectionParameterDownload(sender, e);
                //// Display Update
                cogDisplay_CombineView.StaticGraphics.Clear();
                cogDisplay_CombineView.InteractiveGraphics.Clear();
                cogDisplay_CombineView.Image = m_pCogImageResult;
                //// Result Image를 다른 Tab으로 넘기기
                OnUpdateResultImageEvent(this, new CogImageArgs(m_pInspectionInfo.No, m_pInspectionInfo.InspectType, m_pCogImageResult));
            }
        }

        private void button_SetSourceImage_Click(object sender, EventArgs e)
        {
            ParameterInspectionCombine pParam = m_pInspectionInfo.InspectionParam as ParameterInspectionCombine;
            if (m_pCogImageOrigin == null || m_pCogImagePreprocessing == null)
                return;

            ////  현재 ComboBox 선택 Value 취득
            string strSourceSelected = (string)comboBox_SelectSourceImage.SelectedItem;
            switch (strSourceSelected)
            {
                case "Origin":
                    m_pCogImageSourceSelected = m_pCogImageOrigin.CopyBase(CogImageCopyModeConstants.CopyPixels);
                    pParam.SelectedSourceLevel = eLevelImageSelection.Origin;
                    break;
                case "CurrentProcessing":
                    m_pCogImageSourceSelected = m_pCogImagePreprocessing.CopyBase(CogImageCopyModeConstants.CopyPixels);
                    pParam.SelectedSourceLevel = eLevelImageSelection.CurrentProcessing;
                    break;
                default:
                    KeyValuePair<int, eTypeInspect> pPair = CommonFunction.GetInspectFlagFromStringTag(strSourceSelected);
                    m_pCogImageSourceSelected = CommonFunction.GetResultImage(m_nModelNo, m_strModelName, m_nJobNo, m_strJobName, pPair.Key, pPair.Value);
                    pParam.SelectedSourceLevel = eLevelImageSelection.Custom;
                    pParam.SelectedSourceNo = pPair.Key;
                    pParam.SelectedSourceType = pPair.Value;
                    break;
            }
            cogDisplay_SourceView.StaticGraphics.Clear();
            cogDisplay_SourceView.InteractiveGraphics.Clear();
            cogDisplay_SourceView.Image = m_pCogImageSourceSelected;

            //// 변경 즉시 반영 (적용지연 에러 발생에 대한 대처사항)
            m_pInspectionInfo.InspectionParam = pParam;
            OnInspectionParameterUpdate(sender, e);
        }

        private void button_SetObjectImage_Click(object sender, EventArgs e)
        {
            ParameterInspectionCombine pParam = m_pInspectionInfo.InspectionParam as ParameterInspectionCombine;
            if (m_pCogImageOrigin == null || m_pCogImagePreprocessing == null)
                return;

            ////  현재 ComboBox 선택 Value 취득
            string strObjectSelected = (string)comboBox_SelectObjectImage.SelectedItem;
            switch (strObjectSelected)
            {
                case "Origin":
                    m_pCogImageObjectSelected = m_pCogImageOrigin.CopyBase(CogImageCopyModeConstants.CopyPixels);
                    pParam.SelectedObjectLevel = eLevelImageSelection.Origin;
                    break;
                case "CurrentProcessing":
                    m_pCogImageObjectSelected = m_pCogImagePreprocessing.CopyBase(CogImageCopyModeConstants.CopyPixels);
                    pParam.SelectedObjectLevel = eLevelImageSelection.CurrentProcessing;
                    break;
                default:
                    KeyValuePair<int, eTypeInspect> pPair = CommonFunction.GetInspectFlagFromStringTag(strObjectSelected);
                    m_pCogImageObjectSelected = CommonFunction.GetResultImage(m_nModelNo, m_strModelName, m_nJobNo, m_strJobName, pPair.Key, pPair.Value);
                    pParam.SelectedObjectLevel = eLevelImageSelection.Custom;
                    pParam.SelectedObjectNo = pPair.Key;
                    pParam.SelectedObjectType = pPair.Value;
                    break;
            }
            cogDisplay_ObjectView.StaticGraphics.Clear();
            cogDisplay_ObjectView.InteractiveGraphics.Clear();
            cogDisplay_ObjectView.Image = m_pCogImageObjectSelected;

            //// 변경 즉시 반영 (적용지연 에러 발생에 대한 대처사항)
            m_pInspectionInfo.InspectionParam = pParam;
            OnInspectionParameterUpdate(sender, e);
        }
    }
}
