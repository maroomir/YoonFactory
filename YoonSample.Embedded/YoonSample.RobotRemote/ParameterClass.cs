using System;
using YoonFactory;
namespace YoonSample.RobotRemote
{
    public class ParameterClass : IYoonParameter
    {
        public string OnMessage { get; set; } = "1";
        public string OffMessage { get; set; } = "2";

        public IYoonParameter Clone()
        {
            ParameterClass pParam = new ParameterClass();
            pParam.OnMessage = OnMessage;
            pParam.OffMessage = OffMessage;
            return pParam;
        }

        public void CopyFrom(IYoonParameter pParam)
        {
            if(pParam is ParameterClass pParamPacket)
            {
                pParamPacket.OnMessage = OnMessage;
                pParamPacket.OffMessage = OffMessage;
            }
        }

        public bool Equals(IYoonParameter pParam)
        {
            if(pParam is ParameterClass pParamPacket)
            {
                return pParamPacket.OnMessage == OnMessage &&
                    pParamPacket.OffMessage == OffMessage;
            }
            return false;
        }

        public int GetLength()
        {
            return typeof(ParameterClass).GetProperties().Length;
        }

        public void Set(params string[] pArgs)
        {
            if (pArgs.Length != GetLength()) return;
            OnMessage = pArgs[0];
            OffMessage = pArgs[1];
        }
    }
}
