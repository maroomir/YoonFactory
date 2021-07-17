using System;
using System.Collections.Generic;
using System.Windows.Forms;
using YoonFactory.Cognex;
using YoonFactory.Cognex.Tool;

namespace YoonSample.CognexInspector
{
    public partial class TabCombine : Form, IInspectionTab
    {
        private int m_nIndex = -1;
        private eTypeInspect m_nType = eTypeInspect.None;
        private CognexImage m_pCogImageOrigin;
        private CognexImage m_pCogImagePreprocessing;
        private CognexImage m_pCogImageSourceSelected;
        private CognexImage m_pCogImageObjectSelected;
        private CognexImage m_pCogImageResult;
        public event PassImageCallback OnUpdateResultImageEvent;

        public TabCombine(int nIndex)
        {
            InitializeComponent();

            m_nIndex = nIndex;
            m_nType = eTypeInspect.Combine;
        }

        private void FormTab_Combine_Load(object sender, EventArgs e)
        {
            cogDisplay_SourceView.AutoFit = true;
            cogDisplay_ObjectView.AutoFit = true;
            cogDisplay_CombineView.AutoFit = true;

            //// 유효성 체크
            if(!CommonClass.pParamTemplate.ContainsKey(m_nType)
                && !CommonClass.pCogToolTemplate.ContainsKey(m_nType))
            {
                CommonClass.pCLM.Write("Combine Load Failure : Inspection Info isnot valid");
                Close();
                return;
            }

            ////  Form 초기화
            ParameterInspectionCombine pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionCombine;
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

            //// 이벤트 해제
            checkBox_IsUseCombine.Click -= OnCheckBoxEnableClick;
            radioButton_CombineAdd.Click -= OnRadioButtonCombineClick;
            radioButton_CombineOverlapMax.Click -= OnRadioButtonCombineClick;
            radioButton_CombineOverlapMin.Click -= OnRadioButtonCombineClick;
            radioButton_CombineSubtract.Click -= OnRadioButtonCombineClick;
        }

        private void InitComboBoxSource()
        {
            ParameterInspectionCombine pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionCombine;

            ////  Source Image 초기화
            comboBox_SelectSourceImage.Items.Clear();
            comboBox_SelectSourceImage.Items.Add("Origin");
            comboBox_SelectSourceImage.Items.Add("CurrentProcessing");
            foreach (ToolTemplate pTemplate in CommonClass.pCogToolTemplate.Values)
            {
                if (pTemplate.No == m_nIndex && pTemplate.Name == m_nType.ToString())
                    break;
                string strSelectBoxContents = pTemplate.ToString();
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
                    comboBox_SelectSourceImage.SelectedItem = CommonClass.pCogToolTemplate[pParam.SelectedSourceType].ToString();
                    break;
            }

            ////  Object Image 초기화
            comboBox_SelectObjectImage.Items.Clear();
            comboBox_SelectObjectImage.Items.Add("Origin");
            comboBox_SelectObjectImage.Items.Add("CurrentProcessing");
            foreach (ToolTemplate pTemplate in CommonClass.pCogToolTemplate.Values)
            {
                if (pTemplate.No == m_nIndex && pTemplate.Name == m_nType.ToString())
                    break;
                string strSelectBoxContents = pTemplate.ToString();
                comboBox_SelectSourceImage.Items.Add(strSelectBoxContents);
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
                    comboBox_SelectSourceImage.SelectedItem = CommonClass.pCogToolTemplate[pParam.SelectedObjectType].ToString();
                    break;
            }
        }

        public void OnCheckBoxEnableClick(object sender, EventArgs e)
        {
            CheckBox pBox = (CheckBox)sender;
            ParameterInspectionCombine pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionCombine;
            switch (pBox.Name)
            {
                case "checkBox_IsUseCombine":
                    pParam.IsUse = pBox.Checked;
                    break;
                default:
                    break;
            }

            //// 변경 즉시 반영 (적용지연 에러 발생에 대한 대처사항)
            CommonClass.pParamTemplate[m_nType].SetParameter(pParam, typeof(ParameterInspectionCombine));
            OnInspectionParameterUpdate(sender, e);
        }

        public void OnRadioButtonCombineClick(object sender, EventArgs e)
        {
            RadioButton pButton = (RadioButton)sender;
            ParameterInspectionCombine pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionCombine;
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
            CommonClass.pParamTemplate[m_nType].SetParameter(pParam, typeof(ParameterInspectionCombine));
            OnInspectionParameterUpdate(sender, e);
        }

        public void OnCognexImageUpdate(object sender, CogImageArgs e)
        {
            // Do not Use
        }

