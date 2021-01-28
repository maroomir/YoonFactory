using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using YoonFactory.Cognex;
using YoonFactory.Log;
using YoonFactory.Windows.Log;
using YoonFactory.Param;

namespace YoonSample.CognexInspector
{
    public delegate void PassImageCallback(object sender, CogImageArgs e);
    public class CogImageArgs : EventArgs
    {
        public int InspectNo;
        public eTypeInspect InspectType;
        public ICogImage CogImage;

        public CogImageArgs(ICogImage pImage)
        {
            InspectNo = 0;
            InspectType = eTypeInspect.None;
            CogImage = pImage;
        }

        public CogImageArgs(eTypeInspect nType, ICogImage pImage)
        {
            InspectNo = 0;
            InspectType = nType;
            CogImage = pImage;
        }

        public CogImageArgs(int nNo, eTypeInspect nType, ICogImage pImage)
        {
            InspectNo = nNo;
            InspectType = nType;
            CogImage = pImage;
        }
    }

    public delegate void PassCogToolCallback(object sender, CogToolArgs e);
    public class CogToolArgs : EventArgs
    {
        public eYoonCognexType ToolType;
        public eLabelInspect Label;
        public ICogTool CogTool;
        public ICogImage ContainImage;

        public CogToolArgs(eYoonCognexType nType, eLabelInspect nLabel, ICogTool pTool, ICogImage pImage)
        {
            ToolType = nType;
            Label = nLabel;
            CogTool = pTool;
            if (pImage != null)
                ContainImage = pImage.CopyBase(CogImageCopyModeConstants.CopyPixels);
        }
    }

    public static class CommonClass
    {
        public const int DEFULAT_IMAGE_WIDTH = 640;
        public const int DEFULAT_IMAGE_HEIGHT = 480;
        public const int MAX_SOURCE_NUM = 4;
        public const int MAX_PATTERN_NUM = 4;

        public static ParameterConfig pConfig = new ParameterConfig();
        public static YoonTemplate<eTypeInspect> pParamTemplate = new YoonTemplate<eTypeInspect>();
        public static Template<eTypeInspect, ToolTemplate> pCogToolTemplate = new Template<eTypeInspect, ToolTemplate>("Tools");
        public static Template<eTypeInspect, ResultTemplate> pCogResultTemplate = new Template<eTypeInspect, ResultTemplate>("Results");
        public static YoonConsoler pCLM = new YoonConsoler();
        public static YoonDisplayer pDLM = new YoonDisplayer();

        public static string strWorkDirectory = Directory.GetCurrentDirectory();
        public static string strImageDirectory = Path.Combine(strWorkDirectory, @"Image");
        public static string strParamDirectory = Path.Combine(strWorkDirectory, @"Parameter");
        public static string strResultDirectory = Path.Combine(strWorkDirectory, @"Result");
        public static string strCurrentWorkingDirectory = Path.Combine(strWorkDirectory, @"Current");

        public static KeyValuePair<int, eTypeInspect> GetInspectFlagFromStringTag(string strContentSelected)
        {
            int nInspNo = 0;
            eTypeInspect nInspType = eTypeInspect.None;
            if (strContentSelected == null || strContentSelected == string.Empty || strContentSelected == "Origin" || strContentSelected == "CurrentProcessing")
                return new KeyValuePair<int, eTypeInspect>(nInspNo, nInspType);

            //// String Split 및 Paramter 추출
            string[] pArrayContens = strContentSelected.Split('.');  // 예시 : "0.Preprocessing"
            if (pArrayContens.Length != 2)
                return new KeyValuePair<int, eTypeInspect>(nInspNo, nInspType);

            nInspNo = Convert.ToInt32(pArrayContens[0]);
            nInspType = (eTypeInspect)GetEnumValue(typeof(eTypeInspect), pArrayContens[1]);
            return new KeyValuePair<int, eTypeInspect>(nInspNo, nInspType);
        }
        public static object GetEnumValue(Type type, string strValue)
        {
            if (!type.IsEnum) return -1;

            string[] pArrayNames = Enum.GetNames(type);
            for (int iName = 0; iName < pArrayNames.Length; iName++)
            {
                if (pArrayNames[iName] == strValue)
                {
                    return Enum.GetValues(type).GetValue(iName);
                }
            }
            return -1;
        }

