using System;
using System.IO;
using YoonFactory.Param;
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
        public static YoonParameter pConnectManager = new YoonParameter(pParamConnect, typeof(ParameterConnection));
        public static IYoonTcpIp pTcpIp = null;
    }
}
