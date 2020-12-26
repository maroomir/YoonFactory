using System;
using System.Collections.Generic;
using Gtk;
using YoonSample.Mono;
using YoonFactory;
using YoonFactory.Files.Core;
using YoonFactory.Mono;
using YoonFactory.Mono.Log;
using YoonFactory.Comm;
using YoonFactory.Comm.TCP;

public partial class MainWindow : Gtk.Window
{
    private string m_strTcpBacklog = string.Empty;
    
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();

        InitProgram();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void InitProgram()
    {
        int nStepInit = 0;

        CommonClass.pCLM.Write(string.Format("Step {0} : Init Log Manager", nStepInit++));
        {
            CommonClass.pDLM.OnLogDisplayEvent += OnDisplatLogEvent;
        }

        CommonClass.pCLM.Write(string.Format("Step {0} : Init Configuration", nStepInit++));
        {
            CommonClass.pConnectManager.RootDirectory = CommonClass.strParameterDirectory;
            CommonClass.pConnectManager.LoadParameter(true);
            CommonClass.pActionManager.RootDirectory = CommonClass.strParameterDirectory;
            CommonClass.pActionManager.LoadParameter(true);
            //// Pulls out the setting value
            CommonClass.pParamConnect = CommonClass.pConnectManager.Parameter as ParameterConnection;
            CommonClass.pParamAction = CommonClass.pActionManager.Parameter as ParameterAction;
        }

        CommonClass.pCLM.Write(string.Format("Step {0} : Init Connection", nStepInit++));
        {
            switch (CommonClass.pParamConnect.Type)
            {
                case eCommType.TCPServer:
                    CommonClass.pTCPServer = new YoonServer();
                    CommonClass.pTCPServer.Port = CommonClass.pParamConnect.Port.ToString();
                    CommonClass.pTCPServer.OnShowMessageEvent += OnDisplayTcpIpMessageEvent;
                    CommonClass.pTCPServer.OnShowReceiveDataEvent += OnReceiveTcpIpDataEvent;
                    break;
                case eCommType.TCPClient:
                    CommonClass.pTCPClient = new YoonClient();
                    CommonClass.pTCPClient.Address = CommonClass.pParamConnect.IPAddress;
                    CommonClass.pTCPClient.Port = CommonClass.pParamConnect.Port.ToString();
                    CommonClass.pTCPClient.OnShowMessageEvent += OnDisplayTcpIpMessageEvent;
                    CommonClass.pTCPClient.OnShowReceiveDataEvent += OnReceiveTcpIpDataEvent;
                    break;
                default:
                    break;
            }
        }

        CommonClass.pCLM.Write(string.Format("Step {0} : Init Main Interface", nStepInit++));
        {
            GtkFactory.ChangeWidgetFont(label_TitleMain, PangoFont.SubTitle(), GtkColor.Black, StateType.Normal);
            GtkFactory.ChangeWidgetFont(label_TitleSetting, PangoFont.SubTitle(), GtkColor.Black, StateType.Normal);
            GtkFactory.ChangeWidgetFont(label_TitleAction, PangoFont.SubTitle(), GtkColor.Black, StateType.Normal);

            GtkFactory.ChangeContainerFont(vbox_Main, PangoFont.Default(), GtkColor.Black, StateType.Normal);
        }

        CommonClass.pCLM.Write(string.Format("Step {0} : Init Setting Interface", nStepInit++));
        {
            GtkFactory.SetComboboxItems(ref combobox_Connection, new string[]
            { eCommType.TCPServer.ToString(), eCommType.TCPClient.ToString(), eCommType.RS232.ToString(), eCommType.RS422.ToString()});

            entry_TcpSettingIP.Text = CommonClass.pParamConnect.IPAddress.ToString();
            entry_TcpSettingPort.Text = CommonClass.pParamConnect.Port.ToString();
        }

        CommonClass.pCLM.Write(string.Format("Step {0} : Init Action Interface", nStepInit++));
        {
            GtkFactory.ChangeContainerFont(vbox_Action, PangoFont.Default(), GtkColor.Black, StateType.Normal);

            label_ActionSelect.SetSizeRequest(CommonClass.DEFAULT_CHECKBOX_WIDTH, CommonClass.DEFAULT_CHECKBOX_HEIGHT);
            label_ActionName.SetSizeRequest(CommonClass.DEFAULT_ENTRY_WIDTH, CommonClass.DEFAULT_ENTRY_HEIGHT);
            label_ActionValue.SetSizeRequest(CommonClass.DEFAULT_ENTRY_WIDTH, CommonClass.DEFAULT_ENTRY_HEIGHT);

            if (CommonClass.pListPanelAction == null)
                CommonClass.pListPanelAction = new List<ActionPanel>();

            //// Clear Recipe Panel List
            foreach (ActionPanel pPanel in CommonClass.pListPanelAction)
            {
                if (GtkFactory.IsWidgetInContainter(vbox_Action, pPanel.ParameterHBox))
                    vbox_Action.Remove(pPanel.ParameterHBox);
                pPanel.Dispose();
            }
            CommonClass.pListPanelAction.Clear();

            //// Init Recipe Panel View
            CommonClass.pCLM.Write("Action Panel Set : Param Count = " + CommonClass.MAX_ACTION_NUM.ToString());
            for (int iParam = 0; iParam < CommonClass.MAX_ACTION_NUM; iParam++)
            {
                ActionPanel pPanel = new ActionPanel(iParam + 1, CommonClass.pParamAction.ActionNames[iParam], CommonClass.pParamAction.ActionValues[iParam]);
                pPanel.SetEntryInterface(CommonClass.DEFAULT_ENTRY_WIDTH, CommonClass.DEFAULT_ENTRY_HEIGHT, PangoFont.Comment());
                pPanel.SetCheckboxInterface(CommonClass.DEFAULT_CHECKBOX_WIDTH, CommonClass.DEFAULT_CHECKBOX_HEIGHT, PangoFont.Comment());
                CommonClass.pListPanelAction.Add(pPanel);
                Box pBox = vbox_Action;
                GtkFactory.SetWidgetInBoxLayout(ref pBox, pPanel.ParameterHBox, iParam + 2); // 0 : Button, 1 : Titles
            }
        }

        CommonClass.pCLM.Write(string.Format("Step {0} : Init Event", nStepInit++));
        {
            button_TcpConnect.Clicked += OnButtonTcpClientClick;
            button_TcpDisconnect.Clicked += OnButtonTcpClientClick;

            button_TcpListen.Clicked += OnButtonTcpServerClick;
            button_TcpClose.Clicked += OnButtonTcpServerClick;

            button_Action1.Clicked += OnButtonActionClicked;
            button_Action2.Clicked += OnButtonActionClicked;
            button_Action3.Clicked += OnButtonActionClicked;
            button_Action4.Clicked += OnButtonActionClicked;
            button_Action5.Clicked += OnButtonActionClicked;
            button_Action6.Clicked += OnButtonActionClicked;
            button_Action7.Clicked += OnButtonActionClicked;
            button_Action8.Clicked += OnButtonActionClicked;
            button_Action9.Clicked += OnButtonActionClicked;
            button_Action10.Clicked += OnButtonActionClicked;
            button_Action11.Clicked += OnButtonActionClicked;
            button_Action12.Clicked += OnButtonActionClicked;

            button_SaveSetting.Clicked += OnButtonSettingControlClicked;
            button_ReloadSetting.Clicked += OnButtonSettingControlClicked;

            button_SaveAction.Clicked += OnButtonActionControlClicked;
            button_ReloadAction.Clicked += OnButtonActionControlClicked;
        }

        string strMessage = "Completed Init Program";
        CommonClass.pDLM.Write(eYoonStatus.Info, strMessage);
        CommonClass.pCLM.Write(strMessage);
    }

