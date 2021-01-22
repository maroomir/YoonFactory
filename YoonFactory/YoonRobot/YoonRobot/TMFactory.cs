using YoonFactory.Comm;

namespace YoonFactory.Robot
{
    public static class TechMan
    {
        public static ParameterRemote InitRemoteParameter(eYoonRemoteType nType)
        {
            ParameterRemote pParam = new ParameterRemote();
            switch (nType)
            {
                case eYoonRemoteType.Script:
                    pParam.IPAddress = "192.168.101.102";
                    pParam.Port = "5890";
                    pParam.TCPMode = eYoonCommType.TCPClient;
                    break;
                case eYoonRemoteType.Socket:
                    pParam.IPAddress = "192.168.101.102";
                    pParam.Port = "30000";
                    pParam.TCPMode = eYoonCommType.TCPServer;
                    break;
                default:
                    break;
            }
            return pParam;
        }
    }
}
