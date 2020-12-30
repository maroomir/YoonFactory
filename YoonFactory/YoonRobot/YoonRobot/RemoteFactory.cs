using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Timers;
using YoonFactory.Comm;
using YoonFactory.Comm.TCP;

namespace YoonFactory.Robot.Remote
{
    public class RemoteFactory : IDisposable
    {
        public enum eStepRemote : int
        {
            Wait = -1,
            Listen,
            Connect,
            Retry,
            Send,
            Receive,
            Disconnect,
            Close,
            Release,
        }

        public delegate void ProcessCompleteCallback(object sender, ProcessArgs e);
        public class ProcessArgs : EventArgs
        {
            public eStepRemote CurrentStep;
            public bool IsWaitContinue;
            public bool IsJobOK;

            public ProcessArgs(eStepRemote nStep)
            {
                CurrentStep = nStep;
                IsWaitContinue = true;
                IsJobOK = false;
            }

            public ProcessArgs(eStepRemote nStep, eYoonStatus nStatus)
            {
                CurrentStep = nStep;
                IsWaitContinue = false;
                IsJobOK = (nStatus != eYoonStatus.NG) ? true : false;
            }
        }

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

                //// Release Socket Thread
                StopRemote();
                m_fThreadServer = null;
                m_fThreadClient = null;

                m_pTCPServer.Dispose();
                m_pTCPClient.Dispose();

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        public RemoteFactory()
        {
            m_nJobReservation = eStepRemote.Wait;
            // 서버용 구독
            m_pTCPServer = new YoonServer();
            m_pTCPServer.OnShowReceiveDataEvent += OnTcpReceiveDataEvent;
            m_pTCPServer.OnShowMessageEvent += OnShowTcpMessageEvent;
            // 클라이언트용 구독
            m_pTCPClient = new YoonClient();
            m_pTCPClient.OnShowReceiveDataEvent += OnTcpReceiveDataEvent;
            m_pTCPClient.OnShowMessageEvent += OnShowTcpMessageEvent;
            // Remote Info 가져오기
            m_strYoonFactoryDir = Path.Combine(Directory.GetCurrentDirectory(), @"YoonFactory");
            RemoteInfo = new RemoteContainer();
            RemoteInfo.RootDirectory = m_strYoonFactoryDir;
            if (!RemoteInfo.LoadAll())
            {
                Console.WriteLine("RemoteFactory : Load Failure, Create New Data In Default");
                RemoteInfo = new RemoteContainer();
                RemoteInfo.RootDirectory = m_strYoonFactoryDir;
                RemoteInfo.SaveAll();
            }
            // 내부 Event 구독
            OnProcessCompleteEvent += OnRemoteProcessCompleteEvent;
            IsInit = false;
        }

        public RemoteFactory(string strDirectory)
        {
            m_nJobReservation = eStepRemote.Wait;
            // 서버용 구독
            m_pTCPServer = new YoonServer();
            m_pTCPServer.OnShowReceiveDataEvent += OnTcpReceiveDataEvent;
            m_pTCPServer.OnShowMessageEvent += OnShowTcpMessageEvent;
            // 클라이언트용 구독
            m_pTCPClient = new YoonClient();
            m_pTCPClient.OnShowReceiveDataEvent += OnTcpReceiveDataEvent;
            m_pTCPClient.OnShowMessageEvent += OnShowTcpMessageEvent;
            // Remote_Info 가져오기
            m_strYoonFactoryDir = strDirectory;
            RemoteInfo = new RemoteContainer();
            RemoteInfo.RootDirectory = m_strYoonFactoryDir;
            if (!RemoteInfo.LoadAll())
            {
                Console.WriteLine("RemoteFactory : Load Failure, Create New Data In Default");
                RemoteInfo = new RemoteContainer();
                RemoteInfo.RootDirectory = m_strYoonFactoryDir;
                RemoteInfo.SaveAll();
            }
            // 내부 Event 구독
            OnProcessCompleteEvent += OnRemoteProcessCompleteEvent;
            IsInit = false;
        }

        ~RemoteFactory()
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

