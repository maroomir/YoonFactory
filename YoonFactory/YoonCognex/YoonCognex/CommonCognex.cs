using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.Display;
using System.Runtime.InteropServices;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.LineMax;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.ColorExtractor;
using Cognex.VisionPro.ColorSegmenter;
using YoonFactory.Files;

namespace YoonFactory.Cognex
{
    public enum eYoonCognexType : int
    {
        None = -1,
        // Calibration (Undistort)
        Calibration,
        // Image 전처리
        Convert,
        Sobel,
        Sharpness,
        Filtering,
        // 추출
        Blob,
        ColorExtract,
        ColorSegment,
        // 패턴 매칭
        PMAlign,
        // 찾기 (Line, Circle 등)
        LineFitting,
        // 영상 비교
        ImageSubtract,
        ImageAdd,
        ImageMinMax,
    }

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
            if(vecPixelPos is YoonVector2D vecPos)
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
            if(pMapping is CognexMapping pMappingCognex)
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

    public class CogToolContainer : IYoonContainer, IYoonContainer<ICogTool>
    {
        #region Supported IDisposable Pattern
        ~CogToolContainer()
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
                if (ObjectDictionary != null)
                    ObjectDictionary.Clear();
                ObjectDictionary = null;

            }
            //// .NET Framework에 의하여 관리되지 않는 외부 리소스들을 여기서 정리합니다.
            this.disposed = true;
        }
        #endregion

        public Dictionary<string, ICogTool> ObjectDictionary { get; private set; } = new Dictionary<string, ICogTool>();
        public string RootDirectory { get; set; }

        public void CopyFrom(IYoonContainer pContainer)
        {
            if(pContainer is CogToolContainer pToolContainer)
            {
                ObjectDictionary.Clear();

                RootDirectory = pToolContainer.RootDirectory;
                ObjectDictionary = new Dictionary<string, ICogTool>(pToolContainer.ObjectDictionary);
            }
        }

        public IYoonContainer Clone()
        {
            CogToolContainer pContainer = new CogToolContainer();
            pContainer.RootDirectory = RootDirectory;
            pContainer.ObjectDictionary = new Dictionary<string, ICogTool>(ObjectDictionary);
            return pContainer;
        }

        public bool Add(string strKey, ICogTool pCogTool)
        {
            try
            {
                ObjectDictionary.Add(strKey, pCogTool);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Add(eYoonCognexType nType, string strTag, ICogTool pCogTool)
        {
            string strKey = nType.ToString() + "_" + strTag;
            try
            {
                ObjectDictionary.Add(strKey, pCogTool);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Remove(string strKey)
        {
            return ObjectDictionary.Remove(strKey);
        }

        public bool Remove(eYoonCognexType nType, string strTag)
        {
            string strKey = nType.ToString() + "_" + strTag;
            return ObjectDictionary.Remove(strKey);
        }

        public void Clear()
        {
            ObjectDictionary.Clear();
        }

        public string GetKey(ICogTool pCogTool)
        {
            foreach(string strKey in ObjectDictionary.Keys)
            {
                if (ObjectDictionary[strKey] == pCogTool)
                    return strKey;
            }
            return string.Empty;
        }

        public ICogTool GetValue(string strKey)
        {
            if (ObjectDictionary.ContainsKey(strKey))
                return ObjectDictionary[strKey];
            else
                return null;
        }

        public ICogTool GetValue(eYoonCognexType nType, string strTag)
        {
            string strKeys = nType.ToString() + "_" + strTag;
            if (ObjectDictionary.ContainsKey(strKeys))
                return ObjectDictionary[strKeys];
            else
                return CogToolFactory.InitCognexTool(nType);
        }

        public void SetValue(string strKey, ICogTool pCogTool)
        {
            if (ObjectDictionary.ContainsKey(strKey))
                ObjectDictionary[strKey] = pCogTool;
            else
                Add(strKey, pCogTool);
        }

        public void SetValue(eYoonCognexType nType, string strTag, ICogTool pCogTool)
        {
            string strKeys = nType.ToString() + "_" + strTag;
            if (ObjectDictionary.ContainsKey(strKeys))
                ObjectDictionary[strKeys] = pCogTool;
            else
                Add(nType, strTag, pCogTool);
        }

        public bool LoadValue(string strKey)
        {
            if (RootDirectory == string.Empty) return false;

            eYoonCognexType nType = eYoonCognexType.None;
            string strTag = string.Empty;
            ConvertKey(strKey, ref nType, ref strTag);
            string strFilePath = GetCognexToolFilePath(nType, strTag);
            ICogTool pCogTool = CogToolFactory.LoadCognexToolFromVpp(strFilePath);
            if (pCogTool == null) return false;

            SetValue(nType, strTag, pCogTool);
            return true;
        }

        public bool LoadValue(eYoonCognexType nType, string strTag)
        {
            if (RootDirectory == string.Empty) return false;

            string strFilePath = GetCognexToolFilePath(nType, strTag);
            ICogTool pCogTool = CogToolFactory.LoadCognexToolFromVpp(strFilePath);
            if (pCogTool == null) return false;

            SetValue(nType, strTag, pCogTool);
            return true;
        }

        public bool SaveValue(string strKey)
        {
            if (RootDirectory == string.Empty) return false;

            eYoonCognexType nType = eYoonCognexType.None;
            string strTag = string.Empty;
            ConvertKey(strKey, ref nType, ref strTag);
            string strFilePath = GetCognexToolFilePath(nType, strTag);
            ICogTool pCogTool = GetValue(nType, strTag);
            return CogToolFactory.SaveCognexToolToVpp(pCogTool, strFilePath);
        }

        public bool SaveValue(eYoonCognexType nType, string strTag)
        {
            if (RootDirectory == string.Empty) return false;

            string strFilePath = GetCognexToolFilePath(nType, strTag);
            ICogTool pCogTool = GetValue(nType, strTag);
            return CogToolFactory.SaveCognexToolToVpp(pCogTool, strFilePath);
        }

        public void ConvertKey(string strKey, ref eYoonCognexType nType, ref string strTag)
        {
            string[] strKeys = strKey.Split('_');
            if (strKeys.Length >= 2)
            {
                nType = (eYoonCognexType)Enum.Parse(typeof(eYoonCognexType), strKeys[0]);
                strTag = strKeys[1];
            }
        }

        private string GetCognexToolFilePath(eYoonCognexType pType, string strTag)
        {
            switch (pType)
            {
                case eYoonCognexType.Blob:
                    return Path.Combine(RootDirectory, string.Format(@"CogBlobTool{0}.vpp", strTag));
                case eYoonCognexType.Calibration:
                    return Path.Combine(RootDirectory, string.Format(@"CogCalibCheckerBoardTool{0}.vpp", strTag));
                case eYoonCognexType.ColorExtract:
                    return Path.Combine(RootDirectory, string.Format(@"CogColorExtractorTool{0}.vpp", strTag));
                case eYoonCognexType.ColorSegment:
                    return Path.Combine(RootDirectory, string.Format(@"CogColorSegmenterTool{0}.vpp", strTag));
                case eYoonCognexType.LineFitting:
                    return Path.Combine(RootDirectory, string.Format(@"CogLineFittingTool{0}.vpp", strTag));
                case eYoonCognexType.Filtering:
                    return Path.Combine(RootDirectory, string.Format(@"CogIPOneImageTool{0}.vpp", strTag));
                case eYoonCognexType.Convert:
                    return Path.Combine(RootDirectory, string.Format(@"CogImageConvertTool{0}.vpp", strTag));
                case eYoonCognexType.Sharpness:
                    return Path.Combine(RootDirectory, string.Format(@"CogImageSharpnessTool{0}.vpp", strTag));
                case eYoonCognexType.Sobel:
                    return Path.Combine(RootDirectory, string.Format(@"CogSobelEdgeTool{0}.vpp", strTag));
                case eYoonCognexType.PMAlign:
                    return Path.Combine(RootDirectory, string.Format(@"CogMainPMAlignTool{0}.vpp", strTag));
                case eYoonCognexType.ImageAdd:
                    return Path.Combine(RootDirectory, string.Format(@"CogIPTwoImageAddTool{0}.vpp", strTag));
                case eYoonCognexType.ImageMinMax:
                    return Path.Combine(RootDirectory, string.Format(@"CogIPTwoImageMinMaxTool{0}.vpp", strTag));
                case eYoonCognexType.ImageSubtract:
                    return Path.Combine(RootDirectory, string.Format(@"CogIPTwoImageSubstractTool{0}.vpp", strTag));
                default:
                    break;
            }
            return string.Empty;
        }
    }

    public class CogResultContainer : IYoonContainer, IYoonContainer<CognexResult>
    {

        #region Supported IDisposable Pattern
        ~CogResultContainer()
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
                if (ObjectDictionary != null)
                    ObjectDictionary.Clear();
                ObjectDictionary = null;

            }
            //// .NET Framework에 의하여 관리되지 않는 외부 리소스들을 여기서 정리합니다.
            this.disposed = true;
        }
        #endregion

        public Dictionary<string, CognexResult> ObjectDictionary { get; private set; } = new Dictionary<string, CognexResult>();
        public string RootDirectory { get; set; }

        public void CopyFrom(IYoonContainer pContainer)
        {
            if (pContainer is CogResultContainer pResultContainer)
            {
                ObjectDictionary.Clear();
                ObjectDictionary = new Dictionary<string, CognexResult>(pResultContainer.ObjectDictionary);
                RootDirectory = pResultContainer.RootDirectory;
            }
        }

        public IYoonContainer Clone()
        {
            CogResultContainer pContainer = new CogResultContainer();
            pContainer.ObjectDictionary = new Dictionary<string, CognexResult>(ObjectDictionary);
            pContainer.RootDirectory = RootDirectory;
            return pContainer;
        }

        public bool Add(string strKey, CognexResult pResult)
        {
            try
            {
                ObjectDictionary.Add(strKey, pResult);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Add(eYoonCognexType nType, string strTag, CognexResult pResult)
        {
            try
            {
                string strKeys = nType.ToString() + "_" + strTag;
                ObjectDictionary.Add(strKeys, pResult);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Remove(eYoonCognexType nType, string strTag)
        {
            string strKeys = nType.ToString() + "_" + strTag;
            return ObjectDictionary.Remove(strKeys);
        }

        public bool Remove(string strKey)
        {
            return ObjectDictionary.Remove(strKey);
        }

        public void Clear()
        {
            ObjectDictionary.Clear();
        }

        public string GetKey(CognexResult pResult)
        {
            foreach (string strKey in ObjectDictionary.Keys)
            {
                if (ObjectDictionary[strKey] == pResult)
                    return strKey;
            }
            return string.Empty;
        }

        public CognexResult GetValue(string strKey)
        {
            if (ObjectDictionary.ContainsKey(strKey))
                return ObjectDictionary[strKey];
            else
                return null;
        }

        public CognexResult GetValue(eYoonCognexType nType, string strTag)
        {
            string strKeys = nType.ToString() + "_" + strTag;
            if (ObjectDictionary.ContainsKey(strKeys))
                return ObjectDictionary[strKeys];
            else
                return null;
        }

        public void SetValue(string strKey, CognexResult pResult)
        {
            if (ObjectDictionary.ContainsKey(strKey))
                ObjectDictionary[strKey] = pResult;
            else
                Add(strKey, pResult);
        }

        public void SetValue(eYoonCognexType nType, string strTag, CognexResult pResult)
        {
            string strKeys = nType.ToString() + "_" + strTag;
            if (ObjectDictionary.ContainsKey(strKeys))
                ObjectDictionary[strKeys] = pResult;
            else
                Add(nType, strTag, pResult);
        }

        public bool LoadValue(string strKey)
        {
            return false;
        }

        public bool SaveValue(string strKey)
        {
            if (RootDirectory == null || RootDirectory== string.Empty) return false;

            eYoonCognexType nType = eYoonCognexType.None;
            string strTag = string.Empty;
            ConvertKey(strKey, ref nType, ref strTag);
            string strFilePath = GetCognexResultImagePath(nType, strTag);
            ICogImage pImage = GetValue(nType, strTag).ResultImage;
            if (pImage is CogImage24PlanarColor pImageColor)
                CognexFactory.SaveColorImageToBitmap(pImageColor, strFilePath);
            else if (pImage is CogImage8Grey pImageMono)
                CognexFactory.SaveMonoImageToBitmap(pImageMono, strFilePath);
            return true;
        }

        public bool SaveValue(eYoonCognexType nType, string strTag)
        {
            if (RootDirectory == null || RootDirectory == string.Empty) return false;

            string strFilePath = GetCognexResultImagePath(nType, strTag);
            ICogImage pImage = GetValue(nType, strTag).ResultImage;
            if (pImage is CogImage24PlanarColor pImageColor)
                CognexFactory.SaveColorImageToBitmap(pImageColor, strFilePath);
            else if (pImage is CogImage8Grey pImageMono)
                CognexFactory.SaveMonoImageToBitmap(pImageMono, strFilePath);
            return true;
        }

        public void ConvertKey(string strKey, ref eYoonCognexType nType, ref string strTag)
        {
            string[] strKeys = strKey.Split('_');
            if (strKeys.Length >= 2)
            {
                nType = (eYoonCognexType)Enum.Parse(typeof(eYoonCognexType), strKeys[0]);
                strTag = strKeys[1];
            }
        }

        private string GetCognexResultImagePath(eYoonCognexType pType, string strTag)
        {
            return Path.Combine(RootDirectory, string.Format(@"Result{0}{1}.bmp", pType.ToString(), strTag));
        }
    }

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
                YoonObjectRect pObject = new YoonObjectRect();
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

            YoonObjectLine pObject = new YoonObjectLine();
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

            YoonObjectRect pObject = new YoonObjectRect();
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

        public bool IsEqual(IYoonResult pResult)
        {
            if (pResult == null) return false;

            if (pResult is CognexResult pResultCognex)
            {
                if (!ResultImage.Equals(pResultCognex.ResultImage) || ToolType != pResultCognex.ToolType || TotalScore != pResultCognex.TotalScore)
                    return false;

                foreach(int nNo in CogShapeDictionary.Keys)
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

            if(pResult is CognexResult pResultCognex)
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

            if (ObjectDictionary[0] is YoonObjectRect pObject)
                return pObject.FeaturePos as YoonVector2D;
            else
                return new YoonVector2D();
        }

        public YoonRectAffine2D GetPatternMatchArea()
        {
            if (ToolType != eYoonCognexType.PMAlign) return new YoonRectAffine2D(0, 0, 0);

            if (ObjectDictionary[0] is YoonObjectRect pObject)
                return pObject.PickArea as YoonRectAffine2D;
            else
                return new YoonRectAffine2D(0, 0, 0);
        }

        public double GetPatternRotation()
        {
            if (ToolType != eYoonCognexType.PMAlign) return 0.0;

            if (ObjectDictionary[0] is YoonObjectRect pObject)
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
