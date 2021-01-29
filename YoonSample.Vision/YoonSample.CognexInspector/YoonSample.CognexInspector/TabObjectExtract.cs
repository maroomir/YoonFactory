using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.ColorSegmenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoonFactory;
using YoonFactory.Cognex;

namespace YoonSample.CognexInspector
{
    public partial class TabObjectExtract : Form, IInspectionTab
    {
        private int m_nIndex = -1;
        private eTypeInspect m_nType = eTypeInspect.None;
        private ICogImage m_pCogImageOrigin;
        private ICogImage m_pCogImagePreprocessing;
        private ICogImage m_pCogImageSourceSelected;
        private ICogImage m_pCogImageResult;
        public event PassImageCallback OnUpdateResultImageEvent;

        public TabObjectExtract(int nIndex)
        {
            InitializeComponent();

            m_nIndex = nIndex;
            m_nType = eTypeInspect.ObjectExtract;
        }

        private void FormTab_ObjectExtract_Load(object sender, EventArgs e)
        {
            cogDisplay_PrevView.AutoFit = true;
            cogDisplay_ProcessView.AutoFit = true;

            //// 유효성 체크
            if (!CommonClass.pParamTemplate.ContainsKey(m_nType)
                && !CommonClass.pCogToolTemplate.ContainsKey(m_nType))
            {
                CommonClass.pCLM.Write("Object Extract Load Failure : Inspection Info isnot valid");
                Close();
                return;
            }

            ////  Form 초기화
            ParameterInspectionObjectExtract pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionObjectExtract;
            checkBox_IsUseObjectExtract.Checked = pParam.IsUse;
            checkBox_IsUseBlob.Checked = pParam.IsUseBlob;
            checkBox_IsUseColorSegment.Checked = pParam.IsUseColorSegment;
            if (pParam.CombineType == eTypeProcessTwoImage.OverlapMin)
                radioButton_CombineOverlapMin.Checked = true;
            else if(pParam.CombineType == eTypeProcessTwoImage.OverlapMax)
                radioButton_CombineOverlapMax.Checked = true;

            //// Grid 초기화
            propertyGrid_ParamSetting.SelectedObject = pParam;

            //// View 초기화
            InitComboBoxSource();

            //// 이벤트 초기화
            checkBox_IsUseObjectExtract.Click += OnCheckBoxEnableClick;
            checkBox_IsUseBlob.Click += OnCheckBoxEnableClick;
            checkBox_IsUseColorSegment.Click += OnCheckBoxEnableClick;
            radioButton_CombineOverlapMax.Click += OnRadioButtonCombineClick;
            radioButton_CombineOverlapMin.Click += OnRadioButtonCombineClick;
        }

        private void FormTab_ObjectExtract_FormClosed(object sender, FormClosedEventArgs e)
        {
            //// View 해제
            cogDisplay_PrevView.StaticGraphics.Clear();
            cogDisplay_PrevView.InteractiveGraphics.Clear();
            cogDisplay_PrevView.Dispose();
            cogDisplay_ProcessView.StaticGraphics.Clear();
            cogDisplay_ProcessView.InteractiveGraphics.Clear();
            cogDisplay_ProcessView.Dispose();

            //// 이벤트 해제
            checkBox_IsUseObjectExtract.Click -= OnCheckBoxEnableClick;
            checkBox_IsUseBlob.Click -= OnCheckBoxEnableClick;
            checkBox_IsUseColorSegment.Click -= OnCheckBoxEnableClick;
            radioButton_CombineOverlapMax.Click -= OnRadioButtonCombineClick;
            radioButton_CombineOverlapMin.Click -= OnRadioButtonCombineClick;
        }