        private bool m_bJobPause = false;
        private Thread m_fThreadServer = null;
        private Thread m_fThreadClient = null;
        private eStepRemote m_nJobReservation;
        private string m_strSendMessage = "";
        private string m_strReceiveMessage = "";
        private string m_strYoonFactoryDir = "";
        private YoonServer m_pTCPServer = null;
        private YoonClient m_pTCPClient = null;
        private ParamRobotReceive m_pParamDataReceive = new ParamRobotReceive();
        //private bool IsConnected; // 삭제사유 : WaitHandle(int ms)를 만들어서 Connect / Send / Receive 대체 (ex> Connect시 IsConnect = true => WaitHandle=true)
        //private bool IsSend;      // 개발효과 : Main 함수에서 Wait를 위한 쓰레드를 추가로 구성할 필요 없음 
        //private bool IsReceive;   // 사용분야(BM) : 바슬러 카메라 제어, Intel Realsense 제어시
        private bool m_bWaitHandle = false;
        private bool m_bCompleteProcess = false;
        private eYoonStatus m_nCurrentStatusReceive = eYoonStatus.User;
        private event ProcessCompleteCallback OnProcessCompleteEvent;

        #region Public Access
        public eYoonRobotType CobotType { get; private set; }
        public eYoonRobotRemote CommType { get; private set; }
        public bool IsInit { get; private set; }
        public bool IsConnected { get; private set; }
        public RemoteContainer RemoteInfo { get; private set; }
        // To Enable Subscript Event
        public event RemoteLogCallback OnDisplayLogEvent;
        public event RemoteResultCallback OnPassResultEvent;
        #endregion

        #region Event 처리
        private void OnTcpReceiveDataEvent(object sender, BufferArgs e)
        {
            m_bJobPause = true;
            m_strReceiveMessage = e.StringData;

            //// Message Decoding
            eYoonRobotRemoteHeadReceive nHeader = RemoteSupport.GetRemoteReceiveHeader(CobotType, e.StringData, ref m_pParamDataReceive);

            OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.User, string.Format("Decoding Message Head - " + Enum.GetName(typeof(eYoonRobotRemoteHeadReceive), nHeader))));
            {
                //// Status 확인 및 출력
                m_nCurrentStatusReceive = RemoteSupport.GetRemoteStatus(nHeader);
                OnPassResultEvent(sender, new RemoteResultArgs(nHeader, m_pParamDataReceive, e.StringData));
                OnDisplayLogEvent(sender, new RemoteLogArgs(m_nCurrentStatusReceive, RemoteSupport.GetRemoteLogFromReceiveHeader(nHeader)));
            }

