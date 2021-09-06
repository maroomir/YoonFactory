using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory.Robot.TechMan
{
    public class ParameterConnect : IYoonParameter
    {
        public string IPAddress { get; set; } = "192.168.101.102";
        public string ScriptControlPort { get; set; } = "5890";
        public string SocketPort { get; set; } = "30000";
        public eYoonCommType ScriptMode { get; set; } = eYoonCommType.TCPClient;
        public eYoonCommType SocketMode { get; set; } = eYoonCommType.TCPClient;

        public int GetLength()
        {
            return typeof(ParameterConnect).GetProperties().Length;
        }

        public void Set(params string[] pArgs)
        {
            if (pArgs.Length != GetLength())
                return;
            IPAddress = pArgs[0];
            ScriptControlPort = pArgs[1];
            SocketPort = pArgs[2];
            ScriptMode = (eYoonCommType)Enum.Parse(typeof(eYoonCommType), pArgs[3]);
            SocketMode = (eYoonCommType)Enum.Parse(typeof(eYoonCommType), pArgs[4]);
        }

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
        public double Joint1 { get; set; }
        public double Joint2 { get; set; }
        public double Joint3 { get; set; }
        public double Joint4 { get; set; }
        public double Joint5 { get; set; }
        public double Joint6 { get; set; }
        public double CartX { get; set; }
        public double CartY { get; set; }
        public double CartZ { get; set; }
        public double CartRX { get; set; }
        public double CartRY { get; set; }
        public double CartRZ { get; set; }

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

        public int GetLength()
        {
            throw new NotImplementedException();
        }

        public void Set(params string[] pArgs)
        {
            throw new NotImplementedException();
        }
    }
}
