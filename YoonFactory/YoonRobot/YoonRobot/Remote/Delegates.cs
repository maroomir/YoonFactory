using System;

namespace YoonFactory.Robot.Remote
{
    public struct SendValue
    {
        public string ProjectName;  //
        public string ProgramName;  // UR

        public YoonJointD JointPos;  // UR, TM (J1 ~ J6)
        public YoonCartesianD CartPos;    // UR, TM (X, Y, Z, RX, R

        public int VelocityPercent;

        public string[] ArraySocketParam;   // UR, TM
    }

    public struct ReceiveValue
    {
        public YoonJointD JointPos;   // UR, TM
        public YoonCartesianD CartPos;     // UR, TM
    }

    public class ResultArgs : EventArgs
    {
        public eYoonStatus Status;
        public string Message;
        public eYooneHeadReceive ReceiveHead;
        public ReceiveValue ReceiveData;

        public ResultArgs(eYoonStatus nStatus, string strMessage)
        {
            Status = nStatus;
            ReceiveHead = eYooneHeadReceive.None;
            ReceiveData = new ReceiveValue();
            Message = strMessage;
        }
        public ResultArgs(eYoonStatus nStatus, eYooneHeadReceive nHead, string strMessage)
        {
            Status = nStatus;
            ReceiveHead = eYooneHeadReceive.None;
            ReceiveData = new ReceiveValue();
            Message = strMessage;
        }

        public ResultArgs(eYooneHeadReceive nHead, ReceiveValue pDataReceive, string strMessage)
        {
            Status = eYoonStatus.OK;
            ReceiveHead = nHead;
            Message = strMessage;
            ReceiveData = new ReceiveValue();
            {
                if (pDataReceive.JointPos != null)
                    ReceiveData.JointPos = pDataReceive.JointPos.Clone() as YoonJointD;
                if (pDataReceive.CartPos != null)
                    ReceiveData.CartPos = pDataReceive.CartPos.Clone() as YoonCartesianD;
            }
        }

    }

    public delegate void RemoteResultCallback(object sender, ResultArgs e);
}
