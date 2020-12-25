using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using CommFactory;

namespace YoonFactory.Fastech
{
    public class Fastech
    {
        public class EziMotionPR
        {
            public const int FMM_OK = 0;
            internal static class UnsafeNativeMethods
            {
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_Connect(byte nPortNo, uint dwBaud);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern void FAS_Close(byte nPortNo);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_IsSlaveExist(byte nPortNo, byte iSlaveNo);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_ServoEnable(byte nPortNo, byte iSlaveNo, int bOnOff);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_ServoAlarmReset(byte nPortNo, byte iSlaveNo);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_ClearPosition(byte nPortNo, byte iSlaveNo);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_GetAxisStatus(byte nPortNo, byte iSlaveNo, ref uint dwAxisStatus);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_MoveStop(byte nPortNo, byte iSlaveNo);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_EmergencyStop(byte nPortNo, byte iSlaveNo);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_MoveOriginSingleAxis(byte nPortNo, byte iSlaveNo);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_MoveSingleAxisAbsPos(byte nPortNo, byte iSlaveNo, int lAbsPos, uint lVelocity);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_MoveSingleAxisIncPos(byte nPortNo, byte iSlaveNo, int lIncPos, uint lVelocity);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_MoveVelocity(byte nPortNo, byte iSlaveNo, uint lVelocity, int iVelDir);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_MoveToLimit(byte nPortNo, byte iSlaveNo, uint lVelocity, int iLimitDir);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_GetCommandPos(byte nPortNo, byte iSlaveNo, ref int lCmdPos);
                [DllImport("EziMOTIONPlusR.dll")]
                public static extern int FAS_GetActualPos(byte nPortNo, byte iSlaveNo, ref int lActPos);
            }

            public int Connect(byte nPortNo, uint dwBaud)
            {
                return UnsafeNativeMethods.FAS_Connect(nPortNo, dwBaud);
            }

            public void Close(byte nPortNo)
            {
                UnsafeNativeMethods.FAS_Close(nPortNo);
            }

            public int IsSlaveExist(byte nPortNo, byte iSlaveNo)
            {
                return UnsafeNativeMethods.FAS_IsSlaveExist(nPortNo, iSlaveNo);
            }

            public int ServoEnable(byte nPortNo, byte iSlaveNo, int bOnOff)
            {
                return UnsafeNativeMethods.FAS_ServoEnable(nPortNo, iSlaveNo, bOnOff);
            }

            public int ServoAlarmReset(byte nPortNo, byte iSlaveNo)
            {
                return UnsafeNativeMethods.FAS_ServoAlarmReset(nPortNo, iSlaveNo);
            }

            public int GetAxisStatus(byte nPortNo, byte iSlaveNo, ref uint dwAxisStatus)
            {
                return UnsafeNativeMethods.FAS_GetAxisStatus(nPortNo, iSlaveNo, ref dwAxisStatus);
            }

            public int MoveStop(byte nPortNo, byte iSlaveNo)
            {
                return UnsafeNativeMethods.FAS_MoveStop(nPortNo, iSlaveNo);
            }

            public int EmergencyStop(byte nPortNo, byte iSlaveNo)
            {
                return UnsafeNativeMethods.FAS_EmergencyStop(nPortNo, iSlaveNo);
            }

            public int MoveOriginSingleAxis(byte nPortNo, byte iSlaveNo)
            {
                return UnsafeNativeMethods.FAS_MoveOriginSingleAxis(nPortNo, iSlaveNo);
            }

            public int MoveSingleAxisAbsPos(byte nPortNo, byte iSlaveNo, int lAbsPos, uint lVelocity)
            {
                return UnsafeNativeMethods.FAS_MoveSingleAxisAbsPos(nPortNo, iSlaveNo, lAbsPos, lVelocity);
            }

            public int MoveSingleAxisIncPos(byte nPortNo, byte iSlaveNo, int lIncPos, uint lVelocity)
            {
                return UnsafeNativeMethods.FAS_MoveSingleAxisIncPos(nPortNo, iSlaveNo, lIncPos, lVelocity);
            }

            public int MoveVelocity(byte nPortNo, byte iSlaveNo, uint lVelocity, int iVelDir)
            {
                return UnsafeNativeMethods.FAS_MoveVelocity(nPortNo, iSlaveNo, lVelocity, iVelDir);
            }

            public int MoveToLimit(byte nPortNo, byte iSlaveNo, uint lVelocity, int iLimitDir)
            {
                return UnsafeNativeMethods.FAS_MoveToLimit(nPortNo, iSlaveNo, lVelocity, iLimitDir);
            }

            public int GetCommandPos(byte nPortNo, byte iSlaveNo, ref int lCmdPos)
            {
                return UnsafeNativeMethods.FAS_GetCommandPos(nPortNo, iSlaveNo, ref lCmdPos);
            }

