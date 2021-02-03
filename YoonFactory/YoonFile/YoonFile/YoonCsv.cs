using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YoonFactory.Files
{
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

}
