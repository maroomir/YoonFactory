using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.PMAlign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoonFactory;
using YoonFactory.Align;
using YoonFactory.Cognex;
using YoonFactory.Cognex.Tool;
using YoonFactory.Windows;

namespace YoonSample.CognexInspector
{
    public partial class TabPatternMatching : Form, IInspectionTab
    {
        private int m_nIndex = -1;
        private eTypeInspect m_nType = eTypeInspect.None;
        private ICogImage m_pCogImageOrigin;
        private ICogImage m_pCogImagePreprocessing;
        private ICogImage m_pCogImageSourceSelected;
        private CogImage8Grey m_pCogImageResult;
        private DataTable m_pTableResult = null;
        public event PassImageCallback OnUpdateResultImageEvent;

        public TabPatternMatching(int nIndex)
        {
            InitializeComponent();

            m_nIndex = nIndex;
            m_nType = eTypeInspect.PatternMatching;
        }

        private void FormTab_PatternMatching_Load(object sender, EventArgs e)
        {
            cogDisplay_ProcessView.AutoFit = true;
            cogDisplay_MainPattern.AutoFit = true;
            cogDisplay_SecondPattern.AutoFit = true;
            cogDisplay_ThirdPattern.AutoFit = true;
            cogDisplay_ForthPattern.AutoFit = true;

            //// 유효성 체크
            if (!CommonClass.pParamTemplate.ContainsKey(m_nType)
                && !CommonClass.pCogToolTemplate.ContainsKey(m_nType))
            {
                CommonClass.pCLM.Write("Pattern Matching Load Failure : Inspection Info isnot valid");
                Close();
                return;
            }

            ////  Form 초기화
            ParameterInspectionPatternMatching pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionPatternMatching;
            checkBox_IsUsePM.Checked = pParam.IsUse;
            checkBox_IsUseMainPattern.Checked = pParam.IsUseEachPatterns[eLabelInspect.Main.ToInt()];
            checkBox_IsUseSecondPattern.Checked = pParam.IsUseEachPatterns[eLabelInspect.Second.ToInt()];
            checkBox_IsUseThirdPattern.Checked = pParam.IsUseEachPatterns[eLabelInspect.Third.ToInt()];
            checkBox_IsUseForthPattern.Checked = pParam.IsUseEachPatterns[eLabelInspect.Forth.ToInt()];
            checkBox_IsUseMultiPatternInspection.Checked = pParam.IsUseMultiPatternInspection;
            checkBox_IsCheckAlignOffset.Checked = pParam.IsCheckAlign;

            //// Grid 초기화
            propertyGrid_ParamSetting.SelectedObject = pParam;

            ////  View 초기화
            InitComboBoxSource();
            InitPatternView(eLabelInspect.Main);
            InitPatternView(eLabelInspect.Second);
            InitPatternView(eLabelInspect.Third);
            InitPatternView(eLabelInspect.Forth);
            InitDataTable();

            ////  이벤트 초기화
            checkBox_IsUsePM.Click += OnCheckBoxEnableClick;
            checkBox_IsUseMainPattern.Click += OnCheckBoxEnableClick;
            checkBox_IsUseSecondPattern.Click += OnCheckBoxEnableClick;
            checkBox_IsUseThirdPattern.Click += OnCheckBoxEnableClick;
            checkBox_IsUseForthPattern.Click += OnCheckBoxEnableClick;
        }

        private void FormTab_PatternMatching_FormClosed(object sender, FormClosedEventArgs e)
        {
            //// View 해제
            cogDisplay_ProcessView.StaticGraphics.Clear();
            cogDisplay_ProcessView.InteractiveGraphics.Clear();
            cogDisplay_ProcessView.Dispose();
            cogDisplay_MainPattern.StaticGraphics.Clear();
            cogDisplay_MainPattern.InteractiveGraphics.Clear();
            cogDisplay_MainPattern.Dispose();
            cogDisplay_SecondPattern.StaticGraphics.Clear();
            cogDisplay_SecondPattern.InteractiveGraphics.Clear();
            cogDisplay_SecondPattern.Dispose();
            cogDisplay_ThirdPattern.StaticGraphics.Clear();
            cogDisplay_ThirdPattern.InteractiveGraphics.Clear();
            cogDisplay_ThirdPattern.Dispose();
            cogDisplay_ForthPattern.StaticGraphics.Clear();
            cogDisplay_ForthPattern.InteractiveGraphics.Clear();
            cogDisplay_ForthPattern.Dispose();

            ////  이벤트 해제
            checkBox_IsUsePM.Click -= OnCheckBoxEnableClick;
            checkBox_IsUseMainPattern.Click -= OnCheckBoxEnableClick;
            checkBox_IsUseSecondPattern.Click -= OnCheckBoxEnableClick;
            checkBox_IsUseThirdPattern.Click -= OnCheckBoxEnableClick;
            checkBox_IsUseForthPattern.Click -= OnCheckBoxEnableClick;
        }

