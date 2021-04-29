using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace YoonFactory.CV
{
    public static class CVFactory
    {
        public static IYoonObject FindTemplate(this CVImage pSourceImage, CVImage pTemplateImage, double dScore = 0.7) => TemplateMatch.FindTemplate(pTemplateImage, pSourceImage, dScore);
        public static CVImage Add(this CVImage pSourceImage, CVImage pObjectImage) => TwoImageProcess.Add(pSourceImage, pObjectImage);
        public static CVImage Subtract(this CVImage pSourceImage, CVImage pObjectImage) => TwoImageProcess.Subtract(pSourceImage, pObjectImage);
        public static CVImage Multiply(this CVImage pSourceImage, CVImage pObjectImage) => TwoImageProcess.Multiply(pSourceImage, pObjectImage);
        public static CVImage Divide(this CVImage pSourceImage, CVImage pObjectImage) => TwoImageProcess.Divide(pSourceImage, pObjectImage);
        public static CVImage Max(this CVImage pSourceImage, CVImage pObjectImage) => TwoImageProcess.Max(pSourceImage, pObjectImage);
        public static CVImage Min(this CVImage pSourceImage, CVImage pObjectImage) => TwoImageProcess.Min(pSourceImage, pObjectImage);
        public static CVImage AbsDiff(this CVImage pSourceImage, CVImage pObjectImage) => TwoImageProcess.AbsDiff(pSourceImage, pObjectImage);
        public static CVImage Sobel(this CVImage pSourceImage, int nOrderX = 1, int nOrderY = 0) => Filter.Sobel(pSourceImage, nOrderX, nOrderY);
        public static CVImage Scharr(this CVImage pSourceImage, int nOrderX = 1, int nOrderY = 0) => Filter.Scharr(pSourceImage, nOrderX, nOrderY);
        public static CVImage Laplacian(this CVImage pSourceImage) => Filter.Laplacian(pSourceImage);
        public static CVImage Gaussian(this CVImage pSourceImage, int nSizeX = 3, int nSizeY = 3) => Filter.Gaussian(pSourceImage, nSizeX, nSizeY);
        public static CVImage Canny(this CVImage pSourceImage, double dThresholdMin = 100, double dThresholdMax = 200) => Filter.Canny(pSourceImage, dThresholdMin, dThresholdMax);
        public static CVImage FillFlood(this CVImage pSourceImage, YoonVector2N pVector, byte nThreshold, bool bWhite) => Fill.FillFlood(pSourceImage, pVector, nThreshold, bWhite);
        public static CVImage FillFlood(this CVImage pSourceImage, YoonVector2N pVector, byte nThreshold, Color pFillColor) => Fill.FillFlood(pSourceImage, pVector, nThreshold, pFillColor);

        public static class Converter
        {
            public static Mat ToGrayMatrix(IntPtr pBufferAddress, int nWidth, int nHeight)
            {
                if (pBufferAddress == IntPtr.Zero) return null;
                byte[] pBuffer = new byte[nWidth * nHeight];
                Marshal.Copy(pBufferAddress, pBuffer, 0, nWidth * nHeight);
                return Mat.FromImageData(pBuffer, ImreadModes.Grayscale);
            }

            public static Mat ToGrayMatrix(byte[] pBuffer, int nWidth, int nHeight)
            {
                if (pBuffer == null || pBuffer.Length != nWidth * nHeight) return null;
                return Mat.FromImageData(pBuffer, ImreadModes.Grayscale);
            }

            public static Mat ToColorMatrix(IntPtr pBufferAddress, int nWidth, int nHeight)
            {
                if (pBufferAddress == IntPtr.Zero) return null;
                byte[] pBuffer = new byte[nWidth * nHeight * 3];
                Marshal.Copy(pBufferAddress, pBuffer, 0, nWidth * nHeight);
                return Mat.FromImageData(pBuffer, ImreadModes.Color);
            }

            public static Mat ToColorMatrix(byte[] pBuffer, int nWidth, int nHeight)
            {
                if (pBuffer == null || pBuffer.Length != nWidth * nHeight * 3) return null;
                return Mat.FromImageData(pBuffer, ImreadModes.Color);
            }
        }

        public static class TemplateMatch
        {
            public static IYoonObject FindTemplate(CVImage pTemplateImage, CVImage pSourceImage, double dScore = 0.7)
            {
                double dMatchScore;
                Rect pRectResult = FindTemplate(pTemplateImage.ToMatrix(), pSourceImage.ToMatrix(), out dMatchScore, dScore, TemplateMatchModes.CCoeffNormed);
                return new YoonObject<YoonRect2N>(0, pRectResult.ToYoonRect(), pTemplateImage.CropImage(pRectResult.ToYoonRect()), dMatchScore, (int)(dMatchScore * pRectResult.Width * pRectResult.Height));
            }

            public static Rect FindTemplate(Mat pTemplateMatrix, Mat pSourceMatrix, out double dMatchScore, double dThresholdScore, TemplateMatchModes nMode = TemplateMatchModes.CCoeffNormed)
            {
                Mat pResultImage = new Mat();
                double dMinVal, dMaxVal;
                OpenCvSharp.Point pMinPos, pMaxPos;
                Cv2.MatchTemplate(pSourceMatrix, pTemplateMatrix, pResultImage, nMode);
                Cv2.MinMaxLoc(pResultImage, out dMinVal, out dMaxVal, out pMinPos, out pMaxPos);
                if (dMaxVal > dThresholdScore)
                {
                    dMatchScore = dMaxVal;
                    return new Rect(pMaxPos, pTemplateMatrix.Size());
                }
                else
                    throw new InvalidOperationException("[YOONCV EXCEPTION] Process Failed : Matching score is too low");
            }
        }

        public static class TwoImageProcess
        {
            public static CVImage Add(CVImage pSourceImage, CVImage pObjectImage)
            {
                if (pSourceImage.Width != pObjectImage.Width || pSourceImage.Height != pObjectImage.Height)
                    throw new ArgumentOutOfRangeException("[YOONCV EXCEPTION] Image size is not same");
                return new CVImage(Add(pSourceImage.ToMatrix(), pObjectImage.ToMatrix()));
            }

            public static Mat Add(Mat pSourceMatrix, Mat pObjectMatrix)
            {
                Mat pResultMatrix = new Mat();
                Cv2.Add(pSourceMatrix, pObjectMatrix, pResultMatrix);
                return pResultMatrix;
            }

            public static CVImage Subtract(CVImage pSourceImage, CVImage pObjectImage)
            {
                if (pSourceImage.Width != pObjectImage.Width || pSourceImage.Height != pObjectImage.Height)
                    throw new ArgumentOutOfRangeException("[YOONCV EXCEPTION] Image size is not same");
                return new CVImage(Subtract(pSourceImage.ToMatrix(), pObjectImage.ToMatrix()));
            }

            public static Mat Subtract(Mat pSourceMatrix, Mat pObjectMatrix)
            {
                Mat pResultMatrix = new Mat();
                Cv2.Subtract(pSourceMatrix, pObjectMatrix, pResultMatrix);
                return pResultMatrix;
            }

            public static CVImage Multiply(CVImage pSourceImage, CVImage pObjectImage)
            {
                if (pSourceImage.Width != pObjectImage.Width || pSourceImage.Height != pObjectImage.Height)
                    throw new ArgumentOutOfRangeException("[YOONCV EXCEPTION] Image size is not same");
                return new CVImage(Multiply(pSourceImage.ToMatrix(), pObjectImage.ToMatrix()));
            }

            public static Mat Multiply(Mat pSourceMatrix, Mat pObjectMatrix)
            {
                Mat pResultMatrix = new Mat();
                Cv2.Multiply(pSourceMatrix, pObjectMatrix, pResultMatrix);
                return pResultMatrix;
            }

            public static CVImage Divide(CVImage pSourceImage, CVImage pObjectImage)
            {
                if (pSourceImage.Width != pObjectImage.Width || pSourceImage.Height != pObjectImage.Height)
                    throw new ArgumentOutOfRangeException("[YOONCV EXCEPTION] Image size is not same");
                return new CVImage(Divide(pSourceImage.ToMatrix(), pObjectImage.ToMatrix()));
            }

            public static Mat Divide(Mat pSourceMatrix, Mat pObjectMatrix)
            {
                Mat pResultMatrix = new Mat();
                Cv2.Divide(pSourceMatrix, pObjectMatrix, pResultMatrix);
                return pResultMatrix;
            }

            public static CVImage Max(CVImage pSourceImage, CVImage pObjectImage)
            {
                if (pSourceImage.Width != pObjectImage.Width || pSourceImage.Height != pObjectImage.Height)
                    throw new ArgumentOutOfRangeException("[YOONCV EXCEPTION] Image size is not same");
                return new CVImage(Max(pSourceImage.ToMatrix(), pObjectImage.ToMatrix()));
            }

            public static Mat Max(Mat pSourceMatrix, Mat pObjectMatrix)
            {
                Mat pResultMatrix = new Mat();
                Cv2.Max(pSourceMatrix, pObjectMatrix, pResultMatrix);
                return pResultMatrix;
            }

            public static CVImage Min(CVImage pSourceImage, CVImage pObjectImage)
            {
                if (pSourceImage.Width != pObjectImage.Width || pSourceImage.Height != pObjectImage.Height)
                    throw new ArgumentOutOfRangeException("[YOONCV EXCEPTION] Image size is not same");
                return new CVImage(Min(pSourceImage.ToMatrix(), pObjectImage.ToMatrix()));
            }

            public static Mat Min(Mat pSourceMatrix, Mat pObjectMatrix)
            {
                Mat pResultMatrix = new Mat();
                Cv2.Max(pSourceMatrix, pObjectMatrix, pResultMatrix);
                return pResultMatrix;
            }

            public static CVImage AbsDiff(CVImage pSourceImage, CVImage pObjectImage)
            {
                if (pSourceImage.Width != pObjectImage.Width || pSourceImage.Height != pObjectImage.Height)
                    throw new ArgumentOutOfRangeException("[YOONCV EXCEPTION] Image size is not same");
                return new CVImage(AbsDiff(pSourceImage.ToMatrix(), pObjectImage.ToMatrix()));
            }

            public static Mat AbsDiff(Mat pSourceMatrix, Mat pObjectMatrix)
            {
                Mat pResultMatrix = new Mat();
                Cv2.Absdiff(pSourceMatrix, pObjectMatrix, pResultMatrix);
                return pResultMatrix;
            }
        }

        public static class Filter
        {
            public static CVImage Sobel(CVImage pSourceImage, int nOrderX = 1, int nOrderY = 0)
            {
                return new CVImage(Sobel(pSourceImage.ToMatrix(), nOrderX, nOrderY));
            }

            public static Mat Sobel(Mat pSourceMatrix, int nOrderX, int nOrderY)
            {
                Mat pResultMatrix = new Mat();
                Cv2.Sobel(pSourceMatrix, pResultMatrix, MatType.CV_32F, nOrderX, nOrderY);
                pResultMatrix.ConvertTo(pResultMatrix, MatType.CV_8UC1); // Downscale
                return pResultMatrix;
            }

            public static CVImage Scharr(CVImage pSourceImage, int nOrderX = 1, int nOrderY = 0)
            {
                return new CVImage(Scharr(pSourceImage.ToMatrix(), nOrderX, nOrderY));
            }

            public static Mat Scharr(Mat pSourceMatrix, int nOrderX, int nOrderY)
            {
                Mat pResultMatrix = new Mat();
                Cv2.Scharr(pSourceMatrix, pResultMatrix, MatType.CV_32F, nOrderX, nOrderY);
                pResultMatrix.ConvertTo(pResultMatrix, MatType.CV_8UC1); // Downscale
                return pResultMatrix;
            }

            public static CVImage Laplacian(CVImage pSourceImage)
            {
                return new CVImage(Laplacian(pSourceImage.ToMatrix()));
            }

            public static Mat Laplacian(Mat pSourceMatrix)
            {
                Mat pResultMatrix = new Mat();
                Cv2.Laplacian(pSourceMatrix, pResultMatrix, MatType.CV_32F);
                pResultMatrix.ConvertTo(pResultMatrix, MatType.CV_8UC1); // Downscale
                return pResultMatrix;
            }

            public static CVImage Gaussian(CVImage pSourceImage, int nSizeX = 3, int nSizeY = 3)
            {
                return new CVImage(Gaussian(pSourceImage.ToMatrix(), nSizeX, nSizeY));
            }

            public static Mat Gaussian(Mat pSourceMatrix, int nSizeX, int nSizeY)
            {
                Mat pResultMatrix = new Mat();
                Cv2.GaussianBlur(pSourceMatrix, pResultMatrix, new OpenCvSharp.Size(nSizeX, nSizeY), 1);
                return pResultMatrix;
            }

            public static CVImage Canny(CVImage pSourceImage, double dThresholdMin = 100, double dThresholdMax = 200)
            {
                return new CVImage(Canny(pSourceImage.ToMatrix(), dThresholdMin, dThresholdMax));
            }

            public static Mat Canny(Mat pSourceMatrix, double dThresholdMin, double dThresholdMax)
            {
                Mat pResultMatrix = new Mat();
                Cv2.Canny(pSourceMatrix, pResultMatrix, dThresholdMin, dThresholdMax);
                return pResultMatrix;
            }
        }

        public static class Fill
        {
            public static CVImage FillFlood(CVImage pSourceImage, YoonVector2N pVector, byte nThreshold, bool bWhite)
            {
                return new CVImage(FillFlood(pSourceImage.ToMatrix(), pVector, nThreshold, bWhite));
            }

            public static CVImage FillFlood(CVImage pSourceImage, YoonVector2N pVector, byte nThreshold, Color pFillColor)
            {
                return new CVImage(FillFlood(pSourceImage.ToMatrix(), pVector, nThreshold, pFillColor));
            }

            public static Mat FillFlood(Mat pSourceMatrix, YoonVector2N pVector, byte nThreshold, bool isWhite)
            {
                Rect pFillRect;
                Mat pResultMatrix = pSourceMatrix.Clone();
                if (isWhite)
                    Cv2.FloodFill(pResultMatrix, pVector.ToCVPoint(), Scalar.All(255), out pFillRect, Scalar.All(nThreshold), Scalar.All(nThreshold), FloodFillFlags.Link8);
                else
                    Cv2.FloodFill(pResultMatrix, pVector.ToCVPoint(), Scalar.All(0), out pFillRect, Scalar.All(nThreshold), Scalar.All(nThreshold), FloodFillFlags.Link8);
                return pResultMatrix;
            }

            public static Mat FillFlood(Mat pSourceMatrix, YoonVector2N pVector, byte nThreshold, Color pFillColor)
            {
                Rect pFillRect;
                Mat pResultMatrix = pSourceMatrix.Clone();
                Cv2.FloodFill(pResultMatrix, pVector.ToCVPoint(), pFillColor.ToScalar(), out pFillRect, Scalar.All(nThreshold), Scalar.All(nThreshold), FloodFillFlags.Link8);
                return pResultMatrix;
            }
        }
    }
}
