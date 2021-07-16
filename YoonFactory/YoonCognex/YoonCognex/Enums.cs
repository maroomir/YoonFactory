namespace YoonFactory
{
    public enum eYoonCognexType : int
    {
        None = -1,
        // Calibration (Undistort)
        Calibration,
        // Image 전처리
        Convert,
        Sobel,
        Sharpness,
        Filtering,
        // 추출
        Blob,
        ColorExtract,
        ColorSegment,
        // 패턴 매칭
        PMAlign,
        // 찾기 (Line, Circle 등)
        LineFitting,
        // 영상 비교
        ImageSubtract,
        ImageAdd,
        ImageMinMax,
    }

}
