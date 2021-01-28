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
    public class ToolSection : IYoonSection<string, ICogTool>
    {
        #region Supported IDisposable Pattern
        ~ToolSection()
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
            m_pDicCogTool = null;
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

        private Dictionary<string, ICogTool> m_pDicCogTool;
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
                    m_pListKeyOrdered = value ? new List<string>(m_pDicCogTool.Keys) : null;
                }
            }
        }

        public IEqualityComparer<string> Comparer { get { return m_pDicCogTool.Comparer; } }

        public ICogTool this[int nIndex]
        {
            get
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index ToolSection using integer key: section was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                return m_pDicCogTool[m_pListKeyOrdered[nIndex]];
            }
            set
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index ToolSection using integer key: section was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                var key = m_pListKeyOrdered[nIndex];
                m_pDicCogTool[key] = value;
            }
        }

        public ICogTool this[string strKey]
        {
            get
            {
                ICogTool pTool;
                if (m_pDicCogTool.TryGetValue(strKey, out pTool))
                {
                    return pTool;
                }
                return ToolFactory.InitCognexTool(ParantsType);
            }
            set
            {
                if (IsOrdered && !m_pListKeyOrdered.Contains(strKey, Comparer))
                {
                    m_pListKeyOrdered.Add(strKey);
                }
                m_pDicCogTool[strKey] = value;
            }
        }


        public ToolSection()
            : this(DefaultComparer)
        {
            //
        }

        public ToolSection(IEqualityComparer<string> pStringComparer)
        {
            this.m_pDicCogTool = new Dictionary<string, ICogTool>(pStringComparer);
        }

        public ToolSection(eYoonCognexType nType)
            : this(nType, DefaultComparer)
        {
            //
        }

        public ToolSection(eYoonCognexType nType, IEqualityComparer<string> pStringComparer)
        {
            this.ParantsType = nType;
            this.m_pDicCogTool = new Dictionary<string, ICogTool>(pStringComparer);
        }

        public ToolSection(Dictionary<string, ICogTool> pDic)
            : this(pDic, DefaultComparer)
        {
            //
        }

        public ToolSection(Dictionary<string, ICogTool> pDic, IEqualityComparer<string> pStringComparer)
        {
            this.m_pDicCogTool = new Dictionary<string, ICogTool>(pDic, pStringComparer);
        }

        public ToolSection(ToolSection pSection)
            : this(pSection, DefaultComparer)
        {
            //
        }

        public ToolSection(ToolSection pSection, IEqualityComparer<string> pStringComparer)
        {
            this.m_pDicCogTool = new Dictionary<string, ICogTool>(pSection.m_pDicCogTool, pStringComparer);
        }

        public void Clear()
        {
            if (m_pDicCogTool != null)
                m_pDicCogTool.Clear();
            if (IsOrdered)
            {
                m_pListKeyOrdered.Clear();
            }
        }

        private string GetCognexToolFilePath(string strTag)
        {
            switch (ParantsType)
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

        public bool LoadValue(string strKey)
        {
            if (RootDirectory == string.Empty || strKey == string.Empty)
                return false;

            string strFilePath = GetCognexToolFilePath(strKey);
            ICogTool pCogTool = ToolFactory.LoadCognexToolFromVpp(strFilePath);
            if (pCogTool == null) return false;

            if (!m_pDicCogTool.ContainsKey(strKey))
                Add(strKey, pCogTool);
            else
                m_pDicCogTool[strKey] = pCogTool;
            return true;
        }

        public bool SaveValue(string strKey)
        {
            if (RootDirectory == string.Empty || strKey == string.Empty)
                return false;

            if (!m_pDicCogTool.ContainsKey(strKey)) return false;
            string strFilePath = GetCognexToolFilePath(strKey);
            return ToolFactory.SaveCognexToolToVpp(m_pDicCogTool[strKey], strFilePath);
        }

        public int IndexOf(string strKey)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string) on ToolSection: section was not ordered.");
            }
            return IndexOf(strKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(string strKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string, int) on ToolSection: section was not ordered.");
            }
            return IndexOf(strKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(string strKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string, int, int) on ToolSection: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call LastIndexOf(string) on ToolSection: section was not ordered.");
            }
            return LastIndexOf(strKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(string strKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(string, int) on ToolSection: section was not ordered.");
            }
            return LastIndexOf(strKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(string strKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(string, int, int) on ToolSection: section was not ordered.");
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

        public void Insert(int nIndex, string strKey, ICogTool pValue)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Insert(int, string, ICogTool) on ToolSection: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            m_pDicCogTool.Add(strKey, pValue);
            m_pListKeyOrdered.Insert(nIndex, strKey);
        }

        public void InsertRange(int nIndex, IEnumerable<KeyValuePair<string, ICogTool>> pCollection)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<string, YoonParameter>>) on ToolSection: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveAt(int) on ToolSection: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            var key = m_pListKeyOrdered[nIndex];
            m_pListKeyOrdered.RemoveAt(nIndex);
            m_pDicCogTool.Remove(key);
        }

        public void RemoveRange(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on ToolSection: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call Reverse() on ToolSection: section was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on ToolSection: section was not ordered.");
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

        public ICollection<ICogTool> GetOrderedValues()
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call GetOrderedValues() on ToolSection: section was not ordered.");
            }
            var list = new List<ICogTool>();
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                list.Add(m_pDicCogTool[m_pListKeyOrdered[i]]);
            }
            return list;
        }

        public void Add(string strKey, ICogTool pValue)
        {
            m_pDicCogTool.Add(strKey, pValue);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(strKey);
            }
        }

        public bool ContainsKey(string strKey)
        {
            return m_pDicCogTool.ContainsKey(strKey);
        }

        public ICollection<string> Keys
        {
            get { return IsOrdered ? (ICollection<string>)m_pListKeyOrdered : m_pDicCogTool.Keys; }
        }

        public ICollection<ICogTool> Values
        {
            get
            {
                return m_pDicCogTool.Values;
            }
        }

        public bool Remove(string strKey)
        {
            var ret = m_pDicCogTool.Remove(strKey);
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

        public bool TryGetValue(string strKey, out ICogTool pValue)
        {
            return m_pDicCogTool.TryGetValue(strKey, out pValue);
        }

        public int Count
        {
            get { return m_pDicCogTool.Count; }
        }

        void ICollection<KeyValuePair<string, ICogTool>>.Add(KeyValuePair<string, ICogTool> pCollection)
        {
            ((IDictionary<string, ICogTool>)m_pDicCogTool).Add(pCollection);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(pCollection.Key);
            }
        }

        bool ICollection<KeyValuePair<string, ICogTool>>.Contains(KeyValuePair<string, ICogTool> pCollection)
        {
            return ((IDictionary<string, ICogTool>)m_pDicCogTool).Contains(pCollection);
        }

        void ICollection<KeyValuePair<string, ICogTool>>.CopyTo(KeyValuePair<string, ICogTool>[] pArray, int nIndexArray)
        {
            ((IDictionary<string, ICogTool>)m_pDicCogTool).CopyTo(pArray, nIndexArray);
        }

        bool ICollection<KeyValuePair<string, ICogTool>>.IsReadOnly
        {
            get { return ((IDictionary<string, ICogTool>)m_pDicCogTool).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<string, ICogTool>>.Remove(KeyValuePair<string, ICogTool> pCollection)
        {
            var ret = ((IDictionary<string, ICogTool>)m_pDicCogTool).Remove(pCollection);
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

        public IEnumerator<KeyValuePair<string, ICogTool>> GetEnumerator()
        {
            if (IsOrdered)
            {
                return GetOrderedEnumerator();
            }
            else
            {
                return m_pDicCogTool.GetEnumerator();
            }
        }

        private IEnumerator<KeyValuePair<string, ICogTool>> GetOrderedEnumerator()
        {
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                yield return new KeyValuePair<string, ICogTool>(m_pListKeyOrdered[i], m_pDicCogTool[m_pListKeyOrdered[i]]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator ToolSection(Dictionary<string, ICogTool> pDic)
        {
            return new ToolSection(pDic);
        }

        public static explicit operator Dictionary<string, ICogTool>(ToolSection pContainer)
        {
            return pContainer.m_pDicCogTool;
        }
    }
}
