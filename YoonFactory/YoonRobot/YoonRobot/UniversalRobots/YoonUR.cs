using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoonFactory.Param;
using YoonFactory.Comm;
using YoonFactory.Comm.TCP;

namespace YoonFactory.Robot.UniversialRobot
{
    public class YoonUR : IYoonRemote
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
                    CloseConnect();
                    m_pTcpServer?.Dispose();
                    m_pTcpClient?.Dispose();
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                m_pTcpServer = null;
                m_pTcpClient = null;
                m_pConnectManager = null;
                m_pPacketManager = null;

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        ~YoonUR()
        {
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

        public string RootDirectory { get; private set; } = Path.Combine(Directory.GetCurrentDirectory(), @"YoonFactory", @"YoonUR");

        public IYoonParameter ConnectionParameter => m_pConnectManager.Parameter;

        public IYoonParameter PacketParameter => m_pPacketManager.Parameter;

        public event ShowMessageCallback OnShowMessageEvent;
        public event RecieveDataCallback OnShowReceiveDataEvent;

        private YoonServer m_pTcpServer = null;
        private YoonClient m_pTcpClient = null;
        private YoonParameter m_pConnectManager = null;
        private YoonParameter m_pPacketManager = null;

        public YoonUR()
        {
            // 서버용 구독 초기화
            m_pTcpClient = new YoonClient();
            m_pTcpClient.OnShowMessageEvent += OnShowMessageEvent;
            m_pTcpClient.OnShowReceiveDataEvent += OnShowReceiveDataEvent;
            // 클라이언트용 구독 초기화
            m_pTcpServer = new YoonServer();
            m_pTcpServer.OnShowMessageEvent += OnShowMessageEvent;
            m_pTcpServer.OnShowReceiveDataEvent += OnShowReceiveDataEvent;
            // Parameter 초기화하기
            ParameterConnect pParamConnect = new ParameterConnect();
            ParameterPacket pParamPacket = new ParameterPacket();
            m_pConnectManager = new YoonParameter(pParamConnect, typeof(ParameterConnect));
            m_pPacketManager = new YoonParameter(pParamPacket, typeof(ParameterPacket));
            m_pConnectManager.RootDirectory = RootDirectory;
            m_pPacketManager.RootDirectory = RootDirectory;
            m_pConnectManager.LoadParameter(true);
            m_pPacketManager.LoadParameter(true);
        }

        public YoonUR(YoonUR pRemote)
        {
            RootDirectory = pRemote.RootDirectory;

            // 서버용 구독 초기화
            m_pTcpClient = new YoonClient();
            m_pTcpClient.OnShowMessageEvent += OnShowMessageEvent;
            m_pTcpClient.OnShowReceiveDataEvent += OnShowReceiveDataEvent;
            // 클라이언트용 구독 초기화
            m_pTcpServer = new YoonServer();
            m_pTcpServer.OnShowMessageEvent += OnShowMessageEvent;
            m_pTcpServer.OnShowReceiveDataEvent += OnShowReceiveDataEvent;
            // Parameter 초기화하기
            m_pConnectManager = new YoonParameter(pRemote.ConnectionParameter, typeof(ParameterConnect));
            m_pPacketManager = new YoonParameter(pRemote.PacketParameter, typeof(ParameterPacket));
            m_pConnectManager.RootDirectory = RootDirectory;
            m_pPacketManager.RootDirectory = RootDirectory;
            m_pConnectManager.LoadParameter(false);
            m_pPacketManager.LoadParameter(false);
        }

        public YoonUR(string strRootDir)
        {
            RootDirectory = strRootDir;

            // 서버용 구독 초기화
            m_pTcpClient = new YoonClient();
            m_pTcpClient.OnShowMessageEvent += OnShowMessageEvent;
            m_pTcpClient.OnShowReceiveDataEvent += OnShowReceiveDataEvent;
            // 클라이언트용 구독 초기화
            m_pTcpServer = new YoonServer();
            m_pTcpServer.OnShowMessageEvent += OnShowMessageEvent;
            m_pTcpServer.OnShowReceiveDataEvent += OnShowReceiveDataEvent;
            // Parameter 초기화하기
            ParameterConnect pParamConnect = new ParameterConnect();
            ParameterPacket pParamPacket = new ParameterPacket();
            m_pConnectManager = new YoonParameter(pParamConnect, typeof(ParameterConnect));
            m_pPacketManager = new YoonParameter(pParamPacket, typeof(ParameterPacket));
            m_pConnectManager.RootDirectory = RootDirectory;
            m_pPacketManager.RootDirectory = RootDirectory;
            m_pConnectManager.LoadParameter(true);
            m_pPacketManager.LoadParameter(true);
        }

        public IYoonRemote Clone()
        {
            return new YoonUR(this);
        }

        public void CopyFrom(IYoonRemote pRemote)
        {
            if(pRemote is YoonUR pRemoteUR)
            {
                RootDirectory = pRemoteUR.RootDirectory;
                m_pConnectManager = new YoonParameter(pRemote.ConnectionParameter, typeof(ParameterConnect));
                m_pPacketManager = new YoonParameter(pRemote.PacketParameter, typeof(ParameterPacket));
                m_pConnectManager.RootDirectory = RootDirectory;
                m_pPacketManager.RootDirectory = RootDirectory;
                m_pConnectManager.LoadParameter(false);
                m_pPacketManager.LoadParameter(false);
            }
        }

        public bool OpenConnect()
        {
            throw new NotImplementedException();
        }

        public bool CloseConnect()
        {
            throw new NotImplementedException();
        }

        public bool StartRobot()
        {
            throw new NotImplementedException();
        }

        public bool StopRobot()
        {
            throw new NotImplementedException();
        }

        public bool ResetRobot()
        {
            throw new NotImplementedException();
        }

        public bool SendSocket(string strMessage)
        {
            throw new NotImplementedException();
        }

        public bool SendSocket(string[] pMessage)
        {
            throw new NotImplementedException();
        }

        public void LoadParameter()
        {
            throw new NotImplementedException();
        }

        public void SaveParameter()
        {
            throw new NotImplementedException();
        }

        private string EncodingMessage(eYoonHeadSend nHeader)
        {
            string strMessage = string.Empty;
            ParameterPacket pParam = PacketParameter as ParameterPacket;

            switch (nHeader)
            {
                case eYoonHeadSend.StatusRun:
                    strMessage = "programState";
                    break;
                case eYoonHeadSend.LoadProject:
                    if (pParam.ProgramName == string.Empty) break;
                    if (pParam.ProjectName != string.Empty)
                        strMessage = "Load " + string.Format("/{0}/{1}.urp", pParam.ProjectName, pParam.ProgramName);
                    else
                        strMessage = "Load " + string.Format("/{0}.urp", pParam.ProgramName);
                    break;
                case eYoonHeadSend.Play:
                    strMessage = "Play";
                    break;
                case eYoonHeadSend.Pause:
                    strMessage = "Pause";
                    break;
                case eYoonHeadSend.Stop:
                    strMessage = "Stop";
                    break;
                case eYoonHeadSend.Quit:
                    strMessage = "Quit";
                    break;
                case eYoonHeadSend.Shutdown:
                    strMessage = "Shutdown";
                    break;
                case eYoonHeadSend.PowerOn:
                    strMessage = "power on";
                    break;
                case eYoonHeadSend.PowerOff:
                    strMessage = "power off";
                    break;
                case eYoonHeadSend.BreakRelease:
                    strMessage = "brake release";
                    break;
                default:
                    break;
            }
            if (strMessage != string.Empty)
                strMessage += System.Environment.NewLine;   // Input \r\n (개행문자)

            return strMessage;
        }

        private eYooneHeadReceive DecodingMessage(string strMessage)
        {
            eYooneHeadReceive nHeader = eYooneHeadReceive.None;

            if (strMessage.Contains("PLAYING"))
                nHeader = eYooneHeadReceive.StatusRunOK;
            else if (strMessage.Contains("STOPPED") || strMessage.Contains("PAUSED"))
                nHeader = eYooneHeadReceive.StatusRunNG;
            else if (strMessage.Contains("Loading program"))
                nHeader = eYooneHeadReceive.LoadOK;
            else if (strMessage.Contains("File not found") || strMessage.Contains("Error while loading"))
                nHeader = eYooneHeadReceive.LoadNG;
            else if (strMessage.Contains("Starting program"))
                nHeader = eYooneHeadReceive.PlayOK;
            else if (strMessage.Contains("Failed to execute: Play"))
                nHeader = eYooneHeadReceive.PlayNG;
            else if (strMessage.Contains("Stopped"))
                nHeader = eYooneHeadReceive.StopOk;
            else if (strMessage.Contains("Failed to execute: Stop"))
                nHeader = eYooneHeadReceive.StopNG;
            else if (strMessage.Contains("Pausing"))
                nHeader = eYooneHeadReceive.PauseOK;
            else if (strMessage.Contains("Failed to execute: Pause"))
                nHeader = eYooneHeadReceive.PauseNG;
            else if (strMessage.Contains("Disconnected"))
                nHeader = eYooneHeadReceive.QuitOK;
            else if (strMessage.Contains("Shutting down"))
                nHeader = eYooneHeadReceive.ShuwdownOK;
            else if (strMessage.Contains("running: true"))
                nHeader = eYooneHeadReceive.StatusRunOK;
            else if (strMessage.Contains("running: false"))
                nHeader = eYooneHeadReceive.StatusRunNG;
            else if (strMessage.Contains("Powering on"))
                nHeader = eYooneHeadReceive.PowerOnOK;
            else if (strMessage.Contains("Powering off"))
                nHeader = eYooneHeadReceive.PowerOffOK;
            else if (strMessage.Contains("Brake releasing"))
                nHeader = eYooneHeadReceive.BreakReleaseOK;

            return nHeader;
        }

        private eYoonRemoteType GetRemoteCommType(eYoonHeadSend nHeaderData)
        {
            switch (nHeaderData)
            {
                case eYoonHeadSend.StatusRun:
                case eYoonHeadSend.StatusServo:
                case eYoonHeadSend.StatusError:
                case eYoonHeadSend.StatusSafety:
                case eYoonHeadSend.Reset:
                case eYoonHeadSend.Play:
                case eYoonHeadSend.Pause:
                case eYoonHeadSend.Stop:
                case eYoonHeadSend.Quit:
                case eYoonHeadSend.Shutdown:
                case eYoonHeadSend.PowerOn:
                case eYoonHeadSend.PowerOff:
                case eYoonHeadSend.BreakRelease:
                    return eYoonRemoteType.Dashboard;
            }
            return eYoonRemoteType.None;
        }

        private eYoonStatus GetRemoteStatus(eYooneHeadReceive nHeader)
        {
            if (nHeader.ToString().Contains("OK"))
                return eYoonStatus.OK;
            else if (nHeader.ToString().Contains("NG"))
                return eYoonStatus.NG;
            else
                return eYoonStatus.User;
        }

        private string GetRemoteLogFromReceiveHeader(eYooneHeadReceive nHead)
        {
            switch (nHead)
            {
                case eYooneHeadReceive.StatusServoOK:
                    return "Servo On";
                case eYooneHeadReceive.StatusServoNG:
                    return "Servo Off";
                case eYooneHeadReceive.StatusErrorOK:
                    return "Robot Error";
                case eYooneHeadReceive.StatusErrorNG:
                    return "Robot Error Clear";
                case eYooneHeadReceive.StatusRunOK:
                    return "Robot Running";
                case eYooneHeadReceive.StatusRunNG:
                    return "Robot Running Failure";
                case eYooneHeadReceive.StatusSafetyOK:
                    return "Workspace Safety";
                case eYooneHeadReceive.StatusSafetyNG:
                    return "Workspace Warning";
                case eYooneHeadReceive.LoadOK:
                    return "Load Success";
                case eYooneHeadReceive.LoadNG:
                    return "Load Failure";
                case eYooneHeadReceive.ResetOK:
                    return "Reset Completed";
                case eYooneHeadReceive.ResetNG:
                    return "Reset Failure";
                case eYooneHeadReceive.PlayOK:
                    return "Play";
                case eYooneHeadReceive.PlayNG:
                    return "Play Error";
                case eYooneHeadReceive.PauseOK:
                    return "Pause";
                case eYooneHeadReceive.PauseNG:
                    return "Pause Error";
                case eYooneHeadReceive.StopOk:
                    return "Stop";
                case eYooneHeadReceive.StopNG:
                    return "Stop Error";
                case eYooneHeadReceive.QuitOK:
                    return "Quit";
                case eYooneHeadReceive.ShuwdownOK:
                    return "Quit Error";
                case eYooneHeadReceive.PowerOnOK:
                    return "Robot Power On";
                case eYooneHeadReceive.PowerOffOK:
                    return "Robot Power Off";
                case eYooneHeadReceive.BreakReleaseOK:
                    return "Break Release";
                default:
                    return string.Empty;
            }
        }
    }
}
