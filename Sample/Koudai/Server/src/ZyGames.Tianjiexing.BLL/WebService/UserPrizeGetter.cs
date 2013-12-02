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
using ZyGames.Framework.Data;
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
            var dbProvider = DbConnectionProvider.CreateDbProvider(DbConfig.Data);
            var filter = dbProvider.CreateCommandFilter();
            //command.Columns = "count(ID)";
            //command.Filter = dbProvider.CreateCommandFilter();
            //command.Filter.Condition = condition;
            //command.Filter.AddParam("NickName", name);
            //command.Parser();

            int pageIndex = 0;
            int pageSize = 0;
            //string condition = "Where 1=1";
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
                    filter.Condition += " AND " + filter.FormatExpression("CreateDate", ">=", param.Key);
                    filter.AddParam(param.Key, Convert.ToDateTime(param.Value));
                }
                else if (param.Key == "toDate")
                {
                    filter.Condition += " AND " + filter.FormatExpression("CreateDate", "<=", param.Key);
                    if (param.Value.Length <= 10)
                    {
                        param.Value += " 23:59:59";
                    }
                    filter.AddParam(param.Key, Convert.ToDateTime(param.Value));
                }
                else if (param.Key == "IsTasked")
                {
                    filter.Condition += " AND " + filter.FormatExpression("IsTasked");
                    filter.AddParam("IsTasked", param.Value.ToBool());
                }
                else if (param.Key == "UserID" && !string.IsNullOrEmpty(param.Value))
                {
                    filter.Condition += " AND " + filter.FormatExpression("UserID");
                    filter.AddParam("UserID", Convert.ToString(param.Value));
                }
            }

            pageSize = pageSize <= 0 ? 20 : pageSize;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            int statIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;

            var command = dbProvider.CreateCommandStruct("UserTakePrize", CommandMode.Inquiry);
            command.Columns = "COUNT(ID)";
            command.Filter = filter;
            command.Parser();
            int pageCount = dbProvider.ExecuteScalar(CommandType.Text,command.Sql, command.Parameters).ToInt();

            JsonGrid jsonGrid = new JsonGrid();
            jsonGrid.rows = GetUserPrizeList(dbProvider, filter, statIndex, endIndex);
            jsonGrid.total = pageCount;
            return jsonGrid;
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        private static DataTable GetUserPrizeList(DbBaseProvider dbProvider, CommandFilter condition, int statIndex, int endIndex)
        {
            var command = dbProvider.CreateCommandStruct("UserTakePrize", CommandMode.Inquiry);
            command.Columns = "ID,UserID,ObtainNum,EnergyNum,GameCoin,Gold,ExpNum,VipLv,GainBlessing,ItemPackage,CrystalPackage,SparePackage,EnchantPackage,MailContent,IsTasked,TaskDate,OpUserID,CreateDate,HonourNum,Items";
            command.FromIndex = statIndex;
            command.ToIndex = endIndex;
            command.OrderBy = "ID DESC";
            command.Filter = condition;
            command.Parser();

            DataTable dt = new DataTable();
            using (var reader = dbProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
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