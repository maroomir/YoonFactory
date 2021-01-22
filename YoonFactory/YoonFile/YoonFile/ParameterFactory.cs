using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoonFactory;
using YoonFactory.Files;
using System.IO;

namespace YoonFactory.Files.Core
{
    public class YoonParameter
    {
        public IYoonParameter Parameter { get; set; }
        public Type ParameterType { get; private set; }

        public string RootDirectory { get; set; }

        public YoonParameter()
        {
            Parameter = null;
            ParameterType = typeof(object);
            RootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory");
        }

        public YoonParameter(IYoonParameter pParam, Type pType)
        {
            Parameter = pParam.Clone();
            ParameterType = pType;
            RootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory");
        }

        public void SetParameter(IYoonParameter pParam, Type pType)
        {
            Parameter = pParam.Clone();
            ParameterType = pType;
        }

        public bool IsEqual(YoonParameter pParam)
        {
            if (pParam.Parameter.IsEqual(Parameter) && pParam.ParameterType == ParameterType && pParam.RootDirectory == RootDirectory)
                return true;
            else return false;
        }

        public bool SaveParameter()
        {
            return SaveParameter(ParameterType.FullName);
        }

        public bool SaveParameter(string strFileName)
        {
            if (RootDirectory == string.Empty || Parameter == null) return false;

            string strFilePath = Path.Combine(RootDirectory, string.Format(@"{0}.xml", strFileName));
            YoonXml pXml = new YoonXml(strFilePath);
            if (pXml.SaveFile(Parameter, ParameterType))
                return true;
            else
                return false;
        }

        public bool LoadParameter(bool bSaveIfFalse = false)
        {
            return LoadParameter(ParameterType.FullName, bSaveIfFalse);
        }

        public bool LoadParameter(string strFileName, bool bSaveIfFalse = false)
        {
            if (RootDirectory == string.Empty || Parameter == null) return false;

            object pParam;
            string strFilePath = Path.Combine(RootDirectory, string.Format(@"{0}.xml", strFileName));
            IYoonParameter pParamBk = Parameter.Clone();
            YoonXml pXml = new YoonXml(strFilePath);
            if (pXml.LoadFile(out pParam, ParameterType))
            {
                Parameter = pParam as IYoonParameter;
                if (Parameter != null)
                    return true;
                Parameter = pParamBk;
                if (bSaveIfFalse) SaveParameter();
            }
            return false;
        }
    }

    public class YoonContainer : IYoonContainer, IYoonContainer<string, YoonParameter>
    {
        #region IDisposable Support
        ~YoonContainer()
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

        public static IEqualityComparer<string> DefaultComparer;

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

        public string RootDirectory { get; set; }

