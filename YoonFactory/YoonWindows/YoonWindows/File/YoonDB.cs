using System;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace YoonFactory.Windows.File
{
    /* DataBase 파일을 관리하는 Class */
    public class YoonDB : IDisposable
    {
        private OleDbConnection m_objectConnect;
        private OleDbCommand m_dbCommand;
        private OleDbDataReader m_dbReader;
        private OleDbDataAdapter m_dbAdapter;
        private bool m_isConnect;
        private bool m_isInsertQuery;
        private bool m_isAlreadyCommand;

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    m_objectConnect.Dispose();
                    m_dbCommand.Dispose();
                    m_dbReader.Dispose();
                    m_dbAdapter.Dispose();
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

        public YoonDB()
        {
            m_objectConnect = new OleDbConnection();

            m_isAlreadyCommand = false;
            m_isConnect = false;
        }

        // DB 파일을 Open 한다.
        public bool Open(string filePath)
        {
            //             //// .NET Framework 4.0
            //             StringBuilder connectionPath = new StringBuilder();
            //             connectionPath.clear();
            //             connectionPath.AppendFormat(@"Provider=Microsoft.ACE.OLEDB.12.0;", @"Data Source=", @filePath, @"Persist Security Info=False;");

            //             //// .NET Framework 3.5 
            //             StringBuilder connectionPath = new StringBuilder(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=%s;Persist Security Info=False;");
            //             connectionPath.Replace("%s", filePath);
            // 
            //             m_objectConnect.ConnectionString = connectionPath.ToString();

            //// None StringBuilder Used
            m_objectConnect.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath;

            bool isConnect = false;

            try
            {
                m_objectConnect.Open();
                isConnect = true;
            }
            catch
            {
                isConnect = false;
            }

            m_isConnect = isConnect;
            return isConnect;
        }

        // DB 파일을 Close 한다.
        public bool Close()
        {
            bool isClose = false;

            try
            {
                m_objectConnect.Close();
                m_dbCommand.Dispose();
                m_dbAdapter.Dispose();
                m_dbReader.Dispose();

                isClose = true;
                m_isConnect = false;
            }
            catch
            {
                isClose = false;
            }

            return isClose;
        }

        // DB에 QUERY 명령을 삽입한다.
        public bool Insert(string query)
        {
            bool isInsertQuery;
            int result;

            if (!m_isConnect) return false;
            if (m_isAlreadyCommand)
            {
                m_dbCommand.Dispose();
                m_dbAdapter.Dispose();
                m_isAlreadyCommand = false;
            }

            try
            {
                m_dbCommand = new OleDbCommand(query, m_objectConnect);
                m_dbCommand.CommandType = CommandType.Text;
                result = m_dbCommand.ExecuteNonQuery();
                m_dbAdapter = new OleDbDataAdapter(query, m_objectConnect);
                isInsertQuery = true;
            }
            catch
            {
                isInsertQuery = false;
            }


            m_isAlreadyCommand = true;
            m_isInsertQuery = isInsertQuery;

            return isInsertQuery;
        }

        // 읽어온 DataSet 전체를 반환한다.
        public bool GetData(ref DataSet data)
        {
            if (!m_isConnect) return false;

            m_dbAdapter.Fill(data);

            return true;
        }

        // 읽어온 DataReader 전체를 반환한다.
        public bool GetData(ref OleDbDataReader data)
        {
            if (!m_isConnect) return false;

            data = this.m_dbCommand.ExecuteReader(CommandBehavior.Default);

            return true;
        }

        // 해당 Data로 만든 String 배열을 반환한다.
        public bool GetValue(ref string[] datas, string target)
        {
            int count = 0;
            string dataArray = "";
            string editedArray = "";

            if (!m_isConnect) return false;

            this.GetData(ref m_dbReader);

            while (m_dbReader.Read())
            {
                dataArray += m_dbReader[target].ToString() + ",";
                count++;
            }

            //// 문자열 쪼개기
            int length = dataArray.Length;
            if (length == 0) editedArray = dataArray;
            else
            {
                editedArray = dataArray.Substring(0, length - 1);
            }

            datas = editedArray.Split(',');

            return true;
        }

        // DB에서 선택되어진 순서(Index)값을 반환한다.
        public Int32 SelectIndex()
        {
            if (!m_isConnect) return 0;

            Int32 index = 0;

            OleDbDataReader oleReader = this.m_dbCommand.ExecuteReader(CommandBehavior.Default);

            while (oleReader.Read())
            {
                if (!oleReader.IsDBNull(0)) index = (Int32)(oleReader.GetInt32(0) + 1);
                else
                {
                    index = 0;
                }
            }

            oleReader.Dispose();

            return index;
        }

        // DB에서 선택되어진 값(string)을 받아온다.
        // 필요시 이 값을 Int32, Int64, Double 등으로 반환한다.
        public string SelectValue()
        {
            StringBuilder value = new StringBuilder(255);
            if (!m_isConnect) return value.ToString();

            OleDbDataReader oleReader = this.m_dbCommand.ExecuteReader(CommandBehavior.Default);

            while (oleReader.Read())
            {
                if (!oleReader.IsDBNull(0))
                {
                    //                    value.clear();    //// .NET Framework 4.0
                    value = new StringBuilder(oleReader.GetString(0));
                }
                else
                {
                    continue;
                }
            }

            oleReader.Dispose();

            return value.ToString();
        }
    }

}
