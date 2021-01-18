using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoonFactory.Log;
using YoonFactory.Camera;

namespace YoonSample.ImageViewer
{
    public static class CommonClass
    {
        // Initialize YoonFactory
        public static YoonConsoler pCLM = new YoonConsoler(30);
        public static IYoonCamera pCamera = null;
    }
}
