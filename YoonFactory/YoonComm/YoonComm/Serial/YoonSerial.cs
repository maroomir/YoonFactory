using System;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace YoonFactory.Comm.Serial
{

    public class YoonSerial : IYoonComm, IDisposable
    {

        #region IDisposable Support
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Close();
                    Thread.Sleep(100);
                    m_pSerial.Dispose();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        private SerialPort m_pSerial = new SerialPort();

        public string Port { get; set; }

        public StringBuilder ReceiveMessage { get; private set; }

        public YoonSerial()
        {
            //
        }

        public YoonSerial(string strPort)
        {
            Port = strPort;
        }

        public void CopyFrom(IYoonComm pComm)
        {
            if (pComm is YoonSerial)
            {
                Port = pComm.Port;
            }
        }

        public IYoonComm Clone()
        {
            Close();
            return new YoonSerial(Port);
        }

        public bool Open()
        {
            // Set-up the parameter of serial communication
            if(m_pSerial == null)
                m_pSerial = new SerialPort();
            m_pSerial.PortName = Port;
            m_pSerial.BaudRate = 115200;
            m_pSerial.DataBits = 8;
            m_pSerial.Parity = Parity.None;
            m_pSerial.StopBits = StopBits.One;
            m_pSerial.ReadTimeout = 100;
            m_pSerial.WriteTimeout = 100;
            // Open the port for serial communication
            try
            {
                m_pSerial.Open();
            }
            catch
            {
                Console.Write("Port Open Error!");
                return false;
            }

            if (m_pSerial.IsOpen)
                Console.Write("Port Open Success : " + m_pSerial.PortName);
            else
                Console.Write("Port Open Fail : " + m_pSerial.PortName);
            return true;
        }

        /// <summary>
        /// Open the port to use serial
        /// </summary>
        /// <param name="strPortName">Port Name with HEAD (ex. COM1)</param>
        /// <returns></returns>
        public bool Open(string strPortName)
        {
            // Return false if the port name is invalid
            if (strPortName == "") return false;
            int nHeadLength = strPortName.IndexOf("COM", StringComparison.Ordinal);
            if (nHeadLength <= 0)
            {
                Console.Write("Invalid Port Name : " + strPortName);
                return false;
            }
            Port = strPortName;
            return Open();
        }

        /// <summary>
        /// Close the serial communication
        /// </summary>
        public void Close()
        {
            if (!m_pSerial.IsOpen) return;
            m_pSerial.Close();
            m_pSerial.Dispose();
            m_pSerial = null;
        }


        public bool Send(string strBuffer)
        {
            if (!m_pSerial.IsOpen) return false;

            try
            {
                m_pSerial.Write(strBuffer);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool Send(byte[] pBuffer)
        {
            if (!m_pSerial.IsOpen) return false;

            try
            {
                m_pSerial.Write(pBuffer, 0, pBuffer.Length);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public string Receive(int nWaitTime)
        {
            if (m_pSerial.IsOpen == false) return "";

            int nReceiveSize = m_pSerial.BytesToRead;
            byte[] pBufferIncoming = new byte[nReceiveSize];
            m_pSerial.ReadTimeout = nWaitTime;
            string strReceiveMessage = "";
            try
            {
                if (nReceiveSize != 0)
                {
                    m_pSerial.Read(pBufferIncoming, 0, nReceiveSize);
                    for (int i = 0; i < nReceiveSize; i++)
                    {
                        strReceiveMessage += Convert.ToChar(pBufferIncoming[i]);
                    }

                    ReceiveMessage = new StringBuilder(strReceiveMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return strReceiveMessage;
        }
    }
}
