using System;
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
        public static class Converter
        {
            public static Mat ToMatGrey(IntPtr pBufferAddress, int nWidth, int nHeight)
            {
                if (pBufferAddress == IntPtr.Zero) return null;
                byte[] pBuffer = new byte[nWidth * nHeight];
                Marshal.Copy(pBufferAddress, pBuffer, 0, nWidth * nHeight);
                return Mat.FromImageData(pBuffer, ImreadModes.Grayscale);
            }

            public static Mat ToMatGrey(byte[] pBuffer, int nWidth, int nHeight)
            {
                if (pBuffer == null || pBuffer.Length != nWidth * nHeight) return null;
                return Mat.FromImageData(pBuffer, ImreadModes.Grayscale);
            }

            public static Mat ToMatColor(IntPtr pBufferAddress, int nWidth, int nHeight)
            {
                if (pBufferAddress == IntPtr.Zero) return null;
                byte[] pBuffer = new byte[nWidth * nHeight * 3];
                Marshal.Copy(pBufferAddress, pBuffer, 0, nWidth * nHeight);
                return Mat.FromImageData(pBuffer, ImreadModes.Color);
            }

            public static Mat ToMatColor(byte[] pBuffer, int nWidth, int nHeight)
            {
                if (pBuffer == null || pBuffer.Length != nWidth * nHeight * 3) return null;
                return Mat.FromImageData(pBuffer, ImreadModes.Color);
            }
        }

        public static class TemplateMatch
        {
            public static IYoonObject FindTemplate(CVImage pTemplateImage, CVImage pSourceImage, double dScore = 0.7)
            {
                return FindTemplate(pTemplateImage.ToMatImage(), pSourceImage.ToMatImage(), dScore, TemplateMatchModes.CCoeffNormed);
            }

            public static IYoonObject FindTemplate(Mat pTemplateImage, Mat pSourceImage, double dScore, TemplateMatchModes nMode = TemplateMatchModes.CCoeffNormed)
            {
                Mat pResultImage = new Mat();
                double dMinVal, dMaxVal;
                Point pMinPos, pMaxPos;
                Cv2.MatchTemplate(pSourceImage, pTemplateImage, pResultImage, nMode);
                Cv2.MinMaxLoc(pResultImage, out dMinVal, out dMaxVal, out pMinPos, out pMaxPos);
                if (dMaxVal > dScore)
                {
                    Rect pRectResult = new Rect(pMaxPos, pTemplateImage.Size());
                    return new YoonObject<YoonRect2N>(0, pRectResult.ToYoonRect(), dMaxVal, (int)(dMaxVal * pRectResult.Width * pRectResult.Height));
                }
                else
                    throw new InvalidOperationException("[YOONCV EXCEPTION] Process Failed : Matching score is too low");
            }
        }

        public static class TwoImageProcess
        {

        }
    }
}