            //// TCP Step Change
            m_nJobReservation = eStepRemote.Receive;
            m_bJobPause = false;
        }

        private void OnShowTcpMessageEvent(object sender, MessageArgs e)
        {
            OnDisplayLogEvent(sender, new RemoteLogArgs(e.Status, e.Message));
        }
        #endregion

        #region Remote Class 초기화
        public bool InitRemote(RemoteContainer pRemoteInfo)
        {
            IsInit = false;
            //// TCP Thread가 이미 실행되고 있으면 중단
            if (m_fThreadServer != null || m_fThreadClient != null)
            {
                StopRemote();
                Thread.Sleep(200);
            }
            //// Remote Info 변경
            if (RemoteInfo == null)
                RemoteInfo = new RemoteContainer();
            RemoteInfo.CopyFrom(pRemoteInfo);
            RemoteInfo.RootDirectory = m_strYoonFactoryDir;
            if (RemoteInfo.SaveAll())
                IsInit = true;
            else
                OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Init Remote Data Failure : Save Error"));

            return IsInit;
        }

        public bool InitRemote()
        {
            IsInit = false;
            //// TCP Thread가 이미 실행되고 있으면 중단
            if (m_fThreadServer != null || m_fThreadClient != null)
            {
                StopRemote();
                Thread.Sleep(200);
            }
            //// Remote Info 변경
            if (RemoteInfo == null)
                RemoteInfo = new RemoteContainer();
            if (RemoteInfo.SaveAll())
                IsInit = true;
            else
                OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Init Remote Data Failure : Save Error"));

            return IsInit;
        }

        public bool StartRemote(eYoonRobotType nCobot, eYoonRobotRemote nComm)
        {
            if (RemoteInfo == null || IsInit == false) return false;

            CobotType = nCobot;
            CommType = nComm;
            IsConnected = false;
            m_bJobPause = true; // WaitHandle + DoHandle에서 JobPause 해제시킴
            eYoonCommType nTCPType = RemoteInfo.GetValue(CobotType, CommType).TCPMode;
            if ((int)nTCPType > Enum.GetValues(typeof(eYoonCommType)).Length - 1)
            {
                OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, string.Format("Unable to Start Threading")));
                return false;
            }

            switch (nTCPType)
            {
                case eYoonCommType.TCPServer:
                    //// Setting TCP Server
                    m_pTCPServer.Port = RemoteInfo.GetValue(CobotType, CommType).Port;
                    //// Start TCP Server Threading
                    m_nJobReservation = eStepRemote.Listen;
                    if (m_fThreadServer == null || !m_fThreadServer.IsAlive)
                        m_fThreadServer = new Thread(new ThreadStart(ProcessServerControl));
                    else
                    {
                        OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, string.Format("Previous Thread is Duplicated")));
                        return false;
                    }
                    try
                    {
                        if (m_fThreadServer.ThreadState == ThreadState.Unstarted)
                            m_fThreadServer.Start();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    break;
                case eYoonCommType.TCPClient:
                    //// Setting TCP Client
                    m_pTCPClient.Address = RemoteInfo.GetValue(CobotType, CommType).IPAddress;
                    m_pTCPClient.Port = RemoteInfo.GetValue(CobotType, CommType).Port;
                    //// Start TCP Server Threading
                    m_nJobReservation = eStepRemote.Connect;
                    if (m_fThreadClient == null || !m_fThreadClient.IsAlive)
                        m_fThreadClient = new Thread(new ThreadStart(ProcessClientControl));
                    else
                    {
                        OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, string.Format("Previous Thread is Duplicated")));
                        return false;
                    }
                    try
                    {
                        if (m_fThreadClient.ThreadState == ThreadState.Unstarted)
                            m_fThreadClient.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    break;
                default:
                    return false;
            }

            OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.User, "Start Remote Thread Success"));
            return true;
        }

        public void StopRemote()
        {
            foreach (eYoonCommType nTcpType in Enum.GetValues(typeof(eYoonCommType)))
            {
                if (nTcpType == eYoonCommType.None) continue;
                StopRemote(nTcpType);
            }
        }

        public bool StopRemote(eYoonCommType nCommType)
        {
            if ((int)nCommType > Enum.GetValues(typeof(eYoonCommType)).Length - 1)
            {
                OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Unable to Stop Threading"));
                return false;
            }

            m_nJobReservation = eStepRemote.Release;
            Thread.Sleep(200);

            switch (nCommType)
            {
                case eYoonCommType.TCPServer:
                    if (m_fThreadServer == null) return false;
                    while (m_fThreadServer.ThreadState == ThreadState.Running)
                        Thread.Sleep(100);
                    m_fThreadServer.Abort();
                    m_fThreadServer.Join();
                    break;
                case eYoonCommType.TCPClient:
                    if (m_fThreadClient == null) return false;
                    while (m_fThreadClient.ThreadState == ThreadState.Running)
                        Thread.Sleep(100);
                    m_fThreadClient.Abort();
                    m_fThreadClient.Join();
                    break;
                default:
                    return false;
            }

            string strThreadName = Enum.GetName(typeof(eYoonCommType), nCommType);
            OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, string.Format("Stop Thread {0} Success", strThreadName)));

            return true;
        }
        #endregion

        #region Robot 조종용 함수 (사용자가 사용하는 함수)
        public bool ToRemoteRobotForced(eYoonRobotType nCobot, eYoonRobotRemote nComm, string strMessage)
        {
            if (nCobot != CobotType || nComm != CommType)
            {
                OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Control Type isnot Matched"));
                return false;
            }

            m_bJobPause = true; // WaitHandle + DoHandle에서 JobPause 해제시킴
            Thread.Sleep(100);
            OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Send Message Forced :" + strMessage));
            {
                m_strSendMessage = strMessage;
                m_nJobReservation = eStepRemote.Send;
            }

            return true;
        }

        public bool ToStartRobot(eYoonRobotType nCobot)
        {
            if (nCobot != CobotType || CommType != RemoteSupport.GetRemoteCommType(nCobot, eYoonRobotRemoteHeadSend.Play))
            {
                OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Control Type isnot Matched"));
                return false;
            }

            m_bJobPause = true; // WaitHandle + DoHandle에서 JobPause 해제시킴
            Thread.Sleep(100);

            bool bResultJob = false;
            switch (nCobot)
            {
                case eYoonRobotType.UR:
                    //// 1. Power On
                    m_strSendMessage = RemoteSupport.GetRemoteForcedMessage(nCobot, eYoonRobotRemoteHeadSend.PowerOn, new ParamRobotSend());
                    m_nJobReservation = eStepRemote.Send;
                    if (!WaitHandle()) break;
                    //// 2. Break Release
                    m_strSendMessage = RemoteSupport.GetRemoteForcedMessage(nCobot, eYoonRobotRemoteHeadSend.BreakRelease, new ParamRobotSend());
                    m_nJobReservation = eStepRemote.Send;
                    if (!WaitHandle()) break;
                    //// 3. Play
                    m_strSendMessage = RemoteSupport.GetRemoteForcedMessage(nCobot, eYoonRobotRemoteHeadSend.Play, new ParamRobotSend());
                    m_nJobReservation = eStepRemote.Send;
                    if (!WaitHandle()) break;
                    bResultJob = true;
                    break;
                default:
                    break;
            }
            if (bResultJob) OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.OK, "Robot Start"));
            else OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.NG, "Start Error"));

            return bResultJob;
        }

        public bool ToPauseRobot(eYoonRobotType nCobot)
        {
            if (nCobot != CobotType || CommType != RemoteSupport.GetRemoteCommType(nCobot, eYoonRobotRemoteHeadSend.Pause))
            {
                OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Control Type isnot Matched"));
                return false;
            }

            m_bJobPause = true; // WaitHandle + DoHandle에서 JobPause 해제시킴
            Thread.Sleep(100);

            bool bResultJob = false;
            switch (nCobot)
            {
                case eYoonRobotType.UR:
                    m_strSendMessage = RemoteSupport.GetRemoteForcedMessage(nCobot, eYoonRobotRemoteHeadSend.Pause, new ParamRobotSend());
                    m_nJobReservation = eStepRemote.Send;
                    if (!WaitHandle()) break;
                    bResultJob = true;
                    break;
                default:
                    break;
            }
            if (bResultJob) OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.OK, "Robot Pause"));
            else OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.OK, "Pause Error"));

            return bResultJob;
        }

        public bool ToInitRobot(eYoonRobotType nCobot, string strProjectName, string strProgramName)
        {
            if (strProgramName == string.Empty)
            {
                OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Program Name is empty"));
                return false;
            }
            if (nCobot != CobotType || CommType != RemoteSupport.GetRemoteCommType(nCobot, eYoonRobotRemoteHeadSend.LoadProject))
            {
                OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Control Type isnot Matched"));
                return false;
            }

            ParamRobotSend pParamSend = new ParamRobotSend();
            pParamSend.ProjectName = strProjectName;
            pParamSend.ProgramName = strProgramName;
            m_bJobPause = true; // WaitHandle + DoHandle에서 JobPause 해제시킴
            Thread.Sleep(100);

            bool bResultJob = false;
            switch (nCobot)
            {
                case eYoonRobotType.UR:
                    //// 1. Load Job Path (//____//____.urp)
                    m_strSendMessage = RemoteSupport.GetRemoteForcedMessage(nCobot, eYoonRobotRemoteHeadSend.LoadJob, pParamSend);
                    m_nJobReservation = eStepRemote.Send;
                    if (!WaitHandle()) break;
                    bResultJob = true;
                    break;
                default:
                    break;
            }
            if (bResultJob) OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.OK, "Robot Init"));
            else OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.OK, "Init Error"));

            return bResultJob;
        }

        public bool ToResetRobot(eYoonRobotType nCobot)
        {
            if (nCobot != CobotType || CommType != RemoteSupport.GetRemoteCommType(nCobot, eYoonRobotRemoteHeadSend.Reset))
            {
                OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Control Type isnot Matched"));
                return false;
            }

            m_bJobPause = true; // WaitHandle + DoHandle에서 JobPause 해제시킴
            Thread.Sleep(100);

            bool bResultJob = false;
            switch (nCobot)
            {
                default:
                    break;
            }
            if (bResultJob) OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.OK, "Robot Reset"));
            else OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.OK, "Reset Error"));

            return bResultJob;
        }

        public bool ToStopRobot(eYoonRobotType nCobot)
        {
            if (nCobot != CobotType || CommType != RemoteSupport.GetRemoteCommType(nCobot, eYoonRobotRemoteHeadSend.Stop))
            {
                OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Control Type isnot Matched"));
                return false;
            }

            m_bJobPause = true; // WaitHandle + DoHandle에서 JobPause 해제시킴
            Thread.Sleep(100);

            bool bResultJob = false;
            switch (nCobot)
            {
                case eYoonRobotType.UR:
                    m_strSendMessage = RemoteSupport.GetRemoteForcedMessage(nCobot, eYoonRobotRemoteHeadSend.Stop, new ParamRobotSend());
                    m_nJobReservation = eStepRemote.Send;
                    if (!WaitHandle()) break;
                    bResultJob = true;
                    break;
                default:
                    break;
            }
            if (bResultJob) OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.OK, "Robot Pause"));
            else OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.OK, "Pause Error"));

            return bResultJob;
        }

        public bool ToRequestRobotStatus(eYoonRobotType nCobot)
        {
            if (nCobot != CobotType || CommType != RemoteSupport.GetRemoteCommType(nCobot, eYoonRobotRemoteHeadSend.StatusRun))
            {
                OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Control Type isnot Matched"));
                return false;
            }

            m_bJobPause = true; // WaitHandle + DoHandle에서 JobPause 해제시킴
            Thread.Sleep(100);

            bool bResultJob = false;
            switch (nCobot)
            {
                case eYoonRobotType.UR:
                    m_strSendMessage = RemoteSupport.GetRemoteForcedMessage(nCobot, eYoonRobotRemoteHeadSend.StatusRun, new ParamRobotSend());
                    m_nJobReservation = eStepRemote.Send;
                    if (!WaitHandle()) break;
                    bResultJob = true;
                    break;
                default:
                    break;
            }
            return bResultJob;
        }

        public bool ToRequestSafetyStatus(eYoonRobotType nCobot)
        {
            if (nCobot != CobotType || CommType != RemoteSupport.GetRemoteCommType(nCobot, eYoonRobotRemoteHeadSend.StatusSafety))
            {
                OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Control Type isnot Matched"));
                return false;
            }

            m_bJobPause = true; // WaitHandle + DoHandle에서 JobPause 해제시킴
            Thread.Sleep(100);

            bool bResultJob = false;
            switch (nCobot)
            {
                case eYoonRobotType.UR:
                    m_strSendMessage = RemoteSupport.GetRemoteForcedMessage(nCobot, eYoonRobotRemoteHeadSend.StatusSafety, new ParamRobotSend());
                    m_nJobReservation = eStepRemote.Send;
                    if (!WaitHandle()) break;
                    bResultJob = true;
                    break;
                default:
                    break;
            }
            return bResultJob;
        }

        public void DoHandle()
        {
            m_bJobPause = false;   // 동작 진행을 위해 JobPause를 해제시킴
        }

        public bool WaitHandle(double dMs = 1000)
        {
            System.Timers.Timer pTimer = new System.Timers.Timer(dMs);
            pTimer.Elapsed += OnWaitTimerElapsed;
            pTimer.AutoReset = false;
            pTimer.Enabled = true;

            m_bWaitHandle = true;
            m_bCompleteProcess = false;

            DoHandle();
            pTimer.Start();
            while (m_bWaitHandle) Thread.Sleep(10);
            pTimer.Stop();
            pTimer.Dispose();

            return m_bCompleteProcess;
        }

        private void OnWaitTimerElapsed(object sender, ElapsedEventArgs e)
        {
            m_bJobPause = true;
            OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.Error, "Wait Time Over"));
            {
                m_nJobReservation = eStepRemote.Wait;
                m_bCompleteProcess = false;
                m_bWaitHandle = false;
            }
            m_bJobPause = false;
        }

        private void OnRemoteProcessCompleteEvent(object sender, ProcessArgs e)
        {
            m_bJobPause = true;

            OnDisplayLogEvent(this, new RemoteLogArgs(eYoonStatus.User, "Remote Complete - " + e.CurrentStep.ToString()));
            {
                m_nJobReservation = eStepRemote.Wait;
                m_bCompleteProcess = e.IsJobOK;
                m_bWaitHandle = e.IsWaitContinue;
            }
            m_bJobPause = false;
        }
        #endregion

        #region Remote용 TCP/IP Control Process
        private void ProcessServerControl()
        {
            if (m_pTCPServer == null) return;

            bool bRun = true;

            eStepRemote nJobStep = eStepRemote.Wait;
            eStepRemote nJobStepBK = eStepRemote.Wait;

            while (bRun)
            {
                if (m_bJobPause)
                    continue;

                //// 어떠한 상황에서도 무조건 RELEASE 선언에 순응해야 함
                if (m_nJobReservation == eStepRemote.Release)
                {
                    nJobStep = m_nJobReservation;
                }

                switch (nJobStep)
                {
                    case eStepRemote.Wait:
                        //// 대기 상태에서 연결이 갑작스럽게 끊긴 경우 재연결을 시도함
                        if(nJobStepBK == eStepRemote.Wait && !m_pTCPServer.IsConnected)
                        {
                            nJobStep = eStepRemote.Listen;
                        }
                        //// 연결이 CLOSE 되었다면 다시 OPEN할때까지 기다려야 함
                        else if (nJobStepBK == eStepRemote.Close && m_nJobReservation == eStepRemote.Listen)
                        {
                            nJobStep = m_nJobReservation;
                        }
                        //// 대기 상태에서 다른 임무를 예약받은 경우 해당 예약 업무를 시행함
                        else if (nJobStepBK == eStepRemote.Wait && m_nJobReservation != eStepRemote.Wait)
                        {
                            nJobStep = m_nJobReservation;
                        }
                        //// Client 연결 확인상태에서 다른 Client와 연결되었을 경우 대기상태로 전환
                        else if (nJobStepBK == eStepRemote.Listen && m_pTCPServer.IsConnected)
                        {
                            nJobStepBK = nJobStep;
                            IsConnected = true;
                            OnProcessCompleteEvent(this, new ProcessArgs(eStepRemote.Connect, eYoonStatus.OK));
                        }
                        //// 전송확인이 완료되면 대기 상태로 전환
                        else if (nJobStepBK == eStepRemote.Send && m_pTCPServer.IsSend)
                        {
                            nJobStepBK = nJobStep;
                            OnProcessCompleteEvent(this, new ProcessArgs(eStepRemote.Send));
                        }
                        break;
                    case eStepRemote.Listen:
                        if (m_pTCPServer.Listen())
                        {
                            nJobStepBK = nJobStep;
                            nJobStep = eStepRemote.Wait;
                            OnProcessCompleteEvent(this, new ProcessArgs(eStepRemote.Listen));
                        }
                        else
                        {
                            m_pTCPServer.OnRetryThreadStart();
                            nJobStep = eStepRemote.Retry;
                        }
                        break;
                    case eStepRemote.Retry:
                        if (m_pTCPServer.IsBound)
                        {
                            nJobStepBK = eStepRemote.Listen;
                            nJobStep = eStepRemote.Wait;
                            OnProcessCompleteEvent(this, new ProcessArgs(eStepRemote.Listen));
                        }
                        break;
                    case eStepRemote.Send:
                        if (m_strSendMessage != string.Empty)
                            m_pTCPServer.Send(m_strSendMessage);
                        nJobStepBK = nJobStep;
                        nJobStep = eStepRemote.Wait;
                        break;
                    case eStepRemote.Receive:
                        // Decoding Message is already excute
                        nJobStep = nJobStepBK = eStepRemote.Wait;
                        OnProcessCompleteEvent(this, new ProcessArgs(eStepRemote.Receive, m_nCurrentStatusReceive));
                        break;
                    case eStepRemote.Close:
                        m_pTCPServer.Close();
                        IsConnected = false;
                        nJobStepBK = nJobStep;
                        nJobStep = eStepRemote.Wait;
                        OnProcessCompleteEvent(this, new ProcessArgs(eStepRemote.Close));
                        break;
                    case eStepRemote.Release:
                        m_pTCPServer.Close();
                        IsConnected = false;
                        bRun = false;
                        break;
                }
                Thread.Sleep(2);
            }
        }

        private void ProcessClientControl()
        {
            bool bRun = true;

            eStepRemote nJobStep = eStepRemote.Wait;
            eStepRemote nJobStepBK = eStepRemote.Wait;

            while (bRun)
            {
                if (m_bJobPause)
                    continue;

                //// 어떠한 상황에서도 무조건 RELEASE 선언에 순응해야 함
                if (m_nJobReservation == eStepRemote.Release)
                {
                    nJobStep = m_nJobReservation;
                }

                switch (nJobStep)
                {
                    case eStepRemote.Wait:
                        //// 대기 상태에서 연결이 갑작스럽게 끊긴 경우 재연결을 시도함
                        if (nJobStepBK == eStepRemote.Wait && !m_pTCPClient.IsConnected)
                        {
                            nJobStep = eStepRemote.Connect;
                        }
                        //// 연결이 DISCONNECT 되었다면 다시 OPEN할때까지 기다려야 함
                        else if (nJobStepBK == eStepRemote.Disconnect && m_nJobReservation == eStepRemote.Connect)
                        {
                            nJobStep = m_nJobReservation;
                        }
                        //// 대기 상태에서 다른 임무를 예약받은 경우 해당 예약 업무를 시행함
                        else if (nJobStepBK == eStepRemote.Wait && m_nJobReservation != eStepRemote.Wait)
                        {
                            nJobStep = m_nJobReservation;
                        }
                        //// 전송확인이 완료되면 대기 상태로 전환
                        else if (nJobStepBK == eStepRemote.Send && m_pTCPClient.IsSend)
                        {
                            nJobStepBK = nJobStep;
                            OnProcessCompleteEvent(this, new ProcessArgs(eStepRemote.Send));
                        }
                        break;
                    case eStepRemote.Connect:
                        if (m_pTCPClient.Connect())
                        {
                            nJobStep = eStepRemote.Wait;
                            IsConnected = true;
                            OnProcessCompleteEvent(this, new ProcessArgs(eStepRemote.Connect, eYoonStatus.OK));
                        }
                        else
                        {
                            nJobStepBK = nJobStep;
                            m_pTCPClient.OnRetryThreadStart();
                            nJobStep = eStepRemote.Retry;
                        }
                        break;
                    case eStepRemote.Retry:
                        if (m_pTCPClient.IsConnected)
                        {
                            nJobStepBK = nJobStep;
                            nJobStep = eStepRemote.Wait;
                            IsConnected = true;
                            OnProcessCompleteEvent(this, new ProcessArgs(eStepRemote.Connect, eYoonStatus.OK));
                        }
                        break;
                    case eStepRemote.Send:
                        if (m_strSendMessage != string.Empty)
                            m_pTCPClient.Send(m_strSendMessage);
                        nJobStepBK = nJobStep;
                        nJobStep = eStepRemote.Wait;
                        break;
                    case eStepRemote.Receive:
                        // Decoding Message is already excute
                        nJobStep = nJobStepBK = eStepRemote.Wait;
                        OnProcessCompleteEvent(this, new ProcessArgs(eStepRemote.Receive, m_nCurrentStatusReceive));
                        break;
                    case eStepRemote.Disconnect:
                        m_pTCPClient.Disconnect();
                        IsConnected = false;
                        nJobStepBK = nJobStep;
                        nJobStep = eStepRemote.Wait;
                        OnProcessCompleteEvent(this, new ProcessArgs(eStepRemote.Disconnect));
                        break;
                    case eStepRemote.Release:
                        m_pTCPClient.Disconnect();
                        IsConnected = false;
                        bRun = false;
                        break;
                }
                Thread.Sleep(2);
            }
        }
        #endregion
    }
    public static class RemoteSupport
    {
        public static eYoonRobotRemote GetRemoteCommType(eYoonRobotType nTypeCobot, eYoonRobotRemoteHeadSend nHeaderData)
        {
            switch (nHeaderData)
            {
                case eYoonRobotRemoteHeadSend.StatusRun:
                case eYoonRobotRemoteHeadSend.StatusServo:
                case eYoonRobotRemoteHeadSend.StatusError:
                case eYoonRobotRemoteHeadSend.StatusSafety:
                case eYoonRobotRemoteHeadSend.Reset:
                case eYoonRobotRemoteHeadSend.Play:
                case eYoonRobotRemoteHeadSend.Pause:
                case eYoonRobotRemoteHeadSend.Stop:
                case eYoonRobotRemoteHeadSend.Quit:
                case eYoonRobotRemoteHeadSend.Shutdown:
                case eYoonRobotRemoteHeadSend.PowerOn:
                case eYoonRobotRemoteHeadSend.PowerOff:
                case eYoonRobotRemoteHeadSend.BreakRelease:
                    switch (nTypeCobot)
                    {
                        case eYoonRobotType.UR:
                            return eYoonRobotRemote.Dashboard;
                    }
                    break;
            }
            return eYoonRobotRemote.None;
        }

        public static eYoonStatus GetRemoteStatus(eYoonRobotRemoteHeadReceive nHeader)
        {
            if (nHeader.ToString().Contains("OK"))
                return eYoonStatus.OK;
            else if (nHeader.ToString().Contains("NG"))
                return eYoonStatus.NG;
            else
                return eYoonStatus.User;
        }

        public static eYoonRobotType GetRobotTypeFromString(string strLabelName)
        {
            if (strLabelName.Contains("UR"))
                return eYoonRobotType.UR;
            else if (strLabelName.Contains("TM"))
                return eYoonRobotType.TM;
            else
                return eYoonRobotType.None;
        }

        public static eYoonRobotRemote GetCommModeFromString(string strLabelName)
        {
            if (strLabelName.Contains("DashBoard"))
                return eYoonRobotRemote.Dashboard;
            else if (strLabelName.Contains("Script"))
                return eYoonRobotRemote.Script;
            else if (strLabelName.Contains("Socket"))
                return eYoonRobotRemote.Socket;
            else
                return eYoonRobotRemote.None;
        }

        public static string GetRemoteForcedMessage(eYoonRobotType nType, eYoonRobotRemoteHeadSend nHeader, ParamRobotSend pParamData)
        {
            string strMessage = string.Empty;

            switch (nType)
            {
                case eYoonRobotType.UR:
                    strMessage = URRemote.EncodingMessage(nHeader, pParamData);
                    break;
                default:
                    break;
            }
            return strMessage;
        }

        public static eYoonRobotRemoteHeadReceive GetRemoteReceiveHeader(eYoonRobotType nType, string strMessage, ref ParamRobotReceive pParamData)
        {
            eYoonRobotRemoteHeadReceive nHeader = eYoonRobotRemoteHeadReceive.None;

            switch (nType)
            {
                case eYoonRobotType.UR:
                    nHeader = URRemote.DecodingMessage(strMessage, ref pParamData);
                    break;
                default:
                    break;
            }
            return nHeader;
        }

        public static string GetRemoteLogFromReceiveHeader(eYoonRobotRemoteHeadReceive nHead)
        {
            switch (nHead)
            {
                case eYoonRobotRemoteHeadReceive.StatusServoOK:
                    return "Servo On";
                case eYoonRobotRemoteHeadReceive.StatusServoNG:
                    return "Servo Off";
                case eYoonRobotRemoteHeadReceive.StatusErrorOK:
                    return "Robot Error";
                case eYoonRobotRemoteHeadReceive.StatusErrorNG:
                    return "Robot Error Clear";
                case eYoonRobotRemoteHeadReceive.StatusRunOK:
                    return "Robot Running";
                case eYoonRobotRemoteHeadReceive.StatusRunNG:
                    return "Robot Running Failure";
                case eYoonRobotRemoteHeadReceive.StatusSafetyOK:
                    return "Workspace Safety";
                case eYoonRobotRemoteHeadReceive.StatusSafetyNG:
                    return "Workspace Warning";
                case eYoonRobotRemoteHeadReceive.LoadOK:
                    return "Load Success";
                case eYoonRobotRemoteHeadReceive.LoadNG:
                    return "Load Failure";
                case eYoonRobotRemoteHeadReceive.ResetOK:
                    return "Reset Completed";
                case eYoonRobotRemoteHeadReceive.ResetNG:
                    return "Reset Failure";
                case eYoonRobotRemoteHeadReceive.PlayOK:
                    return "Play";
                case eYoonRobotRemoteHeadReceive.PlayNG:
                    return "Play Error";
                case eYoonRobotRemoteHeadReceive.PauseOK:
                    return "Pause";
                case eYoonRobotRemoteHeadReceive.PauseNG:
                    return "Pause Error";
                case eYoonRobotRemoteHeadReceive.StopOk:
                    return "Stop";
                case eYoonRobotRemoteHeadReceive.StopNG:
                    return "Stop Error";
                case eYoonRobotRemoteHeadReceive.QuitOK:
                    return "Quit";
                case eYoonRobotRemoteHeadReceive.ShuwdownOK:
                    return "Quit Error";
                case eYoonRobotRemoteHeadReceive.PowerOnOK:
                    return "Robot Power On";
                case eYoonRobotRemoteHeadReceive.PowerOffOK:
                    return "Robot Power Off";
                case eYoonRobotRemoteHeadReceive.BreakReleaseOK:
                    return "Break Release";
                default:
                    return string.Empty;
            }
        }
    }
}