        private void InitComboBoxSource()
        {
            ////  Source Image
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
            comboBox_SelectSourceImage.SelectedIndex = 0;

            //// Blob 설정 Image
            ParameterInspectionObjectExtract pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionObjectExtract;
            comboBox_SelectBlobSource.Items.Clear();
            comboBox_SelectBlobSource.Items.Add("Origin");
            comboBox_SelectBlobSource.Items.Add("CurrentProcessing");
            foreach (ToolTemplate pTemplate in CommonClass.pCogToolTemplate.Values)
            {
                if (pTemplate.No == m_nIndex && pTemplate.Name == m_nType.ToString())
                    break;
                string strSelectBoxContents = pTemplate.ToString();
                comboBox_SelectSourceImage.Items.Add(strSelectBoxContents);
            }
            switch (pParam.SelectedBlobImageLevel)
            {
                case eLevelImageSelection.Origin:
                    comboBox_SelectBlobSource.SelectedIndex = 0;
                    break;
                case eLevelImageSelection.CurrentProcessing:
                    comboBox_SelectBlobSource.SelectedIndex = 1;
                    break;
                case eLevelImageSelection.Custom:
                    comboBox_SelectBlobSource.SelectedItem = CommonClass.pCogToolTemplate[pParam.SelectedBlobImageType].ToString();
                    break;
                default:
                    break;
            }

            ////  Color Segment 설정 Image
            comboBox_SelectColorSegmentSource.Items.Clear();
            comboBox_SelectColorSegmentSource.Items.Add("Origin");
            comboBox_SelectColorSegmentSource.Items.Add("CurrentProcessing");
            foreach (ToolTemplate pTemplate in CommonClass.pCogToolTemplate.Values)
            {
                if (pTemplate.No == m_nIndex && pTemplate.Name == m_nType.ToString())
                    break;
                string strSelectBoxContents = pTemplate.ToString();
                comboBox_SelectSourceImage.Items.Add(strSelectBoxContents);
            }
            switch (pParam.SelectedColorSegmentImageLevel)
            {
                case eLevelImageSelection.Origin:
                    comboBox_SelectColorSegmentSource.SelectedIndex = 0;
                    break;
                case eLevelImageSelection.CurrentProcessing:
                    comboBox_SelectColorSegmentSource.SelectedIndex = 1;
                    break;
                case eLevelImageSelection.Custom:
                    comboBox_SelectColorSegmentSource.SelectedItem = CommonClass.pCogToolTemplate[pParam.SelectedColorSegmentImageType].ToString();
                    break;
                default:
                    break;
            }
        }

        public void OnCheckBoxEnableClick(object sender, EventArgs e)
        {
            CheckBox pBox = (CheckBox)sender;
            ParameterInspectionObjectExtract pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionObjectExtract;
            switch (pBox.Name)
            {
                case "checkBox_IsUseObjectExtract":
                    pParam.IsUse = pBox.Checked;
                    break;
                case "checkBox_IsUseBlob":
                    pParam.IsUseBlob = pBox.Checked;
                    break;
                case "checkBox_IsUseColorSegment":
                    pParam.IsUseColorSegment = pBox.Checked;
                    break;
                default:
                    break;
            }

            //// 변경 즉시 반영 (적용지연 에러 발생에 대한 대처사항)
            CommonClass.pParamTemplate[m_nType].Parameter = pParam;
            OnInspectionParameterUpdate(sender, e);
        }

        public void OnRadioButtonCombineClick(object sender, EventArgs e)
        {
            RadioButton pButton = (RadioButton)sender;
            ParameterInspectionObjectExtract pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionObjectExtract;
            switch (pButton.Name)
            {
                case "radioButton_CombineOverlapMax":
                    if (pButton.Checked)
                        pParam.CombineType = eTypeProcessTwoImage.OverlapMax;
                    break;
                case "radioButton_CombineOverlapMin":
                    if (pButton.Checked)
                        pParam.CombineType = eTypeProcessTwoImage.OverlapMin;
                    break;
                default:
                    break;
            }

            //// 변경 즉시 반영 (적용지연 에러 발생에 대한 대처사항)
            CommonClass.pParamTemplate[m_nType].Parameter = pParam;
            OnInspectionParameterUpdate(sender, e);
        }

        public void OnCognexImageUpdate(object sender, CogImageArgs e)
        {
            // Do not Use
        }

        public void OnCognexImageDownload(object sender, CogImageArgs e)
        {
            if (e.InspectType != eTypeInspect.ObjectExtract || e.CogImage == null) return;

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
            CommonClass.pCogToolTemplate[m_nType][e.ToolType][string.Empty] = e.CogTool;
            OnInspectionParameterUpdate(sender, e);
        }

        public void OnCognexToolDownload(object sender, CogToolArgs e)
        {
            //
        }

        public void OnInspectionParameterUpdate(object sender, EventArgs e)
        {
            CommonClass.pConfig.SelectedInspectionNo = m_nIndex;
            CommonClass.pConfig.SelectedInspectionType = m_nType;
        }

