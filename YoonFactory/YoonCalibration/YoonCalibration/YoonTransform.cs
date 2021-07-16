using System;
using System.Collections.Generic;

namespace YoonFactory.Calibration
{
    public class YoonTransform : IDisposable
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
        ~YoonTransform()
        {
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

        private Dictionary<eYoonDir2D, YoonVector2D> m_pDicInsertPos = new Dictionary<eYoonDir2D, YoonVector2D>();
        private Dictionary<eYoonDir2D, YoonVector2D> m_pDicCenterPos = new Dictionary<eYoonDir2D, YoonVector2D>();

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
            if (MmPerPixel.X == 0.0 || MmPerPixel.Y == 0.0) return new YoonVector2D(0.0, 0.0);
            return new YoonVector2D(posMm.X / MmPerPixel.X, posMm.Y / MmPerPixel.Y);
        }

        private double ToPixelX(double dx)
        {
            if (MmPerPixel.X == 0.0) return 0.0;
            return dx / MmPerPixel.X;
        }

        private double ToPixelY(double dy)
        {
            if (MmPerPixel.Y == 0.0) return 0.0;
            return dy / MmPerPixel.Y;
        }

        private YoonVector2D ToMm(YoonVector2D posPixel)
        {
            if (MmPerPixel.X == 0.0 || MmPerPixel.Y == 0.0) return new YoonVector2D(0.0, 0.0);
            return new YoonVector2D(posPixel.X * MmPerPixel.X, posPixel.Y * MmPerPixel.Y);

        }

        private double ToMmX(double dx)
        {
            if (MmPerPixel.X == 0.0) return 0.0;
            return dx * MmPerPixel.X;
        }

        private double ToMmY(double dy)
        {
            if (MmPerPixel.Y == 0.0) return 0.0;
            return dy * MmPerPixel.Y;
        }

        public YoonTransform()
        {
            m_pDicInsertPos.Clear();
            foreach (eYoonDir2D nDir in YoonDirFactory.GetSquareDirections())
            {
                YoonVector2D vd = new YoonVector2D(0.0, 0.0);
                m_pDicInsertPos.Add(nDir, vd);
            }
            m_pDicCenterPos.Clear();
            foreach (eYoonDir2D nDir in YoonDirFactory.GetSquareDirections())
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

        public void SetRealCenterPosition(YoonVector2D posCenter, eYoonDir2D dir)
        {
            m_pDicCenterPos[dir] = posCenter;
        }

        public void CalculateGrobalCoordinate(YoonVector2D pixPoint1, YoonVector2D pixPoint2, double dTheta, eYoonDir2D dir)
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
        public YoonVector2D GetRealCenterPosition(eYoonDir2D dir)
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
        public YoonVector2D GetRealPosition(YoonVector2D pixPoint, eYoonDir2D dir)
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

        private double CalculateRotationCircle_PreTheta(YoonVector2D point1, YoonVector2D point2, double dTheta, eYoonDir2D dir)
        {
            double dRotationTheta = CalculateRotationCircle_Theta(point1, point2);
            double dPreTheta = 0.0;
            switch (dir)
            {
                case eYoonDir2D.TopRight:
                case eYoonDir2D.BottomLeft:
                    dPreTheta = dRotationTheta - (dTheta / 2);
                    break;
                case eYoonDir2D.TopLeft:
                case eYoonDir2D.BottomRight:
                    dPreTheta = dRotationTheta + (dTheta / 2);
                    break;
                default:
                    break;
            }
            return dPreTheta;
        }

        private double CalculateRotationCircle_PostTheta(YoonVector2D point1, YoonVector2D point2, double dTheta, eYoonDir2D dir)
        {
            double dRotationTheta = CalculateRotationCircle_Theta(point1, point2);
            double dPostTheta = 0.0;
            switch (dir)
            {
                case eYoonDir2D.TopRight:
                case eYoonDir2D.BottomLeft:
                    dPostTheta = dRotationTheta + (dTheta / 2);
                    break;
                case eYoonDir2D.TopLeft:
                case eYoonDir2D.BottomRight:
                    dPostTheta = dRotationTheta - (dTheta / 2);
                    break;
                default:
                    break;
            }
            return dPostTheta;
        }

        private YoonVector2D CalculateNormalCoordinate(YoonVector2D point1, YoonVector2D point2, double dTheta, eYoonDir2D dir)
        {
            YoonVector2D pos = new YoonVector2D();
            pos.X = Math.Cos(CalculateRotationCircle_PreTheta(point1, point2, dTheta, dir)) * CalculateRotationCircle_Radius(point1, point2, dTheta);
            pos.Y = Math.Sin(CalculateRotationCircle_PreTheta(point1, point2, dTheta, dir)) * CalculateRotationCircle_Radius(point1, point2, dTheta);
            return pos;
        }

        public YoonVector2D CalculateStandardCoordinate(YoonVector2D point1, YoonVector2D point2, double dTheta, eYoonDir2D dir)
        {
            YoonVector2D pos = new YoonVector2D();
            YoonVector2D dNormalPos = CalculateNormalCoordinate(point1, point2, dTheta, dir);
            switch (dir)
            {
                case eYoonDir2D.TopRight:
                    pos.X = dNormalPos.X;
                    pos.Y = dNormalPos.Y;
                    break;
                case eYoonDir2D.TopLeft:
                    pos.X = -dNormalPos.X;
                    pos.Y = dNormalPos.Y;
                    break;
                case eYoonDir2D.BottomLeft:
                    pos.X = -dNormalPos.X;
                    pos.Y = -dNormalPos.Y;
                    break;
                case eYoonDir2D.BottomRight:
                    pos.X = dNormalPos.X;
                    pos.Y = -dNormalPos.Y;
                    break;
                default:
                    break;
            }
            return pos;
        }
    }

}
