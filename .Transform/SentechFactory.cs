using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Reflection;

using FileFactory;
using PvDotNet;
using Cognex.VisionPro;

namespace YoonFactory.Sentech
{
    /*
     * Sentech GigE camera & lens information
     * http://downloads.i-sentech.com/dl_documents/Datasheet_GIGE_POE_CCD_Sony.pdf
     * 
     * */

    public class SentechParam
    {
        //public MainForm pParant;
        //public SettingForm pParantSet;

        private int fCameraCount;
        private double fDefaultGain;
        private double fDefaultExposureTime;
        private bool fUseCameraIpAddressAutoAssign;
        private bool fUseCameraTriggerMode;
        private bool fUseSentechLog;
        private string fLogFilePath;

        public int CameraCount
        {
            get { return fCameraCount; }
            set { fCameraCount = value; }
        }
        public double DefaultGain
        {
            get { return fDefaultGain; }
            set { fDefaultGain = value; }
        }
        public double DefaultExposureTime
        {
            get { return fDefaultExposureTime; }
            set { fDefaultExposureTime = value; }
        }
        public bool UseCameraIpAddressAutoAssign
        {
            get { return fUseCameraIpAddressAutoAssign; }
            set { fUseCameraIpAddressAutoAssign = value; }
        }
        public bool UseCameraTriggerMode
        {
            get { return fUseCameraTriggerMode; }
            set { fUseCameraTriggerMode = value; }
        }
        public bool UseSentechLog
        {
            get { return fUseSentechLog; }
            set { fUseSentechLog = value; }
        }
        public string LogFilePath
        {
            get { return fLogFilePath; }
            set { fLogFilePath = value; }
        }
    }

    public class CameraParam
    {
        private ICogImage fCogImage;
        private int fGain;
        private int fExposureTime;

        public ICogImage CogImage
        {
            get { return fCogImage; }
            set { fCogImage = value; }
        }
        public int Gain
        {
            get { return fGain; }
            set { fGain = value; }
        }
        public int ExposureTime
        {
            get { return fExposureTime; }
            set { fExposureTime = value; }
        }
    }

    class FTech_SentechGigE
    {
        public static Dictionary<string, string> IpMacTable = new Dictionary<string, string>();

        public bool InitialConnectionFailure = false;

        public int nCameraNum;
        public static SentechParam mParam;
        public CameraParam mCameraParam;

        public AutoResetEvent autoEvent = new AutoResetEvent(false);

        public bool IsPlaying = false;
        UInt32 payloadsize = 0;

        private CogImage8Grey t8GreayImg = null;

        // Handler used to bring link disconnected event in the main UI thread
        private delegate void GenericHandler();
        private GenericHandler mDisconnectedHandler = null;

        // Main application objects: device, stream, pipeline
        private PvDevice mDevice = new PvDevice();
        private PvStream mStream = new PvStream();
        private PvPipeline mPipeline = null;

        // Acquisition state manager
        private PvAcquisitionStateManager mAcquisitionManager = null;

        // Display thread
        private PvDisplayThread mDisplayThread = null;

        private string ipaddress = "";

        private bool bOnLinkDisconnected = false;

        public FTech_SentechGigE()
        {

            // Handlers used to callbacks in the UI thread
            mDisconnectedHandler += new GenericHandler(OnDisconnected);

            // Create pipeline - requires stream
            mPipeline = new PvPipeline(mStream);

            // Create display thread, hook display event
            mDisplayThread = new PvDisplayThread();
            mDisplayThread.OnBufferDisplay += new OnBufferDisplay(OnBufferDisplay);
        }

        /// <summary>
        /// Sentech Camera의 Parameter를 설정한다.
        /// </summary>
        /// <param name="cameraNo"></param>
        /// <param name="param"></param>
        /// <param name="camSetting"></param>
        public void SetSentechGigE(int cameraNo, SentechParam param, CameraParam camParam)
        {
            nCameraNum = cameraNo;
            mParam = param;
            mCameraParam = camParam;
        }

        /// <summary>
        /// IP 주소의 c class 번호를 비교하는 함수
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int ComparisonIPAddrClassC(string a, string b)
        {
            if (a == null)
            {
                if (b == null) return 0;
                else return -1;
            }
            else
            {
                if (b == null) return 1;
            }

            int a3 = a.LastIndexOf(".");
            int a2 = a.LastIndexOf(".", a3 - 1);
            string aa = a.Substring(a2 + 1, a3 - a2 - 1);
            int b3 = b.LastIndexOf(".");
            int b2 = b.LastIndexOf(".", b3 - 1);
            string bb = b.Substring(b2 + 1, b3 - b2 - 1);

            int c1 = Convert.ToInt32(aa);
            int c2 = Convert.ToInt32(bb);

            if (c1 < c2) return -1;
            if (c1 == c2) return 0;
            return 1;
        }

        /// <summary>
        /// IpMacTable 에서 IP주소만, c class 기준으로 정렬된 형태로 돌려준다.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSortedIpList()
        {
            List<string> list = new List<string>();

            foreach (string ip in IpMacTable.Keys)
                list.Add(ip);
            list.Sort(ComparisonIPAddrClassC);

            return list;
        }

        public static List<string> GetIpListFromFile()
        {
            List<string> list = new List<string>();
            string path = "ipaddress.txt";
            if (!System.IO.File.Exists(path))
            {
                MessageBox.Show(string.Format("[{0}] file not exist !", path));
                return GetSortedIpList();
            }

            string[] txt = System.IO.File.ReadAllLines(path);
            for (int i = 0; i < txt.Length && i < mParam.CameraCount; ++i)
                list.Add(txt[i]);

            if (list.Count() < mParam.CameraCount)
            {
                MessageBox.Show(string.Format("[{0}] file contains only {1} ip address ! ({2} needed)", path, list.Count(), mParam.CameraCount));
            }

            return list;
        }

