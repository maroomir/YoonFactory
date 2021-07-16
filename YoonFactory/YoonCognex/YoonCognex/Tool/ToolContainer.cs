using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory.Cognex.Tool
{
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

        protected Dictionary<eYoonCognexType, ToolSection> m_pDicSection;
        protected List<eYoonCognexType> m_pListKeyOrdered;

        public IEqualityComparer<eYoonCognexType> Comparer { get { return m_pDicSection.Comparer; } }

        public ToolSection this[int nIndex]
        {
            get
            {
                if (m_pListKeyOrdered == null)
                {
                    throw new InvalidOperationException("Cannot index ToolContainer using integer key: section was not ordered.");
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
                    throw new InvalidOperationException("Cannot index ToolContainer using integer key: section was not ordered.");
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
                if (m_pListKeyOrdered != null && !m_pListKeyOrdered.Contains(nKey, Comparer))
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
            m_pDicSection = new Dictionary<eYoonCognexType, ToolSection>(pStringComparer);
            m_pListKeyOrdered = new List<eYoonCognexType>();
        }

        public ToolContainer(Dictionary<eYoonCognexType, ToolSection> pDic)
            : this(pDic, DefaultComparer)
        {
            //
        }

        public ToolContainer(Dictionary<eYoonCognexType, ToolSection> pDic, IEqualityComparer<eYoonCognexType> pStringComparer)
        {
            m_pDicSection = new Dictionary<eYoonCognexType, ToolSection>(pDic, pStringComparer);
            m_pListKeyOrdered = new List<eYoonCognexType>(pDic.Keys);
        }

        public ToolContainer(ToolContainer pContainer)
            : this(pContainer, DefaultComparer)
        {
            //
        }

        public ToolContainer(ToolContainer pContainer, IEqualityComparer<eYoonCognexType> pStringComparer)
        {
            m_pDicSection = new Dictionary<eYoonCognexType, ToolSection>(pContainer.m_pDicSection, pStringComparer);
            m_pListKeyOrdered = new List<eYoonCognexType>(pContainer.m_pListKeyOrdered);
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

        IYoonContainer IYoonContainer.Clone()
        {
            return new ToolContainer(this, Comparer);
        }

        public IYoonContainer<eYoonCognexType, ToolSection> Clone()
        {
            return new ToolContainer(this, Comparer);
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

        public eYoonCognexType KeyOf(int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call KeyOf(int) on ToolContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call IndexOf(eYoonCognexType) on ToolContainer: section was not ordered.");
            }
            return IndexOf(nKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(eYoonCognexType nKey, int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonCognexType, int) on ToolContainer: section was not ordered.");
            }
            return IndexOf(nKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(eYoonCognexType nKey, int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonCognexType, int, int) on ToolContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonCognexType) on ToolContainer: section was not ordered.");
            }
            return LastIndexOf(nKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(eYoonCognexType nKey, int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonCognexType, int) on ToolContainer: section was not ordered.");
            }
            return LastIndexOf(nKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(eYoonCognexType nKey, int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonCognexType, int, int) on ToolContainer: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Insert(int, eYoonCognexType, ToolSection) on ToolContainer: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<eYoonCognexType, ToolSection>>) on ToolContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveAt(int) on ToolContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on ToolContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call Reverse() on ToolContainer: section was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on ToolContainer: section was not ordered.");
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
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call GetOrderedValues() on ToolContainer: section was not ordered.");
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
            if (m_pListKeyOrdered != null)
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

        public IEnumerator<KeyValuePair<eYoonCognexType, ToolSection>> GetEnumerator()
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

}
