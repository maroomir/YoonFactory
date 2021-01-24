using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using YoonFactory.Param;
using YoonFactory.Log;
using YoonFactory.Comm;

namespace YoonSample.Xamarin
{
    public static class CommonClass
    {
        // Configuration Parameter
        public static ParameterConnection pParamConnect = new ParameterConnection();
        // Directory Setting
        public static string strApplicationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string strYoonFactoryDirectory = Path.Combine(strApplicationDirectory, "YoonFactory");
        // Configurqtion YoonFactory
        public static YoonConsoler pCLM = new YoonConsoler(30);
        public static YoonParameter pConnectManager = new YoonParameter(pParamConnect, typeof(ParameterConnection));
        public static IYoonTcpIp pTcpIp = null;
    }
}
