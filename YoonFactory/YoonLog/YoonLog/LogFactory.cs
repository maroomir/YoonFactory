﻿// LOG-FACTORY :  CJYOON @ 2017
using System;
using System.Threading.Tasks;
using System.IO;
using YoonFactory.Files;

namespace YoonFactory.Log
{
    public interface IYoonLog
    {
        string RootDirectory { get; set; }
        void SetDirectoryFileExistDays(int nDays);
        void Write(string strMessage, bool isSave);
    }

    /*  Log -> Console 기록 전용 Class */
    public class YoonScribe : IYoonLog, IDisposable
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
        // ~TCPComm() {
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

        public string RootDirectory { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory", "CLMLog");
        private int m_nDirectoryFileExistDays = 1; // 0일 경우 충돌 발생
        private const int MAX_DAY_SPAN = 30;

        public YoonScribe()
        {
            //
        }

        public YoonScribe(int nDays)
        {
            if (nDays > MAX_DAY_SPAN)
                m_nDirectoryFileExistDays = MAX_DAY_SPAN;
            else
                m_nDirectoryFileExistDays = nDays;
        }

        public YoonScribe(string strPath, int nDays)
        {
            RootDirectory = strPath;
            if (nDays > MAX_DAY_SPAN)
                m_nDirectoryFileExistDays = MAX_DAY_SPAN;
            else
                m_nDirectoryFileExistDays = nDays;
        }

        /// <summary>
        /// Log Data 저장 파일의 유지 기간을 지정한다.
        /// </summary>
        /// <param name="nDays"></param>
        public void SetDirectoryFileExistDays(int nDays)
        {
            if (nDays > MAX_DAY_SPAN)
                m_nDirectoryFileExistDays = MAX_DAY_SPAN;
            else
                m_nDirectoryFileExistDays = nDays;
        }

        /// <summary>
        /// Console 창에 Log를 작성하는 Sequence 다.
        /// </summary>
        /// <param name="strMessage"> Message </param>
        public void Write(string strMessage, bool isSave = true)
        {
            string nowTime = string.Format("{0:yyyy-MM-dd HH:mm:ss:fff}", DateTime.Now);
            string strLine = "[" + nowTime + "] " + strMessage;
            Console.WriteLine(strLine);
#if DEBUG
            if (RootDirectory != string.Empty && RootDirectory != null && isSave == true)
                WriteConsoleLog(strLine);
#endif
        }

        /// <summary>
        ///  Console 창에 적혀진 Log를 .txt로 저장하는 Sequence 다.
        /// </summary>
        /// <param name="data"></param>
        private void WriteConsoleLog(string data)
        {
            Task.Factory.StartNew(new Action(() =>
            {
                DeleteDirectoryFileCheck();
            }));

            string strPath = CreateDirectoryFile();
            if (!FileFactory.VerifyFileExtension(ref strPath, ".txt"))
            {
                Console.WriteLine("LogManagement WriteConsoleLog: path fail");
                return;
            }

            StreamWriter sw;
            try
            {
                sw = new StreamWriter(strPath, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("LogManagement WriteConsoleLog Exception: {0}", ex.ToString());
                return;
            }

            try
            {
                sw.WriteLine(data);
                sw.Close();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("LogManagement WriteConsoleLog Exception: {0}", ex.ToString());
                sw.Close();
            }
        }

        /// <summary>
        /// Log Directory 를 확인한다.
        /// </summary>
        /// <returns></returns>
        private string CreateDirectoryFile()
        {
            string strDirPath = string.Empty;
            DateTime now = DateTime.Now;

            try
            {
                strDirPath = Path.Combine(RootDirectory, now.Year.ToString(), now.Month.ToString(), now.Day.ToString(),
                                          now.Hour.ToString() + ".txt");

                FileFactory.VerifyFilePath(strDirPath);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("LogManagement CreateDirectoryFileConsole Exception: {0}", ex.ToString());
            }
            return strDirPath;
        }

        /// <summary>
        /// 삭제될 Directory를 확인한다.
        /// </summary>
        private void DeleteDirectoryFileCheck()
        {
            try
            {
                string strMonthPath;
                DateTime now = DateTime.Now;
                strMonthPath = Path.Combine(RootDirectory, now.Year.ToString(), now.Month.ToString());
                FileFactory.DeleteOldFilesInDirectory(strMonthPath, m_nDirectoryFileExistDays);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("LogManagement DeleteDirectoryFile Exception: {0}", ex.ToString());
            }
        }
    }
}