        private void InitComboBoxSource()
        {
            ParameterInspectionPatternMatching pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionPatternMatching;

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
                default:
                    break;
            }
        }

        private void InitDataTable()
        {
            //// Title 만들기
            List<string> pListTitle = new List<string>();
            {
                pListTitle.Add("Title");
                pListTitle.Add("PosX");     // 1 : 0 ~ 한계값mm
                pListTitle.Add("PosY");     // 2 : 0 ~ 한계값mm
                pListTitle.Add("AlignX");   // 3 : 0 ~ Align한계mm
                pListTitle.Add("AlignY");   // 4 : 0 ~ Align한계mm
                pListTitle.Add("AlignT");   // 5 : 0 ~ Align한계(degree)
            }
            List<object> pListData = new List<object>();
            {
                pListData.Add("Contents");
                pListData.Add("0 px");
                pListData.Add("0 px");
                pListData.Add("0 px");
                pListData.Add("0 px");
                pListData.Add("0 deg");
            }
            ////  Depth Table 생성하기
            m_pTableResult = new DataTable();
            DataTableFactory.SetDataTableColumnTitle(ref m_pTableResult, pListTitle);
            DataTableFactory.SetDataTableRowData(ref m_pTableResult, pListData);

            //// View 등록하기
            if (m_pTableResult != null)
            {
                dataGridView_AlignResult.DataSource = m_pTableResult;
                dataGridView_AlignResult.EditMode = DataGridViewEditMode.EditProgrammatically;
            }

        }

        private void InitPatternView(eLabelInspect nLabel)
        {
            CogPMAlignTool pCogToolPM = CommonClass.pCogToolTemplate[m_nType][eYoonCognexType.PMAlign][nLabel.ToString()] as CogPMAlignTool;
            ICogRegion pCogRegion = pCogToolPM.Pattern.TrainRegion;
            switch(nLabel)
            {
                case eLabelInspect.Main:
                    cogDisplay_MainPattern.StaticGraphics.Clear();
                    cogDisplay_MainPattern.InteractiveGraphics.Clear();
                    cogDisplay_MainPattern.Image = CognexFactory.Transform.Crop(pCogToolPM.Pattern.TrainImage, pCogRegion);
                    break;
                case eLabelInspect.Second:
                    cogDisplay_SecondPattern.StaticGraphics.Clear();
                    cogDisplay_SecondPattern.InteractiveGraphics.Clear();
                    cogDisplay_SecondPattern.Image = CognexFactory.Transform.Crop(pCogToolPM.Pattern.TrainImage, pCogRegion);
                    break;
                case eLabelInspect.Third:
                    cogDisplay_ThirdPattern.StaticGraphics.Clear();
                    cogDisplay_ThirdPattern.InteractiveGraphics.Clear();
                    cogDisplay_ThirdPattern.Image = CognexFactory.Transform.Crop(pCogToolPM.Pattern.TrainImage, pCogRegion);
                    break;
                case eLabelInspect.Forth:
                    cogDisplay_ForthPattern.StaticGraphics.Clear();
                    cogDisplay_ForthPattern.InteractiveGraphics.Clear();
                    cogDisplay_ForthPattern.Image = CognexFactory.Transform.Crop(pCogToolPM.Pattern.TrainImage, pCogRegion);
                    break;
                default:
                    break;
            }
        }

