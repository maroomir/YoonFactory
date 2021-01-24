using System;
using System.Collections.Generic;
using System.IO;
using YoonFactory.Comm;
using YoonFactory.Files;

namespace YoonFactory.Robot
{
    public class ParameterRemote
    {
        public string IPAddress { get; set; } = "127.0.0.1";
        public string Port { get; set; } = "30000";
        public eYoonCommType TCPMode { get; set; } = eYoonCommType.TCPServer;
    }

    public class RemoteSection : IYoonContainer, IYoonContainer<eYoonRemoteType, ParameterRemote>
    {
        #region IDisposable Support
        ~RemoteSection()
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
            m_pDicParam = null;
            m_pListKeyOrdered = null;
            this.disposed = true;
        }
        #endregion

        public static IEqualityComparer<eYoonRemoteType> DefaultComparer = new CaseInsensitiveRemoteTypeComparer();

        class CaseInsensitiveRemoteTypeComparer : IEqualityComparer<eYoonRemoteType>
        {
            public bool Equals(eYoonRemoteType x, eYoonRemoteType y)
            {
                return (x == y);
            }

            public int GetHashCode(eYoonRemoteType obj)
            {
                return obj.GetHashCode();
            }
        }

        public string RootDirectory { get; set; }
        public eYoonRobotType ParantsType { get; set; } = eYoonRobotType.None;

