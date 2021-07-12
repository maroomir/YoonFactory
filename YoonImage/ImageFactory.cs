using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace YoonFactory.Image
{
    /// <summary>
    /// Self-made Image Processing Class by .Net Framework
    /// </summary>
    public static class ImageFactory
    {
        private const uint MAX_LABEL = 10000;
        private const uint MAX_OBJECT = 10000;
        private const uint MAX_PICK_NUM = 100;
        private const uint MAX_FILL_NUM = 1000;

        public static IYoonObject FindPatternAsBinary(this YoonImage pSourceImage, YoonImage pPatternImage) => PatternMatch.FindPatternAsBinary(pPatternImage, pSourceImage);
        public static IYoonObject FindPatternAsBinary(this YoonImage pSourceImage, YoonRect2N scanArea, YoonImage pPatternImage) => PatternMatch.FindPatternAsBinary(scanArea, pPatternImage, pSourceImage);
        public static IYoonObject FindPattern(this YoonImage pSourceImage, YoonImage pPatternImage, int nDiffThreshold = 10) => PatternMatch.FindPattern(pPatternImage, pSourceImage, nDiffThreshold);
        public static IYoonObject FindPattern(this YoonImage pSourceImage, YoonRect2N scanArea, YoonImage pPatternImage, int nDiffThreshold = 10) => PatternMatch.FindPattern(scanArea, pPatternImage, pSourceImage, nDiffThreshold);
        public static YoonImage Combine(this YoonImage pSourceImage, YoonImage pObjectImage) => TwoImageProcess.Combine(pSourceImage, pObjectImage);
        public static YoonImage Add(this YoonImage pSourceImage, YoonImage pObjectImage) => TwoImageProcess.Add(pSourceImage, pObjectImage);
        public static YoonImage Subtract(this YoonImage pSourceImage, YoonImage pObjectImage) => TwoImageProcess.Subtract(pSourceImage, pObjectImage);
        public static YoonImage Sobel(this YoonImage pSourceImage, int nIntensity, bool bCombine = true) => Filter.Sobel(pSourceImage, nIntensity, bCombine);
        public static YoonImage Laplacian(this YoonImage pSourceImage, int nIntensity, bool bCombine = true) => Filter.Laplacian(pSourceImage, nIntensity, bCombine);
        public static YoonImage RC1D(this YoonImage pSourceImage, double dFrequency, bool bCombine = true) => Filter.RC1D(pSourceImage, dFrequency, bCombine);
        public static YoonImage RC2D(this YoonImage pSourceImage, double dFrequency, bool bCombine = true) => Filter.RC2D(pSourceImage, dFrequency, bCombine);
        public static YoonImage Level2D(this YoonImage pSourceImage, ref double dSum) => Filter.Level2D(pSourceImage, ref dSum);
        public static YoonImage DeMargin2D(this YoonImage pSourceImage) => Filter.DeMargin2D(pSourceImage);
        public static YoonImage Smooth1D(this YoonImage pSourceImage, int nMargin = 1, int nStep = 3) => Filter.Smooth1D(pSourceImage, nMargin, nStep);
        public static YoonImage Smooth2D(this YoonImage pSourceImage, int nStep = 5) => Filter.Smooth2D(pSourceImage, nStep);
        public static IYoonObject FindMaxBlob(this YoonImage pSourceImage, YoonRect2N scanArea, byte nThreshold = 128, bool bWhite = false) => Blob.FindMaxBlob(pSourceImage, scanArea, nThreshold, bWhite);
        public static IYoonObject FindMaxBlob(this YoonImage pSourceImage, byte nThreshold = 128, bool bWhite = false) => Blob.FindMaxBlob(pSourceImage, nThreshold, bWhite);
        public static ObjectList<YoonRect2N> FindBlobs(this YoonImage pSourceImage, YoonRect2N scanArea, byte nThreshold = 128, bool bWhite = false) => Blob.FindBlobs(pSourceImage, scanArea, nThreshold, bWhite);
        public static ObjectList<YoonRect2N> FindBlobs(this YoonImage pSourceImage, byte nThreshold = 128, bool bWhite = false) => Blob.FindBlobs(pSourceImage, nThreshold, bWhite);
        public static YoonImage Binarize(this YoonImage pSourceImage, YoonRect2N scanArea, byte nThreshold = 128) => Binary.Binarize(pSourceImage, scanArea, nThreshold);
        public static YoonImage Binarize(this YoonImage pSourceImage, byte nThreshold = 128) => Binary.Binarize(pSourceImage, nThreshold);
        public static YoonImage Erosion(this YoonImage pSourceImage) => Morphology.Erosion(pSourceImage);
        public static YoonImage Erosion(this YoonImage pSourceImage, YoonRect2N scanArea) => Morphology.Erosion(pSourceImage, scanArea);
        public static YoonImage ErosionAsBinary(this YoonImage pSourceImage, int nMophSize) => Morphology.ErosionAsBinary(pSourceImage, nMophSize);
        public static YoonImage ErosionAsBinary(this YoonImage pSourceImage, YoonRect2N scanArea) => Morphology.ErosionAsBinary(pSourceImage, scanArea);
        public static YoonImage Dilation(this YoonImage pSourceImage) => Morphology.Dilation(pSourceImage);
        public static YoonImage Dilation(this YoonImage pSourceImage, YoonRect2N scanArea) => Morphology.Dilation(pSourceImage, scanArea);
        public static YoonImage DilationAsBinary(this YoonImage pSourceImage) => Morphology.DilationAsBinary(pSourceImage);
        public static YoonImage DilationAsBinary(this YoonImage pSourceImage, int nMophSize) => Morphology.DilationAsBinary(pSourceImage, nMophSize);
        public static YoonImage DilationAsBinary(this YoonImage pSourceImage, YoonRect2N scanArea) => Morphology.DilationAsBinary(pSourceImage, scanArea);
        public static YoonImage DilationAsBinary(this YoonImage pSourceImage, YoonRect2N scanArea, int nMophSize) => Morphology.DilationAsBinary(pSourceImage, scanArea, nMophSize);
        public static byte GetNumerousThreshold(this YoonImage pSourceImage) => PixelInspector.GetNumerousThreshold(pSourceImage);
        public static byte GetNumerousThreshold(this YoonImage pSourceImage, YoonRect2N scanArea) => PixelInspector.GetNumerousThreshold(pSourceImage, scanArea);
        public static byte GetAverageThreshold(this YoonImage pSourceImage, YoonRect2N scanArea) => PixelInspector.GetAverageThreshold(pSourceImage, scanArea);
        public static byte GetMinMaxThreshold(this YoonImage pSourceImage, YoonRect2N scanArea) => PixelInspector.GetMinMaxThreshold(pSourceImage, scanArea);
        public static void GetLevelInfo(this YoonImage pSourceImage, YoonRect2N scanArea, out int Min, out int Max, out int Average) => PixelInspector.GetLevelInfo(pSourceImage, scanArea, out Min, out Max, out Average);
        public static YoonImage Zoom(this YoonImage pSourceImage, double dRatio) => Transform.Zoom(pSourceImage, dRatio);
        public static YoonImage Zoom(this YoonImage pSourceImage, double dRatioX, double dRatioY) => Transform.Zoom(pSourceImage, dRatioX, dRatioY);
        public static YoonImage Rotate(this YoonImage pSourceImage, YoonVector2N vecCenter, double dAngle) => Transform.Rotate(pSourceImage, vecCenter, dAngle);
        public static YoonImage Reverse(this YoonImage pSourceImage) => Transform.Reverse(pSourceImage);
        public static YoonImage Warp(this YoonImage pSourceImage, YoonRectAffine2D pRect) => Transform.Warp(pSourceImage, pRect);

        // Converter
        public static class Converter
        {
            public static byte[] To8BitGrayBuffer(int[] pBuffer, int nWidth, int nHeight)
            {
                if (pBuffer.Length != nWidth * nHeight) return null;

                byte[] pByte = new byte[pBuffer.Length];
                Parallel.For(0, nHeight, j =>
                {
                    for (int i = 0; i < nWidth; i++)
                    {
                        byte[] pBytePixel = BitConverter.GetBytes(pBuffer[j * nWidth + i]); // Order by {B/G/R/A}
                        pByte[j * nWidth + i] =
                            (byte) (0.299f * pBytePixel[2] + 0.587f * pBytePixel[1] +
                                    0.114f * pBytePixel[0]); // ITU-RBT.709, YPrPb
                    }
                });
                return pByte;
            }

            public static byte[] To8BitGrayBufferWithRescaling<T>(T[] pBuffer, int nWidth, int nHeight)
                where T : IComparable, IComparable<T>
            {
                if (pBuffer.Length != nWidth * nHeight) return null;

                byte[] pByte = new byte[pBuffer.Length];
                double nValueMax = 0;
                double nValueMin = 65536;
                double dRatio = 1.0;
                Parallel.For(0, nHeight, j =>
                {
                    for (int i = 0; i < nWidth; i++)
                    {
                        double value = Convert.ToDouble(pBuffer[j * nWidth + i]);
                        if (value > nValueMax) nValueMax = value;
                        if (value < nValueMin) nValueMin = value;
                    }
                });
                // Adjust the full gray level
                if (nValueMax > 255) dRatio = 255.0 / nValueMax;
                Parallel.For(0, nHeight, j =>
                {
                    for (int i = 0; i < nWidth; i++)
                    {
                        pByte[j * nWidth + i] = Convert.ToByte(Convert.ToDouble(pBuffer[j * nWidth + i]) * dRatio);
                    }
                });
                return pByte;
            }

            public static int[] To24BitColorBuffer(byte[] pRed, byte[] pGreen, byte[] pBlue, int nWidth, int nHeight)
            {
                if (pRed == null || pRed.Length != nWidth * nHeight ||
                    pGreen == null || pGreen.Length != nWidth * nHeight ||
                    pBlue == null || pBlue.Length != nWidth * nHeight)
                    return null;

                int[] pPixel = new int[nWidth * nHeight];
                Parallel.For(0, nHeight, j =>
                {
                    for (int i = 0; i < nWidth; i++)
                    {
                        byte[] pBytePixel = new byte[4];
                        pBytePixel[0] = pBlue[j * nWidth + i];
                        pBytePixel[1] = pGreen[j * nWidth + i];
                        pBytePixel[2] = pRed[j * nWidth + i];
                        pBytePixel[3] = (byte) 0;
                        pPixel[i] = BitConverter.ToInt32(pBytePixel, 0);
                    }
                });
                return pPixel;
            }

            public static int[] To24BitColorBufferWithUpscaling<T>(T[] pBuffer, int nWidth, int nHeight)
                where T : IComparable, IComparable<T>
            {
                if (pBuffer.Length != nWidth * nHeight) return null;

                int[] pPixel = new int[pBuffer.Length];
                Parallel.For(0, nHeight, j =>
                {
                    for (int i = 0; i < nWidth; i++)
                    {
                        pPixel[j * nWidth + i] = 3 * Math.Max((byte) 0,
                            Math.Min(Convert.ToByte(pBuffer[j * nWidth + i]), (byte) 255));
                    }
                });
                return pPixel;
            }

            public static byte[] To8BitRedBuffer(int[] pBuffer, int nWidth, int nHeight)
            {
                if (pBuffer.Length != nWidth * nHeight) return null;

                byte[] pByte = new byte[pBuffer.Length];
                Parallel.For(0, nHeight, j =>
                {
                    for (int i = 0; i < nWidth; i++)
                    {
                        byte[] pBytePixel = BitConverter.GetBytes(pBuffer[j * nWidth + i]); // Order by {B/G/R/A}
                        pByte[j * nWidth] = pBytePixel[2];
                    }
                });
                return pByte;
            }

            public static byte[] To8BitGreenBuffer(int[] pBuffer, int nWidth, int nHeight)
            {
                if (pBuffer.Length != nWidth * nHeight) return null;

                byte[] pByte = new byte[pBuffer.Length];
                Parallel.For(0, nHeight, j =>
                {
                    for (int i = 0; i < nWidth; i++)
                    {
                        byte[] pBytePixel = BitConverter.GetBytes(pBuffer[j * nWidth + i]); // Order by {B/G/R/A}
                        pByte[j * nWidth] = pBytePixel[1];
                    }
                });
                return pByte;
            }

            public static byte[] To8BitBlueBuffer(int[] pBuffer, int nWidth, int nHeight)
            {
                if (pBuffer.Length != nWidth * nHeight) return null;

                byte[] pByte = new byte[pBuffer.Length];
                Parallel.For(0, nHeight, j =>
                {
                    for (int i = 0; i < nWidth; i++)
                    {
                        byte[] pBytePixel = BitConverter.GetBytes(pBuffer[j * nWidth + i]); // Order by {B/G/R/A}
                        pByte[j * nWidth] = pBytePixel[0];
                    }
                });
                return pByte;
            }
        }

        // Pattern Match
        public static class PatternMatch
        {
            public static IYoonObject FindPatternAsBinary(YoonImage pPatternImage, YoonImage pSourceImage)
            {
                if (pPatternImage.Format != PixelFormat.Format8bppIndexed ||
                    pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                YoonRect2N pResultRect = FindPatternAsBinary(pPatternImage.GetGrayBuffer(), pPatternImage.Width,
                    pPatternImage.Height, pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height,
                    out double dScore, out int nPixelCount, true);
                return new YoonObject<YoonRect2N>(0, pResultRect, pSourceImage.CropImage(pResultRect), dScore,
                    nPixelCount);
            }

            public static YoonRect2N FindPatternAsBinary(byte[] pPatternBuffer, int nPatternWidth, int nPatternHeight,
                byte[] pSourceBuffer, int nSourceWidth, int nSourceHeight, out double dScore, out int nPixelCount,
                bool bWhite = true)
            {
                // Init
                YoonRect2N pFindRect = new YoonRect2N(0, 0, 0, 0);
                // Set-up the skip parameter
                int nJumpX = nPatternWidth / 30;
                int nJumpY = nPatternHeight / 30;
                if (nJumpX < 1) nJumpX = 1;
                if (nJumpY < 1) nJumpY = 1;
                int nFindPosX = 0;
                int nFindPosY = 0;
                double dCoefficient = 0.0;
                // Find the matching accuracy and count
                int nCountWhiteMax = 0;
                int nCountBlackMax = 0;
                for (int iY = 0; iY < nSourceHeight - nPatternHeight; iY += 1)
                {
                    for (int iX = 0; iX < nSourceWidth - nPatternWidth; iX += 1)
                    {
                        int nStartX = iX;
                        int nStartY = iY;
                        // Find the difference within the whole image
                        int nCountWhite = 0;
                        int nCountBlack = 0;
                        int nTotalCountWhite = 0;
                        int nTotalCountBlack = 0;
                        for (int y = 0; y < nPatternHeight - nJumpY; y += nJumpY)
                        {
                            for (int x = 0; x < nPatternWidth - nJumpX; x += nJumpX)
                            {
                                int nGraySource = pSourceBuffer[(nStartY + y) * nSourceWidth + nStartX + x];
                                int nGrayPattern = pPatternBuffer[y * nPatternWidth + x];
                                // If the gray levels are same, then increase the match count
                                if (nGrayPattern == 0)
                                {
                                    nTotalCountBlack++;
                                    if (nGrayPattern == nGraySource)
                                        nCountBlack++;
                                }

                                if (nGrayPattern == 255)
                                {
                                    nTotalCountWhite++;
                                    if (nGrayPattern == nGraySource)
                                        nCountWhite++;
                                }
                            }
                        }
                        if (bWhite)
                        {
                            if (nCountWhite > nCountWhiteMax)
                            {
                                nCountWhiteMax = nCountWhite;
                                nFindPosX = iX;
                                nFindPosY = iY;
                                dCoefficient = 0.0;
                                if (nTotalCountWhite > 1)
                                    dCoefficient = nCountWhite * 100.0 / nTotalCountWhite;
                            }
                        }
                        else
                        {
                            if (nCountBlack > nCountBlackMax)
                            {
                                nCountBlackMax = nCountBlack;
                                nFindPosX = iX;
                                nFindPosY = iY;
                                dCoefficient = 0.0;
                                if (nTotalCountBlack > 1)
                                    dCoefficient = nCountBlack * 100.0 / nTotalCountBlack;
                            }
                        }
                    }
                }

                pFindRect.CenterPos.X = nFindPosX;
                pFindRect.CenterPos.Y = nFindPosY;
                pFindRect.Width = nPatternWidth;
                pFindRect.Height = nPatternHeight;
                dScore = dCoefficient;
                nPixelCount = (bWhite) ? nCountWhiteMax : nCountBlackMax;
                return pFindRect;
            }

            public static IYoonObject FindPatternAsBinary(YoonRect2N pScanArea, YoonImage pPatternImage,
                YoonImage pSourceImage)
            {
                if (pPatternImage.Format != PixelFormat.Format8bppIndexed ||
                    pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                if (!pPatternImage.IsVerifiedArea(pScanArea))
                    throw new ArgumentOutOfRangeException("[YOONIMAGE EXCEPTION] Scan area is not verified");
                YoonRect2N pResultRect = FindPatternAsBinary(pScanArea, pPatternImage.GetGrayBuffer(),
                    pPatternImage.Width, pPatternImage.Height, pSourceImage.GetGrayBuffer(), pSourceImage.Width,
                    pSourceImage.Height, out double dScore, out int nPixelCount);
                return new YoonObject<YoonRect2N>(0, pResultRect, pSourceImage.CropImage(pResultRect), dScore,
                    nPixelCount);
            }

            public static YoonRect2N FindPatternAsBinary(YoonRect2N pScanArea, byte[] pPatternBuffer, int patternWidth, int patternHeight, byte[] pSourceBuffer, int sourceWidth, int sourceHeight, out double score, out int pixelCount)
            {
                YoonRect2N findRect = new YoonRect2N(0, 0, 0, 0);
                double dCoefficient = 0.0;
                int nJumpX = patternWidth / 30;
                int nJumpY = patternHeight / 30;
                if (nJumpX < 1) nJumpX = 1;
                if (nJumpY < 1) nJumpY = 1;
                int nFindPosX = 0;
                int nFindPosY = 0;
                var matchCountMax = 0;
                for (int iY = pScanArea.Top; iY < pScanArea.Bottom - patternHeight; iY += 1)
                {
                    for (int iX = pScanArea.Left; iX < pScanArea.Right - patternWidth; iX += 1)
                    {
                        int nStartX = iX;
                        int nStartY = iY;
                        var matchCount = 0;
                        int nCountWhite = 0;
                        int nCountBlack = 0;
                        int nTotalCountWhite = 0;
                        int nTotalCountBlack = 0;
                        for (int y = 0; y < patternHeight - nJumpY; y += nJumpY)
                        {
                            for (int x = 0; x < patternWidth - nJumpX; x += nJumpX)
                            {
                                int nGraySource = pSourceBuffer[(nStartY + y) * sourceWidth + nStartX + x];
                                int nGrayPattern = pPatternBuffer[y * patternWidth + x];
                                if (nGrayPattern == nGraySource) matchCount++;
                                switch (nGrayPattern)
                                {
                                    case 0:
                                    {
                                        nTotalCountBlack++;
                                        if (nGrayPattern == nGraySource)
                                            nCountBlack++;
                                        break;
                                    }
                                    case 255:
                                    {
                                        nTotalCountWhite++;
                                        if (nGrayPattern == nGraySource)
                                            nCountWhite++;
                                        break;
                                    }
                                }
                            }
                        }
                        matchCount = nCountBlack;
                        if (matchCount > matchCountMax)
                        {
                            matchCountMax = matchCount;
                            nFindPosX = iX;
                            nFindPosY = iY;
                            dCoefficient = 0.0;
                            if (nTotalCountBlack > nTotalCountWhite)
                                dCoefficient = nCountBlack * 100.0 / nTotalCountBlack;
                            else
                                dCoefficient = nCountWhite * 100.0 / nTotalCountWhite;
                        }
                    }
                }
                findRect.CenterPos.X = nFindPosX;
                findRect.CenterPos.Y = nFindPosY;
                findRect.Width = patternWidth;
                findRect.Height = patternHeight;
                score = dCoefficient;
                pixelCount = matchCountMax;
                return findRect;
            }

            public static IYoonObject FindPattern(YoonImage pPatternImage, YoonImage pSourceImage,
                int nDiffThreshold = 10)
            {
                if (pPatternImage.Plane == 1 && pSourceImage.Plane == 1)
                {
                    YoonRect2N pRectResult = FindPattern(pPatternImage.GetGrayBuffer(), pPatternImage.Width,
                        pPatternImage.Height, pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height,
                        (byte) nDiffThreshold, out double dScore, out int nPixelCount);
                    return new YoonObject<YoonRect2N>(0, pRectResult, pSourceImage.CropImage(pRectResult), dScore,
                        nPixelCount);
                }

                if (pPatternImage.Plane == 4 && pSourceImage.Plane == 4)
                {
                    YoonRect2N pRectResult = FindPattern(pPatternImage.GetARGBBuffer(), pPatternImage.Width,
                        pPatternImage.Height, pSourceImage.GetARGBBuffer(), pSourceImage.Width, pSourceImage.Height,
                        nDiffThreshold, out double dScore, out int nPixelCount);
                    return new YoonObject<YoonRect2N>(0, pRectResult, pSourceImage.CropImage(pRectResult), dScore,
                        nPixelCount);
                }

                throw new FormatException("[YOONIMAGE EXCEPTION] Image format arguments is not comportable");
            }

            public static YoonRect2N FindPattern(byte[] pPatternBuffer, int patternWidth, int patternHeight,
                byte[] pSourceBuffer, int sourceWidth, int sourceHeight, byte diffThreshold, out double score,
                out int pixelCount)
            {
                YoonRect2N pFindRect = new YoonRect2N(0, 0, 0, 0);
                int nDiffMin = 2147483647;
                int nDiffSum = 0;
                int nCount = 0;
                int nFindPosX = 0;
                int nFindPosY = 0;
                // Set-up the skip parameter
                int nJumpX = patternWidth / 30;
                int nJumpY = patternHeight / 30;
                if (nJumpX < 1) nJumpX = 1;
                if (nJumpY < 1) nJumpY = 1;
                // Find the matching accuracy and count
                for (int iY = 0; iY < sourceHeight - patternHeight; iY += 1)
                {
                    for (int iX = 0; iX < sourceWidth - patternWidth; iX += 1)
                    {
                        int nStartX = iX;
                        int nStartY = iY;
                        // Find the difference within the whole image
                        nDiffSum = 0;
                        for (int y = 0; y < patternHeight - nJumpY; y += nJumpY)
                        {
                            for (int x = 0; x < patternWidth - nJumpX; x += nJumpX)
                            {
                                int nGraySource = pSourceBuffer[(nStartY + y) * sourceWidth + nStartX + x];
                                int nGrayPattern = pPatternBuffer[y * patternWidth + x];
                                nDiffSum += Math.Abs(nGraySource - nGrayPattern);
                                if (Math.Abs(nGraySource - nGrayPattern) < diffThreshold)
                                    nCount++;
                            }
                        }

                        // Find the lower limit of difference
                        if (nDiffSum < nDiffMin)
                        {
                            nDiffMin = nDiffSum;
                            nFindPosX = iX;
                            nFindPosY = iY;
                        }
                    }
                }

                pFindRect.CenterPos.X = nFindPosX;
                pFindRect.CenterPos.Y = nFindPosY;
                pFindRect.Width = patternWidth;
                pFindRect.Height = patternHeight;
                // Find the coefficient
                byte[] pTempBuffer = new byte[patternWidth * patternHeight];
                for (int j = 0; j < patternHeight; j++)
                for (int i = 0; i < patternWidth; i++)
                    pTempBuffer[j * patternWidth + i] = pSourceBuffer[(nFindPosY + j) * sourceWidth + (nFindPosX + i)];
                double dCoefficient =
                    MathFactory.GetCorrelationCoefficient(pPatternBuffer, pTempBuffer, patternWidth, patternHeight);
                score = dCoefficient;
                pixelCount = nCount;
                return pFindRect;
            }

            public static YoonRect2N FindPattern(int[] pPatternBuffer, int patternWidth, int patternHeight,
                int[] pSourceBuffer, int sourceWidth, int sourceHeight, int diffThreshold, out double score,
                out int pixelCount)
            {
                YoonRect2N findRect = new YoonRect2N(0, 0, 0, 0);
                int nDiffMin = 2147483647;
                int nDiffSum = 0;
                int nCount = 0;
                int nFindPosX = 0;
                int nFindPosY = 0;
                int nJumpX = patternWidth / 60;
                int nJumpY = patternHeight / 60;
                if (nJumpX < 1) nJumpX = 1;
                if (nJumpY < 1) nJumpY = 1;
                for (int iY = 0; iY < sourceHeight - patternHeight; iY += 1)
                {
                    for (int iX = 0; iX < sourceWidth - patternWidth; iX += 1)
                    {
                        int nStartX = iX;
                        int nStartY = iY;
                        nDiffSum = 0;
                        for (int y = 0; y < patternHeight - nJumpY; y += nJumpY)
                        {
                            for (int x = 0; x < patternWidth - nJumpX; x += nJumpX)
                            {
                                int nGraySource = pSourceBuffer[(nStartY + y) * sourceWidth + nStartX + x];
                                int nGrayPattern = pPatternBuffer[y * patternWidth + x];
                                nDiffSum += Math.Abs(nGraySource - nGrayPattern);
                                if (Math.Abs(nGraySource - nGrayPattern) < diffThreshold)
                                    nCount++;
                            }
                        }

                        if (nDiffSum < nDiffMin)
                        {
                            nDiffMin = nDiffSum;
                            nFindPosX = iX;
                            nFindPosY = iY;
                        }
                    }
                }

                findRect.CenterPos.X = nFindPosX;
                findRect.CenterPos.Y = nFindPosY;
                findRect.Width = patternWidth;
                findRect.Height = patternHeight;
                int[] pTempBuffer = new int[patternWidth * patternHeight];
                for (int j = 0; j < patternHeight; j++)
                for (int i = 0; i < patternWidth; i++)
                    pTempBuffer[j * patternWidth + i] = pSourceBuffer[(nFindPosY + j) * sourceWidth + (nFindPosX + i)];
                double dCoefficient =
                    MathFactory.GetCorrelationCoefficient(pPatternBuffer, pTempBuffer, patternWidth, patternHeight);
                score = dCoefficient;
                pixelCount = nCount;
                return findRect;
            }

            public static IYoonObject FindPattern(YoonRect2N scanArea, YoonImage pPatternImage, YoonImage pSourceImage,
                int nDiffThreshold)
            {
                if (pPatternImage.Format != PixelFormat.Format8bppIndexed ||
                    pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                if (!pPatternImage.IsVerifiedArea(scanArea))
                    throw new ArgumentOutOfRangeException("[YOONIMAGE EXCEPTION] Scan area is not verified");
                double dScore;
                int nPixelCount;
                YoonRect2N pRectResult = FindPattern(scanArea, pPatternImage.GetGrayBuffer(), pPatternImage.Width,
                    pPatternImage.Height, pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height,
                    nDiffThreshold, out dScore, out nPixelCount);
                return new YoonObject<YoonRect2N>(0, pRectResult, pSourceImage.CropImage(pRectResult), dScore,
                    nPixelCount);
            }

            public static YoonRect2N FindPattern(YoonRect2N scanArea, byte[] pPatternBuffer, int patternWidth,
                int patternHeight, byte[] pSourceBuffer, int sourceWidth, int sourceHeight, int diffThreshold,
                out double score, out int pixelCount)
            {
                YoonRect2N findRect = new YoonRect2N(0, 0, 0, 0);
                if (patternWidth < 1 || patternHeight < 1)
                    throw new ArgumentException("[YOONIMAGE EXCEPTION] Pattern size is not verified");
                int nDiffMin = 2147483647;
                int nDiffSum = 0;
                int nCount = 0;
                int nFindPosX = 0;
                int nFindPosY = 0;
                int nJumpX = patternWidth / 60;
                int nJumpY = patternHeight / 60;
                if (nJumpX < 1) nJumpX = 1;
                if (nJumpY < 1) nJumpY = 1;
                for (int iY = scanArea.Top; iY < scanArea.Bottom - patternHeight; iY += 2)
                {
                    for (int iX = scanArea.Left; iX < scanArea.Right - patternWidth; iX += 2)
                    {
                        int nStartX = iX;
                        int nStartY = iY;
                        nDiffSum = 0;
                        for (int y = 0; y < patternHeight - nJumpY; y += nJumpY)
                        {
                            for (int x = 0; x < patternWidth - nJumpX; x += nJumpX)
                            {
                                int nGraySource = pSourceBuffer[(nStartY + y) * sourceWidth + nStartX + x];
                                int nGrayPattern = pPatternBuffer[y * patternWidth + x];
                                nDiffSum += Math.Abs(nGraySource - nGrayPattern);
                                if (Math.Abs(nGraySource - nGrayPattern) < diffThreshold)
                                    nCount++;
                            }
                        }

                        if (nDiffSum < nDiffMin)
                        {
                            nDiffMin = nDiffSum;
                            nFindPosX = iX;
                            nFindPosY = iY;
                        }
                    }
                }

                findRect.CenterPos.X = nFindPosX;
                findRect.CenterPos.Y = nFindPosY;
                findRect.Width = patternWidth;
                findRect.Height = patternHeight;
                byte[] pTempBuffer = new byte[patternWidth * patternHeight];
                for (int j = 0; j < patternHeight; j++)
                for (int i = 0; i < patternWidth; i++)
                    pTempBuffer[j * patternWidth + i] = pSourceBuffer[(nFindPosY + j) * sourceWidth + (nFindPosX + i)];
                double dCoefficient =
                    MathFactory.GetCorrelationCoefficient(pPatternBuffer, pTempBuffer, patternWidth, patternHeight);
                pixelCount = nCount;
                score = dCoefficient;
                return findRect;
            }
        }

        public static class TwoImageProcess
        {
            public static YoonImage Combine(YoonImage pSourceImage, YoonImage pObjectImage)
            {
                if (pSourceImage.Width != pObjectImage.Width || pSourceImage.Height != pObjectImage.Height)
                    throw new ArgumentException("[YOONIMAGE EXCEPTION] Source and object size is not same");
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                return new YoonImage(
                    Combine(pSourceImage.GetGrayBuffer(), pObjectImage.GetGrayBuffer(), pSourceImage.Width,
                        pSourceImage.Height),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Combine(byte[] pSourceBuffer, byte[] pObjectBuffer, int nWidth, int nHeight)
            {
                byte[] pResultBuffer = new byte[nWidth * nHeight];
                Parallel.For(0, nHeight, j =>
                {
                    for (int i = 0; i < nWidth; i++)
                    {
                        if (pSourceBuffer[j * nWidth + i] > pObjectBuffer[j * nWidth + i])
                            pResultBuffer[j * nWidth + i] = pSourceBuffer[j * nWidth + i];
                        else
                            pResultBuffer[j * nWidth + i] = pObjectBuffer[j * nWidth + i];
                    }
                });
                return pResultBuffer;
            }

            public static YoonImage Add(YoonImage pSourceImage, YoonImage pObjectImage)
            {
                if (pSourceImage.Width != pObjectImage.Width || pSourceImage.Height != pObjectImage.Height)
                    throw new ArgumentException("[YOONIMAGE EXCEPTION] Source and object size is not same");
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                return new YoonImage(Add(pSourceImage.GetGrayBuffer(), pObjectImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Add(byte[] pSourceBuffer, byte[] pObjectBuffer, int nWidth, int nHeight)
            {
                int nMaxValue = 0;
                int nMinValue = 1024;
                int[] pTempBuffer = new int[nWidth * nHeight];
                byte[] pResultBuffer = new byte[nWidth * nHeight];
                Parallel.For(0, nHeight, j =>
                {
                    for (int i = 0; i < nWidth; i++)
                    {
                        pTempBuffer[j * nWidth + i] = pSourceBuffer[j * nWidth + i] + pObjectBuffer[j * nWidth + i];
                    }
                });
                // Find the combined buffer of maximum gray level and minimum gray level
                for (int j = 0; j < nHeight; j++)
                {
                    for (int i = 0; i < nWidth; i++)
                    {
                        int nValue = pTempBuffer[j * nWidth + i];
                        if (nValue < nMinValue) nMinValue = nValue;
                        if (nValue > nMaxValue) nMaxValue = nValue;
                    }
                }
                // Adjust the gray level
                if (nMaxValue > 255)
                {
                    double dRatio = 255.0 / nMaxValue;
                    Parallel.For(0, nHeight, j =>
                    {
                        for (int i = 0; i < nWidth; i++)
                        {
                            int nValue = (int)(pTempBuffer[j * nWidth + i] * dRatio);
                            if (nValue > 255) nValue = 255;
                            if (nValue < 0) nValue = 0;
                            pResultBuffer[j * nWidth + i] = (byte)nValue;
                        }
                    });
                }
                return pResultBuffer;
            }

            public static YoonImage Subtract(YoonImage pSourceImage, YoonImage pObjectImage)
            {
                if (pSourceImage.Width != pObjectImage.Width || pSourceImage.Height != pObjectImage.Height)
                    throw new ArgumentException("[YOONIMAGE EXCEPTION] Source and object size is not same");
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                return new YoonImage(Subtract(pSourceImage.GetGrayBuffer(), pObjectImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Subtract(byte[] pSourceBuffer, byte[] pObjectBuffer, int nWidth, int nHeight)
            {
                byte[] pResultBuffer = new byte[nWidth * nHeight];
                Parallel.For(0, nHeight, j =>
                {
                    for (int i = 0; i < nWidth; i++)
                    {
                        int nValue = pSourceBuffer[j * nWidth + i] - pObjectBuffer[j * nWidth + i];
                        if (nValue > 255) nValue = 255;
                        if (nValue < 0) nValue = 0;
                        pResultBuffer[j * nWidth + i] = (byte)nValue;
                    }
                });
                return pResultBuffer;
            }
        }

        // Main Filtering in Image Processing
        public static class Filter
        {
            public static YoonImage Sobel(YoonImage pSourceImage, int nIntensity, bool bCombine = true)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                return new YoonImage(
                    Sobel(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, nIntensity, bCombine),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Sobel(byte[] pBuffer, int nWidth, int nHeight, int nIntensity, bool bCombineSource)
            {
                ////  Sobel Mask 생성.
                int maskValue = nIntensity;
                int[,] mask1 = new int[3, 3]
                {
                    {-maskValue, 0, maskValue},
                    {-maskValue, 0, maskValue},
                    {-maskValue, 0, maskValue}
                };
                int[,] mask2 = new int[3, 3]
                {
                    {maskValue, maskValue, maskValue},
                    {0, 0, 0},
                    {-maskValue, -maskValue, -maskValue}
                };
                int nImageWidth = nWidth;
                int nImageHeight = nHeight;
                int nImageSize = nImageWidth * nImageHeight;
                byte[] pResultBuffer = new byte[nImageSize];
                ////  Sobel Masking
                for (int y = 0; y < nHeight - 2; y++)
                {
                    for (int x = 0; x < nWidth - 2; x++)
                    {
                        int nCenterValue1 = 0;
                        int nCenterValue2 = 0;
                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                int posX = x + i;
                                int posY = y + j;
                                byte nValue = pBuffer[posY * nImageWidth + posX];
                                nCenterValue1 += nValue * mask1[i, j];
                                nCenterValue2 += nValue * mask2[i, j];
                            }
                        }

                        int nSum = Math.Abs(nCenterValue1) + Math.Abs(nCenterValue2);
                        if (nSum > 255) nSum = 255;
                        if (nSum < 0) nSum = 0;
                        int nPosX = x + 1;
                        int nPosY = y + 1;
                        pResultBuffer[nPosY * nImageWidth + nPosX] = (byte) nSum;
                    }
                }

                // Get the buffer that combines the filter result with the original
                if (bCombineSource)
                {
                    pResultBuffer = TwoImageProcess.Combine(pBuffer, pResultBuffer, nWidth, nHeight);
                }

                return pResultBuffer;
            }

            public static YoonImage Laplacian(YoonImage pSourceImage, int nIntensity, bool bCombine = true)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                return new YoonImage(
                    Laplacian(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, nIntensity,
                        bCombine),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Laplacian(byte[] pBuffer, int nWidth, int nHeight, int nIntensity, bool bCombineSource)
            {
                if (nWidth < 1 || nHeight < 1)
                    throw new ArgumentException("[YOONIMAGE EXCEPTION] Buffer size is not normalized");
                int nCenterValue = 4 * nIntensity;
                byte[] pResultBuffer = new byte[nWidth * nHeight];
                ////  Laplacian Masking
                for (int j = 1; j < nHeight - 1; j++)
                {
                    for (int i = 1; i < nWidth - 1; i++)
                    {
                        int nValue = nCenterValue * pBuffer[j * nWidth + i] - pBuffer[(j - 1) * nWidth + i] -
                                     pBuffer[j * nWidth + i + 1] - pBuffer[(j + 1) * nWidth + i] -
                                     pBuffer[j * nWidth + i - 1];
                        if (nValue < 0) nValue = 0;
                        if (nValue > 255) nValue = 255;
                        pResultBuffer[j * nWidth + i] = (byte) nValue;
                    }
                }

                // Get the buffer that combines the filter result with the original
                if (bCombineSource)
                {
                    pResultBuffer = TwoImageProcess.Combine(pBuffer, pResultBuffer, nWidth, nHeight);
                }

                return pResultBuffer;
            }

            public static YoonImage RC1D(YoonImage pSourceImage, double dFrequency, bool bCombine = true)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                return new YoonImage(RC1D(pSourceImage.GetGrayBuffer(), pSourceImage.Width, dFrequency, bCombine),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] RC1D(byte[] pBuffer, int nSize, double dFrequency, bool bCombineSource)
            {
                int nWidth = nSize;
                int nHeight = 1;
                double[] pWidth = new double[nWidth];
                double[] pHeight = new double[nHeight];
                byte[] pResultBuffer = new byte[nWidth * nHeight];
                // Filtering Horizontal
                for (int j = 0; j < nHeight; j++)
                {
                    pHeight[j] = pBuffer[j * nWidth + 0];
                    pResultBuffer[j * nWidth + 0] = (byte) (0.5 * pHeight[j]);
                    for (int i = 1; i < nWidth; i++)
                    {
                        pHeight[j] = dFrequency * pHeight[j] + (1 - dFrequency) * pBuffer[j * nWidth + i];
                        double dValue = 0.5 * pHeight[j];
                        if (dValue < 0) dValue = 0;
                        if (dValue > 255) dValue = 255;
                        pResultBuffer[j * nWidth + i] = (byte) dValue;
                    }
                }

                for (int j = 0; j < nHeight; j++)
                {
                    pHeight[j] = pBuffer[j * nWidth + (nWidth - 1)];
                    double dValue = 0.5 * pHeight[j] + pResultBuffer[j * nWidth + (nWidth - 1)];
                    if (dValue < 0) dValue = 0;
                    if (dValue > 255) dValue = 255;
                    pResultBuffer[j * nWidth + (nWidth - 1)] = (byte) dValue;
                    for (int i = nWidth - 2; i >= 0; i--)
                    {
                        pHeight[j] = dFrequency * pHeight[j] + (1 - dFrequency) * pBuffer[j * nWidth + i];
                        dValue = pResultBuffer[j * nWidth + i] + 0.5 * pHeight[j];
                        if (dValue < 0) dValue = 0;
                        if (dValue > 255) dValue = 255;
                        pResultBuffer[j * nWidth + i] = (byte) dValue;
                    }
                }

                // Get the buffer that combines the filter result with the original
                if (bCombineSource)
                {
                    pResultBuffer = TwoImageProcess.Combine(pBuffer, pResultBuffer, nWidth, nHeight);
                }

                return pResultBuffer;
            }

            public static YoonImage RC2D(YoonImage pSourceImage, double dFrequency, bool bCombine = true)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                return new YoonImage(
                    RC2D(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, dFrequency, bCombine),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] RC2D(byte[] pBuffer, int nWidth, int nHeight, double dFrequency, bool bSumSource)
            {
                double[] pWidth = new double[nWidth];
                double[] pHeight = new double[nHeight];
                byte[] pResultBuffer = new byte[nWidth * nHeight];
                // Filtering Horizontal
                for (int j = 0; j < nHeight; j++)
                {
                    pHeight[j] = pBuffer[j * nWidth + 0];
                    pResultBuffer[j * nWidth + 0] = (byte) (0.5 * pHeight[j]);
                    for (int i = 1; i < nWidth; i++)
                    {
                        pHeight[j] = dFrequency * pHeight[j] + (1 - dFrequency) * pBuffer[j * nWidth + i];
                        double dValue = 0.5 * pHeight[j];
                        if (dValue < 0) dValue = 0;
                        if (dValue > 255) dValue = 255;
                        pResultBuffer[j * nWidth + i] = (byte) dValue;
                    }
                }

                for (int j = 0; j < nHeight; j++)
                {
                    pHeight[j] = pBuffer[j * nWidth + (nWidth - 1)];
                    double dValue = 0.5 * pHeight[j] + pResultBuffer[j * nWidth + (nWidth - 1)];
                    if (dValue < 0) dValue = 0;
                    if (dValue > 255) dValue = 255;
                    pResultBuffer[j * nWidth + (nWidth - 1)] = (byte) dValue;
                    for (int i = nWidth - 2; i >= 0; i--)
                    {
                        pHeight[j] = dFrequency * pHeight[j] + (1 - dFrequency) * pBuffer[j * nWidth + i];
                        dValue = pResultBuffer[j * nWidth + i] + 0.5 * pHeight[j];
                        if (dValue < 0) dValue = 0;
                        if (dValue > 255) dValue = 255;
                        pResultBuffer[j * nWidth + i] = (byte) dValue;
                    }
                }

                // Filtering Vertical
                for (int i = 0; i < nWidth; i++)
                {
                    pWidth[i] = pResultBuffer[0 * nWidth + i];
                    pResultBuffer[0 * nWidth + i] = (byte) (0.5 * pWidth[i]);
                    for (int j = 1; j < nHeight; j++)
                    {
                        pWidth[i] = dFrequency * pWidth[i] + (1 - dFrequency) * pResultBuffer[j * nWidth + i];
                        double dValue = 0.5 * pWidth[i];
                        if (dValue < 0) dValue = 0;
                        if (dValue > 255) dValue = 255;
                        pResultBuffer[j * nWidth + i] = (byte) dValue;
                    }
                }

                for (int i = 0; i < nWidth; i++)
                {
                    pWidth[i] = pResultBuffer[(nHeight - 1) * nWidth + i];
                    double dValue = 0.5 * pWidth[i] + pResultBuffer[(nHeight - 1) * nWidth + i];
                    if (dValue < 0) dValue = 0;
                    if (dValue > 255) dValue = 255;
                    pResultBuffer[(nHeight - 1) * nWidth + i] = (byte) dValue;
                    for (int j = nHeight - 2; j >= 0; j--)
                    {
                        pWidth[i] = dFrequency * pWidth[i] + (1 - dFrequency) * pResultBuffer[j * nWidth + i];
                        dValue = pResultBuffer[j * nWidth + i] + 0.5 * pWidth[i];
                        if (dValue < 0) dValue = 0;
                        if (dValue > 255) dValue = 255;
                        pResultBuffer[j * nWidth + i] = (byte) dValue;
                    }
                }

                // Get the buffer that combines the filter result with the original
                if (bSumSource)
                {
                    pResultBuffer = TwoImageProcess.Combine(pBuffer, pResultBuffer, nWidth, nHeight);
                }

                return pResultBuffer;
            }

            public static YoonImage Level2D(YoonImage pSourceImage, ref double dSum)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                return new YoonImage(
                    Level2D(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, ref dSum),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Level2D(byte[] pBuffer, int nWidth, int nHeight, ref double dSum)
            {
                double[,] pAverage = new double[2, 2];
                byte[] pResultBuffer = new byte[nWidth * nHeight];
                double dInverseWidth = 1 / (double) nWidth;
                double dInverseHeight = 1 / (double) nHeight;
                int nCenterX = nWidth / 2;
                int nCenterY = nHeight / 2;
                int nX1 = 0;
                int nY1 = 0;
                int nX2 = 0;
                int nY2 = 0;
                dSum = 0;
                // Adjust the filter
                for (int iNo = 0; iNo < 4; iNo++)
                {
                    int iRow = iNo / 2;
                    int iCol = iNo % 2;
                    int nCount = 0;
                    // Create the filter to different each others
                    switch (iNo)
                    {
                        case 0:
                            nX1 = 2;
                            nY1 = 2;
                            nX2 = nCenterX / 2 - 2;
                            nY2 = nCenterY / 2 - 2;
                            break;
                        case 1:
                            nX1 = nCenterX + nCenterX / 2;
                            nX2 = nWidth - 2;
                            nY1 = 2;
                            nY2 = nCenterY / 2 - 2;
                            break;
                        case 2:
                            nX1 = 2;
                            nY1 = nCenterY + nCenterY / 2;
                            nX2 = nCenterX / 2 - 2;
                            nY2 = nHeight - 2;
                            break;
                        case 3:
                            nX1 = nCenterX + nCenterX / 2;
                            nY1 = nCenterY + nCenterY / 2;
                            nX2 = nWidth - 2;
                            nY2 = nHeight - 2;
                            break;
                    }

                    // Get the average passing filters
                    for (int j = nY1; j < nY2; j += 2)
                    {
                        for (int i = nX1; i < nX2; i += 2)
                        {
                            pAverage[iRow, iCol] = pAverage[iRow, iCol] + pBuffer[j * nWidth + i];
                            nCount++;
                        }
                    }

                    if (nCount > 1) pAverage[iRow, iCol] = pAverage[iRow, iCol] / (float) nCount;
                    else pAverage[iRow, iCol] = 0;
                    dSum += pAverage[iRow, iCol];
                }

                dSum /= (float) 4.0;
                // Find the reciprocal multiplication
                double dDiffX = dInverseWidth * (pAverage[1, 0] + pAverage[1, 1] - pAverage[0, 0] - pAverage[0, 1]);
                double dDiffY = dInverseHeight * (pAverage[0, 1] + pAverage[1, 1] - pAverage[0, 0] - pAverage[1, 0]);
                // Get the result of filtering
                for (int j = 0; j < nHeight; j++)
                {
                    for (int i = 0; i < nWidth; i++)
                    {
                        double nValue = 100 * pBuffer[j * nWidth + i] /
                                        (dSum + dDiffX * (i - nCenterX) + dDiffY * (j - nCenterY));
                        if (nValue > 255) nValue = 255;
                        if (nValue < 0) nValue = 0;
                        pResultBuffer[j * nWidth + i] = (byte) nValue;
                    }
                }

                return pResultBuffer;
            }

            public static YoonImage DeMargin2D(YoonImage pSourceImage)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                return new YoonImage(DeMargin2D(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] DeMargin2D(byte[] pBuffer, int nWidth, int nHeight)
            {
                byte[] pResultBuffer = new byte[nWidth * nHeight];
                int nCenterX = nWidth / 2;
                int nCenterY = nHeight / 2;
                float[] pWidth = new float[nWidth];
                float[] pHeight = new float[nHeight];
                // Add the data for each row
                for (int j = 0; j < nHeight; j++)
                {
                    pHeight[j] = pBuffer[j * nWidth + 0];
                    for (int i = 1; i < nWidth; i++)
                    {
                        pHeight[j] = pHeight[j] + pBuffer[j * nWidth + i];
                    }
                }

                // Overwrite the filter by center value
                float dNorm = pHeight[nCenterY];
                for (int j = 0; j < nHeight; j++)
                {
                    if (pHeight[j] > 1)
                        pHeight[j] = dNorm / pHeight[j];
                    else
                        pHeight[j] = 1;
                }

                // Filtering vertical
                for (int j = 0; j < nHeight; j++)
                for (int i = 0; i < nWidth; i++)
                    pResultBuffer[j * nWidth + i] = (byte) (pHeight[j] * pBuffer[j * nWidth + i]);
                // Add the data for each row
                for (int i = 0; i < nWidth; i++)
                {
                    pWidth[i] = pResultBuffer[0 * nWidth + i];
                    for (int j = 0; j < nHeight; j++)
                    {
                        pWidth[i] = pWidth[i] + pResultBuffer[j * nWidth + i];
                    }
                }

                // Overwrite the filter by center value
                dNorm = pWidth[nCenterX];
                for (int i = 0; i < nWidth; i++)
                {
                    if (pWidth[i] > 0)
                        pWidth[i] = dNorm / pWidth[i];
                    else
                        pWidth[i] = 1;
                }

                // Filtering horizontal
                for (int j = 0; j < nHeight; j++)
                for (int i = 0; i < nWidth; i++)
                    pResultBuffer[j * nWidth + i] = (byte) (pWidth[i] * pResultBuffer[j * nWidth + i]);
                return pResultBuffer;
            }

            public static YoonImage Smooth1D(YoonImage pSourceImage, int nMargin = 1, int nStep = 3)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                return new YoonImage(
                    Smooth1D(pSourceImage.GetGrayBuffer(), pSourceImage.Width * pSourceImage.Height, nMargin, nStep),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Smooth1D(byte[] pBuffer, int nBufferSize, int nMargin, int nStep)
            {
                byte[] pResultBuffer = new byte[nBufferSize];
                if (nStep < 1)
                    nStep = 1;
                for (int i = nMargin; i < nBufferSize - nMargin; i++)
                {
                    int nSum = 0;
                    int nCount = 0;
                    // Data sampling
                    for (int ii = -nMargin; ii <= nMargin; ii += nStep)
                    {
                        int nX = i + ii;
                        if (nX >= nBufferSize)
                            continue;
                        nSum += pBuffer[nX];
                        nCount++;
                    }

                    if (nCount < 1)
                        nCount = 1;
                    // Set the average filter
                    pResultBuffer[i] = (byte) (nSum / nCount);
                }

                return pResultBuffer;
            }

            public static YoonImage Smooth2D(YoonImage pSourceImage, int nStep = 5)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image arguments is not 8bit format");
                return new YoonImage(
                    Smooth2D(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, nStep),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Smooth2D(byte[] pBuffer, int width, int nHeight, int nBlurStep)
            {
                if (width < 1 || nHeight < 1 || pBuffer == null)
                    throw new ArgumentException("[YOONIMAGE EXCEPTION] Buffer size is not normalized");
                byte[] pResultBuffer = new byte[width * nHeight];
                int nStepSize = 2 * nBlurStep / 10;
                if (nStepSize < 1)
                    nStepSize = 1;
                // Set the average filter
                for (int j = 0; j < nHeight; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        int nSum = 0;
                        int nCount = 0;
                        // Extract the average value
                        for (int jj = -nBlurStep; jj <= nBlurStep; jj += nStepSize)
                        {
                            int nY = j + jj;
                            for (int ii = -nBlurStep; ii <= nBlurStep; ii += nStepSize)
                            {
                                int nX = i + ii;
                                if (nX < 0 || nY < 0 || nX >= width || nY >= nHeight)
                                    continue;
                                nSum += pBuffer[nY * width + nX];
                                nCount++;
                            }
                        }

                        if (nCount < 1)
                            nCount = 1;
                        // Overwrite the average obtained by sampling
                        pResultBuffer[j * width + i] = (byte) (nSum / nCount);
                    }
                }

                return pResultBuffer;
            }
        }

        public static class Fill
        {
            public static YoonImage FillBound(YoonImage pSourceImage, int nValue)
            {
                return pSourceImage.Plane switch
                {
                    1 => new YoonImage(
                        FillBound(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, (byte) nValue),
                        pSourceImage.Width, pSourceImage.Height, 1),
                    4 => new YoonImage(
                        FillBound(pSourceImage.GetARGBBuffer(), pSourceImage.Width, pSourceImage.Height, nValue),
                        pSourceImage.Width, pSourceImage.Height, 4),
                    _ => throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct")
                };
            }

            public static int[] FillBound(int[] pBuffer, int nWidth, int nHeight, int nValue)
            {
                int[] pResultBuffer = new int[nWidth * nHeight];
                Array.Copy(pBuffer, pResultBuffer, nWidth * nHeight);
                for (int x = 0; x < nWidth; x++)
                {
                    pResultBuffer[0 * nWidth + x] = nValue;
                    pResultBuffer[(nHeight - 1) * nWidth + x] = nValue;
                }
                for (int y = 0; y < nHeight; y++)
                {
                    pResultBuffer[y * nWidth + 0] = nValue;
                    pResultBuffer[y * nWidth + (nWidth - 1)] = nValue;
                }
                return pResultBuffer;
            }

            public static byte[] FillBound(byte[] pBuffer, int nWidth, int nHeight, byte nValue)
            {
                byte[] pResultBuffer = new byte[nWidth * nHeight];
                Array.Copy(pBuffer, pResultBuffer, nWidth * nHeight);
                for (int x = 0; x < nWidth; x++)
                {
                    pResultBuffer[0 * nWidth + x] = nValue;
                    pResultBuffer[(nHeight - 1) * nWidth + x] = nValue;
                }
                for (int y = 0; y < nHeight; y++)
                {
                    pResultBuffer[y * nWidth + 0] = nValue;
                    pResultBuffer[y * nWidth + (nWidth - 1)] = nValue;
                }
                return pResultBuffer;
            }

            public static YoonImage FillFlood(YoonImage pSourceImage, YoonVector2N pVector, int nThreshold = 128, bool bFillWhite = true, int nValue = 0)
            {
                int iFillCount = 0;
                int iTotalCount = 0;
                if (pSourceImage.Plane == 1)
                {
                    byte[] pBuffer = pSourceImage.GetGrayBuffer();
                    FillFlood(ref pBuffer, ref iFillCount, pSourceImage.Width, pSourceImage.Height, pVector, (byte)nThreshold, bFillWhite, (byte)nValue, ref iTotalCount);
                    return new YoonImage(pBuffer, pSourceImage.Width, pSourceImage.Height, 1);
                }

                else if (pSourceImage.Plane == 4)
                {
                    int[] pBuffer = pSourceImage.GetARGBBuffer();
                    FillFlood(ref pBuffer, ref iFillCount, pSourceImage.Width, pSourceImage.Height, pVector, nThreshold, bFillWhite, nValue, ref iTotalCount);
                    return new YoonImage(pBuffer, pSourceImage.Width, pSourceImage.Height, 1);
                }
                else
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
            }

            public static bool FillFlood(ref int[] pBuffer, ref int nFillCount, int nWidth, int nHeight,
                YoonVector2N pVector, int nThreshold, bool bWhite, int nValue, ref int nTotalCount)
            {
                if (pVector.X < 0 || pVector.X >= nWidth) return true;
                if (pVector.Y < 0 || pVector.Y >= nHeight) return true;
                nTotalCount++; // Activation count
                // Stop filling the pixel when an activation count is higher then the constant count
                if (nTotalCount > MAX_FILL_NUM) return false;
                if (bWhite)
                {
                    if (pBuffer[pVector.Y * nWidth + pVector.X] < nThreshold) return true;
                    pBuffer[pVector.Y * nWidth + pVector.X] = nValue;
                    nFillCount++; // Filling count
                    // Find the flood area in clock directions
                    foreach (eYoonDir2D nDir in YoonDirFactory.GetClockDirections())
                    {
                        FillFlood(ref pBuffer, ref nFillCount, nWidth, nHeight,
                            (YoonVector2N) pVector.GetNextVector(nDir), nThreshold, bWhite, nValue,
                            ref nTotalCount);
                    }
                }
                else
                {
                    if (pBuffer[pVector.Y * nWidth + pVector.X] >= nThreshold) return true;
                    pBuffer[pVector.Y * nWidth + pVector.X] = nValue;
                    nFillCount++;
                    foreach (eYoonDir2D nDir in YoonDirFactory.GetClockDirections())
                    {
                        FillFlood(ref pBuffer, ref nFillCount, nWidth, nHeight,
                            (YoonVector2N) pVector.GetNextVector(nDir), nThreshold, bWhite, nValue,
                            ref nTotalCount);
                    }
                }

                return true;
            }

            public static bool FillFlood(ref byte[] pBuffer, ref int nFillCount, int nWidth, int nHeight,
                YoonVector2N pVector, byte nThreshold, bool bWhite, byte nValue, ref int nTotalCount)
            {
                if (pVector.X < 0 || pVector.X >= nWidth) return true;
                if (pVector.Y < 0 || pVector.Y >= nHeight) return true;
                nTotalCount++;
                if (nTotalCount > MAX_FILL_NUM) return false;
                if (bWhite)
                {
                    if (pBuffer[pVector.Y * nWidth + pVector.X] >= nThreshold)
                    {
                        pBuffer[pVector.Y * nWidth + pVector.X] = nValue;
                        nFillCount++;
                        foreach (eYoonDir2D nDir in YoonDirFactory.GetClockDirections())
                        {
                            FillFlood(ref pBuffer, ref nFillCount, nWidth, nHeight,
                                (YoonVector2N) pVector.GetNextVector(nDir), nThreshold, bWhite, nValue,
                                ref nTotalCount);
                        }
                    }
                }
                else
                {
                    if (pBuffer[pVector.Y * nWidth + pVector.X] < nThreshold)
                    {
                        pBuffer[pVector.Y * nWidth + pVector.X] = nValue;
                        nFillCount++;
                        foreach (eYoonDir2D nDir in YoonDirFactory.GetClockDirections())
                        {
                            FillFlood(ref pBuffer, ref nFillCount, nWidth, nHeight,
                                (YoonVector2N) pVector.GetNextVector(nDir), nThreshold, bWhite, nValue,
                                ref nTotalCount);
                        }
                    }
                }

                return true;
            }

            public static YoonImage FillInside1D(YoonImage pSourceImage, int nThreshold = 128, bool bFillWhite = true,
                int nSize = 5)
            {
                return pSourceImage.Plane switch
                {
                    1 => new YoonImage(
                        FillInside1D(pSourceImage.GetGrayBuffer(), pSourceImage.Width * pSourceImage.Height,
                            (byte) nThreshold, bFillWhite, nSize), pSourceImage.Width, pSourceImage.Height,
                        PixelFormat.Format32bppArgb),
                    4 => new YoonImage(
                        FillInside1D(pSourceImage.GetARGBBuffer(), pSourceImage.Width * pSourceImage.Height, nThreshold,
                            bFillWhite, nSize), pSourceImage.Width, pSourceImage.Height, PixelFormat.Format32bppArgb),
                    _ => throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct")
                };
            }

            public static int[] FillInside1D(int[] pBuffer, int nBufferSize, int nThreshold, bool bWhite, int nSize)
            {
                int[] pResultBuffer = new int[nBufferSize];
                Array.Copy(pBuffer, pResultBuffer, nBufferSize);
                // Fill the white pixel
                if (bWhite)
                {
                    int nStart = -1;
                    for (int i = 0; i < nBufferSize; i++)
                    {
                        if (pBuffer[i] < nThreshold)
                        {
                            if (nStart < 0)
                                nStart = i;
                        }
                        else
                        {
                            if (nStart > 0)
                            {
                                int nIntensity = pBuffer[i];
                                int nEnd = i;
                                int nDiff = nEnd - nStart;
                                if (nDiff <= nSize)
                                {
                                    for (int ii = nStart; ii <= nEnd; ii++)
                                    {
                                        pResultBuffer[ii] = nIntensity;
                                    }
                                }
                            }

                            nStart = -1;
                        }
                    }
                }
                // Fill the black pixel
                else
                {
                    int nStart = -1;
                    for (int i = 0; i < nBufferSize; i++)
                    {
                        if (pBuffer[i] >= nThreshold)
                        {
                            if (nStart < 0)
                                nStart = i;
                        }
                        else
                        {
                            if (nStart > 0)
                            {
                                int nEnd = i;
                                int nDiff = nEnd - nStart;
                                if (nDiff <= nSize)
                                {
                                    for (int ii = nStart; ii <= nEnd; ii++)
                                    {
                                        pResultBuffer[ii] = 0;
                                    }
                                }
                            }

                            nStart = -1;
                        }
                    }
                }

                return pResultBuffer;
            }

            public static byte[] FillInside1D(byte[] pBuffer, int nBufferSize, byte nThreshold, bool bWhite, int nSize)
            {
                byte[] pResultBuffer = new byte[nBufferSize];
                Array.Copy(pBuffer, pResultBuffer, nBufferSize);
                if (bWhite)
                {
                    int nStart = -1;
                    for (int i = 0; i < nBufferSize; i++)
                    {
                        if (pBuffer[i] < nThreshold)
                        {
                            if (nStart < 0)
                                nStart = i;
                        }
                        else
                        {
                            if (nStart > 0)
                            {
                                byte nIntensity = pBuffer[i];
                                int nEnd = i;
                                int nDiff = nEnd - nStart;
                                if (nDiff <= nSize)
                                {
                                    for (int ii = nStart; ii <= nEnd; ii++)
                                    {
                                        pResultBuffer[ii] = nIntensity;
                                    }
                                }
                            }

                            nStart = -1;
                        }
                    }
                }
                else
                {
                    int nStart = -1;
                    for (int i = 0; i < nBufferSize; i++)
                    {
                        if (pBuffer[i] >= nThreshold)
                        {
                            if (nStart < 0)
                                nStart = i;
                        }
                        else
                        {
                            if (nStart > 0)
                            {
                                byte nIntensity = pBuffer[i];
                                int nEnd = i;
                                int nDiff = nEnd - nStart;
                                if (nDiff <= nSize)
                                {
                                    for (int ii = nStart; ii <= nEnd; ii++)
                                    {
                                        pResultBuffer[ii] = nIntensity;
                                    }
                                }
                            }

                            nStart = -1;
                        }
                    }
                }

                return pResultBuffer;
            }

            public static YoonImage FillInside2D(YoonImage pSourceImage, YoonRect2N pScanArea, eYoonDir2DMode nDirMode,
                int nThreshold = 128, bool bFillWhite = true, int nSize = 5)
            {
                if (pSourceImage.Plane == 1)
                {
                    return nDirMode switch
                    {
                        eYoonDir2DMode.AxisX => new YoonImage(
                            FillHorizontal(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pScanArea,
                                (byte) nThreshold, bFillWhite, nSize), pSourceImage.Width, pSourceImage.Height,
                            PixelFormat.Format32bppArgb),
                        eYoonDir2DMode.AxisY => new YoonImage(
                            FillVertical(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pScanArea, (byte) nThreshold,
                                bFillWhite, nSize), pSourceImage.Width, pSourceImage.Height,
                            PixelFormat.Format32bppArgb),
                        _ => throw new ArgumentException("[YOONIMAGE EXCEPTION] Direction of filling is not correct")
                    };
                }

                if (pSourceImage.Plane == 4)
                {
                    return nDirMode switch
                    {
                        eYoonDir2DMode.AxisX => new YoonImage(
                            FillHorizontal(pSourceImage.GetARGBBuffer(), pSourceImage.Width, pScanArea, nThreshold,
                                bFillWhite, nSize), pSourceImage.Width, pSourceImage.Height,
                            PixelFormat.Format32bppArgb),
                        eYoonDir2DMode.AxisY => new YoonImage(
                            FillVertical(pSourceImage.GetARGBBuffer(), pSourceImage.Width, pScanArea, nThreshold,
                                bFillWhite, nSize), pSourceImage.Width, pSourceImage.Height,
                            PixelFormat.Format32bppArgb),
                        _ => throw new ArgumentException("[YOONIMAGE EXCEPTION] Direction of filling is not correct")
                    };
                }

                throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
            }

            public static byte[] FillHorizontal(byte[] pBuffer, int nImageWidth, YoonRect2N pScanArea, byte nThreshold,
                bool bWhite, int nSize)
            {
                if (pScanArea == null)
                    throw new NullReferenceException("[YOONIMAGE EXCEPTION] Scan area has null reference");
                byte[] pResultBuffer = new byte[pBuffer.Length];
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                // Fill the white pixel
                if (bWhite)
                {
                    for (int j = pScanArea.Top; j < pScanArea.Bottom; j++)
                    {
                        int nStartX = -1;
                        for (int i = pScanArea.Left; i < pScanArea.Right; i++)
                        {
                            // If the start is black
                            if (pBuffer[j * nImageWidth + i] < nThreshold)
                            {
                                if (nStartX < 0)
                                    nStartX = i;
                            }
                            else
                            {
                                if (nStartX > pScanArea.Left)
                                {
                                    byte nIntensity = pBuffer[j * nImageWidth + i];
                                    int nEndX = i;
                                    int nDiffX = nEndX - nStartX;
                                    if (nDiffX <= nSize)
                                    {
                                        for (int ii = nStartX; ii <= nEndX; ii++)
                                        {
                                            pResultBuffer[j * nImageWidth + ii] = nIntensity;
                                        }
                                    }
                                }

                                nStartX = -1;
                            }
                        }
                    }
                }
                // Fill the black pixel
                else
                {
                    for (int j = (int) pScanArea.Top; j < pScanArea.Bottom; j++)
                    {
                        int nStartX = -1;
                        for (int i = (int) pScanArea.Left; i < pScanArea.Right; i++)
                        {
                            // If the start is white
                            if (pBuffer[j * nImageWidth + i] >= nThreshold)
                            {
                                if (nStartX < 0)
                                    nStartX = i;
                            }
                            else
                            {
                                // Do not fill the black pixel from beginning
                                if (nStartX > pScanArea.Left)
                                {
                                    int nIntensity = pBuffer[j * nImageWidth + i];
                                    int nEndX = i;
                                    int nDiffX = nEndX - nStartX;
                                    if (nDiffX <= nSize)
                                    {
                                        for (int ii = nStartX; ii <= nEndX; ii++)
                                        {
                                            pResultBuffer[j * nImageWidth + ii] = 0;
                                        }
                                    }
                                }

                                nStartX = -1;
                            }
                        }
                    }
                }

                return pResultBuffer;
            }

            public static int[] FillHorizontal(int[] pBuffer, int nImageWidth, YoonRect2N pScanArea, int nThreshold,
                bool bWhite, int nSize)
            {
                if (pScanArea == null)
                    throw new NullReferenceException("[YOONIMAGE EXCEPTION] Scan area has null reference");
                int[] pResultBuffer = new int[pBuffer.Length];
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                if (bWhite)
                {
                    for (int j = pScanArea.Top; j < pScanArea.Bottom; j++)
                    {
                        int nStartX = -1;
                        for (int i = pScanArea.Left; i < pScanArea.Right; i++)
                        {
                            if (pBuffer[j * nImageWidth + i] < nThreshold)
                            {
                                if (nStartX < 0)
                                    nStartX = i;
                            }
                            else
                            {
                                if (nStartX > pScanArea.Left)
                                {
                                    int nIntensity = pBuffer[j * nImageWidth + i];
                                    int nEndX = i;
                                    int nDiffX = nEndX - nStartX;
                                    if (nDiffX <= nSize)
                                    {
                                        for (int ii = nStartX; ii <= nEndX; ii++)
                                        {
                                            pResultBuffer[j * nImageWidth + ii] = nIntensity;
                                        }
                                    }
                                }

                                nStartX = -1;
                            }
                        }
                    }
                }
                else
                {
                    for (int j = (int) pScanArea.Top; j < pScanArea.Bottom; j++)
                    {
                        int nStartX = -1;
                        for (int i = (int) pScanArea.Left; i < pScanArea.Right; i++)
                        {
                            if (pBuffer[j * nImageWidth + i] >= nThreshold)
                            {
                                if (nStartX < 0)
                                    nStartX = i;
                            }
                            else
                            {
                                if (nStartX > pScanArea.Left)
                                {
                                    int nIntensity = pBuffer[j * nImageWidth + i];
                                    int nEndX = i;
                                    int nDiffX = nEndX - nStartX;
                                    if (nDiffX <= nSize)
                                    {
                                        for (int ii = nStartX; ii <= nEndX; ii++)
                                        {
                                            pResultBuffer[j * nImageWidth + ii] = 0;
                                        }
                                    }
                                }

                                nStartX = -1;
                            }
                        }
                    }
                }

                return pResultBuffer;
            }

            public static byte[] FillVertical(byte[] pBuffer, int nImageWidth, YoonRect2N pScanArea, byte nThreshold, bool bWhite, int nSize)
            {
                if (pScanArea == null)
                    throw new NullReferenceException("[YOONIMAGE EXCEPTION] Scan area has null reference");
                byte[] pResultBuffer = new byte[pBuffer.Length];
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                if (bWhite)
                {
                    for (int i = pScanArea.Left; i < pScanArea.Right; i++)
                    {
                        int nStartY = -1;
                        for (int j = pScanArea.Top; j < pScanArea.Bottom; j++)
                        {
                            if (pBuffer[j * nImageWidth + i] < nThreshold)
                            {
                                if (nStartY < 0)
                                    nStartY = j;
                            }
                            else
                            {
                                if (nStartY > pScanArea.Top)
                                {
                                    byte nIntensity = pBuffer[j * nImageWidth + i];
                                    int nEndY = j;
                                    int nDiffY = nEndY - nStartY;
                                    if (nDiffY <= nSize)
                                    {
                                        for (int jj = nStartY; jj <= nEndY; jj++)
                                        {
                                            pResultBuffer[jj * nImageWidth + i] = nIntensity;
                                        }
                                    }
                                }
                                nStartY = -1;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = pScanArea.Left; i < pScanArea.Right; i++)
                    {
                        int nStartY = -1;
                        for (int j = pScanArea.Top; j < pScanArea.Bottom; j++)
                        {
                            if (pBuffer[j * nImageWidth + i] >= nThreshold)
                            {
                                if (nStartY < 0)
                                    nStartY = j;
                            }
                            else
                            {
                                if (nStartY > pScanArea.Top)
                                {
                                    byte nIntensity = pBuffer[j * nImageWidth + i];
                                    int nEndY = j;
                                    int nDiffY = nEndY - nStartY;
                                    if (nDiffY <= nSize)
                                    {
                                        for (int jj = nStartY; jj <= nEndY; jj++)
                                        {
                                            pResultBuffer[jj * nImageWidth + i] = nIntensity;
                                        }
                                    }
                                }
                                nStartY = -1;
                            }
                        }
                    }
                }
                return pResultBuffer;
            }

            public static int[] FillVertical(int[] pBuffer, int nImageWidth, YoonRect2N pScanArea, int nThreshold,
                bool bWhite, int nSize)
            {
                if (pScanArea == null)
                    throw new NullReferenceException("[YOONIMAGE EXCEPTION] Scan area has null reference");
                int[] pResultBuffer = new int[pBuffer.Length];
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                if (bWhite)
                {
                    for (int i = pScanArea.Left; i < pScanArea.Right; i++)
                    {
                        int nStartY = -1;
                        for (int j = pScanArea.Top; j < pScanArea.Bottom; j++)
                        {
                            if (pBuffer[j * nImageWidth + i] < nThreshold)
                            {
                                if (nStartY < 0)
                                    nStartY = j;
                            }
                            else
                            {
                                if (nStartY > pScanArea.Top)
                                {
                                    int nIntensity = pBuffer[j * nImageWidth + i];
                                    int nEndY = j;
                                    int nDiffY = nEndY - nStartY;
                                    if (nDiffY <= nSize)
                                    {
                                        for (int jj = nStartY; jj <= nEndY; jj++)
                                        {
                                            pResultBuffer[jj * nImageWidth + i] = nIntensity;
                                        }
                                    }
                                }

                                nStartY = -1;
                            }
                        }
                    }
                }
                ////  검은색으로 채울 경우.
                else
                {
                    for (int i = pScanArea.Left; i < pScanArea.Right; i++)
                    {
                        int nStartY = -1;
                        for (int j = pScanArea.Top; j < pScanArea.Bottom; j++)
                        {
                            if (pBuffer[j * nImageWidth + i] >= nThreshold)
                            {
                                if (nStartY < 0)
                                    nStartY = j;
                            }
                            else
                            {
                                if (nStartY > pScanArea.Top)
                                {
                                    int nIntensity = pBuffer[j * nImageWidth + i];
                                    int nEndY = j;
                                    int nDiffY = nEndY - nStartY;
                                    if (nDiffY <= nSize)
                                    {
                                        for (int jj = nStartY; jj <= nEndY; jj++)
                                        {
                                            pResultBuffer[jj * nImageWidth + i] = nIntensity;
                                        }
                                    }
                                }

                                nStartY = -1;
                            }
                        }
                    }
                }

                return pResultBuffer;
            }
        }

        public static class Blob
        {
            public static IYoonObject FindMaxBlob(YoonImage pSourceImage, YoonRect2N pScanArea, byte nThreshold = 128, bool bWhite = false)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return FindMaxBlob(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pScanArea, nThreshold, bWhite);
            }

            public static IYoonObject FindMaxBlob(byte[] pBuffer, int nImageWidth, YoonRect2N pScanArea,
                byte nThreshold, bool bWhite, bool bSquareOnly = false, bool bNormalOnly = false)
            {
                YoonImage pMaxImage = new YoonImage(1, 1, 1);
                int nMaxLength = 0;
                int nMaxLabel = 0;
                int nWidth, nHeight, nLength;
                double dMaxScore = 0.0;
                ObjectList<YoonRect2N> pListObjectInfo =
                    FindBlobs(pBuffer, nImageWidth, pScanArea, nThreshold, bWhite);
                YoonRect2N pMaxArea = new YoonRect2N(0, 0, 0, 0);
                for (int iObject = 0; iObject < pListObjectInfo.Count; iObject++)
                {
                    nWidth = pListObjectInfo[iObject].Object.Width;
                    nHeight = pListObjectInfo[iObject].Object.Height;
                    nLength = pListObjectInfo[iObject].PixelCount;
                    // Only count the square rect blob
                    if (bSquareOnly)
                    {
                        if (nWidth > 3 * nHeight || nHeight > 3 * nWidth)
                        {
                            pListObjectInfo.RemoveAt(iObject);
                            continue;
                        }
                    }

                    // Only count the showing whole blob in image
                    if (bNormalOnly)
                    {
                        int nDiffLeft = pListObjectInfo[iObject].Object.Left;
                        int nDiffTop = pListObjectInfo[iObject].Object.Top;
                        if (nDiffLeft <= 1 || nDiffTop <= 1)
                        {
                            pListObjectInfo.RemoveAt(iObject);
                            continue;
                        }
                    }

                    if (nLength > nMaxLength)
                    {
                        nMaxLength = nLength;
                        nMaxLabel = pListObjectInfo[iObject].Label;
                        dMaxScore = pListObjectInfo[iObject].Score;
                        pMaxImage = pListObjectInfo[iObject].ObjectImage;
                        pMaxArea.CenterPos.X = pScanArea.Left + pListObjectInfo[iObject].Object.Left +
                                               pListObjectInfo[iObject].Object.Width / 2;
                        pMaxArea.CenterPos.Y = pScanArea.Top + pListObjectInfo[iObject].Object.Top +
                                               pListObjectInfo[iObject].Object.Height / 2;
                        pMaxArea.Width = pListObjectInfo[iObject].Object.Right - pListObjectInfo[iObject].Object.Left;
                        pMaxArea.Height = pListObjectInfo[iObject].Object.Bottom - pListObjectInfo[iObject].Object.Top;
                    }
                }

                return new YoonObject<YoonRect2N>(nMaxLabel, pMaxArea, (YoonImage) pMaxImage.Clone(), dMaxScore,
                    nMaxLength);
            }

            public static IYoonObject FindMaxBlob(YoonImage pSourceImage, byte nThreshold = 128, bool bWhite = false)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return FindMaxBlob(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, nThreshold,
                    bWhite);
            }

            public static IYoonObject FindMaxBlob(byte[] pBuffer, int nImageWidth, int imageHeight, byte nThreshold,
                bool isWhite, bool bSquareOnly = false, bool bNormalOnly = false)
            {
                // Save found blobs as list
                ObjectList<YoonRect2N> pListObjectInfo =
                    FindBlobs(pBuffer, nImageWidth, imageHeight, nThreshold, isWhite);
                if (pListObjectInfo.Count == 0)
                    throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Find Object List is empty");
                int nOrder = 0;
                int nMaxLength = 0;
                YoonRect2N pRect = new YoonRect2N(0, 0, 0, 0);
                for (int iObject = 0; iObject < pListObjectInfo.Count; iObject++)
                {

                    int nWidth = pListObjectInfo[iObject].Object.Width;
                    int nHeight = pListObjectInfo[iObject].Object.Height;
                    int nLength = pListObjectInfo[iObject].PixelCount;
                    if (bSquareOnly)
                    {
                        if (nWidth > 3 * nHeight || nHeight > 3 * nWidth)
                        {
                            pListObjectInfo.RemoveAt(iObject);
                            continue;
                        }
                    }

                    if (bNormalOnly)
                    {
                        int diffLeft = pListObjectInfo[iObject].Object.Left;
                        int diffTop = pListObjectInfo[iObject].Object.Top;
                        if (diffLeft <= 1 || diffTop <= 1)
                        {
                            pListObjectInfo.RemoveAt(iObject);
                            continue;
                        }
                    }

                    if (nLength > nMaxLength)
                    {
                        nMaxLength = nLength;
                        nOrder = iObject;
                    }
                }

                return pListObjectInfo[nOrder];
            }

            public static ObjectList<YoonRect2N> FindBlobs(YoonImage pSourceImage, YoonRect2N scanArea,
                byte nThreshold = 128, bool bWhite = false)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return FindBlobs(pSourceImage.GetGrayBuffer(), pSourceImage.Width, scanArea, nThreshold, bWhite);
            }

            public static ObjectList<YoonRect2N> FindBlobs(byte[] pBuffer, int imageWidth, YoonRect2N scanArea,
                byte nThreshold, bool bWhite)
            {
                int nLabelNo = 0;
                int nWidth = scanArea.Width;
                int nHeight = scanArea.Height;
                if (nThreshold < 10) nThreshold = 10;
                byte[] pTempBuffer = new byte[nWidth * nHeight];
                YoonVector2N pStartVector = new YoonVector2N(0, 0);
                ObjectList<YoonRect2N> pListResult = new ObjectList<YoonRect2N>();
                // Copy the source buffer into the temporary buffer
                for (int j = 0; j < nHeight; j++)
                {
                    int y = scanArea.Top + j;
                    for (int i = 0; i < nWidth; i++)
                    {
                        int x = scanArea.Left + i;
                        pTempBuffer[j * nWidth + i] = pBuffer[y * imageWidth + x];
                    }
                }

                // Erase the boundary of temporary buffer
                pTempBuffer = bWhite
                    ? Fill.FillBound(pTempBuffer, nWidth, nHeight, (byte) 0)
                    : Fill.FillBound(pTempBuffer, nWidth, nHeight, (byte) 255);
                // Repeat the function until all blob are found
                while (true)
                {
                    // Get start position
                    YoonVector2N pResultVector = Scanner.Scan2D(pTempBuffer, nWidth, nHeight, eYoonDir2D.Right,
                        pStartVector, nThreshold, bWhite) as YoonVector2N;
                    Debug.Assert(pResultVector != null, nameof(pResultVector) + " != null");
                    // If the scanner is stop on the boundary
                    if (pResultVector.X == -1 && pResultVector.Y == -1)
                        break;
                    // Bind the blob
                    YoonObject<YoonRect2N> pObject =
                        ProcessBind(pTempBuffer, nWidth, nHeight, eYoonDir2D.Right, pResultVector, nThreshold,
                            bWhite) as YoonObject<YoonRect2N>;
                    Debug.Assert(pObject != null, nameof(pObject) + " != null");
                    //////  DetectEdge에서 Error가 발생했을 경우.
                    if (pObject.Object.Left == 0 || pObject.Object.Top == 0 || pObject.Object.Right == 0 ||
                        pObject.Object.Bottom == 0)
                        break;
                    // Erase the rect when the blob is constructed only one pixel
                    if (pObject.Object.Left == -1 || pObject.Object.Top == -1 || pObject.Object.Right == -1 ||
                        pObject.Object.Bottom == -1)
                        continue;
                    // Save the blob in the list
                    if (pListResult.Count < MAX_OBJECT)
                    {
                        pObject.Label = nLabelNo++;
                        pListResult.Add(pObject);
                    }

                    // Control the start vector to the end of these sequence
                    pStartVector = (YoonVector2N) pObject.Object.BottomRight.Clone() + new YoonVector2N(1, 1);
                }

                return pListResult;
            }

            public static ObjectList<YoonRect2N> FindBlobs(YoonImage pSourceImage, byte nThreshold = 128, bool bWhite = false)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return FindBlobs(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, nThreshold, bWhite);
            }

            public static ObjectList<YoonRect2N> FindBlobs(byte[] pBuffer, int nWidth, int nHeight, byte nThreshold,
                bool bWhite)
            {
                ObjectList<YoonRect2N> pListResult = new ObjectList<YoonRect2N>();
                int nLabelNo = 0;
                if (nThreshold < 10) nThreshold = 10;
                byte[] pTempBuffer = new byte[nWidth * nHeight];
                YoonVector2N pStartVector = new YoonVector2N(0, 0);
                pBuffer.CopyTo(pTempBuffer, 0);
                pTempBuffer = bWhite
                    ? Fill.FillBound(pTempBuffer, nWidth, nHeight, (byte) 0)
                    : Fill.FillBound(pTempBuffer, nWidth, nHeight, (byte) 255);
                while (true)
                {
                    YoonVector2N pResultVector = Scanner.Scan2D(pTempBuffer, nWidth, nHeight, eYoonDir2D.Right,
                        pStartVector, nThreshold, bWhite) as YoonVector2N;
                    Debug.Assert(pResultVector != null, nameof(pResultVector) + " != null");
                    if (pResultVector.X == -1 && pResultVector.Y == -1)
                        break;
                    YoonObject<YoonRect2N> pObject =
                        ProcessBind(pTempBuffer, nWidth, nHeight, eYoonDir2D.Right, pResultVector, nThreshold,
                            bWhite) as YoonObject<YoonRect2N>;
                    Debug.Assert(pObject != null, nameof(pObject) + " != null");
                    if (pObject.Object.Left == 0 || pObject.Object.Top == 0 || pObject.Object.Right == 0 ||
                        pObject.Object.Bottom == 0)
                        break;
                    if (pObject.Object.Left == -1 || pObject.Object.Top == -1 || pObject.Object.Right == -1 ||
                        pObject.Object.Bottom == -1)
                        continue;
                    if (pListResult.Count < MAX_OBJECT)
                    {
                        pObject.Label = nLabelNo++;
                        pListResult.Add(pObject);
                    }

                    pStartVector = (YoonVector2N) pObject.Object.BottomRight.Clone() + new YoonVector2N(1, 1);
                }

                return pListResult;
            }

            private enum eYoonStepBinding
            {
                Init,
                Check,
                Go,
                Ignore,
                Stack,
                Rotate,
                Error,
                Finish,
            }

            private static IYoonObject ProcessBind(byte[] pBuffer, int nWidth, int nHeight, eYoonDir2D nDir, YoonVector2N pStartVector, byte nThreshold, bool bWhite)
            {
                int nPixelCount = 0;
                int nBlackCount = 0;
                bool bRun = true;
                YoonRect2N pResultRect = new YoonRect2N(pStartVector.X, pStartVector.Y, 0, 0);
                eYoonDir2D nDirSearch = nDir;
                eYoonDir2D nDirDefault = nDir;
                eYoonDir2DMode nDirMode = eYoonDir2DMode.Clock4;
                eYoonDir2DMode nRotateMode = eYoonDir2DMode.AxisX;
                YoonVector2N pCurrentVector = new YoonVector2N(pStartVector);
                eYoonStepBinding nJobStep = eYoonStepBinding.Init;
                eYoonStepBinding nJobStepBK = nJobStep;
                while (bRun)
                {
                    switch(nJobStep)
                    {
                        case eYoonStepBinding.Init:
                            switch (nDir)
                            {
                                case eYoonDir2D.Top:
                                    nRotateMode = eYoonDir2DMode.AxisY;
                                    nDirMode = eYoonDir2DMode.Clock4;
                                    nJobStep = eYoonStepBinding.Go;
                                    break;
                                case eYoonDir2D.Right:
                                    nRotateMode = eYoonDir2DMode.AxisX;
                                    nDirMode = eYoonDir2DMode.Clock4;
                                    nJobStep = eYoonStepBinding.Go;
                                    break;
                                case eYoonDir2D.Bottom:
                                    nRotateMode = eYoonDir2DMode.AxisY;
                                    nDirMode = eYoonDir2DMode.Clock4;
                                    nJobStep = eYoonStepBinding.Go;
                                    break;
                                case eYoonDir2D.Left:
                                    nRotateMode = eYoonDir2DMode.AxisX;
                                    nDirMode = eYoonDir2DMode.AntiClock4;
                                    nJobStep = eYoonStepBinding.Go;
                                    break;
                                default:
                                    nJobStep = eYoonStepBinding.Error;
                                    break;
                            }
                            break;
                        case eYoonStepBinding.Check:
                            byte value = pBuffer[pCurrentVector.Y * nWidth + pCurrentVector.X];
                            if (bWhite && value >= nThreshold)
                                nJobStep = eYoonStepBinding.Stack;
                            else if (!bWhite && value <= nThreshold)
                                nJobStep = eYoonStepBinding.Stack;
                            else
                            {
                                if (nJobStepBK == eYoonStepBinding.Stack)
                                    nJobStep = eYoonStepBinding.Rotate;
                                else if (nJobStepBK == eYoonStepBinding.Ignore)
                                    nJobStep = eYoonStepBinding.Ignore;
                                else
                                    nJobStep = eYoonStepBinding.Error;
                            }
                            break;
                        case eYoonStepBinding.Go:
                            pCurrentVector.Move(nDirSearch);
                            nJobStep = eYoonStepBinding.Check;
                            break;
                        case eYoonStepBinding.Ignore:
                            nJobStepBK = nJobStep;
                            if (nBlackCount++ >= pResultRect.Width)
                                nJobStep = eYoonStepBinding.Finish;
                            else
                                nJobStep = eYoonStepBinding.Go;
                            break;
                        case eYoonStepBinding.Stack:
                            nJobStepBK = nJobStep;
                            nBlackCount = 0;
                            nPixelCount++;
                            if (pCurrentVector.X < pResultRect.Left)
                                pResultRect.CenterPos.X = pCurrentVector.X + pResultRect.Width / 2;
                            if (pCurrentVector.X > pResultRect.Right)
                                pResultRect.Width = pCurrentVector.X - pResultRect.Left;
                            if (pCurrentVector.Y < pResultRect.Top)
                                pResultRect.CenterPos.Y = pCurrentVector.Y + pResultRect.Height / 2;
                            if (pCurrentVector.Y > pResultRect.Bottom)
                                pResultRect.Height = pCurrentVector.Y - pResultRect.Top;
                            if (pCurrentVector.X == pStartVector.X && pCurrentVector.Y == pStartVector.Y)
                                nJobStep = eYoonStepBinding.Finish;
                            else
                                nJobStep = eYoonStepBinding.Go;
                            break;
                        case eYoonStepBinding.Rotate:
                            nDirSearch = nDirSearch.Go(nDirMode);
                            if (nDirSearch == nDirDefault.Go(nRotateMode))
                            {
                                nDirDefault = nDirSearch;
                                nJobStep = eYoonStepBinding.Ignore;
                            }
                            else
                                nJobStep = eYoonStepBinding.Go;
                            break;
                        case eYoonStepBinding.Error:
                            pResultRect = new YoonRect2N(-1, -1, 0, 0);
                            bRun = false;
                            break;
                        case eYoonStepBinding.Finish:
                            bRun = false;
                            break;
                    }
                }
                YoonImage pResultImage;
                if (pResultRect.CenterPos.X == -1 && pResultRect.CenterPos.Y == -1)
                    pResultImage = new YoonImage(1, 1, 1);
                else
                {
                    byte[] pBufferCrop = new byte[pResultRect.Width * pResultRect.Height];
                    for (int iY = 0; iY < pResultRect.Height; iY++)
                    {
                        int iYSource = iY + pResultRect.Top;
                        for (int iX = 0; iX < pResultRect.Width; iX++)
                        {
                            int iXSource = iX + pResultRect.Left;
                            pBufferCrop[iY * pResultRect.Width + iX] = pBuffer[iYSource * nWidth + iXSource];
                        }
                    }
                    pResultImage = new YoonImage(pBufferCrop, pResultRect.Width, pResultRect.Height, PixelFormat.Format8bppIndexed);
                }
                return new YoonObject<YoonRect2N>(0, pResultRect, pResultImage, nPixelCount);
            }
        }

        public static class Binary
        {
            public static YoonImage Binarize(YoonImage pSourceImage, YoonRect2N pScanArea, byte nThreshold)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(Binarize(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pScanArea, nThreshold),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Binarize(byte[] pBuffer, int nBufferWidth, YoonRect2N pScanArea, byte nThreshold)
            {
                byte[] pResultBuffer = new byte[pBuffer.Length];
                for (int j = pScanArea.Top; j < pScanArea.Bottom; j++)
                {
                    for (int i = pScanArea.Left; i < pScanArea.Right; i++)
                    {
                        if (pBuffer[j * nBufferWidth + i] < nThreshold)
                            pResultBuffer[j * nBufferWidth + i] = 0;
                        else
                            pResultBuffer[j * nBufferWidth + i] = 255;
                    }
                }

                return pResultBuffer;
            }

            public static YoonImage Binarize(YoonImage pSourceImage, byte nThreshold)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(
                    Binarize(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, nThreshold),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Binarize(byte[] pBuffer, int nBufferWidth, int nBufferHeight, byte nThreshold)
            {
                int nSum = 0;
                int nCount = 0;
                byte nTempThreshold = nThreshold;
                byte[] pResultBuffer = new byte[nBufferWidth * nBufferHeight];
                // Adjust threshold
                if (nThreshold < 1)
                {
                    for (int j = 0; j < nBufferHeight; j++)
                    {
                        for (int i = 0; i < nBufferWidth; i++)
                        {
                            nSum += pBuffer[j * nBufferWidth + i];
                            nCount++;
                        }
                    }

                    if (nCount < 1)
                        nTempThreshold = 255;
                    else
                        nTempThreshold = (byte) (nSum / nCount + 20);
                }

                for (int j = 0; j < nBufferHeight; j++)
                {
                    for (int i = 0; i < nBufferWidth; i++)
                    {
                        if (pBuffer[j * nBufferWidth + i] < nTempThreshold)
                            pResultBuffer[j * nBufferWidth + i] = 0;
                        else
                            pResultBuffer[j * nBufferWidth + i] = 255;
                    }
                }

                return pResultBuffer;
            }
        }

        public static class Morphology
        {
            public static YoonImage Erosion(YoonImage pSourceImage)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(Erosion(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static YoonImage Erosion(YoonImage pSourceImage, YoonRect2N scanArea)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(Erosion(pSourceImage.GetGrayBuffer(), pSourceImage.Width, scanArea),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Erosion(byte[] pBuffer, int nBufferWidth, int nBufferHeight)
            {
                byte[] pResultBuffer = new byte[nBufferWidth * nBufferHeight];
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                // Masking for erosion
                for (int y = 0; y < nBufferHeight - 2; y++)
                {
                    for (int x = 0; x < nBufferWidth - 2; x++)
                    {
                        int nPosX, nPosY;
                        byte nMinValue = 255;
                        // Find the minimum gray level of peripheral points
                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                nPosX = x + i;
                                nPosY = y + j;
                                byte nValue = pBuffer[nPosY * nBufferWidth + nPosX];
                                if (nValue < nMinValue)
                                    nMinValue = nValue;
                            }
                        }

                        nPosX = x + 1;
                        nPosY = y + 1;
                        pResultBuffer[nPosY * nBufferWidth + nPosX] = nMinValue;
                    }
                }

                return pResultBuffer;
            }

            public static byte[] Erosion(byte[] pBuffer, int nBufferWidth, YoonRect2N scanArea)
            {
                int nScanWidth = scanArea.Width;
                int nScanHeight = scanArea.Height;
                byte[] pResultBuffer = new byte[pBuffer.Length];
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                byte[] pTempBuffer = new byte[nScanWidth * nScanHeight];
                for (int j = 0; j < nScanHeight; j++)
                for (int i = 0; i < nScanWidth; i++)
                    pTempBuffer[j * nScanWidth + i] = pBuffer[(scanArea.Top + j) * nBufferWidth + scanArea.Left + i];
                for (int y = 0; y < nScanHeight - 2; y++)
                {
                    for (int x = 0; x < nScanWidth - 2; x++)
                    {
                        int nPosX, nPosY;
                        byte nMinValue = 255;
                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                nPosX = x + i;
                                nPosY = y + j;
                                byte nValue = pTempBuffer[nPosY * nBufferWidth + nPosX];
                                if (nValue < nMinValue)
                                    nMinValue = nValue;
                            }
                        }

                        nPosX = scanArea.Left + x + 1;
                        nPosY = scanArea.Top + y + 1;
                        pResultBuffer[nPosY * nBufferWidth + nPosX] = nMinValue;
                    }
                }

                return pResultBuffer;
            }

            public static YoonImage ErosionAsBinary(YoonImage pSourceImage)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(
                    ErosionAsBinary(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static YoonImage ErosionAsBinary(YoonImage pSourceImage, int nMophSize)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(
                    ErosionAsBinary(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, nMophSize),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static YoonImage ErosionAsBinary(YoonImage pSourceImage, YoonRect2N scanArea)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(ErosionAsBinary(pSourceImage.GetGrayBuffer(), pSourceImage.Width, scanArea),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] ErosionAsBinary(byte[] pBuffer, int nBufferWidth, int nBufferHeight)
            {
                byte[] pResultBuffer = new byte[nBufferWidth * nBufferHeight];
                byte[,] pMask = new byte[3, 3] {{255, 255, 255}, {255, 255, 255}, {255, 255, 255}};
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                for (int y = 0; y < nBufferHeight - 2; y++)
                {
                    for (int x = 0; x < nBufferWidth - 2; x++)
                    {
                        int nPosX, nPosY;
                        int nSum = 0;
                        // Fill the white when whole peripheral pixels is white
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                nPosX = x + i;
                                nPosY = y + j;
                                if (pBuffer[nPosY * nBufferWidth + nPosX] == pMask[i, j])
                                    nSum++;
                            }
                        }

                        nPosX = x + 1;
                        nPosY = y + 1;
                        if (nSum == 9)
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 255;
                        else
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 0;
                    }
                }

                return pResultBuffer;
            }

            public static byte[] ErosionAsBinary(byte[] pBuffer, int nBufferWidth, int nBufferHeight, int size)
            {
                byte[] pResultBuffer = new byte[nBufferWidth * nBufferHeight];
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                for (int y = 0; y < nBufferHeight - size; y++)
                {
                    for (int x = 0; x < nBufferWidth - size; x++)
                    {
                        int nPosX, nPosY;
                        bool bBlack = false;
                        // Compare with virtual mask
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                // Check the peripheral pixels
                                nPosX = x + i;
                                nPosY = y + j;
                                if (pBuffer[(y + j) * nBufferWidth + (x + i)] == 0)
                                {
                                    bBlack = true;
                                    break;
                                }
                            }
                        }

                        nPosX = x + size / 2;
                        nPosY = y + size / 2;
                        if (bBlack)
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 0;
                        else
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 255;
                    }
                }

                return pResultBuffer;
            }

            public static byte[] ErosionAsBinary(byte[] pBuffer, int nBufferWidth, YoonRect2N pScanArea)
            {
                int nScanWidth = pScanArea.Width;
                int nScanHeight = pScanArea.Height;
                byte[] pResultBuffer = new byte[pBuffer.Length];
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                byte[] pTempBuffer = new byte[nScanWidth * nScanHeight];
                byte[,] pMask = new byte[3, 3] {{255, 255, 255}, {255, 255, 255}, {255, 255, 255}};
                for (int j = 0; j < nScanHeight; j++)
                for (int i = 0; i < nScanWidth; i++)
                    pTempBuffer[j * nScanWidth + i] = pBuffer[(pScanArea.Top + j) * nBufferWidth + pScanArea.Left + i];
                for (int y = 0; y < nScanHeight - 2; y++)
                {
                    for (int x = 0; x < nScanWidth - 2; x++)
                    {
                        int nSum = 0;
                        int nPosX, nPosY;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                nPosX = x + i;
                                nPosY = y + j;
                                if (pTempBuffer[nPosY * nScanWidth + nPosX] == pMask[i, j])
                                    nSum++;
                            }
                        }

                        nPosX = pScanArea.Left + x + 1;
                        nPosY = pScanArea.Top + y + 1;
                        if (nSum == 9)
                        {
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 255;
                        }
                        else
                        {
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 0;
                        }
                    }
                }

                return pResultBuffer;
            }

            public static YoonImage Dilation(YoonImage pSourceImage)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(Dilation(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static YoonImage Dilation(YoonImage pSourceImage, YoonRect2N pScanArea)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(Dilation(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pScanArea),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Dilation(byte[] pBuffer, int nBufferWidth, int nBufferHeight)
            {
                byte[] pResultBuffer = new byte[nBufferWidth * nBufferHeight];
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                // Masking for dilation
                for (int y = 0; y < nBufferHeight - 2; y++)
                {
                    for (int x = 0; x < nBufferWidth - 2; x++)
                    {
                        int nPosX, nPosY;
                        int nMaxValue = 0;
                        // Find the max gray level within peripheral points
                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                nPosX = x + i;
                                nPosY = y + j;
                                int nValue = pBuffer[nPosY * nBufferWidth + nPosX];
                                if (nValue > nMaxValue)
                                    nMaxValue = nValue;
                            }
                        }

                        nPosX = x + 1;
                        nPosY = y + 1;
                        pBuffer[nPosY * nBufferWidth + nPosX] = (byte) nMaxValue;
                    }
                }

                return pResultBuffer;
            }

            public static byte[] Dilation(byte[] pBuffer, int nBufferWidth, YoonRect2N pScanArea)
            {
                int nScanWidth = pScanArea.Width;
                int nScanHeight = pScanArea.Height;
                byte[] pResultBuffer = new byte[pBuffer.Length];
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                byte[] pTempBuffer = new byte[nScanWidth * nScanHeight];
                for (int j = 0; j < nScanHeight; j++)
                for (int i = 0; i < nScanWidth; i++)
                    pTempBuffer[j * nScanWidth + i] = pBuffer[(pScanArea.Top + j) * nBufferWidth + pScanArea.Left + i];
                for (int y = 0; y < nScanHeight - 2; y++)
                {
                    for (int x = 0; x < nScanWidth - 2; x++)
                    {
                        int nPosX, nPosY;
                        int nMaxValue = 0;
                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                nPosX = x + i;
                                nPosY = y + j;
                                int nValue = pBuffer[nPosY * nBufferWidth + nPosX];
                                if (nValue > nMaxValue)
                                    nMaxValue = nValue;
                            }
                        }

                        nPosX = pScanArea.Left + x + 1;
                        nPosY = pScanArea.Top + y + 1;
                        pBuffer[nPosY * nBufferWidth + nPosX] = (byte) nMaxValue;
                    }
                }

                return pResultBuffer;
            }

            public static YoonImage DilationAsBinary(YoonImage pSourceImage)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(
                    DilationAsBinary(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static YoonImage DilationAsBinary(YoonImage pSourceImage, int nFilterSize)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(
                    DilationAsBinary(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height,
                        nFilterSize),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static YoonImage DilationAsBinary(YoonImage pSourceImage, YoonRect2N pScanArea)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(DilationAsBinary(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pScanArea),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static YoonImage DilationAsBinary(YoonImage pSourceImage, YoonRect2N pScanArea, int nFilterSize)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(
                    DilationAsBinary(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, pScanArea,
                        nFilterSize),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] DilationAsBinary(byte[] pBuffer, int nBufferWidth, int nBufferHeight)
            {
                byte[] pResultBuffer = new byte[nBufferWidth * nBufferHeight];
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                for (int y = 0; y < nBufferHeight - 2; y++)
                {
                    for (int x = 0; x < nBufferWidth - 2; x++)
                    {
                        int nPosX, nPosY;
                        bool bWhite = false;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                nPosX = x + i;
                                nPosY = y + j;
                                if (pBuffer[nPosY * nBufferWidth + nPosX] <= 0) continue;
                                bWhite = true;
                                break;
                            }

                            if (bWhite) break;
                        }

                        nPosX = x + 1;
                        nPosY = y + 1;
                        if (bWhite)
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 255;
                        else
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 0;
                    }
                }

                return pResultBuffer;
            }

            public static byte[] DilationAsBinary(byte[] pBuffer, int nBufferWidth, YoonRect2N pScanArea)
            {
                int scanWidth = pScanArea.Width;
                int scanHeight = pScanArea.Height;
                byte[] pResultBuffer = new byte[pBuffer.Length];
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                byte[] pTempBuffer = new byte[scanWidth * scanHeight];
                for (int j = 0; j < scanHeight; j++)
                for (int i = 0; i < scanWidth; i++)
                    pTempBuffer[j * scanWidth + i] = pBuffer[(pScanArea.Top + j) * nBufferWidth + (pScanArea.Left + i)];
                for (int y = 0; y < scanHeight - 2; y++)
                {
                    for (int x = 0; x < scanWidth - 2; x++)
                    {
                        int nPosX, nPosY;
                        bool bWhite = false;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                nPosX = x + i;
                                nPosY = y + j;
                                if (pTempBuffer[nPosY * scanWidth + nPosX] <= 0) continue;
                                bWhite = true;
                                break;
                            }

                            if (bWhite) break;
                        }

                        nPosX = pScanArea.Left + (x + 1);
                        nPosY = pScanArea.Top + (y + 1);
                        if (bWhite)
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 255;
                        else
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 0;
                    }
                }

                return pResultBuffer;
            }

            public static byte[] DilationAsBinary(byte[] pBuffer, int nBufferWidth, int nBufferHeight, int size)
            {
                byte[] pResultBuffer = new byte[nBufferWidth * nBufferHeight];
                Array.Copy(pBuffer, pResultBuffer, pBuffer.Length);
                for (int y = 0; y < nBufferHeight - size; y++)
                {
                    for (int x = 0; x < nBufferWidth - size; x++)
                    {
                        int nPosX, nPosY;
                        bool bWhite = false;
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                nPosX = x + i;
                                nPosY = y + j;
                                if (pResultBuffer[nPosY * nBufferWidth + nPosX] > 0)
                                {
                                    bWhite = true;
                                    break;
                                }
                            }

                            if (bWhite) break;
                        }

                        nPosX = x + size / 2;
                        nPosY = y + size / 2;
                        if (bWhite)
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 255;
                        else
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 0;
                    }
                }

                return pResultBuffer;
            }

            public static byte[] DilationAsBinary(byte[] pBuffer, int nBufferWidth, int nBufferHeight,
                YoonRect2N pScanArea, int nSize)
            {
                byte[] pResultBuffer = new byte[nBufferWidth * nBufferHeight];
                pBuffer.CopyTo(pResultBuffer, 0);
                ////  테두리 부분(0, size-1)만 따로 검사.
                for (int y = pScanArea.Top; y < pScanArea.Bottom - nSize; y++)
                {
                    for (int x = pScanArea.Left; x < pScanArea.Right - nSize; x++)
                    {
                        bool bWhite = false;
                        int j1 = 0;
                        int j2 = nSize - 1;
                        for (int i = 0; i < nSize; i++)
                        {
                            if (pBuffer[(y + j1) * nBufferWidth + (x + i)] > 0 ||
                                pBuffer[(y + j2) * nBufferWidth + (x + i)] > 0)
                            {
                                bWhite = true;
                                break;
                            }
                        }

                        int i1 = 0;
                        int i2 = nSize - 1;
                        for (int j = 0; j < nSize; j++)
                        {
                            if (bWhite) break;
                            if (pBuffer[(y + j) * nBufferWidth + (x + i1)] > 0 ||
                                pBuffer[(y + j) * nBufferWidth + (x + i2)] > 0)
                            {
                                bWhite = true;
                                break;
                            }
                        }

                        int nPosX = x + nSize / 2;
                        int nPosY = y + nSize / 2;
                        if (bWhite)
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 255;
                        else
                            pResultBuffer[nPosY * nBufferWidth + nPosX] = 0;
                    }
                }

                return pResultBuffer;
            }
        }

        public static class Sort
        {
            public static void SortRects(ref List<YoonRect2N> pList, eYoonDir2D direction)
            {
                YoonRect2N pRectMin, pRectCurr, pRectTemp;
                int nCount = pList.Count;
                for (int i = 0; i < nCount - 1; i++)
                {
                    int nMinCount = i;
                    for (int j = i + 1; j < nCount; j++)
                    {
                        pRectMin = pList[nMinCount];
                        pRectCurr = pList[j];
                        int nDiff = 0;
                        int nHeight = 0;
                        // Height difference takes precedence then the width difference
                        switch (direction)
                        {
                            case eYoonDir2D.TopLeft:
                                nDiff = Math.Abs(pRectMin.Top - pRectCurr.Top);
                                nHeight = pRectMin.Bottom - pRectMin.Top;
                                if (nDiff <= nHeight / 2)
                                {
                                    if (pRectCurr.Left < pRectMin.Left)
                                        nMinCount = j;
                                }
                                else
                                {
                                    if (pRectCurr.Top < pRectMin.Top)
                                        nMinCount = j;
                                }

                                break;
                            case eYoonDir2D.TopRight:
                                nDiff = Math.Abs(pRectMin.Top - pRectCurr.Top);
                                nHeight = pRectMin.Bottom - pRectMin.Top;
                                if (nDiff <= nHeight / 2)
                                {
                                    if (pRectCurr.Right > pRectMin.Right)
                                        nMinCount = j;
                                }
                                else
                                {
                                    if (pRectCurr.Top < pRectMin.Top)
                                        nMinCount = j;
                                }

                                break;
                            case eYoonDir2D.Left:
                                if (pRectCurr.Left < pRectMin.Left)
                                    nMinCount = j;
                                break;
                            case eYoonDir2D.Right:
                                if (pRectCurr.Right > pRectMin.Right)
                                    nMinCount = j;
                                break;
                            default: // Top-Left default
                                nDiff = Math.Abs(pRectMin.Top - pRectCurr.Top);
                                nHeight = pRectMin.Bottom - pRectMin.Top;
                                if (nDiff <= nHeight / 2)
                                    continue;
                                if (pRectCurr.Left < pRectMin.Left)
                                    nMinCount = j;
                                break;
                        }
                    }

                    pRectMin = pList[nMinCount];
                    pRectCurr = pList[i];
                    // Back up the current rectangle and insert the minimum rectangle
                    pRectTemp = new YoonRect2N(pRectCurr.CenterPos.X, pRectCurr.CenterPos.Y, pRectCurr.Width,
                        pRectCurr.Height);
                    pList[i] = new YoonRect2N(pRectMin.CenterPos.X, pRectMin.CenterPos.Y, pRectMin.Width,
                        pRectMin.Height);
                    pList[nMinCount] = pRectTemp;
                }
            }

            public static void CombineRects(ref List<YoonRect2N> pList)
            {
                bool bCombine = false;
                YoonRect2N pRect1 = new YoonRect2N();
                List<YoonRect2N> pListTemp = new List<YoonRect2N>();
                int nTotalCount = pList.Count;
                if (nTotalCount <= 1) return;
                // Copy the original list, then clear that
                pListTemp = pList.GetRange(0, pList.Count);
                pList.Clear();
                // Find all of squares to overlap
                for (int i = 0; i < nTotalCount; i++)
                {
                    pRect1 = pListTemp[i];
                    YoonRect2N pCombineRect = new YoonRect2N(0, 0, 0, 0);
                    if (pRect1.Width == 0)
                        continue;
                    bCombine = false;
                    for (int j = 0; j < nTotalCount; j++)
                    {
                        if (i == j) continue;
                        YoonRect2N pRect2 = pListTemp[j] as YoonRect2N;
                        if (pRect2.Width == 0)
                            continue;
                        // Overlap two rectangles
                        if ((pRect1.Left > pRect2.Left) && (pRect1.Left < pRect2.Right))
                        {
                            if ((pRect1.Top >= pRect2.Top) && (pRect1.Top <= pRect2.Bottom))
                                bCombine = true;
                            if ((pRect1.Bottom >= pRect2.Top) && (pRect1.Bottom <= pRect2.Bottom))
                                bCombine = true;
                            if ((pRect1.Top <= pRect2.Top) && (pRect1.Bottom >= pRect2.Bottom))
                                bCombine = true;
                        }

                        if (pRect1.Right > pRect2.Left && pRect1.Right < pRect2.Right)
                        {
                            if ((pRect1.Top >= pRect2.Top) && (pRect1.Top <= pRect2.Bottom))
                                bCombine = true;
                            if ((pRect1.Bottom >= pRect2.Top) && (pRect1.Bottom <= pRect2.Bottom))
                                bCombine = true;
                            if ((pRect1.Top <= pRect2.Top) && (pRect1.Bottom >= pRect2.Bottom))
                                bCombine = true;
                        }

                        if ((pRect1.Left <= pRect2.Left) && (pRect1.Right >= pRect2.Right))
                        {
                            if ((pRect1.Top >= pRect2.Top) && (pRect1.Top <= pRect2.Bottom))
                                bCombine = true;
                            if ((pRect1.Bottom >= pRect2.Top) && (pRect1.Bottom <= pRect2.Bottom))
                                bCombine = true;
                            if ((pRect1.Top <= pRect2.Top) && (pRect1.Bottom >= pRect2.Bottom))
                                bCombine = true;
                        }

                        if (bCombine)
                        {
                            pCombineRect.CenterPos.X =
                                (pRect1.Left < pRect2.Left) ? pRect1.CenterPos.X : pRect2.CenterPos.X;
                            pCombineRect.Width = (pRect1.Right > pRect2.Right)
                                ? pRect1.Right - pCombineRect.Left
                                : pRect2.Right - pCombineRect.Left;
                            pCombineRect.CenterPos.Y =
                                (pRect1.Top < pRect2.Top) ? pRect1.CenterPos.Y : pRect2.CenterPos.Y;
                            pCombineRect.Height = (pRect1.Bottom > pRect2.Bottom)
                                ? pRect1.Bottom - pCombineRect.Top
                                : pRect2.Bottom - pCombineRect.Top;
                            pListTemp[i] = new YoonRect2N(0, 0, 0, 0);
                            pListTemp[j] = pCombineRect;
                            break;
                        }
                    }
                }

                // Sort only valid squares
                for (int i = 0; i < nTotalCount; i++)
                {
                    pRect1 = pListTemp[i];
                    if (pRect1.Right != 0)
                    {
                        YoonRect2N pRect2 = new YoonRect2N(pRect1.CenterPos.X, pRect1.CenterPos.Y, pRect1.Width,
                            pRect1.Height);
                        pList.Add(pRect2);
                    }
                }

                pListTemp.Clear();
            }

            public static void SortIntegers(ref List<int> pList, eYoonDir2DMode direction)
            {
                int nCount = pList.Count;
                int nMinCount = 0;
                int nMaxCount = 0;
                // Sort ascending
                if (direction == eYoonDir2DMode.Increase)
                {
                    for (int i = 0; i < nCount - 1; i++)
                    {
                        int nMinValue, nCurrValue, nTempValue;
                        nMinCount = i;
                        for (int j = i + 1; j < nCount; j++)
                        {
                            nMinValue = pList[nMinCount];
                            nCurrValue = pList[j];
                            if (nCurrValue < nMinValue)
                                nMinCount = j;
                        }

                        nMinValue = pList[nMinCount];
                        nCurrValue = pList[i];
                        nTempValue = nCurrValue;
                        pList[i] = nMinValue;
                        pList[nMinCount] = nTempValue;
                    }
                }
                // Sort descending
                else if (direction == eYoonDir2DMode.Decrease)
                {
                    for (int i = 0; i < nCount - 1; i++)
                    {
                        int nMaxValue, nCurrValue, nTempValue;
                        nMaxCount = i;
                        for (int j = i + 1; j < nCount; j++)
                        {
                            nMaxValue = (int) pList[nMaxCount];
                            nCurrValue = (int) pList[j];
                            if (nCurrValue > nMaxValue)
                                nMaxCount = j;
                        }

                        nMaxValue = (int) pList[nMaxCount];
                        nCurrValue = (int) pList[i];
                        nTempValue = nCurrValue;
                        pList[i] = nMaxValue;
                        pList[nMaxCount] = nTempValue;
                    }
                }
            }
        }

        // Pixel Scan 및 추출
        public static class Scanner
        {
            public static IYoonVector Scan1D(YoonImage pSourceImage, eYoonDir2D nDir, YoonVector2N vecStart, int threshold = 128, bool isWhite = false)
            {
                if (pSourceImage.Plane == 1)
                {
                    switch (nDir)
                    {
                        case eYoonDir2D.Top:
                            return ScanTop(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, vecStart, (byte)threshold, isWhite);
                        case eYoonDir2D.Bottom:
                            return ScanBottom(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, vecStart, (byte)threshold, isWhite);
                        case eYoonDir2D.Left:
                            return ScanLeft(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, vecStart, (byte)threshold, isWhite);
                        case eYoonDir2D.Right:
                            return ScanRight(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, vecStart, (byte)threshold, isWhite);
                        default:
                            throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Scan direction is not correct");
                    }
                }
                else if (pSourceImage.Plane == 4)
                {
                    switch (nDir)
                    {
                        case eYoonDir2D.Top:
                            return ScanTop(pSourceImage.GetARGBBuffer(), pSourceImage.Width, pSourceImage.Height, vecStart, (byte)threshold, isWhite);
                        case eYoonDir2D.Bottom:
                            return ScanBottom(pSourceImage.GetARGBBuffer(), pSourceImage.Width, pSourceImage.Height, vecStart, (byte)threshold, isWhite);
                        case eYoonDir2D.Left:
                            return ScanLeft(pSourceImage.GetARGBBuffer(), pSourceImage.Width, pSourceImage.Height, vecStart, (byte)threshold, isWhite);
                        case eYoonDir2D.Right:
                            return ScanRight(pSourceImage.GetARGBBuffer(), pSourceImage.Width, pSourceImage.Height, vecStart, (byte)threshold, isWhite);
                        default:
                            throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Scan direction is not correct");
                    }
                }
                else
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
            }

            //  왼쪽 방향으로 Scan하며 threshold보다 크거나 작은 Gray Level 값 가져오기.
            public static IYoonVector ScanLeft(int[] pBuffer, int width, int height, YoonVector2N startPos, int threshold, bool isWhite)
            {
                int value;
                YoonVector2N resultPos = new YoonVector2N(startPos);
                value = pBuffer[resultPos.Y * width + resultPos.X];
                if (isWhite)
                {
                    while (value > threshold && resultPos.X > 0)
                    {
                        resultPos.X--;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                else
                {
                    while (value <= threshold && resultPos.X > 0)
                    {
                        resultPos.X--;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                return resultPos;
            }

            public static IYoonVector ScanLeft(byte[] pBuffer, int width, int height, YoonVector2N startPos, byte threshold, bool isWhite)
            {
                byte value;
                YoonVector2N resultPos = new YoonVector2N(startPos);
                value = pBuffer[resultPos.Y * width + resultPos.X];
                if (isWhite)
                {
                    while (value > threshold && resultPos.X > 0)
                    {
                        resultPos.X--;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                else
                {
                    while (value <= threshold && resultPos.X > 0)
                    {
                        resultPos.X--;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                return resultPos;
            }

            //  오른쪽 방향으로 Scan하며 threshold보다 크거나 작은 Gray Level 값 가져오기.
            public static IYoonVector ScanRight(int[] pBuffer, int width, int height, YoonVector2N startPos, int threshold, bool isWhite)
            {
                int value;
                YoonVector2N resultPos = new YoonVector2N(startPos);
                value = pBuffer[resultPos.Y * width + resultPos.X];
                if (isWhite)
                {
                    while (value > threshold && resultPos.X < width)
                    {
                        resultPos.X++;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                else
                {
                    while (value <= threshold && resultPos.X < width)
                    {
                        resultPos.X++;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                return resultPos;
            }

            public static IYoonVector ScanRight(byte[] pBuffer, int width, int height, YoonVector2N startPos, byte threshold, bool isWhite)
            {
                byte value;
                YoonVector2N resultPos = new YoonVector2N(startPos);
                value = pBuffer[resultPos.Y * width + resultPos.X];
                if (isWhite)
                {
                    while (value > threshold && resultPos.X < width)
                    {
                        resultPos.X++;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                else
                {
                    while (value <= threshold && resultPos.X < width)
                    {
                        resultPos.X++;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                return resultPos;
            }

            //  위쪽 방향으로 Scan하며 threshold보다 크거나 작은 Gray Level 값 가져오기.
            public static IYoonVector ScanTop(int[] pBuffer, int width, int height, YoonVector2N startPos, int threshold, bool isWhite)
            {
                int value;
                YoonVector2N resultPos = new YoonVector2N(startPos);
                value = pBuffer[resultPos.Y * width + resultPos.X];
                if (isWhite)
                {
                    while (value > threshold && resultPos.Y > 0)
                    {
                        resultPos.Y--;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                else
                {
                    while (value <= threshold && resultPos.Y > 0)
                    {
                        resultPos.Y--;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                return resultPos;
            }

            public static IYoonVector ScanTop(byte[] pBuffer, int width, int height, YoonVector2N startPos, byte threshold, bool isWhite)
            {
                byte value;
                YoonVector2N resultPos = new YoonVector2N(startPos);
                value = pBuffer[resultPos.Y * width + resultPos.X];
                if (isWhite)
                {
                    while (value > threshold && resultPos.Y > 0)
                    {
                        resultPos.Y--;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                else
                {
                    while (value <= threshold && resultPos.Y > 0)
                    {
                        resultPos.Y--;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                return resultPos;
            }

            //  위쪽 방향으로 Scan하며 threshold보다 크거나 작은 Gray Level 값 가져오기.
            public static IYoonVector ScanBottom(int[] pBuffer, int width, int height, YoonVector2N startPos, int threshold, bool isWhite)
            {
                int value;
                YoonVector2N resultPos = new YoonVector2N(startPos);
                value = pBuffer[resultPos.Y * width + resultPos.X];
                if (isWhite)
                {
                    while (value > threshold && resultPos.Y < height)
                    {
                        resultPos.Y++;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                else
                {
                    while (value <= threshold && resultPos.Y < height)
                    {
                        resultPos.Y++;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                return resultPos;
            }

            public static IYoonVector ScanBottom(byte[] pBuffer, int width, int height, YoonVector2N startPos, byte threshold, bool isWhite)
            {
                byte value;
                YoonVector2N resultPos = new YoonVector2N(startPos);
                value = pBuffer[resultPos.Y * width + resultPos.X];
                if (isWhite)
                {
                    while (value > threshold && resultPos.Y < height)
                    {
                        resultPos.Y++;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                else
                {
                    while (value <= threshold && resultPos.Y < height)
                    {
                        resultPos.Y++;
                        value = pBuffer[resultPos.Y * width + resultPos.X];
                    }
                }
                return resultPos;
            }

            public static IYoonVector Scan2D(YoonImage pSourceImage, eYoonDir2D nDir, YoonVector2N vecStart, int threshold = 128, bool isWhite = false)
            {
                if (pSourceImage.Plane == 1)
                    return Scan2D(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, nDir, vecStart, (byte)threshold, isWhite);
                else if (pSourceImage.Plane == 4)
                    return Scan2D(pSourceImage.GetARGBBuffer(), pSourceImage.Width, pSourceImage.Height, nDir, vecStart, threshold, isWhite);
                else
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
            }

            //  Object, Pattern 등의 시작위치 찾기
            public static IYoonVector Scan2D(byte[] pBuffer, int width, int height, eYoonDir2D nDir, YoonVector2N startPos, byte threshold, bool isWhite)
            {
                if (startPos.X >= width && startPos.Y >= height)
                    return new YoonVector2N(-1, -1);
                if (startPos.X < 0 && startPos.Y < 0)
                    return new YoonVector2N(-1, -1);
                if (startPos.X >= width || startPos.X < 0)
                {
                    startPos.Move(eYoonDir2D.Bottom);
                    return Scan2D(pBuffer, width, height, nDir.ReverseX(), startPos, threshold, isWhite);
                }
                if (startPos.Y >= height || startPos.Y < 0)
                {
                    startPos.Move(eYoonDir2D.Right);
                    return Scan2D(pBuffer, width, height, nDir.ReverseY(), startPos, threshold, isWhite);
                }
                ////  White IYoonVector를 찾는다.
                if (isWhite)
                {
                    if (pBuffer[startPos.Y * width + startPos.X] >= threshold)
                        return startPos.Clone();
                    else
                    {
                        startPos.Move(nDir);
                        return Scan2D(pBuffer, width, height, nDir, startPos, threshold, isWhite);
                    }
                }
                ////  Black IYoonVector를 찾는다.
                else
                {
                    if (pBuffer[startPos.Y * width + startPos.X] < threshold)
                        return startPos.Clone();
                    else
                    {
                        startPos.Move(nDir);
                        return Scan2D(pBuffer, width, height, nDir, startPos, threshold, isWhite);
                    }
                }
            }

            public static IYoonVector Scan2D(int[] pBuffer, int width, int height, eYoonDir2D nDir, YoonVector2N startPos, int threshold, bool isWhite)
            {
                if (startPos.X >= width && startPos.Y >= height)
                    return new YoonVector2N(-1, -1);
                if (startPos.X < 0 && startPos.Y < 0)
                    return new YoonVector2N(-1, -1);
                if (startPos.X >= width || startPos.X < 0)
                {
                    startPos.Move(eYoonDir2D.Bottom);
                    return Scan2D(pBuffer, width, height, nDir.ReverseX(), startPos, threshold, isWhite);
                }
                if (startPos.Y >= height || startPos.Y < 0)
                {
                    startPos.Move(eYoonDir2D.Right);
                    return Scan2D(pBuffer, width, height, nDir.ReverseY(), startPos, threshold, isWhite);
                }
                ////  White IYoonVector를 찾는다.
                if (isWhite)
                {
                    if (pBuffer[startPos.Y * width + startPos.X] >= threshold)
                        return startPos.Clone();
                    else
                    {
                        startPos.Move(nDir);
                        return Scan2D(pBuffer, width, height, nDir, startPos, threshold, isWhite);
                    }
                }
                ////  Black IYoonVector를 찾는다.
                else
                {
                    if (pBuffer[startPos.Y * width + startPos.X] < threshold)
                        return startPos.Clone();
                    else
                    {
                        startPos.Move(nDir);
                        return Scan2D(pBuffer, width, height, nDir, startPos, threshold, isWhite);
                    }
                }
            }
        }

        // Threshold 추출 및 Pixel 정보 추출
        public static class PixelInspector // -> YoonImage
        {
            //  Histogram 상에서 Peak점, 즉 가장 많은 Gray Level을 추출한다.
            public static byte GetNumerousThreshold(YoonImage pSourceImage)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return GetNumerousThreshold(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height);
            }

            public static byte GetNumerousThreshold(YoonImage pSourceImage, YoonRect2N scanArea)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return GetNumerousThreshold(pSourceImage.GetGrayBuffer(), pSourceImage.Width, scanArea);
            }

            public static byte GetNumerousThreshold(byte[] pBuffer, int imageWidth, YoonRect2N scanArea)
            {
                int i, j, value;
                int maxNumber;
                byte maxLevel;
                int[] histogram = new int[256];
                Array.Clear(histogram, 0, histogram.Length);
                ////  Histogram 그래프를 만든다.
                for (i = scanArea.Left; i < scanArea.Right; i++)
                {
                    for (j = scanArea.Top; j < scanArea.Bottom; j++)
                    {
                        value = pBuffer[j * imageWidth + i];
                        if (value > 255 || value < 0)
                            continue;
                        histogram[value]++;
                    }
                }
                ////  Peak 위치의 Pixel 갯수(number)와 그 때의 X축값(gray level)을 추출한다.
                maxLevel = 0;
                maxNumber = 0;
                for (i = 0; i < 256; i++)
                {
                    if (histogram[i] > maxNumber)
                    {
                        maxNumber = histogram[i];
                        maxLevel = (byte)i;
                    }
                }
                return maxLevel;
            }

            public static byte GetNumerousThreshold(byte[] pBuffer, int imageWidth, int imageHeight)
            {
                int i, value;
                int size = imageWidth * imageHeight;
                int[] histogram = new int[256];
                Array.Clear(histogram, 0, histogram.Length);
                ////  Histogram 그래프를 만든다.
                for (i = 0; i < size; i++)
                {
                    value = pBuffer[i];
                    if (value > 255 || value < 0)
                        continue;
                    histogram[value]++;
                }
                ////  Peak 위치의 Pixel 갯수(number)와 그 때의 X축값(gray level)을 추출한다.
                byte maxLevel = 0;
                int maxNumber = 0;
                for (i = 0; i < 256; i++)
                {
                    if (histogram[i] > maxNumber)
                    {
                        maxNumber = histogram[i];
                        maxLevel = (byte)i;
                    }
                }
                return maxLevel;
            }

            //  Gray Level의 평균을 Threshold로 가져온다.
            public static byte GetAverageThreshold(YoonImage pSourceImage, YoonRect2N scanArea)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return GetAverageThreshold(pSourceImage.GetGrayBuffer(), pSourceImage.Width, scanArea);
            }

            public static byte GetAverageThreshold(byte[] pBuffer, int imageWidth, YoonRect2N scanArea)
            {
                int i, j;
                int sum, count;
                int average;
                sum = 0;
                count = 0;
                for (j = scanArea.Top; j < scanArea.Bottom; j++)
                {
                    for (i = scanArea.Left; i < scanArea.Right; i++)
                    {
                        sum += pBuffer[j * imageWidth + i];
                        count++;
                    }
                }
                if (count < 1) count = 1;
                average = sum / count;
                return (byte)(average > 255 ? 255 : average);
            }

            //  Gray Level의 최대값과 최소값 사이를 Threshold로 설정한다.
            public static byte GetMinMaxThreshold(YoonImage pSourceImage, YoonRect2N scanArea)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return GetMinMaxThreshold(pSourceImage.GetGrayBuffer(), pSourceImage.Width, scanArea);
            }

            public static byte GetMinMaxThreshold(byte[] pBuffer, int imageWidth, YoonRect2N scanArea)
            {
                int average, sum, count;
                int i, j, ii, jj;
                int minLevel, maxLevel, threshold;
                minLevel = 100000;
                maxLevel = 0;
                for (j = scanArea.Top; j < scanArea.Bottom - 3; j++)
                {
                    for (i = scanArea.Left; i < scanArea.Right - 3; i++)
                    {
                        sum = 0;
                        count = 0;
                        for (jj = 0; jj < 3; jj++)
                        {
                            for (ii = 0; ii < 3; ii++)
                            {
                                sum += pBuffer[(j + jj) * imageWidth + (i + ii)];
                                count++;
                            }
                        }
                        average = sum / count;
                        if (average < minLevel) minLevel = average;
                        if (average > maxLevel) maxLevel = average;
                    }
                }
                threshold = (minLevel * 2 + maxLevel) / 3;
                return (byte)(threshold > 255 ? 255 : threshold);
            }

            //  Gray Level 관련 정보 가져오기.
            public static void GetLevelInfo(YoonImage pSourceImage, YoonRect2N scanArea, out int Min, out int Max, out int Average)
            {
                if (pSourceImage.Plane == 1)
                    GetLevelInfo(pSourceImage.GetGrayBuffer(), pSourceImage.Width, scanArea, out Min, out Max, out Average);
                else if (pSourceImage.Plane == 4)
                    GetLevelInfo(pSourceImage.GetARGBBuffer(), pSourceImage.Width, scanArea, out Min, out Max, out Average);
                else
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
            }

            public static void GetLevelInfo(byte[] pBuffer, int imageWidth, YoonRect2N scanArea, out int Min, out int Max, out int Average)
            {
                if (scanArea.Right > imageWidth)
                {
                    Min = Max = Average = -1;
                    return;
                }
                int i, j, ii, jj;
                int count, sum, step;
                double totalSum, totalCount;
                int min, max, average;
                Min = 0;
                Max = 0;
                Average = 0;
                step = 5;
                min = 1000000;
                max = 0;
                totalSum = 0;
                totalCount = 0;
                ////  평균 Gray Level 산출.
                for (j = scanArea.Top; j < scanArea.Bottom; j++)
                {
                    for (i = scanArea.Left; i < scanArea.Right; i++)
                    {
                        if (pBuffer[j * imageWidth + i] >= 255)
                            continue;
                        totalSum += pBuffer[j * imageWidth + i];
                        totalCount++;
                    }
                }
                if (totalCount < 1)
                    totalCount = 1;
                average = (int)(totalSum / totalCount);
                ////  최대, 최소 Gray Level 산출.
                for (j = scanArea.Top; j < scanArea.Bottom - step; j += step)
                {
                    for (i = scanArea.Left; i < scanArea.Right - step; i += step)
                    {
                        sum = 0;
                        count = 0;
                        for (jj = 0; jj < step; jj++)
                        {
                            for (ii = 0; ii < step; ii++)
                            {
                                sum += pBuffer[(j + jj) * imageWidth + (i + ii)];
                                count++;
                            }
                        }
                        sum /= count;
                        if (sum >= 255)
                            continue;
                        if (sum < min) min = sum;
                        if (sum > max) max = sum;
                    }
                }
                Min = min;
                Max = max;
                Average = average;
            }

            public static void GetLevelInfo(int[] pBuffer, int imageWidth, YoonRect2N scanArea, out int Min, out int Max, out int Average)
            {
                if (scanArea.Right > imageWidth)
                {
                    Min = Max = Average = -1;
                    return;
                }

                int i, j, ii, jj;
                int count, sum, step;
                double totalSum, totalCount;
                int min, max, average;
                step = 3;
                min = 1000000;
                max = 0;
                totalSum = 0;
                totalCount = 0;
                for (j = scanArea.Top; j < scanArea.Bottom; j++)
                {
                    for (i = scanArea.Left; i < scanArea.Right; i++)
                    {
                        if (pBuffer[j * imageWidth + i] >= 255)
                            continue;
                        totalSum += pBuffer[j * imageWidth + i];
                        totalCount++;
                    }
                }
                if (totalCount < 1)
                    totalCount = 1;
                average = (int)(totalSum / totalCount);

                for (j = scanArea.Top; j < scanArea.Bottom - step; j += step)
                {
                    for (i = scanArea.Left; i < scanArea.Right - step; i += step)
                    {
                        sum = 0;
                        count = 0;
                        for (jj = 0; jj < step; jj++)
                        {
                            for (ii = 0; ii < step; ii++)
                            {
                                sum += pBuffer[(j + jj) * imageWidth + (i + ii)];
                                count++;
                            }
                        }
                        sum /= count;
                        if (sum >= 255)
                            continue;
                        if (sum < min) min = sum;
                        if (sum > max) max = sum;
                    }
                }
                Min = min;
                Max = max;
                Average = average;
            }
        }

        // Image에 각종 도형 그리기
        public static class Draw
        {
            //  삼각형 칠하기.
            public static void FillTriangle(ref Bitmap pImage, int x, int y, int size, eYoonDir2D direction, Color fillColor, double zoom)
            {
                PointF[] pPoint = new PointF[3];
                pPoint[0].X = (float)(x * zoom);
                pPoint[0].Y = (float)(y * zoom);
                switch (direction)
                {
                    case eYoonDir2D.Top:
                        pPoint[1].X = pPoint[0].X + (float)(size / 2 * zoom);
                        pPoint[1].Y = pPoint[0].Y + (float)(size * zoom);
                        pPoint[2].X = pPoint[0].X - (float)(size / 2 * zoom);
                        pPoint[2].Y = pPoint[0].Y + (float)(size * zoom);
                        break;
                    case eYoonDir2D.Bottom:
                        pPoint[1].X = pPoint[0].X - (float)(size / 2 * zoom);
                        pPoint[1].Y = pPoint[0].Y - (float)(size * zoom);
                        pPoint[2].X = pPoint[0].X + (float)(size / 2 * zoom);
                        pPoint[2].Y = pPoint[0].Y - (float)(size * zoom);
                        break;
                    case eYoonDir2D.Left:
                        pPoint[1].X = pPoint[0].X + (float)(size * zoom);
                        pPoint[1].Y = pPoint[0].Y - (float)(size / 2 * zoom);
                        pPoint[2].X = pPoint[0].X + (float)(size * zoom);
                        pPoint[2].Y = pPoint[0].Y + (float)(size / 2 * zoom);
                        break;
                    case eYoonDir2D.Right:
                        pPoint[1].X = pPoint[0].X - (float)(size * zoom);
                        pPoint[1].Y = pPoint[0].Y + (float)(size / 2 * zoom);
                        pPoint[2].X = pPoint[0].X - (float)(size * zoom);
                        pPoint[2].Y = pPoint[0].Y - (float)(size / 2 * zoom);
                        break;
                }
                using (Graphics graph = Graphics.FromImage(pImage))
                {
                    SolidBrush brush = new SolidBrush(fillColor);
                    graph.FillPolygon(brush, pPoint);
                }
            }

            //  사각형 칠하기.
            public static void FillRect(ref Bitmap pImage, int centerX, int centerY, int width, int height, Color fillColor, double zoom)
            {
                float startX = (float)(centerX - width / 2) * (float)zoom;
                float startY = (float)(centerY - height / 2) * (float)zoom;
                using (Graphics graph = Graphics.FromImage(pImage))
                {
                    SolidBrush brush = new SolidBrush(fillColor);
                    graph.FillRectangle(brush, startX, startY, (float)width, (float)height);
                }
            }

            //  다각형 칠하기.
            public static void FillPoligon(ref Bitmap pImage, YoonVector2N[] pArrayPoint, Color fillColor, double zoom)
            {
                PointF[] pArrayDraw = new PointF[pArrayPoint.Length];
                for(int iPoint = 0; iPoint<pArrayPoint.Length; iPoint++)
                {
                    pArrayDraw[iPoint].X = (float)pArrayPoint[iPoint].X * (float)zoom;
                    pArrayDraw[iPoint].Y = (float)pArrayPoint[iPoint].Y * (float)zoom;
                }

                using (Graphics graph = Graphics.FromImage(pImage))
                {
                    SolidBrush brush = new SolidBrush(fillColor);
                    graph.FillPolygon(brush, pArrayDraw);
                }
            }

            //  전부 칠하기.
            public static void FillCanvas(ref Bitmap pImage, Color fillColor)
            {
                Rectangle pRectCanvas = new Rectangle(0, 0, pImage.Width, pImage.Height);
                using(Graphics graph = Graphics.FromImage(pImage))
                {
                    SolidBrush brush = new SolidBrush(fillColor);
                    Region pRegion = new Region(pRectCanvas);
                    graph.FillRegion(brush, pRegion);
                }
            }

            //  삼각형 그리기.
            public static void DrawTriangle(ref Bitmap pImage, int x, int y, int size, eYoonDir2D direction, Color penColor, int penWidth,  double zoom)
            {
                PointF[] pIYoonVector = new PointF[3];
                pIYoonVector[0].X = (float)(x * zoom);
                pIYoonVector[0].Y = (float)(y * zoom);
                switch (direction)
                {
                    case eYoonDir2D.Top:
                        pIYoonVector[1].X = pIYoonVector[0].X + (float)(size / 2 * zoom);
                        pIYoonVector[1].Y = pIYoonVector[0].Y + (float)(size * zoom);
                        pIYoonVector[2].X = pIYoonVector[0].X - (float)(size / 2 * zoom);
                        pIYoonVector[2].Y = pIYoonVector[0].Y + (float)(size * zoom);
                        break;
                    case eYoonDir2D.Bottom:
                        pIYoonVector[1].X = pIYoonVector[0].X - (float)(size / 2 * zoom);
                        pIYoonVector[1].Y = pIYoonVector[0].Y - (float)(size * zoom);
                        pIYoonVector[2].X = pIYoonVector[0].X + (float)(size / 2 * zoom);
                        pIYoonVector[2].Y = pIYoonVector[0].Y - (float)(size * zoom);
                        break;
                    case eYoonDir2D.Left:
                        pIYoonVector[1].X = pIYoonVector[0].X + (float)(size * zoom);
                        pIYoonVector[1].Y = pIYoonVector[0].Y - (float)(size / 2 * zoom);
                        pIYoonVector[2].X = pIYoonVector[0].X + (float)(size * zoom);
                        pIYoonVector[2].Y = pIYoonVector[0].Y + (float)(size / 2 * zoom);
                        break;
                    case eYoonDir2D.Right:
                        pIYoonVector[1].X = pIYoonVector[0].X - (float)(size * zoom);
                        pIYoonVector[1].Y = pIYoonVector[0].Y + (float)(size / 2 * zoom);
                        pIYoonVector[2].X = pIYoonVector[0].X - (float)(size * zoom);
                        pIYoonVector[2].Y = pIYoonVector[0].Y - (float)(size / 2 * zoom);
                        break;
                }
                using (Graphics graph = Graphics.FromImage(pImage))
                {
                    Pen pen = new Pen(penColor, (float)penWidth);
                    graph.DrawLine(pen, pIYoonVector[0], pIYoonVector[1]);
                    graph.DrawLine(pen, pIYoonVector[1], pIYoonVector[2]);
                    graph.DrawLine(pen, pIYoonVector[2], pIYoonVector[0]);
                }
            }

            //  사각형 그리기.
            public static void DrawRect(ref Bitmap pImage, YoonRect2N rect, Color penColor, int penWidth, double zoom)
            {
                if (rect.Right <= rect.Left || rect.Bottom <= rect.Top)
                    return;
                DrawLine(ref pImage, rect.Left, rect.Top, rect.Right, rect.Top, penColor, penWidth, zoom);
                DrawLine(ref pImage, rect.Right, rect.Top, rect.Right, rect.Bottom, penColor, penWidth, zoom);
                DrawLine(ref pImage, rect.Right, rect.Bottom, rect.Left, rect.Bottom, penColor, penWidth, zoom);
                DrawLine(ref pImage, rect.Left, rect.Bottom, rect.Left, rect.Top, penColor, penWidth, zoom);
            }

            //  선분 그리기.
            public static void DrawLine(ref Bitmap pImage, int x, int y, int x1, int y1, Color penColor, int penWidth, double zoom)
            {
                double deltaX, deltaY, deltaX1, deltaY1;
                deltaX = (double)x * zoom;
                deltaY = (double)y * zoom;
                deltaX1 = (double)x1 * zoom;
                deltaY1 = (double)y1 * zoom;
                using (Graphics graph = Graphics.FromImage(pImage))
                {
                    Pen pen = new Pen(penColor, penWidth);
                    graph.DrawLine(pen, new PointF((float)Math.Round(deltaX), (float)Math.Round(deltaY)), new PointF((float)Math.Round(deltaX1), (float)Math.Round(deltaY1)));
                }
            }

            //  글자 적기.
            public static void DrawText(ref Bitmap pImage, int x, int y, Color fontColor, string text, int fontSize,  double ratio)
            {
                float deltaX, deltaY, size;
                deltaX = (float)(x * ratio);
                deltaY = (float)(y * ratio);
                size = (float)fontSize;
                if (size < 10) size = 10;
                using (Graphics graph = Graphics.FromImage(pImage))
                {
                    Brush brush = new SolidBrush(fontColor);
                    FontFamily fontFamily = new FontFamily("Tahoma");
                    Font font = new Font(fontFamily, size, FontStyle.Regular, GraphicsUnit.Pixel);
                    graph.DrawString(text, font, brush, deltaX, deltaY);
                }
            }

            //  십자가 그리기.
            public static void DrawCross(ref Bitmap pImage, int x, int y, Color penColor, int size, int penWidth, double zoom)
            {
                float deltaX, deltaY;
                float x1, x2, y1, y2;
                deltaX = (float)(x * zoom);
                deltaY = (float)(y * zoom);
                x1 = deltaX - size;
                x2 = deltaX + size;
                y1 = deltaY - size;
                y2 = deltaY + size;
                using (Graphics graph = Graphics.FromImage(pImage))
                {
                    Pen pen = new Pen(penColor, (float)penWidth);
                    graph.DrawLine(pen, new PointF(x1, deltaY), new PointF(x2, deltaY));
                    graph.DrawLine(pen, new PointF(deltaX, y1), new PointF(deltaX, y2));
                }
            }

            //  타원 그리기.
            public static void DrawEllipse(ref Bitmap pImage, YoonRect2N rect, Color penColor, int penWidth, double ratio)
            {
                int x1, y1, x2, y2;
                x1 = (int)Math.Round(rect.Left * ratio);
                y1 = (int)Math.Round(rect.Top * ratio);
                x2 = (int)Math.Round(rect.Right * ratio);
                y2 = (int)Math.Round(rect.Bottom * ratio);
                using (Graphics graph = Graphics.FromImage(pImage))
                {
                    Pen pen = new Pen(penColor, (float)penWidth);
                    graph.DrawEllipse(pen, x1, y1, (x2 - x1), (y2 - y1));
                }
            }
        }

        // Image 변형하기 (확대, 축소, 기울이기 등)  
        public static class Transform
        {
            //  Image 확대, 축소하기.
            public static YoonImage Zoom(YoonImage pSourceImage, double dRatio)
            {
                if (pSourceImage.Plane == 1)
                    return new YoonImage(Zoom(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, dRatio),
                        pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
                else if (pSourceImage.Plane == 4)
                    return new YoonImage(Zoom(pSourceImage.GetARGBBuffer(), pSourceImage.Width, pSourceImage.Height, dRatio),
                        pSourceImage.Width, pSourceImage.Height, PixelFormat.Format32bppArgb);
                else
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
            }

            public static YoonImage Zoom(YoonImage pSourceImage, double dRatioX, double dRatioY)
            {
                if (pSourceImage.Plane == 1)
                    return new YoonImage(Zoom(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, dRatioX, dRatioY),
                        pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
                else if (pSourceImage.Plane == 4)
                    return new YoonImage(Zoom(pSourceImage.GetARGBBuffer(), pSourceImage.Width, pSourceImage.Height, dRatioX, dRatioY),
                        pSourceImage.Width, pSourceImage.Height, PixelFormat.Format32bppArgb);
                else
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
            }

            public static byte[] Zoom(byte[] pBuffer, int sourceWidth, int sourceHeight, double dRatio)
            {
                return Zoom(pBuffer, sourceWidth, sourceHeight, (int)(sourceWidth * dRatio), (int)(sourceHeight * dRatio));
            }

            public static byte[] Zoom(byte[] pBuffer, int sourceWidth, int sourceHeight, double dRatioX, double dRatioY)
            {
                return Zoom(pBuffer, sourceWidth, sourceHeight, (int)(sourceWidth * dRatioX), (int)(sourceHeight * dRatioY));
            }

            public static byte[] Zoom(byte[] pBuffer, int sourceWidth, int sourceHeight, int destWidth, int destHeight)
            {
                int i, j, x, y;
                float x1, y1, x2, y2;                               // 좌표
                float ratioX, ratioY;
                byte intensity1, intensity2, intensity3, intensity4; // Gray Level
                float value1, value2, resultLevel;
                ratioX = (float)destWidth / (float)(sourceWidth - 1);
                ratioY = (float)destHeight / (float)(sourceHeight - 1);
                byte[] pDestination = new byte[destWidth * destHeight];
                ////  비율별 확대, 축소
                for (j = 1; j < sourceHeight; j++)
                {
                    for (i = 1; i < sourceWidth; i++)
                    {
                        x1 = (float)(i - 1) * ratioX;
                        y1 = (float)(j - 1) * ratioY;
                        x2 = (float)i * ratioX;
                        y2 = (float)j * ratioY;

                        if (x2 >= destWidth) x2 = destWidth - 1;
                        if (y2 >= destHeight) y2 = destHeight - 1;
                        intensity1 = pBuffer[(j - 1) * sourceWidth + (i - 1)];
                        intensity2 = pBuffer[(j - 1) * sourceWidth + i];
                        intensity3 = pBuffer[j * sourceWidth + (i - 1)];
                        intensity4 = pBuffer[j * sourceWidth + i];
                        for (y = (int)y1; y <= (int)y2; y++)
                        {
                            for (x = (int)x1; x <= (int)x2; x++)
                            {
                                value1 = (x - x2) * intensity1 / (x1 - x2) + (x - x1) * intensity2 / (x2 - x1);
                                value2 = (x - x2) * intensity3 / (x1 - x2) + (x - x1) * intensity4 / (x2 - x1);
                                resultLevel = (y - y2) * value1 / (y1 - y2) + (y - y1) * value2 / (y2 - y1);
                                pDestination[y * destWidth + x] = (byte)resultLevel;
                            }
                        }
                    }
                }
                return pDestination;
            }

            public static int[] Zoom(int[] pBuffer, int sourceWidth, int sourceHeight, double dRatio)
            {
                return Zoom(pBuffer, sourceWidth, sourceHeight, (int)(sourceWidth * dRatio), (int)(sourceHeight * dRatio));
            }

            public static int[] Zoom(int[] pBuffer, int sourceWidth, int sourceHeight, double dRatioX, double dRatioY)
            {
                return Zoom(pBuffer, sourceWidth, sourceHeight, (int)(sourceWidth * dRatioX), (int)(sourceHeight * dRatioY));
            }

            public static int[] Zoom(int[] pBuffer, int sourceWidth, int sourceHeight, int destWidth, int destHeight)
            {
                int i, j, x, y;
                float x1, y1, x2, y2;                               // 좌표
                float ratioX, ratioY;
                int intensity1, intensity2, intensity3, intensity4; // Gray Level
                float value1, value2, resultLevel;
                ratioX = (float)destWidth / (float)(sourceWidth - 1);
                ratioY = (float)destHeight / (float)(sourceHeight - 1);
                int[] pDestination = new int[destWidth * destHeight];
                ////  비율별 확대, 축소
                for (j = 1; j < sourceHeight; j++)
                {
                    for (i = 1; i < sourceWidth; i++)
                    {
                        x1 = (float)(i - 1) * ratioX;
                        y1 = (float)(j - 1) * ratioY;
                        x2 = (float)i * ratioX;
                        y2 = (float)j * ratioY;

                        if (x2 >= destWidth) x2 = destWidth - 1;
                        if (y2 >= destHeight) y2 = destHeight - 1;
                        intensity1 = pBuffer[(j - 1) * sourceWidth + (i - 1)];
                        intensity2 = pBuffer[(j - 1) * sourceWidth + i];
                        intensity3 = pBuffer[j * sourceWidth + (i - 1)];
                        intensity4 = pBuffer[j * sourceWidth + i];
                        for (y = (int)y1; y <= (int)y2; y++)
                        {
                            for (x = (int)x1; x <= (int)x2; x++)
                            {
                                value1 = (x - x2) * intensity1 / (x1 - x2) + (x - x1) * intensity2 / (x2 - x1);
                                value2 = (x - x2) * intensity3 / (x1 - x2) + (x - x1) * intensity4 / (x2 - x1);
                                resultLevel = (y - y2) * value1 / (y1 - y2) + (y - y1) * value2 / (y2 - y1);
                                pDestination[y * destWidth + x] = (int)resultLevel;
                            }
                        }
                    }
                }
                return pDestination;
            }

            //  회전.
            public static YoonImage Rotate(YoonImage pSourceImage, YoonVector2N vecCenter, double dAngle)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(Rotate(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, vecCenter.X, vecCenter.Y, dAngle),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Rotate(byte[] pBuffer, int bufferWidth, int bufferHeight, int centerX, int centerY, double angle)
            {
                int i, j;
                int x1, y1, x2, y2;
                double theta, sinTheta, cosTheta;
                double posX1, posY1, posX2, posY2;
                double level1, level2, level3, level4, tempLevel;
                double roundX, roundY;
                if (Math.Abs(angle) < 0.001)
                    angle = 0.001;
                theta = angle * 3.141592 / 180.0;
                sinTheta = Math.Sin(theta);
                cosTheta = Math.Cos(theta);
                byte[] pResultBuffer = new byte[bufferWidth * bufferHeight];
                ////  회전 알고리즘.
                for (j = 0; j < bufferHeight; j++)
                {
                    posY1 = (double)j - (double)centerY;
                    for (i = 0; i < bufferWidth; i++)
                    {
                        //////  Image 선회전.
                        posX1 = (double)i - (double)centerX;
                        posX2 = (double)centerX + (posX1 * cosTheta - posY1 * sinTheta);
                        posY2 = (double)centerY + (posX1 * sinTheta + posY1 * cosTheta);
                        if (posX2 < 0.0 || posY2 < 0.0)
                            continue;
                        //////  Image 보간.
                        x1 = (int)posX2;  // 정수형으로 변환하여 위치좌표 얻는다.
                        y1 = (int)posY2;
                        x2 = x1 + 1;
                        y2 = y1 + 1;
                        //////  좌표가 한계 범위를 넘은 경우 스킵.
                        if (x1 < 0 || x1 >= bufferWidth || y1 < 0 || y2 >= bufferHeight)
                            continue;
                        roundX = posX2 - x1;
                        roundY = posY2 - y1;
                        //////  보간을 위한 4개 좌표의 Pixel 산출 후 Filtering으로 Gray Level 선정.
                        level1 = pBuffer[y1 * bufferWidth + x1];
                        level2 = pBuffer[y1 * bufferWidth + x2];
                        level3 = pBuffer[y2 * bufferWidth + x1];
                        level4 = pBuffer[y2 * bufferWidth + x2];
                        tempLevel = (1.0 - roundX) * (1.0 - roundY) * level1 + roundX * (1.0 - roundY) * level2 + (1.0 - roundX) * roundY * level3 + roundX * roundY * level4;
                        if (tempLevel < 0) tempLevel = 0;
                        if (tempLevel > 255) tempLevel = 255;
                        pResultBuffer[j * bufferWidth + i] = (byte)tempLevel;
                    }
                }
                return pResultBuffer;
            }

            //  Image 반전.
            public static YoonImage Reverse(YoonImage pSourceImage)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(Reverse(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Reverse(byte[] pBuffer, int bufferWidth, int bufferHeight)
            {
                int i, j;
                byte[] pResultBuffer = new byte[bufferWidth * bufferHeight];
                for (j = 0; j < bufferHeight; j++)
                {
                    for (i = 0; i < bufferWidth; i++)
                    {
                        pResultBuffer[j * bufferWidth + i] = (byte)(255 - pBuffer[j * bufferWidth + i]);
                    }
                }
                return pResultBuffer;
            }

            //  직사각형 Image를 평행사변형꼴로 기울인다.
            public static YoonImage Warp(YoonImage pSourceImage, YoonRectAffine2D pRect)
            {
                if (pSourceImage.Format != PixelFormat.Format8bppIndexed)
                    throw new FormatException("[YOONIMAGE EXCEPTION] Image format is not correct");
                return new YoonImage(Warp(pSourceImage.GetGrayBuffer(), pSourceImage.Width, pSourceImage.Height, pRect),
                    pSourceImage.Width, pSourceImage.Height, PixelFormat.Format8bppIndexed);
            }

            public static byte[] Warp(byte[] pSourceBuffer, int sourceWidth, int sourceHeight, YoonRectAffine2D pRect)
            {
                int iX, iY;
                int x1, y1, x2, y2;
                double dx, dy, roundX, roundY;
                double level1, level2, level3, level4, tempLevel;
                double leftTopToLeftBottom_X, leftTopToLeftBottom_Y;
                double rightTopToRightBottom_x, rightTopToRightBottom_y;
                double leftBottomToRightBottom_x, leftBottomToRightBottom_y;
                double newLeftBottomX, newLeftBottomY, newRightBottomX, newRightBottomY;
                int bufferSize = (int)(pRect.Width * pRect.Height);
                byte[] pResultBuffer = new byte[bufferSize];
                leftTopToLeftBottom_X = pRect.TopLeft.X - pRect.BottomLeft.X;
                leftTopToLeftBottom_Y = pRect.TopLeft.Y - pRect.BottomLeft.Y;
                rightTopToRightBottom_x = pRect.TopRight.X - pRect.BottomRight.X;
                rightTopToRightBottom_y = pRect.TopRight.Y - pRect.BottomRight.Y;
                ////  보간 작업을 실시한다.
                for (iY = 0; iY < pRect.Height; iY++)
                {
                    newLeftBottomX = pRect.BottomLeft.X + (double)(pRect.Height - iY) * leftTopToLeftBottom_X / pRect.Height;
                    newLeftBottomY = pRect.BottomLeft.Y + (double)(pRect.Height - iY) * leftTopToLeftBottom_Y / pRect.Height;
                    newRightBottomX = pRect.BottomRight.X + (double)(pRect.Height - iY) * rightTopToRightBottom_x / pRect.Height;
                    newRightBottomY = pRect.BottomRight.Y + (double)(pRect.Height - iY) * rightTopToRightBottom_y / pRect.Height;
                    leftBottomToRightBottom_x = newRightBottomX - newLeftBottomX;
                    leftBottomToRightBottom_y = newRightBottomY - newLeftBottomY;
                    for (iX = 0; iX < pRect.Width; iX++)
                    {
                        dx = newLeftBottomX + iX * leftBottomToRightBottom_x / pRect.Width;
                        dy = newLeftBottomY + iX * leftBottomToRightBottom_y / pRect.Width;
                        x1 = (int)dx;
                        y1 = (int)dy;
                        x2 = x1 + 1;
                        y2 = y1 + 1;
                        if (x2 >= sourceWidth) x2 = sourceWidth - 1;
                        if (y2 >= sourceHeight) y2 = sourceHeight - 1;
                        roundX = dx - x1;
                        roundY = dy - y1;
                        level1 = pSourceBuffer[y1 * sourceWidth + x1];
                        level2 = pSourceBuffer[y1 * sourceWidth + x2];
                        level3 = pSourceBuffer[y2 * sourceWidth + x1];
                        level4 = pSourceBuffer[y2 * sourceWidth + x2];
                        tempLevel = (1.0 - roundX) * (1.0 - roundY) * level1 + roundX * (1.0 - roundY) * level2 + (1.0 - roundX) * roundY * level3 + roundX * roundY * level4;
                        //////  Level이 지나칠 경우 0 또는 255로 정의.
                        if (tempLevel < 0) tempLevel = 0;
                        if (tempLevel > 255) tempLevel = 255;
                        pResultBuffer[iY * (int)pRect.Width + iX] = (byte)tempLevel;
                    }
                }
                return pResultBuffer;
            }
        }
    }
}