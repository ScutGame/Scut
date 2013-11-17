/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Data.Sql;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.WebService
{
    public class UserPrizeGetter : BaseDataGetter
    {
        public UserPrizeGetter()
        {
        }

        public override object GetData(JsonParameter[] paramList)
        {
            int pageIndex = 0;
            int pageSize = 0;
            string condition = "Where 1=1";
            List<SqlParameter> paramValues = new List<SqlParameter>();
            foreach (JsonParameter param in paramList)
            {
                if (param.Key == "pageIndex")
                {
                    pageIndex = Convert.ToInt32(param.Value);
                    pageIndex = pageIndex < 1 ? 1 : pageIndex;
                }
                else if (param.Key == "pageSize")
                {
                    pageSize = Convert.ToInt32(param.Value);
                    pageSize = pageSize <= 0 ? 20 : pageSize;
                }
                else if (param.Key == "fromDate")
                {
                    condition += string.Format(" and CreateDate >= @{0}", param.Key);
                    paramValues.Add(SqlParamHelper.MakeInParam("@" + param.Key, SqlDbType.DateTime, 0, Convert.ToDateTime(param.Value)));
                }
                else if (param.Key == "toDate")
                {
                    condition += string.Format(" and CreateDate <= @{0}", param.Key);
                    if (param.Value.Length <= 10)
                    {
                        param.Value += " 23:59:59";
                    }
                    paramValues.Add(SqlParamHelper.MakeInParam("@" + param.Key, SqlDbType.DateTime, 0, Convert.ToDateTime(param.Value)));
                }
                else if (param.Key == "IsTasked")
                {
                    condition += string.Format(" and IsTasked = @{0}", param.Key);
                    paramValues.Add(SqlParamHelper.MakeInParam("@" + param.Key, SqlDbType.Bit, 0, param.Value.ToBool()));
                }
                else if (param.Key == "UserID" && !string.IsNullOrEmpty(param.Value))
                {
                    condition += string.Format(" and UserID = @{0}", param.Key);
                    paramValues.Add(SqlParamHelper.MakeInParam("@" + param.Key, SqlDbType.VarChar, 0, Convert.ToString(param.Value)));
                }
            }

            pageSize = pageSize <= 0 ? 20 : pageSize;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            int statIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            paramValues.Add(SqlParamHelper.MakeInParam("@statIndex", SqlDbType.Int, 0, statIndex));
            paramValues.Add(SqlParamHelper.MakeInParam("@endIndex", SqlDbType.Int, 0, endIndex));

            int pageCount = 0;

            JsonGrid jsonGrid = new JsonGrid();
            jsonGrid.rows = GetUserPrizeList(condition, out pageCount, paramValues.ToArray());
            jsonGrid.total = pageCount;
            return jsonGrid;
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        private static DataTable GetUserPrizeList(string condition, out int pageCount, SqlParameter[] parameters)
        {
            string sql = "SELECT count([ID]) FROM [dbo].[UserTakePrize] " + condition;
            pageCount = Convert.ToInt32(SqlHelper.ExecuteScalar(DbConfig.DataConnectString, CommandType.Text, sql, parameters));
            sql = "SELECT * FROM (SELECT row_number()over(order by [ID] desc) as RowNumber,[ID],[UserID],[ObtainNum],[EnergyNum],[GameCoin],[Gold],[ExpNum],[VipLv],[GainBlessing],[ItemPackage],[CrystalPackage],[SparePackage],[EnchantPackage],[MailContent],[IsTasked],[TaskDate],[OpUserID],[CreateDate],[HonourNum],[Items] from [UserTakePrize] " + condition + ") temp where RowNumber > @statIndex and RowNumber<=@endIndex";
            DataTable dt = new DataTable();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(DbConfig.DataConnectString, CommandType.Text, sql, parameters))
            {
                DataColumn col;
                DataRow row;

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    col = new DataColumn();
                    col.ColumnName = reader.GetName(i);
                    dt.Columns.Add(col);
                }

                while (reader.Read())
                {
                    row = dt.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string colName = dt.Columns[i].ColumnName;
                        row[colName] = reader[colName].ToString();
                    }
                    dt.Rows.Add(row);
                }

            }

            return dt;
        }
    }
}