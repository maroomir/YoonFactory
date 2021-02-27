using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using YoonFactory.Files;

namespace YoonFactory.Image
{
    public class YoonImage : IYoonFile
    {
        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    m_pBitmap.Dispose();
                }
                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                m_pBitmap = null;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~DatabaseControl() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion

        private Bitmap m_pBitmap = null;
        private const int DEFAULT_WIDTH = 640;
        private const int DEFAULT_HEIGHT = 480;

        public string FilePath { get; set; }

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

        public YoonImage(IntPtr ptrAddress)
            : this(DEFAULT_WIDTH, DEFAULT_HEIGHT, PixelFormat.Format8bppIndexed)
        {
        }

        public YoonImage(int nWidth, int nHeight, int nPlane)
        {
            if (nWidth <= 0 || nHeight <= 0)
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
            if (nWidth <= 0 || nHeight <= 0)
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

        public YoonImage(IntPtr ptrAddress, int nWidth, int nHeight, PixelFormat nFormat, eYoonRGBMode nMode = eYoonRGBMode.None)
            : this(nWidth, nHeight, nFormat)
        {
            switch (nFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    SetIndexedImage(ptrAddress);
                    break;
                case PixelFormat.Format24bppRgb:
                    switch(nMode)
                    {
                        case eYoonRGBMode.None:
                            SetRGBImage(ptrAddress);
                            break;
                        case eYoonRGBMode.Mixed:
                            SetRGBImageWithColorMixed(ptrAddress);
                            break;
                        case eYoonRGBMode.Parallel:
                            SetRGBImageWithColorParallel(ptrAddress);
                            break;
                        default:
                            break;
                    }
                    break;
                case PixelFormat.Format32bppArgb:
                    switch (nMode)
                    {
                        case eYoonRGBMode.None:
                            SetARGBImage(ptrAddress);
                            break;
                        case eYoonRGBMode.Mixed:
                            SetARGBImageWithColorMixed(ptrAddress);
                            break;
                        case eYoonRGBMode.Parallel:
                            SetARGBImageWithColorParallel(ptrAddress);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public YoonImage(byte[] pBuffer, int nWidth, int nHeight, PixelFormat nFormat, eYoonRGBMode nMode = eYoonRGBMode.None)
            : this(nWidth, nHeight, nFormat)
        {
            switch (nFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    SetIndexedImage(pBuffer);
                    break;
                case PixelFormat.Format24bppRgb:
                    switch (nMode)
                    {
                        case eYoonRGBMode.None:
                            SetRGBImage(pBuffer);
                            break;
                        case eYoonRGBMode.Mixed:
                            SetRGBImageWithColorMixed(pBuffer);
                            break;
                        case eYoonRGBMode.Parallel:
                            SetRGBImageWithColorParallel(pBuffer);
                            break;
                        default:
                            break;
                    }
                    break;
                case PixelFormat.Format32bppArgb:
                    switch (nMode)
                    {
                        case eYoonRGBMode.Mixed:
                            SetARGBImageWithColorMixed(pBuffer);
                            break;
                        case eYoonRGBMode.Parallel:
                            SetARGBImageWithColorParallel(pBuffer);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public YoonImage(int[] pBuffer, int nWidth, int nHeight, PixelFormat nFormat)
            : this(nWidth, nHeight, nFormat)
        {
            switch (nFormat)
            {
                case PixelFormat.Format32bppArgb:
                    SetARGBImage(pBuffer);
                    break;
                default:
                    break;
            }
        }

        public YoonImage(IntPtr ptrAddress, int nWidth, int nHeight, int nPlane, eYoonRGBMode nMode = eYoonRGBMode.None)
            : this(nWidth, nHeight, nPlane)
        {
            switch (nPlane)
            {
                case 1:
                    SetIndexedImage(ptrAddress);
                    break;
                case 3:
                    switch (nMode)
                    {
                        case eYoonRGBMode.None:
                            SetRGBImage(ptrAddress);
                            break;
                        case eYoonRGBMode.Mixed:
                            SetRGBImageWithColorMixed(ptrAddress);
                            break;
                        case eYoonRGBMode.Parallel:
                            SetRGBImageWithColorParallel(ptrAddress);
                            break;
                        default:
                            break;
                    }
                    break;
                case 4:
                    switch (nMode)
                    {
                        case eYoonRGBMode.None:
                            SetARGBImage(ptrAddress);
                            break;
                        case eYoonRGBMode.Mixed:
                            SetARGBImageWithColorMixed(ptrAddress);
                            break;
                        case eYoonRGBMode.Parallel:
                            SetARGBImageWithColorParallel(ptrAddress);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public YoonImage(byte[] pBuffer, int nWidth, int nHeight, int nPlane, eYoonRGBMode nMode = eYoonRGBMode.None)
            : this(nWidth, nHeight, nPlane)
        {
            switch (nPlane)
            {
                case 1:
                    SetIndexedImage(pBuffer);
                    break;
                case 3:
                    switch (nMode)
                    {
                        case eYoonRGBMode.None:
                            SetRGBImage(pBuffer);
                            break;
                        case eYoonRGBMode.Mixed:
                            SetRGBImageWithColorMixed(pBuffer);
                            break;
                        case eYoonRGBMode.Parallel:
                            SetRGBImageWithColorParallel(pBuffer);
                            break;
                        default:
                            break;
                    }
                    break;
                case 4:
                    switch (nMode)
                    {
                        case eYoonRGBMode.Mixed:
                            SetARGBImageWithColorMixed(pBuffer);
                            break;
                        case eYoonRGBMode.Parallel:
                            SetARGBImageWithColorParallel(pBuffer);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public YoonImage(int[] pBuffer, int nWidth, int nHeight, int nPlane)
            : this(nWidth, nHeight, nPlane)
        {
            switch (nPlane)
            {
                case 4:
                    SetARGBImage(pBuffer);
                    break;
                default:
                    break;
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
                pGraph.DrawImage(m_pBitmap, new Rectangle(0, 0, pBitmap.Width, pBitmap.Height), new Rectangle(0, 0, m_pBitmap.Width, m_pBitmap.Height), GraphicsUnit.Pixel);
            }
        }

        public void CopyFrom(IYoonFile pFile)
        {
            if (pFile is YoonImage pImage)
            {
                FilePath = pImage.FilePath;
                m_pBitmap = pImage.CopyImage();
            }
        }

        public IYoonFile Clone()
        {
            YoonImage pImage = new YoonImage(m_pBitmap);
            pImage.FilePath = FilePath;
            return pImage;
        }

        public bool IsFileExist()
        {
            return FileFactory.VerifyFilePath(FilePath, false);
        }

        public bool LoadFile()
        {
            return LoadImage(FilePath);
        }

        public bool SaveFile()
        {
            return SaveImage(FilePath);
        }

        public void SetVerifiedArea(ref YoonRect2N pArea)
        {
            if (pArea.Left < 0)
                pArea.CenterPos.X = m_pBitmap.Width / 2;
            if (pArea.Top < 0)
                pArea.CenterPos.Y = m_pBitmap.Height / 2;
            if (pArea.Right > m_pBitmap.Width)
                pArea.Width = m_pBitmap.Width - pArea.Left;
            if (pArea.Bottom > m_pBitmap.Height)
                pArea.Height = m_pBitmap.Height - pArea.Top;
        }

        public bool LoadImage(string strPath)
        {
            if (IsFileExist()) return false;
            try
            {
                m_pBitmap = (Bitmap)System.Drawing.Image.FromFile(strPath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public bool SaveImage(string strPath)
        {
            try
            {
                m_pBitmap.Save(strPath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public Bitmap CopyImage()
        {
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap size is not normal");

            YoonImage pResult = new YoonImage(m_pBitmap.Width, m_pBitmap.Height, m_pBitmap.PixelFormat);
            {
                switch (Plane)
                {
                    case 1:
                        pResult.SetIndexedImage(this.GetIndexedBuffer());
                        break;
                    case 3:
                        pResult.SetRGBImage(this.GetRGBBuffer());
                        break;
                    case 4:
                        pResult.SetARGBImage(this.GetARGBBuffer());
                        break;
                    default:
                        break;
                }
            }
            return pResult.m_pBitmap;
        }

        public YoonImage CropImage(YoonRect2N cropArea)
        {
            byte[] pByte;
            YoonImage pImageResult;
            using (pImageResult = new YoonImage(cropArea.Width, cropArea.Height, PixelFormat.Format8bppIndexed))
            {
                for (int iY = 0; iY < cropArea.Height; iY++)
                {
                    int nY = cropArea.Top + iY;
                    if (nY >= m_pBitmap.Height) continue;
                    pByte = new byte[cropArea.Width];
                    for (int iX = 0; iX < cropArea.Height; iX++)
                    {
                        int nX = cropArea.Left + iX;
                        if (nX >= m_pBitmap.Width) continue;
                        pByte[iX] = Math.Max((byte)0, Math.Min(GetIndexedPixel(iX, iY), (byte)255));
                    }
                    pImageResult.SetIndexedLine(pByte, iY);
                }
            }
            return pImageResult;
        }

        public bool SetOffset(byte offset)
        {
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap size is not normal");
            byte[] pByte;
            bool bResult = true;
            for (int iY = 0; iY < m_pBitmap.Height; iY++)
            {
                pByte = new byte[m_pBitmap.Width];
                for (int iX = 0; iX < m_pBitmap.Width; iX++)
                {
                    pByte[iX] = (byte)Math.Max(0, Math.Min(GetIndexedPixel(iX, iY) + offset, 255));
                }
                if (!SetIndexedLine(pByte, iY))
                    bResult = false;
            }
            return bResult;
        }

        public byte[] GetIndexedBuffer()
        {
            return Scan8bitBuffer(new Rectangle(Point.Empty, m_pBitmap.Size));
        }

        public byte[] GetIndexedLine(int nPixelY)
        {
            return Scan8bitBuffer(new Rectangle(0, nPixelY, m_pBitmap.Width, 1));
        }

        public byte GetIndexedPixel(int nPixelX, int nPixelY)
        {
            return Scan8bitBuffer(new Rectangle(nPixelX, nPixelY, 1, 1))[0];
        }

        public byte GetIndexedPixel(YoonVector2N pVec)
        {
            return Scan8bitBuffer(new Rectangle(pVec.X, pVec.Y, 1, 1))[0];
        }

        public byte[] GetRGBBuffer()
        {
            return Scan24bitBuffer(new Rectangle(Point.Empty, m_pBitmap.Size));
        }

        public int[] GetARGBBuffer()
        {
            return Scan32bitBuffer(new Rectangle(Point.Empty, m_pBitmap.Size));
        }

        private byte[] GetPlaneBuffer(int nPlane) // RGB Plane 0 : R, 1 : G, 2 : B / ARGB Plane 0 : A, 1 : R, 2 : G, 3 : B
        {
            switch(m_pBitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return Scan24bitBufferPerPlane(new Rectangle(Point.Empty, m_pBitmap.Size), nPlane);
                case PixelFormat.Format32bppArgb:
                    return Scan32bitBufferPerPlane(new Rectangle(Point.Empty, m_pBitmap.Size), nPlane);
                default:
                    throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            }
        }

        public byte[] GetRedBuffer()
        {
            switch (m_pBitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return GetPlaneBuffer(0); // 0 : Red
                case PixelFormat.Format32bppArgb:
                    return GetPlaneBuffer(1); // 1 : Red
                default:
                    throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            }
        }

        public byte[] GetGreenBuffer()
        {
            switch (m_pBitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return GetPlaneBuffer(1); // 1 : Green
                case PixelFormat.Format32bppArgb:
                    return GetPlaneBuffer(2); // 2 : Green
                default:
                    throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            }
        }

        public byte[] GetBlueBuffer()
        {
            switch (m_pBitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return GetPlaneBuffer(2); // 2 : Blue
                case PixelFormat.Format32bppArgb:
                    return GetPlaneBuffer(3); // 3 : Blue
                default:
                    throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            }
        }
        public byte[] GetRGBLine(int nPixelY)
        {
            return Scan24bitBuffer(new Rectangle(0, nPixelY, m_pBitmap.Width, 1));
        }

        public byte[] GetRGBPixel(int nPixelX, int nPixelY)
        {
            return Scan24bitBuffer(new Rectangle(nPixelX, nPixelY, 1, 1));
        }

        public byte[] GetRGBPixel(YoonVector2N pVec)
        {
            return Scan24bitBuffer(new Rectangle(pVec.X, pVec.Y, 1, 1));
        }

        public int[] GetARGBLine(int nPixelY)
        {
            return Scan32bitBuffer(new Rectangle(0, nPixelY, m_pBitmap.Width, 1));
        }

        public int GetARGBPixel(int nPixelX, int nPixelY)
        {
            return Scan32bitBuffer(new Rectangle(nPixelX, nPixelY, 1, 1))[0];
        }

        public int GetARGBPixel(YoonVector2N pVec)
        {
            return Scan32bitBuffer(new Rectangle(pVec.X, pVec.Y, 1, 1))[0];
        }

        public bool SetIndexedImage(IntPtr pBufferAddress)
        {
            if (pBufferAddress == IntPtr.Zero)
                throw new ArgumentNullException("[YOONIMAGE EXCEPTION] Address is null");
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap size is not normal");

            byte[] pBuffer = new byte[m_pBitmap.Width * m_pBitmap.Height];
            Marshal.Copy(pBufferAddress, pBuffer, 0, m_pBitmap.Width * m_pBitmap.Height);
            return SetIndexedImage(pBuffer);
        }

        public bool SetIndexedImage(byte[] pBuffer)
        {
            return Print8bitBuffer(pBuffer, new Rectangle(Point.Empty, m_pBitmap.Size));
        }

        public bool SetIndexedLine(byte[] pBuffer, int nPixelY)
        {
            return Print8bitBuffer(pBuffer, new Rectangle(0, nPixelY, m_pBitmap.Width, 1));
        }

        public bool SetIndexedPixel(byte nLevel, int nPixelX, int nPixelY)
        {
            return Print8bitBuffer(new byte[1] { nLevel }, new Rectangle(nPixelX, nPixelY, 1, 1));
        }

        public bool SetIndexedPixel(byte nLevel, YoonVector2N pVec)
        {
            return Print8bitBuffer(new byte[1] { nLevel }, new Rectangle(pVec.X, pVec.Y, 1, 1));
        }

        public bool SetRGBImage(IntPtr pBufferAddress)
        {
            if (pBufferAddress == IntPtr.Zero)
                throw new ArgumentNullException("[YOONIMAGE EXCEPTION] Address is null");
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap size is not normal");

            byte[] pBuffer = new byte[m_pBitmap.Width * m_pBitmap.Height * 3];
            Marshal.Copy(pBufferAddress, pBuffer, 0, m_pBitmap.Width * m_pBitmap.Height);
            return SetRGBImage(pBuffer);
        }

        public bool SetRGBImage(byte[] pBuffer)
        {
            return Print24bitBuffer(pBuffer, new Rectangle(Point.Empty, m_pBitmap.Size));
        }

        public bool SetRGBLine(byte[] pBuffer, int nPixelY)
        {
            return Print24bitBuffer(pBuffer, new Rectangle(0, nPixelY, m_pBitmap.Width, 1));
        }

        public bool SetRGBPixel(byte[] pLevel, int nPixelX, int nPixelY)
        {
            return Print24bitBuffer(pLevel, new Rectangle(nPixelX, nPixelY, 1, 1));
        }

        public bool SetRGBPixel(byte[] pLevel, YoonVector2N pVec)
        {
            return Print24bitBuffer(pLevel, new Rectangle(pVec.X, pVec.Y, 1, 1));
        }

        public bool SetRGBImageWithColorMixed(IntPtr pBufferAddress, bool bRGBOrder = true)
        {
            if (pBufferAddress == IntPtr.Zero)
                throw new ArgumentNullException("[YOONIMAGE EXCEPTION] Address is null");
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0 || m_pBitmap.PixelFormat != PixelFormat.Format24bppRgb)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap property is not normal");
            byte[] pBuffer = new byte[m_pBitmap.Width * m_pBitmap.Height * 3];
            Marshal.Copy(pBufferAddress, pBuffer, 0, m_pBitmap.Width * m_pBitmap.Height * 3);
            return SetRGBImageWithColorMixed(pBuffer, bRGBOrder);
        }

        public bool SetRGBImageWithColorMixed(byte[] pBuffer, bool bRGBOrder = true)
        {
            if (m_pBitmap.PixelFormat != PixelFormat.Format24bppRgb)
                throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            if (pBuffer.Length != m_pBitmap.Width * m_pBitmap.Height * 3)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Buffer property is out of range");
            int nRed, nGreen, nBlue;
            if (bRGBOrder) { nRed = 0; nGreen = 1; nBlue = 2; }
            else { nRed = 2; nGreen = 1; nBlue = 0; }
            byte[] pPixel;
            bool bResult = true;
            for (int iY = 0; iY < m_pBitmap.Height; iY++) // Exception for nHeight-1
            {
                pPixel = new byte[m_pBitmap.Width * 3];
                for (int iX = 0; iX < m_pBitmap.Width; iX++)
                {
                    pPixel[iX * 3 + 0] = pBuffer[iY * m_pBitmap.Width * 3 + iX * 3 + nBlue]; // Blue
                    pPixel[iX * 3 + 1] = pBuffer[iY * m_pBitmap.Width * 3 + iX * 3 + nGreen]; // Green
                    pPixel[iX * 3 + 2] = pBuffer[iY * m_pBitmap.Width * 3 + iX * 3 + nRed]; // Red
                }
                if (!SetRGBLine(pPixel, iY))
                    bResult = false;
            }
            return bResult;
        }

        public bool SetRGBImageWithColorParallel(IntPtr pBufferAddress, bool bRGBOrder = true)
        {
            if (pBufferAddress == IntPtr.Zero)
                throw new ArgumentNullException("[YOONIMAGE EXCEPTION] Address is null");
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0 || m_pBitmap.PixelFormat != PixelFormat.Format24bppRgb)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap property is not normal");
            byte[] pBuffer = new byte[m_pBitmap.Width * m_pBitmap.Height * 3];
            Marshal.Copy(pBufferAddress, pBuffer, 0, m_pBitmap.Width * m_pBitmap.Height * 3);
            return SetRGBImageWithColorParallel(pBuffer, bRGBOrder);
        }

        public bool SetRGBImageWithColorParallel(byte[] pBuffer, bool bRGBOrder = true)
        {
            if (m_pBitmap.PixelFormat != PixelFormat.Format24bppRgb)
                throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            if (pBuffer.Length != m_pBitmap.Width * m_pBitmap.Height * 3)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Buffer property is out of range");
            int nRed, nGreen, nBlue;
            if (bRGBOrder) { nRed = 0; nGreen = 1; nBlue = 2; }
            else { nRed = 2; nGreen = 1; nBlue = 0; }
            byte[] pPixel;
            bool bResult = true;
            for (int iPlane = 0; iPlane < 3; iPlane++)
            {
                pPixel = new byte[m_pBitmap.Width * 3];
                for (int iY = 0; iY < m_pBitmap.Height; iY++) // Exception for nHeight-1
                {
                    for (int iX = 0; iX < m_pBitmap.Width; iX++)
                    {
                        pPixel[iX * 3 + 0] = pBuffer[(nBlue * m_pBitmap.Width * m_pBitmap.Height) + iY * m_pBitmap.Width + iX]; // Blue
                        pPixel[iX * 3 + 1] = pBuffer[(nGreen * m_pBitmap.Width * m_pBitmap.Height) + iY * m_pBitmap.Width + iX]; // Green
                        pPixel[iX * 3 + 2] = pBuffer[(nRed * m_pBitmap.Width * m_pBitmap.Height) + iY * m_pBitmap.Width + iX]; // Red
                    }
                    if (!SetRGBLine(pPixel, iY))
                        bResult = false;
                }
            }
            return bResult;
        }

        public bool SetRGBImageWithPlane(byte[] pRed, byte[] pGreen, byte[] pBlue)
        {
            if (m_pBitmap.PixelFormat != PixelFormat.Format24bppRgb)
                throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            if (pRed == null || pRed.Length != m_pBitmap.Width * m_pBitmap.Height ||
                pGreen == null || pGreen.Length != m_pBitmap.Width * m_pBitmap.Height ||
                pBlue == null || pBlue.Length != m_pBitmap.Width * m_pBitmap.Height)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Buffer property is out of range");
            byte[] pPixel;
            bool bResult = true;
            for (int iY = 0; iY < m_pBitmap.Height; iY++)
            {
                pPixel = new byte[m_pBitmap.Width * 3];
                for (int iX = 0; iX < m_pBitmap.Width; iX++)
                {
                    pPixel[iX * 3 + 0] = pBlue[iY * m_pBitmap.Width + iX];
                    pPixel[iX * 3 + 1] = pGreen[iY * m_pBitmap.Width + iX];
                    pPixel[iX * 3 + 2] = pRed[iY * m_pBitmap.Width + iX];
                }
                if (!SetRGBLine(pPixel, iY))
                    bResult = false;
            }
            return bResult;
        }

        public bool SetARGBImage(IntPtr pBufferAddress)
        {
            if (pBufferAddress == IntPtr.Zero)
                throw new ArgumentNullException("[YOONIMAGE EXCEPTION] Address is null");
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap size is not normal");

            int[] pBuffer = new int[m_pBitmap.Width * m_pBitmap.Height];
            Marshal.Copy(pBufferAddress, pBuffer, 0, m_pBitmap.Width * m_pBitmap.Height);
            return SetARGBImage(pBuffer);
        }

        public bool SetARGBImage(int[] pBuffer)
        {
            return Print32bitBuffer(pBuffer, new Rectangle(Point.Empty, m_pBitmap.Size));
        }

        public bool SetARGBLine(int[] pBuffer, int nPixelY)
        {
            return Print32bitBuffer(pBuffer, new Rectangle(0, nPixelY, m_pBitmap.Width, 1));
        }

        public bool SetARGBPixel(int nLevel, int nPixelX, int nPixelY)
        {
            return Print32bitBuffer(new int[1] { nLevel }, new Rectangle(nPixelX, nPixelY, 1, 1));
        }

        public bool SetARGBPixel(int nLevel, YoonVector2N pVec)
        {
            return Print32bitBuffer(new int[1] { nLevel }, new Rectangle(pVec.X, pVec.Y, 1, 1));
        }

        public bool SetARGBImageWithColorMixed(IntPtr pBufferAddress, bool bRGBOrder = true)
        {
            if (pBufferAddress == IntPtr.Zero)
                throw new ArgumentNullException("[YOONIMAGE EXCEPTION] Address is null");
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0 || m_pBitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap property is not normal");
            byte[] pBuffer = new byte[m_pBitmap.Width * m_pBitmap.Height * 4];
            Marshal.Copy(pBufferAddress, pBuffer, 0, m_pBitmap.Width * m_pBitmap.Height * 4);
            return SetARGBImageWithColorMixed(pBuffer, bRGBOrder);
        }

        public bool SetARGBImageWithColorMixed(byte[] pBuffer, bool bRGBOrder = true)
        {
            if (m_pBitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            if (pBuffer.Length != m_pBitmap.Width * m_pBitmap.Height * 3)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Buffer property is out of range");
            int nRed, nGreen, nBlue;
            int nAlpha = 3;
            if (bRGBOrder) { nRed = 0; nGreen = 1; nBlue = 2; } // Alpha = 3
            else { nRed = 2; nGreen = 1; nBlue = 0; } // Alpha = 3
            int[] pPixel;
            bool bResult = true;
            for (int iY = 0; iY < m_pBitmap.Height; iY++) // Exception for nHeight-1
            {
                pPixel = new int[m_pBitmap.Width];
                for (int iX = 0; iX < m_pBitmap.Width; iX++)
                {
                    byte[] pBytePixel = new byte[4];
                    pBytePixel[0] = pBuffer[iY * m_pBitmap.Width * 3 + iX * 3 + nBlue]; // Blue
                    pBytePixel[1] = pBuffer[iY * m_pBitmap.Width * 3 + iX * 3 + nGreen]; // Green
                    pBytePixel[2] = pBuffer[iY * m_pBitmap.Width * 3 + iX * 3 + nRed]; // Red
                    pBytePixel[3] = pBuffer[iY * m_pBitmap.Width * 3 + iX * 3 + nAlpha]; // Alpha = Max (0xFF)
                    pPixel[iX] = BitConverter.ToInt32(pBytePixel, 0);
                }
                if (!SetARGBLine(pPixel, iY))
                    bResult = false;
            }
            return bResult;
        }

        public bool SetARGBImageWithColorParallel(IntPtr pBufferAddress, bool bRGBOrder = true)
        {
            if (pBufferAddress == IntPtr.Zero)
                throw new ArgumentNullException("[YOONIMAGE EXCEPTION] Address is null");
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0 || m_pBitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap property is not normal");
            byte[] pBuffer = new byte[m_pBitmap.Width * m_pBitmap.Height * 4];
            Marshal.Copy(pBufferAddress, pBuffer, 0, m_pBitmap.Width * m_pBitmap.Height * 4);
            return SetARGBImageWithColorParallel(pBuffer, bRGBOrder);
        }

        public bool SetARGBImageWithColorParallel(byte[] pBuffer, bool bRGBOrder = true)
        {
            if (m_pBitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            if (pBuffer.Length != m_pBitmap.Width * m_pBitmap.Height * 3)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Buffer property is out of range");
            int nRed, nGreen, nBlue;
            int nAlpha = 3;
            if (bRGBOrder) { nRed = 0; nGreen = 1; nBlue = 2; } // Alpha = 3
            else { nRed = 2; nGreen = 1; nBlue = 0; } // Alpha = 3
            int[] pPixel;
            bool bResult = true;
            for (int iPlane = 0; iPlane < 3; iPlane++)
            {
                pPixel = new int[m_pBitmap.Width];
                for (int iY = 0; iY < m_pBitmap.Height; iY++) // Exception for nHeight-1
                {
                    for (int iX = 0; iX < m_pBitmap.Width; iX++)
                    {
                        byte[] pBytePixel = new byte[4];
                        pBytePixel[0] = pBuffer[(nBlue * m_pBitmap.Width * m_pBitmap.Height) + iY * m_pBitmap.Width + iX]; // Blue
                        pBytePixel[1] = pBuffer[(nGreen * m_pBitmap.Width * m_pBitmap.Height) + iY * m_pBitmap.Width + iX]; // Green
                        pBytePixel[2] = pBuffer[(nRed * m_pBitmap.Width * m_pBitmap.Height) + iY * m_pBitmap.Width + iX]; // Red
                        pBytePixel[3] = pBuffer[(nAlpha * m_pBitmap.Width * m_pBitmap.Height) + iY * m_pBitmap.Width + iX]; ; // Alpha = Max (0xFF)
                        pPixel[iX] = BitConverter.ToInt32(pBytePixel, 0);
                    }
                    if (!SetARGBLine(pPixel, iY))
                        bResult = false;
                }
            }
            return bResult;
        }

        public bool SetARGBImageWithPlane(byte[] pRed, byte[] pGreen, byte[] pBlue)
        {
            if (m_pBitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            if (pRed == null || pRed.Length != m_pBitmap.Width * m_pBitmap.Height ||
                pGreen == null || pGreen.Length != m_pBitmap.Width * m_pBitmap.Height ||
                pBlue == null || pBlue.Length != m_pBitmap.Width * m_pBitmap.Height)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Buffer property is out of range");
            int[] pPixel;
            bool bResult = true;
            for (int iY = 0; iY < m_pBitmap.Height; iY++)
            {
                pPixel = new int[m_pBitmap.Width];
                for (int iX = 0; iX < m_pBitmap.Width; iX++)
                {
                    byte[] pBytePixel = new byte[4];
                    pBytePixel[0] = pBlue[iY * m_pBitmap.Width + iX];
                    pBytePixel[1] = pGreen[iY * m_pBitmap.Width + iX];
                    pBytePixel[2] = pRed[iY * m_pBitmap.Width + iX];
                    pBytePixel[3] = (byte)0xFF; // Alpha = Max (0xFF)
                    pPixel[iX] = BitConverter.ToInt32(pBytePixel, 0);
                }
                if (!SetARGBLine(pPixel, iY))
                    bResult = false;
            }
            return bResult;
        }

        private byte[] Scan8bitBuffer(Rectangle pRect)
        {
            if (Plane != 1)
                throw new FormatException("[YOONIMAGE EXCEPTION] Bitmap isnot Indexed format");
            if (pRect.X > m_pBitmap.Width || pRect.Y > m_pBitmap.Height)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Rect property is out of range");
            byte[] pBuffer = new byte[pRect.Width * pRect.Height];
            try
            {
                BitmapData pImageData = m_pBitmap.LockBits(pRect, ImageLockMode.ReadOnly, m_pBitmap.PixelFormat);
                Marshal.Copy(pImageData.Scan0, pBuffer, 0, pBuffer.Length);
                m_pBitmap.UnlockBits(pImageData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return pBuffer;
        }

        private byte[] Scan24bitBuffer(Rectangle pRect)
        {
            if (Plane != 3)
                throw new FormatException("[YOONIMAGE EXCEPTION] Bitmap isnot RGB format");
            if (pRect.X > m_pBitmap.Width || pRect.Y > m_pBitmap.Height)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Rect property is out of range");
            byte[] pBuffer = new byte[pRect.Width * pRect.Height * Plane];
            try
            {
                BitmapData pImageData = m_pBitmap.LockBits(pRect, ImageLockMode.ReadOnly, m_pBitmap.PixelFormat);
                Marshal.Copy(pImageData.Scan0, pBuffer, 0, pBuffer.Length);
                m_pBitmap.UnlockBits(pImageData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return pBuffer;
        }

        private int[] Scan32bitBuffer(Rectangle pRect)
        {
            if (Plane != 4)
                throw new FormatException("[YOONIMAGE EXCEPTION] Bitmap isnot RGB format");
            if (pRect.X > m_pBitmap.Width || pRect.Y > m_pBitmap.Height)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Rect property is out of range");
            int[] pBuffer = new int[pRect.Width * pRect.Height];
            try
            {
                BitmapData pImageData = m_pBitmap.LockBits(pRect, ImageLockMode.ReadOnly, m_pBitmap.PixelFormat);
                Marshal.Copy(pImageData.Scan0, pBuffer, 0, pBuffer.Length);
                m_pBitmap.UnlockBits(pImageData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return pBuffer;
        }

        private byte[] Scan24bitBufferPerPlane(Rectangle pRect, int nPlane)
        {
            if (Plane != 3)
                throw new FormatException("[YOONIMAGE EXCEPTION] Bitmap isnot RGB format");
            if (nPlane < 0 || nPlane >= Plane)
                throw new ArgumentException("[YOONIMAGE EXCEPTION] Plane isnot adjust");
            if (pRect.X > m_pBitmap.Width || pRect.Y > m_pBitmap.Height)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Rect property is out of range");
            byte[] pPixelPlanes = new byte[pRect.Width * pRect.Height];
            try
            {
                BitmapData pImageData = m_pBitmap.LockBits(pRect, ImageLockMode.ReadOnly, m_pBitmap.PixelFormat);
                for (int j = 0; j < pRect.Height; j++)
                {
                    for (int i = 0; i < pRect.Width; i++)
                    {
                        byte[] pBytePixel = new byte[3]; // {R, G, B}
                        Marshal.Copy(pImageData.Scan0 + (j * pRect.Width * 3 + i * 3), pBytePixel, 0, pBytePixel.Length);
                        pPixelPlanes[j * pRect.Width + i] = pBytePixel[nPlane];
                    }
                }
                m_pBitmap.UnlockBits(pImageData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return pPixelPlanes;
        }

        private byte[] Scan32bitBufferPerPlane(Rectangle pRect, int nPlane)
        {
            if (Plane != 4)
                throw new FormatException("[YOONIMAGE EXCEPTION] Bitmap isnot RGB format");
            if (nPlane < 0 || nPlane >= Plane)
                throw new ArgumentException("[YOONIMAGE EXCEPTION] Plane isnot adjust");
            if (pRect.X > m_pBitmap.Width || pRect.Y > m_pBitmap.Height)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Rect property is out of range");
            byte[] pPixelPlanes = new byte[pRect.Width * pRect.Height];
            try
            {
                BitmapData pImageData = m_pBitmap.LockBits(pRect, ImageLockMode.ReadOnly, m_pBitmap.PixelFormat);
                for (int j = 0; j < pRect.Height; j++)
                {
                    for (int i = 0; i < pRect.Width; i++)
                    {
                        byte[] pBytePixel = new byte[4]; // {A, R, G, B}
                        Marshal.Copy(pImageData.Scan0 + (j * pRect.Width * 4 + i * 4), pBytePixel, 0, pBytePixel.Length);
                        pPixelPlanes[j * pRect.Width + i] = pBytePixel[nPlane];
                    }
                }
                m_pBitmap.UnlockBits(pImageData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return pPixelPlanes;
        }

        private bool Print8bitBuffer(byte[] pBuffer, Rectangle pRect)
        {
            if (Plane != 1)
                throw new FormatException("[YOONIMAGE EXCEPTION] Bitmap isnot Indexed format");
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap size is not normal");
            if (pRect.X > m_pBitmap.Width || pRect.Y > m_pBitmap.Height)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Rect property is out of range");
            if (pBuffer.Length != pRect.Width * pRect.Height)
                throw new ArgumentException("[YOONIMAGE ERROR] Rect or Buffer property is abnormal");
            try
            {
                BitmapData pImageData = m_pBitmap.LockBits(pRect, ImageLockMode.WriteOnly, m_pBitmap.PixelFormat);
                Marshal.Copy(pBuffer, 0, pImageData.Scan0, pBuffer.Length);
                m_pBitmap.UnlockBits(pImageData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        private bool Print24bitBuffer(byte[] pBuffer, Rectangle pRect)
        {
            if (Plane != 3)
                throw new FormatException("[YOONIMAGE EXCEPTION] Bitmap isnot RGB format");
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap size is not normal");
            if (pRect.X > m_pBitmap.Width || pRect.Y > m_pBitmap.Height)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Rect property is out of range");
            if (pBuffer.Length != pRect.Width * pRect.Height * Plane)
                throw new ArgumentException("[YOONIMAGE ERROR] Rect or Buffer property is abnormal");
            try
            {
                BitmapData pImageData = m_pBitmap.LockBits(pRect, ImageLockMode.WriteOnly, m_pBitmap.PixelFormat);
                Marshal.Copy(pBuffer, 0, pImageData.Scan0, pBuffer.Length);
                m_pBitmap.UnlockBits(pImageData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        private bool Print32bitBuffer(int[] pBuffer, Rectangle pRect)
        {
            if (Plane != 4)
                throw new FormatException("[YOONIMAGE EXCEPTION] Bitmap isnot RGB format");
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap size is not normal");
            if (pRect.X > m_pBitmap.Width || pRect.Y > m_pBitmap.Height)
                throw new ArgumentOutOfRangeException("[YOONIMAGE ERROR] Rect property is out of range");
            if (pBuffer.Length != pRect.Width * pRect.Height)
                throw new ArgumentException("[YOONIMAGE ERROR] Rect or Buffer property is abnormal");
            try
            {
                BitmapData pImageData = m_pBitmap.LockBits(pRect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(pBuffer, 0, pImageData.Scan0, pBuffer.Length);
                m_pBitmap.UnlockBits(pImageData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }
    }
}
