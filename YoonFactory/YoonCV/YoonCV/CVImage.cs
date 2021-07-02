using YoonFactory.Image;
using System;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using YoonFactory.Files;

namespace YoonFactory.CV
{
    public class CVImage : YoonImage
    {
        public CVImage() : base()
        {
            //
        }

        public CVImage(Bitmap pBitmap) : base(pBitmap)
        {
            //
        }

        public CVImage(Mat pMatrix) : this()
        {
            if (pMatrix.Width != 0 && pMatrix.Height != 0)
                m_pBitmap = BitmapConverter.ToBitmap(pMatrix);
        }

        public Mat ToMatrix()
        {
            return BitmapConverter.ToMat(m_pBitmap);
        }

        public override bool LoadImage(string strPath)
        {
            FilePath = strPath;
            if (!IsFileExist()) return false;
            Mat pMat = new Mat(FilePath);
            m_pBitmap = BitmapConverter.ToBitmap(pMat);
            return true;
        }

        public override bool SaveImage(string strPath)
        {
            FilePath = strPath;
            using (Mat pMat = ToMatrix())
            {
                pMat.SaveImage(FilePath);
                return true;
            }
        }

        public Mat CopyMatrix()
        {
            return ToMatrix().Clone();
        }

        public override IYoonFile Clone()
        {
            CVImage pImage = new CVImage(m_pBitmap);
            pImage.FilePath = FilePath;
            return pImage;
        }

        public override YoonImage CropImage(YoonRect2N cropArea)
        {
            Mat pMat = ToMatrix().SubMat(cropArea.ToCVRect());
            return new CVImage(pMat);
        }

        public void ShowImage(string strTitle)
        {
            Cv2.NamedWindow(strTitle, WindowMode.AutoSize);
            Cv2.SetWindowProperty(strTitle, WindowProperty.Fullscreen, 0);
            Cv2.ImShow(strTitle, ToMatrix());
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        public void ShowHistogram(string strTitle, int nChannel)  // B : 0,  G : 1,  R : 2
        {
            if (Plane != 3)
                throw new FormatException("[YOONIMAGE ERROR] Bitmap format is not comportable");

            Mat pHistogramMatrix = new Mat();
            Mat pResultMatrix = Mat.Ones(new OpenCvSharp.Size(256, Height), MatType.CV_8UC1);
            Cv2.CalcHist(new Mat[] { ToMatrix() }, new int[] { nChannel }, null, pHistogramMatrix, 1, new int[] { 256 },
                new Rangef[] { new Rangef(0, 256) });
            Cv2.Normalize(pHistogramMatrix, pHistogramMatrix, 0, 255, NormTypes.MinMax);
            for (int i = 0; i < pHistogramMatrix.Rows; i++)
            {
                Cv2.Line(pResultMatrix, new OpenCvSharp.Point(i, ToMatrix().Height), new OpenCvSharp.Point(i, ToMatrix().Height - pHistogramMatrix.Get<float>(i)),
                    Scalar.White);
            }
            Cv2.ImShow(strTitle, pResultMatrix);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        public void ShowHistogram(string strTitle)
        {
            Mat pMatHistogram = new Mat();
            Mat pMatResult = Mat.Ones(new OpenCvSharp.Size(256, Height), MatType.CV_8UC1);
            Mat pMatSource = (ToGrayImage() as CVImage).ToMatrix();
            Cv2.CalcHist(new Mat[] { pMatSource }, new int[] { 0 }, null, pMatHistogram, 1, new int[] { 256 },
                new Rangef[] { new Rangef(0, 256) });
            Cv2.Normalize(pMatHistogram, pMatHistogram, 0, 255, NormTypes.MinMax);
            for (int i = 0; i < pMatHistogram.Rows; i++)
            {
                Cv2.Line(pMatResult, new OpenCvSharp.Point(i, pMatSource.Height), new OpenCvSharp.Point(i, pMatSource.Height - pMatHistogram.Get<float>(i)),
                    Scalar.White);
            }
            Cv2.ImShow(strTitle, pMatResult);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        public static CVImage operator +(CVImage i1, CVImage i2)
        {
            return CVFactory.TwoImageProcess.Add(i1, i2);
        }

        public static CVImage operator -(CVImage i1, CVImage i2)
        {
            return CVFactory.TwoImageProcess.Subtract(i1, i2);
        }

        public static CVImage operator *(CVImage i1, CVImage i2)
        {
            return CVFactory.TwoImageProcess.Multiply(i1, i2);
        }

        public static CVImage operator /(CVImage i1, CVImage i2)
        {
            return CVFactory.TwoImageProcess.Divide(i1, i2);
        }
    }
}
