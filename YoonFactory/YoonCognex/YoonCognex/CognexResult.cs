using System.Collections.Generic;
using Cognex.VisionPro;
using Cognex.VisionPro.Blob;

namespace YoonFactory.Cognex
{
    public class CognexResult : IYoonResult
    {
        public ICogImage ResultImage { get; private set; } = null;
        public eYoonCognexType ToolType { get; private set; } = eYoonCognexType.None;
        public Dictionary<int, ICogShape> CogShapeDictionary { get; private set; } = new Dictionary<int, ICogShape>();
        public Dictionary<int, IYoonObject> ObjectDictionary { get; private set; } = new Dictionary<int, IYoonObject>();
        public double TotalScore { get; private set; } = 0.0;

        public CognexResult(eYoonCognexType nType)
        {
            ToolType = nType;
            ResultImage = new CogImage8Grey();
        }

        public CognexResult(eYoonCognexType nType, ICogImage pImageResult)
        {
            ToolType = nType;
            if (pImageResult != null)
                ResultImage = pImageResult.CopyBase(CogImageCopyModeConstants.CopyPixels);
        }

        public CognexResult(eYoonCognexType nType, ICogImage pImageResult, double dScore)
        {
            ToolType = nType;
            if (pImageResult != null)
                ResultImage = pImageResult.CopyBase(CogImageCopyModeConstants.CopyPixels);
            TotalScore = dScore;
        }

        public CognexResult(ICogImage pImageResult, CogBlobResultCollection pListResult)
        {
            ToolType = eYoonCognexType.Blob;
            if (pImageResult != null)
                ResultImage = pImageResult.CopyBase(CogImageCopyModeConstants.CopyPixels);

            foreach (CogBlobResult pResult in pListResult)
            {
                CogRectangleAffine pCogRect = pResult.GetBoundingBox(CogBlobAxisConstants.Principal);
                CogShapeDictionary.Add(pResult.ID, new CogRectangleAffine(pCogRect));
                YoonRectObject pObject = new YoonRectObject();
                {
                    pObject.LabelNo = pResult.ID;
                    pObject.FeaturePos = new YoonVector2D(pResult.CenterOfMassX, pResult.CenterOfMassY);
                    pObject.PixelCount = (int)pResult.Area;
                    pObject.PickArea = new YoonRect2D(pCogRect.CenterX - pCogRect.SideXLength / 2, pCogRect.CenterY - pCogRect.SideYLength / 2, pCogRect.SideXLength, pCogRect.SideYLength);
                }
                ObjectDictionary.Add(pResult.ID, pObject);
            }
        }

        public CognexResult(ICogImage pImageResult, CogLineSegment pLine)
        {
            ToolType = eYoonCognexType.LineFitting;
            if (pImageResult != null)
                ResultImage = pImageResult.CopyBase(CogImageCopyModeConstants.CopyPixels);

            YoonLineObject pObject = new YoonLineObject();
            {
                pObject.StartPos = new YoonVector2D();
                (pObject.StartPos as YoonVector2D).X = pLine.StartX;
                (pObject.StartPos as YoonVector2D).Y = pLine.StartY;
                pObject.EndPos = new YoonVector2D();
                (pObject.EndPos as YoonVector2D).X = pLine.EndX;
                (pObject.EndPos as YoonVector2D).Y = pLine.EndY;
            }
            ObjectDictionary.Add(0, pObject);
        }

        public CognexResult(ICogImage pImageResult, CogTransform2DLinear pResult, ICogRegion pPattern, double dScore)
        {
            ToolType = eYoonCognexType.PMAlign;
            if (pImageResult != null)
                ResultImage = pImageResult.CopyBase(CogImageCopyModeConstants.CopyPixels);

            YoonRectObject pObject = new YoonRectObject();
            {
                pObject.LabelNo = 0;
                pObject.FeaturePos = new YoonVector2D(pResult.TranslationX, pResult.TranslationY);
                double dScaleX = pResult.ScalingX;
                double dScaleY = pResult.ScalingY;
                double dWidth = 0.0, dHeight = 0.0;
                switch (pPattern)
                {
                    case CogRectangle pPatternRect:
                        dWidth = pPatternRect.Width * dScaleX;
                        dHeight = pPatternRect.Height * dScaleY;
                        break;
                    case CogRectangleAffine pPatternRectAffine:
                        dWidth = pPatternRectAffine.SideXLength * dScaleX;
                        dHeight = pPatternRectAffine.SideYLength * dScaleY;
                        break;
                    default:
                        break;
                }
                pObject.PickArea = new YoonRectAffine2D(pObject.FeaturePos as YoonVector2D, dWidth, dHeight, pResult.Rotation);
                pObject.Score = dScore * 100;
            }
            ObjectDictionary.Add(0, pObject);
            TotalScore = dScore;
        }

