using System;
using System.Collections.Generic;
using System.IO;
using YoonFactory.Comm;
using YoonFactory.Files;

namespace YoonFactory.Robot
{

    public enum eYoonRobotType : int
    {
        None = -1,
        UR,
        TM,
    }

    public enum eYoonRemoteType : int
    {
        None = -1,
        Socket,
        Script,
        Dashboard,
    }

    public enum eYoonHeadSend : int   // REMOTE MODE = UR : DASHBOARD
    {
        None = -1,
        StatusServo = 0,
        StatusError,
        StatusRun,
        StatusSafety,
        LoadProject,
        LoadJob,
        Reset,
        Play,
        Pause,
        Stop,
        Quit,
        Shutdown,
        PowerOn,
        PowerOff,
        BreakRelease,
    }

    public enum eYooneHeadReceive : int    // REMOTE MODE = UR : DASHBOARD
    {
        None = -1,
        StatusServoOK = 0,
        StatusServoNG,
        StatusErrorOK,
        StatusErrorNG,
        StatusRunOK,
        StatusRunNG,
        StatusSafetyOK,
        StatusSafetyNG,
        LoadOK,
        LoadNG,
        ResetOK,
        ResetNG,
        PlayOK,
        PlayNG,
        PauseOK,
        PauseNG,
        StopOk,
        StopNG,
        QuitOK,
        ShuwdownOK,
        PowerOnOK,
        PowerOffOK,
        BreakReleaseOK,
    }

    public struct SendValue
    {
        public string ProjectName;  //
        public string ProgramName;  // UR

        public YoonJointD JointPos;  // UR, TM (J1 ~ J6)
        public YoonCartD CartPos;    // UR, TM (X, Y, Z, RX, R

        public int VelocityPercent;

        public string[] ArraySocketParam;   // UR, TM
    }

    public struct ReceiveValue
    {
        public YoonJointD JointPos;   // UR, TM
        public YoonCartD CartPos;     // UR, TM
    }

    public delegate void RemoteResultCallback(object sender, ResultArgs e);
    public class ResultArgs : EventArgs
    {
        public eYoonStatus Status;
        public string Message;
        public eYooneHeadReceive ReceiveHead;
        public ReceiveValue ReceiveData;

        public ResultArgs(eYoonStatus nStatus, string strMessage)
        {
            Status = nStatus;
            ReceiveHead = eYooneHeadReceive.None;
            ReceiveData = new ReceiveValue();
            Message = strMessage;
        }
        public ResultArgs(eYoonStatus nStatus, eYooneHeadReceive nHead, string strMessage)
        {
            Status = nStatus;
            ReceiveHead = eYooneHeadReceive.None;
            ReceiveData = new ReceiveValue();
            Message = strMessage;
        }

        public ResultArgs(eYooneHeadReceive nHead, ReceiveValue pDataReceive, string strMessage)
        {
            Status = eYoonStatus.OK;
            ReceiveHead = nHead;
            Message = strMessage;
            ReceiveData = new ReceiveValue();
            {
                if (pDataReceive.JointPos != null)
                    ReceiveData.JointPos = pDataReceive.JointPos.Clone() as YoonJointD;
                if (pDataReceive.CartPos != null)
                    ReceiveData.CartPos = pDataReceive.CartPos.Clone() as YoonCartD;
            }
        }

    }


    public class RemoteContainer : IYoonContainer, IYoonContainer<eYoonRobotType, RemoteSection>
    {
        #region Supported IDisposable Pattern
        ~RemoteContainer()
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

        public static IEqualityComparer<eYoonRobotType> DefaultComparer = new CaseInsensitiveTypeComparer();

        class CaseInsensitiveTypeComparer : IEqualityComparer<eYoonRobotType>
        {
            public bool Equals(eYoonRobotType x, eYoonRobotType y)
            {
                return (x == y);
            }

            public int GetHashCode(eYoonRobotType obj)
            {
                return obj.GetHashCode();
            }
        }

        public string FilesDirectory { get; set; }

        private Dictionary<eYoonRobotType, RemoteSection> m_pDicSection;
        private List<eYoonRobotType> m_pListKeyOrdered;

        public IEqualityComparer<eYoonRobotType> Comparer { get { return m_pDicSection.Comparer; } }

        public static RemoteContainer Default
        {
            get
            {
                RemoteContainer pContainer = new RemoteContainer();
                pContainer.Add(eYoonRobotType.UR, RemoteSection.DefaultUR);
                pContainer.Add(eYoonRobotType.TM, RemoteSection.DefaultTM);
                return pContainer;
            }
        }

