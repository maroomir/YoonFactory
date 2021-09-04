using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory.Robot.TechMan
{
    public class ParameterConnect : IYoonParameter
    {
        public string IPAddress = "192.168.101.102";
        public string ScriptControlPort = "5890";
        public string SocketPort = "30000";
        public eYoonCommType ScriptMode = eYoonCommType.TCPClient;
        public eYoonCommType SocketMode = eYoonCommType.TCPClient;

        public IYoonParameter Clone()
        {
            ParameterConnect pParam = new ParameterConnect();
            pParam.IPAddress = IPAddress;
            pParam.ScriptControlPort = ScriptControlPort;
            pParam.SocketPort = SocketPort;
            pParam.ScriptMode = ScriptMode;
            pParam.SocketMode = SocketMode;
            return pParam;
        }

        public void CopyFrom(IYoonParameter pParam)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IYoonParameter pParam)
        {
            throw new NotImplementedException();
        }
    }

    public class ParameterPacket : IYoonParameter
    {
        public double Joint1;
        public double Joint2;
        public double Joint3;
        public double Joint4;
        public double Joint5;
        public double Joint6;
        public double CartX;
        public double CartY;
        public double CartZ;
        public double CartRX;
        public double CartRY;
        public double CartRZ;

        public IYoonParameter Clone()
        {
            throw new NotImplementedException();
        }

        public void CopyFrom(IYoonParameter pParam)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IYoonParameter pParam)
        {
            throw new NotImplementedException();
        }
    }
}
