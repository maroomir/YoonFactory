using System;
using System.Collections.Generic;
using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using YoonFactory.Image;

namespace YoonFactory.Cognex.Result
{
    public class CognexResult : IYoonResult
    {
        public CognexImage ResultImage { get; private set; } = null;
        public eYoonCognexType ToolType { get; private set; } = eYoonCognexType.None;
        public Dictionary<int, ICogShape> CogShapeDictionary { get; private set; } = new Dictionary<int, ICogShape>();
        public YoonDataset ObjectDataset { get; private set; } = new YoonDataset();
        public double TotalScore { get; private set; } = 0.0;

        public CognexResult(eYoonCognexType nType)
        {
            ToolType = nType;
            ResultImage = new CognexImage();
        }

        public CognexResult(eYoonCognexType nType, CognexImage pImageResult)
        {
            ToolType = nType;
            if (pImageResult != null)
                ResultImage = pImageResult.Clone() as CognexImage;
        }

        public CognexResult(eYoonCognexType nType, CognexImage pImageResult, double dScore)
        {
            ToolType = nType;
            if (pImageResult != null)
                ResultImage = pImageResult.Clone() as CognexImage;
            TotalScore = dScore;
        }

        public CognexResult(CognexImage pImageResult, CogBlobResultCollection pListResult)
        {
            ToolType = eYoonCognexType.Blob;
            if (pImageResult != null)
                ResultImage = pImageResult.Clone() as CognexImage;

            foreach (CogBlobResult pResult in pListResult)
            {
                CogRectangleAffine pCogRect = pResult.GetBoundingBox(CogBlobAxisConstants.Principal);
                CogShapeDictionary.Add(pResult.ID, new CogRectangleAffine(pCogRect));
                YoonObject pObject = new YoonObject();
                {
                    pObject.Label = pResult.ID;
                    pObject.Feature = new YoonRect2D(pCogRect.CenterX - pCogRect.SideXLength / 2, pCogRect.CenterY - pCogRect.SideYLength / 2, pCogRect.SideXLength, pCogRect.SideYLength);
                    pObject.ReferencePosition = new YoonVector2D(pCogRect.CenterX, pCogRect.CenterY);
                    pObject.ObjectImage = pImageResult.CropImage(pCogRect.ToYoonRectAffine());
                    pObject.PixelCount = (int)pResult.Area;
                }
                ObjectDataset.Add(pObject);
            }
        }

        public CognexResult(CognexImage pImageResult, CogLineSegment pLine)
        {
            ToolType = eYoonCognexType.LineFitting;
            if (pImageResult != null)
                ResultImage = pImageResult.Clone() as CognexImage;

            YoonObject pObject = new YoonObject();
            {
                pObject.Label = 0;
                YoonLine2D pObjectLine = new YoonLine2D(new YoonVector2D(pLine.StartX, pLine.StartY), new YoonVector2D(pLine.EndX, pLine.EndY));
                pObject.Feature = pObjectLine.Clone();
                pObject.ReferencePosition = pObjectLine.CenterPos.Clone();
                pObject.ObjectImage = pImageResult.CropImage(pObjectLine.Area);
                pObject.PixelCount = (int)pObjectLine.Length;
            }
            ObjectDataset.Add(pObject);
        }

        public CognexResult(CognexImage pImageResult, CogTransform2DLinear pResult, ICogRegion pPattern, double dScore)
        {
            ToolType = eYoonCognexType.PMAlign;
            if (pImageResult != null)
                ResultImage = pImageResult.Clone() as CognexImage;

            YoonObject pObject = new YoonObject();
            {
                YoonVector2D vecFeature = new YoonVector2D(pResult.TranslationX, pResult.TranslationY);
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
                pObject.Label = 0;
                YoonRectAffine2D pRectAffine = new YoonRectAffine2D(vecFeature, dWidth, dHeight, pResult.Rotation);
                pObject.Feature = pRectAffine.Clone();
                pObject.ReferencePosition = vecFeature.Clone();
                pObject.ObjectImage = pImageResult.CropImage(pRectAffine);
                pObject.Score = dScore * 100;
            }
            ObjectDataset.Add(pObject);
            TotalScore = dScore;
        }

        public string Combine(string strDelimiter)
        {
            return ToolType.ToString() + strDelimiter +
                CogShapeDictionary.Count.ToString() + strDelimiter +
                ObjectDataset.Count.ToString() + strDelimiter +
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
                for (int iObject = 0; iObject < ObjectDataset.Count; iObject++)
                    if (pResultCognex.ObjectDataset[iObject] != ObjectDataset[iObject])
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
                    ResultImage = pResultCognex.ResultImage.Clone() as CognexImage;
                ToolType = pResultCognex.ToolType;
                CogShapeDictionary = new Dictionary<int, ICogShape>(pResultCognex.CogShapeDictionary);
                ObjectDataset = pResultCognex.ObjectDataset.Clone();
                TotalScore = pResultCognex.TotalScore;
            }
        }

        public IYoonResult Clone()
        {
            CognexResult pTargetResult = new CognexResult(ToolType);

            if (ResultImage != null)
                pTargetResult.ResultImage = ResultImage.Clone() as CognexImage;
            pTargetResult.CogShapeDictionary = new Dictionary<int, ICogShape>(CogShapeDictionary);
            pTargetResult.ObjectDataset = ObjectDataset.Clone();
            pTargetResult.TotalScore = TotalScore;
            return pTargetResult;
        }

        public YoonVector2D GetPatternMatchPoint()
        {
            if (ToolType != eYoonCognexType.PMAlign) return new YoonVector2D();

            if (ObjectDataset[0].Feature is YoonRect2D pRect)
                return pRect.CenterPos as YoonVector2D;
            else if (ObjectDataset[0].Feature is YoonRectAffine2D pRectAffine)
                return pRectAffine.CenterPos as YoonVector2D;
            else
                throw new FormatException("[YOONCOGNEX] Object format is not correct");
        }

        public YoonRectAffine2D GetPatternMatchArea()
        {
            if (ToolType != eYoonCognexType.PMAlign) return new YoonRectAffine2D(0, 0, 0);

            else if (ObjectDataset[0].Feature is YoonRectAffine2D pRectAffine)
                return pRectAffine;
            else
                throw new FormatException("[YOONCOGNEX] Object format is not correct");
        }

        public double GetPatternRotation()
        {
            if (ToolType != eYoonCognexType.PMAlign) return 0.0;
            if (ObjectDataset[0].Feature is YoonRectAffine2D pRect)
                return pRect.Rotation;
            throw new FormatException("[YOONCOGNEX] Object format is not correct");
        }
    }
}
