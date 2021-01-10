using System;
using System.Collections.Generic;
using System.IO;
using YoonFactory.Comm;
using YoonFactory.Files;

namespace YoonFactory.Robot.Remote
{

    public enum eYoonRobotType : int
    {
        None = -1,
        UR,
        TM,
    }

    public enum eYoonRobotRemote : int
    {
        None = -1,
        Socket,
        Script,
        Dashboard,
    }

    public enum eYoonRobotRemoteHeadSend : int   // REMOTE MODE = UR : DASHBOARD
    {
        None = -1,
        StatusServo = 0,
        StatusError,
        StatusRun,
        StatusSafety,
        LoadProject,
        LoadJob,
        Reset,
        Play,
        Pause,
        Stop,
        Quit,
        Shutdown,
        PowerOn,
        PowerOff,
        BreakRelease,
    }

    public enum eYoonRobotRemoteHeadReceive : int    // REMOTE MODE = UR : DASHBOARD
    {
        None = -1,
        StatusServoOK = 0,
        StatusServoNG,
        StatusErrorOK,
        StatusErrorNG,
        StatusRunOK,
        StatusRunNG,
        StatusSafetyOK,
        StatusSafetyNG,
        LoadOK,
        LoadNG,
        ResetOK,
        ResetNG,
        PlayOK,
        PlayNG,
        PauseOK,
        PauseNG,
        StopOk,
        StopNG,
        QuitOK,
        ShuwdownOK,
        PowerOnOK,
        PowerOffOK,
        BreakReleaseOK,
    }

    public struct ParamRobotSend
    {
        public string ProjectName;  //
        public string ProgramName;  // UR

        public YoonJointD JointPos;  // UR, TM (J1 ~ J6)
        public YoonCartD CartPos;    // UR, TM (X, Y, Z, RX, R

        public int VelocityPercent;

        public string[] ArraySocketParam;   // UR, TM
    }

    public struct ParamRobotReceive
    {
        public YoonJointD JointPos;   // UR, TM
        public YoonCartD CartPos;     // UR, TM
    }

    public class RemoteParameter : IYoonParameter
    {
        public string IPAddress { get; set; } = "127.0.0.1";
        public string Port { get; set; } = "30000";
        public eYoonCommType TCPMode { get; set; } = eYoonCommType.TCPServer;

        public bool IsEqual(IYoonParameter pParam)
        {
            if(pParam is RemoteParameter pParamRemote)
            {
                if (pParamRemote.IPAddress == IPAddress && pParamRemote.Port == Port && pParamRemote.TCPMode == TCPMode)
                    return true;
            }
            return false;
        }

        public IYoonParameter Clone()
        {
            RemoteParameter pParam = new RemoteParameter();
            pParam.IPAddress = IPAddress;
            pParam.Port = Port;
            pParam.TCPMode = TCPMode;
            return pParam;
        }

        public void CopyFrom(IYoonParameter pParam)
        {
            if (pParam is RemoteParameter pParamRemote)
            {
                IPAddress = pParamRemote.IPAddress;
                Port = pParamRemote.Port;
                TCPMode = pParamRemote.TCPMode;
            }
        }
    }

    public class RemoteContainer : IYoonContainer, IYoonContainer<RemoteParameter>
    {
        #region IDisposable Support
        ~RemoteContainer()
        {
            this.Dispose(false);
        }

        private bool disposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;
            if (disposing)
            {
                ////  .Net Framework에 의해 관리되는 리소스를 여기서 정리합니다.
                if (ObjectDictionary != null)
                    ObjectDictionary.Clear();
                ObjectDictionary = null;

            }
            //// .NET Framework에 의하여 관리되지 않는 외부 리소스들을 여기서 정리합니다.
            this.disposed = true;
        }
        #endregion

        public Dictionary<string, RemoteParameter> ObjectDictionary { get; private set; } = new Dictionary<string, RemoteParameter>();
        public string RootDirectory { get; set; }
        public RemoteParameter this[string strKey]
        {
            get => GetValue(strKey);
            set => SetValue(strKey, value);
        }

        public RemoteContainer()
        {
            SetDefault();
        }

