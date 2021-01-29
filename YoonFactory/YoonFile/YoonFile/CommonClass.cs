// FILE-FACTORY :  CJYOON @2015 ~ 2020

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using Newtonsoft.Json;

using YoonFactory.Files.Ini;

namespace YoonFactory.Files
{
    /* 환경 파일을 관리하는 Class */
    public static class EnvironmentFactory
    {
        public enum eTypeEnvironment
        {
            Windows,
            MacOS,
            Ubuntu,
        }

        public static string GetHomeFolder(eTypeEnvironment fOS)
        {
            switch(fOS)
            {
                case eTypeEnvironment.Windows:
                    return Path.Combine(Environment.GetEnvironmentVariable("HOMEPATH"));
                case eTypeEnvironment.MacOS:
                    return Path.Combine(Environment.GetEnvironmentVariable("HOME"));
                case eTypeEnvironment.Ubuntu:
                    return Path.Combine(Environment.GetEnvironmentVariable("HOME"));
                default:
                    break;
            }
            return string.Empty;
        }

        public static string GetRootFolder(eTypeEnvironment fOS)
        {
            switch(fOS)
            {
                case eTypeEnvironment.Windows:
                    return Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"));
                case eTypeEnvironment.MacOS:
                    return Path.Combine("/Library");
                case eTypeEnvironment.Ubuntu:
                    return Path.Combine("usr", "share");
                default:
                    break;
            }
            return string.Empty;
        }

        public static string GetFontFolder(eTypeEnvironment fOS)
        {
            switch(fOS)
            {
                case eTypeEnvironment.Windows:
                    return Path.Combine(GetRootFolder(fOS), "Fonts");
                case eTypeEnvironment.MacOS:
                    return Path.Combine(GetRootFolder(fOS), "Fonts");
                case eTypeEnvironment.Ubuntu:
                    return Path.Combine(GetRootFolder(fOS), "Fonts");
            }
            return string.Empty;
        }
    }

