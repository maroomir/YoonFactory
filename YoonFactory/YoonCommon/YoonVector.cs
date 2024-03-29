﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace YoonFactory
{
    /// <summary>
    /// 2차원 동차변환 벡터
    /// </summary>
    public class YoonVector2N : IYoonVector, IYoonVector2D<int>, IEquatable<YoonVector2N>
    {
        public int Count { get; } = 3;

        public eYoonDir2D Direction
        {
            get
            {
                if (X == 0 && Y == 0)
                    return eYoonDir2D.Center;
                else if (X == 0 && Y > 0)
                    return eYoonDir2D.Top;
                else if (X == 0 && Y < 0)
                    return eYoonDir2D.Bottom;
                else if (X > 0 && Y == 0)
                    return eYoonDir2D.Right;
                else if (X < 0 && Y == 0)
                    return eYoonDir2D.Left;
                else if (X > 0 && Y > 0)
                    return eYoonDir2D.TopRight;
                else if (X < 0 && Y > 0)
                    return eYoonDir2D.TopLeft;
                else if (X < 0 && Y < 0)
                    return eYoonDir2D.BottomLeft;
                else if (X > 0 && Y < 0)
                    return eYoonDir2D.BottomRight;
                else
                    return eYoonDir2D.None;
            }
            set
            {
                switch(value)
                {
                    case eYoonDir2D.Center:
                        X = Y = 0;
                        break;
                    case eYoonDir2D.Top:
                        X = 0;
                        Y = 1;
                        break;
                    case eYoonDir2D.Bottom:
                        X = 0;
                        Y = -1;
                        break;
                    case eYoonDir2D.Right:
                        X = 1;
                        Y = 0;
                        break;
                    case eYoonDir2D.Left:
                        X = -1;
                        Y = 0;
                        break;
                    case eYoonDir2D.TopRight:
                        X = 1;
                        Y = 1;
                        break;
                    case eYoonDir2D.TopLeft:
                        X = -1;
                        Y = 1;
                        break;
                    case eYoonDir2D.BottomLeft:
                        X = -1;
                        Y = -1;
                        break;
                    case eYoonDir2D.BottomRight:
                        X = 1;
                        Y = -1;
                        break;
                    default:
                        break;
                }
            }
        }

        public void CopyFrom(IYoonVector v)
        {
            if (v is YoonVector2N vec)
            {
                X = vec.X;
                Y = vec.Y;
                W = 1;
            }
        }

        public IYoonVector Clone()
        {
            YoonVector2N v = new YoonVector2N();
            v.X = this.X;
            v.Y = this.Y;
            v.W = this.W;
            return v;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IYoonVector);
        }

        public bool Equals(IYoonVector v)
        {
            if (v is YoonVector2N vec)
            {
                if (X == vec.X &&
                    Y == vec.Y &&
                    W == vec.W)
                    return true;
            }
            return false;
        }

        [XmlIgnore]
        private static double DELTA = 0.0000000000001;
        [XmlIgnore]
        public int W
        {
            get => Array[2];

            set
            {
                Array[2] = value;
            }
        }
        [XmlAttribute]
        public int X
        {
            get => Array[0];

            set
            {
                Array[0] = value;
            }
        }
        [XmlAttribute]
        public int Y
        {
            get => Array[1];

            set
            {
                Array[1] = value;
            }
        }
        public IYoonCart<int> Cartesian
        {
            get => new YoonCartN(X, Y, 0, 0, 0, 0);
            set
            {
                try
                {
                    X = value.X;
                    Y = value.Y;
                }
                catch
                {
                    //
                }
            }
        }
        public int[] Array { get; set; } = new int[3];

        public YoonVector2N()
        {
            X = 0;
            Y = 0;
            W = 1;
        }
        public YoonVector2N(IYoonVector p)
        {
            CopyFrom(p);
        }
        public YoonVector2N(int dx, int dy)
        {
            X = dx;
            Y = dy;
            W = 1;
        }
        public YoonVector2N(eYoonDir2D nDir)
        {
            Direction = nDir;
            W = 1;
        }

        public void Zero()
        {
            X = 0;
            Y = 0;
            W = 1;
        }
        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public IYoonVector Unit()
        {
            double len = this.Length();
            if (len > DELTA)
            {
                len = 1.0 / len;
                X *= (int)len;
                Y *= (int)len;
            }
            return this;
        }

        public double Distance(IYoonVector pPos)
        {
            if (pPos is YoonVector2N vec)
            {
                double dx = this.X - vec.X;
                double dy = this.Y - vec.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }
            else
                return 0.0;
        }

        public eYoonDir2D DirectionTo(IYoonVector pPos)
        {
            if (pPos is YoonVector2N vec)
            {
                YoonVector2N pVecDiff = this - vec;
                return pVecDiff.Direction;
            }
            else
                return eYoonDir2D.None;
        }

        public double Angle2D(IYoonVector pPosObject)
        {
            if (pPosObject is YoonVector2N vec)
            {
                return Math.Atan2(vec.Y - Y, vec.X - X);
            }
            else
                return 0.0;
        }

        public IYoonVector GetScaleVector(int sx, int sy)
        {
            YoonMatrix2N pMatrix = new YoonMatrix2N();
            pMatrix.SetScaleUnit(sx, sy);
            return pMatrix * this;
        }

        public IYoonVector GetNextVector(int dx, int dy)
        {
            YoonMatrix2N pMatrix = new YoonMatrix2N();
            pMatrix.SetMovementUnit(dx, dy);
            return pMatrix * this;
        }

        public IYoonVector GetNextVector(IYoonVector pv)
        {
            if (pv is YoonVector2N pVector)
            {
                YoonMatrix2N pMatrix = new YoonMatrix2N();
                pMatrix.SetMovementUnit(pVector);
                return pMatrix * this;
            }
            return this;
        }

        public IYoonVector GetNextVector(eYoonDir2D dir)
        {
            return GetNextVector(new YoonVector2N(dir));
        }

        public IYoonVector GetRotateVector(double angle)
        {
            YoonMatrix2N pMatrix = new YoonMatrix2N();
            pMatrix.SetRotateUnit(angle);
            return pMatrix * this;
        }

        public IYoonVector GetRotateVector(IYoonVector center, double angle)
        {
            if (center is YoonVector2N pVecCenter)
            {
                YoonVector2N pVecResult = new YoonVector2N(this);
                pVecResult.Move(-pVecCenter);
                pVecResult.Rotate(angle);
                pVecResult.Move(pVecCenter);
                return pVecResult;
            }
            return this;
        }

        public void Scale(int sx, int sy)
        {
            YoonMatrix2N pMatrix = new YoonMatrix2N();
            pMatrix.SetScaleUnit(sx, sy);
            YoonVector2N v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
        }

        public void Move(int dx, int dy)
        {
            YoonMatrix2N pMatrix = new YoonMatrix2N();
            pMatrix.SetMovementUnit(dx, dy);
            YoonVector2N v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
        }

        public void Move(IYoonVector pv)
        {
            if (pv is YoonVector2N pVector)
            {
                YoonMatrix2N pMatrix = new YoonMatrix2N();
                pMatrix.SetMovementUnit(pVector);
                YoonVector2N v = pMatrix * this;
                this.X = v.X;
                this.Y = v.Y;
            }
        }

        public void Move(eYoonDir2D dir)
        {
            Move(new YoonVector2N(dir));
        }

        public void Rotate(double angle)
        {
            YoonMatrix2N pMatrix = new YoonMatrix2N();
            pMatrix.SetRotateUnit(angle);
            YoonVector2N v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
        }

        public void Rotate(IYoonVector center, double angle)
        {
            if (center is YoonVector2N pVecCenter)
            {
                Move(-pVecCenter);
                Rotate(angle);
                Move(pVecCenter);
            }
        }

        public bool Equals(YoonVector2N other)
        {
            return other != null &&
                   Count == other.Count &&
                   Direction == other.Direction &&
                   W == other.W &&
                   X == other.X &&
                   Y == other.Y &&
                   EqualityComparer<IYoonCart<int>>.Default.Equals(Cartesian, other.Cartesian) &&
                   EqualityComparer<int[]>.Default.Equals(Array, other.Array);
        }

        public override int GetHashCode()
        {
            int hashCode = 1178670866;
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            hashCode = hashCode * -1521134295 + Direction.GetHashCode();
            hashCode = hashCode * -1521134295 + W.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IYoonCart<int>>.Default.GetHashCode(Cartesian);
            hashCode = hashCode * -1521134295 + EqualityComparer<int[]>.Default.GetHashCode(Array);
            return hashCode;
        }

        public YoonVector2D ToVector2D()
        {
            return new YoonVector2D(X, Y);
        }

        IYoonFigure IYoonFigure.Clone()
        {
            return Clone();
        }

        public static YoonVector2N operator *(YoonMatrix3X3Int m, YoonVector2N v)
        {
            YoonVector2N pVector = new YoonVector2N();
            for (int i = 0; i < v.Count; i++)
            {
                pVector.Array[i] = 0;
                for (int k = 0; k < m.Length; k++)
                    pVector.Array[i] += m.Array[i, k] * v.Array[k];
            }
            return pVector;
        }
        public static YoonVector2N operator *(YoonVector2N v, int a)
        {
            return new YoonVector2N(v.X * a, v.Y * a);
        }
        public static YoonVector2N operator +(YoonVector2N v1, YoonVector2N v2)
        {
            return new YoonVector2N(v1.X + v2.X, v1.Y + v2.Y);
        }
        public static YoonVector2N operator +(YoonVector2N v, eYoonDir2D d)
        {
            YoonVector2N pVector = new YoonVector2N(d);
            return new YoonVector2N(v.X + pVector.X, v.Y + pVector.Y);
        }
        public static YoonVector2N operator -(YoonVector2N v1, YoonVector2N v2)
        {
            return new YoonVector2N(v1.X - v2.X, v1.Y - v2.Y);
        }
        public static YoonVector2N operator -(YoonVector2N v, eYoonDir2D d)
        {
            YoonVector2N pVector = new YoonVector2N(d);
            return new YoonVector2N(v.X - pVector.X, v.Y - pVector.Y);
        }
        public static YoonVector2N operator -(YoonVector2N v)
        {
            return new YoonVector2N(-v.X, -v.Y);
        }
        public static YoonVector2N operator /(YoonVector2N v, int a)
        {
            return new YoonVector2N(v.X / a, v.Y / a);
        }
        public static int operator *(YoonVector2N v1, YoonVector2N v2) // dot product
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }
        public static bool operator ==(YoonVector2N v1, YoonVector2N v2)
        {
            return v1?.Equals(v2) == true;
        }
        public static bool operator !=(YoonVector2N v1, YoonVector2N v2)
        {
            return v1?.Equals(v2) == false;
        }
    }

    /// <summary>
    /// 2차원 동차변환 벡터
    /// </summary>
    public class YoonVector2D : IYoonVector, IYoonVector2D<double>, IEquatable<YoonVector2D>
    {
        public int Count { get; } = 3;

        public eYoonDir2D Direction
        {
            get
            {
                if (X == 0 && Y == 0)
                    return eYoonDir2D.Center;
                else if (X == 0 && Y > 0)
                    return eYoonDir2D.Top;
                else if (X == 0 && Y < 0)
                    return eYoonDir2D.Bottom;
                else if (X > 0 && Y == 0)
                    return eYoonDir2D.Right;
                else if (X < 0 && Y == 0)
                    return eYoonDir2D.Left;
                else if (X > 0 && Y > 0)
                    return eYoonDir2D.TopRight;
                else if (X < 0 && Y > 0)
                    return eYoonDir2D.TopLeft;
                else if (X < 0 && Y < 0)
                    return eYoonDir2D.BottomLeft;
                else if (X > 0 && Y < 0)
                    return eYoonDir2D.BottomRight;
                else
                    return eYoonDir2D.None;
            }
            set
            {
                switch (value)
                {
                    case eYoonDir2D.Center:
                        X = Y = 0;
                        break;
                    case eYoonDir2D.Top:
                        X = 0;
                        Y = 1;
                        break;
                    case eYoonDir2D.Bottom:
                        X = 0;
                        Y = -1;
                        break;
                    case eYoonDir2D.Right:
                        X = 1;
                        Y = 0;
                        break;
                    case eYoonDir2D.Left:
                        X = -1;
                        Y = 0;
                        break;
                    case eYoonDir2D.TopRight:
                        X = 1;
                        Y = 1;
                        break;
                    case eYoonDir2D.TopLeft:
                        X = -1;
                        Y = 1;
                        break;
                    case eYoonDir2D.BottomLeft:
                        X = -1;
                        Y = -1;
                        break;
                    case eYoonDir2D.BottomRight:
                        X = 1;
                        Y = -1;
                        break;
                    default:
                        break;
                }
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IYoonVector);
        }

        public bool Equals(IYoonVector v)
        {
            if (v is YoonVector2D vec)
            {
                if (X == vec.X &&
                    Y == vec.Y &&
                    W == vec.W)
                    return true;
            }
            return false;
        }

        public void CopyFrom(IYoonVector v)
        {
            if (v is YoonVector2D vec)
            {
                X = vec.X;
                Y = vec.Y;
                W = 1;
            }
        }

        public IYoonVector Clone()
        {
            YoonVector2D v = new YoonVector2D();
            v.X = this.X;
            v.Y = this.Y;
            v.W = this.W;
            return v;
        }
        [XmlIgnore]
        private static double DELTA = 0.0000000000001;
        [XmlIgnore]
        public double W
        {
            get => Array[2];

            set
            {
                Array[2] = value;
            }
        }
        [XmlAttribute]
        public double X
        {
            get => Array[0];

            set
            {
                Array[0] = value;
            }
        }
        [XmlAttribute]
        public double Y
        {
            get => Array[1];

            set
            {
                Array[1] = value;
            }
        }
        public IYoonCart<double> Cartesian
        {
            get => new YoonCartD(X, Y, 0, 0, 0, 0);
            set
            {
                try
                {
                    X = value.X;
                    Y = value.Y;
                }
                catch
                {
                    //
                }
            }
        }
        public double[] Array { get; set; } = new double[3];

        public YoonVector2D()
        {
            X = 0;
            Y = 0;
            W = 1;
        }
        public YoonVector2D(IYoonVector p)
        {
            CopyFrom(p);
        }
        public YoonVector2D(double dx, double dy)
        {
            X = dx;
            Y = dy;
            W = 1;
        }
        public YoonVector2D(eYoonDir2D nDir)
        {
            Direction = nDir;
            W = 1;
        }
        public void Zero()
        {
            X = 0;
            Y = 0;
            W = 1;
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public IYoonVector Unit()
        {
            double len = this.Length();
            if (len > DELTA)
            {
                len = 1.0 / len;
                X *= len;
                Y *= len;
            }
            return this;
        }

        public double Distance(IYoonVector p)
        {
            if (p is YoonVector2D vec)
            {
                double dx = this.X - vec.X;
                double dy = this.Y - vec.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }
            else
                return 0.0;
        }

        public eYoonDir2D DirectionTo(IYoonVector pPos)
        {
            if (pPos is YoonVector2D vec)
            {
                YoonVector2D pVecDiff = this - vec;
                return pVecDiff.Direction;
            }
            else
                return eYoonDir2D.None;
        }

        public double Angle2D(IYoonVector pPosObject)
        {
            if (pPosObject is YoonVector2D vec)
            {
                return Math.Atan2(vec.Y - Y, vec.X - X);
            }
            else
                return 0.0;
        }

        public IYoonVector GetScaleVector(double sx, double sy)
        {
            YoonMatrix2D pMatrix = new YoonMatrix2D();
            pMatrix.SetScaleUnit(sx, sy);
            return pMatrix * this;
        }

        public IYoonVector GetNextVector(double dx, double dy)
        {
            YoonMatrix2D pMatrix = new YoonMatrix2D();
            pMatrix.SetMovementUnit(dx, dy);
            return pMatrix * this;
        }

        public IYoonVector GetNextVector(IYoonVector pv)
        {
            if (pv is YoonVector2D pVector)
            {
                YoonMatrix2D pMatrix = new YoonMatrix2D();
                pMatrix.SetMovementUnit(pVector);
                return pMatrix * this;
            }
            return this;
        }

        public IYoonVector GetNextVector(eYoonDir2D dir)
        {
            return GetNextVector(new YoonVector2D(dir));
        }

        public IYoonVector GetRotateVector(double angle)
        {
            YoonMatrix2D pMatrix = new YoonMatrix2D();
            pMatrix.SetRotateUnit(angle);
            return pMatrix * this;
        }

        public IYoonVector GetRotateVector(IYoonVector center, double angle)
        {
            if (center is YoonVector2D pVecCenter)
            {
                YoonVector2D pVecResult = new YoonVector2D(this);
                pVecResult.Move(-pVecCenter);
                pVecResult.Rotate(angle);
                pVecResult.Move(pVecCenter);
                return pVecResult;
            }
            return this;
        }

        public void Scale(double sx, double sy)
        {
            YoonMatrix2D pMatrix = new YoonMatrix2D();
            pMatrix.SetScaleUnit(sx, sy);
            YoonVector2D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
        }

        public void Move(double dx, double dy)
        {
            YoonMatrix2D pMatrix = new YoonMatrix2D();
            pMatrix.SetMovementUnit(dx, dy);
            YoonVector2D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
        }

        public void Move(IYoonVector pv)
        {
            if (pv is YoonVector2D pVector)
            {
                YoonMatrix2D pMatrix = new YoonMatrix2D();
                pMatrix.SetMovementUnit(pVector);
                YoonVector2D v = pMatrix * this;
                this.X = v.X;
                this.Y = v.Y;
            }
        }

        public void Move(eYoonDir2D dir)
        {
            Move(new YoonVector2D(dir));
        }

        public void Rotate(double angle)
        {
            YoonMatrix2D pMatrix = new YoonMatrix2D();
            pMatrix.SetRotateUnit(angle);
            YoonVector2D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
        }

        public void Rotate(IYoonVector center, double angle)
        {
            if (center is YoonVector2D pVecCenter)
            {
                Move(-pVecCenter);
                Rotate(angle);
                Move(pVecCenter);
            }
        }

        public bool Equals(YoonVector2D other)
        {
            return other != null &&
                   Count == other.Count &&
                   Direction == other.Direction &&
                   W == other.W &&
                   X == other.X &&
                   Y == other.Y &&
                   EqualityComparer<IYoonCart<double>>.Default.Equals(Cartesian, other.Cartesian) &&
                   EqualityComparer<double[]>.Default.Equals(Array, other.Array);
        }

        public override int GetHashCode()
        {
            int hashCode = 1178670866;
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            hashCode = hashCode * -1521134295 + Direction.GetHashCode();
            hashCode = hashCode * -1521134295 + W.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IYoonCart<double>>.Default.GetHashCode(Cartesian);
            hashCode = hashCode * -1521134295 + EqualityComparer<double[]>.Default.GetHashCode(Array);
            return hashCode;
        }

        public YoonVector2N ToVector2N()
        {
            return new YoonVector2N((int)X, (int)Y);
        }

        IYoonFigure IYoonFigure.Clone()
        {
            return Clone();
        }

        public static YoonVector2D operator *(YoonMatrix3X3Double m, YoonVector2D v)
        {
            YoonVector2D pVector = new YoonVector2D();
            for (int i = 0; i < v.Count; i++)
            {
                pVector.Array[i] = 0;
                for (int k = 0; k < m.Length; k++)
                    pVector.Array[i] += m.Array[i, k] * v.Array[k];
            }
            return pVector;
        }
        public static YoonVector2D operator *(YoonVector2D v, double a)
        {
            return new YoonVector2D(v.X * a, v.Y * a);
        }
        public static YoonVector2D operator +(YoonVector2D v1, YoonVector2D v2)
        {
            return new YoonVector2D(v1.X + v2.X, v1.Y + v2.Y);
        }
        public static YoonVector2D operator +(YoonVector2D v, YoonCartD c)
        {
            return new YoonVector2D(v.X + c.X, v.Y + c.Y);
        }
        public static YoonVector2D operator +(YoonVector2D v, eYoonDir2D d)
        {
            YoonVector2D pVector = new YoonVector2D(d);
            return new YoonVector2D(v.X + pVector.X, v.Y + pVector.Y);
        }
        public static YoonVector2D operator -(YoonVector2D v1, YoonVector2D v2)
        {
            return new YoonVector2D(v1.X - v2.X, v1.Y - v2.Y);
        }
        public static YoonVector2D operator -(YoonVector2D v)
        {
            return new YoonVector2D(-v.X, -v.Y);
        }
        public static YoonVector2D operator -(YoonVector2D v, YoonCartD c)
        {
            return new YoonVector2D(v.X - c.X, v.Y - c.Y);
        }
        public static YoonVector2D operator -(YoonVector2D v, eYoonDir2D d)
        {
            YoonVector2D pVector = new YoonVector2D(d);
            return new YoonVector2D(v.X - pVector.X, v.Y - pVector.Y);
        }
        public static YoonVector2D operator /(YoonVector2D v, double a)
        {
            return new YoonVector2D(v.X / a, v.Y / a);
        }
        public static double operator *(YoonVector2D v1, YoonVector2D v2) // dot product
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }
        public static bool operator ==(YoonVector2D v1, YoonVector2D v2)
        {
            return v1?.Equals(v2) == true;
        }
        public static bool operator !=(YoonVector2D v1, YoonVector2D v2)
        {
            return v1?.Equals(v2) == false;
        }
    }

    /// <summary>
    /// 2차원 동차변환 벡터
    /// </summary>
    public class YoonVector3D : IYoonVector, IYoonVector3D<double>, IEquatable<YoonVector3D>
    {
        public int Count { get; } = 4;

        public override bool Equals(object obj)
        {
            return Equals(obj as IYoonVector);
        }

        public bool Equals(IYoonVector v)
        {
            if (v is YoonVector3D vec)
            {
                if (X == vec.X &&
                    Y == vec.Y &&
                    W == vec.W)
                    return true;
            }
            return false;
        }

        public void CopyFrom(IYoonVector v)
        {
            if (v is YoonVector2D vec)
            {
                X = vec.X;
                Y = vec.Y;
                W = 1;
            }
        }

        public IYoonVector Clone()
        {
            YoonVector3D v = new YoonVector3D();
            v.X = this.X;
            v.Y = this.Y;
            v.Z = this.Z;
            v.W = this.W;
            return v;
        }

        [XmlIgnore]
        private static double DELTA = 0.0000000000001;
        [XmlIgnore]
        public double W
        {
            get => Array[3];

            set
            {
                Array[3] = value;
            }
        }
        [XmlAttribute]
        public double X
        {
            get => Array[0];

            set
            {
                Array[0] = value;
            }
        }
        [XmlAttribute]
        public double Y
        {
            get => Array[1];

            set
            {
                Array[1] = value;
            }
        }
        [XmlAttribute]
        public double Z
        {
            get => Array[2];

            set
            {
                Array[2] = value;
            }
        }

        public IYoonCart<double> Cartesian
        {
            get => new YoonCartD(X, Y, Z, 0, 0, 0);
            set
            {
                try
                {
                    X = value.X;
                    Y = value.Y;
                    Z = value.Z;
                }
                catch
                {
                    //
                }
            }
        }

        public double[] Array { get; set; } = new double[4];

        public YoonVector3D()
        {
            X = 0;
            Y = 0;
            Z = 0;
            W = 1;
        }

        public YoonVector3D(IYoonVector p)
        {
            CopyFrom(p);
        }

        public YoonVector3D(double dx, double dy, double dz)
        {
            X = dx;
            Y = dy;
            Z = dz;
            W = 1;
        }

        public void Zero()
        {
            X = 0;
            Y = 0;
            Z = 0;
            W = 1;
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public IYoonVector Unit()
        {
            double len = this.Length();
            if (len > DELTA)
            {
                len = 1.0 / len;
                X *= len;
                Y *= len;
                Z *= len;
            }
            return this;
        }

        public double Distance(IYoonVector p)
        {
            if (p is YoonVector3D vec)
            {
                double dx = this.X - vec.X;
                double dy = this.Y - vec.Y;
                double dz = this.Z - vec.Z;
                return Math.Sqrt(dx * dx + dy * dy);
            }
            else
                return 0.0;
        }

        public double AngleX(IYoonVector pPosObject)
        {
            if (pPosObject is YoonVector3D vec)
            {
                return Math.Atan2(vec.Y - Y, vec.Z - Z);
            }
            else
                return 0.0;
        }

        public double AngleY(IYoonVector pPosObject)
        {
            if (pPosObject is YoonVector3D vec)
            {
                return Math.Atan2(vec.X - X, vec.Z - Z);
            }
            else
                return 0.0;
        }

        public double AngleZ(IYoonVector pPosObject)
        {
            if (pPosObject is YoonVector3D vec)
            {
                return Math.Atan2(vec.Y - Y, vec.X - X);
            }
            else
                return 0.0;
        }

        public IYoonVector GetScaleVector(double sx, double sy, double sz)
        {
            YoonMatrix3D pMatrix = new YoonMatrix3D();
            pMatrix.SetScaleUnit(sx, sy, sz);
            return pMatrix * this;
        }

        public IYoonVector GetNextVector(double dx, double dy, double dz)
        {
            YoonMatrix3D pMatrix = new YoonMatrix3D();
            pMatrix.SetMovementUnit(dx, dy, dz);
            return pMatrix * this;
        }

        public IYoonVector GetNextVector(IYoonVector pv)
        {
            if (pv is YoonVector3D pVector)
            {
                YoonMatrix3D pMatrix = new YoonMatrix3D();
                pMatrix.SetMovementUnit(pVector);
                return pMatrix * this;
            }
            return this;
        }

        public void Scale(double sx, double sy, double sz)
        {
            YoonMatrix3D pMatrix = new YoonMatrix3D();
            pMatrix.SetScaleUnit(sx, sy, sz);
            YoonVector3D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
        }

        public void Move(double dx, double dy, double dz)
        {
            YoonMatrix3D pMatrix = new YoonMatrix3D();
            pMatrix.SetMovementUnit(dx, dy, dz);
            YoonVector3D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
        }

        public void Move(IYoonVector pv)
        {
            if (pv is YoonVector3D pVector)
            {
                YoonMatrix3D pMatrix = new YoonMatrix3D();
                pMatrix.SetMovementUnit(pVector);
                YoonVector3D v = pMatrix * this;
                this.X = v.X;
                this.Y = v.Y;
                this.Z = v.Z;
            }
        }

        public void RotateX(double angle)
        {
            YoonMatrix3D pMatrix = new YoonMatrix3D();
            pMatrix.SetRotateXUnit(angle);
            YoonVector3D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
        }

        public void RotateY(double angle)
        {
            YoonMatrix3D pMatrix = new YoonMatrix3D();
            pMatrix.SetRotateYUnit(angle);
            YoonVector3D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
        }

        public void RotateZ(double angle)
        {
            YoonMatrix3D pMatrix = new YoonMatrix3D();
            pMatrix.SetRotateZUnit(angle);
            YoonVector3D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
        }

        public bool Equals(YoonVector3D other)
        {
            return other != null &&
                   Count == other.Count &&
                   W == other.W &&
                   X == other.X &&
                   Y == other.Y &&
                   Z == other.Z &&
                   EqualityComparer<IYoonCart<double>>.Default.Equals(Cartesian, other.Cartesian) &&
                   EqualityComparer<double[]>.Default.Equals(Array, other.Array);
        }

        public override int GetHashCode()
        {
            int hashCode = -366330435;
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            hashCode = hashCode * -1521134295 + W.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IYoonCart<double>>.Default.GetHashCode(Cartesian);
            hashCode = hashCode * -1521134295 + EqualityComparer<double[]>.Default.GetHashCode(Array);
            return hashCode;
        }

        IYoonFigure IYoonFigure.Clone()
        {
            return Clone();
        }

        public static YoonVector3D operator *(YoonMatrix4X4Double m, YoonVector3D v)
        {
            YoonVector3D pVector = new YoonVector3D();
            for (int i = 0; i < v.Count; i++)
            {
                pVector.Array[i] = 0;
                for (int k = 0; k < m.Length; k++)
                    pVector.Array[i] += m.Array[i, k] * v.Array[k];
            }
            return pVector;
        }
        public static YoonVector3D operator *(YoonVector3D v, double a)
        {
            return new YoonVector3D(v.X * a, v.Y * a, v.Z * a);
        }
        public static YoonVector3D operator +(YoonVector3D v1, YoonVector3D v2)
        {
            return new YoonVector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public static YoonVector3D operator +(YoonVector3D v, YoonCartD c)
        {
            return new YoonVector3D(v.X + c.X, v.Y + c.Y, v.Z + c.Z);
        }
        public static YoonVector3D operator -(YoonVector3D v1, YoonVector3D v2)
        {
            return new YoonVector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static YoonVector3D operator -(YoonVector3D v)
        {
            return new YoonVector3D(-v.X, -v.Y, -v.Z);
        }
        public static YoonVector3D operator -(YoonVector3D v, YoonCartD c)
        {
            return new YoonVector3D(v.X - c.X, v.Y - c.Y, v.Z - c.Z);
        }
        public static YoonVector3D operator /(YoonVector3D v, double a)
        {
            return new YoonVector3D(v.X / a, v.Y / a, v.Z / a);
        }
        public static double operator *(YoonVector3D v1, YoonVector3D v2) // dot product
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }
        public static bool operator ==(YoonVector3D v1, YoonVector3D v2)
        {
            return v1?.Equals(v2) == true;
        }
        public static bool operator !=(YoonVector3D v1, YoonVector3D v2)
        {
            return v1?.Equals(v2) == false;
        }
    }

}
