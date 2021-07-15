using System;
using System.Text;

namespace YoonFactory.Comm
{
    public interface IYoonComm : IDisposable
    {
        string Port { get; set; }
        
        void CopyFrom(IYoonComm pComm);
        IYoonComm Clone();
        
        bool Send(string strBuffer);
        bool Send(byte[] pBuffer);
        bool Open();
        void Close();
    }

    public interface IYoonTcpIp : IYoonComm
    {
        event ShowMessageCallback OnShowMessageEvent;
        event RecieveDataCallback OnShowReceiveDataEvent;

        string RootDirectory { get; set; }
        string Address { get; set; }
        bool IsRetryOpen { get; }
        bool IsSend { get; }
        bool IsConnected { get; }
        StringBuilder sbReceiveMessage { get; }

        void LoadParam();
        void SaveParam();
    }
}
