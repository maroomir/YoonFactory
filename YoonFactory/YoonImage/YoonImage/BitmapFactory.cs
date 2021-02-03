using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace YoonFactory.Image
{
    /* RAW Image 정보를 Bitmap으로 변환시키는 Class */
    public static class BitmapFactory
    {
        /// <summary>
        /// Bitmap을 구성하는 Pixel Format 종류를 구한다.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static PixelFormat GetFormat(int plane)
        {
            switch (plane)
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
                    return PixelFormat.Undefined;
            }
        }
        /// <summary>
        /// 1개 Byte 당 Line 수를 계산한다. (RGBA 또는 BW)
        /// </summary>
        /// <param name="width"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private static int GetStride(int width, int plane)
        {
            return (GetFormat(plane) == PixelFormat.Undefined) ? 0 : plane * width;
        }
        /// <summary>
        /// bitmap data의 속성이 현재 설정된 width, color에 호환 가능한지 확인한다.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static bool IsCompatible(Bitmap bitmap, int width, int height, int plane)
        {
            if (bitmap == null || bitmap.Height != height || bitmap.Width != width || bitmap.PixelFormat != GetFormat(plane))
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 설정된 속성을 부여해서 새로운 Bitmap을 만든다.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public static void CreateBitmap(out Bitmap bitmap, int width, int height, int plane)
        {
            if (width == 0 || height == 0)
            {
                width = height = 640;
            }
            bitmap = new Bitmap(width, height, GetFormat(plane));
            InitBitmap(ref bitmap);
        }

        /// <summary>
        /// 새로 만들어진 Bitmap을 초기화시킨다.
        /// Create Bitmap 메서드와 분리해서 Bitmap 및 Color Palette간의 Memory 충돌 발생시 대처를 빠르게 한다.
        /// </summary>
        /// <param name="bitmap"></param>
        private static void InitBitmap(ref Bitmap bitmap)
        {
            ColorPalette colorPalette;
            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    colorPalette = bitmap.Palette;
                    for (int i = 0; i < 256; i++)
                    {
                        colorPalette.Entries[i] = Color.FromArgb(i, i, i);
                    }
                    bitmap.Palette = colorPalette;
                    break;
                case PixelFormat.Format16bppGrayScale:
                    break;
                case PixelFormat.Format24bppRgb:
                    break;
                case PixelFormat.Format32bppArgb:
                    break;
            }
        }
        /// <summary>
        /// Raw Data를 Bitmap으로 복사한다.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="buffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public static void UpdateBitmap(Bitmap bitmap, byte[] buffer, int width, int height, int plane)
        {
            //// Image Data와 Bitmap의 속성이 호환되는지 확인한다.
            if (!IsCompatible(bitmap, width, height, plane))
            {
                throw new Exception("Cannot update incompatible bitmap.");
            }

            //// Bit가 접근 불가로 잠겨진 Bitmap을 만든다.
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            //// 만들어진 Bitmap의 첫째열 주소를 가져온다.
            IntPtr ptrBmp = bmpData.Scan0;
            //// 주어진 Color 별로 Image Data의 Byte 크기를 확장한다.
            int imageStride = GetStride(width, plane);
            //// Pixel 당 Byte 크기로 판별한 Bitmap 속성이 같은 경우, 직접 복사한다. 
            if (imageStride == bmpData.Stride)
            {
                Marshal.Copy(buffer, 0, ptrBmp, bmpData.Stride * bitmap.Height);
            }
            else //// Byte 크기가 다른 경우 해당되는 Width에 맞게 변환해서 복사한다.
            {
                for (int i = 0; i < bitmap.Height; ++i)
                {
                    Marshal.Copy(buffer, i * imageStride, new IntPtr(ptrBmp.ToInt64() + i * bmpData.Stride), width);
                }
            }
            //// Bitmap의 Bit별 잠금을 해제한다.
            bitmap.UnlockBits(bmpData);
        }
        /// <summary>
        /// Raw Data를 Bitmap으로 복사한다.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="pImageBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="plane"></param>
        public static void UpdateBitmap(Bitmap bitmap, IntPtr pImageBuffer, int width, int height, int plane)
        {
            //// Image Data와 Bitmap의 속성이 호환되는지 확인한다.
            if (!IsCompatible(bitmap, width, height, plane))
            {
                throw new Exception("Cannot update incompatible bitmap.");
            }

            //// Bit가 접근 불가로 잠겨진 Bitmap을 만든다.
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            //// 만들어진 Bitmap의 첫째열 주소를 가져온다.
            IntPtr ptrBmp = bmpData.Scan0;
            //// 주어진 Color 별로 Image Data의 Byte 크기를 확장한다.
            int imageStride = GetStride(width, plane);
            //// 가져온 IntPtr 값을 바탕으로 byte buffer를 생성한다.
            byte[] buffer = new byte[bmpData.Stride * bitmap.Height];
            Marshal.Copy(pImageBuffer, buffer, 0, bmpData.Stride * bitmap.Height);
            //// Pixel 당 Byte 크기로 판별한 Bitmap 속성이 같은 경우, 직접 복사한다. 
            if (imageStride == bmpData.Stride)
            {
                Marshal.Copy(buffer, 0, ptrBmp, bmpData.Stride * bitmap.Height);
            }
            else //// Byte 크기가 다른 경우 해당되는 Width에 맞게 변환해서 복사한다.
            {
                for (int i = 0; i < bitmap.Height; ++i)
                {
                    Marshal.Copy(buffer, i * imageStride, new IntPtr(ptrBmp.ToInt64() + i * bmpData.Stride), width);
                }
            }
            //// Bitmap의 Bit별 잠금을 해제한다.
            bitmap.UnlockBits(bmpData);
        }
        /// <summary>
        /// Bitmap의 Bit Per Pixel 값을 변환해서 반환한다.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap Convert32BppBitmap(Bitmap bitmap)
        {
            return ConvertBitmap(bitmap, PixelFormat.Format32bppArgb);
        }
        /// <summary>
        /// Bitmap의 Bit Per Pixel 값을 변환해서 반환한다.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap Convert24BppBitmap(Bitmap bitmap)
        {
            return ConvertBitmap(bitmap, PixelFormat.Format24bppRgb);
        }
        /// <summary>
        /// Bitmap의 Bit Per Pixel 값을 변환해서 반환한다.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap Convert8BppBitmap(Bitmap bitmap)
        {
            return ConvertBitmap(bitmap, PixelFormat.Format8bppIndexed);
        }
        /// <summary>
        /// Bitmap의 Bit Per Pixel 값을 변환해서 반환한다.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static Bitmap ConvertBitmap(Bitmap bitmap, PixelFormat format)
        {
            Bitmap resulBitmap = new Bitmap(bitmap.Width, bitmap.Height, format);
            using (Graphics graph = Graphics.FromImage(resulBitmap))
            {
                graph.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
            }
            return resulBitmap;
        }
        /// <summary>
        /// Bitmap에서 Bitmap으로 복사한다.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap CopyBitmap(Bitmap bitmap)
        {
            Bitmap resultBitmap = new Bitmap(bitmap, bitmap.Width, bitmap.Height);
            using (Graphics graph = Graphics.FromImage(resultBitmap))
            {
                graph.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
            }
            return resultBitmap;
        }

        /* Bitmap Image의 Memory 영역 관리를 담당하는 Class */
        public static class Memory
        {
            // Bitmap Data를 보관하는 Dictionary
            private static BitmapData pImageData = null;

            // Image로부터 Read/Write가 가능한 Memory Pointer를 가져온다.
            private static IntPtr GetMemory(ref Bitmap pImage)
            {
                return GetMemory(ref pImage, new Rectangle(Point.Empty, pImage.Size), ImageLockMode.ReadWrite);
            }

            // Image로부터 Mode에 맞는 작업을 수행 할 수 있는 Memory Pointer를 가져온다.
            private static IntPtr GetMemory(ref Bitmap pImage, ImageLockMode mode)
            {
                return GetMemory(ref pImage, new Rectangle(Point.Empty, pImage.Size), mode);
            }

            // Image로부터 Rect영역을 Read/Write 할 수 있는 Memory Pointer를 가져온다.
            private static IntPtr GetMemory(ref Bitmap pImage, Rectangle rect)
            {
                return GetMemory(ref pImage, rect, ImageLockMode.ReadWrite);
            }

            // Image로부터 Rect영역을 Mode에 맞게 작업을 수행 할 수 있는 Memory Pointer를 가져온다.
            private static IntPtr GetMemory(ref Bitmap pImage, Rectangle rect, ImageLockMode mode)
            {
                pImageData = pImage.LockBits(rect, mode, pImage.PixelFormat);
                return pImageData.Scan0;
            }

            // Lock Memory 메소드로 취득한 Memory 주소를 반환한다.
            private static void ReleaseMemory(ref Bitmap pImage)
            {
                try
                {
                    pImage.UnlockBits(pImageData);
                }
                catch (Exception)
                {
                    //
                }
            }

            #region Image 입출력
            // Image 전체의 Memory 배열을 복사해온다.
            public static byte[] Scan8bitImage(ref Bitmap pImage)
            {
                if (pImage.PixelFormat != PixelFormat.Format8bppIndexed) return null;
                return Scan8bitImage(ref pImage, new Rectangle(Point.Empty, pImage.Size));
            }

            // Image에서 단행의 Memory 배열을 복사해온다.
            public static byte[] Scan8bitLine(ref Bitmap pImage, int pixelY)
            {
                if (pImage.PixelFormat != PixelFormat.Format8bppIndexed) return null;
                return Scan8bitImage(ref pImage, new Rectangle(0, pixelY, pImage.Width, 1));
            }

            // Image에서 1개 Point의 Gray Level을 복사해온다.
            public static byte Scan8bitPoint(ref Bitmap pImage, int pixelX, int pixelY)
            {
                if (pImage.PixelFormat != PixelFormat.Format8bppIndexed) return 0;
                return Scan8bitImage(ref pImage, new Rectangle(pixelX, pixelY, 1, 1))[0];
            }

            // Image에서 Rect영역에 있는 Memroy를 복사해온다.
            private static byte[] Scan8bitImage(ref Bitmap pImage, Rectangle rect)
            {
                if (pImage.PixelFormat != PixelFormat.Format8bppIndexed ||
                    rect.X > pImage.Width || rect.Y > pImage.Height)
                    return null;

                byte[] pByte = new byte[rect.Width * rect.Height];
                IntPtr pImagePointer = GetMemory(ref pImage, rect, ImageLockMode.ReadOnly);
                Marshal.Copy(pImagePointer, pByte, 0, pByte.Length);
                ReleaseMemory(ref pImage);
                return pByte;
            }

            // Image 전체의 Memory 배열을 복사해온다.
            public static byte[] Scan24bitImageByPlane(ref Bitmap pImage, int nPlane)
            {
                if (pImage.PixelFormat != PixelFormat.Format24bppRgb) return null;
                return Scan24bitImageByPlane(ref pImage, nPlane, new Rectangle(Point.Empty, pImage.Size));
            }

            // Image 전체의 Memory 배열을 복사해온다.
            public static int[] Scan24bitImage(ref Bitmap pImage)
            {
                if (pImage.PixelFormat != PixelFormat.Format24bppRgb) return null;
                return Scan24bitImage(ref pImage, new Rectangle(Point.Empty, pImage.Size));
            }

            // Image에서 단행의 Memory 배열을 복사해온다.
            public static int[] Scan24bitLine(ref Bitmap pImage, int pixelY)
            {
                if (pImage.PixelFormat != PixelFormat.Format24bppRgb) return null;
                return Scan24bitImage(ref pImage, new Rectangle(0, pixelY, pImage.Width, 1));
            }

            // Image에서 1개 Point의 Gray Level을 복사해온다.
            public static int Scan24bitPoint(ref Bitmap pImage, int pixelX, int pixelY)
            {
                if (pImage.PixelFormat != PixelFormat.Format24bppRgb) return 0;
                return Scan24bitImage(ref pImage, new Rectangle(pixelX, pixelY, 1, 1))[0];
            }

            // Image에서 Rect영역에 있는 Memroy를 복사해온다.
            // MSDN을 참고하면 int 15를 BitConvert에 통과시킬 때 {0xFF, 0x00, 0x00, 0x00}로 나옴
            // 즉 byte[]에서 int 값으로 컨버팅할 때 B / G / R / A 순서로 넣어야함
            private static int[] Scan24bitImage(ref Bitmap pImage, Rectangle rect)
            {
                if (pImage.PixelFormat != PixelFormat.Format24bppRgb ||
                        rect.X > pImage.Width || rect.Y > pImage.Height)
                    return null;

                int[] pPixel = new int[rect.Width * rect.Height];
                IntPtr pImagePointer = GetMemory(ref pImage, rect, ImageLockMode.ReadOnly);
                Marshal.Copy(pImagePointer, pPixel, 0, pPixel.Length);
                /*
                for (int j = 0; j < rect.Height; j++)
                {
                    for (int i = 0; i < rect.Width; i++)
                    {
                        byte[] pBytePixel = new byte[4]; // Order by { A(=null), R, G, B }
                        Marshal.Copy(pImagePointer + (j * rect.Width * 3 + i * 3), pBytePixel, 1, 3);
                        Array.Reverse(pBytePixel); // Reverse this array to order { B, G, R, A }
                        pPixel[j * rect.Width + i] = BitConverter.ToInt32(pBytePixel, 0);
                    }
                }
                */
                ReleaseMemory(ref pImage);
                return pPixel;
            }

            // Image에서 Rect영역에 있는 Memroy를 복사해온다.
            // MSDN을 참고하면 int 15를 BitConvert에 통과시킬 때 {0xFF, 0x00, 0x00, 0x00}로 나옴
            // 즉 byte[]에서 int 값으로 컨버팅할 때 B / G / R / A 순서로 넣어야함
            private static byte[] Scan24bitImageByPlane(ref Bitmap pImage, int nPlane, Rectangle rect)    // Plane 0 : R, 1 : G, 2 : B
            {
                if (pImage.PixelFormat != PixelFormat.Format24bppRgb ||
                        rect.X > pImage.Width || rect.Y > pImage.Height)
                    return null;

                byte[] pPixelPlanes = new byte[rect.Width * rect.Height];
                IntPtr pImagePointer = GetMemory(ref pImage, rect, ImageLockMode.ReadOnly);
                for (int j = 0; j < rect.Height; j++)
                {
                    for (int i = 0; i < rect.Width; i++)
                    {
                        byte[] pBytePixel = new byte[3]; // Order by { R, G, B }
                        Marshal.Copy(pImagePointer + (j * rect.Width * 3 + i * 3), pBytePixel, 0, pBytePixel.Length);
                        pPixelPlanes[j * rect.Width + i] = pBytePixel[nPlane];
                    }
                }
                ReleaseMemory(ref pImage);
                return pPixelPlanes;
            }

            // Image 전체에 Memory 배열을 출력한다.
            public static bool Print8bitImage(byte[] pByte, ref Bitmap pImage)
            {
                return Print8bitImage(pByte, ref pImage, new Rectangle(Point.Empty, pImage.Size));
            }

            // Image의 단행 위치에 Memory 배열을 출력한다.
            public static bool Print8bitLine(byte[] pByte, ref Bitmap pImage, int pixelY)
            {
                return Print8bitImage(pByte, ref pImage, new Rectangle(0, pixelY, pImage.Width, 1));
            }

            // Image의 1개 Point에 Gray Level을 출력한다.
            public static bool Print8bitPoint(byte grayLevel, ref Bitmap pImage, int pixelX, int pixelY)
            {
                byte[] pByte = new byte[1] { grayLevel };
                return Print8bitImage(pByte, ref pImage, new Rectangle(pixelX, pixelY, 1, 1));
            }

            // Image에서 Rect 영역에 있는 Memory 배열을 출력한다.
            public static bool Print8bitImage(byte[] pByte, ref Bitmap pImage, Rectangle rect)
            {
                if (pByte.Length != rect.Width * rect.Height)
                    return false;
                IntPtr pImagePointer = GetMemory(ref pImage, rect, ImageLockMode.WriteOnly);
                Marshal.Copy(pByte, 0, pImagePointer, pByte.Length);
                ReleaseMemory(ref pImage);
                return true;
            }

            // Image 전체에 Memory 배열을 출력한다.
            public static bool Print24bitImage(int[] pPixel, ref Bitmap pImage)
            {
                return Print24bitImage(pPixel, ref pImage, new Rectangle(Point.Empty, pImage.Size));
            }

            // Image의 단행 위치에 Memory 배열을 출력한다.
            public static bool Print24bitLine(int[] pPixel, ref Bitmap pImage, int pixelY)
            {
                return Print24bitImage(pPixel, ref pImage, new Rectangle(0, pixelY, pImage.Width, 1));
            }

            // Image의 1개 Point에 Gray Level을 출력한다.
            public static bool Print24bitPoint(int colorLevel, ref Bitmap pImage, int pixelX, int pixelY)
            {
                int[] pPixel = new int[1] { colorLevel };
                return Print24bitImage(pPixel, ref pImage, new Rectangle(pixelX, pixelY, 1, 1));
            }

            // Image에서 Rect 영역에 있는 Memory 배열을 출력한다.
            // MSDN을 참고하면 int 15를 BitConvert에 통과시킬 때 {0xFF, 0x00, 0x00, 0x00}로 나옴
            // 즉 byte[]에서 int 값으로 컨버팅할 때 B / G / R / A 순서로 넣어야함
            public static bool Print24bitImage(int[] pPixel, ref Bitmap pImage, Rectangle rect)
            {
                if (pPixel.Length != rect.Width * rect.Height)
                    return false;
                IntPtr pImagePointer = GetMemory(ref pImage, rect, ImageLockMode.WriteOnly);
                Marshal.Copy(pPixel, 0, pImagePointer, pPixel.Length);
                /*
                for (int j = 0; j < rect.Height; j++)
                {
                    for (int i = 0; i < rect.Width; i++)
                    {
                        byte[] pBytePixel = BitConverter.GetBytes(pPixel[j * rect.Width + i]); // Order by { B, G, R, A }, Const Length : 4
                        Array.Reverse(pBytePixel); // Reverse this array to order { A=null, R, G, B }, Only copy 3 component : R, G, B
                        Marshal.Copy(pBytePixel, 1, pImagePointer + (j * rect.Width * 3 + i * 3), 3);
                    }
                }
                */
                ReleaseMemory(ref pImage);
                return true;
            }
            #endregion

        }
    }
}
