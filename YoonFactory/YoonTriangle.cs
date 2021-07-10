using System.Xml.Serialization;

namespace YoonFactory
{
    public class YoonTriangle2N : IYoonTriangle, IYoonTriangle2D<int>
    {
        public IYoonTriangle Clone()
        {
            YoonTriangle2N t = new YoonTriangle2N();
            t.X = this.X;
            t.Y = this.Y;
            t.Height = this.Height;
            return t;
        }
        public void CopyFrom(IYoonTriangle t)
        {
            if (t is YoonTriangle2N trg)
            {
                X = trg.X;
                Y = trg.Y;
                Height = trg.Height;
            }
        }

        [XmlAttribute]
        public int X { get; set; }
        [XmlAttribute]
        public int Y { get; set; }
        [XmlAttribute]
        public int Height { get; set; }

        public YoonTriangle2N()
        {
            X = 0;
            Y = 0;
            Height = 0;
        }
        public YoonTriangle2N(int dx, int dy, int dh)
        {
            X = dx;
            Y = dy;
            Height = dh;
        }
        public int Area()
        {
            return X * Y / 2;
        }
    }

    public class YoonTriangle2D : IYoonTriangle, IYoonTriangle2D<double>
    {
        public IYoonTriangle Clone()
        {
            YoonTriangle2D t = new YoonTriangle2D();
            t.X = this.X;
            t.Y = this.Y;
            t.Height = this.Height;
            return t;
        }
        public void CopyFrom(IYoonTriangle t)
        {
            if (t is YoonTriangle2D trg)
            {
                X = trg.X;
                Y = trg.Y;
                Height = trg.Height;
            }
        }

        [XmlAttribute]
        public double X { get; set; }
        [XmlAttribute]
        public double Y { get; set; }
        [XmlAttribute]
        public double Height { get; set; }

        public YoonTriangle2D()
        {
            X = 0;
            Y = 0;
            Height = 0;
        }
        public YoonTriangle2D(double dx, double dy, double dh)
        {
            X = dx;
            Y = dy;
            Height = dh;
        }

        public double Area()
        {
            return 0.5 * X * Y;
        }
    }
}
