using System;
using System.Text;
using YoonFactory.Comm;

namespace YoonFactory.Robot
{
    public interface IYoonRemote : IDisposable
    {
        event ShowMessageCallback OnShowMessageEvent;
        event RecieveDataCallback OnShowReceiveDataEvent;

        string RootDirectory { get; }
        IYoonParameter ConnectionParameter { get; }
        IYoonParameter PacketParameter { get; }

        void CopyFrom(IYoonRemote pRemote);
        IYoonRemote Clone();
        void CloseAll();
        bool Open(eYoonRemoteType nType);
        void Close(eYoonRemoteType nType);

        bool StartRobot();
        bool StopRobot();
        bool ResetRobot();
        bool SendSocket(string strMessage);

        void LoadParameter();
        void SaveParameter();
    }
}