            public int GetActualPos(byte nPortNo, byte iSlaveNo, ref int lActPos)
            {
                return UnsafeNativeMethods.FAS_GetActualPos(nPortNo, iSlaveNo, ref lActPos);
            }
        }
        public enum Direction
        {
            Decrease,
            Increase,
        }
        public bool m_isConnected = false;
        public byte m_portNo = 0;
        public byte m_salveNo = 0;
        public uint m_axisStatus = 0;
        public uint m_velocity = 1000;
        public int m_position = 1000;
        public int m_commandPos = 1000;
        public int m_actualPos = 1000;
        public int m_prevPos = 1000;

        public bool Init(string portName, string slaveName, int commandVelocity, int commandPosition)
        {
            ////  Port No 산출.
            int pos = portName.IndexOf("COM");
            int length = portName.Length;
            string strErrorInfo, strClip;
            if (pos <= 0)
            {
                strErrorInfo = "Invalid Port Name : " + portName;
                return false;
            }
            strClip = portName.Substring(pos + 3, length);
            m_portNo = byte.Parse(strClip);
            ////  Slave No 산출.
            pos = slaveName.IndexOf("SLAVE");
            length = slaveName.Length;
            if (pos <= 0)
            {
                strErrorInfo = "Invalid Slave Name : " + slaveName;
                return false;
            }
            strClip = slaveName.Substring(pos + 1, length);
            m_salveNo = byte.Parse(strClip);
            ////  기타 값 초기화.
            m_velocity = (uint)commandVelocity;
            m_position = commandPosition;
            return true;
        }

        public void OpenServer()
        {
            uint baudRate = 115200;
            if (m_isConnected == false)
            {
                if (EziMotionPR.UnsafeNativeMethods.FAS_Connect(m_portNo, baudRate) >= 0)
                {
                    MessageBox.Show("Fastech Connection Failed :  Port Error!");
                }
                else
                {
                    if (EziMotionPR.UnsafeNativeMethods.FAS_IsSlaveExist(m_portNo, m_salveNo) >= 0)
                    {
                        MessageBox.Show("Fastech Connected Failed : Slave Error!");
                    }
                    m_isConnected = true;
                }
            }
        }

        public void CloseServer()
        {
            EziMotionPR.UnsafeNativeMethods.FAS_Close(m_portNo);
            m_isConnected = false;
        }

        public void ServoOn()
        {
            if (m_isConnected)
                EziMotionPR.UnsafeNativeMethods.FAS_ServoEnable(m_portNo, m_salveNo, 1);
        }

        public void ServoOff()
        {
            if (m_isConnected)
                EziMotionPR.UnsafeNativeMethods.FAS_ServoEnable(m_portNo, m_salveNo, 0);
        }

        public void Clear()
        {
            if (m_isConnected)
                EziMotionPR.UnsafeNativeMethods.FAS_ClearPosition(m_portNo, m_salveNo);
        }

        public int MoveOrigin()
        {
            int resultValue = 0;
            if (m_isConnected)
                resultValue = EziMotionPR.UnsafeNativeMethods.FAS_MoveOriginSingleAxis(m_portNo, m_salveNo);
            return resultValue;
        }

        public int MoveDirection(int length, Direction direction)
        {
            int resultValue = 0;
            m_prevPos = m_position;
            if (m_isConnected)
            {
                if (direction == Direction.Increase)
                {
                    resultValue = EziMotionPR.UnsafeNativeMethods.FAS_MoveSingleAxisIncPos(m_portNo, m_salveNo, length, m_velocity);
                    m_position += length;
                }
                else
                {
                    resultValue = EziMotionPR.UnsafeNativeMethods.FAS_MoveSingleAxisIncPos(m_portNo, m_salveNo, -1 * length, m_velocity);
                    m_position -= length;
                }
            }
            return resultValue;
        }

        public int MovePosition(int position)
        {
            int resultValue;
            m_prevPos = m_position;
            m_position = position;
            resultValue = EziMotionPR.UnsafeNativeMethods.FAS_MoveSingleAxisAbsPos(m_portNo, m_salveNo, m_position, m_velocity);
            return resultValue;
        }

        public void MoveVelocity(uint velocity, int direction)
        {
            if (m_isConnected)
                EziMotionPR.UnsafeNativeMethods.FAS_MoveVelocity(m_portNo, m_salveNo, velocity, direction);
        }

        public void MoveStop()
        {
            if (m_isConnected)
                EziMotionPR.UnsafeNativeMethods.FAS_MoveStop(m_portNo, m_salveNo);
        }

        public void GetStatus()
        {
            if (m_isConnected)
            {
                EziMotionPR.UnsafeNativeMethods.FAS_GetActualPos(m_portNo, m_salveNo, ref m_actualPos);
                EziMotionPR.UnsafeNativeMethods.FAS_GetCommandPos(m_portNo, m_salveNo, ref m_commandPos);
                EziMotionPR.UnsafeNativeMethods.FAS_GetAxisStatus(m_portNo, m_salveNo, ref m_axisStatus);
            }
        }

        public void AlarmReset()
        {
            if (m_isConnected)
                EziMotionPR.UnsafeNativeMethods.FAS_ServoAlarmReset(m_portNo, m_salveNo);
        }

        public void Emergency()
        {
            if (m_isConnected)
                EziMotionPR.UnsafeNativeMethods.FAS_EmergencyStop(m_portNo, m_salveNo);
        }
    }
}
