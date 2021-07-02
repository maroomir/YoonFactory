using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.PMAlign;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using YoonFactory.Image;
using YoonFactory.Cognex.Result;

namespace YoonFactory.Cognex
{
    public static class CognexFactory
    {
        public static CognexImage CropImage(this CognexImage pSourceImage, double dCenterX, double dCenterY, double dCropWidth, double dCropHeight) => Transform.CropImage(pSourceImage, dCenterX, dCenterY, dCropWidth, dCropHeight);
        public static CognexImage CropImage(this CognexImage pSourceImage, IYoonRect pRect) => Transform.CropImage(pSourceImage, pRect);
        public static CognexImage ResizeImage(this CognexImage pSourceImage, double dSourceWidth, double dSourceHeight) => Transform.ResizeImage(pSourceImage, dSourceWidth, dSourceHeight);
        public static CognexImage ResizeImage(this CognexImage pSourceImage, IYoonRect pRect) => Transform.ResizeImage(pSourceImage, pRect);
        public static YoonObject<YoonRect2D> CropPattern(this CognexImage pSourceImage, YoonRect2D pRect) => PatternMatch.CropPattern(pSourceImage, pRect);
        public static YoonObject<YoonRect2D> CropPattern(this CognexImage pSourceImage, YoonRect2D pRect, YoonVector2D pOriginPos) => PatternMatch.CropPattern(pSourceImage, pRect);
        public static CognexResult FindPattern(CognexImage pSourceImage, YoonObject<YoonRect2D> pPatternObject, double dMatchThreshold) => PatternMatch.FindPattern(pSourceImage, pPatternObject, dMatchThreshold);
        public static CognexImage Subtract(this CognexImage pSourceImage, CognexImage pObjectImage) => TwoImageProcess.Subtract(pSourceImage, pObjectImage);
        public static CognexImage Add(this CognexImage pSourceImage, CognexImage pObjectImage) => TwoImageProcess.Add(pSourceImage, pObjectImage);
        public static CognexImage OverlapMax(this CognexImage pSourceImage, CognexImage pObjectImage) => TwoImageProcess.OverlapMax(pSourceImage, pObjectImage);
        public static CognexImage OverlapMin(this CognexImage pSourceImage, CognexImage pObjectImage) => TwoImageProcess.OverlapMin(pSourceImage, pObjectImage);
        public static CognexImage EraseRect(this CognexImage pSourceImage, IYoonRect2D<double> pRect) => Editor.EraseRect(pSourceImage, pRect);
        public static CognexImage EraseRect(this CognexImage pSourceImage, double dCenterX, double dCenterY, double dRectWidth, double dRectHeight) => Editor.EraseRect(pSourceImage, dCenterX, dCenterY, dRectWidth, dRectHeight);
        public static CognexImage EraseWithoutRect(this CognexImage pSourceImage, IYoonRect2D<double> pRect) => Editor.EraseWithoutRect(pSourceImage, pRect);
        public static CognexImage EraseWithoutRect(this CognexImage pSourceImage, double dCenterX, double dCenterY, double dRectWidth, double dRectHeight) => Editor.EraseWithoutRect(pSourceImage, dCenterX, dCenterY, dRectWidth, dRectHeight);
        public static CognexImage EraseWithoutPatternMatchRegion(this CognexImage pSourceImage, CognexResult pResult) => Editor.EraseWithoutPatternMatchRegion(pSourceImage, pResult);

        public static class Converter
        {
            public static CogImage8Grey ToImage8Grey(IntPtr pBufferAddress, int nWidth, int nHeight)
            {
                if (pBufferAddress == IntPtr.Zero) return null;

                byte[] pBuffer = new byte[nWidth * nHeight];
                Marshal.Copy(pBufferAddress, pBuffer, 0, nWidth * nHeight);
                return ToImage8Grey(pBuffer, nWidth, nHeight);
            }

            public static CogImage8Grey ToImage8Grey(byte[] pBuffer, int nWidth, int nHeight)
            {
                if (pBuffer == null || pBuffer.Length != nWidth * nHeight) return null;

                CogImage8Grey cogImage = new CogImage8Grey();
                try
                {
                    cogImage.Allocate(nWidth, nHeight);
                    ICogImage8PixelMemory cogMemory = cogImage.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, nWidth, nHeight);
                    Marshal.Copy(pBuffer, 0, cogMemory.Scan0, cogMemory.Width * cogMemory.Height);

                    cogImage.Copy(CogImageCopyModeConstants.CopyPixels);
                    cogMemory.Dispose();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    return null;
                }
                return cogImage;
            }

