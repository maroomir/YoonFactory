using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using YoonFactory.Cognex;

namespace YoonSample.CognexInspector
{
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
}
