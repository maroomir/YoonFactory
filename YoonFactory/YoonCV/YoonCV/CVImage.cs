using YoonFactory.Image;
using System;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using YoonFactory.Files;

namespace YoonFactory.CV
{
    public class CVImage : YoonImage
    {
        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    base.Bitmap.Dispose();
                    Matrix.Dispose();
                }
                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                base.Bitmap = null;
                Matrix = null;

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        ~CVImage() {
           // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
           Dispose(false);
        }
        #endregion

        public Mat Matrix { get; private set; } = null;

        public CVImage() : base()
        {
            Matrix = BitmapConverter.ToMat(base.Bitmap);
        }

        public CVImage(Mat pMatrix) : this()
        {
            Task pTask = Task.Factory.StartNew(() =>base.Bitmap = BitmapConverter.ToBitmap(pMatrix));
            Matrix = pMatrix.Clone();
            pTask.Wait();
        }

        public CVImage(YoonImage pImage)
        {
            Task pTask = Task.Factory.StartNew(() => Matrix = BitmapConverter.ToMat(pImage.Bitmap));
            base.Bitmap = pImage.CopyBitmap();
            pTask.Wait();
        }

        public override bool LoadImage(string strPath)
        {
            FilePath = strPath;
            if (!IsFileExist()) return false;
            Matrix = Cv2.ImRead(strPath);
            base.Bitmap = BitmapConverter.ToBitmap(Matrix);
            return true;
        }

        public bool LoadImage(string strPath, ImreadModes nMode)
        {
            FilePath = strPath;
            if (!IsFileExist()) return false;
            Matrix = Cv2.ImRead(strPath, nMode);
            base.Bitmap = BitmapConverter.ToBitmap(Matrix);
            return true;
        }

        public override bool SaveImage(string strPath)
        {
            FilePath = strPath;
            return Matrix.SaveImage(FilePath);
        }

        public Mat CopyMatrix()
        {
            return Matrix.Clone();
        }

        public override IYoonFile Clone()
        {
            CVImage pImage = new CVImage(Matrix);
            pImage.FilePath = FilePath;
            return pImage;
        }

        public override YoonImage ToGrayImage()
        {
            return new CVImage(Matrix.CvtColor(ColorConversionCodes.BGR2GRAY));
        }

        public override YoonImage ToRGBImage()
        {
            return new CVImage(Matrix.CvtColor(ColorConversionCodes.GRAY2BGR));
        }

        public override YoonImage CropImage(YoonRect2N cropArea)
        {
            return new CVImage(Matrix.SubMat(cropArea.ToCVRect()));
        }

        public void ShowImage(string strTitle)
        {
            Cv2.NamedWindow(strTitle, WindowMode.AutoSize);
            Cv2.SetWindowProperty(strTitle, WindowProperty.Fullscreen, 0);
            Cv2.ImShow(strTitle, Matrix);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        public void ShowHistogram(string strTitle, int nChannel)  // B : 0,  G : 1,  R : 2
        {
            if (Plane != 3)
                throw new FormatException("[YOONIMAGE ERROR] Bitmap format is not comportable");

            Mat pHistogramMatrix = new Mat();
            Mat pResultMatrix = Mat.Ones(new Size(256, Height), MatType.CV_8UC1);
            Cv2.CalcHist(new Mat[] { Matrix }, new int[] { nChannel }, null, pHistogramMatrix, 1, new int[] { 256 },
                new Rangef[] { new Rangef(0, 256) });
            Cv2.Normalize(pHistogramMatrix, pHistogramMatrix, 0, 255, NormTypes.MinMax);
            for (int i = 0; i < pHistogramMatrix.Rows; i++)
            {
                Cv2.Line(pResultMatrix, new Point(i, Matrix.Height), new Point(i, Matrix.Height - pHistogramMatrix.Get<float>(i)),
                    Scalar.White);
            }
            Cv2.ImShow(strTitle, pResultMatrix);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        public void ShowHistogram(string strTitle)
        {
            Mat pMatHistogram = new Mat();
            Mat pMatResult = Mat.Ones(new Size(256, Height), MatType.CV_8UC1);
            Mat pMatSource = (ToGrayImage() as CVImage).Matrix;
            Cv2.CalcHist(new Mat[] { pMatSource }, new int[] { 0 }, null, pMatHistogram, 1, new int[] { 256 },
                new Rangef[] { new Rangef(0, 256) });
            Cv2.Normalize(pMatHistogram, pMatHistogram, 0, 255, NormTypes.MinMax);
            for (int i = 0; i < pMatHistogram.Rows; i++)
            {
                Cv2.Line(pMatResult, new Point(i, pMatSource.Height), new Point(i, pMatSource.Height - pMatHistogram.Get<float>(i)),
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
