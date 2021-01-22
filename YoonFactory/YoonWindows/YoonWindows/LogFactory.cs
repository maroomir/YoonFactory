// LOG-FACTORY :  CJYOON @ 2017
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using YoonFactory.Log;
using YoonFactory.Files;

namespace YoonFactory.Windows.Log
{
    /*  Log 기록시 사용하는 Enum */
    public class LogWindowsArgs : LogArgs
    {
        public Color BackColor;

        public LogWindowsArgs(Color pColor, string strMessage)
        {
            BackColor = pColor;
            Message = strMessage;
        }
    }

    /*  Log -> TextBox 기록 전용 Class */
    public class YoonDisplayer : IYoonLog, IDisposable
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
                if (Repository != null)
                    Repository.Clear();
                Repository = null;
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

        public string RootDirectory { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "YoonFactory", "DLMLog");
        public LogContainer Repository { get; private set; } = new LogContainer();
        public event LogProcessCallback OnProcessLogEvent;
        private int m_nDirectoryFileExistDays = 1; // 0일 경우 충돌 발생

        private static object m_logLock = new object();
        private const int MAX_DAY_SPAN = 30;

        public YoonDisplayer()
        {
            //
        }
        public YoonDisplayer(int nDays)
        {
            if (nDays > MAX_DAY_SPAN)
                m_nDirectoryFileExistDays = MAX_DAY_SPAN;
            else if (nDays < 0)
                m_nDirectoryFileExistDays = 0;
            else
                m_nDirectoryFileExistDays = nDays;
        }

        public YoonDisplayer(string strPath, int nDays)
        {
            RootDirectory = strPath;
            if (nDays > MAX_DAY_SPAN)
                m_nDirectoryFileExistDays = MAX_DAY_SPAN;
            else if (nDays < 0)
                m_nDirectoryFileExistDays = 0;
            else
                m_nDirectoryFileExistDays = nDays;
        }

        public void Write(string strMessage, bool isSave = true)
        {
            Write(eYoonStatus.Info, strMessage, isSave);
        }

        /// <summary>
        /// 일반 Form에서 Log를 기록할 때 사용되어지는 Sequence 다.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="strMessage"></param>
        public void Write(eYoonStatus index, string strMessage, bool isSave = true)
        {
            string strLine = DisplayMessage(index, strMessage);
#if DEBUG
            if (RootDirectory != string.Empty && isSave)
                WriteDisplayLog(strLine);
#endif
        }

        public void Clear()
        {
            if (Repository != null)
                Repository.Clear();
        }


        /// <summary>
        /// Log Message를 화면에 보여준다.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="strMessage"></param>
        private string DisplayMessage(eYoonStatus index, string strMessage)
        {
            lock (m_logLock)
            {
                DateTime pNow = DateTime.Now;
                string nowTime = string.Format("{0:yyyy-MM-dd HH:mm:ss:fff}", pNow);   //현재 시간 가져오기
                string strMessageLine = "";
                if (Repository != null)
                    Repository[pNow] = strMessage;
                switch (index)
                {
                    ////  일반 Process Message 출력
                    case eYoonStatus.Normal:
                        strMessageLine = "[" + nowTime + "] " + strMessage + "\n";
                        OnProcessLogEvent(this, new LogWindowsArgs(Color.White, strMessageLine));
                        break;
                    ////  기타 확인 : 오류 메세지 설명은 별도로 저장
                    case eYoonStatus.Conform:
                        strMessageLine = "[" + nowTime + "]  CONFIRM : " + strMessage + "\n";
                        OnProcessLogEvent(this, new LogWindowsArgs(Color.Khaki, strMessageLine));
                        break;
                    ////  전송 확인 : 전송 메시지 출력
                    case eYoonStatus.Send:
                        strMessageLine = "[" + nowTime + "]  SEND : " + strMessage + "\n";
                        OnProcessLogEvent(this, new LogWindowsArgs(Color.Lavender, strMessageLine));
                        break;
                    ////  수신 확인 : 수신 메시지 출력
                    case eYoonStatus.Receive:
                        strMessageLine = "[" + nowTime + "]  RECEIVE : " + strMessage + "\n";
                        OnProcessLogEvent(this, new LogWindowsArgs(Color.Lavender, strMessageLine));
                        break;
                    ////  사용자 확인 : 사용자 조작시 발생 메시지 출력
                    case eYoonStatus.User:
                        strMessageLine = "[" + nowTime + "]  USER : " + strMessage + "\n";
                        OnProcessLogEvent(this, new LogWindowsArgs(Color.Lime, strMessageLine));
                        break;
                    ////  Insepct 확인 :  Inspection 시 발생 메시지 출력
                    case eYoonStatus.Inspect:
                        strMessageLine = "[" + nowTime + "]  INSPECT : " + strMessage + "\n";
                        OnProcessLogEvent(this, new LogWindowsArgs(Color.Lime, strMessageLine));
                        break;
                    ////  오류 확인 :  오류 메시지 출력
                    case eYoonStatus.Error:
                        strMessageLine = "[" + nowTime + "]  ERROR : " + strMessage + "\n";
                        OnProcessLogEvent(this, new LogWindowsArgs(Color.Salmon, strMessageLine));
                        break;
                    ////  Info 확인 :  상황 확인 메시지 출력
                    case eYoonStatus.Info:
                        strMessageLine = "[" + nowTime + "]  INFO : " + strMessage + "\n";
                        OnProcessLogEvent(this, new LogWindowsArgs(Color.Khaki, strMessageLine));
                        break;
                }
                return strMessageLine;
            }
        }


        /// <summary>
        ///  Console 창에 적혀진 Log를 .txt로 저장하는 Sequence 다.
        /// </summary>
        /// <param name="data"></param>
        private void WriteDisplayLog(string data)
        {
            Task.Factory.StartNew(new System.Action(() =>
            {
                DeleteDirectoryFile();
            }));

            string strPath = CreateDirectoryFile();
            if (!FileFactory.VerifyFileExtension(ref strPath, ".txt"))
            {
                Console.WriteLine("LogManagement WriteDisplayLog: path fail");
                return;
            }

            StreamWriter sw;
            try
            {
                sw = new StreamWriter(strPath, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("LogManagement WriteDisplayLog Exception: {0}", ex.ToString());
                return;
            }

            try
            {
                sw.WriteLine(data);
                sw.Close();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("LogManagement WriteDisplayLog Exception: {0}", ex.ToString());
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
                Console.WriteLine("LogManagement CreateDirectoryFile Exception: {0}", ex.ToString());
            }
            return strDirPath;
        }

        /// <summary>
        /// 삭제될 Directory를 확인한다.
        /// </summary>
        private void DeleteDirectoryFile()
        {
            try
            {
                DateTime pNow = DateTime.Now;
                string strMonthPath = Path.Combine(RootDirectory, pNow.Year.ToString(), pNow.Month.ToString());
                FileFactory.DeleteOldFilesInDirectory(strMonthPath, m_nDirectoryFileExistDays);
                DateTime pPreMonth = pNow.AddMonths(-1); // Pre-Month
                strMonthPath = Path.Combine(RootDirectory, pPreMonth.Year.ToString(), pPreMonth.Month.ToString());
                FileFactory.DeleteOldFilesInDirectory(strMonthPath, m_nDirectoryFileExistDays);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("LogManagement DeleteDirectoryFile Exception: {0}", ex.ToString());
            }
        }
    }
}
