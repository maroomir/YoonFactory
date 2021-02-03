using System;
using System.Collections.Generic;

namespace YoonFactory.Calibration
{

    public class RotationCalibResult : IYoonResult
    {
        public YoonVector2D AverageResolution { get; set; } = new YoonVector2D(0.0, 0.0);
        public Dictionary<eYoonDirRect, YoonVector2D> ResolutionOfParts { get; set; } = new Dictionary<eYoonDirRect, YoonVector2D>();
        public YoonVector2D DeviceCenterPos { get; set; } = new YoonVector2D();

        public RotationCalibResult()
        {
            foreach (eYoonDirRect nDir in Enum.GetValues(typeof(eYoonDirRect)))
                ResolutionOfParts.Add(nDir, new YoonVector2D());
        }

        public string Combine(string strDelimiter)
        {
            return AverageResolution.X.ToString() + strDelimiter +
                AverageResolution.Y.ToString() + strDelimiter +
                ResolutionOfParts.Count.ToString() + strDelimiter +
                DeviceCenterPos.X.ToString() + strDelimiter +
                DeviceCenterPos.Y.ToString() + strDelimiter;
        }

        public bool Equals(IYoonResult pResult)
        {
            if (pResult is RotationCalibResult pResultCalib)
            {
                if (pResultCalib.AverageResolution != AverageResolution || pResultCalib.DeviceCenterPos != DeviceCenterPos)
                    return false;

                foreach (eYoonDirRect nDir in ResolutionOfParts.Keys)
                {
                    if (!pResultCalib.ResolutionOfParts.ContainsKey(nDir) || pResultCalib.ResolutionOfParts[nDir] != ResolutionOfParts[nDir])
                        return false;
                }
                return true;
            }
            return false;
        }

        public void CopyFrom(IYoonResult pResult)
        {
            if (pResult == null) return;

            if (pResult is RotationCalibResult pResultCalib)
            {
                AverageResolution = pResultCalib.AverageResolution.Clone() as YoonVector2D;
                ResolutionOfParts = new Dictionary<eYoonDirRect, YoonVector2D>(pResultCalib.ResolutionOfParts);
                DeviceCenterPos = pResultCalib.DeviceCenterPos.Clone() as YoonVector2D;
            }
        }

        public IYoonResult Clone()
        {
            RotationCalibResult pTargetResult = new RotationCalibResult();

            pTargetResult.AverageResolution = AverageResolution.Clone() as YoonVector2D;
            pTargetResult.ResolutionOfParts = new Dictionary<eYoonDirRect, YoonVector2D>(ResolutionOfParts);
            pTargetResult.DeviceCenterPos = DeviceCenterPos.Clone() as YoonVector2D;
            return pTargetResult;
        }
    }

    public class StaticCalibResult : IYoonResult
    {
        public YoonVector2D AverageResolution { get; set; } = new YoonVector2D(0.0, 0.0);
        public Dictionary<eYoonDirCompass, YoonVector2D> ResolutionOfParts { get; set; } = new Dictionary<eYoonDirCompass, YoonVector2D>();
        public StaticCalibResult()
        {
            foreach (eYoonDirCompass nDir in Enum.GetValues(typeof(eYoonDirRect)))
                ResolutionOfParts.Add(nDir, new YoonVector2D());
        }

        public string Combine(string strDelimiter)
        {
            return AverageResolution.X.ToString() + strDelimiter +
                AverageResolution.Y.ToString() + strDelimiter +
                ResolutionOfParts.Count.ToString() + strDelimiter;
        }

        public bool Equals(IYoonResult pResult)
        {
            if (pResult is StaticCalibResult pResultCalib)
            {
                if (pResultCalib.AverageResolution != AverageResolution) return false;

                foreach (eYoonDirCompass nDir in ResolutionOfParts.Keys)
                {
                    if (!pResultCalib.ResolutionOfParts.ContainsKey(nDir) || pResultCalib.ResolutionOfParts[nDir] != ResolutionOfParts[nDir])
                        return false;
                }
                return true;
            }
            return false;
        }

        public void CopyFrom(IYoonResult pResult)
        {
            if (pResult == null) return;

            if (pResult is StaticCalibResult pResultCalib)
            {
                AverageResolution = pResultCalib.AverageResolution.Clone() as YoonVector2D;
                ResolutionOfParts = new Dictionary<eYoonDirCompass, YoonVector2D>(pResultCalib.ResolutionOfParts);
            }
        }

        public IYoonResult Clone()
        {
            StaticCalibResult pTargetResult = new StaticCalibResult();

            pTargetResult.AverageResolution = AverageResolution.Clone() as YoonVector2D;
            pTargetResult.ResolutionOfParts = new Dictionary<eYoonDirCompass, YoonVector2D>(ResolutionOfParts);
            return pTargetResult;
        }
    }

}
