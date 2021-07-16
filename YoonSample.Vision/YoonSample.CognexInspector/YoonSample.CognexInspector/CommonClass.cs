using System;
using System.IO;
using System.Collections.Generic;
using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using YoonFactory;
using YoonFactory.Align;
using YoonFactory.Cognex;
using YoonFactory.Cognex.Tool;
using YoonFactory.Cognex.Result;
using YoonFactory.Log;
using YoonFactory.Param;

namespace YoonSample.CognexInspector
{
    public delegate void PassImageCallback(object sender, CogImageArgs e);
    public class CogImageArgs : EventArgs
    {
        public int InspectNo;
        public eTypeInspect InspectType;
        public CognexImage Image;

        public CogImageArgs(CognexImage pImage)
        {
            InspectNo = 0;
            InspectType = eTypeInspect.None;
            Image = pImage;
        }

        public CogImageArgs(ICogImage cogImage)
        {
            InspectNo = 0;
            InspectType = eTypeInspect.None;
            Image = new CognexImage(cogImage);
        }

        public CogImageArgs(eTypeInspect nInspType, CognexImage pImage)
        {
            InspectNo = 0;
            InspectType = nInspType;
            Image = pImage;
        }

        public CogImageArgs(eTypeInspect nInspType, ICogImage cogImage)
        {
            InspectNo = 0;
            InspectType = nInspType;
            Image = new CognexImage(cogImage);
        }

        public CogImageArgs(int nNo, eTypeInspect nInspType, CognexImage pImage)
        {
            InspectNo = nNo;
            InspectType = nInspType;
            Image = pImage;
        }

        public CogImageArgs(int nNo, eTypeInspect nInspType, ICogImage cogImage)
        {
            InspectNo = nNo;
            InspectType = nInspType;
            Image = new CognexImage(cogImage);
        }
    }

    public delegate void PassCogToolCallback(object sender, CogToolArgs e);
    public class CogToolArgs : EventArgs
    {
        public eYoonCognexType ToolType;
        public eLabelInspect Label;
        public ICogTool CogTool;
        public CognexImage ContainImage;

        public CogToolArgs(eYoonCognexType nType, eLabelInspect nLabel, ICogTool pTool, CognexImage pImage)
        {
            ToolType = nType;
            Label = nLabel;
            CogTool = pTool;
            if (pImage != null)
                ContainImage = pImage.Clone() as CognexImage;
        }

        public CogToolArgs(eYoonCognexType nType, eLabelInspect nLabel, ICogTool pTool, ICogImage cogImage)
        {
            ToolType = nType;
            Label = nLabel;
            CogTool = pTool;
            if (cogImage != null)
                ContainImage = new CognexImage(cogImage);
        }
    }

    public delegate void PassRequestCallback(object sender, EventArgs e);

    public static class CommonClass
    {
        public const int DEFULAT_IMAGE_WIDTH = 640;
        public const int DEFULAT_IMAGE_HEIGHT = 480;
        public const int MAX_SOURCE_NUM = 4;
        public const int MAX_PATTERN_NUM = 4;
        public const double DEFAULT_POSITION = -10000.0;

        public static YoonTemplate<eTypeInspect> pParamTemplate = new YoonTemplate<eTypeInspect>();
        public static YoonTemplate<eTypeInspect, ToolTemplate> pCogToolTemplate = new YoonTemplate<eTypeInspect, ToolTemplate>("Tools");
        public static YoonTemplate<eTypeInspect, ResultTemplate> pCogResultTemplate = new YoonTemplate<eTypeInspect, ResultTemplate>("Results");
        public static YoonConsoler pCLM = new YoonConsoler(90);
        public static YoonDisplayer pDLM = new YoonDisplayer(90);

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
            string[] pArrayContens = strContentSelected.Split('_');  // Example : "0_Preprocessing"
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

        public static CognexImage GetResultImage(int nInspNo, eTypeInspect nInspType)
        {
            if (nInspNo == 0 && nInspType == eTypeInspect.None) return null;

            //// Result Image 가져오기
            switch (nInspType)
            {
                case eTypeInspect.Preprocessing:
                case eTypeInspect.PatternMatching:
                case eTypeInspect.ObjectExtract:
                case eTypeInspect.Combine:
                    if (pCogResultTemplate[nInspType].GetLastResultImage() != null)
                        return pCogResultTemplate[nInspType].GetLastResultImage().Clone() as CognexImage;
                    break;
            }
            return null;
        }

