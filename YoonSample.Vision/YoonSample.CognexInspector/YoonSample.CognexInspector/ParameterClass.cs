using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using YoonFactory;
using Cognex.VisionPro;
using YoonFactory.Cognex;

namespace YoonSample.CognexInspector
{
    public class ParameterInspectionPreprocessing : IYoonParameter
    {
        [CategoryAttribute("Common"), DescriptionAttribute("Whether to use Preprocessing (true/false)")]
        public bool IsUse { get; set; } = true;
        [CategoryAttribute("Common"), DescriptionAttribute("Whether to use Image Convert (true/false)")]
        public bool IsUseImageConvert { get; set; } = false;
        [CategoryAttribute("Common"), DescriptionAttribute("Whether to use Sobel Edge (true/false)")]
        public bool IsUseSobelEdge { get; set; } = false;
        [CategoryAttribute("Common"), DescriptionAttribute("Whether to use Image Filtering (true/false)")]
        public bool IsUseImageFilter { get; set; } = false;

        public bool IsEqual(IYoonParameter pParam)
        {
            if(pParam is ParameterInspectionPreprocessing pParamPre)
            {
                if (IsUse == pParamPre.IsUse &&
                    IsUseImageConvert == pParamPre.IsUseImageConvert &&
                    IsUseSobelEdge == pParamPre.IsUseSobelEdge &&
                    IsUseImageFilter == pParamPre.IsUseImageFilter)
                    return true;
            }
            return false;
        }

        public void CopyFrom(IYoonParameter pParam)
        {
            if (pParam is ParameterInspectionPreprocessing pParamPre)
            {
                IsUse = pParamPre.IsUse;
                IsUseImageConvert = pParamPre.IsUseImageConvert;
                IsUseSobelEdge = pParamPre.IsUseSobelEdge;
                IsUseImageFilter = pParamPre.IsUseImageFilter;
            }
        }

        public IYoonParameter Clone()
        {
            ParameterInspectionPreprocessing pParam = new ParameterInspectionPreprocessing();
            pParam.IsUse = IsUse;
            pParam.IsUseImageFilter = IsUseImageFilter;
            pParam.IsUseImageConvert = IsUseImageConvert;
            pParam.IsUseSobelEdge = IsUseSobelEdge;
            return pParam;
        }
    }

    public class ParameterInspectionPatternMatching : IYoonParameter
    {
        [CategoryAttribute("Common"), DescriptionAttribute("Whether to use Pattern Match (true/false)")]
        public bool IsUse { get; set; } = true;
        [CategoryAttribute("Common"), DescriptionAttribute("Whether to use each patterns (true/false)")]
        public bool[] IsUseEachPatterns { get; set; } = new bool[CommonClass.MAX_PATTERN_NUM];
        [CategoryAttribute("Setting"), DescriptionAttribute("Whether to inspect multi Pattern (true/false)")]
        public bool IsUseMultiPatternInspection { get; set; } = false;
        [CategoryAttribute("Image"), DescriptionAttribute("Selected Source Image")]
        public eLevelImageSelection SelectedSourceLevel { get; set; } = eLevelImageSelection.Origin;
        [Browsable(false)]
        public int SelectedSourceNo { get; set; } = 0;
        [Browsable(false)]
        public eTypeInspect SelectedSourceType { get; set; } = eTypeInspect.None;
        [CategoryAttribute("Align"), DescriptionAttribute("whether to use Align (true/false)")]
        public bool IsCheckAlign { get; set; } = false;
        [CategoryAttribute("Align"), DescriptionAttribute("whether to Multi-Align (true/false)")]
        public bool IsUseMultiPatternAlign { get; set; } = false;
        [Browsable(false)]
        public eTypeAlign AlignType { get; set; } = eTypeAlign.None;
        [Browsable(false)]
        public bool[] IsOriginTeachedFlags { get; set; } = new bool[CommonClass.MAX_PATTERN_NUM];
        [Browsable(false)]
        public double[] OriginPixelXs { get; set; } = new double[CommonClass.MAX_PATTERN_NUM];
        [Browsable(false)]
        public double[] OriginPixelYs { get; set; } = new double[CommonClass.MAX_PATTERN_NUM];
        [Browsable(false)]
        public double[] OriginRealXs { get; set; } = new double[CommonClass.MAX_PATTERN_NUM];
        [Browsable(false)]
        public double[] OriginRealYs { get; set; } = new double[CommonClass.MAX_PATTERN_NUM];
        [Browsable(false)]
        public double[] OriginThetas { get; set; } = new double[CommonClass.MAX_PATTERN_NUM];
        [CategoryAttribute("Align"), DescriptionAttribute("Offset length referenced X-Axis (mm)")]
        public double OffsetX { get; set; } = 100.0;    // 현재는 Pixel, 향후에 Pixel -> mm로 전환할 것
        [CategoryAttribute("Align"), DescriptionAttribute("Offset length referenced Y-Axis (mm)")]
        public double OffsetY { get; set; } = 100.0;    // 현재는 Pixel, 향후에 Pixel -> mm로 전환할 것
        [CategoryAttribute("Align"), DescriptionAttribute("Offset degree referenced Theta (degree)")]
        public double OffsetT { get; set; } = 100.0;
        [CategoryAttribute("Align"), DescriptionAttribute("Handling NG Condition with 'OR' component (true/false)")]
        public bool IsAlignCheckWithOR { get; set; } = true;