        public void OnCognexImageDownload(object sender, CogImageArgs e)
        {
            if (e.InspectType != eTypeInspect.Combine) return;

            switch (sender)
            {
                case Button pButtonUpdate:
                    m_pCogImageOrigin = e.Image.Clone() as CognexImage;
                    break;
                case IInspectionTab pTabInsp:
                    m_pCogImagePreprocessing = e.Image.Clone() as CognexImage;
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
            //
        }

        private void button_UpdateSetting_Click(object sender, EventArgs e)
        {
            OnInspectionParameterUpdate(sender, e);
            OnInspectionParameterDownload(sender, e);
        }

        public void OnInspectionParameterDownload(object sender, EventArgs e)
        {
            ParameterInspectionCombine pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionCombine;
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
            ParameterInspectionCombine pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionCombine;
            if (!pParam.IsUse) return;

            ////  공용으로 사용하기 전 Process 초기화
            OnInspectionParameterUpdate(sender, e);

            if (CommonClass.ProcessImageCombine(m_pCogImageSourceSelected, m_pCogImageObjectSelected, ref m_pCogImageResult)) // 선택된 Source Image와 Object Image가 들어감
            {
                //// Inspection Info Download
                OnInspectionParameterDownload(sender, e);
                //// Display Update
                cogDisplay_CombineView.StaticGraphics.Clear();
                cogDisplay_CombineView.InteractiveGraphics.Clear();
                cogDisplay_CombineView.Image = m_pCogImageResult.CogImage;
                //// Result Image를 다른 Tab으로 넘기기
                OnUpdateResultImageEvent(this, new CogImageArgs(m_nIndex, m_nType, m_pCogImageResult));
            }
        }

        private void button_SetSourceImage_Click(object sender, EventArgs e)
        {
            ParameterInspectionCombine pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionCombine;
            if (m_pCogImageOrigin == null || m_pCogImagePreprocessing == null)
                return;

            ////  현재 ComboBox 선택 Value 취득
            string strSourceSelected = (string)comboBox_SelectSourceImage.SelectedItem;
            switch (strSourceSelected)
            {
                case "Origin":
                    m_pCogImageSourceSelected = m_pCogImageOrigin.Clone() as CognexImage;
                    pParam.SelectedSourceLevel = eLevelImageSelection.Origin;
                    break;
                case "CurrentProcessing":
                    m_pCogImageSourceSelected = m_pCogImagePreprocessing.Clone() as CognexImage;
                    pParam.SelectedSourceLevel = eLevelImageSelection.CurrentProcessing;
                    break;
                default:
                    KeyValuePair<int, eTypeInspect> pPair = CommonClass.GetInspectFlagFromStringTag(strSourceSelected);
                    m_pCogImageSourceSelected = CommonClass.GetResultImage(pPair.Key, pPair.Value);
                    pParam.SelectedSourceLevel = eLevelImageSelection.Custom;
                    pParam.SelectedSourceNo = pPair.Key;
                    pParam.SelectedSourceType = pPair.Value;
                    break;
            }
            cogDisplay_SourceView.StaticGraphics.Clear();
            cogDisplay_SourceView.InteractiveGraphics.Clear();
            cogDisplay_SourceView.Image = m_pCogImageSourceSelected.CogImage;

            //// 변경 즉시 반영 (적용지연 에러 발생에 대한 대처사항)
            CommonClass.pParamTemplate[m_nType].SetParameter(pParam, typeof(ParameterInspectionCombine));
            OnInspectionParameterUpdate(sender, e);
        }

        private void button_SetObjectImage_Click(object sender, EventArgs e)
        {
            ParameterInspectionCombine pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionCombine;
            if (m_pCogImageOrigin == null || m_pCogImagePreprocessing == null)
                return;

            ////  현재 ComboBox 선택 Value 취득
            string strObjectSelected = (string)comboBox_SelectObjectImage.SelectedItem;
            switch (strObjectSelected)
            {
                case "Origin":
                    m_pCogImageObjectSelected = m_pCogImageOrigin.Clone() as CognexImage;
                    pParam.SelectedObjectLevel = eLevelImageSelection.Origin;
                    break;
                case "CurrentProcessing":
                    m_pCogImageObjectSelected = m_pCogImagePreprocessing.Clone() as CognexImage;
                    pParam.SelectedObjectLevel = eLevelImageSelection.CurrentProcessing;
                    break;
                default:
                    KeyValuePair<int, eTypeInspect> pPair = CommonClass.GetInspectFlagFromStringTag(strObjectSelected);
                    m_pCogImageObjectSelected = CommonClass.GetResultImage(pPair.Key, pPair.Value);
                    pParam.SelectedObjectLevel = eLevelImageSelection.Custom;
                    pParam.SelectedObjectNo = pPair.Key;
                    pParam.SelectedObjectType = pPair.Value;
                    break;
            }
            cogDisplay_ObjectView.StaticGraphics.Clear();
            cogDisplay_ObjectView.InteractiveGraphics.Clear();
            cogDisplay_ObjectView.Image = m_pCogImageObjectSelected.CogImage;

            //// 변경 즉시 반영 (적용지연 에러 발생에 대한 대처사항)
            CommonClass.pParamTemplate[m_nType].SetParameter(pParam, typeof(ParameterInspectionCombine));
            OnInspectionParameterUpdate(sender, e);
        }
    }
}
