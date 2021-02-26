using System;
using System.Collections.Generic;

namespace YoonFactory.Align
{
    /// <summary>
    /// Align 주요 함수 (1D)
    /// </summary>
    public class YoonAlign1D
    {
        private double ToAngle(double dTheta)
        {
            return 180 * dTheta / Math.PI;
        }

        private double ToTheta(double dAngle)
        {
            return Math.PI * dAngle / 180;
        }

        public AlignResult AlignResult { get; private set; } = new AlignResult();

        private YoonVector2D m_vecPosAlign = new YoonVector2D();
        private YoonVector2D m_vecPosCurr = new YoonVector2D();
        private double m_dThetaOrigin = 0.0;
        private double m_dThetaCurr = 0.0;

        public YoonAlign1D()
        {
            //
        }

        public void SetOriginPosition(double dX, double dY, double dTheta)
        {
            m_vecPosAlign = new YoonVector2D(dX, dY);
            m_dThetaOrigin = dTheta;
        }

        public void SetCurrentPosition(double dX, double dY, double dTheta)
        {
            m_vecPosCurr = new YoonVector2D(dX, dY);
            m_dThetaCurr = dTheta;
        }

        public void Calculate(double scale = 1.0)
        {
            ////  Align 계산
            YoonVector2D vdAlignResult = m_vecPosAlign - m_vecPosCurr;

            AlignResult.ResultX = vdAlignResult.X * scale;
            AlignResult.ResultY = vdAlignResult.Y * scale;
            AlignResult.ResultT = (m_dThetaOrigin - m_dThetaCurr) * scale;
        }
    }

    /// <summary>
    /// Align 주요 함수 (2D)
    /// </summary>
    public class YoonAlign2D
    {
        private const double INVALID_NUM = -10000.0;
        private Dictionary<eYoonDir2D, YoonVector2D> m_fDicPosAlign = new Dictionary<eYoonDir2D, YoonVector2D>();
        private Dictionary<eYoonDir2D, YoonVector2D> m_fDicPosCurrent = new Dictionary<eYoonDir2D, YoonVector2D>();

        public eYoonDir2D LeftCameraDirection { get; private set; } = eYoonDir2D.TopLeft;
        public eYoonDir2D RightCameraDirection { get; private set; } = eYoonDir2D.TopRight;
        public AlignResult AlignResult { get; private set; } = new AlignResult();

        private double ToAngle(double dTheta)
        {
            return 180 * dTheta / Math.PI;
        }

        private double ToTheta(double dAngle)
        {
            return Math.PI * dAngle / 180;
        }

        public YoonAlign2D(eYoonDir2D dirLeft, eYoonDir2D dirRight)
        {
            ////  Align 대상위치 초기화
            m_fDicPosAlign.Clear();
            foreach (eYoonDir2D nDir in YoonDirFactory.GetSquareDirections())
            {
                YoonVector2D vd = new YoonVector2D();
                m_fDicPosAlign.Add(nDir, vd);
            }

            ////  현재 위치 초기화
            m_fDicPosCurrent.Clear();
            foreach (eYoonDir2D nDir in YoonDirFactory.GetSquareDirections())
            {
                YoonVector2D vd = new YoonVector2D();
                m_fDicPosCurrent.Add(nDir, vd);
            }

            LeftCameraDirection = dirLeft;
            RightCameraDirection = dirRight;
        }

        public void SetOriginPosition(YoonVector2D posLeft, YoonVector2D posRight)
        {
            m_fDicPosAlign[LeftCameraDirection] = posLeft;
            m_fDicPosAlign[RightCameraDirection] = posRight;
        }

        private void SetCurrentPosition(YoonVector2D posLeft, YoonVector2D posRight)
        {
            m_fDicPosCurrent[LeftCameraDirection] = posLeft;
            m_fDicPosCurrent[RightCameraDirection] = posRight;
        }

        public double CalculateTheta(YoonVector2D posLeft, YoonVector2D posRight, double scale = 1.0)
        {
            ////  Align 위치를 지정한다.
            YoonVector2D posObject_Left = m_fDicPosAlign[LeftCameraDirection];
            YoonVector2D posObject_Right = m_fDicPosAlign[RightCameraDirection];

            ////  회전중심(0,0) 기준 Object가 "Y축과" 이루는 각도 계산
            double thetaObject = Math.Atan2(posObject_Right.X - posObject_Left.X, posObject_Left.Y - posObject_Right.Y);
            if (thetaObject == double.NaN) return INVALID_NUM;

            ////  회전중심(0,0) 기준 Mark가 "Y축과" 이루는 각도 계산 및 현재 값 삽입
            double thetaMark = Math.Atan2(posRight.X - posLeft.X, posLeft.Y - posRight.Y);
            if (thetaMark != double.NaN) SetCurrentPosition(posLeft, posRight);
            else return INVALID_NUM;

            double theta = (thetaObject - thetaMark) * scale;
            AlignResult.ResultT = theta;
            return theta;
        }

        public YoonVector2D CalculateXY(YoonVector2D posLeft, YoonVector2D posRight, double theta, eYoonDir2D dirDefault, double scale = 1.0)
        {
            ////  예외 처리
            if (theta == INVALID_NUM) return new YoonVector2D(INVALID_NUM, INVALID_NUM);

            ////  Align 계산
            YoonVector2D posObject = m_fDicPosAlign[dirDefault];
            YoonVector2D vdAlignResult = new YoonVector2D();
            if (dirDefault == LeftCameraDirection)
            {
                vdAlignResult.X = posObject.X - (posLeft.X * Math.Cos(theta) - posLeft.Y * Math.Sin(theta));
                vdAlignResult.Y = posObject.Y - (posLeft.X * Math.Sin(theta) + posLeft.Y * Math.Cos(theta));
            }
            else if (dirDefault == RightCameraDirection)
            {
                vdAlignResult.X = posObject.X - (posRight.X * Math.Cos(theta) - posRight.Y * Math.Sin(theta));
                vdAlignResult.Y = posObject.Y - (posRight.X * Math.Sin(theta) + posRight.Y * Math.Cos(theta));
            }
            else
            {
                vdAlignResult.X = vdAlignResult.Y = INVALID_NUM;
            }
            if (vdAlignResult.X != INVALID_NUM) SetCurrentPosition(posLeft, posRight);

            AlignResult.ResultX = vdAlignResult.X * scale;
            AlignResult.ResultY = vdAlignResult.Y * scale;
            return vdAlignResult;
        }
    }
}
