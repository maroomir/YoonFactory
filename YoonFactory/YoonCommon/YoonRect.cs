using System;
using System.Xml.Serialization;
using System.Drawing;

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

        public Rectangle ToRectangle
        {
            get => new Rectangle(Left, Top, Width, Height);
        }

        public RectangleF ToRectangleF
        {
            get => new RectangleF(Left, Top, Width, Height);
        }

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

        public IYoonVector2D<int> TopLeft
        {
            get => new YoonVector2N(CenterPos.X - Width / 2, CenterPos.Y - Height / 2);
        }

        public IYoonVector2D<int> TopRight
        {
            get => new YoonVector2N(CenterPos.X + Width / 2, CenterPos.Y - Height / 2);
        }

        public IYoonVector2D<int> BottomLeft
        {
            get => new YoonVector2N(CenterPos.X - Width / 2, CenterPos.Y + Height / 2);
        }

        public IYoonVector2D<int> BottomRight
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
        public YoonRect2N(YoonVector2N pos, int dw, int dh)
        {
            CenterPos = new YoonVector2N();
            CenterPos.X = pos.X;
            CenterPos.Y = pos.Y;
            Width = dw;
            Height = dh;
        }

        public YoonRect2N(int dx, int dy, int dw, int dh)
        {
            CenterPos = new YoonVector2N();
            CenterPos.X = dx;
            CenterPos.Y = dy;
            Width = dw;
            Height = dh;
        }

        public YoonRect2N(eYoonDir2D dir1, YoonVector2N pos1, eYoonDir2D dir2, YoonVector2N pos2)
        {
            if (dir1 == eYoonDir2D.TopLeft && dir2 == eYoonDir2D.BottomRight)
            {
                CenterPos = new YoonVector2N();
                CenterPos.X = (pos2.X - pos1.X) / 2;
                CenterPos.Y = (pos2.Y - pos1.Y) / 2;
                Width = pos2.X - pos1.X;
                Height = pos2.Y - pos1.Y;
            }
            else if (dir1 == eYoonDir2D.BottomRight && dir2 == eYoonDir2D.TopLeft)
            {
                CenterPos = new YoonVector2N();
                CenterPos.X = (pos1.X - pos2.X) / 2;
                CenterPos.Y = (pos1.Y - pos2.Y) / 2;
                Width = pos1.X - pos2.X;
                Height = pos1.Y - pos2.Y;
            }
            if (dir1 == eYoonDir2D.TopRight && dir2 == eYoonDir2D.BottomLeft)
            {
                CenterPos = new YoonVector2N();
                CenterPos.X = (pos2.X - pos1.X) / 2;
                CenterPos.Y = (pos1.Y - pos2.Y) / 2;
                Width = pos2.X - pos1.X;
                Height = pos1.Y - pos2.Y;
            }
            else if (dir1 == eYoonDir2D.BottomLeft && dir2 == eYoonDir2D.TopRight)
            {
                CenterPos = new YoonVector2N();
                CenterPos.X = (pos1.X - pos2.X) / 2;
                CenterPos.Y = (pos2.Y - pos1.Y) / 2;
                Width = pos1.X - pos2.X;
                Height = pos2.Y - pos1.Y;
            }
            else
                throw new ArgumentException("[YOONCOMMON] Direction Argument is not correct");
        }

        public YoonRect2N(YoonVector2N pos1, YoonVector2N pos2)
        {
            CenterPos = new YoonVector2N();
            int posLeft = (pos1.X > pos2.X) ? pos2.X : pos1.X;
            int posRight = (pos1.X < pos2.X) ? pos2.X : pos1.X;
            int posTop = (pos1.Y > pos2.Y) ? pos2.Y : pos1.Y;
            int posBottom = (pos1.Y < pos2.Y) ? pos2.Y : pos1.Y;
            CenterPos.X = (posRight - posLeft) / 2;
            CenterPos.Y = (posBottom - posTop) / 2;
            Width = posRight - posLeft;
            Height = posBottom - posTop;
        }

        public YoonRect2N(Rectangle pRect)
        {
            CenterPos = new YoonVector2N();
            CenterPos.X = (pRect.Right - pRect.Left) / 2;
            CenterPos.Y = (pRect.Bottom - pRect.Top) / 2;
            Width = pRect.Width;
            Height = pRect.Height;
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

        public Rectangle ToRectangle
        {
            get => new Rectangle((int)Left, (int)Top, (int)Width, (int)Height);
        }

        public RectangleF ToRectangleF
        {
            get => new RectangleF((float)Left, (float)Top, (float)Width, (float)Height);
        }

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

        public IYoonVector2D<double> TopLeft
        {
            get => new YoonVector2D(CenterPos.X - Width / 2, CenterPos.Y - Height / 2);
        }

        public IYoonVector2D<double> TopRight
        {
            get => new YoonVector2D(CenterPos.X + Width / 2, CenterPos.Y - Height / 2);
        }

        public IYoonVector2D<double> BottomLeft
        {
            get => new YoonVector2D(CenterPos.X - Width / 2, CenterPos.Y + Height / 2);
        }

        public IYoonVector2D<double> BottomRight
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

        public YoonRect2D(YoonVector2D pos, double dw, double dh)
        {
            CenterPos = new YoonVector2D();
            CenterPos.X = pos.X;
            CenterPos.Y = pos.Y;
            Width = dw;
            Height = dh;
        }

        public YoonRect2D(double dx, double dy, double dw, double dh)
        {
            CenterPos = new YoonVector2D();
            CenterPos.X = dx;
            CenterPos.Y = dy;
            Width = dw;
            Height = dh;
        }

        public YoonRect2D(RectangleF pRect)
        {
            CenterPos = new YoonVector2D();
            CenterPos.X = (pRect.Right - pRect.Left) / 2;
            CenterPos.Y = (pRect.Bottom - pRect.Top) / 2;
            Width = pRect.Width;
            Height = pRect.Height;
        }

        public YoonRect2D(eYoonDir2D dir1, YoonVector2D pos1, eYoonDir2D dir2, YoonVector2D pos2)
        {
            if (dir1 == eYoonDir2D.TopLeft && dir2 == eYoonDir2D.BottomRight)
            {
                CenterPos = new YoonVector2D();
                CenterPos.X = (pos2.X - pos1.X) / 2;
                CenterPos.Y = (pos2.Y - pos1.Y) / 2;
                Width = pos2.X - pos1.X;
                Height = pos2.Y - pos1.Y;
            }
            else if (dir1 == eYoonDir2D.BottomRight && dir2 == eYoonDir2D.TopLeft)
            {
                CenterPos = new YoonVector2D();
                CenterPos.X = (pos1.X - pos2.X) / 2;
                CenterPos.Y = (pos1.Y - pos2.Y) / 2;
                Width = pos1.X - pos2.X;
                Height = pos1.Y - pos2.Y;
            }
            if (dir1 == eYoonDir2D.TopRight && dir2 == eYoonDir2D.BottomLeft)
            {
                CenterPos = new YoonVector2D();
                CenterPos.X = (pos2.X - pos1.X) / 2;
                CenterPos.Y = (pos1.Y - pos2.Y) / 2;
                Width = pos2.X - pos1.X;
                Height = pos1.Y - pos2.Y;
            }
            else if (dir1 == eYoonDir2D.BottomLeft && dir2 == eYoonDir2D.TopRight)
            {
                CenterPos = new YoonVector2D();
                CenterPos.X = (pos1.X - pos2.X) / 2;
                CenterPos.Y = (pos2.Y - pos1.Y) / 2;
                Width = pos1.X - pos2.X;
                Height = pos2.Y - pos1.Y;
            }
            else
                throw new ArgumentException("[YOONCOMMON] Direction Argument is not correct");
        }

        public YoonRect2D(YoonVector2D pos1, YoonVector2D pos2)
        {
            CenterPos = new YoonVector2D();
            double posLeft = (pos1.X > pos2.X) ? pos2.X : pos1.X;
            double posRight = (pos1.X < pos2.X) ? pos2.X : pos1.X;
            double posTop = (pos1.Y > pos2.Y) ? pos2.Y : pos1.Y;
            double posBottom = (pos1.Y < pos2.Y) ? pos2.Y : pos1.Y;
            CenterPos.X = (posRight - posLeft) / 2;
            CenterPos.Y = (posBottom - posTop) / 2;
            Width = posRight - posLeft;
            Height = posBottom - posTop;
        }

        public double Area()
        {
            return Width * Height;
        }
    }

}
