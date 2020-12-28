using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using YoonFactory.Comm;
using YoonFactory.Comm.TCP;

namespace YoonSample.Xamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            int nStepInit = 0;
            CommonClass.pCLM.Write(string.Format("Step {0} : Init Parameter to Form", nStepInit++));
            {
                CommonClass.pConnectManager.LoadParameter(true);
                CommonClass.pParamConnect = CommonClass.pConnectManager.Parameter as ParameterConnection;
                entry_SettingIPAddress.Text = CommonClass.pParamConnect.IPAddress;
                entry_SettingPort.Text = CommonClass.pParamConnect.Port.ToString();
                if (CommonClass.pParamConnect.Type == eYoonTCPType.Server)
                    switch_SettingTCPServer.IsToggled = true;
                else
                    switch_SettingTCPServer.IsToggled = false;
            }

            CommonClass.pCLM.Write(string.Format("Step {0} : Init Tcp Connection", nStepInit++));
            {
                switch(CommonClass.pParamConnect.Type)
                {
                    case eYoonTCPType.Client:
                        CommonClass.pTcpIp = new YoonClient();
                        break;
                    case eYoonTCPType.Server:
                        CommonClass.pTcpIp = new YoonServer();
                        break;
                    default:
                        break;
                }
                CommonClass.pTcpIp.Address = CommonClass.pParamConnect.IPAddress;
                CommonClass.pTcpIp.Port = CommonClass.pParamConnect.Port.ToString();
                CommonClass.pTcpIp.OnShowMessageEvent += OnWriteTcpCommMessage;
                CommonClass.pTcpIp.OnShowReceiveDataEvent += OnReceiveTcpCommData;
            }
        }

        void OnWriteTcpCommMessage(object sender, MessageArgs e)
        {
            CommonClass.pCLM.Write(e.Message);
        }

        void OnReceiveTcpCommData(object sender, BufferArgs e)
        {
            editor_ReceiveMessages.Text = e.StringData;
        }

        void OnToggledTcpSettingSwitch(Object sender, ToggledEventArgs e)
        {
            CommonClass.pCLM.Write("Toggle TCP Setting Switch");
            if (CommonClass.pTcpIp != null)
            {
                CommonClass.pCLM.Write("TCP Connection Close Directly");
                CommonClass.pTcpIp.Close();
                Thread.Sleep(100);
                CommonClass.pTcpIp.OnShowMessageEvent -= OnWriteTcpCommMessage;
                CommonClass.pTcpIp.OnShowReceiveDataEvent -= OnReceiveTcpCommData;
                CommonClass.pTcpIp.Dispose();
            }
            if (e.Value)
            {
                CommonClass.pParamConnect.Type = eYoonTCPType.Server;
                CommonClass.pTcpIp = new YoonServer();
                CommonClass.pTcpIp.OnShowMessageEvent += OnWriteTcpCommMessage;
                CommonClass.pTcpIp.OnShowReceiveDataEvent += OnReceiveTcpCommData;
            }
            else
            {
                CommonClass.pParamConnect.Type = eYoonTCPType.Client;
                CommonClass.pTcpIp = new YoonClient();
                CommonClass.pTcpIp.OnShowMessageEvent += OnWriteTcpCommMessage;
                CommonClass.pTcpIp.OnShowReceiveDataEvent += OnReceiveTcpCommData;
            }
        }

        void OnClickSendMessageButton(Object sender, EventArgs e)
        {
            if (CommonClass.pTcpIp.IsConnected)
            {
                string strMessage = entry_SendMessage.Text;
                CommonClass.pCLM.Write("Send Message To" + strMessage);
                CommonClass.pTcpIp.Send(strMessage);
            }
        }

        void OnClickChangeConnectionButton(Object sender, EventArgs e)
        {
            CommonClass.pCLM.Write("Reset the Address and Port Value");
            {
                if (TCPFactory.VerifyIPAddress(entry_SettingIPAddress.Text))
                    CommonClass.pParamConnect.IPAddress = entry_SettingIPAddress.Text;
                if (TCPFactory.VerifyPort(entry_SettingPort.Text))
                    CommonClass.pParamConnect.Port = Convert.ToInt32(entry_SettingPort.Text);
            }
            CommonClass.pCLM.Write("Reset the Connection");
            {
                CommonClass.pTcpIp.Address = CommonClass.pParamConnect.IPAddress;
                CommonClass.pTcpIp.Port = CommonClass.pParamConnect.Port.ToString();
                if (CommonClass.pTcpIp.IsConnected)
                    CommonClass.pTcpIp.Close();
                else
                    CommonClass.pTcpIp.Open();
                Thread.Sleep(200);
            }
            CommonClass.pCLM.Write("Check the Connect Status");
            {
                if (CommonClass.pTcpIp.IsConnected)
                    button_ChangeConnection.Text = "Disconnect";
                else
                    button_ChangeConnection.Text = "Connect";
            }
        }
    }
}
