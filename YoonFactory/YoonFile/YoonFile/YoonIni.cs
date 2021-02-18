using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using YoonFactory.Files.Ini;

namespace YoonFactory.Files
{
    /* Ini 파일을 관리하는 Class - 공용, @2020 */
    public class YoonIni : IYoonFile, IEnumerable<KeyValuePair<string, IniSection>>, IDictionary<string, IniSection>
    {

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //
                }
                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                if (m_dicSections != null)
                    m_dicSections.Clear();
                m_dicSections = null;
                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~DatabaseControl() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion


        public static IEqualityComparer<string> DefaultComparer = new CaseInsensitiveStringComparer();

        class CaseInsensitiveStringComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return String.Compare(x, y, true) == 0;
            }

            public int GetHashCode(string obj)
            {
                return obj.ToLowerInvariant().GetHashCode();
            }
#if JS
        public new bool Equals(object x, object y) {
            var xs = x as string;
            var ys = y as string;
            if (xs == null || ys == null) {
                return xs == null && ys == null;
            }
            return Equals(xs, ys);
        }

        public int GetHashCode(object obj) {
            if (obj is string) {
                return GetHashCode((string)obj);
            }
            return obj.ToStringInvariant().ToLowerInvariant().GetHashCode();
        }
#endif
        }

        private Dictionary<string, IniSection> m_dicSections;
        public IEqualityComparer<string> StrComparer;
        public string FilePath { get; private set; }

        public bool SaveEmptySections;

        public YoonIni(string strPath) : this(strPath, DefaultComparer)
        {
            //
        }

        public YoonIni(string strPath, IEqualityComparer<string> stringComparer)
        {

            if (FileFactory.VerifyFileExtension(ref strPath, ".ini", true, true))
                FilePath = strPath;
            else
                FilePath = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory", "YoonFactory.ini");
            StrComparer = stringComparer;
            m_dicSections = new Dictionary<string, IniSection>(StrComparer);
        }

        public void CopyFrom(IYoonFile pFile)
        {
            if (pFile is YoonIni pIni)
            {
                FilePath = pIni.FilePath;
                StrComparer = pIni.StrComparer;
                m_dicSections = new Dictionary<string, IniSection>(StrComparer);
            }
        }

        public IYoonFile Clone()
        {
            return new YoonIni(FilePath, StrComparer);
        }

        public bool IsFileExist()
        {
            return FileFactory.VerifyFilePath(FilePath, false);
        }

        public bool LoadFile()
        {
            string strPath = FilePath;
            if (!FileFactory.VerifyFileExtension(ref strPath, ".ini"))
                return false;

            using (FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    IniSection section = null;

                    while (!sr.EndOfStream)
                    {
                        string strLine = sr.ReadLine();

                        if (strLine != null)
                        {
                            string strTrimStart = strLine.TrimStart();

                            if (strTrimStart.Length > 0)
                            {
                                if (strTrimStart[0] == '[')
                                {
                                    int nSectionEnd = strTrimStart.IndexOf(']');
                                    if (nSectionEnd > 0)
                                    {
                                        string strSectionName = strTrimStart.Substring(1, nSectionEnd - 1).Trim();
                                        section = new IniSection(StrComparer);
                                        m_dicSections[strSectionName] = section;
                                    }
                                }
                                else if (section != null && strTrimStart[0] != ';')
                                {
                                    string key;
                                    IniValue value;

                                    if (GetValue(strLine, out key, out value))
                                    {
                                        section[key] = value;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool SaveFile()
        {
            return SaveFile(FileMode.Create);
        }

        public bool SaveFile(FileMode fm)
        {
            string strPath = FilePath;
            if (!FileFactory.VerifyFileExtension(ref strPath, ".ini", true, true))
                return false;

            using (FileStream fs = new FileStream(strPath, fm, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (var section in m_dicSections)
                    {
                        if (section.Value.Count > 0 || SaveEmptySections)
                        {
                            writer.WriteLine(string.Format("[{0}]", section.Key.Trim()));
                            foreach (var kvp in section.Value)
                            {
                                writer.WriteLine(string.Format("{0}={1}", kvp.Key, kvp.Value));
                            }
                            writer.WriteLine("");
                        }
                    }
                }
            }
            return true;
        }

        public bool SaveFile(Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                foreach (var section in m_dicSections)
                {
                    if (section.Value.Count > 0 || SaveEmptySections)
                    {
                        writer.WriteLine(string.Format("[{0}]", section.Key.Trim()));
                        foreach (var kvp in section.Value)
                        {
                            writer.WriteLine(string.Format("{0}={1}", kvp.Key, kvp.Value));
                        }
                        writer.WriteLine("");
                    }
                }
            }
            return true;
        }

        private bool GetValue(string line, out string key, out IniValue value)
        {
            int nAssignIndex = line.IndexOf('=');
            if (nAssignIndex <= 0)
            {
                key = null;
                value = null;
                return false;
            }

            key = line.Substring(0, nAssignIndex).Trim();
            string strValue = line.Substring(nAssignIndex + 1);

            value = new IniValue(strValue);
            return true;
        }

        public bool ContainsSection(string section)
        {
            return m_dicSections.ContainsKey(section);
        }

        public bool TryGetSection(string section, out IniSection result)
        {
            return m_dicSections.TryGetValue(section, out result);
        }

        bool IDictionary<string, IniSection>.TryGetValue(string key, out IniSection value)
        {
            return TryGetSection(key, out value);
        }

        public bool Remove(string section)
        {
            return m_dicSections.Remove(section);
        }

        public IniSection Add(string section, Dictionary<string, IniValue> values)
        {
            return Add(section, new IniSection(values, StrComparer));
        }

        public IniSection Add(string section, IniSection value)
        {
            if (value.Comparer != StrComparer)
            {
                value = new IniSection(value, StrComparer);
            }
            m_dicSections.Add(section, value);
            return value;
        }

        public IniSection Add(string section)
        {
            IniSection value = new IniSection(StrComparer);
            m_dicSections.Add(section, value);
            return value;
        }

        void IDictionary<string, IniSection>.Add(string key, IniSection value)
        {
            Add(key, value);
        }

        bool IDictionary<string, IniSection>.ContainsKey(string key)
        {
            return ContainsSection(key);
        }

        public ICollection<string> Keys
        {
            get { return m_dicSections.Keys; }
        }

        public ICollection<IniSection> Values
        {
            get { return m_dicSections.Values; }
        }

        void ICollection<KeyValuePair<string, IniSection>>.Add(KeyValuePair<string, IniSection> item)
        {
            ((IDictionary<string, IniSection>)m_dicSections).Add(item);
        }

        public void Clear()
        {
            m_dicSections.Clear();
        }

        bool ICollection<KeyValuePair<string, IniSection>>.Contains(KeyValuePair<string, IniSection> item)
        {
            return ((IDictionary<string, IniSection>)m_dicSections).Contains(item);
        }

        void ICollection<KeyValuePair<string, IniSection>>.CopyTo(KeyValuePair<string, IniSection>[] array, int arrayIndex)
        {
            ((IDictionary<string, IniSection>)m_dicSections).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return m_dicSections.Count; }
        }

        bool ICollection<KeyValuePair<string, IniSection>>.IsReadOnly
        {
            get { return ((IDictionary<string, IniSection>)m_dicSections).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<string, IniSection>>.Remove(KeyValuePair<string, IniSection> item)
        {
            return ((IDictionary<string, IniSection>)m_dicSections).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, IniSection>> GetEnumerator()
        {
            return m_dicSections.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IniSection this[string section]
        {
            get
            {
                IniSection s;
                if (m_dicSections.TryGetValue(section, out s))
                {
                    return s;
                }
                s = new IniSection(StrComparer);
                m_dicSections[section] = s;
                return s;
            }
            set
            {
                var v = value;
                if (v.Comparer != StrComparer)
                {
                    v = new IniSection(v, StrComparer);
                }
                m_dicSections[section] = v;
            }
        }

        public string GetContents()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                SaveFile(stream);
                stream.Flush();
                var builder = new StringBuilder(Encoding.UTF8.GetString(stream.ToArray()));
                return builder.ToString();
            }
        }
    }

}
