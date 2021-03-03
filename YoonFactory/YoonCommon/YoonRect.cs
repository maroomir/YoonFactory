using System.Xml.Serialization;

namespace YoonFactory
{
    /// <summary>
    /// 사각형 대응 변수 (기준좌표계 : Pixel과 동일/ 좌+상)
    /// </summary>
    public class YoonRect2N : IYoonRect, IYoonRect2D<int>
    {
        public bool Equals(IYoonRect r)
        {
            if (r is YoonRect2N rect)
            {
                if (CenterPos.X == rect.CenterPos.X &&
                    CenterPos.Y == rect.CenterPos.Y &&
                    Width == rect.Width &&
                    Height == rect.Height)
                    return true;
            }
            return false;
        }

        public IYoonRect Clone()
        {
            YoonRect2N r = new YoonRect2N();
            r.CenterPos = this.CenterPos.Clone() as YoonVector2N;
            r.Width = this.Width;
            r.Height = this.Height;
            return r;
        }
        public void CopyFrom(IYoonRect r)
        {
            if (r is YoonRect2N rect)
            {
                CenterPos = rect.CenterPos.Clone() as YoonVector2N;
                Width = rect.Width;
                Height = rect.Height;
            }
        }

        [XmlAttribute]
        public IYoonVector2D<int> CenterPos { get; set; }
        [XmlAttribute]
        public int Width { get; set; }
        [XmlAttribute]
        public int Height { get; set; }

        public int Left
        {
            get => (CenterPos as YoonVector2N).X - Width / 2;
        }

        public int Top
        {
            get => (CenterPos as YoonVector2N).Y - Height / 2;
        }

        public int Right
        {
            get => (CenterPos as YoonVector2N).X + Width / 2;
        }
        public int Bottom
        {
            get => (CenterPos as YoonVector2N).Y + Height / 2;
        }

        public IYoonVector TopLeft
        {
            get => new YoonVector2N(CenterPos.X - Width / 2, CenterPos.Y - Height / 2);
        }

        public IYoonVector TopRight
        {
            get => new YoonVector2N(CenterPos.X + Width / 2, CenterPos.Y - Height / 2);
        }

        public IYoonVector BottomLeft
        {
            get => new YoonVector2N(CenterPos.X - Width / 2, CenterPos.Y + Height / 2);
        }

        public IYoonVector BottomRight
        {
            get => new YoonVector2N(CenterPos.X + Width / 2, CenterPos.Y + Height / 2);
        }

        public YoonRect2N()
        {
            CenterPos = new YoonVector2N();
            CenterPos.X = 0;
            CenterPos.Y = 0;
            Width = 0;
            Height = 0;
        }

        public YoonRect2N(int dx, int dy, int dw, int dh)
        {
            CenterPos = new YoonVector2N();
            CenterPos.X = dx;
            CenterPos.Y = dy;
            Width = dw;
            Height = dh;
        }
        public int Area()
        {
            return Width * Height;
        }
    }

    /// <summary>
    /// 사각형 대응 변수
    /// </summary>
    public class YoonRect2D : IYoonRect, IYoonRect2D<double>
    {
        public bool Equals(IYoonRect r)
        {
            if (r is YoonRect2D rect)
            {
                if (CenterPos.X == rect.CenterPos.X &&
                    CenterPos.Y == rect.CenterPos.Y &&
                    Width == rect.Width &&
                    Height == rect.Height)
                    return true;
            }
            return false;
        }

        public IYoonRect Clone()
        {
            YoonRect2D r = new YoonRect2D();
            r.CenterPos = this.CenterPos.Clone() as YoonVector2D;
            r.Width = this.Width;
            r.Height = this.Height;
            return r;
        }
        public void CopyFrom(IYoonRect r)
        {
            if (r is YoonRect2D rect)
            {
                CenterPos = rect.CenterPos.Clone() as YoonVector2D;
                Width = rect.Width;
                Height = rect.Height;
            }
        }

        [XmlAttribute]
        public IYoonVector2D<double> CenterPos { get; set; }
        [XmlAttribute]
        public double Width { get; set; }
        [XmlAttribute]
        public double Height { get; set; }

        public double Left
        {
            get => CenterPos.X - Width / 2;
        }

        public double Top
        {
            get => CenterPos.Y - Height / 2;
        }

        public double Right
        {
            get => CenterPos.X + Width / 2;
        }

        public double Bottom
        {
            get => CenterPos.Y + Height / 2;
        }

        public IYoonVector TopLeft
        {
            get => new YoonVector2D(CenterPos.X - Width / 2, CenterPos.Y - Height / 2);
        }

        public IYoonVector TopRight
        {
            get => new YoonVector2D(CenterPos.X + Width / 2, CenterPos.Y - Height / 2);
        }

        public IYoonVector BottomLeft
        {
            get => new YoonVector2D(CenterPos.X - Width / 2, CenterPos.Y + Height / 2);
        }

        public IYoonVector BottomRight
        {
            get => new YoonVector2D(CenterPos.X + Width / 2, CenterPos.Y + Height / 2);
        }

        public YoonRect2D()
        {
            CenterPos = new YoonVector2D();
            CenterPos.X = 0;
            CenterPos.Y = 0;
            Width = 0;
            Height = 0;
        }
        public YoonRect2D(double dx, double dy, double dw, double dh)
        {
            CenterPos = new YoonVector2D();
            CenterPos.X = dx;
            CenterPos.Y = dy;
            Width = dw;
            Height = dh;
        }
        public double Area()
        {
            return Width * Height;
        }
    }

}
