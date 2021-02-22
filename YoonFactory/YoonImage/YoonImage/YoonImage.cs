using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace YoonFactory.Image
{
    public class YoonImage
    {
        private Bitmap m_pBitmap = null;
        private const int DEFAULT_WIDTH = 640;
        private const int DEFAULT_HEIGHT = 480;

        public PixelFormat Format
        {
            get => m_pBitmap.PixelFormat;
        }

        public int Plane
        {
            get
            {
                switch (m_pBitmap.PixelFormat)
                {
                    case PixelFormat.Format8bppIndexed:
                        return 1;
                    case PixelFormat.Format16bppArgb1555:
                    case PixelFormat.Format16bppGrayScale:
                    case PixelFormat.Format16bppRgb555:
                    case PixelFormat.Format16bppRgb565:
                    case PixelFormat.Format1bppIndexed:
                        return 2;
                    case PixelFormat.Format24bppRgb:
                        return 3;
                    case PixelFormat.Format32bppArgb:
                    case PixelFormat.Format32bppPArgb:
                    case PixelFormat.Format32bppRgb:
                        return 4;
                    default:
                        return 0;
                }
            }
        }

        private PixelFormat GetDefaultFormat(int nPlane)
        {
            switch (nPlane)
            {
                case 1:
                    return PixelFormat.Format8bppIndexed;
                case 2:
                    return PixelFormat.Format16bppGrayScale;
                case 3:
                    return PixelFormat.Format24bppRgb;
                case 4:
                    return PixelFormat.Format32bppArgb;
                default:
                    return PixelFormat.DontCare;
            }
        }

        public int Stride
        {
            get => Plane * m_pBitmap.Width;
        }

        public int Width
        {
            get => m_pBitmap.Width;
        }

        public int Height
        {
            get => m_pBitmap.Height;
        }

        public YoonImage()
            : this(DEFAULT_WIDTH, DEFAULT_HEIGHT, PixelFormat.Format8bppIndexed)
        {
        }

        public YoonImage(int nWidth, int nHeight, int nPlane)
        {
            if (nWidth <= 0 || Height <= 0)
            {
                nWidth = DEFAULT_WIDTH;
                nHeight = DEFAULT_HEIGHT;
            }
            m_pBitmap = new Bitmap(nWidth, nHeight, GetDefaultFormat(nPlane));
            ColorPalette pPallete = m_pBitmap.Palette;
            switch (m_pBitmap.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    for (int i = 0; i < 256; i++)
                        pPallete.Entries[i] = Color.FromArgb(i, i, i);
                    m_pBitmap.Palette = pPallete;
                    break;
                default:
                    break;
            }
        }

        public YoonImage(int nWidth, int nHeight, PixelFormat nFormat)
        {
            if (nWidth <= 0 || Height <= 0)
            {
                nWidth = DEFAULT_WIDTH;
                nHeight = DEFAULT_HEIGHT;
            }
            m_pBitmap = new Bitmap(nWidth, nHeight, nFormat);
            ColorPalette pPallete = m_pBitmap.Palette;
            switch (m_pBitmap.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    for (int i = 0; i < 256; i++)
                        pPallete.Entries[i] = Color.FromArgb(i, i, i);
                    m_pBitmap.Palette = pPallete;
                    break;
                default:
                    break;
            }
        }

        public YoonImage(byte[] pBuffer, int nWidth, int nHeight, int nPlane)
            : this(nWidth, nHeight, nPlane)
        {
            try
            {
                BitmapData pData = m_pBitmap.LockBits(new Rectangle(0, 0, m_pBitmap.Width, m_pBitmap.Height), ImageLockMode.ReadWrite, m_pBitmap.PixelFormat);
                if (Stride == pData.Stride)
                {
                    Marshal.Copy(pBuffer, 0, pData.Scan0, pData.Stride * m_pBitmap.Height);
                }
                else
                {
                    for (int i = 0; i < m_pBitmap.Height; i++)
                    {
                        Marshal.Copy(pBuffer, i * Stride, new IntPtr(pData.Scan0.ToInt64() + i * pData.Stride), nWidth);
                    }
                }
                m_pBitmap.UnlockBits(pData);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public YoonImage(IntPtr ptrBuffer, int nWidth, int nHeight, int nPlane)
            : this(nWidth, nHeight, nPlane)
        {
            try
            {
                BitmapData pData = m_pBitmap.LockBits(new Rectangle(0, 0, m_pBitmap.Width, m_pBitmap.Height), ImageLockMode.ReadWrite, m_pBitmap.PixelFormat);
                byte[] pBuffer = new byte[pData.Stride * m_pBitmap.Height];
                Marshal.Copy(ptrBuffer, pBuffer, 0, pData.Stride * m_pBitmap.Height);
                if (Stride == pData.Stride)
                {
                    Marshal.Copy(pBuffer, 0, pData.Scan0, pData.Stride * m_pBitmap.Height);
                }
                else
                {
                    for (int i = 0; i < m_pBitmap.Height; i++)
                    {
                        Marshal.Copy(pBuffer, i * Stride, new IntPtr(pData.Scan0.ToInt64() + i * pData.Stride), nWidth);
                    }
                }
                m_pBitmap.UnlockBits(pData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public YoonImage(Bitmap pBitmap) : this(pBitmap, pBitmap.PixelFormat)
        {
        }

        public YoonImage(Bitmap pBitmap, PixelFormat nFormat)
        {
            m_pBitmap = new Bitmap(pBitmap.Width, pBitmap.Height, nFormat);
            using (Graphics pGraph = Graphics.FromImage(m_pBitmap))
            {
                pGraph.DrawImage(m_pBitmap, new Rectangle(0, 0, m_pBitmap.Width, m_pBitmap.Height), new Rectangle(0, 0, m_pBitmap.Width, m_pBitmap.Height), GraphicsUnit.Pixel);
            }
        }
    }
}
