using Pango;

namespace YoonFactory.Mono
{
    public static class FontFactory
    {
        public static FontDescription Default()
        {
            FontDescription pFt = Pango.FontDescription.FromString("Tahoma");
            pFt.Size = (int)(12 * Pango.Scale.PangoScale); // 12point = 12(num) * Scale(To consider ppi)
            pFt.Style = Pango.Style.Normal;
            pFt.Weight = Weight.Light;
            return pFt;
        }

        public static FontDescription Comment()
        {
            FontDescription pFt = Pango.FontDescription.FromString("Tahoma");
            pFt.Size = (int)(11 * Pango.Scale.PangoScale); // 11point = 11(num) * Scale(To consider ppi)
            pFt.Style = Pango.Style.Normal;
            pFt.Weight = Weight.Light;
            return pFt;
        }

        public static FontDescription Emphasis()
        {
            FontDescription pFt = Pango.FontDescription.FromString("Tahoma");
            pFt.Size = (int)(14 * Pango.Scale.PangoScale); // 12point = 12(num) * Scale(To consider ppi)
            pFt.Style = Pango.Style.Normal;
            pFt.Weight = Weight.Semibold;
            return pFt;
        }

        public static FontDescription StrapLine()
        {
            FontDescription pFt = Pango.FontDescription.FromString("Centarell");
            pFt.Size = (int)(12 * Pango.Scale.PangoScale); // 12point = 12(num) * Scale(To consider ppi)
            pFt.Style = Pango.Style.Italic;
            pFt.Weight = Weight.Semibold;
            return pFt;
        }

        public static FontDescription SubTitle()
        {
            FontDescription pFt = Pango.FontDescription.FromString("Centarell");
            pFt.Size = (int)(16 * Pango.Scale.PangoScale); // 12point = 12(num) * Scale(To consider ppi)
            pFt.Style = Pango.Style.Italic;
            pFt.Weight = Weight.Semibold;
            return pFt;
        }

        public static FontDescription Title()
        {
            FontDescription pFt = Pango.FontDescription.FromString("Centarell");
            pFt.Size = (int)(24 * Pango.Scale.PangoScale); // 12point = 12(num) * Scale(To consider ppi)
            pFt.Style = Pango.Style.Italic;
            pFt.Weight = Weight.Bold;
            return pFt;
        }
    }
}