        private Dictionary<eYoonRemoteType, ParameterRemote> m_pDicParam;
        private List<eYoonRemoteType> m_pListKeyOrdered;

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
                    m_pListKeyOrdered = value ? new List<eYoonRemoteType>(m_pDicParam.Keys) : null;
                }
            }
        }

        public IEqualityComparer<eYoonRemoteType> Comparer { get { return m_pDicParam.Comparer; } }

        public static RemoteSection DefaultUR
        {
            get
            {
                RemoteSection pSection = new RemoteSection(eYoonRobotType.UR);
                foreach (eYoonRemoteType nType in Enum.GetValues(typeof(eYoonRemoteType)))
                    pSection.Add(nType, pSection.GetInitRemoteParam(nType));
                return pSection;
            }
        }

        public static RemoteSection DefaultTM
        {
            get
            {
                RemoteSection pSection = new RemoteSection(eYoonRobotType.TM);
                foreach (eYoonRemoteType nType in Enum.GetValues(typeof(eYoonRemoteType)))
                    pSection.Add(nType, pSection.GetInitRemoteParam(nType));
                return pSection;
            }
        }


        public ParameterRemote this[int nIndex]
        {
            get
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index ParameterSection using integer key: container was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                return m_pDicParam[m_pListKeyOrdered[nIndex]];
            }
            set
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index ParameterSection using integer key: container was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                var key = m_pListKeyOrdered[nIndex];
                m_pDicParam[key] = value;
            }
        }

        public ParameterRemote this[eYoonRemoteType nKey]
        {
            get
            {
                ParameterRemote pParam;
                if (m_pDicParam.TryGetValue(nKey, out pParam))
                {
                    return pParam;
                }
                return GetInitRemoteParam(nKey);
            }
            set
            {
                if (IsOrdered && !m_pListKeyOrdered.Contains(nKey))
                {
                    m_pListKeyOrdered.Add(nKey);
                }
                m_pDicParam[nKey] = value;
            }
        }

        public RemoteSection()
            : this(DefaultComparer)
        {
            //
        }

        public RemoteSection(IEqualityComparer<eYoonRemoteType> pTypeComparer)
        {
            this.m_pDicParam = new Dictionary<eYoonRemoteType, ParameterRemote>(pTypeComparer);
        }

        public RemoteSection(eYoonRobotType nType)
            : this(nType, DefaultComparer)
        {
            //
        }

        public RemoteSection(eYoonRobotType nType, IEqualityComparer<eYoonRemoteType> pTypeComparer)
        {
            this.ParantsType = nType;
            this.m_pDicParam = new Dictionary<eYoonRemoteType, ParameterRemote>(pTypeComparer);
        }

        public RemoteSection(Dictionary<eYoonRemoteType, ParameterRemote> pDic)
            : this(pDic, DefaultComparer)
        {
            //
        }

        public RemoteSection(Dictionary<eYoonRemoteType, ParameterRemote> pDic, IEqualityComparer<eYoonRemoteType> pTypeComparer)
        {
            this.m_pDicParam = new Dictionary<eYoonRemoteType, ParameterRemote>(pDic, pTypeComparer);
        }

        public RemoteSection(RemoteSection pSection)
            : this(pSection, DefaultComparer)
        {
            //
        }

        public RemoteSection(RemoteSection pSection, IEqualityComparer<eYoonRemoteType> pTypeComparer)
        {
            this.m_pDicParam = new Dictionary<eYoonRemoteType, ParameterRemote>(pSection.m_pDicParam, pTypeComparer);
        }

        public void CopyFrom(IYoonContainer pContainer)
        {
            if (pContainer is RemoteSection pSection)
            {
                Clear();
                foreach (eYoonRemoteType nKey in pSection.Keys)
                {
                    Add(nKey, pSection[nKey]);
                }
            }
        }

        public IYoonContainer Clone()
        {
            return new RemoteSection(this, Comparer);
        }

        public void Clear()
        {
            if (m_pDicParam != null)
                m_pDicParam.Clear();
            if (IsOrdered)
            {
                m_pListKeyOrdered.Clear();
            }
        }

        private ParameterRemote GetInitRemoteParam(eYoonRemoteType nKey)
        {
            switch (ParantsType)
            {
                case eYoonRobotType.UR:
                    return UniversalRobotics.InitRemoteParameter(nKey);
                case eYoonRobotType.TM:
                    return TechMan.InitRemoteParameter(nKey);
                default:
                    return new ParameterRemote();
            }
        }

        private string GetRemoteIniFilePath(eYoonRemoteType nRemote)
        {
            return Path.Combine(RootDirectory, string.Format(@"Remote{0}{1}.ini", ParantsType.ToString(), nRemote.ToString()));
        }

        private ParameterRemote LoadRemoteFileFromIni(string strFilePath, eYoonRemoteType nKey)
        {
            if (RootDirectory == string.Empty) return null;

            ParameterRemote pRemote = new ParameterRemote();
            string strKey = nKey.ToString();
            YoonIni ic = new YoonIni(strFilePath);
            ic.LoadFile();
            pRemote.IPAddress = ic[strKey]["IP"].ToString("127.0.0.1");
            pRemote.Port = ic[strKey]["Port"].ToString("1234");
            pRemote.TCPMode = ic[strKey]["Mode"].ToEnum<eYoonCommType>(eYoonCommType.TCPClient);
            return pRemote;
        }

        private bool SaveRemoteFileToIni(string strFilePath, eYoonRemoteType nKey, ParameterRemote pRemote)
        {
            if (RootDirectory == string.Empty) return false;

            string strKey = nKey.ToString();
            YoonIni ic = new YoonIni(strFilePath);
            ic[strKey]["IP"] = pRemote.IPAddress;
            ic[strKey]["Port"] = pRemote.Port;
            ic[strKey]["Mode"] = pRemote.TCPMode.ToString();
            ic.SaveFile();
            return true;
        }

        public bool LoadValue(eYoonRemoteType nKey)
        {
            if (RootDirectory == string.Empty) return false;

            string strFilePath = GetRemoteIniFilePath(nKey);
            ParameterRemote pParam = LoadRemoteFileFromIni(strFilePath, nKey);
            if (pParam == null) return false;

            if (!m_pDicParam.ContainsKey(nKey))
                Add(nKey, pParam);
            else
                m_pDicParam[nKey] = pParam;
            return true;
        }

        public bool SaveValue(eYoonRemoteType nKey)
        {
            if (RootDirectory == string.Empty) return false;

            if (!m_pDicParam.ContainsKey(nKey)) return false;
            string strFilePath = GetRemoteIniFilePath(nKey);
            return SaveRemoteFileToIni(strFilePath, nKey, m_pDicParam[nKey]);
        }

        public int IndexOf(eYoonRemoteType nKey)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonRemoteType) on ParameterSection: container was not ordered.");
            }
            return IndexOf(nKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(eYoonRemoteType nKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonRemoteType, int) on ParameterSection: container was not ordered.");
            }
            return IndexOf(nKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(eYoonRemoteType nKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonRemoteType, int, int) on ParameterSection: container was not ordered.");
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

        public int LastIndexOf(eYoonRemoteType nKey)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonRemoteType) on ParameterSection: container was not ordered.");
            }
            return LastIndexOf(nKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(eYoonRemoteType nKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonRemoteType, int) on ParameterSection: container was not ordered.");
            }
            return LastIndexOf(nKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(eYoonRemoteType nKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonRemoteType, int, int) on ParameterSection: container was not ordered.");
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

        public void Insert(int nIndex, eYoonRemoteType nKey, ParameterRemote pValue)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Insert(int, eYoonRemoteType, ParameterRemote) on ParameterSection: container was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            m_pDicParam.Add(nKey, pValue);
            m_pListKeyOrdered.Insert(nIndex, nKey);
        }

        public void InsertRange(int nIndex, IEnumerable<KeyValuePair<eYoonRemoteType, ParameterRemote>> pCollection)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<eYoonRemoteType, ParameterRemote>>) on ParameterSection: container was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveAt(int) on ParameterSection: container was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            var key = m_pListKeyOrdered[nIndex];
            m_pListKeyOrdered.RemoveAt(nIndex);
            m_pDicParam.Remove(key);
        }

        public void RemoveRange(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on ParameterSection: container was not ordered.");
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
                throw new InvalidOperationException("Cannot call Reverse() on ParameterSection: container was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on ParameterSection: container was not ordered.");
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

        public ICollection<ParameterRemote> GetOrderedValues()
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call GetOrderedValues() on ParameterSection: container was not ordered.");
            }
            var list = new List<ParameterRemote>();
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                list.Add(m_pDicParam[m_pListKeyOrdered[i]]);
            }
            return list;
        }

        public void Add(eYoonRemoteType nKey, ParameterRemote pValue)
        {
            m_pDicParam.Add(nKey, pValue);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(nKey);
            }
        }

        public bool ContainsKey(eYoonRemoteType nKey)
        {
            return m_pDicParam.ContainsKey(nKey);
        }

        public ICollection<eYoonRemoteType> Keys
        {
            get { return IsOrdered ? (ICollection<eYoonRemoteType>)m_pListKeyOrdered : m_pDicParam.Keys; }
        }

        public ICollection<ParameterRemote> Values
        {
            get
            {
                return m_pDicParam.Values;
            }
        }

        public bool Remove(eYoonRemoteType nKey)
        {
            var ret = m_pDicParam.Remove(nKey);
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

        public bool TryGetValue(eYoonRemoteType nKey, out ParameterRemote pValue)
        {
            return m_pDicParam.TryGetValue(nKey, out pValue);
        }

        public int Count
        {
            get { return m_pDicParam.Count; }
        }

        void ICollection<KeyValuePair<eYoonRemoteType, ParameterRemote>>.Add(KeyValuePair<eYoonRemoteType, ParameterRemote> pCollection)
        {
            ((IDictionary<eYoonRemoteType, ParameterRemote>)m_pDicParam).Add(pCollection);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(pCollection.Key);
            }
        }

        bool ICollection<KeyValuePair<eYoonRemoteType, ParameterRemote>>.Contains(KeyValuePair<eYoonRemoteType, ParameterRemote> pCollection)
        {
            return ((IDictionary<eYoonRemoteType, ParameterRemote>)m_pDicParam).Contains(pCollection);
        }

        void ICollection<KeyValuePair<eYoonRemoteType, ParameterRemote>>.CopyTo(KeyValuePair<eYoonRemoteType, ParameterRemote>[] pArray, int nIndexArray)
        {
            ((IDictionary<eYoonRemoteType, ParameterRemote>)m_pDicParam).CopyTo(pArray, nIndexArray);
        }

        bool ICollection<KeyValuePair<eYoonRemoteType, ParameterRemote>>.IsReadOnly
        {
            get { return ((IDictionary<eYoonRemoteType, ParameterRemote>)m_pDicParam).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<eYoonRemoteType, ParameterRemote>>.Remove(KeyValuePair<eYoonRemoteType, ParameterRemote> pCollection)
        {
            var ret = ((IDictionary<eYoonRemoteType, ParameterRemote>)m_pDicParam).Remove(pCollection);
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

        public IEnumerator<KeyValuePair<eYoonRemoteType, ParameterRemote>> GetEnumerator()
        {
            if (IsOrdered)
            {
                return GetOrderedEnumerator();
            }
            else
            {
                return m_pDicParam.GetEnumerator();
            }
        }

        private IEnumerator<KeyValuePair<eYoonRemoteType, ParameterRemote>> GetOrderedEnumerator()
        {
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                yield return new KeyValuePair<eYoonRemoteType, ParameterRemote>(m_pListKeyOrdered[i], m_pDicParam[m_pListKeyOrdered[i]]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator RemoteSection(Dictionary<eYoonRemoteType, ParameterRemote> pDic)
        {
            return new RemoteSection(pDic);
        }

        public static explicit operator Dictionary<eYoonRemoteType, ParameterRemote>(RemoteSection pContainer)
        {
            return pContainer.m_pDicParam;
        }
    }
}
