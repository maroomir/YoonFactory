using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory.Camera
{
    public enum eYoonCamera
    {
        None,
        BaslerMono,
        BaslerColor,
        RealsenseColor,
        RealsenseDepth,
    }

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