            public static CogImage24PlanarColor ToImage24PlanarColorWithMixed(IntPtr pBufferAddress, int nWidth, int nHeight, bool bRGBOrder = true)
            {
                if (pBufferAddress == IntPtr.Zero) return null;

                byte[] pBuffer = new byte[nWidth * nHeight * 3];
                Marshal.Copy(pBufferAddress, pBuffer, 0, nWidth * nHeight * 3);
                return ToImage24PlanarColorWithMixed(pBuffer, nWidth, nHeight, bRGBOrder);
            }

            public static CogImage24PlanarColor ToImage24PlanarColorWithParallel(IntPtr pBufferAddress, int nWidth, int nHeight, bool bRGBOrder = true)
            {
                if (pBufferAddress == IntPtr.Zero) return null;

                byte[] pBuffer = new byte[nWidth * nHeight * 3];
                Marshal.Copy(pBufferAddress, pBuffer, 0, nWidth * nHeight * 3);
                return ToImage24PlanarColorWithParallel(pBuffer, nWidth, nHeight, bRGBOrder);
            }

            public static CogImage24PlanarColor ToImage24PlanarColorWithMixed(byte[] pBuffer, int nWidth, int nHeight, bool bRGBOrder = true)
            {
                if (pBuffer == null || pBuffer.Length != nWidth * nHeight * 3) return null;

                int nRed, nGreen, nBlue;
                if (bRGBOrder) { nRed = 0; nGreen = 1; nBlue = 2; }
                else { nRed = 2; nGreen = 1; nBlue = 0; }

                CogImage24PlanarColor cogImage = new CogImage24PlanarColor();
                try
                {
                    ICogImage8PixelMemory[] cogMemoryPlaneArray = new ICogImage8PixelMemory[3];
                    cogImage.Allocate(nWidth, nHeight);
                    cogImage.Get24PlanarColorPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, nWidth, nHeight, out cogMemoryPlaneArray[nRed], out cogMemoryPlaneArray[nGreen], out cogMemoryPlaneArray[nBlue]);
                    for (int iByte = 0; iByte < nWidth * nHeight * 3; iByte++)
                    {
                        //IntPtr pAddress = new IntPtr(cogMemoryPlaneArray[iByte % nPlane].Scan0.ToInt64() + iByte / nPlane);
                        Marshal.WriteByte(cogMemoryPlaneArray[iByte % 3].Scan0, iByte / 3, pBuffer[iByte]);
                    }
                    cogImage.Copy(CogImageCopyModeConstants.CopyPixels);

                    for (int iMemory = 0; iMemory < 3; iMemory++)
                    {
                        cogMemoryPlaneArray[iMemory].Dispose();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    return null;
                }
                return cogImage;
            }

            public static CogImage24PlanarColor ToImage24PlanarColorWithParallel(byte[] pBuffer, int nWidth, int nHeight, bool bRGBOrder = true)
            {
                if (pBuffer == null || pBuffer.Length != nWidth * nHeight * 3) return null;

                int nRed, nGreen, nBlue;
                if (bRGBOrder) { nRed = 0; nGreen = 1; nBlue = 2; }
                else { nRed = 2; nGreen = 1; nBlue = 0; }

                CogImage24PlanarColor cogImage = new CogImage24PlanarColor();
                try
                {
                    ICogImage8PixelMemory[] cogMemoryPlaneArray = new ICogImage8PixelMemory[3];
                    cogImage.Allocate(nWidth, nHeight);
                    cogImage.Get24PlanarColorPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, nWidth, nHeight, out cogMemoryPlaneArray[nRed], out cogMemoryPlaneArray[nGreen], out cogMemoryPlaneArray[nBlue]);
                    for (int iMemory = 0; iMemory < 3; iMemory++)
                    {
                        Marshal.Copy(pBuffer, 0, cogMemoryPlaneArray[iMemory].Scan0, cogMemoryPlaneArray[iMemory].Width * cogMemoryPlaneArray[iMemory].Height);
                    }
                    cogImage.Copy(CogImageCopyModeConstants.CopyPixels);

                    for (int iMemory = 0; iMemory < 3; iMemory++)
                    {
                        cogMemoryPlaneArray[iMemory].Dispose();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    return null;
                }
                return cogImage;
            }

