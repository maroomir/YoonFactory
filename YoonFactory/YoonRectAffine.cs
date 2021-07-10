using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace YoonFactory
{
    public class YoonRectAffine2D : IYoonRect, IYoonRect2D<double>, IEquatable<YoonRectAffine2D>
    {
        public override bool Equals(object obj)
        {
            return Equals(obj as IYoonRect);
        }

        public bool Equals(IYoonRect r)
        {
            if (r is YoonRectAffine2D rect)
            {
                if (CenterPos.X == rect.CenterPos.X &&
                    CenterPos.Y == rect.CenterPos.Y &&
                    Width == rect.Width &&
                    Height == rect.Height &&
                    Rotation == rect.Rotation)
                    return true;
            }
            return false;
        }

        public IYoonRect Clone()
        {
            YoonRectAffine2D r = new YoonRectAffine2D(0.0, 0.0, 0.0);
            r.CenterPos = this.CenterPos.Clone() as YoonVector2D;
            r.Width = this.Width;
            r.Height = this.Height;
            r.Rotation = this.Rotation;
            return r;
        }
        public void CopyFrom(IYoonRect r)
        {
            if (r is YoonRectAffine2D rect)
            {
                CenterPos = rect.CenterPos.Clone() as YoonVector2D;
                Width = rect.Width;
                Height = rect.Height;
                Rotation = rect.Rotation;
            }
        }

        [XmlAttribute]
        public IYoonVector2D<double> CenterPos
        {
            get => m_vecCenter;
            set
            {
                m_vecCenter = value as YoonVector2D;
                InitRectOrigin(m_vecCenter as YoonVector2D, m_dWidth, m_dHeight);
            }
        }
        [XmlAttribute]
        public double Width
        {
            get => m_dWidth;
            set
            {
                m_dWidth = value;
                InitRectOrigin(m_vecCenter as YoonVector2D, m_dWidth, m_dHeight);
            }
        }
        [XmlAttribute]
        public double Height
        {
            get => m_dHeight;
            set
            {
                m_dHeight = value;
                InitRectOrigin(m_vecCenter as YoonVector2D, m_dWidth, m_dHeight);
            }
        }
        [XmlAttribute]
        public double Rotation
        {
            get => m_dRotation;
            set
            {
                m_dRotation = value;
                InitRectOrigin(m_vecCenter as YoonVector2D, m_dWidth, m_dHeight);

                m_vecCornerRotate_TopLeft = m_vecCornerOrigin_TopLeft.GetRotateVector(m_vecCenter, m_dRotation) as YoonVector2D;
                m_vecCornerRotate_BottomLeft = m_vecCornerOrigin_BottomLeft.GetRotateVector(m_vecCenter, m_dRotation) as YoonVector2D;
                m_vecCornerRotate_TopRight = m_vecCornerOrigin_TopRight.GetRotateVector(m_vecCenter, m_dRotation) as YoonVector2D;
                m_vecCornerRotate_BottomRight = m_vecCornerOrigin_BottomRight.GetRotateVector(m_vecCenter, m_dRotation) as YoonVector2D;
            }
        }

        private void InitRectOrigin(YoonVector2D vecCenter, double dWidth, double dHeight)
        {
            m_vecCornerOrigin_TopLeft.X = vecCenter.X - dWidth / 2;
            m_vecCornerOrigin_TopLeft.Y = vecCenter.Y - dHeight / 2;
            m_vecCornerOrigin_TopRight.X = vecCenter.X + dWidth / 2;
            m_vecCornerOrigin_TopRight.Y = vecCenter.Y - dHeight / 2;
            m_vecCornerOrigin_BottomLeft.X = vecCenter.X - dWidth / 2;
            m_vecCornerOrigin_BottomLeft.Y = vecCenter.Y + dHeight / 2;
            m_vecCornerOrigin_BottomRight.X = vecCenter.X + dWidth / 2;
            m_vecCornerOrigin_BottomRight.Y = vecCenter.Y + dHeight / 2;
        }

        #region 내부 변수
        private YoonVector2D m_vecCornerOrigin_TopLeft = new YoonVector2D();
        private YoonVector2D m_vecCornerOrigin_BottomLeft = new YoonVector2D();
        private YoonVector2D m_vecCornerOrigin_TopRight = new YoonVector2D();
        private YoonVector2D m_vecCornerOrigin_BottomRight = new YoonVector2D();
        private YoonVector2D m_vecCornerRotate_TopLeft = null;
        private YoonVector2D m_vecCornerRotate_BottomLeft = null;
        private YoonVector2D m_vecCornerRotate_TopRight = null;
        private YoonVector2D m_vecCornerRotate_BottomRight = null;
        private YoonVector2D m_vecCenter = new YoonVector2D(); // Set 내 InitOrigin과 충돌에 따른 StackOverflow 방지용
        private double m_dWidth = 0.0; // Set 내 InitOrigin과 충돌에 따른 StackOverflow 방지용
        private double m_dHeight = 0.0; // Set 내 InitOrigin과 충돌에 따른 StackOverflow 방지용
        private double m_dRotation = 0.0; // Set 내 InitOrigin과 충돌에 따른 StackOverflow 방지용
        #endregion

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
            get => m_vecCornerRotate_TopLeft;

        }

        public IYoonVector2D<double> TopRight
        {
            get => m_vecCornerRotate_TopRight;
        }

        public IYoonVector2D<double> BottomLeft
        {
            get => m_vecCornerRotate_BottomLeft;
        }

        public IYoonVector2D<double> BottomRight
        {
            get => m_vecCornerRotate_BottomRight;
        }

        public YoonRectAffine2D()
        {
            CenterPos = new YoonVector2D();
            CenterPos.X = 0;
            CenterPos.Y = 0;
            Width = 0;
            Height = 0;
            Rotation = 0;
        }

        public YoonRectAffine2D(double dWidth, double dHeight, double dTheta)
        {
            CenterPos = new YoonVector2D();
            CenterPos.X = 0;
            CenterPos.Y = 0;
            Width = dWidth;
            Height = dHeight;

            Rotation = dTheta;
        }

        public YoonRectAffine2D(double dX, double dY, double dWidth, double dHeight, double dTheta)
        {
            CenterPos = new YoonVector2D();
            CenterPos.X = dX;
            CenterPos.Y = dY;
            Width = dWidth;
            Height = dHeight;

            Rotation = dTheta;
        }

        public YoonRectAffine2D(YoonVector2D vecPos, double dWidth, double dHeight, double dTheta)
        {
            CenterPos = vecPos.Clone() as YoonVector2D;
            Width = dWidth;
            Height = dHeight;
            Rotation = dTheta;
        }

        public bool IsContain(IYoonVector vec)
        {
            if (vec is YoonVector2D pPos)
            {
                if (m_vecCornerRotate_BottomLeft.X < pPos.X && m_vecCornerRotate_TopLeft.X < pPos.X &&
                    pPos.X < m_vecCornerRotate_BottomRight.X && pPos.X < m_vecCornerRotate_TopRight.X &&
                    m_vecCornerRotate_TopLeft.Y < pPos.Y && m_vecCornerRotate_TopRight.Y < pPos.Y &&
                    pPos.Y < m_vecCornerRotate_BottomLeft.Y && pPos.Y < m_vecCornerRotate_BottomRight.Y)
                    return true;
            }
            return false;
        }

        public double Area()
        {
            return Width * Height;
        }

        public bool Equals(YoonRectAffine2D other)
        {
            return other != null &&
                   EqualityComparer<IYoonVector2D<double>>.Default.Equals(CenterPos, other.CenterPos) &&
                   Width == other.Width &&
                   Height == other.Height &&
                   Rotation == other.Rotation &&
                   EqualityComparer<YoonVector2D>.Default.Equals(m_vecCornerOrigin_TopLeft, other.m_vecCornerOrigin_TopLeft) &&
                   EqualityComparer<YoonVector2D>.Default.Equals(m_vecCornerOrigin_BottomLeft, other.m_vecCornerOrigin_BottomLeft) &&
                   EqualityComparer<YoonVector2D>.Default.Equals(m_vecCornerOrigin_TopRight, other.m_vecCornerOrigin_TopRight) &&
                   EqualityComparer<YoonVector2D>.Default.Equals(m_vecCornerOrigin_BottomRight, other.m_vecCornerOrigin_BottomRight) &&
                   EqualityComparer<YoonVector2D>.Default.Equals(m_vecCornerRotate_TopLeft, other.m_vecCornerRotate_TopLeft) &&
                   EqualityComparer<YoonVector2D>.Default.Equals(m_vecCornerRotate_BottomLeft, other.m_vecCornerRotate_BottomLeft) &&
                   EqualityComparer<YoonVector2D>.Default.Equals(m_vecCornerRotate_TopRight, other.m_vecCornerRotate_TopRight) &&
                   EqualityComparer<YoonVector2D>.Default.Equals(m_vecCornerRotate_BottomRight, other.m_vecCornerRotate_BottomRight) &&
                   EqualityComparer<YoonVector2D>.Default.Equals(m_vecCenter, other.m_vecCenter) &&
                   m_dWidth == other.m_dWidth &&
                   m_dHeight == other.m_dHeight &&
                   m_dRotation == other.m_dRotation &&
                   Left == other.Left &&
                   Top == other.Top &&
                   Right == other.Right &&
                   Bottom == other.Bottom &&
                   EqualityComparer<IYoonVector2D<double>>.Default.Equals(TopLeft, other.TopLeft) &&
                   EqualityComparer<IYoonVector2D<double>>.Default.Equals(TopRight, other.TopRight) &&
                   EqualityComparer<IYoonVector2D<double>>.Default.Equals(BottomLeft, other.BottomLeft) &&
                   EqualityComparer<IYoonVector2D<double>>.Default.Equals(BottomRight, other.BottomRight);
        }

        public override int GetHashCode()
        {
            int hashCode = -408325479;
            hashCode = hashCode * -1521134295 + EqualityComparer<IYoonVector2D<double>>.Default.GetHashCode(CenterPos);
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            hashCode = hashCode * -1521134295 + Rotation.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<YoonVector2D>.Default.GetHashCode(m_vecCornerOrigin_TopLeft);
            hashCode = hashCode * -1521134295 + EqualityComparer<YoonVector2D>.Default.GetHashCode(m_vecCornerOrigin_BottomLeft);
            hashCode = hashCode * -1521134295 + EqualityComparer<YoonVector2D>.Default.GetHashCode(m_vecCornerOrigin_TopRight);
            hashCode = hashCode * -1521134295 + EqualityComparer<YoonVector2D>.Default.GetHashCode(m_vecCornerOrigin_BottomRight);
            hashCode = hashCode * -1521134295 + EqualityComparer<YoonVector2D>.Default.GetHashCode(m_vecCornerRotate_TopLeft);
            hashCode = hashCode * -1521134295 + EqualityComparer<YoonVector2D>.Default.GetHashCode(m_vecCornerRotate_BottomLeft);
            hashCode = hashCode * -1521134295 + EqualityComparer<YoonVector2D>.Default.GetHashCode(m_vecCornerRotate_TopRight);
            hashCode = hashCode * -1521134295 + EqualityComparer<YoonVector2D>.Default.GetHashCode(m_vecCornerRotate_BottomRight);
            hashCode = hashCode * -1521134295 + EqualityComparer<YoonVector2D>.Default.GetHashCode(m_vecCenter);
            hashCode = hashCode * -1521134295 + m_dWidth.GetHashCode();
            hashCode = hashCode * -1521134295 + m_dHeight.GetHashCode();
            hashCode = hashCode * -1521134295 + m_dRotation.GetHashCode();
            hashCode = hashCode * -1521134295 + Left.GetHashCode();
            hashCode = hashCode * -1521134295 + Top.GetHashCode();
            hashCode = hashCode * -1521134295 + Right.GetHashCode();
            hashCode = hashCode * -1521134295 + Bottom.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IYoonVector2D<double>>.Default.GetHashCode(TopLeft);
            hashCode = hashCode * -1521134295 + EqualityComparer<IYoonVector2D<double>>.Default.GetHashCode(TopRight);
            hashCode = hashCode * -1521134295 + EqualityComparer<IYoonVector2D<double>>.Default.GetHashCode(BottomLeft);
            hashCode = hashCode * -1521134295 + EqualityComparer<IYoonVector2D<double>>.Default.GetHashCode(BottomRight);
            return hashCode;
        }

        public IYoonVector2D<double> GetPosition(eYoonDir2D nDir)
        {
            switch (nDir)
            {
                case eYoonDir2D.TopLeft:
                    return TopLeft;
                case eYoonDir2D.Top:
                    return new YoonVector2D(CenterPos.X, Top);
                case eYoonDir2D.TopRight:
                    return TopRight;
                case eYoonDir2D.Left:
                    return new YoonVector2D(Left, CenterPos.Y);
                case eYoonDir2D.Center:
                    return CenterPos;
                case eYoonDir2D.Right:
                    return new YoonVector2D(Right, CenterPos.Y);
                case eYoonDir2D.BottomLeft:
                    return BottomLeft;
                case eYoonDir2D.Bottom:
                    return new YoonVector2D(CenterPos.X, Bottom);
                case eYoonDir2D.BottomRight:
                    return BottomRight;
                default:
                    return new YoonVector2D();
            }
        }

        public static bool operator ==(YoonRectAffine2D r1, YoonRectAffine2D r2)
        {
            return r1?.Equals(r2) == true;
        }

        public static bool operator !=(YoonRectAffine2D r1, YoonRectAffine2D r2)
        {
            return r1?.Equals(r2) == false;
        }
    }

}