        public bool IsEqual(IYoonParameter pParam)
        {
            if (pParam is ParameterInspectionPatternMatching pParamPattern)
            {
                if (IsUse == pParamPattern.IsUse &&
                    IsUseEachPatterns == pParamPattern.IsUseEachPatterns &&
                    IsUseMultiPatternInspection == pParamPattern.IsUseMultiPatternInspection &&
                    SelectedSourceLevel == pParamPattern.SelectedSourceLevel &&
                    SelectedSourceNo == pParamPattern.SelectedSourceNo &&
                    SelectedSourceType == pParamPattern.SelectedSourceType &&
                    IsCheckAlign == pParamPattern.IsCheckAlign &&
                    IsUseMultiPatternAlign == pParamPattern.IsUseMultiPatternAlign &&
                    AlignType == pParamPattern.AlignType &&

                    )
                    return true;
            }
            return false;
        }

        public void CopyFrom(IYoonParameter pParam)
        {
            if (pParam is ParameterInspectionPatternMatching pParamPre)
            {
                IsUse = pParamPre.IsUse;
                IsUseImageConvert = pParamPre.IsUseImageConvert;
                IsUseSobelEdge = pParamPre.IsUseSobelEdge;
                IsUseImageFilter = pParamPre.IsUseImageFilter;
            }
        }

        public IYoonParameter Clone()
        {
            ParameterInspectionPatternMatching pParam = new ParameterInspectionPatternMatching();
            pParam.IsUse = IsUse;
            pParam.IsUseImageFilter = IsUseImageFilter;
            pParam.IsUseImageConvert = IsUseImageConvert;
            pParam.IsUseSobelEdge = IsUseSobelEdge;
            return pParam;
        }
    }

    public class ParameterInspectionObjectExtract : IYoonParameter
    {
        [CategoryAttribute("Common"), DescriptionAttribute("Object Extract 사용 여부 (true/false)")]
        public bool IsUse { get; set; } = true;

        [CategoryAttribute("Common"), DescriptionAttribute("Blob 사용 여부 (true/false)")]
        public bool IsUseBlob { get; set; } = false;
        [CategoryAttribute("Common"), DescriptionAttribute("Color 추출 사용 여부 (true/false)")]
        public bool IsUseColorSegment { get; set; } = false;
        [Browsable(false)]
        public eTypeProcessTwoImage CombineType { get; set; } = eTypeProcessTwoImage.OverlapMin;
        [CategoryAttribute("Image"), DescriptionAttribute("Blob용 Image 선택")]
        public eLevelImageSelection SelectedBlobImageLevel { get; set; } = eLevelImageSelection.Origin;
        [Browsable(false)]
        public int SelectedBlobImageNo { get; set; } = 0;
        [Browsable(false)]
        public eTypeInspect SelectedBlobImageType { get; set; } = eTypeInspect.None;
        [CategoryAttribute("Image"), DescriptionAttribute("Color 추출용 Image 선택")]
        public eLevelImageSelection SelectedColorSegmentImageLevel { get; set; } = eLevelImageSelection.Origin;
        [Browsable(false)]
        public int SelectedColorSegmentImageNo { get; set; } = 0;
        [Browsable(false)]
        public eTypeInspect SelectedColorSegmentImageType { get; set; } = eTypeInspect.None;
    }

    public class ParameterInspectionCombine : IYoonParameter
    {
        [CategoryAttribute("Common"), DescriptionAttribute("Combine 사용 여부 (true/false)")]
        public bool IsUse { get; set; } = true;
        [CategoryAttribute("Setting"), DescriptionAttribute("Combine 설정")]
        public eTypeProcessTwoImage CombineType { get; set; } = eTypeProcessTwoImage.OverlapMin;
        [CategoryAttribute("Image"), DescriptionAttribute("Source용 Image 선택")]
        public eLevelImageSelection SelectedSourceLevel { get; set; } = eLevelImageSelection.Origin;
        [Browsable(false)]
        public int SelectedSourceNo { get; set; } = 0;
        [Browsable(false)]
        public eTypeInspect SelectedSourceType { get; set; } = eTypeInspect.None;
        [CategoryAttribute("Image"), DescriptionAttribute("Object용 Image 선택")]
        public eLevelImageSelection SelectedObjectLevel { get; set; } = eLevelImageSelection.Origin;
        [Browsable(false)]
        public int SelectedObjectNo { get; set; } = 0;
        [Browsable(false)]
        public eTypeInspect SelectedObjectType { get; set; } = eTypeInspect.None;
    }

}
