using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using YoonFactory.Files;

namespace YoonFactory.Param
{
    public class Template<TKey, TValue> : IYoonTemplate, IYoonSection<TKey, TValue> where TKey : IConvertible where TValue : IYoonTemplate
    {
        #region IDisposable Support
        ~Template()
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
            Clear();
            m_pDicTemplate = null;
            m_pListKeyOrdered = null;
            this.disposed = true;
        }
        #endregion

        public static IEqualityComparer<TKey> DefaultComparer = new CaseInsensitiveComparer();

        class CaseInsensitiveComparer : IEqualityComparer<TKey>
        {
            public bool Equals(TKey x, TKey y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(TKey obj)
            {
                return obj.GetHashCode();
            }
        }

        public int No { get; set; }
        public string Name { get; set; }
        public string RootDirectory { get; set; }
        public bool IsMarkNumber { get; set; } = true;
        public override string ToString()
        {
            return (IsMarkNumber) ? Name : string.Format("{0:D2}_{1}", No, Name);
        }

        protected Dictionary<TKey, TValue> m_pDicTemplate;
        protected List<TKey> m_pListKeyOrdered;

        public IEqualityComparer<TKey> Comparer { get { return m_pDicTemplate.Comparer; } }

        public TValue this[int nIndex]
        {
            get
            {
                if (m_pListKeyOrdered == null)
                {
                    throw new InvalidOperationException("Cannot index ParameterContainer using integer key: section was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                return m_pDicTemplate[m_pListKeyOrdered[nIndex]];
            }
            set
            {
                if (m_pListKeyOrdered == null)
                {
                    throw new InvalidOperationException("Cannot index ParameterContainer using integer key: section was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                var key = m_pListKeyOrdered[nIndex];
                m_pDicTemplate[key] = value;
            }
        }

        public TValue this[TKey pKey]
        {
            get
            {
                TValue pParam;
                if (m_pDicTemplate.TryGetValue(pKey, out pParam))
                {
                    return pParam;
                }
                return default(TValue);
            }
            set
            {
                if (m_pListKeyOrdered != null && !m_pListKeyOrdered.Contains(pKey, Comparer))
                {
                    m_pListKeyOrdered.Add(pKey);
                }
                m_pDicTemplate[pKey] = value;
            }
        }

        public Template(string strName)
        {
            No = 0;
            Name = strName;
            IsMarkNumber = false;
            RootDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "YoonFactory");
            this.m_pDicTemplate = new Dictionary<TKey, TValue>(DefaultComparer);
            this.m_pListKeyOrdered = new List<TKey>();
        }

        public Template(int nNo, string strName)
        {
            No = nNo;
            Name = strName;
            IsMarkNumber = true;
            RootDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "YoonFactory");
            this.m_pDicTemplate = new Dictionary<TKey, TValue>(DefaultComparer);
            this.m_pListKeyOrdered = new List<TKey>();
        }

        public Template(Template<TKey, TValue> pTemplate)
        {
            No = pTemplate.No;
            Name = pTemplate.Name;
            IsMarkNumber = pTemplate.IsMarkNumber;
            RootDirectory = pTemplate.RootDirectory;
            this.m_pDicTemplate = new Dictionary<TKey, TValue>(pTemplate.m_pDicTemplate, DefaultComparer);
            this.m_pListKeyOrdered = new List<TKey>();
        }

        public void CopyFrom(IYoonTemplate pTemplate)
        {
            if (pTemplate is Template<TKey, TValue> pTemplateOrigin)
            {
                Clear();

                No = pTemplateOrigin.No;
                Name = pTemplateOrigin.Name;
                IsMarkNumber = pTemplateOrigin.IsMarkNumber;
                RootDirectory = pTemplateOrigin.RootDirectory;
                foreach (TKey pKey in pTemplateOrigin.Keys)
                {
                    Add(pKey, pTemplateOrigin[pKey]);
                }
            }
        }

        public IYoonTemplate Clone()
        {
            return new Template<TKey, TValue>(this);
        }

        public void Clear()
        {
            if (m_pDicTemplate != null)
                m_pDicTemplate.Clear();
            if (m_pListKeyOrdered != null)
            {
                m_pListKeyOrdered.Clear();
            }
        }

        public bool LoadTemplate()
        {
            if (RootDirectory == string.Empty || m_pDicTemplate == null)
            {
                throw new InvalidOperationException("LoadTemplate() on Template<TKey, TValue> : Template Info isnot contained");
            }

            bool bResult = true;
            string strIniFilePath = Path.Combine(RootDirectory, string.Format(@"{0}Template.ini", ToString()));
            using (YoonIni pIni = new YoonIni(strIniFilePath))
            {
                pIni.LoadFile();
                if (No != pIni["HEAD"]["No"].ToInt(No))
                {
                    throw new FileLoadException("LoadTemplate() on Template<TKey, TValue> : Template No isnot matching");
                }
                if (Name != pIni["HEAD"]["Name"].ToString(Name))
                {
                    throw new FileLoadException("LoadTemplate() on Template<TKey, TValue> : Template Name isnot Matching");
                }
                int nCount = pIni["HEAD"]["Count"].ToInt(0);
                for (int iParam = 0; iParam < nCount; iParam++)
                {
                    TKey pKey = pIni["KEY"][iParam.ToString()].To(default(TKey));
                    if (!m_pDicTemplate.ContainsKey(pKey))
                        Add(pKey, default(TValue));
                    m_pDicTemplate[pKey].No = iParam;
                    m_pDicTemplate[pKey].Name = pKey.ToString();
                    m_pDicTemplate[pKey].RootDirectory = Path.Combine(RootDirectory, ToString());
                    if (!m_pDicTemplate[pKey].LoadTemplate())
                        bResult = false;
                }
            }
            return bResult;
        }

        public bool SaveTemplate()
        {
            if (RootDirectory == string.Empty || m_pDicTemplate == null)
            {
                throw new InvalidOperationException("SaveTemplate() on Template<TKey, TValue> : Template Info isnot contained");
            }

            bool bResult = true;
            string strIniFilePath = Path.Combine(RootDirectory, string.Format(@"{0}Template.ini", ToString()));
            using (YoonIni pIni = new YoonIni(strIniFilePath))
            {
                int iParam = 0;
                pIni["HEAD"]["No"] = No;
                pIni["HEAD"]["Name"] = Name;
                pIni["HEAD"]["Count"] = Count;
                foreach (TKey pKey in m_pDicTemplate.Keys)
                {
                    pIni["KEY"][iParam.ToString()] = pKey.ToString();
                    m_pDicTemplate[pKey].No = iParam;
                    m_pDicTemplate[pKey].Name = pKey.ToString();
                    m_pDicTemplate[pKey].RootDirectory = Path.Combine(RootDirectory, ToString());
                    if (!m_pDicTemplate[pKey].SaveTemplate())
                        bResult = false;
                    iParam++;
                }
                pIni.SaveFile();
            }
            return bResult;
        }

        public TKey KeyOf(int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call KeyOf(int) on Template: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            return m_pListKeyOrdered[nIndex];
        }

        public int IndexOf(TKey pKey)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(TKey) on Template: section was not ordered.");
            }
            return IndexOf(pKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(TKey pKey, int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(TKey, int) on Template: section was not ordered.");
            }
            return IndexOf(pKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(TKey pKey, int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(TKey, int, int) on Template: section was not ordered.");
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
                if (Comparer.Equals(m_pListKeyOrdered[i], pKey))
                {
                    return i;
                }
            }
            return -1;
        }

        public int LastIndexOf(TKey pKey)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(TKey) on Template: section was not ordered.");
            }
            return LastIndexOf(pKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(TKey pKey, int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(T, int) on Template: section was not ordered.");
            }
            return LastIndexOf(pKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(TKey pKey, int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(T, int, int) on Template: section was not ordered.");
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
                if (Comparer.Equals(m_pListKeyOrdered[i], pKey))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int nIndex, TKey pKey, TValue pValue)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Insert(int, TKey, TValue) on Template: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            m_pDicTemplate.Add(pKey, pValue);
            m_pListKeyOrdered.Insert(nIndex, pKey);
        }

        public void InsertRange(int nIndex, IEnumerable<KeyValuePair<TKey, TValue>> pCollection)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<TTKey, TValue>>) on Template: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveAt(int) on Template: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            var key = m_pListKeyOrdered[nIndex];
            m_pListKeyOrdered.RemoveAt(nIndex);
            m_pDicTemplate.Remove(key);
        }

