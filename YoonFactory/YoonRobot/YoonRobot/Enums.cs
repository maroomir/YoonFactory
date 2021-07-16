namespace YoonFactory
{
    public enum eYoonRobotType : int
    {
        None = -1,
        UR,
        TM,
    }

    public enum eYoonRemoteType : int
    {
        None = -1,
        Socket,
        Script,
        Dashboard,
    }

    public enum eYoonHeadSend : int   // REMOTE MODE = UR : DASHBOARD
    {
        None = -1,
        StatusServo = 0,
        StatusError,
        StatusRun,
        StatusSafety,
        LoadProject,
        LoadJob,
        Reset,
        Play,
        Pause,
        Stop,
        Quit,
        Shutdown,
        PowerOn,
        PowerOff,
        BreakRelease,
    }

    public enum eYooneHeadReceive : int    // REMOTE MODE = UR : DASHBOARD
    {
        None = -1,
        StatusServoOK = 0,
        StatusServoNG,
        StatusErrorOK,
        StatusErrorNG,
        StatusRunOK,
        StatusRunNG,
        StatusSafetyOK,
        StatusSafetyNG,
        LoadOK,
        LoadNG,
        ResetOK,
        ResetNG,
        PlayOK,
        PlayNG,
        PauseOK,
        PauseNG,
        StopOk,
        StopNG,
        QuitOK,
        ShuwdownOK,
        PowerOnOK,
        PowerOffOK,
        BreakReleaseOK,
    }

}
