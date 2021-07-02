using System;
using System.Collections.Generic;
using System.Linq;
using Cognex.VisionPro;

namespace YoonFactory.Cognex.Result
{
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

        public static IEqualityComparer<eYoonCognexType> DefaultComparer = new CaseInsensitiveTypeComparer();

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

        public string FilesDirectory { get; set; }

        protected Dictionary<eYoonCognexType, ResultSection> m_pDicSection;
        protected List<eYoonCognexType> m_pListKeyOrdered;

        public IEqualityComparer<eYoonCognexType> Comparer { get { return m_pDicSection.Comparer; } }

        public ResultSection this[int nIndex]
        {
            get
            {
                if (m_pListKeyOrdered == null)
                {
                    throw new InvalidOperationException("Cannot index ResultContainer using integer key: section was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                return m_pDicSection[m_pListKeyOrdered[nIndex]];
            }
            set
            {
                if (m_pListKeyOrdered == null)
                {
                    throw new InvalidOperationException("Cannot index ResultContainer using integer key: section was not ordered.");
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
                if (m_pListKeyOrdered != null && !m_pListKeyOrdered.Contains(nKey, Comparer))
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
            m_pDicSection = new Dictionary<eYoonCognexType, ResultSection>(pStringComparer);
            m_pListKeyOrdered = new List<eYoonCognexType>();
        }

        public ResultContainer(Dictionary<eYoonCognexType, ResultSection> pDic)
            : this(pDic, DefaultComparer)
        {
            //
        }

        public ResultContainer(Dictionary<eYoonCognexType, ResultSection> pDic, IEqualityComparer<eYoonCognexType> pStringComparer)
        {
            m_pDicSection = new Dictionary<eYoonCognexType, ResultSection>(pDic, pStringComparer);
            m_pListKeyOrdered = new List<eYoonCognexType>(pDic.Keys);
        }

        public ResultContainer(ResultContainer pContainer)
            : this(pContainer, DefaultComparer)
        {
            //
        }

        public ResultContainer(ResultContainer pContainer, IEqualityComparer<eYoonCognexType> pStringComparer)
        {
            m_pDicSection = new Dictionary<eYoonCognexType, ResultSection>(pContainer.m_pDicSection, pStringComparer);
            m_pListKeyOrdered = new List<eYoonCognexType>(pContainer.m_pListKeyOrdered);
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

        IYoonContainer IYoonContainer.Clone()
        {
            return new ResultContainer(this, Comparer);
        }

        public IYoonContainer<eYoonCognexType, ResultSection> Clone()
        {
            return new ResultContainer(this, Comparer);
        }

        public void Clear()
        {
            if (m_pDicSection != null)
                m_pDicSection.Clear();
            if (m_pListKeyOrdered != null)
            {
                m_pListKeyOrdered.Clear();
            }
        }

        public bool LoadValue(eYoonCognexType nKey)
        {
            if (FilesDirectory == string.Empty) return false;

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
            if (FilesDirectory == string.Empty) return false;

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

        public CognexImage GetResultImage(eYoonCognexType nKey)
        {
            if (m_pDicSection.ContainsKey(nKey))
                return new CognexImage();
            return m_pDicSection[nKey].GetLastResultImage();
        }

        public CognexImage GetLastResultImage()
        {
            if (m_pListKeyOrdered == null || m_pListKeyOrdered.Count == 0)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonCognexType) on ResultContainer: section was not ordered.");
            }
            int nIndex = m_pListKeyOrdered.Count - 1;
            return m_pDicSection[m_pListKeyOrdered[nIndex]].GetLastResultImage();
        }

        public eYoonCognexType KeyOf(int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call KeyOf(int) on ResultContainer: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            return m_pListKeyOrdered[nIndex];
        }

        public int IndexOf(eYoonCognexType nKey)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonCognexType) on ResultContainer: section was not ordered.");
            }
            return IndexOf(nKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(eYoonCognexType nKey, int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonCognexType, int) on ResultContainer: section was not ordered.");
            }
            return IndexOf(nKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(eYoonCognexType nKey, int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonCognexType, int, int) on ResultContainer: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonCognexType) on ResultContainer: section was not ordered.");
            }
            return LastIndexOf(nKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(eYoonCognexType nKey, int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonCognexType, int) on ResultContainer: section was not ordered.");
            }
            return LastIndexOf(nKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(eYoonCognexType nKey, int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonCognexType, int, int) on ResultContainer: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Insert(int, eYoonCognexType, ResultSection) on ResultContainer: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<eYoonCognexType, ResultSection>>) on ResultContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveAt(int) on ResultContainer: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on ResultContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call Reverse() on ResultContainer: section was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on ResultContainer: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call GetOrderedValues() on ResultContainer: section was not ordered.");
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
            if (m_pListKeyOrdered != null)
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
            get { return (m_pListKeyOrdered != null) ? (ICollection<eYoonCognexType>)m_pListKeyOrdered : m_pDicSection.Keys; }
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
            if (m_pListKeyOrdered != null && ret)
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
            if (m_pListKeyOrdered != null)
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

        public IEnumerator<KeyValuePair<eYoonCognexType, ResultSection>> GetEnumerator()
        {
            if (m_pListKeyOrdered != null)
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

}