        public void CopyFrom(IYoonContainer pContainer)
        {
            if (pContainer is RemoteContainer pRemoteContainer)
            {
                ObjectDictionary.Clear();

                RootDirectory = pRemoteContainer.RootDirectory;
                ObjectDictionary = new Dictionary<string, RemoteParameter>(pRemoteContainer.ObjectDictionary);
            }
        }

        public IYoonContainer Clone()
        {
            RemoteContainer pContainer = new RemoteContainer();
            pContainer.RootDirectory = RootDirectory;
            pContainer.ObjectDictionary = new Dictionary<string, RemoteParameter>(ObjectDictionary);
            return pContainer;
        }

        public bool Add(string strKey, RemoteParameter pRemote)
        {
            try
            {
                ObjectDictionary.Add(strKey, pRemote);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Add(eYoonRobotType nRobot, eYoonRobotRemote nRemote, RemoteParameter pRemote)
        {
            string strKey = ConvertKey(nRobot, nRemote);
            try
            {
                ObjectDictionary.Add(strKey, pRemote);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Remove(string strKey)
        {
            return ObjectDictionary.Remove(strKey);
        }

        public bool Remove(eYoonRobotType nRobot, eYoonRobotRemote nRemote)
        {
            string strKey = ConvertKey(nRobot, nRemote);
            return ObjectDictionary.Remove(strKey);
        }

        public void Clear()
        {
            ObjectDictionary.Clear();
        }

        public string GetKey(RemoteParameter pRemote)
        {
            foreach (string strKey in ObjectDictionary.Keys)
            {
                if (ObjectDictionary[strKey] == pRemote)
                    return strKey;
            }
            return string.Empty;
        }

        public RemoteParameter GetValue(string strKey)
        {
            if (ObjectDictionary.ContainsKey(strKey))
                return ObjectDictionary[strKey];
            else
                return new RemoteParameter();
        }

        public RemoteParameter GetValue(eYoonRobotType nRobot, eYoonRobotRemote nRemote = eYoonRobotRemote.Socket)
        {
            string strKey = nRobot.ToString() + "_" + nRemote.ToString();
            if (ObjectDictionary.ContainsKey(strKey))
                return ObjectDictionary[strKey];
            else
                return new RemoteParameter();
        }

        public void SetValue(string strKey, RemoteParameter pRemote)
        {
            if (ObjectDictionary.ContainsKey(strKey))
                ObjectDictionary[strKey] = pRemote;
            else
                Add(strKey, pRemote);
        }

        public void SetValue(eYoonRobotType nRobot, eYoonRobotRemote nRemote, RemoteParameter pRemote)
        {
            string strKey = ConvertKey(nRobot, nRemote);
            if (ObjectDictionary.ContainsKey(strKey))
                ObjectDictionary[strKey] = pRemote;
            else
                Add(nRobot, nRemote, pRemote);
        }

        public bool LoadValue(string strKey)
        {
            if (RootDirectory == string.Empty) return false;

            eYoonRobotType nRobot = eYoonRobotType.None;
            eYoonRobotRemote nRemote = eYoonRobotRemote.None;
            ConvertKey(strKey, ref nRobot, ref nRemote);
            string strFilePath = GetRemoteIniFilePath(nRobot, nRemote);
            RemoteParameter pRemote = LoadRemoteFileFromIni(strFilePath, strKey);
            if (pRemote == null) return false;

            SetValue(strKey, pRemote);
            return true;
        }

        public bool LoadValue(eYoonRobotType nRobot, eYoonRobotRemote nRemote)
        {
            if (RootDirectory == string.Empty) return false;

            string strKey = ConvertKey(nRobot, nRemote);
            string strFilePath = GetRemoteIniFilePath(nRobot, nRemote);
            RemoteParameter pRemote = LoadRemoteFileFromIni(strFilePath, strKey);
            if (pRemote == null) return false;

            SetValue(strKey, pRemote);
            return true;
        }

        public bool SaveValue(string strKey)
        {
            if (RootDirectory == string.Empty) return false;

            eYoonRobotType nRobot = eYoonRobotType.None;
            eYoonRobotRemote nRemote = eYoonRobotRemote.None;
            ConvertKey(strKey, ref nRobot, ref nRemote);
            string strFilePath = GetRemoteIniFilePath(nRobot, nRemote);
            RemoteParameter pRemote = GetValue(strKey);
            return SaveRemoteFileToIni(strFilePath, strKey, pRemote);
        }

        public bool SaveValue(eYoonRobotType nRobot, eYoonRobotRemote nRemote)
        {
            if (RootDirectory == string.Empty) return false;

            string strKey = ConvertKey(nRobot, nRemote);
            string strFilePath = GetRemoteIniFilePath(nRobot, nRemote);
            RemoteParameter pRemote = GetValue(strKey);
            return SaveRemoteFileToIni(strFilePath, strKey, pRemote);
        }

        public void ConvertKey(string strKey, ref eYoonRobotType nRobot, ref eYoonRobotRemote nRemote)
        {
            string[] strKeys = strKey.Split('_');
            if (strKeys.Length >= 2)
            {
                nRobot = (eYoonRobotType)Enum.Parse(typeof(eYoonRobotType), strKeys[0]);
                nRemote = (eYoonRobotRemote)Enum.Parse(typeof(eYoonRobotRemote), strKeys[1]);
            }
        }

        public string ConvertKey(eYoonRobotType nRobot, eYoonRobotRemote nRemote)
        {
            return nRobot.ToString() + "_" + nRemote.ToString();
        }

        private string GetRemoteIniFilePath(eYoonRobotType nRobot, eYoonRobotRemote nRemote)
        {
            return Path.Combine(RootDirectory, string.Format(@"Remote{0}{1}.ini", nRobot.ToString(), nRemote.ToString()));
        }

        private RemoteParameter LoadRemoteFileFromIni(string strFilePath, string strKey)
        {
            if (RootDirectory == string.Empty) return null;

            RemoteParameter pRemote = new RemoteParameter();
            YoonIni ic = new YoonIni(strFilePath);
            ic.LoadFile();
            pRemote.IPAddress = ic[strKey]["IP"].ToString("127.0.0.1");
            pRemote.Port = ic[strKey]["Port"].ToString("1234");
            pRemote.TCPMode = ic[strKey]["Mode"].ToEnum(eYoonCommType.TCPServer);
            return pRemote;
        }

        private bool SaveRemoteFileToIni(string strFilePath, string strKey, RemoteParameter pRemote)
        {
            if (RootDirectory == string.Empty) return false;

            YoonIni ic = new YoonIni(strFilePath);
            ic[strKey]["IP"] = pRemote.IPAddress;
            ic[strKey]["Port"] = pRemote.Port;
            ic[strKey]["Mode"] = pRemote.TCPMode.ToString();
            ic.SaveFile();
            return true;
        }

        public bool LoadAll()
        {
            if (ObjectDictionary == null)
                ObjectDictionary = new Dictionary<string, RemoteParameter>();
            ObjectDictionary.Clear();

            bool bResult = true;
            for (int iRobot = 0; iRobot < Enum.GetValues(typeof(eYoonRobotType)).Length - 1; iRobot++)
            {
                eYoonRobotType nRobot = (eYoonRobotType)iRobot; // LG, UR, TM
                if (nRobot == eYoonRobotType.None) continue;

                for (int iComm = 0; iComm < Enum.GetValues(typeof(eYoonRobotRemote)).Length - 1; iComm++)
                {
                    eYoonRobotRemote nComm = (eYoonRobotRemote)iComm; // Socket, Script, Dashboard
                    if (nComm == eYoonRobotRemote.None) continue;
                    if (!LoadValue(nRobot, nComm))
                        bResult = false;
                }
            }
            return bResult;
        }

        public bool SaveAll()
        {
            if (ObjectDictionary == null)
                return false;

            bool bResult = true;
            foreach (string strKey in ObjectDictionary.Keys)
            {
                if (!SaveValue(strKey))
                    bResult = false;
            }
            return bResult;
        }

        public void SetDefault()
        {
            if (ObjectDictionary == null)
                ObjectDictionary = new Dictionary<string, RemoteParameter>();
            ObjectDictionary.Clear();

            for (int iRobot = 0; iRobot < Enum.GetValues(typeof(eYoonRobotType)).Length - 1; iRobot++)
            {
                eYoonRobotType nRobot = (eYoonRobotType)iRobot; // LG, UR, TM
                if (nRobot == eYoonRobotType.None) continue;

                for (int iComm = 0; iComm < Enum.GetValues(typeof(eYoonRobotRemote)).Length - 1; iComm++)
                {
                    eYoonRobotRemote nComm = (eYoonRobotRemote)iComm; // Socket, Script, Dashboard
                    if (nComm == eYoonRobotRemote.None) continue;

                    RemoteParameter pRemote = new RemoteParameter();
                    switch (ConvertKey(nRobot, nComm))
                    {
                        case "LG_Socket":
                            pRemote.IPAddress = "192.168.101.100";
                            pRemote.Port = "30000";
                            pRemote.TCPMode = eYoonCommType.TCPServer;
                            break;
                        case "UR_Dashboard":
                            pRemote.IPAddress = "192.168.101.101";
                            pRemote.Port = "29999";
                            pRemote.TCPMode = eYoonCommType.TCPClient;
                            break;
                        case "UR_Script":
                            pRemote.IPAddress = "192.168.101.101";
                            pRemote.Port = "30001";
                            pRemote.TCPMode = eYoonCommType.TCPClient;
                            break;
                        case "UR_Socket":
                            pRemote.IPAddress = "192.168.101.101";
                            pRemote.Port = "50000";
                            pRemote.TCPMode = eYoonCommType.TCPClient;
                            break;
                        case "TM_Script":
                            pRemote.IPAddress = "192.168.101.102";
                            pRemote.Port = "5890";
                            pRemote.TCPMode = eYoonCommType.TCPClient;
                            break;
                        case "TM_Socket":
                            pRemote.IPAddress = "192.168.101.102";
                            pRemote.Port = "30000";
                            pRemote.TCPMode = eYoonCommType.TCPServer;
                            break;
                        default:
                            pRemote.Port = "11111";
                            pRemote.TCPMode = eYoonCommType.TCPServer;
                            break;
                    }

                    Add(nRobot, nComm, pRemote);
                }
            }
        }
    }

    public delegate void RemoteLogCallback(object sender, RemoteLogArgs e);
    public class RemoteLogArgs : EventArgs
    {
        public DateTime Time;
        public eYoonStatus Status;
        public string LogMessage;

        public RemoteLogArgs(eYoonStatus nStatus, string strMessage)
        {
            Time = DateTime.Now;
            Status = nStatus;
            switch (Status)
            {
                case eYoonStatus.OK:
                    this.LogMessage = "[OK] : " + strMessage;
                    break;
                case eYoonStatus.NG:
                    this.LogMessage = "[NG] : " + strMessage;
                    break;
                case eYoonStatus.Error:
                    this.LogMessage = "[ERR] : " + strMessage;
                    break;
                default:
                    this.LogMessage = "RemoteFactory : " + strMessage;
                    break;
            }
        }
    }

    public delegate void RemoteResultCallback(object sender, RemoteResultArgs e);
    public class RemoteResultArgs : EventArgs
    {
        public eYoonStatus Status;
        public string Message;
        public eYoonRobotRemoteHeadReceive ReceiveHead;
        public ParamRobotReceive ReceiveData;

        public RemoteResultArgs(eYoonStatus nStatus, string strMessage)
        {
            Status = nStatus;
            ReceiveHead = eYoonRobotRemoteHeadReceive.None;
            ReceiveData = new ParamRobotReceive();
            Message = strMessage;
        }
        public RemoteResultArgs(eYoonStatus nStatus, eYoonRobotRemoteHeadReceive nHead, string strMessage)
        {
            Status = nStatus;
            ReceiveHead = eYoonRobotRemoteHeadReceive.None;
            ReceiveData = new ParamRobotReceive();
            Message = strMessage;
        }

        public RemoteResultArgs(eYoonRobotRemoteHeadReceive nHead, ParamRobotReceive pDataReceive, string strMessage)
        {
            Status = eYoonStatus.OK;
            ReceiveHead = nHead;
            Message = strMessage;
            ReceiveData = new ParamRobotReceive();
            {
                if (pDataReceive.JointPos != null)
                    ReceiveData.JointPos = pDataReceive.JointPos.Clone() as YoonJointD;
                if (pDataReceive.CartPos != null)
                    ReceiveData.CartPos = pDataReceive.CartPos.Clone() as YoonCartD;
            }
        }

    }

}
