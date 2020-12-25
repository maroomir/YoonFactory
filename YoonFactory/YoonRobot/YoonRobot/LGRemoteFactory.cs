namespace YoonFactory.Robot.Remote
{
    public static class LGRemote
    {
        public static string EncodingMessage(eYoonRobotRemoteHeadSend nHeader, ParamRobotSend pParamData)
        {
            string strMessage = string.Empty;

            switch (nHeader)
            {
                case eYoonRobotRemoteHeadSend.StatusRun:
                    strMessage = "REQ,STAT,RUN";
                    break;
                case eYoonRobotRemoteHeadSend.StatusServo:
                    strMessage = "REQ,STAT,SERVO";
                    break;
                case eYoonRobotRemoteHeadSend.StatusSafety:
                    strMessage = "REQ,STAT,SAFE";
                    break;
                case eYoonRobotRemoteHeadSend.StatusError:
                    strMessage = "REQ,STAT,ERR";
                    break;
                case eYoonRobotRemoteHeadSend.Reset:
                    strMessage = "RST";
                    break;
                case eYoonRobotRemoteHeadSend.LoadProject:
                    if (pParamData.ProjectName != string.Empty)
                        strMessage = "LOA,PRJ," + pParamData.ProjectName;
                    break;
                case eYoonRobotRemoteHeadSend.LoadJob:
                    if (pParamData.ProgramName != string.Empty)
                        strMessage = "LOA,PRG," + pParamData.ProgramName;
                    break;
                case eYoonRobotRemoteHeadSend.Play:
                    strMessage = "STA";
                    break;
                case eYoonRobotRemoteHeadSend.Pause:
                    strMessage = "STO";
                    break;
                case eYoonRobotRemoteHeadSend.Stop:
                    strMessage = "EXT";
                    break;
                default:
                    break;
            }
            return strMessage;
        }

        public static eYoonRobotRemoteHeadReceive DecodingMessage(string strMessage, ref ParamRobotReceive pParamData)
        {
            eYoonRobotRemoteHeadReceive nHeader = eYoonRobotRemoteHeadReceive.None;

            switch (strMessage)
            {
                case "STAT,RUN,TRUE":
                    nHeader = eYoonRobotRemoteHeadReceive.StatusRunOK;
                    break;
                case "STAT,RUN,FALSE":
                    nHeader = eYoonRobotRemoteHeadReceive.StatusRunNG;
                    break;
                case "STAT,SERVO,TRUE":
                    nHeader = eYoonRobotRemoteHeadReceive.StatusServoOK;
                    break;
                case "STAT,SERVO,FALSE":
                    nHeader = eYoonRobotRemoteHeadReceive.StatusServoNG;
                    break;
                case "STAT,SAFE,TRUE":
                    nHeader = eYoonRobotRemoteHeadReceive.StatusSafetyOK;
                    break;
                case "STAT,SAFE,FALSE":
                    nHeader = eYoonRobotRemoteHeadReceive.StatusSafetyNG;
                    break;
                case "STAT,ERR,TRUE":
                    nHeader = eYoonRobotRemoteHeadReceive.StatusErrorOK;
                    break;
                case "STAT,ERR,FALSE":
                    nHeader = eYoonRobotRemoteHeadReceive.StatusErrorNG;
                    break;
                case "RCV,RST":
                    nHeader = eYoonRobotRemoteHeadReceive.ResetOK;
                    break;
                case "RCV,STA":
                    nHeader = eYoonRobotRemoteHeadReceive.PlayOK;
                    break;
                case "RCV,STO":
                    nHeader = eYoonRobotRemoteHeadReceive.PauseOK;
                    break;
                case "RCV,EXT":
                    nHeader = eYoonRobotRemoteHeadReceive.StopOk;
                    break;
                case "RCV,LOA":
                    nHeader = eYoonRobotRemoteHeadReceive.LoadNG;
                    break;
                case "ERR,RST":
                    nHeader = eYoonRobotRemoteHeadReceive.ResetNG;
                    break;
                case "ERR,SVO":
                    nHeader = eYoonRobotRemoteHeadReceive.PlayNG;
                    break;
                case "ERR,STA":
                    nHeader = eYoonRobotRemoteHeadReceive.PlayNG;
                    break;
                case "ERR,STO":
                    nHeader = eYoonRobotRemoteHeadReceive.PauseNG;
                    break;
                case "ERR,EXT":
                    nHeader = eYoonRobotRemoteHeadReceive.StopNG;
                    break;
                case "ERR,LOA":
                    nHeader = eYoonRobotRemoteHeadReceive.LoadNG;
                    break;
                default:
                    break;
            }

            return nHeader;
        }
    }
}
