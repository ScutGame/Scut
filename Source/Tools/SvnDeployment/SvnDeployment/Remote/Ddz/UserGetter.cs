using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ZyGames.Core.Data;
using ZyGames.GamesReportService;

namespace ZyGames.OA.BLL.Remote.Ddz
{
    public class UserGetter : IDataGetter
    {
        private int _gameId;
        private int _serverId;
        private string _filter;
        private List<SqlParameter> _parameters;

        public UserGetter(int gameId, int serverId, string filter, List<SqlParameter> parameters)
        {
            _gameId = gameId;
            _serverId = serverId;
            _filter = filter;
            _parameters = parameters;

        }

        public int RecordCount
        {
            get;
            set;
        }

        public DataTable GetData()
        {
            var dbConfig = DbConfig.GetData(_gameId, _serverId);
            string sql = "SELECT count(UserID) FROM GameUser a " + _filter;
            RecordCount = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConfig.ConnectionString, CommandType.Text, sql, _parameters.ToArray()));
            sql = string.Format(@"select * from( 
	SELECT  row_number()over(order by UserID desc) as RowNumber, 
		UserID,Pid,NickName,Sex,UserStatus,MsgState,CreateDate,LastLoginDate,(GiftGold + PayGold + ExtGold - UseGold) as GoldNum,GameCoin,WinNum,FailNum,ScoreNum,RetailID,RealName,Profession,Hobby,(convert(varchar(10),Birthday,120)) Birthday,
		(select [Name] from DdzConfig.dbo.TitleInfo where Id=TitleId) TitleName
	FROM GameUser {0}) as a 
where RowNumber > @statIndex and RowNumber<=@endIndex", _filter);
            DataTable dt = new DataTable();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(dbConfig.ConnectionString, CommandType.Text, sql, _parameters.ToArray()))
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
