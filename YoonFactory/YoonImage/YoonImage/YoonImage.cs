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

        protected Bitmap m_pBitmap = null;
        protected const int DEFAULT_WIDTH = 640;
        protected const int DEFAULT_HEIGHT = 480;

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

        public Bitmap ToBitmap
        {
            get => m_pBitmap;
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
                    SetGrayImage(ptrAddress);
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
                    SetGrayImage(pBuffer);
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
                    SetGrayImage(ptrAddress);
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
                    SetGrayImage(pBuffer);
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

        public bool Equals(YoonImage pObject)
        {
            if (pObject.Width != Width || pObject.Height != Height || pObject.Plane != Plane)
                return false;
            for (int iX = 0; iX < Width; iX++)
            {
                for (int iY = 0; iY < Height; iY++)
                {
                    if (pObject.m_pBitmap.GetPixel(iX, iY) != m_pBitmap.GetPixel(iX, iY))
                        return false;
                }
            }
            return true;
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

        public bool IsVerifiedArea(YoonRect2N pArea)
        {
            return (pArea.Left < 0 || pArea.Top < 0 || pArea.Right > m_pBitmap.Width || pArea.Bottom > m_pBitmap.Height) ? false : true;
        }

        public virtual bool LoadImage(string strPath)
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

        public virtual bool SaveImage(string strPath)
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
                        pResult.SetGrayImage(this.GetGrayBuffer());
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

        public virtual YoonImage CropImage(YoonRect2N cropArea)
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
                        pByte[iX] = Math.Max((byte)0, Math.Min(GetGrayPixel(iX, iY), (byte)255));
                    }
                    pImageResult.SetGrayLine(pByte, iY);
                }
            }
            return pImageResult;
        }

        public YoonImage ToGrayImage()
        {
            if (Plane == 1)
                return this;
            if (Plane != 3 || Plane != 4)
                throw new FormatException("[YOONIMAGE ERROR] Bitmap format is not comportable");
            byte[] pByte = new byte[Width * Height];
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    pByte[j * Width + i] = (byte)(0.299f * GetRedPixel(i, j) + 0.587f * GetGreenPixel(i, j) + 0.114f * GetBluePixel(i, j)); // ITU-RBT.709, YPrPb
                }
            }
            return new YoonImage(pByte, Width, Height, PixelFormat.Format8bppIndexed);
        }

        public void FillTriangle(int x, int y, int size, eYoonDir2D direction, Color fillColor, double zoom)
        {
            PointF[] pPoint = new PointF[3];
            pPoint[0].X = (float)(x * zoom);
            pPoint[0].Y = (float)(y * zoom);
            switch (direction)
            {
                case eYoonDir2D.Top:
                    pPoint[1].X = pPoint[0].X + (float)(size / 2 * zoom);
                    pPoint[1].Y = pPoint[0].Y + (float)(size * zoom);
                    pPoint[2].X = pPoint[0].X - (float)(size / 2 * zoom);
                    pPoint[2].Y = pPoint[0].Y + (float)(size * zoom);
                    break;
                case eYoonDir2D.Bottom:
                    pPoint[1].X = pPoint[0].X - (float)(size / 2 * zoom);
                    pPoint[1].Y = pPoint[0].Y - (float)(size * zoom);
                    pPoint[2].X = pPoint[0].X + (float)(size / 2 * zoom);
                    pPoint[2].Y = pPoint[0].Y - (float)(size * zoom);
                    break;
                case eYoonDir2D.Left:
                    pPoint[1].X = pPoint[0].X + (float)(size * zoom);
                    pPoint[1].Y = pPoint[0].Y - (float)(size / 2 * zoom);
                    pPoint[2].X = pPoint[0].X + (float)(size * zoom);
                    pPoint[2].Y = pPoint[0].Y + (float)(size / 2 * zoom);
                    break;
                case eYoonDir2D.Right:
                    pPoint[1].X = pPoint[0].X - (float)(size * zoom);
                    pPoint[1].Y = pPoint[0].Y + (float)(size / 2 * zoom);
                    pPoint[2].X = pPoint[0].X - (float)(size * zoom);
                    pPoint[2].Y = pPoint[0].Y - (float)(size / 2 * zoom);
                    break;
            }
            using (Graphics graph = Graphics.FromImage(m_pBitmap))
            {
                SolidBrush brush = new SolidBrush(fillColor);
                graph.FillPolygon(brush, pPoint);
            }
        }

        public virtual void FillRect(YoonRect2N pRect, Color fillColor, double dRatio = 1.0)
        {
            float startX = (float)(pRect.CenterPos.X - pRect.Width / 2) * (float)dRatio;
            float startY = (float)(pRect.CenterPos.Y - pRect.Height / 2) * (float)dRatio;
            using (Graphics graph = Graphics.FromImage(m_pBitmap))
            {
                SolidBrush brush = new SolidBrush(fillColor);
                graph.FillRectangle(brush, startX, startY, (float)pRect.Width, (float)pRect.Height);
            }
        }

        public void FillRect(YoonRect2D pRect, Color fillColor, double dRatio = 1.0)
        {
            float startX = (float)(pRect.CenterPos.X - pRect.Width / 2) * (float)dRatio;
            float startY = (float)(pRect.CenterPos.Y - pRect.Height / 2) * (float)dRatio;
            using (Graphics graph = Graphics.FromImage(m_pBitmap))
            {
                SolidBrush brush = new SolidBrush(fillColor);
                graph.FillRectangle(brush, startX, startY, (float)pRect.Width, (float)pRect.Height);
            }
        }

        public void FillRect(int centerX, int centerY, int width, int height, Color fillColor, double zoom)
        {
            float startX = (float)(centerX - width / 2) * (float)zoom;
            float startY = (float)(centerY - height / 2) * (float)zoom;
            using (Graphics graph = Graphics.FromImage(m_pBitmap))
            {
                SolidBrush brush = new SolidBrush(fillColor);
                graph.FillRectangle(brush, startX, startY, (float)width, (float)height);
            }
        }

        public void FillPoligon(YoonVector2N[] pArrayPoint, Color fillColor, double dRatio = 1.0)
        {
            PointF[] pArrayDraw = new PointF[pArrayPoint.Length];
            for (int iPoint = 0; iPoint < pArrayPoint.Length; iPoint++)
            {
                pArrayDraw[iPoint].X = (float)pArrayPoint[iPoint].X * (float)dRatio;
                pArrayDraw[iPoint].Y = (float)pArrayPoint[iPoint].Y * (float)dRatio;
            }

            using (Graphics graph = Graphics.FromImage(m_pBitmap))
            {
                SolidBrush brush = new SolidBrush(fillColor);
                graph.FillPolygon(brush, pArrayDraw);
            }
        }

        public void FillCanvas(Color fillColor)
        {
            Rectangle pRectCanvas = new Rectangle(0, 0, m_pBitmap.Width, m_pBitmap.Height);
            using (Graphics graph = Graphics.FromImage(m_pBitmap))
            {
                SolidBrush brush = new SolidBrush(fillColor);
                Region pRegion = new Region(pRectCanvas);
                graph.FillRegion(brush, pRegion);
            }
        }

        public void DrawTriangle(int x, int y, int size, eYoonDir2D direction, Color penColor, int penWidth, double zoom)
        {
            PointF[] pIYoonVector = new PointF[3];
            pIYoonVector[0].X = (float)(x * zoom);
            pIYoonVector[0].Y = (float)(y * zoom);
            switch (direction)
            {
                case eYoonDir2D.Top:
                    pIYoonVector[1].X = pIYoonVector[0].X + (float)(size / 2 * zoom);
                    pIYoonVector[1].Y = pIYoonVector[0].Y + (float)(size * zoom);
                    pIYoonVector[2].X = pIYoonVector[0].X - (float)(size / 2 * zoom);
                    pIYoonVector[2].Y = pIYoonVector[0].Y + (float)(size * zoom);
                    break;
                case eYoonDir2D.Bottom:
                    pIYoonVector[1].X = pIYoonVector[0].X - (float)(size / 2 * zoom);
                    pIYoonVector[1].Y = pIYoonVector[0].Y - (float)(size * zoom);
                    pIYoonVector[2].X = pIYoonVector[0].X + (float)(size / 2 * zoom);
                    pIYoonVector[2].Y = pIYoonVector[0].Y - (float)(size * zoom);
                    break;
                case eYoonDir2D.Left:
                    pIYoonVector[1].X = pIYoonVector[0].X + (float)(size * zoom);
                    pIYoonVector[1].Y = pIYoonVector[0].Y - (float)(size / 2 * zoom);
                    pIYoonVector[2].X = pIYoonVector[0].X + (float)(size * zoom);
                    pIYoonVector[2].Y = pIYoonVector[0].Y + (float)(size / 2 * zoom);
                    break;
                case eYoonDir2D.Right:
                    pIYoonVector[1].X = pIYoonVector[0].X - (float)(size * zoom);
                    pIYoonVector[1].Y = pIYoonVector[0].Y + (float)(size / 2 * zoom);
                    pIYoonVector[2].X = pIYoonVector[0].X - (float)(size * zoom);
                    pIYoonVector[2].Y = pIYoonVector[0].Y - (float)(size / 2 * zoom);
                    break;
            }
            using (Graphics graph = Graphics.FromImage(m_pBitmap))
            {
                Pen pen = new Pen(penColor, (float)penWidth);
                graph.DrawLine(pen, pIYoonVector[0], pIYoonVector[1]);
                graph.DrawLine(pen, pIYoonVector[1], pIYoonVector[2]);
                graph.DrawLine(pen, pIYoonVector[2], pIYoonVector[0]);
            }
        }

        public void DrawRect(YoonRect2N pRect, Color penColor, int nPenWidth = 1, double dRatio = 1.0)
        {
            if (pRect.Right <= pRect.Left || pRect.Bottom <= pRect.Top)
                return;
            DrawLine((YoonVector2N)pRect.TopLeft, (YoonVector2N)pRect.TopRight, penColor, nPenWidth, dRatio);
            DrawLine((YoonVector2N)pRect.TopRight, (YoonVector2N)pRect.BottomRight, penColor, nPenWidth, dRatio);
            DrawLine((YoonVector2N)pRect.BottomLeft, (YoonVector2N)pRect.BottomRight, penColor, nPenWidth, dRatio);
            DrawLine((YoonVector2N)pRect.TopLeft, (YoonVector2N)pRect.BottomLeft, penColor, nPenWidth, dRatio);
        }

        public void DrawRect(int centerX, int centerY, int width, int height, Color penColor, int nPenWidth, double dRatio)
        {
            if (width <= 0 || height <= 0)
                return;
            DrawLine(centerX - width / 2, centerY - height / 2, centerX + width / 2, centerY - height / 2, penColor, nPenWidth, dRatio);
            DrawLine(centerX + width / 2, centerY - height / 2, centerX + width / 2, centerY + height / 2, penColor, nPenWidth, dRatio);
            DrawLine(centerX - width / 2, centerY + height / 2, centerX + width / 2, centerY + height / 2, penColor, nPenWidth, dRatio);
            DrawLine(centerX - width / 2, centerY - height / 2, centerX - width / 2, centerY + height / 2, penColor, nPenWidth, dRatio);
        }

        public void DrawLine(YoonVector2N vecPos1, YoonVector2N vecPos2, Color penColor, int penWidth, double dRatio = 1.0)
        {
            double deltaX, deltaY, deltaX1, deltaY1;
            deltaX = vecPos1.X * dRatio;
            deltaY = vecPos1.Y * dRatio;
            deltaX1 = vecPos2.X * dRatio;
            deltaY1 = vecPos2.Y * dRatio;
            using (Graphics graph = Graphics.FromImage(m_pBitmap))
            {
                Pen pen = new Pen(penColor, penWidth);
                graph.DrawLine(pen, new PointF((float)Math.Round(deltaX), (float)Math.Round(deltaY)), new PointF((float)Math.Round(deltaX1), (float)Math.Round(deltaY1)));
            }
        }

        public void DrawLine(int x, int y, int x1, int y1, Color penColor, int penWidth, double zoom)
        {
            double deltaX, deltaY, deltaX1, deltaY1;
            deltaX = (double)x * zoom;
            deltaY = (double)y * zoom;
            deltaX1 = (double)x1 * zoom;
            deltaY1 = (double)y1 * zoom;
            using (Graphics graph = Graphics.FromImage(m_pBitmap))
            {
                Pen pen = new Pen(penColor, penWidth);
                graph.DrawLine(pen, new PointF((float)Math.Round(deltaX), (float)Math.Round(deltaY)), new PointF((float)Math.Round(deltaX1), (float)Math.Round(deltaY1)));
            }
        }

        public void DrawText(YoonVector2N vecPos, Color fontColor, string text, int fontSize = 8,  double dRatio = 1.0)
        {
            float deltaX, deltaY, size;
            deltaX = (float)(vecPos.X * dRatio);
            deltaY = (float)(vecPos.Y * dRatio);
            size = (float)fontSize;
            if (size < 10) size = 10;
            using (Graphics graph = Graphics.FromImage(m_pBitmap))
            {
                Brush brush = new SolidBrush(fontColor);
                FontFamily fontFamily = new FontFamily("Tahoma");
                Font font = new Font(fontFamily, size, FontStyle.Regular, GraphicsUnit.Pixel);
                graph.DrawString(text, font, brush, deltaX, deltaY);
            }
        }

        public void DrawCross(YoonVector2N vecPos, Color penColor, int size, int penWidth = 1, double dRatio = 1.0)
        {
            float deltaX, deltaY;
            float x1, x2, y1, y2;
            deltaX = (float)(vecPos.X * dRatio);
            deltaY = (float)(vecPos.Y * dRatio);
            x1 = deltaX - size;
            x2 = deltaX + size;
            y1 = deltaY - size;
            y2 = deltaY + size;
            using (Graphics graph = Graphics.FromImage(m_pBitmap))
            {
                Pen pen = new Pen(penColor, (float)penWidth);
                graph.DrawLine(pen, new PointF(x1, deltaY), new PointF(x2, deltaY));
                graph.DrawLine(pen, new PointF(deltaX, y1), new PointF(deltaX, y2));
            }
        }

        public void DrawEllipse(YoonRect2N rect, Color penColor, int penWidth = 1, double dRatio = 1.0)
        {
            int x1, y1, x2, y2;
            x1 = (int)Math.Round(rect.Left * dRatio);
            y1 = (int)Math.Round(rect.Top * dRatio);
            x2 = (int)Math.Round(rect.Right * dRatio);
            y2 = (int)Math.Round(rect.Bottom * dRatio);
            using (Graphics graph = Graphics.FromImage(m_pBitmap))
            {
                Pen pen = new Pen(penColor, (float)penWidth);
                graph.DrawEllipse(pen, x1, y1, (x2 - x1), (y2 - y1));
            }
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
                    pByte[iX] = (byte)Math.Max(0, Math.Min(GetGrayPixel(iX, iY) + offset, 255));
                }
                if (!SetGrayLine(pByte, iY))
                    bResult = false;
            }
            return bResult;
        }

        public int[] GetGrayHistogram()
        {
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap size is not normal");
            if (m_pBitmap.PixelFormat != PixelFormat.Format8bppIndexed)
                throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            int[] pHistogram = new int[256];
            Array.Clear(pHistogram, 0, pHistogram.Length);
            ////  Histogram 그래프를 만든다.
            for (int iY = 0; iY < m_pBitmap.Height; iY++)
            {
                for (int iX = 0; iX < m_pBitmap.Width; iX++)
                {
                    byte value = GetGrayPixel(iX, iY);
                    if (value > 255 || value < 0)
                        continue;
                    pHistogram[value]++;
                }
            }
            return pHistogram;
        }

        public int[] GetRedHistogram()
        {
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap size is not normal");
            if (m_pBitmap.PixelFormat != PixelFormat.Format32bppArgb && m_pBitmap.PixelFormat != PixelFormat.Format24bppRgb)
                throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            int[] pHistogram = new int[256];
            Array.Clear(pHistogram, 0, pHistogram.Length);
            ////  Histogram 그래프를 만든다.
            for (int iY = 0; iY < m_pBitmap.Height; iY++)
            {
                for (int iX = 0; iX < m_pBitmap.Width; iX++)
                {
                    byte value = GetRedPixel(iX, iY);
                    if (value > 255 || value < 0)
                        continue;
                    pHistogram[value]++;
                }
            }
            return pHistogram;
        }

        public int[] GetBlueHistogram()
        {
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap size is not normal");
            if (m_pBitmap.PixelFormat != PixelFormat.Format32bppArgb && m_pBitmap.PixelFormat != PixelFormat.Format24bppRgb)
                throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            int[] pHistogram = new int[256];
            Array.Clear(pHistogram, 0, pHistogram.Length);
            ////  Histogram 그래프를 만든다.
            for (int iY = 0; iY < m_pBitmap.Height; iY++)
            {
                for (int iX = 0; iX < m_pBitmap.Width; iX++)
                {
                    byte value = GetBluePixel(iX, iY);
                    if (value > 255 || value < 0)
                        continue;
                    pHistogram[value]++;
                }
            }
            return pHistogram;
        }

        public int[] GetGreenHistogram()
        {
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap size is not normal");
            if (m_pBitmap.PixelFormat != PixelFormat.Format32bppArgb && m_pBitmap.PixelFormat != PixelFormat.Format24bppRgb)
                throw new FormatException("[YOONIMAGE EXCEPTION] Pixel format isnot correct");
            int[] pHistogram = new int[256];
            Array.Clear(pHistogram, 0, pHistogram.Length);
            ////  Histogram 그래프를 만든다.
            for (int iY = 0; iY < m_pBitmap.Height; iY++)
            {
                for (int iX = 0; iX < m_pBitmap.Width; iX++)
                {
                    byte value = GetGreenPixel(iX, iY);
                    if (value > 255 || value < 0)
                        continue;
                    pHistogram[value]++;
                }
            }
            return pHistogram;
        }

        public byte[] GetGrayBuffer()
        {
            return Scan8bitBuffer(new Rectangle(Point.Empty, m_pBitmap.Size));
        }

        public byte[] GetGrayLine(int nPixelY)
        {
            return Scan8bitBuffer(new Rectangle(0, nPixelY, m_pBitmap.Width, 1));
        }

        public byte GetGrayPixel(int nPixelX, int nPixelY)
        {
            return Scan8bitBuffer(new Rectangle(nPixelX, nPixelY, 1, 1))[0];
        }

        public byte GetGrayPixel(YoonVector2N pVec)
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

        private byte GetPlanePixel(int nPlane, int nPixelX, int nPixelY)
        {
            switch (m_pBitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return Scan24bitBufferPerPlane(new Rectangle(nPixelX, nPixelY, 1, 1), nPlane)[0];
                case PixelFormat.Format32bppArgb:
                    return Scan32bitBufferPerPlane(new Rectangle(nPixelX, nPixelY, 1, 1), nPlane)[0];
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

        public byte GetRedPixel(int nPixelX, int nPixelY)
        {
            switch (m_pBitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return GetPlanePixel(nPixelX, nPixelY, 0); // 0 : Red
                case PixelFormat.Format32bppArgb:
                    return GetPlanePixel(nPixelX, nPixelY, 1); // 1 : Red
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

        public byte GetGreenPixel(int nPixelX, int nPixelY)
        {
            switch (m_pBitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return GetPlanePixel(nPixelX, nPixelY, 1); // 1 : Green
                case PixelFormat.Format32bppArgb:
                    return GetPlanePixel(nPixelX, nPixelY, 2); // 2 : Green
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

        public byte GetBluePixel(int nPixelX, int nPixelY)
        {
            switch (m_pBitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return GetPlanePixel(nPixelX, nPixelY, 2); // 2 : Blue
                case PixelFormat.Format32bppArgb:
                    return GetPlanePixel(nPixelX, nPixelY, 3); // 3 : Blue
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

        public bool SetGrayImage(IntPtr pBufferAddress)
        {
            if (pBufferAddress == IntPtr.Zero)
                throw new ArgumentNullException("[YOONIMAGE EXCEPTION] Address is null");
            if (m_pBitmap.Width <= 0 || m_pBitmap.Height <= 0)
                throw new IndexOutOfRangeException("[YOONIMAGE ERROR] Bitmap size is not normal");

            byte[] pBuffer = new byte[m_pBitmap.Width * m_pBitmap.Height];
            Marshal.Copy(pBufferAddress, pBuffer, 0, m_pBitmap.Width * m_pBitmap.Height);
            return SetGrayImage(pBuffer);
        }

        public bool SetGrayImage(byte[] pBuffer)
        {
            return Print8bitBuffer(pBuffer, new Rectangle(Point.Empty, m_pBitmap.Size));
        }

        public bool SetGrayLine(byte[] pBuffer, int nPixelY)
        {
            return Print8bitBuffer(pBuffer, new Rectangle(0, nPixelY, m_pBitmap.Width, 1));
        }

        public bool SetGrayPixel(byte nLevel, int nPixelX, int nPixelY)
        {
            return Print8bitBuffer(new byte[1] { nLevel }, new Rectangle(nPixelX, nPixelY, 1, 1));
        }

        public bool SetGrayPixel(byte nLevel, YoonVector2N pVec)
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

        public static YoonImage operator +(YoonImage i1, YoonImage i2)
        {
            return ImageFactory.Add(i1, i2);
        }

        public static YoonImage operator -(YoonImage i1, YoonImage i2)
        {
            return ImageFactory.Subtract(i1, i2);
        }

        public static bool operator ==(YoonImage i1, YoonImage i2)
        {
            return i1.Equals(i2) == true;
        }

        public static bool operator !=(YoonImage i1, YoonImage i2)
        {
            return i1.Equals(i2) == false;
        }
    }
}