using System;
using System.IO;
using Newtonsoft.Json;

namespace YoonFactory.Files
{
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
                m_pDocument = null;
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

        private object m_pDocument;
        private Type m_pTypeDocument;

        public string FilePath { get; private set; }

        public object Document
        {
            get => m_pDocument;
            set
            {
                m_pTypeDocument = Document.GetType();
                m_pDocument = value;
            }
        }

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

        public bool LoadFile()
        {
            if (m_pDocument == null || m_pTypeDocument == null) throw new NullReferenceException("Document has Null Reference");
            return LoadFile(out m_pDocument, m_pTypeDocument);
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

        public bool SaveFile()
        {
            if (m_pDocument == null || m_pTypeDocument == null) throw new NullReferenceException("Document has Null Reference");
            return SaveFile(m_pDocument, m_pTypeDocument);
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