            public static CogImage24PlanarColor ToImage24PlanarColor(byte[] pBufferRed, byte[] pBufferGreen, byte[] pBufferBlue, int nWidth, int nHeight)
            {
                if (pBufferRed == null || pBufferGreen == null || pBufferBlue == null) return null;

                CogImage24PlanarColor cogImage = new CogImage24PlanarColor();
                try
                {
                    ICogImage8PixelMemory[] cogMemoryPlaneArray = new ICogImage8PixelMemory[3];

                    cogImage.Allocate(nWidth, nHeight);
                    cogImage.Get24PlanarColorPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, nWidth, nHeight, out cogMemoryPlaneArray[0], out cogMemoryPlaneArray[1], out cogMemoryPlaneArray[2]);  // Plane 0 : RED, Plane 1 : Green, Plane 2 : Blue
                    Marshal.Copy(pBufferRed, 0, cogMemoryPlaneArray[0].Scan0, cogMemoryPlaneArray[0].Width * cogMemoryPlaneArray[0].Height);
                    Marshal.Copy(pBufferGreen, 0, cogMemoryPlaneArray[1].Scan0, cogMemoryPlaneArray[1].Width * cogMemoryPlaneArray[1].Height);
                    Marshal.Copy(pBufferBlue, 0, cogMemoryPlaneArray[2].Scan0, cogMemoryPlaneArray[2].Width * cogMemoryPlaneArray[2].Height);
                    cogImage.Copy(CogImageCopyModeConstants.CopyPixels);

                    for (int iMemory = 0; iMemory < 3; iMemory++)
                    {
                        cogMemoryPlaneArray[iMemory].Dispose();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    return null;
                }
                return cogImage;
            }
        }

        public static class PatternMatch
        {
            public static YoonObject<YoonRect2D> CropPattern(CognexImage pSourceImage, YoonRect2D pRect)
            {
                YoonObject<YoonRect2D> pObject = new YoonObject<YoonRect2D>();
                {
                    pObject.Label = 0;
                    pObject.Object = pRect.Clone() as YoonRect2D;
                    pObject.ObjectImage = pSourceImage.CropImage(pRect);
                    pObject.ReferencePosition = pRect.CenterPos.Clone();
                }
                return pObject;
            }

            public static YoonObject<YoonRect2D> CropPattern(CognexImage pSourceImage, YoonRect2D pRect, YoonVector2D pOriginPos)
            {
                YoonObject<YoonRect2D> pObject = new YoonObject<YoonRect2D>();
                {
                    pObject.Label = 0;
                    pObject.Object = pRect.Clone() as YoonRect2D;
                    pObject.ObjectImage = pSourceImage.CropImage(pRect);
                    pObject.ReferencePosition = pOriginPos.Clone();
                }
                return pObject;
            }

            public static CogPMAlignPattern GetPatternParam(CognexImage pPatternImage, YoonVector2D pOriginPos,
                bool bAutoLimited = true, bool bAutoThreshold = true, double dCoarseLimit = 10.0, double dFineLimit = 3.0, double dThreshold = 30.0)
            {
                return GetPatternParam(pPatternImage.ToCogImage(), pOriginPos.X, pOriginPos.Y, bAutoLimited, bAutoThreshold, dCoarseLimit, dFineLimit, dThreshold);
            }

            public static CogPMAlignPattern GetPatternParam(CognexImage pPatternImage, eYoonDir2D nDir,
                bool bAutoLimited = true, bool bAutoThreshold = true, double dCoarseLimit = 10.0, double dFineLimit = 3.0, double dThreshold = 30.0)
            {
                YoonRect2D pRectArea = pPatternImage.Area.ToRect2D();
                return GetPatternParam(pPatternImage.ToCogImage(), pRectArea.GetPosition(nDir).X, pRectArea.GetPosition(nDir).Y, bAutoLimited, bAutoThreshold, dCoarseLimit, dFineLimit, dThreshold);
            }