        public void OnInspectionParameterDownload(object sender, EventArgs e)
        {
            ParameterInspectionObjectExtract pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionObjectExtract;
            {
                checkBox_IsUseObjectExtract.Checked = pParam.IsUse;

                checkBox_IsUseBlob.Checked = pParam.IsUseBlob;
                checkBox_IsUseColorSegment.Checked = pParam.IsUseColorSegment;
                if (pParam.CombineType == eTypeProcessTwoImage.OverlapMin)
                    radioButton_CombineOverlapMin.Checked = true;
                else if (pParam.CombineType == eTypeProcessTwoImage.OverlapMax)
                    radioButton_CombineOverlapMax.Checked = true;

                //// Grid 초기화
                propertyGrid_ParamSetting.SelectedObject = pParam;
                //// ComboBox 초기화
                InitComboBoxSource();
            }
        }

        private void button_ProcessObjectExtract_Click(object sender, EventArgs e)
        {
            ParameterInspectionObjectExtract pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionObjectExtract;
            if (!pParam.IsUse) return;

            //// 공용으로 사용하기 전 Process 초기화
            OnInspectionParameterUpdate(sender, e);

            //// Image 가져오기
            CogImage8Grey pImageBlob = GetSourceImage(pParam.SelectedBlobImageLevel, pParam.SelectedBlobImageNo, pParam.SelectedBlobImageType) as CogImage8Grey;
            CogImage24PlanarColor pImageColorExtract = GetSourceImage(pParam.SelectedColorSegmentImageLevel, pParam.SelectedColorSegmentImageNo, pParam.SelectedColorSegmentImageType) as CogImage24PlanarColor;

            if (CommonClass.ProcessObjectExtract(pImageBlob, pImageColorExtract, ref m_pCogImageResult))  // 선택된 Source Image가 들어감
            {
                //// Inspection Info Download
                OnInspectionParameterDownload(sender, e);
                //// Display Update
                cogDisplay_ProcessView.StaticGraphics.Clear();
                cogDisplay_ProcessView.InteractiveGraphics.Clear();
                cogDisplay_ProcessView.Image = m_pCogImageResult;
                //// Result에 맞게 Display 위에 결과 그리기
                CommonClass.SetResultRegionToDisplay(cogDisplay_ProcessView, m_nIndex, m_nType);
                //// Result Image를 다른 Tab으로 넘기기
                OnUpdateResultImageEvent(this, new CogImageArgs(m_nIndex, m_nType, m_pCogImageResult));
            }
        }

        private void button_SetSourceImage_Click(object sender, EventArgs e)
        {
            if (m_pCogImageOrigin == null || m_pCogImagePreprocessing == null)
                return;

            ////  현재 ComboBox 선택 Value 취득
            string strSourceSelected = (string)comboBox_SelectSourceImage.SelectedItem;
            switch(strSourceSelected)
            {
                case "Origin":
                    m_pCogImageSourceSelected = m_pCogImageOrigin.CopyBase(CogImageCopyModeConstants.CopyPixels);
                    break;
                case "CurrentProcessing":
                    m_pCogImageSourceSelected = m_pCogImagePreprocessing.CopyBase(CogImageCopyModeConstants.CopyPixels);
                    break;
                default:
                    KeyValuePair<int, eTypeInspect> pPair = CommonClass.GetInspectFlagFromStringTag(strSourceSelected);
                    m_pCogImageSourceSelected = CommonClass.GetResultImage(pPair.Key, pPair.Value);
                    break;
            }
            cogDisplay_PrevView.StaticGraphics.Clear();
            cogDisplay_PrevView.InteractiveGraphics.Clear();
            cogDisplay_PrevView.Image = m_pCogImageSourceSelected;
        }

        private void button_UpdateSetting_Click(object sender, EventArgs e)
        {
            OnInspectionParameterUpdate(sender, e);
            OnInspectionParameterDownload(sender, e);
        }