        private Dictionary<string, YoonParameter> m_pDicParam;
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
                    m_pListKeyOrdered = value ? new List<string>(m_pDicParam.Keys) : null;
                }
            }
        }

        public IEqualityComparer<string> Comparer { get { return m_pDicParam.Comparer; } }

        public YoonParameter this[int nIndex]
        {
            get
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index ParameterContainer using integer key: container was not ordered.");
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
                    throw new InvalidOperationException("Cannot index ParameterContainer using integer key: container was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                var key = m_pListKeyOrdered[nIndex];
                m_pDicParam[key] = value;
            }
        }

        public YoonParameter this[string strKey]
        {
            get
            {
                YoonParameter pParam;
                if (m_pDicParam.TryGetValue(strKey, out pParam))
                {
                    return pParam;
                }
                return new YoonParameter();
            }
            set
            {
                if (IsOrdered && !m_pListKeyOrdered.Contains(strKey, Comparer))
                {
                    m_pListKeyOrdered.Add(strKey);
                }
                m_pDicParam[strKey] = value;
            }
        }


        public YoonContainer()
            : this(DefaultComparer)
        {
            //
        }

        public YoonContainer(IEqualityComparer<string> pStringComparer)
        {
            this.m_pDicParam = new Dictionary<string, YoonParameter>(pStringComparer);
        }

        public YoonContainer(Dictionary<string, YoonParameter> pDic)
            : this(pDic, DefaultComparer)
        {
            //
        }

        public YoonContainer(Dictionary<string, YoonParameter> pDic, IEqualityComparer<string> pStringComparer)
        {
            this.m_pDicParam = new Dictionary<string, YoonParameter>(pDic, pStringComparer);
        }

        public YoonContainer(YoonContainer pContainer)
            : this(pContainer, default)
        {
            //
        }

        public YoonContainer(YoonContainer pContainer, IEqualityComparer<string> pStringComparer)
        {
            this.m_pDicParam = new Dictionary<string, YoonParameter>(pContainer.m_pDicParam, pStringComparer);
        }

        public void CopyFrom(IYoonContainer pContainer)
        {
            if (pContainer is YoonContainer pParamContainer)
            {
                Clear();
                foreach (string strKey in pParamContainer.Keys)
                {
                    Add(strKey, pParamContainer[strKey]);
                }
            }
        }

        public IYoonContainer Clone()
        {
            return new YoonContainer(this, Comparer);
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
        public bool LoadValue(string strKey)
        {
            if (RootDirectory == string.Empty || strKey == string.Empty)
                return false;

            m_pDicParam[strKey].RootDirectory = RootDirectory;
            if (!m_pDicParam.ContainsKey(strKey))
                Add(strKey, new YoonParameter());
            if (m_pDicParam[strKey].LoadParameter(strKey)) return true;
            else return false;
        }

        public bool SaveValue(string strKey)
        {
            if (RootDirectory == string.Empty || strKey == string.Empty || !m_pDicParam.ContainsKey(strKey))
                return false;

            if (!m_pDicParam.ContainsKey(strKey)) return false;
            m_pDicParam[strKey].RootDirectory = RootDirectory;
            return m_pDicParam[strKey].SaveParameter(strKey);
        }

        public int IndexOf(string strKey)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string) on ParameterContainer: container was not ordered.");
            }
            return IndexOf(strKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(string strKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string, int) on ParameterContainer: container was not ordered.");
            }
            return IndexOf(strKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(string strKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string, int, int) on ParameterContainer: container was not ordered.");
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
                throw new InvalidOperationException("Cannot call LastIndexOf(string) on ParameterContainer: container was not ordered.");
            }
            return LastIndexOf(strKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(string strKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(string, int) on ParameterContainer: container was not ordered.");
            }
            return LastIndexOf(strKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(string strKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(string, int, int) on ParameterContainer: container was not ordered.");
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

        public void Insert(int nIndex, string strKey, YoonParameter pValue)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Insert(int, string, YoonParameter) on ParameterContainer: container was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            m_pDicParam.Add(strKey, pValue);
            m_pListKeyOrdered.Insert(nIndex, strKey);
        }

        public void InsertRange(int nIndex, IEnumerable<KeyValuePair<string, YoonParameter>> pCollection)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<string, YoonParameter>>) on ParameterContainer: container was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveAt(int) on ParameterContainer: container was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on ParameterContainer: container was not ordered.");
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
                throw new InvalidOperationException("Cannot call Reverse() on ParameterContainer: container was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on ParameterContainer: container was not ordered.");
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

        public ICollection<YoonParameter> GetOrderedValues()
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call GetOrderedValues() on ParameterContainer: container was not ordered.");
            }
            var list = new List<YoonParameter>();
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                list.Add(m_pDicParam[m_pListKeyOrdered[i]]);
            }
            return list;
        }

        public void Add(string strKey, YoonParameter pValue)
        {
            m_pDicParam.Add(strKey, pValue);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(strKey);
            }
        }

        public bool ContainsKey(string strKey)
        {
            return m_pDicParam.ContainsKey(strKey);
        }

        public ICollection<string> Keys
        {
            get { return IsOrdered ? (ICollection<string>)m_pListKeyOrdered : m_pDicParam.Keys; }
        }

        public ICollection<YoonParameter> Values
        {
            get
            {
                return m_pDicParam.Values;
            }
        }

        public bool Remove(string strKey)
        {
            var ret = m_pDicParam.Remove(strKey);
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

        public bool TryGetValue(string strKey, out YoonParameter pValue)
        {
            return m_pDicParam.TryGetValue(strKey, out pValue);
        }

        public int Count
        {
            get { return m_pDicParam.Count; }
        }

        void ICollection<KeyValuePair<string, YoonParameter>>.Add(KeyValuePair<string, YoonParameter> pCollection)
        {
            ((IDictionary<string, YoonParameter>)m_pDicParam).Add(pCollection);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(pCollection.Key);
            }
        }

        bool ICollection<KeyValuePair<string, YoonParameter>>.Contains(KeyValuePair<string, YoonParameter> pCollection)
        {
            return ((IDictionary<string, YoonParameter>)m_pDicParam).Contains(pCollection);
        }

        void ICollection<KeyValuePair<string, YoonParameter>>.CopyTo(KeyValuePair<string, YoonParameter>[] pArray, int nIndexArray)
        {
            ((IDictionary<string, YoonParameter>)m_pDicParam).CopyTo(pArray, nIndexArray);
        }

        bool ICollection<KeyValuePair<string, YoonParameter>>.IsReadOnly
        {
            get { return ((IDictionary<string, YoonParameter>)m_pDicParam).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<string, YoonParameter>>.Remove(KeyValuePair<string, YoonParameter> pCollection)
        {
            var ret = ((IDictionary<string, YoonParameter>)m_pDicParam).Remove(pCollection);
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

        public IEnumerator<KeyValuePair<string, YoonParameter>> GetEnumerator()
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

        private IEnumerator<KeyValuePair<string, YoonParameter>> GetOrderedEnumerator()
        {
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                yield return new KeyValuePair<string, YoonParameter>(m_pListKeyOrdered[i], m_pDicParam[m_pListKeyOrdered[i]]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator YoonContainer(Dictionary<string, YoonParameter> pDic)
        {
            return new YoonContainer(pDic);
        }

        public static explicit operator Dictionary<string, YoonParameter>(YoonContainer pContainer)
        {
            return pContainer.m_pDicParam;
        }
    }

    public class YoonTemplate : IYoonTemplate, IYoonTemplate<string, YoonParameter>
    {
        #region IDisposable Support
        ~YoonTemplate()
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
                if (Container != null)
                    Container.Dispose();
                Container = null;

            }
            //// .NET Framework에 의하여 관리되지 않는 외부 리소스들을 여기서 정리합니다.
            this.disposed = true;
        }
        #endregion

        public int No { get; set; }
        public string Name { get; set; }
        public string RootDirectory { get; set; }
        public IYoonContainer<string, YoonParameter> Container { get; set; }

        public YoonTemplate()
        {
            No = 0;
            Name = "Default";
            RootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory");
            Container = new YoonContainer();
        }

        public void CopyFrom(IYoonTemplate pTemplate)
        {
            if (pTemplate is YoonTemplate pTempOrigin)
            {
                Container.Dispose();
                Container = null;

                No = pTempOrigin.No;
                Name = pTempOrigin.Name;
                RootDirectory = pTempOrigin.RootDirectory;
                Container = pTempOrigin.Container.Clone() as YoonContainer;
            }
        }

        public IYoonTemplate Clone()
        {
            YoonTemplate pTemplate = new YoonTemplate();
            pTemplate.No = No;
            pTemplate.Name = Name;
            pTemplate.RootDirectory = RootDirectory;
            pTemplate.Container = Container.Clone() as YoonContainer;

            return pTemplate;
        }

        public bool LoadTemplate()
        {
            if (RootDirectory == string.Empty || Container == null)
                return false;

            bool bResult = true;
            string strSection = string.Format("{0}_{1}", No, Name);
            string strIniFilePath = Path.Combine(RootDirectory, @"YoonTemplate.ini");
            Container.RootDirectory = Path.Combine(RootDirectory, strSection);
            YoonIni pIni = new YoonIni(strIniFilePath);
            {
                pIni.LoadFile();
                int nCount = pIni[strSection]["Count"].ToInt(0);
                for (int iParam = 0; iParam < nCount; iParam++)
                {
                    string strKey = pIni[strSection][iParam].ToString(string.Empty);
                    if (!Container.LoadValue(strKey))
                        bResult = false;
                }
            }
            return bResult;
        }

        public bool SaveTemplate()
        {
            if (RootDirectory == string.Empty || Container == null)
                return false;

            bool bResult = true;
            int iParam = 0;
            string strSection = string.Format("{0}_{1}", No, Name);
            string strIniFilePath = Path.Combine(RootDirectory, @"YoonTemplate.ini");
            Container.RootDirectory = Path.Combine(RootDirectory, strSection);
            YoonIni pIni = new YoonIni(strIniFilePath);
            pIni[strSection]["Count"] = Container.Count;
            foreach (string strKey in Container.Keys)
            {
                pIni[strSection][iParam] = strKey;
                if (!Container.SaveValue(strKey))
                    bResult = false;
                iParam++;
            }
            pIni.SaveFile();
            return bResult;
        }
    }
}
