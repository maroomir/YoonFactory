namespace YoonFactory
{
    public enum eYoonCamera
    {
        None,
        BaslerMono,
        BaslerColor,
        RealsenseColor,
        RealsenseDepth,
    }
    public enum eYoonRSCaptureMode
    {
        None = -1,
        RGBMono,
        RGBColor,
        Depth,
        DepthColorize,
    }

    public enum eYoonRSAlignMode
    {
        None = -1,
        ToRGB,
        ToDepth,
    }
}
