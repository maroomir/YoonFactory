using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using YoonFactory;
using YoonFactory.Image;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace YoonFactory.CV
{
    public class CVImage : YoonImage
    {
        public CVImage(Mat pMat)
        {
            m_pBitmap = BitmapConverter.ToBitmap(pMat);
        }

        public Mat ToMatImage()
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
            using (Mat pMat = ToMatImage())
            {
                pMat.SaveImage(strPath);
                return true;
            }
        }

        public Mat CopyMat()
        {
            return ToMatImage().Clone();
        }

        public override YoonImage CropImage(YoonRect2N cropArea)
        {
            Mat pMat = ToMatImage().SubMat(cropArea.ToCVRect());
            return new CVImage(pMat);
        }

        public void ShowImage(string strTitle)
        {
            Cv2.ImShow(strTitle, ToMatImage());
        }
    }
}
