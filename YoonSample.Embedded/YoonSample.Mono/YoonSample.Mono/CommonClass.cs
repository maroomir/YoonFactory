using System.IO;
using System.Collections.Generic;
using YoonFactory.Comm.TCP;
using YoonFactory.Files.Core;
using YoonFactory.Log;
using YoonFactory.Mono.Log;

namespace YoonSample.Mono
{
    public enum eCommType
    {
        None=-1,
        TCPServer,
        TCPClient,
        RS232,
        RS422,
    }

    public static class CommonClass
    {
        // Const Parameter
        public const int MAX_ACTION_NUM = 12;
        public const int DEFAULT_ENTRY_WIDTH = 300;
        public const int DEFAULT_ENTRY_HEIGHT = 25;
        public const int DEFAULT_CHECKBOX_WIDTH = 100;
        public const int DEFAULT_CHECKBOX_HEIGHT = 25;

        // Configuration Parameter
        public static ParameterConnection pParamConnect = new ParameterConnection();
        public static ParameterAction pParamAction = new ParameterAction();

        // YoonFactory Component
        public static YoonConsoler pCLM = new YoonConsoler(30);
        public static YoonDisplayer pDLM = new YoonDisplayer(30);
        
        public static YoonServer pTCPServer = null;
        public static YoonClient pTCPClient = null;
        public static YoonParameter pConnectManager = new YoonParameter(pParamConnect, typeof(ParameterConnection));
        public static YoonParameter pActionManager = new YoonParameter(pParamAction, typeof(ParameterAction));

        // View Component
        public static List<ActionPanel> pListPanelAction = new List<ActionPanel>();

        // File Path
        public static string strParameterDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Param");
    }
}
