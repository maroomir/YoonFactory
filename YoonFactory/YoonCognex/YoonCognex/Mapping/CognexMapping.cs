using System;
using System.Collections.Generic;
using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;

namespace YoonFactory.Cognex
{
    public class CognexMapping : IYoonMapping
    {
        #region Supported IDisposable Pattern
        ~CognexMapping()
        {
            this.Dispose(false);
        }

        private bool disposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;
            if (disposing)
            {
                ////  .Net Framework에 의해 관리되는 리소스를 여기서 정리합니다.
                if (RealPoints != null)
                    RealPoints.Clear();
                if (PixelPoints != null)
                    PixelPoints.Clear();
                RealPoints = null;
                PixelPoints = null;

            }
            //// .NET Framework에 의하여 관리되지 않는 외부 리소스들을 여기서 정리합니다.
            this.disposed = true;
            Offset = null;
            m_pTransformRealToPixel = null;
            m_pTransoformPixelToReal = null;
        }
        #endregion

        private CogCalibCheckerboard m_pCalibration = null;
        private ICogTransform2D m_pTransformRealToPixel = null;
        private ICogTransform2D m_pTransoformPixelToReal = null;

        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;
        public IYoonVector Offset { get; private set; } = new YoonVector2D(); // Real에 적용되는 Offset
        public List<IYoonVector> RealPoints { get; set; } = new List<IYoonVector>();
        public List<IYoonVector> PixelPoints { get; set; } = new List<IYoonVector>();

        public CognexMapping(CogCalibCheckerboard pCalibration)
        {
            SetSource(pCalibration);
        }

        public CognexMapping(CogCalibCheckerboard pCalibration, YoonVector2D vecOffset)
        {
            SetSource(pCalibration);
            Offset = vecOffset.Clone() as YoonVector2D;
            SetOffsetToCalibrationPoints();
        }

        public CogCalibCheckerboard GetSource()
        {
            return m_pCalibration;
        }

        public void SetSource(CogCalibCheckerboard pCalibration)
        {
            if (pCalibration == null) return;

            m_pCalibration = pCalibration;
            m_pTransformRealToPixel = pCalibration.GetComputedUncalibratedFromRawCalibratedTransform();
            m_pTransoformPixelToReal = m_pTransformRealToPixel.InvertBase();
            Width = pCalibration.CalibrationImage.Width;
            Height = pCalibration.CalibrationImage.Height;
            RealPoints.Clear();
            PixelPoints.Clear();
            for (int iPos = 0; iPos < pCalibration.NumPoints; iPos++)
            {
                YoonVector2D vecRealPos = new YoonVector2D(pCalibration.GetRawCalibratedPointX(iPos), pCalibration.GetRawCalibratedPointY(iPos));
                YoonVector2D vecPixelPos = new YoonVector2D(pCalibration.GetUncalibratedPointX(iPos), pCalibration.GetUncalibratedPointY(iPos));
                RealPoints.Add(vecRealPos);
                PixelPoints.Add(vecPixelPos);
            }
        }

        private void SetOffsetToCalibrationPoints()
        {
            if (Offset == null) return;
            if (Offset is YoonVector2D vecOffset)
            {
                for (int iPos = 0; iPos < RealPoints.Count; iPos++)
                    RealPoints[iPos] = (YoonVector2D)RealPoints[iPos] + vecOffset;
            }
        }

        public void SetReferencePosition(IYoonVector vecPixelPos, IYoonVector vecRealPos)
        {
            if (Offset == null) return;
            YoonVector2D vecOffset = (YoonVector2D)vecRealPos - (YoonVector2D)ToReal(vecPixelPos);

            SetSource(m_pCalibration);  // Position, Width / Height 전부 초기화
            Offset = vecOffset;
            SetOffsetToCalibrationPoints();
        }

        public IYoonVector GetPixelResolution(IYoonVector vecPixelPos)   // 해당 Pixel 기준 -1 pixel 부터 +1 pixel까지의 분해능
        {
            return new YoonVector2D(GetPixelResolutionX(vecPixelPos), GetPixelResolutionY(vecPixelPos));
        }

        public double GetPixelResolutionX(IYoonVector vecPixelPos)   // 해당 Pixel 기준 -1 pixel 부터 +1 pixel까지의 분해능
        {
            double dResolution = 0.0;
            if (vecPixelPos is YoonVector2D vecPos)
            {
                IYoonVector vecStartPos = vecPos + new YoonVector2D(-1, 0);
                IYoonVector vecEndPos = vecPos + new YoonVector2D(1, 0);
                YoonVector2D vecResult = (YoonVector2D)ToReal(vecEndPos) - (YoonVector2D)ToReal(vecStartPos);
                dResolution = Math.Abs(vecResult.X / 2.0);
            }
            return dResolution;
        }

        public double GetPixelResolutionY(IYoonVector vecPixelPos)   // 해당 Pixel 기준 -1 pixel 부터 +1 pixel까지의 분해능
        {
            double dResolution = 0.0;
            if (vecPixelPos is YoonVector2D vecPos)
            {
                IYoonVector vecStartPos = vecPos + new YoonVector2D(0, -1);
                IYoonVector vecEndPos = vecPos + new YoonVector2D(0, 1);
                YoonVector2D vecResult = (YoonVector2D)ToReal(vecEndPos) - (YoonVector2D)ToReal(vecStartPos);
                dResolution = Math.Abs(vecResult.Y / 2.0);
            }
            return dResolution;
        }

        public void CopyFrom(IYoonMapping pMapping)
        {
            if (pMapping is CognexMapping pMappingCognex)
            {
                SetSource(pMappingCognex.GetSource());
                Offset = pMappingCognex.Offset.Clone() as YoonVector2D;
                SetOffsetToCalibrationPoints();
            }
        }

        public IYoonMapping Clone()
        {
            return new CognexMapping(m_pCalibration, Offset as YoonVector2D);
        }

        public IYoonVector ToPixel(IYoonVector vecRealPos)
        {
            if (m_pTransformRealToPixel == null) return new YoonVector2D(-10000, -10000);

            if (vecRealPos is YoonVector2D vecInput)
            {
                vecInput = vecInput - (YoonVector2D)Offset; // RealPos에 반드시 Offset 선적용해야함 (위 식에 따라 - 해야함, + 하면 중복됨)
                double dRealX, dRealY;
                m_pTransformRealToPixel.MapPoint(vecInput.X, vecInput.Y, out dRealX, out dRealY);
                return new YoonVector2D(dRealX, dRealY);
            }
            else
                return new YoonVector2D(-10000, -10000);
        }

        public IYoonVector ToReal(IYoonVector vecPixelPos)
        {
            if (Offset == null || m_pTransoformPixelToReal == null) return new YoonVector2D(-10000, -10000);

            if (vecPixelPos is YoonVector2D vecInput)
            {
                double dMappedX, dMappedY;
                m_pTransoformPixelToReal.MapPoint(vecInput.X, vecInput.Y, out dMappedX, out dMappedY);
                return new YoonVector2D(dMappedX, dMappedY) + (YoonVector2D)Offset;
            }
            else
                return new YoonVector2D(-10000, -10000);
        }
    }

}
