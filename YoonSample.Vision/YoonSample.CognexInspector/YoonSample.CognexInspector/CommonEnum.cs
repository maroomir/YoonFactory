using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonSample.CognexInspector
{
    public enum eDirView : int
    {
        None = -1,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
    }

    public enum eModeDrive : int
    {
        None = -1,
        Auto,
        Manual,
        Simulate,
    }

    public enum eTagResultDisp : int
    {
        None = -1,
        OK,
        NG,
        ERROR,
    }

    public enum eTypeSolution : int
    {
        None = -1,
        Barista = 0,
        Noddle,
        Fried,
        Beer,
    }

    public enum eTypeView : int
    {
        None = -1,
        TopCameraColor = 0,
        TopCameraDepth,
        TopCameraInspection,
        RobotArmCameraColor,
        RobotArmCameraDepth,
        RobotArmInspection,
        RealCameraColor,
        RealCameraDepth,
        RealCameraInspection,
        ResultByFlow,
        ResultByInspection,
        ResultByDepth,
        DriveStatus,
        FlowStatus,
    }

    public enum eTypeCameraDevice : int
    {
        None = -1,
        Realsense,
        Balser,
        Sentech,
    }

    public enum eTypeCaptureMethod : int
    {
        None = -1,
        TopView,
        RobotArm,
        RealView,
        Reserve,
    }

    public enum eTypeCalibrationMethod : int
    {
        None = -1,
        Static,
        Rotation,
        CheckerBoard,
    }

    public enum eTypeRecipe : int
    {
        None = -1,
        Model,
        Job,
        Inspection,
    }

    public enum eTypeImage : int
    {
        None = -1,
        RGB,
        Mono,
        DepthColorize,
        DepthMono,
    }

    public enum eTypeRemoteController : int
    {
        None = -1,
        ToRobot,
        ToComputer,
        ToPLC,
    }

    public enum eTypeJobOrder : int
    {
        None = -1,  // Do Nothing
        Calibration,    // Calibration that Point
        Origin,         // Origin for Align
        Inspection,     // Inspection for Align or other image processing
        Continuous,     // Monitoring as if it works pop-up
    }

    public enum eLabelRemoteSend : int
    {
        None = -1,
        CurrentJob,
        JobOrder,
        Status,
        OffsetX,
        OffsetY,
        OffsetT,
        RealX,
        RealY,
        RealZ,
        RealRX,
        RealRY,
        RealRZ,
        PixelCount,
        Depth,
        MaxDepth,
        MinDepth,
        NextJob,
    }

    public enum eLabelRemoteReceive : int
    {
        None = -1,
        CurrentJob,
        JobOrder,
        Joint1,
        Joint2,
        Joint3,
        Joint4,
        Joint5,
        Joint6,
        CartX,
        CartY,
        CartZ,
        CartRX,
        CartRY,
        CartRZ,
        NextJob,
    }

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

    public enum eTypeGrid : int
    {
        None = -1,
        BY2,
        BY3,
        BY4,
        BY5,
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

    public enum eStepAuto : int
    {
        Rest = -2,
        Wait = -1,
        OpenConnection,
        SendQuickly,
        SendAndReceive,
        DecodeMessage,
        DistributeJob,
        DoCalibration,
        DoOrigin,
        DoInspection,
        KeepInspection,
        Release,
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

    public enum eStepDepth : int
    {
        None = -1,
        Init,
        Combine,
        Error,
        Finish,
    }

    public enum eStepUndistort : int
    {
        None = -1,
        Init,
        Undistort,
        Error,
        Finish,
    }
}