        /// <summary>
        /// 모든 카메라를 찾아서 정보 보여주기 (메시지창)
        /// </summary>
        public static void EnumerateAndListAllCamera()
        {
            Dictionary<string, string> dict = _enumerate_camera();
            string txt = "";
            int i = 0;
            foreach (string ip in dict.Keys)
            {
                txt += String.Format("Camera{0}  IP={1}  MAC={2}\n", i++, ip, dict[ip]);
            }
            MessageBox.Show(txt);
        }

        /// <summary>
        /// 카메라 다 찾고, IP주소 틀린 것 수정 (nic과 subnet mask 상으로 다른 번지 가진 것), 영구 저장.
        /// </summary>
        public static void PrepareAllCamera()
        {
            IpMacTable.Clear();
            IpMacTable = _enumerate_camera();
        }

        /// <summary>
        /// Sentech GigE 카메라를 모두 찾고, 카메라 IP주소를 랜카드 주소 맨 끝자리를 111로 바꾼것으로 강제 세팅한다.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> _enumerate_camera()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            PvSystem lSystem = new PvSystem();
            lSystem.DetectionTimeout = 2000;
            lSystem.Find();
            uint iInterfaceCount = lSystem.InterfaceCount;
            for (uint i = 0; i < iInterfaceCount; i++)
            {
                PvInterface lInterface = lSystem.GetInterface(i);
                uint iDeviceCount = lInterface.DeviceCount;
                for (uint j = 0; j < iDeviceCount; j++)
                {
                    PvDeviceInfo lDeviceInfo = lInterface.GetDeviceInfo(j);
                    string mac_addr = lDeviceInfo.MACAddress;
                    string cam_ip = lDeviceInfo.IPAddress;

                    if (mParam.UseCameraIpAddressAutoAssign)
                    {
                        // IP주소 구성이 올바르지 않으면 (카메라의 IP주소가 interface의 서브넷안에 포함되지 않으면)
                        if (false == lDeviceInfo.IsIPConfigurationValid)
                        {
                            // 카메라 IP주소를, interface 의 IP주소 끝자리에 111을 할당한 것으로 바꾼다.
                            string nic_ip = lInterface.IPAddress;
                            string cam_new_ip = nic_ip.Substring(0, nic_ip.LastIndexOf(".")) + ".111";
                            if (cam_new_ip == nic_ip) // what the !
                            {
                                cam_new_ip = nic_ip.Substring(0, nic_ip.LastIndexOf(".")) + ".112";
                            }
                            PvDevice.SetIPConfiguration(mac_addr, cam_new_ip);
                            System.Console.WriteLine("Auto setting camera[{0}] : ip address changed from [{1}] to [{2}]", mac_addr, cam_ip, cam_new_ip);

                            cam_ip = cam_new_ip;
                        }
                    }

                    if (!dict.ContainsKey(cam_ip))
                        dict.Add(cam_ip, mac_addr);

                    System.Console.WriteLine("Camera {0}: MacAddr:{1} IpAddr:{2} SubnetMask:{3}",
                        j, mac_addr, cam_ip, lDeviceInfo.SubnetMask);
                }
            }
            return dict;
        }

        public bool Connect(string aip, bool initial_connection = false)
        {
            // Just in case we came here still connected...
            Disconnect();

            char[] charsToTrim = { ' ', '\r', '\n', '\0' };
            aip = aip.TrimEnd(charsToTrim);

            payloadsize = 0;

            try
            {
                // Connect to device using ip address
                mDevice.Connect(aip);
                TRACE_SENTECH("mDevice.Connect({0})", aip);

                // Open stream using device IP address
                mStream.Open(aip);
                TRACE_SENTECH("mStream.Open({0})", aip);

                // Negotiate packet size
                mDevice.NegotiatePacketSize();
                TRACE_SENTECH("mDevice.NegotiatePacketSize()");

                // Set stream destination to our stream object
                mDevice.SetStreamDestination(mStream.LocalIPAddress, mStream.LocalPort);
                TRACE_SENTECH("mDevice.SetStreamDestination({0},{1})", mStream.LocalIPAddress, mStream.LocalPort);
            }
            catch
            {
                Disconnect();
                if (initial_connection)
                    InitialConnectionFailure = true;
                return false;
            }

            if (mDevice.IsConnected)
            {
                mDevice.OnEventGenICam += new OnEventGenICam(mDevice_OnEventGenICam);

                // Register to all events of the parameters in the device's node map
                foreach (PvGenParameter lParameter in mDevice.GenParameters)
                {
                    lParameter.OnParameterUpdate += new OnParameterUpdateHandler(OnParameterChanged);
                }

                // Connect link disconnection handler
                mDevice.OnLinkDisconnected += new OnLinkDisconnectedHandler(OnLinkDisconnected);
            }

            if (mStream.IsOpened)
            {
                // Ready image reception
                StartStreaming();
            }

            //// 카메라 노출값을 읽어온 값으로 다시 적용
            SetExposureUS(mCameraParam.ExposureTime);

            ipaddress = aip;

            return true;
        }

        void mDevice_OnEventGenICam(PvDevice aDevice, ushort aEventID, ushort aChannel, ulong aBlockID, ulong aTimestamp, List<PvGenParameter> aData)
        {
            TRACE_SENTECH("mDevice_OnEventGenICam({0})", aEventID.ToString());

            ////string lLog = "Event ID: 0x" + aEventID.ToString("X4") + "\n";
            ////if (aData != null)
            ////{
            ////    foreach (PvGenParameter p in aData)
            ////    {
            ////        lLog += "\t" + p.Name + ": " + p.ToString() + "\n";
            ////    }
            ////}
            ////Console.WriteLine(lLog);
        }

