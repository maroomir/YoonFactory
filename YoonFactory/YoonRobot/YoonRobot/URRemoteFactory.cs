namespace YoonFactory.Robot.Remote
{
    public static class URFactory
    {
        public static string EncodingMessage(eYoonRobotRemoteHeadSend nHeader, ParamRobotSend pParamData)
        {
            string strMessage = string.Empty;

            switch (nHeader)
            {
                case eYoonRobotRemoteHeadSend.StatusRun:
                    strMessage = "programState";
                    break;
                case eYoonRobotRemoteHeadSend.LoadProject:
                    if (pParamData.ProgramName == string.Empty) break;
                    if (pParamData.ProjectName != string.Empty)
                        strMessage = "Load " + string.Format("/{0}/{1}.urp", pParamData.ProjectName, pParamData.ProgramName);
                    else
                        strMessage = "Load " + string.Format("/{0}.urp", pParamData.ProgramName);
                    break;
                case eYoonRobotRemoteHeadSend.Play:
                    strMessage = "Play";
                    break;
                case eYoonRobotRemoteHeadSend.Pause:
                    strMessage = "Pause";
                    break;
                case eYoonRobotRemoteHeadSend.Stop:
                    strMessage = "Stop";
                    break;
                case eYoonRobotRemoteHeadSend.Quit:
                    strMessage = "Quit";
                    break;
                case eYoonRobotRemoteHeadSend.Shutdown:
                    strMessage = "Shutdown";
                    break;
                case eYoonRobotRemoteHeadSend.PowerOn:
                    strMessage = "power on";
                    break;
                case eYoonRobotRemoteHeadSend.PowerOff:
                    strMessage = "power off";
                    break;
                case eYoonRobotRemoteHeadSend.BreakRelease:
                    strMessage = "brake release";
                    break;
                default:
                    break;
            }
            if (strMessage != string.Empty)
                strMessage += System.Environment.NewLine;   // Input \r\n (개행문자)

            return strMessage;
        }
        public static eYoonRobotRemoteHeadReceive DecodingMessage(string strMessage, ref ParamRobotReceive pParamData)
        {
            eYoonRobotRemoteHeadReceive nHeader = eYoonRobotRemoteHeadReceive.None;

            if (strMessage.Contains("PLAYING"))
                nHeader = eYoonRobotRemoteHeadReceive.StatusRunOK;
            else if (strMessage.Contains("STOPPED") || strMessage.Contains("PAUSED"))
                nHeader = eYoonRobotRemoteHeadReceive.StatusRunNG;
            else if (strMessage.Contains("Loading program"))
                nHeader = eYoonRobotRemoteHeadReceive.LoadOK;
            else if (strMessage.Contains("File not found") || strMessage.Contains("Error while loading"))
                nHeader = eYoonRobotRemoteHeadReceive.LoadNG;
            else if (strMessage.Contains("Starting program"))
                nHeader = eYoonRobotRemoteHeadReceive.PlayOK;
            else if (strMessage.Contains("Failed to execute: Play"))
                nHeader = eYoonRobotRemoteHeadReceive.PlayNG;
            else if (strMessage.Contains("Stopped"))
                nHeader = eYoonRobotRemoteHeadReceive.StopOk;
            else if (strMessage.Contains("Failed to execute: Stop"))
                nHeader = eYoonRobotRemoteHeadReceive.StopNG;
            else if (strMessage.Contains("Pausing"))
                nHeader = eYoonRobotRemoteHeadReceive.PauseOK;
            else if (strMessage.Contains("Failed to execute: Pause"))
                nHeader = eYoonRobotRemoteHeadReceive.PauseNG;
            else if (strMessage.Contains("Disconnected"))
                nHeader = eYoonRobotRemoteHeadReceive.QuitOK;
            else if (strMessage.Contains("Shutting down"))
                nHeader = eYoonRobotRemoteHeadReceive.ShuwdownOK;
            else if (strMessage.Contains("running: true"))
                nHeader = eYoonRobotRemoteHeadReceive.StatusRunOK;
            else if (strMessage.Contains("running: false"))
                nHeader = eYoonRobotRemoteHeadReceive.StatusRunNG;
            else if (strMessage.Contains("Powering on"))
                nHeader = eYoonRobotRemoteHeadReceive.PowerOnOK;
            else if (strMessage.Contains("Powering off"))
                nHeader = eYoonRobotRemoteHeadReceive.PowerOffOK;
            else if (strMessage.Contains("Brake releasing"))
                nHeader = eYoonRobotRemoteHeadReceive.BreakReleaseOK;

            return nHeader;
        }

    }
}
