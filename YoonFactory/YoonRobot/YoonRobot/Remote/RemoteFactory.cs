namespace YoonFactory.Robot.Remote
{
    public static class RemoteFactory
    {
        public static eYoonRemoteType GetRemoteCommType(eYoonRobotType nTypeCobot, eYoonHeadSend nHeaderData)
        {
            switch (nHeaderData)
            {
                case eYoonHeadSend.StatusRun:
                case eYoonHeadSend.StatusServo:
                case eYoonHeadSend.StatusError:
                case eYoonHeadSend.StatusSafety:
                case eYoonHeadSend.Reset:
                case eYoonHeadSend.Play:
                case eYoonHeadSend.Pause:
                case eYoonHeadSend.Stop:
                case eYoonHeadSend.Quit:
                case eYoonHeadSend.Shutdown:
                case eYoonHeadSend.PowerOn:
                case eYoonHeadSend.PowerOff:
                case eYoonHeadSend.BreakRelease:
                    switch (nTypeCobot)
                    {
                        case eYoonRobotType.UR:
                            return eYoonRemoteType.Dashboard;
                    }
                    break;
            }
            return eYoonRemoteType.None;
        }

        public static eYoonStatus GetRemoteStatus(eYooneHeadReceive nHeader)
        {
            if (nHeader.ToString().Contains("OK"))
                return eYoonStatus.OK;
            else if (nHeader.ToString().Contains("NG"))
                return eYoonStatus.NG;
            else
                return eYoonStatus.User;
        }

        public static eYoonRobotType GetRobotTypeFromString(string strLabelName)
        {
            if (strLabelName.Contains("UR"))
                return eYoonRobotType.UR;
            else if (strLabelName.Contains("TM"))
                return eYoonRobotType.TM;
            else
                return eYoonRobotType.None;
        }

        public static eYoonRemoteType GetCommModeFromString(string strLabelName)
        {
            if (strLabelName.Contains("DashBoard"))
                return eYoonRemoteType.Dashboard;
            else if (strLabelName.Contains("Script"))
                return eYoonRemoteType.Script;
            else if (strLabelName.Contains("Socket"))
                return eYoonRemoteType.Socket;
            else
                return eYoonRemoteType.None;
        }

        public static string GetRemoteForcedMessage(eYoonRobotType nType, eYoonHeadSend nHeader, SendValue pParamData)
        {
            string strMessage = string.Empty;

            switch (nType)
            {
                case eYoonRobotType.UR:
                    strMessage = URFactory.EncodingMessage(nHeader, pParamData);
                    break;
                default:
                    break;
            }
            return strMessage;
        }

        public static eYooneHeadReceive GetRemoteReceiveHeader(eYoonRobotType nType, string strMessage, ref ReceiveValue pParamData)
        {
            eYooneHeadReceive nHeader = eYooneHeadReceive.None;

            switch (nType)
            {
                case eYoonRobotType.UR:
                    nHeader = URFactory.DecodingMessage(strMessage, ref pParamData);
                    break;
                default:
                    break;
            }
            return nHeader;
        }

        public static string GetRemoteLogFromReceiveHeader(eYooneHeadReceive nHead)
        {
            switch (nHead)
            {
                case eYooneHeadReceive.StatusServoOK:
                    return "Servo On";
                case eYooneHeadReceive.StatusServoNG:
                    return "Servo Off";
                case eYooneHeadReceive.StatusErrorOK:
                    return "Robot Error";
                case eYooneHeadReceive.StatusErrorNG:
                    return "Robot Error Clear";
                case eYooneHeadReceive.StatusRunOK:
                    return "Robot Running";
                case eYooneHeadReceive.StatusRunNG:
                    return "Robot Running Failure";
                case eYooneHeadReceive.StatusSafetyOK:
                    return "Workspace Safety";
                case eYooneHeadReceive.StatusSafetyNG:
                    return "Workspace Warning";
                case eYooneHeadReceive.LoadOK:
                    return "Load Success";
                case eYooneHeadReceive.LoadNG:
                    return "Load Failure";
                case eYooneHeadReceive.ResetOK:
                    return "Reset Completed";
                case eYooneHeadReceive.ResetNG:
                    return "Reset Failure";
                case eYooneHeadReceive.PlayOK:
                    return "Play";
                case eYooneHeadReceive.PlayNG:
                    return "Play Error";
                case eYooneHeadReceive.PauseOK:
                    return "Pause";
                case eYooneHeadReceive.PauseNG:
                    return "Pause Error";
                case eYooneHeadReceive.StopOk:
                    return "Stop";
                case eYooneHeadReceive.StopNG:
                    return "Stop Error";
                case eYooneHeadReceive.QuitOK:
                    return "Quit";
                case eYooneHeadReceive.ShuwdownOK:
                    return "Quit Error";
                case eYooneHeadReceive.PowerOnOK:
                    return "Robot Power On";
                case eYooneHeadReceive.PowerOffOK:
                    return "Robot Power Off";
                case eYooneHeadReceive.BreakReleaseOK:
                    return "Break Release";
                default:
                    return string.Empty;
            }
        }
    }
}
