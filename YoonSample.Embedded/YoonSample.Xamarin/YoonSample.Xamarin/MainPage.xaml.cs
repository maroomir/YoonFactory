using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
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
                        CommonClass.pTcpIp.Address = CommonClass.pParamConnect.IPAddress;
                        CommonClass.pTcpIp.Port = CommonClass.pParamConnect.Port.ToString();
                        break;
                    case eYoonTCPType.Server:
                        CommonClass.pTcpIp = new YoonServer();
                        CommonClass.pTcpIp.Port = CommonClass.pParamConnect.Port.ToString();
                        break;
                    default:
                        break;
                }
            }
        }

        void OnToggledTcpSettingSwitch(Object sender, ToggledEventArgs e)
        {
            CommonClass.pCLM.Write("Toggle TCP Setting Switch");
            if (CommonClass.pTcpIp != null)
            {
                CommonClass.pCLM.Write("TCP Connection Close Directly");
                CommonClass.pTcpIp.Close();
                Thread.Sleep(100);
                CommonClass.pTcpIp.Dispose();
            }
            if (e.Value)
            {
                CommonClass.pParamConnect.Type = eYoonTCPType.Server;
                CommonClass.pTcpIp = new YoonServer();
            }
            else
            {
                CommonClass.pParamConnect.Type = eYoonTCPType.Client;
                CommonClass.pTcpIp = new YoonClient();
            }
        }

        void OnClickSendMessageButton(Object sender, EventArgs e)
        {
            string strMessage = entry_SendMessage.Text;
            CommonClass.pCLM.Write("Send Message To" + strMessage);
            //
        }

        void OnClickOpenConnectionButton(Object sender, EventArgs e)
        {
            switch(CommonClass.pParamConnect.Type)
            {
                case eYoonTCPType.Client:
                    CommonClass.pParamConnect.IPAddress = entry_SettingIPAddress.Text;
                    break;
                case eYoonTCPType.Server:
                    break;
                default:
                    break;
            }
        }
    }
}
