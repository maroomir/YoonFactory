using System.Xml.Serialization;

namespace YoonFactory
{
    public class YoonCartesianN : IYoonCartesian, IYoonCartesian<int>
    {
        public IYoonCartesian Clone()
        {
            YoonCartesianN c = new YoonCartesianN();
            c.X = X;
            c.Y = Y;
            c.Z = Z;
            c.RX = RX;
            c.RY = RY;
            c.RZ = RZ;
            return c;
        }

        public void CopyFrom(IYoonCartesian c)
        {
            if (c is YoonCartesianN cart)
            {
                X = cart.X;
                Y = cart.Y;
                Z = cart.Z;
                RX = cart.RX;
                RY = cart.RY;
                RZ = cart.RZ;
            }
        }

        [XmlIgnore]
        public int X { get; set; }
        [XmlAttribute]
        public int Y { get; set; }
        [XmlAttribute]
        public int Z { get; set; }
        [XmlIgnore]
        public int RX { get; set; }
        [XmlAttribute]
        public int RY { get; set; }
        [XmlAttribute]
        public int RZ { get; set; }

        public int[] Array
        {
            get => new int[6] { X, Y, Z, RX, RY, RZ };
            set
            {
                try
                {
                    X = value[0];
                    Y = value[1];
                    Z = value[2];
                    RX = value[3];
                    RY = value[4];
                    RZ = value[5];
                }
                catch
                {
                    //
                }
            }
        }

        public YoonCartesianN()
        {
            Unit();
        }
        public YoonCartesianN(IYoonCartesian c)
        {
            CopyFrom(c);
        }
        public YoonCartesianN(int dX, int dY, int dZ, int dRX, int dRY, int dRZ)
        {
            X = dX;
            Y = dY;
            Z = dZ;
            RX = dRX;
            RY = dRY;
            RZ = dRZ;
        }

        public void Zero()
        {
            X = Y = Z = RX = RY = RZ = 0;
        }
        public void Unit()
        {
            X = Y = Z = 1;
            RX = RY = RZ = 0;
        }
        public static YoonCartesianN operator *(YoonCartesianN c, int a)
        {
            return new YoonCartesianN(c.X * a, c.Y * a, c.Z * a, c.RX, c.RY, c.RZ);
        }
        public static YoonCartesianN operator +(YoonCartesianN c1, YoonCartesianN c2)
        {
            return new YoonCartesianN(c1.X + c2.X, c1.Y + c2.Y, c1.Z + c2.Z, c1.RX + c2.RX, c1.RY + c2.RY, c1.RZ + c2.RZ);
        }
        public static YoonCartesianN operator +(YoonCartesianN c, YoonVector2N v)
        {
            return new YoonCartesianN(c.X + v.X, c.Y + v.Y, c.Z, c.RX, c.RY, c.RZ);
        }
        public static YoonCartesianN operator -(YoonCartesianN c1, YoonCartesianN c2)
        {
            return new YoonCartesianN(c1.X - c2.X, c1.Y - c2.Y, c1.Z - c2.Z, c1.RX - c2.RX, c1.RY - c2.RY, c1.RZ - c2.RZ);
        }
        public static YoonCartesianN operator -(YoonCartesianN c, YoonVector2N v)
        {
            return new YoonCartesianN(c.X - v.X, c.Y - v.Y, c.Z, c.RX, c.RY, c.RZ);
        }
        public static YoonCartesianN operator /(YoonCartesianN c, int a)
        {
            return new YoonCartesianN(c.X / a, c.Y / a, c.Z / a, c.RX, c.RY, c.RZ);
        }
    }

    public class YoonCartesianD : IYoonCartesian, IYoonCartesian<double>
    {
        public IYoonCartesian Clone()
        {
            YoonCartesianD c = new YoonCartesianD();
            c.X = X;
            c.Y = Y;
            c.Z = Z;
            c.RX = RX;
            c.RY = RY;
            c.RZ = RZ;
            return c;
        }

        public void CopyFrom(IYoonCartesian c)
        {
            if (c is YoonCartesianD cart)
            {
                X = cart.X;
                Y = cart.Y;
                Z = cart.Z;
                RX = cart.RX;
                RY = cart.RY;
                RZ = cart.RZ;
            }
        }

        [XmlIgnore]
        public double X { get; set; }
        [XmlAttribute]
        public double Y { get; set; }
        [XmlAttribute]
        public double Z { get; set; }
        [XmlIgnore]
        public double RX { get; set; }
        [XmlAttribute]
        public double RY { get; set; }
        [XmlAttribute]
        public double RZ { get; set; }

        public double[] Array
        {
            get => new double[6] { X, Y, Z, RX, RY, RZ };
            set
            {
                try
                {
                    X = value[0];
                    Y = value[1];
                    Z = value[2];
                    RX = value[3];
                    RY = value[4];
                    RZ = value[5];
                }
                catch
                {
                    //
                }
            }
        }

        public YoonCartesianD()
        {
            Unit();
        }
        public YoonCartesianD(IYoonCartesian c)
        {
            CopyFrom(c);
        }
        public YoonCartesianD(double dX, double dY, double dZ, double dRX, double dRY, double dRZ)
        {
            X = dX;
            Y = dY;
            Z = dZ;
            RX = dRX;
            RY = dRY;
            RZ = dRZ;
        }

        public void Zero()
        {
            X = Y = Z = RX = RY = RZ = 0;
        }
        public void Unit()
        {
            X = Y = Z = 1;
            RX = RY = RZ = 0;
        }
        public static YoonCartesianD operator *(YoonCartesianD c, double a)
        {
            return new YoonCartesianD(c.X * a, c.Y * a, c.Z * a, c.RX, c.RY, c.RZ);
        }
        public static YoonCartesianD operator +(YoonCartesianD c1, YoonCartesianD c2)
        {
            return new YoonCartesianD(c1.X + c2.X, c1.Y + c2.Y, c1.Z + c2.Z, c1.RX + c2.RX, c1.RY + c2.RY, c1.RZ + c2.RZ);
        }
        public static YoonCartesianD operator +(YoonCartesianD c, YoonVector2D v)
        {
            return new YoonCartesianD(c.X + v.X, c.Y + v.Y, c.Z, c.RX, c.RY, c.RZ);
        }
        public static YoonCartesianD operator -(YoonCartesianD c1, YoonCartesianD c2)
        {
            return new YoonCartesianD(c1.X - c2.X, c1.Y - c2.Y, c1.Z - c2.Z, c1.RX - c2.RX, c1.RY - c2.RY, c1.RZ - c2.RZ);
        }
        public static YoonCartesianD operator -(YoonCartesianD c, YoonVector2D v)
        {
            return new YoonCartesianD(c.X - v.X, c.Y - v.Y, c.Z, c.RX, c.RY, c.RZ);
        }
        public static YoonCartesianD operator /(YoonCartesianD c, double a)
        {
            return new YoonCartesianD(c.X / a, c.Y / a, c.Z / a, c.RX, c.RY, c.RZ);
        }
    }

}
