using System;
using System.Threading.Tasks;
using YoonFactory.Image;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using System.Drawing;
using YoonFactory.Files;

namespace YoonFactory.Cognex
{
    public class CognexImage : YoonImage
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
                    switch (CogImage)
                    {
                        case CogImage8Grey pImage8Bit:
                            pImage8Bit.Dispose();
                            break;
                        case CogImage24PlanarColor pImage24Bit:
                            pImage24Bit.Dispose();
                            break;
                        default:
                            break;
                    }
                }
                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                base.Bitmap = null;
                CogImage = null;

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        ~CognexImage()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(false);
        }
        #endregion

        public static ICogImage ToCogImage(Bitmap pBitmap)
        {
            switch (pBitmap?.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    CogImage8Grey pImage8bit = new CogImage8Grey();
                    pImage8bit.Allocate(pBitmap.Width, pBitmap.Height);
                    for(int iY = 0; iY < pBitmap.Height; iY++)
                    {
                        for (int iX = 0; iX < pBitmap.Width; iX++)
                        {
                            Color pColor = pBitmap.GetPixel(iX, iY);
                            byte nLevel = (byte)(pColor.R * 0.299f + pColor.G * 0.587f + pColor.B * 0.114f); // ITU-RBT.709, YPrPb
                            pImage8bit.SetPixel(iX, iY, nLevel);
                        }
                    }
                    return pImage8bit.CopyBase(CogImageCopyModeConstants.CopyPixels);
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    CogImage24PlanarColor pImage24bit = new CogImage24PlanarColor();
                    pImage24bit.Allocate(pBitmap.Width, pBitmap.Height);
                    for (int iY = 0; iY < pBitmap.Height; iY++)
                    {
                        for (int iX = 0; iX < pBitmap.Width; iX++)
                        {
                            Color pColor = pBitmap.GetPixel(iX, iY);
                            pImage24bit.SetPixel(iX, iY, pColor.R, pColor.G, pColor.B);
                        }
                    }
                    return pImage24bit.CopyBase(CogImageCopyModeConstants.CopyPixels);
                default:
                    System.Diagnostics.Debug.WriteLine("[CognexImage] Unsupported Image Format");
                    return null;
            }
        }

        public static Bitmap ToBitmap(ICogImage pImage)
        {
            if (pImage?.Width == 0 || pImage?.Height == 0)
                return null;
            YoonImage pResultImage = new YoonImage(pImage?.ToBitmap());
            if (pImage is CogImage8Grey)
                pResultImage = pResultImage.ToGrayImage();
            return pResultImage.CopyBitmap();
        }

        public ICogImage CogImage { get; private set; } = null;

        public CognexImage() : base()
        {
            CogImage = ToCogImage(base.Bitmap);
        }

        public CognexImage(ICogImage pImage) : this()
        {
            Task pTask = Task.Factory.StartNew(() => base.Bitmap = ToBitmap(pImage));
            CogImage = pImage.CopyBase(CogImageCopyModeConstants.CopyPixels);
            pTask.Wait();
        }

        public CognexImage(YoonImage pImage)
        {
            Task pTask = Task.Factory.StartNew(() => CogImage = ToCogImage(pImage.Bitmap));
            base.Bitmap = pImage.CopyBitmap();
            pTask.Wait();
        }

        public override bool LoadImage(string strPath)
        {
            FilePath = strPath;
            if (!IsFileExist()) return false;
            CogImageFile pCogFile = new CogImageFile();
            try
            {
                pCogFile.Open(FilePath, CogImageFileModeConstants.Read);
                if (pCogFile[0] != null)
                {
                    CogImage = pCogFile[0].CopyBase(CogImageCopyModeConstants.CopyPixels);
                    base.Bitmap = ToBitmap(CogImage);
                }
                pCogFile.Close();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                pCogFile.Close();
            }
            return false;
        }

        public override bool SaveImage(string strPath)
        {
            FilePath = strPath;
            CogImageFile pCogFile = new CogImageFile();
            try
            {
                if (IsFileExist())
                {
                    pCogFile.Open(FilePath, CogImageFileModeConstants.Update);
                    pCogFile.Append(CogImage);
                    pCogFile.Close();
                }
                else
                {
                    if (FileFactory.VerifyDirectory(FilePath))
                    {
                        pCogFile.Open(FilePath, CogImageFileModeConstants.Write);
                        pCogFile.Append(CogImage);
                        pCogFile.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                pCogFile.Close();
            }
            return false;
        }

        public ICogImage CopyCogImage()
        {
            return CogImage.CopyBase(CogImageCopyModeConstants.CopyPixels);
        }

        public override IYoonFile Clone()
        {
            CognexImage pImage = new CognexImage(CogImage);
            pImage.FilePath = FilePath;
            return pImage;
        }

        public override YoonImage ToGrayImage()
        {
            if (CogImage is CogImage8Grey)
                return Clone() as YoonImage;
            CogImage8Grey pImage = CogImageConvert.GetIntensityImage(CogImage, 0, 0, Width, Height);
            return new CognexImage(pImage);
        }

        public override YoonImage ToRGBImage()
        {
            if (CogImage is CogImage24PlanarColor)
                return Clone() as YoonImage;
            CogImage24PlanarColor pImage = CogImageConvert.GetRGBImage(CogImage, 0, 0, Width, Height) as CogImage24PlanarColor;
            return new CognexImage(pImage);
        }

        public CognexImage ToHSIImage()
        {
            CogImage24PlanarColor pImage = CogImageConvert.GetHSIImage(CogImage, 0, 0, Width, Height) as CogImage24PlanarColor;
            return new CognexImage(pImage);
        }

        public override YoonImage CropImage(YoonRect2N cropArea)
        {
            return CognexFactory.CropImage(this, cropArea);
        }

        public static CognexImage operator +(CognexImage i1, CognexImage i2)
        {
            return CognexFactory.TwoImageProcess.Add(i1, i2);
        }

        public static CognexImage operator -(CognexImage i1, CognexImage i2)
        {
            return CognexFactory.TwoImageProcess.Subtract(i1, i2);
        }
    }
}