        public void OnCheckBoxEnableClick(object sender, EventArgs e)
        {
            CheckBox pBox = (CheckBox)sender;
            ParameterInspectionPatternMatching pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionPatternMatching;
            switch (pBox.Name)
            {
                case "checkBox_IsUsePM":
                    pParam.IsUse = pBox.Checked;
                    break;
                case "checkBox_IsUseMainPattern":
                    pParam.IsUseEachPatterns[eLabelInspect.Main.ToInt()] = pBox.Checked;
                    break;
                case "checkBox_IsUseSecondPattern":
                    pParam.IsUseEachPatterns[eLabelInspect.Second.ToInt()] = pBox.Checked;
                    break;
                case "checkBox_IsUseThirdPattern":
                    pParam.IsUseEachPatterns[eLabelInspect.Third.ToInt()] = pBox.Checked;
                    break;
                case "checkBox_IsUseForthPattern":
                    pParam.IsUseEachPatterns[eLabelInspect.Forth.ToInt()] = pBox.Checked;
                    break;
                case "checkBox_IsUseMultiPatternInspection":
                    pParam.IsUseMultiPatternInspection = pBox.Checked;
                    break;
                case "checkBox_IsCheckAlignOffset":
                    pParam.IsCheckAlign = pBox.Checked;
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
            if (e.InspectType != eTypeInspect.PatternMatching) return;

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
            CommonClass.pCogToolTemplate[m_nType][eYoonCognexType.PMAlign][e.Label.ToString()] = e.CogTool;
            OnInspectionParameterUpdate(sender, e);

            InitPatternView(e.Label);
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
            ParameterInspectionPatternMatching pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionPatternMatching;
            {
                checkBox_IsUsePM.Checked = pParam.IsUse;
                checkBox_IsUseMainPattern.Checked = pParam.IsUseEachPatterns[eLabelInspect.Main.ToInt()];
                checkBox_IsUseSecondPattern.Checked = pParam.IsUseEachPatterns[eLabelInspect.Second.ToInt()];
                checkBox_IsUseThirdPattern.Checked = pParam.IsUseEachPatterns[eLabelInspect.Third.ToInt()];
                checkBox_IsUseForthPattern.Checked = pParam.IsUseEachPatterns[eLabelInspect.Forth.ToInt()];
                checkBox_IsUseMultiPatternInspection.Checked = pParam.IsUseMultiPatternInspection;
                checkBox_IsCheckAlignOffset.Checked = pParam.IsCheckAlign;
                //// Grid 초기화
                propertyGrid_ParamSetting.SelectedObject = pParam;
                //// ComboBox 초기화
                InitComboBoxSource();
            }
        }

        private void button_ProcessPM_Click(object sender, EventArgs e)
        {
            ParameterInspectionPatternMatching pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionPatternMatching;
            AlignResult pResultAlign = new AlignResult();
            YoonVector2D pResultPos = new YoonVector2D();
            double dTheta = 0.0;

            if (!pParam.IsUse) return;

            //// 공용으로 사용하기 전 Parameter 갱신 및 Process 초기화
            OnInspectionParameterUpdate(sender, e);

            if (CommonClass.ProcessPatternMatchAlign(m_pCogImageSourceSelected as CogImage8Grey, ref m_pCogImageResult, ref pResultPos, ref dTheta, ref pResultAlign) > 0)
            {
                //// Inspection Info Download
                OnInspectionParameterDownload(sender, e);
                //// Display Update
                cogDisplay_ProcessView.StaticGraphics.Clear();
                cogDisplay_ProcessView.InteractiveGraphics.Clear();
                cogDisplay_ProcessView.Image = m_pCogImageResult;
                //// Result에 맞게 Display 위에 결과 그리기
                CommonClass.SetPatternOriginRegionToDisplay(cogDisplay_ProcessView, m_nIndex, m_nType);
                CommonClass.SetResultRegionToDisplay(cogDisplay_ProcessView, m_nIndex, m_nType);
                //// Align 결과 Grid에 반영
                try
                {
                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, 0, 1, string.Format("{0:0.###} px", pResultPos.X));
                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, 0, 2, string.Format("{0:0.###} px", pResultPos.Y));
                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, 0, 1, string.Format("{0:0.###} px", pResultAlign.ResultX));
                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, 0, 2, string.Format("{0:0.###} px", pResultAlign.ResultY));
                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, 0, 3, string.Format("{0:0.###} deg", pResultAlign.ResultT));
                    dataGridView_AlignResult.Invalidate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                //// Result Image를 다른 Tab으로 넘기기
                OnUpdateResultImageEvent(this, new CogImageArgs(m_nIndex, m_nType, m_pCogImageResult));
            }
        }

        private void button_SetOrigin_Click(object sender, EventArgs e)
        {
            //// Origin 잡기전에 먼저 물어보기
            DialogResult result = MessageBox.Show("Set These Pattern To Origin?", "", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                //// 공용으로 사용하기 전 Parameter 갱신 및 Process 초기화
                OnInspectionParameterUpdate(sender, e);
                AlignResult pResultAlign = null;
                YoonVector2D pResultPos = null;
                double dTheta = 0.0;
                int nInspPattern = CommonClass.ProcessPatternMatchAlign(m_pCogImageSourceSelected as CogImage8Grey, ref m_pCogImageResult, ref pResultPos, ref dTheta, ref pResultAlign);
                if (nInspPattern < 0) return;
                //// Origin Setting 시작
                if(CommonClass.ProcessPatternMatchOrigin(nInspPattern))
                {
                    //// Inspection Info Download
                    OnInspectionParameterDownload(sender, e);
                    //// Display Update
                    cogDisplay_ProcessView.StaticGraphics.Clear();
                    cogDisplay_ProcessView.InteractiveGraphics.Clear();
                    cogDisplay_ProcessView.Image = m_pCogImageResult;
                    //// Display Origin 표시하기
                    CommonClass.SetPatternOriginRegionToDisplay(cogDisplay_ProcessView, m_nIndex, m_nType);
                    //// Result Image를 다른 Tab으로 넘기기
                    OnUpdateResultImageEvent(this, new CogImageArgs(m_nIndex, m_nType, m_pCogImageResult));
                }
            }
        }

        private void button_SetSourceImage_Click(object sender, EventArgs e)
        {
            ParameterInspectionPatternMatching pParam = CommonClass.pParamTemplate[m_nType].Parameter as ParameterInspectionPatternMatching;
            if (m_pCogImageOrigin == null || m_pCogImagePreprocessing == null)
                return;

            ////  현재 ComboBox 선택 Value 취득
            string strSourceSelected = (string)comboBox_SelectSourceImage.SelectedItem;
            switch (strSourceSelected)
            {
                case "Origin":
                    m_pCogImageSourceSelected = m_pCogImageOrigin.CopyBase(CogImageCopyModeConstants.CopyPixels);
                    pParam.SelectedSourceLevel = eLevelImageSelection.CurrentProcessing;
                    break;
                case "CurrentProcessing":
                    m_pCogImageSourceSelected = m_pCogImagePreprocessing.CopyBase(CogImageCopyModeConstants.CopyPixels);
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
            cogDisplay_ProcessView.StaticGraphics.Clear();
            cogDisplay_ProcessView.InteractiveGraphics.Clear();
            cogDisplay_ProcessView.Image = m_pCogImageSourceSelected;

            //// 변경 즉시 반영 (적용지연 에러 발생에 대한 대처사항)
            CommonClass.pParamTemplate[m_nType].Parameter = pParam;
            OnInspectionParameterUpdate(sender, e);
        }

        private void button_UpdateSetting_Click(object sender, EventArgs e)
        {
            OnInspectionParameterUpdate(sender, e);
            OnInspectionParameterDownload(sender, e);
        }

        private void button_SettingMainPattern_Click(object sender, EventArgs e)
        {
            button_SettingMainPattern.Enabled = false;

            ////  Form 생성하기
            eLabelInspect nLabel = eLabelInspect.Main;
            Form_CogPatternAlign pCogForm = new Form_CogPatternAlign();
            pCogForm.CogImageSource = m_pCogImagePreprocessing;
            pCogForm.CogToolLabel = nLabel;
            pCogForm.CogTool = CommonClass.pCogToolTemplate[m_nType][eYoonCognexType.PMAlign][nLabel.ToString()] as CogPMAlignTool;
            pCogForm.OnUpdateCogToolEvent += OnCognexToolUpdate;
            pCogForm.Show();

            button_SettingMainPattern.Enabled = true;
        }

        private void button_SettingSecondPattern_Click(object sender, EventArgs e)
        {
            button_SettingSecondPattern.Enabled = false;

            ////  Form Initialize
            eLabelInspect nLabel = eLabelInspect.Second;
            Form_CogPatternAlign pCogForm = new Form_CogPatternAlign();
            pCogForm.CogImageSource = m_pCogImagePreprocessing;
            pCogForm.CogToolLabel = nLabel;
            pCogForm.CogTool = CommonClass.pCogToolTemplate[m_nType][eYoonCognexType.PMAlign][nLabel.ToString()] as CogPMAlignTool;
            pCogForm.OnUpdateCogToolEvent += OnCognexToolUpdate;
            pCogForm.Show();

            button_SettingSecondPattern.Enabled = true;
        }

        private void button_SettingThirdPattern_Click(object sender, EventArgs e)
        {
            button_SettingThirdPattern.Enabled = false;

            ////  Form Initialize
            eLabelInspect nLabel = eLabelInspect.Third;
            Form_CogPatternAlign pCogForm = new Form_CogPatternAlign();
            pCogForm.CogImageSource = m_pCogImagePreprocessing;
            pCogForm.CogToolLabel = nLabel;
            pCogForm.CogTool = CommonClass.pCogToolTemplate[m_nType][eYoonCognexType.PMAlign][nLabel.ToString()] as CogPMAlignTool;
            pCogForm.OnUpdateCogToolEvent += OnCognexToolUpdate;
            pCogForm.Show();

            button_SettingThirdPattern.Enabled = true;
        }

        private void button_SettingForthPattern_Click(object sender, EventArgs e)
        {
            button_SettingForthPattern.Enabled = false;

            ////  Form Initialize
            eLabelInspect nLabel = eLabelInspect.Forth;
            Form_CogPatternAlign pCogForm = new Form_CogPatternAlign();
            pCogForm.CogImageSource = m_pCogImagePreprocessing;
            pCogForm.CogToolLabel = nLabel;
            pCogForm.CogTool = CommonClass.pCogToolTemplate[m_nType][eYoonCognexType.PMAlign][nLabel.ToString()] as CogPMAlignTool;
            pCogForm.OnUpdateCogToolEvent += OnCognexToolUpdate;
            pCogForm.Show();

            button_SettingForthPattern.Enabled = true;
        }
    }
}
