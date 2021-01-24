using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using YoonFactory.Cognex;
using YoonFactory.Log;
using YoonFactory.Windows.Log;
using YoonFactory.Param;

namespace YoonSample.CognexInspector
{
    public enum eParamInspect
    {
        Preprocessing,
        PatternMatching,
        ObjectExtract,
        Combine,
    }

    public delegate void PassImageCallback(object sender, CogImageArgs e);
    public class CogImageArgs : EventArgs
    {
        public int InspectNo;
        public eTypeInspect InspectType;
        public ICogImage CogImage;

        public CogImageArgs(ICogImage pImage)
        {
            InspectNo = 0;
            InspectType = eTypeInspect.None;
            CogImage = pImage;
        }

        public CogImageArgs(eTypeInspect nType, ICogImage pImage)
        {
            InspectNo = 0;
            InspectType = nType;
            CogImage = pImage;
        }

        public CogImageArgs(int nNo, eTypeInspect nType, ICogImage pImage)
        {
            InspectNo = nNo;
            InspectType = nType;
            CogImage = pImage;
        }
    }

    public delegate void PassCogToolCallback(object sender, CogToolArgs e);
    public class CogToolArgs : EventArgs
    {
        public eYoonCognexType ToolType;
        public eLabelInspect Label;
        public ICogTool CogTool;
        public ICogImage ContainImage;

        public CogToolArgs(eYoonCognexType nType, eLabelInspect nLabel, ICogTool pTool, ICogImage pImage)
        {
            ToolType = nType;
            Label = nLabel;
            CogTool = pTool;
            if (pImage != null)
                ContainImage = pImage.CopyBase(CogImageCopyModeConstants.CopyPixels);
        }
    }

    public static class CommonClass
    {
        public const int DEFULAT_IMAGE_WIDTH = 640;
        public const int DEFULAT_IMAGE_HEIGHT = 480;
        public const int MAX_SOURCE_NUM = 4;
        public const int MAX_PATTERN_NUM = 4;

        public static YoonContainer<eParamInspect> pParamContainer = new YoonContainer<eParamInspect>();
        public static ToolContainer pCogToolContainer = new ToolContainer();
        public static ResultContainer pCogResultContainer = new ResultContainer();
        public static YoonConsoler pCLM;
        public static YoonDisplayer pDLM;

        // File Paths
        public static string strWorkDirectory = Directory.GetCurrentDirectory();
        public static string strImageDirectory = Path.Combine(strWorkDirectory, @"Image");
        public static string strParamDirectory = Path.Combine(strWorkDirectory, @"Parameter");
        public static string strResultDirectory = Path.Combine(strWorkDirectory, @"Result");
        public static string strCurrentWorkingDirectory = Path.Combine(strWorkDirectory, @"Current");
    }
}