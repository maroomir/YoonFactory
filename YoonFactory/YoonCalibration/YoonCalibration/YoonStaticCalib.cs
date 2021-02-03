using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace YoonFactory.Calibration
{
    public class YoonStaticCalib1D : IDisposable // Static 상태로 해상도 Calibration만 진행 (9-Point Resolution Calibration)
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
                    m_pThread.Abort();
                    Thread.Sleep(100);
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                m_pDicDetectionRect.Clear();
                m_pDicDetectionRect = null;
                Result = null;
                m_pThread = null;

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        ~YoonStaticCalib1D()
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
        public StaticCalibResult Result { get; private set; } = new StaticCalibResult();
        public double CameraPixelWidth { get; set; } = 0.0;
        public double CameraPixelHeight { get; set; } = 0.0;
        public double TargetRealWidth { get; set; } = 0.0;
        public double TargetRealHeight { get; set; } = 0.0;

        public event GrapRequestCallback OnGrapRequestEvent;

        #region 내부 사용 Paramter
        private const double INVALID_NUM = -1000.0;
        private const double INIT_SIZE = -100.0;
        private bool m_bFlagReceive = false;
        private Thread m_pThread = null;
        private Dictionary<eYoonDirCompass, YoonRectAffine2D> m_pDicDetectionRect = new Dictionary<eYoonDirCompass, YoonRectAffine2D>();
        private string m_strConfigFilePath = "";
        #endregion

        public void OnCurrentPositionReceive(object sender, CalibRectArgs e)   // Pattern 대상 Point를 보냄
        {
            m_bFlagReceive = true;
            m_pDicDetectionRect[e.GrapDirection] = e.Rect;  // 위치 값은 주어진 "DirCompass"에 맞게 저장됨
        }

        public YoonStaticCalib1D()
        {
            m_pDicDetectionRect.Clear();
            foreach (eYoonDirCompass nDir in Enum.GetValues(typeof(eYoonDirCompass)))
                m_pDicDetectionRect.Add(nDir, new YoonRectAffine2D(INVALID_NUM, INVALID_NUM, INIT_SIZE, INIT_SIZE, 0));

            ////  File 경로 초기화
            m_strConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory", "StaticCalib1D.ini");
        }

        public bool StartProcess(string strThreadName = "Static")
        {
            if (CameraPixelWidth <= 0 || CameraPixelHeight <= 0 || TargetRealWidth <= 0 || TargetRealHeight <= 0)
                return false;

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
            bool bPlay = true;
            bool bCheckError = false;

            while (bPlay)
            {
                Thread.Sleep(100);

                switch (nJobStep)
                {
                    case eStepYoonCalibration.Wait:
                        if (!m_bFlagReceive) break;
                        //// Flag 초기화
                        m_bFlagReceive = false;
                        //// Wait 종료 후 업무 분배
                        switch (nJobStepBK)
                        {
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
                        }
                        break;
                    case eStepYoonCalibration.Init:
                        nJobStepBK = nJobStep;
                        //// Detection Point 초기화
                        foreach (eYoonDirCompass iDir in m_pDicDetectionRect.Keys)
                            m_pDicDetectionRect[iDir] = new YoonRectAffine2D(INVALID_NUM, INVALID_NUM, INIT_SIZE, INIT_SIZE, 0);
                        nJobStep = eStepYoonCalibration.Origin;
                        break;
                    case eStepYoonCalibration.Origin:
                        nJobStepBK = nJobStep;
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDirCompass.Center)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.TopLeft:
                        nJobStepBK = nJobStep;
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDirCompass.TopLeft)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Top:
                        nJobStepBK = nJobStep;
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDirCompass.Top)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.TopRight:
                        nJobStepBK = nJobStep;
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDirCompass.TopRight)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Right:
                        nJobStepBK = nJobStep;
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDirCompass.Right)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.BottomRight:
                        nJobStepBK = nJobStep;
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDirCompass.BottomRight)); // 촬상 및 위치 화인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Bottom:
                        nJobStepBK = nJobStep;
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDirCompass.Bottom)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.BottomLeft:
                        nJobStepBK = nJobStep;
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDirCompass.BottomLeft)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.Left:
                        nJobStepBK = nJobStep;
                        OnGrapRequestEvent(this, new CalibGrapArgs(eYoonDirCompass.Left)); // 촬상 및 위치 확인
                        nJobStep = eStepYoonCalibration.Wait;
                        break;
                    case eStepYoonCalibration.CalculateResolution:
                        nJobStepBK = nJobStep;
                        bCheckError = false;
                        ////  1. Calibration 결과 계산전 예외처리 확인
                        foreach (eYoonDirCompass iDir in m_pDicDetectionRect.Keys)
                        {
                            if (iDir == eYoonDirCompass.MaxDir) continue;
                            if (m_pDicDetectionRect[iDir].CenterPos.X == INVALID_NUM || m_pDicDetectionRect[iDir].CenterPos.Y == INVALID_NUM)
                                bCheckError = true;
                        }
                        if (bCheckError || Result == null)
                        {
                            Console.WriteLine("Calibration Error : Get Detection Point Failure");
                            break;
                        }
                        ////  2. Calibration 계산
                        double dTotalResolutionX = 0.0, dTotalResolutionY = 0.0;
                        foreach (eYoonDirCompass nDirParts in m_pDicDetectionRect.Keys)
                        {
                            if (nDirParts == eYoonDirCompass.MaxDir) continue;

                            double dPixelWidth = m_pDicDetectionRect[nDirParts].Width;
                            double dPixelHeight = m_pDicDetectionRect[nDirParts].Height;
                            if (dPixelWidth <= 0 || dPixelHeight <= 0)
                            {
                                bCheckError = true;
                                break;
                            }
                            Result.ResolutionOfParts[nDirParts] = new YoonVector2D(TargetRealWidth / dPixelWidth, TargetRealHeight / dPixelHeight);
                            dTotalResolutionX += TargetRealWidth / dPixelWidth;
                            dTotalResolutionY += TargetRealHeight / dPixelHeight;
                        }
                        ////  3. 평균 Threshold 계산
                        if (!bCheckError)
                        {
                            Result.AverageResolution = new YoonVector2D(dTotalResolutionX / (int)eYoonDirCompass.MaxDir, dTotalResolutionY / (int)eYoonDirCompass.MaxDir);
                            Console.WriteLine("Resolution Calibration Success!");
                        }
                        else
                            Console.WriteLine("Calibration Error : Calibration Resolutiuon Failure");
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
