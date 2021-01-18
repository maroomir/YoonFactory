using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory.Log
{
    public class LogRepository : IEnumerable<KeyValuePair<DateTime, string>>, IDictionary<DateTime, string>
    {
        public static IEqualityComparer<DateTime> DefaultComparer;

        class CaseInsensitiveDateTimeComparer : IEqualityComparer<DateTime>
        {
            public bool Equals(DateTime x, DateTime y)
            {
                return DateTime.Compare(x, y) == 0;
            }

            public int GetHashCode(DateTime obj)
            {
                return obj.GetHashCode();
            }
        }

        private Dictionary<DateTime, string> m_pDicLog;
        private List<DateTime> m_pListKeyOrdered;

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
                    m_pListKeyOrdered = value ? new List<DateTime>(m_pDicLog.Keys) : null;
                }
            }
        }

        public IEqualityComparer<DateTime> Comparer { get { return m_pDicLog.Comparer; } }

        public string this[int nIndex]
        {
            get
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index LogRepository using integer key: repository was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                return m_pDicLog[m_pListKeyOrdered[nIndex]];
            }
            set
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index LogRepository using integer key: repository was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                var key = m_pListKeyOrdered[nIndex];
                m_pDicLog[key] = value;
            }
        }

        public string this[DateTime pDateTime]
        {
            get
            {
                string str;
                if(m_pDicLog.TryGetValue(pDateTime, out str))
                {
                    return str;
                }
                return string.Empty;
            }
            set
            {
                if(IsOrdered && !m_pListKeyOrdered.Contains(pDateTime, Comparer))
                {
                    m_pListKeyOrdered.Add(pDateTime);
                }
                m_pDicLog[pDateTime] = value;
            }
        }

        public LogRepository()
            : this(DefaultComparer)
        {
            //
        }

        public LogRepository(IEqualityComparer<DateTime> pDateTimeComparer)
        {
            this.m_pDicLog = new Dictionary<DateTime, string>(pDateTimeComparer);
        }

        public LogRepository(Dictionary<DateTime, string> pDic)
            : this(pDic, DefaultComparer)
        {
            //
        }

        public LogRepository(Dictionary<DateTime, string> pDic, IEqualityComparer<DateTime> pDateTimeComparer)
        {
            this.m_pDicLog = new Dictionary<DateTime, string>(pDic, pDateTimeComparer);
        }

        public LogRepository(LogRepository pRepo)
            : this(pRepo, default)
        {
            //
        }

        public LogRepository(LogRepository pRepo, IEqualityComparer<DateTime> pDateTimeComparer)
        {
            this.m_pDicLog = new Dictionary<DateTime, string>(pRepo.m_pDicLog, pDateTimeComparer);
        }

        public int IndexOf(DateTime pKey) 
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(DateTime) on LogRepository: repository was not ordered.");
            }
            return IndexOf(pKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(DateTime pKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(DateTime, int) on LogRepository: repository was not ordered.");
            }
            return IndexOf(pKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(DateTime pKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(DateTime, int, int) on LogRepository: repository was not ordered.");
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

        public int LastIndexOf(DateTime pKey)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(DateTime) on LogRepository: repository was not ordered.");
            }
            return LastIndexOf(pKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(DateTime pKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(DateTime) on LogRepository: repository was not ordered.");
            }
            return LastIndexOf(pKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(DateTime pKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(DateTime, int, int) on LogRepository: repository was not ordered.");
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

        public void Insert(int nIndex, DateTime pKey, string strValue)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Insert(int, DateTime, string) on LogRepository: repository was not ordered.");
            }
            if(nIndex<0 || nIndex>m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            m_pDicLog.Add(pKey, strValue);
            m_pListKeyOrdered.Insert(nIndex, pKey);
        }

        public void InsertRange(int nIndex, IEnumerable<KeyValuePair<DateTime, string>> pCollection)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<DateTime, string>>) on LogRepository: repository was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveAt(int) on LogRepository: repository was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            var key = m_pListKeyOrdered[nIndex];
            m_pListKeyOrdered.RemoveAt(nIndex);
            m_pDicLog.Remove(key);
        }

        public void RemoveRange(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on LogRepository: repository was not ordered.");
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
                throw new InvalidOperationException("Cannot call Reverse() on LogRepository: repository was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on LogRepository: repository was not ordered.");
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

        public ICollection<string> GetOrderedValues()
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call GetOrderedValues() on IniSection: section was not ordered.");
            }
            var list = new List<string>();
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                list.Add(m_pDicLog[m_pListKeyOrdered[i]]);
            }
            return list;
        }

        public void Add(DateTime pKey, string strValue)
        {
            m_pDicLog.Add(pKey, strValue);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(pKey);
            }
        }

        public bool ContainsKey(DateTime pKey)
        {
            return m_pDicLog.ContainsKey(pKey);
        }


        /// <summary>
        /// Returns this IniSection's collection of keys. If the IniSection is ordered, the keys will be returned in order.
        /// </summary>
        public ICollection<DateTime> Keys
        {
            get { return IsOrdered ? (ICollection<DateTime>)m_pListKeyOrdered : m_pDicLog.Keys; }
        }

        public bool Remove(DateTime pKey)
        {
            var ret = m_pDicLog.Remove(pKey);
            if (IsOrdered && ret)
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

        public bool TryGetValue(DateTime pKey, out string strValue)
        {
            return m_pDicLog.TryGetValue(pKey, out strValue);
        }

        /// <summary>
        /// Returns the values in this IniSection. These values are always out of order. To get ordered values from an IniSection call GetOrderedValues instead.
        /// </summary>
        public ICollection<string> Values
        {
            get
            {
                return m_pDicLog.Values;
            }
        }

        void ICollection<KeyValuePair<DateTime, string>>.Add(KeyValuePair<DateTime, string> pCollection)
        {
            ((IDictionary<DateTime, string>)m_pDicLog).Add(pCollection);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(pCollection.Key);
            }
        }

        public void Clear()
        {
            m_pDicLog.Clear();
            if (IsOrdered)
            {
                m_pListKeyOrdered.Clear();
            }
        }

        bool ICollection<KeyValuePair<DateTime, string>>.Contains(KeyValuePair<DateTime, string> pCollection)
        {
            return ((IDictionary<DateTime, string>)m_pDicLog).Contains(pCollection);
        }

        void ICollection<KeyValuePair<DateTime, string>>.CopyTo(KeyValuePair<DateTime, string>[] pArray, int nIndexArray)
        {
            ((IDictionary<DateTime, string>)m_pDicLog).CopyTo(pArray, nIndexArray);
        }

        public int Count
        {
            get { return m_pDicLog.Count; }
        }

        bool ICollection<KeyValuePair<DateTime, string>>.IsReadOnly
        {
            get { return ((IDictionary<DateTime, string>)m_pDicLog).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<DateTime, string>>.Remove(KeyValuePair<DateTime, string> pCollection)
        {
            var ret = ((IDictionary<DateTime, string>)m_pDicLog).Remove(pCollection);
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

        public IEnumerator<KeyValuePair<DateTime, string>> GetEnumerator()
        {
            if (IsOrdered)
            {
                return GetOrderedEnumerator();
            }
            else
            {
                return m_pDicLog.GetEnumerator();
            }
        }

        private IEnumerator<KeyValuePair<DateTime, string>> GetOrderedEnumerator()
        {
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                yield return new KeyValuePair<DateTime, string>(m_pListKeyOrdered[i], m_pDicLog[m_pListKeyOrdered[i]]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator LogRepository(Dictionary<DateTime, string> pDic)
        {
            return new LogRepository(pDic);
        }

        public static explicit operator Dictionary<DateTime, string>(LogRepository pRepo)
        {
            return pRepo.m_pDicLog;
        }
    }
}
