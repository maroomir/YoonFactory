using PylonC.NET;
using System;

namespace YoonFactory.Camera.Basler
{
    public class PylonFrameArgs : FrameArgs
    {
        public bool IsReverseY;
        public PylonFrameArgs(PylonBuffer<Byte> buffer, long nWidth, long nHeight, bool bReverseY)
        {
            Width = nWidth;
            Height = nHeight;
            BufferSize = buffer.Array.Length;
            Plane = (int)(BufferSize / (Width * Height));
            IsReverseY = bReverseY;
            pAddressBuffer = buffer.Pointer;
        }
    }

}
