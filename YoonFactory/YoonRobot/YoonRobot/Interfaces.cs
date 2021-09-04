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
        

        void CopyFrom(IYoonRemote pRemote);
        IYoonRemote Clone();

        bool Open();
        bool Close();
    }
}
