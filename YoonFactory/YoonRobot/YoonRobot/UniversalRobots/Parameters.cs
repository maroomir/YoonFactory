using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory.Robot.UniversialRobot
{
    public class ParameterConnect : IYoonParameter
    {
        public string IPAddress { get; set; } = "192.168.101.101";
        public string DashboardControlPort { get; set; } = "29999";
        public string ScriptControlPort { get; set; } = "30001";
        public string SocketPort { get; set; } = "50000";
        public eYoonCommType DashboardMode { get; set; } = eYoonCommType.TCPClient;
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
            DashboardControlPort = pArgs[1];
            ScriptControlPort = pArgs[2];
            SocketPort = pArgs[3];
            DashboardMode = (eYoonCommType)Enum.Parse(typeof(eYoonCommType), pArgs[4]);
            ScriptMode = (eYoonCommType)Enum.Parse(typeof(eYoonCommType), pArgs[5]);
            SocketMode = (eYoonCommType)Enum.Parse(typeof(eYoonCommType), pArgs[6]);
        }

        public IYoonParameter Clone()
        {
            ParameterConnect pParam = new ParameterConnect();
            pParam.IPAddress = IPAddress;
            pParam.DashboardControlPort = DashboardControlPort;
            pParam.ScriptControlPort = ScriptControlPort;
            pParam.SocketPort = SocketPort;
            pParam.DashboardMode = DashboardMode;
            pParam.ScriptMode = ScriptMode;
            pParam.SocketMode = SocketMode;
            return pParam;
        }

        public void CopyFrom(IYoonParameter pParam)
        {
            if (pParam is ParameterConnect pParamConnect)
            {
                IPAddress = pParamConnect.IPAddress;
                DashboardControlPort = pParamConnect.DashboardControlPort;
                ScriptControlPort = pParamConnect.ScriptControlPort;
                SocketPort = pParamConnect.SocketPort;
                DashboardMode = pParamConnect.DashboardMode;
                ScriptMode = pParamConnect.ScriptMode;
                SocketMode = pParamConnect.SocketMode;
            }
        }

        public bool Equals(IYoonParameter pParam)
        {
            if (pParam is ParameterConnect pParamConnect)
            {
                return IPAddress == pParamConnect.IPAddress &&
                    DashboardControlPort == pParamConnect.DashboardControlPort &&
                    ScriptControlPort == pParamConnect.ScriptControlPort &&
                    SocketPort == pParamConnect.SocketPort &&
                    DashboardMode == pParamConnect.DashboardMode &&
                    ScriptMode == pParamConnect.ScriptMode &&
                    SocketMode == pParamConnect.SocketMode;
            }
            return false;
        }
    }

    public class ParameterPacket : IYoonParameter
    {
        public string ProjectName { get; set; } = "";
        public string ProgramName { get; set; } = "";
        public double Joint1 { get; set; } = 0.0;
        public double Joint2 { get; set; } = 0.0;
        public double Joint3 { get; set; } = 0.0;
        public double Joint4 { get; set; } = 0.0;
        public double Joint5 { get; set; } = 0.0;
        public double Joint6 { get; set; } = 0.0;
        public double CartX { get; set; } = 0.0;
        public double CartY { get; set; } = 0.0;
        public double CartZ { get; set; } = 0.0;
        public double CartRX { get; set; } = 0.0;
        public double CartRY { get; set; } = 0.0;
        public double CartRZ { get; set; } = 0.0;
        public int VelocityPercent { get; set; } = 0;

        public int GetLength()
        {
            return typeof(ParameterPacket).GetProperties().Length;
        }

        public void Set(params string[] pArgs)
        {
            if (pArgs.Length != GetLength())
                return;
            ProjectName = pArgs[0];
            ProgramName = pArgs[1];
            Joint1 = Double.Parse(pArgs[2]);
            Joint2 = Double.Parse(pArgs[3]);
            Joint3 = Double.Parse(pArgs[4]);
            Joint4 = Double.Parse(pArgs[5]);
            Joint5 = Double.Parse(pArgs[6]);
            Joint6 = Double.Parse(pArgs[7]);
            CartX = Double.Parse(pArgs[8]);
            CartY = Double.Parse(pArgs[9]);
            CartZ = Double.Parse(pArgs[10]);
            CartRX = Double.Parse(pArgs[11]);
            CartRY = Double.Parse(pArgs[12]);
            CartRZ = Double.Parse(pArgs[13]);
            VelocityPercent = Int32.Parse(pArgs[14]);
        }

        public IYoonParameter Clone()
        {
            ParameterPacket pParam = new ParameterPacket();
            pParam.ProjectName = ProjectName;
            pParam.ProgramName = ProgramName;
            pParam.Joint1 = Joint1;
            pParam.Joint2 = Joint2;
            pParam.Joint3 = Joint3;
            pParam.Joint4 = Joint4;
            pParam.Joint5 = Joint5;
            pParam.Joint6 = Joint6;
            pParam.CartX = CartX;
            pParam.CartY = CartY;
            pParam.CartZ = CartZ;
            pParam.CartRX = CartRX;
            pParam.CartRY = CartRY;
            pParam.CartRZ = CartRZ;
            pParam.VelocityPercent = VelocityPercent;
            return pParam;
        }

        public void CopyFrom(IYoonParameter pParam)
        {
            if(pParam is ParameterPacket pParamPacket)
            {
                pParamPacket.ProjectName = ProjectName;
                pParamPacket.ProgramName = ProgramName;
                pParamPacket.Joint1 = Joint1;
                pParamPacket.Joint2 = Joint2;
                pParamPacket.Joint3 = Joint3;
                pParamPacket.Joint4 = Joint4;
                pParamPacket.Joint5 = Joint5;
                pParamPacket.Joint6 = Joint6;
                pParamPacket.CartX = CartX;
                pParamPacket.CartY = CartY;
                pParamPacket.CartZ = CartZ;
                pParamPacket.CartRX = CartRX;
                pParamPacket.CartRY = CartRY;
                pParamPacket.CartRZ = CartRZ;
            }
        }

        public bool Equals(IYoonParameter pParam)
        {
            if (pParam is ParameterPacket pParamPacket)
            {
                return pParamPacket.ProjectName == ProjectName &&
                pParamPacket.ProgramName == ProgramName &&
                pParamPacket.Joint1 == Joint1 &&
                pParamPacket.Joint2 == Joint2 &&
                pParamPacket.Joint3 == Joint3 &&
                pParamPacket.Joint4 == Joint4 &&
                pParamPacket.Joint5 == Joint5 &&
                pParamPacket.Joint6 == Joint6 &&
                pParamPacket.CartX == CartX &&
                pParamPacket.CartY == CartY &&
                pParamPacket.CartZ == CartZ &&
                pParamPacket.CartRX == CartRX &&
                pParamPacket.CartRY == CartRY &&
                pParamPacket.CartRZ == CartRZ;
            }
            return false;
        }
    }
}
