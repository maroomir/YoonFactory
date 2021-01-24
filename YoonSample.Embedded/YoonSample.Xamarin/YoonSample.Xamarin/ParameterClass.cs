using YoonFactory;
using YoonFactory.Comm;

namespace YoonSample.Xamarin
{
    public class ParameterConnection : IYoonParameter
    {
        public eYoonCommType Type { get; set; } = eYoonCommType.TCPClient;
        public string IPAddress { get; set; } = "192.168.71.1";
        public int Port { get; set; } = 5000;

        public bool Equals(IYoonParameter pComparison)
        {
            if (pComparison is ParameterConnection pParamConnection)
            {
                if (IPAddress == pParamConnection.IPAddress &&
                    Type == pParamConnection.Type &&
                    Port == pParamConnection.Port)
                    return true;
            }
            return false;
        }

        public void CopyFrom(IYoonParameter pObject)
        {
            if (pObject is ParameterConnection pParamConnection)
            {
                IPAddress = pParamConnection.IPAddress;
                Type = pParamConnection.Type;
                Port = pParamConnection.Port;
            }
        }

        public IYoonParameter Clone()
        {
            ParameterConnection pParam = new ParameterConnection();
            pParam.IPAddress = IPAddress;
            pParam.Type = Type;
            pParam.Port = Port;
            return pParam;
        }
    }
}