        public RemoteSection this[int nIndex]
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

        public RemoteSection this[eYoonRobotType nKey]
        {
            get
            {
                RemoteSection pSection;
                if (m_pDicSection.TryGetValue(nKey, out pSection))
                {
                    return pSection;
                }
                return new RemoteSection(nKey);
            }
            set
            {
                if (m_pListKeyOrdered != null && !m_pListKeyOrdered.Contains(nKey))
                {
                    m_pListKeyOrdered.Add(nKey);
                }
                m_pDicSection[nKey] = value;
            }
        }


        public RemoteContainer()
            : this(DefaultComparer)
        {
            //
        }

        public RemoteContainer(IEqualityComparer<eYoonRobotType> pComparer)
        {
            this.m_pDicSection = new Dictionary<eYoonRobotType, RemoteSection>(pComparer);
        }

        public RemoteContainer(Dictionary<eYoonRobotType, RemoteSection> pDic)
            : this(pDic, DefaultComparer)
        {
            //
        }

        public RemoteContainer(Dictionary<eYoonRobotType, RemoteSection> pDic, IEqualityComparer<eYoonRobotType> pComparer)
        {
            this.m_pDicSection = new Dictionary<eYoonRobotType, RemoteSection>(pDic, pComparer);
        }

        public RemoteContainer(RemoteContainer pContainer)
            : this(pContainer, DefaultComparer)
        {
            //
        }

        public RemoteContainer(RemoteContainer pContainer, IEqualityComparer<eYoonRobotType> pComparer)
        {
            this.m_pDicSection = new Dictionary<eYoonRobotType, RemoteSection>(pContainer.m_pDicSection, pComparer);
        }

        public void CopyFrom(IYoonContainer pContainer)
        {
            if (pContainer is RemoteContainer pParamContainer)
            {
                Clear();
                foreach (eYoonRobotType nkey in pParamContainer.Keys)
                {
                    Add(nkey, pParamContainer[nkey]);
                }
            }
        }

        IYoonContainer IYoonContainer.Clone()
        {
            return new RemoteContainer(this, Comparer);
        }

        public IYoonContainer<eYoonRobotType, RemoteSection> Clone()
        {
            return new RemoteContainer(this, Comparer);
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

        public bool LoadAll()
        {
            if (FilesDirectory == string.Empty) return false;
            bool bResult = true;
            foreach (eYoonRobotType nKey in m_pDicSection.Keys)
            {
                if (!LoadValue(nKey))
                    bResult = false;
            }
            return bResult;
        }

        public bool LoadValue(eYoonRobotType nKey)
        {
            if (FilesDirectory == string.Empty) return false;

            bool bResult = true;
            if (!m_pDicSection.ContainsKey(nKey))
                Add(nKey, new RemoteSection(nKey));
            else
            {
                m_pDicSection[nKey].ParantsType = nKey;
                foreach (eYoonRemoteType nRemote in m_pDicSection[nKey].Keys)
                {
                    if (!m_pDicSection[nKey].LoadValue(nRemote))
                        bResult = false;
                }
            }
            return bResult;
        }

        public bool SaveAll()
        {
            if (FilesDirectory == string.Empty) return false;
            bool bResult = true;
            foreach (eYoonRobotType nKey in m_pDicSection.Keys)
            {
                if (!SaveValue(nKey))
                    bResult = false;
            }
            return bResult;
        }

        public bool SaveValue(eYoonRobotType nKey)
        {
            if (FilesDirectory == string.Empty) return false;

            if (!m_pDicSection.ContainsKey(nKey))
                return false;

            bool bResult = true;
            m_pDicSection[nKey].ParantsType = nKey;
            foreach (eYoonRemoteType nRemote in m_pDicSection[nKey].Keys)
            {
                if (!m_pDicSection[nKey].SaveValue(nRemote))
                    bResult = false;
            }
            return bResult;
        }

        public int IndexOf(eYoonRobotType nKey)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonRobotType) on RemoteContainer: section was not ordered.");
            }
            return IndexOf(nKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(eYoonRobotType nKey, int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonRobotType, int) on RemoteContainer: section was not ordered.");
            }
            return IndexOf(nKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(eYoonRobotType nKey, int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(eYoonRobotType, int, int) on RemoteContainer: section was not ordered.");
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

        public int LastIndexOf(eYoonRobotType nKey)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonRobotType) on RemoteContainer: section was not ordered.");
            }
            return LastIndexOf(nKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(eYoonRobotType nKey, int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonRobotType, int) on RemoteContainer: section was not ordered.");
            }
            return LastIndexOf(nKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(eYoonRobotType nKey, int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(eYoonRobotType, int, int) on RemoteContainer: section was not ordered.");
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

        public void Insert(int nIndex, eYoonRobotType nKey, RemoteSection pValue)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Insert(int, eYoonRobotType, ParameterSection) on RemoteContainer: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            m_pDicSection.Add(nKey, pValue);
            m_pListKeyOrdered.Insert(nIndex, nKey);
        }

        public void InsertRange(int nIndex, IEnumerable<KeyValuePair<eYoonRobotType, RemoteSection>> pCollection)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<eYoonRobotType, RemoteSection>>) on RemoteContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveAt(int) on RemoteContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on RemoteContainer: section was not ordered.");
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
                throw new InvalidOperationException("Cannot call Reverse() on RemoteContainer: section was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on RemoteContainer: section was not ordered.");
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

