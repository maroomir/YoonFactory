using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using YoonFactory;
using YoonFactory.Align;
using YoonFactory.Cognex;
using YoonFactory.Cognex.Tool;
using YoonFactory.Log;
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

    public delegate void PassRequestCallback(object sender, EventArgs e);

    public static class CommonClass
    {
        public const int DEFULAT_IMAGE_WIDTH = 640;
        public const int DEFULAT_IMAGE_HEIGHT = 480;
        public const int MAX_SOURCE_NUM = 4;
        public const int MAX_PATTERN_NUM = 4;
        public const double DEFAULT_POSITION = -10000.0;

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

        public static ICogImage GetResultImage(int nInspNo, eTypeInspect nInspType)
        {
            if (nInspNo == 0 && nInspType == eTypeInspect.None) return null;

            //// Result Image 가져오기
            switch (nInspType)
            {
                case eTypeInspect.Preprocessing:
                case eTypeInspect.PatternMatching:
                case eTypeInspect.ObjectExtract:
                case eTypeInspect.Combine:
                    if (pCogResultTemplate[nInspType].GetLastResultImage() as CogImage8Grey != null)
                        return pCogResultTemplate[nInspType].GetLastResultImage().CopyBase(CogImageCopyModeConstants.CopyPixels);
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
                        CognexFactory.Draw.DrawCross(pDisplay, CogColorConstants.Red, pParam.OriginPixelXs[nTag.ToInt()], pParam.OriginPixelYs[nTag.ToInt()], 2.0);
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
                                CognexFactory.Draw.DrawPatternMatchCross(pDisplay, pResultTemplate[pKey][strTag]);
                                CognexFactory.Draw.DrawPatternMatchRect(pDisplay, pResultTemplate[pKey][strTag], "", false, true);
                            }
                            break;
                        case eYoonCognexType.Blob:
                            foreach (string strTag in pResultTemplate[pKey].Keys)
                                CognexFactory.Draw.DrawBlobRect(pDisplay, pResultTemplate[pKey][strTag], 2500);
                            break;
                        default:
                            return false;
                    }
                }
            }
            return true;
        }

        public static bool ProcessPatternMatchOrigin(int nInspectedOrigin)
        {
            return false;
        }

        public static bool ProcessPreprocessing(ICogImage pImageSource, ref ICogImage pImageResult)
        {
            return false;
        }

        public static int ProcessPatternMatchAlign(CogImage8Grey pImageSource, ref CogImage8Grey pImageResult, ref YoonVector2D pVecResult, ref double dThetaResult, ref AlignResult pAlignResult)
        {
            return 0;
        }

        public static bool ProcessObjectExtract(CogImage8Grey pImageSourceBlob, CogImage24PlanarColor pImageSourceColorExtract, ref ICogImage pImageResult)    // Blob : 0, ColorExtract : 1
        {
            return false;
        }

        public static bool ProcessImageCombine(ICogImage pImageSourceA, ICogImage pImageSourceB, ref ICogImage pImageResult)
        {
            return false;
        }
    }
}