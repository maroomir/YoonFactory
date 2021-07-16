using YoonFactory.Camera;
using YoonFactory.Image;
using YoonFactory.Log;

namespace YoonSample.ImageViewer
{
    public static class CommonClass
    {
        // Initialize YoonFactory
        public static YoonConsoler pCLM = new YoonConsoler(30);
        public static YoonImage pImage = new YoonImage();
        public static IYoonCamera pCamera = null;
    }
}