        public static bool SetPatternOriginRegionToDisplay(CogDisplay pDisplay, int nInspNo, eTypeInspect nTypeInsp)
        {

            using (ResultTemplate pResultTemplate = pCogResultTemplate[nTypeInsp])
            {
                if (!pResultTemplate.ContainsKey(eYoonCognexType.PMAlign))
                    return false;

                ParameterInspectionPatternMatching pParam = CommonClass.pParamTemplate[nTypeInsp].Parameter as ParameterInspectionPatternMatching;
                foreach (string strTag in pResultTemplate[eYoonCognexType.PMAlign].Keys)
                {
                    eLabelInspect nTag = (eLabelInspect)Enum.Parse(typeof(eLabelInspect), strTag);
                    if (nTag == eLabelInspect.None) continue;
                    if (pParam.IsOriginTeachedFlags[nTag.ToInt()])
                        DisplayFactory.Draw.DrawCross(pDisplay, CogColorConstants.Red, pParam.OriginPixelXs[nTag.ToInt()], pParam.OriginPixelYs[nTag.ToInt()], 2.0);
                }
            }
            return true;
        }

        public static bool SetResultRegionToDisplay(CogDisplay pDisplay, int nInspNo, eTypeInspect nTypeInsp)
        {
            using (ResultTemplate pResultTemplate = pCogResultTemplate[nTypeInsp])
            {
                foreach (eYoonCognexType pKey in pResultTemplate.Keys)
                {
                    switch (pKey)
                    {
                        case eYoonCognexType.PMAlign:
                            foreach (string strTag in pResultTemplate[pKey].Keys)
                            {
                                DisplayFactory.Draw.DrawPatternMatchCross(pDisplay, pResultTemplate[pKey][strTag]);
                                DisplayFactory.Draw.DrawPatternMatchRect(pDisplay, pResultTemplate[pKey][strTag], "", false, true);
                            }
                            break;
                        case eYoonCognexType.Blob:
                            foreach (string strTag in pResultTemplate[pKey].Keys)
                                DisplayFactory.Draw.DrawBlobRect(pDisplay, pResultTemplate[pKey][strTag], 2500);
                            break;
                        default:
                            return false;
                    }
                }
            }
            return true;
        }


        public static CognexImage SetPatternMatchResultOverlap(CognexImage pImageSource, params eLabelInspect[] pArrayInspLabel)
        {
            if (pImageSource.Plane != 1) return null;

            CognexImage pImageResult = null;
            for (int iTool = 0; iTool < pArrayInspLabel.Length; iTool++)
            {
                eLabelInspect nLabel = pArrayInspLabel[iTool];
                if (nLabel != eLabelInspect.Main && nLabel != eLabelInspect.Second && nLabel != eLabelInspect.Third && nLabel != eLabelInspect.Forth)
                    continue;
                if (pCogResultTemplate[eTypeInspect.PatternMatching][eYoonCognexType.PMAlign].ContainsKey(nLabel.ToString()))
                {
                    CognexImage pImageProcessing = CognexFactory.Editor.EraseWithoutPatternMatchRegion(pImageSource, pCogResultTemplate[eTypeInspect.PatternMatching][eYoonCognexType.PMAlign][nLabel.ToString()]);
                    if (pImageProcessing == null) continue;
                    if (iTool == 0)
                        pImageResult = pImageProcessing.Clone() as CognexImage;
                    else
                        pImageResult = CognexFactory.TwoImageProcess.OverlapMax(pImageResult, pImageProcessing);
                }
                if (pImageResult == null) break;
            }
            return pImageResult;
        }

