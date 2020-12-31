using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace YoonFactory.CV
{
    public static class CVFactory
    {
        public static class Converter
        {
            public static Mat ToMatGrey(IntPtr pBufferAddress, int nWidth, int nHeight)
            {
                if (pBufferAddress == IntPtr.Zero) return null;
                byte[] pBuffer = new byte[nWidth * nHeight];
                Marshal.Copy(pBufferAddress, pBuffer, 0, nWidth * nHeight);
                return Mat.FromImageData(pBuffer, ImreadModes.Grayscale);
            }

            public static Mat ToMatGrey(byte[] pBuffer, int nWidth, int nHeight)
            {
                if (pBuffer == null || pBuffer.Length != nWidth * nHeight) return null;
                return Mat.FromImageData(pBuffer, ImreadModes.Grayscale);
            }

            public static Mat ToMatColor(IntPtr pBufferAddress, int nWidth, int nHeight)
            {
                if (pBufferAddress == IntPtr.Zero) return null;
                byte[] pBuffer = new byte[nWidth * nHeight * 3];
                Marshal.Copy(pBufferAddress, pBuffer, 0, nWidth * nHeight);
                return Mat.FromImageData(pBuffer, ImreadModes.Color);
            }

            public static Mat ToMatColor(byte[] pBuffer, int nWidth, int nHeight)
            {
                if (pBuffer == null || pBuffer.Length != nWidth * nHeight * 3) return null;
                return Mat.FromImageData(pBuffer, ImreadModes.Color);
            }
        }
    }
}
