using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonSample.CognexInspector
{
    public enum eTypeInspect : int
    {
        None = -1,
        Preprocessing,
        PatternMatching,
        ObjectExtract,
        Combine,
    }

    static class eTypeInspectMethod
    {
        public static int ToInt(this eTypeInspect e)
        {
            return (int)e;
        }
    }

    public enum eLabelInspect : int
    {
        None = -1,
        Main = 0,
        Second = 1,
        Third = 2,
        Forth = 3,
        Align,
        Calibration,
        Reference,
    }

    static class eLabelInspectMethod
    {
        public static int ToInt(this eLabelInspect e)
        {
            return (int)e;
        }
    }

    public enum eTypeProcessTwoImage : int
    {
        None = -1,
        Add,
        Subtract,
        OverlapMax,
        OverlapMin,
    }

    public enum eTypeAlign : int
    {
        None = -1,
        OnePoint,
        TwoPoint,
        FourPoint,
    }

    public enum eLevelImageSelection : int
    {
        None = -1,
        Origin,
        CurrentProcessing,
        Custom,
    }

    public enum eStepOrigin : int
    {
        None = -1,
        Init,
        PatternMatching,
        Apply,
        Error,
        Finish,
    }

    public enum eStepPreprocessing : int
    {
        None = -1,
        Init,
        Convert,
        ImageFiltering,
        Sobel,
        Result,
        Error,
        Finish,
    }

    public enum eStepPatternMatching : int
    {
        None = -1,
        Init,
        MainPattern,
        SubPattern1,
        SubPattern2,
        SubPattern3,
        SecondPattern,
        ThridPattern,
        ForthPattern,
        Align,
        Result,
        Error,
        Finish,
    }

    public enum eStepObjectExtract : int
    {
        None = -1,
        Init,
        Blob,
        ColorSegment,
        Combine,
        Result,
        Error,
        Finish,
    }

    public enum eStepCombine : int
    {
        None = -1,
        Init,
        Processing,
        Error,
        Finish,
    }
}