        private void button_SettingBlob_Click(object sender, EventArgs e)
        {
            button_SettingBlob.Enabled = false;

            ////  Image Select 식별하기
            int nInspNoSelected = 0;
            eTypeInspect nInspTypeSelected = eTypeInspect.None;
            eLevelImageSelection nLevelSource = eLevelImageSelection.None;
            string strSourceSelected = (string)comboBox_SelectBlobSource.SelectedItem;
            ICogImage pImageBlob = GetSourceImageToComboBoxString(strSourceSelected, ref nLevelSource, ref nInspNoSelected, ref nInspTypeSelected);
            ParameterInspectionObjectExtract pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionObjectExtract;
            pParam.SelectedBlobImageLevel = nLevelSource;
            pParam.SelectedBlobImageNo = nInspNoSelected;
            pParam.SelectedBlobImageType = nInspTypeSelected;

            //// 변경 즉시 반영 (적용지연 에러 발생에 대한 대처사항)
            CommonClass.pParamTemplate[m_nType].Parameter = pParam;
            OnInspectionParameterUpdate(sender, e);

            ////  Form 생성하기
            Form_CogBlob pCogForm = new Form_CogBlob();
            pCogForm.CogImageSource = pImageBlob;
            pCogForm.CogToolLabel = eLabelInspect.None;
            pCogForm.CogTool = CommonClass.pCogToolTemplate[m_nType][eYoonCognexType.Blob][string.Empty] as CogBlobTool;
            pCogForm.OnUpdateCogToolEvent += OnCognexToolUpdate;
            pCogForm.Show();

            button_SettingBlob.Enabled = true;
        }

        private void button_SettingColorSegment_Click(object sender, EventArgs e)
        {
            button_SettingColorSegment.Enabled = false;

            ////  Image Select 식별하기
            int nInspNoSelected = 0;
            eTypeInspect nInspTypeSelected = eTypeInspect.None;
            eLevelImageSelection nLevelSource = eLevelImageSelection.None;
            string strSourceSelected = (string)comboBox_SelectColorSegmentSource.SelectedItem;
            ICogImage pImageColorSegment = GetSourceImageToComboBoxString(strSourceSelected, ref nLevelSource, ref nInspNoSelected, ref nInspTypeSelected);
            ParameterInspectionObjectExtract pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionObjectExtract;
            pParam.SelectedColorSegmentImageLevel = nLevelSource;
            pParam.SelectedColorSegmentImageNo = nInspNoSelected;
            pParam.SelectedColorSegmentImageType = nInspTypeSelected;

            //// 변경 즉시 반영 (적용지연 에러 발생에 대한 대처사항)
            CommonClass.pParamTemplate[m_nType].Parameter = pParam;
            OnInspectionParameterUpdate(sender, e);

            ////  Form 생성하기
            Form_CogColorSegment pCogForm = new Form_CogColorSegment();
            pCogForm.CogImageSource = pImageColorSegment as CogImage24PlanarColor;
            pCogForm.CogToolLabel = eLabelInspect.None;
            pCogForm.CogTool = CommonClass.pCogToolTemplate[m_nType][eYoonCognexType.ColorSegment][string.Empty] as CogColorSegmenterTool;
            pCogForm.OnUpdateCogToolEvent += OnCognexToolUpdate;
            pCogForm.Show();

            button_SettingColorSegment.Enabled = true;
        }

        private ICogImage GetSourceImageToComboBoxString(string strSourceSelected, ref eLevelImageSelection nSourceLevel, ref int nInspNoSelected, ref eTypeInspect nInspTypeSelected)
        {
            ICogImage pImageResult = null;
            switch (strSourceSelected)
            {
                case "Origin":
                    pImageResult = m_pCogImageOrigin.CopyBase(CogImageCopyModeConstants.CopyPixels);
                    nSourceLevel = eLevelImageSelection.Origin;
                    break;
                case "CurrentProcessing":
                    pImageResult = m_pCogImagePreprocessing.CopyBase(CogImageCopyModeConstants.CopyPixels);
                    nSourceLevel = eLevelImageSelection.CurrentProcessing;
                    break;
                default:
                    KeyValuePair<int, eTypeInspect> pPair = CommonClass.GetInspectFlagFromStringTag(strSourceSelected);
                    pImageResult = CommonClass.GetResultImage(pPair.Key, pPair.Value);
                    nSourceLevel = eLevelImageSelection.Custom;
                    nInspNoSelected = pPair.Key;
                    nInspTypeSelected = pPair.Value;
                    break;
            }
            return pImageResult;
        }

        private ICogImage GetSourceImage(eLevelImageSelection nLevel, int nInspNo, eTypeInspect nTypeInsp)
        {
            switch (nLevel)
            {
                case eLevelImageSelection.Origin:
                    return m_pCogImageOrigin;
                case eLevelImageSelection.CurrentProcessing:
                    return m_pCogImagePreprocessing;
                case eLevelImageSelection.Custom:
                    return CommonClass.GetResultImage(nInspNo, nTypeInsp);
                default:
                    return null;
            }
        }
    }
}
