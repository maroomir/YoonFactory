using System;
using Gtk;
using YoonSample.RobotRemote;
using YoonFactory.Param;
using YoonFactory.Log;
using YoonFactory.Comm;
using YoonFactory.Robot.UniversialRobot;

public partial class MainWindow : Gtk.Window
{
    private YoonUR pRemote = null;
    private YoonConsoler pCLM = null;
    private YoonParameter pParamManager = null;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();

        InitComponent();
        InitEvent();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    private void InitComponent()
    {
        pCLM = new YoonConsoler(30);
        pCLM.Write("Init Robots");
        pRemote = new YoonUR();
        pRemote.OnShowMessageEvent += OnShowMessageEvent;
        pRemote.OnShowReceiveDataEvent += OnShowReceiveDataEvent;
        pCLM.Write("Init Parameter");
        ParameterClass pParam = new ParameterClass();
        pParamManager = new YoonParameter(pParam, typeof(ParameterClass));
        pParamManager.LoadParameter(true);
    }

    private void OnShowMessageEvent(object sender, MessageArgs e)
    {
        pCLM.Write("[COMM] " + e.Message);
    }

    private void OnShowReceiveDataEvent(object sender, BufferArgs e)
    {
        pCLM.Write("[RECV] " + e.StringData);
    }

    private void InitEvent()
    {
        pCLM.Write("Init Events");
        button_Power.Clicked += OnPowerButtonClick;
        button_SendOn.Clicked += OnSendOnButtonClick;
        button_SendOff.Clicked += OnSendOffButtonClick;
    }

    private void OnPowerButtonClick(object sender, EventArgs e)
    {
        if (pRemote.PowerOffRobot())
            pCLM.Write("[SEND] Power Off");
    }

    private void OnSendOnButtonClick(object sender, EventArgs e)
    {
        ParameterClass pParam = pParamManager.Parameter as ParameterClass;
        if (pRemote.SendSocket(pParam.OnMessage))
            pCLM.Write("[SEND] " + pParam.OnMessage);
    }

    private void OnSendOffButtonClick(object sender, EventArgs e)
    {
        ParameterClass pParam = pParamManager.Parameter as ParameterClass;
        if (pRemote.SendSocket(pParam.OffMessage))
            pCLM.Write("[SEND] " + pParam.OffMessage);
    }
}
