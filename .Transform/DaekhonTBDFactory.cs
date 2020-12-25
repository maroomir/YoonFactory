using System;
namespace YoonFactory.Daekhon.TBD
{
    public class Daekhon : IDisposable
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
                    m_comm.Dispose();
                }
                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~Daekhon() {
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

        public bool m_isCommOpen;
        public bool m_isTriggerOn;
        public SerialComm m_comm = new SerialComm();
        public void Init(string portName)
        {
            m_isCommOpen = m_comm.OpenPort(portName);
        }
        public void Close()
        {
            if (m_isCommOpen == true)
                m_comm.ClosePort();
        }
        public void Active(bool isOnOff)
        {
            m_isTriggerOn = isOnOff;
            char[] chData = new char[4];
            if (isOnOff == true)
            {
                chData[0] = '\x2f';     // '/'
                chData[1] = '\x6f';     // 'o'
                chData[2] = '\x6e';     // 'n'
                chData[3] = '\x2f';     // '/'
            }
            else
            {
                chData[0] = '\x2f';     // '/'
                chData[1] = '\x6f';     // 'o'
                chData[2] = '\x66';     // 'f'
                chData[3] = '\x2f';     // '/'
            }
            m_comm.WritePort(chData, 4);
            m_comm.ReadPort(500);
        }
        public void ClearEncoder()
        {
            char[] chData = new char[4];
            chData[0] = '\x2f';     // '/'
            chData[1] = '\x72';     // 'r'
            chData[2] = '\x65';     // 'e'
            chData[3] = '\x2f';		// '/'
            m_comm.WritePort(chData, 4);
            m_comm.ReadPort(500);
        }
        public void ClearTrigger()
        {
            char[] chData = new char[4];
            chData[0] = '\x2f';     // '/'
            chData[1] = '\x72';     // 'r'
            chData[2] = '\x74';     // 't'
            chData[3] = '\x2f';		// '/'
            m_comm.WritePort(chData, 4);
            m_comm.ReadPort(500);
        }
        public void SetMode(char mode)
        {
            char[] chData = new char[5];
            chData[0] = '\x2f';             // '/'
            chData[1] = '\x74';             // 't'
            chData[2] = '\x6d';             // 'm'
            chData[3] = (char)('\x30' + mode);// mode
            chData[4] = '\x2f';				// '/'
            m_comm.WritePort(chData, 5);
            m_comm.ReadPort(500);
            SetFPROMData(1, '\x30', (char)('\x30' + mode));		// '/fw000100/' 또는 '/fw000101/'
        }
        public void SetPeriod(double ms, bool isDefault)
        {
            int clock = (int)(ms * 1000000 / 10);
            byte[] hexClock = m_comm.DecToHex(clock, 8);
            char[] chData = new char[12];
            chData[0] = '\x2f';         // '/'
            chData[1] = '\x30';         // '0'
            chData[2] = '\x64';         // 'd'
            chData[3] = (char)hexClock[0];    // clock
            chData[4] = (char)hexClock[1];
            chData[5] = (char)hexClock[2];
            chData[6] = (char)hexClock[3];
            chData[7] = (char)hexClock[4];
            chData[8] = (char)hexClock[5];
            chData[9] = (char)hexClock[6];
            chData[10] = (char)hexClock[7];
            chData[11] = '\x2f';        // '/'
            m_comm.WritePort(chData, 12);
            m_comm.ReadPort(500);
            if (isDefault)
            {
                SetFPROMData(3, (char)hexClock[0], (char)hexClock[1]);
                SetFPROMData(4, (char)hexClock[2], (char)hexClock[3]);
                SetFPROMData(5, (char)hexClock[4], (char)hexClock[5]);
                SetFPROMData(6, (char)hexClock[6], (char)hexClock[7]);
            }
        }
        public void SetPulseWidth(double ms, bool isDefault)
        {
            int clock = (int)(ms * 1000000 / 10);
            byte[] hexClock = m_comm.DecToHex(clock, 8);
            char[] chData = new char[12];
            chData[0] = '\x2f';         // '/'
            chData[1] = '\x30';         // '0'
            chData[2] = '\x77';         // 'w'
            chData[3] = (char)hexClock[0];    // clock
            chData[4] = (char)hexClock[1];
            chData[5] = (char)hexClock[2];
            chData[6] = (char)hexClock[3];
            chData[7] = (char)hexClock[4];
            chData[8] = (char)hexClock[5];
            chData[9] = (char)hexClock[6];
            chData[10] = (char)hexClock[7];
            chData[11] = '\x2f';        // '/'
            m_comm.WritePort(chData, 12);
            m_comm.ReadPort(500);
            if (isDefault)
            {
                SetFPROMData(7, (char)hexClock[0], (char)hexClock[1]);
                SetFPROMData(8, (char)hexClock[2], (char)hexClock[3]);
                SetFPROMData(9, (char)hexClock[4], (char)hexClock[5]);
                SetFPROMData(10, (char)hexClock[6], (char)hexClock[7]);
            }
        }
        public void SetDelayTime(char channel, double us, bool isDefault)
        {
            int clock = (int)(us * 1000 / 10);
            byte[] hexClock = m_comm.DecToHex(clock, 8);
            char[] chData = new char[13];
            chData[0] = '\x2f';         // '/'
            chData[1] = '\x30';         // '0'
            chData[2] = '\x7c';         // '|'
            chData[3] = channel;  // channel
            chData[4] = (char)hexClock[0];    // clock
            chData[5] = (char)hexClock[1];
            chData[6] = (char)hexClock[2];
            chData[7] = (char)hexClock[3];
            chData[8] = (char)hexClock[4];
            chData[9] = (char)hexClock[5];
            chData[10] = (char)hexClock[6];
            chData[11] = (char)hexClock[7];
            chData[12] = '\x2f';        // '/'
            m_comm.WritePort(chData, 13);
            m_comm.ReadPort(500);
            if (isDefault)
            {
                SetFPROMData(24 + 4 * channel, (char)hexClock[0], (char)hexClock[1]);
                SetFPROMData(25 + 4 * channel, (char)hexClock[2], (char)hexClock[3]);
                SetFPROMData(26 + 4 * channel, (char)hexClock[4], (char)hexClock[5]);
                SetFPROMData(27 + 4 * channel, (char)hexClock[6], (char)hexClock[7]);
            }
        }
        public void SetFPROMData(int section, char value1, char value2)
        {
            char[] chData = new char[10];
            chData[0] = '\x2f';     // '/'
            chData[1] = '\x66';     // 'f'
            chData[2] = '\x77';     // 'w'
            chData[3] = '\x30';     // '0'
            chData[4] = '\x30';     // '0'
            chData[5] = (char)('\x30' + (char)(section / 16));    // section.16
            chData[6] = (char)('\x30' + (char)(section % 16));    // section.1
            chData[7] = value1;     // value.16
            chData[8] = value2;     // value.1
            chData[9] = '\x2f';     // '/'
            m_comm.WritePort(chData, 10);
            m_comm.ReadPort(500);
        }
        public void GetPeriod()
        {
            char[] chData = new char[6];
            chData[0] = '\x2f';     // '/'
            chData[1] = '\x67';     // 'g'
            chData[2] = '\x69';     // 'i'
            chData[3] = '\x35';     // '5'
            chData[4] = '\x31';     // '1'
            chData[5] = '\x2f';     // '/'
            m_comm.WritePort(chData, 6);
            m_comm.ReadPort(500);
        }
        public void GetPulseWidth()
        {
            char[] chData = new char[6];
            chData[0] = '\x2f';     // '/'
            chData[1] = '\x67';     // 'g'
            chData[2] = '\x69';     // 'i'
            chData[3] = '\x35';     // '5'
            chData[4] = '\x32';     // '2'
            chData[5] = '\x2f';     // '/'
            m_comm.WritePort(chData, 6);
            m_comm.ReadPort(500);
        }
        public void GetDelayTime(char channel)
        {
            char[] chData = new char[6];
            chData[0] = '\x2f';                 // '/'
            chData[1] = '\x67';                 // 'g'
            chData[2] = '\x69';                 // 'i'
            chData[3] = '\x35';                 // '5'
            chData[4] = (char)('\x33' + channel); // '3' + a
            chData[5] = '\x2f';                 // '/'
            m_comm.WritePort(chData, 6);
            m_comm.ReadPort(500);
        }
    }

}
