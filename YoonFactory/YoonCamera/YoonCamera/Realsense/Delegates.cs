using System;
using Intel.RealSense;

namespace YoonFactory.Camera.Realsense
{
    public class RSMetaArgs : EventArgs
    {
        public long Gain;
        public long ExposureTime;
        public long Gamma;
        public long Fps;
        public long Briteness;
        public long WhiteBalance;
        public long Temperature;

        public RSMetaArgs(Frame pFrame)
        {
            if (pFrame == null) return;
            try
            {
                Gain = pFrame.GetFrameMetadata(FrameMetadataValue.GainLevel);
                ExposureTime = pFrame.GetFrameMetadata(FrameMetadataValue.ActualExposure);
                Gamma = pFrame.GetFrameMetadata(FrameMetadataValue.Gamma);
                Fps = pFrame.GetFrameMetadata(FrameMetadataValue.ActualFps);
                Briteness = pFrame.GetFrameMetadata(FrameMetadataValue.Brightness);
                WhiteBalance = pFrame.GetFrameMetadata(FrameMetadataValue.WhiteBalance);
                Temperature = pFrame.GetFrameMetadata(FrameMetadataValue.Temperature);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    public class RSFrameArgs : FrameArgs
    {
        public double Time;

        public RSFrameArgs(VideoFrame pFrame)
        {
            Width = pFrame.Width;
            Height = pFrame.Height;
            BufferSize = pFrame.DataSize;
            Plane = pFrame.BitsPerPixel / 8; // 8BIT
            Time = pFrame.Timestamp;
            pAddressBuffer = pFrame.Data;
        }
    }

    public class RSDepthArgs : EventArgs
    {
        public int Width;
        public int Height;
        public int BufferSize;
        public float SensorScale;
        public double Time;
        public RSDepth pDepth;

        public RSDepthArgs(DepthFrame pFrame)
        {
            Width = pFrame.Width;
            Height = pFrame.Height;
            BufferSize = pFrame.DataSize;
            SensorScale = pFrame.Sensor.DepthScale;
            Time = pFrame.Timestamp;
            pDepth = new RSDepth(pFrame.Data, pFrame.Width, pFrame.Height);
        }
    }

    public delegate void DepthUpdateCallback(object sender, RSDepthArgs e);
    public delegate void MetaDataUpdateCallback(object sender, RSMetaArgs e);

}
