// LOG-FACTORY :  CJYOON @ 2020
using System;
using System.Collections.Generic;
using Gtk;
using Pango;
using YoonFactory.Log;

namespace YoonFactory.Mono.Log
{
    /*  Log 기록시 사용하는 Enum */
    public class LogArgs : EventArgs
    {
        public Gdk.Color BackColor;
        public string Message;
        public bool IsStackClear;
        
        public LogArgs(Gdk.Color pColor, string strMessage, bool bStackClear)
        {
            BackColor = pColor;
            Message = strMessage;
            IsStackClear = bStackClear;
        }
    }

    /// <summary>
    /// Message 외부 전달시 사용하는 Callback 함수
    /// </summary>
    /// <param name="strMessage">Display로 반환하는 Evnet</param>
    public delegate void LogDisplayCallback(object sender, LogArgs e);

    /*  Log -> TextBox 기록 전용 Class */
    public class YoonDisplayer : IDisposable
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
                Clear();
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

        public const int MAX_LOG_NUM = 500;

        private static object m_logLock = new object();
        private int m_nCountLog = 0;
        public event LogDisplayCallback OnLogDisplayEvent;

        public YoonDisplayer()
        {
            m_nCountLog = 0;
        }
        /// <summary>
        /// 일반 Form에서 Log를 기록할 때 사용되어지는 Sequence 다.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="message"></param>
        public void Write(eYoonStatus index, string message)
        {
            DisplayMessage(index, message);
        }

        public void Clear()
        {
            m_nCountLog = 0;
        }

        /// <summary>
        /// Log Message를 화면에 보여준다.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="message"></param>
        private void DisplayMessage(eYoonStatus index, string message)
        {
            lock (m_logLock)
            {
                bool bRequestClear = false;
                if(m_nCountLog > MAX_LOG_NUM)
                {
                    Clear();
                    bRequestClear = true;
                }

                string nowTime = string.Format("{0:yyyy-MM-dd HH:mm:ss:fff}", DateTime.Now);   //현재 시간 가져오기
                string strMessage = "";
                switch (index)
                {
                    ////  일반 Process Message 출력
                    case eYoonStatus.Normal:
                        strMessage = "[" + nowTime + "] " + message + "\n";
                        OnLogDisplayEvent(this, new LogArgs(ColorFactory.White, strMessage, bRequestClear));
                        break;
                    ////  기타 확인 : 오류 메세지 설명은 별도로 저장
                    case eYoonStatus.Conform:
                        strMessage = "[" + nowTime + "]  CONFIRM : " + message + "\n";
                        OnLogDisplayEvent(this, new LogArgs(ColorFactory.Khaki, strMessage, bRequestClear));
                        break;
                    ////  전송 확인 : 전송 메시지 출력
                    case eYoonStatus.Send:
                        strMessage = "[" + nowTime + "]  SEND : " + message + "\n";
                        OnLogDisplayEvent(this, new LogArgs(ColorFactory.Lavender, strMessage, bRequestClear));
                        break;
                    ////  수신 확인 : 수신 메시지 출력
                    case eYoonStatus.Receive:
                        strMessage = "[" + nowTime + "]  RECEIVE : " + message + "\n";
                        OnLogDisplayEvent(this, new LogArgs(ColorFactory.Lavender, strMessage, bRequestClear));
                        break;
                    ////  사용자 확인 : 사용자 조작시 발생 메시지 출력
                    case eYoonStatus.User:
                        strMessage = "[" + nowTime + "]  USER : " + message + "\n";
                        OnLogDisplayEvent(this, new LogArgs(ColorFactory.Lime, strMessage, bRequestClear));
                        break;
                    ////  Insepct 확인 :  Inspection 시 발생 메시지 출력
                    case eYoonStatus.Inspect:
                        strMessage = "[" + nowTime + "]  INSPECT : " + message + "\n";
                        OnLogDisplayEvent(this, new LogArgs(ColorFactory.Lime, strMessage, bRequestClear));
                        break;
                    ////  오류 확인 :  오류 메시지 출력
                    case eYoonStatus.Error:
                        strMessage = "[" + nowTime + "]  ERROR : " + message + "\n";
                        OnLogDisplayEvent(this, new LogArgs(ColorFactory.Salmon, strMessage, bRequestClear));
                        break;
                    ////  Info 확인 :  상황 확인 메시지 출력
                    case eYoonStatus.Info:
                        strMessage = "[" + nowTime + "]  INFO : " + message + "\n";
                        OnLogDisplayEvent(this, new LogArgs(ColorFactory.Khaki, strMessage, bRequestClear));
                        break;
                }
                m_nCountLog++;
            }
        }
    }
}
