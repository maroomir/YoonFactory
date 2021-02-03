using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Cognex.VisionPro;

namespace YoonFactory.Cognex
{
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

        public IEqualityComparer<string> Comparer { get { return m_pDicCogResult.Comparer; } }

        public CognexResult this[int nIndex]
        {
            get
            {
                if (m_pListKeyOrdered == null)
                {
                    throw new InvalidOperationException("Cannot index ResultSection using integer key: section was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                return m_pDicCogResult[m_pListKeyOrdered[nIndex]];
            }
            set
            {
                if (m_pListKeyOrdered == null)
                {
                    throw new InvalidOperationException("Cannot index ResultSection using integer key: section was not ordered.");
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
                if (m_pListKeyOrdered != null && !m_pListKeyOrdered.Contains(strKey, Comparer))
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
            if (m_pListKeyOrdered != null)
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

        public ICogImage GetResultImage(string strKey)
        {
            if (m_pDicCogResult.ContainsKey(strKey))
                return new CogImage8Grey();
            return m_pDicCogResult[strKey].ResultImage;
        }

        public ICogImage GetLastResultImage()
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string) on ResultSection: section was not ordered.");
            }
            int nIndex = m_pListKeyOrdered.Count - 1;
            return m_pDicCogResult[m_pListKeyOrdered[nIndex]].ResultImage;
        }

        public string KeyOf(int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call KeyOf(int) on ResultSection: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            return m_pListKeyOrdered[nIndex];
        }

        public int IndexOf(string strKey)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string) on ResultSection: section was not ordered.");
            }
            return IndexOf(strKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(string strKey, int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string, int) on ResultSection: section was not ordered.");
            }
            return IndexOf(strKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(string strKey, int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string, int, int) on ResultSection: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(string) on ResultSection: section was not ordered.");
            }
            return LastIndexOf(strKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(string strKey, int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(string, int) on ResultSection: section was not ordered.");
            }
            return LastIndexOf(strKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(string strKey, int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(string, int, int) on ResultSection: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Insert(int, string, CognexResult) on ResultSection: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<string, CognexResult>>) on ResultSection: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call RemoveAt(int) on ResultSection: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on ResultSection: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Reverse() on ResultSection: section was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on ResultSection: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call GetOrderedValues() on ResultSection: section was not ordered.");
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
            if (m_pListKeyOrdered != null)
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
            get { return (m_pListKeyOrdered != null) ? (ICollection<string>)m_pListKeyOrdered : m_pDicCogResult.Keys; }
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
            if (m_pListKeyOrdered != null && ret)
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
            if (m_pListKeyOrdered != null)
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
            if (m_pListKeyOrdered != null && ret)
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
            if (m_pListKeyOrdered != null)
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
