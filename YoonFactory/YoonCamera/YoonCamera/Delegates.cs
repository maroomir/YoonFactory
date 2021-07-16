using System;
using YoonFactory.Image;

namespace YoonFactory.Camera
{
    public class FrameArgs : EventArgs
    {
        public long Width;
        public long Height;
        public long BufferSize;
        public int Plane;
        public IntPtr pAddressBuffer;

        public YoonImage ToImage(eYoonRGBMode nMode = eYoonRGBMode.None)
        {
            return new YoonImage(pAddressBuffer, (int)Width, (int)Height, Plane, nMode);
        }
    }

    public delegate void ImageUpdateCallback(object sender, FrameArgs e);
}