            public static CogPMAlignPattern GetPatternParam(ICogImage cogPatternImage, double dOriginX, double dOriginY,
                bool bAutoLimited = true, bool bAutoThreshold = true, double dCoarseLimit = 10.0, double dFineLimit = 3.0, double dThreshold = 30.0)
            {
                ////  Pattern 영역 및 원점 설정
                CogPMAlignPattern cogPattern = new CogPMAlignPattern();
                {
                    //////  Pattern 설정
                    cogPattern.TrainImage = cogPatternImage;
                    cogPattern.TrainRegion = null;  // 전체 영역
                    cogPattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;
                    //////  Origin 설정
                    cogPattern.Origin.TranslationX = dOriginX;
                    cogPattern.Origin.TranslationY = dOriginY;
                    //////  TrainParam 설정
                    cogPattern.TrainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatMaxAndPatQuick;
                    cogPattern.TrainMode = CogPMAlignTrainModeConstants.Image;
                    cogPattern.GrainLimitAutoSelect = bAutoLimited;
                    if (bAutoLimited == false)
                    {
                        cogPattern.GrainLimitCoarse = dCoarseLimit;
                        cogPattern.GrainLimitFine = dFineLimit;
                    }
                    cogPattern.AutoEdgeThresholdEnabled = bAutoThreshold;
                    if (bAutoThreshold == false) cogPattern.EdgeThreshold = dThreshold;
                }

                ////  Train
                try
                {
                    cogPattern.Train();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (cogPattern.TrainImage != null) return cogPattern;
                else return null;
            }

            public static CogPMAlignPattern GetPatternParam(ICogImage cogPatternImage, ICogRegion cogRegion, double dOriginX, double dOriginY,
                bool bAutoLimited = true, bool bAutoThreshold = true, double dCoarseLimit = 10.0, double dFineLimit = 3.0, double dThreshold = 30.0)
            {
                ////  Pattern 영역 및 원점 설정
                CogPMAlignPattern cogPattern = new CogPMAlignPattern();
                {
                    //////  Pattern 설정
                    cogPattern.TrainImage = cogPatternImage;
                    cogPattern.TrainRegion = cogRegion;
                    cogPattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;
                    //////  Origin 설정
                    cogPattern.Origin.TranslationX = dOriginX;
                    cogPattern.Origin.TranslationY = dOriginY;
                    //////  TrainParam 설정
                    cogPattern.TrainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatMaxAndPatQuick;
                    cogPattern.TrainMode = CogPMAlignTrainModeConstants.Image;
                    cogPattern.GrainLimitAutoSelect = bAutoLimited;
                    if (bAutoLimited == false)
                    {
                        cogPattern.GrainLimitCoarse = dCoarseLimit;
                        cogPattern.GrainLimitFine = dFineLimit;
                    }
                    cogPattern.AutoEdgeThresholdEnabled = bAutoThreshold;
                    if (bAutoThreshold == false) cogPattern.EdgeThreshold = dThreshold;
                }

                ////  Train
                try
                {
                    cogPattern.Train();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (cogPattern.TrainImage != null) return cogPattern;
                else return null;
            }

            public static CognexResult FindPattern(CognexImage pSourceImage, YoonObject<YoonRect2D> pPatternObject, double dMatchThreshold,
                bool bAutoLimited = true, bool bAutoThreshold = true, double dCoarseLimit = 10.0, double dFineLimit = 3.0, double dThreshold = 30.0,
                bool bUseZoneAngle = true, bool bUseZoneScale = true, double dZoneAngleLow = 0.1, double dZoneAngleHigh = 2.0, double dZoneScaleLow = 0.1, double dZoneScaleHigh = 2.0)
            {
                if (pSourceImage == null || pPatternObject == null) return null;

                int nTrainedWidth = 0;
                int nTrainedHeight = 0;
                CognexResult pResult = null;
                ////  Pattern 영역 및 원점 설정
                CogPMAlignPattern cogPatternParam = new CogPMAlignPattern();
                {
                    //////  Parameter 가져오기
                    CognexImage pPatternImage = pPatternObject.ObjectImage as CognexImage;
                    YoonVector2D pOriginPos = pPatternObject.ReferencePosition as YoonVector2D;
                    //////  Pattern 설정
                    cogPatternParam.TrainImage = pPatternImage.ToCogImage();
                    cogPatternParam.TrainRegion = null;  // 전체 영역
                    cogPatternParam.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;
                    //////  Origin 설정
                    cogPatternParam.Origin.TranslationX = pOriginPos.X;
                    cogPatternParam.Origin.TranslationY = pOriginPos.Y;
                    //////  TrainParam 설정
                    cogPatternParam.TrainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatMaxAndPatQuick;
                    cogPatternParam.TrainMode = CogPMAlignTrainModeConstants.Image;
                    cogPatternParam.GrainLimitAutoSelect = bAutoLimited;
                    if (bAutoLimited == false)
                    {
                        cogPatternParam.GrainLimitCoarse = dCoarseLimit;
                        cogPatternParam.GrainLimitFine = dFineLimit;
                    }
                    cogPatternParam.AutoEdgeThresholdEnabled = bAutoThreshold;
                    if (bAutoThreshold == false) cogPatternParam.EdgeThreshold = dThreshold;
                }
                ////  PMAlign Parameter 설정
                CogPMAlignRunParams cogMatchParam = new CogPMAlignRunParams();
                {
                    cogMatchParam.ApproximateNumberToFind = 1;
                    cogMatchParam.AcceptThreshold = dMatchThreshold;
                    //////  Zone Angle 틀어짐 범위 설정
                    if (bUseZoneAngle == true)
                    {
                        cogMatchParam.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;
                        cogMatchParam.ZoneAngle.Low = dZoneAngleLow;
                        cogMatchParam.ZoneAngle.High = dZoneAngleHigh;
                    }
                    else
                    {
                        cogMatchParam.ZoneAngle.Configuration = CogPMAlignZoneConstants.Nominal;
                        cogMatchParam.ZoneAngle.Nominal = 0.0;
                    }
                    //////  Zone Scale 크기변환 범위 설정
                    if (bUseZoneScale == true)
                    {
                        cogMatchParam.ZoneScale.Configuration = CogPMAlignZoneConstants.LowHigh;
                        cogMatchParam.ZoneScale.Low = dZoneScaleLow;
                        cogMatchParam.ZoneScale.High = dZoneScaleHigh;
                    }
                    else
                    {
                        cogMatchParam.ZoneScale.Configuration = CogPMAlignZoneConstants.Nominal;
                        cogMatchParam.ZoneScale.Nominal = 0.0;
                    }
                }
                CogPMAlignTool cogPMAlignTool = new CogPMAlignTool();
                {
                    cogPMAlignTool.InputImage = pSourceImage.ToCogImage();
                    cogPMAlignTool.Pattern = cogPatternParam;
                    cogPMAlignTool.RunParams = cogMatchParam;
                    //////  중심점 출력을 위한 Train 영역 확인
                    nTrainedWidth = cogPatternParam.TrainImage.Width;
                    nTrainedHeight = cogPatternParam.TrainImage.Height;
                }
                ////  Run
                try
                {
                    cogPMAlignTool.Run();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }

                if (cogPMAlignTool.Results.Count > 0)   // ApproximateNumberToFind = 1이므로 Results[0]만 존재함.
                {
                    pResult = new CognexResult(pSourceImage, cogPMAlignTool.Results[0].GetPose(), cogPMAlignTool.Pattern.TrainRegion, cogPMAlignTool.Results[0].Score);
                }
                return pResult;
            }

            public static CognexResult FindPattern(CogImage8Grey cogSourceImage, ref CogPMAlignPattern cogPatternParam, double dMatchThreshold,
                bool bUseZoneAngle = true, bool bUseZoneScale = true, double dZoneAngleLow = 0.1, double dZoneAngleHigh = 2.0, double dZoneScaleLow = 0.1, double dZoneScaleHigh = 2.0)
            {
                if (cogSourceImage == null || cogPatternParam == null) return null;

                int nTrainedWidth = 0;
                int nTrainedHeight = 0;
                CognexResult pResult = null;
                ////  PMAlign Parameter 설정
                CogPMAlignRunParams cogMatchParam = new CogPMAlignRunParams();
                {
                    cogMatchParam.ApproximateNumberToFind = 1;
                    cogMatchParam.AcceptThreshold = dMatchThreshold;
                    //////  Zone Angle 틀어짐 범위 설정
                    if (bUseZoneAngle == true)
                    {
                        cogMatchParam.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;
                        cogMatchParam.ZoneAngle.Low = dZoneAngleLow;
                        cogMatchParam.ZoneAngle.High = dZoneAngleHigh;
                    }
                    else
                    {
                        cogMatchParam.ZoneAngle.Configuration = CogPMAlignZoneConstants.Nominal;
                        cogMatchParam.ZoneAngle.Nominal = 0.0;
                    }
                    //////  Zone Scale 크기변환 범위 설정
                    if (bUseZoneScale == true)
                    {
                        cogMatchParam.ZoneScale.Configuration = CogPMAlignZoneConstants.LowHigh;
                        cogMatchParam.ZoneScale.Low = dZoneScaleLow;
                        cogMatchParam.ZoneScale.High = dZoneScaleHigh;
                    }
                    else
                    {
                        cogMatchParam.ZoneScale.Configuration = CogPMAlignZoneConstants.Nominal;
                        cogMatchParam.ZoneScale.Nominal = 0.0;
                    }
                }
                CogPMAlignTool cogPMAlignTool = new CogPMAlignTool();
                {
                    cogPMAlignTool.InputImage = cogSourceImage;
                    cogPMAlignTool.Pattern = cogPatternParam;
                    cogPMAlignTool.RunParams = cogMatchParam;
                    //////  중심점 출력을 위한 Train 영역 확인
                    nTrainedWidth = cogPatternParam.TrainImage.Width;
                    nTrainedHeight = cogPatternParam.TrainImage.Height;
                }
                ////  Run
                try
                {
                    cogPMAlignTool.Run();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }

                if (cogPMAlignTool.Results.Count > 0)   // ApproximateNumberToFind = 1이므로 Results[0]만 존재함.
                {
                    pResult = new CognexResult(new CognexImage(cogSourceImage), cogPMAlignTool.Results[0].GetPose(), cogPMAlignTool.Pattern.TrainRegion, cogPMAlignTool.Results[0].Score);
                }
                return pResult;
            }
        }

        public static class Editor
        {
            public static CognexImage EraseRect(CognexImage pSourceImage, IYoonRect2D<double> pRect)
            {
                return EraseRect(pSourceImage, pRect.CenterPos.X, pRect.CenterPos.Y, pRect.Width, pRect.Height);
            }

            public static CognexImage EraseRect(CognexImage pSourceImage, double dCenterX, double dCenterY, double dRectWidth, double dRectHeight)
            {
                if (pSourceImage == null) return null;

                ////  새로운 BItmap 위에 Rect 영역만큼 검은색(0)으로 색칠
                CognexImage pFilterImage = pSourceImage.Clone() as CognexImage;
                pFilterImage.FillCanvas(Color.White);
                pFilterImage.FillRect((int)dCenterX, (int)dCenterY, (int)dRectWidth, (int)dRectHeight, Color.Black, 1.0);
                return TwoImageProcess.OverlapMin(pSourceImage, pFilterImage);
            }

            public static CognexImage EraseWithoutRect(CognexImage pSourceImage, IYoonRect2D<double> pRect)
            {
                return EraseWithoutRect(pSourceImage, pRect.CenterPos.X, pRect.CenterPos.Y, pRect.Width, pRect.Height);
            }

            public static CognexImage EraseWithoutRect(CognexImage pSourceImage, double dCenterX, double dCenterY, double dRectWidth, double dRectHeight)
            {
                if (pSourceImage == null) return null;

                ////  새로운 BItmap 위에 Rect 영역만큼 흰색(255)으로 색칠
                CognexImage pFilterImage = pSourceImage.Clone() as CognexImage;
                pFilterImage.FillCanvas(Color.White);
                pFilterImage.FillRect((int)dCenterX, (int)dCenterY, (int)dRectWidth, (int)dRectHeight, Color.White, 1.0);
                return TwoImageProcess.OverlapMin(pSourceImage, pFilterImage);
            }

            public static CognexImage EraseWithoutPatternMatchRegion(CognexImage pSourceImage, CognexResult pResult)
            {
                if (pSourceImage == null || pResult.ToolType != eYoonCognexType.PMAlign) return null;
                double dCenterX = pResult.GetPatternMatchArea().CenterPos.X;
                double dCenterY = pResult.GetPatternMatchArea().CenterPos.Y;
                double dWidth = pResult.GetPatternMatchArea().Width;
                double dHeight = pResult.GetPatternMatchArea().Height;
                return EraseWithoutRect(pSourceImage, dCenterX, dCenterY, dWidth, dHeight);
            }
        }

        public static class Transform
        {
            public static CognexImage CropImage(CognexImage pSourceImage, double dCenterX, double dCenterY, double dCropWidth, double dCropHeight)
            {
                return new CognexImage(CropImage(pSourceImage.ToCogImage(), dCenterX, dCenterY, dCropWidth, dCropHeight));
            }

            public static ICogImage CropImage(ICogImage pSourceImage, double dCenterX, double dCenterY, double dCropWidth, double dCropHeight)
            {
                ////  CogAffineTransform 설정
                CogAffineTransform cogAffine = new CogAffineTransform();
                {
                    cogAffine.ScalingX = 1.0;
                    cogAffine.ScalingY = 1.0;
                    cogAffine.SamplingMode = CogAffineTransformSamplingModeConstants.BilinearInterpolation;
                }
                ////  전송받은 X, Y, Width, Height로 Crop 영역 설정
                CogRectangleAffine cogRectRegion = new CogRectangleAffine();
                {
                    cogRectRegion.CenterX = dCenterX;
                    cogRectRegion.CenterY = dCenterY;
                    cogRectRegion.SideXLength = dCropWidth;
                    cogRectRegion.SideYLength = dCropHeight;
                    cogRectRegion.Rotation = 0.0;
                    cogRectRegion.Skew = 0.0;
                }
                CogAffineTransformTool cogAffineTool = new CogAffineTransformTool();
                {
                    cogAffineTool.InputImage = pSourceImage;
                    cogAffineTool.Region = cogRectRegion;
                    cogAffineTool.RunParams = cogAffine;
                }
                ////  Run
                try
                {
                    cogAffineTool.Run();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (cogAffineTool.RunStatus.Result == CogToolResultConstants.Accept) return cogAffineTool.OutputImage;
                else return null;
            }

            public static CognexImage CropImage(CognexImage pSourceImage, IYoonRect pRect)
            {
                return new CognexImage(CropImage(pSourceImage.ToCogImage(), pRect.ToCogRectAffine()));
            }

            public static ICogImage CropImage(ICogImage pSourceImage, ICogRegion pRegion)
            {
                if (pSourceImage == null) return null;
                switch (pRegion)
                {
                    case CogRectangle cogRect:
                        return CropImage(pSourceImage, cogRect);
                    case CogRectangleAffine cogRectAffine:
                        return CropImage(pSourceImage, cogRectAffine);
                    default:
                        break;
                }
                return null;
            }

            public static ICogImage CropImage(ICogImage pSourceImage, CogRectangle pRect)
            {
                ////  CogAffineTransform 설정
                CogAffineTransform cogAffine = new CogAffineTransform();
                {
                    cogAffine.ScalingX = 1.0;
                    cogAffine.ScalingY = 1.0;
                    cogAffine.SamplingMode = CogAffineTransformSamplingModeConstants.BilinearInterpolation;
                }
                ////  전송받은 X, Y, Width, Height로 Crop 영역 설정
                CogRectangleAffine cogRectRegion = new CogRectangleAffine();
                {
                    cogRectRegion.CenterX = pRect.CenterX;
                    cogRectRegion.CenterY = pRect.CenterY;
                    cogRectRegion.SideXLength = pRect.Width;
                    cogRectRegion.SideYLength = pRect.Height;
                    cogRectRegion.Rotation = 0.0;
                    cogRectRegion.Skew = 0.0;
                }
                CogAffineTransformTool cogAffineTool = new CogAffineTransformTool();
                {
                    cogAffineTool.InputImage = pSourceImage;
                    cogAffineTool.Region = cogRectRegion;
                    cogAffineTool.RunParams = cogAffine;
                }
                ////  Run
                try
                {
                    cogAffineTool.Run();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (cogAffineTool.RunStatus.Result == CogToolResultConstants.Accept) return cogAffineTool.OutputImage;
                else return null;
            }

            public static ICogImage CropImage(ICogImage pSourceImage, CogRectangleAffine pRectAffine)
            {
                ////  CogAffineTransform 설정
                CogAffineTransform cogAffine = new CogAffineTransform();
                {
                    cogAffine.ScalingX = 1.0;
                    cogAffine.ScalingY = 1.0;
                    cogAffine.SamplingMode = CogAffineTransformSamplingModeConstants.BilinearInterpolation;
                }
                CogAffineTransformTool cogAffineTool = new CogAffineTransformTool();
                {
                    cogAffineTool.InputImage = pSourceImage;
                    cogAffineTool.Region = pRectAffine;
                    cogAffineTool.RunParams = cogAffine;

                }
                ////  Run
                try
                {
                    cogAffineTool.Run();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (cogAffineTool.RunStatus.Result == CogToolResultConstants.Accept) return cogAffineTool.OutputImage;
                else return null;
            }

            public static CognexImage ResizeImage(CognexImage pSourceImage, double dSourceWidth, double dSourceHeight)
            {
                return new CognexImage(ResizeImage(pSourceImage.ToCogImage(), dSourceWidth, dSourceHeight));
            }

            public static ICogImage ResizeImage(ICogImage pSourceImage, double dSourceWidth, double dSourceHeight)
            {
                ////  CogAffineTransform 설정
                CogAffineTransform cogAffine = new CogAffineTransform();
                {
                    cogAffine.ScalingX = dSourceWidth / pSourceImage.Width;
                    cogAffine.ScalingY = dSourceHeight / pSourceImage.Height;
                    cogAffine.SamplingMode = CogAffineTransformSamplingModeConstants.BilinearInterpolation;
                }
                CogAffineTransformTool cogAffineTool = new CogAffineTransformTool();
                {
                    //////  Object의 너비.높이를 Source와 동일하게 맞춤.
                    cogAffineTool.InputImage = pSourceImage;
                    cogAffineTool.Region = null;     // 전체 영역
                    cogAffineTool.RunParams = cogAffine;
                }
                ////  Run
                try
                {
                    cogAffineTool.Run();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (cogAffineTool.RunStatus.Result == CogToolResultConstants.Accept) return cogAffineTool.OutputImage;
                else return null;
            }

            public static CognexImage ResizeImage(CognexImage pSourceImage, IYoonRect pRect)
            {
                return new CognexImage(ResizeImage(pSourceImage.ToCogImage(), pRect.ToCogRect()));
            }

            public static ICogImage ResizeImage(ICogImage pSourceImage, CogRectangle pRect)
            {
                ////  CogAffineTransform 설정
                CogAffineTransform cogAffine = new CogAffineTransform();
                {
                    cogAffine.ScalingX = pRect.Width / pSourceImage.Width;
                    cogAffine.ScalingY = pRect.Height / pSourceImage.Height;
                    cogAffine.SamplingMode = CogAffineTransformSamplingModeConstants.BilinearInterpolation;
                }
                CogAffineTransformTool cogAffineTool = new CogAffineTransformTool();
                {
                    //////  Object의 너비.높이를 Source와 동일하게 맞춤.
                    cogAffineTool.InputImage = pSourceImage;
                    cogAffineTool.Region = null;     // 전체 영역
                    cogAffineTool.RunParams = cogAffine;
                }
                ////  Run
                try
                {
                    cogAffineTool.Run();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (cogAffineTool.RunStatus.Result == CogToolResultConstants.Accept) return cogAffineTool.OutputImage;
                else return null;
            }
        }

        public static class TwoImageProcess
        {
            public static CognexImage Subtract(CognexImage pSourceImage, CognexImage pObjectImage)
            {
                return new CognexImage(Subtract(pSourceImage.ToCogImage(), pObjectImage.ToCogImage()));
            }

            public static ICogImage Subtract(ICogImage cogSourceImage, ICogImage cogObjectImage)
            {
                ////  Param 설정
                CogIPTwoImageSubtract cogIPSubstractParam = new CogIPTwoImageSubtract();
                {
                    cogIPSubstractParam.OverflowMode = CogIPTwoImageSubtractOverflowModeConstants.Bounded;
                }
                CogIPTwoImageSubtractTool cogIPSubstractTool = new CogIPTwoImageSubtractTool();
                {
                    cogIPSubstractTool.InputImageA = cogSourceImage;
                    cogIPSubstractTool.InputImageB = cogObjectImage;
                    cogIPSubstractTool.RegionA = null;  // 전체 영역
                    cogIPSubstractTool.RegionB = null;
                    cogIPSubstractTool.RunParams = cogIPSubstractParam;
                }
                ////  Run
                try
                {
                    cogIPSubstractTool.Run();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (cogIPSubstractTool.RunStatus.Result == CogToolResultConstants.Accept) return cogIPSubstractTool.OutputImage;
                else return null;
            }

            public static CognexImage Add(CognexImage pSourceImage, CognexImage pObjectImage)
            {
                return new CognexImage(Add(pSourceImage.ToCogImage(), pObjectImage.ToCogImage()));
            }

            public static ICogImage Add(ICogImage cogSourceImage, ICogImage cogObjectImage)
            {
                ////  Param 설정
                CogIPTwoImageAdd cogIPAddParam = new CogIPTwoImageAdd();
                {
                    cogIPAddParam.OverflowMode = CogIPTwoImageAddOverflowModeConstants.Bounded;
                }
                CogIPTwoImageAddTool cogIPAddTool = new CogIPTwoImageAddTool();
                {
                    cogIPAddTool.InputImageA = cogSourceImage;
                    cogIPAddTool.InputImageB = cogObjectImage;
                    cogIPAddTool.RegionA = null;    // 전체 영역
                    cogIPAddTool.RegionB = null;
                    cogIPAddTool.RunParams = cogIPAddParam;
                }
                ////  Run
                try
                {
                    cogIPAddTool.Run();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (cogIPAddTool.RunStatus.Result == CogToolResultConstants.Accept) return cogIPAddTool.OutputImage;
                else return null;

            }

            public static CognexImage OverlapMax(CognexImage pSourceImage, CognexImage pObjectImage)
            {
                return new CognexImage(OverlapMax(pSourceImage.ToCogImage(), pObjectImage.ToCogImage()));
            }

            public static ICogImage OverlapMax(ICogImage cogSourceImage, ICogImage cogObjectImage)
            {
                ////  Param 설정
                CogIPTwoImageMinMax cogIPMinMaxParam = new CogIPTwoImageMinMax();
                {
                    cogIPMinMaxParam.Operation = CogIPTwoImageMinMaxOperationConstants.Max;
                }
                CogIPTwoImageMinMaxTool cogIPMinMaxTool = new CogIPTwoImageMinMaxTool();
                {
                    cogIPMinMaxTool.InputImageA = cogSourceImage;
                    cogIPMinMaxTool.InputImageB = cogObjectImage;
                    cogIPMinMaxTool.RegionA = null; // 전체 영역
                    cogIPMinMaxTool.RegionB = null;
                    cogIPMinMaxTool.RunParams = cogIPMinMaxParam;
                }
                ////  Run
                try
                {
                    cogIPMinMaxTool.Run();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (cogIPMinMaxTool.RunStatus.Result == CogToolResultConstants.Accept) return cogIPMinMaxTool.OutputImage;
                else return null;
            }

            public static CognexImage OverlapMin(CognexImage pSourceImage, CognexImage pObjectImage)
            {
                return new CognexImage(OverlapMin(pSourceImage.ToCogImage(), pObjectImage.ToCogImage()));
            }

            public static ICogImage OverlapMin(ICogImage cogSourceImage, ICogImage cogObjectImage)
            {
                ////  Param 설정
                CogIPTwoImageMinMax cogIPMinMaxParam = new CogIPTwoImageMinMax();
                {
                    cogIPMinMaxParam.Operation = CogIPTwoImageMinMaxOperationConstants.Min;
                }
                CogIPTwoImageMinMaxTool cogIPMinMaxTool = new CogIPTwoImageMinMaxTool();
                {
                    cogIPMinMaxTool.InputImageA = cogSourceImage;
                    cogIPMinMaxTool.InputImageB = cogObjectImage;
                    cogIPMinMaxTool.RegionA = null; // 전체 영역
                    cogIPMinMaxTool.RegionB = null;
                    cogIPMinMaxTool.RunParams = cogIPMinMaxParam;
                }
                ////  Run
                try
                {
                    cogIPMinMaxTool.Run();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (cogIPMinMaxTool.RunStatus.Result == CogToolResultConstants.Accept) return cogIPMinMaxTool.OutputImage;
                else return null;
            }
        }
    }
}