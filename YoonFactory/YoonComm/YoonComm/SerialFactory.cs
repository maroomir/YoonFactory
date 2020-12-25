using System;
using System.Text;
using System.IO.Ports;

namespace YoonFactory.Comm.Serial
{
    public class SerialComm : IDisposable
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
                    m_serial.Dispose();
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~SerialComm() {
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

        private SerialPort m_serial = new SerialPort();
        /// <summary>
        /// Serail 통신을 위해 Port를 연다.
        /// </summary>
        /// <param name="portName">Port 이름 앞에 반드시 COM_을 붙여야한다.  (ex. COM1)</param>
        /// <returns></returns>
        public bool OpenPort(string portName)
        {
            int pos;
            string strLog;
            ////  잘못된 Port 이름에 대한 예외처리한다.
            if (portName == "") return false;
            pos = portName.IndexOf("COM");
            if (pos <= 0)
            {
                strLog = "Invalid Port Name : " + portName;
                return false;
            }
            ////  Serial 통신의 Param을 정의한다.
            if (m_serial == null) m_serial = new SerialPort();
            m_serial.PortName = portName;
            m_serial.BaudRate = (int)115200;
            m_serial.DataBits = (int)8;
            m_serial.Parity = Parity.None;
            m_serial.StopBits = StopBits.One;
            m_serial.ReadTimeout = (int)100;
            m_serial.WriteTimeout = (int)100;
            ////  Serial 통신을 위해 Port를 개방한다.
            try
            {
                m_serial.Open();
            }
            catch
            {
                strLog = "Port Open Error!";
                return false;
            }
            if (m_serial.IsOpen) strLog = "Port Open Sucess : " + m_serial.PortName;
            else strLog = "Port Open Fail : " + m_serial.PortName;
            return true;
        }
        /// <summary>
        /// Serail 통신을 끝낸다.
        /// </summary>
        public void ClosePort()
        {
            if (m_serial.IsOpen)
            {
                m_serial.Close();
                m_serial.Dispose();
                m_serial = null;
            }
        }
        /// <summary>
        /// Port에 Data를 씌운다.
        /// </summary>
        /// <param name="data"></param>
        public void WritePort(string data)
        {
            if (m_serial.IsOpen == false) return;

            try
            {
                m_serial.Write(data);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void WritePort(char[] data, int length)
        {
            if (m_serial.IsOpen == false) return;

            try
            {
                m_serial.Write(data, 0, length);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Port에서 Data를 읽어온다.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string ReadPort(int waitTime)
        {
            if (m_serial.IsOpen == false) return "";

            int recieveSize = m_serial.BytesToRead;
            byte[] incommBuffer = new byte[recieveSize];
            string resultData = "";
            m_serial.ReadTimeout = waitTime;
            try
            {
                if (recieveSize != 0)
                {
                    resultData = "";
                    m_serial.Read(incommBuffer, 0, recieveSize);
                    for (int i = 0; i < recieveSize; i++)
                    {
                        resultData += Convert.ToChar(incommBuffer[i]);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resultData;
        }

        /// <summary>
        /// 문자열을 ASCII 문자열로 변환한다.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string StringToHex(string text)
        {
            string resultHex = "";
            byte[] arrayByteStr = Encoding.ASCII.GetBytes(text);
            foreach (byte byteStr in arrayByteStr)
            {
                resultHex += string.Format("{0:x2}", byteStr);
            }
            return resultHex;
        }
        /// <summary>
        /// 10진수를 ASCII 문자열로 변환한다.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] DecToHex(int data, int length)
        {
            return Encoding.ASCII.GetBytes(data.ToString().ToCharArray(), 0, length);
        }
    }
}
