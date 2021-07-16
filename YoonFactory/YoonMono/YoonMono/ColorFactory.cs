using Gdk;

namespace YoonFactory.Mono
{
    public static class ColorFactory
    {
        public static Color Black = new Color(0, 0, 0);
        public static Color White = new Color(255, 255, 255);
        public static Color Gray = new Color(128, 128, 128);
        public static Color Red = new Color(255, 0, 0);
        public static Color Salmon = new Color(250, 128, 114);
        public static Color Blue = new Color(0, 0, 255);
        public static Color Green = new Color(0, 255, 0);
        public static Color Lime = new Color(191, 255, 0);
        public static Color Yellow = new Color(255, 255, 0);
        public static Color Purple = new Color(128, 0, 128);
        public static Color Lavender = new Color(230, 230, 250);
        public static Color RedPurple = new Color(135, 27, 77);
        public static Color Magenta = new Color(255, 0, 255);
        public static Color Violate = new Color(200, 0, 128);
        public static Color Khaki = new Color(195, 176, 145);
        public static Color DarkKhaki = new Color(189, 183, 107);

        public static Color Convert(System.Drawing.Color pColor)
        {
            return new Color(pColor.R, pColor.G, pColor.B);
        }

        public static Color Convert(byte nR, byte nG, byte nB)
        {
            return new Color(nR, nG, nB);
        }
    }
}