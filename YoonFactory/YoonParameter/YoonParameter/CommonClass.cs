﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoonFactory.Files;
using System.IO;

namespace YoonFactory.Param
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

        public bool Equals(YoonParameter pParam)
        {
            if (pParam.Parameter.Equals(Parameter) && pParam.ParameterType == ParameterType && pParam.RootDirectory == RootDirectory)
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

    public class YoonContainer<T> : IYoonContainer, IYoonContainer<T, YoonParameter> where T : IConvertible
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

        public static IEqualityComparer<T> DefaultComparer = new CaseInsensitiveComparer();

        class CaseInsensitiveComparer : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }

        public string FilesDirectory { get; set; }

        protected Dictionary<T, YoonParameter> m_pDicParam;
        protected List<T> m_pListKeyOrdered;

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
                    m_pListKeyOrdered = value ? new List<T>(m_pDicParam.Keys) : null;
                }
            }
        }

        public IEqualityComparer<T> Comparer { get { return m_pDicParam.Comparer; } }

        public YoonParameter this[int nIndex]
        {
            get
            {
                if (!IsOrdered)
                {
                    throw new InvalidOperationException("Cannot index ParameterContainer using integer key: section was not ordered.");
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
                    throw new InvalidOperationException("Cannot index ParameterContainer using integer key: section was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                var key = m_pListKeyOrdered[nIndex];
                m_pDicParam[key] = value;
            }
        }

        public YoonParameter this[T pKey]
        {
            get
            {
                YoonParameter pParam;
                if (m_pDicParam.TryGetValue(pKey, out pParam))
                {
                    return pParam;
                }
                return new YoonParameter();
            }
            set
            {
                if (IsOrdered && !m_pListKeyOrdered.Contains(pKey, Comparer))
                {
                    m_pListKeyOrdered.Add(pKey);
                }
                m_pDicParam[pKey] = value;
            }
        }

        public YoonContainer()
        {
            this.m_pDicParam = new Dictionary<T, YoonParameter>(DefaultComparer);
        }

        public YoonContainer(Dictionary<T, YoonParameter> pDic)
        {
            this.m_pDicParam = new Dictionary<T, YoonParameter>(pDic, DefaultComparer);
        }

        public YoonContainer(YoonContainer<T> pContainer)
        {
            this.m_pDicParam = new Dictionary<T, YoonParameter>(pContainer.m_pDicParam, DefaultComparer);
        }

        public void CopyFrom(IYoonContainer pContainer)
        {
            if (pContainer is YoonContainer<T> pParamContainer)
            {
                Clear();
                foreach (T pKey in pParamContainer.Keys)
                {
                    Add(pKey, pParamContainer[pKey]);
                }
            }
        }

        IYoonContainer IYoonContainer.Clone()
        {
            return new YoonContainer<T>(this);
        }

        public IYoonContainer<T, YoonParameter> Clone()
        {
            return new YoonContainer<T>(this);
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

        public bool LoadValue(T pKey)
        {
            if (FilesDirectory == string.Empty || pKey == null)
                return false;

            if (!m_pDicParam.ContainsKey(pKey))
                Add(pKey, new YoonParameter());
            m_pDicParam[pKey].RootDirectory = FilesDirectory;
            if (m_pDicParam[pKey].LoadParameter(pKey.ToString())) return true;
            else return false;
        }

        public bool SaveValue(T pKey)
        {
            if (FilesDirectory == string.Empty || pKey == null || !m_pDicParam.ContainsKey(pKey))
                return false;

            m_pDicParam[pKey].RootDirectory = FilesDirectory;
            return m_pDicParam[pKey].SaveParameter(pKey.ToString());
        }

        public int IndexOf(T pKey)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(T) on ParameterContainer: section was not ordered.");
            }
            return IndexOf(pKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(T pKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(T, int) on ParameterContainer: section was not ordered.");
            }
            return IndexOf(pKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(T pKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call IndexOf(T, int, int) on ParameterContainer: section was not ordered.");
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
                if(Comparer.Equals(m_pListKeyOrdered[i], pKey))
                {
                    return i;
                }
            }
            return -1;
        }

        public int LastIndexOf(T strKey)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(T) on ParameterContainer: section was not ordered.");
            }
            return LastIndexOf(strKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(T strKey, int nIndex)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(T, int) on ParameterContainer: section was not ordered.");
            }
            return LastIndexOf(strKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(T pKey, int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(T, int, int) on ParameterContainer: section was not ordered.");
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

        public void Insert(int nIndex, T pKey, YoonParameter pValue)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Insert(int, T, YoonParameter) on ParameterContainer: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            m_pDicParam.Add(pKey, pValue);
            m_pListKeyOrdered.Insert(nIndex, pKey);
        }

        public void InsertRange(int nIndex, IEnumerable<KeyValuePair<T, YoonParameter>> pCollection)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<T, YoonParameter>>) on ParameterContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveAt(int) on ParameterContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on ParameterContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call Reverse() on ParameterContainer: section was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (!IsOrdered)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on ParameterContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call GetOrderedValues() on ParameterContainer: section was not ordered.");
            }
            var list = new List<YoonParameter>();
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                list.Add(m_pDicParam[m_pListKeyOrdered[i]]);
            }
            return list;
        }

        public void Add(T pKey, YoonParameter pValue)
        {
            m_pDicParam.Add(pKey, pValue);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(pKey);
            }
        }

        public bool ContainsKey(T pKey)
        {
            return m_pDicParam.ContainsKey(pKey);
        }

        public ICollection<T> Keys
        {
            get { return IsOrdered ? (ICollection<T>)m_pListKeyOrdered : m_pDicParam.Keys; }
        }

        public ICollection<YoonParameter> Values
        {
            get
            {
                return m_pDicParam.Values;
            }
        }

        public bool Remove(T pKey)
        {
            var ret = m_pDicParam.Remove(pKey);
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

        public bool TryGetValue(T pKey, out YoonParameter pValue)
        {
            return m_pDicParam.TryGetValue(pKey, out pValue);
        }

        public int Count
        {
            get { return m_pDicParam.Count; }
        }

        void ICollection<KeyValuePair<T, YoonParameter>>.Add(KeyValuePair<T, YoonParameter> pCollection)
        {
            ((IDictionary<T, YoonParameter>)m_pDicParam).Add(pCollection);
            if (IsOrdered)
            {
                m_pListKeyOrdered.Add(pCollection.Key);
            }
        }

        bool ICollection<KeyValuePair<T, YoonParameter>>.Contains(KeyValuePair<T, YoonParameter> pCollection)
        {
            return ((IDictionary<T, YoonParameter>)m_pDicParam).Contains(pCollection);
        }

        void ICollection<KeyValuePair<T, YoonParameter>>.CopyTo(KeyValuePair<T, YoonParameter>[] pArray, int nIndexArray)
        {
            ((IDictionary<T, YoonParameter>)m_pDicParam).CopyTo(pArray, nIndexArray);
        }

        bool ICollection<KeyValuePair<T, YoonParameter>>.IsReadOnly
        {
            get { return ((IDictionary<T, YoonParameter>)m_pDicParam).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<T, YoonParameter>>.Remove(KeyValuePair<T, YoonParameter> pCollection)
        {
            var ret = ((IDictionary<T, YoonParameter>)m_pDicParam).Remove(pCollection);
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

        public IEnumerator<KeyValuePair<T, YoonParameter>> GetEnumerator()
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

        private IEnumerator<KeyValuePair<T, YoonParameter>> GetOrderedEnumerator()
        {
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                yield return new KeyValuePair<T, YoonParameter>(m_pListKeyOrdered[i], m_pDicParam[m_pListKeyOrdered[i]]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator YoonContainer<T>(Dictionary<T, YoonParameter> pDic)
        {
            return new YoonContainer<T>(pDic);
        }

        public static explicit operator Dictionary<T, YoonParameter>(YoonContainer<T> pContainer)
        {
            return pContainer.m_pDicParam;
        }
    }

    public class YoonTemplate<T> : YoonContainer<T>, IYoonTemplate where T : IConvertible
    {
        public int No { get; set; }
        public string Name { get; set; }
        public string RootDirectory { get; set; }

        public override string ToString()
        {
            return string.Format("{0:D2}_{1}", No, Name);
        }

        public YoonTemplate()
        {
            No = 0;
            Name = "Default";
            RootDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "YoonFactory");
            m_pDicParam = new Dictionary<T, YoonParameter>(DefaultComparer);
        }

        public void CopyFrom(IYoonTemplate pTemplate)
        {
            if (pTemplate is YoonTemplate<T> pTempOrigin)
            {
                Clear();

                No = pTempOrigin.No;
                Name = pTempOrigin.Name;
                RootDirectory = pTempOrigin.RootDirectory;
                foreach (T pKey in pTempOrigin.Keys)
                {
                    Add(pKey, pTempOrigin[pKey]);
                }
            }
        }

        public new IYoonTemplate Clone()
        {
            YoonTemplate<T> pTemplate = new YoonTemplate<T>();
            {
                pTemplate.No = No;
                pTemplate.Name = Name;
                pTemplate.RootDirectory = RootDirectory;
                pTemplate.m_pDicParam = new Dictionary<T, YoonParameter>(m_pDicParam, DefaultComparer);
            }
            return pTemplate;
        }

        public bool LoadTemplate()
        {
            if (RootDirectory == string.Empty || m_pDicParam == null)
                return false;

            string strIniFilePath = Path.Combine(RootDirectory, @"YoonTemplate.ini");
            base.FilesDirectory = Path.Combine(RootDirectory, ToString());
            bool bResult = true;
            using (YoonIni pIni = new YoonIni(strIniFilePath))
            {
                pIni.LoadFile();
                No = pIni["HEAD"]["No"].ToInt(No);
                Name = pIni["HEAD"]["Name"].ToString(Name);
                int nCount = pIni["HEAD"]["Count"].ToInt(0);
                for (int iParam = 0; iParam < nCount; iParam++)
                {
                    T pKey = pIni["KEY"][iParam.ToString()].To(default(T));
                    if (!LoadValue(pKey))
                        bResult = false;
                }
            }
            return bResult;
        }

        public bool SaveTemplate()
        {
            if (RootDirectory == string.Empty || m_pDicParam == null)
                return false;

            string strIniFilePath = Path.Combine(RootDirectory, @"YoonTemplate.ini");
            base.FilesDirectory = Path.Combine(RootDirectory, ToString());
            bool bResult = true;
            using (YoonIni pIni = new YoonIni(strIniFilePath))
            {
                int iParam = 0;
                pIni["HEAD"]["No"] = No;
                pIni["HEAD"]["Name"] = Name;
                pIni["HEAD"]["Count"] = Count;
                foreach (T pKey in Keys)
                {
                    pIni["KEY"][(iParam++).ToString()] = pKey.ToString();
                    if (!SaveValue(pKey))
                        bResult = false;
                }
                pIni.SaveFile();
            }
            return bResult;
        }
    }

    public class CommonTemplate<TKey, TValue> : IYoonTemplate where TKey : IConvertible
    {
        #region IDisposable Support
        ~CommonTemplate()
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

        public override string ToString()
        {
            return string.Format("{0:D2}_{1}", No, Name);
        }

        public IYoonContainer<TKey, TValue> Container { get; set; }


        public CommonTemplate(IYoonContainer<TKey, TValue> pContainer)
        {
            No = 0;
            Name = "Default";
            RootDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "YoonFactory");
            Container = pContainer;
            RootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory");
        }

        public void CopyFrom(IYoonTemplate pTemplate)
        {
            if (pTemplate is CommonTemplate<TKey, TValue> pTempOrigin)
            {
                Container.Dispose();
                Container = null;

                No = pTempOrigin.No;
                Name = pTempOrigin.Name;
                RootDirectory = pTempOrigin.RootDirectory;
                Container = pTempOrigin.Container.Clone();
            }
        }

        public IYoonTemplate Clone()
        {
            CommonTemplate<TKey, TValue> pTemplate = new CommonTemplate<TKey, TValue>(Container);
            pTemplate.No = No;
            pTemplate.Name = Name;
            pTemplate.RootDirectory = RootDirectory;

            return pTemplate;
        }

        public bool LoadTemplate()
        {
            if (RootDirectory == string.Empty || Container == null)
                return false;

            string strIniFilePath = Path.Combine(RootDirectory, @"CommonTemplate.ini");
            Container.FilesDirectory = Path.Combine(RootDirectory, ToString());
            bool bResult = true;
            using (YoonIni pIni = new YoonIni(strIniFilePath))
            {
                pIni.LoadFile();
                string strHeadName = "HEAD:" + typeof(TValue).ToString();
                No = pIni[strHeadName]["No"].ToInt(No);
                Name = pIni[strHeadName]["Name"].ToString(Name);
                int nCount = pIni[strHeadName]["Count"].ToInt(0);
                for (int iParam = 0; iParam < nCount; iParam++)
                {
                    string strKeyName = "KEY:" + typeof(TValue).ToString();
                    TKey pKey = pIni[strKeyName][iParam.ToString()].To(default(TKey));
                    if (!Container.LoadValue(pKey))
                        bResult = false;
                }
            }
            return bResult;
        }

        public bool SaveTemplate()
        {
            if (RootDirectory == string.Empty || Container == null)
                return false;

            string strIniFilePath = Path.Combine(RootDirectory, @"CommonTemplate.ini");
            Container.FilesDirectory = Path.Combine(RootDirectory, ToString());
            bool bResult = true;
            using (YoonIni pIni = new YoonIni(strIniFilePath))
            {
                int iParam = 0;
                string strHeadName = "HEAD:" + typeof(TValue).ToString();
                pIni[strHeadName]["No"] = No;
                pIni[strHeadName]["Name"] = Name;
                pIni[strHeadName]["Count"] = Container.Count;
                foreach (TKey pKey in Container.Keys)
                {
                    string strKeyName = "KEY:" + typeof(TValue).ToString();
                    pIni[strKeyName][(iParam++).ToString()] = pKey.ToString();
                    if (!Container.SaveValue(pKey))
                        bResult = false;
                }
                pIni.SaveFile();
            }
            return bResult;
        }
    }

}