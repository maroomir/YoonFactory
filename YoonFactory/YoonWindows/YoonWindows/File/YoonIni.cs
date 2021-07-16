using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using YoonFactory.Files;

namespace YoonFactory.Windows.File
{
    /* Ini 파일을 관리하는 Class - Windows 전용 */
    public class YoonIni
    {
        private string m_iniPath;

        /// <summary>
        /// Ini 파일의 위치를 인자로 받는 생성자다.
        /// </summary>
        /// <param name="path"></param>
        public YoonIni(string path)
        {
            if (FileFactory.VerifyFileExtension(ref path, ".ini", true, true))
                m_iniPath = path;
            else
                m_iniPath = Path.Combine(Directory.GetCurrentDirectory(), "YoonFacotry", @"YoonFactory.ini");
        }

        #region 안전한 DLL 선언
        [SuppressUnmanagedCodeSecurity]
        internal static class SafeNativeMethods
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
            internal static extern int GetPrivateProfileString(
            string section,
            string key,
            string def,
            StringBuilder retVal,
            int size,
            string filePath);
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
            internal static extern int WritePrivateProfileString(
                string section,
                string key,
                string val,
                string filePath);
        }
        #endregion


        /// <summary>
        /// Ini의 Data를 읽어온다.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string GetValue(string section, string key, object defaultValue)
        {

            StringBuilder result = new StringBuilder(255);
            try
            {
                int i = SafeNativeMethods.GetPrivateProfileString(section, key, "", result, 255, m_iniPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            if (result.ToString() == String.Empty) return defaultValue.ToString(); // 예외 시 초기화 처리
            else return result.ToString();
        }

        public string GetValueToString(string section, string key, string defaultValue)
        {
            return GetValue(section, key, defaultValue);
        }

        public T GetValueToEnum<T>(string section, string key, T defaultValue)
        {
            string strValue = GetValue(section, key, defaultValue);

            if (!System.Enum.IsDefined(typeof(T), strValue))
                return default(T);

            return (T)System.Enum.Parse(typeof(T), strValue, true);
        }

        public int GetValueToInt(string section, string key, int defaultValue)
        {
            return Convert.ToInt32(GetValue(section, key, defaultValue));
        }

        public double GetValueToDouble(string section, string key, double defaultValue)
        {
            return Convert.ToDouble(GetValue(section, key, defaultValue));
        }

        public short GetValueToShort(string section, string key, short defaultValue)
        {
            return Convert.ToInt16(GetValue(section, key, defaultValue));
        }

        public Byte GetValueToByte(string section, string key, byte defaultValue)
        {
            return Convert.ToByte(GetValue(section, key, defaultValue));
        }

        /// <summary>
        /// Ini의 Data를 설정한다.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValue(string section, string key, string value)
        {
            SafeNativeMethods.WritePrivateProfileString(section, key, value, m_iniPath);
        }
    }

}
