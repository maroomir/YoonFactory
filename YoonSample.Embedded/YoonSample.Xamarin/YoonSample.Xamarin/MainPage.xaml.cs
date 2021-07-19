using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using YoonFactory;
using YoonFactory.Comm;
using YoonFactory.Comm.TCP;

namespace YoonSample.Xamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            CommonClass.pConnectManager.RootDirectory = CommonClass.strYoonFactoryDirectory;
            {
                CommonClass.pConnectManager.LoadParameter(true);
                CommonClass.pParamConnect = CommonClass.pConnectManager.Parameter as ParameterConnection;
                entry_SettingIPAddress.Text = CommonClass.pParamConnect.IPAddress;
                entry_SettingPort.Text = CommonClass.pParamConnect.Port.ToString();
                if (CommonClass.pParamConnect.Type == eYoonCommType.TCPServer)
                    switch_SettingTCPServer.IsToggled = true;
                else
                    switch_SettingTCPServer.IsToggled = false;
            }
            ////  Fill the configuration
            switch (CommonClass.pParamConnect.Type)
            {
                case eYoonCommType.TCPClient:
                    CommonClass.pTcpIp = new YoonClient(CommonClass.strYoonFactoryDirectory);
                    break;
                case eYoonCommType.TCPServer:
                    CommonClass.pTcpIp = new YoonServer(CommonClass.strYoonFactoryDirectory);
                    break;
                default:
                    break;
            }
            CommonClass.pTcpIp.Address = CommonClass.pParamConnect.IPAddress;
            CommonClass.pTcpIp.Port = CommonClass.pParamConnect.Port.ToString();
            CommonClass.pTcpIp.OnShowMessageEvent += OnWriteTcpCommMessage;
            CommonClass.pTcpIp.OnShowReceiveDataEvent += OnReceiveTcpCommData;
        }

        void OnWriteTcpCommMessage(object sender, MessageArgs e)
        {
            //
        }

        void OnReceiveTcpCommData(object sender, BufferArgs e)
        {
            editor_ReceiveMessages.Text = e.StringData;
        }

        void OnToggledTcpSettingSwitch(Object sender, ToggledEventArgs e)
        {
            if (CommonClass.pTcpIp != null)
            {
                CommonClass.pTcpIp.Close();
                Thread.Sleep(100);
                CommonClass.pTcpIp.OnShowMessageEvent -= OnWriteTcpCommMessage;
                CommonClass.pTcpIp.OnShowReceiveDataEvent -= OnReceiveTcpCommData;
                CommonClass.pTcpIp.Dispose();
            }
            if (e.Value)
            {
                CommonClass.pParamConnect.Type = eYoonCommType.TCPServer;
                CommonClass.pTcpIp = new YoonServer(CommonClass.strYoonFactoryDirectory);
                CommonClass.pTcpIp.OnShowMessageEvent += OnWriteTcpCommMessage;
                CommonClass.pTcpIp.OnShowReceiveDataEvent += OnReceiveTcpCommData;
            }
            else
            {
                CommonClass.pParamConnect.Type = eYoonCommType.TCPClient;
                CommonClass.pTcpIp = new YoonClient(CommonClass.strYoonFactoryDirectory);
                CommonClass.pTcpIp.OnShowMessageEvent += OnWriteTcpCommMessage;
                CommonClass.pTcpIp.OnShowReceiveDataEvent += OnReceiveTcpCommData;
            }
        }

        void OnClickSendMessageButton(Object sender, EventArgs e)
        {
            if (CommonClass.pTcpIp.IsConnected)
            {
                string strMessage = entry_SendMessage.Text;
                CommonClass.pTcpIp.Send(strMessage);
            }
        }

        void OnClickChangeConnectionButton(Object sender, EventArgs e)
        {
            if (CommunicationFactory.VerifyIPAddress(entry_SettingIPAddress.Text))
                CommonClass.pParamConnect.IPAddress = entry_SettingIPAddress.Text;
            if (CommunicationFactory.VerifyTCPPort(entry_SettingPort.Text))
                CommonClass.pParamConnect.Port = Convert.ToInt32(entry_SettingPort.Text);
            //// Save the parameter directly
            CommonClass.pConnectManager.SetParameter(CommonClass.pParamConnect, typeof(ParameterConnection));
            if (CommonClass.pConnectManager.SaveParameter())
                CommonClass.pConnectManager.LoadParameter();
            //// Connection Open/Close
            CommonClass.pTcpIp.Address = CommonClass.pParamConnect.IPAddress;
            CommonClass.pTcpIp.Port = CommonClass.pParamConnect.Port.ToString();
            if (CommonClass.pTcpIp.IsConnected)
                CommonClass.pTcpIp.Close();
            else
                CommonClass.pTcpIp.Open();
            Thread.Sleep(200);
            ////  Change the Label
            if (CommonClass.pTcpIp.IsConnected)
                button_ChangeConnection.Text = "Disconnect";
            else
                button_ChangeConnection.Text = "Connect";
        }
    }
}
