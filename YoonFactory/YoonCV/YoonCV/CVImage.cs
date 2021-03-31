using YoonFactory.Image;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace YoonFactory.CV
{
    public class CVImage : YoonImage
    {
        public CVImage(Mat pMatrix)
        {
            m_pBitmap = BitmapConverter.ToBitmap(pMatrix);
        }

        public Mat ToMatrix()
        {
            return BitmapConverter.ToMat(m_pBitmap);
        }

        public override bool LoadImage(string strPath)
        {
            if (IsFileExist()) return false;
            Mat pMat = new Mat(strPath);
            m_pBitmap = BitmapConverter.ToBitmap(pMat);
            return true;
        }

        public override bool SaveImage(string strPath)
        {
            using (Mat pMat = ToMatrix())
            {
                pMat.SaveImage(strPath);
                return true;
            }
        }

        public Mat CopyMatrix()
        {
            return ToMatrix().Clone();
        }

        public override YoonImage CropImage(YoonRect2N cropArea)
        {
            Mat pMat = ToMatrix().SubMat(cropArea.ToCVRect());
            return new CVImage(pMat);
        }

        public void ShowImage(string strTitle)
        {
            Cv2.ImShow(strTitle, ToMatrix());
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
