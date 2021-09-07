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
        [Browsable(false)]
        public bool IsPassRecently { get; set; } = true;

        public int GetLength()
        {
            return typeof(ParameterInspectionPreprocessing).GetProperties().Length;
        }

        public void Set(params string[] pArgs)
        {
            if (pArgs.Length != GetLength()) return;
            IsUse = Boolean.Parse(pArgs[0]);
            IsUseImageConvert = Boolean.Parse(pArgs[1]);
            IsUseSobelEdge = Boolean.Parse(pArgs[2]);
            IsUseImageFilter = Boolean.Parse(pArgs[3]);
            IsPassRecently = Boolean.Parse(pArgs[4]);
        }

        public bool Equals(IYoonParameter pParam)
        {
            if(pParam is ParameterInspectionPreprocessing pParamPre)
            {
                if (IsUse == pParamPre.IsUse &&
                    IsPassRecently == pParamPre.IsPassRecently &&
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
                IsPassRecently = pParamPre.IsPassRecently;
                IsUseImageConvert = pParamPre.IsUseImageConvert;
                IsUseSobelEdge = pParamPre.IsUseSobelEdge;
                IsUseImageFilter = pParamPre.IsUseImageFilter;
            }
        }

        public IYoonParameter Clone()
        {
            ParameterInspectionPreprocessing pParam = new ParameterInspectionPreprocessing();
            pParam.IsUse = IsUse;
            pParam.IsPassRecently = IsPassRecently;
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
        public bool IsPassRecently { get; set; } = true;
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
        public double[] OriginThetas { get; set; } = new double[CommonClass.MAX_PATTERN_NUM];
        [CategoryAttribute("Align"), DescriptionAttribute("Offset length referenced X-Axis (mm)")]
        public double OffsetX { get; set; } = 100.0;    // 현재는 Pixel, 향후에 Pixel -> mm로 전환할 것
        [CategoryAttribute("Align"), DescriptionAttribute("Offset length referenced Y-Axis (mm)")]
        public double OffsetY { get; set; } = 100.0;    // 현재는 Pixel, 향후에 Pixel -> mm로 전환할 것
        [CategoryAttribute("Align"), DescriptionAttribute("Offset degree referenced Theta (degree)")]
        public double OffsetT { get; set; } = 100.0;
        [CategoryAttribute("Align"), DescriptionAttribute("Handling NG Condition with 'OR' component (true/false)")]
        public bool IsAlignCheckWithOR { get; set; } = true;

        public int GetLength()
        {
            return typeof(ParameterInspectionPatternMatching).GetProperties().Length;
        }

        public void Set(params string[] pArgs)
        {
            if (pArgs.Length != GetLength()) return;
            throw new NotSupportedException("[Parameter] Forbidden to Direct Setting");
        }

        public bool Equals(IYoonParameter pParam)
        {
            if (pParam is ParameterInspectionPatternMatching pParamPattern)
            {
                if (IsUse == pParamPattern.IsUse &&
                    IsPassRecently == pParamPattern.IsPassRecently &&
                    IsUseEachPatterns == pParamPattern.IsUseEachPatterns &&
                    IsUseMultiPatternInspection == pParamPattern.IsUseMultiPatternInspection &&
                    SelectedSourceLevel == pParamPattern.SelectedSourceLevel &&
                    SelectedSourceNo == pParamPattern.SelectedSourceNo &&
                    SelectedSourceType == pParamPattern.SelectedSourceType &&
                    IsCheckAlign == pParamPattern.IsCheckAlign &&
                    IsUseMultiPatternAlign == pParamPattern.IsUseMultiPatternAlign &&
                    AlignType == pParamPattern.AlignType &&
                    Array.Equals(IsOriginTeachedFlags, pParamPattern.IsOriginTeachedFlags) &&
                    Array.Equals(OriginPixelXs, pParamPattern.OriginPixelXs) &&
                    Array.Equals(OriginPixelYs, pParamPattern.OriginPixelYs) &&
                    Array.Equals(OriginThetas, pParamPattern.OriginThetas) &&
                    OffsetX == pParamPattern.OffsetX &&
                    OffsetY == pParamPattern.OffsetY &&
                    OffsetT == pParamPattern.OffsetT &&
                    IsAlignCheckWithOR == pParamPattern.IsAlignCheckWithOR
                    )
                    return true;
            }
            return false;
        }

        public void CopyFrom(IYoonParameter pParam)
        {
            if (pParam is ParameterInspectionPatternMatching pParamPattern)
            {
                IsUse = pParamPattern.IsUse;
                IsPassRecently = pParamPattern.IsPassRecently;
                IsUseEachPatterns = pParamPattern.IsUseEachPatterns;
                IsUseMultiPatternInspection = pParamPattern.IsUseMultiPatternInspection;
                SelectedSourceLevel = pParamPattern.SelectedSourceLevel;
                SelectedSourceNo = pParamPattern.SelectedSourceNo;
                SelectedSourceType = pParamPattern.SelectedSourceType;
                IsCheckAlign = pParamPattern.IsCheckAlign;
                IsUseMultiPatternAlign = pParamPattern.IsUseMultiPatternAlign;
                AlignType = pParamPattern.AlignType;
                Array.Copy(pParamPattern.IsOriginTeachedFlags, IsOriginTeachedFlags, CommonClass.MAX_PATTERN_NUM);
                Array.Copy(pParamPattern.OriginPixelXs, OriginPixelXs, CommonClass.MAX_PATTERN_NUM);
                Array.Copy(pParamPattern.OriginPixelYs, OriginPixelYs, CommonClass.MAX_PATTERN_NUM);
                Array.Copy(pParamPattern.OriginThetas, OriginThetas, CommonClass.MAX_PATTERN_NUM);
                OffsetX = pParamPattern.OffsetX;
                OffsetY = pParamPattern.OffsetY;
                OffsetT = pParamPattern.OffsetT;
                IsAlignCheckWithOR = pParamPattern.IsAlignCheckWithOR;
            }
        }

        public IYoonParameter Clone()
        {
            ParameterInspectionPatternMatching pParam = new ParameterInspectionPatternMatching();
            pParam.IsUse = IsUse;
            pParam.IsPassRecently = IsPassRecently;
            pParam.IsUseEachPatterns = IsUseEachPatterns;
            pParam.IsUseMultiPatternInspection = IsUseMultiPatternInspection;
            pParam.SelectedSourceLevel = SelectedSourceLevel;
            pParam.SelectedSourceNo = SelectedSourceNo;
            pParam.SelectedSourceType = SelectedSourceType;
            pParam.IsCheckAlign = IsCheckAlign;
            pParam.IsUseMultiPatternAlign = IsUseMultiPatternAlign;
            pParam.AlignType = AlignType;
            Array.Copy(IsOriginTeachedFlags, pParam.IsOriginTeachedFlags, CommonClass.MAX_PATTERN_NUM);
            Array.Copy(OriginPixelXs, pParam.OriginPixelXs, CommonClass.MAX_PATTERN_NUM);
            Array.Copy(OriginPixelYs, pParam.OriginPixelYs, CommonClass.MAX_PATTERN_NUM);
            Array.Copy(OriginThetas, pParam.OriginThetas, CommonClass.MAX_PATTERN_NUM);
            pParam.OffsetX = OffsetX;
            pParam.OffsetY = OffsetY;
            pParam.OffsetT = OffsetT;
            pParam.IsAlignCheckWithOR = IsAlignCheckWithOR;
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
        public bool IsPassRecently { get; set; } = true;
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

        public int GetLength()
        {
            return typeof(ParameterInspectionObjectExtract).GetProperties().Length;
        }

        public void Set(params string[] pArgs)
        {
            if (pArgs.Length != GetLength()) return;
            throw new NotSupportedException("[Parameter] Forbidden to Direct Setting");
        }

        public bool Equals(IYoonParameter pParam)
        {
            if (pParam is ParameterInspectionObjectExtract pParamObjExtract)
            {
                if (IsUse == pParamObjExtract.IsUse &&
                    IsPassRecently == pParamObjExtract.IsPassRecently &&
                    IsUseBlob == pParamObjExtract.IsUseBlob &&
                    IsUseColorSegment == pParamObjExtract.IsUseColorSegment &&
                    CombineType == pParamObjExtract.CombineType &&
                    SelectedBlobImageLevel == pParamObjExtract.SelectedBlobImageLevel &&
                    SelectedBlobImageNo == pParamObjExtract.SelectedBlobImageNo &&
                    SelectedBlobImageType == pParamObjExtract.SelectedBlobImageType &&
                    SelectedColorSegmentImageLevel == pParamObjExtract.SelectedColorSegmentImageLevel &&
                    SelectedColorSegmentImageNo == pParamObjExtract.SelectedColorSegmentImageNo &&
                    SelectedColorSegmentImageType == pParamObjExtract.SelectedColorSegmentImageType
                    )
                    return true;
            }
            return false;
        }

        public void CopyFrom(IYoonParameter pParam)
        {
            if (pParam is ParameterInspectionObjectExtract pParamObjExtract)
            {
                IsUse = pParamObjExtract.IsUse;
                IsPassRecently = pParamObjExtract.IsPassRecently;
                IsUseBlob = pParamObjExtract.IsUseBlob;
                IsUseColorSegment = pParamObjExtract.IsUseColorSegment;
                CombineType = pParamObjExtract.CombineType;
                SelectedBlobImageLevel = pParamObjExtract.SelectedBlobImageLevel;
                SelectedBlobImageNo = pParamObjExtract.SelectedBlobImageNo;
                SelectedBlobImageType = pParamObjExtract.SelectedBlobImageType;
                SelectedColorSegmentImageLevel = pParamObjExtract.SelectedColorSegmentImageLevel;
                SelectedColorSegmentImageNo = pParamObjExtract.SelectedColorSegmentImageNo;
                SelectedColorSegmentImageType = pParamObjExtract.SelectedColorSegmentImageType;
            }
        }

        public IYoonParameter Clone()
        {
            ParameterInspectionObjectExtract pParam = new ParameterInspectionObjectExtract();
            pParam.IsUse = IsUse;
            pParam.IsPassRecently = IsPassRecently;
            pParam.IsUseBlob = IsUseBlob;
            pParam.IsUseColorSegment = IsUseColorSegment;
            pParam.CombineType = CombineType;
            pParam.SelectedBlobImageLevel = SelectedBlobImageLevel;
            pParam.SelectedBlobImageNo = SelectedBlobImageNo;
            pParam.SelectedBlobImageType = SelectedBlobImageType;
            pParam.SelectedColorSegmentImageLevel = SelectedColorSegmentImageLevel;
            pParam.SelectedColorSegmentImageNo = SelectedColorSegmentImageNo;
            pParam.SelectedColorSegmentImageType = SelectedColorSegmentImageType;
            return pParam;
        }
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
        public bool IsPassRecently { get; set; } = true;
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

        public int GetLength()
        {
            return typeof(ParameterInspectionCombine).GetProperties().Length;
        }

        public void Set(params string[] pArgs)
        {
            if (pArgs.Length != GetLength()) return;
            throw new NotSupportedException("[Parameter] Forbidden to Direct Setting");
        }

        public bool Equals(IYoonParameter pParam)
        {
            if (pParam is ParameterInspectionCombine pParamCombine)
            {
                if (IsUse == pParamCombine.IsUse &&
                    IsPassRecently == pParamCombine.IsPassRecently &&
                    CombineType == pParamCombine.CombineType &&
                    SelectedSourceLevel == pParamCombine.SelectedSourceLevel &&
                    SelectedSourceNo == pParamCombine.SelectedSourceNo &&
                    SelectedSourceType == pParamCombine.SelectedSourceType &&
                    SelectedObjectLevel == pParamCombine.SelectedObjectLevel &&
                    SelectedObjectNo == pParamCombine.SelectedObjectNo &&
                    SelectedObjectType == pParamCombine.SelectedObjectType
                    )
                    return true;
            }
            return false;
        }

        public void CopyFrom(IYoonParameter pParam)
        {
            if (pParam is ParameterInspectionCombine pParamCombine)
            {
                IsUse = pParamCombine.IsUse;
                IsPassRecently = pParamCombine.IsPassRecently;
                CombineType = pParamCombine.CombineType;
                SelectedSourceLevel = pParamCombine.SelectedSourceLevel;
                SelectedSourceNo = pParamCombine.SelectedSourceNo;
                SelectedSourceType = pParamCombine.SelectedSourceType;
                SelectedObjectLevel = pParamCombine.SelectedObjectLevel;
                SelectedObjectNo = pParamCombine.SelectedObjectNo;
                SelectedObjectType = pParamCombine.SelectedObjectType;
            }
        }

        public IYoonParameter Clone()
        {
            ParameterInspectionCombine pParam = new ParameterInspectionCombine();
            pParam.IsUse = IsUse;
            pParam.IsPassRecently = IsPassRecently;
            pParam.CombineType = CombineType;
            pParam.SelectedSourceLevel = SelectedSourceLevel;
            pParam.SelectedSourceNo = SelectedSourceNo;
            pParam.SelectedSourceType = SelectedSourceType;
            pParam.SelectedObjectLevel = SelectedObjectLevel;
            pParam.SelectedObjectNo = SelectedObjectNo;
            pParam.SelectedObjectType = SelectedObjectType;
            return pParam;
        }
    }
}
