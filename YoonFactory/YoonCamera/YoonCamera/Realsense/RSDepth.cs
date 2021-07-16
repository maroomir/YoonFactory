using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace YoonFactory.Camera.Realsense
{
    public class RSDepth : IYoonDepth, IYoonDepth<short>
    {
        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                if (Buffer != null)
                    Clear();

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        ~RSDepth()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(false);
        }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            GC.SuppressFinalize(this);
        }
        #endregion

        public RSDepth()
        {
            Width = 0;
            Height = 0;
            Buffer = null;
        }

        public RSDepth(int nWidth, int nHeight)
        {
            Width = nWidth;
            Height = nHeight;
            Buffer = new short[Width * Height];
        }

        public RSDepth(IntPtr pBuffer, int nWidth, int nHeight)
        {
            Width = nWidth;
            Height = nHeight;
            Buffer = new short[Width * Height];
            Marshal.Copy(pBuffer, Buffer, 0, Width * Height);
        }

        public IYoonDepth Clone()
        {
            RSDepth d = new RSDepth();
            d.Width = Width;
            d.Height = Height;
            if (Buffer != null)
            {
                d.Buffer = new short[Width * Height];
                Array.Copy(Buffer, d.Buffer, Width * Height);
            }
            return d;
        }

        public void CopyFrom(IYoonDepth d)
        {
            if (Buffer != null) Clear();

            if (d is RSDepth pDepth)
            {
                Width = pDepth.Width;
                Height = pDepth.Height;
                if (pDepth.Buffer != null)
                {
                    Buffer = new short[Width * Height];
                    Array.Copy(pDepth.Buffer, Buffer, Width * Height);
                }
            }
        }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public short[] Buffer { get; protected set; }

        public void Clear()
        {
            if (Buffer == null) return;
            Array.Clear(Buffer, 0, Width * Height);
            Buffer = null;
        }

        public void SetBuffer(IntPtr pBuffer, int nWidth, int nHeight)
        {
            if (Buffer != null) Clear();

            Width = nWidth;
            Height = nHeight;
            Marshal.Copy(pBuffer, Buffer, 0, Width * Height);
        }

        public void SetBuffer(IntPtr pBuffer)
        {
            if (Buffer != null) Clear();

            if (Width > 0 && Height > 0)
            {
                Buffer = new short[Width * Height];
                Marshal.Copy(pBuffer, Buffer, 0, Width * Height);
            }
        }

        public int SignificantCount
        {
            get
            {
                int nCount = 0;
                for (int i = 0; i < Buffer.Length; i++)
                {
                    if (Buffer[i] > 0)
                        nCount++;
                }
                return nCount;
            }
        }

        public short Center
        {
            get => GetDepth(Width / 2, Height / 2);
        }

        public short Average
        {
            get
            {
                int nCount = 0;
                long nSum = 0;
                for (int i = 0; i < Buffer.Length; i++)
                {
                    if (Buffer[i] > 0)
                    {
                        nSum += Buffer[i];
                        nCount++;
                    }
                }
                if (nCount == 0) return 0;
                else return (short)(nSum / nCount);
            }
        }

        public short GetAverage(int nStartX, int nStartY, int nWidth, int nHeight)
        {
            if (nWidth < 0 || nWidth >= Width || nHeight < 0 || nHeight >= Height ||
                nStartX < 0 || nStartX >= Width || nStartY < 0 || nStartY >= Height)
                return 0;

            int nCount = 0;
            long nSum = 0;
            for (int iY = 0; iY < Height; iY++)
            {
                if (iY < nStartY || iY >= nStartY + nHeight) continue;
                for (int iX = 0; iX < Width; iX++)
                {
                    if (iX < nStartX || iX >= nStartX + nWidth) continue;
                    if (GetDepth(iX, iY) > 0)
                    {
                        nSum += GetDepth(iX, iY);
                        nCount++;
                    }
                }
            }
            if (nCount == 0) return 0;
            else return (short)(nSum / nCount);
        }

        public short GetAverage(YoonVector2N vecStartPos, int nWidth, int nHeight)
        {
            return GetAverage(vecStartPos.X, vecStartPos.Y, nWidth, nHeight);
        }

        public short GetDepth(int nX, int nY)
        {
            if (nX < 0 || nX >= Width || nY < 0 || nY >= Height)
                return 0;
            else
                return Buffer[nY * Width + nX];
        }

        public short GetDepth(YoonVector2N vecPos)
        {
            return GetDepth(vecPos.X, vecPos.Y);
        }

        public void SetDepth(short nValue, int nX, int nY)
        {
            if (Buffer == null) return;
            if (nX < 0 || nX >= Width || nY < 0 || nY >= Height)
                return;

            Buffer[nY * Width + nX] = nValue;
        }

        public void SetDepth(short nValue, YoonVector2N vecPos)
        {
            SetDepth(nValue, vecPos.X, vecPos.Y);
        }

        public int OverCount(short nValue, bool bIncludeThreshold = false)
        {
            int nCount = 0;
            for (int i = 0; i < Buffer.Length; i++)
            {
                if (Buffer[i] == 0) continue;

                if (bIncludeThreshold && Buffer[i] >= nValue) nCount++;
                else if (Buffer[i] > nValue) nCount++;
            }
            return nCount;
        }

        public int UnderCount(short nValue, bool bIncludeThreshold = false)
        {
            int nCount = 0;
            for (int i = 0; i < Buffer.Length; i++)
            {
                if (Buffer[i] == 0) continue;

                if (bIncludeThreshold && Buffer[i] <= nValue) nCount++;
                else if (Buffer[i] < nValue) nCount++;
            }
            return nCount;
        }

        public int EqualCount(short nValue)
        {
            int nCount = 0;
            for (int i = 0; i < Buffer.Length; i++)
            {
                if (Buffer[i] == nValue) nCount++;
            }
            return nCount;
        }

        public IYoonDepth Crop(int nStartX, int nStartY, int nWidth, int nHeight)
        {
            if (nWidth < 0 || nWidth >= Width || nHeight < 0 || nHeight >= Height ||
                nStartX < 0 || nStartX >= Width || nStartY < 0 || nStartY >= Height)
                return new RSDepth();

            RSDepth pCrop = new RSDepth();
            pCrop.Width = (nStartX + nWidth < Width) ? nWidth : Width - nStartX;
            pCrop.Height = (nStartY + nHeight < Height) ? nHeight : Height - nStartY;
            pCrop.Buffer = new short[pCrop.Width * pCrop.Height];
            int iCursorX = 0, iCursorY = 0;
            for (int iY = 0; iY < Height; iY++)
            {
                if (iY < nStartY || iY >= nStartY + pCrop.Height) continue;
                iCursorX = 0;
                for (int iX = 0; iX < Width; iX++)
                {
                    if (iX < nStartX || iX >= nStartX + pCrop.Width) continue;
                    pCrop.Buffer[iCursorY * pCrop.Width + iCursorX] = GetDepth(iX, iY);
                    iCursorX++;
                }
                iCursorY++;
            }
            return pCrop;
        }

        public IYoonDepth Crop(YoonVector2N vecStartPos, int nWidth, int nHeight)
        {
            return Crop(vecStartPos.X, vecStartPos.Y, nWidth, nHeight);
        }

        public List<int[]> GetDepthScale(float dScaleSensor, int nPixelResolution)
        {
            if (nPixelResolution == 0 || dScaleSensor == 0.0) return null;
            if (Width % nPixelResolution != 0 || Height % nPixelResolution != 0) return null;
            List<int[]> pListDepthScale = new List<int[]>();

            for (int iRow = 0; iRow < Height / nPixelResolution; iRow++)
            {
                int[] pArrayDepthScaleRow = new int[Width / nPixelResolution];
                for (int iCol = 0; iCol < Width / nPixelResolution; iCol++)
                {
                    int nDepthScaling = 0;
                    int nCountPartial = 0;
                    for (int jY = iRow * nPixelResolution; jY < (iRow + 1) * nPixelResolution; jY++)
                    {
                        for (int jX = iCol * nPixelResolution; jX < (iCol + 1) * nPixelResolution; jX++)
                        {
                            short nDepthData = GetDepth(jX, jY);
                            nDepthScaling += nDepthData;
                            nCountPartial++;
                        }
                    }
                    pArrayDepthScaleRow[iCol] = nDepthScaling / nCountPartial;
                }
                pListDepthScale.Add(pArrayDepthScaleRow);
            }
            return pListDepthScale;
        }

        public byte[] GetDepthScale(ref float dValueResolution, float dScaleSensor, int nPixelResolution)
        {
            if (nPixelResolution == 0 || dScaleSensor == 0.0) return null;
            if (Width % nPixelResolution != 0 || Height % nPixelResolution != 0) return null;

            float dEffectRange = YoonRealsense.MAX_DEPTH_DIST / dScaleSensor;
            dValueResolution = dEffectRange / byte.MaxValue;
            if (dValueResolution == 0) return null;

            byte[] pBuffer = new byte[(Width / nPixelResolution) * (Height / nPixelResolution)];

            for (int iRow = 0; iRow < Height / nPixelResolution; iRow++)
            {
                for (int iCol = 0; iCol < Width / nPixelResolution; iCol++)
                {
                    int nDepthScaling = 0;
                    int nCountPartial = 0;
                    for (int jY = iRow * nPixelResolution; jY < (iRow + 1) * nPixelResolution; jY++)
                    {
                        for (int jX = iCol * nPixelResolution; jX < (iCol + 1) * nPixelResolution; jX++)
                        {
                            short nDepthData = GetDepth(jX, jY);
                            if (nDepthData > 0 && nDepthData < dEffectRange) nDepthScaling += nDepthData;
                            else if (nDepthData >= dEffectRange) nDepthScaling += (short)dEffectRange;
                            else continue;
                            nCountPartial++;
                        }
                    }
                    if (nCountPartial > 0)
                        pBuffer[iRow * (Width / nPixelResolution) + iCol] = (byte)((nDepthScaling / nCountPartial) / dValueResolution);
                }
            }
            return pBuffer;
        }
    }
}