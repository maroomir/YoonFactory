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

    public class ToolContainer : IYoonContainer, IYoonContainer<eYoonCognexType, ToolSection>
    {
        #region Supported IDisposable Pattern
        ~ToolContainer()
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
            m_pDicSection = null;
            m_pListKeyOrdered = null;
            this.disposed = true;
        }
        #endregion

        public static IEqualityComparer<eYoonCognexType> DefaultComparer;

        class CaseInsensitiveTypeComparer : IEqualityComparer<eYoonCognexType>
        {
            public bool Equals(eYoonCognexType x, eYoonCognexType y)
            {
                return (x == y);
            }

            public int GetHashCode(eYoonCognexType obj)
            {
                return obj.GetHashCode();
            }
        }

        public string RootDirectory { get; set; }

        private Dictionary<eYoonCognexType, ToolSection> m_pDicSection;
        private List<eYoonCognexType> m_pListKeyOrdered;

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
                    m_pListKeyOrdered = value ? new List<eYoonCognexType>(m_pDicSection.Keys) : null;
                }
            }
        }


        public IEqualityComparer<eYoonCognexType> Comparer { get { return m_pDicSection.Comparer; } }

        public ToolSection this[int nIndex]
        {
            get
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index ToolContainer using integer key: container was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                return m_pDicSection[m_pListKeyOrdered[nIndex]];
            }
            set
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index ToolContainer using integer key: container was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                var key = m_pListKeyOrdered[nIndex];
                m_pDicSection[key] = value;
            }
        }

        public ToolSection this[eYoonCognexType nKey]
        {
            get
            {
                ToolSection pSection;
                if (m_pDicSection.TryGetValue(nKey, out pSection))
                {
                    return pSection;
                }
                return new ToolSection(nKey);
            }
            set
            {
                if (IsOrdered && !m_pListKeyOrdered.Contains(nKey, Comparer))
                {
                    m_pListKeyOrdered.Add(nKey);
                }
                m_pDicSection[nKey] = value;
            }
        }


        public ToolContainer()
            : this(DefaultComparer)
        {
            //
        }

        public ToolContainer(IEqualityComparer<eYoonCognexType> pStringComparer)
        {
            this.m_pDicSection = new Dictionary<eYoonCognexType, ToolSection>(pStringComparer);
        }

        public ToolContainer(Dictionary<eYoonCognexType, ToolSection> pDic)
            : this(pDic, DefaultComparer)
        {
            //
        }

        public ToolContainer(Dictionary<eYoonCognexType, ToolSection> pDic, IEqualityComparer<eYoonCognexType> pStringComparer)
        {
            this.m_pDicSection = new Dictionary<eYoonCognexType, ToolSection>(pDic, pStringComparer);
        }

        public ToolContainer(ToolContainer pContainer)
            : this(pContainer, default)
        {
            //
        }

        public ToolContainer(ToolContainer pContainer, IEqualityComparer<eYoonCognexType> pStringComparer)
        {
            this.m_pDicSection = new Dictionary<eYoonCognexType, ToolSection>(pContainer.m_pDicSection, pStringComparer);
        }

        public void CopyFrom(IYoonContainer pContainer)
        {
            if (pContainer is ToolContainer pToolContainer)
            {
                Clear();
                foreach (eYoonCognexType nkey in pToolContainer.Keys)
                {
                    Add(nkey, pToolContainer[nkey]);
                }
            }
        }

        public IYoonContainer Clone()
        {
            return new ToolContainer(this, Comparer);
        }

        public void Clear()
        {
            if (m_pDicSection != null)
                m_pDicSection.Clear();
            if (IsOrdered)
            {
                m_pListKeyOrdered.Clear();
            }
        }

        public bool LoadValue(eYoonCognexType nKey)
        {
            if (RootDirectory == string.Empty) return false;

            bool bResult = true;
            if (!m_pDicSection.ContainsKey(nKey))
                Add(nKey, new ToolSection(nKey));
            else
            {
                m_pDicSection[nKey].ParantsType = nKey;
                foreach (string strTag in m_pDicSection[nKey].Keys)
                {
                    if (!m_pDicSection[nKey].LoadValue(strTag))
                        bResult = false;
                }
            }
            return bResult;
        }

        public bool SaveValue(eYoonCognexType nKey)
        {
            if (RootDirectory == string.Empty) return false;

            if (!m_pDicSection.ContainsKey(nKey))
                return false;

            bool bResult = true;
            m_pDicSection[nKey].ParantsType = nKey;
            foreach (string strTag in m_pDicSection[nKey].Keys)
            {
                if (!m_pDicSection[nKey].SaveValue(strTag))
                    bResult = false;
            }
            return bResult;
        }

        public int IndexOf(eYoonCognexType nKey)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonCognexType) on ToolContainer: container was not ordered.");
            }
            return IndexOf(nKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(eYoonCognexType nKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonCognexType, int) on ToolContainer: container was not ordered.");
            }
            return IndexOf(nKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(eYoonCognexType nKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonCognexType, int, int) on ToolContainer: container was not ordered.");
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
                if (Comparer.Equals(m_pListKeyOrdered[i], nKey))
                {
                    return i;
                }
            }
            return -1;
        }

        public int LastIndexOf(eYoonCognexType nKey)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonCognexType) on ToolContainer: container was not ordered.");
            }
            return LastIndexOf(nKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(eYoonCognexType nKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonCognexType, int) on ToolContainer: container was not ordered.");
            }
            return LastIndexOf(nKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(eYoonCognexType nKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonCognexType, int, int) on ToolContainer: container was not ordered.");
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
                if (Comparer.Equals(m_pListKeyOrdered[i], nKey))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int nIndex, eYoonCognexType nKey, ToolSection pValue)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Insert(int, eYoonCognexType, ToolSection) on ToolContainer: container was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            m_pDicSection.Add(nKey, pValue);
            m_pListKeyOrdered.Insert(nIndex, nKey);
        }

        public void InsertRange(int nIndex, IEnumerable<KeyValuePair<eYoonCognexType, ToolSection>> pCollection)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<eYoonCognexType, ToolSection>>) on ToolContainer: container was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveAt(int) on ToolContainer: container was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            var key = m_pListKeyOrdered[nIndex];
            m_pListKeyOrdered.RemoveAt(nIndex);
            m_pDicSection.Remove(key);
        }

        public void RemoveRange(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on ToolContainer: container was not ordered.");
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
                throw new InvalidOperationException("Cannot call Reverse() on ToolContainer: container was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on ToolContainer: container was not ordered.");
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

        public ICollection<ToolSection> GetOrderedValues()
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call GetOrderedValues() on ToolContainer: container was not ordered.");
            }
            var list = new List<ToolSection>();
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                list.Add(m_pDicSection[m_pListKeyOrdered[i]]);
            }
            return list;
        }

        public void Add(eYoonCognexType nkey, ToolSection pValue)
        {
            m_pDicSection.Add(nkey, pValue);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(nkey);
            }
        }

        public bool ContainsKey(eYoonCognexType nKey)
        {
            return m_pDicSection.ContainsKey(nKey);
        }

        public ICollection<eYoonCognexType> Keys
        {
            get { return IsOrdered ? (ICollection<eYoonCognexType>)m_pListKeyOrdered : m_pDicSection.Keys; }
        }

        public ICollection<ToolSection> Values
        {
            get
            {
                return m_pDicSection.Values;
            }
        }

        public bool Remove(eYoonCognexType nKey)
        {
            var ret = m_pDicSection.Remove(nKey);
            if (IsOrdered && ret)
            {
                for (int i = 0; i < m_pListKeyOrdered.Count; i++)
                {
                    if (Comparer.Equals(m_pListKeyOrdered[i], nKey))
                    {
                        m_pListKeyOrdered.RemoveAt(i);
                        break;
                    }
                }
            }
            return ret;
        }

        public bool TryGetValue(eYoonCognexType nKey, out ToolSection pValue)
        {
            return m_pDicSection.TryGetValue(nKey, out pValue);
        }

        public int Count
        {
            get { return m_pDicSection.Count; }
        }

        void ICollection<KeyValuePair<eYoonCognexType, ToolSection>>.Add(KeyValuePair<eYoonCognexType, ToolSection> pCollection)
        {
            ((IDictionary<eYoonCognexType, ToolSection>)m_pDicSection).Add(pCollection);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(pCollection.Key);
            }
        }

        bool ICollection<KeyValuePair<eYoonCognexType, ToolSection>>.Contains(KeyValuePair<eYoonCognexType, ToolSection> pCollection)
        {
            return ((IDictionary<eYoonCognexType, ToolSection>)m_pDicSection).Contains(pCollection);
        }

        void ICollection<KeyValuePair<eYoonCognexType, ToolSection>>.CopyTo(KeyValuePair<eYoonCognexType, ToolSection>[] pArray, int nIndexArray)
        {
            ((IDictionary<eYoonCognexType, ToolSection>)m_pDicSection).CopyTo(pArray, nIndexArray);
        }

        bool ICollection<KeyValuePair<eYoonCognexType, ToolSection>>.IsReadOnly
        {
            get { return ((IDictionary<eYoonCognexType, ToolSection>)m_pDicSection).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<eYoonCognexType, ToolSection>>.Remove(KeyValuePair<eYoonCognexType, ToolSection> pCollection)
        {
            var ret = ((IDictionary<eYoonCognexType, ToolSection>)m_pDicSection).Remove(pCollection);
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

        public IEnumerator<KeyValuePair<eYoonCognexType, ToolSection>> GetEnumerator()
        {
            if (IsOrdered)
            {
                return GetOrderedEnumerator();
            }
            else
            {
                return m_pDicSection.GetEnumerator();
            }
        }

        private IEnumerator<KeyValuePair<eYoonCognexType, ToolSection>> GetOrderedEnumerator()
        {
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                yield return new KeyValuePair<eYoonCognexType, ToolSection>(m_pListKeyOrdered[i], m_pDicSection[m_pListKeyOrdered[i]]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator ToolContainer(Dictionary<eYoonCognexType, ToolSection> pDic)
        {
            return new ToolContainer(pDic);
        }

        public static explicit operator Dictionary<eYoonCognexType, ToolSection>(ToolContainer pContainer)
        {
            return pContainer.m_pDicSection;
        }
    }

    public class ResultContainer : IYoonContainer, IYoonContainer<eYoonCognexType, ResultSection>
    {
        #region Supported IDisposable Pattern
        ~ResultContainer()
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
            m_pDicSection = null;
            m_pListKeyOrdered = null;
            this.disposed = true;
        }
        #endregion

        public static IEqualityComparer<eYoonCognexType> DefaultComparer;

        class CaseInsensitiveTypeComparer : IEqualityComparer<eYoonCognexType>
        {
            public bool Equals(eYoonCognexType x, eYoonCognexType y)
            {
                return (x == y);
            }

            public int GetHashCode(eYoonCognexType obj)
            {
                return obj.GetHashCode();
            }
        }

        public string RootDirectory { get; set; }

        private Dictionary<eYoonCognexType, ResultSection> m_pDicSection;
        private List<eYoonCognexType> m_pListKeyOrdered;

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
                    m_pListKeyOrdered = value ? new List<eYoonCognexType>(m_pDicSection.Keys) : null;
                }
            }
        }

        public IEqualityComparer<eYoonCognexType> Comparer { get { return m_pDicSection.Comparer; } }

        public ResultSection this[int nIndex]
        {
            get
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index ResultContainer using integer key: container was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                return m_pDicSection[m_pListKeyOrdered[nIndex]];
            }
            set
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index ResultContainer using integer key: container was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                var key = m_pListKeyOrdered[nIndex];
                m_pDicSection[key] = value;
            }
        }

        public ResultSection this[eYoonCognexType nKey]
        {
            get
            {
                ResultSection pSection;
                if (m_pDicSection.TryGetValue(nKey, out pSection))
                {
                    return pSection;
                }
                return new ResultSection(nKey);
            }
            set
            {
                if (IsOrdered && !m_pListKeyOrdered.Contains(nKey, Comparer))
                {
                    m_pListKeyOrdered.Add(nKey);
                }
                m_pDicSection[nKey] = value;
            }
        }


        public ResultContainer()
            : this(DefaultComparer)
        {
            //
        }

        public ResultContainer(IEqualityComparer<eYoonCognexType> pStringComparer)
        {
            this.m_pDicSection = new Dictionary<eYoonCognexType, ResultSection>(pStringComparer);
        }

        public ResultContainer(Dictionary<eYoonCognexType, ResultSection> pDic)
            : this(pDic, DefaultComparer)
        {
            //
        }

        public ResultContainer(Dictionary<eYoonCognexType, ResultSection> pDic, IEqualityComparer<eYoonCognexType> pStringComparer)
        {
            this.m_pDicSection = new Dictionary<eYoonCognexType, ResultSection>(pDic, pStringComparer);
        }

        public ResultContainer(ResultContainer pContainer)
            : this(pContainer, default)
        {
            //
        }

        public ResultContainer(ResultContainer pContainer, IEqualityComparer<eYoonCognexType> pStringComparer)
        {
            this.m_pDicSection = new Dictionary<eYoonCognexType, ResultSection>(pContainer.m_pDicSection, pStringComparer);
        }

        public void CopyFrom(IYoonContainer pContainer)
        {
            if (pContainer is ResultContainer pResultContainer)
            {
                Clear();
                foreach (eYoonCognexType nkey in pResultContainer.Keys)
                {
                    Add(nkey, pResultContainer[nkey]);
                }
            }
        }

        public IYoonContainer Clone()
        {
            return new ResultContainer(this, Comparer);
        }

        public void Clear()
        {
            if (m_pDicSection != null)
                m_pDicSection.Clear();
            if (IsOrdered)
            {
                m_pListKeyOrdered.Clear();
            }
        }

        public bool LoadValue(eYoonCognexType nKey)
        {
            if (RootDirectory == string.Empty) return false;

            bool bResult = true;
            if (!m_pDicSection.ContainsKey(nKey))
                Add(nKey, new ResultSection(nKey));
            else
            {
                m_pDicSection[nKey].ParantsType = nKey;
                foreach (string strTag in m_pDicSection[nKey].Keys)
                {
                    if (!m_pDicSection[nKey].LoadValue(strTag))
                        bResult = false;
                }
            }
            return bResult;
        }

        public bool SaveValue(eYoonCognexType nKey)
        {
            if (RootDirectory == string.Empty) return false;

            if (!m_pDicSection.ContainsKey(nKey))
                return false;

            bool bResult = true;
            m_pDicSection[nKey].ParantsType = nKey;
            foreach (string strTag in m_pDicSection[nKey].Keys)
            {
                if (!m_pDicSection[nKey].SaveValue(strTag))
                    bResult = false;
            }
            return bResult;
        }

        public int IndexOf(eYoonCognexType nKey)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonCognexType) on ResultContainer: container was not ordered.");
            }
            return IndexOf(nKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(eYoonCognexType nKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonCognexType, int) on ResultContainer: container was not ordered.");
            }
            return IndexOf(nKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(eYoonCognexType nKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonCognexType, int, int) on ResultContainer: container was not ordered.");
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
                if (Comparer.Equals(m_pListKeyOrdered[i], nKey))
                {
                    return i;
                }
            }
            return -1;
        }

        public int LastIndexOf(eYoonCognexType nKey)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonCognexType) on ResultContainer: container was not ordered.");
            }
            return LastIndexOf(nKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(eYoonCognexType nKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonCognexType, int) on ResultContainer: container was not ordered.");
            }
            return LastIndexOf(nKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(eYoonCognexType nKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonCognexType, int, int) on ResultContainer: container was not ordered.");
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
                if (Comparer.Equals(m_pListKeyOrdered[i], nKey))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int nIndex, eYoonCognexType nKey, ResultSection pValue)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Insert(int, eYoonCognexType, ResultSection) on ResultContainer: container was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            m_pDicSection.Add(nKey, pValue);
            m_pListKeyOrdered.Insert(nIndex, nKey);
        }

        public void InsertRange(int nIndex, IEnumerable<KeyValuePair<eYoonCognexType, ResultSection>> pCollection)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<eYoonCognexType, ResultSection>>) on ResultContainer: container was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveAt(int) on ResultContainer: container was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            var key = m_pListKeyOrdered[nIndex];
            m_pListKeyOrdered.RemoveAt(nIndex);
            m_pDicSection.Remove(key);
        }

        public void RemoveRange(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on ResultContainer: container was not ordered.");
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
                throw new InvalidOperationException("Cannot call Reverse() on ResultContainer: container was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on ResultContainer: container was not ordered.");
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

        public ICollection<ResultSection> GetOrderedValues()
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call GetOrderedValues() on ResultContainer: container was not ordered.");
            }
            var list = new List<ResultSection>();
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                list.Add(m_pDicSection[m_pListKeyOrdered[i]]);
            }
            return list;
        }

        public void Add(eYoonCognexType nkey, ResultSection pValue)
        {
            m_pDicSection.Add(nkey, pValue);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(nkey);
            }
        }

        public bool ContainsKey(eYoonCognexType nKey)
        {
            return m_pDicSection.ContainsKey(nKey);
        }

        public ICollection<eYoonCognexType> Keys
        {
            get { return IsOrdered ? (ICollection<eYoonCognexType>)m_pListKeyOrdered : m_pDicSection.Keys; }
        }

        public ICollection<ResultSection> Values
        {
            get
            {
                return m_pDicSection.Values;
            }
        }

        public bool Remove(eYoonCognexType nKey)
        {
            var ret = m_pDicSection.Remove(nKey);
            if (IsOrdered && ret)
            {
                for (int i = 0; i < m_pListKeyOrdered.Count; i++)
                {
                    if (Comparer.Equals(m_pListKeyOrdered[i], nKey))
                    {
                        m_pListKeyOrdered.RemoveAt(i);
                        break;
                    }
                }
            }
            return ret;
        }

        public bool TryGetValue(eYoonCognexType nKey, out ResultSection pValue)
        {
            return m_pDicSection.TryGetValue(nKey, out pValue);
        }

        public int Count
        {
            get { return m_pDicSection.Count; }
        }

        void ICollection<KeyValuePair<eYoonCognexType, ResultSection>>.Add(KeyValuePair<eYoonCognexType, ResultSection> pCollection)
        {
            ((IDictionary<eYoonCognexType, ResultSection>)m_pDicSection).Add(pCollection);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(pCollection.Key);
            }
        }

        bool ICollection<KeyValuePair<eYoonCognexType, ResultSection>>.Contains(KeyValuePair<eYoonCognexType, ResultSection> pCollection)
        {
            return ((IDictionary<eYoonCognexType, ResultSection>)m_pDicSection).Contains(pCollection);
        }

        void ICollection<KeyValuePair<eYoonCognexType, ResultSection>>.CopyTo(KeyValuePair<eYoonCognexType, ResultSection>[] pArray, int nIndexArray)
        {
            ((IDictionary<eYoonCognexType, ResultSection>)m_pDicSection).CopyTo(pArray, nIndexArray);
        }

        bool ICollection<KeyValuePair<eYoonCognexType, ResultSection>>.IsReadOnly
        {
            get { return ((IDictionary<eYoonCognexType, ResultSection>)m_pDicSection).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<eYoonCognexType, ResultSection>>.Remove(KeyValuePair<eYoonCognexType, ResultSection> pCollection)
        {
            var ret = ((IDictionary<eYoonCognexType, ResultSection>)m_pDicSection).Remove(pCollection);
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

        public IEnumerator<KeyValuePair<eYoonCognexType, ResultSection>> GetEnumerator()
        {
            if (IsOrdered)
            {
                return GetOrderedEnumerator();
            }
            else
            {
                return m_pDicSection.GetEnumerator();
            }
        }

        private IEnumerator<KeyValuePair<eYoonCognexType, ResultSection>> GetOrderedEnumerator()
        {
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                yield return new KeyValuePair<eYoonCognexType, ResultSection>(m_pListKeyOrdered[i], m_pDicSection[m_pListKeyOrdered[i]]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator ResultContainer(Dictionary<eYoonCognexType, ResultSection> pDic)
        {
            return new ResultContainer(pDic);
        }

        public static explicit operator Dictionary<eYoonCognexType, ResultSection>(ResultContainer pContainer)
        {
            return pContainer.m_pDicSection;
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
