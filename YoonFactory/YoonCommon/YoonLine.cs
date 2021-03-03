using System.Xml.Serialization;
using System;

namespace YoonFactory
{
    public class YoonLine2N : IYoonLine, IYoonLine2D<int>
    {
        [XmlAnyAttribute]
        public IYoonVector2D<int> StartPos { get; set; }
        [XmlAnyAttribute]
        public IYoonVector2D<int> EndPos { get; set; }

        public IYoonVector2D<int> CenterPos
        {
            get => new YoonVector2N((StartPos.X + EndPos.X) / 2, (StartPos.Y + EndPos.Y) / 2);
        }

        public bool Equals(IYoonLine pLine)
        {
            if (pLine is YoonLine2N pLine2N)
            {
                if (StartPos.X == pLine2N.StartPos.X &&
                    StartPos.Y == pLine2N.StartPos.Y &&
                    StartPos.W == pLine2N.StartPos.W &&
                    EndPos.X == pLine2N.EndPos.X &&
                    EndPos.Y == pLine2N.EndPos.Y &&
                    EndPos.W == pLine2N.EndPos.W)
                    return true;
            }
            return false;
        }

        public IYoonLine Clone()
        {
            YoonLine2N pLine = new YoonLine2N();
            pLine.StartPos = StartPos.Clone() as YoonVector2N;
            pLine.EndPos = EndPos.Clone() as YoonVector2N;
            return pLine;
        }

        public void CopyFrom(IYoonLine pLine)
        {
            if(pLine is YoonLine2N pLine2N)
            {
                StartPos = pLine2N.StartPos.Clone() as YoonVector2N;
                EndPos = pLine2N.EndPos.Clone() as YoonVector2N;
            }
        }

        public YoonLine2N()
        {
            StartPos = new YoonVector2N();
            EndPos = new YoonVector2N();
            StartPos.X = 0;
            StartPos.Y = 0;
            EndPos.X = 0;
            EndPos.Y = 0;
        }

        public YoonLine2N(int startX, int startY, int endX, int endY)
        {
            StartPos = new YoonVector2N(startX, startY);
            EndPos = new YoonVector2N(endX, endY);
        }

        public YoonLine2N(YoonVector2N pVecStart, YoonVector2N pVecEnd)
        {
            StartPos = pVecStart.Clone() as YoonVector2N;
            EndPos = pVecEnd.Clone() as YoonVector2N;
        }

        public int Length()
        {
            return (int)Math.Sqrt(Math.Pow(StartPos.X - EndPos.X, 2.0) + Math.Pow(StartPos.Y - EndPos.Y, 2.0));
        }
    }

    public class YoonLine2D : IYoonLine, IYoonLine2D<double>
    {
        [XmlAnyAttribute]
        public IYoonVector2D<double> StartPos { get; set; }
        [XmlAnyAttribute]
        public IYoonVector2D<double> EndPos { get; set; }

        public IYoonVector2D<double> CenterPos
        {
            get => new YoonVector2D((StartPos.X + EndPos.X) / 2, (StartPos.Y + EndPos.Y) / 2);
        }

        public bool Equals(IYoonLine pLine)
        {
            if (pLine is YoonLine2D pLine2D)
            {
                if (StartPos.X == pLine2D.StartPos.X &&
                    StartPos.Y == pLine2D.StartPos.Y &&
                    StartPos.W == pLine2D.StartPos.W &&
                    EndPos.X == pLine2D.EndPos.X &&
                    EndPos.Y == pLine2D.EndPos.Y &&
                    EndPos.W == pLine2D.EndPos.W)
                    return true;
            }
            return false;
        }

        public IYoonLine Clone()
        {
            YoonLine2D pLine = new YoonLine2D();
            pLine.StartPos = StartPos.Clone() as YoonVector2D;
            pLine.EndPos = EndPos.Clone() as YoonVector2D;
            return pLine;
        }

        public void CopyFrom(IYoonLine line)
        {
            if (line is YoonLine2D pLine)
            {
                StartPos = pLine.StartPos.Clone() as YoonVector2D;
                EndPos = pLine.EndPos.Clone() as YoonVector2D;
            }
        }

        public YoonLine2D()
        {
            StartPos = new YoonVector2D();
            EndPos = new YoonVector2D();
            StartPos.X = 0.0;
            StartPos.Y = 0.0;
            EndPos.X = 0.0;
            EndPos.Y = 0.0;
        }

        public YoonLine2D(double startX, double startY, double endX, double endY)
        {
            StartPos = new YoonVector2D(startX, startY);
            EndPos = new YoonVector2D(endX, endY);
        }

        public YoonLine2D(YoonVector2D pVecStart, YoonVector2D pVecEnd)
        {
            StartPos = pVecStart.Clone() as YoonVector2D;
            EndPos = pVecEnd.Clone() as YoonVector2D;
        }

        public double Length()
        {
            return Math.Sqrt(Math.Pow(StartPos.X - EndPos.X, 2.0) + Math.Pow(StartPos.Y - EndPos.Y, 2.0));
        }
    }
}