    /* 일반 파일을 관리하는 Class */
    public static class FileFactory
    {
        public static bool VerifyDirectory(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    if (Directory.Exists(path))
                        return true;
                }
                else
                    return true;
            }
            return false;
        }

        public static bool VerifyFilePath(string path, bool bCreateFile = true)
        {
            if (!string.IsNullOrEmpty(path))
            {
                FileInfo fi = new FileInfo(path);
                if (!VerifyDirectory(fi.DirectoryName))
                    return false;

                try
                {
                    if (!fi.Exists)
                    {
                        if (!bCreateFile) return false;
                        FileStream fs = fi.Create();
                        fs.Close();
                        return true;
                    }
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return false;
        }

        /// <summary>
        /// VerifyFileExtension
        /// </summary>
        /// <param name="path"> File Path </param>
        /// <param name="strExt"> Extension string as like ".Ext" </param>
        /// <param name="bChangeExtension"> To change the extension as strExt </param>
        /// <returns></returns>
        public static bool VerifyFileExtension(ref string path, string strExt, bool bChangeExtension = false, bool bCreateFile = false)
        {
            if (VerifyFilePath(path, bCreateFile))
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Extension != strExt)
                {
                    if (!bChangeExtension) return false;
                    string strFilePath = Path.Combine(fi.DirectoryName, Path.GetFileNameWithoutExtension(path) + strExt);
                    if (!VerifyFilePath(strFilePath, bCreateFile)) return false;
                    path = strFilePath;
                    return true;
                }
                else
                    return true;
            }
            return false;
        }

        public static bool IsFileExist(string filename)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(filename);
            return fi.Exists;
        }

        public static List<string> GetFileListInDir(string rootPath, List<string> fListFile)
        {
            if (fListFile == null)
                return null;
            FileAttributes fAttribute = File.GetAttributes(rootPath);
            //// If rootpath is Directory
            if ((fAttribute & FileAttributes.Directory) == FileAttributes.Directory)
            {
                DirectoryInfo di = new DirectoryInfo(rootPath);
                //// continues abtract subdirectory
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    GetFileListInDir(dir.FullName, fListFile);
                }
                //// abstract subfiles
                foreach (FileInfo fir in di.GetFiles())
                {
                    GetFileListInDir(fir.FullName, fListFile);
                }
            }
            else
            {
                FileInfo fi = new FileInfo(rootPath);
                fListFile.Add(fi.FullName);
            }
            return fListFile;
        }

        public static string GetTextFromFile(string path)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Open);
            }
            catch
            {
                Console.WriteLine(string.Format("Error opening file ({0}) !", path));
                return string.Empty;
            }

            string strComplete = string.Empty;
            using (StreamReader sr = new StreamReader(fs))
            {
                string strLine;
                while ((strLine = sr.ReadLine()) != null)
                    strComplete += strLine;
            }
            return strComplete;
        }

        public static List<string> GetTextLinesFromFile(string path)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Open);
            }
            catch
            {
                Console.WriteLine(string.Format("Error opening file ({0}) !", path));
                return null;
            }

            List<string> pListTextComplete = new List<string>();
            using (StreamReader sr = new StreamReader(fs))
            {
                string strLine;
                while ((strLine = sr.ReadLine()) != null)
                    pListTextComplete.Add(strLine);
            }
            return pListTextComplete;
        }

        public static bool AppendTextToFile(string path, string data)
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(path, FileMode.Append);
            }
            catch
            {
                Console.WriteLine(string.Format("Error opening file ({0}) !", path));
                return false;
            }

            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(data);
            sw.Close();
            return true;
        }

        public static bool DeleteFilePath(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                FileInfo fi = new FileInfo(filePath);
                if (!VerifyDirectory(fi.DirectoryName))
                    return false;

                try
                {
                    if (fi.Exists)
                    {
                        fi.Delete();
                        return true;
                    }
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return false;
        }

        public static void DeleteExtensionFilesInDirectory(string dirPath, string strExt)
        {
            if (VerifyDirectory(dirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                //// File Clear in DirPath
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    if (file.Extension == strExt)
                        file.Delete();
                }
                //// Directory clear in DirPath
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                    DeleteExtensionFilesInDirectory(dir.FullName, strExt);
            }
        }

        public static void DeleteIncludeSpecificInDirectory(string dirPath, string strSpecific, bool bCheckFileNameOnly = false)
        {
            if (VerifyDirectory(dirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                //// File Clear in DirPath
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    if (bCheckFileNameOnly)
                    {
                        string strFileName = file.Name + "." + file.Extension;
                        if (strFileName.Contains(strSpecific))
                            file.Delete();
                    }
                    else
                    {
                        if (file.FullName.Contains(strSpecific))
                            file.Delete();
                    }
                }
                //// Directory clear in DirPath
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                    DeleteExtensionFilesInDirectory(dir.FullName, strSpecific);
            }
        }

        public static void DeleteOldFilesInDirectory(string dirPath, int nDateSpan)
        {
            DeleteOldFilesInDirectory(dirPath, DateTime.Now, nDateSpan);
        }

        public static void DeleteOldFilesInDirectory(string dirPath, DateTime pDateStart, int nDateSpan)
        {
            if (VerifyDirectory(dirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                //// File Clear in DirPath
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    TimeSpan pSpan = pDateStart - file.CreationTime;

                    if ((int)pSpan.TotalDays >= nDateSpan)
                        file.Delete();
                }
                //// Directory clear in DirPath
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                    DeleteOldFilesInDirectory(dir.FullName, pDateStart, nDateSpan);
            }
        }

        public static void DeleteAllFilesInDirectory(string dirPath)
        {
            if (VerifyDirectory(dirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                //// File Clear in DirPath
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
                //// Directory clear in DirPath
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                {
                    DeleteAllFilesInDirectory(dirPath);
                }
            }
        }
    }

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

        public bool SaveFile(FileMode fm = FileMode.Create)
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

    /* Csv 파일을 관리하는 Class At Dictionary<int, String> */
    public class YoonCsv : IYoonFile
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

        public string FilePath { get; private set; }

        // Csv File 위치를 인자로 받는다.
        public YoonCsv(string path)
        {
            if (FileFactory.VerifyFileExtension(ref path, ".csv", true, true))
                FilePath = path;
            else
                FilePath = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory", "YoonFactory.csv");
        }

        public void CopyFrom(IYoonFile pFile)
        {
            if (pFile is YoonCsv pCsv)
            {
                FilePath = pCsv.FilePath;
            }
        }

        public IYoonFile Clone()
        {
            return new YoonCsv(FilePath);
        }

        public bool IsFileExist()
        {
            return FileFactory.VerifyFilePath(FilePath, false);
        }

        // Csv의 Data를 읽어온다.
        public List<string[]> GetBlock()
        {
            List<string[]> pListResult = new List<string[]>();
            StreamReader reader = new StreamReader(FilePath);
            while (!reader.EndOfStream)
            {
                string strMessage = reader.ReadLine();
                string[] pArrayValue = strMessage.Split(',');
                pListResult.Add(pArrayValue);
            }
            reader.Close();
            return pListResult;
        }

        // Csv의 Data를 설정한다.
        public bool SetBlock(List<string[]> pListValue)
        {
            FileStream fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fileStream, Encoding.Default);
            string[] pArrayValue = null;
            string strData = "";
            bool bResult = true;
            try
            {
                for (int iRow = 0; iRow < pListValue.Count; iRow++)
                {
                    pArrayValue = pListValue[iRow];
                    strData = "";
                    for (int iCol = 0; iCol < pArrayValue.Length; iCol++)
                    {
                        strData += pArrayValue[iCol];
                        if (iCol < pArrayValue.Length - 1)
                        {
                            strData += ",";
                        }
                    }
                    writer.WriteLine(strData);
                }
            }
            catch
            {
                bResult = false;
            }
            writer.Close();
            return bResult;
        }

        // Csv의 Data를 1개 Line만 저장한다.
        public bool SetLine(string[] pArrayValue)
        {
            FileStream fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fileStream, Encoding.Default);
            string strData = "";
            bool bResult = true;
            try
            {
                for (int iCol = 0; iCol < pArrayValue.Length; iCol++)
                {
                    strData += pArrayValue[iCol];
                    if (iCol < pArrayValue.Length - 1)
                    {
                        strData += ",";
                    }
                }
                writer.WriteLine(strData);
            }
            catch
            {
                bResult = false;
            }
            writer.Close();
            return bResult;
        }

        // Csv의 Data를 1개 Line만 저장한다.
        public bool SetLine(string strLine)
        {
            FileStream fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fileStream, Encoding.Default);
            if (strLine.Split(',').Length == 0) return false;

            bool bResult = true;
            try
            {
                writer.WriteLine(strLine);
            }
            catch
            {
                bResult = false;
            }
            writer.Close();
            return bResult;
        }
    }

    /* XML Control Class to Class @2020*/
    public class YoonXml : IYoonFile
    {
        public string FilePath { get; private set; }

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

        public YoonXml(string strPath)
        {
            FilePath = strPath;
        }

        public void CopyFrom(IYoonFile pFile)
        {
            if (pFile is YoonXml pXml)
            {
                FilePath = pXml.FilePath;
            }
        }

        public IYoonFile Clone()
        {
            return new YoonXml(FilePath);
        }

        public bool IsFileExist()
        {
            return FileFactory.VerifyFilePath(FilePath, false);
        }

        // Load XML File
        public bool LoadFile(out object pParamData, Type pType)
        {
            StreamReader sr;
            XmlSerializer xl;

            string strPath = FilePath;
            if (!FileFactory.VerifyFileExtension(ref strPath, ".xml"))
            {
                Console.WriteLine("Load XML File Failure : " + FilePath + "\n");
                pParamData = null;
                return false;
            }
            try
            {
                sr = new StreamReader(strPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadXML_Critical : " + ex.ToString() + "\n");
                pParamData = null;
                return false;
            }
            try
            {
                xl = new XmlSerializer(pType);
                pParamData = xl.Deserialize(sr);
                sr.Close();  // IOException : Sharing violation on path 오류 수정
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadXML_Critical : " + ex.ToString() + "\n");
                pParamData = null;
                sr.Close();
                return false;
            }
            return true;
        }

        public bool SaveFile(object pParamData, Type pType)
        {
            StreamWriter sw;
            XmlSerializer xs;

            string strPath = FilePath;
            if (!FileFactory.VerifyFileExtension(ref strPath, ".xml", false, true))
            {
                Console.WriteLine("Create XML File Failure : " + FilePath + "\n");
                return false;
            }

            try
            {
                sw = new StreamWriter(strPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveXML_Critical : " + ex.ToString() + "\n");
                return false;
            }
            try
            {
                xs = new XmlSerializer(pType);
                xs.Serialize(sw, pParamData);
                sw.Close(); // IOException : Sharing violation on path 오류 수정
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveXML_Critical : " + ex.ToString() + "\n");
                sw.Close();
                return false;
            }
            
            return true;
        }
    }

    /* ZIP File Management Class @2020 */
    public class YoonZip
    {
        private string m_zipFilePath;
        public YoonZip(string strPath)
        {
            if (FileFactory.VerifyFileExtension(ref strPath, ".zip"))
                m_zipFilePath = strPath;
            else
                m_zipFilePath = "YoonZip.zip";
        }

        public string DirectoryPath { get; set; }

        public string ZipFilePath
        {
            get
            {
                return m_zipFilePath;
            }
            set
            {
                if (FileFactory.VerifyFileExtension(ref value, ".zip"))
                    m_zipFilePath = value;
            }
        }

        public void CompressZip()
        {
            if (!FileFactory.VerifyDirectory(DirectoryPath))
                return;

            List<string> fListFile = FileFactory.GetFileListInDir(DirectoryPath, new List<String>());
            using (FileStream fs = new FileStream(ZipFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                using (ZipArchive zipArchive = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    foreach (string str in fListFile)
                    {
                        string path = str.Substring(DirectoryPath.Length + 1);
                        zipArchive.CreateEntryFromFile(str, path);
                    }
                }
            }
        }

        public void CompressZip(string strDirPath)
        {
            if (!FileFactory.VerifyDirectory(strDirPath))
                return;

            List<string> fListFile = FileFactory.GetFileListInDir(strDirPath, new List<String>());
            using (FileStream fs = new FileStream(ZipFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                using (ZipArchive za = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    foreach (string str in fListFile)
                    {
                        string path = str.Substring(strDirPath.Length + 1);
                        za.CreateEntryFromFile(str, path);
                    }
                }
            }
        }

        public void ExtractZip()
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            using (ZipArchive za = System.IO.Compression.ZipFile.OpenRead(ZipFilePath))
            {
                foreach (ZipArchiveEntry zaentry in za.Entries)
                {
                    string strFilePath = Path.Combine(DirectoryPath, zaentry.FullName);
                    string strSubDir = Path.GetDirectoryName(strFilePath);
                    if (!Directory.Exists(strSubDir))
                    {
                        Directory.CreateDirectory(strSubDir);
                    }
                    zaentry.ExtractToFile(strFilePath);
                }
            }
        }

        public void ExtractZip(string strDirPath)
        {
            if (!Directory.Exists(strDirPath))
            {
                Directory.CreateDirectory(strDirPath);
            }
            using (ZipArchive za = System.IO.Compression.ZipFile.OpenRead(ZipFilePath))
            {
                foreach (ZipArchiveEntry zaentry in za.Entries)
                {
                    string strFilePath = Path.Combine(strDirPath, zaentry.FullName);
                    string strSubDir = Path.GetDirectoryName(strFilePath);
                    if (!Directory.Exists(strSubDir))
                    {
                        Directory.CreateDirectory(strSubDir);
                    }
                    zaentry.ExtractToFile(strFilePath);
                }
            }
        }
    }

    /* JSON File Control Class - NetFramework Version @2020 */
    public class YoonJson : IYoonFile
    {
        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                }
                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
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

        public string FilePath { get; private set; }

        public YoonJson(string strPath)
        {
            if (!FileFactory.VerifyFileExtension(ref strPath, ".json", false, false))
                FilePath = strPath;
            else
                FilePath = Path.Combine(Directory.GetCurrentDirectory(), "YoonJSON.json");
        }

        public void CopyFrom(IYoonFile pFile)
        {
            if (pFile is YoonJson pJson)
            {
                FilePath = pJson.FilePath;
            }
        }

        public IYoonFile Clone()
        {
            return new YoonJson(FilePath);
        }

        public bool IsFileExist()
        {
            return FileFactory.IsFileExist(FilePath);
        }

        public bool LoadFile(out object pParamData, Type pType)
        {
            string strPath = FilePath;
            if (!FileFactory.VerifyFileExtension(ref strPath, ".xml"))
            {
                Console.WriteLine("Load Json File Failure : " + FilePath + "\n");
                pParamData = null;
                return false;
            }

            StreamReader sr;
            JsonSerializer js;

            try
            {
                sr = new StreamReader(strPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadJson_Critical : " + ex.ToString() + "\n");
                pParamData = null;
                return false;
            }
            try
            {
                js = new JsonSerializer();
                pParamData = js.Deserialize(sr, pType);
                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadJson_Critical : " + ex.ToString() + "\n");
                pParamData = null;
                sr.Close();
                return false;
            }
            return true;
        }

        public bool SaveFile(object pParamData, Type pType)
        {
            string strPath = FilePath;
            if (!FileFactory.VerifyFileExtension(ref strPath, ".xml", false, true))
            {
                Console.WriteLine("Create XML File Failure : " + FilePath + "\n");
                return false;
            }

            StreamWriter sw;
            JsonSerializer xs;

            try
            {
                sw = new StreamWriter(strPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveJson_Critical : " + ex.ToString() + "\n");
                return false;
            }
            try
            {
                xs = new JsonSerializer();
                xs.Serialize(sw, pParamData);
                sw.Close(); // IOException : Sharing violation on path 오류 수정
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveJson_Critical : " + ex.ToString() + "\n");
                sw.Close();
                return false;
            }
            return true;
        }
    }
}