    public void OnDisplatLogEvent(object sender, LogArgs e)
    {
        GtkFactory.ChangeContainerColor(textview_Log, e.BackColor, StateType.Normal);
        textview_Log.Buffer.Text += e.Message;
    }

    public void OnDisplayTcpIpMessageEvent(object sender, MessageArgs e)
    {
        CommonClass.pDLM.Write(e.Status, e.Message);
    }

    public void OnReceiveTcpIpDataEvent(object sender, BufferArgs e)
    {
        //
    }

    protected void OnButtonTcpClientClick(object sender, EventArgs e)
    {
        Button buttonSelected = (Button)sender;
        switch(buttonSelected.Label)
        {
            case "Connect":
                if (CommonClass.pTCPClient.IsConnected) return;
                CommonClass.pTCPClient.Address = CommonClass.pParamConnect.IPAddress;
                CommonClass.pTCPClient.Port = CommonClass.pParamConnect.Port.ToString();
                CommonClass.pTCPClient.Connect();
                break;
            case "Disconnect":
                if (!CommonClass.pTCPClient.IsConnected) return;
                CommonClass.pTCPClient.Disconnect();
                break;
        }
    }

    protected void OnButtonTcpServerClick(object sender, EventArgs e)
    {
        Button buttonSelected = (Button)sender;
        switch (buttonSelected.Label)
        {
            case "Listen":
                if (CommonClass.pTCPServer.IsBound) return;
                CommonClass.pTCPServer.Port = CommonClass.pParamConnect.Port.ToString();
                CommonClass.pTCPServer.Listen();
                break;
            case "Close":
                if (!CommonClass.pTCPServer.IsBound) return;
                CommonClass.pTCPServer.Close();
                break;
        }
    }

    protected void OnButtonSerialClick(object sender, EventArgs e)
    {
        Button buttonSelected = (Button)sender;
        switch(buttonSelected.Label)
        {
            case "Connect":
                break;
            case "Disconnect":
                break;
        }
    }

    protected void OnButtonActionClicked(object sender, EventArgs e)
    {
        Button buttonSelected = (Button)sender;
        switch(buttonSelected.Label)
        {
            case "Action1":
                break;
            case "Action2":
                break;
            case "Action3":
                break;
            case "Action4":
                break;
            case "Action5":
                break;
            case "Action6":
                break;
            case "Action7":
                break;
            case "Action8":
                break;
            case "Action9":
                break;
            case "Action10":
                break;
            case "Action11":
                break;
            case "Action12":
                break;
        }
    }

    protected void OnButtonSettingControlClicked(object sender, EventArgs e)
    {
        Button buttonSelected = (Button)sender;
        switch(buttonSelected.Label)
        {
            case "Save":
                break;
            case "Reload":
                break;
        }
    }

    protected void OnButtonActionControlClicked(object sender, EventArgs e)
    {
        Button buttonSelected = (Button)sender;
        switch(buttonSelected.Label)
        {
            case "Save":
                break;
            case "Reload":
                break;
        }
    }
}
