using System;
using YoonFactory;
using YoonFactory.Comm.TCP;
namespace YoonSample.Mono
{
    public class ParameterConnection : IYoonParameter
    {
        public eCommType Type { get; set; } = eCommType.TCPClient;
        public string IPAddress { get; set; } = "192.168.71.1";
        public int Port { get; set; } = 5000;

        public int GetLength()
        {
            return typeof(ParameterConnection).GetProperties().Length;
        }

        public void Set(params string[] pArgs)
        {
            if (pArgs.Length != GetLength()) return;
            Type = (eCommType)Enum.Parse(typeof(eCommType), pArgs[0]);
            IPAddress = pArgs[1];
            Port = Int32.Parse(pArgs[2]);
        }

        public bool Equals(IYoonParameter pComparison)
        {
            if(pComparison is ParameterConnection pParamConnection)
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
            if(pObject is ParameterConnection pParamConnection)
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

    public class ParameterAction : IYoonParameter
    {
        public string[] ActionNames { get; set; } = new string[CommonClass.MAX_ACTION_NUM]
        { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"};
        public string[] ActionValues { get; set; } = new string[CommonClass.MAX_ACTION_NUM]
        { "Value", "Value", "Value", "Value", "Value", "Value", "Value", "Value", "Value", "Value", "Value", "Value"};

        public int GetLength()
        {
            return ActionNames.Length + ActionValues.Length;
        }

        public void Set(params string[] pArgs)
        {
            if (pArgs.Length != GetLength()) return;
            Array.Copy(pArgs, 0, ActionNames, 0, ActionNames.Length);
            Array.Copy(pArgs, ActionNames.Length, ActionValues, 0, ActionValues.Length);
        }

        public bool Equals(IYoonParameter pComparision)
        {
            if(pComparision is ParameterAction pParamAction)
            {
                for (int iCount = 0; iCount < CommonClass.MAX_ACTION_NUM; iCount++)
                {
                    if (ActionNames[iCount] != pParamAction.ActionNames[iCount] ||
                       ActionValues[iCount] != pParamAction.ActionValues[iCount])
                        return false;
                }
                return true;
            }
            return false;
        }

        public void CopyFrom(IYoonParameter pObject)
        {
            if(pObject is ParameterAction pParamAction)
            {
                for (int iCount = 0; iCount < CommonClass.MAX_ACTION_NUM; iCount++)
                {
                    ActionNames[iCount] = pParamAction.ActionNames[iCount];
                    ActionValues[iCount] = pParamAction.ActionValues[iCount];
                }
            }
        }

        public IYoonParameter Clone()
        {
            ParameterAction pParam = new ParameterAction();
            for (int iCount = 0; iCount < CommonClass.MAX_ACTION_NUM; iCount++)
            {
                pParam.ActionNames[iCount] = ActionNames[iCount];
                pParam.ActionValues[iCount] = ActionValues[iCount];
            }
            return pParam;
        }
    }
}
