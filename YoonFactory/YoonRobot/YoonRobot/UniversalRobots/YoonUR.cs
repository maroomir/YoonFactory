using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoonFactory.Comm;
using YoonFactory.Comm.TCP;

namespace YoonFactory.Robot.UniversialRobot
{
    public class YoonUR : IYoonRemote
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

                Close();

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        ~YoonUR()
        {
            Dispose(false);
        }


        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            GC.SuppressFinalize(this);
        }
        #endregion

        public string RootDirectory { get; private set; } = Path.Combine(Directory.GetCurrentDirectory(), @"YoonFactory", @"YoonUR");

        public event ShowMessageCallback OnShowMessageEvent;
        public event RecieveDataCallback OnShowReceiveDataEvent;

        private YoonServer m_pTcpServer = null;
        private YoonClient m_pTcpClient = null;

        public YoonUR()
        {
            // 서버용 구독 초기화
            m_pTcpClient = new YoonClient();
            m_pTcpClient.OnShowMessageEvent += OnShowMessageEvent;
            m_pTcpClient.OnShowReceiveDataEvent += OnShowReceiveDataEvent;
            // 클라이언트용 구독 초기화
            m_pTcpServer = new YoonServer();
            m_pTcpServer.OnShowMessageEvent += OnShowMessageEvent;
            m_pTcpServer.OnShowReceiveDataEvent += OnShowReceiveDataEvent;
            // Parameter 초기화하기
            
        }

        public IYoonRemote Clone()
        {
            throw new NotImplementedException();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public void CopyFrom(IYoonRemote pRemote)
        {
            throw new NotImplementedException();
        }

        public bool Open()
        {
            throw new NotImplementedException();
        }
    }
}