        public ICollection<RemoteSection> GetOrderedValues()
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call GetOrderedValues() on RemoteContainer: section was not ordered.");
            }
            var list = new List<RemoteSection>();
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                list.Add(m_pDicSection[m_pListKeyOrdered[i]]);
            }
            return list;
        }

        public void Add(eYoonRobotType nkey, RemoteSection pValue)
        {
            m_pDicSection.Add(nkey, pValue);
            if (m_pListKeyOrdered != null)
            {
                m_pListKeyOrdered.Add(nkey);
            }
        }

        public bool ContainsKey(eYoonRobotType nKey)
        {
            return m_pDicSection.ContainsKey(nKey);
        }

        public ICollection<eYoonRobotType> Keys
        {
            get { return (m_pListKeyOrdered != null) ? (ICollection<eYoonRobotType>)m_pListKeyOrdered : m_pDicSection.Keys; }
        }

        public ICollection<RemoteSection> Values
        {
            get
            {
                return m_pDicSection.Values;
            }
        }

        public bool Remove(eYoonRobotType nKey)
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

        public bool TryGetValue(eYoonRobotType nKey, out RemoteSection pValue)
        {
            return m_pDicSection.TryGetValue(nKey, out pValue);
        }

        public int Count
        {
            get { return m_pDicSection.Count; }
        }

        void ICollection<KeyValuePair<eYoonRobotType, RemoteSection>>.Add(KeyValuePair<eYoonRobotType, RemoteSection> pCollection)
        {
            ((IDictionary<eYoonRobotType, RemoteSection>)m_pDicSection).Add(pCollection);
            if (m_pListKeyOrdered != null)
            {
                m_pListKeyOrdered.Add(pCollection.Key);
            }
        }

        bool ICollection<KeyValuePair<eYoonRobotType, RemoteSection>>.Contains(KeyValuePair<eYoonRobotType, RemoteSection> pCollection)
        {
            return ((IDictionary<eYoonRobotType, RemoteSection>)m_pDicSection).Contains(pCollection);
        }

        void ICollection<KeyValuePair<eYoonRobotType, RemoteSection>>.CopyTo(KeyValuePair<eYoonRobotType, RemoteSection>[] pArray, int nIndexArray)
        {
            ((IDictionary<eYoonRobotType, RemoteSection>)m_pDicSection).CopyTo(pArray, nIndexArray);
        }

        bool ICollection<KeyValuePair<eYoonRobotType, RemoteSection>>.IsReadOnly
        {
            get { return ((IDictionary<eYoonRobotType, RemoteSection>)m_pDicSection).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<eYoonRobotType, RemoteSection>>.Remove(KeyValuePair<eYoonRobotType, RemoteSection> pCollection)
        {
            var ret = ((IDictionary<eYoonRobotType, RemoteSection>)m_pDicSection).Remove(pCollection);
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

        public IEnumerator<KeyValuePair<eYoonRobotType, RemoteSection>> GetEnumerator()
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

        private IEnumerator<KeyValuePair<eYoonRobotType, RemoteSection>> GetOrderedEnumerator()
        {
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                yield return new KeyValuePair<eYoonRobotType, RemoteSection>(m_pListKeyOrdered[i], m_pDicSection[m_pListKeyOrdered[i]]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator RemoteContainer(Dictionary<eYoonRobotType, RemoteSection> pDic)
        {
            return new RemoteContainer(pDic);
        }

        public static explicit operator Dictionary<eYoonRobotType, RemoteSection>(RemoteContainer pContainer)
        {
            return pContainer.m_pDicSection;
        }
    }
}
