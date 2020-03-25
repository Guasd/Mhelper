using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json;

namespace Infrastructure.UsefulModels
{
    public class BaseInfo
    {
        public string ToJsonForDataTable(DataTable dt)
        {
            string JsonResult = JsonConvert.SerializeObject(dt);
            return JsonResult;
        }

        public string ToJsonForDataTableObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            StringBuilder JsonResult = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonResult.Append("[");
                for(int j = 0; j < dt.Rows.Count; j++)
                {
                    JsonResult.Append("{");
                    for(int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i < dt.Columns.Count - 1)
                        {
                            JsonResult.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\""
                            + dt.Rows[j][i].ToString() + "\",");
                        }
                        else if (i < dt.Columns.Count - 1)
                        {
                            JsonResult.Append("\"" + dt.Columns[i].ColumnName.ToString() 
                            + "\":" + "\"" + ds.Tables[0].Rows[j][i].ToString() + "\"");
                        }
                    }
                    if (j == dt.Rows.Count - 1)
                    {
                        JsonResult.Append("}");
                    }
                    else
                    {
                        JsonResult.Append("},");
                    }
                }
                JsonResult.Append("]");
                return JsonResult.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
