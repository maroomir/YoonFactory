using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoonFactory.Log;
using Cognex.VisionPro;
using System.Threading;
using System.Drawing.Text;
using YoonFactory.Cognex;
using YoonFactory.Align;
using YoonFactory.Windows;
using YoonFactory.Param;
using YoonFactory.Cognex.Result;
using YoonFactory.Cognex.Tool;
using YoonFactory;

namespace YoonSample.CognexInspector
{
    public partial class FormMain : Form
    {
        private CognexImage m_pCogRGBImageOrigin = new CognexImage();
        private DataTable m_pTableResult = null;

        public event PassImageCallback OnUpdatePreviewImageEvent;
        public event PassRequestCallback OnRequestParameterUpdateEvent;
        public event PassRequestCallback OnRequestParameterDownloadEvent;

        public FormMain()
        {
            InitializeComponent();

            CommonClass.pCLM.RootDirectory = CommonClass.strCurrentWorkingDirectory;
            CommonClass.pDLM.RootDirectory = CommonClass.strCurrentWorkingDirectory;
            CommonClass.pDLM.OnProcessLogEvent += OnLogDisplay;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            PrintInspectionSettingMessage(eYoonStatus.Info, "Grid Initialize");
            {
                if (!CommonClass.pCogToolTemplate.LoadTemplate())
                    return;

                if (CommonClass.pParamTemplate.Count == 0 ||
                   CommonClass.pCogToolTemplate.Count == 0)
                {
                    PrintInspectionSettingMessage(eYoonStatus.Conform, "Initialize Config");
                    Init_Template();
                }
                PrintInspectionSettingMessage(eYoonStatus.Info, "Binding Info");
                {
                    bindingSource_Inspection.ResetBindings(false);
                    bindingSource_Inspection.DataSource = CommonClass.pCogToolTemplate.ToList();
                    dataGridView_SelectInspection.DataSource = bindingSource_Inspection;
                    dataGridView_SelectInspection.Columns[0].HeaderText = "Type";
                    dataGridView_SelectInspection.Columns[0].Width = 100;
                    dataGridView_SelectInspection.Columns[1].HeaderText = "Name";
                    dataGridView_SelectInspection.Columns[1].Width = 150;
                    dataGridView_SelectInspection.EditMode = DataGridViewEditMode.EditProgrammatically;
                    dataGridView_SelectInspection.Invalidate();
                }

                Init_ResultTable();
            }

            PrintInspectionSettingMessage(eYoonStatus.Info, string.Format("Init Model No : {0}, Model Name : {1}", CommonClass.pCogToolTemplate.No, CommonClass.pCogToolTemplate.Name));
            {
                Init_View();
            }

            PrintInspectionSettingMessage(eYoonStatus.Info, "Event Subscription");
            {
                //
            }

            PrintInspectionSettingMessage(eYoonStatus.Info, "Load Inspection Setting Form Complete");
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Save All Recipe Setting?", "", MessageBoxButtons.YesNo);
            {
                PrintInspectionSettingMessage(eYoonStatus.Info, "Save All Recipe");
                CommonClass.pParamTemplate.SaveTemplate();
                CommonClass.pCogToolTemplate.SaveTemplate();
                CommonClass.pCogResultTemplate.SaveTemplate();
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            PrintInspectionSettingMessage(eYoonStatus.Info, "Clear Values");
            {
                //
            }

            PrintInspectionSettingMessage(eYoonStatus.Info, "Clear Tab Control");
            {
                int nCountPage = tabControl_Setting.TabPages.Count;
                for (int iPage = nCountPage - 1; iPage >= 0; iPage--)   // 뒷페이지부터 없애야함 (또는 foreach 사용)
                {
                    TabPage pTabPage = tabControl_Setting.TabPages[iPage];
                    //// Page 내 Component 삭제
                    int nCountChild = pTabPage.Controls.Count;
                    for (int iChild = 0; iChild < nCountChild; iChild++)
                    {
                        if (pTabPage.Controls[iChild] is IInspectionTab)
                        {
                            IInspectionTab pTabInspection = pTabPage.Controls[iChild] as IInspectionTab;
                            OnUpdatePreviewImageEvent -= pTabInspection.OnCognexImageDownload;
                            pTabInspection.OnUpdateResultImageEvent -= OnResultImageUpdate;
                        }
                        pTabPage.Controls[iChild].Dispose();
                    }
                    //// Page 삭제
                    tabControl_Setting.TabPages.Remove(pTabPage);
                }

                if (tabControl_Setting != null) tabControl_Setting.TabPages.Clear();
            }
            //// 종료 전 Memory 해제
            GC.Collect();
        }

        private void PrintInspectionSettingMessage(eYoonStatus nType, string strMessage)
        {
            try
            {
                CommonClass.pCLM.Write("[Inspection Setting] " + strMessage);
                CommonClass.pDLM.Write(nType, "[Inspection Setting] " + strMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public delegate void OnLogDisplayCallback(object sender, LogArgs e);
        public void OnLogDisplay(object sender, LogArgs e)
        {
            if (textBox_InspectionMessage.InvokeRequired)
            {
                textBox_InspectionMessage.BeginInvoke(new OnLogDisplayCallback(OnLogDisplay), sender, e);
            }
            else
            {
                if (e is LogDisplayerArgs pEvent)
                {
                    textBox_InspectionMessage.BackColor = pEvent.BackColor;
                    textBox_InspectionMessage.Text += pEvent.Message + Environment.NewLine;
                }
            }
        }

        public void OnResultImageUpdate(object sender, CogImageArgs e)
        {
            if (CommonClass.pCogResultTemplate[e.InspectType].GetLastResultImage() == null)
            {
                PrintInspectionSettingMessage(eYoonStatus.Error, "Abnormal Result Error");
                return;
            }
            ////  View Image Input
            int nOrderNext = CommonClass.pParamTemplate.IndexOf(e.InspectType) + 1;
            if (nOrderNext >= CommonClass.pParamTemplate.Count) return;
            eTypeInspect nOrderNextKey = CommonClass.pParamTemplate.KeyOf(nOrderNext);
            OnUpdatePreviewImageEvent(sender, new CogImageArgs(CommonClass.pCogToolTemplate[nOrderNextKey].No, nOrderNextKey, e.Image));
        }

        private void Init_View()
        {
            PrintInspectionSettingMessage(eYoonStatus.Info, "Tab Control Init");
            {
                int nCountPage = tabControl_Setting.TabPages.Count;
                for (int iPage = nCountPage - 1; iPage >= 0; iPage--)   // 뒷페이지부터 없애야함 (또는 foreach 사용)
                {
                    TabPage pTabPage = tabControl_Setting.TabPages[iPage];
                    //// Page 내 Component 삭제
                    int nCountChild = pTabPage.Controls.Count;
                    for (int iChild = 0; iChild < nCountChild; iChild++)
                    {
                        if (pTabPage.Controls[iChild] is IInspectionTab)
                        {
                            IInspectionTab pTabInspection = pTabPage.Controls[iChild] as IInspectionTab;
                            OnUpdatePreviewImageEvent -= pTabInspection.OnCognexImageDownload;
                            pTabInspection.OnUpdateResultImageEvent -= OnResultImageUpdate;
                        }
                        pTabPage.Controls[iChild].Dispose();
                    }
                    //// Page 삭제
                    tabControl_Setting.TabPages.Remove(pTabPage);
                }

                if (tabControl_Setting != null) tabControl_Setting.TabPages.Clear();
            }

            PrintInspectionSettingMessage(eYoonStatus.Info, "Tab Control Reset");
            {
                for (int iPage = 0; iPage < CommonClass.pCogToolTemplate.Count; iPage++)
                {
                    TabPage pTabPage = new TabPage(CommonClass.pCogToolTemplate[iPage].ToString());

                    switch (CommonClass.pCogToolTemplate.KeyOf(iPage))
                    {
                        case eTypeInspect.Preprocessing:
                            TabPreprocessing pTabPreprocessing = new TabPreprocessing(iPage);
                            {
                                //// Event 등록 및 구독
                                OnUpdatePreviewImageEvent += pTabPreprocessing.OnCognexImageDownload;
                                OnRequestParameterUpdateEvent += pTabPreprocessing.OnInspectionParameterUpdate;
                                OnRequestParameterDownloadEvent += pTabPreprocessing.OnInspectionParameterDownload;
                                pTabPreprocessing.OnUpdateResultImageEvent += OnResultImageUpdate;
                                //// View 설정
                                pTabPreprocessing.TopLevel = false;
                                pTabPage.Controls.Add(pTabPreprocessing);
                                pTabPreprocessing.Left = 0;
                                pTabPreprocessing.Top = 0;
                                try
                                {
                                    pTabPreprocessing.Show();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }
                            }
                            break;
                        case eTypeInspect.PatternMatching:
                            TabPatternMatching pTabPM = new TabPatternMatching(iPage);
                            {
                                //// Event 등록
                                OnUpdatePreviewImageEvent += pTabPM.OnCognexImageDownload;
                                OnRequestParameterUpdateEvent += pTabPM.OnInspectionParameterUpdate;
                                OnRequestParameterDownloadEvent += pTabPM.OnInspectionParameterDownload;
                                pTabPM.OnUpdateResultImageEvent += OnResultImageUpdate;
                                //// View 설정
                                pTabPM.TopLevel = false;
                                pTabPage.Controls.Add(pTabPM);
                                pTabPM.Left = 0;
                                pTabPM.Top = 0;
                                try
                                {
                                    pTabPM.Show();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }
                            }
                            break;
                        case eTypeInspect.ObjectExtract:
                            TabObjectExtract pTabExtract = new TabObjectExtract(iPage);
                            {
                                //// Event 등록
                                OnUpdatePreviewImageEvent += pTabExtract.OnCognexImageDownload;
                                OnRequestParameterUpdateEvent += pTabExtract.OnInspectionParameterUpdate;
                                OnRequestParameterDownloadEvent += pTabExtract.OnInspectionParameterDownload;
                                pTabExtract.OnUpdateResultImageEvent += OnResultImageUpdate;
                                //// View 설정
                                pTabExtract.TopLevel = false;
                                pTabPage.Controls.Add(pTabExtract);
                                pTabExtract.Left = 0;
                                pTabExtract.Top = 0;
                                try
                                {
                                    pTabExtract.Show();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }
                            }
                            break;
                        case eTypeInspect.Combine:
                            TabCombine pTabCombine = new TabCombine(iPage);
                            {
                                //// Event 등록
                                OnUpdatePreviewImageEvent += pTabCombine.OnCognexImageDownload;
                                OnRequestParameterUpdateEvent += pTabCombine.OnInspectionParameterUpdate;
                                OnRequestParameterDownloadEvent += pTabCombine.OnInspectionParameterDownload;
                                pTabCombine.OnUpdateResultImageEvent += OnResultImageUpdate;
                                //// View 설정
                                pTabCombine.TopLevel = false;
                                pTabPage.Controls.Add(pTabCombine);
                                pTabCombine.Left = 0;
                                pTabCombine.Top = 0;
                                try
                                {
                                    pTabCombine.Show();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    tabControl_Setting.TabPages.Add(pTabPage);
                    pTabPage.Show();
                }
            }
        }

        private void Init_Template()
        {
            CommonClass.pParamTemplate.Clear();
            CommonClass.pCogToolTemplate.Clear();

            PrintInspectionSettingMessage(eYoonStatus.Info, "Parameter Template Initialize");
            {
                CommonClass.pParamTemplate.No = 1;
                CommonClass.pParamTemplate.Name = "Param";
                CommonClass.pParamTemplate[eTypeInspect.Preprocessing] = new YoonParameter(new ParameterInspectionPreprocessing(), typeof(ParameterInspectionPreprocessing));
                CommonClass.pParamTemplate[eTypeInspect.PatternMatching] = new YoonParameter(new ParameterInspectionPatternMatching(), typeof(ParameterInspectionPatternMatching));
                CommonClass.pParamTemplate[eTypeInspect.ObjectExtract] = new YoonParameter(new ParameterInspectionObjectExtract(), typeof(ParameterInspectionObjectExtract));
                CommonClass.pParamTemplate[eTypeInspect.Combine] = new YoonParameter(new ParameterInspectionCombine(), typeof(ParameterInspectionCombine));
            }

            PrintInspectionSettingMessage(eYoonStatus.Info, "Cognex Tool Template Initialize");
            {
                CommonClass.pCogToolTemplate.No = 1;
                CommonClass.pCogToolTemplate.Name = "Tool";
                CommonClass.pCogToolTemplate[eTypeInspect.Preprocessing] = new ToolTemplate(0, "Preprocessing");
                CommonClass.pCogToolTemplate[eTypeInspect.PatternMatching] = new ToolTemplate(1, "PatternMatching");
                CommonClass.pCogToolTemplate[eTypeInspect.ObjectExtract] = new ToolTemplate(2, "ObjectExtract");
                CommonClass.pCogToolTemplate[eTypeInspect.Combine] = new ToolTemplate(3, "Combine");
            }

            PrintInspectionSettingMessage(eYoonStatus.Info, "Cognex Result Template Initialize");
            {
                CommonClass.pCogResultTemplate.No = 1;
                CommonClass.pCogResultTemplate.Name = "Tool";
                CommonClass.pCogResultTemplate[eTypeInspect.Preprocessing] = new ResultTemplate(0, "Preprocessing");
                CommonClass.pCogResultTemplate[eTypeInspect.PatternMatching] = new ResultTemplate(1, "PatternMatching");
                CommonClass.pCogResultTemplate[eTypeInspect.ObjectExtract] = new ResultTemplate(2, "ObjectExtract");
                CommonClass.pCogResultTemplate[eTypeInspect.Combine] = new ResultTemplate(3, "Combine");
            }
        }

        private void Init_ResultTable()
        {
            int nRowCount = 0;
            //// Title 만들기
            List<object> pListTitle = new List<object>();
            {
                pListTitle.Add("Title");
                nRowCount = 1;
                foreach (eTypeInspect nType in CommonClass.pCogToolTemplate.Keys)
                {
                    pListTitle.Add(CommonClass.pCogToolTemplate[nType].ToString());  // OK or NG
                    nRowCount++;
                    switch (nType)
                    {
                        case eTypeInspect.Preprocessing:
                            break;
                        case eTypeInspect.PatternMatching:
                            pListTitle.Add("Score1");  // i++ : 100%
                            pListTitle.Add("Score2");  // i++ : 100%
                            pListTitle.Add("Score3");  // i++ : 100%
                            pListTitle.Add("Score4");  // i++ : 100%
                            pListTitle.Add("PosX");   // i++ : 0.00 px
                            pListTitle.Add("PosY");   // i++ : 0.00 px
                            pListTitle.Add("AlignX"); // i++ : 0.00 px
                            pListTitle.Add("AlignY"); // i++ : 0.00 px
                            pListTitle.Add("AlignT"); // i++ : 0.00 deg
                            nRowCount += 9;
                            break;
                        case eTypeInspect.ObjectExtract:
                            pListTitle.Add("BlobCount"); // i++ : 1000 ea
                            nRowCount += 1;
                            break;
                        case eTypeInspect.Combine:
                            break;
                        default:
                            break;
                    }
                }
            }
            //// Data 생성하기
            List<object> pListData = new List<object>();
            {
                pListData.Add("Contetns");
                foreach (eTypeInspect nType in CommonClass.pCogToolTemplate.Keys)
                {
                    switch (nType)
                    {
                        case eTypeInspect.Preprocessing:
                            pListData.Add("NG");  // OK or NG
                            break;
                        case eTypeInspect.PatternMatching:
                            pListData.Add("NG");  // OK or NG
                            pListData.Add("- %");  // i++ : 100%
                            pListData.Add("- %");  // i++ : 100%
                            pListData.Add("- %");  // i++ : 100%
                            pListData.Add("- %");  // i++ : 100%
                            pListData.Add("- px"); // i++ : 0.00 px
                            pListData.Add("- px"); // i++ : 0.00 px
                            pListData.Add("- px"); // i++ : 0.00 px
                            pListData.Add("- px"); // i++ : 0.00 px
                            pListData.Add("- deg"); // i++ : 0.00 deg
                            break;
                        case eTypeInspect.ObjectExtract:
                            pListData.Add("NG");  // OK or NG
                            pListData.Add("- ea"); // i++ : 1000 ea
                            break;
                        case eTypeInspect.Combine:
                            pListData.Add("NG");  // OK or NG
                            break;
                        default:
                            break;
                    }
                }
            }
            ////  Result Table 생성하기
            DataTable pTableResult = new DataTable();
            List<string> pListTitleNo = new List<string>();
            for (int i = 0; i < nRowCount; i++)
                pListTitleNo.Add(i.ToString());
            DataTableFactory.SetDataTableColumnTitle(ref pTableResult, pListTitleNo);
            DataTableFactory.SetDataTableRowData(ref pTableResult, pListTitle);
            DataTableFactory.SetDataTableRowData(ref pTableResult, pListData);
            m_pTableResult = DataTableFactory.ConvertCrossTable(pTableResult);

            if (m_pTableResult != null)
            {
                dataGridView_Result.DataSource = m_pTableResult;
                dataGridView_Result.EditMode = DataGridViewEditMode.EditProgrammatically;
                dataGridView_Result.AllowUserToAddRows = false;
            }
        }

        private void button_EditInspection_Click(object sender, EventArgs e)
        {
            //
        }

        private void button_ImageLoad_Click(object sender, EventArgs e)
        {
            PrintInspectionSettingMessage(eYoonStatus.Info, "Image Load");
            {
                //// Image Load
                using (OpenFileDialog pDlg = new OpenFileDialog())
                {
                    pDlg.Filter = "bmp | *.bmp";
                    if (pDlg.ShowDialog() == DialogResult.OK)
                    {
                        string strFilePath = pDlg.FileName;
                        m_pCogRGBImageOrigin.LoadImage(strFilePath);
                        //// 각 Tab에 이미지 분배하기
                        foreach (eTypeInspect nType in CommonClass.pParamTemplate.Keys)
                        {
                            OnUpdatePreviewImageEvent(sender, new CogImageArgs(CommonClass.pCogToolTemplate[nType].No, nType, m_pCogRGBImageOrigin));
                        }
                    }
                }
            }
        }

        private void button_ImageSave_Click(object sender, EventArgs e)
        {
            PrintInspectionSettingMessage(eYoonStatus.Info, "Image Save");
            {
                //// Image Save 하기
                using (SaveFileDialog pDlg = new SaveFileDialog())
                {
                    pDlg.Filter = "bmp | *.bmp";
                    if (pDlg.ShowDialog() == DialogResult.OK)
                    {
                        string strFilePath = pDlg.FileName;
                        m_pCogRGBImageOrigin.SaveImage(strFilePath);
                    }
                }
            }
        }

        private void button_ProcessInspection_Click(object sender, EventArgs e)
        {
            //// 검사전 모든 항목에 대해 최신화 요청
            OnRequestParameterUpdateEvent(sender, e);

            if (ProcessInspection())
            {
                label_Result.Text = "OK";
                label_Result.ForeColor = Color.Lime;
            }
            else
            {
                label_Result.Text = "NG";
                label_Result.ForeColor = Color.Red;
            }
        }

        private bool ProcessInspection()
        {
            if (m_pCogRGBImageOrigin == null)
                return false;

            int nStepInit = 0, nRowResult = 0;
            string strMessage = string.Empty;
            bool bResultInsp = true;
            CognexImage pImageProcessing = m_pCogRGBImageOrigin.Clone() as CognexImage;

            PrintInspectionSettingMessage(eYoonStatus.Info, string.Format("Inspection {0} : Process Start", nStepInit++));
            for (int iInsp = 0; iInsp < CommonClass.pCogToolTemplate.Count; iInsp++)
            {
                //// 초기화
                eTypeInspect pKey = CommonClass.pCogToolTemplate.KeyOf(iInsp);
                CognexImage pImageTemp = null;
                AlignResult pResultAlign = null;
                YoonVector2D pResultPos = null;
                double dResultTheta = 0.0;
                //// Type별 영상처리 및 출력방법 분화
                switch (pKey)
                {
                    case eTypeInspect.Preprocessing:
                        PrintInspectionSettingMessage(eYoonStatus.Info, string.Format("Inspection {0} : Preprocess", nStepInit++));
                        ////  영상처리
                        if (!CommonClass.ProcessPreprocessing(pImageProcessing, ref pImageTemp))
                        {
                            strMessage = string.Format("Inspection {0} : Preprocessing Failure", nStepInit);
                            CommonClass.pCLM.Write(strMessage);
                            CommonClass.pDLM.Write(eYoonStatus.Error, strMessage);
                            DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "NG");
                            bResultInsp = false;
                            continue;
                        }
                        ////  Grid에 표시
                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "OK");
                        break;
                    case eTypeInspect.PatternMatching:
                        PrintInspectionSettingMessage(eYoonStatus.Info, string.Format("Inspection {0} : Pattern Matching", nStepInit++));
                        ParameterInspectionPatternMatching pParamPM = CommonClass.pParamTemplate[pKey].Parameter as ParameterInspectionPatternMatching;
                        {
                            ////  Source Image 정의하기
                            CognexImage pImageResult = new CognexImage();
                            CognexImage pImagePMSource = null;
                            switch (pParamPM.SelectedSourceLevel)
                            {
                                case eLevelImageSelection.CurrentProcessing:
                                    pImagePMSource = pImageProcessing;
                                    break;
                                case eLevelImageSelection.Custom:
                                    pImagePMSource = CommonClass.GetResultImage(pParamPM.SelectedSourceNo, pParamPM.SelectedSourceType);
                                    break;
                                default:
                                    ////  Grid에 표시
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "NG");
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- px");
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- px");
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- px");
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- px");
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- deg");
                                    continue;
                            }
                            ////  영상처리
                            if (CommonClass.ProcessPatternMatchAlign(pImagePMSource, ref pImageResult, ref pResultPos, ref dResultTheta, ref pResultAlign) <= 0)
                            {
                                strMessage = string.Format("Inspection {0} : Pattern Match Failure", nStepInit);
                                CommonClass.pCLM.Write(strMessage);
                                CommonClass.pDLM.Write(eYoonStatus.Error, strMessage);
                                DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "NG");
                                bResultInsp = false;
                                continue;
                            }
                            //// Align Result 계산
                            if (pParamPM.IsCheckAlign)
                            {
                                if (pParamPM.IsAlignCheckWithOR)
                                {
                                    if (Math.Abs(pResultAlign.ResultX) > pParamPM.OffsetX || Math.Abs(pResultAlign.ResultY) > pParamPM.OffsetY || Math.Abs(pResultAlign.ResultT) > pParamPM.OffsetT)
                                    {
                                        ////  Grid에 표시
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "NG");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- px");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- px");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- px");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- px");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- deg");
                                        bResultInsp = false;
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (Math.Abs(pResultAlign.ResultX) > pParamPM.OffsetX && Math.Abs(pResultAlign.ResultY) > pParamPM.OffsetY && Math.Abs(pResultAlign.ResultT) > pParamPM.OffsetT)
                                    {
                                        ////  Grid에 표시
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "NG");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- px");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- px");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- px");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- px");
                                        DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- deg");
                                        bResultInsp = false;
                                        continue;
                                    }
                                }
                            }
                            ////  Grid에 표시
                            try
                            {
                                DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "OK");

                                if (pParamPM.IsUseEachPatterns[eLabelInspect.Main.ToInt()])
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, string.Format("{0:0.###} %", (CommonClass.pCogResultTemplate[pKey][eYoonCognexType.PMAlign][eLabelInspect.Main.ToString()].TotalScore)));
                                else
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                if (pParamPM.IsUseEachPatterns[eLabelInspect.Second.ToInt()])
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, string.Format("{0:0.###} %", (CommonClass.pCogResultTemplate[pKey][eYoonCognexType.PMAlign][eLabelInspect.Second.ToString()].TotalScore)));
                                else
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                if (pParamPM.IsUseEachPatterns[eLabelInspect.Third.ToInt()])
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, string.Format("{0:0.###} %", (CommonClass.pCogResultTemplate[pKey][eYoonCognexType.PMAlign][eLabelInspect.Third.ToString()].TotalScore)));
                                else
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");
                                if (pParamPM.IsUseEachPatterns[eLabelInspect.Forth.ToInt()])
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, string.Format("{0:0.###} %", (CommonClass.pCogResultTemplate[pKey][eYoonCognexType.PMAlign][eLabelInspect.Forth.ToString()].TotalScore)));
                                else
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- %");

                                DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, string.Format("{0:0.###} px", pResultPos.X));
                                DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, string.Format("{0:0.###} px", pResultPos.Y));
                                DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, string.Format("{0:0.###} px", pResultAlign.ResultX));
                                DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, string.Format("{0:0.###} px", pResultAlign.ResultY));
                                DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, string.Format("{0:0.###} deg", pResultAlign.ResultT));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                                return false;
                            }
                            ////  Temp로 Result Image 복사
                            pImageTemp = pImageResult;
                        }
                        break;
                    case eTypeInspect.ObjectExtract:
                        PrintInspectionSettingMessage(eYoonStatus.Info, string.Format("Inspection {0} : Object Extract", nStepInit++));
                        ParameterInspectionObjectExtract pParamObjectExtract = CommonClass.pParamTemplate[pKey].Parameter as ParameterInspectionObjectExtract;
                        {
                            //// Source Image 정의하기
                            CognexImage pImageBlobSource = null;
                            CognexImage pImageColorSource = null;
                            switch (pParamObjectExtract.SelectedBlobImageLevel)
                            {
                                case eLevelImageSelection.CurrentProcessing:
                                    pImageBlobSource = pImageProcessing;
                                    break;
                                case eLevelImageSelection.Custom:
                                    pImageBlobSource = CommonClass.GetResultImage(pParamObjectExtract.SelectedBlobImageNo, pParamObjectExtract.SelectedBlobImageType);
                                    break;
                                default:
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "NG");
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- ea");
                                    continue;
                            }
                            switch (pParamObjectExtract.SelectedColorSegmentImageLevel)
                            {
                                case eLevelImageSelection.Origin:
                                    pImageColorSource = m_pCogRGBImageOrigin;
                                    break;
                                case eLevelImageSelection.CurrentProcessing:
                                    pImageColorSource = pImageProcessing;
                                    break;
                                case eLevelImageSelection.Custom:
                                    pImageColorSource = CommonClass.GetResultImage(pParamObjectExtract.SelectedColorSegmentImageNo, pParamObjectExtract.SelectedColorSegmentImageType);
                                    break;
                                default:
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "NG");
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- ea");
                                    continue;
                            }
                            //// 영상 처리
                            if (!CommonClass.ProcessObjectExtract(pImageBlobSource, pImageColorSource, ref pImageTemp))
                            {
                                strMessage = string.Format("Inspection {0} : Object Extract Failure", nStepInit);
                                CommonClass.pCLM.Write(strMessage);
                                CommonClass.pDLM.Write(eYoonStatus.Error, strMessage);
                                DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "NG");
                                DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- ea");
                                bResultInsp = false;
                                continue;
                            }
                            ////  Grid에 표시
                            try
                            {
                                DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "OK");
                                if (pParamObjectExtract.IsUseBlob)
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, string.Format("{0} ea", (CommonClass.pCogResultTemplate[pKey][eYoonCognexType.Blob][string.Empty].ObjectList.Count)));
                                else
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "- ea");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                                return false;
                            }
                        }
                        break;
                    case eTypeInspect.Combine:
                        ParameterInspectionCombine pParamCombine = CommonClass.pParamTemplate[pKey].Parameter as ParameterInspectionCombine;
                        {
                            //// Source Image 정의하기
                            CognexImage pImageSourceCombine = null;
                            CognexImage pImageObjectCombine = null;
                            switch (pParamCombine.SelectedSourceLevel)
                            {
                                case eLevelImageSelection.Origin:
                                    pImageSourceCombine = m_pCogRGBImageOrigin;
                                    break;
                                case eLevelImageSelection.CurrentProcessing:
                                    pImageSourceCombine = pImageProcessing;
                                    break;
                                case eLevelImageSelection.Custom:
                                    pImageSourceCombine = CommonClass.GetResultImage(pParamCombine.SelectedSourceNo, pParamCombine.SelectedSourceType);
                                    break;
                                default:
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "NG");
                                    continue;
                            }
                            switch (pParamCombine.SelectedObjectLevel)
                            {
                                case eLevelImageSelection.Origin:
                                    pImageObjectCombine = m_pCogRGBImageOrigin;
                                    break;
                                case eLevelImageSelection.CurrentProcessing:
                                    pImageObjectCombine = pImageProcessing;
                                    break;
                                case eLevelImageSelection.Custom:
                                    pImageObjectCombine = CommonClass.GetResultImage(pParamCombine.SelectedObjectNo, pParamCombine.SelectedObjectType);
                                    break;
                                default:
                                    DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "NG");
                                    continue;
                            }
                            //// 영상 처리
                            if (!CommonClass.ProcessImageCombine(pImageSourceCombine, pImageObjectCombine, ref pImageTemp))
                            {
                                strMessage = string.Format("Inspection {0} : Combine Failure", nStepInit);
                                CommonClass.pCLM.Write(strMessage);
                                CommonClass.pDLM.Write(eYoonStatus.Error, strMessage);
                                DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "NG");
                                bResultInsp = false;
                                continue;
                            }
                            ////  Grid에 표시
                            DataTableFactory.ChangeDataTableData(ref m_pTableResult, nRowResult++, 1, "OK");
                        }
                        break;
                    default:
                        continue;
                }
                if (pImageTemp != null)
                    pImageProcessing = pImageTemp.Clone() as CognexImage;
            }

            return bResultInsp;
        }
    }
}
