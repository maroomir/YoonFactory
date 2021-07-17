using System;
using System.Collections.Generic;

namespace YoonFactory.Calibration
{

    public class RotationCalibResult : IYoonResult
    {
        public YoonVector2D AverageResolution { get; set; } = new YoonVector2D(0.0, 0.0);
        public Dictionary<eYoonDir2D, YoonVector2D> ResolutionOfParts { get; set; } = new Dictionary<eYoonDir2D, YoonVector2D>();
        public YoonVector2D DeviceCenterPos { get; set; } = new YoonVector2D();

        public RotationCalibResult()
        {
            foreach (eYoonDir2D nDir in YoonDirFactory.GetSquareDirections())
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

        public bool Insert(string strCombineResult, string strDelimiter = ",")
        {
            string[] pArrayResult = strCombineResult.Split(strDelimiter.ToCharArray()[0]);
            if (pArrayResult?.Length != 5) return false;
            AverageResolution.X = Convert.ToDouble(pArrayResult[0]);
            AverageResolution.Y = Convert.ToDouble(pArrayResult[1]);
            DeviceCenterPos.X = Convert.ToDouble(pArrayResult[3]);
            DeviceCenterPos.Y = Convert.ToDouble(pArrayResult[4]);
            return true;
        }

        public bool Equals(IYoonResult pResult)
        {
            if (pResult is RotationCalibResult pResultCalib)
            {
                if (pResultCalib.AverageResolution != AverageResolution || pResultCalib.DeviceCenterPos != DeviceCenterPos)
                    return false;

                foreach (eYoonDir2D nDir in ResolutionOfParts.Keys)
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
                ResolutionOfParts = new Dictionary<eYoonDir2D, YoonVector2D>(pResultCalib.ResolutionOfParts);
                DeviceCenterPos = pResultCalib.DeviceCenterPos.Clone() as YoonVector2D;
            }
        }

        public IYoonResult Clone()
        {
            RotationCalibResult pTargetResult = new RotationCalibResult();

            pTargetResult.AverageResolution = AverageResolution.Clone() as YoonVector2D;
            pTargetResult.ResolutionOfParts = new Dictionary<eYoonDir2D, YoonVector2D>(ResolutionOfParts);
            pTargetResult.DeviceCenterPos = DeviceCenterPos.Clone() as YoonVector2D;
            return pTargetResult;
        }
    }

    public class StaticCalibResult : IYoonResult
    {
        public YoonVector2D AverageResolution { get; set; } = new YoonVector2D(0.0, 0.0);
        public Dictionary<eYoonDir2D, YoonVector2D> ResolutionOfParts { get; set; } = new Dictionary<eYoonDir2D, YoonVector2D>();
        public StaticCalibResult()
        {
            foreach (eYoonDir2D nDir in Enum.GetValues(typeof(eYoonDir2D)))
                ResolutionOfParts.Add(nDir, new YoonVector2D());
        }

        public string Combine(string strDelimiter)
        {
            return AverageResolution.X.ToString() + strDelimiter +
                AverageResolution.Y.ToString() + strDelimiter +
                ResolutionOfParts.Count.ToString() + strDelimiter;
        }

        public bool Insert(string strCombineResult, string strDelimiter = ",")
        {
            string[] pArrayResult = strCombineResult.Split(strDelimiter.ToCharArray()[0]);
            if (pArrayResult?.Length != 3) return false;
            AverageResolution.X = Convert.ToDouble(pArrayResult[0]);
            AverageResolution.Y = Convert.ToDouble(pArrayResult[1]);
            return true;
        }

        public bool Equals(IYoonResult pResult)
        {
            if (pResult is StaticCalibResult pResultCalib)
            {
                if (pResultCalib.AverageResolution != AverageResolution) return false;

                foreach (eYoonDir2D nDir in ResolutionOfParts.Keys)
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
                ResolutionOfParts = new Dictionary<eYoonDir2D, YoonVector2D>(pResultCalib.ResolutionOfParts);
            }
        }

        public IYoonResult Clone()
        {
            StaticCalibResult pTargetResult = new StaticCalibResult();

            pTargetResult.AverageResolution = AverageResolution.Clone() as YoonVector2D;
            pTargetResult.ResolutionOfParts = new Dictionary<eYoonDir2D, YoonVector2D>(ResolutionOfParts);
            return pTargetResult;
        }
    }

}
