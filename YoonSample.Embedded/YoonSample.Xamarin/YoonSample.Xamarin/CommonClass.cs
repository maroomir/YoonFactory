using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using YoonFactory.Files.Core;
using YoonFactory.Log;
using YoonFactory.Comm;

namespace YoonSample.Xamarin
{
    public static class CommonClass
    {
        // Configuration Parameter
        public static ParameterConnection pParamConnect = new ParameterConnection();
        // Configurqtion YoonFactory
        public static ConsoleLogManager pCLM = new ConsoleLogManager(30);
        public static YoonParameter pConnectManager = new YoonParameter(pParamConnect, typeof(ParameterConnection));
        public static IYoonTcpIp pTcpIp = null;
    }
}
