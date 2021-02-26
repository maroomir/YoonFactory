using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace YoonFactory.Calibration
{
    public class YoonRotationCalib1D : IDisposable // 1개의 Target 위치에 대한 Rotation Calibration
    {
        protected enum eStepYoonCalibration
        {
            Wait = -1,
            Init,
            Origin,
            TopLeft,
            Top,
            TopRight,
            Right,
            BottomRight,
            Bottom,
            BottomLeft,
            Left,
            Rotate,
            OppoRotate,
            TurnBack,
            CalculateResolution,
            CalculateRotate,
            Finish,
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    m_pGCT.Dispose();
                    m_pThread.Abort();
                    Thread.Sleep(100);
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                Result = null;
                m_pDicDetectionPoint.Clear();
                m_pDicDetectionPoint = null;
                m_pDicCurrentReceiveFlag.Clear();
                m_pDicCurrentReceiveFlag = null;

                m_pGCT = null;
                m_pThread = null;

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        ~YoonRotationCalib1D()
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

        public bool IsCompleted { get; private set; } = false;
        public RotationCalibResult Result { get; private set; } = new RotationCalibResult();
        public eYoonDir2D AlignmentTargetLocation1 { get; private set; } = eYoonDir2D.TopLeft;
        public eYoonDir2D AlignmentTargetLocation2 { get; private set; } = eYoonDir2D.TopRight;
        public eYoonDir2D CalibrationDefaultLocation { get; private set; } = eYoonDir2D.TopLeft;
        public double RotationDegree { get; set; } = 0.0;
        public double CameraPixelWidth { get; set; } = 0.0;
        public double CameraPixelHeight { get; set; } = 0.0;
        public bool IsReverseCoordinateX { get; set; } = false;
        public bool IsReverseCoordinateY { get; set; } = false;
        public bool IsFlipCoordinateXY { get; set; } = false;

        #region 내부 사용 Parameter
        private const double INVALID_NUM = -10000.0;
        // Point 설정 Param
        private Dictionary<eYoonDir2D, bool> m_pDicCurrentReceiveFlag = new Dictionary<eYoonDir2D, bool>();
        private YoonCartD m_pPoseRequest = new YoonCartD();
        // Align Calibration을 위한 Class
        private Thread m_pThread = null;
        private YoonTransform m_pGCT = new YoonTransform();
        // Process Calibration 관련 설정
        private Dictionary<eYoonDir2D, Dictionary<eYoonDir2D, YoonVector2D>> m_pDicDetectionPoint = new Dictionary<eYoonDir2D, Dictionary<eYoonDir2D, YoonVector2D>>();
        private YoonVector2D m_vecRealPitch = new YoonVector2D();
        private YoonVector2D m_vecInitPixelSize = new YoonVector2D(INVALID_NUM, INVALID_NUM);
        // 기타 설정
        private string m_strConfigFilePath = "";
        private bool m_bFlagInit = false;
        #endregion

        public event PoseSendCallback OnMovementPoseSendEvent;  // 로봇 Pose를 보냄
        public event GrapRequestCallback OnGrapRequestEvent;

        public void OnCurrentPositionReceive(object sender, CalibPointArgs e)   // Pattern 대상 Point를 보냄
        {
            if (e.DeviceDirection != AlignmentTargetLocation1 || e.DeviceDirection != AlignmentTargetLocation2) return;

            m_pDicCurrentReceiveFlag[e.DeviceDirection] = true;
            m_pDicDetectionPoint[e.DeviceDirection][e.GrapDirection] = e.Point;  // 위치 값은 주어진 "DirCompass"에 맞게 저장됨
        }

        public YoonRotationCalib1D(eYoonDir2D nDir1, eYoonDir2D nDir2)
        {
            AlignmentTargetLocation1 = nDir1;
            AlignmentTargetLocation2 = nDir2;

            ////  Dictionary 초기화
            m_pDicDetectionPoint.Clear();
            foreach (eYoonDir2D nDir in YoonDirFactory.GetSquareDirections())
            {
                Dictionary<eYoonDir2D, YoonVector2D> pDicPoint = new Dictionary<eYoonDir2D, YoonVector2D>();
                pDicPoint.Clear();
                foreach (eYoonDir2D jDir in Enum.GetValues(typeof(eYoonDir2D)))
                {
                    pDicPoint.Add(jDir, new YoonVector2D(INVALID_NUM, INVALID_NUM));
                }
                m_pDicDetectionPoint.Add(nDir, pDicPoint);
            }
            m_pDicCurrentReceiveFlag.Clear();
            m_pDicCurrentReceiveFlag.Add(nDir1, false);
            m_pDicCurrentReceiveFlag.Add(nDir2, false);

            ////  File 경로 초기화
            m_strConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory", "RotCalib1D.ini");
        }

        private void FlipCoordinate(ref YoonVector2D pVecRealPos)
        {
            if (IsFlipCoordinateXY)
            {
                double dY = pVecRealPos.X;
                double dX = pVecRealPos.Y;
                pVecRealPos.X = dX;
                pVecRealPos.Y = dY;
                pVecRealPos.W = -pVecRealPos.W;
            }
        }

        private void ReverseCoordinate(ref YoonVector2D pVecRealPos)
        {
            pVecRealPos.X *= ((IsReverseCoordinateX) ? -1 : 1);
            pVecRealPos.Y *= ((IsReverseCoordinateY) ? -1 : 1);
        }

        public void InitRealPosition(YoonCartD pRealPos, YoonVector2D vecPixelSize)
        {
            //// 사전 Data 입력 확인
            if (CameraPixelWidth <= 0 || CameraPixelHeight <= 0 || vecPixelSize.X <= 0 || vecPixelSize.Y <= 0) return;

            //// 멤버변수 업데이트
            m_vecInitPixelSize = vecPixelSize.Clone() as YoonVector2D;
            //// 현재 로봇/스테이지의 기구적 Position (스테이지 / 로봇 원점 기준)
            m_pPoseRequest = pRealPos;
            //// Camera FOV 기준으로 Calibration시 이동 간격 계산
            m_vecRealPitch.X = CameraPixelWidth * m_vecInitPixelSize.X;   // mm per pixel
            m_vecRealPitch.Y = CameraPixelHeight * m_vecInitPixelSize.Y;  // mm per pixel
            //// Reverse 및 Swarp에 따른 좌표이동 변경
            FlipCoordinate(ref m_vecRealPitch);
            ReverseCoordinate(ref m_vecRealPitch);    // Swarp를 먼저해서 좌표를 바꿔주고 Reverse 체크할 것
            m_bFlagInit = true;
        }

        public bool StartProcess(eYoonDir2D nDir, string strThreadName = "Rotation")
        {
            if (!m_bFlagInit || m_vecInitPixelSize.X <= 0 || m_vecInitPixelSize.Y <= 0)
                return false;
            if (nDir != AlignmentTargetLocation1 && nDir != AlignmentTargetLocation2)
                return false;

            m_bFlagInit = false;
            CalibrationDefaultLocation = nDir;
            ////  Thread 시작
            try
            {
                m_pThread = new Thread(new ThreadStart(ProcessCalibration));
                m_pThread.Name = "Calibration Thread [" + strThreadName + "]";
                m_pThread.Start();
            }
            catch (ThreadStartException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        private void ProcessCalibration()
        {
            eStepYoonCalibration nJobStep = eStepYoonCalibration.Init;
            eStepYoonCalibration nJobStepBK = eStepYoonCalibration.Wait;
            YoonVector2D vecCameraCenter = new YoonVector2D(CameraPixelWidth / 2, CameraPixelHeight / 2);
            YoonCartD cartPosOrigin = null;
            bool bPlay = true;
            bool bCheckError = false;

            while (bPlay)
            {
                Thread.Sleep(100);

                switch (nJobStep)
                {
                    case eStepYoonCalibration.Wait:
                        if (m_pDicCurrentReceiveFlag.Values.Any(x => x == false))
                            break;
                        //// Flag 초기화
                        foreach (eYoonDir2D nDir in m_pDicCurrentReceiveFlag.Keys)
                            m_pDicCurrentReceiveFlag[nDir] = false;
                        //// Wait 종료 후 업무 분배
                        switch (nJobStepBK)
                        {
                            case eStepYoonCalibration.Init:
                                nJobStep = eStepYoonCalibration.Origin;
                                break;
                            case eStepYoonCalibration.Origin:
                                nJobStep = eStepYoonCalibration.TopLeft;
                                break;
                            case eStepYoonCalibration.TopLeft:
                                nJobStep = eStepYoonCalibration.Top;
                                break;
                            case eStepYoonCalibration.Top:
                                nJobStep = eStepYoonCalibration.TopRight;
                                break;
                            case eStepYoonCalibration.TopRight:
                                nJobStep = eStepYoonCalibration.Right;
                                break;
                            case eStepYoonCalibration.Right:
                                nJobStep = eStepYoonCalibration.BottomRight;
                                break;
                            case eStepYoonCalibration.BottomRight:
                                nJobStep = eStepYoonCalibration.Bottom;
                                break;
                            case eStepYoonCalibration.Bottom:
                                nJobStep = eStepYoonCalibration.BottomLeft;
                                break;
                            case eStepYoonCalibration.BottomLeft:
                                nJobStep = eStepYoonCalibration.Left;
                                break;
                            case eStepYoonCalibration.Left:
                                nJobStep = eStepYoonCalibration.CalculateResolution;
                                break;
                            case eStepYoonCalibration.TurnBack:
                                nJobStep = eStepYoonCalibration.Rotate;
                                break;
                            case eStepYoonCalibration.Rotate:
                                nJobStep = eStepYoonCalibration.OppoRotate;
                                break;
                            case eStepYoonCalibration.OppoRotate:
                                nJobStep = eStepYoonCalibration.CalculateRotate;
                                break;
                        }
                        break;
                    case eStepYoonCalibration.Init:
                        nJobStepBK = nJobStep;
                        //// Detection Point 초기화
                        foreach (eYoonDir2D iDir in m_pDicDetectionPoint.Keys)
                        {
                            foreach (eYoonDir2D jDir in m_pDicDetectionPoint[iDir].Keys)
                            {
                                m_pDicDetectionPoint[iDir][jDir].X = INVALID_NUM;
                                m_pDicDetectionPoint[iDir][jDir].Y = INVALID_NUM;
                            }
                        }
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Center, m_pPoseRequest));
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Center));
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Origin:
                        nJobStepBK = nJobStep;
                        YoonVector2D vecOffset = vecCameraCenter - m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.Center];
                        vecOffset.X *= m_vecInitPixelSize.X;   //  mm per pixel
                        vecOffset.Y *= m_vecInitPixelSize.Y;
                        m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.Center].X = INVALID_NUM;
                        m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.Center].Y = INVALID_NUM;
                        //// 이동거리 산출
                        YoonVector2D vecMovementToCenter = vecOffset / 3; // 9-Point Calibration이므로 3으로 나눔
                        FlipCoordinate(ref vecMovementToCenter);
                        ReverseCoordinate(ref vecMovementToCenter);
                        //// 이동위치 산출
                        m_pPoseRequest += vecMovementToCenter; // 초기 -> 원점위치로 변경
                        cartPosOrigin = m_pPoseRequest.Clone() as YoonCartD; // 원점 위치 복사해놓기
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Center, m_pPoseRequest)); // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Center)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.TopLeft:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest -= m_vecRealPitch;   // 원점 -> TopLeft로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.TopLeft, m_pPoseRequest));   // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.TopLeft)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Top:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.X += m_vecRealPitch.X; // TopLeft -> Top으로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Top, m_pPoseRequest));   // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Top)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.TopRight:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.X += m_vecRealPitch.X; // Top -> TopRight로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.TopRight, m_pPoseRequest));   // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.TopRight)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Right:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.Y += m_vecRealPitch.Y; // TopRight -> Right로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Right, m_pPoseRequest));  // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Right)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.BottomRight:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.Y += m_vecRealPitch.Y; // Right -> BottomRight로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.BottomRight, m_pPoseRequest)); // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.BottomRight)); // 촬상 및 위치 화인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Bottom:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.X -= m_vecRealPitch.X; // BottomRight -> Bottom으로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Bottom, m_pPoseRequest)); // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Bottom)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.BottomLeft:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.X -= m_vecRealPitch.X; // Bottom -> BottomLeft로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.BottomLeft, m_pPoseRequest)); // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.BottomLeft)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Left:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.Y -= m_vecRealPitch.Y; // BottomLeft -> Left로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Left, m_pPoseRequest)); // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Left)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.CalculateResolution:
                        nJobStepBK = nJobStep;
                        ////  9-Point Calibration 계산
                        bCheckError = false;
                        ////  1. Calibration 결과 계산전 예외처리 확인
                        foreach (eYoonDir2D iDir in m_pDicDetectionPoint.Keys)
                        {
                            if (iDir == eYoonDir2D.None) continue;
                            if (m_pDicDetectionPoint[CalibrationDefaultLocation][iDir].X == INVALID_NUM || m_pDicDetectionPoint[CalibrationDefaultLocation][iDir].Y == INVALID_NUM)
                                bCheckError = true;
                        }
                        if (bCheckError || Result == null)
                        {
                            Console.WriteLine("Calibration Error : Get Detection Point Failure");
                            break;
                        }
                        ////  2. Calibration 계산
                        double dTotalResolutionX = 0.0, dTotalResolutionY = 0.0;
                        foreach (eYoonDir2D nDirParts in Result.ResolutionOfParts.Keys)
                        {
                            double dPixPitchX = INVALID_NUM;
                            double dPixPitchY = INVALID_NUM;
                            switch (nDirParts)
                            {
                                case eYoonDir2D.TopLeft:  // TL to T => X, TL to L => Y
                                    dPixPitchX = m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.Top].X - m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.TopLeft].X;
                                    dPixPitchY = m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.Left].Y - m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.TopLeft].Y;
                                    break;
                                case eYoonDir2D.TopRight: // TR to T => X, TR to R => Y
                                    dPixPitchX = m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.TopRight].X - m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.Top].X;
                                    dPixPitchY = m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.Right].Y - m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.TopRight].Y;
                                    break;
                                case eYoonDir2D.BottomLeft: // BL to B => X, L to BL => Y
                                    dPixPitchX = m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.Bottom].X - m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.BottomLeft].X;
                                    dPixPitchY = m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.BottomLeft].Y - m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.Left].Y;
                                    break;
                                case eYoonDir2D.BottomRight: // BR to B => X, BR to R => Y
                                    dPixPitchX = m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.BottomRight].X - m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.Bottom].X;
                                    dPixPitchY = m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.BottomRight].Y - m_pDicDetectionPoint[CalibrationDefaultLocation][eYoonDir2D.Right].Y;
                                    break;
                                default:
                                    break;
                            }
                            if (dPixPitchX == INVALID_NUM || dPixPitchY == INVALID_NUM)
                            {
                                bCheckError = true;
                                break;
                            }
                            Result.ResolutionOfParts[nDirParts] = new YoonVector2D(m_vecRealPitch.X / dPixPitchX, m_vecRealPitch.Y / dPixPitchY);
                            dTotalResolutionX += m_vecRealPitch.X / dPixPitchX;
                            dTotalResolutionY += m_vecRealPitch.Y / dPixPitchY;
                        }
                        ////  3. 평균 Threshold 계산
                        if (!bCheckError)
                        {
                            Result.AverageResolution = new YoonVector2D(dTotalResolutionX / YoonDirFactory.GetSquareDirections().Length, dTotalResolutionY / YoonDirFactory.GetSquareDirections().Length);
                            Console.WriteLine("Resolution Calibration Success!");
                            nJobStep = eStepYoonCalibration.TurnBack;
                        }
                        if (bCheckError)
                        {
                            Console.WriteLine("Calibration Error : Calibration Resolutiuon Failure");
                            nJobStep = eStepYoonCalibration.Finish;
                        }
                        break;
                    case eStepYoonCalibration.TurnBack:
                        //// Rotation Calibration시 재사용을 위한 Detection Point 초기화
                        foreach (eYoonDir2D iDir in m_pDicDetectionPoint.Keys)
                        {
                            foreach (eYoonDir2D jDir in m_pDicDetectionPoint[iDir].Keys)
                            {
                                m_pDicDetectionPoint[iDir][jDir].X = INVALID_NUM;
                                m_pDicDetectionPoint[iDir][jDir].Y = INVALID_NUM;
                            }
                        }
                        //// Rotation Calibration 직전에 GCT 등록
                        YoonVector2D vdFOV = new YoonVector2D(CameraPixelWidth, CameraPixelHeight);
                        m_pGCT.SetCameraSetting(vdFOV, m_vecInitPixelSize);
                        //// 저장된 원점으로 Turn Back (이동)
                        m_pPoseRequest = cartPosOrigin.Clone() as YoonCartD;    // 원점으로 재변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Center, m_pPoseRequest)); // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Center)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Rotate:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.RZ += RotationDegree;   // 원점에서 D만큼 회전 (TCP Z 기준)
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Top, eYoonDir2DMode.Clock, m_pPoseRequest));   // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Top)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.OppoRotate:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.RZ -= (RotationDegree * 2);   // 원점에서 -D만큼 회전 (TCP Z 기준)
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Bottom, eYoonDir2DMode.AntiClock, m_pPoseRequest));   // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Bottom)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.CalculateRotate:
                        nJobStepBK = nJobStep;
                        ////  Calibration 결과 계산전 예외처리 확인
                        bCheckError = false;
                        foreach (eYoonDir2D nDir in m_pDicDetectionPoint.Keys)
                        {
                            if (nDir == eYoonDir2D.Top || nDir == eYoonDir2D.Bottom)
                            {
                                if (m_pDicDetectionPoint[AlignmentTargetLocation1][nDir].X == INVALID_NUM || m_pDicDetectionPoint[AlignmentTargetLocation1][nDir].Y == INVALID_NUM ||
                                    m_pDicDetectionPoint[AlignmentTargetLocation2][nDir].X == INVALID_NUM || m_pDicDetectionPoint[AlignmentTargetLocation2][nDir].Y == INVALID_NUM)
                                    bCheckError = true;
                            }
                        }
                        if (bCheckError || Result == null)
                        {
                            Console.WriteLine("Calibration Error : Get Detection Point Failure");
                            nJobStep = eStepYoonCalibration.Finish;
                            break;
                        }
                        ////  Global 좌표를 설정한다.
                        m_pGCT.CalculateGrobalCoordinate(m_pDicDetectionPoint[AlignmentTargetLocation1][eYoonDir2D.Top], m_pDicDetectionPoint[AlignmentTargetLocation1][eYoonDir2D.Top], RotationDegree * Math.PI / 180, AlignmentTargetLocation1);
                        m_pGCT.CalculateGrobalCoordinate(m_pDicDetectionPoint[AlignmentTargetLocation2][eYoonDir2D.Bottom], m_pDicDetectionPoint[AlignmentTargetLocation2][eYoonDir2D.Bottom], RotationDegree * Math.PI / 180, AlignmentTargetLocation2);
                        ////  처리 후 예상되는 Camera의 Center Position을 가져온다.
                        Result.DeviceCenterPos = m_pGCT.GetRealCenterPosition(CalibrationDefaultLocation);
                        Console.WriteLine("Rotation Calibration Success!");
                        nJobStep = eStepYoonCalibration.Finish;
                        break;
                    case eStepYoonCalibration.Finish:
                        bPlay = false;
                        IsCompleted = !bCheckError;
                        break;
                }
            }
        }
    }

    public class YoonRotationCalib2D : IDisposable // 2개의 Device에 대한 Rotation Calibration
    {
        protected enum eStepYoonCalibration
        {
            Wait = -1,
            Init,
            Origin,
            TopLeft,
            Top,
            TopRight,
            Right,
            BottomRight,
            Bottom,
            BottomLeft,
            Left,
            Rotate,
            OppoRotate,
            TurnBack,
            CalculateResolution,
            CalculateRotate,
            Finish,
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    m_pGCT.Dispose();
                    m_pThread.Abort();
                    Thread.Sleep(100);
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                ResultDictionary.Clear();
                ResultDictionary = null;
                m_pDicDetectionPoint.Clear();
                m_pDicDetectionPoint = null;
                m_pDicCurrentReceiveFlag.Clear();
                m_pDicCurrentReceiveFlag = null;

                m_pGCT = null;
                m_pThread = null;

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        ~YoonRotationCalib2D()
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

        public bool IsCompleted { get; private set; } = false;
        public Dictionary<eYoonDir2D, RotationCalibResult> ResultDictionary { get; private set; } = new Dictionary<eYoonDir2D, RotationCalibResult>();
        public eYoonDir2D AlignmentTargetLocation1 { get; private set; } = eYoonDir2D.TopLeft;
        public eYoonDir2D AlignmentTargetLocation2 { get; private set; } = eYoonDir2D.TopRight;
        public double RotationDegree { get; set; } = 0.0;
        public double CameraPixelWidth { get; set; } = 0.0;
        public double CameraPixelHeight { get; set; } = 0.0;
        public bool IsReverseCoordinateX { get; set; } = false;
        public bool IsReverseCoordinateY { get; set; } = false;
        public bool IsFlipCoordinateXY { get; set; } = false;

        #region 내부 사용 Parameter
        private const double INVALID_NUM = -10000.0;
        // Point 설정 Param
        private Dictionary<eYoonDir2D, bool> m_pDicCurrentReceiveFlag = new Dictionary<eYoonDir2D, bool>();
        private YoonCartD m_pPoseRequest = new YoonCartD();
        // Align Calibration을 위한 Class
        private Thread m_pThread = null;
        private YoonTransform m_pGCT = new YoonTransform();
        // Process Calibration 관련 설정
        private Dictionary<eYoonDir2D, Dictionary<eYoonDir2D, YoonVector2D>> m_pDicDetectionPoint = new Dictionary<eYoonDir2D, Dictionary<eYoonDir2D, YoonVector2D>>();
        private YoonVector2D m_vecRealPitch = new YoonVector2D();
        private YoonVector2D m_vecInitPixelSize = new YoonVector2D(INVALID_NUM, INVALID_NUM);
        // 기타 설정
        private string m_strConfigFilePath = "";
        private bool m_bFlagInit = false;
        #endregion

        public event PoseSendCallback OnMovementPoseSendEvent;  // 로봇 Pose를 보냄
        public event GrapRequestCallback OnGrapRequestEvent;

        public void OnCurrentPositionReceive(object sender, CalibPointArgs e)   // Pattern 대상 Point를 보냄
        {
            if (e.DeviceDirection != AlignmentTargetLocation1 || e.DeviceDirection != AlignmentTargetLocation2) return;

            m_pDicCurrentReceiveFlag[e.DeviceDirection] = true;
            m_pDicDetectionPoint[e.DeviceDirection][e.GrapDirection] = e.Point;  // 위치 값은 주어진 "DirCompass"에 맞게 저장됨
        }

        public YoonRotationCalib2D(eYoonDir2D nDir1, eYoonDir2D nDir2)
        {
            AlignmentTargetLocation1 = nDir1;
            AlignmentTargetLocation2 = nDir2;

            ////  Dictionary 초기화
            m_pDicDetectionPoint.Clear();
            foreach (eYoonDir2D iDir in YoonDirFactory.GetSquareDirections())
            {
                Dictionary<eYoonDir2D, YoonVector2D> pDicPoint = new Dictionary<eYoonDir2D, YoonVector2D>();
                pDicPoint.Clear();
                foreach (eYoonDir2D jDir in Enum.GetValues(typeof(eYoonDir2D)))
                {
                    pDicPoint.Add(jDir, new YoonVector2D(INVALID_NUM, INVALID_NUM));
                }
                m_pDicDetectionPoint.Add(iDir, pDicPoint);
            }
            ResultDictionary.Clear();
            ResultDictionary.Add(nDir1, new RotationCalibResult());
            ResultDictionary.Add(nDir2, new RotationCalibResult());
            m_pDicCurrentReceiveFlag.Clear();
            m_pDicCurrentReceiveFlag.Add(nDir1, false);
            m_pDicCurrentReceiveFlag.Add(nDir2, false);

            ////  File 경로 초기화
            m_strConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory", "RotCalib2D.ini");
        }

        private void FlipCoordinate(ref YoonVector2D pVecRealPos)
        {
            if (IsFlipCoordinateXY)
            {
                double dY = pVecRealPos.X;
                double dX = pVecRealPos.Y;
                pVecRealPos.X = dX;
                pVecRealPos.Y = dY;
                pVecRealPos.W = -pVecRealPos.W;
            }
        }

        private void ReverseCoordinate(ref YoonVector2D pVecRealPos)
        {
            pVecRealPos.X *= ((IsReverseCoordinateX) ? -1 : 1);
            pVecRealPos.Y *= ((IsReverseCoordinateY) ? -1 : 1);
        }

        public void InitRealPosition(YoonCartD pRealPos, YoonVector2D vecPixelSize)
        {
            //// 사전 Data 입력 확인
            if (CameraPixelWidth <= 0 || CameraPixelHeight <= 0 || vecPixelSize.X <= 0 || vecPixelSize.Y <= 0) return;

            //// 멤버변수 업데이트
            m_vecInitPixelSize = vecPixelSize.Clone() as YoonVector2D;
            //// 현재 로봇/스테이지의 기구적 Position (스테이지 / 로봇 원점 기준)
            m_pPoseRequest = pRealPos;
            //// Camera FOV 기준으로 Calibration시 이동 간격 계산
            m_vecRealPitch.X = CameraPixelWidth * m_vecInitPixelSize.X;   // mm per pixel
            m_vecRealPitch.Y = CameraPixelHeight * m_vecInitPixelSize.Y;  // mm per pixel
            //// Reverse 및 Swarp에 따른 좌표이동 변경
            FlipCoordinate(ref m_vecRealPitch);
            ReverseCoordinate(ref m_vecRealPitch);    // Swarp를 먼저해서 좌표를 바꿔주고 Reverse 체크할 것
            m_bFlagInit = true;
        }

        public bool StartProcess(string strThreadName = "Rotation")
        {
            if (!m_bFlagInit || m_vecInitPixelSize.X <= 0 || m_vecInitPixelSize.Y <= 0)
                return false;

            m_bFlagInit = false;
            ////  Thread 시작
            try
            {
                m_pThread = new Thread(new ThreadStart(ProcessCalibration));
                m_pThread.Name = "Calibration Thread [" + strThreadName + "]";
                m_pThread.Start();
            }
            catch (ThreadStartException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        private void ProcessCalibration()
        {
            eStepYoonCalibration nJobStep = eStepYoonCalibration.Init;
            eStepYoonCalibration nJobStepBK = eStepYoonCalibration.Wait;
            YoonVector2D vecCameraCenter = new YoonVector2D(CameraPixelWidth / 2, CameraPixelHeight / 2);
            YoonCartD cartPosOrigin = null;
            bool bPlay = true;
            bool bCheckError = false;

            while (bPlay)
            {
                Thread.Sleep(100);

                switch (nJobStep)
                {
                    case eStepYoonCalibration.Wait:
                        if (m_pDicCurrentReceiveFlag.Values.Any(x => x == false))
                            break;
                        //// Flag 초기화
                        foreach (eYoonDir2D nDir in m_pDicCurrentReceiveFlag.Keys)
                            m_pDicCurrentReceiveFlag[nDir] = false;
                        //// Wait 종료 후 업무 분배
                        switch (nJobStepBK)
                        {
                            case eStepYoonCalibration.Init:
                                nJobStep = eStepYoonCalibration.Origin;
                                break;
                            case eStepYoonCalibration.Origin:
                                nJobStep = eStepYoonCalibration.TopLeft;
                                break;
                            case eStepYoonCalibration.TopLeft:
                                nJobStep = eStepYoonCalibration.Top;
                                break;
                            case eStepYoonCalibration.Top:
                                nJobStep = eStepYoonCalibration.TopRight;
                                break;
                            case eStepYoonCalibration.TopRight:
                                nJobStep = eStepYoonCalibration.Right;
                                break;
                            case eStepYoonCalibration.Right:
                                nJobStep = eStepYoonCalibration.BottomRight;
                                break;
                            case eStepYoonCalibration.BottomRight:
                                nJobStep = eStepYoonCalibration.Bottom;
                                break;
                            case eStepYoonCalibration.Bottom:
                                nJobStep = eStepYoonCalibration.BottomLeft;
                                break;
                            case eStepYoonCalibration.BottomLeft:
                                nJobStep = eStepYoonCalibration.Left;
                                break;
                            case eStepYoonCalibration.Left:
                                nJobStep = eStepYoonCalibration.CalculateResolution;
                                break;
                            case eStepYoonCalibration.TurnBack:
                                nJobStep = eStepYoonCalibration.Rotate;
                                break;
                            case eStepYoonCalibration.Rotate:
                                nJobStep = eStepYoonCalibration.OppoRotate;
                                break;
                            case eStepYoonCalibration.OppoRotate:
                                nJobStep = eStepYoonCalibration.CalculateRotate;
                                break;
                        }
                        break;
                    case eStepYoonCalibration.Init:
                        nJobStepBK = nJobStep;
                        //// Detection Point 초기화
                        foreach (eYoonDir2D iDir in m_pDicDetectionPoint.Keys)
                        {
                            foreach (eYoonDir2D jDir in m_pDicDetectionPoint[iDir].Keys)
                            {
                                m_pDicDetectionPoint[iDir][jDir].X = INVALID_NUM;
                                m_pDicDetectionPoint[iDir][jDir].Y = INVALID_NUM;
                            }
                        }
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Center, m_pPoseRequest));
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Center));
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Origin:
                        nJobStepBK = nJobStep;
                        YoonVector2D vecTotal = new YoonVector2D();
                        foreach (eYoonDir2D nDir in m_pDicDetectionPoint.Keys)
                        {
                            if (nDir == AlignmentTargetLocation1 || nDir == AlignmentTargetLocation2)
                            {
                                YoonVector2D vecOffset = vecCameraCenter - m_pDicDetectionPoint[nDir][eYoonDir2D.Center];
                                vecTotal.X += vecOffset.X * m_vecInitPixelSize.X;   //  mm per pixel
                                vecTotal.Y += vecOffset.Y * m_vecInitPixelSize.Y;
                                //// 오동작 방지를 위해 해당 부분 초기화
                                m_pDicDetectionPoint[nDir][eYoonDir2D.Center].X = INVALID_NUM;
                                m_pDicDetectionPoint[nDir][eYoonDir2D.Center].Y = INVALID_NUM;
                            }
                        }
                        YoonVector2D vecMovementToCenter = (vecTotal / m_pDicDetectionPoint.Keys.Count);
                        FlipCoordinate(ref vecMovementToCenter);
                        ReverseCoordinate(ref vecMovementToCenter);
                        m_pPoseRequest += vecMovementToCenter; // 초기 -> 원점위치로 변경
                        cartPosOrigin = m_pPoseRequest.Clone() as YoonCartD; // 원점 위치 복사해놓기
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Center, m_pPoseRequest)); // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Center)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.TopLeft:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest -= m_vecRealPitch;   // 원점 -> TopLeft로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.TopLeft, m_pPoseRequest));   // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.TopLeft)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Top:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.X += m_vecRealPitch.X; // TopLeft -> Top으로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Top, m_pPoseRequest));   // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Top)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.TopRight:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.X += m_vecRealPitch.X; // Top -> TopRight로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.TopRight, m_pPoseRequest));   // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.TopRight)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Right:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.Y += m_vecRealPitch.Y; // TopRight -> Right로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Right, m_pPoseRequest));  // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Right)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.BottomRight:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.Y += m_vecRealPitch.Y; // Right -> BottomRight로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.BottomRight, m_pPoseRequest)); // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.BottomRight)); // 촬상 및 위치 화인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Bottom:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.X -= m_vecRealPitch.X; // BottomRight -> Bottom으로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Bottom, m_pPoseRequest)); // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Bottom)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.BottomLeft:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.X -= m_vecRealPitch.X; // Bottom -> BottomLeft로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.BottomLeft, m_pPoseRequest)); // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.BottomLeft)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Left:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.Y -= m_vecRealPitch.Y; // BottomLeft -> Left로 변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Left, m_pPoseRequest)); // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Left)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.CalculateResolution:
                        nJobStepBK = nJobStep;
                        ////  9-Point Calibration 계산
                        bCheckError = false;
                        YoonVector2D vecTotalResolution = new YoonVector2D(0, 0);
                        foreach (eYoonDir2D iDir in m_pDicCurrentReceiveFlag.Keys)
                        {
                            if (iDir == AlignmentTargetLocation1 || iDir == AlignmentTargetLocation2) // 반드시 2개만 처리함
                            {
                                ////  1. Calibration 결과 계산전 예외처리 확인
                                foreach (eYoonDir2D kDir in m_pDicDetectionPoint.Keys)
                                {
                                    if (kDir == eYoonDir2D.None) continue;
                                    if (m_pDicDetectionPoint[iDir][kDir].X == INVALID_NUM || m_pDicDetectionPoint[iDir][kDir].Y == INVALID_NUM)
                                        bCheckError = true;
                                }
                                if (bCheckError || ResultDictionary == null)
                                {
                                    Console.WriteLine("Calibration Error : Get Detection Point Failure");
                                    break;
                                }
                                ////  2. Calibration 계산
                                double dTotalResolutionX = 0.0, dTotalResolutionY = 0.0;
                                foreach (eYoonDir2D nDirParts in ResultDictionary[iDir].ResolutionOfParts.Keys)
                                {
                                    double dPixPitchX = INVALID_NUM;
                                    double dPixPitchY = INVALID_NUM;
                                    switch (nDirParts)
                                    {
                                        case eYoonDir2D.TopLeft:  // TL to T => X, TL to L => Y
                                            dPixPitchX = m_pDicDetectionPoint[iDir][eYoonDir2D.Top].X - m_pDicDetectionPoint[iDir][eYoonDir2D.TopLeft].X;
                                            dPixPitchY = m_pDicDetectionPoint[iDir][eYoonDir2D.Left].Y - m_pDicDetectionPoint[iDir][eYoonDir2D.TopLeft].Y;
                                            break;
                                        case eYoonDir2D.TopRight: // TR to T => X, TR to R => Y
                                            dPixPitchX = m_pDicDetectionPoint[iDir][eYoonDir2D.TopRight].X - m_pDicDetectionPoint[iDir][eYoonDir2D.Top].X;
                                            dPixPitchY = m_pDicDetectionPoint[iDir][eYoonDir2D.Right].Y - m_pDicDetectionPoint[iDir][eYoonDir2D.TopRight].Y;
                                            break;
                                        case eYoonDir2D.BottomLeft: // BL to B => X, L to BL => Y
                                            dPixPitchX = m_pDicDetectionPoint[iDir][eYoonDir2D.Bottom].X - m_pDicDetectionPoint[iDir][eYoonDir2D.BottomLeft].X;
                                            dPixPitchY = m_pDicDetectionPoint[iDir][eYoonDir2D.BottomLeft].Y - m_pDicDetectionPoint[iDir][eYoonDir2D.Left].Y;
                                            break;
                                        case eYoonDir2D.BottomRight: // BR to B => X, BR to R => Y
                                            dPixPitchX = m_pDicDetectionPoint[iDir][eYoonDir2D.BottomRight].X - m_pDicDetectionPoint[iDir][eYoonDir2D.Bottom].X;
                                            dPixPitchY = m_pDicDetectionPoint[iDir][eYoonDir2D.BottomRight].Y - m_pDicDetectionPoint[iDir][eYoonDir2D.Right].Y;
                                            break;
                                        default:
                                            break;
                                    }
                                    if (dPixPitchX == INVALID_NUM || dPixPitchY == INVALID_NUM)
                                    {
                                        bCheckError = true;
                                        break;
                                    }
                                    ResultDictionary[iDir].ResolutionOfParts[nDirParts] = new YoonVector2D(m_vecRealPitch.X / dPixPitchX, m_vecRealPitch.Y / dPixPitchY);
                                    dTotalResolutionX += m_vecRealPitch.X / dPixPitchX;
                                    dTotalResolutionY += m_vecRealPitch.Y / dPixPitchY;
                                }
                                ////  3. 평균 Threshold 계산
                                if (!bCheckError)
                                {
                                    ResultDictionary[iDir].AverageResolution = new YoonVector2D(dTotalResolutionX / YoonDirFactory.GetSquareDirections().Length, dTotalResolutionY / YoonDirFactory.GetSquareDirections().Length);
                                    vecTotalResolution += ResultDictionary[iDir].AverageResolution;
                                }
                            }
                        }
                        if (bCheckError)
                        {
                            Console.WriteLine("Calibration Error : Calibration Resolutiuon Failure");
                            nJobStep = eStepYoonCalibration.Finish;
                        }
                        else
                        {
                            m_vecInitPixelSize = vecTotalResolution / 2;    // Init Pixel Size에 현재 분해능 평균값 삽입 (2 Device이므로 2로 나눔)
                            Console.WriteLine("Resolution Calibration Success!");
                            nJobStep = eStepYoonCalibration.TurnBack;
                        }
                        break;
                    case eStepYoonCalibration.TurnBack:
                        //// Rotation Calibration시 재사용을 위한 Detection Point 초기화
                        foreach (eYoonDir2D iDir in m_pDicDetectionPoint.Keys)
                        {
                            foreach (eYoonDir2D jDir in m_pDicDetectionPoint[iDir].Keys)
                            {
                                m_pDicDetectionPoint[iDir][jDir].X = INVALID_NUM;
                                m_pDicDetectionPoint[iDir][jDir].Y = INVALID_NUM;
                            }
                        }
                        //// Rotation Calibration 직전에 GCT 등록
                        YoonVector2D vdFOV = new YoonVector2D(CameraPixelWidth, CameraPixelHeight);
                        m_pGCT.SetCameraSetting(vdFOV, m_vecInitPixelSize);
                        //// 저장된 원점으로 Turn Back (이동)
                        m_pPoseRequest = cartPosOrigin.Clone() as YoonCartD;    // 원점으로 재변경
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Center, m_pPoseRequest)); // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Center)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Rotate:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.RZ += RotationDegree;   // 원점에서 D만큼 회전 (TCP Z 기준)
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Top, eYoonDir2DMode.Clock, m_pPoseRequest));   // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Top)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.OppoRotate:
                        nJobStepBK = nJobStep;
                        m_pPoseRequest.RZ -= (RotationDegree * 2);   // 원점에서 -D만큼 회전 (TCP Z 기준)
                        OnMovementPoseSendEvent(this, new CalibPoseArgs(eYoonDir2D.Bottom, eYoonDir2DMode.AntiClock, m_pPoseRequest));   // 이동
                        Thread.Sleep(500);
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDir2D.Bottom)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.CalculateRotate:
                        nJobStepBK = nJobStep;
                        ////  Calibration 결과 계산전 예외처리 확인
                        bCheckError = false;
                        foreach (eYoonDir2D nDir in m_pDicDetectionPoint.Keys)
                        {
                            if (nDir == eYoonDir2D.Top || nDir == eYoonDir2D.Bottom)
                            {
                                if (m_pDicDetectionPoint[AlignmentTargetLocation1][nDir].X == INVALID_NUM || m_pDicDetectionPoint[AlignmentTargetLocation1][nDir].Y == INVALID_NUM ||
                                    m_pDicDetectionPoint[AlignmentTargetLocation2][nDir].X == INVALID_NUM || m_pDicDetectionPoint[AlignmentTargetLocation2][nDir].Y == INVALID_NUM)
                                    bCheckError = true;
                            }
                        }
                        if (bCheckError || ResultDictionary == null)
                        {
                            Console.WriteLine("Calibration Error : Get Detection Point Failure");
                            nJobStep = eStepYoonCalibration.Finish;
                            break;
                        }
                        ////  Global 좌표를 설정한다.
                        m_pGCT.CalculateGrobalCoordinate(m_pDicDetectionPoint[AlignmentTargetLocation1][eYoonDir2D.Top], m_pDicDetectionPoint[AlignmentTargetLocation1][eYoonDir2D.Top], RotationDegree * Math.PI / 180, AlignmentTargetLocation1);
                        m_pGCT.CalculateGrobalCoordinate(m_pDicDetectionPoint[AlignmentTargetLocation2][eYoonDir2D.Bottom], m_pDicDetectionPoint[AlignmentTargetLocation2][eYoonDir2D.Bottom], RotationDegree * Math.PI / 180, AlignmentTargetLocation2);
                        ////  처리 후 예상되는 Camera의 Center Position을 가져온다.
                        ResultDictionary[AlignmentTargetLocation1].DeviceCenterPos = m_pGCT.GetRealCenterPosition(AlignmentTargetLocation1);
                        ResultDictionary[AlignmentTargetLocation2].DeviceCenterPos = m_pGCT.GetRealCenterPosition(AlignmentTargetLocation2);
                        Console.WriteLine("Rotation Calibration Success!");
                        nJobStep = eStepYoonCalibration.Finish;
                        break;
                    case eStepYoonCalibration.Finish:
                        bPlay = false;
                        IsCompleted = !bCheckError;
                        break;
                }
            }
        }
    }

}