        public void Disconnect()
        {
            // If streaming, stop streaming
            if (mStream.IsOpened)
            {
                StopStreaming();
                mStream.Close();
                TRACE_SENTECH("mStream.Close()");
            }

            if (mDevice.IsConnected)
            {
                // 카메라 디버그를 위해 TriggerMode 끈 상태로 만들어 놓는다. (아니면 일일이 트리거 모드 꺼야 카메라 영상 볼수 있으니까)
                if (!bOnLinkDisconnected) // 링크가 강제로 끊어진 상태에서는 패러미터 건드리면 안된다.
                {
                    // enum: Off=0 On=1
                    mDevice.GenParameters.SetEnumValue("TriggerMode", 0);
                    TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"TriggerMode\", 0)");
                    // enum: Software=0 Line0=1 Hardware=34
                    mDevice.GenParameters.SetEnumValue("TriggerSource", 0);
                    TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"TriggerSource\", 0)");
                }

                // Disconnect events
                mDevice.OnLinkDisconnected -= new OnLinkDisconnectedHandler(OnLinkDisconnected);
                foreach (PvGenParameter lP in mDevice.GenParameters)
                {
                    lP.OnParameterUpdate -= new OnParameterUpdateHandler(OnParameterChanged);
                }

                mDevice.Disconnect();
                TRACE_SENTECH("mDevice.Disconnect()");
            }

            bOnLinkDisconnected = false;
            InitialConnectionFailure = false;
        }

        void OnParameterChanged(PvGenParameter aParameter)
        {
            string name = "";
            string value = "";
            try
            {
                name = aParameter.Name;
                if (aParameter.IsReadable)
                    value = aParameter.ToString();
            }
            catch
            {
            }

            TRACE_SENTECH("{0} {1}", name, value);
        }

        private void OnAcquisitionModeChanged()
        {
            TRACE_SENTECH("");
            ////// Get parameter
            ////PvGenEnum lParameter = mDevice.GenParameters.GetEnum("AcquisitionMode");
        }

        void OnAcquisitionStateChanged(PvDevice aDevice, PvStreamBase aStream, uint aSource, PvAcquisitionState aState)
        {
            TRACE_SENTECH("{0}", aState);
            ////// Invoke event in main UI thread
            ////BeginInvoke(mAcquisitionStateChangedHandler);
        }

        private void OnAcquisitionStateChanged()
        {
            TRACE_SENTECH("-- 2");
        }

        private void OnLinkDisconnected(PvDevice aDevice)
        {
            TRACE_SENTECH("");
            mDisconnectedHandler();
        }

        private void OnDisconnected()
        {
            TRACE_SENTECH("");
            bOnLinkDisconnected = true;
            StopStreaming();
            Disconnect();

            Thread.Sleep(200);
            CameraManager.OnCameraDisconnected(nCameraNum);
        }

        private void StartStreaming()
        {
            // Start threads
            mDisplayThread.Start(mPipeline, mDevice.GenParameters);
            TRACE_SENTECH("mDisplayThread.Start()");
            mDisplayThread.Priority = PvThreadPriority.AboveNormal;

            // Configure acquisition state manager
            mAcquisitionManager = new PvAcquisitionStateManager(mDevice, mStream);
            mAcquisitionManager.OnAcquisitionStateChanged += new OnAcquisitionStateChanged(OnAcquisitionStateChanged);

            // Start pipeline
            mPipeline.Start();
            TRACE_SENTECH("mPipeline.Start()");
        }

        void OnBufferDisplay(PvDisplayThread aDisplayThread, PvBuffer aBuffer)
        {
            TRACE_SENTECH("{0}x{1}", aBuffer.Image.Width, aBuffer.Image.Height);

            lock (mglock)
            {
                bGrabStarted = false;
            }

            try
            {
                TRACE_SENTECH("prepare buffer");

                int w = (int)aBuffer.Image.Width;
                int h = (int)aBuffer.Image.Height;
                CogImage8Root root = new CogImage8Root();

                TRACE_SENTECH("copy buffer");
                unsafe
                {
                    IntPtr ptr = (IntPtr)aBuffer.Image.DataPointer;
                    root.Initialize(w, h, ptr, w, null);
                }

                TRACE_SENTECH("set cog image");
                if (t8GreayImg == null || t8GreayImg.Width != w || t8GreayImg.Height != h)
                {
                    t8GreayImg = new CogImage8Grey();
                }

                t8GreayImg.SetRoot(root);

                TRACE_SENTECH("assign image");
                mCameraParam.CogImage = t8GreayImg;

                TRACE_SENTECH("notify ui");
                ////  외부 Main Form 및 Setting Form 연동시 사용함.
                //mParam.pParant.OnCameraUpdated(nCameraNum);
                //if (mParam.pParentSet != null)
                //    if (mParam.pParentSet.nCameraSelect == nCameraNum)
                //        mParam.pParentSet.OnCameraUpdated(_CLS_BASE.pParentSet.nCameraSelect);
            }
            catch
            {
            }
            //Console.WriteLine("bGrabStarted = false");
            TRACE_SENTECH("done.");
        }

        object mglock = new object();

        private void StopStreaming()
        {
            if (!mDisplayThread.IsRunning)
            {
                return;
            }

            // Stop display thread
            mDisplayThread.Stop(false);
            TRACE_SENTECH("mDisplayThread.Stop(false)");

            // Release acquisition manager
            mAcquisitionManager = null;

            // Stop pipeline
            if (mPipeline.IsStarted)
            {
                mPipeline.Stop();
                TRACE_SENTECH("mPipeline.Stop()");
            }
            //이거 살려놓으면 여기서 안넘어간다. 이유를 당장 알수없어서 그냥 막는다.
            //            // Wait on display thread
            //            mDisplayThread.WaitComplete();
            //            TRACE_SENTECH("mDisplayThread.WaitComplete()");
        }

        public bool IsLive()
        {
            return IsPlaying;
        }

        private void _start_acquisition()
        {
            // Get payload size
            if (payloadsize == 0)
            {
                payloadsize = PayloadSize;
                if (payloadsize > 0)
                {
                    // Propagate to pipeline to make sure buffers are big enough
                    mPipeline.BufferSize = payloadsize;
                }
                TRACE_SENTECH("mPipeline.BufferSize = {0}", payloadsize);
            }

            // Reset pipeline
            mPipeline.Reset();
            TRACE_SENTECH("mPipeline.Reset()");

            // Reset stream statistics
            PvGenCommand lResetStats = mStream.Parameters.GetCommand("Reset");
            lResetStats.Execute();
            TRACE_SENTECH("lResetStats.Execute(\"Reset\")");

            // Reset display thread stats (mostly frames displayed per seconds)
            mDisplayThread.ResetStatistics();
            TRACE_SENTECH("mDisplayThread.ResetStatistics()");

            // Use acquisition manager to send the acquisition start command to the device
            mAcquisitionManager.Start();
            TRACE_SENTECH("mAcquisitionManager.Start()");
        }

        public bool StartAcquisition()
        {
            if (InitialConnectionFailure == true)
            {
                // 카메라영상에 특정 이미지 띄우기
                mCameraParam.CogImage = GetConnectionFailureImage();
                //mParam.pParent.OnCameraUpdated(nCameraNum);
                return false;
            }

            TRACE_SENTECH("");
            if (!mDevice.IsConnected || !mStream.IsOpened)
            {
                TRACE_SENTECH("mDevice.IsConnected={0} mStream.IsOpened={1}", mDevice.IsConnected, mStream.IsOpened);

                // 연결끊고
                bOnLinkDisconnected = true;
                StopStreaming();
                Disconnect();

                // 다시 연결한다
                if (Connect(ipaddress))
                    SetupDefaultParameters();
                IsPlaying = false;
            }

            if (IsPlaying)
                return false;

            _start_acquisition();

            IsPlaying = true;

            return true;
        }

        private bool bGrabStarted = false;

        public void ManualGrab()
        {
            if (!mDevice.IsConnected || !mStream.IsOpened)
            {
                TRACE_SENTECH("mDevice.IsConnected={0} mStream.IsOpened={1}", mDevice.IsConnected, mStream.IsOpened);
                return;
            }

            if (!IsPlaying)
            {
                TRACE_SENTECH("IsPlaying={0}", IsPlaying);
                return;
            }

            if (mAcquisitionManager.State != PvAcquisitionState.Locked)
            {
                TRACE_SENTECH("mAcquisitionManager not locked");
                return;
            }

            lock (mglock)
            {
                if (bGrabStarted)
                {
                    TRACE_SENTECH("bGrabStarted");
                    return; // manual capture is pending. too fast request !
                }
                bGrabStarted = true;
            }

            if (mParam.UseCameraTriggerMode)
            {
                // Command
                mDevice.GenParameters.ExecuteCommand("TriggerSoftware");
                TRACE_SENTECH("mDevice.GenParameters.ExecuteCommand(\"TriggerSoftware\")");
                TRACE_SENTECH("done");
            }
            else
            {
                TRACE_SENTECH("no trigger mode");
            }
        }

        public void StopAcquisition()
        {
            if (InitialConnectionFailure == true)
            {
                // 카메라영상에 특정 이미지 띄우기
                mCameraParam.CogImage = GetConnectionFailureImage();
                //mParam.pParent.OnCameraUpdated(nCameraNum);
                return;
            }

            if (!mDevice.IsConnected || !mStream.IsOpened)
            {
                TRACE_SENTECH("mDevice.IsConnected={0} mStream.IsOpened={1}", mDevice.IsConnected, mStream.IsOpened);
                return;
            }

            if (!IsPlaying)
            {
                TRACE_SENTECH("IsPlaying={0}", IsPlaying);
                return;
            }

            // Use acquisition manager to send the acquisition stop command to the device
            mAcquisitionManager.Stop();
            TRACE_SENTECH("mAcquisitionManager.Stop()");

            IsPlaying = false;
        }

        private UInt32 PayloadSize
        {
            get
            {
                // Get parameters required
                PvGenInteger lPayloadSize = mDevice.GenParameters.GetInteger("PayloadSize");
                PvGenInteger lWidth = mDevice.GenParameters.GetInteger("Width");
                PvGenInteger lHeight = mDevice.GenParameters.GetInteger("Height");
                PvGenEnum lPixelFormat = mDevice.GenParameters.GetEnum("PixelFormat");

                // Try getting the payload size from the PayloadSize mandatory parameter
                Int64 lPayloadSizeValue = 0;
                if (lPayloadSize != null)
                {
                    try
                    {
                        lPayloadSizeValue = lPayloadSize.Value;
                    }
                    catch
                    {
                    }
                }

                // Compute poor man's payload size - for devices not maintaining PayloadSize properly
                Int64 lPoorMansPayloadSize = 0;
                if ((lWidth != null) && (lHeight != null) && (lPixelFormat != null))
                {
                    Int64 lWidthValue = lWidth.Value;
                    Int64 lHeightValue = lHeight.Value;

                    Int64 lPixelFormatValue = lPixelFormat.ValueInt;
                    Int64 lPixelSizeInBits = PvImage.GetPixelBitCount((PvPixelType)lPixelFormatValue);

                    lPoorMansPayloadSize = (lWidthValue * lHeightValue * lPixelSizeInBits) / 8;
                }

                // Take max
                Int64 lBestPayloadSize = Math.Max(lPayloadSizeValue, lPoorMansPayloadSize);
                if ((lBestPayloadSize > 0) && (lBestPayloadSize < UInt32.MaxValue))
                {
                    // Round up to make it mod 32 (works around an issue with some devices)
                    if ((lBestPayloadSize % 32) != 0)
                    {
                        lBestPayloadSize = ((lBestPayloadSize / 32) + 1) * 32;
                    }

                    return (UInt32)lBestPayloadSize;
                }

                // Could not compute/retrieve payload size...
                return 0;
            }
        }

        /// <summary>
        /// Connect된 Sentech카메라의 모든 property를 출력창에 출력한다.
        /// </summary>
        public void PrintAllProperties()
        {
            PvGenParameterArray lParameterArray = mDevice.GenParameters;
            uint lCount = lParameterArray.Count;
            for (uint i = 0; i < lCount; i++)
            {
                PvGenParameter lParameter = lParameterArray.Get(i);
                if (lParameter.Visibility == PvGenVisibility.Invisible)// continue;
                {
                    continue;
                }
                System.Console.WriteLine("ParameterName:{0}", lParameter.Name);
                System.Console.WriteLine(" Type:{0}", lParameter.Type);
                System.Console.WriteLine(" Category:{0}", lParameter.Category);
                System.Console.WriteLine(" Visibility:{0}", lParameter.Visibility);
                System.Console.WriteLine(" IsAvailable:{0}", lParameter.IsAvailable);
                if (!lParameter.IsAvailable)// continue;
                {
                    continue;
                }
                if (lParameter.Type == PvGenType.Integer)
                {
                    PvGenInteger lInteger = (PvGenInteger)lParameter;
                    System.Console.WriteLine(" Value:{0} Min:{1} Max:{2}"
                    , lInteger.Value, lInteger.Min, lInteger.Max);
                }
                else if (lParameter.Type == PvGenType.Enum)
                {
                    PvGenEnum lEnum = (PvGenEnum)lParameter;
                    System.Console.WriteLine(" Value:{0}[{1}]", lEnum.ValueString, lEnum.ValueInt);
                    long lEnumCount = lEnum.EntriesCount;
                    for (long j = 0; j < lEnumCount; j++)
                    {
                        PvGenEnumEntry lEnumEntry = lEnum.GetEntryByIndex(j);
                        System.Console.WriteLine(" EntryValue:{0}[{1}]"
                        , lEnumEntry.ValueString, lEnumEntry.ValueInt);
                    }
                }
                else if (lParameter.Type == PvGenType.Boolean)
                {
                    PvGenBoolean lBoolean = (PvGenBoolean)lParameter;
                    System.Console.WriteLine(" Value:{0}", lBoolean.Value);
                }
                else if (lParameter.Type == PvGenType.String)
                {
                    PvGenString lString = (PvGenString)lParameter;
                    System.Console.WriteLine(" Value:{0}", lString.Value);
                }
                else if (lParameter.Type == PvGenType.Float)
                {
                    PvGenFloat lFloat = (PvGenFloat)lParameter;
                    System.Console.WriteLine(" Value:{0}", lFloat.Value);
                }
            }
        }

        /// <summary>
        /// microsecond 단위로 노출값을 설정한다.
        /// </summary>
        /// <param name="value"></param>
        public void SetExposureUS(int value)
        {
            if (!mDevice.IsConnected)
                return;

            bool was_playing = IsPlaying;
            if (was_playing)
                StopAcquisition();

            Thread.Sleep(100);

            try { mDevice.GenParameters.SetEnumValue("ExposureMode", "Timed"); }
            catch { }
            try { mDevice.GenParameters.SetEnumValue("ExposureAuto", "Off"); }
            catch { }
            try { mDevice.GenParameters.SetFloatValue("ExposureTime", value); }
            catch { }
            TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"ExposureMode\", \"Timed\")");
            TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"ExposureAuto\", \"Off\")");
            TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"ExposureTime\", {0})", value);

            if (was_playing)
                StartAcquisition();
        }

        /// <summary>
        /// microsecond 단위로 현재 카메라 노출값을 돌려준다.
        /// </summary>
        /// <param name="value"></param>
        public int GetExposureUS()
        {
            if (!mDevice.IsConnected)
                return 0;

            bool was_playing = IsPlaying;
            if (was_playing)
                StopAcquisition();

            Thread.Sleep(100);

            double time = 0;
            try
            {
                time = mDevice.GenParameters.GetFloatValue("ExposureTime");
                TRACE_SENTECH("mDevice.GenParameters.GetFloatValue(\"ExposureTime\")");
            }
            catch
            {
            }
            if (was_playing)
                StartAcquisition();

            return (int)(time);
        }

        /// <summary>
        /// 다음과 같은 기본 패러미터를 강제로 설정한다.
        ///  - frame rate = 4Hz
        ///  - 노출 모드 : 시간간격
        ///  - 노출 시간 : 10000us = 10ms
        ///  - 자동 노출 : off
        ///  - 카메라IP주소 : 현 상태를 카메라에 영구히 저장
        /// </summary>
        public void SetupDefaultParameters()
        {
            if (!mDevice.IsConnected)
                return;

            bool was_playing = IsPlaying;
            if (was_playing)
                StopAcquisition();

            Thread.Sleep(100);

            // { Continuous=0, SingleFrame=1, MultiFrame=2 }
            mDevice.GenParameters.SetEnumValue("AcquisitionMode", 1);
            TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"AcquisitionMode\", 1)");

            // frame rate
            double fps = 4;
            if (mParam.UseCameraTriggerMode)
                fps = 20;
            mDevice.GenParameters.SetFloatValue("AcquisitionFrameRate", fps);
            TRACE_SENTECH("mDevice.GenParameters.SetFloatValue(\"AcquisitionFrameRate\", {0})", fps);

            // exposure time
            mDevice.GenParameters.SetEnumValue("ExposureMode", "Timed");
            mDevice.GenParameters.SetEnumValue("ExposureAuto", "Off");
            mDevice.GenParameters.SetFloatValue("ExposureTime", (double)mParam.DefaultExposureTime);
            TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"ExposureMode\", \"Timed\")");
            TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"ExposureAuto\", \"Off\")");
            TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"ExposureTime\", {0})", (double)mParam.DefaultExposureTime);

            // default gain = 0
            mDevice.GenParameters.SetFloatValue("Gain", 0.0);

            // 카메라 디버그를 위해 트리거 모드 꺼놓고 (카메라에 이 값이 저장되므로)
            {
                // enum: Off=0 On=1
                mDevice.GenParameters.SetEnumValue("TriggerMode", 0);
                TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"TriggerMode\", 0)");
                // enum: Software=0 Line0=1 Hardware=34
                mDevice.GenParameters.SetEnumValue("TriggerSource", 0);
                TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"TriggerSource\", 0)");
                // enum: Off=0 ReadOut=1 PreviousFrame=2
                mDevice.GenParameters.SetEnumValue("TriggerOverlap", 0);
                TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"TriggerOverlap\", 0)");
            }

            // persistent parameters
            long ipaddr = mDevice.GenParameters.GetIntegerValue("GevCurrentIPAddress");
            mDevice.GenParameters.SetIntegerValue("GevPersistentIPAddress", ipaddr);
            TRACE_SENTECH("mDevice.GenParameters.SetIntegerValue(\"GevPersistentIPAddress\", {0})", ipaddr);

            long subnetmask = mDevice.GenParameters.GetIntegerValue("GevCurrentSubnetMask");
            mDevice.GenParameters.SetIntegerValue("GevPersistentSubnetMask", subnetmask);
            TRACE_SENTECH("mDevice.GenParameters.SetIntegerValue(\"GevPersistentSubnetMask\", {0})", subnetmask);

            mDevice.GenParameters.SetBooleanValue("GevCurrentIPConfigurationPersistentIP", true);
            TRACE_SENTECH("mDevice.GenParameters.SetBooleanValue(\"GevCurrentIPConfigurationPersistentIP\", \"true\")");

            // save persistent parameters
            // refer to page 34 of "ebus_sdk_programmers_guide.pdf" for more information
            mDevice.GenParameters.SetEnumValue("UserSetSelector", "UserSet1");
            mDevice.GenParameters.ExecuteCommand("UserSetSave");
            mDevice.GenParameters.SetEnumValue("UserSetDefaultSelector", "UserSet1");
            TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"UserSetSelector\", \"UserSet1\")");
            TRACE_SENTECH("mDevice.GenParameters.ExecuteCommand(\"UserSetSave\")");
            TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"UserSetDefaultSelector\", \"UserSet1\")");

            // TriggeredMode 세팅
            if (mParam.UseCameraTriggerMode)
            {
                // enum: Off=0 On=1
                mDevice.GenParameters.SetEnumValue("TriggerMode", 1);
                TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"TriggerMode\", 1)");
                // enum: Software=0 Line0=1 Hardware=34
                mDevice.GenParameters.SetEnumValue("TriggerSource", 0);
                TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"TriggerSource\", 0)");
                // enum: Off=0 ReadOut=1 PreviousFrame=2
                mDevice.GenParameters.SetEnumValue("TriggerOverlap", 2);
                TRACE_SENTECH("mDevice.GenParameters.SetEnumValue(\"TriggerOverlap\", 2)");
            }

            if (was_playing)
                StartAcquisition();
        }

        private static object objLog = new object();
        public void TRACE_SENTECH(string str, params object[] args)
        {
            if (!mParam.UseSentechLog)
                return;

            str = string.Format(str, args);

            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            MethodBase mb = sf.GetMethod();

            string function_name = mb.Name;

            lock (objLog)
            {
                DateTime time = DateTime.Now;
                string tTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}.{1:000}", time, time.Millisecond);
                string sLog = "[" + tTime + "](" + nCameraNum.ToString() + ":" + ipaddress + ") (" + function_name + ") " + str;

                string pathName = mParam.LogFilePath;
                string fileName = pathName + "SentechLog.txt";
                FileManagement.VerifyDirectory(pathName);
                FileManagement.AppendTextToFile(fileName, sLog);
            }
        }

        private ICogImage connection_failure_image = null;
        private ICogImage GetConnectionFailureImage()
        {
            if (connection_failure_image == null)
            {
                MessageBox.Show("Capture Fail!");
            }

            return connection_failure_image;
        }
    }

    public class CameraThread
    {
        public string strLog = "";
        public int camera_num;
        public SentechParam m_param;
        public CameraParam m_camera_param;
        FTech_SentechGigE sentech_camera;
        string ipaddress_copy = null;

        public CameraThread() { }
        public CameraThread(int num)
        {
            camera_num = num;
            sentech_camera = new FTech_SentechGigE();
        }
        
        public void SetCameraParam(int num, SentechParam param, CameraParam cameraParam)
        {
            camera_num = num;
            m_param = param;
            m_camera_param = cameraParam;
            sentech_camera.SetSentechGigE(camera_num, m_param, m_camera_param);
        }

        public int GetExposureUS()
        {
            return sentech_camera.GetExposureUS();
        }

        public void SetExposureUS(int value)
        {
            sentech_camera.SetExposureUS(value);
        }

        public bool IsLive()
        {
            return sentech_camera.IsPlaying;
        }

        public bool LiveStart()
        {
            return sentech_camera.StartAcquisition();
        }

        public void LiveStop()
        {
            sentech_camera.StopAcquisition();
        }

        public void ManualGrab()
        {
            sentech_camera.ManualGrab();
        }

        public bool Connect(string ipaddress)
        {
            if (ipaddress == null)
                return false;

            ipaddress_copy = ipaddress;
            sentech_camera.nCameraNum = camera_num;
            if (!sentech_camera.Connect(ipaddress, true))
                return false;

            sentech_camera.SetupDefaultParameters();

            if (m_param.UseCameraTriggerMode)
                return sentech_camera.StartAcquisition();

            return true;
        }

        public void Disconnect()
        {
            if (m_param.UseCameraTriggerMode)
                sentech_camera.StopAcquisition();

            sentech_camera.Disconnect();
        }


        public bool RestartThread()
        {
            Disconnect();
            Thread.Sleep(100);
            return Connect(ipaddress_copy);
        }
    }

    /// <summary>
    /// Sentech Camera 제어를 위한 관리 Class
    /// </summary>
    public class CameraManager
    {
        private static void StartCameraManagerThread()
        {
            m_isRunSentechManagerThread = true;
            sentechManagerThread = new Thread(new ThreadStart(ThreadProcess));
            sentechManagerThread.Name = "CameraManagerThread";
            sentechManagerThread.Start();
        }

        private static void StopCameraManagerThread()
        {
            m_isRunSentechManagerThread = false;
            Thread.Sleep(100);
        }

        private static Thread sentechManagerThread = null;
        private static bool m_isRunSentechManagerThread = true;
        public static int m_cameraNo;
        public static SentechParam m_param;
        public static List<CameraParam> m_listCameraParam;
        public static System.Diagnostics.Stopwatch m_stopWatch = System.Diagnostics.Stopwatch.StartNew();

        private static object lockLiveState = new object();

        public static void SetCameraParam(int cameraNo, SentechParam param, List<CameraParam> listCamParam)
        {
            m_cameraNo = cameraNo;
            m_param = param;
            m_listCameraParam = listCamParam;
        }

        public static void ThreadProcess()
        {
            int accumulateTime = 0;
            m_stopWatch.Start();

            ////  Camera Live를 위한 Emulate Thread
            while (m_isRunSentechManagerThread)
            {
                //////  현재 시간 측정
                m_stopWatch.Stop();
                accumulateTime += (int)m_stopWatch.ElapsedMilliseconds;
                m_stopWatch.Reset();
                m_stopWatch.Start();

                //////  만약 Live할 시간이 되었다면
                int timeStep = 10; // 최대한 빠른(10us) 시간 내에
                if (accumulateTime > timeStep)
                {
                    accumulateTime -= timeStep;

                    //// Live 켜진 카메라들에서 Image 획득 시도
                    for (int i = 0; i < m_param.CameraCount; i++)
                    {
                        lock (lockLiveState)
                        {
                            if (m_isliveStarted[i])
                            {
                                Thread.Sleep(1); // 이거 없으면 거지같은 카메라가 몇개가 제대로 시작이 안된다. (...)
                                m_sentechCamera[i].ManualGrab();
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
            m_stopWatch.Stop();
        }

        public static CameraThread[] m_sentechCamera = new CameraThread[m_param.CameraCount];
        private static bool[] m_isliveStarted = new bool[m_param.CameraCount];
        public static string[] m_ipAddressTable = new string[m_param.CameraCount]; // 카메라 IP address 목록

        public static void Init()
        {
            //// 카메라의 Parameter를 정의한다.
            for (int iCam = 0; iCam < m_param.CameraCount; iCam++)
            {
                m_sentechCamera[iCam].SetCameraParam(m_cameraNo, m_param, m_listCameraParam[iCam]);
            }
            //// 카메라들을 검색하고 IP주소가 잘못된 것은 바로 세팅한다.
            FTech_SentechGigE.PrepareAllCamera();

            //// 카메라 IP목록을 생성한다.
            List<string> IpList = null;
            if (m_param.UseCameraIpAddressAutoAssign)
            {
                IpList = FTech_SentechGigE.GetSortedIpList();
            }
            else
            {
                IpList = FTech_SentechGigE.GetIpListFromFile();
            }

            for (int i = 0; i < Math.Min(IpList.Count, m_param.CameraCount); ++i)
            {
                m_ipAddressTable[i] = IpList[i];
            }

            lock (lockLiveState)
            {
                for (int i = 0; i < m_param.CameraCount; i++)
                    m_isliveStarted[i] = false;
            }

            if (m_param.UseCameraTriggerMode)
                StartCameraManagerThread();
        }

        public static void Term()
        {
            if (m_param.UseCameraTriggerMode)
                StopCameraManagerThread();

            //// 카메라 객체 해제
            for (int i = 0; i < m_param.CameraCount; i++)
            {
                if (m_sentechCamera[i] != null)
                {
                    m_sentechCamera[i].Disconnect();
                }
            }
        }

        public static void OnCameraDisconnected(int camera_index)
        {
            MessageBox.Show("Camera disconnected. reconnecting...");
            Connect(camera_index);
        }

        public static void EnumerateAndListAllCamera()
        {
            FTech_SentechGigE.EnumerateAndListAllCamera();
        }

        public static void Connect(int camera_index)
        {
            m_sentechCamera[camera_index] = new CameraThread(camera_index);
            CameraConnector cc = new CameraConnector(); // 쓰레드로 모든 카메라들에 병렬로 동시에 연결처리한다.
            cc.Run(m_sentechCamera[camera_index], m_ipAddressTable[camera_index]);
        }

        public static List<string> WaitPendingConnection()
        {
            string failed_camera;
            List<string> failed_camera_list = new List<string>();

            // 모든 카메라가 연결 완료될 때 까지 기다린다.
            while (CameraConnector.CheckPendingNum() > 0)
            {
                failed_camera = CameraConnector.FirstFailedItem();
                if (failed_camera != null)
                    failed_camera_list.Add(failed_camera);
                Thread.Sleep(300);
            }

            while ((failed_camera = CameraConnector.FirstFailedItem()) != null)
            {
                failed_camera_list.Add(failed_camera);
            }

            return failed_camera_list;
        }

        public static bool Start(params object[] objs)
        {
            bool success = true;
            foreach (object o in objs)
            {
                int camera_index = -1;
                try
                {
                    camera_index = (int)o;
                }
                catch
                {
                }
                if (camera_index >= 0)
                {
                    // 카메라 노출값을 읽어온 값으로 다시 적용해주고
                    // 여기서 쌩뚱맞게 죽는 경우가 갑자기 또 생겨서 막아버린다.
                    // 노출값은 카메라 연결시에 처음에 한번 해주고있어서 크게 문제는 안된다.
                    // 세팅창에서 변경시 어떻게 되는지는 확인 요망
                    //                    SetExposureUS(camera_index, Recipe.Cur.Camera[camera_index].Exposure);

                    // 카메라 시작
                    if (camera_index >= 0 && camera_index < m_sentechCamera.Length)
                        if (m_sentechCamera[camera_index] != null)
                            if (!m_param.UseCameraTriggerMode)
                            {
                                success &= m_sentechCamera[camera_index].LiveStart();
                            }

                    lock (lockLiveState)
                    {
                        m_isliveStarted[camera_index] = true;
                    }
                }
            }

            ///@        성공 못한 애는 (1회)에 한해 카메라 쓰레드를 새로 만들고 재시작해본다.

            return success;
        }

        public static bool IsLive(int camera_index)
        {
            if (camera_index < 0 || camera_index >= m_sentechCamera.Length)
                return false;
            if (m_sentechCamera[camera_index] == null)
                return false;
            return m_sentechCamera[camera_index].IsLive();
        }

        public static void Stop(params object[] objs)
        {
            foreach (object o in objs)
            {
                int camera_index = -1;
                try
                {
                    camera_index = (int)o;
                }
                catch
                {
                }
                lock (lockLiveState)
                {
                    if (!m_param.UseCameraTriggerMode)
                    {
                        if (camera_index >= 0 && camera_index < m_sentechCamera.Length)
                            if (m_sentechCamera[camera_index] != null)
                                m_sentechCamera[camera_index].LiveStop();
                    }

                    lock (lockLiveState)
                    {
                        m_isliveStarted[camera_index] = false;
                    }
                }
            }
        }

        public static void StartAll()
        {
            Start(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
        }

        public static void StopAll()
        {
            Stop(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17);
        }

        public static int GetExposureUS(int camera_index)
        {
            if (m_sentechCamera[camera_index] == null)
                return 0;
            return m_sentechCamera[camera_index].GetExposureUS();
        }

        public static void SetExposureUS(int camera_index, int exposure_us)
        {
            if (m_sentechCamera[camera_index] != null)
                m_sentechCamera[camera_index].SetExposureUS(exposure_us);
        }

        /// 동기 캡쳐, 영상이 들어올때까지 대기한다.
        public static bool GrabBlock(int camera_index, int time_out = 500)
        {

            return false;
        }

        /// 비동기 캡쳐, 영상이 들어오는 것을 기다리지 않는다.
        public static void GrabAsync(int camera_index)
        {
        }
    }

    /// <summary>
    /// N개 이상의 카메라 연결을 병렬로 동시에 처리하기 위한 Thread 대리자.
    /// </summary>
    class CameraConnector
    {
        private static object obj = new object();
        public static HashSet<Thread> runningCameraConnector = new HashSet<Thread>();

        public static List<string> failedCamera = new List<string>();

        public static string FirstFailedItem()
        {
            string ipaddress = null;
            lock (obj)
            {
                if (failedCamera.Count > 0)
                {
                    ipaddress = failedCamera[0];
                    failedCamera.RemoveAt(0);
                }
            }
            return ipaddress;
        }


        public static int CheckPendingNum()
        {
            return runningCameraConnector.Count;
        }

        private Thread thread = null;
        private CameraThread m_cameraThread = null;
        private string m_ip = null;

        public void Run(CameraThread cameraThread, string ipAddress)
        {
            if (cameraThread == null || ipAddress == null) return;
            m_cameraThread = cameraThread;
            m_ip = ipAddress;
            thread = new Thread(new ThreadStart(ThreadProcess));
            thread.Start();
        }

        public void ThreadProcess()
        {
            lock (obj)
            {
                runningCameraConnector.Add(Thread.CurrentThread);
            }
            if (false == m_cameraThread.Connect(m_ip))
            {
                lock (obj)
                {
                    failedCamera.Add(m_ip);
                }
            }
            lock (obj)
            {
                runningCameraConnector.Remove(Thread.CurrentThread);
            }
        }
    }
}