        public string Combine(string strDelimiter)
        {
            return ToolType.ToString() + strDelimiter +
                CogShapeDictionary.Count.ToString() + strDelimiter +
                ObjectDictionary.Count.ToString() + strDelimiter +
                TotalScore.ToString();
        }

        public bool Equals(IYoonResult pResult)
        {
            if (pResult == null) return false;

            if (pResult is CognexResult pResultCognex)
            {
                if (!ResultImage.Equals(pResultCognex.ResultImage) || ToolType != pResultCognex.ToolType || TotalScore != pResultCognex.TotalScore)
                    return false;

                foreach (int nNo in CogShapeDictionary.Keys)
                    if (!pResultCognex.CogShapeDictionary.ContainsKey(nNo) || pResultCognex.CogShapeDictionary[nNo] != CogShapeDictionary[nNo])
                        return false;
                foreach (int nNo in ObjectDictionary.Keys)
                    if (!pResultCognex.ObjectDictionary.ContainsKey(nNo) || pResultCognex.ObjectDictionary[nNo] != ObjectDictionary[nNo])
                        return false;

                return true;
            }
            return false;
        }

        public void CopyFrom(IYoonResult pResult)
        {
            if (pResult == null) return;

            if (pResult is CognexResult pResultCognex)
            {
                if (pResultCognex.ResultImage != null)
                    ResultImage = pResultCognex.ResultImage.CopyBase(CogImageCopyModeConstants.CopyPixels);
                ToolType = pResultCognex.ToolType;
                CogShapeDictionary = new Dictionary<int, ICogShape>(pResultCognex.CogShapeDictionary);
                ObjectDictionary = new Dictionary<int, IYoonObject>(pResultCognex.ObjectDictionary);
                TotalScore = pResultCognex.TotalScore;
            }
        }

        public IYoonResult Clone()
        {
            CognexResult pTargetResult = new CognexResult(ToolType);

            if (ResultImage != null)
                pTargetResult.ResultImage = ResultImage.CopyBase(CogImageCopyModeConstants.CopyPixels);
            pTargetResult.CogShapeDictionary = new Dictionary<int, ICogShape>(CogShapeDictionary);
            pTargetResult.ObjectDictionary = new Dictionary<int, IYoonObject>(ObjectDictionary);
            pTargetResult.TotalScore = TotalScore;
            return pTargetResult;
        }

        public YoonVector2D GetPatternMatchPoint()
        {
            if (ToolType != eYoonCognexType.PMAlign) return new YoonVector2D();

            if (ObjectDictionary[0] is YoonRectObject pObject)
                return pObject.FeaturePos as YoonVector2D;
            else
                return new YoonVector2D();
        }

        public YoonRectAffine2D GetPatternMatchArea()
        {
            if (ToolType != eYoonCognexType.PMAlign) return new YoonRectAffine2D(0, 0, 0);

            if (ObjectDictionary[0] is YoonRectObject pObject)
                return pObject.PickArea as YoonRectAffine2D;
            else
                return new YoonRectAffine2D(0, 0, 0);
        }

        public double GetPatternRotation()
        {
            if (ToolType != eYoonCognexType.PMAlign) return 0.0;

            if (ObjectDictionary[0] is YoonRectObject pObject)
            {
                if (pObject.PickArea is YoonRectAffine2D pRect)
                    return pRect.Rotation;
                else
                    return 0.0;
            }
            else
                return 0.0;
        }
    }

}
