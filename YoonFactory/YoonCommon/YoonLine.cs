﻿using System.Xml.Serialization;
using System;

namespace YoonFactory
{
    public class YoonLine2N : IYoonLine, IYoonLine2D<int>
    {
        [XmlAnyAttribute]
        public IYoonVector2D<int> StartPos { get; private set; } = new YoonVector2N(0, 0);
        [XmlAnyAttribute]
        public IYoonVector2D<int> EndPos { get; private set; } = new YoonVector2N(0, 0);

        public IYoonVector2D<int> CenterPos => new YoonVector2N((StartPos.X + EndPos.X) / 2, (StartPos.Y + EndPos.Y) / 2);

        public double Length => Math.Sqrt(Math.Pow(StartPos.X - EndPos.X, 2.0) + Math.Pow(StartPos.Y - EndPos.Y, 2.0));

        private double m_dSlope = 0.0;
        private double m_dIntercept = 0.0;

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
            //
        }

        public YoonLine2N(int startX, int startY, int endX, int endY)
        {
            StartPos = new YoonVector2N(startX, startY);
            EndPos = new YoonVector2N(endX, endY);
            m_dSlope = (endY - startY) / (endX - startX);
            m_dIntercept = startY - m_dSlope * startX;
        }

        public YoonLine2N(double dSlope, double dIntercept)
        {
            m_dSlope = dSlope;
            m_dIntercept = dIntercept;
            StartPos = new YoonVector2N(-1, Y(1));
            EndPos = new YoonVector2N(1, Y(1));
        }

        public YoonLine2N(params YoonVector2N[] args)
        {
            if (args.Length >= 2 || args.Length < 10)
            {
                int[] pX = new int[args.Length];
                int[] pY = new int[args.Length];
                int nMinX = 65535;
                int nMaxX = -65535;
                for (int iVector = 0; iVector < args.Length; iVector++)
                {
                    pX[iVector] = args[iVector].X;
                    pY[iVector] = args[iVector].Y;
                    if (nMinX > pX[iVector]) nMinX = pX[iVector];
                    if (nMaxX < pX[iVector]) nMaxX = pX[iVector];
                }
                if (!MathFactory.LeastSquare(ref m_dSlope, ref m_dIntercept, args.Length, pX, pY))
                    return;
                StartPos = new YoonVector2N(nMinX, Y(nMinX));
                EndPos = new YoonVector2N(nMaxX, Y(nMaxX));
                //// Slope, Intercept Recalculating - Only use Integer Form
                m_dSlope = (EndPos.Y - StartPos.Y) / (EndPos.X - StartPos.X);
                m_dIntercept = StartPos.Y - m_dSlope * StartPos.X;
            }
        }

        public int X(int nY)
        {
            return (int)((nY - m_dIntercept) / m_dSlope);
        }

        public int Y(int nX)
        {
            return (int)(nX * m_dSlope + m_dIntercept);
        }

        public bool IsContain(IYoonVector pVector)
        {
            switch(pVector)
            {
                case YoonVector2N pVector2N:
                    return pVector2N.Y == pVector2N.X * m_dSlope + m_dIntercept;
                case YoonVector2D pVector2D:
                    return pVector2D.Y == pVector2D.X * m_dSlope + m_dIntercept;
                default:
                    return false;
            }
        }

        public double Distance(IYoonVector pVector)
        {
            switch (pVector)
            {
                case YoonVector2N pVector2N:
                    return Math.Abs(m_dSlope * pVector2N.X - pVector2N.Y + m_dIntercept) / Math.Sqrt(m_dSlope * m_dSlope + 1);
                case YoonVector2D pVector2D:
                    return Math.Abs(m_dSlope * pVector2D.X - pVector2D.Y + m_dIntercept) / Math.Sqrt(m_dSlope * m_dSlope + 1);
                default:
                    throw new ArgumentException("[YOONCOMMON] Vector argument format is not correct");
            }
        }
    }

    public class YoonLine2D : IYoonLine, IYoonLine2D<double>
    {
        [XmlAnyAttribute]
        public IYoonVector2D<double> StartPos { get; private set; } = new YoonVector2D(0, 0);
        [XmlAnyAttribute]
        public IYoonVector2D<double> EndPos { get; private set; } = new YoonVector2D(0, 0);

        public IYoonVector2D<double> CenterPos => new YoonVector2D((StartPos.X + EndPos.X) / 2, (StartPos.Y + EndPos.Y) / 2);

        public double Length => Math.Sqrt(Math.Pow(StartPos.X - EndPos.X, 2.0) + Math.Pow(StartPos.Y - EndPos.Y, 2.0));

        private double m_dSlope = 0.0;
        private double m_dIntercept = 0.0;

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
            //
        }

        public YoonLine2D(double startX, double startY, double endX, double endY)
        {
            StartPos = new YoonVector2D(startX, startY);
            EndPos = new YoonVector2D(endX, endY);
            m_dSlope = (endY - startY) / (endX - startX);
            m_dIntercept = startY - m_dSlope * startX;
        }

        public YoonLine2D(double dSlope, double dIntercept)
        {
            m_dSlope = dSlope;
            m_dIntercept = dIntercept;
            StartPos = new YoonVector2D(-1, Y(1));
            EndPos = new YoonVector2D(1, Y(1));
        }

        public YoonLine2D(params YoonVector2D[] args)
        {
            if (args.Length >= 2 || args.Length < 10)
            {
                double[] pX = new double[args.Length];
                double[] pY = new double[args.Length];
                double dMinX = 65535;
                double dMaxX = -65535;
                for (int iVector = 0; iVector < args.Length; iVector++)
                {
                    pX[iVector] = args[iVector].X;
                    pY[iVector] = args[iVector].Y;
                    if (dMinX > pX[iVector]) dMinX = pX[iVector];
                    if (dMaxX < pX[iVector]) dMaxX = pX[iVector];
                }
                if (!MathFactory.LeastSquare(ref m_dSlope, ref m_dIntercept, args.Length, pX, pY))
                    return;
                StartPos = new YoonVector2D(dMinX, Y(dMinX));
                EndPos = new YoonVector2D(dMaxX, Y(dMaxX));
            }
        }

        public double X(double dY)
        {
            return (dY - m_dIntercept) / m_dSlope;
        }

        public double Y(double dX)
        {
            return dX * m_dSlope + m_dIntercept;
        }

        public bool IsContain(IYoonVector pVector)
        {
            switch (pVector)
            {
                case YoonVector2N pVector2N:
                    return pVector2N.Y == pVector2N.X * m_dSlope + m_dIntercept;
                case YoonVector2D pVector2D:
                    return pVector2D.Y == pVector2D.X * m_dSlope + m_dIntercept;
                default:
                    return false;
            }
        }

        public double Distance(IYoonVector pVector)
        {
            switch (pVector)
            {
                case YoonVector2N pVector2N:
                    return Math.Abs(m_dSlope * pVector2N.X - pVector2N.Y + m_dIntercept) / Math.Sqrt(m_dSlope * m_dSlope + 1);
                case YoonVector2D pVector2D:
                    return Math.Abs(m_dSlope * pVector2D.X - pVector2D.Y + m_dIntercept) / Math.Sqrt(m_dSlope * m_dSlope + 1);
                default:
                    throw new ArgumentException("[YOONCOMMON] Vector argument format is not correct");
            }
        }
    }
}