        public void RemoveRange(int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on Template: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call Reverse() on Template: section was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on Template: section was not ordered.");
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

        public ICollection<TValue> GetOrderedValues()
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call GetOrderedValues() on Template: section was not ordered.");
            }
            var list = new List<TValue>();
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                list.Add(m_pDicTemplate[m_pListKeyOrdered[i]]);
            }
            return list;
        }

        public void Add(TKey pKey, TValue pValue)
        {
            m_pDicTemplate.Add(pKey, pValue);
            if (m_pListKeyOrdered != null)
            {
                m_pListKeyOrdered.Add(pKey);
            }
        }

        public bool ContainsKey(TKey pKey)
        {
            return m_pDicTemplate.ContainsKey(pKey);
        }

        public ICollection<TKey> Keys
        {
            get { return (m_pListKeyOrdered != null) ? (ICollection<TKey>)m_pListKeyOrdered : m_pDicTemplate.Keys; }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return m_pDicTemplate.Values;
            }
        }

        public bool Remove(TKey pKey)
        {
            var ret = m_pDicTemplate.Remove(pKey);
            if (m_pListKeyOrdered != null && ret)
            {
                for (int i = 0; i < m_pListKeyOrdered.Count; i++)
                {
                    if (Comparer.Equals(m_pListKeyOrdered[i], pKey))
                    {
                        m_pListKeyOrdered.RemoveAt(i);
                        break;
                    }
                }
            }
            return ret;
        }

        public bool TryGetValue(TKey pKey, out TValue pValue)
        {
            return m_pDicTemplate.TryGetValue(pKey, out pValue);
        }

        public int Count
        {
            get { return m_pDicTemplate.Count; }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pCollection)
        {
            ((IDictionary<TKey, TValue>)m_pDicTemplate).Add(pCollection);
            if (m_pListKeyOrdered != null)
            {
                m_pListKeyOrdered.Add(pCollection.Key);
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pCollection)
        {
            return ((IDictionary<TKey, TValue>)m_pDicTemplate).Contains(pCollection);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] pArray, int nIndexArray)
        {
            ((IDictionary<TKey, TValue>)m_pDicTemplate).CopyTo(pArray, nIndexArray);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return ((IDictionary<TKey, TValue>)m_pDicTemplate).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> pCollection)
        {
            var ret = ((IDictionary<TKey, TValue>)m_pDicTemplate).Remove(pCollection);
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

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (m_pListKeyOrdered != null)
            {
                return GetOrderedEnumerator();
            }
            else
            {
                return m_pDicTemplate.GetEnumerator();
            }
        }

        private IEnumerator<KeyValuePair<TKey, TValue>> GetOrderedEnumerator()
        {
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                yield return new KeyValuePair<TKey, TValue>(m_pListKeyOrdered[i], m_pDicTemplate[m_pListKeyOrdered[i]]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator Template<TKey, TValue>(Dictionary<TKey, TValue> pDic)
        {
            return new Template<TKey, TValue>(pDic);
        }

        public static explicit operator Dictionary<TKey, TValue>(Template<TKey, TValue> pContainer)
        {
            return pContainer.m_pDicTemplate;
        }
    }
}
