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
    public class InspectionInfo : IYoonParameter
    {
        public int No { get; set; } = 0;
        public eTypeInspect InspectType { get; set; } = eTypeInspect.Preprocessing;
        public bool InspectResult { get; set; } = false;
        [Browsable(false)]
        public IParameterInspection InspectionParam { get; set; } = null;
        [Browsable(false)]
        public ICogImage[] SourceImages { get; set; } = new ICogImage[CommonClass.MAX_SOURCE_NUM];
        [Browsable(false)]
        public ICogImage ResultImage { get; set; } = new CogImage24PlanarColor();
        [Browsable(false)]
        public CogToolContainer CogToolContainer { get; set; } = new CogToolContainer();
        [Browsable(false)]
        public CogResultContainer CogResultContainer { get; set; } = new CogResultContainer();
    }

    public class ParameterInspectionPreprocessing : IParameterInspection
    {
        [CategoryAttribute("Common"), DescriptionAttribute("전처리 검사 사용 여부 (true/false)")]
        public bool IsUse { get; set; } = true;
        [CategoryAttribute("Common"), DescriptionAttribute("Image Convert 사용 여부 (true/false)")]
        public bool IsUseImageConvert { get; set; } = false;
        [CategoryAttribute("Common"), DescriptionAttribute("Sobel Edge 사용 여부 (true/false)")]
        public bool IsUseSobelEdge { get; set; } = false;
        [CategoryAttribute("Common"), DescriptionAttribute("Image Filtering 사용 여부 (true/false)")]
        public bool IsUseImageFilter { get; set; } = false;
    }

    public class ParameterInspectionPatternMatching : IParameterInspection
    {
        [CategoryAttribute("Common"), DescriptionAttribute("Pattern 검사 사용 여부 (true/false)")]
        public bool IsUse { get; set; } = true;
        [CategoryAttribute("Common"), DescriptionAttribute("각 Pattern들의 사용 여부 (true/false)")]
        public bool[] IsUseEachPatterns { get; set; } = new bool[CommonClass.MAX_PATTERN_NUM];
        [CategoryAttribute("Setting"), DescriptionAttribute("Multi Pattern 검사 여부 (true/false)")]
        public bool IsUseMultiPatternInspection { get; set; } = false;
        [CategoryAttribute("Image"), DescriptionAttribute("Source Image 선택")]
        public eLevelImageSelection SelectedSourceLevel { get; set; } = eLevelImageSelection.Origin;
        [Browsable(false)]
        public int SelectedSourceNo { get; set; } = 0;
        [Browsable(false)]
        public eTypeInspect SelectedSourceType { get; set; } = eTypeInspect.None;
        [CategoryAttribute("Align"), DescriptionAttribute("Align 검사 여부 (true/false)")]
        public bool IsCheckAlign { get; set; } = false;
        [CategoryAttribute("Align"), DescriptionAttribute("Align 검사에 Multi-Align 사용 여부 (true/false)")]
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
        [CategoryAttribute("Align"), DescriptionAttribute("X 기준 Align 틀어짐 정도 (mm)")]
        public double OffsetX { get; set; } = 100.0;    // 현재는 Pixel, 향후에 Pixel -> mm로 전환할 것
        [CategoryAttribute("Align"), DescriptionAttribute("Y 기준 Align 틀어짐 정도 (mm)")]
        public double OffsetY { get; set; } = 100.0;    // 현재는 Pixel, 향후에 Pixel -> mm로 전환할 것
        [CategoryAttribute("Align"), DescriptionAttribute("Theta 기준 Align 틀어짐 정도 (degree)")]
        public double OffsetT { get; set; } = 100.0;
        [CategoryAttribute("Align"), DescriptionAttribute("Align NG 조건을 OR로 처리 (true/false)")]
        public bool IsAlignCheckWithOR { get; set; } = true;
    }

    public class ParameterInspectionObjectExtract : IParameterInspection
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

    public class ParameterInspectionCombine : IParameterInspection
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