        public static bool ProcessPatternMatchOrigin(int nInspectedOrigin)
        {
            ////  현재 Inspection 값 취득
            int nModelNo = CommonClass.pConfig.SelectedModelNo;
            string strModelName = CommonClass.pConfig.SelectedModelName;
            int nJobNo = CommonClass.pConfig.SelectedJobNo;
            string strJobName = CommonClass.pConfig.SelectedJobName;
            int nInspNo = CommonClass.pConfig.SelectedInspectionNo;
            eTypeInspect nTypeInsp = CommonClass.pConfig.SelectedInspectionType;

            //// Process Parameter 초기화
            bool bRun = true, bResultOrigin = false;
            int nIndexModel = 0, nIndexJob = 0, nIndexInsp = 0;
            eStepOrigin nJobStep = eStepOrigin.Init;
            eStepOrigin nJobStepBK = eStepOrigin.None;
            JobInfo pJobInfo = null;
            InspectionInfo pInspectionInfo = null;
            ParameterInspectionPatternMatching pParamPM = null;

            while (bRun)
            {
                switch (nJobStep)
                {
                    case eStepOrigin.Init:
                        nJobStepBK = nJobStep;
                        nIndexModel = GetModelIndex(nModelNo, strModelName);
                        nIndexJob = GetJobIndex(nModelNo, strModelName, nJobNo, strJobName);
                        nIndexInsp = GetInspectionIndex(nModelNo, strModelName, nJobNo, strJobName, nInspNo, nTypeInsp);
                        if (nIndexModel < 0 || nIndexJob < 0 || nIndexInsp < 0)
                        {
                            nJobStep = eStepOrigin.Error;
                            break;
                        }
                        pJobInfo = CommonClass.pListModel[nIndexModel].JobList[nIndexJob];
                        pInspectionInfo = CommonClass.pListModel[nIndexModel].JobList[nIndexJob].InspectionList[nIndexInsp];
                        switch (nTypeInsp)
                        {
                            case eTypeInspect.PatternMatching:
                                nJobStep = eStepOrigin.Apply;
                                break;
                            default:
                                nJobStep = eStepOrigin.Finish;
                                break;
                        }
                        break;
                    case eStepOrigin.PatternMatching:
                        break;
                    case eStepOrigin.Apply:
                        nJobStepBK = nJobStep;
                        if (nInspectedOrigin > CommonClass.MAX_PATTERN_NUM || nInspectedOrigin <= 0)
                        {
                            nJobStep = eStepOrigin.Error;
                            break;
                        }
                        //// Origin Teaching 여부 초기화
                        pParamPM = pInspectionInfo.InspectionParam as ParameterInspectionPatternMatching;
                        for (int iPattern = 0; iPattern < CommonClass.MAX_PATTERN_NUM; iPattern++)
                        {
                            pParamPM.IsOriginTeachedFlags[iPattern] = false;
                            pParamPM.OriginPixelXs[iPattern] = CommonClass.DEFAULT_POSITION;
                            pParamPM.OriginPixelYs[iPattern] = CommonClass.DEFAULT_POSITION;
                            pParamPM.OriginRealXs[iPattern] = CommonClass.DEFAULT_POSITION;
                            pParamPM.OriginRealYs[iPattern] = CommonClass.DEFAULT_POSITION;
                            pParamPM.OriginThetas[iPattern] = CommonClass.DEFAULT_POSITION;
                        }
                        //// Align Type 재등록
                        switch (nInspectedOrigin)
                        {
                            case 1:
                                pParamPM.AlignType = eTypeAlign.OnePoint;
                                break;
                            case 2:
                                if (pParamPM.IsUseMultiPatternAlign)
                                    pParamPM.AlignType = eTypeAlign.TwoPoint;
                                else pParamPM.AlignType = eTypeAlign.OnePoint;
                                break;
                            case 3:
                            case 4:
                                if (pParamPM.IsUseMultiPatternAlign)
                                    pParamPM.AlignType = eTypeAlign.FourPoint;
                                else pParamPM.AlignType = eTypeAlign.OnePoint;
                                break;
                            default:
                                break;
                        }
                        if (pParamPM.AlignType == eTypeAlign.None)
                        {
                            nJobStep = eStepOrigin.Error;
                            break;
                        }
                        //// Teaching값 재등록
                        for (int iPattern = 0; iPattern < nInspectedOrigin; iPattern++)
                        {
                            CognexResult pResult = null;
                            switch (iPattern) // Pattern Count == 최대 4개 (0, 1, 2, 3)
                            {
                                case 0:
                                    pResult = pInspectionInfo.CogResultContainer.GetValue(eYoonCognexType.PMAlign, eLabelInspect.Main.ToString());
                                    break;
                                case 1:
                                    pResult = pInspectionInfo.CogResultContainer.GetValue(eYoonCognexType.PMAlign, eLabelInspect.Second.ToString());
                                    break;
                                case 2:
                                    pResult = pInspectionInfo.CogResultContainer.GetValue(eYoonCognexType.PMAlign, eLabelInspect.Third.ToString());
                                    break;
                                case 3:
                                    pResult = pInspectionInfo.CogResultContainer.GetValue(eYoonCognexType.PMAlign, eLabelInspect.Forth.ToString());
                                    break;
                                default:
                                    continue;
                            }
                            if (pResult.TotalScore > 0) // Save as Real Position
                            {
                                pParamPM.IsOriginTeachedFlags[iPattern] = true;
                                YoonVector2D vecPixel = pResult.GetPatternMatchPoint();
                                pParamPM.OriginPixelXs[iPattern] = vecPixel.X;
                                pParamPM.OriginPixelYs[iPattern] = vecPixel.Y;
                                YoonVector2D vecReal = pJobInfo.CalibrationMap.ToReal(vecPixel) as YoonVector2D;
                                pParamPM.OriginRealXs[iPattern] = vecReal.X;
                                pParamPM.OriginRealYs[iPattern] = vecReal.Y;
                                pParamPM.OriginThetas[iPattern] = pResult.GetPatternRotation();
                            }
                        }
                        pInspectionInfo.InspectionParam = pParamPM;
                        bResultOrigin = true;
                        nJobStep = eStepOrigin.Finish;
                        break;
                    case eStepOrigin.Error:
                        switch (nJobStepBK)
                        {
                            case eStepOrigin.Init:
                                CommonClass.pCLM.Write("Origin Process Init Failure");
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "Origin Init Failure");
                                break;
                            case eStepOrigin.Apply:
                                CommonClass.pCLM.Write("Origin Apply Failure");
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "PM Origin Failure");
                                break;
                        }
                        nJobStep = eStepOrigin.Finish;
                        bResultOrigin = false;
                        break;
                    case eStepOrigin.Finish:
                        CommonClass.pListModel[nIndexModel].JobList[nIndexJob].InspectionList[nIndexInsp] = pInspectionInfo;
                        bRun = false;
                        break;
                }
            }

