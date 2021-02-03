using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;

namespace YoonFactory.Windows
{
    public static class DataTableFactory
    {
        public static void SetDataTableColumnTitle(ref DataTable pTable, List<string> pListStrTitle)
        {
            if (pTable == null || pListStrTitle == null) return;

            for(int iColumn = 0; iColumn < pListStrTitle.Count; iColumn++)
            {
                pTable.Columns.Add(new DataColumn(pListStrTitle[iColumn]));
            }
        }

        public static void SetDataTableColumnTitle(ref DataTable pTable, string[] pArrayStrTitle)
        {
            if (pTable == null || pArrayStrTitle == null) return;

            for (int iColumn = 0; iColumn < pArrayStrTitle.Length; iColumn++)
            {
                pTable.Columns.Add(new DataColumn(pArrayStrTitle[iColumn]));
            }
        }

        public static void SetDataTableRowData(ref DataTable pTable, List<object> pListObject)
        {
            if (pTable == null || pListObject == null) return;
            if (pListObject.Count != pTable.Columns.Count) return;

            DataRow pRow = pTable.NewRow();
            for (int iColumn = 0; iColumn < pTable.Columns.Count; iColumn++)
            {
                string strDataTitle = pTable.Columns[iColumn].ColumnName;
                pRow[strDataTitle] = pListObject[iColumn];
            }
            pTable.Rows.Add(pRow);
        }

        public static void ChangeDataTableData(ref DataTable pTable, int nRow, int nColumn, object pObject)
        {
            if (pTable == null) return;
            if (nColumn < 0 || nColumn >= pTable.Columns.Count) return;
            if (nRow < 0 || nRow >= pTable.Rows.Count) return;

            pTable.Rows[nRow][nColumn] = pObject;
        }

        public static void ChangeDataTableData(ref DataTable pTable, int nRow, string strColumn, object pObject)
        {
            if (pTable == null) return;
            if (!pTable.Columns.Contains(strColumn)) return;
            if (nRow < 0 || nRow >= pTable.Rows.Count) return;

            pTable.Rows[nRow][strColumn] = pObject;
        }

        public static DataTable ConvertCrossTable(DataTable pTable)
        {
            DataTable pTableCross = new DataTable();

            try
            {
                ////  Column 명, Type 정의
                ArrayList pArrayCol = new ArrayList();
                //// 첫번째 Row를 Column으로 변환 후 Title로 삼기 (Row[0] -> Column[0], Title Column)
                for (int iRow = 0; iRow < pTable.Rows.Count; iRow++)
                {
                    if (iRow < pTable.Columns.Count)
                    {
                        pArrayCol.Add(pTable.Columns[iRow].ColumnName);
                    }
                    pTableCross.Columns.Add(pTable.Rows[iRow][0].ToString(), typeof(String));
                }
                //// Cross Table 내 Data 삽입 (RowCollection → ColumnCollection)
                for (int iColumn = 1; iColumn < pTable.Columns.Count; iColumn++)
                {
                    DataRow pRowCross = pTableCross.NewRow();
                    for (int iRow = 0; iRow < pTable.Rows.Count; iRow++)
                    {
                        pRowCross[iRow] = pTable.Rows[iRow][iColumn];
                    }
                    pTableCross.Rows.Add(pRowCross);
                }
            }
            catch (Exception ex) // 프로그램에서 예상하지 못한 Exception을 처리합니다.
            {
                Console.WriteLine(ex.ToString());
            }

            return pTableCross;
        }
    }
}