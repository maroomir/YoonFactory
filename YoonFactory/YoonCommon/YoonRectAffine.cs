using System.Xml.Serialization;

namespace YoonFactory
{
    public class YoonRectAffine2D : IYoonRect, IYoonRect2D<double>
    {
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
    }

}
