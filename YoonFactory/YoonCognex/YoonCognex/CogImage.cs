using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoonFactory.Image;
using Cognex.VisionPro;
using System.Drawing;

namespace YoonFactory.Cognex
{
    public class CogImage : YoonImage
    {
        public CogImage(ICogImage pImage)
        {
            m_pBitmap = pImage.ToBitmap();
        }

        public ICogImage ToCogImage()
        {
            switch(Plane)
            {
                case 1:
                    CogImage8Grey pImage = new CogImage8Grey();
                    pImage.Allocate(Width, Height);
                    for (int iY = 0; iY < Height; iY++)
                    {
                        for (int iX = 0; iX < Width; iX++)
                        {
                            Color pColor = m_pBitmap.GetPixel(iX, iY);
                            byte nLevel = (byte)(pColor.R * 0.299f + pColor.G * 0.587f + pColor.B * 0.114f);
                            pImage.SetPixel(iX, iY, nLevel);
                        }
                    }
                    return pImage;
                    break;
            }
        }
    }
}
