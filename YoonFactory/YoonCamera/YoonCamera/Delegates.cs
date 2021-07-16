using System;

namespace YoonFactory.Camera
{
    public class FrameArgs : EventArgs
    {
        public long Width;
        public long Height;
        public long BufferSize;
        public int Plane;
        public IntPtr pAddressBuffer;
    }

    public delegate void ImageUpdateCallback(object sender, FrameArgs e);
}
