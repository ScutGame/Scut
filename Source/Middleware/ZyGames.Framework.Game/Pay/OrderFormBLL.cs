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
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ZyGames.Framework.Common;
using ZyGames.Framework.Data;
using ZyGames.Framework.Data.Sql;

namespace ZyGames.Framework.Game.Pay
{
	/// <summary>
	/// Order form BL.
	/// </summary>
    public class OrderFormBLL
    {
        /// <summary>
        /// Add the specified model.
        /// </summary>
        /// <param name="model">Model.</param>
        public bool Add(OrderInfo model)
        {
            model.CreateDate = DateTime.Now;
            CommandStruct command = new CommandStruct("OrderInfo", CommandMode.Insert);
            command.AddParameter("OrderNO", SqlDbType.VarChar, model.OrderNO);
            command.AddParameter("MerchandiseName", SqlDbType.VarChar, model.MerchandiseName);
            command.AddParameter("PayType", SqlDbType.VarChar, model.PayType);
            command.AddParameter("Amount", SqlDbType.Decimal, model.Amount);
            command.AddParameter("Currency", SqlDbType.VarChar, model.Currency);
            command.AddParameter("Expand", SqlDbType.VarChar, model.Expand);
            command.AddParameter("SerialNumber", SqlDbType.VarChar, model.SerialNumber);
            command.AddParameter("PassportID", SqlDbType.VarChar, model.PassportID);
            command.AddParameter("ServerID", SqlDbType.Int, model.ServerID);
            command.AddParameter("GameID", SqlDbType.Int, model.GameID);
            command.AddParameter("gameName", SqlDbType.VarChar, model.GameName);
            command.AddParameter("ServerName", SqlDbType.VarChar, model.ServerName);
            command.AddParameter("PayStatus", SqlDbType.Int, model.PayStatus);
            command.AddParameter("Signature", SqlDbType.VarChar, model.Signature);
            command.AddParameter("Remarks", SqlDbType.Text, model.Remarks);
            command.AddParameter("GameCoins", SqlDbType.Int, model.GameCoins);
            command.AddParameter("SendState", SqlDbType.Int, model.SendState);
            command.AddParameter("RetailID", SqlDbType.VarChar, model.RetailID);//添加渠道商ID 孙德尧 2012/4/1 9:24
            command.AddParameter("DeviceID", SqlDbType.VarChar, model.DeviceID == null ? string.Empty : model.DeviceID);
            if (model.SendDate > DateTime.MinValue)
            {
                command.AddParameter("SendDate", SqlDbType.DateTime, model.SendDate);
            }
            command.AddParameter("CreateDate", SqlDbType.DateTime, model.CreateDate);
            command.Parser();

            int rows = SqlHelper.ExecuteNonQuery(ConfigManger.connectionString, CommandType.Text, command.Sql, command.SqlParameters);
            if (rows > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(OrderInfo model)
        {
            CommandStruct command = new CommandStruct("OrderInfo", CommandMode.Modify);
            command.AddParameter("SerialNumber", SqlDbType.VarChar, model.SerialNumber);
            command.AddParameter("PayStatus", SqlDbType.Int, model.PayStatus);
            command.AddParameter("@Signature", SqlDbType.VarChar, 0, model.Signature);
            command.Filter = new CommandFilter();
            command.Filter.Condition = "OrderNO=@OrderNO";
            command.Filter.AddParam("@OrderNO", SqlDbType.VarChar, 0, model.OrderNO);
            command.Parser();

            int rows = SqlHelper.ExecuteNonQuery(ConfigManger.connectionString, CommandType.Text, command.Sql, command.SqlParameters);
            return rows > 0;
        }

		/// <summary>
		/// 获取游戏币.
		/// </summary>
		/// <returns>The list.</returns>
		/// <param name="game">Game.</param>
		/// <param name="Server">Server.</param>
		/// <param name="Account">Account.</param>
        public OrderInfo[] GetList(int game, int Server, string Account)
        {
            //return dal.GetList(game, Server, Account);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select [OrderNO],[MerchandiseName],[PayType],[Amount],[Currency],[Expand],[SerialNumber],[PassportID],[ServerID],[GameID],[gameName],[ServerName],[PayStatus],[Signature],[Remarks],[GameCoins],[SendState],[CreateDate],[SendDate],[RetailID],[DeviceID] ");
            strSql.Append(" FROM OrderInfo");
            strSql.Append(" where gameID=@game and ServerID=@Server and PassportID=@PassportID and SendState=1 and PayStatus=2 ");
            SqlParameter[] parameters = {
					new SqlParameter("@PassportID", SqlDbType.VarChar,100),
                   	new SqlParameter("@Server", SqlDbType.Int,4),
                    new SqlParameter("@game", SqlDbType.VarChar,50)};
            parameters[0].Value = Account;
            parameters[1].Value = Server;
            parameters[2].Value = game;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(ConfigManger.connectionString, CommandType.Text, strSql.ToString(), parameters))
            {
                List<OrderInfo> olist = new List<OrderInfo>();
                while (reader.Read())
                {
                    OrderInfo ordermode = SetOrderInfo(reader);

                    olist.Add(ordermode);
                }
                return olist.ToArray();
            }
        }

		/// <summary>
		/// Determines whether this instance is exists the specified OrderNo.
		/// </summary>
		/// <returns><c>true</c> if this instance is exists the specified OrderNo; otherwise, <c>false</c>.</returns>
		/// <param name="OrderNo">Order no.</param>
        public bool IsExists(string OrderNo)
        {
            string sql = "select top 1 OrderNO from OrderInfo where OrderNO=@OrderNO";
            object obj = SqlHelper.ExecuteScalar(ConfigManger.connectionString, CommandType.Text, sql, new SqlParameter("@OrderNO", OrderNo));
            return obj != null;
            //return dal.IsExists(OrderNo);
        }

		/// <summary>
		/// Updates the by91.
		/// </summary>
		/// <returns><c>true</c>, if by91 was updated, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="callback">If set to <c>true</c> callback.</param>
        public bool UpdateBy91(OrderInfo model, bool callback)
        {
            CommandStruct command = new CommandStruct("OrderInfo", CommandMode.Modify);
            if (callback)
            {
                command.AddParameter("MerchandiseName", SqlDbType.VarChar, model.MerchandiseName);
                command.AddParameter("PayType", SqlDbType.VarChar, model.PayType);
                command.AddParameter("Amount", SqlDbType.Decimal, model.Amount);
                command.AddParameter("SendState", SqlDbType.Int, model.SendState);
                command.AddParameter("PayStatus", SqlDbType.Int, model.PayStatus);
                command.AddParameter("GameCoins", SqlDbType.Int, model.GameCoins);
                command.AddParameter("Signature", SqlDbType.VarChar, model.Signature);
            }
            else
            {
                command.AddParameter("ServerID", SqlDbType.Int, model.ServerID);
                command.AddParameter("PassportID", SqlDbType.VarChar, model.PassportID);
                command.AddParameter("GameID", SqlDbType.Int, model.GameID);
                command.AddParameter("RetailID", SqlDbType.VarChar, model.RetailID);//20
                //修改了服务器名称为空写库的BUG panx 2012-11-26
                if (!string.IsNullOrEmpty(model.ServerName))
                {
                    command.AddParameter("ServerName", SqlDbType.VarChar, model.ServerName);
                }
                //修改了游戏名称为空写库的BUG panx 2012-11-26
                if (!string.IsNullOrEmpty(model.GameName))
                {
                    command.AddParameter("gameName", SqlDbType.VarChar, model.GameName);
                }
            }
            command.Filter = new CommandFilter();
            command.Filter.Condition = "OrderNO=@OrderNO";
            command.Filter.AddParam("@OrderNO", SqlDbType.VarChar, 0, model.OrderNO);
            command.Parser();
            int rows = SqlHelper.ExecuteNonQuery(ConfigManger.connectionString, CommandType.Text, command.Sql, command.SqlParameters);
            return rows > 0;
        }

		/// <summary>
		/// Add91s the pay.
		/// </summary>
		/// <returns><c>true</c>, if pay was add91ed, <c>false</c> otherwise.</returns>
		/// <param name="order">Order.</param>
		/// <param name="callback">If set to <c>true</c> callback.</param>
        public bool Add91Pay(OrderInfo order, bool callback)
        {
            if (!IsExists(order.OrderNO))
            {
                return Add(order);
            }
            else
            {
                return UpdateBy91(order, callback);
            }
        }


        private OrderInfo SetOrderInfo(SqlDataReader reader)
        {
            OrderInfo ordermode = new OrderInfo();
            ordermode.OrderNO = reader["OrderNO"].ToNotNullString();
            ordermode.MerchandiseName = reader["MerchandiseName"].ToNotNullString();
            ordermode.PayType = reader["PayType"].ToNotNullString();
            ordermode.Amount = reader["Amount"].ToDecimal();
            ordermode.Currency = reader["Currency"].ToNotNullString();
            ordermode.Expand = reader["Expand"].ToNotNullString();
            ordermode.SerialNumber = reader["SerialNumber"].ToNotNullString();
            ordermode.PassportID = reader["PassportID"].ToNotNullString();
            ordermode.ServerID = reader["ServerID"].ToInt();
            ordermode.GameID = reader["GameID"].ToInt();
            ordermode.GameName = reader["gameName"].ToNotNullString();
            ordermode.ServerName = reader["ServerName"].ToNotNullString();
            ordermode.PayStatus = reader["PayStatus"].ToInt();
            ordermode.Signature = reader["Signature"].ToNotNullString();
            ordermode.Remarks = reader["Remarks"].ToNotNullString();
            ordermode.GameCoins = reader["GameCoins"].ToInt();
            ordermode.SendState = reader["SendState"].ToInt();
            ordermode.CreateDate = reader["CreateDate"].ToDateTime();
            ordermode.SendDate = reader["SendDate"].ToDateTime();
            ordermode.RetailID = reader["RetailID"].ToNotNullString();
            ordermode.DeviceID = reader["DeviceID"].ToNotNullString();
            return ordermode;
        }


        /// <summary>
        /// Updatestr the specified OrderNo.
        /// </summary>
        /// <param name="OrderNo">Order no.</param>
        public bool Updatestr(string OrderNo)
        {
            CommandStruct command = new CommandStruct("OrderInfo", CommandMode.Modify);
            command.AddParameter("SendState", 2);
            command.AddParameter("SendDate", SqlDbType.DateTime, DateTime.Now);
            command.Filter = new CommandFilter();
            command.Filter.Condition = "OrderNO=@OrderNO";
            command.Filter.AddParam("@OrderNO", SqlDbType.VarChar, 0, OrderNo);
            command.Parser();

            int rows = SqlHelper.ExecuteNonQuery(ConfigManger.connectionString, CommandType.Text, command.Sql, command.SqlParameters);
            {
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 支付取游戏名改为直接查库-wuzf
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="serverID"></param>
        /// <returns></returns>
        public ServerInfo GetServerData(int gameID, int serverID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT TOP 1 ID,GameID,(SELECT GameName FROM GameInfo WHERE GameInfo.GameID=ServerInfo.GameID) GameName,ServerName  FROM ServerInfo WHERE GameID=@GameID AND ID=@ServerID");
            SqlParameter[] parameters = {
                   	new SqlParameter("@GameID", SqlDbType.Int, 0),
                    new SqlParameter("@ServerID", SqlDbType.Int, 0)};
            parameters[0].Value = gameID;
            parameters[1].Value = serverID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(ConfigManger.connectionString, CommandType.Text, strSql.ToString(), parameters))
            {
                var serverInfo = new ServerInfo
                {
                    GameID = gameID,
                    ID = serverID,
                    Name = string.Empty,
                    GameName = string.Empty
                };
                if (reader.Read())
                {
                    serverInfo.GameName = reader["GameName"].ToNotNullString();
                    serverInfo.Name = reader["ServerName"].ToNotNullString();
                }
                return serverInfo;
            }
        }

        internal bool PaySuccess(string orderNo, OrderInfo orderInfo)
        {
            CommandStruct command = new CommandStruct("OrderInfo", CommandMode.Modify);
            orderInfo.PayStatus = 2;
            command.AddParameter("PayStatus", SqlDbType.Int, orderInfo.PayStatus);
            if (!string.IsNullOrEmpty(orderInfo.PayType))
            {
                command.AddParameter("PayType", SqlDbType.VarChar, orderInfo.PayType);
            }
            if (orderInfo.Amount > 0 && orderInfo.GameCoins > 0)
            {
                command.AddParameter("Amount", SqlDbType.Decimal, orderInfo.Amount);
                command.AddParameter("GameCoins", SqlDbType.Int, orderInfo.GameCoins);
            }
            command.Filter = new CommandFilter();
            command.Filter.Condition = "OrderNO=@OrderNO";
            command.Filter.AddParam("@OrderNO", SqlDbType.VarChar, 0, orderNo);
            command.Parser();

            int rows = SqlHelper.ExecuteNonQuery(ConfigManger.connectionString, CommandType.Text, command.Sql, command.SqlParameters);
            return rows > 0;
        }
    }
}