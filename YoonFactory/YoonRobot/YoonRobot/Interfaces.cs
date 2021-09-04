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

        bool OpenConnect();
        bool CloseConnect();
        bool StartRobot();
        bool StopRobot();
        bool ResetRobot();
        bool SendSocket(string strMessage);
        bool SendSocket(string[] pMessage);

        void LoadParameter();
        void SaveParameter();
    }
}
