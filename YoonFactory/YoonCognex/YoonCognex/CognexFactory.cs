using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.PMAlign;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using YoonFactory.Image;

namespace YoonFactory.Cognex
{
    public static class CognexFactory
    {
        #region Read CogImage from File
        public static CogImage8Grey LoadCogImage8GreyFromBitmap(string strPath)
        {
            CogImage8Grey cogImage = null;
            int nLength = strPath.LastIndexOf(".");
            if (nLength < 0) nLength = strPath.Length;
            strPath = strPath.Substring(0, nLength) + ".bmp";
            bool isSuccess = false;
            try
            {
                ////  Cognex Image Filer 활용한 Bitmap Loading
                CogImageFileBMP cogBitmap = new CogImageFileBMP();
                cogBitmap.Open(strPath, CogImageFileModeConstants.Read);
                if (cogBitmap[0] != null)
                {
                    cogImage = CogImageConvert.GetIntensityImage(cogBitmap[0], 0, 0, cogBitmap[0].Width, cogBitmap[0].Height);
                    isSuccess = true;
                }
                cogBitmap.Close();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            if (!isSuccess)
            {
                ////  Microsoft 기본함수 활용한 Bitmap Loading
                Bitmap bmpImage = null;
                try
                {
                    bmpImage = new Bitmap(strPath);
                }
                catch { }

                if (bmpImage == null)
                {
                    MessageBox.Show(string.Format("Cannot open file : {0}", strPath));
                    return null;
                }
                cogImage = new CogImage8Grey();
                cogImage.Allocate(bmpImage.Width, bmpImage.Height);
                for (int iY = 0; iY < bmpImage.Height; iY++)
                {
                    for (int iX = 0; iX < bmpImage.Width; iX++)
                    {
                        Color cPixel = bmpImage.GetPixel(iX, iY);
                        byte nLevel = (byte)(cPixel.R * 0.299f + cPixel.G * 0.587f + cPixel.B * 0.114f);
                        cogImage.SetPixel(iX, iY, nLevel);
                    }
                }
            }
            return cogImage;
        }

        public static CogImage24PlanarColor LoadCogImage24PlanarColorFromBitmap(string strPath)
        {
            CogImage24PlanarColor cogImage = null;
            int nLength = strPath.LastIndexOf(".");
            if (nLength < 0) nLength = strPath.Length;
            strPath = strPath.Substring(0, nLength) + ".bmp";
            bool isSuccess = false;
            try
            {
                ////  Cognex Image Filer 활용한 Bitmap Loading
                CogImageFileBMP cogBitmap = new CogImageFileBMP();
                cogBitmap.Open(strPath, CogImageFileModeConstants.Read);
                if (cogBitmap[0] != null)
                {
                    cogImage = CogImageConvert.GetRGBImage(cogBitmap[0], 0, 0, cogBitmap[0].Width, cogBitmap[0].Height) as CogImage24PlanarColor;
                    isSuccess = true;
                }
                cogBitmap.Close();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            if (!isSuccess)
            {
                ////  Microsoft 기본함수 활용한 Bitmap Loading
                Bitmap bmpImage = null;
                try
                {
                    bmpImage = new Bitmap(strPath);
                }
                catch { }

                if (bmpImage == null)
                {
                    MessageBox.Show(string.Format("Cannot open file : {0}", strPath));
                    return null;
                }
                cogImage = new CogImage24PlanarColor();
                cogImage.Allocate(bmpImage.Width, bmpImage.Height);
                for (int iY = 0; iY < bmpImage.Height; iY++)
                {
                    for (int iX = 0; iX < bmpImage.Width; iX++)
                    {
                        Color cPixel = bmpImage.GetPixel(iX, iY);
                        cogImage.SetPixel(iX, iY, cPixel.R, cPixel.G, cPixel.B);
                    }
                }
            }
            return cogImage;
        }

        public static CogImage8Grey LoadCogImage8GreyFromJPEG(string strPath)
        {
            CogImage8Grey cogImage = null;
            int nLength = strPath.LastIndexOf(".");
            if (nLength < 0) nLength = strPath.Length;
            strPath = strPath.Substring(0, nLength) + ".jpg";
            bool isSuccess = false;
            try
            {
                CogImageFileJPEG cogJpeg = new CogImageFileJPEG();
                cogJpeg.Open(strPath, CogImageFileModeConstants.Read);
                if (cogJpeg[0] != null)
                {
                    cogImage = CogImageConvert.GetIntensityImage(cogJpeg[0], 0, 0, cogJpeg[0].Width, cogJpeg[0].Height);
                    isSuccess = true;
                }
                cogJpeg.Close();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            if (!isSuccess)
            {
                MessageBox.Show(string.Format("Cannot open file : {0}", strPath));
            }
            return cogImage;
        }

        public static CogImage24PlanarColor LoadCogImage24PlanarFromJPEG(string strPath)
        {
            CogImage24PlanarColor cogImage = null;
            int nLength = strPath.LastIndexOf(".");
            if (nLength < 0) nLength = strPath.Length;
            strPath = strPath.Substring(0, nLength) + ".jpg";
            bool isSuccess = false;
            try
            {
                CogImageFileJPEG cogJpeg = new CogImageFileJPEG();
                cogJpeg.Open(strPath, CogImageFileModeConstants.Read);
                if (cogJpeg[0] != null)
                {
                    cogImage = CogImageConvert.GetRGBImage(cogJpeg[0], 0, 0, cogJpeg[0].Width, cogJpeg[0].Height) as CogImage24PlanarColor;
                    isSuccess = true;
                }
                cogJpeg.Close();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            if (!isSuccess)
            {
                MessageBox.Show(string.Format("Cannot open file : {0}", strPath));
            }
            return cogImage;
        }
        #endregion

        #region Write CogImage from File
        public static void SaveMonoImageToBitmap(CogImage8Grey cogImage, string strPath)
        {
            string strExtension = Path.GetExtension(strPath);
            string strDir = Path.GetDirectoryName(strPath);
            if (strExtension != ".bmp") return;
            ////  Directory가 없는 경우 경로 생성
            if (Directory.Exists(strDir) == false) Directory.CreateDirectory(strDir);
            ////  CogBitmap 생성 및 Image 저장
            try
            {
                CogImageFileBMP cogBitmap = new CogImageFileBMP();
                cogBitmap.Open(strPath, CogImageFileModeConstants.Write);
                cogBitmap.Append(cogImage);
                cogBitmap.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public static void SaveColorImageToBitmap(CogImage24PlanarColor cogImage, string strPath)
        {
            string strExtension = Path.GetExtension(strPath);
            string strDir = Path.GetDirectoryName(strPath);
            if (strExtension != ".bmp") return;
            ////  Directory가 없는 경우 경로 생성
            if (Directory.Exists(strDir) == false) Directory.CreateDirectory(strDir);
            ////  CogBitmap 생성 및 Image 저장
            try
            {
                CogImageFileBMP cogBitmap = new CogImageFileBMP();
                cogBitmap.Open(strPath, CogImageFileModeConstants.Write);
                cogBitmap.Append(cogImage);
                cogBitmap.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public static void SaveMonoImageToJPEG(CogImage8Grey cogImage, string strPath)
        {
            string strExtension = Path.GetExtension(strPath);
            string strDir = Path.GetDirectoryName(strPath);
            if (strExtension != ".jpg") return;
            ////  Directory가 없는 경우 경로 생성
            if (Directory.Exists(strDir) == false) Directory.CreateDirectory(strDir);
            ////  CogJPEG 생성 및 Image 저장
            try
            {
                CogImageFileJPEG cogJpeg = new CogImageFileJPEG();
                cogJpeg.Open(strPath, CogImageFileModeConstants.Write);
                cogJpeg.Append(cogImage);
                cogJpeg.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public static void SaveColorImageToJPEG(CogImage24PlanarColor cogImage, string strPath)
        {
            string strExtension = Path.GetExtension(strPath);
            string strDir = Path.GetDirectoryName(strPath);
            if (strExtension != ".jpg") return;
            ////  Directory가 없는 경우 경로 생성
            if (Directory.Exists(strDir) == false) Directory.CreateDirectory(strDir);
            ////  CogJPEG 생성 및 Image 저장
            try
            {
                CogImageFileJPEG cogJpeg = new CogImageFileJPEG();
                cogJpeg.Open(strPath, CogImageFileModeConstants.Write);
                cogJpeg.Append(cogImage);
                cogJpeg.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region Display Event Handling Function
        public static void GetMousePositionAtCogDisplay(CogDisplay pDisplay, int nX, int nY, out double dX, out double dY)
        {
            CogDisplayPanAnchorConstants fAnchor;
            dX = 0.0;
            dY = 0.0;
            try
            {
                double anchorX, anchorY;

                ////  Mouse Point(nX, nY)에는 이미 anchor가 합해짐.
                pDisplay.GetImagePanAnchor(out anchorX, out anchorY, out fAnchor);

                double clientWidth = pDisplay.ClientRectangle.Width * anchorX;
                double clientHeight = pDisplay.ClientRectangle.Height * anchorY;
                double imageWidth = pDisplay.Image.Width * anchorX;
                double imageHeight = pDisplay.Image.Height * anchorY;
                double displayX = pDisplay.PanX;
                double displayY = pDisplay.PanY;

                dX = ((double)nX - clientWidth) / pDisplay.Zoom - displayX + imageWidth;    //  Client 좌표를 Image 좌표로 전환.
                dY = ((double)nY - clientHeight) / pDisplay.Zoom - displayY + imageHeight;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

        }
        #endregion

        public static class Converter
        {
            /// <summary>
            /// Image Convert 사용해 24bit Image를 8bit Image로 변환하는 Process
            /// </summary>
            /// <param name="cogImage">대상 24bit Image</param>
            /// <returns></returns>
            public static CogImage8Grey ToImage8Grey(ICogImage cogImage)
            {
                ////  CogImageConvertor 설정
                CogImageConvertRunParams cogConvertParam = new CogImageConvertRunParams();
                {
                    cogConvertParam.RunMode = CogImageConvertRunModeConstants.Intensity;
                }
                CogImageConvertTool cogConvertTool = new CogImageConvertTool();
                {
                    cogConvertTool.InputImage = cogImage;
                    cogConvertTool.Region = null;   // 전체 영역.
                    cogConvertTool.RunParams = cogConvertParam;
                }

                //// CogImageConvertor 실행
                try
                {
                    cogConvertTool.Run();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (cogConvertTool.RunStatus.Result == CogToolResultConstants.Accept) return cogConvertTool.OutputImage as CogImage8Grey;
                else return null;
            }

            /// <summary>
            /// CogIPOneImage를 사용한 이진화 Process
            /// </summary>
            /// <param name="cogImage">이진화 대상 8bit Image</param>
            /// <returns></returns>
            public static CogImage8Grey ToImage8GreyBinary(CogImage8Grey cogImage)
            {
                ////  CogOneImageProcess 설정
                CogIPOneImageQuantize cogIPQuantize = new CogIPOneImageQuantize();
                {
                    cogIPQuantize.Levels = CogIPOneImageQuantizeLevelConstants.s2;
                }
                CogIPOneImageMultiplyConstant cogIPMultiplyConstant = new CogIPOneImageMultiplyConstant();
                {
                    cogIPMultiplyConstant.ConstantValue = 2.0;
                    cogIPMultiplyConstant.ConstantPlane0Value = 1.0;
                    cogIPMultiplyConstant.ConstantPlane1Value = 1.0;
                    cogIPMultiplyConstant.ConstantPlane2Value = 1.0;
                    cogIPMultiplyConstant.OverflowMode = CogIPOneImageMultiplyConstantOverflowModeConstants.Bounded;
                }
                ////  CogOneImageProcessTool 설정
                CogIPOneImageTool cogIPTool = new CogIPOneImageTool();
                {
                    cogIPTool.InputImage = cogImage;
                    cogIPTool.Region = null;    // 전체 영억
                    cogIPTool.RegionMode = CogRegionModeConstants.PixelAlignedBoundingBox;
                    cogIPTool.Operators.Add(cogIPQuantize); //  양자화로 0, 128로 분활
                    cogIPTool.Operators.Add(cogIPMultiplyConstant); // 상수곱으로 0, 255로 확장
                }

                ////  CogOneImageProcess 실행
                try
                {
                    cogIPTool.Run();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (cogIPTool.RunStatus.Result == CogToolResultConstants.Accept) return cogIPTool.OutputImage as CogImage8Grey;
                else return null;
            }

            #region Buffer에서 CogImage 만들기
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
            #endregion
        }

        public static class PatternMatch
        {

            #region Pattern Matching 하기 (PMAlign)
            public static CogPMAlignPattern TrainPattern(CogImage8Grey cogImage, double dOriginX, double dOriginY, bool bAutoLimited, bool bAutoThreshold,
                                                                  double dCoarseLimit = 10.0, double dFineLimit = 3.0, double dThreshold = 30.0)
            {
                ////  Pattern 영역 및 원점 설정
                CogPMAlignPattern cogPattern = new CogPMAlignPattern();
                {
                    //////  Pattern 설정
                    cogPattern.TrainImage = cogImage;
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

            public static CogPMAlignPattern TrainPatternWithTopLeftOrigin(CogImage8Grey cogImage, bool bAutoLimited, bool bAutoThreshold,
                                                                  double dCoarseLimit = 10.0, double dFineLimit = 3.0, double dThreshold = 30.0)
            {
                ////  Pattern 영역 및 원점 설정
                CogPMAlignPattern cogPattern = new CogPMAlignPattern();
                {
                    //////  Pattern 설정
                    cogPattern.TrainImage = cogImage;
                    cogPattern.TrainRegion = null;  // 전체 영역
                    cogPattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;
                    //////  Origin 설정
                    cogPattern.Origin.TranslationX = 0.0;
                    cogPattern.Origin.TranslationY = 0.0;   //  Pattern Train 원점은 무조건 Top-Left로 가져감.
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

            public static CognexResult FindPattern(CogImage8Grey cogImage, ref CogPMAlignPattern cogPattern, double dMatchingThreshold, bool bUseZoneAngle, bool bUseZoneScale,
                                                           double dZoneAngleLow = 0.1, double dZoneAngleHigh = 2.0, double dZoneScaleLow = 0.1, double dZoneScaleHigh = 2.0)
            {
                if (cogPattern == null) return null;    // Train 된 Pattern 없을시 NULL 처리.

                int nTrainedWidth = 0;
                int nTrainedHeight = 0;
                CognexResult pResult = null;
                ////  PMAlign Parameter 설정
                CogPMAlignRunParams cogPMParam = new CogPMAlignRunParams();
                {
                    cogPMParam.ApproximateNumberToFind = 1;
                    cogPMParam.AcceptThreshold = dMatchingThreshold;
                    //////  Zone Angle 틀어짐 범위 설정
                    if (bUseZoneAngle == true)
                    {
                        cogPMParam.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;
                        cogPMParam.ZoneAngle.Low = dZoneAngleLow;
                        cogPMParam.ZoneAngle.High = dZoneAngleHigh;
                    }
                    else
                    {
                        cogPMParam.ZoneAngle.Configuration = CogPMAlignZoneConstants.Nominal;
                        cogPMParam.ZoneAngle.Nominal = 0.0;
                    }
                    //////  Zone Scale 크기변환 범위 설정
                    if (bUseZoneScale == true)
                    {
                        cogPMParam.ZoneScale.Configuration = CogPMAlignZoneConstants.LowHigh;
                        cogPMParam.ZoneScale.Low = dZoneScaleLow;
                        cogPMParam.ZoneScale.High = dZoneScaleHigh;
                    }
                    else
                    {
                        cogPMParam.ZoneScale.Configuration = CogPMAlignZoneConstants.Nominal;
                        cogPMParam.ZoneScale.Nominal = 0.0;
                    }
                }
                CogPMAlignTool cogPMAlignTool = new CogPMAlignTool();
                {
                    cogPMAlignTool.InputImage = cogImage;
                    cogPMAlignTool.Pattern = cogPattern;
                    cogPMAlignTool.RunParams = cogPMParam;
                    //////  중심점 출력을 위한 Train 영역 확인
                    nTrainedWidth = cogPattern.TrainImage.Width;
                    nTrainedHeight = cogPattern.TrainImage.Height;
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
                    pResult = new CognexResult(cogImage, cogPMAlignTool.Results[0].GetPose(), cogPMAlignTool.Pattern.TrainRegion, cogPMAlignTool.Results[0].Score);
                }
                return pResult;
            }

            public static YoonVector2D FindPatternToPoint(CogImage8Grey cogImage, ref CogPMAlignPattern cogPattern, double dMatchingThreshold, bool bUseZoneAngle, bool bUseZoneScale,
                                                                      double dZoneAngleLow = 0.1, double dZoneAngleHigh = 2.0, double dZoneScaleLow = 0.1, double dZoneScaleHigh = 2.0)
            {
                if (cogPattern == null) return new YoonVector2D(-10000.0f, -10000.0f);    // Train 된 Pattern 없을시 NULL 처리.

                int nTrainedWidth = 0;
                int nTrainedHeight = 0;
                YoonVector2D fPointResult = new YoonVector2D();
                ////  PMAlign Parameter 설정
                CogPMAlignRunParams cogPMParam = new CogPMAlignRunParams();
                {
                    cogPMParam.ApproximateNumberToFind = 1;
                    cogPMParam.AcceptThreshold = dMatchingThreshold;
                    //////  Zone Angle 틀어짐 범위 설정
                    if (bUseZoneAngle == true)
                    {
                        cogPMParam.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;
                        cogPMParam.ZoneAngle.Low = dZoneAngleLow;
                        cogPMParam.ZoneAngle.High = dZoneAngleHigh;
                    }
                    else
                    {
                        cogPMParam.ZoneAngle.Configuration = CogPMAlignZoneConstants.Nominal;
                        cogPMParam.ZoneAngle.Nominal = 0.0;
                    }
                    //////  Zone Scale 크기변환 범위 설정
                    if (bUseZoneScale == true)
                    {
                        cogPMParam.ZoneScale.Configuration = CogPMAlignZoneConstants.LowHigh;
                        cogPMParam.ZoneScale.Low = dZoneScaleLow;
                        cogPMParam.ZoneScale.High = dZoneScaleHigh;
                    }
                    else
                    {
                        cogPMParam.ZoneScale.Configuration = CogPMAlignZoneConstants.Nominal;
                        cogPMParam.ZoneScale.Nominal = 0.0;
                    }
                }
                CogPMAlignTool cogPMAlignTool = new CogPMAlignTool();
                {
                    cogPMAlignTool.InputImage = cogImage;
                    cogPMAlignTool.Pattern = cogPattern;
                    cogPMAlignTool.RunParams = cogPMParam;
                    //////  중심점 출력을 위한 Train 영역 확인
                    nTrainedWidth = cogPattern.TrainImage.Width;
                    nTrainedHeight = cogPattern.TrainImage.Height;
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
                    fPointResult.X = (float)cogPMAlignTool.Results[0].GetPose().TranslationX;
                    fPointResult.Y = (float)cogPMAlignTool.Results[0].GetPose().TranslationY;
                }
                return fPointResult;
            }

            public static CogRectangleAffine FindPatternToCogRect(CogImage8Grey cogImage, ref CogPMAlignPattern cogPattern, double dMatchingThreshold, bool bUseZoneAngle, bool bUseZoneScale,
                                                                      double dZoneAngleLow = 0.1, double dZoneAngleHigh = 2.0, double dZoneScaleLow = 0.1, double dZoneScaleHigh = 2.0)
            {
                if (cogPattern == null) return null;    // Train 된 Pattern 없을시 NULL 처리.
                if (cogPattern.Origin.TranslationX != 0.0 || cogPattern.Origin.TranslationY != 0.0) // Train Position이 Top_Left가 아닌 경우 예외처리.
                {
                    MessageBox.Show(string.Format("Cannot use trained position : X={0:E2}, Y={1:E2}", cogPattern.Origin.TranslationX, cogPattern.Origin.TranslationY));
                    return null;
                }

                int nTrainedWidth = 0;
                int nTrainedHeight = 0;
                CogRectangleAffine cogRectResult = new CogRectangleAffine();
                ////  PMAlign Parameter 설정
                CogPMAlignRunParams cogPMParam = new CogPMAlignRunParams();
                {
                    cogPMParam.ApproximateNumberToFind = 1;
                    cogPMParam.AcceptThreshold = dMatchingThreshold;
                    //////  Zone Angle 틀어짐 범위 설정
                    if (bUseZoneAngle == true)
                    {
                        cogPMParam.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;
                        cogPMParam.ZoneAngle.Low = dZoneAngleLow;
                        cogPMParam.ZoneAngle.High = dZoneAngleHigh;
                    }
                    else
                    {
                        cogPMParam.ZoneAngle.Configuration = CogPMAlignZoneConstants.Nominal;
                        cogPMParam.ZoneAngle.Nominal = 0.0;
                    }
                    //////  Zone Scale 크기변환 범위 설정
                    if (bUseZoneScale == true)
                    {
                        cogPMParam.ZoneScale.Configuration = CogPMAlignZoneConstants.LowHigh;
                        cogPMParam.ZoneScale.Low = dZoneScaleLow;
                        cogPMParam.ZoneScale.High = dZoneScaleHigh;
                    }
                    else
                    {
                        cogPMParam.ZoneScale.Configuration = CogPMAlignZoneConstants.Nominal;
                        cogPMParam.ZoneScale.Nominal = 0.0;
                    }
                }
                CogPMAlignTool cogPMAlignTool = new CogPMAlignTool();
                {
                    cogPMAlignTool.InputImage = cogImage;
                    cogPMAlignTool.Pattern = cogPattern;
                    cogPMAlignTool.RunParams = cogPMParam;
                    //////  중심점 출력을 위한 Train 영역 확인
                    nTrainedWidth = cogPattern.TrainImage.Width;
                    nTrainedHeight = cogPattern.TrainImage.Height;
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
                    double dOriginX = cogPMAlignTool.Results[0].GetPose().TranslationX;
                    double dOriginY = cogPMAlignTool.Results[0].GetPose().TranslationY;
                    double dRectWidth = (double)nTrainedWidth * cogPMAlignTool.Results[0].GetPose().ScalingX;    // Rect 너비/높이는 Train 값에 Scaling 값을 곱함.
                    double dRectHeight = (double)nTrainedHeight * cogPMAlignTool.Results[0].GetPose().ScalingY;
                    double dRectRotation = cogPMAlignTool.Results[0].GetPose().Rotation;
                    //////  Rect Area 설정
                    cogRectResult.CenterX = dOriginX + dRectWidth / 2;
                    cogRectResult.CenterY = dOriginY + dRectHeight / 2; // Top-Left를 Center 위치로 변환.
                    cogRectResult.SideXLength = dRectWidth;
                    cogRectResult.SideYLength = dRectHeight;
                    cogRectResult.Rotation = dRectRotation;
                    cogRectResult.Skew = 0.0;
                }
                return cogRectResult;
            }
            #endregion
        }

        public static class Editor
        {
            #region Image 수정하거나 지우기
            public static CogImage8Grey ErasePatternMatchRegion(CogImage8Grey cogImage, CogPMAlignPattern pPattern, double dAlignX, double dAlignY)
            {
                if (cogImage == null) return null;

                ICogRegion pRegion = pPattern.TrainRegion;
                switch (pRegion)
                {
                    case CogRectangle pRegionRect:
                        return EraseRect(cogImage, dAlignX, dAlignY, pRegionRect.Width, pRegionRect.Height);
                    case CogRectangleAffine pRegionRectAffine:
                        return EraseRect(cogImage, dAlignX, dAlignY, pRegionRectAffine.SideXLength, pRegionRectAffine.SideYLength);
                    default:
                        break;
                }
                return null;
            }

            public static CogImage8Grey EraseRect(CogImage8Grey cogImage, double dCenterX, double dCenterY, double dRectWidth, double dRectHeight)
            {
                if (cogImage == null) return null;

                ////  새로운 BItmap 위에 Rect 영역만큼 검은색(0)으로 색칠
                Bitmap pBitmap = new Bitmap(cogImage.Width, cogImage.Height);
                {
                    ImageFactory.Draw.FillCanvas(ref pBitmap, Color.White);
                    ImageFactory.Draw.FillRect(ref pBitmap, (int)dCenterX, (int)dCenterY, (int)dRectWidth, (int)dRectHeight, Color.Black, 1.0);
                }
                ////  새로운 Bitmap으로 Filter 제작 후 Overlap 함
                CogImage8Grey cogFilterImage = new CogImage8Grey(pBitmap);
                return TwoImageProcess.OverlapMin(cogImage, cogFilterImage) as CogImage8Grey;
            }

            public static CogImage8Grey EraseWithoutPatternMatchRegion(CogImage8Grey cogImage, CognexResult pResult)
            {
                if (cogImage == null || pResult.ToolType != eYoonCognexType.PMAlign) return null;
                double dCenterX = pResult.GetPatternMatchArea().CenterPos.X;
                double dCenterY = pResult.GetPatternMatchArea().CenterPos.Y;
                double dWidth = pResult.GetPatternMatchArea().Width;
                double dHeight = pResult.GetPatternMatchArea().Height;
                return EraseWithoutRect(cogImage, dCenterX, dCenterY, dWidth, dHeight);
            }

            public static CogImage8Grey EraseWithoutRect(CogImage8Grey cogImage, double dCenterX, double dCenterY, double dRectWidth, double dRectHeight)
            {
                if (cogImage == null) return null;

                ////  새로운 BItmap 위에 Rect 영역만큼 흰색(0)으로 색칠
                Bitmap pBitmap = new Bitmap(cogImage.Width, cogImage.Height);
                {
                    ImageFactory.Draw.FillCanvas(ref pBitmap, Color.Black);
                    ImageFactory.Draw.FillRect(ref pBitmap, (int)dCenterX, (int)dCenterY, (int)dRectWidth, (int)dRectHeight, Color.White, 1.0);
                }
                ////  새로운 Bitmap으로 Filter 제작 후 Overlap 함
                CogImage8Grey cogFilterImage = new CogImage8Grey(pBitmap);
                return TwoImageProcess.OverlapMin(cogImage, cogFilterImage) as CogImage8Grey;
            }
            #endregion
        }

        public static class Draw
        {
            #region Display에 그리기
            public static bool DrawGrid(CogDisplay fDisplay, int nPartsX, int nPartsY, string strGroup = "")   // nPart가 2이면 Grid 선은 1개, nPart가 3이면 Grid 선은 2개
            {
                if (nPartsX <= 1 || nPartsY <= 1) return false;
                if (fDisplay == null || fDisplay.IsDisposed) return false;
                if (fDisplay.Width == 0 || fDisplay.Height == 0) return false;

                ////  Parts 간 폭 구하기
                double dPartWidth = fDisplay.Width / nPartsX;
                double dPartHeight = fDisplay.Height / nPartsY;

                ////  Grid 그리기
                for (int iPart = 1; iPart < nPartsX; iPart++)
                {
                    CogLineSegment cogLine_Vert = new CogLineSegment();
                    {
                        cogLine_Vert.Color = CogColorConstants.Yellow;
                        cogLine_Vert.SetStartEnd(dPartWidth * iPart, 0, dPartWidth * iPart, fDisplay.Height);
                        cogLine_Vert.LineWidthInScreenPixels = 1;
                        cogLine_Vert.Interactive = false;
                    }
                    fDisplay.InteractiveGraphics.Add(cogLine_Vert, strGroup, false);
                }
                for (int iPart = 1; iPart < nPartsY; iPart++)
                {
                    CogLineSegment cogLine_Horz = new CogLineSegment();
                    {
                        cogLine_Horz.Color = CogColorConstants.Yellow;
                        cogLine_Horz.SetStartEnd(0, dPartHeight * iPart, fDisplay.Width, dPartHeight * iPart);
                        cogLine_Horz.LineWidthInScreenPixels = 1;
                        cogLine_Horz.Interactive = false;
                    }
                    fDisplay.InteractiveGraphics.Add(cogLine_Horz, strGroup, false);
                }
                return true;
            }

            public static CogRectangle DrawROI(CogDisplay fDisplay, double dCenterX, double dCenterY, double dWidth, double dHeight, string strGroup = "")
            {
                if (fDisplay == null || fDisplay.IsDisposed) return null;

                ////  ROI 사각형 그리기
                CogRectangle fCogRectROI = new CogRectangle();
                {
                    fCogRectROI.X = dCenterX;
                    fCogRectROI.Y = dCenterY;
                    fCogRectROI.Width = dWidth;
                    fCogRectROI.Height = dHeight;
                    fCogRectROI.Color = CogColorConstants.Red;
                    fCogRectROI.GraphicDOFEnable = CogRectangleDOFConstants.All;
                    fCogRectROI.TipText = "ROI";
                    fCogRectROI.Interactive = false; // 움직임 없음. 고정.
                }
                fDisplay.InteractiveGraphics.Add(fCogRectROI, strGroup, false);
                return fCogRectROI;
            }

            public static bool DrawPatternTrainROI(CogDisplay fDisplay, double dStartX, double dStartY, double dWidth, double dHeight, string strGroup = "")
            {
                if (fDisplay == null || fDisplay.IsDisposed) return false;

                ////  Mark ROI 사각형 아래에 Text 적기
                CogGraphicLabel cogLabel = new CogGraphicLabel();
                {
                    cogLabel.BackgroundColor = CogColorConstants.None;
                    cogLabel.Color = CogColorConstants.Red;
                    cogLabel.SetXYText(dStartX, dStartY, "Mark ROI");
                    cogLabel.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Regular);
                    cogLabel.Interactive = false;
                    cogLabel.Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                }
                fDisplay.InteractiveGraphics.Add(cogLabel, strGroup, false);

                ////  Mark ROI 안에 십자가 그리기
                double dCenterX = dStartX + dWidth / 2;
                double dCenterY = dStartY + dHeight / 2;
                double dLineLength = 5;
                CogLineSegment cogLine_Horz = new CogLineSegment();
                {
                    cogLine_Horz.Color = CogColorConstants.Yellow;
                    cogLine_Horz.SetStartEnd(dCenterX - dLineLength, dCenterY, dCenterX + dLineLength, dCenterY);
                    cogLine_Horz.LineWidthInScreenPixels = 1;
                    cogLine_Horz.Interactive = false;
                }
                CogLineSegment cogLine_Vert = new CogLineSegment();
                {
                    cogLine_Vert.Color = CogColorConstants.Yellow;
                    cogLine_Vert.SetStartEnd(dCenterX, dCenterY - dLineLength, dCenterX, dCenterY + dLineLength);
                    cogLine_Vert.LineWidthInScreenPixels = 1;
                    cogLine_Vert.Interactive = false;
                }
                fDisplay.InteractiveGraphics.Add(cogLine_Horz, strGroup, false);
                fDisplay.InteractiveGraphics.Add(cogLine_Vert, strGroup, false);
                return true;
            }

            public static bool DrawBlobRect(CogDisplay fDisplay, CognexResult pResult, int nMinimumPixelCount, string strGroup = "", bool bInteractive = false, bool bDisplayText = false)
            {
                if (fDisplay == null || fDisplay.IsDisposed) return false;
                if (pResult == null || pResult.ToolType != eYoonCognexType.Blob) return false;
                if (pResult.CogShapeDictionary == null || pResult.CogShapeDictionary.Count == 0) return false;
                if (pResult.ObjectDictionary == null || pResult.ObjectDictionary.Count == 0) return false;

                foreach (int nID in pResult.CogShapeDictionary.Keys)
                {
                    if (!pResult.ObjectDictionary.ContainsKey(nID) || (pResult.ObjectDictionary[nID] as YoonRectObject).PixelCount < nMinimumPixelCount) continue;

                    CogRectangleAffine cogRectAffine = pResult.CogShapeDictionary[nID] as CogRectangleAffine;
                    {
                        cogRectAffine.Color = CogColorConstants.DarkGreen;
                        cogRectAffine.LineWidthInScreenPixels = 1;
                        cogRectAffine.Interactive = false;
                    }
                    fDisplay.InteractiveGraphics.Add(cogRectAffine, strGroup, false);

                    if (bDisplayText == true)
                    {
                        string strTextID = string.Format("ID={0}", nID);
                        CogGraphicLabel cogLabel = new CogGraphicLabel();
                        {
                            cogLabel.BackgroundColor = CogColorConstants.None;
                            cogLabel.Color = CogColorConstants.Red;
                            cogLabel.SetXYText(cogRectAffine.CornerYX, cogRectAffine.CornerYY + 10, strTextID);
                            cogLabel.Font = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
                            cogLabel.Interactive = false;
                            cogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                        }
                        fDisplay.InteractiveGraphics.Add(cogLabel, strGroup, false);
                    }
                }
                return true;
            }

            public static bool DrawPatternMatchCross(CogDisplay fDisplay, CognexResult pResult, string strGroup = "", bool bInteractive = false, bool bDisplayText = false)
            {
                if (pResult == null || pResult.ToolType != eYoonCognexType.PMAlign)
                    return false;
                if (pResult.ObjectDictionary[0] is YoonRectObject pObjectPattern)
                {
                    YoonVector2D pMatchPos = pObjectPattern.FeaturePos as YoonVector2D;
                    YoonRectAffine2D pPickArea = pObjectPattern.PickArea as YoonRectAffine2D;
                    if (pMatchPos.X == 0.0 && pMatchPos.Y == 0.0)
                        return false;
                    if (pPickArea.Width == 0.0 || pPickArea.Height == 0.0 || pObjectPattern.Score == 0)
                        return false;

                    double dLineLength = 2.0;
                    CogLineSegment cogLine_Horz = new CogLineSegment();
                    {
                        cogLine_Horz.Color = CogColorConstants.Green;
                        cogLine_Horz.SetStartEnd(pMatchPos.X - dLineLength, pMatchPos.Y, pMatchPos.X + dLineLength, pMatchPos.Y);
                        cogLine_Horz.LineWidthInScreenPixels = 2;
                        cogLine_Horz.Interactive = bInteractive;
                    }
                    CogLineSegment cogLine_Vert = new CogLineSegment();
                    {
                        cogLine_Vert.Color = CogColorConstants.Green;
                        cogLine_Vert.SetStartEnd(pMatchPos.X, pMatchPos.Y - dLineLength, pMatchPos.X, pMatchPos.Y + dLineLength);
                        cogLine_Vert.LineWidthInScreenPixels = 1;
                        cogLine_Vert.Interactive = bInteractive;
                    }
                    fDisplay.InteractiveGraphics.Add(cogLine_Horz, strGroup, false);
                    fDisplay.InteractiveGraphics.Add(cogLine_Vert, strGroup, false);

                    if (bDisplayText == true && pObjectPattern.Score > 0)
                    {
                        string strTextScore = string.Format("Mark={0:0.0}", pObjectPattern.Score);
                        CogGraphicLabel cogLabel = new CogGraphicLabel();
                        {
                            cogLabel.BackgroundColor = CogColorConstants.None;
                            cogLabel.Color = CogColorConstants.Red;
                            cogLabel.SetXYText(pMatchPos.X + dLineLength, pMatchPos.Y + dLineLength, strTextScore);
                            cogLabel.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
                            cogLabel.Interactive = false;
                            cogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                        }
                        fDisplay.InteractiveGraphics.Add(cogLabel, strGroup, false);
                    }
                    return true;
                }

                return false;
            }

            public static bool DrawPatternMatchRect(CogDisplay fDisplay, CognexResult pResult, string strGroup = "", bool bInteractive = false, bool bDisplayText = false)
            {
                if (pResult == null || pResult.ToolType != eYoonCognexType.PMAlign)
                    return false;

                if (pResult.ObjectDictionary[0] is YoonRectObject pObjectPattern)
                {
                    YoonVector2D pMatchPos = pObjectPattern.FeaturePos as YoonVector2D;
                    YoonRectAffine2D pPickArea = pObjectPattern.PickArea as YoonRectAffine2D;
                    if (pMatchPos.X == 0.0 && pMatchPos.Y == 0.0)
                        return false;
                    if (pPickArea.Width == 0.0 || pPickArea.Height == 0.0 || pObjectPattern.Score == 0)
                        return false;
                    CogRectangle fCogRectMatchResult = new CogRectangle();
                    {
                        fCogRectMatchResult.SetCenterWidthHeight(pMatchPos.X, pMatchPos.Y, pPickArea.Width, pPickArea.Height);
                        fCogRectMatchResult.Color = CogColorConstants.Green;
                        fCogRectMatchResult.GraphicDOFEnable = CogRectangleDOFConstants.All;
                        fCogRectMatchResult.LineWidthInScreenPixels = 3;
                        fCogRectMatchResult.TipText = "Mark ROI";
                        fCogRectMatchResult.Interactive = bInteractive;
                    }
                    fDisplay.InteractiveGraphics.Add(fCogRectMatchResult, strGroup, false);

                    if (bDisplayText == true && pObjectPattern.Score > 0)
                    {
                        string strTextScore = string.Format("Mark={0:0.0}", pObjectPattern.Score);
                        CogGraphicLabel cogLabel = new CogGraphicLabel();
                        {
                            cogLabel.BackgroundColor = CogColorConstants.None;
                            cogLabel.Color = CogColorConstants.Red;
                            cogLabel.SetXYText(pMatchPos.X - pPickArea.Width / 2, pMatchPos.Y + pPickArea.Height / 2 + 10, strTextScore);
                            cogLabel.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
                            cogLabel.Interactive = false;
                            cogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                        }
                        fDisplay.InteractiveGraphics.Add(cogLabel, strGroup, false);
                    }
                    return true;
                }

                return false;
            }

            public static bool DrawCalibrationMapping(CogDisplay fDisplay, CognexMapping pMapping, string strGroup = "", bool bInteractive = false, bool bDisplayText = false)
            {
                if (pMapping == null) return false;
                if (pMapping.PixelPoints.Count == 0 || pMapping.RealPoints.Count == 0 || pMapping.PixelPoints.Count != pMapping.RealPoints.Count) return false;
                if (pMapping.Width == 0 || pMapping.Height == 0) return false;

                double dLineLength = 2.0;
                for (int iPos = 0; iPos < pMapping.PixelPoints.Count; iPos++)
                {
                    YoonVector2D pVecPixel = pMapping.PixelPoints[iPos] as YoonVector2D;
                    CogLineSegment cogLine_Horz = new CogLineSegment();
                    {
                        cogLine_Horz.Color = CogColorConstants.Cyan;
                        cogLine_Horz.SetStartEnd(pVecPixel.X - dLineLength, pVecPixel.Y, pVecPixel.X + dLineLength, pVecPixel.Y);
                        cogLine_Horz.LineWidthInScreenPixels = 2;
                        cogLine_Horz.Interactive = bInteractive;
                    }
                    CogLineSegment cogLine_Vert = new CogLineSegment();
                    {
                        cogLine_Vert.Color = CogColorConstants.Cyan;
                        cogLine_Vert.SetStartEnd(pVecPixel.X, pVecPixel.Y - dLineLength, pVecPixel.X, pVecPixel.Y + dLineLength);
                        cogLine_Vert.LineWidthInScreenPixels = 2;
                        cogLine_Vert.Interactive = bInteractive;
                    }
                    fDisplay.InteractiveGraphics.Add(cogLine_Horz, strGroup, false);
                    fDisplay.InteractiveGraphics.Add(cogLine_Vert, strGroup, false);

                    if (bDisplayText == true && pMapping.RealPoints[iPos] is YoonVector2D pVecReal)
                    {
                        string strTextPos = string.Format("X,Y=({0:0.0},{1:0.0})", pVecReal.X, pVecReal.Y);
                        CogGraphicLabel cogLabel = new CogGraphicLabel();
                        {
                            cogLabel.BackgroundColor = CogColorConstants.None;
                            cogLabel.Color = CogColorConstants.Cyan;
                            cogLabel.SetXYText(pVecPixel.X + dLineLength, pVecPixel.Y + dLineLength, strTextPos);
                            cogLabel.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 9.0f, FontStyle.Bold);
                            cogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                        }
                        fDisplay.InteractiveGraphics.Add(cogLabel, strGroup, false);
                    }
                }
                return true;
            }

            public static bool DrawRuler(CogDisplay fDisplay, double dStartX, double dStartY, double dEndX, double dEndY, double dResolution, string strGroup = "", bool bDisplayText = false)
            {
                string strTextLength = "";

                ////  상부 측정 Point의 표시
                CogLineSegment cogLineStartCross_Horz = new CogLineSegment();
                {
                    cogLineStartCross_Horz.Color = CogColorConstants.Yellow;
                    cogLineStartCross_Horz.SetStartEnd(dStartX - 5.0, dStartY, dStartX + 5.0, dStartY);
                    cogLineStartCross_Horz.LineWidthInScreenPixels = 3;
                    cogLineStartCross_Horz.Interactive = false;
                }
                CogLineSegment cogLineStartCross_Vert = new CogLineSegment();
                {
                    cogLineStartCross_Vert.Color = CogColorConstants.Yellow;
                    cogLineStartCross_Vert.SetStartEnd(dStartX, dStartY - 5.0, dStartX, dStartY + 5.0);
                    cogLineStartCross_Vert.LineWidthInScreenPixels = 3;
                    cogLineStartCross_Vert.Interactive = false;
                }
                fDisplay.InteractiveGraphics.Add(cogLineStartCross_Horz, strGroup, false);
                fDisplay.InteractiveGraphics.Add(cogLineStartCross_Vert, strGroup, false);

                ////  실제 측정 Draw
                CogLineSegment fCogLine = new CogLineSegment();
                {
                    fCogLine.Color = CogColorConstants.Yellow;
                    fCogLine.SetStartEnd(dStartX, dStartY, dEndX, dEndY);
                    fCogLine.LineWidthInScreenPixels = 3;
                    fCogLine.Interactive = false;
                }
                fDisplay.InteractiveGraphics.Add(fCogLine, strGroup, false);

                ////  하부 측정 Point의 표시
                CogLineSegment cogLineEndCross_Horz = new CogLineSegment();
                {
                    cogLineEndCross_Horz.Color = CogColorConstants.Yellow;
                    cogLineEndCross_Horz.SetStartEnd(dEndX - 5.0, dEndY, dEndX + 5.0, dEndY);
                    cogLineEndCross_Horz.LineWidthInScreenPixels = 3;
                    cogLineEndCross_Horz.Interactive = false;
                }
                CogLineSegment cogLineEndCross_Vert = new CogLineSegment();
                {
                    cogLineEndCross_Vert.Color = CogColorConstants.Yellow;
                    cogLineEndCross_Vert.SetStartEnd(dEndX, dEndY - 5.0, dEndX, dEndY + 5.0);
                    cogLineEndCross_Vert.LineWidthInScreenPixels = 3;
                    cogLineEndCross_Vert.Interactive = false;
                }
                fDisplay.InteractiveGraphics.Add(cogLineEndCross_Horz, strGroup, false);
                fDisplay.InteractiveGraphics.Add(cogLineEndCross_Vert, strGroup, false);

                ////  실측정(mm) 정보 표기
                if (bDisplayText == true)
                {
                    double dPosLabelX = dEndX + 10.0;
                    double dPosLabelY = dEndY + 10.0;
                    double dLineLength = fCogLine.Length * dResolution;
                    strTextLength = string.Format("{0:F3} mm", dLineLength);
                    CogGraphicLabel cogLabel = new CogGraphicLabel();
                    {
                        cogLabel.BackgroundColor = CogColorConstants.None;
                        cogLabel.Color = CogColorConstants.Yellow;
                        cogLabel.SetXYText(dPosLabelX, dPosLabelY, strTextLength);
                        cogLabel.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 14.0f, FontStyle.Bold);
                        cogLabel.Interactive = false;
                        cogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                    }
                    fDisplay.InteractiveGraphics.Add(cogLabel, strGroup, false);
                }
                return true;
            }

            public static CogRectangle DrawRect(CogDisplay fDisplay, CogColorConstants nColor, double dCenterX, double dCenterY, double dWidth, double dHeight, bool bInteractive = false, string strGroup = "")
            {
                ////  ROI 사각형 그리기
                CogRectangle fCogRectROI = new CogRectangle();
                {
                    fCogRectROI.X = dCenterX;
                    fCogRectROI.Y = dCenterY;
                    fCogRectROI.Width = dWidth;
                    fCogRectROI.Height = dHeight;
                    fCogRectROI.Color = nColor;
                    fCogRectROI.GraphicDOFEnable = CogRectangleDOFConstants.All;
                    fCogRectROI.Interactive = bInteractive;
                }
                fDisplay.InteractiveGraphics.Add(fCogRectROI, strGroup, false);
                return fCogRectROI;
            }

            public static CogLineSegment DrawLine(CogDisplay fDisplay, CogColorConstants nColor, double dStartX, double dStartY, double dEndX, double dEndY, bool bInteractive = false, string strGroup = "")
            {
                CogLineSegment fCogLine = new CogLineSegment();
                {
                    fCogLine.Color = CogColorConstants.Blue;
                    fCogLine.SetStartEnd(dStartX, dStartY, dEndX, dEndY);
                    fCogLine.LineWidthInScreenPixels = 2;
                    fCogLine.Color = nColor;
                    fCogLine.Interactive = bInteractive;
                }
                fDisplay.InteractiveGraphics.Add(fCogLine, strGroup, false);
                return fCogLine;
            }

            public static void DrawCross(CogDisplay fDisplay, CogColorConstants nColor, double dPointX, double dPointY, double dLineLength, bool bInteractive = false, string strGroup = "")
            {
                CogLineSegment cogLine_Horz = new CogLineSegment();
                {
                    cogLine_Horz.Color = CogColorConstants.Green;
                    cogLine_Horz.SetStartEnd(dPointX - dLineLength, dPointY, dPointX + dLineLength, dPointY);
                    cogLine_Horz.LineWidthInScreenPixels = 2;
                    cogLine_Horz.Color = nColor;
                    cogLine_Horz.Interactive = bInteractive;
                }
                CogLineSegment cogLine_Vert = new CogLineSegment();
                {
                    cogLine_Vert.Color = CogColorConstants.Green;
                    cogLine_Vert.SetStartEnd(dPointX, dPointY - dLineLength, dPointX, dPointY + dLineLength);
                    cogLine_Vert.LineWidthInScreenPixels = 1;
                    cogLine_Vert.Color = nColor;
                    cogLine_Vert.Interactive = bInteractive;
                }
                fDisplay.InteractiveGraphics.Add(cogLine_Horz, strGroup, false);
                fDisplay.InteractiveGraphics.Add(cogLine_Vert, strGroup, false);
            }

            public static CogGraphicLabel DrawText(CogDisplay fDisplay, CogColorConstants nColor, double dPosX, double dPosY, string strText, float dSizeFont = 14.0f, bool bInteractive = false, string strGroup = "")
            {
                CogGraphicLabel fCogLabel = new CogGraphicLabel();
                {
                    fCogLabel.BackgroundColor = CogColorConstants.None;
                    fCogLabel.Color = nColor;
                    fCogLabel.SetXYText(dPosX, dPosY, strText);
                    fCogLabel.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, dSizeFont, FontStyle.Bold);
                    fCogLabel.Interactive = bInteractive;
                    fCogLabel.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                }
                fDisplay.InteractiveGraphics.Add(fCogLabel, strGroup, false);
                return fCogLabel;
            }
            #endregion

        }

        public static class Transform
        {
            #region Image 변형하기 (Crop, Resize, Affine)

            public static ICogImage Crop(ICogImage cogImage, double dCenterX, double dCenterY, double dCropWidth, double dCropHeight)
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
                    cogAffineTool.InputImage = cogImage;
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

            public static ICogImage Crop(ICogImage cogImage, ICogRegion cogRegion)
            {
                if (cogImage == null) return null;
                switch (cogRegion)
                {
                    case CogRectangle cogRect:
                        return Crop(cogImage, cogRect);
                    case CogRectangleAffine cogRectAffine:
                        return Crop(cogImage, cogRectAffine);
                    default:
                        break;
                }
                return null;
            }

            public static ICogImage Crop(ICogImage cogImage, CogRectangle cogRectCrop)
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
                    cogRectRegion.CenterX = cogRectCrop.CenterX;
                    cogRectRegion.CenterY = cogRectCrop.CenterY;
                    cogRectRegion.SideXLength = cogRectCrop.Width;
                    cogRectRegion.SideYLength = cogRectCrop.Height;
                    cogRectRegion.Rotation = 0.0;
                    cogRectRegion.Skew = 0.0;
                }
                CogAffineTransformTool cogAffineTool = new CogAffineTransformTool();
                {
                    cogAffineTool.InputImage = cogImage;
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

            public static ICogImage Crop(ICogImage cogImage, CogRectangleAffine cogRectCrop)
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
                    cogAffineTool.InputImage = cogImage;
                    cogAffineTool.Region = cogRectCrop;
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

            public static ICogImage Resize(ICogImage cogImage, double dSourceWidth, double dSourceHeight)
            {
                ////  CogAffineTransform 설정
                CogAffineTransform cogAffine = new CogAffineTransform();
                {
                    cogAffine.ScalingX = dSourceWidth / cogImage.Width;
                    cogAffine.ScalingY = dSourceHeight / cogImage.Height;
                    cogAffine.SamplingMode = CogAffineTransformSamplingModeConstants.BilinearInterpolation;
                }
                CogAffineTransformTool cogAffineTool = new CogAffineTransformTool();
                {
                    //////  Object의 너비.높이를 Source와 동일하게 맞춤.
                    cogAffineTool.InputImage = cogImage;
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

            public static ICogImage Resize(ICogImage cogImage, CogRectangle cogRectFrame)
            {
                ////  CogAffineTransform 설정
                CogAffineTransform cogAffine = new CogAffineTransform();
                {
                    cogAffine.ScalingX = cogRectFrame.Width / cogImage.Width;
                    cogAffine.ScalingY = cogRectFrame.Height / cogImage.Height;
                    cogAffine.SamplingMode = CogAffineTransformSamplingModeConstants.BilinearInterpolation;
                }
                CogAffineTransformTool cogAffineTool = new CogAffineTransformTool();
                {
                    //////  Object의 너비.높이를 Source와 동일하게 맞춤.
                    cogAffineTool.InputImage = cogImage;
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

            #endregion

        }

        public static class TwoImageProcess
        {
            #region 차영상, 곱영상 구하기 (Substract, Add ...)
            public static ICogImage Substract(ICogImage cogInputImageA, ICogImage cogInputImageB)
            {
                ////  Param 설정
                CogIPTwoImageSubtract cogIPSubstractParam = new CogIPTwoImageSubtract();
                {
                    cogIPSubstractParam.OverflowMode = CogIPTwoImageSubtractOverflowModeConstants.Bounded;
                }
                CogIPTwoImageSubtractTool cogIPSubstractTool = new CogIPTwoImageSubtractTool();
                {
                    cogIPSubstractTool.InputImageA = cogInputImageA;
                    cogIPSubstractTool.InputImageB = cogInputImageB;
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

            public static ICogImage Add(ICogImage cogInputImageA, ICogImage cogInputImageB)
            {
                ////  Param 설정
                CogIPTwoImageAdd cogIPAddParam = new CogIPTwoImageAdd();
                {
                    cogIPAddParam.OverflowMode = CogIPTwoImageAddOverflowModeConstants.Bounded;
                }
                CogIPTwoImageAddTool cogIPAddTool = new CogIPTwoImageAddTool();
                {
                    cogIPAddTool.InputImageA = cogInputImageA;
                    cogIPAddTool.InputImageB = cogInputImageB;
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

            public static ICogImage OverlapMax(ICogImage cogInputImageA, ICogImage cogInputImageB)
            {
                ////  Param 설정
                CogIPTwoImageMinMax cogIPMinMaxParam = new CogIPTwoImageMinMax();
                {
                    cogIPMinMaxParam.Operation = CogIPTwoImageMinMaxOperationConstants.Max;
                }
                CogIPTwoImageMinMaxTool cogIPMinMaxTool = new CogIPTwoImageMinMaxTool();
                {
                    cogIPMinMaxTool.InputImageA = cogInputImageA;
                    cogIPMinMaxTool.InputImageB = cogInputImageB;
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

            public static ICogImage OverlapMin(ICogImage cogInputImageA, ICogImage cogInputImageB)
            {
                ////  Param 설정
                CogIPTwoImageMinMax cogIPMinMaxParam = new CogIPTwoImageMinMax();
                {
                    cogIPMinMaxParam.Operation = CogIPTwoImageMinMaxOperationConstants.Min;
                }
                CogIPTwoImageMinMaxTool cogIPMinMaxTool = new CogIPTwoImageMinMaxTool();
                {
                    cogIPMinMaxTool.InputImageA = cogInputImageA;
                    cogIPMinMaxTool.InputImageB = cogInputImageB;
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
            #endregion
        }
    }
}
