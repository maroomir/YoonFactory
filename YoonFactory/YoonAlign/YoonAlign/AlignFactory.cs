using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace YoonFactory.Align
{
    /// <summary>
    /// Align 결과값을 담는 클래스
    /// </summary>
    public class AlignResult : IYoonResult
    {
        private double fResultX;
        private double fResultY;
        private double fResultT;
        public double ResultX
        {
            get { return fResultX; }
            set { fResultX = value; }
        }
        public double ResultY
        {
            get { return fResultY; }
            set { fResultY = value; }
        }
        public double ResultT
        {
            get { return fResultT; }
            set { fResultT = value; }
        }

        public AlignResult()
        {
            fResultX = 0.0;
            fResultY = 0.0;
            fResultT = 0.0;
        }

        public bool IsEqual(IYoonResult pResult)
        {
            if (pResult == null) return false;

            if(pResult is AlignResult pResultAlign)
            {
                if (ResultX == pResultAlign.ResultX && ResultY == pResultAlign.ResultY && ResultT == pResultAlign.ResultT)
                    return true;
            }
            return false;
        }

        public void CopyFrom(IYoonResult pResult)
        {
            if (pResult == null) return;

            if(pResult is AlignResult pResultAlign)
            {
                ResultX = pResultAlign.ResultX;
                ResultY = pResultAlign.ResultY;
                ResultT = pResultAlign.ResultT;
            }
        }

        public IYoonResult Clone()
        {
            AlignResult pTargetResult = new AlignResult();
            pTargetResult.ResultX = ResultX;
            pTargetResult.ResultY = ResultY;
            pTargetResult.ResultT = ResultT;
            return pTargetResult;
        }
    }

    public class GrobalCoordinateTransform : IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.

                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                m_pDicInsertPos.Clear();
                m_pDicInsertPos = null;
                m_pDicCenterPos.Clear();
                m_pDicCenterPos = null;

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
         ~GrobalCoordinateTransform() {
           // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
           Dispose(false);
         }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            GC.SuppressFinalize(this);
        }
        #endregion

        public YoonVector2D FOV { get; private set; } = new YoonVector2D();
        public YoonVector2D MmPerPixel { get; private set; } = new YoonVector2D();

        private Dictionary<eYoonDirRect, YoonVector2D> m_pDicInsertPos = new Dictionary<eYoonDirRect, YoonVector2D>();
        private Dictionary<eYoonDirRect, YoonVector2D> m_pDicCenterPos = new Dictionary<eYoonDirRect, YoonVector2D>();

        private double ToAngle(double dTheta)
        {
            return 180 * dTheta / Math.PI;
        }

        private double ToTheta(double dAngle)
        {
            return Math.PI * dAngle / 180;
        }

        private YoonVector2D ToPixel(YoonVector2D posMm)
        {
            if(MmPerPixel.X == 0.0 || MmPerPixel.Y==0.0) return new YoonVector2D(0.0, 0.0);
            return new YoonVector2D(posMm.X / MmPerPixel.X, posMm.Y / MmPerPixel.Y);
        }

        private double ToPixelX(double dx)
        {
            if(MmPerPixel.X == 0.0)   return 0.0;
            return dx / MmPerPixel.X;
        }

        private double ToPixelY(double dy)
        {
            if(MmPerPixel.Y == 0.0)   return 0.0;
            return dy / MmPerPixel.Y;
        }

        private YoonVector2D ToMm(YoonVector2D posPixel)
        {
            if(MmPerPixel.X == 0.0 || MmPerPixel.Y==0.0) return new YoonVector2D(0.0, 0.0);
            return new YoonVector2D(posPixel.X * MmPerPixel.X, posPixel.Y * MmPerPixel.Y);

        }

        private double ToMmX(double dx)
        {
            if(MmPerPixel.X == 0.0)   return 0.0;
            return dx * MmPerPixel.X;
        }

        private double ToMmY(double dy)
        {
            if(MmPerPixel.Y == 0.0)   return 0.0;
            return dy * MmPerPixel.Y;
        }

        public GrobalCoordinateTransform()
        {
            m_pDicInsertPos.Clear();
            foreach (eYoonDirRect nDir in Enum.GetValues(typeof(eYoonDirRect)))
            {
                YoonVector2D vd = new YoonVector2D(0.0, 0.0);
                m_pDicInsertPos.Add(nDir, vd);
            }
            m_pDicCenterPos.Clear();
            foreach (eYoonDirRect nDir in Enum.GetValues(typeof(eYoonDirRect)))
            {
                YoonVector2D vd = new YoonVector2D(0.0, 0.0);
                m_pDicCenterPos.Add(nDir, vd);
            }
        }

        public void SetCameraSetting(YoonVector2D fov, YoonVector2D mmPerPixel)
        {
            FOV = fov;
            MmPerPixel = mmPerPixel;
        }

        public void SetRealCenterPosition(YoonVector2D posCenter, eYoonDirRect dir)
        {
            m_pDicCenterPos[dir] = posCenter;
        }

        public void CalculateGrobalCoordinate(YoonVector2D pixPoint1, YoonVector2D pixPoint2, double dTheta, eYoonDirRect dir)
        {
            m_pDicInsertPos[dir] = CalculateStandardCoordinate(pixPoint1, pixPoint2, dTheta, dir);
            m_pDicCenterPos[dir] = m_pDicInsertPos[dir] + GetCenterOffset(pixPoint1);
        }

        private YoonVector2D GetCenterOffset(YoonVector2D pixPoint)
        {
            if (FOV.X == 0.0 || MmPerPixel.X == 0.0) return new YoonVector2D(0.0, 0.0);
            ////  일반 좌표계는 우측 / 상향으로 증가하는 방향(+)이다.
            ////  Pixel 좌표계는 우측 / 하향으로 증가하는 방향(+)이다.
            double cx = ToMmX(FOV.X / 2.0 - pixPoint.X);
            double cy = ToMmY(pixPoint.Y - FOV.Y / 2.0);
            return new YoonVector2D(cx, cy);
        }

        /// <summary>
        /// 각 Camera의 중심의 실측(mm) 좌표를 가져온다.
        /// 회전중심을 원점(0.0, 0.0)으로 한다.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public YoonVector2D GetRealCenterPosition(eYoonDirRect dir)
        {
            ////  회전중심을 (0.0)으로 하는 실측(mm) 좌표로 Return함.
            return m_pDicCenterPos[dir];
        }

        /// <summary>
        /// 해당 Pixel 좌표에 대한 실측(mm) 좌표를 가져온다.
        /// 회전중심을 원점(0.0, 0.0)으로 한다.
        /// </summary>
        /// <param name="pixPoint1"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public YoonVector2D GetRealPosition(YoonVector2D pixPoint, eYoonDirRect dir)
        {
            ////  회전중심을 (0.0)으로 하는 실측(mm) 좌표로 Return함.
            ////  Pixel 좌표계에서만 사용하므로 GetCenterOffset을 TRUE로 고정한다.
            return m_pDicCenterPos[dir] - GetCenterOffset(pixPoint);
        }

        private YoonVector2D CalculateDistance_XY(YoonVector2D point1, YoonVector2D point2)
        {
            YoonVector2D pos = new YoonVector2D();
            pos.X = Math.Abs(point1.X - point2.X) * MmPerPixel.X;
            pos.Y = Math.Abs(point1.Y - point2.Y) * MmPerPixel.Y;
            return pos;
        }

        private double CalculateDistance_Hypo(YoonVector2D point1, YoonVector2D point2)
        {
            YoonVector2D delta = CalculateDistance_XY(point1, point2);
            return Math.Sqrt(Math.Pow(delta.X, 2.0) + Math.Pow(delta.Y, 2.0));
        }

        private double CalculateRotationCircle_Theta(YoonVector2D point1, YoonVector2D point2)
        {
            YoonVector2D delta = CalculateDistance_XY(point1, point2);
            double hypo = CalculateDistance_Hypo(point1, point2);
            return Math.Asin(Math.Sin(Math.PI / 2) / (hypo / 2) * (delta.X / 2));
        }

        private double CalculateRotationCircle_Radius(YoonVector2D point1, YoonVector2D point2, double dTheta)
        {
            double dHypo = CalculateDistance_Hypo(point1, point2);
            return Math.Abs(Math.Sin(Math.PI / 2) * dHypo / 2 / Math.Sin(dTheta / 2));
        }

        private double CalculateRotationCircle_PreTheta(YoonVector2D point1, YoonVector2D point2, double dTheta, eYoonDirRect dir)
        {
            double dRotationTheta = CalculateRotationCircle_Theta(point1, point2);
            double dPreTheta = 0.0;
            switch(dir)
            {
                case eYoonDirRect.TopRight:
                case eYoonDirRect.BottomLeft:
                    dPreTheta = dRotationTheta - (dTheta / 2);
                    break;
                case eYoonDirRect.TopLeft:
                case eYoonDirRect.BottomRight:
                    dPreTheta = dRotationTheta + (dTheta / 2);
                    break;
                default:
                    break;
            }
            return dPreTheta;
        }

        private double CalculateRotationCircle_PostTheta(YoonVector2D point1, YoonVector2D point2, double dTheta, eYoonDirRect dir)
        {
            double dRotationTheta = CalculateRotationCircle_Theta(point1, point2);
            double dPostTheta = 0.0;
            switch (dir)
            {
                case eYoonDirRect.TopRight:
                case eYoonDirRect.BottomLeft:
                    dPostTheta = dRotationTheta + (dTheta / 2);
                    break;
                case eYoonDirRect.TopLeft:
                case eYoonDirRect.BottomRight:
                    dPostTheta = dRotationTheta - (dTheta / 2);
                    break;
                default:
                    break;
            }
            return dPostTheta;
        }

        private YoonVector2D CalculateNormalCoordinate(YoonVector2D point1, YoonVector2D point2, double dTheta, eYoonDirRect dir)
        {
            YoonVector2D pos = new YoonVector2D();
            pos.X = Math.Cos(CalculateRotationCircle_PreTheta(point1, point2, dTheta, dir))*CalculateRotationCircle_Radius(point1, point2, dTheta);
            pos.Y = Math.Sin(CalculateRotationCircle_PreTheta(point1, point2, dTheta, dir))*CalculateRotationCircle_Radius(point1, point2, dTheta);
            return pos;
        }

        public YoonVector2D CalculateStandardCoordinate(YoonVector2D point1, YoonVector2D point2, double dTheta, eYoonDirRect dir)
        {
            YoonVector2D pos = new YoonVector2D();
            YoonVector2D dNormalPos = CalculateNormalCoordinate(point1, point2, dTheta, dir);
            switch(dir)
            {
                case eYoonDirRect.TopRight:
                    pos.X = dNormalPos.X;
                    pos.Y = dNormalPos.Y;
                    break;
                case eYoonDirRect.TopLeft:
                    pos.X = -dNormalPos.X;
                    pos.Y = dNormalPos.Y;
                    break;
                case eYoonDirRect.BottomLeft:
                    pos.X = -dNormalPos.X;
                    pos.Y = -dNormalPos.Y;
                    break;
                case eYoonDirRect.BottomRight:
                    pos.X = dNormalPos.X;
                    pos.Y = -dNormalPos.Y;
                    break;
                default:
                    break;
            }
            return pos;
        }
    }

    /// <summary>
    /// Align 주요 함수 (1D)
    /// </summary>
    public class Align1D
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

        public Align1D()
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
    public class Align2D
    {
        private const double INVALID_NUM = -10000.0;
        private Dictionary<eYoonDirRect, YoonVector2D> m_fDicPosAlign = new Dictionary<eYoonDirRect,YoonVector2D>();
        private Dictionary<eYoonDirRect, YoonVector2D> m_fDicPosCurrent = new Dictionary<eYoonDirRect,YoonVector2D>();

        public eYoonDirRect LeftCameraDirection { get; private set; } = eYoonDirRect.TopLeft;
        public eYoonDirRect RightCameraDirection { get; private set; } = eYoonDirRect.TopRight;
        public AlignResult AlignResult { get; private set; } = new AlignResult();

        private double ToAngle(double dTheta)
        {
            return 180 * dTheta / Math.PI;
        }

        private double ToTheta(double dAngle)
        {
            return Math.PI * dAngle / 180;
        }

        public Align2D(eYoonDirRect dirLeft, eYoonDirRect dirRight)
        {
            ////  Align 대상위치 초기화
            m_fDicPosAlign.Clear();
            for (eYoonDirRect iDir = 0; iDir < eYoonDirRect.MaxDir; iDir++)
            {
                YoonVector2D vd = new YoonVector2D();
                m_fDicPosAlign.Add(iDir, vd);
            }

            ////  현재 위치 초기화
            m_fDicPosCurrent.Clear();
            for (eYoonDirRect iDir = 0; iDir < eYoonDirRect.MaxDir; iDir++)
            {
                YoonVector2D vd = new YoonVector2D();
                m_fDicPosCurrent.Add(iDir, vd);
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

        public YoonVector2D CalculateXY(YoonVector2D posLeft, YoonVector2D posRight, double theta, eYoonDirRect dirDefault, double scale = 1.0)
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

    #region History Checking 용도의 미사용 Class
    /*
    
    public class PixelCoordinateTransfer : IDisposable
    {
        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.

                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~TCPComm() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            //GC.SuppressFinalize(this);
        }
        #endregion

        private const double INVALID_NUM = -10000.0;

        public double Theta { get; private set; } = 0.0;
        public double RealPitchX { get; private set; } = 0.0;
        public double RealPitchY { get; private set; } = 0.0;
        public double CameraWidth { get; private set; } = 0.0;
        public double CameraHeight { get; private set; } = 0.0;

        private YoonVectorD m_vecFOV = null;
        private YoonVectorD m_vecMmPitch = null;

        private YoonVectorD ViaCenter(YoonVectorD pixPoint)
        {
            if (m_vecFOV.X == 0.0) return new YoonVectorD(0.0, 0.0);
            ////  일반 좌표계는 우측 / 상향으로 증가하는 방향(+)이다.
            ////  Pixel 좌표계는 우측 / 하향으로 증가하는 방향(+)이다.
            double cx = pixPoint.X - m_vecFOV.X / 2.0;
            double cy = m_vecFOV.Y / 2.0 - pixPoint.Y;
            return new YoonVectorD(cx, cy);
        }

        private YoonVectorD ViaOrigin(YoonVectorD vdPoint)
        {
            if (m_vecFOV.X == 0.0) return new YoonVectorD(0.0, 0.0);
            ////  일반 좌표계는 우측 / 상향으로 증가하는 방향(+)이다.
            ////  Pixel 좌표계는 우측 / 하향으로 증가하는 방향(+)이다.
            double x = m_vecFOV.X / 2.0 + vdPoint.X;
            double y = m_vecFOV.Y / 2.0 - vdPoint.Y;
            return new YoonVectorD((x > 0) ? x : 0, (y > 0) ? y : 0);
        }

        public PixelCoordinateTransfer()
        {
            m_vecFOV = new YoonVectorD();
            m_vecMmPitch = new YoonVectorD();
    }

        public void SetFieldOfView(double dWidth, double dHeight)
        {
            CameraWidth = dWidth;
            CameraHeight = dHeight;
            m_vecFOV.X = dWidth;
            m_vecFOV.Y = dHeight;
        }

        public void SetTheta(double dTheta)
        {
            Theta = dTheta;
        }

        public void SetTheta(YoonVectorD pixPoint1, YoonVectorD pixPoint2)
        {
            ////  pixPoint1, pixPoint2를 잇는 선분과 "Y축" 간의 각도를 구해야하므로 ATAN(Y/X)가 아닌 ATAN(X/Y)로 계산한다.
            if (pixPoint2.Y > pixPoint1.Y) Theta = Math.Atan2(pixPoint2.X - pixPoint1.X, pixPoint2.Y - pixPoint1.Y);
            else Theta = Math.Atan2(pixPoint1.X - pixPoint2.X, pixPoint1.Y - pixPoint2.Y);
        }

        public void SetCalibrationPitch(double mmPitchX, double mmPitchY)
        {
            RealPitchX = mmPitchX;
            RealPitchY = mmPitchY;
            m_vecMmPitch.X = mmPitchX;
            m_vecMmPitch.Y = mmPitchY;
        }

        public YoonVectorD GetPixelPosition(YoonVectorD pixPoint)
        {
            ////  Pixel 좌표계를 Center 중심 좌표계로 전환
            YoonVectorD vdPoint = ViaCenter(pixPoint);
            ////  Tilt 값에 맞게 회전시킴
            YoonVectorD vdPoint_Tilt = CalculateTilt(vdPoint, Theta);
            ////  위 값을 Pixel 좌표계로 변환해서 출력
            return ViaOrigin(vdPoint_Tilt);
        }

        public YoonVectorD CalculatePixelResolution(YoonVectorD pixPoint1, YoonVectorD pixPoint2)
        {
            ////  2점 기준으로 Pixel Resultion 값을 계산함.
            YoonVectorD mmPerPixel = new YoonVectorD(INVALID_NUM, INVALID_NUM);
            if (pixPoint1.X != pixPoint2.X) mmPerPixel.X = m_vecMmPitch.X / Math.Abs(pixPoint1.X - pixPoint2.X);
            if (pixPoint1.Y != pixPoint2.Y) mmPerPixel.Y = m_vecMmPitch.Y / Math.Abs(pixPoint1.Y - pixPoint2.Y);
            return mmPerPixel;
        }

        public double GetPixelResolutionX(YoonVectorD pixPoint1, YoonVectorD pixPoint2)
        {
            return CalculatePixelResolution(pixPoint1, pixPoint2).X;
        }

        public double GetPixelResolutionY(YoonVectorD pixPoint1, YoonVectorD pixPoint2)
        {
            return CalculatePixelResolution(pixPoint1, pixPoint2).Y;
        }

        private YoonVectorD CalculateTilt(YoonVectorD vdPoint, double theta)
        {
            ////  YoonAlign 알고리즘은 Object 위치 기준이 "Y축"이므로 Theta는 무조건 반대로 적용해야 함
            theta *= -1; 

            double dx = vdPoint.X * Math.Cos(theta) - vdPoint.Y * Math.Sin(theta);
            double dy = vdPoint.X * Math.Sin(theta) + vdPoint.Y * Math.Cos(theta);
            return new YoonVectorD(dx, dy);
        }
    }

    /// <summary>
    /// 아신 방식 AUTO CALIBRATION이 적용되어진 일체형 클래스임.
    /// 기존 Align2D 또는 Align4D와 다른 방식으로 동작함
    /// </summary>
    public class Align2D_Asin
    {
        #region CalibrationData 저장 영역
        /// <summary>
        /// Align Calibration 결과값 담는 변수 클래스
        /// </summary>
        public class CalibrationData2D
        {
            // 증가 방향
            private double fResultX_increase;
            private double fResultY_increase;
            private double fResultT_clockwise;
            private double fResultTX_clockwise;
            private double fResultTY_clockwise;
            // 감소 방향
            private double fResultX_decrease;
            private double fResultY_decrease;
            private double fResultT_anticlockwise;
            private double fResultTX_anticlockwise;
            private double fResultTY_anticlockwise;
            public double ResultX_Inc
            {
                get { return fResultX_increase; }
                set { fResultX_increase = value; }
            }
            public double ResultY_Inc
            {
                get { return fResultY_increase; }
                set { fResultY_increase = value; }
            }
            public double ResultT_Clw
            {
                get { return fResultT_clockwise; }
                set { fResultT_clockwise = value; }
            }
            public double ResultTX_Clw
            {
                get { return fResultTX_clockwise; }
                set { fResultTX_clockwise = value; }
            }
            public double ResultTY_Clw
            {
                get { return fResultTY_clockwise; }
                set { fResultTY_clockwise = value; }
            }
            public double ResultX_Dec
            {
                get { return fResultX_decrease; }
                set { fResultX_decrease = value; }
            }
            public double ResultY_Dec
            {
                get { return fResultY_decrease; }
                set { fResultY_decrease = value; }
            }
            public double ResultT_Aclw
            {
                get { return fResultT_anticlockwise; }
                set { fResultT_anticlockwise = value; }
            }
            public double ResultTX_Aclw
            {
                get { return fResultTX_anticlockwise; }
                set { fResultTX_anticlockwise = value; }
            }
            public double ResultTY_Aclw
            {
                get { return fResultTY_anticlockwise; }
                set { fResultTY_anticlockwise = value; }
            }
            public CalibrationData2D()
            {
                Init();
            }
            public void Init()
            {
                fResultX_increase = 0.0;
                fResultY_increase = 0.0;
                fResultT_clockwise = 1.0;
                fResultTX_clockwise = 0.0;
                fResultTY_clockwise = 0.0;
                fResultX_decrease = 0.0;
                fResultY_decrease = 0.0;
                fResultT_anticlockwise = 1.0;
                fResultTX_anticlockwise = 0.0;
                fResultTY_anticlockwise = 0.0;
            }
        }
        #endregion

        #region 변수 정의부
        public const int MAX_DIR_LINE = 2;
        public List<YoonVectorD> m_fListAlignObject2D = new List<YoonVectorD>();
        public CalibrationData2D m_fAutoCalibData = new CalibrationData2D();
        public ResultYoonAlign m_fAlignResult = new ResultYoonAlign();
        public double m_mmPerPixel = 0.0;
        public double m_markToMarkDistance = 0.0;
        #endregion

        public Align2D_Asin()
        {
            m_fListAlignObject2D.Clear();
            for (int i = 0; i < MAX_DIR_LINE; i++)
            {
                YoonVectorD vd = new YoonVectorD(0.00, 0.00);
                m_fListAlignObject2D.Add(vd);
            }
        }
        public void SetMmPerPixel(double mmPerPixel)
        {
            m_mmPerPixel = mmPerPixel;
        }
        public void SetMarkToMarkDistance(double dMarkToMark)
        {
            m_markToMarkDistance = dMarkToMark;
        }
        public void SetAutoCalibrationData(CalibrationData2D fAutoCalData)
        {
            m_fAutoCalibData = fAutoCalData;
        }
        public void SetObjectPosition2D(YoonVectorD objectLeft, YoonVectorD objectRight)
        {
            m_fListAlignObject2D[(int)eDirYoonAlignX.Left] = objectLeft;
            m_fListAlignObject2D[(int)eDirYoonAlignX.Right] = objectRight;
        }

        public double CalculateTheta2D_None(YoonVectorD markLeft, YoonVectorD objectLeft, YoonVectorD markRight, YoonVectorD objectRight, int dir)
        {
            if (m_mmPerPixel == 0.0 || m_markToMarkDistance == 0.0) return 0.0;

            ////  목표(Object) 위치를 지정한다.
            double markerToMarker = m_markToMarkDistance / m_mmPerPixel;
            SetObjectPosition2D(objectLeft, objectRight);
            ////  두 목표 위치로부터 메인 스테이지 각도를 계산한다.
            ////  목표 위치간 각도는 항상 같은 값이여야 하며, 기준 위치간 X 좌표 거리는 언제나 Mark 간 거리와 같아야한다.
            double diffObjectY = objectRight.y - objectLeft.y;
            double diffObjectX = markerToMarker;
            double diffObjectAngle = Math.Atan2(diffObjectY, diffObjectX);
            ////  두 Mark 간 각도를 계산한다.
            double diffMarkY = markRight.y - markLeft.y;
            double diffSlope = markerToMarker;
            double diffMarkAngle = Math.Asin(diffMarkY / diffSlope);
            ////  Mark간 각도 및 목표위치간 각도는 항상 -PI/2 ~ PI/2 의 범위를 갖게 된다.
            ////  따라서 diffAngle은 -PI ~ PI의 범위를 갖는다.
            double diffAngle = diffObjectAngle - diffMarkAngle;
            ////  Theta 계산시 방향 및 AutoCalibration 값을 반영한다.
            if (dir < 0) dir = -1;
            else dir = 1;
            double theta = dir * diffAngle * (180 / Math.PI);
            m_fAlignResult.ResultT = theta;
            return theta;
        }

        public YoonVectorD CalculateXY2D_None(YoonVectorD markLeft, YoonVectorD objectLeft, YoonVectorD markRight, YoonVectorD objectRight, int dir)
        {
            if (m_mmPerPixel == 0.0 || m_markToMarkDistance == 0.0) return new YoonVectorD(0.0, 0.0);

            ////  목표(Object) 위치를 지정한다.
            double markerToMarker = m_markToMarkDistance / m_mmPerPixel;
            ////  두 목표 위치간 거리(Offset)를 계산한다.
            YoonVectorD deltaLeft = new YoonVectorD(objectLeft - markLeft);
            YoonVectorD deltaRight = new YoonVectorD(objectRight - markRight);
            double resultX = (deltaLeft.x + deltaRight.x) / 2.0;
            double resultY = (deltaLeft.y + deltaRight.y) / 2.0;
            ////  최종 Offset 산출시 방향 및 AutoCalibration 값을 반영한다.
            if (dir < 0) dir = -1;
            else dir = 1;
            YoonVectorD xyOffset = new YoonVectorD();
            xyOffset.x = resultX * m_mmPerPixel;
            xyOffset.y = resultY * m_mmPerPixel;
            m_fAlignResult.ResultX = xyOffset.x;
            m_fAlignResult.ResultT = xyOffset.y;
            return xyOffset;
        }

        public double CalculateTheta2D_AutoCal(YoonVectorD markLeft, YoonVectorD objectLeft, YoonVectorD markRight, YoonVectorD objectRight, int dir)
        {
            if (m_mmPerPixel == 0.0 || m_markToMarkDistance == 0.0) return 0.0;

            ////  목표(Object) 위치를 지정한다.
            double markerToMarker = m_markToMarkDistance / m_mmPerPixel;
            SetObjectPosition2D(objectLeft, objectRight);
            ////  두 목표 위치로부터 메인 스테이지 각도를 계산한다.
            ////  목표 위치간 각도는 항상 같은 값이여야 하며, 기준 위치간 X 좌표 거리는 언제나 Mark 간 거리와 같아야한다.
            double diffObjectY = objectRight.y - objectLeft.y;
            double diffObjectX = markerToMarker;
            double diffObjectAngle = Math.Atan2(diffObjectY, diffObjectX);
            ////  두 Mark 간 각도를 계산한다.
            double diffMarkY = markRight.y - markLeft.y;
            double diffSlope = markerToMarker;
            double diffMarkAngle = Math.Asin(diffMarkY / diffSlope);
            ////  Mark간 각도 및 목표위치간 각도는 항상 -PI/2 ~ PI/2 의 범위를 갖게 된다.
            ////  따라서 diffAngle은 -PI ~ PI의 범위를 갖는다.
            double diffAngle = diffObjectAngle - diffMarkAngle;
            ////  Theta 계산시 방향 및 AutoCalibration 값을 반영한다.
            if (dir < 0) dir = -1;
            else dir = 1;
            double theta = dir * diffAngle * (180 / Math.PI);
            if (theta > 0) theta *= m_fAutoCalibData.ResultT_Clw;
            else theta *= m_fAutoCalibData.ResultT_Aclw;
            m_fAlignResult.ResultT = theta;
            return theta;
        }

        public YoonVectorD CalculateXY2D_AutoCal(YoonVectorD markLeft, YoonVectorD objectLeft, YoonVectorD markRight, YoonVectorD objectRight, int dir, double theta)
        {
            if (m_mmPerPixel == 0.0 || m_markToMarkDistance == 0.0) return new YoonVectorD(0.0, 0.0);

            ////  목표(Object) 위치를 지정한다.
            double markerToMarker = m_markToMarkDistance / m_mmPerPixel;
            ////  두 목표 위치간 거리(Offset)를 계산한다.
            YoonVectorD deltaLeft = new YoonVectorD(objectLeft - markLeft);
            YoonVectorD deltaRight = new YoonVectorD(objectRight - markRight);
            double resultX = (deltaLeft.x + deltaRight.x) / 2.0;
            double resultY = (deltaLeft.y + deltaRight.y) / 2.0;
            ////  최종 Offset 산출시 방향 및 AutoCalibration 값을 반영한다.
            if (dir < 0) dir = -1;
            else dir = 1;
            YoonVectorD xyOffset = new YoonVectorD();
            xyOffset.x = resultX * m_mmPerPixel;
            xyOffset.y = resultY * m_mmPerPixel;
            if (theta > 0)
            {
                xyOffset.x += dir * theta * m_fAutoCalibData.ResultTX_Clw;
                xyOffset.y += dir * theta * m_fAutoCalibData.ResultTY_Clw;
            }
            else

            {
                xyOffset.x += dir * theta * m_fAutoCalibData.ResultTX_Aclw;
                xyOffset.y += dir * theta * m_fAutoCalibData.ResultTY_Aclw;
            }
            m_fAlignResult.ResultX = xyOffset.x;
            m_fAlignResult.ResultT = xyOffset.y;
            return xyOffset;
        }

        public bool CalculateAutoCalibration2D(YoonVectorD markLeft, YoonVectorD objectLeft, YoonVectorD markRight, YoonVectorD objectRight, eDirYoonAlignClock dirClock, double dMovement)
        {
            if (m_mmPerPixel == 0.0 || m_markToMarkDistance == 0.0) return false;

            ////  Align 알고리즘 실행
            int dirAlign = 1;
            double theta = CalculateTheta2D_None(markLeft, objectLeft, markRight, objectRight, dirAlign);
            YoonVectorD offsetXY = CalculateXY2D_None(markLeft, objectLeft, markRight, objectRight, dirAlign);
            ////  Auto Calibration Result 추가
            double dMovementRadian = 0.0;
            switch (dirClock)
            {
                case eDirYoonAlignClock.Left:
                    m_fAutoCalibData.ResultX_Dec = Math.Abs(dMovement / offsetXY.x);
                    break;
                case eDirYoonAlignClock.Right:
                    m_fAutoCalibData.ResultX_Inc = Math.Abs(dMovement / offsetXY.x);
                    break;
                case eDirYoonAlignClock.Top:
                    m_fAutoCalibData.ResultY_Inc = Math.Abs(dMovement / offsetXY.y);
                    break;
                case eDirYoonAlignClock.Bottom:
                    m_fAutoCalibData.ResultY_Dec = Math.Abs(dMovement / offsetXY.y);
                    break;
                case eDirYoonAlignClock.Clock:
                    dMovementRadian = dMovement * 180 / Math.PI;
                    m_fAutoCalibData.ResultT_Clw = Math.Abs(dMovementRadian / theta);
                    m_fAutoCalibData.ResultTX_Clw = Math.Abs(offsetXY.x / theta);
                    m_fAutoCalibData.ResultTY_Clw = Math.Abs(offsetXY.y / theta);
                    break;
                case eDirYoonAlignClock.AntiClock:
                    dMovementRadian = dMovement * 180 / Math.PI;
                    m_fAutoCalibData.ResultTY_Aclw = Math.Abs(dMovementRadian / theta);
                    m_fAutoCalibData.ResultTX_Aclw = Math.Abs(offsetXY.x / theta);
                    m_fAutoCalibData.ResultTY_Aclw = Math.Abs(offsetXY.y / theta);
                    break;
                default:
                    break;
            }
            return true;
        }

        public void CalculateMovement2D_AutoCal(YoonVectorD markLeft, YoonVectorD objectLeft, YoonVectorD markRight, YoonVectorD objectRight)
        {
            ////  Align 알고리즘 실행
            int dirAlign = 1;
            double theta = CalculateTheta2D_AutoCal(markLeft, objectLeft, markRight, objectRight, dirAlign);
            YoonVectorD offsetXY = CalculateXY2D_AutoCal(markLeft, objectLeft, markRight, objectRight, dirAlign, theta);
        }
    }

    /// <summary>
    /// 구방식의 회전 Calibration Align 함수 (4D)
    /// </summary>
    public class Align4D_Rotate
    {
        #region RotationData 저쟝 영역
        public class RotationData
        {
            // 회전 중심값
            private double fCenterX;
            private double fCenterY;
            public double CenterX
            {
                get { return fCenterX; }
                set { fCenterX = value; }
            }
            public double CenterY
            {
                get { return fCenterY; }
                set { fCenterY = value; }
            }
            public RotationData()
            {
                Init();
            }
            public void Init()
            {
                fCenterX = 0.0;
                fCenterY = 0.0;
            }
        }
        #endregion

        public const int MAX_DIR_RECT = 4;
        public List<YoonVectorD> m_fListAlignObject4D = new List<YoonVectorD>();
        public List<RotationData> m_fListRotationData4D = new List<RotationData>();
        public List<YoonVectorD> m_fListRotation_Clk = new List<YoonVectorD>();
        public List<YoonVectorD> m_fListRotation_Aclk = new List<YoonVectorD>();
        public double m_mmPerPixel = 0.0;
        public Align4D_Rotate()
        {
            m_fListAlignObject4D.Clear();
            for (int i = 0; i < MAX_DIR_RECT; i++)
            {
                YoonVectorD vd = new YoonVectorD(0.00, 0.00);
                m_fListAlignObject4D.Add(vd);
            }
            m_fListRotation_Clk.Clear();
            for (int i = 0; i < MAX_DIR_RECT; i++)
            {
                YoonVectorD vd = new YoonVectorD(0.00, 0.00);
                m_fListRotation_Clk.Add(vd);
            }
            m_fListRotation_Aclk.Clear();
            for (int i = 0; i < MAX_DIR_RECT; i++)
            {
                YoonVectorD vd = new YoonVectorD(0.00, 0.00);
                m_fListRotation_Aclk.Add(vd);
            }
        }
        public void SetRotationCalibrationData(RotationData[] fRotationData)
        {
            if (fRotationData.Length != MAX_DIR_RECT) return;

            for (int i = 0; i < MAX_DIR_RECT; i++)
            {
                m_fListRotationData4D[i] = fRotationData[i];
            }
        }
        public void SetObjectPosition4D(YoonVectorD objectTL, YoonVectorD objectTR, YoonVectorD objectBL, YoonVectorD objectBR)
        {
            m_fListAlignObject4D[(int)eYoonDirRect.TopLeft] = objectTL;
            m_fListAlignObject4D[(int)eYoonDirRect.TopRight] = objectTR;
            m_fListAlignObject4D[(int)eYoonDirRect.BottomLeft] = objectBL;
            m_fListAlignObject4D[(int)eYoonDirRect.BottomRight] = objectBR;
        }
        public double CalculateTheta4D_None(YoonVectorD[] markPos, YoonVectorD[] objectPos, int dir)
        {
            ////
            return 0.0;
        }
        public YoonVectorD CalculateXY4D_None(YoonVectorD[] markPos, YoonVectorD[] objectPos, int dir)
        {
            ////
            return new YoonVectorD();
        }
        public bool CalculateRotationCalibration4D(YoonVectorD[] markRect, YoonVectorD[] objectRect, eDirYoonAlignClock dirClock, double dAngle, bool isContinued)
        {
            if (m_mmPerPixel == 0.0) return false;
            ////  임시 Buffer 생성 및 초기화
            YoonVectorD[] dCenterPoint = new YoonVectorD[MAX_DIR_RECT];
            for (int iDir = 0; iDir < MAX_DIR_RECT; iDir++)
            {
                dCenterPoint[iDir] = new YoonVectorD(0.0, 0.0);
            }
            ////  계산 및 결과값 출력
            switch (dirClock)
            {
                case eDirYoonAlignClock.Clock:
                    for (int iDir = 0; iDir < MAX_DIR_RECT; iDir++)
                    {
                        m_fListRotation_Clk[iDir] = CalculateRotationFuction(markRect[iDir], objectRect[iDir], dAngle);
                        ////  Result 값 저장을 위한 임시 Buffer
                        dCenterPoint[iDir] = m_fListRotation_Clk[iDir];
                    }
                    break;
                case eDirYoonAlignClock.AntiClock:
                    for (int iDir = 0; iDir < MAX_DIR_RECT; iDir++)
                    {
                        m_fListRotation_Aclk[iDir] = CalculateRotationFuction(markRect[iDir], objectRect[iDir], dAngle);
                        ////  Result 값 저장을 위한 임시 Buffer
                        dCenterPoint[iDir] = m_fListRotation_Aclk[iDir];
                    }
                    break;
                default:
                    break;

            }
            ////  결과값 출력
            for (int iDir = 0; iDir < MAX_DIR_RECT; iDir++)
            {
                if (isContinued == false)
                {
                    m_fListRotationData4D[iDir].CenterX = m_fListRotation_Clk[iDir].x;
                    m_fListRotationData4D[iDir].CenterY = m_fListRotation_Clk[iDir].y;
                }
                else
                {
                    m_fListRotationData4D[iDir].CenterX += m_fListRotation_Clk[iDir].x;
                    m_fListRotationData4D[iDir].CenterX /= 2;
                    m_fListRotationData4D[iDir].CenterY += m_fListRotation_Clk[iDir].y;
                    m_fListRotationData4D[iDir].CenterY /= 2;
                }
            }
            return true;
        }

        public YoonVectorD CalculateRotationFuction(YoonVectorD object1, YoonVectorD object2, double dAngle)
        {
            ////  주요 변수 선언 및 정리
            double sinT = Math.Sin(dAngle);
            double cosT = Math.Cos(dAngle);
            double sinT2 = Math.Pow(sinT, 2.0);
            double cosT2 = Math.Pow(cosT, 2.0);
            double sinTcosT = sinT * cosT;
            double cosT_1 = cosT - 1;
            double x1 = object1.x;
            double y1 = object1.y;
            double x2 = object2.x;
            double y2 = object2.y;
            YoonVectorD dCenter = new YoonVectorD(0.0, 0.0);
            ////  회전중심 공식 및 계산 결과 출력
            dCenter.x = -cosT * x1 / 2 - sinT2 * x1 / (2 * cosT_1) + sinT * y1 / 2 - sinTcosT * y1 / (2 * cosT_1) + x2 / 2 + sinT * y2 / (2 * cosT_1);
            dCenter.y = sinTcosT * x1 / (2 * cosT_1) - sinT * x1 / 2 - sinT2 * y1 / (2 * cosT_1) - cosT * y1 / 2 - sinT * x2 / (2 * cosT_1) + y2 / 2;
            ////  결과값 Return
            return dCenter;
        }
    }
    */
    #endregion

}
