using YoonFactory.Comm;

namespace YoonFactory.Robot.UniversialRobot
{
    public static class RemoteFactory
    {
        public static string EncodingMessage(eYoonHeadSend nHeader, ParameterPacket pParamData)
        {
            string strMessage = string.Empty;

            switch (nHeader)
            {
                case eYoonHeadSend.StatusRun:
                    strMessage = "programState";
                    break;
                case eYoonHeadSend.LoadProject:
                    if (pParamData.ProgramName == string.Empty) break;
                    if (pParamData.ProjectName != string.Empty)
                        strMessage = "Load " + string.Format("/{0}/{1}.urp", pParamData.ProjectName, pParamData.ProgramName);
                    else
                        strMessage = "Load " + string.Format("/{0}.urp", pParamData.ProgramName);
                    break;
                case eYoonHeadSend.Play:
                    strMessage = "Play";
                    break;
                case eYoonHeadSend.Pause:
                    strMessage = "Pause";
                    break;
                case eYoonHeadSend.Stop:
                    strMessage = "Stop";
                    break;
                case eYoonHeadSend.Quit:
                    strMessage = "Quit";
                    break;
                case eYoonHeadSend.Shutdown:
                    strMessage = "Shutdown";
                    break;
                case eYoonHeadSend.PowerOn:
                    strMessage = "power on";
                    break;
                case eYoonHeadSend.PowerOff:
                    strMessage = "power off";
                    break;
                case eYoonHeadSend.BreakRelease:
                    strMessage = "brake release";
                    break;
                default:
                    break;
            }
            if (strMessage != string.Empty)
                strMessage += System.Environment.NewLine;   // Input \r\n (개행문자)

            return strMessage;
        }

        public static eYooneHeadReceive DecodingMessage(string strMessage, ref ParameterPacket pParamData)
        {
            eYooneHeadReceive nHeader = eYooneHeadReceive.None;

            if (strMessage.Contains("PLAYING"))
                nHeader = eYooneHeadReceive.StatusRunOK;
            else if (strMessage.Contains("STOPPED") || strMessage.Contains("PAUSED"))
                nHeader = eYooneHeadReceive.StatusRunNG;
            else if (strMessage.Contains("Loading program"))
                nHeader = eYooneHeadReceive.LoadOK;
            else if (strMessage.Contains("File not found") || strMessage.Contains("Error while loading"))
                nHeader = eYooneHeadReceive.LoadNG;
            else if (strMessage.Contains("Starting program"))
                nHeader = eYooneHeadReceive.PlayOK;
            else if (strMessage.Contains("Failed to execute: Play"))
                nHeader = eYooneHeadReceive.PlayNG;
            else if (strMessage.Contains("Stopped"))
                nHeader = eYooneHeadReceive.StopOk;
            else if (strMessage.Contains("Failed to execute: Stop"))
                nHeader = eYooneHeadReceive.StopNG;
            else if (strMessage.Contains("Pausing"))
                nHeader = eYooneHeadReceive.PauseOK;
            else if (strMessage.Contains("Failed to execute: Pause"))
                nHeader = eYooneHeadReceive.PauseNG;
            else if (strMessage.Contains("Disconnected"))
                nHeader = eYooneHeadReceive.QuitOK;
            else if (strMessage.Contains("Shutting down"))
                nHeader = eYooneHeadReceive.ShuwdownOK;
            else if (strMessage.Contains("running: true"))
                nHeader = eYooneHeadReceive.StatusRunOK;
            else if (strMessage.Contains("running: false"))
                nHeader = eYooneHeadReceive.StatusRunNG;
            else if (strMessage.Contains("Powering on"))
                nHeader = eYooneHeadReceive.PowerOnOK;
            else if (strMessage.Contains("Powering off"))
                nHeader = eYooneHeadReceive.PowerOffOK;
            else if (strMessage.Contains("Brake releasing"))
                nHeader = eYooneHeadReceive.BreakReleaseOK;

            return nHeader;
        }

        public static eYoonRemoteType GetRemoteCommType(eYoonHeadSend nHeaderData)
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
                    return eYoonRemoteType.Dashboard;
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