        public static bool ProcessPatternMatchOrigin(int nInspectedOrigin)
        {
            //// Process Parameter 초기화
            bool bRun = true, bResultOrigin = false;
            eTypeInspect nTypeInsp = eTypeInspect.PatternMatching;
            eStepOrigin nJobStep = eStepOrigin.Init;
            eStepOrigin nJobStepBK = eStepOrigin.None;
            ParameterInspectionPatternMatching pParamPM = null;

            while (bRun)
            {
                switch (nJobStep)
                {
                    case eStepOrigin.Init:
                        nJobStepBK = nJobStep;
                        pParamPM = pParamTemplate[nTypeInsp].Parameter as ParameterInspectionPatternMatching;
                        if (nInspectedOrigin > MAX_PATTERN_NUM || nInspectedOrigin <= 0)
                        {
                            nJobStep = eStepOrigin.Error;
                            break;
                        }
                        for (int iPattern = 0; iPattern < MAX_PATTERN_NUM; iPattern++)
                        {
                            pParamPM.IsOriginTeachedFlags[iPattern] = false;
                            pParamPM.OriginPixelXs[iPattern] = DEFAULT_POSITION;
                            pParamPM.OriginPixelYs[iPattern] = DEFAULT_POSITION;
                            pParamPM.OriginThetas[iPattern] = DEFAULT_POSITION;
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
                                else
                                    pParamPM.AlignType = eTypeAlign.OnePoint;
                                break;
                            case 3:
                            case 4:
                                if (pParamPM.IsUseMultiPatternAlign)
                                    pParamPM.AlignType = eTypeAlign.FourPoint;
                                else
                                    pParamPM.AlignType = eTypeAlign.OnePoint;
                                break;
                            default:
                                pParamPM.AlignType = eTypeAlign.None;
                                break;
                        }
                        if (pParamPM.AlignType == eTypeAlign.None)
                            nJobStep = eStepOrigin.Error;
                        else
                            nJobStep = eStepOrigin.Apply;
                        break;
                    case eStepOrigin.PatternMatching:
                        break;
                    case eStepOrigin.Apply:
                        nJobStepBK = nJobStep;
                        //// Teaching값 재등록
                        for (int iPattern = 0; iPattern < nInspectedOrigin; iPattern++)
                        {
                            CognexResult pResult = null;
                            switch (iPattern) // Pattern Count == 최대 4개 (0, 1, 2, 3)
                            {
                                case 0:
                                    pResult = pCogResultTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Main.ToString()];
                                    break;
                                case 1:
                                    pResult = pCogResultTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Second.ToString()];
                                    break;
                                case 2:
                                    pResult = pCogResultTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Third.ToString()];
                                    break;
                                case 3:
                                    pResult = pCogResultTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Forth.ToString()];
                                    break;
                                default:
                                    continue;
                            }
                            if (pResult.TotalScore > 0) // Save as Real Position
                            {
                                pParamPM.IsOriginTeachedFlags[iPattern] = true;
                                pParamPM.OriginPixelXs[iPattern] = pResult.GetPatternMatchPoint().X;
                                pParamPM.OriginPixelYs[iPattern] = pResult.GetPatternMatchPoint().Y;
                                pParamPM.OriginThetas[iPattern] = pResult.GetPatternRotation();
                            }
                        }
                        pParamTemplate[nTypeInsp].Parameter = pParamPM;
                        bResultOrigin = true;
                        nJobStep = eStepOrigin.Finish;
                        break;
                    case eStepOrigin.Error:
                        switch (nJobStepBK)
                        {
                            case eStepOrigin.Init:
                                CommonClass.pCLM.Write("Origin Process Init Failure");
                                CommonClass.pDLM.Write(eYoonStatus.Error, "Origin Init Failure");
                                break;
                            case eStepOrigin.Apply:
                                CommonClass.pCLM.Write("Origin Apply Failure");
                                CommonClass.pDLM.Write(eYoonStatus.Error, "PM Origin Failure");
                                break;
                        }
                        nJobStep = eStepOrigin.Finish;
                        bResultOrigin = false;
                        break;
                    case eStepOrigin.Finish:
                        bRun = false;
                        break;
                }
            }
            return bResultOrigin;
        }

        public static bool ProcessPreprocessing(CognexImage pImageSource, ref CognexImage pImageResult)
        {
            if (pImageSource == null) return false;

            //// Process Parameter 초기화
            bool bRun = true;
            bool bResult = true;
            string strErrorMessage = string.Empty;
            eTypeInspect nTypeInsp = eTypeInspect.Preprocessing;
            eStepPreprocessing nJobStep = eStepPreprocessing.Init;
            eStepPreprocessing nJobStepBK = eStepPreprocessing.None;
            CognexResult pResultInsp = null;
            ParameterInspectionPreprocessing pParam = null;
            CognexImage pImagePipeline = pImageSource.Clone() as CognexImage;

            while (bRun)
            {
                switch (nJobStep)
                {
                    case eStepPreprocessing.Init:
                        nJobStepBK = nJobStep;
                        pParam = pParamTemplate[nTypeInsp].Parameter as ParameterInspectionPreprocessing;
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
                            nJobStep = eStepPreprocessing.ImageFiltering;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (ToolFactory.ImageConvert(pCogToolTemplate[nTypeInsp][eYoonCognexType.Convert][string.Empty], pImageSource, ref strErrorMessage, ref pResultInsp))
                        {
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.Convert] = new ResultSection();
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.Convert][string.Empty] = pResultInsp.Clone() as CognexResult;
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
                        if (ToolFactory.ImageFiltering(pCogToolTemplate[nTypeInsp][eYoonCognexType.Filtering][string.Empty], pImagePipeline, ref strErrorMessage, ref pResultInsp))
                        {
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.Filtering] = new ResultSection();
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.Filtering][string.Empty] = pResultInsp.Clone() as CognexResult;
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
                        if (ToolFactory.SobelEdge(pCogToolTemplate[nTypeInsp][eYoonCognexType.Sobel][string.Empty], pImagePipeline, ref strErrorMessage, ref pResultInsp))
                        {
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.Sobel] = new ResultSection();
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.Sobel][string.Empty] = pResultInsp.Clone() as CognexResult;
                            pImagePipeline = pResultInsp.ResultImage;
                            nJobStep = eStepPreprocessing.Result;
                        }
                        else
                            nJobStep = eStepPreprocessing.Error;
                        break;
                    case eStepPreprocessing.Result:
                        if (nJobStepBK == eStepPreprocessing.Init)
                            bResult = false;
                        else
                        {
                            bResult = true;
                            pImageResult = pImagePipeline.Clone() as CognexImage;
                            pCLM.Write("Preprocess Success");
                            pDLM.Write(eYoonStatus.Inspect, "Preprocess Success");
                        }
                        pParamTemplate[nTypeInsp].Parameter = pParam;
                        pCogResultTemplate[nTypeInsp].SaveTemplate();
                        nJobStep = eStepPreprocessing.Finish;
                        break;
                    case eStepPreprocessing.Error:
                        switch (nJobStepBK)
                        {
                            case eStepPreprocessing.Init:
                                pCLM.Write("Preprocess Init Failure");
                                pDLM.Write(eYoonStatus.Error, "Preprocess Init Failure");
                                break;
                            case eStepPreprocessing.Convert:
                                pCLM.Write("Preprocess Convert Failure >>" + strErrorMessage);
                                pDLM.Write(eYoonStatus.Error, "Preprocess Convert Failure");
                                break;
                            case eStepPreprocessing.ImageFiltering:
                                pCLM.Write("Preprocess IPOneImage Failure >>" + strErrorMessage);
                                pDLM.Write(eYoonStatus.Error, "Preprocess Image Filtering Failure");
                                break;
                            case eStepPreprocessing.Sobel:
                                pCLM.Write("Preprocess Sobel Failure >>" + strErrorMessage);
                                pDLM.Write(eYoonStatus.Error, "Preprocess Sobel Failure");
                                break;
                        }
                        bResult = false;
                        nJobStep = eStepPreprocessing.Finish;
                        break;
                    case eStepPreprocessing.Finish:
                        bRun = false;
                        break;
                }
            }
            return bResult;
        }

        public static int ProcessPatternMatchAlign(CognexImage pImageSource, ref CognexImage pImageResult, ref YoonVector2D pVecResult, ref double dThetaResult, ref AlignResult pAlignResult)
        {
            if (pImageSource == null) return -1;
            int iPatternInspected = -1;

            //// Process Parameter 초기화
            bool bRun = true;
            string strErrorMessage = string.Empty;
            eTypeInspect nTypeInsp = eTypeInspect.PatternMatching;
            eStepPatternMatching nJobStep = eStepPatternMatching.Init;
            eStepPatternMatching nJobStepBK = eStepPatternMatching.None;
            CognexResult pResultInsp = null;
            bool[] pArrayFlagResultContain = null;
            YoonVector2D[] pArrayResultVec = null;
            double[] pArrayTheta = null;
            ParameterInspectionPatternMatching pParam = null;

            while (bRun)
            {
                switch (nJobStep)
                {
                    case eStepPatternMatching.Init:
                        nJobStepBK = nJobStep;
                        pParam = pParamTemplate[nTypeInsp].Parameter as ParameterInspectionPatternMatching;
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
                        pCogResultTemplate[nTypeInsp][eYoonCognexType.PMAlign] = new ResultSection();
                        nJobStep = eStepPatternMatching.MainPattern;
                        break;
                    case eStepPatternMatching.MainPattern:
                        if (!pParam.IsUseEachPatterns[eLabelInspect.Main.ToInt()])
                        {
                            nJobStep = eStepPatternMatching.SecondPattern;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (ToolFactory.PMAlign(pCogToolTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Main.ToString()], pImageSource, ref strErrorMessage, ref pResultInsp))
                        {
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Main.ToString()] = pResultInsp.Clone() as CognexResult;
                            pArrayFlagResultContain[eLabelInspect.Main.ToInt()] = true;
                            pArrayResultVec[eLabelInspect.Main.ToInt()] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[eLabelInspect.Main.ToInt()] = pResultInsp.GetPatternRotation();
                            nJobStep = eStepPatternMatching.SecondPattern;
                            iPatternInspected++;
                        }
                        else
                            nJobStep = eStepPatternMatching.SubPattern1;
                        break;
                    case eStepPatternMatching.SubPattern1:
                        if (!pParam.IsUseEachPatterns[eLabelInspect.Second.ToInt()])
                        {
                            nJobStep = eStepPatternMatching.SubPattern2;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (ToolFactory.PMAlign(pCogToolTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Second.ToString()], pImageSource, ref strErrorMessage, ref pResultInsp))
                        {
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Second.ToString()] = pResultInsp.Clone() as CognexResult;
                            pArrayFlagResultContain[eLabelInspect.Second.ToInt()] = true;
                            pArrayResultVec[eLabelInspect.Second.ToInt()] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[eLabelInspect.Second.ToInt()] = pResultInsp.GetPatternRotation();
                            nJobStep = eStepPatternMatching.Align;
                            iPatternInspected++;
                        }
                        else
                            nJobStep = eStepPatternMatching.SubPattern2;
                        break;
                    case eStepPatternMatching.SubPattern2:
                        if (!pParam.IsUseEachPatterns[eLabelInspect.Third.ToInt()])
                        {
                            nJobStep = eStepPatternMatching.SubPattern3;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (ToolFactory.PMAlign(pCogToolTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Third.ToString()], pImageSource, ref strErrorMessage, ref pResultInsp))
                        {
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Third.ToString()] = pResultInsp.Clone() as CognexResult;
                            pArrayFlagResultContain[eLabelInspect.Third.ToInt()] = true;
                            pArrayResultVec[eLabelInspect.Third.ToInt()] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[eLabelInspect.Third.ToInt()] = pResultInsp.GetPatternRotation();
                            nJobStep = eStepPatternMatching.Align;
                            iPatternInspected++;
                        }
                        else
                            nJobStep = eStepPatternMatching.SubPattern3;
                        break;
                    case eStepPatternMatching.SubPattern3:
                        if (!pParam.IsUseEachPatterns[eLabelInspect.Forth.ToInt()])
                        {
                            nJobStep = eStepPatternMatching.Error;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (ToolFactory.PMAlign(pCogToolTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Forth.ToString()], pImageSource, ref strErrorMessage, ref pResultInsp))
                        {
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Forth.ToString()] = pResultInsp.Clone() as CognexResult;
                            pArrayFlagResultContain[eLabelInspect.Forth.ToInt()] = true;
                            pArrayResultVec[eLabelInspect.Forth.ToInt()] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[eLabelInspect.Forth.ToInt()] = pResultInsp.GetPatternRotation();
                            nJobStep = eStepPatternMatching.Align;
                            iPatternInspected++;
                        }
                        else
                            nJobStep = eStepPatternMatching.Error;
                        break;
                    case eStepPatternMatching.SecondPattern:
                        if (!pParam.IsUseMultiPatternInspection || !pParam.IsUseEachPatterns[eLabelInspect.Second.ToInt()])
                        {
                            nJobStep = eStepPatternMatching.ThridPattern;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (ToolFactory.PMAlign(pCogToolTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Second.ToString()], pImageSource, ref strErrorMessage, ref pResultInsp))
                        {
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Second.ToString()] = pResultInsp.Clone() as CognexResult;
                            pArrayFlagResultContain[eLabelInspect.Second.ToInt()] = true;
                            pArrayResultVec[eLabelInspect.Second.ToInt()] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[eLabelInspect.Second.ToInt()] = pResultInsp.GetPatternRotation();
                            nJobStep = eStepPatternMatching.ThridPattern;
                            iPatternInspected++;
                        }
                        else
                            nJobStep = eStepPatternMatching.Align;
                        break;
                    case eStepPatternMatching.ThridPattern:
                        if (!pParam.IsUseMultiPatternInspection || !pParam.IsUseEachPatterns[eLabelInspect.Third.ToInt()])
                        {
                            nJobStep = eStepPatternMatching.ForthPattern;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (ToolFactory.PMAlign(pCogToolTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Third.ToString()], pImageSource, ref strErrorMessage, ref pResultInsp))
                        {
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Third.ToString()] = pResultInsp.Clone() as CognexResult;
                            pArrayFlagResultContain[eLabelInspect.Third.ToInt()] = true;
                            pArrayResultVec[eLabelInspect.Third.ToInt()] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[eLabelInspect.Third.ToInt()] = pResultInsp.GetPatternRotation();
                            nJobStep = eStepPatternMatching.ForthPattern;
                            iPatternInspected++;
                        }
                        else
                            nJobStep = eStepPatternMatching.Align;
                        break;
                    case eStepPatternMatching.ForthPattern:
                        if (!pParam.IsUseMultiPatternInspection || !pParam.IsUseEachPatterns[eLabelInspect.Forth.ToInt()])
                        {
                            nJobStep = eStepPatternMatching.Align;
                            break;
                        }
                        nJobStepBK = nJobStep;
                        if (ToolFactory.PMAlign(pCogToolTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Forth.ToString()], pImageSource, ref strErrorMessage, ref pResultInsp))
                        {
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.PMAlign][eLabelInspect.Forth.ToString()] = pResultInsp.Clone() as CognexResult;
                            pArrayFlagResultContain[eLabelInspect.Forth.ToInt()] = true;
                            pArrayResultVec[eLabelInspect.Forth.ToInt()] = pResultInsp.GetPatternMatchPoint().Clone() as YoonVector2D;
                            pArrayTheta[eLabelInspect.Forth.ToInt()] = pResultInsp.GetPatternRotation();
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
                                        dOffsetX += (pArrayResultVec[iPattern].X - pParam.OriginPixelXs[iPattern]);
                                        dOffsetY += (pArrayResultVec[iPattern].Y - pParam.OriginPixelYs[iPattern]);
                                        dOffsetT += (pArrayTheta[iPattern] - pParam.OriginThetas[iPattern]);
                                    }
                                }
                                pAlignResult.X = dOffsetX / iPatternInspected;
                                pAlignResult.Y = dOffsetY / iPatternInspected;
                                pAlignResult.Theta = dOffsetT / iPatternInspected;
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
                                break;
                            case eStepPatternMatching.MainPattern:
                                pImageResult = SetPatternMatchResultOverlap(pImageSource, eLabelInspect.Main);
                                pVecResult = pArrayResultVec[eLabelInspect.Main.ToInt()] as YoonVector2D;
                                break;
                            case eStepPatternMatching.SecondPattern:
                                pImageResult = SetPatternMatchResultOverlap(pImageSource, eLabelInspect.Main, eLabelInspect.Second);
                                pVecResult = pArrayResultVec[eLabelInspect.Main.ToInt()] as YoonVector2D;
                                break;
                            case eStepPatternMatching.ThridPattern:
                                pImageResult = SetPatternMatchResultOverlap(pImageSource, eLabelInspect.Main, eLabelInspect.Second, eLabelInspect.Third);
                                pVecResult = pArrayResultVec[eLabelInspect.Main.ToInt()] as YoonVector2D;
                                break;
                            case eStepPatternMatching.ForthPattern:
                                pImageResult = SetPatternMatchResultOverlap(pImageSource, eLabelInspect.Main, eLabelInspect.Second, eLabelInspect.Third, eLabelInspect.Forth);
                                pVecResult = pArrayResultVec[eLabelInspect.Main.ToInt()] as YoonVector2D;
                                break;
                            case eStepPatternMatching.SubPattern1:
                                pImageResult = SetPatternMatchResultOverlap(pImageSource, eLabelInspect.Second);
                                pVecResult = pArrayResultVec[eLabelInspect.Second.ToInt()] as YoonVector2D;
                                break;
                            case eStepPatternMatching.SubPattern2:
                                pImageResult = SetPatternMatchResultOverlap(pImageSource, eLabelInspect.Third);
                                pVecResult = pArrayResultVec[eLabelInspect.Third.ToInt()] as YoonVector2D;
                                break;
                            case eStepPatternMatching.SubPattern3:
                                pImageResult = SetPatternMatchResultOverlap(pImageSource, eLabelInspect.Forth);
                                pVecResult = pArrayResultVec[eLabelInspect.Forth.ToInt()] as YoonVector2D;
                                break;
                            default:
                                break;
                        }
                        pCLM.Write("Pattern Match Success");
                        pDLM.Write(eYoonStatus.Inspect, "Pattern Match Success");
                        nJobStep = eStepPatternMatching.Finish;
                        break;
                    case eStepPatternMatching.Error:
                        switch (nJobStepBK)
                        {
                            case eStepPatternMatching.Init:
                                pCLM.Write("Pattern Match Init Failure");
                                pDLM.Write(eYoonStatus.Error, "Pattern Match Init Failure");
                                break;
                            case eStepPatternMatching.MainPattern:
                            case eStepPatternMatching.SubPattern1:
                            case eStepPatternMatching.SubPattern2:
                            case eStepPatternMatching.SubPattern3:
                                pCLM.Write("Pattern Match Failure >>" + strErrorMessage);
                                pDLM.Write(eYoonStatus.Error, "Pattern Match Failure");
                                break;
                        }
                        iPatternInspected = -1;
                        nJobStep = eStepPatternMatching.Finish;
                        break;
                    case eStepPatternMatching.Finish:
                        pParamTemplate[nTypeInsp].Parameter = pParam;
                        pCogResultTemplate[nTypeInsp].SaveTemplate();
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

        public static bool ProcessObjectExtract(CognexImage pImageSourceBlob, CognexImage pImageSourceColorExtract, ref CognexImage pImageResult)    // Blob : 0, ColorExtract : 1
        {
            if (pImageSourceColorExtract == null || pImageSourceBlob == null) return false;

            //// Process Parameter 초기화
            bool bRun = true;
            bool bResult = false;
            string strErrorMessage = string.Empty;
            eTypeInspect nTypeInsp = eTypeInspect.ObjectExtract;
            eStepObjectExtract nJobStep = eStepObjectExtract.Init;
            eStepObjectExtract nJobStepBK = eStepObjectExtract.None;
            CognexResult pResultInsp = null;
            ParameterInspectionObjectExtract pParam = null;
            CognexImage pImageBlob = null;
            CognexImage pImageColorSegment = null;
            //// Combine 관련 변수 초기화
            bool bInspectBlob = false;
            CognexImage pImageCombine = null;

            while (bRun)
            {
                switch (nJobStep)
                {
                    case eStepObjectExtract.Init:
                        nJobStepBK = nJobStep;
                        pParam = pParamTemplate[nTypeInsp].Parameter as ParameterInspectionObjectExtract;
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
                        if (ToolFactory.Blob(pCogToolTemplate[nTypeInsp][eYoonCognexType.Blob][string.Empty], pImageSourceBlob, ref strErrorMessage, ref pResultInsp))
                        {
                            bInspectBlob = true;
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.Blob] = new ResultSection();
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.Blob][string.Empty] = pResultInsp.Clone() as CognexResult;
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
                        if (ToolFactory.ColorSegment(pCogToolTemplate[nTypeInsp][eYoonCognexType.ColorSegment][string.Empty], pImageSourceColorExtract, ref strErrorMessage, ref pResultInsp))
                        {
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.ColorSegment] = new ResultSection();
                            pCogResultTemplate[nTypeInsp][eYoonCognexType.ColorSegment][string.Empty] = pResultInsp.Clone() as CognexResult;
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
                                pImageCombine = CognexFactory.TwoImageProcess.Add(pImageBlob, pImageColorSegment);
                                break;
                            case eTypeProcessTwoImage.OverlapMax:
                                pImageCombine = CognexFactory.TwoImageProcess.OverlapMax(pImageBlob, pImageColorSegment);
                                break;
                            case eTypeProcessTwoImage.OverlapMin:
                                pImageCombine = CognexFactory.TwoImageProcess.OverlapMin(pImageBlob, pImageColorSegment);
                                break;
                            case eTypeProcessTwoImage.Subtract:
                                pImageCombine = CognexFactory.TwoImageProcess.Subtract(pImageBlob, pImageColorSegment);
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
                                bResult = false;
                                break;
                            case eStepObjectExtract.Blob:
                            case eStepObjectExtract.ColorSegment:
                            case eStepObjectExtract.Combine:
                                bResult = true;
                                break;
                        }
                        pParam.IsPassRecently = bResult;
                        pCLM.Write("Object Extract Success");
                        pDLM.Write(eYoonStatus.Inspect, "Object Extract Success");
                        nJobStep = eStepObjectExtract.Finish;
                        break;
                    case eStepObjectExtract.Error:
                        switch (nJobStepBK)
                        {
                            case eStepObjectExtract.Init:
                                pCLM.Write("Object Extract Init Failure");
                                pDLM.Write(eYoonStatus.Error, "Object Extract Init Failure");
                                break;
                            case eStepObjectExtract.Blob:
                                pCLM.Write("Blob Inspect Failure");
                                pDLM.Write(eYoonStatus.Error, "Blob Inspect Failure");
                                break;
                            case eStepObjectExtract.ColorSegment:
                                pCLM.Write("Color Segmentation Failure");
                                pDLM.Write(eYoonStatus.Error, "Color Segmentation Failure");
                                break;
                            case eStepObjectExtract.Combine:
                                pCLM.Write("Image Combine Failure");
                                pDLM.Write(eYoonStatus.Error, "Image Combine Failure");
                                break;
                        }
                        bResult = false;
                        nJobStep = eStepObjectExtract.Finish;
                        break;
                    case eStepObjectExtract.Finish:
                        pParamTemplate[nTypeInsp].Parameter = pParam;
                        pCogResultTemplate[nTypeInsp].SaveTemplate();
                        bRun = false;
                        break;
                }
            }
            return bResult;
        }

        public static bool ProcessImageCombine(CognexImage pImageSourceA, CognexImage pImageSourceB, ref CognexImage pImageResult)
        {
            if (pImageSourceA == null || pImageSourceB == null) return false;

            ////  현재 Inspection 값 취득
            bool bRun = true;
            bool bResult = false;
            string strErrorMessage = string.Empty;
            eTypeInspect nTypeInsp = eTypeInspect.Combine;
            eStepCombine nJobStep = eStepCombine.Init;
            eStepCombine nJobStepBK = eStepCombine.None;
            ParameterInspectionCombine pParam = null;

            while (bRun)
            {
                switch (nJobStep)
                {
                    case eStepCombine.Init:
                        nJobStepBK = nJobStep;
                        pParam = pParamTemplate[nTypeInsp].Parameter as ParameterInspectionCombine;
                        if (!pParam.IsUse)
                        {
                            nJobStep = eStepCombine.Finish;
                            break;
                        }
                        nJobStep = eStepCombine.Processing;
                        break;
                    case eStepCombine.Processing:
                        nJobStepBK = nJobStep;
                        eYoonCognexType nTypeCognex = eYoonCognexType.None;
                        switch (pParam.CombineType)
                        {
                            case eTypeProcessTwoImage.Add:
                                nTypeCognex = eYoonCognexType.ImageAdd;
                                pImageResult = CognexFactory.TwoImageProcess.Add(pImageSourceA, pImageSourceB);
                                pCogResultTemplate[nTypeInsp][nTypeCognex][string.Empty] = new CognexResult(nTypeCognex, pImageResult);
                                break;
                            case eTypeProcessTwoImage.OverlapMax:
                                nTypeCognex = eYoonCognexType.ImageMinMax;
                                pImageResult = CognexFactory.TwoImageProcess.OverlapMax(pImageSourceA, pImageSourceB);
                                pCogResultTemplate[nTypeInsp][nTypeCognex][string.Empty] = new CognexResult(nTypeCognex, pImageResult);
                                break;
                            case eTypeProcessTwoImage.OverlapMin:
                                nTypeCognex = eYoonCognexType.ImageMinMax;
                                pImageResult = CognexFactory.TwoImageProcess.OverlapMin(pImageSourceA, pImageSourceB);
                                pCogResultTemplate[nTypeInsp][nTypeCognex][string.Empty] = new CognexResult(nTypeCognex, pImageResult);
                                break;
                            case eTypeProcessTwoImage.Subtract:
                                nTypeCognex = eYoonCognexType.ImageSubtract;
                                pImageResult = CognexFactory.TwoImageProcess.Subtract(pImageSourceA, pImageSourceB);
                                pCogResultTemplate[nTypeInsp][nTypeCognex][string.Empty] = new CognexResult(nTypeCognex, pImageResult);
                                break;
                            default:
                                break;
                        }
                        if (pImageResult == null)
                            nJobStep = eStepCombine.Error;
                        else
                        {
                            pCLM.Write("Image Combine Success");
                            pDLM.Write(eYoonStatus.Inspect, "Image Combine Success");
                            bResult = true;
                            nJobStep = eStepCombine.Finish;
                        }
                        break;
                    case eStepCombine.Error:
                        switch (nJobStepBK)
                        {
                            case eStepCombine.Init:
                                pCLM.Write("Image Combine Init Failure");
                                pDLM.Write(eYoonStatus.Error, "Image Combine Init Failure");
                                break;
                            case eStepCombine.Processing:
                                pCLM.Write("Image Combine Failure");
                                pDLM.Write(eYoonStatus.Error, "Image Combine Failure");
                                break;
                        }
                        bResult = false;
                        nJobStep = eStepCombine.Finish;
                        break;
                    case eStepCombine.Finish:
                        pParam.IsPassRecently = bResult;
                        pParamTemplate[nTypeInsp].Parameter = pParam;
                        pCogResultTemplate[nTypeInsp].SaveTemplate();
                        bRun = false;
                        break;
                }
            }
            return bResult;
        }
    }
}