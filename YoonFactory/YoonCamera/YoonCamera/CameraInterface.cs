using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory.Camera
{
    public interface IYoonDepth : IDisposable
    {
        int Width { get; }
        int Height { get; }
        int SignificantCount { get; }
        void SetBuffer(IntPtr pAddress);
        void SetBuffer(IntPtr pAddress, int nWidth, int nHeight);
        void Clear();
        IYoonDepth Clone();
        void CopyFrom(IYoonDepth d);
        IYoonDepth Crop(int nStartX, int nStartY, int nWidth, int nHeight);
        IYoonDepth Crop(YoonVector2N vecStartPos, int nWidth, int nHeight);
    }

    public interface IYoonDepth<T> : IYoonDepth where T : IComparable, IComparable<T>
    {
        T[] Buffer { get; }
        T Center { get; }
        T Average { get; }
        T GetAverage(int nStartX, int nStartY, int nWidth, int nHeight);
        T GetAverage(YoonVector2N vecStartPos, int nWidth, int nHeight);
        T GetDepth(int nX, int nY);
        T GetDepth(YoonVector2N vecPos);
        void SetDepth(T value, int nX, int nY);
        void SetDepth(T value, YoonVector2N vecPos);
        int EqualCount(T threshold);
        int OverCount(T threshold, bool bIncludeThreshold);
        int UnderCount(T threshold, bool bIncludeThreshold);
    }

    public interface IYoonCamera
    {
        event ImageUpdateCallback OnCameraImageUpdateEvent;

        bool IsOpenCamera { get; }
        bool IsStartCamera { get; }
        bool IsLiveOn { get; }

        int OpenCamera(int nNo);
        bool StartCamera();
        void LiveOn();
        void LiveOff();
        bool GetImage(uint nTimeout);
    }
}
