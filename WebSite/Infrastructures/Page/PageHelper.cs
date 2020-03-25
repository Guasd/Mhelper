using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Infrastructures.Page
{
    public class PageHelper
    {
        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="table"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataTable GetPageForDataTable(DataTable table,int PageIndex,int PageSize)
        {
            if (PageIndex > 0)
            {
                DataTable NewTable = table;
                NewTable.Clear();
                int RowBegin = (PageIndex - 1) * PageSize;
                int RowEnd = PageIndex * PageSize;

                if (RowBegin >= NewTable.Rows.Count)
                {
                    return NewTable;
                }

                if (RowEnd > NewTable.Rows.Count)
                {
                    RowEnd = NewTable.Rows.Count;
                }

                for(int i = RowBegin; i <= RowEnd - 1; i++)
                {
                    DataRow NewDr = NewTable.NewRow();
                    DataRow dr = table.Rows[i];
                    foreach(DataColumn column in table.Columns)
                    {
                        NewDr[column.ColumnName] = dr[column.ColumnName];
                    }
                    NewTable.Rows.Add(NewDr);
                }
                return NewTable;
            }
            else
            {
                return table;
            }
        }
    }
}
