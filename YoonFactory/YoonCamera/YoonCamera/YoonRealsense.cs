﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Intel.RealSense;

namespace YoonFactory.Camera.Realsense
{
    public enum eYoonRSCaptureMode
    {
        None = -1,
        RGBMono,
        RGBColor,
        Depth,
        DepthColorize,
    }

    public enum eYoonRSAlignMode
    {
        None = -1,
        ToRGB,
        ToDepth,
    }

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
                for(int i=0; i<Buffer.Length; i++)
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

        public int OverCount(short nValue, bool bIncludeThreshold=false)
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
    }
    
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

    public class YoonRealsense : IYoonCamera, IDisposable
    {
        public const int MAX_IMAGE_WIDTH = 1920;    // D435 기준
        public const int MAX_IMAGE_HEIGHT = 1080;
        public const int MAX_DEPTH_WIDTH = 1280;
        public const int MAX_DEPTH_HEIGHT = 720;
        public const float MAX_DEPTH_DIST = 1.5f;

        private Pipeline m_pPipeLine;
        private Context m_pContext;
        private Device m_pCurrentCamera;
        private Sensor m_pCurrentRGBSensor;
        private Sensor m_pCurrentDepthSensor;
        private Colorizer m_pColorizer;
        private Align m_pAlign;
        private eYoonRSAlignMode m_nAlignMode;
        private DeviceList m_pListCamera;
        private List<eYoonRSCaptureMode> m_pListCaptureMode;
        private bool m_bFlagUseRGBStream = false;
        private bool m_bFlagUseDepthStream = false;
        private bool m_bFlagUseAlignStream = false;

        public event ImageUpdateCallback OnCameraImageUpdateEvent;
        public event ImageUpdateCallback OnDepthImageUpdateEvent;
        public event DepthUpdateCallback OnDepthDataUpdateEvent;
        public event MetaDataUpdateCallback OnMetaDataUpdateEvent;
        
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int DepthWidth { get; set; }
        public int DepthHeight { get; set; }
        public bool IsOpenCamera { get; private set; }
        public bool IsStartCamera { get; private set; }
        public bool IsLiveOn { get; private set; }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    IsLiveOn = false;
                    Thread.Sleep(200);
                    if (m_pPipeLine != null)
                        m_pPipeLine.Dispose();
                    if (m_pContext != null)
                        m_pContext.Dispose();
                    if (m_pCurrentCamera != null)
                        m_pCurrentCamera.Dispose();
                    if (m_pCurrentRGBSensor != null)
                        m_pCurrentRGBSensor.Dispose();
                    if (m_pCurrentDepthSensor != null)
                        m_pCurrentDepthSensor.Dispose();
                    if (m_pColorizer != null)
                        m_pColorizer.Dispose();
                    if (m_pAlign != null)
                        m_pAlign.Dispose();

                    // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                    // TODO: 큰 필드를 null로 설정합니다.
                    if (m_pListCaptureMode != null)
                        m_pListCaptureMode.Clear();
                    if (m_pListCamera != null)
                        m_pListCamera.Dispose();

                    m_pListCaptureMode = null;
                    m_pListCamera = null;

                    disposedValue = true;
                }
            }
        }

        public YoonRealsense(eYoonRSCaptureMode nModeCapture, eYoonRSAlignMode nModeAlign = eYoonRSAlignMode.None)
        {
            m_pListCaptureMode = new List<eYoonRSCaptureMode>();
            m_pListCaptureMode.Add(nModeCapture);

            m_nAlignMode = nModeAlign;

            ImageWidth = DepthWidth = 640;
            ImageHeight = DepthHeight = 480;
        }

        public YoonRealsense(List<eYoonRSCaptureMode> pListMode, eYoonRSAlignMode nModeAlign = eYoonRSAlignMode.None)
        {
            m_pListCaptureMode = new List<eYoonRSCaptureMode>(pListMode);

            m_nAlignMode = nModeAlign;

            ImageWidth = DepthWidth = 640;
            ImageHeight = DepthHeight = 480;
        }

        public YoonRealsense(int nWidth, int nHeight, eYoonRSAlignMode nModeAlign = eYoonRSAlignMode.None)
        {
            m_pListCaptureMode = new List<eYoonRSCaptureMode>();
            foreach (eYoonRSCaptureMode nModeCapture in Enum.GetValues(typeof(eYoonRSCaptureMode)))
            {
                if (nModeCapture == eYoonRSCaptureMode.None) continue;
                m_pListCaptureMode.Add(nModeCapture);
            }

            m_nAlignMode = nModeAlign;

            ImageWidth = DepthWidth = nWidth;
            ImageHeight = DepthHeight = nHeight;
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        ~YoonRealsense()
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
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region Interface 연결 함수
        public int OpenCamera(int nNo = 0)
        {
            if (IsOpenCamera) return -1;
            if (ImageWidth > MAX_IMAGE_WIDTH || ImageHeight > MAX_IMAGE_HEIGHT) return -1;
            if (DepthWidth > MAX_DEPTH_WIDTH || DepthHeight > MAX_DEPTH_HEIGHT) return -1;

            try
            {
                m_pContext = new Context();
                m_pListCamera = m_pContext.QueryDevices();
                if (m_pListCamera.Count == 0 || nNo >= m_pListCamera.Count)
                {
                    Console.WriteLine("Open Realsense Failure : Camera Lack");
                    return 0;
                }
                m_pCurrentCamera = m_pListCamera[nNo];
                foreach (Sensor pSensor in m_pCurrentCamera.Sensors)
                {
                    switch (pSensor.Info[CameraInfo.Name])
                    {
                        case "Stereo Module":
                            m_pCurrentDepthSensor = pSensor;
                            break;
                        case "RGB Camera":
                            m_pCurrentRGBSensor = pSensor;
                            break;
                        default:
                            break;
                    }
                }
                m_pPipeLine = new Pipeline(m_pContext);
                m_pColorizer = new Colorizer();
                if (m_nAlignMode == eYoonRSAlignMode.ToRGB)
                {
                    m_pAlign = new Align(Stream.Color);
                    m_bFlagUseAlignStream = true;
                }
                else if (m_nAlignMode == eYoonRSAlignMode.ToDepth)
                {
                    m_pAlign = new Align(Stream.Depth);
                    m_bFlagUseAlignStream = true;
                }
                else
                    m_pAlign = new Align(Stream.Any);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Open Realsense Failure : Invaild Error");
                Console.WriteLine(ex.ToString());
                return -1;
            }

            IsOpenCamera = true;
            return m_pListCamera.Count;
        }

        public bool StartCamera()
        {
            if (IsStartCamera) return false;
            if (ImageWidth > MAX_IMAGE_WIDTH || ImageHeight > MAX_IMAGE_HEIGHT) return false;
            if (DepthWidth > MAX_DEPTH_WIDTH || DepthHeight > MAX_DEPTH_HEIGHT) return false;

            try
            {
                ////  Stream 등록
                Config pConfig = new Config();
                foreach (eYoonRSCaptureMode nMode in m_pListCaptureMode)
                {
                    switch(nMode)
                    {
                        case eYoonRSCaptureMode.RGBMono:
                            if (m_bFlagUseRGBStream == true) continue;
                            pConfig.EnableStream(Stream.Color, ImageWidth, ImageHeight, Format.Y8);
                            m_bFlagUseRGBStream = true;
                            break;
                        case eYoonRSCaptureMode.RGBColor:
                            if (m_bFlagUseRGBStream == true) continue;
                            pConfig.EnableStream(Stream.Color, ImageWidth, ImageHeight, Format.Rgb8);
                            m_bFlagUseRGBStream = true;
                            break;
                        case eYoonRSCaptureMode.Depth:
                        case eYoonRSCaptureMode.DepthColorize:
                            if (m_bFlagUseDepthStream == true) continue;
                            pConfig.EnableStream(Stream.Depth, DepthWidth, DepthHeight, Format.Any);
                            m_bFlagUseDepthStream = true;
                            break;
                    }
                }
                PipelineProfile pProfile = m_pPipeLine.Start(pConfig);
            }
            catch (Exception ex)
            {
                Console.Write("Start Realsense Failure : Invaild Error");
                Console.WriteLine(ex.ToString());
                return false;
            }

            IsStartCamera = true;
            return true;
        }

        public void LiveOn()
        {
            IsLiveOn = true;
            Task.Run(new Action(ProcessLive));
        }

        public void LiveOff()
        {
            IsLiveOn = false;
            Thread.Sleep(100);
        }

        public bool GetImage(uint nTimeout)
        {
            return GetImage(eYoonRSCaptureMode.RGBColor, nTimeout);
        }
        #endregion

        public bool GetImage(eYoonRSCaptureMode nMode, uint nTimeout = 1000)
        {
            if (m_pContext == null || m_pPipeLine == null) return false;

            bool bResult = false;
            try
            {
                using (FrameSet pFrameSet = m_pPipeLine.WaitForFrames(nTimeout))
                {
                    DepthFrame pFrameDepth;
                    VideoFrame pFrameView;
                    switch (nMode)
                    {
                        case eYoonRSCaptureMode.RGBMono:
                        case eYoonRSCaptureMode.RGBColor:
                            pFrameView = pFrameSet.ColorFrame.DisposeWith(pFrameSet);
                            if (pFrameView != null)
                            {
                                OnCameraImageUpdateEvent(this, new RSFrameArgs(pFrameView));
                                bResult = true;
                            }
                            break;
                        case eYoonRSCaptureMode.DepthColorize:
                            if (m_bFlagUseAlignStream)
                            {
                                FrameSet pFrameSetAlign = m_pAlign.Process<FrameSet>(pFrameSet).DisposeWith(pFrameSet);  // Frame 가져오기 전에 RGB Camera와 Depth Camera간의 Align 맞춤
                                pFrameDepth = pFrameSetAlign.DepthFrame.DisposeWith(pFrameSetAlign);
                                pFrameView = m_pColorizer.Process<VideoFrame>(pFrameDepth).DisposeWith(pFrameSetAlign);
                            }
                            else
                            {
                                pFrameDepth = pFrameSet.DepthFrame.DisposeWith(pFrameSet);
                                pFrameView = m_pColorizer.Process<VideoFrame>(pFrameDepth).DisposeWith(pFrameSet);
                            }
                            if (pFrameView != null)
                            {
                                OnDepthImageUpdateEvent(this, new RSFrameArgs(pFrameView));
                                bResult = true;
                            }
                            break;
                        default:
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Capture Failure : Invaild Error");
                Console.WriteLine(ex.ToString());
                return false;
            }

            return bResult;
        }

        public bool GetDepth(eYoonRSCaptureMode nMode, uint nTimeout = 1000)
        {
            if (m_pContext == null || m_pPipeLine == null) return false;

            bool bResult = false;
            try
            {
                using (FrameSet pFrameSet = m_pPipeLine.WaitForFrames(nTimeout))
                {
                    DepthFrame pFrameDepth;

                    switch (nMode)
                    {
                        case eYoonRSCaptureMode.Depth:
                            if (m_bFlagUseAlignStream)
                            {
                                FrameSet pFrameSetAlign = m_pAlign.Process<FrameSet>(pFrameSet).DisposeWith(pFrameSet);  // Frame 가져오기 전에 RGB Camera와 Depth Camera간의 Align 맞춤
                                pFrameDepth = pFrameSetAlign.DepthFrame.DisposeWith(pFrameSetAlign);
                            }
                            else
                                pFrameDepth = pFrameSet.DepthFrame.DisposeWith(pFrameSet);
                            if (pFrameDepth != null && pFrameDepth.Sensor != null)
                            {
                                OnDepthDataUpdateEvent(this, new RSDepthArgs(pFrameDepth));
                                bResult = true;
                            }
                            break;
                        default:
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Capture Failure : Invaild Error");
                Console.WriteLine(ex.ToString());
                return false;
            }
            return bResult;
        }

        public bool GetMetaData(eYoonRSCaptureMode nMode, uint nTimeout = 1000)
        {
            if (m_pContext == null || m_pPipeLine == null) return false;

            bool bResult = false;
            try
            {
                using (FrameSet pFrameSet = m_pPipeLine.WaitForFrames(nTimeout))
                {
                    Frame pFrame;
                    switch (nMode)
                    {
                        case eYoonRSCaptureMode.RGBMono:
                        case eYoonRSCaptureMode.RGBColor:
                            pFrame = pFrameSet.ColorFrame.DisposeWith(pFrameSet);
                            if (pFrame != null)
                            {
                                OnMetaDataUpdateEvent(this, new RSMetaArgs(pFrame));
                                bResult = true;
                            }
                            break;
                        case eYoonRSCaptureMode.DepthColorize:
                        case eYoonRSCaptureMode.Depth:
                            pFrame = pFrameSet.DepthFrame.DisposeWith(pFrameSet);
                            if (pFrame != null)
                            {
                                OnMetaDataUpdateEvent(this, new RSMetaArgs(pFrame));
                                bResult = true;
                            }
                            break;
                        default:
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Capture Failure : Invaild Error");
                Console.WriteLine(ex.ToString());
                return false;
            }
            return bResult;
        }

        private void ProcessLive()
        {
            if (m_pContext == null || m_pPipeLine == null) return;

            while (IsLiveOn)
            {
                try
                {
                    // We wait for the next available FrameSet and using it as a releaser object that would track
                    // all newly allocated .NET frames, and ensure deterministic finalization
                    // at the end of scope. 
                    using (FrameSet pFrameSet = m_pPipeLine.WaitForFrames())
                    {
                        if (m_bFlagUseRGBStream)
                        {
                            VideoFrame pFrameColor = pFrameSet.ColorFrame.DisposeWith(pFrameSet);
                            if (pFrameColor != null) Dispatcher.CurrentDispatcher.Invoke(OnCameraImageUpdateEvent, DispatcherPriority.Render, this, new RSFrameArgs(pFrameColor));
                        }
                        if (m_bFlagUseDepthStream && !m_bFlagUseAlignStream)
                        {
                            DepthFrame pDepth = pFrameSet.DepthFrame.DisposeWith(pFrameSet);
                            if (pDepth != null && pDepth.Sensor != null) Dispatcher.CurrentDispatcher.Invoke(OnDepthDataUpdateEvent, DispatcherPriority.Render, this, new RSDepthArgs(pDepth));
                            VideoFrame pFrameDepth = m_pColorizer.Process<VideoFrame>(pDepth).DisposeWith(pFrameSet);
                            if (pFrameDepth != null) Dispatcher.CurrentDispatcher.Invoke(OnDepthImageUpdateEvent, DispatcherPriority.Render, this, new RSFrameArgs(pFrameDepth));
                        }
                        else if(m_bFlagUseDepthStream && m_bFlagUseDepthStream)
                        {
                            FrameSet pFrameSetAlign = m_pAlign.Process<FrameSet>(pFrameSet).DisposeWith(pFrameSet);
                            DepthFrame pDepthAlign = pFrameSetAlign.DepthFrame.DisposeWith(pFrameSetAlign);
                            if (pDepthAlign != null && pDepthAlign.Sensor != null) Dispatcher.CurrentDispatcher.Invoke(OnDepthDataUpdateEvent, DispatcherPriority.Render, this, new RSDepthArgs(pDepthAlign));
                            VideoFrame pFrameDepthAlign = m_pColorizer.Process<VideoFrame>(pDepthAlign).DisposeWith(pFrameSetAlign);
                            if (pFrameDepthAlign != null) Dispatcher.CurrentDispatcher.Invoke(OnDepthImageUpdateEvent, DispatcherPriority.Render, this, new RSFrameArgs(pFrameDepthAlign));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
            }
        }
    }

    public static class RealsenseFactory
    {
        public static List<int[]> GetDepthScaleToList(RSDepth pDepthData, float dScaleSensor, int nPixelResolution)
        {
            if (nPixelResolution == 0 || dScaleSensor == 0.0) return null;
            if (pDepthData.Width % nPixelResolution != 0 || pDepthData.Height % nPixelResolution != 0) return null;
            List<int[]> pListDepthScale = new List<int[]>();

            for (int iRow = 0; iRow < pDepthData.Height / nPixelResolution; iRow++)
            {
                int[] pArrayDepthScaleRow = new int[pDepthData.Width / nPixelResolution];
                for (int iCol = 0; iCol < pDepthData.Width / nPixelResolution; iCol++)
                {
                    int nDepthScaling = 0;
                    int nCountPartial = 0;
                    for (int jY = iRow * nPixelResolution; jY < (iRow + 1) * nPixelResolution; jY++)
                    {
                        for (int jX = iCol * nPixelResolution; jX < (iCol + 1) * nPixelResolution; jX++)
                        {
                            short nDepthData = pDepthData.GetDepth(jX, jY);
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

        public static byte[] GetDepthScaleToBuffer(RSDepth pDepthData, ref float dValueResolution, float dScaleSensor, int nPixelResolution)
        {
            if (nPixelResolution == 0 || dScaleSensor == 0.0) return null;
            if (pDepthData.Width % nPixelResolution != 0 || pDepthData.Height % nPixelResolution != 0) return null;

            float dEffectRange = YoonRealsense.MAX_DEPTH_DIST / dScaleSensor;
            dValueResolution = dEffectRange / byte.MaxValue;
            if (dValueResolution == 0) return null;

            byte[] pBuffer = new byte[(pDepthData.Width / nPixelResolution) * (pDepthData.Height / nPixelResolution)];

            for (int iRow = 0; iRow < pDepthData.Height / nPixelResolution; iRow++)
            {
                for (int iCol = 0; iCol < pDepthData.Width / nPixelResolution; iCol++)
                {
                    int nDepthScaling = 0;
                    int nCountPartial = 0;
                    for (int jY = iRow * nPixelResolution; jY < (iRow + 1) * nPixelResolution; jY++)
                    {
                        for (int jX = iCol * nPixelResolution; jX < (iCol + 1) * nPixelResolution; jX++)
                        {
                            short nDepthData = pDepthData.GetDepth(jX, jY);
                            if (nDepthData > 0 && nDepthData < dEffectRange) nDepthScaling += nDepthData;
                            else if (nDepthData >= dEffectRange) nDepthScaling += (short)dEffectRange;
                            else continue;
                            nCountPartial++;
                        }
                    }
                    if (nCountPartial > 0)
                        pBuffer[iRow * (pDepthData.Width / nPixelResolution) + iCol] = (byte)((nDepthScaling / nCountPartial) / dValueResolution);
                }
            }

            return pBuffer;
        }
    }
}