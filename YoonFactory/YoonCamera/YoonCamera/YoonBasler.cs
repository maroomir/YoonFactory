using PylonC.NET;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

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

    /// <summary>
    /// C#의 OpenCV 등 비상용 라이브러리에서 Basler Camera를 사용하기 위한 기본 Class
    /// 해당 Class를 사용하려면 다음 namespace가 선언되며, 개체 브라우저에 다음 참조가 추가되어야 한다.
    ///    => PylonC.Net
    /// </summary>
    public class YoonBasler : IYoonCamera, IDisposable
    {
        public const uint MAX_CAM = 1;
        public const uint MAX_BUFFER = 5;

        protected PYLON_DEVICE_HANDLE m_device; // 장치의 handle
        protected PYLON_STREAMGRABBER_HANDLE m_grabber; // 연속 grabber의 handle
        protected PYLON_WAITOBJECT_HANDLE m_wait;
        protected PYLON_CHUNKPARSER_HANDLE m_chunkParser;
        protected Dictionary<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> m_buffers; // Grab에 사용되는 Handle과 Buffer
        protected PylonGrabResult_t m_grabResult;

        protected NODE_HANDLE m_node; // 장치-PC간 Param 교환 handle
        protected NODEMAP_HANDLE m_nodeMap;

        protected bool m_isInit;
        protected string m_cameraName;
        protected long m_cameraWidth, m_cameraHeight;
        protected uint m_payloadSize;
        protected int m_imageBand;

        public event ImageUpdateCallback OnCameraImageUpdateEvent;

        public bool IsOpenCamera { get; private set; }
        public bool IsStartCamera { get; private set; }
        public bool IsLiveOn { get; private set; }
        public long ImageWidth { get; set; }
        public long ImageHeight { get; set; }
        public long Gain { get; set; }
        public double ExposureTime { get; set; }
        public bool IsReverseX { get; set; }
        public bool IsReverseY { get; set; }
        public bool IsRotateImage { get; set; }
        public int Plane { get; set; }

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
                    m_device = null;
                    m_grabber = null;
                    m_wait = null;
                    m_chunkParser = null;
                    m_node = null;
                    m_nodeMap = null;

                    // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                    // TODO: 큰 필드를 null로 설정합니다.
                    m_buffers.Clear();
                    m_buffers = null;

                    disposedValue = true;
                }
            }
        }

        public YoonBasler()
        {
            m_device = new PYLON_DEVICE_HANDLE();
            m_grabber = new PYLON_STREAMGRABBER_HANDLE();
            m_wait = new PYLON_WAITOBJECT_HANDLE();
            m_chunkParser = new PYLON_CHUNKPARSER_HANDLE();
            m_buffers = new Dictionary<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<byte>>();

            m_node = new NODE_HANDLE();
            m_nodeMap = new NODEMAP_HANDLE();

            m_isInit = false;
            m_cameraName = null;
            m_cameraWidth = 640;
            m_cameraHeight = 480;
            m_payloadSize = 1;
            m_imageBand = 1;

            IsRotateImage = false;
            IsReverseY = false;
            Plane = 1;
            ImageWidth = 640; // 표준 해상도
            ImageHeight = 480;
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        ~YoonBasler()
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

        #region Basler Camera의 초기화 및 종료 코드
        protected bool Init(PYLON_DEVICE_HANDLE device)
        {
            bool isAvailAcquisitionStart;
            bool isAvailFrameStart;

            string triggerSelectorValue = "FrameStart";
            string errorMessage;

            uint streams;
            long imageSize;
            int i;

            m_device = device;

            // Mono-8bit로 Setting
            if (Pylon.DeviceFeatureIsAvailable(m_device, "EnumEntry_PixelFormat_Mono8"))
                Pylon.DeviceFeatureFromString(m_device, "PixelFormat", "Mono8");

            // Camera가 Trigger Mode로만 동작하는지 확인한다.
            isAvailAcquisitionStart = Pylon.DeviceFeatureIsAvailable(m_device, "EnumEntry_TriggerSelector_AcquisitionStart");
            isAvailFrameStart = Pylon.DeviceFeatureIsAvailable(m_device, "EnumEntry_TriggerSelector_FrameStart");

            if (isAvailAcquisitionStart && !isAvailFrameStart)
            {
                Pylon.DeviceFeatureFromString(m_device, "TriggerSelector", "AcquisitionStart");
                Pylon.DeviceFeatureFromString(m_device, "TriggerMode", "On");
                triggerSelectorValue = "AcquisitionStart";
            }
            else
            {
                if (isAvailAcquisitionStart)
                {
                    Pylon.DeviceFeatureFromString(m_device, "TriggerSelector", "AcquisitionStart");
                    Pylon.DeviceFeatureFromString(m_device, "TriggerMode", "Off");
                }

                Pylon.DeviceFeatureFromString(m_device, "TriggerSelector", "FrameStart");
                Pylon.DeviceFeatureFromString(m_device, "TriggerMode", "On");
                triggerSelectorValue = "FrameStart";
            }

            Pylon.DeviceFeatureFromString(m_device, "TriggerSelector", triggerSelectorValue);
            Pylon.DeviceFeatureFromString(m_device, "TriggerSource", "Software"); // Software에서 Trigger를 주는 방식
            Pylon.DeviceFeatureFromString(m_device, "AcquisitionMode", "Continuous");

            // Gig-E Camera의 패킷 사이즈를 1500으로 초기화시킨다.
            if (Pylon.DeviceFeatureIsWritable(m_device, "GevSCPSPacketSize"))
            {
                Pylon.DeviceSetIntegerFeature(m_device, "GevSCPSPacketSize", 1500);
            }

            // 지원 가능한 Image Stream의 숫자를 가져온다.
            streams = Pylon.DeviceGetNumStreamGrabberChannels(m_device);
            if (streams < 1)
            {
                errorMessage = "The transport layer doesn't support image streams : " + m_cameraName;
                MessageBox.Show(errorMessage);
                return false;
            }

            // 1번 Cam에 대한 Stream Grabber를 만든다.
            m_grabber = Pylon.DeviceGetStreamGrabber(m_device, 0);
            Pylon.StreamGrabberOpen(m_grabber);

            // 만들어진 Stream Grabber의 대기값 Control을 가져온다.
            m_wait = Pylon.StreamGrabberGetWaitObject(m_grabber);

            // 이미지 Width, Heigh를 얻어와서 4의 배수가 아니면 다시 조정한다.            
            m_cameraWidth = Pylon.DeviceGetIntegerFeature(m_device, "Width");
            m_cameraHeight = Pylon.DeviceGetIntegerFeature(m_device, "Height");
            m_payloadSize = checked((uint)Pylon.DeviceGetIntegerFeature(m_device, "PayloadSize"));

            imageSize = m_cameraHeight * m_cameraWidth;

            if (imageSize >= 1)
            {
                Plane = (int)m_payloadSize / (int)imageSize;
            }

            if (Plane == 1) m_imageBand = 1;
            else m_imageBand = 3;

            long temp_a = m_cameraWidth / 4;
            long temp_b = m_cameraWidth % 4;

            if (temp_b != 0)
            {
                m_cameraWidth = temp_a * 4;
            }

            Pylon.DeviceSetIntegerFeature(m_device, "Width", m_cameraWidth);
            Pylon.DeviceSetIntegerFeature(m_device, "Height", m_cameraHeight);

            //// 원하는대로 설정했는지 확인한다.
            m_cameraWidth = Pylon.DeviceGetIntegerFeature(m_device, "Width");
            m_cameraHeight = Pylon.DeviceGetIntegerFeature(m_device, "Height");

            //// Payload 크기를 재설정한다.
            imageSize = m_cameraWidth * m_cameraHeight * Plane;
            m_payloadSize = checked((uint)Pylon.DeviceGetIntegerFeature(device, "PayloadSize"));
            if (m_payloadSize != imageSize)
            {
                Pylon.DeviceSetIntegerFeature(m_device, "PayloadSize", imageSize);
                m_payloadSize = checked((uint)Pylon.DeviceGetIntegerFeature(m_device, "PayloadSize"));
            }

            // 버퍼 사용 갯수를 알려준다.
            Pylon.StreamGrabberSetMaxNumBuffer(m_grabber, MAX_BUFFER);

            // Payload보다 큰 size의 버퍼는 사용하지 않는다.
            Pylon.StreamGrabberSetMaxBufferSize(m_grabber, (uint)m_payloadSize);

            // Grab 시 준비해야할 자료들을 호출해둔다.
            Pylon.StreamGrabberPrepareGrab(m_grabber);

            // Grab 시 Image가 저장되는 Buffer 주소를 큐 메모리에 등록해둔다.
            for (i = 0; i < MAX_BUFFER; i++)
            {
                PylonBuffer<Byte> bufferData = new PylonBuffer<byte>(m_payloadSize, true);
                PYLON_STREAMBUFFER_HANDLE bufferKey = Pylon.StreamGrabberRegisterBuffer(m_grabber, ref bufferData);
                m_buffers.Add(bufferKey, bufferData);
            }

            // 큐 메모리에 등록되어진 Buffer 주소를 Device에 알려준다.
            i = 0;
            foreach (KeyValuePair<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> bufferAray in m_buffers)
            {
                Pylon.StreamGrabberQueueBuffer(m_grabber, bufferAray.Key, i);
                i++;
            }

            // 언제든지 Grab할 수 있도록 Start 대기명령을 준다.
            Pylon.DeviceExecuteCommandFeature(m_device, "AcquisitionStart");

            // Camera에서 받을 Image를 설정한다.
            if (m_cameraWidth < 1 || m_cameraHeight < 1 || m_cameraWidth > 10000 || m_cameraHeight > 10000)
            {
                m_cameraWidth = 4608;
                m_cameraHeight = 3288;
                m_isInit = false;
            }

            //// Image를 90도 돌려야하는지 확인한다.
            if (IsRotateImage)
            {
                ImageWidth = m_cameraHeight;
                ImageHeight = m_cameraWidth;

                temp_a = ImageWidth / 4;
                ImageWidth = temp_a * 4;
            }
            else
            {
                ImageWidth = m_cameraWidth;
                ImageHeight = m_cameraHeight;
            }

            m_isInit = true;

            // Node를 설정한다.
            m_nodeMap = Pylon.DeviceGetNodeMap(m_device);
            m_node.SetInvalid();

            SetExposure(50);

            return true;
        }

        protected void Close()
        {
            if (m_isInit)
            {
                Pylon.DeviceClose(m_device);
                Pylon.DestroyDevice(m_device);
            }
        }
        #endregion

        #region 외부 접근가능 함수
        public int OpenCamera(int nCameraID)
        {
            uint numDevices;
            string cameraUserID;
            int nReturnValue = -1;

            try
            {
                Pylon.Initialize();
            }
            catch
            {
                return -1;
            }

            // 모든 Camera를 나열한다.
            numDevices = Pylon.EnumerateDevices();
            if (numDevices <= 0)
            {
                Console.WriteLine("Can't get Camera Device");
                Pylon.Terminate();
                return -1;
            }

            // 초기화 시작
            for (int iCam = 0; iCam < numDevices; iCam++)
            {
                if (iCam != nCameraID) continue;

                m_device = Pylon.CreateDeviceByIndex((uint)iCam);
                Pylon.DeviceOpen(m_device, Pylon.cPylonAccessModeControl | Pylon.cPylonAccessModeStream);

                if (Pylon.DeviceFeatureIsReadable(m_device, "DeviceUserID"))
                {
                    cameraUserID = Pylon.DeviceFeatureToString(m_device, "DeviceUserID");
                    Console.WriteLine("Get Device User ID : " + cameraUserID);
                    nReturnValue = (int)numDevices;
                    IsOpenCamera = true;
                    break;
                }
                else
                {
                    Console.WriteLine(string.Format("Can't get Device User ID : #%d", iCam));
                    Pylon.DeviceClose(m_device);
                    Pylon.DestroyDevice(m_device);
                    IsOpenCamera = false;
                    continue;
                }
            }

            return nReturnValue;
        }

        public bool StartCamera()
        {
            if (!IsOpenCamera) return false;

            bool bResult = Init(m_device);
            if (bResult)
            {
                SetGain(Gain);
                SetExposure(ExposureTime);
                SetReverseX(IsReverseX);
                SetReverseY(IsReverseY);
            }
            return bResult;
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

        public bool GetImage(uint waitTime)
        {
            if (m_isInit == false) return false;

            bool isResult;

            // Image를 가져오기 위해 Trigger 신호를 보낸다!
            Pylon.DeviceExecuteCommandFeature(m_device, "TriggerSoftware");

            if (waitTime < 1) return true;

            isResult = WaitImage(waitTime);
            return isResult;
        }
        #endregion

        #region Basler Camera Grab 함수
        protected void ProcessLive()
        {
            while(IsLiveOn)
            {
                GetImage(1000);
            }
        }

        protected bool WaitImage(uint waitTime)
        {
            if (m_isInit == false) return false;

            long counter;
            int bufferIndex;
            bool isReady, hasCRC, isOk;
            long imageSize = ImageWidth * ImageHeight * m_imageBand;

            // Buffer가 채워지길 기다린다. 제한 시간은 waitTime 만큼.
            isReady = Pylon.WaitObjectWait(m_wait, waitTime);
            if (isReady == false) return false; // Time over 발생 시

            isReady = Pylon.StreamGrabberRetrieveResult(m_grabber, out m_grabResult);
            if (isReady == false) return false; // Result가 비어있을 시

            // Grab 결과값에서 index를 가져온다.
            bufferIndex = (int)m_grabResult.Context;

            // 위에서 가져온 index번째 Buffer에 Image를 저장한다.
            if (m_grabResult.Status == EPylonGrabStatus.Grabbed)
            {
                PylonBuffer<Byte> buffer;
                m_buffers.TryGetValue(m_grabResult.hBuffer, out buffer);

                //// Buffer 상에 덩어리(Chunk) 형태로 Data들이 들어왔다면, 이를 점검한다.
                if (m_grabResult.PayloadType == EPylonPayloadType.PayloadType_ChunkData)
                {
                    Pylon.ChunkParserAttachBuffer(m_chunkParser, buffer);
                    hasCRC = Pylon.ChunkParserHasCRC(m_chunkParser);
                    if (hasCRC)
                    {
                        isOk = Pylon.ChunkParserCheckCRC(m_chunkParser);
                    }

                    ////// Chunck Counter 값을 되찾아온다.
                    counter = Pylon.DeviceGetIntegerFeature(m_device, "ChunkFramecounter");
                }
                //// 현재 Image를 Image Buffer에 저장한다.
                OnCameraImageUpdateEvent(this, new PylonFrameArgs(buffer, ImageWidth, ImageHeight, IsReverseY));
                /*
                int block = (int)m_cameraWidth * (int)m_imageBand * sizeof(byte);

                if (m_isReverseY)
                {
                    for (i = 0; i < m_cameraHeight; i++)
                    {
                        Array.Copy(buffer.Array, block * (m_cameraHeight - i - 1), pArrayImageBuffer, block * i, block);
                    }
                }
                else
                {
                    for (i = 0; i < m_cameraHeight; i++)
                    {
                        Array.Copy(buffer.Array, block * i, pArrayImageBuffer, block * i, block);
                    }
                }*/
            }
            else if (m_grabResult.Status == EPylonGrabStatus.Failed) return false;

            // 모든 처리가 끝난 후 Buffer와 참조 Index를 다시 큐 메모리에 넣는다.
            Pylon.StreamGrabberQueueBuffer(m_grabber, m_grabResult.hBuffer, bufferIndex);

            return true;
        }
        #endregion
/*
        #region Basler Camera의 이미지 표시 함수
        public bool DisplayImage(ref Bitmap sourceImage)
        {
            if (pArrayImageBuffer == null) pArrayImageBuffer = new byte[sizeof(byte) * ImageWidth * ImageHeight * m_imageBand];

            int nPlane = 1; // mono-8bit Image

            // Bitmap Image 파일의 사용 적합성 체크
            if (BitmapFactory.IsCompatible(sourceImage, (int)ImageWidth, (int)ImageHeight, nPlane))
            {
                //// screen에 현재 Data 업데이트
                BitmapFactory.UpdateBitmap(sourceImage, pArrayImageBuffer, (int)ImageWidth, (int)ImageHeight, nPlane);
                return true;
            }
            else
            {
                //// 새로운 Bitmap 생성 후 screen에 현재 Data 업데이트
                BitmapFactory.CreateBitmap(out sourceImage, (int)ImageWidth, (int)ImageHeight, nPlane);
                BitmapFactory.UpdateBitmap(sourceImage, pArrayImageBuffer, (int)ImageWidth, (int)ImageHeight, nPlane);
                return false;
            }

        }
        #endregion
*/
        #region Camera 설정 함수
        // Pylon 4.0 이후 Camera Param Setting 시 Node를 필수로 사용한다.
        public bool GetNode(string name)
        {
            if (m_isInit == false) return false;

            m_node = GenApi.NodeMapGetNode(m_nodeMap, name);

            if (m_node.IsValid == false)
            {
                m_node.SetInvalid();
                return false;
            }

            if (GenApi.NodeIsReadable(m_node) == false) return false;
            if (GenApi.NodeIsWritable(m_node) == false) return false;

            // Node 핸들을 Setting 후 다시 가져온다.
            m_node = GenApi.NodeGetAlias(m_node);

            return true;
        }

        public void SetDigitalShift(long shift)
        {
            if (m_isInit == false) return;
            if (GetNode("DigitalShift") == false) return;

            if (shift < 0) shift = 0;
            if (shift > 4) shift = 4;

            GenApi.IntegerSetValue(m_node, shift);
        }

        public void GetGainRange(ref long minValue, ref long maxValue, ref long inc) //// 변수는 ref로 넘겨줘야 함
        {
            if (m_isInit == false) return;
            if (GetNode("Gain") == false) return;

            minValue = GenApi.IntegerGetMin(m_node);
            maxValue = GenApi.IntegerGetMax(m_node);
            inc = GenApi.IntegerGetInc(m_node);
        }

        public long GetGain()
        {
            if (m_isInit == false) return 0;
            if (GetNode("Gain") == false) return 0;

            long gain = GenApi.IntegerGetValue(m_node);
            return gain;
        }

        public void SetGain(long gain)
        {
            if (m_isInit == false) return;
            if (GetNode("Gain") == false) return;

            long minValue = GenApi.IntegerGetMin(m_node);
            long maxValue = GenApi.IntegerGetMax(m_node);

            if (gain < minValue) gain = minValue;
            if (gain > maxValue) gain = maxValue;

            GenApi.IntegerSetValue(m_node, gain);
            Thread.Sleep(10);
        }

        public void SetExposure(double ms)
        {
            if (m_isInit == false) return;
            if (GetNode("ExposureTime") == false) return;

            double us = ms * 1000.0;

            long min_us = GenApi.IntegerGetMin(m_node);
            long max_us = GenApi.IntegerGetMax(m_node);
            long inc_us = GenApi.IntegerGetInc(m_node);

            if (inc_us > 1)
            {
                double temp_us = us / inc_us;
                us = temp_us * inc_us;
            }

            if (us < min_us) us = min_us;
            if (us > max_us) us = max_us;

            GenApi.IntegerSetValue(m_node, (long)us);
            Thread.Sleep(10);
        }

        public double GetExposure()
        {
            if (m_isInit == false) return 0.0;
            if (GetNode("ExposureTime") == false) return 0.0;

            long us = GenApi.IntegerGetValue(m_node);
            double ms = (double)us / 1000.0;
            return ms;
        }

        public void GetExposureRange(ref double min_ms, ref double max_ms) //// 변수는 ref로 넘겨줘야 함
        {
            if (m_isInit == false) return;
            if (GetNode("ExposureTime") == false) return;

            long min_us = GenApi.IntegerGetMin(m_node);
            long max_us = GenApi.IntegerGetMax(m_node);

            min_ms = (double)min_us / 1000.0;
            max_ms = (double)max_us / 1000.0;
        }

        public void SetReverseX(bool isOnOff)
        {
            if (m_isInit == false) return;

            Pylon.DeviceSetBooleanFeature(m_device, "ReverseX", isOnOff);
            Thread.Sleep(10);
        }

        public void SetReverseY(bool isOnOff)
        {
            IsReverseY = isOnOff;
        }
        #endregion
    }
}