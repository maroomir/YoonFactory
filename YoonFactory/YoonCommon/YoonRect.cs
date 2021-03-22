using System;
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
                CenterPos = (pos1 + pos2) / 2;
                Width = pos2.X - pos1.X;
                Height = pos2.Y - pos1.Y;
            }
            else if (dir1 == eYoonDir2D.BottomRight && dir2 == eYoonDir2D.TopLeft)
            {
                CenterPos = (pos1 + pos2) / 2;
                Width = pos1.X - pos2.X;
                Height = pos1.Y - pos2.Y;
            }
            if (dir1 == eYoonDir2D.TopRight && dir2 == eYoonDir2D.BottomLeft)
            {
                CenterPos = (pos1 + pos2) / 2;
                Width = pos2.X - pos1.X;
                Height = pos1.Y - pos2.Y;
            }
            else if (dir1 == eYoonDir2D.BottomLeft && dir2 == eYoonDir2D.TopRight)
            {
                CenterPos = (pos1 + pos2) / 2;
                Width = pos1.X - pos2.X;
                Height = pos2.Y - pos1.Y;
            }
            else
                throw new ArgumentException("[YOONCOMMON] Direction Argument is not correct");
        }

        public YoonRect2N(params YoonVector2N[] pArgs)
        {
            if (pArgs.Length >= 2)
            {
                int nInitLeft = (pArgs[0].X > pArgs[1].X) ? pArgs[1].X : pArgs[0].X;
                int nInitRight = (pArgs[0].X < pArgs[1].X) ? pArgs[1].X : pArgs[0].X;
                int nInitTop = (pArgs[0].Y > pArgs[1].Y) ? pArgs[1].Y : pArgs[0].Y;
                int nInitBottom = (pArgs[0].Y < pArgs[1].Y) ? pArgs[1].Y : pArgs[0].Y;
                YoonVector2N pPosTopLeft = new YoonVector2N(nInitLeft, nInitTop);
                YoonVector2N pPosBottomRight = new YoonVector2N(nInitRight, nInitBottom);
                for (int iParam = 2; iParam < pArgs.Length; iParam++)
                {
                    eYoonDir2D nDirDiffTopLeft = pPosTopLeft.DirectionTo(pArgs[iParam]);
                    eYoonDir2D nDirDiffBottomRight = pPosBottomRight.DirectionTo(pArgs[iParam]);
                    switch (nDirDiffTopLeft)
                    {
                        case eYoonDir2D.TopLeft:
                            pPosTopLeft.X = pArgs[iParam].X;
                            pPosTopLeft.Y = pArgs[iParam].Y;
                            break;
                        case eYoonDir2D.Top:
                            pPosTopLeft.Y = pArgs[iParam].Y;
                            break;
                        case eYoonDir2D.TopRight:
                            pPosTopLeft.Y = pArgs[iParam].Y;
                            break;
                        case eYoonDir2D.Left:
                            pPosTopLeft.X = pArgs[iParam].X;
                            break;
                        case eYoonDir2D.BottomLeft:
                            pPosTopLeft.X = pArgs[iParam].X;
                            break;
                        default:
                            break;
                    }
                    switch (nDirDiffBottomRight)
                    {
                        case eYoonDir2D.TopRight:
                            pPosBottomRight.X = pArgs[iParam].X;
                            break;
                        case eYoonDir2D.Right:
                            pPosBottomRight.X = pArgs[iParam].X;
                            break;
                        case eYoonDir2D.BottomLeft:
                            pPosBottomRight.Y = pArgs[iParam].Y;
                            break;
                        case eYoonDir2D.Bottom:
                            pPosBottomRight.Y = pArgs[iParam].Y;
                            break;
                        case eYoonDir2D.BottomRight:
                            pPosBottomRight.X = pArgs[iParam].X;
                            pPosBottomRight.Y = pArgs[iParam].Y;
                            break;
                        default:
                            break;
                    }
                }
                CenterPos = (pPosTopLeft + pPosBottomRight) / 2;
                Width = (pPosBottomRight - pPosTopLeft).X;
                Height = (pPosBottomRight - pPosTopLeft).Y;
            }
        }

        public bool IsContain(IYoonVector vec)
        {
            if (vec is YoonVector2D pPos)
            {
                if (Left < pPos.X && pPos.X < Right &&
                    Top < pPos.Y && pPos.Y < Bottom)
                    return true;
            }
            return false;
        }

        public double Area()
        {
            return Width * Height;
        }

        public static YoonRect2N operator +(YoonRect2N r1, YoonRect2N r2)
        {
            int nTop = Math.Min(r1.Top, r2.Top);
            int nBottom = Math.Max(r1.Bottom, r2.Bottom);
            int nLeft = Math.Min(r1.Left, r2.Left);
            int nRight = Math.Max(r1.Right, r2.Right);
            return new YoonRect2N((nLeft + nRight) / 2, (nTop + nBottom) / 2, nRight - nLeft, nBottom - nTop);
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

        public YoonRect2D(eYoonDir2D dir1, YoonVector2D pos1, eYoonDir2D dir2, YoonVector2D pos2)
        {
            if (dir1 == eYoonDir2D.TopLeft && dir2 == eYoonDir2D.BottomRight)
            {
                CenterPos = (pos1 + pos2) / 2;
                Width = pos2.X - pos1.X;
                Height = pos2.Y - pos1.Y;
            }
            else if (dir1 == eYoonDir2D.BottomRight && dir2 == eYoonDir2D.TopLeft)
            {
                CenterPos = (pos1 + pos2) / 2;
                Width = pos1.X - pos2.X;
                Height = pos1.Y - pos2.Y;
            }
            if (dir1 == eYoonDir2D.TopRight && dir2 == eYoonDir2D.BottomLeft)
            {
                CenterPos = (pos1 + pos2) / 2;
                Width = pos2.X - pos1.X;
                Height = pos1.Y - pos2.Y;
            }
            else if (dir1 == eYoonDir2D.BottomLeft && dir2 == eYoonDir2D.TopRight)
            {
                CenterPos = (pos1 + pos2) / 2;
                Width = pos1.X - pos2.X;
                Height = pos2.Y - pos1.Y;
            }
            else
                throw new ArgumentException("[YOONCOMMON] Direction Argument is not correct");
        }

        public YoonRect2D(params YoonVector2D[] pArgs)
        {
            if(pArgs.Length >= 2)
            {
                double dInitLeft = (pArgs[0].X > pArgs[1].X) ? pArgs[1].X : pArgs[0].X;
                double dInitRight = (pArgs[0].X < pArgs[1].X) ? pArgs[1].X : pArgs[0].X;
                double dInitTop = (pArgs[0].Y > pArgs[1].Y) ? pArgs[1].Y : pArgs[0].Y;
                double dInitBottom = (pArgs[0].Y < pArgs[1].Y) ? pArgs[1].Y : pArgs[0].Y;
                YoonVector2D pPosTopLeft = new YoonVector2D(dInitLeft, dInitTop);
                YoonVector2D pPosBottomRight = new YoonVector2D(dInitRight, dInitBottom);
                for (int iParam = 2; iParam < pArgs.Length; iParam++)
                {
                    eYoonDir2D nDirDiffTopLeft = pPosTopLeft.DirectionTo(pArgs[iParam]);
                    eYoonDir2D nDirDiffBottomRight = pPosBottomRight.DirectionTo(pArgs[iParam]);
                    switch(nDirDiffTopLeft)
                    {
                        case eYoonDir2D.TopLeft:
                            pPosTopLeft.X = pArgs[iParam].X;
                            pPosTopLeft.Y = pArgs[iParam].Y;
                            break;
                        case eYoonDir2D.Top:
                            pPosTopLeft.Y = pArgs[iParam].Y;
                            break;
                        case eYoonDir2D.TopRight:
                            pPosTopLeft.Y = pArgs[iParam].Y;
                            break;
                        case eYoonDir2D.Left:
                            pPosTopLeft.X = pArgs[iParam].X;
                            break;
                        case eYoonDir2D.BottomLeft:
                            pPosTopLeft.X = pArgs[iParam].X;
                            break;
                        default:
                            break;
                    }
                    switch(nDirDiffBottomRight)
                    {
                        case eYoonDir2D.TopRight:
                            pPosBottomRight.X = pArgs[iParam].X;
                            break;
                        case eYoonDir2D.Right:
                            pPosBottomRight.X = pArgs[iParam].X;
                            break;
                        case eYoonDir2D.BottomLeft:
                            pPosBottomRight.Y = pArgs[iParam].Y;
                            break;
                        case eYoonDir2D.Bottom:
                            pPosBottomRight.Y = pArgs[iParam].Y;
                            break;
                        case eYoonDir2D.BottomRight:
                            pPosBottomRight.X = pArgs[iParam].X;
                            pPosBottomRight.Y = pArgs[iParam].Y;
                            break;
                        default:
                            break;
                    }
                }
                CenterPos = (pPosTopLeft + pPosBottomRight) / 2;
                Width = (pPosBottomRight - pPosTopLeft).X;
                Height = (pPosBottomRight - pPosTopLeft).Y;
            }
        }

        public bool IsContain(IYoonVector vec)
        {
            if (vec is YoonVector2D pPos)
            {
                if (Left < pPos.X && pPos.X < Right &&
                    Top < pPos.Y && pPos.Y < Bottom)
                    return true;
            }
            return false;
        }

        public double Area()
        {
            return Width * Height;
        }

        public static YoonRect2D operator +(YoonRect2D r1, YoonRect2D r2)
        {
            double nTop = Math.Min(r1.Top, r2.Top);
            double nBottom = Math.Max(r1.Bottom, r2.Bottom);
            double nLeft = Math.Min(r1.Left, r2.Left);
            double nRight = Math.Max(r1.Right, r2.Right);
            return new YoonRect2D((nLeft + nRight) / 2, (nTop + nBottom) / 2, nRight - nLeft, nBottom - nTop);
        }
    }

}
