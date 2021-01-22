using YoonFactory.Comm;

namespace YoonFactory.Robot
{
    public static class UniversalRobotics
    {
        public static ParameterRemote InitRemoteParameter(eYoonRemoteType nType)
        {
            ParameterRemote pParam = new ParameterRemote();
            switch (nType)
            {
                case eYoonRemoteType.Dashboard:
                    pParam.IPAddress = "192.168.101.101";
                    pParam.Port = "29999";
                    pParam.TCPMode = eYoonCommType.TCPClient;
                    break;
                case eYoonRemoteType.Script:
                    pParam.IPAddress = "192.168.101.101";
                    pParam.Port = "30001";
                    pParam.TCPMode = eYoonCommType.TCPClient;
                    break;
                case eYoonRemoteType.Socket:
                    pParam.IPAddress = "192.168.101.101";
                    pParam.Port = "50000";
                    pParam.TCPMode = eYoonCommType.TCPClient;
                    break;
                default:
                    break;
            }
            return pParam;
        }

        public static string EncodingMessage(eYoonHeadSend nHeader, SendValue pParamData)
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
        public static eYooneHeadReceive DecodingMessage(string strMessage, ref ReceiveValue pParamData)
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
    }
}
