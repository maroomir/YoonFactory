using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.LineMax;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.ColorExtractor;
using Cognex.VisionPro.ColorSegmenter;

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

    public class ResultSection : IYoonSection<string, CognexResult>
    {
        #region Supported IDisposable Pattern
        ~ResultSection()
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
            }
            //// .NET Framework에 의하여 관리되지 않는 외부 리소스들을 여기서 정리합니다.
            this.disposed = true;
            Clear();
            m_pDicCogResult = null;
            m_pListKeyOrdered = null;
            this.disposed = true;
        }
        #endregion

        public static IEqualityComparer<string> DefaultComparer = new CaseInsensitiveStringComparer();

        class CaseInsensitiveStringComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return string.Compare(x, y) == 0;
            }

            public int GetHashCode(string obj)
            {
                return obj.ToLower().GetHashCode();
            }
        }

        public string RootDirectory { get; set; } = string.Empty;
        public eYoonCognexType ParantsType { get; set; } = eYoonCognexType.None;

        private Dictionary<string, CognexResult> m_pDicCogResult;
        private List<string> m_pListKeyOrdered;

        public bool IsOrdered
        {
            get
            {
                return m_pListKeyOrdered != null;
            }
            set
            {
                if (IsOrdered != value)
                {
                    m_pListKeyOrdered = value ? new List<string>(m_pDicCogResult.Keys) : null;
                }
            }
        }


        public IEqualityComparer<string> Comparer { get { return m_pDicCogResult.Comparer; } }

        public CognexResult this[int nIndex]
        {
            get
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index ResultSection using integer key: container was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                return m_pDicCogResult[m_pListKeyOrdered[nIndex]];
            }
            set
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index ResultSection using integer key: container was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                var key = m_pListKeyOrdered[nIndex];
                m_pDicCogResult[key] = value;
            }
        }

        public CognexResult this[string strKey]
        {
            get
            {
                CognexResult pResult;
                if (m_pDicCogResult.TryGetValue(strKey, out pResult))
                {
                    return pResult;
                }
                return new CognexResult(ParantsType);
            }
            set
            {
                if (IsOrdered && !m_pListKeyOrdered.Contains(strKey, Comparer))
                {
                    m_pListKeyOrdered.Add(strKey);
                }
                m_pDicCogResult[strKey] = value;
            }
        }


        public ResultSection()
            : this(DefaultComparer)
        {
            //
        }

        public ResultSection(IEqualityComparer<string> pStringComparer)
        {
            this.m_pDicCogResult = new Dictionary<string, CognexResult>(pStringComparer);
        }

        public ResultSection(eYoonCognexType nType)
            : this(nType, DefaultComparer)
        {
            //
        }

        public ResultSection(eYoonCognexType nType, IEqualityComparer<string> pStringComparer)
        {
            this.ParantsType = nType;
            this.m_pDicCogResult = new Dictionary<string, CognexResult>(pStringComparer);
        }

        public ResultSection(Dictionary<string, CognexResult> pDic)
            : this(pDic, DefaultComparer)
        {
            //
        }

        public ResultSection(Dictionary<string, CognexResult> pDic, IEqualityComparer<string> pStringComparer)
        {
            this.m_pDicCogResult = new Dictionary<string, CognexResult>(pDic, pStringComparer);
        }

        public ResultSection(ResultSection pSection)
            : this(pSection, DefaultComparer)
        {
            //
        }

        public ResultSection(ResultSection pSection, IEqualityComparer<string> pStringComparer)
        {
            this.m_pDicCogResult = new Dictionary<string, CognexResult>(pSection.m_pDicCogResult, pStringComparer);
        }

        public void Clear()
        {
            if (m_pDicCogResult != null)
                m_pDicCogResult.Clear();
            if (IsOrdered)
            {
                m_pListKeyOrdered.Clear();
            }
        }

        private string GetCognexResultImagePath(eYoonCognexType pType, string strTag)
        {
            return Path.Combine(RootDirectory, string.Format(@"Result{0}{1}.bmp", pType.ToString(), strTag));
        }

        public bool LoadValue(string strKey)
        {
            return false;
        }

        public bool SaveValue(string strKey)
        {
            if (RootDirectory == string.Empty) return false;

            string strFilePath = GetCognexResultImagePath(ParantsType, strKey);
            ICogImage pImage = m_pDicCogResult[strKey].ResultImage;
            switch (pImage)
            {
                case CogImage24PlanarColor pImageColor:
                    CognexFactory.SaveColorImageToBitmap(pImageColor, strFilePath);
                    break;
                case CogImage8Grey pImageMono:
                    CognexFactory.SaveMonoImageToBitmap(pImageMono, strFilePath);
                    break;
                default:
                    return false;
            }
            return true;
        }

        public int IndexOf(string strKey)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string) on ResultSection: container was not ordered.");
            }
            return IndexOf(strKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(string strKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string, int) on ResultSection: container was not ordered.");
            }
            return IndexOf(strKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(string strKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string, int, int) on ResultSection: container was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            if (nCount < 0)
            {
                throw new IndexOutOfRangeException("Count cannot be less than zero." + Environment.NewLine + "Parameter name: nCount");
            }
            if (nIndex + nCount > m_pListKeyOrdered.Count)
            {
                throw new ArgumentException("Index and count were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }
            var end = nIndex + nCount;
            for (int i = nIndex; i < end; i++)
            {
                if (Comparer.Equals(m_pListKeyOrdered[i], strKey))
                {
                    return i;
                }
            }
            return -1;
        }

        public int LastIndexOf(string strKey)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(string) on ResultSection: container was not ordered.");
            }
            return LastIndexOf(strKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(string strKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(string, int) on ResultSection: container was not ordered.");
            }
            return LastIndexOf(strKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(string strKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(string, int, int) on ResultSection: container was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            if (nCount < 0)
            {
                throw new IndexOutOfRangeException("Count cannot be less than zero." + Environment.NewLine + "Parameter name : nCount");
            }
            if (nIndex + nCount > m_pListKeyOrdered.Count)
            {
                throw new ArgumentException("Index and Count were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }
            var end = nIndex + nCount;
            for (int i = end - 1; i >= nIndex; i--)
            {
                if (Comparer.Equals(m_pListKeyOrdered[i], strKey))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int nIndex, string strKey, CognexResult pValue)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Insert(int, string, CognexResult) on ResultSection: container was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            m_pDicCogResult.Add(strKey, pValue);
            m_pListKeyOrdered.Insert(nIndex, strKey);
        }

        public void InsertRange(int nIndex, IEnumerable<KeyValuePair<string, CognexResult>> pCollection)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<string, CognexResult>>) on ResultSection: container was not ordered.");
            }
            if (pCollection == null)
            {
                throw new ArgumentNullException("Value cannot be null." + Environment.NewLine + "Parameter name: pCollection");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            foreach (var kvp in pCollection)
            {
                Insert(nIndex, kvp.Key, kvp.Value);
                nIndex++;
            }
        }

        public void RemoveAt(int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call RemoveAt(int) on ResultSection: container was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            var key = m_pListKeyOrdered[nIndex];
            m_pListKeyOrdered.RemoveAt(nIndex);
            m_pDicCogResult.Remove(key);
        }

        public void RemoveRange(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on ResultSection: container was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            if (nCount < 0)
            {
                throw new IndexOutOfRangeException("Count cannot be less than zero." + Environment.NewLine + "Parameter name: nCount");
            }
            if (nIndex + nCount > m_pListKeyOrdered.Count)
            {
                throw new ArgumentException("Index and count were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }
            for (int i = 0; i < nCount; i++)
            {
                RemoveAt(nIndex);
            }
        }

        public void Reverse()
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Reverse() on ResultSection: container was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on ResultSection: container was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            if (nCount < 0)
            {
                throw new IndexOutOfRangeException("Count cannot be less than zero." + Environment.NewLine + "Parameter name: nCount");
            }
            if (nIndex + nCount > m_pListKeyOrdered.Count)
            {
                throw new ArgumentException("Index and count were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }
            m_pListKeyOrdered.Reverse(nIndex, nCount);
        }

        public ICollection<CognexResult> GetOrderedValues()
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call GetOrderedValues() on ResultSection: container was not ordered.");
            }
            var list = new List<CognexResult>();
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                list.Add(m_pDicCogResult[m_pListKeyOrdered[i]]);
            }
            return list;
        }

        public void Add(string strKey, CognexResult pValue)
        {
            m_pDicCogResult.Add(strKey, pValue);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(strKey);
            }
        }

        public bool ContainsKey(string strKey)
        {
            return m_pDicCogResult.ContainsKey(strKey);
        }

        public ICollection<string> Keys
        {
            get { return IsOrdered ? (ICollection<string>)m_pListKeyOrdered : m_pDicCogResult.Keys; }
        }

        public ICollection<CognexResult> Values
        {
            get
            {
                return m_pDicCogResult.Values;
            }
        }

        public bool Remove(string strKey)
        {
            var ret = m_pDicCogResult.Remove(strKey);
            if (IsOrdered && ret)
            {
                for (int i = 0; i < m_pListKeyOrdered.Count; i++)
                {
                    if (Comparer.Equals(m_pListKeyOrdered[i], strKey))
                    {
                        m_pListKeyOrdered.RemoveAt(i);
                        break;
                    }
                }
            }
            return ret;
        }

        public bool TryGetValue(string strKey, out CognexResult pValue)
        {
            return m_pDicCogResult.TryGetValue(strKey, out pValue);
        }

        public int Count
        {
            get { return m_pDicCogResult.Count; }
        }

        void ICollection<KeyValuePair<string, CognexResult>>.Add(KeyValuePair<string, CognexResult> pCollection)
        {
            ((IDictionary<string, CognexResult>)m_pDicCogResult).Add(pCollection);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(pCollection.Key);
            }
        }

        bool ICollection<KeyValuePair<string, CognexResult>>.Contains(KeyValuePair<string, CognexResult> pCollection)
        {
            return ((IDictionary<string, CognexResult>)m_pDicCogResult).Contains(pCollection);
        }

        void ICollection<KeyValuePair<string, CognexResult>>.CopyTo(KeyValuePair<string, CognexResult>[] pArray, int nIndexArray)
        {
            ((IDictionary<string, CognexResult>)m_pDicCogResult).CopyTo(pArray, nIndexArray);
        }

        bool ICollection<KeyValuePair<string, CognexResult>>.IsReadOnly
        {
            get { return ((IDictionary<string, CognexResult>)m_pDicCogResult).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<string, CognexResult>>.Remove(KeyValuePair<string, CognexResult> pCollection)
        {
            var ret = ((IDictionary<string, CognexResult>)m_pDicCogResult).Remove(pCollection);
            if (IsOrdered && ret)
            {
                for (int i = 0; i < m_pListKeyOrdered.Count; i++)
                {
                    if (Comparer.Equals(m_pListKeyOrdered[i], pCollection.Key))
                    {
                        m_pListKeyOrdered.RemoveAt(i);
                        break;
                    }
                }
            }
            return ret;
        }

        public IEnumerator<KeyValuePair<string, CognexResult>> GetEnumerator()
        {
            if (IsOrdered)
            {
                return GetOrderedEnumerator();
            }
            else
            {
                return m_pDicCogResult.GetEnumerator();
            }
        }

        private IEnumerator<KeyValuePair<string, CognexResult>> GetOrderedEnumerator()
        {
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                yield return new KeyValuePair<string, CognexResult>(m_pListKeyOrdered[i], m_pDicCogResult[m_pListKeyOrdered[i]]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator ResultSection(Dictionary<string, CognexResult> pDic)
        {
            return new ResultSection(pDic);
        }

        public static explicit operator Dictionary<string, CognexResult>(ResultSection pContainer)
        {
            return pContainer.m_pDicCogResult;
        }
    }
}