            return bResultOrigin;
        }

        public static bool ProcessPreprocessing(ICogImage pImageSource, ref ICogImage pImageResult)
        {
            if (pImageSource == null) return false;

            ////  현재 Inspection 값 취득
            int nModelNo = CommonClass.pConfig.SelectedModelNo;
            string strModelName = CommonClass.pConfig.SelectedModelName;
            int nJobNo = CommonClass.pConfig.SelectedJobNo;
            string strJobName = CommonClass.pConfig.SelectedJobName;
            int nInspNo = CommonClass.pConfig.SelectedInspectionNo;
            eTypeInspect nTypeInsp = CommonClass.pConfig.SelectedInspectionType;

            //// Process Parameter 초기화
            bool bRun = true;
            int nIndexModel = 0, nIndexJob = 0, nIndexInsp = 0;
            string strErrorMessage = string.Empty;
            eStepPreprocessing nJobStep = eStepPreprocessing.Init;
            eStepPreprocessing nJobStepBK = eStepPreprocessing.None;
            CognexResult pResultInsp = null;
            InspectionInfo pInspectionInfo = null;
            ParameterInspectionPreprocessing pParam = null;
            ICogImage pImagePipeline = null;

            while (bRun)
            {
                switch (nJobStep)
                {
                    case eStepPreprocessing.Init:
                        nJobStepBK = nJobStep;
                        nIndexModel = GetModelIndex(nModelNo, strModelName);
                        nIndexJob = GetJobIndex(nModelNo, strModelName, nJobNo, strJobName);
                        nIndexInsp = GetInspectionIndex(nModelNo, strModelName, nJobNo, strJobName, nInspNo, nTypeInsp);
                        if (nIndexModel < 0 || nIndexJob < 0 || nIndexInsp < 0)
                        {
                            nJobStep = eStepPreprocessing.Error;
                            break;
                        }
                        pInspectionInfo = CommonClass.pListModel[nIndexModel].JobList[nIndexJob].InspectionList[nIndexInsp];
                        pInspectionInfo.SourceImages[0] = pImageSource.CopyBase(CogImageCopyModeConstants.CopyPixels);
                        pParam = pInspectionInfo.InspectionParam as ParameterInspectionPreprocessing;
                        if (!pParam.IsUse)
                        {
                            nJobStep = eStepPreprocessing.Result;
                            break;
                        }
                        nJobStep = eStepPreprocessing.Convert;
                        break;
                    case eStepPreprocessing.Convert:
                        if (!pParam.IsUseImageConvert)
                        {
                            pImagePipeline = pInspectionInfo.SourceImages[0];
                            nJobStep = eStepPreprocessing.ImageFiltering;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (CogToolFactory.ImageConvert(pInspectionInfo.CogToolContainer.GetValue(eYoonCognexType.Convert, string.Empty), pImageSource, ref strErrorMessage, ref pResultInsp))
                        {
                            pInspectionInfo.CogResultContainer.SetValue(eYoonCognexType.Convert, string.Empty, pResultInsp.Clone() as CognexResult);
                            pInspectionInfo.CogResultContainer.SaveValue(eYoonCognexType.Convert, string.Empty);
                            pImagePipeline = pResultInsp.ResultImage;
                            nJobStep = eStepPreprocessing.ImageFiltering;
                        }
                        else
                            nJobStep = eStepPreprocessing.Error;
                        break;
                    case eStepPreprocessing.ImageFiltering:
                        if (!pParam.IsUseImageFilter)
                        {
                            nJobStep = eStepPreprocessing.Sobel;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (CogToolFactory.ImageFiltering(pInspectionInfo.CogToolContainer.GetValue(eYoonCognexType.Filtering, string.Empty), pImagePipeline, ref strErrorMessage, ref pResultInsp))
                        {
                            pInspectionInfo.CogResultContainer.SetValue(eYoonCognexType.Filtering, string.Empty, pResultInsp.Clone() as CognexResult);
                            pInspectionInfo.CogResultContainer.SaveValue(eYoonCognexType.Filtering, string.Empty);
                            pImagePipeline = pResultInsp.ResultImage;
                            nJobStep = eStepPreprocessing.Sobel;
                        }
                        else
                            nJobStep = eStepPreprocessing.Error;
                        break;
                    case eStepPreprocessing.Sobel:
                        if (!pParam.IsUseSobelEdge)
                        {
                            nJobStep = eStepPreprocessing.Result;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (CogToolFactory.SobelEdge(pInspectionInfo.CogToolContainer.GetValue(eYoonCognexType.Sobel, string.Empty), pImagePipeline, ref strErrorMessage, ref pResultInsp))
                        {
                            pInspectionInfo.CogResultContainer.SetValue(eYoonCognexType.Sobel, string.Empty, pResultInsp.Clone() as CognexResult);
                            pInspectionInfo.CogResultContainer.SaveValue(eYoonCognexType.Sobel, string.Empty);
                            pImagePipeline = pResultInsp.ResultImage;
                            nJobStep = eStepPreprocessing.Result;
                        }
                        else
                            nJobStep = eStepPreprocessing.Error;
                        break;
                    case eStepPreprocessing.Result:
                        if (nJobStepBK == eStepPreprocessing.Init)
                            pInspectionInfo.InspectResult = false;
                        else
                        {
                            pInspectionInfo.InspectResult = true;
                            pInspectionInfo.ResultImage = pImagePipeline.CopyBase(CogImageCopyModeConstants.CopyPixels);
                            pImageResult = pInspectionInfo.ResultImage;
                            CommonClass.pCLM.Write("Preprocess Success");
                            CommonClass.pDLMAction.Write(eYoonStatus.Inspect, "Preprocess Success");
                        }
                        nJobStep = eStepPreprocessing.Finish;
                        break;
                    case eStepPreprocessing.Error:
                        switch (nJobStepBK)
                        {
                            case eStepPreprocessing.Init:
                                CommonClass.pCLM.Write("Preprocess Init Failure");
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "Preprocess Init Failure");
                                break;
                            case eStepPreprocessing.Convert:
                                CommonClass.pCLM.Write("Preprocess Convert Failure >>" + strErrorMessage);
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "Preprocess Convert Failure");
                                break;
                            case eStepPreprocessing.ImageFiltering:
                                CommonClass.pCLM.Write("Preprocess IPOneImage Failure >>" + strErrorMessage);
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "Preprocess Image Filtering Failure");
                                break;
                            case eStepPreprocessing.Sobel:
                                CommonClass.pCLM.Write("Preprocess Sobel Failure >>" + strErrorMessage);
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "Preprocess Sobel Failure");
                                break;
                        }
                        nJobStep = eStepPreprocessing.Finish;
                        pInspectionInfo.InspectResult = false;
                        break;
                    case eStepPreprocessing.Finish:
                        //// Inspection 결과값 등 갱신
                        CommonClass.pListModel[nIndexModel].JobList[nIndexJob].InspectionList[nIndexInsp] = pInspectionInfo;
                        bRun = false;
                        break;
                }
            }

            return pInspectionInfo.InspectResult;
        }

        public static int ProcessPatternMatchAlign(CogImage8Grey pImageSource, ref CogImage8Grey pImageResult, ref YoonVector2D pVecResult, ref double dThetaResult, ref AlignResult pAlignResult)
        {
            if (pImageSource == null) return -1;
            int iPatternInspected = -1;

            ////  현재 Inspection 값 취득
            int nModelNo = CommonClass.pConfig.SelectedModelNo;
            string strModelName = CommonClass.pConfig.SelectedModelName;
            int nJobNo = CommonClass.pConfig.SelectedJobNo;
            string strJobName = CommonClass.pConfig.SelectedJobName;
            int nInspNo = CommonClass.pConfig.SelectedInspectionNo;
            eTypeInspect nTypeInsp = CommonClass.pConfig.SelectedInspectionType;

            //// Process Parameter 초기화
            bool bRun = true;
            string strErrorMessage = string.Empty;
            int nIndexModel = 0, nIndexJob = 0, nIndexInsp = 0;
            eStepPatternMatching nJobStep = eStepPatternMatching.Init;
            eStepPatternMatching nJobStepBK = eStepPatternMatching.None;
            CognexResult pResultInsp = null;
            bool[] pArrayFlagResultContain = null;
            YoonVector2D[] pArrayResultVec = null;
            double[] pArrayTheta = null;
            JobInfo pJobInfo = null;
            InspectionInfo pInspectionInfo = null;
            ParameterInspectionPatternMatching pParam = null;

            while (bRun)
            {
                switch (nJobStep)
                {
                    case eStepPatternMatching.Init:
                        nJobStepBK = nJobStep;
                        nIndexModel = GetModelIndex(nModelNo, strModelName);
                        nIndexJob = GetJobIndex(nModelNo, strModelName, nJobNo, strJobName);
                        nIndexInsp = GetInspectionIndex(nModelNo, strModelName, nJobNo, strJobName, nInspNo, nTypeInsp);
                        if (nIndexModel < 0 || nIndexJob < 0 || nIndexInsp < 0)
                        {
                            nJobStep = eStepPatternMatching.Error;
                            break;
                        }
                        pJobInfo = CommonClass.pListModel[nIndexModel].JobList[nIndexJob];
                        pInspectionInfo = CommonClass.pListModel[nIndexModel].JobList[nIndexJob].InspectionList[nIndexInsp];
                        pInspectionInfo.SourceImages[0] = pImageSource.CopyBase(CogImageCopyModeConstants.CopyPixels);
                        pParam = pInspectionInfo.InspectionParam as ParameterInspectionPatternMatching;
                        if (!pParam.IsUse)
                        {
                            nJobStep = eStepPatternMatching.Result;
                            break;
                        }
                        iPatternInspected = 0;
                        pArrayResultVec = new YoonVector2D[CommonClass.MAX_PATTERN_NUM];
                        pArrayTheta = new double[CommonClass.MAX_PATTERN_NUM];
                        pArrayFlagResultContain = new bool[CommonClass.MAX_PATTERN_NUM];
                        for (int iPattern = 0; iPattern < CommonClass.MAX_PATTERN_NUM; iPattern++)
                        {
                            pArrayResultVec[iPattern] = new YoonVector2D();
                            pArrayFlagResultContain[iPattern] = false;
                        }
                        nJobStep = eStepPatternMatching.MainPattern;
                        break;
                    case eStepPatternMatching.MainPattern:
                        if (!pParam.IsUseEachPatterns[CommonFunction.GetPatternIndex(eLevelPattern.Main)])
                        {
                            nJobStep = eStepPatternMatching.SecondPattern;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (CogToolFactory.PMAlign(pInspectionInfo.CogToolContainer.GetValue(eYoonCognexType.PMAlign, eLabelInspect.Main.ToString()), pInspectionInfo.SourceImages[0], ref strErrorMessage, ref pResultInsp))
                        {
                            pInspectionInfo.CogResultContainer.SetValue(eYoonCognexType.PMAlign, eLabelInspect.Main.ToString(), pResultInsp.Clone() as CognexResult);
                            pInspectionInfo.CogResultContainer.SaveValue(eYoonCognexType.PMAlign, eLabelInspect.Main.ToString());
                            int iPattern = GetPatternIndex(eLevelPattern.Main);
                            pArrayFlagResultContain[iPattern] = true;
                            pArrayResultVec[iPattern] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[iPattern] = pResultInsp.GetPatternRotation();
                            nJobStep = eStepPatternMatching.SecondPattern;
                            iPatternInspected++;
                        }
                        else
                            nJobStep = eStepPatternMatching.SubPattern1;
                        break;
                    case eStepPatternMatching.SubPattern1:
                        if (!pParam.IsUseEachPatterns[CommonFunction.GetPatternIndex(eLevelPattern.Second)])
                        {
                            nJobStep = eStepPatternMatching.SubPattern2;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (CogToolFactory.PMAlign(pInspectionInfo.CogToolContainer.GetValue(eYoonCognexType.PMAlign, eLabelInspect.Second.ToString()), pInspectionInfo.SourceImages[0], ref strErrorMessage, ref pResultInsp))
                        {
                            pInspectionInfo.CogResultContainer.SetValue(eYoonCognexType.PMAlign, eLabelInspect.Second.ToString(), pResultInsp.Clone() as CognexResult);
                            pInspectionInfo.CogResultContainer.SaveValue(eYoonCognexType.PMAlign, eLabelInspect.Second.ToString());
                            int iPattern = GetPatternIndex(eLevelPattern.Second);
                            pArrayFlagResultContain[iPattern] = true;
                            pArrayResultVec[iPattern] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[iPattern] = pResultInsp.GetPatternRotation();
                            nJobStep = eStepPatternMatching.Align;
                            iPatternInspected++;
                        }
                        else
                            nJobStep = eStepPatternMatching.SubPattern2;
                        break;
                    case eStepPatternMatching.SubPattern2:
                        if (!pParam.IsUseEachPatterns[CommonFunction.GetPatternIndex(eLevelPattern.Third)])
                        {
                            nJobStep = eStepPatternMatching.SubPattern3;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (CogToolFactory.PMAlign(pInspectionInfo.CogToolContainer.GetValue(eYoonCognexType.PMAlign, eLabelInspect.Third.ToString()), pInspectionInfo.SourceImages[0], ref strErrorMessage, ref pResultInsp))
                        {
                            pInspectionInfo.CogResultContainer.SetValue(eYoonCognexType.PMAlign, eLabelInspect.Third.ToString(), pResultInsp.Clone() as CognexResult);
                            pInspectionInfo.CogResultContainer.SaveValue(eYoonCognexType.PMAlign, eLabelInspect.Third.ToString());
                            int iPattern = GetPatternIndex(eLevelPattern.Third);
                            pArrayFlagResultContain[iPattern] = true;
                            pArrayResultVec[iPattern] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[iPattern] = pResultInsp.GetPatternRotation();
                            nJobStep = eStepPatternMatching.Align;
                            iPatternInspected++;
                        }
                        else
                            nJobStep = eStepPatternMatching.SubPattern3;
                        break;
                    case eStepPatternMatching.SubPattern3:
                        if (!pParam.IsUseEachPatterns[CommonFunction.GetPatternIndex(eLevelPattern.Forth)])
                        {
                            nJobStep = eStepPatternMatching.Error;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (CogToolFactory.PMAlign(pInspectionInfo.CogToolContainer.GetValue(eYoonCognexType.PMAlign, eLabelInspect.Forth.ToString()), pInspectionInfo.SourceImages[0], ref strErrorMessage, ref pResultInsp))
                        {
                            pInspectionInfo.CogResultContainer.SetValue(eYoonCognexType.PMAlign, eLabelInspect.Forth.ToString(), pResultInsp.Clone() as CognexResult);
                            pInspectionInfo.CogResultContainer.SaveValue(eYoonCognexType.PMAlign, eLabelInspect.Forth.ToString());
                            int iPattern = GetPatternIndex(eLevelPattern.Forth);
                            pArrayFlagResultContain[iPattern] = true;
                            pArrayResultVec[iPattern] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[iPattern] = pResultInsp.GetPatternRotation();
                            nJobStep = eStepPatternMatching.Align;
                            iPatternInspected++;
                        }
                        else
                            nJobStep = eStepPatternMatching.Error;
                        break;
                    case eStepPatternMatching.SecondPattern:
                        if (!pParam.IsUseMultiPatternInspection || !pParam.IsUseEachPatterns[CommonFunction.GetPatternIndex(eLevelPattern.Second)])
                        {
                            nJobStep = eStepPatternMatching.ThridPattern;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (CogToolFactory.PMAlign(pInspectionInfo.CogToolContainer.GetValue(eYoonCognexType.PMAlign, eLabelInspect.Second.ToString()), pInspectionInfo.SourceImages[0], ref strErrorMessage, ref pResultInsp))
                        {
                            pInspectionInfo.CogResultContainer.SetValue(eYoonCognexType.PMAlign, eLabelInspect.Second.ToString(), pResultInsp.Clone() as CognexResult);
                            pInspectionInfo.CogResultContainer.SaveValue(eYoonCognexType.PMAlign, eLabelInspect.Second.ToString());
                            int iPattern = GetPatternIndex(eLevelPattern.Second);
                            pArrayFlagResultContain[iPattern] = true;
                            pArrayResultVec[iPattern] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[iPattern] = pResultInsp.GetPatternRotation();
                            nJobStep = eStepPatternMatching.ThridPattern;
                            iPatternInspected++;
                        }
                        else
                            nJobStep = eStepPatternMatching.Align;
                        break;
                    case eStepPatternMatching.ThridPattern:
                        if (!pParam.IsUseMultiPatternInspection || !pParam.IsUseEachPatterns[CommonFunction.GetPatternIndex(eLevelPattern.Third)])
                        {
                            nJobStep = eStepPatternMatching.ForthPattern;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (CogToolFactory.PMAlign(pInspectionInfo.CogToolContainer.GetValue(eYoonCognexType.PMAlign, eLabelInspect.Third.ToString()), pInspectionInfo.SourceImages[0], ref strErrorMessage, ref pResultInsp))
                        {
                            pInspectionInfo.CogResultContainer.SetValue(eYoonCognexType.PMAlign, eLabelInspect.Third.ToString(), pResultInsp.Clone() as CognexResult);
                            pInspectionInfo.CogResultContainer.SaveValue(eYoonCognexType.PMAlign, eLabelInspect.Third.ToString());
                            int iPattern = GetPatternIndex(eLevelPattern.Third);
                            pArrayFlagResultContain[iPattern] = true;
                            pArrayResultVec[iPattern] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[iPattern] = pResultInsp.GetPatternRotation();
                            nJobStep = eStepPatternMatching.ForthPattern;
                            iPatternInspected++;
                        }
                        else
                            nJobStep = eStepPatternMatching.Align;
                        break;
                    case eStepPatternMatching.ForthPattern:
                        if (!pParam.IsUseMultiPatternInspection || !pParam.IsUseEachPatterns[CommonFunction.GetPatternIndex(eLevelPattern.Forth)])
                        {
                            nJobStep = eStepPatternMatching.Align;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (CogToolFactory.PMAlign(pInspectionInfo.CogToolContainer.GetValue(eYoonCognexType.PMAlign, eLabelInspect.Forth.ToString()), pInspectionInfo.SourceImages[0], ref strErrorMessage, ref pResultInsp))
                        {
                            pInspectionInfo.CogResultContainer.SetValue(eYoonCognexType.PMAlign, eLabelInspect.Forth.ToString(), pResultInsp.Clone() as CognexResult);
                            pInspectionInfo.CogResultContainer.SaveValue(eYoonCognexType.PMAlign, eLabelInspect.Forth.ToString());
                            int iPattern = GetPatternIndex(eLevelPattern.Forth);
                            pArrayFlagResultContain[iPattern] = true;
                            pArrayResultVec[iPattern] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[iPattern] = pResultInsp.GetPatternRotation();
                            nJobStep = eStepPatternMatching.Align;
                            iPatternInspected++;
                        }
                        else
                            nJobStep = eStepPatternMatching.Align;
                        break;
                    case eStepPatternMatching.Align:
                        if (!pParam.IsCheckAlign || iPatternInspected <= 0)
                        {
                            nJobStep = eStepPatternMatching.Result;
                            break;
                        }
                        if (pAlignResult == null)
                            pAlignResult = new AlignResult();
                        switch (pParam.AlignType)
                        {
                            case eTypeAlign.OnePoint:
                                double dOffsetX = 0.0, dOffsetY = 0.0, dOffsetT = 0.0;
                                for (int iPattern = 0; iPattern < CommonClass.MAX_PATTERN_NUM; iPattern++)
                                {
                                    if (pParam.IsOriginTeachedFlags[iPattern] && pArrayFlagResultContain[iPattern])
                                    {
                                        YoonVector2D vecRealPos = pJobInfo.CalibrationMap.ToReal(pArrayResultVec[iPattern]) as YoonVector2D;    // Trarnsfer to Real Pos
                                        dOffsetX += (vecRealPos.X - pParam.OriginRealXs[iPattern]);
                                        dOffsetY += (vecRealPos.Y - pParam.OriginRealYs[iPattern]);
                                        dOffsetT += (pArrayTheta[iPattern] - pParam.OriginThetas[iPattern]);
                                    }
                                }
                                pAlignResult.ResultX = dOffsetX / iPatternInspected;
                                pAlignResult.ResultY = dOffsetY / iPatternInspected;
                                pAlignResult.ResultT = dOffsetT / iPatternInspected;
                                break;
                            default:
                                break;
                        }
                        nJobStep = eStepPatternMatching.Result;
                        break;
                    case eStepPatternMatching.Result:
                        switch (nJobStepBK)
                        {
                            case eStepPatternMatching.Init:
                                pInspectionInfo.InspectResult = false;
                                break;
                            case eStepPatternMatching.MainPattern:
                                pInspectionInfo.InspectResult = true;
                                pImageResult = SetPatternMatchResultOverlap(new eLabelInspect[1] { eLabelInspect.Main }, pInspectionInfo.SourceImages[0], nModelNo, strModelName, nJobNo, strJobName, nInspNo, nTypeInsp);
                                pInspectionInfo.ResultImage = pImageResult;
                                pVecResult = pJobInfo.CalibrationMap.ToReal(pArrayResultVec[GetPatternIndex(eLevelPattern.Main)]) as YoonVector2D;    // Trarnsfer to Real Pos
                                break;
                            case eStepPatternMatching.SecondPattern:
                                pInspectionInfo.InspectResult = true;
                                pImageResult = SetPatternMatchResultOverlap(new eLabelInspect[2] { eLabelInspect.Main, eLabelInspect.Second }, pInspectionInfo.SourceImages[0], nModelNo, strModelName, nJobNo, strJobName, nInspNo, nTypeInsp);
                                pInspectionInfo.ResultImage = pImageResult;
                                pVecResult = pJobInfo.CalibrationMap.ToReal(pArrayResultVec[GetPatternIndex(eLevelPattern.Main)]) as YoonVector2D;    // Trarnsfer to Real Pos
                                break;
                            case eStepPatternMatching.ThridPattern:
                                pInspectionInfo.InspectResult = true;
                                pImageResult = SetPatternMatchResultOverlap(new eLabelInspect[3] { eLabelInspect.Main, eLabelInspect.Second, eLabelInspect.Third },
                                    pInspectionInfo.SourceImages[0], nModelNo, strModelName, nJobNo, strJobName, nInspNo, nTypeInsp);
                                pInspectionInfo.ResultImage = pImageResult;
                                pVecResult = pJobInfo.CalibrationMap.ToReal(pArrayResultVec[GetPatternIndex(eLevelPattern.Main)]) as YoonVector2D;    // Trarnsfer to Real Pos
                                break;
                            case eStepPatternMatching.ForthPattern:
                                pInspectionInfo.InspectResult = true;
                                pImageResult = SetPatternMatchResultOverlap(new eLabelInspect[4] { eLabelInspect.Main, eLabelInspect.Second, eLabelInspect.Third, eLabelInspect.Forth },
                                    pInspectionInfo.SourceImages[0], nModelNo, strModelName, nJobNo, strJobName, nInspNo, nTypeInsp);
                                pInspectionInfo.ResultImage = pImageResult;
                                pVecResult = pJobInfo.CalibrationMap.ToReal(pArrayResultVec[GetPatternIndex(eLevelPattern.Main)]) as YoonVector2D;    // Trarnsfer to Real Pos
                                break;
                            case eStepPatternMatching.SubPattern1:
                                pInspectionInfo.InspectResult = true;
                                pImageResult = SetPatternMatchResultOverlap(new eLabelInspect[1] { eLabelInspect.Second }, pInspectionInfo.SourceImages[0], nModelNo, strModelName, nJobNo, strJobName, nInspNo, nTypeInsp);
                                pInspectionInfo.ResultImage = pImageResult;
                                pVecResult = pJobInfo.CalibrationMap.ToReal(pArrayResultVec[GetPatternIndex(eLevelPattern.Second)]) as YoonVector2D;    // Trarnsfer to Real Pos
                                break;
                            case eStepPatternMatching.SubPattern2:
                                pInspectionInfo.InspectResult = true;
                                pImageResult = SetPatternMatchResultOverlap(new eLabelInspect[1] { eLabelInspect.Third }, pInspectionInfo.SourceImages[0], nModelNo, strModelName, nJobNo, strJobName, nInspNo, nTypeInsp);
                                pInspectionInfo.ResultImage = pImageResult;
                                pVecResult = pJobInfo.CalibrationMap.ToReal(pArrayResultVec[GetPatternIndex(eLevelPattern.Third)]) as YoonVector2D;    // Trarnsfer to Real Pos
                                break;
                            case eStepPatternMatching.SubPattern3:
                                pInspectionInfo.InspectResult = true;
                                pImageResult = SetPatternMatchResultOverlap(new eLabelInspect[1] { eLabelInspect.Forth }, pInspectionInfo.SourceImages[0], nModelNo, strModelName, nJobNo, strJobName, nInspNo, nTypeInsp);
                                pInspectionInfo.ResultImage = pImageResult;
                                pVecResult = pJobInfo.CalibrationMap.ToReal(pArrayResultVec[GetPatternIndex(eLevelPattern.Forth)]) as YoonVector2D;    // Trarnsfer to Real Pos
                                break;
                            default:
                                break;
                        }
                        CommonClass.pCLM.Write("Pattern Match Success");
                        CommonClass.pDLMAction.Write(eYoonStatus.Inspect, "Pattern Match Success");
                        nJobStep = eStepPatternMatching.Finish;
                        break;
                    case eStepPatternMatching.Error:
                        switch (nJobStepBK)
                        {
                            case eStepPatternMatching.Init:
                                CommonClass.pCLM.Write("Pattern Match Init Failure");
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "Pattern Match Init Failure");
                                break;
                            case eStepPatternMatching.MainPattern:
                            case eStepPatternMatching.SubPattern1:
                            case eStepPatternMatching.SubPattern2:
                            case eStepPatternMatching.SubPattern3:
                                CommonClass.pCLM.Write("Pattern Match Failure >>" + strErrorMessage);
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "Pattern Match Failure");
                                break;
                        }
                        iPatternInspected = -1;
                        pInspectionInfo.InspectResult = false;
                        nJobStep = eStepPatternMatching.Finish;
                        break;
                    case eStepPatternMatching.Finish:
                        CommonClass.pListModel[nIndexModel].JobList[nIndexJob].InspectionList[nIndexInsp] = pInspectionInfo;
                        //// 선언한 배열 초기화
                        if (pArrayResultVec != null)
                            pArrayResultVec = null;
                        if (pArrayFlagResultContain != null)
                            pArrayFlagResultContain = null;
                        bRun = false;
                        break;
                }
            }

            return iPatternInspected;
        }

        public static bool ProcessObjectExtract(CogImage8Grey pImageSourceBlob, CogImage24PlanarColor pImageSourceColorExtract, ref ICogImage pImageResult)    // Blob : 0, ColorExtract : 1
        {
            if (pImageSourceColorExtract == null || pImageSourceBlob == null) return false;

            ////  현재 Inspection 값 취득
            int nModelNo = CommonClass.pConfig.SelectedModelNo;
            string strModelName = CommonClass.pConfig.SelectedModelName;
            int nJobNo = CommonClass.pConfig.SelectedJobNo;
            string strJobName = CommonClass.pConfig.SelectedJobName;
            int nInspNo = CommonClass.pConfig.SelectedInspectionNo;
            eTypeInspect nTypeInsp = CommonClass.pConfig.SelectedInspectionType;

            //// Process Parameter 초기화
            bool bRun = true;
            string strErrorMessage = string.Empty;
            int nIndexModel = 0, nIndexJob = 0, nIndexInsp = 0;
            eStepObjectExtract nJobStep = eStepObjectExtract.Init;
            eStepObjectExtract nJobStepBK = eStepObjectExtract.None;
            CognexResult pResultInsp = null;
            InspectionInfo pInspectionInfo = null;
            ParameterInspectionObjectExtract pParam = null;
            ICogImage pImageBlob = null;
            ICogImage pImageColorSegment = null;
            //// (Combine 관련 변수 초기화)
            bool bInspectBlob = false;
            ICogImage pImageCombine = null;

            while (bRun)
            {
                switch (nJobStep)
                {
                    case eStepObjectExtract.Init:
                        nJobStepBK = nJobStep;
                        nIndexModel = GetModelIndex(nModelNo, strModelName);
                        nIndexJob = GetJobIndex(nModelNo, strModelName, nJobNo, strJobName);
                        nIndexInsp = GetInspectionIndex(nModelNo, strModelName, nJobNo, strJobName, nInspNo, nTypeInsp);
                        if (nIndexModel < 0 || nIndexJob < 0 || nIndexInsp < 0)
                        {
                            nJobStep = eStepObjectExtract.Error;
                            break;
                        }
                        pInspectionInfo = CommonClass.pListModel[nIndexModel].JobList[nIndexJob].InspectionList[nIndexInsp];
                        pInspectionInfo.SourceImages[0] = pImageSourceBlob.CopyBase(CogImageCopyModeConstants.CopyPixels);
                        pInspectionInfo.SourceImages[1] = pImageSourceColorExtract.CopyBase(CogImageCopyModeConstants.CopyPixels);
                        pParam = pInspectionInfo.InspectionParam as ParameterInspectionObjectExtract;
                        if (!pParam.IsUse)
                        {
                            nJobStep = eStepObjectExtract.Result;
                            break;
                        }
                        nJobStep = eStepObjectExtract.Blob;
                        break;
                    case eStepObjectExtract.Blob:
                        if (!pParam.IsUseBlob)
                        {
                            nJobStep = eStepObjectExtract.ColorSegment;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (CogToolFactory.Blob(pInspectionInfo.CogToolContainer.GetValue(eYoonCognexType.Blob, string.Empty), pInspectionInfo.SourceImages[0], ref strErrorMessage, ref pResultInsp))
                        {
                            bInspectBlob = true;
                            pInspectionInfo.CogResultContainer.SetValue(eYoonCognexType.Blob, string.Empty, pResultInsp.Clone() as CognexResult);
                            pInspectionInfo.CogResultContainer.SaveValue(eYoonCognexType.Blob, string.Empty);
                            pImageBlob = pResultInsp.ResultImage;
                            nJobStep = eStepObjectExtract.ColorSegment;
                        }
                        else
                            nJobStep = eStepObjectExtract.Error;
                        break;
                    case eStepObjectExtract.ColorSegment:
                        if (!pParam.IsUseColorSegment)
                        {
                            nJobStep = eStepObjectExtract.Result;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (CogToolFactory.ColorSegment(pInspectionInfo.CogToolContainer.GetValue(eYoonCognexType.ColorSegment, string.Empty), pInspectionInfo.SourceImages[1], ref strErrorMessage, ref pResultInsp))
                        {
                            pInspectionInfo.CogResultContainer.SetValue(eYoonCognexType.ColorSegment, string.Empty, pResultInsp.Clone() as CognexResult);
                            pInspectionInfo.CogResultContainer.SaveValue(eYoonCognexType.ColorSegment, string.Empty);
                            pImageColorSegment = pResultInsp.ResultImage;
                            if (bInspectBlob) nJobStep = eStepObjectExtract.Combine;
                            else nJobStep = eStepObjectExtract.Result;
                        }
                        else
                            nJobStep = eStepObjectExtract.Error;
                        break;
                    case eStepObjectExtract.Combine:
                        nJobStepBK = nJobStep;
                        switch (pParam.CombineType)
                        {
                            case eTypeProcessTwoImage.Add:
                                pImageCombine = CognexFactory.ImageAdd(pImageBlob, pImageColorSegment);
                                break;
                            case eTypeProcessTwoImage.OverlapMax:
                                pImageCombine = CognexFactory.ImageOverlapMax(pImageBlob, pImageColorSegment);
                                break;
                            case eTypeProcessTwoImage.OverlapMin:
                                pImageCombine = CognexFactory.ImageOverlapMin(pImageBlob, pImageColorSegment);
                                break;
                            case eTypeProcessTwoImage.None:
                                pImageCombine = CognexFactory.ImageSubstract(pImageBlob, pImageColorSegment);
                                break;
                            default:
                                break;
                        }
                        if (pImageCombine == null)
                            nJobStep = eStepObjectExtract.Error;
                        else
                            nJobStep = eStepObjectExtract.Result;
                        break;
                    case eStepObjectExtract.Result:
                        switch (nJobStepBK)
                        {
                            case eStepObjectExtract.Init:
                                pInspectionInfo.InspectResult = false;
                                break;
                            case eStepObjectExtract.Blob:
                                pInspectionInfo.InspectResult = true;
                                pImageResult = pImageBlob.CopyBase(CogImageCopyModeConstants.CopyPixels);
                                pInspectionInfo.ResultImage = pImageResult;
                                break;
                            case eStepObjectExtract.ColorSegment:
                                pInspectionInfo.InspectResult = true;
                                pImageResult = pImageColorSegment.CopyBase(CogImageCopyModeConstants.CopyPixels);
                                pInspectionInfo.ResultImage = pImageResult;
                                break;
                            case eStepObjectExtract.Combine:
                                pInspectionInfo.InspectResult = true;
                                pImageResult = pImageCombine.CopyBase(CogImageCopyModeConstants.CopyPixels);
                                pInspectionInfo.ResultImage = pImageResult;
                                break;
                        }
                        CommonClass.pCLM.Write("Object Extract Success");
                        CommonClass.pDLMAction.Write(eYoonStatus.Inspect, "Object Extract Success");
                        nJobStep = eStepObjectExtract.Finish;
                        break;
                    case eStepObjectExtract.Error:
                        switch (nJobStepBK)
                        {
                            case eStepObjectExtract.Init:
                                CommonClass.pCLM.Write("Object Extract Init Failure");
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "Object Extract Init Failure");
                                break;
                            case eStepObjectExtract.Blob:
                                CommonClass.pCLM.Write("Blob Inspect Failure");
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "Blob Inspect Failure");
                                break;
                            case eStepObjectExtract.ColorSegment:
                                CommonClass.pCLM.Write("Color Segmentation Failure");
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "Color Segmentation Failure");
                                break;
                            case eStepObjectExtract.Combine:
                                CommonClass.pCLM.Write("Image Combine Failure");
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "Image Combine Failure");
                                break;
                        }
                        pInspectionInfo.InspectResult = false;
                        nJobStep = eStepObjectExtract.Finish;
                        break;
                    case eStepObjectExtract.Finish:
                        CommonClass.pListModel[nIndexModel].JobList[nIndexJob].InspectionList[nIndexInsp] = pInspectionInfo;
                        bRun = false;
                        break;
                }
            }

            return pInspectionInfo.InspectResult;
        }

        public static bool ProcessImageCombine(ICogImage pImageSourceA, ICogImage pImageSourceB, ref ICogImage pImageResult)
        {
            if (pImageSourceA == null || pImageSourceB == null) return false;

            ////  현재 Inspection 값 취득
            int nModelNo = CommonClass.pConfig.SelectedModelNo;
            string strModelName = CommonClass.pConfig.SelectedModelName;
            int nJobNo = CommonClass.pConfig.SelectedJobNo;
            string strJobName = CommonClass.pConfig.SelectedJobName;
            int nInspNo = CommonClass.pConfig.SelectedInspectionNo;
            eTypeInspect nTypeInsp = CommonClass.pConfig.SelectedInspectionType;

            //// Process Parameter 초기화
            bool bRun = true;
            int nIndexModel = 0, nIndexJob = 0, nIndexInsp = 0;
            string strErrorMessage = string.Empty;
            eStepCombine nJobStep = eStepCombine.Init;
            eStepCombine nJobStepBK = eStepCombine.None;
            InspectionInfo pInspectionInfo = null;
            ParameterInspectionCombine pParam = null;

            while (bRun)
            {
                switch (nJobStep)
                {
                    case eStepCombine.Init:
                        nJobStepBK = nJobStep;
                        nIndexModel = GetModelIndex(nModelNo, strModelName);
                        nIndexJob = GetJobIndex(nModelNo, strModelName, nJobNo, strJobName);
                        nIndexInsp = GetInspectionIndex(nModelNo, strModelName, nJobNo, strJobName, nInspNo, nTypeInsp);
                        if (nIndexModel < 0 || nIndexJob < 0 || nIndexInsp < 0)
                        {
                            nJobStep = eStepCombine.Error;
                            break;
                        }
                        pInspectionInfo = CommonClass.pListModel[nIndexModel].JobList[nIndexJob].InspectionList[nIndexInsp];
                        pInspectionInfo.SourceImages[0] = pImageSourceA;
                        pInspectionInfo.SourceImages[1] = pImageSourceB;
                        pParam = pInspectionInfo.InspectionParam as ParameterInspectionCombine;
                        if (!pParam.IsUse)
                        {
                            nJobStep = eStepCombine.Finish;
                            break;
                        }
                        nJobStep = eStepCombine.Processing;
                        break;
                    case eStepCombine.Processing:
                        nJobStepBK = nJobStep;
                        switch (pParam.CombineType)
                        {
                            case eTypeProcessTwoImage.Add:
                                pImageResult = CognexFactory.ImageAdd(pInspectionInfo.SourceImages[0], pInspectionInfo.SourceImages[1]);
                                break;
                            case eTypeProcessTwoImage.OverlapMax:
                                pImageResult = CognexFactory.ImageOverlapMax(pInspectionInfo.SourceImages[0], pInspectionInfo.SourceImages[1]);
                                break;
                            case eTypeProcessTwoImage.OverlapMin:
                                pImageResult = CognexFactory.ImageOverlapMin(pInspectionInfo.SourceImages[0], pInspectionInfo.SourceImages[1]);
                                break;
                            case eTypeProcessTwoImage.None:
                                pImageResult = CognexFactory.ImageSubstract(pInspectionInfo.SourceImages[0], pInspectionInfo.SourceImages[1]);
                                break;
                            default:
                                break;
                        }
                        if (pImageResult == null)
                            nJobStep = eStepCombine.Error;
                        else
                        {
                            CommonClass.pCLM.Write("Image Combine Success");
                            CommonClass.pDLMAction.Write(eYoonStatus.Inspect, "Image Combine Success");
                            pInspectionInfo.ResultImage = pImageResult;
                            pInspectionInfo.InspectResult = true;
                            nJobStep = eStepCombine.Finish;
                        }
                        break;
                    case eStepCombine.Error:
                        switch (nJobStepBK)
                        {
                            case eStepCombine.Init:
                                CommonClass.pCLM.Write("Image Combine Init Failure");
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "Image Combine Init Failure");
                                break;
                            case eStepCombine.Processing:
                                CommonClass.pCLM.Write("Image Combine Failure");
                                CommonClass.pDLMAction.Write(eYoonStatus.Error, "Image Combine Failure");
                                break;
                        }
                        pInspectionInfo.InspectResult = false;
                        nJobStep = eStepCombine.Finish;
                        break;
                    case eStepCombine.Finish:
                        CommonClass.pListModel[nIndexModel].JobList[nIndexJob].InspectionList[nIndexInsp] = pInspectionInfo;
                        bRun = false;
                        break;
                }
            }
            return pInspectionInfo.InspectResult;
        }
    }
}