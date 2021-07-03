using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public CognexImage() : base()
        {
            //
        }

        public CognexImage(Bitmap pBitmap) : base(pBitmap)
        {
            //
        }

        public CognexImage(ICogImage pImage) : this()
        {
            if (pImage.Width != 0 && pImage.Height != 0)
                m_pBitmap = ConvertBitmap(pImage);
        }

        private Bitmap ConvertBitmap(ICogImage cogImage)
        {
            YoonImage pResultImage = new YoonImage(cogImage.ToBitmap());
            if (cogImage is CogImage8Grey)
                pResultImage = pResultImage.ToGrayImage();
            return pResultImage.CopyImage();
        }

        public ICogImage ToCogImage()
        {
            switch (Plane)
            {
                case 1:
                    CogImage8Grey pImage8bit = new CogImage8Grey();
                    pImage8bit.Allocate(Width, Height);
                    for (int iY = 0; iY < Height; iY++)
                    {
                        for (int iX = 0; iX < Width; iX++)
                        {
                            Color pColor = m_pBitmap.GetPixel(iX, iY);
                            byte nLevel = (byte)(pColor.R * 0.299f + pColor.G * 0.587f + pColor.B * 0.114f); // ITU-RBT.709, YPrPb
                            pImage8bit.SetPixel(iX, iY, nLevel);
                        }
                    }
                    return pImage8bit;
                case 3:
                case 4:
                    CogImage24PlanarColor pImage24bit = new CogImage24PlanarColor();
                    pImage24bit.Allocate(Width, Height);
                    for (int iY = 0; iY < Height; iY++)
                    {
                        for (int iX = 0; iX < Width; iX++)
                        {
                            Color pColor = m_pBitmap.GetPixel(iX, iY);
                            pImage24bit.SetPixel(iX, iY, pColor.R, pColor.G, pColor.B);
                        }
                    }
                    return pImage24bit;
            }
            return null;
        }

        public override bool LoadImage(string strPath)
        {
            FilePath = strPath;
            if (!IsFileExist()) return false;
            ICogImage pCogImage = null;
            CogImageFile pCogFile = new CogImageFile();
            try
            {
                pCogFile.Open(FilePath, CogImageFileModeConstants.Read);
                if (pCogFile[0] != null)
                {
                    pCogImage = pCogFile[0];
                    m_pBitmap = ConvertBitmap(pCogImage);
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
            ICogImage pImage = ToCogImage();
            CogImageFile pCogFile = new CogImageFile();
            try
            {
                if (IsFileExist())
                {
                    pCogFile.Open(FilePath, CogImageFileModeConstants.Update);
                    pCogFile.Append(pImage);
                    pCogFile.Close();
                }
                else
                {
                    if (FileFactory.VerifyDirectory(FilePath))
                    {
                        pCogFile.Open(FilePath, CogImageFileModeConstants.Write);
                        pCogFile.Append(pImage);
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
            return ToCogImage().CopyBase(CogImageCopyModeConstants.CopyPixels);
        }

        public override IYoonFile Clone()
        {
            CognexImage pImage = new CognexImage(m_pBitmap);
            pImage.FilePath = FilePath;
            return pImage;
        }

        public override YoonImage ToGrayImage()
        {
            ICogImage pImageSource = ToCogImage();
            if (pImageSource is CogImage8Grey)
                return Clone() as YoonImage;
            CogImage8Grey pImage = CogImageConvert.GetIntensityImage(pImageSource, 0, 0, Width, Height);
            return new CognexImage(pImage);
        }

        public override YoonImage ToRGBImage()
        {
            ICogImage pImageSource = ToCogImage();
            if (pImageSource is CogImage24PlanarColor)
                return Clone() as YoonImage;
            CogImage24PlanarColor pImage = CogImageConvert.GetRGBImage(pImageSource, 0, 0, Width, Height) as CogImage24PlanarColor;
            return new CognexImage(pImage);
        }

        public CognexImage ToHSIImage()
        {
            CogImage24PlanarColor pImage = CogImageConvert.GetHSIImage(ToCogImage(), 0, 0, Width, Height) as CogImage24PlanarColor;
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
