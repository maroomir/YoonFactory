using System;
using System.Text;

namespace YoonFactory.Comm
{
    public interface IYoonTcpIp : IDisposable
    {
        event ShowMessageCallback OnShowMessageEvent;
        event RecieveDataCallback OnShowReceiveDataEvent;

        string RootDirectory { get; set; }
        string Address { get; set; }
        string Port { get; set; }
        bool IsRetryOpen { get; }
        bool IsSend { get; }
        bool IsConnected { get; }
        StringBuilder sbReceiveMessage { get; }

        void CopyFrom(IYoonTcpIp pTcpIp);
        IYoonTcpIp Clone();
        void LoadParam();
        void SaveParam();

        bool Send(string strBuffer);
        bool Send(byte[] pBuffer);
        bool Open();
        void Close();
    }
}
